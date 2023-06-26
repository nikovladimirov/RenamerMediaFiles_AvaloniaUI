namespace RenamerMediaFiles.Services.Interfaces
{
    public interface IDialogService
    {
        void ShowFileInFolder(string path);
        bool OpenFolderDialog(string defaultPath, out string selectedPath);
        void ShowMessage(string message, bool isInfoMessage);
    }
}