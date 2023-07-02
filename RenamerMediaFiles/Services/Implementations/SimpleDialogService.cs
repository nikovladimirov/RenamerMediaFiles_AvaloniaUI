using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using RenamerMediaFiles.Services.Interfaces;
using RenamerMediaFiles.Views;

namespace RenamerMediaFiles.Services.Implementations
{
    public class SimpleDialogService : ISimpleDialogService
    {
        public void ShowFileInFolder(string path)
        {
            if (!File.Exists(path))
                return;

            var argument = "/select, \"" + path + "\"";
            Process.Start("explorer.exe", argument);
        }

        public async Task<string?> OpenFolderDialog(string defaultPath)
        {
            var openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Title = MainWindow.Instance.Title;
            if(Directory.Exists(defaultPath))
                openFolderDialog.Directory = defaultPath;

            return await openFolderDialog.ShowAsync(MainWindow.Instance);
        }

        public async void ShowMessage(string message, bool isInfoMessage)
        {
            await MessageBox.Show(MainWindow.Instance, message, MainWindow.Instance.Title,
                MessageBox.MessageBoxButtons.Ok);
        }
    }
}