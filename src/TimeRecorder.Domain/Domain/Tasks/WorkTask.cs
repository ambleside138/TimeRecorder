using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Hospitals;
using TimeRecorder.Domain.Domain.Processes;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

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
        private Product _Product;

        public Product Product
        {
            get => _Product;
            set => RaisePropertyChangedIfSet(ref _Product, value);
        }
        #endregion

        #region HospitalId変更通知プロパティ
        private Identity<Hospital> _HospitalId;

        public Identity<Hospital> HospitalId
        {
            get => _HospitalId;
            set => RaisePropertyChangedIfSet(ref _HospitalId, value);
        }
        #endregion

        #region ProcessId変更通知プロパティ
        private Identity<Process> _ProcessId;

        public Identity<Process> ProcessId
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
            return new WorkTask { Id = Identity<WorkTask>.Temporary, HospitalId = Identity<Hospital>.Empty, ProcessId = Identity<Process>.Empty, };
        }

        // VSの場合、「クイックアクションとリファクタリング」からコンストラクタコードの生成が可能
        public WorkTask(Identity<WorkTask> id, string title, TaskCategory taskCategory, Product product, Identity<Hospital> hospitalId, Identity<Process> processId, string remarks, TaskProgress taskProgress)
        {
            Id = id;
            _Title = title;
            _TaskCategory = taskCategory;
            _Product = product;
            _HospitalId = hospitalId;
            _ProcessId = processId;
            _Remarks = remarks;
            TaskProgress = taskProgress;
            
        }

        private WorkTask() { }

        public WorkingTimeRange StartTask()
        {
            return WorkingTimeRange.ForNew(Id);
        }
    }
}
