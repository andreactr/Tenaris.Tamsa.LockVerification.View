using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenaris.Library.ConnectionMonitor;
using Tenaris.Library.Framework.Factory;
using Tenaris.Library.Log;
using Tenaris.Tamsa.LockVerification.Manager.Shared;
using Tenaris.Tamsa.LockVerification.Model.Configuration;

namespace Tenaris.Tamsa.LockVerification.Model.Manager
{
    public class ManagerHandler
    {
        #region Static Attributes

        /// <summary>
        /// use to lock the logical
        /// </summary>
        private static readonly object lockInstance = new object();

        /// <summary>
        /// profile manager client
        /// </summary>
        private static ManagerHandler instance;

        #endregion static attributes

        #region Public Event

        /// <summary>
        /// handler status manager has changed
        /// </summary>
        public event EventHandler<EventArgs> StatusManagerChange;

        public event EventHandler<LockVerificationStatusChangeEventArgs> OnLockVerificationStatusChange;

        public event EventHandler<LastVerificationChangeEventArgs> OnLastVerificationChange;

        #endregion public event

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleIslands"/> class.
        /// </summary>
        private ManagerHandler()
        {
            ProxyConfiguration.ConfigureRemoting();

            IFactory<ILockVerificationManager> factory = FactoryProvider.Instance.CreateFactory<ILockVerificationManager>("LockVerificationManager");
            manager = factory.Create();
        }

        #endregion constructors

        #region Public Properties

        /// <summary>
        /// Gets the Instance of the Island class
        /// </summary>
        public static ManagerHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockInstance)
                    {
                        if (instance == null)
                        {
                            instance = new ManagerHandler();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// profile manager client
        /// </summary>
        public ILockVerificationManager manager
        {
            get;
            private set;
        }

        public Verification LastVerification
        {
            get
            {
                if (manager != null)
                    return (Verification)(manager.LastVerification);
                else
                    return null;
            }
        }

        public LockVerificationHistory LastLockVerificationHistory
        {
            get
            {
                if (manager != null)
                    return (LockVerificationHistory)(manager.LastLockVerificationHistory);
                else
                    return null;
            }
        }

        #endregion

        #region Public Function

        /// <summary>
        /// initialize profile manager instance and create objects
        /// </summary>
        /// <param name="machine"></param>
        public static void Initialize()
        {
            try
            {
                Tenaris.Library.UI.Framework.GlobalProperties.ConnectionStatusProperties.SetConnectionState("Lock Verification Manager", true);
                Trace.Message("{0}  -> [LockVerificationManager] Connection Monitor; LockVerificationManager is changed state to ==> [CONNECTED]", DateTime.Now);
                ConnectionMonitor.Instance.StateChanged += Instance.InstanceStateChanged;
            }
            catch (Exception ex)
            {
                Trace.Exception(ex, "Error in LockVerificationManager {0} ..", ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Uninitialize()
        {
            try
            {
                manager.OnLastVerificationChange -= Manager_OnLastVerificationChange;
                manager.OnLockVerificationStatusChange -= Manager_OnLockVerificationStatusChange;
                
                manager.Dispose();
            }
            catch
            {
                manager.Dispose();
            }

            Trace.Message("{0} -> [ILockVerificationManager] Disposed", DateTime.Now);
        }

        #endregion

        #region Private Events

        private void Manager_OnLastVerificationChange(object sender, LastVerificationChangeEventArgs e)
        {
            if (OnLastVerificationChange != null)
                OnLastVerificationChange(this, e);
        }

        private void Manager_OnLockVerificationStatusChange(object sender, LockVerificationStatusChangeEventArgs e)
        {
            if (OnLockVerificationStatusChange != null)
                OnLockVerificationStatusChange(this, e);
        }

        #endregion

        #region Private Methods

        private void InstanceStateChanged(object sender, StateChangeEventArgs e)
        {
            try
            {
                if (e.Connection is ILockVerificationManager)
                {
                    if (e.IsConnected)
                    {
                        manager.OnLastVerificationChange -= Manager_OnLastVerificationChange;
                        manager.OnLockVerificationStatusChange -= Manager_OnLockVerificationStatusChange;

                        manager.OnLastVerificationChange += Manager_OnLastVerificationChange;
                        manager.OnLockVerificationStatusChange += Manager_OnLockVerificationStatusChange;

                        if (StatusManagerChange != null)
                        {
                            StatusManagerChange(e.IsConnected, e);
                        }

                        Tenaris.Library.UI.Framework.GlobalProperties.ConnectionStatusProperties.SetConnectionState("Lock Verification Manager", true);
                        Trace.Message("{0}  -> [ILockVerificationManager] Connection Monitor; LockVerificationManager is changed state to ==> [CONNECTED]", DateTime.Now);
                    }
                    else if (e.IsDisconnected)
                    {
                        Tenaris.Library.UI.Framework.GlobalProperties.ConnectionStatusProperties.SetConnectionState("Lock Verification Manager", false);
                        Trace.Message("{0} -> [ILockVerificationManager] Connection Monitor; Change state to ==> [DISCONNECTED]", DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Exception(ex);
            }
        }

        #endregion

        #region Public Methods

        public TransactionResult SendAuthorization(string user, string password, string comment)
        {
            try
            {
                return manager.SendAuthorization(user, password, comment);
            }
            catch (Exception ex)
            {
                Trace.Exception(ex, "Error in Send Authorization {0} ..", ex.Message);
                return new TransactionResult() { Code = -993, Message = "Error in Send Authorization" };
            }
        }

        public TransactionResult SaveAccept()
        {
            try
            {
                return manager.SaveAccept();
            }
            catch (Exception ex)
            {
                Trace.Exception(ex, "Error in Save Accept {0} ..", ex.Message);
                return new TransactionResult() { Code = -993, Message = "Error in Save Accept" };
            }
        }

        public Dictionary<int, string> GetTimeConfiguration()
        {
            try
            {
                return manager.GetTimeConfiguration();
            }
            catch (Exception ex)
            {
                Trace.Exception(ex, "Error in Send Authorization {0} ..", ex.Message);
                return null;
            }
        }

        #endregion
    }
}
