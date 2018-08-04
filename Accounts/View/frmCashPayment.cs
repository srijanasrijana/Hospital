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
using BusinessLogic.Accounting;

namespace Accounts
{
    public partial class frmCashPayment : Form, IfrmAddAccClass, IfrmDateConverter, ILOVLedger, IDueDate, IVoucherRecurring, IVoucherList, IVoucherReference

    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;
        DataTable dt1 = new DataTable();
        DataTable dtDueDateInfo = new DataTable();
        private string Prefix = "";
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private int DefaultAccClass;
        ArrayList AccountClassID = new ArrayList();
        //Check if the Account has been already set by LOVLedger
        bool hasChanged = false;
        private bool IsFieldChanged = false;
        private int currRowPosition = 0;
        private int OnlyReqdDetailRows = 0;
        private int CurrAccLedgerID = 0;
        private string CurrBal = "";
        private int CurrRowPos = 0;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private bool IsNegativeCash = false;
        private bool IsNegativeBank = false;
        private int CashPaymentIDCopy = 0;
        private int loopCounter = 0;
        ListItem liProjectID = new ListItem();
        ListItem liCashID = new ListItem();
        private bool IsShortcutKey = false;
        ListItem SeriesID = new ListItem();
        decimal totalAmt = 0;
        string totalRptAmt = "";
        List<int> AccClassID = new List<int>();
        private Accounts.Model.dsCashPayment dsCashPayment = new Model.dsCashPayment();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        int m_CashPaymentID = 0;
        DataTable dtAccClassID = new DataTable();
        SourceGrid.Cells.Button CPbtnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents CPevtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents CPevtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents CPevtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents CPevtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        DataTable dtGetOpeningBalance = new DataTable();
        CashPayment m_CashPayment = new CashPayment();
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
        private enum GridColumnCP : int
        {
            CPDel = 0, CPCode_No, CPParticular_Account_Head, CPAmount, CPCurrent_Balance, CPRemarks, CPLedger_ID, CPCurrent_Bal_Actual, CPRef_Amt
        };
        public frmCashPayment()
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
        public frmCashPayment(int CashPaymentID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_CashPaymentID = CashPaymentID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }
        public frmCashPayment(int CashPaymentID)
        {
            InitializeComponent();
            this.m_CashPaymentID = CashPaymentID;
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
            CPtxtDate.Text = Date.ToSystem(DotNetDate);
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

        //Customized header
        private class MyHeaderCP : SourceGrid.Cells.ColumnHeader
        {
            public MyHeaderCP(object value)
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

        private void ListTreeNodes(TreeView tv, TreeNode tn, List<TreeNode> tnReturn)
        {
            foreach (TreeNode nd in tn.Nodes)
                ListTreeNodes(tv, nd, tnReturn);

            tnReturn.Add(tn);
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

        private void frmCashPayment_Load(object sender, EventArgs e)
        {
            AddReferenceColumns();
            if (CPtxtCashPaymentID.Text != "")
                dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(CPtxtCashPaymentID.Text), "CASH_PMNT");
            CPchkDoNotClose.Checked = true;
            CPChangeState(EntryMode.NEW);
            //ListProject(cboProjectName);
            LoadComboboxProject(CPcboProjectName, 0);
            ShowAccClassInTreeView(treeAccClass, null, 0);
            m_mode = EntryMode.NEW;

            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            CPtxtDate.Mask = Date.FormatToMask();
            CPtxtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            //For Loading The Optional Fields
            CPOptionalFields();
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


                //Default cash Account according to user root or other users
                int DefaultCashAccNum = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_CASH_ACCOUNT") : UserPreference.GetValue("DEFAULT_CASH_ACCOUNT", uid));
                string DefaultCashName = "";

                //Add cash to combocashAccount
                DataTable dtCashLedgers = Ledger.GetAllLedger(CashID);
                foreach (DataRow drCashLedgers in dtCashLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCashLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    CPcmboCashAccount.Items.Add(new ListItem((int)drCashLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drCashLedgers["LedgerID"]) == DefaultCashAccNum)
                        DefaultCashName = drLedgerInfo["LedName"].ToString();
                }
                CPcmboCashAccount.DisplayMember = "value";//This value is  for showing at Load condition
                CPcmboCashAccount.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                CPcmboCashAccount.Text = DefaultCashName;

                #endregion

                #region Customevents mainly for saving purpose

                CPcboSeriesName.SelectedIndexChanged += new EventHandler(CPText_Change);
                CPcmboCashAccount.SelectedIndexChanged += new EventHandler(CPText_Change);
                CPtxtVchNo.TextChanged += new EventHandler(CPText_Change);
                CPtxtDate.TextChanged += new EventHandler(CPText_Change);
                CPbtnDate.Click += new EventHandler(CPText_Change);
                CPcboProjectName.SelectedIndexChanged += new EventHandler(CPText_Change);
                CPtxtRemarks.TextChanged += new EventHandler(CPText_Change);

                //Event trigerred when delete button is clicked
                CPevtDelete.Click += new EventHandler(CPDelete_Row_Click);
                CPevtAmountFocusLost.FocusLeft += new EventHandler(CPAmount_Focus_Lost);
                //evtAmountFocusLost.got += new EventHandler(Amount_Focus_Lost); 
                CPevtAccountFocusLost.FocusLeft += new EventHandler(CPevtAccountFocusLost_FocusLeft);
                #endregion
                CPcmboCashAccount_SelectedIndexChanged(sender, e);

                grdCashPayment.Redim(2, 9);
                CPbtnRowDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;

                //Prepare the header part for grid
                CPAddGridHeader();
                CPAddRowCashPayment(1);
                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID
                if (m_CashPaymentID > 0)
                {
                    //Show the values in fields
                    try
                    {

                        if (m_isRecurring)
                        {
                            CPChangeState(EntryMode.NEW);
                        }
                        else
                            CPChangeState(EntryMode.NORMAL);
                        int vouchID = 0;
                        try
                        {
                            vouchID = m_CashPaymentID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }

                        CashPayment m_CashPayment = new CashPayment();


                        if (m_CashPaymentID != 0)
                            vouchID = m_CashPaymentID;

                        //Getting the value of SeriesID via MasterID or VouchID

                        int SeriesIDD = m_CashPayment.GetSeriesIDFromMasterID(vouchID);

                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Cash Payment");
                            CPcboSeriesName.Text = "";
                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            CPcboSeriesName.Text = dr["EngName"].ToString();
                        }
                        DataTable dtCashPaymentMaster = m_CashPayment.GetCashPaymentMaster(vouchID);
                        if (dtCashPaymentMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }
                        DataRow drCashPaymentMaster = dtCashPaymentMaster.Rows[0];

                        //Show the corresponding Cash Account Ledger in Combobox
                        DataTable dtCashLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCashPaymentMaster["LedgerID"]), LangMgr.DefaultLanguage);
                        DataRow drCashLedgerInfo = dtCashLedgerInfo.Rows[0];
                        CPcmboCashAccount.Text = drCashLedgerInfo["LedName"].ToString();
                        //txtVchNo.Text = drCashPaymentMaster["Voucher_No"].ToString();                 
                        if (!m_isRecurring)
                        {
                            //txtVoucherNo.Text = drCashLedgerInfor["Voucher_No"].ToString();
                            CPtxtDate.Text = Date.DBToSystem(drCashPaymentMaster["CashPayment_Date"].ToString());
                        }
                        else
                        {
                            CPtxtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if recurring load today's date
                            //txtduedate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                        }

                        CPtxtRemarks.Text = drCashPaymentMaster["Remarks"].ToString();
                        CPtxtCashPaymentID.Text = drCashPaymentMaster["CashPaymentID"].ToString();
                        dsCashPayment.Tables["tblCashPaymentMaster"].Rows.Add(CPcboSeriesName.Text, drCashPaymentMaster["Voucher_No"].ToString(), Date.DBToSystem(drCashPaymentMaster["CashPayment_Date"].ToString()), CPcmboCashAccount.Text, drCashPaymentMaster["Remarks"].ToString());
                        DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drCashPaymentMaster["ProjectID"]), LangMgr.DefaultLanguage);
                        if (dtProjectInfo.Rows.Count > 0)
                        {
                            DataRow drProjectInfo = dtProjectInfo.Rows[0];
                            CPcboProjectName.Text = drProjectInfo["Name"].ToString();
                        }
                        //For Additional Fields
                        if (NumberOfFields > 0)
                        {
                            if (NumberOfFields == 1)
                            {
                                CPlblfirst.Visible = true;
                                CPtxtfirst.Visible = true;
                                CPlblsecond.Visible = false;
                                CPtxtsecond.Visible = false;
                                CPlblthird.Visible = false;
                                CPtxtthird.Visible = false;
                                CPlblfourth.Visible = false;
                                CPtxtfourth.Visible = false;
                                CPlblfifth.Visible = false;
                                CPtxtfifth.Visible = false;
                                CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();

                                CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                                CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                                CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                                CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                                CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();
                            }
                            else if (NumberOfFields == 2)
                            {
                                CPlblfirst.Visible = true;
                                CPtxtfirst.Visible = true;
                                CPlblsecond.Visible = true;
                                CPtxtsecond.Visible = true;
                                CPlblthird.Visible = false;
                                CPtxtthird.Visible = false;
                                CPlblfourth.Visible = false;
                                CPtxtfourth.Visible = false;
                                CPlblfifth.Visible = false;
                                CPtxtfifth.Visible = false;
                                CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();

                                CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                                CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                                CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                                CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                                CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();
                            }
                            else if (NumberOfFields == 3)
                            {
                                CPlblfirst.Visible = true;
                                CPtxtfirst.Visible = true;
                                CPlblsecond.Visible = true;
                                CPtxtsecond.Visible = true;
                                CPlblthird.Visible = true;
                                CPtxtthird.Visible = true;
                                CPlblfourth.Visible = false;
                                CPtxtfourth.Visible = false;
                                CPlblfifth.Visible = false;
                                CPtxtfifth.Visible = false;
                                CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                CPlblthird.Text = drdtadditionalfield["Field3"].ToString();

                                CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                                CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                                CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                                CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                                CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();

                            }
                            else if (NumberOfFields == 4)
                            {
                                CPlblfirst.Visible = true;
                                CPtxtfirst.Visible = true;
                                CPlblsecond.Visible = true;
                                CPtxtsecond.Visible = true;
                                CPlblthird.Visible = true;
                                CPtxtthird.Visible = true;
                                CPlblfourth.Visible = true;
                                CPtxtfourth.Visible = true;
                                CPlblfifth.Visible = false;
                                CPtxtfifth.Visible = false;
                                CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                CPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                                CPlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                                CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                                CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                                CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                                CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                                CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();

                            }
                            else if (NumberOfFields == 5)
                            {
                                CPlblfirst.Visible = true;
                                CPtxtfirst.Visible = true;
                                CPlblsecond.Visible = true;
                                CPtxtsecond.Visible = true;
                                CPlblthird.Visible = true;
                                CPtxtthird.Visible = true;
                                CPlblfourth.Visible = true;
                                CPtxtfourth.Visible = true;
                                CPlblfifth.Visible = true;
                                CPtxtfifth.Visible = true;

                                CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                CPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                                CPlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                                CPlblfifth.Text = drdtadditionalfield["Field5"].ToString();

                                CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                                CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                                CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                                CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                                CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();
                            }


                        }
                        DataTable dtCashPaymentDetail = m_CashPayment.GetCashPaymentDetail((Convert.ToInt32(drCashPaymentMaster["CashPaymentID"])));
                        for (int i = 1; i <= dtCashPaymentDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtCashPaymentDetail.Rows[i - 1];

                            grdCashPayment[i, 1].Value = i.ToString();
                            grdCashPayment[i, 2].Value = drDetail["LedgerName"].ToString();
                            grdCashPayment[i, 3].Value = drDetail["Amount"].ToString();
                            grdCashPayment[i, 4].Value = drDetail["Remarks"].ToString();
                            grdCashPayment[i, 6].Value = drDetail["LedgerID"].ToString();
                            //    AddRowCashPayment(grdCashPayment.RowsCount);
                            //    dsCashPayment.Tables["tblCashPaymentMaster"].Rows.Add(drDetail["LedgerName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                            //}

                            //Code To Get The Current Balance of the Respective Ledger
                            string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
                            string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
                            DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(drDetail["LedgerID"]), null);
                            if (dtLdrInfo.Rows.Count != 1)
                            {
                                grdCashPayment[i, (int)GridColumnCP.CPCurrent_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdCashPayment[i, 7].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
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
                                grdCashPayment[i, (int)GridColumnCP.CPCurrent_Balance].Value = strBalance;
                                grdCashPayment[i, (int)GridColumnCP.CPCurrent_Bal_Actual].Value = Balance.ToString();

                            }
                            CPAddRowCashPayment(grdCashPayment.RowsCount);
                            dsCashPayment.Tables["tblCashPaymentDetails"].Rows.Add(drDetail["LedgerName"].ToString(), (Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                            totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                            totalRptAmt = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();
                        }
                        DataRow drCashDetails = dtCashPaymentDetail.Rows[0];

                        DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(CPtxtCashPaymentID.Text), "CASH_PMNT");
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
                        // if recurring is true then donot load recurring settings for new voucher
                        if (!m_isRecurring)
                            CPCheckRecurringSetting(CPtxtCashPaymentID.Text);
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
        private void CPText_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
        }

        private void CPDelete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = grdCashPayment.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdCashPayment.RowsCount - 2)
            {
                #region Reference related
                if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(CPtxtCashPaymentID.Text), Convert.ToInt32(grdCashPayment[CurRow, (int)GridColumnCP.CPLedger_ID].Value), "CASH_PMNT"))
                {
                    Global.MsgError("You must delete all other vouchers with reference against this voucher to delete this transaction!");
                    return;
                }

                #endregion

                grdCashPayment.Rows.Remove(ctx.Position.Row);
            }
        }

        private void CPAccount_Selected(object sender, EventArgs e)
        {
            //SourceGrid.CellContext ct = new SourceGrid.CellContext();
            //try
            //{
            //    ct = (SourceGrid.CellContext)sender;
            //}
            //catch (Exception ex)
            //{
            //    //Global.Msg(ex.Message);
            //}
            //if (ct.DisplayText == "")
            //    return;

            //int RowCount = grdCashPayment.RowsCount;
            ////Add a new row
            //string CurRow = (string)grdCashPayment[RowCount - 1, 2].Value;

            ////Check whether the new row is already added
            //if (CurRow != "(NEW)")
            //{
            //    AddRowCashPayment(RowCount);
            //    //Clear (NEW) on other colums as well
            //    ClearNew(RowCount - 1);
            //}
        }

        private void CPAccount_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
        }
        bool isNewReferenceVoucher = false, isAgainstRef = false;
        private void CPAccount_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                int CurRow = grdCashPayment.Selection.GetSelectionRegion().GetRowsIndex()[0];

                #region Reference related
                // if new voucher reference is created for this, donot open the reference form 
                if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(CPtxtCashPaymentID.Text), Convert.ToInt32(grdCashPayment[CurRow, (int)GridColumnCP.CPLedger_ID].Value), "CASH_PMNT"))
                {
                    //Global.MsgError("You must delete all other vouchers with reference against this voucher to delete this transaction!");
                    isNewReferenceVoucher = true;
                    return;
                }
                isNewReferenceVoucher = false;
                #endregion

                frmLOVLedger frm = new frmLOVLedger(this);
                frm.ShowDialog();
                SendKeys.Send("{Tab}");
            }
        }
        decimal amt = 0;
        bool amt_Focused_First = true;
        /// <summary>
        /// used to store current value of Amount before edting it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CPAmount_Focused(object sender, EventArgs e)
        {
            try
            {
                if (amt_Focused_First)
                {
                    int RowCount = grdCashPayment.RowsCount;
                    int CurRow = grdCashPayment.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    amt = Convert.ToDecimal(grdCashPayment[CurRow, (int)GridColumnCP.CPAmount].Value);
                    amt_Focused_First = false;
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                SendKeys.Send("{Tab}");
                return;
            }
        }
        private void CPevtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            int ledID = 0, vouchID = 0;
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            int ledgerID = 0;
            string CurrentBal = "";
            try
            {
                if (grdCashPayment[CurrRowPos, (int)GridColumnCP.CPParticular_Account_Head].Value.ToString() == "(NEW)" || grdCashPayment[CurrRowPos, (int)GridColumnCP.CPParticular_Account_Head].Value.ToString() == "")
                    return;

                try
                {
                    ledgerID = Convert.ToInt32(grdCashPayment[CurrRowPos, (int)GridColumnCP.CPLedger_ID].Value);
                    CurrentBal = grdCashPayment[CurrRowPos, (int)GridColumnCP.CPCurrent_Bal_Actual].Value.ToString();
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
            else ledID = ledgerID;
            if (VoucherReference.CheckIfReferece(ledID) && !isNewReferenceVoucher) // if isBillReference is true for given ledger then load the reference form
            {
                Form fc = Application.OpenForms["frmReference"];

                if (fc != null)
                    fc.Close();

                if (CPtxtCashPaymentID.Text != "")
                {
                    vouchID = Convert.ToInt32(CPtxtCashPaymentID.Text);
                }

                frmReference fr = new frmReference(this, vouchID, "CASH_PMNT", ledID);
                fr.ShowDialog();
                //SendKeys.Send("{Tab}");
            }
            #endregion

            CPFillAllGridRow(CurrRowPos, CurrAccLedgerID, CurrBal);
        }

        private void CPAccount_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmLOVLedger frm = new frmLOVLedger(this, e);
            frm.ShowDialog();
        }

        private void CPAmount_Focus_Lost(object sender, EventArgs e)
        {
            amt_Focused_First = true;
            int RowCount = grdCashPayment.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            string AccName = (string)(grdCashPayment[CurRow, (int)GridColumnCP.CPParticular_Account_Head].Value);
            //CurrAccLedgerID = Convert.ToInt32(grdCashPayment[CurRow, (int)GridColumn.Ledger_ID].Value);
            CurrBal = (string)(grdCashPayment[CurRow, (int)GridColumnCP.CPCurrent_Balance].Value); // updated current balance is sent for calculation of new current balance
            //Check if the input value is correct
            if (grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumnCP.CPAmount].Value == "" || grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumnCP.CPAmount].Value == null)
                grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumnCP.CPAmount].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            if (!Misc.IsNumeric(grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumnCP.CPAmount].Value))
            {
                Global.MsgError("Invalid Amount!");
                ctx.Value = "";
                return;
            }

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
                check = Budget.CheckBudget(Convert.ToInt32(grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumnCP.CPLedger_ID].Value), Convert.ToDecimal(grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumnCP.CPAmount].Value), strXML);
           
            if(!check)
            {
                grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumnCP.CPAmount].Value = 0;
            }

            #region Reference related
            if (m_mode == EntryMode.EDIT)
            {
                grdCashPayment[CurRow, (int)GridColumnCP.CPRef_Amt].Value = VoucherReference.GetAmtForAgainstRef(Convert.ToInt32(CPtxtCashPaymentID.Text), "CONTRA", Convert.ToInt32(grdCashPayment[CurRow, (int)GridColumnCP.CPLedger_ID].Value));
            }

            // check if transaction amount is greater than reference amount
            if (!CPCheckAmtAgainstRefAmt())
            {
                return;
            }
            #endregion

            //CurrBal = (string)(grdCashPayment[CurRow, (int)GridColumn.Current_Bal_Actual].Value);
            CPFillGridRowExceptLedgerID(CurRow, CurrAccLedgerID, CurrBal, amt);

            double checkformat = Convert.ToDouble(grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumnCP.CPAmount].Value.ToString());
            //string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumnCP.CPAmount].Value = checkformat;
            if ((string)(grdCashPayment[RowCount - 1, (int)GridColumnCP.CPParticular_Account_Head].Value) != "(NEW)")
            {
                CPAddRowCashPayment(RowCount);
            }
            //amt = Convert.ToDecimal(grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value);
            //Decimal TempAmount = Convert.ToDecimal(grdCashPayment[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value);
        }

        //Calculate amount and fill on all column on Account focus lost event
        private void CPFillAllGridRow(int RowPosition, int LdrID, string CurrBal)
        {
            decimal TempAmount = 0;
            string CurrentLedgerBalance = CurrBal.ToString();
            string[] CurrentBalance = CurrBal.ToString().Split('(');
            try
            {
                TempAmount = Convert.ToDecimal(grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPAmount].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPCurrent_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }

            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPCurrent_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPCurrent_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else
            {
                grdCashPayment[RowPosition, (int)GridColumnCP.CPCurrent_Balance].Value = CurrBal;
            }
            grdCashPayment[RowPosition, (int)GridColumnCP.CPLedger_ID].Value = LdrID;
            grdCashPayment[RowPosition, (int)GridColumnCP.CPCurrent_Bal_Actual].Value = CurrBal;

        }

        //Calculate and onlu fill or change value of current balance
        private void CPFillGridRowExceptLedgerID(int RowPosition, int LdrID, string CurrBal, decimal amt)
        {

            decimal TempAmount = 0;
            string CurrentLedgerBalance = "";
            decimal temporary = 0;
            string[] CurrentBalance = new string[2];
            try
            {
                if (CurrBal == "")
                {
                    CurrentLedgerBalance = grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPCurrent_Bal_Actual].ToString();
                    CurrentBalance = grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPCurrent_Bal_Actual].ToString().Split('(');
                }
                else
                {
                    CurrentLedgerBalance = CurrBal.ToString();
                    CurrentBalance = CurrBal.ToString().Split('(');
                }


                TempAmount = Convert.ToDecimal(grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPAmount].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            // old code for updating the grid after editing the amount which doesnot perform proper calculation
            //if (CurrentLedgerBalance.Contains("Dr"))
            //{


            //grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";               
            //}

            //else if (CurrentLedgerBalance.Contains("Cr"))
            //{
            //    grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";                
            //}

            //else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            //{      
            //    grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";               
            //}
            //else
            //{
            //    grdCashPayment[RowPosition, (int)GridColumn.Current_Balance].Value = CurrBal;
            //}

            // new code for updating grid after editing the amount
            // new_cur_balance = old_cur_balance + new_amount - old_amount 
            //grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount) - amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";

            if (CurrentLedgerBalance.Contains("Cr"))
            {
                temporary = (Convert.ToDecimal(CurrentBalance[0]) + (-1) * Convert.ToDecimal(TempAmount) + amt);
            }
            else if (CurrentLedgerBalance.Contains("Dr"))
            {

                temporary = Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount) - amt;
                temporary = Convert.ToDecimal(temporary * -1);
            }

            if (temporary < 0)
            {
                grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPCurrent_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }
            else if (temporary > 0)
            {
                grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPCurrent_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else if (temporary == 0)
            {
                grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPCurrent_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {

                grdCashPayment[Convert.ToInt32(RowPosition), (int)GridColumnCP.CPCurrent_Balance].Value = (Convert.ToDecimal(TempAmount) + amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";

            }
            else
            {
                grdCashPayment[RowPosition, (int)GridColumnCP.CPCurrent_Balance].Value = CurrBal;
            }
            

        }

        /// <summary>
        /// Writes the header part for grdJournal
        /// </summary>
        private void CPAddGridHeader()
        {
            grdCashPayment[0, (int)GridColumnCP.CPDel] = new MyHeaderCP("Del");
            grdCashPayment[0, (int)GridColumnCP.CPDel].Column.Width = 40;
            grdCashPayment.Columns[(int)GridColumnCP.CPDel].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdCashPayment[0, (int)GridColumnCP.CPCode_No] = new MyHeaderCP("Code No.");
            grdCashPayment[0, (int)GridColumnCP.CPCode_No].Column.Width = 60;
            grdCashPayment.Columns[(int)GridColumnCP.CPCode_No].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdCashPayment[0, (int)GridColumnCP.CPParticular_Account_Head] = new MyHeaderCP("Particular/Accounting Head");
            grdCashPayment[0, (int)GridColumnCP.CPParticular_Account_Head].Column.Width = 150;
            grdCashPayment.Columns[(int)GridColumnCP.CPParticular_Account_Head].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdCashPayment[0, (int)GridColumnCP.CPAmount] = new MyHeaderCP("Amount");
            grdCashPayment[0, (int)GridColumnCP.CPAmount].Column.Width = 100;
            grdCashPayment.Columns[(int)GridColumnCP.CPAmount].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdCashPayment[0, (int)GridColumnCP.CPCurrent_Balance] = new MyHeaderCP("Current Balance");
            grdCashPayment[0, (int)GridColumnCP.CPCurrent_Balance].Column.Width = 100;
            grdCashPayment.Columns[(int)GridColumnCP.CPCurrent_Balance].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdCashPayment[0, (int)GridColumnCP.CPRemarks] = new MyHeaderCP("Remarks");
            grdCashPayment[0, (int)GridColumnCP.CPRemarks].Column.Width = 150;
            grdCashPayment.Columns[(int)GridColumnCP.CPRemarks].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdCashPayment[0, (int)GridColumnCP.CPLedger_ID] = new MyHeaderCP("Ledger ID");
            grdCashPayment[0, (int)GridColumnCP.CPLedger_ID].Column.Visible = false;

            grdCashPayment[0, (int)GridColumnCP.CPCurrent_Bal_Actual] = new MyHeaderCP("Current Balance");
            grdCashPayment[0, (int)GridColumnCP.CPCurrent_Bal_Actual].Column.Visible = false;

            grdCashPayment[0, (int)GridColumnCP.CPRef_Amt] = new SourceGrid.Cells.ColumnHeader("RefAmt");
            grdCashPayment[0, (int)GridColumnCP.CPRef_Amt].Column.Width = 50;
            grdCashPayment[0, (int)GridColumnCP.CPRef_Amt].Column.Visible = false;
        }

        /// <summary>
        /// Adds the row in the Journal field
        /// </summary>
        private void CPAddRowCashPayment(int RowCount)
        {
            //Add a new row
            grdCashPayment.Redim(Convert.ToInt32(RowCount + 1), grdCashPayment.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            grdCashPayment[i, (int)GridColumnCP.CPDel] = btnDelete;

            grdCashPayment[i, (int)GridColumnCP.CPDel].AddController(CPevtDelete);

            grdCashPayment[i, (int)GridColumnCP.CPCode_No] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox CPtxtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            CPtxtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdCashPayment[i, (int)GridColumnCP.CPParticular_Account_Head] = new SourceGrid.Cells.Cell("", CPtxtAccount);
            CPtxtAccount.Control.GotFocus += new EventHandler(CPAccount_Focused);
            CPtxtAccount.Control.LostFocus += new EventHandler(CPAccount_Leave);
            CPtxtAccount.Control.KeyDown += new KeyEventHandler(CPAccount_KeyDown);
            CPtxtAccount.Control.TextChanged += new EventHandler(CPText_Change);
            grdCashPayment[i, (int)GridColumnCP.CPParticular_Account_Head].AddController(CPevtAccountFocusLost);
            grdCashPayment[i, (int)GridColumnCP.CPParticular_Account_Head].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox CPtxtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            CPtxtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            CPtxtAmount.Control.GotFocus += new EventHandler(CPAmount_Focused);

            grdCashPayment[i, (int)GridColumnCP.CPAmount] = new SourceGrid.Cells.Cell("", CPtxtAmount);
            CPtxtAmount.Control.TextChanged += new EventHandler(CPText_Change);
            grdCashPayment[i, (int)GridColumnCP.CPAmount].AddController(CPevtAmountFocusLost);
            grdCashPayment[i, (int)GridColumnCP.CPAmount].Value = "";

            SourceGrid.Cells.Editors.TextBox CPtxtCurrentBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            CPtxtCurrentBalance.EditableMode = SourceGrid.EditableMode.None;
            grdCashPayment[i, (int)GridColumnCP.CPCurrent_Balance] = new SourceGrid.Cells.Cell("", CPtxtCurrentBalance);
            grdCashPayment[i, (int)GridColumnCP.CPCurrent_Balance].Value = "";

            SourceGrid.Cells.Editors.TextBox CPtxtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            CPtxtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            CPtxtRemarks.Control.TextChanged += new EventHandler(CPText_Change);
            grdCashPayment[i, (int)GridColumnCP.CPRemarks] = new SourceGrid.Cells.Cell("", CPtxtRemarks);
            grdCashPayment[i, (int)GridColumnCP.CPRemarks].Value = "";

            SourceGrid.Cells.Editors.TextBox CPtxtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            CPtxtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdCashPayment[i, (int)GridColumnCP.CPLedger_ID] = new SourceGrid.Cells.Cell("", CPtxtLedgerID);
            grdCashPayment[i, (int)GridColumnCP.CPLedger_ID].Value = "";

            SourceGrid.Cells.Editors.TextBox CPtxtCurrBal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            CPtxtCurrBal.EditableMode = SourceGrid.EditableMode.None;
            grdCashPayment[i, (int)GridColumnCP.CPCurrent_Bal_Actual] = new SourceGrid.Cells.Cell("", CPtxtCurrBal);
            grdCashPayment[i, (int)GridColumnCP.CPCurrent_Bal_Actual].Value = "";

            SourceGrid.Cells.Editors.TextBox CPtxtRefAmt = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            grdCashPayment[i, (int)GridColumnCP.CPRef_Amt] = new SourceGrid.Cells.Cell("", CPtxtRefAmt);
            grdCashPayment[i, (int)GridColumnCP.CPRef_Amt].Value = "0(Dr)";

        }

        private void CPChangeState(EntryMode Mode)
        {
            m_mode = Mode;

            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    CPEnableControls(false);
                    CPButtonState(true, true, false, true, false);
                    IsFieldChanged = false;
                    break;
                case EntryMode.NEW:
                    CPEnableControls(true);
                    CPButtonState(false, false, true, false, true);
                    CPLoadSeriesNo();
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
                case EntryMode.EDIT:
                    CPEnableControls(true);
                    CPButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
            }
        }

        private void CPLoadSeriesNo()
        {
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("CASH_PMNT");
            CPcboSeriesName.Items.Clear();
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                CPcboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            CPcboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            CPcboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            CPcboSeriesName.SelectedIndex = 0;
        }

        private void CPEnableControls(bool Enable)
        {
            chkRecurring.Enabled = btnSetup.Enabled = CPtxtVchNo.Enabled = CPtxtDate.Enabled = CPtxtRemarks.Enabled = grdCashPayment.Enabled = CPcboSeriesName.Enabled = CPcmboCashAccount.Enabled = CPcboProjectName.Enabled = tabControl1.Enabled = Enable;
        }

        private void CPButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            CPbtnNew.Enabled = New;
            CPbtnEdit.Enabled = Edit;
            CPbtnSave.Enabled = Save;
            CPbtnDelete.Enabled = Delete;
            CPbtnCancel.Enabled = Cancel;
        }

        //A function from the Interface IfrmAccClassID. Used to apply the Datatable to this form from AddAccClass Form

        public void AddAccClassID(DataTable AccClassID)
        {
            //try
            //{
            //    dtAccClassID = AccClassID;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
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

        private void CPbtnSave_Click(object sender, EventArgs e)
        {
            if (CPCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("CASH_PAYMENT_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("CASH_PAYMENT_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }

            if (Freeze.IsDateFreeze(Date.ToDotNet(CPtxtDate.Text)))
            {
                return;
            }

            #region BLOCK FOR MANUAL VOUCHER NUMBERING TYPE
            VoucherConfiguration m_VouConfig = new VoucherConfiguration();
            if (SeriesID.ID > 0)
            {
                DataTable dtVouConfigInfo = m_VouConfig.GetVouNumConfiguration(Convert.ToInt32(SeriesID.ID));

                //if (dtVouConfigInfo.Rows.Count > 0)
                //{
                DataRow drVouConfigInfo = dtVouConfigInfo.Rows[0];
                if (drVouConfigInfo["NumberingType"].ToString() == "Manual")
                {
                    //Enter in this block only when VoucherNumberingType is Manual
                    //Checking for Manual VoucherNumberingType
                    try
                    {
                        string returnStr = m_VouConfig.ValidateManualVouNum(CPtxtVchNo.Text, Convert.ToInt32(SeriesID.ID));
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
                    //}
                }
            }
            #endregion

            ListItem liLedgerID = new ListItem();
            liLedgerID = (ListItem)CPcmboCashAccount.SelectedItem;

            //Check Validation
            if (!CPValidate())
                return;

            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }
            if (drdtadditionalfield["IsField1Required"].ToString() == "True")
            {
                if (CPtxtfirst.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field1"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField2Required"].ToString() == "True")
            {
                if (CPtxtsecond.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field2"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField3Required"].ToString() == "True")
            {
                if (CPtxtthird.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field3"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField4Required"].ToString() == "True")
            {
                if (CPtxtfourth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field4"].ToString() + " " + "is Required Field");
                    return;
                }

            }
            if (drdtadditionalfield["IsField5Required"].ToString() == "True")
            {
                if (CPtxtfifth.Text == "")
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
                        CPdueDate();
                        frmDueDate dueDate1 = new frmDueDate(this, dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            dueDate1.ShowDialog();
                        }

                        #region Add voucher number if voucher number is automatic and hidden from the setting
                        int increasedSeriesNum = 0;
                        SeriesID = (ListItem)CPcboSeriesName.SelectedItem;
                        string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumTypeNoUpdate(Convert.ToInt32(SeriesID.ID), out increasedSeriesNum);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers are totally finished!");
                                return;
                            }

                            CPtxtVchNo.Text = m_vounum.ToString();
                            CPtxtVchNo.Enabled = false;
                        }
                        #endregion

                        isNew = true;
                        NewGrid = " ";
                        OldGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + CPtxtVchNo.Text + "Series" + CPcboSeriesName.Text + "Project" + CPcboProjectName.Text + "Date" + CPtxtDate.Text + "Cash A/C" + CPcmboCashAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdCashPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdCashPayment[i + 1, (int)GridColumnCP.CPParticular_Account_Head].Value.ToString();
                            string amt = grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;

                        //Call to Convert into XML Format
                        string CashPaymentXMLString = CPReadAllJournalEntry();
                        if (CashPaymentXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast journal entry to XML!");
                            return;
                        }

                        #region Check Negative cash and Negative bank
                        //Read from sourcegrid and store it to table
                        DataTable CashPaymentDetails = new DataTable();
                        CashPaymentDetails.Columns.Add("Ledger");
                        //CashPaymentDetails.Columns.Add("LedgerFolioNo");
                        //CashPaymentDetails.Columns.Add("BudgetHeadNo");
                        CashPaymentDetails.Columns.Add("Amount");
                        CashPaymentDetails.Columns.Add("Remarks");
                        double totalgrdAmount = 0;
                        for (int i = 0; i < grdCashPayment.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            //  string[] ledgerName = grdCashReceipt[i + 1, 2].ToString().Split('[');
                            string[] ledgerName = grdCashPayment[i + 1, (int)GridColumnCP.CPParticular_Account_Head].ToString().Split('[');
                            CashPaymentDetails.Rows.Add(ledgerName[0].ToString(), grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value, grdCashPayment[i + 1, (int)GridColumnCP.CPCurrent_Balance].Value);

                            //Check for empty Amount
                            object objAmount = grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            //Generally in this section no need to check negative cash and negative bank because all ledgeres associated in detai section are receiver...they will get amount from master section.
                            //Block for checking negative cash and negative Bank
                            #region BLOCK FOR CHECKING NEGATIVE CASH AND NEGATIVE BANK
                            /*
                            #region BLOCK FOR CHECKING NEGATIVE CASH
                            if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                            {
                                bool isCashAccount = false;
                                isCashAccount = Ledger.IsCashAccount((ledgerName[0].ToString()));
                                if (isCashAccount)//If Cash account
                                {
                                    double mDbalCash = 0;
                                    double mCbalCash = 0;
                                    double totalDrCash = 0;
                                    double totalCrCash = 0;

                                    int CashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.Language);
                                    Transaction.GetLedgerBalance(CashLedgerId, ref mDbalCash, ref mCbalCash, arrNode);
                                    //In case of Cash Payment,Bank Account in master section is Credit and other accounts in Detail section are bydefault Debit
                                    totalCrCash = mCbalCash;//In case of detail section,all ledger is posted as Debit...soo credit amount of ledger is its own amount in Transaction.
                                    totalDrCash += (mDbalCash + Convert.ToDouble(grdCashPayment[i + 1, 3].Value));//The Ledgers of Details section will be posted as Debit,soo total balance will be the summation of its own balance and grid value
                                    if ((totalDrCash - totalCrCash) > 0)
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
                                    int bankLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.Language);
                                    Transaction.GetLedgerBalance(bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode);
                                    //In case of Cash Payment,Cash Account in master section is Credit and other accounts in Detail section are bydefault Debit
                                    totalCrBank = mCbalBank;//Ledgers of Details sections will be posted as Debit,so Credit balance will its only own balance
                                    totalDrBank += (mDbalBank + Convert.ToDouble(grdCashPayment[i + 1, 3].Value));

                                    if ((totalDrBank - totalCrBank) > 0)
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

                            */

                            #endregion

                        }

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
                            // int CashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);
                            Transaction.GetLedgerBalance(null, null, Convert.ToInt32(liLedgerID.ID), ref mDbalCash, ref mCbalCash, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero

                            //Incase of Cash Payment,master ledger is bydefulat Cash...and here Cash is Payment soo it would be Credit because Cash is paying amount for other account
                            //Here Total Credit amount of master is calculated from Details section by adding all amount of all account in Details section
                            totalCrCash += (mCbalCash + totalgrdAmount);//this is the amount of self ledger(Cash Ledger) and all the amount of detail portion.Actually amount of detail section and master section should be equall soo whatever amount reamin in detail section will be same in master section
                            totalDrCash = mDbalCash;
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
                        #endregion

                        #region Save xml data to Database
                        int returnJournalId = 0;
                        try
                        {
                            //Global.m_db.BeginTransaction();

                            using (System.IO.StringWriter swStringWriter = new StringWriter())
                            {
                                ////using (SqlCommand dbCommand = new SqlCommand("Acc.xmlCashPaymentInsert", Global.m_db.cn))
                                ////{
                                    // we are going to use store procedure  
                                    ////dbCommand.CommandType = CommandType.StoredProcedure;
                                    ////// Add input parameter and set its properties.
                                    ////SqlParameter parameter = new SqlParameter();
                                    ////// Store procedure parameter name  
                                    ////parameter.ParameterName = "@cashpayment";
                                    ////// Parameter type as XML 
                                    ////parameter.DbType = DbType.Xml;
                                    ////parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                    ////parameter.Value = CashPaymentXMLString; // XML string as parameter value  
                                    ////// Add the parameter in Parameters collection.
                                    ////dbCommand.Parameters.Add(parameter);
                                    ////SqlParameter parameter1 = new SqlParameter();
                                    ////// Store procedure parameter name  
                                    ////parameter1.ParameterName = "@returnId";
                                    ////// Parameter type as XML 
                                    ////parameter1.DbType = DbType.Int32;
                                    ////parameter1.Direction = ParameterDirection.Output; // Output Parameter
                                    ////// Add the parameter in Parameters collection.
                                    ////dbCommand.Parameters.Add(parameter1);
                                    ////Global.m_db.cn.Open();
                                    ////int intRetValue = dbCommand.ExecuteNonQuery();
                                    ////returnJournalId = Convert.ToInt32(parameter1.Value);

                                    //to insert CahPayment
                                    returnJournalId = CashPayment.InsertCashPayment(CashPaymentXMLString);
                                    // to send recurring settings
                                    CashPayment.InsertVoucherRecurring(m_dtRecurringSetting, returnJournalId);
                                    //MessageBox.Show(intRetValue.ToString());
      
                                    // to send reference information
                                    CashPayment.InsertReference(returnJournalId, dtReference);
                                     //}
                            }
                        }
                        catch (Exception ex)
                        {
                            //Global.m_db.RollBackTransaction();
                            Global.MsgError(ex.Message);
                        }
                        #endregion

                        //Update the last AutoNumber in tblSeries,only if the voucher hide type is true
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }

                        Global.Msg("Cash Payment created successfully!");
                        //  AccClassID.Clear();
                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_CashPaymentID, "CASH_PAYMENT"); // set isPosed to true for recurring voucher
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID); // set isPosed to true for recurring voucher
                            m_isRecurring = false;
                        }

                        CPClearVoucher();
                        CPChangeState(EntryMode.NEW);

                        #region Delete After full integration of sp
                        //CashPayment m_CashPayment = new CashPayment();
                        //DateTime CashPayment_Date = Date.ToDotNet(txtDate.Text);

                        ////ListItem liLedgerID = new ListItem();
                        ////liLedgerID = (ListItem)cmboCashAccount.SelectedItem;
                        //liProjectID = (ListItem)cboProjectName.SelectedItem;

                        //SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        //if (AccClassID.Count != 0)
                        //{
                        //    m_CashPayment.Create(Convert.ToInt32(SeriesID.ID), liLedgerID.ID, txtVchNo.Text, CashPayment_Date, txtRemarks.Text, CashPaymentDetails, AccClassID.ToArray(),Convert.ToInt32(liProjectID.ID));

                        //}
                        //else
                        //{
                        //    int[] a = new int[] { 1 };
                        //    m_CashPayment.Create(Convert.ToInt32(SeriesID.ID), liLedgerID.ID, txtVchNo.Text, CashPayment_Date, txtRemarks.Text, CashPaymentDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID));

                        //}
                        #endregion

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
                        CPdueDate();
                        frmDueDate dueDate1 = new frmDueDate(this, dt1, Convert.ToInt32(CPtxtCashPaymentID.Text), "CASH_PMNT");
                        if (dt1.Rows.Count > 0)
                        {
                            dueDate1.ShowDialog();
                        }
                        isNew = false;
                        NewGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + CPtxtVchNo.Text + "Series" + CPcboSeriesName.Text + "Project" + CPcboProjectName.Text + "Date" + CPtxtDate.Text + "Cash A/C" + CPcmboCashAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdCashPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdCashPayment[i + 1, (int)GridColumnCP.CPParticular_Account_Head].Value.ToString();
                            string amt = grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;

                        //Call to Convert into XML Format
                        string CashPaymentXMLString = CPReadAllJournalEntry();
                        if (CashPaymentXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast journal entry to XML!");
                            return;
                        }

                        #region Checking section for negative cash and negative bank
                        //Read from sourcegrid and store it to table
                        DataTable CashPaymentDetails = new DataTable();
                        CashPaymentDetails.Columns.Add("Ledger");
                        //CashPaymentDetails.Columns.Add("LedgerFolioNo");
                        //CashPaymentDetails.Columns.Add("BudgetHeadNo");
                        CashPaymentDetails.Columns.Add("Amount");
                        CashPaymentDetails.Columns.Add("Remarks");
                        double totalgrdAmount = 0;
                        for (int i = 0; i < grdCashPayment.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdCashPayment[i + 1, (int)GridColumnCP.CPParticular_Account_Head].ToString().Split('[');
                            CashPaymentDetails.Rows.Add(ledgerName[0].ToString(), grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value, grdCashPayment[i + 1, (int)GridColumnCP.CPCurrent_Balance].Value);
                            //Check for empty Amount
                            object objAmount = grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }

                            //Generally in this section no need to check negative cash and negative bank because all ledgeres associated in detai section are receiver...they will get amount from master section..
                            #region BLOCK FOR CHECKING NEGATIVE CASH AND NEGATIVE BANK
                            /*
                            #region BLOCK FOR CHECKING NEGATIVE CASH
                            if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                            {
                                bool isCashAccount = false;
                                isCashAccount = Ledger.IsCashAccount((ledgerName[0].ToString()));
                                if (isCashAccount)//If Cash account
                                {
                                    double mDbalCash = 0;
                                    double mCbalCash = 0;
                                    double totalDrCash = 0;
                                    double totalCrCash = 0;

                                    int CashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.Language);
                                    Transaction.GetLedgerBalance(CashLedgerId, ref mDbalCash, ref mCbalCash, arrNode);
                                    //In case of Cash Payment,Bank Account in master section is Credit and other accounts in Detail section are bydefault Debit
                                    totalCrCash = mCbalCash;//In case of detail section,all ledger is posted as Debit...soo credit amount of ledger is its own amount in Transaction.
                                    totalDrCash += (mDbalCash + Convert.ToDouble(grdCashPayment[i + 1, 3].Value));//The Ledgers of Details section will be posted as Debit,soo total balance will be the summation of its own balance and grid value
                                    if ((totalDrCash - totalCrCash) > 0)
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
                                    int bankLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.Language);
                                    Transaction.GetLedgerBalance(bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode);
                                    //In case of Cash Payment,Cash Account in master section is Credit and other accounts in Detail section are bydefault Debit
                                    totalCrBank = mCbalBank;//Ledgers of Details sections will be posted as Debit,so Credit balance will its only own balance
                                    totalDrBank += (mDbalBank + Convert.ToDouble(grdCashPayment[i + 1, 3].Value));

                                    if ((totalDrBank - totalCrBank) > 0)
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

                             */
                            #endregion
                        }
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
                            Transaction.GetLedgerBalance(null, null, Convert.ToInt32(liLedgerID.ID), ref mDbalCash, ref mCbalCash, AccountClassID, 0);//we dont need to check according to project soo ProjecID is kept as zero

                            //Incase of Cash Payment,master ledger is bydefulat Cash...and here Cash is Payment soo it would be Credit because Cash is paying amount for other account
                            //Here Total Credit amount of master is calculated from Details section by adding all amount of all account in Details section
                            totalCrCash += (mCbalCash + totalgrdAmount);//this is the amount of self ledger(Cash Ledger) and all the amount of detail portion.Actually amount of detail section and master section should be equall soo whatever amount reamin in detail section will be same in master section
                            totalDrCash = mDbalCash;
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
                        #endregion

                        //CashPayment m_CashPayment = new CashPayment();
                        //if (!m_CashPayment.RemoveCashPaymentEntry(Convert.ToInt32(txtCashPaymentID.Text)))
                        //{
                        //    MessageBox.Show("Unable to Update record. Please restart the modification");
                        //    return;
                        //}

                        #region Save xml data to Database
                        int returnJournalId = 0;

                        //Global.m_db.BeginTransaction();

                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            //using (SqlCommand dbCommand = new SqlCommand("Acc.xmlCashPaymentUpdate", Global.m_db.cn))
                            //{
                                // we are going to use store procedure  
                                //dbCommand.CommandType = CommandType.StoredProcedure;
                                //// Add input parameter and set its properties.
                                //SqlParameter parameter = new SqlParameter();
                                //// Store procedure parameter name  
                                //parameter.ParameterName = "@cashpayment";
                                //// Parameter type as XML 
                                //parameter.DbType = DbType.Xml;
                                //parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                //parameter.Value = CashPaymentXMLString; // XML string as parameter value  
                                //// Add the parameter in Parameters collection.
                                //dbCommand.Parameters.Add(parameter);
                                ////SqlParameter parameter1 = new SqlParameter();
                                ////// Store procedure parameter name  
                                ////parameter1.ParameterName = "@returnId";
                                ////// Parameter type as XML 
                                ////parameter1.DbType = DbType.Int32;
                                ////parameter1.Direction = ParameterDirection.Output; // Output Parameter
                                ////// Add the parameter in Parameters collection.
                                ////dbCommand.Parameters.Add(parameter1);
                                //if (Global.m_db.cn.State == ConnectionState.Open)
                                //{
                                //    Global.m_db.cn.Close();
                                //}

                                //Global.m_db.cn.Open();
                               // int intRetValue = dbCommand.ExecuteNonQuery();
                                //returnJournalId = Convert.ToInt32(parameter1.Value);

                              //to edit CahPayment
                                CashPayment.EditCashPayment(CashPaymentXMLString);

                                // to send recurring settings
                                CashPayment.ModifyVoucherRecurring(m_dtRecurringSetting, Convert.ToInt32(CPtxtCashPaymentID.Text));
                                //MessageBox.Show(intRetValue.ToString());
      
                                // to modify against references in the voucher
                                CashPayment.ModifyReference(Convert.ToInt32(CPtxtCashPaymentID.Text), dtReference, ToDeleteRows);
                           // }
                        }
                        #endregion

                        Global.Msg("Cash Payement modified successfully!");
                        // AccClassID.Clear();
                        CPtxtCashPaymentID.Clear();
                        CPClearVoucher();
                        CPChangeState(EntryMode.NEW);
                        CPbtnNew_Click(sender, e);
                        if (CPchkPrntWhileSaving.Checked)
                        {
                            prntDirect = 1;
                            CPNavigation(Navigate.Last);
                            btnPrint_Click(sender, e);
                            CPClearVoucher();
                            CPChangeState(EntryMode.NEW);
                            CPbtnNew_Click(sender, e);
                        }
                        if (!CPchkDoNotClose.Checked)
                            this.Close();

                        #region Update code remove after use of sp
                        // CashPayment m_CashPayment = new CashPayment();
                        //DateTime CashPayment_Date = Date.ToDotNet(txtDate.Text);                     
                        //ListItem LiSeriesID = new ListItem();
                        //LiSeriesID = (ListItem)cboSeriesName.SelectedItem;
                        //liProjectID = (ListItem)cboProjectName.SelectedItem;
                        //if (AccClassID.Count != 0)
                        //{
                        //    m_CashPayment.Modify(Convert.ToInt32(txtCashPaymentID.Text), Convert.ToInt32(LiSeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, CashPayment_Date, txtRemarks.Text, CashPaymentDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID));
                        //}
                        //else
                        //{
                        //    int[] a = new int[] { 1 };
                        //    m_CashPayment.Modify(Convert.ToInt32(txtCashPaymentID.Text), Convert.ToInt32(LiSeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, CashPayment_Date, txtRemarks.Text, CashPaymentDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID));

                        //}
                        #endregion

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
            if (CPchkPrntWhileSaving.Checked)
            {
                prntDirect = 1;
                CPNavigation(Navigate.Last);
                btnPrint_Click(sender, e);
                CPClearVoucher();
                CPChangeState(EntryMode.NEW);
                CPbtnNew_Click(sender, e);
            }
            if (!CPchkDoNotClose.Checked)
                this.Close();
        }

        /// <summary>
        /// Read all journal Entry
        /// </summary>
        /// <returns></returns>
        private string CPReadAllJournalEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            SeriesID = (ListItem)CPcboSeriesName.SelectedItem;
            liProjectID = (ListItem)CPcboProjectName.SelectedItem;
            liCashID = (ListItem)CPcmboCashAccount.SelectedItem;

            //validate grid entry
            if (!CPValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();
            #region  Journal
            tw.WriteStartElement("CASHPAYMENT");
            {
                ///For Journal Master Section  
                ///
                string first = CPtxtfirst.Text;
                string second = CPtxtsecond.Text;
                string third = CPtxtthird.Text;
                string fourth = CPtxtfourth.Text;
                string fifth = CPtxtfifth.Text;
                int SID = System.Convert.ToInt32(SeriesID.ID);
                int CashID = System.Convert.ToInt32(liCashID.ID);
                string Voucher_No = System.Convert.ToString(CPtxtVchNo.Text);
                DateTime CashPayment_Date = Date.ToDotNet(CPtxtDate.Text);
                string Remarks = System.Convert.ToString(CPtxtRemarks.Text);
                string CashPaymentID = System.Convert.ToString(CPtxtCashPaymentID.Text);
                int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                DateTime Created_Date = System.Convert.ToDateTime(Date.GetServerDate());
                //string Created_By = System.Convert.ToString("Admin");
                string Created_By = User.CurrentUserName;
                DateTime Modified_Date = System.Convert.ToDateTime(Date.GetServerDate());
                //string Modified_By = System.Convert.ToString("Admin");
                string Modified_By = User.CurrentUserName;
                tw.WriteStartElement("CASHPAYMENTMASTER");
                tw.WriteElementString("SeriesID", SID.ToString());
                tw.WriteElementString("LedgerID", CashID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("CashPayment_Date", Date.ToDB(CashPayment_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("CashPaymentID", CashPaymentID.ToString());
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
                //int CashPaymentID = 0;
                int LedgerID = 0;
                Decimal Amount = 0;
                string DrCr = "";
                string RemarksDetail = "";
                tw.WriteStartElement("CASHPAYMENTDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    //JournalID = System.Convert.ToInt32(288);
                    LedgerID = System.Convert.ToInt32(grdCashPayment[i + 1, (int)GridColumnCP.CPLedger_ID].Value);
                    Amount = System.Convert.ToDecimal(grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value);
                    RemarksDetail = System.Convert.ToString(grdCashPayment[i + 1, (int)GridColumnCP.CPRemarks].Value);
                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("CashPaymentID", CashPaymentID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                tw.WriteStartElement("CASHDEBTORSDUEDATE");
                foreach (DataRow drduedate in dtDueDateInfo.Rows)
                {
                    tw.WriteStartElement("DUEDATEDETAIL");
                    tw.WriteElementString("DUEDATE", Date.ToDB(Convert.ToDateTime(drduedate["DueDate"])));
                    tw.WriteElementString("LedgerID", drduedate["LedgerID"].ToString());
                    tw.WriteElementString("VoucherType", "CASH_PMNT");
                    tw.WriteElementString("VoucherNo", Voucher_No);
                    tw.WriteEndElement();
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
                }


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
        private bool CPValidateGrid()
        {
            int[] LdrID = new int[20];
            decimal[] Amt = new decimal[20];

            //Validate input grid record
            for (int i = 0; i < grdCashPayment.Rows.Count - 1; i++)
            {
                try
                {
                    //if ledger ID repeats then message it
                    // if LedgerID is not present in between them
                    int tempValue = 0;
                    decimal tempDecValue = 0;
                    try
                    {
                        tempValue = System.Convert.ToInt32(grdCashPayment[i + 1, (int)GridColumnCP.CPLedger_ID].Value);
                    }
                    catch (Exception ex)
                    {
                        tempValue = 0;
                    }
                    try
                    {
                        tempDecValue = System.Convert.ToDecimal(grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value);
                    }
                    catch (Exception ex)
                    {
                        tempDecValue = 0;
                    }

                    if (tempValue != 0 && tempDecValue == 0)
                    {
                        return false;
                    }

                    if (LdrID.Contains(tempValue))
                    {
                        if (i + 2 == grdCashPayment.Rows.Count && grdCashPayment[i + 1, (int)GridColumnCP.CPParticular_Account_Head].Value.ToString() == "(NEW)")
                        {
                            //Do Nothing
                        }
                        else
                            return false;
                    }
                    else
                        LdrID[i] = tempValue;

                    if (i + 2 == grdCashPayment.Rows.Count && grdCashPayment[i + 1, (int)GridColumnCP.CPParticular_Account_Head].Value.ToString() == "(NEW)")
                    {
                        //Donothing
                    }
                    else
                        Amt[i] = tempDecValue;
                }

                catch
                {
                    return false;
                }
            }
            OnlyReqdDetailRows = LdrID.Count(i => i != 0);
            return true;
        }

        private bool CPValidate()
        {
            bool bValidate = false;
            FormHandle m_FH = new FormHandle();
            bValidate = m_FH.Validate();

            if (CPcboSeriesName.SelectedItem == null)
            {
                Global.MsgError("Invalid Series Name Selected");
                CPcboSeriesName.Focus();
                bValidate = false;
            }
            if (CPcmboCashAccount.SelectedItem == null)
            {
                Global.MsgError("Invalid Cash Account Selected");
                CPcmboCashAccount.Focus();
                bValidate = false;
            }
            if (!(grdCashPayment.Rows.Count > 1))
            {
                Global.MsgError("Invalid Account Ledger Selected in grid");
                grdCashPayment.Focus();
                bValidate = false;
            }
            return bValidate;
        }

        private void CPbtnEdit_Click(object sender, EventArgs e)
        {
            if (CPCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_PAYMENT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(CPtxtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            isNew = false;
            OldGrid = " ";
            OldGrid = OldGrid + "Voucher No" + CPtxtVchNo.Text + "Series" + CPcboSeriesName.Text + "Project" + CPcboProjectName.Text + "Date" + CPtxtDate.Text + "Cash A/C" + CPcmboCashAccount.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdCashPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdCashPayment[i + 1, (int)GridColumnCP.CPParticular_Account_Head].Value.ToString();
                string amt = grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value.ToString();
                OldGrid = OldGrid + string.Concat(particular, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;

            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            if (CPtxtCashPaymentID.Text.Length <= 0)
            {
                Global.MsgError("Please navigate to existing cash payment first and then try again!");
                return;
            }
            CPEnableControls(true);
            CPChangeState(EntryMode.EDIT);

            //if automatic voucher number increment is selected
            string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
            if (NumberingType == "AUTOMATIC")
                CPtxtVchNo.Enabled = false;
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

        private bool CPNavigation(Navigate NavTo)
        {
            try
            {
                if (CPtxtCashPaymentID.Text != "")
                    dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(CPtxtCashPaymentID.Text), "CASH_PMNT");

                if (!IsShortcutKey)
                {
                    CPChangeState(EntryMode.NORMAL);
                }
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(CPtxtCashPaymentID.Text);
                    if (CashPaymentIDCopy > 0)
                    {
                        VouchID = CashPaymentIDCopy;
                        CashPaymentIDCopy = 0;
                    }
                    else
                    {
                        VouchID = Convert.ToInt32(CPtxtCashPaymentID.Text);
                    }

                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
                IJournal m_Journal = new Journal();

                DataTable dtCashPaymentMaster = m_CashPayment.NavigateCashPaymentMaster(VouchID, NavTo);
                if (dtCashPaymentMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }
                DataRow drCashPaymentMaster = dtCashPaymentMaster.Rows[0]; //There is only one row. First row is the required record
                if (IsShortcutKey)
                {
                    CPtxtRemarks.Text = drCashPaymentMaster["Remarks"].ToString();
                    IsShortcutKey = false;
                    CPtxtRemarks.SelectionStart = CPtxtRemarks.Text.Length + 1;
                    return false;
                }

                //Clear everything in the form
                CPClearVoucher();
                //Write the corresponding textboxes


                //Show the corresponding Cash Account Ledger in Combobox
                DataTable dtCashLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCashPaymentMaster["LedgerID"]), LangMgr.DefaultLanguage);
                DataRow drCashLedgerInfo = dtCashLedgerInfo.Rows[0];
                CPcmboCashAccount.Text = drCashLedgerInfo["LedName"].ToString();

                //Show the Corresponding SeriesName in Combobox
                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drCashPaymentMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Cash Payment");
                    CPcboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    CPcboSeriesName.Text = dr["EngName"].ToString();
                }

                lblVouNo.Visible = true;
                CPtxtVchNo.Visible = true;
                CPtxtVchNo.Text = drCashPaymentMaster["Voucher_No"].ToString();
                CPtxtDate.Text = Date.DBToSystem(drCashPaymentMaster["CashPayment_Date"].ToString());
                CPtxtRemarks.Text = drCashPaymentMaster["Remarks"].ToString();
                //show the corresponding ProjectName according to ProjectID
                DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drCashPaymentMaster["ProjectID"]), LangMgr.DefaultLanguage);
                if (dtProjectInfo.Rows.Count != 0)
                {
                    DataRow drProjectInfo = dtProjectInfo.Rows[0];
                    CPcboProjectName.Text = drProjectInfo["Name"].ToString();
                    CPtxtCashPaymentID.Text = drCashPaymentMaster["CashPaymentID"].ToString();
                    dsCashPayment.Tables["tblCashPaymentMaster"].Rows.Add(CPcboSeriesName.Text, drCashPaymentMaster["Voucher_No"].ToString(), Date.DBToSystem(drCashPaymentMaster["CashPayment_Date"].ToString()), CPcmboCashAccount.Text, drCashPaymentMaster["Remarks"].ToString(), drProjectInfo["Name"].ToString());
                }
                else
                {
                    CPcboProjectName.Text = "None";
                    CPtxtCashPaymentID.Text = drCashPaymentMaster["CashPaymentID"].ToString();
                    dsCashPayment.Tables["tblCashPaymentMaster"].Rows.Add(CPcboSeriesName.Text, drCashPaymentMaster["Voucher_No"].ToString(), Date.DBToSystem(drCashPaymentMaster["CashPayment_Date"].ToString()), CPcmboCashAccount.Text, drCashPaymentMaster["Remarks"].ToString(), "None");
                }
                //For Additional Fields
                if (NumberOfFields > 0)
                {
                    if (NumberOfFields == 1)
                    {
                        CPlblfirst.Visible = true;
                        CPtxtfirst.Visible = true;
                        CPlblsecond.Visible = false;
                        CPtxtsecond.Visible = false;
                        CPlblthird.Visible = false;
                        CPtxtthird.Visible = false;
                        CPlblfourth.Visible = false;
                        CPtxtfourth.Visible = false;
                        CPlblfifth.Visible = false;
                        CPtxtfifth.Visible = false;
                        CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();

                        CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                        CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                        CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                        CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                        CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 2)
                    {
                        CPlblfirst.Visible = true;
                        CPtxtfirst.Visible = true;
                        CPlblsecond.Visible = true;
                        CPtxtsecond.Visible = true;
                        CPlblthird.Visible = false;
                        CPtxtthird.Visible = false;
                        CPlblfourth.Visible = false;
                        CPtxtfourth.Visible = false;
                        CPlblfifth.Visible = false;
                        CPtxtfifth.Visible = false;
                        CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();

                        CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                        CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                        CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                        CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                        CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 3)
                    {
                        CPlblfirst.Visible = true;
                        CPtxtfirst.Visible = true;
                        CPlblsecond.Visible = true;
                        CPtxtsecond.Visible = true;
                        CPlblthird.Visible = true;
                        CPtxtthird.Visible = true;
                        CPlblfourth.Visible = false;
                        CPtxtfourth.Visible = false;
                        CPlblfifth.Visible = false;
                        CPtxtfifth.Visible = false;
                        CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        CPlblthird.Text = drdtadditionalfield["Field3"].ToString();

                        CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                        CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                        CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                        CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                        CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 4)
                    {
                        CPlblfirst.Visible = true;
                        CPtxtfirst.Visible = true;
                        CPlblsecond.Visible = true;
                        CPtxtsecond.Visible = true;
                        CPlblthird.Visible = true;
                        CPtxtthird.Visible = true;
                        CPlblfourth.Visible = true;
                        CPtxtfourth.Visible = true;
                        CPlblfifth.Visible = false;
                        CPtxtfifth.Visible = false;
                        CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        CPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                        CPlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                        CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                        CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                        CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                        CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                        CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 5)
                    {
                        CPlblfirst.Visible = true;
                        CPtxtfirst.Visible = true;
                        CPlblsecond.Visible = true;
                        CPtxtsecond.Visible = true;
                        CPlblthird.Visible = true;
                        CPtxtthird.Visible = true;
                        CPlblfourth.Visible = true;
                        CPtxtfourth.Visible = true;
                        CPlblfifth.Visible = true;
                        CPtxtfifth.Visible = true;

                        CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        CPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                        CPlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                        CPlblfifth.Text = drdtadditionalfield["Field5"].ToString();

                        CPtxtfirst.Text = drCashPaymentMaster["Field1"].ToString();
                        CPtxtsecond.Text = drCashPaymentMaster["Field2"].ToString();
                        CPtxtthird.Text = drCashPaymentMaster["Field3"].ToString();
                        CPtxtfourth.Text = drCashPaymentMaster["Field4"].ToString();
                        CPtxtfifth.Text = drCashPaymentMaster["Field5"].ToString();
                    }


                }
                DataTable dtCashPaymentDetail = m_CashPayment.GetCashPaymentDetail(Convert.ToInt32(CPtxtCashPaymentID.Text));
                for (int i = 1; i <= dtCashPaymentDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtCashPaymentDetail.Rows[i - 1];
                    grdCashPayment[i, (int)GridColumnCP.CPCode_No].Value = i.ToString();
                    grdCashPayment[i, (int)GridColumnCP.CPParticular_Account_Head].Value = drDetail["LedgerName"].ToString();
                    grdCashPayment[i, (int)GridColumnCP.CPAmount].Value = (Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdCashPayment[i, (int)GridColumnCP.CPRemarks].Value = drDetail["Remarks"].ToString();
                    grdCashPayment[i, (int)GridColumnCP.CPLedger_ID].Value = drDetail["LedgerID"].ToString();

                    //Code To Get The Current Balance of the Respective Ledger
                    string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
                    string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
                    DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(drDetail["LedgerID"]), null);
                    if (dtLdrInfo.Rows.Count != 1)
                    {
                        grdCashPayment[i, (int)GridColumnCP.CPCurrent_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        grdCashPayment[i, 7].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
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
                        grdCashPayment[i, (int)GridColumnCP.CPCurrent_Balance].Value = strBalance;
                        grdCashPayment[i, (int)GridColumnCP.CPCurrent_Bal_Actual].Value = Balance.ToString();

                    }
                    CPAddRowCashPayment(grdCashPayment.RowsCount);
                    dsCashPayment.Tables["tblCashPaymentDetails"].Rows.Add(drDetail["LedgerName"].ToString(), (Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                    totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                    totalRptAmt = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();
                }
                DataRow drCashDetails = dtCashPaymentDetail.Rows[0];

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(CPtxtCashPaymentID.Text), "CASH_PMNT");
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
                    CPCheckRecurringSetting(CPtxtCashPaymentID.Text);
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
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_PAYMENT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            if (!ContinueWithoutSaving())
            {
                return;
            }
            CPNavigation(Navigate.First);
            IsFieldChanged = false;
        }


        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_PAYMENT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            if (!ContinueWithoutSaving())
            {
                return;
            }
            CPNavigation(Navigate.Prev);
            IsFieldChanged = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_PAYMENT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            if (!ContinueWithoutSaving())
            {
                return;
            }
            CPNavigation(Navigate.Next);
            IsFieldChanged = false;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_PAYMENT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            if (!ContinueWithoutSaving())
            {
                return;
            }
            CPNavigation(Navigate.Last);
            IsFieldChanged = false;
        }

        private void CPClearVoucher()
        {
            m_isRecurring = false;
            m_RVID = 0; 
            CPClearCashPayment();
            grdCashPayment.Redim(2, 9);
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            CPAddGridHeader(); //Write header part
            CPAddRowCashPayment(1);
            ClearRecurringSetting();
            dtReference.Rows.Clear();
            AddReferenceColumns();
        }

        private void CPClearCashPayment()
        {
            CPtxtVchNo.Clear(); //actually generate a new voucher no.
            // txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            CPtxtRemarks.Clear();
            grdCashPayment.Rows.Clear();
            CPcboSeriesName.Text = string.Empty;
            foreach (ListItem lst in CPcmboCashAccount.Items)
            {
                if (lst.ID == Convert.ToInt32(Settings.GetSettings("DEFAULT_CASH_ACCOUNT")))
                {
                    CPcmboCashAccount.Text = lst.Value;
                    break;
                }
            }
            CPcboProjectName.SelectedIndex = 0;
        }

        private void CPbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CPcboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CPOptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    CPtxtVchNo.Enabled = true;
                    SeriesID = (ListItem)CPcboSeriesName.SelectedItem;
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
                        CPtxtVchNo.Visible = true;
                        CPtxtVchNo.Text = m_vounum.ToString();
                        CPtxtVchNo.Enabled = false;
                    }
                    else if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        lblVouNo.Visible = false;
                        CPtxtVchNo.Visible = false;
                    }
                    if (m_CashPaymentID > 0 && !m_isRecurring)
                    {
                        lblVouNo.Visible = true;
                        CPtxtVchNo.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddClass_Click(object sender, EventArgs e)
        {
            //if (txtCashPaymentID.Text == "")
            //{
            //    frmAddAccClass frm = new frmAddAccClass(this);
            //    frm.Show();
            //}
            //else
            //{
            //    frmAddAccClass frm = new frmAddAccClass(this, Convert.ToInt32(txtCashPaymentID.Text), "CASH_PMNT");
            //    frm.Show();
            //}
        }

        private void CPbtnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_PAYMENT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            if (!ContinueWithoutSaving())
            {
                return;
            }
            CPClearVoucher();
            CPChangeState(EntryMode.NEW);
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (CashPaymentIDCopy > 0)
            {
                if (m_mode == EntryMode.NEW)
                {
                    CPNavigation(Navigate.ID);
                    CPEnableControls(true);
                    CPChangeState(EntryMode.NEW);
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
            CashPaymentIDCopy = Convert.ToInt32(CPtxtCashPaymentID.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            CPPrintPreviewCR(PrintType.CrystalReport);
        }

        private void CPbtnDelete_Click(object sender, EventArgs e)
        {
            if (CPCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("CASH_PAYMENT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }

            NewGrid = NewGrid + "Voucher No" + CPtxtVchNo.Text + "Series" + CPcboSeriesName.Text + "Project" + CPcboProjectName.Text + "Date" + CPtxtDate.Text + "Cash A/C" + CPcmboCashAccount.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdCashPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdCashPayment[i + 1, (int)GridColumnCP.CPParticular_Account_Head].Value.ToString();
                string amt = grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value.ToString();
                NewGrid = NewGrid + string.Concat(particular, amt);
            }
            NewGrid = "NewGridValues" + NewGrid;

            if (Freeze.IsDateFreeze(Date.ToDotNet(CPtxtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            try
            {

                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the cash payment - " + CPtxtCashPaymentID.Text + "?") == DialogResult.Yes)
                {
                    CashPayment DelCashReceipt = new CashPayment();
                    // delete reference
                    string res = VoucherReference.DeleteReference(Convert.ToInt32(CPtxtCashPaymentID.Text), "CASH_PMNT");
                    if (res != "Success")
                    {
                        Global.MsgError("Unable to delete the voucher due to " + res);
                        return;
                    }
                    if (DelCashReceipt.RemoveCashPaymentEntry(Convert.ToInt32(CPtxtCashPaymentID.Text)))
                    {

                        AuditLogDetail auditlog = new AuditLogDetail();
                        auditlog.ComputerName = Global.ComputerName;
                        auditlog.UserName = User.CurrentUserName;
                        auditlog.Voucher_Type = "CASH_PMNT";
                        auditlog.Action = "DELETE";
                        auditlog.Description = NewGrid;
                        auditlog.RowID = Convert.ToInt32(CPtxtCashPaymentID.Text);
                        auditlog.MAC_Address = Global.MacAddess;
                        auditlog.IP_Address = Global.IpAddress;
                        auditlog.VoucherDate = Date.ToDB(DateTime.Now).ToString();

                        auditlog.CreateAuditLog(auditlog);

                        RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "CASH_PAYMENT"); // deleting the recurring setting if voucher is deleted

                        Global.Msg("Cash Payment -" + CPtxtCashPaymentID.Text + " deleted successfully!");
                        //Navigate to 1 step previous
                        if (!this.CPNavigation(Navigate.Prev))
                        {
                            //This must be because there are no records or this was the first one

                            //If this was the first, try to navigate to second
                            if (!this.CPNavigation(Navigate.Next))
                            {
                                //This was the last one, there are no records left. Simply clear the form and stay calm
                                CPbtnNew_Click(sender, e);
                            }
                        }
                    }
                    else
                        Global.MsgError("There was an error while deleting cash payment -" + CPtxtCashPaymentID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void CPcboSeriesName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CPtxtDate.Focus();
            }
        }

        private void CPtxtDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CPcmboCashAccount.Focus();
            }
        }

        private void CPcmboCashAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CPcboProjectName.Focus();
            }
        }

        private void CPcboProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                grdCashPayment.Focus();
            }
        }

        private void CPtxtRemarks_KeyDown(object sender, KeyEventArgs e)
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
                CPNavigation(Navigate.Last);
            }
            if (e.KeyValue == 13)
            {
                CPchkDoNotClose.Focus();
            }
        }

        private void CPchkDoNotClose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CPchkPrntWhileSaving.Focus();
            }
        }

        private void CPchkPrntWhileSaving_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CPbtnSave.Focus();
            }
        }

        private void txtRemarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmCashPayment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            //else if (e.KeyCode == Keys.N && e.Control)
            //{
            //    btnNew_Click(sender, e);
            //}
            else if (e.KeyCode == Keys.E && e.Control)
            {
                CPbtnEdit_Click(sender, e);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                CPbtnSave_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete && e.Control)
            {
                CPbtnDelete_Click(sender, e);
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

        private void CPbtnCancel_Click(object sender, EventArgs e)
        {
            CPChangeState(EntryMode.NORMAL);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void CPbtnDate_Click(object sender, EventArgs e)
        {
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(CPtxtDate.Text));
            frm.ShowDialog();
        }

        private void frmCashPayment_FormClosing(object sender, FormClosingEventArgs e)
        {

            //if (!ContinueWithoutSaving())
            //{
            //    e.Cancel = true;
            //}
        }

        private void CPbtn_groupvoucherposting_Click(object sender, EventArgs e)
        {

            CashPayment cpayment = new CashPayment();
            int rowid = cpayment.GetRowID(CPtxtVchNo.Text);
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
            for (int i = 0; i < grdCashPayment.Rows.Count - 2; i++)
            {
                dtbulkvoucher.Rows.Add(grdCashPayment[i + 1, (int)GridColumnCP.CPParticular_Account_Head].Value, "Debit", grdCashPayment[i + 1, (int)GridColumnCP.CPAmount].Value, grdCashPayment[i + 1, (int)GridColumnCP.CPLedger_ID].Value, "CASH_PMNT", CPtxtVchNo.Text, grdCashPayment[i + 1, (int)GridColumnCP.CPRemarks].Value);
            }
            frmGroupVoucherList fgl = new frmGroupVoucherList(dtbulkvoucher, SID, ProjectID, rowid);
            fgl.ShowDialog();
        }

        private void CPcmboCashAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            liCashID = (ListItem)CPcmboCashAccount.SelectedItem;
            int ledgerid = liCashID.ID;
            //  DataTable dtLdrInfo = Ledger.GetLedgerDetail("1", null, null, Convert.ToInt32(ledgerid));

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
                        CPlblCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        CPlblCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                    }
                    else
                    {
                        CPlblCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        CPlblCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                    }
                }
                else
                {
                    CPlblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    CPlblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
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
            //lblCurrentBalance.Text = strBalance;
            CPlblCurrentBalance.Text = strBalance;

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
        private void   CPdueDate()
        {
            dt1.Rows.Clear();
            dt1.Columns.Clear();
            dt1.Columns.Add("LedgerId");
            dt1.Columns.Add("LedgerName");

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            tw.WriteStartElement("LedgerIDs");

            for (int i = 1; i <= grdCashPayment.Rows.Count() - 2; i++)
            {
                //int ledgerId = Convert.ToInt32(grdCashPayment[i, (int)GridColumn.Ledger_ID].Value);
                //int GroupID = AccountGroup.GetGroupIDByLedgerID(ledgerId);
                //if (GroupID == 29)
                //{
                //    dt1.Rows.Add(grdCashPayment[i, (int)GridColumn.Ledger_ID].Value, grdCashPayment[i, (int)GridColumn.Particular_Account_Head].Value);
                //}

                tw.WriteElementString("LedgerID", grdCashPayment[i, (int)GridColumnCP.CPLedger_ID].Value.ToString());
                
            }
            tw.WriteEndElement();
            // tw.WriteFullEndElement();
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = "";
            int a = 0;
            a = ms.ToArray().Length;
            strXML = AEncoder.GetString(ms.ToArray());
            dt1 = Ledger.FilterLedgersByGroup(strXML,0,29);

        }
        public void AddLedgerDueDate(DataTable dt)
        {
            dtDueDateInfo = dt;
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
        private void CPOptionalFields()
        {
            SeriesID = (ListItem)CPcboSeriesName.SelectedItem;
            DataTable dtadditionalfield = Sales.GetAdditionalFields(SeriesID.ID);
            drdtadditionalfield = dtadditionalfield.Rows[0];
            NumberOfFields = Convert.ToInt32(drdtadditionalfield["NumberOfField"].ToString());
            if (NumberOfFields > 0)
            {
                if (NumberOfFields == 1)
                {
                    CPlblfirst.Visible = true;
                    CPtxtfirst.Visible = true;
                    CPlblsecond.Visible = false;
                    CPtxtsecond.Visible = false;
                    CPlblthird.Visible = false;
                    CPtxtthird.Visible = false;
                    CPlblfourth.Visible = false;
                    CPtxtfourth.Visible = false;
                    CPlblfifth.Visible = false;
                    CPtxtfifth.Visible = false;
                    CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                }
                else if (NumberOfFields == 2)
                {
                    CPlblfirst.Visible = true;
                    CPtxtfirst.Visible = true;
                    CPlblsecond.Visible = true;
                    CPtxtsecond.Visible = true;
                    CPlblthird.Visible = false;
                    CPtxtthird.Visible = false;
                    CPlblfourth.Visible = false;
                    CPtxtfourth.Visible = false;
                    CPlblfifth.Visible = false;
                    CPtxtfifth.Visible = false;
                    CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                }
                else if (NumberOfFields == 3)
                {
                    CPlblfirst.Visible = true;
                    CPtxtfirst.Visible = true;
                    CPlblsecond.Visible = true;
                    CPtxtsecond.Visible = true;
                    CPlblthird.Visible = true;
                    CPtxtthird.Visible = true;
                    CPlblfourth.Visible = false;
                    CPtxtfourth.Visible = false;
                    CPlblfifth.Visible = false;
                    CPtxtfifth.Visible = false;
                    CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    CPlblthird.Text = drdtadditionalfield["Field3"].ToString();

                }
                else if (NumberOfFields == 4)
                {
                    CPlblfirst.Visible = true;
                    CPtxtfirst.Visible = true;
                    CPlblsecond.Visible = true;
                    CPtxtsecond.Visible = true;
                    CPlblthird.Visible = true;
                    CPtxtthird.Visible = true;
                    CPlblfourth.Visible = true;
                    CPtxtfourth.Visible = true;
                    CPlblfifth.Visible = false;
                    CPtxtfifth.Visible = false;
                    CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    CPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                    CPlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                }
                else if (NumberOfFields == 5)
                {
                    CPlblfirst.Visible = true;
                    CPtxtfirst.Visible = true;
                    CPlblsecond.Visible = true;
                    CPtxtsecond.Visible = true;
                    CPlblthird.Visible = true;
                    CPtxtthird.Visible = true;
                    CPlblfourth.Visible = true;
                    CPtxtfourth.Visible = true;
                    CPlblfifth.Visible = true;
                    CPtxtfifth.Visible = true;

                    CPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    CPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    CPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                    CPlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                    CPlblfifth.Text = drdtadditionalfield["Field5"].ToString();
                }
            }
            else
            {
                CPlblfirst.Visible = false;
                CPtxtfirst.Visible = false;
                CPlblsecond.Visible = false;
                CPtxtsecond.Visible = false;
                CPlblthird.Visible = false;
                CPtxtthird.Visible = false;
                CPlblfourth.Visible = false;
                CPtxtfourth.Visible = false;
                CPlblfifth.Visible = false;
                CPtxtfifth.Visible = false;
            }

        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            CPPrintPreviewCR(PrintType.CrystalReport);
        }
        private void CPPrintPreviewCR(PrintType myPrintType)
        {
            dsCashPayment.Clear();
            totalAmt = 0;
            rptCashPayment rpt = new rptCashPayment();
            rpt.SetDataSource(dsCashPayment);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

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

            bool empty = CPNavigation(Navigate.ID);
            if (empty == false)
            {
                return;
            }
            else
            {
                // Navigation(Navigate.ID);
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

        #region methods related to recurring
        DataTable m_dtRecurringSetting = new DataTable();
        public void GetRecurringSetting(DataTable dt)
        {
            if (dt == null)
                chkRecurring.Checked = false; // if cancel button is clicked then the chkrecurring is unchecked

            else
                this.m_dtRecurringSetting = dt;
        }
        private void CPchkRecurring_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkRecurring.Checked && m_dtRecurringSetting.Rows.Count == 0))
            {
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "CASH_PAYMENT", m_dtRecurringSetting);
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
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "CASH_PAYMENT"); // delete from database
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
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "CASH_PAYMENT", m_dtRecurringSetting);
            fvr.ShowDialog();
        }

        string RSID = null, recurringVoucherID = null;
        public void CPCheckRecurringSetting(string VoucherID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            m_dtRecurringSetting = RecurringVoucher.GetRecurringVoucherSetting(VoucherID, "CASH_PAYMENT");
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

        #region methods related to  voucher list
        private void btnList_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    // check if the user has permission to view the voucher
            //    if (!UserPermission.ChkUserPermission("CASH_PAYMENT_VIEW"))
            //    {
            //        Global.MsgError("Sorry! you dont have permission to View Cash Payment. Please contact your administrator for permission.");
            //        return;
            //    }

            //    string VoucherType = "CASH_PAYMENT";

            //    frmVoucherList fvl = new frmVoucherList(this, VoucherType);
            //    fvl.ShowDialog();

            //}
            //catch (Exception ex)
            //{
            //    Global.MsgError(ex.Message);
            //}

            try
            {
                // check if the user has permission to view the voucher
                if (!UserPermission.ChkUserPermission("CASH_PAYMENT_VIEW"))
                {
                    Global.MsgError("Sorry! you dont have permission to View Cash Payment. Please contact your administrator for permission.");
                    return;
                }

                string[] vouchValues = new string[5];
                vouchValues[0] = "CASH_PAYMENT";               // voucherType
                vouchValues[1] = "Acc.tblCashPaymentMaster";   // master tableName for the given voucher type  
                vouchValues[2] = "Acc.tblCashPaymentDetails";  // details tableName for the given voucher type
                vouchValues[3] = "CashPaymentID";              // master ID for the given master table
                vouchValues[4] = "CashPayment_Date";              // date field for a given voucher

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
            m_CashPaymentID = VoucherID;
            CPtxtCashPaymentID.Text = VoucherID.ToString();
            CPNavigation(Navigate.ID);
            //frmCashPayment_Load(null, null);
        } 
        #endregion

        #region method related to bank reconciliation closing
        public bool CPCheckIfBankReconciliationClosed()
        {
            try
            {
                bool res = false;

                for (int i = 1; i < grdCashPayment.Rows.Count; i++)
                {
                    int bankID = Convert.ToInt32(grdCashPayment[i, (int)GridColumnCP.CPLedger_ID].Value);

                    res = BankReconciliation.IsBankReconciliationClosed(bankID, Date.ToDotNet(CPtxtDate.Text));
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
                //    grdCashPayment[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Credit";

                //else
                //    grdCashPayment[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Debit";

                grdCashPayment[CurrRowPos, (int)GridColumnCP.CPAmount].Value = amt.ToString();
                grdCashPayment[CurrRowPos, (int)GridColumnCP.CPRef_Amt].Value = amt.ToString();// +crDr;

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

        public bool CPCheckAmtAgainstRefAmt()
        {
            try
            {
                bool res = true;
                decimal Amt = Convert.ToDecimal(grdCashPayment[CurrRowPos, (int)GridColumnCP.CPAmount].Value);
                //string crdr = grdCashPayment[CurrRowPos, (int)GridColumn.Dr_Cr].Value.ToString();
                string crdr = "Debit";

                string amtCrDr = grdCashPayment[CurrRowPos, (int)GridColumnCP.CPRef_Amt].Value.ToString();
                decimal refAmt = Convert.ToDecimal(amtCrDr.Substring(0, amtCrDr.Length - 4));
                string ledgerName = grdCashPayment[CurrRowPos, (int)GridColumnCP.CPParticular_Account_Head].Value.ToString();
                if ((refAmt > 0) && (refAmt < Amt) && (amtCrDr.Contains(crdr.Substring(0, crdr.Length - 4))))
                {
                    Global.MsgError("Your transaction amount for ledger " + ledgerName + " is " + Amt + " \n which is greater than the reference amount i.e. " + refAmt + " !");
                    grdCashPayment[CurrRowPos, (int)GridColumnCP.CPAmount].Value = refAmt.ToString();
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void lblDifferenceAmount_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void lblCreditTotal_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
