using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DateManager;

namespace BusinessLogic
{
    public class BankPayment
    {
        public void Create(int SeriesID, int LedgerID, string VoucherNo, DateTime BankPayment_Date, string Remarks, DataTable BankPaymentDetails, int[] AccClassID, int ProjectID)
        {
            #region Language Mgmt
            string Language = "";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    Language = "ENGLISH";
                    break;
                case Lang.Nepali:
                    Language = "NEPALI";
                    break;
            }

            #endregion

            //Check if the JournalDetails has fields

            if (BankPaymentDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
                return;
            }

            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();
            ////This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in BankPaymentDetails.Rows)
            {
                //Check whether the ledger name are correct
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetLedgerIDFromName");
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, row["Ledger"].ToString());
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, Language);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                if (objReturn.Value.ToString() == "-100")//It return -100 in case of failure
                    throw new Exception("Ledger Name - " + row["Ledger"].ToString() + " not found!");
            }
            double Amount = 0;

            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spBankPaymentCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.NVarChar, 50, SeriesID);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@BankPayment_Date", SqlDbType.DateTime, BankPayment_Date);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.NVarChar, 200, ProjectID);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnID = Convert.ToInt32(objReturn.Value);

                for (int i = 0; i < BankPaymentDetails.Rows.Count; i++)
                {
                    DataRow dr = BankPaymentDetails.Rows[i];

                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spBankPaymentDetailCreate");
                    Global.m_db.AddParameter("@BankPaymentID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being                  
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Amount += Convert.ToDouble(dr["Amount"]);
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    Global.m_db.AddParameter("@ChequeNumber", SqlDbType.NVarChar, 50, dr["ChequeNumber"].ToString());
                    if (dr["ChequeDate"] == "")
                    {
                        Global.m_db.AddParameter("@ChequeDate", SqlDbType.Date, null);
                    }
                    else
                    {
                        Global.m_db.AddParameter("@ChequeDate", SqlDbType.Date, Date.ToDB(Convert.ToDateTime(dr["ChequeDate"])));
                    }
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Bank Payement");
                    }
                    //Also insert the transaction in tbltransaction for details
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, BankPayment_Date);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "BANK_PMNT");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);

                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Bank Payment");
                    }

                    //Add the information  in Acc.tblTransactionClas
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "BANK_PMNT");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Bank Payment");
                        }
                    }
                }

                //Also insert the transaction in tbltransaction for master
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, BankPayment_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Amount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "BANK_PMNT");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);

                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Bank Payment");
                }

                //Add the information  in Acc.tblTransactionClass     
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "BANK_PMNT");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Bank Payment");
                    }
                }

                Global.m_db.CommitTransaction();

            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();

                throw ex;

                #region SQLException
                //switch (ex.Number)
                //{
                //    case 4060: // Invalid Database 
                //        Global.Msg("Invalid Database", MBType.Error, "Error");
                //        break;

                //    case 18456: // Login Failed 
                //        Global.Msg("Login Failed!", MBType.Error, "Error");
                //        break;

                //    case 547: // ForeignKey Violation , Check Constraint
                //        Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                //        break;

                //    case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                //        Global.Msg("The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                //        break;

                //    case 2601: // Unique Index/Constriant Violation 
                //        Global.Msg("Unique index violation!", MBType.Warning, "Error");
                //        break;

                //    case 5000: //Trigger violation
                //        Global.Msg("Trigger violation!", MBType.Warning, "Error");
                //        break;

                //    default:
                //        break;
                ////}
                #endregion
            }
        }

        public void Modify(int BankPaymentID, int SeriesID, int LedgerID, string VoucherNo, DateTime BankPayment_Date, string Remarks, DataTable BankPaymentDetails, int[] AccClassID, int ProjectID)
        {
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spBankPaymentModify");
                Global.m_db.AddParameter("@BankPaymentID", SqlDbType.Int, BankPaymentID);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.NVarChar, 50, SeriesID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@BankPayment_Date", SqlDbType.DateTime, BankPayment_Date);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                //int ReturnID = Convert.ToInt32(objReturn.Value);
                //First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblBankPaymentDetails WHERE BankPaymentID='" + BankPaymentID.ToString() + "'");
                //Now insert the new ones
                string Language = "";
                switch (LangMgr.DefaultLanguage)
                {
                    case Lang.English:
                        Language = "ENGLISH";
                        break;
                    case Lang.Nepali:
                        Language = "NEPALI";
                        break;
                }

                //First delete the old transaction
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='BANK_PMNT' AND RowID='" + BankPaymentID.ToString() + "'");

                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='BANK_PMNT' AND RowID='" + BankPaymentID.ToString() + "'");

                double Amount = 0;
                for (int i = 0; i < BankPaymentDetails.Rows.Count; i++)
                {
                    DataRow dr = BankPaymentDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spBankPaymentDetailCreate");
                    Global.m_db.AddParameter("@BankPaymentID", SqlDbType.Int, BankPaymentID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being                  
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Amount += Convert.ToDouble(dr["Amount"]);
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    Global.m_db.AddParameter("@ChequeNumber", SqlDbType.NVarChar, 50, dr["ChequeNumber"].ToString());
                    if (dr["ChequeDate"] == "")
                    {
                        Global.m_db.AddParameter("@ChequeDate", SqlDbType.Date, null);
                    }
                    else
                    {
                        Global.m_db.AddParameter("@ChequeDate", SqlDbType.Date, Date.ToDB(Convert.ToDateTime(dr["ChequeDate"])));
                    }
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Bank Payment");
                    }
                    //Also insert the transaction in tbltransaction
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, BankPayment_Date);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "BANK_PMNT");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, BankPaymentID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);

                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Bank Payment");
                    }

                    //Now add the New editable records for Acc.tblTransactionClass            
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, BankPaymentID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "BANK_PMNT");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Bank Payment");
                        }
                    }
                }
                ////Also insert the transaction in tbltransaction
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, BankPayment_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Amount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "BANK_PMNT");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, BankPaymentID.ToString());
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);

                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Bank Payment");
                }
                //Add the information  in Acc.tblTransactionClas
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, BankPaymentID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "BANK_PMNT");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Bank Payment");
                    }
                }

                Global.m_db.CommitTransaction();
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();

                throw ex;

                #region SQLException
                //switch (ex.Number)
                //{
                //    case 4060: // Invalid Database 
                //        Global.Msg("Invalid Database", MBType.Error, "Error");
                //        break;

                //    case 18456: // Login Failed 
                //        Global.Msg("Login Failed!", MBType.Error, "Error");
                //        break;

                //    case 547: // ForeignKey Violation , Check Constraint
                //        Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                //        break;

                //    case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                //        Global.Msg("The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                //        break;

                //    case 2601: // Unique Index/Constriant Violation 
                //        Global.Msg("Unique index violation!", MBType.Warning, "Error");
                //        break;

                //    case 5000: //Trigger violation
                //        Global.Msg("Trigger violation!", MBType.Warning, "Error");
                //        break;

                //    default:
                //        break;
                //}
                #endregion
            }
        }
        public static string InsertVoucherRecurring(DataTable dtVoucherRecurring, int voucherID)
        {
            try
            {
                #region Save voucher recurring settings
                string res = "";
                Global.m_db.BeginTransaction();

                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherID"] = voucherID;
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "BANK_PAYMENT";
                    res = RecurringVoucher.CreateRecurringVoucherSetting(dtVoucherRecurring);
                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Bank Payment Recurring settings.");
                    }
                }
                #endregion

                Global.m_db.CommitTransaction();
                return res;
            }
            catch (Exception ex)
            {

                Global.MsgError(ex.Message);
                Global.m_db.RollBackTransaction();
                return "0";

            }
        }
        public static string ModifyVoucherRecurring(DataTable dtVoucherRecurring, int voucherID)
        {
            try
            {
                #region Save voucher recurring settings
                string res = "";
                Global.m_db.BeginTransaction();

                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherID"] = voucherID;
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "BANK_PAYMENT";
                    res = RecurringVoucher.ModifyRecurringVoucherSetting(dtVoucherRecurring);

                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Bank Payment recurring settings.");
                    }
                }
                #endregion

                Global.m_db.CommitTransaction();
                return res;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                Global.m_db.RollBackTransaction();
                return "0";

            }
        }
        public bool RemoveBankPaymentEntry(int BankPaymentID)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spRemoveBankPaymentEntry");
                Global.m_db.AddParameter("@BankPaymentID", SqlDbType.Int, BankPaymentID);
                //system.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Simply Deletes the Journal from given Journal ID
        /// </summary>
        /// <param name="BankPaymentID"></param>
        public bool Delete(int BankPaymentID)
        {
            //If he's sure he has intentionally pressed delete button,

            try
            {

                //Flush data from Transaction table
                Global.m_db.InsertUpdateQry("DELETE Acc.tblTransaction WHERE RowID='" + BankPaymentID.ToString() + "' AND VoucherType='BANK_PMNT'");

                //Delete data from BankPaymentMaster Master, BankPaymentMaster Details gets automatically flushed away

                Global.m_db.InsertUpdateQry("DELETE Acc.tblBankPaymentMaster WHERE BankPaymentID='" + BankPaymentID.ToString() + "'");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }



        /// <summary>
        /// Outputs the previous Journal Voucher Information. It sends blank if no previous record is available
        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public DataTable NavigateBankPaymentMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spBankPaymentNavigate");
            Global.m_db.AddParameter("@CurrentID", SqlDbType.Int, CurrentID);
            string strNavigate = "FIRST";
            switch (NavTo)
            {
                case Navigate.First:
                    strNavigate = "FIRST";
                    break;
                case Navigate.Last:
                    strNavigate = "LAST";
                    break;
                case Navigate.Next:
                    strNavigate = "NEXT";
                    break;
                case Navigate.Prev:
                    strNavigate = "PREV";
                    break;
                case Navigate.ID:
                    strNavigate = "ID";
                    break;
            }
            Global.m_db.AddParameter("@NavigateTo", SqlDbType.NVarChar, 20, strNavigate);
            DataTable dtBankPaymentMst = Global.m_db.GetDataTable();
            return dtBankPaymentMst;
        }

        public DataTable GetBankPaymentDetail(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spGetBankPaymentDetail");
            Global.m_db.AddParameter("@MasterID", SqlDbType.Int, MasterID);
            string Language = "ENGLISH";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    Language = "ENGLISH";
                    break;
                case Lang.Nepali:
                    Language = "NEPALI";
                    break;
            }
            Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, Language);
            DataTable dtBankReceiptDtl = Global.m_db.GetDataTable();
            return dtBankReceiptDtl;
        }
        public static string InsertReference(int voucherID, DataTable dtReference)
        {
            try
            {
                #region Save voucher reference settings
                string res = "";
                Global.m_db.BeginTransaction();
                foreach (DataRow dr in dtReference.Select("[RefID] is null"))
                {
                    VoucherReference.CreateReference(dr, voucherID, "BANK_PMT");
                }

                foreach (DataRow dr in dtReference.Select("[RefID] is not null"))
                {
                    VoucherReference.CreateReferenceVoucher(dr, voucherID, "BANK_PMT");
                }

                Global.m_db.CommitTransaction();
                return res;
                #endregion
            }

            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                Global.m_db.RollBackTransaction();
                return "0";

            }
        }

        public static string ModifyReference(int voucherID, DataTable dtReference, string RowsToDelete)
        {
            try
            {
                #region Save modified voucher reference settings
                string res = "";
                Global.m_db.BeginTransaction();
                //VoucherReference.DeleteRefAgainstForVoucher(voucherID, "SALES");
                if (RowsToDelete.Length > 0)
                    Global.m_db.InsertUpdateQry("delete from System.tblReferenceVoucher where RVID in(" + RowsToDelete.Substring(0, RowsToDelete.Length - 1) + ")");

                if (dtReference.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtReference.Select("[RefID] is null and [RVID] is null"))
                    {
                        VoucherReference.CreateReference(dr, voucherID, "BANK_PMT");
                    }

                    foreach (DataRow dr in dtReference.Select("[RefID] is not null and [RVID] is null"))
                    {
                        VoucherReference.CreateReferenceVoucher(dr, voucherID, "BANK_PMT");
                    } 
                }

                Global.m_db.CommitTransaction();
                return res;
                #endregion
            }

            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                Global.m_db.RollBackTransaction();
                return "0";
            }
        }
        public DataTable GetBankPaymentMaster(int BankPaymentID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblBankPaymentMaster WHERE BankPaymentID ='" + BankPaymentID + "'", "BankPaymentMaster");
        }

        public int GetSeriesIDFromMasterID(int MasterID)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Acc.tblBankPaymentMaster WHERE BankPaymentID ='" + MasterID + "'");
            return Convert.ToInt32(returnID);
        }

        public int GetRowID(string vouchernumber)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT BankPaymentID FROM Acc.tblBankPaymentMaster WHERE Voucher_No ='" + vouchernumber + "'");
            return Convert.ToInt32(returnID);
        }


        public static int InsertBankPayment(string BankPaymentXMLString)
        {

            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();

                Global.m_db.setCommandText("Acc.xmlBankPaymentInsert");
                Global.m_db.setCommandType(CommandType.StoredProcedure);

                Global.m_db.AddParameter("@bankpayment", SqlDbType.Xml, BankPaymentXMLString);
                // Global.m_db.AddParameter("@returnId", SqlDbType.Int, returnID);

                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@returnId", SqlDbType.Int, 20);

                Global.m_db.ProcessParameter();

                int ReturnID = Convert.ToInt32(objReturn.Value);
                if (ReturnID <= 0)
                {
                    throw new Exception("problem occured while inserting data into master table");
                }


                Global.m_db.CommitTransaction();
                return ReturnID;
            }

            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
            }

        }

        public static string EditBankPayment(string BankPaymentXMLString)
        {

            try
            {
                string res = "";
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();

                Global.m_db.setCommandText("Acc.xmlBankPaymentUpdate");
                Global.m_db.setCommandType(CommandType.StoredProcedure);


                Global.m_db.AddParameter("@bankpayment", SqlDbType.Xml, BankPaymentXMLString);

                Global.m_db.ProcessParameter();

                Global.m_db.CommitTransaction();
                return res;
            }

            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
            }

        }

    }
}
