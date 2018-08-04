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
    public class LeaveRequest
    {
        public static DataTable getEmployeeInfoByCode(string employeecode)
        {
            string str = "select * from HRM.tblEmployee where staffcode='"+employeecode+"'";
            return Global.m_db.SelectQry(str, "tblEmployeeInformation");
        }
        public static DataTable getLeaveType()
        {
            string str = "select * from HRM.tblLeaveSetup";
            return Global.m_db.SelectQry(str, "tblLeavesetup");
        }

        public static bool InsertLeaveRequest(int employeeid,int leavetypeid,string leavetypename,DateTime fromdate,DateTime todate,bool halfleave,string reason)
        {
                  Global.m_db.ClearParameter();
                  Global.m_db.setCommandType(CommandType.StoredProcedure);
                  Global.m_db.setCommandText("HRM.spInsertLeaveRequest");
                  Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int,employeeid);
                  Global.m_db.AddParameter("@LeaveTypeID", SqlDbType.Int,leavetypeid);
                  Global.m_db.AddParameter("@LeaveTypeName", SqlDbType.NVarChar,20, leavetypename);
                  Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime,fromdate);
                  Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime,todate);
                  Global.m_db.AddParameter("@Halfleave", SqlDbType.Bit,halfleave);
                  Global.m_db.AddParameter("@Reason", SqlDbType.NVarChar, 100, reason);
                  System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                  Global.m_db.ProcessParameter();
                  if (paramReturn.Value.ToString() == "SUCCESS")
                      return true;
                  else
                      return false;
        }

        public static DataTable getApprovedLeave(int EmployeeID,int LeaveTypeID,DateTime FromDate,DateTime ToDate)
        {
          //  string str = "select * from hrm.tblLeaveRequest where EmployeeID='"+EmployeeID+"' and LeaveTypeID='"+LeaveTypeID+"' and LeaveAppliedDate Between "+FromDate+" and "+ToDate+"";
            //string str = "select * from hrm.tblLeaveRequest where EmployeeID='" + EmployeeID + "' and LeaveTypeID='" + LeaveTypeID + "'";
            //return Global.m_db.SelectQry(str, "tblLeaveApprovedInformation");
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("HRM.spGetLeaveRequest");
            Global.m_db.AddParameter("@EmployeeID", SqlDbType.Int, EmployeeID);
            Global.m_db.AddParameter("@LeaveTypeID", SqlDbType.Int, LeaveTypeID);
            Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
            Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
            DataTable dtLeaveApprove = Global.m_db.GetDataTable();
            return dtLeaveApprove; 
        }

        public static int GetMonthEnd(int Year,string MonthName)
        {
            object returnID;
            string query = "select " + MonthName + " from Date.NepDate where Years='" + Year + "'";
           // returnID = Global.m_db.GetScalarValue("select '"+MonthName+"' from Date.NepDate where Years='" + Year + "'");
            returnID = Global.m_db.GetScalarValue(query);
            return Convert.ToInt32(returnID);
        }
    }
}
