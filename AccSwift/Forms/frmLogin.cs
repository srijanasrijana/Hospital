using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BusinessLogic;
using DateManager;
using BusinessLogic.Accounting;

namespace AccSwift
{
    public partial class frmLogin : Form
    {
        private MDIMain m_MDI;

        public frmLogin()
        {
            InitializeComponent();
        }
        public frmLogin(MDIMain MDI)
        {
            InitializeComponent();
            m_MDI = MDI;
           // MessageBox.Show("Fourth Message");
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validation
                if (cboUserName.Text.Length > 0 && txtPassword.Text.Length == 0)
                {
                    txtPassword.Focus();
                    return;
                }

                if (cboUserName.Text.Length < 1 && txtPassword.Text.Length < 1)
                {
                    MessageBox.Show("Please enter proper Username and Password!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }
                #endregion
                string EncryptedPassword = Cryptography.Crypto.Encrypt(txtPassword.Text, "Ac104");
                DataTable dtUserinfo = User.CheckUserAvailability(cboUserName.Text, EncryptedPassword);
                if (dtUserinfo.Rows.Count > 0)
                {   //Login Successful
                    User.CurrUserID = Convert.ToInt32(dtUserinfo.Rows[0]["UserID"]);
                    User.CurrentUserName = (dtUserinfo.Rows[0]["UserName"]).ToString();

                    //Saving username for remember username checkbox to inventory.properties.settings
                    if (chkRemember.Checked)
                    {
                        Inventory.Properties.Settings.Default.UserName = cboUserName.Text;
                        Inventory.Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Inventory.Properties.Settings.Default.UserName = string.Empty;
                        Inventory.Properties.Settings.Default.Save();
                    }

                    this.Close();
                    //Message is the user is Disable
                    DataTable dtUser = User.CheckUserStatus(User.CurrUserID);
                    DataRow druser = dtUser.Rows[0];
                    if (druser["UserStatus"].ToString() == "0")
                    {
                        MessageBox.Show("The Selected User Has Been Disabled Please Contact Administrator!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //Enable all menus
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;

                    ///Load Settings here
                    #region passparameter after successful login
                    User.CurrentUserName = User.GetCurrUser();

                    //public static double VAT = 13;//Convert.ToDouble(drSlabInfo["Rate"]);
                    //public static int VATLedgerID =10; //AccountGroup.GetLedgerIDFromLedgerNumber(10);      
                    Global.Default_Date = Date.DateType.Nepali;
                    Global.Default_Formate = Date.DateFormat.YYYY_MM_DD;
                    Global.Default_Language = Lang.English;//LangMgr.LangToLangType(Settings.GetSettings("DEFAULT_LANGUAGE"));

                    Global.Default_Formate = (Date.DateFormat)Enum.Parse(typeof(Date.DateFormat), Settings.GetSettings("DATE_FORMAT").Replace('/', '_'), true);

                    Global.Default_NegativeCash = Settings.NegativeCashToNegativeCashType(Settings.GetSettings("DEFAULT_NEGATIVECASH"));
                    //Global.Default_NegativeStock = 
                    Global.Default_NegativeStock = (NegativeStock)Enum.Parse(typeof(NegativeStock), Settings.GetSettings("DEFAULT_NEGATIVESTOCK"), true);
                    //if (negStock == "Allow") Global.Default_NegativeStock = NegativeStock.Allow;
                    //else if (negStock == "Warn") Global.Default_NegativeStock = NegativeStock.Warn;
                    //else if (negStock == "Deny") Global.Default_NegativeStock = NegativeStock.Deny;

                    Global.Default_BudgetLimit = Settings.BudgetLimitAction(Settings.GetSettings("DEFAULT_BUDGET_LIMIT"));

                    Global.Default_NegativeBank = Settings.NegativeBankToNegativeBankType(Settings.GetSettings("DEFAULT_NEGATIVEBANK"));
                    Global.VAT_Settings = Settings.GetSettings("VAT");
                    
                    Global.Fiscal_English_Year = "11/12";
                    Global.Fiscal_Nepali_Year = "067/068";
                    Global.Fiscal_Year_Start = Convert.ToDateTime("2012-12-18");
                    Global.Fiscal_Year_End = Convert.ToDateTime("2013-12-06");

                    Global.DecimalPlaces = 2;
                    // Global.Comma_Separated = Settings.GetSettings("COMMA_SEPARATED");
                    Global.Decimal_Format = Settings.GetSettings("DECIMAL_FORMAT");
                    Global.Multi_Currency = Settings.GetSettings("MULTI_CURRENCY");
                    Global.Default_Cash_Account = Settings.GetSettings("DEFAULT_CASH_ACCOUNT");
                    Global.Default_Bank_Account = Settings.GetSettings("DEFAULT_BANK_ACCOUNT");
                    //Global.Credit_Limit ==Settings.CreditLimitFunction(Settings.GetSettings("CREDIT_LIMIT"));
                    Global.Auto_BackUp = Settings.GetSettings("AUTO_BACKUP");
                    Global.Backup_Interval_Day = Settings.GetSettings("BACKUP_INTERVAL_DAY");
                    Global.Backup_Path = Settings.GetSettings("BACKUP_PATH");
                    Global.Prepared_By = Settings.GetSettings("PREPARED_BY");
                    Global.Checked_By = Settings.GetSettings("CHECKED_BY");
                    Global.Approved_By = Settings.GetSettings("APPROVED_BY");

                    //DataTable dtSelectedReminder = Reminder.GetReminderIfExistToday(User.CurrUserID, 1);
                    //if (dtSelectedReminder.Rows.Count > 0)
                    //{
                    //    frmReminderList frm = new frmReminderList();
                    //    frm.ShowDialog();
                    //}


                    try
                    {
                        DataTable dt = ChequeReceipt.GetChequeCashDetails();
                        if (dt.Rows.Count > 0)
                        {
                            Forms.frmChequeCashReminder chequecash = new Forms.frmChequeCashReminder();
                            chequecash.ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {

                        Global.MsgError(ex.Message);
                    }


                    #endregion
                }
                else
                {
                    MessageBox.Show("Invalid Username and/or Password!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Text = string.Empty;
                    txtPassword.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = (chkShowPassword.Checked ? Convert.ToChar(0) : '*');
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            #region Remember the username

            if(Inventory.Properties.Settings.Default.UserName != string.Empty)
            {
                chkRemember.Checked = true;

                cboUserName.Text = Inventory.Properties.Settings.Default.UserName;
                txtPassword.Select();
            }
            #endregion
            //#region Language Maintenance
        //    try
        //    {
        //        this.Font = LangMgr.GetFont(); //Set font to whole form
        //        lblUserName.Text = LangMgr.Translate("USER_NAME", LangMgr.Language);
        //        lblPassword.Text = LangMgr.Translate("PASSWORD", LangMgr.Language);
        //        btnLogin.Text = LangMgr.Translate("LOGIN", LangMgr.Language);
        //        btnLogin.Font = LangMgr.GetFont();
        //        btnCancel.Text = LangMgr.Translate("CANCEL", LangMgr.Language);
        //        btnCancel.Font = LangMgr.GetFont();

        //        chkRemember.Text = LangMgr.Translate("REMEMBER_USERNAME", LangMgr.Language);
        //        chkShowPassword.Text = LangMgr.Translate("SHOW_PASSWORD", LangMgr.Language);
        //    }
        //    catch (Exception ex)
        //    {
                
        //        Global.MsgError(ex.Message);
        //    }
        //#endregion

           // cboUserName.Items.Clear();
            try
            {
                //DBFunc db = new DBFunc();
                //db.cmdCommand.CommandText = "SELECT UserName FROM System.tblLogin WHERE Remember='1'";
                //SqlDataReader rdrUser = db.cmdCommand.ExecuteReader();
                //while (rdrUser.Read())
                //{
                //    cboUserName.Items.Add(rdrUser["UserName"].ToString());
                //}
            }
            catch (Exception ex)
            {

            }

        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }



      

    }
}