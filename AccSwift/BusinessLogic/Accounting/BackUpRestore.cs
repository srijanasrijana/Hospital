using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo; 
using System.Data;
using RegistryManager;
using System.Data.SqlClient;

namespace BusinessLogic
{
    public class BackUpRestore
    {
        
        private static BackUpRestore _TestObject;
        private static Object _locker = new Object();

        private BackUpRestore()
        {

        }

        public BackUpRestore GetInstance()
        {
            lock (_locker)
            {
                if (_TestObject == null)
                    _TestObject = new BackUpRestore();
                return _TestObject;
            }
        }


        public static void BackMyDB(string dbname, string path)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("System.spBackUpDB");
            Global.m_db.AddParameter("@db", SqlDbType.NVarChar, 50, dbname);
            Global.m_db.AddParameter("@d", SqlDbType.NVarChar, 100, path);
            //cmd.Parameters.Add(new SqlParameter("@yapilanIslemTuru", SqlDbType.NVarChar)).Value = yapilanIslemTipi;               
            Global.m_db.ProcessParameter();

            Global.Msg("BackUp Completes!!");
        }


        public static void BackupDatabase(String databaseName, String destinationPath)
        {
            try
            {
                ServerConnection connection = new ServerConnection(Global.m_db.cn);

                Server sqlServer = new Server(connection);

                Database db = sqlServer.Databases[databaseName];

                Backup sqlBackup = new Backup();
                sqlBackup.Action = BackupActionType.Database;
                sqlBackup.BackupSetDescription = "ArchiveDataBase:" + DateTime.Now.ToShortDateString();
                sqlBackup.BackupSetName = "Archive";

                sqlBackup.Database = databaseName;

                BackupDeviceItem deviceItem = new BackupDeviceItem(destinationPath, DeviceType.File);

                sqlBackup.Initialize = true;
                sqlBackup.Checksum = true;
                sqlBackup.ContinueAfterError = true;

                sqlBackup.Devices.Add(deviceItem);
                sqlBackup.Incremental = false;

                //sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);
                sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

                sqlBackup.FormatMedia = false;
                sqlBackup.SqlBackup(sqlServer);
            }
            catch (Exception ex)
            {
            }

        }      

        public static bool RestoreDatabase(String databaseName, String filePath, String dataFilePath, String logFilePath)
        {
            try
            {
                Restore sqlRestore = new Restore();

                BackupDeviceItem deviceItem = new BackupDeviceItem(filePath, DeviceType.File);
                sqlRestore.Devices.Add(deviceItem);
                sqlRestore.Database = databaseName;
                ServerConnection connection = new ServerConnection(RegistryManager.RegManager.ServerName, RegistryManager.RegManager.DBUser, RegistryManager.RegManager.DBPassword);
                //ServerConnection connection = new ServerConnection(ConnectionDetail);
                Server sqlServer = new Server(connection);
                Database db = sqlServer.Databases[databaseName];
                sqlRestore.Action = RestoreActionType.Database;
                db = sqlServer.Databases[databaseName];
                DataTable dtFileList = sqlRestore.ReadFileList(sqlServer);
                string dbLogicalName = dtFileList.Rows[0][0].ToString();
                string dbPhysicalName = dtFileList.Rows[0][1].ToString();
                string logLogicalName = dtFileList.Rows[1][0].ToString();
                string logPhysicalName = dtFileList.Rows[1][1].ToString();
                RelocateFile rf = new RelocateFile(databaseName, dbLogicalName);
                sqlRestore.RelocateFiles.Add(new RelocateFile(dbLogicalName, dataFilePath));
                sqlRestore.RelocateFiles.Add(new RelocateFile(logLogicalName, logFilePath));
                sqlRestore.ReplaceDatabase = true;
                sqlRestore.Complete += new ServerMessageEventHandler(sqlRestore_Complete);
                sqlRestore.PercentCompleteNotification = 10;
                sqlRestore.PercentComplete +=
                new PercentCompleteEventHandler(sqlRestore_PercentComplete);

                sqlRestore.SqlRestore(sqlServer);
                db = sqlServer.Databases[databaseName];
                db.SetOnline();
                sqlServer.Refresh();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static event EventHandler<PercentCompleteEventArgs> PercentComplete;

        public static void sqlRestore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            if (PercentComplete != null)
                PercentComplete(sender, e);

        }

        public static event EventHandler<ServerMessageEventArgs> Complete;

       public static void sqlRestore_Complete(object sender, ServerMessageEventArgs e)
        {
            if (Complete != null)
                Complete(sender, e);
        }


        //used to get the file names to recreate.
        public string GetDBFilePath(String databaseName, String userName, String password, String serverName)
        {
            ServerConnection connection = new ServerConnection(serverName, userName, password);
            Server sqlServer = new Server(connection);
            Database db = sqlServer.Databases[databaseName];
            return sqlServer.Databases[databaseName].PrimaryFilePath;
        }

        //GetServerMethod
        public Server GetSqlServer(string server, bool integratedSecurity, string loginName, string loginPassword)
        {
            try
            {
                Server serverInstance = new Microsoft.SqlServer.Management.Smo.Server(server);
                if (integratedSecurity)
                {
                    serverInstance.ConnectionContext.LoginSecure = false;
                    serverInstance.ConnectionContext.Login = loginName;
                    serverInstance.ConnectionContext.Password = loginPassword;
                }
                return serverInstance;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region       Copy data helper

        //         Server sqlServer;

        //        public DataCopyHelper(String serverName, String userName, String password)
        //        {
        //            sqlServer = new Server(new ServerConnection(serverName, userName, password));
        //        }

        //        public void CopyData(String sourceDatabase, String destinationDataBase)
        //        {
        //            Database dbSource = sqlServer.Databases[sourceDatabase];
        //            Database dbDestination = sqlServer.Databases[destinationDataBase];

        //            if (dbDestination == null || dbSource == null)
        //                throw new Exception("Specified Database not found the server " + sqlServer.Name);
        //            StringBuilder sqlScript = new StringBuilder("");
        //            sqlScript.AppendLine("USE " + destinationDataBase + ";");
        //            sqlScript.AppendLine("");

        //            foreach (Table dataTable in dbSource.Tables)
        //            {
        //                if (!dbDestination.Tables.Contains(dataTable.Name, dataTable.Schema))
        //                    continue;
        //                sqlScript.AppendFormat("INSERT INTO {0} \n SELECT * FROM {0}", dataTable.Name);
        //                sqlScript.AppendLine();
        //            }

        //            dbDestination.ExecuteNonQuery(sqlScript.ToString());
        //        }

        //        public void TrancateData(String[] tableNames, String databaseName)
        //        {
        //            StringBuilder sqlScript = new StringBuilder("");
        //            sqlScript.AppendFormat("USE {0};", databaseName);
        //            sqlScript.AppendLine();
        //            foreach (String tableName in tableNames)
        //            {
        //                sqlScript.AppendFormat("TRUNCATE TABLE {0}", tableName);
        //                sqlScript.AppendLine();
        //            }

        //            Database db = sqlServer.Databases[databaseName];
        //            db.ExecuteNonQuery(sqlScript.ToString());
        //        }

        //        public void TruncateDatabase(String databaseName)
        //        {
        //            Database db = sqlServer.Databases[databaseName];
        //            if(db==null)
        //                throw new Exception("Specified Database not found the server " + sqlServer.Name);
        //            List<String> tables = new List<string>();
        //            foreach (Table dataTable in db.Tables)
        //            {
        //                tables.Add(dataTable.Name);
        //            }

        //            this.TrancateData(tables.ToArray(), databaseName);
        //        }

        #endregion

    }
}