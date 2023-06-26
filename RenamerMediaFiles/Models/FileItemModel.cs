using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MetadataExtractor;
using RenamerMediaFiles.Helpers;
using RenamerMediaFiles.Services.Interfaces;

namespace RenamerMediaFiles.Models
{
    public class FileItemModel
    {
        private static readonly Dictionary<DateSource, (string attributeName, string attributeTag, string tagMask)>
            _dateSourceDictionary =
                new Dictionary<DateSource, (string attributeName, string attributeTag, string tagMask)>
                {
                    { DateSource.Exif_IFD0, ("Exif IFD0", "Date/Time", "yyyy:MM:dd HH:mm:ss") },
                    { DateSource.Exif_SubIFD, ("Exif SubIFD", "Date/Time Original", "yyyy:MM:dd HH:mm:ss") },
                    // {
                    //     DateSource.File,
                    //     ("File", "File Modified Date", "ddd MMM dd HH:mm:ss K yyyy")
                    // },
                    {
                        DateSource.QuickTime_Metadata_Header,
                        ("QuickTime Metadata Header", "Creation Date", "ddd MMM dd HH:mm:ss K yyyy")
                    },  
                    {
                        DateSource.QuickTime_Movie_Header,
                        ("QuickTime Movie Header", "Created", "ddd MMM dd HH:mm:ss yyyy")
                    },
                };

        private readonly IDialogService _dialogService;
        public string NewNameFormat { get; private set; }
        private readonly bool _replaceFullName;
        private string _additionalName;

        public string FilePathDisplayValue { get; private set; }
        public string FilePath { get; private set; }
        public string OriginalFileName { get; private set; }
        public string Extension { get; private set; }
        public List<MetadataItemModel> MetaDataItems { get; set; } = new List<MetadataItemModel>();
        public FileInfo FileInfo { get; private set; }
        public string UsedMask { get; private set; }
        
        public string Exception { get; set; }
        
        public FileItemModel(FileInfo fileInfo, string rootPath, string newNameFormat, bool replaceFullName, IEnumerable<StringModel> removingByMasks, float timeZoneOffset)
        {
            NewNameFormat = newNameFormat;
            _replaceFullName = replaceFullName;

            try
            {
                RefreshFileInfo(fileInfo, rootPath, removingByMasks);
                ReadMetadata(timeZoneOffset);
                SelectSingleMetadata();
            }
            catch (Exception ex)
            {
                Exception = $"ReadMetaData. {ex.Message}";
            }
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

        private void ReadMetadata(float timeZoneOffset)
        {
            var metadata = ImageMetadataReader.ReadMetadata(FileInfo.FullName);
            foreach (var key in _dateSourceDictionary.Keys)
                ReadAttribute(metadata, key, timeZoneOffset);
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
        
        private void ReadAttribute(IReadOnlyList<MetadataExtractor.Directory> metadata, DateSource dateSource, float timeZoneOffset)
        {
            (string attributeName, string attributeTag, string tagMask) result;
            if (!_dateSourceDictionary.TryGetValue(dateSource, out result))
                return;

            var dictionaryExif = metadata.FirstOrDefault(x => string.Equals(x.Name, result.attributeName));
            if (dictionaryExif == null)
                return;

            var datetimeTag = dictionaryExif.Tags.FirstOrDefault(x => string.Equals(x.Name, result.attributeTag));
            if (datetimeTag == null)
                return;

            if (!DateTime.TryParseExact(datetimeTag.Description, result.tagMask,
                    CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime resultDateTime))
                return;

            if (result.tagMask.Contains("K"))
                resultDateTime = resultDateTime.AddHours(timeZoneOffset);

            if (resultDateTime.Year < 1990 || resultDateTime.Year > DateTime.Now.Year)
                throw new Exception($"{dateSource}. Incorrect datetime {resultDateTime.ToString(NewNameFormat)}");

            var existResultDateTime =
                MetaDataItems.FirstOrDefault(x => Math.Abs((x.SourceDateTime - resultDateTime).TotalMinutes) < 1);

            if (existResultDateTime == null)
                MetaDataItems.Add(new MetadataItemModel(resultDateTime, dateSource, _replaceFullName, NewNameFormat, _additionalName));
            else
                existResultDateTime.AddDateSource(dateSource);
        }
        

        public void ShowFileInFolder(string path)
        {
            _dialogService.ShowFileInFolder(path);
        }
    }
}