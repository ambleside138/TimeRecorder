using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking.Reports;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Driver.CsvDriver
{
    class DailyWorkRecordHeaderToWorkTimeRowConverter
    {
        public WorkTimeRow[] Convert(DailyWorkRecordHeader dailyWorkRecordHeader)
        {
            var targetDate = DateTimeParser.ConvertFromYmd(dailyWorkRecordHeader.WorkYmd);
            var dateText = targetDate.Value.ToString("M月d日");

            var listRow = new List<WorkTimeRow>();

            foreach (var task in dailyWorkRecordHeader.DailyWorkTaskUnits)
            {
                var row = new WorkTimeRow
                {
                    Ymd = dailyWorkRecordHeader.WorkYmd,
                    DateText = dateText,
                    TaskCategory = ConvertCsvCategoryText(task.TaskCategory),
                    ProductOrClient = ConvertToProductOrClient(task),
                    TaskProcess = task.WorkProcess?.Title ?? "不明",
                    Remarks = task.Title,
                    TotalMinutes = task.TotalWorkMinutes,
                    IsFixed = task.IsScheduled,
                };

                listRow.Add(row);
            }

            return listRow.ToArray();
        }

        private string ConvertCsvCategoryText(TaskCategory category)
        {
            return category switch
            {
                TaskCategory.UnKnown => "不明",
                TaskCategory.Develop => "*開発作業 (ｿﾌﾄｳｪｱ)",
                TaskCategory.ResearchAndDevelopment => "研究開発",
                TaskCategory.Introduce => "*契約後の導入作業",
                TaskCategory.Maintain => "保守・客先対応・障害対策",
                TaskCategory.Other => "その他営業・事務作業等",
                _ => "",
            };
        }

        private string ConvertToProductOrClient(DailyWorkTaskUnit taskUnit)
        {
            var productName = taskUnit.Product.Name;
            var clientName = taskUnit.Client.Name;

            // 製品名・案件名ともに指定がなければ作業内容のみ記載
            if(taskUnit.Product.Id.IsEmpty
                && taskUnit.Client.Id.IsEmpty)
            {
                return "その他";
                //return taskUnit.WorkProcess.Title;
            }

            // 案件名が入っていなければ無条件で製品名を記載
            if (taskUnit.Client.Id.IsEmpty)
            {
                switch(taskUnit.TaskCategory)
                {
                    case TaskCategory.Develop:
                        return productName + " 開発";

                    case TaskCategory.Maintain:
                    case TaskCategory.Introduce:
                        return productName + " 保守";

                    default:
                        return productName + WorkTimeRow.AlertMessage;
                }
            }

            // 案件名が入っている場合、保守の場合のみ案件名を記載
            // それ以外の場合は製品名
            switch (taskUnit.TaskCategory)
            {
                case TaskCategory.Introduce:
                case TaskCategory.Maintain:
                    return clientName;

                default:
                    return taskUnit.Product.Name + WorkTimeRow.AlertMessage;
            }
        }

 
    }
}
