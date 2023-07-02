using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MetadataExtractor;
using Microsoft.Extensions.DependencyInjection;
using RenamerMediaFiles.Services.Interfaces;
using Directory = MetadataExtractor.Directory;

namespace RenamerMediaFiles.Models
{
    public class FileItemModel
    {
        private ISimpleDialogService? _dialogService;
        private readonly bool _changeNameByMasks;
        private string _additionalName;
        private SettingsModel? _settingsModel;

        ISimpleDialogService? DialogService =>
            _dialogService ??= App.Current.Services.GetService<ISimpleDialogService>();

        SettingsModel? SettingsModel =>
            _settingsModel ??= App.Current.Services.GetService<SettingsModel>();

        public string NewNameFormat { get; private set; }
        public string FilePathDisplayValue { get; private set; }
        public string FilePath { get; private set; }
        public string OriginalFileName { get; private set; }
        public string Extension { get; private set; }
        public List<MetadataItemModel> MetaDataItems { get; set; } = new List<MetadataItemModel>();
        public FileInfo FileInfo { get; private set; }
        public string UsedMask { get; private set; }

        public string Exception { get; set; }

        public FileItemModel(FileInfo fileInfo, string rootPath, string newNameFormat, bool changeNameByMasks,
            IEnumerable<StringModel> removingByMasks)
        {
            NewNameFormat = newNameFormat;
            _changeNameByMasks = changeNameByMasks;

            try
            {
                RefreshFileInfo(fileInfo, rootPath, removingByMasks);
                ReadAllMetadata();
                SelectSingleMetadata();
            }
            catch (Exception ex)
            {
                Exception = $"ReadMetaData. {ex.Message}";
            }
        }

        public void ShowFileInFolder(string path)
        {
            DialogService?.ShowFileInFolder(path);
        }

        private void RefreshFileInfo(FileInfo fileInfo, string rootPath, IEnumerable<StringModel> removingNameParts)
        {
            FileInfo = fileInfo;
            OriginalFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            Extension = fileInfo.Extension;
            FilePath = fileInfo.Directory.FullName;

            FilePathDisplayValue = FilePath.Replace(rootPath, string.Empty);
            if (string.IsNullOrEmpty(FilePathDisplayValue))
                FilePathDisplayValue = @"\";

            UsedMask = null;
            if (_changeNameByMasks)
            {
                _additionalName = OriginalFileName;
                foreach (var mask in removingNameParts)
                {
                    if (Regex.Match(_additionalName, mask.Value).Success)
                    {
                        UsedMask = mask.Value;
                        _additionalName = Regex.Replace(_additionalName, mask.Value, string.Empty);
                        break;
                    }
                }
            }
        }

        private void ReadAllMetadata()
        {
            if (SettingsModel == null)
                return;
            
            var metadata = ImageMetadataReader.ReadMetadata(FileInfo.FullName);
            foreach (var metadataInfo in SettingsModel.MetadataInfos)
            {
                var resultDateTime = ReadMetadata(metadata, metadataInfo, SettingsModel.MetaDateTimeExtensions);
                if (resultDateTime == null)
                    continue;
                
                var existResultDateTime = MetaDataItems.FirstOrDefault(x => Math.Abs((x.SourceDateTime - resultDateTime.Value).TotalMinutes) < 1);

                if (existResultDateTime == null)
                    MetaDataItems.Add(new MetadataItemModel(resultDateTime.Value, metadataInfo.Caption, _changeNameByMasks,
                        NewNameFormat, _additionalName));
                else
                    existResultDateTime.AddDateSource(metadataInfo.Caption);
            }
        }

        private void SelectSingleMetadata()
        {
            var singleMetaData = MetaDataItems.OrderBy(x => x.DateSources.FirstOrDefault())
                .ThenBy(x => x.SourceDateTime).FirstOrDefault();
            if (singleMetaData == null)
                return;

            var index = 2;
            var destinationPath = Path.Combine(FilePath, $"{singleMetaData.NewFileName}{Extension}");
            if (IsCurrentName(destinationPath))
                return;

            var newName = singleMetaData.NewFileName;
            while (File.Exists(destinationPath))
            {
                newName = $"{singleMetaData.NewFileName} ({index++})";
                destinationPath = Path.Combine(FilePath, $"{newName}{Extension}");

                if (IsCurrentName(destinationPath))
                    break;
            }

            singleMetaData.NewFileName = newName;

            if (!IsCurrentName(destinationPath))
            {
                singleMetaData.Selected = true;
            }
        }

        private bool IsCurrentName(string destinationPath)
        {
            return string.Equals(FileInfo.FullName, destinationPath, StringComparison.OrdinalIgnoreCase);
        }

        public static DateTime? ReadMetadata(IReadOnlyList<Directory> metadata, MetadataInfoModel metadataInfo,
            IReadOnlyCollection<MetaDateTimeExtension> metaExtensions)
        {
            var resultDateTime = GetDateTime(metadata, metadataInfo);
            if (resultDateTime == null)
                return null;

            return UpdateDateTimeByExtensions(metadata, metaExtensions, resultDateTime.Value);
        }

        public static DateTime UpdateDateTimeByExtensions(IReadOnlyList<Directory> metadata,
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

                if (string.Equals(extension.ConditionEqual, MetaTypes.AttributeValue))
                {
                    if (string.Equals(datetimeTag.Description, extension.AttributeValue))
                    {
                        resultDateTime = resultDateTime.AddHours(extension.OffsetHour);
                        return resultDateTime;
                    }
                }
            }

            return resultDateTime;
        }
        
        public static DateTime? GetDateTime(IReadOnlyList<Directory> metadata, MetadataInfoModel metadataInfoModel)
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
}