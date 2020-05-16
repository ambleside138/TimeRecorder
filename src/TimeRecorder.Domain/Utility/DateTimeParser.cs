using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Utility
{
    public static class DateTimeParser
    {
        public static DateTime? ConvertFromHHmmss(string HHmmss)
        {
            if (string.IsNullOrEmpty(HHmmss))
                return null;

            if(DateTime.TryParseExact(HHmmss, "HHmmss", null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static DateTime? ConvertFromYmd(string ymd)
        {
            if (string.IsNullOrEmpty(ymd))
                return null;

            if (DateTime.TryParseExact(ymd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
