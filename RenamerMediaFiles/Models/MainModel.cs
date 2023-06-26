using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using RenamerMediaFiles.Helpers;
using RenamerMediaFiles.Services.Interfaces;
using RenamerMediaFiles.Workers;

namespace RenamerMediaFiles.Models
{
    public class MainModel : ModelBase
    {
        private string _status;
        private bool _isBusy;

        private List<FileItemModel> _files;

        private readonly IDialogService _dialogService;
        private readonly SettingsModel _settingsModel;

        public MainModel(SettingsModel settingsModel, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _settingsModel = settingsModel;
        }


        #region Settings Properties
        
        public List<FileItemModel> Files
        {
            get => _files;
            private set
            {
                _files = value;
                this.RaiseAndSetIfChanged(ref _files, value);
            }
        }

        #endregion Settings Properties

        #region Properties

        public string Status
        {
            get => _status;
            private set
            {
                _status = value;
                this.RaiseAndSetIfChanged(ref _status, value);
            }
        }
                
        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                _isBusy = value;
                this.RaiseAndSetIfChanged(ref _isBusy, value);
            }
        }

        #endregion Properties

        #region Public methods

        
        public async void Read()
        {
            if (_isBusy)
                return;

            IsBusy = true;

            var reader = new FilesReader(
                _settingsModel.RootPath,
                _settingsModel.ExtensionText,
                _settingsModel.NewNameFormat,
                _settingsModel.ReplaceFullName,
                _settingsModel.RemovingByMasks,
                _settingsModel.TimeZoneOffset
            );

            try
            {
                reader.ProcessChanged += OnProcessChanged;
                var result = await Task.Factory.StartNew(() => reader.ReadFiles());

                Files = result;
            }
            catch (Exception ex)
            {
                Files = null;
                ShowMessage(ex.Message, false);
            }
            finally
            {
                reader.ProcessChanged -= OnProcessChanged;
                IsBusy = false;
            }
        }


        public async void Rename()
        {
            if (_isBusy)
                return;

            IsBusy = true;
            var filesRenamer = new FilesRenamer();

            try
            {
                filesRenamer.ProcessChanged += OnProcessChanged;
                var result = await Task.Factory.StartNew(() => filesRenamer.Rename(_files));

                ShowMessage(result.message, result.isOk);
            }
            finally
            {
                filesRenamer.ProcessChanged -= OnProcessChanged;
                IsBusy = false;
            }

            Read();
        }

        #endregion

        #region Private methods
        

        private void ShowMessage(string message, bool isInfoMessage)
        {
            _dialogService.ShowMessage(message, isInfoMessage);
        }

        private void OnProcessChanged(string type, int value, int maxValue)
        {
            Status = $"{type}: {value / maxValue * 100}%";
        }

        #endregion
    }
}