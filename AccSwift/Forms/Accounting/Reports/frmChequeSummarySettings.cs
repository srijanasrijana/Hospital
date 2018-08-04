using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DateManager;
using BusinessLogic;
using System.Collections;
using Common;

namespace Inventory
{
    public partial class frmChequeSummarySettings : Form, IfrmDateConverter, IfrmSelectAccClassID
    {
        private bool isFromDate = false;
        ReportPreference m_ReportPreference;
        private string Prefix = "";
        ArrayList AccClassID = new ArrayList();
        public frmChequeSummarySettings()
        {
            InitializeComponent();
        }

        public void DateConvert(DateTime DotNetDate)
        {
            if (isFromDate)
                txtFromDate.Text = Date.ToSystem(DotNetDate);
            if (!isFromDate)
                txtToDate.Text = Date.ToSystem(DotNetDate);
        }

        private void frmChequeSummarySettings_Load(object sender, EventArgs e)
        {
            m_ReportPreference = new ReportPreference();
            LoadComboboxProject(cboProjectName, 0);
          // cboBanks.SelectedIndex = 0;
            LoadMonths();
            int checkuserid = User.CurrUserID;
            DataTable dtrpt = m_ReportPreference.GetPreferenceCount(checkuserid, "CHEQUE_REPORT");
            if (dtrpt.Rows.Count > 0)
            {
                foreach (DataRow dr in dtrpt.Rows)
                {
                    switch (dr["Code"].ToString())
                    {

                        case "CR_ACCOUNTING_CLASS":
                            AccClassID.Add(dr["Value"].ToString());
                            ArrayList arrchildAccClassIds = new ArrayList();
                            AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                            foreach (object obj in arrchildAccClassIds)
                            {
                                int i = (int)obj;
                                AccClassID.Add(i.ToString());
                            }

                            break;
                        case "CR_PROJECT":
                            cboProjectName.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;

                        //case "CR_BANK":
                        //    cboBanks.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                        //    break;

                        //case "CR_PARTY":
                        //    cboParty.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                        //    break;

                        case "CR_ALL":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdbChequeAll.Checked = true;
                            }
                            else
                            {
                                rdbChequeAll.Checked = false;
                            }
                            break;
                        case "CR_GIVEN":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdbChequeGiven.Checked = true;
                            }
                            else
                            {
                                rdbChequeGiven.Checked = false;
                            }
                            break;
                        case "CR_RECEIVED":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdbChequeReceived.Checked = true;
                            }
                            else 
                            {
                                rdbChequeReceived.Checked = false;
                            }
                            break;
                        case "CR_BOTH":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdbBoth.Checked = true;
                                txtFromDate.Mask = DateManager.Date.FormatToMask();
                                txtFromDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                                txtToDate.Mask = Date.FormatToMask();
                                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                            }
                            else
                            {
                                rdbBoth.Checked = false;
                            }
                            break;
                        case "CR_CLEARED":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdbCleared.Checked = true;
                            }
                            else
                            {
                                rdbCleared.Checked = false;
                            }
                            break;
                        case "CR_UNCLEARED":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdbUnCleared.Checked = true;
                            }
                            else
                            {
                                rdbUnCleared.Checked = false;
                            }
                            break;
                        case "CR_DATE":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkDateRange.Checked = true;
                            }
                            else
                            {
                                chkDateRange.Checked = false;
                            }
                            break;
                        case "CR_CBANK":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkBank.Checked = true;
                            }
                            else
                            {
                                chkBank.Checked = false;
                            }
                            break;
                        case "CR_CPARTY":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkParty.Checked = true;
                            }
                            else
                            {
                                chkParty.Checked = false;
                            }
                            break;
                    }
                }
            }
            else
            {
                txtFromDate.Mask = DateManager.Date.FormatToMask();
                txtFromDate.Text = Date.ToSystem(Global.Fiscal_Year_Start);
                txtToDate.Mask = Date.FormatToMask();
                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                rdbChequeAll.Checked = true;
                rdbBoth.Checked = true;
                //If nothing is selected add Root class ID
                AccClassID.Add(Global.GlobalAccClassID.ToString());
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkBank_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBank.Checked)
            {
                cboBanks.Enabled = true;
                LoadcboBanks(cboBanks);
            }
            if (!chkBank.Checked)
            {
                cboBanks.Enabled = false;
                cboBanks.Text = "";
                cboBanks.Items.Clear();
            }
        }

        private void LoadcboBanks(ComboBox cboBanks)
        {
            try
            {
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                foreach (DataRow drBankLedgers in dtBankLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboBanks.Items.Add(new ListItem((int)drBankLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cboBanks.DisplayMember = "value";//This value is  for showing at Load condition
                cboBanks.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                foreach (ListItem lst in cboBanks.Items)
                {
                    if (lst.ID == Convert.ToInt32(Settings.GetSettings("DEFAULT_BANK_ACCOUNT")))
                    {
                        cboBanks.Text = lst.Value;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadcboParty(ComboBox cboParty)
        {
            try
            {
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                cboParty.Items.Add(new ListItem(0, "All"));
                int CreditorID = AccountGroup.GetGroupIDFromGroupNumber(114); //****************************** id must be global
                DataTable dtCreditorLedgers = Ledger.GetAllLedger(CreditorID);
                foreach (DataRow drCreditorLedgers in dtCreditorLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCreditorLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboParty.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }

                int DebtorID = AccountGroup.GetGroupIDFromGroupNumber(29);
                DataTable dtDebtorLedgers = Ledger.GetAllLedger(DebtorID);
                foreach (DataRow drDebtorLedgers in dtDebtorLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drDebtorLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboParty.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cboParty.SelectedIndex = 0;
                cboParty.DisplayMember = "value";//This value is  for showing at Load condition
                cboParty.ValueMember = "id";//This value is stored only not to be shown at Load condition                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chkParty_CheckedChanged(object sender, EventArgs e)
        {
            if (chkParty.Checked)
            {
                cboParty.Enabled = true;
                LoadcboParty(cboParty);
            }
            if (!chkParty.Checked)
            {
                cboParty.Enabled = false;
                cboParty.Text = "";
                cboParty.Items.Clear();
            }
        }

        private void btnFromDate_Click(object sender, EventArgs e)
        {
            isFromDate = true;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtFromDate.Text));
            frm.ShowDialog();
        }

        private void btnToDate_Click(object sender, EventArgs e)
        {
            isFromDate = false;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtToDate.Text));
            frm.ShowDialog();
        }

        private void chkDateRange_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDateRange.Checked)
            {
                txtFromDate.Enabled = true;
                btnFromDate.Enabled = true;
                txtToDate.Enabled = true;
                btnToDate.Enabled = true;
            }
            if (!chkDateRange.Checked)
            {
                txtFromDate.Enabled = false;
                btnFromDate.Enabled = false;
                txtToDate.Enabled = false;
                btnToDate.Enabled = false;
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("CHEQUE_REPORT");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
            //    return;
            //}
            ChequeRegisterSettings m_ChequeShow = new ChequeRegisterSettings();//Dynamic memory allocation of an object   
            if (rdbChequeAll.Checked)
                m_ChequeShow.ChequeReceived = 0;
            if (rdbChequeGiven.Checked)
                m_ChequeShow.ChequeReceived = 1;
            if (rdbChequeReceived.Checked)
                m_ChequeShow.ChequeReceived = 2;
            //m_ChequeShow.ChequeReceived = rdbChequeReceived.Checked;

            if (rdbBoth.Checked)
                m_ChequeShow.Status = ChequeStatus.Both;
            if (rdbCleared.Checked)
                m_ChequeShow.Status = ChequeStatus.Cleared;
            if (rdbUnCleared.Checked)
                m_ChequeShow.Status = ChequeStatus.Uncleared;

            if (chkDateRange.Checked)
            {
                m_ChequeShow.HasDateRange = chkDateRange.Checked;
                m_ChequeShow.FromDate = Date.ToDotNet(txtFromDate.Text);
                m_ChequeShow.ToDate = Date.ToDotNet(txtToDate.Text);
            }
            if (!chkDateRange.Checked)
            {
                m_ChequeShow.HasDateRange = chkDateRange.Checked;
                m_ChequeShow.FromDate = null;
                m_ChequeShow.ToDate = null;
            }

            if (chkBank.Checked)
            {
                m_ChequeShow.HasBank = chkBank.Checked;
                ListItem liBankID = new ListItem();
                liBankID = (ListItem)cboBanks.SelectedItem;
                m_ChequeShow.BankID = Convert.ToInt32(liBankID.ID);
            }
            if (chkParty.Checked)
            {
                m_ChequeShow.HasParty = chkParty.Checked;
                ListItem liPartyID = new ListItem();
                liPartyID = (ListItem)cboParty.SelectedItem;
                m_ChequeShow.PartyID = Convert.ToInt32(liPartyID.ID);
            }
            m_ChequeShow.AccClassID = AccClassID;
            ListItem liProjectID = new ListItem();
            liProjectID = (ListItem)cboProjectName.SelectedItem;
            m_ChequeShow.ProjectID = liProjectID.ID;
            frmChequeReport frmShow = new frmChequeReport(m_ChequeShow);//Passing object as an argument            
            // frmShow.MdiParent = this.MdiParent;
            frmShow.ShowDialog();  
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

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {
            ////just for test
            //ChequeRegisterSettings CRS = new ChequeRegisterSettings();
            //try
            //{
            //    CRS.AccClassID = AccClassID;

            //}
            //catch
            //{
            //    //Ignore 
            //}
            //frmSelectAccClass frm = new frmSelectAccClass(this, CRS.AccClassID);

            //if (!frm.IsDisposed)
            //    frm.ShowDialog();
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
                //For Accounting Class
                Code = "CR_ACCOUNTING_CLASS";
                Value = Global.GlobalAccClassID.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Project
                Code = "CR_PROJECT";
                Value = cboProjectName.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For All
                Code = "CR_ALL";
                if (rdbChequeAll.Checked)
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
                //For given
                Code = "CR_GIVEN";
                if (rdbChequeGiven.Checked)
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
                //For received
                Code = "CR_RECEIVED";
                if (rdbChequeReceived.Checked)
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
                //For Both
                Code = "CR_BOTH";
                if (rdbBoth.Checked)
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
                //For Cleared
                Code = "CR_CLEARED";
                if (rdbCleared.Checked)
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
                //For UnCleared
                Code = "CR_UNCLEARED";
                if (rdbUnCleared.Checked)
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
                //For Date
                Code = "CR_DATE";
                if (chkDateRange.Checked)
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
                //For Bank
                Code = "CR_BANK";
                Value = cboBanks.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Party
                Code = "CR_PARTY";
                Value = cboParty.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For UnCleared
                Code = "CR_UNCLEARED";
                if (rdbUnCleared.Checked)
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
                //For CBank
                Code = "CR_CBANK";
                if (chkBank.Checked)
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
                //For CParty
                Code = "CR_CPARTY";
                if (chkParty.Checked)
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
            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            //MessageBox.Show(strXML);
            return strXML;
            #region code not in use
            //string Code, Value;
            //int PreferenceID, UserID;
            //UserID = User.CurrUserID;
            //DataTable dt = m_ReportPreference.GetPreferenceInfo(UserID, "CHEQUE_REPORT");

            ////For Accounting Class
            //Code = "CR_ACCOUNTING_CLASS";
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
            ////For Project
            //Code = "CR_PROJECT";
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
            ////For All
            //Code = "CR_ALL";
            //if (rdbChequeAll.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            ////For given
            //Code = "CR_GIVEN";
            //if (rdbChequeGiven.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            ////For received
            //Code = "CR_RECEIVED";
            //if (rdbChequeReceived.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            ////For Both
            //Code = "CR_BOTH";
            //if (rdbBoth.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            ////For Cleared
            //Code = "CR_CLEARED";
            //if (rdbCleared.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            ////For UnCleared
            //Code = "CR_UNCLEARED";
            //if (rdbUnCleared.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            ////For Date
            //Code = "CR_DATE";
            //if (chkDateRange.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            ////For Bank
            //Code = "CR_BANK";
            //Value = cboBanks.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Party
            //Code = "CR_PARTY";
            //Value = cboParty.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For UnCleared
            //Code = "CR_UNCLEARED";
            //if (rdbUnCleared.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            ////For CBank
            //Code = "CR_CBANK";
            //if (chkBank.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            ////For CParty
            //Code = "CR_CPARTY";
            //if (chkParty.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
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
            //if (dt.Rows.Count < 1)
            //{
            //    Global.Msg("Report Preferences Inserted Sucessfully!!!");
            //}
            //else
            //{
            //    Global.Msg("Report Preferences Modified Sucessfully!!!");
            //}
            #endregion
        }

        private void btnSelectAccClass_Click_1(object sender, EventArgs e)
        {
            //just for test
            ChequeRegisterSettings CRS = new ChequeRegisterSettings();
            try
            {
                CRS.AccClassID = AccClassID;

            }
            catch
            {
                //Ignore 
            }
            frmSelectAccClass frm = new frmSelectAccClass(this, CRS.AccClassID);

            if (!frm.IsDisposed)
                frm.ShowDialog();
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
