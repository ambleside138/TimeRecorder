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
using Livet.Behaviors.Messaging.Windows;
using TimeRecorder.Messaging.Windows;

namespace TimeRecorder.Behaviors.Messaging.Windows;

public class ModalWindowInteractionMessageAction : WindowInteractionMessageAction
{
    protected override void InvokeAction(Livet.Messaging.InteractionMessage m)
    {
        if (m is ModalWindowActionMessage windowMessage && AssociatedObject != null)
        {
            var window = Window.GetWindow(AssociatedObject);
            if (window == null) return;

            window.DialogResult = windowMessage.DialogResult;
        }

        base.InvokeAction(m);
    }
}
