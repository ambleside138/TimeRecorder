using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TimeRecorder.Styles
{
    public static class TabControlAssist
    {




        public static DataTemplate GetOptionalControl(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(OptionalControlProperty);
        }

        public static void SetOptionalControl(DependencyObject obj, ContentPresenter value)
        {
            obj.SetValue(OptionalControlProperty, value);
        }

        // Using a DependencyProperty as the backing store for OptionalControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionalControlProperty =
            DependencyProperty.RegisterAttached("OptionalControl", typeof(DataTemplate), typeof(TabControlAssist), new PropertyMetadata(null));




        public static GridLength GetSplitterWidth(DependencyObject obj)
        {
            return (GridLength)obj.GetValue(SplitterWidthProperty);
        }

        public static void SetSplitterWidth(DependencyObject obj, GridLength value)
        {
            obj.SetValue(SplitterWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for SplitterWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SplitterWidthProperty =
            DependencyProperty.RegisterAttached("SplitterWidth", typeof(GridLength), typeof(TabControlAssist), new PropertyMetadata(new GridLength(1)));




    }
}
