using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using TimeRecorder.Host;

namespace TimeRecorder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();

        private Mutex _Mutex = new Mutex(false, "TimeRecorder");

        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeLogger();

            _Logger.Info("アプリケーションの開始");

            if (_Mutex.WaitOne(0, false) == false)
            {
                // 起動済みのウィンドウをアクティブにする
                ShowPrevProcess();

                // ここまで
                _Mutex.Close();
                _Mutex = null;
                Shutdown();

                _Logger.Info("二重起動のため終了します");
                return;
            }

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            base.OnStartup(e);

            ContainerHelper.Setup();

            NotificationService.Current.Setup();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NotificationService.Current.Uninstall();

            _Logger.Info("アプリケーションの終了");
            NLog.LogManager.Shutdown(); // Flush and close down internal threads and timers
            base.OnExit(e);
        }
        private void InitializeLogger()
        {
            var conf = new LoggingConfiguration();
            // ファイル出力定義
            var file = new FileTarget("logfile")
            {
                Encoding = System.Text.Encoding.UTF8,
                Layout = "${longdate} [${threadid:padding=2}] [${uppercase:${level:padding=-5}}] ${callsite}() - ${message}${exception:format=ToString}",
                FileName = "${basedir}/logs/TimeRecorder_${date:format=yyyyMMdd}.log",
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveFileName = "${basedir}/logs/TimeRecorder.log.{#}",
                ArchiveEvery = FileArchivePeriod.None,
                MaxArchiveFiles = 10
            };
            conf.AddTarget(file);

            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, file));

            // 設定を反映する
            LogManager.Configuration = conf;
        }

        private void App_DispatcherUnhandledException(
                object sender,
                System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //            string errorMember = e.Exception.TargetSite.Name;
            //            string errorMessage = e.Exception.Message;
            //            string message = string.Format(
            //        @"例外が{0}で発生。プログラムを継続しますか？
            //エラーメッセージ：{1}",
            //                                      errorMember, errorMessage);
            //            MessageBoxResult result
            //              = MessageBox.Show(message, "DispatcherUnhandledException",
            //                                MessageBoxButton.YesNo, MessageBoxImage.Warning);


            //            if (result == MessageBoxResult.Yes)
            //                e.Handled = true;

            _Logger.Error(e.Exception, "不明なエラーが発生しました");

            SnackbarService.Current.ShowMessage(GetExceptionMessage(e.Exception));

            e.Handled = true;
        }

        private string GetExceptionMessage(Exception ex)
        {
            if(ex.InnerException!= null)
            {
                return GetExceptionMessage(ex.InnerException);
            }

            return ex.Message;
        }


        #region 別プロセスのウィンドウを前面にだす
        // タスクトレイに収まっている場合に未対応

        public static bool ShowPrevProcess()
        {
            var thisProcess = Process.GetCurrentProcess();
            var hProcesses = Process.GetProcessesByName(thisProcess.ProcessName);
            var iThisProcessId = thisProcess.Id;

            foreach (var hProcess in hProcesses)
            {
                if (hProcess.Id != iThisProcessId)
                {
                    WindowActivator.Activate(hProcess.MainWindowHandle);
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
