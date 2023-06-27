using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly MainModel _mainModel;

        [ObservableProperty] private List<FileItemViewModel> files;

        private RelayCommand _renameCommand;

        public MainViewModel(MainModel mainModel, SettingsViewModel settingsViewModel)
        {
            _mainModel = mainModel;
            _mainModel.PropertyChanged += MainWindowModelOnPropertyChanged;

            SettingsViewModel = settingsViewModel;
        }

        ~MainViewModel()
        {
            _mainModel.PropertyChanged -= MainWindowModelOnPropertyChanged;
        }

        #region Properties

        public SettingsViewModel SettingsViewModel { get; }
        public string Status => _mainModel.Status;
        public bool IsBusy => _mainModel.IsBusy;

        #endregion Properties

        #region Commands

        public RelayCommand RenameCommand => _renameCommand ??= new RelayCommand(Rename, CanRename);

        [RelayCommand]
        public void Read(object obj)
        {
            _mainModel.Read();
        }

        #endregion Commands

        #region Private Methods

        private void MainWindowModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_mainModel.Files):
                    Files = _mainModel.Files.Select(x => new FileItemViewModel(x)).ToList();
                    RenameCommand.NotifyCanExecuteChanged();
                    break;
            }

            OnPropertyChanged(e.PropertyName);
        }

        private void Rename()
        {
            _mainModel.Rename();
        }

        private bool CanRename()
        {
            return Files != null && Files.Count > 0;
        }

        #endregion Private Methods
    }
}