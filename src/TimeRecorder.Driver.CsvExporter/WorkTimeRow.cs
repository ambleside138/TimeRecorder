using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Driver.CsvExporter
{
    class WorkTimeRow
    {
        public static string AlertMessage = " [ *** 要確認 *** ]";

        [Ignore]
        public string Ymd { get; set; }

        public string DateText { get; set; }

        public string TaskCategory { get; set; }

        public string ProductOrClient { get; set; }

        public string TaskProcess { get; set; }

        public string Remarks { get; set; }

        public string ManHour { get; set; }

        private int _TotalMinutes;

        [Ignore]
        public int TotalMinutes
        {
            get { return _TotalMinutes; }
            set 
            { 
                _TotalMinutes = value;
                ManHour = CalcMonHour();
            }
        }

        [Ignore]
        public bool IsFixed { get; set; }

        public int GetManHourMinutes()
        {
            double.TryParse(ManHour, out double hour);

            return (int)(hour * 60);
        }

        private string CalcMonHour()
        {
            var hour = _TotalMinutes / 60;
            var min = _TotalMinutes % 60;

            if (min == 0)
            {
                return hour.ToString();
            }

            if (min >= 45)
            {
                return (hour + 1).ToString();
            }
            else if (min > 15 && min < 45)
            {
                return $"{hour}.5";
            }
            else
            {
                return hour.ToString();
            }
        }

        public string GetWaringMessage()
        {
            if(ProductOrClient.Contains(AlertMessage))
            {
                return $"{Ymd} {Remarks}";
            }

            return "";
        }

    }
}
