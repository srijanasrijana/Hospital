using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using BusinessLogic.HRM;
using BusinessLogic.HRM.Report;
using DateManager;

namespace HRM.View.Reports
{
    public partial class frmEmployeeAnnualReportSettings : Form
    {
        public frmEmployeeAnnualReportSettings()
        {
            InitializeComponent();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                EmpAnnualReportSettings settings = new EmpAnnualReportSettings();
                settings.EmployeeID = Convert.ToInt32(cmbEmployeeList.SelectedValue);
                settings.EmployeeName = cmbEmployeeList.Text;
                settings.Year = cmbYear.Text;
                ListItem SelItem = (ListItem)cmbFromMonth.SelectedItem;
                settings.FromMonth = SelItem.ID;
                settings.FacultyID = Convert.ToInt32(cmbFaculty.SelectedValue);
                SelItem = (ListItem)cmbToMonth.SelectedItem;
                settings.ToMonth = SelItem.ID;
                settings.FacultyName = cmbFaculty.Text;
                frmCommonReport.reportType reportType = frmCommonReport.reportType.Annual;

                if (rbtnTaxReport.Checked)
                {
                    reportType = frmCommonReport.reportType.TaxAdjust;
                }
                frmCommonReport frmReport = new frmCommonReport(settings, reportType);
                frmReport.ShowDialog();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void LoadYears()
        {
            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();
            DateTime FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);
            int refYearStart = 0;
            int refYearEnd = 0;
            int FYMonth = 0;
            int refDay = 0;
            Date.EngToNep(FYStartDate, ref refYearStart, ref FYMonth, ref refDay);

            Date.EngToNep(DateTime.Today, ref refYearEnd, ref FYMonth, ref refDay);
            for (int i = refYearStart; i <= refYearEnd + 1; i++)
            {
                cmbYear.Items.Add(i);
            }
            cmbYear.SelectedItem = refYearEnd;
            //present month is selected
        }
        private void LoadMonths(ComboBox cmbMonth)
        {
            cmbMonth.Items.Clear();
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
            // old code to load month combobox starting from start month
            //do
            //{
            //    if (MonthCounter > 12)
            //        MonthCounter = 1;
            //    cmbMonth.Items.Add(ListDate[MonthCounter - 1]);
            //    MonthCounter++;

            //} while (MonthCounter != FYMonth);

            // new code 
            for (int i = 0; i < 12; i++)
            {
                cmbMonth.Items.Add(ListDate[MonthCounter - 1]);
                if (MonthCounter >= 12)
                    MonthCounter = 1;
                else
                    MonthCounter++;
            }
            Date.EngToNep(DateTime.Today, ref refYear, ref FYMonth, ref refDay);
            cmbMonth.SelectedItem = ListDate[FYMonth - 1];

        }

        public void LoadFiscalYear()
        {
            //DataTable dt = CompanyInfo.GetFiscalYearInfo();
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            ListItem item = new ListItem(0,m_CompanyDetails.FiscalYear);
            cmbYear.Items.Add(item);
        }
        private void frmEmployeeAnnualReportSettings_Load(object sender, EventArgs e)
        {
            try
            {
                //chkIsTaxAdjust.Checked = false;
                rbtnEmployeeReport.Checked = true;
                //LoadYears();
                LoadFiscalYear();
                LoadMonths(cmbFromMonth);
                LoadMonths(cmbToMonth);

                // load employee list for easier search functionality
                DataTable dtEmployeeList = Employee.GetEmployeeList();
                cmbEmployeeList.DataSource = null;
                cmbEmployeeList.DataSource = dtEmployeeList;
                cmbEmployeeList.DisplayMember = "Name";
                cmbEmployeeList.ValueMember = "ID";

                DataTable dtFaculty = Hrm.GetEmpFacultyForCmb();
                cmbFaculty.DataSource = dtFaculty;
                cmbFaculty.ValueMember = "ID";
                cmbFaculty.DisplayMember = "Value";

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

        private void rbtnEmployeeReport_CheckedChanged(object sender, EventArgs e)
        {
            gbEmployeeReport.Visible = !rbtnTaxReport.Checked;
            gbTaxAdjsut.Visible = rbtnTaxReport.Checked;
        }

    }
}
