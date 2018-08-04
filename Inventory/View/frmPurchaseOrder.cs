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
using Inventory.DataSet;
using Inventory;
using Common;
using Inventory.Reports;

namespace Inventory
{
    public partial class frmPurchaseOrder : Form, ListProduct, IVoucherRecurring, IVoucherList, IfrmDateConverter
    {
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private int loopCounter = 0;
        private string Prefix = "";
        // from previous
        bool hasChanged = false;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private bool IsFieldChanged = false;
        private bool IsShortcutKey = false;
        private double purchaserate = 0;
        private string productcode = "";
        private int CurrRowPos = 0;
        double NetAmount = 0;
        double TotalQty = 0;
        private int PurchaseInvoiceIDCopy = 0;
        ////For Export Menu
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
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
            Del = 0, SNo, Code_No, ProductName, Qty, PurchaseRate, Amount, Updated_Qty, Pending_Qty, ProductID
        };
        private DataSet.dsPurchaseOrderNew dsPurchaseOrderNew = new DataSet.dsPurchaseOrderNew();
        ListItem liProjectID = new ListItem();
        private int PurchaseOrderIDCopy = 0;
        List<int> AccClassID = new List<int>();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        PurchaseOrder m_PurchaseOrder = new PurchaseOrder();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked        
        DataTable dtAccClassID = new DataTable();
        /* SourceGrid.Cells.Controllers.CustomEvents evtQty = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtRate = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtProduct = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtProuctCode = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtDiscPercentage = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtDiscount = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtOrderNo = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtProductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtcboDrCrSelectedIndexChanged = new SourceGrid.Cells.Controllers.CustomEvents();*/
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtQty = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtRate = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProduct = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductCode = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtOrderNo = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtProductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();


        private int m_PurchaseOrderID;
        public frmPurchaseOrder()
        {
            InitializeComponent();
        }

        bool m_isRecurring = false;
        int m_RVID = 0; 
        /// <summary>
        /// constructor to open the form from voucher recurring reminder
        /// </summary>
        /// <param name="SalesInvoiceID"></param>
        /// <param name="isRecurring"></param>
        public frmPurchaseOrder(int PurchaseOrderID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_PurchaseOrderID = PurchaseOrderID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }
        public frmPurchaseOrder(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;

        }
        public frmPurchaseOrder(int PurchaseOrderID)
        {
            InitializeComponent();
            this.m_PurchaseOrderID = PurchaseOrderID;
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

        private void EnableControls(bool Enable)
        {
            chkRecurring.Enabled = btnSetup.Enabled = cboCashParty.Enabled = txtRemarks.Enabled = txtDate.Enabled = txtOrderNo.Enabled = grdPurchaseOrder.Enabled = treeAccClass.Enabled = btnAddAccClass.Enabled = tabControl1.Enabled = cboProjectName.Enabled = btnDate.Enabled = Enable;
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

        private void OrderNo_Selected(object sender, EventArgs e)
        {
            int maxOrderNo = 0;

            DataTable dtMaxPurchaseOrderMasterInfo = PurchaseOrder.GetPurchaseOrderMasterInfoByOrderNo(-1);
            if (dtMaxPurchaseOrderMasterInfo.Rows.Count > 0)
            {
                DataRow drMaxPurchaseOrderMasterInfo = dtMaxPurchaseOrderMasterInfo.Rows[0];

                try
                {
                    maxOrderNo = Convert.ToInt32(drMaxPurchaseOrderMasterInfo["OrderNo"]) + 1;
                }
                catch (Exception ex)
                {
                    maxOrderNo = 1;
                }
            }

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
                DisplayOrderNo();
            }
            //Get Purchase Order Master Info according OrderNo
            if ((dtPurchaseOrderMasterInfo.Rows.Count > 0) && (txtOrderNo.Text != ""))
            {
                //Fill the master info in corresponding Controls
                DataRow drPurchaseOrderMasterInfo = dtPurchaseOrderMasterInfo.Rows[0];
                txtDate.Text = Date.DBToSystem(drPurchaseOrderMasterInfo["PurchaseOrder_Date"].ToString());
                txtOrderNo.Text = drPurchaseOrderMasterInfo["OrderNo"].ToString();
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseOrderMasterInfo["CashPartyID"]), LangMgr.DefaultLanguage);
                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                cboCashParty.Text = drLedgerInfo["LedName"].ToString();


                //Get Order Detail Info with help of masterID

                DataTable dtPurchaseOrderDetailInfo = m_PurchaseOrder.GetPurchaseOrderDetails(Convert.ToInt32(drPurchaseOrderMasterInfo["PurchaseOrderID"]));
                decimal Amount = 0;
                int PendingQty = 0;
                for (int i = 1; i <= dtPurchaseOrderDetailInfo.Rows.Count; i++)
                {
                    DataRow drDetail = dtPurchaseOrderDetailInfo.Rows[i - 1];
                    grdPurchaseOrder[i, (int)GridColumn.SNo].Value = i.ToString();
                    grdPurchaseOrder[i, (int)GridColumn.Code_No].Value = drDetail["ProductCode"].ToString();
                    grdPurchaseOrder[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                    grdPurchaseOrder[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
                    grdPurchaseOrder[i, (int)GridColumn.PurchaseRate].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString();
                    Amount = (Convert.ToInt32(drDetail["Quantity"]) * Convert.ToDecimal(drDetail["PurchaseRate"]));
                    grdPurchaseOrder[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Amount);

                    //If we have updated quantity
                    if (drDetail["UpdatedQty"].ToString() != "")
                    {
                        grdPurchaseOrder[i, (int)GridColumn.Updated_Qty].Value = (Convert.ToInt32(drDetail["UpdatedQty"])).ToString();
                        PendingQty = (Convert.ToInt32(drDetail["Quantity"]) - Convert.ToInt32(drDetail["UpdatedQty"]));
                        grdPurchaseOrder[i, (int)GridColumn.Pending_Qty].Value = PendingQty.ToString();
                    }

                    //AddRowProduct(grdPurchaseOrder.RowsCount);
                    AddRowProduct1(grdPurchaseOrder.RowsCount);

                }
            }
            else if ((dtPurchaseOrderMasterInfo.Rows.Count <= 0) && (txtOrderNo.Text != "") && (maxOrderNo != (Convert.ToInt32(txtOrderNo.Text))))
            {
                Global.Msg("Order Number Doesnot Exist");
                DisplayOrderNo();


            }

        }

        private void DisplayOrderNo()
        {
            //Display the Latest Order Number in corresponding Controls
            DataTable dtPurchaseOrderMasterInfo = PurchaseOrder.GetPurchaseOrderMasterInfoByOrderNo(-1);//Getting Maximum value of Purchase Order 
            DataRow drPurchaseOrderMasterInfo = dtPurchaseOrderMasterInfo.Rows[0];
            if (drPurchaseOrderMasterInfo["OrderNo"].ToString() == "")
            {
                txtOrderNo.Text = "1";
            }
            else
            {
                txtOrderNo.Text = (Convert.ToInt32(drPurchaseOrderMasterInfo["OrderNo"]) + 1).ToString();//Updating the order number incrementing by one                 
            }
        }

        private void frmPurchaseOrder_Load(object sender, EventArgs e)
        {
            chkDoNotClose.Checked = true;
            DisplayOrderNo();
            // ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            ChangeState(EntryMode.NEW);
            txtPurchaseOrderID.Visible = false;
            ShowAccClassInTreeView(treeAccClass, null, 0);

            m_mode = EntryMode.NEW;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            try
            {
                //Event trigerred when delete button is clicked
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                //Event when account is selected
                evtProduct.FocusLeft += new EventHandler(Product_Selected);

                //Event when productcode is selected
                evtProductCode.FocusLeft += new EventHandler(ProductCode_Selected);
                //Event when Quntity is selected
                evtQty.FocusLeft += new EventHandler(Qty_Modified);
                //Event when Rate is selected
                evtRate.FocusLeft += new EventHandler(PurchaseRate_Modified);

                //Event when OrderNo is selected
                evtOrderNo.FocusLeft += new EventHandler(OrderNo_Selected);
                txtOrderNo.LostFocus += new EventHandler(OrderNo_Selected);

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

                cboCashParty.DisplayMember = "value";//This value is  for showing at Load condition
                cboCashParty.ValueMember = "id";//This value is stored only not to be shown at Load condition
                cboCashParty.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
                #endregion

                #region Another Method


                //ANOTHER METHOD

                //string[] Cash_In_Hand = Ledger.GetLedgerList(102);
                //string[] Debtor = Ledger.GetLedgerList(29);
                //string[] Creditor = Ledger.GetLedgerList(114);
                //string[] Party = new string[Cash_In_Hand.Length + Debtor.Length + Creditor.Length];
                //Cash_In_Hand.CopyTo(Party, 0);
                //Debtor.CopyTo(Party, Cash_In_Hand.Length);
                //Creditor.CopyTo(Party, Debtor.Length + Cash_In_Hand.Length);
                //cmboCashPartyAcc.Items.Clear();

                //foreach (string str in Party)
                //{

                //    cmboCashPartyAcc.Items.Add(str);
                //}

                #endregion
                grdPurchaseOrder.Redim(2, 10);
                btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                AddGridHeader();
                //AddRowProduct(1);
                AddRowProduct1(1);

                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID
                if (m_PurchaseOrderID > 0)
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
                            vouchID = m_PurchaseOrderID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }
                        Sales m_Sales = new Sales();
                        //Getting the value of SeriesID via MasterID or VouchID


                        DataTable dtPurchaseOrderMaster = m_PurchaseOrder.GetPurchaseOrderMasterInfo(vouchID.ToString());



                        if (dtPurchaseOrderMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }
                        DataRow drPurchaseOrderMaster = dtPurchaseOrderMaster.Rows[0];
                        txtDate.Text = Date.DBToSystem(drPurchaseOrderMaster["PurchaseOrder_Date"].ToString());
                        txtRemarks.Text = drPurchaseOrderMaster["Remarks"].ToString();
                        txtPurchaseOrderID.Text = drPurchaseOrderMaster["PurchaseOrderID"].ToString();
                        if (!m_isRecurring)
                        {
                            //txtVoucherNo.Text = drSalesOrderMaster["Voucher_No"].ToString();
                            txtDate.Text = Date.DBToSystem(drPurchaseOrderMaster["PurchaseOrder_Date"].ToString());
                        }
                        else
                        {
                            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if recurring load today's date
                            //txtduedate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                        }
                        //dsSalesInvoice.Tables["tblSalesInvoiceMaster"].Rows.Add(cboSeriesName.Text, drSalesInvoiceMaster["Voucher_No"].ToString(), Date.DBToSystem(drSalesInvoiceMaster["SalesInvoice_Date"].ToString()), cboCashParty.Text, txtOrderNo.Text, cboDepot.Text, cmboSalesAcc.Text, drSalesInvoiceMaster["Remarks"].ToString());
                        //Show the corresponding Cash/Party Account Ledger in Combobox
                        DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseOrderMaster["CashPartyID"]), LangMgr.DefaultLanguage);
                        DataRow drCashPartyLedgerInfo = dtCashPartyInfo.Rows[0];
                        //Show the corresponding Sales Account Ledger in Combobox          
                        DataTable dtPurchaseOrderDetails = m_PurchaseOrder.GetPurchaseOrderDetails(vouchID);

                        for (int i = 1; i <= dtPurchaseOrderDetails.Rows.Count; i++)
                        {
                            DataRow drDetail = dtPurchaseOrderDetails.Rows[i - 1];
                            grdPurchaseOrder[i, (int)GridColumn.SNo].Value = i.ToString();
                            grdPurchaseOrder[i, (int)GridColumn.Code_No].Value = drDetail["ProductCode"].ToString();
                            grdPurchaseOrder[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                            grdPurchaseOrder[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();
                            grdPurchaseOrder[i, (int)GridColumn.PurchaseRate].Value = drDetail["PurchaseRate"].ToString();
                            grdPurchaseOrder[i, (int)GridColumn.Amount].Value = drDetail["Amount"].ToString();
                            grdPurchaseOrder[i, (int)GridColumn.Updated_Qty].Value = drDetail["UpdatedQty"].ToString();
                            grdPurchaseOrder[i, (int)GridColumn.Pending_Qty].Value = drDetail["PendingQty"].ToString();
                            grdPurchaseOrder[i, (int)GridColumn.ProductID].Value = drDetail["ProductID"].ToString();

                            //AddRowProduct(grdPurchaseOrder.RowsCount);
                            AddRowProduct1(grdPurchaseOrder.RowsCount);
                            //dsSalesInvoice.Tables["tblSalesInvoiceDetails"].Rows.Add(drDetail["Code"].ToString(), drDetail["ProductName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Quantity"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["SalesRate"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["DiscPercentage"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Discount"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Net_Amount"])).ToString());
                        }

                    }


                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }

                #endregion

                //txtOrderNo.Enabled = false;
                // if recurring is true then donot load recurring settings for new voucher
                if (!m_isRecurring)
                    CheckRecurringSetting(txtPurchaseOrderID.Text);
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

        }

        private void AddGridHeader()
        {

            grdPurchaseOrder[0, (int)GridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del");

            grdPurchaseOrder[0, (int)GridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S.No.");

            grdPurchaseOrder[0, (int)GridColumn.Code_No] = new SourceGrid.Cells.ColumnHeader("Code");
            grdPurchaseOrder[0, (int)GridColumn.Code_No].Column.Width = 100;

            grdPurchaseOrder[0, (int)GridColumn.ProductName] = new SourceGrid.Cells.ColumnHeader("Product Name");
            grdPurchaseOrder[0, (int)GridColumn.ProductName].Column.Width = 100;

            grdPurchaseOrder[0, (int)GridColumn.Qty] = new SourceGrid.Cells.ColumnHeader("Qty");

            grdPurchaseOrder[0, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.ColumnHeader("Rate");

            grdPurchaseOrder[0, (int)GridColumn.Amount] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdPurchaseOrder[0, (int)GridColumn.Amount].Column.Width = 100;

            grdPurchaseOrder[0, (int)GridColumn.Updated_Qty] = new SourceGrid.Cells.ColumnHeader("Updated Quantity");
            grdPurchaseOrder[0, (int)GridColumn.Updated_Qty].Column.Width = 100;

            grdPurchaseOrder[0, (int)GridColumn.Pending_Qty] = new SourceGrid.Cells.ColumnHeader("Pending Quantity");
            grdPurchaseOrder[0, (int)GridColumn.Pending_Qty].Column.Width = 100;

            grdPurchaseOrder[0, (int)GridColumn.ProductID] = new SourceGrid.Cells.ColumnHeader("ProductID");
            grdPurchaseOrder[0, (int)GridColumn.ProductID].Column.Visible = false;









        }

        private void AddRowProduct(int RowCount)
        {
            try
            {
                //Add a new row
                grdPurchaseOrder.Redim(Convert.ToInt32(RowCount + 1), grdPurchaseOrder.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdPurchaseOrder[i, (int)GridColumn.Del] = btnDelete;
                grdPurchaseOrder[i, (int)GridColumn.Del].AddController(evtDelete);
                grdPurchaseOrder[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdPurchaseOrder[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();
                grdPurchaseOrder[i, (int)GridColumn.Code_No].Value = "(NEW)";
                //SourceGrid.Cells.Editors.ComboBox cboProduct = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                SourceGrid.Cells.Editors.ComboBox cboProduct = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                SourceGrid.Cells.Editors.ComboBox cboProductCode = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                //DataTable dt = Product.GetProductList(0);
                DataTable dt = Product.GetProductList(0);
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
                for (int i1 = 0; i1 < dt.Rows.Count; i1++)
                {
                    DataRow dr = dt.Rows[i1];
                    //lstProduct.Add(dr[LangField].ToString());
                    lstProduct.Add(dr[LangField].ToString());
                    lstProductCode.Add(dr["ProductCode"].ToString());
                }

                //for code
                cboProductCode.StandardValues = (string[])lstProductCode.ToArray<string>();
                cboProductCode.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
                cboProductCode.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboProductCode.Control.LostFocus += new EventHandler(ProductCode_Selected);

                cboProductCode.EditableMode = SourceGrid.EditableMode.Focus;
                grdPurchaseOrder[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell("", cboProductCode);
                grdPurchaseOrder[i, (int)GridColumn.Code_No].AddController(evtProductCode);
                grdPurchaseOrder[i, (int)GridColumn.Code_No].Value = "(NEW)";

                cboProduct.StandardValues = (string[])lstProduct.ToArray<string>();
                //cboProduct.StandardValues =  (string[])lstProduct.ToArray<string>();
                cboProduct.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
                cboProduct.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboProduct.Control.LostFocus += new EventHandler(Product_Selected);
                cboProduct.EditableMode = SourceGrid.EditableMode.Focus;
                grdPurchaseOrder[i, (int)GridColumn.ProductName] = new SourceGrid.Cells.Cell("", cboProduct);
                grdPurchaseOrder[i, (int)GridColumn.ProductName].AddController(evtProduct);
                grdPurchaseOrder[i, (int)GridColumn.ProductName].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus;
                grdPurchaseOrder[i, 4] = new SourceGrid.Cells.Cell("", txtQty);
                //grdSalesOrder[i, 4].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;
                grdPurchaseOrder[i, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                //grdSalesOrder[i, 5].Value = "(NEW)";

                //for  amount
                grdPurchaseOrder[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell();
                //grdPurchaseOrder[i, 6].Value = "(NEW)";


                //for Updated quantity
                grdPurchaseOrder[i, (int)GridColumn.Updated_Qty] = new SourceGrid.Cells.Cell();
                //grdSalesOrder[i, 7].Value = "(NEW)";

                //for Pending quantity
                grdPurchaseOrder[i, (int)GridColumn.Pending_Qty] = new SourceGrid.Cells.Cell();


                //grdSalesOrder[i, 8].Value = "(NEW)";

                grdPurchaseOrder[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell("ID");
                grdPurchaseOrder[i, (int)GridColumn.ProductID].Value = "0";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
            int CurRow = grdPurchaseOrder.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Using the name find corresponding code
            //DataTable dt = Product.GetProductByName(ct.DisplayText);
            DataTable dt = Product.GetProductByCode(ct.DisplayText);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row
                    grdPurchaseOrder[(CurRow), (int)GridColumn.ProductName].Value = dr[LangField].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double PurchaseRate = Math.Round(Convert.ToDouble(dr["PurchaseRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.
                    grdPurchaseOrder[(CurRow), (int)GridColumn.PurchaseRate].Value = PurchaseRate.ToString();
                    grdPurchaseOrder[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
            }
            int RowsCount = grdPurchaseOrder.RowsCount;
            string LastProductCell = (string)grdPurchaseOrder[RowsCount - 1, (int)GridColumn.PurchaseRate].Value;
            ////Check whether the new row is already added
            if (LastProductCell != "(NEW)")
            {
                // AddRowProduct(RowsCount);
                AddRowProduct1(RowsCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowsCount - 1);
            }
            WriteAmount(CurRow, 1);//Write amount on grid'cell when quantity is unit


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
            int CurRow = grdPurchaseOrder.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Using the name find corresponding code
            //DataTable dt = Product.GetProductByName(ct.DisplayText);
            DataTable dt = Product.GetProductByName(ct.DisplayText);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //Add a new row
                    grdPurchaseOrder[(CurRow), (int)GridColumn.Code_No].Value = dr["ProductCode"].ToString();
                    //If this is a new row, then do nothing
                    if (isNewRow(CurRow))
                        return;

                    double PurchaseRate = Math.Round(Convert.ToDouble(dr["PurchaseRate"].ToString()), Global.DecimalPlaces);//Round the value of Rate after decimal.Here we set 2 digit after decimal.
                    grdPurchaseOrder[(CurRow), (int)GridColumn.PurchaseRate].Value = PurchaseRate.ToString();
                    grdPurchaseOrder[(CurRow), (int)GridColumn.Qty].Value = "1"; //Set quantity to 1 by default
                }
            }
            int RowsCount = grdPurchaseOrder.RowsCount;
            string LastProductCell = (string)grdPurchaseOrder[RowsCount - 1, (int)GridColumn.ProductName].Value;
            ////Check whether the new row is already added
            if (LastProductCell != "(NEW)")
            {
                //AddRowProduct(RowsCount);
                AddRowProduct1(RowsCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowsCount - 1);
            }
            WriteAmount(CurRow, 1);//Write amount on grid'cell when quantity is unit

        }

        private void ClearNew(int RowCount)
        {

        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdPurchaseOrder.RowsCount - 2)
                grdPurchaseOrder.Rows.Remove(ctx.Position.Row);
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
                int CurRow = grdPurchaseOrder.Selection.GetSelectionRegion().GetRowsIndex()[0];

                if (isNewRow(CurRow))
                    return;

                int RowCount = grdPurchaseOrder.RowsCount;
                object Qty = ((TextBox)sender).Text;
                //bool IsInt = Misc.IsInt(Qty, false);//This function check whether variable is integer or not?
                //if (IsInt == false)
                //{
                //    Global.MsgError("The quantity you posted is invalid! Please post the integer value");
                //    grdPurchaseOrder[CurRow, (int)GridColumn.Amount].Value = "";
                //    grdPurchaseOrder[CurRow, (int)GridColumn.Qty].Value = "1";
                //    return;
                //}

                //Check whether the value of quantity is zero or not?
                if (Convert.ToDouble(Qty) == 0)
                {
                    Global.MsgError("The Quantity shouldnot be zero. Fill the Quantity first!");
                    grdPurchaseOrder[CurRow, (int)GridColumn.Qty].Value = "1";
                    grdPurchaseOrder[CurRow, (int)GridColumn.Amount].Value = "0";
                    ((TextBox)sender).Text = "1";
                    return;
                }
                TotalQty += Convert.ToDouble(Qty);
                lblTotalQty.Text = TotalQty.ToString();
                WriteAmount(CurRow, Convert.ToDouble(Qty));
                CalculateNetAmount();

                //Call the function when there is no any discount then bydefault set the zero discount and post the value of amount in 
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
                int CurRow = grdPurchaseOrder.Selection.GetSelectionRegion().GetRowsIndex()[0];
                if (isNewRow(CurRow))
                    return;

                int RowCount = grdPurchaseOrder.RowsCount;
                object PurchaseRate = ((TextBox)sender).Text;
                bool IsDouble = Misc.IsNumeric(PurchaseRate);//This function check whether variable is Double  or not?
                if (IsDouble == false)
                {
                    Global.MsgError("The Rate you posted is invalid! Please post the integer value");
                    grdPurchaseOrder[CurRow, (int)GridColumn.Amount].Value = "";
                    grdPurchaseOrder[CurRow, (int)GridColumn.Qty].Value = "1";
                    return;
                }

                string Qty = grdPurchaseOrder[CurRow, (int)GridColumn.Qty].Value.ToString();
                double Amount = Convert.ToDouble(Qty) * Convert.ToDouble(PurchaseRate);
                grdPurchaseOrder[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
                //CalculateGrossAmount();
                //WriteNetAmount(CurRow);
                CalculateNetAmount();
                //CalculateTotalQuantity();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void WriteAmount(int CurRow, double Qty)
        {
            string PurchaseRate = grdPurchaseOrder[CurRow, (int)GridColumn.PurchaseRate].Value.ToString();
            double m_PurchaseRate = 0;
            try
            {
                m_PurchaseRate = Convert.ToDouble(PurchaseRate);
            }
            catch (Exception)
            {
            }
            double Amount = Convert.ToDouble(Qty) * m_PurchaseRate;
            grdPurchaseOrder[CurRow, (int)GridColumn.Amount].Value = Amount.ToString();
        }

        private bool isNewRow(int CurRow)
        {
            if (grdPurchaseOrder[CurRow, (int)GridColumn.Code_No].Value == "(NEW)")
                return true;
            else
                return false;
        }

        private bool Validate()
        {
            bool bValidate = false;
            FormHandle m_FH = new FormHandle();
            bValidate = m_FH.Validate();
            if (txtOrderNo.Text == "")
            {
                Global.MsgError("Order Number is Required!");
                txtOrderNo.Focus();
                bValidate = false;
            }
            else if (cboCashParty.SelectedItem == null)
            {
                Global.MsgError("Invalid Cash Party Account is Selected");
            }

            m_FH.AddValidate(txtDate, DType.DATE, "Please enter valid date");
            return bValidate;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            //Check Validation
            if (!Validate())
                return;
            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }



            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    isNew = true;
                    NewGrid = " ";
                    OldGrid = " ";
                    NewGrid = NewGrid + "Voucher No" + txtDate.Text + "Series" + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Cash/Party" + cboCashParty.Text + "OrderNo" + txtOrderNo.Text;
                    for (int i = 0; i < grdPurchaseOrder.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                    {
                        string productname = grdPurchaseOrder[i + 1, (int)GridColumn.ProductName].Value.ToString();
                        string qty = grdPurchaseOrder[i + 1, (int)GridColumn.Qty].Value.ToString();
                        string rate = grdPurchaseOrder[i + 1, (int)GridColumn.PurchaseRate].Value.ToString();
                        string amt = grdPurchaseOrder[i + 1, (int)GridColumn.Amount].Value.ToString();
                        NewGrid = NewGrid + string.Concat(productname, qty, rate, amt);
                    }
                    NewGrid = "NewGridValues" + NewGrid;
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable PurchaseOrderDetails = new DataTable();
                        PurchaseOrderDetails.Columns.Add("ProductCode");
                        PurchaseOrderDetails.Columns.Add("Product");
                        PurchaseOrderDetails.Columns.Add("Quantity");
                        PurchaseOrderDetails.Columns.Add("PurchaseRate");
                        PurchaseOrderDetails.Columns.Add("Amount");
                        PurchaseOrderDetails.Columns.Add("UpdatedQty");
                        PurchaseOrderDetails.Columns.Add("PendingQty");
                        PurchaseOrderDetails.Columns.Add("ProductID");
                        for (int i = 0; i < grdPurchaseOrder.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            PurchaseOrderDetails.Rows.Add(grdPurchaseOrder[i + 1, (int)GridColumn.Code_No].Value, grdPurchaseOrder[i + 1, (int)GridColumn.Amount].Value, grdPurchaseOrder[i + 1, (int)GridColumn.Qty].Value, grdPurchaseOrder[i + 1, (int)GridColumn.PurchaseRate].Value, grdPurchaseOrder[i + 1, (int)GridColumn.Amount].Value, grdPurchaseOrder[i + 1, (int)GridColumn.Updated_Qty].Value, grdPurchaseOrder[i + 1, (int)GridColumn.Pending_Qty].Value, grdPurchaseOrder[i + 1, (int)GridColumn.ProductID].Value);
                        }
                        DateTime PurchaseOrder_Date = Date.ToDotNet(txtDate.Text);
                        ListItem LiCashPartyLedgerID = new ListItem();
                        LiCashPartyLedgerID = (ListItem)cboCashParty.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        if (AccClassID.Count != 0)
                        {
                            m_PurchaseOrder.Create(Convert.ToInt32(LiCashPartyLedgerID.ID), Convert.ToInt32(txtOrderNo.Text), PurchaseOrder_Date, txtRemarks.Text, PurchaseOrderDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), OldGrid, NewGrid, isNew, m_dtRecurringSetting);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_PurchaseOrder.Create(Convert.ToInt32(LiCashPartyLedgerID.ID), Convert.ToInt32(txtOrderNo.Text), PurchaseOrder_Date, txtRemarks.Text, PurchaseOrderDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), OldGrid, NewGrid, isNew, m_dtRecurringSetting);
                        }
                        Global.Msg("Purchase Order created successfully!");

                        // if the voucher is recurring and has been posted or saved, modify voucherposting table to set isPosted = true
                        string res;
                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_PurchaseOrderID, "PURCHASE_ORDER");
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID);
                            m_isRecurring = false;
                        }

                        ChangeState(EntryMode.NEW);
                        ClearPurchaseOrder();
                        DisplayOrderNo();

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
                                Global.Msg("ERROR: The Order Number already exists! Please enter another Order Number!", MBType.Warning, "Error");
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
                    NewGrid = NewGrid + "Voucher No" + txtDate.Text + "Series" + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Cash/Party" + cboCashParty.Text + "OrderNo" + txtOrderNo.Text;
                    for (int i = 0; i < grdPurchaseOrder.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                    {
                        string productname = grdPurchaseOrder[i + 1, (int)GridColumn.ProductName].Value.ToString();
                        string qty = grdPurchaseOrder[i + 1, (int)GridColumn.Qty].Value.ToString();
                        string rate = grdPurchaseOrder[i + 1, (int)GridColumn.PurchaseRate].Value.ToString();
                        string amt = grdPurchaseOrder[i + 1, (int)GridColumn.Amount].Value.ToString();
                        NewGrid = NewGrid + string.Concat(productname, qty, rate, amt);
                    }
                    NewGrid = "NewGridValues" + NewGrid;
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable PurchaseOrderDetails = new DataTable();
                        PurchaseOrderDetails.Columns.Add("ProductCode");
                        PurchaseOrderDetails.Columns.Add("Product");
                        PurchaseOrderDetails.Columns.Add("Quantity");
                        PurchaseOrderDetails.Columns.Add("PurchaseRate");
                        PurchaseOrderDetails.Columns.Add("Amount");
                        PurchaseOrderDetails.Columns.Add("UpdatedQty");
                        PurchaseOrderDetails.Columns.Add("PendingQty");
                        PurchaseOrderDetails.Columns.Add("ProductID");
                        for (int i = 0; i < grdPurchaseOrder.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            PurchaseOrderDetails.Rows.Add(grdPurchaseOrder[i + 1, (int)GridColumn.Code_No].Value, grdPurchaseOrder[i + 1, (int)GridColumn.ProductName].Value, grdPurchaseOrder[i + 1, (int)GridColumn.Qty].Value, grdPurchaseOrder[i + 1, (int)GridColumn.PurchaseRate].Value, grdPurchaseOrder[i + 1, (int)GridColumn.Amount].Value, grdPurchaseOrder[i + 1, (int)GridColumn.Updated_Qty].Value, grdPurchaseOrder[i + 1, (int)GridColumn.Pending_Qty].Value, grdPurchaseOrder[i + 1, (int)GridColumn.ProductID].Value);
                        }

                        DateTime PurchaseOrder_Date = Date.ToDotNet(txtDate.Text);
                        ListItem LiCashPartyID = new ListItem();
                        LiCashPartyID = (ListItem)cboCashParty.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;

                        if (AccClassID.Count != 0)
                        {
                            m_PurchaseOrder.Modify(Convert.ToInt32(txtPurchaseOrderID.Text), Convert.ToInt32(Convert.ToInt32(LiCashPartyID.ID)), txtOrderNo.Text, PurchaseOrder_Date, txtRemarks.Text, PurchaseOrderDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), OldGrid, NewGrid, isNew, m_dtRecurringSetting);

                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_PurchaseOrder.Modify(Convert.ToInt32(txtPurchaseOrderID.Text), Convert.ToInt32(Convert.ToInt32(LiCashPartyID.ID)), txtOrderNo.Text, PurchaseOrder_Date, txtRemarks.Text, PurchaseOrderDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), OldGrid, NewGrid, isNew, m_dtRecurringSetting);


                        }
                        Global.Msg("Purchase Order modified successfully!");
                        ChangeState(EntryMode.NORMAL);
                        ClearPurchaseOrder();
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

        private void ClearPurchaseOrder()
        {
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            txtOrderNo.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            // txtDate.Text = string.Empty;
            lblTotalQty.Text = "";
            lblNetAmout.Text = "";
            TotalQty = 0;
            grdPurchaseOrder.Rows.Clear();
            grdPurchaseOrder.Redim(2, 10);
            AddGridHeader(); //Write header part
            //AddRowServices(1);
            // AddRowProduct(1);
            AddRowProduct1(1);
            ClearRecurringSetting();
            m_isRecurring = false;

        }


        private bool Navigation(Navigate NavTo)
        {
            try
            {

                ChangeState(EntryMode.NORMAL);
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtPurchaseOrderID.Text);
                    if (PurchaseOrderIDCopy > 0)
                    {
                        VouchID = PurchaseOrderIDCopy;
                        PurchaseOrderIDCopy = 0;

                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtPurchaseOrderID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                DataTable dtPurchaseOrderMaster = m_PurchaseOrder.NavigatePurchaseOrderMaster(VouchID, NavTo);
                if (dtPurchaseOrderMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    return false;
                }

                //Clear everything in the form
                ClearPurchaseOrder();
                //Write the corresponding textboxes
                DataRow drPurchaseOrderMaster = dtPurchaseOrderMaster.Rows[0]; //There is only one row. First row is the required record
                //Show the corresponding Cash/Party Account Ledger in Combobox
                DataTable dtCashPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseOrderMaster["CashPartyID"]), LangMgr.DefaultLanguage);
                foreach (DataRow drCashPartyLedgerInfo in dtCashPartyInfo.Rows)
                {
                    cboCashParty.Text = drCashPartyLedgerInfo["LedName"].ToString();
                }

                txtDate.Text = Date.DBToSystem(drPurchaseOrderMaster["PurchaseOrder_Date"].ToString());
                txtRemarks.Text = drPurchaseOrderMaster["Remarks"].ToString();
                txtPurchaseOrderID.Text = drPurchaseOrderMaster["PurchaseOrderID"].ToString();
                txtOrderNo.Text = drPurchaseOrderMaster["OrderNo"].ToString();

                dsPurchaseOrderNew.Tables["tblPurchaseOrderMaster"].Rows.Add(cboCashParty.Text, drPurchaseOrderMaster["OrderNo"].ToString(), Date.DBToSystem(drPurchaseOrderMaster["PurchaseOrder_Date"].ToString()), drPurchaseOrderMaster["Remarks"].ToString());
                // dsSalesOrder.Tables["tblSalesOrderMaster"].Rows.Add(cboCashParty.Text, drSalesOrderMaster["OrderNo"].ToString(), Date.DBToSystem(drSalesOrderMaster["SalesOrder_Date"].ToString()), drSalesOrderMaster["Remarks"].ToString());
                DataTable dtPurchaseOrderDetails = m_PurchaseOrder.GetPurchaseOrderDetails(Convert.ToInt32(txtPurchaseOrderID.Text));
                decimal Amount = 0;
                int PendingQty = 0;
                int TQty = 0;
                for (int i = 1; i <= dtPurchaseOrderDetails.Rows.Count; i++)
                {
                    DataRow drDetail = dtPurchaseOrderDetails.Rows[i - 1];
                    grdPurchaseOrder[i, (int)GridColumn.SNo].Value = i.ToString();
                    grdPurchaseOrder[i, (int)GridColumn.Code_No].Value = drDetail["ProductCode"].ToString();
                    grdPurchaseOrder[i, (int)GridColumn.ProductName].Value = drDetail["ProductName"].ToString();
                    grdPurchaseOrder[i, (int)GridColumn.Qty].Value = drDetail["Quantity"].ToString();

                    grdPurchaseOrder[i, (int)GridColumn.PurchaseRate].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString();
                    Amount = (Convert.ToInt32(drDetail["Quantity"]) * Convert.ToDecimal(drDetail["PurchaseRate"]));

                    //TQty +=Convert.ToInt32( drDetail["Quantity"].ToString());
                    //  grdPurchaseOrder[i, 5].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString();
                    //Amount += (Convert.ToInt32(drDetail["Quantity"]) * Convert.ToDecimal(drDetail["PurchaseRate"]));
                    grdPurchaseOrder[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Amount);

                    //If we have updated quantity
                    if (drDetail["UpdatedQty"].ToString() != "")
                    {
                        grdPurchaseOrder[i, (int)GridColumn.Updated_Qty].Value = (Convert.ToInt32(drDetail["UpdatedQty"])).ToString();
                        PendingQty = (Convert.ToInt32(drDetail["Quantity"]) - Convert.ToInt32(drDetail["UpdatedQty"]));
                        grdPurchaseOrder[i, (int)GridColumn.Pending_Qty].Value = PendingQty.ToString();
                    }
                    grdPurchaseOrder[i, (int)GridColumn.ProductID].Value = Convert.ToInt32(drDetail["ProductID"]);
                    //AddRowProduct(grdPurchaseOrder.RowsCount);
                    AddRowProduct1(grdPurchaseOrder.RowsCount);
                    dsPurchaseOrderNew.Tables["tblPurchaseOrderDetails"].Rows.Add(drDetail["ProductCode"].ToString(), drDetail["ProductName"].ToString(), drDetail["Quantity"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["PurchaseRate"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString());
                    //dsSalesOrder.Tables["tblSalesOrderDetails"].Rows.Add(drDetail["ProductCode"].ToString(), drDetail["ProductName"].ToString(), drDetail["Quantity"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["SalesRate"])).ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString());
                }
                lblNetAmout.Text = Amount.ToString();
                lblTotalQty.Text = TQty.ToString();
                DataTable dtAccClassDtl = AccountClass.GetPOAccClassInfo(Convert.ToInt32(txtPurchaseOrderID.Text));
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
                // if recurring is true then donot load recurring settings for new voucher
                if (!m_isRecurring)
                    CheckRecurringSetting(txtPurchaseOrderID.Text);
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
            /* bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
             if (chkUserPermission == false)
             {
                 Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                 return;
             }

             if (m_mode == EntryMode.NEW)
             {
                 if (Global.MsgQuest("Do you want to save changes to Purchase Invoice?") == DialogResult.Yes)
                     btnSave_Click(sender, e);

             }

             Navigation(Navigate.First);*/
            //bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_ORDER_VIEW");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
            //    return;
            //}

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Order?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.First);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_ORDER_VIEW");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
            //    return;
            //}

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Order?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_ORDER_VIEW");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
            //    return;
            //}

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Order?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Next);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_ORDER_VIEW");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
            //    return;
            //}

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Purchase Order?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}

            Navigation(Navigate.Last);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_ORDER_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearPurchaseOrder();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
            DisplayOrderNo();
        }
        private void AddRowProduct1(int RowCount)
        {
            try
            {
                grdPurchaseOrder.Redim(Convert.ToInt32(RowCount + 1), grdPurchaseOrder.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowCount;
                grdPurchaseOrder[i, (int)GridColumn.Del] = btnDelete;
                grdPurchaseOrder[i, (int)GridColumn.Del].AddController(evtDelete);
                grdPurchaseOrder[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());
                grdPurchaseOrder[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell();

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
                grdPurchaseOrder[i, 3] = new SourceGrid.Cells.Cell("", txtProduct);
                txtProduct.Control.GotFocus += new EventHandler(Product_Focused);
                txtProduct.Control.LostFocus += new EventHandler(Product_Leave);
                txtProduct.Control.KeyDown += new KeyEventHandler(Product_KeyDown);
                txtProduct.Control.TextChanged += new EventHandler(Text_Change);
                grdPurchaseOrder[i, (int)GridColumn.ProductName].AddController(evtProductFocusLost);
                grdPurchaseOrder[i, (int)GridColumn.ProductName].Value = "(NEW)";

                //SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                //txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                //txtQty.EditableMode = SourceGrid.EditableMode.Focus;
                //grdPurchaseOrder[i, 4] = new SourceGrid.Cells.Cell("", txtQty);
                ////grdSalesOrder[i, 4].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdPurchaseOrder[i, (int)GridColumn.Qty] = new SourceGrid.Cells.Cell("1", txtQty);
                //grdPurchaseInvoice[i, 4].Value = "(NEW)";

                //SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                //txtPurchaseRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                //txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;
                //grdPurchaseOrder[i, 5] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                ////grdSalesOrder[i, 5].Value = "(NEW)";

                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.Control.LostFocus += new EventHandler(PurchaseRate_Modified);
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;
                grdPurchaseOrder[i, (int)GridColumn.PurchaseRate] = new SourceGrid.Cells.Cell("", txtPurchaseRate);
                // grdPurchaseInvoice[i, 5].Value = "(NEW)";

                ////for  amount
                //grdPurchaseOrder[i, 6] = new SourceGrid.Cells.Cell();
                ////grdPurchaseOrder[i, 6].Value = "(NEW)";
                ////for Updated quantity
                //grdPurchaseOrder[i, 7] = new SourceGrid.Cells.Cell();
                ////grdSalesOrder[i, 7].Value = "(NEW)";
                ////for Pending quantity
                //grdPurchaseOrder[i, 8] = new SourceGrid.Cells.Cell();
                ////grdSalesOrder[i, 8].Value = "(NEW)";


                grdPurchaseOrder[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell();
                // grdPurchaseInvoice[i, 6].Value = "(NEW)";
                // SourceGrid.Cells.Editors.TextBox txtDiscPercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                //txtDiscPercentage.Control.LostFocus += new EventHandler(DiscPercentage_Modified);
                //txtDiscPercentage.EditableMode = SourceGrid.EditableMode.Focus;
                //txtDiscPercentage.Control.Text = "0";

                grdPurchaseOrder[i, (int)GridColumn.Updated_Qty] = new SourceGrid.Cells.Cell();
                //grdPurchaseOrder[i, 7] = new SourceGrid.Cells.Cell("", txtDiscPercentage);
                //grdPurchaseOrder[i, 7].Value = "0";
                //SourceGrid.Cells.Editors.TextBox txtDiscount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                //txtDiscount.Control.LostFocus += new EventHandler(Discount_Modified);
                //txtDiscount.EditableMode = SourceGrid.EditableMode.Focus;

                grdPurchaseOrder[i, (int)GridColumn.Pending_Qty] = new SourceGrid.Cells.Cell();
                //grdPurchaseOrder[i, 8] = new SourceGrid.Cells.Cell("0", txtDiscount);
                //grdPurchaseOrder[i, 8].Value = "0";

                grdPurchaseOrder[i, (int)GridColumn.ProductID] = new SourceGrid.Cells.Cell("");
                grdPurchaseOrder[i, (int)GridColumn.ProductID].Value = "0";

                //grdPurchaseOrder[i, 10] = new SourceGrid.Cells.Cell("");
                // grdPurchaseOrder[i, 10].Value = "0";

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
                //frmlovproduct frm = new frmlovproduct(this);
                //frm.ShowDialog();
            }
        }
        private void Product_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            int RowsCount = grdPurchaseOrder.RowsCount;
            string LastServicesCell = (string)grdPurchaseOrder[RowsCount - 1, 3].Value;

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
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            //FillAllGridRow(CurrRowPos);
        }

        public void AddProduct(int productid, string productcode, string productname, bool IsSelected, double purchaserate, double salesrate, int qty, int defaultUnitID)
        {
            if (IsSelected)
            {
                int CurRow = grdPurchaseOrder.Selection.GetSelectionRegion().GetRowsIndex()[0];
                // CurrRowPos = ctx.Position.Row;
                ctx.Text = productname;
                purchaserate = purchaserate;
                productcode = productcode;
                grdPurchaseOrder[CurRow, (int)GridColumn.Code_No].Value = productcode;
                grdPurchaseOrder[CurRow, (int)GridColumn.PurchaseRate].Value = purchaserate;
                grdPurchaseOrder[CurRow, (int)GridColumn.ProductID].Value = productid.ToString();

                int RowsCount = grdPurchaseOrder.RowsCount;

                string LastServicesCell = (string)grdPurchaseOrder[RowsCount - 1, (int)GridColumn.ProductName].Value;
                ////Check whether the new row is already added
                if (LastServicesCell == "(NEW)")
                {
                    AddRowProduct1(RowsCount);
                    //Clear (NEW) on other colums as well
                    //ClearNew(RowsCount - 1);
                }
            }
            hasChanged = true;
        }
        private void CalculateNetAmount()
        {
            try
            {
                NetAmount = 0;
                for (int i = 1; i < grdPurchaseOrder.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row.If it is ,it must be the last row.

                    if (i == grdPurchaseOrder.Rows.Count)
                        return;
                    double m_NetAmount = 0;
                    string m_Value = Convert.ToString(grdPurchaseOrder[i, (int)GridColumn.Amount].Value);
                    if (m_Value.Length == 0)
                        m_NetAmount = 0;
                    else
                        m_NetAmount = Convert.ToDouble(grdPurchaseOrder[i, (int)GridColumn.Amount].Value);

                    NetAmount += m_NetAmount;
                    double VatAmt = 0;

                    VatAmt = (NetAmount * 13) / 100;
                    //lblVat.Text = VatAmt.ToString();
                    lblNetAmout.Text = NetAmount.ToString();

                }
            }
            catch (Exception ex)
            {

                Global.MsgError("Error in NetAmount calucation!");
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            PurchaseInvoiceIDCopy = Convert.ToInt32(txtPurchaseOrderID.Text);
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

        private void frmPurchaseOrder_KeyDown(object sender, KeyEventArgs e)
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
                // btnEdit_Click(sender, e);
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
                //  btnPrint_Click(sender, e);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            isNew = false;
            bool chkUserPermission = UserPermission.ChkUserPermission("PURCHASE_ORDER_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            OldGrid = " ";
            OldGrid = OldGrid + "Voucher No" + txtDate.Text + "Series" + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Cash/Party" + cboCashParty.Text + "OrderNo" + txtOrderNo.Text;
            for (int i = 0; i < grdPurchaseOrder.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string productname = grdPurchaseOrder[i + 1, 3].Value.ToString();
                string qty = grdPurchaseOrder[i + 1, 4].Value.ToString();
                string rate = grdPurchaseOrder[i + 1, 5].Value.ToString();
                string amt = grdPurchaseOrder[i + 1, 6].Value.ToString();
                OldGrid = OldGrid + string.Concat(productname, qty, rate, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;
            ChangeState(EntryMode.EDIT);
            EnableControls(true);
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

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdPurchaseOrder.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            string AccName = (string)(grdPurchaseOrder[RowCount - 1, 3].Value);
            //Check if the input value is correct
            if (grdPurchaseOrder[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == "" || grdPurchaseOrder[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == null)
            {
                grdPurchaseOrder[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            }

            double checkformat = Convert.ToDouble(grdPurchaseOrder[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value.ToString());
            string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdPurchaseOrder[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = insertvalue;

            if (AccName != "(NEW)")
            {
                AddRowProduct(RowCount);
            }
        }


        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }
        private void PrintPreviewCR(PrintType myPrintType)
        {

            dsPurchaseOrderNew.Clear();
            rptPurchaseOrder rpt = new rptPurchaseOrder();
            Misc.WriteLogo(dsPurchaseOrderNew, "tblImage");
            rpt.SetDataSource(dsPurchaseOrderNew);

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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            //bool chkUserPermission = UserPermission.ChkUserPermission("SALE_RETURN_DELETE");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
            //    return;
            //}
            //string GridValues = "";
            //GridValues = GridValues + "Voucher No" + "-" + txtVoucherNo.Text + "," + "Voucher Date" + "-" + txtDate.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + "," + "Cash/Party" + "-" + cboCashParty.Text + "," + "Depot" + "-" + cboDepot.Text + "," + "OrderNo" + "-" + txtOrderNo.Text + ",";
            ////Collect the Contents of the grid for audit log
            //for (int i = 0; i < grdSalesReturn.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            //{
            //    string productname = grdSalesReturn[i + 1, (int)GridColumn.ProductName].Value.ToString();
            //    string qty = grdSalesReturn[i + 1, (int)GridColumn.Qty].Value.ToString();
            //    string rate = grdSalesReturn[i + 1, (int)GridColumn.SalesRate].Value.ToString();
            //    string amt = grdSalesReturn[i + 1, (int)GridColumn.Amount].Value.ToString();
            //    GridValues = GridValues + string.Concat(productname, qty, rate, amt);
            //}
            //GridValues = "GridValues" + GridValues;
            //ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            //try
            //{
            //    //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
            //    if (Global.MsgQuest("Are you sure you want to delete the Invoice - " + txtSalesReturnID.Text + "?") == DialogResult.Yes)
            //    {

            //        SalesReturn Delsalesreturn = new SalesReturn();
            //        //Sales DelSales = new Sales();
            //        if (Delsalesreturn.RemoveSalesReturnEntry(Convert.ToInt32(txtSalesReturnID.Text), GridValues))
            //        {
            //            Global.Msg("Return -" + txtSalesReturnID.Text + " deleted successfully!");
            //            // Navigate to 1 step previous
            //            if (!this.Navigation(Navigate.Prev))
            //            {
            //                //This must be because there are no records or this was the first one
            //                //If this was the first, try to navigate to second
            //                if (!this.Navigation(Navigate.Next))
            //                {
            //                    //This was the last one, there are no records left. Simply clear the form and stay calm
            //                    ChangeState(EntryMode.NEW);
            //                }
            //            }
            //        }
            //        else
            //            Global.MsgError("There was an error while deleting Return -" + txtSalesReturnID.Text + "!");
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            m_MDIForm.OpenFormArrayParam("frmAccClass");
        }
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
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "SALES", m_dtRecurringSetting);
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
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "SALES"); // delete from database
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
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "SALES", m_dtRecurringSetting);
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
        private void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                // check if the user has permission to view the voucher
                if (!UserPermission.ChkUserPermission("PURCHASE_ORDER_VIEW"))
                {
                    Global.MsgError("Sorry! you donot have permission to view Purchase Order. Please contact your administrator for permission.");
                    return;
                }
                string[] vouchValues = new string[5];
                vouchValues[0] = "PURCHASE_ORDER";               // voucherType
                vouchValues[1] = "Inv.tblPurchaseOrderMaster";   // master tableName for the given voucher type  
                vouchValues[2] = "Inv.tblPurchaseOrderDetails";  // details tableName for the given voucher type
                vouchValues[3] = "PurchaseOrderID";              // master ID for the given master table
                vouchValues[4] = "PurchaseOrder_Date";              // date field for a given voucher

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
            m_PurchaseOrderID = VoucherID;
            txtPurchaseOrderID.Text = VoucherID.ToString();
            Navigation(Navigate.ID);
           // frmPurchaseOrder_Load(null, null);
        }
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
