using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using DBLogic;
using System.Windows.Forms;
using RegistryManager;

namespace BusinessLogic
{

    public class DatabaseParam
    {        
        private string _ServerName;

        public string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }

        private string _DatabaseName;

        public string DatabaseName
        {
            get { return _DatabaseName; }
            set { _DatabaseName = value; }
        }

        //Data file parameters
        private string _DataFileName;

        public string DataFileName
        {
            get { return _DataFileName; }
            set { _DataFileName = value; }
        }

        private string _DataPathName;

        public string DataPathName
        {
            get { return _DataPathName; }
            set { _DataPathName = value; }
        }

        private string _DataFileSize;

        public string DataFileSize
        {
            get { return _DataFileSize; }
            set { _DataFileSize = value; }
        }

        private string _DataFileGrowth;

        public string DataFileGrowth
        {
            get { return _DataFileGrowth; }
            set { _DataFileGrowth = value; }
        }

        //Log file parameters
        public string _LogFileName;

        public string LogFileName
        {
            get { return _LogFileName; }
            set { _LogFileName = value; }
        }

        private string _LogPathName;

        public string LogPathName
        {
            get { return _LogPathName; }
            set { _LogPathName = value; }
        }

        private string _LogFileSize;

        public string LogFileSize
        {
            get { return _LogFileSize; }
            set { _LogFileSize = value; }
        }

        private string _LogFileGrowth;

        public string LogFileGrowth
        {
            get { return _LogFileGrowth; }
            set { _LogFileGrowth = value; }
        }

    }

    public static class CreateDB
    {     
         
        public static Boolean IsDBExist( string TestDBName)
        {
            
            //In case of SQL Server connection
            SqlDb m_SqlDb = new SqlDb();
            m_SqlDb.ServerName = RegManager.ServerName;
            // m_SqlDb.DbName = myReg.Read("DATABASE");
            m_SqlDb.DbName = "master";
            m_SqlDb.UserName = RegManager.DBUser;
            m_SqlDb.Password = RegManager.DBPassword;
            m_SqlDb.Connect();
            DataTable dt = new DataTable();
            string strQuery = "SELECT * FROM sys.sysdatabases where name= '" + TestDBName + "'";
            dt =  m_SqlDb.SelectQry(strQuery, "sys.sysdatabases");
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;        
        }

        public static void CreateDatabase(DatabaseParam DBParam)
        {

            //In case of SQL Server connection
            SqlDb m_SqlDb = new SqlDb();
            m_SqlDb.ServerName = RegManager.ServerName;
            // m_SqlDb.DbName = myReg.Read("DATABASE");
            m_SqlDb.DbName = "master";
            m_SqlDb.UserName = RegManager.DBUser;
            m_SqlDb.Password = RegManager.DBPassword;
            m_SqlDb.Connect();

            string sqlCreateDBQuery = " CREATE DATABASE " + DBParam.DatabaseName + " ON PRIMARY "
                                + " (NAME = " + DBParam.DataFileName + ", "
                                + " FILENAME = '" + DBParam.DataPathName + "', "
                                + " SIZE = 6MB,"
                                + "	FILEGROWTH =" + DBParam.DataFileGrowth + ") "
                                + " LOG ON (NAME =" + DBParam.LogFileName + ", "
                                + " FILENAME = '" + DBParam.LogPathName + "', "
                                + " SIZE = 1MB, "
                                + "	FILEGROWTH =" + DBParam.LogFileGrowth + ") ";
            
            try
            {
                m_SqlDb.SelectQry(sqlCreateDBQuery,"mastertable");

                //MessageBox.Show("Database has been created successfully!");
            }
            catch (System.Exception ex)
            {
              MessageBox.Show(ex.ToString());
            }
            finally
            {
              //  tmpConn.Close();
            }
            return;
        }
    }

}
