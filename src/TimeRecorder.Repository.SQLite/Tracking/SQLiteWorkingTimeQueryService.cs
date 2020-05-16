using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.Utility;

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
  , task.remarks as remarks
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
                var listRow = c.Query<WorkingTimeForTimelineTableRow>(sql, new { ymd });

                list.AddRange(listRow.Select(r => r.ConvertToDto()));
            });

            return list.OrderBy(i => i.StartDateTime).ToArray();
        }

        private class WorkingTimeForTimelineTableRow
        {
            public int WorkingTimeId { get; set; }

            public int WorkTaskId { get; set; }

            public string StartTime { get; set; }

            public string EndTime { get; set; }

            public string Title { get; set; }

            public TaskCategory  TaskCategory { get; set; }

            public string Remarks { get; set; }

            public WorkingTimeForTimelineDto ConvertToDto()
            {
                
                return new WorkingTimeForTimelineDto
                {
                    WorkingTimeId = new Domain.Utility.Identity<Domain.Domain.Tracking.WorkingTimeRange>(WorkingTimeId),
                    WorkTaskId = new Domain.Utility.Identity<Domain.Domain.Tasks.WorkTask>(WorkTaskId),
                    StartDateTime = DateTimeParser.ConvertFromHHmmss(StartTime).Value,
                    EndDateTime = DateTimeParser.ConvertFromHHmmss(EndTime),
                    TaskTitle = Title,
                    TaskCategory = TaskCategory,
                    TaskRemarks = Remarks,
                };
            }
        }
    }
}
