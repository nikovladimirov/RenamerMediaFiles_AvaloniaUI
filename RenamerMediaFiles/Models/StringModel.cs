using RenamerMediaFiles.Helpers;

namespace RenamerMediaFiles.Models
{
    public class StringModel 
    {
        private string _value;

        public StringModel(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }
    }
}