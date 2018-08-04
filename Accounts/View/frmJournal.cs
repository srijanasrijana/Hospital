using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Collections;
using Common;
using DateManager;
using ErrorManager;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using Accounts.Reports;
using CrystalDecisions.Shared;
using Accounts;
using BusinessLogic.Accounting;

namespace Accounts
{
    public partial class frmJournal : Form, IfrmAddAccClass, IfrmDateConverter, ILOVLedger, IDueDate, IVoucherRecurring, IVoucherList, IVoucherReference
    {
        DataTable dt1 = new DataTable();
        DataTable dtDueDateInfo = new DataTable();
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private string Prefix = "";
        private int OnlyReqdDetailRows = 0;
        private bool IsFieldChanged = false;
        private bool IsShortcutKey = false;
        private bool IsNegativeCash = false;
        ListItem liProjectID = new ListItem();
        private bool IsNegativeBank = false;
        ListItem SeriesID = new ListItem();
        private int loopCounter = 0;
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        ArrayList AccountClassID = new ArrayList();
        private Accounts.Model.dsJournal dsJournal = new Accounts.Model.dsJournal();
        List<int> AccClassID = new List<int>();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        double m_DebitAmount = 0;//Holds the total debit amount on the Voucher
        double m_CreditAmount = 0;
        int m_JournalID = 0;
        string totalRptAmount = "";
        decimal totalAmt = 0;
        private int JournalIDCopy = 0;
        //Check if the Account has been already set by LOVLedger
        bool hasChanged = false;
        private int CurrAccLedgerID = 0;
        private string CurrBal = " ";
        private string ActualBal = "0";
        private string LdgType = " ";
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
        private enum GridColumn : int
        {
            Del = 0, SNo, Particular_Account_Head, Dr_Cr, Amount, Current_Balance, Remarks, Ledger_ID, Current_Bal_Actual, Ref_Amt, Ledger_Type
        };

        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;
        //private string CheckDecimalPlace = "";

        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtcboDrCrSelectedIndexChanged = new SourceGrid.Cells.Controllers.CustomEvents();

        //For Export Menu
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";

        public frmJournal()
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
        public frmJournal(int JournalID, bool isRecurring, int RVID)
        {
            InitializeComponent();
            this.m_JournalID = JournalID;
            m_isRecurring = isRecurring;
            m_RVID = RVID;
        }

        public void DateConvert(DateTime DotNetDate)
        {
            txtDate.Text = Date.ToSystem(DotNetDate);
        }
        public void AddLedger(string LedgerName, int LedgerID, string CurrentBalance, bool IsSelected, string ActualBal, string LedgerType)
        {
            try
            {
                if (IsSelected)
                {
                    ctx.Text = LedgerName;
                    CurrAccLedgerID = LedgerID;
                    CurrBal = CurrentBalance;
                    this.ActualBal = ActualBal;
                    this.LdgType = LedgerType;
                }
                hasChanged = true;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
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
            }
        }

        //Customized header
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
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
                DataTable dtUserInfo = User.GetUserInfo(User.CurrUserID); //user id must be read from global i.e current user id
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
        private void LoadUseInfo(int UseID, TreeNode tn, int[] CheckedIDs, TreeView tvUse)
        {
            foreach (TreeNode nd in tn.Nodes)
            {
                nd.Checked = false; //first clear the checkmark if anything is checked previously
                LoadUseInfo(UseID, nd, CheckedIDs, tvUse);
                foreach (int id in CheckedIDs)
                    if (Convert.ToInt32(nd.Tag) == id)
                        nd.Checked = true;
            }
        }

        public frmJournal(int JournalID)
        {
            InitializeComponent();
            this.m_JournalID = JournalID;
        }

        public void AddAccClass()
        {
            try
            {
                //Clear the checked nodes of Treeview and relaoding the tree view
                //treeAccClass.Nodes.Clear();
                ShowAccClassInTreeView(treeAccClass, null, 0);
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
        private void frmJournal_Load(object sender, EventArgs e)
        {
            // initialize columns of reference table
            AddReferenceColumns();
            if (txtJournalID.Text != "")
                dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(txtJournalID.Text), "JRNL");

            chkDoNotClose.Checked = true;
            //DataTable dtcat= POSSalesInvoice.GetProductCategory();
            ChangeState(EntryMode.NEW);
            //ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);

            ShowAccClassInTreeView(treeAccClass, null, 0);
            m_mode = EntryMode.NEW;
            //CheckDecimalPlace = Settings.GetSettings("DEFAULT_DECIMALPLACES");
            ////Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.ToSystem(Date.GetServerDate()); //By default show the current date from the sqlserver.
            //For Loading The Optional Fields
            OptionalFields();
            try
            {
                #region Customevents mainly for saving purpose
                cboSeriesName.SelectedIndexChanged += new EventHandler(Text_Change);
                txtVchNo.TextChanged += new EventHandler(Text_Change);
                txtDate.TextChanged += new EventHandler(Text_Change);
                btnDate.Click += new EventHandler(Text_Change);
                cboProjectName.SelectedIndexChanged += new EventHandler(Text_Change);
                txtRemarks.TextChanged += new EventHandler(Text_Change);


                //Event trigerred when delete button is clicked
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                evtAmountFocusLost.FocusLeft += new EventHandler(Amount_Focus_Lost);

                evtAccountFocusLost.FocusLeft += new EventHandler(evtAccountFocusLost_FocusLeft);
                // evtcboDrCrSelectedIndexChanged.ValueChanged += new EventHandler(evtDrCr_Changed);
                evtcboDrCrSelectedIndexChanged.FocusLeft += new EventHandler(evtDrCr_Changed);

                #endregion

                grdJournal.Redim(2, Enum.GetValues(typeof(GridColumn)).Length);
                btnRowDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;

                //Prepare the header part for grid
                AddGridHeader();
                AddRowJournal(1);

                //Replace this block with navigation function later after completion of journal code

                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID
                if (m_JournalID > 0)
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
                            vouchID = m_JournalID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }
                        Journal m_Journal = new Journal();
                        //Getting the value of SeriesID via MasterID or VouchID
                        int SeriesIDD = m_Journal.GetSeriesIDFromMasterID(vouchID);
                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Journal");
                            cboSeriesName.Text = "";
                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            cboSeriesName.Text = dr["EngName"].ToString();
                        }
                        DataTable dtJournalMasterInfo = m_Journal.GetJournalMasterDtl(vouchID.ToString());
                        if (dtJournalMasterInfo.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }
                        //Write the corresponding textboxes
                        DataRow drJournalMasterInfo = dtJournalMasterInfo.Rows[0];

                        if (!m_isRecurring)
                        {
                            txtVchNo.Text = drJournalMasterInfo["Voucher_No"].ToString();
                            txtDate.Text = Date.DBToSystem(drJournalMasterInfo["Journal_Date"].ToString());
                            txtJournalID.Text = drJournalMasterInfo["JournalID"].ToString();
                        }
                        else
                        {
                            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if recurring load today's date
                            //txtduedate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                        }
                        txtRemarks.Text = drJournalMasterInfo["Remarks"].ToString();

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

                                txtfirst.Text = drJournalMasterInfo["Field1"].ToString();
                                txtsecond.Text = drJournalMasterInfo["Field2"].ToString();
                                txtthird.Text = drJournalMasterInfo["Field3"].ToString();
                                txtfourth.Text = drJournalMasterInfo["Field4"].ToString();
                                txtfifth.Text = drJournalMasterInfo["Field5"].ToString();
                            }
                            else if (NumberOfFields == 2)
                            {
                                lblfirst.Visible = true;
                                txtfirst.Visible = true;

                                txtsecond.Visible = true;
                                lblthird.Visible = false;
                                txtthird.Visible = false;
                                lblfourth.Visible = false;
                                txtfourth.Visible = false;
                                lblfifth.Visible = false;
                                txtfifth.Visible = false;
                                lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                                lblsecond.Text = drdtadditionalfield["Field2"].ToString();

                                txtfirst.Text = drJournalMasterInfo["Field1"].ToString();
                                txtsecond.Text = drJournalMasterInfo["Field2"].ToString();
                                txtthird.Text = drJournalMasterInfo["Field3"].ToString();
                                txtfourth.Text = drJournalMasterInfo["Field4"].ToString();
                                txtfifth.Text = drJournalMasterInfo["Field5"].ToString();
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

                                txtfirst.Text = drJournalMasterInfo["Field1"].ToString();
                                txtsecond.Text = drJournalMasterInfo["Field2"].ToString();
                                txtthird.Text = drJournalMasterInfo["Field3"].ToString();
                                txtfourth.Text = drJournalMasterInfo["Field4"].ToString();
                                txtfifth.Text = drJournalMasterInfo["Field5"].ToString();

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

                                txtfirst.Text = drJournalMasterInfo["Field1"].ToString();
                                txtsecond.Text = drJournalMasterInfo["Field2"].ToString();
                                txtthird.Text = drJournalMasterInfo["Field3"].ToString();
                                txtfourth.Text = drJournalMasterInfo["Field4"].ToString();
                                txtfifth.Text = drJournalMasterInfo["Field5"].ToString();

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

                                txtfirst.Text = drJournalMasterInfo["Field1"].ToString();
                                txtsecond.Text = drJournalMasterInfo["Field2"].ToString();
                                txtthird.Text = drJournalMasterInfo["Field3"].ToString();
                                txtfourth.Text = drJournalMasterInfo["Field4"].ToString();
                                txtfifth.Text = drJournalMasterInfo["Field5"].ToString();
                            }
                        }
                        dsJournal.Tables["tblJournalMaster"].Rows.Add(cboSeriesName.Text, drJournalMasterInfo["Voucher_No"].ToString(), Date.DBToSystem(drJournalMasterInfo["Journal_Date"].ToString()), drJournalMasterInfo["Remarks"].ToString());
                        DataTable dtJournalDetail = m_Journal.GetJournalDetail(Convert.ToInt32(drJournalMasterInfo["JournalID"]));
                        for (int i = 1; i <= dtJournalDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtJournalDetail.Rows[i - 1];
                            grdJournal[i, (int)GridColumn.SNo].Value = i.ToString();
                            grdJournal[i, (int)GridColumn.Particular_Account_Head].Value = drDetail["LedgerName"].ToString();
                            grdJournal[i, (int)GridColumn.Dr_Cr].Value = drDetail["DrCr"].ToString();
                            grdJournal[i, (int)GridColumn.Amount].Value = Convert.ToDouble(drDetail["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            // old code 
                            //    grdJournal[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                            //    grdJournal[i, (int)GridColumn.Ledger_ID].Value = drDetail["LedgerID"].ToString();
                            //    AddRowJournal(grdJournal.RowsCount);
                            //    dsJournal.Tables["tblJournalDetails"].Rows.Add(drDetail["LedgerName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                            //}

                            // new code starts here to load current balance for recurring voucher posting and for when the journal is opened from DayBook
                            ///////////////////////You have to specify accounting class here *************************************
                            ///Get the Ledger Detail Info by sending LedgerID
                            ///then place LedgerID and Current balance in appropriate field
                            //DataTable dtLdrInfo = Ledger.GetLedgerDetail("1", null, null, Convert.ToInt32(drDetail["LedgerID"]));
                            DataTable dtLdrInfo = Ledger.GetLedgerDetails1("<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>", null, null, Convert.ToInt32(drDetail["LedgerID"]), 0, null);

                            if (dtLdrInfo.Rows.Count != 1)
                            {
                                grdJournal[i, (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdJournal[i, (int)GridColumn.Current_Bal_Actual].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdJournal[i, (int)GridColumn.Ledger_Type].Value = " ";

                            }
                            else
                            {
                                DataRow drLdrInfo = dtLdrInfo.Rows[0];

                                double Amount = 0;
                                string drcr = " ";
                                // double Amount = (Convert.ToDouble(drLdrInfo["DebitTotal"]) + Convert.ToDouble(drLdrInfo["OpenBalDr"])) - (Convert.ToDouble(drLdrInfo["CreditTotal"]) + Convert.ToDouble(drLdrInfo["OpenBalCr"]));

                                if (drLdrInfo["DrCr"].ToString() == "DEBIT")//if current balance is debit
                                {
                                    grdJournal[i, (int)GridColumn.Current_Balance].Value = Convert.ToDouble(drLdrInfo["FinalBal"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                    if (drLdrInfo["LedgerType"].ToString() == "DR")//if ledger type is debit
                                    {
                                        Amount = Convert.ToDouble(drLdrInfo["FinalBal"]);
                                        if (drDetail["DrCr"].ToString() == "Debit")//if grid amount is debit, also ledger type is debit so we need to substract grid amount to get actual balance before this transaction
                                        {
                                            Amount = Amount - Convert.ToDouble(drDetail["Amount"]);
                                        }
                                        else
                                        {
                                            Amount = Amount + Convert.ToDouble(drDetail["Amount"]);
                                        }
                                        //find out wheather the balance is dr or cr
                                        //if (Amount >= 0)
                                        //{
                                        //    drcr = "(Dr.)";
                                        //}
                                        //else
                                        //{
                                        //    drcr = "(Cr.)";
                                        //}


                                    }
                                    else//if ledger type is credit but has balance of type debit
                                    {
                                        Amount = 0 - Convert.ToDouble(drLdrInfo["FinalBal"]);
                                        if (drDetail["DrCr"].ToString() == "Debit")//if grid amount is debit
                                        {
                                            Amount = Amount + Convert.ToDouble(drDetail["Amount"]);
                                        }
                                        else
                                        {
                                            Amount = Amount - Convert.ToDouble(drDetail["Amount"]);
                                        }

                                        //if (Amount >= 0)
                                        //{
                                        //    drcr = "(Cr.)";
                                        //}
                                        //else
                                        //{
                                        //    drcr = "(Dr.)";
                                        //}
                                    }


                                }
                                else
                                {
                                    grdJournal[i, (int)GridColumn.Current_Balance].Value = Convert.ToDouble(drLdrInfo["FinalBal"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                    if (drLdrInfo["LedgerType"].ToString() == "DR")//if ledger type is debit
                                    {
                                        Amount = 0 - Convert.ToDouble(drLdrInfo["FinalBal"]);
                                        if (drDetail["DrCr"].ToString() == "Debit")//if grid amount is debit also ledger type is debit so we need to substract to get actual balance before this transaction
                                        {
                                            Amount = Amount - Convert.ToDouble(drDetail["Amount"]);
                                        }
                                        else
                                        {
                                            Amount = Amount + Convert.ToDouble(drDetail["Amount"]);
                                        }

                                        //if (Amount >= 0)
                                        //{
                                        //    drcr = "(Dr.)";
                                        //}
                                        //else
                                        //{
                                        //    drcr = "(Cr.)";
                                        //}

                                    }

                                    else//if ledger type is credit n has balance of type credit
                                    {
                                        Amount = Convert.ToDouble(drLdrInfo["FinalBal"]);
                                        if (drDetail["DrCr"].ToString() == "Debit")//if grid amount is debit
                                        {
                                            Amount = Amount + Convert.ToDouble(drDetail["Amount"]);
                                        }
                                        else
                                        {
                                            Amount = Amount - Convert.ToDouble(drDetail["Amount"]);
                                        }

                                        //if (Amount >= 0)
                                        //{
                                        //    drcr = "(Cr.)";
                                        //}
                                        //else
                                        //{
                                        //    drcr = "(Dr.)";
                                        //}
                                    }

                                }
                                grdJournal[i, 8].Value = Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                grdJournal[i, (int)GridColumn.Ledger_Type].Value = drLdrInfo["LedgerType"].ToString();


                                //if (drLdrInfo["Debit"] == DBNull.Value || Convert.ToInt32(drLdrInfo["Debit"]) == 0)
                                //{
                                //    if (drLdrInfo["Credit"] == DBNull.Value || Convert.ToInt32(drLdrInfo["Credit"]) == 0)
                                //    {
                                //        grdJournal[i, (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                //        grdJournal[i, 8].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                //    }
                                //    else
                                //    {
                                //        grdJournal[i, (int)GridColumn.Current_Balance].Value = Convert.ToDecimal(drLdrInfo["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                //        grdJournal[i, 8].Value = Convert.ToDecimal(drLdrInfo["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                //    }
                                //}
                                //else
                                //{
                                //    if (drLdrInfo["Credit"] == DBNull.Value || Convert.ToInt32(drLdrInfo["Credit"]) == 0)
                                //    {
                                //        grdJournal[i, (int)GridColumn.Current_Balance].Value = Convert.ToDecimal(drLdrInfo["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                //        grdJournal[i, 8].Value = Convert.ToDecimal(drLdrInfo["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                //    }
                                //    else
                                //    {
                                //        if (Convert.ToDecimal(drLdrInfo["Debit"]) == Convert.ToDecimal(drLdrInfo["Credit"]))
                                //        {
                                //            grdJournal[i, (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                //            grdJournal[i, 8].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                //        }

                                //        if (Convert.ToDecimal(drLdrInfo["Debit"]) > Convert.ToDecimal(drLdrInfo["Credit"]))
                                //        {
                                //            grdJournal[i, (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(drLdrInfo["Debit"]) - Convert.ToDecimal(drLdrInfo["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                //            grdJournal[i, 8].Value = (Convert.ToDecimal(drLdrInfo["Debit"]) - Convert.ToDecimal(drLdrInfo["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                                //        }
                                //        if (Convert.ToDecimal(drLdrInfo["Debit"]) < Convert.ToDecimal(drLdrInfo["Credit"]))
                                //        {
                                //            grdJournal[i, (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(drLdrInfo["Credit"]) - Convert.ToDecimal(drLdrInfo["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                //            grdJournal[i, 8].Value = (Convert.ToDecimal(drLdrInfo["Credit"]) - Convert.ToDecimal(drLdrInfo["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                                //        }
                                //    }
                                //}
                            }
                            // grdJournal[0, 5] = Current Balance  *****ABOVE CODE
                            grdJournal[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                            grdJournal[i, (int)GridColumn.Ledger_ID].Value = drDetail["LedgerID"].ToString();
                            AddRowJournal(grdJournal.RowsCount);

                            //This is for crystal report
                            if (drDetail["DrCr"].ToString() == "Debit")
                            {
                                dsJournal.Tables["tblJournalDetails"].Rows.Add(drDetail["LedgerName"].ToString(), null, null, Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), null, drDetail["Remarks"].ToString());
                                totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                            }
                            else
                            {
                                dsJournal.Tables["tblJournalDetails"].Rows.Add(drDetail["LedgerName"].ToString(), null, null, null, Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                            }
                            totalRptAmount = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();
                        }
                        // end of new code 
                        //Calculate the Debit and Credit totals
                        CalculateDrCr();

                        // if recurring is true then donot load recurring settings for new voucher
                        if (!m_isRecurring)
                            CheckRecurringSetting(txtJournalID.Text);

                    }
                #endregion

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
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
                if (MessageBox.Show("Changes have not been saved yet. Are you sure you want to continue without saving?", "Exiting...", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
        /// <summary>
        /// This is the common text change event for all controls just to track if any changes has been made. This will help to show a save dialogue box while closing the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Text_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
                int CurRow = grdJournal.Selection.GetSelectionRegion().GetRowsIndex()[0];
                //Do not delete if its the last Row because it contains (NEW)
                if (ctx.Position.Row <= grdJournal.RowsCount - 2)
                {
                    #region Reference related
                    if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(txtJournalID.Text), Convert.ToInt32(grdJournal[CurRow, (int)GridColumn.Ledger_ID].Value), "JRNL"))
                    {
                        Global.MsgError("You must delete all other vouchers with reference against this voucher to delete this transaction!");
                        return;
                    }

                    #endregion

                    grdJournal.Rows.Remove(ctx.Position.Row);
                }
                CalculateDrCr();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        bool isNewReferenceVoucher = false, isAgainstRef = false;
        private void Account_Focused(object sender, EventArgs e)
        {
            try
            {
                if (!hasChanged)
                {
                    ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                    int CurRow = grdJournal.Selection.GetSelectionRegion().GetRowsIndex()[0];

                    #region Reference related
                    // if new voucher reference is created for this, donot open the reference form 
                    if (m_mode == EntryMode.EDIT && VoucherReference.IsNewReferenceVoucher(Convert.ToInt32(txtJournalID.Text), Convert.ToInt32(grdJournal[CurRow, (int)GridColumn.Ledger_ID].Value), "JRNL"))
                    {
                        //Global.MsgError("You must delete all other vouchers with reference against this voucher to delete this transaction!");
                        isNewReferenceVoucher = true;
                        return;
                    }
                    isNewReferenceVoucher = false;

                    #endregion

                    //incase user does not select any ledger
                    CurrAccLedgerID = Convert.ToInt32(grdJournal[CurRow, (int)GridColumn.Ledger_ID].Value);
                    if (grdJournal[CurRow, (int)GridColumn.Current_Balance].Value == null || grdJournal[CurRow, (int)GridColumn.Current_Balance].Value == "" || grdJournal[CurRow, (int)GridColumn.Current_Balance].Value == " ")
                        CurrBal = "0";
                    else
                        CurrBal = Convert.ToString(grdJournal[CurRow, (int)GridColumn.Current_Balance].Value);

                    if (grdJournal[CurRow, (int)GridColumn.Current_Bal_Actual].Value == null || grdJournal[CurRow, (int)GridColumn.Current_Bal_Actual].Value == "" || grdJournal[CurRow, (int)GridColumn.Current_Bal_Actual].Value == " ")
                        ActualBal = "0";
                    else
                        ActualBal = Convert.ToString(grdJournal[CurRow, (int)GridColumn.Current_Bal_Actual].Value);

                    LdgType = Convert.ToString(grdJournal[CurRow, (int)GridColumn.Ledger_Type].Value);

                    frmLOVLedger frm = new frmLOVLedger(this);
                    frm.ShowDialog();
                    SendKeys.Send("{Tab}");

                }
                //SendKeys.Send("{Tab}");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void CrDr_Changed(object sender, EventArgs e)
        {
            int RowCount = grdJournal.RowsCount;
            int CurRow = grdJournal.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //amt = Convert.ToDecimal(grdJournal[CurRow, (int)GridColumn.Amount].Value);
            amt = 0;
            CurrBal = Convert.ToString(grdJournal[CurRow, (int)GridColumn.Current_Bal_Actual].Value);
            FillGridRowExceptLedgerID(CurrRowPos);//, CurrAccLedgerID, CurrBal, amt);
            SendKeys.Send("{Tab}");
        }
        private void Account_Leave(object sender, EventArgs e)
        {
            hasChanged = false;

        }
        decimal amt = 0;
        bool amt_Focused_First = true;
        /// <summary>
        /// used to store current value of Amount before edting it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Amount_Focused(object sender, EventArgs e)
        {
            try
            {
                if (amt_Focused_First)
                {
                    int RowCount = grdJournal.RowsCount;
                    int CurRow = grdJournal.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    amt = Convert.ToDecimal(grdJournal[CurRow, (int)GridColumn.Amount].Value);
                    amt_Focused_First = false;


                }
                else
                {
                    //SendKeys.Send("{Tab}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                //SendKeys.Send("{Tab}");
                return;
            }
        }
        private void evtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            int ledID = 0, vouchID = 0;
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            int ledgerID = 0;
            string CurrentBal = "";
            try
            {
                if (grdJournal[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)" || grdJournal[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString() == "")
                    return;

                grdJournal[CurrRowPos, (int)GridColumn.Ledger_ID].Value = CurrAccLedgerID.ToString();
                grdJournal[CurrRowPos, (int)GridColumn.Current_Balance].Value = CurrBal.ToString();

                grdJournal[CurrRowPos, (int)GridColumn.Current_Bal_Actual].Value = ActualBal.ToString();
                grdJournal[CurrRowPos, (int)GridColumn.Ledger_Type].Value = LdgType.ToString();

                CurrAccLedgerID = 0;
                CurrBal = " ";
                ActualBal = " ";
                LdgType = " ";


                //try
                //{
                //    ledgerID = Convert.ToInt32(grdJournal[CurrRowPos, (int)GridColumn.Ledger_ID].Value);
                //    CurrentBal = grdJournal[CurrRowPos, (int)GridColumn.Current_Bal_Actual].Value.ToString();
                //}
                //catch
                //{
                //    ledgerID = 0;
                //}
                //if (ledgerID != 0 && CurrAccLedgerID == 0)
                //{
                //    CurrAccLedgerID = ledgerID;
                //    CurrBal = CurrentBal;
                //}
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

                if (txtJournalID.Text != "")
                {
                    vouchID = Convert.ToInt32(txtJournalID.Text);
                }

                frmReference fr = new frmReference(this, vouchID, "JRNL", ledID);
                fr.ShowDialog();
                //SendKeys.Send("{Tab}");
            }
            #endregion

            //FillAllGridRow(CurrRowPos, CurrAccLedgerID, CurrBal);

        }
        private void evtDrCr_Changed(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;

            if (grdJournal[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)")
                return;
            // string strAmountLength = grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Amount].Value.ToString();
            if (grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Amount].Value == "" || grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Amount].Value == " ")
                grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Amount].Value = 0;
            #region Budget Check
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
            if (grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
            {
                check = Budget.CheckBudget(Convert.ToInt32(grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Ledger_ID].Value), Convert.ToDecimal(grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Amount].Value), strXML);
            }
            else
            {
                check = Budget.CheckBudget(Convert.ToInt32(grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Ledger_ID].Value), (0 - Convert.ToDecimal(grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Amount].Value)), strXML);

            }
            if (!check)
            {
                grdJournal[Convert.ToInt32(CurrRowPos), (int)GridColumn.Amount].Value = 0;
            }

            //// check if transaction amount is greater than reference amount
            //if (!CheckAmtAgainstRefAmt())
            //{
            //    return;
            //}

            #endregion
            CurrAccLedgerID = Convert.ToInt32(grdJournal[CurrRowPos, (int)GridColumn.Ledger_ID].Value);
            CalculateDrCr();

            CurrBal = Convert.ToString(grdJournal[CurrRowPos, (int)GridColumn.Current_Bal_Actual].Value);
            if (CurrRowPos > 0)
            {
                #region recommend amount
                decimal debTotal = Convert.ToDecimal(lblDebitTotal.Text.Trim());
                decimal creTotal = Convert.ToDecimal(lblCreditTotal.Text.Trim());
                if (grdJournal[CurrRowPos, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
                {
                    debTotal = debTotal - Convert.ToDecimal(grdJournal[CurrRowPos, (int)GridColumn.Amount].Value);
                    if (debTotal < creTotal)
                    {
                        grdJournal[CurrRowPos, (int)GridColumn.Amount].Value = creTotal - debTotal;
                        lblDebitTotal.Text = creTotal.ToString();
                        lblDifferenceAmount.Text = "0";
                    }
                    else
                    {
                        grdJournal[CurrRowPos, (int)GridColumn.Amount].Value = 0;
                        lblDebitTotal.Text = debTotal.ToString();
                        lblDifferenceAmount.Text = (debTotal - creTotal).ToString();

                    }

                }
                else
                {
                    creTotal = creTotal - Convert.ToDecimal(grdJournal[CurrRowPos, (int)GridColumn.Amount].Value);
                    if (creTotal < debTotal)
                    {
                        grdJournal[CurrRowPos, (int)GridColumn.Amount].Value = debTotal - creTotal;
                        lblCreditTotal.Text = debTotal.ToString();
                        lblDifferenceAmount.Text = "0";
                    }
                    else
                    {
                        grdJournal[CurrRowPos, (int)GridColumn.Amount].Value = 0;
                        lblCreditTotal.Text = creTotal.ToString();
                        lblDifferenceAmount.Text = (creTotal - debTotal).ToString();
                    }
                }
                #endregion
                //grdJournal[CurrRowPos, 4].Value = grdJournal[CurrRowPos-1, 4].Value;
                //  grdJournal[CurrRowPos, (int)GridColumn.Amount].Value = lblDifferenceAmount.Text;
            }
            //amt = 0;
            FillGridRowExceptLedgerID(CurrRowPos);//, CurrAccLedgerID, CurrBal, amt);



        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.N)
            {
                txtRemarks.Focus();
            }
            else
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                frmLOVLedger frm = new frmLOVLedger(this, e);
                frm.ShowDialog();
            }
        }

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {

            int RowCount = grdJournal.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            CurrRowPos = ctx.Position.Row;
            string AccName = (string)(grdJournal[RowCount - 1, 2].Value);

            if (grdJournal[CurRow, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)" || 
                grdJournal[CurRow, (int)GridColumn.Particular_Account_Head].Value.ToString() == "")
                return;

            CurrBal = (string)(grdJournal[CurRow, (int)GridColumn.Current_Balance].Value); // updated current balance is sent for calculation of new current balance

            //Check if the input value is correct
            if (grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == "" || 
                grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == null)
            {
                grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            }
            if (!Misc.IsNumeric(grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value))
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
            if (grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
            {
                check = Budget.CheckBudget(Convert.ToInt32(grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Ledger_ID].Value), Convert.ToDecimal(grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value), strXML);
            }
            else
            {
                check = Budget.CheckBudget(Convert.ToInt32(grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Ledger_ID].Value), (0 - Convert.ToDecimal(grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value)), strXML);

            }
            if (!check)
            {
                grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = 0;
            }

            //#region Reference related
            //if (m_mode == EntryMode.EDIT)
            //{
            //    grdJournal[CurRow, (int)GridColumn.Ref_Amt].Value = VoucherReference.GetAmtForAgainstRef(Convert.ToInt32(txtJournalID.Text), "JRNL", Convert.ToInt32(grdJournal[CurRow, (int)GridColumn.Ledger_ID].Value));
            //}

            //// check if transaction amount is greater than reference amount
            //if (!CheckAmtAgainstRefAmt())
            //{
            //    return;
            //} 
            //#endregion

            FillGridRowExceptLedgerID(CurRow);//, CurrAccLedgerID, CurrBal, amt);
            double checkformat = Convert.ToDouble(grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value.ToString());
            //string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdJournal[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = checkformat;

            CalculateDrCr();
            if (AccName != "(NEW)")
            {
                AddRowJournal(RowCount);
            }
        }

        //Calculate amount fill on all column on Account focus lost event
        private void FillAllGridRow(int RowPosition, int LdrID, string CurrBalance)
        {
            decimal temporary = 0;
            decimal TempAmount = 0;
            string CurrentLedgerBalance = CurrBalance.ToString(); // grdJournal[Convert.ToInt32(RowPosition), 8].ToString();
            string DrCr = grdJournal[Convert.ToInt32(RowPosition), 3].Value.ToString();
            string[] CurrentBalance = CurrBalance.ToString().Split('(');

            try
            {
                TempAmount = Convert.ToDecimal(grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Amount].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                if (DrCr == "Debit")
                {
                    grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
                if (DrCr == "Credit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0]) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                    if (temporary > 0)
                    {
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                if (DrCr == "Credit")
                {
                    grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0].ToString()) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                    if (temporary > 0)
                    {
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                if (DrCr == "Credit")
                {
                    grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
            }
            else
            {
                grdJournal[RowPosition, (int)GridColumn.Current_Balance].Value = CurrBalance;
            }
            grdJournal[RowPosition, (int)GridColumn.Ledger_ID].Value = LdrID;
            grdJournal[RowPosition, (int)GridColumn.Current_Bal_Actual].Value = CurrBalance;

            CurrAccLedgerID = 0;
            CurrBal = "";
        }

        //Calculate and onlu fill or change value of current balance
        private void FillGridRowExceptLedgerID(int RowPosition)//, int LdrID, string CurrBal, decimal amt)
        {
            try
            {
                decimal actualbal = Convert.ToDecimal(grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Bal_Actual].ToString());
                decimal amount = Convert.ToDecimal(grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Amount].ToString());
                string ldgtype = grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Ledger_Type].ToString();
                string AmountDrCr = grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Dr_Cr].Value.ToString();

                decimal UpdatedAmount = 0;

                if (ldgtype == "DR")
                {
                    if (AmountDrCr == "Debit")
                    {
                        UpdatedAmount = actualbal + amount;
                    }
                    else if (AmountDrCr == "Credit")
                    {
                        UpdatedAmount = actualbal - amount;
                    }
                    if (UpdatedAmount >= 0)
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = UpdatedAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    else
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = Math.Abs(UpdatedAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";

                    UpdatedAmount = 0;

                }

                else if (ldgtype == "CR")
                {
                    if (AmountDrCr == "Debit")
                    {
                        UpdatedAmount = actualbal - amount;
                    }
                    else if (AmountDrCr == "Credit")
                    {
                        UpdatedAmount = actualbal + amount;
                    }
                    if (UpdatedAmount >= 0)
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = UpdatedAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    else
                        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = Math.Abs(UpdatedAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";

                    UpdatedAmount = 0;

                }

            }
            catch (Exception ex)
            {

                Global.MsgError(ex.Message);
            }
            //amt_Focused_First = true;
            //decimal temporary = 0;
            //decimal TempAmount = 0;
            //string CurrentLedgerBalance = "";
            //string[] CurrentBalance = new string[2];
            //if (grdJournal[RowPosition, 2].Value.ToString() == "(NEW)")
            //    return;
            //if (CurrBal == "")
            //{
            //    CurrentLedgerBalance = grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Bal_Actual].ToString();
            //    CurrentBalance = grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Bal_Actual].ToString().Split('(');
            //}
            //else
            //{
            //    CurrentLedgerBalance = CurrBal.ToString();
            //    CurrentBalance = CurrBal.ToString().Split('(');
            //}

            //string DrCr = grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Dr_Cr].Value.ToString();
            //try
            //{
            //    TempAmount = Convert.ToDecimal(grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Amount].Value);
            //}
            //catch
            //{
            //    TempAmount = 0;
            //}

            //if (CurrentLedgerBalance.Contains("Dr"))
            //{
            //    if (DrCr == "Debit")
            //    {
            //        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount) - amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            //    }
            //    if (DrCr == "Credit")
            //    {
            //        temporary = (Convert.ToDecimal(CurrentBalance[0]) + (-1) * Convert.ToDecimal(TempAmount) + amt);
            //        if (temporary == 0)
            //        {
            //            grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //        }
            //        if (temporary < 0)
            //        {
            //            grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            //        }
            //        if (temporary > 0)
            //        {
            //            grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            //        }
            //    }
            //}

            //else if (CurrentLedgerBalance.Contains("Cr"))
            //{
            //    if (DrCr == "Credit")
            //    {
            //        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount) - amt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            //    }
            //    if (DrCr == "Debit")
            //    {
            //        temporary = (Convert.ToDecimal(CurrentBalance[0].ToString()) + (-1) * Convert.ToDecimal(TempAmount) + amt);
            //        if (temporary == 0)
            //        {
            //            grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //        }
            //        if (temporary < 0)
            //        {
            //            grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            //        }
            //        if (temporary > 0)
            //        {
            //            grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            //        }
            //    }
            //}

            //else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            //{
            //    if (DrCr == "Credit")
            //    {
            //        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            //    }
            //    if (DrCr == "Debit")
            //    {
            //        grdJournal[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            //    }
            //}
            //else
            //{
            //    grdJournal[RowPosition, (int)GridColumn.Current_Balance].Value = CurrBal;
            //}
        }

        /// <summary>
        /// Sums up all the debit and credit amounts on the voucher grid
        /// </summary>
        private void CalculateDrCr()
        {
            try
            {
                double dr, cr;
                dr = cr = 0;
                for (int i = 1; i < grdJournal.RowsCount; i++)
                {
                    //Check for empty Amount
                    object objAmount = grdJournal[i, (int)GridColumn.Amount].Value;
                    if (objAmount == null)
                        continue;
                    ////Check if it is the (NEW) row. If it is, it must be the last row.
                    if ((i == grdJournal.Rows.Count) || (grdJournal[i, (int)GridColumn.Amount].Value.ToString().ToUpper() == "(NEW)"))
                        continue;
                    double m_Amount = 0;

                    string m_Value = Convert.ToString(grdJournal[i, (int)GridColumn.Amount].Value);//Had to do this because it showed error when the cell was left blank
                    if (m_Value.Length == 0)
                        m_Amount = 0;
                    else
                    {
                        Double.TryParse(grdJournal[i, (int)GridColumn.Amount].Value.ToString(), out m_Amount);
                        try
                        {
                            Convert.ToDouble(grdJournal[i, (int)GridColumn.Amount].Value);
                        }
                        catch
                        {
                            MessageBox.Show("Please enter valid amount!");
                        }
                    }

                    if (grdJournal[i, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
                        dr += m_Amount;
                    else if (grdJournal[i, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                        cr += m_Amount;
                }
                m_DebitAmount = dr;
                m_CreditAmount = cr;

                //if(CheckDecimalPlace=="2")
                //{
                lblDebitTotal.Text = m_DebitAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblCreditTotal.Text = m_CreditAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblDifferenceAmount.Text = (Math.Abs(m_DebitAmount - m_CreditAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            }
            catch (Exception ex)
            {
                lblDebitTotal.Text = "-";
                lblCreditTotal.Text = "-";
                lblDifferenceAmount.Text = "-";
                //Just ignore any errors
            }
        }

        /// <summary>
        /// Adds the row in the Journal field
        /// </summary>
        private void AddRowJournal(int RowCount)
        {

            SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
            //Add a new row
            grdJournal.Redim(Convert.ToInt32(RowCount + 1), grdJournal.ColumnsCount);

            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            //Delete Row
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;
            grdJournal[i, (int)GridColumn.Del] = btnDelete;
            grdJournal[i, (int)GridColumn.Del].AddController(evtDelete);
            grdJournal[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdJournal[i, (int)GridColumn.Particular_Account_Head] = new SourceGrid.Cells.Cell("", txtAccount);
            txtAccount.Control.GotFocus += new EventHandler(Account_Focused);
            txtAccount.Control.LostFocus += new EventHandler(Account_Leave);
            txtAccount.Control.KeyDown += new KeyEventHandler(Account_KeyDown);
            txtAccount.Control.TextChanged += new EventHandler(Text_Change);
            grdJournal[i, (int)GridColumn.Particular_Account_Head].AddController(evtAccountFocusLost);
            grdJournal[i, (int)GridColumn.Particular_Account_Head].Value = "(NEW)";

            SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
            cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;
            //cboDrCr.Control.SelectedIndexChanged += new EventHandler(CrDr_Changed);
            string strDrCr = "Debit";
            //if (grdJournal[i - 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
            //    strDrCr = "Credit";
            //this is for auto debit and credit type
            if (i > 2)
            {
                if ((m_DebitAmount - m_CreditAmount) > 0)
                {
                    strDrCr = "Credit";
                }
                if ((m_DebitAmount - m_CreditAmount) < 0)
                {
                    strDrCr = "Debit";
                }
            }
            grdJournal[i, (int)GridColumn.Dr_Cr] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);
            cboDrCr.Control.SelectedIndexChanged += new EventHandler(Text_Change);
            grdJournal[i, (int)GridColumn.Dr_Cr].AddController(evtcboDrCrSelectedIndexChanged);

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdJournal[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell("", txtAmount);
            txtAmount.Control.TextChanged += new EventHandler(Text_Change);
            grdJournal[i, (int)GridColumn.Amount].AddController(evtAmountFocusLost);
            txtAmount.Control.GotFocus += new EventHandler(Amount_Focused);
            SourceGrid.Cells.Editors.TextBox txtCurrentBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrentBalance.EditableMode = SourceGrid.EditableMode.None;
            grdJournal[i, (int)GridColumn.Current_Balance] = new SourceGrid.Cells.Cell("", txtCurrentBalance);
            grdJournal[i, (int)GridColumn.Current_Balance].Value = "0";

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdJournal[i, (int)GridColumn.Remarks] = new SourceGrid.Cells.Cell("", txtRemarks);
            txtRemarks.Control.TextChanged += new EventHandler(Text_Change);
            // grdJournal[i, 5].Value = "";

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdJournal[i, (int)GridColumn.Ledger_ID] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdJournal[i, (int)GridColumn.Ledger_ID].Value = "0";

            SourceGrid.Cells.Editors.TextBox txtCurrentBalance1 = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            grdJournal[i, (int)GridColumn.Current_Bal_Actual] = new SourceGrid.Cells.Cell("", txtCurrentBalance1);
            grdJournal[i, (int)GridColumn.Current_Bal_Actual].Value = "0";

            SourceGrid.Cells.Editors.TextBox txtRefAmt = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            grdJournal[i, (int)GridColumn.Ref_Amt] = new SourceGrid.Cells.Cell("", txtRefAmt);
            grdJournal[i, (int)GridColumn.Ref_Amt].Value = "0(Dr)";

            SourceGrid.Cells.Editors.TextBox txtLedgerType = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdJournal[i, (int)GridColumn.Ledger_Type] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdJournal[i, (int)GridColumn.Ledger_Type].Value = " ";

            #region ALTERNATE COLOR
            if (RowCount % 2 == 0)
            {
                //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
            }
            else
            {
                alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
            }
            grdJournal[i, (int)GridColumn.Del].View = alternate;
            grdJournal[i, (int)GridColumn.Particular_Account_Head].View = alternate;
            grdJournal[i, (int)GridColumn.Dr_Cr].View = alternate;
            grdJournal[i, (int)GridColumn.Amount].View = alternate;
            grdJournal[i, (int)GridColumn.Current_Balance].View = alternate;
            grdJournal[i, (int)GridColumn.Remarks].View = alternate;
            grdJournal[i, (int)GridColumn.Ledger_ID].View = alternate;
            grdJournal[i, (int)GridColumn.Current_Bal_Actual].View = alternate;

            #endregion

        }

        private void btnGrpSave_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
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
                        string returnStr = m_VouConfig.ValidateManualVouNum(txtVchNo.Text, Convert.ToInt32(SeriesID.ID));
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
            if (!Validate())
                return;

            //Read checked AccClassID
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

            if (Global.DecimalPlaces == 2)
            {
                if ((m_DebitAmount - m_CreditAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, 2)) != "0.00")
                {
                    Global.Msg("Debit and Credit amount are not equal!");
                    return;
                }
            }
            else if (Global.DecimalPlaces == 3)
            {
                if ((m_DebitAmount - m_CreditAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, 3)) != "0.000")
                {
                    Global.Msg("Debit and Credit amount are not equal!");
                    return;
                }
            }
            else if (Global.DecimalPlaces == 4)
            {
                if ((m_DebitAmount - m_CreditAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, 4)) != "0.0000")
                {
                    Global.Msg("Debit and Credit amount are not equal!");
                    return;
                }
            }
            else
            {
                if ((m_DebitAmount - m_CreditAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, 0)) != "0")
                {
                    Global.Msg("Debit and Credit amount are not equal!");
                    return;
                }
            }

            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    try
                    {

                        dueDate();
                        frmDueDate dueDate1 = new frmDueDate(this, dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            dueDate1.ShowDialog();
                        }
                        isNew = true;

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

                            txtVchNo.Text = m_vounum.ToString();
                            txtVchNo.Enabled = false;
                        }
                        #endregion

                        NewGrid = " ";
                        OldGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + "-" + txtVchNo.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + ",";
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdJournal.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdJournal[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString() + ",";
                            string drcr = grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() + ",";
                            string amt = grdJournal[i + 1, (int)GridColumn.Amount].Value.ToString() + ",";
                            NewGrid = NewGrid + string.Concat(particular, drcr, amt) + ",";
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        //Call to Convert into XML Format
                        string JournalXMLString = ReadAllJournalEntry();
                        if (JournalXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast journal entry to XML!");
                            return;
                        }

                        //Read from sourcegrid and store it to table
                        #region Used for Checking Negative Cash And Negative Bank Delete after Transfering Code to Stored Procedure
                        DataTable JournalDetails = new DataTable();
                        JournalDetails.Columns.Add("Ledger");
                        JournalDetails.Columns.Add("DrCr");
                        JournalDetails.Columns.Add("Amount");
                        JournalDetails.Columns.Add("Remarks");
                        for (int i = 0; i < OnlyReqdDetailRows; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdJournal[i + 1, (int)GridColumn.Particular_Account_Head].ToString().Split('[');
                            //Solved problem with (NEW)
                            object journalAmount = grdJournal[i + 1, (int)GridColumn.Amount].Value;
                            if (ledgerName[0].ToString().ToUpper() == "(NEW)" && (journalAmount == null || journalAmount.ToString() == "0" || journalAmount.ToString() == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))))
                                continue;
                            else if (ledgerName[0].ToString().ToUpper() == "(NEW)" && journalAmount != null)
                            {
                                MessageBox.Show("Account Name not selected!!");
                                return;
                            }
                            try
                            {
                                Convert.ToDouble(journalAmount);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            JournalDetails.Rows.Add(ledgerName[0].ToString(), grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value, grdJournal[i + 1, (int)GridColumn.Amount].Value, grdJournal[i + 1, (int)GridColumn.Current_Balance].Value);

                            bool isCashAccount = false;
                            double totalDrCash, totalCrCash;
                            totalDrCash = totalCrCash = 0;
                            isCashAccount = Ledger.IsCashAccount(ledgerName[0].ToString());

                            #region GET CREDIT LIMIT SETTINGS
                            //here i am going to check the credit limit if any                          
                            int ID = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);
                            DataTable dt = Ledger.GetAllLedger(114);  //ID must be dynamic
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (ID != Convert.ToInt32(dr["LedgerID"]))
                                {
                                    //Do nothing
                                }
                                else
                                {
                                //    switch (Global.Credit_Limit)
                                //    {
                                        
                                //    }
                                }
                            }
                            #endregion

                            //Check the negative cash and negative Bank  ledger who is responsible for payment...so,If sorcegrid is Debit,no need to check because it will recieve amount bt check negative cash and negative bank in case of Credit

                            if (grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")//check negative cash and negative bank in case of ledger is responisble of payment otherwise no need to check...
                            {

                                #region BLOCK FOR CHECKING NEGATIVE CASH
                                //execute following code when only if setting of Negative cash is in Warn or Deny
                                if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                                {
                                    if (isCashAccount)//If cash account
                                    {
                                        double actualbal = 0;
                                        try
                                        {
                                            actualbal = Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Current_Bal_Actual].Value.ToString());
                                        }
                                        catch
                                        {
                                            actualbal = 0;
                                        }
                                        double amount = Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Amount].Value.ToString());

                                        if ((actualbal - amount) >= 0)
                                        {
                                            IsNegativeCash = false;
                                        }
                                        else
                                        {
                                            IsNegativeCash = true;
                                        }
                                       

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
                                }
                                #endregion

                                #region BLOCK FOR CHECKING NEGATIVE BANK
                                if ((Global.Default_NegativeBank == NegativeBank.Warn) || (Global.Default_NegativeBank == NegativeBank.Deny))
                                {

                                    bool isBankAccount = false;
                                    isBankAccount = Ledger.IsBankAccount((ledgerName[0].ToString()));
                                    if (isBankAccount)//If Bank account
                                    {
                                        double actualbal = 0;
                                        try
                                        {
                                            actualbal = Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Current_Bal_Actual].Value.ToString());
                                        }
                                        catch
                                        {
                                            actualbal = 0;
                                        }
                                        double amount = Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Amount].Value.ToString());

                                        if ((actualbal - amount) >= 0)
                                        {
                                            IsNegativeBank = false;
                                        }
                                        else
                                        {
                                            IsNegativeBank = true;
                                        }
                                       

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
                                                    {
                                                        return;
                                                    }
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
                        }

                        #endregion

                        #region Save xml data to Database
                        int returnJournalId = 0;
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                             //To insert journal
                            returnJournalId = Journal.InsertJournal(JournalXMLString);
                            // to send recurring settings
                            Journal.InsertVoucherRecurring(m_dtRecurringSetting, returnJournalId);

                            // to send reference information
                            Journal.InsertReference(returnJournalId, dtReference);
                            
                       }
                        #endregion
                        if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                        {
                            object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                        }
                        Global.Msg("Journal created successfully!");
                        if (m_isRecurring)
                        {
                            //RecurringVoucher.ModifyRecurringVoucherPosting(m_JournalID, "JOURNAL"); // set isPosed to true for recurring voucher
                            RecurringVoucher.ModifyRecurringVoucherPosting(m_RVID); // set isPosed to true for recurring voucher
                            m_isRecurring = false;
                        }

                        ClearVoucher();
                        ChangeState(EntryMode.NEW);
                        btnNew_Click(sender, e);

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
                        dueDate();
                        frmDueDate dueDate1 = new frmDueDate(this, dt1, Convert.ToInt32(txtJournalID.Text), "JRNL");
                        if (dt1.Rows.Count > 0)
                        {
                            dueDate1.ShowDialog();
                        }
                        isNew = false;
                        NewGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + "-" + txtVchNo.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + ",";
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdJournal.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdJournal[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                            string drcr = grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString();
                            string amt = grdJournal[i + 1, (int)GridColumn.Amount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, drcr, amt);
                        }

                        NewGrid = "NewGridValues" + NewGrid;
                        string JournalXMLString = ReadAllJournalEntry();
                        if (JournalXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast journal entry to XML!");
                            return;
                        }

                        //Read from sourcegrid and store it to table
                        #region Used for Checking Negative Cash And Negative Bank Delete after Transfering Code to Stored Procedure
                        DataTable JournalDetails = new DataTable();
                        JournalDetails.Columns.Add("Ledger");
                        JournalDetails.Columns.Add("DrCr");
                        JournalDetails.Columns.Add("Amount");
                        JournalDetails.Columns.Add("Remarks");

                        for (int i = 0; i < grdJournal.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdJournal[i + 1, (int)GridColumn.Particular_Account_Head].ToString().Split('[');
                            //Solved problem with (NEW)
                            object journalAmount = grdJournal[i + 1, (int)GridColumn.Amount].Value;
                            if (ledgerName[0].ToString().ToUpper() == "(NEW)" && (journalAmount == null || journalAmount.ToString() == "0" || journalAmount.ToString() == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))))
                                continue;
                            else if (ledgerName[0].ToString().ToUpper() == "(NEW)" && journalAmount != null)
                            {
                                MessageBox.Show("Account Name not selected!!");
                                return;
                            }
                            try
                            {
                                Convert.ToDouble(journalAmount);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            JournalDetails.Rows.Add(ledgerName[0].ToString(), grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value, grdJournal[i + 1, (int)GridColumn.Amount].Value, grdJournal[i + 1, (int)GridColumn.Current_Balance].Value);
                            bool isCashAccount = false;
                            double totalDrCash, totalCrCash;
                            totalDrCash = totalCrCash = 0;
                            isCashAccount = Ledger.IsCashAccount(ledgerName[0].ToString());
                            //Check the negative cash and negative Bank  ledger who is responsible for payment...so,If sorcegrid is Debit,no need to check because it will recieve amount bt check negative cash and negative bank in case of Credit
                            if (grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")//check negative cash and negative bank in case of ledger is responisble of payment otherwise no need to check...
                            {
                                #region BLOCK FOR CHECKING NEGATIVE CASH
                                //execute following code when only if setting of Negative cash is in Warn or Deny
                                if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                                {
                                    if (isCashAccount)//If cash account
                                    {
                                        double actualbal = 0;
                                        try
                                        {
                                            actualbal = Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Current_Bal_Actual].Value.ToString());
                                        }
                                        catch
                                        {
                                            actualbal = 0;
                                        }
                                        double amount = Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Amount].Value.ToString());

                                        if ((actualbal - amount) >= 0)
                                        {
                                            IsNegativeCash = false;
                                        }
                                        else
                                        {
                                            IsNegativeCash = true;
                                        }

                                        //double mDbalCash = 0;
                                        //double mCbalCash = 0;
                                        //int cashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);//Getting LedgerID with help of LedgerName
                                        //Transaction.GetLedgerBalance(null, null, cashLedgerId, ref mDbalCash, ref mCbalCash, AccountClassID, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                        ////Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        //if (grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        //{
                                        //    totalDrCash = (mDbalCash + Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Amount].Value));//In case of Cash and Bank ...Ledger Balance is Debit
                                        //}
                                        //else if (grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                                        //{
                                        //    totalCrCash = (mCbalCash + Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Amount].Value));//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
                                        //}
                                        //if ((totalDrCash - totalCrCash) >= 0)
                                        //    IsNegativeCash = false;
                                        //else
                                        //    IsNegativeCash = true;

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
                                }
                                #endregion

                                #region BLOCK FOR CHECKING NEGATIVE BANK
                                if ((Global.Default_NegativeBank == NegativeBank.Warn) || (Global.Default_NegativeBank == NegativeBank.Deny))
                                {

                                    bool isBankAccount = false;
                                    isBankAccount = Ledger.IsBankAccount((ledgerName[0].ToString()));
                                    if (isBankAccount)//If Bank account
                                    {
                                        double actualbal = 0;
                                        try
                                        {
                                            actualbal = Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Current_Bal_Actual].Value.ToString());
                                        }
                                        catch
                                        {
                                            actualbal = 0;
                                        }
                                        double amount = Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Amount].Value.ToString());

                                        if ((actualbal - amount) >= 0)
                                        {
                                            IsNegativeBank = false;
                                        }
                                        else
                                        {
                                            IsNegativeBank = true;
                                        }
                                        //double mDbalBank = 0;
                                        //double mCbalBank = 0;
                                        //double totalDrBank = 0;
                                        //double totalCrBank = 0;

                                        //int bankLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);

                                        //Transaction.GetLedgerBalance(null, null, bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                        ////Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        //if (grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        //{
                                        //    totalDrBank = (mDbalBank + Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Amount].Value));//In case of Cash and Bank ...Ledger Balance is Debit

                                        //}
                                        //else if (grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                                        //{
                                        //    totalCrBank = (mCbalBank + Convert.ToDouble(grdJournal[i + 1, (int)GridColumn.Amount].Value));//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
                                        //}
                                        //if ((totalDrBank - totalCrBank) >= 0)
                                        //    IsNegativeBank = false;
                                        //else
                                        //    IsNegativeBank = true;

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
                                                    {
                                                        return;
                                                    }
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
                        }
                        #endregion

                        //Now delete the previous entry and then save new entry
                        //call Acc.spRemoveJournalEntry

                        //IJournal m_Journal = new Journal();
                        //if (!m_Journal.RemoveJournalEntry(Convert.ToInt32(txtJournalID.Text)))
                        //{
                        //    MessageBox.Show("Unable to Update record. Please restart the modification");
                        //    return;
                        //}

                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            //using (SqlCommand dbCommand = new SqlCommand("Acc.xmlJournalUpdate", Global.m_db.cn))
                            //{
                            //    // we are going to use store procedure  
                            //    dbCommand.CommandType = CommandType.StoredProcedure;
                            //    // Add input parameter and set its properties.
                            //    SqlParameter parameter = new SqlParameter();
                            //    // Store procedure parameter name  
                            //    parameter.ParameterName = "@journal";
                            //    // Parameter type as XML 
                            //    parameter.DbType = DbType.Xml;
                            //    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                            //    parameter.Value = JournalXMLString; // XML string as parameter value  
                            //    // Add the parameter in Parameters collection.
                            //    dbCommand.Parameters.Add(parameter);
                            //    if (Global.m_db.cn.State == ConnectionState.Open)
                            //    {
                            //        Global.m_db.cn.Close();
                            //    }
                            //    Global.m_db.cn.Open();
                            //    int intRetValue = dbCommand.ExecuteNonQuery();

                            //For update Journal entry
                            Journal.EditJournal(JournalXMLString);

                            // to modify recurring settings
                            Journal.ModifyVoucherRecurring(m_dtRecurringSetting, Convert.ToInt32(txtJournalID.Text));

                            // to modify against references in the voucher
                            Journal.ModifyReference(Convert.ToInt32(txtJournalID.Text), dtReference, ToDeleteRows);
                            // MessageBox.Show(intRetValue.ToString());

                            // }
                        }
                        #endregion

                        Global.Msg("Journal modified successfully!");
                        txtJournalID.Clear();
                        ClearVoucher();
                        ChangeState(EntryMode.NEW);
                        btnNew_Click(sender, e);
                        txtJournalID.Clear();

                        if (chkPrntWhileSaving.Checked)
                        {
                            prntDirect = 1;
                            Navigation(Navigate.Last);
                            button3_Click(sender, e);
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
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

            }

        }

        private void dueDate()
        {
            try
            {
                dt1.Rows.Clear();
                dt1.Columns.Clear();
                dt1.Columns.Add("LedgerId");
                dt1.Columns.Add("LedgerName");

                int i = 1;
                // int ledgerId = 0;

                System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

                tw.WriteStartDocument();
                tw.WriteStartElement("LedgerIDs");

                for (i = 1; i <= grdJournal.Rows.Count() - 2; i++)
                {
                    //try
                    //{

                    //    string str = (grdJournal[i, (int)GridColumn.Ledger_ID].Value).ToString();
                    //    ledgerId = Convert.ToInt32(str);
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    //int GroupID = AccountGroup.GetGroupIDByLedgerID(ledgerId);
                    //if (GroupID == 29 && grdJournal[i, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
                    //{
                    //    dt1.Rows.Add(grdJournal[i, (int)GridColumn.Ledger_ID].Value, grdJournal[i, (int)GridColumn.Particular_Account_Head].Value);
                    //}

                    if (grdJournal[i, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
                    {
                        tw.WriteElementString("LedgerID", grdJournal[i, (int)GridColumn.Ledger_ID].Value.ToString());
                        //dt1.Rows.Add(grdJournal[i, (int)GridColumn.Ledger_ID].Value, grdJournal[i, (int)GridColumn.Particular_Account_Head].Value);
                    }
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
                dt1 = Ledger.FilterLedgersByGroup(strXML, 0, 29);
                // if(strXML=='' || strXML==null)
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        /// <summary>
        /// Read all journal Entry
        /// </summary>
        /// <returns></returns>
        private string ReadAllJournalEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            SeriesID = (ListItem)cboSeriesName.SelectedItem;
            liProjectID = (ListItem)cboProjectName.SelectedItem;

            //validate grid entry
            if (!ValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();

            #region  Journal
            tw.WriteStartElement("JOURNAL");
            {
                ///For Journal Master Section
                string first = txtfirst.Text;
                string second = txtsecond.Text;
                string third = txtthird.Text;
                string fourth = txtfourth.Text;
                string fifth = txtfifth.Text;
                ///int JournalID = System.Convert.ToInt32(9); // Auto increment               
                int SID = System.Convert.ToInt32(SeriesID.ID);
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);

                string JournalID = System.Convert.ToString(txtJournalID.Text);
                DateTime Journal_Date = Date.ToDotNet(txtDate.Text);
                string Remarks = System.Convert.ToString(txtRemarks.Text);
                int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                //string Created_By = System.Convert.ToString("Admin");
                string Created_By = User.CurrentUserName;
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                // string Modified_By = System.Convert.ToString("Admin");
                string Modified_By = User.CurrentUserName;

                tw.WriteStartElement("JOURNALMASTER");
                tw.WriteElementString("SeriesID", SID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("Journal_Date", Date.ToDB(Journal_Date));
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
                // int JournalID = 0;
                int LedgerID = 0;
                Decimal Amount = 0;
                string DrCr = "";
                string RemarksDetail = "";
                tw.WriteStartElement("JOURNALDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    JournalID = System.Convert.ToString(txtJournalID.Text);
                    LedgerID = System.Convert.ToInt32(grdJournal[i + 1, (int)GridColumn.Ledger_ID].Value);
                    Amount = System.Convert.ToDecimal(grdJournal[i + 1, (int)GridColumn.Amount].Value);
                    DrCr = System.Convert.ToString(grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value);
                    RemarksDetail = System.Convert.ToString(grdJournal[i + 1, (int)GridColumn.Remarks].Value);
                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("JournalID", JournalID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("DrCr", DrCr.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();

                tw.WriteStartElement("JOURNALDEBTORSDUEDATE");
                foreach (DataRow drduedate in dtDueDateInfo.Rows)
                {
                    tw.WriteStartElement("DUEDATEDETAIL");
                    tw.WriteElementString("DUEDATE", Date.ToDB(Convert.ToDateTime(drduedate["DueDate"])));
                    tw.WriteElementString("LedgerID", drduedate["LedgerID"].ToString());
                    tw.WriteElementString("VoucherType", "JRNL");
                    tw.WriteElementString("VoucherNo", Voucher_No);
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                //Write Checked Accounting class ID
                try
                {
                    ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
                    tw.WriteStartElement("AccClassIDSettings");
                    foreach (string tag in arrNode)
                    {
                        AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccClassID", Convert.ToInt32(tag).ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }
                //For Computer Name
                string ComputerName = Global.ComputerName;
                string Ip_Address = Global.IpAddress;
                string Mac_Address = Global.MacAddess;
                tw.WriteStartElement("COMPUTERDETAILS");
                tw.WriteElementString("CompDetails", ComputerName.ToString());
                tw.WriteElementString("MacAddress", Mac_Address.ToString());
                tw.WriteElementString("IpAddress", Ip_Address.ToString());
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
        private bool ValidateGrid()
        {
            int[] LdrID = new int[200];
            decimal[] Amt = new decimal[200];

            //Validate input grid record
            for (int i = 0; i < grdJournal.Rows.Count - 1; i++)
            {
                try
                {
                    //if ledger ID repeats then message it
                    // if LedgerID is not present in between them
                    int tempValue = 0;
                    decimal tempDecValue = 0;
                    try
                    {
                        tempValue = System.Convert.ToInt32(grdJournal[i + 1, (int)GridColumn.Ledger_ID].Value);
                    }
                    catch (Exception ex)
                    {
                        tempValue = 0;
                    }
                    try
                    {
                        tempDecValue = System.Convert.ToDecimal(grdJournal[i + 1, (int)GridColumn.Amount].Value);
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
                        if (i + 2 == grdJournal.Rows.Count && grdJournal[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)")
                        {
                            //Do Nothing
                        }
                        else
                            return false;
                    }
                    else
                        LdrID[i] = tempValue;

                    if (i + 2 == grdJournal.Rows.Count && grdJournal[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)")
                    {
                        //Donothing
                    }
                    else
                        Amt[i] = tempDecValue;
                }

                catch (Exception ex)
                {
                    MessageBox.Show("error" + ex);
                    return false;
                }
            }
            OnlyReqdDetailRows = LdrID.Count(i => i != 0);
            return true;
        }

        private bool Validate()
        {
            bool bValidate = false;
            FormHandle m_FH = new FormHandle();

            m_FH.AddValidate(txtDate, DType.DATE, "Please enter valid date");

            bValidate = m_FH.Validate();

            if (cboSeriesName.Text == "")
            {
                Global.MsgError("Invalid Series Name,please select valid Series Name and try again!");
                bValidate = false;
            }

            if (cboProjectName.SelectedItem == null)
            {
                Global.MsgError("Invalid Project Name Selected");
                cboProjectName.Focus();
                bValidate = false;
            }
            if (!(grdJournal.Rows.Count > 2))
            {
                Global.MsgError("Please complete the journal entry!");
                grdJournal.Focus();
                bValidate = false;
            }
            return bValidate;
        }

        //Enables and disables the button states
         private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
        }

        private void EnableControls(bool Enable)
        {
            chkRecurring.Enabled = btnSetup.Enabled = txtVchNo.Enabled = txtDate.Enabled = btnDate.Enabled = txtRemarks.Enabled = grdJournal.Enabled = cboSeriesName.Enabled = cboProjectName.Enabled = tabControl1.Enabled = Enable;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false);
                    IsFieldChanged = false;
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    LoadSeriesNo();
                    // txtDate.Text
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
            }
        }

        private void LoadSeriesNo()
        {
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("JNL");
            cboSeriesName.Items.Clear();
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                cboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            cboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            cboSeriesName.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        /// <summary>
        /// Writes the header part for grdJournal
        /// </summary>
        private void AddGridHeader()
        {
            grdJournal[0, (int)GridColumn.Del] = new MyHeader("Delete");
            grdJournal[0, (int)GridColumn.Del].Column.Width = 20;
            grdJournal.Columns[(int)GridColumn.Del].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdJournal[0, (int)GridColumn.SNo] = new MyHeader("SNo.");
            grdJournal[0, (int)GridColumn.SNo].Column.Width = 30;
            grdJournal.Columns[(int)GridColumn.SNo].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdJournal[0, (int)GridColumn.Particular_Account_Head] = new MyHeader("Particulars/Accounting Head");
            grdJournal[0, (int)GridColumn.Particular_Account_Head].Column.Width = 200;
            grdJournal.Columns[(int)GridColumn.Particular_Account_Head].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdJournal[0, (int)GridColumn.Dr_Cr] = new MyHeader("Dr/Cr");
            grdJournal[0, (int)GridColumn.Dr_Cr].Column.Width = 55;
            grdJournal.Columns[(int)GridColumn.Dr_Cr].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdJournal[0, (int)GridColumn.Amount] = new MyHeader("Amount");
            grdJournal[0, (int)GridColumn.Amount].Column.Width = 100;
            grdJournal.Columns[(int)GridColumn.Amount].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdJournal[0, (int)GridColumn.Current_Balance] = new MyHeader("Balance");
            grdJournal[0, (int)GridColumn.Current_Balance].Column.Width = 140;
            grdJournal.Columns[(int)GridColumn.Current_Balance].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdJournal[0, (int)GridColumn.Remarks] = new MyHeader("Remarks");
            grdJournal[0, (int)GridColumn.Remarks].Column.Width = 210;
            grdJournal.Columns[(int)GridColumn.Remarks].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdJournal[0, (int)GridColumn.Ledger_ID] = new MyHeader("LedgerID");
            grdJournal[0, (int)GridColumn.Ledger_ID].Column.Width = 50;
            grdJournal[0, (int)GridColumn.Ledger_ID].Column.Width = 50;
            grdJournal[0, (int)GridColumn.Ledger_ID].Column.Visible = false;

            grdJournal[0, (int)GridColumn.Current_Bal_Actual] = new MyHeader("CurrentBalance");
            //  grdJournal[0, (int)GridColumn.Current_Bal_Actual].Column.Visible = false;
            grdJournal[0, (int)GridColumn.Current_Bal_Actual].Column.Visible = false;

            grdJournal[0, (int)GridColumn.Ref_Amt] = new MyHeader("RefAmt");
            grdJournal[0, (int)GridColumn.Ref_Amt].Column.Width = 50;
            grdJournal[0, (int)GridColumn.Ref_Amt].Column.Visible = false;

            grdJournal[0, (int)GridColumn.Ledger_Type] = new MyHeader("LedgerType");
            grdJournal[0, (int)GridColumn.Ledger_Type].Column.Width = 50;
            grdJournal[0, (int)GridColumn.Ledger_Type].Column.Visible = false;



        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }

            if (!ContinueWithoutSaving())
            {
                return;
            }

            ClearVoucher();

            //Set the default date to today's date
            txtDate.Text = Date.ToSystem(Date.GetServerDate());

            ChangeState(EntryMode.NEW);
        }

        /// <summary>
        /// Clears the whole voucher
        /// </summary>
        private void ClearVoucher()
        {
            m_isRecurring = false;
            m_RVID = 0;
            ClearJournal();
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdJournal.Redim(2, Enum.GetValues(typeof(GridColumn)).Length);
            AddGridHeader(); //Write header part
            AddRowJournal(1);
            ClearRecurringSetting();
            dtReference.Rows.Clear();
            AddReferenceColumns();

        }

        //Clears the text of every field of Journal form
        private void ClearJournal()
        {
            txtVchNo.Clear(); //actually generate a new voucher no.
            // txtDate.Clear();
            txtRemarks.Clear();
            //txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            grdJournal.Rows.Clear();
            //cboSeriesName.Text = string.Empty;
            cboProjectName.SelectedIndex = 0;
            lblCreditTotal.Text = string.Empty;
            lblDebitTotal.Text = string.Empty;
        }

        private bool Navigation(Navigate NavTo)
        {
            try
            {
                if (!IsShortcutKey)
                {
                    ChangeState(EntryMode.NORMAL);
                }
                //Get the one step previous voucher
                int vouchID = 0;
                bool isCopy = false;

                try
                {
                    if (JournalIDCopy > 0) //Copy mode, so simply paste the id
                    {
                        vouchID = JournalIDCopy;
                        JournalIDCopy = 0;
                        isCopy = true;
                    }
                    else //if not in copy mode
                    {
                        vouchID = Convert.ToInt32(txtJournalID.Text);
                    }
                }
                catch (Exception)
                {
                    vouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
                IJournal m_Journal = new Journal();

                DataTable dtJournalMaster = m_Journal.NavigateJournalMaster(vouchID, NavTo);

                if (dtJournalMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;

                }
                //This section is for shortcut key eg.: when Ctrl+R is pressed only narration is reloaded.

                DataRow drJournalMaster = dtJournalMaster.Rows[0]; //There is only one row. First row is the required record

                if (IsShortcutKey)
                {
                    txtRemarks.Text = drJournalMaster["Remarks"].ToString();
                    IsShortcutKey = false;
                    txtRemarks.SelectionStart = txtRemarks.Text.Length + 1;
                    return false;
                }
                //Clear everything in the form
                ClearVoucher();

                ///Write the corresponding textboxes
                /// DataRow drJournalMaster = dtJournalMaster.Rows[0]; 
                ///There is only one row. First row is the required record
                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drJournalMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Journal");
                    cboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dt.Rows[0];
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

                        txtfirst.Text = drJournalMaster["Field1"].ToString();
                        txtsecond.Text = drJournalMaster["Field2"].ToString();
                        txtthird.Text = drJournalMaster["Field3"].ToString();
                        txtfourth.Text = drJournalMaster["Field4"].ToString();
                        txtfifth.Text = drJournalMaster["Field5"].ToString();
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

                        txtfirst.Text = drJournalMaster["Field1"].ToString();
                        txtsecond.Text = drJournalMaster["Field2"].ToString();
                        txtthird.Text = drJournalMaster["Field3"].ToString();
                        txtfourth.Text = drJournalMaster["Field4"].ToString();
                        txtfifth.Text = drJournalMaster["Field5"].ToString();
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

                        txtfirst.Text = drJournalMaster["Field1"].ToString();
                        txtsecond.Text = drJournalMaster["Field2"].ToString();
                        txtthird.Text = drJournalMaster["Field3"].ToString();
                        txtfourth.Text = drJournalMaster["Field4"].ToString();
                        txtfifth.Text = drJournalMaster["Field5"].ToString();

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

                        txtfirst.Text = drJournalMaster["Field1"].ToString();
                        txtsecond.Text = drJournalMaster["Field2"].ToString();
                        txtthird.Text = drJournalMaster["Field3"].ToString();
                        txtfourth.Text = drJournalMaster["Field4"].ToString();
                        txtfifth.Text = drJournalMaster["Field5"].ToString();

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

                        txtfirst.Text = drJournalMaster["Field1"].ToString();
                        txtsecond.Text = drJournalMaster["Field2"].ToString();
                        txtthird.Text = drJournalMaster["Field3"].ToString();
                        txtfourth.Text = drJournalMaster["Field4"].ToString();
                        txtfifth.Text = drJournalMaster["Field5"].ToString();
                    }


                }
                lblVouNo.Visible = true;
                txtVchNo.Visible = true;
                if (!isCopy)
                {
                    txtVchNo.Text = drJournalMaster["Voucher_No"].ToString();
                    txtDate.Text = Date.DBToSystem(drJournalMaster["Journal_Date"].ToString());

                }
                else
                {
                    txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); // if copied, load today's date
                }
                txtRemarks.Text = drJournalMaster["Remarks"].ToString();
                txtJournalID.Text = drJournalMaster["JournalID"].ToString();

                DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drJournalMaster["ProjectID"]), LangMgr.DefaultLanguage);
                if (dtProjectInfo.Rows.Count != 0)
                {
                    DataRow drProjectInfo = dtProjectInfo.Rows[0];
                    cboProjectName.Text = drProjectInfo["Name"].ToString();
                    //This is for crystal reports
                    dsJournal.Tables["tblJournalMaster"].Rows.Add(cboSeriesName.Text, drJournalMaster["Voucher_No"].ToString(), Date.DBToSystem(drJournalMaster["Journal_Date"].ToString()), drJournalMaster["Remarks"].ToString(), drProjectInfo["Name"].ToString());
                }
                else
                {
                    cboProjectName.Text = "None";
                    //This is for crystal reports
                    dsJournal.Tables["tblJournalMaster"].Rows.Add(cboSeriesName.Text, drJournalMaster["Voucher_No"].ToString(), Date.DBToSystem(drJournalMaster["Journal_Date"].ToString()), drJournalMaster["Remarks"].ToString(), "None");
                }

                DataTable dtJournalDetail = m_Journal.GetJournalDetail(Convert.ToInt32(txtJournalID.Text));
                for (int i = 1; i <= dtJournalDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtJournalDetail.Rows[i - 1];
                    grdJournal[i, (int)GridColumn.SNo].Value = i.ToString();
                    grdJournal[i, (int)GridColumn.Particular_Account_Head].Value = drDetail["LedgerName"].ToString();
                    grdJournal[i, (int)GridColumn.Dr_Cr].Value = drDetail["DrCr"].ToString();
                    grdJournal[i, (int)GridColumn.Amount].Value = Convert.ToDouble(drDetail["Amount"]).ToString();

                    ///////////////////////You have to specify accounting class here *************************************
                    ///Get the Ledger Detail Info by sending LedgerID
                    ///then place LedgerID and Current balance in appropriate field
                    //DataTable dtLdrInfo = Ledger.GetLedgerDetail("1", null, null, Convert.ToInt32(drDetail["LedgerID"]));
                    DataTable dtLdrInfo = Ledger.GetLedgerDetails1("<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>", null, null, Convert.ToInt32(drDetail["LedgerID"]), 0, null);

                    if (dtLdrInfo.Rows.Count != 1)
                    {
                        grdJournal[i, (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        grdJournal[i, (int)GridColumn.Current_Bal_Actual].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    else
                    {

                        DataRow drLdrInfo = dtLdrInfo.Rows[0];

                        double Amount = 0;
                        string drcr = " ";
                        // double Amount = (Convert.ToDouble(drLdrInfo["DebitTotal"]) + Convert.ToDouble(drLdrInfo["OpenBalDr"])) - (Convert.ToDouble(drLdrInfo["CreditTotal"]) + Convert.ToDouble(drLdrInfo["OpenBalCr"]));

                        if (drLdrInfo["DrCr"].ToString() == "DEBIT")//if current balance is debit
                        {
                            grdJournal[i, (int)GridColumn.Current_Balance].Value = Convert.ToDouble(drLdrInfo["FinalBal"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Dr.)");
                            if (drLdrInfo["LedgerType"].ToString() == "DR")//if ledger type is debit
                            {
                                Amount = Convert.ToDouble(drLdrInfo["FinalBal"]);
                                if (drDetail["DrCr"].ToString() == "Debit")//if grid amount is debit, also ledger type is debit so we need to substract grid amount to get actual balance before this transaction
                                {
                                    Amount = Amount - Convert.ToDouble(drDetail["Amount"]);
                                }
                                else
                                {
                                    Amount = Amount + Convert.ToDouble(drDetail["Amount"]);
                                }
                               

                            }
                            else//if ledger type is credit but has balance of type debit
                            {
                                Amount = 0 - Convert.ToDouble(drLdrInfo["FinalBal"]);
                                if (drDetail["DrCr"].ToString() == "Debit")//if grid amount is debit
                                {
                                    Amount = Amount + Convert.ToDouble(drDetail["Amount"]);
                                }
                                else
                                {
                                    Amount = Amount - Convert.ToDouble(drDetail["Amount"]);
                                }

                                
                            }


                        }
                        else
                        {
                            grdJournal[i, (int)GridColumn.Current_Balance].Value = Convert.ToDouble(drLdrInfo["FinalBal"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces) + "(Cr.)");
                            if (drLdrInfo["LedgerType"].ToString() == "DR")//if ledger type is debit
                            {
                                Amount = 0 - Convert.ToDouble(drLdrInfo["FinalBal"]);
                                if (drDetail["DrCr"].ToString() == "Debit")//if grid amount is debit also ledger type is debit so we need to substract to get actual balance before this transaction
                                {
                                    Amount = Amount - Convert.ToDouble(drDetail["Amount"]);
                                }
                                else
                                {
                                    Amount = Amount + Convert.ToDouble(drDetail["Amount"]);
                                }

                               

                            }

                            else//if ledger type is credit n has balance of type credit
                            {
                                Amount = Convert.ToDouble(drLdrInfo["FinalBal"]);
                                if (drDetail["DrCr"].ToString() == "Debit")//if grid amount is debit
                                {
                                    Amount = Amount + Convert.ToDouble(drDetail["Amount"]);
                                }
                                else
                                {
                                    Amount = Amount - Convert.ToDouble(drDetail["Amount"]);
                                }

                                
                            }

                        }
                        grdJournal[i, 8].Value = Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        grdJournal[i, (int)GridColumn.Ledger_Type].Value = drLdrInfo["LedgerType"].ToString();



                    }
                    // grdJournal[0, 5] = Current Balance  *****ABOVE CODE
                    grdJournal[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                    grdJournal[i, (int)GridColumn.Ledger_ID].Value = drDetail["LedgerID"].ToString();
                    AddRowJournal(grdJournal.RowsCount);

                    //This is for crystal report
                    if (drDetail["DrCr"].ToString() == "Debit")
                    {
                        dsJournal.Tables["tblJournalDetails"].Rows.Add(drDetail["LedgerName"].ToString(), null, null, Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), null, drDetail["Remarks"].ToString());
                        totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                    }
                    else
                    {
                        dsJournal.Tables["tblJournalDetails"].Rows.Add(drDetail["LedgerName"].ToString(), null, null, null, Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                    }
                    totalRptAmount = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();
                }
                //Calculate the Debit and Credit totals
                CalculateDrCr();
                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtJournalID.Text), "JRNL");
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

                // load recurring settings for saved voucher
                CheckRecurringSetting(txtJournalID.Text);

                // load all against reference for the voucher
                if (txtJournalID.Text != "")
                    dtReference = VoucherReference.GetAllRefAgainstForVoucher(Convert.ToInt32(txtJournalID.Text), "JRNL");

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
                {
                    if (Convert.ToInt32(nd.Tag) == id)
                    {
                        nd.Checked = true;
                    }
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you don't have permission to View. Please contact your administrator for permission.");
                return;
            }
            //IsFieldChanged = false;
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            Navigation(Navigate.Next);
            IsFieldChanged = false;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            // IsFieldChanged = false;
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            Navigation(Navigate.First);
            IsFieldChanged = false;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //IsFieldChanged = false;
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}
            Navigation(Navigate.Prev);
            IsFieldChanged = false;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //IsFieldChanged = false;
            //if (!ContinueWithoutSaving())
            //{
            //    return;
            //}

            Navigation(Navigate.Last);
            IsFieldChanged = false;

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            if (txtJournalID.Text != null && txtJournalID.Text != "")
            {
                isNew = false;
                OldGrid = " ";
                bool chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_MODIFY");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                    return;
                }
                if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
                {
                    MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                    return;
                }
                OldGrid = OldGrid + "Voucher No" + "-" + txtVchNo.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + ",";
                //Collect the Contents of the grid for audit log
                for (int i = 0; i < grdJournal.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                {
                    string particular = grdJournal[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString() + ",";
                    string drcr = grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() + ",";
                    string amt = grdJournal[i + 1, (int)GridColumn.Amount].Value.ToString() + ",";
                    OldGrid = OldGrid + string.Concat(particular, drcr, amt) + ",";
                }
                OldGrid = "OldGridValues" + OldGrid;
                //if (!ContinueWithoutSaving())
                //{
                //    return;
                //}
                if (txtJournalID.Text.Length <= 0)
                {
                    Global.MsgError("Please navigate to existing journal first and then try again!");
                    return;
                }
                EnableControls(true);
                ChangeState(EntryMode.EDIT);

                //if automatic voucher number increment is selected
                string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                if (NumberingType == "AUTOMATIC")
                    txtVchNo.Enabled = false;

            }
            else
                ChangeState(EntryMode.NEW);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdJournal.Rows.Count > 3)  // if grid doesnot contain any data then donot copy
                    JournalIDCopy = Convert.ToInt32(txtJournalID.Text);
                else
                    throw new Exception();
                //MessageBox.Show("There is nothing to copy. Please first navigate item!");

            }
            catch
            {
                MessageBox.Show("There is nothing to copy. Please first navigate item!");
            }
        }

        private void cboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    txtVchNo.Enabled = true;
                    SeriesID = (ListItem)cboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                    if (NumberingType == "AUTOMATIC" && !m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            return;
                        }
                        lblVouNo.Visible = true;
                        txtVchNo.Visible = true;
                        txtVchNo.Text = m_vounum.ToString();
                        txtVchNo.Enabled = false;
                    }
                    else if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        lblVouNo.Visible = false;
                        txtVchNo.Visible = false;
                    }
                    if (m_JournalID > 0 && !m_isRecurring)
                    {
                        lblVouNo.Visible = true;
                        txtVchNo.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (JournalIDCopy > 0)
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

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CheckIfBankReconciliationClosed())
            {
                return;
            }
            bool chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
           string GridValues = "";
           GridValues = GridValues + "Voucher No" + "-" + txtVchNo.Text + "," + "Series" + "-" + cboSeriesName.Text + "," + "Project" + "-" + cboProjectName.Text + "," + "Date" + "-" + txtDate.Text + ",";
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdJournal.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdJournal[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString() + ",";
                string drcr = grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() + ",";
                string amt = grdJournal[i + 1, (int)GridColumn.Amount].Value.ToString() + ",";
                GridValues = GridValues + string.Concat(particular, drcr, amt) + ",";
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            try
            {
                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the Journal - " + txtJournalID.Text + "?") == DialogResult.Yes)
                {
                    Journal DelJournal = new Journal();

                    // delete reference
                    string res = VoucherReference.DeleteReference(Convert.ToInt32(txtJournalID.Text), "JRNL");
                    if (res != "Success")
                    {
                        Global.MsgError("Unable to delete the voucher due to " + res);
                        return;
                    }

                    if(DelJournal.RemoveJournalEntry(Convert.ToInt32(txtJournalID.Text)))
                    {

                        AuditLogDetail auditlog = new AuditLogDetail();
                        auditlog.ComputerName = Global.ComputerName;
                        auditlog.UserName = User.CurrentUserName;
                        auditlog.Voucher_Type = "JRNL";
                        auditlog.Action = "DELETE";
                        auditlog.Description = NewGrid;
                        auditlog.RowID = Convert.ToInt32(txtJournalID.Text);//use master master id
                        auditlog.MAC_Address = Global.MacAddess;
                        auditlog.IP_Address = Global.IpAddress;
                        auditlog.VoucherDate = Date.ToDB(DateTime.Now).ToString();

                        auditlog.CreateAuditLog(auditlog);


                        RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "JOURNAL"); // deleting the recurring setting if voucher is deleted

                        Global.Msg("Journal -" + txtJournalID.Text + " deleted successfully!");



                        //Navigate to 1 step previous
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
                        Global.MsgError("There was an error while deleting Journal -" + txtJournalID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void cboSeriesName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDate.Focus();
            }
        }

        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboProjectName.Focus();
            }
        }

        private void cboProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 || e.KeyCode == Keys.Tab)
            {
                grdJournal.Focus();
            }
        }

        private void chkCash_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                chkCheque.Focus();
            }
        }

        private void chkCheque_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtCheckNo.Focus();
            }
        }

        private void txtCheckNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboBankName.Focus();
            }
        }

        private void cboBankName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtRemarks.Focus();
            }
        }

        private void txtRemarks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && e.Modifiers == Keys.Control)
            {
                bool chkUserPermission = UserPermission.ChkUserPermission("JOURNAL_VIEW");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                    return;
                }
                IsShortcutKey = true;
                Navigation(Navigate.Last);
            }
            if (e.KeyValue == 13)
            {
                chkDoNotClose.Focus();
            }
        }

        private void chkDoNotClose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                chkPrntWhileSaving.Focus();
            }
        }

        private void chkPrntWhileSaving_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnSave.Focus();
            }
        }

        private void chkCheque_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCheque.Checked)
            {
                txtCheckNo.Enabled = cboBankName.Enabled = true;
            }
            if (!chkCheque.Checked)
            {
                txtCheckNo.Enabled = cboBankName.Enabled = false;
            }
        }

        private void frmJournal_KeyDown(object sender, KeyEventArgs e)
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
                btnEdit_Click(sender, e);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                btnGrpSave_Click(sender, e);
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
                button3_Click(sender, e);
            }
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }

        private void frmJournal_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!ContinueWithoutSaving())
            //{
            //    e.Cancel = true;
            //}

        }

        private void frmJournal_FormClosed(object sender, FormClosedEventArgs e)
        {
            //btnExit_Click(sender, e);
        }

        private void btn_groupvoucherposting_Click(object sender, EventArgs e)
        {
            Journal m_Journal = new Journal();
            // DataTable dtJournalDetail = m_Journal.GetJournalDetail(Convert.ToInt32(drJournalMasterInfo["JournalID"]));
            int rowid = m_Journal.GetRowID(txtVchNo.Text);
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
            for (int i = 0; i < grdJournal.Rows.Count - 2; i++)
            {
                dtbulkvoucher.Rows.Add(grdJournal[i + 1, (int)GridColumn.Particular_Account_Head].Value, grdJournal[i + 1, (int)GridColumn.Dr_Cr].Value, grdJournal[i + 1, (int)GridColumn.Amount].Value, grdJournal[i + 1, (int)GridColumn.Ledger_ID].Value, "JRNL", txtVchNo.Text, grdJournal[i + 1, (int)GridColumn.Remarks].Value);
            }
            frmGroupVoucherList fgl = new frmGroupVoucherList(dtbulkvoucher, SID, ProjectID, rowid);
            fgl.ShowDialog();
        }

        private void cboProjectName_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 13 || e.KeyCode == Keys.Tab)
            //{
            //    grdJournal.Focus();
            //}
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
            dsJournal.Clear();
            totalAmt = 0;
            rptJournal rpt = new rptJournal();
            Misc.WriteLogo(dsJournal, "tblImage");
            rpt.SetDataSource(dsJournal);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_City = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvTotalAmt = new CrystalDecisions.Shared.ParameterDiscreteValue();
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
                rpt.SetParameterValue("Total_Amt", totalRptAmount);
                if (totalRptAmount == "")
                {
                    totalRptAmount = "0";
                }
                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;
                string inwords = AmountToWords.ConvertNumberAsText(totalRptAmount);
                rpt.SetParameterValue("AmtInWords", inwords);
                frmReportViewer frm = new frmReportViewer();
                frm.SetReportSource(rpt);
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

                //frm.Show();
                frm.WindowState = FormWindowState.Maximized;
            }
        }

        private void statusJournal_Click(object sender, EventArgs e)
        {

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
                frmVoucherRecurring fvr = new frmVoucherRecurring(this, "JOURNAL", m_dtRecurringSetting);
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
                    int res = RecurringVoucher.DeleteRecurringVoucherSetting(recurringVoucherID, "JOURNAL"); // delete from database
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
            frmVoucherRecurring fvr = new frmVoucherRecurring(this, "JOURNAL", m_dtRecurringSetting);
            fvr.ShowDialog();
        }

        string RSID = null, recurringVoucherID = null;
        public void CheckRecurringSetting(string VoucherID)
        {
            Global.m_db.setCommandType(CommandType.Text);
            m_dtRecurringSetting = RecurringVoucher.GetRecurringVoucherSetting(VoucherID, "JOURNAL");
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

        #region methods related to voucher List
        private void btnList_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    // check if the user has permission to view the voucher
            //    if (!UserPermission.ChkUserPermission("JOURNAL_VIEW"))
            //    {
            //        Global.MsgError("Sorry! you donot have permission to View Journal. Please contact your administrator for permission.");
            //        return;
            //    }
            //    string VoucherType = "JOURNAL";

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
                if (!UserPermission.ChkUserPermission("JOURNAL_VIEW"))
                {
                    Global.MsgError("Sorry! you donot have permission to View Journal. Please contact your administrator for permission.");
                    return;
                }
                string[] vouchValues = new string[5];
                vouchValues[0] = "JOURNAL";               // voucherType
                vouchValues[1] = "Acc.tblJournalMaster";   // master tableName for the given voucher type
                vouchValues[2] = "Acc.tblJournalDetail";  // details tableName for the given voucher type
                vouchValues[3] = "JournalID";              // master ID for the given master table
                vouchValues[4] = "Journal_Date";              // date field for a given voucher

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
            m_JournalID = VoucherID;
            frmJournal_Load(null, null);
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
                if (crDr == "(Cr)")
                    grdJournal[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Credit";

                else
                    grdJournal[CurrRowPos, (int)GridColumn.Dr_Cr].Value = "Debit";

                grdJournal[CurrRowPos, (int)GridColumn.Amount].Value = amt.ToString();
                grdJournal[CurrRowPos, (int)GridColumn.Ref_Amt].Value = amt.ToString() + crDr;

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

        public bool CheckAmtAgainstRefAmt()
        {
            try
            {
                bool res = true;
                decimal Amt = Convert.ToDecimal(grdJournal[CurrRowPos, (int)GridColumn.Amount].Value);
                string crdr = grdJournal[CurrRowPos, (int)GridColumn.Dr_Cr].Value.ToString();

                string amtCrDr = grdJournal[CurrRowPos, (int)GridColumn.Ref_Amt].Value.ToString();
                decimal refAmt = Convert.ToDecimal(amtCrDr.Substring(0, amtCrDr.Length - 4));
                string ledgerName = grdJournal[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString();
                if ((refAmt > 0) && (refAmt < Amt) && (amtCrDr.Contains(crdr.Substring(0, crdr.Length - 4))))
                {
                    Global.MsgError("Your transaction amount for ledger " + ledgerName + " is " + Amt + " \n which is greater than the reference amount i.e. " + refAmt + " !");
                    grdJournal[CurrRowPos, (int)GridColumn.Amount].Value = refAmt.ToString();
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

        #region method related to bank reconciliation closing
        public bool CheckIfBankReconciliationClosed()
        {
            try
            {
                bool res = false;

                for (int i = 1; i < grdJournal.Rows.Count; i++)
                {
                    int bankID = Convert.ToInt32(grdJournal[i, (int)GridColumn.Ledger_ID].Value);

                    res = BankReconciliation.IsBankReconciliationClosed(bankID, Date.ToDotNet(txtDate.Text));
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

            };
        }

        #endregion

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }






    }
    }

