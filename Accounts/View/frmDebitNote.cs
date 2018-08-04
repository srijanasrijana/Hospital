using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using CrystalDecisions.Shared;
using Common;
using Accounts.Reports;

namespace  Accounts

{
    public partial class frmDebitNote : Form,IfrmAddAccClass, IfrmDateConverter, IfrmAccountLedger, ILOVLedger
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;  
        //Check if the Account has been already set by LOVLedger
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
        private Accounts.Model.dsDebitNote dsDebitNoteInvoice = new Accounts.Model.dsDebitNote();
        bool hasChanged = false;
        private string Prefix = "";
        private int currRowPosition = 0;
        private string CurrBal = "";
        private int loopCounter = 0;
        DevAge.Windows.Forms.DevAgeTextBox ctx; 
        private bool IsShortcutKey = false;
        private bool IsNegativeCash = false;
        private bool IsNegativeBank = false;
        private int DebitNoteIDCopy = 0;
        List<int> AccClassID = new List<int>();
        ListItem SeriesID = new ListItem();
        ListItem PartyID = new ListItem();
        ArrayList AccountClassID = new ArrayList();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        double m_DebitAmount = 0;//Holds the total debit amount on the Voucher
        double m_CreditAmount = 0;
        DataTable dtAccClassID = new DataTable();
        int m_DebitNoteID = 0;
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
            Del = 0, SNo, Account, Amount, Remarks, LedgerId
        };
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();

        public frmDebitNote()
        {
            InitializeComponent();
        }

        public frmDebitNote(int DebitNoteID)
        {
            InitializeComponent();
            this.m_DebitNoteID = DebitNoteID;
        }

        public void AddLedger(string LedgerName, int LedgerID, string CurrentBalance, bool IsSelected,string ActualBal,string LedgerType)
        {
            if (IsSelected)
            {
                //if(LedgerName.Trim().Length>0)
                ctx.Text = LedgerName;
                currRowPosition = LedgerID;
                CurrBal = CurrentBalance;
            }
            hasChanged = true;
        }

        public void AddAccountLedger(string LedgerName)
        {

        }

        public void DateConvert(DateTime DotNetDate)
        {
            txtDate.Text = Date.ToSystem(DotNetDate);
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
            //DataTable dt = new DataTable();
            //try
            //{
            //    dt = AccountClass.GetAccClassTable(AccClassID);
            //}
            //catch (Exception ex)
            //{
            //    Global.Msg(ex.Message);
            //}

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dt.Rows[i];

            //    TreeNode t = new TreeNode(dr[LangField].ToString());
            //    t.Tag = dr["AccClassID"].ToString();
            //    //Check if it is a parent Or if it has childs
            //    try
            //    {
            //        if (ChildCount((int)dr["AccClassID"]) > 0)
            //        {
            //            //t.IsContainer = true;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }

            //    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //    if (n == null)
            //    {
            //        tv.Nodes.Add(t); //Primary Group
            //    }
            //    else
            //    {
            //        n.Nodes.Add(t); //Secondary Group
            //    }
            //}
            //Insert the tag on the selected node to carry AccClassID
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

        private void frmDebitNote_Load(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NEW);           
            //ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            #region BLOCK FOR SHWOWING DEBTOR AND CREDITOR LEDGERS IN PARTY ACCOUNT COMBOBOX
            int Debtor = AccountGroup.GetGroupIDFromGroupNumber(29);

            DataTable dtDebtorLedgers = Ledger.GetAllLedger(Debtor);
            foreach(DataRow drDebtorLedgers in dtDebtorLedgers.Rows)
            {                
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drDebtorLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                cboPartyAcc.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            int Creditor = AccountGroup.GetGroupIDFromGroupNumber(114);
            DataTable dtCreditorLedgers = Ledger.GetAllLedger(Creditor);
            foreach(DataRow drCreditorLedgers in dtCreditorLedgers.Rows)
            {
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCreditorLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                cboPartyAcc.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboPartyAcc.DisplayMember = "value";//This value is  for showing at Load condition
            cboPartyAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            //cboPartyAcc.SelectedIndex = 0;

            #endregion
            ShowAccClassInTreeView(treeAccClass, null, 0);
            //Display this form at formLoad condition according to DebitNoteID which is  obtained by double clicking DebitNoteRegister Form
            txtDebitNoteID.Visible = false;
            m_mode = EntryMode.NEW;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            //For Loading The Optional Fields
            OptionalFields();
            try
            {
                //Event trigerred when delete button is clicked
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                //Event when account is selected
                evtAccount.FocusLeft += new EventHandler(Account_Selected);
                evtAccountFocusLost.FocusLeft += new EventHandler(evtAccountFocusLost_FocusLeft);
                evtAmountFocusLost.FocusLeft += new EventHandler(Amount_Focus_Lost);
                grdDebitNote.Redim(2, 6);
                btnRowDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                AddGridHeader();
                AddRowDebitNote(1);

                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID

                if (m_DebitNoteID > 0)
                {
                    //Show the values in fields

                    try
                    {

                        ChangeState(EntryMode.NORMAL);
                      
                        int vouchID = 0;
                        try
                        {
                            vouchID = m_DebitNoteID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }

                        DebitNote m_DebitNote = new DebitNote();

                        //Getting the value of SeriesID via MasterID or VouchID

                        int SeriesIDD = m_DebitNote.GetSeriesIDFromMasterID(vouchID);

                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this DebitNote");
                            cboSeriesName.Text = "";

                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            cboSeriesName.Text = dr["EngName"].ToString();

                        }

                        DataTable dtDebitNoteMaster = m_DebitNote.GetDebitNoteMaster(vouchID);

                        if (dtDebitNoteMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }

                        //Write the corresponding textboxes

                        DataRow drDebitNoteMaster = dtDebitNoteMaster.Rows[0];
                        txtVchNo.Text = drDebitNoteMaster["Voucher_No"].ToString();
                        txtDate.Text = Date.DBToSystem(drDebitNoteMaster["DebitNote_Date"].ToString());
                        txtRemarks.Text = drDebitNoteMaster["Remarks"].ToString();
                        txtDebitNoteID.Text = drDebitNoteMaster["DebitNoteID"].ToString();
                        int ID = Convert.ToInt32(drDebitNoteMaster["LedgerID"].ToString());
                        cboPartyAcc.Text = Ledger.GetLedgerNameFromID(ID);
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

                                txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                                txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                                txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                                txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                                txtfifth.Text = drDebitNoteMaster["Field5"].ToString();
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

                                txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                                txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                                txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                                txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                                txtfifth.Text = drDebitNoteMaster["Field5"].ToString();
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

                                txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                                txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                                txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                                txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                                txtfifth.Text = drDebitNoteMaster["Field5"].ToString();

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

                                txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                                txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                                txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                                txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                                txtfifth.Text = drDebitNoteMaster["Field5"].ToString();

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

                                txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                                txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                                txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                                txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                                txtfifth.Text = drDebitNoteMaster["Field5"].ToString();
                            }


                        }
                        DataTable dtDebitNoteDetail = m_DebitNote.GetDebitNoteDetail(Convert.ToInt32(drDebitNoteMaster["DebitNoteID"]));


                        for (int i = 1; i <= dtDebitNoteDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtDebitNoteDetail.Rows[i - 1];

                            grdDebitNote[i, (int)GridColumn.SNo].Value = i.ToString();
                            grdDebitNote[i, (int)GridColumn.Account].Value = drDetail["LedgerName"].ToString();

                            grdDebitNote[i, (int)GridColumn.Amount].Value = drDetail["Amount"].ToString();
                            grdDebitNote[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                            AddRowDebitNote(grdDebitNote.RowsCount);
                           
                        }



                #endregion
                    }
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

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdDebitNote.RowsCount - 2)
                grdDebitNote.Rows.Remove(ctx.Position.Row);
        }

        private void Account_Selected(object sender, EventArgs e)
        {
            //SourceGrid.CellContext ct = new SourceGrid.CellContext();
            //try
            //{
            //    ct = (SourceGrid.CellContext)sender;
            //}
            //catch (Exception ex)
            //{

            //}
            //if (ct.DisplayText == "")
            //    return;
            //int RowCount = grdDebitNote.RowsCount;

            //string CurRow = (string)grdDebitNote[RowCount - 1, 2].Value;

            ////Check whether the new row is already added
            //if (CurRow != "(NEW)")
            //{
            //    AddRowDebitNote(RowCount);
            //    //Clear (NEW) on other colums as well
            //    ClearNew(RowCount - 1);
            //}

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

        private void evtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int a = ctx.Position.Row;
            grdDebitNote[a, 5].Value = currRowPosition;
        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmLOVLedger frm = new frmLOVLedger(this, e);
            frm.ShowDialog();
        }

        /// <summary>
        /// Clears the (NEW) in the newly created row just after insertion of new Data in grid
        /// </summary>
        /// <param name="RowCount"></param>
        private void ClearNew(int RowCount)
        {
            if (grdDebitNote[RowCount, (int)GridColumn.Account].Value == "(NEW)")
                grdDebitNote[RowCount, (int)GridColumn.Account].Value = "";
            if (grdDebitNote[RowCount, (int)GridColumn.Amount].Value == "(NEW)")
                grdDebitNote[RowCount, (int)GridColumn.Amount].Value = "";
            if (grdDebitNote[RowCount, (int)GridColumn.Remarks].Value == "(NEW)")
                grdDebitNote[RowCount, (int)GridColumn.Remarks].Value = "";
        }

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdDebitNote.RowsCount;
            string CurRow = (string)grdDebitNote[RowCount - 1, 2].Value;
            if (CurRow != "(NEW)")
            {
                AddRowDebitNote(RowCount);
            }
        }
        /// <summary>
        /// Writes the header part for grdJournal
        /// </summary>
        private void AddGridHeader()
        {
            grdDebitNote[0, (int)GridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del"); 
            grdDebitNote[0, (int)GridColumn.Del].Column.Width = 25;
            grdDebitNote.Columns[(int)GridColumn.Del].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdDebitNote[0, (int)GridColumn.SNo] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdDebitNote.Columns[(int)GridColumn.SNo].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdDebitNote[0, (int)GridColumn.Account] = new SourceGrid.Cells.ColumnHeader("Account");
            grdDebitNote[0, (int)GridColumn.Account].Column.Width = 200;
            grdDebitNote.Columns[(int)GridColumn.Account].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdDebitNote[0, (int)GridColumn.Amount] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdDebitNote[0, (int)GridColumn.Amount].Column.Width = 200;
            grdDebitNote.Columns[(int)GridColumn.Amount].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdDebitNote[0, (int)GridColumn.Remarks] = new SourceGrid.Cells.ColumnHeader("Remarks");
            grdDebitNote[0, (int)GridColumn.Remarks].Column.Width = 200;
            grdDebitNote.Columns[(int)GridColumn.Remarks].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdDebitNote[0, (int)GridColumn.LedgerId] = new SourceGrid.Cells.ColumnHeader("Ledger ID");
            grdDebitNote[0, (int)GridColumn.LedgerId].Column.Width = 50;
            grdDebitNote[0, (int)GridColumn.LedgerId].Column.Visible = false;
        }
        

        /// <summary>
        /// Adds the row in the DebitNote field
        /// </summary>
        private void AddRowDebitNote(int RowCount)
        {
            //Add a new row
            grdDebitNote.Redim(Convert.ToInt32(RowCount + 1), grdDebitNote.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Accounts.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            grdDebitNote[i, (int)GridColumn.Del] = btnDelete;
            grdDebitNote[i, (int)GridColumn.Del].AddController(evtDelete);

            grdDebitNote[i, (int)GridColumn.SNo] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdDebitNote[i, (int)GridColumn.Account] = new SourceGrid.Cells.Cell("", txtAccount);
            txtAccount.Control.GotFocus += new EventHandler(Account_Focused);
            txtAccount.Control.LostFocus += new EventHandler(Account_Leave);
            txtAccount.Control.KeyDown += new KeyEventHandler(Account_KeyDown);
            grdDebitNote[i, (int)GridColumn.Account].AddController(evtAccountFocusLost);
            grdDebitNote[i, (int)GridColumn.Account].Value = "(NEW)"; 

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;

            grdDebitNote[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell("", txtAmount);
            grdDebitNote[i, (int)GridColumn.Amount].AddController(evtAmountFocusLost);
            //grdDebitNote[i, 3].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;

            grdDebitNote[i, (int)GridColumn.Remarks] = new SourceGrid.Cells.Cell("", txtRemarks);
           // grdDebitNote[i, 4].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.Focus;
            grdDebitNote[i, (int)GridColumn.LedgerId] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdDebitNote[i, (int)GridColumn.LedgerId].Value = "";

        }

        //A function from the Interface IfrmAccClassID. Used to apply the Datatable to this form from AddAccClass Form

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }


            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("Input date is out of range!");
                return;
            }
            #region BLOCK FOR MANUAL VOUCHER NUMBERING TYPE

            VoucherConfiguration m_VouConfig = new VoucherConfiguration();
            if (SeriesID.ID > 0)
            {
                DataTable dtVouConfigInfo = m_VouConfig.GetVouNumConfiguration(Convert.ToInt32(SeriesID.ID));
                if (dtVouConfigInfo.Rows.Count > 0)
                {
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
            ListItem liProjectID = new ListItem();
            liProjectID = (ListItem)cboProjectName.SelectedItem;
            switch (m_mode)
            {
                    //Incase of Debitnote,all ledgers of Detail section are responsible for payment soo need to check negative cash and negative bank.
                    //Incase of Master section,the list of ledgers are combination of Debtor and Creditor group soo no need to check negative cash and negative bank
                    //finally,the checking of negative cash and negative bank are only essential for detail section not for master section
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    try
                    {
                        IDebitNote m_DebitNote = new DebitNote();
                        DateTime DebitNoteDate = Date.ToDotNet(txtDate.Text);
                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        ListItem liLedgerID = new ListItem();
                        liLedgerID = (ListItem)cboPartyAcc.SelectedItem;
                        //Read from sourcegrid and store it to table
                        DataTable DebitNoteDetails = new DataTable();
                        DebitNoteDetails.Columns.Add("Ledger");
                        DebitNoteDetails.Columns.Add("Amount");
                        DebitNoteDetails.Columns.Add("Remarks");
                        for (int i = 0; i < grdDebitNote.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdDebitNote[i + 1, (int)GridColumn.Account].ToString().Split('[');
                            DebitNoteDetails.Rows.Add(ledgerName[0].ToString(), grdDebitNote[i + 1, (int)GridColumn.Amount].Value, grdDebitNote[i + 1, (int)GridColumn.Remarks].Value);


                            //IDebitNote m_DebitNote = new DebitNote();
                            //DateTime DebitNoteDate = Date.ToDotNet(txtDate.Text);
                            //SeriesID = (ListItem)cboSeriesName.SelectedItem;
                            //ListItem liLedgerID = new ListItem();
                            //liLedgerID = (ListItem)cboPartyAcc.SelectedItem;


                            //Block for checking negative cash and negative cash...

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
                                    Transaction.GetLedgerBalance(null, null, CashLedgerId, ref mDbalCash, ref mCbalCash, arrNode, 0);//here projectid is kept as O because it ignores project,we dont need Project while checking ledger balance over here
                                    //In case of Debit Note,Bank Account in master section is Debit and other accounts in Detail section are bydefault Credit
                                    totalDrCash = mDbalCash;
                                    totalCrCash += (mCbalCash + Convert.ToDouble(grdDebitNote[i + 1, (int)GridColumn.Amount].Value));//The ledgers of Details section will be posted as Credit,soo tatal balance will be the summation of its own balance and grid value
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
                                    totalCrBank += (mCbalBank + Convert.ToDouble(grdDebitNote[i + 1, (int)GridColumn.Amount].Value));
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
                        }

                            #endregion
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;

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

                            if (AccClassID.Count != 0)
                            {
                                m_DebitNote.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, DebitNoteDate, txtRemarks.Text, DebitNoteDetails, AccClassID.ToArray(),Convert.ToInt16(liProjectID.ID),OF);

                            }
                            else
                            {
                                int[] a = new int[] { 1 };
                                m_DebitNote.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, DebitNoteDate, txtRemarks.Text, DebitNoteDetails, a.ToArray(),Convert.ToInt32(liProjectID.ID),OF);

                            }

                            //Update the last AutoNumber in tblSeries,only if the voucher hide type is true
                            if (NumberingType == "AUTOMATIC" && m_VouConfig.GetIsVouHideType(SeriesID.ID))
                            {
                                object m_vounum = m_VouConfig.UpdateLastVoucherNum(SeriesID.ID, increasedSeriesNum);
                            }

                        Global.Msg("Debit Note created successfully!");
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
                case EntryMode.EDIT: //if edit button is pressed
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable DebitNoteDetails = new DataTable();
                        DebitNoteDetails.Columns.Add("Ledger");             
                        DebitNoteDetails.Columns.Add("Amount");
                        DebitNoteDetails.Columns.Add("Remarks");
                        for (int i = 0; i < grdDebitNote.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            DebitNoteDetails.Rows.Add(grdDebitNote[i + 1, (int)GridColumn.Account].Value, grdDebitNote[i + 1, (int)GridColumn.Amount].Value, grdDebitNote[i + 1, (int)GridColumn.Remarks].Value);
                        }
                        IDebitNote m_DebitNote = new DebitNote();
                        DateTime DebitNoteDate = Date.ToDotNet(txtDate.Text);
                        SeriesID = (ListItem)cboSeriesName.SelectedItem;

                        ListItem liLedgerID = new ListItem();
                        liLedgerID = (ListItem)cboPartyAcc.SelectedItem;
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        if (AccClassID.Count != 0)
                        {
                            m_DebitNote.Modify(Convert.ToInt32(txtDebitNoteID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, DebitNoteDate, txtRemarks.Text, DebitNoteDetails, AccClassID.ToArray(),Convert.ToInt32(liProjectID.ID),OF);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_DebitNote.Modify(Convert.ToInt32(txtDebitNoteID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, DebitNoteDate, txtRemarks.Text, DebitNoteDetails, a.ToArray(),Convert.ToInt32(liProjectID.ID),OF);

                        
                        }
                         Global.Msg("Debit Note modified successfully!");
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
            if (cboPartyAcc.SelectedItem == null)
            {
                Global.MsgError("Invalid Party Account Selected");
                cboPartyAcc.Focus();
                bValidate = false;
            }
            if(!(grdDebitNote.Rows.Count>2))
            {
                Global.MsgError("Invalid Account Ledgers Selected in grid");
                grdDebitNote.Focus();
                bValidate = false; 

            }
            return bValidate;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;

            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false);
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    LoadSeriesNo();
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    break;
            }
        }

        private void LoadSeriesNo()
        {
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("DR_NOT");
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
            txtVchNo.Enabled = txtDate.Enabled = txtRemarks.Enabled = grdDebitNote.Enabled=cboPartyAcc.Enabled=cboSeriesName.Enabled=tabControl1.Enabled= Enable;
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_MODIFY");
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
            EnableControls(true);
            ChangeState(EntryMode.EDIT);

            //if automatic voucher number increment is selected
            string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
            if (NumberingType == "AUTOMATIC")
                txtVchNo.Enabled = false;
        }

        /// <summary>
        /// Clears the whole voucher
        /// </summary>
        private void ClearVoucher()
        {
            ClearDebitNote();
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdDebitNote.Redim(2, 6);
            AddGridHeader(); //Write header part
            AddRowDebitNote(1);
        }

        //Clears the text of every field of Journal form
        private void ClearDebitNote()
        {
            cboSeriesName.Text = string.Empty;
            cboPartyAcc.Text = string.Empty;
            txtVchNo.Clear(); //actually generate a new voucher no.
           // txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtRemarks.Clear();
            grdDebitNote.Rows.Clear();
            
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
                ChangeState(EntryMode.NORMAL);
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtDebitNoteID.Text);
                    if (DebitNoteIDCopy > 0) //Copy mode, so simply paste the id
                    {
                        VouchID = DebitNoteIDCopy;
                        DebitNoteIDCopy = 0;
                    }
                    else //if not in copy mode
                    {
                        VouchID = Convert.ToInt32(txtDebitNoteID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                IDebitNote m_DebitNote = new DebitNote();
                DataTable dtDebitNoteMaster = m_DebitNote.NavigateDebitNoteMaster(VouchID, NavTo);
                if (dtDebitNoteMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }
                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                DataRow drDebitNoteMaster = dtDebitNoteMaster.Rows[0]; //There is only one row. First row is the required record

                //Show the corresponding Party Account Ledger in Combobox
                DataTable dtPartyLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drDebitNoteMaster["LedgerID"]), LangMgr.DefaultLanguage);
                foreach (DataRow drPartyLedgerInfo in dtPartyLedgerInfo.Rows)
                {
                    cboPartyAcc.Text = drPartyLedgerInfo["LedName"].ToString();
                }
 
                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drDebitNoteMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Debit Note");
                    cboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();
                }
                lblVouNo.Visible = true;
                txtVchNo.Visible = true;
                txtVchNo.Text = drDebitNoteMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drDebitNoteMaster["DebitNote_Date"].ToString());
                txtRemarks.Text = drDebitNoteMaster["Remarks"].ToString();
                txtDebitNoteID.Text = drDebitNoteMaster["DebitNoteID"].ToString();
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

                        txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                        txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                        txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                        txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                        txtfifth.Text = drDebitNoteMaster["Field5"].ToString();
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

                        txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                        txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                        txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                        txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                        txtfifth.Text = drDebitNoteMaster["Field5"].ToString();
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

                        txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                        txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                        txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                        txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                        txtfifth.Text = drDebitNoteMaster["Field5"].ToString();

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

                        txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                        txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                        txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                        txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                        txtfifth.Text = drDebitNoteMaster["Field5"].ToString();

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

                        txtfirst.Text = drDebitNoteMaster["Field1"].ToString();
                        txtsecond.Text = drDebitNoteMaster["Field2"].ToString();
                        txtthird.Text = drDebitNoteMaster["Field3"].ToString();
                        txtfourth.Text = drDebitNoteMaster["Field4"].ToString();
                        txtfifth.Text = drDebitNoteMaster["Field5"].ToString();
                    }


                }

                dsDebitNoteInvoice.Tables["tblDebitNotMaster"].Rows.Add(cboSeriesName.Text, txtVchNo.Text, txtDate.Text, cboPartyAcc.Text,txtRemarks.Text);
                DataTable dtDebitNoteDetail = m_DebitNote.GetDebitNoteDetail(Convert.ToInt32(txtDebitNoteID.Text));
                for (int i = 1; i <= dtDebitNoteDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtDebitNoteDetail.Rows[i - 1];
                    grdDebitNote[i, (int)GridColumn.SNo].Value = i.ToString();
                    grdDebitNote[i, (int)GridColumn.Account].Value = drDetail["LedgerName"].ToString();
                   // grdDebitNote[i, 3].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdDebitNote[i, (int)GridColumn.Amount].Value = Convert.ToDecimal(drDetail["Amount"]).ToString();
                    grdDebitNote[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                    AddRowDebitNote(grdDebitNote.RowsCount);
                    dsDebitNoteInvoice.Tables["tblDebitNoteDetails"].Rows.Add(drDetail["LedgerName"].ToString(),Convert.ToDecimal(drDetail["Amount"]).ToString(),drDetail["Remarks"].ToString());
                 
                }
                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtDebitNoteID.Text), "DR_NOT");
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
            bool chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if(m_mode==EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Debit Note?") == DialogResult.Yes)
            //       btnSave_Click(sender, e);                

            //}            
               
                Navigation(Navigate.First);
        
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Debit Note?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //} 
           
            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Debit Note?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //} 
           
            Navigation(Navigate.Next);

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Debit Note?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //} 
           
            Navigation(Navigate.Last);
        }

        private void txtVchNo_TextChanged(object sender, EventArgs e)
        {
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

                    if (txtVchNo.Text == "")
                    {
                        txtVchNo.Text = "Main";
                        txtVchNo.Enabled = false;
                    }

                    if (NumberingType == "AUTOMATIC" && !m_VouConfig.GetIsVouHideType(SeriesID.ID))
                    {
                        txtVchNo.Enabled = true;
                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            //

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
                    if (m_DebitNoteID > 0)
                    {
                        lblVouNo.Visible = true;
                        txtVchNo.Visible = true;
                    }

                }//end if
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (DebitNoteIDCopy > 0)
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

        private void btnCopy_Click(object sender, EventArgs e)
        {
            DebitNoteIDCopy = Convert.ToInt32(txtDebitNoteID.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DEBIT_NOTE_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you don't have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            //ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            try
            {

                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the Debit Note - " + txtDebitNoteID.Text + "?") == DialogResult.Yes)
                {                    
                    DebitNote DelDebitNote = new DebitNote();
                    if (DelDebitNote.Delete(Convert.ToInt32(txtDebitNoteID.Text)))
                    {
                        Global.Msg("Debit note -" + txtDebitNoteID.Text + " deleted successfully!");
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
                        Global.MsgError("There was an error while deleting debit note -" + txtDebitNoteID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cboProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void grdDebitNote_Enter(object sender, EventArgs e)
        {
           
        }

        private void grdDebitNote_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtRemarks.Focus();
            }
        }

        private void cboSeriesName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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

        private void cboProjectName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboPartyAcc.Focus();
            }
        }

        private void cboPartyAcc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            grdDebitNote.Focus();
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

        private void frmDebitNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }

        private void cboPartyAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            PartyID = (ListItem)cboPartyAcc.SelectedItem;
            int ledgerid = PartyID.ID;
           // DataTable dtLdrInfo = Ledger.GetLedgerDetail("1", null, null, Convert.ToInt32(ledgerid));

            string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
            string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";

            DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(ledgerid), null);
            if (dtLdrInfo.Rows.Count <= 0)
            {
                lblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                return;
            }


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
            lblCurrentBalance.Text = strBalance;
            lblCurrentBalance.Text = strBalance;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
            dsDebitNoteInvoice.Clear();
            rptDebitNoteTransaction rpt = new rptDebitNoteTransaction();
            Misc.WriteLogo(dsDebitNoteInvoice, "tblImage");
            rpt.SetDataSource(dsDebitNoteInvoice);

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

            //pdvFont.Value = "Arial";
            //pvCollection.Clear();
            //pvCollection.Add(pdvFont);
            //rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

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

            // Navigation(Navigate.ID);
            bool empty = Navigation(Navigate.ID);
            if (empty == false)
            {
                return;
            }
            else
            {

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

                //frm.Show();
                frm.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }
    }
}
