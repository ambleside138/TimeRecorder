using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Contents.Shared;

class DateTimePickerViewModel
{
    public ReactiveProperty<DateTime> SelectedDate { get; }

    public DateTimePickerViewModel(DateTime? date)
    {
        SelectedDate = new ReactiveProperty<DateTime>(date ?? SystemClockServiceLocator.Current.Now.Date);
    }
}
