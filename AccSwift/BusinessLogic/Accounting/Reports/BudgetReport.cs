using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogic.Accounting.Reports
{
   public class BudgetReport
    {
       public static DataTable GetBudgetReportData(int BudgetID,string ClassIDs,int ProjectID)
       {
           try
           {
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Acc.spBudgetReport");
               Global.m_db.AddParameter("@BudgetID", SqlDbType.Int, BudgetID);
               Global.m_db.AddParameter("@ClassIDs", SqlDbType.NVarChar,300, ClassIDs);
               Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);

               return Global.m_db.GetDataTable();
           }
           catch(Exception ex)
           {
               Global.Msg(ex.Message);
               return null;
           }
       }
    }
}
