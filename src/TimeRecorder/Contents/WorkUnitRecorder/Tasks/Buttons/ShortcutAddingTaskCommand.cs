using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Configurations.Items;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks.Buttons
{
    /// <summary>
    /// お気に入りタスクからタスク追加するコマンドを表します
    /// </summary>
    class ShortcutAddingTaskCommand : IAddingTaskCommand
    {
        private readonly WorkTaskRegistor _WorkTaskRegistor = new();
        private readonly FavoriteWorkTask _WorkTask;

        public ShortcutAddingTaskCommand(FavoriteWorkTask workTask)
        {
            _WorkTask = workTask;
        }

        public void Invoke()
        {
            var domainObj = _WorkTask.ConvertToDomainModel();
            _WorkTaskRegistor.AddWorkTask(domainObj, needStart: true);
        }
    }
}
