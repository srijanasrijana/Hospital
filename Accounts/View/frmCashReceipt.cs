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
using System.IO;
using System.Data.SqlClient;
using CrystalDecisions.Shared;
using Common;
using Accounts.Reports;


namespace Accounts
{
    public partial class frmCashReceipt : Form, IfrmAddAccClass, IfrmDateConverter, ILOVLedger, IVoucherRecurring, IVoucherList, IVoucherReference

    {

        //This is to record column name according to the column number
        private enum GridColumnCR : int
        {
            CRDel = 0, CRCode_No, CRParticular_Account_Head, CRAmount, CRCurrent_Balance, CRRemarks, CRV_Type, CRVoucher_No, CRLedger_ID, CRCurrent_Bal_Actual, CRRef_Amt
        };

        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private int DefaultAccClass;
        private string Prefix = "";
        //Check if the Account has been already set by LOVLedger
        bool hasChanged = false;
        private int CurrAccLedgerID = 0;
        private string CurrBal = "";
        private int CurrRowPos = 0;
        private int OnlyReqdDetailRows = 0;
        private bool IsFieldChanged = false;
        private int currRowPosition = 0;
        ListItem liProjectID = new ListItem();
        ListItem liCashID = new ListItem();
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private bool IsNegativeCash = false;
        private bool IsNegativeBank = false;
        private int CashReceiptIDCopy = 0;
        ListItem SeriesID = new ListItem();
        private bool IsShortcutKey = false;
        private int loopCounter = 0;
        decimal totalAmt = 0;
        ArrayList AccountClassID = new ArrayList();
        string totalRptAmt = "";
        List<int> AccClassID = new List<int>();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private Accounts.Model.dsCashReceipt dsCashReceipt = new Model.dsCashReceipt();
        ListItem LiProjectID = new ListItem();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        DataTable dtAccClassID = new DataTable();
        SourceGrid.Cells.Button CRbtnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents CRevtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents CRevtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents CRevtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents CRevtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        CashReceipt m_CashReceipt = new CashReceipt();
        private int m_CashReceiptID = 0;
        DataTable dtGetOpeningBalance = new DataTable();
        //For Export Menu
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
        private object CashReceiptXMLString;
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        public frmCashReceipt()
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
        public frmCashReceipt(int CashReceiptID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_CashReceiptID = CashReceiptID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }
        public frmCashReceipt(int CashReceiptID)
        {
            InitializeComponent();
            this.m_CashReceiptID = CashReceiptID;
        }

        public void AddLedger(string LedgerName, int LedgerID, string CurrentBalance, bool IsSelected, string ActualBal, string LedgerType)
        {
            if (IsSelected)
            {
                ctx.Text = LedgerName;
                CurrAccLedgerID = LedgerID;
                CurrBal = CurrentBalance;
            }
            hasChanged = true;
        }

        public void DateConvert(DateTime DotNetDate)
        {
            CRtxtDate.Text = Date.ToSystem(DotNetDate);
        }

        private int ChildCount(int AccClassID)
        {
            try
            {
                int m_RecCount = (int)User.GetAccessInfo(AccClassID).Rows.Count;
                return m_RecCount;
            }
            catch
            {
                throw;
            }
        }

        //Customized header
        private class MyHeaderCR : SourceGrid.Cells.ColumnHeader
        {
            public MyHeaderCR(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 9, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;

                AutomaticSortEnabled = false;
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

        private void frmCashReceipt_Load(object sender, EventArgs e)
        {
            AddReferenceColumns();
            if (CRtxtCashReceiptID.Text != "")
                dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(CRtxtCashReceiptID.Text), "CASH_RCPT");
            CRchkDoNotClose.Checked = true;
            CRChangeState(EntryMode.NEW);
            LoadComboboxProject(CRcboProjectName, 0);
            //ListProject(cboProjectName);
            ShowAccClassInTreeView(treeAccClass, null, 0);

            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            CRtxtDate.Mask = Date.FormatToMask();
            CRtxtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.

            //For Loading The Optional Fields
            CROptionalFields();
            try
            {
                #region Cash Account according to User Setting

                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int CashID = AccountGroup.GetGroupIDFromGroupNumber(102);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultCash Account according to user root or other users
                int DefaultCashAccNum = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_CASH_ACCOUNT") : UserPreference.GetValue("DEFAULT_CASH_ACCOUNT", uid));
                string DefaultCashName = "";

                //Add Cash to comboCashAccount
                DataTable dtCashLedgers = Ledger.GetAllLedger(CashID);
                foreach (DataRow drCashLedgers in dtCashLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCashLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    CRcmboCashAccount.Items.Add(new ListItem((int)drCashLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drCashLedgers["LedgerID"]) == DefaultCashAccNum)
                        DefaultCashName = drLedgerInfo["LedName"].ToString();
                }

                CRcmboCashAccount.DisplayMember = "value";//This value is  for showing at Load condition
                CRcmboCashAccount.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                CRcmboCashAccount.SelectedText = DefaultCashName;
                //cmboCashAccount.Text = DefaultCashName;

                #endregion

                #region Customevents mainly for saving purpose
                CRcboSeriesName.SelectedIndexChanged += new EventHandler(CRText_Change);
                CRcmboCashAccount.SelectedIndexChanged += new EventHandler(CRText_Change);
                CRtxtVchNo.TextChanged += new EventHandler(CRText_Change);
                CRtxtDate.TextChanged += new EventHandler(CRText_Change);
                CRbtnDate.Click += new EventHandler(CRText_Change);
                CRcboProjectName.SelectedIndexChanged += new EventHandler(CRText_Change);
                CRtxtRemarks.TextChanged += new EventHandler(CRText_Change);

                //Event trigerred when delete button is clicked
                CRevtDelete.Click += new EventHandler(CRDelete_Row_Click);
                CRevtAmountFocusLost.FocusLeft += new EventHandler(CRAmount_Focus_Lost);
                CRevtAccountFocusLost.FocusLeft += new EventHandler(CRevtAccountFocusLost_FocusLeft);
                #endregion

                grdCashReceipt.Redim(2, 11);
                CRbtnRowDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;

                //Prepare the header part for grid
                CRAddGridHeader();
                CRAddRowCashReceipt(1);

                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID


                if (m_CashReceiptID > 0)
                {
                    //Show the values in fields

                    try
                    {

                        CRChangeState(EntryMode.NORMAL);

                        int vouchID = 0;
                        try
                        {
                            if (m_isRecurring)
                            {
                                CRChangeState(EntryMode.NEW);
                            }
                            else
                                CRChangeState(EntryMode.NORMAL);
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }

                        CashReceipt m_CashReceipt = new CashReceipt();


                        if (m_CashReceiptID != 0)
                            vouchID = m_CashReceiptID;

                        //Getting the value of SeriesID via MasterID or VouchID

                        int SeriesIDD = m_CashReceipt.GetSeriesIDFromMasterID(vouchID);


                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Cash Receipt");
                            CRcboSeriesName.Text = "";

                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            CRcboSeriesName.Text = dr["EngName"].ToString();

                        }

                        DataTable dtCashReceiptMaster = m_CashReceipt.GetCashReceiptMaster(vouchID);


                        if (dtCashReceiptMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }


                        DataRow drCashReceiptMaster = dtCashReceiptMaster.Rows[0];
                        if (m_CashReceiptID != 0)
                        {
                            CRtxtVchNo.Text = drCashReceiptMaster["Voucher_No"].ToString();
                            CRtxtDate.Text = Date.DBToSystem(drCashReceiptMaster["CashReceipt_Date"].ToString());
                        }
                        else
                        {
                            CRtxtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if recurring load today's date
                            //txtduedate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                        }

                        CRtxtRemarks.Text = drCashReceiptMaster["Remarks"].ToString();
                        CRtxtCashReceiptID.Text = drCashReceiptMaster["CashReceiptID"].ToString();
                        dsCashReceipt.Tables["tblCashReceiptMaster"].Rows.Add(CRcboSeriesName.Text, drCashReceiptMaster["Voucher_No"].ToString(), Date.DBToSystem(drCashReceiptMaster["CashReceipt_Date"].ToString()), drCashReceiptMaster["Remarks"].ToString());
                        DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drCashReceiptMaster["ProjectID"]), LangMgr.DefaultLanguage);
                        if (dtProjectInfo.Rows.Count > 0)
                        {
                            DataRow drProjectInfo = dtProjectInfo.Rows[0];
                            CRcboProjectName.Text = drProjectInfo["Name"].ToString();
                        }
                        //Show the corresponding Bank Account Ledger in Combobox
                        DataTable dtCashLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCashReceiptMaster["LedgerID"]), LangMgr.DefaultLanguage);
                        DataRow drCashLedgerInfo = dtCashLedgerInfo.Rows[0];

                        CRcmboCashAccount.Text = drCashLedgerInfo["LedName"].ToString();
                        //For Additional Fields
                        if (NumberOfFields > 0)
                        {
                            if (NumberOfFields == 1)
                            {
                                CRlblfirst.Visible = true;
                                CRtxtfirst.Visible = true;
                                CRlblsecond.Visible = false;
                                CRtxtsecond.Visible = false;
                                CRlblthird.Visible = false;
                                CRtxtthird.Visible = false;
                                CRlblfourth.Visible = false;
                                CRtxtfourth.Visible = false;
                                CRlblfifth.Visible = false;
                                CRtxtfifth.Visible = false;
                                CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();

                                CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                                CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                                CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                                CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                                CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();
                            }
                            else if (NumberOfFields == 2)
                            {
                                CRlblfirst.Visible = true;
                                CRtxtfirst.Visible = true;
                                CRlblsecond.Visible = true;
                                CRtxtsecond.Visible = true;
                                CRlblthird.Visible = false;
                                CRtxtthird.Visible = false;
                                CRlblfourth.Visible = false;
                                CRtxtfourth.Visible = false;
                                CRlblfifth.Visible = false;
                                CRtxtfifth.Visible = false;
                                CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();

                                CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                                CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                                CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                                CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                                CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();
                            }
                            else if (NumberOfFields == 3)
                            {
                                CRlblfirst.Visible = true;
                                CRtxtfirst.Visible = true;
                                CRlblsecond.Visible = true;
                                CRtxtsecond.Visible = true;
                                CRlblthird.Visible = true;
                                CRtxtthird.Visible = true;
                                CRlblfourth.Visible = false;
                                CRtxtfourth.Visible = false;
                                CRlblfifth.Visible = false;
                                CRtxtfifth.Visible = false;
                                CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                CRlblthird.Text = drdtadditionalfield["Field3"].ToString();

                                CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                                CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                                CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                                CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                                CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();

                            }
                            else if (NumberOfFields == 4)
                            {
                                CRlblfirst.Visible = true;
                                CRtxtfirst.Visible = true;
                                CRlblsecond.Visible = true;
                                CRtxtsecond.Visible = true;
                                CRlblthird.Visible = true;
                                CRtxtthird.Visible = true;
                                CRlblfourth.Visible = true;
                                CRtxtfourth.Visible = true;
                                CRlblfifth.Visible = false;
                                CRtxtfifth.Visible = false;
                                CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                CRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                                CRlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                                CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                                CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                                CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                                CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                                CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();

                            }
                            else if (NumberOfFields == 5)
                            {
                                CRlblfirst.Visible = true;
                                CRtxtfirst.Visible = true;
                                CRlblsecond.Visible = true;
                                CRtxtsecond.Visible = true;
                                CRlblthird.Visible = true;
                                CRtxtthird.Visible = true;
                                CRlblfourth.Visible = true;
                                CRtxtfourth.Visible = true;
                                CRlblfifth.Visible = true;
                                CRtxtfifth.Visible = true;

                                CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                CRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                                CRlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                                CRlblfifth.Text = drdtadditionalfield["Field5"].ToString();

                                CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                                CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                                CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                                CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                                CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();
                            }


                        }

                        DataTable dtCashReceiptDetail = m_CashReceipt.GetCashReceiptDetail(vouchID);
                        for (int i = 1; i <= dtCashReceiptDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtCashReceiptDetail.Rows[i - 1];

                            grdCashReceipt[i, (int)GridColumnCR.CRCode_No].Value = i.ToString();
                            grdCashReceipt[i, (int)GridColumnCR.CRParticular_Account_Head].Value = drDetail["LedgerName"].ToString();
                            grdCashReceipt[i, (int)GridColumnCR.CRAmount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                            grdCashReceipt[i, (int)GridColumnCR.CRRemarks].Value = drDetail["Remarks"].ToString();
                            grdCashReceipt[i, (int)GridColumnCR.CRV_Type].Value = drDetail["VoucherType"].ToString();
                            grdCashReceipt[i, (int)GridColumnCR.CRVoucher_No].Value = drDetail["VoucherNumber"].ToString();
                            grdCashReceipt[i, (int)GridColumnCR.CRLedger_ID].Value = drDetail["LedgerID"].ToString();
                            //AddRowCashReceipt(grdCashReceipt.RowsCount);
                            //dsCashReceipt.Tables["tblCashReceiptDetails"].Rows.Add(drDetail["LedgerName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                            //Code To Get The Current Balance of the Respective Ledger
                            string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
                            string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
                            DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(drDetail["LedgerID"]), null);
                            if (dtLdrInfo.Rows.Count != 1)
                            {
                                grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Bal_Actual].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            }
                            else
                            {

                                DataRow drLdrInfo = dtLdrInfo.Rows[0];//Get first record
                                decimal Debit = 0;
                                decimal Credit = 0;
                                decimal DebitOpeningBal = 0;
                                decimal CreditOpeningBal = 0;
                                if (!(drLdrInfo["DebitTotal"] is DBNull))
                                {
                                    Debit = Convert.ToDecimal(drLdrInfo["DebitTotal"]);
                                }
                                else
                                    Debit = 0;

                                if (!(drLdrInfo["CreditTotal"] is DBNull))
                                {
                                    Credit = Convert.ToDecimal(drLdrInfo["CreditTotal"]);
                                }
                                else
                                    Credit = 0;

                                if (!(drLdrInfo["OpenBalDr"] is DBNull))
                                {
                                    DebitOpeningBal = Convert.ToDecimal(drLdrInfo["OpenBalDr"]);
                                }
                                else
                                    DebitOpeningBal = 0;

                                if (!(drLdrInfo["OpenBalCr"] is DBNull))
                                {
                                    CreditOpeningBal = Convert.ToDecimal(drLdrInfo["OpenBalCr"]);
                                }
                                else
                                    CreditOpeningBal = 0;

                                //Calculate Debit and Credit Totals
                                decimal DebitTotal = Debit + DebitOpeningBal;
                                decimal CreditTotal = Credit + CreditOpeningBal;

                                decimal Balance = DebitTotal - CreditTotal;
                                string strBalance = "";

                                /*//////TODO///////////
                                1. Make a function to check whether the given ledger ID is Debit Type or Credit Type
                                2. If its Debit Type and Balance is zero, show Dr. else show Cr. if balance is zero
                                */
                                //If +ve is present, show as Dr
                                strBalance = ((Balance < 0) ? Balance * -1 : Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                if (Balance >= 0)
                                    strBalance = strBalance + " (Dr.)";

                                else //If balance is -ve, its Cr.
                                    strBalance = strBalance + " (Cr.)";


                                //Write balance into the grid
                                grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Balance].Value = strBalance;
                                grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Bal_Actual].Value = Balance.ToString();

                            }

                            CRAddRowCashReceipt(grdCashReceipt.RowsCount);
                            dsCashReceipt.Tables["tblCashReceiptDetails"].Rows.Add(drDetail["LedgerName"].ToString(), (Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                            totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                            totalRptAmt = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();

                        }

                        DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(CRtxtCashReceiptID.Text), "CASH_RCPT");
                        List<int> AccClassIDs = new List<int>();
                        foreach (DataRow dr in dtAccClassDtl.Rows)
                        {
                            AccClassIDs.Add(Convert.ToInt32(dr["AccClassID"]));
                        }
                        treeAccClass.ExpandAll();

                        //Check for the treeview if it has Use
                        foreach (TreeNode tn in treeAccClass.Nodes)
                        {
                            LoadAccClassInfo(vouchID, tn, AccClassIDs.ToArray<int>(), treeAccClass);
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
                        btnExport.Enabled = true;
                    }


                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }

                #endregion
                // if recurring is true then donot load recurring settings for new voucher
                if (!m_isRecurring)
                    CRCheckRecurringSetting(CRtxtCashReceiptID.Text);
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
            IsFieldChanged = false;
        }

        /// <summary>
        /// Check whether user wants to close the form without saving. Returns true if he wants to navigate away without saving.
        /// </summary>
        /// <returns></returns>
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
            return true;
        }

        /// <summary>
        /// This is the common text change event for all controls just to track if any changes has been made. This will help to show a save dialogue box while closing the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CRText_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
        }

        private void  CRDelete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = grdCashReceipt.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdCashReceipt.RowsCount - 2)
            {
                #region Reference related
                if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(CRtxtCashReceiptID.Text), Convert.ToInt32(grdCashReceipt[CurRow, (int)GridColumnCR.CRLedger_ID].Value), "CASH_RCPT"))
                {
                    Global.MsgError("You must delete all other vouchers with reference against this voucher to delete this transaction!");
                    return;
                }

                #endregion

                grdCashReceipt.Rows.Remove(ctx.Position.Row);
            }
        }

        

        private void CRAccount_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
        }

        bool isNewReferenceVoucher = false, isAgainstRef = false;
        private void CRAccount_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                int CurRow = grdCashReceipt.Selection.GetSelectionRegion().GetRowsIndex()[0];

                #region Reference related
                // if new voucher reference is created for this, donot open the reference form 
                if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(CRtxtCashReceiptID.Text), Convert.ToInt32(grdCashReceipt[CurRow, (int)GridColumnCR.CRLedger_ID].Value), "CASH_RCPT"))
                {
                    //Global.MsgError("You must delete all other vouchers with reference against this voucher to delete this transaction!");
                    isNewReferenceVoucher = true;
                    return;
                }
                isNewReferenceVoucher = false;
                #endregion

                //if (ctx.Text == "(NEW)") ctx.Text = "";
                frmLOVLedger frm = new frmLOVLedger(this);
                frm.ShowDialog();
                SendKeys.Send("{Tab}");
            }
        }

        private void CRevtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            int ledID = 0, vouchID = 0;
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            int ledgerID = 0;
            string CurrentBal = "";
            try
            {
                if (grdCashReceipt[CurrRowPos, 2].Value.ToString() == "(NEW)" || grdCashReceipt[CurrRowPos, 2].Value.ToString() == "")
                    return;

                try
                {
                    ledgerID = Convert.ToInt32(grdCashReceipt[CurrRowPos, (int)GridColumnCR.CRLedger_ID].Value);
                    CurrentBal = grdCashReceipt[CurrRowPos, (int)GridColumnCR.CRCurrent_Bal_Actual].Value.ToString();
                }
                catch
                {
                    ledgerID = 0;
                }
                if (ledgerID != 0 && CurrAccLedgerID == 0)
                {
                    CurrAccLedgerID = ledgerID;
                    CurrBal = CurrentBal;
                }
            }
            catch
            {
                return;
            }
            #region Reference related


            if (m_mode == EntryMode.NEW) ledID = CurrAccLedgerID;
            else ledID = ledgerID; //Convert.ToInt32(grdCashReceipt[CurrRowPos, (int)GridColumn.Ledger_ID].Value);
            if (VoucherReference.CheckIfReferece(ledID) && !isNewReferenceVoucher) // if isBillReference is true for given ledger then load the reference form
            {
                Form fc = Application.OpenForms["frmReference"];

                if (fc != null)
                    fc.Close();

                if (CRtxtCashReceiptID.Text != "")
                {
                    vouchID = Convert.ToInt32(CRtxtCashReceiptID.Text);
                }

                frmReference fr = new frmReference(this, vouchID, "CASH_RCPT", ledID);
                fr.ShowDialog();
                //SendKeys.Send("{Tab}");
            }
            #endregion

            CRFillAllGridRow(CurrRowPos, CurrAccLedgerID, CurrBal);
        }

        private void CRAccount_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmLOVLedger frm = new frmLOVLedger(this, e);
            frm.ShowDialog();
        }

        private void CRAmount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdCashReceipt.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            string AccName = (string)(grdCashReceipt[RowCount - 1, 2].Value);
            //Check if the input value is correct
            if (grdCashReceipt[Convert.ToInt32(CurRow), 3].Value == "" || grdCashReceipt[Convert.ToInt32(CurRow), 3].Value == null)
                grdCashReceipt[Convert.ToInt32(CurRow), 3].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            if (!Misc.IsNumeric(grdCashReceipt[Convert.ToInt32(CurRow), 3].Value))
            {
                Global.MsgError("Invalid Amount!");
                ctx.Value = "";
                return;
            }

            #region Reference related
            if (m_mode == EntryMode.EDIT)
            {
                grdCashReceipt[CurRow, (int)GridColumnCR.CRRef_Amt].Value = VoucherReference.GetAmtForAgainstRef(Convert.ToInt32(CRtxtCashReceiptID.Text), "CASH_RCPT", Convert.ToInt32(grdCashReceipt[CurRow, (int)GridColumnCR.CRLedger_ID].Value));
            }

            // check if transaction amount is greater than reference amount
            if (!CRCheckAmtAgainstRefAmt())
            {
                return;
            }
            #endregion

            CRFillGridRowExceptLedgerID(CurRow, CurrAccLedgerID, CurrBal);
            double checkformat = Convert.ToDouble(grdCashReceipt[Convert.ToInt32(CurRow), 3].Value.ToString());
            //string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdCashReceipt[Convert.ToInt32(CurRow), 3].Value = checkformat;
            if (AccName != "(NEW)")
            {
                CRAddRowCashReceipt(RowCount);
            }
        }

        //Calculate amount and fill on all column on Account focus lost event
        private void CRFillAllGridRow(int RowPosition, int LdrID, string CurrBal)
        {
            decimal TempAmount = 0;
            string CurrentLedgerBalance = CurrBal.ToString();
            string[] CurrentBalance = CurrBal.ToString().Split('(');

            try
            {
                TempAmount = Convert.ToDecimal(grdCashReceipt[Convert.ToInt32(RowPosition), 3].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                grdCashReceipt[Convert.ToInt32(RowPosition), (int)GridColumnCR.CRCurrent_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }

            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                grdCashReceipt[Convert.ToInt32(RowPosition), (int)GridColumnCR.CRCurrent_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                grdCashReceipt[Convert.ToInt32(RowPosition), (int)GridColumnCR.CRCurrent_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else
            {
                grdCashReceipt[RowPosition, (int)GridColumnCR.CRCurrent_Balance].Value = CurrBal;
            }
            grdCashReceipt[RowPosition, (int)GridColumnCR.CRLedger_ID].Value = LdrID;
            grdCashReceipt[RowPosition, (int)GridColumnCR.CRCurrent_Bal_Actual].Value = CurrBal;

        }

        //Calculate and onlu fill or change value of current balance
        private void CRFillGridRowExceptLedgerID(int RowPosition, int LdrID, string CurrBal)
        {
            decimal TempAmount = 0;
            string CurrentLedgerBalance = "";
            string[] CurrentBalance = new string[2];
            if (CurrBal == "")
            {
                CurrentLedgerBalance = grdCashReceipt[Convert.ToInt32(RowPosition), (int)GridColumnCR.CRCurrent_Bal_Actual].ToString();
                CurrentBalance = grdCashReceipt[Convert.ToInt32(RowPosition), 7].ToString().Split('(');
            }
            else
            {
                CurrentLedgerBalance = CurrBal.ToString();
                CurrentBalance = CurrBal.ToString().Split('(');
            }

            try
            {
                TempAmount = Convert.ToDecimal(grdCashReceipt[Convert.ToInt32(RowPosition), 3].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                grdCashReceipt[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(CurrentBalance[0]) - Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }

            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                grdCashReceipt[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                grdCashReceipt[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else
            {
                grdCashReceipt[RowPosition, 4].Value = CurrBal;
            }
        }

        /// <summary>
        /// Writes the header part forgrdCashReceipt
        /// </summary>
        private void CRAddGridHeader()
        {
            grdCashReceipt[0, (int)GridColumnCR.CRDel] = new MyHeaderCR("Del");
            grdCashReceipt[0, (int)GridColumnCR.CRDel].Column.Width = 30;
            grdCashReceipt.Columns[(int)GridColumnCR.CRDel].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdCashReceipt[0, (int)GridColumnCR.CRCode_No] = new MyHeaderCR("Code No.");
            grdCashReceipt[0, (int)GridColumnCR.CRCode_No].Column.Width = 60;
            grdCashReceipt.Columns[(int)GridColumnCR.CRCode_No].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;


            grdCashReceipt[0, (int)GridColumnCR.CRParticular_Account_Head] = new MyHeaderCR("Particular/Accounting Head");
            grdCashReceipt[0, (int)GridColumnCR.CRParticular_Account_Head].Column.Width = 200;
            grdCashReceipt.Columns[(int)GridColumnCR.CRParticular_Account_Head].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;


            grdCashReceipt[0, (int)GridColumnCR.CRAmount] = new MyHeaderCR("Amount");
            grdCashReceipt[0, (int)GridColumnCR.CRAmount].Column.Width = 100;
            grdCashReceipt.Columns[(int)GridColumnCR.CRAmount].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;


            grdCashReceipt[0, (int)GridColumnCR.CRCurrent_Balance] = new MyHeaderCR("Current Balance");
            grdCashReceipt[0, (int)GridColumnCR.CRCurrent_Balance].Column.Width = 100;
            grdCashReceipt.Columns[(int)GridColumnCR.CRCurrent_Balance].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;


            grdCashReceipt[0, (int)GridColumnCR.CRRemarks] = new MyHeaderCR("Remarks");
            grdCashReceipt[0, (int)GridColumnCR.CRRemarks].Column.Width = 150;
            grdCashReceipt.Columns[(int)GridColumnCR.CRRemarks].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;


            grdCashReceipt[0, (int)GridColumnCR.CRV_Type] = new MyHeaderCR("V Type");
            grdCashReceipt[0, (int)GridColumnCR.CRV_Type].Column.Width = 100;
            grdCashReceipt.Columns[(int)GridColumnCR.CRDel].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;


            grdCashReceipt[0, (int)GridColumnCR.CRVoucher_No] = new MyHeaderCR("Voucher No.");
            grdCashReceipt[0, (int)GridColumnCR.CRVoucher_No].Column.Width = 100;
            grdCashReceipt.Columns[(int)GridColumnCR.CRV_Type].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;


            grdCashReceipt[0, (int)GridColumnCR.CRLedger_ID] = new MyHeaderCR("Ledger ID");
            grdCashReceipt[0, (int)GridColumnCR.CRLedger_ID].Column.Width = 80;
            grdCashReceipt[0, (int)GridColumnCR.CRLedger_ID].Column.Visible = false;

            grdCashReceipt[0, (int)GridColumnCR.CRCurrent_Bal_Actual] = new MyHeaderCR("Current Balance");
            grdCashReceipt[0, (int)GridColumnCR.CRCurrent_Bal_Actual].Column.Width = 80;
            grdCashReceipt[0, (int)GridColumnCR.CRCurrent_Bal_Actual].Column.Visible = false;

            grdCashReceipt[0, (int)GridColumnCR.CRRef_Amt] = new SourceGrid.Cells.ColumnHeader("RefAmt");
            grdCashReceipt[0, (int)GridColumnCR.CRRef_Amt].Column.Width = 50;
            grdCashReceipt[0, (int)GridColumnCR.CRRef_Amt].Column.Visible = false;

        }
        /// <summary>
        /// Adds the row in the Journal field
        /// </summary>
        private void CRAddRowCashReceipt(int RowCount)
        {
            //Add a new row
            grdCashReceipt.Redim(Convert.ToInt32(RowCount + 1), grdCashReceipt.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            grdCashReceipt[i, (int)GridColumnCR.CRDel] = btnDelete;
            grdCashReceipt[i, (int)GridColumnCR.CRDel].AddController(CRevtDelete);

            grdCashReceipt[i, (int)GridColumnCR.CRCode_No] = new SourceGrid.Cells.Cell(i.ToString());



            //Account Head/Particulars
            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdCashReceipt[i, (int)GridColumnCR.CRParticular_Account_Head] = new SourceGrid.Cells.Cell("", txtAccount);
            txtAccount.Control.GotFocus += new EventHandler(CRAccount_Focused);
            txtAccount.Control.LostFocus += new EventHandler(CRAccount_Leave);
            txtAccount.Control.KeyDown += new KeyEventHandler(CRAccount_KeyDown);
            txtAccount.Control.TextChanged += new EventHandler(CRText_Change);
            grdCashReceipt[i, (int)GridColumnCR.CRParticular_Account_Head].AddController(CRevtAccountFocusLost);
            grdCashReceipt[i, (int)GridColumnCR.CRParticular_Account_Head].Value = "(NEW)";


            //Amount
            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdCashReceipt[i, (int)GridColumnCR.CRAmount] = new SourceGrid.Cells.Cell("", txtAmount);
            txtAmount.Control.TextChanged += new EventHandler(CRText_Change);
            grdCashReceipt[i, (int)GridColumnCR.CRAmount].AddController(CRevtAmountFocusLost);
            grdCashReceipt[i, (int)GridColumnCR.CRAmount].Value = "";


            //Current Balance
            SourceGrid.Cells.Editors.TextBox txtCurrentBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrentBalance.EditableMode = SourceGrid.EditableMode.None;
            grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Balance] = new SourceGrid.Cells.Cell("", txtCurrentBalance);
            grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Balance].Value = "";


            //Remarks
            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            txtRemarks.Control.TextChanged += new EventHandler(CRText_Change);
            grdCashReceipt[i, (int)GridColumnCR.CRRemarks] = new SourceGrid.Cells.Cell("", txtRemarks);
            grdCashReceipt[i, (int)GridColumnCR.CRRemarks].Value = "";


            //Voucher Type
            SourceGrid.Cells.Editors.ComboBox combo = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            combo.StandardValues = new string[] { "SALES", "JRNL", "PURCH_RTN", "CASH_PMNT", "BANK_PMNT", "NONE" };
            combo.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.EditableMode = SourceGrid.EditableMode.Focus;

            grdCashReceipt[i, (int)GridColumnCR.CRV_Type] = new SourceGrid.Cells.Cell("", combo);


            //Voucher Number
            SourceGrid.Cells.Editors.TextBox txtVoNo = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtVoNo.EditableMode = SourceGrid.EditableMode.Focus;
            grdCashReceipt[i, (int)GridColumnCR.CRVoucher_No] = new SourceGrid.Cells.Cell("", txtVoNo);
            grdCashReceipt[i, (int)GridColumnCR.CRVoucher_No].Value = "";


            //Ledger ID
            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdCashReceipt[i, (int)GridColumnCR.CRLedger_ID] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdCashReceipt[i, (int)GridColumnCR.CRLedger_ID].Value = "";


            //Current Balance Actual
            SourceGrid.Cells.Editors.TextBox txtCurrBal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrBal.EditableMode = SourceGrid.EditableMode.None;
            grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Bal_Actual] = new SourceGrid.Cells.Cell("", txtCurrBal);
            grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Bal_Actual].Value = "";

            //For Reference Amount
            SourceGrid.Cells.Editors.TextBox txtRefAmt = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            grdCashReceipt[i, (int)GridColumnCR.CRRef_Amt] = new SourceGrid.Cells.Cell("", txtRefAmt);
            grdCashReceipt[i, (int)GridColumnCR.CRRef_Amt].Value = "0(Dr)";

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

        private void CRbtnSave_Click(object sender, EventArgs e)
        {
            if (CRCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("CASH_RECEIPT_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("CASH_RECEIPT_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }

            if (Freeze.IsDateFreeze(Date.ToDotNet(CRtxtDate.Text)))
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
                        string returnStr = m_VouConfig.ValidateManualVouNum(CRtxtVchNo.Text, Convert.ToInt32(SeriesID.ID));
                        switch (returnStr)
                        {
                            case "INVALID_SERIES":
                                MessageBox.Show("Invalid Series Name,please select valid Series Name and try again!");
                                return;

                            case "BLANK_WARN":
                                if (MessageBox.Show("Voucher Number is Blank, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                break;
                            case "BLANK_DONT_ALLOW":
                                MessageBox.Show("Voucher Number is Blank,Please fill the Voucher Number first!");
                                return;
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
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Msg(ex.Message);
                    }
                }
            }
            #endregion

            ListItem liLedgerID = new ListItem();
            liLedgerID = (ListItem)CRcmboCashAccount.SelectedItem;
            //Check Validation
            if (!CRValidate())
                return;


            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }
            if (drdtadditionalfield["IsField1Required"].ToString() == "True")
            {
                if (CRtxtfirst.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field1"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField2Required"].ToString() == "True")
            {
                if (CRtxtsecond.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field2"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField3Required"].ToString() == "True")
            {
                if (CRtxtthird.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field3"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField4Required"].ToString() == "True")
            {
                if (CRtxtfourth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field4"].ToString() + " " + "is Required Field");
                    return;
                }

            }
            if (drdtadditionalfield["IsField5Required"].ToString() == "True")
            {
                if (CRtxtfifth.Text == "")
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
                        SeriesID = (ListItem)CRcboSeriesName.SelectedItem;
                        string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumTypeNoUpdate(Convert.ToInt32(SeriesID.ID), out increasedSeriesNum);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers are totally finished!");
                                return;
                            }

                            CRtxtVchNo.Text = m_vounum.ToString();
                            CRtxtVchNo.Enabled = false;
                        }
                        #endregion

                        isNew = true;
                        NewGrid = " ";
                        OldGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + CRtxtVchNo.Text + "Series" + CRcboSeriesName.Text + "Project" + CRcboProjectName.Text + "Date" + CRtxtDate.Text + "Cash Account" + CRcmboCashAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdCashReceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdCashReceipt[i + 1, 2].Value.ToString();
                            string amt = grdCashReceipt[i + 1, 3].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        string CashReceiptXMLString = CRReadAllCashReceiptEntry();
                        if (CashReceiptXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast journal entry to XML!");
                            return;
                        }


                        #region block to check negative cash and bank
                        //Read from sourcegrid and store it to table
                        DataTable CashReceiptDetails = new DataTable();
                        CashReceiptDetails.Columns.Add("Ledger");
                        //CashReceiptDetails.Columns.Add("LedgerFolioNo");
                        //CashReceiptDetails.Columns.Add("BudgetHeadNo");
                        CashReceiptDetails.Columns.Add("Amount");
                        CashReceiptDetails.Columns.Add("Remarks");

                        double totalgrdAmount = 0;
                        for (int i = 0; i < grdCashReceipt.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdCashReceipt[i + 1, 2].ToString().Split('[');
                            CashReceiptDetails.Rows.Add(ledgerName[0].ToString(), grdCashReceipt[i + 1, 3].Value, grdCashReceipt[i + 1, 4].Value);
                            //Check for empty Amount
                            object objAmount = grdCashReceipt[i + 1, 3].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdCashReceipt[i + 1, 3].Value);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            //we need to check both negative cash and negative bank in Details section...All ledgers of Detail Section are resposible for payment
                            //Block for checking negative cash.
                            #region BLOCK FOR CHECKING NEGATIVE CASH
                            if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))//Only go through this section when setting have Warn and Deny for negative cash and negative bank
                            {
                                bool isCashAccount = false;
                                isCashAccount = Ledger.IsCashAccount((ledgerName[0].ToString()));//Check the grid's ledger is cash account or not?
                                if (isCashAccount)//If Bank account
                                {
                                    double mDbalCash = 0;
                                    double mCbalCash = 0;
                                    double totalDrCash = 0;
                                    double totalCrCash = 0;

                                    int CashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);
                                    Transaction.GetLedgerBalance(null, null, CashLedgerId, ref mDbalCash, ref mCbalCash, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                    //In case of Cash Receipt,Bank Account in master section is Debit and other accounts in Detail section are bydefault Credit
                                    totalDrCash = mDbalCash;
                                    totalCrCash += (mCbalCash + Convert.ToDouble(grdCashReceipt[i + 1, 3].Value));//The ledgers of Details section will be posted as Credit,soo tatal balance will be the summation of its own balance and grid value
                                    if ((totalDrCash - totalCrCash) >= 0)
                                        IsNegativeCash = false;
                                    else
                                        IsNegativeCash = true;

                                    #region NEGATIVE CASH SETTINGS
                                    switch (Global.Default_NegativeCash)
                                    {
                                        case NegativeCash.Allow:
                                            //if (IsNegativeCash == true)
                                            //Do nothing
                                            break;
                                        case NegativeCash.Warn:
                                            if (IsNegativeCash == true)
                                            {
                                                if (MessageBox.Show("Your Cash Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                                {
                                                    return;
                                                }
                                            }
                                            break;
                                        case NegativeCash.Deny:
                                            if (IsNegativeCash == true)
                                            {
                                                Global.MsgError("Your Cash amount is negative,you are not allowed to submit this voucher!!!");
                                                return;
                                            }
                                            break;

                                    }
                                    #endregion

                                }
                            }

                            #endregion

                            //Block for negative bank
                            #region BLOCK FOR CHECKING NEGATIVE BANK
                            if ((Global.Default_NegativeBank == NegativeBank.Warn) || (Global.Default_NegativeBank == NegativeBank.Deny))
                            {
                                bool isBankAccount = false;
                                isBankAccount = Ledger.IsBankAccount((ledgerName[0].ToString()));
                                if (isBankAccount)//If Bank account
                                {
                                    double mDbalBank = 0;
                                    double mCbalBank = 0;
                                    double totalDrBank = 0;
                                    double totalCrBank = 0;
                                    int bankLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);
                                    Transaction.GetLedgerBalance(null, null, bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                    //In case of Bank Receipt,Bank Account in master section is Debit and other accounts in Detail section are bydefault Credit
                                    totalDrBank = mDbalBank;//Ledgers of Details sections will be posted as Credit,so debit balance will its only own balance
                                    totalCrBank += (mCbalBank + Convert.ToDouble(grdCashReceipt[i + 1, 3].Value));
                                    if ((totalDrBank - totalCrBank) >= 0)
                                        IsNegativeBank = false;
                                    else
                                        IsNegativeBank = true;

                                    //If -ve Bank not allowed in settings
                                    #region NEGATIVE BANK SETTINNGS
                                    switch (Global.Default_NegativeBank)
                                    {
                                        case NegativeBank.Allow:
                                            //if (IsNegativeCash == true)
                                            //Do nothing
                                            break;
                                        case NegativeBank.Warn:
                                            if (IsNegativeBank == true)
                                            {
                                                if (MessageBox.Show("Your Bank Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                                    return;
                                            }
                                            break;
                                        case NegativeBank.Deny:
                                            if (IsNegativeBank == true)
                                            {
                                                Global.MsgError("Your Bank amount is negative,you are not allowed to submit this voucher!!!");
                                                return;
                                            }
                                            break;
                                    }
                                    #endregion

                                }
                            }

                            #endregion
                        }
                        //Incase of Receipt,master ledger is responsible for Receipt soo no need to check negative cash and negative Bank
                        #region BLOCK FOR CHECKING NEGATIVE CASH
                        /*
                        //WE need to check only negative Cash for master Cash Ledger because this ledger is bydefault Bank soo no need to check for negative Bank                        
                        #region BLOCK FOR CHECKING NEGATIVE CASH
                        //execute following code when only if setting of Negative Cash is in Warn or Deny
                        if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                        {
                            //Incase of CashReceipt and CashPayment master ledger is bydefault Cash soo we neednot to check ledger weather it falls uder Cash or not?? 
                            double mDbalCash = 0;
                            double mCbalCash = 0;
                            double totalDrCash, totalCrCash;
                            totalDrCash = totalCrCash = 0;
                            Transaction.GetLedgerBalance(Convert.ToInt32(liLedgerID.ID), ref mDbalCash, ref mCbalCash, arrNode);

                            //Incase of CashReceipt,master ledger is bydefulat cash...and here cash is receipt soo it would be Debit because cash is getting amount from other account
                            //Here Total Debit amount of master is calculated from Details section by adding all amount of all account in Details section

                            totalDrCash += (mDbalCash + totalgrdAmount);//
                            totalCrCash = mCbalCash;
                            if ((totalDrCash - totalCrCash) >= 0)
                                IsNegativeCash = false;
                            else
                                IsNegativeCash = true;

                            //If -ve cash not allowed in settings
                            #region NEGATIVE CASH SETTINGS
                            switch (Global.Default_NegativeCash)
                            {
                                case NegativeCash.Allow:
                                    //if (IsNegativeCash == true)
                                    //Do nothing
                                    break;
                                case NegativeCash.Warn:
                                    if (IsNegativeCash == true)
                                    {
                                        if (MessageBox.Show("Your Cash Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            return;
                                        }
                                    }
                                    break;
                                case NegativeCash.Deny:
                                    if (IsNegativeCash == true)
                                    {
                                        Global.MsgError("Your Cash amount is negative,you are not allowed to submit this voucher!!!");
                                        return;
                                    }
                                    break;

                            }
                            #endregion

                        }
                        #endregion                 
                        */
                        #endregion
                        #endregion

                        #region Save xml data to Database
                        int returnJournalId = 0;
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            //using (SqlCommand dbCommand = new SqlCommand("Acc.xmlCashReceiptInsert", Global.m_db.cn))
                            //{
                            //    // we are going to use store procedure  
                            //    dbCommand.CommandType = CommandType.StoredProcedure;
                            //    // Add input parameter and set its properties.
                            //    SqlParameter parameter = new SqlParameter();
                            //    // Store procedure parameter name  
                            //    parameter.ParameterName = "@cashreceipt";
                            //    // Parameter type as XML 
                            //    parameter.DbType = DbType.Xml;
                            //    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                            //    parameter.Value = CashReceiptXMLString; // XML string as parameter value  
                            //    // Add the parameter in Parameters collection.
                            //    dbCommand.Parameters.Add(parameter);
                            //    SqlParameter parameter1 = new SqlParameter();
                            //    // Store procedure parameter name  
                            //    parameter1.ParameterName = "@returnId";
                            //    // Parameter type as XML 
                            //    parameter1.DbType = DbType.Int32;
                            //    parameter1.Direction = ParameterDirection.Output; // Output Parameter
                            //    // Add the parameter in Parameters collection.
                            //    dbCommand.Parameters.Add(parameter1);

                            //    Global.m_db.cn.Open();
                            //    int intRetValue = dbCommand.ExecuteNonQuery();
                            //    returnJournalId = Convert.ToInt32(parameter1.Value);

                            //to insert CashReceipt
                                 returnJournalId = CashReceipt.InsertCashReceipt(CashReceiptXMLString);

                              // to send recurring settings
                                CashReceipt.InsertVoucherRecurring(m_dtRecurringSetting, returnJournalId);
                                //MessageBox.Show(intRetValue.ToString());   

                                // to send reference information
                                CashReceipt.InsertReference(returnJournalId, dtReference);
                            //}
                        }
                        #endregion
                        //Update the last AutoNumber in tblSeries,only if the voucher hide type is true
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }
                        Global.Msg("Cash Receipt created successfully!");
                        // if the voucher is recurring and has been posted or saved, modify voucherposting table to set isPosted = true
                        string res;
                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_CashReceiptID, "CASH_RECEIPT");
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID);
                            m_isRecurring = false;
                        }
                        // AccClassID.Clear();
                        CRClearVoucher();
                        CRChangeState(EntryMode.NEW);



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
                case EntryMode.EDIT: //if edit button is pressed
                    try
                    {
                        isNew = false;
                        NewGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + CRtxtVchNo.Text + "Series" + CRcboSeriesName.Text + "Project" + CRcboProjectName.Text + "Date" + CRtxtDate.Text + "Cash Account" + CRcmboCashAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdCashReceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdCashReceipt[i + 1, (int)GridColumnCR.CRParticular_Account_Head].Value.ToString();
                            string amt = grdCashReceipt[i + 1, (int)GridColumnCR.CRAmount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        //Call to Convert into XML Format

                        string CashReceiptXMLString = CRReadAllCashReceiptEntry();
                        if (CashReceiptXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast CashReceipt entry to XML!");
                            return;
                        }

                        #region block to check negative cash and bank
                        //Read from sourcegrid and store it to table
                        DataTable CashReceiptDetails = new DataTable();
                        CashReceiptDetails.Columns.Add("Ledger");
                        //CashReceiptDetails.Columns.Add("LedgerFolioNo");
                        //CashReceiptDetails.Columns.Add("BudgetHeadNo");
                        CashReceiptDetails.Columns.Add("Amount");
                        CashReceiptDetails.Columns.Add("Remarks");
                        double totalgrdAmount = 0;
                        for (int i = 0; i < grdCashReceipt.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdCashReceipt[i + 1, (int)GridColumnCR.CRParticular_Account_Head].ToString().Split('[');
                            CashReceiptDetails.Rows.Add(ledgerName[0].ToString(), grdCashReceipt[i + 1, (int)GridColumnCR.CRAmount].Value, grdCashReceipt[i + 1, (int)GridColumnCR.CRCurrent_Balance].Value);
                            //Check for empty Amount
                            object objAmount = grdCashReceipt[i + 1, (int)GridColumnCR.CRAmount].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdCashReceipt[i + 1, (int)GridColumnCR.CRAmount].Value);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            //we need to check both negative cash and negative bank in Details section...All ledgers of Details section are responsible for payment soo need to check both negative cash and negative Bank
                            //Block for checking negative cash.
                            #region BLOCK FOR CHECKING NEGATIVE CASH
                            if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))//Only go through this section when setting have Warn and Deny for negative cash and negative bank
                            {
                                bool isCashAccount = false;
                                isCashAccount = Ledger.IsCashAccount((ledgerName[0].ToString()));//Check the grid's ledger is cash account or not?
                                if (isCashAccount)//If Bank account
                                {
                                    double mDbalCash = 0;
                                    double mCbalCash = 0;
                                    double totalDrCash = 0;
                                    double totalCrCash = 0;

                                    int CashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);
                                    Transaction.GetLedgerBalance(null, null, CashLedgerId, ref mDbalCash, ref mCbalCash, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                    //In case of Cash Receipt,Bank Account in master section is Debit and other accounts in Detail section are bydefault Credit
                                    totalDrCash = mDbalCash;
                                    totalCrCash += (mCbalCash + Convert.ToDouble(grdCashReceipt[i + 1, (int)GridColumnCR.CRAmount].Value));//The ledgers of Details section will be posted as Credit,soo tatal balance will be the summation of its own balance and grid value
                                    if ((totalDrCash - totalCrCash) >= 0)
                                        IsNegativeCash = false;
                                    else
                                        IsNegativeCash = true;

                                    #region NEGATIVE CASH SETTINGS
                                    switch (Global.Default_NegativeCash)
                                    {
                                        case NegativeCash.Allow:
                                            //if (IsNegativeCash == true)
                                            //Do nothing
                                            break;
                                        case NegativeCash.Warn:
                                            if (IsNegativeCash == true)
                                            {
                                                if (MessageBox.Show("Your Cash Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                                {
                                                    return;
                                                }
                                            }
                                            break;
                                        case NegativeCash.Deny:
                                            if (IsNegativeCash == true)
                                            {
                                                Global.MsgError("Your Cash amount is negative,you are not allowed to submit this voucher!!!");
                                                return;
                                            }
                                            break;

                                    }
                                    #endregion

                                }
                            }

                            #endregion
                            //Block for negative bank
                            #region BLOCK FOR CHECKING NEGATIVE BANK
                            if ((Global.Default_NegativeBank == NegativeBank.Warn) || (Global.Default_NegativeBank == NegativeBank.Deny))
                            {
                                bool isBankAccount = false;
                                isBankAccount = Ledger.IsBankAccount((ledgerName[0].ToString()));
                                if (isBankAccount)//If Bank account
                                {
                                    double mDbalBank = 0;
                                    double mCbalBank = 0;
                                    double totalDrBank = 0;
                                    double totalCrBank = 0;
                                    int bankLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);
                                    Transaction.GetLedgerBalance(null, null, bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                    //In case of Bank Receipt,Bank Account in master section is Debit and other accounts in Detail section are bydefault Credit
                                    totalDrBank = mDbalBank;//Ledgers of Details sections will be posted as Credit,so debit balance will its only own balance
                                    totalCrBank += (mCbalBank + Convert.ToDouble(grdCashReceipt[i + 1, 3].Value));
                                    if ((totalDrBank - totalCrBank) >= 0)
                                        IsNegativeBank = false;
                                    else
                                        IsNegativeBank = true;

                                    //If -ve Bank not allowed in settings
                                    #region NEGATIVE BANK SETTINNGS
                                    switch (Global.Default_NegativeBank)
                                    {
                                        case NegativeBank.Allow:
                                            //if (IsNegativeCash == true)
                                            //Do nothing
                                            break;
                                        case NegativeBank.Warn:
                                            if (IsNegativeBank == true)
                                            {
                                                if (MessageBox.Show("Your Bank Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                                    return;
                                            }
                                            break;
                                        case NegativeBank.Deny:
                                            if (IsNegativeBank == true)
                                            {
                                                Global.MsgError("Your Bank amount is negative,you are not allowed to submit this voucher!!!");
                                                return;
                                            }
                                            break;
                                    }
                                    #endregion

                                }
                            }

                            #endregion
                        }

                        // The ledger of master section is responsible for Receipt soo no need to check negative cash and negative Bank


                        #endregion
                        //IJournal m_Journal = new Journal();
                        DateTime CashReceipt_Date = Date.ToDotNet(CRtxtDate.Text);
                #endregion

                        //CashReceipt m_CashReceipt = new CashReceipt();
                        //if (!m_CashReceipt.RemoveCashReceiptEntry(Convert.ToInt32(txtCashReceiptID.Text)))
                        //{
                        //    MessageBox.Show("Unable to Update record. Please restart the modification");
                        //    return;
                        //}

                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            //using (SqlCommand dbCommand = new SqlCommand("Acc.xmlCashReceiptUpdate", Global.m_db.cn))
                            //{
                            //    // we are going to use store procedure  
                            //    dbCommand.CommandType = CommandType.StoredProcedure;
                            //    // Add input parameter and set its properties.
                            //    SqlParameter parameter = new SqlParameter();
                            //    // Store procedure parameter name  
                            //    parameter.ParameterName = "@cashreceipt";
                            //    // Parameter type as XML 
                            //    parameter.DbType = DbType.Xml;
                            //    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                            //    parameter.Value = CashReceiptXMLString; // XML string as parameter value  
                            //    if (Global.m_db.cn.State == ConnectionState.Open)
                            //    {
                            //        Global.m_db.cn.Close();
                            //    }
                            //    // Add the parameter in Parameters collection.
                            //    dbCommand.Parameters.Add(parameter);
                            //    Global.m_db.cn.Open();
                            //    int intRetValue = dbCommand.ExecuteNonQuery();
                                //MessageBox.Show(intRetValue.ToString());   
                            //to edit cashReceipt
                                CashReceipt.EditCashReceipt(CashReceiptXMLString);
                                // to send recurring settings
                                CashReceipt.ModifyVoucherRecurring(m_dtRecurringSetting, Convert.ToInt32(CRtxtCashReceiptID.Text));

                                // to modify against references in the voucher
                                CashReceipt.ModifyReference(Convert.ToInt32(CRtxtCashReceiptID.Text), dtReference, ToDeleteRows);
                          //  }
                        }
                        #endregion

                        Global.Msg("Cash Receipt modified successfully!");
                        // AccClassID.Clear();

                        CRtxtCashReceiptID.Clear();
                        CRClearVoucher();


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


            }

        }

        /// <summary>
        /// Read all journal Entry
        /// </summary>
        /// <returns></returns>
        private string CRReadAllCashReceiptEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            SeriesID = (ListItem)CRcboSeriesName.SelectedItem;
            liProjectID = (ListItem)CRcboProjectName.SelectedItem;
            liCashID = (ListItem)CRcmboCashAccount.SelectedItem;

            //Check validation of grid entry
            try
            {
                if (!CRValidateGrid())
                    return string.Empty;
            }
            catch
            {
                throw;
            }


            tw.WriteStartDocument();
            #region  CashReceipt
            tw.WriteStartElement("CASHRECEIPT");
            {
                ///For Journal Master Section     
                string first = CRtxtfirst.Text;
                string second = CRtxtsecond.Text;
                string third = CRtxtthird.Text;
                string fourth = CRtxtfourth.Text;
                string fifth = CRtxtfifth.Text;
                int SID = System.Convert.ToInt32(SeriesID.ID);
                int CashID = System.Convert.ToInt32(liCashID.ID);
                string Voucher_No = System.Convert.ToString(CRtxtVchNo.Text);
                string CashReceiptID = System.Convert.ToString(CRtxtCashReceiptID.Text);
                DateTime CashReceipt_Date = Date.ToDotNet(CRtxtDate.Text);
                string Remarks = System.Convert.ToString(CRtxtRemarks.Text);
                int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                DateTime Created_Date = System.Convert.ToDateTime(Date.GetServerDate());
                // string Created_By = System.Convert.ToString("Admin");
                string Created_By = User.CurrentUserName;
                DateTime Modified_Date = System.Convert.ToDateTime(Date.GetServerDate());
                //string Modified_By = System.Convert.ToString("Admin");
                string Modified_By = User.CurrentUserName;
                tw.WriteStartElement("CASHRECEIPTMASTER");
                tw.WriteElementString("SeriesID", SID.ToString());
                tw.WriteElementString("LedgerID", CashID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("CashReceiptID", CashReceiptID.ToString());
                tw.WriteElementString("CashReceipt_Date", Date.ToDB(CashReceipt_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("ProjectID", ProjectID.ToString());
                tw.WriteElementString("Created_Date", Date.ToDB(Created_Date));
                tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Date.ToDB(Modified_Date));
                tw.WriteElementString("Modified_By", Modified_By.ToString());
                tw.WriteElementString("Field1", first);
                tw.WriteElementString("Field2", second);
                tw.WriteElementString("Field3", third);
                tw.WriteElementString("Field4", fourth);
                tw.WriteElementString("Field5", fifth);
                tw.WriteEndElement();

                ///For journal Detail Section             
                int LedgerID = 0;
                Decimal Amount = 0;
                string RemarksDetail = "";
                string voucherType = "";
                string voucherNo = "";
                tw.WriteStartElement("CASHRECEIPTDETAIL");

                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    LedgerID = System.Convert.ToInt32(grdCashReceipt[i + 1, (int)GridColumnCR.CRLedger_ID].Value);
                    CashReceiptID = System.Convert.ToString(CRtxtCashReceiptID.Text);
                    Amount = System.Convert.ToDecimal(grdCashReceipt[i + 1, (int)GridColumnCR.CRAmount].Value);
                    RemarksDetail = System.Convert.ToString(grdCashReceipt[i + 1, (int)GridColumnCR.CRRemarks].Value);
                    voucherType = System.Convert.ToString(grdCashReceipt[i + 1, (int)GridColumnCR.CRV_Type].Value);
                    voucherNo = System.Convert.ToString(grdCashReceipt[i + 1, (int)GridColumnCR.CRVoucher_No].Value);

                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("CashReceiptID", CashReceiptID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteElementString("VoucherType", voucherType.ToString());
                    tw.WriteElementString("VoucherNumber", voucherNo.ToString());
                    tw.WriteEndElement();
                }
            }
            tw.WriteEndElement();
            //Write Checked Accounting class ID
            try
            {
                ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
                tw.WriteStartElement("ACCCLASSIDS");
                foreach (string tag in arrNode)
                {
                    AccClassID.Add(Convert.ToInt32(tag));
                    tw.WriteElementString("AccID", Convert.ToInt32(tag).ToString());
                }
                tw.WriteEndElement();
            }
            catch
            { }
            //For Computer Name
            string ComputerName = Global.ComputerName;
            string MacAddress = Global.MacAddess;
            string IpAddress = Global.IpAddress;
            tw.WriteStartElement("COMPUTERDETAILS");
            tw.WriteElementString("CompDetails", ComputerName.ToString());
            tw.WriteElementString("MacAddress", MacAddress.ToString());
            tw.WriteElementString("IpAddress", IpAddress.ToString());
            tw.WriteEndElement();

            if (isNew == true)
            {
                string testnew = "INSERT";
                tw.WriteStartElement("LOGGRIDDETAILS");
                tw.WriteElementString("isNew", testnew);
                tw.WriteElementString("NewGridDetails", NewGrid);
                tw.WriteElementString("OldGridDetails", OldGrid);
                tw.WriteEndElement();
            }
            else if (isNew == false)
            {
                string testnew = "UPDATE";
                tw.WriteStartElement("LOGGRIDDETAILS");
                tw.WriteElementString("isNew", testnew);
                tw.WriteElementString("NewGridDetails", NewGrid);
                tw.WriteElementString("OldGridDetails", OldGrid);
                tw.WriteEndElement();
                //  }



            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            // MessageBox.Show(strXML);
            return strXML;
        }

        //It Validates all the entry in the grid Only valid rows are count and validate
        private bool CRValidateGrid()
        {
            var LdrID = new List<int>();
            decimal[] Amt = new decimal[20];

            //Validate input grid record
            for (int i = 0; i < grdCashReceipt.Rows.Count - 1; i++)
            {
                try
                {
                    //if ledger ID repeats then message it
                    // if LedgerID is not present in between them

                    int tmpLedgerID = 0;
                    decimal tempDecValue = 0;



                    //Check if Ledger ID is integer
                    try
                    {
                        string strLedgerID = grdCashReceipt[i + 1, (int)GridColumnCR.CRLedger_ID].ToString();

                        //Dont try to convert if the string is empty, like in case of (NEW) row
                        if (!String.IsNullOrEmpty(strLedgerID))
                            tmpLedgerID = System.Convert.ToInt32(strLedgerID);
                    }
                    catch
                    {
                        throw new Exception("Invalid Ledger ID in the details");

                    }

                    //Amount must be decimal
                    try
                    {
                        string strAmount = grdCashReceipt[i + 1, (int)GridColumnCR.CRAmount].ToString();
                        //Dont try to convert if the string is empty, like in case of (NEW) row
                        if (!String.IsNullOrEmpty(strAmount))
                            tempDecValue = System.Convert.ToDecimal(strAmount);
                    }
                    catch
                    {
                        throw new Exception("Invalid Amount in the details");
                    }

                    //1. Check for duplicate ledger IDs. If there are multiple same ledger ID validation is false
                    if (LdrID.Contains(tmpLedgerID))
                    {
                        throw new Exception("Multiple ledgers detected. You cannot add same ledger multiple times");
                    }

                    //Add Ledger ID to List
                    if (tmpLedgerID > 0)
                        LdrID.Add(tmpLedgerID);
                }
                catch //Some mysterious error occurs
                {
                    throw;
                }
            }

            //Passes all checks so return true
            OnlyReqdDetailRows = LdrID.Count(i => i != 0);
            return true;
        }



        private bool CRValidate()
        {
            bool bValidate = false;
            FormHandle m_FH = new FormHandle();
            bValidate = m_FH.Validate();
            if (CRcboSeriesName.SelectedItem == null)
            {
                Global.MsgError("Invalid Series Name Selected");
                CRcboSeriesName.Focus();
                bValidate = false;
            }
            if (CRcmboCashAccount.SelectedItem == null)
            {
                Global.MsgError("Invalid Cash Account Selected");
                CRcmboCashAccount.Focus();
                bValidate = false;

            }
            if (CRcboProjectName.SelectedItem == null)
            {
                Global.MsgError("Invalid Project Name Selected");
                CRcboProjectName.Focus();
                bValidate = false;

            }
            if (!(grdCashReceipt.Rows.Count > 1))
            {
                Global.MsgError("Invalid Account Ledger Selected in grid");
                grdCashReceipt.Focus();
                bValidate = false;
            }
            return bValidate;
        }

        private void CRChangeState(EntryMode Mode)
        {
            m_mode = Mode;

            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    CREnableControls(false);
                    CRButtonState(true, true, false, true, false);
                    IsFieldChanged = false;
                    break;
                case EntryMode.NEW:
                    CREnableControls(true);
                    CRButtonState(false, false, true, false, true);
                    CRLoadSeriesNo();
                    IsFieldChanged = false;
                    btnSetup.Enabled = CRchkRecurring.Checked;
                    break;
                case EntryMode.EDIT:
                    CREnableControls(true);
                    CRButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    btnSetup.Enabled = CRchkRecurring.Checked;
                    break;

            }
        }

        private void CRLoadSeriesNo()
        {
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("CASH_RCPT");
            CRcboSeriesName.Items.Clear();
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                CRcboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            CRcboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            CRcboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            CRcboSeriesName.SelectedIndex = 0;
        }

        private void CREnableControls(bool Enable)
        {
            CRchkRecurring.Enabled = btnSetup.Enabled = CRtxtVchNo.Enabled = CRtxtDate.Enabled = CRtxtRemarks.Enabled = grdCashReceipt.Enabled = CRcboSeriesName.Enabled = CRcmboCashAccount.Enabled = CRcboProjectName.Enabled = tabControl1.Enabled = Enable;
        }

        private void CRButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            CRbtnNew.Enabled = New;
            CRbtnEdit.Enabled = Edit;
            CRbtnSave.Enabled = Save;
            CRbtnDelete.Enabled = Delete;
            CRbtnCancel.Enabled = Cancel;
        }

        private bool CRNavigation(Navigate NavTo)
        {
            try
            {
                if (CRtxtCashReceiptID.Text != "")
                    dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(CRtxtCashReceiptID.Text), "CASH_RCPT");

                //CashReceipt m_CashReceipt = new CashReceipt();
                if (!IsShortcutKey)
                {
                    CRChangeState(EntryMode.NORMAL);
                }
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(CRtxtCashReceiptID.Text);
                    if (CashReceiptIDCopy > 0)
                    {
                        VouchID = CashReceiptIDCopy;
                        CashReceiptIDCopy = 0;

                    }
                    else
                    {
                        VouchID = Convert.ToInt32(CRtxtCashReceiptID.Text);

                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                DataTable dtCashReceiptMaster = m_CashReceipt.NavigateCashReceiptMaster(VouchID, NavTo);
                if (dtCashReceiptMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }

                DataRow drCashReceiptMaster = dtCashReceiptMaster.Rows[0]; //There is only one row. First row is the required record
                if (IsShortcutKey)
                {
                    CRtxtRemarks.Text = drCashReceiptMaster["Remarks"].ToString();
                    IsShortcutKey = false;
                    CRtxtRemarks.SelectionStart = CRtxtRemarks.Text.Length + 1;
                    return false;
                }


                //Clear everything in the form
                CRClearVoucher();

                //Write the corresponding textboxes


                //Show the corresponding Bank Account Ledger in Combobox
                DataTable dtCashLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCashReceiptMaster["LedgerID"]), LangMgr.DefaultLanguage);
                DataRow drCashLedgerInfo = dtCashLedgerInfo.Rows[0];

                CRcmboCashAccount.Text = drCashLedgerInfo["LedName"].ToString();

                //Show the Corresponding SeriesName in Combobox
                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drCashReceiptMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Cash Receipt");
                    CRcboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    CRcboSeriesName.Text = dr["EngName"].ToString();
                }

                //show the corresponding ProjectNae according to ProjectID
                lblVouNo.Visible = true;
                CRtxtVchNo.Visible = true;
                CRtxtVchNo.Text = drCashReceiptMaster["Voucher_No"].ToString();
                CRtxtDate.Text = Date.DBToSystem(drCashReceiptMaster["CashReceipt_Date"].ToString());
                CRtxtRemarks.Text = drCashReceiptMaster["Remarks"].ToString();
                DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drCashReceiptMaster["ProjectID"]), LangMgr.DefaultLanguage);
                if (dtProjectInfo.Rows.Count != 0)
                {
                    DataRow drProjectInfo = dtProjectInfo.Rows[0];
                    CRcboProjectName.Text = drProjectInfo["Name"].ToString();
                    CRtxtCashReceiptID.Text = drCashReceiptMaster["CashReceiptID"].ToString();
                    dsCashReceipt.Tables["tblCashReceiptMaster"].Rows.Add(CRcboSeriesName.Text, drCashReceiptMaster["Voucher_No"].ToString(), Date.DBToSystem(drCashReceiptMaster["CashReceipt_Date"].ToString()), drCashReceiptMaster["Remarks"].ToString(), CRcmboCashAccount.Text, drProjectInfo["Name"].ToString());
                }
                else
                {
                    CRcboProjectName.Text = "None";
                    CRtxtCashReceiptID.Text = drCashReceiptMaster["CashReceiptID"].ToString();
                    dsCashReceipt.Tables["tblCashReceiptMaster"].Rows.Add(CRcboSeriesName.Text, drCashReceiptMaster["Voucher_No"].ToString(), Date.DBToSystem(drCashReceiptMaster["CashReceipt_Date"].ToString()), drCashReceiptMaster["Remarks"].ToString(), CRcmboCashAccount.Text, "None");
                }

                //For Additional Fields
                if (NumberOfFields > 0)
                {
                    if (NumberOfFields == 1)
                    {
                        CRlblfirst.Visible = true;
                        CRtxtfirst.Visible = true;
                        CRlblsecond.Visible = false;
                        CRtxtsecond.Visible = false;
                        CRlblthird.Visible = false;
                        CRtxtthird.Visible = false;
                        CRlblfourth.Visible = false;
                        CRtxtfourth.Visible = false;
                        CRlblfifth.Visible = false;
                        CRtxtfifth.Visible = false;
                        CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();

                        CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                        CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                        CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                        CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                        CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 2)
                    {
                        CRlblfirst.Visible = true;
                        CRtxtfirst.Visible = true;
                        CRlblsecond.Visible = true;
                        CRtxtsecond.Visible = true;
                        CRlblthird.Visible = false;
                        CRtxtthird.Visible = false;
                        CRlblfourth.Visible = false;
                        CRtxtfourth.Visible = false;
                        CRlblfifth.Visible = false;
                        CRtxtfifth.Visible = false;
                        CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();

                        CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                        CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                        CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                        CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                        CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 3)
                    {
                        CRlblfirst.Visible = true;
                        CRtxtfirst.Visible = true;
                        CRlblsecond.Visible = true;
                        CRtxtsecond.Visible = true;
                        CRlblthird.Visible = true;
                        CRtxtthird.Visible = true;
                        CRlblfourth.Visible = false;
                        CRtxtfourth.Visible = false;
                        CRlblfifth.Visible = false;
                        CRtxtfifth.Visible = false;
                        CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        CRlblthird.Text = drdtadditionalfield["Field3"].ToString();

                        CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                        CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                        CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                        CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                        CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 4)
                    {
                        CRlblfirst.Visible = true;
                        CRtxtfirst.Visible = true;
                        CRlblsecond.Visible = true;
                        CRtxtsecond.Visible = true;
                        CRlblthird.Visible = true;
                        CRtxtthird.Visible = true;
                        CRlblfourth.Visible = true;
                        CRtxtfourth.Visible = true;
                        CRlblfifth.Visible = false;
                        CRtxtfifth.Visible = false;
                        CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        CRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                        CRlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                        CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                        CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                        CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                        CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                        CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 5)
                    {
                        CRlblfirst.Visible = true;
                        CRtxtfirst.Visible = true;
                        CRlblsecond.Visible = true;
                        CRtxtsecond.Visible = true;
                        CRlblthird.Visible = true;
                        CRtxtthird.Visible = true;
                        CRlblfourth.Visible = true;
                        CRtxtfourth.Visible = true;
                        CRlblfifth.Visible = true;
                        CRtxtfifth.Visible = true;

                        CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        CRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                        CRlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                        CRlblfifth.Text = drdtadditionalfield["Field5"].ToString();

                        CRtxtfirst.Text = drCashReceiptMaster["Field1"].ToString();
                        CRtxtsecond.Text = drCashReceiptMaster["Field2"].ToString();
                        CRtxtthird.Text = drCashReceiptMaster["Field3"].ToString();
                        CRtxtfourth.Text = drCashReceiptMaster["Field4"].ToString();
                        CRtxtfifth.Text = drCashReceiptMaster["Field5"].ToString();
                    }


                }
                DataTable dtCashDetail = m_CashReceipt.GetCashReceiptDetail(Convert.ToInt32(CRtxtCashReceiptID.Text));
                for (int i = 1; i <= dtCashDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtCashDetail.Rows[i - 1];
                    grdCashReceipt[i, (int)GridColumnCR.CRCode_No].Value = i.ToString();
                    grdCashReceipt[i, (int)GridColumnCR.CRParticular_Account_Head].Value = drDetail["LedgerName"].ToString();
                    grdCashReceipt[i, (int)GridColumnCR.CRLedger_ID].Value = drDetail["LedgerID"].ToString();
                    grdCashReceipt[i, (int)GridColumnCR.CRAmount].Value = (Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdCashReceipt[i, (int)GridColumnCR.CRV_Type].Value = drDetail["VoucherType"].ToString();
                    grdCashReceipt[i, (int)GridColumnCR.CRVoucher_No].Value = drDetail["VoucherNumber"].ToString();


                    //Code To Get The Current Balance of the Respective Ledger
                    string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
                    string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
                    DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(drDetail["LedgerID"]), null);
                    if (dtLdrInfo.Rows.Count != 1)
                    {
                        grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Bal_Actual].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    else
                    {

                        DataRow drLdrInfo = dtLdrInfo.Rows[0];//Get first record
                        decimal Debit = 0;
                        decimal Credit = 0;
                        decimal DebitOpeningBal = 0;
                        decimal CreditOpeningBal = 0;
                        if (!(drLdrInfo["DebitTotal"] is DBNull))
                        {
                            Debit = Convert.ToDecimal(drLdrInfo["DebitTotal"]);
                        }
                        else
                            Debit = 0;

                        if (!(drLdrInfo["CreditTotal"] is DBNull))
                        {
                            Credit = Convert.ToDecimal(drLdrInfo["CreditTotal"]);
                        }
                        else
                            Credit = 0;

                        if (!(drLdrInfo["OpenBalDr"] is DBNull))
                        {
                            DebitOpeningBal = Convert.ToDecimal(drLdrInfo["OpenBalDr"]);
                        }
                        else
                            DebitOpeningBal = 0;

                        if (!(drLdrInfo["OpenBalCr"] is DBNull))
                        {
                            CreditOpeningBal = Convert.ToDecimal(drLdrInfo["OpenBalCr"]);
                        }
                        else
                            CreditOpeningBal = 0;

                        //Calculate Debit and Credit Totals
                        decimal DebitTotal = Debit + DebitOpeningBal;
                        decimal CreditTotal = Credit + CreditOpeningBal;

                        decimal Balance = DebitTotal - CreditTotal;
                        string strBalance = "";

                        /*//////TODO///////////
                        1. Make a function to check whether the given ledger ID is Debit Type or Credit Type
                        2. If its Debit Type and Balance is zero, show Dr. else show Cr. if balance is zero
                        */
                        //If +ve is present, show as Dr
                        strBalance = ((Balance < 0) ? Balance * -1 : Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        if (Balance >= 0)
                            strBalance = strBalance + " (Dr.)";

                        else //If balance is -ve, its Cr.
                            strBalance = strBalance + " (Cr.)";


                        //Write balance into the grid
                        grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Balance].Value = strBalance;
                        grdCashReceipt[i, (int)GridColumnCR.CRCurrent_Bal_Actual].Value = Balance.ToString();

                    }

                    CRAddRowCashReceipt(grdCashReceipt.RowsCount);
                    dsCashReceipt.Tables["tblCashReceiptDetails"].Rows.Add(drDetail["LedgerName"].ToString(), (Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                    totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                    totalRptAmt = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();
                }

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(CRtxtCashReceiptID.Text), "CASH_RCPT");
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
                btnExport.Enabled = true;
                // if recurring is true then donot load recurring settings for new voucher
                if (!m_isRecurring)
                    CRCheckRecurringSetting(CRtxtCashReceiptID.Text);
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
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            CRNavigation(Navigate.First);
            IsFieldChanged = false;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            CRNavigation(Navigate.Prev);
            IsFieldChanged = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            CRNavigation(Navigate.Next);
            IsFieldChanged = false;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            CRNavigation(Navigate.Last);
            IsFieldChanged = false;
        }

        private void CRClearVoucher()
        {
            m_isRecurring = false;
            m_RVID = 0; 
            CRClearCashReceipt();
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdCashReceipt.Redim(2, 11);
            CRAddGridHeader(); //Write header part
            CRAddRowCashReceipt(1);
            CRClearRecurringSetting();
            dtReference.Rows.Clear();
            AddReferenceColumns();
        }

        private void CRClearCashReceipt()
        {
            CRtxtVchNo.Text = string.Empty;
            CRcboSeriesName.Text = string.Empty;
            // txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            foreach (ListItem lst in CRcmboCashAccount.Items)
            {
                if (lst.ID == Convert.ToInt32(Settings.GetSettings("DEFAULT_CASH_ACCOUNT")))
                {
                    CRcmboCashAccount.Text = lst.Value;
                    break;
                }
            }
            CRtxtRemarks.Text = string.Empty;
            CRcboProjectName.SelectedIndex = 0;
        }

        private void CRbtnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_RECEIPT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            if (!ContinueWithoutSaving())
            {
                return;
            }
            CRClearVoucher();
            CRChangeState(EntryMode.NEW);
        }

        private void CRbtnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            CashReceiptIDCopy = Convert.ToInt32(CRtxtCashReceiptID.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (CashReceiptIDCopy > 0)
            {
                if (m_mode == EntryMode.NEW)
                {
                    CRNavigation(Navigate.ID);
                    CREnableControls(true);
                    CRChangeState(EntryMode.NEW);
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            CRPrintPreviewCR(PrintType.CrystalReport);

        }

        private void CRcboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CROptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    CRtxtVchNo.Enabled = true;
                    SeriesID = (ListItem)CRcboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));
                    //txtVchNo.Enabled = false;

                    //if (txtVchNo.Text == "")
                    //{
                    //    txtVchNo.Text = "Main";

                    //}

                    if (NumberingType == "AUTOMATIC" && !m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {

                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            return;
                        }
                        lblVouNo.Visible = true;
                        CRtxtVchNo.Visible = true;
                        CRtxtVchNo.Text = m_vounum.ToString();
                        CRtxtVchNo.Enabled = false;
                    }
                    else if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        lblVouNo.Visible = false;
                        CRtxtVchNo.Visible = false;
                    }
                    if (m_CashReceiptID > 0 && !m_isRecurring)
                    {
                        lblVouNo.Visible = true;
                        CRtxtVchNo.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CRbtnEdit_Click(object sender, EventArgs e)
        {
            if (CRCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_RECEIPT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(CRtxtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(CRtxtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            isNew = false;
            OldGrid = " ";
            OldGrid = OldGrid + "Voucher No" + CRtxtVchNo.Text + "Series" + CRcboSeriesName.Text + "Project" + CRcboProjectName.Text + "Date" + CRtxtDate.Text + "Cash Account" + CRcmboCashAccount.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdCashReceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdCashReceipt[i + 1, (int)GridColumnCR.CRParticular_Account_Head].Value.ToString();
                string amt = grdCashReceipt[i + 1, (int)GridColumnCR.CRAmount].Value.ToString();
                OldGrid = OldGrid + string.Concat(particular, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;

            if (!ContinueWithoutSaving())
            {
                return;
            }
            if (CRtxtCashReceiptID.Text.Length <= 0)
            {
                Global.MsgError("Please navigate to existing cash receipt first and then try again!");
                return;
            }
            CREnableControls(true);
            CRChangeState(EntryMode.EDIT);

            //if automatic voucher number increment is selected
            string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
            if (NumberingType == "AUTOMATIC")
                CRtxtVchNo.Enabled = false;
        }

        private void CRbtnDelete_Click(object sender, EventArgs e)
        {
            if (CRCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_RECEIPT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }

            NewGrid = NewGrid + "Voucher No" + CRtxtVchNo.Text + "Series" + CRcboSeriesName.Text + "Project" + CRcboProjectName.Text + "Date" + CRtxtDate.Text + "Cash Account" + CRcmboCashAccount.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdCashReceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdCashReceipt[i + 1, 2].Value.ToString();
                string amt = grdCashReceipt[i + 1, 3].Value.ToString();
                NewGrid = NewGrid + string.Concat(particular, amt);
            }
            NewGrid = "NewGridValues" + NewGrid;
            if (Freeze.IsDateFreeze(Date.ToDotNet(CRtxtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            try
            {

                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the cash receipt - " + CRtxtCashReceiptID.Text + "?") == DialogResult.Yes)
                {
                    CashReceipt DelCashReceipt = new CashReceipt();

                    // delete reference
                    string res = VoucherReference.DeleteReference(Convert.ToInt32(CRtxtCashReceiptID.Text), "CASH_RCPT");
                    if (res != "Success")
                    {
                        Global.MsgError("Unable to delete the voucher due to " + res);
                        return;
                    }

                    if (DelCashReceipt.RemoveCashReceiptEntry(Convert.ToInt32(CRtxtCashReceiptID.Text)))
                    {

                        AuditLogDetail auditlog = new AuditLogDetail();
                        auditlog.ComputerName = Global.ComputerName;
                        auditlog.UserName = User.CurrentUserName;
                        auditlog.Voucher_Type = "CASH_RCPT";
                        auditlog.Action = "DELETE";
                        auditlog.Description = NewGrid;
                        auditlog.RowID = Convert.ToInt32(CRtxtCashReceiptID.Text);
                        auditlog.MAC_Address = Global.MacAddess;
                        auditlog.IP_Address = Global.IpAddress;
                        auditlog.VoucherDate = Date.ToDB(DateTime.Now).ToString();

                        auditlog.CreateAuditLog(auditlog);

                        RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "CASH_RECEIPT"); // deleting the recurring setting if voucher is deleted

                        Global.Msg("Cash Receipt -" + CRtxtCashReceiptID.Text + " deleted successfully!");
                        //Navigate to 1 step previous
                        if (!this.CRNavigation(Navigate.Prev))
                        {
                            //This must be because there are no records or this was the first one

                            //If this was the first, try to navigate to second
                            if (!this.CRNavigation(Navigate.Next))
                            {
                                //This was the last one, there are no records left. Simply clear the form and stay calm
                                CRbtnNew_Click(sender, e);
                            }
                        }
                    }
                    else
                        Global.MsgError("There was an error while deleting cash receipt -" + CRtxtCashReceiptID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void CRcboSeriesName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CRtxtDate.Focus();
            }
        }

        private void CRtxtDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CRcmboCashAccount.Focus();
            }
        }

        private void CRcmboCashAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CRcboProjectName.Focus();
            }
        }

        private void CRcboProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                grdCashReceipt.Focus();
            }
        }



        private void CRtxtRemarks_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.R && e.Modifiers == Keys.Control)
            {
                bool chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_VIEW");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                    return;
                }
                IsShortcutKey = true;
                CRNavigation(Navigate.Last);
            }
            if (e.KeyValue == 13)
            {
                CRchkDoNotClose.Focus();
            }
        }

        private void CRchkDoNotClose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CRchkPrntWhileSaving.Focus();
            }
        }

        private void CRchkPrntWhileSaving_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CRbtnSave.Focus();
            }
        }

        private void frmCashReceipt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.N && e.Control)
            {
                CRbtnNew_Click(sender, e);
            }
            else if (e.KeyCode == Keys.E && e.Control)
            {
                CRbtnEdit_Click(sender, e);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                CRbtnSave_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete && e.Control)
            {
                CRbtnDelete_Click(sender, e);
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
                btnPrint_Click(sender, e);
            }
        }

        private void CRbtnCancel_Click(object sender, EventArgs e)
        {
            CRChangeState(EntryMode.NORMAL);
        }

        private void CRbtnDate_Click(object sender, EventArgs e)
        {
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(CRtxtDate.Text));
            frm.ShowDialog();
        }

        private void frmCashReceipt_FormClosing(object sender, FormClosingEventArgs e)
        {
            //    if (!ContinueWithoutSaving())
            //    {
            //        e.Cancel = true;
            //    }
        }

        private void CRbtn_groupvoucherposting_Click(object sender, EventArgs e)
        {
            CashReceipt creceipt = new CashReceipt();
            int rowid = creceipt.GetRowID(CRtxtVchNo.Text);
            int SID = System.Convert.ToInt32(SeriesID.ID);
            int ProjectID = System.Convert.ToInt32(liProjectID.ID);
            DataTable dtbulkvoucher = new DataTable();
            dtbulkvoucher.Columns.Add("Particulars");
            dtbulkvoucher.Columns.Add("DrCr");
            dtbulkvoucher.Columns.Add("Amount");
            dtbulkvoucher.Columns.Add("LedgerID");
            dtbulkvoucher.Columns.Add("VoucherType");
            dtbulkvoucher.Columns.Add("VoucherNo");
            dtbulkvoucher.Columns.Add("Remarks");
            //  string ledid = grdBankReceipt[1, 9].Value.ToString();
            for (int i = 0; i < grdCashReceipt.Rows.Count - 2; i++)
            {
                dtbulkvoucher.Rows.Add(grdCashReceipt[i + 1, (int)GridColumnCR.CRParticular_Account_Head].Value, "Credit", grdCashReceipt[i + 1, (int)GridColumnCR.CRAmount].Value, grdCashReceipt[i + 1, (int)GridColumnCR.CRLedger_ID].Value, "CASH_RCPT", CRtxtVchNo.Text, grdCashReceipt[i + 1, (int)GridColumnCR.CRRemarks].Value);
            }
            frmGroupVoucherList fgl = new frmGroupVoucherList(dtbulkvoucher, SID, ProjectID, rowid);
            fgl.ShowDialog();
        }

        private void CRcmboCashAccount_SelectedIndexChanged(object sender, EventArgs e)
        {

            liCashID = (ListItem)CRcmboCashAccount.SelectedItem;
            int ledgerid = liCashID.ID;

            //DataTable dtLdrInfo = Ledger.GetLedgerDetail("1", null, null, Convert.ToInt32(ledgerid));
            string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
            string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";

            DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(ledgerid), null);
            GetOpeningBalance openbal = new GetOpeningBalance();
            int uid = User.CurrUserID;
            DataTable dtroleinfo = User.GetUserInfo(uid);
            DataRow drrole = dtroleinfo.Rows[0];
            int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());
            int ParentAcc = 0;
            DefaultAccClass = Convert.ToInt32(UserPreference.GetValue("ACCOUNT_CLASS", uid));
            ParentAcc = GetRootAccClassIDD();

            dtGetOpeningBalance = openbal.GetOpeningBalanceByParent(ParentAcc, ledgerid);


            if (dtLdrInfo.Rows.Count <= 0)
            {
                if (dtGetOpeningBalance.Rows.Count > 0)
                {
                    DataRow dropeningBal = dtGetOpeningBalance.Rows[0];
                    if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                    {
                        CRlblCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        CRlblCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                    }
                    else
                    {
                        CRlblCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        CRlblCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                    }
                }
                else
                {
                    CRlblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    CRlblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }

                return;
            }

            // DataRow dr = dtLdrInfo.Rows[0];
            DataRow drLdrInfo = dtLdrInfo.Rows[0];//Get first record
            decimal Debit = 0;
            decimal Credit = 0;
            decimal DebitOpeningBal = 0;
            decimal CreditOpeningBal = 0;
            if (!(drLdrInfo["DebitTotal"] is DBNull))
            {
                Debit = Convert.ToDecimal(drLdrInfo["DebitTotal"]);
            }
            else
                Debit = 0;

            if (!(drLdrInfo["CreditTotal"] is DBNull))
            {
                Credit = Convert.ToDecimal(drLdrInfo["CreditTotal"]);
            }
            else
                Credit = 0;

            if (!(drLdrInfo["OpenBalDr"] is DBNull))
            {
                DebitOpeningBal = Convert.ToDecimal(drLdrInfo["OpenBalDr"]);
            }
            else
                DebitOpeningBal = 0;

            if (!(drLdrInfo["OpenBalCr"] is DBNull))
            {
                CreditOpeningBal = Convert.ToDecimal(drLdrInfo["OpenBalCr"]);
            }
            else
                CreditOpeningBal = 0;


            //Calculate Debit and Credit Totals
            decimal DebitTotal = Debit + DebitOpeningBal;
            decimal CreditTotal = Credit + CreditOpeningBal;

            decimal Balance = DebitTotal - CreditTotal;
            string strBalance = "";


            //If +ve is present, show as Dr
            strBalance = ((Balance < 0) ? Balance * -1 : Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            if (Balance >= 0)
                strBalance = strBalance + " (Dr.)";

            else //If balance is -ve, its Cr.
                strBalance = strBalance + " (Cr.)";


            //Write balance into the grid
            //lblCurrentBalance.Text= strBalance;
            CRlblCurrentBalance.Text = strBalance;


        }
        private int GetRootAccClassIDD()
        {
            if (DefaultAccClass > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(DefaultAccClass));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }
            return 1;//The default root class ID
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CROptionalFields()
        {
            SeriesID = (ListItem)CRcboSeriesName.SelectedItem;
            DataTable dtadditionalfield = Sales.GetAdditionalFields(SeriesID.ID);
            drdtadditionalfield = dtadditionalfield.Rows[0];
            NumberOfFields = Convert.ToInt32(drdtadditionalfield["NumberOfField"].ToString());
            if (NumberOfFields > 0)
            {
                if (NumberOfFields == 1)
                {
                    CRlblfirst.Visible = true;
                    CRtxtfirst.Visible = true;
                    CRlblsecond.Visible = false;
                    CRtxtsecond.Visible = false;
                    CRlblthird.Visible = false;
                    CRtxtthird.Visible = false;
                    CRlblfourth.Visible = false;
                    CRtxtfourth.Visible = false;
                    CRlblfifth.Visible = false;
                    CRtxtfifth.Visible = false;
                    CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                }
                else if (NumberOfFields == 2)
                {
                    CRlblfirst.Visible = true;
                    CRtxtfirst.Visible = true;
                    CRlblsecond.Visible = true;
                    CRtxtsecond.Visible = true;
                    CRlblthird.Visible = false;
                    CRtxtthird.Visible = false;
                    CRlblfourth.Visible = false;
                    CRtxtfourth.Visible = false;
                    CRlblfifth.Visible = false;
                    CRtxtfifth.Visible = false;
                    CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                }
                else if (NumberOfFields == 3)
                {
                    CRlblfirst.Visible = true;
                    CRtxtfirst.Visible = true;
                    CRlblsecond.Visible = true;
                    CRtxtsecond.Visible = true;
                    CRlblthird.Visible = true;
                    CRtxtthird.Visible = true;
                    CRlblfourth.Visible = false;
                    CRtxtfourth.Visible = false;
                    CRlblfifth.Visible = false;
                    CRtxtfifth.Visible = false;
                    CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    CRlblthird.Text = drdtadditionalfield["Field3"].ToString();

                }
                else if (NumberOfFields == 4)
                {
                    CRlblfirst.Visible = true;
                    CRtxtfirst.Visible = true;
                    CRlblsecond.Visible = true;
                    CRtxtsecond.Visible = true;
                    CRlblthird.Visible = true;
                    CRtxtthird.Visible = true;
                    CRlblfourth.Visible = true;
                    CRtxtfourth.Visible = true;
                    CRlblfifth.Visible = false;
                    CRtxtfifth.Visible = false;
                    CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    CRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                    CRlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                }
                else if (NumberOfFields == 5)
                {
                    CRlblfirst.Visible = true;
                    CRtxtfirst.Visible = true;
                    CRlblsecond.Visible = true;
                    CRtxtsecond.Visible = true;
                    CRlblthird.Visible = true;
                    CRtxtthird.Visible = true;
                    CRlblfourth.Visible = true;
                    CRtxtfourth.Visible = true;
                    CRlblfifth.Visible = true;
                    CRtxtfifth.Visible = true;

                    CRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    CRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    CRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                    CRlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                    CRlblfifth.Text = drdtadditionalfield["Field5"].ToString();
                }
            }
            else
            {
                CRlblfirst.Visible = false;
                CRtxtfirst.Visible = false;
                CRlblsecond.Visible = false;
                CRtxtsecond.Visible = false;
                CRlblthird.Visible = false;
                CRtxtthird.Visible = false;
                CRlblfourth.Visible = false;
                CRtxtfourth.Visible = false;
                CRlblfifth.Visible = false;
                CRtxtfifth.Visible = false;
            }

        }

        private void CRbtnPrintPreview_Click(object sender, EventArgs e)
        {
            CRPrintPreviewCR(PrintType.CrystalReport);
        }
        private void CRPrintPreviewCR(PrintType myPrintType)
        {
            dsCashReceipt.Clear();
            totalAmt = 0;
            rptCashReceipt rpt = new rptCashReceipt();
            //Misc.WriteLogo(dsCashReceipt, "tblImage");
            rpt.SetDataSource(dsCashReceipt);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPrintDate = new CrystalDecisions.Shared.ParameterDiscreteValue();

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();

            #region
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

            #endregion

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
            //pdvPrintDate.Value = Date.ToSystem(DateTime.Now);
            //pvCollection.Clear();
            //pvCollection.Add(pdvPrintDate);
            //rpt.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);

            // Navigation(Navigate.ID);

            bool empty = CRNavigation(Navigate.ID);
            if (empty == false)
            {
                return;
            }
            else
            {
                rpt.SetParameterValue("Tot_Amt", totalRptAmt);
                if (totalRptAmt == "")
                {
                    totalRptAmt = "0";
                }
                string inwords = AmountToWords.ConvertNumberAsText(totalRptAmt);
                //MessageBox.Show(inwords);
                rpt.SetParameterValue("AmtInWords", inwords);
                frmReportViewer frm = new frmReportViewer();
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
                        frmemail sendemail = new frmemail(FileName, 1);
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

        #region Methods related to recurring
        DataTable m_dtRecurringSetting = new DataTable();
        public void GetRecurringSetting(DataTable dt)
        {
            if (dt == null)
                CRchkRecurring.Checked = false; // if cancel button is clicked then the chkrecurring is unchecked

            else
                this.m_dtRecurringSetting = dt;
        }
        private void CRchkRecurring_CheckedChanged(object sender, EventArgs e)
        {
            if ((CRchkRecurring.Checked && m_dtRecurringSetting.Rows.Count == 0))
            {
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "CASH_RECEIPT", m_dtRecurringSetting);
                fvr.ShowDialog();
                if (m_dtRecurringSetting.Rows.Count == 0)  // if settings are not available then uncheck the checkbox
                    CRchkRecurring.Checked = false;
            }
            else if (CRchkRecurring.Checked == false && m_dtRecurringSetting.Rows.Count > 0) //if previously saved settings are available
            {
                //if (txtSalesInvoiceID.Text != "" || txtSalesInvoiceID != null)
                //{
                if (Global.MsgQuest("Are you sure you want to delete the saved recurring voucher settings?") == DialogResult.Yes)
                {
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "CASH_RECEIPT"); // delete from database
                    CRClearRecurringSetting();  // clear local variables
                }
                else
                    CRchkRecurring.Checked = true;

                //}
                //else
                //    ClearRecurringSetting();
            }
            if ((CRchkRecurring.Checked == true && m_mode == EntryMode.EDIT) || (CRchkRecurring.Checked == true && m_mode == EntryMode.NEW))
                btnSetup.Enabled = true;
            else
                btnSetup.Enabled = false;

        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "CASH_RECEIPT", m_dtRecurringSetting);
            fvr.ShowDialog();
        }

        string RSID = null, recurringVoucherID = null;
        public void CRCheckRecurringSetting(string VoucherID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            m_dtRecurringSetting = RecurringVoucher.GetRecurringVoucherSetting(VoucherID, "CASH_RECEIPT");
            if (m_dtRecurringSetting.Rows.Count > 0)
            {
                CRchkRecurring.Checked = true;
                RSID = m_dtRecurringSetting.Rows[0]["RVID"].ToString();
                recurringVoucherID = m_dtRecurringSetting.Rows[0]["VoucherID"].ToString();
            }
            else
            {
                CRchkRecurring.Checked = false;
            }
        }
        private void CRClearRecurringSetting()
        {
            m_dtRecurringSetting.Rows.Clear();
            CRchkRecurring.Checked = false;
            recurringVoucherID = "";
            RSID = "";
        } 
        #endregion

        #region Methods related to voucher list
        private void btnList_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!UserPermission.ChkUserPermission("CASH_RECEIPT_VIEW"))
            //    {
            //        Global.MsgError("Sorry! you dont have permission to View Cash Receipt. Please contact your administrator for permission.");
            //        return;
            //    }
            //    string voucherType = "CASH_RECEIPT";

            //    frmVoucherList fvl = new frmVoucherList(this, voucherType);
            //    fvl.ShowDialog();

            //}
            //catch (Exception ex)
            //{
            //    Global.MsgError(ex.Message);
            //}

            try
            {
                if (!UserPermission.ChkUserPermission("CASH_RECEIPT_VIEW"))
                {
                    Global.MsgError("Sorry! you dont have permission to View Cash Receipt. Please contact your administrator for permission.");
                    return;
                }
                string[] vouchValues = new string[5];
                vouchValues[0] = "CASH_RECEIPT";               // voucherType
                vouchValues[1] = "Acc.tblCashReceiptMaster";   // master tableName for the given voucher type  
                vouchValues[2] = "Acc.tblCashReceiptDetails";  // details tableName for the given voucher type
                vouchValues[3] = "CashReceiptID";              // master ID for the given master table
                vouchValues[4] = "CashReceipt_Date";              // date field for a given voucher

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
            m_CashReceiptID = VoucherID;
            CRtxtCashReceiptID.Text = VoucherID.ToString();
            CRNavigation(Navigate.ID);
            // frmCashReceipt_Load(null, null);
        } 
        #endregion

        #region method related to bank reconciliation closed
        public bool CRCheckIfBankReconciliationClosed()
        {
            try
            {
                bool res = false;

                for (int i = 1; i < grdCashReceipt.Rows.Count; i++)
                {
                    int bankID = Convert.ToInt32(grdCashReceipt[i, (int)GridColumnCR.CRLedger_ID].Value);

                    res = BankReconciliation.IsBankReconciliationClosed(bankID, Date.ToDotNet(CRtxtDate.Text));
                    if (res == true)
                    {
                        Global.MsgError("Bank Reconciliation is closed for this Bank, So you cannot add, edit or delete the vocher !");
                        break;
                    }
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
                dr1["VoucherType"] = "JRNL";
                dr1["RefName"] = null;
                dr1["RefID"] = refID;
                dr1["IsAgainst"] = "true";
                dtReference.Rows.Add(dr1);
                //if (crDr == "(Cr)")
                //    grdCashReceipt[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Credit";

                //else
                //    grdCashReceipt[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Debit";

                grdCashReceipt[CurrRowPos, (int)GridColumnCR.CRAmount].Value = amt.ToString();
                grdCashReceipt[CurrRowPos, (int)GridColumnCR.CRRef_Amt].Value = amt.ToString(); //+ crDr;

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
                dr1["VoucherType"] = "JRNL";
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

        public bool CRCheckAmtAgainstRefAmt()
        {
            try
            {
                bool res = true;
                decimal Amt = Convert.ToDecimal(grdCashReceipt[CurrRowPos, (int)GridColumnCR.CRAmount].Value);
                //string crdr = grdCashReceipt[CurrRowPos, (int)GridColumn.Dr_Cr].Value.ToString();
                string crdr ="Credit";

                string amtCrDr = grdCashReceipt[CurrRowPos, (int)GridColumnCR.CRRef_Amt].Value.ToString();
                decimal refAmt = Convert.ToDecimal(amtCrDr.Substring(0, amtCrDr.Length - 4));
                string ledgerName = grdCashReceipt[CurrRowPos, (int)GridColumnCR.CRParticular_Account_Head].Value.ToString();
                if ((refAmt > 0) && (refAmt < Amt) && (amtCrDr.Contains(crdr.Substring(0, crdr.Length - 4))))
                {
                    Global.MsgError("Your transaction amount for ledger " + ledgerName + " is " + Amt + " \n which is greater than the reference amount i.e. " + refAmt + " !");
                    grdCashReceipt[CurrRowPos, (int)GridColumnCR.CRAmount].Value = refAmt.ToString();
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
        #endregion

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

       

    

    }
}
