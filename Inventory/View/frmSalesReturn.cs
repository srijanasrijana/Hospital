using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using System.Collections;
using CrystalDecisions.Shared;
using Inventory;
using Inventory.Reports;
using Common;


namespace Inventory
{
    public interface IfrmSalesReturn
    {
        void SalesReturn(int RowID);
    }
    public partial class frmSalesReturn : Form, ListProduct, IVoucherRecurring, IVoucherList, IfrmDateConverter, IVoucherReference
    {

        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;
        DataTable dt1 = new DataTable();
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private string Prefix = "";
        private int loopCounter = 0;
        bool hasChanged = false;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private bool IsFieldChanged = false;
        private bool IsShortcutKey = false;
        private double purchaserate = 0;
        private string productcode = "";
        private int CurrRowPos = 0;
        private int SalesInvoiceIDCopy = 0;
        //For Export Menu
        ContextMenu Menu_Export;
        private double VAT = 0;
        private double TAX1 = 0;
        private double TAX2 = 0;
        private double TAX3 = 0;
        private double tax1amt = 0;
        private double tax2amt = 0;
        private double tax3amt = 0;
        public double GAmount = 0;
        private int prntDirect = 0;
        private string FileName = "";


        string Tax1checkvalue = "";
        string Tax2checkvalue = "";
        string Tax3checkvalue = "";
        string Tax1value = "";
        string Tax2value = "";
        string Tax3value = "";
        double AdjustmentAmount = 0;

        private IMDIMainForm m_MDIForm;
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        private enum GridColumn : int
        {
            Del = 0, SNo, Code_No, ProductName, Qty, SalesRate, Amount, SplDisc_Percent, SplDisc, NetAmount, ProductID
        };

        private Inventory.Model.dsSalesReturn dsSalesReturn = new Model.dsSalesReturn();
        SalesReturn m_SalesReturn = new SalesReturn();
        ListItem liProjectID = new ListItem();
        private int SalesReturnIDCopy = 0;
        ListItem SeriesID = new ListItem();
        List<int> AccClassID = new List<int>();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        DataTable dtAccClassID = new DataTable();
        private int m_SalesReturnID;
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtQty = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtRate = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProduct = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductCode = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscPercentage = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtGrossAmt = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtNetAmt = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        double GrossAmount = 0;
        double NetAmount = 0;

        bool m_isRecurring = false;
        int m_RVID = 0;
        /// <summary>
        /// constructor to open the form from voucher recurring reminder
        /// </summary>
        /// <param name="SalesInvoiceID"></param>
        /// <param name="isRecurring"></param>
        public frmSalesReturn(int SalesReturnID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_SalesReturnID = SalesReturnID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }
        public frmSalesReturn()
        {
            InitializeComponent();
        }
        public frmSalesReturn(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;

        }
        public frmSalesReturn(int SalesReturnID)
        {
            InitializeComponent();
            this.m_SalesReturnID = SalesReturnID;
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            // check if transaction amount is greater than reference amount
            if (!CheckAmtAgainstRefAmt())
            {
                return;
            }

            #region BLOCK FOR MANUAL VOUCHER NUMBERING TYPE
            VoucherConfiguration m_VouConfig = new VoucherConfiguration();
            if (SeriesID.ID > 0)
            {
                DataTable dtVouConfigInfo = m_VouConfig.GetVouNumConfiguration(Convert.ToInt32(SeriesID.ID));
                DataRow drVouConfigInfo = dtVouConfigInfo.Rows[0];
                if (drVouConfigInfo["NumberingType"].ToString() == "Manual")
                {
                    //Enter in this block only when VoucherNumberingType is Manual
                    //Checking for Manual VoucherNumberingType
                    try
                    {
                        string returnStr = m_VouConfig.ValidateManualVouNum(txtVoucherNo.Text, Convert.ToInt32(SeriesID.ID), txtSalesReturnID.Text == "" ? 0 : Convert.ToInt32(txtSalesReturnID.Text), "Inv.tblSalesReturnMaster", (m_mode == EntryMode.NEW) ? true : false);
                        switch (returnStr)
                        {
                            case "INVALID_SERIES":
                                {
                                    MessageBox.Show("Invalid Series Name,please select valid Series Name and try again!");
                                    return;
                                }
                                break;
                            case "BLANK_WARN":
                                if (MessageBox.Show("Voucher Number is Blank, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                break;
                            case "BLANK_DONT_ALLOW":
                                MessageBox.Show("Voucher Number is Blank,Please fill the Voucher Number first!");
                                return;
                                break;
                            case "SUCCESS":

                                break;
                            case "DUPLICATE_WARN":
                                if (MessageBox.Show("Voucher Number is Duplicated, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                break;
                            case "DUPLICATE_DONT_ALLOW":
                                {
                                    MessageBox.Show("Voucher Number is Duplicated,Please insert the unique Voucher Number");
                                    return;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                        Global.Msg(ex.Message);
                    }
                }
            }
            #endregion

            //Check Validation
            if (!Validate())
                return;
            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }
            if (drdtadditionalfield["IsField1Required"].ToString() == "True")
            {
                if (txtfirst.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field1"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField2Required"].ToString() == "True")
            {
                if (txtsecond.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field2"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField3Required"].ToString() == "True")
            {
                if (txtthird.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field3"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField4Required"].ToString() == "True")
            {
                if (txtfourth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field4"].ToString() + " " + "is Required Field");
                    return;
                }

            }
            if (drdtadditionalfield["IsField5Required"].ToString() == "True")
            {
                if (txtfifth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field5"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            //Reading value of slabs
            //for Sales Return and Sales Purchase

            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed

                    #region Add voucher number if voucher number is automatic and hidden from the setting
                    int increasedSeriesNum = 0;
                    SeriesID = (ListItem)cboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                    if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        object m_vounum = m_VouConfig.GenerateVouNumTypeNoUpdate(Convert.ToInt32(SeriesID.ID), out increasedSeriesNum);
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            return;
                        }

                        txtVoucherNo.Text = m_vounum.ToString();
                        txtVoucherNo.Enabled = false;
                    }
                    #endregion

                    isNew = true;
                    OldGrid = " ";
                    NewGrid = " ";
                    NewGrid = NewGrid + "-" + "Voucher No" + txtVoucherNo + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Cash/Party" + cboCashParty.Text + "Depot" + cboDepot.Text + "OrderNo" + txtOrderNo.Text;
                    //Collect the Contents of the grid for audit log
                    for (int i = 0; i < grdSalesReturn.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                    {
                        string productname = grdSalesReturn[i + 1, (int)GridColumn.ProductName].Value.ToString();
                        string qty = grdSalesReturn[i + 1, (int)GridColumn.Qty].Value.ToString();
                        string rate = grdSalesReturn[i + 1, (int)GridColumn.SalesRate].Value.ToString();
                        string amt = grdSalesReturn[i + 1, (int)GridColumn.Amount].Value.ToString();
                        NewGrid = NewGrid + string.Concat(productname, qty, rate, amt);
                    }
                    NewGrid = "NewGridValues" + NewGrid;
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable SalesReturnDetails = new DataTable();
                        SalesReturnDetails.Columns.Add("Code");
                        SalesReturnDetails.Columns.Add("Product");
                        SalesReturnDetails.Columns.Add("Quantity");
                        SalesReturnDetails.Columns.Add("SalesRate");
                        SalesReturnDetails.Columns.Add("Amount");
                        SalesReturnDetails.Columns.Add("DiscPercentage");
                        SalesReturnDetails.Columns.Add("Discount");
                        SalesReturnDetails.Columns.Add("NetAmount");
                        SalesReturnDetails.Columns.Add("ProductID");
                        for (int i = 0; i < grdSalesReturn.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            SalesReturnDetails.Rows.Add(grdSalesReturn[i + 1, (int)GridColumn.Code_No].Value, grdSalesReturn[i + 1, (int)GridColumn.ProductName].Value, grdSalesReturn[i + 1, (int)GridColumn.Qty].Value, grdSalesReturn[i + 1, (int)GridColumn.SalesRate].Value, grdSalesReturn[i + 1, (int)GridColumn.Amount].Value, grdSalesReturn[i + 1, (int)GridColumn.SplDisc_Percent].Value, grdSalesReturn[i + 1, (int)GridColumn.SplDisc].Value, grdSalesReturn[i + 1, (int)GridColumn.NetAmount].Value, grdSalesReturn[i + 1, (int)GridColumn.ProductID].Value);
                        }
                        DateTime SalesReturn_Date = Date.ToDotNet(txtDate.Text);
                        ListItem liSalesLedgerID = new ListItem();
                        liSalesLedgerID = (ListItem)cmboSalesAcc.SelectedItem;

                        ListItem LiCashPartyLedgerID = new ListItem();
                        LiCashPartyLedgerID = (ListItem)cboCashParty.SelectedItem;

                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        int Tax1ID = AccountGroup.GetLedgerIDFromLedgerNumber(314);
                        int Tax2ID = AccountGroup.GetLedgerIDFromLedgerNumber(315);
                        int Tax3ID = AccountGroup.GetLedgerIDFromLedgerNumber(316);
                        int VatID = AccountGroup.GetLedgerIDFromLedgerNumber(412);
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;

                        if (txtSalesTax1.Text == "")
                        {

                            txtSalesTax1.Text = Convert.ToDouble(0.000).ToString();

                        }
                        if (txtSalesTax2.Text == "")
                        {

                            txtSalesTax2.Text = Convert.ToDouble(0.000).ToString();

                        }
                        if (txtSalesTax3.Text == "")
                        {

                            txtSalesTax3.Text = Convert.ToDouble(0.000).ToString();

                        }
                        //  lblAdjustment.Text = drSalesReturnMaster["Total_Amt"].ToString();

                        if (AccClassID.Count != 0)
                        {
                            m_SalesReturn.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liSalesLedgerID.ID), liSalesLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), LiCashPartyLedgerID.Value.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, SalesReturn_Date, txtRemarks.Text, SalesReturnDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Sales_Tax1On, Global.Default_Sales_Tax1Check, Global.Default_Tax2, Global.Default_Sales_Tax2On, Global.Default_Sales_Tax2Check, Global.Default_Tax3, Global.Default_Sales_Tax3On, Global.Default_Sales_Tax3On, OldGrid, NewGrid, isNew, txtSalesTax1.Text, txtSalesTax2.Text, txtSalesTax3.Text, lblVat.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblNetAmout.Text, OF, m_dtRecurringSetting, dtReference);
                            // m_SalesReturn.Modify(Convert.ToInt32(txtSalesReturnID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liSalesLedgerID.ID), Convert.ToInt32(LiCashPartyLedgerID.ID), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, SalesInvoice_Date, txtRemarks.Text, SalesReturnDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Sales_Tax1On, Global.Default_Sales_Tax1Check, Global.Default_Tax2, Global.Default_Sales_Tax2On, Global.Default_Sales_Tax2Check, Global.Default_Tax3, Global.Default_Sales_Tax3On, Global.Default_Sales_Tax3On, OldGrid, NewGrid, isNew, OF);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_SalesReturn.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liSalesLedgerID.ID), liSalesLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), LiCashPartyLedgerID.Value.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, SalesReturn_Date, txtRemarks.Text, SalesReturnDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Sales_Tax1On, Global.Default_Sales_Tax1Check, Global.Default_Tax2, Global.Default_Sales_Tax2On, Global.Default_Sales_Tax2Check, Global.Default_Tax3, Global.Default_Sales_Tax3On, Global.Default_Sales_Tax3On, OldGrid, NewGrid, isNew, txtSalesTax1.Text, txtSalesTax2.Text, txtSalesTax3.Text, lblVat.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblNetAmout.Text, OF, m_dtRecurringSetting, dtReference);
                        }
                        //Update the last AutoNumber in tblSeries,only if the voucher hide type is true
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }

                        Global.Msg("Sales Return created successfully!");
                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_SalesReturnID, "SALES_RETURN");
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID);
                            m_isRecurring = false;
                        }

                        AccClassID.Clear();
                        ChangeState(EntryMode.NORMAL);

                        ////Do not close the form if do not close is checked
                        if (!chkDoNotClose.Checked)
                            this.Close();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        #region SQLExceptions
                        switch (ex.Number)
                        {
                            case 4060: // Invalid Database 
                                Global.Msg("Invalid Database", MBType.Error, "Error");
                                break;

                            case 18456: // Login Failed 
                                Global.Msg("Login Failed!", MBType.Error, "Error");
                                break;

                            case 547: // ForeignKey Violation , Check Constraint
                                Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                                break;

                            case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                                Global.Msg("ERROR: The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                                break;

                            case 2601: // Unique Index/Constriant Violation 
                                Global.Msg("Unique index violation!", MBType.Warning, "Error");
                                break;

                            case 5000: //Trigger violation
                                Global.Msg("Trigger violation!", MBType.Warning, "Error");
                                break;

                            default:
                                Global.MsgError(ex.Message);
                                break;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }

                    break;

                #endregion

                #region EDIT
                case EntryMode.EDIT: //if new button is pressed
                    isNew = false;
                    NewGrid = " ";
                    NewGrid = NewGrid + "Voucher No" + txtVoucherNo + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Cash/Party" + cboCashParty.Text + "Depot" + cboDepot.Text + "OrderNo" + txtOrderNo.Text;
                    //Collect the Contents of the grid for audit log
                    for (int i = 0; i < grdSalesReturn.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                    {
                        string productname = grdSalesReturn[i + 1, (int)GridColumn.ProductName].Value.ToString();
                        string qty = grdSalesReturn[i + 1, (int)GridColumn.Qty].Value.ToString();
                        string rate = grdSalesReturn[i + 1, (int)GridColumn.SalesRate].Value.ToString();
                        string amt = grdSalesReturn[i + 1, (int)GridColumn.Amount].Value.ToString();
                        NewGrid = NewGrid + string.Concat(productname, qty, rate, amt);
                    }
                    NewGrid = "NewGridValues" + NewGrid;
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable SalesReturnDetails = new DataTable();
                        SalesReturnDetails.Columns.Add("Code");
                        SalesReturnDetails.Columns.Add("Product");
                        SalesReturnDetails.Columns.Add("Quantity");
                        SalesReturnDetails.Columns.Add("SalesRate");
                        SalesReturnDetails.Columns.Add("Amount");
                        SalesReturnDetails.Columns.Add("DiscPercentage");
                        SalesReturnDetails.Columns.Add("Discount");
                        SalesReturnDetails.Columns.Add("NetAmount");
                        SalesReturnDetails.Columns.Add("ProductID");
                        for (int i = 0; i < grdSalesReturn.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            SalesReturnDetails.Rows.Add(grdSalesReturn[i + 1, (int)GridColumn.Code_No].Value, grdSalesReturn[i + 1, (int)GridColumn.ProductName].Value, grdSalesReturn[i + 1, (int)GridColumn.Qty].Value, grdSalesReturn[i + 1, (int)GridColumn.SalesRate].Value, grdSalesReturn[i + 1, (int)GridColumn.Amount].Value, grdSalesReturn[i + 1, (int)GridColumn.SplDisc_Percent].Value, grdSalesReturn[i + 1, (int)GridColumn.SplDisc].Value, grdSalesReturn[i + 1, (int)GridColumn.NetAmount].Value, grdSalesReturn[i + 1, (int)GridColumn.ProductID].Value);
                        }

                        DateTime SalesInvoice_Date = Date.ToDotNet(txtDate.Text);
                        ListItem liSalesLedgerID = new ListItem();
                        liSalesLedgerID = (ListItem)cmboSalesAcc.SelectedItem;

                        ListItem LiCashPartyLedgerID = new ListItem();
                        LiCashPartyLedgerID = (ListItem)cboCashParty.SelectedItem;

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;

                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;

                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        int Tax1ID = AccountGroup.GetLedgerIDFromLedgerNumber(314);
                        int Tax2ID = AccountGroup.GetLedgerIDFromLedgerNumber(315);
                        int Tax3ID = AccountGroup.GetLedgerIDFromLedgerNumber(316);
                        int VatID = AccountGroup.GetLedgerIDFromLedgerNumber(412);

                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        //if (chkRecurring.Checked)
                        //{
                        //    m_dtRecurringSetting.Rows[0]["RVID"] = RSID;  // send id of voucher setting for modification
                        //    m_dtRecurringSetting.Rows[0]["VoucherID"] = txtSalesReturnID.Text;
                        //}
                        if (txtSalesTax1.Text == "")
                        {

                            txtSalesTax1.Text = Convert.ToDouble(0.000).ToString();

                        }
                        if (txtSalesTax2.Text == "")
                        {

                            txtSalesTax2.Text = Convert.ToDouble(0.000).ToString();

                        }
                        if (txtSalesTax3.Text == "")
                        {

                            txtSalesTax3.Text = Convert.ToDouble(0.000).ToString();

                        }


                        if (AccClassID.Count != 0)
                        {
                            m_SalesReturn.Modify(Convert.ToInt32(txtSalesReturnID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liSalesLedgerID.ID), liSalesLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, SalesInvoice_Date, txtRemarks.Text, SalesReturnDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Sales_Tax1On, Global.Default_Sales_Tax1Check, Global.Default_Tax2, Global.Default_Sales_Tax2On, Global.Default_Sales_Tax2Check, Global.Default_Tax3, Global.Default_Sales_Tax3On, Global.Default_Sales_Tax3On, OldGrid, NewGrid, isNew, txtSalesTax1.Text, txtSalesTax2.Text, txtSalesTax3.Text, lblVat.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblNetAmout.Text, OF, m_dtRecurringSetting, dtReference, ToDeleteRows);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_SalesReturn.Modify(Convert.ToInt32(txtSalesReturnID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liSalesLedgerID.ID), liSalesLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, SalesInvoice_Date, txtRemarks.Text, SalesReturnDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Sales_Tax1On, Global.Default_Sales_Tax1Check, Global.Default_Tax2, Global.Default_Sales_Tax2On, Global.Default_Sales_Tax2Check, Global.Default_Tax3, Global.Default_Sales_Tax3On, Global.Default_Sales_Tax3On, OldGrid, NewGrid, isNew, txtSalesTax1.Text, txtSalesTax2.Text, txtSalesTax3.Text, lblVat.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblNetAmout.Text, OF, m_dtRecurringSetting, dtReference, ToDeleteRows);
                            // LiCashPartyLedgerID.Value.ToString()

                        }
                        Global.Msg("Sales Return modified successfully!");
                        AccClassID.Clear();
                        ChangeState(EntryMode.NORMAL);
                        ClearVoucher();
                        btnNew_Click(sender, e);

                        ////Do not close the form if do not close is checked
                        if (checkBox2.Checked)
                        {
                            Navigation(Navigate.Last);
                            ClearVoucher();
                            ChangeState(EntryMode.NEW);
                            btnNew_Click(sender, e);
                        }

                        if (!chkDoNotClose.Checked)
                            this.Close();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        #region SQLExceptions
                        switch (ex.Number)
                        {
                            case 4060: // Invalid Database 
                                Global.Msg("Invalid Database", MBType.Error, "Error");
                                break;

                            case 18456: // Login Failed 
                                Global.Msg("Login Failed!", MBType.Error, "Error");
                                break;

                            case 547: // ForeignKey Violation , Check Constraint
                                Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                                break;

                            case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                                Global.Msg("ERROR: The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                                break;

                            case 2601: // Unique Index/Constriant Violation 
                                Global.Msg("Unique index violation!", MBType.Warning, "Error");
                                break;

                            case 5000: //Trigger violation
                                Global.Msg("Trigger violation!", MBType.Warning, "Error");
                                break;

                            default:
                                Global.MsgError(ex.Message);
                                break;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError("Please Insert All the Requirements such as Gov Taxes" + " " + ex.Message);
                    }

                    break;

                #endregion
            }
            ClearVoucher();
        }

        #region New Added Function for due date
        //private void dueDate()
        //{
        //    dt1.Rows.Clear();
        //    dt1.Columns.Clear();
        //    dt1.Columns.Add("ProductID");
        //    dt1.Columns.Add("ProductName");

        //    int i = 1;
        //    int ProductID = 0;
        //    for (i = 1; i <= grdSalesReturn. Rows.Count() - 2; i++)
        //    {
        //        try
        //        {

        //            string str = (grdSalesReturn[i, (int)GridColumn.ProductID].Value).ToString();
        //            ProductID = Convert.ToInt32(str);
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        int GroupID = AccountGroup.GetGroupIDByLedgerID(ProductID);
        //        if (GroupID == 29 && grdSalesReturn[i, (int)GridColumn.NetAmount].Value.ToString() == "Debit")
        //        {
        //            dt1.Rows.Add(grdSalesReturn[i, (int)GridColumn.ProductID].Value, grdSalesReturn[i, (int)GridColumn.ProductName].Value);
        //        }
        //    }
        //}
        #endregion
        private void ClearSalesReturn(bool IsVoucherNumFinished)
        {
            txtVoucherNo.Clear();
            cmboSalesAcc.Text = string.Empty;
            cboSeriesName.Text = string.Empty;
            cboDepot.Text = string.Empty;
            cboCashParty.Text = string.Empty;
            lblAdjustment.Text = null;
            //actually generate a new voucher no.
            // txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtOrderNo.Clear();
            //txtSalesTax1.Clear();
            //txtSalesTax2.Clear();
            //txtSalesTax3.Clear();
            txtSalesTax1.Text = txtSalesTax2.Text = txtSalesTax3.Text = lblAdjustment.Text = lblGrandTotal.Text = lblVat.Text = lblGross.Text = lblNetAmout.Text = 
                (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //txtSalesTax2.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //txtSalesTax3.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //lblAdjustment.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //lblGrandTotal.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //lblVat.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //lblGross.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //lblNetAmout.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            txtRemarks.Clear();
            if (IsVoucherNumFinished == false)
            {
                grdSalesReturn.Rows.Clear();
            }
        }
        private void ClearVoucher()
        {
            m_isRecurring = false;
            m_RVID = 0;
            ClearSalesReturn(false);
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdSalesReturn.Redim(2, 11);
            AddGridHeader(); //Write header part
            //AddRowProduct(1);
            AddRowProduct1(1);
            ClearRecurringSetting();
            dtReference.Rows.Clear();
            AddReferenceColumns();
            m_refAmt = "0(Dr)";
            m_SalesReturnID = 0;
            txtSalesReturnID.Text = "0";
        }

        private bool Navigation(Navigate NavTo)
        {
            try
            {
                if (txtSalesReturnID.Text != "")
                    dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(txtSalesReturnID.Text), "SLS_RTN");

                ChangeState(EntryMode.NORMAL);
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtSalesReturnID.Text);
                    if (SalesReturnIDCopy > 0)
                    {
                        VouchID = SalesReturnIDCopy;
                        SalesReturnIDCopy = 0;
                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtSalesReturnID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
                DataTable dtSalesReturnMaster = m_SalesReturn.NavigateSalesReturnMaster(VouchID, NavTo);
                if (dtSalesReturnMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }
                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                DataRow drSalesReturnMaster = dtSalesReturnMaster.Rows[0]; //There is only one row. First row is the required record        

                //Show the corresponding Cash/Party Account Ledger in Combobox
                DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesReturnMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                //DataRow drCashPartyLedgerInfo = dtCashPartyInfo.Rows[0];
                foreach (DataRow drCashPartyLedgerInfo in dtCashPartyInfo.Rows)
                {
                    cboCashParty.Text = drCashPartyLedgerInfo["LedName"].ToString();
                }

                //Show the corresponding Sales Account Ledger in Combobox
                DataTable dtSalesLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesReturnMaster["SalesLedgerID"]), LangMgr.DefaultLanguage);
                //DataRow drSalesLedgerInfo = dtSalesLedgerInfo.Rows[0];
                foreach (DataRow drSalesLedgerInfo in dtSalesLedgerInfo.Rows)
                {
                    cmboSalesAcc.Text = drSalesLedgerInfo["LedName"].ToString();
                }

                //show the corresoponding SeriesName in Combobox
                DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drSalesReturnMaster["SeriesID"]));
                if (dtSeriesInfo.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Sales Return");
                    cboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dtSeriesInfo.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();
                }
                lblVouNo.Visible = true;
                txtVoucherNo.Visible = true;
                txtVoucherNo.Text = drSalesReturnMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drSalesReturnMaster["SalesReturn_Date"].ToString());
                txtRemarks.Text = drSalesReturnMaster["Remarks"].ToString();
                txtSalesReturnID.Text = drSalesReturnMaster["SalesReturnID"].ToString();
                txtSalesTax1.Text = Convert.ToDecimal(drSalesReturnMaster["Tax1"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                txtSalesTax2.Text = Convert.ToDecimal(drSalesReturnMaster["Tax2"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));;
                txtSalesTax3.Text = Convert.ToDecimal(drSalesReturnMaster["Tax3"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));;
                lblVat.Text =   Convert.ToDecimal(drSalesReturnMaster["VAT"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                txtRemarks.Text = drSalesReturnMaster["Remarks"].ToString();

                //For Additional Fields
                if (NumberOfFields > 0)
                {
                    if (NumberOfFields == 1)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = false;
                        txtsecond.Visible = false;
                        lblthird.Visible = false;
                        txtthird.Visible = false;
                        lblfourth.Visible = false;
                        txtfourth.Visible = false;
                        lblfifth.Visible = false;
                        txtfifth.Visible = false;
                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();

                        txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                        txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                        txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                        txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                        txtfifth.Text = drSalesReturnMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 2)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = true;
                        txtsecond.Visible = true;
                        lblthird.Visible = false;
                        txtthird.Visible = false;
                        lblfourth.Visible = false;
                        txtfourth.Visible = false;
                        lblfifth.Visible = false;
                        txtfifth.Visible = false;
                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        lblsecond.Text = drdtadditionalfield["Field2"].ToString();

                        txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                        txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                        txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                        txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                        txtfifth.Text = drSalesReturnMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 3)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = true;
                        txtsecond.Visible = true;
                        lblthird.Visible = true;
                        txtthird.Visible = true;
                        lblfourth.Visible = false;
                        txtfourth.Visible = false;
                        lblfifth.Visible = false;
                        txtfifth.Visible = false;
                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        lblthird.Text = drdtadditionalfield["Field3"].ToString();

                        txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                        txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                        txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                        txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                        txtfifth.Text = drSalesReturnMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 4)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = true;
                        txtsecond.Visible = true;
                        lblthird.Visible = true;
                        txtthird.Visible = true;
                        lblfourth.Visible = true;
                        txtfourth.Visible = true;
                        lblfifth.Visible = false;
                        txtfifth.Visible = false;
                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        lblthird.Text = drdtadditionalfield["Field3"].ToString();
                        lblfourth.Text = drdtadditionalfield["Field4"].ToString();

                        txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                        txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                        txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                        txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                        txtfifth.Text = drSalesReturnMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 5)
                    {
                        lblfirst.Visible = true;
                        txtfirst.Visible = true;
                        lblsecond.Visible = true;
                        txtsecond.Visible = true;
                        lblthird.Visible = true;
                        txtthird.Visible = true;
                        lblfourth.Visible = true;
                        txtfourth.Visible = true;
                        lblfifth.Visible = true;
                        txtfifth.Visible = true;

                        lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        lblthird.Text = drdtadditionalfield["Field3"].ToString();
                        lblfourth.Text = drdtadditionalfield["Field4"].ToString();
                        lblfifth.Text = drdtadditionalfield["Field5"].ToString();

                        txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                        txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                        txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                        txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                        txtfifth.Text = drSalesReturnMaster["Field5"].ToString();
                    }
                }
                //Getting DepotInfo
                DataTable dtDepotInfo = Depot.GetDepotInfo(Convert.ToInt32(drSalesReturnMaster["DepotID"]));
                DataRow drDepotInfo = dtDepotInfo.Rows[0];
                cboDepot.Text = drDepotInfo["DepotName"].ToString();
                txtVoucherNo.Text = drSalesReturnMaster["Voucher_No"].ToString();
                txtOrderNo.Text = drSalesReturnMaster["Order_No"].ToString();
                txtDate.Text = Date.DBToSystem(drSalesReturnMaster["SalesReturn_Date"].ToString());
                txtRemarks.Text = drSalesReturnMaster["Remarks"].ToString();

                lblSpecialDiscount.Text = Convert.ToDecimal(drSalesReturnMaster["SpecialDiscount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblNetAmout.Text = Convert.ToDecimal(drSalesReturnMaster["Net_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblTotalQty.Text = drSalesReturnMaster["Total_Qty"].ToString();
                lblGross.Text = Convert.ToDecimal(drSalesReturnMaster["Gross_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblGrandTotal.Text =  Convert.ToDecimal(drSalesReturnMaster["Total_Amt"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                dsSalesReturn.Tables["tblSalesReturnMaster"].Rows.Add(cboSeriesName.Text, drSalesReturnMaster["Voucher_No"].ToString(), Date.DBToSystem(drSalesReturnMaster["SalesReturn_Date"].ToString()), cboCashParty.Text, txtOrderNo.Text, cboDepot.Text, cmboSalesAcc.Text, drSalesReturnMaster["Remarks"].ToString());
                DataTable dtSalesReturnDetails = m_SalesReturn.GetSalesReturnDetails(Convert.ToInt32(txtSalesReturnID.Text));
                for (int i = 1; i <= dtSalesReturnDetails.Rows.Count; i++)
                {
                    DataRow drDetail = dtSalesReturnDetails.Rows[i - 1];
                    grdSalesReturn[i, (int)GridColumn.SNo].Value = i.ToString();
                    grdSalesReturn[i, (int)GridColumn.Code_No].Value = drDetail["Code"].ToString();
                    grdSalesReturn[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                    grdSalesReturn[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
                    grdSalesReturn[i, (int)GridColumn.SalesRate].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["SalesRate"])).ToString();
                    grdSalesReturn[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdSalesReturn[i, (int)GridColumn.SplDisc_Percent].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString();
                    grdSalesReturn[i, (int)GridColumn.SplDisc].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString();
                    grdSalesReturn[i, (int)GridColumn.NetAmount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString();
                    grdSalesReturn[i, (int)GridColumn.ProductID].Value = drDetail["ProductID"].ToString();
                    //AddRowProduct(grdSalesReturn.RowsCount);
                    AddRowProduct1(grdSalesReturn.RowsCount);
                    dsSalesReturn.Tables["tblSalesReturnDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(), drDetail["Quantity"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["SalesRate"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());
                }
                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtSalesReturnID.Text), "SALES");
                List<int> AccClassIDs = new List<int>();
                foreach (DataRow dr in dtAccClassDtl.Rows)
                {
                    AccClassIDs.Add(Convert.ToInt32(dr["AccClassID"]));
                }
                treeAccClass.ExpandAll();

                //Check for the treeview if it has Use
                foreach (TreeNode tn in treeAccClass.Nodes)
                {
                    LoadAccClassInfo(VouchID, tn, AccClassIDs.ToArray<int>(), treeAccClass);
                    int pid = Convert.ToInt32(tn.Tag);
                    if (AccClassIDs.ToArray<int>().Contains(pid))
                    {
                        tn.Checked = true;
                    }
                    else
                    {
                        tn.Checked = false;
                    }
                }
                CheckRecurringSetting(txtSalesReturnID.Text);

                btnExport.Enabled = true;
                return true;

            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
                return false;
            }
        }

        private void LoadAccClassInfo(int AccClassID, TreeNode tn, int[] CheckedIDs, TreeView tvAccClass)
        {
            foreach (TreeNode nd in tn.Nodes)
            {
                nd.Checked = false; //first clear the checkmark if anything is checked previously
                LoadAccClassInfo(AccClassID, nd, CheckedIDs, tvAccClass);

                foreach (int id in CheckedIDs)
                    if (Convert.ToInt32(nd.Tag) == id)
                        nd.Checked = true;
            }
            foreach (int parentid in CheckedIDs)
            {
                if (Convert.ToInt32(tn.Tag) != parentid)
                    tn.Checked = false;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {

            Navigation(Navigate.First);
        }

        private void AddRowProduct(int RowCount)
        {
            try
            {
                //Add a new row
                grdSalesReturn.Redim(Convert.ToInt32(RowCount + 1), grdSalesReturn.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdSalesReturn[i, (int)GridColumn.Del] = btnDelete;
                grdSalesReturn[i, (int)GridColumn.Del].AddController(evtDiscount);
                grdSalesReturn[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdSalesReturn[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();
                grdSalesReturn[i, (int)GridColumn.Code_No].Value = "(NEW)";
                //SourceGrid.Cells.Editors.ComboBox cboProduct = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                SourceGrid.Cells.Editors.ComboBox cboProduct = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                SourceGrid.Cells.Editors.ComboBox cboProductCode = new SourceGrid.Cells.Editors.ComboBox(typeof(string));

                //DataTable dt = Product.GetProductList(0);
                DataTable dtProductInfo = Product.GetProductList(0);
                List<string> lstProduct = new List<string>();
                List<string> lstProductCode = new List<string>();
                #region Language Management


                string LangField = "English";
                switch (LangMgr.DefaultLanguage)
                {
                    case Lang.English:
                        LangField = "EngName";
                        break;
                    case Lang.Nepali:
                        LangField = "NepName";
                        break;

                }
                #endregion

                //List<string> lstProduct = new List<string>();
                for (int i1 = 0; i1 < dtProductInfo.Rows.Count; i1++)
                {
                    DataRow dr = dtProductInfo.Rows[i1];
                    lstProduct.Add(dr[LangField].ToString());
                    lstProductCode.Add(dr["ProductCode"].ToString());
                }
                cboProduct.StandardValues = (string[])lstProduct.ToArray<string>();
                cboProduct.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
                cboProduct.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboProduct.Control.LostFocus += new EventHandler(Product_Selected);


                cboProduct.EditableMode = SourceGrid.EditableMode.Focus;
                grdSalesReturn[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell("", cboProduct);
                grdSalesReturn[i, (int)GridColumn.ProductName].AddController(evtProduct);
                grdSalesReturn[i, (int)GridColumn.ProductName].Value = "(NEW)";

                //for code
                cboProductCode.StandardValues = (string[])lstProductCode.ToArray<string>();
                cboProductCode.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
                cboProductCode.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboProductCode.Control.LostFocus += new EventHandler(ProductCode_Selected);

                cboProductCode.EditableMode = SourceGrid.EditableMode.Focus;
                grdSalesReturn[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell("", cboProductCode);
                grdSalesReturn[i, (int)GridColumn.Code_No].AddController(evtProductCode);
                grdSalesReturn[i, (int)GridColumn.Code_No].Value = "(NEW)";

                //for Gross Amount


                grdSalesReturn[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell("");
                grdSalesReturn[i, (int)GridColumn.Amount].AddController(evtGrossAmt);
                grdSalesReturn[i, (int)GridColumn.Amount].Value = "(NEW)";


                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus;
                grdSalesReturn[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("", txtQty);
                grdSalesReturn[i, (int)GridColumn.Qty].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtSalesRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtSalesRate.Control.LostFocus += new EventHandler(SalesRate_Modified);
                txtSalesRate.EditableMode = SourceGrid.EditableMode.Focus;

                grdSalesReturn[i, (int)GridColumn.SalesRate] = new SourceGrid.Cells.Cell("", txtSalesRate);
                grdSalesReturn[i, (int)GridColumn.SalesRate].Value = "(NEW)";

                grdSalesReturn[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell();
                grdSalesReturn[i, (int)GridColumn.Amount].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);

                txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                txtDiscPercentage.Control.Text = "0";
                grdSalesReturn[i, (int)GridColumn.SplDisc_Percent] = new SourceGrid.Cells.Cell("", txtDiscPercentage);
                grdSalesReturn[i, (int)GridColumn.SplDisc_Percent].Value = "0";

                SourceGrid.Cells.Editors.TextBox txtDiscount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscount.Control.LostFocus += new EventHandler(Discount_Modified);
                txtDiscount.EditableMode = SourceGrid.EditableMode.Focus;

                grdSalesReturn[i, (int)GridColumn.SplDisc] = new SourceGrid.Cells.Cell("0", txtDiscount);
                grdSalesReturn[i, (int)GridColumn.SplDisc].Value = "0";

                grdSalesReturn[i, (int)GridColumn.NetAmount] = new SourceGrid.Cells.Cell("Net Amount");
                grdSalesReturn[i, (int)GridColumn.NetAmount].Value = "0";

                grdSalesReturn[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell("ID");
                grdSalesReturn[i, (int)GridColumn.ProductID].Value = "0";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void EnableControls(bool Enable)
        {
            chkRecurring.Enabled = btnSetup.Enabled = txtVoucherNo.Enabled = cboDepot.Enabled = cboProjectName.Enabled = cmboSalesAcc.Enabled = cboSeriesName.Enabled = cboCashParty.Enabled = txtRemarks.Enabled = txtDate.Enabled = txtOrderNo.Enabled = grdSalesReturn.Enabled = cboSeriesName.Enabled = cboDepot.Enabled = treeAccClass.Enabled = btnAddAccClass.Enabled = tabControl1.Enabled = cboProjectName.Enabled = btnDate.Enabled = Enable;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false, true);
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true, true);
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true, true);
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
            }
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel, bool AddAccClass)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
            btnAddAccClass.Enabled = AddAccClass;

        }

        private int ChildCount(int AccClassID)
        {
            try
            {
                int m_RecCount = (int)User.GetAccessInfo(AccClassID).Rows.Count;
                return m_RecCount;
            }
            catch (Exception ex)
            {
                throw;
                return 0;
            }
        }
        //Recursive Function to Show Access Level in Treeview
        private void ShowAccClassInTreeView(TreeView tv, TreeNode n, int AccClassID)
        {
            #region Language Management

            tv.Font = LangMgr.GetFont();

            string LangField = "EngName";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;

            }
            #endregion
            if (Global.GlobalAccClassID == 1 && Global.GlobalAccessRoleID == 37)
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = AccountClass.GetAccClassTable(AccClassID);
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    TreeNode t = new TreeNode(dr[LangField].ToString());
                    t.Tag = dr["AccClassID"].ToString();
                    //Check if it is a parent Or if it has childs
                    try
                    {
                        if (ChildCount((int)dr["AccClassID"]) > 0)
                        {
                            //t.IsContainer = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                    if (n == null)
                    {
                        t.Checked = true;
                        tv.Nodes.Add(t); //Primary Group
                    }
                    else
                    {
                        t.Checked = true;
                        n.Nodes.Add(t); //Secondary Group
                    }
                }
            }
            else
            {

                DataTable dtUserInfo = User.GetUserInfo(User.CurrUserID); //user id must be read from  global i.e current user id
                DataRow drUserInfo = dtUserInfo.Rows[0];
                ArrayList AccClassChildIDs = new ArrayList();
                ArrayList tempParentAccClassChildIDs = new ArrayList();
                AccClassChildIDs.Clear();
                AccClassChildIDs.Add(Convert.ToInt32(drUserInfo["AccClassID"]));
                AccountClass.GetChildIDs(Convert.ToInt32(drUserInfo["AccClassID"]), ref AccClassChildIDs);
                DataTable dt = new DataTable();
                try
                {
                    dt = AccountClass.GetAccClassTable(AccClassID);
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    TreeNode t = new TreeNode(dr[LangField].ToString());
                    t.Tag = dr["AccClassID"].ToString();
                    tempParentAccClassChildIDs.Clear();
                    AccountClass.GetChildIDs(Convert.ToInt32(t.Tag), ref tempParentAccClassChildIDs);
                    //Check if it is a parent Or if it has childs
                    try
                    {
                        if (ChildCount((int)dr["AccClassID"]) > 0)
                        {
                            //t.IsContainer = true;
                        }

                        foreach (int itemIDs in AccClassChildIDs)  //To check if 
                        {
                            if (Convert.ToInt32(t.Tag) == itemIDs)
                            {
                                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                                loopCounter--;
                                t.Checked = true;
                                if (n == null)
                                {
                                    tv.Nodes.Add(t); //Primary Group
                                    return;
                                }
                                else if (Convert.ToInt32(t.Tag) == Convert.ToInt32(drUserInfo["AccClassID"]))
                                {
                                    t.Checked = true;
                                    tv.Nodes.Add(t);
                                    return;
                                }
                                else
                                {
                                    n.Nodes.Add(t); //Secondary Group
                                }
                            }
                            if (tempParentAccClassChildIDs.Contains(itemIDs) && loopCounter == 0)
                            {
                                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        }


        //Fill the cboUnder List box with Project Head
        private void ListProject(ComboBox ComboBoxControl)
        {
            #region Language Management
            ComboBoxControl.Font = LangMgr.GetFont();
            string LangField = "English";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;
            }
            #endregion

            chkDoNotClose.Checked = true;
            ComboBoxControl.Items.Clear();
            DataTable dt = Project.GetProjectTable(-1);
            ComboBoxControl.Items.Add(new ListItem((0), "None"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], dr[LangField].ToString()));
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }

        private void frmSalesReturn_Load(object sender, EventArgs e)
        {
            AddReferenceColumns();
            if (txtSalesReturnID.Text != "")
                dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(txtSalesReturnID.Text), "SLS_RTN");

            chkDoNotClose.Checked = true;
            //for displying slabs on Label
            DataTable dtSlabInfo = Slabs.GetSlabInfo(SlabType.None);//Getting all the info of Slabs
            foreach (DataRow drSlabInfo in dtSlabInfo.Rows)
            {
                if (drSlabInfo["Code"].ToString() == "TAX1")
                {
                    lblSalesTax1.Text = drSlabInfo["Name"].ToString();
                    TAX1 = Convert.ToDouble(drSlabInfo["Rate"].ToString());
                }

                if (drSlabInfo["Code"].ToString() == "TAX2")
                {
                    lblSalesTax2.Text = drSlabInfo["Name"].ToString();
                    TAX2 = Convert.ToDouble(drSlabInfo["Rate"].ToString());
                }

                if (drSlabInfo["Code"].ToString() == "TAX3")
                {
                    lblSalesTax3.Text = drSlabInfo["Name"].ToString();
                    TAX3 = Convert.ToDouble(drSlabInfo["Rate"].ToString());
                }

                if (drSlabInfo["Code"].ToString() == "VAT")
                {
                    label7.Text = drSlabInfo["Name"].ToString();
                    VAT = Convert.ToDouble(drSlabInfo["Rate"].ToString());
                }

            }

            double m_Tax1, m_Tax2, m_Tax3, m_Vat;
            m_Tax1 = m_Tax2 = m_Tax3 = m_Vat = 0;
            //Displaying Slabs according to Settings
            if (Global.Default_Sales_Tax1Check == "1")
            {
                lblSalesTax1.Visible = true;
                txtSalesTax1.Visible = true;
            }
            if (Global.Default_Sales_Tax1Check == "0")
            {
                lblSalesTax1.Visible = false;
                txtSalesTax1.Visible = false;
                TAX1 = 0;
            }
            if (Global.Default_Sales_Tax2Check == "1")
            {
                lblSalesTax2.Visible = true;
                txtSalesTax2.Visible = true;
            }
            if (Global.Default_Sales_Tax2Check == "0")
            {
                lblSalesTax2.Visible = false;
                txtSalesTax2.Visible = false;
                TAX2 = 0;
            }
            if (Global.Default_Sales_Tax3Check == "1")
            {
                lblSalesTax3.Visible = true;
                txtSalesTax3.Visible = true;
            }
            if (Global.Default_Sales_Tax3Check == "0")
            {
                lblSalesTax3.Visible = false;
                txtSalesTax3.Visible = false;
                TAX3 = 0;
            }

            DataTable dtvat = new DataTable();
            dtvat = Slabs.GetSlabInfo(SlabType.VAT);
            DataRow drvat = dtvat.Rows[0];
            VAT = Convert.ToDouble(drvat[3].ToString());

            LoadComboboxProject(cboProjectName, 0);
            ChangeState(EntryMode.NEW);
            txtSalesReturnID.Visible = false;
            ShowAccClassInTreeView(treeAccClass, null, 0);

            #region BLOCK FOR SHOWING SERIES NAME IN COMBOBOX
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("SLS_RTN");
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                cboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            cboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            cboSeriesName.SelectedIndex = 0;
            #endregion

            #region BLOCK OF SHOWING DEPOT IN COMBOBOX
            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            foreach (DataRow dr in dtDepotInfo.Rows)
            {
                cboDepot.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboDepot.SelectedIndex = 0;
            #endregion
            m_mode = EntryMode.NEW;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            try
            {
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                //Event trigerred when delete button is clicked
                evtDiscount.Click += new EventHandler(Delete_Row_Click);
                //Event when account is selected
                evtProduct.FocusLeft += new EventHandler(Product_Selected);
                //Event when Quntity is selected
                evtProductCode.FocusLeft += new EventHandler(ProductCode_Selected);
                evtQty.FocusLeft += new EventHandler(Qty_Modified);
                //Event when Rate is selected
                evtRate.FocusLeft += new EventHandler(SalesRate_Modified);

                //Event when DiscPercentage is selected
                evtDiscPercentage.FocusLeft += new EventHandler(DiscPercentage_Modified);

                //Event when Discount is selected
                evtDiscount.FocusLeft += new EventHandler(Qty_Modified);

                evtGrossAmt.ValueChanged += new EventHandler(GrossAmount_Modified);

                evtNetAmt.ValueChanged += new EventHandler(NetAmount_Modified);

                //For Loading The Optional Fields
                OptionalFields();
                #region BLOCK FOR SHOWING SALES LEDGER IN COMBOBOX

                //Displaying the all ledgers associated with Sales AccountGroup in DropDownList
                int Sales_ID = AccountGroup.GetGroupIDFromGroupNumber(112);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultSales Account according to user root or other users
                int DefaultSalesAccNum = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_SALES_ACCOUNT") : UserPreference.GetValue("DEFAULT_SALES_ACCOUNT", uid));
                string DefaultSalesName = "";

                //Add sales to comboSalesAccount
                DataTable dtSalesLedgers = Ledger.GetAllLedger(Sales_ID);
                foreach (DataRow drSalesLedgers in dtSalesLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cmboSalesAcc.Items.Add(new ListItem((int)drSalesLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drSalesLedgers["LedgerID"]) == DefaultSalesAccNum)
                        DefaultSalesName = drLedgerInfo["LedName"].ToString();
                }
                cmboSalesAcc.DisplayMember = "value";//This value is  for showing at Load condition
                cmboSalesAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cmboSalesAcc.Text = DefaultSalesName;


                #endregion

                #region BLOCK FOR SHOWING THE LEDGERS OF CASH IN HAND,DEBTOR AND CREDITOR IN CASH/PARTY COMBOBOX
                //cmboCashPartyAcc.Items.Add(new ListItem(0, "Choose Cash/Party Ledger"));//At the first index of combobox show the "Choose Cash/Party Ledger"
                int Cash_In_Hand = AccountGroup.GetGroupIDFromGroupNumber(102);
                DataTable dtCash_In_HandLedgers = Ledger.GetAllLedger(Cash_In_Hand);//Collecting the Ledgers corresponding to Cash_In_Hand group
                foreach (DataRow drCash_In_HandLedgers in dtCash_In_HandLedgers.Rows)
                {
                    cboCashParty.Items.Add(new ListItem((int)drCash_In_HandLedgers["LedgerID"], drCash_In_HandLedgers["EngName"].ToString()));
                }
                int Debtor = AccountGroup.GetGroupIDFromGroupNumber(29);
                DataTable dtDebtorLedgers = Ledger.GetAllLedger(Debtor);
                foreach (DataRow drDebtorLedgers in dtDebtorLedgers.Rows)
                {
                    cboCashParty.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drDebtorLedgers["EngName"].ToString()));
                }
                int Creditor = AccountGroup.GetGroupIDFromGroupNumber(114);
                DataTable dtCreditorLedgers = Ledger.GetAllLedger(Creditor);

                foreach (DataRow drCreditorLedgers in dtCreditorLedgers.Rows)
                {
                    cboCashParty.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drCreditorLedgers["EngName"].ToString()));
                }

                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                foreach (DataRow drBankLedger in dtBankLedgers.Rows)
                {
                    cboCashParty.Items.Add(new ListItem((int)drBankLedger["LedgerID"], drBankLedger["EngName"].ToString()));
                }

                cboCashParty.DisplayMember = "value";//This value is  for showing at Load condition
                cboCashParty.ValueMember = "id";//This value is stored only not to be shown at Load condition
                cboCashParty.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
                #endregion

                #region Another Method



                #endregion

                //Prepare Grid
                grdSalesReturn.Redim(2, 11);

                btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                AddGridHeader();
                //AddRowProduct(1);
                AddRowProduct1(1);

                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID
                if (m_SalesReturnID > 0)
                {
                    //Show the values in fields
                    try
                    {

                        if (m_isRecurring)
                        {
                            ChangeState(EntryMode.NEW);
                        }
                        else
                            ChangeState(EntryMode.NORMAL);
                        int vouchID = 0;
                        try
                        {
                            vouchID = m_SalesReturnID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }

                        Sales m_Sales = new Sales();

                        //Getting the value of SeriesID via MasterID or VouchID
                        int SeriesIDD = m_SalesReturn.GetSeriesIDFromMasterID(vouchID);
                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Sales Return");
                            cboSeriesName.Text = "";
                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            cboSeriesName.Text = dr["EngName"].ToString();

                        }
                        DataTable dtSalesReturnMaster = m_SalesReturn.GetSalesReturnMasterInfo(vouchID.ToString());

                        if (dtSalesReturnMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }
                        foreach (DataRow drSalesReturnMaster in dtSalesReturnMaster.Rows)
                        {
                            txtVoucherNo.Text = drSalesReturnMaster["Voucher_No"].ToString();
                            txtDate.Text = Date.DBToSystem(drSalesReturnMaster["SalesReturn_Date"].ToString());
                            txtRemarks.Text = drSalesReturnMaster["Remarks"].ToString();
                            txtSalesReturnID.Text = drSalesReturnMaster["SalesReturnID"].ToString();
                            txtOrderNo.Text = drSalesReturnMaster["Order_No"].ToString();
                            lblNetAmout.Text = Convert.ToDecimal(drSalesReturnMaster["Net_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            txtSalesTax1.Text =  Convert.ToDecimal(drSalesReturnMaster["Tax1"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));;
                            txtSalesTax2.Text =  Convert.ToDecimal(drSalesReturnMaster["Tax2"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));;
                            txtSalesTax3.Text =  Convert.ToDecimal(drSalesReturnMaster["Tax3"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));;
                            lblVat.Text =        Convert.ToDecimal(drSalesReturnMaster["VAT"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            lblTotalQty.Text = drSalesReturnMaster["Total_Qty"].ToString();
                            lblSpecialDiscount.Text = Convert.ToDecimal(drSalesReturnMaster["SpecialDiscount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            lblGross.Text = Convert.ToDecimal(drSalesReturnMaster["Gross_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                            lblGrandTotal.Text = Convert.ToDecimal(drSalesReturnMaster["Total_Amt"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));




                            //For Additional Fields
                            if (NumberOfFields > 0)
                            {
                                if (NumberOfFields == 1)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = false;
                                    txtsecond.Visible = false;
                                    lblthird.Visible = false;
                                    txtthird.Visible = false;
                                    lblfourth.Visible = false;
                                    txtfourth.Visible = false;
                                    lblfifth.Visible = false;
                                    txtfifth.Visible = false;
                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();

                                    txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                                    txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesReturnMaster["Field5"].ToString();
                                }
                                else if (NumberOfFields == 2)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = true;
                                    txtsecond.Visible = true;
                                    lblthird.Visible = false;
                                    txtthird.Visible = false;
                                    lblfourth.Visible = false;
                                    txtfourth.Visible = false;
                                    lblfifth.Visible = false;
                                    txtfifth.Visible = false;
                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();

                                    txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                                    txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesReturnMaster["Field5"].ToString();
                                }
                                else if (NumberOfFields == 3)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = true;
                                    txtsecond.Visible = true;
                                    lblthird.Visible = true;
                                    txtthird.Visible = true;
                                    lblfourth.Visible = false;
                                    txtfourth.Visible = false;
                                    lblfifth.Visible = false;
                                    txtfifth.Visible = false;
                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                    lblthird.Text = drdtadditionalfield["Field3"].ToString();

                                    txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                                    txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesReturnMaster["Field5"].ToString();

                                }
                                else if (NumberOfFields == 4)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = true;
                                    txtsecond.Visible = true;
                                    lblthird.Visible = true;
                                    txtthird.Visible = true;
                                    lblfourth.Visible = true;
                                    txtfourth.Visible = true;
                                    lblfifth.Visible = false;
                                    txtfifth.Visible = false;
                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();

                                    txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                                    txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesReturnMaster["Field5"].ToString();

                                }
                                else if (NumberOfFields == 5)
                                {
                                    lblfirst.Visible = true;
                                    txtfirst.Visible = true;
                                    lblsecond.Visible = true;
                                    txtsecond.Visible = true;
                                    lblthird.Visible = true;
                                    txtthird.Visible = true;
                                    lblfourth.Visible = true;
                                    txtfourth.Visible = true;
                                    lblfifth.Visible = true;
                                    txtfifth.Visible = true;

                                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();
                                    lblfifth.Text = drdtadditionalfield["Field5"].ToString();

                                    txtfirst.Text = drSalesReturnMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesReturnMaster["Field2"].ToString();
                                    txtthird.Text = drSalesReturnMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesReturnMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesReturnMaster["Field5"].ToString();
                                }


                            }

                            dsSalesReturn.Tables["tblSalesReturnMaster"].Rows.Add(cboSeriesName.Text, drSalesReturnMaster["Voucher_No"].ToString(), Date.DBToSystem(drSalesReturnMaster["SalesReturn_Date"].ToString()), cboDepot.Text, drSalesReturnMaster["Order_No"].ToString(), cboCashParty.Text, cmboSalesAcc.Text, drSalesReturnMaster["Remarks"].ToString());
                            DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesReturnMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drCashPartyLedgerInfo = dtCashPartyInfo.Rows[0];
                            cboCashParty.Text = drCashPartyLedgerInfo["LedName"].ToString();

                            //Show the corresponding Sales Account Ledger in Combobox
                            DataTable dtSalesLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesReturnMaster["SalesLedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drSalesLedgerInfo = dtSalesLedgerInfo.Rows[0];
                            cmboSalesAcc.Text = drSalesLedgerInfo["LedName"].ToString();

                            DataTable dtDepotDtlInfo = Depot.GetDepotInfo(Convert.ToInt32(drSalesReturnMaster["DepotID"].ToString()));
                            foreach (DataRow drDepotInfo in dtDepotDtlInfo.Rows)
                            {
                                cboDepot.Text = drDepotInfo["DepotName"].ToString();
                            }
                        }
                        DataTable dtSalesReturnDetails = m_SalesReturn.GetSalesReturnDetails(vouchID);
                        for (int i = 1; i <= dtSalesReturnDetails.Rows.Count; i++)
                        {
                            DataRow drDetail = dtSalesReturnDetails.Rows[i - 1];
                            grdSalesReturn[i, (int)GridColumn.SNo].Value = i.ToString();
                            grdSalesReturn[i, (int)GridColumn.Code_No].Value = drDetail["Code"].ToString();
                            grdSalesReturn[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                            grdSalesReturn[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
                            grdSalesReturn[i, (int)GridColumn.SalesRate].Value = drDetail["SalesRate"].ToString();
                            grdSalesReturn[i, (int)GridColumn.Amount].Value = drDetail["Amount"].ToString();
                            grdSalesReturn[i, (int)GridColumn.SplDisc_Percent].Value = drDetail["DiscPercentage"].ToString();
                            grdSalesReturn[i, (int)GridColumn.SplDisc].Value = drDetail["Discount"].ToString();
                            grdSalesReturn[i, (int)GridColumn.NetAmount].Value = drDetail["Net_Amount"].ToString();

                            grdSalesReturn[i, (int)GridColumn.ProductID].Value = drDetail["ProductID"].ToString();
                            //AddRowProduct(grdSalesReturn.RowsCount);
                            AddRowProduct1(grdSalesReturn.RowsCount);
                            dsSalesReturn.Tables["tblSalesReturnDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Quantity"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["SalesRate"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());
                        }
                        // if recurring is true then donot load recurring settings for new voucher
                        if (!m_isRecurring)
                            CheckRecurringSetting(txtSalesReturnID.Text);
                    }


                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }

                #endregion


                Tax1checkvalue = Settings.GetSettings("DEFAULT_SALES_TAX1CHECK");
                Tax2checkvalue = Settings.GetSettings("DEFAULT_SALES_TAX2CHECK");
                Tax3checkvalue = Settings.GetSettings("DEFAULT_SALES_TAX3CHECK");
                Tax1value = Settings.GetSettings("DEFAULT_SALES_TAX1");
                Tax2value = Settings.GetSettings("DEFAULT_SALES_TAX2");
                Tax3value = Settings.GetSettings("DEFAULT_SALES_TAX3");

            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdSalesReturn.RowsCount - 2)
                grdSalesReturn.Rows.Remove(ctx.Position.Row);
        }

        /// <summary>
        /// Sums up all NetAmount
        /// </summary>

        private void CalculateNetAmount()
        {
            try
            {
                double VatAmt = 0;
                double NetAmount = 0;
                for (int i = 1; i < grdSalesReturn.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row.If it is ,it must be the last row.

                    if (i == grdSalesReturn.Rows.Count)
                        return;
                    double m_NetAmount = 0;
                    string m_Value = Convert.ToString(grdSalesReturn[i, (int)GridColumn.NetAmount].Value);
                    if (m_Value.Length == 0)
                        m_NetAmount = 0;
                    else
                        m_NetAmount = Convert.ToDouble(grdSalesReturn[i, (int)GridColumn.NetAmount].Value);

                    NetAmount += m_NetAmount;

                    //Also consider 13% VAT too
                    VatAmt = 0;
                    lblNetAmout.Text = NetAmount.ToString();

                    string Vatvalue = Settings.GetSettings("DEFAULT_SALES_VAT");
                    //// int productid = Convert.ToInt32(grdSalesInvoice[i, 11].Value);
                    string pname = grdSalesReturn[i, 3].Value.ToString();
                    int productid = Sales.GetProductIDFromName(pname, Lang.English);
                    DataTable dtcheckvatapplicable = Product.GetProductByID(productid);
                    DataRow drcheckvatapplicable = dtcheckvatapplicable.Rows[0];

                    if (drcheckvatapplicable["IsVatApplicable"].ToString() == "1")
                    {
                        if (Vatvalue == "1")
                        {
                            VatAmt += (NetAmount * VAT) / 100;
                            lblVat.Text = VatAmt.ToString();

                        }
                    }


                }

                #region Calculate other taxes
                //lblNetAmout.Text = (NetAmount + VatAmt).ToString();
                //string Tax1checkvalue = Settings.GetSettings("DEFAULT_SALES_TAX1CHECK");
                //string Tax2checkvalue = Settings.GetSettings("DEFAULT_SALES_TAX2CHECK");
                //string Tax3checkvalue = Settings.GetSettings("DEFAULT_SALES_TAX3CHECK");
                //string Tax1value = Settings.GetSettings("DEFAULT_SALES_TAX1");
                //string Tax2value = Settings.GetSettings("DEFAULT_SALES_TAX2");
                //string Tax3value = Settings.GetSettings("DEFAULT_SALES_TAX3");
                if (Tax1checkvalue == "1")
                {
                    if (Tax1value == "Nt Amt")
                    {
                        // double tax1amt=0;
                        tax1amt = (NetAmount * TAX1) / 100;
                        txtSalesTax1.Text = tax1amt.ToString();
                    }
                    else if (Tax1value == "Gross")
                    {

                        tax1amt = (GAmount * TAX1) / 100;
                        txtSalesTax1.Text = tax1amt.ToString();
                    }
                }
                if (Tax2checkvalue == "1")
                {
                    if (Tax2value == "Nt Amt")
                    {
                        tax2amt = (NetAmount * TAX2) / 100;
                        txtSalesTax2.Text = tax2amt.ToString();
                    }
                    else if (Tax2value == "Tax 1")
                    {
                        tax2amt = (tax1amt * TAX2) / 100;
                        txtSalesTax2.Text = tax2amt.ToString();
                    }
                    else if (Tax2value == "Gross")
                    {
                        tax2amt = (GAmount * TAX2) / 100;
                        txtSalesTax2.Text = tax2amt.ToString();
                    }
                }
                if (Tax3checkvalue == "1")
                {
                    if (Tax3value == "Nt Amt")
                    {
                        tax3amt = (NetAmount * TAX3) / 100;
                        txtSalesTax3.Text = tax3amt.ToString();
                    }
                    else if (Tax3value == "Tax 1")
                    {
                        tax3amt = (tax1amt * TAX3) / 100;
                        txtSalesTax3.Text = tax3amt.ToString();
                    }
                    else if (Tax3value == "Tax 2")
                    {
                        tax3amt = (tax2amt * TAX3) / 100;
                        txtSalesTax3.Text = tax3amt.ToString();
                    }
                    if (Tax3value == "Gross")
                    {
                        tax3amt = (GAmount * TAX3) / 100;
                        txtSalesTax3.Text = tax3amt.ToString();
                    }
                }
                #endregion

                NetAmount += AdjustmentAmount;//Include Adjustment Amount to Net Amount

                double GrandTotal = NetAmount + VatAmt + tax1amt + tax2amt + tax3amt;

                //Write all labels
                lblVat.Text = VatAmt.ToString((Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                lblGrandTotal.Text = GrandTotal.ToString((Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                lblNetAmout.Text = NetAmount.ToString((Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
            }
            catch (Exception ex)
            {

                Global.MsgError("Error in NetAmount calucation!");
            }
        }

        /// <summary>
        /// Sums up all Gross Amount
        /// </summary>
        private void CalculateGrossAmount()
        {
            try
            {

                GrossAmount = 0;
                for (int i = 1; i < grdSalesReturn.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row.If it is ,it must be the last row.

                    if (i == grdSalesReturn.Rows.Count)
                        return;
                    double m_GrossAmount = 0;
                    string m_Value = Convert.ToString(grdSalesReturn[i, (int)GridColumn.Amount].Value);
                    if (m_Value.Length == 0)
                        m_GrossAmount = 0;
                    else
                        m_GrossAmount = Convert.ToDouble(grdSalesReturn[i, (int)GridColumn.Amount].Value);

                    GrossAmount += m_GrossAmount;
                    lblGross.Text = GrossAmount.ToString();

                }
            }
            catch (Exception ex)
            {

                Global.MsgError("Error in Gross Amount calucation!");
            }
        }

        /// <summary>
        /// Sums up all Quantity
        /// </summary>
        /// 
        private void CalculateTotalQuantity()
        {
            try
            {
                double TotalQuantity = 0;
                for (int i = 1; i < grdSalesReturn.RowsCount - 1; i++)
                {
                    if (i == grdSalesReturn.Rows.Count)
                        return;
                    double m_TotalQuantity = 0;
                    string m_Value = Convert.ToString(grdSalesReturn[i, (int)GridColumn.Qty].Value);
                    if (m_Value.Length == 0)
                        m_TotalQuantity = 0;
                    else
                        m_TotalQuantity = (Convert.ToDouble(grdSalesReturn[i, (int)GridColumn.Qty].Value));

                    TotalQuantity += m_TotalQuantity;

                    lblTotalQty.Text = TotalQuantity.ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                //Global.MsgError("Error in Total Quantity calucation!"); 
            }
        }
        /// <summary>
        /// Sum up all discount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateTotalDiscount()
        {
            try
            {
                Double TotalDiscount = 0;
                for (int i = 1; i < grdSalesReturn.RowsCount - 1; i++)
                {
                    if (i == grdSalesReturn.Rows.Count)
                        return;
                    double m_TotalDiscount = 0;
                    string m_Value = Convert.ToString(grdSalesReturn[i, (int)GridColumn.SplDisc].Value);
                    if (m_Value.Length == 0)
                        m_TotalDiscount = 0;
                    else
                        m_TotalDiscount = Convert.ToDouble(grdSalesReturn[i, (int)GridColumn.SplDisc].Value);

                    TotalDiscount += m_TotalDiscount;

                    lblSpecialDiscount.Text = TotalDiscount.ToString();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError("Error in Total Discount calucation!");
            }
        }

        private void ProductCode_Selected(object sender, EventArgs e)
        {
            #region Language Management


            string LangField = "English";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;

            }
            #endregion
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                //Global.Msg(ex.Message);
            }
            int CurRow = grdSalesReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];
            DataTable dt = Product.GetProductByCode(ct.DisplayText);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row
                    grdSalesReturn[(CurRow), 3].Value = dr[LangField].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double SalesRate = Math.Round(Convert.ToDouble(dr["SalesRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.
                    grdSalesReturn[(CurRow), (int)GridColumn.SalesRate].Value = SalesRate.ToString();
                    grdSalesReturn[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
            }
            int RowsCount = grdSalesReturn.RowsCount;
            string LastProductCell = (string)grdSalesReturn[RowsCount - 1, (int)GridColumn.ProductName].Value;
            ////Check whether the new row is already added
            if (LastProductCell != "(NEW)")
            {
                //AddRowProduct(RowsCount);
                AddRowProduct1(RowsCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowsCount - 1);
            }
            WriteAmount(CurRow, (int)GridColumn.SNo);//Write amount on grid'cell when quantity is unit
            WriteNetAmount(CurRow);//Write Net amount on corresponding cell of grid.It can also handle when value of quantity is unit and discount is 0
            CalculateGrossAmount();//After summing up all gross amount,this function display the value in label
            CalculateNetAmount(); //After summing up all net amount,this function display the value in lablel
            CalculateTotalQuantity();
        }

        private void Product_Selected(object sender, EventArgs e)
        {
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                //Global.Msg(ex.Message);
            }
            int CurRow = grdSalesReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Using the name find corresponding code
            //DataTable dt = Product.GetProductByName(ct.DisplayText);
            DataTable dt = Product.GetProductByName(ct.DisplayText);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row
                    grdSalesReturn[(CurRow), (int)GridColumn.Code_No].Value = dr["ProductCode"].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double SalesRate = Math.Round(Convert.ToDouble(dr["SalesRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.
                    grdSalesReturn[(CurRow), (int)GridColumn.SalesRate].Value = SalesRate.ToString();
                    grdSalesReturn[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
            }
            int RowsCount = grdSalesReturn.RowsCount;
            string LastProductCell = (string)grdSalesReturn[RowsCount - 1, (int)GridColumn.ProductName].Value;
            ////Check whether the new row is already added
            if (LastProductCell != "(NEW)")
            {
                //AddRowProduct(RowsCount);
                AddRowProduct1(RowsCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowsCount - 1);
            }
            WriteAmount(CurRow, (int)GridColumn.SNo);//Write amount on grid'cell when quantity is unit
            WriteNetAmount(CurRow);//Write Net amount on corresponding cell of grid.It can also handle when value of quantity is unit and discount is 0
            CalculateGrossAmount();//After summing up all gross amount,this function display the value in label
            CalculateNetAmount(); //After summing up all net amount,this function display the value in lablel
            CalculateTotalQuantity();
        }

        private void WriteAmount(int CurRow, double Qty)
        {
            //string SalesRate = grdSalesReturn[CurRow, (int)GridColumn.SalesRate].Value.ToString();
            object salesRate = grdSalesReturn[CurRow, (int)GridColumn.SalesRate].Value;

            string SalesRate = (salesRate != null) ? salesRate.ToString() : "0";
            double m_SalesRate = 0;

            try
            {
                m_SalesRate = Convert.ToDouble(SalesRate);
            }
            catch (Exception)
            {
            }
            double Amount = Convert.ToDouble(Qty) * m_SalesRate;
            grdSalesReturn[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
        }

        private void WriteNetAmount(int CurRow)
        {
            string Amount = grdSalesReturn[CurRow, (int)GridColumn.Amount].Value.ToString();
            string Discount = grdSalesReturn[CurRow, (int)GridColumn.SplDisc].Value.ToString();
            double NetAmount = Convert.ToDouble(Amount) - Convert.ToDouble(Discount);
            grdSalesReturn[CurRow, (int)GridColumn.NetAmount].Value = NetAmount.ToString();
        }

        /// <summary>
        /// Check whether the Current Row is a new row or not
        /// </summary>
        /// <param name="CurRow"></param>
        /// <returns></returns>
        /// 

        private bool isNewRow(int CurRow)
        {
            if (grdSalesReturn[CurRow, (int)GridColumn.Code_No].Value == "(NEW)")
                return true;
            else
                return false;
        }

        private void Qty_Modified(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext ct = new SourceGrid.CellContext();
                try
                {
                    ct = (SourceGrid.CellContext)sender;
                    if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                        return;
                }
                catch (Exception ex)
                {
                    //Global.Msg(ex.Message);
                }
                //find the current row of source grid
                int CurRow = grdSalesReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];

                if (isNewRow(CurRow))
                    return;

                int RowCount = grdSalesReturn.RowsCount;
                object Qty = ((TextBox)sender).Text;
                //bool IsInt = Misc.IsInt(Qty, false);//This function check whether variable is integer or not?
                //if (IsInt == false)
                //{
                //    Global.MsgError("The quantity you posted is invalid! Please post the integer value");
                //    grdSalesReturn[CurRow, (int)GridColumn.Amount].Value = "";
                //    grdSalesReturn[CurRow, (int)GridColumn.Qty].Value = "1";
                //    return;
                //}

                ////Check whether the value of quantity is zero or not?
                if (Convert.ToDouble(Qty) <= 0)
                {
                    Global.MsgError("The Quantity shouldnot be zero. Fill the Quantity first!");
                    grdSalesReturn[CurRow, (int)GridColumn.Qty].Value = "1";
                    grdSalesReturn[CurRow, (int)GridColumn.Amount].Value = "0";
                    ((TextBox)sender).Text = "1";
                    return;
                }

                WriteAmount(CurRow, Convert.ToDouble(Qty));
                CalculateGrossAmount();

                //Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
                WriteNetAmount(CurRow);
                CalculateNetAmount();
                CalculateTotalQuantity();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// This function help to edit the Rate.If anyone want to insert the rate according to his/her choice then this fuction does work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SalesRate_Modified(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext ct = new SourceGrid.CellContext();
                try
                {
                    ct = (SourceGrid.CellContext)sender;
                    if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                        return;
                }
                catch (Exception ex)
                {
                    //Global.Msg(ex.Message);
                }
                //find the current row of source grid
                int CurRow = grdSalesReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;

                int RowCount = grdSalesReturn.RowsCount;
                object SalesRate = ((TextBox)sender).Text;
                bool IsDouble = Misc.IsNumeric(SalesRate);//This function check whether variable is Double  or not?
                if (IsDouble == false)
                {
                    Global.MsgError("The Sales Rate you posted is invalid! Please post the integer value");
                    grdSalesReturn[CurRow, (int)GridColumn.Amount].Value = "";
                    grdSalesReturn[CurRow, (int)GridColumn.Qty].Value = "1.0";
                    return;
                }
                string Qty = grdSalesReturn[CurRow, (int)GridColumn.Qty].Value.ToString();
                double Amount = Convert.ToDouble(Qty) * Convert.ToDouble(SalesRate);
                grdSalesReturn[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
                CalculateGrossAmount();
                WriteNetAmount(CurRow);
                CalculateNetAmount();
                CalculateTotalQuantity();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void GrossAmount_Modified(object sender, EventArgs e)
        {
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                //Global.Msg(ex.Message);
            }
            Global.Msg("hi to all");
            int CurRow = grdSalesReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
            if (isNewRow(CurRow))
                return;

        }

        private void NetAmount_Modified(object sender, EventArgs e)
        {
            Global.Msg("Net Modified");
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                //Global.Msg(ex.Message);
            }
        }

        private void DiscPercentage_Modified(object sender, EventArgs e)
        {
            try
            {

                SourceGrid.CellContext ct = new SourceGrid.CellContext();
                try
                {
                    ct = (SourceGrid.CellContext)sender;
                    if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                        return;
                }
                catch (Exception ex)
                {
                    //Global.Msg(ex.Message);
                }
                int CurRow = grdSalesReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
                if (isNewRow(CurRow))
                    return;
                double Amount = 0;
                Amount = Convert.ToDouble(grdSalesReturn[CurRow, 6].Value);

                //if (Amount == 0)
                //{
                //    Global.MsgError("The Amount shouldnot be Zero.Please post the required value in corresponding field!");
                //    grdSalesReturn[CurRow, 7].Value = "0";
                //    grdSalesReturn[CurRow, 8].Value = "0";
                //    grdSalesReturn[CurRow, 9].Value = "0";
                //    return;
                //}
                object DisPercentage = ((TextBox)sender).Text;
                bool IsDouble = Misc.IsNumeric(DisPercentage);//This function check whether variable is double or not?
                if (IsDouble == false)
                {
                    Global.MsgError("The Discount Percentage you posted is invalid! Please post the  numeric value");
                    return;
                }
                double Discount = Math.Round(((Amount * Convert.ToDouble(DisPercentage)) / 100), Global.DecimalPlaces);

                //double NetAmount = Math.Round((Amount - Discount), Global.DecimalPlaces);


                grdSalesReturn[(CurRow), (int)GridColumn.SplDisc].Value = Discount.ToString();
                WriteNetAmount(CurRow);
                CalculateNetAmount();
                CalculateTotalDiscount();

            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }

        private void Discount_Modified(object sender, EventArgs e)
        {

            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;
                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception ex)
            {
                //Global.Msg(ex.Message);
            }
            int CurRow = grdSalesReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
            if (isNewRow(CurRow))
                return;
            double Amount = 0;
            Amount = Convert.ToDouble(grdSalesReturn[CurRow, (int)GridColumn.Amount].Value);
            if (Amount == 0)
            {
                Global.MsgError("The Amount shouldnot be Zero. Please post the required value in corresponding field!");
                grdSalesReturn[CurRow, (int)GridColumn.SplDisc_Percent].Value = "0";
                grdSalesReturn[CurRow, (int)GridColumn.SplDisc].Value = "0";
                grdSalesReturn[CurRow, (int)GridColumn.NetAmount].Value = "0";
                return;
            }
            object Discount = ((TextBox)sender).Text;
            bool IsDouble = Misc.IsNumeric(Discount);//This function check whether variable is double or not?
            if (IsDouble == false)
            {
                Global.MsgError("The Discount Amount you posted is invalid! Please post the  numeric value");
                return;
            }
            double DiscPercentage = Math.Round(((Convert.ToDouble(Discount) * 100) / Amount), Global.DecimalPlaces);
            //double NetAmount = Math.Round((Amount - Convert.ToDouble(Discount)), Global.DecimalPlaces);

            grdSalesReturn[(CurRow), (int)GridColumn.SplDisc_Percent].Value = DiscPercentage.ToString();
            WriteNetAmount(CurRow);
            CalculateNetAmount();
            CalculateTotalDiscount();
        }

        private void ClearNew(int RowCount)
        {
            ////if (grdProduct[RowCount, 2].Value == "(NEW)")
            ////    grdProduct[RowCount, 2].Value = "";
            //if (grdProduct[RowCount, 3].Value == "(NEW)")
            //    grdProduct[RowCount, 3].Value = "";
            //if (grdProduct[RowCount, 4].Value == "(NEW)")
            //    grdProduct[RowCount, 4].Value = "";
            //if (grdProduct[RowCount, 5].Value == "(NEW)")
            //    grdProduct[RowCount, 5].Value = "";
            //if (grdProduct[RowCount, 6].Value == "(NEW)")
            //    grdProduct[RowCount, 6].Value = "";
            //if (grdProduct[RowCount, 7].Value == "(NEW)")
            //    grdProduct[RowCount, 7].Value = "";
        }

        private void AddGridHeader()
        {
            grdSalesReturn[0, (int)GridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del");
            grdSalesReturn[0, (int)GridColumn.Del].Column.Width = 25;

            grdSalesReturn[0, (int)GridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S.No.");

            grdSalesReturn[0, (int)GridColumn.Code_No] = new SourceGrid.Cells.ColumnHeader("Code");
            grdSalesReturn[0, (int)GridColumn.Code_No].Column.Width = 100;

            grdSalesReturn[0, (int)GridColumn.ProductName] = new SourceGrid.Cells.ColumnHeader("Product Name");
            grdSalesReturn[0, (int)GridColumn.ProductName].Column.Width = 120;

            grdSalesReturn[0, (int)GridColumn.Qty] = new SourceGrid.Cells.ColumnHeader("Qty");
            grdSalesReturn[0, (int)GridColumn.Qty].Column.Width = 100;

            grdSalesReturn[0, (int)GridColumn.SalesRate] = new SourceGrid.Cells.ColumnHeader("SalesRate");
            grdSalesReturn[0, (int)GridColumn.SalesRate].Column.Width = 150;

            grdSalesReturn[0, (int)GridColumn.Amount] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdSalesReturn[0, (int)GridColumn.Amount].Column.Width = 150;

            grdSalesReturn[0, (int)GridColumn.SplDisc_Percent] = new SourceGrid.Cells.ColumnHeader("Spl. Disc%");
            grdSalesReturn[0, (int)GridColumn.SplDisc_Percent].Column.Width = 150;

            grdSalesReturn[0, (int)GridColumn.SplDisc] = new SourceGrid.Cells.ColumnHeader("Spl. Disc");
            grdSalesReturn[0, (int)GridColumn.SplDisc].Column.Width = 100;

            grdSalesReturn[0, (int)GridColumn.NetAmount] = new SourceGrid.Cells.ColumnHeader("Net Amount");
            grdSalesReturn[0, (int)GridColumn.NetAmount].Column.Width = 150;

            grdSalesReturn[0, (int)GridColumn.ProductID] = new SourceGrid.Cells.ColumnHeader("ProductID");
            grdSalesReturn[0, (int)GridColumn.ProductID].Column.Width = 15;
            grdSalesReturn[0, (int)GridColumn.ProductID].Column.Visible = false;

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_RETURN_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Sales Return?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);
            //}

            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_RETURN_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Sales Return?") == DialogResult.Yes)
            //        btnSave_Click(sender, e); 
            //}
            Navigation(Navigate.Next);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_RETURN_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Sales Return?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);
            //}

            Navigation(Navigate.Last);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_RETURN_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            isNew = true;
            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);

            //isNew = true;
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
            //    return;
            //}
            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
            IsFieldChanged = false;
            cboSeriesName.SelectedIndex = 0;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_RETURN_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            isNew = false;
            OldGrid = " ";
            OldGrid = OldGrid + "Voucher No" + txtVoucherNo + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Cash/Party" + cboCashParty.Text + "Depot" + cboDepot.Text + "OrderNo" + txtOrderNo.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdSalesReturn.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string productname = grdSalesReturn[i + 1, (int)GridColumn.ProductName].Value.ToString();
                string qty = grdSalesReturn[i + 1, (int)GridColumn.Qty].Value.ToString();
                string rate = grdSalesReturn[i + 1, (int)GridColumn.SalesRate].Value.ToString();
                string amt = grdSalesReturn[i + 1, (int)GridColumn.Amount].Value.ToString();
                OldGrid = OldGrid + string.Concat(productname, qty, rate, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;
            EnableControls(true);
            ChangeState(EntryMode.EDIT);

            //if automatic voucher number increment is selected
            string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
            if (NumberingType == "AUTOMATIC")
                txtVoucherNo.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (SalesInvoiceIDCopy > 0)
            {
                if (m_mode == EntryMode.NEW)
                {
                    Navigation(Navigate.ID);
                    EnableControls(true);
                    ChangeState(EntryMode.NEW);
                }
                else
                {
                    Global.Msg("Please press New Button for Pasting the copied Voucher");
                }
            }
            else
            {
                Global.Msg("Please navigate to a voucher and press copy button first to specify the source voucher");
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            SalesInvoiceIDCopy = Convert.ToInt32(txtSalesReturnID.Text);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void cboProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSalesReturnID_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    SeriesID = (ListItem)cboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));
                    txtVoucherNo.Enabled = true;
                    if (NumberingType == "AUTOMATIC" && !m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            // disables all controls except cboSeriesName
                            ClearSalesReturn(true);
                            EnableControls(false);
                            cboSeriesName.Enabled = true;
                        }
                        else
                        {
                            lblVouNo.Visible = true;
                            txtVoucherNo.Visible = true;
                            EnableControls(true);
                            txtVoucherNo.Text = m_vounum.ToString();
                            txtVoucherNo.Enabled = false;
                        }
                    }
                    else if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        lblVouNo.Visible = false;
                        txtVoucherNo.Visible = false;
                    }
                    if (m_SalesReturnID > 0 && !m_isRecurring)
                    {
                        lblVouNo.Visible = true;
                        txtVoucherNo.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtSalesTax2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSalesTax3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSalesTax1_TextChanged(object sender, EventArgs e)
        {

        }

        private void treeAccClass_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void txtRemarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtOrderNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDate_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void cmboSalesAcc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboCashParty_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtVoucherNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboDepot_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddRowProduct1(int RowCount)
        {
            //Add a new row
            try
            {
                grdSalesReturn.Redim(Convert.ToInt32(RowCount + 1), grdSalesReturn.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdSalesReturn[i, (int)GridColumn.Del] = btnDelete;
                grdSalesReturn[i, (int)GridColumn.Del].AddController(evtDelete);
                grdSalesReturn[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdSalesReturn[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();

                #region Language Management


                string LangField = "English";
                switch (LangMgr.DefaultLanguage)
                {
                    case Lang.English:
                        LangField = "EngName";
                        break;
                    case Lang.Nepali:
                        LangField = "NepName";
                        break;

                }
                #endregion
                SourceGrid.Cells.Editors.TextBox txtProduct = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtProduct.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdSalesReturn[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell("", txtProduct);
                txtProduct.Control.GotFocus += new EventHandler(Product_Focused);
                txtProduct.Control.LostFocus += new EventHandler(Product_Leave);
                txtProduct.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                txtProduct.Control.TextChanged += new EventHandler(Text_Change);
                // txtProduct.Control.LostFocus += new EventHandler(Product_Selected);
                grdSalesReturn[i, (int)GridColumn.ProductName].AddController(evtProductFocusLost);
                grdSalesReturn[i, (int)GridColumn.ProductName].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdSalesReturn[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("1", txtQty);
                //grdPurchaseInvoice[i, 4].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(SalesRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;

                grdSalesReturn[i, (int)GridColumn.SalesRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                // grdPurchaseInvoice[i, 5].Value = "(NEW)";

                //grdPurchaseReturn[i, 6] = new SourceGrid.Cells.Cell("");
                //grdPurchaseReturn[i, 6].AddController(evtGrossAmt);
                grdSalesReturn[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell();
                // grdPurchaseInvoice[i, 6].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);
                txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                txtDiscPercentage.Control.Text = "0";

                grdSalesReturn[i, (int)GridColumn.SplDisc_Percent] = new SourceGrid.Cells.Cell("", txtDiscPercentage);
                grdSalesReturn[i, (int)GridColumn.SplDisc_Percent].Value = "0";
                SourceGrid.Cells.Editors.TextBox txtDiscount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscount.Control.LostFocus += new EventHandler(Discount_Modified);
                txtDiscount.EditableMode = SourceGrid.EditableMode.Focus;

                grdSalesReturn[i, (int)GridColumn.SplDisc] = new SourceGrid.Cells.Cell("0", txtDiscount);
                grdSalesReturn[i, (int)GridColumn.SplDisc].Value = "0";

                grdSalesReturn[i, (int)GridColumn.NetAmount] = new SourceGrid.Cells.Cell("Net Amount");
                grdSalesReturn[i, (int)GridColumn.NetAmount].Value = "0";

                grdSalesReturn[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell("ProductID");
                grdSalesReturn[i, (int)GridColumn.ProductID].Value = "0";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Product_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                frmListOfProduct flp = new frmListOfProduct(this);
                flp.ShowDialog();
                SendKeys.Send("{Tab}");

            }
        }
        private void Product_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            int RowsCount = grdSalesReturn.RowsCount;
            string LastServicesCell = (string)grdSalesReturn[RowsCount - 1, (int)GridColumn.ProductName].Value;

            ////Check whether the new row is already added
            if (LastServicesCell != "(NEW)")
            {
                //AddRowProduct(RowsCount);
                AddRowProduct1(RowsCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowsCount - 1);
            }
        }
        private void Product_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmListOfProduct flp = new frmListOfProduct(this, e);
            flp.ShowDialog();
        }
        private void Text_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;

        }
        private void evtProductFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            // FillAllGridRow(CurrRowPos);
        }
        public void AddProduct(int productid, string productcode, string productname, bool IsSelected, double purchaserate, double salesrate, int qty, int defaultUnitID)
        {

            //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //CurrRowPos = ctx.Position.Row;
            if (IsSelected)
            {
                int CurRow = grdSalesReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];
                // CurrRowPos = ctx.Position.Row;
                ctx.Text = productname;
                purchaserate = salesrate;
                productcode = productcode;
                grdSalesReturn[CurRow, (int)GridColumn.Code_No].Value = productcode;
                grdSalesReturn[CurRow, (int)GridColumn.SalesRate].Value = purchaserate;
                grdSalesReturn[CurRow, (int)GridColumn.ProductID].Value = productid.ToString();

                int RowsCount = grdSalesReturn.RowsCount;

                string LastServicesCell = (string)grdSalesReturn[RowsCount - 1, (int)GridColumn.ProductName].Value;
                ////Check whether the new row is already added
                if (LastServicesCell == "(NEW)")
                {
                    // AddRowProduct(RowsCount);
                    AddRowProduct1(RowsCount);
                    //Clear (NEW) on other colums as well
                    //ClearNew(RowsCount - 1);
                }

            }
            hasChanged = true;



        }
        private void FillAllGridRow(int RowPosition)
        {

        }
        private void PurchaseRate_Modified(object sender, EventArgs e)
        {
            try
            {

                SourceGrid.CellContext ct = new SourceGrid.CellContext();
                try
                {
                    ct = (SourceGrid.CellContext)sender;
                    if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                        return;
                }
                catch (Exception ex)
                {
                    //Global.Msg(ex.Message);
                }
                //find the current row of source grid
                int CurRow = grdSalesReturn.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;

                int RowCount = grdSalesReturn.RowsCount;
                object PurchaseRate = ((TextBox)sender).Text;
                bool IsDouble = Misc.IsNumeric(PurchaseRate);//This function check whether variable is Double  or not?
                if (IsDouble == false)
                {
                    Global.MsgError("The Sales Rate you posted is invalid! Please post the integer value");
                    grdSalesReturn[CurRow, (int)GridColumn.Amount].Value = "";
                    grdSalesReturn[CurRow, (int)GridColumn.Qty].Value = "1";
                    return;
                }

                string Qty = grdSalesReturn[CurRow, (int)GridColumn.Qty].Value.ToString();
                double Amount = Convert.ToDouble(Qty) * Convert.ToDouble(PurchaseRate);
                grdSalesReturn[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
                CalculateGrossAmount();
                WriteNetAmount(CurRow);
                CalculateNetAmount();
                CalculateTotalQuantity();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void frmSalesReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.N && e.Control)
            {
                btnNew_Click(sender, e);
            }
            else if (e.KeyCode == Keys.E && e.Control)
            {
                btnEdit_Click(sender, e);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                btnSave_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete && e.Control)
            {
                //  btnDelete_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F && e.Control)
            {
                btnFirst_Click(sender, e);
            }
            else if (e.KeyCode == Keys.P && e.Shift)
            {
                btnPrev_Click(sender, e);
            }
            else if (e.KeyCode == Keys.N && e.Shift)
            {
                btnNext_Click(sender, e);
            }
            else if (e.KeyCode == Keys.L && e.Control)
            {
                btnLast_Click(sender, e);
            }
            else if (e.KeyCode == Keys.C && e.Control)
            {
                btnCopy_Click(sender, e);
            }
            else if (e.KeyCode == Keys.V && e.Control)
            {
                btnPaste_Click(sender, e);
            }
            else if (e.KeyCode == Keys.P && e.Control)
            {
                button3_Click(sender, e);
            }
        }
        private void LoadComboboxProject(ComboBox ComboBoxControl, int ProjectID)
        {
            #region Language Management
            string LangField = "EngName";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;
            }
            #endregion
            DataTable dt = Project.GetProjectTable(ProjectID);
            //DataTable dt1 = AccountClass.GetAccClassTable(ProjectID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], Prefix + " " + dr[LangField].ToString()));
                Prefix += "----";
                LoadComboboxProject(ComboBoxControl, Convert.ToInt16(dr["ProjectID"].ToString()));
            }
            //Prefix = "--";
            if (Prefix.Length > 1)
            {
                Prefix = Prefix.Remove(Prefix.Length - 4, 4);
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }
        private void OptionalFields()
        {
            SeriesID = (ListItem)cboSeriesName.SelectedItem;
            DataTable dtadditionalfield = Sales.GetAdditionalFields(SeriesID.ID);
            drdtadditionalfield = dtadditionalfield.Rows[0];
            NumberOfFields = Convert.ToInt32(drdtadditionalfield["NumberOfField"].ToString());
            if (NumberOfFields > 0)
            {
                if (NumberOfFields == 1)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = false;
                    txtsecond.Visible = false;
                    lblthird.Visible = false;
                    txtthird.Visible = false;
                    lblfourth.Visible = false;
                    txtfourth.Visible = false;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                }
                else if (NumberOfFields == 2)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = false;
                    txtthird.Visible = false;
                    lblfourth.Visible = false;
                    txtfourth.Visible = false;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                }
                else if (NumberOfFields == 3)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = true;
                    txtthird.Visible = true;
                    lblfourth.Visible = false;
                    txtfourth.Visible = false;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    lblthird.Text = drdtadditionalfield["Field3"].ToString();

                }
                else if (NumberOfFields == 4)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = true;
                    txtthird.Visible = true;
                    lblfourth.Visible = true;
                    txtfourth.Visible = true;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();

                }
                else if (NumberOfFields == 5)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = true;
                    txtthird.Visible = true;
                    lblfourth.Visible = true;
                    txtfourth.Visible = true;
                    lblfifth.Visible = true;
                    txtfifth.Visible = true;

                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();
                    lblfifth.Text = drdtadditionalfield["Field5"].ToString();
                }
            }
            else
            {
                lblfirst.Visible = false;
                txtfirst.Visible = false;
                lblsecond.Visible = false;
                txtsecond.Visible = false;
                lblthird.Visible = false;
                txtthird.Visible = false;
                lblfourth.Visible = false;
                txtfourth.Visible = false;
                lblfifth.Visible = false;
                txtfifth.Visible = false;
            }

        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }
        private void PrintPreviewCR(PrintType myPrintType)
        {
            dsSalesReturn.Clear();
            rptSalesReturn rpt = new rptSalesReturn();
            Misc.WriteLogo(dsSalesReturn, "tblImage");
            rpt.SetDataSource(dsSalesReturn);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPrintDate = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            int uid = User.CurrUserID;
            DataTable dtroleinfo = User.GetUserInfo(uid);
            DataRow drrole = dtroleinfo.Rows[0];
            int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());

            if (roleid == 37)//if user is root, get information from tblCompany
            {

                pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Name);
                rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                //pdvCompany_Address.Value = m_CompanyDetails.Address1;
                pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
            }
            else //if user is not root, take information from tblUserPreference
            {
                string companyname = UserPreference.GetValue("COMPANY_NAME", uid);
                string companyaddress = UserPreference.GetValue("COMPANY_ADDRESS", uid);
                string companycity = UserPreference.GetValue("COMPANY_CITY", uid);
                string companypan = UserPreference.GetValue("COMPANY_PAN", uid);
                string companyphone = UserPreference.GetValue("COMPANY_PHONE", uid);
                string companyslogan = UserPreference.GetValue("COMPANY_SLOGAN", uid);

                pdvCompany_Name.Value = companyname;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Name);
                rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = companypan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            }
            pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvPreparedBy);
            rpt.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

            pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvCheckedBy);
            rpt.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

            pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvApprovedBy);
            rpt.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);

            pdvPrintDate.Value = Date.ToSystem(DateTime.Now);
            pvCollection.Clear();
            pvCollection.Add(pdvPrintDate);
            rpt.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);
            bool empty = Navigation(Navigate.ID);
            if (empty == false)
            {
                return;
            }
            else
            {
                // Navigation(Navigate.ID);

                Common.frmReportViewer frm = new Common.frmReportViewer();
                frm.SetReportSource(rpt);
                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;
                switch (prntDirect)
                {
                    case 1:
                        rpt.PrintOptions.PrinterName = "";
                        rpt.PrintToPrinter(1, false, 0, 0);
                        prntDirect = 0;
                        return;
                    case 2:
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        rpt.Export();
                        rpt.Close();
                        return;
                    case 3:
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        rpt.Export();
                        rpt.Close();
                        return;
                    case 4:
                        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                        rpt.Export();
                        Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        rpt.Close();
                        return;
                    default:
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }

                // frm.Show();
                frm.WindowState = FormWindowState.Maximized;

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            if (!CheckAmtAgainstRefAmt())
                return;

            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_RETURN_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            string GridValues = "";
            GridValues = GridValues + "Voucher No" + "-" + txtVoucherNo.Text + "," + "Voucher Date" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Cash/Party" + "-" + cboCashParty.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdSalesReturn.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string productname = grdSalesReturn[i + 1, (int)GridColumn.ProductName].Value.ToString();
                string qty = grdSalesReturn[i + 1, (int)GridColumn.Qty].Value.ToString();
                string rate = grdSalesReturn[i + 1, (int)GridColumn.SalesRate].Value.ToString();
                string amt = grdSalesReturn[i + 1, (int)GridColumn.Amount].Value.ToString();
                GridValues = GridValues + string.Concat(productname, qty, rate, amt);
            }
            GridValues = "GridValues" + GridValues;
            ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            try
            {
                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the Invoice - " + txtSalesReturnID.Text + "?") == DialogResult.Yes)
                {

                    SalesReturn Delsalesreturn = new SalesReturn();
                    // delete reference
                    string res = VoucherReference.DeleteReference(Convert.ToInt32(txtSalesReturnID.Text), "SLS_RTN");
                    if (res != "Success")
                    {
                        Global.MsgError("Unable to delete the voucher due to " + res + "! \n You must delete all other vouchers with reference against this voucher to delete this transaction!");
                        return;
                    }

                    //Sales DelSales = new Sales();
                    if (Delsalesreturn.RemoveSalesReturnEntry(Convert.ToInt32(txtSalesReturnID.Text), GridValues))
                    {
                        Global.Msg("Return -" + txtSalesReturnID.Text + " deleted successfully!");
                        // Navigate to 1 step previous
                        if (!this.Navigation(Navigate.Prev))
                        {
                            //This must be because there are no records or this was the first one
                            //If this was the first, try to navigate to second
                            if (!this.Navigation(Navigate.Next))
                            {
                                //This was the last one, there are no records left. Simply clear the form and stay calm
                                ChangeState(EntryMode.NEW);
                            }
                        }
                    }
                    else
                        Global.MsgError("There was an error while deleting Return -" + txtSalesReturnID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Menu_Export = new ContextMenu();
            MenuItem mnuExcel = new MenuItem();
            mnuExcel.Name = "mnuExcel";
            mnuExcel.Text = "E&xcel";
            MenuItem mnuPDF = new MenuItem();
            mnuPDF.Name = "mnuPDF";
            mnuPDF.Text = "&PDF";
            MenuItem mnuEmail = new MenuItem();
            mnuEmail.Name = "mnuEmail";
            mnuEmail.Text = "E&mail";

            Menu_Export.MenuItems.Add(mnuExcel);
            Menu_Export.MenuItems.Add(mnuPDF);
            Menu_Export.MenuItems.Add(mnuEmail);
            Menu_Export.Show(btnExport, new Point(0, btnExport.Height));

            foreach (MenuItem Item in Menu_Export.MenuItems)
                Item.Click += new EventHandler(Menu_Click);
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            switch (((MenuItem)sender).Name)
            {
                case "mnuExcel":
                    //Code for excel export
                    SaveFileDialog SaveFD = new SaveFileDialog();
                    SaveFD.InitialDirectory = "D:";
                    SaveFD.Title = "Enter Filename:";
                    SaveFD.Filter = "*.xls|*.xls";
                    if (SaveFD.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFD.FileName;
                        FileName = SaveFD.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 2;
                    button3_Click(sender, e);
                    break;
                case "mnuPDF":
                    //Code for pdf export
                    SaveFileDialog SaveFDPdf = new SaveFileDialog();
                    SaveFDPdf.InitialDirectory = "D:";
                    SaveFDPdf.Title = "Enter Filename:";
                    SaveFDPdf.Filter = "*.pdf|*.pdf";
                    if (SaveFDPdf.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFDPdf.FileName;
                        FileName = SaveFDPdf.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 3;
                    button3_Click(sender, e);
                    break;
                case "mnuEmail":
                    //Code for pdf export
                    SaveFileDialog SaveFDExcelEmail = new SaveFileDialog();
                    SaveFDExcelEmail.InitialDirectory = "D:";
                    SaveFDExcelEmail.Title = "Enter Filename:";
                    SaveFDExcelEmail.Filter = "*.xls|*.xls"; ;
                    if (SaveFDExcelEmail.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFDExcelEmail.FileName;
                        FileName = SaveFDExcelEmail.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 4;
                    button3_Click(sender, e);
                    break;
            }

        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            m_MDIForm.OpenFormArrayParam("frmAccClass");
        }

        #region methods related to recurring

        DataTable m_dtRecurringSetting = new DataTable();
        public void GetRecurringSetting(DataTable dt)
        {
            if (dt == null)
                chkRecurring.Checked = false; // if cancel button is clicked then the chkrecurring is unchecked

            else
                this.m_dtRecurringSetting = dt;
        }
        private void ClearRecurringSetting()
        {
            m_dtRecurringSetting.Rows.Clear();
            chkRecurring.Checked = false;
            recurringVoucherID = "";
            RSID = "";
        }
        private void chkRecurring_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkRecurring.Checked && m_dtRecurringSetting.Rows.Count == 0))
            {
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "SALES_RETURN", m_dtRecurringSetting);
                fvr.ShowDialog();
                if (m_dtRecurringSetting.Rows.Count == 0)  // if settings are not available then uncheck the checkbox
                    chkRecurring.Checked = false;
            }
            else if (chkRecurring.Checked == false && m_dtRecurringSetting.Rows.Count > 0) //if previously saved settings are available
            {
                //if (txtSalesInvoiceID.Text != "" || txtSalesInvoiceID != null)
                //{
                if (Global.MsgQuest("Are you sure you want to delete the saved recurring voucher settings?") == DialogResult.Yes)
                {
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "SALES_RETURN"); // delete from database
                    ClearRecurringSetting();  // clear local variables
                }
                else
                    chkRecurring.Checked = true;

                //}
                //else
                //    ClearRecurringSetting();
            }
            if ((chkRecurring.Checked == true && m_mode == EntryMode.EDIT) || (chkRecurring.Checked == true && m_mode == EntryMode.NEW))
                btnSetup.Enabled = true;
            else
                btnSetup.Enabled = false;
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "SALES_RETURN", m_dtRecurringSetting);
            fvr.ShowDialog();
        }

        string RSID = null, recurringVoucherID = null;
        public void CheckRecurringSetting(string VoucherID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            m_dtRecurringSetting = RecurringVoucher.GetRecurringVoucherSetting(VoucherID, "SALES_RETURN");
            if (m_dtRecurringSetting.Rows.Count > 0)
            {
                chkRecurring.Checked = true;
                RSID = m_dtRecurringSetting.Rows[0]["RVID"].ToString();
                recurringVoucherID = m_dtRecurringSetting.Rows[0]["VoucherID"].ToString();
            }
            else
            {
                chkRecurring.Checked = false;
            }
        } 
        #endregion

        #region methods related to voucher list
        private void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UserPermission.ChkUserPermission("SALE_RETURN_VIEW"))
                {
                    Global.MsgError("Sorry! you dont have permission to View Sales Return. Please contact your administrator for permission.");
                    return;
                }
                string[] vouchValues = new string[5];
                vouchValues[0] = "SALES_RETURN";               // voucherType
                vouchValues[1] = "Inv.tblSalesReturnMaster";   // master tableName for the given voucher type  
                vouchValues[2] = "Inv.tblSalesReturnDetails";  // details tableName for the given voucher type
                vouchValues[3] = "SalesReturnID";              // master ID for the given master table
                vouchValues[4] = "SalesReturn_Date";              // date field for a given voucher

                frmVoucherList fvl = new frmVoucherList(this, vouchValues);
                fvl.ShowDialog();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void GetVoucher(int VoucherID)
        {
            m_SalesReturnID = VoucherID;
            txtSalesReturnID.Text = VoucherID.ToString();
            Navigation(Navigate.ID);
            //frmSalesReturn_Load(null, null);
        } 
        #endregion

        #region method related bank reconciliation
        public bool CheckIfBankReconciliationClosed()
        {
            try
            {
                bool res = false;
                ListItem bankId = (ListItem)cboCashParty.SelectedItem;
                if (BankReconciliation.IsBankReconciliationClosed(Convert.ToInt32(bankId.ID), Date.ToDotNet(txtDate.Text)))
                {
                    Global.MsgError("Bank Reconciliation is closed for this Bank, So you cannot add, edit or delete the vocher !");
                    res = true;
                }
                return res;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return true;

            }
        } 
        #endregion

        #region methods related to reference
        public DataTable dtReference = new DataTable();
        public bool isSelected = false;
        public string m_refAmt = "0(Dr)";
        public int CurrAccLedgerID = 0;
        public string ToDeleteRows = " ";
        public void AddReferenceColumns()
        {
            dtReference.Columns.Clear();
            dtReference.Columns.Add("LedgerID");
            dtReference.Columns.Add("VoucherType");
            dtReference.Columns.Add("RefName");
            dtReference.Columns.Add("RefID");
            dtReference.Columns.Add("IsAgainst");
        }
        public void GetAgainstReference(int refID, decimal amt, string crDr)
        {
            try
            {
                // remove the previously saved reference settings to add current settings
                foreach (DataRow dr in dtReference.Select("LedgerID = " + CurrAccLedgerID + " and IsAgainst ='true'"))
                {
                    if (dr["RVID"].ToString() != "0")
                        ToDeleteRows += dr["RVID"] + ",";
                    dr.Delete();
                }
                dtReference.AcceptChanges();

                DataRow dr1 = dtReference.NewRow();
                dr1["LedgerID"] = CurrAccLedgerID;
                dr1["VoucherType"] = "SLS_RTN";
                dr1["RefName"] = null;
                dr1["RefID"] = refID;
                dr1["IsAgainst"] = "true";
                dtReference.Rows.Add(dr1);
                //if (crDr == "(Cr)")
                //    grdContra[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Credit";

                //else
                //    grdContra[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Debit";

                //grdContra[CurrRowPos, (int)GridColumn.Amount].Value = amt.ToString();
                //grdContra[CurrRowPos, (int)GridColumn.Ref_Amt].Value = amt.ToString() + crDr;
                m_refAmt = amt + crDr;
                // SendKeys.Send("{Tab}");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void GetNewReference(string refName)
        {
            try
            {
                // remove the previously saved reference settings to add current settings
                foreach (DataRow dr in dtReference.Select("LedgerID = " + CurrAccLedgerID + " and IsAgainst ='false'"))
                {
                    if (dr["RVID"].ToString() != "0")
                        ToDeleteRows += dr["RVID"] + ",";
                    dr.Delete();
                }
                dtReference.AcceptChanges();

                DataRow dr1 = dtReference.NewRow();
                dr1["LedgerID"] = CurrAccLedgerID;
                dr1["VoucherType"] = "SLS_RTN";
                dr1["RefName"] = refName;
                dr1["RefID"] = DBNull.Value;
                dr1["IsAgainst"] = "false";
                dtReference.Rows.Add(dr1);
                //hasChanged = true;
                // SendKeys.Send("{Tab}");

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void RemoveReference()
        {
            try
            {
                // if against reference or new references are saved and for the same ledger if none is selected, remove the references 
                foreach (DataRow dr in dtReference.Select("LedgerID = " + CurrAccLedgerID + ""))
                {
                    if (dr["RVID"].ToString() != "0")
                        ToDeleteRows += dr["RVID"] + ",";
                    dr.Delete();
                }
                dtReference.AcceptChanges();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public bool CheckAmtAgainstRefAmt()
        {
            try
            {
                bool res = true;
                decimal Amt = Convert.ToDecimal(lblGrandTotal.Text);
                string crdr = "Credit";

                string amtCrDr = m_refAmt;
                decimal refAmt = Convert.ToDecimal(amtCrDr.Substring(0, amtCrDr.Length - 4));
                string ledgerName = cboCashParty.Text; //grdContra[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString();
                if ((refAmt > 0) && (refAmt < Amt) && (amtCrDr.Contains(crdr.Substring(0, crdr.Length - 4))))
                {
                    Global.MsgError("Your transaction amount for ledger " + ledgerName + " is " + Amt + " \n which is greater than the reference amount i.e. " + refAmt + " !");
                    res = false;
                }

                return res;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return false;
            }
        }
        int ledID = 0, vouchID = 0;
        bool isNewReferenceVoucher = false, isAgainstRef = false;

        private void cboCashParty_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cboCashParty.SelectedIndex > 0)
                {
                    ListItem LiCashPartyLedgerID = new ListItem();
                    LiCashPartyLedgerID = (ListItem)cboCashParty.SelectedItem;
                    CurrAccLedgerID = LiCashPartyLedgerID.ID;
                    if (m_mode == EntryMode.EDIT)
                        isNewReferenceVoucher = VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(txtSalesReturnID.Text), CurrAccLedgerID, "SLS_RTN");
                    else
                        isNewReferenceVoucher = false;
                    if (VoucherReference.CheckIfReferece(CurrAccLedgerID) && !isNewReferenceVoucher) // if isBillReference is true for given ledger then load the reference form
                    {
                        Form fc = Application.OpenForms["frmReference"];

                        if (fc != null)
                            fc.Close();

                        if (txtSalesReturnID.Text != "")
                        {
                            vouchID = Convert.ToInt32(txtSalesReturnID.Text);
                        }

                        frmReference fr = new frmReference(this, vouchID, "SLS_RTN", CurrAccLedgerID);
                        fr.ShowDialog();
                        //SendKeys.Send("{Tab}");
                    }
                }
            }
            catch (Exception ex)
            {

                Global.MsgError(ex.Message);
            }
        }
        #endregion

        private void btnDate_Click(object sender, EventArgs e)
        {
            try
            {
                Common.frmDateConverter frm = new Common.frmDateConverter(this, Date.ToDotNet(txtDate.Text));
                frm.ShowDialog();
            }
            catch (Exception)
            {
                Global.MsgError("Date is not in correct format.");
            }
        }

        public void DateConvert(DateTime ReturnDotNetDate)
        {
            try
            {
                txtDate.Text = Date.ToSystem(ReturnDotNetDate);
            }
            catch (Exception)
            {
                Global.MsgError("Date is not in correct format.");
            }
        }
    }
}
