using Livet;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Contents.WorkUnitRecorder.Tasks.Editor;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Host;
using MaterialDesignThemes.Wpf;
using Livet.Behaviors.Messaging.Windows;
using Livet.Messaging.Windows;
using TimeRecorder.Messaging.Windows;
using TimeRecorder.Domain.Domain.Segments;

namespace TimeRecorder.Contents.WorkUnitRecorder.Editor;

// 設定画面で継承して利用している

class WorkTaskEditDialogViewModel : ViewModel
{
    public WorkTaskViewModel TaskCardViewModel { get; }

    public ReactivePropertySlim<bool> IsEditMode { get; } = new ReactivePropertySlim<bool>(false);

    private readonly WorkTaskEditDialogModel _WorkTaskEditDialogModel = new();

    public WorkProcess[] Processes { get; }

    public ReactivePropertySlim<Client[]> Clients { get; }

    public Product[] Products { get; }

    public Segment[] Segments { get; }

    public bool NeedDelete { get; set; } = false;

    public bool NeedStart { get; set; } = false;

    public ReactivePropertySlim<bool> ShowQuickStartButton { get; }

    public ReactivePropertySlim<bool> ShowDeleteButton { get; }

    public ReactivePropertySlim<bool> ShowClientEditorArea { get; } = new ReactivePropertySlim<bool>(false);

    public ReactivePropertySlim<Client[]> SourceClients { get; } = new();
    public ReactivePropertySlim<Client> SelectedClientSource { get; } = new();

    public ReactivePropertySlim<string> ClientNameKana { get; } = new();

    private readonly string _timeCardUrl;

    public WorkTaskEditDialogViewModel(string timeCardUrl = "")
        : this(WorkTask.ForNew(), timeCardUrl) { }

    public WorkTaskEditDialogViewModel(WorkTask model, string timeCardUrl = "")
    {
        IsEditMode.Value = model.Id.IsTemporary == false;

        Processes = _WorkTaskEditDialogModel.GetProcesses(model.ProcessId);
        Clients = new(_WorkTaskEditDialogModel.GetClients());
        Products = _WorkTaskEditDialogModel.GetProducts(model.ProductId);
        Segments = _WorkTaskEditDialogModel.GetSegments();

        TaskCardViewModel = new WorkTaskViewModel(model, Processes, Clients.Value, Products, Segments);

        ShowQuickStartButton = new ReactivePropertySlim<bool>(IsEditMode.Value == false);
        ShowDeleteButton = new ReactivePropertySlim<bool>(IsEditMode.Value);

        _timeCardUrl = timeCardUrl;
    }

    public void Regist()
    {
        if (TaskCardViewModel.TryValidate())
        {
            Messenger.Raise(new ModalWindowActionMessage("RegistKey") { DialogResult = true });
        }
    }

    public void RegistAndStart()
    {
        if (TaskCardViewModel.TryValidate())
        {
            NeedStart = true;
            Messenger.Raise(new ModalWindowActionMessage("RegistKey") { DialogResult = true });
        }
    }

    public void Delete()
    {
        NeedDelete = true;
        Messenger.Raise(new ModalWindowActionMessage("RegistKey") { DialogResult = true });
    }

    public async void ToggleClientEditArea()
    {
        ShowClientEditorArea.Value = !ShowClientEditorArea.Value;

        if(ShowClientEditorArea.Value)
        {
            SourceClients.Value = await _WorkTaskEditDialogModel.GetClientSourceAsync(_timeCardUrl);
        }
    }

    public void RegistClient()
    {
        _WorkTaskEditDialogModel.PutClient( Client.ForSource( SelectedClientSource.Value.Name, ClientNameKana.Value ));

        Clients.Value = _WorkTaskEditDialogModel.GetClients();
        ShowClientEditorArea.Value = false;

        SelectedClientSource.Value = Client.Empty;
        ClientNameKana.Value = "";
    }
}
