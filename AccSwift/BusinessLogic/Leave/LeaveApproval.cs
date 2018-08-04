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
    public class LeaveApproval
    {
        public static DataTable getLeaveRequest()
        {
            string str = "select LR.*,E.FirstName+E.LastName EmployeeName from hrm.tblLeaveRequest LR,HRM.tblEmployee E where LR.EmployeeID=E.ID order by LeaveAppliedDate desc";
            return Global.m_db.SelectQry(str, "tblleaverequest");
        }

        public static DataTable updateLeaveRequest(int leaverequestid,bool isapproved)
        {
             string str="";
            if(isapproved==true)
                str = "update hrm.tblLeaveRequest set isapproved='"+isapproved+"' where LeaveRequestID=1";
            else
                str = "update hrm.tblLeaveRequest set isapproved='" + isapproved + "' where LeaveRequestID=0";
            return Global.m_db.SelectQry(str, "tblLeaveRequest");
        }
    }
}
