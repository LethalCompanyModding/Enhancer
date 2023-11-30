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
using HandlebarsDotNet;
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

[TaskName("PreMake")]
[IsDependentOn(typeof(CompileTask))]
public sealed class PreMakeTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext ctx)
    {
        ctx.CreateDirectory(ctx.DistributionDirectory);
        ctx.CleanDirectory(ctx.DistributionDirectory);
    }
}

public abstract class MakeTask : FrostingTask<BuildContext>
{
    public abstract string DistributionName { get; }
    
    public override void Run(BuildContext ctx)
    {
        ctx.Log.Information($"Preparing {DistributionName} distribution");
        var targetDirectory = ctx.DistributionDirectory.Combine(DistributionName);
        ctx.CreateDirectory(targetDirectory);
        ctx.CleanDirectory(targetDirectory);

        PreparePackage(ctx, targetDirectory);
        
        CollectFiles(ctx, targetDirectory);
        CompressPackage(ctx, targetDirectory);
    }

    public virtual void PreparePackage(BuildContext ctx, DirectoryPath targetDirectory) { }
    
    public void CollectFiles(BuildContext ctx, DirectoryPath copyToPath)
    {
        var filesToCopy = CoreFilesToCollect(ctx).Concat(FilesToCollect(ctx));
        ctx.CopyFiles(filesToCopy , copyToPath);
    }
    
    public IEnumerable<FilePath> CoreFilesToCollect(BuildContext ctx)
    {
        yield return ctx.RootDirectory.CombineWithFilePath("README.md");
        yield return ctx.RootDirectory.Combine("assets").Combine("icons").CombineWithFilePath("icon.png");
        yield return ctx.RootDirectory.Combine("Enhancer").Combine("bin").CombineWithFilePath("Enhancer.dll");
    }

    public virtual IEnumerable<FilePath> FilesToCollect(BuildContext ctx) { yield break; }

    public void CompressPackage(BuildContext ctx, DirectoryPath packagePath)
    {
        var targetZipName = ctx.TargetZipName(DistributionName); 
        ctx.Log.Information($"Packing {targetZipName}");
        ctx.Zip(packagePath, ctx.DistributionDirectory.CombineWithFilePath(targetZipName));
    }
}

[TaskName("MakeThunderstore")]
[IsDependentOn(typeof(PreMakeTask))]
public sealed class MakeThunderstoreTask : MakeTask
{
    public override string DistributionName => "thunderstore";

    public override void PreparePackage(BuildContext ctx, DirectoryPath targetDirectory)
    {
        WriteManifest(ctx, targetDirectory.CombineWithFilePath("manifest.json"));
    }

    public void WriteManifest(BuildContext ctx, FilePath writeToPath)
    {
        HandlebarsTemplate<TextWriter, object, object> manifestTemplate;
        
        using (StreamReader templateFileReader = File.OpenText(ctx.RootDirectory.Combine("assets").CombineWithFilePath("manifest.json.handlebars").FullPath))
        {
            manifestTemplate = Handlebars.Compile(templateFileReader);
        }
        
        using (StreamWriter manifestFileWriter = File.CreateText(writeToPath.FullPath))
        {
            manifestTemplate(manifestFileWriter, new { context = ctx });
        }
    }
}

[TaskName("Publish")]
[IsDependentOn(typeof(MakeThunderstoreTask))]
public class PublishTask : FrostingTask { }

[TaskName("Default")]
[IsDependentOn(typeof(CompileTask))]
public class DefaultTask : FrostingTask { }