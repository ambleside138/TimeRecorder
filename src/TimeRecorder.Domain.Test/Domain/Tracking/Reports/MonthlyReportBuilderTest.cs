using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Domain.Tracking.Reports;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Test.Domain.Tracking.Reports
{
    [TestFixture]
    class MonthlyReportBuilderTest
    {
        private MonthlyReportBuilder _MonthlyReportBuilder;

        private YmdString _Ymd0501 = new YmdString("20200501");
        private YmdString _Ymd0510 = new YmdString("20200510");
        private YmdString _Ymd0531 = new YmdString("20200531");

        private WorkProcess _Process01 = new WorkProcess(new Identity<WorkProcess>(1), "プロセス1");
        private WorkProcess _Process02 = new WorkProcess(new Identity<WorkProcess>(2), "プロセス2");
        private WorkProcess _Process03 = new WorkProcess(new Identity<WorkProcess>(3), "プロセス3");

        private Product _Product01 = new Product(new Identity<Product>(1), "製品1", "1");
        private Product _Product02 = new Product(new Identity<Product>(2), "製品2", "2");
        private Product _Product03 = new Product(new Identity<Product>(3), "製品3", "3");

        private Client _Client01 = new Client(new Identity<Client>(1), "田中", "タナカ");
        private Client _Client02 = new Client(new Identity<Client>(2), "佐藤", "サトウ");
        private Client _Client03 = new Client(new Identity<Client>(3), "青木", "アオキ");


        [SetUp]
        public void Setup()
        {
            _MonthlyReportBuilder = new MonthlyReportBuilder(new YearMonth(2020, 5));
        }

        [Test]
        public void データなし()
        {
            var list = _MonthlyReportBuilder.Build(new DailyWorkResults { WorkingTimeRecordForReports = new WorkingTimeRecordForReport[0] });
            Assert.IsTrue(list.Length == 0);
        }

        [Test]
        public void 同日複数レコードあり_同一種別含まない()
        {
            var records = new WorkingTimeRecordForReport[]
            {
                new WorkingTimeRecordForReport
                {
                    Ymd = _Ymd0501,
                    TaskCategory = TaskCategory.Develop,
                    WorkProcess = _Process01,
                    Product = _Product01,
                    Client = _Client01,
                    StartDateTime = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "100000").Value,
                    EndDateTime   = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "103000").Value,
                    Title = "サンプル作業１",
                    WorkingTimeId = new Identity<WorkingTimeRange>(1),
                    WorkTaskId = new Identity<WorkTask>(1),
                },
                new WorkingTimeRecordForReport
                {
                    Ymd = _Ymd0501,
                    TaskCategory = TaskCategory.Develop,
                    WorkProcess = _Process02,
                    Product = _Product01,
                    Client = _Client01,
                    StartDateTime = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "103000").Value,
                    EndDateTime   = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "113000").Value,
                    Title = "サンプル作業２",
                    WorkingTimeId = new Identity<WorkingTimeRange>(2),
                    WorkTaskId = new Identity<WorkTask>(2),
                },
            };

            var list = _MonthlyReportBuilder.Build(new DailyWorkResults { WorkingTimeRecordForReports = records });

            Assert.IsTrue(list.Length == 31);
            Assert.IsTrue(list[0].WorkYmd == _Ymd0501.Value);
            Assert.IsTrue(list[0].DailyWorkTaskUnits.Count == 2);

            Assert.IsTrue(list[0].DailyWorkTaskUnits[0].TaskId.Value == 1);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[0].WorkingTimeRanges.Count == 1);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[0].TotalWorkMinutes == 30);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[0].Title == "サンプル作業１");

            Assert.IsTrue(list[0].DailyWorkTaskUnits[1].TaskId.Value == 2);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[1].WorkingTimeRanges.Count == 1);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[1].TotalWorkMinutes == 60);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[1].Title == "サンプル作業２");
        }

        [Test]
        public void 同日複数レコードあり_同一種別含む()
        {
            var records = new WorkingTimeRecordForReport[]
           {
                new WorkingTimeRecordForReport
                {
                    Ymd = _Ymd0501,
                    TaskCategory = TaskCategory.Develop,
                    WorkProcess = _Process01,
                    Product = _Product01,
                    Client = _Client01,
                    StartDateTime = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "100000").Value,
                    EndDateTime   = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "103000").Value,
                    Title = "サンプル作業１",
                    WorkingTimeId = new Identity<WorkingTimeRange>(1),
                    WorkTaskId = new Identity<WorkTask>(1),
                },
                new WorkingTimeRecordForReport
                {
                    Ymd = _Ymd0501,
                    TaskCategory = TaskCategory.Develop,
                    WorkProcess = _Process02,
                    Product = _Product01,
                    Client = _Client01,
                    StartDateTime = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "103000").Value,
                    EndDateTime   = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "113000").Value,
                    Title = "サンプル作業２",
                    WorkingTimeId = new Identity<WorkingTimeRange>(2),
                    WorkTaskId = new Identity<WorkTask>(2),
                },
                new WorkingTimeRecordForReport
                {
                    Ymd = _Ymd0501,
                    TaskCategory = TaskCategory.Develop,
                    WorkProcess = _Process01,
                    Product = _Product01,
                    Client = _Client01,
                    StartDateTime = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "120000").Value,
                    EndDateTime   = DateTimeParser.ConvertFromYmdHHmmss(_Ymd0501.Value, "143000").Value,
                    Title = "サンプル作業１",
                    WorkingTimeId = new Identity<WorkingTimeRange>(3),
                    WorkTaskId = new Identity<WorkTask>(1),
                },
           };

            var list = _MonthlyReportBuilder.Build(new DailyWorkResults { WorkingTimeRecordForReports = records });

            Assert.IsTrue(list.Length == 31);
            Assert.IsTrue(list[0].WorkYmd == _Ymd0501.Value);
            Assert.IsTrue(list[0].DailyWorkTaskUnits.Count == 2);

            Assert.IsTrue(list[0].DailyWorkTaskUnits[0].TaskId.Value == 1);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[0].WorkingTimeRanges.Count == 2);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[0].TotalWorkMinutes == 180);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[0].Title == "サンプル作業１");

            Assert.IsTrue(list[0].DailyWorkTaskUnits[1].TaskId.Value == 2);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[1].WorkingTimeRanges.Count == 1);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[1].TotalWorkMinutes == 60);
            Assert.IsTrue(list[0].DailyWorkTaskUnits[1].Title == "サンプル作業２");
        }
    }
}
