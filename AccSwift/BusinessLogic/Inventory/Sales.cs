using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using DateManager;


/************************************
 * Class Name: Sales
 * Version: 1.1.3 
 * Created By: Bimal khadka
 * Created Date:  2014
 * Modified Date: 23rd Jan, 2015
 * Changes: Sales Entry in Inventory Transaction table  and Applied Transactions begin(),Commit(),RollBack() to all craete() and MOdify(),Changes code to modify AuditLog
 * 
 * *********************************/


namespace BusinessLogic
{
    public class Sales
    {
        public void Create(int SeriesID, int SalesLedgerID, string salesledgername, int CashPartyLedgerID, string cashpartyledgername, int Tax1ID, int Tax2ID, int Tax3ID, int VatID, int DepotID, int? OrderNo, string Voucher_No, DateTime SalesInvoice_Date, string Remarks, DataTable SalesInvoiceDetails, int[] AccClassID, int ProjectID, double Tax1, string Tax1On, string Tax1OnCheck, double Tax2, string Tax2On, string Tax2OnCheck, double Tax3, string Tax3On, string Tax3OnCheckn, string oldgrid, string newgrid, bool isNew, string tax1, string tax2, string tax3, string totalqty, string grossamt, string netamt, string specialdiscount, string VAT, string DueDays, DateTime DueDate, OptionalField of, DataTable dtVoucherRecurring, DataTable dtReference)
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
            if (SalesInvoiceDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
            }

            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();

            ////This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in SalesInvoiceDetails.Rows)
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
            double NetAmount = 0;
            double VatAmt =0;
            double GrossAmount = 0;
            double TotalTaxAmount = 0;
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spSalesInvoiceCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@SalesLedgerID", SqlDbType.Int, SalesLedgerID);
                Global.m_db.AddParameter("@CashPartyLedgerID", SqlDbType.Int, CashPartyLedgerID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@DueDays", SqlDbType.NVarChar,10, DueDays);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, Voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@SalesInvoice_Date", SqlDbType.DateTime, SalesInvoice_Date);
                Global.m_db.AddParameter("@SalesDueDate", SqlDbType.DateTime, DueDate);
                Global.m_db.AddParameter("@OrderNo", SqlDbType.Int, OrderNo); 
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Tax1", SqlDbType.Money, tax1);
                Global.m_db.AddParameter("@Tax2", SqlDbType.Money, tax2);
                Global.m_db.AddParameter("@Tax3", SqlDbType.Money, tax3);
                Global.m_db.AddParameter("@VAT", SqlDbType.Money, VAT);
                Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, netamt);
                Global.m_db.AddParameter("@Total_Amount", SqlDbType.Money, (Convert.ToDouble(netamt) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(VAT)));
                Global.m_db.AddParameter("@TotalQty", SqlDbType.Float, totalqty);
                Global.m_db.AddParameter("@Gross_Amount", SqlDbType.Money, grossamt);
                Global.m_db.AddParameter("@SpecialDiscount", SqlDbType.Money, specialdiscount);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                double? UpdatedQty = null;
                int ReturnID = Convert.ToInt32(objReturn.Value);
                for (int i = 0; i < SalesInvoiceDetails.Rows.Count; i++)
                {
                    DataRow dr = SalesInvoiceDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                     Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spSalesInvoiceDetailCreate");
                    Global.m_db.AddParameter("@SalesInvoiceID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar,30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float,Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, Convert.ToDouble(dr["SalesRate"]));                  
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DiscPercentage", SqlDbType.Money, Convert.ToDouble(dr["DiscPercentage"]));
                    Global.m_db.AddParameter("@Discount", SqlDbType.Money, Convert.ToDouble(dr["Discount"]));
                    Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, Convert.ToDouble(dr["NetAmount"]));
                    Global.m_db.AddParameter("@QtyUnitID", SqlDbType.Money, Convert.ToDouble(dr["QtyUnitID"]));

                    NetAmount += Convert.ToDouble(dr["NetAmount"]);
                    GrossAmount += Convert.ToDouble(dr["Amount"]);
                
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");   
                    }

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }

                    #region THIS CODE WILL USE WHEN INVENTORY SYSTEM IS IMPLEMENTED
                    //Addition in Inv.tblInventoryTrans 

                    ////Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                   // Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    //Added recently
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "OUTGOING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                   // if (ParamReturnInventory.Value.ToString() != "SUCCESS")
                    //{
                     //   Global.m_db.RollBackTransaction();
                        //throw new Exception("Unable to create sales inventory transaction");
                   // }

                    #endregion

                    //Updating PurchaseOrderDetails table
                    if (dr["SalesOrderDetailsID"].ToString() != "")//Only when we are going to create invoice according to order then only use this portion
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Inv.spSalesOrderDetailModify");
                        Global.m_db.AddParameter("@SalesOrderDetailsID", SqlDbType.Int, Convert.ToInt32(dr["SalesOrderDetailsID"]));
                        UpdatedQty = Convert.ToDouble(dr["Quantity"]);
                        Global.m_db.AddParameter("@UpdatedQty", SqlDbType.Int, UpdatedQty);
                        System.Data.SqlClient.SqlParameter ParamReturnPurchaseOrderDetails = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                    }
                }

                int GroupID = AccountGroup.GetGroupIDByLedgerID(CashPartyLedgerID);
                if (GroupID == 29)
                {
                    #region For Inserting Due Date
                    //FOR Inserting The Due Date 
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("system.spDueDateCreate");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@DueDate", SqlDbType.DateTime, DueDate);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, CashPartyLedgerID);//Set same for both for time being
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 30, "SALES");
                    Global.m_db.AddParameter("@VoucherNo", SqlDbType.NVarChar, 20, Voucher_No);
                    System.Data.SqlClient.SqlParameter DueDateInfo = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (DueDateInfo.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create DueDate");
                    }
                }
                #endregion
               

                try
                {
                    if (isNew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "SINV";
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
                    else if (isNew == false)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "SINV";
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

                 #region Also insert the transaction in tblTransaction  for Sales
                //sales a/c is who is selling the product to party a/c soo it receives the money soo it is debit
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, SalesLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, salesledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
               // (Convert.ToDouble(netamt) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(VatAmt))
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (Convert.ToDouble(netamt)));
               // Global.m_db.AddParameter("@Amount", SqlDbType.Money, (Convert.ToDouble(netamt) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(VAT)));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID = Convert.ToInt32(paramReturn1.Value);

                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Sales Invoice");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                }

                 #endregion
  
                //In case of sales Invoice,Party account is responsible for paying money for Sales a/c soo it pays including Vat soo Cash party and Vat a/c is Credit
                #region Also insert the transaction in tbltransaction for Cash/Party
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int,CashPartyLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, cashpartyledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (Convert.ToDouble(netamt) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(VAT)));
               // Global.m_db.AddParameter("@Amount", SqlDbType.Money, NetAmount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID1 = Convert.ToInt32(paramReturn3.Value);
                if (paramReturn3.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Sales Invoice");
                }


                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }
                }
                #endregion

                #region //Transaction for vat  for VAT On Sales
                if (Convert.ToDecimal(VAT) > 0)
                {
                    Global.VATLedgerName = Ledger.GetLedgerNameFromID(Global.VATPayableID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.VATLedgerID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.VATLedgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, VAT);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn4 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID2 = Convert.ToInt32(paramReturn4.Value);
                    if (paramReturn4.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Sales Invoice");
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
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Invoice");
                        }
                    } 
                }
                #endregion

                #region  //Transaction for Tax1
                if (Convert.ToDecimal(tax1) > 0)
                {
                    Global.Tax1Name = Ledger.GetLedgerNameFromID(Global.Tax1LedgerID);//Change Tax1Name in global.cs according to database name
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax1ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax1Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax1Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax1);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn5 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID3 = Convert.ToInt32(paramReturn5.Value);
                    if (paramReturn5.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Sales Invoice");
                    }

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Invoice");
                        }
                    } 
                }
                    #endregion

                #region Transaction for Tax2
                if (Convert.ToDecimal(tax2) > 0)
                {
                    Global.Tax2Name = Ledger.GetLedgerNameFromID(Global.Tax2LedgerID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax2ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax2Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax2Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax2);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn6 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    int ReturnTransactID4 = Convert.ToInt32(paramReturn6.Value);
                    if (paramReturn6.Value.ToString() == "FAILURE")
                    {
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
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Invoice");
                        }
                    }
                    
                }
                 #endregion

                #region Transaction for Tax3
                if (Convert.ToDecimal(tax3) > 0)
                {
                    Global.Tax3Name = Ledger.GetLedgerNameFromID(Global.Tax3LedgerID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax3ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax3Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax3Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax3);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn7 = Global.m_db.AddOutputParameter("Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    int ReturnTransactID5 = Convert.ToInt32(paramReturn7.Value);
                    if (paramReturn7.Value.ToString() == "FAILURE")
                    {
                        throw new Exception("Unable to create Sales Invoice");
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
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Invoice");
                        }
                    } 
                }
               #endregion

                #region Save voucher recurring settings
                    if (dtVoucherRecurring.Rows.Count > 0)
                    {
                        dtVoucherRecurring.Rows[0]["VoucherID"] = ReturnID;
                        dtVoucherRecurring.Rows[0]["VoucherType"] = "SALES_INVOICE";
                        string res = RecurringVoucher.CreateRecurringVoucherSetting(dtVoucherRecurring);
                        if (res == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Invoice due to recurring settings.");
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

        public void Modify(int SalesInvoiceID, int SeriesID, int SalesLedgerID, string salesledgername, int CashPartyLedgerID, string cashpartyledgername, int Tax1ID, int Tax2ID, int Tax3ID, int VatID, int DepotID, string Order_No, string Voucher_No, DateTime SalesInvoice_Date, string Remarks, DataTable SalesInvoiceDetails, int[] AccClassID, int ProjectID, double Tax1, string Tax1On, string Tax1OnCheck, double Tax2, string Tax2On, string Tax2OnCheck, double Tax3, string Tax3On, string Tax3OnCheckn, string oldgrid, string newgrid, bool isNew, string tax1, string tax2, string tax3, string totalqty, string grossamt, string netamt, string specialdiscount, string VAT, string DueDays, DateTime DueDate, OptionalField of, DataTable dtVoucherRecurring, DataTable dtReference, string RowsToDelete)
        {
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

            double NetAmount = 0;
            double VatAmt = 0;
            double GrossAmount = 0;
            double TotalTaxAmount = 0;
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spSalesInvoiceModify");
                Global.m_db.AddParameter("SalesInvoiceID", SqlDbType.Int, SalesInvoiceID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@SalesLedgerID", SqlDbType.Int, SalesLedgerID);
                Global.m_db.AddParameter("@DueDays", SqlDbType.NVarChar, 10, DueDays);
                Global.m_db.AddParameter("@CashPartyLedgerID", SqlDbType.Int, CashPartyLedgerID);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, Voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@SalesInvoice_Date", SqlDbType.DateTime, SalesInvoice_Date);
                Global.m_db.AddParameter("@SalesDueDate", SqlDbType.DateTime, DueDate);
                Global.m_db.AddParameter("@OrderNo", SqlDbType.NVarChar, Order_No);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Tax1", SqlDbType.Money, tax1);
                Global.m_db.AddParameter("@Tax2", SqlDbType.Money, tax2);
                Global.m_db.AddParameter("@Tax3", SqlDbType.Money, tax3);
                Global.m_db.AddParameter("@VAT", SqlDbType.Money, VAT);
                Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, netamt);
                Global.m_db.AddParameter("@Total_Amount", SqlDbType.Money, (Convert.ToDouble(netamt) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(VAT)));
                Global.m_db.AddParameter("@TotalQty", SqlDbType.Float, totalqty);
                Global.m_db.AddParameter("@Gross_Amount", SqlDbType.Money, grossamt);
                Global.m_db.AddParameter("@SpecialDiscount", SqlDbType.Money, specialdiscount);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                //int ReturnID = Convert.ToInt32(objReturn.Value);




                //First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblSalesInvoiceDetails WHERE SalesInvoiceID='" + SalesInvoiceID.ToString() + "'");


                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='SALES' AND RowID='" + SalesInvoiceID.ToString() + "'");


                //First delete the old transaction
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='SALES' AND RowID='" + SalesInvoiceID.ToString() + "'");

               
                //First delete the old transaction from table inventorytransaction according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM inv.tblInventoryTrans WHERE VoucherType='SALES' AND RowID='" + SalesInvoiceID.ToString() + "'");



                for (int i = 0; i < SalesInvoiceDetails.Rows.Count; i++)
                {
                    DataRow dr = SalesInvoiceDetails.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spSalesInvoiceDetailCreate");
                    Global.m_db.AddParameter("@SalesInvoiceID", SqlDbType.Int, SalesInvoiceID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, Convert.ToDouble(dr["SalesRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DiscPercentage", SqlDbType.Money, Convert.ToDouble(dr["DiscPercentage"]));
                    Global.m_db.AddParameter("@Discount", SqlDbType.Money, Convert.ToDouble(dr["Discount"]));
                    Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, Convert.ToDouble(dr["NetAmount"]));
                    Global.m_db.AddParameter("@QtyUnitID", SqlDbType.Money, Convert.ToDouble(dr["QtyUnitID"]));

                    NetAmount += Convert.ToDouble(dr["NetAmount"]);
                    GrossAmount += Convert.ToDouble(dr["Amount"]);
                   
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }

                    #region THIS CODE WILL USE WHEN INVENTORY SYSTEM IS IMPLEMENTED
                    ////Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                    //Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ProductID", SqlDbType.Int, 30, Convert.ToInt32(dr["ProductID"]));//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "OUTGOING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    // if (ParamReturnInventory.Value.ToString() != "SUCCESS")
                    //{
                    //    Global.m_db.RollBackTransaction();
                    //    throw new Exception("Unable to create sales inventory transaction");
                    //}
                    #endregion
                }

                //First delete the old transaction
                Global.m_db.InsertUpdateQry("DELETE FROM System.tblDueDate WHERE VoucherType='SALES' AND RowID='" + SalesInvoiceID.ToString() + "'");

                int GroupID = AccountGroup.GetGroupIDByLedgerID(CashPartyLedgerID);
                if (GroupID == 29)//Debtor
                {
                    #region For Inserting Due Date
                    //FOR Inserting The Due Date 
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("system.spDueDateCreate");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID.ToString());
                    Global.m_db.AddParameter("@DueDate", SqlDbType.DateTime, DueDate);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, CashPartyLedgerID);//Set same for both for time being
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 30, "SALES");
                    Global.m_db.AddParameter("@VoucherNo", SqlDbType.NVarChar, 20, Voucher_No);
                    System.Data.SqlClient.SqlParameter DueDateInfo = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (DueDateInfo.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create DueDate");
                    }
                }
                    #endregion


                //Log the record into tblauditlog
                bool bAudit = AuditLog.AppendLog(Global.ConcatAllCompInfo, User.CurrentUserName, "SINV", "INSERT", oldgrid + newgrid, SalesInvoiceID, SalesInvoice_Date);


                //Also consider the 13% vat
                if (Global.VAT_Settings == "1")//If vat setting is true then read accoring to settings
                {
                    VatAmt = (NetAmount * (Global.Default_Vat)) / 100;
                }
                else if (Global.VAT_Settings == "0")
                {
                    VatAmt = (NetAmount) / 100;

                }

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

                #region Also insert the transaction in tblTransaction  for Sales

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, SalesLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, salesledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                //Global.m_db.AddParameter("@Amount", SqlDbType.Money, (NetAmount + VatAmt)+TotalTaxAmount);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (Convert.ToDouble(netamt)));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID = Convert.ToInt32(paramReturn1.Value);
                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Invoice");
                }

                //Now add the New editable records for Acc.tblTransactionClass     
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }
                }
                #endregion


                #region Also insert the transaction in tbltransaction for Cash/Party

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, CashPartyLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, cashpartyledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (Convert.ToDouble(netamt) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(VAT)));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID1 = Convert.ToInt32(paramReturn3.Value);

                if (paramReturn3.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Sales Invoice");
                }

                //Now add the New editable records for Acc.tblTransactionClass

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");

                    }
                }
                #endregion


                #region  Transaction for vat  for VAT On Sales
                if (Convert.ToDecimal(VAT) > 0)
                {
                    Global.VATLedgerName = Ledger.GetLedgerNameFromID(Global.VATPayableID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.VATLedgerID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.VATLedgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, VAT);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn4 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID2 = Convert.ToInt32(paramReturn4.Value);
                    if (paramReturn4.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }

                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID2.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Invoice");
                        }
                    } 
                }
                #endregion


                #region Transaction for Tax1 on Sales
                if (Convert.ToDecimal(tax1) > 0)
                {
                    Global.Tax1Name = Ledger.GetLedgerNameFromID(Global.Tax1LedgerID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax1ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax1Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax1Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax1);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn5 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID3 = Convert.ToInt32(paramReturn5.Value);
                    if (paramReturn5.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Invoice");
                        }
                    } 
                }
                #endregion


                #region  Transaction for Tax2 on Sales
                if (Convert.ToDecimal(tax2) > 0)
                {
                    Global.Tax2Name = Ledger.GetLedgerNameFromID(Global.Tax2LedgerID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax2ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax2Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax2Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax2);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID);
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
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Invoice");
                        }
                    } 
                }
                #endregion


                #region Transaction for Tax3 on Sales
                if (Convert.ToDecimal(tax3) > 0)
                {
                    Global.Tax3Name = Ledger.GetLedgerNameFromID(Global.Tax3LedgerID);
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, SalesInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax3ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax3Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax3Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax3);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn7 = Global.m_db.AddOutputParameter("Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    int ReturnTransactID5 = Convert.ToInt32(paramReturn7.Value);
                    if (paramReturn7.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID5.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, SalesInvoiceID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Sales Invoice");
                        }
                    } 
                }
                #endregion

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

                // to modify against references in the voucher
                ModifyReference(SalesInvoiceID, dtReference, RowsToDelete);

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
        //Get the information of SalesReport for Summary All
    
        public DataTable GetAllSalesInfo(DateTime  FromDate, DateTime ToDate,bool IsSalesAccount,int SalesLedgerID)
        {
            string SQL = "";
              //Get the information from  SalesInvoiceMaster and SalesInvoiceDetails according to the TimeRange
             SQL = "Select a.SalesInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount, "+
                "d.EngName SalesAccount,e.EngName Product,b.Quantity,b.SalesRate,b.Amount,b.DiscPercentage, "+
                "b.Discount,b.Net_Amount FROM Inv.tblSalesInvoiceMaster a, Inv.tblSalesInvoiceDetails b, "+
                "Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE a.SalesInvoiceID=b.SalesInvoiceID "+
                "AND a.CashPartyLedgerID=c.LedgerID AND a.SalesLedgerID=d.LedgerID AND b.ProductID = e.ProductID "+
                "AND a.SalesInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' ";
            if (IsSalesAccount == true)
            {
                SQL += "AND a.SalesLedgerID ='" + SalesLedgerID + "'";
            
            }
          
            return Global.m_db.SelectQry(SQL, "table");
        }

        //Get the information of SalesReport for SummaryCredit
        /// <summary>
        /// Collecting only Debtor and Creditor Account Ledgers 
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>

        public DataTable GetSummaryCreditInfo(DateTime FromDate, DateTime ToDate, bool IsSalesAccount, int SalesLedgerID)
        { 
            // Block for Collecting Subgroups under Debtor and Creditor and again corresponding LedgerID under them 
            //Collecting Subgroups of Debtors in arraylist
             int Debtor = AccountGroup.GetGroupIDFromGroupNumber(29);
             DataTable dtDebtor = Ledger.GetAllLedger(Debtor);

             int Creditor = AccountGroup.GetGroupIDFromGroupNumber(114);
             DataTable dtCreditor = Ledger.GetAllLedger(Creditor);
            string LedgerIDList = "";
            for (int i = 0; i <= dtDebtor.Rows.Count-1; i++)
            {
                DataRow drDebtor = dtDebtor.Rows[i];
                if (i == 0)
                    LedgerIDList = "'" + drDebtor["LedgerID"].ToString() + "'";
                else
                    LedgerIDList += "," + "'" + drDebtor["LedgerID"].ToString() + "'";
            }
            for (int j = 0; j <= dtCreditor.Rows.Count - 1; j++)
            {
                DataRow drCreditor = dtCreditor.Rows[j];
       
                    LedgerIDList += "," + "'" + drCreditor["LedgerID"].ToString() + "'";
            }
            string SQL = "Select a.SalesInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount, "+
                            "d.EngName SalesAccount,e.EngName Product,b.Quantity,b.SalesRate,b.Amount, "+
                            "b.DiscPercentage,b.Discount,b.Net_Amount FROM Inv.tblSalesInvoiceMaster a, "+
                            "Inv.tblSalesInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e "+
                            "WHERE a.SalesInvoiceID=b.SalesInvoiceID AND "+
                            "a.CashPartyLedgerID=c.LedgerID AND a.SalesLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND "+
                            "a.SalesInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(FromDate) + "' AND " +
                            "a.CashPartyLedgerID IN(" + LedgerIDList + ")";

            if (IsSalesAccount == true)
            {
                SQL += "AND a.SalesLedgerID ='" + SalesLedgerID + "'";

            }
            return Global.m_db.SelectQry(SQL, "table");        
        }
        //Get the information of SalesReport for SummaryCash
        /// <summary>
        /// Collecting only ledgers of Cash_In_Hand
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>

        public DataTable GetSummaryCashInfo(DateTime FromDate, DateTime ToDate, bool IsSalesAccount, int SalesLedgerID)
        {
            int CashInHand = AccountGroup.GetGroupIDFromGroupNumber(102);
            DataTable dtCash = Ledger.GetAllLedger(CashInHand);
            string CreditLedgerList= "";
            for (int i = 0; i <= dtCash.Rows.Count - 1; i++)
            {
                DataRow drCash = dtCash.Rows[i];             
                if (i == 0)
                    CreditLedgerList = "'" + drCash["LedgerID"].ToString() + "'";
                else
                    CreditLedgerList += "," + "'" + drCash["LedgerID"].ToString() + "'";
            }
            string SQL = "Select a.SalesInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName SalesAccount, "+
                            "e.EngName Product,b.Quantity,b.SalesRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM "+
                            "Inv.tblSalesInvoiceMaster a, Inv.tblSalesInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, "+
                            "Inv.tblProduct e WHERE a.SalesInvoiceID=b.SalesInvoiceID AND a.CashPartyLedgerID=c.LedgerID AND "+
                            "a.SalesLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND a.SalesInvoice_Date "+
                            "BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND a.CashPartyLedgerID IN(" + CreditLedgerList + ")";

            if (IsSalesAccount == true)
            {
                SQL += "AND a.SalesLedgerID ='" + SalesLedgerID + "'";

            }
            return Global.m_db.SelectQry(SQL, "table");
        }

        public DataTable GetVoucherNoSingleInfo(DateTime FromDate, DateTime ToDate, string VoucherNo, bool IsSalesAccount, int SalesLedgerID)
        {
            string SQL = "Select a.SalesInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName SalesAccount, "+
                            "e.EngName Product,b.Quantity,b.SalesRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM  "+
                            "Inv.tblSalesInvoiceMaster a, Inv.tblSalesInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e "+
                            "WHERE a.SalesInvoiceID=b.SalesInvoiceID AND a.CashPartyLedgerID=c.LedgerID AND a.SalesLedgerID=d.LedgerID AND "+
                            "b.ProductID = e.ProductID AND a.SalesInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND  " +
                                 "a.Voucher_No ='" + VoucherNo + "'";

            if (IsSalesAccount == true)
            {
                SQL += "AND a.SalesLedgerID ='" + SalesLedgerID + "'";

            }
            return Global.m_db.SelectQry(SQL, "table");
        }

        //public DataTable GetProductQuantityInfo(DateTime FromDate, DateTime ToDate, int FromQty, int ToQty)
        //{
        //    string SQL = "Select a.SalesInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName SalesAccount,e.EngName Product,b.Quantity,b.Rate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM Inv.tblSalesInvoiceMaster a, Inv.tblSalesInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE a.SalesInvoiceID=b.SalesInvoiceID AND a.CashPartyLedgerID=c.LedgerID AND a.SalesLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND a.SalesInvoice_Date BETWEEN '" + FromDate + "' AND '" + ToDate + "' AND b.Quantity BETWEEN '" + FromQty + "' AND '" + ToQty + "'";
        //    return Global.m_db.SelectQry(SQL, "table");      
        //}

        public DataTable GetSingleProductInfo(DateTime FromDate, DateTime ToDate, int ProductID, bool IsSalesAccount, int SalesLedgerID)
        {
            string SQL = "Select a.SalesInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName SalesAccount, " +
                            "e.EngName Product,b.Quantity,b.SalesRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM Inv.tblSalesInvoiceMaster a, " +
                            "Inv.tblSalesInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE a.SalesInvoiceID=b.SalesInvoiceID AND " +
                            "a.CashPartyLedgerID=c.LedgerID AND a.SalesLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND " +
                            "a.SalesInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND b.ProductID='" + ProductID + "'";

            if (IsSalesAccount == true)
            {
                SQL += "AND a.SalesLedgerID ='" + SalesLedgerID + "'";

            }
            return Global.m_db.SelectQry(SQL, "table");    
        }

        public DataTable GetProductQuantityInfo(DateTime FromDate, DateTime ToDate, int ProductID, int FromQuantity, int ToQuantity, bool IsSalesAccount, int SalesLedgerID)
        {
              string SQL = "Select a.SalesInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName SalesAccount, "+
                            "e.EngName Product,b.Quantity,b.SalesRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM Inv.tblSalesInvoiceMaster a, "+
                            "Inv.tblSalesInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE a.SalesInvoiceID=b.SalesInvoiceID AND "+
                            "a.CashPartyLedgerID=c.LedgerID AND a.SalesLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND "+
                            "a.SalesInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND b.ProductID='" + ProductID + "' AND " +
                            "b.Quantity BETWEEN '" + FromQuantity + "' AND '" + ToQuantity + "'";

              if (IsSalesAccount == true)
              {
                  SQL += "AND a.SalesLedgerID ='" + SalesLedgerID + "'";

              }

            return Global.m_db.SelectQry(SQL, "table");
        }

        public DataTable GetPartySelectedPartyInfo(DateTime FromDate, DateTime ToDate, int PartyID, bool IsSalesAccount, int SalesLedgerID)
        {

            string SQL = "Select a.SalesInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName SalesAccount,e.EngName Product, "+
                            "b.Quantity,b.SalesRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM Inv.tblSalesInvoiceMaster a, "+
                            "Inv.tblSalesInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE a.SalesInvoiceID=b.SalesInvoiceID AND "+
                            "a.CashPartyLedgerID=c.LedgerID AND a.SalesLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND  "+
                            "a.SalesInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND a.CashPartyLedgerID='" + PartyID + "'";

            if (IsSalesAccount == true)
            {
                SQL += "AND a.SalesLedgerID ='" + SalesLedgerID + "'";
            }
            return Global.m_db.SelectQry(SQL, "table");        
        }

        public DataTable GetPartyGroupInfo(DateTime FromDate, DateTime ToDate, int PartyGrpID, bool IsSalesAccount, int SalesLedgerID)
        {
            DataTable dtParty = Ledger.GetAllLedger(PartyGrpID);
            string PartyLedgerList= "";
            for (int i = 0; i <= dtParty.Rows.Count - 1; i++)
            {
                DataRow drParty = dtParty.Rows[i];
                if (i == 0)
                    PartyLedgerList = "'" + drParty["LedgerID"].ToString() + "'";
                else
                    PartyLedgerList += "," + "'" + drParty["LedgerID"].ToString() + "'";
            }
            string SQL = "Select a.SalesInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName SalesAccount, "+
                            "e.EngName Product,b.Quantity,b.SalesRate,b.Amount,b.DiscPercentage, "+
                            "b.Discount,b.Net_Amount FROM Inv.tblSalesInvoiceMaster a, "+
                            "Inv.tblSalesInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE  "+
                            "a.SalesInvoiceID=b.SalesInvoiceID AND a.CashPartyLedgerID=c.LedgerID AND "+
                            "a.SalesLedgerID=d.LedgerID AND "+
                            "b.ProductID = e.ProductID AND a.SalesInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND  " +
                            "a.CashPartyLedgerID IN(" + PartyLedgerList + ")";

            if (IsSalesAccount == true)
            {
                SQL += "AND a.SalesLedgerID ='" + SalesLedgerID + "'";
            }
            return Global.m_db.SelectQry(SQL, "table");
        }

        public DataTable GetSalesInvoiceMasterInfo(string RowID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblSalesInvoiceMaster WHERE SalesInvoiceID ='" + RowID + "'", "table");
        }

        public DataTable GetSalesInvoiceMasterInfoByOrderNo(string OrderNo)
        {
            string strQuery = "SELECT * FROM Inv.tblSalesInvoiceMaster WHERE OrderNo ='"+OrderNo+"'";
            return Global.m_db.SelectQry(strQuery, "table");
        }
 

        public DataTable NavigateSalesInvoiceMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spSalesInvoiceNavigate");
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
            DataTable dtSalesInvoiceMst = Global.m_db.GetDataTable();
            return dtSalesInvoiceMst;
        }

        /// <summary>
        /// Outputs the Journal Detail Information. 
        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public DataTable GetSalesInvoiceDetails(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spGetSalesInvoiceDetail");
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

        public int GetSeriesIDFromMasterID(int MasterID)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Inv.tblSalesInvoiceMaster WHERE SalesInvoiceID ='" + MasterID + "'");
            return Convert.ToInt32(returnID);
        }

        public static DataTable GetSalesReport(int? SalesLedgerID, int? ProductGroupID, int? ProductID, int? PartyGroupID, int? PartyID, int? DepotID, int? ProjectID, DateTime? FromDate, DateTime? ToDate, InventoryReportType SalesReportType, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetSalesReport");
                Global.m_db.AddParameter("@SalesLedgerID", SqlDbType.Int, SalesLedgerID);
                Global.m_db.AddParameter("@ProductGroupID", SqlDbType.Int, ProductGroupID);
                Global.m_db.AddParameter("@ProductID", SqlDbType.Int, ProductID);
                Global.m_db.AddParameter("@PartyGroupID", SqlDbType.Int, PartyGroupID);
                Global.m_db.AddParameter("@PartyID", SqlDbType.Int, PartyID);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@SalesReportTypeIndex", SqlDbType.Int, (int)SalesReportType);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtSalesReportDtl = Global.m_db.GetDataTable();
                return dtSalesReportDtl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                    throw ex;
            }

        }
        public void InsertPOSSalesInvoice(int SeriesID, int SalesID, string SalesAC, int PartyID, string PartyAC, int DepotID, int OrderNo, string VoucherNumber, DateTime POSSalesInvoiceDate, string Remarks, int ProjectID, string CreatedBy, DateTime CreatedDate, int Quantity, double GrossAmount, double Discount, double NetAmount, double tax1amt, double tax2amt, double tax3amt, double vatamt, double TotalAmount, DataTable dtproductdetail, int[] AccClassID)
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
            //double NetAmount = 0;
            //double VatAmt = 0;
            //double GrossAmount = 0;
            double TotalTaxAmount = 0;
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spSalesInvoiceCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@SalesLedgerID", SqlDbType.Int, SalesID);
                Global.m_db.AddParameter("@CashPartyLedgerID", SqlDbType.Int, PartyID);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, VoucherNumber);//Set same for both for time being
                Global.m_db.AddParameter("@SalesInvoice_Date", SqlDbType.DateTime, POSSalesInvoiceDate);
                Global.m_db.AddParameter("@OrderNo", SqlDbType.Int, OrderNo);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Tax1", SqlDbType.Money, tax1amt);
                Global.m_db.AddParameter("@Tax2", SqlDbType.Money, tax2amt);
                Global.m_db.AddParameter("@Tax3", SqlDbType.Money, tax3amt);
                Global.m_db.AddParameter("@VAT", SqlDbType.Money, vatamt);
                Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, NetAmount);
                Global.m_db.AddParameter("@Total_Amount", SqlDbType.Money, TotalAmount);
                Global.m_db.AddParameter("@TotalQty", SqlDbType.Int, Quantity);
                Global.m_db.AddParameter("@Gross_Amount", SqlDbType.Money,GrossAmount);
                Global.m_db.AddParameter("@SpecialDiscount", SqlDbType.Money, Discount);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50,CreatedBy);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int? UpdatedQty = null;
                int ReturnID = Convert.ToInt32(objReturn.Value);
                for (int i = 0; i < dtproductdetail.Rows.Count; i++)
                {
                    DataRow dr = dtproductdetail.Rows[i];
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spSalesInvoiceDetailCreate");
                    Global.m_db.AddParameter("@SalesInvoiceID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["ProductCode"].ToString());
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["ProductName"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Int, Convert.ToInt32(dr["Quantity"]));
                    Global.m_db.AddParameter("@SalesRate", SqlDbType.Money, Convert.ToDouble(dr["SalesRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Total"]));
                    Global.m_db.AddParameter("@DiscPercentage", SqlDbType.Money, 0);
                    Global.m_db.AddParameter("@Discount", SqlDbType.Money, 0);
                    Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, Convert.ToDouble(dr["Total"]));

                    //NetAmount += Convert.ToDouble(dr["NetAmount"]);
                    //GrossAmount += Convert.ToDouble(dr["Amount"]);

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }

                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }

                    #region THIS CODE WILL USE WHEN INVENTORY SYSTEM IS IMPLEMENTED
                    //Addition in Inv.tblInventoryTrans 

                    ////Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, POSSalesInvoiceDate);
                    Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["ProductName"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Int, Convert.ToInt32(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "OUTGOING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    #endregion

                }
               

                //Also insert the transaction in tblTransaction  for Sales
                //sales a/c is who is selling the product to party a/c soo it receives the money soo it is debit
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, POSSalesInvoiceDate);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, SalesLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, SalesAC);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, TotalAmount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID = Convert.ToInt32(paramReturn1.Value);

                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Sales Invoice");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }
                }


                //In case of sales Invoice,Party account is responsible for paying money for Sales a/c soo it pays including Vat soo Cash party and Vat a/c is Credit
                //Also insert the transaction in tbltransaction for Cash/Party
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, POSSalesInvoiceDate);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int,CashPartyLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, PartyAC);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, NetAmount);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID1 = Convert.ToInt32(paramReturn3.Value);
                if (paramReturn3.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Sales Invoice");
                }


                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }
                }

                //Transaction for vat  for VAT On Sales
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, POSSalesInvoiceDate);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.VATLedgerID);
                Global.VATLedgerName = Ledger.GetLedgerNameFromID(Global.VATPayableID);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.VATLedgerName);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, vatamt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn4 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID2 = Convert.ToInt32(paramReturn4.Value);
                if (paramReturn4.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Sales Invoice");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }
                }

                //for tax1
                //Transaction for Tax1
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, POSSalesInvoiceDate);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax1ID);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax1Name);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax1Amt);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax1amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn5 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID3 = Convert.ToInt32(paramReturn5.Value);
                if (paramReturn5.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Sales Invoice");
                }

                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }
                }

                //FOR TAX2
                //Transaction for Tax2
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, POSSalesInvoiceDate);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax2Name);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax2amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn6 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int ReturnTransactID4 = Convert.ToInt32(paramReturn6.Value);
                if (paramReturn6.Value.ToString() == "FAILURE")
                {
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
                    }
                }

                //Transaction for Tax3
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, POSSalesInvoiceDate);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax3ID);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax3Name);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax3Amt);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax3amt);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "SALES");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn7 = Global.m_db.AddOutputParameter("Return", SqlDbType.NVarChar, 20);

                Global.m_db.ProcessParameter();
                int ReturnTransactID5 = Convert.ToInt32(paramReturn7.Value);
                if (paramReturn7.Value.ToString() == "FAILURE")
                {
                    throw new Exception("Unable to create Sales Invoice");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "SALES");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Sales Invoice");
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

        public static DataTable GetProductQtyInStock(int productID,int AccClassID)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "SELECT * FROM Inv.tblOpeningQuantity WHERE ProductID='" + productID + "' and AccClassID='" + AccClassID + "'";
            dt = Global.m_db.SelectQry(strQuery, "tbl");
            return dt;
        }

        public static DataTable GetProductQtyInStockForRpt(int AccClassID,DateTime? createddate)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select p.Created_Date CreatedDate,p.ProductID,p.EngName ProductName,oq.OpenPurchaseQty Quantity,um.UnitName Unit,d.DepotName Depot from " + " "
                        +"Inv.tblProduct p,Inv.tblOpeningQuantity oq,"
                       +" system.tblUnitMaintenance um,inv.tblDepot d where "
                        + "p.ProductID=oq.ProductID and p.DepotID=d.DepotID and p.UnitMaintenanceID=um.UnitMaintenanceID and oq.AccClassID='" + AccClassID + "' and p.created_date<='"+createddate+"'";
            dt = Global.m_db.SelectQry(strQuery, "tbl");
            return dt;
        }
        public static DataTable GetProductQtyInStockForRptWithDepot(int AccClassID, int? ProductID, string Depotname, DateTime? createddate)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select p.Created_Date CreatedDate,p.ProductID,p.EngName ProductName,oq.OpenPurchaseQty Quantity,um.UnitName Unit,d.DepotName Depot from " + " "
                        + "Inv.tblProduct p,Inv.tblOpeningQuantity oq,"
                       + " system.tblUnitMaintenance um,inv.tblDepot d where "
                        + "p.ProductID=oq.ProductID and p.DepotID=d.DepotID and p.UnitMaintenanceID=um.UnitMaintenanceID and oq.AccClassID='" + AccClassID + "' and p.ProductID='" + ProductID + "' and d.DepotName='" + Depotname + "' and p.created_date<='" + createddate + "'";
            dt = Global.m_db.SelectQry(strQuery, "tbl");
            return dt;
        }
        public static DataTable GetProductQtyInStockForRptSingleProduct(int AccClassID, int? ProductID, DateTime? createddate)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select p.Created_Date CreatedDate,p.ProductID,p.EngName ProductName,oq.OpenPurchaseQty Quantity,um.UnitName Unit,d.DepotName Depot from " + " "
                        +"Inv.tblProduct p,Inv.tblOpeningQuantity oq,"
                       +" system.tblUnitMaintenance um,inv.tblDepot d where "
                        + "p.ProductID=oq.ProductID and p.DepotID=d.DepotID and p.UnitMaintenanceID=um.UnitMaintenanceID and oq.AccClassID='" + AccClassID + "' and p.ProductID='" + ProductID + "'  and p.created_date<='" + createddate + "'";
            dt = Global.m_db.SelectQry(strQuery, "tbl");
            return dt;
        }
        public static DataTable GetProductQtyInStockForRptSingleProductWithDepot(int AccClassID, int? ProductID, string Depotname, DateTime? createddate)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select p.Created_Date CreatedDate,p.ProductID,p.EngName ProductName,oq.OpenPurchaseQty Quantity,um.UnitName Unit,d.DepotName Depot from " + " "
                        + "Inv.tblProduct p,Inv.tblOpeningQuantity oq,"
                       + " system.tblUnitMaintenance um,inv.tblDepot d where "
                        + "p.ProductID=oq.ProductID and p.DepotID=d.DepotID and p.UnitMaintenanceID=um.UnitMaintenanceID and oq.AccClassID='" + AccClassID + "' and p.ProductID='" + ProductID + "' and d.DepotName='" + Depotname + "'  and p.created_date<='" + createddate + "'";
            dt = Global.m_db.SelectQry(strQuery, "tbl");
            return dt;
        }
        public static DataTable GetProductQtyInStockForRptProductGroup(int AccClassID, int? ProductGroupID, DateTime? createddate)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select p.Created_Date CreatedDate,p.ProductID,p.EngName ProductName,oq.OpenPurchaseQty Quantity,um.UnitName Unit,d.DepotName Depot from " + " "
                        + "Inv.tblProduct p,Inv.tblOpeningQuantity oq,"
                       + " system.tblUnitMaintenance um,inv.tblDepot d where "
                        + "p.ProductID=oq.ProductID and p.DepotID=d.DepotID and p.UnitMaintenanceID=um.UnitMaintenanceID and oq.AccClassID='" + AccClassID + "' and p.Groupid='" + ProductGroupID + "'  and p.created_date<='" + createddate + "'";
            dt = Global.m_db.SelectQry(strQuery, "tbl");
            return dt;
        }
        public static DataTable GetProductQtyInStockForRptProductGroupWithDepot(int AccClassID, int? ProductID, string Depotname, DateTime? createddate)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select p.Created_Date CreatedDate,p.ProductID,p.EngName ProductName,oq.OpenPurchaseQty Quantity,um.UnitName Unit,d.DepotName Depot from " + " "
                        + "Inv.tblProduct p,Inv.tblOpeningQuantity oq,"
                       + " system.tblUnitMaintenance um,inv.tblDepot d where "
                        + "p.ProductID=oq.ProductID and p.DepotID=d.DepotID and p.UnitMaintenanceID=um.UnitMaintenanceID and oq.AccClassID='" + AccClassID + "' and p.ProductID='" + ProductID + "' and d.DepotName='" + Depotname + "' and p.created_date<='" + createddate + "'";
            dt = Global.m_db.SelectQry(strQuery, "tbl");
            return dt;
        }

        public  DataTable GetSalesInvoiceMasterForDebtors(int MasterID)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select * from Inv.tblSalesInvoiceMaster where SalesInvoiceID='" + MasterID + "'";
            dt = Global.m_db.SelectQry(strQuery, "tblSalesMaster");
            return dt;
        }

        public DataTable GetDueDateForDebtors(int MasterID,string VoucherType)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select * from System.tblDueDate where RowID='" + MasterID + "' and VoucherType='"+VoucherType+"'";
            dt = Global.m_db.SelectQry(strQuery, "tblSalesMaster");
            return dt;
        }

        public bool RemoveSalesEntry(int SalesInvoiceID,string GridValues)
        {
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spRemoveSalesInvoiceEntry");
                Global.m_db.AddParameter("@SalesInvoiceID", SqlDbType.Int, SalesInvoiceID);
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
        public static int GetProductIDFromName(string Name, Lang Language)
        {
            string LangField = "EngName";
            switch (Language)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;
            }


            object objResult = Global.m_db.GetScalarValue("SELECT ProductID FROM Inv.tblProduct WHERE " + LangField + "='" + Name + "'");
            return Convert.ToInt32(objResult);
        }

        public static DataTable GetAdditionalFields(int SeriesID)
        {
            DataTable dt;
            string strQuery = "";
            strQuery = "select * from System.tbloptionalfields where SeriesID='" + SeriesID + "' ";
            dt = Global.m_db.SelectQry(strQuery, "tblSalesMaster");
            return dt;
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
                    VoucherReference.CreateReference(dr, voucherID, "SALES");
                }

                foreach (DataRow dr in dtReference.Select("[RefID] is not null"))
                {
                    VoucherReference.CreateReferenceVoucher(dr, voucherID, "SALES");
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
                if(RowsToDelete.Length > 0)
                    Global.m_db.InsertUpdateQry("delete from System.tblReferenceVoucher where RVID in(" + RowsToDelete.Substring(0, RowsToDelete.Length - 1) + ")");

                if (dtReference.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtReference.Select("[RefID] is null"))
                    {
                        VoucherReference.CreateReference(dr, voucherID, "SALES");
                    }

                    foreach (DataRow dr in dtReference.Select("[RefID] is not null"))
                    {
                        VoucherReference.CreateReferenceVoucher(dr, voucherID, "SALES");
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

        public static DataTable GetTransactInfo()
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("System.spGetTransactInfo");

                return Global.m_db.GetDataTable();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
