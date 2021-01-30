using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain
{
    // structで定義すると引数なしコンストラクタを隠せないので必ずclassで定義するように

    /// <summary>
    /// 時間の概念を含まない日付の値オブジェクトを表します
    /// </summary>
    public class YmdString : ValueObject<YmdString>, IComparable<YmdString>
    {
        public static YmdString Empty => new YmdString("");

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


        public int CompareTo([AllowNull] YmdString other)
        {
            if (other == null)
                return 1;

            return Value.CompareTo(other.Value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
