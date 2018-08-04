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
using Common;
using DateManager;
using System.IO;
using System.Data.SqlClient;
using CrystalDecisions.Shared;
using Accounts.Reports;


namespace Accounts
{
    public partial class frmBankReceipt : Form, IfrmAddAccClass, IfrmDateConverter, ILOVLedger, IVoucherRecurring, IVoucherList, IVoucherReference
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private int DefaultAccClass;
        private string Prefix = "";
        //Check if the Account has been already set by LOVLedger
        private int OnlyReqdDetailRows = 0;
        private bool IsFieldChanged = false;
        private int CurrAccLedgerID = 0;
        private string CurrBal = "";
        private int CurrRowPos = 0;
        ListItem liProjectID = new ListItem();
        ListItem liBankID = new ListItem();
        private bool hasChanged = false;
        private int currRowPosition = 0;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        SourceGrid.CellContext ctx1 = new SourceGrid.CellContext();
        private bool IsChequeDateButton = false;
        private bool IsNegativeCash = false;
        private bool IsNegativeBank = false;
        private int BankReceiptIDCopy = 0;
        private int loopCounter = 0;
        List<int> AccClassID = new List<int>();
        private bool IsShortcutKey = false;
        ListItem SeriesID = new ListItem();
        decimal totalAmt = 0;
        string totalRptAmt = "";
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        private enum GridColumnBR : int
        {
            BRDel = 0, BRCode_No, BRParticular_Account_Head, BRAmount, BRCurrent_Balance, BRRemarks, BRCheque_No, BRCheque_Bank, BRChequeDate, BRLedger_ID, BRCurrent_Bal_Actual, BRRef_Amt
        };
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        private Accounts.Model.dsBankReceipt dsBankReceipt = new Model.dsBankReceipt();

        DataTable dtGetOpeningBalance = new DataTable();

        DataTable dtAccClassID = new DataTable();
        SourceGrid.Cells.Button BRbtnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents BRevtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BRevtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BRevtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BRevtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BRevtChequeNumberFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BRevtChequeBankFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BRevtChequeDateFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();

        BankReceipt m_BankReceipt = new BankReceipt();
        private int m_BankReceiptID;
        //For Export Menu
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
        public frmBankReceipt()
        {
            InitializeComponent();
        }
        bool m_isRecurring = false;
        int m_RVID = 0;
        /// <summary>
        /// constructor to open the form from voucher recurring reminder
        /// </summary>
        /// <param name="BankReceiptID"></param>
        /// <param name="isRecurring"></param>
        public frmBankReceipt(int BankReceiptID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_BankReceiptID = BankReceiptID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }

        public frmBankReceipt(int BankReceiptID)
        {
            InitializeComponent();
            this.m_BankReceiptID = BankReceiptID;
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
            if (!IsChequeDateButton)
                BRtxtDate.Text = Date.ToSystem(DotNetDate);
            if (IsChequeDateButton)
                ctx1.Value = Date.ToSystem(DotNetDate);
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
        private class MyHeaderBR : SourceGrid.Cells.ColumnHeader
        {
            public MyHeaderBR(object value)
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

        private void frmBankReceipt_Load(object sender, EventArgs e)
        {
            dtReference.Rows.Clear();
            AddReferenceColumns();
            BRChangeState(EntryMode.NEW);
            //ListProject(cboProjectName);
            LoadComboboxProject(BRcboProjectName, 0);
            ShowAccClassInTreeView(treeAccClass, null, 0);
            Transaction m_Transaction = new Transaction();
            m_mode = EntryMode.NEW;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            BRtxtDate.Mask = Date.FormatToMask();
            BRtxtDate.Text = Date.ToSystem(Date.GetServerDate()); //By default show the current date from the sqlserver.
            //For Loading The Optional Fields
            BROptionalFields();
            try
            {
                #region Customevents mainly for saving purpose
                BRcboSeriesName.SelectedIndexChanged += new EventHandler(BRText_Change);
                BRcomboBankAccount.SelectedIndexChanged += new EventHandler(BRText_Change);
                BRcomboBankAccount.SelectedIndexChanged += new EventHandler(BRcmboBankAccount_IndexChanged);
                BRtxtVchNo.TextChanged += new EventHandler(BRText_Change);
                BRtxtDate.TextChanged += new EventHandler(BRText_Change);
                BRbtnDate.Click += new EventHandler(BRText_Change);
                BRcboProjectName.SelectedIndexChanged += new EventHandler(BRText_Change);
                BRtxtRemarks.TextChanged += new EventHandler(BRText_Change);

                //Event trigerred when delete button is clicked
                BRevtDelete.Click += new EventHandler(BRDelete_Row_Click);
                BRevtAmountFocusLost.FocusLeft += new EventHandler(BRAmount_Focus_Lost);
                BRevtAccountFocusLost.FocusLeft += new EventHandler(BRevtAccountFocusLost_FocusLeft);
                BRevtChequeDateFocusLost.Click += new EventHandler(BRChequeDate_Click);
                BRevtChequeDateFocusLost.Click += new EventHandler(BRText_Change);
                #endregion
                //MessageBox.Show(IsFieldChanged.ToString());

                #region Load Default bank as per user preference
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultBank Account according to user root or other users
                int DefaultBankAccNum = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_BANK_ACCOUNT") : UserPreference.GetValue("DEFAULT_BANK_ACCOUNT", uid));
                string DefaultBankName = "";

                //Add Banks to comboBankAccount
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                foreach (DataRow drBankLedgers in dtBankLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    BRcomboBankAccount.Items.Add(new ListItem((int)drBankLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drBankLedgers["LedgerID"]) == DefaultBankAccNum)
                        DefaultBankName = drLedgerInfo["LedName"].ToString();
                }



                BRcomboBankAccount.DisplayMember = "value";//This value is  for showing at Load condition
                BRcomboBankAccount.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                BRcomboBankAccount.Text = DefaultBankName;


                #endregion

                grdBankReceipt.Redim(2, 13);
                BRbtnRowDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                BRAddGridHeader();

                BRAddRowBankReceipt(1);

                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID


                if (m_BankReceiptID > 0)
                {
                    //Show the values in fields

                    try
                    {

                        if (m_isRecurring)
                        {
                            BRChangeState(EntryMode.NEW);
                        }
                        else
                            BRChangeState(EntryMode.NORMAL);

                        int vouchID = 0;
                        try
                        {
                            vouchID = m_BankReceiptID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }


                        BankPayment m_BankPayment = new BankPayment();


                        //Getting the value of SeriesID via MasterID or VouchID

                        int SeriesIDD = m_BankReceipt.GetSeriesIDFromMasterID(vouchID);



                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Bank Receipt");
                            BRcboSeriesName.Text = "";

                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            BRcboSeriesName.Text = dr["EngName"].ToString();

                        }

                        DataTable dtBankReceiptMaster = m_BankReceipt.GetBankReceiptMaster(vouchID);


                        if (dtBankReceiptMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }


                        DataRow drBankReceiptMaster = dtBankReceiptMaster.Rows[0];
                        if (!m_isRecurring)
                        {
                            BRtxtVchNo.Text = drBankReceiptMaster["Voucher_No"].ToString();
                            BRtxtDate.Text = Date.DBToSystem(drBankReceiptMaster["BankReceipt_Date"].ToString());
                        }
                        else
                        {
                            BRtxtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if recurring load today's date
                            //txtduedate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                        }

                        BRtxtRemarks.Text = drBankReceiptMaster["Remarks"].ToString();
                        BRtxtBankID.Text = drBankReceiptMaster["BankReceiptID"].ToString();
                        dsBankReceipt.Tables["tblBankReceiptMaster"].Rows.Add(BRcboSeriesName.Text, drBankReceiptMaster["Voucher_No"].ToString(), Date.DBToSystem(drBankReceiptMaster["BankReceipt_Date"].ToString()), BRcomboBankAccount.Text, drBankReceiptMaster["Remarks"].ToString());
                        DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drBankReceiptMaster["ProjectID"]), LangMgr.DefaultLanguage);
                        if (dtProjectInfo.Rows.Count > 0)
                        {
                            DataRow drProjectInfo = dtProjectInfo.Rows[0];
                            BRcboProjectName.Text = drProjectInfo["Name"].ToString();
                        }
                        //Show the corresponding Bank Account Ledger in Combobox
                        DataTable dtBankLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankReceiptMaster["LedgerID"]), LangMgr.DefaultLanguage);
                        DataRow drBankLedgerInfo = dtBankLedgerInfo.Rows[0];

                        BRcomboBankAccount.Text = drBankLedgerInfo["LedName"].ToString();


                        //For Additional Fields
                        if (NumberOfFields > 0)
                        {
                            if (NumberOfFields == 1)
                            {
                                BRlblfirst.Visible = true;
                                BRtxtfirst.Visible = true;
                                BRlblsecond.Visible = false;
                                BRtxtsecond.Visible = false;
                                BRlblthird.Visible = false;
                                BRtxtthird.Visible = false;
                                BRlblfourth.Visible = false;
                                BRtxtfourth.Visible = false;
                                BRlblfifth.Visible = false;
                                BRtxtfifth.Visible = false;
                                BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();

                                BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                                BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                                BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                                BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                                BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();
                            }
                            else if (NumberOfFields == 2)
                            {
                                BRlblfirst.Visible = true;
                                BRtxtfirst.Visible = true;
                                BRlblsecond.Visible = true;
                                BRtxtsecond.Visible = true;
                                BRlblthird.Visible = false;
                                BRtxtthird.Visible = false;
                                BRlblfourth.Visible = false;
                                BRtxtfourth.Visible = false;
                                BRlblfifth.Visible = false;
                                BRtxtfifth.Visible = false;
                                BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();

                                BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                                BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                                BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                                BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                                BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();
                            }
                            else if (NumberOfFields == 3)
                            {
                                BRlblfirst.Visible = true;
                                BRtxtfirst.Visible = true;
                                BRlblsecond.Visible = true;
                                BRtxtsecond.Visible = true;
                                BRlblthird.Visible = true;
                                BRtxtthird.Visible = true;
                                BRlblfourth.Visible = false;
                                BRtxtfourth.Visible = false;
                                BRlblfifth.Visible = false;
                                BRtxtfifth.Visible = false;
                                BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                BRlblthird.Text = drdtadditionalfield["Field3"].ToString();

                                BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                                BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                                BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                                BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                                BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();

                            }
                            else if (NumberOfFields == 4)
                            {
                                BRlblfirst.Visible = true;
                                BRtxtfirst.Visible = true;
                                BRlblsecond.Visible = true;
                                BRtxtsecond.Visible = true;
                                BRlblthird.Visible = true;
                                BRtxtthird.Visible = true;
                                BRlblfourth.Visible = true;
                                BRtxtfourth.Visible = true;
                                BRlblfifth.Visible = false;
                                BRtxtfifth.Visible = false;
                                BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                BRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                                BRlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                                BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                                BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                                BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                                BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                                BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();

                            }
                            else if (NumberOfFields == 5)
                            {
                                BRlblfirst.Visible = true;
                                BRtxtfirst.Visible = true;
                                BRlblsecond.Visible = true;
                                BRtxtsecond.Visible = true;
                                BRlblthird.Visible = true;
                                BRtxtthird.Visible = true;
                                BRlblfourth.Visible = true;
                                BRtxtfourth.Visible = true;
                                BRlblfifth.Visible = true;
                                BRtxtfifth.Visible = true;

                                BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                BRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                                BRlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                                BRlblfifth.Text = drdtadditionalfield["Field5"].ToString();

                                BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                                BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                                BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                                BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                                BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();
                            }


                        }
                        DataTable dtBankReceiptDetail = m_BankReceipt.GetBankReceiptDetail(vouchID);

                        for (int i = 1; i <= dtBankReceiptDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtBankReceiptDetail.Rows[i - 1];

                            grdBankReceipt[i, 1].Value = i.ToString();
                            grdBankReceipt[i, 2].Value = drDetail["LedgerName"].ToString();
                            grdBankReceipt[i, 3].Value = Convert.ToDecimal(drDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            grdBankReceipt[i, 5].Value = drDetail["Remarks"].ToString();
                            grdBankReceipt[i, 6].Value = drDetail["ChequeNumber"].ToString();
                            //    grdBankReceipt[i, 6].Value = drDetail["ChequeBank"].ToString();
                            //    if (drDetail["ChequeDate"].ToString() == "")
                            //    {
                            //        grdBankReceipt[i, 7].Value = "";
                            //    }
                            //    else
                            //    {
                            //        grdBankReceipt[i, 7].Value = Date.DBToSystem(drDetail["ChequeDate"].ToString());
                            //    }
                            //    AddRowBankReceipt(grdBankReceipt.RowsCount);

                            //    dsBankReceipt.Tables["tblBankReceiptDetails"].Rows.Add(drDetail["LedgerName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                            //}

                            grdBankReceipt[i, 7].Value = drDetail["ChequeBank"].ToString();
                            grdBankReceipt[i, 11].Value = drDetail["VoucherType"].ToString();
                            grdBankReceipt[i, 12].Value = drDetail["VoucherNumber"].ToString();
                            grdBankReceipt[i, 4].Value = drDetail["Remarks"].ToString();
                            grdBankReceipt[i, 9].Value = drDetail["LedgerID"].ToString();

                            //grdBankReceipt[i, 4].Value = drDetail["Remarks"].ToString();
                            //grdBankReceipt[i, 5].Value = drDetail["ChequeNumber"].ToString();
                            //grdBankReceipt[i, 6].Value = drDetail["ChequeBank"].ToString();
                            //Code To Get The Current Balance of the Respective Ledger
                            DataTable dtLdrInfo = Ledger.GetLedgerDetail("1", null, null, Convert.ToInt32(drDetail["LedgerID"]));
                            if (dtLdrInfo.Rows.Count != 1)
                            {
                                grdBankReceipt[i, 4].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdBankReceipt[i, 10].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            }
                            else
                            {
                                DataRow drLdrInfo = dtLdrInfo.Rows[0];
                                if (drLdrInfo["Debit"] == DBNull.Value || Convert.ToInt32(drLdrInfo["Debit"]) == 0)
                                {
                                    if (drLdrInfo["Credit"] == DBNull.Value || Convert.ToInt32(drLdrInfo["Credit"]) == 0)
                                    {
                                        grdBankReceipt[i, 4].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                        grdBankReceipt[i, 10].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                    }
                                    else
                                    {
                                        grdBankReceipt[i, 4].Value = Convert.ToDecimal(drLdrInfo["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                        grdBankReceipt[i, 10].Value = Convert.ToDecimal(drLdrInfo["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                    }
                                }
                                else
                                {
                                    if (drLdrInfo["Credit"] == DBNull.Value || Convert.ToInt32(drLdrInfo["Credit"]) == 0)
                                    {
                                        grdBankReceipt[i, 4].Value = Convert.ToDecimal(drLdrInfo["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                        grdBankReceipt[i, 10].Value = Convert.ToDecimal(drLdrInfo["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                    }
                                    else
                                    {
                                        if (Convert.ToDecimal(drLdrInfo["Debit"]) == Convert.ToDecimal(drLdrInfo["Credit"]))
                                        {
                                            grdBankReceipt[i, 4].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                            grdBankReceipt[i, 10].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                        }

                                        if (Convert.ToDecimal(drLdrInfo["Debit"]) > Convert.ToDecimal(drLdrInfo["Credit"]))
                                        {
                                            grdBankReceipt[i, 4].Value = (Convert.ToDecimal(drLdrInfo["Debit"]) - Convert.ToDecimal(drLdrInfo["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                            grdBankReceipt[i, 10].Value = (Convert.ToDecimal(drLdrInfo["Debit"]) - Convert.ToDecimal(drLdrInfo["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                        }
                                        if (Convert.ToDecimal(drLdrInfo["Debit"]) < Convert.ToDecimal(drLdrInfo["Credit"]))
                                        {
                                            grdBankReceipt[i, 4].Value = (Convert.ToDecimal(drLdrInfo["Credit"]) - Convert.ToDecimal(drLdrInfo["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                            grdBankReceipt[i, 10].Value = (Convert.ToDecimal(drLdrInfo["Credit"]) - Convert.ToDecimal(drLdrInfo["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                        }
                                    }
                                }
                            }

                            if (drDetail["ChequeDate"].ToString() == "")
                            {
                                grdBankReceipt[i, 8].Value = "";
                            }
                            else
                            {
                                grdBankReceipt[i, 8].Value = Date.DBToSystem(drDetail["ChequeDate"].ToString());
                            }
                            BRAddRowBankReceipt(grdBankReceipt.RowsCount);
                            dsBankReceipt.Tables["tblBankReceiptDetails"].Rows.Add(drDetail["LedgerName"].ToString(), (Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                            totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                            totalRptAmt = (Convert.ToDecimal(totalAmt)).ToString();
                        }
                        IsFieldChanged = false;
                    }


                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }

                #endregion

                // if recurring is true then donot load recurring settings for new voucher
                if (!m_isRecurring)
                    BRCheckRecurringSetting(BRtxtBankID.Text);

            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        private void BRcmboBankAccount_IndexChanged(object sender, EventArgs e)
        {

            liBankID = (ListItem)BRcomboBankAccount.SelectedItem;
            #region BLOCK FOR CALCULATIING OPENING BALANCE
            GetOpeningBalance openbal = new GetOpeningBalance();
            int uid = User.CurrUserID;
            DataTable dtroleinfo = User.GetUserInfo(uid);
            DataRow drrole = dtroleinfo.Rows[0];
            int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());
            int ParentAcc = 0;
            DefaultAccClass = Convert.ToInt32(UserPreference.GetValue("ACCOUNT_CLASS", uid));
            ParentAcc = GetRootAccClassIDD();
            //if (roleid == 37)
            //{
            //    ParentAcc = Convert.ToInt32(Settings.GetSettings("ACCOUNT_CLASS"));
            //}
            //else
            //{
            //    ParentAcc = Convert.ToInt32(UserPreference.GetValue("ACCOUNT_CLASS", uid));
            //}
            dtGetOpeningBalance = openbal.GetOpeningBalanceByParent(ParentAcc, liBankID.ID);

            //GetOpeningBalance openbal = new GetOpeningBalance();
            //dtGetOpeningBalance = openbal.GetOpeningBalanceByParent(Global.ParentAccClassID, liBankID.ID);
            #endregion
            DataTable dtBankCurrBalance = new DataTable();
            dtBankCurrBalance = Ledger.GetLedgerDetail("1", null, null, liBankID.ID);
            if (dtBankCurrBalance.Rows.Count <= 0)
            {
                if (dtGetOpeningBalance.Rows.Count > 0)
                {
                    DataRow dropeningBal = dtGetOpeningBalance.Rows[0];
                    if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                    {
                        BRlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        BRlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                    }
                    else
                    {
                        BRlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        BRlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                    }
                }
                else
                {
                    BRlblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    BRlblBankCurrBalHidden.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }
                //lblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //lblBankCurrBalHidden.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                BRFilllblCurrentBankBalance();
                return;
            }
            DataRow dr = dtBankCurrBalance.Rows[0];
            if (dtGetOpeningBalance.Rows.Count > 0)
            {
                DataRow dropeningBal = dtGetOpeningBalance.Rows[0];
                if (dr["Debit"] == DBNull.Value || Convert.ToInt32(dr["Debit"]) == 0)
                {
                    if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                    {
                        if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                        {
                            BRlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                            BRlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        }
                        else
                        {
                            BRlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                            BRlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        }
                        //lblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        //lblBankCurrBalHidden.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    else
                    {
                        if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                        {
                            decimal openbalance = Convert.ToDecimal(dropeningBal["OpenBal"].ToString());
                            decimal creditamt = Convert.ToDecimal(dr["Credit"].ToString());

                            if (openbalance > creditamt)
                            {
                                BRlblBankCurrentBalance.Text = (Convert.ToDecimal(dropeningBal["OpenBal"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                                BRlblBankCurrBalHidden.Text = (Convert.ToDecimal(dropeningBal["OpenBal"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                            }
                            else
                            {
                                BRlblBankCurrentBalance.Text = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                                BRlblBankCurrBalHidden.Text = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                            }

                        }
                        else
                        {
                            BRlblBankCurrentBalance.Text = (Convert.ToDecimal(dr["Credit"]) + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                            BRlblBankCurrBalHidden.Text = (Convert.ToDecimal(dr["Credit"]) + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        }
                        //lblBankCurrentBalance.Text = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        //lblBankCurrBalHidden.Text = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                    }
                }
                else
                {
                    if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                    {
                        if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                        {
                            BRlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                            BRlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        }
                        else
                        {
                            BRlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                            BRlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        }
                        //lblBankCurrentBalance.Text = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        // lblBankCurrBalHidden.Text = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                    }
                    else
                    {
                        if (Convert.ToDecimal(dr["Debit"]) == Convert.ToDecimal(dr["Credit"]))
                        {
                            if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                            {
                                BRlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                                BRlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                            }
                            else
                            {
                                BRlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                                BRlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                            }
                            //lblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            //lblBankCurrBalHidden.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }

                        if (Convert.ToDecimal(dr["Debit"]) > Convert.ToDecimal(dr["Credit"]))
                        {
                            if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                            {
                                BRlblBankCurrentBalance.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"]) + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                                BRlblBankCurrBalHidden.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"]) + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                            }
                            else
                            {
                                decimal openbalance = Convert.ToDecimal(dropeningBal["OpenBal"].ToString());
                                decimal amt = Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"]);

                                if (openbalance > amt)
                                {
                                    BRlblBankCurrentBalance.Text = (Convert.ToDecimal(dropeningBal["OpenBal"]) - amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                                    BRlblBankCurrBalHidden.Text = (Convert.ToDecimal(dropeningBal["OpenBal"]) - amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                                }
                                else
                                {
                                    BRlblBankCurrentBalance.Text = (amt - Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                                    BRlblBankCurrBalHidden.Text = (amt - Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                                }
                                //lblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                                //lblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                            }
                            //lblBankCurrentBalance.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                            //lblBankCurrBalHidden.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        }
                        if (Convert.ToDecimal(dr["Debit"]) < Convert.ToDecimal(dr["Credit"]))
                        {
                            if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                            {
                                decimal openbalance = Convert.ToDecimal(dropeningBal["OpenBal"].ToString());
                                decimal amt = Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"]);

                                if (openbalance > amt)
                                {
                                    BRlblBankCurrentBalance.Text = (Convert.ToDecimal(dropeningBal["OpenBal"]) - amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                                    BRlblBankCurrBalHidden.Text = (Convert.ToDecimal(dropeningBal["OpenBal"]) - amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                                }
                                else
                                {
                                    BRlblBankCurrentBalance.Text = (amt - Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                                    BRlblBankCurrBalHidden.Text = (amt - Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                                }
                            }
                            else
                            {
                                BRlblBankCurrentBalance.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"]) + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                                BRlblBankCurrBalHidden.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"]) + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                            }
                            //lblBankCurrentBalance.Text = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                            //lblBankCurrBalHidden.Text = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        }
                    }
                }

                BRFilllblCurrentBankBalance();
            }
            else
            {
                if (dr["Debit"] == DBNull.Value || Convert.ToInt32(dr["Debit"]) == 0)
                {
                    if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                    {
                        BRlblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        BRlblBankCurrBalHidden.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    else
                    {
                        BRlblBankCurrentBalance.Text = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        BRlblBankCurrBalHidden.Text = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                    }
                }
                else
                {
                    if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                    {
                        BRlblBankCurrentBalance.Text = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        BRlblBankCurrBalHidden.Text = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                    }
                    else
                    {
                        if (Convert.ToDecimal(dr["Debit"]) == Convert.ToDecimal(dr["Credit"]))
                        {
                            BRlblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            BRlblBankCurrBalHidden.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }

                        if (Convert.ToDecimal(dr["Debit"]) > Convert.ToDecimal(dr["Credit"]))
                        {
                            BRlblBankCurrentBalance.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                            BRlblBankCurrBalHidden.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        }
                        if (Convert.ToDecimal(dr["Debit"]) < Convert.ToDecimal(dr["Credit"]))
                        {
                            BRlblBankCurrentBalance.Text = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                            BRlblBankCurrBalHidden.Text = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        }
                    }
                    BRFilllblCurrentBankBalance();
                }
                // if recurring is true then donot load recurring settings for new voucher
                if (!m_isRecurring)
                    BRCheckRecurringSetting(BRtxtBankID.Text);
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
        private void BRText_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
        }

        private void BRDelete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = grdBankReceipt.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdBankReceipt.RowsCount - 2)
            {
                #region Reference related
                if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(BRtxtBankID.Text), Convert.ToInt32(grdBankReceipt[CurRow, (int)GridColumnBR.BRLedger_ID].Value), "JRNL"))
                {
                    Global.MsgError("You must delete all other vouchers with reference against this voucher to delete this transaction!");
                    return;
                }

                grdBankReceipt.Rows.Remove(ctx.Position.Row);
                #endregion
            }
        }

        private void BRChequeDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = true;
            ctx1 = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx1.Position.Row;
            if (grdBankReceipt[CurrRowPos, 8].DisplayText.ToString() != "")
            {
                //frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Convert.ToDateTime(ctx1.Value))));
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(ctx1.Value.ToString()));
                frm.ShowDialog();
            }
            else
            {
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Date.GetServerDate())));
                frm.ShowDialog();
            }
        }

     
        private void BRAccount_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
        }
        bool isNewReferenceVoucher = false, isAgainstRef = false;
        private void BRAccount_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                int CurRow = grdBankReceipt.Selection.GetSelectionRegion().GetRowsIndex()[0];

                #region Reference related
                // if new voucher reference is created for this, donot open the reference form 
                if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(BRtxtBankID.Text), Convert.ToInt32(grdBankReceipt[CurRow, (int)GridColumnBR.BRLedger_ID].Value), "BANK_RCPT"))
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
        decimal amt = 0;
        bool amt_Focused_First = true;
        /// <summary>
        /// used to store current value of Amount before edting it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRAmount_Focused(object sender, EventArgs e)
        {
            try
            {
                if (amt_Focused_First)
                {
                    int RowCount = grdBankReceipt.RowsCount;
                    int CurRow = grdBankReceipt.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    amt = Convert.ToDecimal(grdBankReceipt[CurRow, (int)GridColumnBR.BRAmount].Value);
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


        private void BRevtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            int ledID = 0, vouchID = 0;
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            int ledgerID = 0;
            string CurrentBal = "";
            try
            {
                if (grdBankReceipt[CurrRowPos, (int)GridColumnBR.BRParticular_Account_Head].Value.ToString() == "(NEW)" || grdBankReceipt[CurrRowPos, (int)GridColumnBR.BRParticular_Account_Head].Value.ToString() == "")
                    return;
                try
                {
                    ledgerID = Convert.ToInt32(grdBankReceipt[CurrRowPos, (int)GridColumnBR.BRLedger_ID].Value);
                    CurrentBal = grdBankReceipt[CurrRowPos, (int)GridColumnBR.BRCurrent_Bal_Actual].Value.ToString();
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

                if (BRtxtBankID.Text != "")
                {
                    vouchID = Convert.ToInt32(BRtxtBankID.Text);
                }

                frmReference fr = new frmReference(this, vouchID, "BANK_RCPT", ledID);
                fr.ShowDialog();
                //SendKeys.Send("{Tab}");
            }
            #endregion

            BRFillAllGridRow(CurrRowPos, CurrAccLedgerID, CurrBal);
        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmLOVLedger frm = new frmLOVLedger(this, e);
            frm.ShowDialog();
        }

        private void BRFillAllGridRow(int RowPosition, int LdrID, string CurrBalance)
        {
            decimal TempAmount = 0;
            string CurrentLedgerBalance = CurrBalance.ToString();
            string[] CurrentBalance = CurrBalance.ToString().Split('(');

            try
            {
                TempAmount = Convert.ToDecimal(grdBankReceipt[Convert.ToInt32(RowPosition), 3].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                grdBankReceipt[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }

            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                grdBankReceipt[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                grdBankReceipt[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else
            {
                grdBankReceipt[RowPosition, (int)GridColumnBR.BRCurrent_Balance].Value = CurrBalance;
            }
            grdBankReceipt[RowPosition, (int)GridColumnBR.BRLedger_ID].Value = LdrID;
            grdBankReceipt[RowPosition, (int)GridColumnBR.BRCurrent_Bal_Actual].Value = CurrBalance;
            CurrAccLedgerID = 0;
            CurrBal = "";
        }

        private void BRFillGridRowExceptLedgerID(int RowPosition, int LdrID, string CurrBal, decimal amt)
        {
            try
            {
                amt_Focused_First = true;
                decimal TempAmount = 0;
                string CurrentLedgerBalance = "";
                string[] CurrentBalance = new string[2];
                decimal temporary = 0;

                if (CurrBal == "")
                {
                    CurrentLedgerBalance = grdBankReceipt[Convert.ToInt32(RowPosition), 10].ToString();
                    CurrentBalance = grdBankReceipt[Convert.ToInt32(RowPosition), 10].ToString().Split('(');
                }
                else
                {
                    CurrentLedgerBalance = CurrBal.ToString();
                    CurrentBalance = CurrBal.ToString().Split('(');
                }
                try
                {
                    TempAmount = Convert.ToDecimal(grdBankReceipt[Convert.ToInt32(RowPosition), 3].Value);
                }
                catch
                {
                    TempAmount = 0;
                }

                // old code 
                BRFilllblCurrentBankBalance();

               
                if (CurrentLedgerBalance.Contains("Dr"))
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount) - amt);
                    temporary = Convert.ToDecimal(temporary * -1);
                }
                else if (CurrentLedgerBalance.Contains("Cr"))
                {
                   
                    temporary = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount) - amt);
                }
                        
                if (temporary < 0)
                {
                    grdBankReceipt[Convert.ToInt32(RowPosition), (int)GridColumnBR.BRCurrent_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                else if (temporary > 0)
                {
                    grdBankReceipt[Convert.ToInt32(RowPosition), (int)GridColumnBR.BRCurrent_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
                else if (temporary == 0)
                {
                    grdBankReceipt[Convert.ToInt32(RowPosition), (int)GridColumnBR.BRCurrent_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
               
                else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
                {
                   
                    grdBankReceipt[Convert.ToInt32(RowPosition), (int)GridColumnBR.BRCurrent_Balance].Value = (Convert.ToDecimal(TempAmount)+ amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    
                }
                else
                {
                    grdBankReceipt[RowPosition, (int)GridColumnBR.BRCurrent_Balance].Value = CurrBal;
                }
            
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        //it display current balance at the side of the bank account combobox
        private void BRFilllblCurrentBankBalance()
        {

            decimal TotalInputAmount = 0;
            decimal temporary = 0;
            string CurrentLedgerBalanceForBank = "";
            string[] CurrentBalanceForBank = new string[2];
            try
            {
                CurrentLedgerBalanceForBank = BRlblBankCurrBalHidden.Text;
                CurrentBalanceForBank = CurrentLedgerBalanceForBank.Split('(');
                //Get all the balance of the of the grid entry
                for (int i = 0; i < grdBankReceipt.Rows.Count - 1; i++)
                {
                    TotalInputAmount += Convert.ToDecimal(grdBankReceipt[i + 1, 3].Value);
                    BRlblTotalAmount.Text = TotalInputAmount.ToString();
                }
            }
            catch
            { }

            if (CurrentLedgerBalanceForBank.Contains("Dr"))
            {
                BRlblBankCurrentBalance.Text = (Convert.ToDecimal(CurrentBalanceForBank[0]) + Convert.ToDecimal(TotalInputAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }
            else if (CurrentLedgerBalanceForBank.Contains("Cr"))
            {
                temporary = (Convert.ToDecimal(CurrentBalanceForBank[0]) + (-1) * Convert.ToDecimal(TotalInputAmount));
                if (temporary == 0)
                    BRlblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                if (temporary < 0)
                {
                    BRlblBankCurrentBalance.Text = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
                if (temporary > 0)
                {
                    BRlblBankCurrentBalance.Text = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
            }
            else
            {
                BRlblBankCurrentBalance.Text = (TotalInputAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }
            //string test = lblBankCurrentBalance.Text;

        }

        private void BRAmount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdBankReceipt.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            string AccName = (string)(grdBankReceipt[RowCount - 1, 2].Value);
            CurrBal = (string)(grdBankReceipt[CurRow, (int)GridColumnBR.BRCurrent_Balance].Value); // updated current balance is sent for calculation of new current balance

            //Check if the input value is correct
            if (grdBankReceipt[Convert.ToInt32(CurRow), 3].Value == "" || grdBankReceipt[Convert.ToInt32(CurRow), 3].Value == null)
                grdBankReceipt[Convert.ToInt32(CurRow), 3].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            if (!Misc.IsNumeric(grdBankReceipt[Convert.ToInt32(CurRow), 3].Value))
            {
                Global.MsgError("Invalid Amount!");
                ctx.Value = "";
                return;
            }
            #region Reference related
            if (m_mode == EntryMode.EDIT)
            {
                grdBankReceipt[CurRow, (int)GridColumnBR.BRRef_Amt].Value = VoucherReference.GetAmtForAgainstRef(Convert.ToInt32(BRtxtBankID.Text), "BANK_RCPT", Convert.ToInt32(grdBankReceipt[CurRow, (int)GridColumnBR.BRLedger_ID].Value));
            }

            // check if transaction amount is greater than reference amount
            if (!BRCheckAmtAgainstRefAmt())
            {
                return;
            }
            #endregion

            double checkformat = Convert.ToDouble(grdBankReceipt[Convert.ToInt32(CurRow), 3].Value.ToString());
            //string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdBankReceipt[Convert.ToInt32(CurRow), 3].Value = checkformat;

            BRFillGridRowExceptLedgerID(CurRow, CurrAccLedgerID, CurrBal, amt);
            if (AccName != "(NEW)")
            {
                BRAddRowBankReceipt(RowCount);
            }
        }
        /// <summary>
        /// Writes the header part for grdJournal
        /// </summary>
        private void BRAddGridHeader()
        {
            grdBankReceipt[0, 0] = new MyHeaderBR("Del");
            grdBankReceipt[0, 1] = new MyHeaderBR("Code No.");
            grdBankReceipt[0, 2] = new MyHeaderBR("Particular/Accounting Head");
            grdBankReceipt[0, 3] = new MyHeaderBR("Amount");
            grdBankReceipt[0, 4] = new MyHeaderBR("CurrentBalance");
            grdBankReceipt[0, 5] = new MyHeaderBR("Remarks");
            grdBankReceipt[0, 6] = new MyHeaderBR("Cheque Number");
            grdBankReceipt[0, 7] = new MyHeaderBR("Cheque Bank");
            grdBankReceipt[0, 8] = new MyHeaderBR("Cheque Date");
            grdBankReceipt[0, 9] = new MyHeaderBR("LedgerID");
            grdBankReceipt[0, 10] = new MyHeaderBR("CurrentBalance");
            grdBankReceipt[0, 11] = new MyHeaderBR("V Type");
            grdBankReceipt[0, 12] = new MyHeaderBR("Voucher No.");

            grdBankReceipt[0, 0].Column.Width = 30;
            grdBankReceipt.Columns[(int)GridColumnBR.BRDel].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankReceipt[0, 1].Column.Width = 60;
            grdBankReceipt.Columns[(int)GridColumnBR.BRCode_No].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankReceipt[0, 2].Column.Width = 150;
            grdBankReceipt.Columns[(int)GridColumnBR.BRParticular_Account_Head].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankReceipt[0, 3].Column.Width = 100;
            grdBankReceipt.Columns[(int)GridColumnBR.BRAmount].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankReceipt[0, 4].Column.Width = 100;
            grdBankReceipt.Columns[(int)GridColumnBR.BRCurrent_Balance].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankReceipt[0, 5].Column.Width = 100;
            grdBankReceipt.Columns[(int)GridColumnBR.BRRemarks].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankReceipt[0, 6].Column.Width = 150;
            grdBankReceipt.Columns[(int)GridColumnBR.BRCheque_No].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankReceipt[0, 7].Column.Width = 100;
            grdBankReceipt.Columns[(int)GridColumnBR.BRCheque_Bank].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankReceipt[0, 8].Column.Width = 100;
            grdBankReceipt.Columns[(int)GridColumnBR.BRChequeDate].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankReceipt[0, 11].Column.Width = 80;
         

            grdBankReceipt[0, 12].Column.Width = 80;
          

            grdBankReceipt[0, 9].Column.Visible = false;
            grdBankReceipt[0, 10].Column.Visible = false;

            grdBankReceipt[0, (int)GridColumnBR.BRRef_Amt] = new SourceGrid.Cells.ColumnHeader("RefAmt");
            grdBankReceipt[0, (int)GridColumnBR.BRRef_Amt].Column.Width = 50;
            grdBankReceipt[0, (int)GridColumnBR.BRRef_Amt].Column.Visible = false;

        }

        /// <summary>
        /// Adds the row in the Journal field
        /// </summary>
        private void BRAddRowBankReceipt(int RowCount)
        {
            //Add a new row
            grdBankReceipt.Redim(Convert.ToInt32(RowCount + 1), grdBankReceipt.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            grdBankReceipt[i, 0] = btnDelete;
            grdBankReceipt[i, 0].AddController(BRevtDelete);

            grdBankReceipt[i, 1] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdBankReceipt[i, 2] = new SourceGrid.Cells.Cell("", txtAccount);
            txtAccount.Control.GotFocus += new EventHandler(BRAccount_Focused);
            txtAccount.Control.LostFocus += new EventHandler(BRAccount_Leave);
            txtAccount.Control.KeyDown += new KeyEventHandler(Account_KeyDown);
            txtAccount.Control.TextChanged += new EventHandler(BRText_Change);
            grdBankReceipt[i, 2].AddController(BRevtAccountFocusLost);
            grdBankReceipt[i, 2].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankReceipt[i, 3] = new SourceGrid.Cells.Cell("", txtAmount);
            txtAmount.Control.TextChanged += new EventHandler(BRText_Change);
            txtAmount.Control.GotFocus += new EventHandler(BRAmount_Focused);
            grdBankReceipt[i, 3].AddController(BRevtAmountFocusLost);
            grdBankReceipt[i, 3].Value = "";

            SourceGrid.Cells.Editors.TextBox txtCurrentBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrentBalance.EditableMode = SourceGrid.EditableMode.None;
            grdBankReceipt[i, 4] = new SourceGrid.Cells.Cell("", txtCurrentBalance);
            grdBankReceipt[i, 4].Value = "";

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankReceipt[i, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
            txtRemarks.Control.TextChanged += new EventHandler(BRText_Change);

            SourceGrid.Cells.Editors.TextBox txtChequeNumber = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtChequeNumber.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankReceipt[i, 6] = new SourceGrid.Cells.Cell("", txtChequeNumber);
            txtChequeNumber.Control.TextChanged += new EventHandler(BRText_Change);
            grdBankReceipt[i, 6].AddController(BRevtChequeNumberFocusLost);
            grdBankReceipt[i, 6].Value = "";

            SourceGrid.Cells.Editors.TextBox txtChequeBank = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtChequeBank.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankReceipt[i, 7] = new SourceGrid.Cells.Cell("", txtChequeBank);
            txtChequeBank.Control.TextChanged += new EventHandler(BRText_Change);
            grdBankReceipt[i, 7].AddController(BRevtChequeBankFocusLost);
            grdBankReceipt[i, 7].Value = "";

            SourceGrid.Cells.Button btnChequeDate = new SourceGrid.Cells.Button(""); //Date.ToSystem(DateTime.Today)
            txtChequeNumber.EditableMode = SourceGrid.EditableMode.SingleClick;
            grdBankReceipt[i, 8] = btnChequeDate;
            grdBankReceipt[i, 8].AddController(BRevtChequeDateFocusLost);

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdBankReceipt[i, 9] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdBankReceipt[i, 9].Value = "";

            SourceGrid.Cells.Editors.TextBox txCurrBal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txCurrBal.EditableMode = SourceGrid.EditableMode.None;
            grdBankReceipt[i, 10] = new SourceGrid.Cells.Cell("", txCurrBal);
            grdBankReceipt[i, 10].Value = "";

            SourceGrid.Cells.Editors.ComboBox combo = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            combo.StandardValues = new string[] { "SALES", "JRNL", "PURCH_RTN", "CASH_PMNT", "BANK_PMNT", "NONE" };
            combo.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.EditableMode = SourceGrid.EditableMode.Focus;

            grdBankReceipt[i, 11] = new SourceGrid.Cells.Cell("", combo);

            SourceGrid.Cells.Editors.TextBox txtVoNo = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtVoNo.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankReceipt[i, 12] = new SourceGrid.Cells.Cell("", txtVoNo);
            grdBankReceipt[i, 12].Value = "";

            SourceGrid.Cells.Editors.TextBox txtRefAmt = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            grdBankReceipt[i, (int)GridColumnBR.BRRef_Amt] = new SourceGrid.Cells.Cell("", txtRefAmt);
            grdBankReceipt[i, (int)GridColumnBR.BRRef_Amt].Value = "0(Dr)";

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

        private void BRbtnSave_Click(object sender, EventArgs e)
        {
            if (BRCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("BANK_RECEIPT_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("BANK_RECEIPT_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }


            if (Freeze.IsDateFreeze(Date.ToDotNet(BRtxtDate.Text)))
            {
                return;
            }

            #region BLOCK FOR MANUAL VOUCHER NUMBERING TYPE

            VoucherConfiguration m_VouConfig = new VoucherConfiguration();
            if (SeriesID.ID > 0)
            {
                DataTable dtVouConfigInfo = m_VouConfig.GetVouNumConfiguration(Convert.ToInt32(SeriesID.ID));
                if (dtVouConfigInfo.Rows.Count < 0)
                    return;
                DataRow drVouConfigInfo = dtVouConfigInfo.Rows[0];
                if (drVouConfigInfo["NumberingType"].ToString() == "Manual")
                {
                    try
                    {
                        string returnStr = m_VouConfig.ValidateManualVouNum(BRtxtVchNo.Text, Convert.ToInt32(SeriesID.ID));
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

            //Check Validation
            if (!BRValidate())
                return;

            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }
            if (drdtadditionalfield["IsField1Required"].ToString() == "True")
            {
                if (BRtxtfirst.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field1"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField2Required"].ToString() == "True")
            {
                if (BRtxtsecond.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field2"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField3Required"].ToString() == "True")
            {
                if (BRtxtthird.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field3"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField4Required"].ToString() == "True")
            {
                if (BRtxtfourth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field4"].ToString() + " " + "is Required Field");
                    return;
                }

            }
            if (drdtadditionalfield["IsField5Required"].ToString() == "True")
            {
                if (BRtxtfifth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field5"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            ListItem liLedgerID = new ListItem();
            liLedgerID = (ListItem)BRcomboBankAccount.SelectedItem;

            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    try
                    {
                        #region Add voucher number if voucher number is automatic and hidden from the setting
                        int increasedSeriesNum = 0;
                        SeriesID = (ListItem)BRcboSeriesName.SelectedItem;
                        string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumTypeNoUpdate(Convert.ToInt32(SeriesID.ID), out increasedSeriesNum);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers are totally finished!");
                                return;
                            }

                            BRtxtVchNo.Text = m_vounum.ToString();
                            BRtxtVchNo.Enabled = false;
                        }
                        #endregion

                        isNew = true;
                        NewGrid = " ";
                        OldGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + BRtxtVchNo.Text + "Series" + BRcboSeriesName.Text + "Project" + BRcboProjectName.Text + "Date" + BRtxtDate.Text + "Bank" + BRcomboBankAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdBankReceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdBankReceipt[i + 1, 2].Value.ToString();
                            string amt = grdBankReceipt[i + 1, 3].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        //Call to Convert into XML Format
                        string BankReceiptXMLString = BRReadAllBankReceiptEntry();
                        if (BankReceiptXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast bank receipt entry to XML!");
                            return;
                        }
                        //return;

                        #region Check Negative cash n negative bank delete after sp
                        //Read from sourcegrid and store it to table
                        DataTable BankReceiptDetails = new DataTable();
                        BankReceiptDetails.Columns.Add("Ledger");
                        BankReceiptDetails.Columns.Add("Amount");
                        BankReceiptDetails.Columns.Add("Remarks");
                        BankReceiptDetails.Columns.Add("ChequeNumber");
                        BankReceiptDetails.Columns.Add("ChequeBank");
                        BankReceiptDetails.Columns.Add("ChequeDate");
                        double totalgrdAmount = 0;
                        for (int i = 0; i < grdBankReceipt.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdBankReceipt[i + 1, 2].ToString().Split('[');

                            if (grdBankReceipt[i + 1, 5].Value == "" && grdBankReceipt[i + 1, 6].Value == "")
                            {
                                grdBankReceipt[i + 1, 7].Value = "";
                            }

                            BankReceiptDetails.Rows.Add(ledgerName[0].ToString(), grdBankReceipt[i + 1, 3].Value, grdBankReceipt[i + 1, 4].Value, grdBankReceipt[i + 1, 5].Value, grdBankReceipt[i + 1, 6].Value, grdBankReceipt[i + 1, 7].Value);
                            //Check for empty Amount
                            object objAmount = grdBankReceipt[i + 1, 3].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdBankReceipt[i + 1, 3].Value);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            //we need to check both negative cash and negative bank in Details section....All ledgers of Details section are responsible for payment soo need to check both negative cash and negative Bank

                            #region BLOCK FOR CHECKING NEGATIVE CASH
                            if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                            {
                                bool isCashAccount = false;
                                isCashAccount = Ledger.IsCashAccount((ledgerName[0].ToString()));
                                if (isCashAccount)//If Bank account
                                {
                                    double mDbalCash = 0;
                                    double mCbalCash = 0;
                                    double totalDrCash = 0;
                                    double totalCrCash = 0;

                                    int CashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);
                                    Transaction.GetLedgerBalance(null, null, CashLedgerId, ref mDbalCash, ref mCbalCash, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                    //In case of Bank Receipt,Bank Account in master section is Debit and other accounts in Detail section are bydefault Credit
                                    totalDrCash = mDbalCash;
                                    totalCrCash += (mCbalCash + Convert.ToDouble(grdBankReceipt[i + 1, 3].Value));//The ledgers of Details section will be posted as Credit,soo tatal balance will be the summation of its own balance and grid value
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
                                    //In case of Cash Receipt,Cash Account in master section is Debit and other accounts in Detail section are bydefault Credit
                                    totalDrBank = mDbalBank;//Ledgers of Details sections will be posted as Credit,so debit balance will its only own balance
                                    totalCrBank += (mCbalBank + Convert.ToDouble(grdBankReceipt[i + 1, 3].Value));
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
                        #region BLOCK FOR CHECKING NEGATIVE BANK
                        /*
                        //WE need to check only negative Bank for master Bank Ledger because this ledger is bydefault Bank soo no need to check for negative Cash
                        #region BLOCK FOR CHECKING NEGATIVE BANK
                        //execute following code when only if setting of Negative Bank is in Warn or Deny
                        if ((Global.Default_NegativeBank == NegativeBank.Warn) || (Global.Default_NegativeBank == NegativeBank.Deny))
                        {
                            //Incase of BankhReceipt and BankPayment master ledger is bydefault Bank soo we neednot to check ledger weather it falls uder Bank or not?? 
                            double mDbalBank = 0;
                            double mCbalBank = 0;
                            double totalDrBank, totalCrBank;
                            totalDrBank = totalCrBank = 0;
                            Transaction.GetLedgerBalance(Convert.ToInt32(liLedgerID.ID), ref mDbalBank, ref mCbalBank, arrNode);

                            //Incase of BankReceipt,master ledger is bydefulat Bank...and here Bank is receipt soo it would be Debit because Bank is getting amount from other account
                            //Here Total Debit amount of master is calculated from Details section by adding all amount of all account in Details section

                            totalDrBank += (mDbalBank + totalgrdAmount);//
                            totalCrBank = mCbalBank;
                            if ((totalDrBank - totalCrBank) >= 0)
                                IsNegativeBank = false;
                            else
                                IsNegativeBank = true;

                            //If -ve cash not allowed in settings

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
                        #endregion                 
                        */
                        #endregion
                        DateTime BankReceipt_Date = Date.ToDotNet(BRtxtDate.Text);
                        #endregion

                        #region Save xml data to Database
                        int returnJournalId = 0;
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            //using (SqlCommand dbCommand = new SqlCommand("Acc.xmlBankReceiptInsert", Global.m_db.cn))
                            //{

                            //    // we are going to use store procedure  
                            //    dbCommand.CommandType = CommandType.StoredProcedure;
                            //    // Add input parameter and set its properties.
                            //    SqlParameter parameter = new SqlParameter();
                            //    // Store procedure parameter name  
                            //    parameter.ParameterName = "@bankreceipt";
                            //    // Parameter type as XML 
                            //    parameter.DbType = DbType.Xml;
                            //    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                            //    parameter.Value = BankReceiptXMLString; // XML string as parameter value  

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

                              //  Global.m_db.cn.Open();
                              //  int intRetValue = dbCommand.ExecuteNonQuery();
                            //    returnJournalId = Convert.ToInt32(parameter1.Value);

                            //To insert BankReceipt
                                returnJournalId =  BankReceipt.InsertBankREceipt(BankReceiptXMLString);
                                // to send recurring settings
                                BankReceipt.InsertVoucherRecurring(m_dtRecurringSetting, returnJournalId);
                                //MessageBox.Show(intRetValue.ToString()); 

                                // to send reference information
                                BankReceipt.InsertReference(returnJournalId, dtReference);
 
                            //}
                        }
                        #endregion
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }

                        Global.Msg("Bank Receipt created successfully!");
                        //AccClassID.Clear();
                        // if the voucher is recurring and has been posted or saved, modify voucherposting table to set isPosted = true
                        string res;
                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_BankReceiptID, "BANK_RECEIPT");
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID);
                            m_isRecurring = false;
                        }
                        BRClearVoucher();
                        BRChangeState(EntryMode.NEW);


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
                        NewGrid = NewGrid + "Voucher No" + BRtxtVchNo.Text + "Series" + BRcboSeriesName.Text + "Project" + BRcboProjectName.Text + "Date" + BRtxtDate.Text + "Bank" + BRcomboBankAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdBankReceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdBankReceipt[i + 1, 2].Value.ToString();
                            string amt = grdBankReceipt[i + 1, 3].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        //Call to Convert into XML Format
                        string BankReceiptXMLString = BRReadAllBankReceiptEntry();
                        if (BankReceiptXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast bank receipt entry to XML!");
                            return;
                        }

                        #region negative bank  and cash check
                        //Read from sourcegrid and store it to table
                        DataTable BankReceiptDetails = new DataTable();
                        BankReceiptDetails.Columns.Add("Ledger");
                        BankReceiptDetails.Columns.Add("Amount");
                        BankReceiptDetails.Columns.Add("Remarks");
                        BankReceiptDetails.Columns.Add("ChequeNumber");
                        BankReceiptDetails.Columns.Add("ChequeBank");
                        BankReceiptDetails.Columns.Add("ChequeDate");
                        double totalgrdAmount = 0;
                        for (int i = 0; i < grdBankReceipt.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdBankReceipt[i + 1, 2].ToString().Split('[');
                            if (grdBankReceipt[i + 1, 5].Value == "" || grdBankReceipt[i + 1, 6].Value == "")
                            {
                                grdBankReceipt[i + 1, 7].Value = "";
                            }
                            BankReceiptDetails.Rows.Add(ledgerName[0].ToString(), grdBankReceipt[i + 1, 3].Value, grdBankReceipt[i + 1, 4].Value, grdBankReceipt[i + 1, 5].Value, grdBankReceipt[i + 1, 6].Value, grdBankReceipt[i + 1, 7].Value);
                            //Check for empty Amount
                            object objAmount = grdBankReceipt[i + 1, 3].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdBankReceipt[i + 1, 3].Value);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            //we need to check both negative cash and negative bank in Details section...    //we need to check both negative cash and negative bank in Details section....All ledgers of Details section are responsible for payment soo need to check both negative cash and negative Bank

                            #region BLOCK FOR CHECKING NEGATIVE CASH
                            if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                            {
                                bool isCashAccount = false;
                                isCashAccount = Ledger.IsCashAccount((ledgerName[0].ToString()));
                                if (isCashAccount)//If Bank account
                                {
                                    double mDbalCash = 0;
                                    double mCbalCash = 0;
                                    double totalDrCash = 0;
                                    double totalCrCash = 0;

                                    int CashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);
                                    Transaction.GetLedgerBalance(null, null, CashLedgerId, ref mDbalCash, ref mCbalCash, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                    //In case of Bank Receipt,Bank Account in master section is Debit and other accounts in Detail section are bydefault Credit
                                    totalDrCash = mDbalCash;
                                    totalCrCash += (mCbalCash + Convert.ToDouble(grdBankReceipt[i + 1, 3].Value));//The ledgers of Details section will be posted as Credit,soo tatal balance will be the summation of its own balance and grid value
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
                                    //In case of Cash Receipt,Cash Account in master section is Debit and other accounts in Detail section are bydefault Credit
                                    totalDrBank = mDbalBank;//Ledgers of Details sections will be posted as Credit,so debit balance will its only own balance
                                    totalCrBank += (mCbalBank + Convert.ToDouble(grdBankReceipt[i + 1, 3].Value));
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
                        #region BLOCK FOR CHECKING NEGATIVE BANK
                        /*
                        //WE need to check only negative Bank for master Bank Ledger because this ledger is bydefault Bank soo no need to check for negative Cash
                        #region BLOCK FOR CHECKING NEGATIVE BANK
                        //execute following code when only if setting of Negative Bank is in Warn or Deny
                        if ((Global.Default_NegativeBank == NegativeBank.Warn) || (Global.Default_NegativeBank == NegativeBank.Deny))
                        {
                            //Incase of BankhReceipt and BankPayment master ledger is bydefault Bank soo we neednot to check ledger weather it falls uder Bank or not?? 
                            double mDbalBank = 0;
                            double mCbalBank = 0;
                            double totalDrBank, totalCrBank;
                            totalDrBank = totalCrBank = 0;
                            Transaction.GetLedgerBalance(Convert.ToInt32(liLedgerID.ID), ref mDbalBank, ref mCbalBank, arrNode);

                            //Incase of BankReceipt,master ledger is bydefulat Bank...and here Bank is receipt soo it would be Debit because Bank is getting amount from other account
                            //Here Total Debit amount of master is calculated from Details section by adding all amount of all account in Details section

                            totalDrBank += (mDbalBank + totalgrdAmount);//
                            totalCrBank = mCbalBank;
                            if ((totalDrBank - totalCrBank) >= 0)
                                IsNegativeBank = false;
                            else
                                IsNegativeBank = true;

                            //If -ve cash not allowed in settings

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
                        #endregion                 
                        */
                        #endregion
                        #endregion

                        //BankReceipt m_BankReceipt = new BankReceipt();
                        //if (!m_BankReceipt.RemoveBankReceiptEntry(Convert.ToInt32(txtBankID.Text)))
                        //{
                        //    MessageBox.Show("Unable to Update record. Please restart the modification");
                        //    return;
                        //}

                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            //using (SqlCommand dbCommand = new SqlCommand("Acc.xmlBankReceiptUpdate", Global.m_db.cn))
                            //{
                            //    // we are going to use store procedure  
                            //    dbCommand.CommandType = CommandType.StoredProcedure;
                            //    // Add input parameter and set its properties.
                            //    SqlParameter parameter = new SqlParameter();
                            //    // Store procedure parameter name  
                            //    parameter.ParameterName = "@bankreceipt";
                            //    // Parameter type as XML 
                            //    parameter.DbType = DbType.Xml;
                            //    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                            //    parameter.Value = BankReceiptXMLString; // XML string as parameter value  
                            //    if (Global.m_db.cn.State == ConnectionState.Open)
                            //    {
                            //        Global.m_db.cn.Close();
                            //    }
                            //    // Add the parameter in Parameters collection.
                            //    dbCommand.Parameters.Add(parameter);
                            //    Global.m_db.cn.Open();
                            //    int intRetValue = dbCommand.ExecuteNonQuery();
                            //    //MessageBox.Show(intRetValue.ToString());  
   
                                //to edit BankReceipt
                                BankReceipt.EditBankReceipt(BankReceiptXMLString);
                                // to send recurring settings
                                BankReceipt.ModifyVoucherRecurring(m_dtRecurringSetting, Convert.ToInt32(BRtxtBankID.Text));

                                // to modify against references in the voucher
                                BankReceipt.ModifyReference(Convert.ToInt32(BRtxtBankID.Text), dtReference, ToDeleteRows);
                           // }
                        }
                        #endregion

                        Global.Msg("Bank Receipt modified successfully!");
                        //AccClassID.Clear();
                        BRtxtBankID.Clear();
                        BRClearVoucher();
                        BRChangeState(EntryMode.NEW);

                        #region delete after successful use of sp
                        //BankReceipt m_BankReceipt = new BankReceipt();
                        //DateTime BankReceiptDate = Date.ToDotNet(txtDate.Text);                       

                        //ListItem LiSeriesID = new ListItem();
                        //LiSeriesID = (ListItem)cboSeriesName.SelectedItem;
                        //ListItem liProjectID = new ListItem();
                        //liProjectID = (ListItem)cboProjectName.SelectedItem;
                        //if (AccClassID.Count != 0)
                        //{
                        //    m_BankReceipt.Modify(Convert.ToInt32(txtBankID.Text), Convert.ToInt32(LiSeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, BankReceiptDate, txtRemarks.Text, BankReceiptDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID));
                        //}
                        //else
                        //{
                        //    int[] a = new int[] { 1 };
                        //    m_BankReceipt.Modify(Convert.ToInt32(txtBankID.Text), Convert.ToInt32(LiSeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, BankReceiptDate, txtRemarks.Text, BankReceiptDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID));

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
            if (BRchkPrntWhileSaving.Checked)
            {
                prntDirect = 1;
                BRNavigation(Navigate.Last);
                btnPrint_Click(sender, e);
                BRClearVoucher();
                BRChangeState(EntryMode.NEW);
                btnNew_Click(sender, e);
            }
            if (!BRchkDoNotClose.Checked)
                this.Close();
        }

        /// <summary>
        /// Read all journal Entry
        /// </summary>
        /// <returns></returns>
        private string BRReadAllBankReceiptEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            SeriesID = (ListItem)BRcboSeriesName.SelectedItem;
            liProjectID = (ListItem)BRcboProjectName.SelectedItem;
            liBankID = (ListItem)BRcomboBankAccount.SelectedItem;

            //validate grid entry
            if (!BRValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();
            #region  Bank Receipt
            tw.WriteStartElement("BANKRECEIPT");
            {
                ///For Bank Receipt Master Section  
                string first = BRtxtfirst.Text;
                string second = BRtxtsecond.Text;
                string third = BRtxtthird.Text;
                string fourth = BRtxtfourth.Text;
                string fifth = BRtxtfifth.Text;
                int SID = System.Convert.ToInt32(SeriesID.ID);
                int BankID = System.Convert.ToInt32(liBankID.ID);
                int BankPaymentID = BRtxtBankID.Text == "" ? 0 : System.Convert.ToInt32(BRtxtBankID.Text);
                string Voucher_No = System.Convert.ToString(BRtxtVchNo.Text);
                DateTime BankReceipt_Date = Date.ToDotNet(BRtxtDate.Text);
                string Remarks = System.Convert.ToString(BRtxtRemarks.Text);
                int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                DateTime Created_Date = System.Convert.ToDateTime(Date.GetServerDate());
                string Created_By = User.CurrentUserName;
                DateTime Modified_Date = System.Convert.ToDateTime(Date.GetServerDate());
                //string Modified_By = System.Convert.ToString("Admin");
                string Modified_By = User.CurrentUserName;
                tw.WriteStartElement("BANKRECEIPTMASTER");
                tw.WriteElementString("SeriesID", SID.ToString());
                tw.WriteElementString("LedgerID", BankID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("BankReceiptID", BankPaymentID.ToString()); // for edit mode
                tw.WriteElementString("BankReceipt_Date", Date.ToDB(BankReceipt_Date));
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
                //int BankReceiptID = 0;
                int LedgerID = 0;
                Decimal Amount = 0;
                string RemarksDetail = "";
                string ChequeNumber = "";
                string ChequeBank = "";
                string ChequeDate = "";
                string VoucherType = "";
                string VoucherNumber = "";
                DateTime ChequeDateValue;
                tw.WriteStartElement("BANKRECEIPTDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    BankID = BRtxtBankID.Text == "" ? 0 : System.Convert.ToInt32(BRtxtBankID.Text);
                    LedgerID = System.Convert.ToInt32(grdBankReceipt[i + 1, 9].Value);
                    Amount = System.Convert.ToDecimal(grdBankReceipt[i + 1, 3].Value);
                    RemarksDetail = System.Convert.ToString(grdBankReceipt[i + 1, 5].Value);
                    ChequeNumber = System.Convert.ToString(grdBankReceipt[i + 1, 6].Value);
                    ChequeBank = System.Convert.ToString(grdBankReceipt[i + 1, 7].Value);
                    VoucherType = System.Convert.ToString(grdBankReceipt[i + 1, 11].Value);
                    VoucherNumber = System.Convert.ToString(grdBankReceipt[i + 1, 12].Value);
                    if (ChequeNumber == "")
                    {
                        ChequeDate = "";
                        ChequeDateValue = Date.GetServerDate();
                    }
                    //ChequeDate = null;
                    else
                    {
                        //ChequeDate = System.Convert.ToString(grdBankReceipt[i + 1, 8].Value);
                        ChequeDate = grdBankReceipt[i + 1, 8].Value.ToString();
                        ChequeDateValue = Date.ToDotNet(ChequeDate);
                    }

                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("BankReceiptID", BankID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteElementString("ChequeNumber", ChequeNumber.ToString());
                    tw.WriteElementString("ChequeBank", ChequeBank.ToString());
                    tw.WriteElementString("VoucherType", VoucherType.ToString());
                    tw.WriteElementString("VoucherNumber", VoucherNumber.ToString());
                    // DateTime BankReceipt_Date = Date.ToDotNet(txtDate.Text);
                    // tw.WriteElementString("BankReceipt_Date", Date.ToDB(BankReceipt_Date));
                    if (ChequeDate != "")
                        // tw.WriteElementString("ChequeDate", Date.ToDotNet(ChequeDate).ToString());
                        tw.WriteElementString("ChequeDate", Date.ToDB(ChequeDateValue));
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
                }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            //MessageBox.Show(strXML);
            return strXML;
        }

        //It Validates all the entry in the grid Only valid rows are count and validate
        private bool BRValidateGrid()
        {
            int[] LdrID = new int[20];
            decimal[] Amt = new decimal[20];

            //Validate input grid record
            for (int i = 0; i < grdBankReceipt.Rows.Count - 1; i++)
            {
                try
                {
                    //if ledger ID repeats then message it
                    // if LedgerID is not present in between them
                    int tempValue = 0;
                    decimal tempDecValue = 0;
                    try
                    {
                        tempValue = System.Convert.ToInt32(grdBankReceipt[i + 1, 9].Value);
                    }
                    catch (Exception ex)
                    {
                        tempValue = 0;
                    }
                    try
                    {
                        tempDecValue = System.Convert.ToDecimal(grdBankReceipt[i + 1, 3].Value);
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
                        if (i + 2 == grdBankReceipt.Rows.Count && grdBankReceipt[i + 1, 2].Value.ToString() == "(NEW)")
                        {
                            //Do Nothing
                        }
                        else
                            return false;
                    }
                    else
                        LdrID[i] = tempValue;

                    if (i + 2 == grdBankReceipt.Rows.Count && grdBankReceipt[i + 1, 2].Value.ToString() == "(NEW)")
                    {
                        //Donothing
                    }
                    else
                        Amt[i] = tempDecValue;

                    liBankID = (ListItem)BRcomboBankAccount.SelectedItem;
                    if (LdrID.Contains(liBankID.ID))
                    {
                        MessageBox.Show("Same bank transaction is invalid!");
                        return false;
                    }
                }

                catch
                {
                    return false;
                }
            }
            OnlyReqdDetailRows = LdrID.Count(i => i != 0);
            return true;

            return true;
        }

        private bool BRValidate()
        {
            bool bValidate = false;
            FormHandle m_FH = new FormHandle();
            bValidate = m_FH.Validate();
            if (BRcboSeriesName.SelectedItem == null)
            {
                Global.MsgError("Invalid Series Name Selected");
                BRcboSeriesName.Focus();
                bValidate = false;
            }
            if (BRcomboBankAccount.SelectedItem == null)
            {
                Global.MsgError("Invalid Bank Account Selected");
                BRcomboBankAccount.Focus();
                bValidate = false;
            }

            if (BRcboProjectName.SelectedItem == null)
            {
                Global.MsgError("Invalid Project Name Selected");
                BRcboProjectName.Focus();
                bValidate = false;
            }
            if (!(grdBankReceipt.Rows.Count > 1))
            {
                Global.MsgError("Invalid Account Ledger Selected in grid");
                grdBankReceipt.Focus();
                bValidate = false;
            }
            return bValidate;
        }

        private void BRChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    BREnableControls(false);
                    BRButtonState(true, true, false, true, false);
                    IsFieldChanged = false;
                    break;
                case EntryMode.NEW:
                    BREnableControls(true);
                    BRButtonState(false, false, true, false, true);
                    BRLoadSeriesNo();
                    IsFieldChanged = false;
                    btnSetup.Enabled = BRchkRecurring.Checked;
                    break;
                case EntryMode.EDIT:
                    BREnableControls(true);
                    BRButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    btnSetup.Enabled = BRchkRecurring.Checked;
                    break;
            }
        }

        private void BRLoadSeriesNo()
        {
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("BANK_RCPT");
            BRcboSeriesName.Items.Clear();
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                BRcboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            BRcboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            BRcboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            BRcboSeriesName.SelectedIndex = 0;
        }

        private void BREnableControls(bool Enable)
        {
            BRchkRecurring.Enabled = btnSetup.Enabled = btnSetup.Enabled = BRchkRecurring.Checked; BRtxtVchNo.Enabled = BRtxtDate.Enabled = BRtxtRemarks.Enabled = grdBankReceipt.Enabled = Enable = BRcboSeriesName.Enabled = BRcomboBankAccount.Enabled = BRcboProjectName.Enabled = tabControl1.Enabled = Enable;
        }

        private void BRButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            BRbtnNew.Enabled = New;
            BRbtnEdit.Enabled = Edit;
            BRbtnSave.Enabled = Save;
            BRbtnDelete.Enabled = Delete;
            BRbtnCancel.Enabled = Cancel;
        }

        //Loads the Use  of the specific role id

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

        private bool BRNavigation(Navigate NavTo)
        {
            try
            {
                if (BRtxtBankID.Text != "")
                    dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(BRtxtBankID.Text), "BANK_RCPT");

                if (!IsShortcutKey)
                {
                    //CashReceipt m_CashReceipt = new CashReceipt();
                    BRChangeState(EntryMode.NORMAL);
                }
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(BRtxtBankID.Text);
                    if (BankReceiptIDCopy > 0)
                    {
                        VouchID = BankReceiptIDCopy;
                        BankReceiptIDCopy = 0;

                    }
                    else
                    {
                        VouchID = Convert.ToInt32(BRtxtBankID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                DataTable dtBankReceiptMaster = m_BankReceipt.NavigateBankReceiptMaster(VouchID, NavTo);
                if (dtBankReceiptMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }
                //Write the corresponding textboxes
                DataRow drBankReceiptMaster = dtBankReceiptMaster.Rows[0]; //There is only one row. First row is the required record
                if (IsShortcutKey)
                {
                    BRtxtRemarks.Text = drBankReceiptMaster["Remarks"].ToString();
                    IsShortcutKey = false;
                    BRtxtRemarks.SelectionStart = BRtxtRemarks.Text.Length + 1;
                    return false;
                }

                //Clear everything in the form
                BRClearVoucher();

                //Show the corresponding Bank Account Ledger in Combobox
                DataTable dtBankLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankReceiptMaster["LedgerID"]), LangMgr.DefaultLanguage);
                DataRow drBankLedgerInfo = dtBankLedgerInfo.Rows[0];

                BRcomboBankAccount.Text = drBankLedgerInfo["LedName"].ToString();

                //Show the Corresponding SeriesName in Combobox
                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drBankReceiptMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Bank Receipt");
                    BRcboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    BRcboSeriesName.Text = dr["EngName"].ToString();
                }
                lblVouNo.Visible = true;
                BRtxtVchNo.Visible = true;
                BRtxtVchNo.Text = drBankReceiptMaster["Voucher_No"].ToString();
                BRtxtDate.Text = Date.DBToSystem(drBankReceiptMaster["BankReceipt_Date"].ToString());
                BRtxtRemarks.Text = drBankReceiptMaster["Remarks"].ToString();
                DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drBankReceiptMaster["ProjectID"]), LangMgr.DefaultLanguage);
                if (dtProjectInfo.Rows.Count != 0)
                {
                    DataRow drProjectInfo = dtProjectInfo.Rows[0];
                    BRcboProjectName.Text = drProjectInfo["Name"].ToString();
                    BRtxtBankID.Text = drBankReceiptMaster["BankReceiptID"].ToString();
                    dsBankReceipt.Tables["tblBankReceiptMaster"].Rows.Add(BRcboSeriesName.Text, drBankReceiptMaster["Voucher_No"].ToString(), Date.DBToSystem(drBankReceiptMaster["BankReceipt_Date"].ToString()), BRcomboBankAccount.Text, drBankReceiptMaster["Remarks"].ToString(), drProjectInfo["Name"].ToString());

                }
                else
                {
                    BRcboProjectName.Text = "None";
                    BRtxtBankID.Text = drBankReceiptMaster["BankReceiptID"].ToString();
                    dsBankReceipt.Tables["tblBankReceiptMaster"].Rows.Add(BRcboSeriesName.Text, drBankReceiptMaster["Voucher_No"].ToString(), Date.DBToSystem(drBankReceiptMaster["BankReceipt_Date"].ToString()), BRcomboBankAccount.Text, drBankReceiptMaster["Remarks"].ToString(), "None");

                }
                //For Additional Fields
                if (NumberOfFields > 0)
                {
                    if (NumberOfFields == 1)
                    {
                        BRlblfirst.Visible = true;
                        BRtxtfirst.Visible = true;
                        BRlblsecond.Visible = false;
                        BRtxtsecond.Visible = false;
                        BRlblthird.Visible = false;
                        BRtxtthird.Visible = false;
                        BRlblfourth.Visible = false;
                        BRtxtfourth.Visible = false;
                        BRlblfifth.Visible = false;
                        BRtxtfifth.Visible = false;
                        BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();

                        BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                        BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                        BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                        BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                        BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 2)
                    {
                        BRlblfirst.Visible = true;
                        BRtxtfirst.Visible = true;
                        BRlblsecond.Visible = true;
                        BRtxtsecond.Visible = true;
                        BRlblthird.Visible = false;
                        BRtxtthird.Visible = false;
                        BRlblfourth.Visible = false;
                        BRtxtfourth.Visible = false;
                        BRlblfifth.Visible = false;
                        BRtxtfifth.Visible = false;
                        BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();

                        BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                        BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                        BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                        BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                        BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 3)
                    {
                        BRlblfirst.Visible = true;
                        BRtxtfirst.Visible = true;
                        BRlblsecond.Visible = true;
                        BRtxtsecond.Visible = true;
                        BRlblthird.Visible = true;
                        BRtxtthird.Visible = true;
                        BRlblfourth.Visible = false;
                        BRtxtfourth.Visible = false;
                        BRlblfifth.Visible = false;
                        BRtxtfifth.Visible = false;
                        BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        BRlblthird.Text = drdtadditionalfield["Field3"].ToString();

                        BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                        BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                        BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                        BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                        BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 4)
                    {
                        BRlblfirst.Visible = true;
                        BRtxtfirst.Visible = true;
                        BRlblsecond.Visible = true;
                        BRtxtsecond.Visible = true;
                        BRlblthird.Visible = true;
                        BRtxtthird.Visible = true;
                        BRlblfourth.Visible = true;
                        BRtxtfourth.Visible = true;
                        BRlblfifth.Visible = false;
                        BRtxtfifth.Visible = false;
                        BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        BRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                        BRlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                        BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                        BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                        BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                        BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                        BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 5)
                    {
                        BRlblfirst.Visible = true;
                        BRtxtfirst.Visible = true;
                        BRlblsecond.Visible = true;
                        BRtxtsecond.Visible = true;
                        BRlblthird.Visible = true;
                        BRtxtthird.Visible = true;
                        BRlblfourth.Visible = true;
                        BRtxtfourth.Visible = true;
                        BRlblfifth.Visible = true;
                        BRtxtfifth.Visible = true;

                        BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        BRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                        BRlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                        BRlblfifth.Text = drdtadditionalfield["Field5"].ToString();

                        BRtxtfirst.Text = drBankReceiptMaster["Field1"].ToString();
                        BRtxtsecond.Text = drBankReceiptMaster["Field2"].ToString();
                        BRtxtthird.Text = drBankReceiptMaster["Field3"].ToString();
                        BRtxtfourth.Text = drBankReceiptMaster["Field4"].ToString();
                        BRtxtfifth.Text = drBankReceiptMaster["Field5"].ToString();
                    }


                }
                DataTable dtBankDetail = m_BankReceipt.GetBankReceiptDetail(Convert.ToInt32(BRtxtBankID.Text));
                for (int i = 1; i <= dtBankDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtBankDetail.Rows[i - 1];
                    grdBankReceipt[i, 1].Value = i.ToString();
                    grdBankReceipt[i, 2].Value = drDetail["LedgerName"].ToString();
                    grdBankReceipt[i, 9].Value = drDetail["LedgerID"].ToString();
                    grdBankReceipt[i, 3].Value = (Convert.ToDecimal(drDetail["Amount"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdBankReceipt[i, 5].Value = drDetail["Remarks"].ToString();
                    grdBankReceipt[i, 6].Value = drDetail["ChequeNumber"].ToString();
                    grdBankReceipt[i, 7].Value = drDetail["ChequeBank"].ToString();
                    grdBankReceipt[i, 11].Value = drDetail["VoucherType"].ToString();
                    grdBankReceipt[i, 12].Value = drDetail["VoucherNumber"].ToString();
                    //grdBankReceipt[i, 4].Value = drDetail["Remarks"].ToString();
                    //grdBankReceipt[i, 5].Value = drDetail["ChequeNumber"].ToString();
                    //grdBankReceipt[i, 6].Value = drDetail["ChequeBank"].ToString();
                    //Code To Get The Current Balance of the Respective Ledger
                    DataTable dtLdrInfo = Ledger.GetLedgerDetail("1", null, null, Convert.ToInt32(drDetail["LedgerID"]));
                    if (dtLdrInfo.Rows.Count != 1)
                    {
                        grdBankReceipt[i, 4].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        grdBankReceipt[i, 10].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    else
                    {
                        DataRow drLdrInfo = dtLdrInfo.Rows[0];
                        if (drLdrInfo["Debit"] == DBNull.Value || Convert.ToInt32(drLdrInfo["Debit"]) == 0)
                        {
                            if (drLdrInfo["Credit"] == DBNull.Value || Convert.ToInt32(drLdrInfo["Credit"]) == 0)
                            {
                                grdBankReceipt[i, 4].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdBankReceipt[i, 10].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            }
                            else
                            {
                                grdBankReceipt[i, 4].Value = Convert.ToDecimal(drLdrInfo["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                grdBankReceipt[i, 10].Value = Convert.ToDecimal(drLdrInfo["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                            }
                        }
                        else
                        {
                            if (drLdrInfo["Credit"] == DBNull.Value || Convert.ToInt32(drLdrInfo["Credit"]) == 0)
                            {
                                grdBankReceipt[i, 4].Value = Convert.ToDecimal(drLdrInfo["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                grdBankReceipt[i, 10].Value = Convert.ToDecimal(drLdrInfo["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                            }
                            else
                            {
                                if (Convert.ToDecimal(drLdrInfo["Debit"]) == Convert.ToDecimal(drLdrInfo["Credit"]))
                                {
                                    grdBankReceipt[i, 4].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                    grdBankReceipt[i, 10].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                }

                                if (Convert.ToDecimal(drLdrInfo["Debit"]) > Convert.ToDecimal(drLdrInfo["Credit"]))
                                {
                                    grdBankReceipt[i, 4].Value = (Convert.ToDecimal(drLdrInfo["Debit"]) - Convert.ToDecimal(drLdrInfo["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                    grdBankReceipt[i, 10].Value = (Convert.ToDecimal(drLdrInfo["Debit"]) - Convert.ToDecimal(drLdrInfo["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                }
                                if (Convert.ToDecimal(drLdrInfo["Debit"]) < Convert.ToDecimal(drLdrInfo["Credit"]))
                                {
                                    grdBankReceipt[i, 4].Value = (Convert.ToDecimal(drLdrInfo["Credit"]) - Convert.ToDecimal(drLdrInfo["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                    grdBankReceipt[i, 10].Value = (Convert.ToDecimal(drLdrInfo["Credit"]) - Convert.ToDecimal(drLdrInfo["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                }
                            }
                        }
                    }

                    if (drDetail["ChequeDate"].ToString() == "")
                    {
                        grdBankReceipt[i, 8].Value = "";
                    }
                    else
                    {
                        grdBankReceipt[i, 8].Value = Date.DBToSystem(drDetail["ChequeDate"].ToString());
                    }
                    BRAddRowBankReceipt(grdBankReceipt.RowsCount);
                    dsBankReceipt.Tables["tblBankReceiptDetails"].Rows.Add(drDetail["LedgerName"].ToString(), (Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                    totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                    totalRptAmt = (Convert.ToDecimal(totalAmt)).ToString();
                }

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(BRtxtBankID.Text), "BANK_RCPT");
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
                    BRCheckRecurringSetting(BRtxtBankID.Text);
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
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            BRNavigation(Navigate.First);
            IsFieldChanged = false;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            BRNavigation(Navigate.Prev);
            IsFieldChanged = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            BRNavigation(Navigate.Next);
            IsFieldChanged = false;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            BRNavigation(Navigate.Last);
            IsFieldChanged = false;
        }

        private void BRClearVoucher()
        {
            m_isRecurring = false;
            m_RVID = 0; 
            BRcomboBankAccount.Text = string.Empty;
            BRClearBankReceipt();
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdBankReceipt.Redim(2, 13);
            BRAddGridHeader(); //Write header part
            BRAddRowBankReceipt(1);
            BRClearRecurringSetting();
            dtReference.Rows.Clear();
            AddReferenceColumns();
        }

        private void BRClearBankReceipt()
        {
            BRtxtBankID.Clear();
            BRtxtVchNo.Clear(); //actually generate a new voucher no.
            BRtxtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            BRtxtRemarks.Clear();
            grdBankReceipt.Rows.Clear();
            BRcboSeriesName.Text = string.Empty;
            BRcboProjectName.SelectedIndex = 0;
            foreach (ListItem lst in BRcomboBankAccount.Items)
            {
                if (lst.ID == Convert.ToInt32(Settings.GetSettings("DEFAULT_BANK_ACCOUNT")))
                {
                    BRcomboBankAccount.Text = lst.Value;
                    break;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BRcboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BROptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    BRtxtVchNo.Enabled = true;
                    SeriesID = (ListItem)BRcboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));

                    //if (txtVchNo.Text == "")
                    //{
                    //    txtVchNo.Text = "Main";
                    //    txtVchNo.Enabled = false;
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
                        BRtxtVchNo.Visible = true;
                        BRtxtVchNo.Text = m_vounum.ToString();
                        BRtxtVchNo.Enabled = false;
                    }
                    else if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        lblVouNo.Visible = false;
                        BRtxtVchNo.Visible = false;
                    }
                    if (m_BankReceiptID > 0 && !m_isRecurring)
                    {
                        lblVouNo.Visible = true;
                        BRtxtVchNo.Visible = true;
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
            if (BRCheckIfBankReconciliationClosed())
            {
                return;
            }
            isNew = false;
            OldGrid = " ";
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_RECEIPT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(BRtxtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            OldGrid = OldGrid + "Voucher No" + BRtxtVchNo.Text + "Series" + BRcboSeriesName.Text + "Project" + BRcboProjectName.Text + "Date" + BRtxtDate.Text + "Bank" + BRcomboBankAccount.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdBankReceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdBankReceipt[i + 1, 2].Value.ToString();
                string amt = grdBankReceipt[i + 1, 3].Value.ToString();
                OldGrid = OldGrid + string.Concat(particular, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            if (BRtxtBankID.Text.Length <= 0)
            {
                Global.MsgError("Please navigate to existing bank receipt first and then try again!");
                return;
            }
            BREnableControls(true);
            BRChangeState(EntryMode.EDIT);

            //if automatic voucher number increment is selected
            string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
            if (NumberingType == "AUTOMATIC")
                BRtxtVchNo.Enabled = false;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_RECEIPT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }

            BRClearVoucher();
            BRChangeState(EntryMode.NEW);
            IsFieldChanged = false;
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            BankReceiptIDCopy = Convert.ToInt32(BRtxtBankID.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (BankReceiptIDCopy > 0)
            {
                if (m_mode == EntryMode.NEW)
                {
                    BRNavigation(Navigate.ID);
                    BREnableControls(true);
                    BRChangeState(EntryMode.NEW);
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
            BRPrintPreviewCR(PrintType.CrystalReport);

        }

     
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (BRCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_RECEIPT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }

            NewGrid = NewGrid + "Voucher No" + BRtxtVchNo.Text + "Series" + BRcboSeriesName.Text + "Project" + BRcboProjectName.Text + "Date" + BRtxtDate.Text + "Bank" + BRcomboBankAccount.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdBankReceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdBankReceipt[i + 1, 2].Value.ToString();
                string amt = grdBankReceipt[i + 1, 3].Value.ToString();
                NewGrid = NewGrid + string.Concat(particular, amt);
            }
            NewGrid = "NewGridValues" + NewGrid;
            if (Freeze.IsDateFreeze(Date.ToDotNet(BRtxtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            try
            {

                if (!String.IsNullOrEmpty(BRtxtBankID.Text))
                {
                    if (Global.MsgQuest("Are you sure you want to delete the Bank Receipt - " + BRtxtBankID.Text + "?") == DialogResult.Yes)
                    {
                        BankReceipt m_BankReceipt = new BankReceipt();

                        // delete reference
                        string res = VoucherReference.DeleteReference(Convert.ToInt32(BRtxtBankID.Text), "BANK_RCPT");
                        if (res != "Success")
                        {
                            Global.MsgError("Unable to delete the voucher due to " + res);
                            return;
                        }

                        if (m_BankReceipt.RemoveBankReceiptEntry(Convert.ToInt32(BRtxtBankID.Text)))
                        {


                            AuditLogDetail auditlog = new AuditLogDetail();
                            auditlog.ComputerName = Global.ComputerName;
                            auditlog.UserName = User.CurrentUserName;
                            auditlog.Voucher_Type = "BANK_RCPT";
                            auditlog.Action = "DELETE";
                            auditlog.Description = NewGrid;
                            auditlog.RowID = Convert.ToInt32(BRtxtBankID.Text);
                            auditlog.MAC_Address = Global.MacAddess;
                            auditlog.IP_Address = Global.IpAddress;
                            auditlog.VoucherDate = Date.ToDB(DateTime.Now).ToString();

                            auditlog.CreateAuditLog(auditlog);

                            RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "BANK_RECEIPT"); // deleting the recurring setting if voucher is deleted

                            Global.Msg("Bank Receipt -" + BRtxtBankID.Text + " deleted successfully!");
                            //Navigate to 1 step previous
                            if (!this.BRNavigation(Navigate.Prev))
                            {
                                //This must be because there are no records or this was the first one

                                //If this was the first, try to navigate to second
                                if (!this.BRNavigation(Navigate.Next))
                                {
                                    //This was the last one, there are no records left. Simply clear the form and stay calm
                                    btnNew_Click(sender, e);
                                }
                            }
                        }
                        else
                            Global.MsgError("There was an error while deleting Bank Receipt -" + BRtxtBankID.Text + "!");
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void BRcboSeriesName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BRtxtDate.Focus();
            }
        }

        private void BRtxtDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BRcomboBankAccount.Focus();
            }
        }

        private void BRcomboBankAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BRcboProjectName.Focus();
            }
        }

        private void BRcboProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                grdBankReceipt.Focus();
            }
        }

        private void BRtxtRemarks_KeyDown(object sender, KeyEventArgs e)
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
                BRNavigation(Navigate.Last);
            }
            if (e.KeyValue == 13)
            {
                BRchkDoNotClose.Focus();
            }
        }

        private void BRchkDoNotClose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BRchkPrntWhileSaving.Focus();
            }
        }

        private void BRchkPrntWhileSaving_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BRbtnSave.Focus();
            }
        }

        private void frmBankReceipt_KeyDown(object sender, KeyEventArgs e)
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
                BRbtnSave_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete && e.Control)
            {
                btnDelete_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F && e.Control)
            {
                btnFirst_Click(sender, e);
            }
            //else if (e.KeyCode == Keys.P && e.Shift)
            //{
            //    btnPrev_Click(sender, e);
            //}
            //else if (e.KeyCode == Keys.N && e.Shift)
            //{
            //    btnNext_Click(sender, e);
            //}
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            BRChangeState(EntryMode.NORMAL);
        }

        private void BRbtnDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = false;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(BRtxtDate.Text));
            frm.ShowDialog();
        }

        private void frmBankReceipt_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ContinueWithoutSaving())
            {
                e.Cancel = true;
            }
        }

        private void BRbtn_groupvoucherposting_Click(object sender, EventArgs e)
        {
            BankReceipt breceipt = new BankReceipt();
            int rowid = breceipt.GetRowID(BRtxtVchNo.Text);
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
            string ledid = grdBankReceipt[1, 9].Value.ToString();
            for (int i = 0; i < grdBankReceipt.Rows.Count - 1; i++)
            {
                dtbulkvoucher.Rows.Add(grdBankReceipt[i + 1, 2].Value, "Credit", grdBankReceipt[i + 1, 3].Value, grdBankReceipt[i + 1, 9].Value, "BANK_RCPT", BRtxtVchNo.Text, grdBankReceipt[i + 1, 5].Value);
            }
            frmGroupVoucherList fgl = new frmGroupVoucherList(dtbulkvoucher, SID, ProjectID, rowid);
            fgl.ShowDialog();
        }

        private int GetRootAccClassID()
        {
            if (Global.GlobalAccClassID > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(Global.GlobalAccClassID));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }
            return 1;//The default root class ID
        }
        private void GetOpeningBalanceSummary(int AccClassID, ref double TotalDrOpBal, ref double TotalCrOpBal)
        {
            DataTable dtAllLedgersInfo = OpeningBalance.GetAccClassOpeningBalance(AccClassID, -1); //Collect all ledger information first
            //double TotalDrOpBal, TotalCrOpBal;
            TotalDrOpBal = TotalCrOpBal = 0;
            foreach (DataRow drAllLedgersInfo in dtAllLedgersInfo.Rows)
            {
                if (!drAllLedgersInfo.IsNull("OpenBal"))
                {
                    if (drAllLedgersInfo["OpenBalDrCr"].ToString() == "DEBIT")
                        TotalDrOpBal += (Convert.ToDouble(drAllLedgersInfo["OpenBal"]));
                    else if (drAllLedgersInfo["OpenBalDrCr"].ToString() == "CREDIT")
                        TotalCrOpBal += (Convert.ToDouble(drAllLedgersInfo["OpenBal"]));
                }
            }
            //TotalDrOpBal = 5999;
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
        private void BROptionalFields()
        {
            SeriesID = (ListItem)BRcboSeriesName.SelectedItem;
            DataTable dtadditionalfield = Sales.GetAdditionalFields(SeriesID.ID);
            drdtadditionalfield = dtadditionalfield.Rows[0];
            NumberOfFields = Convert.ToInt32(drdtadditionalfield["NumberOfField"].ToString());
            if (NumberOfFields > 0)
            {
                if (NumberOfFields == 1)
                {
                    BRlblfirst.Visible = true;
                    BRtxtfirst.Visible = true;
                    BRlblsecond.Visible = false;
                    BRtxtsecond.Visible = false;
                    BRlblthird.Visible = false;
                    BRtxtthird.Visible = false;
                    BRlblfourth.Visible = false;
                    BRtxtfourth.Visible = false;
                    BRlblfifth.Visible = false;
                    BRtxtfifth.Visible = false;
                    BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                }
                else if (NumberOfFields == 2)
                {
                    BRlblfirst.Visible = true;
                    BRtxtfirst.Visible = true;
                    BRlblsecond.Visible = true;
                    BRtxtsecond.Visible = true;
                    BRlblthird.Visible = false;
                    BRtxtthird.Visible = false;
                    BRlblfourth.Visible = false;
                    BRtxtfourth.Visible = false;
                    BRlblfifth.Visible = false;
                    BRtxtfifth.Visible = false;
                    BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                }
                else if (NumberOfFields == 3)
                {
                    BRlblfirst.Visible = true;
                    BRtxtfirst.Visible = true;
                    BRlblsecond.Visible = true;
                    BRtxtsecond.Visible = true;
                    BRlblthird.Visible = true;
                    BRtxtthird.Visible = true;
                    BRlblfourth.Visible = false;
                    BRtxtfourth.Visible = false;
                    BRlblfifth.Visible = false;
                    BRtxtfifth.Visible = false;
                    BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    BRlblthird.Text = drdtadditionalfield["Field3"].ToString();

                }
                else if (NumberOfFields == 4)
                {
                    BRlblfirst.Visible = true;
                    BRtxtfirst.Visible = true;
                    BRlblsecond.Visible = true;
                    BRtxtsecond.Visible = true;
                    BRlblthird.Visible = true;
                    BRtxtthird.Visible = true;
                    BRlblfourth.Visible = true;
                    BRtxtfourth.Visible = true;
                    BRlblfifth.Visible = false;
                    BRtxtfifth.Visible = false;
                    BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    BRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                    BRlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                }
                else if (NumberOfFields == 5)
                {
                    BRlblfirst.Visible = true;
                    BRtxtfirst.Visible = true;
                    BRlblsecond.Visible = true;
                    BRtxtsecond.Visible = true;
                    BRlblthird.Visible = true;
                    BRtxtthird.Visible = true;
                    BRlblfourth.Visible = true;
                    BRtxtfourth.Visible = true;
                    BRlblfifth.Visible = true;
                    BRtxtfifth.Visible = true;

                    BRlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    BRlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    BRlblthird.Text = drdtadditionalfield["Field3"].ToString();
                    BRlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                    BRlblfifth.Text = drdtadditionalfield["Field5"].ToString();
                }
            }
            else
            {
                BRlblfirst.Visible = false;
                BRtxtfirst.Visible = false;
                BRlblsecond.Visible = false;
                BRtxtsecond.Visible = false;
                BRlblthird.Visible = false;
                BRtxtthird.Visible = false;
                BRlblfourth.Visible = false;
                BRtxtfourth.Visible = false;
                BRlblfifth.Visible = false;
                BRtxtfifth.Visible = false;
            }

        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            BRPrintPreviewCR(PrintType.CrystalReport);
        }
        private void BRPrintPreviewCR(PrintType myPrintType)
        {
            dsBankReceipt.Clear();
            totalAmt = 0;
            rptBankReceipt rpt = new rptBankReceipt();
            //Fill the logo on the report
            Misc.WriteLogo(dsBankReceipt, "tblImage");
            rpt.SetDataSource(dsBankReceipt);

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



            bool empty = BRNavigation(Navigate.ID);
            if (empty == false)
            {
                return;
            }
            else
            {

                //  Navigation(Navigate.ID);
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

        DataTable m_dtRecurringSetting = new DataTable();
        public void GetRecurringSetting(DataTable dt)
        {
            if (dt == null)
                BRchkRecurring.Checked = false; // if cancel button is clicked then the chkrecurring is unchecked

            else
                this.m_dtRecurringSetting = dt;
        }
        private void BRchkRecurring_CheckedChanged(object sender, EventArgs e)
        {
            if ((BRchkRecurring.Checked && m_dtRecurringSetting.Rows.Count == 0))
            {
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "BANK_RECEIPT", m_dtRecurringSetting);
                fvr.ShowDialog();
                if (m_dtRecurringSetting.Rows.Count == 0)  // if settings are not available then uncheck the checkbox
                    BRchkRecurring.Checked = false;
            }
            else if (BRchkRecurring.Checked == false && m_dtRecurringSetting.Rows.Count > 0) //if previously saved settings are available
            {
                //if (txtSalesInvoiceID.Text != "" || txtSalesInvoiceID != null)
                //{
                if (Global.MsgQuest("Are you sure you want to delete the saved recurring voucher settings?") == DialogResult.Yes)
                {
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "BANK_RECEIPT"); // delete from database
                    BRClearRecurringSetting();  // clear local variables
                }
                else
                    BRchkRecurring.Checked = true;

                //}
                //else
                //    ClearRecurringSetting();
            }
            if ((BRchkRecurring.Checked == true && m_mode == EntryMode.EDIT) || (BRchkRecurring.Checked == true && m_mode == EntryMode.NEW))
                btnSetup.Enabled = true;
            else
                btnSetup.Enabled = false;
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "BANK_RECEIPT", m_dtRecurringSetting);
            fvr.ShowDialog();
        }

        string RSID = null, recurringVoucherID = null;
        public void BRCheckRecurringSetting(string VoucherID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            m_dtRecurringSetting = RecurringVoucher.GetRecurringVoucherSetting(VoucherID, "BANK_RECEIPT");
            if (m_dtRecurringSetting.Rows.Count > 0)
            {
                BRchkRecurring.Checked = true;
                RSID = m_dtRecurringSetting.Rows[0]["RVID"].ToString();
                recurringVoucherID = m_dtRecurringSetting.Rows[0]["VoucherID"].ToString();
            }
            else
            {
                BRchkRecurring.Checked = false;
            }
        }
        private void BRClearRecurringSetting()
        {
            m_dtRecurringSetting.Rows.Clear();
            BRchkRecurring.Checked = false;
            recurringVoucherID = "";
            RSID = "";
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!UserPermission.ChkUserPermission("BANK_RECEIPT_VIEW"))
            //    {
            //        Global.MsgError("Sorry! you dont have permission to View Bank Receipt. Please contact your administrator for permission.");
            //        return;
            //    }
            //    string VoucherType = "BANK_RECEIPT";

            //    frmVoucherList fvl = new frmVoucherList(this, VoucherType);
            //    fvl.ShowDialog();

            //}
            //catch (Exception ex)
            //{
            //    Global.MsgError(ex.Message);
            //}

            try
            {
                if (!UserPermission.ChkUserPermission("BANK_RECEIPT_VIEW"))
                {
                    Global.MsgError("Sorry! you dont have permission to View Bank Receipt. Please contact your administrator for permission.");
                    return;
                }
                string[] vouchValues = new string[5];
                vouchValues[0] = "BANK_RECEIPT";               // voucherType
                vouchValues[1] = "Acc.tblBankReceiptMaster";   // master tableName for the given voucher type  
                vouchValues[2] = "Acc.tblBankReceiptDetails";  // details tableName for the given voucher type
                vouchValues[3] = "BankReceiptID";              // master ID for the given master table
                vouchValues[4] = "BankReceipt_Date";              // date field for a given voucher

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
            m_BankReceiptID = VoucherID;
            BRtxtBankID.Text = VoucherID.ToString();
            BRNavigation(Navigate.ID);
            //frmBankReceipt_Load(null, null);
        }
        public bool  BRCheckIfBankReconciliationClosed()
        {
            try
            {
                bool res = false;
                ListItem bankId = (ListItem)BRcomboBankAccount.SelectedItem;
                if (BankReconciliation.IsBankReconciliationClosed(Convert.ToInt32(bankId.ID), Date.ToDotNet(BRtxtDate.Text)))
                {
                    Global.MsgError("Bank Reconciliation is closed for this Bank, So you cannot add, edit or delete the vocher !");
                    return true;
                }
                for (int i = 1; i < grdBankReceipt.Rows.Count; i++)
                {
                    int bankID = Convert.ToInt32(grdBankReceipt[i, (int)GridColumnBR.BRLedger_ID].Value);

                    res = BankReconciliation.IsBankReconciliationClosed(bankID, Date.ToDotNet(BRtxtDate.Text));
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
                return false;
            }
        }

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
                //    grdBankReceipt[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Credit";

                //else
                //    grdBankReceipt[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Debit";

                grdBankReceipt[CurrRowPos, (int)GridColumnBR.BRAmount].Value = amt.ToString();
                //grdBankReceipt[CurrRowPos, (int)GridColumn.Ref_Amt].Value = amt.ToString() + crDr;

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

        public bool BRCheckAmtAgainstRefAmt()
        {
            try
            {
                bool res = true;
                decimal Amt = Convert.ToDecimal(grdBankReceipt[CurrRowPos, (int)GridColumnBR.BRAmount].Value);
                //string crdr = grdBankReceipt[CurrRowPos, (int)GridColumn.Dr_Cr].Value.ToString();
                string crdr ="Credit";

                string amtCrDr = grdBankReceipt[CurrRowPos, (int)GridColumnBR.BRRef_Amt].Value.ToString();
                decimal refAmt = Convert.ToDecimal(amtCrDr.Substring(0, amtCrDr.Length - 4));
                string ledgerName = grdBankReceipt[CurrRowPos, (int)GridColumnBR.BRParticular_Account_Head].Value.ToString();
                if ((refAmt > 0) && (refAmt < Amt) && (amtCrDr.Contains(crdr.Substring(0, crdr.Length - 4))))
                {
                    Global.MsgError("Your transaction amount for ledger " + ledgerName + " is " + Amt + " \n which is greater than the reference amount i.e. " + refAmt + " !");
                    grdBankReceipt[CurrRowPos, (int)GridColumnBR.BRAmount].Value = refAmt.ToString();
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

        private void comboBankAccount_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void grdBankReceipt_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}

