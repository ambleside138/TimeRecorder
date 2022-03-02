using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Contents.Shared;

class ConfirmDialogViewModel
{
    public ReactivePropertySlim<string> Title { get; }

    public ReactivePropertySlim<string> Description { get; }

    public ReactivePropertySlim<string> YesButtonText { get; }
    public ReactivePropertySlim<string> NoButtonText { get; }

    public ConfirmDialogViewModel(string title, string description, string yesButtonText, string noButtonText)
    {
        Title = new ReactivePropertySlim<string>(title);
        Description = new ReactivePropertySlim<string>(description);
        YesButtonText = new ReactivePropertySlim<string>(yesButtonText);
        NoButtonText = new ReactivePropertySlim<string>(noButtonText);
    }


}
