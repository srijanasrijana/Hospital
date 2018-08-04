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
    public partial class frmEmployeeAdvanceSettings : Form
    {
        public frmEmployeeAdvanceSettings()
        {
            InitializeComponent();
        }

        #region load month and year combobox
        private void LoadYears()
        {
            try
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
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            //present month is selected
        }

        private void LoadMonths()
        {
            try
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
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        } 
        #endregion
        private void frmEmployeeAdvanceSetting_Load(object sender, EventArgs e)
        {
            LoadYears();
            LoadMonths();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
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
                EmployeeAdvanceSettings EAS = new EmployeeAdvanceSettings();

                ListItem liMth = (ListItem)cmbMonth.SelectedItem;
                EAS.MonthID = liMth.ID;

                EAS.Month = cmbMonth.Text;
                EAS.Year = Convert.ToInt32(cmbYear.SelectedItem.ToString());


                // check if any instance of frmEmployeeAdvance is running or open, if so close the open form and reopen the form with currently chosen settings
                Form fc = Application.OpenForms["frmEmployeeAdvance"];

                if (fc != null)
                    fc.Close(); 

                frmEmployeeAdvance fea = new frmEmployeeAdvance(EAS);
                fea.Show();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }
    }
}
