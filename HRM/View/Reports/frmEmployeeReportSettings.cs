using BusinessLogic;
using BusinessLogic.HRM;
using Common;
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
    public partial class frmEmployeeReportSettings : Form, IfrmDateConverter
    {
        private int _paySlipID = 0;
        private int[] _paySlipIDs = null;
        private string DateString = "FROM";
        public frmEmployeeReportSettings()
        {
            InitializeComponent();
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

        private void LoadDepartment()
        {
            DataTable dtData = null;
            dtData = Hrm.GetDepartmentForCmb();
            DataRow dr = dtData.NewRow();
            dr["ID"] = 0;
            dr["Value"] = "All";
            dtData.Rows.InsertAt(dr, 0);
            if (dtData.Rows.Count > 0)
            {
                cmbDepartment.DataSource = null;
                cmbDepartment.DataSource = dtData;
                cmbDepartment.DisplayMember = "Value";
                cmbDepartment.ValueMember = "ID";
                cmbDepartment.SelectedIndex = 0;
            }
        }

        private void LoadFaculty()
        {
            DataTable dtData = null;
            dtData = Hrm.GetEmpFacultyForCmb();
            DataRow dr = dtData.NewRow();
            dr["ID"] = 0;
            dr["Value"] = "All";
            dtData.Rows.InsertAt(dr, 0);
            if (dtData.Rows.Count > 0)
            {
                cmbFaculty.DataSource = null;
                cmbFaculty.DataSource = dtData;
                cmbFaculty.DisplayMember = "Value";
                cmbFaculty.ValueMember = "ID";
                cmbFaculty.SelectedIndex = 0;
            }
        }

        public void LoadSalarySheetDates(object sender, EventArgs e)
        {
            try
            {
                ListItem SelItem = (ListItem)cmbMonth.SelectedItem;
                DataTable dt = Employee.GetSalarySheetDate(SelItem.ID, Convert.ToInt32(cmbYear.SelectedItem));

                cmbDate.DataSource = dt;
                cmbDate.DisplayMember = "PaySlipDate";
                cmbDate.ValueMember = "PaySlipDate";
            }
            catch (Exception EX)
            {
                Global.MsgError(EX.Message);
            }
        }
        private void frmEmployeeReportSetting_Load(object sender, EventArgs e)
        {
            LoadYears();
            LoadMonths();
            LoadDepartment();
            LoadFaculty();
            txtFromDate.Mask = Date.FormatToMask();
            txtToDate.Mask = Date.FormatToMask();

            txtFromDate.Text = Date.ToSystem(Date.GetServerDate());
            txtToDate.Text = Date.ToSystem(Date.GetServerDate());

            chkDate.Checked = false;
            LoadSalarySheetDates(null,null);

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {

                BusinessLogic.HRM.Report.EmployeeReportSettings settings = new BusinessLogic.HRM.Report.EmployeeReportSettings();

                settings.paySlipDate  = null;
                settings.fromDate = null;
                settings.toDate = null;
                if(chkDate.Checked)
                {
                    //settings.paySlipDate = Date.ToDotNet(cmbDate.Text);
                    settings.fromDate = Date.ToDotNet(txtFromDate.Text);
                    settings.toDate = Date.ToDotNet(txtToDate.Text);
                }
                else
                {
                    settings.paySlipDate = null;
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

                if (cmbDepartment.SelectedIndex == -1)
                {
                    Global.Msg("Please select a Department.");
                    return;
                }

                if (cmbFaculty.SelectedIndex == -1)
                {
                    Global.Msg("Please select a Faculty.");
                    return;
                }

                if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1)
                {
                    int month = 0;
                    string year = "";
                    ListItem SelItem = (ListItem)cmbMonth.SelectedItem;
                    month = SelItem.ID;
                    year = cmbYear.SelectedItem.ToString();
                    int departmentID = 0;
                    departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                    int FacultyID = 0;
                    FacultyID = Convert.ToInt32(cmbFaculty.SelectedValue);
                    DataTable dt = Employee.GetPayslipMaster(month, year, departmentID, FacultyID, true);
                    if (dt.Rows.Count > 0)
                    {

                        // check if any instance of frmSalaryList,frmEmployeePF,frmSalarySheet,frmCommonReport is running or open, if so close the open form and reopen the form with currently chosen settings
                        FormCollection formCollection = Application.OpenForms;
                        foreach (Form form in formCollection)
                        {
                            if (form.Name == "frmSalaryList" || form.Name == "frmEmployeePF" || form.Name == "frmSalarySheet" || form.Name == "frmPensionFundReport")
                            {
                                form.Close();
                                break;
                            }
                        }
                       
                        _paySlipID = Convert.ToInt16(dt.Rows[0]["salaryPaySlipID"].ToString());
                        _paySlipIDs = new int[dt.Rows.Count];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            _paySlipIDs[i] = Convert.ToInt32(dt.Rows[i]["salaryPaySlipID"].ToString());
                        }

                        settings.paySlipIds = _paySlipIDs;
                        settings.faculty = cmbFaculty.Text;

                        if (rbtnSalaryList.Checked == true)
                        {
                            //frmSalaryList salL = new frmSalaryList(_paySlipID, cmbFaculty.Text);
                            frmSalaryList salL = new frmSalaryList(settings); //new frmSalaryList(_paySlipIDs, cmbFaculty.Text, chkRemaining.Checked, date, Date.ToDotNet(txtFromDate.Text), Date.ToDotNet(txtToDate.Text));
                            salL.Show();
                        }
                        else if (rbtnPF.Checked == true)
                        {
                            //frmEmployeePF empPF = new frmEmployeePF(_paySlipID, cmbFaculty.Text);
                            frmEmployeePF empPF = new frmEmployeePF(settings);//(_paySlipIDs, cmbFaculty.Text, chkRemaining.Checked, date, Date.ToDotNet(txtFromDate.Text), Date.ToDotNet(txtToDate.Text));
                            empPF.Show();
                        }
                        else if (rbtnSSR.Checked == true)
                        {
                            //frmSalarySheet ss = new frmSalarySheet(_paySlipID, cmbFaculty.Text);
                            frmSalarySheet ss = new frmSalarySheet(settings);//(_paySlipIDs, cmbFaculty.Text, chkRemaining.Checked, date, Date.ToDotNet(txtFromDate.Text), Date.ToDotNet(txtToDate.Text));
                            ss.Show();
                        }

                        else
                        {
                            if (rbtnPension.Checked == true)
                            {
                                //frmCommonReport ss = new frmCommonReport(_paySlipID, cmbFaculty.Text, frmCommonReport.reportType.Pension);
                                frmCommonReport ss = new frmCommonReport(settings, frmCommonReport.reportType.Pension);//t(_paySlipIDs, cmbFaculty.Text, frmCommonReport.reportType.Pension, chkRemaining.Checked, date, Date.ToDotNet(txtFromDate.Text), Date.ToDotNet(txtToDate.Text));
                                ss.Show();
                            }
                            else if (rbtnCIT.Checked == true)
                            {
                                //frmCommonReport ss = new frmCommonReport(_paySlipID, cmbFaculty.Text, frmCommonReport.reportType.CIT);
                                frmCommonReport ss = new frmCommonReport(settings, frmCommonReport.reportType.CIT);//(_paySlipIDs, cmbFaculty.Text, frmCommonReport.reportType.CIT, chkRemaining.Checked, date, Date.ToDotNet(txtFromDate.Text), Date.ToDotNet(txtToDate.Text));
                                ss.Show();
                            }
                            else if (rbtnTAXOnePercent.Checked == true)
                            {
                                //frmCommonReport ss = new frmCommonReport(_paySlipID, cmbFaculty.Text, frmCommonReport.reportType.Tax);
                                frmCommonReport ss = new frmCommonReport(settings, frmCommonReport.reportType.TaxOnePercent);//(_paySlipIDs, cmbFaculty.Text, frmCommonReport.reportType.Tax, chkRemaining.Checked, date, Date.ToDotNet(txtFromDate.Text), Date.ToDotNet(txtToDate.Text));
                                ss.Show();
                            }
                            else if (rbtnTax15and25Pecent.Checked == true)
                            {
                                //frmCommonReport ss = new frmCommonReport(_paySlipID, cmbFaculty.Text, frmCommonReport.reportType.Tax);
                                frmCommonReport ss = new frmCommonReport(settings, frmCommonReport.reportType.Tax15and25Percent);//(_paySlipIDs, cmbFaculty.Text, frmCommonReport.reportType.Tax, chkRemaining.Checked, date, Date.ToDotNet(txtFromDate.Text), Date.ToDotNet(txtToDate.Text));
                                ss.Show();
                            }
                            else if (rbtnWelFare.Checked == true)
                            {
                                //frmCommonReport ss = new frmCommonReport(_paySlipID, cmbFaculty.Text, frmCommonReport.reportType.Tax);
                                frmCommonReport ss = new frmCommonReport(settings, frmCommonReport.reportType.Wefare);//(_paySlipIDs, cmbFaculty.Text, frmCommonReport.reportType.Wefare, chkRemaining.Checked, date, Date.ToDotNet(txtFromDate.Text), Date.ToDotNet(txtToDate.Text));
                                ss.Show();
                            }
                        }
                    }
                    else
                    {
                        _paySlipID = 0;
                        Global.Msg("There is no data for the month, " + SelItem.Value.ToString());
                    }
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cmbDate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            cmbDate.Enabled  = txtFromDate.Enabled = txtToDate.Enabled = btnFromDate.Enabled = btnToDate.Enabled = chkDate.Checked;
        }

        private void chkRemaining_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbtnSalaryList_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnFromDate_Click(object sender, EventArgs e)
        {
            try
            {
                DateString = "FROM";
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtFromDate.Text));
                frm.ShowDialog();
            }
            catch 
            {
                Global.MsgError("Date is not in correct format !");
            }
        }

        private void btnToDate_Click(object sender, EventArgs e)
        {
            try
            {
                DateString = "TO";
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtToDate.Text));
                frm.ShowDialog();
            }
            catch 
            {
                Global.MsgError("Date is not in correct format !"); 
            }
        }


        public void DateConvert(DateTime ReturnDotNetDate)
        {
            if (DateString == "FROM")
            {
                txtFromDate.Text = Date.ToSystem(ReturnDotNetDate);
            }
            else
            {
                txtToDate.Text = Date.ToSystem(ReturnDotNetDate);
            }
        }
    }
}
