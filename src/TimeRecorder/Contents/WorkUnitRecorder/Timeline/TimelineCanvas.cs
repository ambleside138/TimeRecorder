using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TimeRecorder.Contents.WorkUnitRecorder.Timeline;

namespace TimeRecorder.Contents.WorkUnitRecorder;

class TimelineCanvas : Canvas
{
    #region StartHour依存関係プロパティ
    public int StartHour
    {
        get { return (int)GetValue(StartHourProperty); }
        set { SetValue(StartHourProperty, value); }
    }

    // Using a DependencyProperty as the backing store for StartHour.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StartHourProperty =
        DependencyProperty.Register("StartHour", typeof(int), typeof(TimelineCanvas), new PropertyMetadata(0));

    #endregion

    #region EndHour依存関係プロパティ
    public int EndHour
    {
        get { return (int)GetValue(EndHourProperty); }
        set { SetValue(EndHourProperty, value); }
    }

    // Using a DependencyProperty as the backing store for EndHour.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EndHourProperty =
        DependencyProperty.Register("EndHour", typeof(int), typeof(TimelineCanvas), new PropertyMetadata(23, EndHourChanged));

    private static void EndHourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TimelineCanvas)d).InitializeHourText();
    }

    #endregion

    #region HourHeight依存関係プロパティ
    public int HourHeight
    {
        get { return (int)GetValue(HourHeightProperty); }
        set { SetValue(HourHeightProperty, value); }
    }

    // 描画の都合上、4の倍数かつ60以上を推奨
    // ＃作業時間の最小単位を15分、1分を1px以上で表現すると考える場合

    // Using a DependencyProperty as the backing store for HourHeight.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HourHeightProperty =
        DependencyProperty.Register("HourHeight", typeof(int), typeof(TimelineCanvas), new PropertyMetadata(TimelineProperties.Current.HourHeight));
    #endregion


    private const int _HeaderWidth = 30;

    public TimelineCanvas()
    {
        StartHour = TimelineProperties.Current.StartHour;

        InitializeHourText();
    }

    private void InitializeHourText()
    {
        Children.Clear();

        for (var t = StartHour; t <= EndHour; t++)
        {
            var text = new TextBlock
            {
                Text = t.ToString("00")
            };

            var yPosi = (t - StartHour) * HourHeight;
            SetTop(text, yPosi + 8);
            SetLeft(text, 8);

            Children.Add(text);

            var hLine = new Line
            {
                X1 = _HeaderWidth,
                X2 = 500,
                Y1 = yPosi + HourHeight / 2,
                Y2 = yPosi + HourHeight / 2,
                StrokeThickness = 1,
                Stroke = Brushes.LightGray
            };
            Children.Add(hLine);

            hLine = new Line
            {
                X1 = 0,
                X2 = 500,
                Y1 = yPosi + HourHeight,
                Y2 = yPosi + HourHeight,
                StrokeThickness = 1,
                Stroke = Brushes.Black
            };
            Children.Add(hLine);
        }

        Height = (EndHour - StartHour) * HourHeight;

        // 縦棒
        var vline = new Line
        {
            X1 = _HeaderWidth,
            X2 = _HeaderWidth,
            Y1 = 0,
            Y2 = Height,
            StrokeThickness = 1,
            Stroke = Brushes.Black
        };

        Children.Add(vline);
    }


}
