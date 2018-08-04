using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Collections;
using Inventory.DataSet;
using DateManager;
using System.IO;
using System.Data;
using CrystalDecisions.Shared;
using System.Threading;
using Common;
using Inventory.Reports;
using BusinessLogic.Accounting;
using DevAge.ComponentModel.Validator;


namespace Inventory
{
    public interface IfrmPurchaseInvoice
    {
        void PurchaseInvoice(int RowID);
    }
    public partial class frmPurchaseInvoice : Form, IfrmAddAccClass, ListProduct, IfrmDateConverter, IfrmAccountLedger, IVoucherRecurring, IVoucherList, IVoucherReference
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        bool hasChanged = false;
        string Prefix = "";
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private bool IsFieldChanged = false;
        private bool IsShortcutKey = false;
        private double purchaserate = 0;
        private double prate = 0;
        private string productcode = "";
        private int CurrRowPos = 0;
        private int CurrAccLProductid = 0;
        private string ProdName = "";
        private string prodcode = "";
        private double salerate = 0;

        private IMDIMainForm m_MDIForm;

        private double VAT = 0;
        private double TAX1 = 0;
        private double TAX2 = 0;
        private double TAX3 = 0;

        private double tax1amt = 0;
        private double tax2amt = 0;
        private double tax3amt = 0;

        private string iscustomduty = "";
        string Vatvalue = "0";
        // private double GrossAmount = 0;
        //private double NetAmount = 0;
        double CostAfterCustomDuty = 0;
        public double GAmount = 0;

        //private bool IsSelected = false;

        int? OrderNo = null;
        double TotalQty = 0;
        private int loopCounter = 0;
        private int PurchaseInvoiceIDCopy = 0;
        List<int> AccClassID = new List<int>();
        ListItem SeriesID = new ListItem();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        Services m_Service = new Services();
        Purchase m_PurchaseInvoice = new Purchase();
        private int m_PurchaseInvoiceID;
        private Inventory.Model.dsPurchaseInvoice dsPurchaseInvoice = new Model.dsPurchaseInvoice();
        ListItem liProjectID = new ListItem();
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtQty = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtUnitChanged = new SourceGrid.Cells.Controllers.CustomEvents();

        SourceGrid.Cells.Controllers.CustomEvents evtRate = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProduct = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProuctCode = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscPercentage = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtOrderNo = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtcboDrCrSelectedIndexChanged = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtCustomDutyPer_Modified = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtCustomDutyAmount_Modified = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtFreight = new SourceGrid.Cells.Controllers.CustomEvents();

        SourceGrid.Cells.Controllers.CustomEvents evtVAT = new SourceGrid.Cells.Controllers.CustomEvents();
       
        private SourceGrid.Cells.Views.Cell RowView;

        //For Export Menu
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
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
            Del = 0, SNo, Code_No, ProductName, Qty, Units, Default_Unit, Default_Qty, PurchaseRate, Amount, SplDisc_Percent, SplDisc, NetAmount, PurchaseOrderDetailID, VAT, CustomDuty_Percent, CustomDuty, Freight, ProductID
        };

        bool m_isRecurring = false;
        int m_RVID = 0; 
        //public frmPurchaseInvoice()
        //{
        //    InitializeComponent();
        //}
        public frmPurchaseInvoice(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;

        }
        public frmPurchaseInvoice(int PurchaseInvoiceID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_PurchaseInvoiceID = PurchaseInvoiceID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }
        public frmPurchaseInvoice(int PurchaseInvoiceID)
        {
            InitializeComponent();
            this.m_PurchaseInvoiceID = PurchaseInvoiceID;
        }

        public void AddAccClass()
        {
            try
            {
                //Clear the checked nodes of Treeview and relaoding the tree view
                treeAccClass.Nodes.Clear();
                ShowAccClassInTreeView(treeAccClass, null, 0);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void OrderNo_Selected(object sender, EventArgs e)
        {
            //write code here
            ChangeState(EntryMode.NEW);
            DataTable dtPurchaseOrderMasterInfo = new DataTable();

            //Check control value is int or not???
            object m_OrderNo = (object)txtOrderNo.Text;
            bool IsOrderNoIsInt = Misc.IsInt(m_OrderNo, false);
            if ((IsOrderNoIsInt) && (txtOrderNo.Text != ""))//If orderno is int value
            {
                dtPurchaseOrderMasterInfo = PurchaseOrder.GetPurchaseOrderMasterInfoByOrderNo(Convert.ToInt32(txtOrderNo.Text));  //with help of OrderNo,Get the information from Inv.tblPurchaseOrderMaster....
            }
            else if ((!IsOrderNoIsInt) && (txtOrderNo.Text != ""))
            {
                Global.Msg("Please Enter the Integer Value");
                txtOrderNo.Text = "";
                txtOrderNo.Focus();
                return;
            }

            if ((dtPurchaseOrderMasterInfo.Rows.Count > 0) && (txtOrderNo.Text != ""))
            {
                DataRow drPurchaseOrderMasterInfo = dtPurchaseOrderMasterInfo.Rows[0];
                txtOrderNo.Text = drPurchaseOrderMasterInfo["OrderNo"].ToString();
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseOrderMasterInfo["CashPartyID"]), LangMgr.DefaultLanguage);
                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                cboCashPartyAcc.Text = drLedgerInfo["LedName"].ToString();

                //Get the Detail information with help of PurchaseOrderMasterID
                PurchaseOrder m_PurchasaeOrder = new PurchaseOrder();
                DataTable dtPurchaseOrderDetailInfo = m_PurchasaeOrder.GetPurchaseOrderDetails(Convert.ToInt32(drPurchaseOrderMasterInfo["PurchaseOrderID"]));
                double Amount = 0;
                double TotalNet = 0;
                int TotalQty = 0;
                double VatAmt = 0;
                for (int i = 1; i <= dtPurchaseOrderDetailInfo.Rows.Count; i++)
                {
                    DataRow drDetail = dtPurchaseOrderDetailInfo.Rows[i - 1];
                    grdPurchaseInvoice[i, (int)GridColumn.SNo].Value = i.ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.Code_No].Value = drDetail["ProductCode"].ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.PurchaseRate].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString();
                    int Quantity = 0;
                    if ((drDetail["UpdatedQty"].ToString() != ""))
                    {
                        Quantity = (Convert.ToInt32(drDetail["Quantity"]) - (Convert.ToInt32(drDetail["UpdatedQty"])));
                        grdPurchaseInvoice[i, (int)GridColumn.Qty].Value = Quantity;
                        TotalQty += Quantity;
                        Amount = (Quantity * Convert.ToDouble(drDetail["PurchaseRate"]));
                    }
                    else
                    {
                        grdPurchaseInvoice[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
                        TotalQty += Convert.ToInt32(drDetail["Quantity"].ToString());
                        Amount = (Convert.ToInt32(drDetail["Quantity"]) * Convert.ToDouble(drDetail["PurchaseRate"]));
                    }
                    //CalculateNetAmount();
                    //CalculateTotalQuantity();

                    grdPurchaseInvoice[i, (int)GridColumn.Amount].Value = Amount.ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.NetAmount].Value = Amount.ToString();
                    TotalNet += Amount;
                    grdPurchaseInvoice[i, (int)GridColumn.PurchaseOrderDetailID].Value = drDetail["PurchaseOrderDetailsID"].ToString();

                    //Write corresponding Order No.
                    grdPurchaseInvoice[i, (int)GridColumn.ProductID].Value = drDetail["ProductID"].ToString();

                    //For VAt
                    DataTable dtcheckvatapplicable = Product.GetProductByID(Convert.ToInt32(drDetail["ProductID"]));
                    DataRow drcheckvatapplicable = dtcheckvatapplicable.Rows[0];

                    if (drcheckvatapplicable["IsVatApplicable"].ToString() == "1")
                    {
                        string Vatvalue = Settings.GetSettings("DEFAULT_PURCHASE_VAT");
                        if (Vatvalue == "1")
                        {
                            VatAmt = (Amount * VAT) / 100;

                        }
                    }
                    //If there is already a row for new then don't add a new row
                    if (grdPurchaseInvoice[grdPurchaseInvoice.Rows.Count - 1, (int)GridColumn.ProductName].Value.ToString() != "(NEW)")
                        AddRowProduct1(grdPurchaseInvoice.RowsCount);

                }
                lblVat.Text = VatAmt.ToString();
                lblNetAmout.Text = TotalNet.ToString();
                lblGross.Text = TotalNet.ToString();
                lblTotalQty.Text = TotalQty.ToString();
                lblGrandTotal.Text = (Convert.ToDouble(lblNetAmout.Text) + Convert.ToDouble(lblVat.Text)).ToString();

            }
            else if ((dtPurchaseOrderMasterInfo.Rows.Count <= 0) && (txtOrderNo.Text != ""))
            {
                Global.Msg("Order Number doesnot Exist");
                txtOrderNo.Text = string.Empty;
                txtOrderNo.Focus();
            }
        }

        bool isTax1;
        bool isTax2;
        bool isTax3;
        private void frmPurchaseInvoice_Load(object sender, EventArgs e)
        {
            AddReferenceColumns();
            if (txtPurchaseInvoiceID.Text != "")
                dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(txtPurchaseInvoiceID.Text), "PURCH");

            chkDoNotClose.Checked = true;
            //Load TAX and VAT Rate
            DataTable dtvat = new DataTable();
            dtvat = Slabs.GetSlabInfo(SlabType.VAT);
            DataRow drvat = dtvat.Rows[0];
            VAT = Convert.ToDouble(drvat[3].ToString());

             isTax1 = Convert.ToBoolean(Convert.ToInt32(Settings.GetSettings("DEFAULT_PURCHASE_TAX1CHECK")));
            isTax2=Convert.ToBoolean(Convert.ToInt32(Settings.GetSettings("DEFAULT_PURCHASE_TAX2CHECK")));
            isTax3 = Convert.ToBoolean(Convert.ToInt32(Settings.GetSettings("DEFAULT_PURCHASE_TAX3CHECK")));
            if (isTax1)
            {
                DataTable dttax1 = new DataTable();
                dttax1 = Slabs.GetSlabInfo(SlabType.TAX1);
                DataRow drtax1 = dttax1.Rows[0];
                TAX1 = Convert.ToDouble(drtax1[3].ToString());
            }
            else
                TAX1 = 0;

            DataTable dttax2 = new DataTable();
            dttax2 = Slabs.GetSlabInfo(SlabType.TAX2);
            DataRow drtax2 = dttax2.Rows[0];
            TAX2 = Convert.ToDouble(drtax2[3].ToString());

            DataTable dttax3 = new DataTable();
            dttax3 = Slabs.GetSlabInfo(SlabType.TAX3);
            DataRow drtax3 = dttax3.Rows[0];
            TAX3 = Convert.ToDouble(drtax3[3].ToString());

            iscustomduty = Settings.GetSettings("CUSTOMDUTY");
            Vatvalue = Settings.GetSettings("DEFAULT_PURCHASE_VAT");
            Global.product = true;
            //Just make visible false for Depot elements ....use only when inventory system is implemented
            ChangeState(EntryMode.NEW);
            txtPurchaseInvoiceID.Visible = false;

            #region BLOCK FOR SHOWING SERIES NAME IN COMBOBOX

            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("PURCH");
            if (dtSeriesInfo.Rows.Count > 0)
            {
                for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
                {
                    DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                    cboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
                cboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cboSeriesName.SelectedIndex = 0;
            }
            #endregion

            #region BLOCK OF SHOWING DEPOT IN COMBOBOX
            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            if (dtDepotInfo.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDepotInfo.Rows)
                {
                    cboDepot.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cboDepot.SelectedIndex = 0;
            }

            #endregion
            m_mode = EntryMode.NEW;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            try
            {
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                //Event trigerred when delete button is clicked
                //evtDiscount.Click += new EventHandler(Delete_Row_Click);

                //Event when account is selected
                evtProduct.FocusLeft += new EventHandler(Product_Selected);

                //Event when ProductCode is selected
                evtProuctCode.FocusLeft += new EventHandler(ProductCode_Selected);

                //Event when Quntity is selected
                evtQty.FocusLeft += new EventHandler(Qty_Modified);
                //evtQty.FocusLeft += new EventHandler(PurchaseRate_Modified);

                //Event when Rate is selected
                evtRate.FocusLeft += new EventHandler(PurchaseRate_Modified);

                //Event when DiscPercentage is selected
                evtDiscPercentage.FocusLeft += new EventHandler(DiscPercentage_Modified);

                //Event when Discount is selected
                evtDiscount.FocusLeft += new EventHandler(Qty_Modified);

                evtUnitChanged.ValueChanged += new EventHandler(Unit_Changed);

                evtCustomDutyPer_Modified.FocusLeft += new EventHandler(CustomDutyPer_Modified);

                evtCustomDutyAmount_Modified.FocusLeft += new EventHandler(CustomDutyAmount_Modified);

                evtFreight.FocusLeft += new EventHandler(Freight_Modified);

                evtOrderNo.FocusLeft += new EventHandler(OrderNo_Selected);
                txtOrderNo.LostFocus += new EventHandler(OrderNo_Selected);

                evtVAT.FocusLeft += new EventHandler(VATAmt_FocusLeft);

                evtAmountFocusLost.FocusLeft += new EventHandler(Amount_Focus_Lost);
                //For Loading The Optional Fields
                OptionalFields();

                #region Purchase Account according to User Setting
                //Displaying the all ledgers associated with Purchase AccountGroup in DropDownList
                int PurchaseID = AccountGroup.GetGroupIDFromGroupNumber(12);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultPurchase Account according to user root or other users
                int DefaultPurchaseAccNum = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_PURCHASE_ACCOUNT") : UserPreference.GetValue("DEFAULT_PURCHASE_ACCOUNT", uid));
                string DefaultPurchaseName = "";

                //Add Banks to comboPurchaseAccount
                DataTable dtPurchaseLedgers = Ledger.GetAllLedger(PurchaseID);
                foreach (DataRow drPurchaseLedgers in dtPurchaseLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboPurchaseAcc.Items.Add(new ListItem((int)drPurchaseLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drPurchaseLedgers["LedgerID"]) == DefaultPurchaseAccNum)
                        DefaultPurchaseName = drLedgerInfo["LedName"].ToString();
                }
                cboPurchaseAcc.DisplayMember = "value";//This value is  for showing at Load condition
                cboPurchaseAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cboPurchaseAcc.Text = DefaultPurchaseName;

                #endregion

                #region BLOCK FOR SHOWING THE LEDGERS OF CASH IN HAND,DEBTOR AND CREDITOR IN CASH/PARTY COMBOBOX
                LoadComboboxParty();
                //cmboCashPartyAcc.Items.Add(new ListItem(0, "Choose Cash/Party Ledger"));//At the first index of combobox show the "Choose Cash/Party Ledger"
                //int Cash_In_Hand = AccountGroup.GetGroupIDFromGroupNumber(102);
                //DataTable dtCash_In_HandLedgers = Ledger.GetAllLedger(Cash_In_Hand);//Collecting the Ledgers corresponding to Cash_In_Hand group
                //foreach (DataRow drCash_In_HandLedgers in dtCash_In_HandLedgers.Rows)
                //{
                //    cmboCashPartyAcc.Items.Add(new ListItem((int)drCash_In_HandLedgers["LedgerID"], drCash_In_HandLedgers["EngName"].ToString()));
                //}
                //int Debtor = AccountGroup.GetGroupIDFromGroupNumber(29);
                //DataTable dtDebtorLedgers = Ledger.GetAllLedger(Debtor);
                //foreach (DataRow drDebtorLedgers in dtDebtorLedgers.Rows)
                //{
                //    cmboCashPartyAcc.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drDebtorLedgers["EngName"].ToString()));
                //}
                //int Creditor = AccountGroup.GetGroupIDFromGroupNumber(114);
                //DataTable dtCreditorLedgers = Ledger.GetAllLedger(Creditor);
                //foreach (DataRow drCreditorLedgers in dtCreditorLedgers.Rows)
                //{
                //    cmboCashPartyAcc.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drCreditorLedgers["EngName"].ToString()));
                //}

                //int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
                //DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                //foreach (DataRow drBankLedger in dtBankLedgers.Rows)
                //{
                //    cmboCashPartyAcc.Items.Add(new ListItem((int)drBankLedger["LedgerID"], drBankLedger["EngName"].ToString()));
                //}
                //cmboCashPartyAcc.DisplayMember = "value";//This value is  for showing at Load condition
                //cmboCashPartyAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition
                //cmboCashPartyAcc.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox

                #endregion

                grdPurchaseInvoice.Redim(2, 19);
                btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                evtProductFocusLost.FocusLeft += new EventHandler(evtProductFocusLost_FocusLeft);
                //Prepare the header part for grid

                AddGridHeader();
                //bool test = IsFieldChanged;
                //MessageBox.Show(test.ToString());
                //AddRowProduct(1);
                AddRowProduct1(1);

                cboProjectName.Items.Clear();
                //cboProjectName.Items.Add(new ListItem(0, "======= NO PARENT ======="));
                //ListProject(cboProjectName);
                LoadComboboxProject(cboProjectName, 0);

                ShowAccClassInTreeView(treeAccClass, null, 0);
                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID
                txtPurchaseInvoiceID.Text = m_PurchaseInvoiceID.ToString();
                ////if (m_PurchaseInvoiceID > 0)
                ////{
                ////    Navigation(Navigate.ID);
                ////}
                if (m_PurchaseInvoiceID > 0)
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
                            vouchID = m_PurchaseInvoiceID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }
                        //Purchase m_Purchase = new Purchase();
                        Purchase m_Purchase = new Purchase();

                        //Getting the value of SeriesID via MasterID or VouchID
                        int SeriesIDD = m_Purchase.GetSeriesIDFromMasterID(vouchID);

                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Purchase Invoice");
                            cboSeriesName.Text = "";
                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            cboSeriesName.Text = dr["EngName"].ToString();
                        }
                        DataTable dtPurchaseInvoiceMaster = m_Purchase.GetPurchaseInvoiceMasterInfo(vouchID.ToString());

                        if (dtPurchaseInvoiceMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }
                        foreach (DataRow drPurchaseInvoiceMaster in dtPurchaseInvoiceMaster.Rows)
                        {
                            if (!m_isRecurring)
                            {
                                txtVoucherNo.Text = drPurchaseInvoiceMaster["Voucher_No"].ToString();
                                txtDate.Text = Date.DBToSystem(drPurchaseInvoiceMaster["PurchaseInvoice_Date"].ToString());
                            }
                            else
                            {
                                txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if recurring load today's date
                            }

                            txtPartyBillNumber.Text = drPurchaseInvoiceMaster["PartyBillNumber"].ToString();
                            //txtVoucherNo.Text = drPurchaseInvoiceMaster["Voucher_No"].ToString();
                            //txtDate.Text = Date.DBToSystem(drPurchaseInvoiceMaster["PurchaseInvoice_Date"].ToString());
                            txtRemarks.Text = drPurchaseInvoiceMaster["Remarks"].ToString();
                            txtPurchaseInvoiceID.Text = drPurchaseInvoiceMaster["PurchaseInvoiceID"].ToString();
                            txtOrderNo.Text = drPurchaseInvoiceMaster["OrderNo"].ToString();
                            lblNetAmout.Text = drPurchaseInvoiceMaster["Net_Amount"].ToString();
                            lblTax1.Text = drPurchaseInvoiceMaster["Tax1"].ToString();
                            lblTax2.Text = drPurchaseInvoiceMaster["Tax2"].ToString();
                            lblTax3.Text = drPurchaseInvoiceMaster["Tax3"].ToString();
                            lblVat.Text = drPurchaseInvoiceMaster["VAT"].ToString();
                            lblCustomduty.Text = drPurchaseInvoiceMaster["CustomDuty"].ToString();
                            lblFreight.Text = drPurchaseInvoiceMaster["Freight"].ToString();
                            lblTotalQty.Text = drPurchaseInvoiceMaster["TotalQty"].ToString();
                            lblSpecialDiscount.Text = drPurchaseInvoiceMaster["SpecialDiscount"].ToString();
                            lblGross.Text = drPurchaseInvoiceMaster["Gross_Amount"].ToString();
                            lblGrandTotal.Text = (Convert.ToDouble(lblNetAmout.Text) + Convert.ToDouble(lblTax1.Text) + Convert.ToDouble(lblTax2.Text) + Convert.ToDouble(lblTax3.Text) + Convert.ToDouble(lblVat.Text)).ToString();
                            lblGrandTotalAfterCustomduty.Text = (Convert.ToDouble(lblGrandTotal.Text) + Convert.ToDouble(lblCustomduty.Text) + Convert.ToDouble(lblFreight.Text)).ToString();
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

                                    txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();
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

                                    txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();
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

                                    txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();

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

                                    txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();

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

                                    txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();
                                }


                            }
                            dsPurchaseInvoice.Tables["tblPurchaseInvoiceMaster"].Rows.Add(cboSeriesName.Text, drPurchaseInvoiceMaster["Voucher_No"].ToString(), Date.DBToSystem(drPurchaseInvoiceMaster["PurchaseInvoice_Date"].ToString()), cboDepot.Text, drPurchaseInvoiceMaster["OrderNo"].ToString(), cboCashPartyAcc.Text, cboPurchaseAcc.Text, drPurchaseInvoiceMaster["Remarks"].ToString());

                            DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseInvoiceMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drCashPartyLedgerInfo = dtCashPartyInfo.Rows[0];

                            cboCashPartyAcc.Text = drCashPartyLedgerInfo["LedName"].ToString();

                            //Show the corresponding Purchase Account Ledger in Combobox
                            DataTable dtPurchaseLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseInvoiceMaster["PurchaseLedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drPurchaseLedgerInfo = dtPurchaseLedgerInfo.Rows[0];
                            cboPurchaseAcc.Text = drPurchaseLedgerInfo["LedName"].ToString();

                            DataTable dtDepotDtlInfo = Depot.GetDepotInfo(Convert.ToInt32(drPurchaseInvoiceMaster["DepotID"].ToString()));
                            foreach (DataRow drDepotInfo in dtDepotDtlInfo.Rows)
                            {
                                cboDepot.Text = drDepotInfo["DepotName"].ToString();
                            }


                        }

                        DataTable dtPurchaseInvoiceDetails = m_Purchase.GetPurchaseInvoiceDetails(vouchID);
                        if (dtPurchaseInvoiceDetails.Rows.Count > 0)
                        {
                            for (int i = 1; i <= dtPurchaseInvoiceDetails.Rows.Count; i++)
                            {
                                DataRow drDetail = dtPurchaseInvoiceDetails.Rows[i - 1];
                                grdPurchaseInvoice[i, (int)GridColumn.SNo].Value = i.ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.Code_No].Value = drDetail["Code"].ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();

                                grdPurchaseInvoice[i, (int)GridColumn.Qty].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["CurUnitQty"])).ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.Default_Qty].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Quantity"])).ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.Default_Unit].Value = Convert.ToInt32(drDetail["UnitID"].ToString());

                                grdPurchaseInvoice[i, (int)GridColumn.PurchaseRate].Value = drDetail["PurchaseRate"].ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.SplDisc_Percent].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.SplDisc].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.NetAmount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.ProductID].Value = drDetail["ProductID"].ToString();
                                grdPurchaseInvoice[i, (int)GridColumn.VAT].Value = Convert.ToDecimal(drDetail["VAT"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdPurchaseInvoice[i, (int)GridColumn.CustomDuty_Percent].Value = Convert.ToDecimal(drDetail["CustomDutyPercentage"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdPurchaseInvoice[i, (int)GridColumn.CustomDuty].Value = Convert.ToDecimal(drDetail["CustomDuty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdPurchaseInvoice[i, (int)GridColumn.Freight].Value = Convert.ToDecimal(drDetail["Freight"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                                //AddRowProduct(grdPurchaseInvoice.RowsCount);
                                AddRowProduct1(grdPurchaseInvoice.RowsCount);
                                dsPurchaseInvoice.Tables["tblPurchaseInvoiceDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(), drDetail["Quantity"].ToString(), drDetail["PurchaseRate"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());
                            }
                        }
                        // if recurring is true then donot load recurring settings for new voucher
                        if (!m_isRecurring)
                            CheckRecurringSetting(txtPurchaseInvoiceID.Text);
                    }

                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        public void LoadComboboxParty()
        {
            int Cash_In_Hand = AccountGroup.GetGroupIDFromGroupNumber(102);
            DataTable dtCash_In_HandLedgers = Ledger.GetAllLedger(Cash_In_Hand);
           
            int Debtor = AccountGroup.GetGroupIDFromGroupNumber(29);
            DataTable dtDebtorLedgers = Ledger.GetAllLedger(Debtor);
            dtDebtorLedgers.Merge(dtCash_In_HandLedgers);
            
            int Creditor = AccountGroup.GetGroupIDFromGroupNumber(114);
            DataTable dtCreditorLedgers = Ledger.GetAllLedger(Creditor);
            dtDebtorLedgers.Merge(dtCreditorLedgers);
           

            int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
            DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
            dtDebtorLedgers.Merge(dtBankLedgers);

            cboCashPartyAcc.DataSource = null;
            cboCashPartyAcc.DataSource = dtDebtorLedgers;
            cboCashPartyAcc.DisplayMember = "EngName";//This value is  for showing at Load condition
            cboCashPartyAcc.ValueMember = "LedgerID";//This value is stored only not to be shown at Load condition
            cboCashPartyAcc.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
        }
        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdPurchaseInvoice.RowsCount - 2)
            {
                grdPurchaseInvoice.Rows.Remove(ctx.Position.Row);
               
                CalculateNetAmount();
                CalculateTotalDiscount();
                CalculateTotalCustomDuty();
            }
        }

        private bool isVatAmtChanged = false;
        private void VATAmt_FocusLeft(object sender, EventArgs e)
        {
           
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not calculate if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdPurchaseInvoice.RowsCount - 2)
            {
                    isVatAmtChanged = true;
                CalculateNetAmount();
                CalculateTotalDiscount();
                CalculateTotalCustomDuty();
            }
        }
        public decimal CalculateVat(decimal m_NetAmount)
        {
            try
            {
                decimal VatAmt = 0;

                if (Vatvalue == "1")
                {
                    int productid = Convert.ToInt32(grdPurchaseInvoice[CurrRowPos, (int)GridColumn.ProductID].Value);
                    string vatApplicable = "0";
                    if (productid > 0)
                    {
                        DataTable dtCheckVATApplicable = Product.GetProductByID(productid);
                        DataRow drcheckvatapplicable = dtCheckVATApplicable.Rows[0];
                        vatApplicable = drcheckvatapplicable["IsVatApplicable"].ToString();
                    }
                    

                    if (vatApplicable == "1") //If Product is VAT applicable
                    {
                        if (isVatAmtChanged)
                        {

                            if (grdPurchaseInvoice[CurrRowPos, (int)GridColumn.VAT].Value != null)
                                VatAmt += Convert.ToDecimal(grdPurchaseInvoice[CurrRowPos, (int)GridColumn.VAT].Value);
                            else
                                grdPurchaseInvoice[CurrRowPos, (int)GridColumn.VAT].Value = "0";

                            isVatAmtChanged = false;

                        }
                        else
                        {
                            decimal amt = Convert.ToDecimal(grdPurchaseInvoice[CurrRowPos, (int)GridColumn.NetAmount].Value);

                            VatAmt = (m_NetAmount + (Convert.ToDecimal(TAX1) * amt / 100)) * Convert.ToDecimal(VAT) / 100;
                        }


                    }
                }
                return VatAmt;

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// Sums up all NetAmount
        /// </summary>

        private void CalculateNetAmount()
        {
            try
            {
                double NetAmount = 0;
                double VatAmt = 0;
                //string Vatvalue = Settings.GetSettings("DEFAULT_SALES_VAT");//Overall vat applicable or not 1=applicable 0=not applicable

                  #region Loop through all rows of the grid
                    for (int i = 1; i < grdPurchaseInvoice.RowsCount - 1; i++)
                    {
                        //Check if it is the (NEW) row.If it is ,it must be the last row.

                        if (i == grdPurchaseInvoice.Rows.Count)
                            return;
                        CurrRowPos = i;
                        double m_NetAmount = 0;
                        string m_Value = Convert.ToString(grdPurchaseInvoice[i, (int)GridColumn.NetAmount].Value);
                        if (m_Value.Length == 0)
                            m_NetAmount = 0;
                        else
                            m_NetAmount = Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.NetAmount].Value);


                        NetAmount += m_NetAmount;



                        //VAT application
                        //Check if product is VAT applicable or not

                        //if (Vatvalue == "1")//If system is VAT applicable
                        //{
                        //    int productid = Convert.ToInt32(grdPurchaseInvoice[i, (int)GridColumn.ProductID].Value);

                        //    DataTable dtCheckVATApplicable = Product.GetProductByID(productid);
                        //    DataRow drcheckvatapplicable = dtCheckVATApplicable.Rows[0];

                        //    if (drcheckvatapplicable["IsVatApplicable"].ToString() == "1") //If Product is VAT applicable
                        //    {
                        //        if (isVatAmtChanged)
                        //        {

                        //            if (grdPurchaseInvoice[i, (int)GridColumn.VAT].Value != null)
                        //                VatAmt += Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.VAT].Value);
                        //            else
                        //                grdPurchaseInvoice[i, (int)GridColumn.VAT].Value = "0";

                        //            isVatAmtChanged = false;
                        //        }
                        //        else
                        //            VatAmt += (m_NetAmount * VAT) / 100;


                        //    }
                        //}
                       // VatAmt += Convert.ToDouble(CalculateVat(Convert.ToDecimal((m_NetAmount))));

                        if (Vatvalue == "1")
                        {
                            if (grdPurchaseInvoice[i, (int)GridColumn.VAT].Value != null)
                                VatAmt += Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.VAT].Value);
                            else
                                grdPurchaseInvoice[i, (int)GridColumn.VAT].Value = "0";
                        }
                        else
                            VatAmt = 0;

                    }
                    #endregion
                
                    string Tax1checkvalue = Settings.GetSettings("DEFAULT_PURCHASE_TAX1CHECK");
                    string Tax2checkvalue = Settings.GetSettings("DEFAULT_PURCHASE_TAX2CHECK");
                    string Tax3checkvalue = Settings.GetSettings("DEFAULT_PURCHASE_TAX3CHECK");
                    string Tax1value = Settings.GetSettings("DEFAULT_PURCHASE_TAX1");
                    string Tax2value = Settings.GetSettings("DEFAULT_PURCHASE_TAX2");
                    string Tax3value = Settings.GetSettings("DEFAULT_PURCHASE_TAX3");
                    if (Tax1checkvalue == "1")
                    {
                        if (Tax1value == "Nt Amt")
                        {
                            // double tax1amt=0;
                            tax1amt = (NetAmount * TAX1) / 100;
                            lblTax1.Text = tax1amt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }
                        else if (Tax1value == "Gross")
                        {

                            tax1amt = (GAmount * TAX1) / 100;
                            lblTax1.Text = tax1amt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }
                    }
                    if (Tax2checkvalue == "1")
                    {
                        if (Tax2value == "Nt Amt")
                        {
                            tax2amt = (NetAmount * TAX2) / 100;
                            lblTax2.Text = tax2amt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }
                        else if (Tax2value == "Tax 1")
                        {
                            tax2amt = (tax1amt * TAX2) / 100;
                            lblTax2.Text = tax2amt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }
                        else if (Tax2value == "Gross")
                        {
                            tax2amt = (GAmount * TAX2) / 100;
                            lblTax2.Text = tax2amt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }
                    }
                    if (Tax3checkvalue == "1")
                    {
                        if (Tax3value == "Nt Amt")
                        {
                            tax3amt = (NetAmount * TAX3) / 100;
                            lblTax3.Text = tax3amt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }
                        else if (Tax3value == "Tax 1")
                        {
                            tax3amt = (tax1amt * TAX3) / 100;
                            lblTax3.Text = tax3amt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }
                        else if (Tax3value == "Tax 2")
                        {
                            tax3amt = (tax2amt * TAX3) / 100;
                            lblTax3.Text = tax3amt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }
                        if (Tax3value == "Gross")
                        {
                            tax3amt = (GAmount * TAX3) / 100;
                            lblTax3.Text = tax3amt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }
                    }
                    //double CustomDutyTotal = 0;
                    //CustomDutyTotal += Convert.ToDouble(grdPurchaseInvoice[i, 13].Value);
                    //lblcustomduty.Text = CustomDutyTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    lblVat.Text = VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        lblNetAmout.Text = NetAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    double GrandTotal = 0;
                    GrandTotal = GrandTotal + (NetAmount + VatAmt + tax1amt + tax2amt + tax3amt);
                    lblGrandTotal.Text = GrandTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                
            }
            catch (Exception ex)
            {
                Global.MsgError("Error in NetAmount calucation ! due to "+ex.Message);
            }
        }
        //private void CalculateNetAmount()
        //{
        //    try
        //    {
        //        double NetAmount = 0;
        //        for (int i = 1; i < grdPurchaseInvoice.RowsCount-1 ; i++)
        //        {
        //            //Check if it is the (NEW) row.If it is ,it must be the last row.
        //            if (grdPurchaseInvoice[i, (int)GridColumn.ProductName].Value.ToString() == "(NEW)")
        //                return;
        //            //if (i == grdPurchaseInvoice.Rows.Count)
        //            //    return;
        //            double m_NetAmount = 0;
        //            string m_Value = Convert.ToString(grdPurchaseInvoice[i, (int)GridColumn.NetAmount].Value);
        //            if (m_Value.Length == 0)
        //                m_NetAmount = 0;
        //            else
        //                m_NetAmount = Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.NetAmount].Value);

        //            NetAmount += m_NetAmount;
        //            lblNetAmout.Text = NetAmount.ToString();
        //            //Also consider 13% VAT too
        //            double VatAmt = 0;
        //            string Vatvalue = Settings.GetSettings("DEFAULT_PURCHASE_VAT");

        //            string pname = grdPurchaseInvoice[i, (int)GridColumn.ProductName].Value.ToString();
        //            int productid = Product.GetProductIDFromName(pname, Lang.English);

        //            DataTable dtcheckvatapplicable = Product.GetProductByID(productid);
        //            DataRow drcheckvatapplicable = dtcheckvatapplicable.Rows[0];

        //            if (drcheckvatapplicable["IsVatApplicable"].ToString() == "1")
        //            {
        //                if (Vatvalue == "1")
        //                {
        //                    VatAmt = (NetAmount * VAT) / 100;
        //                    lblVat.Text = VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
        //                    //grdPurchaseInvoice[i, 11].Value = VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces));
        //                }
        //            }

        //            //{
        //            //    VatAmt = (NetAmount * VAT) / 100;
        //            //    lblVat.Text = VatAmt.ToString();

        //            //}

        //            string Tax1checkvalue = Settings.GetSettings("DEFAULT_PURCHASE_TAX1CHECK");
        //            string Tax2checkvalue = Settings.GetSettings("DEFAULT_PURCHASE_TAX2CHECK");
        //            string Tax3checkvalue = Settings.GetSettings("DEFAULT_PURCHASE_TAX3CHECK");
        //            string Tax1value = Settings.GetSettings("DEFAULT_PURCHASE_TAX1");
        //            string Tax2value = Settings.GetSettings("DEFAULT_PURCHASE_TAX2");
        //            string Tax3value = Settings.GetSettings("DEFAULT_PURCHASE_TAX3");
        //            if (Tax1checkvalue == "1")
        //            {
        //                if (Tax1value == "Nt Amt")
        //                {
        //                    // double tax1amt=0;
        //                    tax1amt = (NetAmount * TAX1) / 100;
        //                    lbltax1.Text = tax1amt.ToString();
        //                }
        //                else if (Tax1value == "Gross")
        //                {

        //                    tax1amt = (GAmount * TAX1) / 100;
        //                    lbltax1.Text = tax1amt.ToString();
        //                }
        //            }
        //            if (Tax2checkvalue == "1")
        //            {
        //                if (Tax2value == "Nt Amt")
        //                {
        //                    tax2amt = (NetAmount * TAX2) / 100;
        //                    lbltax2.Text = tax2amt.ToString();
        //                }
        //                else if (Tax2value == "Tax 1")
        //                {
        //                    tax2amt = (tax1amt * TAX2) / 100;
        //                    lbltax2.Text = tax2amt.ToString();
        //                }
        //                else if (Tax2value == "Gross")
        //                {
        //                    tax2amt = (GAmount * TAX2) / 100;
        //                    lbltax2.Text = tax2amt.ToString();
        //                }
        //            }
        //            if (Tax3checkvalue == "1")
        //            {
        //                if (Tax3value == "Nt Amt")
        //                {
        //                    tax3amt = (NetAmount * TAX3) / 100;
        //                    lbltax3.Text = tax3amt.ToString();
        //                }
        //                else if (Tax3value == "Tax 1")
        //                {
        //                    tax3amt = (tax1amt * TAX3) / 100;
        //                    lbltax3.Text = tax3amt.ToString();
        //                }
        //                else if (Tax3value == "Tax 2")
        //                {
        //                    tax3amt = (tax2amt * TAX3) / 100;
        //                    lbltax3.Text = tax3amt.ToString();
        //                }
        //                if (Tax3value == "Gross")
        //                {
        //                    tax3amt = (GAmount * TAX3) / 100;
        //                    lbltax3.Text = tax3amt.ToString();
        //                }
        //            }
        //            //double CustomDutyTotal = 0;
        //            //CustomDutyTotal += Convert.ToDouble(grdPurchaseInvoice[i, 13].Value);
        //            //lblcustomduty.Text = CustomDutyTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

        //            double GrandTotal = 0;
        //            GrandTotal = GrandTotal + (NetAmount + VatAmt + tax1amt + tax2amt + tax3amt);
        //            lblgrandtotal.Text = GrandTotal.ToString();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MsgError("Error in NetAmount calucation!");
        //    }
        //}
        /// <summary>
        /// Sums up all Gross Amount
        /// </summary>

        private void CalculateGrossAmount()
        {
            try
            {
                double GrossAmount = 0;
                for (int i = 1; i < grdPurchaseInvoice.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row.If it is ,it must be the last row.

                    if (i == grdPurchaseInvoice.Rows.Count)
                        return;
                    double m_GrossAmount = 0;
                    string m_Value = Convert.ToString(grdPurchaseInvoice[i, (int)GridColumn.Amount].Value);
                    if (m_Value.Length == 0)
                        m_GrossAmount = 0;
                    else
                        m_GrossAmount = Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.Amount].Value);

                    GrossAmount += m_GrossAmount;
                    GAmount = GrossAmount;
                    lblGross.Text = GrossAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                    //if (Global.Default_Purchase_Tax1Check == "1")
                    //{
                    //    if (Global.Default_Purchase_Tax1On == "Gross")
                    //    {      
                    //        tax1amt=(GrossAmount*TAX1)/100;
                    //        lbltax1.Text = tax1amt.ToString();
                    //    }
                    //if (Global.Default_Sales_Tax2Check == "1")
                    //{
                    //    if (Global.Default_Purchase_Tax2On == "Gross")
                    //    {

                    //        tax2amt = (GrossAmount * TAX2) / 100;
                    //        lbltax2.Text = tax2amt.ToString();
                    //    }
                    //    else if (Global.Default_Purchase_Tax2On == "Tax 1")
                    //    {
                    //        tax2amt = (tax1amt * TAX2) / 100;
                    //        lbltax2.Text = tax2amt.ToString();
                    //    }
                    //}
                    //if (Global.Default_Sales_Tax3Check == "1")
                    //{
                    //    if (Global.Default_Purchase_Tax3On == "Gross")
                    //    {
                    //        tax3amt = (GrossAmount * TAX3) / 100;
                    //        lbltax2.Text = tax3amt.ToString();
                    //    }
                    //    else if(Global.Default_Purchase_Tax3On == "Tax 1")
                    //    {
                    //        tax3amt = (tax1amt * TAX3) / 100;
                    //        lbltax2.Text = tax3amt.ToString();
                    //    }
                    //    else if (Global.Default_Purchase_Tax3On == "Tax 2")
                    //    {
                    //        tax3amt = (tax2amt * TAX3) / 100;
                    //        lbltax2.Text = tax3amt.ToString();
                    //    }
                    //}
                    //}
                    //if (Global.Default_Sales_Tax2Check == "1")
                    //{
                    //    if (Global.Default_Purchase_Tax2On == "Nt Amt")
                    //    {
                    //        tax2amt = (NetAmount * TAX2) / 100;
                    //        lbltax2.Text = tax2amt.ToString();
                    //    }
                    //    else if (Global.Default_Purchase_Tax2On == "Tax 1")
                    //    {
                    //        tax2amt = (tax1amt * TAX2) / 100;
                    //        lbltax2.Text = tax2amt.ToString();
                    //    }
                    //    else if (Global.Default_Purchase_Tax2On == "Gross")
                    //    {
                    //        tax2amt = (GrossAmount * TAX2) / 100;
                    //        lbltax2.Text = tax2amt.ToString();
                    //    }
                    //}
                    //if (Global.Default_Sales_Tax3Check == "1")
                    //{
                    //    if (Global.Default_Purchase_Tax3On == "Nt Amt")
                    //    {
                    //        tax3amt = (NetAmount * TAX3) / 100;
                    //        lbltax2.Text = tax3amt.ToString();
                    //    }
                    //    else if (Global.Default_Purchase_Tax3On == "Tax 1")
                    //    {
                    //        tax3amt = (tax1amt * TAX3) / 100;
                    //        lbltax2.Text = tax3amt.ToString();
                    //    }
                    //    else if (Global.Default_Purchase_Tax3On == "Tax 2")
                    //    {
                    //        tax3amt = (tax2amt * TAX3) / 100;
                    //        lbltax2.Text = tax3amt.ToString();
                    //    }
                    //    if (Global.Default_Purchase_Tax3On == "Gross")
                    //    {
                    //        tax3amt = (GrossAmount * TAX3) / 100;
                    //        lbltax2.Text = tax3amt.ToString();
                    //    }
                    //}

                }
            }
            catch (Exception)
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
                for (int i = 1; i < grdPurchaseInvoice.RowsCount - 1; i++)
                {
                    if (i == grdPurchaseInvoice.Rows.Count)
                        return;
                    double m_TotalQuantity = 0;
                    string m_Value = Convert.ToString(grdPurchaseInvoice[i, (int)GridColumn.Qty].Value);
                    if (m_Value.Length == 0)
                        m_TotalQuantity = 0;
                    else
                        m_TotalQuantity = Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.Qty].Value);

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
        private void CalculateTotalCustomDuty()
        {
            try
            {
                double TotalCustomDuty = 0;
                for (int i = 1; i < grdPurchaseInvoice.RowsCount - 1; i++)
                {
                    if (i == grdPurchaseInvoice.Rows.Count)
                        return;
                    double m_TotalCustomDuty = 0;
                    string m_Value = Convert.ToString(grdPurchaseInvoice[i, (int)GridColumn.CustomDuty].Value);
                    if (m_Value.Length == 0 || m_Value == "0" || m_Value == "0.00")
                        m_TotalCustomDuty = 0;
                    else
                        m_TotalCustomDuty = Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.CustomDuty].Value);

                    TotalCustomDuty += m_TotalCustomDuty;

                }
                lblCustomduty.Text = TotalCustomDuty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblGrandTotalAfterCustomduty.Text = (Convert.ToDecimal(TotalCustomDuty) + Convert.ToDecimal(lblGrandTotal.Text)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                //Global.MsgError("Error in Total Quantity calucation!"); 
            }
        }

        private void CalculateTotalFreight()
        {
            try
            {
                double TotalFreight = 0;
                for (int i = 1; i < grdPurchaseInvoice.RowsCount - 1; i++)
                {
                    if (i == grdPurchaseInvoice.Rows.Count)
                        return;
                    double m_TotalFreight = 0;
                    string m_Value = Convert.ToString(grdPurchaseInvoice[i, (int)GridColumn.Freight].Value);
                    if (m_Value.Length == 0 || m_Value == "0" || m_Value == "0.00")
                        m_TotalFreight = 0;
                    else
                        m_TotalFreight = Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.Freight].Value);

                    TotalFreight += m_TotalFreight;

                    lblFreight.Text = TotalFreight.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    lblGrandTotalAfterCustomduty.Text = (TotalFreight + Convert.ToDouble(lblGrandTotal.Text) + Convert.ToDouble(lblCustomduty.Text)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
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
                for (int i = 1; i < grdPurchaseInvoice.RowsCount - 1; i++)
                {
                    if (i == grdPurchaseInvoice.Rows.Count)
                        return;
                    double m_TotalDiscount = 0;
                    string m_Value = Convert.ToString(grdPurchaseInvoice[i, (int)GridColumn.SplDisc].Value);
                    if (m_Value.Length == 0)
                        m_TotalDiscount = 0;
                    else

                        m_TotalDiscount = Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.SplDisc].Value);

                    TotalDiscount += m_TotalDiscount;

                    lblSpecialDiscount.Text = TotalDiscount.ToString();

                }
                lblSpecialDiscount.Text = TotalDiscount.ToString();

            }
            catch (Exception)
            {

                Global.MsgError("Error in Total Discount calucation!");
            }
        }

        //product code

        private void ProductCode_Selected(object sender, EventArgs e)
        {
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;

                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception)
            {

            }
            int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Using the name find corresponding code
            DataTable dt = Product.GetProductByCode(ct.DisplayText);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row

                    grdPurchaseInvoice[(CurRow), (int)GridColumn.ProductName].Value = dr["EngName"].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double PurchaseRate = Math.Round(Convert.ToDouble(dr["PurchaseRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.

                    grdPurchaseInvoice[(CurRow), (int)GridColumn.PurchaseRate].Value = PurchaseRate.ToString();
                    grdPurchaseInvoice[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
                int RowsCount = grdPurchaseInvoice.RowsCount;
                string LastServicesCell = (string)grdPurchaseInvoice[RowsCount - 1, 3].Value;
                ////Check whether the new row is already added
                if (LastServicesCell != "(NEW)")
                {
                    //AddRowProduct(RowsCount);
                    AddRowProduct1(RowsCount);
                    //Clear (NEW) on other colums as well
                    ClearNew(RowsCount - 1);
                }
                WriteAmount(CurRow, 1);//Write amount on grid'cell when quantity is unit
                WriteNetAmount(CurRow);//Write Net amount on corresponding cell of grid.It can also handle when value of quantity is unit and discount is 0
                CalculateGrossAmount();//After summing up all gross amount,this function display the value in label
                CalculateNetAmount(); //After summing up all net amount,this function display the value in lablel
                CalculateTotalQuantity();
            }
        }

        private void Product_Selected(object sender, EventArgs e)
        {
            //if (!hasChanged)
            //{
            //    ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            //    frmLOVLedger frm = new frmLOVLedger(this);
            //    frm.ShowDialog();
            //}  
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;

            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;

                if (ct.DisplayText == "" || ct.DisplayText == "(NEW)")
                    return;
            }
            catch (Exception)
            {

            }
            int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Using the name find corresponding code
            //AccSwift.Forms.Inventory.frmListOfProduct fm = new Forms.Inventory.frmListOfProduct(this);
            //fm.ShowDialog();
            DataTable dt = Product.GetProductByName(ct.DisplayText);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row

                    grdPurchaseInvoice[(CurRow), (int)GridColumn.Code_No].Value = dr["ProductCode"].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double PurchaseRate = Math.Round(Convert.ToDouble(dr["PurchaseRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.

                    grdPurchaseInvoice[(CurRow), (int)GridColumn.PurchaseRate].Value = PurchaseRate.ToString();
                    grdPurchaseInvoice[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
                int RowsCount = grdPurchaseInvoice.RowsCount;
                string LastServicesCell = (string)grdPurchaseInvoice[RowsCount - 1, (int)GridColumn.ProductName].Value;
                ////Check whether the new row is already added
                if (LastServicesCell != "(NEW)")
                {
                    // AddRowProduct(RowsCount);
                    AddRowProduct1(RowsCount);
                    //Clear (NEW) on other colums as well
                    ClearNew(RowsCount - 1);
                }
                WriteAmount(CurRow, 1);//Write amount on grid'cell when quantity is unit
                WriteNetAmount(CurRow);//Write Net amount on corresponding cell of grid.It can also handle when value of quantity is unit and discount is 0
                CalculateGrossAmount();//After summing up all gross amount,this function display the value in label
                CalculateNetAmount(); //After summing up all net amount,this function display the value in lablel
                CalculateTotalQuantity();
            }
        }

        private void WriteAmount(int CurRow, double Qty)
        {
            try
            {
                object purchRate = grdPurchaseInvoice[CurRow, (int)GridColumn.PurchaseRate].Value;

                string PurchaseRate = (purchRate != null) ? purchRate.ToString() : "0";

                double Amount = Convert.ToDouble(Qty) * Convert.ToDouble(PurchaseRate);
                double discount = 0;
                string str_Discount = grdPurchaseInvoice[CurRow, (int)GridColumn.SplDisc].Value.ToString();
                if (str_Discount.Length <= 0)
                    discount = 0;
                else
                    discount = Convert.ToDouble(grdPurchaseInvoice[CurRow, (int)GridColumn.SplDisc].Value.ToString());

                grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
                grdPurchaseInvoice[CurRow, (int)GridColumn.NetAmount].Value = (Amount - discount).ToString();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void WriteNetAmount(int CurRow)
        {
            try
            {
                string Amount = grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value.ToString();
                string Discount = grdPurchaseInvoice[CurRow, (int)GridColumn.SplDisc].Value.ToString();
                double NetAmount = Convert.ToDouble(Amount) - Convert.ToDouble(Discount);
                grdPurchaseInvoice[CurRow, (int)GridColumn.NetAmount].Value = NetAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //Write VAT Amount in the Respective Column
                double VatAmt = 0;

                string pname = grdPurchaseInvoice[CurRow, (int)GridColumn.ProductName].Value.ToString();
                int productid = Product.GetProductIDFromName(pname, Lang.English);
                //DataTable dtcheckvatapplicable = Product.GetProductByID(productid);
                //DataRow drcheckvatapplicable = dtcheckvatapplicable.Rows[0];

                //if (drcheckvatapplicable["IsVatApplicable"].ToString() == "1")
                //{
                //    if (Vatvalue == "1")
                //    {
                //        VatAmt = (NetAmount * VAT) / 100;
                //        grdPurchaseInvoice[CurRow, (int)GridColumn.VAT].Value = VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //    }

                //}
                //else
                //{
                CurrRowPos = CurRow;
                VatAmt = Convert.ToDouble(CalculateVat(Convert.ToDecimal(NetAmount)));
                grdPurchaseInvoice[CurRow, (int)GridColumn.VAT].Value = VatAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //}
                double CostBeforeCustomeDuty = NetAmount + VatAmt;
                grdPurchaseInvoice[CurRow, (int)GridColumn.CustomDuty].Value = (CostBeforeCustomeDuty * (Convert.ToDouble(grdPurchaseInvoice[CurRow, (int)GridColumn.CustomDuty_Percent].Value) / 100)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        /// <summary>
        /// Check whether the Current Row is a new row or not
        /// </summary>
        /// <param name="CurRow"></param>
        /// <returns></returns>
        /// 

        private bool isNewRow(int CurRow)
        {
            if (grdPurchaseInvoice[CurRow, (int)GridColumn.Code_No].Value == "(NEW)")
                return true;
            else
                return false;
        }

        private void Qty_Modified(object sender, EventArgs e)
        {
            try
            {
                //find the current row of source grid
                int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;

                int RowCount = grdPurchaseInvoice.RowsCount;

                object Qty = ((TextBox)sender).Text;
                // bool IsInt = Misc.IsInt(Qty, false);//This function check whether variable is integer or not?

                //with help of hidden column PurchaseOrderDetailsID,we can find the the details info PurchaseOrderDetails

                if ((grdPurchaseInvoice[CurRow, (int)GridColumn.PurchaseOrderDetailID].Value.ToString() != "") && (Convert.ToInt32(grdPurchaseInvoice[CurRow, (int)GridColumn.PurchaseOrderDetailID].Value) > 0))//It means Invoice is going to make according to Order,so we have to check Quantity
                {

                    DataTable dtPurchaseOrderDetails = PurchaseOrder.GetPurchaseOrderDetailsByDetailsID(Convert.ToInt32(grdPurchaseInvoice[CurRow, (int)GridColumn.PurchaseOrderDetailID].Value));
                    DataRow drPurchaseOrderDetails = dtPurchaseOrderDetails.Rows[0];
                    double InvoiceQty = Convert.ToDouble(Qty);
                    double OrderQty = Convert.ToDouble(drPurchaseOrderDetails["Quantity"]);
                    if (InvoiceQty > OrderQty)
                    {
                        Global.Msg("Entered Quantity is greater than Pending Quantity");
                        return;
                    }
                }
                //if (IsInt == false)
                //{
                //    //Global.MsgError("The quantity you posted is invalid! Please post the integer value");
                //    Global.MsgError("The quantity Cannot be blank, Please Enter the valid quantity");
                //   // grdPurchaseInvoice.Focus();
                //    //this.Focus();
                //    grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value = "";
                //    grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value = "1.0";
                //    ((TextBox)sender).Text = "1";
                //    //grdPurchaseInvoice.Selection.Focus(new SourceGrid.Position(CurRow, 4), true);

                //    return;
                //}

                //Check whether the value of quantity is zero or not?
                if (Convert.ToDouble(Qty) == 0)
                {
                    Global.MsgError("The Quantity shouldnot be zero. Fill the Quantity first!");
                    grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value = "1";
                    grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value = "0";
                    ((TextBox)sender).Text = " ";
                    return;
                }
                // check if decimal palces is applicable or not
                decimal decQty = Convert.ToDecimal(Qty);
                int intQty = (int)decQty;

                if ((Convert.ToDecimal(intQty) != decQty))
                {
                    if (!Product.IsDecimalApplicable(Convert.ToInt32(grdPurchaseInvoice[CurRow, (int)GridColumn.ProductID].Value)))
                    {
                        Global.Msg("Decimal places is not applicable for this product ! Plesse enter valid value !");
                        //grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value = "1.0";
                        ((TextBox)sender).Text = intQty.ToString();
                        Qty = intQty;
                    }
                }

                TotalQty += Convert.ToDouble(Qty);
                lblTotalQty.Text = TotalQty.ToString();
                WriteAmount(CurRow, Convert.ToDouble(Qty));

                grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value = Qty;
                Unit_Changed(sender, e);

                //CalculateGrossAmount();
                ////Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
                //WriteNetAmount(CurRow);
                ////CalculateVat(0);
                //CalculateNetAmount();
                //CalculateTotalQuantity();
                //CalculateTotalCustomDuty();
                //VATAmt_FocusLeft(sender, e);
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

        private void PurchaseRate_Modified(object sender, EventArgs e)
        {
            try
            {
                //find the current row of source grid
                int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;
                int RowCount = grdPurchaseInvoice.RowsCount;

                object PurchaseRate = ((TextBox)sender).Text;
                grdPurchaseInvoice[CurRow, (int)GridColumn.PurchaseRate].Value = PurchaseRate.ToString();

                bool IsDouble = Misc.IsNumeric(PurchaseRate);//This function check whether variable is Double  or not?

                if (IsDouble == false)
                {
                    Global.MsgError("The Purchase Rate you posted is invalid! Please post the integer value");
                    grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value = "";
                    grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value = "1";
                    // grdPurchaseInvoice[CurRow, 4].Value = " ";
                    return;
                }

                grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value = (Convert.ToDecimal(grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value) * Convert.ToDecimal(PurchaseRate)).ToString();
                Unit_Changed(sender, e);

                //if (grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value.ToString() == " " || grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value.ToString() == "0")
                //{
                //    // MessageBox.Show("Please Enter Valid Quantity");
                //    grdPurchaseInvoice.Selection.Focus(new SourceGrid.Position(CurRow, (int)GridColumn.Qty), true);
                //}
                //else
                //{
                //    string Qty = grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value.ToString();
                //    double Amount = Convert.ToDouble(Qty) * Convert.ToDouble(PurchaseRate);
                //    grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
                //    CalculateGrossAmount();
                //    WriteNetAmount(CurRow);
                //    CalculateNetAmount();
                //    CalculateTotalQuantity();
                //    CalculateTotalCustomDuty();
                //    //VATAmt_FocusLeft(sender, e);

                //}

                // grdPurchaseInvoice.Selection.Focus(new SourceGrid.Position(CurRow, 4), true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DiscPercentage_Modified(object sender, EventArgs e)
        {
            int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
            double Amount = 0;
            Amount = Convert.ToDouble(grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value);
            if (Amount == 0)
            {
                Global.MsgError("The Amount shouldnot be Zero.Please post the required value in corresponding field!");
                grdPurchaseInvoice[CurRow, (int)GridColumn.SplDisc_Percent].Value = "0";
                grdPurchaseInvoice[CurRow, (int)GridColumn.SplDisc].Value = "0";
                grdPurchaseInvoice[CurRow, (int)GridColumn.NetAmount].Value = "0";
                return;
            }
            object DisPercentage = ((TextBox)sender).Text;
            bool IsDouble = Misc.IsNumeric(DisPercentage);//This function check whether variable is double or not?
            if (IsDouble == false)
            {
                Global.MsgError("The Discount Percentage you posted is invalid! Please post the  numeric value");
                return;
            }
            double Discount = Math.Round(((Amount * Convert.ToDouble(DisPercentage)) / 100), Global.DecimalPlaces);
            //double NetAmount = Math.Round((Amount - Discount), Global.DecimalPlaces);

            if (isNewRow(CurRow))
                return;

            grdPurchaseInvoice[(CurRow), (int)GridColumn.SplDisc].Value = Discount.ToString();

            WriteNetAmount(CurRow);
            CalculateTotalDiscount();
            CalculateNetAmount();
            CalculateTotalCustomDuty();

        }

        private void Discount_Modified(object sender, EventArgs e)
        {
            try
            {
                int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
                double Amount = 0;
                Amount = Convert.ToDouble(grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value);
                if (Amount == 0)
                {
                    Global.MsgError("The Amount shouldnot be Zero. Please post the required value in corresponding field!");
                    grdPurchaseInvoice[CurRow, (int)GridColumn.SplDisc_Percent].Value = "0";
                    grdPurchaseInvoice[CurRow, (int)GridColumn.SplDisc].Value = "0";
                    grdPurchaseInvoice[CurRow, (int)GridColumn.NetAmount].Value = "0";
                    return;
                }
                object Discount = ((TextBox)sender).Text;
                bool IsDouble = Misc.IsNumeric(Discount);//This function check whether variable is double or not?
                if (IsDouble == false)
                {
                    Global.MsgError("The Discount Amount you posted is invalid! Please post the  numeric value");
                    grdPurchaseInvoice[CurRow, (int)GridColumn.SplDisc].Value = "0";
                    grdPurchaseInvoice[CurRow, (int)GridColumn.SplDisc_Percent].Value = "0";
                    ((TextBox)sender).Text = "0";
                    grdPurchaseInvoice[CurRow, (int)GridColumn.NetAmount].Value = Amount.ToString();
                    return;
                }
                double DiscPercentage = Math.Round(((Convert.ToDouble(Discount) * 100) / Amount), Global.DecimalPlaces);
                //double NetAmount = Math.Round((Amount - Convert.ToDouble(Discount)), Global.DecimalPlaces);
                if (isNewRow(CurRow))
                    return;
                grdPurchaseInvoice[(CurRow), (int)GridColumn.SplDisc_Percent].Value = DiscPercentage.ToString();
                grdPurchaseInvoice[(CurRow), (int)GridColumn.NetAmount].Value = (Amount - Convert.ToDouble(Discount)).ToString();
                grdPurchaseInvoice[(CurRow), (int)GridColumn.SplDisc].Value = Convert.ToDouble(Discount).ToString();

                //  grdPurchaseInvoice[(CurRow), 6].Value = Amount - Convert.ToDouble(Discount);
                WriteNetAmount(CurRow);
                CalculateNetAmount();
                CalculateTotalDiscount();
                CalculateTotalCustomDuty();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void CustomDutyPer_Modified(object sender, EventArgs e)
        {
            try
            {
                int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid

                object CDPercentage = ((TextBox)sender).Text;
                grdPurchaseInvoice[CurRow, (int)GridColumn.CustomDuty_Percent].Value = Convert.ToDouble(CDPercentage).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                double NetAmt = Convert.ToDouble(grdPurchaseInvoice[CurRow, (int)GridColumn.NetAmount].Value);
                double VatAmt = Convert.ToDouble(grdPurchaseInvoice[CurRow, (int)GridColumn.VAT].Value);
                grdPurchaseInvoice[CurRow, (int)GridColumn.CustomDuty].Value = ((NetAmt + VatAmt) * (Convert.ToDouble(CDPercentage) / 100)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                CalculateTotalCustomDuty();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                
            }
        }
        private void CustomDutyAmount_Modified(object sender, EventArgs e)
        {
            try
            {
                int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
                double NetAmt = Convert.ToDouble(grdPurchaseInvoice[CurRow, (int)GridColumn.NetAmount].Value);
                double VatAmt = Convert.ToDouble(grdPurchaseInvoice[CurRow, (int)GridColumn.VAT].Value);
                object CustomDuty = ((TextBox)sender).Text;
                bool IsDouble = Misc.IsNumeric(CustomDuty);//This function check whether variable is double or not?
                if (IsDouble == false)
                {
                    Global.MsgError("The Custom Duty Amount you posted is invalid! Please post the  numeric value");
                    return;
                }
                double CustomDutyPercentage = 0;
                if (NetAmt + VatAmt == 0)
                {
                    CustomDutyPercentage = 0.00;
                }
                else
                {
                    CustomDutyPercentage = Math.Round(((Convert.ToDouble(CustomDuty) * 100) / (NetAmt + VatAmt)), Global.DecimalPlaces);
                }
                grdPurchaseInvoice[CurRow, (int)GridColumn.CustomDuty_Percent].Value = CustomDutyPercentage.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                grdPurchaseInvoice[CurRow, (int)GridColumn.CustomDuty].Value = Convert.ToDouble(CustomDuty).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                if (isNewRow(CurRow))
                    return;
                CalculateTotalCustomDuty();
                //VATAmt_FocusLeft(sender, e);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);                
            }

        }
        private void Freight_Modified(object sender, EventArgs e)
        {
            int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
            object Freight = ((TextBox)sender).Text;
            grdPurchaseInvoice[CurRow, 17].Value = Convert.ToDouble(Freight).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //CalculateTotalFreight();
            try
            {
                double TotalFreight = 0;
                for (int i = 1; i < grdPurchaseInvoice.RowsCount - 1; i++)
                {
                    if (i == grdPurchaseInvoice.Rows.Count)
                        return;
                    double m_TotalFreight = 0;
                    string m_Value = Convert.ToString(grdPurchaseInvoice[i, (int)GridColumn.Freight].Value);
                    if (m_Value.Length == 0 || m_Value == "0" || m_Value == "0.00")
                        m_TotalFreight = 0;
                    else
                        m_TotalFreight = Convert.ToDouble(grdPurchaseInvoice[i, (int)GridColumn.Freight].Value);

                    TotalFreight += m_TotalFreight;

                    lblFreight.Text = TotalFreight.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    lblGrandTotalAfterCustomduty.Text = (TotalFreight + Convert.ToDouble(lblGrandTotal.Text) + Convert.ToDouble(lblCustomduty.Text)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                //Global.MsgError("Error in Total Quantity calucation!"); 
            }

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
            grdPurchaseInvoice[0, (int)GridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del");
            grdPurchaseInvoice[0, (int)GridColumn.Del].Column.Width = 25;

            grdPurchaseInvoice[0, (int)GridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdPurchaseInvoice[0, (int)GridColumn.SNo].Column.Width = 40;

            grdPurchaseInvoice[0, (int)GridColumn.Code_No] = new SourceGrid.Cells.ColumnHeader("Code");
            grdPurchaseInvoice[0, (int)GridColumn.Code_No].Column.Width = 60;

            grdPurchaseInvoice[0, (int)GridColumn.ProductName] = new SourceGrid.Cells.ColumnHeader("Product Name");
            grdPurchaseInvoice[0, (int)GridColumn.ProductName].Column.Width = 120;

            grdPurchaseInvoice[0, (int)GridColumn.Qty] = new SourceGrid.Cells.ColumnHeader("Qty");
            grdPurchaseInvoice[0, (int)GridColumn.Qty].Column.Width = 50;

            grdPurchaseInvoice[0, (int)GridColumn.Units] = new SourceGrid.Cells.ColumnHeader("Unit");
            grdPurchaseInvoice[0, (int)GridColumn.Units].Column.Width = 100;
            grdPurchaseInvoice.Columns[(int)GridColumn.Units].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdPurchaseInvoice[0, (int)GridColumn.Default_Qty] = new SourceGrid.Cells.ColumnHeader("Default_Qty");
            grdPurchaseInvoice[0, (int)GridColumn.Default_Qty].Column.Visible = false;

            grdPurchaseInvoice[0, (int)GridColumn.Default_Unit] = new SourceGrid.Cells.ColumnHeader("Default_Unit");
            grdPurchaseInvoice[0, (int)GridColumn.Default_Unit].Column.Visible = false;

            grdPurchaseInvoice[0, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.ColumnHeader("PurchaseRate");
            grdPurchaseInvoice[0, (int)GridColumn.PurchaseRate].Column.Width = 80;

            grdPurchaseInvoice[0, (int)GridColumn.Amount] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdPurchaseInvoice[0, (int)GridColumn.Amount].Column.Width = 100;

            grdPurchaseInvoice[0, (int)GridColumn.SplDisc_Percent] = new SourceGrid.Cells.ColumnHeader("Spl. Disc%");
            grdPurchaseInvoice[0, (int)GridColumn.SplDisc_Percent].Column.Width = 65;

            grdPurchaseInvoice[0, (int)GridColumn.SplDisc] = new SourceGrid.Cells.ColumnHeader("Spl. Disc");
            grdPurchaseInvoice[0, (int)GridColumn.SplDisc].Column.Width = 60;

            grdPurchaseInvoice[0, (int)GridColumn.NetAmount] = new SourceGrid.Cells.ColumnHeader("Net Amount");
            grdPurchaseInvoice[0, (int)GridColumn.NetAmount].Column.Width = 85;

            grdPurchaseInvoice[0, (int)GridColumn.PurchaseOrderDetailID] = new SourceGrid.Cells.ColumnHeader("PurchaseOrderDetailsID");
            grdPurchaseInvoice[0, (int)GridColumn.PurchaseOrderDetailID].Column.Width = 5;
            grdPurchaseInvoice[0, (int)GridColumn.PurchaseOrderDetailID].Column.Visible = false;

            grdPurchaseInvoice[0, (int)GridColumn.VAT] = new SourceGrid.Cells.ColumnHeader("VAT");
            grdPurchaseInvoice[0, (int)GridColumn.VAT].Column.Width = 60;

            grdPurchaseInvoice[0, (int)GridColumn.CustomDuty_Percent] = new SourceGrid.Cells.ColumnHeader("Custom Duty%");
            grdPurchaseInvoice[0, (int)GridColumn.CustomDuty_Percent].Column.Width = 90;

            grdPurchaseInvoice[0, (int)GridColumn.CustomDuty] = new SourceGrid.Cells.ColumnHeader("Custom Duty Amt");
            grdPurchaseInvoice[0, (int)GridColumn.CustomDuty].Column.Width = 100;

            grdPurchaseInvoice[0, (int)GridColumn.Freight] = new SourceGrid.Cells.ColumnHeader("Freight");
            grdPurchaseInvoice[0, (int)GridColumn.Freight].Column.Width = 100;

            grdPurchaseInvoice[0, (int)GridColumn.ProductID] = new SourceGrid.Cells.ColumnHeader("ProductID");
            grdPurchaseInvoice[0, (int)GridColumn.ProductID].Column.Visible = false;

            if (iscustomduty == "0")
            {
                grdPurchaseInvoice[0, (int)GridColumn.CustomDuty_Percent].Column.Visible = false;
                grdPurchaseInvoice[0, (int)GridColumn.CustomDuty].Column.Visible = false;
                //grdPurchaseInvoice[0, (int)GridColumn.Freight].Column.Visible = false;
                lblCustomduty.Visible = false;
                label17.Visible = false;
                lblGrandTotalAfterCustomduty.Visible = false;
            }
            else
            {
                label12.Text = "Total Before Custom Duty";
            }
            if (Vatvalue == "0")
            {
                grdPurchaseInvoice[0, (int)GridColumn.VAT].Column.Visible = false;
                label7.Visible = false;
                lblVat.Visible = false;
            }
            //else
            //{
            //    grdPurchaseInvoice[0, (int)GridColumn.VAT].Column.Visible = true;
            //    grdPurchaseInvoice[0, (int)GridColumn.CustomDuty_Percent].Column.Visible = true;
            //    grdPurchaseInvoice[0, (int)GridColumn.CustomDuty].Column.Visible = true;
            //    lblcustomduty.Visible = true;
            //    label14.Visible = true;
            //    label17.Visible = true;
            //    lblgrandtotalaftercustomduty.Visible = true;
            //    label12.Text = "Total Before Custom Duty";

            //}


        }

        /// <summary>
        /// Adds the row in the Purchase Invoice field
        /// </summary>
        /// 

        private void AddRowProduct(int RowCount)
        {
            try
            {
                //Add a new row
                grdPurchaseInvoice.Redim(Convert.ToInt32(RowCount + 1), grdPurchaseInvoice.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdPurchaseInvoice[i, (int)GridColumn.Del] = btnDelete;
                grdPurchaseInvoice[i, (int)GridColumn.Del].AddController(evtDelete);
                grdPurchaseInvoice[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdPurchaseInvoice[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();
                grdPurchaseInvoice[i, (int)GridColumn.Code_No].Value = "(NEW)";
                SourceGrid.Cells.Editors.ComboBox cboProduct = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                SourceGrid.Cells.Editors.ComboBox cboProductCode = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                DataTable dt = Product.GetProductList(0);
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
                List<string> lstProduct = new List<string>();
                List<string> lstProductCode = new List<string>();
                for (int i1 = 0; i1 < dt.Rows.Count; i1++)
                {
                    DataRow dr = dt.Rows[i1];
                    lstProduct.Add(dr[LangField].ToString());
                    lstProductCode.Add(dr["ProductCode"].ToString());
                }

                //for product code

                cboProductCode.StandardValues = (string[])lstProductCode.ToArray<string>();
                cboProductCode.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
                cboProductCode.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboProductCode.Control.LostFocus += new EventHandler(ProductCode_Selected);
                cboProductCode.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseInvoice[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell("", cboProductCode);
                grdPurchaseInvoice[i, (int)GridColumn.Code_No].AddController(evtProuctCode);
                grdPurchaseInvoice[i, (int)GridColumn.Code_No].Value = "(NEW)";

                //for product
                cboProduct.StandardValues = (string[])lstProduct.ToArray<string>();
                cboProduct.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
                cboProduct.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboProduct.Control.LostFocus += new EventHandler(Product_Selected);
                cboProduct.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseInvoice[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell("", cboProduct);
                grdPurchaseInvoice[i, (int)GridColumn.ProductName].AddController(evtProduct);
                grdPurchaseInvoice[i, (int)GridColumn.ProductName].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseInvoice[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("", txtQty);
                grdPurchaseInvoice[i, (int)GridColumn.Qty].Value = "0";

                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseInvoice[i, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                grdPurchaseInvoice[i, (int)GridColumn.PurchaseRate].Value = "0";

                grdPurchaseInvoice[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell();
                grdPurchaseInvoice[i, (int)GridColumn.Amount].Value = "0";

                SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);
                txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                txtDiscPercentage.Control.Text = "0";

                grdPurchaseInvoice[i, (int)GridColumn.SplDisc_Percent] = new SourceGrid.Cells.Cell("", txtDiscPercentage);
                grdPurchaseInvoice[i, (int)GridColumn.SplDisc_Percent].Value = "0";

                SourceGrid.Cells.Editors.TextBox txtDiscount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscount.Control.LostFocus += new EventHandler(Discount_Modified);
                txtDiscount.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseInvoice[i, (int)GridColumn.SplDisc] = new SourceGrid.Cells.Cell("0", txtDiscount);
                grdPurchaseInvoice[i, (int)GridColumn.SplDisc].Value = "0";

                grdPurchaseInvoice[i, (int)GridColumn.NetAmount] = new SourceGrid.Cells.Cell("Net Amount");
                grdPurchaseInvoice[i, (int)GridColumn.NetAmount].Value = "0";

                grdPurchaseInvoice[i, (int)GridColumn.PurchaseOrderDetailID] = new SourceGrid.Cells.Cell("");
                grdPurchaseInvoice[i, (int)GridColumn.PurchaseOrderDetailID].Value = "0";

                //grdPurchaseInvoice[i, 11] = new SourceGrid.Cells.Cell("");
                //grdPurchaseInvoice[i, 11].Value = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        //A function from the Interface IfrmAccClassID. Used to apply the Datatable to this form from AddAccClass Form     

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            // check if transaction amount is greater than reference amount
            //if (!CheckAmtAgainstRefAmt())
            //{
            //    return;
            //}
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }


            #region BLOCK FOR MANUAL VOUCHER NUMBERING TYPE

            VoucherConfiguration m_VouConfig = new VoucherConfiguration();
            if (SeriesID.ID > 0)
            {
                DataTable dtVouConfigInfo = m_VouConfig.GetVouNumConfiguration(Convert.ToInt32(SeriesID.ID));

                foreach (DataRow drVouConfigInfo in dtVouConfigInfo.Rows)
                {

                    if (drVouConfigInfo["NumberingType"].ToString() == "Manual")
                    {

                        //Enter in this block only when VoucherNumberingType is Manual
                        //Checking for Manual VoucherNumberingType
                        try
                        {

                            string returnStr = m_VouConfig.ValidateManualVouNum(txtVoucherNo.Text, Convert.ToInt32(SeriesID.ID));

                            switch (returnStr)
                            {
                                case "INVALID_SERIES":
                                    {
                                        MessageBox.Show("Invalid Series Name,please select valid Series Name and try again!");
                                        return;
                                    }
                                    //break;
                                case "BLANK_WARN":
                                    if (MessageBox.Show("Voucher Number is Blank, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                    {
                                        return;
                                    }
                                    break;
                                case "BLANK_DONT_ALLOW":
                                    MessageBox.Show("Voucher Number is Blank,Please fill the Voucher Number first!");
                                    return;
                                    //break;
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
                                    //break;

                            }

                        }
                        catch (Exception ex)
                        {

                            Global.Msg(ex.Message);
                        }
                    }
                }
            }
            #endregion
            //Check Validation
            if (!Validate())
                return;

            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (int tag in arrNode)
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
            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    try
                    {
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
                        NewGrid = NewGrid + "Voucher No" + "-" + txtVoucherNo.Text + "," + "Voucher Date" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Cash/Party" + "-" + cboCashPartyAcc.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdPurchaseInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string productname = grdPurchaseInvoice[i + 1, (int)GridColumn.ProductName].Value.ToString() + ",";
                            string qty = grdPurchaseInvoice[i + 1, (int)GridColumn.Qty].Value.ToString() + ",";
                            string rate = grdPurchaseInvoice[i + 1, (int)GridColumn.PurchaseRate].Value.ToString() + ",";
                            string amt = grdPurchaseInvoice[i + 1, (int)GridColumn.Amount].Value.ToString() + ",";
                            NewGrid = NewGrid + string.Concat(productname, qty, rate, amt) + ",";
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        //Read from sourcegrid and store it to table
                        DataTable PurchaseInvoiceDetails = new DataTable();
                        PurchaseInvoiceDetails.Columns.Add("Code",typeof(string));
                        PurchaseInvoiceDetails.Columns.Add("Product",typeof(string));
                        PurchaseInvoiceDetails.Columns.Add("Quantity",typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("PurchaseRate",typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("Amount", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("DiscPercentage", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("Discount", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("NetAmount", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("PurchaseOrderDetailsID",typeof(Int32));
                        PurchaseInvoiceDetails.Columns.Add("ProductID",typeof(Int32));
                        PurchaseInvoiceDetails.Columns.Add("VAT", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("CustomDutyPercentage", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("CustomDuty", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("Freight", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("QtyUnitID", typeof(Int32));

                        for (int i = 0; i < grdPurchaseInvoice.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            PurchaseInvoiceDetails.Rows.Add(grdPurchaseInvoice[i + 1, (int)GridColumn.Code_No].Value,
                                grdPurchaseInvoice[i + 1, (int)GridColumn.ProductName].Value,
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.Default_Qty].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.PurchaseRate].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.Amount].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.SplDisc_Percent].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.SplDisc].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.NetAmount].Value),
                                Convert.ToInt32(grdPurchaseInvoice[i + 1, (int)GridColumn.PurchaseOrderDetailID].Value),
                                grdPurchaseInvoice[i + 1, (int)GridColumn.ProductID].Value,
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.VAT].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.CustomDuty_Percent].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.CustomDuty].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.Freight].Value),
                                Convert.ToInt32(grdPurchaseInvoice[i + 1, (int)GridColumn.Units].Value));

                        }

                        DateTime PurchaseInvoice_Date = Date.ToDotNet(txtDate.Text);
                        ListItem liPurchaseLedgerID = new ListItem();
                        liPurchaseLedgerID = (ListItem)cboPurchaseAcc.SelectedItem;
                        //ListItem LiCashPartyLedgerID = new ListItem();
                        //LiCashPartyLedgerID = (ListItem)cmboCashPartyAcc.SelectedItem;
                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        int Tax1ID = AccountGroup.GetLedgerIDFromLedgerNumber(25717);
                        int Tax2ID = AccountGroup.GetLedgerIDFromLedgerNumber(25718);
                        int Tax3ID = AccountGroup.GetLedgerIDFromLedgerNumber(25719);
                        int VatID = AccountGroup.GetLedgerIDFromLedgerNumber(4698);
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        if (txtOrderNo.Text != "")
                            OrderNo = Convert.ToInt32(txtOrderNo.Text);
                        if (AccClassID.Count != 0)
                        {
                            m_PurchaseInvoice.Create1(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), liPurchaseLedgerID.Value.ToString(), Convert.ToInt32(cboCashPartyAcc.SelectedValue), cboCashPartyAcc.Text, Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), OrderNo, txtVoucherNo.Text, PurchaseInvoice_Date, txtRemarks.Text, PurchaseInvoiceDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check, OldGrid, NewGrid, isNew, lblTax1.Text, lblTax2.Text, lblTax3.Text, lblVat.Text, lblNetAmout.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblCustomduty.Text, lblFreight.Text, txtPartyBillNumber.Text, OF, m_dtRecurringSetting, dtReference);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_PurchaseInvoice.Create1(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), liPurchaseLedgerID.Value.ToString(),Convert.ToInt32(cboCashPartyAcc.SelectedValue), cboCashPartyAcc.Text, Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), OrderNo, txtVoucherNo.Text, PurchaseInvoice_Date, txtRemarks.Text, PurchaseInvoiceDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check, OldGrid, NewGrid, isNew, lblTax1.Text, lblTax2.Text, lblTax3.Text, lblVat.Text, lblNetAmout.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblCustomduty.Text, lblFreight.Text, txtPartyBillNumber.Text, OF, m_dtRecurringSetting, dtReference);
                        }
                        //Update the last AutoNumber in tblSeries,only if the voucher hide type is true
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }

                        Global.Msg("Purchase Invoice created successfully!");

                        // if the voucher is recurring and has been posted or saved, modify voucherposting table to set isPosted = true
                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_PurchaseInvoiceID, "PURCHASE_INVOICE");
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID);
                            m_isRecurring = false;
                        }
                        AccClassID.Clear();
                        ChangeState(EntryMode.NORMAL);
                        ClearVoucher();
                        ChangeState(EntryMode.NEW);
                        btnNew_Click(sender, e);

                        if (chkprintwhilesaving.Checked)
                        {
                            prntDirect = 1;
                            Navigation(Navigate.Last);
                            Print_Click(sender, e);
                            ClearVoucher();
                            ChangeState(EntryMode.NEW);
                            btnNew_Click(sender, e);
                        }
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
                                Global.Msg("ERROR: The Order Number already exists! Please Enter another Order Number!", MBType.Warning, "Error");
                                txtOrderNo.Focus();
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
                    try
                    {
                        isNew = false;
                        NewGrid = " ";
                        NewGrid = "Voucher No" + "-" + txtVoucherNo.Text + "," + "Voucher Date" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Cash/Party" + "-" + cboCashPartyAcc.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdPurchaseInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string productname = grdPurchaseInvoice[i + 1, (int)GridColumn.ProductName].Value.ToString() + ",";
                            string qty = grdPurchaseInvoice[i + 1, (int)GridColumn.Qty].Value.ToString() + ",";
                            string rate = grdPurchaseInvoice[i + 1, (int)GridColumn.PurchaseRate].Value.ToString() + ",";
                            string amt = grdPurchaseInvoice[i + 1, (int)GridColumn.Amount].Value.ToString() + ",";
                            NewGrid = NewGrid + string.Concat(productname, qty, rate, amt) + ",";
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        //Read from sourcegrid and store it to table
                        DataTable PurchaseInvoiceDetails = new DataTable();
                        PurchaseInvoiceDetails.Columns.Add("Code", typeof(string));
                        PurchaseInvoiceDetails.Columns.Add("Product", typeof(string));
                        PurchaseInvoiceDetails.Columns.Add("Quantity", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("PurchaseRate", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("Amount", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("DiscPercentage", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("Discount", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("NetAmount", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("PurchaseOrderDetailsID", typeof(Int32));
                        PurchaseInvoiceDetails.Columns.Add("ProductID", typeof(Int32));
                        PurchaseInvoiceDetails.Columns.Add("VAT", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("CustomDutyPercentage", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("CustomDuty", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("Freight", typeof(double));
                        PurchaseInvoiceDetails.Columns.Add("QtyUnitID", typeof(Int32));

                        for (int i = 0; i < grdPurchaseInvoice.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            PurchaseInvoiceDetails.Rows.Add(grdPurchaseInvoice[i + 1, (int)GridColumn.Code_No].Value,
                                grdPurchaseInvoice[i + 1, (int)GridColumn.ProductName].Value,
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.Default_Qty].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.PurchaseRate].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.Amount].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.SplDisc_Percent].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.SplDisc].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.NetAmount].Value),
                                Convert.ToInt32(grdPurchaseInvoice[i + 1, (int)GridColumn.PurchaseOrderDetailID].Value),
                                grdPurchaseInvoice[i + 1, (int)GridColumn.ProductID].Value,
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.VAT].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.CustomDuty_Percent].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.CustomDuty].Value),
                                Convert.ToDouble(grdPurchaseInvoice[i + 1, (int)GridColumn.Freight].Value),
                                Convert.ToInt32(grdPurchaseInvoice[i + 1, (int)GridColumn.Units].Value));
                        }

                        DateTime PurchaseInvoice_Date = Date.ToDotNet(txtDate.Text);
                        ListItem liPurchaseLedgerID = new ListItem();
                        liPurchaseLedgerID = (ListItem)cboPurchaseAcc.SelectedItem;

                        //ListItem LiCashPartyLedgerID = new ListItem();
                        //LiCashPartyLedgerID = (ListItem)cmboCashPartyAcc.SelectedItem;
                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        int Tax1ID = AccountGroup.GetLedgerIDFromLedgerNumber(25717);
                        int Tax2ID = AccountGroup.GetLedgerIDFromLedgerNumber(25718);
                        int Tax3ID = AccountGroup.GetLedgerIDFromLedgerNumber(25719);
                        int VatID = AccountGroup.GetLedgerIDFromLedgerNumber(4698);
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;

                        

                        string tax1="0";
                        string tax2="0";
                        string tax3="0";
                        if(isTax1)
                        {
                            tax1= lblTax1.Text;

                        }
                        if(isTax2)
                        {
                            tax2=lblTax2.Text;
                        }
                        if(isTax3)
                        {
                            tax3=lblTax3.Text;
                        }
                        //if (chkRecurring.Checked)
                        //{
                        //    m_dtRecurringSetting.Rows[0]["RVID"] = RSID;  // send id of voucher setting for modification
                        //    m_dtRecurringSetting.Rows[0]["VoucherID"] = txtPurchaseInvoiceID.Text;
                        //}
                        if (AccClassID.Count != 0)
                        {
                            m_PurchaseInvoice.Modify(Convert.ToInt32(txtPurchaseInvoiceID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), liPurchaseLedgerID.Value.ToString(), Convert.ToInt32(cboCashPartyAcc.SelectedValue), cboCashPartyAcc.Text.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, PurchaseInvoice_Date, txtRemarks.Text, PurchaseInvoiceDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check, OldGrid, NewGrid, isNew,tax1, tax2, tax3, lblVat.Text, lblNetAmout.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblCustomduty.Text, lblFreight.Text, txtPartyBillNumber.Text, OF, m_dtRecurringSetting, dtReference, ToDeleteRows);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_PurchaseInvoice.Modify(Convert.ToInt32(txtPurchaseInvoiceID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liPurchaseLedgerID.ID), liPurchaseLedgerID.Value.ToString(), Convert.ToInt32(cboCashPartyAcc.SelectedValue), cboCashPartyAcc.Text.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, PurchaseInvoice_Date, txtRemarks.Text, PurchaseInvoiceDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Purchase_Tax1On, Global.Default_Purchase_Tax1Check, Global.Default_Tax2, Global.Default_Purchase_Tax2On, Global.Default_Purchase_Tax2Check, Global.Default_Tax3, Global.Default_Purchase_Tax3On, Global.Default_Purchase_Tax3Check, OldGrid, NewGrid, isNew, tax1, tax2, tax3, lblVat.Text, lblNetAmout.Text, lblTotalQty.Text, lblGross.Text, lblSpecialDiscount.Text, lblCustomduty.Text, lblFreight.Text, txtPartyBillNumber.Text, OF, m_dtRecurringSetting, dtReference, ToDeleteRows);

                        }
                        Global.Msg("Purchase Invoice modified successfully!");
                        AccClassID.Clear();
                        // ChangeState(EntryMode.NORMAL);
                        ClearVoucher();
                        ChangeState(EntryMode.NEW);
                        btnNew_Click(sender, e);

                        if (chkprintwhilesaving.Checked)
                        {
                            prntDirect = 1;
                            Navigation(Navigate.Last);
                            Print_Click(sender, e);
                            ClearVoucher();
                            ChangeState(EntryMode.NEW);
                            btnNew_Click(sender, e);
                        }
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
            }
        }

        private bool Validate()
        {
            bool bValidate = false;
            FormHandle m_FH = new FormHandle();
            bValidate = m_FH.Validate();
            if (cboSeriesName.SelectedItem == null)
            {
                Global.MsgError("Invalid Series Name Selected");
                cboSeriesName.Focus();
                bValidate = false;
            }
            else if (cboCashPartyAcc.SelectedItem == null)
            {
                Global.MsgError("Invalid Cash/Party Account Selected");
                cboCashPartyAcc.Focus();
                bValidate = false;
            }
            else if (cboDepot.SelectedItem == null)
            {
                Global.MsgError("Invalid Depot Selected");
                cboDepot.Focus();
                bValidate = false;
            }
            else if (cboPurchaseAcc.SelectedItem == null)
            {
                Global.MsgError("Invalid Purchase Account Selected");
                cboPurchaseAcc.Focus();
                bValidate = false;
            }
            else if (!(grdPurchaseInvoice.Rows.Count > 2))
            {
                Global.MsgError("Invalid Account Ledger Selected in grid");
                grdPurchaseInvoice.Focus();
                bValidate = false;
            }

            m_FH.AddValidate(txtDate, DType.DATE, "Please enter valid date");
            return bValidate;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false, true);
                    IsFieldChanged = false;
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true, true);
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true, true);
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
            }
        }

        private void EnableControls(bool Enable)
        {
          btnCashParty.Enabled = txtPartyBillNumber.Enabled = cboProjectName.Enabled = btnDate.Enabled = chkRecurring.Enabled = btnSetup.Enabled = txtVoucherNo.Enabled = cboCashPartyAcc.Enabled = cboPurchaseAcc.Enabled = txtRemarks.Enabled = txtDate.Enabled = txtOrderNo.Enabled = grdPurchaseInvoice.Enabled = cboSeriesName.Enabled = cboDepot.Enabled = treeAccClass.Enabled = btnAddAccClass.Enabled = tabControl1.Enabled = Enable;
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

        private void cboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetVoucherNo();
        }

        /// <summary>
        /// Sets voucher number
        /// </summary>
        private void SetVoucherNo()
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
                            ClearPurchaseInvoice(true);
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
                    if (m_PurchaseInvoiceID > 0 && !m_isRecurring)
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            string value = Settings.GetSettings("FREEZE_STATUS");
            string startdate = Settings.GetSettings("FREEZE_START_DATE");
            string enddate = Settings.GetSettings("FREEZE_END_DATE");
            DateTime dstart = Convert.ToDateTime(startdate);
            DateTime dend = Convert.ToDateTime(enddate);
            DateTime tdate = Date.ToDotNet(txtDate.Text);
            if (value == "1" && tdate >= dstart && tdate <= dend)
            {
                MessageBox.Show("The transaction is Freeze You Canot Edit");
            }
            else
            {
                bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_MODIFY");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                    return;
                }
                isNew = false;
                OldGrid = " ";
                OldGrid = OldGrid + "Voucher No" + "-" + txtVoucherNo.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Cash/Party" + "-" + cboCashPartyAcc.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
                //Collect the Contents of the grid for audit log
                for (int i = 0; i < grdPurchaseInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                {
                    string productname = grdPurchaseInvoice[i + 1, (int)GridColumn.ProductName].Value.ToString() + ",";
                    string qty = grdPurchaseInvoice[i + 1, (int)GridColumn.Qty].Value.ToString() + ",";
                    string rate = grdPurchaseInvoice[i + 1, (int)GridColumn.PurchaseRate].Value.ToString() + ",";
                    string amt = grdPurchaseInvoice[i + 1, (int)GridColumn.Amount].Value.ToString() + ",";
                    OldGrid = OldGrid + string.Concat(productname, qty, rate, amt) + ",";
                }
                OldGrid = "OldGridValues" + OldGrid;
                if (!ContinueWithoutSaving())
                {
                    return;
                }
                EnableControls(true);
                ChangeState(EntryMode.EDIT);
                txtOrderNo.Enabled = false;

                //if automatic voucher number increment is selected
                string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                if (NumberingType == "AUTOMATIC")
                    txtVoucherNo.Enabled = false;

            }
        }

        private int ChildCount(int AccClassID)
        {
            try
            {
                int m_RecCount = (int)User.GetAccessInfo(AccClassID).Rows.Count;
                return m_RecCount;
            }
            catch (Exception)
            {
                throw;
                //return 0;
            }
        }

        //Recursive Function to Show Access Level in Treeview
        private void ShowAccClassInTreeView(TreeView tv, TreeNode n, int AccClassID)
        {
            tv.Nodes.Clear();
            TreeNode temp = new TreeNode();
            temp.Text = "ROOT";
            temp.Tag = 1;
            tv.Nodes.Add(temp);

            //#region Language Management

            //tv.Font = LangMgr.GetFont();

            //string LangField = "EngName";
            //switch (LangMgr.DefaultLanguage)
            //{
            //    case Lang.English:
            //        LangField = "EngName";
            //        break;
            //    case Lang.Nepali:
            //        LangField = "NepName";
            //        break;

            //}
            //#endregion

            //if (Global.GlobalAccClassID == 1 && Global.GlobalAccessRoleID == 37)
            //{
            //    DataTable dt = new DataTable();
            //    try
            //    {
            //        dt = AccountClass.GetAccClassTable(AccClassID);
            //    }
            //    catch (Exception ex)
            //    {
            //        Global.Msg(ex.Message);
            //    }
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        DataRow dr = dt.Rows[i];

            //        TreeNode t = new TreeNode(dr[LangField].ToString());
            //        t.Tag = dr["AccClassID"].ToString();
            //        //Check if it is a parent Or if it has childs
            //        try
            //        {
            //            if (ChildCount((int)dr["AccClassID"]) > 0)
            //            {
            //                //t.IsContainer = true;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }

            //        ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //        if (n == null)
            //        {
            //            t.Checked = true;
            //            tv.Nodes.Add(t); //Primary Group
            //        }
            //        else
            //        {
            //            t.Checked = true;
            //            n.Nodes.Add(t); //Secondary Group
            //        }
            //    }
            //}
            //else
            //{

            //    DataTable dtUserInfo = User.GetUserInfo(User.CurrUserID); //user id must be read from  global i.e current user id
            //    DataRow drUserInfo = dtUserInfo.Rows[0];
            //    ArrayList AccClassChildIDs = new ArrayList();
            //    ArrayList tempParentAccClassChildIDs = new ArrayList();
            //    AccClassChildIDs.Clear();
            //    AccClassChildIDs.Add(Convert.ToInt32(drUserInfo["AccClassID"]));
            //    AccountClass.GetChildIDs(Convert.ToInt32(drUserInfo["AccClassID"]), ref AccClassChildIDs);
            //    DataTable dt = new DataTable();
            //    try
            //    {
            //        dt = AccountClass.GetAccClassTable(AccClassID);
            //    }
            //    catch (Exception ex)
            //    {
            //        Global.Msg(ex.Message);
            //    }
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        DataRow dr = dt.Rows[i];
            //        TreeNode t = new TreeNode(dr[LangField].ToString());
            //        t.Tag = dr["AccClassID"].ToString();
            //        tempParentAccClassChildIDs.Clear();
            //        AccountClass.GetChildIDs(Convert.ToInt32(t.Tag), ref tempParentAccClassChildIDs);
            //        //Check if it is a parent Or if it has childs
            //        try
            //        {
            //            if (ChildCount((int)dr["AccClassID"]) > 0)
            //            {
            //                //t.IsContainer = true;
            //            }

            //            foreach (int itemIDs in AccClassChildIDs)  //To check if 
            //            {
            //                if (Convert.ToInt32(t.Tag) == itemIDs)
            //                {
            //                    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //                    loopCounter--;
            //                    t.Checked = true;
            //                    if (n == null)
            //                    {
            //                        tv.Nodes.Add(t); //Primary Group
            //                        return;
            //                    }
            //                    else if (Convert.ToInt32(t.Tag) == Convert.ToInt32(drUserInfo["AccClassID"]))
            //                    {
            //                        t.Checked = true;
            //                        tv.Nodes.Add(t);
            //                        return;
            //                    }
            //                    else
            //                    {
            //                        n.Nodes.Add(t); //Secondary Group
            //                    }
            //                }
            //                if (tempParentAccClassChildIDs.Contains(itemIDs) && loopCounter == 0)
            //                {
            //                    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }
            //    }
            //}

            //Insert the tag on the selected node to carry AccClassID
        }

        private void ClearVoucher()
        {
            m_isRecurring = false;
            m_RVID = 0;
            ClearPurchaseInvoice(false);
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdPurchaseInvoice.Redim(2, 19);
            AddGridHeader(); //Write header part
            //AddRowServices(1);
            //AddRowProduct(1);
            AddRowProduct1(1);
            ClearRecurringSetting();
            dtReference.Rows.Clear();
            AddReferenceColumns();
            m_refAmt = "0(Dr)";
            m_PurchaseInvoiceID = 0;
            txtPurchaseInvoiceID.Text = "0";
        }

        private void ClearPurchaseInvoice(bool IsVouNumFinished)
        {
            txtVoucherNo.Clear();
            //actually generate a new voucher no.
            //  txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtOrderNo.Clear();
            txtRemarks.Clear();
            cboDepot.SelectedIndex = 0; //cboDepot.Text = string.Empty; //While clearing the form, default value will be selected. Not empty string
            cboPurchaseAcc.Text = string.Empty;
            cboSeriesName.SelectedIndex = 0; //cboSeriesName.Text = string.Empty; //While clearing the form, default value will be selected.
            cboCashPartyAcc.SelectedIndex = 0; //cmboCashPartyAcc.Text = string.Empty; //While clearing the form, default value will be selected. Not empty string
            lblTax1.Text = "0.00";
            lblTax1.Text = "0.00";
            lblTax1.Text = "0.00";
            lblGross.Text = "0.00";
            lblSpecialDiscount.Text = "0.00";
            lblTotalQty.Text = "0.00";
            lblGrandTotal.Text = "0.00";
            lblVat.Text = "0.00";
            lblGrandTotalAfterCustomduty.Text = "0.00";
            lblNetAmout.Text = "0.00";
            lblFreight.Text = "0.00";
            lblCustomduty.Text = "0.00";
            txtPartyBillNumber.Clear();
            if (!IsVouNumFinished)
            {
                grdPurchaseInvoice.Rows.Clear();
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

        private bool Navigation(Navigate NavTo)
        {
            try
            {
                if (txtPurchaseInvoiceID.Text != "")
                    dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(txtPurchaseInvoiceID.Text), "PURCH");

                ChangeState(EntryMode.NORMAL);
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtPurchaseInvoiceID.Text);
                    if (PurchaseInvoiceIDCopy > 0)
                    {
                        VouchID = PurchaseInvoiceIDCopy;
                        PurchaseInvoiceIDCopy = 0;

                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtPurchaseInvoiceID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                DataTable dtPurchaseInvoiceMaster = m_PurchaseInvoice.NavigatePurchaseInvoiceMaster(VouchID, NavTo);
                if (dtPurchaseInvoiceMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }

                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                DataRow drPurchaseInvoiceMaster = dtPurchaseInvoiceMaster.Rows[0]; //There is only one row. First row is the required record

                //Show the corresponding Cash/Party Account Ledger in Combobox
                DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseInvoiceMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                foreach (DataRow drCashPartyLedgerInfo in dtCashPartyInfo.Rows)
                {
                    cboCashPartyAcc.Text = drCashPartyLedgerInfo["LedName"].ToString();
                }

                //Show the corresponding Purchase Account Ledger in Combobox
                DataTable dtPurchaseLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseInvoiceMaster["PurchaseLedgerID"]), LangMgr.DefaultLanguage);
                foreach (DataRow drPurchaseLedgerInfo in dtPurchaseLedgerInfo.Rows)
                {
                    cboPurchaseAcc.Text = drPurchaseLedgerInfo["LedName"].ToString();
                }

                //Show the corresponding Depot in control

                DataTable dtDepotInfo = Depot.GetDepotInfo(Convert.ToInt32(drPurchaseInvoiceMaster["DepotID"]));
                if (dtDepotInfo.Rows.Count > 0)
                {
                    DataRow drDepotInfo = dtDepotInfo.Rows[0];
                    cboDepot.Text = drDepotInfo["DepotName"].ToString();
                }

                //show the corresoponding SeriesName in Combobox
                DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drPurchaseInvoiceMaster["SeriesID"]));
                if (dtSeriesInfo.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Purchase Invoice");
                    cboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dtSeriesInfo.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();
                }
                txtPartyBillNumber.Text = drPurchaseInvoiceMaster["PartyBillNumber"].ToString();
                lblVouNo.Visible = true;
                txtVoucherNo.Visible = true;
                txtVoucherNo.Text = drPurchaseInvoiceMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drPurchaseInvoiceMaster["PurchaseInvoice_Date"].ToString());
                txtRemarks.Text = drPurchaseInvoiceMaster["Remarks"].ToString();
                txtPurchaseInvoiceID.Text = drPurchaseInvoiceMaster["PurchaseInvoiceID"].ToString();
                txtOrderNo.Text = drPurchaseInvoiceMaster["OrderNo"].ToString();
                lblNetAmout.Text = Convert.ToDecimal(drPurchaseInvoiceMaster["Net_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)); 
                lblTax1.Text = Convert.ToDecimal(drPurchaseInvoiceMaster["Tax1"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblTax2.Text = Convert.ToDecimal(drPurchaseInvoiceMaster["Tax2"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblTax3.Text = Convert.ToDecimal(drPurchaseInvoiceMaster["Tax3"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblVat.Text = Convert.ToDecimal(drPurchaseInvoiceMaster["VAT"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblCustomduty.Text =Convert.ToDecimal( drPurchaseInvoiceMaster["CustomDuty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblFreight.Text = Convert.ToDecimal(drPurchaseInvoiceMaster["Freight"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblTotalQty.Text = drPurchaseInvoiceMaster["TotalQty"].ToString();
                lblSpecialDiscount.Text = drPurchaseInvoiceMaster["SpecialDiscount"].ToString();
                lblGross.Text = Convert.ToDecimal(drPurchaseInvoiceMaster["Gross_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblGrandTotal.Text = (Convert.ToDouble(lblNetAmout.Text) + Convert.ToDouble(lblTax1.Text) + Convert.ToDouble(lblTax2.Text) + Convert.ToDouble(lblTax3.Text) + Convert.ToDouble(lblVat.Text)).ToString();
                lblGrandTotalAfterCustomduty.Text = (Convert.ToDouble(lblGrandTotal.Text) + Convert.ToDouble(lblCustomduty.Text) + Convert.ToDouble(lblFreight.Text)).ToString();
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

                        txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();
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

                        txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();
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

                        txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();

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

                        txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();

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

                        txtfirst.Text = drPurchaseInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drPurchaseInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drPurchaseInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drPurchaseInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drPurchaseInvoiceMaster["Field5"].ToString();
                    }


                }

                dsPurchaseInvoice.Tables["tblPurchaseInvoiceMaster"].Rows.Add(cboSeriesName.Text, drPurchaseInvoiceMaster["Voucher_No"].ToString(), Date.DBToSystem(drPurchaseInvoiceMaster["PurchaseInvoice_Date"].ToString()), cboDepot.Text, drPurchaseInvoiceMaster["OrderNo"].ToString(), cboCashPartyAcc.Text, cboPurchaseAcc.Text, drPurchaseInvoiceMaster["Remarks"].ToString());
                DataTable dtPurchaseInvoiceDetails = m_PurchaseInvoice.GetPurchaseInvoiceDetails(Convert.ToInt32(txtPurchaseInvoiceID.Text));
                for (int i = 1; i <= dtPurchaseInvoiceDetails.Rows.Count; i++)
                {
                    DataRow drDetail = dtPurchaseInvoiceDetails.Rows[i - 1];
                    grdPurchaseInvoice[i, (int)GridColumn.SNo].Value = i.ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.Code_No].Value = drDetail["Code"].ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();

                    grdPurchaseInvoice[i, (int)GridColumn.Qty].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["CurUnitQty"])).ToString();
                    

                    grdPurchaseInvoice[i, (int)GridColumn.PurchaseRate].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.SplDisc_Percent].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.SplDisc].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.NetAmount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.ProductID].Value =productID= Convert.ToInt32(drDetail["ProductID"]);
                    grdPurchaseInvoice[i, (int)GridColumn.VAT].Value = Convert.ToDecimal(drDetail["VAT"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdPurchaseInvoice[i, (int)GridColumn.CustomDuty_Percent].Value = Convert.ToDecimal(drDetail["CustomDutyPercentage"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdPurchaseInvoice[i, (int)GridColumn.CustomDuty].Value = Convert.ToDecimal(drDetail["CustomDuty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdPurchaseInvoice[i, (int)GridColumn.Freight].Value = Convert.ToDecimal(drDetail["Freight"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                    grdPurchaseInvoice[i, (int)GridColumn.Default_Qty].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Quantity"])).ToString();
                    grdPurchaseInvoice[i, (int)GridColumn.Default_Unit].Value = Convert.ToInt32(drDetail["UnitID"].ToString());
                    LoadCompoundUnit(i);

                    SourceGrid.CellContext context = new SourceGrid.CellContext(grdPurchaseInvoice, new SourceGrid.Position(i, (int)GridColumn.Units));
                    int index = Array.IndexOf(arrInt, Convert.ToInt32(drDetail["QtyUnitID"]));
                    context.Cell.Editor.SetCellValue(context, arrStr[index]);

                    // AddRowProduct(grdPurchaseInvoice.RowsCount);
                    AddRowProduct1(grdPurchaseInvoice.RowsCount);
                    dsPurchaseInvoice.Tables["tblPurchaseInvoiceDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(), drDetail["Quantity"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());
                }

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtPurchaseInvoiceID.Text), "PURCH");
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
                CheckRecurringSetting(txtPurchaseInvoiceID.Text);

                btnExport.Enabled = true;
                ChangeState(EntryMode.NORMAL);

                return true;

            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
                return false;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            IsFieldChanged = false;
            Navigation(Navigate.First);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            IsFieldChanged = false;
            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            //IsFieldChanged = false;
            Navigation(Navigate.Next);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            //IsFieldChanged = false;
            Navigation(Navigate.Last);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            EnableControls(true);
            ChangeState(EntryMode.NEW);
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            //frmAccountClass frm = new frmAccountClass(this);
            //frm.Show();
            m_MDIForm.OpenFormArrayParam("frmAccClass");
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            PurchaseInvoiceIDCopy = Convert.ToInt32(txtPurchaseInvoiceID.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (PurchaseInvoiceIDCopy > 0)
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

        private void btnNew_Click_1(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
            SetVoucherNo();
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            IsFieldChanged = false;
        }

        private void Print_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }

            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            string gridvaluebeforedelete = "";
            gridvaluebeforedelete = gridvaluebeforedelete + "Voucher No" + "-" + txtVoucherNo.Text + "," + "Voucher Date" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Cash/Party" + "-" + cboCashPartyAcc.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdPurchaseInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string productname = grdPurchaseInvoice[i + 1, 3].Value.ToString() + ",";
                string qty = grdPurchaseInvoice[i + 1, 4].Value.ToString() + ",";
                string rate = grdPurchaseInvoice[i + 1, 5].Value.ToString() + ",";
                string amt = grdPurchaseInvoice[i + 1, 6].Value.ToString() + ",";
                gridvaluebeforedelete = gridvaluebeforedelete + string.Concat(productname, qty, rate, amt) + ",";
            }
            gridvaluebeforedelete = "GridValues" + gridvaluebeforedelete;


            ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            try
            {
                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the Invoice - " + txtPurchaseInvoiceID.Text + "?") == DialogResult.Yes)
                {
                    Purchase DelPurchase = new Purchase();
                    // delete reference
                    string res = VoucherReference.DeleteReference(Convert.ToInt32(txtPurchaseInvoiceID.Text), "PURCHASE");
                    if (res != "Success")
                    {
                        Global.MsgError("Unable to delete the voucher due to " + res + "! \n You must delete all other vouchers with reference against this voucher to delete this transaction!");
                        return;
                    }

                    if (DelPurchase.RemovePurchaseEntry(Convert.ToInt32(txtPurchaseInvoiceID.Text), gridvaluebeforedelete))
                    {
                        Global.Msg("Invoice -" + txtPurchaseInvoiceID.Text + " deleted successfully!");
                        RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "PURCHASE_INVOICE"); // deleting the recurring setting if voucher is deleted
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
                        Global.MsgError("There was an error while deleting invoice -" + txtPurchaseInvoiceID.Text + "!");
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            ////Do not delete if its the last Row because it contains (NEW)
            //if (ctx.Position.Row <= grdPurchaseInvoice.RowsCount - 2)
            //    grdPurchaseInvoice.Rows.Remove(ctx.Position.Row);


        }

        private void btnPrint_KeyDown(object sender, KeyEventArgs e)
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
                btnDelete_Click(sender, e);
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
                Print_Click(sender, e);
            }
        }

        private void txtDate_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtOrderNo_TextChanged(object sender, EventArgs e)
        {

        }
        private void AddRowProduct1(int RowCount)
        {
            //Add a new row
            try
            {
                grdPurchaseInvoice.Redim(Convert.ToInt32(RowCount + 1), grdPurchaseInvoice.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdPurchaseInvoice[i, (int)GridColumn.Del] = btnDelete;
                grdPurchaseInvoice[i, (int)GridColumn.Del].AddController(evtDelete);
                grdPurchaseInvoice[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdPurchaseInvoice[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();

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
                grdPurchaseInvoice[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell("", txtProduct);
                txtProduct.Control.GotFocus += new EventHandler(Product_Focused);
                txtProduct.Control.LostFocus += new EventHandler(Product_Leave);
                txtProduct.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                txtProduct.Control.TextChanged += new EventHandler(Text_Change);
                grdPurchaseInvoice[i, (int)GridColumn.ProductName].AddController(evtProductFocusLost);
                grdPurchaseInvoice[i, (int)GridColumn.ProductName].Value = "(NEW)";



                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdPurchaseInvoice[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("1", txtQty);
                txtQty.Control.TextChanged += new EventHandler(Text_Change);
                //grdPurchaseInvoice[i, 4].Value = "(NEW)";

                #region related to compound unit
                
                LoadCompoundUnit(i);

                grdPurchaseInvoice[i, (int)GridColumn.Default_Qty] = new SourceGrid.Cells.Cell("");
                grdPurchaseInvoice[i, (int)GridColumn.Default_Qty].Value = "0";

                grdPurchaseInvoice[i, (int)GridColumn.Default_Unit] = new SourceGrid.Cells.Cell("");
                grdPurchaseInvoice[i, (int)GridColumn.Default_Unit].Value = "0";

                #endregion
                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseInvoice[i, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                txtPurchaseRate.Control.TextChanged += new EventHandler(Text_Change);


                // grdPurchaseInvoice[i, 5].Value = "(NEW)";

                grdPurchaseInvoice[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell();
                // grdPurchaseInvoice[i, 6].Value = "(NEW)";
                SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);
                //txtDiscPercentage.Control.TextChanged += new EventHandler(Text_Change);
                txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                txtDiscPercentage.Control.Text = "0";
                //bool test = IsFieldChanged;
                //MessageBox.Show(test.ToString());


                grdPurchaseInvoice[i, (int)GridColumn.SplDisc_Percent] = new SourceGrid.Cells.Cell("", txtDiscPercentage);
                grdPurchaseInvoice[i, (int)GridColumn.SplDisc_Percent].Value = "0";
                SourceGrid.Cells.Editors.TextBox txtDiscount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscount.Control.LostFocus += new EventHandler(Discount_Modified);
                txtDiscount.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseInvoice[i, (int)GridColumn.SplDisc] = new SourceGrid.Cells.Cell("0", txtDiscount);
                grdPurchaseInvoice[i, (int)GridColumn.SplDisc].Value = "0";

                grdPurchaseInvoice[i, (int)GridColumn.NetAmount] = new SourceGrid.Cells.Cell("Net Amount");
                grdPurchaseInvoice[i, (int)GridColumn.NetAmount].Value = "0";
                grdPurchaseInvoice[i, (int)GridColumn.NetAmount].AddController(evtAmountFocusLost);

                grdPurchaseInvoice[i, (int)GridColumn.PurchaseOrderDetailID] = new SourceGrid.Cells.Cell("");
                grdPurchaseInvoice[i, (int)GridColumn.PurchaseOrderDetailID].Value = "0";

                SourceGrid.Cells.Editors.TextBox VATAmt = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                //VATAmt.EditableMode = SourceGrid.EditableMode.None;
                grdPurchaseInvoice[i, (int)GridColumn.VAT] = new SourceGrid.Cells.Cell("0", VATAmt);
                VATAmt.EditableMode = SourceGrid.EditableMode.Focus;
                VATAmt.Control.TextChanged += new EventHandler(Text_Change);
                grdPurchaseInvoice[i, (int)GridColumn.VAT].AddController(evtVAT);
                txtPurchaseRate.Control.TextChanged += new EventHandler(Text_Change);

                SourceGrid.Cells.Editors.TextBox txtCustomDuty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtCustomDuty.EditableMode = SourceGrid.EditableMode.Focus;
                txtCustomDuty.Control.LostFocus += new EventHandler(CustomDutyPer_Modified);
                grdPurchaseInvoice[i, (int)GridColumn.CustomDuty_Percent] = new SourceGrid.Cells.Cell("0", txtCustomDuty);
                txtPurchaseRate.Control.TextChanged += new EventHandler(Text_Change);
                if (iscustomduty == "1")
                    grdPurchaseInvoice[i, (int)GridColumn.CustomDuty_Percent].Value = Global.Default_CustomDuty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                else
                    grdPurchaseInvoice[i, (int)GridColumn.CustomDuty_Percent].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                SourceGrid.Cells.Editors.TextBox txtCustomDutyValue = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtCustomDutyValue.EditableMode = SourceGrid.EditableMode.Focus;
                txtCustomDutyValue.Control.LostFocus += new EventHandler(CustomDutyAmount_Modified);
                grdPurchaseInvoice[i, (int)GridColumn.CustomDuty] = new SourceGrid.Cells.Cell("0", txtCustomDutyValue);
                txtPurchaseRate.Control.TextChanged += new EventHandler(Text_Change);


                SourceGrid.Cells.Editors.TextBox txtfreight = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtfreight.EditableMode = SourceGrid.EditableMode.Focus;
                txtfreight.Control.LostFocus += new EventHandler(Freight_Modified);
                grdPurchaseInvoice[i, (int)GridColumn.Freight] = new SourceGrid.Cells.Cell("0", txtfreight);
                grdPurchaseInvoice[i, (int)GridColumn.Freight].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                txtPurchaseRate.Control.TextChanged += new EventHandler(Text_Change);

                grdPurchaseInvoice[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell("");
                grdPurchaseInvoice[i, (int)GridColumn.ProductID].Value = "0";


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
            int RowsCount = grdPurchaseInvoice.RowsCount;
            string LastServicesCell = (string)grdPurchaseInvoice[RowsCount - 1, 3].Value;
            // Qty_Modified(sender,e);
            ////Check whether the new row is already added
            if (LastServicesCell != "(NEW)")
            {
                // AddRowProduct(RowsCount);
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
            ////If the row is not modified or in the (NEW) mode, just skip
            //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //CurrRowPos = ctx.Position.Row;
            //FillAllGridRow(CurrRowPos);
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            int Productid = 0;
            string ProductName = "";
            string productcode = "";
            double purchaserate = 0;
            double salesrate = 0;
            try
            {
                if (grdPurchaseInvoice[CurrRowPos, (int)GridColumn.ProductName].Value.ToString() == "(NEW)" || grdPurchaseInvoice[CurrRowPos, (int)GridColumn.ProductName].Value.ToString() == "")
                    return;
                try
                {
                    Productid = Convert.ToInt32(grdPurchaseInvoice[CurrRowPos, (int)GridColumn.ProductID].Value);
                    ProductName = grdPurchaseInvoice[CurrRowPos, (int)GridColumn.Code_No].Value.ToString();
                    productcode = grdPurchaseInvoice[CurrRowPos, (int)GridColumn.SNo].Value.ToString();
                    purchaserate = Convert.ToDouble(grdPurchaseInvoice[CurrRowPos, (int)GridColumn.Qty].Value);
                    salesrate = Convert.ToDouble(grdPurchaseInvoice[CurrRowPos, (int)GridColumn.PurchaseRate].Value);
                    productID = Productid;

                    LoadCompoundUnit(CurrRowPos);

                }
                catch
                {
                    Productid = 0;
                }
                if (Productid != 0 && CurrAccLProductid == 0)
                {
                    CurrAccLProductid = Productid;
                    ProdName = ProductName;
                    prodcode = productcode;
                    prate = purchaserate;
                    salerate = salesrate;
                }

            }
            catch
            {
                return;
            }

            //FillAllGridRow(CurrRowPos, CurrAccLProductid,prodcode, ProdName, prate, salerate);
                                LoadCompoundUnit(CurrRowPos);

        }

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            //int RowCount = grdPurchaseInvoice.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            //string AccName = (string)(grdPurchaseInvoice[RowCount - 1, (int)GridColumn.ProductName].Value);
            ////Check if the input value is correct
            //if (grdPurchaseInvoice[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == "" || grdPurchaseInvoice[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == null)
            //{
            //    grdPurchaseInvoice[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            //}

            //double checkformat = Convert.ToDouble(grdPurchaseInvoice[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value.ToString());
            //string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //grdPurchaseInvoice[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = insertvalue;

            //if (AccName != "(NEW)")
            //{
            //    AddRowProduct(RowCount);
            //}

            CalculateGrossAmount();
            WriteNetAmount(CurRow);
            CalculateNetAmount();
            CalculateTotalQuantity();
            CalculateTotalCustomDuty();
        }
        public void AddProduct(int productid, string productcode, string productname, bool IsSelected, double purchaserate, double salesrate, int qty, int defaultUnitID)
        {
            //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //CurrRowPos = ctx.Position.Row;
            if (IsSelected)
            {
                int CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
                // CurrRowPos = ctx.Position.Row;
                ctx.Text = productname;
                //purchaserate = purchaserate;
                //productcode = productcode;

                grdPurchaseInvoice[CurRow, (int)GridColumn.Code_No].Value = productcode;
                grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value = 1;
                grdPurchaseInvoice[CurRow, (int)GridColumn.PurchaseRate].Value = purchaserate;
                grdPurchaseInvoice[CurRow, (int)GridColumn.ProductID].Value = productid.ToString();

                grdPurchaseInvoice[CurRow, (int)GridColumn.Default_Unit].Value = defaultUnitID;

                int RowsCount = grdPurchaseInvoice.RowsCount;
                string LastServicesCell = (string)grdPurchaseInvoice[RowsCount - 1, (int)GridColumn.ProductName].Value;

                ////Check whether the new row is already added
                if (LastServicesCell == "(NEW)")
                {
                    // AddRowProduct(RowsCount);
                    AddRowProduct1(RowsCount);
                    //Clear (NEW) on other colums as well
                    ClearNew(RowsCount - 1);
                }

            }
            hasChanged = true;
        }
        private void FillAllGridRow(int RowPosition)
        {
            // grdPurchaseInvoice[RowPosition, 2].Value = productcode;
            //grdPurchaseInvoice[RowPosition, 5].Value = purchaserate;
            //grdJournal[RowPosition, 7].Value = LdrID;
            //grdJournal[RowPosition, 8].Value = CurrBalance;
            //productcode = "";
            // purchaserate = 0;
        }

        private void grdPurchaseInvoice_Click(object sender, EventArgs e)
        {
            //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //CurrRowPos = ctx.Position.Row;
        }

        private void frmPurchaseInvoice_FormClosed(object sender, FormClosedEventArgs e)
        {
            Global.product = false;
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            Common.frmDateConverter frm = new Common.frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
            // m_MDIForm.OpenForm("frmDateConverter");
        }
        public void DateConvert(DateTime DotNetDate)
        {
            txtDate.Text = Date.ToSystem(DotNetDate);
        }

        private void grdPurchaseInvoice_Validating(object sender, CancelEventArgs e)
        {

        }

        private void btn_tenderamount_Click(object sender, EventArgs e)
        {
            double totalamount;
            double vatamount = 0;
            double tax1amount = 0;
            double tax2amount = 0;
            double tax3amount = 0;
            if (lblVat.Text != "0.00")
            {
                vatamount = Convert.ToDouble(lblVat.Text);
            }
            if (lblTax1.Text != "0.00")
            {
                tax1amount = Convert.ToDouble(lblTax1.Text);
            }
            if (lblTax2.Text != "0.00")
            {
                tax2amount = Convert.ToDouble(lblTax2.Text);
            }
            if (lblTax3.Text != "0.00")
            {
                tax3amount = Convert.ToDouble(lblTax3.Text);
            }
            totalamount = (Convert.ToDouble(lblNetAmout.Text)) + vatamount + tax1amount + tax2amount + tax3amount;
            // frmtenderamount ft = new frmtenderamount(totalamount);
            //  ft.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ContinueWithoutSaving()
        {
            if (IsFieldChanged)
            {
                if (MessageBox.Show("Changes has not been saved yet. Are you sure you want to continue without saving?", "Exiting...", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return true;
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
                    Print_Click(sender, e);
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
                    Print_Click(sender, e);
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
                    Print_Click(sender, e);
                    break;
            }
        }

        private void btncashparty_Click(object sender, EventArgs e)
        {
            //frmAccountSetup frm = new frmAccountSetup(this);
            //frm.Show();
            m_MDIForm.OpenFormArrayParam("frmAccSetUp");

            LoadComboboxParty();
        }
        public void AddAccountLedger(string ledgerName)
        {
            try
            {
                cboCashPartyAcc.Items.Clear();
                #region BLOCK FOR SHOWING THE LEDGERS OF CASH IN HAND,DEBTOR AND CREDITOR IN CASH/PARTY COMBOBOX
                //cmboCashPartyAcc.Items.Add(new ListItem(0, "Choose Cash/Party Ledger"));//At the first index of combobox show the "Choose Cash/Party Ledger"
                int Cash_In_Hand = AccountGroup.GetGroupIDFromGroupNumber(102);
                DataTable dtCash_In_HandLedgers = Ledger.GetAllLedger(Cash_In_Hand);//Collecting the Ledgers corresponding to Cash_In_Hand group
                foreach (DataRow drCash_In_HandLedgers in dtCash_In_HandLedgers.Rows)
                {
                    cboCashPartyAcc.Items.Add(new ListItem((int)drCash_In_HandLedgers["LedgerID"], drCash_In_HandLedgers["EngName"].ToString()));
                }
                int Debtor = AccountGroup.GetGroupIDFromGroupNumber(29);
                DataTable dtDebtorLedgers = Ledger.GetAllLedger(Debtor);
                foreach (DataRow drDebtorLedgers in dtDebtorLedgers.Rows)
                {
                    cboCashPartyAcc.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drDebtorLedgers["EngName"].ToString()));
                }
                int Creditor = AccountGroup.GetGroupIDFromGroupNumber(114);
                DataTable dtCreditorLedgers = Ledger.GetAllLedger(Creditor);
                foreach (DataRow drCreditorLedgers in dtCreditorLedgers.Rows)
                {
                    cboCashPartyAcc.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drCreditorLedgers["EngName"].ToString()));
                }

                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                foreach (DataRow drBankLedger in dtBankLedgers.Rows)
                {
                    cboCashPartyAcc.Items.Add(new ListItem((int)drBankLedger["LedgerID"], drBankLedger["EngName"].ToString()));
                }
                cboCashPartyAcc.DisplayMember = "value";//This value is  for showing at Load condition
                cboCashPartyAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition
                cboCashPartyAcc.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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


            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;
            dsPurchaseInvoice.Clear();
            rptPurchaseInvoice rpt = new rptPurchaseInvoice();
            Misc.WriteLogo(dsPurchaseInvoice, "tblImage");
            rpt.SetDataSource(dsPurchaseInvoice);

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

                // pdvCompany_Address.Value = m_CompanyDetails.Address1;
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
                frmProgress ProgressForm = new frmProgress();
                // Initialize the thread that will handle the background process
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        ProgressForm.ShowDialog();
                    }
                ));
                backgroundThread.Start();
                //Update the progressbar
                ProgressForm.UpdateProgress(20, "Initializing Report Viewer...");
                Common.frmReportViewer frm = new Common.frmReportViewer();
                frm.SetReportSource(rpt);

                //CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                //DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //CrDiskFileDestinationOptions.DiskFileName = FileName;
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
                        frmemail sendemail = new frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        m_MDIForm.OpenFormArrayParam("frmReportViewer");
                        rpt.Close();
                        return;
                    default:
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }
                frm.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void cboPurchaseAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBudgetLimit();
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
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "PURCHASE_INVOICE", m_dtRecurringSetting);
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
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "PURCHASE_INVOICE"); // delete from database
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
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "PURCHASE_INVOICE", m_dtRecurringSetting);
            fvr.ShowDialog();
        }

        string RSID = null, recurringVoucherID = null;
        public void CheckRecurringSetting(string VoucherID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            m_dtRecurringSetting = RecurringVoucher.GetRecurringVoucherSetting(VoucherID, "PURCHASE_INVOICE");
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
                // check if the user has permission to view the voucher
                if (!UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW"))
                {
                    Global.MsgError("Sorry! you dont have permission to View Purchase Invoice. Please contact your administrator for permission.");
                    return;
                }
                string[] vouchValues = new string[5];
                vouchValues[0] = "PURCHASE_INVOICE";               // voucherType
                vouchValues[1] = "Inv.tblPurchaseInvoiceMaster";   // master tableName for the given voucher type  
                vouchValues[2] = "Inv.tblPurchaseInvoiceDetails";  // details tableName for the given voucher type
                vouchValues[3] = "PurchaseInvoiceID";              // master ID for the given master table
                vouchValues[4] = "PurchaseInvoice_Date";              // date field for a given voucher

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
            m_PurchaseInvoiceID = VoucherID;
            txtPurchaseInvoiceID.Text = VoucherID.ToString();
            Navigation(Navigate.ID);
            //frmPurchaseInvoice_Load(null, null);
        } 
        #endregion

        #region method related to bank reconciliation
		
        public bool CheckIfBankReconciliationClosed()
        {
            try
            {
                bool res = false;
                //ListItem bankId = (ListItem)cmboCashPartyAcc.SelectedItem;
                if (BankReconciliation.IsBankReconciliationClosed(Convert.ToInt32(cboCashPartyAcc.SelectedValue), Date.ToDotNet(txtDate.Text)))
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
                dr1["VoucherType"] = "PURCH";
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
                dr1["VoucherType"] = "PURCH";
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
                decimal Amt = Convert.ToDecimal(lblGrandTotalAfterCustomduty.Text);
                string crdr = "Debit";

                string amtCrDr = m_refAmt;
                decimal refAmt = Convert.ToDecimal(amtCrDr.Substring(0, amtCrDr.Length - 4));
                string ledgerName = cboCashPartyAcc.Text; //grdContra[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString();
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
                if (cboCashPartyAcc.SelectedIndex > 0)
                {
                    //ListItem LiCashPartyLedgerID = new ListItem();
                    //LiCashPartyLedgerID = (ListItem)cboCashPartyAcc.SelectedItem;
                    //CurrAccLedgerID = LiCashPartyLedgerID.ID;
                    CurrAccLedgerID = Convert.ToInt32(cboCashPartyAcc.SelectedValue);
                    if (m_mode == EntryMode.EDIT)
                        isNewReferenceVoucher = VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(txtPurchaseInvoiceID.Text), CurrAccLedgerID, "PURCH");
                    else
                        isNewReferenceVoucher = false;
                    if (VoucherReference.CheckIfReferece(CurrAccLedgerID) && !isNewReferenceVoucher) // if isBillReference is true for given ledger then load the reference form
                    {
                        Form fc = Application.OpenForms["frmReference"];

                        if (fc != null)
                            fc.Close();

                        if (txtPurchaseInvoiceID.Text != "")
                        {
                            vouchID = Convert.ToInt32(txtPurchaseInvoiceID.Text);
                        }

                        frmReference fr = new frmReference(this, vouchID, "PURCH", CurrAccLedgerID);
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

        private void lblgrandtotalaftercustomduty_TextChanged(object sender, EventArgs e)
        {
            CheckBudgetLimit();           
        }
        private void CheckBudgetLimit()
        {
            //get selected accounting class ids for budget check
            string strXML = " ";
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            tw.WriteStartDocument();
            try
            {
                ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
                tw.WriteStartElement("AccClassIDSettings");
                if (arrNode.Count > 0)
                {
                    foreach (string tag in arrNode)
                    {
                        AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccClassID", Convert.ToInt32(tag).ToString());
                    }
                }
                else
                {
                    tw.WriteElementString("AccClassID", "1");
                }
                tw.WriteEndElement();

                tw.WriteEndDocument();
                tw.Flush();
                tw.Close();
                strXML = AEncoder.GetString(ms.ToArray());
            }
            catch
            { }

            //check for budget limit
            bool check = true;

            ListItem liLedgerID = new ListItem();
            liLedgerID = (ListItem)cboPurchaseAcc.SelectedItem;
            check = Budget.CheckBudget(Convert.ToInt32(liLedgerID.ID), Convert.ToDecimal(lblGrandTotal.Text.Trim()), strXML);
            if (!check)
            {
                // grdPurchaseInvoice[Convert.ToInt32(grdPurchaseInvoice.Rows.Count), (int)GridColumn.Amount].Value = 0;
                //  lblgrandtotal.Text = (Convert.ToDecimal(lblgrandtotal.Text.Trim()) - Convert.ToDecimal(grdPurchaseInvoice[Convert.ToInt32(grdPurchaseInvoice.Rows.Count), (int)GridColumn.Amount].Value)).ToString();
                btnSave.Enabled = false;
            }
            else
                btnSave.Enabled = true;
        }


        #region methods related to compound unit
        int[] arrInt;
        string[] arrStr;
        int productID = 0;
        public void GetListForGrdUnit()
        {

            DataTable dtUnit = UnitMaintenance.GetAllRelatedUnit(productID);
            if (dtUnit.Rows.Count > 0)
            {
                arrInt = new int[dtUnit.Rows.Count];
                arrStr = new string[dtUnit.Rows.Count];

                for (int i = 0; i < dtUnit.Rows.Count; i++)
                {
                    DataRow dr = dtUnit.Rows[i];
                    arrInt[i] = Convert.ToInt32(dr["UnitID"]);
                    arrStr[i] = dr["UnitName"].ToString();

                }
            }
        }

        private void Unit_Changed(object sender, EventArgs e)
        {
            try
            {
                int CurRow = 0;
                try
                {
                    CurRow = grdPurchaseInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
                }
                catch
                {
                    CurRow = 0;
                    return;
                }
                if (CurRow > 0)
                {

                    decimal actualQty = UnitMaintenance.ConvertCompoundUnit(Convert.ToInt32(grdPurchaseInvoice[CurRow, (int)GridColumn.Default_Unit].Value), Convert.ToInt32(grdPurchaseInvoice[CurRow, (int)GridColumn.Units].Value), Convert.ToDecimal(grdPurchaseInvoice[CurRow, (int)GridColumn.Qty].Value));
                    grdPurchaseInvoice[CurRow, (int)GridColumn.Amount].Value = (actualQty * Convert.ToDecimal(grdPurchaseInvoice[CurRow, (int)GridColumn.PurchaseRate].Value)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdPurchaseInvoice[CurRow, (int)GridColumn.Default_Qty].Value = actualQty;

                    //decimal StockQty = Convert.ToDecimal(grdPurchaseInvoice[CurRow, (int)GridColumn.Stock_Qty_Actual].Value.ToString());
                    //grdPurchaseInvoice[CurRow, (int)GridColumn.Stock_Qty].Value = (StockQty - Convert.ToDecimal(grdPurchaseInvoice[CurRow, (int)GridColumn.Default_Qty].Value)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                    //Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
                    WriteNetAmount(CurRow);
                    CalculateGrossAmount();

                    //CalculateAdjustmentAmount();
                    CalculateNetAmount();
                    CalculateTotalQuantity();
                    CalculateTotalCustomDuty();

                }


            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void LoadCompoundUnit(int row)
        {
            try
            {
                GetListForGrdUnit();

                SourceGrid.Cells.Editors.ComboBox cboUnits = new SourceGrid.Cells.Editors.ComboBox(typeof(int), arrInt, true);
                cboUnits.Control.FormattingEnabled = true;
                cboUnits.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboUnits.Control.AutoCompleteMode = AutoCompleteMode.Append;
                ValueMapping comboMapping = new ValueMapping();
                comboMapping.DisplayStringList = arrStr;
                comboMapping.ValueList = arrInt;
                comboMapping.SpecialList = arrStr;
                comboMapping.SpecialType = typeof(string);
                comboMapping.BindValidator(cboUnits);
                cboUnits.Control.DropDownStyle = ComboBoxStyle.DropDownList;

                grdPurchaseInvoice[row, (int)GridColumn.Units] = new SourceGrid.Cells.Cell();
                grdPurchaseInvoice[row, (int)GridColumn.Units].Editor = cboUnits;

                SourceGrid.CellContext context = new SourceGrid.CellContext(grdPurchaseInvoice, new SourceGrid.Position(row, (int)GridColumn.Units));
                string selected = arrStr[0];
                try
                {
                    selected = Convert.ToString(grdPurchaseInvoice[row, (int)GridColumn.Default_Unit].Value); // if product is selected then load default unit of the product in combobox unit
                    int index = Array.IndexOf(arrInt, Convert.ToInt32(selected));
                    selected = arrStr[index];
                }
                catch
                {
                    selected = arrStr[0];
                }
                context.Cell.Editor.SetCellValue(context, selected);

                //grdPurchaseInvoice[row, (int)GridColumn.Units].View = new SourceGrid.Cells.Views.Cell(RowView);
                grdPurchaseInvoice[row, (int)GridColumn.Units].AddController(evtUnitChanged);

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        #endregion

       
    }
}
