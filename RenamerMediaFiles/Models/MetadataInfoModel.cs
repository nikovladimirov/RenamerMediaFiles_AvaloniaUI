namespace RenamerMediaFiles.Models;

public class MetadataInfoModel : ModelBase
{
    private string _caption;
    private float _offsetHour;
    private string _datetimeMask;
    private string _attributeTag;
    private string _attributeName;

    public MetadataInfoModel()
    {
        
    }
    
    public MetadataInfoModel(string caption, string attributeName, string attributeTag, string datetimeMask, float offsetHour)
    {
        _caption = caption;
        _attributeName = attributeName;
        _attributeTag = attributeTag;
        _datetimeMask = datetimeMask;
        _offsetHour = offsetHour;
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
    public float OffsetHour 
    {
        get => _offsetHour;
        set => SetProperty(ref _offsetHour, value);
    }
}