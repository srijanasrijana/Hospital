using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace BusinessLogic.Accounting
{
    public class MultipleLedger
    {
      
        public void CreateMultipleLedger(DataTable dt,string ledgerhead)
        {
         //private string s;
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spMultipleLedgerCreate");
                    if (dr["LedgerName"].ToString() != "")
                    {
                        Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, dr["LedgerName"].ToString());
                    }
                    else
                    {
                        Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, null);
                    }
                    Global.m_db.AddParameter("@Under_EngName", SqlDbType.NVarChar, 50, ledgerhead);
                    Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, dr["LedgerCode"].ToString());
                    Global.m_db.AddParameter("@PreYrBal", SqlDbType.Money, null);
                    Global.m_db.AddParameter("@PreYrBalDrCr", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@OpCCYID", SqlDbType.Int, 1);
                    Global.m_db.AddParameter("@OpCCR", SqlDbType.Money, null);
                    Global.m_db.AddParameter("@OpCCRDate", SqlDbType.DateTime, null);
                    //Global.m_db.AddParameter("@OpDrCr", SqlDbType.NVarChar,10, OpBalance_DrCr);
                    Global.m_db.AddParameter("@PerName", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@Address1", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@Address2", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@City", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@Phone", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@Email", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@Company", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@Website", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@VatPanNo", SqlDbType.NVarChar, 50, null);
                    Global.m_db.AddParameter("@CreditLimit", SqlDbType.Money, null);
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, null);
                    Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrentUserName);
                    //Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                   // Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50,null);
                    SqlParameter objReturn = (SqlParameter)Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    SqlParameter objReturnID = Global.m_db.AddOutputParameter("@ReturnID", SqlDbType.Int);
                    Global.m_db.ProcessParameter();
                    //return (objReturn.Value.ToString() == "SUCCESS" ? true : false);
                    string s = objReturn.Value.ToString(); 
                }
                //MessageBox.Show("Ledger Created Successfully");
                Global.Msg("Ledger created successfully!");
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         

        }
    }
}
