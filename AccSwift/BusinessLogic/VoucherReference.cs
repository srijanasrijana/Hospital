using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace BusinessLogic
{
    public static class VoucherReference
    {
        public static int CreateReference(DataRow dr, int voucherID, string voucherType)//int ledgerID, int voucherID, string voucherType, string refName, bool isAgainst)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@ledgerID", SqlDbType.Int, dr["ledgerID"]);
                Global.m_db.AddParameter("@voucherID", SqlDbType.Int, voucherID);
                Global.m_db.AddParameter("@voucherType", SqlDbType.NVarChar, voucherType);
                Global.m_db.AddParameter("@isAgainst", SqlDbType.Bit, dr["isAgainst"]);
                Global.m_db.AddParameter("@ref", SqlDbType.NVarChar, dr["refName"]);
                SqlParameter objReturn = (SqlParameter)Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spReferenceCreate");

                Global.m_db.ProcessParameter();

                return (objReturn.Value.ToString() == "Failure" ? 0 : 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// displays details of reference related to a ledger  
        /// </summary>
        /// <param name="ledgerID"></param>
        /// <returns></returns>
        //public static DataTable GetReference(int voucherID, string voucherType, int ledgerID)  //(int ledgerID)
        //{
        //    try
        //    {
        //        string sql = "";
        //        //if(voucherID == 0)
        //        sql = "Select R.RefID, R.Ref as RefName, Sum( T.Debit_Amount) as DrAmt,Sum(T.Credit_Amount) as CrAmt from System.tblReference R inner join System.tblReferenceVoucher RV on R.RefID = RV.RefID inner join Acc.tblTransaction T on (RV.VoucherID = T.RowID and R.LedgerID = T.LedgerID)  where R.LedgerID = " + ledgerID + " and T.RowID not in ("+voucherID+") Group By R.RefID, R.Ref ";
        //        return Global.m_db.SelectQry(sql, "System.tblReferenceVoucher");       
        //       // string sql = "Select R.RefID,(Select TR.Ref from System.tblReference TR where TR.RefID = R.RefID) as RefName, Sum( T.Debit_Amount) as DrAmt,Sum(T.Credit_Amount) as CrAmt from System.tblReference R inner join System.tblReferenceVoucher RV on R.RefID = RV.RefID inner join Acc.tblTransaction T on RV.VoucherID = T.RowID and RV.VoucherType = T.VoucherType and R.LedgerID = T.LedgerID Where R.LedgerID = " + ledgerID + " Group By R.RefID";
        //       // return Global.m_db.SelectQry(sql, "System.tblReference");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        /// <summary>
        /// checks if an Ledger is reference ledger
        /// </summary>
        /// <param name="ledgerID"></param>
        /// <returns></returns>
        public static bool CheckIfReferece(int ledgerID)
        {
            try
            {
                string sql = "select COALESCE(IsBillReference, 'false') IsBillReference from Acc.tblLedger where LedgerID = " + ledgerID + "";
                bool res = Convert.ToBoolean(Convert.ToInt32(Global.m_db.GetScalarValue(sql)));
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int CreateReferenceVoucher(DataRow dr, int voucherID, string voucherType)//(int refID, int voucherID, string voucherType, bool isAgainst)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@refID", SqlDbType.Int, dr["refID"]);
                Global.m_db.AddParameter("@voucherID", SqlDbType.Int, voucherID);
                Global.m_db.AddParameter("@voucherType", SqlDbType.NVarChar, voucherType);
                Global.m_db.AddParameter("@isAgainst", SqlDbType.Bit, dr["isAgainst"]);
                string sql = "";
                sql = "insert into System.tblReferenceVoucher(RefID, VoucherID, VoucherType, IsAgainst) values(@refID, @voucherID, @voucherType, @isAgainst)";
                return Global.m_db.InsertUpdateQry(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static DataTable GetReference(int ledgerID, int voucherID, string voucherType)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@LedID", SqlDbType.Int, ledgerID);
                Global.m_db.AddParameter("@VoucherID", SqlDbType.Int, voucherID);
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, voucherType);

                Global.m_db.setCommandText("System.spGetReferenceAmt");
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@AccClassIDs", SqlDbType.NVarChar, 500);

                return Global.m_db.GetDataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static int ModifyReferenceVoucher(DataRow dr, int voucherID, string voucherType)//(int refID, int voucherID, string voucherType, bool isAgainst)
        {
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();             
                string sql = "";
                sql = "delete from System.tblReferenceVoucher where RefID = " + dr["refID"] + ", VoucherID = " + voucherID + ", VoucherType = " + voucherType + ", IsAgainst = 1";
                Global.m_db.InsertUpdateQry(sql);
                CreateReferenceVoucher(dr, voucherID, voucherType);
                Global.m_db.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
            }
        }
        /// <summary>
        /// when voucher consisting of reference is deleted, this method uses prcoedure that check dependencies and deletes the references
        /// </summary>
        /// <param name="vouID"></param>
        /// <param name="vouType"></param>
        /// <returns></returns>
        public static string DeleteReference(int vouID, string vouType)
        {
             try
            {
                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@VouID", SqlDbType.Int, vouID);
                Global.m_db.AddParameter("@VouType", SqlDbType.NVarChar, vouType);

                Global.m_db.setCommandText("System.spDeleteReference");
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@ReturnResult", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                return paramReturn.Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// to check if the reference name exists previously for the same ledger
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="ledgerID"></param>
        /// <returns></returns>
        public static bool IsReferenceExist(string reference, int ledgerID)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@ref", SqlDbType.NVarChar, reference);
                Global.m_db.AddParameter("@ledgerID", SqlDbType.Int, ledgerID);
                Global.m_db.setCommandType(CommandType.Text);
                Global.m_db.setCommandText("Select RefID from System.tblReference where Ref=@ref and LedgerID= @ledgerID");
                DataTable dt = Global.m_db.GetDataTable();
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                Global.MsgError(ex.Message);
                return false;
            }
        }
        public static DataTable GetAllRefAgainstForVoucher(int voucherID, string voucherType)
        {
            try
            {
               
                return Global.m_db.SelectQry("select b.RVID, a.LedgerID, b.VoucherType,  a.Ref RefName, a.RefID, b.IsAgainst from System.tblReference a inner join System.tblReferenceVoucher b on a.RefID = b.RefID where b.VoucherID = " + voucherID + " and b.VoucherType = '" + voucherType + "' ", "System.tblReference");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// deletes reference of a ledger against a given voucher
        /// </summary>
        /// <param name="voucherID"></param>
        /// <param name="voucherType"></param>
        /// <returns></returns>
        public static int DeleteRefAgainstForVoucher(int voucherID, string voucherType)
        {
            try
            {

               return Global.m_db.InsertUpdateQry("delete from System.tblReferenceVoucher where VoucherID = " + voucherID + " and VoucherType = '" + voucherType + "'");// and IsAgainst = 1");
                //return Global.m_db.InsertUpdateQry("delete from System.tblReference where RefID not in (select RefID from System.tblReferenceVoucher) ");// where VoucherType = " + voucherID + " and VoucherType = '" + voucherType + "'");// and IsAgainst = 1");

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// checks if new reference is created for a ledger in a given voucher and gets number of dependencies
        /// </summary>
        /// <param name="voucherID"></param>
        /// <param name="ledgerID"></param>
        /// <param name="voucherType"></param>
        /// <returns></returns>
        public static bool IsNewReferenceVoucher(int voucherID, int ledgerID, string voucherType)
        {
            try
            {
                bool res = false;
                string sql = "select IsAgainst, (select count(c.RefID) from System.tblReferenceVoucher c where c.RefID = b.RefID and c.IsAgainst = 1 ) refCount from System.tblReferenceVoucher a inner join System.tblReference b on a.RefID = b.RefID where a.VoucherID = " + voucherID + " and a.VoucherType = '" + voucherType + "' and b.LedgerID = " + ledgerID + " ";
                DataTable dtresult = Global.m_db.SelectQry(sql, "System.tblReference");
                if (dtresult.Rows.Count > 0)
                {
                    if (Convert.ToBoolean((dtresult.Rows[0]["IsAgainst"].ToString())) == false && Convert.ToInt32(dtresult.Rows[0]["refCount"].ToString()) > 0)
                    {
                        res = true;
                    }
                    //else
                    //    res = false;
                }
                //else
                //    res = false;
                //bool res = Convert.ToBoolean(Convert.ToInt32(result == null ? 0 : result));
                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetReferenceForVoucher(int voucherID, string voucherType)
        {
            try
            {
                return Global.m_db.SelectQry("select * from System.tblReferenceVoucher where VoucherID = " + voucherID + " and VoucherType = " + voucherType + "", "System.tblReferenceVoucher");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// this method is used when amount of voucher in which reference against is done is modified
        /// </summary>
        /// <param name="voucherID"></param>
        /// <param name="voucherType"></param>
        /// <param name="ledgerID"></param>
        /// <returns></returns>
        public static string GetAmtForAgainstRef(int voucherID, string voucherType, int ledgerID)
        {
            try
            {
                //DataTable dt;
                string AmtCrDr = "";
                //string sql = "Select R.RefID,(Select TR.Ref from System.tblReference TR where TR.RefID = R.RefID) as RefName,"+ 
                //             " Sum( T.Debit_Amount) as DrAmt,Sum(T.Credit_Amount) as CrAmt from System.tblReference R inner join System.tblReferenceVoucher RV on R.RefID = RV.RefID"+
                //             " inner join Acc.tblTransaction T on (RV.VoucherID = T.RowID and RV.VoucherType = T.VoucherType and R.LedgerID = T.LedgerID ) where T.RowID "+
                //             " not in( select RowID from Acc.tblTransaction where RowID = " + voucherID + " and VoucherType = '" + voucherType + "' and LedgerID = " + ledgerID + ") and R.LedgerID = " + ledgerID + " " +
                //             " and RV.RefID in (select b.RefID from System.tblReferenceVoucher a cross join System.tblReference b where a.RefID = b.RefID and a.VoucherID = " + voucherID + " and a.VoucherType = '" + voucherType + "' and b.LedgerID = " + ledgerID + ")" + 
                //             " Group By R.RefID ";

                Global.m_db.AddParameter("@LedID", SqlDbType.Int, ledgerID);
                Global.m_db.AddParameter("@VoucherID", SqlDbType.Int, voucherID);
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, voucherType);

                Global.m_db.setCommandText("System.spGetReferenceAmt");
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@AccClassIDs", SqlDbType.NVarChar, 500);

                DataTable dt = Global.m_db.GetDataTable();//= Global.m_db.SelectQry(sql, "System.tblReferenceVoucher");
                if (dt.Rows.Count > 0)
                {
                    Decimal amt = Convert.ToDecimal(dt.Rows[0]["CrAmt"]) - Convert.ToDecimal(dt.Rows[0]["DrAmt"]);
                    AmtCrDr = (Math.Abs(amt)).ToString();
                    // if
                    AmtCrDr += (amt < 0) ? "(Dr)" : "(Cr)";
                }
                else
                {
                    AmtCrDr = "0(Dr)";
                }
                return AmtCrDr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        //}

        }
        //public static DataTable GetRefAmtForEditing(int voucherID, string voucherType, int ledgerID)
        //{
        //    try
        //    {
        //        string sql = "Select R.RefID,(Select TR.Ref from System.tblReference TR where TR.RefID = R.RefID) as RefName, Sum( T.Debit_Amount) as DrAmt,Sum(T.Credit_Amount) as CrAmt from System.tblReference R inner join System.tblReferenceVoucher RV on R.RefID = RV.RefID inner join Acc.tblTransaction T on (RV.VoucherID = T.RowID and RV.VoucherType = T.VoucherType and R.LedgerID = T.LedgerID ) where T.RowID not in( select RowID from Acc.tblTransaction where RowID = "+voucherID+" and VoucherType = '"+voucherType+"' and LedgerID = "+ledgerID+") Group By R.RefID ";
        //        return Global.m_db.SelectQry(sql, "System.tblReferenceVoucher");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
