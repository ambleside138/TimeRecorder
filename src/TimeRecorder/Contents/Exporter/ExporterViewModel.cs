using Livet;
using Livet.Messaging.IO;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using TimeRecorder.Configurations;
using TimeRecorder.Configurations.Items;
using TimeRecorder.Host;
using TimeRecorder.NavigationRail;

namespace TimeRecorder.Contents.Exporter
{
    public class ExporterViewModel : ViewModel, IContentViewModel
    {


        public NavigationIconButtonViewModel NavigationIcon => new NavigationIconButtonViewModel { Title = "レポート", IconKey = "FileDocumentOutline" };

        public int[] Years => Enumerable.Range(DateTime.Today.Year - 2, 5).ToArray();

        public int[] Months => Enumerable.Range(1, 12).ToArray();

        public ReactivePropertySlim<int> SelectedYear { get; } = new ReactivePropertySlim<int>(DateTime.Today.Year);

        public ReactivePropertySlim<int> SelectedMonth { get; } = new ReactivePropertySlim<int>(DateTime.Today.Month - 1);

        public ReactivePropertySlim<bool> AutoAdjust { get; } = new ReactivePropertySlim<bool>(true);

        public ReactivePropertySlim<string> ImportKey { get; } = new ReactivePropertySlim<string>("");

        public ReactivePropertySlim<bool> UseWorkingHourApiImport { get; } = new ReactivePropertySlim<bool>();

        public string ExportFilter => "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";

        public ReadOnlyReactivePropertySlim<string> InitialFileName { get; }

        private readonly ExporterModel _ExporterModel = new ExporterModel();
        public ReactivePropertySlim<bool> IsSelected { get; } = new();

        public ExporterViewModel()
        {
            InitialFileName = SelectedYear.CombineLatest(SelectedMonth, (year, month) => $"工数管理_{year}年{month:00}月分")
                                          .ToReadOnlyReactivePropertySlim();

            UseWorkingHourApiImport.Value = string.IsNullOrEmpty(_ExporterModel.WorkingHourImportUrl) == false;

            var paramConfig = UserConfigurationManager.Instance.GetConfiguration<ImportParamConfig>(ConfigKey.ImportParam);
            ImportKey.Value = paramConfig?.Param ?? "";
        }

        public void Export(SavingFileSelectionMessage message)
        {
            var savePath = message.Response?.FirstOrDefault();
            if (savePath == null)
                return;

            _ExporterModel.Export(SelectedYear.Value, SelectedMonth.Value, savePath, AutoAdjust.Value);

        }

        public async void Import()
        {
            if(UseWorkingHourApiImport.Value)
            {
                var targetYearMonth = new Domain.Domain.YearMonth(SelectedYear.Value, SelectedMonth.Value);
                await _ExporterModel.ImportWorkingHourByApi(targetYearMonth, ImportKey.Value);
            }
            else
            {
                _ExporterModel.ImportFile(ImportKey.Value);
            }
        }
    }
}
