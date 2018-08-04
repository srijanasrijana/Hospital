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
    public class LeaveSetup
    {
        public static DataTable createLeavesetup(string leavename, string code, string Leavetype,int limitvalue,string limittype,bool isaccumulated,bool istransfertonextyear )
        {
            string str = "insert into HRM.tblLeaveSetup values('" + leavename + "','" + code + "','" + Leavetype + "','" + limitvalue + "','" + limittype + "','" + isaccumulated + "','" + istransfertonextyear + "')";
            return Global.m_db.SelectQry(str, "tblleavesetupmaster");
        }

        public static DataTable getLeaveMasterSetup()
        {
            string str = "select * from HRM.tblLeaveSetup";
            return Global.m_db.SelectQry(str, "tblAdditionDeduction");
        }

        public static DataTable getLeaveSetUpByID(int ID)
        {
            string str = "select * from HRM.tblLeaveSetup  where LeaveSetUpId=" + ID + "";
            return Global.m_db.SelectQry(str, "tblLeaveSetup");
        }

        public static DataTable updateLeaveSetUP(int leavesetupid,string leavename, string code, string Leavetype, int limitvalue, string limittype, bool isaccumulated, bool istransfertonextyear)
        {
            string str = "update HRM.tblLeaveSetup set LeaveName='" + leavename + "',Code='" + code + "',LeaveType='" + Leavetype + "',Limit='" + limitvalue + "',LimitType='" + limittype + "',IsAccumulated='" + isaccumulated + "',IsNextYearTransfer='" + istransfertonextyear + "'  where LeaveSetUpId='" + leavesetupid + "'";
            return Global.m_db.SelectQry(str, "tblLeaveSetup");
        }

        public static DataTable deleteFromLeaveSetup(int id)
        {
            string str = "delete from HRM.tblLeaveSetup where LeaveSetUpId=" + id + "";
            return Global.m_db.SelectQry(str, "tblLeaveSetup");
        }
    }
}
