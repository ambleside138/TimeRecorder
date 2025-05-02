using Livet;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Messaging.Windows;

namespace TimeRecorder.Contents.WorkUnitRecorder;
internal class TimeCardLinkEditWindowViewModel : ViewModel
{
    public ReactivePropertySlim<string> Url { get; }

	public TimeCardLinkEditWindowViewModel(string url)
	{
		Url = new ReactivePropertySlim<string>(url);
	}

    public void Regist()
    {
        Messenger.Raise(new ModalWindowActionMessage("RegistKey") { DialogResult = true });
    }

}
