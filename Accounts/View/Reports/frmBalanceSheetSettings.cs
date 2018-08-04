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
using System.IO;
using System.Data.SqlClient;
using Common;
using Accounts.Reports;


namespace Accounts
{
    public partial class frmBalanceSheetSettings : Form, IfrmSelectAccClassID, IfrmDateConverter
    {
        ReportPreference m_ReportPreference;
        ArrayList AccClassID = new ArrayList();
        DataTable dtAccClassID = new DataTable();
        private string Prefix = "";
        public frmBalanceSheetSettings()
        {
            InitializeComponent();
        }
        IMDIMainForm frmMDI;
        public frmBalanceSheetSettings(IMDIMainForm frm)
        {
            InitializeComponent();
            frmMDI = frm;
        }

        public void DateConvert(DateTime DotNetDate)
        {

            txtToDate.Text = Date.ToSystem(DotNetDate);

        }

        private void LoadMonths()
        {
            //Check Fiscal year(By default in English)
            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();
            //get first month from start fiscal date
            // DateTime start = new DateTime();
            //if(CompDetails.FYFrom != null)                      
           DateTime start = Convert.ToDateTime(CompDetails.FYFrom); //English fiscal year

            ListItem[] ListDate = new ListItem[12];
            for (int month = 0; month < 12; month++)
            {
                ListDate[month] = new ListItem();
                ListDate[month].ID = month + 1;
                ListDate[month].Value = Date.GetMonthList((Date.DateType)Date.DefaultDate, Language.LanguageType.English)[month + 1];

            }
             // DateTime FYStartDate = new DateTime();
             //if(CompDetails.FYFrom != null)  
           DateTime FYStartDate =Convert.ToDateTime(CompDetails.FYFrom);

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

        private void frmBalanceSheetSettings_Load(object sender, EventArgs e)
        {
            m_ReportPreference = new ReportPreference();
            Global.CheckAcc = "";
            //load combobox from start date to end date
            //Load cboMonths
            LoadMonths();
            //ListProject(cboProjectName);
            // cboProjectName.Items.Add(new ListItem((0), "All"));
            LoadComboboxProject(cboProjectName, 0);
            //Enable the this radiobutton at form load condition
            //rdAllGrps.Checked = true;
            //rdbtnVertical.Checked = true;
            int checkuserid = User.CurrUserID;
            DataTable dt = m_ReportPreference.GetPreferenceCount(checkuserid, "BALANCE_SHEET");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    switch (dr["Code"].ToString())
                    {
                        case "BS_DATE":
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
                        case "BS_ACCOUNTING_CLASS":
                            AccClassID.Add(dr["Value"].ToString());
                            ArrayList arrchildAccClassIds = new ArrayList();
                            AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                            foreach (object obj in arrchildAccClassIds)
                            {
                                int i = (int)obj;
                                AccClassID.Add(i.ToString());
                             }

                            break;
                        case "BS_STYLE":
                            cmbbalancesheetstyle.SelectedIndex =Convert.ToInt32( dr["Value"].ToString());
                            break;
                        case "BS_ISSUMMARY":
                            if (dr["Value"].ToString() == "True")
                            {
                                rdSummary.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "False")
                            {
                                rdbtnDetail.Checked = true;
                            }
                            break;
                        case "BS_PROJECT":
                            int indexvalue = Convert.ToInt32(dr["Value"].ToString());
                            cboProjectName.SelectedIndex = indexvalue;
                            //cboProjectName.SelectedValue = indexvalue;
                            
                            break;
                        case "BS_SHOWZEROBALANCE":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkShowZeroBal.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                chkShowZeroBal.Checked = false;
                            }
                            break;
                        case "BS_SHOW_SECOND_LEVEL":
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
                cmbbalancesheetstyle.SelectedIndex = 0;
                //load combobox from start date to end date
                //Load cboMonths
                LoadMonths();
                //ListProject(cboProjectName);
                // cboProjectName.Items.Add(new ListItem((0), "All"));
                LoadComboboxProject(cboProjectName, 0);
                //Enable the this radiobutton at form load condition
                //rdAllGrps.Checked = true;
                //rdbtnVertical.Checked = true;
                rdSummary.Checked = true;
                txtToDate.Mask = Date.FormatToMask();
                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
                //txtFromDate.Text = Date.ToSystem(new DateTime(2009, 01, 24));
                //If nothing is selected add Root class ID
                AccClassID.Add(Global.GlobalAccClassID.ToString());
                //AccClassID.Add("1");
                //just for test
                ArrayList arrchildAccClassIds = new ArrayList();
                //AccountClass.GetChildIDs(1, ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                AccountClass.GetChildIDs(Global.GlobalAccClassID, ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                foreach (object obj in arrchildAccClassIds)
                {
                    int i = (int)obj;
                    AccClassID.Add(i.ToString());
                }
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
            
            bool chkUserPermission = UserPermission.ChkUserPermission("BALANCE_SHEET");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            //if (cmbbalancesheetstyle.SelectedIndex == 0 || cmbbalancesheetstyle.SelectedIndex == 2)
            //{
                BalanceSheetSettings m_BSShow = new BalanceSheetSettings();//Dynamic memory allocation of an object
                //txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate 

                txtToDate.Mask = Date.FormatToMask();

                //m_BSShow.FromDate = Date.ToDotNet(txtFromDate.Text);// Converting  datetime came via controls into DonNet datetime formate 
                m_BSShow.FromDate = Date.ToDotNet(Date.ToSystem(Global.Fiscal_Year_Start));// Converting  datetime came via controls into DonNet datetime formate 
                m_BSShow.ToDate = Date.ToDotNet(txtToDate.Text);

                //Get the GroupID of the Asset and Liabilities Group Account


                m_BSShow.AccClassID = AccClassID;
                m_BSShow.ShowZeroBalance = chkShowZeroBal.Checked;
                m_BSShow.Detail = rdbtnDetail.Checked;
                m_BSShow.ShowSecondLevelDtl = chkShowSecLevGrpDet.Checked;
                //if (rbtnTformate.Checked)
                //    m_BSShow.DispFormat = BalanceSheetSettings.DisplayFormat.TFormat;
                //else if (rdbtnVertical.Checked)
                if (cmbbalancesheetstyle.SelectedIndex == 0)
                    m_BSShow.DispFormat = BalanceSheetSettings.DisplayFormat.Vertical;
                else if (cmbbalancesheetstyle.SelectedIndex == 2)
                    m_BSShow.DispFormat = BalanceSheetSettings.DisplayFormat.Standard;
                 else if (cmbbalancesheetstyle.SelectedIndex == 1)
                    m_BSShow.DispFormat = BalanceSheetSettings.DisplayFormat.TFormat;


                ListItem liProjectInfo = new ListItem();
                liProjectInfo = (ListItem)cboProjectName.SelectedItem;
                m_BSShow.ProjectID = Convert.ToInt32(liProjectInfo.ID);

                frmBalanceSheet frmShow = new frmBalanceSheet(m_BSShow,frmMDI);//Passing object as an argument 
                frmShow.Show();
            //}
            #region old code
            //if (cmbbalancesheetstyle.SelectedIndex == 1)
            //{
            //    BalanceSheetSettings m_BSShow = new BalanceSheetSettings();//Dynamic memory allocation of an object
            //    //txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate 

            //    txtToDate.Mask = Date.FormatToMask();

            //    //m_BSShow.FromDate = Date.ToDotNet(txtFromDate.Text);// Converting  datetime came via controls into DonNet datetime formate 
            //    m_BSShow.FromDate = Date.ToDotNet(Date.ToSystem(Global.Fiscal_Year_Start));// Converting  datetime came via controls into DonNet datetime formate 
            //    m_BSShow.ToDate = Date.ToDotNet(txtToDate.Text);

            //    //Get the GroupID of the Asset and Liabilities Group Account


            //    m_BSShow.AccClassID = AccClassID;
            //    m_BSShow.ShowZeroBalance = chkShowZeroBal.Checked;
            //    m_BSShow.Detail = rdbtnDetail.Checked;
            //    m_BSShow.ShowSecondLevelDtl = chkShowSecLevGrpDet.Checked;
            //    //if (rbtnTformate.Checked)
            //        m_BSShow.DispFormat = BalanceSheetSettings.DisplayFormat.TFormat;
            //    //else if (rdbtnVertical.Checked)
            //    //    m_BSShow.DispFormat = BalanceSheetSettings.DisplayFormat.Vertical;

            //    ListItem liProjectInfo = new ListItem();
            //    liProjectInfo = (ListItem)cboProjectName.SelectedItem;
            //    m_BSShow.ProjectID = Convert.ToInt32(liProjectInfo.ID);

            //    frmBalanceSheet frmShow = new frmBalanceSheet(m_BSShow,frmMDI);//Passing object as an argument 
            //    frmShow.Show();
            //}
            //else if (cmbbalancesheetstyle.SelectedIndex == 2)
            //{
            //    BalanceSheetSettings m_BSShow = new BalanceSheetSettings();//Dynamic memory allocation of an object
            //    //txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate 

            //    txtToDate.Mask = Date.FormatToMask();

            //    //m_BSShow.FromDate = Date.ToDotNet(txtFromDate.Text);// Converting  datetime came via controls into DonNet datetime formate 
            //    m_BSShow.FromDate = Date.ToDotNet(Date.ToSystem(Global.Fiscal_Year_Start));// Converting  datetime came via controls into DonNet datetime formate 
            //    m_BSShow.ToDate = Date.ToDotNet(txtToDate.Text);

            //    //Get the GroupID of the Asset and Liabilities Group Account


            //    m_BSShow.AccClassID = AccClassID;
            //    m_BSShow.ShowZeroBalance = chkShowZeroBal.Checked;
            //    m_BSShow.ShowSecondLevelDtl = chkShowSecLevGrpDet.Checked;

            //    ListItem liProjectInfo = new ListItem();
            //    liProjectInfo = (ListItem)cboProjectName.SelectedItem;
            //    m_BSShow.ProjectID = Convert.ToInt32(liProjectInfo.ID);
            //    frmMBalanceSheet frmmbal = new frmMBalanceSheet(m_BSShow);
            //    frmmbal.ShowDialog();
            //}
            #endregion
        }

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {
            BalanceSheetSettings BSS = new BalanceSheetSettings();
            try
            {
                BSS.AccClassID = AccClassID;

            }
            catch
            {
                //Ignore 
            }
            frmSelectAccClass frm = new frmSelectAccClass(this, BSS.AccClassID);

            if (!frm.IsDisposed)
                frm.ShowDialog();
            //frmSelectAccClass frm = new frmSelectAccClass(this);
            //frm.Show();
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
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

        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnSelectAccClass.Focus();
            }
        }

        private void rbtnTformate_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyValue == 13)
            //{
            //    rdbtnVertical.Focus();
            //}
        }

        private void rdbtnVertical_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                sComboBox1.Focus();
            }
        }

        private void sComboBox1_KeyDown(object sender, KeyEventArgs e)
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
                rdbtnDetail.Focus();
            }
        }

        private void rdbtnDetail_KeyDown(object sender, KeyEventArgs e)
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
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
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

        private void cboMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem SelItem = (ListItem)cboMonths.SelectedItem;
                CompanyDetails CompDetails = new CompanyDetails();
                CompDetails = CompanyInfo.GetInfo();
                //DateTime FYStartDate = new DateTime();
                //if (CompDetails.FYFrom != null)
                  DateTime   FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);

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

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void frmBalanceSheetSettings_KeyDown(object sender, KeyEventArgs e)
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

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void btnsavestate_Click(object sender, EventArgs e)
        {
            int UserID;
            string ReadXMLDetails;
            UserID = User.CurrUserID;
            ReadXMLDetails=ChangeReportPreferences();
           string Result= BalanceSheet.RptPreferences(UserID,ReadXMLDetails);
            if(Result=="INSERT")
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
                Code = "BS_DATE";
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
                Code = "BS_ACCOUNTING_CLASS";
                Value = Global.GlobalAccClassID.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Balance Sheet Style
                Code = "BS_STYLE";
                Value = cmbbalancesheetstyle.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Summary or details
                Code = "BS_ISSUMMARY";
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
                //For Project
                Code = "BS_PROJECT"; 
                Value = cboProjectName.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Zero Balance
                Code = "BS_SHOWZEROBALANCE";
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
                //For Second Level
                Code = "BS_SHOW_SECOND_LEVEL";
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

           
        }

    }
}
