// Largely derived from https://github.com/BepInEx/BepInEx/blob/master/build/Program.cs

using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
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
        BuildType = ctx.Argument("build-type", ProjectBuildType.Development);
        var props = Project.FromFile(RootDirectory.CombineWithFilePath("Directory.Build.props").FullPath, new ProjectOptions());
        VersionPrefix = props.GetPropertyValue("VersionPrefix");
        CurrentCommit = ctx.GitLogTip(RootDirectory);
    }

    public enum ProjectBuildType
    {
        Release,
        Development,
    }
    
    public ProjectBuildType BuildType { get; }
    
    public DirectoryPath RootDirectory { get; }
    public DirectoryPath DistributionDirectory { get; }
    
    public string VersionPrefix { get; }
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

    public string TargetZipName(string targetName) => $"AugmentedEnhancer-{BuildPackageVersion}-{targetName}.zip";
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

[TaskName("Compile")]
[IsDependentOn(typeof(CleanTask))]
public sealed class CompileTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext ctx)
    {
        var buildSettings = new DotNetBuildSettings
        {
            Configuration = "Release"
        };
        
        if (ctx.BuildType != BuildContext.ProjectBuildType.Release)
        {
            buildSettings.MSBuildSettings = new()
            {
                VersionSuffix = ctx.VersionSuffix,
                Properties =
                {
                    ["SourceRevisionId"] = new[] { ctx.CurrentCommit.Sha },
                    ["RepositoryBranch"] = new[] { ctx.GitBranchCurrent(ctx.RootDirectory).FriendlyName }
                }
            };
        }

        ctx.DotNetBuild(ctx.RootDirectory.FullPath, buildSettings);
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(CompileTask))]
public class DefaultTask : FrostingTask { }