using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace BusinessLogic.Accounting
{
    public class BulkPosting
    {
        public  DataTable GetBulkMasterinfo()
        {
            DataTable dt = new DataTable();
            string strQuery = "SELECT * FROM Acc.tblBulkPostingMaster";
            return Global.m_db.SelectQry(strQuery,"tblbulkmaster");  
        }
        public string insertbulkvoucher(string catname)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spBulkVoucherCreate");
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, catname);
                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                if (paramReturn.Value.ToString() == "SUCCESS")
                    return "SUCCESS";
                else
                    return "FAILURE";
               // return objReturn.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return "FAILURE";
            }
        }

        public string updatebulkvoucher(string catname,int id)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spBulkVoucherUpdate");
                Global.m_db.AddParameter("@Name", SqlDbType.NVarChar, 50, catname);
                Global.m_db.AddParameter("@id", SqlDbType.Int, id);
                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                if (paramReturn.Value.ToString() == "SUCCESS")
                    return "SUCCESS";
                else
                    return "FAILURE";
               
            }
                
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return "FAILURE";
            }
        }

        public bool deletebulkvoucherrow(int id)
        {
            try
            {
                Global.m_db.InsertUpdateQry("DELETE Acc.tblbulkpostingmaster WHERE ID='" + id.ToString() + "'");
                return true;
            }
            catch (Exception ex)
            {   
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void insertintobulkpostingdetail(DataTable dt,int id,int sid,int pid,int Rowid)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++ )
                {
                    DataRow dr = dt.Rows[i];
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spBulkVoucherDetails");
                    Global.m_db.AddParameter("@BulkPostingMasterID", SqlDbType.Int, id);
                    Global.m_db.AddParameter("@Particulars", SqlDbType.NVarChar, 50, dr["Particulars"].ToString());
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 50, dr["DrCr"].ToString());
                    Global.m_db.AddParameter("@Amount", SqlDbType.Float,Convert.ToDouble( dr["Amount"].ToString()));
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int,Convert.ToInt32(dr["LedgerID"].ToString()));
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 50, dr["VoucherType"].ToString());
                    Global.m_db.AddParameter("@VoucherNumber", SqlDbType.NVarChar, 50, dr["VoucherNo"].ToString());
                 
                    if (dr["Remarks"].ToString()==" ")
                    {
                        Global.m_db.AddParameter("@remarks", SqlDbType.NVarChar, 50, " ");
                    }
                    else
                        Global.m_db.AddParameter("@remarks", SqlDbType.NVarChar, 50, dr["Remarks"].ToString());
                    Global.m_db.AddParameter("@sid", SqlDbType.Int, sid);
                    Global.m_db.AddParameter("@pid", SqlDbType.Int, pid);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, Rowid);
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                }
                //if (paramReturn.Value.ToString() == "SUCCESS")
                //    return "SUCCESS";
                //else
                //    return "FAILURE";
                // return objReturn.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                //return "FAILURE";
            }
        }

        public DataTable GetAllDataFromBulk(int id)
        {
            string strQuery = "SELECT * FROM  Acc.tblBulkPostingDetails WHERE BulkPostingMasterID='" + id + "'";
            DataTable dt = Global.m_db.SelectQry(strQuery, "tbl");
            return dt;
        }

        public void ChangeBulkPostingInfo(DataTable dt,int BulkPostingMasterID)
        {
            try
            {
                 Global.m_db.BeginTransaction();
                 Global.m_db.ClearParameter();
                 Global.m_db.setCommandType(CommandType.StoredProcedure);
                 Global.m_db.setCommandText("Acc.spDeleteBulkPosting");
                 Global.m_db.AddParameter("@BulkMasterID", SqlDbType.Int, BulkPostingMasterID);
                 System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                 Global.m_db.ProcessParameter();
                 if (objReturn.Value.ToString() != "SUCCESS")
                 {
                     Global.m_db.RollBackTransaction();
                     throw new Exception("Unable to Insert");
                 }

                //Insert Update Data to BulkPosting
                 for (int i = 0; i < dt.Rows.Count; i++)
                 {
                     DataRow dr = dt.Rows[i];
                     Global.m_db.ClearParameter();
                     Global.m_db.setCommandType(CommandType.StoredProcedure);
                     Global.m_db.setCommandText("Acc.spBulkVoucherDetails");
                     Global.m_db.AddParameter("@BulkPostingMasterID", SqlDbType.Int, BulkPostingMasterID);
                     Global.m_db.AddParameter("@Particulars", SqlDbType.NVarChar, 50, dr["Particulars"].ToString());
                     Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 50, dr["DrCr"].ToString());
                     Global.m_db.AddParameter("@Amount", SqlDbType.Float, Convert.ToDouble(dr["Amount"].ToString()));
                     Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Convert.ToInt32(dr["LedgerID"].ToString()));
                     Global.m_db.AddParameter("@sid", SqlDbType.Int, Convert.ToInt32(dr["sid"].ToString()));
                     Global.m_db.AddParameter("@pid", SqlDbType.Int, Convert.ToInt32(dr["pid"].ToString()));
                     Global.m_db.AddParameter("@RowID", SqlDbType.Int, Convert.ToInt32(dr["rowid"].ToString()));
                     Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 50, dr["VoucherType"].ToString());
                     Global.m_db.AddParameter("@VoucherNumber", SqlDbType.NVarChar, 50, dr["VoucherNumber"].ToString());
                     if (dr["Remarks"].ToString() == " ")
                     {
                         Global.m_db.AddParameter("@remarks", SqlDbType.NVarChar, 50, " ");
                     }
                     else
                         Global.m_db.AddParameter("@remarks", SqlDbType.NVarChar, 50, dr["Remarks"].ToString());
                     System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                     Global.m_db.ProcessParameter();

                     if (paramReturn.Value.ToString() != "SUCCESS")
                     {
                         Global.m_db.RollBackTransaction();
                         throw new Exception("Unable to Insert");
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

        public DataTable GetSeriesID(int id,string vouchertype)
        {
            DataTable dt = new DataTable();
            string strQuery = "SELECT SeriesID FROM Acc.tblBulkPostingDetails where BulkPostingMasterID='"+id+"' and VoucherType='"+vouchertype+"'";
            return Global.m_db.SelectQry(strQuery, "tblbulkdetail");    
        }

        public DataTable GetAccClassIDs(int rowid,string vouchertype)
        {
            DataTable dt = new DataTable();
            string strQuery = "SELECT distinct AccClassID FROM Acc.tblTransactionClass where rowid='" + rowid + "' and VoucherType='" + vouchertype + "'";
            return Global.m_db.SelectQry(strQuery, "tbltransactionclass");    
        }

        public int GetRowID(int bulkpostingmaster)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT distinct rowid FROM Acc.tblBulkPostingDetails WHERE BulkPostingMasterID ='" + bulkpostingmaster + "'");
            return Convert.ToInt32(returnID);
        }

        public DataTable GetVoucherType(int id)
        {
            DataTable dt = new DataTable();
            string strQuery = "SELECT Distinct VoucherType FROM Acc.tblBulkPostingDetails where BulkPostingMasterID='" + id + "'";
            return Global.m_db.SelectQry(strQuery, "tblbulkdetailvouchertype");
        }

        public int GetCashID(int bulkpostingmasterid,string vouchernumber)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("select pm.LedgerID from Acc.tblCashPaymentMaster pm where CashPaymentID in (select RowID from Acc.tblBulkPostingDetails where   BulkPostingMasterID='" + bulkpostingmasterid + "' and VoucherType='" + vouchernumber + "')");
            return Convert.ToInt32(returnID);
        }

        public int GetBankID(int bulkpostingmasterid, string vouchernumber)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("select pm.LedgerID from Acc.tblBankPaymentMaster pm where BankPaymentID in (select RowID from Acc.tblBulkPostingDetails where   BulkPostingMasterID='" + bulkpostingmasterid + "' and VoucherType='" + vouchernumber + "')");
            return Convert.ToInt32(returnID);
        }

        public int GetBankReceiptID(int bulkpostingmasterid, string vouchernumber)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("select pm.LedgerID from Acc.tblBankReceiptMaster pm where BankReceiptID in (select RowID from Acc.tblBulkPostingDetails where   BulkPostingMasterID='" + bulkpostingmasterid + "' and VoucherType='" + vouchernumber + "')");
            return Convert.ToInt32(returnID);
        }

        public int GetCashReceiptID(int bulkpostingmasterid, string vouchernumber)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("select pm.LedgerID from Acc.tblCashReceiptMaster pm where CashReceiptID in (select RowID from Acc.tblBulkPostingDetails where   BulkPostingMasterID='" + bulkpostingmasterid + "' and VoucherType='" + vouchernumber + "')");
            return Convert.ToInt32(returnID);
        }


    }
}
