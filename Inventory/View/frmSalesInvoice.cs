using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Collections;
using DateManager;
using System.Text.RegularExpressions;
using CrystalDecisions.Shared;
using System.Drawing.Printing;
using Common;
using Inventory.Reports;
using DevAge.ComponentModel.Validator;
using BusinessLogic.HOS;
using Hospital.View;


namespace Inventory
{
    public interface IfrmSalesInvoice
    {
        void SalesInvoice(int RowID);
    }
    public partial class frmSalesInvoice : Form, IfrmAddAccClass, ListProduct, IfrmDateConverter, IfrmAccountLedger, IVoucherRecurring, IVoucherList, IVoucherReference, IListOfPatient
    {
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private double purchaserate = 0;
        private double prate = 0;
        private string productcode = "";
        private int CurrRowPos = 0;
        private int CurrAccLProductid = 0;

        private double VAT = 0;
        private double TAX1 = 0;
        private double TAX2 = 0;
        private double TAX3 = 0;
        private string Prefix = "";
        private double tax1amt = 0;
        private double tax2amt = 0;
        private double tax3amt = 0;
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;
        private int QtyinStock;
        //private double GrossAmount = 0;
        //private double NetAmount = 0;
        public double GAmount = 0;
        public double AdjAmount = 0;
        private int loopCounter = 0;
        bool hasChanged = false;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private bool IsFieldChanged = false;
        private bool IsShortcutKey = false;

        ListItem liParty = new ListItem();

        private IMDIMainForm m_MDIForm;
        //Adjustment Grid Variables
        double AdjustmentAmount = 0;

        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }

        //Enumeration for grid column of SalesInvoice
        private enum SalesInvoiceGridColumn : int
        {
            Del = 0, SNo, Code_No, ProductName, Qty, Units, Default_Unit, Default_Qty, Stock_Qty, Stock_Qty_Actual, SalesRate, Amount, SplDisc_Percent, SplDisc, NetAmount, SalesOrderDetailID, ProductID
        };


        //Enumeration for grid column of adjustment
        private enum AdjustmentGridColumn : int
        {
            Del = 0, SNo, BillSundry, Narration, Charge, percent, AdjustmentAmount
        };

        //private enum GridColumn : int
        //{

        //};

        // from previous data
        int? OrderNo = null;
        ListItem liProjectID = new ListItem();
        private int SalesInvoiceIDCopy = 0;
        ListItem SeriesID = new ListItem();
        List<int> AccClassID = new List<int>();

        public ArrayList AccClassIDs = new ArrayList();
        private int UserAccClass;

        private Inventory.Model.dsSalesInvoice dsSalesInvoice = new Model.dsSalesInvoice();

        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked

        Sales m_SalesInvoice = new Sales();

        //For Due Date
        private bool iSDueDate = false;
        private bool isForDueDate = false;

        //For BarCode Reader
        private DataTable orderTab = new DataTable();
        DataRow dtrow;


        private int m_SalesInvoiceID;
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Button btnAdjustmentRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAdjustmentDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtQty = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtUnitChanged = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtRate = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProduct = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtBillSundry = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductCode = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtOrderNo = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscPercentage = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDiscount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtBillSundryFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();

        private SourceGrid.Cells.Views.Cell RowView;
        //For Export Menu
        ContextMenu Menu_Export;
        private int prntDirectForPOS = 0;
        private int prntDirect = 0;
        private string FileName = "";

        int tickCounter = 0; //Counts the tick for the timer used for product PIN reading from barcode

        //public frmSalesInvoice()
        //{
        //    InitializeComponent();
        //}
        bool m_isRecurring = false;
        int m_RVID = 0;
        public frmSalesInvoice(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;
        }
        /// <summary>
        /// constructor to open the form from voucher recurring reminder
        /// </summary>
        /// <param name="SalesInvoiceID"></param>
        /// <param name="isRecurring"></param>
        public frmSalesInvoice(int SalesInvoiceID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_SalesInvoiceID = SalesInvoiceID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }
        public frmSalesInvoice(int SalesInvoiceID)
        {
            InitializeComponent();
            this.m_SalesInvoiceID = SalesInvoiceID;
        }
        System.Drawing.Printing.PaperSize paperSize = null;
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
            DataTable dtSalesOrderMasterInfo = new DataTable();

            //Check control value is int or not???
            object m_OrderNo = (object)txtOrderNo.Text;
            bool IsOrderNoIsInt = Misc.IsInt(m_OrderNo, false);
            if ((IsOrderNoIsInt) && (txtOrderNo.Text != ""))//If orderno is int value
            {
                dtSalesOrderMasterInfo = SalesOrder.GetSalesOrderMasterInfoByOrderNo(Convert.ToInt32(txtOrderNo.Text));  //with help of OrderNo,Get the information from Inv.tblPurchaseOrderMaster....
            }
            else if ((!IsOrderNoIsInt) && (txtOrderNo.Text != ""))
            {
                Global.Msg("Please Enter the Integer Value");
                txtOrderNo.Text = "";
                txtOrderNo.Focus();
                return;
            }

            if ((dtSalesOrderMasterInfo.Rows.Count > 0) && (txtOrderNo.Text != ""))
            {
                DataRow drSalesOrderMasterInfo = dtSalesOrderMasterInfo.Rows[0];
                txtOrderNo.Text = drSalesOrderMasterInfo["OrderNo"].ToString();
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesOrderMasterInfo["CashPartyID"]), LangMgr.DefaultLanguage);
                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                cboCashParty.Text = drLedgerInfo["LedName"].ToString();

                //Get the Detail information with help of PurchaseOrderMasterID
                SalesOrder m_SalesOrder = new SalesOrder();
                DataTable dtSalesOrderDetailInfo = m_SalesOrder.GetSalesOrderDetails(Convert.ToInt32(drSalesOrderMasterInfo["SalesOrderID"]));
                double Amount = 0;
                for (int i = 1; i <= dtSalesOrderDetailInfo.Rows.Count; i++)
                {
                    DataRow drDetail = dtSalesOrderDetailInfo.Rows[i - 1];
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SNo].Value = i.ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Code_No].Value = drDetail["ProductCode"].ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductID].Value = drDetail["ProductID"].ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesRate].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["SalesRate"])).ToString();
                    int Quantity = 0;
                    if ((drDetail["UpdatedQty"].ToString()) != "")
                    {
                        Quantity = (Convert.ToInt32(drDetail["Quantity"]) - (Convert.ToInt32(drDetail["UpdatedQty"])));
                        grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty].Value = Quantity;
                        // Amount = (Quantity * Convert.ToDouble(drDetail["SalesRate"]));
                    }
                    else
                    {
                        grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty].Value = drDetail["Quantity"].ToString();
                        //Amount = (Convert.ToInt32(drDetail["Quantity"]) * Convert.ToDouble(drDetail["SalesRate"]));
                    }
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty].Value = drDetail["Quantity"].ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount].Value = drDetail["Amount"].ToString();
                    //grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount].Value = Amount.ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Value = drDetail["SalesOrderDetailsID"].ToString();
                    //AddRowProduct(grdSalesInvoice.RowsCount);
                    AddRowProduct1(grdSalesInvoice.RowsCount);

                }

            }
            else if ((dtSalesOrderMasterInfo.Rows.Count <= 0) && (txtOrderNo.Text != ""))
            {
                Global.Msg("Order Number doesnot Exist");
                txtOrderNo.Text = string.Empty;
                txtOrderNo.Focus();
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

        private void frmSalesInvoice_Load(object sender, EventArgs e)
        {

            AddReferenceColumns();
            if (txtSalesInvoiceID.Text != "")
                dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(txtSalesInvoiceID.Text), "SALES");

            btnSetup.Enabled = false;

            chkDoNotClose.Checked = true;
            //For Barcode
            initOrderTable();

            //Check For the Negative Stock
            string negativestock = Settings.GetSettings("DEFAULT_NEGATIVESTOCK");
            if (negativestock == "Allow")
            {
                Global.Default_NegativeStock = NegativeStock.Allow;
            }
            else if (negativestock == "Warn")
            {
                Global.Default_NegativeStock = NegativeStock.Warn;
            }
            else if (negativestock == "Deny")
            {
                Global.Default_NegativeStock = NegativeStock.Deny;
            }

            DataTable dtvat = new DataTable();
            dtvat = Slabs.GetSlabInfo(SlabType.VAT);
            DataRow drvat = dtvat.Rows[0];
            VAT = Convert.ToDouble(drvat[3].ToString());

            DataTable dttax1 = new DataTable();
            dttax1 = Slabs.GetSlabInfo(SlabType.TAX1);
            DataRow drtax1 = dttax1.Rows[0];
            TAX1 = Convert.ToDouble(drtax1[3].ToString());

            DataTable dttax2 = new DataTable();
            dttax2 = Slabs.GetSlabInfo(SlabType.TAX2);
            DataRow drtax2 = dttax2.Rows[0];
            TAX2 = Convert.ToDouble(drtax2[3].ToString());

            DataTable dttax3 = new DataTable();
            dttax3 = Slabs.GetSlabInfo(SlabType.TAX3);
            DataRow drtax3 = dttax3.Rows[0];
            TAX3 = Convert.ToDouble(drtax3[3].ToString());

            Global.product = true;
            // ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            ChangeState(EntryMode.NEW);
            txtSalesInvoiceID.Visible = false;
            ShowAccClassInTreeView(treeAccClass, null, 0);

            #region BLOCK FOR SHOWING SERIES NAME IN COMBOBOX

            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("SALES");
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

            //For Loading The Optional Fields
            //OptionalFields();

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
                //Event trigerred when delete button is clicked
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                evtAdjustmentDelete.Click += new EventHandler(AdjustmentDelete_Row_Click);
                //Event when account is selected
                evtProduct.FocusLeft += new EventHandler(ProductCode_Selected);

                //Event when BillSundry is selected
                evtBillSundry.FocusLeft += new EventHandler(BillSundry_Selected);

                //Event when ProductCode is selected
                evtProductCode.FocusLeft += new EventHandler(ProductCode_Selected);

                //Event when OrderNo is selected

                evtOrderNo.FocusLeft += new EventHandler(OrderNo_Selected);

                txtOrderNo.LostFocus += new EventHandler(OrderNo_Selected);

                //Event when Quntity is selected
                evtQty.FocusLeft += new EventHandler(Qty_Modified);

                evtUnitChanged.ValueChanged += new EventHandler(Unit_Changed);
                //Event when Rate is selected
                evtRate.FocusLeft += new EventHandler(SalesRate_Modified);

                //Event when DiscPercentage is selected
                evtDiscPercentage.FocusLeft += new EventHandler(DiscPercentage_Modified);

                //Event when Discount is selected
                evtDiscount.FocusLeft += new EventHandler(Qty_Modified);

                #region Sales Account according to User Setting

                //Displaying the all ledgers associated with Sales AccountGroup in DropDownList
                int Sales_ID = AccountGroup.GetGroupIDFromGroupNumber(112);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultSales Account according to user root or other users
                int DefaultSalesAcc = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_SALES_ACCOUNT") : UserPreference.GetValue("DEFAULT_SALES_ACCOUNT", uid));
                string DefaultSalesName = "";

                //Add Sales to comboSalesAccount
                DataTable dtSalesLedgers = Ledger.GetAllLedger(Sales_ID);
                foreach (DataRow drSalesLedgers in dtSalesLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cmboSalesAcc.Items.Add(new ListItem((int)drSalesLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drSalesLedgers["LedgerID"]) == DefaultSalesAcc)
                        DefaultSalesName = drLedgerInfo["LedName"].ToString();
                }
                cmboSalesAcc.DisplayMember = "value";//This value is  for showing at Load condition
                cmboSalesAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cmboSalesAcc.Text = DefaultSalesName;

                #endregion

                #region BLOCK FOR SHOWING THE LEDGERS OF CASH IN HAND,DEBTOR AND CREDITOR IN CASH/PARTY COMBOBOX
                LoadCboCashParty();
                #endregion

                grdSalesInvoice.Redim(1, 17);
                grdAdjustment.Redim(1, 7);
                btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                evtProductFocusLost.FocusLeft += new EventHandler(evtProductFocusLost_FocusLeft);
                evtBillSundryFocusLost.FocusLeft += new EventHandler(evtBillSundryFocusLost_FocusLeft);
                //Prepare the header part for grid
                AddAdjustmentGridHeader();
                AddGridHeader();
                AddRowProduct1(1);
                AddAdjustmentRow();
                cboProjectName.Items.Clear();
                cboDepot.SelectedIndex = 0;
                LoadComboboxProject(cboProjectName, 0);

                //  ShowAccClassInTreeView(treeAccClass, null, 0);

                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID (Old Code, In new code use navigation instead)
                txtSalesInvoiceID.Text = m_SalesInvoiceID.ToString();
                //if (m_SalesInvoiceID > 0)
                //{
                //    Navigation(Navigate.ID);
                //}
                if (m_SalesInvoiceID > 0)
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
                            vouchID = m_SalesInvoiceID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }

                        Sales m_Sales = new Sales();

                        //Getting the value of SeriesID via MasterID or VouchID
                        int SeriesIDD = m_Sales.GetSeriesIDFromMasterID(vouchID);

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
                        DataTable dtSalesInvoiceMaster = m_Sales.GetSalesInvoiceMasterInfo(vouchID.ToString());
                        if (dtSalesInvoiceMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");


                            return;
                        }
                        foreach (DataRow drSalesInvoiceMaster in dtSalesInvoiceMaster.Rows)
                        {
                            //txtproductpin.Text =    drSalesInvoiceMaster["PartyBillNumber"].ToString();
                            // if the form is recurring then donot load voucher number
                            if (!m_isRecurring)
                            {
                                txtVoucherNo.Text = drSalesInvoiceMaster["Voucher_No"].ToString();
                                txtDate.Text = Date.DBToSystem(drSalesInvoiceMaster["SalesInvoice_Date"].ToString());
                            }
                            else
                            {
                                txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if recurring load today's date
                                //txtduedate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                            }

                            txtRemarks.Text = drSalesInvoiceMaster["Remarks"].ToString();
                            txtSalesInvoiceID.Text = drSalesInvoiceMaster["SalesInvoiceID"].ToString();
                            txtOrderNo.Text = drSalesInvoiceMaster["OrderNo"].ToString();
                            lblNetAmout.Text = drSalesInvoiceMaster["Net_Amount"].ToString();
                            lblTax1.Text = drSalesInvoiceMaster["Tax1"].ToString();
                            lblTax2.Text = drSalesInvoiceMaster["Tax2"].ToString();
                            lblTax3.Text = drSalesInvoiceMaster["Tax3"].ToString();
                            lblVat.Text = drSalesInvoiceMaster["VAT"].ToString();
                            lblTotalQty.Text = drSalesInvoiceMaster["TotalQty"].ToString();
                            lblSpecialDiscount.Text = drSalesInvoiceMaster["SpecialDiscount"].ToString();
                            lblGross.Text = drSalesInvoiceMaster["Gross_Amount"].ToString();
                            lblGrandTotal.Text = (Convert.ToDouble(lblNetAmout.Text) + Convert.ToDouble(lblTax1.Text) + Convert.ToDouble(lblTax2.Text) + Convert.ToDouble(lblTax3.Text) + Convert.ToDouble(lblVat.Text)).ToString();
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

                                    txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();
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

                                    txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();
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

                                    txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();

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

                                    txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();

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

                                    txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                                    txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                                    txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                                    txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                                    txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();
                                }

                            }
                            dsSalesInvoice.Tables["tblSalesInvoiceMaster"].Rows.Add(cboSeriesName.Text, drSalesInvoiceMaster["Voucher_No"].ToString(), Date.DBToSystem(drSalesInvoiceMaster["SalesInvoice_Date"].ToString()), cboDepot.Text, drSalesInvoiceMaster["OrderNo"].ToString(), cboCashParty.Text, cmboSalesAcc.Text, drSalesInvoiceMaster["Remarks"].ToString());

                            DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesInvoiceMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drCashPartyLedgerInfo = dtCashPartyInfo.Rows[0];

                            cboCashParty.Text = drCashPartyLedgerInfo["LedName"].ToString();

                            //Show the corresponding Purchase Account Ledger in Combobox
                            DataTable dtSalesLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesInvoiceMaster["SalesLedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drSalesLedgerInfo = dtSalesLedgerInfo.Rows[0];
                            cmboSalesAcc.Text = drSalesLedgerInfo["LedName"].ToString();

                            DataTable dtDepotDtlInfo = Depot.GetDepotInfo(Convert.ToInt32(drSalesInvoiceMaster["DepotID"].ToString()));
                            foreach (DataRow drDepotInfo in dtDepotDtlInfo.Rows)
                            {
                                cboDepot.Text = drDepotInfo["DepotName"].ToString();
                            }
                        }

                        DataTable dtSalesInvoiceDetails = m_Sales.GetSalesInvoiceDetails(vouchID);
                        if (dtSalesInvoiceDetails.Rows.Count > 0)
                        {
                            for (int i = 1; i <= dtSalesInvoiceDetails.Rows.Count; i++)
                            {
                                DataRow drDetail = dtSalesInvoiceDetails.Rows[i - 1];
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SNo].Value = i.ToString();
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Code_No].Value = drDetail["Code"].ToString();
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty].Value = drDetail["CurUnitQty"].ToString();
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesRate].Value = drDetail["SalesRate"].ToString();
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString();
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString();
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.NetAmount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString();
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductID].Value = productID = Convert.ToInt32(drDetail["ProductID"].ToString());
                                AddRowProduct1(grdSalesInvoice.RowsCount);

                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Default_Qty].Value = Convert.ToInt32(drDetail["Quantity"]);
                                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Default_Unit].Value = Convert.ToInt32(drDetail["UnitID"]);
                                LoadCompoundUnit(i);

                                SourceGrid.CellContext context = new SourceGrid.CellContext(grdSalesInvoice, new SourceGrid.Position(i, (int)SalesInvoiceGridColumn.Units));
                                int index = Array.IndexOf(arrInt, Convert.ToInt32(drDetail["QtyUnitID"]));
                                context.Cell.Editor.SetCellValue(context, arrStr[index]);

                                dsSalesInvoice.Tables["tblSalesInvoiceDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(), drDetail["Quantity"].ToString(), drDetail["SalesRate"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());
                            }
                        }
                        // if recurring is true then donot load recurring settings for new voucher
                        if (!m_isRecurring)
                            CheckRecurringSetting(txtSalesInvoiceID.Text);
                    }

                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }

                #endregion

                txtProductPin.Select();

                // set value of SALES_REPORT_TYPE From Global
                //String salesReportType = Settings.GetSettings("SALES_REPORT_TYPE");
                //Global.Default_Sales_Report_Type = salesReportType;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }
        /// <summary>
        /// Method FOR SHOWING THE LEDGERS OF CASH IN HAND,DEBTOR AND CREDITOR IN CASH/PARTY COMBOBOX
        /// </summary>
        private void LoadCboCashParty()
        {
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
            double salesrate = 0;
            try
            {
                if (grdSalesInvoice[CurrRowPos, (int)SalesInvoiceGridColumn.ProductName].Value.ToString() == "(NEW)" || grdSalesInvoice[CurrRowPos, (int)SalesInvoiceGridColumn.ProductName].Value.ToString() == "")
                    return;
                try
                {
                    Productid = Convert.ToInt32(grdSalesInvoice[CurrRowPos, (int)SalesInvoiceGridColumn.ProductID].Value);
                    ProductName = grdSalesInvoice[CurrRowPos, (int)SalesInvoiceGridColumn.Code_No].Value.ToString();
                    productcode = grdSalesInvoice[CurrRowPos, (int)SalesInvoiceGridColumn.SNo].Value.ToString();
                    //purchaserate = Convert.ToDouble(grdSalesInvoice[CurrRowPos, 4].Value);
                    salesrate = Convert.ToDouble(grdSalesInvoice[CurrRowPos, (int)SalesInvoiceGridColumn.Qty].Value);

                    productID = Productid;
                    //GetListForGrdUnit();
                    LoadCompoundUnit(CurrRowPos);
                }
                catch
                {
                    Productid = 0;
                }
                if (Productid != 0 && CurrAccLProductid == 0)
                {
                    CurrAccLProductid = Productid;

                }
            }
            catch
            {
                return;
            }
            //FillAllGridRow(CurrRowPos, CurrAccLProductid,prodcode, ProdName, prate, salerate);
        }




        #region For Adjustment Amount Calculations

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdSalesInvoice.RowsCount - 2)
                grdSalesInvoice.Rows.Remove(ctx.Position.Row);

            CalculateGrossAmount();

            //Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
            //WriteNetAmount(CurRow);
            CalculateAdjustmentAmount();
            CalculateNetAmount();
            CalculateTotalQuantity();
        }

        private void evtBillSundryFocusLost_FocusLeft(object sender, EventArgs e)
        {

            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            string BillSundry = "";
            double Amount = 0.00;

            try
            {
                if (grdAdjustment[CurrRowPos, (int)AdjustmentGridColumn.BillSundry].Value.ToString() == "(NEW)" || grdAdjustment[CurrRowPos, (int)AdjustmentGridColumn.BillSundry].Value.ToString() == "")
                    return;

                BillSundry = grdAdjustment[CurrRowPos, (int)AdjustmentGridColumn.BillSundry].Value.ToString();
                Amount = Convert.ToDouble(grdAdjustment[CurrRowPos, (int)AdjustmentGridColumn.AdjustmentAmount].Value);

            }
            catch
            {
                return;
            }

        }

        private void AdjustmentDelete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdAdjustment.RowsCount - 1)
                grdAdjustment.Rows.Remove(ctx.Position.Row);
        }

        private void BillSundry_Selected(object sender, EventArgs e)
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
            int CurRow = grdAdjustment.Selection.GetSelectionRegion().GetRowsIndex()[0];
            int RowsCount = grdAdjustment.RowsCount;
            string LastBillSundryCell = (string)grdAdjustment[RowsCount - 1, (int)AdjustmentGridColumn.BillSundry].Value;
            ////Check whether the new row is already added
            if (LastBillSundryCell != "(NEW)")
            {
                AddAdjustmentRow();
                //Clear (NEW) on other colums as well
            }
            // WriteAdjustmentAmount(CurRow);
        }

        private void cmbBillSundrySelectedIndex_Changed(object sender, EventArgs e)
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
            int RowsCount = grdAdjustment.RowsCount;
            string LastBillSundryCell = (string)grdAdjustment[RowsCount - 1, (int)AdjustmentGridColumn.BillSundry].Value;
            ////Check whether the new row is already added
            if (LastBillSundryCell != "(NEW)")
            {
                AddAdjustmentRow();
                //Clear (NEW) on other colums as well
            }


        }

        private void AddAdjustmentGridHeader()
        {
            grdAdjustment[0, (int)AdjustmentGridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del");
            grdAdjustment[0, (int)AdjustmentGridColumn.Del].Column.Width = 30;
            grdAdjustment[0, (int)AdjustmentGridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdAdjustment[0, (int)AdjustmentGridColumn.SNo].Column.Width = 100;
            grdAdjustment[0, (int)AdjustmentGridColumn.BillSundry] = new SourceGrid.Cells.ColumnHeader("Bill Sundry");
            grdAdjustment[0, (int)AdjustmentGridColumn.BillSundry].Column.Width = 100;
            grdAdjustment[0, (int)AdjustmentGridColumn.Narration] = new SourceGrid.Cells.ColumnHeader("Narration");
            grdAdjustment[0, (int)AdjustmentGridColumn.Narration].Column.Width = 100;
            grdAdjustment[0, (int)AdjustmentGridColumn.Charge] = new SourceGrid.Cells.ColumnHeader("Charge");
            grdAdjustment[0, (int)AdjustmentGridColumn.Charge].Column.Width = 100;
            grdAdjustment[0, (int)AdjustmentGridColumn.percent] = new SourceGrid.Cells.ColumnHeader("Percent");
            grdAdjustment[0, (int)AdjustmentGridColumn.percent].Column.Width = 100;
            grdAdjustment[0, (int)AdjustmentGridColumn.AdjustmentAmount] = new SourceGrid.Cells.ColumnHeader("Adjustment Amount(Rs)");
            grdAdjustment[0, (int)AdjustmentGridColumn.AdjustmentAmount].Column.Width = 150;
        }

        private void AddAdjustmentRow()
        {
            //Add a new row
            try
            {
                int RowCount = grdAdjustment.RowsCount;
                grdAdjustment.Redim(Convert.ToInt32(RowCount + 1), grdAdjustment.ColumnsCount);
                grdAdjustment.Redim(Convert.ToInt32(RowCount + 1), grdAdjustment.ColumnsCount);
                SourceGrid.Cells.Button btnAdjustmentDelete = new SourceGrid.Cells.Button("");
                btnAdjustmentDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                int i = RowCount;
                grdAdjustment[i, (int)AdjustmentGridColumn.Del] = btnAdjustmentDelete;
                grdAdjustment[i, (int)AdjustmentGridColumn.Del].AddController(evtAdjustmentDelete);

                grdAdjustment[i, (int)AdjustmentGridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());

                SourceGrid.Cells.Editors.ComboBox cmbBillSundry = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                cmbBillSundry.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                cmbBillSundry.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbBillSundry.StandardValues = new string[] { "Round off(+)", "Round off(-)" };
                grdAdjustment[i, (int)AdjustmentGridColumn.BillSundry] = new SourceGrid.Cells.Cell("", cmbBillSundry);
                //cmbBillSundry.Control.LostFocus += new EventHandler(Product_Leave);
                //cmbBillSundry.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                //cmbBillSundry.Control.TextChanged += new EventHandler(Adjustment_Amount_Focus_Lost);
                //grdAdjustment[i, 1].AddController(evtBillSundryFocusLost);
                grdAdjustment[i, (int)AdjustmentGridColumn.BillSundry].Value = "(NEW)";


                SourceGrid.Cells.Editors.TextBox txtNarration = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtNarration.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdAdjustment[i, (int)AdjustmentGridColumn.Narration] = new SourceGrid.Cells.Cell("", txtNarration);
                txtNarration.Control.TextChanged += new EventHandler(Text_Change);

                SourceGrid.Cells.Editors.TextBox txtcharge = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtcharge.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdAdjustment[i, (int)AdjustmentGridColumn.Charge] = new SourceGrid.Cells.Cell("0.000", txtcharge);
                txtcharge.Control.TextChanged += new EventHandler(Text_Change);

                SourceGrid.Cells.Editors.TextBox txtpercent = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtpercent.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdAdjustment[i, (int)AdjustmentGridColumn.percent] = new SourceGrid.Cells.Cell("0.000 ", txtpercent);
                txtpercent.Control.TextChanged += new EventHandler(Text_Change);

                SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtAmount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdAdjustment[i, (int)AdjustmentGridColumn.AdjustmentAmount] = new SourceGrid.Cells.Cell("0.000", txtAmount);
                txtAmount.Control.LostFocus += new EventHandler(Adjustment_Amount_Focus_Lost);

            }
            catch (Exception ex)
            {
            }
        }

        private void AdjustmentAmount_Text_Change(object sender, EventArgs e)
        {
            //try
            //{
            //    double AdjustmentAmount = 0;
            //    for (int i = 1; i <= grdAdjustment.RowsCount - 1; i++)
            //    {
            //        if (i == grdAdjustment.Rows.Count)
            //            return;
            //        double m_AdjustmentAmount = 0;
            //        string m_Value = Convert.ToString(grdAdjustment[i, (int)AdjustmentGridColumn.AdjustmentAmount].Value);
            //        if (m_Value.Length == 0)
            //            m_AdjustmentAmount = 0.000;
            //        else
            //            m_AdjustmentAmount = Convert.ToDouble(grdAdjustment[i, (int)AdjustmentGridColumn.AdjustmentAmount].Value);

            //        if (grdAdjustment[i, (int)AdjustmentGridColumn.BillSundry].Value == "Round off(+)")
            //        {
            //            AdjustmentAmount += m_AdjustmentAmount;
            //            AdjAmount = AdjustmentAmount;
            //        }

            //        else if (grdAdjustment[i, (int)AdjustmentGridColumn.BillSundry].Value == "Round off(-)")
            //        {
            //            AdjustmentAmount -= m_AdjustmentAmount;
            //            AdjAmount = AdjustmentAmount;
            //        }

            //        lblAdjustment.Text = AdjustmentAmount.ToString();
            //    }
            //}
            //catch (Exception ex)
            //{

            //    Global.MsgError("Error in Adjustment Amount calucation!");
            //}
        }

        private void BillSundry_Focus_Lost(object sender, EventArgs e)
        {
            hasChanged = false;
        }

        private void Adjustment_Amount_Focus_Lost(object sender, EventArgs e)
        {

            IsFieldChanged = true;
            int RowCount = grdAdjustment.RowsCount;
            string BillSundry = (string)(grdAdjustment[RowCount - 1, 2].Value);


            try
            {

                //#BUG: When grid focuses out, it takes the previous value, not the edited value, so replace it with the new value
                object NewAdjustmentAmount = ((TextBox)sender).Text;
                int CurRow = grdAdjustment.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (grdAdjustment[CurRow, (int)AdjustmentGridColumn.AdjustmentAmount].Value != NewAdjustmentAmount)
                    grdAdjustment[CurRow, (int)AdjustmentGridColumn.AdjustmentAmount].Value = NewAdjustmentAmount;

                //Add a new row
                if (BillSundry != "(NEW)")
                {
                    AddAdjustmentRow();
                }


                CalculateNetAmount();

            }
            catch
            {
                return;
            }
        }


        /// <summary>
        /// Calculates the total Net amount of Adjustment tab, calculating all the amount column
        /// </summary>
        private void CalculateAdjustmentAmount()
        {

            AdjustmentAmount = 0; //Reset the Global Adjustment Variable
            try
            {

                double m_AdjustmentAmount = 0;
                for (int i = 1; i < grdAdjustment.RowsCount - 1; i++)
                {
                    if (i == grdAdjustment.Rows.Count)
                        return;

                    string m_Value = Convert.ToString(grdAdjustment[i, (int)AdjustmentGridColumn.AdjustmentAmount].Value);
                    if (m_Value.Length == 0)
                        m_AdjustmentAmount = 0.000;
                    else
                        m_AdjustmentAmount = Convert.ToDouble(grdAdjustment[i, (int)AdjustmentGridColumn.AdjustmentAmount].Value);

                    if (grdAdjustment[i, (int)AdjustmentGridColumn.BillSundry].Value.ToString() == "Round off(+)")
                    {
                        AdjustmentAmount += m_AdjustmentAmount;
                    }

                    else if (grdAdjustment[i, (int)AdjustmentGridColumn.BillSundry].Value.ToString() == "Round off(-)")
                    {
                        AdjustmentAmount -= m_AdjustmentAmount;
                    }

                }

                // AdjustmentAmount=Math.Round((AdjustmentAmount),Global.DecimalPlaces);
                lblAdjustment.Text = AdjustmentAmount.ToString((Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
            }
            catch (Exception ex)
            {
                Global.MsgError("Error in Adjustment Amount calucation!");
            }
        }



        #endregion


        /// <summary>
        /// Sums up all NetAmount
        /// </summary>

        private void CalculateNetAmount()
        {
            try
            {
                double NetAmount = 0;
                //Also consider 13% VAT too
                double VatAmt = 0;

                string Vatvalue = Settings.GetSettings("DEFAULT_SALES_VAT");//Overall vat applicable or not 1=applicable 0=not applicable


                #region Loop through all rows of the grid
                for (int i = 1; i < grdSalesInvoice.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row.If it is ,it must be the last row.

                    if (i == grdSalesInvoice.Rows.Count)
                        return;
                    double m_NetAmount = 0;
                    string m_Value = Convert.ToString(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.NetAmount].Value);
                    if (m_Value.Length == 0)
                        m_NetAmount = 0;
                    else
                        m_NetAmount = Convert.ToDouble(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.NetAmount].Value);


                    NetAmount += m_NetAmount;



                    //VAT application
                    //Check if product is VAT applicable or not

                    if (Vatvalue == "1")//If system is VAT applicable
                    {
                        int productid = Convert.ToInt32(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductID].Value);

                        DataTable dtCheckVATApplicable = Product.GetProductByID(productid);
                        DataRow drcheckvatapplicable = dtCheckVATApplicable.Rows[0];

                        if (drcheckvatapplicable["IsVatApplicable"].ToString() == "1") //If Product is VAT applicable
                        {

                            VatAmt += (m_NetAmount * VAT) / 100;


                        }
                    }

                }
                #endregion

                #region Calculate other taxes
                //lblNetAmout.Text = (NetAmount + VatAmt).ToString();
                string Tax1checkvalue = Settings.GetSettings("DEFAULT_SALES_TAX1CHECK");
                string Tax2checkvalue = Settings.GetSettings("DEFAULT_SALES_TAX2CHECK");
                string Tax3checkvalue = Settings.GetSettings("DEFAULT_SALES_TAX3CHECK");
                string Tax1value = Settings.GetSettings("DEFAULT_SALES_TAX1");
                string Tax2value = Settings.GetSettings("DEFAULT_SALES_TAX2");
                string Tax3value = Settings.GetSettings("DEFAULT_SALES_TAX3");
                if (Tax1checkvalue == "1")
                {
                    if (Tax1value == "Nt Amt")
                    {
                        // double tax1amt=0;
                        tax1amt = (NetAmount * TAX1) / 100;
                        lblTax1.Text = tax1amt.ToString();
                    }
                    else if (Tax1value == "Gross")
                    {

                        tax1amt = (GAmount * TAX1) / 100;
                        lblTax1.Text = tax1amt.ToString();
                    }
                }
                if (Tax2checkvalue == "1")
                {
                    if (Tax2value == "Nt Amt")
                    {
                        tax2amt = (NetAmount * TAX2) / 100;
                        lblTax2.Text = tax2amt.ToString();
                    }
                    else if (Tax2value == "Tax 1")
                    {
                        tax2amt = (tax1amt * TAX2) / 100;
                        lblTax2.Text = tax2amt.ToString();
                    }
                    else if (Tax2value == "Gross")
                    {
                        tax2amt = (GAmount * TAX2) / 100;
                        lblTax2.Text = tax2amt.ToString();
                    }
                }
                if (Tax3checkvalue == "1")
                {
                    if (Tax3value == "Nt Amt")
                    {
                        tax3amt = (NetAmount * TAX3) / 100;
                        lblTax3.Text = tax3amt.ToString();
                    }
                    else if (Tax3value == "Tax 1")
                    {
                        tax3amt = (tax1amt * TAX3) / 100;
                        lblTax3.Text = tax3amt.ToString();
                    }
                    else if (Tax3value == "Tax 2")
                    {
                        tax3amt = (tax2amt * TAX3) / 100;
                        lblTax3.Text = tax3amt.ToString();
                    }
                    if (Tax3value == "Gross")
                    {
                        tax3amt = (GAmount * TAX3) / 100;
                        lblTax3.Text = tax3amt.ToString();
                    }
                }
                #endregion



                CalculateAdjustmentAmount();

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
                double GrossAmount = 0;
                for (int i = 1; i < grdSalesInvoice.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row.If it is ,it must be the last row.

                    if (i == grdSalesInvoice.Rows.Count)
                        return;
                    double m_GrossAmount = 0;
                    string m_Value = Convert.ToString(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount].Value);
                    if (m_Value.Length == 0)
                        m_GrossAmount = 0;
                    else
                        m_GrossAmount = Convert.ToDouble(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount].Value);

                    GrossAmount += m_GrossAmount;
                    GAmount = GrossAmount;

                    lblGross.Text = GrossAmount.ToString((Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

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
                for (int i = 1; i < grdSalesInvoice.RowsCount - 1; i++)
                {
                    if (i == grdSalesInvoice.Rows.Count)
                        return;
                    double m_TotalQuantity = 0;
                    string m_Value = Convert.ToString(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty].Value);
                    if (m_Value.Length == 0)
                        m_TotalQuantity = 0;
                    else
                        m_TotalQuantity = Convert.ToDouble(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty].Value);

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
        private void CalculateTotalDiscount()
        {
            try
            {
                Double TotalDiscount = 0;
                for (int i = 1; i < grdSalesInvoice.RowsCount - 1; i++)
                {
                    if (i == grdSalesInvoice.Rows.Count)
                        return;
                    double m_TotalDiscount = 0;
                    string m_Value = Convert.ToString(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc].Value);
                    if (m_Value.Length == 0)
                        m_TotalDiscount = 0;
                    else
                        m_TotalDiscount = Convert.ToDouble(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc].Value);

                    TotalDiscount += m_TotalDiscount;

                    lblSpecialDiscount.Text = TotalDiscount.ToString();
                    lblDiscountAmount.Text = TotalDiscount.ToString();
                    // lblGross.Text = (Convert.ToDouble(lblGross.Text) - TotalDiscount).ToString();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError("Error in Total Discount calucation!");
            }
        }

        /// <summary>
        /// Sum up all discount; This method is specially for Special Discount modified in grid.
        /// </summary>
        private void CalculateTotalDiscount1()
        {
            try
            {
                Double TotalDiscount = 0;
                for (int i = 1; i < grdSalesInvoice.RowsCount - 1; i++)
                {
                    if (i == grdSalesInvoice.Rows.Count)
                        return;
                    double m_TotalDiscount = 0;
                    //string m_Value = Convert.ToString(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc].Value);
                    //string m_Value = "";
                    double Amount = 0;
                    Amount = Convert.ToDouble(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount].Value);

                    double DisPercentage = Convert.ToDouble(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value);
                    double Discount = 0;
                    if (DisPercentage != 0)
                    {
                        Discount = Math.Round(((Amount * Convert.ToDouble(DisPercentage)) / 100), Global.DecimalPlaces);
                    }
                    if (Discount == 0)
                        m_TotalDiscount = 0;
                    else
                        m_TotalDiscount = Discount;

                    TotalDiscount += m_TotalDiscount;

                    lblSpecialDiscount.Text = TotalDiscount.ToString();
                    lblDiscountAmount.Text = TotalDiscount.ToString();
                    // lblGross.Text = (Convert.ToDouble(lblGross.Text) - TotalDiscount).ToString();
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

            //Using the name find corresponding code
            //DataTable dt = Product.GetProductByName(ct.DisplayText);
            DataTable dt = Product.GetProductByCode(ct.DisplayText);
            //if (dt.Rows.Count <= 0)
            //{
            //    Global.MsgError("The selected Product/Service name is invalid. Please select it from the list and try again.");
            //    return;
            //}            


            int CurRow = grdSalesInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row
                    grdSalesInvoice[(CurRow), 3].Value = dr[LangField].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double SalesRate = Math.Round(Convert.ToDouble(dr["SalesRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.
                    grdSalesInvoice[(CurRow), (int)SalesInvoiceGridColumn.SalesRate].Value = SalesRate.ToString();
                    grdSalesInvoice[(CurRow), (int)SalesInvoiceGridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
            }
            int RowsCount = grdSalesInvoice.RowsCount;
            string LastProductCell = (string)grdSalesInvoice[RowsCount - 1, (int)SalesInvoiceGridColumn.ProductName].Value;
            ////Check whether the new row is already added
            if (LastProductCell != "(NEW)")
            {
                AddRowProduct1(RowsCount);
                //Clear (NEW) on other colums as well
            }
            WriteAmount(CurRow, (int)SalesInvoiceGridColumn.SNo);//Write amount on grid'cell when quantity is unit
            WriteNetAmount(CurRow);//Write Net amount on corresponding cell of grid.It can also handle when value of quantity is unit and discount is 0
            CalculateGrossAmount();
            CalculateAdjustmentAmount();//After summing up all gross amount,this function display the value in label
            CalculateNetAmount(); //After summing up all net amount,this function display the value in lablel
            CalculateTotalQuantity();
        }



        private void WriteAmount(int CurRow, double Qty)
        {

            double m_SalesRate = 0;
            try
            {
                string SalesRate = Convert.ToDecimal(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SalesRate].Value).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                m_SalesRate = Convert.ToDouble(SalesRate);
                double Amount = Convert.ToDouble(Qty) * m_SalesRate;
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value = Convert.ToDecimal(Amount).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            }
            catch (Exception) //Just do nothing in case of error
            {
            }

        }

        private void WriteNetAmount(int CurRow)
        {

            try
            {
                string Amount = Convert.ToDecimal(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                string Discount = Convert.ToDecimal(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SplDisc].Value).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                double NetAmount = Convert.ToDouble(Amount) - Convert.ToDouble(Discount);
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.NetAmount].Value = Convert.ToDecimal(NetAmount).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            }
            catch //Do nothing in case of error
            {

            }

        }

        /// <summary>
        /// This method is specially for Special Discount Modified as there was a problem
        /// </summary>
        /// <param name="CurRow"></param>
        private void WriteNetAmount1(int CurRow)
        {

            try
            {
                string Amount = grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value.ToString();
                double DisPercentage = Convert.ToDouble(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value);
                double Discount = Math.Round(((Convert.ToDouble(Amount) * DisPercentage) / 100), Global.DecimalPlaces);
                double NetAmount = Convert.ToDouble(Amount) - Convert.ToDouble(Discount);
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.NetAmount].Value = Convert.ToDecimal(NetAmount).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            }
            catch //Do nothing in case of error
            {

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
            if (grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Code_No].Value == "(NEW)")
                return true;
            else
                return false;

        }

        private void Qty_Modified(object sender, EventArgs e)
        {
            try
            {
                //find the current row of source grid
                int CurRow = grdSalesInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];

                if (isNewRow(CurRow))
                    return;

                int RowCount = grdSalesInvoice.RowsCount;
                object Qty = ((TextBox)sender).Text;
                //bool IsInt = Misc.IsInt(Qty, false);//This function check whether variable is integer or not?

                //with help of hidden column PurchaseOrderDetailsID,we can find the the details info PurchaseOrderDetails


                //10th Column is for SalesDetailsID 
                string SalesOrderDetailID = grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Value.ToString();
                if ((SalesOrderDetailID != "") && (Convert.ToInt32(SalesOrderDetailID) > 0))//It means Invoice is going to make according to Order,so we have to check Quantity
                {


                    DataTable dtSalesOrderDetails = SalesOrder.GetSalesOrderDetailsByDetailsID(Convert.ToInt32(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Value));
                    DataRow drSalesOrderDetails = dtSalesOrderDetails.Rows[0];

                    double InvoiceQty = Convert.ToDouble(Qty);
                    double OrderQty = Convert.ToDouble(drSalesOrderDetails["Quantity"]);
                    if (InvoiceQty > OrderQty)
                    {
                        Global.Msg("Entered Quantity is greater than Pending Quantity");
                        return;
                    }


                }


                //Check whether the value of quantity is zero or not?
                if (!(Convert.ToDouble(Qty) > 0))
                {
                    Global.MsgError("The Quantity shouldnot be zero. Fill the Quantity first!");
                    //grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value = "1.0";
                    //grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value = "0";
                    ((TextBox)sender).Text = "1";
                    Qty = 1;
                   // return;
                }

                // check if decimal palces is applicable or not
                decimal decQty = Convert.ToDecimal(Qty);
                int intQty = (int)decQty;

                if ((Convert.ToDecimal(intQty) != decQty))
                {
                    if (!Product.IsDecimalApplicable(Convert.ToInt32(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.ProductID].Value)))
                    {
                        Global.Msg("Decimal places is not applicable for this product ! Plesse enter valid value !");
                        //grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value = "1.0";
                        ((TextBox)sender).Text = intQty.ToString();
                        Qty = intQty;
                    }
                }

                //grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Stock_Qty].Value = StockQty - Convert.ToInt32(Qty);

                WriteAmount(CurRow, Convert.ToDouble(Qty));
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value = Qty;
                Unit_Changed(sender, e);

                //CalculateGrossAmount();

                ////Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
                //WriteNetAmount(CurRow);
                //CalculateAdjustmentAmount();
                //CalculateNetAmount();
                //CalculateTotalQuantity();
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
                //find the current row of source grid
                int CurRow = grdSalesInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;

                int RowCount = grdSalesInvoice.RowsCount;
                object SalesRate = ((TextBox)sender).Text;
                bool IsDouble = Misc.IsNumeric(SalesRate);//This function check whether variable is Double  or not?
                if (IsDouble == false)
                {
                    Global.MsgError("The Sales Rate you posted is invalid! Please post the integer value");
                    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value = "";
                    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value = "1";
                    return;
                }

                string Qty = grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value.ToString();

                Unit_Changed(sender, e);
                //double Amount = Convert.ToDouble(Qty) * Convert.ToDouble(SalesRate);
                //grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value = Amount.ToString();
                //CalculateGrossAmount();
                //WriteNetAmount(CurRow);
                //CalculateAdjustmentAmount();
                //CalculateNetAmount();
                //CalculateTotalQuantity();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        //for discount Percentage modification
        private void DiscPercentage_Modified(object sender, EventArgs e)
        {
            int CurRow = grdSalesInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
            double Amount = 0;
            Amount = Convert.ToDouble(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value);

            //if (Amount == 0)
            //{
            //    Global.MsgError("The Amount shouldnot be Zero.Please post the required value in corresponding field!");
            //    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value = "0";
            //    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SplDisc].Value = "0";
            //    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.NetAmount].Value = "0";
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
            lblDiscount.Text = DisPercentage.ToString();

            //double NetAmount = Math.Round((Amount - Discount), Global.DecimalPlaces);

            if (isNewRow(CurRow))
                return;
            grdSalesInvoice[(CurRow), (int)SalesInvoiceGridColumn.SplDisc].Value = Discount.ToString();
            WriteNetAmount(CurRow);
            CalculateAdjustmentAmount();
            CalculateNetAmount();
            CalculateTotalDiscount();
        }

        private void Discount_Modified(object sender, EventArgs e)
        {
            int CurRow = grdSalesInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];//Finding the current row of SourceGrid
            double Amount = 0;
            Amount = Convert.ToDouble(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value);
            //if (Amount == 0)
            //{
            //    Global.MsgError("The Amount shouldnot be Zero. Please post the required value in corresponding field!");
            //    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value = "0";
            //    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SplDisc].Value = "0";
            //    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.NetAmount].Value = "0";
            //    return;
            //}
            object Discount = ((TextBox)sender).Text;
            bool IsDouble = Misc.IsNumeric(Discount);//This function check whether variable is double or not?
            if (IsDouble == false)
            {
                Global.MsgError("The Discount Amount you posted is invalid! Please post the  numeric value");
                return;
            }
            double disc = (Convert.ToDouble(Discount));
            double DiscPercentage =0;
            if(disc!=0)
            {
                DiscPercentage = Math.Round(((Convert.ToDouble(Discount) * 100) / Amount), Global.DecimalPlaces);
            }
            lblDiscount.Text = DiscPercentage.ToString();
            //double NetAmount = Math.Round((Amount - Convert.ToDouble(Discount)), Global.DecimalPlaces);
            if (isNewRow(CurRow))
                return;
            grdSalesInvoice[(CurRow), (int)SalesInvoiceGridColumn.SplDisc_Percent].Value = DiscPercentage.ToString();

            ////Calling Discpercentage_modified again after the discount percent is calculated.
            //DiscPercentage_Modified(sender, e);

            WriteNetAmount1(CurRow);
            CalculateAdjustmentAmount();
            CalculateNetAmount();
            CalculateTotalDiscount1();

        }

        private void AddGridHeader()
        {
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Del].Column.Width = 30;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Del].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.SNo].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Code_No] = new SourceGrid.Cells.ColumnHeader("Code");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Code_No].Column.Width = 150;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Code_No].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.ProductName] = new SourceGrid.Cells.ColumnHeader("Product Name");
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.ProductName].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.ProductName].Column.Width = 190;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Qty] = new SourceGrid.Cells.ColumnHeader("Qty");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Qty].Column.Width = 60;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Qty].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Units] = new SourceGrid.Cells.ColumnHeader("Unit");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Units].Column.Width = 100;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Units].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Default_Qty] = new SourceGrid.Cells.ColumnHeader("Default_Qty");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Default_Qty].Column.Visible = false;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Default_Unit] = new SourceGrid.Cells.ColumnHeader("Default_Unit");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Default_Unit].Column.Visible = false;

            //grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Default_Qty].Column.Width = 100;
            //grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Default_Qty].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            //grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Units] = new SourceGrid.Cells.ColumnHeader("Unit");
            //grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Units].Column.Width = 100;
            //grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Units].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Stock_Qty] = new SourceGrid.Cells.ColumnHeader("Stock Qty");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Stock_Qty].Column.Width = 60;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Qty].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Stock_Qty_Actual] = new SourceGrid.Cells.ColumnHeader("Stock Qty Actual");
            //grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Stock_Qty_Actual].Column.Width = 60;
            //grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Stock_Qty_Actual].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Stock_Qty_Actual].Visible = false;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SalesRate] = new SourceGrid.Cells.ColumnHeader("SalesRate");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SalesRate].Column.Width = 80;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.SalesRate].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Amount] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.Amount].Column.Width = 120;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.Amount].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SplDisc_Percent] = new SourceGrid.Cells.ColumnHeader("Spl. Disc%");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SplDisc_Percent].Column.Width = 80;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.SplDisc_Percent].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SplDisc] = new SourceGrid.Cells.ColumnHeader("Spl. Disc");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SplDisc].Column.Width = 100;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.SplDisc].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.NetAmount] = new SourceGrid.Cells.ColumnHeader("Net Amount");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.NetAmount].Column.Width = 150;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.NetAmount].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SalesOrderDetailID] = new SourceGrid.Cells.ColumnHeader("SalesOrderDetailsID");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Column.Width = 5;
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Column.Visible = false;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.SalesOrderDetailID].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.ProductID] = new SourceGrid.Cells.ColumnHeader("ProductID");
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.ProductID].Column.Width = 10;
            grdSalesInvoice[0, (int)SalesInvoiceGridColumn.ProductID].Column.Visible = false;
            grdSalesInvoice.Columns[(int)SalesInvoiceGridColumn.ProductID].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;


            grdSalesInvoice.AutoStretchColumnsToFitWidth = true;
            //grdSalesInvoice.AutoSizeCells();
            //grdSalesInvoice.Columns.StretchToFit();
        }

        /// <summary>
        /// Adds the row in the Sales Invoice field
        /// </summary>
        /// 

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }

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
                        string returnStr;
                        //string returnStr = m_VouConfig.ValidateManualVouNum(txtVoucherNo.Text, Convert.ToInt32(SeriesID.ID));
                        if (m_mode == EntryMode.NEW)
                            returnStr = m_VouConfig.ValidateManualVouNum(txtVoucherNo.Text, Convert.ToInt32(SeriesID.ID), txtSalesInvoiceID.Text == "" ? 0 : Convert.ToInt32(txtSalesInvoiceID.Text), "Inv.tblSalesInvoiceMaster", true);

                        else
                            returnStr = m_VouConfig.ValidateManualVouNum(txtVoucherNo.Text, Convert.ToInt32(SeriesID.ID), txtSalesInvoiceID.Text == "" ? 0 : Convert.ToInt32(txtSalesInvoiceID.Text), "Inv.tblSalesInvoiceMaster", false);


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
            if (txtOrderNo.Text != "")
                OrderNo = Convert.ToInt32(txtOrderNo.Text);

            if (txtDueDays.Text == "")
                txtDueDate.Text = Date.ToSystem(Convert.ToDateTime(Date.GetServerDate())).ToString();
            if (drdtadditionalfield["IsField1Required"].ToString() == "True")
            {
                if (txtfirst.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field1"].ToString() + " " + "is Required Field");
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
                        int increasedSeriesNum = 0;
                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumTypeNoUpdate(Convert.ToInt32(SeriesID.ID), out increasedSeriesNum);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers has already been finished!");
                                return;
                            }

                            txtVoucherNo.Text = m_vounum.ToString();
                        }

                        isNew = true;
                        OldGrid = " ";
                        NewGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + "-" + txtVoucherNo.Text + "," + "Voucher Date" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Date" + "-" + txtDueDate.Text + "," + "Cash/Party" + "-" + cboCashParty.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string productname = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductName].Value.ToString() + ",";
                            string qty = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Qty].Value.ToString() + ",";
                            string rate = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesRate].Value.ToString() + ",";
                            string amt = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Amount].Value.ToString() + ",";
                            NewGrid = NewGrid + string.Concat(productname, qty, rate, amt) + ",";
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        //Read from sourcegrid and store it to table
                        DataTable SalesInvoiceDetails = new DataTable();
                        SalesInvoiceDetails.Columns.Add("Code");
                        SalesInvoiceDetails.Columns.Add("Product");
                        SalesInvoiceDetails.Columns.Add("Quantity");
                        SalesInvoiceDetails.Columns.Add("SalesRate");
                        SalesInvoiceDetails.Columns.Add("Amount");
                        SalesInvoiceDetails.Columns.Add("DiscPercentage");
                        SalesInvoiceDetails.Columns.Add("Discount");
                        SalesInvoiceDetails.Columns.Add("NetAmount");
                        SalesInvoiceDetails.Columns.Add("SalesOrderDetailsID");
                        SalesInvoiceDetails.Columns.Add("ProductID");
                        SalesInvoiceDetails.Columns.Add("QtyUnitID");

                        for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            SalesInvoiceDetails.Rows.Add(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Code_No].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductName].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Default_Qty].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesRate].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Amount].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SplDisc].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.NetAmount].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductID].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Units].Value);
                        }

                        // add data of SalesInvoiceDetails to dtsalesInvoice which is used to  print the receipt
                        dtsalesInvoice = SalesInvoiceDetails;

                        DateTime SalesInvoice_Date = Date.ToDotNet(txtDate.Text);
                        ListItem liSalesLedgerID = new ListItem();
                        liSalesLedgerID = (ListItem)cmboSalesAcc.SelectedItem;

                        ListItem LiCashPartyLedgerID = new ListItem();
                        LiCashPartyLedgerID = (ListItem)cboCashParty.SelectedItem;

                        DateTime SalesDueDate = Date.ToDotNet(txtDueDate.Text);


                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        int Tax1ID = AccountGroup.GetLedgerIDFromLedgerNumber(314);
                        int Tax2ID = AccountGroup.GetLedgerIDFromLedgerNumber(315);
                        int Tax3ID = AccountGroup.GetLedgerIDFromLedgerNumber(316);
                        int VatID = AccountGroup.GetLedgerIDFromLedgerNumber(412);

                        #region Check For The Negative Stock
                        double QuantityAvailable = 0;

                        string AccClassIDsXMLString = ReadAllAccClassID();
                        string ProjectIDsXMLString = ReadAllProjectID();

                        int uid = User.CurrUserID;
                        DataTable dtroleinfo = User.GetUserInfo(uid);
                        DataRow dr = dtroleinfo.Rows[0];
                        UserAccClass = Convert.ToInt32(dr["AccClassID"].ToString());

                        double closingQuantity = 0;

                        //Donot Check is Inventory is not applicable

                        for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            bool isTrans = false;
                            int productid = Convert.ToInt32(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductID].Value);
                            DataTable dtcheckinventoryapplicable = Product.GetProductByID(productid);
                            DataRow drcheckvinventoryapplicable = dtcheckinventoryapplicable.Rows[0];

                            //if (drcheckvinventoryapplicable["IsInventoryApplicable"].ToString() == "1")
                            //{
                            //    DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(grdSalesInvoice[i + 1, 11].Value.ToString()), " ", Date.ToDotNet(txtDate.Text).AddDays(1), true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                            //    DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(grdSalesInvoice[i + 1, 11].Value.ToString()), "", Date.ToDotNet(txtDate.Text).AddDays(1), true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
                            //    // DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(m_StockStatus.ProductGroupID, m_StockStatus.ProductID, m_StockStatus.Depot, m_StockStatus.AtTheEndDate, m_StockStatus.ShowZeroQunatity, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);//use InventoyBookType becuase its index is zero soo it looks for all VoucherType and its difference than InventoryBook becuase it is filtered by Product
                            //    if (dtTransactionStockStatusInfo.Rows.Count != 0)
                            //    {
                            //        foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
                            //        {
                            //            // if (dtTransactionStockStatusInfo.Rows.Count != 0)
                            //            //{
                            //            foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
                            //            {
                            //                if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
                            //                {
                            //                    isTrans = true;
                            //                    closingQuantity = Convert.ToDouble(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
                            //                }
                            //            }
                            //            if (isTrans == false)
                            //            {
                            //                closingQuantity = Convert.ToDouble(drOpeningStockStatusInfo["Quantity"]);
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(grdSalesInvoice[i + 1, 11].Value.ToString()), " ", Date.ToDotNet(txtDate.Text).AddDays(1), true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                            //        if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                            //        {
                            //            DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                            //            closingQuantity = Convert.ToDouble(dropen["Quantity"].ToString());
                            //        }
                            //    }

                            double qty = Convert.ToDouble(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Qty].Value.ToString());
                            int productID = Convert.ToInt32(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductID].Value.ToString());

                            double stockQty = Convert.ToDouble(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Stock_Qty].Value.ToString());

                            DataTable dtStockStatusInfo = Product.GetAllProduct(null, Convert.ToInt32(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductID].Value.ToString()), " ", Date.ToDotNet(txtDate.Text).AddDays(1), true, StockStatusType.DiffInStock);
                            if (dtStockStatusInfo.Rows.Count > 0)
                            {
                                closingQuantity = Convert.ToDouble(dtStockStatusInfo.Rows[0]["ClosingStock"].ToString());
                                if ((closingQuantity - qty) < 0 || stockQty < 0)
                                {
                                    if (Global.Default_NegativeStock == NegativeStock.Warn)
                                    {
                                        if (MessageBox.Show("Quantity in Stock is Less Than Selling Quantity, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            return;
                                        }
                                    }
                                    if (Global.Default_NegativeStock == NegativeStock.Deny)
                                    {
                                        Global.MsgError("Stock Quantity is Lesser than in selling quantity , you are not allowed to submit this voucher!!!");
                                        return;
                                    }
                                }
                            }
                            //if (Global.Default_NegativeStock == NegativeStock.Warn)
                            //{
                            //    if (closingQuantity < qty)
                            //    {
                            //        if (MessageBox.Show("Quantity in Stock is Less Than Selling Quantity, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                            //        {
                            //            return;
                            //        }
                            //    }
                            //}
                            //if (Global.Default_NegativeStock == NegativeStock.Deny)
                            //{
                            //    if (closingQuantity < qty)
                            //    {
                            //        Global.MsgError("Stock Quantity is Lesser than in selling quantity , you are not allowed to submit this voucher!!!");
                            //        return;
                            //    }
                            //}
                        }

                        #endregion

                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        if (AccClassID.Count != 0)
                        {
                            m_SalesInvoice.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liSalesLedgerID.ID), liSalesLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), LiCashPartyLedgerID.Value.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), OrderNo, txtVoucherNo.Text, SalesInvoice_Date, txtRemarks.Text, SalesInvoiceDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Sales_Tax1On, Global.Default_Sales_Tax1Check, Global.Default_Tax2, Global.Default_Sales_Tax2On, Global.Default_Sales_Tax2Check, Global.Default_Tax3, Global.Default_Sales_Tax3On, Global.Default_Sales_Tax3On, OldGrid, NewGrid, isNew, lblTax1.Text, lblTax2.Text, lblTax3.Text, lblTotalQty.Text, lblGross.Text, lblNetAmout.Text, lblSpecialDiscount.Text, lblVat.Text, txtDueDays.Text, SalesDueDate, OF, m_dtRecurringSetting, dtReference);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_SalesInvoice.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liSalesLedgerID.ID), liSalesLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), LiCashPartyLedgerID.Value.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), OrderNo, txtVoucherNo.Text, SalesInvoice_Date, txtRemarks.Text, SalesInvoiceDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Sales_Tax1On, Global.Default_Sales_Tax1Check, Global.Default_Tax2, Global.Default_Sales_Tax2On, Global.Default_Sales_Tax2Check, Global.Default_Tax3, Global.Default_Sales_Tax3On, Global.Default_Sales_Tax3On, OldGrid, NewGrid, isNew, lblTax1.Text, lblTax2.Text, lblTax3.Text, lblTotalQty.Text, lblGross.Text, lblNetAmout.Text, lblSpecialDiscount.Text, lblVat.Text, txtDueDays.Text, SalesDueDate, OF, m_dtRecurringSetting, dtReference);
                        }

                        //Update AutoNumber in tblSeries only after save, if Voucher number type is hidden
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }
                        //PostApiBill();
                        Global.Msg("Sales Invoice created successfully!");

                        // if the voucher is recurring and has been posted or saved, modify voucherposting table to set isPosted = true
                        string res;
                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_SalesInvoiceID, "SALES_INVOICE");
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID);
                            m_isRecurring = false;
                        }
                        //if checkbox for print while saving is checked
                        if (checkBox2.Checked)
                        {
                            prntDirectForPOS = 1;
                            prntDirect = 1;
                            Navigation(Navigate.Last);
                            btnPrint_Click(sender, e);
                            //ClearVoucher();
                            //ChangeState(EntryMode.NEW);
                            //btnNew_Click(sender, e);
                        }
                        //if AccClassID is not cleared then it will doubled if the form is not reloaded by closing .
                        AccClassID.Clear();
                        ClearVoucher();
                        ChangeState(EntryMode.NEW);
                        btnNew_Click(sender, e);



                        //Do not close the form if do not close is checked
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
                    try
                    {
                        isNew = false;
                        NewGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + "-" + txtVoucherNo.Text + "," + "Voucher Date" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Date" + "-" + txtDueDate.Text + "," + "Cash/Party" + "-" + cboCashParty.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string productname = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductName].Value.ToString();
                            string qty = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Qty].Value.ToString();
                            string rate = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesRate].Value.ToString();
                            string amt = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Amount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(productname, qty, rate, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;

                        //Read from sourcegrid and store it to table
                        DataTable SalesInvoiceDetails = new DataTable();
                        SalesInvoiceDetails.Columns.Add("Code");
                        SalesInvoiceDetails.Columns.Add("Product");
                        SalesInvoiceDetails.Columns.Add("Quantity");
                        SalesInvoiceDetails.Columns.Add("SalesRate");
                        SalesInvoiceDetails.Columns.Add("Amount");
                        SalesInvoiceDetails.Columns.Add("DiscPercentage");
                        SalesInvoiceDetails.Columns.Add("Discount");
                        SalesInvoiceDetails.Columns.Add("NetAmount");
                        SalesInvoiceDetails.Columns.Add("ProductID");
                        SalesInvoiceDetails.Columns.Add("QtyUnitID");

                        for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            SalesInvoiceDetails.Rows.Add(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Code_No].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductName].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Default_Qty].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesRate].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Amount].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SplDisc].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.NetAmount].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductID].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Units].Value);
                        }

                        // add data of SalesInvoiceDetails to dtsalesInvoice which is used to  print the receipt
                        dtsalesInvoice = SalesInvoiceDetails;

                        DateTime SalesInvoice_Date = Date.ToDotNet(txtDate.Text);
                        ListItem liSalesLedgerID = new ListItem();
                        liSalesLedgerID = (ListItem)cmboSalesAcc.SelectedItem;

                        ListItem LiCashPartyLedgerID = new ListItem();
                        LiCashPartyLedgerID = (ListItem)cboCashParty.SelectedItem;

                        DateTime SalesDueDate = Date.ToDotNet(txtDueDate.Text);

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;

                        ListItem LiDepotID = new ListItem();
                        LiDepotID = (ListItem)cboDepot.SelectedItem;

                        int Tax1ID = AccountGroup.GetLedgerIDFromLedgerNumber(314);
                        int Tax2ID = AccountGroup.GetLedgerIDFromLedgerNumber(315);
                        int Tax3ID = AccountGroup.GetLedgerIDFromLedgerNumber(316);
                        int VatID = AccountGroup.GetLedgerIDFromLedgerNumber(412);

                        liProjectID = (ListItem)cboProjectName.SelectedItem;


                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        if (chkRecurring.Checked)
                        {
                            m_dtRecurringSetting.Rows[0]["RVID"] = RSID;  // send id of voucher setting for modification
                            m_dtRecurringSetting.Rows[0]["VoucherID"] = txtSalesInvoiceID.Text;
                        }
                        if (AccClassID.Count != 0)
                        {
                            m_SalesInvoice.Modify(Convert.ToInt32(txtSalesInvoiceID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liSalesLedgerID.ID), liSalesLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), LiCashPartyLedgerID.Value.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, SalesInvoice_Date, txtRemarks.Text, SalesInvoiceDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Sales_Tax1On, Global.Default_Sales_Tax1Check, Global.Default_Tax2, Global.Default_Sales_Tax2On, Global.Default_Sales_Tax2Check, Global.Default_Tax3, Global.Default_Sales_Tax3On, Global.Default_Sales_Tax3On, OldGrid, NewGrid, isNew, lblTax1.Text, lblTax2.Text, lblTax3.Text, lblTotalQty.Text, lblGross.Text, lblNetAmout.Text, lblSpecialDiscount.Text, lblVat.Text, txtDueDays.Text, SalesDueDate, OF, m_dtRecurringSetting, dtReference, ToDeleteRows);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_SalesInvoice.Modify(Convert.ToInt32(txtSalesInvoiceID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liSalesLedgerID.ID), liSalesLedgerID.Value.ToString(), Convert.ToInt32(LiCashPartyLedgerID.ID), LiCashPartyLedgerID.Value.ToString(), Tax1ID, Tax2ID, Tax3ID, VatID, Convert.ToInt32(LiDepotID.ID), txtOrderNo.Text, txtVoucherNo.Text, SalesInvoice_Date, txtRemarks.Text, SalesInvoiceDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), Global.Default_Tax1, Global.Default_Sales_Tax1On, Global.Default_Sales_Tax1Check, Global.Default_Tax2, Global.Default_Sales_Tax2On, Global.Default_Sales_Tax2Check, Global.Default_Tax3, Global.Default_Sales_Tax3On, Global.Default_Sales_Tax3On, OldGrid, NewGrid, isNew, lblTax1.Text, lblTax2.Text, lblTax3.Text, lblTotalQty.Text, lblGross.Text, lblNetAmout.Text, lblSpecialDiscount.Text, lblVat.Text, txtDueDays.Text, SalesDueDate, OF, m_dtRecurringSetting, dtReference, ToDeleteRows);
                        }
                        if (!Validate())
                            return;
                        Global.Msg("Sales Invoice modified successfully!");
                        //if checkbox for print while saving is checked
                        if (checkBox2.Checked)
                        {
                            prntDirectForPOS = 1;
                            prntDirect = 1;
                            Navigation((Navigate)Convert.ToInt32(txtSalesInvoiceID.Text.Trim()));
                            btnPrint_Click(sender, e);
                            //ClearVoucher();
                            //ChangeState(EntryMode.NEW);
                            //btnNew_Click(sender, e);
                        }
                        AccClassID.Clear();
                        ClearVoucher();
                        ChangeState(EntryMode.NEW);
                        btnNew_Click(sender, e);


                        lblAdjustment.Text = null;
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
            else if (cboCashParty.SelectedItem == null)
            {
                Global.MsgError("Invalid Cash Party Account Selected");
                cboCashParty.Focus();
                bValidate = false;
            }
            else if (cmboSalesAcc.SelectedItem == null)
            {
                Global.MsgError("Invalid Sales Account Selected");
                cmboSalesAcc.Focus();
                bValidate = false;
            }
            else if (txtVoucherNo.Text == " ")
            {
                Global.MsgError("Invalid Voucher no");
                txtVoucherNo.Focus();
                bValidate = false;
            }
            else if (!(grdSalesInvoice.Rows.Count > 2))
            {
                Global.MsgError("Invalid Account Ledger Selected in grid");
                grdSalesInvoice.Focus();
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
            chkRecurring.Enabled = btnSetup.Enabled = txtVoucherNo.Enabled = cboDepot.Enabled = cmboSalesAcc.Enabled = cboSeriesName.Enabled
                = cboCashParty.Enabled = txtRemarks.Enabled = txtDate.Enabled = txtOrderNo.Enabled = grdSalesInvoice.Enabled
                = cboSeriesName.Enabled = cboDepot.Enabled = treeAccClass.Enabled = btnAddAccClass.Enabled = tabControl1.Enabled = txtDueDays.Enabled
                = txtProductPin.Enabled = btnDate.Enabled = btnDueDate.Enabled = cboProjectName.Enabled = btnCashParty.Enabled = txtDueDate.Enabled
                = Enable;
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
                            MessageBox.Show("Your voucher numbers has already been finished!");
                            // disables all controls except cboSeriesName
                            ClearSalesInvoice(true);
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
                    if (m_SalesInvoiceID > 0 && !m_isRecurring)
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

            //Check if the date is in frozen range
            if (Settings.isFrozen(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("The transaction is Frozen. You cannot edit anything when the date falls in frozen range");
                return;
            }

            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            isNew = false;
            OldGrid = " ";
            OldGrid = OldGrid + "Voucher No" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Cash/Party" + "-" + cboCashParty.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string productname = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductName].Value.ToString() + ",";
                string qty = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Qty].Value.ToString() + ",";
                string rate = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesRate].Value.ToString() + ",";
                string amt = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Amount].Value.ToString() + ",";
                OldGrid = OldGrid + string.Concat(productname, qty, rate, amt) + ",";
            }
            AddAdjustmentRow();
            OldGrid = "OldGridValues" + OldGrid;
            EnableControls(true);
            ChangeState(EntryMode.EDIT);

            //if automatic voucher number increment is selected
            string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
            if (NumberingType == "AUTOMATIC")
                txtVoucherNo.Enabled = false;
        }

        private void ClearVoucher()
        {
            m_isRecurring = false;
            m_RVID = 0;
            ClearSalesInvoice(false);
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdSalesInvoice.Redim(1, 17);
            grdAdjustment.Redim(1, 7);
            AddGridHeader(); //Write header part
            AddAdjustmentGridHeader();
            AddRowProduct1(1);
            AddAdjustmentRow();
            ClearRecurringSetting();
            dtReference.Rows.Clear();
            AddReferenceColumns();
            m_refAmt = "0(Dr)";
            m_SalesInvoiceID = 0;
            txtSalesInvoiceID.Text = "0";
        }


        private void ClearSalesInvoice(bool IsVoucherNumFinished)
        {
            txtVoucherNo.Clear();
            cmboSalesAcc.Text = string.Empty;
            //cboSeriesName.Text = string.Empty;
            cboDepot.Text = string.Empty;
            //cboCashParty.Text = string.Empty;
            cboCashParty.SelectedIndex = 0;
            cmboSalesAcc.Text = string.Empty;


            //actually generate a new voucher no.
            // txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtOrderNo.Clear();
            txtRemarks.Clear();
            lblGrandTotal.Text = "0.00";
            lblVat.Text = "0.00";
            lblTax1.Text = "0.00";
            lblTax1.Text = "0.00";
            lblTax1.Text = "0.00";
            lblGross.Text = "0.00";
            lblSpecialDiscount.Text = "0.00";
            lblTotalQty.Text = "0.00";
            lblNetAmout.Text = "0.00";

            lblDiscountAmount.Text = "0.00";
            lblDiscount.Text = "0.00";

            if (IsVoucherNumFinished == false)
            {
                grdSalesInvoice.Rows.Clear();
            }
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

            //Insert the tag on the selected node to carry AccClassID
        }

        private bool Navigation(Navigate NavTo)
        {
            try
            {
                if (txtSalesInvoiceID.Text != "")
                    dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(txtSalesInvoiceID.Text), "SALES");

                ChangeState(EntryMode.NORMAL);
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtSalesInvoiceID.Text);
                    if (SalesInvoiceIDCopy > 0)
                    {
                        VouchID = SalesInvoiceIDCopy;
                        SalesInvoiceIDCopy = 0;
                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtSalesInvoiceID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
                DataTable dtSalesInvoiceMaster = new DataTable();
                //When we have to show records according to OrderNo
                //NavigateSalesInvoiceMaster
                //dtSalesInvoiceMaster = m_SalesInvoice.GetSalesInvoiceMasterInfoByOrderNo(txtOrderNo.Text);
                dtSalesInvoiceMaster = m_SalesInvoice.NavigateSalesInvoiceMaster(VouchID, NavTo);


                if (dtSalesInvoiceMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }
                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                DataRow drSalesInvoiceMaster = dtSalesInvoiceMaster.Rows[0]; //There is only one row. First row is the required record        

                //Show the corresponding Cash/Party Account Ledger in Combobox
                DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesInvoiceMaster["CashPartyLedgerID"]), LangMgr.DefaultLanguage);
                //DataRow drCashPartyLedgerInfo = dtCashPartyInfo.Rows[0];
                foreach (DataRow drCashPartyLedgerInfo in dtCashPartyInfo.Rows)
                {
                    cboCashParty.Text = drCashPartyLedgerInfo["LedName"].ToString();
                }

                //Show the corresponding Sales Account Ledger in Combobox
                DataTable dtSalesLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesInvoiceMaster["SalesLedgerID"]), LangMgr.DefaultLanguage);
                //DataRow drSalesLedgerInfo = dtSalesLedgerInfo.Rows[0];
                foreach (DataRow drSalesLedgerInfo in dtSalesLedgerInfo.Rows)
                {
                    cmboSalesAcc.Text = drSalesLedgerInfo["LedName"].ToString();
                }

                //show the corresoponding SeriesName in Combobox
                DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drSalesInvoiceMaster["SeriesID"]));
                if (dtSeriesInfo.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Sales Invoice");
                    cboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dtSeriesInfo.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();
                }

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

                        txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();
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

                        txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();
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

                        txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();
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

                        txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();

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

                        txtfirst.Text = drSalesInvoiceMaster["Field1"].ToString();
                        txtsecond.Text = drSalesInvoiceMaster["Field2"].ToString();
                        txtthird.Text = drSalesInvoiceMaster["Field3"].ToString();
                        txtfourth.Text = drSalesInvoiceMaster["Field4"].ToString();
                        txtfifth.Text = drSalesInvoiceMaster["Field5"].ToString();
                    }

                }

                //Getting DepotInfo
                DataTable dtDepotInfo = Depot.GetDepotInfo(Convert.ToInt32(drSalesInvoiceMaster["DepotID"]));
                DataRow drDepotInfo = dtDepotInfo.Rows[0];
                cboDepot.Text = drDepotInfo["DepotName"].ToString();

                double dnetAmount = 0, dtax1 = 0, dtax2 = 0, dtax3 = 0, dvat = 0, ddiscount = 0, dgrossTotal = 0, dgrandTotal = 0;
                lblVouNo.Visible = true;
                txtVoucherNo.Visible = true;
                txtVoucherNo.Text = drSalesInvoiceMaster["Voucher_No"].ToString();
                txtDueDays.Text = drSalesInvoiceMaster["DueDays"].ToString();
                txtOrderNo.Text = drSalesInvoiceMaster["OrderNo"].ToString();

                dnetAmount = Convert.ToDouble(drSalesInvoiceMaster["Net_Amount"].ToString());
                lblNetAmout.Text = dnetAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                dtax1 = Convert.ToDouble(drSalesInvoiceMaster["Tax1"].ToString());
                lblTax1.Text = dtax1.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                dtax2 = Convert.ToDouble(drSalesInvoiceMaster["Tax2"].ToString());
                lblTax2.Text = dtax2.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                dtax3 = Convert.ToDouble(drSalesInvoiceMaster["Tax3"].ToString());
                lblTax3.Text = dtax3.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                dvat = Convert.ToDouble(drSalesInvoiceMaster["VAT"].ToString());
                lblVat.Text = dvat.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                lblTotalQty.Text = drSalesInvoiceMaster["TotalQty"].ToString();

                ddiscount = Convert.ToDouble(drSalesInvoiceMaster["SpecialDiscount"].ToString());
                lblSpecialDiscount.Text = ddiscount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                lblDiscountAmount.Text = ddiscount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                dgrossTotal = Convert.ToDouble(drSalesInvoiceMaster["Gross_Amount"].ToString());
                lblGross.Text = dgrossTotal.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                double ddiscountPercent = 0;
                ddiscountPercent = (ddiscount / dgrossTotal) * 100;
                lblDiscount.Text = ddiscountPercent.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                dgrandTotal = dnetAmount + dtax1 + dtax2 + dtax3 + dvat;
                lblGrandTotal.Text = dgrandTotal.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                txtDate.Text = Date.DBToSystem(drSalesInvoiceMaster["SalesInvoice_Date"].ToString());

                if (txtDueDays.Text == "0")
                {
                    txtDueDate.Text = txtDueDate.Text = Date.DBToSystem(drSalesInvoiceMaster["SalesInvoice_Date"].ToString());

                }

                else if (txtDueDate.Text != "0")
                {
                    txtDueDate.Text = Date.DBToSystem(drSalesInvoiceMaster["SalesDueDate"].ToString());

                }

                txtRemarks.Text = drSalesInvoiceMaster["Remarks"].ToString();
                txtSalesInvoiceID.Text = drSalesInvoiceMaster["SalesInvoiceID"].ToString();

                dsSalesInvoice.Tables["tblSalesInvoiceMaster"].Rows.Add(cboSeriesName.Text, drSalesInvoiceMaster["Voucher_No"].ToString(), Date.DBToSystem(drSalesInvoiceMaster["SalesInvoice_Date"].ToString()), cboCashParty.Text, txtOrderNo.Text, cboDepot.Text, cmboSalesAcc.Text, drSalesInvoiceMaster["Remarks"].ToString());

                DataTable dtSalesInvoiceDetails = m_SalesInvoice.GetSalesInvoiceDetails(Convert.ToInt32(txtSalesInvoiceID.Text));
                DataTable dTable = Product.GetAllProduct(null, 0, " ", DateTime.Today, true, StockStatusType.DiffInStock);

                for (int i = 1; i <= dtSalesInvoiceDetails.Rows.Count; i++)
                {
                    DataRow drDetail = dtSalesInvoiceDetails.Rows[i - 1];

                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SNo].Value = i.ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Code_No].Value = drDetail["Code"].ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["CurUnitQty"])).ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesRate].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["SalesRate"])).ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    double discPer = 0;
                    discPer = Convert.ToDouble(drDetail["DiscPercentage"]);
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value = discPer.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.NetAmount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString();
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductID].Value = productID = Convert.ToInt32(drDetail["ProductID"]);

                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Default_Qty].Value = Convert.ToInt32(drDetail["Quantity"]);
                    grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Default_Unit].Value = Convert.ToInt32(drDetail["UnitID"]);
                    LoadCompoundUnit(i);

                    SourceGrid.CellContext context = new SourceGrid.CellContext(grdSalesInvoice, new SourceGrid.Position(i, (int)SalesInvoiceGridColumn.Units));
                    int index = Array.IndexOf(arrInt, Convert.ToInt32(drDetail["QtyUnitID"]));
                    context.Cell.Editor.SetCellValue(context, arrStr[index]);

                    //grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Units].Value = ;//Convert.ToInt32(drDetail["UnitName"]);

                    foreach (DataRow dr in dTable.Select("[productID] = " + drDetail["ProductID"] + ""))
                    {
                        grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Stock_Qty_Actual].Value = Convert.ToInt32(dr["ClosingStock"]) + Convert.ToInt32(drDetail["Quantity"]);
                        grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Stock_Qty].Value = Convert.ToInt32(dr["ClosingStock"]);
                    }
                    AddRowProduct1(grdSalesInvoice.RowsCount);
                    dsSalesInvoice.Tables["tblSalesInvoiceDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(), drDetail["Quantity"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["SalesRate"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());
                }
                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtSalesInvoiceID.Text), "SALES");
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

                CheckRecurringSetting(txtSalesInvoiceID.Text);

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
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Sales Invoice?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}
            if (!ContinueWithoutSaving())
            {
                return;
            }
            IsFieldChanged = false;
            Navigation(Navigate.First);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }



            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Sales Invoice?") == DialogResult.Yes)
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
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Sales Invoice?") == DialogResult.Yes)
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
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
            IsFieldChanged = false;
            AddAdjustmentRow();
            //cboSeriesName.SelectedIndex = 0;
            cboSeriesName_SelectedIndexChanged(sender, e);
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            //frmAccountClass frm = new frmAccountClass(this);
            //frm.Show();
            m_MDIForm.OpenFormArrayParam("frmAccClass");
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            SalesInvoiceIDCopy = Convert.ToInt32(txtSalesInvoiceID.Text);
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

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            prntDirectForPOS = 1;
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }

            //#region Reference related
            //// if new voucher reference is created for this, donot open the reference form 
            //if (m_mode == EntryMode.NORMAL && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(txtSalesInvoiceID.Text), CurrAccLedgerID, "SALES"))
            //{
            //    //Global.MsgError("You must delete all other vouchers with reference against this voucher to delete this transaction!");
            //    isNewReferenceVoucher = true;
            //    return;
            //}
            //isNewReferenceVoucher = false;
            //#endregion

            bool chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            string GridValues = "";
            GridValues = GridValues + "Voucher No" + "-" + txtVoucherNo.Text + "," + "Voucher Date" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Cash/Party" + "-" + cboCashParty.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string productname = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductName].Value.ToString();
                string qty = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Qty].Value.ToString();
                string rate = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesRate].Value.ToString();
                string amt = grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Amount].Value.ToString();
                GridValues = GridValues + string.Concat(productname, qty, rate, amt);
            }
            GridValues = "GridValues" + GridValues;
            ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            try
            {
                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the Invoice - " + txtSalesInvoiceID.Text + "?") == DialogResult.Yes)
                {
                    Sales DelSales = new Sales();
                    // delete reference
                    string res = VoucherReference.DeleteReference(Convert.ToInt32(txtSalesInvoiceID.Text), "SALES");
                    if (res != "Success")
                    {
                        Global.MsgError("Unable to delete the voucher due to " + res + "! \n You must delete all other vouchers with reference against this voucher to delete this transaction!");
                        return;
                    }

                    if (DelSales.RemoveSalesEntry(Convert.ToInt32(txtSalesInvoiceID.Text), GridValues))
                    {
                        RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "SALES_INVOICE"); // deleting the recurring setting if voucher is deleted

                        Global.Msg("Invoice -" + txtSalesInvoiceID.Text + " deleted successfully!");
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
                        Global.MsgError("There was an error while deleting invoice -" + txtSalesInvoiceID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void frmSalesInvoice_KeyDown(object sender, KeyEventArgs e)
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
                //btnPrev_Click(sender, e);
            }
            else if (e.KeyCode == Keys.N && e.Shift)
            {
                //btnNext_Click(sender, e);
            }
            else if (e.KeyCode == Keys.L && e.Control)
            {
                btnLast_Click(sender, e);
            }
            //else if (e.KeyCode == Keys.C && e.Control)
            //{
            //    btnCopy_Click(sender, e);
            //}
            //else if (e.KeyCode == Keys.V && e.Control)
            //{
            //    btnPaste_Click(sender, e);
            //}
            else if (e.KeyCode == Keys.P && e.Control)
            {
                btnPrint_Click(sender, e);
            }
        }

        /// <summary>
        /// Adds a row in the grid
        /// </summary>
        /// <param name="RowCount"></param>

        private void AddRowProduct(int RowCount)
        {

            //Add a new row
            try
            {

                grdSalesInvoice.Redim(Convert.ToInt32(RowCount + 1), grdSalesInvoice.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Del] = btnDelete;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Del].AddController(evtDelete);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Code_No] = new SourceGrid.Cells.Cell();

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
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName] = new SourceGrid.Cells.Cell("", txtProduct);
                txtProduct.Control.GotFocus += new EventHandler(Product_Focused);
                txtProduct.Control.LostFocus += new EventHandler(Product_Focus_Lost);
                txtProduct.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                txtProduct.Control.TextChanged += new EventHandler(Text_Change);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName].AddController(evtProductFocusLost);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty] = new SourceGrid.Cells.Cell("1", txtQty);
                txtQty.Control.TextChanged += new EventHandler(Text_Change);
                //grdPurchaseInvoice[i, 4].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(SalesRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                txtPurchaseRate.Control.TextChanged += new EventHandler(Text_Change);
                // grdPurchaseInvoice[i, 5].Value = "(NEW)";

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount] = new SourceGrid.Cells.Cell();
                // grdPurchaseInvoice[i, 6].Value = "(NEW)";
                SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);
                txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                txtDiscPercentage.Control.Text = "0";

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc_Percent] = new SourceGrid.Cells.Cell("", txtDiscPercentage);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value = "0";
                SourceGrid.Cells.Editors.TextBox txtDiscount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscount.Control.LostFocus += new EventHandler(Discount_Modified);
                txtDiscount.EditableMode = SourceGrid.EditableMode.Focus;

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc] = new SourceGrid.Cells.Cell("0", txtDiscount);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc].Value = "0";

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.NetAmount] = new SourceGrid.Cells.Cell("Net Amount");
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.NetAmount].Value = "0";

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesOrderDetailID] = new SourceGrid.Cells.Cell("");
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Value = "0";

                //IsSelected = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void Product_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            int RowsCount = grdSalesInvoice.RowsCount;
            string LastServicesCell = (string)grdSalesInvoice[RowsCount - 1, (int)SalesInvoiceGridColumn.ProductName].Value;
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

        private void AddRowProduct1(int RowCount)
        {

            //Add a new row
            try
            {
                RowView = new SourceGrid.Cells.Views.Cell();
                if (grdSalesInvoice.Rows.Count % 2 == 0)
                {
                    RowView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    RowView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }

                grdSalesInvoice.Redim(Convert.ToInt32(RowCount + 1), grdSalesInvoice.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Del] = btnDelete;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Del].AddController(evtDelete);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SNo].View = new SourceGrid.Cells.Views.Cell(RowView);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Code_No] = new SourceGrid.Cells.Cell();
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Code_No].View = new SourceGrid.Cells.Views.Cell(RowView);

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
                grdSalesInvoice[i, 3] = new SourceGrid.Cells.Cell("", txtProduct);
                txtProduct.Control.GotFocus += new EventHandler(Product_Focused);
                txtProduct.Control.LostFocus += new EventHandler(Product_Leave);
                txtProduct.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                txtProduct.Control.TextChanged += new EventHandler(Text_Change);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName].AddController(evtProductFocusLost);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName].Value = "(NEW)";
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName].View = new SourceGrid.Cells.Views.Cell(RowView);

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty] = new SourceGrid.Cells.Cell("1", txtQty);
                txtQty.Control.TextChanged += new EventHandler(Text_Change);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty].View = new SourceGrid.Cells.Views.Cell(RowView);
                //grdPurchaseInvoice[i, 4].Value = "(NEW)";

                #region related to compound unit
                //GetListForGrdUnit();
                //SourceGrid.Cells.Editors.ComboBox cboUnits = new SourceGrid.Cells.Editors.ComboBox(typeof(int), arrInt, true);
                //cboUnits.Control.FormattingEnabled = true;
                //cboUnits.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                //cboUnits.Control.AutoCompleteMode = AutoCompleteMode.Append;
                //ValueMapping comboMapping = new ValueMapping();
                //comboMapping.DisplayStringList = arrStr;
                //comboMapping.ValueList = arrInt;
                //comboMapping.SpecialList = arrStr;
                //comboMapping.SpecialType = typeof(string);
                //comboMapping.BindValidator(cboUnits);
                //grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Units] = new SourceGrid.Cells.Cell();
                //grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Units].Editor = cboUnits;

                //SourceGrid.CellContext context = new SourceGrid.CellContext(grdSalesInvoice, new SourceGrid.Position(i, (int)SalesInvoiceGridColumn.Units));
                //context.Cell.Editor.SetCellValue(context, arrStr[0]);
                LoadCompoundUnit(i);

                #endregion

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Default_Qty] = new SourceGrid.Cells.Cell("");
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Default_Qty].Value = "0";

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Default_Unit] = new SourceGrid.Cells.Cell("");
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Default_Unit].Value = "0";

                SourceGrid.Cells.Editors.TextBox txtStockQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtStockQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtStockQty.EditableMode = SourceGrid.EditableMode.None; //SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Stock_Qty] = new SourceGrid.Cells.Cell("0", txtStockQty);
                txtStockQty.Control.TextChanged += new EventHandler(Text_Change);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Stock_Qty].View = new SourceGrid.Cells.Views.Cell(RowView);

                SourceGrid.Cells.Editors.TextBox txtStockQtyActual = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtStockQtyActual.Control.LostFocus += new EventHandler(Qty_Modified);
                txtStockQtyActual.EditableMode = SourceGrid.EditableMode.None; //SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Stock_Qty_Actual] = new SourceGrid.Cells.Cell("0", txtStockQtyActual);
                txtStockQtyActual.Control.TextChanged += new EventHandler(Text_Change);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Stock_Qty_Actual].View = new SourceGrid.Cells.Views.Cell(RowView);

                SourceGrid.Cells.Editors.TextBox txtSalesRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtSalesRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                txtSalesRate.EditableMode = SourceGrid.EditableMode.Focus;

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesRate] = new SourceGrid.Cells.Cell("", txtSalesRate);
                txtSalesRate.Control.TextChanged += new EventHandler(Text_Change);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesRate].View = new SourceGrid.Cells.Views.Cell(RowView);

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount] = new SourceGrid.Cells.Cell();
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Amount].View = new SourceGrid.Cells.Views.Cell(RowView);
                // grdPurchaseInvoice[i, 6].Value = "(NEW)";
                SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);
                //txtDiscPercentage.Control.TextChanged += new EventHandler(Text_Change);
                txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                txtDiscPercentage.Control.Text = "0";
                //bool test = IsFieldChanged;
                //MessageBox.Show(test.ToString());
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc_Percent] = new SourceGrid.Cells.Cell("", txtDiscPercentage);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value = "0";
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc_Percent].View = new SourceGrid.Cells.Views.Cell(RowView);

                SourceGrid.Cells.Editors.TextBox txtDiscount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtDiscount.Control.LostFocus += new EventHandler(Discount_Modified);
                txtDiscount.EditableMode = SourceGrid.EditableMode.Focus;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc] = new SourceGrid.Cells.Cell("0", txtDiscount);
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc].Value = "0";
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SplDisc].View = new SourceGrid.Cells.Views.Cell(RowView);

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.NetAmount] = new SourceGrid.Cells.Cell("Net Amount");
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.NetAmount].Value = "0";
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.NetAmount].View = new SourceGrid.Cells.Views.Cell(RowView);

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesOrderDetailID] = new SourceGrid.Cells.Cell("");
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Value = "0";

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductID] = new SourceGrid.Cells.Cell("");
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductID].Value = "0";


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

        private void Product_Focus_Lost(object sender, EventArgs e)
        {
            hasChanged = false;
        }

        private void Product_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.N)
            {
                txtRemarks.Focus();
            }
            else
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                frmListOfProduct flp = new frmListOfProduct(this, e);
                flp.ShowDialog();
            }

        }

        private void Text_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;

        }

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdSalesInvoice.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            //Check if the input value is correct
            if (grdSalesInvoice[Convert.ToInt32(CurRow), (int)SalesInvoiceGridColumn.Amount].Value == "" || grdSalesInvoice[Convert.ToInt32(CurRow), (int)SalesInvoiceGridColumn.Amount].Value == null)
            {
                grdSalesInvoice[Convert.ToInt32(CurRow), (int)SalesInvoiceGridColumn.Amount].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            }

            double checkformat = Convert.ToDouble(grdSalesInvoice[Convert.ToInt32(CurRow), (int)SalesInvoiceGridColumn.Amount].Value.ToString());
            string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdSalesInvoice[Convert.ToInt32(CurRow), (int)SalesInvoiceGridColumn.Amount].Value = insertvalue;


        }

        public void AddProduct(int productid, string productcode, string productname, bool IsSelected, double purchaserate, double salesrate, int qty, int defaultUnitID)
        {
            //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            // CurrRowPos = ctx.Position.Row;
            int stockQty = 0;
            if (IsSelected)
            {
                int CurRow = grdSalesInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
                // CurrRowPos = ctx.Position.Row;
                ctx.Text = productname;
                purchaserate = salesrate;

                //for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                //{
                //    if (Convert.ToInt32(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductID].Value) == productid)
                //    {
                //        stockQty += Convert.ToInt32(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Qty].Value);
                //    }
                //}
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Default_Unit].Value = defaultUnitID;

                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Code_No].Value = productcode;
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SalesRate].Value = purchaserate;
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.ProductID].Value = productid;

                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Stock_Qty].Value = qty - stockQty;
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Stock_Qty_Actual].Value = qty - stockQty;

                int RowsCount = grdSalesInvoice.RowsCount;
                string LastServicesCell = (string)grdSalesInvoice[RowsCount - 1, (int)SalesInvoiceGridColumn.ProductName].Value;

                ////Check whether the new row is already added
                if (LastServicesCell != "(NEW)" || (CurRow + 1 == RowsCount))
                {
                    AddRowProduct1(RowsCount);
                }

            }
            hasChanged = true;
        }

        public int CalculateStockQty(int productID)
        {
            int stockQty = 0;

            for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                if (Convert.ToInt32(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductID].Value) == productID)
                {
                    stockQty += Convert.ToInt32(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Qty].Value);
                }
            }
            return stockQty;


        }
        private void PurchaseRate_Modified(object sender, EventArgs e)
        {
            try
            {
                //find the current row of source grid
                int CurRow = grdSalesInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;
                int RowCount = grdSalesInvoice.RowsCount;

                object PurchaseRate = ((TextBox)sender).Text;
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SalesRate].Value = PurchaseRate.ToString();

                bool IsDouble = Misc.IsNumeric(PurchaseRate);//This function check whether variable is Double  or not?

                if (IsDouble == false)
                {
                    Global.MsgError("The Purchase Rate you posted is invalid! Please post the integer value");
                    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value = "";
                    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value = "1";
                    return;
                }
                grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value = (Convert.ToDecimal(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value) * Convert.ToDecimal(PurchaseRate)).ToString();
                Unit_Changed(sender, e);
                //string Qty = grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value.ToString();
                //double Amount = Convert.ToDouble(Qty) * Convert.ToDouble(PurchaseRate);

                //grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value = Amount.ToString();

                //CalculateGrossAmount();
                //WriteNetAmount(CurRow);
                //CalculateNetAmount();
                //CalculateTotalQuantity();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmSalesInvoice_FormClosed(object sender, FormClosedEventArgs e)
        {
            Global.product = false;
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            isForDueDate = false;
            Common.frmDateConverter frm = new Common.frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }

        public void DateConvert(DateTime DotNetDate)
        {
            if (isForDueDate == false)
            {
                txtDate.Text = Date.ToSystem(DotNetDate);
            }
            else
            {
                txtDueDate.Text = Date.ToSystem(DotNetDate);
                DateTime TodaysDate = System.Convert.ToDateTime(Date.GetServerDate());
                TimeSpan servicePeriod = System.Convert.ToDateTime(Date.ToDotNet(txtDueDate.Text)) - TodaysDate;
                double Period = servicePeriod.TotalDays;
                txtDueDays.Text = Period.ToString();
            }
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
            frmtenderamount ft = new frmtenderamount(totalamount);
            ft.ShowDialog();
            btnSave.Focus();
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

        private int GetRootAccClassID()
        {
            if (UserAccClass > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(UserAccClass));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }
            return 1;//The default root class ID
        }

        private string ReadAllAccClassID()
        {
            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            //foreach (string tag in arrNode)
            //{
            //    AccClassID.Add(Convert.ToInt32(tag));
            //}

            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in arrNode)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            AccClassIDs.AddRange(arrChildAccClassIDs);

            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("STOCKSTATUS");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("AccClassIDSettings");
                    foreach (string tag in arrNode)
                    {
                        AccClassIDs.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccClassID", Convert.ToInt32(tag).ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            return strXML;
        }

        private string ReadAllProjectID()
        {
            #region  ProjectID
            liProjectID = (ListItem)cboProjectName.SelectedItem;
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(liProjectID.ID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + liProjectID.ID + "'";

            for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
            {
                ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
            }
            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            //tw.WriteStartElement("INVENTORYBOOK");
            tw.WriteStartElement("STOCKSTATUS");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ProjectIDSettings");
                    tw.WriteElementString("ProjectID", Convert.ToInt32(liProjectID.ID).ToString());
                    foreach (string tag in ProjectIDCollection)
                    {
                        //AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("ProjectID", Convert.ToInt32(tag).ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            return strXML;
        }

        private void txtproductpin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtproductpin_TextChanged(sender, e);
            }
        }

        private void RefreshDataGrid(object sender, KeyEventArgs e)
        {
            int i = 0;
            int j = 0;
            // grdSalesInvoice.Rows.Clear();

            if (orderTab.Rows.Count > 0)
            {
                i = grdSalesInvoice.Rows.Count - 1;
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductName].Value = orderTab.Rows[j]["item"].ToString();
                //grdSalesInvoice[i + 1, 4].Value = orderTab.Rows[i]["qty"].ToString();
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.SalesRate].Value =Convert.ToDecimal(orderTab.Rows[j]["price"]).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Code_No].Value = orderTab.Rows[j]["code"].ToString();
                //grdSalesInvoice[i + 1, 6].Value = orderTab.Rows[i]["price"].ToString();
                //grdSalesInvoice[i + 1, 9].Value = orderTab.Rows[i]["price"].ToString();
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.ProductID].Value = orderTab.Rows[j]["id"].ToString();
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Default_Unit].Value = orderTab.Rows[j]["unitID"].ToString();
                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Stock_Qty].Value = (Convert.ToDecimal(orderTab.Rows[j]["stockQty"]) 
                                    - Convert.ToDecimal(grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Qty].Value)).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                grdSalesInvoice[i, (int)SalesInvoiceGridColumn.Stock_Qty_Actual].Value = orderTab.Rows[j]["stockQty"].ToString();

                productID = Convert.ToInt32(orderTab.Rows[j]["id"]);
                LoadCompoundUnit(i);
                //calculations after a product is added.
                WriteAmount(i, Convert.ToDouble(1));


                //Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
                WriteNetAmount(i);

            }
            orderTab.Rows.Clear();


        }

        private void initOrderTable()
        {
            orderTab.Columns.Clear();
            orderTab.Columns.Add("id");
            orderTab.Columns.Add("qty");
            orderTab.Columns.Add("item");
            orderTab.Columns.Add("code");
            orderTab.Columns.Add("cost");
            orderTab.Columns.Add("price");
            orderTab.Columns.Add("value");
            orderTab.Columns.Add("unitID");
            orderTab.Columns.Add("stockQty");

        }

        private void cboCashParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            
         //ListItem liParty = new ListItem();
            liParty = (ListItem)cboCashParty.SelectedItem;
            int PartyID = liParty.ID;

            try
            {
                SetPatientDetail(PartyID);
            }
            catch (Exception ex)
            {
                //ex.Message();
            }

            DataTable dt = Ledger.GetAllLedger1(29); //Get All Ledgers with group debtors
            foreach (DataRow drdebtors in dt.Rows)
            {
                if (drdebtors["LedgerID"].ToString() == PartyID.ToString())
                {
                    txtDueDays.Enabled = true;
                    txtDueDays.Text = 0.ToString();
                    txtDueDate.Mask = Date.FormatToMask();
                    txtDueDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
                    iSDueDate = true;
                    txtDueDays.Visible = true;
                    txtDueDate.Visible = true;
                    label16.Visible = true;
                    label17.Visible = true;
                    btnDueDate.Visible = true;
                    break;
                }
                else
                {
                    txtDueDays.Enabled = false;
                    txtDueDays.Text = "";
                    txtDueDate.Text = "";
                    iSDueDate = false;
                    txtDueDays.Visible = false;
                    txtDueDate.Visible = false;
                    label16.Visible = false;
                    label17.Visible = false;
                    btnDueDate.Visible = false;
                }
            }
        }

        private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void btnduedate_Click(object sender, EventArgs e)
        {
            isForDueDate = true;
            if (iSDueDate == true)
            {
                Common.frmDateConverter frm = new Common.frmDateConverter(this, Date.ToDotNet(txtDueDate.Text));
                frm.ShowDialog();
            }
        }

        private void txtduedays_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtDueDays.Text.Trim()))
                {
                    DateTime TodaysDate = Date.GetServerDate();
                    txtDueDate.Text = Date.ToSystem(TodaysDate.AddDays(Convert.ToDouble(txtDueDays.Text))).ToString();
                }
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
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
                    btnPrint_Click(sender, e);
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
                    btnPrint_Click(sender, e);
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
                    btnPrint_Click(sender, e);
                    break;
            }
        }

        private void btncashparty_Click(object sender, EventArgs e)
        {
            //frmAccountSetup frm = new frmAccountSetup(this);
            //frm.Show();
            m_MDIForm.OpenFormArrayParam("frmAccSetUp");

        }

        public void AddAccountLedger(string ledgerName)
        {
            try
            {
                cboCashParty.Items.Clear();
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
            prntDirectForPOS = 0;
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void PrintPreviewCR(PrintType myPrintType)
        {
            try
            {
                if (Global.Default_Sales_Report_Type == "POS")
                {
                    dtsalesInvoice = new DataTable();

                    dtsalesInvoice.Columns.Add("Code");
                    dtsalesInvoice.Columns.Add("Product");
                    dtsalesInvoice.Columns.Add("Quantity");
                    dtsalesInvoice.Columns.Add("SalesRate");
                    dtsalesInvoice.Columns.Add("Amount");
                    dtsalesInvoice.Columns.Add("DiscPercentage");
                    dtsalesInvoice.Columns.Add("Discount");
                    dtsalesInvoice.Columns.Add("NetAmount");
                    dtsalesInvoice.Columns.Add("SalesOrderDetailsID");
                    dtsalesInvoice.Columns.Add("ProductID");

                    for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                    {
                        //dtsalesInvoice.
                        dtsalesInvoice.Rows.Add(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Code_No].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductName].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Qty].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesRate].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Amount].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SplDisc].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.NetAmount].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductID].Value);
                    }
                    PrintReceipt();
                }
                else if (Global.Default_Sales_Report_Type == "Hospital")
                {
                    dtsalesInvoice = new DataTable();

                    dtsalesInvoice.Columns.Add("Code");
                    dtsalesInvoice.Columns.Add("Product");
                    dtsalesInvoice.Columns.Add("Quantity");
                    dtsalesInvoice.Columns.Add("SalesRate");
                    dtsalesInvoice.Columns.Add("Amount");
                    dtsalesInvoice.Columns.Add("DiscPercentage");
                    dtsalesInvoice.Columns.Add("Discount");
                    dtsalesInvoice.Columns.Add("NetAmount");
                    dtsalesInvoice.Columns.Add("SalesOrderDetailsID");
                    dtsalesInvoice.Columns.Add("ProductID");

                    for (int i = 0; i < grdSalesInvoice.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                    {
                        //dtsalesInvoice.
                        dtsalesInvoice.Rows.Add(grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Code_No].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductName].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Qty].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesRate].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.Amount].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SplDisc_Percent].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SplDisc].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.NetAmount].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.SalesOrderDetailID].Value, grdSalesInvoice[i + 1, (int)SalesInvoiceGridColumn.ProductID].Value);
                    }
                    PrintHospitalReceipt();
                }
                else
                {
                    dsSalesInvoice.Clear();
                    rptSalesInvoice rpt = new rptSalesInvoice();
                    Misc.WriteLogo(dsSalesInvoice, "tblImage");
                    rpt.SetDataSource(dsSalesInvoice);

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

                    //Parameters for Total part
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvGrossTotal = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvDiscountAmt = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvNetTotal = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvVAT = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvGrandTotal = new CrystalDecisions.Shared.ParameterDiscreteValue();

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

                        #region Region for Total part
                        pdvGrossTotal.Value = lblGross.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvGrossTotal);
                        rpt.DataDefinition.ParameterFields["GrossTotal"].ApplyCurrentValues(pvCollection);

                        pdvDiscountAmt.Value = lblDiscountAmount.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvDiscountAmt);
                        rpt.DataDefinition.ParameterFields["DiscountAmt"].ApplyCurrentValues(pvCollection);

                        pdvNetTotal.Value = lblNetAmout.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvNetTotal);
                        rpt.DataDefinition.ParameterFields["NetTotal"].ApplyCurrentValues(pvCollection);

                        pdvVAT.Value = lblVat.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvVAT);
                        rpt.DataDefinition.ParameterFields["VAT"].ApplyCurrentValues(pvCollection);

                        pdvGrandTotal.Value = lblGrandTotal.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvGrandTotal);
                        rpt.DataDefinition.ParameterFields["GrandTotal"].ApplyCurrentValues(pvCollection);
                        #endregion
                    }
                    else //if user is not root, take information from tblUserPreference
                    {
                        try
                        {
                            string companyname = UserPreference.GetValue("COMPANY_NAME", uid);
                            pdvCompany_Name.Value = companyname;
                            string companyaddress = UserPreference.GetValue("COMPANY_ADDRESS", uid);
                            string companycity = UserPreference.GetValue("COMPANY_CITY", uid);
                            pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                            string companypan = UserPreference.GetValue("COMPANY_PAN", uid);
                            pdvCompany_PAN.Value = companypan;
                            string companyphone = UserPreference.GetValue("COMPANY_PHONE", uid);
                            pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                            string companyslogan = UserPreference.GetValue("COMPANY_SLOGAN", uid);
                            pdvCompany_Slogan.Value = companyslogan;
                        }
                        catch (Exception)
                        {
                        }

                        pvCollection.Clear();
                        pvCollection.Add(pdvCompany_Name);
                        rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);


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

                        #region Region for Total part
                        pdvGrossTotal.Value = lblGross.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvGrossTotal);
                        rpt.DataDefinition.ParameterFields["GrossTotal"].ApplyCurrentValues(pvCollection);

                        pdvDiscountAmt.Value = lblDiscountAmount.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvDiscountAmt);
                        rpt.DataDefinition.ParameterFields["DiscountAmt"].ApplyCurrentValues(pvCollection);

                        pdvNetTotal.Value = lblNetAmout.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvNetTotal);
                        rpt.DataDefinition.ParameterFields["NetTotal"].ApplyCurrentValues(pvCollection);

                        pdvVAT.Value = lblVat.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvVAT);
                        rpt.DataDefinition.ParameterFields["VAT"].ApplyCurrentValues(pvCollection);

                        pdvGrandTotal.Value = lblGrandTotal.Text;
                        pvCollection.Clear();
                        pvCollection.Add(pdvGrandTotal);
                        rpt.DataDefinition.ParameterFields["GrandTotal"].ApplyCurrentValues(pvCollection);
                        #endregion
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
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        #region methods related to POS (receipt) printing
        PrintDocument pdoc = null;
        DataTable dtsalesInvoice;
        int docHeight = 0;
        int docwidth = 285; // int docwidth = 230; //230 is up to amount value
        /// <summary>
        /// to dispay printer settings and print preview for sales invoice
        /// </summary>
        public void PrintReceipt()
        {
            try
            {
                docHeight = 340 + grdSalesInvoice.Rows.Count * 20;
                if (Global.Default_Sales_Report_Type == "POS")
                {
                    PrintDialog diag = new PrintDialog();
                    PrinterSettings ps = new PrinterSettings();
                    pdoc = new PrintDocument();

                    Font font = new Font("Courier New", 7);

                    System.Drawing.Printing.PaperSize psize = new System.Drawing.Printing.PaperSize("Custom", docwidth, docHeight);
                    diag.Document = pdoc;
                    diag.Document.DefaultPageSettings.PaperSize = psize;

                    pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);
                    if (prntDirectForPOS == 1)
                    {
                        pdoc.Print();
                    }
                    else
                    {
                        DialogResult res = diag.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            PrintPreviewDialog pp = new PrintPreviewDialog();
                            pp.Document = pdoc;
                            DialogResult result = pp.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                pdoc.Print();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void PrintHospitalReceipt()
        {
            try
            {
                docHeight = 340 + grdSalesInvoice.Rows.Count * 20;
                if (Global.Default_Sales_Report_Type == "Hospital")
                {
                    PrintDialog diag = new PrintDialog();
                    PrinterSettings ps = new PrinterSettings();
                    pdoc = new PrintDocument();

                    Font font = new Font("Courier New", 7);

                    System.Drawing.Printing.PaperSize psize = new System.Drawing.Printing.PaperSize("Custom", docwidth, docHeight);
                    diag.Document = pdoc;
                    diag.Document.DefaultPageSettings.PaperSize = psize;

                    pdoc.PrintPage += new PrintPageEventHandler(p_doc_PrintHospital);
                    if (prntDirectForPOS == 1)
                    {
                        pdoc.Print();
                    }
                    else
                    {
                        DialogResult res = diag.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            PrintPreviewDialog pp = new PrintPreviewDialog();
                            pp.Document = pdoc;
                            DialogResult result = pp.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                pdoc.Print();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        /// <summary>
        /// to add or draw values from invoice into the printable report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
          // if(m_PatientID != 0)
          // {
              // PrintPOSForHospital(e);
         //  }
         //  else
         //  {
             PrintPOS(e);
          // }
          
          
        }
        void p_doc_PrintHospital(object sender, PrintPageEventArgs e)
        {
            // if(m_PatientID != 0)
            // {
            PrintPOSForHospital(e);
            //  }
            //  else
            //  {
           // PrintPOS(e);
            // }


        }


        string m_PatientName = "";
        string m_Age = "";
        string m_sex = "";
        string m_Telephone = "";
        string m_Mobile = "";
        string m_Date = "";
        string m_Address = "";
        string m_City = "";
        int m_PatientID = 0;
        int m_ledgerId = 0;
        string m_DoctorName = "";

        public void SetPatientDetail(int ledgerID)
        {
            DataTable dtStudent = Patient.GetPatientDetail(ledgerID);
            if (dtStudent.Rows.Count > 0)
            {
                DataRow dr = dtStudent.Rows[0];
                m_PatientName = dr["PatientName"].ToString();
                m_Age = dr["Age"].ToString();
                int gender = Convert.ToInt32(dr["Sex"].ToString());
                if (gender == 1)
                {
                    m_sex = "Male";
                }
                else if (gender == 2)
                {
                    m_sex = "Female";
                }
                else
                {
                    m_sex = "Other";
                }
                m_Telephone = dr["Telephone"].ToString();
                m_Mobile = dr["Mobile"].ToString();
                m_Address = dr["Address"].ToString();
                m_PatientID = Convert.ToInt32(dr["PatientID"]);
                m_DoctorName = dr["DoctorName"].ToString();

            }

            else
            {
                ClearPatientDetail();
            }
        }
       
     
        
        public void PrintPOSForHospital(PrintPageEventArgs e)
        {
            // Create rectangle for drawing.
            float x = 0.0F;
            float y = 10.0F;
            float width = 285.0F;
            float height = 20.0F;
            Font font3 = new Font("Courier New", 9, FontStyle.Bold);
            Font font2 = new Font("Courier New", 8, FontStyle.Bold);

            

            // formatting the string for central alignment
            StringFormat stringCentralFormat = new StringFormat();
            stringCentralFormat.Alignment = StringAlignment.Center;
            stringCentralFormat.LineAlignment = StringAlignment.Center;

            //formating string for right alignment
            StringFormat stringRightFormat = new StringFormat();
            stringRightFormat.Alignment = StringAlignment.Far;
            stringRightFormat.LineAlignment = StringAlignment.Far;

            DataTable dtCompany = CompanyDetails.GetCompanyInfo();
            Graphics g = e.Graphics;
            Font font = new Font("Courier New", 8);
            float fontHeight = font.GetHeight();
            int startX = 15;
            int startY = 20;
            int offset = 20;
          
            // write comapny name
            g.DrawString(dtCompany.Rows[0]["Name"].ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(x, y, width, height), stringCentralFormat);
            //offset += 10;

            // write company address
            g.DrawString(dtCompany.Rows[0]["Address1"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width, height), stringCentralFormat);
            offset += 20;

            // write company PAN
            g.DrawString("PAN: " + dtCompany.Rows[0]["PAN"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width, height), stringCentralFormat);
            offset += 20;

            // write company phone number
            g.DrawString(dtCompany.Rows[0]["Telephone"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width, height), stringCentralFormat);
            offset += 20;

            // to write bill number and date
            g.DrawString("Bill no.:" + txtVoucherNo.Text, font, new SolidBrush(Color.Black), 15, startY + offset);
            //offset += 10;

            g.DrawString("Date:" + txtDate.Text, font, new SolidBrush(Color.Black), startX + 125, startY + offset);
            offset += 20;

            g.DrawString(m_PatientID > 0 ? "Name : " + m_PatientName : cboCashParty.Text, font3, new SolidBrush(Color.Black), 25, 140);

            g.DrawString("Age : " + m_Age, font3, new SolidBrush(Color.Black), 25, 155);

            g.DrawString("Address : " + m_Address, font3, new SolidBrush(Color.Black), 25, 170);


            g.DrawString("Sex : " + m_sex, font3, new SolidBrush(Color.Black), 25, 185);

            g.DrawString("Telephone : " + m_Telephone, font3, new SolidBrush(Color.Black), 25, 200);

            g.DrawString("Mobile : " + m_Mobile, font3, new SolidBrush(Color.Black), 25, 215);

            g.DrawString("Dr.Counsultant : " + "Dr." + m_DoctorName, font2, new SolidBrush(Color.Black), 25, 230);      

 

        }

        public void ClearPatientDetail()
        {
            m_PatientName = "";
            m_Age = "";
            m_sex = "";
            m_Telephone = "";
            m_Mobile = "";
            m_Date = "";
            m_Address = "";
            m_City = "";
            m_PatientID = 0;
            m_ledgerId = 0;

        }
        void PrintPOS( PrintPageEventArgs e)
        {
            try
            {
                // Create rectangle for drawing.
                float x = 0.0F;
                float y = 10.0F;
                float width = 285.0F;
                float height = 20.0F;
                //RectangleF drawRect = new RectangleF(x, y, width, height);

                // formatting the string for central alignment
                StringFormat stringCentralFormat = new StringFormat();
                stringCentralFormat.Alignment = StringAlignment.Center;
                stringCentralFormat.LineAlignment = StringAlignment.Center;

                //formating string for right alignment
                StringFormat stringRightFormat = new StringFormat();
                stringRightFormat.Alignment = StringAlignment.Far;
                stringRightFormat.LineAlignment = StringAlignment.Far;

                DataTable dtCompany = CompanyDetails.GetCompanyInfo();
                Graphics g = e.Graphics;
                Font font = new Font("Courier New", 8);
                float fontHeight = font.GetHeight();
                int startX = 15;
                int startY = 20;
                int offset = 20;
                String underLine = "------------------------------------------";
                string underLine1 = "-----------------------------------------";
                String underLine2 = "----------------------";
                // write comapny name
                g.DrawString(dtCompany.Rows[0]["Name"].ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(x, y, width, height), stringCentralFormat);
                //offset += 10;

                // write company address
                g.DrawString(dtCompany.Rows[0]["Address1"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width, height), stringCentralFormat);
                offset += 20;

                // write company PAN
                g.DrawString("PAN: " + dtCompany.Rows[0]["PAN"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width, height), stringCentralFormat);
                offset += 20;

                // write company phone number
                g.DrawString(dtCompany.Rows[0]["Telephone"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width, height), stringCentralFormat);
                offset += 20;

                // to write bill number and date
                g.DrawString("Bill no.:" + txtVoucherNo.Text, font, new SolidBrush(Color.Black), x, startY + offset);
                //offset += 10;

                g.DrawString("Date:" + txtDate.Text, font, new SolidBrush(Color.Black), startX + 125, startY + offset);
                offset += 20;

                // to draw header values
                int columnSpace = 0;

                g.DrawString("SN", font, new SolidBrush(Color.Black), columnSpace, startY + offset);
                columnSpace += 10;

                g.DrawString("Particulars", font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset, StringFormat.GenericTypographic);
                columnSpace += 100;

                g.DrawString("Qty", font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);
                columnSpace += 30;

                g.DrawString("Rate", font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);
                columnSpace += 50;

                g.DrawString("Amount", font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);
                columnSpace += 70;

                offset += 20;
                // to draw another line
                g.DrawString(underLine1, font, new SolidBrush(Color.Black), 0, startY + offset);
                offset += 20;

                int rowCount = 0; // to count the number of rows for S.N.
                // to fill sales data in the receipt
                foreach (DataRow dr in dtsalesInvoice.Rows)
                {
                    rowCount++;
                    columnSpace = 0;

                    g.DrawString(rowCount.ToString(), font, new SolidBrush(Color.Black), columnSpace + 3, startY + offset);
                    columnSpace += 10;

                    String productName = dr["Product"].ToString();

                    // to trim the length of string if it exceeds a particular length
                    int myStringWidth = TextRenderer.MeasureText(productName, font).Width;
                    SizeF size = g.MeasureString(dr["Product"].ToString(), font, dr["Product"].ToString().Length);
                    if (myStringWidth > 100)
                    {
                        string result = productName.Substring(0, productName.Length * 100 / myStringWidth); // +"...";
                        g.DrawString(result, font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);
                    }
                    else
                        g.DrawString(productName, font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);
                    columnSpace += 100;

                    g.DrawString(dr["Quantity"].ToString(), font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);
                    columnSpace += 25;

                    double rate = Convert.ToDouble(dr["SalesRate"]);
                    g.DrawString(rate == 0 ? "0.00" : rate.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)), font, new SolidBrush(Color.Black), new RectangleF(startX + columnSpace, startY + offset, 50, 15), stringRightFormat);
                    columnSpace += 50;

                    //g.DrawString(dr["Amount"].ToString(), font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);//, stringRightFormat);
                    //columnSpace += 70;

                    g.DrawString(dr["Amount"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(startX + columnSpace, startY + offset, 50, 15), stringRightFormat); //startX + columnSpace, startY + offset);//, stringRightFormat);
                    columnSpace += 75;
                    //g.DrawString(dr["DiscPercentage"].ToString(), font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);
                    //columnSpace += 20;

                    //g.DrawString(dr["Discount"].ToString(), font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);
                    //columnSpace += 20;

                    //g.DrawString(dr["NetAmount"].ToString(), font, new SolidBrush(Color.Black), startX + columnSpace, startY + offset);
                    //columnSpace += 20;

                    offset += 20;
                }

                offset += 40;
                g.DrawString(underLine2, font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, 250, height), stringRightFormat);

                offset += 20;
                double grossTotal = Convert.ToDouble(lblGross.Text);
                g.DrawString("Gross Total : " + (grossTotal == 0 ? "0.00" : grossTotal.ToString(Misc.FormatNumber(false, Global.DecimalPlaces))), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, 250, height), stringRightFormat);

                offset += 20;
                double disc = Convert.ToDouble(lblDiscountAmount.Text);
                g.DrawString("Discount : " + (disc == 0 ? "0.00" : disc.ToString(Misc.FormatNumber(false, Global.DecimalPlaces))), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, 250, height), stringRightFormat);

                // offset += 10;
                // g.DrawString(underLine2, font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width-10, height), stringRightFormat);

                //offset += 10;
                //g.DrawString("Net Amount :"+lblNetAmout.Text, font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width-10, height), stringRightFormat);

                offset += 20;
                g.DrawString("Adjustment : " + lblAdjustment.Text, font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, 250, height), stringRightFormat);

                offset += 20;
                g.DrawString(underLine2, font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, 250, height), stringRightFormat);

                offset += 20;
                g.DrawString("Total : " + lblGrandTotal.Text, font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, 250, height), stringRightFormat);

                offset += 30;
                g.DrawString(underLine, font, new SolidBrush(Color.Black), x, startY + offset);

                offset += 20;
                g.DrawString("Welcome to " + dtCompany.Rows[0]["Name"].ToString(), font, new SolidBrush(Color.Black), x, y + offset);
                offset += 10;
                g.DrawString("Exchange within 2 days with the ", font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, docwidth, height), stringCentralFormat);
                offset += 10;
                g.DrawString("invoice", font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, docwidth, height), stringCentralFormat);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
       
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }





        private void btncashparty_Leave(object sender, EventArgs e)
        {
            cboCashParty.Items.Clear();
            LoadCboCashParty();
        }



        private void txtproductpin_TextChanged(object sender, EventArgs e)
        {
            timerProductPIN.Start();


        }

        /// <summary>
        /// Adds the product into the grid simply by getting Product ID
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        private bool AddProductByID(string ProductCode)
        {
            try
            {


                // DataTable dtgetprod = SalesInvoiceModel.GetAllProductByID(Convert.ToInt32(txtproductpin.Text));
                DataTable dtgetprod = SalesInvoiceModel.GetProducts(0, ProductCode, Global.ParentAccClassID);

                if (dtgetprod.Rows.Count > 0)
                {
                    DataRow drprod = dtgetprod.Rows[0];
                    DataTable dtProductStock = Product.GetAllProduct(null, Convert.ToInt32(drprod["ProductID"]), " ", DateTime.Today, true, StockStatusType.DiffInStock);

                    string productname = drprod["engname"].ToString();
                    dtrow = orderTab.NewRow();
                    dtrow["id"] = drprod["ProductID"].ToString();
                    dtrow["qty"] = 1;
                    dtrow["item"] = productname;
                    dtrow["code"] = drprod["ProductCode"].ToString();
                    dtrow["cost"] = drprod["PurchaseRate"].ToString();
                    dtrow["price"] = drprod["SalesRate"].ToString();
                    dtrow["unitID"] = drprod["UnitID"].ToString();
                    dtrow["value"] = double.Parse(dtrow["qty"].ToString()) * double.Parse(dtrow["price"].ToString());
                    dtrow["stockQty"] = dtProductStock.Rows[0]["ClosingStock"].ToString();
                    orderTab.Rows.Add(dtrow);
                    RefreshDataGrid(null, null);
                    AddRowProduct1(grdSalesInvoice.RowsCount);


                }
                return true;


            }
            catch (Exception ex)
            {

                Global.Msg("Error occurred while reading barcode. Message:" + ex.Message);

                return false;
            }

        }



        private void timerProductPIN_Tick(object sender, EventArgs e)
        {

            if (tickCounter >= 15) //One and half Second elapsed
            {
                //First Stop the timer so that if any messagebox or something is shown, it doesnt run again and again.
                timerProductPIN.Stop();
                tickCounter = 0;

                bool validate = true;
                if (String.IsNullOrEmpty(txtProductPin.Text.Trim()))
                    validate = false;

                Regex regex = new Regex("^[0-9]*$");
                if (!regex.IsMatch(txtProductPin.Text))
                {

                    MessageBox.Show("Number Only Allowed");
                    validate = false;
                }


                if (validate == true)
                {
                    if (AddProductByID(txtProductPin.Text.Trim()))
                    {
                        CalculateGrossAmount();
                        CalculateAdjustmentAmount();
                        CalculateNetAmount();
                        CalculateTotalQuantity();
                        //grdSalesInvoice[(grdSalesInvoice.Rows-1), (int)SalesInvoiceGridColumn.ProductName].Value
                        txtProductPin.Text = "";
                    }

                }
                else //Validate=false
                {

                    timerProductPIN.Stop();

                }



            }
            else
                tickCounter++; //Increase tickcounter until it gets 10

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
        private void chkRecurring_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkRecurring.Checked && m_dtRecurringSetting.Rows.Count == 0))
            {
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "SALES_INVOICE", m_dtRecurringSetting);
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
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "SALES_INVOICE"); // delete from database
                    ClearRecurringSetting();  // clear local variables
                }
                else
                    chkRecurring.Checked = true;
            }
            if ((chkRecurring.Checked == true && m_mode == EntryMode.EDIT) || (chkRecurring.Checked == true && m_mode == EntryMode.NEW))
                btnSetup.Enabled = true;
            else
                btnSetup.Enabled = false;
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "SALES_INVOICE", m_dtRecurringSetting);
            fvr.ShowDialog();
        }

        string RSID = null, recurringVoucherID = null;
        public void CheckRecurringSetting(string VoucherID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            m_dtRecurringSetting = RecurringVoucher.GetRecurringVoucherSetting(VoucherID, "SALES_INVOICE");
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
        private void ClearRecurringSetting()
        {
            m_dtRecurringSetting.Rows.Clear();
            chkRecurring.Checked = false;
            recurringVoucherID = "";
            RSID = "";
        }
        #endregion

        #region methods related to voucher list

        private void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UserPermission.ChkUserPermission("SALE_INVOICE_VIEW"))
                {
                    Global.MsgError("Sorry! you dont have permission to View Sales Invoice. Please contact your administrator for permission.");
                    return;
                }
                string[] vouchValues = new string[5];
                vouchValues[0] = "SALES_INVOICE";               // voucherType
                vouchValues[1] = "Inv.tblSalesInvoiceMaster";   // master tableName for the given voucher type  
                vouchValues[2] = "Inv.tblSalesInvoiceDetails";  // details tableName for the given voucher type
                vouchValues[3] = "SalesInvoiceID";              // master ID for the given master table
                vouchValues[4] = "SalesInvoice_Date";              // date field for a given voucher

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
            m_SalesInvoiceID = VoucherID;
            txtSalesInvoiceID.Text = VoucherID.ToString();
            Navigation(Navigate.ID);
            //frmSalesInvoice_Load(null, null);
        }
        #endregion

        #region method related to bank reconciliation closing
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
        public string ToDeleteRows = "";
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
                dr1["RVID"] = 0;
                dr1["LedgerID"] = CurrAccLedgerID;
                dr1["VoucherType"] = "SALES";
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
                dr1["RVID"] = 0;
                dr1["LedgerID"] = CurrAccLedgerID;
                dr1["VoucherType"] = "SALES";
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
                string crdr = "Debit";

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
                    //int partyID;
                    //partyID = Convert.ToInt32(cboCashParty.SelectedValue);
                    //try
                    //{
                    //    SetPatientDetail(partyID);
                    //}
                    //catch (Exception)
                    //{

                    //}




                    ListItem LiCashPartyLedgerID = new ListItem();
                    LiCashPartyLedgerID = (ListItem)cboCashParty.SelectedItem;
                    CurrAccLedgerID = LiCashPartyLedgerID.ID;
                    if (m_mode == EntryMode.EDIT)
                        isNewReferenceVoucher = VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(txtSalesInvoiceID.Text), CurrAccLedgerID, "SALES");
                    else
                        isNewReferenceVoucher = false;
                    if (VoucherReference.CheckIfReferece(CurrAccLedgerID) && !isNewReferenceVoucher) // if isBillReference is true for given ledger then load the reference form
                    {
                        Form fc = Application.OpenForms["frmReference"];

                        if (fc != null)
                            fc.Close();

                        if (txtSalesInvoiceID.Text != "")
                        {
                            vouchID = Convert.ToInt32(txtSalesInvoiceID.Text);
                        }

                        frmReference fr = new frmReference(this, vouchID, "SALES", CurrAccLedgerID);
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
                    CurRow = grdSalesInvoice.Selection.GetSelectionRegion().GetRowsIndex()[0];
                }
                catch
                {
                    CurRow = 0;
                    return;
                }
                if (CurRow > 0)
                {

                    decimal actualQty = UnitMaintenance.ConvertCompoundUnit(Convert.ToInt32(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Default_Unit].Value), Convert.ToInt32(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Units].Value), Convert.ToDecimal(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Qty].Value));
                    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Amount].Value = (actualQty * Convert.ToDecimal(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.SalesRate].Value)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Default_Qty].Value = actualQty;

                    decimal StockQty = Convert.ToDecimal(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Stock_Qty_Actual].Value.ToString());
                    if (StockQty > 0)
                    {
                        grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Stock_Qty].Value = (StockQty - Convert.ToDecimal(grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Default_Qty].Value)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                       
                    }
                    else
                    {
                        grdSalesInvoice[CurRow, (int)SalesInvoiceGridColumn.Stock_Qty].Value = 0;



                    }
                    CalculateGrossAmount();

                    //Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
                    WriteNetAmount(CurRow);
                    CalculateAdjustmentAmount();
                    CalculateNetAmount();
                    CalculateTotalQuantity();
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

                grdSalesInvoice[row, (int)SalesInvoiceGridColumn.Units] = new SourceGrid.Cells.Cell();
                grdSalesInvoice[row, (int)SalesInvoiceGridColumn.Units].Editor = cboUnits;

                SourceGrid.CellContext context = new SourceGrid.CellContext(grdSalesInvoice, new SourceGrid.Position(row, (int)SalesInvoiceGridColumn.Units));
                string selected = arrStr[0];
                try
                {
                    selected = Convert.ToString(grdSalesInvoice[row, (int)SalesInvoiceGridColumn.Default_Unit].Value); // if product is selected then load default unit of the product in combobox unit
                    int index = Array.IndexOf(arrInt, Convert.ToInt32(selected));
                    selected = arrStr[index];
                }
                catch
                {
                    selected = arrStr[0];
                }
                context.Cell.Editor.SetCellValue(context, selected);

                grdSalesInvoice[row, (int)SalesInvoiceGridColumn.Units].View = new SourceGrid.Cells.Views.Cell(RowView);
                grdSalesInvoice[row, (int)SalesInvoiceGridColumn.Units].AddController(evtUnitChanged);

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        #endregion
    
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void lblVouNo_Click(object sender, EventArgs e)
        {

        }

        public void AddPatient(string RegistrationNo, string Name, string Address, string Age, string Telephone, string Gender, int PatientID, int LedgerID)
        {
            try
            {

                m_ledgerId = LedgerID;

                cboCashParty.SelectedValue = LedgerID;
                SetPatientDetail(LedgerID);

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
    }
}
