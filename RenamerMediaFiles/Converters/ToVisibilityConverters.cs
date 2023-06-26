using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RenamerMediaFiles.Converters
{
    public class StringIsNullToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var result = string.IsNullOrEmpty(value?.ToString());
            if (parameter is string parameterString)
            {
                switch (parameterString)
                {
                    case "invert":
                        result = !result;
                        break;
                }
            }

            return result;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException("StringIsNullToBoolConverter");
        }
    }
}