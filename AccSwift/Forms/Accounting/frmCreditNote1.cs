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
using AccSwift.DataSet;
using AccSwift.CrystalReports;
using DateManager;

namespace AccSwift
{
    public partial class frmCreditNote : Form,IfrmAddAccClass, IfrmDateConverter, IfrmAccountLedger, ILOVLedger
    {
        //Check if the Account has been already set by LOVLedger
        bool hasChanged = false;
        private string Prefix = "";
        private int currRowPosition = 0;
        private string CurrBal = "";
        DevAge.Windows.Forms.DevAgeTextBox ctx; 
        private int CreditNoteIDCopy = 0;
        private int loopCounter = 0;
        List<int> AccClassID = new List<int>();
        ListItem SeriesID = new ListItem();
        ListItem PartyID = new ListItem();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private DataSet.dsCrediteNote dsCreditNote = new DataSet.dsCrediteNote();

        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        double m_DebitAmount = 0;//Holds the total debit amount on the Voucher
        double m_CreditAmount = 0;
        int m_CreditNoteID = 0;
        DataTable dtAccClassID = new DataTable();
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();

        public frmCreditNote()
        {
            InitializeComponent();
        }

        public frmCreditNote(int CreditNoteID)
        {
            InitializeComponent();
            this.m_CreditNoteID = CreditNoteID;
        }

        public void AddLedger(string LedgerName, int LedgerID, string CurrentBalance, bool IsSelected)
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
                int m_RecCount = (int)User.GetChildTable(AccClassID).Rows.Count;
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
        private void frmCreditNote_Load(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NEW);
            LoadComboboxProject(cboProjectName, 0);
            //ListProject(cboProjectName);
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
            foreach (DataRow drCreditorLedgers in dtCreditorLedgers.Rows)
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
            txtCreditNoteID.Visible = false;
            m_mode = EntryMode.NEW;

            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            try
            {
                //Event trigerred when delete button is clicked

                evtDelete.Click += new EventHandler(Delete_Row_Click);

                //Event when account is selected
                evtAccount.FocusLeft += new EventHandler(Account_Selected);
                evtAccountFocusLost.FocusLeft += new EventHandler(evtAccountFocusLost_FocusLeft);

                evtAmountFocusLost.FocusLeft += new EventHandler(Amount_Focus_Lost);

                grdCreditNote.Redim(2, 6);
                btnRowDelete.Image = global::AccSwift.Properties.Resources.gnome_window_close;

                //Prepare the header part for grid
                AddGridHeader();

                AddRowCreditNote(1);

                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID


                if (m_CreditNoteID > 0)
                {
                    //Show the values in fields

                    try
                    {

                        ChangeState(EntryMode.NORMAL);

                        int vouchID = 0;
                        try
                        {
                            vouchID = m_CreditNoteID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }

             
                        CreditNote m_CreditNote = new CreditNote();

                        //Getting the value of SeriesID via MasterID or VouchID

                        int SeriesID = m_CreditNote.GetSeriesIDFromMasterID(vouchID);

                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesID);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Credit Note");
                            cboSeriesName.Text = "";

                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            cboSeriesName.Text = dr["EngName"].ToString();

                        }

                        DataTable dtCreditNoteMaster = m_CreditNote.GetCreditNoteMaster(vouchID);

                        if (dtCreditNoteMaster.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }


                        DataRow drCreditNoteMaster = dtCreditNoteMaster.Rows[0];
                        txtVchNo.Text = drCreditNoteMaster["Voucher_No"].ToString();
                        txtDate.Text = Date.DBToSystem(drCreditNoteMaster["CreditNote_Date"].ToString());
                        txtRemarks.Text = drCreditNoteMaster["Remarks"].ToString();
                        txtCreditNoteID.Text = drCreditNoteMaster["CreditNoteID"].ToString();
                        dsCreditNote.Tables["tblCreditNoteMaster"].Rows.Add(cboSeriesName.Text, drCreditNoteMaster["Voucher_No"].ToString(), Date.DBToSystem(drCreditNoteMaster["CreditNote_Date"].ToString()), cboPartyAcc.Text, drCreditNoteMaster["Remarks"].ToString());

                        DataTable dtCreditNoteDetail = m_CreditNote.GetCreditNoteDetail(Convert.ToInt32(drCreditNoteMaster["CreditNoteID"]));


                        for (int i = 1; i <= dtCreditNoteDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtCreditNoteDetail.Rows[i - 1];

                            grdCreditNote[i, 1].Value = i.ToString();
                            grdCreditNote[i, 2].Value = drDetail["LedgerName"].ToString();
                            grdCreditNote[i, 3].Value = drDetail["Amount"].ToString();
                            grdCreditNote[i, 4].Value = drDetail["Remarks"].ToString();
                            AddRowCreditNote(grdCreditNote.RowsCount);
                            dsCreditNote.Tables["tblCreditNoteDetails"].Rows.Add(drDetail["LedgerName"].ToString(), drDetail["Amount"].ToString(), drDetail["Remarks"].ToString());
                           
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

        /// <summary>
        /// Writes the header part for grdJournal
        /// </summary>
        private void AddGridHeader()
        {
            grdCreditNote[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
            grdCreditNote[0, 1] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdCreditNote[0, 2] = new SourceGrid.Cells.ColumnHeader("Account");          
            grdCreditNote[0, 3] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdCreditNote[0, 4] = new SourceGrid.Cells.ColumnHeader("Remarks");
            grdCreditNote[0, 5] = new SourceGrid.Cells.ColumnHeader("Ledger ID");
            grdCreditNote[0, 0].Column.Width = 25;
            grdCreditNote[0, 2].Column.Width = 200;
            grdCreditNote[0, 3].Column.Width = 200;
            grdCreditNote[0, 4].Column.Width = 200;
            grdCreditNote[0, 5].Column.Width = 50;
            grdCreditNote[0, 5].Column.Visible = false;
        }

        /// <summary>
        /// Adds the row in the DebitNote field
        /// </summary>
        private void AddRowCreditNote(int RowCount)
        {
            //Add a new row
            grdCreditNote.Redim(Convert.ToInt32(RowCount + 1), grdCreditNote.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::AccSwift.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            grdCreditNote[i, 0] = btnDelete;
            grdCreditNote[i, 0].AddController(evtDelete);
            grdCreditNote[i, 1] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdCreditNote[i, 2] = new SourceGrid.Cells.Cell("", txtAccount);
            txtAccount.Control.GotFocus += new EventHandler(Account_Focused);
            txtAccount.Control.LostFocus += new EventHandler(Account_Leave);
            txtAccount.Control.KeyDown += new KeyEventHandler(Account_KeyDown);
            grdCreditNote[i, 2].AddController(evtAccountFocusLost);
            grdCreditNote[i, 2].Value = "(NEW)"; 
          
            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;

            grdCreditNote[i, 3] = new SourceGrid.Cells.Cell("", txtAmount);
            grdCreditNote[i, 3].AddController(evtAmountFocusLost);
            //grdCreditNote[i, 3].Value = "(NEW)";
            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;

            grdCreditNote[i, 4] = new SourceGrid.Cells.Cell("", txtRemarks);
           // grdCreditNote[i, 4].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.Focus;
            grdCreditNote[i, 5] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdCreditNote[i, 5].Value = "";

        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdCreditNote.RowsCount - 2)
                grdCreditNote.Rows.Remove(ctx.Position.Row);         
        }

        /// <summary>
        /// Sums up all the debit and credit amounts on the voucher grid
        /// </summary>
        //private void CalculateDrCr()
        //{


        //    try
        //    {
        //        double dr, cr;
        //        dr = cr = 0;
        //        for (int i = 1; i < grdCreditNote.RowsCount - 1; i++)
        //        {
        //            //Check if it is the (NEW) row. If it is, it must be the last row.
        //            if (i == grdCreditNote.Rows.Count)
        //                return;

        //            double m_Amount = 0;
        //            string m_Value = Convert.ToString(grdCreditNote[i, 4].Value);//Had to do this because it showed error when the cell was left blank
        //            if (m_Value.Length == 0)
        //                m_Amount = 0;
        //            else
        //                m_Amount = Convert.ToDouble(grdCreditNote[i, 4].Value);

        //            if (grdCreditNote[i, 3].Value.ToString() == "Debit")
        //                dr += m_Amount;
        //            else if (grdCreditNote[i, 3].Value.ToString() == "Credit")
        //                cr += m_Amount;
        //        }
        //        m_DebitAmount = dr;
        //        m_CreditAmount = cr;

        //        lblDebitTotal.Text = m_DebitAmount.ToString();
        //        lblCreditTotal.Text = m_CreditAmount.ToString();
        //        lblDifferenceAmount.Text = (Convert.ToDouble(m_DebitAmount) - Convert.ToDouble(m_CreditAmount)).ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MsgError("Error in Debit/Credit calucation!");
        //    }


        //}

        private void Account_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                //if (ctx.Text == "(NEW)") ctx.Text = "";
                frmLOVLedger frm = new frmLOVLedger(this);
                frm.ShowDialog();
            }  
        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmLOVLedger frm = new frmLOVLedger(this, e);
            frm.ShowDialog();  
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
            //int RowCount = grdCreditNote.RowsCount;
            //string CurRow = (string)grdCreditNote[RowCount - 1, 2].Value;
            ////Check whether the new row is already added
            //if (CurRow != "(NEW)")
            //{
            //    AddRowCreditNote(RowCount);
            //    //Clear (NEW) on other colums as well
            //    ClearNew(RowCount - 1);
            //}
        }

        private void Account_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
        }

        private void evtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int a = ctx.Position.Row;
            grdCreditNote[a, 5].Value = currRowPosition;
        }

        /// <summary>
        /// Clears the (NEW) in the newly created row just after insertion of new Data in grid
        /// </summary>
        /// <param name="RowCount"></param>
        private void ClearNew(int RowCount)
        {
            if (grdCreditNote[RowCount, 2].Value == "(NEW)")
                grdCreditNote[RowCount, 2].Value = "";
            if (grdCreditNote[RowCount, 3].Value == "(NEW)")
                grdCreditNote[RowCount, 3].Value = "";
            if (grdCreditNote[RowCount, 4].Value == "(NEW)")
                grdCreditNote[RowCount, 4].Value = "";
        }

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdCreditNote.RowsCount;
            string CurRow = (string)grdCreditNote[RowCount - 1, 2].Value;
            if (CurRow != "(NEW)")
            {
                AddRowCreditNote(RowCount);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CREDIT_NOTE_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
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
            #endregion
            //Check Validation
            if (!Validate())
                return;
            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }
            ListItem liProjectId = new ListItem();
            liProjectId = (ListItem) cboProjectName.SelectedItem;           
            switch (m_mode)
            {
                    // Incase of CreditNote,the ledgers of Detail section are resposible for payment soo no need to check negative cash and negative bank
                    //similary ,in Master part  is the list of Debtor and Creditor ledgers soo ,obviously no need to check negative cash and negative bank
                    //finally,no need to check negative cash and negative bank in both cash i.e. Master section and Detail section

                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable CreditNoteDetails = new DataTable();
                        CreditNoteDetails.Columns.Add("Ledger");                 
                        CreditNoteDetails.Columns.Add("Amount");
                        CreditNoteDetails.Columns.Add("Remarks");
                        for (int i = 0; i < grdCreditNote.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdCreditNote[i + 1, 2].ToString().Split('[');
                            CreditNoteDetails.Rows.Add(ledgerName[0].ToString(), grdCreditNote[i + 1, 3].Value, grdCreditNote[i + 1, 4].Value);
                        }
                        ICreditNote m_CreditNote = new CreditNote();
                        DateTime CreditNoteDate = Date.ToDotNet(txtDate.Text);

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        ListItem liLedgerID = new ListItem();
                        liLedgerID = (ListItem)cboPartyAcc.SelectedItem;
                        if (AccClassID.Count != 0)
                        {
                            m_CreditNote.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, CreditNoteDate, txtRemarks.Text, CreditNoteDetails, AccClassID.ToArray(),Convert.ToInt32(liProjectId.ID));
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_CreditNote.Create(Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, CreditNoteDate, txtRemarks.Text, CreditNoteDetails, a.ToArray(),Convert.ToInt32(liProjectId.ID));

                        
                        }
                        Global.Msg("Credit Note created successfully!");
                        //ChangeState(EntryMode.NORMAL);
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
                        DataTable CreditNoteDetails = new DataTable();

                        CreditNoteDetails.Columns.Add("Ledger");
                        CreditNoteDetails.Columns.Add("Amount");
                        CreditNoteDetails.Columns.Add("Remarks");

                        for (int i = 0; i < grdCreditNote.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            CreditNoteDetails.Rows.Add(grdCreditNote[i + 1, 2].Value, grdCreditNote[i + 1, 3].Value, grdCreditNote[i + 1, 4].Value);
                        }
                        ICreditNote m_CreditNote = new CreditNote();
                        DateTime CreditNoteDate = Date.ToDotNet(txtDate.Text);
                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        ListItem liLedgerID = new ListItem();
                        liLedgerID = (ListItem)cboPartyAcc.SelectedItem;
                        if (AccClassID.Count != 0)
                        {
                            m_CreditNote.Modify(Convert.ToInt32(txtCreditNoteID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, CreditNoteDate, txtRemarks.Text, CreditNoteDetails, AccClassID.ToArray(),Convert.ToInt32(liProjectId.ID));
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_CreditNote.Modify(Convert.ToInt32(txtCreditNoteID.Text), Convert.ToInt32(SeriesID.ID), Convert.ToInt32(liLedgerID.ID), txtVchNo.Text, CreditNoteDate, txtRemarks.Text, CreditNoteDetails, a.ToArray(),Convert.ToInt32(liProjectId.ID));
                        }
                        Global.Msg("Credit Note modified successfully!");
                        //ChangeState(EntryMode.NORMAL);
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
            if (!(grdCreditNote.Rows.Count > 2))
            {
                Global.MsgError("Invalid Account Ledger Selected in grid");
                grdCreditNote.Focus();
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
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("CR_NOT");
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
            txtVchNo.Enabled = txtDate.Enabled = txtRemarks.Enabled = grdCreditNote.Enabled =cboSeriesName.Enabled=cboPartyAcc.Enabled=tabControl1.Enabled= Enable;
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
            bool chkUserPermission = UserPermission.ChkUserPermission("CREDIT_NOTE_CREATE_MODIFY");
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
        }

        /// <summary>
        /// Clears the whole voucher
        /// </summary>
        private void ClearVoucher()
        {
            ClearCreditNote();
            treeAccClass.Nodes.Clear();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdCreditNote.Redim(2, 6);
            AddGridHeader(); //Write header part
            AddRowCreditNote(1);
        }

        //Clears the text of every field of Journal form
        private void ClearCreditNote()
        {
            cboSeriesName.Text = string.Empty;
            cboPartyAcc.Text = string.Empty;
            txtVchNo.Clear(); //actually generate a new voucher no.
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtRemarks.Clear();
            grdCreditNote.Rows.Clear();
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
                    VouchID = Convert.ToInt32(txtCreditNoteID.Text);
                    if (CreditNoteIDCopy > 0) //Copy mode, so simply paste the id
                    {
                        VouchID = CreditNoteIDCopy;
                        CreditNoteIDCopy = 0;
                    }
                    else //if not in copy mode
                    {
                        VouchID = Convert.ToInt32(txtCreditNoteID.Text);
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                ICreditNote m_CreditNote = new CreditNote();
                DataTable dtCreditNoteMaster = m_CreditNote.NavigateCreditNoteMaster(VouchID, NavTo);
                if (dtCreditNoteMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    return false;
                }

                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                DataRow drCreditNoteMaster = dtCreditNoteMaster.Rows[0]; //There is only one row. First row is the required record
                //Show the corresponding Party Account Ledger in Combobox
                DataTable dtPartyLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCreditNoteMaster["LedgerID"]), LangMgr.DefaultLanguage);
                foreach (DataRow drPartyLedgerInfo in dtPartyLedgerInfo.Rows)
                {
                    cboPartyAcc.Text = drPartyLedgerInfo["LedName"].ToString();
                }
       
                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drCreditNoteMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Credit Note");
                    cboSeriesName.Text = "";
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();

                }
                txtVchNo.Text = drCreditNoteMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drCreditNoteMaster["CreditNote_Date"].ToString());
                txtRemarks.Text = drCreditNoteMaster["Remarks"].ToString();
                txtCreditNoteID.Text = drCreditNoteMaster["CreditNoteID"].ToString();
                dsCreditNote.Tables["tblDebitNoteMaster"].Rows.Add(cboSeriesName.Text, drCreditNoteMaster["Voucher_No"].ToString(), Date.DBToSystem(drCreditNoteMaster["CreditNote_Date"].ToString()), cboPartyAcc.Text, drCreditNoteMaster["Remarks"].ToString());
                DataTable dtCreditNoteDetail = m_CreditNote.GetCreditNoteDetail(Convert.ToInt32(txtCreditNoteID.Text));
                for (int i = 1; i <= dtCreditNoteDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtCreditNoteDetail.Rows[i - 1];

                    grdCreditNote[i, 1].Value = i.ToString();
                    grdCreditNote[i, 2].Value = drDetail["LedgerName"].ToString();
                    grdCreditNote[i, 3].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdCreditNote[i, 4].Value = drDetail["Remarks"].ToString();
                    AddRowCreditNote(grdCreditNote.RowsCount);
                    dsCreditNote.Tables["tblDebitNoteDetails"].Rows.Add(drDetail["LedgerName"].ToString(), drDetail["Amount"].ToString(), drDetail["Remarks"].ToString());

                }

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtCreditNoteID.Text), "CR_NOT");
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
            bool chkUserPermission = UserPermission.ChkUserPermission("CREDIT_NOTE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Credit Note?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }
          
            Navigation(Navigate.First);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CREDIT_NOTE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Credit Note?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }
           
            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CREDIT_NOTE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Credit Note?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }
           
            Navigation(Navigate.Next);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CREDIT_NOTE_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Credit Note?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }
           
            Navigation(Navigate.Last);
        }

        private void cboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                  //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    SeriesID = (ListItem)cboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));
                    txtVchNo.Enabled = true;
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

        private void btnNew_Click(object sender, EventArgs e)
        {

            bool chkUserPermission = UserPermission.ChkUserPermission("CREDIT_NOTE_CREATE_MODIFY");
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

        private void btnCopy_Click(object sender, EventArgs e)
        {
            CreditNoteIDCopy = Convert.ToInt32(txtCreditNoteID.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (CreditNoteIDCopy > 0)
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
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CREDIT_NOTE_DELETE");
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
            //ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            try
            {

                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the Credit Note - " + txtCreditNoteID.Text + "?") == DialogResult.Yes)
                {                    
                    CreditNote DelCreditNote = new CreditNote();
                    if (DelCreditNote.Delete(Convert.ToInt32(txtCreditNoteID.Text)))
                    {
                        Global.Msg("Credit Note -" + txtCreditNoteID.Text + " deleted successfully!");
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
                        Global.MsgError("There was an error while deleting Credit Note -" + txtCreditNoteID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }


        }

        private void frmCreditNote_KeyDown(object sender, KeyEventArgs e)
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
            DataTable dtLdrInfo = Ledger.GetLedgerDetail("1", null, null, Convert.ToInt32(ledgerid));
            if (dtLdrInfo.Rows.Count <= 0)
            {
                lblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                return;
            }

            DataRow dr = dtLdrInfo.Rows[0];

            if (dr["Debit"] == DBNull.Value || Convert.ToInt32(dr["Debit"]) == 0)
            {
                if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                {
                    lblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    lblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }
                else
                {
                    lblCurrentBalance.Text = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                    lblCurrentBalance.Text = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                }
            }
            else
            {
                if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                {
                    lblCurrentBalance.Text = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                    lblCurrentBalance.Text = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                }
                else
                {
                    if (Convert.ToDecimal(dr["Debit"]) == Convert.ToDecimal(dr["Credit"]))
                    {
                        lblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        lblCurrentBalance.Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }

                    if (Convert.ToDecimal(dr["Debit"]) > Convert.ToDecimal(dr["Credit"]))
                    {
                        lblCurrentBalance.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                        lblCurrentBalance.Text = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                    }
                    if (Convert.ToDecimal(dr["Debit"]) < Convert.ToDecimal(dr["Credit"]))
                    {
                        lblCurrentBalance.Text = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                        lblCurrentBalance.Text = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                    }
                }
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
      
    }
}
