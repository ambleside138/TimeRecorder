using Livet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TimeRecorder.Contents;
using TimeRecorder.Contents.Configuration;
using TimeRecorder.Contents.Exporter;
using TimeRecorder.NavigationRail;
using TimeRecorder.Contents.WorkUnitRecorder;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using TimeRecorder.Helpers;
using TimeRecorder.Configurations;
using TimeRecorder.Configurations.Items;
using TimeRecorder.Contents.Archive;
using TimeRecorder.Contents.Todo;

namespace TimeRecorder.Host;

public class MainWindowViewModel : ViewModel
{
    public ObservableSynchronizedCollection<IContentViewModel> Contents { get; } = new ObservableSynchronizedCollection<IContentViewModel>();

    public ObservableSynchronizedCollection<NavigationIconButtonViewModel> NavigationIconButtons { get; } = new ObservableSynchronizedCollection<NavigationIconButtonViewModel>();

    public SnackbarMessageQueue SnackMessageQueue { get; } = SnackbarService.Current.MessageQueue;

    private readonly MainModel _MainModel = new();

    public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();

    private MainWindowViewModel()
    {
        TransitionHelper.Current.SetMessanger(Messenger);

        var message = _MainModel.CheckHealth();
        if (string.IsNullOrEmpty(message) == false)
            SnackbarService.Current.ShowMessage(message);

        SetupTheme();
    }

    private static void SetupTheme()
    {
        var theme = UserConfigurationManager.Instance.GetConfiguration<ThemeConfig>(ConfigKey.Theme);
        if (theme != null)
        {
            ThemeService.ApplyFromName(theme.ThemeName);
        }
    }

    public void Initialize()
    {
        if (UserConfigurationManager.Instance.GetConfiguration<UseTodoConfig>(ConfigKey.UseTodo)?.UseTodo ?? true)
        {
            Contents.Add(new TodoViewModel());
        }

        Contents.Add(new WorkUnitRecorderViewModel());
        Contents.Add(new ArchiveManagerViewModel());
        Contents.Add(new ExporterViewModel());
        Contents.Add(new ConfigurationViewModel());

        foreach (var c in Contents)
        {
            NavigationIconButtons.Add(c.NavigationIcon);
        }
    }
}
