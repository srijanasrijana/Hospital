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
using Inventory.DataSet;
using Inventory.CrystalReports;
using DateManager;
using System.Data.SqlClient;
using ErrorManager;
using System.IO;
using Inventory.Forms;
using CrystalDecisions.Shared;
using Common;

namespace Inventory
{
    public partial class frmBankPayment : Form,IfrmAddAccClass, IfrmDateConverter, ILOVLedger,IDueDate
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
        private DataSet.dsBankPayment dsBankPayment = new DataSet.dsBankPayment();
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
        private enum GridColumn : int
        {
            Del = 0, Code_No, Particular_Account_Head, Amount, Current_Balance, Remarks, Cheque_No, ChequeDate,  Ledger_ID, Current_Bal_Actual
        };

        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtChequeDateFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();

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

        public frmBankPayment(int BankPaymentID)
        {
            InitializeComponent();
            this.m_BankPaymentID = BankPaymentID;
        }

        public void AddLedger(string LedgerName, int LedgerID,string CurrentBalance, bool IsSelected)
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
            if(!IsChequeDateButton)
            txtDate.Text = Date.ToSystem(DotNetDate);
            if(IsChequeDateButton)
            ctx1.Value = Date.ToSystem(DotNetDate);
        }

        private int ChildCount(int AccClassID)
        {
            try
            {
                int m_RecCount = (int)User.GetChildTable(AccClassID).Rows.Count;
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
        private void Text_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
        }

        private void frmBankPayment_Load(object sender, EventArgs e)
        {
            chkDoNotClose.Checked = true;
            ChangeState(EntryMode.NEW);
           // ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            ShowAccClassInTreeView(treeAccClass, null, 0);
            m_mode = EntryMode.NEW;          
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
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
                cmboBankAccount.SelectedIndexChanged += new EventHandler(Text_Change);
                cmboBankAccount.SelectedIndexChanged += new EventHandler(cmboBankAccount_IndexChanged);
                cboProjectName.SelectedIndexChanged += new EventHandler(Text_Change);
                txtRemarks.TextChanged += new EventHandler(Text_Change);

                //Event trigerred when delete button is clicked
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                evtAccountFocusLost.FocusLeft += new EventHandler(evtAccountFocusLost_FocusLeft); 
                evtAmountFocusLost.FocusLeft += new EventHandler(Amount_Focus_Lost);
                evtChequeDateFocusLost.Click += new EventHandler(ChequeDate_Click);
                evtChequeDateFocusLost.Click += new EventHandler(Text_Change);                            
                #endregion

                #region Load cboBankAccount According to User Setting
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                foreach (DataRow drBankLedgers in dtBankLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cmboBankAccount.Items.Add(new ListItem((int)drBankLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cmboBankAccount.DisplayMember = "value";//This value is  for showing at Load condition
                cmboBankAccount.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());
                foreach (ListItem lst in cmboBankAccount.Items)
                {
                    if (roleid == 37)
                    {
                        if (lst.ID == Convert.ToInt32(Settings.GetSettings("DEFAULT_BANK_ACCOUNT")))
                        {

                            cmboBankAccount.Text = lst.Value;
                            // MessageBox.Show(IsFieldChanged.ToString());
                            IsFieldChanged = false;
                            break;
                        }
                    }
                    else
                    {
                        //UserPreference.GetValue("DEFAULT_DATE", uid)
                        if (lst.ID == Convert.ToInt32(UserPreference.GetValue("DEFAULT_BANK_ACCOUNT", uid)))
                        {

                            cmboBankAccount.Text = lst.Value;
                            // MessageBox.Show(IsFieldChanged.ToString());
                            IsFieldChanged = false;
                            break;
                        }
                    }
                }
               
                #endregion

                grdBankPayment.Redim(2, 10);
                btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                AddGridHeader();
                AddRowBankPayment(1);
                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID


                if (m_BankPaymentID > 0)
                {
                    //Show the values in fields

                    try
                    {

                        ChangeState(EntryMode.NORMAL);

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
                            cboSeriesName.Text = "";

                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            cboSeriesName.Text = dr["EngName"].ToString();

                        }

                        DataTable dtBankPaymentMaster = m_BankPayment.GetBankPaymentMaster(vouchID);


                        if (dtBankPaymentMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }


                        DataRow drBankPaymentMaster = dtBankPaymentMaster.Rows[0];
                        txtVchNo.Text = drBankPaymentMaster["Voucher_No"].ToString();
                        txtDate.Text = Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString());
                        txtRemarks.Text = drBankPaymentMaster["Remarks"].ToString();
                        txtBankID.Text = drBankPaymentMaster["BankPaymentID"].ToString();
                        dsBankPayment.Tables["tblBankPaymentMaster"].Rows.Add(cboSeriesName.Text, drBankPaymentMaster["Voucher_No"].ToString(), Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString()), cmboBankAccount.Text, drBankPaymentMaster["Remarks"].ToString());
                        DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drBankPaymentMaster["ProjectID"]), LangMgr.DefaultLanguage);
                        if (dtProjectInfo.Rows.Count > 0)
                        {
                            DataRow drProjectInfo = dtProjectInfo.Rows[0];
                            cboProjectName.Text = drProjectInfo["Name"].ToString();
                        }
                        //Show the corresponding Bank Account Ledger in Combobox
                        DataTable dtBankLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankPaymentMaster["LedgerID"]), LangMgr.DefaultLanguage);
                        DataRow drBankLedgerInfo = dtBankLedgerInfo.Rows[0];

                        cmboBankAccount.Text = drBankLedgerInfo["LedName"].ToString();
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

                                txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                txtfifth.Text = drBankPaymentMaster["Field5"].ToString();
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

                                txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                txtfifth.Text = drBankPaymentMaster["Field5"].ToString();
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

                                txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                txtfifth.Text = drBankPaymentMaster["Field5"].ToString();

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

                                txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                txtfifth.Text = drBankPaymentMaster["Field5"].ToString();

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

                                txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                                txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                                txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                                txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                                txtfifth.Text = drBankPaymentMaster["Field5"].ToString();
                            }


                        }

                        DataTable dtBankPaymentDetail = m_BankPayment.GetBankPaymentDetail(vouchID);


                        for (int i = 1; i <= dtBankPaymentDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtBankPaymentDetail.Rows[i - 1];

                            grdBankPayment[i, (int)GridColumn.Code_No].Value = i.ToString();
                            grdBankPayment[i, (int)GridColumn.Particular_Account_Head].Value = drDetail["LedgerName"].ToString();
                            grdBankPayment[i, (int)GridColumn.Amount].Value = drDetail["Amount"].ToString();
                            grdBankPayment[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                            grdBankPayment[i, (int)GridColumn.Cheque_No].Value = drDetail["ChequeNumber"].ToString();
                            grdBankPayment[i, (int)GridColumn.Ledger_ID].Value = drDetail["LedgerID"].ToString();
                            if (drDetail["ChequeDate"].ToString() == "")
                            {
                                grdBankPayment[i, (int)GridColumn.ChequeDate].Value = "";
                            }
                            else
                            {
                                grdBankPayment[i, (int)GridColumn.ChequeDate].Value = Date.DBToSystem(drDetail["ChequeDate"].ToString());
                            }
                            AddRowBankPayment(grdBankPayment.RowsCount);
                            dsBankPayment.Tables["tblBankPaymentDetails"].Rows.Add(drDetail["LedgerName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                        }


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

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdBankPayment.RowsCount - 2)
                grdBankPayment.Rows.Remove(ctx.Position.Row);
        }
     
        private void Account_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
        }

        private void Account_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;               
                frmLOVLedger frm = new frmLOVLedger(this);
                frm.ShowDialog();
                SendKeys.Send("{Tab}");
            }    
        }

        private void evtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            int ledgerID = 0;
            string CurrentBal = "";
            try
            {
                if (grdBankPayment[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)" || grdBankPayment[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString() == "")
                    return;
                try
                {
                    ledgerID = Convert.ToInt32(grdBankPayment[CurrRowPos, (int)GridColumn.Ledger_ID].Value);
                    CurrentBal = grdBankPayment[CurrRowPos, (int)GridColumn.Current_Bal_Actual].Value.ToString();
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
            FillAllGridRow(CurrRowPos, CurrAccLedgerID, CurrBal);
        }

        private void FillAllGridRow(int RowPosition, int LdrID, string CurrBalance)
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
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";                               
            }
            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";              
            }
            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";              
            }
            else
            {
                grdBankPayment[RowPosition, (int)GridColumn.Current_Balance].Value = CurrBalance;
            }
            grdBankPayment[RowPosition, (int)GridColumn.Ledger_ID].Value = LdrID;
            grdBankPayment[RowPosition, (int)GridColumn.Current_Bal_Actual].Value = CurrBalance;
            CurrAccLedgerID = 0;
            CurrBal = "";  
        }

        private void FillGridRowExceptLedgerID(int RowPosition, int LdrID, string CurrBal)
        {            
            decimal TempAmount = 0;
            string CurrentLedgerBalance = "";
            string[] CurrentBalance = new string[2];           

            if (CurrBal == "")
            {
                CurrentLedgerBalance = grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Bal_Actual].ToString();
                CurrentBalance = grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Bal_Actual].ToString().Split('(');
            }
            else
            {
                CurrentLedgerBalance = CurrBal.ToString();
                CurrentBalance = CurrBal.ToString().Split('(');
            }          
            try
            {
                TempAmount = Convert.ToDecimal(grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Amount].Value);
            }
            catch
            {
                TempAmount = 0;
            }
            //Call for Current amount change in cboBankAccount
            FilllblCurrentBankBalance();

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";                               
            }
            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";               
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {               
               //grdBankPayment[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";               
                grdBankPayment[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";               
            }
            else
            {
                grdBankPayment[RowPosition, (int)GridColumn.Current_Balance].Value = CurrBal;
            }
        }

        //it display current balance at the side of the bank account combobox
        private void FilllblCurrentBankBalance()
        {
            decimal TotalInputAmount = 0;
            decimal temporary = 0;
            string CurrentLedgerBalanceForBank = "";
            string[] CurrentBalanceForBank = new string[2];
            try
            {
                CurrentLedgerBalanceForBank = lblBankCurrBalHidden.Text;
                CurrentBalanceForBank = CurrentLedgerBalanceForBank.Split('(');
                //Get all the balance of the of the grid entry
                for (int i = 0; i < grdBankPayment.Rows.Count - 1; i++)
                {
                    TotalInputAmount += Convert.ToDecimal(grdBankPayment[i + 1, (int)GridColumn.Amount].Value);
                    lblTotalAmount.Text = TotalInputAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }
            }
            catch
            { }

            if (CurrentLedgerBalanceForBank.Contains("Cr"))
            {
                lblBankCurrentBalance.Text = (Convert.ToDecimal(CurrentBalanceForBank[0]) + Convert.ToDecimal(TotalInputAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }
            else if (CurrentLedgerBalanceForBank.Contains("Dr"))
            {
                temporary = (Convert.ToDecimal(CurrentBalanceForBank[0]) + (-1) * Convert.ToDecimal(TotalInputAmount));
                if (temporary == 0)
                    lblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                if (temporary < 0)
                {
                    lblBankCurrentBalance.Text = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (temporary > 0)
                {
                    lblBankCurrentBalance.Text = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
            }
            else
            { 
                lblBankCurrentBalance.Text = (TotalInputAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            }

        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmLOVLedger frm = new frmLOVLedger(this, e);
            frm.ShowDialog();  
        }

        private void ChequeDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = true;
            ctx1 = (SourceGrid.CellContext)sender;
            if (ctx1.DisplayText.ToString() != "")
            {
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Convert.ToDateTime(ctx1.Value))));
                frm.ShowDialog();
            }
            else
            {
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Date.GetServerDate())));
                frm.ShowDialog();
            }
        }       

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdBankPayment.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            string AccName = (string)(grdBankPayment[RowCount - 1, (int)GridColumn.Particular_Account_Head].Value);
            //Check if the input value is correct
            if (grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == "" || grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == null)
                grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            if (!Misc.IsNumeric(grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value))
            {
                Global.MsgError("Invalid Amount!");
                ctx.Value = "";
                return;
            }
            double checkformat = Convert.ToDouble(grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value.ToString());
            string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdBankPayment[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = insertvalue;
            FillGridRowExceptLedgerID(CurRow, CurrAccLedgerID, CurrBal);           
            if (AccName != "(NEW)")
            {
                AddRowBankPayment(RowCount);
            }

        }

        private void cmboBankAccount_IndexChanged(object sender, EventArgs e)
        {
            liBankID = (ListItem)cmboBankAccount.SelectedItem;

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
           // dtBankCurrBalance = Ledger.GetLedgerDetail("1", null, null, liBankID.ID);

            string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
            string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
            dtBankCurrBalance = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, liBankID.ID, null);

            if (dtBankCurrBalance.Rows.Count <= 0)
            {
                if (dtGetOpeningBalance.Rows.Count>0)
                {
                DataRow dropeningBal = dtGetOpeningBalance.Rows[0];
                if (dropeningBal["OpenBalDrCr"].ToString() == "DEBIT")
                {
                    lblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                    lblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                }
                else
                {
                    lblBankCurrentBalance.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                    lblBankCurrBalHidden.Text = (0 + Convert.ToDecimal(dropeningBal["OpenBal"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                }
                }
                else
                {
                lblBankCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblBankCurrBalHidden.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }
                FilllblCurrentBankBalance();
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
            lblBankCurrentBalance.Text = strBalance;
            lblBankCurrBalHidden.Text = Balance.ToString();
            // FilllblCurrentBankBalance();
               

        }

        /// <summary>
        /// Writes the header part for grdJournal
        /// </summary>
        private void AddGridHeader()
        {
            grdBankPayment[0,(int)GridColumn.Del] = new MyHeader("Del");
            grdBankPayment[0, (int)GridColumn.Del].Column.Width = 30;

            grdBankPayment[0, (int)GridColumn.Code_No] = new MyHeader("Code No.");
            grdBankPayment[0, (int)GridColumn.Code_No].Column.Width = 60;

            grdBankPayment[0, (int)GridColumn.Particular_Account_Head] = new MyHeader("Particular/Accounting Head");
            grdBankPayment[0, (int)GridColumn.Particular_Account_Head].Column.Width = 150;

            grdBankPayment[0, (int)GridColumn.Amount] = new MyHeader("Amount");
            grdBankPayment[0, (int)GridColumn.Amount].Column.Width = 100;

            grdBankPayment[0, (int)GridColumn.Current_Balance] = new MyHeader("CurrentBalance");
            grdBankPayment[0, (int)GridColumn.Current_Balance].Column.Width = 100;

            grdBankPayment[0, (int)GridColumn.Remarks] = new MyHeader("Remarks");
            grdBankPayment[0, (int)GridColumn.Remarks].Column.Width = 100;

            grdBankPayment[0, (int)GridColumn.Cheque_No] = new MyHeader("Cheque Number");
            grdBankPayment[0, (int)GridColumn.Cheque_No].Column.Width = 100;

            grdBankPayment[0, (int)GridColumn.ChequeDate] = new MyHeader("Cheque Date");
            grdBankPayment[0, (int)GridColumn.ChequeDate].Column.Width = 100;

            grdBankPayment[0, (int)GridColumn.Ledger_ID] = new MyHeader("LedgerID");
            grdBankPayment[0, (int)GridColumn.Ledger_ID].Column.Visible = false;

            grdBankPayment[0, (int)GridColumn.Current_Bal_Actual] = new MyHeader("CurrentBalance");
            grdBankPayment[0, (int)GridColumn.Current_Bal_Actual].Column.Visible = false;        

        }

        /// <summary>
        /// Adds the row in the Journal field
        /// </summary>
        private void AddRowBankPayment(int RowCount)
        {
            //Add a new row
            grdBankPayment.Redim(Convert.ToInt32(RowCount + 1), grdBankPayment.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;

            int i = RowCount;
            grdBankPayment[i, (int)GridColumn.Del] = btnDelete;
            grdBankPayment[i, (int)GridColumn.Del].AddController(evtDelete);

            grdBankPayment[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdBankPayment[i, (int)GridColumn.Particular_Account_Head] = new SourceGrid.Cells.Cell("", txtAccount);
            txtAccount.Control.GotFocus += new EventHandler(Account_Focused);
            txtAccount.Control.LostFocus += new EventHandler(Account_Leave);
            txtAccount.Control.KeyDown += new KeyEventHandler(Account_KeyDown);
            txtAccount.Control.TextChanged += new EventHandler(Text_Change);
            grdBankPayment[i, (int)GridColumn.Particular_Account_Head].AddController(evtAccountFocusLost);
            grdBankPayment[i, (int)GridColumn.Particular_Account_Head].Value = "(NEW)";   

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankPayment[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell("", txtAmount);
            txtAmount.Control.TextChanged += new EventHandler(Text_Change);
            grdBankPayment[i, (int)GridColumn.Amount].AddController(evtAmountFocusLost);
            grdBankPayment[i, (int)GridColumn.Amount].Value = "";          

            SourceGrid.Cells.Editors.TextBox txtCurrentBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrentBalance.EditableMode = SourceGrid.EditableMode.None;
            grdBankPayment[i, (int)GridColumn.Current_Balance] = new SourceGrid.Cells.Cell("", txtCurrentBalance);
            grdBankPayment[i, (int)GridColumn.Current_Balance].Value = ""; 

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankPayment[i, (int)GridColumn.Remarks] = new SourceGrid.Cells.Cell("", txtRemarks);
            txtRemarks.Control.TextChanged += new EventHandler(Text_Change); 

            SourceGrid.Cells.Editors.TextBox txtChequeNumber = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtChequeNumber.EditableMode = SourceGrid.EditableMode.Focus;
            grdBankPayment[i, (int)GridColumn.Cheque_No] = new SourceGrid.Cells.Cell("", txtChequeNumber);
            txtChequeNumber.Control.TextChanged += new EventHandler(Text_Change);

            SourceGrid.Cells.Button btnChequeDate = new SourceGrid.Cells.Button(""); //Date.ToSystem(DateTime.Today)
            txtChequeNumber.EditableMode = SourceGrid.EditableMode.SingleClick;
            //btnChequeDate.Controller.OnClick += new EventHandler(Text_Change);
            grdBankPayment[i, (int)GridColumn.ChequeDate] = btnChequeDate;
            grdBankPayment[i, (int)GridColumn.ChequeDate].AddController(evtChequeDateFocusLost);

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdBankPayment[i, (int)GridColumn.Ledger_ID] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdBankPayment[i, (int)GridColumn.Ledger_ID].Value = "";

            SourceGrid.Cells.Editors.TextBox txtCurrBal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrBal.EditableMode = SourceGrid.EditableMode.None;
            grdBankPayment[i, (int)GridColumn.Current_Bal_Actual] = new SourceGrid.Cells.Cell("", txtCurrBal);
            grdBankPayment[i, (int)GridColumn.Current_Bal_Actual].Value = "";  

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
                    IsFieldChanged = false;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    break;
            }
        }

        private void LoadSeriesNo()
        {
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("BANK_PMNT");
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

        private void EnableControls(bool Enable)
        {
            txtVchNo.Enabled = txtDate.Enabled = txtRemarks.Enabled = grdBankPayment.Enabled =cboSeriesName.Enabled=cmboBankAccount.Enabled= cboProjectName.Enabled=tabControl1.Enabled= Enable;
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
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
            if (cmboBankAccount.SelectedItem == null)
            {
                Global.MsgError("Invalid Bank Account Selected");
                cmboBankAccount.Focus();
                bValidate = false;
            }
             if (cboProjectName.SelectedItem == null)
            {
                Global.MsgError("Invalid Project Name Selected");
                cboProjectName.Focus();
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EnableControls(true);
            ChangeState(EntryMode.EDIT);
        }

        private void ClearVoucher()
        {
            ClearBankPayment();
            grdBankPayment.Redim(2,10);
            AddGridHeader(); //Write header part
            AddRowBankPayment(1);            
        }

        private void ClearBankPayment()
        {
            txtVchNo.Clear(); //actually generate a new voucher no.
           // txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtRemarks.Clear();
            grdBankPayment.Rows.Clear();
            foreach (ListItem lst in cmboBankAccount.Items)
            {
                if (lst.ID == Convert.ToInt32(Settings.GetSettings("DEFAULT_BANK_ACCOUNT")))
                {
                    cmboBankAccount.Text = lst.Value;
                    break;
                }
            }
            cboSeriesName.Text = string.Empty;
            cboProjectName.SelectedIndex = 0;         
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
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtBankID.Text);
                    if (BankPaymentIDCopy > 0)
                    {
                        VouchID = BankPaymentIDCopy;
                        BankPaymentIDCopy = 0;
                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtBankID.Text);                    
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
                    txtRemarks.Text = drBankPaymentMaster["Remarks"].ToString();
                    IsShortcutKey = false;
                    txtRemarks.SelectionStart = txtRemarks.Text.Length + 1;
                    return false;
                }                                                                       

                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                

                //Show the corresponding Cash Account Ledger in Combobox
                DataTable dtBankLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankPaymentMaster["LedgerID"]), LangMgr.DefaultLanguage);
                DataRow drBankLedgerInfo = dtBankLedgerInfo.Rows[0];

                cmboBankAccount.Text = drBankLedgerInfo["LedName"].ToString();

                //Show the Corresponding SeriesName in Combobox
                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drBankPaymentMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Bank Payment");
                    cboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();
                }
                txtVchNo.Text = drBankPaymentMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString());
                txtRemarks.Text = drBankPaymentMaster["Remarks"].ToString();

                DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drBankPaymentMaster["ProjectID"]), LangMgr.DefaultLanguage);
                if (dtProjectInfo.Rows.Count != 0)
                {
                    DataRow drProjectInfo = dtProjectInfo.Rows[0];
                    cboProjectName.Text = drProjectInfo["Name"].ToString();                  
                    txtBankID.Text = drBankPaymentMaster["BankPaymentID"].ToString();
                    dsBankPayment.Tables["tblBankPaymentMaster"].Rows.Add(cboSeriesName.Text, drBankPaymentMaster["Voucher_No"].ToString(), Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString()), cmboBankAccount.Text, drBankPaymentMaster["Remarks"].ToString(), drProjectInfo["Name"].ToString());
                }
                else
                {
                    cboProjectName.Text ="None";                  
                    txtBankID.Text = drBankPaymentMaster["BankPaymentID"].ToString();
                    dsBankPayment.Tables["tblBankPaymentMaster"].Rows.Add(cboSeriesName.Text, drBankPaymentMaster["Voucher_No"].ToString(), Date.DBToSystem(drBankPaymentMaster["BankPayment_Date"].ToString()), cmboBankAccount.Text, drBankPaymentMaster["Remarks"].ToString(),"None");
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

                        txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        txtfifth.Text = drBankPaymentMaster["Field5"].ToString();
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

                        txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        txtfifth.Text = drBankPaymentMaster["Field5"].ToString();
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

                        txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        txtfifth.Text = drBankPaymentMaster["Field5"].ToString();

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

                        txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        txtfifth.Text = drBankPaymentMaster["Field5"].ToString();

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

                        txtfirst.Text = drBankPaymentMaster["Field1"].ToString();
                        txtsecond.Text = drBankPaymentMaster["Field2"].ToString();
                        txtthird.Text = drBankPaymentMaster["Field3"].ToString();
                        txtfourth.Text = drBankPaymentMaster["Field4"].ToString();
                        txtfifth.Text = drBankPaymentMaster["Field5"].ToString();
                    }


                }
                DataTable dtBankPaymentDetail = m_BankPayment.GetBankPaymentDetail(Convert.ToInt32(txtBankID.Text));
                for (int i = 1; i <= dtBankPaymentDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtBankPaymentDetail.Rows[i - 1];
                    grdBankPayment[i, (int)GridColumn.Code_No].Value = i.ToString();
                    grdBankPayment[i, (int)GridColumn.Particular_Account_Head].Value = drDetail["LedgerName"].ToString();
                    grdBankPayment[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdBankPayment[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                    grdBankPayment[i, (int)GridColumn.Cheque_No].Value = drDetail["ChequeNumber"].ToString();
                    grdBankPayment[i, (int)GridColumn.Ledger_ID].Value = drDetail["ledgerid"].ToString();
                    if (drDetail["ChequeDate"].ToString() == "")
                    {
                        grdBankPayment[i, (int)GridColumn.ChequeDate].Value = "";
                    }
                    else
                    {
                        grdBankPayment[i, (int)GridColumn.ChequeDate].Value = Date.DBToSystem(drDetail["ChequeDate"].ToString());
                    }
                    //Code To Get The Current Balance of the Respective Ledger
                    string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
                    string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
                    DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(drDetail["LedgerID"]), null);
                    if (dtLdrInfo.Rows.Count != 1)
                    {
                        grdBankPayment[i, (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        grdBankPayment[i, (int)GridColumn.Current_Bal_Actual].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
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
                        grdBankPayment[i, (int)GridColumn.Current_Balance].Value = strBalance;
                        grdBankPayment[i, (int)GridColumn.Current_Bal_Actual].Value = Balance.ToString();

                       
                    }
                    AddRowBankPayment(grdBankPayment.RowsCount);
                    dsBankPayment.Tables["tblBankPaymentDetails"].Rows.Add(drDetail["LedgerName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());//),drDetail["ChequeNumber"].ToString(),Convert.ToDateTime(drDetail["ChequeDate"]));
                    totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                    totalRptAmt = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();
                }

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtBankID.Text), "BANK_PMNT");
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
            if (!ContinueWithoutSaving())
            {
                return;
            }           
            Navigation(Navigate.First);
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
            if (!ContinueWithoutSaving())
            {
                return;
            }
            Navigation(Navigate.Prev);
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
            if (!ContinueWithoutSaving())
            {
                return;
            }
            Navigation(Navigate.Next);
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
            if (!ContinueWithoutSaving())
            {
                return;
            }
            Navigation(Navigate.Last);
            IsFieldChanged = false;         
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            BankPaymentIDCopy = Convert.ToInt32(txtBankID.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (BankPaymentIDCopy > 0)
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
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
                cmboBankAccount.Focus();
            }
        }

        private void cmboBankAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboProjectName.Focus();
            }
        }

        private void cboProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                grdBankPayment.Focus();
            }
        }

        private void txtRemarks_KeyDown(object sender, KeyEventArgs e)
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
                btnEdit_Click(sender, e);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                btnSave_Click_1(sender, e);
            }
            else if (e.KeyCode == Keys.Delete && e.Control)
            {
                btnDelete_Click_1(sender, e);
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

        private void btnNew_Click_1(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }

            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
                    
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            try
            {
                if (!String.IsNullOrEmpty(txtBankID.Text))
                {
                    if (Global.MsgQuest("Are you sure you want to delete the Bank Payment - " + txtBankID.Text + "?") == DialogResult.Yes)
                    {
                        BankPayment m_BankPayment = new BankPayment();
                        if (m_BankPayment.RemoveBankPaymentEntry(Convert.ToInt32(txtBankID.Text)))
                        {
                            Global.Msg("Bank Payment -" + txtBankID.Text + " deleted successfully!");
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
                            Global.MsgError("There was an error while deleting Bank Payment -" + txtBankID.Text + "!");
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void btnEdit_Click_2(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            if (!ContinueWithoutSaving())
            {
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            if (txtBankID.Text.Length <= 0)
            {
                Global.MsgError("Please navigate to existing Bank entry first and then try again!");
                return;
            }
            isNew = false;
            OldGrid = " ";
            OldGrid = OldGrid + "Voucher No" + txtVchNo.Text + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Bank" + cmboBankAccount.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdBankPayment[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                string amt = grdBankPayment[i + 1, (int)GridColumn.Amount].Value.ToString();
                OldGrid = OldGrid + string.Concat(particular, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;

            EnableControls(true);
            ChangeState(EntryMode.EDIT);         
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_PAYMENT_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your Administrator for permission.");
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
                DataRow drVouConfigInfo = dtVouConfigInfo.Rows[0];
                if (dtVouConfigInfo.Rows.Count < 0)
                    return;
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

            ListItem liLedgerID = new ListItem();
            liLedgerID = (ListItem)cmboBankAccount.SelectedItem;

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
            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    try
                    {
                        dueDate();
                        Forms.frmDueDate dueDate1 = new Forms.frmDueDate(this, dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            dueDate1.ShowDialog();
                        }
                        isNew = true;
                        NewGrid = " ";
                        OldGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + txtVchNo.Text + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Bank" + cmboBankAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdBankPayment[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                            string amt = grdBankPayment[i + 1, (int)GridColumn.Amount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;

                        //Call to Convert into XML Format
                        string BankPaymentXMLString = ReadAllBankPaymentEntry();
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
                            string[] ledgerName = grdBankPayment[i + 1, (int)GridColumn.Particular_Account_Head].ToString().Split('[');
                            if (grdBankPayment[i + 1, (int)GridColumn.Remarks].Value == "")
                            {
                                grdBankPayment[i + 1, (int)GridColumn.Cheque_No].Value = "";
                            }

                            BankPaymentDetails.Rows.Add(ledgerName[0].ToString(), grdBankPayment[i + 1, (int)GridColumn.Amount].Value, grdBankPayment[i + 1, (int)GridColumn.Current_Balance].Value, grdBankPayment[i + 1, (int)GridColumn.Remarks].Value, grdBankPayment[i + 1, (int)GridColumn.Cheque_No].Value);
                            //Check for empty Amount
                            object objAmount = grdBankPayment[i + 1, (int)GridColumn.Amount].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdBankPayment[i + 1, (int)GridColumn.Amount].Value);
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
                                    //In case of Bank Payment,Bank Account in master section is Credit and other accounts in Detail section are bydefault Debit
                                    totalCrCash = mCbalCash;//In case of detail section,all ledger is posted as Debit...soo credit amount of ledger is its own amount in Transaction.
                                    totalDrCash += (mDbalCash + Convert.ToDouble(grdBankPayment[i + 1, 3].Value));//The Ledgers of Details section will be posted as Debit,soo total balance will be the summation of its own balance and grid value
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
                                    int bankLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.Language);
                                    Transaction.GetLedgerBalance(bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode);
                                    //In case of Bank Payment,Bank Account in master section is Credit and other accounts in Detail section are bydefault Debit
                                    totalCrBank = mCbalBank;//Ledgers of Details sections will be posted as Debit,so Credit balance will its only own balance
                                    totalDrBank += (mDbalBank + Convert.ToDouble(grdBankPayment[i + 1, 3].Value));

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
                            */
                            #endregion
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
                            Transaction.GetLedgerBalance(null, null, Convert.ToInt32(liLedgerID.ID), ref mDbalBank, ref mCbalBank, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero

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
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlBankPaymentInsert", Global.m_db.cn))
                            {
                                // we are going to use store procedure  
                                dbCommand.CommandType = CommandType.StoredProcedure;
                                // Add input parameter and set its properties.
                                SqlParameter parameter = new SqlParameter();
                                // Store procedure parameter name  
                                parameter.ParameterName = "@bankpayment";
                                // Parameter type as XML 
                                parameter.DbType = DbType.Xml;
                                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                parameter.Value = BankPaymentXMLString; // XML string as parameter value  
                                // Add the parameter in Parameters collection.
                                dbCommand.Parameters.Add(parameter);
                                Global.m_db.cn.Open();
                                int intRetValue = dbCommand.ExecuteNonQuery();                                                            
                            }
                        }
                        #endregion

                        Global.Msg("Bank payment created successfully!");
                        AccClassID.Clear();
                        ClearVoucher();
                        //ChangeState(EntryMode.NEW);
                        //btnNew_Click_1(sender, e);

                        #region Bank Payment Create Delete After Successful integration
                        //DateTime BankPayment_Date = Date.ToDotNet(txtDate.Text);
                        //ListItem liProjectID = new ListItem();
                        //liProjectID = (ListItem)cboProjectName.SelectedItem;
                        //ListItem LiSeriesID = new ListItem();
                        //LiSeriesID = (ListItem)cboSeriesName.SelectedItem;
                        //if (AccClassID.Count != 0)
                        //{
                        //    //  m_BankPayment.Create(Convert.ToInt32(LiSeriesID.ID), liLedgerID.ID, txtVchNo.Text, BankPayment_Date, txtRemarks.Text, BankPaymentDetails, AccClassID.ToArray(), liProjectID.ID, txtNarration.Text, txtAmtInWords.Text, txtCashChequeSpecify.Text);
                            //m_BankPayment.Create(Convert.ToInt32(LiSeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, BankPayment_Date, txtRemarks.Text, BankPaymentDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID));
                        //}
                        //else
                        //{
                        //    int[] a = new int[] { 1 };
                        //    m_BankPayment.Create(Convert.ToInt32(LiSeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, BankPayment_Date, txtRemarks.Text, BankPaymentDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID));

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
                        dueDate();
                        Forms.frmDueDate dueDate1 = new Forms.frmDueDate(this, dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            dueDate1.ShowDialog();
                        }
                        isNew = false;
                        NewGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + txtVchNo.Text + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text + "Bank" + cmboBankAccount.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdBankPayment.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdBankPayment[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                            string amt = grdBankPayment[i + 1, (int)GridColumn.Amount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;

                        //Call to Convert into XML Format
                        string BankPaymentXMLString = ReadAllBankPaymentEntry();
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
                            string[] ledgerName = grdBankPayment[i + 1, (int)GridColumn.Particular_Account_Head].ToString().Split('[');
                            if (grdBankPayment[i + 1, (int)GridColumn.Remarks].Value == "")
                            {
                                grdBankPayment[i + 1, (int)GridColumn.Cheque_No].Value = "";
                            }
                            BankPaymentDetails.Rows.Add(ledgerName[0].ToString(), grdBankPayment[i + 1, (int)GridColumn.Amount].Value, grdBankPayment[i + 1, (int)GridColumn.Current_Balance].Value, grdBankPayment[i + 1, (int)GridColumn.Remarks].Value, grdBankPayment[i + 1, (int)GridColumn.Cheque_No].Value);
                            //Check for empty Amount
                            object objAmount = grdBankPayment[i + 1, (int)GridColumn.Amount].Value;
                            if (objAmount == null)
                            {
                                MessageBox.Show("Amount must not be null!!");
                                return;
                            }
                            try
                            {
                                totalgrdAmount += Convert.ToDouble(grdBankPayment[i + 1, (int)GridColumn.Amount].Value);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            //Generally in this section no need to check negative cash and negative bank because all ledgeres associated in detai section are receiver...they will get amount from master section.
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
                                    //In case of Bank Payment,Bank Account in master section is Credit and other accounts in Detail section are bydefault Debit
                                    totalCrCash = mCbalCash;//In case of detail section,all ledger is posted as Debit...soo credit amount of ledger is its own amount in Transaction.
                                    totalDrCash += (mDbalCash + Convert.ToDouble(grdBankPayment[i + 1, 3].Value));//The Ledgers of Details section will be posted as Debit,soo total balance will be the summation of its own balance and grid value
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
                                    int bankLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.Language);
                                    Transaction.GetLedgerBalance(bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode);
                                    //In case of Bank Payment,Bank Account in master section is Credit and other accounts in Detail section are bydefault Debit
                                    totalCrBank = mCbalBank;//Ledgers of Details sections will be posted as Debit,so Credit balance will its only own balance
                                    totalDrBank += (mDbalBank + Convert.ToDouble(grdBankPayment[i + 1, 3].Value));

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
                            */
                            #endregion
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
                            Transaction.GetLedgerBalance(null, null, Convert.ToInt32(liLedgerID.ID), ref mDbalBank, ref mCbalBank, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero

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


                        BankPayment m_BankPayment = new BankPayment();
                        if (!m_BankPayment.RemoveBankPaymentEntry(Convert.ToInt32(txtBankID.Text)))
                        {
                            MessageBox.Show("Unable to Update record. Please restart the modification");
                            return;
                        }

                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlBankPaymentInsert", Global.m_db.cn))
                            {
                                // we are going to use store procedure  
                                dbCommand.CommandType = CommandType.StoredProcedure;
                                // Add input parameter and set its properties.
                                SqlParameter parameter = new SqlParameter();
                                // Store procedure parameter name  
                                parameter.ParameterName = "@bankpayment";
                                // Parameter type as XML 
                                parameter.DbType = DbType.Xml;
                                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                parameter.Value = BankPaymentXMLString; // XML string as parameter value  
                                if (Global.m_db.cn.State == ConnectionState.Open)
                                {
                                    Global.m_db.cn.Close();
                                }
                                // Add the parameter in Parameters collection.
                                dbCommand.Parameters.Add(parameter);
                                Global.m_db.cn.Open();
                                int intRetValue = dbCommand.ExecuteNonQuery();
                            }
                        }
                        #endregion

                        if (!Validate())
                            return;
                        Global.Msg("Bank payment modified successfully!");
                        AccClassID.Clear();
                        ClearVoucher();
                        ChangeState(EntryMode.NORMAL);
                        btnNew_Click_1(sender, e);

                        if (chkPrntWhileSaving.Checked)
                        {
                            prntDirect = 1;
                            Navigation(Navigate.Last);
                            btnPrint_Click(sender, e);
                            ClearVoucher();
                            ChangeState(EntryMode.NEW);
                            btnNew_Click_1(sender, e);
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

        /// <summary>
        /// Read all journal Entry
        /// </summary>
        /// <returns></returns>
        private string ReadAllBankPaymentEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            SeriesID = (ListItem)cboSeriesName.SelectedItem;
            liProjectID = (ListItem)cboProjectName.SelectedItem;
            liBankID = (ListItem)cmboBankAccount.SelectedItem;

            //validate grid entry
            if (!ValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();

            #region  Bank Payment
            tw.WriteStartElement("BANKPAYMENT");
            {
                ///For Bank Payment Master Section
                ///int JournalID = System.Convert.ToInt32(9); // Auto increment  
                ///
                string first = txtfirst.Text;
                string second = txtsecond.Text;
                string third = txtthird.Text;
                string fourth = txtfourth.Text;
                string fifth = txtfifth.Text;
                int SID = System.Convert.ToInt32(SeriesID.ID);
                int LedgerID = System.Convert.ToInt32(liBankID.ID);
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                string BankID = System.Convert.ToString(txtBankID.Text);
                DateTime BankPayment_Date = Date.ToDotNet(txtDate.Text);
                string Remarks = System.Convert.ToString(txtRemarks.Text);
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
                    BankID = System.Convert.ToString(txtBankID.Text);
                    LedgerID1 = System.Convert.ToInt32(grdBankPayment[i + 1, (int)GridColumn.Ledger_ID].Value);
                    Amount = System.Convert.ToDecimal(grdBankPayment[i + 1, (int)GridColumn.Amount].Value);
                    RemarksDetail = System.Convert.ToString(grdBankPayment[i + 1, (int)GridColumn.Remarks].Value);
                    ChequeNumber = System.Convert.ToString(grdBankPayment[i + 1, (int)GridColumn.Cheque_No].Value);
                    if (ChequeNumber == "")
                    {
                        ChequeDate = "";
                        ChequeDateValue = Date.GetServerDate();
                    }
                    else
                    {
                        ChequeDate = System.Convert.ToString(grdBankPayment[i + 1, (int)GridColumn.ChequeDate].Value);
                        ChequeDateValue = Date.ToDotNet(ChequeDate);
                    }

                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("BankPaymentID", BankID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID1.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());                  
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteElementString("ChequeNumber", ChequeNumber.ToString());
                    if(ChequeDate != "")
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
        private bool ValidateGrid()
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
                        tempValue = System.Convert.ToInt32(grdBankPayment[i + 1, (int)GridColumn.Ledger_ID].Value);
                    }
                    catch (Exception ex)
                    {
                        tempValue = 0;
                    }
                    try
                    {
                        tempDecValue = System.Convert.ToDecimal(grdBankPayment[i + 1, (int)GridColumn.Amount].Value);
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
                        if (i + 2 == grdBankPayment.Rows.Count && grdBankPayment[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)")
                        {
                            //Do Nothing
                        }
                        else
                            return false;
                    }
                    else
                        LdrID[i] = tempValue;

                    if (i + 2 == grdBankPayment.Rows.Count && grdBankPayment[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)")
                    {
                        //Donothing
                    }
                    else
                        Amt[i] = tempDecValue;

                    liBankID = (ListItem)cmboBankAccount.SelectedItem;
                    if (LdrID.Contains(liBankID.ID))
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

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void cboSeriesName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            OptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    SeriesID = (ListItem)cboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));

                    //if (txtVchNo.Text == "")
                    //{
                    //    txtVchNo.Text = "Main";
                    //    txtVchNo.Enabled = false;
                    //}

                    if (NumberingType == "AUTOMATIC")
                    {
                        txtVchNo.Enabled = true;
                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            //disable all the controls except cboSeriesName

                            return;
                        }
                        txtVchNo.Text = m_vounum.ToString();
                        txtVchNo.Enabled = false;
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

        private void btnDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = false;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }

      
        private void frmBankPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
          
            //if (!ContinueWithoutSaving())
            //{
            //    e.Cancel = true;
            //}
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

        private void btn_groupvoucherposting_Click(object sender, EventArgs e)
        {
            BankPayment bpayment = new BankPayment();
            int rowid = bpayment.GetRowID(txtVchNo.Text);
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
                dtbulkvoucher.Rows.Add(grdBankPayment[i + 1, (int)GridColumn.Particular_Account_Head].Value, "Debit", grdBankPayment[i + 1, (int)GridColumn.Amount].Value, grdBankPayment[i + 1, (int)GridColumn.Ledger_ID].Value, "BANK_PMNT", txtVchNo.Text, grdBankPayment[i + 1, (int)GridColumn.Remarks].Value);
            }
            Inventory.Forms.Accounting.frmGroupVoucherList fgl = new Forms.Accounting.frmGroupVoucherList(dtbulkvoucher, SID, ProjectID, rowid);
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
        private void dueDate()
        {
            dt1.Rows.Clear();
            dt1.Columns.Clear();
            dt1.Columns.Add("LedgerId");
            dt1.Columns.Add("LedgerName");

            for (int i = 1; i <= grdBankPayment.Rows.Count() - 2; i++)
            {
                int ledgerId = Convert.ToInt32(grdBankPayment[i, (int)GridColumn.Ledger_ID].Value);
                int GroupID = AccountGroup.GetGroupIDByLedgerID(ledgerId);
                if (GroupID == 29)
                {
                    dt1.Rows.Add(grdBankPayment[i, (int)GridColumn.Ledger_ID].Value, grdBankPayment[i, (int)GridColumn.Particular_Account_Head].Value);
                }

            }

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
            dsBankPayment.Clear();
            totalAmt = 0;
            rptBankPayment rpt = new rptBankPayment();
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

            bool empty = Navigation(Navigate.ID);
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
}
