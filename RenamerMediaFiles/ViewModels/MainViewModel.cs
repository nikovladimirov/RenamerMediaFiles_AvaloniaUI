﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using RenamerMediaFiles.Models;
using RenamerMediaFiles.Services.Implementations;

namespace RenamerMediaFiles.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly MainModel _mainModel;
        private readonly SettingsModel _settingsModel;
        
        private List<FileItemViewModel> _files;

        public MainViewModel()
        {
            var fileService = new JsonFileService();
            var dialogService = new DialogService();
            
            _settingsModel = new SettingsModel(fileService, dialogService);
            _settingsModel.Init();

            _mainModel = new MainModel(_settingsModel, dialogService);
            _mainModel.PropertyChanged += MainWindowModelOnPropertyChanged;
            
            SettingsViewModel = new SettingsViewModel(_settingsModel);
        }

        ~MainViewModel()
        {
            _mainModel.PropertyChanged -= MainWindowModelOnPropertyChanged;
        }

        public SettingsViewModel SettingsViewModel { get; private set; }
        
        private void MainWindowModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_mainModel.Files):
                    MakeInUIThread(() =>
                    {
                        _files = _mainModel.Files.Select(x => new FileItemViewModel(x)).ToList();
                        Debug.WriteLine(
                            $"Files?.Count {_mainModel.Files?.Count ?? 0}");
                    });
                    break;
            }

            MakeInUIThread(() => { OnPropertyChanged(e.PropertyName); });
        }

        public string Status => _mainModel.Status;
        public bool IsBusy => _mainModel.IsBusy;
        public bool IsReadyForRename => _mainModel.Files != null && _mainModel.Files.Count > 0;

        public List<FileItemViewModel> Files => _files;


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

        [RelayCommand]
        public void RenameCommand(object obj)
        {
            _mainModel.Rename();
        }

        [RelayCommand]
        public void ReadCommand(object obj)
        {
            _mainModel.Read();
        }

        #endregion Private Methods
    }
}