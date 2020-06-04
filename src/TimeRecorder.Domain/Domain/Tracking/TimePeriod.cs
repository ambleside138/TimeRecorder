using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Domain.Domain.Tracking
{
    /// <summary>
    /// 開始時刻・終了時刻を表します
    /// </summary>
    public class TimePeriod : ValueObject<TimePeriod>
    {
        /// <summary>
        /// 開始日時
        /// </summary>
        public DateTime StartDateTime { get; private set; }

        /// <summary>
        /// 終了日時
        /// </summary>
        public DateTime? EndDateTime { get; private set; }

        private static ISystemClock _SystemClock => SystemClockServiceLocator.Current;

        /// <summary>
        /// 作業対象の日付を表します
        /// </summary>
        public string TargetYmd => StartDateTime.ToYmd();

        public bool IsStopped => EndDateTime.HasValue;

        /// <summary>
        /// 現在時刻が開始～終了時刻の範囲内かどうかを表します
        /// </summary>
        public bool WithinRangeAtCurrentTime => StartDateTime < _SystemClock.Now
                                && (EndDateTime.HasValue == false || EndDateTime.Value > _SystemClock.Now);

        public static TimePeriod CreateForStart()
        {
            return new TimePeriod(_SystemClock.Now, null);
        }

        public TimePeriod(string startHHmm, string endHHmm)
            : this(DateTimeParser.ConvertFromHHmmss(startHHmm).Value, DateTimeParser.ConvertFromHHmmss(endHHmm)) { }

        public TimePeriod(DateTime start, DateTime? end)
        {
            if (end.HasValue
                && start.Date != end.Value.Date)
                throw new ArgumentException("開始時刻と終了時刻は同一日を指定してください");

            StartDateTime = start;
            EndDateTime = end;
        }


        /// <summary>
        /// 作業時間を計算します
        /// </summary>
        /// <returns></returns>
        public int CalcWorkTimeMinutes()
        {
            if (EndDateTime.HasValue
                && _SystemClock.Now > EndDateTime.Value)
            {
                return (int)(EndDateTime.Value - StartDateTime).TotalMinutes;
            }

            if (_SystemClock.Now > StartDateTime)
            {
                return (int)(_SystemClock.Now - StartDateTime).TotalMinutes;
            }
            else
            {
                return 0;
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StartDateTime;
            yield return EndDateTime;
        }
    }
}
