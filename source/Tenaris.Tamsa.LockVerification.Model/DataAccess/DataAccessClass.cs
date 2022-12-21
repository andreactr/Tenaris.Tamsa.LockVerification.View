using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tenaris.Library.DbClient;
using Tenaris.Library.Log;
using Tenaris.Tamsa.LockVerification.Manager.Shared;
using Tenaris.Tamsa.LockVerification.Model.Configuration;

namespace Tenaris.Tamsa.LockVerification.Model.DataAccess
{
    public class DataAccessClass
    {
        #region Private Attributes

        /// <summary>
        /// Instance of the Class to be used
        /// </summary>
        private static DataAccessClass instance;

        /// <summary>
        /// Utility object for synchronization
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// DbClient instance.
        /// </summary>
        private readonly DbClient databaseClient;

        // <summary>
        /// Flag indicating whether this object is disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        /// String Connection 
        /// </summary>
        private string stringConnectionDB;

        /// <summary>
        /// Area Code
        /// </summary>
        private string codeArea;

        #endregion

        #region public Fields

        /// <summary>
        /// Instance of the Class to be used
        /// </summary>
        public static DataAccessClass Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new DataAccessClass();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccess"/> class.
        /// </summary>
        /// <param name="databaseClient"></param>
        /// <param name="idArea"></param>
        private DataAccessClass()
        {
            try
            {
                // Assign idArea
                this.codeArea = ViewConfiguration.Settings.AreaCode;
                this.stringConnectionDB = ViewConfiguration.Settings.ConnectionStringName;
                // Assign dbclient
                this.databaseClient = new DbClient(this.stringConnectionDB);

                this.databaseClient.AddCommand(StoredProcedures.SpGetLockVerificationHistory);
                
            }
            catch (Exception ex)
            {
                Trace.Exception(ex, "Failed to create DataAccess : {0}", ex.Message);
                throw;
            }
        }

        #endregion

        #region Private Methods

        public void DoUninitialize()
        {
            this.databaseClient.ClearCommands();
            this.databaseClient.Dispose();
        }

        #endregion

        #region Get Methods

        public List<LockVerificationHistory> GetLockVerificationHistory()
        {
            List<LockVerificationHistory> listLockVerificationHistory = new List<LockVerificationHistory>();

            try
            {
                using (var command = databaseClient.GetCommand(StoredProcedures.SpGetLockVerificationHistory))
                {
                    using (DataTable rdrm = command.ExecuteTable())
                    {
                        foreach (DataRow row in rdrm.Rows)
                        {
                            LockVerificationHistory lvh = new LockVerificationHistory();

                            lvh.idLockVerificationHistory = int.Parse(row[StoredProcedures.Fields.idLockVerificationHistory].ToString());
                            lvh.idStatusLockVerification = (LockVerificationStatus)int.Parse(row[StoredProcedures.Fields.idStatusLockVerification].ToString());
                            lvh.NameStatusLockVerification = row[StoredProcedures.Fields.NameStatusLockVerification].ToString();
                            lvh.idLastInspectionHistory = int.Parse(row[StoredProcedures.Fields.idLastInspectionHistory].ToString());
                            var lastInsDate = row[StoredProcedures.Fields.LastInspectionDate].ToString();
                            lvh.LastInspectionDate = lastInsDate == String.Empty ? DateTimeOffset.Now : DateTimeOffset.Parse(lastInsDate);

                            string temp1 = row[StoredProcedures.Fields.idUserAuthorization].ToString();

                            if (!string.IsNullOrEmpty(temp1))
                            {
                                lvh.idUserAuthorization = int.Parse(temp1);
                                lvh.UserAuthorization = row[StoredProcedures.Fields.UserAuthorization].ToString();
                                lvh.Comments = row[StoredProcedures.Fields.Comments].ToString();
                            }

                            lvh.idUserLoggedIn = int.Parse(row[StoredProcedures.Fields.idUserLoggedIn].ToString());
                            lvh.UserLoggedIn = row[StoredProcedures.Fields.UserLoggedIn].ToString();

                            lvh.OpenDateTime = DateTime.Parse(row[StoredProcedures.Fields.OpenDateTime].ToString());

                            string temp2 = row[StoredProcedures.Fields.AcceptDateTime].ToString();

                            if (!string.IsNullOrEmpty(temp2))
                            {
                                lvh.AcceptDateTime = DateTime.Parse(temp2);
                            }

                            string temp3 = row[StoredProcedures.Fields.idTrackingLock].ToString();

                            if (!string.IsNullOrEmpty(temp3))
                            {
                                lvh.idTrackingLock = int.Parse(temp3);
                            }

                            string temp4 = row[StoredProcedures.Fields.idItemStatusLock].ToString();

                            if (!string.IsNullOrEmpty(temp3))
                            {
                                lvh.idItemStatusLock = int.Parse(temp4);
                            }

                            lvh.InsertDateTime = DateTime.Parse(row[StoredProcedures.Fields.InsDateTime].ToString());


                            listLockVerificationHistory.Add(lvh);

                        }

                    }

                    return listLockVerificationHistory;

                }
            }
            catch (Exception ex)
            {
                Trace.Exception(ex, "Error on DataAccess Method: GetLasserLenghtMeasurement");
                return listLockVerificationHistory;
            }
        }
        
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.databaseClient.Dispose();
                }
            }
            this.disposed = true;
        }

        ~DataAccessClass()
        {
            this.Dispose(false);
        }

        #endregion
    }
}

