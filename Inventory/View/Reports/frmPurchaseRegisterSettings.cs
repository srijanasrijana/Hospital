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
using Common;
using Inventory.View.Reports;

namespace Inventory
{
    public partial class frmPurchaseRegisterSettings : Form,IfrmSelectAccClassID,IfrmDateConverter
    {
        ReportPreference m_ReportPreference;
        ArrayList AccClassID = new ArrayList();
        private string Prefix = "";
        private bool IsFromDate = false;
              public frmPurchaseRegisterSettings()
        {
            InitializeComponent();
        }
      
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

        private void btnShow_Click(object sender, EventArgs e)
        {
            InventoryBookSettings m_PRS = new InventoryBookSettings();
            ListItem LiPartyGroupID = new ListItem();
            LiPartyGroupID = (ListItem)cboPartyGroup.SelectedItem;
            ListItem liPartySingleID = new ListItem();
            ListItem liPartyID = new ListItem();
            liPartyID = (ListItem)cboPartySingle.SelectedItem;

            //for party group
            if (rdPartyGroup.Checked)
            {
                m_PRS.partyGroupID = Convert.ToInt32(LiPartyGroupID.ID);
            }
            else
            {
                m_PRS.partyGroupID = null;
            }
            //for single party
            if (rdpartySingle.Checked)
            {
                m_PRS.PartyID = Convert.ToInt32(liPartyID.ID);
            }
            else
            {
                m_PRS.PartyID = null;
            }
            //for date range
            if (chkDateRange.Checked)
            {
                txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate
                txtToDate.Mask = Date.FormatToMask();
                m_PRS.FromDate = Date.ToDotNet(txtFromDate.Text);
                m_PRS.ToDate = Date.ToDotNet(txtToDate.Text);
            }
            else
            {
                m_PRS.FromDate = null;
                m_PRS.ToDate = null;
            }

            //for Depot
            ListItem LiDepotID = new ListItem();
            LiDepotID = (ListItem)cboDepotwise.SelectedItem;
            if (chkDepot.Checked)
            {
                m_PRS.DepotID = Convert.ToInt32(LiDepotID.ID);
            }
            else
            {
                m_PRS.DepotID = null;
            }

            //for Project 
            ListItem LiProjectID = new ListItem();
            LiProjectID = (ListItem)cboProjectName.SelectedItem;
            if (chkProject.Checked)
            {
                m_PRS.ProjectID = Convert.ToInt32(LiProjectID.ID);
            }
            else
            {
                m_PRS.ProjectID = null;
            }
            m_PRS.AccClassID = AccClassID;
            m_PRS.AccClassID = AccClassID;
            if (m_PRS.PartyID > 0)
            {
                frmPurchaseRegisterPatyWise frms = new frmPurchaseRegisterPatyWise(m_PRS);
                frms.Show();
            }
            else
            {
                frmPurchaseRegister frm = new frmPurchaseRegister(m_PRS);
                frm.Show();
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
            //ComboBoxControl.Items.Add(new ListItem((0), "All"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], dr[LangField].ToString()));
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }

        private void frmPurchaseRegisterSettings_Load(object sender, EventArgs e)
        {
            LoadMonths();
            m_ReportPreference = new ReportPreference();
            groupBox2.Enabled = false;
               
           // ListProject(cboProjectName);
           // ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            txtToDate.Mask = Date.FormatToMask();
            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
            txtFromDate.Mask = Date.FormatToMask();
            txtFromDate.Text = Date.ToSystem(Global.Fiscal_Year_Start);

            rdPartyAll.Checked = true;
            cboPartyGroup.Enabled = false;
            cboPartySingle.Enabled = false;
            cboProjectName.Enabled = false;
            cboDepotwise.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;

            //Block for displaying the Ledgers of Cash/Party Account in combobox
            int Cash_In_Hand = 102;//GroupID of Cash_In_Hand is 102
            DataTable dtCash_In_HandLedgers = Ledger.GetAllLedger(Cash_In_Hand);//Collecting the Ledgers corresponding to Cash_In_Hand group
            if (dtCash_In_HandLedgers.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCash_In_HandLedgers.Rows.Count; i++)
                {
                    DataRow drCash_In_HandLedgers = dtCash_In_HandLedgers.Rows[i - 1];
                    cboPartySingle.Items.Add(new ListItem((int)drCash_In_HandLedgers["LedgerID"], drCash_In_HandLedgers["EngName"].ToString()));
                }
            }
            int Debtor = 29;//GroupID of Debtor is 29
            DataTable dtDebtorLedgers = Ledger.GetAllLedger(Debtor);
            if (dtDebtorLedgers.Rows.Count > 0)
            {
                for (int i = 1; i <= dtDebtorLedgers.Rows.Count; i++)
                {
                    DataRow drDebtorLedgers = dtDebtorLedgers.Rows[i - 1];
                    cboPartySingle.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drDebtorLedgers["EngName"].ToString()));
                }
            }
            int Creditor = 114;
            DataTable dtCreditorLedgers = Ledger.GetAllLedger(Creditor);
            if (dtCreditorLedgers.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCreditorLedgers.Rows.Count; i++)
                {
                    DataRow drCreditorLedgers = dtCreditorLedgers.Rows[i - 1];
                    cboPartySingle.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drCreditorLedgers["EngName"].ToString()));
                }
            }
            cboPartySingle.DisplayMember = "value";//This value is  for showing at Load condition
            cboPartySingle.ValueMember = "id";//This value is stored only not to be shown at Load condition
            cboPartySingle.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox

            //Display the Cash/Party Group in load condition
            //In Cash/Party Group is the collection of Debtor,Creditor and CashInHand

            //For Cash_In_Hand Account Group which falls under Cash/Party Account Group
            DataTable dtCashInHandID = AccountGroup.GetGroupByID(102, LangMgr.DefaultLanguage);
            DataRow drCashInHandID = dtCashInHandID.Rows[0];
            DataTable dtCashInHandInfo = AccountGroup.GetGroupTable(Convert.ToInt32(drCashInHandID["ID"]));
            cboPartyGroup.Items.Add(new ListItem((int)drCashInHandID["ID"], drCashInHandID["Name"].ToString()));
            foreach (DataRow drCashInHandInfo in dtCashInHandInfo.Rows)
            {
                cboPartyGroup.Items.Add(new ListItem((int)drCashInHandInfo["GroupID"], drCashInHandInfo["EngName"].ToString()));
            }

            //For Debtor Account Group which falls under Cash/Party Account Group
            DataTable dtDebtorID = AccountGroup.GetGroupByID(29, LangMgr.DefaultLanguage);
            DataRow drDebtorID = dtDebtorID.Rows[0];
            cboPartyGroup.Items.Add(new ListItem((int)drDebtorID["ID"], drDebtorID["Name"].ToString()));
            DataTable dtDebtorInfo = AccountGroup.GetGroupTable(Convert.ToInt32(drDebtorID["ID"]));

            foreach (DataRow drDebtorInfo in dtDebtorInfo.Rows)
            {
                cboPartyGroup.Items.Add(new ListItem((int)drDebtorInfo["GroupID"], drDebtorInfo["EngName"].ToString()));
            }
            //For Creditor Account Group which falls under Cash/Party Account Group            
            DataTable dtCreditorID = AccountGroup.GetGroupByID(114, LangMgr.DefaultLanguage);
            DataRow drCreditID = dtCreditorID.Rows[0];
            cboPartyGroup.Items.Add(new ListItem((int)drCreditID["ID"], drCreditID["Name"].ToString()));
            DataTable dtCreditorInfo = AccountGroup.GetGroupTable(Convert.ToInt32(drCreditID["ID"]));
            foreach (DataRow drCreditorInfo in dtCreditorInfo.Rows)
            {
                cboPartyGroup.Items.Add(new ListItem((int)drCreditorInfo["GroupID"], drCreditorInfo["EngName"].ToString()));
            }
            cboPartyGroup.DisplayMember = "value";//This value is  for showing at Load condition
            cboPartyGroup.ValueMember = "id";//This value is stored only not to be shown at Load condition
            cboPartyGroup.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox

            //for depot
            #region BLOCK OF SHOWING DEPOT IN COMBOBOX
            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            foreach (DataRow dr in dtDepotInfo.Rows)
            {

                cboDepotwise.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboDepotwise.SelectedIndex = 0;
            #endregion
            int checkuserid = User.CurrUserID;
            DataTable dtrpt = m_ReportPreference.GetPreferenceCount(checkuserid, "PRUCHASE_REGISTER");
            if (dtrpt.Rows.Count > 0)
            {
                foreach (DataRow dr in dtrpt.Rows)
                {
                    switch (dr["Code"].ToString())
                    {

                        case "PRS_ACCOUNTING_CLASS":
                            AccClassID.Add(dr["Value"].ToString());
                            ArrayList arrchildAccClassIds = new ArrayList();
                            AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                            foreach (object obj in arrchildAccClassIds)
                            {
                                int i = (int)obj;
                                AccClassID.Add(i.ToString());
                            }

                            break;
                        case "PRS_PROJECT":
                            cboProjectName.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PRS_DEPOT":
                            cboDepotwise.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PRS_SINGLEPARTY":
                            cboPartySingle.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PRS_PARTYGROUP":
                            cboPartyGroup.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PRS_RALLPARTY":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdPartyAll.Checked = true;
                            }
                            else
                            {
                                rdPartyAll.Checked = false;
                            }
                            break;
                        case "PRS_RSINGLEPARTY":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdpartySingle.Checked = true;
                            }
                            else
                            {
                                rdpartySingle.Checked = false;
                            }
                            break;
                        case "PRS_RPARTYGROUP":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdPartyGroup.Checked = true;
                            }
                            else
                            {
                                rdPartyGroup.Checked = false;
                            }
                            break;
                        case "PRS_CDEPOT":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkDepot.Checked = true;

                            }
                            else
                            {
                                chkDepot.Checked = false;
                            }
                            break;
                        case "PRS_CPROJECT":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkProject.Checked = true;
                            }
                            else
                            {
                                chkProject.Checked = false;
                            }
                            break;

                        case "PRS_DATE":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkDateRange.Checked = true;
                            }
                            else
                            {
                                chkDateRange.Checked = false;
                            }
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

        private void rdpartySingle_CheckedChanged(object sender, EventArgs e)
        {
            if (rdpartySingle.Checked)
            {
                cboPartySingle.Enabled = true;
            }
            else
            {
                cboPartySingle.Enabled = false;
            }
        }

        private void rdPartyGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPartyGroup.Checked)
            {
                cboPartyGroup.Enabled = true;
            }
            else
            {
                cboPartyGroup.Enabled = false;
            }
        }

        private void chkDepot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepot.Checked)
            {
                cboDepotwise.Enabled = true;
            }
            else
            {
                cboDepotwise.Enabled = false;
            }
        }

        private void chkProject_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProject.Checked)
            {
                cboProjectName.Enabled = true;
            }
            else
            {
                cboProjectName.Enabled = false;
            }
        }

        private void chkDateRange_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDateRange.Checked)
            {
                groupBox2.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                btnFromDate.Enabled = true;
                btnToDate.Enabled = true;


            }
            else
            {
                groupBox2.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                btnFromDate.Enabled = false;
                btnToDate.Enabled = false;
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
            Common.frmSelectAccClass frm = new Common.frmSelectAccClass(this, ALS.AccClassID);
            if (!frm.IsDisposed)
                frm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //From date converter
        private void btnFromDate_Click(object sender, EventArgs e)
        {
            IsFromDate = true;//this variable is used as flag to notify which date is selected to change the date converter...coz same funtion is used to change the date  
            DateTime dtDate = Date.ToDotNet(txtFromDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

        //TO date converter
        private void btnToDate_Click(object sender, EventArgs e)
        {
            IsFromDate = false;
            DateTime dtDate = Date.ToDotNet(txtToDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
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

        private void btnsavestate_Click(object sender, EventArgs e)
        {
            ChangeReportPreferences();
        }
        private void ChangeReportPreferences()
        {
            string Code, Value;
            int PreferenceID, UserID;
            UserID = User.CurrUserID;
            DataTable dt = m_ReportPreference.GetPreferenceInfo(UserID, "PRUCHASE_REGISTER");

            //For Accounting Class
            Code = "PRS_ACCOUNTING_CLASS";
            Value = Global.GlobalAccClassID.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For Project
            Code = "PRS_PROJECT";
            Value = cboProjectName.SelectedIndex.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For Depot
            Code = "PRS_DEPOT";
            Value = cboDepotwise.SelectedIndex.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For Single Party
            Code = "PRS_SINGLEPARTY";
            Value = cboPartySingle.SelectedIndex.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For Party Group
            Code = "PRS_PARTYGROUP";
            Value = cboPartyGroup.SelectedIndex.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For R all party
            Code = "PRS_RALLPARTY";
            if (rdPartyAll.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }

            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For  rsingle party
            Code = "PRS_RSINGLEPARTY";
            if (rdpartySingle.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For r product Party
            Code = "PRS_RPARTYGROUP";
            if (rdPartyGroup.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }

            //For c depot
            Code = "PRS_CDEPOT";
            if (chkDepot.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }

            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For c project
            Code = "PRS_CPROJECT";
            if (chkProject.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }

            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For c date
            Code = "PRS_DATE";
            if (chkDateRange.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }

            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            if (dt.Rows.Count < 1)
            {
                Global.Msg("Report Preferences Inserted Sucessfully!!!");
            }
            else
            {
                Global.Msg("Report Preferences Modified Sucessfully!!!");
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
            //do
            //{
            //    if (MonthCounter > 12)
            //        MonthCounter = 1;
            //    cboMonths.Items.Add(ListDate[MonthCounter - 1]);
            //    MonthCounter++;
            //} while (MonthCounter != FYMonth);

            // new code 
            for (int i = 0; i < 12; i++)
            {
                cboMonths.Items.Add(ListDate[MonthCounter - 1]);
                if (MonthCounter >= 12)
                    MonthCounter = 1;
                else
                    MonthCounter++;
            }
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
