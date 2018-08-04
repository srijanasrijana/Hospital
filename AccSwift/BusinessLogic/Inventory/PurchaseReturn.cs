using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using DateManager;

/************************************
 * Class Name: PurchaseReturn
 * Version: 1.1.3 
 * Created By: Bimal khadka
 * Created Date:  2014
 * Modified Date: 23rd Jan, 2015
 * Changes: PurchaseReturn Entry in Inventory Transaction table  and Applied Transactions begin(),Commit(),RollBack() to all craete and MOdify,Changes code to modify AuditLog
 * 
 * *********************************/

namespace BusinessLogic
{
    public class PurchaseReturn
    {
        public void Create(int SeriesID, int PurchaseLedgerID, string purchaseledgername, int CashPartyLedgerID, string cashpartyledgername, int Tax1ID, int Tax2ID, int Tax3ID, int VatID, int DepotID, string Order_No, string Voucher_No, DateTime PurchaseReturn_Date, string Remarks, DataTable PurchaseReturnDetails, int[] AccClassID, int ProjectID, double Tax1, string Tax1On, string Tax1OnCheck, double Tax2, string Tax2On, string Tax2OnCheck, double Tax3, string Tax3On, string Tax3OnCheck, string oldgrid, string newgrid, bool isnew, string tax1, string tax2, string tax3, string vat, string totalqty, string grossamt, string specialdiscount, string netamount, OptionalField of, DataTable dtVoucherRecurring)
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

            //Check if the PurchaseInvoiceDetails has fields
            if (PurchaseReturnDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
            }

            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();

            ////This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in PurchaseReturnDetails.Rows)
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
                    throw new Exception("Product Name - " + row["Product"].ToString() + " not found!");
            }

            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spPurchaseReturnCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@PurchaseLedgerID", SqlDbType.Int, PurchaseLedgerID);
                Global.m_db.AddParameter("@CashPartyLedgerID", SqlDbType.Int, CashPartyLedgerID);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, Voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@PurchaseReturn_Date", SqlDbType.DateTime, PurchaseReturn_Date);
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
                for (int i = 0; i < PurchaseReturnDetails.Rows.Count; i++)
                {
                    DataRow dr = PurchaseReturnDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spPurchaseReturnDetailCreate");
                    Global.m_db.AddParameter("@PurchaseReturnID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DiscPercentage", SqlDbType.Money, Convert.ToDouble(dr["DiscPercentage"]));
                    Global.m_db.AddParameter("@Discount", SqlDbType.Money, Convert.ToDouble(dr["Discount"]));
                    Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, Convert.ToDouble(dr["Net_Amount"]));

                    NetAmount += Convert.ToDouble(dr["Net_Amount"]);
                    GrossAmount += Convert.ToDouble(dr["Amount"]);

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }

                    #region THIS CODE WILL USE WHEN INVENTORY SYSTEM IS IMPLEMENTED
                    //Addition in Inv.tblInventoryTrans 

                    ////Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                    //Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "OUTGOING");//in purchase Return....product will return to Cashparty's depot,so decrease of product in purchase depot
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    // if (ParamReturnInventory.Value.ToString() != "SUCCESS")
                    //{
                    //   Global.m_db.RollBackTransaction();
                    //   throw new Exception("Unable to create salesReturn inventory transaction");
                    // }

                    #endregion
                }
                try
                {
                    if (isnew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "PRTN";
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
                        string VoucherType = "PRTN";
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
                    VatAmt = (NetAmount * Global.Default_Vat / 100);
                }
                else if (Global.VAT_Settings == "0")
                {
                    VatAmt = 0;
                }

                double m_Tax1Amt = Slabs.CalculateSlabsDetails(SlabType.TAX1, SalesPurchaseType.PURCHASE, (NetAmount + VatAmt), GrossAmount);
                double m_Tax2Amt = Slabs.CalculateSlabsDetails(SlabType.TAX2, SalesPurchaseType.PURCHASE, (NetAmount + VatAmt), GrossAmount);
                double m_Tax3Amt = Slabs.CalculateSlabsDetails(SlabType.TAX3, SalesPurchaseType.PURCHASE, (NetAmount + VatAmt), GrossAmount);


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
                //Also insert the transaction in tblTransaction  for Purchase

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, PurchaseLedgerID);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, purchaseledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                //Global.m_db.AddParameter("@Amount", SqlDbType.Money, ((NetAmount + VatAmt) + TotalTaxAmount));
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (NetAmount));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();


                int ReturnTransactID = Convert.ToInt32(paramReturn1.Value);

                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }

                //Also insert the transaction in tbltransaction for Cash/Party
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, CashPartyLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, cashpartyledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                // Global.m_db.AddParameter("@Amount", SqlDbType.Money, NetAmount);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, ((NetAmount + VatAmt) + TotalTaxAmount));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID1 = Convert.ToInt32(paramReturn3.Value);
                if (paramReturn3.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
                }

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }

                //Transaction for vat  for VAT On Purchase
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.VATReceivableID);
              //  Global.VATLedgerName = Ledger.GetLedgerNameFromID(Global.VATReceivableID);
                //Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.VATLedgerName);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, VatAmt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn4 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID2 = Convert.ToInt32(paramReturn4.Value);
                if (paramReturn4.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }



                //Transaction for Tax1
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax1ID);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax1Name);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax1Amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn5 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID3 = Convert.ToInt32(paramReturn5.Value);
                if (paramReturn5.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }


                //Transaction for Tax2
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                // Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax2ID);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax2Name);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax2Amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn6 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int ReturnTransactID4 = Convert.ToInt32(paramReturn6.Value);
                if (paramReturn6.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }

                //Transaction for Tax3
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax3ID);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax3Name);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax3Amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn7 = Global.m_db.AddOutputParameter("Return", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int ReturnTransactID5 = Convert.ToInt32(paramReturn7.Value);
                if (paramReturn7.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }
                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherID"] = ReturnID;
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "PURCHASE_RETURN";
                    string res = RecurringVoucher.CreateRecurringVoucherSetting(dtVoucherRecurring);
                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase return due to recurring settings.");
                    }
                }
                #endregion
                Global.m_db.CommitTransaction();

            }
            catch (Exception ex)
            {

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

        public void Modify(int PurchaseReturnID, int SeriesID, int PurchaseLedgerID, int CashPartyLedgerID, int Tax1ID, int Tax2ID, int Tax3ID, int VatID, int DepotID, string Order_No, string Voucher_No, DateTime PurchaseReturn_Date, string Remarks, DataTable PurchaseReturnDetails, int[] AccClassID, int ProjectID, double Tax1, string Tax1On, string Tax1OnCheck, double Tax2, string Tax2On, string Tax2OnCheck, double Tax3, string Tax3On, string Tax3OnCheck, string tax1, string tax2, string tax3, string vat, string totalqty, string grossamt, string specialdiscount, string netamount, OptionalField of, DataTable dtVoucherRecurring)
        {
            double NetAmount = 0;
            double VatAmt = 0;
            double TotalTaxAmount = 0;
            double GrossAmount = 0;
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spPurchaseReturnModify");
                Global.m_db.AddParameter("PurchaseReturnID", SqlDbType.Int, PurchaseReturnID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@PurchaseLedgerID", SqlDbType.Int, PurchaseLedgerID);
                Global.m_db.AddParameter("@CashPartyLedgerID", SqlDbType.Int, CashPartyLedgerID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, Voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@PurchaseReturn_Date", SqlDbType.DateTime, PurchaseReturn_Date);
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
                Global.m_db.AddParameter("@VAT", SqlDbType.Money, vat);
                Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, netamount);
                Global.m_db.AddParameter("@Total_Amt", SqlDbType.Money, (Convert.ToDouble(netamount) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(vat)));
                Global.m_db.AddParameter("@Total_Qty", SqlDbType.Float, totalqty);
                Global.m_db.AddParameter("@Gross_Amount", SqlDbType.Money, grossamt);
                Global.m_db.AddParameter("@SpecialDiscount", SqlDbType.Money, specialdiscount);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                //int ReturnID = Convert.ToInt32(objReturn.Value);

                //First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblPurchaseReturnDetails WHERE PurchaseReturnID='" + PurchaseReturnID.ToString() + "'");

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
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='PURCH_RTN' AND RowID='" + PurchaseReturnID.ToString() + "'");

                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='PURCH_RTN' AND RowID='" + PurchaseReturnID.ToString() + "'");

                //First delete the old transaction from table inventorytransaction according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM inv.tblInventoryTrans WHERE VoucherType='PURCH_RTN' AND RowID='" + PurchaseReturnID.ToString() + "'");


                for (int i = 0; i < PurchaseReturnDetails.Rows.Count; i++)
                {
                    DataRow dr = PurchaseReturnDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spPurchaseReturnDetailCreate");
                    Global.m_db.AddParameter("@PurchaseReturnID", SqlDbType.Int, PurchaseReturnID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DiscPercentage", SqlDbType.Money, Convert.ToDouble(dr["DiscPercentage"]));
                    Global.m_db.AddParameter("@Discount", SqlDbType.Money, Convert.ToDouble(dr["Discount"]));
                    Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, Convert.ToDouble(dr["NetAmount"]));
                    NetAmount += Convert.ToDouble(dr["NetAmount"]);

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }


                    #region THIS CODE WILL USE WHEN INVENTORY SYSTEM IS IMPLEMENTED
                    //Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                    //   Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "OUTGOING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID);
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
                //  bool bAudit = AuditLog.AppendLog(Global.ConcatAllCompInfo, User.CurrentUserName, "SRTN", "INSERT", oldgrid + newgrid, SalesReturnID, SalesReturn_Date);

                //Also consider the 13% vat
                if (Global.VAT_Settings == "1")//If vat setting is true then read accoring to settings
                {
                    VatAmt = (NetAmount * (Global.Default_Vat)) / 100;
                }
                else if (Global.VAT_Settings == "0")
                {
                    VatAmt = (NetAmount) / 100;

                }

                double m_Tax1Amt = Slabs.CalculateSlabsDetails(SlabType.TAX1, SalesPurchaseType.PURCHASE, (NetAmount + VatAmt), GrossAmount);
                double m_Tax2Amt = Slabs.CalculateSlabsDetails(SlabType.TAX2, SalesPurchaseType.PURCHASE, (NetAmount + VatAmt), GrossAmount);
                double m_Tax3Amt = Slabs.CalculateSlabsDetails(SlabType.TAX3, SalesPurchaseType.PURCHASE, (NetAmount + VatAmt), GrossAmount);


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
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, PurchaseLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (NetAmount + VatAmt) + TotalTaxAmount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID = Convert.ToInt32(paramReturn1.Value);
                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
                }

                //Now add the New editable records for Acc.tblTransactionClass     

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }
                //Also insert the transaction in tbltransaction for Cash/Party
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, CashPartyLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, NetAmount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID1 = Convert.ToInt32(paramReturn3.Value);

                if (paramReturn3.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
                }

                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }
                //Transaction for vat  for VAT On PURCHASE

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.VATPayableID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, VatAmt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn4 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID2 = Convert.ToInt32(paramReturn4.Value);
                if (paramReturn4.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
                }

                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID2.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }


                //Transaction for Tax1
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax1ID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax1Amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn5 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID3 = Convert.ToInt32(paramReturn5.Value);
                if (paramReturn5.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
                }
                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }


                //Transaction for Tax2
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax2ID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax2Amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn6 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int ReturnTransactID4 = Convert.ToInt32(paramReturn6.Value);
                if (paramReturn6.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
                }
                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID4.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }

                //Transaction for Tax3
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseReturn_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax3ID);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax3Amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn7 = Global.m_db.AddOutputParameter("Return", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int ReturnTransactID5 = Convert.ToInt32(paramReturn7.Value);
                if (paramReturn7.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Return");
                }
                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID5.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH_RTN");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Return");
                    }
                }
                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "PURCHASE_RETURN";
                    string result = RecurringVoucher.ModifyRecurringVoucherSetting(dtVoucherRecurring);

                    if (result == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Purchase Return due to recurring settings.");
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
                ////}
                #endregion
            }
        }

        public DataTable GetPurchaseReturnMasterInfo(string RowID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblPurchaseReturnMaster WHERE PurchaseReturnID ='" + RowID + "'", "table");
        }

        public int GetSeriesIDFromMasterID(int MasterID)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Inv.tblPurchaseReturnMaster WHERE PurchaseReturnID ='" + MasterID + "'");
            return Convert.ToInt32(returnID);
        }

        public DataTable GetPurchaseReturnDetails(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spGetPurchaseReturnDetail");
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
            DataTable dtPurchaseRtnDtl = Global.m_db.GetDataTable();
            return dtPurchaseRtnDtl;
        }

        public DataTable NavigatePurchaseReturnMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spPurchaseReturnNavigate");
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

            DataTable dtPurchaseReturnMst = Global.m_db.GetDataTable();
            return dtPurchaseReturnMst;
        }
        public bool RemovePurchaseReturnEntry(int PurchaseReturnID, string GridValues)
        {
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spRemovePurchaseReturnEntry");
                Global.m_db.AddParameter("@PurchaseReturnID", SqlDbType.Int, PurchaseReturnID);
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
        //public DataTable GetPurchaseReturnMasterInfo(string RowID)
        //{
        //    return Global.m_db.SelectQry("select * from Inv.tblPurchaseReturnMaster WHERE PurchaseReturnID ='" + RowID + "'", "table");
        //}

    }
}
