namespace RenamerMediaFiles.Models;

public class MetadataInfo
{
    public MetadataInfo(string dateSource, string attributeName, string attributeTag, string tagMask)
    {
        DateSource = dateSource;
        AttributeName = attributeName;
        AttributeTag = attributeTag;
        TagMask = tagMask;
    }

    public string DateSource { get; set; }
    public string AttributeName { get; set; }
    public string AttributeTag { get; set; }
    public string TagMask { get; set; }
    public float OffsetHour { get; set; } = 3;
}