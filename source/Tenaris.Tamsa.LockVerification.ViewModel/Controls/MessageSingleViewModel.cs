using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tenaris.Tamsa.LockVerification.ViewModel.Controls
{
    public class MessageSingleViewModel : NotificationObject
    {
        private string _textMessage;

        public string TextMessage
        {
            get
            {
                return _textMessage;
            }
            set
            {
                if (_textMessage != value)
                {
                    _textMessage = value;
                    RaisePropertyChanged(() => TextMessage);
                }
            }
        }
    }
}
