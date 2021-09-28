using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.NavigationRail;

namespace TimeRecorder.Contents.Todo
{
    /// <summary>
    /// TodoリストをあらわすViewModelです
    /// </summary>
    public class TodoListNavigationItemViewModel : NavigationIconButtonViewModel
    {

        public bool CanEditTitle { get; init; }

        public ReadOnlyReactivePropertySlim<int> TaskCount { get; }

        public TodoListNavigationItemViewModel(TodoList todoList)
        {
            TodoList = todoList;

            IconKey = todoList.IconKey ?? PackIconKind.FormatListBulleted.ToString();

            Title = todoList.Title;
            CanEditTitle = todoList.Id.IsFixed == false;

            TaskCount = TodoList.ObserveProperty(i => i.FilteredCount)
                                .ToReadOnlyReactivePropertySlim(initialValue:todoList.FilteredCount)
                                .AddTo(CompositeDisposable);
        }

        public TodoList TodoList { get; }

               
        public void FocusToAdditionTextbox() => Messenger.Raise(new Livet.Messaging.InteractionMessage("FocusToTextBox"));

        public void FocusToTitleTextbox() => Messenger.Raise(new Livet.Messaging.InteractionMessage("FocusToTitleTextBox"));

    }
}
