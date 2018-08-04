using BusinessLogic;
using BusinessLogic.HOS;
using BusinessLogic.HOS.Report;
using Common;
using DateManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hospital.View.Report
{
    public partial class frmPatientSetting : Form, IfrmDateConverter
    {
        public frmPatientSetting()
        {
            InitializeComponent();
        }
        private bool IsFromDate = false;
        string m_RptType = "General";
        ReportPreference m_ReportPreference;
        ArrayList AccClassID = new ArrayList();
        private void cboProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void btnFromDate_Click(object sender, EventArgs e)
        {
            IsFromDate = true;//this variable is used as flag to notify which date is selected to change the date converter...coz same funtion is used to change the date  
            DateTime dtDate = Date.ToDotNet(txtFromDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

      

        private void btnToDate_Click(object sender, EventArgs e)
        {
            IsFromDate = false;
            DateTime dtDate = Date.ToDotNet(txtToDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        
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

        private void frmPatientSetting_Load(object sender, EventArgs e)
        {

            try
            {
                this.Text = m_RptType + " Patient Report";

                LoadComboBox(cboPatientAll, CmbType.Patient);
                LoadMonths();
                txtToDate.Mask = Date.FormatToMask();
                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
                txtFromDate.Mask = Date.FormatToMask();
                txtFromDate.Text = Date.ToSystem(Global.Fiscal_Year_Start);
                rdPatientAll.Checked = true;
                cboPatientAll.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;


            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
           
        }
        private enum CmbType
        {
           
          
            PatientType,
            Patient
        }
        ListItem liPatientType = new ListItem();
        ListItem liPatient = new ListItem();
        private void LoadComboBox(ComboBox cbo, CmbType type)
        {
            try
            {
                cbo.Items.Clear();
                DataTable dt = new DataTable();
                switch (type)
                {
                                    
                   case CmbType.Patient:
                        dt = Patient.GetAllPatient();
                        break;
                }

                if (dt.Rows.Count > 0)
                {
                    if (type == CmbType.Patient )
                        cbo.Items.Add(new ListItem(0, "All"));

                    foreach (DataRow dr in dt.Rows)
                    {
                        cbo.Items.Add(new ListItem((int)dr["ID"], dr["Value"].ToString()));

                    }
                   
                    if (type == CmbType.Patient)
                        cbo.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void cboPatType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkPatType_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                //int id;
                PatientReportSetting srs = new PatientReportSetting();
             

                ListItem liPartyID = new ListItem();
                liPartyID = (ListItem)cboPatientAll.SelectedItem;
                if (rdpartySingle.Checked)
                {
                    srs.patientID = Convert.ToInt32(liPartyID.ID);
                }
                else
                {
                    srs.patientID = null;
                }

                if (chkDateRange.Checked)
                {
                    txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate
                    txtToDate.Mask = Date.FormatToMask();
                    srs.FromDate = Date.ToDotNet(txtFromDate.Text);
                    srs.ToDate = Date.ToDotNet(txtToDate.Text);
                }
                else
                {
                    srs.FromDate = null;
                    srs.ToDate = null;
                }
                srs.AccClassID = AccClassID;

                //srs.rptType = m_RptType;
                frmPatientReport sr = new frmPatientReport(srs);
                sr.Show();
            }
            catch (Exception ex)
            {

                Global.MsgError(ex.Message);
            }


        }

        private void rdPatientAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPatientAll.Checked)
            {
                cboPatientAll.Enabled = true;
                cboPatientAll.SelectedIndex = 0;
                
            }
            else
                cboPatientAll.Enabled = false;
        }

        private void grpParty_Enter(object sender, EventArgs e)
        {

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

        private void rdpartySingle_CheckedChanged(object sender, EventArgs e)
        {
            if (rdpartySingle.Checked)
            {
                cboPatientAll.Enabled = true;
            }
            else
            {
                cboPatientAll.Enabled = false;
            }
        }
    }
}
