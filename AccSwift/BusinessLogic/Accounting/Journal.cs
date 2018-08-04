using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using DateManager;
using System.Xml.Serialization;

namespace BusinessLogic
{

    public interface IJournal
    {
        void Create(int SeriesID, string VoucherNo, DateTime JournalDate, string Remarks, DataTable JournalDetails, int[] AccClassID, int ProjectID, DataTable dtVoucherRecurring);
        void Modify(int JournalID, int SeriesID, string VoucherNo, DateTime JournalDate, string Remarks, DataTable JournalDetails, int[] AccClassID, int ProjectID, DataTable dtVoucherRecurring);

        bool RemoveJournalEntry(int JournalID);
        DataTable NavigateJournalMaster(int CurrentID, Navigate NavTo);
        DataTable GetJournalDetail(int MasterID);
    }

    public class Journal : IJournal
    {

        public void Create(int SeriesID, string VoucherNo, DateTime JournalDate, string Remarks, DataTable JournalDetails, int[] AccClassID, int ProjectID, DataTable dtVoucherRecurring)
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

            if (JournalDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
                return;
            }
            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();

            //This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in JournalDetails.Rows)
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

                //Check whether the same ledger name is posted in both debit and credit side.
                if (row["DrCr"].ToString() == "Debit")
                {
                    if (Credit.BinarySearch(row["Ledger"]) >= 0)
                        throw new Exception("Same Ledger cannot be posted as debit and credit");
                    Debit.Add(row["Ledger"]);
                }
                else if (row["DrCr"].ToString() == "Credit")
                {
                    if (Debit.BinarySearch(row["Ledger"]) >= 0)
                        throw new Exception("Same Ledger cannot be posted as debit and credit");
                    Credit.Add(row["Ledger"]);
                }



            }//end foreach

            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spJournalCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@Journal_Date", SqlDbType.DateTime, JournalDate);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnID = Convert.ToInt32(objReturn.Value);

                for (int i = 0; i < JournalDetails.Rows.Count; i++)
                {
                    DataRow dr = JournalDetails.Rows[i];

                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spJournalDetailCreate");
                    Global.m_db.AddParameter("@JournalID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    //Global.m_db.AddParameter("@LedgerFolioNo", SqlDbType.NVarChar, 50, dr["LedgerFolioNo"].ToString());
                    //Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, dr["LedgerCode"].ToString());
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString());
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        throw new Exception("Unable to create Journal");
                    }

                    //Also insert the transaction in tbltransaction
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, JournalDate);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString().ToUpper());
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "JNL");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);

                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();

                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);


                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Journal");
                    }

                    ////Add the information  in Acc.tblTransactionClass
                    foreach (int _AccClassID in AccClassID)
                    {
                        //Global.m_db.InsertUpdateQry("INSERT INTO dbo.tblUseDtl(PesticideRegID,UseID) VALUES('" + objReturn.Value.ToString() + "','" + _AccClassID + "')");
                        //DataRow drAccClassID = AccClassID.Rows[i1];
                        //Also insert the transaction in tbltransacClass
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "JNL");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Journal");
                        }
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

        public void Modify(int JournalID, int SeriesID, string VoucherNo, DateTime JournalDate, string Remarks, DataTable JournalDetails, int[] AccClassID, int ProjectID, DataTable dtVoucherRecurring)
        {
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spJournalModify");
                Global.m_db.AddParameter("@JournalID", SqlDbType.Int, JournalID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.NVarChar, 50, SeriesID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@Journal_Date", SqlDbType.DateTime, JournalDate);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 50, ProjectID);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                //First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblJournalDetail WHERE JournalID='" + JournalID.ToString() + "'");

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
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='JRNL' AND RowID='" + JournalID.ToString() + "'");


                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID

                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='JRNL' AND RowID='" + JournalID.ToString() + "'");

                for (int i = 0; i < JournalDetails.Rows.Count; i++)
                {
                    DataRow dr = JournalDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spJournalDetailCreate");
                    Global.m_db.AddParameter("@JournalID", SqlDbType.Int, JournalID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    //Global.m_db.AddParameter("@LedgerFolioNo", SqlDbType.NVarChar, 50, dr["LedgerFolioNo"].ToString());
                    //Global.m_db.AddParameter("@LedgerCode", SqlDbType.NVarChar, 50, dr["LedgerCode"].ToString());                    
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString());
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Journal");
                    }

                    //Also insert the transaction in tbltransaction
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, JournalDate);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 300, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString().ToUpper());
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "JNL");
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 50, ProjectID);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, JournalID);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);

                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Journal");
                    }

                    ////Add the information  in Acc.tblTransactionClass
                    foreach (int _AccClassID in AccClassID)
                    {
                        //Global.m_db.InsertUpdateQry("INSERT INTO dbo.tblUseDtl(PesticideRegID,UseID) VALUES('" + objReturn.Value.ToString() + "','" + _AccClassID + "')");
                        //DataRow drAccClassID = AccClassID.Rows[i1];
                        //Also insert the transaction in tbltransacClass

                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, JournalID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "JNL");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Journal");
                        }
                    }
                }

                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "SALES_INVOICE";
                    string result = RecurringVoucher.ModifyRecurringVoucherSetting(dtVoucherRecurring);

                    if (result == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Sales Invoice due to recurring settings.");
                    }
                }
                #endregion

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

        public DataTable GetJournalMasterInfo(string RowID)
        {
            string DrCr = "Debit";
            return Global.m_db.SelectQry("select top 1 jm.*,jd.LedgerID from Acc.tblJournalMaster jm,Acc.tblJournalDetail jd where jm.JournalID='" + RowID + "' and jm.JournalID=jd.JournalID and jd.DrCr='" + DrCr + "' order by amount desc", "table");
            //return Global.m_db.SelectQry("select top 1 jm.*,jd.LedgerID from Acc.tblJournalMaster jm,Acc.tblJournalDetail jd where jm.JournalID='" + RowID + "' and jm.JournalID=jd.JournalID  order by amount desc", "table");
            //return Global.m_db.SelectQry("SELECT * FROM Acc.tblJournalMaster WHERE JournalID ='" + RowID + "'", "table");
        }


        /// <summary>
        /// Gives the Journal Information
        /// </summary>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public static DataTable GetJournalInfo(int JournalID = 0)
        {
            if (JournalID > 0) //Show particular records
                return Global.m_db.SelectQry("SELECT * FROM Acc.tblJournalMaster WHERE JournalID=" + JournalID.ToString() + " ORDER BY Journal_Date ASC, JournalID ASC", "tblJournal");
            else
                return Global.m_db.SelectQry("SELECT * FROM Acc.tblJournalMaster ORDER BY Journal_Date ASC, JournalID ASC", "tblJournal");

        }


        /// <summary>
        /// Simply Deletes the Journal from given Journal ID
        /// </summary>
        /// <param name="JournalID"></param>
        public bool RemoveJournalEntry(int JournalID)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spRemoveJournalEntry");
                Global.m_db.AddParameter("@JournalID", SqlDbType.Int, JournalID);
              //  system.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                return true;
               
            }
            catch
            {
                return false;
            }



         
        }

        /// <summary>
        /// Outputs the previous Journal Voucher Information. It sends blank if no previous record is available
        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public DataTable NavigateJournalMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spJournalNavigate");
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
            DataTable dtJournalMst = Global.m_db.GetDataTable();
            return dtJournalMst;
        }

        /// <summary>
        /// Outputs the Journal Detail Information. 
        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public DataTable GetJournalDetail(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spGetJournalDetail");
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
            DataTable dtJournalDtl = Global.m_db.GetDataTable();
            return dtJournalDtl;
        }

        public DataTable GetJournalMasterDtl(string RowID)
        {
            DataTable dt = Global.m_db.SelectQry("SELECT * FROM Acc.tblJournalMaster WHERE JournalID='" + RowID + "'", "VoucherNoInfo");
            return dt;
        }

        public int GetSeriesIDFromMasterID(int MasterID)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Acc.tblJournalMaster WHERE JournalID ='" + MasterID + "'");
            return Convert.ToInt32(returnID);
        }
        public int GetRowID(string vouchernumber)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT JournalID FROM Acc.tblJournalMaster WHERE Voucher_No ='" + vouchernumber + "'");
            return Convert.ToInt32(returnID);
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
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "JOURNAL";
                    res = RecurringVoucher.CreateRecurringVoucherSetting(dtVoucherRecurring);
                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Journal Recurring settings.");
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
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "JOURNAL";
                    res = RecurringVoucher.ModifyRecurringVoucherSetting(dtVoucherRecurring);

                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Journal recurring settings.");
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
                    VoucherReference.CreateReference(dr, voucherID, "JRNL");
                }

                foreach (DataRow dr in dtReference.Select("[RefID] is not null"))
                {
                    VoucherReference.CreateReferenceVoucher(dr, voucherID, "JRNL");
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
                //Global.m_db.BeginTransaction();
                //VoucherReference.DeleteRefAgainstForVoucher(voucherID, "SALES");
                if (RowsToDelete.Length > 0)
                    Global.m_db.InsertUpdateQry("delete from System.tblReferenceVoucher where RVID in(" + RowsToDelete.Substring(0, RowsToDelete.Length - 1) + ")");

                if (dtReference.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtReference.Select("[RefID] is null and [RVID] is null"))
                    {
                        VoucherReference.CreateReference(dr, voucherID, "JRNL");
                    }

                    foreach (DataRow dr in dtReference.Select("[RefID] is not null and [RVID] is null"))
                    {
                        VoucherReference.CreateReferenceVoucher(dr, voucherID, "JRNL");
                    }
                    
                }
                Global.m_db.InsertUpdateQry("delete from System.tblReference where RefID not in (select RefID from System.tblReferenceVoucher) ");// where VoucherType = " + voucherID + " and VoucherType = '" + voucherType + "'");// and IsAgainst = 1");

                // Global.m_db.CommitTransaction();
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
        /// <summary>
        /// JournalDetail.cs
        /// Summary description for OrderDetail.
        /// </summary>
        public class JournalDetail
        {
            public JournalDetail()
            {
            }
            private int Journal_DetailID;
            private int JournalID;
            private int LedgerID;
            private Decimal Amount;
            private string DrCr;
            private string Remarks;

            //[XmlAttribute]
            //public int Journal_DetailID
            //{
            //    get
            //    {
            //        return this.PID;
            //    }
            //    set
            //    {
            //        PID = value;
            //    }
            //}

            //[XmlAttribute]
            //public int JournalID
            //{
            //    get
            //    {
            //        return this.PID;
            //    }
            //    set
            //    {
            //        PID = value;
            //    }
            //}

            //[XmlAttribute]
            //public int LedgerID
            //{
            //    get
            //    {
            //        return this.PID;
            //    }
            //    set
            //    {
            //        PID = value;
            //    }
            //}

            //[XmlAttribute]
            //public Decimal Amount
            //{
            //    get
            //    {
            //        return this.UPrice;
            //    }
            //    set
            //    {
            //        UPrice = value;
            //    }
            //}

            //[XmlAttribute]
            //public string DrCr
            //{
            //    get
            //    {
            //        return this.Qty;
            //    }
            //    set
            //    {
            //        Qty = value;
            //    }
            //}

            //[XmlAttribute]
            //public string Remarks
            //{
            //    get
            //    {
            //        return this.disc;
            //    }
            //    set
            //    {
            //        disc = value;
            //    }
            //}


        }

        public static int InsertJournal(string JournalXMLString)
        {

            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();

                Global.m_db.setCommandText("Acc.xmlJournalInsert");
                Global.m_db.setCommandType(CommandType.StoredProcedure);

                Global.m_db.AddParameter("@journal", SqlDbType.Xml, JournalXMLString);
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

        public static string EditJournal(string JournalXMLString)
        {

            try
            {
                string res = "";
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();

                Global.m_db.setCommandText("Acc.xmlJournalUpdate");
                Global.m_db.setCommandType(CommandType.StoredProcedure);

                //   Global.m_db.AddParameter("@returnId", SqlDbType.Int, returnID);
                Global.m_db.AddParameter("@journal", SqlDbType.Xml, JournalXMLString);

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
