using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections;
using DateManager;

namespace BusinessLogic
{
    public interface ICreditNote
    {
        void Create(int SeriesID,int LedgerID, string VoucherNo, DateTime CreditNoteDate, string Remarks, DataTable CreditNoteDetails,int[] AccClassID,int ProjectID,OptionalField of);
        void Modify(int CreditNoteID, int SeriesID,int LedgerID, string VoucherNo, DateTime CreditNoteDate, string Remarks, DataTable CreditNoteDetails, int[] AccClassID,int ProjectID,OptionalField of);
        ////void Delete(string AccountHeadName);
        DataTable NavigateCreditNoteMaster(int CurrentID, Navigate NavTo);
        DataTable GetCreditNoteDetail(int MasterID);
    }

    public class CreditNote : ICreditNote
    {
        public void Create(int SeriesID,int LedgerID, string VoucherNo, DateTime CreditNoteDate, string Remarks, DataTable CreditNoteDetails,int[] AccClassID,int ProjectID,OptionalField of)
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
            if (CreditNoteDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
                return;
            }
            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();
            //This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in CreditNoteDetails.Rows)
            {
                //Check whether the ledger name are correct
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetLedgerIDFromName");
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 50, row["Ledger"].ToString());
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
                Global.m_db.setCommandText("Acc.spCreditNoteCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@CreditNote_Date", SqlDbType.DateTime,CreditNoteDate);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID",SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnID = Convert.ToInt32(objReturn.Value);
                for (int i = 0; i < CreditNoteDetails.Rows.Count; i++)
                {
                    DataRow dr = CreditNoteDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spCreditNoteDetailCreate");
                    Global.m_db.AddParameter("@CreditNoteID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Amount += Convert.ToDouble(dr["Amount"]);
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Credit Note");
                    }
                    //Also insert the transaction in tbltransaction
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, CreditNoteDate);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CR_NOT");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);
                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Credit Note");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CR_NOT");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Credit Note");
                        }
                    }
                }
                string LedgerName = Ledger.GetLedgerNameFromID(LedgerID);
                //Also insert the transaction in tbltransaction for master
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, CreditNoteDate);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, LedgerName);//Set same for both for time being
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Amount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CR_NOT");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);
                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Credit Note");
                }
                //Add the information  in Acc.tblTransactionClass
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1);
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CR_NOT");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Credit Note");
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

        public void Modify(int CreditNoteID, int SeriesID,int LedgerID, string VoucherNo, DateTime CreditNoteDate, string Remarks, DataTable CreditNoteDetails, int[] AccClassID,int ProjectID,OptionalField of)
        {
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spCreditNoteModify");
                Global.m_db.AddParameter("@CreditNoteID", SqlDbType.Int, CreditNoteID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int,SeriesID);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@CreditNote_Date", SqlDbType.DateTime, CreditNoteDate);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                //First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblCreditNoteDetail WHERE CreditNoteID='" + CreditNoteID.ToString() + "'");
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
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='CR_NOT' AND RowID='" + CreditNoteID.ToString() + "'");

                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='CR_NOT' AND RowID='" + CreditNoteID.ToString() + "'");
                double Amount = 0;
                for (int i = 0; i < CreditNoteDetails.Rows.Count; i++)
                {
                    DataRow dr = CreditNoteDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spCreditNoteDetailCreate");
                    Global.m_db.AddParameter("@CreditNoteID", SqlDbType.Int, CreditNoteID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Amount += Convert.ToDouble(dr["Amount"]);
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Credit Note");
                    }
                    //Also insert the transaction in tbltransaction
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, CreditNoteDate);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CR_NOT");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, CreditNoteID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);
                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Credit Note");
                    }

                    //Now add the New editable records for Acc.tblTransactionClass
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, CreditNoteID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CR_NOT");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Credit Note");
                        }
                    }
                }
                string LedgerName = Ledger.GetLedgerNameFromID(LedgerID);
                //Also insert the transaction in tbltransaction
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, CreditNoteDate);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, LedgerName);//Set same for both for time being
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Amount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CR_NOT");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, CreditNoteID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);

                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Credit Note");
                }
                //Add the information  in Acc.tblTransactionClass
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, CreditNoteID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CR_NOT");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Credit Note");
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

        public static DataTable GetCredNoteMasterInfo(DateTime FromDate, DateTime ToDate)
        {
            string SQL = "SELECT * FROM Acc.tblCreditNoteMaster WHERE CreditNote_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "'";
            return Global.m_db.SelectQry(SQL, "table");
        }

        public static DataTable GetCreditNoteMasterInfo(DateTime FromDate, DateTime ToDate, ArrayList AccClassID, string VoucherType)
        {
            string AccClassID1 = "";
            int i = 0;

            foreach (object j in AccClassID)
            {
                if (i == 0)// for first GroupID
                    AccClassID1 = "'" + j.ToString() + "'";
                else  //Separating Other GroupID by commas
                    AccClassID1 += "," + "'" + j.ToString() + "'";
                i++;
            }

            string SQLAccClassID = "";
            if (AccClassID1 != "")
            {
                SQLAccClassID = " AND AccClassID IN (" + AccClassID1 + ")";
            }

            return Global.m_db.SelectQry("SELECT * FROM Acc.tblCreditNoteMaster WHERE CreditNote_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND CreditNoteID IN (SELECT RowID FROM Acc.tblTransactionClass WHERE VoucherType = '" + VoucherType + "'" + SQLAccClassID + ")", "table");
        }

        public static DataTable GetCredNoteDtlInfo(int CreditNoteID)
        {
            //return Global.m_db.SelectQry("SELECT * FROM Acc.tblCreditNoteDetail WHERE CreditNoteID ='" + CreditNoteID + "' AND DrCr ='Credit'", "table");
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblCreditNoteDetail WHERE CreditNoteID ='" + CreditNoteID + "' ", "table");
        }

        /// <summary>
        /// Outputs the Credit Note Detail Information. 
        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public DataTable GetCreditNoteDetail(int CreditNoteID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spGetCreditNoteDetail");
            Global.m_db.AddParameter("@CreditNoteID", SqlDbType.Int, CreditNoteID);
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
            DataTable dtCreditNoteDtl = Global.m_db.GetDataTable();
            return dtCreditNoteDtl;
        }

        public DataTable GetCreditNoteMaster(int CreditNoteID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Acc.tblCreditNoteMaster WHERE CreditNoteID ='" + CreditNoteID + "'", "CreditNoteMaster");
        }

        /// <summary>
        /// Outputs the previous Credit Note Voucher Information. It sends blank if no previous record is available
        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public DataTable NavigateCreditNoteMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spCreditNoteNavigate");
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

            DataTable dtCreditNoteMst = Global.m_db.GetDataTable();
            return dtCreditNoteMst;
        }


        /// <summary>
        /// Simply Deletes the Credit Note from given Credit Note ID
        /// </summary>
        /// <param name="JournalID"></param>
        public bool Delete(int CreditNoteID)
        {
            //If he's sure he has intentionally pressed delete button,

            try
            {

                //Flush data from Transaction table
                Global.m_db.InsertUpdateQry("DELETE Acc.tblTransaction WHERE RowID='" + CreditNoteID.ToString() + "' AND VoucherType='CR_NOT'");

                //Delete data from Journal Master, Journal Details gets automatically flushed away
                Global.m_db.InsertUpdateQry("DELETE Acc.tblCreditNoteMaster WHERE CreditNoteID='" + CreditNoteID.ToString() + "'");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public int GetSeriesIDFromMasterID(int MasterID)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Acc.tblCreditNoteMaster WHERE CreditNoteID ='" + MasterID + "'");
            return Convert.ToInt32(returnID);
        }

    }
}
