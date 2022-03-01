
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using TimeRecorder.Contents;
using TimeRecorder.Controls.WindowLocation;
using TimeRecorder.Host;

namespace TimeRecorder;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    private NotifyIconProxy _NotifyIcon;

    #region WindowSettings 依存関係プロパティ
    // ウィンドウ位置保存のロジックは下記記事参照
    // http://grabacr.net/archives/1585
    public IWindowSettings WindowSettings
    {
        get { return (IWindowSettings)this.GetValue(WindowSettingsProperty); }
        set { this.SetValue(WindowSettingsProperty, value); }
    }
    public static readonly DependencyProperty WindowSettingsProperty =
        DependencyProperty.Register("WindowSettings", typeof(IWindowSettings), typeof(MainWindow), new UIPropertyMetadata(new JsonFileWindowSettings()));

    #endregion

    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainWindowViewModel.Instance;

        var iconPath = new Uri("pack://application:,,,/TimeRecorder;component/clock_32.ico", UriKind.Absolute);
        _NotifyIcon = new NotifyIconProxy(iconPath, "工数管理");
        _NotifyIcon.DoubleClick += (_, __) => ShowWindow();
        _NotifyIcon.Click += (_, __) => ShowWindow();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        if (WindowSettings == null)
            return;

        WindowSettings.Reload();

        if (WindowSettings.Placement.HasValue)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            var placement = this.WindowSettings.Placement.Value;
            placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            placement.flags = 0;
            placement.showCmd = (placement.showCmd == SW.SHOWMINIMIZED) ? SW.SHOWNORMAL : placement.showCmd;

            NativeMethods.SetWindowPlacement(hwnd, ref placement);
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (ApplicationService.Instance.IsShutDownProcessing)
        {
            if (WindowSettings != null)
            {
                WINDOWPLACEMENT placement;
                var hwnd = new WindowInteropHelper(this).Handle;
                NativeMethods.GetWindowPlacement(hwnd, out placement);
                WindowSettings.Placement = placement;
                WindowSettings.Save();
            }
        }
        else
        {
            // クローズ処理をキャンセルして、タスクバーの表示も消す
            e.Cancel = true;
            this.WindowState = System.Windows.WindowState.Minimized;
            ///this.ShowInTaskbar = false;
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        // ウィンドウを閉じる際に、タスクトレイのアイコンを削除する。
        _NotifyIcon.Dispose();
    }

    public void ShowWindow()
    {
        // ウィンドウ表示&最前面に持ってくる
        if (this.WindowState == System.Windows.WindowState.Minimized)
            this.WindowState = System.Windows.WindowState.Normal;

        this.Show();
        this.Activate();
        this.ShowInTaskbar = true;
    }

}
