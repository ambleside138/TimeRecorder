using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Domain.Test.Domain.Tracking
{
    [TestFixture]
    public class TimePeriodTest
    {
        [Test]
        public void 作業時間_終了時刻確定_終了時刻前()
        {
            var start = new DateTime(2020, 6, 1, 9, 0, 0);
            var end = new DateTime(2020, 6, 1, 10, 30, 0);
            var timePeriod = new TimePeriod(start, end);

            var fixedClock = new FixedSystemClock(new DateTime(2020, 6, 1, 9, 50, 0));
            SystemClockServiceLocator.SetSystemClock(fixedClock);

            var min = timePeriod.CalcWorkTimeMinutes();
            Assert.AreEqual(min, 50);
        }

        [Test]
        public void 作業時間_終了時刻確定_終了時刻後()
        {
            var start = new DateTime(2020, 6, 1, 9, 0, 0);
            var end = new DateTime(2020, 6, 1, 10, 30, 0);
            var timePeriod = new TimePeriod(start, end);

            var fixedClock = new FixedSystemClock(new DateTime(2020, 6, 1, 10, 35, 0));
            SystemClockServiceLocator.SetSystemClock(fixedClock);

            var min = timePeriod.CalcWorkTimeMinutes();
            Assert.AreEqual(min, 90);
        }

        [Test]
        public void 作業時間_終了時刻未確定_開始時間前()
        {
            var start = new DateTime(2020, 6, 1, 9, 0, 0);
            DateTime? end = null;
            var timePeriod = new TimePeriod(start, end);

            var fixedClock = new FixedSystemClock(new DateTime(2020, 6, 1, 8, 35, 0));
            SystemClockServiceLocator.SetSystemClock(fixedClock);

            var min = timePeriod.CalcWorkTimeMinutes();
            Assert.AreEqual(min, 0);
        }

        [Test]
        public void 作業時間_終了時刻未確定_開始時間後()
        {
            var start = new DateTime(2020, 6, 1, 9, 0, 0);
            DateTime? end = null;
            var timePeriod = new TimePeriod(start, end);

            var fixedClock = new FixedSystemClock(new DateTime(2020, 6, 1, 10, 35, 0));
            SystemClockServiceLocator.SetSystemClock(fixedClock);

            var min = timePeriod.CalcWorkTimeMinutes();
            Assert.AreEqual(min, 95);
        }

        [TestCase(8,59,false)] // 開始時刻前：作業中でない
        [TestCase(9,0,true)] // 開始時刻ちょうど：作業中
        [TestCase(9,1,true)] // 開始時刻後：作業中
        public void 作業中_終了時刻未定(int hour, int min, bool result)
        {
            var start = new DateTime(2020, 6, 1, 9, 0, 0);
            DateTime? end = null;
            var timePeriod = new TimePeriod(start, end);

            var fixedClock = new FixedSystemClock(new DateTime(2020, 6, 1, hour, min, 0));
            SystemClockServiceLocator.SetSystemClock(fixedClock);

            Assert.AreEqual(timePeriod.WithinRangeAtCurrentTime, result);
        }


        [TestCase(8, 30, 8, 59, false, TestName = "開始・終了共に範囲外（開始時間前）")]
        [TestCase(8, 30, 9, 30, true, TestName = "開始範囲外、終了は範囲内")]
        [TestCase(8, 30, 10, 59, true, TestName = "内包_開始・終了共に範囲外")]
        [TestCase(9, 30, 9, 59, true, TestName = "内包_開始・終了共に範囲内")]
        [TestCase(11, 30, 11, 59, false, TestName = "開始・終了共に範囲外（終了時間後）")]
        [TestCase(8, 30, 9, 00, false, TestName = "境界値_終了時刻が開始時刻に一致")]
        [TestCase(10, 30, 11, 59, false, TestName = "境界値_開始時刻が終了時刻に一致")]
        [TestCase(8, 30, 0, 0, true, TestName = "終了時刻未定_開始時刻前")]
        [TestCase(9, 30, 0, 0, true, TestName = "終了時刻未定_開始時刻後～終了時刻前")]
        [TestCase(11, 30, 0, 0, false, TestName = "終了時刻未定_終了時刻後")]
        public void 時間重複_終了時刻確定(int hour, int min, int endhour, int endmin, bool result)
        {
            var start = new DateTime(2020, 6, 1, 9, 0, 0);
            var   end = new DateTime(2020, 6, 1, 10, 30, 0);

            var timePeriod = new TimePeriod(start, end);

            var otherStart = new DateTime(2020, 6, 1,    hour,    min, 0);
            var   otherEnd = (endhour == 0 && endmin == 0 ) ? (DateTime?)null : new DateTime(2020, 6, 1, endhour, endmin, 0);
            var otherTimePeriod = new TimePeriod(otherStart, otherEnd);

            var overlap = timePeriod.IsOverlapped(otherTimePeriod);
            Assert.AreEqual(overlap, result);
        }

        [TestCase(8, 30, 8, 59, false, TestName = "開始・終了共に範囲外（開始時間前）")]
        [TestCase(8, 30, 9, 30, true, TestName = "開始範囲外、終了は範囲内")]
        [TestCase(9, 30, 9, 59, true, TestName = "内包_開始・終了共に範囲内")]
        [TestCase(8, 30, 9, 00, false, TestName = "境界値_終了時刻が開始時刻に一致")]
        [TestCase(8, 30, 0, 0, true, TestName = "終了時刻未定_開始時刻前")]
        [TestCase(9, 30, 0, 0, true, TestName = "終了時刻未定_開始時刻後～終了時刻前")]
        public void 時間重複_終了時刻未定(int hour, int min, int endhour, int endmin, bool result)
        {
            var start = new DateTime(2020, 6, 1, 9, 0, 0);

            var timePeriod = new TimePeriod(start, null);

            var otherStart = new DateTime(2020, 6, 1, hour, min, 0);
            var otherEnd = (endhour == 0 && endmin == 0) ? (DateTime?)null : new DateTime(2020, 6, 1, endhour, endmin, 0);
            var otherTimePeriod = new TimePeriod(otherStart, otherEnd);

            var overlap = timePeriod.IsOverlapped(otherTimePeriod);
            Assert.AreEqual(overlap, result);
        }
    }
}
