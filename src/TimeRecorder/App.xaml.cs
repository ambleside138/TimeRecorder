using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        protected override void OnStartup(StartupEventArgs e)
        {
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

    }
}
