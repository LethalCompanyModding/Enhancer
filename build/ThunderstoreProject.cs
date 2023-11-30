using System.Configuration;
using System.Runtime.Serialization;
using Cake.Core.IO;
using Tomlyn;
using Tomlyn.Model;

namespace Build;

public class TomlDictionary<TKey, TValue>: Dictionary<TKey, TValue>, ITomlMetadataProvider where TKey: notnull
{
    // storage for comments and whitespace
    TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }
}

public class ThunderstoreProject : ITomlMetadataProvider
{
    public static TomlModelOptions ThunderstoreProjectTomlOptions = new()
    {
        ConvertPropertyName = s => s,
        ConvertFieldName = s => s,
    };
    
    public static ThunderstoreProject FromFile(FilePath filePath)
    {
        using StreamReader thunderstoreConfigReader = File.OpenText(filePath.FullPath);
        return Toml.ToModel<ThunderstoreProject>(
            thunderstoreConfigReader.ReadToEnd(), 
            options: ThunderstoreProjectTomlOptions
        );
    }

    public void ToFile(FilePath filePath)
    {
        using StreamWriter thunderstoreConfigReader = File.CreateText(filePath.FullPath);
        thunderstoreConfigReader.Write(Toml.FromModel(this, options: ThunderstoreProjectTomlOptions));
    }
    
    public string GetPackageId(string? versionNumber) => Package is null 
        ? throw new ConfigurationErrorsException("Missing package metadata.") 
        : $"{Package.Namespace}-{Package.Name}-{versionNumber}";

    public string GetPackageId() => Package is null 
        ? throw new ConfigurationErrorsException("Missing package metadata.") 
        : GetPackageId(Package.VersionNumber);

    public string GetBuildArchiveFilename(string? versionNumber) => $"{GetPackageId(versionNumber)}.zip";
    
    public string GetBuildArchiveFilename() => $"{GetPackageId()}.zip";
    
    [DataMember(Name = "config")]
    public ConfigData? Config { get; set; }
    
    [DataMember(Name = "general")]
    public GeneralData? General { get; set; }
    
    [DataMember(Name = "package")]
    public PackageData? Package { get; set; }
    
    [DataMember(Name = "build")]
    public BuildData? Build { get; set; }
    
    [DataMember(Name = "publish")]
    public PublishData? Publish { get; set; }
    
    // storage for comments and whitespace
    TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }
}

public class ConfigData : ITomlMetadataProvider
{
    [DataMember(Name = "schemaVersion")]
    public string? SchemaVersion { get; set; }
    
    // storage for comments and whitespace
    TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }
}

public class GeneralData : ITomlMetadataProvider
{
    [DataMember(Name = "repository")]
    public string? Repository { get; set; }
    
    // storage for comments and whitespace
    TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }
}

public class PackageData : ITomlMetadataProvider
{
    [DataMember(Name = "namespace")]
    public string? Namespace { get; set; }
    
    [DataMember(Name = "name")]
    public string? Name { get; set; }
    
    [DataMember(Name = "versionNumber")]
    public string? VersionNumber { get; set; }
    
    [DataMember(Name = "description")]
    public string? DescriptionUrl { get; set; }
    
    [DataMember(Name = "websiteUrl")]
    public string? WebsiteUrl { get; set; }
    
    [DataMember(Name = "containsNsfwContent")]
    public bool? ContainsNsfwContent { get; set; }

    [DataMember(Name = "dependencies")] 
    public TomlDictionary<string, string> Dependencies { get; } = new();
    
    // storage for comments and whitespace
    TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }
}

public class BuildData : ITomlMetadataProvider
{
    [DataMember(Name = "icon")]
    public string? Icon { get; set; }
    
    [DataMember(Name = "readme")]
    public string? Readme { get; set; }
    
    [DataMember(Name = "outdir")]
    public string? OutDir { get; set; }

    [DataMember(Name = "copy")] 
    public List<CopyPath> CopyPaths { get; } = new();
    
    // storage for comments and whitespace
    TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }
}

public class CopyPath : ITomlMetadataProvider
{
    [DataMember(Name = "source")] 
    public string? Source { get; set; }
    
    [DataMember(Name = "target")] 
    public string? Target { get; set; }
    
    // storage for comments and whitespace
    TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }
}

public class PublishData : ITomlMetadataProvider
{
    [DataMember(Name = "communities")] 
    public List<string> Communities { get; } = new();
    
    [DataMember(Name = "categories")] 
    public List<string> Categories { get; } = new();
    
    // storage for comments and whitespace
    TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }
}