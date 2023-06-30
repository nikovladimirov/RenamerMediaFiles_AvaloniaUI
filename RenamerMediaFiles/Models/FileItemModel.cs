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
        private readonly bool _replaceFullName;
        private string _additionalName;

        ISimpleDialogService? DialogService => _dialogService ??= App.Current.Services.GetService<ISimpleDialogService>();

        public string NewNameFormat { get; private set; }
        public string FilePathDisplayValue { get; private set; }
        public string FilePath { get; private set; }
        public string OriginalFileName { get; private set; }
        public string Extension { get; private set; }
        public List<MetadataItemModel> MetaDataItems { get; set; } = new List<MetadataItemModel>();
        public FileInfo FileInfo { get; private set; }
        public string UsedMask { get; private set; }
        
        public string Exception { get; set; }
        
        public FileItemModel(FileInfo fileInfo, string rootPath, string newNameFormat, bool replaceFullName, IEnumerable<StringModel> removingByMasks)
        {
            NewNameFormat = newNameFormat;
            _replaceFullName = replaceFullName;

            try
            {
                RefreshFileInfo(fileInfo, rootPath, removingByMasks);
                ReadMetadata();
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
            if (!_replaceFullName)
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

        private void ReadMetadata()
        {
            var metadata = ImageMetadataReader.ReadMetadata(FileInfo.FullName);
            foreach (var keyValue in DefaultSettings.DefaultMetadataInfos)
                ReadAttribute(metadata, keyValue);
        }
        
        private void SelectSingleMetadata()
        {
            var singleMetaData = MetaDataItems.OrderBy(x=>x.DateSources.FirstOrDefault()).ThenBy(x => x.SourceDateTime).FirstOrDefault();
            if(singleMetaData == null)
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
        
        private void ReadAttribute(IReadOnlyList<Directory> metadata, KeyValuePair<string, MetadataInfo> dateSource)
        {
            var resultDateTime = GetDateTime(metadata, dateSource.Key, dateSource.Value, dateSource.Value.OffsetHour);
            
            if(resultDateTime == null)
                return;
            
            var existResultDateTime =
                MetaDataItems.FirstOrDefault(x => Math.Abs((x.SourceDateTime - resultDateTime.Value).TotalMinutes) < 1);

            if (existResultDateTime == null)
                MetaDataItems.Add(new MetadataItemModel(resultDateTime.Value, dateSource.Key, _replaceFullName, NewNameFormat, _additionalName));
            else
                existResultDateTime.AddDateSource(dateSource.Key);
        }

        public static DateTime? GetDateTime(IReadOnlyList<Directory> metadata, string dateSource, MetadataInfo metadataInfo,
            float timeZoneOffset)
        {

            var dictionaryExif = metadata.FirstOrDefault(x => string.Equals(x.Name, metadataInfo.AttributeName));
            if (dictionaryExif == null)
                return null;

            var datetimeTag = dictionaryExif.Tags.FirstOrDefault(x => string.Equals(x.Name, metadataInfo.AttributeTag));
            if (datetimeTag == null)
                return null;

            if (!DateTime.TryParseExact(datetimeTag.Description, metadataInfo.TagMask,
                    CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime resultDateTime))
                return null;
            
            if(resultDateTime.Kind== DateTimeKind.Local)
                resultDateTime = resultDateTime.ToUniversalTime();
            
            resultDateTime = resultDateTime.AddHours(timeZoneOffset);

            if (resultDateTime.Year < 1990 || resultDateTime.Year > DateTime.Now.Year)
                throw new Exception($"{dateSource}. Incorrect datetime {resultDateTime.ToString()}");

            return resultDateTime;
        }
    }
}