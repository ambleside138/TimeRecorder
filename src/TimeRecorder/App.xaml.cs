using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TimeRecorder.Host;

namespace TimeRecorder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex _Mutex = new Mutex(false, "TimeRecorder");

        protected override void OnStartup(StartupEventArgs e)
        {
            if (_Mutex.WaitOne(0, false) == false)
            {
                // 起動済みのウィンドウをアクティブにする
                ShowPrevProcess();

                // ここまで
                _Mutex.Close();
                _Mutex = null;
                Shutdown();
                return;
            }

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            base.OnStartup(e);

            ContainerHelper.Setup();
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
                    ShowWindow(hProcess.MainWindowHandle, SW_NORMAL);
                    SetForegroundWindow(hProcess.MainWindowHandle);
                    return true;
                }
            }

            return false;
        }

        [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
        private static extern int ShowWindow(
        System.IntPtr hWnd,
        int nCmdShow
    );


        [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
        private static extern bool SetForegroundWindow(
            System.IntPtr hWnd
        );


        private const int SW_NORMAL = 1; 
        #endregion

    }
}
