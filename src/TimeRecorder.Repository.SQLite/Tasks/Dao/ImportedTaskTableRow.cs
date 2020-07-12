using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;

namespace TimeRecorder.Repository.SQLite.Tasks.Dao
{
    class ImportedTaskTableRow
    {
        public string ImportKey { get; set; }

        public string Title { get; set; }

        public string Source { get; set; }

        public DateTime CreateDateTime { get; set; }

        public static ImportedTaskTableRow FromDomainObject(WorkTask workTask)
        {
            return new ImportedTaskTableRow
            {
                ImportKey = workTask.ImportSource.Key,
                Title = workTask.Title,
                Source = workTask.ImportSource.Kind,
            };
        }

        public ImportedTask ConvertToDomainObject()
        {
            return new ImportedTask
            {
                ImportKey = ImportKey,
                Title = Title,
                Source = Source,
                CreateDateTime = CreateDateTime,
            };
        }
    }
}
