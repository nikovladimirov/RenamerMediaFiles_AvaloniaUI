using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.ViewModels
{
    public class FileItemViewModel : ViewModelBase
    {
        private readonly FileItemModel _fileItemModel;
        
        public FileItemViewModel(FileItemModel fileItemModel)
        {
            _fileItemModel = fileItemModel;
            MetaDataItems = _fileItemModel.MetaDataItems.Select(x => new MetadataItemViewModel(_fileItemModel, x)).ToList();
            OpenFolderCommand = ReactiveCommand.Create<string>(OpenFolder);
        }
        
        public List<MetadataItemViewModel> MetaDataItems { get; }
        public string FilePathDisplayValue => _fileItemModel.FilePathDisplayValue;
        public string OriginalFileName => _fileItemModel.OriginalFileName;
        public string Extension => _fileItemModel.Extension;
        public string UsedMask => _fileItemModel.UsedMask;
        public string Exception => _fileItemModel.Exception;
        public string FullName => _fileItemModel.FileInfo.FullName;
        public ReactiveCommand<string,Unit> OpenFolderCommand{get;}
        
        private void OpenFolder(string path)
        {
            _fileItemModel.ShowFileInFolder(path);
        }
    }
}