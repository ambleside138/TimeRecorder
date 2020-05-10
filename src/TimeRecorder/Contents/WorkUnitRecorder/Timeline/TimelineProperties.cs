using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Contents.WorkUnitRecorder.Timeline
{
    class TimelineProperties
    {
        public static TimelineProperties Current { get; } = new TimelineProperties();

        /// <summary>
        /// １時間あたりの高さ
        /// </summary>
        public int HourHeight { get; set; } = 60;

    }
}
