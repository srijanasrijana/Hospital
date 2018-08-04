using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using System.Collections;

namespace AccSwift.Forms
{
    public partial class frmUserPreference : Form
    {
        UserPreference m_UserPreference;
        private string Prefix = "";
        ArrayList AccountClassID = new ArrayList();
        List<int> AccClassID = new List<int>();
        private int loopCounter = 0;
        private int AccClassIDValue;
        private int ParentAccClass;
        public frmUserPreference()
        {
            InitializeComponent();
        }

        private void frmUserPreference_Load(object sender, EventArgs e)
        {
            //For Default Accounting Class
            LoadCombobox(cmbAccountClass, 0);

            int ParentID = 0;
            ShowAccClassInTreeView(tvAccClass, null, 0);
            ParentID = GetRootAccClassID();

            for (int i = 0; i < 7; i++)
            {
                cmbDecimalPlaces.Items.Add(i.ToString());
            }
            LoadcboBankAccount(cmbBankAccount);
            LoadcboCashAccount(cmbCashAccount);
            LoadcboPurchaseAccount(cmbPurchaseAccount);
            LoadcboSalesAccount(cmbSalesAccount);
            SetSettingsTree();
            m_UserPreference = new UserPreference();
            int checkuserid = User.CurrUserID;
            DataTable dt = m_UserPreference.GetPreferenceCount(checkuserid);
            foreach (DataRow dr in dt.Rows)
            {
                switch (dr["Code"].ToString())
                {
                    //Get GMOption Setting
                    #region Get GMOption Setting
                    case "DEFAULT_DATE":
                        if (dr["Value"].ToString() == "English")
                        {
                            rbDateEnglish.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Nepali")
                        {
                            rbDateNepali.Checked = true;
                        }
                        break;

                    case "DATE_FORMAT":
                        if (dr["Value"].ToString() == "YYYY/MM/DD")
                        {
                            cmbDateFormat.SelectedIndex = 0;
                        }
                        else if (dr["Value"].ToString() == "DD/MM/YYYY")
                        {
                            cmbDateFormat.SelectedIndex = 1;
                        }
                        else if (dr["Value"].ToString() == "MM/DD/YYYY")
                        {
                            cmbDateFormat.SelectedIndex = 2;
                        }
                        break;

                    case "DEFAULT_DECIMALPLACES":
                        cmbDecimalPlaces.Text = dr["Value"].ToString();
                        break;

                    case "COMMA_SEPARATED":
                        if (dr["Value"].ToString() == "0")
                            chkCommaSeparated.Checked = false;
                        if (dr["Value"].ToString() == "1")
                            chkCommaSeparated.Checked = true;
                        break;

                    case "DECIMAL_FORMAT":
                        if (dr["Value"].ToString() == "0")
                            rdbDecimalFormatInBracket.Checked = true;
                        if (dr["Value"].ToString() == "1")
                            rdbDecimalFormatInNegative.Checked = true;
                        break;

                    case "MAIL_SERVER":
                        txtMailServer.Text = dr["Value"].ToString();
                        break;
                    case "SERVER_PORT":
                        txtServerPort.Text = dr["Value"].ToString();
                        break;
                    case "USER_EMAIL":
                        txtUserEmail.Text = dr["Value"].ToString();
                        break;
                    case "PASSWORD":
                        txtPassword.Text = Cryptography.Crypto.Decrypt(dr["Value"].ToString(), "Ac104");
                        //txtpassword.Text = dr["Value"].ToString();
                        break;

                    #endregion
                    //Get GMAccounting Settings
                    #region Get GMAccounting Settings
                    case "DEFAULT_CASH_ACCOUNT": //before this load the combobox
                        foreach (ListItem lst in cmbCashAccount.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["Value"]))
                            {
                                cmbCashAccount.Text = lst.Value;
                                break;
                            }
                        }
                        // cboCashAccount.SelectedIndex =Convert.ToInt32(dr["Value"]);                       
                        break;

                    case "DEFAULT_BANK_ACCOUNT": //before this load the combobox
                        foreach (ListItem lst in cmbBankAccount.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["Value"]))
                            {
                                cmbBankAccount.Text = lst.Value;
                                break;
                            }
                        }
                        //cboBankAccount.SelectedIndex = Convert.ToInt32(dr["Value"]);
                        break;
                    #endregion

                    //Get GeneralMain Settings
                    #region Get GeneralMain Settings
                    case "DEFAULT_PURCHASE_ACCOUNT": //before this load the combobox
                        foreach (ListItem lst in cmbPurchaseAccount.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["Value"]))
                            {
                                cmbPurchaseAccount.Text = lst.Value;
                                break;
                            }
                        }
                        // cboCashAccount.SelectedIndex =Convert.ToInt32(dr["Value"]);                       
                        break;

                    case "DEFAULT_SALES_ACCOUNT": //before this load the combobox
                        foreach (ListItem lst in cmbSalesAccount.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["Value"]))
                            {
                                cmbSalesAccount.Text = lst.Value;
                                break;
                            }
                        }
                        // cboCashAccount.SelectedIndex =Convert.ToInt32(dr["Value"]);                       
                        break;

                    case "ACCOUNT_CLASS": //before this load the combobox
                        foreach (ListItem lst in cmbAccountClass.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["Value"]))
                            {
                                cmbAccountClass.Text = lst.Value;
                                break;
                            }
                        }
                        // cboAccountClass.SelectedIndex = Convert.ToInt32(dr["Value"]);                       
                        break;
                    #endregion

                    #region Get Com,pany Information Setting
                    case "COMPANY_NAME":
                        txtCompanyName.Text = dr["Value"].ToString();
                        break;
                    case "COMPANY_ADDRESS":
                        txtCompanyAddress.Text = dr["Value"].ToString();
                        break;
                    case "COMPANY_CITY":
                        txtCompanyCity.Text = dr["Value"].ToString();
                        break;
                    case "COMPANY_PAN":
                        txtCompanyPAN.Text = dr["Value"].ToString();
                        break;
                    case "COMPANY_PHONE":
                        txtCompanyPhone.Text = dr["Value"].ToString();
                        break;
                    case "COMPANY_SLOGAN":
                        txtCompanySlogan.Text = dr["Value"].ToString();
                        break;

                    #endregion
                }
            }
            tabSettings.SelectedIndex = 1;
            isEmailChanged = false;
        }
        private void SetSettingsTree()
        {
            Font boldFont = new Font(tvUserPreference.Font, FontStyle.Bold);
            /* General
             * 
             * Accounting
             *      Cash/Bank Account
             *      Sales
             *      Purchase
             * 
            */
            TreeNode tnGeneral = tvUserPreference.Nodes.Add("GENERAL", "General", 0);
            tnGeneral.NodeFont = boldFont;
            tnGeneral.Text = tnGeneral.Text; //Just to redraw the node in order to display text properly

            tnGeneral.Nodes.Add("OPTIONS", "Options", 1);
            tnGeneral.Nodes.Add("ACCOUNTS", "Accounts", 1);

            TreeNode tnSalesPurchase = tvUserPreference.Nodes.Add("SALESPURCHASE", "Sales and Purchases", 0);
            tnSalesPurchase.NodeFont = boldFont;
            tnSalesPurchase.Text = tnSalesPurchase.Text;

            tnSalesPurchase.Nodes.Add("SPSETTINGS", "Settings", 1);

            TreeNode tnCompanyInfo = tvUserPreference.Nodes.Add("COMPANYINFO", "Company Info", 0);
            tnCompanyInfo.NodeFont = boldFont;
            tnCompanyInfo.Text = tnCompanyInfo.Text;
            tnCompanyInfo.Nodes.Add("COMPANYINFO", "Company Informations", 1);

            tvUserPreference.ExpandAll();
        }
        private void LoadcboBankAccount(ComboBox cboBankAcc)
        {
            try
            {
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                //DataTable dtroleinfo = User.GetUserInfo(uid);
                //DataRow drrole = dtroleinfo.Rows[0];
                //int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());
                //int roleid = Global.GlobalAccessRoleID;

                //DefaultBank Account according to user root or other users
                int DefaultBankAccNum = Convert.ToInt32(Global.GlobalAccessRoleID == 37 ? Settings.GetSettings("DEFAULT_BANK_ACCOUNT") : UserPreference.GetValue("DEFAULT_BANK_ACCOUNT", uid));
                string DefaultBankName = "";

                //Add Banks to comboBankAccount
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                foreach (DataRow drBankLedgers in dtBankLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboBankAcc.Items.Add(new ListItem((int)drBankLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drBankLedgers["LedgerID"]) == DefaultBankAccNum)
                        DefaultBankName = drLedgerInfo["LedName"].ToString();
                }

                cboBankAcc.DisplayMember = "value";//This value is  for showing at Load condition
                cboBankAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cboBankAcc.Text = DefaultBankName;
            }
            catch
            { }

        }
        private void LoadcboCashAccount(ComboBox cboCashAcc)
        {
            try
            {
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int CashID = AccountGroup.GetGroupIDFromGroupNumber(102);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                //DataTable dtroleinfo = User.GetUserInfo(uid);
                //DataRow drrole = dtroleinfo.Rows[0];
                //int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultCash Account according to user root or other users
                int DefaultCashAccNum = Convert.ToInt32(Global.GlobalAccessRoleID == 37 ? Settings.GetSettings("DEFAULT_CASH_ACCOUNT") : UserPreference.GetValue("DEFAULT_CASH_ACCOUNT", uid));
                string DefaultCashName = "";

                //Add Cash to comboCashAccount
                DataTable dtCashLedgers = Ledger.GetAllLedger(CashID);
                foreach (DataRow drCashLedgers in dtCashLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCashLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboCashAcc.Items.Add(new ListItem((int)drCashLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drCashLedgers["LedgerID"]) == DefaultCashAccNum)
                        DefaultCashName = drLedgerInfo["LedName"].ToString();
                }
                cboCashAcc.DisplayMember = "value";//This value is  for showing at Load condition
                cboCashAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cboCashAcc.Text = DefaultCashName;
            }
            catch
            { }
        }

        private void LoadcboPurchaseAccount(ComboBox cboPurchaseAcc)
        {
            try
            {
                #region BLOCK FOR SHOWING PURCHASE LEDGER IN COMBOBOX
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int PurchaseID = AccountGroup.GetGroupIDFromGroupNumber(12);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                //DataTable dtroleinfo = User.GetUserInfo(uid);
                //DataRow drrole = dtroleinfo.Rows[0];
                //int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultPurchase Account according to user root or other users
                int DefaultPurchaseAccNum = Convert.ToInt32(Global.GlobalAccessRoleID == 37 ? Settings.GetSettings("DEFAULT_PURCHASE_ACCOUNT") : UserPreference.GetValue("DEFAULT_PURCHASE_ACCOUNT", uid));
                string DefaultPurchaseName = "";

                //Add Purchase to comboPurchaseAccount
                DataTable dtPurchaseLedgers = Ledger.GetAllLedger(PurchaseID);
                foreach (DataRow drPurchaseLedgers in dtPurchaseLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboPurchaseAcc.Items.Add(new ListItem((int)drPurchaseLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drPurchaseLedgers["LedgerID"]) == DefaultPurchaseAccNum)
                        DefaultPurchaseName = drLedgerInfo["LedName"].ToString();
                }
                cboPurchaseAcc.DisplayMember = "value";//This value is  for showing at Load condition
                cboPurchaseAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cboPurchaseAcc.Text = DefaultPurchaseName;
                #endregion
            }
            catch
            {
            }

        }

        private void LoadcboSalesAccount(ComboBox cboSalesAcc)
        {
            try
            {
                #region BLOCK FOR SHOWING PURCHASE LEDGER IN COMBOBOX
                //Displaying the all ledgers associated with Sales  AccountGroup in DropDownList
                int Sales_ID = AccountGroup.GetGroupIDFromGroupNumber(112);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                //DataTable dtroleinfo = User.GetUserInfo(uid);
                //DataRow drrole = dtroleinfo.Rows[0];
                //int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultSales Account according to user root or other users
                int DefaultSalesAcc = Convert.ToInt32(Global.GlobalAccessRoleID == 37 ? Settings.GetSettings("DEFAULT_SALES_ACCOUNT") : UserPreference.GetValue("DEFAULT_SALES_ACCOUNT", uid));
                string DefaultSalesName = "";

                //Add Sales to comboSalesAccount
                DataTable dtSalesLedgers = Ledger.GetAllLedger(Sales_ID);
                foreach (DataRow drSalesLedgers in dtSalesLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboSalesAcc.Items.Add(new ListItem((int)drSalesLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox

                    if (Convert.ToInt32(drSalesLedgers["LedgerID"]) == DefaultSalesAcc)
                        DefaultSalesName = drLedgerInfo["LedName"].ToString();
                }
                cboSalesAcc.DisplayMember = "value";//This value is  for showing at Load condition
                cboSalesAcc.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                cboSalesAcc.Text = DefaultSalesName;
                #endregion
            }
            catch
            { }
        }

        private void tvUserPreference_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Name)
            {
                case "OPTIONS":
                    tabSettings.SelectTab(tabGMOptions);
                    lblBreadCrumb.Text = "General >> Options";
                    break;
                case "ACCOUNTS":
                    tabSettings.SelectTab(tabGMAccounting);
                    lblBreadCrumb.Text = "General >> Accounts";
                    break;
                case "SPSETTINGS":
                    tabSettings.SelectTab(tabGeneralMain);
                    lblBreadCrumb.Text = "Sales and Purchase >> Settings";
                    break;
                case "COMPANYINFO":
                    tabSettings.SelectTab(tabCompanyInfo);
                    lblBreadCrumb.Text = "Company Informations >> Settings";
                    break;
                default:
                    tabSettings.SelectTab(tabGeneralMain);
                    lblBreadCrumb.Text = "Sales and Purchase >> Settings";
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ChangeUserPreferences();
            this.Close();
        }

        private void ChangeUserPreferences()
        {

            string Code, Value;
            int PreferenceID, UserID;
            UserID = User.CurrUserID;
            DataTable dt = m_UserPreference.GetPreferenceInfo(UserID);

            if (isEmailChanged)  // if email address has been changed, enable gmail for the new email address
            {
                Global.Msg("Goto: https://accounts.google.com/DisplayUnlockCaptcha to enable gmail.");
                System.Diagnostics.Process.Start("https://accounts.google.com/DisplayUnlockCaptcha");
            }
            //GMOptions Settings
            #region GMOptions Setting

            //bool isNewPreference = false; // to check whether prefernce is added or updated
            if (rbDateEnglish.Checked)
            {
                Code = "DEFAULT_DATE";
                Value = "English";
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //if (dt.Rows.Count < 1)
                //{
                //    m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                //}
                //else
                //{
                //    m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                //}
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;
                }
            }
            else if (rbDateNepali.Checked)
            {
                Code = "DEFAULT_DATE";
                Value = "Nepali";
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                        //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }

            }
            if (cmbDateFormat.Text != "")
            {
                m_UserPreference = new UserPreference();
                Code = "DATE_FORMAT";
                Value = cmbDateFormat.Text.ToString();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);

                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }

            }
            if (cmbDecimalPlaces.Text != "")
            {
                Code = "DEFAULT_DECIMALPLACES";
                Value = cmbDecimalPlaces.Text.ToString();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                // m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);   
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;
                }
            }
            if (chkCommaSeparated.Checked)
            {
                Code = "COMMA_SEPARATED";
                Value = "1";
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                // m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    // isNewPreference = true;
                }
            }
            else if (!chkCommaSeparated.Checked)
            {
                Code = "COMMA_SEPARATED";
                Value = "0";
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;
                }
            }
            if (rdbDecimalFormatInBracket.Checked)
            {
                Code = "DECIMAL_FORMAT";
                Value = "0";
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }
            }
            if (txtMailServer.Text != "")
            {

                Code = "MAIL_SERVER";
                Value = txtMailServer.Text.Trim();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                        //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;
                }
            }

            if (txtServerPort.Text != "")
            {
                Code = "SERVER_PORT";
                Value = txtServerPort.Text.Trim();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                        //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;
                }
            }
            if (txtUserEmail.Text != "")
            {
                Code = "USER_EMAIL";
                Value = txtUserEmail.Text.Trim();
                // string EncryptedPassword = Cryptography.Crypto.Encrypt(Value, "Ac104"); // encryption is not required for mail address
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                        //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }

            }
            if (txtPassword.Text != "")
            {
                Code = "PASSWORD";
                Value = txtPassword.Text.Trim();
                string EncryptedPassword = Cryptography.Crypto.Encrypt(Value, "Ac104");
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, EncryptedPassword);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                        //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, EncryptedPassword);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }
            }
            #endregion

            #region GMAccounting Settings
            if (cmbCashAccount.Text != "")
            {
                ListItem liDefaultCashAccount = (ListItem)cmbCashAccount.SelectedItem;
                Code = "DEFAULT_CASH_ACCOUNT";
                Value = liDefaultCashAccount.ID.ToString();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                //if (dt.Rows.Count < 1)
                //{
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }

            }

            if (cmbBankAccount.Text != "")
            {
                ListItem liDefaultBankAccount = (ListItem)cmbBankAccount.SelectedItem;
                Code = "DEFAULT_BANK_ACCOUNT";
                Value = liDefaultBankAccount.ID.ToString();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }

            }
            #endregion

            #region Company Informations Settings
            if (txtCompanyName.Text != "")
            {
                Code = "COMPANY_NAME";
                Value = txtCompanyName.Text.Trim();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }
            }
            if (txtCompanyName.Text != "")
            {
                Code = "COMPANY_NAME";
                Value = txtCompanyName.Text.Trim();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }
            }
            if (txtCompanyAddress.Text != "")
            {
                Code = "COMPANY_ADDRESS";
                Value = txtCompanyAddress.Text.Trim();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }
            }
            if (txtCompanyCity.Text != "")
            {
                Code = "COMPANY_CITY";
                Value = txtCompanyCity.Text.Trim();
                PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }
            }
            if (txtCompanyPAN.Text != "")
            {
                Code = "COMPANY_PAN";
                Value = txtCompanyPAN.Text.Trim();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }
            }
            if (txtCompanyPhone.Text != "")
            {
                Code = "COMPANY_PHONE";
                Value = txtCompanyPhone.Text.Trim();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }
            }
            if (txtCompanySlogan.Text != "")
            {
                Code = "COMPANY_SLOGAN";
                Value = txtCompanySlogan.Text.Trim();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }
            }

            #endregion

            #region GeneralMain Settings
            if (cmbPurchaseAccount.Text != "")
            {
                ListItem liDefaultPurchaseAccount = (ListItem)cmbPurchaseAccount.SelectedItem;
                Code = "DEFAULT_PURCHASE_ACCOUNT";
                Value = liDefaultPurchaseAccount.ID.ToString();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);

                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }

            }

            if (cmbSalesAccount.Text != "")
            {
                ListItem liDefaultSalesAccount = (ListItem)cmbSalesAccount.SelectedItem;
                Code = "DEFAULT_SALES_ACCOUNT";
                Value = liDefaultSalesAccount.ID.ToString();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }

            }

            if (cmbAccountClass.Text != "")
            {
                ListItem liAccClassID = new ListItem();
                liAccClassID = (ListItem)cmbAccountClass.SelectedItem;
                Code = "ACCOUNT_CLASS";
                Value = liAccClassID.ID.ToString();
                //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
                //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);

                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Code"].ToString() == Code)
                    {
                        //m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
                        m_UserPreference.UpdateUserPreference(UserID, Code, Value);
                        count = 1;
                    }
                if (count == 0)
                {
                    //m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
                    m_UserPreference.SetUserPreference(UserID, Code, Value);
                    //isNewPreference = true;

                }

            }
            //// Read checked AccClassID
            //ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            //foreach (string tag in arrNode)
            //{
            //    AccClassID.Add(Convert.ToInt32(tag));
            //}
            //Code = "ACCOUNT_CLASS";
            //AccClassIDValue = Convert.ToInt32(AccClassID[0].ToString());
            //ParentAccClass = GetRootAccClassID();
            //Value = ParentAccClass.ToString();

            //PreferenceID = Convert.ToInt32(m_UserPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_UserPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_UserPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            //m_UserPreference.SetSettings(Code, Value);
            #endregion

            #region change the global variables for users other than system admin, when the user preference is changed
            if (Global.GlobalAccessRoleID != 37)
            {
                if (rbDateNepali.Checked)
                {
                    Global.Default_Date = Date.DateType.Nepali;
                }
                else if (rbDateEnglish.Checked)
                {
                    Global.Default_Date = Date.DateType.English;
                }

                switch (cmbDateFormat.Text)
                {
                    case "YYYY/MM/DD":
                        Date.DefaultFormat = Date.DateFormat.YYYY_MM_DD;
                        break;
                    case "DD/MM/YYYY":
                        Date.DefaultFormat = Date.DateFormat.DD_MM_YYYY;
                        break;
                    case "MM/DD/YYYY":
                        Date.DefaultFormat = Date.DateFormat.MM_DD_YYYY;
                        break;
                }

                Global.DecimalPlaces = Convert.ToInt32(cmbDecimalPlaces.Text);

                Global.mailserver = txtMailServer.Text;
                Global.serverport = txtServerPort.Text;
                Global.useremail = txtUserEmail.Text;
                Global.password = Cryptography.Crypto.Encrypt(txtPassword.Text, "Ac104"); ;

                Global.Comma_Separated = chkCommaSeparated.Checked;
            } 
            #endregion

            Global.Msg("User Preferences Modified Sucessfully!!!");
            isEmailChanged = false;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ChangeUserPreferences();
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
                    //Check if it is a parent Or if it has children
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
                int m_RecCount = (int)User.GetAccessInfo(AccClassID).Rows.Count;
                return m_RecCount;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return -1;
            }
        }

        private int GetRootAccClassID()
        {
            if (AccClassIDValue > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(AccClassIDValue));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }
            return 1;//The default root class ID
        }

        private void LoadCombobox(ComboBox cboAccClass, int AccClassID)
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
            DataTable dt = AccountClass.GetAccClassTable(AccClassID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                cboAccClass.Items.Add(new ListItem((int)dr["AccClassID"], Prefix + " " + dr[LangField].ToString()));
                Prefix += "----";
                LoadCombobox(cboAccClass, Convert.ToInt16(dr["AccClassID"].ToString()));
            }
            //Prefix = "--";
            if (Prefix.Length > 1)
            {
                Prefix = Prefix.Remove(Prefix.Length - 4, 4);
            }
            cboAccClass.SelectedIndex = 0;
        }

        bool isEmailChanged = false;    
        private void txtUserEmail_TextChanged(object sender, EventArgs e)
        {
            isEmailChanged = true;
        }

    }
}
