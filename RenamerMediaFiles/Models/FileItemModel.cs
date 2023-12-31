﻿using System;
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
        private IFileService _iFileService;
        private SettingsModel _settingsModel;

        public string FilePathDisplayValue { get; private set; }
        public string FilePath { get; private set; }
        public string OriginalFileName { get; private set; }
        public string Extension { get; private set; }
        public List<MetadataItemModel> MetaDataItems { get; set; } = new List<MetadataItemModel>();
        public FileInfo FileInfo { get; private set; }
        public string UsedMask { get; private set; }

        public string Exception { get; set; }

        public FileItemModel(SettingsModel settingsModel, ISimpleDialogService simpleDialogService, IFileService fileService)
        {
            _simpleDialogService = simpleDialogService;
            _iFileService = fileService;
            _settingsModel = settingsModel;
        }

        public void Init(FileInfo fileInfo, string rootPath, string newNameFormat, bool addAdditionalName, IEnumerable<StringModel> removingByMasks)
        {
            try
            {
                RefreshFileInfo(fileInfo, rootPath);
                var additionalName = addAdditionalName ? GetAdditionalName(OriginalFileName, removingByMasks) : null;
                ReadAllMetadata(FileInfo.FullName, newNameFormat, additionalName);
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

        public void SaveFileMetadata(string filePath)
        {
            var metadataInfo = MediaMetadataWrapper.GetMetadataInfo(filePath);
            var jsonPath = $"{Path.GetDirectoryName(filePath)}\\{Path.GetFileNameWithoutExtension(filePath)}.json";

            _iFileService.Save(jsonPath, metadataInfo);
        }

        public string GetAdditionalName(string originalFileName, IEnumerable<StringModel> removingNameParts)
        {
            var result = originalFileName;
            foreach (var mask in removingNameParts)
            {
                if (!Regex.Match(result, mask.Value).Success) 
                    continue;
                
                UsedMask = mask.Value;
                return Regex.Replace(result, mask.Value, string.Empty);
            }
            return result;
        }
        
        public void RefreshFileInfo(FileInfo fileInfo, string rootPath)
        {
            FileInfo = fileInfo;
            OriginalFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            Extension = fileInfo.Extension;
            FilePath = fileInfo.Directory.FullName;

            FilePathDisplayValue = FilePath.Replace(rootPath, string.Empty) + @"\";
        }

        public void ReadAllMetadata(string fullName, string newNameFormat, string? additionalName)
        {
            var metadata = MediaMetadataWrapper.ReadMetadataDirectories(fullName);
            foreach (var metadataInfo in _settingsModel.MetadataInfos)
            {
                var dateTime = MediaMetadataWrapper.ReadDateTime(metadata, metadataInfo);
                if (dateTime == null)
                    continue;
                
                var metadataItemModel = MetaDataItems.FirstOrDefault(x => Math.Abs((x.SourceDateTime - dateTime.Value).TotalMinutes) < 1);

                if (metadataItemModel == null)
                    MetaDataItems.Add(metadataItemModel = new MetadataItemModel(metadataInfo.Caption, dateTime.Value));
                else
                    metadataItemModel.AddMetaInfoCaption(metadataInfo.Caption);

                var modifiedDateTime = MediaMetadataWrapper.ApplyMetadataExtensions(metadata,
                    _settingsModel.MetaDateTimeExtensions, dateTime.Value);
                
                var newName =string.IsNullOrEmpty(additionalName) ? modifiedDateTime.ToString(newNameFormat) :  $"{modifiedDateTime.ToString(newNameFormat)} {additionalName}";
                metadataItemModel.NewFileName = newName;
            }
        }

        private void SelectSingleMetadata()
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