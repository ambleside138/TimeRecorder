using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain
{
    public class YearMonth : ValueObject<YearMonth>
    {
        public int Year { get; }

        public int Month { get; }

        public static YearMonth FromYmdString(YmdString ymdString)
        {
            var dateTime = ymdString.ToDateTime().Value;
            return new YearMonth(dateTime.Year, dateTime.Month);
        }

        public YearMonth(int year, int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException("出力対象月は1～12の間で指定してください");

            Year = year;
            Month = month;
        }

        public bool Contains(YmdString ymdString)
        {
            var dt = ymdString.ToDateTime();
            if (dt == null)
                return false;

            return dt.Value.Year == Year
                    && dt.Value.Month == Month;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Year;
            yield return Month;
        }

        public DateTime StartDate => new DateTime(Year, Month, 1);

        public DateTime EndDate => new DateTime(Year, Month +1, 1).AddDays(-1);
    }
}
