using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Commands;
using Tenaris.Tamsa.LockVerification.ViewModel.Controls;
using Tenaris.Library.Log;
using Tenaris.Tamsa.LockVerification.ViewModel.Enum;
using System.Windows.Threading;
using System.Windows.Input;
using Tenaris.Tamsa.LockVerification.Model.Manager;
using Tenaris.Tamsa.LockVerification.Manager.Shared;
using Tenaris.Tamsa.LockVerification.Model.DataAccess;

namespace Tenaris.Tamsa.LockVerification.ViewModel
{
    public class MainWindowViewModel : NotificationObject
    {
        #region Private Attributes

        private WindowState _curWindowState;

        private Visibility _isVisibility;

        private MessageLoginViewModel _viewMessageLogin;

        private MessageSingleViewModel _viewMessageSingle;

        private Visibility _isVisibleMessageLogin;

        private Visibility _isVisibleMessageSingle;

        private string _chronometer;

        private string _chronometerStatus;

        private Verification _lastVerification;

        private LockVerificationHistory _lastLockVerificationHistory;

        private Dictionary<int, string> _timeConfiguration;

        private TimeSpan _chronometerTimeSpan;

        private List<LockVerificationHistory> _listLockVerificationHistory;

        #endregion

        #region Public Attributes

        public string Title
        {
            get { return LanguageResource.Language.ViewTitle; }
        }

        public string Version
        {
            get
            {
                var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                return (attributes.Length > 0) ? ((AssemblyFileVersionAttribute)attributes[0]).Version : "1.0";
            }
        }

        public WindowState CurWindowState
        {
            get
            {
                return _curWindowState;
            }
            set
            {
                if (_curWindowState != value)
                {
                    _curWindowState = value;
                    RaisePropertyChanged(() => CurWindowState);
                }
            }
        }

        public Visibility IsVisibility
        {
            get
            {
                return _isVisibility;
            }
            set
            {
                if (_isVisibility != value)
                {
                    _isVisibility = value;
                    RaisePropertyChanged(() => IsVisibility);
                }
            }
        }

        public MessageLoginViewModel ViewMessageLogin
        {
            get
            {
                return _viewMessageLogin;
            }
            set
            {
                if (_viewMessageLogin != value)
                {
                    _viewMessageLogin = value;
                    RaisePropertyChanged(() => ViewMessageLogin);
                }
            }
        }

        public MessageSingleViewModel ViewMessageSingle
        {
            get
            {
                return _viewMessageSingle;
            }
            set
            {
                if (_viewMessageSingle != value)
                {
                    _viewMessageSingle = value;
                    RaisePropertyChanged(() => ViewMessageSingle);
                }
            }
        }

        public Visibility IsVisibleMessageLogin
        {
            get
            {
                return _isVisibleMessageLogin;
            }
            set
            {
                if (_isVisibleMessageLogin != value)
                {
                    _isVisibleMessageLogin = value;
                    RaisePropertyChanged(() => IsVisibleMessageLogin);
                }
            }
        }

        public Visibility IsVisibleMessageSingle
        {
            get
            {
                return _isVisibleMessageSingle;
            }
            set
            {
                if (_isVisibleMessageSingle != value)
                {
                    _isVisibleMessageSingle = value;
                    RaisePropertyChanged(() => IsVisibleMessageSingle);
                }
            }
        }

        public string Chronometer
        {
            get
            {
                return _chronometer;
            }
            set
            {
                if (_chronometer != value)
                {
                    _chronometer = value;
                    RaisePropertyChanged(() => Chronometer);
                }
            }
        }

        public string ChronometerStatus
        {
            get
            {
                return _chronometerStatus;
            }
            set
            {
                if (_chronometerStatus != value)
                {
                    _chronometerStatus = value;
                    RaisePropertyChanged(() => ChronometerStatus);
                }
            }
        }

        public Verification LastVerification
        {
            get
            {
                return _lastVerification;
            }
            set
            {
                if (_lastVerification != value)
                {
                    _lastVerification = value;
                    RaisePropertyChanged(() => LastVerification);
                }
            }
        }

        public LockVerificationHistory LastLockVerificationHistory
        {
            get
            {
                return _lastLockVerificationHistory;
            }
            set
            {
                if (_lastLockVerificationHistory != value)
                {
                    _lastLockVerificationHistory = value;
                    RaisePropertyChanged(() => LastLockVerificationHistory);
                }
            }
        }

        public ManagerHandler ManagerHandlerInstance { get { return ManagerHandler.Instance; } }

        public List<LockVerificationHistory> ListLockVerificationHistory
        {
            get { return _listLockVerificationHistory; }
            set
            {
                if (_listLockVerificationHistory != value)
                {
                    _listLockVerificationHistory = value;
                    RaisePropertyChanged(() => ListLockVerificationHistory);
                }
            }
        }

        #endregion

        #region Commands

        public DelegateCommand AcceptCommand
        {
            get;
            private set;
        }

        public DelegateCommand SearchCommand
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            try
            {
                this.AcceptCommand = new DelegateCommand(this.OnAccept, this.OnCanAccept);
                this.SearchCommand = new DelegateCommand(this.OnSearch);

                ViewMessageLogin = new MessageLoginViewModel();
                ViewMessageSingle = new MessageSingleViewModel();

                IsVisibleMessageLogin = Visibility.Hidden;
                IsVisibleMessageSingle = Visibility.Hidden;

                ManagerHandler.Initialize();

                ManagerHandlerInstance.OnLastVerificationChange += MethodLastVerificationChange;
                ManagerHandlerInstance.OnLockVerificationStatusChange += MethodLockVerificationStatusChange;
                ManagerHandlerInstance.StatusManagerChange += MethodStatusManagerChange;

                //this.LastVerification = ManagerHandlerInstance.LastVerification;

                //this._timeConfiguration = ManagerHandlerInstance.GetTimeConfiguration();

                //UpdateLockVerificationView();

                InitializeChronometer();

                this.ListLockVerificationHistory = DataAccessClass.Instance.GetLockVerificationHistory();

            }
            catch (Exception ex)
            {
                Trace.Exception(ex, "Error in constructor MainWindowViewModel :{0}", ex.Message);
            }
        }

        #endregion

        #region Private Methods

        private void InitializeChronometer()
        {
            DispatcherTimer dispathcer = new DispatcherTimer();

            dispathcer.Interval = new TimeSpan(0, 0, 1);

            dispathcer.Tick += (s, a) =>
            {
                if (_chronometerTimeSpan.TotalSeconds > 0)
                {
                    _chronometerTimeSpan = _chronometerTimeSpan.Subtract(new TimeSpan(0, 0, 1));

                    //Application.Current.Dispatcher.Invoke((Action)delegate
                    //{
                        Chronometer = _chronometerTimeSpan.ToString(@"hh\:mm\:ss");
                    //});
                }
                //else
                //{
                //    dispathcer.Stop();
                //}
            };

            dispathcer.Start();
        }

        private void MethodLastVerificationChange(object sender, LastVerificationChangeEventArgs e)
        {
            this.LastVerification = (Verification)e.LastVerification;

            UpdateLockVerificationView();

            OnSearch();

        }

        private void MethodLockVerificationStatusChange(object sender, LockVerificationStatusChangeEventArgs e)
        {
            UpdateLockVerificationView();
        }

        private void MethodStatusManagerChange(object sender, EventArgs e)
        {
            if ((bool)sender)
            {
                Chronometer = "00:00:00";

                this.LastVerification = ManagerHandlerInstance.LastVerification;

                this._timeConfiguration = ManagerHandlerInstance.GetTimeConfiguration();

                UpdateLockVerificationView();

            }
        }

        private void UpdateLockVerificationView()
        {
            this.LastLockVerificationHistory = ManagerHandlerInstance.LastLockVerificationHistory;

            if((this.Chronometer == "00:00:00" && this.LastLockVerificationHistory.idStatusLockVerification != LockVerificationStatus.Stop) || (this.LastLockVerificationHistory.idStatusLockVerification == LockVerificationStatus.Inactive))
            {
                DateTimeOffset nextstop = this.LastVerification.InspectionDateTime.Add(TimeSpan.Parse(this._timeConfiguration[(int)LockVerificationStatus.Stop]));

                TimeSpan diff = nextstop - DateTimeOffset.Now;

                _chronometerTimeSpan = diff;

                ChronometerStatus = LockVerificationStatus.Inactive.ToString();
            }

            switch(this.LastLockVerificationHistory.idStatusLockVerification)
            {
                case LockVerificationStatus.Inactive:

                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        this.IsVisibleMessageSingle = Visibility.Visible;
                        this.IsVisibleMessageLogin = Visibility.Collapsed;

                        this.ViewMessageSingle.TextMessage = "";

                    });

                    break;
                case LockVerificationStatus.Warning:

                    //Application.Current.Dispatcher.Invoke((Action)delegate
                    //{
                    //    this.IsVisibility = Visibility.Visible;
                    //});

                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        this.CurWindowState = WindowState.Normal;
                        this.IsVisibleMessageSingle = Visibility.Visible;
                        this.IsVisibleMessageLogin = Visibility.Collapsed;

                        this.ViewMessageSingle.TextMessage = string.Format(LanguageResource.Language.MessageWarning, this._timeConfiguration[(int)LockVerificationStatus.Warning]);

                        ChronometerStatus = LockVerificationStatus.Warning.ToString();

                    });

                    break;
                case LockVerificationStatus.Alarm:

                    //Application.Current.Dispatcher.Invoke((Action)delegate
                    //{
                    //    this.IsVisibility = Visibility.Visible;
                    //});

                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        this.CurWindowState = WindowState.Normal;
                        this.IsVisibleMessageSingle = Visibility.Collapsed;
                        this.IsVisibleMessageLogin = Visibility.Visible;

                        this.ViewMessageLogin.TextMessage = string.Format(LanguageResource.Language.MessageAlarm, this._timeConfiguration[(int)LockVerificationStatus.Alarm], this._timeConfiguration[(int)LockVerificationStatus.Stop]);

                        if (this.LastLockVerificationHistory.AcceptDateTime.HasValue)
                        {
                            this.ViewMessageLogin.ErrorMessage = LanguageResource.Language.MessageAccept;
                        }

                        ChronometerStatus = LockVerificationStatus.Alarm.ToString();
                    });

                    break;
                case LockVerificationStatus.Stop:

                    //Application.Current.Dispatcher.Invoke((Action)delegate
                    //{
                    //    this.IsVisibility = Visibility.Visible;
                    //});

                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        this.CurWindowState = WindowState.Normal;
                        this.IsVisibleMessageSingle = Visibility.Collapsed;
                        this.IsVisibleMessageLogin = Visibility.Visible;

                        this.ViewMessageLogin.TextMessage = string.Format(LanguageResource.Language.MessageLock, this._timeConfiguration[(int)LockVerificationStatus.Stop]);

                        if (this.LastLockVerificationHistory.AcceptDateTime.HasValue)
                        {
                            this.ViewMessageLogin.ErrorMessage = LanguageResource.Language.MessageAccept;
                        }

                        ChronometerStatus = LockVerificationStatus.Stop.ToString();
                    });
                    
                    break;
            }            
        }

        #endregion

        #region Commands Methods

        private void OnAccept()
        {
            bool flagMinimized = false;

            switch (this.LastLockVerificationHistory.idStatusLockVerification)
            {
                case LockVerificationStatus.Inactive:

                    flagMinimized = true;


                    break;
                case LockVerificationStatus.Warning:

                    if (!this.LastLockVerificationHistory.AcceptDateTime.HasValue)
                    {
                        var sa = ManagerHandlerInstance.SaveAccept();

                        if (sa.Code == Constants.CodeOK)
                        {
                            this.LastLockVerificationHistory = ManagerHandlerInstance.LastLockVerificationHistory;
                            flagMinimized = true;
                        }
                        else
                        {
                            this.ViewMessageLogin.ErrorMessage = sa.Message;
                        }
                    }

                    break;
                case LockVerificationStatus.Alarm:
                case LockVerificationStatus.Stop:

                    if (!string.IsNullOrEmpty(this.ViewMessageLogin.User) && !string.IsNullOrEmpty(this.ViewMessageLogin.Password) && !string.IsNullOrEmpty(this.ViewMessageLogin.Comment))
                    {
                        if (!this.LastLockVerificationHistory.AcceptDateTime.HasValue)
                        {
                            var sa = ManagerHandlerInstance.SendAuthorization(this.ViewMessageLogin.User, this.ViewMessageLogin.Password, this.ViewMessageLogin.Comment);

                            if (sa.Code == Constants.CodeOK)
                            {
                                this.LastLockVerificationHistory = ManagerHandlerInstance.LastLockVerificationHistory;

                                this.ViewMessageLogin.User = "";
                                this.ViewMessageLogin.Password = "";
                                this.ViewMessageLogin.Comment = "";

                                this.ViewMessageLogin.ErrorMessage = "";

                                flagMinimized = true;
                            }
                            else
                            {
                                this.ViewMessageLogin.ErrorMessage = sa.Message;
                            }
                        }
                    }

                    break;
            }

            if(flagMinimized || this.LastLockVerificationHistory.AcceptDateTime.HasValue)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    this.CurWindowState = WindowState.Minimized;
                });
            }            
        }

        private bool OnCanAccept()
        {
            //if (this.LastLockVerificationHistory != null)
            //{
            //    if (this.LastLockVerificationHistory.idStatusLockVerification == LockVerificationStatus.Alarm || this.LastLockVerificationHistory.idStatusLockVerification == LockVerificationStatus.Stop)
            //    {
            //        if (string.IsNullOrEmpty(this.ViewMessageLogin.User) && string.IsNullOrEmpty(this.ViewMessageLogin.Password) && string.IsNullOrEmpty(this.ViewMessageLogin.Comment))
            //        {
            //            return false;
            //        }
            //    }
            //}

            return true;
        }

        private void OnSearch()
        {
            this.ListLockVerificationHistory = DataAccessClass.Instance.GetLockVerificationHistory();
        }

        #endregion

    }
}
