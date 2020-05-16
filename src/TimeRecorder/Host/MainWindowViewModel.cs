using Livet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TimeRecorder.Contents;
using TimeRecorder.Contents.Configuration;
using TimeRecorder.Contents.Exporter;
using TimeRecorder.NavigationRail.ViewModels;
using TimeRecorder.Contents.WorkUnitRecorder;
using MaterialDesignThemes.Wpf;

namespace TimeRecorder.Host
{
    public class MainWindowViewModel : ViewModel
    {
        public ObservableSynchronizedCollection<IContentViewModel> Contents { get; } = new ObservableSynchronizedCollection<IContentViewModel>();

        public ObservableSynchronizedCollection<NavigationIconButtonViewModel> NavigationIconButtons { get; } = new ObservableSynchronizedCollection<NavigationIconButtonViewModel>();

        public SnackbarMessageQueue SnackMessageQueue { get; } = SnackbarService.Current.MessageQueue;
        public void Initialize()
        {
            Contents.Add(new WorkUnitRecorderViewModel());
            Contents.Add(new ExporterViewModel());
            Contents.Add(new ConfigurationViewModel());

            foreach(var c in Contents)
            {
                NavigationIconButtons.Add(c.NavigationIcon);
            }
        }

        public void CreateNewTask()
        {
            var content = Contents.OfType<WorkUnitRecorderViewModel>().First();

            content.ExecuteNewTaskDialog();
        }
    }
}
