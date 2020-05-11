using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Utility
{
    public static class DateTimeParser
    {
        public static DateTime? ConvertFromHHmm(string HHmm)
        {
            if (string.IsNullOrEmpty(HHmm))
                return null;

            if(DateTime.TryParseExact(HHmm, "HHmm", null, System.Globalization.DateTimeStyles.None, out DateTime result))
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
