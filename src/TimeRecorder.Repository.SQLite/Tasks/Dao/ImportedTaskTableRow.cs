using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Tasks;

namespace TimeRecorder.Repository.SQLite.Tasks.Dao;

class ImportedTaskTableRow
{
    public string ImportKey { get; set; }

    public string Title { get; set; }

    public string Source { get; set; }

    public DateTime CreateDateTime { get; set; }

    public int WorkTaskId { get; set; }

    public static ImportedTaskTableRow FromDomainObject(int taskId, ImportedTask importSource)
    {
        return new ImportedTaskTableRow
        {
            ImportKey = importSource.ImportKey,
            Title = importSource.Title,
            Source = importSource.Source,
            WorkTaskId = taskId,
        };
    }

    public ImportedTask ConvertToDomainObject()
    {
        return new ImportedTask
        {
            ImportKey = ImportKey,
            Title = Title,
            Source = Source,
            WorkTaskId = WorkTaskId,
            CreateDateTime = CreateDateTime,
        };
    }
}
