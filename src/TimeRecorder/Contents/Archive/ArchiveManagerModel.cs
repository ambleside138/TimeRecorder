using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking.Reports;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.Archive
{
    public class ArchiveManagerModel : NotificationObject, IDisposable
    {
        public ReactivePropertySlim<DateTime> TargetDate { get; }
        private LivetCompositeDisposable _Disposables = new();
        private bool _DisposedValue;

        private readonly GetDailyWorkRecordHeadersUseCase _GetDailyWorkRecordHeadersUseCase;

        public ObservableCollection<WorkingTimeRecordForReport> DailyWorkRecordHeaders { get; } = new ObservableCollection<WorkingTimeRecordForReport>();

        public ReactiveProperty<bool> NoResults { get; } = new ReactiveProperty<bool>();

        public ArchiveManagerModel()
        {
            _GetDailyWorkRecordHeadersUseCase = new GetDailyWorkRecordHeadersUseCase(ContainerHelper.GetRequiredService<IDailyWorkRecordQueryService>());

            TargetDate = new ReactivePropertySlim<DateTime>(DateTime.Today);
            TargetDate.Subscribe(_ => Load()).AddTo(_Disposables);
        }


        public void Load()
        {
            DailyWorkRecordHeaders.Clear();

            var headers = _GetDailyWorkRecordHeadersUseCase.Get(new YmdString(TargetDate.Value));
            DailyWorkRecordHeaders.AddRange(headers);

            NoResults.Value = DailyWorkRecordHeaders.Count == 0;
        }

        #region disposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_DisposedValue)
            {
                if (disposing)
                {
                    _Disposables?.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                _DisposedValue = true;
            }
        }


        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        } 
        #endregion
    }
}
