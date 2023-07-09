using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.Workers
{
    public class FilesRenamer : BaseFilesWorker
    {
        public int CountRename { get; private set; }
        
        public (bool isOk, string message) Rename(List<FileItemModel> files)
        {
            CountRename = 0;

            var sbErrors = new StringBuilder();

            var filesToRename = files.Where(x => x.MetaDataItems.Any(y => y.Selected)).ToList();
            for (int i = 0; i < filesToRename.Count; i++)
            {
                var fileItemModel = filesToRename[i];
                if (i % 100 == 0)
                {
                    OnProcessChanged("Rename", i + 1, filesToRename.Count);
                    Thread.Sleep(10);
                }

                var applyName = fileItemModel.MetaDataItems.FirstOrDefault(x => x.Selected);
                if (applyName == null)
                    continue;
                
                try
                {
                    var destinationPath = Path.Combine(fileItemModel.FilePath, applyName.NewFileName + fileItemModel.Extension);
                    if (IsCurrentName(fileItemModel, destinationPath))
                        continue;

                    var index = 2;
                    while (File.Exists(destinationPath))
                    {
                        destinationPath = Path.Combine(fileItemModel.FilePath, $"{applyName.NewFileName} ({index++}){fileItemModel.Extension}");
                        if (IsCurrentName(fileItemModel, destinationPath))
                            break;
                    }

                    if (IsCurrentName(fileItemModel, destinationPath))
                        continue;

                    RenameActual(fileItemModel.FileInfo.FullName, destinationPath);
                    CountRename++;
                }
                catch (Exception ex)
                {
                    sbErrors.AppendLine($"{fileItemModel.FileInfo.FullName} {ex.Message}");
                    fileItemModel.Exception = ex.Message;
                }
                finally
                {
                    if (string.IsNullOrEmpty(fileItemModel.Exception))
                        applyName.Selected = false;
                }
            }

            if (sbErrors.Length > 0)
                return (false, sbErrors.ToString());

            return (true, $"Renamed files: {CountRename}");
        }

        internal virtual void RenameActual(string sourceFullPath, string desitantionFullPath)
        {
            File.Move(sourceFullPath, desitantionFullPath);
        }
        
        private bool IsCurrentName(FileItemModel fileItemVm, string destinationPath)
        {
            return string.Equals(fileItemVm.FileInfo.FullName, destinationPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}