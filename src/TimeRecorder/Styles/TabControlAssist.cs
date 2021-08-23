using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TimeRecorder.Styles
{
    public static class TabControlAssist
    {



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
