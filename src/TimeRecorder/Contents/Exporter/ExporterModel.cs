using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using TimeRecorder.Configurations;
using TimeRecorder.Configurations.Items;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Host;

namespace TimeRecorder.Contents.Exporter;

class ExporterModel
{
    private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();

    private readonly ExportMonthlyReportUseCase _ExportMonthlyReportUseCase;

    private readonly WorkingHourUseCase _WorkingHourUseCase;

    private readonly ImportWorkingHourUseCase _ImportWorkingHourUseCase;

    private static HttpClient client = new();

    public string WorkingHourImportUrl { get; private set; }

    public ExporterModel()
    {
        _ExportMonthlyReportUseCase = new ExportMonthlyReportUseCase(
            ContainerHelper.GetRequiredService<IDailyWorkRecordQueryService>(),
            ContainerHelper.GetRequiredService<IReportDriver>());

        _WorkingHourUseCase = new WorkingHourUseCase(ContainerHelper.GetRequiredService<IWorkingHourRepository>());

        _ImportWorkingHourUseCase = new ImportWorkingHourUseCase(
            ContainerHelper.GetRequiredService<IWorkingHourRepository>(),
            ContainerHelper.GetRequiredService<IWorkingHourImportDriver>());

        WorkingHourImportUrl = UserConfigurationManager.Instance.GetConfiguration<WorkingHourImportApiUrlConfig>(ConfigKey.WorkingHourImportApiUrl)?.URL ?? "";
    }

    public void Export(int year, int month, string path, bool autoAdjust, bool useNewFormat)
    {
        var targetYearMonth = new Domain.Domain.YearMonth(year, month);

        var result = _ExportMonthlyReportUseCase.Export(targetYearMonth, path, autoAdjust, useNewFormat);

        if (result.IsSuccessed)
        {
            try
            {
                Clipboard.SetText(result.Rows);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }

            SnackbarService.Current.ShowMessage("クリップボードと以下のパスに工数集計結果を出力しました" + Environment.NewLine + path);
        }
        else
        {
            var sb = new StringBuilder()
                        .AppendLine("【エラーあり】工数集計結果を出力しました")
                        .AppendLine(path);

            SnackbarService.Current.ShowMessage(sb.ToString());
        }

    }

    public async Task ImportWorkingHourByApi(YearMonth yearMonth, string param)
    {
        SnackbarService.Current.ShowMessage("勤務時間取込処理を開始します");

        var requestUri = $"{WorkingHourImportUrl}?key={param}";
        var response = await client.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
            // JSONを受け取るにはSpreadSheetのAPIをanyoneで公開する必要がある
            var json = await response.Content.ReadAsStringAsync();
            _Logger.Info("GET JSON" + Environment.NewLine + json);

            var result = JsonSerializer.Deserialize<WorkingHourJson>(json, JsonSerializerHelper.DefaultOptions);

            if (result?.records.Any() == true)
            {
                var targetData = result.records.Select(r => r.ConvertToDomainModel())
                                               .Where(r => yearMonth.Contains(r.Ymd))
                                               .ToArray();

                _WorkingHourUseCase.Import(targetData);

                //SnackbarService.Current.ShowMessage($"{targetData.Count(t => t.IsEmpty == false)}件の勤務時間を取り込みました");
                _Logger.Info($"{targetData.Count(t => t.IsEmpty == false)}件の勤務時間を取り込みました");
            }
        }
        else
        {
            _Logger.Error("Api実行に失敗 code=" + response.StatusCode);
        }
    }

    public void ImportFile(string path)
    {
        path = path.Replace("\"", "");
        var results = _ImportWorkingHourUseCase.Import(path);

        UserConfigurationManager.Instance.SetConfiguration(new ImportParamConfig { Param = path });

        if (results.Any())
        {
            var min = results.Min(r => r.Ymd.Value);
            var max = results.Max(r => r.Ymd.Value);

            var minDate = DateTimeParser.ConvertFromYmd(min)?.ToString("yyyy/MM/dd") ?? "";
            var maxDate = DateTimeParser.ConvertFromYmd(max)?.ToString("yyyy/MM/dd") ?? "";

            SnackbarService.Current.ShowMessage($"{results.Length}件 ({minDate} ～ {maxDate}) の勤務時間情報を取り込みました");
        }
        else
        {
            SnackbarService.Current.ShowMessage($"取込対象の勤務時間情報はありません");
        }
    }

}
