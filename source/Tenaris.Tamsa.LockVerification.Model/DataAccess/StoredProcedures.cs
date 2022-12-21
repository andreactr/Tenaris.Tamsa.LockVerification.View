using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tenaris.Tamsa.LockVerification.Model.DataAccess
{
    public class StoredProcedures
    {
        #region Get Stored Procedures

        public const string SpGetLockVerificationHistory = "[Ndt_Tamsa].[GetLockVerificationHistory]";

        #endregion

        #region Fields

        public static class Fields
        {
            /// <summary>
            /// 
            /// </summary>
            public const string idLockVerificationHistory = "idLockVerificationHistory";

            /// <summary>
            /// 
            /// </summary>
            public const string idStatusLockVerification = "idStatusLockVerification";

            /// <summary>
            /// 
            /// </summary>
            public const string NameStatusLockVerification = "NameStatusLockVerification";

            /// <summary>
            /// 
            /// </summary>
            public const string idLastInspectionHistory = "idLastInspectionHistory";

            /// <summary>
            /// 
            /// </summary>
            public const string LastInspectionDate = "LastInspectionDate";

            /// <summary>
            /// 
            /// </summary>
            public const string idUserAuthorization = "idUserAuthorization";

            /// <summary>
            /// 
            /// </summary>
            public const string UserAuthorization = "UserAuthorization";

            /// <summary>
            /// 
            /// </summary>
            public const string Comments = "Comments";
            
            /// <summary>
            /// 
            /// </summary>
            public const string idUserLoggedIn = "idUserLoggedIn";

            /// <summary>
            /// 
            /// </summary>
            public const string UserLoggedIn = "UserLoggedIn";

            /// <summary>
            /// 
            /// </summary>
            public const string OpenDateTime = "OpenDateTime";

            /// <summary>
            /// 
            /// </summary>
            public const string AcceptDateTime = "AcceptDateTime";

            /// <summary>
            /// 
            /// </summary>
            public const string idTrackingLock = "idTrackingLock";

            /// <summary>
            /// 
            /// </summary>
            public const string idItemStatusLock = "idItemStatusLock";

            /// <summary>
            /// 
            /// </summary>
            public const string InsDateTime = "InsDateTime";

        }

        #endregion

    }
}
