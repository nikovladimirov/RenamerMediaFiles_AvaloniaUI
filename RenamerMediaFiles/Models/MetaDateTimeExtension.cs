namespace RenamerMediaFiles.Models;

public class MetaDateTimeExtension : ModelBase
{
    private string _caption;
    private string _conditionEqual;
    private string _metadataName;
    private string _tagName;
    private string _tagDescription;
    private float _offsetHour;

    public MetaDateTimeExtension()
    {
        _caption = "New extension";
    }
    
    public MetaDateTimeExtension(string caption, string conditionEqual, string metadataName, string tagName, string tagDescription, float offsetHour)
    {
        _caption = caption;
        _conditionEqual = conditionEqual;
        _metadataName = metadataName;
        _tagName = tagName;
        _tagDescription = tagDescription;
        _offsetHour = offsetHour;
    }
    
    public MetaDateTimeExtension(string caption, string conditionEqual, string metadataName, float offsetHour)
    {
        _caption = caption;
        _conditionEqual = conditionEqual;
        _metadataName = metadataName;
        _offsetHour = offsetHour;
    }

    public string Caption
    {
        get => _caption;
        set => SetProperty(ref _caption, value);
    }
    
    public string ConditionEqual
    {
        get => _conditionEqual;
        set => SetProperty(ref _conditionEqual, value);
    }
    
    public string MetadataName 
    {
        get => _metadataName;
        set => SetProperty(ref _metadataName, value);
    }
    
    public string TagName 
    {
        get => _tagName;
        set => SetProperty(ref _tagName, value);
    }
    
    public string TagDescription
    {
        get => _tagDescription;
        set => SetProperty(ref _tagDescription, value);
    }
    
    public float OffsetHour
    {
        get => _offsetHour;
        set => SetProperty(ref _offsetHour, value);
    }
}