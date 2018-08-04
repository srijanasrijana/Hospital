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
using AccSwift.CrystalReports;
using AccSwift.DataSet;
using DateManager;


namespace AccSwift
{
    public partial class frmContra : Form, IfrmAddAccClass, IfrmDateConverter
    {
        private bool IsShortcutKey = false;
        ListItem liProjectID = new ListItem();
        private bool IsFieldChanged = false;
        private bool IsNegativeCash = false;
        private bool IsNegativeBank = false;
        private int ContraIDCopy = 0;
        private int loopCounter = 0;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        SourceGrid.CellContext ctx1 = new SourceGrid.CellContext();
        private bool IsChequeDateButton = false;
        ListItem SeriesID = new ListItem();
        ArrayList AccountClassID = new ArrayList();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        double m_DebitAmount = 0;//Holds the total debit amount on the Voucher
        double m_CreditAmount = 0;
        private DataSet.dsContra dsContra = new DataSet.dsContra();
        List<int> AccClassID = new List<int>();
        int m_ContraID = 0;
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtChequeDateFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        Contra m_Contra = new Contra();

        public frmContra()
        {
            InitializeComponent();
        }

        public frmContra(int ContraID)
        {
            InitializeComponent();
            this.m_ContraID = ContraID;
        }

        public void DateConvert(DateTime DotNetDate)
        {
            //txtDate.Text = Date.ToSystem(DotNetDate);
             if (!IsChequeDateButton)
                 txtDate.Text = Date.ToSystem(DotNetDate);
             if (IsChequeDateButton)
                 ctx1.Value = Date.ToSystem(DotNetDate);
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

            DataTable dtUserInfo = User.GetUserInfo(User.CurrUser); //user id must be read from  global i.e current user id
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

        private void frmContra_Load(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NEW);          
            txtContraID.Visible = false;
            ListProject(cboProjectName);
            ShowAccClassInTreeView(treeAccClass, null, 0);
            m_mode = EntryMode.NEW;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
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

                evtChequeDateFocusLost.Click += new EventHandler(ChequeDate_Click);
                evtChequeDateFocusLost.Click += new EventHandler(Text_Change);    
                #endregion                                

                grdContra.Redim(2,11);
                btnRowDelete.Image = global::AccSwift.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                AddGridHeader();
                AddRowContra(1);

                #region Navigation at form load
                if (m_ContraID > 0)
                {
                    //Show the values in fields
                    try
                    {
                        ChangeState(EntryMode.NORMAL);
                        int vouchID = 0;
                        try
                        {
                            vouchID = m_ContraID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }
                        Contra m_Contra = new Contra();
                        //Getting SeriesID from MasterID

                        int SeriesID = m_Contra.GetSeriesIDFromMasterID(vouchID);

                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesID);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Contra");
                            cboSeriesName.Text = "";
                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            cboSeriesName.Text = dr["EngName"].ToString();
                        }
                        DataTable dtContraMasterInfo= m_Contra.GetContraMasterInfo(vouchID);

                        if (dtContraMasterInfo.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }
                        DataRow drContraMaster = dtContraMasterInfo.Rows[0];
                        txtVchNo.Text = drContraMaster["Voucher_No"].ToString();
                        txtDate.Text = Date.DBToSystem(drContraMaster["Contra_Date"].ToString());
                        txtRemarks.Text = drContraMaster["Remarks"].ToString();
                        txtContraID.Text= drContraMaster["ContraID"].ToString();
                        dsContra.Tables["tblConraMaster"].Rows.Add(cboSeriesName.Text, drContraMaster["Voucher_No"].ToString(), Date.DBToSystem(drContraMaster["Contra_Date"].ToString()), drContraMaster["Remarks"].ToString());
                        DataTable dtContraDetail = m_Contra.GetContraDetail(Convert.ToInt32(drContraMaster["ContraID"]));
                        for (int i = 1; i <= dtContraDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtContraDetail.Rows[i - 1];
                            grdContra[i, 1].Value = i.ToString();
                            grdContra[i, 2].Value = drDetail["LedgerName"].ToString();
                            grdContra[i, 3].Value = drDetail["DrCr"].ToString();
                            grdContra[i, 4].Value = drDetail["Amount"].ToString();
                            grdContra[i, 5].Value = drDetail["Remarks"].ToString();
                            AddRowContra(grdContra.RowsCount);
                            dsContra.Tables["tblContraDetails"].Rows.Add(drDetail["LedgerName"].ToString(), drDetail["DrCr"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());

                        }
                        CalculateDrCr();
                    }                  
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                //grid1.AutoSizeCells();
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
            if (ctx.Position.Row <= grdContra.RowsCount - 2)
                grdContra.Rows.Remove(ctx.Position.Row);

            CalculateDrCr();
        }

        private void evtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;

            }
            catch (Exception ex)
            {
            }

            if (ct.DisplayText == "")
                return;

            int RowCount = grdContra.RowsCount;
            //Add a new row

            string CurRow = (string)grdContra[RowCount - 1, 2].Value;

            //Check whether the new row is already added
            if (CurRow != "(NEW)")
            {
                AddRowContra(RowCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowCount - 1);
            }
        }

        private void Account_Leave(object sender, EventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;
            string ledgerName = cbo.Text.ToString();
            if (cbo.Text.ToString().Contains('['))
            {
                string[] split = ledgerName.Split(new Char[] { '[' });
                ledgerName = split[0].ToString();
            }
            else
            {
                ledgerName = cbo.Text.ToString();
            }

            //Check if Ledger name is blank or (NEW)
            if ((ledgerName.ToUpper() == "(NEW)") || String.IsNullOrEmpty(ledgerName))
                return;

            //Check if the ledger exists
            int LedgerID = Ledger.GetLedgerIdFromName(ledgerName, LangMgr.DefaultLanguage);
            if (LedgerID <= 0)
            {
                if (Global.MsgQuest("The specified ledger does not seem to exist. Do you want to create a new ledger with the given name?") == DialogResult.Yes)
                {
                    frmAccountSetup frm = new frmAccountSetup(this, ledgerName);
                    frm.ShowDialog();
                }
            }
        }

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            CalculateDrCr();
        }

        private void AddGridHeader()
        {
            grdContra[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
            grdContra[0, 1] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdContra[0, 2] = new SourceGrid.Cells.ColumnHeader("Account");
            grdContra[0, 3] = new SourceGrid.Cells.ColumnHeader("Dr/Cr");
            grdContra[0, 4] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdContra[0, 5] = new SourceGrid.Cells.ColumnHeader("Current Balance");
            grdContra[0, 6] = new SourceGrid.Cells.ColumnHeader("Cheque Number");
            grdContra[0, 7] = new SourceGrid.Cells.ColumnHeader("Cheque Date");
            grdContra[0, 8] = new SourceGrid.Cells.ColumnHeader("Remarks");
            grdContra[0, 9] = new SourceGrid.Cells.ColumnHeader("Ledger ID");
            grdContra[0, 10] = new SourceGrid.Cells.ColumnHeader("Current Balance");

            grdContra[0, 0].Column.Width = 50;
            grdContra[0, 1].Column.Width = 60;
            grdContra[0, 2].Column.Width = 200;
            grdContra[0, 3].Column.Width = 50;
            grdContra[0, 4].Column.Width = 100;
            grdContra[0, 5].Column.Width = 100;
            grdContra[0, 6].Column.Width = 150;
            grdContra[0, 7].Column.Width = 150;
            grdContra[0, 8].Column.Width = 100;

            grdContra[0, 9].Column.Visible=false;
            grdContra[0, 10].Column.Visible = false;
            grdContra[0, 5].Column.Visible = false;


            //grdContra[0, 7].Column.Visible = false;
            //grdContra[0, 8].Column.Visible = false;

        }

        /// <summary>
        /// Adds the row in the Contra field
        /// </summary>
        private void AddRowContra(int RowCount)
        {
           

            //Add a new row
            grdContra.Redim(Convert.ToInt32(RowCount + 1), grdContra.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::AccSwift.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;
            grdContra[i, 0] = btnDelete;
            grdContra[i, 0].AddController(evtDelete);

            grdContra[i, 1] = new SourceGrid.Cells.Cell(i.ToString()); 
                   
            string[] Cash = Ledger.GetLedgerList(102);//Getting All Ledger corresponding to Cash Account
            SourceGrid.Cells.Editors.ComboBox cboAccount = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            string[] Bank = Ledger.GetLedgerList(7);
            string[] Cash_Bank = new string[Cash.Length + Bank.Length];
            Cash.CopyTo(Cash_Bank, 0);
            Bank.CopyTo(Cash_Bank, Cash.Length);
            cboAccount.StandardValues = Cash_Bank;           
            cboAccount.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboAccount.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboAccount.Control.LostFocus += new EventHandler(evtAccountFocusLost_FocusLeft);
            cboAccount.Control.LostFocus += new EventHandler(Account_Leave);
            cboAccount.EditableMode = SourceGrid.EditableMode.Focus;

            grdContra[i, 2] = new SourceGrid.Cells.Cell("", cboAccount);
            grdContra[i, 2].AddController(evtAccountFocusLost);
            grdContra[i, 2].Value = "(NEW)";


            SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
            cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;

            cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;

            string strDrCr = "Debit";

            if (grdContra[i - 1, 3].Value.ToString() == "Debit")
                strDrCr = "Credit";

            grdContra[i, 3] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;

            grdContra[i, 4] = new SourceGrid.Cells.Cell("", txtAmount);
            grdContra[i, 4].AddController(evtAmountFocusLost);
            grdContra[i, 4].Value = "(NEW)";

           

            SourceGrid.Cells.Editors.TextBox txtchequenumber = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtchequenumber.EditableMode = SourceGrid.EditableMode.Focus;

            grdContra[i, 6] = new SourceGrid.Cells.Cell("", txtchequenumber);
            grdContra[i, 6].Value = "(NEW)";

            SourceGrid.Cells.Button btnChequeDate = new SourceGrid.Cells.Button(""); //Date.ToSystem(DateTime.Today)
            txtchequenumber.EditableMode = SourceGrid.EditableMode.SingleClick;
           // btnChequeDate.Controller.OnClick += new EventHandler(Text_Change);
            grdContra[i, 7] = btnChequeDate;
            grdContra[i, 7].AddController(evtChequeDateFocusLost);

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;

            grdContra[i, 8] = new SourceGrid.Cells.Cell("", txtRemarks);
            grdContra[i, 8].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdContra[i, 9] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdContra[i, 9].Value = "";

            SourceGrid.Cells.Editors.TextBox txtCurrBal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrBal.EditableMode = SourceGrid.EditableMode.None;
            grdContra[i, 10] = new SourceGrid.Cells.Cell("", txtCurrBal);
            grdContra[i, 10].Value = "";  

           

        }

        private void CalculateDrCr()
        {
            try
            {
                double dr, cr;
                dr = cr = 0;
                for (int i = 1; i < grdContra.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row. If it is, it must be the last row.
                    if (i == grdContra.Rows.Count)
                        return;

                    double m_Amount = 0;
                    string m_Value = Convert.ToString(grdContra[i, 4].Value);//Had to do this because it showed error when the cell was left blank
                    if (m_Value.Length == 0)
                        m_Amount = 0;
                    else
                        m_Amount = Convert.ToDouble(grdContra[i, 4].Value);

                    if (grdContra[i, 3].Value.ToString() == "Debit")
                        dr += m_Amount;
                    else if (grdContra[i, 3].Value.ToString() == "Credit")
                        cr += m_Amount;
                }
                m_DebitAmount = dr;
                m_CreditAmount = cr;

                lblDebitTotal.Text = m_DebitAmount.ToString();
                lblCreditTotal.Text = m_CreditAmount.ToString();
                lblDifferenceAmount.Text = (Convert.ToDouble(m_DebitAmount) - Convert.ToDouble(m_CreditAmount)).ToString();
            }
            catch (Exception ex)
            {
                Global.MsgError("Error in Debit/Credit calucation!");
            }
        }

        private void ClearNew(int RowCount)
        {
            if (grdContra[RowCount, 2].Value == "(NEW)")
                grdContra[RowCount, 2].Value = "";
            if (grdContra[RowCount, 3].Value == "(NEW)")
                grdContra[RowCount, 3].Value = "";
            if (grdContra[RowCount, 4].Value == "(NEW)")
                grdContra[RowCount, 4].Value = "";
            //if (grdContra[RowCount, 5].Value == "(NEW)")
            //    grdContra[RowCount, 5].Value = "";

        }

        //A function from the Interface IfrmAccClassID. Used to apply the Datatable to this form from AddAccClass Form
        private void btnSave_Click(object sender, EventArgs e)
        {
             bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_CREATE_MODIFY");
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
            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag   in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
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

            //Check whether dr. and cr. total amount is equal
            if ((Convert.ToDouble(m_DebitAmount) - Convert.ToDouble(m_CreditAmount)) != 0)
            {
                Global.Msg("Debit and Credit amount are not equal!");
                return;
            }
            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable ContraDetails = new DataTable();
                        ContraDetails.Columns.Add("Ledger");
                        ContraDetails.Columns.Add("DrCr");
                        ContraDetails.Columns.Add("Amount");
                        ContraDetails.Columns.Add("Remarks");
                        ContraDetails.Columns.Add("Cheque Number");
                        ContraDetails.Columns.Add("Cheque Date");

                        for (int i = 0; i < grdContra.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdContra[i + 1, 2].ToString().Split('[');
                            ContraDetails.Rows.Add(ledgerName[0].ToString(), grdContra[i + 1, 3].Value, grdContra[i + 1, 4].Value, grdContra[i + 1, 5].Value);
                            bool isCashAccount = false;
                            double totalDrCash, totalCrCash;
                            totalDrCash = totalCrCash = 0;
                            isCashAccount = Ledger.IsCashAccount(ledgerName[0].ToString());
                            //Block for checking Negative Cash and Negative bank

                            //Check the negative cash and negative Bank  ledger who is responsible for payment...so,If sorcegrid is Debit,no need to check because it will recieve amount bt check negative cash and negative bank in case of Credit
                            if (grdContra[i + 1, 3].Value.ToString() == "Credit")//check negative cash and negative bank in case of ledger is responisble of payment otherwise no need to check...
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
                                        if (grdContra[i + 1, 3].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrCash =  Convert.ToDouble(grdContra[i + 1, 4].Value);//In case of Cash and Bank ...Ledger Balance is Debit
                                        }
                                        else if (grdContra[i + 1, 3].Value.ToString() == "Credit")
                                        {
                                            totalCrCash = Convert.ToDouble(grdContra[i + 1, 4].Value);//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
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

                                        Transaction.GetLedgerBalance(null,null,bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode,0);//we dont need to check according to project soo ProjecID is kept as zero
                                        //Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        if (grdContra[i + 1, 3].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrBank = Convert.ToDouble(grdContra[i + 1, 4].Value);

                                        }
                                        else if (grdContra[i + 1, 3].Value.ToString() == "Credit")//If ledger posted in Grid is Credit Type
                                        {
                                            totalCrBank =  Convert.ToDouble(grdContra[i + 1, 4].Value);
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
                    
                        DateTime ContraDate = Date.ToDotNet(txtDate.Text);

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;
                        if (AccClassID.Count != 0)
                        {
                            m_Contra.Create(Convert.ToInt32(SeriesID.ID), txtVchNo.Text, ContraDate, txtRemarks.Text, ContraDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID));
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_Contra.Create(Convert.ToInt32(SeriesID.ID), txtVchNo.Text, ContraDate, txtRemarks.Text, ContraDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID));

                        
                        }
                        Global.Msg("Contra created successfully!");
                        ChangeState(EntryMode.NORMAL);

                        
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
                        DataTable ContraDetails = new DataTable();
                        ContraDetails.Columns.Add("Ledger");
                        ContraDetails.Columns.Add("DrCr");
                        ContraDetails.Columns.Add("Amount");
                        ContraDetails.Columns.Add("Remarks");

                        for (int i = 0; i < grdContra.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdContra[i + 1, 2].ToString().Split('[');
                            ContraDetails.Rows.Add(grdContra[i + 1, 2].Value, grdContra[i + 1, 3].Value, grdContra[i + 1, 4].Value, grdContra[i + 1, 5].Value);
                            bool isCashAccount = false;
                            double totalDrCash, totalCrCash;
                            totalDrCash = totalCrCash = 0;
                            isCashAccount = Ledger.IsCashAccount(ledgerName[0].ToString());
                            //Block for checking Negative Cash and Negative bank

                            //Check the negative cash and negative Bank  ledger who is responsible for payment...so,If sorcegrid is Debit,no need to check because it will recieve amount bt check negative cash and negative bank in case of Credit
                            if (grdContra[i + 1, 3].Value.ToString() == "Credit")//check negative cash and negative bank in case of ledger is responisble of payment otherwise no need to check...
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
                                        if (grdContra[i + 1, 3].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrCash =  Convert.ToDouble(grdContra[i + 1, 4].Value);//In case of Cash and Bank ...Ledger Balance is Debit
                                        }
                                        else if (grdContra[i + 1, 3].Value.ToString() == "Credit")
                                        {
                                            totalCrCash =  Convert.ToDouble(grdContra[i + 1, 4].Value);//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
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
                                        if (grdContra[i + 1, 3].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrBank =  Convert.ToDouble(grdContra[i + 1, 4].Value);//In case of Cash and Bank ...Ledger Balance is Debit

                                        }
                                        else if (grdContra[i + 1, 3].Value.ToString() == "Credit")
                                        {
                                            totalCrBank =  Convert.ToDouble(grdContra[i + 1, 4].Value);//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
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

                        DateTime Contra_Date = Date.ToDotNet(txtDate.Text);
                        Contra m_Contra = new Contra();
                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;
                        if (AccClassID.Count != 0)
                        {
                            m_Contra.Modify(Convert.ToInt32(txtContraID.Text), Convert.ToInt32(SeriesID.ID), txtVchNo.Text, Contra_Date, txtRemarks.Text, ContraDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID));
                        }
                        else 
                        {
                            int[] a = new int[] { 1 };
                            m_Contra.Modify(Convert.ToInt32(txtContraID.Text), Convert.ToInt32(SeriesID.ID), txtVchNo.Text, Contra_Date, txtRemarks.Text, ContraDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID));                        
                        }
                        Global.Msg("Contra modified successfully!");
                        ChangeState(EntryMode.NORMAL);
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
            if (!(grdContra.Rows.Count > 2))
            {
                Global.MsgError("Invalid Account Ledger Selected in grid");
                grdContra.Focus();
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
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("CNTR");
            cboSeriesName.Items.Clear();
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                cboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboSeriesName.SelectedIndex = 0;
            cboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            cboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
          
        }

        private void EnableControls(bool Enable)
        {
            txtVchNo.Enabled = txtDate.Enabled = txtRemarks.Enabled = grdContra.Enabled = cboSeriesName.Enabled = Enable;
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

        private void ClearVoucher()
        {
            ClearContra();
            grdContra.Redim(2, 6);
            AddGridHeader(); //Write header part
            AddRowContra(1);
        }

        private void ClearContra()
        {
            txtVchNo.Clear(); //actually generate a new voucher no.
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtRemarks.Clear();
            grdContra.Rows.Clear();
            cboProjectName.SelectedIndex = 0;
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
                    VouchID = Convert.ToInt32(txtContraID.Text);
                    if (ContraIDCopy > 0)
                    {
                        VouchID = ContraIDCopy;
                        ContraIDCopy = 0;
                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtContraID.Text);
                    
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
                DataTable dtContraMaster = m_Contra.NavigateContraMaster(VouchID, NavTo);
                if (dtContraMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    return false;
                }

                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                DataRow drContraMaster = dtContraMaster.Rows[0]; //There is only one row. First row is the required record
                if (IsShortcutKey)
                {
                    txtRemarks.Text = drContraMaster["Remarks"].ToString();
                    IsShortcutKey = false;
                    txtRemarks.SelectionStart = txtRemarks.Text.Length + 1;
                    return false;
                }        

                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drContraMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Contra");
                    cboSeriesName.Text = "";                    
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();
                }
                txtVchNo.Text = drContraMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drContraMaster["Contra_Date"].ToString());
                txtRemarks.Text = drContraMaster["Remarks"].ToString();
                txtContraID.Text = drContraMaster["ContraID"].ToString();
                dsContra.Tables["tblContraMaster"].Rows.Add(cboSeriesName.Text, drContraMaster["Voucher_No"].ToString(), Date.DBToSystem(drContraMaster["Contra_Date"].ToString()), drContraMaster["Remarks"].ToString());

                DataTable dtContraDetail = m_Contra.GetContraDetail(Convert.ToInt32(txtContraID.Text));
                for (int i = 1; i <= dtContraDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtContraDetail.Rows[i - 1];
                    grdContra[i, 1].Value = i.ToString();
                    grdContra[i, 2].Value = drDetail["LedgerName"].ToString();
                    grdContra[i, 3].Value = drDetail["DrCr"].ToString();
                    grdContra[i, 4].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdContra[i, 5].Value = drDetail["Remarks"].ToString();
                    dsContra.Tables["tblContraDetails"].Rows.Add(drDetail["LedgerName"].ToString(), drDetail["DrCr"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                    AddRowContra(grdContra.RowsCount);
                }
                //Calculate the Debit and Credit totals
                CalculateDrCr();

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtContraID.Text), "CNTR");
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
                }
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
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Contra?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }
           
            Navigation(Navigate.First);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Contra?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }
           
            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Contra?") == DialogResult.Yes)
                    btnSave_Click(sender, e);

            }
           
            Navigation(Navigate.Next);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            if (m_mode == EntryMode.NEW)
            {
                if (Global.MsgQuest("Do you want to save changes to Contra?") == DialogResult.Yes)
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
                    txtVchNo.Enabled = true;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));                   
                    //if (NumberingType == "")
                    //{
                    //    txtVchNo.Text = "Main";
                    //    txtVchNo.Enabled = false;
                    //}

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

        private void btnAddClass_Click(object sender, EventArgs e)
        {
            if (txtContraID.Text == "")
            {
                frmAddAccClass frm = new frmAddAccClass(this);
                frm.Show();
            }
            else
            {
                frmAddAccClass frm = new frmAddAccClass(this, Convert.ToInt32(txtContraID.Text), "CNTR");
                frm.Show();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_CREATE_MODIFY");
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_CREATE_MODIFY");
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
            ContraIDCopy = Convert.ToInt32(txtContraID.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (ContraIDCopy > 0)
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
            dsContra.Clear();
            rptContra rpt = new rptContra();
            rpt.SetDataSource(dsContra);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
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

            pdvCompany_Address.Value = m_CompanyDetails.Address1;
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

            Navigation(Navigate.ID);

            frmReportViewer frm = new frmReportViewer();
            frm.SetReportSource(rpt);
            frm.Show();
            frm.WindowState = FormWindowState.Maximized;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_DELETE");
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
                if (Global.MsgQuest("Are you sure you want to delete the contra - " + txtContraID.Text + "?") == DialogResult.Yes)
                {                    
                    Contra DelContra = new Contra();
                    if (DelContra.Delete(Convert.ToInt32(txtContraID.Text)))
                    {
                        Global.Msg("Contra -" + txtContraID.Text + " deleted successfully!");
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
                        Global.MsgError("There was an error while deleting contra -" + txtContraID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }


        }

        private void frmContra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void ChequeDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = true;
            ctx1 = (SourceGrid.CellContext)sender;
            if (ctx1.Value.ToString() != "")
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

        //public void DateConvert(DateTime DotNetDate)
        //{
        //    if (!IsChequeDateButton)
        //        txtDate.Text = Date.ToSystem(DotNetDate);
        //    if (IsChequeDateButton)
        //        ctx1.Value = Date.ToSystem(DotNetDate);
        //}
    }
}
