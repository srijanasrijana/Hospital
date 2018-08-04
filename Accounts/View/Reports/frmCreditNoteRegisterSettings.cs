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
using DateManager;
using Common;


namespace Accounts
{
    public partial class frmCreditNoteRegisterSettings : Form, IfrmSelectAccClassID,IfrmDateConverter
    {
        private bool IsFromDate = false;
        ReportPreference m_ReportPreference;
        private string Prefix = "";
        ArrayList AccClassID = new ArrayList();

        DataTable dtAccClassID = new DataTable();
        public frmCreditNoteRegisterSettings()
        {
            InitializeComponent();
        }

        private void frmCreditNoteRegisterSettings_Load(object sender, EventArgs e)
        {

            try
            {
                LoadMonths();
                m_ReportPreference = new ReportPreference();
                LoadComboboxProject(cboProjectName, 0);
                grpDate.Enabled = false;
                txtFromDate.Mask = Date.FormatToMask();
                txtToDate.Mask = Date.FormatToMask();
                txtToDate.Text = Date.ToSystem(Date.GetServerDate()); //By default show the current date from the sqlserver.
                txtFromDate.Text = Date.ToSystem(Global.Fiscal_Year_Start);
                int checkuserid = User.CurrUserID;
                DataTable dtrpt = m_ReportPreference.GetPreferenceCount(checkuserid, "CREDIT_NOTE");
                if (dtrpt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtrpt.Rows)
                    {
                        switch (dr["Code"].ToString())
                        {

                            case "CN_ACCOUNTING_CLASS":
                                AccClassID.Add(dr["Value"].ToString());
                                ArrayList arrchildAccClassIds = new ArrayList();
                                AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                                foreach (object obj in arrchildAccClassIds)
                                {
                                    int i = (int)obj;
                                    AccClassID.Add(i.ToString());
                                }

                                break;
                            case "CN_PROJECT":
                                cboProjectName.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                                break;

                            case "CN_DATE":
                                if (dr["Value"].ToString() == "1")
                                {
                                    chkDate.Checked = true;
                                }
                                else
                                {
                                    chkDate.Checked = false;
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
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
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




        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked == true)
            {
                grpDate.Enabled = true;



            }
            else
            {

                grpDate.Enabled = false;


            }
        }

        //A function from the Interface IfrmAccClassID. Used to apply the Datatable to this form from AddAccClass Form

        public void AddSelectedAccClassID(DataTable AccClassID1)
        {
            try
            {

                AccClassID.Clear();
                for (int i = 0; i < AccClassID1.Rows.Count; i++)
                {
                    DataRow drAccClassID = AccClassID1.Rows[i];
                    AccClassID.Add(drAccClassID["AccClassID"].ToString());

                }

            }
            catch (Exception)
            {

                throw;
            }


        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CREDITNOTE_REGISTER");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            CreditNoteRegisterSettings m_CredNoteRegSettings = new CreditNoteRegisterSettings();
            txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate 
            txtToDate.Mask = Date.FormatToMask();
            m_CredNoteRegSettings.FromDate = Date.ToDotNet(txtFromDate.Text);// Converting  datetime came via controls into DonNet datetime formate 
            m_CredNoteRegSettings.ToDate = Date.ToDotNet(txtToDate.Text);
            m_CredNoteRegSettings.AccClassID = m_CredNoteRegSettings.AccClassID;
            if (chkDate.Checked)
            {
                m_CredNoteRegSettings.HasDateRange = true;
            }
            else
                m_CredNoteRegSettings.HasDateRange = false;
            ListItem liProjectID = new ListItem();
            liProjectID = (ListItem)cboProjectName.SelectedItem;
            m_CredNoteRegSettings.ProjectID = Convert.ToInt32(liProjectID.ID);

            frmCreditNoteRegister frm = new frmCreditNoteRegister(m_CredNoteRegSettings);
            frm.Show();

        }

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {

            frmSelectAccClass frm = new frmSelectAccClass(this);
            frm.Show();
        }

        private void frmCreditNoteRegisterSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
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

        private void button1_Click(object sender, EventArgs e)
        {

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
                Code = "CN_ACCOUNTING_CLASS";
                Value = Global.GlobalAccClassID.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Project
                Code = "CN_PROJECT";
                Value = cboProjectName.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For  date
                Code = "CN_DATE";
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

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            //MessageBox.Show(strXML);
            return strXML;
           
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
