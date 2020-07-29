using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using TimeRecorder.Domain.Domain.Tasks;

namespace TimeRecorder.Converters
{
    [ValueConversion(typeof(Enum), typeof(SolidColorBrush))]
    class TaskCategoryToSolidBrushConverter : IValueConverter
    {
        private readonly static Dictionary<TaskCategory, SolidColorBrush> _DictColors;

        static TaskCategoryToSolidBrushConverter()
        {
            Color brush(string key) => (Color)App.Current.Resources[key];

            _DictColors  = new Dictionary<TaskCategory, SolidColorBrush>
            {
                [TaskCategory.UnKnown] = new SolidColorBrush(brush("Blue GreyPrimary100")),
                [TaskCategory.Develop] = new SolidColorBrush(brush("PurplePrimary300")),
                [TaskCategory.ResearchAndDevelopment] = new SolidColorBrush(brush("IndigoPrimary300")),
                [TaskCategory.Introduce] = new SolidColorBrush(brush("TealPrimary300")),
                [TaskCategory.Maintain] = new SolidColorBrush(brush("RedPrimary300")),
                [TaskCategory.Other] = new SolidColorBrush(brush("BrownPrimary100")),
            };
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            return _DictColors[(TaskCategory)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        
    }
}
