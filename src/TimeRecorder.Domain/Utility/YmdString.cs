using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TimeRecorder.Domain.Utility
{
    /// <summary>
    /// 日付を表します
    /// </summary>
    public struct YmdString : IEquatable<YmdString>
    {
        public static YmdString Empty => new YmdString();

        public static YmdString NoPlan => new YmdString("99991231");

        public string Value { get; }

        public YmdString(DateTime dateTime)
        {
            Value = dateTime.ToString("yyyyMMdd");
        }

        public YmdString(string ymd)
        {
            Value = ymd;
        }


        public DateTime? ToDateTime()
        {
            return DateTimeParser.ConvertFromYmd(Value);
        }

        public bool Equals(YmdString other)
        {
            return Value == other.Value;
        }
    }
}
