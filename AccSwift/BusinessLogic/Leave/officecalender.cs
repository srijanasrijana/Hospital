using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DateManager;

namespace BusinessLogic.Leave
{
    public class officecalender
    {
        public static DataTable GetEndofmonth(int Year)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select * from Date.NepDate where Years='" + Year + "'";
            dt = Global.m_db.SelectQry(strQuery, "NepDate");
            return dt;
        }

        public static void createofficeCalender(DataTable dtOfficeCalender)
        {
            try
            {
                Global.m_db.BeginTransaction();
                foreach (DataRow drcalender in dtOfficeCalender.Rows)
                {
                    string DateBS = drcalender["DateBS"].ToString();
                    string DateAD = drcalender["DateAD"].ToString();
                    string Day = drcalender["Day"].ToString();
                    int Workhour = Convert.ToInt32(drcalender["WorkHour"].ToString());
                    string Status = drcalender["Status"].ToString();
                    string Reason = drcalender["Reason"].ToString();
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("HRM.spCreateOfficeCalender");
                    Global.m_db.AddParameter("@DateBS", SqlDbType.NVarChar,10,DateBS);
                    Global.m_db.AddParameter("@DateAD", SqlDbType.NVarChar, 10, DateAD);
                    Global.m_db.AddParameter("@Day", SqlDbType.NVarChar,20, Day);
                    Global.m_db.AddParameter("@Workhour", SqlDbType.Int, Workhour);
                    Global.m_db.AddParameter("@Status", SqlDbType.NVarChar, 10, Status);
                    Global.m_db.AddParameter("@Reason", SqlDbType.NVarChar, 100, Reason);
               
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Office Calender");
                    }
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Office Calender");
                    }
                }
                Global.m_db.CommitTransaction();
                 
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                MessageBox.Show(ex.Message);  
            }
            
        }
    }
}
