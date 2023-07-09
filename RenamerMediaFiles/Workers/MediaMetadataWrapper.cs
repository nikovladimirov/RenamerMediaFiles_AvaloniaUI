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
    public static IReadOnlyList<Directory> ReadMetadata(string fullName)
    {
        return ImageMetadataReader.ReadMetadata(fullName);
    }

    /// <summary>
    /// Calculate datetime from metadata
    /// </summary>
    public static DateTime? CalculateDateTime(IReadOnlyList<Directory> metadata, MetadataInfoModel metadataInfo,
        IReadOnlyCollection<MetaDateTimeExtension> metaExtensions)
    {
        var resultDateTime = ReadDateTime(metadata, metadataInfo);
        if (resultDateTime == null)
            return null;

        return ApplyMetadataExtensions(metadata, metaExtensions, resultDateTime.Value);
    }

    /// <summary>
    /// Apply metadata extensions
    /// </summary>
    public static DateTime ApplyMetadataExtensions(IReadOnlyList<Directory> metadata,
        IReadOnlyCollection<MetaDateTimeExtension> extensions, DateTime resultDateTime)
    {
        foreach (var extension in extensions)
        {
            var dictionaryExif = metadata.FirstOrDefault(x => string.Equals(x.Name, extension.AttributeName));
            if (dictionaryExif == null)
                continue;

            if (string.Equals(extension.ConditionEqual, MetaTypes.AttributeName))
            {
                resultDateTime = resultDateTime.AddHours(extension.OffsetHour);
                return resultDateTime;
            }

            var datetimeTag =
                dictionaryExif.Tags.FirstOrDefault(x => string.Equals(x.Name, extension.AttributeTag));
            if (datetimeTag == null)
                continue;

            if (string.Equals(extension.ConditionEqual, MetaTypes.AttributeTag))
            {
                resultDateTime = resultDateTime.AddHours(extension.OffsetHour);
                return resultDateTime;
            }

            if (!string.Equals(extension.ConditionEqual, MetaTypes.AttributeValue) ||
                !string.Equals(datetimeTag.Description, extension.AttributeValue))
                continue;

            resultDateTime = resultDateTime.AddHours(extension.OffsetHour);
            return resultDateTime;
        }

        return resultDateTime;
    }

    private static DateTime? ReadDateTime(IReadOnlyList<Directory> metadata, MetadataInfoModel metadataInfoModel)
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
}