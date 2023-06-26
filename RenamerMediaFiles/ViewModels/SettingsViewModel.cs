using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using RenamerMediaFiles.Helpers;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;
        
        private ReactiveCommand<Unit,Unit> _addMaskItemCommand;
        private ReactiveCommand<Unit,Unit> _removeMaskItemCommand;
        private ReactiveCommand<Unit,Unit> _setDefaultMaskItemsCommand;
        private ReactiveCommand<Unit,Unit> _saveConfigCommand;
        private ReactiveCommand<Unit,Unit> _loadConfigCommand;
        private ReactiveCommand<Unit,Unit> _selectFolderCommand;

        public SettingsViewModel(SettingsModel settingsModel)
        {
            LoadConfigCommand = ReactiveCommand.Create(LoadConfig);
            SaveConfigCommand = ReactiveCommand.Create(SaveConfig);
            RemoveMaskItemCommand = ReactiveCommand.Create(RemoveMaskItemMethod);
            SetDefaultMaskItemsCommand = ReactiveCommand.Create(SetDefaultMaskItemsMethod);
            AddMaskItemCommand = ReactiveCommand.Create(AddMaskItem);
            SelectFolderCommand = ReactiveCommand.Create(SelectFolder);

            _settingsModel = settingsModel;
        }

        private void AddMaskItem()
        {
            _settingsModel.RemovingByMasks.Insert(0, new StringModel(@"^<your regex value>$"));
        }

        public ReactiveCommand<Unit,Unit> LoadConfigCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveConfigCommand { get; }
        public ReactiveCommand<Unit,Unit> RemoveMaskItemCommand { get; }
        public ReactiveCommand<Unit,Unit> SetDefaultMaskItemsCommand { get; }
        public ReactiveCommand<Unit,Unit> AddMaskItemCommand { get; }
        public ReactiveCommand<Unit,Unit> SelectFolderCommand { get; }


        
        #region Properties

        public ObservableCollection<StringModel> RemovingByMasks => _settingsModel.RemovingByMasks;

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

        #region Private Methods
        
        private void LoadConfig()
        {
            _settingsModel.LoadConfig();
        }

        private void SaveConfig()
        {
            _settingsModel.SaveConfig();
        }

        private void SetDefaultMaskItemsMethod()
        {
            _settingsModel.SetDefaultMaskItemsMethod();
        }
        
        private void SelectFolder()
        {
            _settingsModel.SelectFolder();
        }

        private void RemoveMaskItemMethod()
        {
            _settingsModel.RemoveMaskItemMethod();
        }

        #endregion
    }
}