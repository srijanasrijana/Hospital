using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using BusinessLogic;
using DateManager;
using Common;


namespace Accounts

{
    public interface IfrmAccountLedger
    {
        void AddAccountLedger(string LedgerName);
    }

    public partial class frmAccountSetup : Form, ILOVCurrency, IfrmOpeningBalance, IfrmPreviousYearBalance
    {
        private bool IsUsingInterface = false;
        private Boolean IsFieldChanged = false;
        private int LedgerID = 0;
        ListItem AccountGroupID = new ListItem();
        private enum DebitCredit { Debit, Credit };
        private SearchIn m_SearchIn = SearchIn.Account_Groups; //Default SearchIN Holder
        private string LedgerName;
        private EntryMode m_mode = EntryMode.NEW; //Stores the current mode or state of which button is clicked
        private IfrmAccountLedger m_ParentForm = null;
        private int _ledgeriD;
        public DataTable FromOpeningBalance = new DataTable();
        public DataTable FromPreYearBalance = new DataTable();

        //Store the expansion state of treeview
        private bool IsExpand = false;

        //To store the previous selection on ledger -> parent account group
        public object m_LdrPrevGroupSelection;
        private IMDIMainForm m_MDIForm;
        //for the purpose of audit log

        private string OldAcGroup = " ";
        private string NewAcGroup = " ";
        private bool isNew;
        private string OldLedgerDesc = " ";
        private string NewLedgerDesc = " ";

        private bool IsChange = false;
        private DateTime? varDueDate = null;

        public frmAccountSetup()
        {
            InitializeComponent();
        }

        public frmAccountSetup(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;

        }

        public frmAccountSetup(Form parentForm)
        {
            InitializeComponent();
            IsUsingInterface = true;
            m_ParentForm = (IfrmAccountLedger)parentForm;
        }

        public frmAccountSetup(Form ParentForm, string ledgerName)
        {
            try
            {
                InitializeComponent();
                ChangeState(EntryMode.NEW);
                m_ParentForm = (IfrmAccountLedger)ParentForm;
                this.LedgerName = ledgerName;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmAccountSetup_Load(object sender, EventArgs e)
        {
            frmProgress ProgressForm = new frmProgress();

            try
            {
                // Initialize the thread that will handle the background process
                System.Threading.Thread backgroundThread = new System.Threading.Thread(
                    new System.Threading.ThreadStart(() =>
                    {

                        ProgressForm.ShowDialog();
                    }
                ));

                backgroundThread.Start();
                ProgressForm.UpdateProgress(20, "Initializing...");


                //Fill the account head name in the treeview
                ShowAccountHeadInTreeView(tvAccount);//tvAccount, null, 0);
                ProgressForm.UpdateProgress(40, "Initializing...");

                ListAccGroup(cboParentGrp);
                ListAccGroup(cboLdrAcGroup); //Ledger tab's Account group combo filling
                ManageLanguage();

                //Fill the date fields and change their mask
                txtLdrOpDate.Mask = Date.FormatToMask();
                switch (LangMgr.DefaultLanguage)
                {

                    case Lang.English:
                        txtLdrOpDate.PromptChar = '_';
                        break;
                    case Lang.Nepali:
                        txtLdrOpDate.PromptChar = '=';
                        break;
                }
                cboOpBalDrCr.SelectedIndex = 0;
                cboPrevYrBal.SelectedIndex = 0;

                FillCurrency(cboLdrOpCCY);
                ProgressForm.UpdateProgress(60, "Initializing...");

                AccountGroup.Search(SearchIn.Account_Groups, SearchOp.Begins_With, "", SearchOp.Begins_With, "", LangMgr.DefaultLanguage,false );//Search a blank text with begins with so that all the available data is listed


                //Set the default search selection
                cboSrchSearchIn1.Items.Clear();
                cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Account_Groups, LangMgr.Translate("ACCOUNT_GROUP", "Account Groups")));
                cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Ledgers, LangMgr.Translate("ACCOUNT_LEDGER", "Ledgers")));
                cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Accounts_Under, LangMgr.Translate("ACCOUNT_UNDER", "Accout Under")));
                cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Ledgers_Under, LangMgr.Translate("LEDGER_UNDER", "Ledger Under")));


                cboSrchSearchIn1.SelectedIndex = 0;
                cboSrchOP1.SelectedIndex = 0;

                cboOpBalDrCr.Items.Clear();
                cboOpBalDrCr.Items.Add(new ListItem((int)DebitCredit.Debit, LangMgr.Translate("DEBIT")));
                cboOpBalDrCr.Items.Add(new ListItem((int)DebitCredit.Credit, LangMgr.Translate("CREDIT")));
                cboOpBalDrCr.SelectedIndex = 0;

                cboPrevYrBal.Items.Clear();
                cboPrevYrBal.Items.Add(new ListItem((int)DebitCredit.Debit, LangMgr.Translate("DEBIT")));
                cboPrevYrBal.Items.Add(new ListItem((int)DebitCredit.Credit, LangMgr.Translate("CREDIT")));
                cboPrevYrBal.SelectedIndex = 0;


                //Just make an empty search so that the gridview loads the default data
                Search(SearchIn.Account_Groups, SearchOp.Begins_With, "", SearchOp.Equals, "");


                ProgressForm.UpdateProgress(80, "Getting Ready...");

                ChangeState(EntryMode.NORMAL);

                //Assign onchange event to all controls.
                #region Assign Onchange Event to all controls
                txtGroupCode.TextChanged += new EventHandler(Text_Change);
                txtGroupName.TextChanged += new EventHandler(Text_Change);
                cboParentGrp.SelectedIndexChanged += new EventHandler(Text_Change);
                txtDescription.TextChanged += new EventHandler(Text_Change);

                txtLedgerCode.TextChanged += new EventHandler(Text_Change);
                txtLedgerName.TextChanged += new EventHandler(Text_Change);
                cboLdrAcGroup.SelectedIndexChanged += new EventHandler(Text_Change);
                txtLdrRemarks.TextChanged += new EventHandler(Text_Change);
                cboLdrOpCCY.SelectedIndexChanged += new EventHandler(Text_Change);
                btnLdrCCYLookup.Click += new EventHandler(Text_Change);
                txtLdrOpCCYRate.TextChanged += new EventHandler(Text_Change);
                txtLdrOpDate.TextChanged += new EventHandler(Text_Change);
                btnOpeningBalance.Click += new EventHandler(Text_Change);
                txtLdrOpBalance.TextChanged += new EventHandler(Text_Change);
                // txtLdrPrevYrBal.TextChanged += new EventHandler(Text_Change);
                btnPreviousYearBalance.Click += new EventHandler(Text_Change);
                cboPrevYrBal.SelectedIndexChanged += new EventHandler(Text_Change);

                txtLdrPerName.TextChanged += new EventHandler(Text_Change);
                txtLdrAddress1.TextChanged += new EventHandler(Text_Change);
                txtLdrAddress2.TextChanged += new EventHandler(Text_Change);
                txtLdrCity.TextChanged += new EventHandler(Text_Change);
                txtLdrTelephone.TextChanged += new EventHandler(Text_Change);
                txtLdrEmail.TextChanged += new EventHandler(Text_Change);
                txtLdrCompany.TextChanged += new EventHandler(Text_Change);
                txtLdrWebsite.TextChanged += new EventHandler(Text_Change);
                txtVatPanNo.TextChanged += new EventHandler(Text_Change);
                txtCreditLimit.TextChanged += new EventHandler(Text_Change);

                #endregion


                //If it were called via interface from another form
                if (this.m_ParentForm != null)
                {
                    //Do all stuffs that new button would do

                    //Select Ledger Tab
                    tabAccountSetup.SelectedIndex = 1;
                    btnLdrNew_Click(sender, e);
                    //Only put the ledger name to the textbox
                    txtLedgerName.Text = this.LedgerName;
                    //focus on the ledgername
                    txtLedgerName.Focus();
                }


            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            ProgressForm.UpdateProgress(100, "Preparing report for display...");
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
        }//End function

        //Validate Form inputs under Group Tab
        private bool ValidateGroup()
        {
            bool bValidate = false;

            FormHandle m_FHandle = new FormHandle();
            //m_FHandle.AddValidate(txtGroupName, DType.NAME, "Invalid account group name. Please choose a valid group name");

            bValidate = m_FHandle.Validate();
            if (txtGroupName.Text.Trim() == "")
            {
                bValidate = false;
            }
            else
            {
                bValidate = true;
            }
            if (cboParentGrp.SelectedItem == null)
            {
                Global.MsgError("Invalid Parent Group Name Selected");
                bValidate = false;
            }

            return bValidate;
        }

        //Validate Ledger fields
        private bool ValidateLedger()
        {
            //FormHandle m_FHandle = new FormHandle();
            //m_FHandle.AddValidate(txtLedgerName, DType., "Invalid ledger name. Please choose a valid ledger name and try again.");
            ////m_FHandle.AddValidate(txtLdrOpBalance, DType.FLOAT, "Invalid opening balance. The opening balance can only be a number.");
            ////m_FHandle.AddValidate(txtLdrOpCCYRate, DType.FLOAT, "Invalid opening balance rate. It can only accept numbers.");
            ////m_FHandle.AddValidate(txtLdrOpDate, DType.DATE, "Invalid opening balance date. Please enter a valid date and try again.");
            //return m_FHandle.Validate();

            if (txtLedgerName.Text.Trim().Length == 0)
            {
                Global.MsgError("Invalid ledger name.");
                txtLedgerName.Focus();
                return false;
            }
            return true;
        }

        //Language switchability
        public void ManageLanguage()
        {
            //Set the font of whole form 
            this.Font = LangMgr.GetFont();
            LangMgr langMgr = new LangMgr();
            langMgr.AddTranslation("GROUP_NAME", lblGroupName);
            langMgr.AddTranslation("PARENT_GROUP", lblParentGroup);
            langMgr.AddTranslation("DESCRIPTION", lblDescription);
            langMgr.AddTranslation("TREEVIEW", tabDisplay.TabPages["tbTree"]);
            langMgr.AddTranslation("LISTVIEW", tabDisplay.TabPages["tbList"]);
            langMgr.AddTranslation("ACCOUNT_GROUP", tabAccountSetup.TabPages["tabAccount"]);
            langMgr.AddTranslation("ACCOUNT_LEDGER", tabAccountSetup.TabPages["tabLedger"]);
            langMgr.AddTranslation("NEW", btnGrpNew);
            langMgr.AddTranslation("EDIT", btnGrpEdit);
            langMgr.AddTranslation("SAVE", btnGrpSave);
            langMgr.AddTranslation("DELETE", btnGrpDelete);
            langMgr.AddTranslation("CANCEL", btnGrpCancel);
            langMgr.AddTranslation("NEW", btnLdrNew);
            langMgr.AddTranslation("EDIT", btnLdrEdit);
            langMgr.AddTranslation("SAVE", btnLdrSave);
            langMgr.AddTranslation("DELETE", btnLdrDelete);
            langMgr.AddTranslation("CANCEL", btnLdrCancel);
            langMgr.BulkTranslate();


                 
        }

        //Recursive Function to Show Account Group in Treeview
        public void ShowAccountHeadInTreeView(TreeView tv)//TreeView tv, TreeNode n, int Group_ID)
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

            #region oldcode
            //DataTable dt;
            //dt = AccountGroup.GetGroupTable(Group_ID);


            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dt.Rows[i];
            //    TreeNode t = new TreeNode(dr[LangField].ToString(), 0, 0);

            //    ShowAccountHeadInTreeView(tv, t, Convert.ToInt16(dr["GroupID"].ToString()));

            //    if (n == null)

            //        tv.Nodes.Add(t); //Primary Group

            //    else
            //    {
            //        n.Nodes.Add(t); //Secondary Group


            //    }
            //    //Also add Ledgers
            //    DataTable dtLedger = Ledger.GetLedgerTable(Convert.ToInt32(dr["GroupID"]));
            //    for (int j = 0; j < dtLedger.Rows.Count; j++)
            //    {
            //        DataRow drLedger = dtLedger.Rows[j];
            //        TreeNode tnLedger = new TreeNode(drLedger[LangField].ToString());
            //        tnLedger.ForeColor = Color.DarkBlue;
            //        t.Nodes.Add(tnLedger);
            //    }

            //}
            #endregion
            DataSet ds = AccountGroup.GetDataSetForAccountTreeView();
            foreach(DataRow drParentRow in ds.Tables[0].Rows)
            {
                if (String.IsNullOrEmpty(drParentRow["ParentID"].ToString()))
                {
                    TreeNode tnParentnode = new TreeNode();
                    tnParentnode.Text = drParentRow["GroupName"].ToString();
                    tnParentnode.Tag = Convert.ToInt32(drParentRow["GroupID"]);

                    SetTreeviewChilds(drParentRow, tnParentnode);
                    foreach (DataRow childLedger in drParentRow.GetChildRows("ChildLedger"))
                    {
                        TreeNode childLedgerNode = new TreeNode();
                        childLedgerNode.Text = childLedger["LedgerName"].ToString();
                        childLedgerNode.Tag = Convert.ToInt32(childLedger["LedgerID"]);
                        childLedgerNode.ForeColor = System.Drawing.Color.DarkBlue;
                        tnParentnode.Nodes.Add(childLedgerNode);
                    }
                    tv.Nodes.Add(tnParentnode);
                }
            }

        }

        public void SetTreeviewChilds(DataRow dtRow,TreeNode tn)
        {
            DataRow[] ChildRows = dtRow.GetChildRows("ChildGroup");
            foreach(DataRow childrow in ChildRows)
            {
                TreeNode childnode = new TreeNode();
                childnode.Text = childrow["GroupName"].ToString();
                childnode.Tag = Convert.ToInt32(childrow["GroupID"]);
                if(childrow.GetChildRows("ChildGroup").Length>0)
                {
                    SetTreeviewChilds(childrow, childnode);
                }
                foreach( DataRow childLedger in childrow.GetChildRows("ChildLedger"))
                {
                    TreeNode childLedgerNode = new TreeNode();
                    childLedgerNode.Text = childLedger["LedgerName"].ToString();
                    childLedgerNode.Tag = Convert.ToInt32(childLedger["LedgerID"]);
                    childLedgerNode.ForeColor = System.Drawing.Color.DarkBlue;
                    childnode.Nodes.Add(childLedgerNode);
                }
                tn.Nodes.Add(childnode);
            }

        }

        //Fill the cboUnder List box with Account Head
        private void ListAccGroup(ComboBox ComboBoxControl)
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
            DataTable dt = AccountGroup.GetGroupTable(-1);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                //Check if it is Ledger's tab's parent accounting group
                if (ComboBoxControl == cboLdrAcGroup)
                {
                    //Chop off the Assets, Liabilities, Income and Expenditure
                    try
                    {
                        if (Convert.ToInt32(dr["GroupNumber"]) > 0 && Convert.ToInt32(dr["GroupNumber"]) <= 4)
                            continue;
                    }
                    catch { }
                }


                ComboBoxControl.Items.Add(new ListItem((int)dr["GroupID"], dr[LangField].ToString()));

            }

            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearAccountHeadForm();
            txtGroupID.Clear();
            m_mode = EntryMode.NEW;
            EnableControls(true);
            ButtonState(false, false, true, false, true);
            IsFieldChanged = false;
            int numberingType = 0;
            DataTable config = new DataTable();
            DataTable format = new DataTable();

            config = Ledger.GetGroupConfig();
            foreach (DataRow getconfig in config.Rows)
            {
                numberingType = Convert.ToInt32(getconfig["NumberingType"]);

            }
            if (numberingType == 1)
            {
                format = Ledger.GetGrpFormatParameter();
                string str = "";
                foreach (DataRow dr in format.Rows)
                {
                    switch (dr["Type"].ToString())
                    {
                        case "Symbol":
                            str = str + dr["Parameter"].ToString();
                            break;
                        case "(AutoNumber)":
                            {
                                if (Convert.ToInt32(dr["Parameter"]) < 10)
                                    str = str + "00000" + dr["parameter"].ToString();
                                else if (Convert.ToInt32(dr["Parameter"]) >= 10 && (Convert.ToInt32(dr["Parameter"]) < 100))
                                    str = str + "0000" + dr["parameter"].ToString();
                                else if (Convert.ToInt32(dr["Parameter"]) >= 100 && (Convert.ToInt32(dr["Parameter"]) < 1000))
                                    str = str + "000" + dr["parameter"].ToString();
                                else if (Convert.ToInt32(dr["Parameter"]) >= 1000 && (Convert.ToInt32(dr["Parameter"]) < 10000))
                                    str = str + "00" + dr["parameter"].ToString();
                                else if (Convert.ToInt32(dr["Parameter"]) >= 10000 && (Convert.ToInt32(dr["Parameter"]) < 100000))
                                    str = str + "0" + dr["parameter"].ToString();
                                else
                                    str = str + dr["parameter"].ToString();
                                break;
                            }
                        case "Date":
                            {

                                if (dr["Parameter"].ToString() == "NEPALI_FISCAL_YEAR")
                                    str = str + Global.Fiscal_Nepali_Year;

                                else if (dr["Parameter"].ToString() == "ENGLISH_FISCAL_YEAR")
                                    str = str + Global.Fiscal_English_Year;
                                break;
                            }
                    }

                    //str = str + dr["parameter"].ToString();
                    txtGroupCode.Text = str;
                    this.Refresh();
                }


                txtGroupCode.Enabled = false;
                
                IsFieldChanged = false;
                
            }

        }

        private void tvAccount_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //If it is currently in edit or new mode, data has to be saved before going elsewhere
            if (!ContinueWithoutSaving())
            {
                return;
            }

            ClearAccountHeadForm();

            try
            {
                //Show the selected value in respective fields
                if (e.Node.ForeColor == Color.DarkBlue)//Ledger is being Selected
                {

                    tabAccountSetup.SelectedIndex = 1;
                    txtLedgerName.Text = e.Node.Text;

                    int ID = Ledger.GetLedgerIdFromName(e.Node.Text, LangMgr.DefaultLanguage);

                    if (String.IsNullOrEmpty(ID.ToString())) //if no data is found
                    {
                        Global.Msg("ERROR: Please select the ledger properly and then try again");
                        return;
                    }

                    //Show Credit Limit if the account falls under Debtor and Creditor
                    //Check the Ledger ID if it falls on Debtor or Creditor
                    bool isDebtor = AccountGroup.IsLedgerUnderGroup(Convert.ToInt32(ID), AccountGroup.GetIDFromType(AccountType.Debtor));
                    bool isCreditor = AccountGroup.IsLedgerUnderGroup(Convert.ToInt32(ID), AccountGroup.GetIDFromType(AccountType.Creditor));


                    lblCreditLimit.Visible = isDebtor || isCreditor;
                    txtCreditLimit.Visible = isDebtor || isCreditor;



                    //Fill the ledger form 
                    FillLedgerForm(ID);

                    tvAccount.Focus();//Let the focus remain to treeview
                    ChangeState(EntryMode.NORMAL);

                }
                else//Account group is being selected
                {

                    tabAccountSetup.SelectedIndex = 0;

                    object m_GroupID = AccountGroup.GetIDFromName(e.Node.Text, LangMgr.DefaultLanguage);

                    //Fill the textboxes and other fields
                    FillAcGroupForm(Convert.ToInt32(m_GroupID));

                    tvAccount.Focus();//Let the focus remain to treeview
                    ChangeState(EntryMode.NORMAL);


                }


            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                //throw;

            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isNew = false;
            OldAcGroup = " ";
            NewAcGroup = " ";
            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }

            //Not Editable untill a node is selected in the tvAccount treeView
            if (string.IsNullOrEmpty(txtGroupID.Text))
            {
                Global.MsgError("Please select a Group in the Tree View first.");
                return;
            }

            OldAcGroup = "GroupCode" + "-" + OldAcGroup + txtGroupCode.Text + "," + "GroupName" + "-" + txtGroupName.Text + "," + "ParentName" + "-" + cboParentGrp.Text+",";
            string editACHead = txtGroupName.Text.Trim();
            if (AccountGroup.CheckBuiltIn(editACHead, LangMgr.DefaultLanguage) == 1)
            {
                Global.Msg("Account Group - " + editACHead + " is a System group.Sorry! cannot be edited.");
                ChangeState(EntryMode.NORMAL);
                tvAccount.Nodes.Clear();

                ShowAccountHeadInTreeView(tvAccount);//tvAccount, null, 0);

                ClearAccountHeadForm();
                IsFieldChanged = false;
            }
            else
            {
                ChangeState(EntryMode.EDIT);
            }
        }

        //Clears the text of every field of Account Head form
        private void ClearAccountHeadForm()
        {
            txtGroupCode.Clear();
            txtGroupName.Clear();
            cboParentGrp.Text = "";
            txtDescription.Clear();
        }

        //Clears all the fields of Ledger form
        private void ClearLedgerForm()
        {
            // txtBudgetHeadNo.Clear();

            txtLedgerCode.Clear();
            txtLedgerName.Clear();
            cboLdrAcGroup.Text = "";
            txtLdrRemarks.Clear();
            txtLdrOpBalance.Clear();
            txtLdrOpDate.Clear();
            cboOpBalDrCr.SelectedIndex = 0;
            cboLdrOpCCY.Text = Currency.GetCodeFromID(Currency.Default_Curr_ID);
            txtLdrOpCCYRate.Text = "0.00";
            txtLdrOpBalance.Text = "0.00";
            txtLdrOpDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtLdrRemarks.Clear();
            txtLdrPerName.Clear();
            txtLdrAddress1.Clear();
            txtLdrAddress2.Clear();
            txtLdrCity.Clear();
            txtLdrCompany.Clear();
            txtLdrEmail.Clear();
            //txtLdrLedgerID.Clear();
            txtLdrWebsite.Clear();
            txtVatPanNo.Clear();
            txtCreditLimit.Clear();
            cboLdrAcGroup.Text = "";
            chkReference.Checked = chkReference.Visible = false;
            cboLdrAcGroup.SelectedIndex = 0;
            if (m_mode == EntryMode.NEW)
            {
                txtLedgerName.Text = LedgerName;
            }

        }

        //Enables and disables the button states
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnGrpNew.Enabled = btnLdrNew.Enabled = New;
            btnGrpEdit.Enabled = btnLdrEdit.Enabled = Edit;
            btnGrpSave.Enabled = btnLdrSave.Enabled = Save;
            btnGrpDelete.Enabled = btnLdrDelete.Enabled = Delete;
            btnGrpCancel.Enabled = btnLdrCancel.Enabled = Cancel;
        }

        //Account Group Save button pressed
        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (chkUserPermission == false)
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }

            //Check Validation
            if (!ValidateGroup())
                return;

            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    if (tabAccountSetup.SelectedIndex == 0) //Account Head
                    {
                        isNew = true;
                        OldAcGroup = " ";
                        NewAcGroup = " ";
                        NewAcGroup = "GroupCode" + "-" + NewAcGroup + txtGroupCode.Text + "," + "GroupName" + "-" + txtGroupName.Text + "," + "ParentName" + "-" + cboParentGrp.Text+",";
                        try
                        {
                            IAccountGroup acHead = new AccountGroup();
                            acHead.Create(txtGroupCode.Text.Trim(), txtGroupName.Text.Trim(), cboParentGrp.Text.Trim(), txtDescription.Text.Trim(),OldAcGroup,NewAcGroup,isNew);

                            //Increment the Group code (automated number)
                            Global.m_db.InsertUpdateQry("update acc.tblGroupCodeFormat set Parameter=Parameter+1 where TYPE='(AutoNumber)' ");


                            Global.Msg("Account head created successfully!");
                            ChangeState(EntryMode.NORMAL);


                        }
                        catch (SqlException ex)
                        {

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
                        }
                        catch (Exception ex)
                        {
                            Global.MsgError(ex.Message);
                        }
                    }

                    else if (tabAccountSetup.SelectedIndex == 2) //Ledger Selected
                    {
                        //Is Data Freezed?
                        //Get Datafreeze variable from Database
                        //if freeze then show error message and return

                        try
                        {
                            IAccountGroup acHead = new AccountGroup();
                            acHead.Create(txtGroupCode.Text.Trim(), txtGroupName.Text.Trim(), cboParentGrp.Text.Trim(), txtDescription.Text.Trim(),OldAcGroup,NewAcGroup,isNew);

                            Global.Msg("Account head created successfully!");

                            ChangeState(EntryMode.NORMAL);

                            ListAccGroup(cboParentGrp);

                        }
                        catch (Exception ex)
                        {
                            Global.MsgError(ex.Message);
                        }
                    }

                    break;

                #endregion

                #region EDIT
                case EntryMode.EDIT:
                        isNew = false;
                        NewAcGroup = " ";
                        NewAcGroup = "GroupCode" + "-" + NewAcGroup + txtGroupCode.Text + "," + "GroupName" + "-" + txtGroupName.Text + "," + "ParentName" + "-" + cboParentGrp.Text + ",";
                    //Check whether the modified parent group is not under our own group
                    ArrayList ReturnIDs = new ArrayList();
                    AccountGroup.GetAccountsUnder(Convert.ToInt32(txtGroupID.Text.Trim()), ReturnIDs);
                    ListItem liGroupID = new ListItem();


                    liGroupID = (ListItem)cboParentGrp.SelectedItem;

                    if (ReturnIDs.BinarySearch(liGroupID.ID) >= 0) //if found returns greater index than -1
                    {
                        Global.Msg("The parent group name selected is the child of its own group");
                        return;
                    }

                    if (Convert.ToInt32(txtGroupID.Text.Trim()) == liGroupID.ID)
                    {
                        Global.Msg("The parent group name selected can not be itself.");
                        return;
                    }

                    try
                    {

                        IAccountGroup acHead = new AccountGroup();
                        acHead.Modify(txtGroupCode.Text.Trim(), Convert.ToInt32(txtGroupID.Text.Trim()), txtGroupName.Text.Trim(), cboParentGrp.Text.Trim(), txtDescription.Text.Trim(),OldAcGroup,NewAcGroup,isNew);

                        Global.Msg("Account Group modified successfully!");
                        ChangeState(EntryMode.NORMAL);

                        ListAccGroup(cboParentGrp);
                    }
                    catch (SqlException ex)
                    {

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
                                Global.Msg("The group name already exists! Please choose another group names!", MBType.Warning, "Error");
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
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

                //}//end switch(m_mode)
            }
            UpdateAccountTree();
            ListAccGroup(cboParentGrp);
            ListAccGroup(cboLdrAcGroup);


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAccountHeadForm();
            if(isNew == false)
            {
                //Fill the textboxes and other fields
                FillAcGroupForm(Convert.ToInt32(txtGroupID.Text));
            }
            ChangeState(EntryMode.NORMAL);
        }

        private void EnableControls(bool Enable)
        {
            txtGroupCode.Enabled = txtGroupName.Enabled = cboParentGrp.Enabled = txtDescription.Enabled = chkReference.Enabled = Enable;
            txtLedgerCode.Enabled = txtLedgerName.Enabled = cboLdrAcGroup.Enabled = txtLdrRemarks.Enabled = txtLdrPerName.Enabled = txtLdrAddress1.Enabled = txtLdrAddress2.Enabled = txtLdrCity.Enabled = txtLdrCompany.Enabled = txtLdrEmail.Enabled = txtLdrTelephone.Enabled = txtLdrWebsite.Enabled = btnOpeningBalance.Enabled = btnPreviousYearBalance.Enabled = txtVatPanNo.Enabled = txtCreditLimit.Enabled = Enable;
            btnLdrCCYLookup.Enabled = txtLdrOpBalance.Enabled = cboLdrOpCCY.Enabled = cboOpBalDrCr.Enabled = txtLdrOpCCYRate.Enabled = txtLdrOpDate.Enabled = true;
        }

        private void ChangeState(EntryMode Mode)
        {
            //On every changestate, clear the Opening Balance table
            FromOpeningBalance.Rows.Clear();
            FromPreYearBalance.Rows.Clear();

            m_mode = Mode;

            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false);
                    IsFieldChanged = false;
                    chkAutoCalculate.Enabled = false;
                    RateTextBox.Enabled = false;
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    chkAutoCalculate.Enabled = true;
                    RateTextBox.Enabled = true;
                    RateTextBox.Text = "";
                    chkAutoCalculate.Checked = false;
                    int numberingType = 0;
                    DataTable config = new DataTable();
                    DataTable format = new DataTable();

                    config = Ledger.GetLedgerConfig();
                    foreach (DataRow getconfig in config.Rows)
                    {
                        numberingType = Convert.ToInt32(getconfig["NumberingType"]);

                    }
                    if (numberingType == 1)
                    {
                        format = Ledger.GetFormatParameter();
                        string str = "";
                        foreach (DataRow dr in format.Rows)
                        {
                            switch (dr["Type"].ToString())
                            {
                                case "Symbol":
                                    str = str + dr["Parameter"].ToString();
                                    break;
                                case "(AutoNumber)":
                                    {
                                        if (Convert.ToInt32(dr["Parameter"]) < 10)
                                            str = str + "00000" + dr["parameter"].ToString();
                                        else if (Convert.ToInt32(dr["Parameter"]) >= 10 && (Convert.ToInt32(dr["Parameter"]) < 100))
                                            str = str + "0000" + dr["parameter"].ToString();
                                        else if (Convert.ToInt32(dr["Parameter"]) >= 100 && (Convert.ToInt32(dr["Parameter"]) < 1000))
                                            str = str + "000" + dr["parameter"].ToString();
                                        else if (Convert.ToInt32(dr["Parameter"]) >= 1000 && (Convert.ToInt32(dr["Parameter"]) < 10000))
                                            str = str + "00" + dr["parameter"].ToString();
                                        else if (Convert.ToInt32(dr["Parameter"]) >= 10000 && (Convert.ToInt32(dr["Parameter"]) < 100000))
                                            str = str + "0" + dr["parameter"].ToString();
                                        else
                                            str = str + dr["parameter"].ToString();
                                        break;

                                    }
                                case "Date":
                                    {

                                        if (dr["Parameter"].ToString() == "NEPALI_FISCAL_YEAR")
                                            str = str + Global.Fiscal_Nepali_Year;

                                        else if (dr["Parameter"].ToString() == "ENGLISH_FISCAL_YEAR")
                                            str = str + Global.Fiscal_English_Year;
                                        break;

                                    }




                            }

                            //str = str + dr["parameter"].ToString();
                            txtLedgerCode.Text = str;
                            this.Refresh();
                        }


                        txtLedgerCode.Enabled = false;
                    }
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    chkAutoCalculate.Enabled = true;
                    RateTextBox.Enabled = true;
                    break;



            }
        }

        private void UpdateAccountTree()
        {
            tvAccount.Nodes.Clear();
            ShowAccountHeadInTreeView(tvAccount);//tvAccount, null, 0);

            //Expand/Collapse button state fix
            IsExpand = false;
            btnToggleExpand.Text = "Expand";
        }

        /// <summary>
        /// Fills the available currency in the combo box
        /// </summary>
        /// <param name="ComboBoxControl"></param>
        private void FillCurrency(ComboBox ComboBoxControl)
        {

            ComboBoxControl.Items.Clear();
            DataTable dt = Currency.GetCurrencyTable();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["CCYID"], dr["Code"].ToString()));

            }

            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to delete. Please contact your administrator for permission.");
                return;
            }

            //Not Delete able untill a node is selected in the tvAccount treeView
            if (string.IsNullOrEmpty(txtGroupID.Text))
            {
                Global.MsgError("Please select a Group in the Tree View first.");
                return;
            }

            //Check if a group has any child group or ledger.
            //If yes than do not allow to delete
            int groupId = Convert.ToInt32(txtGroupID.Text.Trim());
            DataTable dtl = Ledger.GetLedgerTable(groupId);
            DataTable dt = AccountGroup.GetGroupTable(groupId);
            if(dt.Rows.Count>0 || dtl.Rows.Count>0)
            {
                Global.MsgError("Sorry! you can not delete a Account Group that has child Account Group or Ledger. First delete the child Account Group or the Ledger.");
                return;
            }

            //Ask if the account head is to be deleted
            if (Global.MsgQuest("Are you sure you want to delete the Account head - " + txtGroupName.Text.Trim() + "?") == DialogResult.No)
                return;

            //If the user confirms deletion
            try
            {
                IAccountGroup acHead = new AccountGroup();
                string delACHead = txtGroupName.Text.Trim();
                if (AccountGroup.CheckBuiltIn(delACHead, LangMgr.DefaultLanguage) == 0)
                {
                    acHead.Delete(delACHead);
                    Global.Msg("Account Group - " + delACHead + " deleted successfully!");
                }
                else
                    Global.Msg("Account Group - " + delACHead + " is a System group.Sorry! cannot be deleted.");

                ChangeState(EntryMode.NORMAL);

                tvAccount.Nodes.Clear();

                ShowAccountHeadInTreeView(tvAccount);//tvAccount, null, 0);

                ClearAccountHeadForm();
                txtGroupID.Clear();
                

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            IsFieldChanged = false;
        }

        private void collapsibleSplitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Invalidate(true); //Refreshes the UI of the form so that everything is perfectly resized
        }

        private void collapsibleSplitter1_MouseClick(object sender, MouseEventArgs e)
        {
            Invalidate(true); //Refreshes the UI of the form so that everything is perfectly resized
        }

        private void btnLdrSave_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }


            //Check Validation
            if (!ValidateLedger())
                return;
            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    if (tabAccountSetup.SelectedIndex == 1) //Ledger Selected
                    {
                        try
                        {
                            isNew = true;
                            NewLedgerDesc = " ";
                            OldLedgerDesc = " ";
                            NewLedgerDesc = NewLedgerDesc + "LedgerCode" + "-" + txtLedgerCode.Text + "," + "LedgerName" + "-" + txtLedgerName.Text + "," + "ACHead" + "-" + cboLdrAcGroup.Text+",";
                            ILedger acLedger = new Ledger();
                            ListItem lstLdrAcGrp = (ListItem)cboLdrAcGroup.SelectedItem;
                            if (lstLdrAcGrp == null)//check if non existing account group is typed
                                throw new Exception("Invalid account head name selected!");


                            #region Forget currency till it is fully developed
                            ListItem lstOpCCY = (ListItem)cboLdrOpCCY.SelectedItem;
                            if (lstOpCCY == null)//check if non existing currency code is typed
                                throw new Exception("Invalid currency code selected!");


                            Double opBal, CCYRate;
                            if (txtLdrOpBalance.Text.Trim().Length <= 0)
                                opBal = 0;
                            else
                                opBal = Convert.ToDouble(txtLdrOpBalance.Text.Trim());


                            //For previous year balance 


                            if (txtLdrOpCCYRate.Text.Trim().Length <= 0)
                                CCYRate = 0;
                            else
                                CCYRate = Convert.ToDouble(txtLdrOpCCYRate.Text.Trim());

                            DateTime dtOpBalCCR = Date.ToDotNet(txtLdrOpDate.Text);
                            #endregion

                            // Double PrevYrBal = 0;
                            // if (!Double.TryParse(txtLdrPrevYrBal.Text.Trim(), out PrevYrBal))
                            //  PrevYrBal = 0;


                            //dtOpBalCCR = null;
                            //string strOpBalDrCr="DEBIT";
                            //ListItem lstDrCr = (ListItem)cboOpBalDrCr.SelectedItem;
                            //switch ((DebitCredit)lstDrCr.ID)
                            //{
                            //    case DebitCredit.Debit:
                            //        strOpBalDrCr = "DEBIT";
                            //        break;
                            //    case DebitCredit.Credit:
                            //        strOpBalDrCr = "CREDIT";
                            //        break;
                            //}

                            //For Previous year balance
                            string strPrevYrBal = "DEBIT";
                            ListItem lstPreYrBalDrCr = (ListItem)cboPrevYrBal.SelectedItem;
                            switch ((DebitCredit)lstPreYrBalDrCr.ID)
                            {
                                case DebitCredit.Debit:
                                    strPrevYrBal = "DEBIT";
                                    break;
                                case DebitCredit.Credit:
                                    strPrevYrBal = "CREDIT";
                                    break;
                            }
                           
                            //string ledgername = txtLedgerName.Text.Trim();
                            //ledgername = ledgername.Replace("'", "\'");
                           // bool result = acLedger.Create(txtLedgerCode.Text.Trim(), txtLedgerName.Text.Trim(), lstLdrAcGrp.ID, 0.00, strPrevYrBal, lstOpCCY.ID, CCYRate, dtOpBalCCR, txtLdrRemarks.Text.Trim(), txtLdrPerName.Text.Trim(), txtLdrAddress1.Text.Trim(), txtLdrAddress2.Text.Trim(), txtLdrCity.Text.Trim(), txtLdrTelephone.Text.Trim(), txtLdrEmail.Text.Trim(), txtLdrCompany.Text.Trim(), txtLdrWebsite.Text.Trim(), txtVatPanNo.Text.Trim(), txtCreditLimit.Text.Length > 0 ? Convert.ToDouble(txtCreditLimit.Text) : 0, Convert.ToBoolean(chkAutoCalculate.Checked ? 1 : 0), RateTextBox.Text.Length > 0 ? float.Parse(RateTextBox.Text) : 0, OldLedgerDesc, NewLedgerDesc, isNew, chkReference.Checked);
                            bool result = acLedger.Create(txtLedgerCode.Text.Trim(), txtLedgerName.Text.Trim(), lstLdrAcGrp.ID, 0.00, strPrevYrBal,
                            lstOpCCY.ID, CCYRate, dtOpBalCCR, txtLdrRemarks.Text.Trim(), txtLdrPerName.Text.Trim(), txtLdrAddress1.Text.Trim(),
                            txtLdrAddress2.Text.Trim(), txtLdrCity.Text.Trim(), txtLdrTelephone.Text.Trim(), txtLdrEmail.Text.Trim(), txtLdrCompany.Text.Trim(),
                            txtLdrWebsite.Text.Trim(), txtVatPanNo.Text.Trim(), txtCreditLimit.Text.Length > 0 ? Convert.ToDouble(txtCreditLimit.Text) : 0,
                            Convert.ToBoolean(chkAutoCalculate.Checked ? 1 : 0), RateTextBox.Text.Length > 0 ? float.Parse(RateTextBox.Text) : 0, OldLedgerDesc,
                            NewLedgerDesc, isNew, out LedgerID);
                            //Increment the ledger code (automated number)
                            Global.m_db.InsertUpdateQry("update acc.tblLedgerCodeFormat set Parameter=Parameter+1 where TYPE='(AutoNumber)' ");


                            //Get current LedgerID and save it to tblOpeningBalance
                            int LdrID = Ledger.GetLedgerIdFromName(txtLedgerName.Text.Trim(), LangMgr.DefaultLanguage);
                           // int LdrID = Ledger.GetLedgerIdFromName(ledgername, LangMgr.DefaultLanguage);
                            OpeningBalance.InsertAccountOpeningBalance(LdrID, FromOpeningBalance, varDueDate);
                            OpeningBalance.InsertAccountPreYearBalance(LdrID, FromPreYearBalance);
                            chkAutoCalculate.Checked = false;
                            RateTextBox.Text = "";


                            if (result)
                                Global.Msg("Ledger created successfully!");
                            else
                                Global.Msg("There is some problem in ledger creation");


                            ChangeState(EntryMode.NORMAL);

                        }
                        catch (SqlException ex)
                        {
                            #region catch sql exception

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
                    }


                    break;

                #endregion

                #region EDIT
                case EntryMode.EDIT:
                    if (tabAccountSetup.SelectedIndex == 1) //Ledger Selected
                    {

                        try
                        {
                            isNew = false;
                            NewLedgerDesc = " ";
                            NewLedgerDesc = NewLedgerDesc + "LedgerCode" + txtLedgerCode.Text + "LedgerName" + txtLedgerName.Text + "ACHead" + cboLdrAcGroup.Text;
                            ILedger acLedger = new Ledger();
                            ListItem lstLdrAcGrp = (ListItem)cboLdrAcGroup.SelectedItem;
                            if (lstLdrAcGrp == null)//check if non existing account group is typed
                                throw new Exception("Invalid account head name selected!");
                            ListItem lstOpCCY = (ListItem)cboLdrOpCCY.SelectedItem;
                            if (lstOpCCY == null)//check if non existing currency code is typed
                                throw new Exception("Invalid currency code selected!");

                            Double opBal, PreYrBal, CCYRate;
                            if (txtLdrOpBalance.Text.Trim().Length <= 0)
                                opBal = 0;
                            else
                                opBal = Convert.ToDouble(txtLdrOpBalance.Text.Trim());

                            //For previous year balance 
                            //  if (txtLdrPrevYrBal.Text.Trim().Length <= 0)
                            //  PreYrBal = 0;
                            // else
                            //    PreYrBal = Convert.ToDouble(txtLdrPrevYrBal.Text.Trim());

                            if (txtLdrOpCCYRate.Text.Trim().Length <= 0)
                                CCYRate = 0;
                            else
                                CCYRate = Convert.ToDouble(txtLdrOpCCYRate.Text.Trim());

                            DateTime dtOpBalCCR = Date.ToDotNet(txtLdrOpDate.Text);

                            //dtOpBalCCR = null;

                            //string strOpBalDrCr="DEBIT";
                            //ListItem lstDrCr = (ListItem)cboOpBalDrCr.SelectedItem;
                            //switch ((DebitCredit)lstDrCr.ID)
                            //{
                            //    case DebitCredit.Debit:
                            //        strOpBalDrCr = "DEBIT";
                            //        break;
                            //    case DebitCredit.Credit:
                            //        strOpBalDrCr = "CREDIT";
                            //        break;
                            //}

                            //For Previous year balance
                            string strPrevYrBal = "DEBIT";
                            ListItem lstPreyrbalDrCr = (ListItem)cboPrevYrBal.SelectedItem;
                            switch ((DebitCredit)lstPreyrbalDrCr.ID)
                            {
                                case DebitCredit.Debit:
                                    strPrevYrBal = "DEBIT";
                                    break;
                                case DebitCredit.Credit:
                                    strPrevYrBal = "CREDIT";
                                    break;
                            }

                          //  bool result = acLedger.Modify(Convert.ToInt32(txtLdrLedgerID.Text), txtLedgerCode.Text.Trim(), txtLedgerName.Text.Trim(), lstLdrAcGrp.ID, 0.00, strPrevYrBal, lstOpCCY.ID, CCYRate, dtOpBalCCR, txtLdrRemarks.Text.Trim(), txtLdrPerName.Text.Trim(), txtLdrAddress1.Text.Trim(), txtLdrAddress2.Text.Trim(), txtLdrCity.Text.Trim(), txtLdrTelephone.Text.Trim(), txtLdrEmail.Text.Trim(), txtLdrCompany.Text.Trim(), txtLdrWebsite.Text.Trim(), txtVatPanNo.Text.Trim(), Convert.ToDouble(txtCreditLimit.Text), Convert.ToBoolean(chkAutoCalculate.Checked ? 1 : 0), RateTextBox.Text.Length > 0 ? float.Parse(RateTextBox.Text) : 0,OldLedgerDesc,NewLedgerDesc,isNew, chkReference.Checked);
                            bool result = acLedger.Modify(Convert.ToInt32(txtLdrLedgerID.Text), txtLedgerCode.Text.Trim(), txtLedgerName.Text.Trim(),
                               lstLdrAcGrp.ID, 0.00, strPrevYrBal, lstOpCCY.ID, CCYRate, dtOpBalCCR, txtLdrRemarks.Text.Trim(), txtLdrPerName.Text.Trim(),
                               txtLdrAddress1.Text.Trim(), txtLdrAddress2.Text.Trim(), txtLdrCity.Text.Trim(), txtLdrTelephone.Text.Trim(), txtLdrEmail.Text.Trim(),
                               txtLdrCompany.Text.Trim(), txtLdrWebsite.Text.Trim(), txtVatPanNo.Text.Trim(), Convert.ToDouble(txtCreditLimit.Text),
                               Convert.ToBoolean(chkAutoCalculate.Checked ? 1 : 0), RateTextBox.Text.Length > 0 ? float.Parse(RateTextBox.Text) : 0, OldLedgerDesc,
                               NewLedgerDesc, isNew);

                            //Get current LedgerID and save it to tblOpeningBalance
                            //int LdrID = Ledger.GetLedgerIdFromName(txtLedgerName.Text.Trim(), LangMgr.Language);
                            if (FromOpeningBalance.Rows.Count > 0)
                            {
                                OpeningBalance.InsertAccountOpeningBalance(Convert.ToInt32(txtLdrLedgerID.Text), FromOpeningBalance, varDueDate);
                            }

                            if (FromPreYearBalance.Rows.Count > 0)
                            {
                                OpeningBalance.InsertAccountPreYearBalance(Convert.ToInt32(txtLdrLedgerID.Text), FromPreYearBalance);
                            }

                            if (result)
                                Global.Msg("Ledger modified successfully!");
                            else
                                Global.Msg("There is some problem in ledger modification");


                            ChangeState(EntryMode.NORMAL);



                        }
                        catch (Exception ex)
                        {
                            Global.MsgError(ex.Message);
                        }
                    }
                    break;
                #endregion

            }//end switch(m_mode)

            if (IsUsingInterface)
            {
                this.Close();
                m_ParentForm.AddAccountLedger(txtLedgerName.Text);
                IsUsingInterface = false;

            }
            else
            {
                UpdateAccountTree();
                ClearLedgerForm();
                // IfrmAccountLedger. grdJournal.Refresh();

                //It is called by other parent forms

                if (m_ParentForm != null)
                {
                    m_ParentForm.AddAccountLedger(txtLedgerName.Text);
                    // m_ParentForm.cboAccount.StandardValues = Ledger.GetLedgerList(0);
                }


                if (this.m_ParentForm != null)//This must be called from Interface
                {
                    this.Close();

                }
            }
            IsFieldChanged = false;

        }

        private void btnLdrNew_Click(object sender, EventArgs e)
        {
            isNew = true;

            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearLedgerForm();
            txtLdrLedgerID.Clear();
            ChangeState(EntryMode.NEW);
            IsFieldChanged = false;

        }

        private void btnLdrEdit_Click(object sender, EventArgs e)
        {
            OldLedgerDesc = " ";
            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }

            //Not Edit able untill a node is selected in the tvAccount treeView
            if (string.IsNullOrEmpty(txtLdrLedgerID.Text))
            {
                Global.MsgError("Please select a Ledger in the Tree View first.");
                return;
            }

            isNew = false;
            OldLedgerDesc = OldLedgerDesc + "LedgerCode" + "-" + txtLedgerCode.Text + "," + "LedgerName" + "-" + txtLedgerName.Text + "," + "ACHead" + "-" + cboLdrAcGroup.Text+",";
            ChangeState(EntryMode.EDIT);
        }

        private void tabAccountSetup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void btnLdrCancel_Click(object sender, EventArgs e)
        {
            ClearLedgerForm();
            if(!isNew)
            {
                //Fill the ledger form 
                FillLedgerForm(Convert.ToInt32(txtLdrLedgerID.Text));
            }
            ChangeState(EntryMode.NORMAL);
        }

        private void btnLdrCCYLookup_Click(object sender, EventArgs e)
        {

            frmLOVCurrency m_frmLOVCurr = new frmLOVCurrency(this);

            m_frmLOVCurr.ShowDialog();

        }

        //A function from the Interface ILOVCurrency. Used to set the Currency Code to this form from LOV
        public void AddCurrency(string CurrencyCode)
        {
            cboLdrOpCCY.Text = CurrencyCode;
        }

        //A function from the Interface IfrmOpeningBalance. Used to set the OpeningBalance to this form from OpeningBaance
        public void AddOpeningBalance(DataTable AllOpeningBalance, DateTime? DueDate)
        {
            FromOpeningBalance = AllOpeningBalance;
            varDueDate = DueDate;

        }

        public void AddPreYearBalance(DataTable AllPreYearBalance)
        {
            FromPreYearBalance = AllPreYearBalance;
        }

        private void Search(SearchIn m_SearchIn, SearchOp SrchOP1, string SearchParam1, SearchOp SrchOP2, string SearchParam2)
        {

            //Let the whole row to be selected
            grdListView.SelectionMode = SourceGrid.GridSelectionMode.Row;




            //Disable multiple selection
            grdListView.Selection.EnableMultiSelection = false;

            string FilterString = "";
            switch (m_SearchIn)
            {
                #region Account Groups Search
                case SearchIn.Account_Groups:
                #endregion
                #region Accounts Under Search
                case SearchIn.Accounts_Under:
                    try
                    {
                        DataTable dtSrchGroup = AccountGroup.Search(m_SearchIn, SrchOP1, SearchParam1, SrchOP2, SearchParam2, LangMgr.DefaultLanguage, false);
                        FilterString = "";
                        DataRow[] drFound2 = dtSrchGroup.Select(FilterString);
                        //Finally fill all the values in the grid with no filter applied
                        FillAccountGroupInGrid(drFound2);
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }

                    break;
                #endregion


                #region Ledger Under Search
                case SearchIn.Ledgers_Under:

                #endregion
                #region Ledger Search
                case SearchIn.Ledgers:

                    try
                    {
                      DataTable dtSrchLedger = Ledger.Search(m_SearchIn, SrchOP1, SearchParam1, SrchOP2, SearchParam2, LangMgr.DefaultLanguage,false);
                        //if (dtSrchLedger!=null)
                        //{
                        FilterString = "";
                        DataRow[] drFound2 = dtSrchLedger.Select(FilterString);

                      //Finally fill all the values in the grid with no filter applied
                        FillLedgerInGrid(drFound2);
                        //}

                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ListItem m_SearchInItem = (ListItem)cboSrchSearchIn1.SelectedItem;
            ListItem li1 = (ListItem)cboSrchOP1.SelectedItem;
            ListItem li2 = (ListItem)cboSrchOP2.SelectedItem;
            if (li2 == null)
                li2 = new ListItem(0, "");//Begins_With
            m_SearchIn = (SearchIn)m_SearchInItem.ID; //Set the private function searchIn so that gridclick() may know what is the current mode.
            try
            {
                Search((SearchIn)m_SearchInItem.ID, (SearchOp)li1.ID, txtSrchParam1.Text, (SearchOp)li2.ID, txtSrchParam2.Text);
                tabDisplay.SelectedIndex = 1; //Select the listview
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        /// <summary>
        /// Fills the Account fields from account group id
        /// </summary>
        /// <param name="ID"></param>
        private void FillAcGroupForm(int ID)
        {

            tabAccountSetup.SelectedIndex = 0;
            //Get details from ID
            DataTable dtGroup = AccountGroup.GetGroupByID(ID, LangMgr.DefaultLanguage);

            DataRow drGroup = dtGroup.Rows[0];
            txtGroupCode.Text = drGroup["LedgerCode"].ToString();
            txtGroupID.Text = drGroup["ID"].ToString();
            txtGroupName.Text = drGroup["Name"].ToString();
            txtDescription.Text = drGroup["Remarks"].ToString();
            string strUnder = drGroup["Parent"].ToString();
            if (strUnder == null)
                cboParentGrp.Text = "(MAIN ACCOUNT)";
            else
                cboParentGrp.Text = strUnder;
        }

        /// <summary>
        /// Takes the Ledger ID and fills the ledger form of the corresponding ID
        /// </summary>
        /// <param name="ID"></param>
        private void FillLedgerForm(int ID)
        {
            try
            {
                //Get details from ID
                DataTable dtLedger = Ledger.GetLedgerInfo(ID, LangMgr.DefaultLanguage);
                DataRow drLedger = dtLedger.Rows[0];

                ClearLedgerForm();
                txtLedgerCode.Text = drLedger["LedgerCode"].ToString();
                txtLedgerName.Text = drLedger["LedName"].ToString();
                //txtLdrOpBalance.Text = drLedger["OpBal"].ToString();
                // txtLdrPrevYrBal.Text = drLedger["PreYrBal"].ToString();
                txtLdrLedgerID.Text = drLedger["ID"].ToString();
                _ledgeriD = Convert.ToInt32(drLedger["ID"]);
                // for reference checkBox
                int isRef = Convert.ToInt32((drLedger["IsBillReference"] == DBNull.Value) ? 0 : drLedger["IsBillReference"]);
                chkReference.Checked = Convert.ToBoolean(isRef);
                // txtLdrOpDate.Text = Date.DBToSystem (drLedger["OpCCRDate"].ToString().ToString());

                //string LangDrCr = LangMgr.Translate(drLedger["DrCr"].ToString());
                //string PreYrBalDrCr = LangMgr.Translate(drLedger["DrCr"].ToString());


                //cboOpBalDrCr.Text = LangDrCr;
                //cboPrevYrBal.Text = PreYrBalDrCr;
                cboLdrOpCCY.Text = Currency.GetCodeFromID(Convert.ToInt32(drLedger["OpCCYID"]));
                txtLdrOpCCYRate.Text = drLedger["OpCCR"].ToString();

                txtLdrRemarks.Text = drLedger["Remarks"].ToString();
                //txtBudgetHeadNo.Text = drLedger["BudgetHeadNo"].ToString();
                cboLdrAcGroup.Text = drLedger["GroupName"].ToString();
                txtLdrPerName.Text = drLedger["PersonName"].ToString();
                txtLdrAddress1.Text = drLedger["Address1"].ToString();
                txtLdrAddress2.Text = drLedger["Address2"].ToString();
                txtLdrCity.Text = drLedger["City"].ToString();
                txtLdrTelephone.Text = drLedger["Phone"].ToString();
                txtLdrEmail.Text = drLedger["Email"].ToString();
                txtLdrCompany.Text = drLedger["Company"].ToString();
                txtLdrWebsite.Text = drLedger["Website"].ToString();
                txtVatPanNo.Text = drLedger["VatPanNo"].ToString();
                txtCreditLimit.Text = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal((String.IsNullOrEmpty(drLedger["CreditLimit"].ToString()) ? 0 : drLedger["CreditLimit"]))).ToString();
                chkAutoCalculate.Checked = Convert.ToBoolean((String.IsNullOrEmpty(drLedger["Calculated"].ToString()) ? 0 : drLedger["Calculated"]));
                if (drLedger["Calculate_Rate"].ToString() == "0")
                    RateTextBox.Text = "";
                else
                    RateTextBox.Text = (String.IsNullOrEmpty(drLedger["Calculate_Rate"].ToString()) ? "" : drLedger["Calculate_Rate"]).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
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
            else
            {
                return true;
            }
        }

        //An event which is revoked when a cell is clicked. 
        private void Grid_Click(object sender, EventArgs e)
        {
            //If it is currently in edit or new mode, data has to be saved before going elsewhere
            if (!ContinueWithoutSaving())
            {
                return;
            }

            try
            {
                //Get the Selected Row
                int CurRow = grdListView.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cell = new SourceGrid.CellContext(grdListView, new SourceGrid.Position(CurRow, 0));
                if (cell == null)
                    return;
                //Call the interface function to add the text in the parent form container
                switch (m_SearchIn)
                {
                    case SearchIn.Accounts_Under:
                    case SearchIn.Account_Groups:
                        tabAccountSetup.SelectedIndex = 0;
                        FillAcGroupForm(Convert.ToInt32(cell.Value));
                        break;
                    case SearchIn.Ledger_Op_Bal:
                    case SearchIn.Ledgers_Under:
                    case SearchIn.Ledgers:

                        FillLedgerForm(Convert.ToInt32(cell.Value));
                        tabAccountSetup.SelectedIndex = 1;
                        tabLedgerDetails.SelectedIndex = 0;
                        break;
                }
                ChangeState(EntryMode.NORMAL);
            }
            catch (Exception ex)
            {
                //Do nothing because sometimes while changing tab, no selections would be made
            }
        }

        private void FillLedgerInGrid(DataRow[] drFound)
        {
            try
            {
                grdListView.Rows.Clear();
                //WriteHeader();

                grdListView.Redim(drFound.Count() + 1, 5);
                grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
                grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Ledger");
                grdListView[0, 2] = new SourceGrid.Cells.ColumnHeader("Acc. Group");
                grdListView[0, 3] = new SourceGrid.Cells.ColumnHeader("Type");
                grdListView[0, 0].Column.Width = 40;
                grdListView[0, 1].Column.Width = 100;
                grdListView[0, 2].Column.Width = 100;
                //grdListView[0, 4].Column.Width = 150;
                //grdListView[0, 5].Column.Width = 60;

                //Initialise the event handler
                SourceGrid.Cells.Controllers.CustomEvents CellClick = new SourceGrid.Cells.Controllers.CustomEvents();
                CellClick.Click += new EventHandler(Grid_Click);

                SourceGrid.Cells.Controllers.CustomEvents CellMove = new SourceGrid.Cells.Controllers.CustomEvents();
                CellMove.FocusEntered += new EventHandler(Grid_Click);

                for (int i = 1; i <= drFound.Count(); i++)
                {

                    DataRow dr = drFound[i - 1];

                    grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["ID"].ToString());


                    grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["LedName"].ToString());
                    grdListView[i, 1].AddController(CellClick);
                    grdListView[i, 1].AddController(CellMove);


                    grdListView[i, 2] = new SourceGrid.Cells.Cell(dr["GroupName"].ToString());
                    grdListView[i, 2].AddController(CellClick);
                    grdListView[i, 2].AddController(CellMove);

                    string DebitCredit;
                    if (dr["DrCr"].ToString() == "DEBIT")
                        DebitCredit = "Debit";
                    else if (dr["DrCr"].ToString() == "CREDIT")
                        DebitCredit = "Credit";
                    else
                        DebitCredit = "Unknown";

                    grdListView[i, 3] = new SourceGrid.Cells.Cell(DebitCredit);
                    grdListView[i, 3].AddController(CellClick);
                    grdListView[i, 3].AddController(CellMove);

                }

                //Hide the ID column
                grdListView.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        private void FillAccountGroupInGrid(DataRow[] drFound)
        {
            try
            {
                grdListView.Rows.Clear();
                //WriteHeader();
                switch (m_SearchIn)
                {
                    case SearchIn.Accounts_Under:
                    case SearchIn.Account_Groups:
                        grdListView.Redim(drFound.Count() + 1, 4);
                        grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
                        grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Acc. Group");
                        grdListView[0, 2] = new SourceGrid.Cells.ColumnHeader("Parent Group");
                        grdListView[0, 3] = new SourceGrid.Cells.ColumnHeader("Type");
                        grdListView[0, 0].Column.Width = 40;
                        grdListView[0, 1].Column.Width = 100;
                        grdListView[0, 2].Column.Width = 100;
                        grdListView[0, 3].Column.Width = 50;
                        break;
                    case SearchIn.Ledger_Op_Bal:
                    case SearchIn.Ledgers_Under:
                    case SearchIn.Ledgers:
                        grdListView.Redim(drFound.Count() + 1, 5);
                        grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
                        grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Ledger");
                        grdListView[0, 2] = new SourceGrid.Cells.ColumnHeader("Acc. Group");
                        grdListView[0, 3] = new SourceGrid.Cells.ColumnHeader("Opening Balance");
                        grdListView[0, 3] = new SourceGrid.Cells.ColumnHeader("Type");
                        grdListView[0, 0].Column.Width = 40;
                        grdListView[0, 1].Column.Width = 100;
                        grdListView[0, 2].Column.Width = 100;
                        grdListView[0, 3].Column.Width = 50;
                        break;
                }

                //grdListView[0, 4].Column.Width = 150;
                //grdListView[0, 5].Column.Width = 60;

                //Initialise the event handler
                SourceGrid.Cells.Controllers.CustomEvents CellClick = new SourceGrid.Cells.Controllers.CustomEvents();
                CellClick.Click += new EventHandler(Grid_Click);

                SourceGrid.Cells.Controllers.CustomEvents CellMove = new SourceGrid.Cells.Controllers.CustomEvents();
                CellMove.FocusEntered += new EventHandler(Grid_Click);

                for (int i = 1; i <= drFound.Count(); i++)
                {

                    DataRow dr = drFound[i - 1];

                    grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["GroupID"].ToString());


                    grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                    grdListView[i, 1].AddController(CellClick);
                    grdListView[i, 1].AddController(CellMove);

                    string Parent = dr["Parent"].ToString();
                    if (Parent == null || Parent == "")
                        Parent = ("(PRIMARY)");

                    grdListView[i, 2] = new SourceGrid.Cells.Cell(Parent);
                    grdListView[i, 2].AddController(CellClick);
                    grdListView[i, 2].AddController(CellMove);

                    string DebitCredit;
                    if (dr["Type"].ToString() == "DR")
                        DebitCredit = "Debit";
                    else if (dr["Type"].ToString() == "CR")
                        DebitCredit = "Credit";
                    else
                        DebitCredit = "Unknown";

                    grdListView[i, 3] = new SourceGrid.Cells.Cell(DebitCredit);
                    grdListView[i, 0].AddController(CellClick);
                    grdListView[i, 0].AddController(CellMove);


                }
                grdListView.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

        }


        private void cboSrchSearchIn1_SelectedIndexChanged(object sender, EventArgs e)
        {

            cboSrchOP1.Items.Clear();
            cboSrchOP2.Items.Clear();

            switch ((SearchIn)cboSrchSearchIn1.SelectedIndex)
            {
                case SearchIn.Account_Groups:

                case SearchIn.Accounts_Under:
                case SearchIn.Ledgers:
                case SearchIn.Ledgers_Under:
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Begins_With, LangMgr.Translate("BEGINS_WITH", "Begins With")));
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Contains, LangMgr.Translate("CONTAINS", "Contains")));
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Equals, LangMgr.Translate("EQUALS", "Equals")));
                    cboSrchOP2.Enabled = false;
                    txtSrchParam2.Enabled = false;
                    cboSrchOP1.SelectedIndex = 0;
                    break;
                case SearchIn.Ledger_Op_Bal:
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Equals, LangMgr.Translate("EQUALS", "Equals")));
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Greater_Or_Equals, LangMgr.Translate("GREATER_OR_EQUALS", "Greater or Equals")));
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Smaller_Or_Equals, LangMgr.Translate("SMALLER_OR_EQUALS", "Smaller or Equals")));
                    //cboSrchOP2.Items.Add(new ListItem((int)SearchOp.Equals, LangMgr.Translate("EQUALS", "Equals")));
                    //cboSrchOP2.Items.Add(new ListItem((int)SearchOp.Greater_Or_Equals, LangMgr.Translate("GREATER_OR_EQUALS", "Greater or Equals")));
                    cboSrchOP2.Items.Add(new ListItem((int)SearchOp.Smaller_Or_Equals, LangMgr.Translate("SMALLER_OR_EQUALS", "Smaller or Equals")));
                    cboSrchOP2.Enabled = true;
                    txtSrchParam2.Enabled = true;
                    cboSrchOP1.SelectedIndex = 0;
                    cboSrchOP2.SelectedIndex = 0;
                    break;
            }
        }

        private void btnLdrDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you don't have permission to delete. Please contact your administrator for permission.");
                return;
            }

            //Not Delete able untill a node is selected in the tvAccount treeView
            if (string.IsNullOrEmpty(txtLdrLedgerID.Text))
            {
                Global.MsgError("Please select a Ledger in the Tree View first.");
                return;
            }

            //First check this ledger has done transaction or not?If this ledger has done transaction then dont permit to delete.
            //Check this ledger exist in tblTransaction or not???
            bool chkLedgerInTransaction = Transaction.CheckLedgerInTransaction(_ledgeriD);
            if (chkLedgerInTransaction == true)
            {
                Global.Msg("This Ledger Has Been Used In Transaction So It Can Not Be Deleted!");
                return;
            }
            else
            {
                //Ask if the account head is to be deleted
                if (Global.MsgQuest("Are you sure you want to delete the Account Ledger - " + txtLedgerName.Text.Trim() + "?") == DialogResult.No)
                    return;

                //If the user confirms deletion
                try
                {
                    Ledger mLedger = new Ledger();
                    string delACHead = txtLedgerName.Text.Trim();
                    if (Transaction.CheckBuiltIn(delACHead, LangMgr.DefaultLanguage) == 0)
                    {
                        mLedger.DeleteLedger(_ledgeriD);
                        Global.Msg("Account Ledger - " + txtLedgerName.Text.Trim() + " deleted successfully!");
                    }
                    else
                        Global.Msg("Account Ledger - " + delACHead + " is a System ledger.Sorry! cannot be deleted.");
                    
                    ChangeState(EntryMode.NORMAL);
                    tvAccount.Nodes.Clear();
                    ShowAccountHeadInTreeView(tvAccount);//tvAccount, null, 0);
                    ClearLedgerForm();
                    txtLdrLedgerID.Clear();

                }
                catch (Exception ex)
                {
                    Global.MsgError("Transaction of " + txtLedgerName.Text.Trim() + " is detected.Sorry! Cannot be deleted.");
                }
            }
            IsFieldChanged = false;
        }

        private void cboLdrAcGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            //If this is new mode and Opening Balance has already been selected, simply reset the opening balance so that user has to enter them again
            if (((m_mode == EntryMode.NEW) || (m_mode == EntryMode.EDIT)) && (FromOpeningBalance.Rows.Count > 0) && (cboLdrAcGroup.SelectedItem != m_LdrPrevGroupSelection))
            {
                if (Global.MsgQuest("You have already made an entry for opening balance. If you change the parent group, you need to re-enter the opening balance again. Do you wish to continue?") == DialogResult.Yes)
                {
                    //Flush the Opening Balance and show the Opening Balance dialogue again
                    FromOpeningBalance.Rows.Clear();
                    FromPreYearBalance.Rows.Clear();
                    OpeningBalanceSettings OBS = new OpeningBalanceSettings();
                    OBS.LedgerID = 0;//Since it is in new mode
                    OBS.ParentGroupID = ((ListItem)cboLdrAcGroup.SelectedItem).ID;
                    OBS.LedgerName = txtLedgerName.Text.Trim();

                    frmOpeningBalance frm = new frmOpeningBalance(this, OBS);
                    frm.ShowDialog();
                }
                else
                {
                    //Return to previous entry
                    cboLdrAcGroup.SelectedItem = m_LdrPrevGroupSelection;

                }
            }
            m_LdrPrevGroupSelection = cboLdrAcGroup.SelectedItem;

            if (Convert.ToInt32(((ListItem)cboLdrAcGroup.SelectedItem).ID) == 29 || Convert.ToInt32(((ListItem)cboLdrAcGroup.SelectedItem).ID) == 114)
            {
                //chkReference.Visible = true;
                chkReference.Visible = false;
            }
            else
                chkReference.Visible = false;
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

        private void txtBudgetHeadNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtGroupName.Focus();
            }
        }

        private void txtGroupName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboParentGrp.Focus();
            }
        }

        private void cboParentGrp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDescription.Focus();
            }
        }

        private void txtBudgetHeadNoLdr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtLedgerName.Focus();
            }
        }

        private void txtLedgerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboLdrAcGroup.Focus();
            }
        }

        private void cboLdrAcGroup_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cboLdrAcGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtLdrRemarks.Focus();
            }
        }

        private void txtLdrRemarks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtLdrOpCCYRate.Focus();
            }
        }

        private void txtLdrOpBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboOpBalDrCr.Focus();
            }
        }

        private void cboOpBalDrCr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                // txtLdrPrevYrBal.Focus();
            }
        }

        /*   private void txtLdrPrevYrBal_KeyDown(object sender, KeyEventArgs e)
           {
               if (e.KeyValue == 13)
               {
                   cboPrevYrBal.Focus();
               }
           }
           */
        private void btnOpeningBalance_Click(object sender, EventArgs e)
        {
            if (m_mode == EntryMode.NEW)
                LedgerID = 0;
            else
                LedgerID = Convert.ToInt32(txtLdrLedgerID.Text);

            OpeningBalanceSettings OBS = new OpeningBalanceSettings();
            try
            {

                OBS.LedgerID = LedgerID;
                OBS.ParentGroupID = ((ListItem)cboLdrAcGroup.SelectedItem).ID;
                OBS.LedgerName = txtLedgerName.Text.Trim();
            }
            catch
            {
                //Ignore 
            }
            frmOpeningBalance frm = new frmOpeningBalance(this, OBS);
            if (!frm.IsDisposed)
                frm.ShowDialog();
        }

        private void btnForgetSearch_Click(object sender, EventArgs e)
        {
            //To forget the search, simply search with empty string
            Search(SearchIn.Account_Groups, SearchOp.Begins_With, "", SearchOp.Equals, "");
        }

        private void btnToggleExpand_Click(object sender, EventArgs e)
        {
            if (IsExpand)
            {
                tvAccount.CollapseAll();
                IsExpand = false;
                btnToggleExpand.Text = "Expand";
            }
            else
            {
                tvAccount.ExpandAll();
                IsExpand = true;
                btnToggleExpand.Text = "Collapse";
            }
        }

        private void frmAccountSetup_KeyDown(object sender, KeyEventArgs e)
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
                btnLdrEdit_Click(sender,e);
                btnEdit_Click(sender, e);
            }
            else if (e.KeyCode == Keys.S && e.Control)
            {
                btnLdrSave_Click(sender, e);
                btnSave_Click(sender, e);
            }
        }

        private void txtGroupName_TextChanged(object sender, EventArgs e)
        {
            //if(txtGroupName.Text=="")
            //IsFieldChanged = true;
        }

        private void txtBudgetHeadNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBudgetHeadNo_Enter(object sender, EventArgs e)
        {

        }

        private void txtBudgetHeadNo_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cboParentGrp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtGroupName_Enter(object sender, EventArgs e)
        {

        }

        private void txtGroupName_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cboOpBalDrCr_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtLdrOpBalance_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmAccountSetup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ContinueWithoutSaving())
            {
                e.Cancel = true;
            }
        }

        private void grdListView_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPreviousYearBalance_Click(object sender, EventArgs e)
        {
            if (m_mode == EntryMode.NEW)
                LedgerID = 0;
            else
                LedgerID = Convert.ToInt32(txtLdrLedgerID.Text);

            OpeningBalanceSettings OBS = new OpeningBalanceSettings();
            try
            {

                OBS.LedgerID = LedgerID;
                OBS.ParentGroupID = ((ListItem)cboLdrAcGroup.SelectedItem).ID;
            }
            catch
            {
                //Ignore 
            }
            frmPreviousYearBalance frm = new frmPreviousYearBalance(this, OBS);
            if (!frm.IsDisposed)
                frm.ShowDialog();
        }

        private void txtLedgerName_TextChanged(object sender, EventArgs e)
        {
            //txtLedgerName.Text= FirstLetterToUpper(txtLedgerName.Text);
            //txtLedgerName.Focus();
        }

        private void txtLedgerName_Leave(object sender, EventArgs e)
        {
            txtLedgerName.Text = UppercaseFirst(txtLedgerName.Text);
            //txtLedgerName.Text = txtLedgerName.Text.Replace(txtLedgerName.Text.Substring(0, 1), txtLedgerName.Text.ToUpper());
        }
        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public string FirstLetterToUpper(string str)
        {
            if (str != null)
            {
                if (str.Length > 1)
                    return char.ToUpper(str[0]) + str.Substring(1);
                else
                    return str.ToUpper();
            }
            return str;
        }

        private void chkAutoCalculate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoCalculate.Checked)
            {
                RateLabel.Visible = true;
                RateTextBox.Visible = true;
            }
            else
            {
                RateLabel.Visible = false;
                RateTextBox.Text = "";
                RateTextBox.Visible = false;

            }
        }

        private void RateTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && RateTextBox.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;

            }
        }

        private void txtLedgerName_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void cboLdrAcGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void tpLedgerDetails_Click(object sender, EventArgs e)
        {

        }

        private void tabLedger_Click(object sender, EventArgs e)
        {

        }

        private void txtLdrLedgerID_TextChanged(object sender, EventArgs e)
        {

        }





    }//End class
}//End Namespace
