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
    public class Contra
    {
        public void Create(int SeriesID, string VoucherNo, DateTime ContraDate, string Remarks, DataTable ContraDetails, int[] AccClassID, int ProjectID, string oldgrid, string newgrid, bool isnew, OptionalField of, DataTable dtVoucherRecurring, DataTable dtReference)
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
            if (ContraDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
                return;
            }
            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();
            //This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in ContraDetails.Rows)
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
            }
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spContraCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@Contra_Date", SqlDbType.DateTime, ContraDate);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnID = Convert.ToInt32(objReturn.Value);
                for (int i = 0; i < ContraDetails.Rows.Count; i++)
                {
                    //string ChequeDate = "";
                    // DateTime ChequeDateValue;
                    //ChequeDateValue = Date.ToDotNet(ChequeDate);
                    // tw.WriteElementString("ChequeDate", Date.ToDB(ChequeDateValue));
                    DateTime? chequedatevalue;
                    DataRow dr = ContraDetails.Rows[i];
                    string cheqdate = dr["ChequeDate"].ToString();
                    if (cheqdate != "")
                    {
                        chequedatevalue = Date.ToDotNet(cheqdate);
                    }
                    else
                    {
                        chequedatevalue = null;
                    }
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spContraDetailCreate");
                    Global.m_db.AddParameter("@ContraID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString());
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    Global.m_db.AddParameter("@chequenumber", SqlDbType.NVarChar, 20, dr["ChequeNumber"].ToString());
                    //Global.m_db.AddParameter("@chequedate", SqlDbType.NVarChar, 20, dr["ChequeDate"].ToString());
                    if (cheqdate != "")
                    {
                        Global.m_db.AddParameter("@chequedate", SqlDbType.DateTime, chequedatevalue);
                    }
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Contra");
                    }
                    //Also insert the transaction in tbltransaction
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, ContraDate);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString().ToUpper());
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CNTR");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);
                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Contra");
                    }

                    try
                    {
                        if (isnew == true)
                        {
                            string username = User.CurrentUserName;
                            string voucherdate = Date.ToDB(DateTime.Now).ToString();
                            string VoucherType = "CONTRA";
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
                            string VoucherType = "CONTRA";
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

                    //Now add the New editable records for Acc.tblTransactionClass
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CNTR");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Contra");
                        }
                    }
                }
                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherID"] = ReturnID;
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "CONTRA";
                    string res = RecurringVoucher.CreateRecurringVoucherSetting(dtVoucherRecurring);
                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Contra voucher due to recurring settings.");
                    }
                }
                #endregion

                // to send reference information
                InsertReference(ReturnID, dtReference);
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

        public void Modify(int ContraID, int SeriesID, string VoucherNo, DateTime ContraDate, string Remarks, DataTable ContraDetails, int[] AccClassID, int ProjectID, string oldgrid, string newgrid, bool isnew, OptionalField of, DataTable dtVoucherRecurring, DataTable dtReference, string ToDeleteRows)
        {
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spContraModify");
                Global.m_db.AddParameter("@ContraID", SqlDbType.Int, ContraID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNo);//Set same for both for time being
                Global.m_db.AddParameter("@Contra_Date", SqlDbType.DateTime, ContraDate);
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
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblContraDetails WHERE ContraID='" + ContraID.ToString() + "'");
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

                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='CNTR' AND RowID='" + ContraID.ToString() + "'");

                //First delete the old transaction
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='CNTR' AND RowID='" + ContraID.ToString() + "'");



                for (int i = 0; i < ContraDetails.Rows.Count; i++)
                {
                    DataRow dr = ContraDetails.Rows[i];
                    DateTime? chequedatevalue;
                    string cheqdate = dr["ChequeDate"].ToString();
                    if (cheqdate != "")
                    {
                        chequedatevalue = Date.ToDotNet(cheqdate);
                    }
                    else
                    {
                        chequedatevalue = null;
                    }
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spContraDetailCreate");
                    Global.m_db.AddParameter("@ContraID", SqlDbType.Int, ContraID.ToString());
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString());
                    Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, dr["Remarks"].ToString());
                    Global.m_db.AddParameter("@chequenumber", SqlDbType.NVarChar, 20, dr["ChequeNumber"].ToString());
                    if (cheqdate != "")
                    {
                        Global.m_db.AddParameter("@chequedate", SqlDbType.DateTime, chequedatevalue);
                    }
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Contra");
                    }
                    //Also insert the transaction in tbltransaction
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, ContraDate);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, dr["Ledger"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, dr["DrCr"].ToString().ToUpper());
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "CNTR");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ContraID);
                    paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID = Convert.ToInt32(paramReturn.Value);

                    if (paramReturn.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Contra");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ContraID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "CNTR");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Contra");
                        }
                    }

                    try
                    {
                        if (isnew == true)
                        {
                            string username = User.CurrentUserName;
                            string voucherdate = Date.ToDB(DateTime.Now).ToString();
                            string VoucherType = "CONTRA";
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
                            string VoucherType = "CONTRA";
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


                }

                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "CONTRA";
                    dtVoucherRecurring.Rows[0]["VoucherID"] = ContraID.ToString();                        
                    string result = RecurringVoucher.ModifyRecurringVoucherSetting(dtVoucherRecurring);

                    if (result == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Contra voucher due to recurring settings.");
                    }
                }
                #endregion

                // to modify against references in the voucher
                ModifyReference(ContraID, dtReference, ToDeleteRows);

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
        public DataTable NavigateContraMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spContraNavigate");
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

            DataTable dtContraMst = Global.m_db.GetDataTable();
            return dtContraMst;


        }

        /// <summary>
        /// Outputs the Journal Detail Information. 
        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public DataTable GetContraDetail(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Acc.spGetContraDetail");
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
            DataTable dtContraDtl = Global.m_db.GetDataTable();
            return dtContraDtl;


        }


        /// <summary>
        /// Simply Deletes the contra from given contra ID
        /// </summary>
        /// <param name="JournalID"></param>
        public bool Delete(int ContraID)
        {
            //If he's sure he has intentionally pressed delete button,

            try
            {

                //Flush data from Transaction table
                Global.m_db.InsertUpdateQry("DELETE Acc.tblTransaction WHERE RowID='" + ContraID.ToString() + "' AND VoucherType='CNTR'");

                //Delete data from Journal Master, Journal Details gets automatically flushed away
                Global.m_db.InsertUpdateQry("DELETE Acc.tblContraMaster WHERE ContraID='" + ContraID.ToString() + "'");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public DataTable GetContraMasterInfo(int ContraID)
        {

            return Global.m_db.SelectQry("SELECT * FROM Acc.tblContraMaster WHERE ContraID ='" + ContraID + "'", "Contra");

        }

        public int GetSeriesIDFromMasterID(int MasterID)
        {

            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Acc.tblContraMaster WHERE ContraID ='" + MasterID + "'");

            return Convert.ToInt32(returnID);


        }
        public static string InsertReference(int voucherID, DataTable dtReference)
        {
            try
            {
                #region Save voucher reference settings
                string res = "";
               // Global.m_db.BeginTransaction();
                foreach (DataRow dr in dtReference.Select("[RefID] is null"))
                {
                    VoucherReference.CreateReference(dr, voucherID, "CNTR");
                }

                foreach (DataRow dr in dtReference.Select("[RefID] is not null"))
                {
                    VoucherReference.CreateReferenceVoucher(dr, voucherID, "CNTR");
                }

                //Global.m_db.CommitTransaction();
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
                        VoucherReference.CreateReference(dr, voucherID, "CNTR");
                    }

                    foreach (DataRow dr in dtReference.Select("[RefID] is not null and [RVID] is null"))
                    {
                        VoucherReference.CreateReferenceVoucher(dr, voucherID, "CNTR");
                    }
                    
                }
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
    }
}
