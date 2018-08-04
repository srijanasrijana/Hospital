using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogic
{
    public class ReportPreference
    {
        public DataTable GetPreferenceInfo(int userid,string reporttype)
        {
            DataTable dt = new DataTable();
            string strQuery = "select rp.Title,rp.Code,urp.Value from System.tblReportPreferences rp,system.tblUserReportPreferences urp where rp.ReportPreferenceID=urp.ReportPreferenceID and urp.UserID='" + userid + "' and ReportType='"+reporttype+"'";
            dt = Global.m_db.SelectQry(strQuery, "tblreportpreference");
            return dt;
        }
        public string GetUserPreference(string Code)
        {
            try
            {
                object Result = Global.m_db.GetScalarValue("SELECT ReportPreferenceID FROM System.tblReportPreferences WHERE Code='" + Code + "'");
                return Result.ToString();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool UpdateUserPreference(int userid, int preferenceid, string value)
        {
            try
            {
                Global.m_db.InsertUpdateQry("UPDATE System.tblUserReportPreferences SET Value='" + value + "' where userid='" + userid + "' and ReportPreferenceID='" + preferenceid + "' ");
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool SetUserPreference(int userid, int preferenceid, string value)
        {
            try
            {
                Global.m_db.InsertUpdateQry("insert into System.tblUserReportPreferences values('" + userid + "','" + preferenceid + "','" + value + "')");
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable GetPreferenceCount(int userid,string reporttype)
        {
            DataTable dt = new DataTable();
            string strQuery = "select p.Title,p.Code,up.Value from System.tblReportPreferences p,System.tblUserReportPreferences up where p.ReportPreferenceID=up.ReportPreferenceID and up.UserID='"+userid+"'and p.ReportType='"+reporttype+"'";
            dt = Global.m_db.SelectQry(strQuery, "tblreportpreference");
            return dt;
        }
    }
}
