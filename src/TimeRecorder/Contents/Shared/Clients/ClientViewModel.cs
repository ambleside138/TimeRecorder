using Livet;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Clients;

namespace TimeRecorder.Contents.Shared.Clients;
internal class ClientViewModel : ViewModel
{
    public ReactiveProperty<string> Name { get; }
    public ReactiveProperty<string> KanaName { get; }
    //public ReactiveProperty<bool> Invalid { get; }
    //public ReactiveProperty<bool> NeedPinned { get; }

    public ClientViewModel(Client model)
    {
        Name = new ReactiveProperty<string>(model.Name);
        KanaName = new ReactiveProperty<string>(model.KanaName);
    }
}
