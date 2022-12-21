﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenaris.Library.ConnectionMonitor;

namespace Tenaris.Tamsa.LockVerification.Model.Configuration
{
    /// <summary>
    /// adquire the configuration of app.conf 
    /// </summary>
    public static class ProxyConfiguration
    {

        private static object lockConfiguration;
        /// <summary>
        /// flag that indicate if configure remoting
        /// </summary>
        public static bool IsRemotingConfigured { get; private set; }

        /// <summary>
        /// create instance and Initialize members
        /// </summary>
        static ProxyConfiguration()
        {
            ProxyConfiguration.lockConfiguration = new object();
            ProxyConfiguration.IsRemotingConfigured = false;
        }
        /// <summary>
        /// configure by remoting 
        /// </summary>
        public static void ConfigureRemoting()
        {
            lock (ProxyConfiguration.lockConfiguration)
            {
                if (!ProxyConfiguration.IsRemotingConfigured)
                {
                    System.Runtime.Remoting.RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
                    ProxyConfiguration.IsRemotingConfigured = true;
                }
            }
        }

        /// <summary>
        /// dispose the connection Monitor
        /// </summary>
        public static void DisposeConnectionMonitor()
        {
            if (ConnectionMonitor.Instance != null)
            {
                ConnectionMonitor.Instance.Dispose();
            }
        }
    }
}
