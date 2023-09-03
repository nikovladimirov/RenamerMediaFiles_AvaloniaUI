namespace RenamerMediaFiles.Models;

public class MetadataTypes
{
    public static readonly string[] AllItems = new[] { DirectoryName, TagName, TagDescription };

    public const string DirectoryName = "DirectoryName";
    public const string TagName = "TagName";
    public const string TagDescription = "TagDescription";
}