using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tenaris.Tamsa.LockVerification.Model.Configuration
{
    public class ViewConfiguration : ConfigurationSection
    {
        public static ViewConfiguration Settings
        {
            get { return (ViewConfiguration)ConfigurationManager.GetSection("ViewConfiguration"); }
            set { ConfigurationManager.RefreshSection("ViewConfiguration"); }
        }

        [ConfigurationProperty("ApplicationName", IsRequired = true)]
        public string ApplicationName
        {
            get
            {
                return this["ApplicationName"].ToString();
            }
            set
            {
                this["ApplicationName"] = value;
            }
        }        

        [ConfigurationProperty("AreaCode", IsRequired = true)]
        public string AreaCode
        {
            get
            {
                return this["AreaCode"].ToString();
            }
            set
            {
                this["AreaCode"] = value;
            }
        }

        [ConfigurationProperty("ConnectionStringName", IsRequired = true)]
        public string ConnectionStringName
        {
            get
            {
                return this["ConnectionStringName"].ToString();
            }
            set
            {
                this["ConnectionStringName"] = value;
            }
        }

    }
}
