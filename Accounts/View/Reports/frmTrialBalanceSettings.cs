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
using Accounts.Reports;


namespace Accounts
{
    public partial class frmTrialBalanceSetting : Form, IfrmSelectAccClassID, IfrmDateConverter
    {
        ArrayList AccClassID = new ArrayList();
        ReportPreference m_ReportPreference;
        DataTable dtAccClassID = new DataTable();
        private string Prefix = "";
        private IMDIMainForm frmMDI;

        public frmTrialBalanceSetting()
        {
            InitializeComponent();
        }

        public frmTrialBalanceSetting(IMDIMainForm frm)
        {
            InitializeComponent();
            frmMDI = frm;
        }

        public void DateConvert(DateTime DotNetDate)
        {

            txtToDate.Text = Date.ToSystem(DotNetDate);

        }

        private void frmTrialBalanceSetting_Load(object sender, EventArgs e)
        {
            m_ReportPreference = new ReportPreference();
            //txtToDate.Text = Date.ToSystem(new DateTime(2009, 01, 24));
            Global.CheckAcc = "";
            //load combobox from start date to end date
            //Load cboMonths
            LoadMonths();
            LoadComboboxProject(cboProjectName, 0);
            int checkuserid = User.CurrUserID;
            DataTable dt = m_ReportPreference.GetPreferenceCount(checkuserid, "TRIAL_BALANCE");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    switch (dr["Code"].ToString())
                    {
                        case "TB_DATE":
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
                        case "TB_ALLGROUP":
                            txtToDate.Mask = Date.FormatToMask();
                            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
                            if (dr["Value"].ToString() == "1")
                            {
                                rdAllGrps.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdAllGrps.Checked = false;
                            }
                            break;
                        case "TB_ONLYPRIMARYGROUP":
                            txtToDate.Mask = Date.FormatToMask();
                            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
                            if (dr["Value"].ToString() == "1")
                            {
                                rdOnlyPrimaryGrps.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdOnlyPrimaryGrps.Checked = false;
                            }
                            break;
                        case "TB_LEDGERONLY":
                            txtToDate.Mask = Date.FormatToMask();
                            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
                            if (dr["Value"].ToString() == "1")
                            {
                                rbtnledgersonly.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rbtnledgersonly.Checked = false;
                            }
                            break;
                        case "TB_ACCOUNTING_CLASS":
                            AccClassID.Add(dr["Value"].ToString());
                            ArrayList arrchildAccClassIds = new ArrayList();
                            AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                            foreach (object obj in arrchildAccClassIds)
                            {
                                int i = (int)obj;
                                AccClassID.Add(i.ToString());
                            }

                            break;
                        case "TB_ISSUMMARY":
                            if (dr["Value"].ToString() == "True")
                            {
                                rdSummary.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "False")
                            {
                                rbtnDetail.Checked = true;
                            }
                            break;
                        case "TB_PROJECT":
                            int indexvalue = Convert.ToInt32(dr["Value"].ToString());
                            cboProjectName.SelectedIndex = indexvalue;
                            //cboProjectName.SelectedValue = indexvalue;

                            break;
                        case "TB_SHOWZEROBALANCE":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkShowZeroBal.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                chkShowZeroBal.Checked = false;
                            }
                            break;
                        case "TB_SHOW_SECOND_LEVEL":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkShowSecLevGrpDet.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                chkShowSecLevGrpDet.Checked = false;
                            }
                            break;

                    }
                }

            }
            else
            {
                rdAllGrps.Checked = true;
                rdSummary.Checked = true;
                //cboProjectName.Items.Add(new ListItem((0), "All"));
                //LoadComboboxProject(cboProjectName, 0);
                //ListProject(cboProjectName);
                //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
                //txtToDate.Mask = Date.FormatToMask();
                txtToDate.Mask = Date.FormatToMask();
                txtToDate.Text = Date.ToSystem(Date.GetServerDate());// Displaying Current DateTime at FormLoad Condition
                //txtToDate.Text = Date.ToSystem(new DateTime(2009, 01, 24));

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
            DateTime start=new DateTime();
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
            ComboBoxControl.Items.Add(new ListItem((0), "All"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], dr[LangField].ToString()));
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
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
       // After firing Show button displaying TrialBalance Form
        private void btnShow_Click(object sender, EventArgs e)
        {

            bool chkUserPermission = UserPermission.ChkUserPermission("CLOSING_TRIAL_BALANCE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            if (chkDate.Checked)
            {
                Global.checkbox = true;
            }
            else
            {
                Global.checkbox = false;
            }
            TrialBalanceSettings m_TBShow = new TrialBalanceSettings();//Dynamic memory allocation of an object           

            txtToDate.Mask =  Date.FormatToMask();       
            //m_TBShow.FromDate =Date.ToDotNet(Date.ToSystem(Global.Fiscal_Year_Start));// Converting  datetime came via controls into DonNet datetime formate 
       
            //txtToDate.Text = Date.ToSystem(Date.GetServerDate());
            if(chkDate.Checked)
            {
                m_TBShow.ToDate = Date.ToDotNet(txtToDate.Text);
              Global.PYToDate=Date.ToDotNet(txtToDate.Text).AddYears(-1);
              Global.PYFromDate = Convert.ToDateTime(" 01 / 01 / 1900");
            }


           // m_TBShow.ToDate = Date.ToDotNet(ToDatefrom);
            m_TBShow.HasDateRange = chkDate.Checked;
           


            m_TBShow.AccClassID = AccClassID;
            m_TBShow.ShowZeroBalance = chkShowZeroBal.Checked;
            m_TBShow.ShowSecondLevelGroupDtl = chkShowSecLevGrpDet.Checked;
            m_TBShow.GroupID = 0;//Manually inserting null GroupID
            
            m_TBShow.AllGroups = rdAllGrps.Checked;
            m_TBShow.OnlyPrimaryGroups = rdOnlyPrimaryGrps.Checked;
            m_TBShow.LedgerOnly = rbtnledgersonly.Checked;
            m_TBShow.Details = rbtnDetail.Checked;

            ListItem liProjectID = new ListItem();
            liProjectID = (ListItem)cboProjectName.SelectedItem;
            m_TBShow.ProjectID = Convert.ToInt32(liProjectID.ID);

            m_TBShow.ShowPreviousYear = chkshowpreviousyear.Checked;
            frmTrial frmShow = new frmTrial(m_TBShow,this,frmMDI);//Passing object as an argument            
           // frmShow.MdiParent = this.MdiParent;
            frmShow.Show();  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Month"></param>
        /// <returns></returns>
        public string GetDateFromMonth(string Month)
        {
            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();
            int selectedMonthNumber = 1;
            string ToDate;
            int daysinMonth=0;
            ///
            ////You may use switch case to get the integer value of the month
            ///
            if (cboMonths.Text == "January") selectedMonthNumber = 01;
            if (cboMonths.Text == "February") selectedMonthNumber = 02;
            if (cboMonths.Text == "March") selectedMonthNumber = 03;
            if (cboMonths.Text == "April") selectedMonthNumber = 04;
            if (cboMonths.Text == "May") selectedMonthNumber = 05;
            if (cboMonths.Text == "June") selectedMonthNumber = 06;
            if (cboMonths.Text == "July") selectedMonthNumber = 07;
            if (cboMonths.Text == "August") selectedMonthNumber = 08;
            if (cboMonths.Text == "September") selectedMonthNumber = 09;
            if (cboMonths.Text == "October") selectedMonthNumber = 10;
            if (cboMonths.Text == "November") selectedMonthNumber = 11;
            if (cboMonths.Text == "December") selectedMonthNumber = 12;
            if (cboMonths.Text == "Baisakh") selectedMonthNumber = 01;
            if (cboMonths.Text == "Jestha") selectedMonthNumber = 02;
            if (cboMonths.Text == "Asadh") selectedMonthNumber = 03;
            if (cboMonths.Text == "Shrawan") selectedMonthNumber = 04;
            if (cboMonths.Text == "Bhadra") selectedMonthNumber = 05;
            if (cboMonths.Text == "Aswin") selectedMonthNumber = 06;
            if (cboMonths.Text == "Kartik") selectedMonthNumber = 07;
            if (cboMonths.Text == "Mangsir") selectedMonthNumber = 08;
            if (cboMonths.Text == "Poush") selectedMonthNumber = 09;
            if (cboMonths.Text == "Magh") selectedMonthNumber = 10;
            if (cboMonths.Text == "Falgun") selectedMonthNumber = 11;
            if (cboMonths.Text == "Chaitra") selectedMonthNumber = 12; 

            //get first month from start fiscal date                
            //  DateTime FYStartDate = new DateTime();
            //if(CompDetails.FYFrom != null)
                DateTime  FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);
                int lowYearFY = FYStartDate.Year;
                int retYear = 0;
                int retMonth = 0;
                int retDay = 0;
                Date.EngToNep(FYStartDate,ref retYear,ref retMonth, ref retDay);

                //string[] LowYear = Global.Fiscal_English_Year.Split('/');
                if (selectedMonthNumber <= 12 && selectedMonthNumber >= retMonth)
                {
                    if (Date.DefaultDate==Date.DateType.Nepali)
                    {
                        DataTable dtDaysInNepaliMonths = new DataTable();
                        // nepYear = Date.GetYearfromMonthNep(month);
                        dtDaysInNepaliMonths = Date.LastDayofMonthNep(lowYearFY, selectedMonthNumber);
                        DataRow drDaysInNepaliMonths = dtDaysInNepaliMonths.Rows[0];
                        daysinMonth = Convert.ToInt32(drDaysInNepaliMonths[cboMonths.Text].ToString());
                    }
                    else if(Date.DefaultDate==Date.DateType.English)
                    {
                        daysinMonth = DateTime.DaysInMonth(lowYearFY, selectedMonthNumber);
                    }
                    ToDate = lowYearFY + "/" + selectedMonthNumber + "/" + daysinMonth;                  
                }
                else
                {
                    if (Date.DefaultDate == Date.DateType.Nepali)
                    {
                        DataTable dtDaysInNepaliMonths = new DataTable();
                        // nepYear = Date.GetYearfromMonthNep(month);
                        dtDaysInNepaliMonths = Date.LastDayofMonthNep(lowYearFY + 1, selectedMonthNumber);
                        DataRow drDaysInNepaliMonths = dtDaysInNepaliMonths.Rows[0];
                        daysinMonth = Convert.ToInt32(drDaysInNepaliMonths[cboMonths.Text].ToString());
                    }
                    else
                    {
                        daysinMonth = DateTime.DaysInMonth(lowYearFY + 1, selectedMonthNumber);
                    }
                    ToDate = (lowYearFY + 1) + "/" + selectedMonthNumber + "/" + daysinMonth;
                }

                ToDate = Date.ToSystem(Convert.ToDateTime(ToDate));
            return ToDate;
        }


        //private DateTime GetEnglishDateFromNepMonth(int month)
        //{
        //    //int nepYear = 0000;
        //    //DataTable dtLastDaysofMonthNep = new DataTable();
        //    //nepYear = Date.GetYearfromMonthNep(month);
        //    //dtLastDaysofMonthNep = Date.LastDayofMonthNep(nepYear, month);
        //    //DataRow drLastDaysofMonthNep = dtLastDaysofMonthNep.Rows[0];
        //    ////Get english date from nepali date
        //    //DateTime dt = Date.ConvNepToEng(nepYear.ToString() + "/" + month.ToString() + "/" + drLastDaysofMonthNep["Asadh"].ToString());
        //    //return dt;
        //}
        // For Enabling the DateRange GroupBox if Checkbox is checked otherwise making disable

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
 
        }

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {

            //just for test
    
            TrialBalanceSettings TBS = new TrialBalanceSettings();
            try
            {
                TBS.AccClassID = AccClassID;
               
            }
            catch
            {
                //Ignore 
            }
            frmSelectAccClass frm = new frmSelectAccClass(this,TBS.AccClassID);
           
            if (!frm.IsDisposed)
                frm.ShowDialog();
        }

        private void rdAllGrps_CheckedChanged(object sender, EventArgs e)
        {
            chkShowSecLevGrpDet.Enabled = false;
        }

        private void rdOnlyPrimaryGrps_CheckedChanged(object sender, EventArgs e)
        {
            if(Global.isOpeningTrial==false)
            chkShowSecLevGrpDet.Enabled = true;
        }

        private void chkDate_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkDate.Checked)
            {
                txtToDate.Enabled = true;
                cboMonths.Enabled = true;
                btnDate.Enabled = true;
                btnToday.Enabled = true;
            }
            if (!chkDate.Checked)
            {
                txtToDate.Enabled = false;
                cboMonths.Enabled = false;
                btnDate.Enabled = false;
                btnToday.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnSelectAccClass.Focus();
            }
        }

        private void rdAllGrps_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                rdOnlyPrimaryGrps.Focus();
            }
        }

        private void rdOnlyPrimaryGrps_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboCurrency.Focus();
            }
        }

        private void cboCurrency_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtRate.Focus();
            }
        }

        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboProjectName.Focus();
            }
        }

        private void cboProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                rdSummary.Focus();
            }
        }

        private void rdSummary_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                rbtnDetail.Focus();
            }
        }

        private void rbtnDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                chkShowZeroBal.Focus();
            }
        }

        private void chkShowZeroBal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                chkShowSecLevGrpDet.Focus();
            }
        }

        private void chkShowSecLevGrpDet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnShow.Focus();
            }
        }


        private void btnDate_Click(object sender, EventArgs e)
        {
            DateTime dtDate = Date.ToDotNet(txtToDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
            _frmDateConverter.ShowDialog();
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
               DateTime  FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);

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
                if (Date.DefaultDate == Date.DateType.Nepali)
                {
                    //Get Last Day
                    DataTable LastDay = Date.LastDayofMonthNep(FYYear, SelItem.ID);
                    FinalDate = Date.NepToEng(FYYear, SelItem.ID, Convert.ToInt16(LastDay.Rows[0][0]));
                }
                else
                    FinalDate = new DateTime(FYYear, SelItem.ID, DateTime.DaysInMonth(FYYear, SelItem.ID));

                txtToDate.Text = Date.ToSystem(FinalDate);
            }
            catch
            {
                //Ignore
            }
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            //Get Todays date in DateTime
            DateTime Today = Date.GetServerDate();
            //Convert it to the System type
            string SysDate = Date.ToSystem(Today);
            //Put the date in mask edit box
            txtToDate.Text = SysDate;

            //Blank the month combo
            cboMonths.SelectedIndex = -1;
            cboMonths.Text = "";
        }

        private void frmTrialBalanceSetting_KeyDown(object sender, KeyEventArgs e)
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
                Code = "TB_DATE";
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

                Code = "TB_ALLGROUP";
                if (rdAllGrps.Checked)
                {
                    Value = "1";
                }
                else
                { Value = "0"; }
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                Code = "TB_ONLYPRIMARYGROUP";
                if (rdOnlyPrimaryGrps.Checked)
                {
                    Value = "1";
                }
                else
                { Value = "0"; }
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                Code = "TB_LEDGERONLY";
                if (rbtnledgersonly.Checked)
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
                Code = "TB_ACCOUNTING_CLASS";
                Value = Global.GlobalAccClassID.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                Code = "TB_ISSUMMARY";
                if (rdSummary.Checked)
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
                Code = "TB_PROJECT";
                Value = cboProjectName.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                Code = "TB_SHOWZEROBALANCE";
                if (chkShowZeroBal.Checked)
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
                Code = "TB_SHOW_SECOND_LEVEL";
                if (chkShowSecLevGrpDet.Checked)
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
            //string Code, Value;
            //int PreferenceID, UserID;
            //UserID = User.CurrUserID;
            //DataTable dt = m_ReportPreference.GetPreferenceInfo(UserID, "TRIAL_BALANCE");
            //if (chkDate.Checked)
            //{
            //    Code = "TB_DATE";
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
            //    Code = "TB_DATE";
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

            //if (rdAllGrps.Checked)
            //{
            //    Code = "TB_ALLGROUP";
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
            //    Code = "TB_ALLGROUP";
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
            //if (rdOnlyPrimaryGrps.Checked)
            //{
            //    Code = "TB_ONLYPRIMARYGROUP";
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
            //    Code = "TB_ONLYPRIMARYGROUP";
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
            //if (rbtnledgersonly.Checked)
            //{
            //    Code = "TB_LEDGERONLY";
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
            //    Code = "TB_LEDGERONLY";
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
            //Code = "TB_ACCOUNTING_CLASS";
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

            ////For Summary or details
            //Code = "TB_ISSUMMARY";
            //if (rdSummary.Checked)
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
            //Code = "TB_PROJECT";
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
            ////For Zero Balance
            //Code = "TB_SHOWZEROBALANCE";
            //if (chkShowZeroBal.Checked)
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
            ////For Second Level
            //Code = "TB_SHOW_SECOND_LEVEL";
            //if (chkShowSecLevGrpDet.Checked)
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
        }


    }
}
