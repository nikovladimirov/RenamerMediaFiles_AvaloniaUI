using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using RenamerMediaFiles.Models;
using RenamerMediaFiles.Services.Implementations;

namespace RenamerMediaFiles.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly MainModel _mainModel;
        private readonly SettingsModel _settingsModel;

        private List<FileItemViewModel> _fileViewModels;

        public MainViewModel()
        {
            var fileService = new JsonFileService();
            var dialogService = new DialogService();
            
            _settingsModel = new SettingsModel(fileService, dialogService);
            _settingsModel.Init();

            _mainModel = new MainModel(_settingsModel, dialogService);
            _mainModel.PropertyChanged += MainWindowModelOnPropertyChanged;
            
            SettingsViewModel = new SettingsViewModel(_settingsModel);
            
            ReadCommand = ReactiveCommand.Create(Read);//, this.WhenAnyValue(vm => _settingsModel.IsValidSettings));
            RenameCommand = ReactiveCommand.Create(Rename);//, this.WhenAnyValue(vm => IsReadyForRename));
        }


        // ~MainViewModel()
        // {
        //     if (_mainWindowModel != null)
        //         _mainWindowModel.PropertyChanged -= MainWindowModelOnPropertyChanged;
        // }

        public SettingsViewModel SettingsViewModel { get; private set; }
        
        private void MainWindowModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_mainModel.Files):
                    MakeInUIThread(() =>
                    {
                        _fileViewModels = _mainModel.Files.Select(x => new FileItemViewModel(x)).ToList();
                        Debug.WriteLine(
                            $"Files?.Count {_mainModel.Files?.Count ?? 0}");
                    });
                    break;
            }

            // MakeInUIThread(() => { this.RaiseAndSetIfChanged() OnPropertyChanged(e.PropertyName); });
        }

        public ReactiveCommand<Unit,Unit> ReadCommand {get;}
        public ReactiveCommand<Unit,Unit> RenameCommand {get;}

        public string Status => _mainModel.Status;
        public bool IsBusy => _mainModel.IsBusy;
        public bool IsReadyForRename => _mainModel.Files != null && _mainModel.Files.Count > 0;

        public List<FileItemViewModel> FileViewModels => _fileViewModels;


        #region Private Methods

        private static void MakeInUIThread(Action action)
        {
            // if (Thread.CurrentThread == Application.Current.Dispatcher.Thread)
            // {
                action();
                // return;
            // }
            // Application.Current.Dispatcher.Invoke(action);
        }

        private void Rename()
        {
            _mainModel.Rename();
        }

        private void Read()
        {
            _mainModel.Read();
        }

        #endregion Private Methods
    }
}