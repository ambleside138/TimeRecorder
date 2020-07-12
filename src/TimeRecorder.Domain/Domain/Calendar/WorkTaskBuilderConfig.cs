using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Calendar
{
    public class WorkTaskBuilderConfig
    {
        public List<EventMapper> EventMappers { get; set; } = new List<EventMapper>();

        public List<TitleMapper> TitleMappers { get; set; } = new List<TitleMapper>();
    }

    public class EventMapper
    {
        public string EventKind { get; set; }

        public int WorkProcessId{ get; set; }
    }

    /// <summary>
    /// 旧設定定義
    /// </summary>
    public class TitleMapper
    {
        public string ScheduleTitle { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public int ProductId { get; set; }

        public int WorkProcessId { get; set; }

        public ScheduleTitleMap ConvertToScheduleTitleMap()
        {
            return new ScheduleTitleMap
            {
                ScheduleTitle = ScheduleTitle,
                TaskCategory = TaskCategory,
                ProductId = ProductId,
                WorkProcessId = WorkProcessId,
            };
        }
    }

    /// <summary>
    /// 新設定定義
    /// </summary>
    public class ScheduleTitleMap : IEquatable<ScheduleTitleMap>
    {
        private readonly Guid _Guid = Guid.NewGuid();

        // 取込対象とするタイトル
        public string ScheduleTitle { get; set; }

        // タスクのタイトル
        public string MapTitle { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public int ProductId { get; set; }

        public int ClientId { get; set; }

        public int WorkProcessId { get; set; }

        public WorkTask ConvertToDomainModel()
        {
            var obj = WorkTask.ForNew();
            obj.Title = ScheduleTitle;
            obj.TaskCategory = TaskCategory;
            obj.ProductId = new Identity<Product>(ProductId);
            obj.ClientId = new Identity<Client>(ClientId);
            obj.ProcessId = new Identity<WorkProcess>(WorkProcessId);
            obj.IsTemporary = true;

            return obj;
        }

        public static ScheduleTitleMap FromDomainObject(WorkTask workTask)
        {
            return new ScheduleTitleMap
            {
                ScheduleTitle = workTask.Title,
                TaskCategory = workTask.TaskCategory,
                ProductId = workTask.ProductId.Value,
                ClientId = workTask.ClientId.Value,
                WorkProcessId = workTask.ProcessId.Value,
            };
        }

        public bool Equals([AllowNull] ScheduleTitleMap other)
        {
            if (other == null)
                return false;

            return _Guid == other._Guid;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_Guid);
        }
    }
}
