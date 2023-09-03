using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.ViewModels
{
    public partial class FileItemViewModel : ViewModelBase
    {
        private readonly FileItemModel _fileItemModel;
        
        public FileItemViewModel(FileItemModel fileItemModel)
        {
            _fileItemModel = fileItemModel;
            MetaDataItems = _fileItemModel.MetaDataItems.Select(x => new MetadataItemViewModel(_fileItemModel, x)).ToList();
        }
        
        public List<MetadataItemViewModel> MetaDataItems { get; }
        public string FilePathDisplayValue => _fileItemModel.FilePathDisplayValue;
        public string OriginalFileName => _fileItemModel.OriginalFileName;
        public string Extension => _fileItemModel.Extension;
        public string UsedMask => _fileItemModel.UsedMask;
        public string Exception => _fileItemModel.Exception;
        public string FullName => _fileItemModel.FileInfo.FullName;
        
        [RelayCommand]
        public void OpenFolder(string path)
        {
            _fileItemModel.ShowFileInFolder(path);
        }
        
        [RelayCommand]
        public void SaveFileMetadata(string path)
        {
            _fileItemModel.SaveFileMetadata(path);
        }
    }
}