using BusinessLogic;
using BusinessLogic.HRM;
using BusinessLogic.HRM.Report;
using DateManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HRM.View.Reports
{
    public partial class frmEmployeeLoanSettings : Form
    {
        ListItem liLoan = new ListItem();
        public frmEmployeeLoanSettings()
        {
            InitializeComponent();
        }

        private void frmEmployeeLoanSettings_Load(object sender, EventArgs e)
        {
            LoadYears();
            LoadMonths();
            LoadLoan();
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

        private void LoadMonths()
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
        private void LoadLoan()
        {
            try
            {
                cmbLoan.Items.Clear();
                DataTable dt = new DataTable();

                dt = Hrm.GetLoanForCmb();                        
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        cmbLoan.Items.Add(new ListItem((int)dr["ID"], dr["Value"].ToString()));

                    }
                    //cbo.DataSource = dt;
                    //cbo.DisplayMember = "Value";
                    //cbo.ValueMember = "ID";
                    cmbLoan.SelectedIndex = 0;
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

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbLoan.SelectedIndex == -1)
                {
                    Global.Msg("Please select a loan.");
                    return;
                }

                if (cmbMonth.SelectedIndex == -1)
                {
                    Global.Msg("Please select a month.");
                    return;
                }

                if (cmbYear.SelectedIndex == -1)
                {
                    Global.Msg("Please select a Year.");
                    return;
                }

                EmployeeLoanSettings ELS = new EmployeeLoanSettings();
                liLoan = (ListItem)cmbLoan.SelectedItem;
                ELS.LoanID = liLoan.ID;
                ELS.Loan = cmbLoan.SelectedItem.ToString();
                ListItem liMth = (ListItem)cmbMonth.SelectedItem;
                ELS.MonthID = liMth.ID;
                ELS.Month = cmbMonth.SelectedItem.ToString();
                ELS.Year = Convert.ToInt32(cmbYear.SelectedItem.ToString());
                bool isFixed = false;
                DataTable dt = Employee.GetLoanReport(ELS.LoanID, ELS.MonthID, ELS.Year, ref isFixed);
                if(dt.Rows.Count <= 0)
                {
                    Global.Msg("No record found.");
                    return;
                }
                frmEmployeeLoan EL = new frmEmployeeLoan(ELS);
                EL.Show();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
    }
}
