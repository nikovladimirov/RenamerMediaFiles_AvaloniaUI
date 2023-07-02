namespace RenamerMediaFiles.Models;

public class MetadataInfoModel : ModelBase
{
    private string _caption;
    private string _datetimeMask;
    private string _attributeTag;
    private string _attributeName;

    public MetadataInfoModel()
    {
        _caption = "New meta info";
    }
    
    public MetadataInfoModel(string caption, string attributeName, string attributeTag, string datetimeMask)
    {
        _caption = caption;
        _attributeName = attributeName;
        _attributeTag = attributeTag;
        _datetimeMask = datetimeMask;
    }

    public string Caption
    {
        get => _caption;
        set => SetProperty(ref _caption, value);
    }
    
    public string AttributeName 
    {
        get => _attributeName;
        set => SetProperty(ref _attributeName, value);
    }
    
    public string AttributeTag 
    {
        get => _attributeTag;
        set => SetProperty(ref _attributeTag, value);
    }
    
    public string DatetimeMask 
    {
        get => _datetimeMask;
        set => SetProperty(ref _datetimeMask, value);
    }
}