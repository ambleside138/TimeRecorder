using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tracking;

namespace TimeRecorder.Repository.SQLite.Tracking
{
    public class SQLiteDailyWorkRecordQueryService : IDailyWorkRecordQueryService
    {
        #region SQL
        const string _Query = @"
SELECT
  time.id 
  , time.taskid
  , time.starttime
  , time.endtime
  , task.title
  , task.taskcategory
FROM
  workingtimes time
INNER JOIN
  worktasks task ON task.id = time.taskid
WHERE
  time.ymd = @ymd
";
        #endregion

        public DailyWorkRecordHeader SelectByYmd(string ymd)
        {
            //RepositoryAction.Query(c =>
            //{
            //    results = c.Query(_Query, new { ymd })
            //                 .Select(r => r.ConvertToDomainObject())
            //                 .OrderBy(r => r.StartDateTime)
            //                 .ToArray();
            //});

            return new DailyWorkRecordHeader();
        }
    }
}
