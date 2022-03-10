using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using TimeRecorder.Domain.Utility.Attributes;

namespace TimeRecorder.Converters;

[ValueConversion(typeof(Enum), typeof(bool))]
class EnumToIconKeyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return "";

        var fi = value.GetType().GetField(value.ToString());
        if (fi == null)
            return "";

        var attribute = (IconKeyAttribute)Attribute.GetCustomAttribute(fi, typeof(IconKeyAttribute));

        return attribute.Key;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
