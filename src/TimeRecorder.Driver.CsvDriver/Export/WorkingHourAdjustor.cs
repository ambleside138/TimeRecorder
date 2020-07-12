using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking.Reports;

namespace TimeRecorder.Driver.CsvDriver
{
    class WorkingHourAdjustor
    {

        public static void AdjustTimes(DailyWorkRecordHeader[] dailyWorkRecordHeaders, WorkTimeRow[] rows)
        {
            // 補正
            foreach (var day in dailyWorkRecordHeaders.Where(h => h.DailyWorkTaskUnits.Any()))
            {
                var adjustTargets = rows.Where(r => r.Ymd == day.WorkYmd).ToArray();

                bool? needAdjust()
                {
                    var sum = adjustTargets.Sum(r => r.GetManHourMinutes());

                    // 実際の勤務時間とタスク時間の合計の差分が30分以上にならないようにする
                    var diff = day.CalcExpectedTotalWorkTimeSpan().TotalMinutes - sum;

                    if (diff >= 30)
                    {
                        return true;
                    }
                    else if (diff <= -30)
                    {
                        return false;
                    }
                    else
                    {
                        return null;
                    }
                };

                var isAdjusted = false;

                for (int i = 0; i < 10; i++)
                {
                    if (isAdjusted)
                        break;

                    // 実際の勤務時間とタスク時間の合計の差分が30分以上にならないようにする
                    var result = needAdjust();
                    if (result.HasValue)
                    {
                        // 超えた場合は時間が長い順に30分ずつ調整
                        foreach (var record in adjustTargets.Where(t => t.IsFixed == false).OrderByDescending(t => t.TotalMinutes))
                        {
                            if (result == true)
                            {
                                // たりない
                                record.TotalMinutes += 30;
                            }
                            else if (result == false)
                            {
                                // ながすぎ
                                record.TotalMinutes -= 30;
                            }

                            if (needAdjust().HasValue == false)
                            {
                                // 調整OK
                                isAdjusted = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }


            }
        }
    }
}
