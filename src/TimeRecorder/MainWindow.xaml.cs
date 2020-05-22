using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private NotifyIconProxy _NotifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            var iconPath = new Uri("pack://application:,,,/TimeRecorder;component/clock_32.ico", UriKind.Absolute);
            _NotifyIcon = new NotifyIconProxy(iconPath, "工数管理");
            _NotifyIcon.DoubleClick += (_, __) => ShowWindow();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // クローズ処理をキャンセルして、タスクバーの表示も消す
            e.Cancel = true;
            this.WindowState = System.Windows.WindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // ウィンドウを閉じる際に、タスクトレイのアイコンを削除する。
            _NotifyIcon.Dispose();
        }

        private void ShowWindow()
        {
            // ウィンドウ表示&最前面に持ってくる
            if (this.WindowState == System.Windows.WindowState.Minimized)
                this.WindowState = System.Windows.WindowState.Normal;

            this.Show();
            this.Activate();
            this.ShowInTaskbar = true;
        }

        public void Notify(ToolTipIconKind kind, string title, string message)
        {
            _NotifyIcon.ShowBalloonTip(1000, title, message, kind);
        }

    }
}
