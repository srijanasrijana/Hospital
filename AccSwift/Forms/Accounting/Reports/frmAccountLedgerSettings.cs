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
using BusinessLogic.Accounting.Reports;
using Inventory.Forms.Accounting.Reports;
using Common;

namespace Inventory
{
    public partial class frmAccountLedgerSettings : Form, IfrmSelectAccClassID,IfrmDateConverter
    {
        private bool IsFromDate = false;
        ReportPreference m_ReportPreference;
        private string Prefix = "";
        ArrayList AccClassID = new ArrayList();
        public frmAccountLedgerSettings()
        {
            InitializeComponent();
        }

        //A function from the Interface IfrmAccClassID. Used to apply the Datatable to this form from AddAccClass Form

        public void AddSelectedAccClassID(DataTable AccClassID1)

        {
            try
            {
                AccClassID.Clear();
                ////If nothing is selected, simply send the root class id
                if (AccClassID1.Rows.Count == 0)
                {
                    AccClassID.Add("0");
                }

                else
                {
                    for (int i = 0; i < AccClassID1.Rows.Count; i++)
                    {
                        DataRow drAccClassID = AccClassID1.Rows[i];
                        AccClassID.Add(drAccClassID["AccClassID"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void frmAccountLedgerSettings_Load(object sender, EventArgs e)
        {
            m_ReportPreference = new ReportPreference();
            LoadComboboxProject(cboProjectName, 0);
            Global.CheckAcc = "";
            Global.acc = "L";
            LoadMonths();
            rbtnChooseLedger.Checked = true;
            rbtnChooseAccountGrp.Checked = false;
            rbtnSummary.Checked = true;
            rbtnDetails.Checked = false;
            cmbChooseAccountGroup.Enabled = false;

            grpDate.Enabled = false;// Disabling the DateRange Groupbox at FormLoad condition
            txtToDate.Mask = Date.FormatToMask();
            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
            txtFromDate.Mask = Date.FormatToMask();
            txtFromDate.Text = Date.ToSystem(Global.Fiscal_Year_Start);

            cmbChooseLedger.Items.Clear();//Clear the items in combobox which are already exists
            AccountGroup m_AccountGrp = new AccountGroup();//Dyanamic memory allocation of an object
            cmbChooseLedger.Items.Add(new ListItem(0, "All"));//For "All" item of combobox,pass 0 ID manually
            //For Showing the List of Ledger in combobox

            //Collecting Distinct LedgerID
            DataTable dtLedger = m_AccountGrp.GetDistinctLedgerID();
            foreach (DataRow drLedger in dtLedger.Rows)
            {
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drLedger["LedgerID"]), LangMgr.DefaultLanguage);
                foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                {
                    cmbChooseLedger.Items.Add(new ListItem((int)drLedger["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox               
                }
            }
          
            cmbChooseLedger.DisplayMember = "value";//This value is  for showing at Load condition
            cmbChooseLedger.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            cmbChooseLedger.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
            ////FOR ACCOUNT GROUP
            cmbChooseAccountGroup.Items.Clear();
            cmbChooseAccountGroup.Items.Add(new ListItem(0, "All"));//For "All" item of combobox,pass 0 ID manually
            
 #region Language Management
            cmbChooseAccountGroup.Font = LangMgr.GetFont();
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
            DataTable dt = AccountGroup.GetGroupTable(-1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                cmbChooseAccountGroup.Items.Add(new ListItem((int)dr["GroupID"], dr[LangField].ToString()));
            }
            cmbChooseAccountGroup.DisplayMember = "value";
            cmbChooseAccountGroup.ValueMember = "id";
            cmbChooseAccountGroup.SelectedIndex = 0;

          
           int checkuserid = User.CurrUserID;
            DataTable dtrpt = m_ReportPreference.GetPreferenceCount(checkuserid, "ACCOUNT_LEDGER");
            if (dtrpt.Rows.Count > 0)
            {
                foreach (DataRow dr in dtrpt.Rows)
                {
                    switch (dr["Code"].ToString())
                    {
                        case "AL_DATE":
                            txtToDate.Mask = Date.FormatToMask();
                            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
                            if (dr["Value"].ToString() == "1")
                            {
                                chkDate.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                chkDate.Checked = false;
                            }
                            break;
                        case "AL_ISACCOUNTLEDGER":
                            if (dr["Value"].ToString() == "True")
                            {
                                rbtnChooseLedger.Checked = true;
                                cmbChooseLedger.Enabled = true;
                                cmbChooseAccountGroup.Enabled = false;

                            }
                            else if (dr["Value"].ToString() == "False")
                            {
                                rbtnChooseAccountGrp.Checked = true;

                                cmbChooseLedger.Enabled = false;
                                cmbChooseAccountGroup.Enabled = true;
                            }
                            break;


                        case "AL_ACCOUNTING_CLASS":
                            AccClassID.Add(dr["Value"].ToString());
                            ArrayList arrchildAccClassIds = new ArrayList();
                            AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                            foreach (object obj in arrchildAccClassIds)
                            {
                                int i = (int)obj;
                                AccClassID.Add(i.ToString());
                            }

                            break;
                        case "AL_PROJECT":
                            int indexvalue = Convert.ToInt32(dr["Value"].ToString());
                            cboProjectName.SelectedIndex = indexvalue;
                            //cboProjectName.SelectedValue = indexvalue;

                            break;
                        case "AL_ACCOUNTLEDGER":
                            cmbChooseLedger.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;

                        case "AL_ACCOUNTGROUP":
                            cmbChooseAccountGroup.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;

                    }
                }
            }
            else
            {
                //If nothing is selected add Root class ID
                AccClassID.Add(Global.GlobalAccClassID.ToString());
                //just for test
                ArrayList arrchildAccClassIds = new ArrayList();
                AccountClass.GetChildIDs(Global.GlobalAccClassID, ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                foreach (object obj in arrchildAccClassIds)
                {
                    int i = (int)obj;
                    AccClassID.Add(i.ToString());
                }

            }

        }
        private void LoadMonths()
        {
            //Check Fiscal year(By default in English)
            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();

            //IF there are no companies created, simply return
            if (CompDetails == null)
            {
                return;
            }
            //get first month from start fiscal date 
            DateTime start = new DateTime();
            if (CompDetails.FYFrom != null)
            {
                start = Convert.ToDateTime(CompDetails.FYFrom); //English fiscal year
            }

            ListItem[] ListDate = new ListItem[12];
            for (int month = 0; month < 12; month++)
            {
                ListDate[month] = new ListItem();
                ListDate[month].ID = month + 1;
                ListDate[month].Value = Date.GetMonthList((Date.DateType)Date.DefaultDate, Language.LanguageType.English)[month + 1];

            }

            //   DateTime FYStartDate =  new DateTime();
            //if(CompDetails.FYFrom != null) 
            DateTime FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);

            //Convert Fiscal year to nepali
            int refYear = 0;
            int FYMonth = FYStartDate.Month;
            int refDay = 0;

            //If DateType is Nepali, load Nepali month
            if (Date.DefaultDate == Date.DateType.Nepali)
                Date.EngToNep(start, ref refYear, ref FYMonth, ref refDay);

            //Get the nepali fiscal year starting month
            int MonthCounter = FYMonth;
            do
            {
                if (MonthCounter > 12)
                    MonthCounter = 1;
                cboMonths.Items.Add(ListDate[MonthCounter - 1]);
                MonthCounter++;
            } while (MonthCounter != FYMonth);

        }           
        private void btnShow_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_LEDGER");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;

            }
            try
            {
                ListItem liProjectID = new ListItem();
                liProjectID = (ListItem)cboProjectName.SelectedItem;
                AccountLedgerSettings m_AccountLedger = new AccountLedgerSettings();
                ListItem liLedgerID = new ListItem();
                ListItem liGroupID = new ListItem();
                liLedgerID = (ListItem)cmbChooseLedger.SelectedItem;
                liGroupID = (ListItem)cmbChooseAccountGroup.SelectedItem;
                m_AccountLedger.LedgerID = liLedgerID.ID;//Pass the ledgerID of selected item of combobox
                m_AccountLedger.AccountGroupID = liGroupID.ID;
                m_AccountLedger.ChooseLedger = rbtnChooseLedger.Checked;
                m_AccountLedger.ChooseAccountGrp = rbtnChooseAccountGrp.Checked;
                m_AccountLedger.HasDateRange = chkDate.Checked;
                m_AccountLedger.FromDate = Date.ToDotNet(txtFromDate.Text);// Converting  datetime came via controls into DonNet datetime formate 
                m_AccountLedger.ToDate = Date.ToDotNet(txtToDate.Text);

                m_AccountLedger.ProjectID = Convert.ToInt32(liProjectID.ID);

                //Go to the transaction form directly when particular ledgerID is selected, this is for all transactions details
                TransactSettings m_transactSetting = new TransactSettings();
                m_transactSetting.HasDateRange = chkDate.Checked;
                m_transactSetting.FromDate = m_AccountLedger.FromDate;
                m_transactSetting.ToDate = m_AccountLedger.ToDate;
                m_transactSetting.LedgerID = liLedgerID.ID;
                m_transactSetting.AccountGroupID = liGroupID.ID;
                m_transactSetting.AccClassID = AccClassID;
                m_transactSetting.ProjectID = Convert.ToInt32(liProjectID.ID);

                //Go to the Stransaction form directly when particular ledgerID is selected this is for only summary of transaction
                TransactionSettingForSummmary m_transactSettings = new TransactionSettingForSummmary();
                m_transactSettings.HasDateRange = chkDate.Checked;
                m_transactSettings.FromDate = m_AccountLedger.FromDate;
                m_transactSettings.ToDate = m_AccountLedger.ToDate;
                m_transactSettings.LedgerID = liLedgerID.ID;
                m_transactSettings.AccountGroupID = liGroupID.ID;
                m_transactSettings.AccClassID = AccClassID;
                m_transactSettings.ProjectID = Convert.ToInt32(liProjectID.ID);

  #region  //For Details
                if (rbtnDetails.Checked == true)
                {


                    if (rbtnChooseLedger.Checked)
                    {
                        if (cmbChooseLedger.SelectedIndex != 0)
                        {
                            frmTransaction frm = new frmTransaction(m_transactSetting);
                            frm.Show();
                        }
                        else//This is 
                        {
                            frmTransaction frm = new frmTransaction(m_transactSetting, "ALL_LEDGERS");
                            frm.Show();
                        }
                    }

                    if (rbtnChooseAccountGrp.Checked)
                    {
                        if (cmbChooseAccountGroup.SelectedIndex != 0)
                        {
                            frmTransaction frm = new frmTransaction(m_transactSetting, "PARTICULAR_GROUP");
                            frm.Show();
                        }
                        else
                        {
                            frmTransaction frm = new frmTransaction(m_transactSetting, "ALL_LEDGERS");
                            frm.Show();
                        }
                    }

                }

       #endregion

 #region For summary

                else if (rbtnSummary.Checked == true)
                {

                    if (rbtnChooseLedger.Checked)
                    {
                        if (cmbChooseLedger.SelectedIndex != 0)
                        {
                            frmSTransaction frm = new frmSTransaction(m_transactSettings);
                            frm.Show();

                            
                        }
                        else 
                        {
                            frmSTransaction frm = new frmSTransaction(m_transactSettings, "ALL_LEDGERS");
                            frm.Show();
                        }
                    }

                    if (rbtnChooseAccountGrp.Checked)
                    {
                        if (cmbChooseAccountGroup.SelectedIndex != 0)
                        {
                            frmSTransaction frm = new frmSTransaction(m_transactSettings, "PARTICULAR_GROUP");
                            frm.Show();
                        }
                        else
                        {
                            frmSTransaction frm = new frmSTransaction(m_transactSettings, "ALL_LEDGERS");
                            frm.Show();
                        }
                    }



                }
 #endregion
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        
        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked == true)
            {
                grpDate.Enabled = true;// Enable groupbox if it is checked
            }
            else
            {
                grpDate.Enabled = false;// otherwise make it disable
            }
        }

        private void txtFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtToDate.Focus();
            }
        }

        //From date converter
        private void btnDate_Click(object sender, EventArgs e)
        {
            IsFromDate = true;//this variable is used as flag to notify which date is selected to change the date converter...coz same funtion is used to change the date  
            DateTime dtDate = Date.ToDotNet(txtFromDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

        //To date converter
        private void button1_Click(object sender, EventArgs e)
        {
            IsFromDate = false;
            DateTime dtDate = Date.ToDotNet(txtToDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

        public void DateConvert(DateTime DotNetDate)
        {
            if (IsFromDate)//If form date is selected
            {
                txtFromDate.Text = Date.ToSystem(DotNetDate);
            }
            else//IF TO date is selected
            {
                txtToDate.Text = Date.ToSystem(DotNetDate);
            }
        }

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {
            //just for test
            AccountLedgerSettings ALS = new AccountLedgerSettings();
            try
            {
                ALS.AccClassID = AccClassID;

            }
            catch
            {
                //Ignore 
            }
            frmSelectAccClass frm = new frmSelectAccClass(this, ALS.AccClassID);

            if (!frm.IsDisposed)
                frm.ShowDialog();
        }

        private void frmAccountLedgerSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnSelectAccClass_Click_1(object sender, EventArgs e)
        {
            //just for test
            DayBookSettings DBS = new DayBookSettings();
            try
            {
                DBS.AccClassID = AccClassID;
            }
            catch
            {
                //Ignore 
            }
            frmSelectAccClass frm = new frmSelectAccClass(this, DBS.AccClassID);

            if (!frm.IsDisposed)
                frm.ShowDialog();
        }

        private void btnsavestate_Click(object sender, EventArgs e)
        {
            int UserID;
            string ReadXMLDetails;
            UserID = User.CurrUserID;
            ReadXMLDetails = ChangeReportPreferences();
            string Result = BalanceSheet.RptPreferences(UserID, ReadXMLDetails);
            if (Result == "INSERT")
            {     
                Global.Msg("Report Preferences Inserted Sucessfully!!!");
            }

            else if (Result == "UPDATE")
            {
                Global.Msg("Report Preferences Updated Sucessfully!!!");
            }
            else if (Result == "FAILURE")
            {
                Global.Msg("Failed!!!");
            }
        }
        private string ChangeReportPreferences()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            tw.WriteStartDocument();
            #region  Report PreferenceDetail
            string Code, Value;
            tw.WriteStartElement("RPD");
            {
                Code = "AL_DATE";
                if (chkDate.Checked)
                {
                    Value = "1";
                }
                else
                {
                    Value = "0";
                }
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Accounting Class
                Code = "AL_ACCOUNTING_CLASS";
                Value = Global.GlobalAccClassID.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();

                Code = "AL_ACCOUNTLEDGER";
                Value = cmbChooseLedger.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();

                Code = "AL_ACCOUNTGROUP";
                Value = cmbChooseAccountGroup.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Project
                Code = "AL_PROJECT";
                Value = cboProjectName.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();


                Code = "AL_ISACCOUNTLEDGER";
                if (rbtnChooseLedger.Checked)
                {
                    Value = "True";
                }
                else
                {
                    Value = "False";
                }
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
               
            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            //MessageBox.Show(strXML);
            return strXML;
            //string Code, Value;
            //int PreferenceID, UserID;
            //UserID = User.CurrUserID;
            //DataTable dt = m_ReportPreference.GetPreferenceInfo(UserID, "ACCOUNT_LEDGER");
            //if (chkDate.Checked)
            //{
            //    Code = "AL_DATE";
            //    Value = "1";
            //    PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //    if (dt.Rows.Count < 1)
            //    {
            //        m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //    }
            //    else
            //    {
            //        m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //    }
            //}
            //else
            //{
            //    Code = "AL_DATE";
            //    Value = "0";
            //    PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //    if (dt.Rows.Count < 1)
            //    {
            //        m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //    }
            //    else
            //    {
            //        m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //    }
            //}

            ////For Accounting Class
            //Code = "AL_ACCOUNTING_CLASS";
            //Value = Global.GlobalAccClassID.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For PL Style
            //Code = "AL_ISACCOUNTLEDGER";
            //if (rbtnChooseLedger.Checked)
            //{
            //    Value = "True";
            //}
            //else
            //{
            //    Value = "False";
            //}
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Project
            //Code = "AL_PROJECT";
            //Value = cboProjectName.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Account Ledger
            //Code = "AL_ACCOUNTLEDGER";
            //Value = cmbChooseLedger.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Account Group
            //Code = "AL_ACCOUNTGROUP";
            //Value = cmbChooseAccountGroup.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}

            //if (dt.Rows.Count < 1)
            //{
            //    Global.Msg("Report Preferences Inserted Sucessfully!!!");
            //}
            //else
            //{
            //    Global.Msg("Report Preferences Modified Sucessfully!!!");
            //}
        }

        private void rbtnChooseLedger_Click(object sender, EventArgs e)
        {
            cmbChooseAccountGroup.Enabled = false;
            cmbChooseLedger.Enabled = true;
        }

        private void rbtnChooseAccountGrp_Click(object sender, EventArgs e)
        {
            cmbChooseLedger.Enabled = false;
            cmbChooseAccountGroup.Enabled = true;
        }

        private void cboMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem SelItem = (ListItem)cboMonths.SelectedItem;
                CompanyDetails CompDetails = new CompanyDetails();
                CompDetails = CompanyInfo.GetInfo();
                // DateTime FYStartDate = new DateTime();
                //if(CompDetails.FYFrom != null)
                DateTime FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);

                //Convert Fiscal year to nepali
                int FYYear = FYStartDate.Year;
                int FYMonth = FYStartDate.Month;
                int FYDay = FYStartDate.Day;

                //If DateType is Nepali, load Nepali month
                if (Date.DefaultDate == Date.DateType.Nepali)
                    Date.EngToNep(FYStartDate, ref FYYear, ref FYMonth, ref FYDay);
                //Get the nepali fiscal year starting month


                //If FYMonth is greater than selected month, then the year is next year
                if (FYMonth > SelItem.ID)
                    FYYear++;

                //If it was Nepali, set back to DateTime type
                DateTime FinalDate;
                DateTime StartDate;
                if (Date.DefaultDate == Date.DateType.Nepali)
                {
                    //Get First Day
                    StartDate = Date.NepToEng(FYYear, SelItem.ID, 1);
                    //Get Last Day
                    DataTable LastDay = Date.LastDayofMonthNep(FYYear, SelItem.ID);
                    FinalDate = Date.NepToEng(FYYear, SelItem.ID, Convert.ToInt16(LastDay.Rows[0][0]));
                }
                else
                {
                    StartDate = Date.NepToEng(FYYear, SelItem.ID, 1);
                    FinalDate = new DateTime(FYYear, SelItem.ID, DateTime.DaysInMonth(FYYear, SelItem.ID));
                }
                txtFromDate.Text = Date.ToSystem(StartDate);
                txtToDate.Text = Date.ToSystem(FinalDate);
            }
            catch
            {
                //Ignore
            }
        }

     
    }
}
