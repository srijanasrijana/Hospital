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
    public class Purchase
    {
        public void Create(int SeriesID, int PurchaseLedgerID, string purchaseLedgerName, int CashPartyLedgerID, string cashpartyledgername, int Tax1ID, int Tax2ID, int Tax3ID, int VatID, int DepotID, int? OrderNo, string Voucher_No, DateTime PurchaseInvoice_Date, string Remarks, DataTable PurchaseInvoiceDetails, int[] AccClassID, int ProjectID, double Tax1, string Tax1On, string Tax1OnCheck, double Tax2, string Tax2On, string Tax2OnCheck, double Tax3, string Tax3On, string Tax3OnCheck, string oldgrid, string newgrid, bool isNew, string tax1, string tax2, string tax3, string vat, string netamount, string totalqty, string grossamt, string specialdiscount, string customduty, string freight, string PartyBillNumber, OptionalField of, DataTable dtVoucherRecurring, DataTable dtReference)
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
            if (PurchaseInvoiceDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
                return;
            }
            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();
            ////This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in PurchaseInvoiceDetails.Rows)
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
            double NetAmount = 0;
            double GrossAmount = 0;
            double TotalTaxAmount = 0;
            try
            {
                // Global.m_db.InsertUpdateQry("INSERT INTO Account.tblAccountHead(AccountHeadName,Under_Account_ID,Description,BuiltIn) VALUES ('" + AccountHeadName + "','1','" + Description + "','0')");
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spPurchaseInvoiceCreate");
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@PurchaseLedgerID", SqlDbType.Int, PurchaseLedgerID);
                Global.m_db.AddParameter("@CashPartyLedgerID", SqlDbType.Int, CashPartyLedgerID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, Voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@PurchaseInvoice_Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                Global.m_db.AddParameter("@OrderNo", SqlDbType.Int, OrderNo);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Tax1", SqlDbType.Money, tax1);
                Global.m_db.AddParameter("@Tax2", SqlDbType.Money, tax2);
                Global.m_db.AddParameter("@Tax3", SqlDbType.Money, tax3);
                Global.m_db.AddParameter("@VAT", SqlDbType.Money, vat);
                Global.m_db.AddParameter("@CustomDuty", SqlDbType.Money, customduty);
                Global.m_db.AddParameter("@Freight", SqlDbType.Money, freight);
                Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, netamount);
                Global.m_db.AddParameter("@Total_Amount", SqlDbType.Money, (Convert.ToDouble(netamount) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(vat) + Convert.ToDouble(customduty) + Convert.ToDouble(freight)));
                Global.m_db.AddParameter("@TotalQty", SqlDbType.Float, (totalqty));
                Global.m_db.AddParameter("@Gross_Amount", SqlDbType.Money, grossamt);
                Global.m_db.AddParameter("@SpecialDiscount", SqlDbType.Money, specialdiscount);
                Global.m_db.AddParameter("@Created_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@PartyBillNumber", SqlDbType.NVarChar, 20, PartyBillNumber);
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnID = Convert.ToInt32(objReturn.Value);
                double? Quantity;
                double? UpdatedQty;

                for (int i = 0; i < PurchaseInvoiceDetails.Rows.Count; i++)
                {
                    DataRow dr = PurchaseInvoiceDetails.Rows[i];

                    //Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Quantity", SqlDbType.Int, Convert.ToInt32(dr["Quantity"]));
                    //Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spPurchaseInvoiceDetailCreate");
                    Global.m_db.AddParameter("@PurchaseInvoiceID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.Int, 30, Convert.ToInt32(dr["ProductID"]));//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Quantity = Convert.ToDouble(dr["Quantity"]);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Quantity);
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DiscPercentage", SqlDbType.Money, Convert.ToDouble(dr["DiscPercentage"]));
                    Global.m_db.AddParameter("@Discount", SqlDbType.Money, Convert.ToDouble(dr["Discount"]));
                    Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, Convert.ToDouble(dr["NetAmount"]));
                    Global.m_db.AddParameter("@VAT", SqlDbType.Money, Convert.ToDouble(dr["VAT"]));
                    Global.m_db.AddParameter("@CustomDutyPercentage", SqlDbType.Decimal, Convert.ToDouble(dr["CustomDutyPercentage"]));
                    Global.m_db.AddParameter("@CustomDuty", SqlDbType.Decimal, Convert.ToDouble(dr["CustomDuty"]));
                    Global.m_db.AddParameter("@Freight", SqlDbType.Decimal, Convert.ToDouble(dr["Freight"]));

                    NetAmount += Convert.ToDouble(dr["NetAmount"]);
                    GrossAmount += Convert.ToDouble(dr["Amount"]);

                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }


                    #region This code will use when inventory system is implemented
                    ////Addition in Inv.tblInventoryTrans 

                    //Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                    // Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ProductID", SqlDbType.Int, 30, Convert.ToInt32(dr["ProductID"]));//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "INCOMING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    // if (ParamReturnInventory.Value.ToString() != "SUCCESS")
                    // {
                    //  Global.m_db.RollBackTransaction();
                    //   throw new Exception("Unable to create Purchase inventory transaction");
                    //  }
                    #endregion

                    //Updating PurchaseOrderDetails table
                    if (dr["PurchaseOrderDetailsID"].ToString() != "")//Only when we are going to create invoice according to order then only use this portion
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Inv.spPurchaseOrderDetailModify");
                        Global.m_db.AddParameter("@PurchaseOrderDetailsID", SqlDbType.Int, Convert.ToInt32(dr["PurchaseOrderDetailsID"]));
                        UpdatedQty = Convert.ToDouble(dr["Quantity"]);
                        Global.m_db.AddParameter("@UpdatedQty", SqlDbType.Int, UpdatedQty);
                        System.Data.SqlClient.SqlParameter ParamReturnPurchaseOrderDetails = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                    }

                }
                //Code To Insert AuditLog
                try
                {
                    if (isNew == true)
                    {
                        string username = User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                        string VoucherType = "PINV";
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
                        string VoucherType = "PINV";
                        string action = "UPDATE";
                        int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldgrid + newgrid;
                        //testing
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
                double VatAmt = 0;

                //Considering Vat according to settings
                if (Global.VAT_Settings == "1")//If vat setting is true then read accoring to settings
                {
                    VatAmt = (NetAmount * (Global.Default_Vat)) / 100;
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
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                // Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, PurchaseLedgerID);//Set same for both for time being purchaseLedgerName
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, purchaseLedgerName);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                //(Convert.ToDouble(netamount) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(VatAmt))
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(netamount));
                //Global.m_db.AddParameter("@Amount", SqlDbType.Money, (Convert.ToDouble(netamount) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(vat)));
                //Global.m_db.AddParameter("@Amount", SqlDbType.Money,tax1);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID = Convert.ToInt32(paramReturn1.Value);
                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Invoice");
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
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                }
                //Also insert the transaction in tbltransaction for Cash/Party
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, CashPartyLedgerID);//Set same for both for time being cashpartyledgername
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, cashpartyledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(Convert.ToDouble(netamount) + Convert.ToDouble(vat) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(customduty) + Convert.ToDouble(freight)));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID1 = Convert.ToInt32(paramReturn3.Value);
                if (paramReturn3.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Invoice");
                }
                //Now add the New editable records for Acc.tblTransactionClass
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                }

                //Transaction for vat  for VAT On Purchase

                System.Data.SqlClient.SqlParameter paramReturn4 = new System.Data.SqlClient.SqlParameter();
                if (Convert.ToDecimal(vat) > 0)
                {

                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, Date.ToDB(PurchaseInvoice_Date));
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.VATLedgerID);
                    // Global.VATLedgerName = Ledger.GetLedgerNameFromID(Global.VATReceivableID);
                    Global.m_db.AddParameter("@LedgerID", SqlDbType.NVarChar, 30, Global.VATReceivableID);

                    //  Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, "Vat Receivable");
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(vat));
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    paramReturn4 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramReturn4.Value.ToString() == "FAILURE" || paramReturn4.Value is DBNull)
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }


                    //Now add the New editable records for Acc.tblTransactionClas
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, paramReturn4.Value);
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }
                    }
                }

                //Transaction for Tax1
                if (Convert.ToDecimal(tax1) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax1ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax1Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax1Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax1);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn5 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID3 = Convert.ToInt32(paramReturn5.Value);
                    if (paramReturn5.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
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
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }
                    }


                }
                //Transaction for Tax2
                if (Convert.ToDecimal(tax2) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                    // Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax2ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax2Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    // Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax2Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax2);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn6 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    int ReturnTransactID4 = Convert.ToInt32(paramReturn6.Value);
                    if (paramReturn6.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
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
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }
                    }
                }

                //Transaction for Tax3
                if (Convert.ToDecimal(tax3) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax3ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.Tax3Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax3Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax3);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn7 = Global.m_db.AddOutputParameter("Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    int ReturnTransactID5 = Convert.ToInt32(paramReturn7.Value);
                    if (paramReturn7.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
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
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }

                    }

                }
                //Transaction for Custom Duty for CustomDuty On Purchase
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, "Custom Duty");
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, customduty);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                System.Data.SqlClient.SqlParameter paramReturn8 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID8 = Convert.ToInt32(paramReturn8.Value);
                if (paramReturn8.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Invoice");
                }
                //Now add the New editable records for Acc.tblTransactionClas
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID8.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                }

                //Transaction for Freight for Freight On Purchase
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, "Freight");
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, freight);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID);
                System.Data.SqlClient.SqlParameter paramReturn9 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID9 = Convert.ToInt32(paramReturn9.Value);
                if (paramReturn9.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Invoice");
                }
                //Now add the New editable records for Acc.tblTransactionClas
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID9.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, ReturnID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                }

                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherID"] = ReturnID;
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "PURCHASE_INVOICE";
                    string res = RecurringVoucher.CreateRecurringVoucherSetting(dtVoucherRecurring);
                    if (res == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice due to recurring settings.");
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

        System.Text.Encoding commonAEncoder;
        System.IO.MemoryStream commonms;
        System.Xml.XmlTextWriter commontw;
        public void Create1(int SeriesID, int PurchaseLedgerID, string purchaseLedgerName, int CashPartyLedgerID, string cashpartyledgername, int Tax1ID, int Tax2ID, int Tax3ID, int VatID, int DepotID, int? OrderNo, string Voucher_No, DateTime PurchaseInvoice_Date, string Remarks, DataTable PurchaseInvoiceDetails, int[] AccClassID, int ProjectID, double Tax1, string Tax1On, string Tax1OnCheck, double Tax2, string Tax2On, string Tax2OnCheck, double Tax3, string Tax3On, string Tax3OnCheck, string oldgrid, string newgrid, bool isNew, string tax1, string tax2, string tax3, string vat, string netamount, string totalqty, string grossamt, string specialdiscount, string customduty, string freight, string PartyBillNumber, OptionalField of, DataTable dtVoucherRecurring, DataTable dtReference)
        {
            string PurchaseXML = " ";
            string CommonXML = " ";

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
            if (PurchaseInvoiceDetails.Rows.Count == 0)
            {
                throw new Exception("Please fill the ledger details");
            }
            ArrayList Debit = new ArrayList();
            ArrayList Credit = new ArrayList();
            ////This loop is to check whether ledger names are correct and properly implemented
            foreach (DataRow row in PurchaseInvoiceDetails.Rows)
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
                {
                    throw new Exception("Product Name - " + row["Product"].ToString() + " not found!");
                }
            }

            double NetAmount = 0;
            double GrossAmount = 0;
            double TotalTaxAmount = 0;

            try
            {
                //write xml for insertion
                #region write xml for purchase invoice

                System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

                tw.WriteStartDocument();
                tw.WriteStartElement("PURCHASEINVOICE");
                {
                    tw.WriteStartElement("PURCHASEINVOICEMASTER");
                    tw.WriteElementString("SeriesID", SeriesID.ToString());
                    tw.WriteElementString("PurchaseLedgerID", PurchaseLedgerID.ToString());
                    tw.WriteElementString("CashPartyLedgerID", CashPartyLedgerID.ToString());
                    tw.WriteElementString("ProjectID", ProjectID.ToString());
                    tw.WriteElementString("DepotID", DepotID.ToString());
                    tw.WriteElementString("OrderNo", OrderNo.ToString());
                    tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                    tw.WriteElementString("PurchaseInvoice_Date", Date.ToDB(PurchaseInvoice_Date));
                    tw.WriteElementString("Remarks", Remarks.ToString());
                    tw.WriteElementString("Created_By", User.CurrUserID.ToString());
                    tw.WriteElementString("Created_Date", Date.ToDB(DateTime.Today));
                    tw.WriteElementString("Net_Amount", netamount.ToString());
                    tw.WriteElementString("Tax1", tax1.ToString());
                    tw.WriteElementString("Tax2", tax2.ToString());
                    tw.WriteElementString("Tax3", tax3.ToString());
                    tw.WriteElementString("VAT", vat.ToString());
                    tw.WriteElementString("Total_Amount", (Convert.ToDouble(netamount) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(vat) + Convert.ToDouble(customduty) + Convert.ToDouble(freight)).ToString());
                    tw.WriteElementString("Gross_Amount", grossamt.ToString());
                    tw.WriteElementString("SpecialDiscount", specialdiscount.ToString());
                    tw.WriteElementString("TotalQty", totalqty.ToString());
                    tw.WriteElementString("CustomDuty", customduty.ToString());
                    tw.WriteElementString("Freight", freight.ToString());
                    tw.WriteElementString("PartyBillNumber", PartyBillNumber.ToString());
                    tw.WriteElementString("Field1", of.First.ToString());
                    tw.WriteElementString("Field2", of.Second.ToString());
                    tw.WriteElementString("Field3", of.Third.ToString());
                    tw.WriteElementString("Field4", of.Fourth.ToString());
                    tw.WriteElementString("Field5", of.Fifth.ToString());
                    tw.WriteEndElement();
                }

                double? Quantity;
               // double? UpdatedQty;

                tw.WriteStartElement("PURCHASEINVOICEDETAIL");

                for (int i = 0; i < PurchaseInvoiceDetails.Rows.Count; i++)
                {
                    DataRow dr = PurchaseInvoiceDetails.Rows[i];

                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("Code", dr["Code"].ToString());
                    tw.WriteElementString("ProductID", dr["ProductID"].ToString());
                    tw.WriteElementString("ProductName", dr["Product"].ToString());
                    Quantity = Convert.ToDouble(dr["Quantity"]);
                    tw.WriteElementString("Quantity", Quantity.ToString());
                    tw.WriteElementString("PurchaseRate", Convert.ToDouble(dr["PurchaseRate"]).ToString());
                    tw.WriteElementString("Amount", Convert.ToDouble(dr["Amount"]).ToString());
                    tw.WriteElementString("DiscPercentage", Convert.ToDouble(dr["DiscPercentage"]).ToString());
                    tw.WriteElementString("Discount", Convert.ToDouble(dr["Discount"]).ToString());
                    tw.WriteElementString("Net_Amount", Convert.ToDouble(dr["NetAmount"]).ToString());
                    tw.WriteElementString("VAT", Convert.ToDouble(dr["VAT"]).ToString());
                    tw.WriteElementString("CustomDutyPercentage", Convert.ToDouble(dr["CustomDutyPercentage"]).ToString());
                    tw.WriteElementString("CustomDuty", Convert.ToDouble(dr["CustomDuty"]).ToString());
                    tw.WriteElementString("Freight", Convert.ToDouble(dr["Freight"]).ToString());
                    tw.WriteElementString("QtyUnitID", Convert.ToDouble(dr["QtyUnitID"]).ToString());
                    tw.WriteEndElement();

                    NetAmount += Convert.ToDouble(dr["NetAmount"]);
                    GrossAmount += Convert.ToDouble(dr["Amount"]);
                }
                tw.WriteEndElement();
                tw.WriteFullEndElement();
                tw.WriteEndDocument();
                tw.Flush();
                tw.Close();
                PurchaseXML = AEncoder.GetString(ms.ToArray());

                #endregion

                #region write common xml

                commonAEncoder = System.Text.Encoding.Unicode;
                commonms = new System.IO.MemoryStream();
                commontw = new System.Xml.XmlTextWriter(commonms, commonAEncoder);
                commontw.WriteStartDocument();
                commontw.WriteStartElement("COMMON");

                try
                {
                    if (isNew == true)
                    {
                        string username = " ";
                          username=  User.CurrentUserName;
                        string voucherdate = Date.ToDB(DateTime.Now).ToString();
                       // string VoucherType = "PINV";
                        string action = "INSERT";
                       // int rowid = 0;
                        string ComputerName = Global.ComputerName;
                        string MacAddress = Global.MacAddess;
                        string IpAddress = Global.IpAddress;
                        string desc = oldgrid + newgrid;

                        commontw.WriteStartElement("AUDITDETAIL");
                        commontw.WriteElementString("ComputerName", ComputerName.ToString());
                        commontw.WriteElementString("UserName", username.ToString());
                        commontw.WriteElementString("action", action.ToString());
                        commontw.WriteElementString("Description", desc.ToString());
                        commontw.WriteElementString("MAC_Address", MacAddress.ToString());
                        commontw.WriteElementString("IP_Address", IpAddress.ToString());
                        commontw.WriteElementString("VoucherDate", voucherdate.ToString());

                        commontw.WriteEndElement();
                        //Global.m_db.ClearParameter();
                        //Global.m_db.setCommandType(CommandType.StoredProcedure);
                        //Global.m_db.setCommandText("system.spAddAuditLog");
                        //Global.m_db.AddParameter("@ComputerName", SqlDbType.NVarChar, 50, ComputerName);
                        //Global.m_db.AddParameter("@UserName", SqlDbType.NVarChar, 50, username);//Set same for both for time being
                        //Global.m_db.AddParameter("@Action", SqlDbType.NVarChar, 50, action);
                        //Global.m_db.AddParameter("@Description", SqlDbType.NVarChar, 4000, desc);
                        //Global.m_db.AddParameter("@RowID", SqlDbType.Int, rowid);
                        //Global.m_db.AddParameter("@MAC_Address", SqlDbType.NVarChar, 50, MacAddress);
                        //Global.m_db.AddParameter("@IP_Address", SqlDbType.NVarChar, 50, IpAddress);
                        //Global.m_db.AddParameter("@Voucher_Type", SqlDbType.NVarChar, 50, VoucherType);
                        //Global.m_db.AddParameter("@VoucherDate", SqlDbType.NVarChar, 200, voucherdate);
                        //object objReturn1 = Global.m_db.AddOutputParameter("@Result", SqlDbType.NVarChar, 20);
                        //Global.m_db.ProcessParameter();

                    }

                }
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
                }
                double VatAmt = 0;

                //Considering Vat according to settings
                if (Global.VAT_Settings == "1")//If vat setting is true then read accoring to settings
                {
                    VatAmt = (NetAmount * (Global.Default_Vat)) / 100;
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
                commontw.WriteStartElement("LEDGERDETAIL");
                //write xml for purchase ledgerID
                WriteCommonLedgerXml(PurchaseLedgerID, purchaseLedgerName, Convert.ToDouble(netamount), 0, PurchaseInvoice_Date, ProjectID);
                //write xml for purchase cashparty ledgerid
                WriteCommonLedgerXml(CashPartyLedgerID, cashpartyledgername, 0, Convert.ToDouble((Convert.ToDouble(netamount) + Convert.ToDouble(vat) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(customduty) + Convert.ToDouble(freight))), PurchaseInvoice_Date, ProjectID);
                // System.Data.SqlClient.SqlParameter paramReturn4 = new System.Data.SqlClient.SqlParameter();
                if (Convert.ToDecimal(vat) > 0)
                {
                    //write xml for purchase vatreceivable id
                    WriteCommonLedgerXml(Global.VATReceivableID, Global.VATReceivableLdg, Convert.ToDouble(vat), 0, PurchaseInvoice_Date, ProjectID);
                }
                //Transaction for Tax1
                if (Convert.ToDecimal(tax1) > 0)
                {
                    //write xml for purchase tax1 id
                    WriteCommonLedgerXml(Tax1ID, Global.PurchaseTax1Name, Convert.ToDouble(tax1), 0, PurchaseInvoice_Date, ProjectID);
                }
                //Transaction for Tax2
                if (Convert.ToDecimal(tax2) > 0)
                {
                    //write xml for purchase tax2 id
                    WriteCommonLedgerXml(Tax2ID, Global.PurchaseTax2Name, Convert.ToDouble(tax2), 0, PurchaseInvoice_Date, ProjectID);
                }
                //Transaction for Tax3
                if (Convert.ToDecimal(tax3) > 0)
                {
                    //write xml for purchase tax2 id
                    WriteCommonLedgerXml(Tax3ID, Global.PurchaseTax3Name, Convert.ToDouble(tax3), 0, PurchaseInvoice_Date, ProjectID);
                }
                //write xml for purchase custom duty id
                WriteCommonLedgerXml(504, "Custom Duty", Convert.ToDouble(customduty), 0, PurchaseInvoice_Date, ProjectID);
                //write xml for Freight id
                WriteCommonLedgerXml(503, "Freight", Convert.ToDouble(freight), 0, PurchaseInvoice_Date, ProjectID);
                commontw.WriteEndElement();

               commontw.WriteStartElement("AccClassIDSettings");

                foreach (int _AccClassID in AccClassID)
                {
                    commontw.WriteElementString("AccClassID", _AccClassID.ToString());
                }
                commontw.WriteEndElement();

                //write xml for recurring voucher
                commontw.WriteStartElement("RECURRINGVOUCHER");

                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVoucherRecurring.Rows)
                    {
                        commontw.WriteStartElement("DETAIL");
                        commontw.WriteElementString("Description", dtVoucherRecurring.Rows[0]["Description"].ToString());
                        commontw.WriteElementString("RecurringType", dtVoucherRecurring.Rows[0]["RecurringType"].ToString());
                        commontw.WriteElementString("Unit1", dtVoucherRecurring.Rows[0]["Unit1"].ToString());
                        commontw.WriteElementString("Unit2", dtVoucherRecurring.Rows[0]["Unit2"].ToString());
                        commontw.WriteElementString("Date", Date.ToDB(Convert.ToDateTime(dtVoucherRecurring.Rows[0]["Date"].ToString())));

                        commontw.WriteEndElement();
                    }
                }
                commontw.WriteEndElement();
                commontw.WriteFullEndElement();
                commontw.WriteEndDocument();
                commontw.Flush();
                commontw.Close();

                    CommonXML= commonAEncoder.GetString(commonms.ToArray());
                #endregion

                    #region call store procedure
               //     Global.m_db.BeginTransaction();
                 Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Inv.xmlPurchaseInvoiceInsert2");
                        Global.m_db.AddParameter("@specific", SqlDbType.Xml,PurchaseXML);
                        Global.m_db.AddParameter("@common", SqlDbType.Xml,CommonXML);//Set same for both for time being
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@lastReturn", SqlDbType.NVarChar, 20);
                 Global.m_db.ProcessParameter();
                string  Returnstr= objReturn.Value.ToString();
                if(Returnstr=="FAILURE")
                {
                    //throw new Exception();
                }
               // Global.m_db.CommitTransaction();
                    #endregion
                    // to send reference information
                // InsertReference(ReturnID, dtReference);
            }
            catch (Exception ex)
            {
              //  Global.m_db.RollBackTransaction();

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


        private void WriteCommonLedgerXml(int LedgerID, string LedgerName, double DebitAmount, double CreditAmount, DateTime Date, int ProjectID)
        {
            commontw.WriteStartElement("DETAIL");
            commontw.WriteElementString("LedgerID", LedgerID.ToString());
            commontw.WriteElementString("LedgerName", LedgerName.ToString());
            commontw.WriteElementString("DebitAmount", DebitAmount.ToString());
            commontw.WriteElementString("CreditAmount", CreditAmount.ToString());
            commontw.WriteElementString("TransactDate", Date.ToString());
            commontw.WriteElementString("ProjectID", ProjectID.ToString());
            commontw.WriteEndElement();

        }

        public void Modify(int PurchaseInvoiceID, int SeriesID, int PurchaseLedgerID, string purchaseledgername, int CashPartyLedgerID, string cashpurchaseledgername, int Tax1ID, int Tax2ID, int Tax3ID, int VatID, int DepotID, string Order_No, string Voucher_No, DateTime PurchaseInvoice_Date, string Remarks, DataTable PurchaseInvoiceDetails, int[] AccClassID, int ProjectID, double Tax1, string Tax1On, string Tax1OnCheck, double Tax2, string Tax2On, string Tax2OnCheck, double Tax3, string Tax3On, string Tax3OnCheck, string oldgrid, string newgrid, bool isNew, string tax1, string tax2, string tax3, string vat, string netamount, string totalqty, string grossamt, string specialdiscount, string CustomDuty, string Freight, string PartyBillNumber, OptionalField of, DataTable dtVoucherRecurring, DataTable dtReference, string ToDeleteRows)
        {
            double NetAmount = 0;
            double GrossAmount = 0;
            double TotalTaxAmount = 0;
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spPurchaseInvoiceModify");
                Global.m_db.AddParameter("PurchaseInvoiceID", SqlDbType.Int, PurchaseInvoiceID);
                Global.m_db.AddParameter("@SeriesID", SqlDbType.Int, SeriesID);
                Global.m_db.AddParameter("@PurchaseLedgerID", SqlDbType.Int, PurchaseLedgerID);
                Global.m_db.AddParameter("@CashPartyLedgerID", SqlDbType.Int, CashPartyLedgerID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@Voucher_No", SqlDbType.NVarChar, 30, Voucher_No);//Set same for both for time being
                Global.m_db.AddParameter("@PurchaseInvoice_Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                Global.m_db.AddParameter("@Order_No", SqlDbType.NVarChar, Order_No);
                Global.m_db.AddParameter("@Remarks", SqlDbType.NVarChar, 200, Remarks);
                Global.m_db.AddParameter("@Tax1", SqlDbType.Money, tax1);
                Global.m_db.AddParameter("@Tax2", SqlDbType.Money, tax2);
                Global.m_db.AddParameter("@Tax3", SqlDbType.Money, tax3);
                Global.m_db.AddParameter("@VAT", SqlDbType.Money, vat);
                Global.m_db.AddParameter("@CustomDuty", SqlDbType.Money, CustomDuty);
                Global.m_db.AddParameter("@Freight", SqlDbType.Money, Freight);
                Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, netamount);
                Global.m_db.AddParameter("@Total_Amount", SqlDbType.Money, (Convert.ToDouble(netamount) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(vat) + Convert.ToDouble(CustomDuty) + Convert.ToDouble(Freight)));
                Global.m_db.AddParameter("@TotalQty", SqlDbType.Float, totalqty);
                Global.m_db.AddParameter("@Gross_Amount", SqlDbType.Money, grossamt);
                Global.m_db.AddParameter("@SpecialDiscount", SqlDbType.Money, specialdiscount);
                Global.m_db.AddParameter("@Modified_By", SqlDbType.NVarChar, 50, User.CurrUserID.ToString());
                Global.m_db.AddParameter("@PartyBillNumber", SqlDbType.NVarChar, 20, PartyBillNumber);
                Global.m_db.AddParameter("@First", SqlDbType.NVarChar, 50, of.First);
                Global.m_db.AddParameter("@Second", SqlDbType.NVarChar, 50, of.Second);
                Global.m_db.AddParameter("@Third", SqlDbType.NVarChar, 50, of.Third);
                Global.m_db.AddParameter("@Fourth", SqlDbType.NVarChar, 50, of.Fourth);
                Global.m_db.AddParameter("@Fifth", SqlDbType.NVarChar, 50, of.Fifth);
                System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                //int ReturnID = Convert.ToInt32(objReturn.Value);
                //First delete the old record
                Global.m_db.InsertUpdateQry("DELETE FROM Inv.tblPurchaseInvoiceDetails WHERE PurchaseInvoiceID='" + PurchaseInvoiceID.ToString() + "'");
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
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransaction WHERE VoucherType='PURCH' AND RowID='" + PurchaseInvoiceID.ToString() + "'");

                //First delete the previous records of Acc.tblTransactionClass according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM Acc.tblTransactionClass WHERE VoucherType='PURCH' AND RowID='" + PurchaseInvoiceID.ToString() + "'");

                //First delete the old transaction from table inventorytransaction according to VoucherType and RowID
                Global.m_db.InsertUpdateQry("DELETE FROM inv.tblInventoryTrans WHERE VoucherType='PURCH' AND RowID='" + PurchaseInvoiceID.ToString() + "'");

                double? Quantity;
                double? UpdatedQty;

                for (int i = 0; i < PurchaseInvoiceDetails.Rows.Count; i++)
                {
                    DataRow dr = PurchaseInvoiceDetails.Rows[i];

                    //Now go for the Detail Inserts
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spPurchaseInvoiceDetailCreate");
                    Global.m_db.AddParameter("@PurchaseInvoiceID", SqlDbType.Int, PurchaseInvoiceID.ToString());
                    Global.m_db.AddParameter("@Code", SqlDbType.NVarChar, 30, dr["Code"].ToString());
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@PurchaseRate", SqlDbType.Money, Convert.ToDouble(dr["PurchaseRate"]));
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(dr["Amount"]));
                    Global.m_db.AddParameter("@DiscPercentage", SqlDbType.Money, Convert.ToDouble(dr["DiscPercentage"]));
                    Global.m_db.AddParameter("@Discount", SqlDbType.Money, Convert.ToDouble(dr["Discount"]));
                    Global.m_db.AddParameter("@Net_Amount", SqlDbType.Money, Convert.ToDouble(dr["NetAmount"]));
                    Global.m_db.AddParameter("@VAT", SqlDbType.Money, Convert.ToDouble(dr["VAT"]));
                    Global.m_db.AddParameter("@CustomDutyPercentage", SqlDbType.Decimal, Convert.ToDouble(dr["CustomDutyPercentage"]));
                    Global.m_db.AddParameter("@CustomDuty", SqlDbType.Decimal, Convert.ToDouble(dr["CustomDuty"]));
                    Global.m_db.AddParameter("@Freight", SqlDbType.Decimal, Convert.ToDouble(dr["Freight"]));
                    Global.m_db.AddParameter("@QtyunitID", SqlDbType.Decimal, Convert.ToDouble(dr["QtyunitID"]));

                    NetAmount += Convert.ToDouble(dr["NetAmount"]);
                    GrossAmount += Convert.ToDouble(dr["Amount"]);
                    System.Data.SqlClient.SqlParameter paramReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                    if (paramReturn.Value.ToString() != "SUCCESS")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }

                    #region This code will use when inventory system is implemented
                    ////Also insert the transaction in Inv.tblInventoryTrnas  
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Inv.spInventoryTransCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                    // Global.m_db.AddParameter("@ProductName", SqlDbType.NVarChar, 30, dr["Product"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@ProductID", SqlDbType.NVarChar, 30, dr["ProductID"].ToString());//Set same for both for time being
                    Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Quantity", SqlDbType.Float, Convert.ToDouble(dr["Quantity"]));
                    Global.m_db.AddParameter("@InOut", SqlDbType.NVarChar, 10, "INCOMING");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID);
                    System.Data.SqlClient.SqlParameter ParamReturnInventory = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    //if (ParamReturnInventory.Value.ToString() != "SUCCESS")
                    //{
                    //    Global.m_db.RollBackTransaction();
                    //    throw new Exception("Unable to create Purchase inventory transaction");
                    //}
                    #endregion

                    //Updating PurchaseOrderDetails table
                    if (dr["PurchaseOrderDetailsID"].ToString() != "")//Only when we are going to create invoice according to order then only use this portion
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Inv.spPurchaseOrderDetailModify");
                        Global.m_db.AddParameter("@PurchaseOrderDetailsID", SqlDbType.Int, Convert.ToInt32(dr["PurchaseOrderDetailsID"]));
                        UpdatedQty = Convert.ToDouble(dr["Quantity"]);
                        Global.m_db.AddParameter("@UpdatedQty", SqlDbType.Int, UpdatedQty);
                        System.Data.SqlClient.SqlParameter ParamReturnPurchaseOrderDetails = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();
                    }
                }
                double VatAmt = 0;

                //Log the record into tblauditlog
                bool bAudit = AuditLog.AppendLog(Global.ConcatAllCompInfo, User.CurrentUserName, "PINV", "INSERT", oldgrid + newgrid, PurchaseInvoiceID, PurchaseInvoice_Date);
                //Considering Vat according to settings
                if (Global.VAT_Settings == "1")//If vat setting is true then read accoring to settings
                {
                    VatAmt = (NetAmount * (Global.Default_Vat)) / 100;
                }
                else if (Global.VAT_Settings == "0")
                {
                    VatAmt = 0;
                }

                //double m_Tax1Amt = Slabs.CalculateSlabsDetails(SlabType.TAX1, SalesPurchaseType.PURCHASE, (NetAmount + VatAmt), GrossAmount);
                //double m_Tax2Amt = Slabs.CalculateSlabsDetails(SlabType.TAX2, SalesPurchaseType.PURCHASE, (NetAmount + VatAmt), GrossAmount);
                //double m_Tax3Amt = Slabs.CalculateSlabsDetails(SlabType.TAX3, SalesPurchaseType.PURCHASE, (NetAmount + VatAmt), GrossAmount);
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
                TotalTaxAmount = Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3);
                //Also insert the transaction in tblTransaction  for Purchase
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, PurchaseLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, purchaseledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                // Global.m_db.AddParameter("@Amount", SqlDbType.Money, (NetAmount + VatAmt));
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, (Convert.ToDouble(netamount)));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                System.Data.SqlClient.SqlParameter paramReturn1 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                int ReturnTransactID = Convert.ToInt32(paramReturn1.Value);
                if (paramReturn1.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Invoice");
                }
                //Now add the New editable records for Acc.tblTransactionClass
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                }
                //Also insert the transaction in tbltransaction for Cash/Party
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, CashPartyLedgerID);//Set same for both for time being
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, cashpurchaseledgername);
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Convert.ToDouble(Convert.ToDouble(netamount) + Convert.ToDouble(vat) + Convert.ToDouble(tax1) + Convert.ToDouble(tax2) + Convert.ToDouble(tax3) + Convert.ToDouble(CustomDuty) + Convert.ToDouble(Freight)));
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "CREDIT");
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID);
                System.Data.SqlClient.SqlParameter paramReturn3 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID1 = Convert.ToInt32(paramReturn3.Value);

                if (paramReturn3.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Invoice");
                }

                //Now add the New editable records for Acc.tblTransactionClass
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID1.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                }
                //Transaction for vat  for VAT On Purchase

                if (Convert.ToDecimal(vat) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Global.VATLedgerID);
                    //  Global.VATLedgerName = Ledger.GetLedgerNameFromID(Global.VATReceivableID);

                    Global.m_db.AddParameter("@LedgerID", SqlDbType.NVarChar, 30, Global.VATReceivableID);
                    //  Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.VATLedgerName);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, vat);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID);
                    System.Data.SqlClient.SqlParameter paramReturn4 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();

                    int ReturnTransactID2 = Convert.ToInt32(paramReturn4.Value);

                    if (paramReturn4.Value.ToString() == "FAILURE" || paramReturn4.Value is DBNull)
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }

                    //Now add the New editable records for Acc.tblTransactionClas
                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID2.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }

                    }
                }

                //Transaction for Tax1
                if (Convert.ToDecimal(tax1) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax1ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.PurchaseTax1Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax1Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax1);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn5 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    int ReturnTransactID3 = Convert.ToInt32(paramReturn5.Value);
                    if (paramReturn5.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID3.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }
                    }


                }
                //Transaction for Tax2
                if (Convert.ToDecimal(tax2) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax2ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.PurchaseTax2Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    // Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax2Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax2);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn6 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    int ReturnTransactID4 = Convert.ToInt32(paramReturn6.Value);
                    if (paramReturn6.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID4.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }
                    }

                }
                //Transaction for Tax3
                if (Convert.ToDecimal(tax3) > 0)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactCreate");
                    Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                    //Global.m_db.AddParameter("@LedgerID", SqlDbType.Int, Tax3ID);
                    Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, Global.PurchaseTax3Name);
                    Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                    //Global.m_db.AddParameter("@Amount", SqlDbType.Money, m_Tax3Amt);
                    Global.m_db.AddParameter("@Amount", SqlDbType.Money, tax3);
                    Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                    Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID);
                    Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                    System.Data.SqlClient.SqlParameter paramReturn7 = Global.m_db.AddOutputParameter("Return", SqlDbType.NVarChar, 20);

                    Global.m_db.ProcessParameter();
                    int ReturnTransactID5 = Convert.ToInt32(paramReturn7.Value);
                    if (paramReturn7.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                    //Now add the New editable records for Acc.tblTransactionClass

                    foreach (int _AccClassID in AccClassID)
                    {
                        Global.m_db.ClearParameter();
                        Global.m_db.setCommandType(CommandType.StoredProcedure);
                        Global.m_db.setCommandText("Acc.spTransactClassCreate");
                        Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID5.ToString());
                        Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                        Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID.ToString());
                        Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                        System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        Global.m_db.ProcessParameter();

                        if (paramTransactClassID.Value.ToString() == "FAILURE")
                        {
                            Global.m_db.RollBackTransaction();
                            throw new Exception("Unable to create Purchase Invoice");
                        }

                    }
                }

                //Transaction for Custom Duty for CustomDuty On Purchase
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, "Custom Duty");
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, CustomDuty);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID);
                System.Data.SqlClient.SqlParameter paramReturn8 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID8 = Convert.ToInt32(paramReturn8.Value);
                if (paramReturn8.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Invoice");
                }
                //Now add the New editable records for Acc.tblTransactionClas
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID8.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                }

                //Transaction for Freight for Freight On Purchase
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spTransactCreate");
                Global.m_db.AddParameter("@Date", SqlDbType.DateTime, PurchaseInvoice_Date);
                Global.m_db.AddParameter("@LedgerName", SqlDbType.NVarChar, 30, "Freight");
                Global.m_db.AddParameter("@Language", SqlDbType.NVarChar, 30, Language);
                Global.m_db.AddParameter("@Amount", SqlDbType.Money, Freight);
                Global.m_db.AddParameter("@DrCr", SqlDbType.NVarChar, 10, "DEBIT");
                Global.m_db.AddParameter("@VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID);
                System.Data.SqlClient.SqlParameter paramReturn9 = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();
                int ReturnTransactID9 = Convert.ToInt32(paramReturn9.Value);
                if (paramReturn9.Value.ToString() == "FAILURE")
                {
                    Global.m_db.RollBackTransaction();
                    throw new Exception("Unable to create Purchase Invoice");
                }
                //Now add the New editable records for Acc.tblTransactionClas
                foreach (int _AccClassID in AccClassID)
                {
                    Global.m_db.ClearParameter();
                    Global.m_db.setCommandType(CommandType.StoredProcedure);
                    Global.m_db.setCommandText("Acc.spTransactClassCreate");
                    Global.m_db.AddParameter("@TransactID", SqlDbType.Int, ReturnTransactID9.ToString());
                    Global.m_db.AddParameter("@AccClassID", SqlDbType.Int, _AccClassID);//Set same for both for time being
                    Global.m_db.AddParameter("@RowID", SqlDbType.Int, PurchaseInvoiceID.ToString());
                    Global.m_db.AddParameter("VoucherType", SqlDbType.NVarChar, 20, "PURCH");
                    System.Data.SqlClient.SqlParameter paramTransactClassID = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                    Global.m_db.ProcessParameter();
                    if (paramTransactClassID.Value.ToString() == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to create Purchase Invoice");
                    }
                }
                #region Save voucher recurring settings
                if (dtVoucherRecurring.Rows.Count > 0)
                {
                    dtVoucherRecurring.Rows[0]["VoucherType"] = "PURCHASE_INVOICE";
                    string result = RecurringVoucher.ModifyRecurringVoucherSetting(dtVoucherRecurring);

                    if (result == "FAILURE")
                    {
                        Global.m_db.RollBackTransaction();
                        throw new Exception("Unable to modify Purchase Invoice due to recurring settings.");
                    }
                }
                #endregion

                // to modify against references in the voucher
                ModifyReference(PurchaseInvoiceID, dtReference, ToDeleteRows);
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

        public int GetSeriesIDFromMasterID(int MasterID)
        {
            object returnID;
            returnID = Global.m_db.GetScalarValue("SELECT SeriesID FROM Inv.tblPurchaseInvoiceMaster WHERE PurchaseInvoiceID ='" + MasterID + "'");
            return Convert.ToInt32(returnID);
        }

        public DataTable GetPurchaseInvoiceMasterInfo(string RowID)
        {
            return Global.m_db.SelectQry("SELECT * FROM Inv.tblPurchaseInvoiceMaster WHERE PurchaseInvoiceID ='" + RowID + "'", "table");
        }

        public static DataTable GetPurchaseInvoiceMasterInfoByOrderNo(string OrderNo)
        {
            string strQuery = "SELECT * FROM Inv.tblPurchaseInvoiceMaster WHERE OrderNo ='" + OrderNo + "'";
            return Global.m_db.SelectQry(strQuery, "dt");

        }

        public DataTable NavigatePurchaseInvoiceMaster(int CurrentID, Navigate NavTo)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spPurchaseInvoiceNavigate");
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

            DataTable dtPurchaseInvoiceMst = Global.m_db.GetDataTable();
            return dtPurchaseInvoiceMst;
        }

        /// <summary>

        /// </summary>
        /// <param name="CurrentID"></param>
        /// <returns></returns>

        public DataTable GetPurchaseInvoiceDetails(int MasterID)
        {
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("Inv.spGetPurchaseInvoiceDetail");
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
            DataTable dtPurchaseInvoiceDtl = Global.m_db.GetDataTable();
            return dtPurchaseInvoiceDtl;
        }


        public DataTable GetAllPurchaseInfo(DateTime FromDate, DateTime ToDate, bool IsPurchaseAccount, int PurchaseLedgerID)
        {
            string SQL = "";
            //Get the information from  PurchaseInvoiceMaster and PurchaseInvoiceDetails according to the TimeRange
            SQL = "Select a.PurchaseInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount, " +
               "d.EngName PurchaseAccount,e.EngName Product,b.Quantity,b.PurchaseRate,b.Amount,b.DiscPercentage, " +
               "b.Discount,b.Net_Amount FROM Inv.tblPurchaseInvoiceMaster a, Inv.tblPurchaseInvoiceDetails b, " +
               "Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE a.PurchaseInvoiceID=b.PurchaseInvoiceID " +
               "AND a.CashPartyLedgerID=c.LedgerID AND a.PurchaseLedgerID=d.LedgerID AND b.ProductID = e.ProductID " +
               "AND a.PurchaseInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' ";
            if (IsPurchaseAccount == true)
            {
                SQL += "AND a.PurchaseLedgerID ='" + PurchaseLedgerID + "'";

            }

            return Global.m_db.SelectQry(SQL, "table");
        }

        //Get the information of PurchaseReport for SummaryCredit
        /// <summary>
        /// Collecting only Debtor and Creditor Account Ledgers 
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>

        public DataTable GetSummaryCreditInfo(DateTime FromDate, DateTime ToDate, bool IsPurchaseAccount, int PurchaseLedgerID)
        {
            // Block for Collecting Subgroups under Debtor and Creditor and again corresponding LedgerID under them 
            //Collecting Subgroups of Debtors in arraylist

            int Debtor = AccountGroup.GetGroupIDFromGroupNumber(29);
            DataTable dtDebtor = Ledger.GetAllLedger(Debtor);

            int Creditor = AccountGroup.GetGroupIDFromGroupNumber(114);
            DataTable dtCreditor = Ledger.GetAllLedger(Creditor);
            string LedgerIDList = "";
            for (int i = 0; i <= dtDebtor.Rows.Count - 1; i++)
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
            string SQL = "Select a.PurchaseInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount, " +
                            "d.EngName PurchaseAccount,e.EngName Product,b.Quantity,b.PurchaseRate,b.Amount, " +
                            "b.DiscPercentage,b.Discount,b.Net_Amount FROM Inv.tblPurchaseInvoiceMaster a, " +
                            "Inv.tblPurchaseInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e " +
                            "WHERE a.PurchaseInvoiceID=b.PurchaseInvoiceID AND " +
                            "a.CashPartyLedgerID=c.LedgerID AND a.PurchaseLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND " +
                            "a.PurchaseInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(FromDate) + "' AND " +
                            "a.CashPartyLedgerID IN(" + LedgerIDList + ")";

            if (IsPurchaseAccount == true)
            {
                SQL += "AND a.PurchaseLedgerID ='" + PurchaseLedgerID + "'";

            }
            return Global.m_db.SelectQry(SQL, "table");
        }
        //Get the information of PurchaseReport for SummaryCash
        /// <summary>
        /// Collecting only ledgers of Cash_In_Hand
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>

        public DataTable GetSummaryCashInfo(DateTime FromDate, DateTime ToDate, bool IsPurchaseAccount, int PurchaseLedgerID)
        {
            int CashInHand = AccountGroup.GetGroupIDFromGroupNumber(102);
            DataTable dtCash = Ledger.GetAllLedger(CashInHand);
            string CreditLedgerList = "";
            for (int i = 0; i <= dtCash.Rows.Count - 1; i++)
            {
                DataRow drCash = dtCash.Rows[i];
                if (i == 0)
                    CreditLedgerList = "'" + drCash["LedgerID"].ToString() + "'";
                else
                    CreditLedgerList += "," + "'" + drCash["LedgerID"].ToString() + "'";
            }
            string SQL = "Select a.PurchaseInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName PurchaseAccount, " +
                            "e.EngName Product,b.Quantity,b.PurchaseRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM " +
                            "Inv.tblPurchaseInvoiceMaster a, Inv.tblPurchaseInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, " +
                            "Inv.tblProduct e WHERE a.PurchaseInvoiceID=b.PurchaseInvoiceID AND a.CashPartyLedgerID=c.LedgerID AND " +
                            "a.PurchaseLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND a.PurchaseInvoice_Date " +
                            "BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND a.CashPartyLedgerID IN(" + CreditLedgerList + ")";

            if (IsPurchaseAccount == true)
            {
                SQL += "AND a.PurchaseLedgerID ='" + PurchaseLedgerID + "'";

            }
            return Global.m_db.SelectQry(SQL, "table");
        }

        public DataTable GetVoucherNoSingleInfo(DateTime FromDate, DateTime ToDate, string VoucherNo, bool IsPurchaseAccount, int PurchaseLedgerID)
        {
            string SQL = "Select a.PurchaseInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName PurchaseAccount, " +
                            "e.EngName Product,b.Quantity,b.PurchaseRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM  " +
                            "Inv.tblPurchaseInvoiceMaster a, Inv.tblPurchaseInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e " +
                            "WHERE a.PurchaseInvoiceID=b.PurchaseInvoiceID AND a.CashPartyLedgerID=c.LedgerID AND a.PurchaseLedgerID=d.LedgerID AND " +
                            "b.ProductID = e.ProductID AND a.PurchaseInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND  " +
                            "a.Voucher_No ='" + VoucherNo + "'";

            if (IsPurchaseAccount == true)
            {
                SQL += "AND a.PurchaseLedgerID ='" + PurchaseLedgerID + "'";

            }
            return Global.m_db.SelectQry(SQL, "table");
        }


        public DataTable GetSingleProductInfo(DateTime FromDate, DateTime ToDate, int ProductID, bool IsPurchaseAccount, int PurchaseLedgerID)
        {
            string SQL = "Select a.PurchaseInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName PurchaseAccount, " +
                            "e.EngName Product,b.Quantity,b.PurchaseRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM Inv.tblPurchaseInvoiceMaster a, " +
                            "Inv.tblPurchaseInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE a.PurchaseInvoiceID=b.PurchaseInvoiceID AND " +
                            "a.CashPartyLedgerID=c.LedgerID AND a.PurchaseLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND " +
                            "a.PurchaseInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND b.ProductID='" + ProductID + "'";

            if (IsPurchaseAccount == true)
            {
                SQL += "AND a.PurchaseLedgerID ='" + PurchaseLedgerID + "'";

            }
            return Global.m_db.SelectQry(SQL, "table");
        }

        public DataTable GetProductQuantityInfo(DateTime FromDate, DateTime ToDate, int ProductID, int FromQuantity, int ToQuantity, bool IsPurchaseAccount, int PurchaseLedgerID)
        {
            string SQL = "Select a.PurchaseInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName PurchaseAccount, " +
                          "e.EngName Product,b.Quantity,b.PurchaseRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM Inv.tblPurchaseInvoiceMaster a, " +
                          "Inv.tblPurchaseInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE a.PurchaseInvoiceID=b.PurchaseInvoiceID AND " +
                          "a.CashPartyLedgerID=c.LedgerID AND a.PurchaseLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND " +
                          "a.PurchaseInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND b.ProductID='" + ProductID + "' AND " +
                          "b.Quantity BETWEEN '" + FromQuantity + "' AND '" + ToQuantity + "'";

            if (IsPurchaseAccount == true)
            {
                SQL += "AND a.PurchaseLedgerID ='" + PurchaseLedgerID + "'";

            }

            return Global.m_db.SelectQry(SQL, "table");
        }

        public DataTable GetPartySelectedPartyInfo(DateTime FromDate, DateTime ToDate, int PartyID, bool IsPurchaseAccount, int PurchaseLedgerID)
        {

            string SQL = "Select a.PurchaseInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName PurchaseAccount,e.EngName Product, " +
                            "b.Quantity,b.PurchaseRate,b.Amount,b.DiscPercentage,b.Discount,b.Net_Amount FROM Inv.tblPurchaseInvoiceMaster a, " +
                            "Inv.tblPurchaseInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE a.PurchaseInvoiceID=b.PurchaseInvoiceID AND " +
                            "a.CashPartyLedgerID=c.LedgerID AND a.PurchaseLedgerID=d.LedgerID AND b.ProductID = e.ProductID AND  " +
                            "a.PurchaseInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND a.CashPartyLedgerID='" + PartyID + "'";

            if (IsPurchaseAccount == true)
            {
                SQL += "AND a.PurchaseLedgerID ='" + PurchaseLedgerID + "'";
            }
            return Global.m_db.SelectQry(SQL, "table");
        }

        public DataTable GetPartyGroupInfo(DateTime FromDate, DateTime ToDate, int PartyGrpID, bool IsPurchaseAccount, int PurchaseLedgerID)
        {
            DataTable dtParty = Ledger.GetAllLedger(PartyGrpID);
            string PartyLedgerList = "";
            for (int i = 0; i <= dtParty.Rows.Count - 1; i++)
            {
                DataRow drParty = dtParty.Rows[i];
                if (i == 0)
                    PartyLedgerList = "'" + drParty["LedgerID"].ToString() + "'";
                else
                    PartyLedgerList += "," + "'" + drParty["LedgerID"].ToString() + "'";
            }
            string SQL = "Select a.PurchaseInvoice_Date, a.Voucher_No, b.code,c.EngName PartyAccount,d.EngName PurchaseAccount, " +
                            "e.EngName Product,b.Quantity,b.PurchaseRate,b.Amount,b.DiscPercentage, " +
                            "b.Discount,b.Net_Amount FROM Inv.tblPurchaseInvoiceMaster a, " +
                            "Inv.tblPurchaseInvoiceDetails b, Acc.tblLedger c, Acc.tblLedger d, Inv.tblProduct e WHERE  " +
                            "a.PurchaseInvoiceID=b.PurchaseInvoiceID AND a.CashPartyLedgerID=c.LedgerID AND " +
                            "a.PurchaseLedgerID=d.LedgerID AND " +
                            "b.ProductID = e.ProductID AND a.PurchaseInvoice_Date BETWEEN '" + Date.ToDB(FromDate) + "' AND '" + Date.ToDB(ToDate) + "' AND  " +
                            "a.CashPartyLedgerID IN(" + PartyLedgerList + ")";

            if (IsPurchaseAccount == true)
            {
                SQL += "AND a.PurchaseLedgerID ='" + PurchaseLedgerID + "'";
            }
            return Global.m_db.SelectQry(SQL, "table");
        }

        public static DataTable GetPurchaseReport(int? PurchaseLedgerID, int? ProductGroupID, int? ProductID, int? PartyGroupID, int? PartyID, int? DepotID, int? ProjectID, DateTime? FromDate, DateTime? ToDate, InventoryReportType PurchaseReportType, string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            try
            {
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spGetPurchaseReport");
                Global.m_db.AddParameter("@PurchaseLedgerID", SqlDbType.Int, PurchaseLedgerID);
                Global.m_db.AddParameter("@ProductGroupID", SqlDbType.Int, ProductGroupID);
                Global.m_db.AddParameter("@ProductID", SqlDbType.Int, ProductID);
                Global.m_db.AddParameter("@PartyGroupID", SqlDbType.Int, PartyGroupID);
                Global.m_db.AddParameter("@PartyID", SqlDbType.Int, PartyID);
                Global.m_db.AddParameter("@DepotID", SqlDbType.Int, DepotID);
                Global.m_db.AddParameter("@ProjectID", SqlDbType.Int, ProjectID);
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, FromDate);
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, ToDate);
                Global.m_db.AddParameter("@PurchaseReportTypeIndex", SqlDbType.Int, (int)PurchaseReportType);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                DataTable dtPurchaseReportDtl = Global.m_db.GetDataTable();
                return dtPurchaseReportDtl;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                throw ex;
            }

        }

        public bool RemovePurchaseEntry(int PurchaseInvoiceID, string GridValues)
        {
            try
            {
                Global.m_db.BeginTransaction();
                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Inv.spRemovePurchaseInvoiceEntry");
                Global.m_db.AddParameter("@PurchaseInvoiceID", SqlDbType.Int, PurchaseInvoiceID);
                //system.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                Global.m_db.ProcessParameter();

                string username = User.CurrentUserName;
                string voucherdate = Date.ToDB(DateTime.Now).ToString();
                string VoucherType = "PINV";
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
                    VoucherReference.CreateReference(dr, voucherID, "PURCHASE");
                }

                foreach (DataRow dr in dtReference.Select("[RefID] is not null"))
                {
                    VoucherReference.CreateReferenceVoucher(dr, voucherID, "PURCHASE");
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

        static string ModifyReference(int voucherID, DataTable dtReference, string RowsToDelete)
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
                        VoucherReference.CreateReference(dr, voucherID, "PURCHASE");
                    }

                    foreach (DataRow dr in dtReference.Select("[RefID] is not null and [RVID] is null"))
                    {
                        VoucherReference.CreateReferenceVoucher(dr, voucherID, "PURCHASE");
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
