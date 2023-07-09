using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RenamerMediaFiles.Helpers;
using RenamerMediaFiles.Services.Interfaces;

namespace RenamerMediaFiles.Models
{
    public class SettingsModel : ModelBase
    {
        private readonly IFileService _fileService;
        private readonly ISimpleDialogService _simpleDialogService;

        private string _rootPath = DefaultSettings.RootPath;
        private string _newNameFormat = DefaultSettings.NewNameFormat;
        private string _extensionText = DefaultSettings.ExtensionText;

        private bool _isValidSettings;
        private bool _changeNameByMasks = DefaultSettings.ChangeNameByMasks;
        private string _maskTextDemo;

        public SettingsModel(IFileService fileService, ISimpleDialogService simpleDialogService)
        {
            _fileService = fileService;
            _simpleDialogService = simpleDialogService;
        }


        #region Serialized Properties

        public ObservableCollection<StringModel> RemovingByMasks { get; set; } =
            new ObservableCollection<StringModel>();

        public ObservableCollection<MetaDateTimeExtension> MetaDateTimeExtensions { get; set; } =
            new ObservableCollection<MetaDateTimeExtension>();

        public ObservableCollection<MetadataInfoModel> MetadataInfos { get; set; } =
            new ObservableCollection<MetadataInfoModel>();

        public string RootPath
        {
            get { return _rootPath; }
            set
            {
                SetProperty(ref _rootPath, value);
                CheckFields();
            }
        }

        public string ExtensionText
        {
            get => _extensionText;
            set
            {
                SetProperty(ref _extensionText, value);
                CheckFields();
            }
        }

        public string NewNameFormat
        {
            get => _newNameFormat;
            set
            {
                SetProperty(ref _newNameFormat, value);
                CheckFields();
            }
        }

        public bool ChangeNameByMasks
        {
            get => _changeNameByMasks;
            set => SetProperty(ref _changeNameByMasks, value);
        }

        #endregion Serialized Properties

        #region Properties

        [JsonIgnore]
        public string MaskTextDemo
        {
            get => _maskTextDemo;
            private set => SetProperty(ref _maskTextDemo, value);
        }

        [JsonIgnore]
        public bool IsValidSettings
        {
            get => _isValidSettings;
            private set => SetProperty(ref _isValidSettings, value);
        }

        #endregion

        public void Init()
        {
            LoadConfig();
            CheckFields();
        }

        public void AddMaskItemMethod()
        {
            RemovingByMasks.Insert(0, new StringModel(@"^<your regex value>$"));
        }

        public void RemoveMaskItemMethod()
        {
            if (RemovingByMasks.Count == 0)
                return;

            RemovingByMasks.RemoveAt(0);
        }

        public void AddMetadataInfoMethod()
        {
            MetadataInfos.Insert(0, new MetadataInfoModel());
        }

        public void RemoveMetadataInfoMethod()
        {
            if (MetadataInfos.Count == 0)
                return;

            MetadataInfos.RemoveAt(0);
        }

        public void SetDefaultMaskItemsMethod()
        {
            RemovingByMasks.Clear();
            DefaultSettings.RemoveByMask.ForEach(x => RemovingByMasks.Add(new StringModel(x)));
        }

        public void SetDefaultMetadataInfosMethod()
        {
            MetadataInfos.Clear();
            DefaultSettings.DefaultMetadataInfos.ForEach(x => MetadataInfos.Add(x.Value));
        }

        public void SetDefaultMetaExtensionsMethod()
        {
            MetaDateTimeExtensions.Clear();
            DefaultSettings.MetaDateTimeExtensions.ForEach(x => MetaDateTimeExtensions.Add(x));
        }

        public void RemoveMetaExtensionMethod()
        {
            if (MetaDateTimeExtensions.Count == 0)
                return;

            MetaDateTimeExtensions.RemoveAt(0);
        }

        public void AddMetaExtensionMethod()
        {
            MetaDateTimeExtensions.Insert(0, new MetaDateTimeExtension());
        }

        public async Task SelectFolder()
        {
            var path = await _simpleDialogService.OpenFolderDialog(RootPath);
            if (!Directory.Exists(path))
                return;

            RootPath = path;
        }

        public void LoadConfig()
        {
            try
            {
                var loadedSettings = _fileService.Load<SettingsModel>(DefaultSettings.ConfigPath);
                if (loadedSettings == default)
                {
                    SetDefaultMaskItemsMethod();
                    SetDefaultMetadataInfosMethod();
                    SetDefaultMetaExtensionsMethod();
                    return;
                }

                loadedSettings.CopyByInterfaceTo(this);
                OnPropertyChanged(nameof(RemovingByMasks));
                OnPropertyChanged(nameof(MetadataInfos));
                OnPropertyChanged(nameof(MetaDateTimeExtensions));
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, false);
            }
        }

        public void SaveConfig()
        {
            try
            {
                _fileService.Save(DefaultSettings.ConfigPath, this);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, false);
            }
        }

        private bool CheckFieldsImpl()
        {
            if (!Directory.Exists(RootPath))
                return false;
            MaskTextDemo = DateTime.Now.ToString(_newNameFormat);

            return true;
        }

        private void CheckFields()
        {
            try
            {
                IsValidSettings = CheckFieldsImpl();
            }
            catch (Exception)
            {
                IsValidSettings = false;
            }
        }

        private void ShowMessage(string message, bool isInfoMessage)
        {
            _simpleDialogService.ShowMessage(message, isInfoMessage);
        }
    }
}