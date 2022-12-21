using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tenaris.Tamsa.LockVerification.ViewModel.Controls
{
    public class MessageLoginViewModel : NotificationObject
    {
        private string _textMessage;

        private string _user;

        private string _password;

        private string _comment;

        private string _errorMessage;

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

        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    RaisePropertyChanged(() => User);
                }
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged(() => Password);
                }
            }
        }

        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    RaisePropertyChanged(() => Comment);
                }
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    RaisePropertyChanged(() => ErrorMessage);
                }
            }
        }
    }
}
