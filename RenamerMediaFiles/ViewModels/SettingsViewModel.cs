using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;

        public SettingsViewModel(SettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
            _settingsModel.Init();
            _settingsModel.PropertyChanged += SettingsModelOnPropertyChanged;
        }

        ~SettingsViewModel()
        {
            _settingsModel.PropertyChanged -= SettingsModelOnPropertyChanged;
        }

        #region Properties
        
        public IReadOnlyCollection<MetaDateTimeExtension> MetaDateTimeExtensions => _settingsModel.MetaDateTimeExtensions;
        public IReadOnlyCollection<MetadataInfoModel> MetadataInfos => _settingsModel.MetadataInfos;
        public IReadOnlyCollection<StringModel> RemovingByMasks => _settingsModel.RemovingByMasks;

        public bool IsValidSettings => _settingsModel.IsValidSettings;
        public string MaskTextDemo => _settingsModel.MaskTextDemo;

        public string RootPath
        {
            get => _settingsModel.RootPath;
            set => _settingsModel.RootPath = value;
        }

        public string ExtensionText
        {
            get => _settingsModel.ExtensionText;
            set => _settingsModel.ExtensionText = value;
        }

        public string NewNameFormat
        {
            get => _settingsModel.NewNameFormat;
            set => _settingsModel.NewNameFormat = value;
        }

        public bool ChangeNameByMasks
        {
            get => _settingsModel.ChangeNameByMasks;
            set => _settingsModel.ChangeNameByMasks = value;
        }
        
        #endregion

        #region Commands
        
        [RelayCommand]
        public void LoadConfig()
        {
            _settingsModel.LoadConfig();
        }

        [RelayCommand]
        public void SaveConfig()
        {
            _settingsModel.SaveConfig();
        }
        
        [RelayCommand]
        public void SelectFolder()
        {
            _settingsModel.SelectFolder();
        }
        
        [RelayCommand]
        public void AddMaskItem()
        {
            _settingsModel.AddMaskItemMethod();
        }
        
        [RelayCommand]
        public void RemoveMaskItem()
        {
            _settingsModel.RemoveMaskItemMethod();
        }

        [RelayCommand]
        public void SetDefaultMaskItems()
        {
            _settingsModel.SetDefaultMaskItemsMethod();
        }        
        
        [RelayCommand]
        public void AddMetadataInfoItem()
        {
            _settingsModel.AddMetadataInfoMethod();
        }
        
        [RelayCommand]
        public void RemoveMetadataInfoItem()
        {
            _settingsModel.RemoveMetadataInfoMethod();
        }

        [RelayCommand]
        public void SetDefaultMetadataInfoItems()
        {
            _settingsModel.SetDefaultMetadataInfosMethod();
        }
        [RelayCommand]
        public void AddMetaExtensionItem()
        {
            _settingsModel.AddMetaExtensionMethod();
        }
        
        [RelayCommand]
        public void RemoveMetaExtensionItem()
        {
            _settingsModel.RemoveMetaExtensionMethod();
        }

        [RelayCommand]
        public void SetDefaultMetaExtensionItems()
        {
            _settingsModel.SetDefaultMetaExtensionsMethod();
        }
        
        #endregion Commands
        
        #region Private Methods

        private void SettingsModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #endregion
    }
}