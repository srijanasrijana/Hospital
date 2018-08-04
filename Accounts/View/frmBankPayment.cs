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
using System.Data.SqlClient;
using ErrorManager;
using System.IO;
using CrystalDecisions.Shared;
using Common;
using Accounts.Reports;
using BusinessLogic.Accounting;

namespace Accounts
{
    public partial class frmBankPayment : Form, IfrmAddAccClass, IfrmDateConverter, ILOVLedger, IDueDate, IVoucherRecurring, IVoucherList, IVoucherReference
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;
        DataTable dt1 = new DataTable();
        DataTable dtDueDateInfo = new DataTable();
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private string Prefix = "";
        private int DefaultAccClass;
        ListItem liProjectID = new ListItem();
        ListItem liBankID = new ListItem();
        private bool hasChanged = false;
        private bool IsFieldChanged = false;
        private int OnlyReqdDetailRows = 0;
        private int currRowPosition = 0;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        SourceGrid.CellContext ctx1 = new SourceGrid.CellContext();
        private bool IsChequeDateButton = false;
        private bool IsNegativeCash = false;
        private bool IsNegativeBank = false;
        private int loopCounter = 0;
        private int BankPaymentIDCopy = 0;
        ListItem SeriesID = new ListItem();
        private bool IsShortcutKey = false;
        List<int> AccClassID = new List<int>();
        private Accounts.Model.dsBankPayment dsBankPayment = new Accounts.Model.dsBankPayment();
        decimal totalAmt = 0;
        string totalRptAmt = "";
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        DataTable dtAccClassID = new DataTable();
        ArrayList AccountClassID = new ArrayList();
        private int CurrAccLedgerID = 0;
        private string CurrBal = "";
        private int CurrRowPos = 0;
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        private enum GridColumn1 : int
        {
            BPDel = 0, BPCode_No, BPParticular_Account_Head, BPAmount, BPCurrent_Balance, BPRemarks, BPCheque_No, BPChequeDate, BPLedger_ID, BPCurrent_Bal_Actual, BPRef_Amt
        };

        SourceGrid.Cells.Button BPbtnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents BPevtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BPevtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BPevtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BPevtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents BPevtChequeDateFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();

        BankPayment m_BankPayment = new BankPayment();
        private int m_BankPaymentID;

        DataTable dtGetOpeningBalance = new DataTable();
        //For Export Menu
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
        public frmBankPayment()
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
        public frmBankPayment(int BankPaymentID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_BankPaymentID = BankPaymentID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }
        public frmBankPayment(int BankPaymentID)
        {
            InitializeComponent();
            this.m_BankPaymentID = BankPaymentID;
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
                BPtxtDate.Text = Date.ToSystem(DotNetDate);
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

        //Customized header
        private class MyHeader1 : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader1(object value)
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

        /// <summary>
        /// This is the common text change event for all controls just to track if any changes has been made. This will help to show a save dialogue box while closing the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BPText_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
        }

        private void frmBankPayment_Load(object sender, EventArgs e)
        {
            BPAddReferenceColumns();
            if (BPtxtBankID.Text != "")
                BPdtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(BPtxtBankID.Text), "CONTRA");
            BPchkDoNotClose.Checked = true;
           BPChangeState(EntryMode.NEW);
            // ListProject(cboProjectName);
            LoadComboboxProject(BPcboProjectName, 0);
            ShowAccClassInTreeView(treeAccClass, null, 0);
            m_mode = EntryMode.NEW;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            BPtxtDate.Mask = Date.FormatToMask();
            BPtxtDate.Text = Date.ToSystem(Date.GetServerDate()); //By default show the current date from the sqlserver.
            //For Loading The Optional Fields
        //    BPOptionalFields();
            try
            {

                #region Load cboBankAccount According to User Setting
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                // int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());



                BPLoadComboboxBank();
                BPcmboBankAccount_IndexChanged(null, null);
              

                #endregion

                #region Customevents mainly for saving purpose
                BPcboSeriesName.SelectedIndexChanged += new EventHandler(BPText_Change);
                BPtxtVchNo.TextChanged += new EventHandler(BPText_Change);
                BPtxtDate.TextChanged += new EventHandler(BPText_Change);
                BPbtnDate.Click += new EventHandler(BPText_Change);
                BPcmboBankAccount.SelectedIndexChanged += new EventHandler(BPText_Change);
                BPcmboBankAccount.SelectedIndexChanged += new EventHandler(BPcmboBankAccount_IndexChanged);
                BPcboProjectName.SelectedIndexChanged += new EventHandler(BPText_Change);
                BPtxtRemarks.TextChanged += new EventHandler(BPText_Change);

                //Event trigerred when delete button is clicked
                BPevtDelete.Click += new EventHandler(BPDelete_Row_Click);
                BPevtAccountFocusLost.FocusLeft += new EventHandler(BPevtAccountFocusLost_FocusLeft);
                BPevtAmountFocusLost.FocusLeft += new EventHandler(BPAmount_Focus_Lost);
                BPevtChequeDateFocusLost.Click += new EventHandler(BPChequeDate_Click);
                BPevtChequeDateFocusLost.Click += new EventHandler(BPText_Change);
                #endregion

          
                grdBankPayment.Redim(2, 11);
                BPbtnRowDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                BPAddGridHeader();
                BPAddRowBankPayment(1);
                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID


                if (m_BankPaymentID > 0)
                {
                    //Show the values in fields

                    try
                    {

                        if (m_isRecurring)
                        {
                            BPChangeState(EntryMode.NEW);
                        }
                        else
                            BPChangeState(EntryMode.NORMAL);

                        int vouchID = 0;
                        try
                        {
                            vouchID = m_BankPaymentID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }


                        BankPayment m_BankPayment = new BankPayment();


                        //Getting the value of SeriesID via MasterID or VouchID

                        int SeriesIDD = m_BankPayment.GetSeriesIDFromMasterID(vouchID);


                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Bank Payment");
                            BPcboSeriesName.Text = "";

                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            BPcboSeriesName.Text = dr["EngName"].ToString();

                        }

                        DataTable dtBankPaymentMaster = m_BankPayment.GetBankPaymentMaster(vouchID);


                        if (dtBankPaymentMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }


                        DataRow drBankPaymentMaster = dtBankPaymentMaster.Rows[0];
                        if (!m_isRecurring)
                        {
                            BPtxtVchNo.Text = drBankPaymentMaster["Voucher_No"].ToString();
                            BPtxtDate.Text = Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString());
                        }
                        else
                        {
                            BPtxtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if recurring load today's date
                            //txtduedate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                        }
                        BPtxtRemarks.Text = drBankPaymentMaster["Remarks"].ToString();
                        BPtxtBankID.Text = drBankPaymentMaster["BankPaymentID"].ToString();
                        dsBankPayment.Tables["tblBankPaymentMaster"].Rows.Add(BPcboSeriesName.Text, drBankPaymentMaster["Voucher_No"].ToString(), Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString()), BPcmboBankAccount.Text, drBankPaymentMaster["Remarks"].ToString());
                        DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drBankPaymentMaster["ProjectID"]), LangMgr.DefaultLanguage);
                        if (dtProjectInfo.Rows.Count > 0)
                        {
                            DataRow drProjectInfo = dtProjectInfo.Rows[0];
                            BPcboProjectName.Text = drProjectInfo["Name"].ToString();
                        }
                        //Show the corresponding Bank Account Ledger in Combobox
                        DataTable dtBankLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankPaymentMaster["LedgerID"]), LangMgr.DefaultLanguage);
                        DataRow drBankLedgerInfo = dtBankLedgerInfo.Rows[0];

                        BPcmboBankAccount.Text = drBankLedgerInfo["LedName"].ToString();
                        //For Additional Fields
                        if (NumberOfFields > 0)
                        {
                            if (NumberOfFields == 1)
                            {
                                BPlblfirst.Visible = true;
                                BPtxtfirst.Visible = true;
                                BPlblsecond.Visible = false;
                                BPtxtsecond.Visible = false;
                                BPlblthird.Visible = false;
                                BPtxtthird.Visible = false;
                                BPlblfourth.Visible = false;
                                BPtxtfourth.Visible = false;
                                BPlblfifth.Visible = false;
                                BPtxtfifth.Visible = false;
                                BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();

                                BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();
                            }
                            else if (NumberOfFields == 2)
                            {
                                BPlblfirst.Visible = true;
                                BPtxtfirst.Visible = true;
                                BPlblsecond.Visible = true;
                                BPtxtsecond.Visible = true;
                                BPlblthird.Visible = false;
                                BPtxtthird.Visible = false;
                                BPlblfourth.Visible = false;
                                BPtxtfourth.Visible = false;
                                BPlblfifth.Visible = false;
                                BPtxtfifth.Visible = false;
                                BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();

                                BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();
                            }
                            else if (NumberOfFields == 3)
                            {
                                BPlblfirst.Visible = true;
                                BPtxtfirst.Visible = true;
                                BPlblsecond.Visible = true;
                                BPtxtsecond.Visible = true;
                                BPlblthird.Visible = true;
                                BPtxtthird.Visible = true;
                                BPlblfourth.Visible = false;
                                BPtxtfourth.Visible = false;
                                BPlblfifth.Visible = false;
                                BPtxtfifth.Visible = false;
                                BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                BPlblthird.Text = drdtadditionalfield["Field3"].ToString();

                                BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();

                            }
                            else if (NumberOfFields == 4)
                            {
                                BPlblfirst.Visible = true;
                                BPtxtfirst.Visible = true;
                                BPlblsecond.Visible = true;
                                BPtxtsecond.Visible = true;
                                BPlblthird.Visible = true;
                                BPtxtthird.Visible = true;
                                BPlblfourth.Visible = true;
                                BPtxtfourth.Visible = true;
                                BPlblfifth.Visible = false;
                                BPtxtfifth.Visible = false;
                                BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                BPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                                BPlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                                BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();

                            }
                            else if (NumberOfFields == 5)
                            {
                                BPlblfirst.Visible = true;
                                BPtxtfirst.Visible = true;
                                BPlblsecond.Visible = true;
                                BPtxtsecond.Visible = true;
                                BPlblthird.Visible = true;
                                BPtxtthird.Visible = true;
                                BPlblfourth.Visible = true;
                                BPtxtfourth.Visible = true;
                                BPlblfifth.Visible = true;
                                BPtxtfifth.Visible = true;

                                BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                                BPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                                BPlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                                BPlblfifth.Text = drdtadditionalfield["Field5"].ToString();

                                BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();
                            }


                        }

                        DataTable dtBankPaymentDetail = m_BankPayment.GetBankPaymentDetail(vouchID);


                        for (int i = 1; i <= dtBankPaymentDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtBankPaymentDetail.Rows[i - 1];

                            grdBankPayment[i, (int)GridColumn1.BPCode_No].Value = i.ToString();
                            grdBankPayment[i, (int)GridColumn1.BPParticular_Account_Head].Value = drDetail["LedgerName"].ToString();
                            grdBankPayment[i, (int)GridColumn1.BPAmount].Value = Convert.ToDecimal(drDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces));
                            grdBankPayment[i, (int)GridColumn1.BPRemarks].Value = drDetail["Remarks"].ToString();
                            grdBankPayment[i, (int)GridColumn1.BPCheque_No].Value = drDetail["ChequeNumber"].ToString();
                            grdBankPayment[i, (int)GridColumn1.BPLedger_ID].Value = drDetail["LedgerID"].ToString();
                            if (drDetail["ChequeDate"].ToString() == "")
                            {
                                grdBankPayment[i, (int)GridColumn1.BPChequeDate].Value = "";
                            }
                            else
                            {
                                grdBankPayment[i, (int)GridColumn1.BPChequeDate].Value = Date.DBToSystem(drDetail["ChequeDate"].ToString());
                            }

                        //    AddRowBankPayment(grdBankPayment.RowsCount);
                        //    dsBankPayment.Tables["tblBankPaymentDetails"].Rows.Add(drDetail["LedgerName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                        //}
                            //Code To Get The Current Balance of the Respective Ledger
                            string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
                            string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
                            DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(drDetail["LedgerID"]), null);
                            if (dtLdrInfo.Rows.Count != 1)
                            {
                                grdBankPayment[i, (int)GridColumn1.BPCurrent_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdBankPayment[i, (int)GridColumn1.BPCurrent_Bal_Actual].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
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
                                strBalance = ((Balance < 0) ? Balance * -1 : Balance).ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces));
                                if (Balance >= 0)
                                    strBalance = strBalance + " (Dr.)";

                                else //If balance is -ve, its Cr.
                                    strBalance = strBalance + " (Cr.)";


                                //Write balance into the grid
                                grdBankPayment[i, (int)GridColumn1.BPCurrent_Balance].Value = strBalance;
                                grdBankPayment[i, (int)GridColumn1.BPCurrent_Bal_Actual].Value = Balance.ToString();


                            }
                            BPAddRowBankPayment(grdBankPayment.RowsCount);
                            dsBankPayment.Tables["tblBankPaymentDetails"].Rows.Add(drDetail["LedgerName"].ToString(), (Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                            totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                            totalRptAmt = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();

                        }

                        IsFieldChanged = false;
                        DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(BPtxtBankID.Text), "BANK_PMNT");
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
                    CheckRecurringSetting(BPtxtBankID.Text);
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }
        public void BPLoadComboboxBank()
        {
            int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
            DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);

            BPcmboBankAccount.DataSource = dtBankLedgers;
            BPcmboBankAccount.DisplayMember = "EngName";
            BPcmboBankAccount.ValueMember = "LedgerID";
            BPcmboBankAccount.SelectedIndex = 0;
        }
        private void BPDelete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = grdBankPayment.Selection.GetSelectionRegion().GetRowsIndex()[0];

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdBankPayment.RowsCount - 2)
            {
                grdBankPayment.Rows.Remove(ctx.Position.Row);

                #region Reference related
                if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(BPtxtBankID.Text), Convert.ToInt32(grdBankPayment[CurRow, (int)GridColumn1.BPLedger_ID].Value), "BANK_PMT"))
                {
                    Global.MsgError("You must delete all other vouchers with reference against this voucher to delete this transaction!");
                    return;
                }

                #endregion
            }
        }

        private void BPAccount_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
        }

        bool isNewReferenceVoucher = false, isAgainstRef = false;

        private void BPAccount_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;

                int CurRow = grdBankPayment.Selection.GetSelectionRegion().GetRowsIndex()[0];

                #region Reference related
                // if new voucher reference is created for this, donot open the reference form 
                if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(BPtxtBankID.Text), Convert.ToInt32(grdBankPayment[CurRow, (int)GridColumn1.BPLedger_ID].Value), "BANK_PMT"))
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
        bool BPamt_Focused_First = true;
        /// <summary>
        /// used to store current value of Amount before edting it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BPAmount_Focused(object sender, EventArgs e)
        {
            try
            {
                if (BPamt_Focused_First)
                {
                    int RowCount = grdBankPayment.RowsCount;
                    int CurRow = grdBankPayment.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    amt = Convert.ToDecimal(grdBankPayment[CurRow, (int)GridColumn1.BPAmount].Value);
                    BPamt_Focused_First = false;
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
        private void BPevtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            int ledID = 0, vouchID = 0;

            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            int ledgerID = 0;
            string CurrentBal = "";
            try
            {
                if (grdBankPayment[CurrRowPos, (int)GridColumn1.BPParticular_Account_Head].Value.ToString() == "(NEW)" || grdBankPayment[CurrRowPos, (int)GridColumn1.BPParticular_Account_Head].Value.ToString() == "")
                    return;
                try
                {
                    ledgerID = Convert.ToInt32(grdBankPayment[CurrRowPos, (int)GridColumn1.BPLedger_ID].Value);
                    CurrentBal = grdBankPayment[CurrRowPos, (int)GridColumn1.BPCurrent_Bal_Actual].Value.ToString();
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

                if (BPtxtBankID.Text != "")
                {
                    vouchID = Convert.ToInt32(BPtxtBankID.Text);
                }

                frmReference fr = new frmReference(this, vouchID, "CONTRA", ledID);
                fr.ShowDialog();
                //SendKeys.Send("{Tab}");
            }
            #endregion

            BPFillAllGridRow(CurrRowPos, CurrAccLedgerID, CurrBal);
        }

        private void BPFillAllGridRow(int RowPosition, int LdrID, string CurrBalance)
        {
            decimal TempAmount = 0;
            string CurrentLedgerBalance = CurrBalance.ToString();
            string[] CurrentBalance = CurrBalance.ToString().Split('(');
            try
            {
                TempAmount = Convert.ToDecimal(grdBankPayment[Convert.ToInt32(RowPosition), 3].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPCurrent_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }
            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPCurrent_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPCurrent_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else
            {
                grdBankPayment[RowPosition, (int)GridColumn1.BPCurrent_Balance].Value = CurrBalance;
            }
            grdBankPayment[RowPosition, (int)GridColumn1.BPLedger_ID].Value = LdrID;
            grdBankPayment[RowPosition, (int)GridColumn1.BPCurrent_Bal_Actual].Value = CurrBalance;
            CurrAccLedgerID = 0;
            CurrBal = "";
        }

        private void BPFillGridRowExceptLedgerID(int RowPosition, int LdrID, string CurrBal, decimal amt)
        {
            BPamt_Focused_First = true;
            decimal TempAmount = 0;
            string CurrentLedgerBalance = "";
            decimal temporary = 0;
            string[] CurrentBalance = new string[2];

            if (CurrBal == "" || CurrBal == null)
            {
                CurrentLedgerBalance = grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPCurrent_Bal_Actual].ToString();
                CurrentBalance = grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPCurrent_Bal_Actual].ToString().Split('(');
            }
            else
            {
                CurrentLedgerBalance = CurrBal.ToString();
                CurrentBalance = CurrBal.ToString().Split('(');
            }
            try
            {
                TempAmount = Convert.ToDecimal(grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPAmount].Value);
            }
            catch
            {
                TempAmount = 0;
            }
            
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
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPCurrent_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }
            else if (temporary > 0)
            {
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPCurrent_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else if (temporary == 0)
            {
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPCurrent_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {

                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn1.BPCurrent_Balance].Value = (Convert.ToDecimal(TempAmount) + amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";

            }
            else
            {
                grdBankPayment[RowPosition, (int)GridColumn1.BPCurrent_Balance].Value = CurrBal;
            }
            
        }

        //it display current balance at the side of the bank account combobox
        private void BPFilllblCurrentBankBalance()
        {
            decimal TotalInputAmount = 0;
            decimal temporary = 0;
            string CurrentLedgerBalanceForBank = "";
            string[] CurrentBalanceForBank = new string[2];
            try
            {
                CurrentLedgerBalanceForBank = BPlblBankCurrBalHidden.Text;
                CurrentBalanceForBank = CurrentLedgerBalanceForBank.Split('(');
                //Get all the balance of the of the grid entry
                for (int i = 0; i < grdBankPayment.Rows.Count - 1; i++)
                {
                    TotalInputAmount += Convert.ToDecimal(grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value);
                    BPlblTotalAmount.Text = TotalInputAmount.ToString();
                }
            }
            catch
            { }

            if (CurrentLedgerBalanceForBank.Contains("Cr"))
            {
                BPlblBankCurrentBalance.Text = (Convert.ToDecimal(CurrentBalanceForBank[0]) + Convert.ToDecimal(TotalInputAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else if (CurrentLedgerBalanceForBank.Contains("Dr"))
            {
                temporary = (Convert.ToDecimal(CurrentBalanceForBank[0]) + (-1) * Convert.ToDecimal(TotalInputAmount));
                if (temporary == 0)
                    BPlblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                if (temporary < 0)
                {
                    BPlblBankCurrentBalance.Text = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (temporary > 0)
                {
                    BPlblBankCurrentBalance.Text = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
            }
            else
            {
                BPlblBankCurrentBalance.Text = (TotalInputAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }

        }

        private void BPAccount_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmLOVLedger frm = new frmLOVLedger(this, e);
            frm.ShowDialog();
        }

        private void BPChequeDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = true;
            ctx1 = (SourceGrid.CellContext)sender;
            if (ctx1.DisplayText.ToString() != "")
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

        private void BPAmount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdBankPayment.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            string AccName = (string)(grdBankPayment[RowCount - 1, (int)GridColumn1.BPParticular_Account_Head].Value);
            CurrBal = (string)(grdBankPayment[CurRow, (int)GridColumn1.BPCurrent_Balance].Value); // updated current balance is sent for calculation of new current balance
            //Check if the input value is correct
            if (grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn1.BPAmount].Value == "" || grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn1.BPAmount].Value == null)
                grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn1.BPAmount].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            if (!Misc.IsNumeric(grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn1.BPAmount].Value))
            {
                Global.MsgError("Invalid Amount!");
                ctx.Value = "";
                return;
            }
            #region Reference related
            if (m_mode == EntryMode.EDIT)
            {
                grdBankPayment[CurRow, (int)GridColumn1.BPRef_Amt].Value = VoucherReference.GetAmtForAgainstRef(Convert.ToInt32(BPtxtBankID.Text), "BANK_PMT", Convert.ToInt32(grdBankPayment[CurRow, (int)GridColumn1.BPLedger_ID].Value));
            }

            // check if transaction amount is greater than reference amount
            if (!BPCheckAmtAgainstRefAmt())
            {
                return;
            }
            #endregion

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
            
                check = Budget.CheckBudget(Convert.ToInt32(grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn1.BPLedger_ID].Value), Convert.ToDecimal(grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn1.BPAmount].Value), strXML);           

            if (!check)
            {
                grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn1.BPAmount].Value = 0;
            }
            

            double checkformat = Convert.ToDouble(grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn1.BPAmount].Value.ToString());
            //string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn1.BPAmount].Value = checkformat;
            BPFillGridRowExceptLedgerID(CurRow, CurrAccLedgerID, CurrBal, amt);

            BPFilllblCurrentBankBalance();
            if (AccName != "(NEW)")
            {
                BPAddRowBankPayment(RowCount);
            }

        }

        private void BPcmboBankAccount_IndexChanged(object sender, EventArgs e)
        {
            //liBankID = (ListItem)cmboBankAccount.SelectedItem;
            int bankID = Convert.ToInt32(BPcmboBankAccount.SelectedValue);
            #region BLOCK FOR CALCULATIING OPENING BALANCE
            GetOpeningBalance openbal = new GetOpeningBalance();
            int uid = User.CurrUserID;
            DataTable dtroleinfo = User.GetUserInfo(uid);
            DataRow drrole = dtroleinfo.Rows[0];
            int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());
            int ParentAcc = 0;
            DefaultAccClass = Convert.ToInt32(UserPreference.GetValue("ACCOUNT_CLASS", uid));
            ParentAcc = GetRootAccClassIDD();
           
            dtGetOpeningBalance = openbal.GetOpeningBalanceByParent(ParentAcc, bankID);
           
            #endregion
            DataTable dtBankCurrBalance = new DataTable();
          

            string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
            string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
            //dtBankCurrBalance = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, liBankID.ID, null);
            dtBankCurrBalance = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, bankID, null);

            if (dtBankCurrBalance.Rows.Count <= 0)
            {
                if (dtGetOpeningBalance.Rows.Count > 0)
                {
                    DataRow dropeningBal = dtGetOpeningBalance.Rows[0];
                    if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                    {
                        BPlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        BPlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                    }
                    else
                    {
                        BPlblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        BPlblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                    }
                }
                else
                {
                    BPlblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    BPlblBankCurrBalHidden.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }
                BPFilllblCurrentBankBalance();
                return;
            }
            DataRow dr = dtBankCurrBalance.Rows[0];


            DataRow drLdrInfo = dtBankCurrBalance.Rows[0];//Get first record
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
            BPlblBankCurrentBalance.Text = strBalance;
            BPlblBankCurrBalHidden.Text = Balance.ToString();
          //  FilllblCurrentBankBalance();


        }

        /// <summary>
        /// Writes the header part for grdJournal
        /// </summary>
        private void BPAddGridHeader()
        {
            grdBankPayment[0, (int)GridColumn1.BPDel] = new MyHeader1("Del");
            grdBankPayment[0, (int)GridColumn1.BPDel].Column.Width = 30;
            grdBankPayment.Columns[(int)GridColumn1.BPDel].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankPayment[0, (int)GridColumn1.BPCode_No] = new MyHeader1("Code No.");
            grdBankPayment[0, (int)GridColumn1.BPCode_No].Column.Width = 60;
            grdBankPayment.Columns[(int)GridColumn1.BPCode_No].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankPayment[0, (int)GridColumn1.BPParticular_Account_Head] = new MyHeader1("Particular/Accounting Head");
            grdBankPayment[0, (int)GridColumn1.BPParticular_Account_Head].Column.Width = 150;
            grdBankPayment.Columns[(int)GridColumn1.BPParticular_Account_Head].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankPayment[0, (int)GridColumn1.BPAmount] = new MyHeader1("Amount");
            grdBankPayment[0, (int)GridColumn1.BPAmount].Column.Width = 100;
            grdBankPayment.Columns[(int)GridColumn1.BPAmount].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankPayment[0, (int)GridColumn1.BPCurrent_Balance] = new MyHeader1("CurrentBalance");
            grdBankPayment[0, (int)GridColumn1.BPCurrent_Balance].Column.Width = 100;
            grdBankPayment.Columns[(int)GridColumn1.BPCurrent_Balance].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankPayment[0, (int)GridColumn1.BPRemarks] = new MyHeader1("Remarks");
            grdBankPayment[0, (int)GridColumn1.BPRemarks].Column.Width = 100;
            grdBankPayment.Columns[(int)GridColumn1.BPRemarks].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankPayment[0, (int)GridColumn1.BPCheque_No] = new MyHeader1("Cheque Number");
            grdBankPayment[0, (int)GridColumn1.BPCheque_No].Column.Width = 100;
            grdBankPayment.Columns[(int)GridColumn1.BPCheque_No].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankPayment[0, (int)GridColumn1.BPChequeDate] = new MyHeader1("Cheque Date");
            grdBankPayment[0, (int)GridColumn1.BPChequeDate].Column.Width = 100;
            grdBankPayment.Columns[(int)GridColumn1.BPChequeDate].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdBankPayment[0, (int)GridColumn1.BPLedger_ID] = new MyHeader1("LedgerID");
            grdBankPayment[0, (int)GridColumn1.BPLedger_ID].Column.Visible = false;

            grdBankPayment[0, (int)GridColumn1.BPCurrent_Bal_Actual] = new MyHeader1("CurrentBalance");
            grdBankPayment[0, (int)GridColumn1.BPCurrent_Bal_Actual].Column.Visible = false;
            
            grdBankPayment[0, (int)GridColumn1.BPRef_Amt] = new SourceGrid.Cells.ColumnHeader("RefAmt");
            grdBankPayment[0, (int)GridColumn1.BPRef_Amt].Column.Width = 50;
            grdBankPayment[0, (int)GridColumn1.BPRef_Amt].Column.Visible = false;

        }

        /// <summary>
        /// Adds the row in the Journal field
        /// </summary>
        private void BPAddRowBankPayment(int RowCount)
        {
            //Add a new row
            grdBankPayment.Redim(Convert.ToInt32(RowCount + 1), grdBankPayment.ColumnsCount);
            SourceGrid.Cells.Button BPbtnDelete = new SourceGrid.Cells.Button("");
            BPbtnDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;

            int i = RowCount;
            grdBankPayment[i, (int)GridColumn1.BPDel] = BPbtnDelete;
            grdBankPayment[i, (int)GridColumn1.BPDel].AddController(BPevtDelete);

            grdBankPayment[i, (int)GridColumn1.BPCode_No] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox BPtxtPmtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            BPtxtPmtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdBankPayment[i, (int)GridColumn1.BPParticular_Account_Head] = new SourceGrid.Cells.Cell("", BPtxtPmtAccount);
            BPtxtPmtAccount.Control.GotFocus += new EventHandler(BPAccount_Focused);
            BPtxtPmtAccount.Control.LostFocus += new EventHandler(BPAccount_Leave);
            BPtxtPmtAccount.Control.KeyDown += new KeyEventHandler(BPAccount_KeyDown);
            BPtxtPmtAccount.Control.TextChanged += new EventHandler(BPText_Change);
            grdBankPayment[i, (int)GridColumn1.BPParticular_Account_Head].AddController(BPevtAccountFocusLost);
            grdBankPayment[i, (int)GridColumn1.BPParticular_Account_Head].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox BPtxtPmtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            BPtxtPmtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankPayment[i, (int)GridColumn1.BPAmount] = new SourceGrid.Cells.Cell("", BPtxtPmtAmount);
            BPtxtPmtAmount.Control.TextChanged += new EventHandler(BPText_Change);
            grdBankPayment[i, (int)GridColumn1.BPAmount].AddController(BPevtAmountFocusLost);
            BPtxtPmtAmount.Control.GotFocus += new EventHandler(BPAmount_Focused);
            grdBankPayment[i, (int)GridColumn1.BPAmount].Value = "";

            SourceGrid.Cells.Editors.TextBox BPtxtCurrentBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            BPtxtCurrentBalance.EditableMode = SourceGrid.EditableMode.None;
            grdBankPayment[i, (int)GridColumn1.BPCurrent_Balance] = new SourceGrid.Cells.Cell("", BPtxtCurrentBalance);
            grdBankPayment[i, (int)GridColumn1.BPCurrent_Balance].Value = "";

            SourceGrid.Cells.Editors.TextBox BPtxtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            BPtxtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankPayment[i, (int)GridColumn1.BPRemarks] = new SourceGrid.Cells.Cell("", BPtxtRemarks);
            BPtxtRemarks.Control.TextChanged += new EventHandler(BPText_Change);

            SourceGrid.Cells.Editors.TextBox BPtxtChequeNumber = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            BPtxtChequeNumber.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankPayment[i, (int)GridColumn1.BPCheque_No] = new SourceGrid.Cells.Cell("", BPtxtChequeNumber);
            BPtxtChequeNumber.Control.TextChanged += new EventHandler(BPText_Change);

            SourceGrid.Cells.Button BPbtnChequeDate = new SourceGrid.Cells.Button(""); //Date.ToSystem(DateTime.Today)
            BPtxtChequeNumber.EditableMode = SourceGrid.EditableMode.SingleClick;
            //btnChequeDate.Controller.OnClick += new EventHandler(Text_Change);
            grdBankPayment[i, (int)GridColumn1.BPChequeDate] = BPbtnChequeDate;
            grdBankPayment[i, (int)GridColumn1.BPChequeDate].AddController(BPevtChequeDateFocusLost);

            SourceGrid.Cells.Editors.TextBox BPtxtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            BPtxtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdBankPayment[i, (int)GridColumn1.BPLedger_ID] = new SourceGrid.Cells.Cell("", BPtxtLedgerID);
            grdBankPayment[i, (int)GridColumn1.BPLedger_ID].Value = "";

            SourceGrid.Cells.Editors.TextBox BPtxtCurrBal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            BPtxtCurrBal.EditableMode = SourceGrid.EditableMode.None;
            grdBankPayment[i, (int)GridColumn1.BPCurrent_Bal_Actual] = new SourceGrid.Cells.Cell("", BPtxtCurrBal);
            grdBankPayment[i, (int)GridColumn1.BPCurrent_Bal_Actual].Value = "";

            SourceGrid.Cells.Editors.TextBox BPtxtRefAmt = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            grdBankPayment[i, (int)GridColumn1.BPRef_Amt] = new SourceGrid.Cells.Cell("", BPtxtRefAmt);
            grdBankPayment[i, (int)GridColumn1.BPRef_Amt].Value = "0(Dr)";

        }

        private void BPChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    BPEnableControls(false);
                    BPButtonState(true, true, false, true, false);
                    IsFieldChanged = false;
                    break;
                case EntryMode.NEW:
                    BPEnableControls(true);
                    BPButtonState(false, false, true, false, true);
                    BPLoadSeriesNo();
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
                case EntryMode.EDIT:
                    BPEnableControls(true);
                    BPButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
            }
        }

        private void BPLoadSeriesNo()
        {
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("BANK_PMNT");
            BPcboSeriesName.Items.Clear();
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                BPcboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            BPcboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            BPcboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            BPcboSeriesName.SelectedIndex = 0;
        }

        private void BPEnableControls(bool Enable)
        {
            chkRecurring.Enabled = btnSetup.Enabled = BPtxtVchNo.Enabled = BPtxtDate.Enabled = BPtxtRemarks.Enabled = 
                grdBankPayment.Enabled = BPcboSeriesName.Enabled = BPcmboBankAccount.Enabled = BPcboProjectName.Enabled = 
                tabControl1.Enabled = Enable;
        }

        private void BPButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            BPbtnNew.Enabled = New;
            BPbtnEdit.Enabled = Edit;
            BPbtnSave.Enabled = Save;
            BPbtnDelete.Enabled = Delete;
            BPbtnCancel.Enabled = Cancel;
        }

        private bool BPValidate()
        {
            bool bValidate = false;
            FormHandle m_FH = new FormHandle();
            bValidate = m_FH.Validate();
            if (BPcboSeriesName.SelectedItem == null)
            {
                Global.MsgError("Invalid Series Name Selected");
                BPcboSeriesName.Focus();
                bValidate = false;
            }
            if (BPcmboBankAccount.SelectedItem == null)
            {
                Global.MsgError("Invalid Bank Account Selected");
                BPcmboBankAccount.Focus();
                bValidate = false;
            }
            if (BPcboProjectName.SelectedItem == null)
            {
                Global.MsgError("Invalid Project Name Selected");
                BPcboProjectName.Focus();
                bValidate = false;
            }
            if (!(grdBankPayment.Rows.Count > 1))
            {
                Global.MsgError("Invalid Account Ledger Selected in grid");
                grdBankPayment.Focus();
                bValidate = false;
            }
            return bValidate;
        }

        private void BPbtnEdit_Clicks(object sender, EventArgs e)
        {
            BPEnableControls(true);
            BPChangeState(EntryMode.EDIT);
        }

        private void BPClearVoucher()
        {
            m_isRecurring = false;
            m_RVID = 0;
            BPClearBankPayment();
            grdBankPayment.Redim(2, 11);
            BPAddGridHeader(); //Write header part
            BPAddRowBankPayment(1);
            ClearRecurringSetting();
            BPdtReference.Rows.Clear();
            BPAddReferenceColumns();
        }

        private void BPClearBankPayment()
        {
            BPtxtVchNo.Clear(); //actually generate a new voucher no.
            // txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            BPtxtRemarks.Clear();
           
            BPLoadComboboxBank();
            BPcboSeriesName.Text = string.Empty;
            BPcboProjectName.SelectedIndex = 0;
            BPlblTotalAmount.Text = "";
        }

        private bool BPNavigation(Navigate NavTo)
        {
            try
            {
                if (BPtxtBankID.Text != "")
                    BPdtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(BPtxtBankID.Text), "BANK_PMNT");

                if (!IsShortcutKey)
                {
                    BPChangeState(EntryMode.NORMAL);
                }
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(BPtxtBankID.Text);
                    if (BankPaymentIDCopy > 0)
                    {
                        VouchID = BankPaymentIDCopy;
                        BankPaymentIDCopy = 0;
                    }
                    else
                    {
                        VouchID = Convert.ToInt32(BPtxtBankID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
                IJournal m_Journal = new Journal();

                DataTable dtBankPaymentMaster = m_BankPayment.NavigateBankPaymentMaster(VouchID, NavTo);
                if (dtBankPaymentMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }
                //this is for short cut key eg in narration CTRL+R
                DataRow drBankPaymentMaster = dtBankPaymentMaster.Rows[0]; //There is only one row. First row is the required record 
                if (IsShortcutKey)
                {
                    BPtxtRemarks.Text = drBankPaymentMaster["Remarks"].ToString();
                    IsShortcutKey = false;
                    BPtxtRemarks.SelectionStart = BPtxtRemarks.Text.Length + 1;
                    return false;
                }

                //Clear everything in the form
                BPClearVoucher();
                //Write the corresponding textboxes


                //Show the corresponding Cash Account Ledger in Combobox
                DataTable dtBankLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankPaymentMaster["LedgerID"]), LangMgr.DefaultLanguage);
                DataRow drBankLedgerInfo = dtBankLedgerInfo.Rows[0];

                BPcmboBankAccount.Text = drBankLedgerInfo["LedName"].ToString();

                //Show the Corresponding SeriesName in Combobox
                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drBankPaymentMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Bank Payment");
                    BPcboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    BPcboSeriesName.Text = dr["EngName"].ToString();
                }
                BPlblVouNo.Visible = true;
                BPtxtVchNo.Visible = true;
                BPtxtVchNo.Text = drBankPaymentMaster["Voucher_No"].ToString();
                BPtxtDate.Text = Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString());
                BPtxtRemarks.Text = drBankPaymentMaster["Remarks"].ToString();

                DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drBankPaymentMaster["ProjectID"]), LangMgr.DefaultLanguage);
                if (dtProjectInfo.Rows.Count != 0)
                {
                    DataRow drProjectInfo = dtProjectInfo.Rows[0];
                    BPcboProjectName.Text = drProjectInfo["Name"].ToString();
                    BPtxtBankID.Text = drBankPaymentMaster["BankPaymentID"].ToString();
                    dsBankPayment.Tables["tblBankPaymentMaster"].Rows.Add(BPcboSeriesName.Text, drBankPaymentMaster["Voucher_No"].ToString(), Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString()), BPcmboBankAccount.Text, drBankPaymentMaster["Remarks"].ToString(), drProjectInfo["Name"].ToString());
                }
                else
                {
                    BPcboProjectName.Text = "None";
                    BPtxtBankID.Text = drBankPaymentMaster["BankPaymentID"].ToString();
                    dsBankPayment.Tables["tblBankPaymentMaster"].Rows.Add(BPcboSeriesName.Text, drBankPaymentMaster["Voucher_No"].ToString(), Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString()), BPcmboBankAccount.Text, drBankPaymentMaster["Remarks"].ToString(), "None");
                }

                //For Additional Fields
                if (NumberOfFields > 0)
                {
                    if (NumberOfFields == 1)
                    {
                        BPlblfirst.Visible = true;
                        BPtxtfirst.Visible = true;
                        BPlblsecond.Visible = false;
                        BPtxtsecond.Visible = false;
                        BPlblthird.Visible = false;
                        BPtxtthird.Visible = false;
                        BPlblfourth.Visible = false;
                        BPtxtfourth.Visible = false;
                        BPlblfifth.Visible = false;
                        BPtxtfifth.Visible = false;
                        BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();

                        BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 2)
                    {
                        BPlblfirst.Visible = true;
                        BPtxtfirst.Visible = true;
                        BPlblsecond.Visible = true;
                        BPtxtsecond.Visible = true;
                        BPlblthird.Visible = false;
                        BPtxtthird.Visible = false;
                        BPlblfourth.Visible = false;
                        BPtxtfourth.Visible = false;
                        BPlblfifth.Visible = false;
                        BPtxtfifth.Visible = false;
                        BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();

                        BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();
                    }
                    else if (NumberOfFields == 3)
                    {
                        BPlblfirst.Visible = true;
                        BPtxtfirst.Visible = true;
                        BPlblsecond.Visible = true;
                        BPtxtsecond.Visible = true;
                        BPlblthird.Visible = true;
                        BPtxtthird.Visible = true;
                        BPlblfourth.Visible = false;
                        BPtxtfourth.Visible = false;
                        BPlblfifth.Visible = false;
                        BPtxtfifth.Visible = false;
                        BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        BPlblthird.Text = drdtadditionalfield["Field3"].ToString();

                        BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 4)
                    {
                        BPlblfirst.Visible = true;
                        BPtxtfirst.Visible = true;
                        BPlblsecond.Visible = true;
                        BPtxtsecond.Visible = true;
                        BPlblthird.Visible = true;
                        BPtxtthird.Visible = true;
                        BPlblfourth.Visible = true;
                        BPtxtfourth.Visible = true;
                        BPlblfifth.Visible = false;
                        BPtxtfifth.Visible = false;
                        BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        BPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                        BPlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                        BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();

                    }
                    else if (NumberOfFields == 5)
                    {
                        BPlblfirst.Visible = true;
                        BPtxtfirst.Visible = true;
                        BPlblsecond.Visible = true;
                        BPtxtsecond.Visible = true;
                        BPlblthird.Visible = true;
                        BPtxtthird.Visible = true;
                        BPlblfourth.Visible = true;
                        BPtxtfourth.Visible = true;
                        BPlblfifth.Visible = true;
                        BPtxtfifth.Visible = true;

                        BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                        BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                        BPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                        BPlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                        BPlblfifth.Text = drdtadditionalfield["Field5"].ToString();

                        BPtxtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        BPtxtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        BPtxtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        BPtxtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        BPtxtfifth.Text = drBankPaymentMaster["Field5"].ToString();
                    }


                }
                DataTable dtBankPaymentDetail = m_BankPayment.GetBankPaymentDetail(Convert.ToInt32(BPtxtBankID.Text));
                for (int i = 1; i <= dtBankPaymentDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtBankPaymentDetail.Rows[i - 1];
                    grdBankPayment[i, (int)GridColumn1.BPCode_No].Value = i.ToString();
                    grdBankPayment[i, (int)GridColumn1.BPParticular_Account_Head].Value = drDetail["LedgerName"].ToString();
                    grdBankPayment[i, (int)GridColumn1.BPAmount].Value = Convert.ToDecimal(drDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces));
                    grdBankPayment[i, (int)GridColumn1.BPRemarks].Value = drDetail["Remarks"].ToString();
                    grdBankPayment[i, (int)GridColumn1.BPCheque_No].Value = drDetail["ChequeNumber"].ToString();
                    grdBankPayment[i, (int)GridColumn1.BPLedger_ID].Value = drDetail["ledgerid"].ToString();
                    if (drDetail["ChequeDate"].ToString() == "")
                    {
                        grdBankPayment[i, (int)GridColumn1.BPChequeDate].Value = "";
                    }
                    else
                    {
                        grdBankPayment[i, (int)GridColumn1.BPChequeDate].Value = Date.DBToSystem(drDetail["ChequeDate"].ToString());
                    }
                    //Code To Get The Current Balance of the Respective Ledger
                    string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
                    string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
                    DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(drDetail["LedgerID"]), null);
                    if (dtLdrInfo.Rows.Count != 1)
                    {
                        grdBankPayment[i, (int)GridColumn1.BPCurrent_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        grdBankPayment[i, (int)GridColumn1.BPCurrent_Bal_Actual].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
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
                        strBalance = ((Balance < 0) ? Balance * -1 : Balance).ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces));
                        if (Balance >= 0)
                            strBalance = strBalance + " (Dr.)";

                        else //If balance is -ve, its Cr.
                            strBalance = strBalance + " (Cr.)";


                        //Write balance into the grid
                        grdBankPayment[i, (int)GridColumn1.BPCurrent_Balance].Value = strBalance;
                        grdBankPayment[i, (int)GridColumn1.BPCurrent_Bal_Actual].Value = Balance.ToString();


                    }
                    BPAddRowBankPayment(grdBankPayment.RowsCount);
                    dsBankPayment.Tables["tblBankPaymentDetails"].Rows.Add(drDetail["LedgerName"].ToString(), (Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                    totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                    totalRptAmt = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();
                }
                BPFilllblCurrentBankBalance();

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(BPtxtBankID.Text), "BANK_PMNT");
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
                    CheckRecurringSetting(BPtxtBankID.Text);
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
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}           
            BPNavigation(Navigate.First);
            IsFieldChanged = false;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            BPNavigation(Navigate.Prev);
            IsFieldChanged = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            BPNavigation(Navigate.Next);
            IsFieldChanged = false;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            BPNavigation(Navigate.Last);
            IsFieldChanged = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BPbtnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            BankPaymentIDCopy = Convert.ToInt32(BPtxtBankID.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (BankPaymentIDCopy > 0)
            {
                if (m_mode == EntryMode.NEW)
                {
                    BPNavigation(Navigate.ID);
                    BPEnableControls(true);
                    BPChangeState(EntryMode.NEW);
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
            BPPrintPreviewCR(PrintType.CrystalReport);
        }

        private void cboSeriesName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BPtxtDate.Focus();
            }
        }

        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BPcmboBankAccount.Focus();
            }
        }

        private void cmboBankAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BPcboProjectName.Focus();
            }
        }

        private void cboProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                grdBankPayment.Focus();
            }
        }

        private void bPtxtRemarks_KeyDown(object sender, KeyEventArgs e)
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
                BPNavigation(Navigate.Last);
            }
            if (e.KeyValue == 13)
            {
                BPchkDoNotClose.Focus();
            }
        }

        private void chkDoNotClose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BPchkPrntWhileSaving.Focus();
            }
        }

        private void chkPrntWhileSaving_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                BPbtnSave.Focus();
            }
        }

        private void frmBankPayment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            //else if (e.KeyCode == Keys.N && e.Control)
            //{
            //    btnNew_Click_1(sender, e);
            //}
            else if (e.KeyCode == Keys.E && e.Control)
            {
                BPbtnEdit_Clicks(sender, e);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                BPbtnSave_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete && e.Control)
            {
                BPbtnDelete_Click(sender, e);
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

        private void BPbtnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }

            BPClearVoucher();
            BPEnableControls(true);
            BPChangeState(EntryMode.NEW);

        }

        private void BPbtnDelete_Click(object sender, EventArgs e)
        {
            if (BPCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }

            NewGrid = NewGrid + "Voucher No" + BPtxtVchNo.Text + "Series" + BPcboSeriesName.Text + "Project" + BPcboProjectName.Text + "Date" + BPtxtDate.Text + "Bank" + BPcmboBankAccount.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdBankPayment[i + 1, (int)GridColumn1.BPParticular_Account_Head].Value.ToString();
                string amt = grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value.ToString();
                NewGrid = NewGrid + string.Concat(particular, amt);
            }
            NewGrid = "NewGridValues" + NewGrid;
            if (Freeze.IsDateFreeze(Date.ToDotNet(BPtxtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            try
            {
                if (!String.IsNullOrEmpty(BPtxtBankID.Text))
                {
                    if (Global.MsgQuest("Are you sure you want to delete the Bank Payment - " + BPtxtBankID.Text + "?") == DialogResult.Yes)
                    {
                        BankPayment m_BankPayment = new BankPayment();
                        // delete reference
                        string res = VoucherReference.DeleteReference(Convert.ToInt32(BPtxtBankID.Text), "BANK_PMNT");
                        if (res != "Success")
                        {
                            Global.MsgError("Unable to delete the voucher due to " + res);
                            return;
                        }

                        if (m_BankPayment.RemoveBankPaymentEntry(Convert.ToInt32(BPtxtBankID.Text)))
                        {

                            AuditLogDetail auditlog = new AuditLogDetail();
                            auditlog.ComputerName = Global.ComputerName;
                            auditlog.UserName = User.CurrentUserName;
                            auditlog.Voucher_Type = "BANK_PAYMENT";
                            auditlog.Action = "DELETE";
                            auditlog.Description = NewGrid;
                            auditlog.RowID = Convert.ToInt32(BPtxtBankID.Text);
                            auditlog.MAC_Address = Global.MacAddess;
                            auditlog.IP_Address = Global.IpAddress;
                            auditlog.VoucherDate = Date.ToDB(DateTime.Now).ToString();

                            auditlog.CreateAuditLog(auditlog);
                           
                            RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "BANK_PAYMENT"); // deleting the recurring setting if voucher is deleted

                            Global.Msg("Bank Payment -" + BPtxtBankID.Text + " deleted successfully!");
                            //Navigate to 1 step previous
                            if (!this.BPNavigation(Navigate.Prev))
                            {
                                //This must be because there are no records or this was the first one
                                //If this was the first, try to navigate to second
                                if (!this.BPNavigation(Navigate.Next))
                                {
                                    //This was the last one, there are no records left. Simply clear the form and stay calm
                                    BPChangeState(EntryMode.NEW);
                                }
                            }
                        }
                        else
                            Global.MsgError("There was an error while deleting Bank Payment -" + BPtxtBankID.Text + "!");
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void BPbtnEdit_Click(object sender, EventArgs e)
        {
            if (BPCheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }


            if (Freeze.IsDateFreeze(Date.ToDotNet(BPtxtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }

            if (BPtxtBankID.Text.Length <= 0)
            {
                Global.MsgError("Please navigate to existing Bank entry first and then try again!");
                return;
            }
            isNew = false;
            OldGrid = " ";
            OldGrid = OldGrid + "Voucher No" + BPtxtVchNo.Text + "Series" + BPcboSeriesName.Text + "Project" + BPcboProjectName.Text + "Date" + BPtxtDate.Text + "Bank" + BPcmboBankAccount.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdBankPayment[i + 1, (int)GridColumn1.BPParticular_Account_Head].Value.ToString();
                string amt = grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value.ToString();
                OldGrid = OldGrid + string.Concat(particular, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;

            BPEnableControls(true);
            BPChangeState(EntryMode.EDIT);

            //if automatic voucher number increment is selected
            string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
            if (NumberingType == "AUTOMATIC")
                BPtxtVchNo.Enabled = false;
        }

        private void BPbtnSave_Click(object sender, EventArgs e)
        {
            if (BPCheckIfBankReconciliationClosed())
            {
                return;
            }


            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your Administrator for permission.");
                return;
            }


            if (Freeze.IsDateFreeze(Date.ToDotNet(BPtxtDate.Text)))
            {
                return;
            }

            #region BLOCK FOR MANUAL VOUCHER NUMBERING TYPE
            VoucherConfiguration m_VouConfig = new VoucherConfiguration();
            if (SeriesID.ID > 0)
            {
                DataTable dtVouConfigInfo = m_VouConfig.GetVouNumConfiguration(Convert.ToInt32(SeriesID.ID));
                DataRow drVouConfigInfo = dtVouConfigInfo.Rows[0];
                if (dtVouConfigInfo.Rows.Count < 0)
                    return;
                if (drVouConfigInfo["NumberingType"].ToString() == "Manual")
                {
                    //Enter in this block only when VoucherNumberingType is Manual
                    //Checking for Manual VoucherNumberingType
                    try
                    {
                        string returnStr = m_VouConfig.ValidateManualVouNum(BPtxtVchNo.Text, Convert.ToInt32(SeriesID.ID));
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

            //ListItem liLedgerID = new ListItem();
            //liLedgerID = (ListItem)cmboBankAccount.SelectedItem;
            int bankID = Convert.ToInt32(BPcmboBankAccount.SelectedValue);
            //Check Validation
            if (!BPValidate())
                return;

            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }
            if (drdtadditionalfield["IsField1Required"].ToString() == "True")
            {
                if (BPtxtfirst.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field1"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField2Required"].ToString() == "True")
            {
                if (BPtxtsecond.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field2"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField3Required"].ToString() == "True")
            {
                if (BPtxtthird.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field3"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField4Required"].ToString() == "True")
            {
                if (BPtxtfourth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field4"].ToString() + " " + "is Required Field");
                    return;
                }

            }
            if (drdtadditionalfield["IsField5Required"].ToString() == "True")
            {
                if (BPtxtfifth.Text == "")
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
                        BPdueDate();
                        frmDueDate dueDate1 = new frmDueDate(this, dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            dueDate1.ShowDialog();
                        }

                        #region Add voucher number if voucher number is automatic and hidden from the setting
                        int increasedSeriesNum = 0;
                        SeriesID = (ListItem)BPcboSeriesName.SelectedItem;
                        string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.GenerateVouNumTypeNoUpdate(Convert.ToInt32(SeriesID.ID), out increasedSeriesNum);
                            if (m_vounum == null)
                            {
                                MessageBox.Show("Your voucher numbers are totally finished!");
                                return;
                            }

                            BPtxtVchNo.Text = m_vounum.ToString();
                            BPtxtVchNo.Enabled = false;
                        }
                        #endregion

                        isNew = true;
                        NewGrid = " ";
                        OldGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + BPtxtVchNo.Text + "Series" + BPcboSeriesName.Text + "Project" + BPcboProjectName.Text + "Date" + BPtxtDate.Text + "Bank" + BPcmboBankAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdBankPayment[i + 1, (int)GridColumn1.BPParticular_Account_Head].Value.ToString();
                            string amt = grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;

                        //Call to Convert into XML Format
                        string BankPaymentXMLString = BPReadAllBankPaymentEntry();
                        if (BankPaymentXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast bank payment entry to XML!");
                            return;
                        }
                        //return;

                        #region Used for negative bank and negative cash Delete after transfering to Sp
                        //Read from sourcegrid and store it to table
                        DataTable BankPaymentDetails = new DataTable();
                        BankPaymentDetails.Columns.Add("Ledger");
                        BankPaymentDetails.Columns.Add("Amount");
                        BankPaymentDetails.Columns.Add("Remarks");
                        BankPaymentDetails.Columns.Add("ChequeNumber");
                        BankPaymentDetails.Columns.Add("ChequeDate");
                        double totalgrdAmount = 0;
                        for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdBankPayment[i + 1, (int)GridColumn1.BPParticular_Account_Head].ToString().Split('[');
                            if (grdBankPayment[i + 1, (int)GridColumn1.BPRemarks].Value == "")
                            {
                                grdBankPayment[i + 1, (int)GridColumn1.BPCheque_No].Value = "";
                            }

                            BankPaymentDetails.Rows.Add(ledgerName[0].ToString(), grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value, grdBankPayment[i + 1, (int)GridColumn1.BPCurrent_Balance].Value, grdBankPayment[i + 1, (int)GridColumn1.BPRemarks].Value, grdBankPayment[i + 1, (int)GridColumn1.BPCheque_No].Value);
                            //Check for empty Amount
                            object objAmount = grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            
                 
                        }
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
                            Transaction.GetLedgerBalance(null, null, bankID, ref mDbalBank, ref mCbalBank, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero

                            //Incase of Bank Payment,master ledger is bydefulat Bank...and here Bank is Payment soo it would be Credit because Bank is paying amount for other account
                            //Here Total Credit amount of master is calculated from Details section by adding all amount of all account in Details section
                            totalCrBank += (mCbalBank + totalgrdAmount);//this is the amount of self ledger(Bank Ledger) and all the amount of detail portion.Actually amount of detail section and master section should be equall soo whatever amount reamin in detail section will be same in master section                        
                            totalDrBank = mDbalBank;
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

                        #endregion

                        #region Save xml data to Database
                        int returnJournalId = 0;
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                    
                                //To insert BankPayment
                                returnJournalId = BankPayment.InsertBankPayment(BankPaymentXMLString);
                                // to send recurring settings
                                BankPayment.InsertVoucherRecurring(m_dtRecurringSetting, returnJournalId);

                                // to send reference information
                                BankPayment.InsertReference(returnJournalId, BPdtReference);
                                //}
                            
                        }
                        #endregion

                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }

                        Global.Msg("Bank payment created successfully!");
                        // if the voucher is recurring and has been posted or saved, modify voucherposting table to set isPosted = true
                        string res;
                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_BankPaymentID, "BANK_PAYMENT");
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID);
                            m_isRecurring = false;
                        }

                        // AccClassID.Clear();
                        BPClearVoucher();
                        //ChangeState(EntryMode.NEW);
                        //btnNew_Click_1(sender, e);

                      

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
                        BPdueDate();
                        frmDueDate dueDate1 = new frmDueDate(this, dt1,Convert.ToInt32(BPtxtBankID.Text),"BANK_PMNT");
                        if (dt1.Rows.Count > 0)
                        {
                            dueDate1.ShowDialog();
                        }
                        isNew = false;
                        NewGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + BPtxtVchNo.Text + "Series" + BPcboSeriesName.Text + "Project" + BPcboProjectName.Text + "Date" + BPtxtDate.Text + "Bank" + BPcmboBankAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdBankPayment[i + 1, (int)GridColumn1.BPParticular_Account_Head].Value.ToString();
                            string amt = grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;

                        //Call to Convert into XML Format
                        string BankPaymentXMLString = BPReadAllBankPaymentEntry();
                        if (BankPaymentXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast bank payment entry to XML!");
                            return;
                        }

                        #region negative bank and cash check
                        //Read from sourcegrid and store it to table
                        DataTable BankPaymentDetails = new DataTable();
                        BankPaymentDetails.Columns.Add("Ledger");
                        BankPaymentDetails.Columns.Add("Amount");
                        BankPaymentDetails.Columns.Add("Remarks");
                        BankPaymentDetails.Columns.Add("ChequeNumber");
                        BankPaymentDetails.Columns.Add("ChequeDate");
                        double totalgrdAmount = 0;
                        for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdBankPayment[i + 1, (int)GridColumn1.BPParticular_Account_Head].ToString().Split('[');
                            if (grdBankPayment[i + 1, (int)GridColumn1.BPRemarks].Value == "")
                            {
                                grdBankPayment[i + 1, (int)GridColumn1.BPCheque_No].Value = "";
                            }
                            BankPaymentDetails.Rows.Add(ledgerName[0].ToString(), grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value, grdBankPayment[i + 1, (int)GridColumn1.BPCurrent_Balance].Value, grdBankPayment[i + 1, (int)GridColumn1.BPRemarks].Value, grdBankPayment[i + 1, (int)GridColumn1.BPCheque_No].Value);
                            //Check for empty Amount
                            object objAmount = grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            //Generally in this section no need to check negative cash and negative bank because all ledgeres associated in detai section are receiver...they will get amount from master section.
                          
                        }
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
                            Transaction.GetLedgerBalance(null, null, bankID, ref mDbalBank, ref mCbalBank, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero

                            //Incase of Bank Payment,master ledger is bydefulat Bank...and here Bank is Payment soo it would be Credit because Bank is paying amount for other account
                            //Here Total Credit amount of master is calculated from Details section by adding all amount of all account in Details section
                            totalCrBank += (mCbalBank + totalgrdAmount);//this is the amount of self ledger(Bank Ledger) and all the amount of detail portion.Actually amount of detail section and master section should be equall soo whatever amount reamin in detail section will be same in master section
                            totalDrBank = mDbalBank;
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

                        #endregion


                      

                        #region Save xml data to Database
                        int returnJournalId = 0;
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                           

                            //to edit bank payment
                                BankPayment.EditBankPayment(BankPaymentXMLString);

                                BankPayment.ModifyVoucherRecurring(m_dtRecurringSetting, Convert.ToInt32(BPtxtBankID.Text));
                                //MessageBox.Show(intRetValue.ToString());   

                                // to modify against references in the voucher
                                BankPayment.ModifyReference(Convert.ToInt32(BPtxtBankID.Text), BPdtReference, ToDeleteRows);
                            //}
                        }
                        #endregion

                        if (!BPValidate())
                            return;
                        Global.Msg("Bank payment modified successfully!");
                        // AccClassID.Clear();
                        BPtxtBankID.Clear();
                        BPClearVoucher();
                        BPChangeState(EntryMode.NORMAL);
                        BPbtnNew_Click(sender, e);

                        if (BPchkPrntWhileSaving.Checked)
                        {
                            prntDirect = 1;
                            BPNavigation(Navigate.Last);
                            btnPrint_Click(sender, e);
                            BPClearVoucher();
                            BPChangeState(EntryMode.NEW);
                            BPbtnNew_Click(sender, e);
                        }
                        if (!BPchkDoNotClose.Checked)
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

        /// <summary>
        /// Read all journal Entry
        /// </summary>
        /// <returns></returns>
        private string BPReadAllBankPaymentEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            SeriesID = (ListItem)BPcboSeriesName.SelectedItem;
            liProjectID = (ListItem)BPcboProjectName.SelectedItem;
            //liBankID = (ListItem)cmboBankAccount.SelectedItem;
            int bankID = Convert.ToInt32(BPcmboBankAccount.SelectedValue);
            //validate grid entry
            if (!BPValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();

            #region  Bank Payment
            tw.WriteStartElement("BANKPAYMENT");
            {
                ///For Bank Payment Master Section
                ///int JournalID = System.Convert.ToInt32(9); // Auto increment  
                ///
                string first = BPtxtfirst.Text;
                string second = BPtxtsecond.Text;
                string third = BPtxtthird.Text;
                string fourth = BPtxtfourth.Text;
                string fifth = BPtxtfifth.Text;
                int SID = System.Convert.ToInt32(SeriesID.ID);
                int LedgerID = System.Convert.ToInt32(bankID);
                string Voucher_No = System.Convert.ToString(BPtxtVchNo.Text);
                string BankID = System.Convert.ToString(BPtxtBankID.Text);
                DateTime BankPayment_Date = Date.ToDotNet(BPtxtDate.Text);
                string Remarks = System.Convert.ToString(BPtxtRemarks.Text);
                int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                DateTime Created_Date = System.Convert.ToDateTime(Date.GetServerDate());
                string Created_By = User.CurrentUserName;
                DateTime Modified_Date = System.Convert.ToDateTime(Date.GetServerDate());
                string Modified_By = User.CurrentUserName;

                tw.WriteStartElement("BANKPAYMENTMASTER");
                tw.WriteElementString("SeriesID", SID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("BankPaymentID", BankID.ToString());
                tw.WriteElementString("BankPayment_Date", Date.ToDB(BankPayment_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("ProjectID", ProjectID.ToString());
                tw.WriteElementString("Created_Date", Date.ToDB(Created_Date));

                tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Date.ToDB(Modified_Date));
                tw.WriteElementString("Field1", first);
                tw.WriteElementString("Field2", second);
                tw.WriteElementString("Field3", third);
                tw.WriteElementString("Field4", fourth);
                tw.WriteElementString("Field5", fifth);

                tw.WriteEndElement();
                ///For journal Detail Section                           
                int LedgerID1 = 0;
                Decimal Amount = 0;
                string RemarksDetail = "";
                string ChequeNumber = "";
                string ChequeDate = "";
                DateTime ChequeDateValue;
                tw.WriteStartElement("BANKPAYMENTDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    BankID = System.Convert.ToString(BPtxtBankID.Text);
                    LedgerID1 = System.Convert.ToInt32(grdBankPayment[i + 1, (int)GridColumn1.BPLedger_ID].Value);
                    Amount = System.Convert.ToDecimal(grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value);
                    RemarksDetail = System.Convert.ToString(grdBankPayment[i + 1, (int)GridColumn1.BPRemarks].Value);
                    ChequeNumber = System.Convert.ToString(grdBankPayment[i + 1, (int)GridColumn1.BPCheque_No].Value);
                    if (ChequeNumber == "")
                    {
                        ChequeDate = "";
                        ChequeDateValue = Date.GetServerDate();
                    }
                    else
                    {
                        ChequeDate = System.Convert.ToString(grdBankPayment[i + 1, (int)GridColumn1.BPChequeDate].Value);
                        ChequeDateValue = Date.ToDotNet(ChequeDate);
                    }

                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("BankPaymentID", BankID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID1.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteElementString("ChequeNumber", ChequeNumber.ToString());
                    if (ChequeDate != "")
                        // tw.WriteElementString("ChequeDate",Date.ToDotNet(ChequeDate).ToString());
                        tw.WriteElementString("ChequeDate", Date.ToDB(ChequeDateValue));
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                tw.WriteStartElement("BANKDEBTORSDUEDATE");
                foreach (DataRow drduedate in dtDueDateInfo.Rows)
                {
                    tw.WriteStartElement("DUEDATEDETAIL");
                    tw.WriteElementString("DUEDATE", Date.ToDB(Convert.ToDateTime(drduedate["DueDate"])));
                    tw.WriteElementString("LedgerID", drduedate["LedgerID"].ToString());
                    tw.WriteElementString("VoucherType", "BANK_PMNT");
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
            // MessageBox.Show(strXML);
            return strXML;
        }

        //It Validates all the entry in the grid Only valid rows are count and validate
        private bool BPValidateGrid()
        {
            int[] LdrID = new int[20];
            decimal[] Amt = new decimal[20];

            //Validate input grid record
            for (int i = 0; i < grdBankPayment.Rows.Count - 1; i++)
            {
                try
                {
                    //if ledger ID repeats then message it
                    // if LedgerID is not present in between them
                    int tempValue = 0;
                    decimal tempDecValue = 0;
                    try
                    {
                        tempValue = System.Convert.ToInt32(grdBankPayment[i + 1, (int)GridColumn1.BPLedger_ID].Value);
                    }
                    catch (Exception ex)
                    {
                        tempValue = 0;
                    }
                    try
                    {
                        tempDecValue = System.Convert.ToDecimal(grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value);
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
                        if (i + 2 == grdBankPayment.Rows.Count && grdBankPayment[i + 1, (int)GridColumn1.BPParticular_Account_Head].Value.ToString() == "(NEW)")
                        {
                            //Do Nothing
                        }
                        else
                            return false;
                    }
                    else
                        LdrID[i] = tempValue;

                    if (i + 2 == grdBankPayment.Rows.Count && grdBankPayment[i + 1, (int)GridColumn1.BPParticular_Account_Head].Value.ToString() == "(NEW)")
                    {
                        //Donothing
                    }
                    else
                        Amt[i] = tempDecValue;

                    //liBankID = (ListItem)cmboBankAccount.SelectedItem;
                    int bankID = Convert.ToInt32(BPcmboBankAccount.SelectedValue);
                    if (LdrID.Contains(bankID))
                    {
                        MessageBox.Show("Same bank transaction not valid!");
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
        }

        private void BPbtnCancel_Click(object sender, EventArgs e)
        {
            BPChangeState(EntryMode.NORMAL);
        }

        private void BPcboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BPOptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    SeriesID = (ListItem)BPcboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));

                   

                    if (NumberingType == "AUTOMATIC" && !m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        BPtxtVchNo.Enabled = true;
                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            //disable all the controls except cboSeriesName

                            return;
                        }
                        BPlblVouNo.Visible = true;
                        BPtxtVchNo.Visible = true;
                        BPtxtVchNo.Text = m_vounum.ToString();
                        BPtxtVchNo.Enabled = false;
                    }
                    else if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        BPlblVouNo.Visible = false;
                        BPtxtVchNo.Visible = false;
                    }
                    if (m_BankPaymentID > 0 && !m_isRecurring)
                    {
                        BPlblVouNo.Visible = true;
                        BPtxtVchNo.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cboProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BPbtnDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = false;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(BPtxtDate.Text));
            frm.ShowDialog();
        }


        private void BPfrmBankPayment_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!BPContinueWithoutSaving())
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Check whether user wants to close the form without saving. Returns true if he wants to navigate away without saving.
        /// </summary>
        /// <returns></returns>
        private bool BPContinueWithoutSaving()
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

        private void BPbtn_groupvoucherposting_Click(object sender, EventArgs e)
        {
            BankPayment bpayment = new BankPayment();
            int rowid = bpayment.GetRowID(BPtxtVchNo.Text);
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
            for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++)
            {
                dtbulkvoucher.Rows.Add(grdBankPayment[i + 1, (int)GridColumn1.BPParticular_Account_Head].Value, "Debit", grdBankPayment[i + 1, (int)GridColumn1.BPAmount].Value, grdBankPayment[i + 1, (int)GridColumn1.BPLedger_ID].Value, "BANK_PMNT", BPtxtVchNo.Text, grdBankPayment[i + 1, (int)GridColumn1.BPRemarks].Value);
            }
            frmGroupVoucherList fgl = new frmGroupVoucherList(dtbulkvoucher, SID, ProjectID, rowid);
            fgl.ShowDialog();
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
        private void BPdueDate()
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

            for (int i = 1; i <= grdBankPayment.Rows.Count() - 2; i++)
            {
              
                tw.WriteElementString("LedgerID", grdBankPayment[i, (int)GridColumn1.BPLedger_ID].Value.ToString());

            }
            tw.WriteEndElement();
           
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
        private void BPOptionalFields()
        {
            SeriesID = (ListItem)BPcboSeriesName.SelectedItem;
            DataTable dtadditionalfield = Sales.GetAdditionalFields(SeriesID.ID);
            drdtadditionalfield = dtadditionalfield.Rows[0];
            NumberOfFields = Convert.ToInt32(drdtadditionalfield["NumberOfField"].ToString());
            if (NumberOfFields > 0)
            {
                if (NumberOfFields == 1)
                {
                    BPlblfirst.Visible = true;
                    BPtxtfirst.Visible = true;
                    BPlblsecond.Visible = false;
                    BPtxtsecond.Visible = false;
                    BPlblthird.Visible = false;
                    BPtxtthird.Visible = false;
                    BPlblfourth.Visible = false;
                    BPtxtfourth.Visible = false;
                    BPlblfifth.Visible = false;
                    BPtxtfifth.Visible = false;
                    BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                }
                else if (NumberOfFields == 2)
                {
                    BPlblfirst.Visible = true;
                    BPtxtfirst.Visible = true;
                    BPlblsecond.Visible = true;
                    BPtxtsecond.Visible = true;
                    BPlblthird.Visible = false;
                    BPtxtthird.Visible = false;
                    BPlblfourth.Visible = false;
                    BPtxtfourth.Visible = false;
                    BPlblfifth.Visible = false;
                    BPtxtfifth.Visible = false;
                    BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                }
                else if (NumberOfFields == 3)
                {
                    BPlblfirst.Visible = true;
                    BPtxtfirst.Visible = true;
                    BPlblsecond.Visible = true;
                    BPtxtsecond.Visible = true;
                    BPlblthird.Visible = true;
                    BPtxtthird.Visible = true;
                    BPlblfourth.Visible = false;
                    BPtxtfourth.Visible = false;
                    BPlblfifth.Visible = false;
                    BPtxtfifth.Visible = false;
                    BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    BPlblthird.Text = drdtadditionalfield["Field3"].ToString();

                }
                else if (NumberOfFields == 4)
                {
                    BPlblfirst.Visible = true;
                    BPtxtfirst.Visible = true;
                    BPlblsecond.Visible = true;
                    BPtxtsecond.Visible = true;
                    BPlblthird.Visible = true;
                    BPtxtthird.Visible = true;
                    BPlblfourth.Visible = true;
                    BPtxtfourth.Visible = true;
                    BPlblfifth.Visible = false;
                    BPtxtfifth.Visible = false;
                    BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    BPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                    BPlblfourth.Text = drdtadditionalfield["Field4"].ToString();

                }
                else if (NumberOfFields == 5)
                {
                    BPlblfirst.Visible = true;
                    BPtxtfirst.Visible = true;
                    BPlblsecond.Visible = true;
                    BPtxtsecond.Visible = true;
                    BPlblthird.Visible = true;
                    BPtxtthird.Visible = true;
                    BPlblfourth.Visible = true;
                    BPtxtfourth.Visible = true;
                    BPlblfifth.Visible = true;
                    BPtxtfifth.Visible = true;

                    BPlblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    BPlblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    BPlblthird.Text = drdtadditionalfield["Field3"].ToString();
                    BPlblfourth.Text = drdtadditionalfield["Field4"].ToString();
                    BPlblfifth.Text = drdtadditionalfield["Field5"].ToString();
                }
            }
            else
            {
                BPlblfirst.Visible = false;
                BPtxtfirst.Visible = false;
                BPlblsecond.Visible = false;
                BPtxtsecond.Visible = false;
                BPlblthird.Visible = false;
                BPtxtthird.Visible = false;
                BPlblfourth.Visible = false;
                BPtxtfourth.Visible = false;
                BPlblfifth.Visible = false;
                BPtxtfifth.Visible = false;
            }

        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {

            BPPrintPreviewCR(PrintType.CrystalReport);
        }
        private void BPPrintPreviewCR(PrintType myPrintType)
        {
            dsBankPayment.Clear();
            totalAmt = 0;
            rptBankPayment rpt = new rptBankPayment();
            //Fill the logo on the report
            Misc.WriteLogo(dsBankPayment, "tblImage");
            rpt.SetDataSource(dsBankPayment);

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



            bool empty = BPNavigation(Navigate.ID);
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
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "BANK_PAYMENT", m_dtRecurringSetting);
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
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "BANK_PAYMENT"); // delete from database
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
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "BANK_PAYMENT", m_dtRecurringSetting);
            fvr.ShowDialog();
        }

        string RSID = null, recurringVoucherID = null;
        public void CheckRecurringSetting(string VoucherID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            m_dtRecurringSetting = RecurringVoucher.GetRecurringVoucherSetting(VoucherID, "BANK_PAYMENT");
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
            //try
            //{
            //    if (!UserPermission.ChkUserPermission("BANK_PAYMENT_VIEW"))
            //    {
            //        Global.MsgError("Sorry! you dont have permission to View Bank Payment. Please contact your administrator for permission.");
            //        return;
            //    }
            //    string VoucherType = "BANK_PAYMENT";

            //    frmVoucherList fvl = new frmVoucherList(this, VoucherType);
            //    fvl.ShowDialog();

            //}
            //catch (Exception ex)
            //{
            //    Global.MsgError(ex.Message);
            //}

            try
            {
                if (!UserPermission.ChkUserPermission("BANK_PAYMENT_VIEW"))
                {
                    Global.MsgError("Sorry! you dont have permission to View Bank Payment. Please contact your administrator for permission.");
                    return;
                }
                string[] vouchValues = new string[5];
                vouchValues[0] = "BANK_PAYMENT";               // voucherType
                vouchValues[1] = "Acc.tblBankPaymentMaster";   // master tableName for the given voucher type  
                vouchValues[2] = "Acc.tblBankPaymentDetails";  // details tableName for the given voucher type
                vouchValues[3] = "BankPaymentID";              // master ID for the given master table
                vouchValues[4] = "BankPayment_Date";              // date field for a given voucher

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
            m_BankPaymentID = VoucherID;
            BPtxtBankID.Text = VoucherID.ToString();
            // frmBankPayment_Load(null, null);
            BPNavigation(Navigate.ID);
        } 
        #endregion

        #region method related to bank reconciliation closing

        public bool BPCheckIfBankReconciliationClosed()
        {
            try
            {
                bool res = false;
                //  int bankID = 0;
                if (BankReconciliation.IsBankReconciliationClosed(Convert.ToInt32(BPcmboBankAccount.SelectedValue), Date.ToDotNet(BPtxtDate.Text)))
                {
                    Global.MsgError("Bank Reconciliation is closed for this Bank, So you cannot add, edit or delete the vocher !");
                    return true;
                }
                for (int i = 1; i < grdBankPayment.Rows.Count; i++)
                {
                    int bankID = Convert.ToInt32(grdBankPayment[i, (int)GridColumn1.BPLedger_ID].Value);

                    res = BankReconciliation.IsBankReconciliationClosed(bankID, Date.ToDotNet(BPtxtDate.Text));
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
        public DataTable BPdtReference = new DataTable();
        public bool isSelected = false;
        public string ToDeleteRows = " ";
        public void BPAddReferenceColumns()
        {
            BPdtReference.Columns.Clear();
            BPdtReference.Columns.Add("LedgerID");
            BPdtReference.Columns.Add("VoucherType");
            BPdtReference.Columns.Add("RefName");
            BPdtReference.Columns.Add("RefID");
            BPdtReference.Columns.Add("IsAgainst");
        }
        public void GetAgainstReference(int refID, decimal amt, string crDr)
        {
            try
            {
                // remove the previously saved reference settings to add current settings
                foreach (DataRow dr in BPdtReference.Select("LedgerID = " + CurrAccLedgerID + " and IsAgainst ='true'"))
                {
                    if (dr["RVID"].ToString() != "0")
                        ToDeleteRows += dr["RVID"] + ",";
                    dr.Delete();
                }
                BPdtReference.AcceptChanges();

                DataRow dr1 = BPdtReference.NewRow();
                dr1["LedgerID"] = CurrAccLedgerID;
                dr1["VoucherType"] = "JRNL";
                dr1["RefName"] = null;
                dr1["RefID"] = refID;
                dr1["IsAgainst"] = "true";
                BPdtReference.Rows.Add(dr1);
                //if (crDr == "(Cr)")
                //    grdBankPayment[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Credit";

                //else
                //    grdBankPayment[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Debit";

                grdBankPayment[CurrRowPos, (int)GridColumn1.BPAmount].Value = amt.ToString();
                //grdBankPayment[CurrRowPos, (int)GridColumn.Ref_Amt].Value = amt.ToString() + crDr;

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
                foreach (DataRow dr in BPdtReference.Select("LedgerID = " + CurrAccLedgerID + " and IsAgainst ='false'"))
                {
                    if (dr["RVID"].ToString() != "0")
                        ToDeleteRows += dr["RVID"] + ",";
                    dr.Delete();
                }
                BPdtReference.AcceptChanges();

                DataRow dr1 = BPdtReference.NewRow();
                dr1["LedgerID"] = CurrAccLedgerID;
                dr1["VoucherType"] = "JRNL";
                dr1["RefName"] = refName;
                dr1["RefID"] = DBNull.Value;
                dr1["IsAgainst"] = "false";
                BPdtReference.Rows.Add(dr1);
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
                foreach (DataRow dr in BPdtReference.Select("LedgerID = " + CurrAccLedgerID + ""))
                {
                    if (dr["RVID"].ToString() != "0")
                        ToDeleteRows += dr["RVID"] + ",";
                    dr.Delete();
                }
                BPdtReference.AcceptChanges();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public bool BPCheckAmtAgainstRefAmt()
        {
            try
            {
                bool res = true;
                decimal Amt = Convert.ToDecimal(grdBankPayment[CurrRowPos, (int)GridColumn1.BPAmount].Value);
                // string crdr = grdBankPayment[CurrRowPos, (int)GridColumn.Dr_Cr].Value.ToString();
                string crdr ="Debit";

                string amtCrDr = grdBankPayment[CurrRowPos, (int)GridColumn1.BPRef_Amt].Value.ToString();
                decimal refAmt = Convert.ToDecimal(amtCrDr.Substring(0, amtCrDr.Length - 4));
                string ledgerName = grdBankPayment[CurrRowPos, (int)GridColumn1.BPParticular_Account_Head].Value.ToString();
                if ((refAmt > 0) && (refAmt < Amt) && (amtCrDr.Contains(crdr.Substring(0, crdr.Length - 4))))
                {
                    Global.MsgError("Your transaction amount for ledger " + ledgerName + " is " + Amt + " \n which is greater than the reference amount i.e. " + refAmt + " !");
                    grdBankPayment[CurrRowPos, (int)GridColumn1.BPAmount].Value = refAmt.ToString();
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

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void cmboBankAccount_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        


    }
}
