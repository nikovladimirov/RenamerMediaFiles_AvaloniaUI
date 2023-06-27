using System.ComponentModel;
using System.Linq;
using RenamerMediaFiles.Helpers;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.ViewModels
{
    public class MetadataItemViewModel : ViewModelBase
    {
        private readonly MetadataItemModel _metadataItemModel;
        private readonly FileItemModel _fileItemModel;

        public MetadataItemViewModel(FileItemModel fileItemModel, MetadataItemModel metadataItemModel)
        {
            _fileItemModel = fileItemModel;
            _metadataItemModel = metadataItemModel;
            _metadataItemModel.PropertyChanged += MetadataItemModelOnPropertyChanged;

            SourceDateTimeDisplayValue = _metadataItemModel.SourceDateTime.ToString(_fileItemModel.NewNameFormat);
        }

        ~MetadataItemViewModel()
        {
            _metadataItemModel.PropertyChanged -= MetadataItemModelOnPropertyChanged;
        }

        private void MetadataItemModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public string SourceDateTimeDisplayValue { get; }
        public string DateSourceDisplayValue => _metadataItemModel.DateSourceDisplayValue;

        public bool Selected
        {
            get => _metadataItemModel.Selected;
            set
            {
                _metadataItemModel.Selected = value;
                if (value)
                    _fileItemModel.MetaDataItems.Where(x => x != _metadataItemModel).ForEach(x => x.Selected = false);
            }
        }

        public string NewFileName
        {
            get => _metadataItemModel.NewFileName;
            set => _metadataItemModel.NewFileName = value;
        }
    }
}