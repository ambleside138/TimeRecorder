using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.Tracking.Reports;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Driver.CsvExporter
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
                    DateText = dateText,
                    TaskCategory = ConvertCsvCategoryText(task.TaskCategory),
                    ProductOrClient = ConvertToProductOrClient(task),
                    TaskProcess = task.WorkProcess.Title,
                    Remarks = task.Title,
                    ManHour = CalcMonHour(task.TotalWorkMinutes),
                };

                listRow.Add(row);
            }

            return listRow.ToArray();
        }

        private string ConvertCsvCategoryText(TaskCategory category)
        {
            switch (category)
            {
                case TaskCategory.UnKnown:
                    return "不明";
                case TaskCategory.Develop:
                    return "*開発作業 (ｿﾌﾄｳｪｱ)";
                case TaskCategory.ResearchAndDevelopment:
                    return "研究開発";
                case TaskCategory.Introduce:
                    return "*契約後の導入作業";
                case TaskCategory.Maintain:
                    return "保守・客先対応・障害対策";
                case TaskCategory.Other:
                    return "その他営業・事務作業等";
                default:
                    return "";
            }
        }

        private string ConvertToProductOrClient(DailyWorkTaskUnit taskUnit)
        {
            var productName = taskUnit.Product.Name;
            var clientName = taskUnit.Client.Name;

            // 製品名・案件名ともに指定がなければ作業内容のみ記載
            if(taskUnit.Product.Id.IsEmpty
                && taskUnit.Client.Id.IsEmpty)
            {
                return taskUnit.WorkProcess.Title;
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
                        return productName + "*******";
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
                    return taskUnit.Product.Name + "*******";
            }
        }

        private string CalcMonHour(int totalMinutes)
        {
            var hour = totalMinutes / 60;
            var min = totalMinutes % 60;

            if(min == 0)
            {
                return hour.ToString();
            }

            if (min >= 45)
            {
                return (hour + 1).ToString();
            }
            else if(min > 15 && min < 45)
            {
                return $"{hour}.5";
            }
            else
            {
                return hour.ToString();
            }
        }
    }
}
