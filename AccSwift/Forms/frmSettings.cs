using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BusinessLogic;
using DateManager;
using System.Collections;
using Microsoft.Win32;


namespace AccSwift
{
    public partial class frmSettings : Form
    {
        string cboSalesTax2 = "";
        string cboSalesTax3 = "";
        string cboPurchaseTax2 = "";
        string cboPurchaseTax3 = "";
        ArrayList AccountClassID = new ArrayList();
        List<int> AccClassID = new List<int>();
        private int loopCounter = 0;
        private int AccClassIDValue;
        private int ParentAccClass;
        public frmSettings()
        {
            InitializeComponent();
        }

        private void LoadcboBankAccount(ComboBox cboBankAcc)
        {
            try
            {
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
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultBank Account according to user root or other users
                int DefaultCashAccNum = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_CASH_ACCOUNT") : UserPreference.GetValue("DEFAULT_CASH_ACCOUNT", uid));
                string DefaultCashName = "";

                //Add Banks to comboBankAccount
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
            DataTable dtroleinfo = User.GetUserInfo(uid);
            DataRow drrole = dtroleinfo.Rows[0];
            int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


            //DefaultBank Account according to user root or other users
            int DefaultPurchaseAccNum = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_PURCHASE_ACCOUNT") : UserPreference.GetValue("DEFAULT_PURCHASE_ACCOUNT", uid));
            string DefaultPurchaseName = "";

            //Add Banks to comboBankAccount
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
            { }
        }

        private void LoadcboSalesAccount(ComboBox cboSalesAcc)
        {
            try
            {
                #region BLOCK FOR SHOWING PURCHASE LEDGER IN COMBOBOX
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int Sales_ID = AccountGroup.GetGroupIDFromGroupNumber(112);

                //Find user and get the access role type
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());


                //DefaultSales Account according to user root or other users
                int DefaultSalesAcc = Convert.ToInt32(roleid == 37 ? Settings.GetSettings("DEFAULT_SALES_ACCOUNT") : UserPreference.GetValue("DEFAULT_SALES_ACCOUNT", uid));
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

       
        private void frmSettings_Load(object sender, EventArgs e)
        {
            //For Default Accounting Class
            int ParentID = 0;
            ShowAccClassInTreeView(treeAccClass, null, 0);
            ParentID = GetRootAccClassID();
            
             #region for showing in combobox in slabs settings
            //sales
            //for tax1
            cboSalesTx1.Items.Add("Nt Amt");
            cboSalesTx1.Items.Add("Gross");

            //for tax2
            cboSalesTx2.Items.Add("Nt Amt");
            cboSalesTx2.Items.Add("Gross");
            cboSalesTx2.Items.Add("Tax 1");

            //for tax 3
            cboSalesTx3.Items.Add("Nt Amt");
            cboSalesTx3.Items.Add("Gross");
            cboSalesTx3.Items.Add("Tax 1");
            cboSalesTx3.Items.Add("Tax 2");

            //Purchase

            //for tax1
            cboPurchTx1.Items.Add("Nt Amt");
            cboPurchTx1.Items.Add("Gross");
            
            //for tax2
            cboPurchTx2.Items.Add("Nt Amt");
            cboPurchTx2.Items.Add("Gross");
            cboPurchTx2.Items.Add("Tax 1");

            //for tax3
            cboPurchTx3.Items.Add("Nt Amt");
            cboPurchTx3.Items.Add("Gross");
            cboPurchTx3.Items.Add("Tax 1");
            cboPurchTx3.Items.Add("Tax 2");

           #endregion
            //Adding Number of Decimal Places in Combobox
            for (int i = 0; i < 7; i++)
            {
                cboDecimalPlaces.Items.Add(i.ToString());
            }
            LoadcboBankAccount(cboBankAccount);
            LoadcboCashAccount(cboCashAccount);
            LoadcboPurchaseAccount(cmboPurchaseAccount);
            LoadcboSalesAccount(cmboSalesAccount);
            //LoadcboAccountingClass(cboaccountingclass);

            //rbtnCashWarn.Checked = true;
            //rbtnBankWarn.Checked = true;
            SetSettingsTree();
            DataTable dt = Settings.GetSettingTable();
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
                            cboDateFormat.SelectedIndex = 0;
                        }
                        else if (dr["Value"].ToString() == "DD/MM/YYYY")
                        {
                            cboDateFormat.SelectedIndex = 1;
                        }
                        else if (dr["Value"].ToString() == "MM/DD/YYYY")
                        {
                            cboDateFormat.SelectedIndex = 2;
                        }
                    
                        break;

                    case "DEFAULT_DECIMALPLACES":
                        cboDecimalPlaces.Text = dr["Value"].ToString();
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

                    case "DEFAULT_LANGUAGE":
                        if (dr["Value"].ToString() == "English")
                        {
                            rbLangEnglish.Checked = true;                        
                        }
                        else if (dr["Value"].ToString() == "Nepali")
                        {
                            rbLangNepali.Checked = true;
                        }
                        break;

                    case "MULTI_CURRENCY":
                        if (dr["Value"].ToString() == "0")
                            chkMultiCurrency.Checked = false;
                        if (dr["Value"].ToString() == "1")
                            chkMultiCurrency.Checked = true;
                        break;

                
                    case "P/L_AMOUNT":
                       txtamount.Text=dr["Value"].ToString();
                        break;
                    //case "VAT":
                    //    if (dr["Value"].ToString() == "1")
                    //    {
                    //        chkVat.Checked = true;
                    //    }
                    //    else if (dr["Value"].ToString() == "0")
                    //    {
                    //        chkVat.Checked = false;
                    //    }
                        break;
                    #endregion

                    //Get GMAccounting Settings
                    #region Get GMAccounting Settings
                    case "DEFAULT_CASH_ACCOUNT": //before this load the combobox
                        foreach (ListItem lst in cboCashAccount.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["Value"]))
                            {
                                cboCashAccount.Text = lst.Value;
                                break;
                            }
                        }
                       // cboCashAccount.SelectedIndex =Convert.ToInt32(dr["Value"]);                       
                        break;

                    case "DEFAULT_NEGATIVECASH":
                        if (dr["Value"].ToString() == "Allow")
                        {
                            rbtnCashAllow.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Warn")
                        {
                            rbtnCashWarn.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Deny")
                        {
                            rbtnCashDeny.Checked = true;
                        }
                        break;

                    case "DEFAULT_BANK_ACCOUNT": //before this load the combobox
                        foreach (ListItem lst in cboBankAccount.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["Value"]))
                            {
                                cboBankAccount.Text = lst.Value;
                                break;
                            }
                        }
                        //cboBankAccount.SelectedIndex = Convert.ToInt32(dr["Value"]);
                        break;

                    case "DEFAULT_NEGATIVEBANK":
                        if (dr["Value"].ToString() == "Allow")
                        {
                            rbtnBankAllow.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Warn")
                        {
                            rbtnBankWarn.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Deny")
                        {
                            rbtnBankDeny.Checked = true;
                        }
                        break;

                    case "CREDIT_LIMIT":
                        if (dr["Value"].ToString() == "0")
                        {
                            chkCreditLimit.Checked = false;
                        }
                        if (dr["Value"].ToString() == "Warn")
                        {
                            chkCreditLimit.Checked = true;
                            rdbCrLimWarnOnly.Checked = true;
                        }
                        if (dr["Value"].ToString() == "Deny")
                        {
                            chkCreditLimit.Checked = true;
                            rdbCrLimDeny.Checked = true;
                        }
                        break;

                    case "POST_DATE_TRANSACTION":
                        if (dr["Value"].ToString() == "Allow")
                        {
                            rdbAllowPT.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Warn")
                        {
                            rdbWarnPT.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Deny")
                        {
                            rdbDenyPT.Checked = true;
                        }
                        break;

                    case "DEFAULT_BUDGET_LIMIT":
                        if (dr["Value"].ToString() == "Allow")
                        {
                            rdbAllowBL.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Warn")
                        {
                            rdbWarnBL.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Deny")
                        {
                            rdbDenyBL.Checked = true;
                        }
                        break;
                        //break;

                    #endregion

                    //Get GeneralMain Settings
                    #region Get GeneralMain Settings
                    case "DEFAULT_PURCHASE_ACCOUNT": //before this load the combobox
                        foreach (ListItem lst in cmboPurchaseAccount.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["Value"]))
                            {
                                cmboPurchaseAccount.Text = lst.Value;
                                break;
                            }
                        }
                        // cboCashAccount.SelectedIndex =Convert.ToInt32(dr["Value"]);                       
                        break;

                    case "DEFAULT_SALES_ACCOUNT": //before this load the combobox
                        foreach (ListItem lst in cmboSalesAccount.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["Value"]))
                            {
                                cmboSalesAccount.Text = lst.Value;
                                break;
                            }
                        }
                        // cboCashAccount.SelectedIndex =Convert.ToInt32(dr["Value"]);                       
                        break;

                        case "SALES_REPORT_TYPE":
                        //foreach (ListItem lst in cmboSalesReprtType.Items)
                        //{
                        //    if (lst.Value.ToString() == dr["Value"].ToString())
                        //    {
                        //        cmboSalesReprtType.Text = lst.Value;
                        ////    }
                        if (dr["Value"].ToString() == "General")
                        {
                            cmboSalesReprtType.Text = "General";
                        }
                        else
                        if (dr["Value"].ToString() == "POS")
                        {
                            cmboSalesReprtType.Text = "POS";
                        }
                        else if (dr["Value"].ToString() == "Hospital")
                        {
                            cmboSalesReprtType.Text = "Hospital";
                        }
                        break;

                        //case "HOSPITAL_REPORT_TYPE":
                        //if (dr["Value"].ToString() == "General")
                        //{
                        //    cmboSalesReprtType.Text = "General";
                        //}
                        //else if (dr["Value"].ToString() == "Hospital")
                        //{
                        //    cmboSalesReprtType.Text = "Hospital";
                        //}
                        //break;

                    case "DEFAULT_NEGATIVESTOCK":
                        if (dr["Value"].ToString() == "Allow")
                        {
                            rbtnSalesAllow.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Warn")
                        {
                            rbtnSalesWarn.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "Deny")
                        {
                            rbtnSalesDeny.Checked = true;
                        }
                        break;
                    #endregion

                    //Get OBackupRestore Settings
                    #region Get OBackupRestore Settings
                    case "AUTO_BACKUP":
                        if (dr["Value"].ToString() == "1")
                            chkAutoBackUp.Checked = true;
                        else
                            chkAutoBackUp.Checked = false;
                        break;

                    case "BACKUP_INTERVAL_DAY":
                        txtBackUpIntervalDay.Text = dr["Value"].ToString();                      
                        break;

                    case "BACKUP_PATH":
                        if (dr["Value"].ToString() == "0")
                        {
                            rdbDefaultBackUpPath.Checked = true;
                        }
                        else if (dr["Value"].ToString() != "0")
                        {
                            rdbCustomBackUpPath.Checked = true;
                            txtCustomBackUpPath.Text = dr["Value"].ToString();
                        }
                        break;

                    #endregion

                    //Get ODefaults Settings
                    #region Get ODefaults Settings
                    case "PREPARED_BY":
                        txtPreparedBy.Text = dr["Value"].ToString();
                        break;
                    case "CHECKED_BY":
                        txtCheckedBy.Text = dr["Value"].ToString();
                        break;
                    case "APPROVED_BY":
                        txtApprovedBy.Text = dr["Value"].ToString();
                        break;
                    case "AUTHORISED_CAPITAL":
                        txtauthorisedcapital.Text = dr["Value"].ToString();
                        break;
                    case "ISSUED_CAPITAL":
                       txtissuedcapital.Text = dr["Value"].ToString();
                        break;
                    case "MAIL_SERVER":
                        txtmailserver.Text = dr["Value"].ToString();
                        break;
                    case "SERVER_PORT":
                       txtserverport.Text = dr["Value"].ToString();
                        break;
                    case "USER_EMAIL":
                        txtuseremail.Text = dr["Value"].ToString();
                        break;
                    case "PASSWORD":
                        txtpassword.Text = Cryptography.Crypto.Decrypt(dr["Value"].ToString(), "Ac104"); // write the decrypted password in textbox, otherwise 
                        // the encrypted password is encrypted again and again i.e. everytime the OK or Apply button is clicked.
                        break;
                    #endregion

                    //Get Slabs Settings
                    #region GET SLABS SETTINGS

                    //for Sales 
                    case "DEFAULT_SALES_TAX1":
                        if (dr["Value"].ToString() == "Nt Amt")
                        {
                            cboSalesTx1.Text = "Nt Amt";
                        }
                        if (dr["Value"].ToString() == "Gross")
                        {
                            cboSalesTx1.Text = "Gross";
                        }
                        break;
                    case "DEFAULT_SALES_TAX1CHECK":
                        if (dr["Value"].ToString() == "1")
                        {
                            chkSalesTx1.Checked = true;
                            cboSalesTx1.Enabled = true;
                        }
                        if (dr["Value"].ToString() == "0")
                        {
                            chkSalesTx2.Checked = false;
                            cboSalesTx1.Enabled = false;
                        }
                        break;
                    case "DEFAULT_SALES_TAX2":
                        if (dr["Value"].ToString() == "Nt Amt")
                        {
                            cboSalesTx2.Text = "Nt Amt";
                            cboSalesTax2 = "Nt Amt";

                        }
                        if (dr["Value"].ToString() == "Gross")
                        {
                            cboSalesTx2.Text = "Gross";
                            cboSalesTax2 = "Gross";
                        }
                        if (dr["Value"].ToString() == "Tax 1")
                        {
                            cboSalesTx2.Text = "Tax 1";
                            cboSalesTax2 = "Nt Amt";

                        }
                        break;
                    case "DEFAULT_SALES_TAX2CHECK":
                        if (dr["Value"].ToString() == "1")
                        {
                            chkSalesTx2.Checked = true;
                            cboSalesTx2.Enabled = true;
                        }
                        if (dr["Value"].ToString() == "0")
                        {
                            chkSalesTx2.Checked = false;
                            cboSalesTx2.Enabled = false;
                        }
                        break;
                    case "DEFAULT_SALES_TAX3":
                        if (dr["Value"].ToString() == "Nt Amt")
                        {
                            cboSalesTx3.Text = "Nt Amt";
                            cboSalesTax3 = "Nt Amt";
                        }
                        if (dr["Value"].ToString() == "Gross")
                        {
                            cboSalesTx3.Text = "Gross";
                            cboSalesTax3 = "Gross";
                        }
                        if (dr["Value"].ToString() == "Tax 1")
                        {
                            cboSalesTx3.Text = "Tax 1";
                            cboSalesTax3 = "Nt Amt";
                        }
                        if (dr["Value"].ToString() == "Tax 2")
                        {
                            cboSalesTx3.Text = "Tax 2";
                            cboSalesTax3 = "Nt Amt";
                        }
                        break;
                    case "DEFAULT_SALES_TAX3CHECK":
                        if (dr["Value"].ToString() == "1")
                        {
                            chkSalesTx3.Checked = true;
                            cboSalesTx3.Enabled = true;
                        }
                        if (dr["Value"].ToString() == "0")
                        {
                            chkSalesTx3.Checked = false;
                            cboSalesTx3.Enabled = false;
                        }
                        break;
                    case "DEFAULT_SALES_VAT":
                        if (dr["Value"].ToString() == "1")
                        {
                            chkSalesVat.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "0")
                        {
                            chkSalesVat.Checked = false;
                        }
                        break;

                    //for purchase

                    case "DEFAULT_PURCHASE_TAX1":

                        if (dr["Value"].ToString() == "Nt Amt")
                        {
                            cboPurchTx1.Text = "Nt Amt";
                        }
                        if (dr["Value"].ToString() == "Gross")
                        {
                            cboPurchTx1.Text = "Gross";
                        }
                        break;
                    case "DEFAULT_PURCHASE_TAX1CHECK":
                        if (dr["Value"].ToString() == "1")
                        {
                            chkPurchTx1.Checked = true;
                            cboPurchTx1.Enabled = true;
                        }
                        if (dr["Value"].ToString() == "0")
                        {
                            chkPurchTx1.Checked = false;
                            cboPurchTx1.Enabled = false;
                        }
                        break;

                    case "DEFAULT_PURCHASE_TAX2":

                        if (dr["Value"].ToString() == "Nt Amt")
                        {
                            cboPurchTx2.Text = "Nt Amt";
                            cboPurchaseTax2 = "Nt Amt";
                        }
                        if (dr["Value"].ToString() == "Gross")
                        {
                            cboPurchTx2.Text = "Gross";
                            cboPurchaseTax2 = "Gross";
                        }
                        if (dr["Value"].ToString() == "Tax 1")
                        {
                            cboPurchTx2.Text = "Tax 1";
                            cboPurchaseTax2 = "Nt Amt";
                        }
                        break;
                    case "DEFAULT_PURCHASE_TAX2CHECK":
                        if (dr["Value"].ToString() == "1")
                        {
                            chkPurchTx2.Checked = true;
                            cboPurchTx2.Enabled = true;
                        }
                        if (dr["Value"].ToString() == "0")
                        {
                            chkPurchTx2.Checked = false;
                            cboPurchTx2.Enabled = false;
                        }
                        break;

                    case "DEFAULT_PURCHASE_TAX3":
                        if (dr["Value"].ToString() == "Nt Amt")
                        {
                            cboPurchTx3.Text = "Nt Amt";
                            cboPurchaseTax3 = "Nt Amt";
                        }
                        if (dr["Value"].ToString() == "Gross")
                        {
                            cboPurchTx3.Text = "Gross";
                            cboPurchaseTax3 = "Gross";
                        }
                        if (dr["Value"].ToString() == "Tax 1")
                        {
                            cboPurchTx3.Text = "Tax 1";
                            cboPurchaseTax3 = "Nt Amt";
                        }
                        if (dr["Value"].ToString() == "Tax 2")
                        {
                            cboPurchTx3.Text = "Tax 2";
                            cboPurchaseTax3 = "Nt Amt";
                        }
                        break;
                    case "DEFAULT_PURCHASE_TAX3CHECK":
                        if (dr["Value"].ToString() == "1")
                        {
                            chkPurchTx3.Checked = true;
                            cboPurchTx3.Enabled = true;
                        }
                        if (dr["Value"].ToString() == "0")
                        {
                            chkPurchTx3.Checked = false;
                            cboPurchTx3.Enabled = false;
                        }
                        break;

                    case "DEFAULT_PURCHASE_VAT":
                        if (dr["Value"].ToString() == "1")
                        {
                            chkPurchVat.Checked = true;
                        }
                        else if (dr["Value"].ToString() == "0")
                        {
                            chkPurchVat.Checked = false;
                        }
                        break;

                    case "CUSTOMDUTY":
                        if (dr["Value"].ToString() == "1")
                            chkcustomduty.Checked = true;
                        else
                            chkcustomduty.Checked = false;
                        break;



                    #endregion

                    //Get GeneralMain Settings
                    #region Get GeneralMain Settings
                    //For Accounting Class
                    //case "ACCOUNT_CLASS":
                    //    if (dr["Value"].ToString() == "1")
                    //    {
                    //        cboSalesTx1.Text = "Nt Amt";
                    //    }
                    //    if (dr["Value"].ToString() == "Gross")
                    //    {
                    //        cboSalesTx1.Text = "Gross";
                    //    }
                    //    break;
                    #endregion

                }
                isEmailChanged = false;
            }


            tvSettings.ExpandAll();
            tvSettings.SelectedNode = tvSettings.Nodes[0].Nodes[0];
        }

        private void SetSettingsTree()
        {
            Font boldFont = new Font(tvSettings.Font, FontStyle.Bold);
            /* General
             * 
             * Accounting
             *      Cash/Bank Account
             *      Sales
             *      Purchase
             * 
            */
            TreeNode tnGeneral=tvSettings.Nodes.Add("GENERAL","General",1);
            tnGeneral.NodeFont = boldFont;
            tnGeneral.Text = tnGeneral.Text; //Just to redraw the node in order to display text properly

            tnGeneral.Nodes.Add("OPTIONS", "Options",1);
            tnGeneral.Nodes.Add("ACCOUNTS", "Accounts", 1);
            
            TreeNode tnSalesPurchase = tvSettings.Nodes.Add("SALESPURCHASE", "Sales and Purchases",1);
            tnSalesPurchase.NodeFont = boldFont;
            tnSalesPurchase.Text = tnSalesPurchase.Text;

            tnSalesPurchase.Nodes.Add("SPSETTINGS", "Settings", 1);
            tnSalesPurchase.Nodes.Add("SPSLABS", "Slabs", 1);
        
            TreeNode tnOthers= tvSettings.Nodes.Add("OTHERS","Others",1);
            tnOthers.NodeFont = boldFont;
            tnOthers.Text = tnOthers.Text; //Just to redraw the node in order to display text properly
            tnOthers.Nodes.Add("ODEFAULTS", "Defaults", 1);
            tnOthers.Nodes.Add("OBACKUPRESTORE", "Backup and Restore", 1);
        }

        private void tvSettings_AfterSelect(object sender, TreeViewEventArgs e)
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
                case "ODEFAULTS":
                    tabSettings.SelectTab(tabODefaults);
                    lblBreadCrumb.Text = "Others >> Defaults";
                    break;
                case "OBACKUPRESTORE":
                    tabSettings.SelectTab(tabBackupRestore);
                    lblBreadCrumb.Text = "Others >> Backup and Restore";
                    break;
                case "SPSLABS":
                    tabSettings.SelectTab(tabSlabs);
                    lblBreadCrumb.Text = "Sales and Purchase >>Slabs ";
                    break;
                case "SPSETTINGS":
                    tabSettings.SelectTab(tabGeneralMain);
                    lblBreadCrumb.Text = "Sales and Purchase >> Settings";
                    break;

                default:
                    tabSettings.SelectedTab = tabSettings.SelectedTab;
                    break;
            }

        }

        /// <summary>
        /// This is the method for Modification of tblSettings...The value of tblSetting will be changed according to Code 
        /// </summary>
        private void ModifySettings()
        {
            if (isEmailChanged)  // if email address has been changed, enable gmail for the new email address
            {
                Global.Msg("Goto: https://accounts.google.com/DisplayUnlockCaptcha to enable gmail.");
                System.Diagnostics.Process.Start("https://accounts.google.com/DisplayUnlockCaptcha");
            }
            Settings m_Settings = new Settings();
            string Code, Value;

            //GMOptions Settings
            #region GMOptions Setting
            if (rbDateEnglish.Checked)
            {
                Code = "DEFAULT_DATE";
                Value = "English";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Date = Date.DateType.English;//Passing the Default Date to the Global variable When this information is saved to database
                //Such that not necessary to close the main form to update this information
            }
            else if (rbDateNepali.Checked)
            {
                Code = "DEFAULT_DATE";
                Value = "Nepali";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Date = Date.DateType.Nepali;

            }
            if (cboDateFormat.Text != "")
            {
                Code = "DATE_FORMAT";
                Value = cboDateFormat.Text.ToString();
                m_Settings.SetSettings(Code, Value);
                switch (cboDateFormat.Text)
                {
                    case "YYYY/MM/DD":
                        Global.Default_Formate = Date.DateFormat.YYYY_MM_DD;
                        break;
                    case "DD/MM/YYYY":
                        Global.Default_Formate = Date.DateFormat.DD_MM_YYYY;
                        break;
                    case "MM/DD/YYYY":
                        Global.Default_Formate = Date.DateFormat.MM_DD_YYYY;
                        break;
                    default:
                        Global.Default_Formate = Date.DateFormat.YYYY_MM_DD;
                        break;
                }
            }
            if (cboDecimalPlaces.Text != "")
            {
                Code = "DEFAULT_DECIMALPLACES";
                Value = cboDecimalPlaces.Text.ToString();
                m_Settings.SetSettings(Code, Value);
                Global.DecimalPlaces = Convert.ToInt16(cboDecimalPlaces.Text);
            }
            if (chkCommaSeparated.Checked)
            {
                Code = "COMMA_SEPARATED";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Comma_Separated = true;
            }
            else if (!chkCommaSeparated.Checked)
            {
                Code = "COMMA_SEPARATED";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Comma_Separated = false;
            }
            if (rdbDecimalFormatInBracket.Checked)
            {
                Code = "DECIMAL_FORMAT";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Decimal_Format = "0";
            }
            else if (rdbDecimalFormatInNegative.Checked)
            {
                Code = "DECIMAL_FORMAT";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Decimal_Format = "1";
            }
            if (rbLangEnglish.Checked)
            {
                Code = "DEFAULT_LANGUAGE";
                Value = "English";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Language = Lang.English;
            }
            else if (rbLangNepali.Checked)
            {
                Code = "DEFAULT_LANGUAGE";
                Value = "Nepali";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Language = Lang.Nepali;
            }
            //BLOCK FOR MULTI CURRENCY SETTINGS
            if (chkMultiCurrency.Checked)
            {
                Code = "MULTI_CURRENCY";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Multi_Currency = "1";
            }
            else
            {
                Code = "MULTI_CURRENCY";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Multi_Currency = "0";
            }

            Code = "P/L_AMOUNT";
            Value = txtamount.Text;
            m_Settings.SetSettings(Code, Value);
            Global.plamount = Convert.ToDouble(txtamount.Text);
            ////BLOCK FOR VAT SETTINGS
            //if (chkVat.Checked)
            //{
            //    Code = "VAT";
            //    Value = "1";
            //    m_Settings.SetSettings(Code, Value);
            //    Global.VAT_Settings = "1";
            //}
            //else
            //{
            //    Code = "VAT";
            //    Value = "0";
            //    m_Settings.SetSettings(Code, Value);
            //    Global.VAT_Settings = "0";
            //}
           #endregion

            //GMAccounting Settings
            #region GMAccounting Settings
            if (cboCashAccount.Text !="")
            {
                ListItem liDefaultCashAccount = (ListItem)cboCashAccount.SelectedItem;
                Code = "DEFAULT_CASH_ACCOUNT";
                Value = liDefaultCashAccount.ID.ToString();
                m_Settings.SetSettings(Code, Value);
                Global.Default_Cash_Account =liDefaultCashAccount.Value;     //////////////////////////////
            }
            if (rbtnCashAllow.Checked)
            {
                Code = "DEFAULT_NEGATIVECASH";
                Value = "Allow";
                m_Settings.SetSettings(Code, Value);
                Global.Default_NegativeCash = NegativeCash.Allow;
            }
            else if (rbtnCashWarn.Checked)
            {
                Code = "DEFAULT_NEGATIVECASH";
                Value = "Warn";
                m_Settings.SetSettings(Code, Value);
                Global.Default_NegativeCash = NegativeCash.Warn;
            }
            else if (rbtnCashDeny.Checked)
            {
                Code = "DEFAULT_NEGATIVECASH";
                Value = "Deny";
                m_Settings.SetSettings(Code, Value);
                Global.Default_NegativeCash = NegativeCash.Deny;
            }
            if (cboBankAccount.Text != "")
            {
                ListItem liDefaultBankAccount = (ListItem)cboBankAccount.SelectedItem;
                Code = "DEFAULT_BANK_ACCOUNT";
                Value = liDefaultBankAccount.ID.ToString();
                m_Settings.SetSettings(Code, Value);
                Global.Default_Bank_Account = liDefaultBankAccount.Value;         /////////////////////////////
            }
            //Block for NegativeBank
            if (rbtnBankAllow.Checked)
            {
                Code = "DEFAULT_NEGATIVEBANK";
                Value = "Allow";
                m_Settings.SetSettings(Code, Value);
                Global.Default_NegativeBank = NegativeBank.Allow;

            }
            else if (rbtnBankWarn.Checked)
            {
                Code = "DEFAULT_NEGATIVEBANK";
                Value = "Warn";
                m_Settings.SetSettings(Code, Value);
                Global.Default_NegativeBank = NegativeBank.Warn;
            }
            else if (rbtnBankDeny.Checked)
            {
                Code = "DEFAULT_NEGATIVEBANK";
                Value = "Deny";
                m_Settings.SetSettings(Code, Value);
                Global.Default_NegativeBank = NegativeBank.Deny;
            }
            if (chkCreditLimit.Checked)
            {
                if (rdbCrLimWarnOnly.Checked)
                {
                    Code = "CREDIT_LIMIT";
                    Value = "Warn";
                    m_Settings.SetSettings(Code, Value);
                    Global.Credit_Limit = CreditLimit.Warn;        
                }
                else if (rdbCrLimDeny.Checked)
                {
                    Code = "CREDIT_LIMIT";
                    Value = "Deny";
                    m_Settings.SetSettings(Code, Value);
                    Global.Credit_Limit = CreditLimit.Deny;  
                }
            }
            else
            {
                Code = "CREDIT_LIMIT";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Credit_Limit = CreditLimit.Null;  
            }

            //Post date transaction
            if (rdbAllowPT.Checked)
            {
                Code = "POST_DATE_TRANSACTION";
                Value = "Allow";
                m_Settings.SetSettings(Code, Value);                
            }
            else if (rdbWarnPT.Checked)
            {
                Code = "POST_DATE_TRANSACTION";
                Value = "Warn";
                m_Settings.SetSettings(Code, Value);
            }
            else if (rdbDenyPT.Checked)
            {
                Code = "POST_DATE_TRANSACTION";
                Value = "Deny";
                m_Settings.SetSettings(Code, Value);
            }

            //Budget transaction
            if (rdbAllowBL.Checked)
            {
                Code = "DEFAULT_BUDGET_LIMIT";
                Value = "Allow";
                m_Settings.SetSettings(Code, Value);
                Global.Default_BudgetLimit = BudgetLimit.Allow;
            }
            else if (rdbWarnBL.Checked)
            {
                Code = "DEFAULT_BUDGET_LIMIT";
                Value = "Warn";
                m_Settings.SetSettings(Code, Value);
                Global.Default_BudgetLimit = BudgetLimit.Warn;
            }
            else if (rdbDenyBL.Checked)
            {
                Code = "DEFAULT_BUDGET_LIMIT";
                Value = "Deny";
                m_Settings.SetSettings(Code, Value);
                Global.Default_BudgetLimit = BudgetLimit.Deny;
            }
            #endregion

            //GeneralMain Settings    
            #region GeneralMain Settings
            if (cmboPurchaseAccount.Text != "")
            {
                ListItem liDefaultPurchaseAccount = (ListItem)cmboPurchaseAccount.SelectedItem;
                Code = "DEFAULT_PURCHASE_ACCOUNT";
                Value = liDefaultPurchaseAccount.ID.ToString();
                m_Settings.SetSettings(Code, Value);
                Global.Default_Cash_Account = liDefaultPurchaseAccount.Value;     //////////////////////////////
            }

            if (cmboSalesAccount.Text != "")
            {
                ListItem liDefaultSalesAccount = (ListItem)cmboSalesAccount.SelectedItem;
                Code = "DEFAULT_Sales_ACCOUNT";
                Value = liDefaultSalesAccount.ID.ToString();
                m_Settings.SetSettings(Code, Value);
                Global.Default_Cash_Account = liDefaultSalesAccount.Value;     //////////////////////////////
            }

            //Block for NegativeStock
            if (rbtnSalesAllow.Checked)
            {
                Code = "DEFAULT_NEGATIVESTOCK";
                Value = "Allow";
                m_Settings.SetSettings(Code, Value);
                Global.Default_NegativeStock = NegativeStock.Allow;

            }
            else if (rbtnSalesWarn.Checked)
            {
                Code = "DEFAULT_NEGATIVESTOCK";
                Value = "Warn";
                m_Settings.SetSettings(Code, Value);
                Global.Default_NegativeStock = NegativeStock.Warn;
            }
            else if (rbtnSalesDeny.Checked)
            {
                Code = "DEFAULT_NEGATIVESTOCK";
                Value = "Deny";
                m_Settings.SetSettings(Code, Value);
                Global.Default_NegativeStock = NegativeStock.Deny;
            }

            if (cmboSalesReprtType.Text != "")
            {
                //ListItem liDefalutSalesReportType = (ListItem)cmboSalesReprtType.SelectedItem;
                Code = "SALES_REPORT_TYPE";
                
                //Value = liDefalutSalesReportType.Value.ToString();
                Value = cmboSalesReprtType.Text.ToString();
                m_Settings.SetSettings(Code, Value);
                Global.Default_Sales_Report_Type = Value;
            }
            //else
            //{
            //    Code = "HOSPITAL_REPORT_TYPE";
            //    //Value = liDefalutSalesReportType.Value.ToString();
            //    Value = cmboSalesReprtType.Text.ToString();
            //    m_Settings.SetSettings(Code, Value);
            //    Global.Default_Sales_Report_Type = Value;
            //}
            //Read checked AccClassID
            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }
            Code = "ACCOUNT_CLASS";
            AccClassIDValue =Convert.ToInt32(AccClassID[0].ToString());
            ParentAccClass = GetRootAccClassID();
            Value = ParentAccClass.ToString();
            m_Settings.SetSettings(Code, Value);
            #endregion


            //OBackupRestore Settings
            #region OBackupRestore Settings

            if (chkAutoBackUp.Checked)
            {
                if (txtBackUpIntervalDay.Text != "")
                {
                    if (!Misc.IsNumeric(txtBackUpIntervalDay.Text))
                    {
                        MessageBox.Show("Invalid backup Interval day!!");
                        txtBackUpIntervalDay.Focus();
                        return;
                    }
                    else
                    {
                        Code = "BACKUP_INTERVAL_DAY";
                        Value = txtBackUpIntervalDay.Text;
                        m_Settings.SetSettings(Code, Value);
                        Global.Backup_Interval_Day = txtBackUpIntervalDay.Text;
                    }
                }

                if (rdbDefaultBackUpPath.Checked)
                {
                    Code = "BACKUP_PATH";
                    Value = "0";
                    m_Settings.SetSettings(Code, Value);
                    Global.Backup_Path = "0";
                }
                else if (rdbCustomBackUpPath.Checked)
                {
                    if (txtCustomBackUpPath.Text == "")
                    {
                        MessageBox.Show("Enter valid backup path.");
                        txtCustomBackUpPath.Focus();
                        return;
                    }
                    if (Directory.Exists(txtCustomBackUpPath.Text))
                    {
                        Code = "BACKUP_PATH";
                        Value = txtCustomBackUpPath.Text;
                        m_Settings.SetSettings(Code, Value);
                        Global.Backup_Path = txtCustomBackUpPath.Text;
                    }
                    else
                    {
                        try
                        {
                            if (MessageBox.Show("Backup path directory does not exist. Create now?", "Create Directory", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                Directory.CreateDirectory(txtCustomBackUpPath.Text);
                                Code = "BACKUP_PATH";
                                Value = txtCustomBackUpPath.Text;
                                m_Settings.SetSettings(Code, Value);
                                Global.Backup_Path = txtCustomBackUpPath.Text;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Unable to create directory.");
                        }
                    }
                }

                Code = "AUTO_BACKUP";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Auto_BackUp = "1";   
            }
            else
            {
                Code = "AUTO_BACKUP";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Auto_BackUp = "0";            
            }

            #endregion

            //ODefaults Settings
            #region ODefaults Settings
            if (txtPreparedBy.Text != "")
            {
                Code = "PREPARED_BY";
                Value = txtPreparedBy.Text.Trim();
                m_Settings.SetSettings(Code, Value);
                Global.Prepared_By = txtPreparedBy.Text.Trim();
            }
            if (txtCheckedBy.Text != "")
            {
                Code = "CHECKED_BY";
                Value = txtCheckedBy.Text.Trim();
                m_Settings.SetSettings(Code, Value);
                Global.Checked_By = txtCheckedBy.Text.Trim();
            }
            if (txtApprovedBy.Text != "")
            {
                Code = "APPROVED_BY";
                Value = txtApprovedBy.Text.Trim();
                m_Settings.SetSettings(Code, Value);
                Global.Approved_By = txtApprovedBy.Text.Trim();
            }
            if (txtauthorisedcapital.Text != "")
            {
                Code = "AUTHORISED_CAPITAL";
                Value = txtauthorisedcapital.Text.Trim();
                m_Settings.SetSettings(Code, Value);
               
            }
            if (txtissuedcapital.Text != "")
            {
                Code = "ISSUED_CAPITAL";
                Value = txtissuedcapital.Text.Trim();
                m_Settings.SetSettings(Code, Value);
               
            }
            
            if (txtmailserver.Text != "")
            {
                Code = "MAIL_SERVER";
                Value = txtmailserver.Text.Trim();
                m_Settings.SetSettings(Code, Value);
               // Global.Checked_By = txtCheckedBy.Text.Trim();
                Global.mailserver = Value;
            }
            if (txtserverport.Text != "")
            {
                Code = "SERVER_PORT";
                Value = txtserverport.Text.Trim();
                m_Settings.SetSettings(Code, Value);
                //Global.Approved_By = txtApprovedBy.Text.Trim();
                Global.serverport = Value;
            }
            if (txtuseremail.Text != "")
            {
                Code = "USER_EMAIL";
                Value = txtuseremail.Text.Trim();
                m_Settings.SetSettings(Code, Value);
                Global.useremail = Value;

            }
            if (txtpassword.Text != "")
            {
                Code = "PASSWORD";
                Value = txtpassword.Text.Trim();
                string EncryptedPassword = Cryptography.Crypto.Encrypt(Value, "Ac104");
                m_Settings.SetSettings(Code, EncryptedPassword);
                Global.password = EncryptedPassword;
            }
            #endregion

            //Slabs Settings
            #region Slabs Settings
            if (chkSalesTx1.Checked)
            {
                Code = "DEFAULT_SALES_TAX1";
                Value = cboSalesTx1.Text;
                m_Settings.SetSettings(Code, Value);
                Code = "DEFAULT_SALES_TAX1CHECK";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Sales_Tax1Check = "1";
                Global.Default_Sales_Tax1On = cboSalesTx1.Text;
            }
            else
            {
                Code = "DEFAULT_SALES_TAX1CHECK";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Sales_Tax1Check = "0";
            }

            if (chkSalesTx2.Checked)
            {

                Code = "DEFAULT_SALES_TAX2";
                Value = cboSalesTx2.Text;
                m_Settings.SetSettings(Code, Value);

                //for checkbox
                Code = "DEFAULT_SALES_TAX2CHECK";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Sales_Tax2Check = "1";
                Global.Default_Sales_Tax2On = cboSalesTx2.Text;
            }
            else
            {
                Code = "DEFAULT_SALES_TAX2CHECK";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Sales_Tax2Check = "0";
            }
            if (chkSalesTx3.Checked)
            {
                Code = "DEFAULT_SALES_TAX3";
                Value = cboSalesTx3.Text;
                m_Settings.SetSettings(Code, Value);
                Code = "DEFAULT_SALES_TAX3CHECK";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Sales_Tax3Check = "1";
                Global.Default_Sales_Tax3On = cboSalesTx3.Text;
            }
            else
            {
                Code = "DEFAULT_SALES_TAX3CHECK";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Sales_Tax3Check = "0";
            }

            if (chkSalesVat.Checked)
            {
                Code = "DEFAULT_SALES_VAT";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Sales_Vat = "1";
            }
            else
            {
                Code = "DEFAULT_SALES_VAT";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Sales_Vat = "0";
            }


            //for purchase slabs

            if (chkPurchTx1.Checked)
            {
                Code = "DEFAULT_PURCHASE_TAX1";
                Value = cboPurchTx1.Text;
                m_Settings.SetSettings(Code, Value);
                Code = "DEFAULT_PURCHASE_TAX1CHECK";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Purchase_Tax1Check = "1";
                Global.Default_Purchase_Tax1On = cboPurchTx1.Text;
            }
            else
            {
                Code = "DEFAULT_PURCHASE_TAX1CHECK";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Purchase_Tax1Check = "0";
            }
            if (chkPurchTx2.Checked)
            {
                Code = "DEFAULT_PURCHASE_TAX2";
                Value = cboPurchTx2.Text;
                m_Settings.SetSettings(Code, Value);
                Code = "DEFAULT_PURCHASE_TAX2CHECK";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Purchase_Tax2Check = "1";
                Global.Default_Purchase_Tax2On = cboPurchTx2.Text;
            }
            else
            {
                Code = "DEFAULT_PURCHASE_TAX2CHECK";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Purchase_Tax1Check = "0";
            }
            if (chkPurchTx3.Checked)
            {
                Code = "DEFAULT_PURCHASE_TAX3";
                Value = cboPurchTx3.Text;
                m_Settings.SetSettings(Code, Value);
                Code = "DEFAULT_PURCHASE_TAX3CHECK";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Purchase_Tax3Check = "1";
                Global.Default_Purchase_Tax3On = cboPurchTx3.Text;
            }
            else
            {
                Code = "DEFAULT_PURCHASE_TAX3CHECK";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Purchase_Tax3Check = "0";

            }
            if (chkPurchVat.Checked)
            {
                Code = "DEFAULT_PURCHASE_VAT";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Purchase_Vat = "1";
            }
            else
            {
                Code = "DEFAULT_PURCHASE_VAT";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Purchase_Vat = "0";
            }
            if (chkcustomduty.Checked)
            {
                Code = "CUSTOMDUTY";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
              
            }
            else
            {
                Code = "CUSTOMDUTY";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                
            }
            #endregion


            isEmailChanged = false;
            Global.Msg("Settings Modified Sucessfully!!!");        
        
        }

        /// <summary>
        /// This is the method for setting the Value in tblSetting according to Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SETTING_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            ModifySettings();
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SETTING_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            ModifySettings();

        }

        private void frmSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {               
                FolderBrowserDialog FB = new FolderBrowserDialog();
                DialogResult result = FB.ShowDialog();
                if( result == DialogResult.OK )
                {
                    txtCustomBackUpPath.Text = FB.SelectedPath;                   
                }
            }

        private void chkAutoBackUp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoBackUp.Checked)
                grpAutoBackUp.Enabled = true;
            else
                grpAutoBackUp.Enabled = false;
        }

      
        private void chkSalesTx1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalesTx1.Checked)
            {
                cboSalesTx1.Enabled = true;
            }
            else
            {
                //cboSalesTx1.Enabled = false;
                if (chkSalesTx2.Checked && cboSalesTx2.Text == "Tax 1")
                {
                    MessageBox.Show("Sorry! Tax 1 is in use with Tax 2.");
                    chkSalesTx1.Checked = true;
                }

                else if (chkSalesTx3.Checked && cboSalesTx3.Text == "Tax 1")
                {
                    MessageBox.Show("Sorry! Tax 1 is in use with Tax 3.");
                    chkSalesTx1.Checked = true;
                }

                else
                    cboSalesTx1.Enabled = false;

            }
        }

        private void chkSalesTx2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalesTx2.Checked && cboSalesTx2.Text== "Tax 1" && chkSalesTx1.Checked==false )
            {
                //MessageBox.Show("Sorry! Tax 1 is not enabled .");
                cboSalesTx2.Enabled = false;
                chkSalesTx2.Checked = false;
            }
            else if (chkSalesTx2.Checked)
            {
                cboSalesTx2.Enabled = true;
            }
            else
            {
                // cboSalesTx2.Enabled = false;
                if (chkSalesTx3.Checked && cboSalesTx3.Text == "Tax 2")
                {
                    MessageBox.Show("Sorry! Tax 2 is in use with Tax 3 .");
                    chkSalesTx2.Checked = true;
                }
                else
                    cboSalesTx2.Enabled = false;

            }
        }

        private void chkSalesTx3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalesTx3.Checked && cboSalesTx3.Text== "Tax 1" && chkSalesTx1.Checked==false)
            {
                //MessageBox.Show("Sorry! Tax 1 is not enabled.");
                cboSalesTx3.Enabled = false;
                chkSalesTx3.Checked = false;
            }
            else if (chkSalesTx3.Checked && cboSalesTx3.Text == "Tax 2" && chkSalesTx2.Checked == false)
            {
                //MessageBox.Show("Sorry! Tax 2 is not enabled.");
                cboSalesTx3.Enabled = false;
                chkSalesTx3.Checked = false;
            }
            else if (chkSalesTx3.Checked)
            {
                cboSalesTx3.Enabled = true;
            }
            else
            {
                cboSalesTx3.Enabled = false;
            }
        }

        private void cboSalesTx2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboSalesTx2.SelectedItem.ToString() == "Tax 1")
            {
                if (chkSalesTx1.Checked == false)
                {
                    //MessageBox.Show("Sorry! Tax 1 is not enabled.");
                    cboSalesTx2.Text = cboSalesTax2;

                }
            }

        }

        private void cboSalesTx3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboSalesTx3.SelectedItem.ToString() == "Tax 1")
            {
                if (chkSalesTx1.Checked == false)
                {
                   // MessageBox.Show("Sorry! Tax 1 is not enabled.");
                    cboSalesTx3.Text = cboSalesTax3;

                }


            }

            if (cboSalesTx3.SelectedItem.ToString() == "Tax 2")
            {
                if (chkSalesTx2.Checked == false)
                {
                    //MessageBox.Show("Sorry! Tax 2 is not enabled.");
                    cboSalesTx3.Text = cboSalesTax3;

                }


            }

        }

        private void chkPurchTx1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPurchTx1.Checked)
            {
                cboPurchTx1.Enabled = true;
            }
            else
            {
                //cboSalesTx1.Enabled = false;
                if (chkPurchTx2.Checked && cboPurchTx2.Text == "Tax 1")
                {
                    MessageBox.Show("Sorry! Tax 1 is in use with Tax 2.");
                    chkPurchTx1.Checked = true;
                }

                else if (chkPurchTx3.Checked && cboPurchTx3.Text == "Tax 1")
                {
                    MessageBox.Show("Sorry! Tax 1 is in use with Tax 3.");
                    chkPurchTx1.Checked = true;
                }

                else
                    cboSalesTx1.Enabled = false;
            }

        }

        private void chkPurchTx2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPurchTx2.Checked && cboPurchTx2.Text == "Tax 1" && chkPurchTx1.Checked == false)
            {
                //MessageBox.Show("Sorry! Tax 1 is not enabled .");
                cboPurchTx2.Enabled = false;
                chkPurchTx2.Checked = false;
            }
            else if (chkPurchTx2.Checked)
            {
                cboPurchTx2.Enabled = true;
            }
            else
            {
                //cboPurchTx2.Enabled = false;
                if (chkPurchTx3.Checked && cboPurchTx3.Text == "Tax 2")
                {
                    MessageBox.Show("Sorry! Tax 2 is in use with Tax 3.");
                    chkPurchTx2.Checked = true;
                }
                else
                    cboPurchTx2.Enabled = false;
            }


        }

        private void chkPurchTx3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPurchTx3.Checked && cboPurchTx3.Text == "Tax 1" && chkPurchTx1.Checked == false)
            {
               // MessageBox.Show("Sorry! Tax 1 is not enabled.");
                cboPurchTx3.Enabled = false;
                chkPurchTx3.Checked = false;
            }
            else if (chkPurchTx3.Checked && cboPurchTx3.Text == "Tax 2" && chkPurchTx2.Checked == false)
            {
                //MessageBox.Show("Sorry! Tax 2 is not enabled.");
                cboPurchTx3.Enabled = false;
                chkPurchTx3.Checked = false;
            }
            else  if (chkPurchTx3.Checked)
            {
                cboPurchTx3.Enabled = true;
            }
            else
            {
                cboPurchTx3.Enabled = false;
            }
        }


        private void cboPurchTx2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboPurchTx2.SelectedItem.ToString() == "Tax 1")
            {
                if (chkPurchTx1.Checked == false)
                {
                    //MessageBox.Show("Sorry! Tax 1 is not enabled.");
                    cboPurchTx2.Text = cboPurchaseTax2;

                }
            }

        }

        private void cboPurchTx3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboPurchTx3.SelectedItem.ToString() == "Tax 1")
            {
                if (chkPurchTx1.Checked == false)
                {
                   // MessageBox.Show("Sorry! Tax 1 is not enabled.");
                    cboPurchTx3.Text = cboPurchaseTax3;

                }


            }

            if (cboPurchTx3.SelectedItem.ToString() == "Tax 2")
            {
                if (chkPurchTx2.Checked == false)
                {
                   // MessageBox.Show("Sorry! Tax 2 is not enabled.");
                    cboPurchTx3.Text = cboPurchaseTax3;

                }


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

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void btnregistry_Click(object sender, EventArgs e)
        {
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\", "", ""); //Tree
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift", "", ""); //Branch
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift", "ACTIVATION_CODE", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift", "DATABASE", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift", "PWD", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift", "SERVERNAME", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift", "USER", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\COMP001", "", ""); //SubBranch
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\COMP001", "CODE", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\COMP001", "DATABASE", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\COMP001", "NAME", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\GENERAL", "", ""); //SubBranch
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\GENERAL", "DATE_FORMAT", "YYYY_MM_DD", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\GENERAL", "DEFAULT_DATE", "NEPALI", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\COMP001\FY001", "", ""); //SubBranch
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\COMP001\FY001", "DATABASE", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\COMP001\FY001", "DAY", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\COMP001\FY001", "MONTH", " ", RegistryValueKind.String); //Branch's value
            //Registry.SetValue(@"HKEY_CURRENT_USER\Software\Bent Ray Test\AccSwift\COMP001\FY001", "YEAR", " ", RegistryValueKind.String); //Branch's value
          


        }
        bool isEmailChanged = false;
        private void txtuseremail_TextChanged(object sender, EventArgs e)
        {
            isEmailChanged = true;
        }

    
    }
}
