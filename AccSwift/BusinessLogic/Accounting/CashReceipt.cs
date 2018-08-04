using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DBLogic;
using DateManager;

namespace BusinessLogic
{
    public class CashReceipt
    {
        public void Create(int SeriesID, int LedgerID, string VoucherNo, DateTime CashReceipt_Date, string Remarks, DataTable CashReceiptDetails, int[] AccClassID, int ProjectID)
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
            if (CashReceiptDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
                return;
            }
            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();
            //This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in CashReceiptDetails.Rows)
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
                Global.m_db.setCommandText("Acc.spCashReceiptCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@CashReceipt_Date", SqlDbType.DateTime, CashReceipt_Date);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnID = Convert.ToInt32(objReturn.Value);
                for (int i = 0; i < CashReceiptDetails.Rows.Count; i++)
                {
                    DataRow dr = CashReceiptDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spCashReceiptDetailCreate");
                    Global.m_db.AddParameter("@CashReceiptID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@LedgerFolioNo", SqlDbType.NVarChar, 50, dr["LedgerFolioNo"].ToString());
                    //Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, dr["LedgerCode"].ToString());                                                          
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Amount += Convert.ToDouble(dr["Amount"]);
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Cash Receipt");
                    }
                    //Also insert the transaction in tbltransaction
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, CashReceipt_Date);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CASH_RCPT");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);

                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Cash Receipt");
                    }

                    ////Add the information  in Acc.tblTransactionClass
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CASH_RCPT");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Cash Receipt");
                        }
                    }
                }
                //Also insert the transaction in tbltransaction
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, CashReceipt_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Amount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CASH_RCPT");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);

                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Cash Receipt");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CASH_RCPT");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Cash Receipt");
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

        public void Modify(int CashReceiptID, int SeriesID, int LedgerID, string VoucherNo, DateTime CashReceipt_Date, string Remarks, DataTable CashReceiptDetails, int[] AccClassID, int ProjectID)
        {
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spCashReceiptModify");
                Global.m_db.AddParameter("@CashReceiptID", SqlDbType.Int, CashReceiptID);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@CashReceipt_Date", SqlDbType.DateTime, CashReceipt_Date);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                //int ReturnID = Convert.ToInt32(objReturn.Value);
                //First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblCashReceiptDetails WHERE CashReceiptID='" + CashReceiptID.ToString() + "'");
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
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='CASH_RCPT' AND RowID='" + CashReceiptID.ToString() + "'");

                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='CASH_RCPT' AND RowID='" + CashReceiptID.ToString() + "'");

                double Amount = 0;
                for (int i = 0; i < CashReceiptDetails.Rows.Count; i++)
                {
                    DataRow dr = CashReceiptDetails.Rows[i];

                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spCashReceiptDetailCreate");
                    Global.m_db.AddParameter("@CashReceiptID", SqlDbType.Int, CashReceiptID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@LedgerFolioNo", SqlDbType.NVarChar, 50, dr["LedgerFolioNo"].ToString());
                    //Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, dr["LedgerCode"].ToString());                                                          
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Amount += Convert.ToDouble(dr["Amount"]);
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Cash Receipt");
                    }
                    //Also insert the transaction in tbltransaction
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, CashReceipt_Date);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CASH_RCPT");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, CashReceiptID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);

                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Cash Receipt");
                    }

                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, CashReceiptID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CASH_RCPT");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Cash Receipt");
                        }
                    }
                }
                ////Also insert the transaction in tbltransaction
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, CashReceipt_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Amount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CASH_RCPT");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, CashReceiptID.ToString());
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);

                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Cash Receipt");
                }
                //Add the information  in Acc.tblTransactionClass
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, CashReceiptID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CASH_RCPT");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Cash Receipt");
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
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "CASH_RECEIPT";
                    res = RecurringVoucher.CreateRecurringVoucherSetting(dtVoucherRecurring);
                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Cash Receipt Recurring settings.");
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
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "CASH_RECEIPT";
                    res = RecurringVoucher.ModifyRecurringVoucherSetting(dtVoucherRecurring);

                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Cash Receipt recurring settings.");
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

        public static string InsertReference(int voucherID, DataTable dtReference)
        {
            try
            {
                #region Save voucher reference settings
                string res = "";
                Global.m_db.BeginTransaction();
                foreach (DataRow dr in dtReference.Select("[RefID] is null"))
                {
                    VoucherReference.CreateReference(dr, voucherID, "CASH_RCPT");
                }

                foreach (DataRow dr in dtReference.Select("[RefID] is not null"))
                {
                    VoucherReference.CreateReferenceVoucher(dr, voucherID, "CASH_RCPT");
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
                        VoucherReference.CreateReference(dr, voucherID, "CASH_RCPT");
                    }

                    foreach (DataRow dr in dtReference.Select("[RefID] is not null and [RVID] is null"))
                    {
                        VoucherReference.CreateReferenceVoucher(dr, voucherID, "CASH_RCPT");
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
        public bool RemoveCashReceiptEntry(int CashReceiptID)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spRemoveCashReceiptEntry");
                Global.m_db.AddParameter("@CashReceiptID", SqlDbType.Int, CashReceiptID);
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
        /// Outputs the previous Cash Receipt Voucher Information. It sends blank if no previous record is available
        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public DataTable NavigateCashReceiptMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spCashReceiptNavigate");
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
            DataTable dtCashReceiptMst = Global.m_db.GetDataTable();
            return dtCashReceiptMst;
        }

        /// <summary>
        /// Outputs the Journal Detail Information. 
        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public DataTable GetCashReceiptDetail(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spGetCashReceiptDetail");
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
            DataTable dtCashReceiptDtl = Global.m_db.GetDataTable();
            return dtCashReceiptDtl;
        }

        public DataTable GetCashReceiptMaster(int CashReceiptID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblCashReceiptMaster WHERE CashReceiptID ='" + CashReceiptID + "'", "CashReceiptMaster");
        }

        public int GetSeriesIDFromMasterID(int MasterID)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Acc.tblCashReceiptMaster WHERE CashReceiptID ='" + MasterID + "'");
            return Convert.ToInt32(returnID);
        }

        /// <summary>
        /// Simply Deletes the CashReceipt from given Journal ID
        /// </summary>
        /// <param name="CashReceiptID"></param>
        public bool Delete(int CashReceiptID)
        {
            //If he's sure he has intentionally pressed delete button,
            try
            {

                //Flush data from Transaction table
                Global.m_db.InsertUpdateQry("DELETE Acc.tblTransaction WHERE RowID='" + CashReceiptID.ToString() + "' AND VoucherType='CASH_RCPT'");

                //Delete data from CashReceiptMaster Master, CashReceiptMaster Details gets automatically flushed away
                Global.m_db.InsertUpdateQry("DELETE Acc.tblCashReceiptMaster WHERE CashReceiptID='" + CashReceiptID.ToString() + "'");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }



        public int GetRowID(string vouchernumber)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT CashReceiptID FROM Acc.tblCashReceiptMaster WHERE Voucher_No ='" + vouchernumber + "'");
            return Convert.ToInt32(returnID);
        }


        public static int InsertCashReceipt(string CashReceiptXMLString)
        {

            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();

                Global.m_db.setCommandText("Acc.xmlCashReceiptInsert");
                Global.m_db.setCommandType(CommandType.StoredProcedure);

                Global.m_db.AddParameter("@cashreceipt", SqlDbType.Xml, CashReceiptXMLString);
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

        public static string EditCashReceipt(string CashReceiptXMLString)
        {

            try
            {
                string res = "";
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();

                Global.m_db.setCommandText("Acc.xmlCashReceiptUpdate");
                Global.m_db.setCommandType(CommandType.StoredProcedure);


                Global.m_db.AddParameter("@cashreceipt", SqlDbType.Xml, CashReceiptXMLString);

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
