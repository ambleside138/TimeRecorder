using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Livet.Messaging;
using Livet.Messaging.Windows;

namespace TimeRecorder.Messaging.Windows
{
    /// <summary>
    /// ModalWindowのResponseを設定して閉じるためのMessageです
    /// </summary>
    public class ModalWindowActionMessage : WindowActionMessage
    {


        public bool? DialogResult
        {
            get { return (bool?)GetValue(DialogResultProperty); }
            set { SetValue(DialogResultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DialogResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof(bool?), typeof(ModalWindowActionMessage), new PropertyMetadata(null));



        // This is a constructor for to create this instance at View layer.
        public ModalWindowActionMessage()
        {
        }

        // This is a construcot when using to create this instance at ViewModel layer to send it using Messenger class.
        public ModalWindowActionMessage(string messageKey)
            : base(messageKey)
        {

        }

        /*
         * Define some properties if you need using propdp code snippets.
         */

        /// <summary>
        /// Please do not remove this method.
        /// </summary>
        /// <returns>A new instance of this.</returns>
        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new ModalWindowActionMessage();
        }
    }
}
