using Livet;
using Livet.Messaging.IO;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TimeRecorder.Host;
using TimeRecorder.NavigationRail.ViewModels;

namespace TimeRecorder.Contents.Exporter
{
    public class ExporterViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new NavigationIconButtonViewModel { Title = "出力", IconKey = "FileExport" };

        public int[] Years => Enumerable.Range(DateTime.Today.Year - 2, 5).ToArray();

        public int[] Months => Enumerable.Range(1, 12).ToArray();

        public ReactivePropertySlim<int> SelectedYear { get; } = new ReactivePropertySlim<int>(DateTime.Today.Year);

        public ReactivePropertySlim<int> SelectedMonth { get; } = new ReactivePropertySlim<int>(DateTime.Today.Month - 1);

        public string ExportFilter => "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";

        private readonly ExporterModel _ExporterModel = new ExporterModel();

        public void Export(SavingFileSelectionMessage message)
        {
            var savePath = message.Response.First();
            _ExporterModel.Export(SelectedYear.Value, SelectedMonth.Value, savePath);

            SnackbarService.Current.ShowMessage("以下のパスに工数集計結果を出力しました" + Environment.NewLine + savePath);
        }
    }
}
