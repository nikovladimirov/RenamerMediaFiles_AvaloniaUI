using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RenamerMediaFiles.Services.Interfaces;
using RenamerMediaFiles.Workers;

namespace RenamerMediaFiles.Models
{
    public class FileItemModel
    {
        private ISimpleDialogService _simpleDialogService;
        private SettingsModel _settingsModel;

        public string FilePathDisplayValue { get; private set; }
        public string FilePath { get; private set; }
        public string OriginalFileName { get; private set; }
        public string Extension { get; private set; }
        public List<MetadataItemModel> MetaDataItems { get; set; } = new List<MetadataItemModel>();
        public FileInfo FileInfo { get; private set; }
        public string UsedMask { get; private set; }

        public string Exception { get; set; }

        public FileItemModel(SettingsModel settingsModel, ISimpleDialogService simpleDialogService)
        {
            _simpleDialogService = simpleDialogService;
            _settingsModel = settingsModel;
        }

        public void Init(FileInfo fileInfo, string rootPath, string newNameFormat, bool addAdditionalName, IEnumerable<StringModel> removingByMasks)
        {
            try
            {
                RefreshFileInfo(fileInfo, rootPath);
                var additionalName = addAdditionalName ? GetAdditionalName(removingByMasks) : null;
                ReadAllMetadata(newNameFormat, additionalName);
                SelectSingleMetadata();
            }
            catch (Exception ex)
            {
                Exception = $"ReadMetaData. {ex.Message}";
            }
        }

        public void ShowFileInFolder(string path)
        {
            _simpleDialogService.ShowFileInFolder(path);
        }

        internal string GetAdditionalName(IEnumerable<StringModel> removingNameParts)
        {
            var result = OriginalFileName;
            foreach (var mask in removingNameParts)
            {
                if (!Regex.Match(result, mask.Value).Success) 
                    continue;
                
                UsedMask = mask.Value;
                return Regex.Replace(result, mask.Value, string.Empty);
            }
            return result;
        }
        
        internal void RefreshFileInfo(FileInfo fileInfo, string rootPath)
        {
            FileInfo = fileInfo;
            OriginalFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            Extension = fileInfo.Extension;
            FilePath = fileInfo.Directory.FullName;

            FilePathDisplayValue = FilePath.Replace(rootPath, string.Empty);
            if (string.IsNullOrEmpty(FilePathDisplayValue))
                FilePathDisplayValue = @"\";
        }

        internal void ReadAllMetadata(string newNameFormat, string? additionalName)
        {
            if (_settingsModel == null)
                return;
            
            var metadata = MediaMetadataWrapper.ReadMetadata(FileInfo.FullName);
            foreach (var metadataInfo in _settingsModel.MetadataInfos)
            {
                var dateTime = MediaMetadataWrapper.CalculateDateTime(metadata, metadataInfo, _settingsModel.MetaDateTimeExtensions);
                if (dateTime == null)
                    continue;
                
                var existResultDateTime = MetaDataItems.FirstOrDefault(x => Math.Abs((x.SourceDateTime - dateTime.Value).TotalMinutes) < 1);

                if (existResultDateTime == null)
                {
                    var newName =string.IsNullOrEmpty(additionalName) ? dateTime.Value.ToString(newNameFormat) :  $"{dateTime.Value.ToString(newNameFormat)} {additionalName}";
                    MetaDataItems.Add(new MetadataItemModel(metadataInfo.Caption, dateTime.Value, newName));
                }
                else
                    existResultDateTime.AddMetaInfoCaption(metadataInfo.Caption);
            }
        }

        internal void SelectSingleMetadata()
        {
            var singleMetaData = MetaDataItems.OrderBy(x => x.MetaInfoCaptions.FirstOrDefault())
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
    }
}