using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

using System.Data;

namespace BusinessLogic
{
   public  class Depreciation
    {
       public void SaveDepreciation(DataTable dt)
       {
           string strQuery = "delete System.tblDepreciation";
           Global.m_db.SelectQry(strQuery, "tbl");
           
           try
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   DataRow dr = dt.Rows[i];
                   Global.m_db.ClearParameter();
                   Global.m_db.setCommandType(CommandType.StoredProcedure);
                   Global.m_db.setCommandText("System.spSaveDepreciationInfo");
                   Global.m_db.AddParameter("@LedgerID", SqlDbType.Int,Convert.ToInt32( dr["LedgerID"].ToString()));
                   Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 50, dr["LedgerName"].ToString());
                   Global.m_db.AddParameter("@DepreciationValue", SqlDbType.Int,Convert.ToInt32( dr["Depreciationvalue"].ToString()));
                   System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                   Global.m_db.ProcessParameter();
               }
               Global.Msg("Successfully Saved");
            
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);   
           }
       }
       public DataTable GetDataFromDepreciation(int ledgerid)
       {
           DataTable dt = new DataTable();
           string strQuery = "select * from System.tblDepreciation where LedgerID='"+ledgerid+"'";    
           dt = Global.m_db.SelectQry(strQuery, "Search");
           return dt;
       }
    }
}
