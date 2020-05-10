using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.UseCase.Tracking;

namespace TimeRecorder.Repository.SQLite.Tracking
{
    public class SQLiteWorkingTimeQueryService : IWorkingTimeQueryService
    {
        public WorkingTimeForTimelineDto[] SelectByYmd(string ymd)
        {
            #region SQL
            const string sql = @"
SELECT
  time.id as WorkingTimeId
  , time.taskid as WorkTaskId
  , time.starttime as starttime
  , time.endtime as endtime
  , task.title as title
  , task.taskcategory as taskcategory
FROM
  workingtimes time
INNER JOIN
  worktasks task ON task.id = time.taskid
WHERE
  time.ymd = @ymd
";
            #endregion

            var list = new List<WorkingTimeForTimelineDto>();

            RepositoryAction.Query(c =>
            {
                list.AddRange(c.Query<WorkingTimeForTimelineDto>(sql, new { ymd }));
            });

            return list.OrderBy(i => i.StartTime).ToArray();
        }
    }
}
