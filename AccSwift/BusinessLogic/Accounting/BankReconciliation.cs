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
    public class BankReconciliation
    {
        private double TotalBankReconciliationAmount=0;
        private string DrCrValue = "";

        // public void Create(int SeriesID, string VoucherNo, DateTime ContraDate, string Remarks, DataTable ContraDetails, int[] AccClassID,int ProjectID)
        public void Create(int LedgerID, DateTime BankReconciliation_Date, DataTable BankReconciliationDetails, string VoucherNo, int ProjectID, string LedgerName,
            int SeriesID, int[] AccClassID,OptionalField of, string Remarks,bool isnew,string oldgrid, string newgrid)
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

            ////Check if the JournalDetails has fields

            if (BankReconciliationDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
                return;
            }


            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();


            ////This loop is to check whether ledger names are correct and properly implemented
            //foreach (DataRow row in BankReconciliationDetails.Rows)
            //{
            //    //Check whether the ledger name are correct
            //    Global.m_db.ClearParameter();
            //    Global.m_db.setCommandType(CommandType.StoredProcedure);
            //    Global.m_db.setCommandText("Acc.spGetLedgerIDFromName");
            //    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 50, row["Ledger"].ToString());
            //    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, Language);
            //    System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
            //    Global.m_db.ProcessParameter();

            //    if (objReturn.Value.ToString() == "-100")//It return -100 in case of failure
            //        throw new Exception("Ledger Name - " + row["Ledger"].ToString() + " not found!");
            //}

            try
            {
            //    // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spBankReconciliationCreate");
                //Global.m_db.AddParameter("@SeriesName", SqlDbType.NVarChar, 50, SeriesName);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@BankReconciliation_Date", SqlDbType.DateTime, BankReconciliation_Date);
                Global.m_db.AddParameter("@voucherno",SqlDbType.NVarChar,VoucherNo);
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 500, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 500, ProjectID);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnID = Convert.ToInt32(objReturn.Value);

                for (int i = 0; i < BankReconciliationDetails.Rows.Count; i++)
                {
                    DataRow dr = BankReconciliationDetails.Rows[i];
                    DrCrValue = dr["DrCr"].ToString();
                    TotalBankReconciliationAmount =TotalBankReconciliationAmount+ Convert.ToDouble(dr["Amount"]);
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spBankReconciliationDetailCreate");
                    Global.m_db.AddParameter("@BankReconciliationID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                   
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    //Amount += Convert.ToDouble(dr["Amount"]);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString());
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Bank Reconciliation");
                    }

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, BankReconciliation_Date);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());
                   // Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);//Set same for both for time being
                    //MessageBox.Show("THE Ledger IS" + dr["Ledger"].ToString());
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    if (dr["DrCr"].ToString() == "Debit")
                    {
                        Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    }
                    else if (dr["DrCr"].ToString() == "Credit")
                    {
                        Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    }
                   // MessageBox.Show("THE Amount IS" + dr["Amount"].ToString());
                   
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "BRECON");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@projectid", SqlDbType.Int, 0);
                    //MessageBox.Show("THE DRCR IS" + dr["DrCr"].ToString());
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    //System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);
                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create BankReconciliation");
                    }


                    //Now add the New editable records for Acc.tblTransactionClass
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                       // Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "BRECON");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Bank Reconciliation");
                        }
                    }
                }
                //For Bank Information
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, BankReconciliation_Date);
                //Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, LedgerName);//Set same for both for time being
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, dr["LedgerID"].ToString());
                // Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(TotalBankReconciliationAmount));
                if (DrCrValue == "Debit")
                {
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                }
                else if (DrCrValue == "Credit")
                {
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                }
                //Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString().ToUpper());
                // Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CNTR");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "BRECON");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 0);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
               // paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);
                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Bank Reconciliation");
                }

                //Now add the New editable records for Acc.tblTransactionClass
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    // Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "BRECON");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Bank Reconciliation");
                    }
                }

                try
                {
                    if (isnew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "BRECON";
                        string action = "INSERT";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldgrid + newgrid;

                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("system.spAddAuditLog");
                        Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
                        Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
                        Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
                        Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
                        Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
                        Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
                        Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
                        object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                    }
                    else if (isnew == false)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "BRECON";
                        string action = "UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldgrid + newgrid;

                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("system.spAddAuditLog");
                        Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
                        Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
                        Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
                        Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
                        Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
                        Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
                        Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
                        Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
                        object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                    }
                }
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
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
        public void Modify(int BankReconciliationID, int LedgerID, DateTime BankReconciliation_Date, DataTable BankReconciliationDetails, string VoucherNo, int ProjectID, string LedgerName, int SeriesID, int[] AccClassID, OptionalField of, string Remarks)
        {
            try
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

                Global.m_db.BeginTransaction();

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spBankReconciliationModify");
                Global.m_db.AddParameter("@BankReconciliationID", SqlDbType.Int, BankReconciliationID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, LedgerID);
                Global.m_db.AddParameter("@BankReconciliation_Date", SqlDbType.DateTime, BankReconciliation_Date);
                Global.m_db.AddParameter("@voucherno", SqlDbType.NVarChar, VoucherNo);
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 500, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 500, ProjectID);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='BRECON' AND RowID=" + BankReconciliationID + "");

                //First delete the old transaction
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='BRECON' AND RowID=" + BankReconciliationID + "");
               
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblBankReconciliationDetails WHERE BankReconciliationID=" + BankReconciliationID + "");

                for (int i = 0; i < BankReconciliationDetails.Rows.Count; i++)
                {
                    DataRow dr = BankReconciliationDetails.Rows[i];
                    DrCrValue = dr["DrCr"].ToString();
                    TotalBankReconciliationAmount = TotalBankReconciliationAmount + Convert.ToDouble(dr["Amount"]);
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spBankReconciliationDetailCreate");
                    Global.m_db.AddParameter("@BankReconciliationID", SqlDbType.Int, BankReconciliationID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being

                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString());
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Bank Reconciliation");
                    }

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, BankReconciliation_Date);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    if (dr["DrCr"].ToString() == "Debit")
                    {
                        Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    }
                    else if (dr["DrCr"].ToString() == "Credit")
                    {
                        Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    }

                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "BRECON");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, BankReconciliationID);
                    Global.m_db.AddParameter("@projectid", SqlDbType.Int, 0);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);
                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify BankReconciliation");
                    }


                    //Now add the New editable records for Acc.tblTransactionClass
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, BankReconciliationID);
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "BRECON");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to modify Bank Reconciliation");
                        }
                    }
                }
                //For Bank Information
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, BankReconciliation_Date);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, LedgerName);//Set same for both for time being
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(TotalBankReconciliationAmount));
                if (DrCrValue == "Debit")
                {
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                }
                else if (DrCrValue == "Credit")
                {
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                }
                
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "BRECON");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, BankReconciliationID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, 0);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID1 = Convert.ToInt32(paramReturn1.Value);
                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to modify Bank Reconciliation");
                }

                //Now add the New editable records for Acc.tblTransactionClass
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, BankReconciliationID);
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "BRECON");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Bank Reconciliation");
                    }
                }

                Global.m_db.CommitTransaction();
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// Simply Deletes the Bank Reconciliation from given BankReconciliation ID
        /// </summary>
        /// <param name="BankReconciliationID"></param>
        public static bool RemoveBankReconciliationEntry(int BankReconciliationID)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spRemoveBankReconciliationEntry");
                Global.m_db.AddParameter("@BankReconciliationID", SqlDbType.Int, BankReconciliationID);
                Global.m_db.ProcessParameter();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int GetSeriesIDFromMasterID(int MasterID)
        {
            try
            {
                object returnID;
                returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Acc.tblBankReconciliationMaster WHERE BankReconciliationID ='" + MasterID + "'");
                return Convert.ToInt32(returnID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable NavaigateToBankReconciliation(Navigate nav,int BankReconciliationID, int BankID)
        {
            try
            {
                string sql = "select det.BankReconciliationID,(select LedgerID from Acc.tblBankReconciliationMaster m where det.BankReconciliationID = m.BankReconciliationID) as BankID, led.LedgerID, led.EngName, det.DrCr, det.Amount, det.Remarks  from Acc.tblBankReconciliationDetails det cross join Acc.tblLedger led where det.LedgerID = led.LedgerID ";
                switch (nav)
                {
                    case Navigate.First:
                        sql += " and det.BankReconciliationID = (select min(d.BankReconciliationID) from  Acc.tblBankReconciliationDetails d inner join Acc.tblBankReconciliationMaster m on d.BankReconciliationID = m.BankReconciliationID where m.LedgerID =" + BankID + " )";
                        break;

                    case Navigate.Prev:
                        sql += " and det.BankReconciliationID = (select top 1 d.BankReconciliationID from  Acc.tblBankReconciliationDetails d inner join Acc.tblBankReconciliationMaster m on d.BankReconciliationID = m.BankReconciliationID where m.LedgerID =" + BankID + "  and d.BankReconciliationID < " + BankReconciliationID + " order by d.BankReconciliationID  desc)";
                        break;

                    case Navigate.Next:
                        sql += " and det.BankReconciliationID =(select top 1 d.BankReconciliationID from  Acc.tblBankReconciliationDetails d inner join Acc.tblBankReconciliationMaster m on d.BankReconciliationID = m.BankReconciliationID where m.LedgerID =" + BankID + " and  d.BankReconciliationID > " + BankReconciliationID + ")";
                        break;

                    case Navigate.Last:
                        sql += " and det.BankReconciliationID = (select max(d.BankReconciliationID) from  Acc.tblBankReconciliationDetails d inner join Acc.tblBankReconciliationMaster m on d.BankReconciliationID = m.BankReconciliationID where m.LedgerID =" + BankID + " )";
                        break;

                    case Navigate.ID:
                        sql += " and det.BankReconciliationID = "+BankReconciliationID+" ";
                        break;
                }                 
                return Global.m_db.SelectQry(sql, "Acc.tblBankReconciliationDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetBankReconciliationMaster(int BankReconciliationID)
        {
            try
            {
                string sql = "select mas.BankReconciliationID, mas.SeriesID, led.LedgerID, led.EngName, mas.BankReconciliation_Date,mas.Voucher_No, mas.Field1, mas.Field2, mas.Field3, mas.Field4, mas.Field5, mas.Remarks, mas.ProjectID from Acc.tblBankReconciliationMaster mas cross join Acc.tblLedger led where led.LedgerID = led.LedgerID and mas.BankReconciliationID = " + BankReconciliationID + " ";
                return Global.m_db.SelectQry(sql, "Acc.tblBankReconciliationMaster");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int InsertClosedBankReconciliation(int BankID, DateTime Date, string Description)
        {
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();

                string sql = "delelte from Acc.tblClosedBankReconciliation where BankID = "+BankID+"";
                Global.m_db.InsertUpdateQry(sql);

                Global.m_db.AddParameter("@BankID", SqlDbType.NVarChar, BankID);
                Global.m_db.AddParameter("@Date", SqlDbType.Date, Date);
                Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, Description);
                sql = "insert into Acc.tblClosedBankReconciliation(BankID, Date, Description) values(@BankID, @Date, @Description) ";
                Global.m_db.InsertUpdateQry(sql);
                Global.m_db.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
            }
        }
        public static bool IsBankReconciliationClosed(int LedgerID, DateTime Date)
        {
            string sql = "select BankID from Acc.tblClosedBankReconciliation where BankID = " + LedgerID + "  and Date >= '" + DateManager.Date.ToDB(Date) + "' ";
                if(Global.m_db.SelectQry(sql, "Acc.tblBankReconciliationMaster").Rows.Count == 0)
                    return false;
                else 
                    return true;
        }
        public static bool IsBankReconciliationClosed(int LedgerID)
        {
            string sql = "select BankID from Acc.tblClosedBankReconciliation where BankID = " + LedgerID + " ";
            if (Global.m_db.SelectQry(sql, "Acc.tblBankReconciliationMaster").Rows.Count == 0)
                return false;
            else
                return true;
        }
        public static string ClosingDate = " ";
        public static Decimal GetOpeningBalUptoClosingDate(int BankID)
        {
            try
            {
                DataTable dt = new DataTable();
                Global.m_db.ClearParameter();
                DateTime date = Convert.ToDateTime(Global.m_db.GetScalarValue("select Date from Acc.tblClosedBankReconciliation where BankID =" + BankID + ""));
                ClosingDate = date.ToShortDateString();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetLedgerDetailsByGroupOrLedgerID");
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.NVarChar, 100,"<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>");
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.NVarChar,100, "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>");

                Global.m_db.AddParameter("@Transaction_Start_Date", SqlDbType.DateTime, null); //DBNull.Value);
               
                Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, date);
                // else
                //Global.m_db.AddParameter("@Transaction_End_Date", SqlDbType.DateTime, null);//DBNull.Value);
                //if (LedgerID != null)
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, BankID);
                // else
                //  Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, null);//DBNull.Value);

                //if (GroupID != null)
                Global.m_db.AddParameter("@GroupID", SqlDbType.Int, null);
                //else
                //Global.m_db.AddParameter("@GroupID", SqlDbType.Int, null);//DBNull.Value);

                Global.m_db.AddParameter("@Settings", SqlDbType.Xml,100, null);//"<Settings><SelectColumns><Column>LedgerID</Column><Column>Debit</Column><Column>Credit</Column></SelectColumns></Settings>");

                dt = Global.m_db.GetDataTable();
                 //dt = Global.m_db.SelectQry("Acc.spGetLedgerDetailsByGroupOrLedgerID", "tbl");
                DataRow dr = dt.Rows[0];
                Decimal bal = Convert.ToDecimal(dr["DebitTotal"]) + Convert.ToDecimal(dr["OpenBalDr"]) - (Convert.ToDecimal(dr["CreditTotal"]) + Convert.ToDecimal(dr["OpenBalCr"]));
                return bal;
            }
            catch (Exception ex)
            {
                Global.m_db.RollBackTransaction();
                throw ex;
            }
        }
    }
}
