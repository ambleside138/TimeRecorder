using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Host;

public class SnackbarService
{
    private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();

    public static SnackbarService Current { get; } = new SnackbarService();

    public SnackbarMessageQueue MessageQueue { get; } = new SnackbarMessageQueue();

    public void ShowMessage(string messaage)
    {
        MessageQueue.Enqueue(messaage);
        _Logger.Info("[Message] " + messaage);
    }
}
