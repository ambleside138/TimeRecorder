﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace TimeRecorder.Converters;

[ValueConversion(typeof(string), typeof(Visibility))]
class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(value?.ToString())
            || (parameter != null && parameter?.ToString() == value?.ToString()))
        {
            return Visibility.Collapsed;
        }
        else
        {
            return Visibility.Visible;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
