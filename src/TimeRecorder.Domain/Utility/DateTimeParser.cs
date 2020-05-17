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

            return ConvertFromCore(HHmmss, "HHmmss");
        }

        public static DateTime? ConvertFromYmd(string ymd)
        {
            if (string.IsNullOrEmpty(ymd))
                return null;

            return ConvertFromCore(ymd, "yyyyMMdd");
        }

        public static DateTime? ConvertFromYmdHHmmss(string ymd, string HHmmss)
        {
            if (string.IsNullOrEmpty(ymd))
                return null;

            if (string.IsNullOrEmpty(HHmmss))
                return null;

            return ConvertFromCore(ymd+ HHmmss, "yyyyMMddHHmmss");
        }

        private static DateTime? ConvertFromCore(string value, string format)
        {
            if (DateTime.TryParseExact(value, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime result))
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
