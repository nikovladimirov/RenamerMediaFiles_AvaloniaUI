namespace RenamerMediaFiles.Models;

public class MetaDateTimeExtension : ModelBase
{
    private string _caption;
    private string _conditionEqual;
    private string _directoryName;
    private string _tagName;
    private string _tagDescription;
    private float _offsetHour;

    public MetaDateTimeExtension()
    {
        _caption = "New extension";
    }
    
    public MetaDateTimeExtension(string caption, string conditionEqual, string directoryName, string tagName, string tagDescription, float offsetHour)
    {
        _caption = caption;
        _conditionEqual = conditionEqual;
        _directoryName = directoryName;
        _tagName = tagName;
        _tagDescription = tagDescription;
        _offsetHour = offsetHour;
    }
    
    public MetaDateTimeExtension(string caption, string conditionEqual, string directoryName, float offsetHour)
    {
        _caption = caption;
        _conditionEqual = conditionEqual;
        _directoryName = directoryName;
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
    
    public string DirectoryName 
    {
        get => _directoryName;
        set => SetProperty(ref _directoryName, value);
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