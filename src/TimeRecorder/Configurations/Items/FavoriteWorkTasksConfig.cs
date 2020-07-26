using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;
using TimeRecorder.Domain.Domain.Tasks;

namespace TimeRecorder.Configurations.Items
{
    /// <summary>
    /// お気に入りのWorkTaskリスト設定を表します
    /// </summary>
    class FavoriteWorkTasksConfig : ConfigItemBase
    {
        public FavoriteWorkTask[] FavoriteWorkTasks { get; set; }
        internal override ConfigKey Key => ConfigKey.FavoriteWorkTask;
    }

    public class FavoriteWorkTask : IEquatable<FavoriteWorkTask>
    {
        private readonly Guid _Guid = Guid.NewGuid();

        public string ButtonTitle { get; set; }

        public string Title { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public int ProductId { get; set; }

        public int ClientId { get; set; }

        public int ProcessId { get; set; }

        public WorkTask ConvertToDomainModel()
        {
            var obj = WorkTask.ForNewFavorite();
            obj.Title = Title;
            obj.TaskCategory = TaskCategory;
            obj.ProductId  = new Domain.Identity<Domain.Domain.Products.Product>(ProductId);
            obj.ClientId = new Domain.Identity<Domain.Domain.Clients.Client>(ClientId);
            obj.ProcessId = new Domain.Identity<Domain.Domain.WorkProcesses.WorkProcess>(ProcessId);

            return obj;
        }

        public static FavoriteWorkTask FromDomainObject(WorkTask workTask)
        {
            return new FavoriteWorkTask
            {
                Title = workTask.Title,
                TaskCategory = workTask.TaskCategory,
                ProductId = workTask.ProductId.Value,
                ClientId = workTask.ClientId.Value,
                ProcessId = workTask.ProcessId.Value,
            };
        }

        public bool Equals([AllowNull] FavoriteWorkTask other)
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
