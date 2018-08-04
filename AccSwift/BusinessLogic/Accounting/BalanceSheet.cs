using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Collections;

namespace BusinessLogic
{
   public  class BalanceSheet
    {
       public static string RptPreferences(int UserID,string  RptXMLPreferences)
       {
           try
           {
               Global.m_db.ClearParameter();
               Global.m_db.setCommandType(CommandType.StoredProcedure);
               Global.m_db.setCommandText("Inv.spReportPreferences");
               Global.m_db.AddParameter("@UserID", SqlDbType.Int, UserID);
               Global.m_db.AddParameter("@ReportPreferencesInfo", SqlDbType.Xml, RptXMLPreferences);
               System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
               Global.m_db.ProcessParameter();
               if (paramReturn.Value.ToString() == "INSERT")
                   return "INSERT";
               else if (paramReturn.Value.ToString() == "UPDATE")
               {
                   return "UPDATE";
               }
               else
               {
                   return "FAILURE";
               }
                  
               
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
               return "FAILURE";
           }
       }
  
    }
}
