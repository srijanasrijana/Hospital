using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class RecurringVoucher
    {
        public static string CreateRecurringVoucherSetting(DataTable dtVoucherRecurring)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.AddParameter("@VoucherID", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["VoucherID"].ToString());
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["VoucherType"].ToString());
                Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["Description"].ToString());
                Global.m_db.AddParameter("@RecurringType", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["RecurringType"].ToString());
                Global.m_db.AddParameter("@Unit1", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["Unit1"].ToString());
                Global.m_db.AddParameter("@Unit2", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["Unit2"].ToString());
                Global.m_db.AddParameter("@Date", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["Date"].ToString());

                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spVoucherRecurringCreate");
                System.Data.SqlClient.SqlParameter paramReturn8 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                int res = Global.m_db.ProcessParameter();
                return paramReturn8.Value.ToString();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return "failure";
            }

        }
        public static DataTable GetRecurringVoucherSetting(string voucherID, string voucherType)
        {
            try
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblRecurringVoucher where VoucherID='" + voucherID + "' AND VoucherType= '" + voucherType + "' ", "System.tblRecurringVoucher");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }

        public static int DeleteRecurringVoucherSetting(string voucherID, string voucherType)
        {
             try
            {
              return Global.m_db.InsertUpdateQry("DELETE FROM System.tblRecurringVoucher where VoucherID='" + voucherID + "' AND VoucherType= '" + voucherType + "' ");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return -1;
            } 
        }
        public static string ModifyRecurringVoucherSetting(DataTable dtVoucherRecurring)
        {
             try
            {
                 DeleteRecurringVoucherSetting(dtVoucherRecurring.Rows[0]["VoucherID"].ToString(), dtVoucherRecurring.Rows[0]["VoucherType"].ToString());

                return CreateRecurringVoucherSetting(dtVoucherRecurring);
            //Global.m_db.AddParameter("@VoucherID", SqlDbType.NVarChar,dtVoucherRecurring.Rows[0]["VoucherID"].ToString());
            //Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["VoucherType"].ToString());
            //Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["Description"].ToString());
            //Global.m_db.AddParameter("@RecurringType", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["RecurringType"].ToString());
            //Global.m_db.AddParameter("@Unit1", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["Unit1"].ToString());
            //Global.m_db.AddParameter("@Unit2", SqlDbType.NVarChar, dtVoucherRecurring.Rows[0]["Unit2"].ToString());

            //Global.m_db.setCommandType(CommandType.StoredProcedure);
            //Global.m_db.setCommandText("System.spVoucherRecurringModify");
            //System.Data.SqlClient.SqlParameter paramReturn8 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

            //int res = Global.m_db.ProcessParameter();
            //return paramReturn8.Value.ToString();
        }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// creates new voucher posting
        /// </summary>
        /// <param name="dtRecurringVoucherPosting"></param>
        /// <returns></returns>
        public static string CreateRecurringVoucherPosting(DataTable dtRecurringVoucherPosting)
        {
             try
            {
            //Global.m_db.AddParameter("@VoucherNo", SqlDbType.NVarChar,dtRecurringVoucherPosting.Rows[0]["VoucherNo"].ToString());
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, dtRecurringVoucherPosting.Rows[0]["VoucherType"].ToString());
                Global.m_db.AddParameter("@Date", SqlDbType.NVarChar, dtRecurringVoucherPosting.Rows[0]["Date"].ToString());
                Global.m_db.AddParameter("@VoucherID", SqlDbType.NVarChar, dtRecurringVoucherPosting.Rows[0]["VoucherID"].ToString());
                Global.m_db.AddParameter("@isPosted", SqlDbType.NVarChar, dtRecurringVoucherPosting.Rows[0]["isPosted"].ToString());

                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spRecurringVoucherPostingCreate");
                System.Data.SqlClient.SqlParameter paramReturn8 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                int res = Global.m_db.ProcessParameter();
                return paramReturn8.Value.ToString();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }


        public static int DeleteRecurringVoucherPosting(string voucherID, string voucherType)
        {
             try
            {
                return Global.m_db.InsertUpdateQry("DELETE FROM System.tblRecurringVoucherPosting where VoucherID='" + voucherID + "' AND VoucherType= '" + voucherType + "' ");
         }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// Delete all posted voucherpostings from dates before today 
        /// </summary>
        /// <returns></returns>
        public static int DeleteRecurringVoucherPosting()
        {
             try
            {
             return Global.m_db.InsertUpdateQry("DELETE FROM System.tblRecurringVoucherPosting where isPosted=1 and Date< '" + System.DateTime.Today + "' ");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return -1;
            }
        }
        public static DataTable GetRecurringVoucherPosting(string voucherID, string voucherType)
        {
             try
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblRecurringVoucherPosting where VoucherID='" + voucherID + "' AND VoucherType= '" + voucherType + "'", "System.tblRecurringVoucherPosting");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }
        public static DataTable GetRecurringVoucherPosting(DateTime date)
        {
             try
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblRecurringVoucherPosting where Date = '" + DateManager.Date.ToDB(date) + "'", "System.tblRecurringVoucherPosting");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }

        public static DataTable GetRecurringVoucher()
        {
             try
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblRecurringVoucher", "System.tblRecurringVoucher");
             }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// selects all voucher settings created before today
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable GetRecurringVoucher(DateTime date)
        {
             try
            {
                return Global.m_db.SelectQry("SELECT * FROM System.tblRecurringVoucher where Date <'" + DateManager.Date.ToDB(date) + "'", "System.tblRecurringVoucher");

             }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// gets all vouchers which are not posted
        /// </summary>
        /// <returns></returns>
        public static DataTable GetNotPostedRecurring()
        {
             try
            {
                string str = "";

                 if(Global.Default_Date.ToString() == "English") // if defalut date is english then display voucher date in english format
                     str = "select distinct rvp.*,rv.RecurringType,rv.Description,rv.RVID, RVPID from System.tblRecurringVoucherPosting rvp , System.tblRecurringVoucher rv where rvp.VoucherID=rv.VoucherID and rvp.isPosted = 0 and rvp.VoucherType = rv.VoucherType";

                 else                                    // if defalut date is nepali then display voucher date in nepali format
                     str = "select distinct Date.fnEngtoNep(rvp.Date) as Date,rvp.VoucherType, RVPID rvp.VoucherID, isPosted,rv.RecurringType,rv.Description,rv.RVID from System.tblRecurringVoucherPosting rvp , System.tblRecurringVoucher rv where rvp.VoucherID=rv.VoucherID and rvp.isPosted = 0 and rvp.VoucherType = rv.VoucherType";

             return Global.m_db.SelectQry(str, "System.tblRecurringVoucherPosting");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }
        /// <summary>
        ///  gets all vouchers which are not posted in a particular date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable GetNotPostedRecurring(DateTime date)
        {
             try
            {
                string str = "";

                if (Global.Default_Date.ToString() == "English") // if defalut date is english then display voucher date in english format
                    str = "select distinct rvp.RVPID,rvp.Date,rvp.VoucherType, rvp.VoucherID, rvp.isPosted,  rv.RecurringType,rv.RVID,rv.Description from System.tblRecurringVoucherPosting rvp left join System.tblRecurringVoucher rv on rvp.VoucherID=rv.VoucherID where rvp.isPosted = 0 and rvp.VoucherType = rv.VoucherType and rvp.Date='" + DateManager.Date.ToDB(date) + "'";
                
                else                                    // if defalut date is nepali then display voucher date in nepali format
                    str = "select distinct rvp.RVPID,Date.fnEngtoNep(rvp.Date) as Date,rvp.VoucherType, rvp.VoucherID, rvp.isPosted,  rv.RecurringType,rv.RVID,rv.Description from System.tblRecurringVoucherPosting rvp left join System.tblRecurringVoucher rv on rvp.VoucherID=rv.VoucherID where rvp.isPosted = 0 and rvp.VoucherType = rv.VoucherType and rvp.Date='" + DateManager.Date.ToDB(date) + "'";

                    return Global.m_db.SelectQry(str, "System.tblRecurringVoucherPosting");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }
        public static DataTable GetRecurringSetting(int RVID)
        {
             try
            {
             return Global.m_db.SelectQry("select * from System.tblRecurringVoucher where RVID='" + RVID + "'", "System.tblRecurringVoucher");
             }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Sets voucher posting isPosted = true
        /// </summary>
        /// <param name="voucherID"></param>
        /// <param name="voucherType"></param>
        /// <returns></returns>
        public static string ModifyRecurringVoucherPosting(int RVPID)//int voucherID, String voucherType)
        {
            try
            {
                Global.m_db.ClearParameter();
                //Global.m_db.AddParameter("@VoucherID", SqlDbType.NVarChar, voucherID);
                //Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, voucherType);
                Global.m_db.AddParameter("@RVPID", SqlDbType.NVarChar, RVPID);
                Global.m_db.AddParameter("@isPosted", SqlDbType.NVarChar, "true");

                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spRecurringVoucherPostingModify");
                System.Data.SqlClient.SqlParameter paramReturn8 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                int res = Global.m_db.ProcessParameter();
                return paramReturn8.Value.ToString();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return null;
            }
        }
    }
}