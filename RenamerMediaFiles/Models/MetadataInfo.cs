using System.Collections.Generic;

namespace RenamerMediaFiles.Models;

public class MetadataInfo
{
    public class DirectoryItem
    {
        public DirectoryItem(string directoryName)
        {
            DirectoryName = directoryName;
            Tags = new List<TagItem>();
        }
        
        public string DirectoryName { get; set; }
        public List<TagItem> Tags { get; set; }
    }

    public class TagItem
    {
        public TagItem(string tagItemName, string? tagItemDescription)
        {
            TagName = tagItemName;
            TagDescription = tagItemDescription;
        }

        public string TagName { get; set; }
        public string? TagDescription { get; set; }
    }
}