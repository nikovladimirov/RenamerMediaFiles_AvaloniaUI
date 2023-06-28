using System.Threading.Tasks;

namespace RenamerMediaFiles.Services.Interfaces
{
    public interface ISimpleDialogService
    {
        void ShowFileInFolder(string path);
        Task<string?> OpenFolderDialog(string defaultPath);
        void ShowMessage(string message, bool isInfoMessage);
    }
}