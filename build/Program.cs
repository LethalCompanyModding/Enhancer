// Largely derived from https://github.com/BepInEx/BepInEx/blob/master/build/Program.cs

using System.Configuration;
using Build;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.Command;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Git;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;

return new CakeHost()
    .UseContext<BuildContext>()
    .Run(args);

public class BuildContext : FrostingContext
{
    public BuildContext(ICakeContext ctx) : base(ctx)
    {
        RootDirectory = ctx.Environment.WorkingDirectory.GetParent();
        DistributionDirectory = RootDirectory.Combine("dist");
        ThunderstoreConfigFile = RootDirectory.Combine("assets").CombineWithFilePath("thunderstore.toml");
        CurrentCommit = ctx.GitLogTip(RootDirectory);
        
        SolutionBuildProperties = Project.FromFile(
            RootDirectory.CombineWithFilePath("Directory.Build.props").FullPath,
            new ProjectOptions()
        );
        SolutionThunderstoreProperties = ThunderstoreProject.FromFile(ThunderstoreConfigFile.FullPath);
        
        BuildType = ctx.Argument("build-type", ProjectBuildType.Development);
        GitHubReleaseTagName = ctx.Argument<string>("github-tag-name", "");
    }
    
    public static void WriteToGithubOutput(string key, string value)
    {
        var gitHubOutputPath = System.Environment.GetEnvironmentVariable("GITHUB_OUTPUT");
        if (gitHubOutputPath is null) return;
        using StreamWriter fileStream = File.AppendText(gitHubOutputPath);
        fileStream.Write($"{key}={value}\n");
    }

    public enum ProjectBuildType
    {
        Release,
        Development,
    }
    
    public ProjectBuildType BuildType { get; }
    public string GitHubReleaseTagName { get; }
    
    public DirectoryPath RootDirectory { get; }
    public DirectoryPath DistributionDirectory { get; }
    public FilePath ThunderstoreConfigFile { get; }
    
    public Project SolutionBuildProperties { get; }
    public ThunderstoreProject SolutionThunderstoreProperties { get; }
    public string VersionPrefix => SolutionBuildProperties.GetPropertyValue("VersionPrefix");
    public GitCommit CurrentCommit { get; }
    
    public string VersionSuffix => BuildType switch
    {
        ProjectBuildType.Release      => "",
        ProjectBuildType.Development  => "dev",
        _                             => throw new ArgumentOutOfRangeException()
    };
    
    public string BuildPackageVersion => VersionPrefix + BuildType switch
    {
        ProjectBuildType.Release => "",
        _                        => $"-{VersionSuffix}+{this.GitShortenSha(RootDirectory, CurrentCommit)}",
    };
}

[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext ctx)
    {
        ctx.CreateDirectory(ctx.DistributionDirectory);
        ctx.CleanDirectory(ctx.DistributionDirectory);

        ctx.Log.Information("Cleaning up old build objects");
        ctx.CleanDirectories(ctx.RootDirectory.Combine("**/Enhancer/**/bin").FullPath);
        ctx.CleanDirectories(ctx.RootDirectory.Combine("**/Enhancer/**/obj").FullPath);
    }
}

[TaskName("PatchBuildProps")]
public sealed class PatchBuildPropsTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext ctx) => 
        !string.IsNullOrWhiteSpace(ctx.GitHubReleaseTagName) && ctx.GitHubReleaseTagName != ctx.VersionPrefix;

    public override void Run(BuildContext ctx)
    {
        ctx.SolutionBuildProperties.SetProperty("VersionPrefix", ctx.GitHubReleaseTagName);
        ctx.SolutionBuildProperties.Save();
    }
}

[TaskName("Compile")]
[IsDependentOn(typeof(CleanTask))]
[IsDependentOn(typeof(PatchBuildPropsTask))]
public sealed class CompileTask : FrostingTask<BuildContext>
{
    public DotNetBuildSettings DetermineBuildSettings(BuildContext ctx)
    {
        var buildSettings = new DotNetBuildSettings
        {
            Configuration = "Release"
        };
        ConfigureDevelopmentMetadata(ctx, buildSettings);
        return buildSettings;
    }

    public void ConfigureDevelopmentMetadata(BuildContext ctx, DotNetBuildSettings buildSettings)
    {
        if (ctx.BuildType == BuildContext.ProjectBuildType.Release) return;
        
        buildSettings.MSBuildSettings = new DotNetMSBuildSettings
        {
            VersionSuffix = ctx.VersionSuffix,
            Properties =
            {
                ["SourceRevisionId"] = new[] { ctx.CurrentCommit.Sha },
                ["RepositoryBranch"] = new[] { ctx.GitBranchCurrent(ctx.RootDirectory).FriendlyName }
            }
        };
    }

    public override void Run(BuildContext ctx)
    {
        ctx.DotNetBuild(ctx.RootDirectory.FullPath, DetermineBuildSettings(ctx));
    }
}

[TaskName("PatchThunderstoreMetadata")]
public sealed class PatchThunderstoreMetadataTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext ctx) =>
        ctx.SolutionThunderstoreProperties.Package?.VersionNumber != ctx.VersionPrefix;

    public override void Run(BuildContext ctx)
    {
        if (ctx.SolutionThunderstoreProperties.Package is null) throw new ConfigurationErrorsException();
        ctx.SolutionThunderstoreProperties.Package!.VersionNumber = ctx.VersionPrefix;
        ctx.SolutionThunderstoreProperties.ToFile(ctx.ThunderstoreConfigFile);
    }
}

[TaskName("BuildThunderstore")]
[IsDependentOn(typeof(CompileTask))]
[IsDependentOn(typeof(PatchThunderstoreMetadataTask))]
public sealed class BuildThunderstoreTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext ctx) => !ctx.FileExists(ctx.DistributionDirectory.CombineWithFilePath(
        ctx.SolutionThunderstoreProperties.GetBuildArchiveFilename(ctx.BuildPackageVersion)));

    public override void Run(BuildContext ctx)
    {
        ctx.CreateDirectory(ctx.DistributionDirectory);
        ctx.CleanDirectory(ctx.DistributionDirectory);
        
        ctx.Command(
            new []{ "tcli", "tcli.exe" },
            new ProcessArgumentBuilder()
                .Append("build")
                .AppendSwitch("--config-path", ctx.ThunderstoreConfigFile.FullPath),
            expectedExitCode: 0
        );

        RenameBuildArchive(ctx);
    }
    
    public void RenameBuildArchive(BuildContext ctx)
    {
        var oldBuildArchiveName = ctx.SolutionThunderstoreProperties.GetBuildArchiveFilename();
        var oldBuildArchivePath = ctx.DistributionDirectory
            .CombineWithFilePath(oldBuildArchiveName);

        var newBuildArchiveName = ctx.SolutionThunderstoreProperties.GetBuildArchiveFilename(ctx.BuildPackageVersion);
        var newBuildArchivePath = ctx.DistributionDirectory
            .CombineWithFilePath(newBuildArchiveName);

        ctx.MoveFile(oldBuildArchivePath, newBuildArchivePath);
        BuildContext.WriteToGithubOutput("THUNDERSTORE_PACKAGE_PATH", ctx.RootDirectory.GetRelativePath(newBuildArchivePath).FullPath);
        BuildContext.WriteToGithubOutput("THUNDERSTORE_PACKAGE_NAME", newBuildArchiveName);
    }
}

[TaskName("PublishThunderstore")]
[IsDependentOn(typeof(BuildThunderstoreTask))]
public sealed class PublishTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext ctx)
    {
        var newBuildArchiveName = ctx.SolutionThunderstoreProperties.GetBuildArchiveFilename(ctx.BuildPackageVersion);
        var newBuildArchivePath = ctx.DistributionDirectory
            .CombineWithFilePath(newBuildArchiveName);
        
        ctx.Command(
            new []{ "tcli", "tcli.exe" },
            new ProcessArgumentBuilder()
                .Append("publish")
                .AppendSwitch("--config-path", ctx.ThunderstoreConfigFile.FullPath)
                .AppendSwitch("--file", newBuildArchivePath.FullPath),
            expectedExitCode: 0
        );
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(CompileTask))]
public class DefaultTask : FrostingTask { }