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

namespace TimeRecorder.Contents.WorkUnitRecorder.Editor
{
    class WorkTaskEditDialogViewModel : ViewModel
    {
        public WorkTaskViewModel TaskCardViewModel { get; }

        public ReactivePropertySlim<bool> IsEditMode { get; } = new ReactivePropertySlim<bool>(false);

        private readonly WorkTaskEditDialogModel _WorkTaskEditDialogModel = new WorkTaskEditDialogModel();

        public WorkProcess[] Processes { get; }

        public Client[] Clients { get; }

        public Product[] Products { get; }

        public bool NeedDelete { get; set; } = false;

        public bool NeedStart { get; set; } = false;

        public WorkTaskEditDialogViewModel()
            : this(WorkTask.ForNew()) { }

        public WorkTaskEditDialogViewModel(WorkTask model)
        {
            IsEditMode.Value = model.Id.IsTemporary == false;

            Processes = _WorkTaskEditDialogModel.GetProcesses();
            Clients = _WorkTaskEditDialogModel.GetClients();
            Products = _WorkTaskEditDialogModel.GetProducts();

            TaskCardViewModel = new WorkTaskViewModel(model, Processes, Clients, Products);
        }

        public void Regist()
        {
            if(TaskCardViewModel.TryValidate())
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

    }
}
