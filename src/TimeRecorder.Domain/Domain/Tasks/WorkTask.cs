using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Domain.Products;

namespace TimeRecorder.Domain.Domain.Tasks
{
    /// <summary>
    /// 開発・作業内容を表します
    /// </summary>
    public class WorkTask : NotificationDomainModel
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

        #region Product変更通知プロパティ

        #region ProductId変更通知プロパティ
        private Identity<Product> _ProductId;

        public Identity<Product> ProductId
        {
            get => _ProductId;
            set => RaisePropertyChangedIfSet(ref _ProductId, value);
        }
        #endregion

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

        #region Remarks変更通知プロパティ
        private string _Remarks;

        public string Remarks
        {
            get => _Remarks;
            set => RaisePropertyChangedIfSet(ref _Remarks, value);
        }
        #endregion

        public TaskProgress TaskProgress { get; private set; }

        #region IsPlaned変更通知プロパティ
        private bool _IsPlaned = true;

        public bool IsPlaned
        {
            get => _IsPlaned;
            set => RaisePropertyChangedIfSet(ref _IsPlaned, value);
        }
        #endregion
        
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

        // VSの場合、「クイックアクションとリファクタリング」からコンストラクタコードの生成が可能
        public WorkTask(Identity<WorkTask> id, string title, TaskCategory taskCategory, Identity<Product> productid, Identity<Client> ClientId, Identity<WorkProcess> processId, string remarks, TaskProgress taskProgress)
        {
            Id = id;
            _Title = title;
            _TaskCategory = taskCategory;
            _ProductId = productid;
            _ClientId = ClientId;
            _ProcessId = processId;
            _Remarks = remarks;
            TaskProgress = taskProgress;
            
        }

        private WorkTask() { }
    }
}
