using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLogic.HRM.Report
{
    public class SalaryList
    {
        public DataTable GetSalaryList(EmployeeReportSettings settings)//(int[] paySlipIDs, bool isRemaining, string date)
        {
            string paySlips = Employee.ConvertIDsToString(settings.paySlipIds);
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("HRM.spGetSalaryList");
            Global.m_db.AddParameter("@paySlipID", SqlDbType.NVarChar, paySlips);
            Global.m_db.AddParameter("@Date", SqlDbType.NVarChar, settings.paySlipDate);
            Global.m_db.AddParameter("@fromDate", SqlDbType.Date, settings.fromDate);
            Global.m_db.AddParameter("@toDate", SqlDbType.Date, settings.toDate);

            Global.m_db.AddParameter("@isRemaining", SqlDbType.Bit, settings.isRemaining);

            DataTable dt = Global.m_db.GetDataTable();
            dt.DefaultView.Sort = "StaffName";  // sort the datatable according to MasterID in ascending order
            dt = dt.DefaultView.ToTable();
            return dt;
        }
    }
}
