using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DateManager;


namespace BusinessLogic
{

    /************************************
     * Class Name: AuditLog
     * Version: 1.1.3 
     * Created By: Shamit Shrestha
     * Created Date: 1st Jul, 2014
     * Modified Date: 23rd Jan, 2015
     * *********************************/

   
    public class AuditLogDetail
    {
        public string  ComputerName{ get; set; }
        public string  UserName{ get; set; }
        public string  Voucher_Type{ get; set; }
        public string  Action{ get; set; }
        public string  Description{ get; set; }
        public int   RowID{ get; set; }
        public string  MAC_Address{ get; set; }
        public string  IP_Address{ get; set; }
        public string VoucherDate { get; set; }

         


        public void CreateAuditLog(AuditLogDetail auditlog)   
       {


        
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("system.spAddAuditLog");

            Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, auditlog.ComputerName);
            Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50,auditlog.UserName);
            Global.m_db.AddParameter("@voucher_Type", SqlDbType.NVarChar, 50, auditlog.Voucher_Type);
            Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50,auditlog.Action);
            Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, auditlog.Description);
            Global.m_db.AddParameter("@RowID", SqlDbType.Int, auditlog.RowID);
            Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50,auditlog.MAC_Address);
            Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50,auditlog.IP_Address);
            Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, auditlog.VoucherDate);
            
            
            Global.m_db.ProcessParameter();


        }
     }
    public class AuditLog
    {
        public static DataTable GetUserTable(int AuditLogID)
        {
            DataTable dt = new DataTable();
            string strQuery = "";

            if (AuditLogID == -1)//All Groups
            {
                strQuery = "SELECT * FROM system.tblUser";
            }
            return Global.m_db.SelectQry(strQuery, "tblauditlog");

        }

        public static DataTable GetLogInfo(string filtervalue,string username,DateTime fromdate,DateTime todate)
        {
            DataTable dt = new DataTable();
            Global.m_db.ClearParameter();
            Global.m_db.setCommandText("System.spGetAuditLogReport");
            Global.m_db.setCommandType( CommandType.StoredProcedure);
            Global.m_db.AddParameter("@VoucherTypes", SqlDbType.NVarChar,1000,filtervalue);
            Global.m_db.AddParameter("@userName", SqlDbType.NVarChar, 50, username);
            Global.m_db.AddParameter("@fromDate", SqlDbType.Date, fromdate);
            Global.m_db.AddParameter("@toDate", SqlDbType.Date, todate);
            dt = Global.m_db.GetDataTable();

            //if (username == " ")
            //{
            //    string SQL = "SELECT * FROM System.tblAuditLog WHERE Voucher_Type IN ( "+ (filtervalue) +"  ) and VoucherDate between '" + Date.ToDB(fromdate) + "' and '" + Date.ToDB(todate) + "'";
            //    dt = Global.m_db.SelectQry(SQL, "AuditLog");
            //}
            //else
            //{
            //    dt = Global.m_db.SelectQry("SELECT * FROM System.tblAuditLog WHERE Voucher_Type IN (" + (filtervalue) + ") and VoucherDate between '" +Date.ToDB( fromdate) + "' and '" +Date.ToDB( todate )+ "' and UserName = '" + username + "'", "AuditLog");
            //}
            return dt;
        }


        /// <summary>
        /// Writes the log into Audit Log
        /// </summary>
        /// <param name="ComputerName"></param>
        /// <param name="username"></param>
        /// <param name="VoucherType"></param>
        /// <param name="action"></param>
        /// <param name="description"></param>
        /// <param name="rowid"></param>
        /// <param name="voucherdate"></param>
        /// <returns></returns>
        public static bool AppendLog(string ComputerName, string username, string VoucherType, string action, string description, int rowid, DateTime voucherdate)
        {  
            try
            {

                string SQL = "INSERT INTO System.tblAuditLog(computername,username,voucher_type,action,description,rowid,voucherdate) VALUES ('" + ComputerName + "','" + username + "','" + VoucherType + "','" + action + "','" + description + "','" + rowid.ToString() + "','" + voucherdate + "')";
                Global.m_db.InsertUpdateQry(SQL);
                return true;
            }
            catch (Exception ex)
            {
                Global.MsgError("Error while writing the AuditLog. Message: " + ex.Message);
            }
            return false;

              
        }
    }
}
