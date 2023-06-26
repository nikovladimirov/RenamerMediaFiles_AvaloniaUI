namespace RenamerMediaFiles.Workers
{
    public class BaseFilesWorker
    {
        public event ProcessArgs ProcessChanged;
        public delegate void ProcessArgs(string type, int value, int maxValue);

        protected void OnProcessChanged(string type, int value, int maxValue)
        {
            var handler = ProcessChanged;
            handler?.Invoke(type, value, maxValue);
        }
    }
}