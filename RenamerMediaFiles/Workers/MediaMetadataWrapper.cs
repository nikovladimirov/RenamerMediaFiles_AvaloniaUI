using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetadataExtractor;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.Workers;

public static class MediaMetadataWrapper
{
    /// <summary>
    /// Read metadata from file
    /// </summary>
    public static IReadOnlyList<Directory> ReadMetadataDirectories(string fullName)
    {
        return ImageMetadataReader.ReadMetadata(fullName);
    }
    
    /// <summary>
    /// Apply metadata extensions
    /// </summary>
    public static DateTime ApplyMetadataExtensions(IReadOnlyList<Directory> metadata,
        IReadOnlyCollection<MetaDateTimeExtension> extensions, DateTime resultDateTime)
    {
        foreach (var extension in extensions)
        {
            var dictionaryExif = metadata.FirstOrDefault(x => string.Equals(x.Name.Trim(), extension.DirectoryName));
            if (dictionaryExif == null)
                continue;

            if (string.Equals(extension.ConditionEqual, MetadataTypes.DirectoryName))
            {
                resultDateTime = resultDateTime.AddHours(extension.OffsetHour);
                return resultDateTime;
            }

            var datetimeTag =
                dictionaryExif.Tags.FirstOrDefault(x => string.Equals(x.Name.Trim(), extension.TagName));
            if (datetimeTag == null)
                continue;

            if (string.Equals(extension.ConditionEqual, MetadataTypes.TagName))
            {
                resultDateTime = resultDateTime.AddHours(extension.OffsetHour);
                return resultDateTime;
            }

            if (!string.Equals(extension.ConditionEqual, MetadataTypes.TagDescription) ||
                !string.Equals(datetimeTag.Description?.Trim(), extension.TagDescription))
                continue;

            resultDateTime = resultDateTime.AddHours(extension.OffsetHour);
            return resultDateTime;
        }

        return resultDateTime;
    }

    public static DateTime? ReadDateTime(IReadOnlyList<Directory> metadata, MetadataInfoModel metadataInfoModel)
    {
        var dictionaryExif = metadata.FirstOrDefault(x => string.Equals(x.Name, metadataInfoModel.AttributeName));
        if (dictionaryExif == null)
            return null;

        var datetimeTag =
            dictionaryExif.Tags.FirstOrDefault(x => string.Equals(x.Name, metadataInfoModel.AttributeTag));
        if (datetimeTag == null)
            return null;

        if (!DateTime.TryParseExact(datetimeTag.Description, metadataInfoModel.DatetimeMask,
                CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime resultDateTime))
            return null;

        if (resultDateTime.Kind == DateTimeKind.Local)
            resultDateTime = resultDateTime.ToUniversalTime();

        if (resultDateTime.Year < 1990 || resultDateTime.Year > DateTime.Now.Year)
            throw new Exception($"{metadataInfoModel.Caption}. Incorrect datetime {resultDateTime}");

        return resultDateTime;
    }

    public static List<MetadataInfo.DirectoryItem> GetMetadataInfo(string filePath)
    {
        var directories = ReadMetadataDirectories(filePath);
        
        var list = new List<MetadataInfo.DirectoryItem>();
        foreach (var directory in directories)
        {
            var metadataItemJson = new MetadataInfo.DirectoryItem(directory.Name);
            foreach (var tagItem in directory.Tags)
                metadataItemJson.Tags.Add(new MetadataInfo.TagItem(tagItem.Name, tagItem.Description));
                
            list.Add(metadataItemJson);
        }

        return list;
    }
}