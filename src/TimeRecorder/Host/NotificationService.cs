using Microsoft.Toolkit.Uwp.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TimeRecorder.Contents.WorkUnitRecorder;
using TimeRecorder.Domain.UseCase.Tasks;
using Windows.Foundation.Collections;

namespace TimeRecorder.Host
{
    public class NotificationService
    {
        private const string _ActionTypeCode = "action";

        private const string _ActionTypeStartTask = "starttask";

        private const string _ActionTypeStartLunch = "startlunch";

        private const string _SelectionTaskKey = "task";

        private const string _AttributeText = "TimeRecorder ⏰ 工数管理";

        public static NotificationService Current => new();

        private NotificationService()
        {

        }

        public void Setup()
        {
            // Listen to notification activation
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                // Obtain the arguments from the notification
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                // Obtain any user input (text boxes, menu selections) from the notification
                ValueSet userInput = toastArgs.UserInput;

                // Need to dispatch to UI thread if performing UI operations
                Application.Current.Dispatcher.Invoke(delegate
                {
                    if(args.TryGetValue(_ActionTypeCode, out string value))
                    {
                        switch(value)
                        {
                            case _ActionTypeStartTask:
                                userInput.TryGetValue(_SelectionTaskKey, out object key);
                                StartSelectedWorkTask(key?.ToString());
                                break;

                            case _ActionTypeStartLunch:
                                StartBreak();
                                break;
                        }
                    }
                    else
                    {
                        ((MainWindow)Application.Current.MainWindow).ShowWindow();
                    }
                });
            };
        }

        private void StartSelectedWorkTask(string workTaskId)
        {
            var contents = MainWindowViewModel.Instance.Contents.OfType<WorkUnitRecorderViewModel>().First();

            var selectedTask = contents.PlanedTaskCards.FirstOrDefault(c => c.Dto.TaskId.Value.ToString() == workTaskId);
            selectedTask?.StartOrStopWorkTask();
        }

        private void StartBreak()
        {
            var contents = MainWindowViewModel.Instance.Contents.OfType<WorkUnitRecorderViewModel>().First();
            contents.StopCurrentTask();
        }

        public void Uninstall()
        {
            ToastNotificationManagerCompat.History.Clear();
            ToastNotificationManagerCompat.Uninstall();
        }

        public void Info(string title, string content)
               => InfoCore(title, content).Show();

        private ToastContentBuilder InfoCore(string title, string content)
        {
            return new ToastContentBuilder()
                 .AddText(title)
                 .AddText(content)
                 .AddAttributionText(_AttributeText);
        }

        public void ShowTaskStarterInteractor(IEnumerable<WorkTaskWithTimesDto> workTasks, string content)
        {
            var title = "お知らせ";
            var selector = new ToastSelectionBox(_SelectionTaskKey);
            
            // 表示できる項目数に上限があるよう
            foreach(var itm in workTasks.Take(5).Select(t => new ToastSelectionBoxItem(t.TaskId.Value.ToString(), $"[{t.ProcessName}] {t.Title}")))
            {
                selector.Items.Add(itm);
            }

            selector.DefaultSelectionBoxItemId = selector.Items.FirstOrDefault()?.Id ?? "";

            new ToastContentBuilder()
                 .SetToastScenario(ToastScenario.Reminder)
                 .AddText(title)
                 .AddText(content)
                 .AddAttributionText(_AttributeText)
                 .AddToastInput(selector)
                 .AddButton(new ToastButton()
                                .SetContent("開始")
                                .AddArgument(_ActionTypeCode, _ActionTypeStartTask))
                 .AddButton(new ToastButtonSnooze())
                 .Show();
        }

        public void ShowLunchStartInteractor()
        {
            var title = "お知らせ";
            var content = "お昼休憩の時間になりました";

            new ToastContentBuilder()
                 .SetToastScenario(ToastScenario.Reminder)
                 .AddText(title)
                 .AddText(content)
                 .AddAttributionText(_AttributeText)
                 .AddButton(new ToastButton()
                                .SetContent("休む")
                                .AddArgument(_ActionTypeCode, _ActionTypeStartLunch))
                 .AddButton(new ToastButtonSnooze())
                 .Show();
        }
    }
}
