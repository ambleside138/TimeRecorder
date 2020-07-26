using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain;
using TimeRecorder.Repository.SQLite.Tasks.Dao;

namespace TimeRecorder.Repository.SQLite.Tasks
{
    public class SQLiteWorkTaskRepository : IWorkTaskRepository
    {
        public WorkTask Add(WorkTask task)
        {
            return AddCore(task, null);
        }

        public WorkTask AddForSchedule(WorkTask task, ImportedTask workTaskImportSource)
        {

            return AddCore(task, workTaskImportSource);
        }

        private WorkTask AddCore(WorkTask task, ImportedTask workTaskImportSource)
        {
            var row = WorkTaskTableRow.FromDomainObject(task);

            RepositoryAction.Transaction((c, t) =>
            {
                var dao = new WorkTaskDao(c, t);
                var id = dao.Insert(row);

                // ID採番結果
                row.Id = id;

                // スケジュールからの取込の場合は取込歴にも残す
                if (task.IsScheduled)
                {
                    var importDao = new ImportedTaskDao(c, t);
                    importDao.Insert(ImportedTaskTableRow.FromDomainObject(id, workTaskImportSource));
                }
            });

            return WorkTaskFactory.Create(row, task.IsCompleted);
        }

        public void Delete(Identity<WorkTask> identity)
        {
            RepositoryAction.Transaction((c, t) =>
            {
                new WorkTaskDao(c, t).Delete(identity.Value);
                new WorkTaskCompletedDao(c, t).DeleteByWorkTaskId(identity.Value);
            });
        }

        public void Edit(WorkTask task)
        {
            RepositoryAction.Transaction((c, t) =>
            {
                var row = WorkTaskTableRow.FromDomainObject(task);
                var dao = new WorkTaskDao(c, t);
                var compDao = new WorkTaskCompletedDao(c, t);
                dao.Update(row);

                if(task.IsCompleted)
                {
                    compDao.InsertIfNotExist(task.Id.Value);
                }
                else
                {
                    compDao.DeleteByWorkTaskId(task.Id.Value);
                }
            });
        }

        public WorkTask SelectById(Identity<WorkTask> identity)
        {
            WorkTask results = null;

            RepositoryAction.Query(c =>
            {
                var dao = new WorkTaskDao(c, null);
                var compDao = new WorkTaskCompletedDao(c, null);
                var exist = compDao.IsCompleted(identity.Value);

                results = WorkTaskFactory.Create(dao.SelectById(identity.Value), exist);
            });

            return results;
        }

        public ImportedTask[] SelectByImportKeys(string[] importKeys)
        {
            ImportedTask[] results = null;

            RepositoryAction.Query(c =>
            {
                var dao = new ImportedTaskDao(c, null);

                results = dao.SelectByImportKeys(importKeys).Select(d => d.ConvertToDomainObject()).ToArray();
            });

            return results;
        }
    }
}
