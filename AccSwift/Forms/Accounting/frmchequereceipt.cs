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
using Inventory.DataSet;
using Inventory.CrystalReports;
using DateManager;
using ErrorManager;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using Inventory.Forms;
using CrystalDecisions.Shared;
using BusinessLogic.Accounting;
using Common;
namespace Inventory.Forms.Accounting
{
    public partial class frmchequereceipt : Form, IfrmDateConverter, ILOVLedger, IfrmAddAccClass
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield; 
        private int prntDirect = 0;
        private int BankReceiptIDCopy = 0;
        decimal totalAmt = 0;
        string totalRptAmt = "";
        ChequeReceipt m_ChequeReceipt = new ChequeReceipt();
        private int m_ChequeReceiptID;
        private dsChequeReceipt dsChequeReceipt = new dsChequeReceipt();
        BusinessLogic.Accounting.ChequeReceipt m_chequeReceipt = new BusinessLogic.Accounting.ChequeReceipt();
        private bool IsChequeDateButton = false;
        private bool IsChequeCashDateButton = false;
        private int OnlyReqdDetailRows = 0;
         private bool IsNegativeCash = false;
         private bool IsNegativeBank = false;
        SourceGrid.CellContext ctx1 = new SourceGrid.CellContext();
        SourceGrid.CellContext ctx2 = new SourceGrid.CellContext();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        ArrayList AccountClassID = new ArrayList();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        private bool IsFieldChanged = false;
        ListItem liProjectID = new ListItem();
        ListItem liBankID = new ListItem();
        private bool hasChanged = false;
        List<int> AccClassID = new List<int>();
        private bool IsShortcutKey = false;
        ListItem SeriesID = new ListItem();
        private string Prefix = "";
        private int loopCounter = 0;
        private int CurrAccLedgerID = 0;
        private string CurrBal = "";
        private int CurrRowPos = 0;
        ContextMenu Menu_Export;
       
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
            Del = 0, Code_No, Particular_Account_Head,Dr_Cr, Amount, Current_Balance, Remarks, Cheque_No, ChequeBank, ChequeDate, Ledger_ID, Current_Bal_Actual, ChequeCash_Date
        };

        DevAge.Windows.Forms.DevAgeTextBox ctx;
        double m_DebitAmount = 0;//Holds the total debit amount on the Voucher
        double m_CreditAmount = 0;
         private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        DataTable dtAccClassID = new DataTable();
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtChequeNumberFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtChequeBankFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtChequeDateFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtChequeCashDateFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtcboDrCrSelectedIndexChanged = new SourceGrid.Cells.Controllers.CustomEvents();
        public frmchequereceipt()
        {
            InitializeComponent();
        }
       
        public frmchequereceipt(int ChequeReceiptID)
        {
            InitializeComponent();
            this.m_ChequeReceiptID = ChequeReceiptID;
        }
        private void frmchequereceipt_Load(object sender, EventArgs e)
        {
            chkDoNotClose.Checked = true;
            ChangeState(EntryMode.NEW);
            //ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            ShowAccClassInTreeView(treeAccClass, null, 0);
            Transaction m_Transaction = new Transaction();
            m_mode = EntryMode.NEW;
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.ToSystem(Date.GetServerDate());
            //For Loading The Optional Fields
            OptionalFields();
            try
            {
                #region Customevents mainly for saving purpose
                cboSeriesName.SelectedIndexChanged += new EventHandler(Text_Change);
                comboBankAccount.SelectedIndexChanged += new EventHandler(Text_Change);
                //comboBankAccount.SelectedIndexChanged += new EventHandler(cmboBankAccount_IndexChanged);
                txtVchNo.TextChanged += new EventHandler(Text_Change);
                txtDate.TextChanged += new EventHandler(Text_Change);
                btnDate.Click += new EventHandler(Text_Change);
                cboProjectName.SelectedIndexChanged += new EventHandler(Text_Change);
                txtRemarks.TextChanged += new EventHandler(Text_Change);

                //Event trigerred when delete button is clicked
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                evtAmountFocusLost.FocusLeft += new EventHandler(Amount_Focus_Lost);
                evtAccountFocusLost.FocusLeft += new EventHandler(evtAccountFocusLost_FocusLeft);
                evtChequeDateFocusLost.Click += new EventHandler(ChequeDate_Click);
                evtChequeCashDateFocusLost.Click += new EventHandler(ChequeCashDate_Click);
                evtChequeDateFocusLost.Click += new EventHandler(Text_Change);
                evtcboDrCrSelectedIndexChanged.ValueChanged += new EventHandler(evtDrCr_Changed);
                #endregion
                #region Load Selected bank as per user
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                foreach (DataRow drBankLedgers in dtBankLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    comboBankAccount.Items.Add(new ListItem((int)drBankLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                comboBankAccount.DisplayMember = "value";//This value is  for showing at Load condition
                comboBankAccount.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());
                foreach (ListItem lst in comboBankAccount.Items)
                {
                    if (roleid == 37)
                    {
                        if (lst.ID == Convert.ToInt32(Settings.GetSettings("DEFAULT_BANK_ACCOUNT")))
                        {

                            comboBankAccount.Text = lst.Value;
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

                            comboBankAccount.Text = lst.Value;
                            // MessageBox.Show(IsFieldChanged.ToString());
                            IsFieldChanged = false;
                            break;
                        }
                    }
                }
                #endregion
                grdchequereceipt.Redim(2, 13);
                btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                AddGridHeader();
                AddRowChequeReceipt(1);

               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }
        private void AddGridHeader()
        {
            grdchequereceipt[0, (int)GridColumn.Del] = new MyHeader("Del");
            grdchequereceipt[0, (int)GridColumn.Del].Column.Width = 30;

            grdchequereceipt[0, (int)GridColumn.Code_No] = new MyHeader("Code No.");
            grdchequereceipt[0, (int)GridColumn.Code_No].Column.Width = 80;

            grdchequereceipt[0, (int)GridColumn.Particular_Account_Head] = new MyHeader("Particular/Accounting Head");
            grdchequereceipt[0, (int)GridColumn.Particular_Account_Head].Column.Width = 190;

            grdchequereceipt[0, (int)GridColumn.Dr_Cr] = new MyHeader("Dr/Cr");
            grdchequereceipt[0, (int)GridColumn.Dr_Cr].Column.Width = 60;
            grdchequereceipt[0, (int)GridColumn.Dr_Cr].Column.Visible = false;

            grdchequereceipt[0, (int)GridColumn.Amount] = new MyHeader("Amount");
            grdchequereceipt[0, (int)GridColumn.Amount].Column.Width = 100;

            grdchequereceipt[0, (int)GridColumn.Current_Balance] = new MyHeader("CurrentBalance");
            grdchequereceipt[0, (int)GridColumn.Current_Balance].Column.Width = 120;
            grdchequereceipt[0, (int)GridColumn.Current_Balance].Column.Visible = false;

            grdchequereceipt[0, (int)GridColumn.Remarks] = new MyHeader("Remarks");
            grdchequereceipt[0, (int)GridColumn.Remarks].Column.Width = 100;

            grdchequereceipt[0, (int)GridColumn.Cheque_No] = new MyHeader("Cheque Number");
            grdchequereceipt[0, (int)GridColumn.Cheque_No].Column.Width = 150;

            grdchequereceipt[0, (int)GridColumn.ChequeBank] = new MyHeader("Cheque Bank");
            grdchequereceipt[0, (int)GridColumn.ChequeBank].Column.Width = 100;

            grdchequereceipt[0, (int)GridColumn.ChequeDate] = new MyHeader("Cheque Date");
            grdchequereceipt[0, (int)GridColumn.ChequeDate].Column.Width = 100;

            grdchequereceipt[0, (int)GridColumn.Ledger_ID] = new MyHeader("LedgerID");
            grdchequereceipt[0, (int)GridColumn.Ledger_ID].Column.Visible = false;

            grdchequereceipt[0, (int)GridColumn.Current_Bal_Actual] = new MyHeader("CurrentBalance");
            grdchequereceipt[0, (int)GridColumn.Current_Bal_Actual].Column.Visible = false;

            grdchequereceipt[0, (int)GridColumn.ChequeCash_Date] = new MyHeader("Cheque-CashDate");
            grdchequereceipt[0, (int)GridColumn.ChequeCash_Date].Column.Width = 120;

            
        }
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

        private void btnDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = false;
            IsChequeCashDateButton = false;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }
        public void DateConvert(DateTime DotNetDate)
        {
            if (!IsChequeDateButton && !IsChequeCashDateButton)
                txtDate.Text = Date.ToSystem(DotNetDate);
            if (IsChequeDateButton && !IsChequeCashDateButton)
                ctx1.Value = Date.ToSystem(DotNetDate);
            if (IsChequeCashDateButton && !IsChequeDateButton)
                ctx2.Value = Date.ToSystem(DotNetDate);
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
        private void EnableControls(bool Enable)
        {
            txtVchNo.Enabled = txtDate.Enabled = txtRemarks.Enabled =  Enable = cboSeriesName.Enabled = comboBankAccount.Enabled = cboProjectName.Enabled = tcchequereceipt.Enabled = Enable;
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
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
        private void LoadSeriesNo()
        {
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("CHEQUERCPT");
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
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));

                    if (NumberingType == "AUTOMATIC")
                    {

                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
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

        private void AddRowChequeReceipt(int RowCount)
        {
            //Add a new row
            grdchequereceipt.Redim(Convert.ToInt32(RowCount + 1), grdchequereceipt.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            grdchequereceipt[i, (int)GridColumn.Del] = btnDelete;
            grdchequereceipt[i, (int)GridColumn.Del].AddController(evtDelete);

            grdchequereceipt[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdchequereceipt[i, (int)GridColumn.Particular_Account_Head] = new SourceGrid.Cells.Cell("", txtAccount);
            txtAccount.Control.GotFocus += new EventHandler(Account_Focused);
            txtAccount.Control.LostFocus += new EventHandler(Account_Leave);
            txtAccount.Control.KeyDown += new KeyEventHandler(Account_KeyDown);
            txtAccount.Control.TextChanged += new EventHandler(Text_Change);

            grdchequereceipt[i, (int)GridColumn.Particular_Account_Head].AddController(evtAccountFocusLost);
            grdchequereceipt[i, (int)GridColumn.Particular_Account_Head].Value = "(NEW)";

            SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
            cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;
            string strDrCr = "Debit";
            if (grdchequereceipt[i - 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
                strDrCr = "Credit";
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
            grdchequereceipt[i, (int)GridColumn.Dr_Cr] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);
            cboDrCr.Control.SelectedIndexChanged += new EventHandler(Text_Change);
            grdchequereceipt[i, (int)GridColumn.Dr_Cr].AddController(evtcboDrCrSelectedIndexChanged);


            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdchequereceipt[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell("", txtAmount);
            txtAmount.Control.TextChanged += new EventHandler(Text_Change);
            grdchequereceipt[i, (int)GridColumn.Amount].AddController(evtAmountFocusLost);
            grdchequereceipt[i, (int)GridColumn.Amount].Value = "";

            SourceGrid.Cells.Editors.TextBox txtCurrentBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrentBalance.EditableMode = SourceGrid.EditableMode.None;
            grdchequereceipt[i, (int)GridColumn.Current_Balance] = new SourceGrid.Cells.Cell("", txtCurrentBalance);
            grdchequereceipt[i, (int)GridColumn.Current_Balance].Value = "";

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdchequereceipt[i, (int)GridColumn.Remarks] = new SourceGrid.Cells.Cell("", txtRemarks);
            txtRemarks.Control.TextChanged += new EventHandler(Text_Change);

            SourceGrid.Cells.Editors.TextBox txtChequeNumber = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtChequeNumber.EditableMode = SourceGrid.EditableMode.Focus;
            grdchequereceipt[i, (int)GridColumn.Cheque_No] = new SourceGrid.Cells.Cell("", txtChequeNumber);
            txtChequeNumber.Control.TextChanged += new EventHandler(Text_Change);
            grdchequereceipt[i, (int)GridColumn.Cheque_No].AddController(evtChequeNumberFocusLost);
            grdchequereceipt[i, (int)GridColumn.Cheque_No].Value = "";

            SourceGrid.Cells.Editors.TextBox txtChequeBank = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtChequeBank.EditableMode = SourceGrid.EditableMode.Focus;
            grdchequereceipt[i, (int)GridColumn.ChequeBank] = new SourceGrid.Cells.Cell("", txtChequeBank);
            txtChequeBank.Control.TextChanged += new EventHandler(Text_Change);
            grdchequereceipt[i, (int)GridColumn.ChequeBank].AddController(evtChequeBankFocusLost);
            grdchequereceipt[i, (int)GridColumn.ChequeBank].Value = "";

            SourceGrid.Cells.Button btnChequeDate = new SourceGrid.Cells.Button(""); //Date.ToSystem(DateTime.Today)
            txtChequeNumber.EditableMode = SourceGrid.EditableMode.SingleClick;
            grdchequereceipt[i, (int)GridColumn.ChequeDate] = btnChequeDate;
            grdchequereceipt[i, (int)GridColumn.ChequeDate].AddController(evtChequeDateFocusLost);

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdchequereceipt[i, (int)GridColumn.Ledger_ID] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdchequereceipt[i, (int)GridColumn.Ledger_ID].Value = "";

            SourceGrid.Cells.Editors.TextBox txCurrBal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txCurrBal.EditableMode = SourceGrid.EditableMode.None;
            grdchequereceipt[i, (int)GridColumn.Current_Bal_Actual]= new SourceGrid.Cells.Cell("", txCurrBal);
            grdchequereceipt[i, (int)GridColumn.Current_Bal_Actual].Value = "";

            SourceGrid.Cells.Button btnChequeCashDate = new SourceGrid.Cells.Button(""); //Date.ToSystem(DateTime.Today)
            txtChequeNumber.EditableMode = SourceGrid.EditableMode.SingleClick;
            grdchequereceipt[i, (int)GridColumn.ChequeCash_Date] = btnChequeCashDate;
            grdchequereceipt[i, (int)GridColumn.ChequeCash_Date].AddController(evtChequeCashDateFocusLost);

        }
        private void Text_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
        }
        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdchequereceipt.RowsCount - 2)
                grdchequereceipt.Rows.Remove(ctx.Position.Row);
        }
        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdchequereceipt.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            string AccName = (string)(grdchequereceipt[RowCount - 1, (int)GridColumn.Particular_Account_Head].Value);
            //Check if the input value is correct
            if (grdchequereceipt[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == "" || grdchequereceipt[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == null)
            {
                grdchequereceipt[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            
            }

            if (!Misc.IsNumeric(grdchequereceipt[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value))
            {
                Global.MsgError("Invalid Amount!");
                ctx.Value = "";
                return;
            }

            FillGridRowExceptLedgerID(CurRow, CurrAccLedgerID, CurrBal);
            double checkformat = Convert.ToDouble(grdchequereceipt[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value.ToString());
            string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdchequereceipt[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = insertvalue;
          
            CalculateDrCr();
            if (AccName != "(NEW)")
            {
                AddRowChequeReceipt(RowCount);
            }
          
        }

        private void evtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            int ledgerID = 0;
            string CurrentBal = "";
            try
            {
                if (grdchequereceipt[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)" || grdchequereceipt[CurrRowPos, (int)GridColumn.Particular_Account_Head].Value.ToString() == "")
                    return;
                try
                {
                    ledgerID = Convert.ToInt32(grdchequereceipt[CurrRowPos, (int)GridColumn.Ledger_ID].Value);
                    CurrentBal = grdchequereceipt[CurrRowPos, (int)GridColumn.Current_Bal_Actual].Value.ToString();
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

        private void ChequeDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = true;
            IsChequeCashDateButton = false;
            ctx1 = (SourceGrid.CellContext)sender;
            string checkdate =ctx1.DisplayText.ToString();
            if (ctx1.DisplayText.ToString() != "")
            {
                //frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Convert.ToDateTime(ctx1.Value))));
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(checkdate));
                frm.ShowDialog();
            }
            else
            {
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Date.GetServerDate())));
                frm.ShowDialog();
            }
        }
        private void ChequeCashDate_Click(object sender, EventArgs e)
        {
            IsChequeCashDateButton = true;
            IsChequeDateButton = false;
            ctx2 = (SourceGrid.CellContext)sender;
            string CashChequeDate = ctx2.DisplayText.ToString();
            if (ctx2.DisplayText.ToString() != "")
            {
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(CashChequeDate));
                frm.ShowDialog();
            }
            else
            {
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Date.GetServerDate())));
                frm.ShowDialog();
            }
        }

        private void FillGridRowExceptLedgerID(int RowPosition, int LdrID, string CurrBal)
        {
            decimal temporary = 0;
            decimal TempAmount = 0;
            string CurrentLedgerBalance = "";
            string[] CurrentBalance = new string[2];
            if (CurrBal == "")
            {
                CurrentLedgerBalance = grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Bal_Actual].ToString();
                CurrentBalance = grdchequereceipt[Convert.ToInt32(RowPosition),  (int)GridColumn.Current_Bal_Actual].ToString().Split('(');
            }
            else
            {
                CurrentLedgerBalance = CurrBal.ToString();
                CurrentBalance = CurrBal.ToString().Split('(');
            }
            string DrCr = grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Dr_Cr].Value.ToString();

            try
            {
                TempAmount = Convert.ToDecimal(grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Amount].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                if (DrCr == "Debit")
                {
                    grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
                if (DrCr == "Credit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0]) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                    if (temporary > 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                if (DrCr == "Credit")
                {
                    grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0].ToString()) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                    if (temporary > 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                if (DrCr == "Credit")
                {
                    grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
            }
            else
            {
                grdchequereceipt[RowPosition, 5].Value = CurrBal;
            }
          
        }
        private void FillAllGridRow(int RowPosition, int LdrID, string CurrBalance)
        {
            decimal temporary = 0;
            decimal TempAmount = 0;
            string CurrentLedgerBalance = CurrBalance.ToString(); // grdchequereceipt[Convert.ToInt32(RowPosition), 8].ToString();
            string DrCr = grdchequereceipt[Convert.ToInt32(RowPosition), 3].Value.ToString();
            string[] CurrentBalance = CurrBalance.ToString().Split('(');

            try
            {
                TempAmount = Convert.ToDecimal(grdchequereceipt[Convert.ToInt32(RowPosition), 4].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                if (DrCr == "Debit")
                {
                    grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
                if (DrCr == "Credit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0]) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                    if (temporary > 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                if (DrCr == "Credit")
                {
                    grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0].ToString()) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                    if (temporary > 0)
                    {
                        grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                if (DrCr == "Credit")
                {
                    grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    grdchequereceipt[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
            }
            else
            {
                grdchequereceipt[RowPosition, (int)GridColumn.Current_Balance].Value = CurrBalance;
            }
            grdchequereceipt[RowPosition, (int)GridColumn.Ledger_ID].Value = LdrID;
            grdchequereceipt[RowPosition, (int)GridColumn.Current_Bal_Actual].Value = CurrBalance;

            CurrAccLedgerID = 0;
            CurrBal = "";
            
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
                //if (ctx.Text == "(NEW)") ctx.Text = "";
                frmLOVLedger frm = new frmLOVLedger(this);
                frm.ShowDialog();
                SendKeys.Send("{Tab}");
            }
        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmLOVLedger frm = new frmLOVLedger(this, e);
            frm.ShowDialog();
        }
        public void AddLedger(string LedgerName, int LedgerID, string CurrentBalance, bool IsSelected)
        {
            if (IsSelected)
            {
                ctx.Text = LedgerName;
                CurrAccLedgerID = LedgerID;
                CurrBal = CurrentBalance;
            }
            hasChanged = true;
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
            return true;
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

        private void evtDrCr_Changed(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            if (CurrRowPos > 1)
            {
               
                //grdchequereceipt[CurrRowPos, 4].Value = lblDifferenceAmount.Text;
            }
            FillGridRowExceptLedgerID(CurrRowPos, CurrAccLedgerID, CurrBal);
        }

        private void CalculateDrCr()
        {
            try
            {
                double dr, cr;
                dr = cr = 0;
                for (int i = 1; i < grdchequereceipt.RowsCount; i++)
                {
                    //Check for empty Amount
                    object objAmount = grdchequereceipt[i, (int)GridColumn.Amount].Value;
                    if (objAmount == null)
                        continue;
                    ////Check if it is the (NEW) row. If it is, it must be the last row.
                    if ((i == grdchequereceipt.Rows.Count) || (grdchequereceipt[i, (int)GridColumn.Amount].Value.ToString().ToUpper() == "(NEW)"))
                        continue;
                    double m_Amount = 0;

                    string m_Value = Convert.ToString(grdchequereceipt[i, (int)GridColumn.Amount].Value);//Had to do this because it showed error when the cell was left blank
                    if (m_Value.Length == 0)
                        m_Amount = 0;
                    else
                    {
                        Double.TryParse(grdchequereceipt[i, (int)GridColumn.Amount].Value.ToString(), out m_Amount);
                        try
                        {
                            Convert.ToDouble(grdchequereceipt[i, (int)GridColumn.Amount].Value);
                        }
                        catch
                        {
                            MessageBox.Show("Please enter valid amount!");
                        }
                    }

                    if (grdchequereceipt[i, 3].Value.ToString() == "Debit")
                        dr += m_Amount;
                    else if (grdchequereceipt[i, 3].Value.ToString() == "Credit")
                        cr += m_Amount;
                }
                m_DebitAmount = dr;
                m_CreditAmount = cr;
                
            }
            catch (Exception ex)
            {
                //lblDebitTotal.Text = "-";
                //lblCreditTotal.Text = "-";
                //lblDifferenceAmount.Text = "-";
                //Just ignore any errors
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
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

           
            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    try
                    {
                        isNew = true;
                        NewGrid = " ";
                        OldGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + txtVchNo.Text + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdchequereceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdchequereceipt[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                            string drcr = grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString();
                            string amt = grdchequereceipt[i + 1, (int)GridColumn.Amount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, drcr, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        //Call to Convert into XML Format
                        string ChequeReceiptXMLString = ReadAllChequeReceiptEntry();
                        if (ChequeReceiptXMLString == string.Empty)
                        {
                            //MessageBox.Show("Unable to cast Cheque Receipt entry to XML!");
                            return;
                        }

                        //Read from sourcegrid and store it to table
                        #region Used for Checking Negative Cash And Negative Bank Delete after Transfering Code to Stored Procedure
                        DataTable ChequeReceiptDetails = new DataTable();
                        ChequeReceiptDetails.Columns.Add("Ledger");
                        ChequeReceiptDetails.Columns.Add("DrCr");
                        ChequeReceiptDetails.Columns.Add("Amount");
                        ChequeReceiptDetails.Columns.Add("Remarks");
                        for (int i = 0; i < OnlyReqdDetailRows; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdchequereceipt[i + 1, (int)GridColumn.Particular_Account_Head].ToString().Split('[');
                            //Solved problem with (NEW)
                            object ChequeReceiptAmount = grdchequereceipt[i + 1, (int)GridColumn.Amount].Value;
                            if (ledgerName[0].ToString().ToUpper() == "(NEW)" && (ChequeReceiptAmount == null || ChequeReceiptAmount.ToString() == "0" || ChequeReceiptAmount.ToString() == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))))
                                continue;
                            else if (ledgerName[0].ToString().ToUpper() == "(NEW)" && ChequeReceiptAmount != null)
                            {
                                MessageBox.Show("Account Name not selected!!");
                                return;
                            }
                            try
                            {
                                Convert.ToDouble(ChequeReceiptAmount);
                            }
                            catch
                            {
                                MessageBox.Show("Please enter valid amount!");
                                return;
                            }
                            ChequeReceiptDetails.Rows.Add(ledgerName[0].ToString(), grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value, grdchequereceipt[i + 1, (int)GridColumn.Amount].Value, grdchequereceipt[i + 1, (int)GridColumn.Current_Balance].Value);

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
                                    switch (Global.Credit_Limit)
                                    {
                                        case CreditLimit.Warn:
                                            if (MessageBox.Show("Your credit Limit exceed, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                            {
                                                return;
                                            }
                                            break;
                                        case CreditLimit.Deny:
                                            Global.MsgError("Your credit Limit exceed, you are not allowed to submit this voucher!!!");
                                            return;
                                    }
                                }
                            }
                            #endregion

                            //Check the negative cash and negative Bank  ledger who is responsible for payment...so,If sorcegrid is Debit,no need to check because it will recieve amount bt check negative cash and negative bank in case of Credit

                            if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")//check negative cash and negative bank in case of ledger is responisble of payment otherwise no need to check...
                            {

                                #region BLOCK FOR CHECKING NEGATIVE CASH
                                //execute following code when only if setting of Negative cash is in Warn or Deny
                                if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                                {
                                    if (isCashAccount)//If cash account
                                    {
                                        double mDbalCash = 0;
                                        double mCbalCash = 0;
                                        int cashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);//Getting LedgerID with help of LedgerName
                                        Transaction.GetLedgerBalance(null, null, cashLedgerId, ref mDbalCash, ref mCbalCash, AccountClassID, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                        //Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrCash = Convert.ToDouble(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value);
                                        }
                                        else if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                                        {
                                            totalCrCash = Convert.ToDouble(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value);
                                        }
                                        //Total amount of ledger is summation of its own amount and amount posted from grid by enduser.
                                        totalDrCash += mDbalCash;
                                        totalCrCash += mCbalCash;
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
                                        //Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrBank = Convert.ToDouble(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value);

                                        }
                                        else if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                                        {
                                            totalCrBank = Convert.ToDouble(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value);
                                        }

                                        //Total amount of ledger is summation of its own amount and amount posted from grid by enduser.
                                        totalDrBank += mDbalBank;
                                        totalCrBank += mCbalBank;
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
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlChequeReceiptInsert", Global.m_db.cn))
                            {
                                // we are going to use store procedure  
                                dbCommand.CommandType = CommandType.StoredProcedure;
                                // Add input parameter and set its properties.
                                SqlParameter parameter = new SqlParameter();
                                // Store procedure parameter name  
                                parameter.ParameterName = "@ChequeReceipt";
                                // Parameter type as XML 
                                parameter.DbType = DbType.Xml;
                                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                parameter.Value = ChequeReceiptXMLString; // XML string as parameter value  
                                // Add the parameter in Parameters collection.
                                dbCommand.Parameters.Add(parameter);
                                Global.m_db.cn.Open();
                                int intRetValue = dbCommand.ExecuteNonQuery();
                                //MessageBox.Show(intRetValue.ToString());                                
                            }
                        }
                        #endregion

                        Global.Msg("Cheque Receipt created successfully!");
                        AccClassID.Clear();
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
                        isNew = false;
                        NewGrid = " ";
                        NewGrid = NewGrid + "Voucher No" + txtVchNo.Text + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text;
                        //Collect the Contents of the grid for audit log
                        for (int i = 0; i < grdchequereceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                        {
                            string particular = grdchequereceipt[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                            string drcr = grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString();
                            string amt = grdchequereceipt[i + 1, (int)GridColumn.Amount].Value.ToString();
                            NewGrid = NewGrid + string.Concat(particular, drcr, amt);
                        }
                        NewGrid = "NewGridValues" + NewGrid;
                        string ChequeReceiptXMLString = ReadAllChequeReceiptEntry();
                        if (ChequeReceiptXMLString == string.Empty)
                        {
                            MessageBox.Show("Unable to cast Cheque Receipt entry to XML!");
                            return;
                        }

                        //Read from sourcegrid and store it to table

                        #region Used for Checking Negative Cash And Negative Bank Delete after Transfering Code to Stored Procedure
                        DataTable JournalDetails = new DataTable();
                        JournalDetails.Columns.Add("Ledger");
                        JournalDetails.Columns.Add("DrCr");
                        JournalDetails.Columns.Add("Amount");
                        JournalDetails.Columns.Add("Remarks");

                        for (int i = 0; i < grdchequereceipt.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdchequereceipt[i + 1, (int)GridColumn.Particular_Account_Head].ToString().Split('[');
                            //Solved problem with (NEW)
                            object journalAmount = grdchequereceipt[i + 1, (int)GridColumn.Amount].Value;
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
                            JournalDetails.Rows.Add(ledgerName[0].ToString(), grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value, grdchequereceipt[i + 1, (int)GridColumn.Amount].Value, grdchequereceipt[i + 1, (int)GridColumn.Current_Balance].Value);
                            bool isCashAccount = false;
                            double totalDrCash, totalCrCash;
                            totalDrCash = totalCrCash = 0;
                            isCashAccount = Ledger.IsCashAccount(ledgerName[0].ToString());
                            //Check the negative cash and negative Bank  ledger who is responsible for payment...so,If sorcegrid is Debit,no need to check because it will recieve amount bt check negative cash and negative bank in case of Credit
                            if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")//check negative cash and negative bank in case of ledger is responisble of payment otherwise no need to check...
                            {
                                #region BLOCK FOR CHECKING NEGATIVE CASH
                                //execute following code when only if setting of Negative cash is in Warn or Deny
                                if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                                {
                                    if (isCashAccount)//If cash account
                                    {
                                        double mDbalCash = 0;
                                        double mCbalCash = 0;
                                        int cashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);//Getting LedgerID with help of LedgerName
                                        Transaction.GetLedgerBalance(null, null, cashLedgerId, ref mDbalCash, ref mCbalCash, AccountClassID, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                        //Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrCash = (mDbalCash + Convert.ToDouble(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value));//In case of Cash and Bank ...Ledger Balance is Debit
                                        }
                                        else if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                                        {
                                            totalCrCash = (mCbalCash + Convert.ToDouble(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value));//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
                                        }
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
                                        //Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrBank = (mDbalBank + Convert.ToDouble(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value));//In case of Cash and Bank ...Ledger Balance is Debit

                                        }
                                        else if (grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                                        {
                                            totalCrBank = (mCbalBank + Convert.ToDouble(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value));//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
                                        }
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

                        IJournal m_Journal = new Journal();
                        if (!m_chequeReceipt.RemoveChequeReceiptEntry(Convert.ToInt32(txtchequereceiptid.Text)))
                        {
                            MessageBox.Show("Unable to Update record. Please restart the modification");
                            return;
                        }

                        #region Save xml data to Database
                        using (System.IO.StringWriter swStringWriter = new StringWriter())
                        {
                            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlChequeReceiptInsert", Global.m_db.cn))
                            {
                                // we are going to use store procedure  
                                dbCommand.CommandType = CommandType.StoredProcedure;
                                // Add input parameter and set its properties.
                                SqlParameter parameter = new SqlParameter();
                                // Store procedure parameter name  
                                parameter.ParameterName = "@ChequeReceipt";
                                // Parameter type as XML 
                                parameter.DbType = DbType.Xml;
                                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                                parameter.Value = ChequeReceiptXMLString; // XML string as parameter value  
                                // Add the parameter in Parameters collection.
                                dbCommand.Parameters.Add(parameter);
                                Global.m_db.cn.Close();
                                Global.m_db.cn.Open();
                                int intRetValue = dbCommand.ExecuteNonQuery();
                                //MessageBox.Show(intRetValue.ToString());
                            }
                        }
                        #endregion

                        Global.Msg("Cheque Receipt modified successfully!");
                        AccClassID.Clear();
                        ClearVoucher();
                     
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
            if (!(grdchequereceipt.Rows.Count > 2))
            {
                Global.MsgError("Please complete the journal entry!");
                grdchequereceipt.Focus();
                bValidate = false;
            }
            return bValidate;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;

            bool chkUserPermission = UserPermission.ChkUserPermission("CHEQUE_RECEIPT_VIEW");
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
         private void ClearVoucher()
        {
            ClearChequeReceipt();
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdchequereceipt.Redim(2, 13);
            AddGridHeader(); //Write header part
            AddRowChequeReceipt(1);
        }
        private void ClearChequeReceipt()
        {
            txtVchNo.Clear(); //actually generate a new voucher no.
           // txtDate.Clear();
            txtRemarks.Clear();
            //txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            grdchequereceipt.Rows.Clear();
            cboSeriesName.Text = string.Empty;
            cboProjectName.SelectedIndex = 0;
         
        }

        private string ReadAllChequeReceiptEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            SeriesID = (ListItem)cboSeriesName.SelectedItem;
            liProjectID = (ListItem)cboProjectName.SelectedItem;
            liBankID = (ListItem)comboBankAccount.SelectedItem;

            //validate grid entry
            if (!ValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();
            #region  Cheque Receipt
            tw.WriteStartElement("CHEQUERECEIPT");
            {
                ///For Journal Master Section
                ///int JournalID = System.Convert.ToInt32(9); // Auto increment  
                ///
                string first = txtfirst.Text;
                string second = txtsecond.Text;
                string third = txtthird.Text;
                string fourth = txtfourth.Text;
                string fifth = txtfifth.Text;
                int SID = System.Convert.ToInt32(SeriesID.ID);
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime ChequeReceipt_Date = Date.ToDotNet(txtDate.Text);
                string Remarks = System.Convert.ToString(txtRemarks.Text);
                int ProjectID = System.Convert.ToInt32(liProjectID.ID);
                int BankID = Convert.ToInt32(liBankID.ID);
                string BankName = liBankID.Value.ToString();
                String ChequeReceiptID = System.Convert.ToString(txtchequereceiptid.Text);
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                //string Created_By = System.Convert.ToString("Admin");
                string Created_By = User.CurrentUserName;
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                // string Modified_By = System.Convert.ToString("Admin");
                string Modified_By = User.CurrentUserName;
                tw.WriteStartElement("CHEQUERECEIPTMASTER");
                tw.WriteElementString("SeriesID", SID.ToString());
                tw.WriteElementString("BankID", BankID.ToString());
                tw.WriteElementString("BankName", BankName);
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("ChequeReceipt_Date", Date.ToDB(ChequeReceipt_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("chequeReceiptID", ChequeReceiptID.ToString());
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
              //  int ChequeReceiptID = 0;
                int LedgerID = 0;
                decimal Amount = 0;
                string DrCr = "";
                string RemarksDetail = "";
                string ChequeNumber = "";
                string ChequeBank="";
                string ChequeDate="";
                DateTime ChequeDateValue=new DateTime();
                string ChequeCashDate = "";
                DateTime ChequeCashDateValue = new DateTime() ;
                tw.WriteStartElement("CHEQUERECEIPTDETAILS");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    ChequeReceiptID = System.Convert.ToString(txtchequereceiptid.Text);
                    LedgerID = System.Convert.ToInt32(grdchequereceipt[i + 1, (int)GridColumn.Ledger_ID].Value);
                    Amount = System.Convert.ToDecimal(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value);
                    DrCr = System.Convert.ToString(grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value);
                    RemarksDetail = System.Convert.ToString(grdchequereceipt[i + 1, (int)GridColumn.Remarks].Value);
                    ChequeNumber = System.Convert.ToString(grdchequereceipt[i + 1, (int)GridColumn.Cheque_No].Value);
                    ChequeBank = System.Convert.ToString(grdchequereceipt[i + 1, (int)GridColumn.ChequeBank].Value);
                    if (ChequeNumber == "")
                    {
                        ChequeDate = "";
                        ChequeDateValue = Date.GetServerDate();
                        ChequeCashDate = "";
                        ChequeCashDateValue = Date.GetServerDate();
                    }
                    else
                    {
                        try
                        {
                            ChequeDate = grdchequereceipt[i + 1, (int)GridColumn.ChequeDate].Value.ToString();
                            ChequeCashDate = grdchequereceipt[i + 1, (int)GridColumn.ChequeCash_Date].Value.ToString();
                            if(!String.IsNullOrEmpty(ChequeDate))
                            {
                                ChequeDateValue = Date.ToDotNet(ChequeDate);
                            }
                            if (!String.IsNullOrEmpty(ChequeCashDate))
                            {
                                
                                ChequeCashDateValue = Date.ToDotNet(ChequeCashDate);
                            }
                        }
                        catch (Exception ex)
                        {
                            Global.MsgError("Error in Cheque Date or Cheque Cash Date. Please correct and re-enter them");
                            return "";
                        }
                    }
                   
                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("ChequeReceiptID", ChequeReceiptID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("DrCr", DrCr.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteElementString("ChequeNumber", ChequeNumber.ToString());
                    tw.WriteElementString("ChequeBank", ChequeBank.ToString());   
                    if(ChequeDate!="")
                    tw.WriteElementString("ChequeDate",Date.ToDB(ChequeDateValue));
                    if(ChequeCashDate!="")
                    tw.WriteElementString("ChequeCashDate",Date.ToDB(ChequeCashDateValue));
                    tw.WriteElementString("IsChequeCash", 0.ToString());
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

        private bool ValidateGrid()
        {
            int[] LdrID = new int[20];
            decimal[] Amt = new decimal[20];

            //Validate input grid record
            for (int i = 0; i < grdchequereceipt.Rows.Count - 1; i++)
            {
                try
                {
                    //if ledger ID repeats then message it
                    // if LedgerID is not present in between them
                    int tempValue = 0;
                    decimal tempDecValue = 0;
                    try
                    {
                        tempValue = System.Convert.ToInt32(grdchequereceipt[i + 1, (int)GridColumn.Ledger_ID].Value);
                    }
                    catch (Exception ex)
                    {
                        tempValue = 0;
                    }
                    try
                    {
                        tempDecValue = System.Convert.ToDecimal(grdchequereceipt[i + 1, (int)GridColumn.Amount].Value);
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
                        if (i + 2 == grdchequereceipt.Rows.Count && grdchequereceipt[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)")
                        {
                            //Do Nothing
                        }
                        else
                            return false;
                    }
                    else
                        LdrID[i] = tempValue;

                    if (i + 2 == grdchequereceipt.Rows.Count && grdchequereceipt[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString() == "(NEW)")
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

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            bool chkUserPermission = UserPermission.ChkUserPermission("CHEQUE_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            isNew = false;
            OldGrid = " ";
           
            OldGrid = OldGrid + "Voucher No" + txtVchNo.Text + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdchequereceipt.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string particular = grdchequereceipt[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                string drcr = grdchequereceipt[i + 1, (int)GridColumn.Dr_Cr].Value.ToString();
                string amt = grdchequereceipt[i + 1, (int)GridColumn.Amount].Value.ToString();
                OldGrid = OldGrid + string.Concat(particular, drcr, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;
            if (!ContinueWithoutSaving())
            {
                return;
            }
            if (txtchequereceiptid.Text.Length <= 0)
            {
                Global.MsgError("Please navigate to existing journal first and then try again!");
                return;
            }
            EnableControls(true);
            ChangeState(EntryMode.EDIT);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool Navigation(Navigate NavTo)
        {
            try
            {
                if (!IsShortcutKey)
                {
                    //CashReceipt m_CashReceipt = new CashReceipt();
                    ChangeState(EntryMode.NORMAL);
                }
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtchequereceiptid.Text);
                    if (BankReceiptIDCopy > 0)
                    {
                        VouchID = BankReceiptIDCopy;
                        BankReceiptIDCopy = 0;

                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtchequereceiptid.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                DataTable dtChequeReceiptMaster = m_chequeReceipt.NavigateChequeReceiptMaster(VouchID, NavTo);
                if (dtChequeReceiptMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    return false;
                }
                //Write the corresponding textboxes
                DataRow drChequeReceiptMaster = dtChequeReceiptMaster.Rows[0]; //There is only one row. First row is the required record
                if (IsShortcutKey)
                {
                    txtRemarks.Text = drChequeReceiptMaster["Remarks"].ToString();
                    IsShortcutKey = false;
                    txtRemarks.SelectionStart = txtRemarks.Text.Length + 1;
                    return false;
                }

                //Clear everything in the form
                ClearVoucher();

                // Show the corresponding Bank Account Ledger in Combobox
                DataTable dtledgerInfo = m_chequeReceipt.GetChequeReceiptDetail(Convert.ToInt32(drChequeReceiptMaster["chequeReceiptID"]));
                DataRow drchequeReceiptDetail = dtledgerInfo.Rows[0];
                DataTable dtBankLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drchequeReceiptDetail["LedgerID"]), LangMgr.DefaultLanguage);
                DataRow drBankLedgerInfo = dtBankLedgerInfo.Rows[0];

                comboBankAccount.Text = drBankLedgerInfo["LedName"].ToString();

                //Show the Corresponding SeriesName in Combobox
                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drChequeReceiptMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Bank Receipt");
                    cboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();
                }
                txtVchNo.Text = drChequeReceiptMaster["VoucherNo"].ToString();
                txtDate.Text = Date.DBToSystem(drChequeReceiptMaster["chequeReceiptDate"].ToString());
                txtRemarks.Text = drChequeReceiptMaster["Remarks"].ToString();
                DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(drChequeReceiptMaster["ProjectID"]), LangMgr.DefaultLanguage);
                if (dtProjectInfo.Rows.Count != 0)
                {
                    DataRow drProjectInfo = dtProjectInfo.Rows[0];
                    cboProjectName.Text = drProjectInfo["Name"].ToString();
                    txtchequereceiptid.Text = drChequeReceiptMaster["chequeReceiptID"].ToString();
                  //  dschequeRec.Tables["tblChequeReceiptMaster"].Rows.Add(cboSeriesName.Text, drChequeReceiptMaster["VoucherNo"].ToString(), comboBankAccount.Text, drChequeReceiptMaster["Remarks"].ToString(), drProjectInfo["Name"].ToString());
                    dsChequeReceipt.Tables["tblChequeReceiptMaster"].Rows.Add(cboSeriesName.Text, drChequeReceiptMaster["VoucherNo"].ToString(), drChequeReceiptMaster["chequeReceiptDate"].ToString(), drChequeReceiptMaster["Remarks"].ToString(), drProjectInfo["Name"].ToString(), comboBankAccount.Text);
                }
                else
                {
                    cboProjectName.Text = "None";
                    txtchequereceiptid.Text = drChequeReceiptMaster["chequeReceiptID"].ToString();
                    dsChequeReceipt.Tables["tblChequeReceiptMaster"].Rows.Add(cboSeriesName.Text, drChequeReceiptMaster["VoucherNo"].ToString(), Date.DBToSystem(drChequeReceiptMaster["chequeReceiptDate "].ToString()), drChequeReceiptMaster["Remarks"].ToString(), drChequeReceiptMaster["chequeReceiptDate"].ToString(), comboBankAccount.Text, "None");

                }

                DataTable dtChequeReceiptDetail = m_chequeReceipt.GetChequeReceiptDetail(Convert.ToInt32(txtchequereceiptid.Text));
                for (int i = 1; i <= dtChequeReceiptDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtChequeReceiptDetail.Rows[i - 1];
                    grdchequereceipt[i, (int)GridColumn.Code_No].Value = i.ToString();
                    grdchequereceipt[i, (int)GridColumn.Particular_Account_Head].Value = drDetail["LedgerName"].ToString();
                    grdchequereceipt[i, (int)GridColumn.Ledger_ID].Value = Convert.ToInt32(drDetail["LedgerID"]);
                    grdchequereceipt[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdchequereceipt[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                    grdchequereceipt[i, (int)GridColumn.Cheque_No].Value = drDetail["ChequeNumber"].ToString();
                    grdchequereceipt[i, (int)GridColumn.ChequeBank].Value = drDetail["ChequeBank"].ToString();
                    grdchequereceipt[i, (int)GridColumn.ChequeDate].Value = Date.DBToSystem(drDetail["chequeDate"].ToString());


                    if (drDetail["chequeCashDate"].ToString() == "")
                    {
                        grdchequereceipt[i, (int)GridColumn.ChequeCash_Date].Value = "";
                    }
                    else
                    {
                        grdchequereceipt[i, (int)GridColumn.ChequeCash_Date].Value = Date.DBToSystem(drDetail["chequeCashDate"].ToString());
                    }
                    AddRowChequeReceipt(grdchequereceipt.RowsCount);
                    dsChequeReceipt.Tables["tblChequeReceiptDetail"].Rows.Add(drDetail["LedgerName"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["ChequeNumber"].ToString(), drDetail["ChequeBank"].ToString(), Date.DBToSystem(drDetail["chequeDate"].ToString()), drDetail["Remarks"].ToString());
                    totalAmt = (totalAmt + Convert.ToDecimal(drDetail["Amount"]));
                    totalRptAmt = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(totalAmt)).ToString();
                }

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtchequereceiptid.Text), "CHEQUE_RCPT");
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
               // btnExport.Enabled = true;
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

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CHEQUE_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            Navigation(Navigate.Last);
            IsFieldChanged = false;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CHEQUE_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            Navigation(Navigate.First);
            IsFieldChanged = false;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CHEQUE_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            Navigation(Navigate.Prev);
            IsFieldChanged = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CHEQUE_RECEIPT_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            Navigation(Navigate.Next);
            IsFieldChanged = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

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

       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }
        private void PrintPreviewCR(PrintType myPrintType)
        {
           dsChequeReceipt.Clear();
            rptChequeReceipt rpt = new rptChequeReceipt();
           // Misc.WriteLogo(dschequeRec, "tblImage");
            rpt.SetDataSource(dsChequeReceipt);

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
                // Navigation(Navigate.ID);
              
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("CHEQUE_RECEIPT_DELETE");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
            //    return;
            //}
            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            try
            {

                if (!String.IsNullOrEmpty(txtchequereceiptid.Text))
                {
                    if (Global.MsgQuest("Are you sure you want to delete the Cheque Receipt - " + txtchequereceiptid.Text + "?") == DialogResult.Yes)
                    {
                        ChequeReceipt m_chequeReceipt = new ChequeReceipt();
                        if(m_chequeReceipt.RemoveChequeReceiptEntry(Convert.ToInt32(txtchequereceiptid.Text)))
                        {
                            Global.Msg("Cheque Receipt -" + txtchequereceiptid.Text + " deleted successfully!");
                            //Navigate to 1 step previous
                            if (!this.Navigation(Navigate.Prev))
                            {
                                //This must be because there are no records or this was the first one

                                //If this was the first, try to navigate to second
                                if (!this.Navigation(Navigate.Next))
                                {
                                    //This was the last one, there are no records left. Simply clear the form and stay calm
                                    btnNew_Click(sender, e);
                                }
                            }
                        }
                        else
                            Global.MsgError("There was an error while deleting Bank Receipt -" + txtchequereceiptid.Text + "!");
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
    
    }
}
