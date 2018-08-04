using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using DateManager;

/************************************
 * Class Name: SalesReturn
 * Version: 1.1.3 
 * Created By: Bimal khadka
 * Created Date:  2014
 * Modified Date: 23rd Jan, 2015
 * Changes: SalesReturn Entry in Inventory Transaction table  and Applied Transactions begin(),Commit(),RollBack() to all craete and MOdify,Changes code to modify AuditLog
 * 
 * *********************************/


namespace BusinessLogic
{
    public class SalesReturn
    {
        public void Create(int SeriesID, int SalesLedgerID, string salesledgername, int CashPartyLedgerID, string cashpartyname, int Tax1ID, int Tax2ID, int Tax3ID, int VatID, int DepotID, string Order_No, string Voucher_No, DateTime SalesReturn_Date, string Remarks, DataTable SalesReturnDetails, int[] AccClassID, int ProjectID, double Tax1, string Tax1On, string Tax1OnCheck, double Tax2, string Tax2On, string Tax2OnCheck, double Tax3, string Tax3On, string Tax3OnCheck, string oldgrid, string newgrid, bool isnew, string tax1, string tax2, string tax3, string vat, string totalqty, string grossamt, string specialdiscount, string netamount, OptionalField of, DataTable dtVoucherRecurring, DataTable dtReference)
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

            //Check if the SalesInvoiceDetails has fields
            if (SalesReturnDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
            }

            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();

            ////This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in SalesReturnDetails.Rows)
            {
                //Check whether the ledger name are correct
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetProductIDFromName");
                Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 50, row["Product"].ToString());
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 20, Language);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                if (objReturn.Value.ToString() == "-100")//It return -100 in case of failure
                    throw new Exception("Services Name - " + row["Product"].ToString() + " not found!");
            }

            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spSalesReturnCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@SalesLedgerID", SqlDbType.Int, SalesLedgerID);
                Global.m_db.AddParameter("@CashPartyLedgerID", SqlDbType.Int, CashPartyLedgerID);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, Voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@SalesReturn_Date", SqlDbType.DateTime, SalesReturn_Date);
                Global.m_db.AddParameter("@Order_No", SqlDbType.NVarChar, Order_No);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                Global.m_db.AddParameter("@Tax1", SqlDbType.Money, tax1);
                Global.m_db.AddParameter("@Tax2", SqlDbType.Money, tax2);
                Global.m_db.AddParameter("@Tax3", SqlDbType.Money, tax3);
                Global.m_db.AddParameter("@VAT", SqlDbType.Money, vat);
                Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, netamount);
                Global.m_db.AddParameter("@Total_Amt", SqlDbType.Money, (Convert.ToDouble(netamount) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(vat)));
                Global.m_db.AddParameter("@Total_Qty", SqlDbType.Float, totalqty);
                Global.m_db.AddParameter("@Gross_Amount", SqlDbType.Money, grossamt);
                Global.m_db.AddParameter("@SpecialDiscount", SqlDbType.Money, specialdiscount);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnID = Convert.ToInt32(objReturn.Value);
                double NetAmount = 0;
                double GrossAmount = 0;
                double TotalTaxAmount = 0;
                double VatAmt = 0;
                for (int i = 0; i < SalesReturnDetails.Rows.Count; i++)
                {
                    DataRow dr = SalesReturnDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spSalesReturnDetailCreate");
                    Global.m_db.AddParameter("@SalesReturnID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, Convert.ToDouble(dr["SalesRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DiscPercentage", SqlDbType.Money, Convert.ToDouble(dr["DiscPercentage"]));
                    Global.m_db.AddParameter("@Discount", SqlDbType.Money, Convert.ToDouble(dr["Discount"]));
                    Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, Convert.ToDouble(dr["NetAmount"]));

                    NetAmount += Convert.ToDouble(dr["NetAmount"]);
                    GrossAmount += Convert.ToDouble(dr["Amount"]);

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }

                    #region THIS CODE WILL USE WHEN INVENTORY SYSTEM IS IMPLEMENTED
                    //Addition in Inv.tblInventoryTrans 

                    ////Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                    // Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "INCOMING");//in Sales Return....product will return to sales a/c's depot,so increase of product 
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    // if (ParamReturnInventory.Value.ToString() != "SUCCESS")
                    //{
                    //   Global.m_db.RollBackTransaction();
                    //throw new Exception("Unable to create salesReturn inventory transaction");
                    // }

                    #endregion
                }
                try
                {
                    if (isnew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "SRTN";
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
                        string VoucherType = "SRTN";
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

                //Considering Vat according to settings
                if (Global.VAT_Settings == "1")//If vat setting is true then read accoring to settings
                {
                    VatAmt = (NetAmount * (Global.Default_Vat)) / 100;
                }
                else if (Global.VAT_Settings == "0")
                {
                    VatAmt = 0;
                }

                //double m_Tax1Amt = Slabs.CalculateSlabsDetails(SlabType.TAX1, SalesPurchaseType.SALES, (NetAmount + VatAmt), GrossAmount);
                //double m_Tax2Amt = Slabs.CalculateSlabsDetails(SlabType.TAX2, SalesPurchaseType.SALES, (NetAmount + VatAmt), GrossAmount);
                //double m_Tax3Amt = Slabs.CalculateSlabsDetails(SlabType.TAX3, SalesPurchaseType.SALES, (NetAmount + VatAmt), GrossAmount);

                //if (Tax1OnCheck == "1")//if Tax1 is checked then consider its value in transaction too
                //{
                //    TotalTaxAmount += m_Tax1Amt;
                //}
                //if (Tax2OnCheck == "1")
                //{
                //    TotalTaxAmount += m_Tax2Amt;
                //}
                //if (Tax2OnCheck == "1")
                //{
                //    TotalTaxAmount += m_Tax3Amt;
                //}
                //Also insert the transaction in tblTransaction  for Sales

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, SalesLedgerID);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, salesledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                //Global.m_db.AddParameter("@Amount", SqlDbType.Money, ((NetAmount + VatAmt) + TotalTaxAmount));
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (NetAmount));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();


                int ReturnTransactID = Convert.ToInt32(paramReturn1.Value);

                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Return");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                }

                //Also insert the transaction in tbltransaction for Cash/Party
                //In case of Sales Return Party account will receive money from Sales a/c soo Party a/c and Vat a/c will be Debit
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, CashPartyLedgerID);//Set same for both for time being ,if not it will insert  -100 instead of real CashPartLedgerID in transaction table.
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, cashpartyname);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                //Global.m_db.AddParameter("@Amount", SqlDbType.Money, ((NetAmount + VatAmt) + TotalTaxAmount));
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (Convert.ToDouble(netamount) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(vat)));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID1 = Convert.ToInt32(paramReturn3.Value);
                if (paramReturn3.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Return");
                }

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                }

                //Transaction for vat  for VAT On Sales
                if (Convert.ToDecimal(vat) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.VATPayableID);
                    //Global.VATLedgerName = Ledger.GetLedgerNameFromID(Global.VATPayableID);
                    //Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.VATPayableLdg);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, vat);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn4 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID2 = Convert.ToInt32(paramReturn4.Value);
                    if (paramReturn4.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID2.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Return");
                        }
                    }

                }
                //Transaction for Tax1
                if (Convert.ToDecimal(tax1) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax1Name);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax1ID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax1Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax1);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn5 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID3 = Convert.ToInt32(paramReturn5.Value);
                    if (paramReturn5.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Return");
                        }
                    }
                }

                //Transaction for Tax2
                if (Convert.ToDecimal(tax2) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax2ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax2Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax2Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax2);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn6 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    int ReturnTransactID4 = Convert.ToInt32(paramReturn6.Value);
                    if (paramReturn6.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID4.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Return");
                        }
                    }

                }
                //Transaction for Tax3
                if (Convert.ToDecimal(tax3) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax3ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax3Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax3Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax3);

                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn7 = Global.m_db.AddOutputParameter("Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    int ReturnTransactID5 = Convert.ToInt32(paramReturn7.Value);
                    if (paramReturn7.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID5.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Return");
                        }
                    }

                }
                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherID"] = ReturnID;
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "SALES_RETURN";
                    string res = RecurringVoucher.CreateRecurringVoucherSetting(dtVoucherRecurring);
                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return due to recurring settings.");
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
                ////}
                #endregion
            }
        }

        public void Modify(int SalesReturnID, int SeriesID, int SalesLedgerID, string salesledgername, int CashPartyLedgerID, int Tax1ID, int Tax2ID, int Tax3ID, int VatID, int DepotID, string Order_No, string Voucher_No, DateTime SalesReturn_Date, string Remarks, DataTable SalesReturnDetails, int[] AccClassID, int ProjectID, double Tax1, string Tax1On, string Tax1OnCheck, double Tax2, string Tax2On, string Tax2OnCheck, double Tax3, string Tax3On, string Tax3OnCheck, string oldgrid, string newgrid, bool isnew, string tax1, string tax2, string tax3, string VAT, string totalqty, string grossamt, string specialdiscount, string netamount, OptionalField of, DataTable dtVoucherRecurring, DataTable dtReference, string ToDeleteRows)
        {
            double NetAmount = 0;
            double VatAmt = 0;
            double GrossAmount = 0;
            double TotalTaxAmount = 0;

            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spSalesReturnModify");
                Global.m_db.AddParameter("@SalesReturnID", SqlDbType.Int, SalesReturnID);
                Global.m_db.AddParameter("DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@SalesLedgerID", SqlDbType.Int, SalesLedgerID);
                Global.m_db.AddParameter("@CashPartyLedgerID", SqlDbType.Int, CashPartyLedgerID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, Voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@SalesReturn_Date", SqlDbType.DateTime, SalesReturn_Date);
                Global.m_db.AddParameter("@Order_No", SqlDbType.NVarChar, Order_No);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                Global.m_db.AddParameter("@Tax1", SqlDbType.Money, tax1);
                Global.m_db.AddParameter("@Tax2", SqlDbType.Money, tax2);
                Global.m_db.AddParameter("@Tax3", SqlDbType.Money, tax3);
                Global.m_db.AddParameter("@VAT", SqlDbType.Money, VAT);
                Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, netamount);
                Global.m_db.AddParameter("@Total_Amt", SqlDbType.Money, (Convert.ToDouble(netamount) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(VAT)));
                Global.m_db.AddParameter("@Total_Qty", SqlDbType.Float, Convert.ToDouble(totalqty));
                Global.m_db.AddParameter("@Gross_Amount", SqlDbType.Money, grossamt);
                Global.m_db.AddParameter("@SpecialDiscount", SqlDbType.Money, specialdiscount);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                //int ReturnID = Convert.ToInt32(objReturn.Value);

                //First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblSalesReturnDetails WHERE SalesReturnID='" + SalesReturnID.ToString() + "'");

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
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='SLS_RTN' AND RowID='" + SalesReturnID.ToString() + "'");


                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='SLS_RTN' AND RowID='" + SalesReturnID.ToString() + "'");

                //First delete the old transaction from table inventorytransaction according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM inv.tblInventoryTrans WHERE VoucherType='SLS_RTN' AND RowID='" + SalesReturnID.ToString() + "'");

                for (int i = 0; i < SalesReturnDetails.Rows.Count; i++)
                {
                    DataRow dr = SalesReturnDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spSalesReturnDetailCreate");
                    Global.m_db.AddParameter("@SalesReturnID", SqlDbType.Int, SalesReturnID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, Convert.ToDouble(dr["SalesRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DiscPercentage", SqlDbType.Money, Convert.ToDouble(dr["DiscPercentage"]));
                    Global.m_db.AddParameter("@Discount", SqlDbType.Money, Convert.ToDouble(dr["Discount"]));
                    Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, Convert.ToDouble(dr["NetAmount"]));
                    NetAmount += Convert.ToDouble(dr["NetAmount"]);
                    GrossAmount += Convert.ToDouble(dr["Amount"]);

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }



                    #region THIS CODE WILL USE WHEN INVENTORY SYSTEM IS IMPLEMENTED
                    //Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                    //  Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "INCOMING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    //if (ParamReturnInventory.Value.ToString() != "SUCCESS")
                    //{
                    //    Global.m_db.RollBackTransaction();
                    //    throw new Exception("Unable to create salesReturn inventory transaction");
                    //}
                    #endregion
                }

                //Log the record into tblauditlog
                bool bAudit = AuditLog.AppendLog(Global.ConcatAllCompInfo, User.CurrentUserName, "SRTN", "INSERT", oldgrid + newgrid, SalesReturnID, SalesReturn_Date);

                VatAmt = Convert.ToDouble(VAT);
                //Also consider the 13% vat
                //if (Global.VAT_Settings == "1")//If vat setting is true then read accoring to settings
                //{
                //    VatAmt = (NetAmount * (Global.Default_Vat)) / 100;
                //}
                //else if (Global.VAT_Settings == "0")
                //{
                //    VatAmt = (NetAmount) / 100;
                //}
                double m_Tax1Amt = Slabs.CalculateSlabsDetails(SlabType.TAX1, SalesPurchaseType.SALES, (NetAmount + VatAmt), GrossAmount);
                double m_Tax2Amt = Slabs.CalculateSlabsDetails(SlabType.TAX2, SalesPurchaseType.SALES, (NetAmount + VatAmt), GrossAmount);
                double m_Tax3Amt = Slabs.CalculateSlabsDetails(SlabType.TAX3, SalesPurchaseType.SALES, (NetAmount + VatAmt), GrossAmount);

                if (Tax1OnCheck == "1")//if Tax1 is checked then consider its value in transaction too
                {
                    TotalTaxAmount += m_Tax1Amt;
                }
                if (Tax2OnCheck == "1")
                {
                    TotalTaxAmount += m_Tax2Amt;
                }
                if (Tax2OnCheck == "1")
                {
                    TotalTaxAmount += m_Tax3Amt;
                }
                //Also insert the transaction in tblTransaction  for Sales

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, salesledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (NetAmount));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int ReturnTransactID = Convert.ToInt32(paramReturn1.Value);
                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Return");
                }

                //Now add the New editable records for Acc.tblTransactionClass     

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                }
                //Also insert the transaction in tbltransaction for Cash/Party
                //In case of Sales Return Party account will receive money from Sales a/c soo Party a/c and Vat a/c will be Debit
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, CashPartyLedgerID);//Set same for both for time being
                //Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, cashpartyname);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.NVarChar, 30, CashPartyLedgerID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, ((NetAmount + VatAmt) + TotalTaxAmount));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();


                int ReturnTransactID1 = Convert.ToInt32(paramReturn3.Value);

                //In case of failure Rollback the transaction 
                if (paramReturn3.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Return");
                }

                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                }
                //Transaction for vat  for VAT On Sales

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.VATPayableID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, VatAmt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn4 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID2 = Convert.ToInt32(paramReturn4.Value);
                if (paramReturn4.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Return");
                }

                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID2.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                }

                //Transaction for Tax1
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax1ID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax1Amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn5 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID3 = Convert.ToInt32(paramReturn5.Value);
                if (paramReturn5.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Return");
                }
                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                }

                //Transaction for Tax2
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax2ID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax2Amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn6 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int ReturnTransactID4 = Convert.ToInt32(paramReturn6.Value);
                if (paramReturn6.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Return");
                }
                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID4.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                }

                //Transaction for Tax3
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax3ID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax3Amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn7 = Global.m_db.AddOutputParameter("Return", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int ReturnTransactID5 = Convert.ToInt32(paramReturn7.Value);
                if (paramReturn7.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Return");
                }
                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID5.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SLS_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Return");
                    }
                }

                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "SALES_RETURN";
                    dtVoucherRecurring.Rows[0]["VoucherID"] = SalesReturnID;

                    string result = RecurringVoucher.ModifyRecurringVoucherSetting(dtVoucherRecurring);

                    if (result == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Sales Return due to recurring settings.");
                    }
                }
                #endregion

                // to modify against references in the voucher
                ModifyReference(SalesReturnID, dtReference, ToDeleteRows);

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

        public DataTable NavigateSalesReturnMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spSalesReturnNavigate");
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
            DataTable dtSalesReturnMst = Global.m_db.GetDataTable();
            return dtSalesReturnMst;
        }

        //added new
        /*
        public DataTable NavigateSalesReturnDetails(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spSalesReturnNavigate");
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
            DataTable dtSalesReturnDetails = Global.m_db.GetDataTable();
            return dtSalesReturnDetails;
        }
        */
        public DataTable GetSalesReturnDetails(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spGetSalesReturnDetail");
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

        public DataTable GetSalesReturnMasterInfo(string RowID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblSalesReturnMaster WHERE SalesReturnID ='" + RowID + "'", "table");
        }

        public int GetSeriesIDFromMasterID(int MasterID)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Inv.tblSalesReturnMaster WHERE SalesReturnID ='" + MasterID + "'");
            return Convert.ToInt32(returnID);
        }

        public bool RemoveSalesReturnEntry(int SalesReturnID, string GridValues)
        {
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spRemoveSalesReturnEntry");
                Global.m_db.AddParameter("@SalesReturnID", SqlDbType.Int, SalesReturnID);
                //system.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                string username = User.CurrentUserName;
                string voucherdate = Date.ToDB(DateTime.Now).ToString();
                string VoucherType = "SINV";
                string action = "DELETE";
                int rowid = 0;
                string ComputerName = Global.ComputerName;
                string MacAddress = Global.MacAddess;
                string IpAddress = Global.IpAddress;
                string desc = GridValues;

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

                Global.m_db.CommitTransaction();
                return true;
            }
            catch
            {
                Global.m_db.RollBackTransaction();
                return false;
            }
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
                    VoucherReference.CreateReference(dr, voucherID, "SLS_RTN");
                }

                foreach (DataRow dr in dtReference.Select("[RefID] is not null"))
                {
                    VoucherReference.CreateReferenceVoucher(dr, voucherID, "SLS_RTN");
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
                        VoucherReference.CreateReference(dr, voucherID, "SLS_RTN");
                    }

                    foreach (DataRow dr in dtReference.Select("[RefID] is not null and [RVID] is null"))
                    {
                        VoucherReference.CreateReferenceVoucher(dr, voucherID, "SLS_RTN");
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
