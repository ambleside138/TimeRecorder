using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tracking
{
    /// <summary>
    /// 作業時間（編集用）ViewModel
    /// </summary>
    class WorkingTimeViewModel : ViewModel
    {
        public WorkingTimeRange DomainModel { get; }

        public ReactivePropertySlim<DateTime> TargetDate { get; }

        public ReactiveProperty<string> StartTimeText { get; }

        public ReactiveProperty<string> EndTimeText { get; }

        public WorkingTimeViewModel(WorkingTimeRange workingTimeRange)
        {
            DomainModel = workingTimeRange;

            TargetDate = new ReactivePropertySlim<DateTime>(DomainModel.TimePeriod.StartDateTime);

            StartTimeText = new ReactiveProperty<string>(DomainModel.TimePeriod.StartDateTime.ToString("HHmm"))
                            .SetValidateNotifyError(x => string.IsNullOrEmpty(x) ? "入力必須です" : null)
                            .SetValidateNotifyError(x => DateTimeParser.ConvertFromHHmm(x).HasValue == false ? "HHmm形式で入力してください" : null)
                            .AddTo(CompositeDisposable);

            EndTimeText = new ReactiveProperty<string>(DomainModel.TimePeriod.EndDateTime?.ToString("HHmm") ?? "")
                            .SetValidateNotifyError(x => string.IsNullOrEmpty(x) == false && DateTimeParser.ConvertFromHHmm(x).HasValue == false ? "HHmm形式で入力してください" : null)
                            .AddTo(CompositeDisposable);
        }


        public bool TryValidate()
        {
            var result = true;

            StartTimeText.ForceValidate();
            result &= StartTimeText.HasErrors == false;

            EndTimeText.ForceValidate();
            result &= EndTimeText.HasErrors == false;

            DomainModel.EditTimes(GetStartDateTime(), GetEndDateTime());

            return result;
        }


        public DateTime GetStartDateTime() => DateTimeParser.ConvertFromYmdHHmmss(TargetDate.Value.ToYmd(), StartTimeText.Value + "00").Value;
        public DateTime? GetEndDateTime() => DateTimeParser.ConvertFromYmdHHmmss(TargetDate.Value.ToYmd(), EndTimeText.Value + "00");

    }
}
