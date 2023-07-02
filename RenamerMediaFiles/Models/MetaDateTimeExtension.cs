using System;

namespace RenamerMediaFiles.Models;

public class MetaDateTimeExtension : ModelBase
{
    private string _caption;
    private string _conditionEqual;
    private string _attributeName;
    private string _attributeTag;
    private string _attributeValue;
    private float _offsetHour;

    public MetaDateTimeExtension()
    {
        _caption = "New extension";
    }
    
    public MetaDateTimeExtension(string caption, string conditionEqual, string attributeName, string attributeTag, string attributeValue, float offsetHour)
    {
        _caption = caption;
        _conditionEqual = conditionEqual;
        _attributeName = attributeName;
        _attributeTag = attributeTag;
        _attributeValue = attributeValue;
        _offsetHour = offsetHour;
    }
    
    public MetaDateTimeExtension(string caption, string conditionEqual, string attributeName, float offsetHour)
    {
        _caption = caption;
        _conditionEqual = conditionEqual;
        _attributeName = attributeName;
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
    
    public string AttributeValue
    {
        get => _attributeValue;
        set => SetProperty(ref _attributeValue, value);
    }
    
    public float OffsetHour
    {
        get => _offsetHour;
        set => SetProperty(ref _offsetHour, value);
    }
}