using System.IO;
using System.Text;
using Newtonsoft.Json;
using RenamerMediaFiles.Services.Interfaces;

namespace RenamerMediaFiles.Services.Implementations
{
    public class JsonFileService : IFileService
    {
        public T Load<T>(string path)
        {
            if (!File.Exists(path))
                return default;

            var data = File.ReadAllText(path, Encoding.UTF8);
            var settings = JsonConvert.DeserializeObject<T>(data);
            return settings;
        }

        public void Save<T>(string path, T source)
        {
            var data = JsonConvert.SerializeObject(source, Formatting.Indented);
            File.WriteAllText(path, data, Encoding.UTF8);
        }
    }
}