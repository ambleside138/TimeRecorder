using Livet.Behaviors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TimeRecorder.Controls
{
    // https://github.com/Grabacr07/MetroTrilithon/blob/master/source/MetroTrilithon.Desktop/UI/Controls/CallMethodButton.cs

    /// <summary>
    /// クリックされたときに、指定したメソッドを実行する <see cref="Button"/> を表します。
    /// </summary>
    public class CallMethodButton : Button
    {
        private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();

        static CallMethodButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CallMethodButton), new FrameworkPropertyMetadata(typeof(Button)));
        }

        private readonly MethodBinder _binder = new();
        private readonly MethodBinderWithArgument _binderWithArgument = new();
        private bool _hasParameter;

        #region MethodTarget 依存関係プロパティ

        public object MethodTarget
        {
            get { return this.GetValue(MethodTargetProperty); }
            set { this.SetValue(MethodTargetProperty, value); }
        }
        public static readonly DependencyProperty MethodTargetProperty =
            DependencyProperty.Register(nameof(MethodTarget), typeof(object), typeof(CallMethodButton), new UIPropertyMetadata(null));

        #endregion

        #region MethodName 依存関係プロパティ

        public string MethodName
        {
            get { return (string)this.GetValue(MethodNameProperty); }
            set { this.SetValue(MethodNameProperty, value); }
        }
        public static readonly DependencyProperty MethodNameProperty =
            DependencyProperty.Register(nameof(MethodName), typeof(string), typeof(CallMethodButton), new UIPropertyMetadata(null));

        #endregion

        #region MethodParameter 依存関係プロパティ

        public object MethodParameter
        {
            get { return this.GetValue(MethodParameterProperty); }
            set { this.SetValue(MethodParameterProperty, value); }
        }
        public static readonly DependencyProperty MethodParameterProperty =
            DependencyProperty.Register(nameof(MethodParameter), typeof(object), typeof(CallMethodButton), new UIPropertyMetadata(null, MethodParameterPropertyChangedCallback));

        private static void MethodParameterPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (CallMethodButton)d;
            source._hasParameter = true;
        }

        #endregion

        protected override void OnClick()
        {
            base.OnClick();

            var target = this.MethodTarget ?? this.DataContext;
            if (target == null) return;

            var contentName = "";
            var icon = Content as PackIcon;
            if (icon != null)
                contentName = icon.Kind.ToString();
            else
                contentName = Content?.ToString() ?? "";

            contentName += " @" + DataContext.GetType().FullName;

            if (string.IsNullOrEmpty(this.MethodName))
            {
                _Logger.Warn("[ButtonClicked] no methodName " + contentName);
                return;
            }

            _Logger.Info($"[ButtonClicked] {MethodName} | {contentName}");

            if (this._hasParameter)
            {
                this._binderWithArgument.Invoke(target, this.MethodName, this.MethodParameter);
            }
            else
            {
                this._binder.Invoke(target, this.MethodName);
            }
        }
    }
}
