using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RenamerMediaFiles.Helpers;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.Workers
{
    public class FilesReader : BaseFilesWorker
    {
        private readonly string _rootPath;
        private readonly string _extensionsText;
        private readonly string _newNameFormat;
        private readonly bool _changeNameByMasks;
        private readonly IEnumerable<StringModel> _removingNameParts;

        public FilesReader(string rootPath, string extensionsText, string newNameFormat, bool changeNameByMasks, IEnumerable<StringModel> removingNameParts)
        {
            if (rootPath.EndsWith("\\"))
                rootPath = rootPath.Remove(rootPath.Length - 1);
            
            _rootPath = rootPath;
            _extensionsText = extensionsText;
            _newNameFormat = newNameFormat;
            _changeNameByMasks = changeNameByMasks;
            _removingNameParts = removingNameParts;
        }
        
        public List<FileItemModel> ReadFiles()
        {
            var extensions = Split(_extensionsText.Replace(".", string.Empty).ToLower());
            if (extensions.Length == 0)
                return null;

            if (string.IsNullOrEmpty(_rootPath) || !Directory.Exists(_rootPath))
                return null;

            var fileInfos = new List<FileInfo>();
            PrepareFileList(new DirectoryInfo(_rootPath), extensions, fileInfos);

            var files = new ConcurrentStack<FileItemModel>();

            var count = Environment.ProcessorCount - 1;
            if (count <= 1)
            {
                _indexProgress = 0;
                WorkWithFileInfoList(fileInfos, files, fileInfos.Count);
            }
            else
            {
                _indexProgress = 0;
                var countInGroup = (int)Math.Ceiling(((float)fileInfos.Count) / count);
                var groups = fileInfos.Partition(countInGroup);
                var tasks = new List<Task>();
                foreach (var group in groups)
                {
                    tasks.Add(Task.Run(() => WorkWithFileInfoList(group, files, fileInfos.Count)));
                }
                
                Task.WaitAll(tasks.ToArray());
            }

            return files.OrderByDescending(x => x.MetaDataItems != null && x.MetaDataItems.Any(y => y.Selected))
                .ThenByDescending(x => x.MetaDataItems?.FirstOrDefault()?.NewFileName?.Length)
                .ThenBy(x => x.OriginalFileName)
                .ToList();
        }
        
        private int _indexProgress;

        private void WorkWithFileInfoList(List<FileInfo> fileInfos, ConcurrentStack<FileItemModel> files, int totalFilesCount)
        {
            for (int i = 0; i < fileInfos.Count; i++)
            {
                ++_indexProgress;
                if (i % 100 == 0)
                {
                    OnProcessChanged("Read metadata", _indexProgress, totalFilesCount);
                    Thread.Sleep(10);
                }

                var fileItemModel = new FileItemModel();
                fileItemModel.Init(fileInfos[i], _rootPath, _newNameFormat, _changeNameByMasks, _removingNameParts);

                if (string.IsNullOrEmpty(fileItemModel.Exception) &&
                    (!fileItemModel.MetaDataItems.Any()
                     || string.Equals(fileItemModel.MetaDataItems.First().NewFileName, fileItemModel.OriginalFileName,
                         StringComparison.OrdinalIgnoreCase)))
                    continue;

                files.Push(fileItemModel);
            }
        }
        
        private void PrepareFileList(DirectoryInfo currentDirectory, string[] extensions,
            List<FileInfo> files)
        {
            if (currentDirectory.FullName == _rootPath)
            {
                var topLevel = currentDirectory.GetDirectories();
                for (int i = 0; i < topLevel.Length; i++)
                {
                    OnProcessChanged("Read files", i + 1, topLevel.Length);
                    Thread.Sleep(10);
                    PrepareFileList(topLevel[i], extensions, files);
                }
            }
            else
            {
                foreach (var directoryInfo in currentDirectory.GetDirectories())
                {
                    PrepareFileList(directoryInfo, extensions, files);
                }
            }

            files.AddRange(currentDirectory.GetFiles()
                .Where(x => extensions.Contains(x.Extension.Replace(".", string.Empty).ToLower())));
        }
        private string[] Split(string source)
        {
            return source.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}