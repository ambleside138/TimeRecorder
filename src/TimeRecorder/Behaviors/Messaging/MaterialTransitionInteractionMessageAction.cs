using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Livet.Messaging;
using Livet.Behaviors.Messaging;


namespace TimeRecorder.Behaviors.Messaging;

public class MaterialTransitionInteractionMessageAction : TransitionInteractionMessageAction
{
    #region HostBorder依存関係プロパティ
    public Border HostBorder
    {
        get { return (Border)GetValue(HostBorderProperty); }
        set { SetValue(HostBorderProperty, value); }
    }

    // Using a DependencyProperty as the backing store for HostBorder.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HostBorderProperty =
        DependencyProperty.Register("HostBorder", typeof(Border), typeof(MaterialTransitionInteractionMessageAction), new PropertyMetadata(null));
    #endregion


    protected override void InvokeAction(Livet.Messaging.InteractionMessage m)
    {
        try
        {
            if (HostBorder != null)
            {
                HostBorder.Visibility = Visibility.Visible;
            }

            base.InvokeAction(m);
        }
        finally
        {
            if (HostBorder != null)
            {
                HostBorder.Visibility = Visibility.Collapsed;
            }
        }
    }
}
