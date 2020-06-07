using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimeRecorder.Helpers
{
    public enum ModalTransitionResponse
    {
        Yes,
        No,
        Cancel
    }

    class TransitionHelper
    {
        public static TransitionHelper Current { get; } = new TransitionHelper();

        private InteractionMessenger _Messenger;

        public void SetMessanger(InteractionMessenger messenger)
        {
            _Messenger = messenger;
        }

        public ModalTransitionResponse TransitionModal<TView>(INotifyPropertyChanged viewModel)
        {
            if (_Messenger == null)
                throw new InvalidOperationException("Messengerが設定されていません");

            var message = new TransitionMessage(typeof(TView), viewModel, TransitionMode.Modal, "ModalTransitionKey");

            _Messenger.Raise(message);

            return ConvertToResponse(message.Response);
        }

        private ModalTransitionResponse ConvertToResponse(bool? result)
        {
            if(result.HasValue)
            {
                return result.Value ? ModalTransitionResponse.Yes : ModalTransitionResponse.No;
            }
            else
            {
                return ModalTransitionResponse.Cancel;
            }
        }
    }
}
