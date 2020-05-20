using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder
{
    // 参考
    // http://sourcechord.hatenablog.com/entry/2017/02/11/125649
    // https://github.com/sourcechord/WPFSamples/blob/master/NotifyIconSample/NotifyIconSample/NotifyIconEx.cs

    public enum ToolTipIconKind
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    class NotifyIconProxy
    {
        private System.Windows.Forms.NotifyIcon _notify;

        public string Text
        {
            get { return _notify.Text; }
            set { _notify.Text = value; }
        }

        public bool Visible
        {
            get { return _notify.Visible; }
            set { _notify.Visible = value; }
        }

        public Uri IconPath
        {
            set
            {
                if (value == null) { return; }
                var iconStream = System.Windows.Application.GetResourceStream(value).Stream;
                _notify.Icon = new System.Drawing.Icon(iconStream);
            }
        }

        public System.Windows.Controls.ContextMenu ContextMenu { get; set; }

        public ToolTipIconKind BalloonTipIcon
        {
            get { return (ToolTipIconKind)_notify.BalloonTipIcon; }
            set { _notify.BalloonTipIcon = (System.Windows.Forms.ToolTipIcon)value; }
        }

        public string BalloonTipTitle
        {
            get { return _notify.BalloonTipTitle; }
            set { _notify.BalloonTipTitle = value; }
        }

        public string BalloonTipText
        {
            get { return _notify.BalloonTipText; }
            set { _notify.BalloonTipText = value; }
        }

        public void ShowBalloonTip(int timeout)
        {
            _notify.ShowBalloonTip(timeout);
        }

        public void ShowBalloonTip(int timeout, string tipTitle, string tipText, ToolTipIconKind tipIcon)
        {
            var icon = (System.Windows.Forms.ToolTipIcon)tipIcon;
            _notify.ShowBalloonTip(timeout, tipTitle, tipText, icon);
        }

        public NotifyIconProxy()
            : this(null) { }

        public NotifyIconProxy(Uri iconPath)
            : this(iconPath, null) { }

        public NotifyIconProxy(Uri iconPath, string text)
            : this(iconPath, text, null) { }

        public NotifyIconProxy(Uri iconPath, string text, System.Windows.Controls.ContextMenu menu)
        {
            // 各種プロパティを初期化
            _notify = new System.Windows.Forms.NotifyIcon();
            IconPath = iconPath;
            Text = text;
            ContextMenu = menu;

            // マウス右ボタンUpのタイミングで、ContextMenuの表示を行う
            // ダミーの透明ウィンドウを表示し、このウィンドウのアクティブ状態を用いてContextMenuの表示/非表示を切り替える
            _notify.MouseUp += (s, e) =>
            {
                if (e.Button != System.Windows.Forms.MouseButtons.Right) { return; }

                var win = new System.Windows.Window()
                {
                    WindowStyle = System.Windows.WindowStyle.None,
                    ShowInTaskbar = false,
                    AllowsTransparency = true,
                    Background = System.Windows.Media.Brushes.Transparent,
                    Content = new System.Windows.Controls.Grid(),
                    ContextMenu = ContextMenu
                };

                var isClosed = false;
                win.Activated += (_, __) =>
                {
                    if (win.ContextMenu != null)
                    {
                        win.ContextMenu.IsOpen = true;
                    }
                };
                win.Closing += (_, __) =>
                {
                    isClosed = true;
                };

                win.Deactivated += (_, __) =>
                {
                    if (win.ContextMenu != null)
                    {
                        win.ContextMenu.IsOpen = false;
                    }
                    if (!isClosed)
                    {
                        win.Close();
                    }
                };

                // ダミーウィンドウ表示&アクティブ化をする。
                // ⇒これがActivatedイベントで、ContextMenuが表示される
                win.Show();
                win.Activate();
            };

            _notify.Visible = true;
        }

        #region NotifyIconクラスの各種イベントをラップする
        public event EventHandler BalloonTipClicked
        {
            add { _notify.BalloonTipClicked += value; }
            remove { _notify.BalloonTipClicked -= value; }
        }

        public event EventHandler BalloonTipClosed
        {
            add { _notify.BalloonTipClosed += value; }
            remove { _notify.BalloonTipClosed -= value; }
        }

        public event EventHandler BalloonTipShown
        {
            add { _notify.BalloonTipShown += value; }
            remove { _notify.BalloonTipShown -= value; }
        }

        public event EventHandler Click
        {
            add { _notify.Click += value; }
            remove { _notify.Click -= value; }
        }

        public event EventHandler Disposed
        {
            add { _notify.Disposed += value; }
            remove { _notify.Disposed -= value; }
        }

        public event EventHandler DoubleClick
        {
            add { _notify.DoubleClick += value; }
            remove { _notify.DoubleClick -= value; }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _notify.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
