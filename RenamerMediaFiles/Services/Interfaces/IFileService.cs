namespace RenamerMediaFiles.Services.Interfaces
{
    public interface IFileService
    {
        T Load<T>(string path);
        void Save<T>(string path, T source);
    }
}