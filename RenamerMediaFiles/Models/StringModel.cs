namespace RenamerMediaFiles.Models
{
    public class StringModel : ModelBase
    {
        private string _value;

        public StringModel(string value)
        {
            _value = value;
        }

        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}