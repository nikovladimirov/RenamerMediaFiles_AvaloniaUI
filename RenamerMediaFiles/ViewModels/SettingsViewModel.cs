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

        public float TimeZoneOffset
        {
            get => _settingsModel.TimeZoneOffset;
            set => _settingsModel.TimeZoneOffset = value;
        }

        public bool ReplaceFullName
        {
            get => _settingsModel.ReplaceFullName;
            set => _settingsModel.ReplaceFullName = value;
        }
        #endregion

        #region Commands
        
        [RelayCommand]
        public void AddMaskItem()
        {
            _settingsModel.RemovingByMasks.Insert(0, new StringModel(@"^<your regex value>$"));
        }
        
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
        public void SetDefaultMaskItems()
        {
            _settingsModel.SetDefaultMaskItemsMethod();
        }
        
        [RelayCommand]
        public void SelectFolder()
        {
            _settingsModel.SelectFolder();
        }

        [RelayCommand]
        public void RemoveMaskItem()
        {
            _settingsModel.RemoveMaskItemMethod();
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