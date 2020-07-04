using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Host;

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

        public void Export(int year, int month, string path, bool autoAdjust)
        {
            var result = _ExportMonthlyReportUseCase.Export(new Domain.Domain.YearMonth(year, month), path, autoAdjust);

            if(result.IsSuccessed)
            {
                SnackbarService.Current.ShowMessage("以下のパスに工数集計結果を出力しました" + Environment.NewLine + path);
            }
            else
            {
                var sb = new StringBuilder()
                            .AppendLine("【エラーあり】工数集計結果を出力しました")
                            .AppendLine(path);

                SnackbarService.Current.ShowMessage(sb.ToString());
            }

        }

    }
}
