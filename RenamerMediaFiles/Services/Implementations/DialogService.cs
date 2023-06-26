using System.Diagnostics;
using System.IO;
using RenamerMediaFiles.Services.Interfaces;
// using Application = System.Windows.Application;
// using MessageBox = System.Windows.MessageBox;

namespace RenamerMediaFiles.Services.Implementations
{
    public class DialogService : IDialogService
    {
        public void ShowFileInFolder(string path)
        {
            if (!File.Exists(path))
                return;

            var argument = "/select, \"" + path + "\"";
            Process.Start("explorer.exe", argument);
        }

        public bool OpenFolderDialog(string defaultPath, out string selectedPath)
        {
            // using (var dialog = new FolderBrowserDialog())
            // {
            //     if (Directory.Exists(defaultPath))
            //         dialog.SelectedPath = defaultPath;
            //
            //     var result = dialog.ShowDialog();
            //     if (result != DialogResult.OK)
            //     {
            //         selectedPath = null;
            //         return false;
            //     }
            //
            //     selectedPath = dialog.SelectedPath;
            //     return true;
            // }
            selectedPath = "";
            return false;
        }

        public void ShowMessage(string message, bool isInfoMessage)
        {
            // MessageBox.Show(message, Application.Current.MainWindow?.Title, MessageBoxButton.OK,
            //     isInfoMessage ? MessageBoxImage.Information : MessageBoxImage.Error);
        }
    }
}