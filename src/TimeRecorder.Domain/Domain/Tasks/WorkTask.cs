using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.Calendar;

namespace TimeRecorder.Domain.Domain.Tasks
{
    /// <summary>
    /// 開発・作業内容を表します
    /// </summary>
    public class WorkTask : Entity<WorkTask>
    {
        public Identity<WorkTask> Id { get; private set; }

        #region Title変更通知プロパティ
        private string _Title;

        public string Title
        {
            get => _Title;
            set => RaisePropertyChangedIfSet(ref _Title, value);
        }
        #endregion

        #region TaskCategory変更通知プロパティ
        private TaskCategory _TaskCategory;

        public TaskCategory TaskCategory
        {
            get => _TaskCategory;
            set => RaisePropertyChangedIfSet(ref _TaskCategory, value);
        }
        #endregion

        #region ProductId変更通知プロパティ
        private Identity<Product> _ProductId;

        public Identity<Product> ProductId
        {
            get => _ProductId;
            set => RaisePropertyChangedIfSet(ref _ProductId, value);
        }
        #endregion

        #region ClientId変更通知プロパティ
        private Identity<Client> _ClientId;

        public Identity<Client> ClientId
        {
            get => _ClientId;
            set => RaisePropertyChangedIfSet(ref _ClientId, value);
        }
        #endregion

        #region ProcessId変更通知プロパティ
        private Identity<WorkProcess> _ProcessId;

        public Identity<WorkProcess> ProcessId
        {
            get => _ProcessId;
            set => RaisePropertyChangedIfSet(ref _ProcessId, value);
        }
        #endregion

        public TaskSource TaskSource { get; private set; }

        public bool IsTemporary => TaskSource.IsTemporary();

        public bool IsScheduled => TaskSource == TaskSource.Schedule;

        public bool IsCompleted { get; private set; } = false;

        public static WorkTask ForNew()
        {
            return new WorkTask 
            { 
                Id = Identity<WorkTask>.Temporary, 
                ClientId = Identity<Client>.Empty, 
                ProductId = Identity<Product>.Empty, 
                ProcessId = Identity<WorkProcess>.Empty, 
            };
        }

        public static WorkTask ForNewFavorite()
        {
            var t = ForNew();
            t.TaskSource = TaskSource.Favorite;

            return t;
        }

        public static WorkTask FromScheduledEvent(ScheduledEvent scheduledEvent)
        {
            var workTask = ForNew();
            workTask.Title = scheduledEvent.Title;
            workTask.TaskCategory = TaskCategory.Develop;
            workTask.TaskSource = TaskSource.Schedule;

            return workTask;
        }

        // VSの場合、「クイックアクションとリファクタリング」からコンストラクタコードの生成が可能
        public WorkTask(
            Identity<WorkTask> id, 
            string title, 
            TaskCategory taskCategory, 
            Identity<Product> productid, 
            Identity<Client> ClientId, 
            Identity<WorkProcess> processId, 
            TaskSource source,
            bool isCompleted)
        {
            Id = id;
            _Title = title;
            _TaskCategory = taskCategory;
            _ProductId = productid;
            _ClientId = ClientId;
            _ProcessId = processId;
            TaskSource = source;
            IsCompleted = isCompleted;
        }

        public void Complete()
        {
            IsCompleted = true;
        }

        public void ReStart()
        {
            IsCompleted = false;
        }

        protected override IEnumerable<object> GetIdentityValues()
        {
            yield return Id;
        }

        private WorkTask() { }
    }
}
