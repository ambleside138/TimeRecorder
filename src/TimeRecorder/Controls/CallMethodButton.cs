using Livet.Behaviors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TimeRecorder.Controls;

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
        get => GetValue(MethodTargetProperty);
        set => SetValue(MethodTargetProperty, value);
    }
    public static readonly DependencyProperty MethodTargetProperty =
        DependencyProperty.Register(nameof(MethodTarget), typeof(object), typeof(CallMethodButton), new UIPropertyMetadata(null));

    #endregion

    #region MethodName 依存関係プロパティ

    public string MethodName
    {
        get => (string)GetValue(MethodNameProperty);
        set => SetValue(MethodNameProperty, value);
    }
    public static readonly DependencyProperty MethodNameProperty =
        DependencyProperty.Register(nameof(MethodName), typeof(string), typeof(CallMethodButton), new UIPropertyMetadata(null));

    #endregion

    #region MethodParameter 依存関係プロパティ

    public object MethodParameter
    {
        get => GetValue(MethodParameterProperty);
        set => SetValue(MethodParameterProperty, value);
    }
    public static readonly DependencyProperty MethodParameterProperty =
        DependencyProperty.Register(nameof(MethodParameter), typeof(object), typeof(CallMethodButton), new UIPropertyMetadata(null, MethodParameterPropertyChangedCallback));

    private static void MethodParameterPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var source = (CallMethodButton)d;
        source._hasParameter = true;
    }

    #endregion



    public bool ShowContextMenuOnClick
    {
        get => (bool)GetValue(ShowContextMenuOnClickProperty);
        set => SetValue(ShowContextMenuOnClickProperty, value);
    }

    // Using a DependencyProperty as the backing store for ShowContextMenuOnClick.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowContextMenuOnClickProperty =
        DependencyProperty.Register("ShowContextMenuOnClick", typeof(bool), typeof(CallMethodButton), new PropertyMetadata(false));



    protected override void OnClick()
    {
        base.OnClick();

        var target = MethodTarget ?? DataContext;
        if (target == null) return;

        if (ShowContextMenuOnClick
             && ContextMenu != null)
        {
            // https://stackoverflow.com/questions/555252/show-contextmenu-on-left-click-using-only-xaml/
            // If we use binding in our context menu, then it's DataContext won't be set when we show the menu on left click. It
            // seems setting DataContext for ContextMenu is hardcoded in WPF when user right clicks on a control? So we have to set
            // up ContextMenu.DataContext manually here.
            if (ContextMenu.DataContext == null)
            {
                ContextMenu.SetBinding(DataContextProperty, new Binding { Source = target });
            }
            if (ContextMenu.PlacementTarget == null)
            {
                ContextMenu.PlacementTarget = this;
            }
            ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            ContextMenu.HorizontalOffset = ContextMenu.Width - ActualWidth;
            ContextMenu.IsOpen = true;
            return;
        }


        string contentName;
        if (Content is PackIcon icon)
            contentName = icon.Kind.ToString();
        else
            contentName = Content?.ToString() ?? "";

        contentName += " @" + target.GetType().FullName;

        if (string.IsNullOrEmpty(MethodName))
        {
            _Logger.Warn("[ButtonClicked] no methodName " + contentName);
            return;
        }

        _Logger.Info($"[ButtonClicked] {MethodName} | {contentName}");



        if (_hasParameter)
        {
            _binderWithArgument.Invoke(target, MethodName, MethodParameter);
        }
        else
        {
            _binder.Invoke(target, MethodName);
        }
    }
}
