using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain;

/// <summary>
/// 年月を表す値オブジェクトです
/// </summary>
public class YearMonth : ValueObject<YearMonth>
{
    /// <summary> 年 </summary>
    public int Year { get; }

    /// <summary> 月 </summary>
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

    /// <summary>
    /// 引数の日付を含むかどうかを判定します
    /// </summary>
    /// <param name="ymdString"></param>
    /// <returns></returns>
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

    public DateTime StartDate => new(Year, Month, 1);

    public DateTime EndDate

    {
        get
        {
            var nextMonth = new DateTime(Year, Month, 1).AddMonths(1);
            return nextMonth.AddDays(-1);
        }

    }
}
