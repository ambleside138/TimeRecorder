using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.UseCase.Tracking.Reports;

namespace TimeRecorder.Contents.Exporter
{
    class ExporterModel
    {
        private readonly ExportMonthlyReportUseCase _ExportMonthlyReportUseCase;

        public ExporterModel()
        {
            _ExportMonthlyReportUseCase = new ExportMonthlyReportUseCase(
                ContainerHelper.Resolver.Resolve<IDailyWorkRecordQueryService>(),
                ContainerHelper.Resolver.Resolve<IReportDriver>());
        }

        public void Export(int year, int month, string path)
        {
            _ExportMonthlyReportUseCase.Export(new Domain.Domain.YearMonth(year, month), path);
        }

    }
}
