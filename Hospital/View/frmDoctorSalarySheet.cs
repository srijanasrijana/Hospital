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
using BusinessLogic.HOS;
using System.Data.SqlClient;

namespace Hospital.View
{
    public partial class frmDoctorSalarySheet : Form, IfrmDateConverter, IPaySlip
    {
        public frmDoctorSalarySheet()
        {
            InitializeComponent();
        }
        private bool isChkFestiveChanged = false;
        private int _paySlipID = 0;
        private bool isJournalEdit = false;
        bool IsDeactivateFestive = false;
        private DataTable dTableDeduction;
        private DataRow[] drFoundDeduction;
        private DataRow[] drFound;
        private DataTable dTable;
        private string FilterString = "";
        private bool isSavedData = false;
        bool isPresenceChanged = false;
        DataTable dtLoanNames = null;
        int CurrRowPos = 0;
        DataTable dtSalaryAdjust = null;
        string curInsurenceAmt = "0";

        BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
        SourceGrid.Cells.Controllers.CustomEvents evtPresenceValueChanged = new SourceGrid.Cells.Controllers.CustomEvents();
        private void frmDoctorSalarySheet_Load(object sender, EventArgs e)
        {
            Doctor.UpdateDoctorGrade();
            dtSalaryAdjust = InitSalaryAdjsutTable();           
            LoadFaculty();
            LoadDepartment();
            LoadMonths();
            LoadYears();

            cmbMonth_SelectedIndexChanged(sender, e);
            evtPresenceValueChanged.ValueChanged += new EventHandler(PresenceChanged_ValueChanged);
        }

        private void PresenceChanged_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!isPresenceChanged)
                {
                    SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
                    CurrRowPos = ctx.Position.Row;

                    string EmployeeID = grdPaySlips[CurrRowPos, (int)GridColumns.DoctorID2].Value.ToString();
                    DataRow[] dataRows = dtSalaryAdjust.Select("[DoctorID] = " + EmployeeID);
                    if (dataRows.Count() > 0)
                    {
                        if (Global.MsgQuest("If you change the presence of doctor, adjusted salary info will be deleted ! \n Are you sure you want to keep the changes ?") == System.Windows.Forms.DialogResult.No)
                        {
                            isPresenceChanged = true;
                            grdPaySlips[CurrRowPos, (int)GridColumns.Presence32].Value = true;

                            return;
                        }
                        foreach (DataRow dataRow in dataRows)
                        {
                            dtSalaryAdjust.Rows.Remove(dataRow);
                        }

                        cmbMonth_SelectedIndexChanged(null, null);
                    }
                }

                else
                {
                    isPresenceChanged = false;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        public DataTable InitSalaryAdjsutTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DoctorID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));

            return dt;
        }
        private void LoadDepartment()
        {
            DataTable dtData = null;
            dtData = Hos.getDepartmentCmb();
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
            dtData = Hos.GetFacultyByCmb();
            DataRow dr = dtData.NewRow();
           
            if (dtData.Rows.Count > 0)
            {
                cmbFaculty.DataSource = null;
                cmbFaculty.DataSource = dtData;
                cmbFaculty.DisplayMember = "Value";
                cmbFaculty.ValueMember = "ID";
                cmbFaculty.SelectedIndex = 0;
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

        private void LoadMonths()
        {
            cmbMonth.Items.Clear();
            //Check Fiscal year(By default in English)
            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();
            //get first month from start fiscal date
            
                        
            DateTime start = Convert.ToDateTime(CompDetails.FYFrom); //English fiscal year

            ListItem[] ListDate = new ListItem[12];
            for (int month = 0; month < 12; month++)
            {
                ListDate[month] = new ListItem();
                ListDate[month].ID = month + 1;
                ListDate[month].Value = Date.GetMonthList((Date.DateType)Date.DefaultDate, Language.LanguageType.English)[month + 1];

            }
         
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


        private void btnpay_Click(object sender, EventArgs e)
        {
       
            try
            {
                if (Global.MsgQuest(" तपाइँले  " + cmbMonth.Text + "  महिनाको तलब बनाउँदै हुनुहुन्छ । के यो साँचो हो? \n यदि ठिक बटन थिच्नुभयो भने त्यसको जिम्मेवार तपाई  आफैँ हुनुपर्ने छ । \n  ठिक वा बेठिक बटन थिच्नुहोस् ।") != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                //bool chkUserPermission = UserPermission.ChkUserPermission("HRM_SALARY_SAVE");
                //if (chkUserPermission == false)
                //{
                //    Global.MsgError("Sorry! you dont have permission to save salary sheet. Please contact your administrator for permission.");
                //    return;
                //}
                BusinessLogic.HRM.paySlipDetails ps = new BusinessLogic.HRM.paySlipDetails();
                CompanyDetails CompDetails = new CompanyDetails();
                CompDetails = CompanyInfo.GetInfo();
                DateTime FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);
                //Details For Saving in Master
                ListItem li = (ListItem)cmbMonth.SelectedItem;
                if (cmbMonth.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select the Month First");
                    return;
                }
                int ID = li.ID;
                string monthName1 = li.Value;
                DateTime Created_Date = System.Convert.ToDateTime(Date.GetServerDate());
                string Created_By = User.CurrentUserName;
                string Modified_by = User.CurrentUserName;
                string selectedYear = "";
                if (cmbYear.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select the year first");
                    return;
                }
                if(cmbDepartment.SelectedIndex == -1)
                {
                    Global.Msg("Please select a Department");
                    return;
                }
                if(cmbFaculty.SelectedIndex == -1)
                {
                    Global.Msg("Please select a Faculty");
                    return;
                }
                selectedYear = cmbYear.SelectedItem.ToString();
                int DepartID = 0;
                DepartID = Convert.ToInt32(cmbDepartment.SelectedValue);
                int FacultyID = 0;
                FacultyID = Convert.ToInt32(cmbFaculty.SelectedValue);

                DataTable dtLoan = new DataTable();
                dtLoan.Columns.Add("DoctorID");
                dtLoan.Columns.Add("LoanID");
                //dtLoan.Columns.Add("PaySlipID");
                dtLoan.Columns.Add("LoanMthPremium");
               

                //For MAsterPaySlip
                DataTable dtabledesignation = new DataTable();
                DataTable dtEmployeeMasterDetails = new DataTable();
                dtEmployeeMasterDetails.Columns.Add("doctorID");
                dtEmployeeMasterDetails.Columns.Add("employeeCode");
                dtEmployeeMasterDetails.Columns.Add("employeeName");
                dtEmployeeMasterDetails.Columns.Add("designationID");
                dtEmployeeMasterDetails.Columns.Add("empLevel");
                dtEmployeeMasterDetails.Columns.Add("basicSalary");
                dtEmployeeMasterDetails.Columns.Add("grade");
                dtEmployeeMasterDetails.Columns.Add("gradeAmount");
                dtEmployeeMasterDetails.Columns.Add("pfAmount");
                dtEmployeeMasterDetails.Columns.Add("pensionfAmount");
                dtEmployeeMasterDetails.Columns.Add("inflationAlw");
                dtEmployeeMasterDetails.Columns.Add("admAlw");
                dtEmployeeMasterDetails.Columns.Add("academicAlw");
                dtEmployeeMasterDetails.Columns.Add("postAlw");
                dtEmployeeMasterDetails.Columns.Add("festivalAlw");
                dtEmployeeMasterDetails.Columns.Add("miscAllowance");
                dtEmployeeMasterDetails.Columns.Add("overTimeAlw");

                dtEmployeeMasterDetails.Columns.Add("grossAmount");
                dtEmployeeMasterDetails.Columns.Add("pfDeduct");
                dtEmployeeMasterDetails.Columns.Add("pensionfDeduct");
                dtEmployeeMasterDetails.Columns.Add("taxDeduct");
                dtEmployeeMasterDetails.Columns.Add("KKDeduct");
                dtEmployeeMasterDetails.Columns.Add("NLKDeduct");
                dtEmployeeMasterDetails.Columns.Add("accommodation");
                dtEmployeeMasterDetails.Columns.Add("electricity");
                dtEmployeeMasterDetails.Columns.Add("loan");
                dtEmployeeMasterDetails.Columns.Add("advance");
                dtEmployeeMasterDetails.Columns.Add("MiscDeduct");
                dtEmployeeMasterDetails.Columns.Add("totalDedcut");
                dtEmployeeMasterDetails.Columns.Add("netSalary");
                dtEmployeeMasterDetails.Columns.Add("EmpPresence");
                dtEmployeeMasterDetails.Columns.Add("OnePercentTax");
                dtEmployeeMasterDetails.Columns.Add("PFAdjust");
                dtEmployeeMasterDetails.Columns.Add("PensionAdjust");
                dtEmployeeMasterDetails.Columns.Add("InsuranceAmt");

                if(_paySlipID > 0)
                {
                    dtEmployeeMasterDetails.Columns.Add("PrimaryID");
                }
                //bool checkEmpSelect = false;
                int currPos = 0;
                //for (int i = 2; i <= drFoundemployee.Count()+1; i++)
                //{
                for (int i = 3; i <= drFound.Count() + 2; i++)
                {
                    currPos = i;
                    string st = grdPaySlips[i, 0].Value.ToString();
                    if (grdPaySlips[i, 0].Value.ToString() == "True")
                    {
                        //checkEmpSelect = true;

                        ps.doctorID = Convert.ToInt32(grdPaySlips[i, (int)GridColumns.DoctorID2].Value);
                        ps.employeeCode = grdPaySlips[i, (int)GridColumns.Code3].ToString();
                        ps.employeeName = grdPaySlips[i, (int)GridColumns.Name4].ToString();
                        dtabledesignation = Doctor.getSpecilizationByName(grdPaySlips[i, (int)GridColumns.Designation5].ToString());
                        DataRow drdesignation = dtabledesignation.Rows[0];
                        ps.designationID = Convert.ToInt32(drdesignation["SpecilizationID"]);
                        ps.empLevel = grdPaySlips[i, (int)GridColumns.Level6].ToString();
                        ps.basicSalary = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Basic_Salary7].ToString());
                        ps.grade = Convert.ToInt32(grdPaySlips[i, (int)GridColumns.Grade8].Value);
                        ps.gradeAmount = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Grade_Amt9].ToString());
                        ps.pfAmount = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.PF_Add10].ToString());
                        ps.pensionfAmount = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Pension_Add11].ToString());
                        ps.inflationAlw = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Allow_Dearness12].ToString());
                        ps.admAlw = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Allow_Administrative13].ToString());
                        ps.academicAlw = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Allow_Academic14].ToString());
                        ps.postAlw = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Allow_General15].ToString());
                        ps.festivalAlw = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Allow_Festival16].ToString());
                        ps.miscAllowance = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Allow_Misc17].ToString());
                        ps.overTimeAlw = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Allow_OverTime177].ToString());
                        ps.grossAmount = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Gross_Amt18].ToString());
                        ps.pfDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.PF_Deduct19].ToString());
                        ps.pensionfDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.PensionF_Deduct20].ToString());
                        ps.taxDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Tax_Deduct28].ToString());
                        ps.KKDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Employee_Welfare21].ToString());
                        ps.NLKoshDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.CIT22].ToString());
                        ps.accommodation = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.QC_Accomodation23].ToString());
                        ps.electricity = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.QC_Electricity24].ToString());
                        ps.InsuranceAmt = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Insurence_Amt30].ToString());
                        decimal loanPremium = 0;
                        string presence = grdPaySlips[i, (int)GridColumns.Presence32].ToString();

                        if (presence == "True")
                        {
                            for (int c = 0; c < 5; c++)
                            {
                                loanPremium = Convert.ToDecimal(grdPaySlips[i, ((int)GridColumns.Loan1_255 + c)].ToString());
                                if (loanPremium > 0)
                                {
                                    dtLoan.Rows.Add(Convert.ToInt32(grdPaySlips[i, (int)GridColumns.DoctorID2].Value),
                                                    Convert.ToInt32(grdPaySlips[2, ((int)GridColumns.Loan1_255 + c)].Value),
                                                    loanPremium
                                                    );
                                }
                            } 
                        }
                        ps.loan = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Loan_Deduct25].ToString());
                        ps.advanceDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Advance_Deduct26].ToString());
                        ps.MiscDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Misc_Deduct27].ToString());
                        ps.totalDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Total_Deduct29].ToString());
                        ps.netSalary = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Net_Salary31].ToString());
                        ps.EmpPresence = Convert.ToBoolean(grdPaySlips[i, (int)GridColumns.Presence32].Value);
                        ps.OnePercentTax = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.OnePercentTax35].Value.ToString());
                        ps.PensionAdjust = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.PensionAdjust].Value.ToString());
                        ps.PFAdjust = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.PFAdjust].Value.ToString());
                        if (_paySlipID > 0)
                        {
                            ps.PrimaryID = Convert.ToInt32(grdPaySlips[i, (int)GridColumns.PrimaryID33].Value);
                            dtEmployeeMasterDetails.Rows.Add(ps.employeeID, ps.employeeCode, ps.employeeName, ps.designationID, ps.empLevel, ps.basicSalary, ps.grade, ps.gradeAmount,
                                ps.pfAmount, ps.pensionfAmount, ps.inflationAlw, ps.admAlw, ps.academicAlw, ps.postAlw, ps.festivalAlw, ps.miscAllowance,ps.overTimeAlw, ps.grossAmount,
                                ps.pfDeduct, ps.pensionfDeduct, ps.taxDeduct, ps.KKDeduct, ps.NLKoshDeduct, ps.accommodation, ps.electricity, ps.loan, ps.advanceDeduct, ps.MiscDeduct,
                                ps.totalDeduct, ps.netSalary, ps.EmpPresence, ps.OnePercentTax,ps.PFAdjust,ps.PensionAdjust,ps.InsuranceAmt, ps.PrimaryID);
                        }
                        else
                        {
                            dtEmployeeMasterDetails.Rows.Add(ps.doctorID, ps.employeeCode, ps.employeeName, ps.designationID, ps.empLevel, ps.basicSalary, ps.grade, ps.gradeAmount,
                                ps.pfAmount, ps.pensionfAmount, ps.inflationAlw, ps.admAlw, ps.academicAlw, ps.postAlw, ps.festivalAlw, ps.miscAllowance, ps.overTimeAlw, ps.grossAmount, ps.pfDeduct, ps.pensionfDeduct,
                                ps.taxDeduct, ps.KKDeduct, ps.NLKoshDeduct, ps.accommodation, ps.electricity, ps.loan, ps.advanceDeduct, ps.MiscDeduct, 
                                ps.totalDeduct, ps.netSalary, ps.EmpPresence, ps.OnePercentTax, ps.PFAdjust,ps.PensionAdjust,ps.InsuranceAmt);
                        }
                                                                                                                                                                     


                    }

                }
                decimal tSalary = 0, tGrade = 0, tPF = 0,tPensionF=0, tInflationAlw = 0, tAdmAlw = 0,tAcademicAlw = 0,tPostAlw=0,tFestivalAlw=0, tMiscAllow = 0,tOverTimeAlw =0, tGrossAmount = 0, tPFDeduct = 0,tPensionFDeduct=0, tTaxDeduct = 0, tKK = 0, tNLK = 0, tAccommodation = 0, tElectricity = 0, tLoan = 0, tAdvance = 0, tMiscDeduct = 0, tTotalDeduct = 0, tNetSalary = 0, tOnePercentTax =0,tPensionAdjust =0, tPFAdjust =0, tInsuranceAmt =0;
                //assign variables with total row                                                                                                                                                                                                                                           
                currPos = currPos + 1;
                tSalary = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Basic_Salary7].ToString());
                tGrade = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Grade_Amt9].ToString());
                tPF = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.PF_Add10].ToString());
                tPensionF = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Pension_Add11].ToString());
                tInflationAlw = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Allow_Dearness12].ToString());
                tAdmAlw = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Allow_Administrative13].ToString());
                tAcademicAlw = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Allow_Academic14].ToString());
                tPostAlw = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Allow_General15].ToString());
                tFestivalAlw = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Allow_Festival16].ToString());
                tMiscAllow = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Allow_Misc17].ToString());
                tOverTimeAlw = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Allow_OverTime177].ToString());

                tGrossAmount = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Gross_Amt18].ToString());
                tPFDeduct = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.PF_Deduct19].ToString());
                tPensionFDeduct = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.PensionF_Deduct20].ToString());
                
                tKK = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Employee_Welfare21].ToString());
                tNLK = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.CIT22].ToString());
                tAccommodation = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.QC_Accomodation23].ToString());
                tElectricity = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.QC_Electricity24].ToString());
                tLoan = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Loan_Deduct25].ToString());
                tAdvance = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Advance_Deduct26].ToString());
                tMiscDeduct = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Misc_Deduct27].ToString());
                tTaxDeduct = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Tax_Deduct28].ToString());
                tTotalDeduct = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Total_Deduct29].ToString());
                tNetSalary = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Net_Salary31].ToString());

                tOnePercentTax = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.OnePercentTax35].ToString());
                tPensionAdjust = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.PensionAdjust].ToString());
                tPFAdjust = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.PFAdjust].ToString());
                tInsuranceAmt = Convert.ToDecimal(grdPaySlips[currPos, (int)GridColumns.Insurence_Amt30].ToString());
                
               
                if (_paySlipID > 0)
                    BusinessLogic.HRM.Employee.UpdateSavedPaySlip(_paySlipID, ID, monthName1, Created_Date, Created_By, Modified_by, selectedYear, DepartID, FacultyID, 0, tSalary, tGrade, tPF, tPensionF, tInflationAlw, tAdmAlw, tAcademicAlw, tPostAlw, tFestivalAlw, tMiscAllow, tGrossAmount, tPFDeduct, tPensionFDeduct, tTaxDeduct, tKK, tNLK, tAccommodation, tElectricity, tLoan, tAdvance, tMiscDeduct, tTotalDeduct, tNetSalary, chkFestiveMonth.Checked, txtnarration.Text.Trim(), dtEmployeeMasterDetails, tOverTimeAlw, tOnePercentTax, tPFAdjust, tPensionAdjust, dtLoan,tInsuranceAmt, dtSalaryAdjust);//, dtallowances, dtdeduct);
                else
                   Doctor.SavePaySlip(0, ID, monthName1, Created_Date, Created_By, Modified_by, selectedYear, DepartID, FacultyID, 0, tSalary, tGrade, tPF, tPensionF, tInflationAlw, tAdmAlw, tAcademicAlw, tPostAlw, tFestivalAlw, tMiscAllow, tGrossAmount, tPFDeduct, tPensionFDeduct, tTaxDeduct, tKK, tNLK, tAccommodation, tElectricity, tLoan, tAdvance, tMiscDeduct, tTotalDeduct, tNetSalary, chkFestiveMonth.Checked, txtnarration.Text.Trim(), dtEmployeeMasterDetails, tOverTimeAlw, tOnePercentTax, tPFAdjust, tPensionAdjust, dtLoan, tInsuranceAmt, dtSalaryAdjust);//, dtallowances, dtdeduct);
                //Post The Journal For Respective Transaction
                //Load the saved value and other changes
                LoadSavedGrid();
                dtSalaryAdjust = InitSalaryAdjsutTable();
               
            }
            catch (SqlException ex)
            {
                Global.MsgError(ex.Message + " " + ex.LineNumber);
                dtSalaryAdjust.Columns.Remove("PaySlipID");
            } 
        }
        

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void DateConvert(DateTime ReturnDotNetDate)
        {
            txtdate.Text = Date.ToSystem(ReturnDotNetDate);
        }

        public void InsertJournalId(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteJournalId(int journalId)
        {
            throw new NotImplementedException();
        }
        private void LoadSavedGrid()
        {
            try
            {
                if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1 && cmbDepartment.SelectedIndex != -1 && cmbFaculty.SelectedIndex != -1)
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
                    DataTable dt = Employee.GetPayslipMaster(month, year, departmentID, FacultyID, false);


                    // load employee list for easier search functionality
                    DataTable dtEmployeeList = Doctor.GetDoctorList(FacultyID);
                    cmbEmployeeList.DataSource = null;
                    cmbEmployeeList.DataSource = dtEmployeeList;
                    cmbEmployeeList.DisplayMember = "Name";
                    cmbEmployeeList.ValueMember = "ID";


                    if (dt.Rows.Count > 0)
                    {
                        chkByCash.Checked = false;
                        _paySlipID = Convert.ToInt16(dt.Rows[0]["salaryPaySlipID"].ToString());
                        if (Convert.ToInt16(dt.Rows[0]["isVoucherEntered"].ToString()) == 0)
                        {
                            //LoadSavedSalary(_paySlipID);
                            lblMsg.Text = "Voucher entry for this month's salary sheet is pending.";
                            LoadForm();
                            btnpay.Enabled = true;
                            btnJournalEntry.Enabled = true;
                            grpPosting.Enabled = true;
                            chkJournalPosting.Checked = true;
                            isJournalEdit = true;
                            txtnarration.Enabled = true;
                            if (dt.Rows[0]["IsFestiveMonth"].ToString() == "True" && !isChkFestiveChanged)
                                chkFestiveMonth.Checked = true;
                            else if (dt.Rows[0]["IsFestiveMonth"].ToString() == "False" && !isChkFestiveChanged)
                                chkFestiveMonth.Checked = false;
                            if (IsDeactivateFestive)
                                chkFestiveMonth.Enabled = true;

                            departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                            int facultyID = 0;
                            facultyID = Convert.ToInt32(cmbFaculty.SelectedValue);

                            dTable = Doctor.DoctorsalaryDetails(departmentID, FacultyID, _paySlipID);
                            drFound = dTable.Select(FilterString);

                            dTableDeduction = employees.getDeductionOnly();
                            drFoundDeduction = dTableDeduction.Select(FilterString);

                            fillGrid();
                        }
                        else
                        {
                           LoadSavedSalary(_paySlipID);
                            lblMsg.Text = "Voucher entry has been done for this month's salary sheet. Thus, the salary sheet can not be modified.";
                            btnpay.Enabled = false;
                            grpPosting.Enabled = false;
                            //btnJournalEntry.Enabled = false;
                            isJournalEdit = false;
                            txtnarration.Enabled = false;
                            if (dt.Rows[0]["IsFestiveMonth"].ToString() == "True")
                                chkFestiveMonth.Checked = true;
                            else
                                chkFestiveMonth.Checked = false;
                            chkFestiveMonth.Enabled = false;

                        }
                    }
                    else
                    {
                        _paySlipID = 0;
                        LoadForm();
                        lblMsg.Text = "Salary sheet for the month is not saved.";
                        btnpay.Enabled = true;
                        btnJournalEntry.Enabled = true;
                        txtnarration.Clear();
                        txtnarration.Enabled = true;
                        if (IsDeactivateFestive)
                            chkFestiveMonth.Enabled = true;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadSavedSalary(int paySlipId)
        {
            isSavedData = true;
            //These tabs are removed for bisal bazars
            tabControl1.TabPages.Remove(tpadditionalallowances);
            tabControl1.TabPages.Remove(tpotherdeduction);

            txtdate.Mask = Date.FormatToMask();
            txtdate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            LoadComboboxItems(cmbBankName, !chkByCash.Checked);

            dTable = employees.savedSalaryDetails(paySlipId);
            drFound = dTable.Select(FilterString);


            fillGrid();
            DataTable dtN = employees.GetSalaryMaster(paySlipId);
            //DataTable dtN = employees.salaryDetails(0, 0, paySlipId);
            txtnarration.Text = dtN.Rows[0]["Narration"].ToString();
        }
        private void LoadForm()
        {
            chkFestiveMonth.Checked = false;
            isSavedData = false;
            //These tabs are removed for bisal bazars
            tabControl1.TabPages.Remove(tpadditionalallowances);
            tabControl1.TabPages.Remove(tpotherdeduction);

            txtdate.Mask = Date.FormatToMask();
            txtdate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            LoadComboboxItems(cmbBankName, true);

            int departmentID = 0;
            departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
            int facultyID = 0;
            facultyID = Convert.ToInt32(cmbFaculty.SelectedValue);

            dTable = Doctor.DoctorsalaryDetails(departmentID, facultyID, _paySlipID);
            drFound = dTable.Select(FilterString);

            dTableDeduction = employees.getDeductionOnly();
            drFoundDeduction = dTableDeduction.Select(FilterString);


            fillGrid();



        }
        public enum GridColumns
        {
            SN1 = 1, DoctorID2, Code3, Name4, Designation5, Level6, Basic_Salary7, Grade8, Grade_Amt9, PF_Add10,
            Pension_Add11, Allow_Dearness12, Allow_Administrative13, Allow_Academic14, Allow_General15, Allow_Festival16, Allow_Misc17,
            Allow_OverTime177, Gross_Amt18, PF_Deduct19, PensionF_Deduct20,
            Employee_Welfare21, CIT22, QC_Accomodation23, QC_Electricity24, Loan1_255, Loan2_255, Loan3_255, Loan4_255, Loan5_255, 
            Loan_Deduct25, Advance_Deduct26, Misc_Deduct27, Tax_Deduct28, Total_Deduct29, Insurence_Amt30, Net_Salary31,
            Presence32, PrimaryID33, Starting_Salary34, OnePercentTax35, PFAdjust, PensionAdjust
        }
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
                backHeader.BackColor = ColorTranslator.FromHtml("#e6f7ff");
                backHeader.Border = DevAge.Drawing.RectangleBorder.RectangleBlack1Width;
                view.Background = backHeader;
                view.Font = new Font("Arial", 9, FontStyle.Regular);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;
                AutomaticSortEnabled = false;
            }
        }
        private void writeHeader()
        {
            grdPaySlips[0, 0] = new MyHeader("");
            grdPaySlips[0, 0].RowSpan = 2;
            grdPaySlips[0, 0].Column.Width = 30;
            grdPaySlips[0, 0].Column.Visible = false;//checkbox hidden for bishal bazar

            grdPaySlips[0, (int)GridColumns.SN1] = new MyHeader("SN");
            grdPaySlips[0, (int)GridColumns.SN1].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.SN1].Column.Width = 30;
           
            grdPaySlips[0, (int)GridColumns.DoctorID2] = new MyHeader("Doctor ID");
            grdPaySlips[0, (int)GridColumns.DoctorID2].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.DoctorID2].Column.Width = 1;
            grdPaySlips[0, (int)GridColumns.DoctorID2].Column.Visible = false;//Doctor ID

            grdPaySlips[0, (int)GridColumns.Code3] = new MyHeader("Code");
            grdPaySlips[0, (int)GridColumns.Code3].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Code3].Column.Width = 40;
            grdPaySlips[0, (int)GridColumns.Code3].Column.Visible = false;
           
            grdPaySlips[0, (int)GridColumns.Name4] = new MyHeader("Name");
            grdPaySlips[0, (int)GridColumns.Name4].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Name4].Column.Width = 120;

            grdPaySlips[0, (int)GridColumns.Designation5] = new MyHeader("Specilization");
            grdPaySlips[0, (int)GridColumns.Designation5].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Designation5].Column.Width = 100;

            grdPaySlips[0, (int)GridColumns.Level6] = new MyHeader("Level");
            grdPaySlips[0, (int)GridColumns.Level6].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Level6].Column.Width = 100;
            grdPaySlips[0, (int)GridColumns.Level6].Column.Visible = false;

            grdPaySlips[0, (int)GridColumns.Basic_Salary7] = new MyHeader("Basic Salary");
            grdPaySlips[0, (int)GridColumns.Basic_Salary7].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Basic_Salary7].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.Basic_Salary7].Column.Visible = false;

            grdPaySlips[0, (int)GridColumns.Grade8] = new MyHeader("Grade");
            grdPaySlips[0, (int)GridColumns.Grade8].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Grade8].Column.Width = 30;

            grdPaySlips[0, (int)GridColumns.Grade_Amt9] = new MyHeader("Grade Amount");
            grdPaySlips[0, (int)GridColumns.Grade_Amt9].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Grade_Amt9].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.PF_Add10] = new MyHeader("PF(Add)");
            grdPaySlips[0, (int)GridColumns.PF_Add10].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.PF_Add10].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.Pension_Add11] = new MyHeader("Pension F(Add)");
            grdPaySlips[0, (int)GridColumns.Pension_Add11].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Pension_Add11].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.Allow_Dearness12] = new MyHeader("Allowance");
            grdPaySlips[0, (int)GridColumns.Allow_Dearness12].ColumnSpan = 7;
            grdPaySlips[1, (int)GridColumns.Allow_Dearness12] = new MyHeader("Dearness");
            grdPaySlips[1, (int)GridColumns.Allow_Dearness12].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.Allow_Dearness12].Column.Visible = false;
                
            grdPaySlips[1, (int)GridColumns.Allow_Administrative13] = new MyHeader("Administrative");
            grdPaySlips[1, (int)GridColumns.Allow_Administrative13].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.Allow_Administrative13].Column.Visible = false;

            grdPaySlips[1, (int)GridColumns.Allow_Academic14] = new MyHeader("Academic");
            grdPaySlips[1, (int)GridColumns.Allow_Academic14].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.Allow_Academic14].Column.Visible = false;
         
            grdPaySlips[1, (int)GridColumns.Allow_General15] = new MyHeader("General");
            grdPaySlips[1, (int)GridColumns.Allow_General15].Column.Width = 80;
         
            grdPaySlips[1, (int)GridColumns.Allow_Festival16] = new MyHeader("Festival");
            grdPaySlips[1, (int)GridColumns.Allow_Festival16].Column.Width = 80;

            grdPaySlips[1, (int)GridColumns.Allow_Misc17] = new MyHeader("Miscellaneous");
            grdPaySlips[1, (int)GridColumns.Allow_Misc17].Column.Width = 80;
         
            grdPaySlips[1, (int)GridColumns.Allow_OverTime177] = new MyHeader("OverTime");

            grdPaySlips[0, (int)GridColumns.Gross_Amt18] = new MyHeader("Gross Amount");
            grdPaySlips[0, (int)GridColumns.Gross_Amt18].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Gross_Amt18].Column.Width = 100;

            grdPaySlips[0, (int)GridColumns.PF_Deduct19] = new MyHeader("PF (Deduct)");
            grdPaySlips[0, (int)GridColumns.PF_Deduct19].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.PF_Deduct19].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.PensionF_Deduct20] = new MyHeader("Pension F(Deduct)");
            grdPaySlips[0, (int)GridColumns.PensionF_Deduct20].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.PensionF_Deduct20].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.Employee_Welfare21] = new MyHeader("Doctor Welfare");
            grdPaySlips[0, (int)GridColumns.Employee_Welfare21].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Employee_Welfare21].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.CIT22] = new MyHeader("CIT");
            grdPaySlips[0, (int)GridColumns.CIT22].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.CIT22].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.QC_Accomodation23] = new MyHeader("Quarter Charge");
            grdPaySlips[0, (int)GridColumns.QC_Accomodation23].ColumnSpan = 2;
            grdPaySlips[1, (int)GridColumns.QC_Accomodation23] = new MyHeader("Accommodation");
            grdPaySlips[1, (int)GridColumns.QC_Accomodation23].Column.Width = 80;
          
            grdPaySlips[1, (int)GridColumns.QC_Electricity24] = new MyHeader("Electricity");
            grdPaySlips[1, (int)GridColumns.QC_Electricity24].Column.Width = 80;
                     
            dtLoanNames = Hrm.GetLoanForCmb();
            dtLoanNames.DefaultView.Sort = "ID";  // sort the datatable according to MasterID in ascending order
            dtLoanNames = dtLoanNames.DefaultView.ToTable();

            grdPaySlips[0, (int)GridColumns.Loan1_255] = new MyHeader("Loan");
            grdPaySlips[0, (int)GridColumns.Loan1_255].ColumnSpan = 6;
            grdPaySlips[1, (int)GridColumns.Loan1_255] = new MyHeader(dtLoanNames.Rows[0]["Value"].ToString());
            grdPaySlips[2, (int)GridColumns.Loan1_255] = new MyHeader(dtLoanNames.Rows[0]["ID"].ToString());
            grdPaySlips[2, (int)GridColumns.Loan1_255].Row.Visible = false;
            grdPaySlips[1, (int)GridColumns.Loan1_255].Column.Width = 70;

            grdPaySlips[1, (int)GridColumns.Loan2_255] = new MyHeader(dtLoanNames.Rows[1]["Value"].ToString());
            grdPaySlips[2, (int)GridColumns.Loan2_255] = new MyHeader(dtLoanNames.Rows[2]["ID"].ToString());
            grdPaySlips[1, (int)GridColumns.Loan2_255].Column.Width = 70;

            grdPaySlips[1, (int)GridColumns.Loan3_255] = new MyHeader(dtLoanNames.Rows[2]["Value"].ToString());
            grdPaySlips[2, (int)GridColumns.Loan3_255] = new MyHeader(dtLoanNames.Rows[2]["ID"].ToString());
            grdPaySlips[1, (int)GridColumns.Loan3_255].Column.Width = 70;

            grdPaySlips[1, (int)GridColumns.Loan4_255] = new MyHeader(dtLoanNames.Rows[3]["Value"].ToString());
            grdPaySlips[2, (int)GridColumns.Loan4_255] = new MyHeader(dtLoanNames.Rows[3]["ID"].ToString());
            grdPaySlips[1, (int)GridColumns.Loan4_255].Column.Width = 70;

            grdPaySlips[1, (int)GridColumns.Loan5_255] = new MyHeader(dtLoanNames.Rows[4]["Value"].ToString());
            grdPaySlips[2, (int)GridColumns.Loan5_255] = new MyHeader(dtLoanNames.Rows[4]["ID"].ToString());
            grdPaySlips[1, (int)GridColumns.Loan5_255].Column.Width = 70;

            grdPaySlips[1, (int)GridColumns.Loan_Deduct25] = new MyHeader("Total Loan Deduction");
            grdPaySlips[1, (int)GridColumns.Loan_Deduct25].Column.Width = 70;
            grdPaySlips[0, (int)GridColumns.Loan_Deduct25].Column.Width = 80;
        
            grdPaySlips[0, (int)GridColumns.Advance_Deduct26] = new MyHeader("Advance Deduction");
            grdPaySlips[0, (int)GridColumns.Advance_Deduct26].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Advance_Deduct26].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.Misc_Deduct27] = new MyHeader("Misc Deduction");
            grdPaySlips[0, (int)GridColumns.Misc_Deduct27].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Misc_Deduct27].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.Tax_Deduct28] = new MyHeader("Tax Deduction");
            grdPaySlips[0, (int)GridColumns.Tax_Deduct28].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Tax_Deduct28].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.Total_Deduct29] = new MyHeader("Total Deduction");
            grdPaySlips[0, (int)GridColumns.Total_Deduct29].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Total_Deduct29].Column.Width = 100;

            grdPaySlips[0, (int)GridColumns.Insurence_Amt30] = new MyHeader("Insurence Amt");
            grdPaySlips[0, (int)GridColumns.Insurence_Amt30].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.Net_Salary31] = new MyHeader("Net Salary");
            grdPaySlips[0, (int)GridColumns.Net_Salary31].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Net_Salary31].Column.Width = 100;

            grdPaySlips[0, (int)GridColumns.Presence32] = new MyHeader("Presence");
            grdPaySlips[0, (int)GridColumns.Presence32].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Presence32].Column.Width = 70;

            grdPaySlips[0, (int)GridColumns.PrimaryID33] = new MyHeader("PrimaryID");
            grdPaySlips[0, (int)GridColumns.PrimaryID33].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.PrimaryID33].Column.Width = 70;
            grdPaySlips[1, (int)GridColumns.PrimaryID33].Column.Visible = false;

            grdPaySlips[0, (int)GridColumns.Starting_Salary34] = new MyHeader("StartingSalary");
            grdPaySlips[0, (int)GridColumns.Starting_Salary34].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Starting_Salary34].Column.Visible = false;

            grdPaySlips[0, (int)GridColumns.OnePercentTax35] = new MyHeader("One PercentTax");
            grdPaySlips[0, (int)GridColumns.OnePercentTax35].RowSpan = 2;
        
            grdPaySlips[0, (int)GridColumns.PensionAdjust] = new MyHeader("PensionAdjust");
            grdPaySlips[0, (int)GridColumns.PensionAdjust].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.PFAdjust] = new MyHeader("PFAdjust");
            grdPaySlips[0, (int)GridColumns.PFAdjust].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.OnePercentTax35].Column.Visible =grdPaySlips[0, (int)GridColumns.PFAdjust].Column.Visible =
            grdPaySlips[0, (int)GridColumns.PensionAdjust].Column.Visible = false;       

        }
        private void fillGrid()
        {
            ListItem SelItem = (ListItem)cmbMonth.SelectedItem;


            grdPaySlips.Rows.Clear();
            grdPaySlips.Redim(drFound.Count() + 4, 45);
            grdPaySlips.FixedRows = 2;
            grdPaySlips.FixedColumns = 5;
            writeHeader();
            if (drFound.Count() == 0)
                return;
           
            SourceGrid.Cells.Views.Cell AlternateColor = new SourceGrid.Cells.Views.Cell();

            try
            {
                #region
                for (int i = 3; i <= drFound.Count() + 2; i++)
                {
                    CurrRowPos = i;
                    if (i % 2 == 0)
                        AlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    else
                        AlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);

                    DataRow dr = drFound[i - 3];
                    SourceGrid.Cells.CheckBox checkView = new SourceGrid.Cells.CheckBox("", true);

                    grdPaySlips[i, 0] = checkView;

                    grdPaySlips[i, (int)GridColumns.SN1] = new SourceGrid.Cells.Cell((i - 2).ToString());
                    grdPaySlips[i, (int)GridColumns.SN1].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    grdPaySlips[i, (int)GridColumns.DoctorID2] = new SourceGrid.Cells.Cell(dr["DoctorID"].ToString());


                    grdPaySlips[i, (int)GridColumns.Code3] = new SourceGrid.Cells.Cell(dr["DoctorCode"].ToString());
                    grdPaySlips[i, (int)GridColumns.Code3].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    grdPaySlips[i, (int)GridColumns.Name4] = new SourceGrid.Cells.Cell(dr["doctorname"].ToString());
                    grdPaySlips[i, (int)GridColumns.Name4].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    grdPaySlips[i, (int)GridColumns.Designation5] = new SourceGrid.Cells.Cell(dr["SpecilizationName"].ToString());
                    grdPaySlips[i, (int)GridColumns.Designation5].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    grdPaySlips[i, (int)GridColumns.Level6] = new SourceGrid.Cells.Cell(isSavedData ? dr["Level"].ToString() : dr["LevelName"].ToString());
                    grdPaySlips[i, (int)GridColumns.Level6].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal basicSalary = 0;
                    if (!dr.IsNull("BasicSalary"))
                        basicSalary = Convert.ToDecimal(dr["BasicSalary"]);

                    object AdjustSum = null;
                    if (dtSalaryAdjust.Rows.Count > 0)
                    {
                        DataRow[] drs = dtSalaryAdjust.Select("[DoctorID] = " + dr["DoctorID"]);
                        if (drs.Count() > 0)
                        {
                            AdjustSum = drs.CopyToDataTable().Compute("sum(Amount)", "");
                        }
                    }

                    if (AdjustSum != null)
                    {
                        basicSalary += Convert.ToDecimal(AdjustSum);
                    }
                    grdPaySlips[i, (int)GridColumns.Basic_Salary7] = new SourceGrid.Cells.Cell(basicSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Basic_Salary7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Basic_Salary7].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    int grade = 0;
                    if (!dr.IsNull("Grade"))
                        grade = Convert.ToInt32(dr["Grade"].ToString());

                    grdPaySlips[i, (int)GridColumns.Grade8] = new SourceGrid.Cells.Cell(grade.ToString());
                    grdPaySlips[i, (int)GridColumns.Grade8].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal gradeAmount = 0;
                    if (isSavedData)
                        gradeAmount = Convert.ToDecimal(dr["GradeAmount"].ToString());
                    else
                    {
                        decimal perGradeAmt = Convert.ToDecimal(dr["GradeAmt"].ToString());
                        gradeAmount = perGradeAmt * grade;
                    }

                    grdPaySlips[i, (int)GridColumns.Grade_Amt9] = new SourceGrid.Cells.Cell(gradeAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Grade_Amt9].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Grade_Amt9].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal pfAmount = 0, PFAdjust = 0, PensionAdjust = 0;

                    if (isSavedData)
                    {
                        pfAmount = Convert.ToDecimal(dr["PFAmount"].ToString());
                        grdPaySlips[i, (int)GridColumns.PF_Add10] = new SourceGrid.Cells.Cell(pfAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlips[i, (int)GridColumns.PF_Add10].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlips[i, (int)GridColumns.PF_Add10].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                    }
                    else
                    {
                        if (dr["IsPF"].ToString() == "False")
                        {
                            grdPaySlips[i, (int)GridColumns.PF_Add10] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                            grdPaySlips[i, (int)GridColumns.PF_Add10].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                            grdPaySlips[i, (int)GridColumns.PF_Add10].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                        }
                        else if (dr["IsPF"].ToString() == "True")
                        {

                            pfAmount = (basicSalary + gradeAmount) * (decimal)0.1;
                            PFAdjust = Convert.ToDecimal(dr["PFAdjust"].ToString());
                            pfAmount += PFAdjust;
                            grdPaySlips[i, (int)GridColumns.PF_Add10] = new SourceGrid.Cells.Cell((pfAmount).ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                            grdPaySlips[i, (int)GridColumns.PF_Add10].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                            grdPaySlips[i, (int)GridColumns.PF_Add10].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                        }
                    }

                    decimal pensionFAmt = 0;
                    if (isSavedData)
                    {
                        pensionFAmt = Convert.ToDecimal(dr["PensionFAmt"].ToString());
                        grdPaySlips[i, (int)GridColumns.Pension_Add11] = new SourceGrid.Cells.Cell(pensionFAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlips[i, (int)GridColumns.Pension_Add11].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlips[i, (int)GridColumns.Pension_Add11].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                    }
                    else
                    {
                        if (dr["isPension"].ToString() == "False")
                        {
                            grdPaySlips[i, (int)GridColumns.Pension_Add11] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                            grdPaySlips[i, (int)GridColumns.Pension_Add11].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                            grdPaySlips[i, (int)GridColumns.Pension_Add11].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                        }
                        else if (dr["isPension"].ToString() == "True")
                        {

                            pensionFAmt = (basicSalary + gradeAmount) * (decimal)0.1;
                            PensionAdjust = Convert.ToDecimal(dr["PensionAdjust"].ToString());
                            pensionFAmt += PensionAdjust;
                            PensionAdjust = Convert.ToDecimal(dr["PensionAdjust"].ToString());
                            grdPaySlips[i, (int)GridColumns.Pension_Add11] = new SourceGrid.Cells.Cell((pensionFAmt).ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                            grdPaySlips[i, (int)GridColumns.Pension_Add11].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                            grdPaySlips[i, (int)GridColumns.Pension_Add11].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                        }
                    }


                    decimal InflationAlw = 0;
                    if (!dr.IsNull("InflationAlw"))
                        InflationAlw = Convert.ToDecimal(dr["InflationAlw"].ToString());

                    grdPaySlips[i, (int)GridColumns.Allow_Dearness12] = new SourceGrid.Cells.Cell(InflationAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Allow_Dearness12].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Allow_Dearness12].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal AdmistrativeAlw = 0;
                    if (!dr.IsNull("AdmistrativeAlw"))
                        AdmistrativeAlw = Convert.ToDecimal(dr["AdmistrativeAlw"].ToString());

                    grdPaySlips[i, (int)GridColumns.Allow_Administrative13] = new SourceGrid.Cells.Cell(AdmistrativeAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Allow_Administrative13].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Allow_Administrative13].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal AcademicAlw = 0;
                    if (!dr.IsNull("AcademicAlw"))
                        AcademicAlw = Convert.ToDecimal(dr["AcademicAlw"].ToString());

                    grdPaySlips[i, (int)GridColumns.Allow_Academic14] = new SourceGrid.Cells.Cell(AcademicAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Allow_Academic14].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Allow_Academic14].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal OverTimeAlw = 0;
                    if (!dr.IsNull("OverTimeAlw"))
                        OverTimeAlw = Convert.ToDecimal(dr["OverTimeAlw"].ToString());

                    SourceGrid.Cells.Editors.TextBox txtOverTime = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                  
                    grdPaySlips[i, (int)GridColumns.Allow_OverTime177] = new SourceGrid.Cells.Cell((OverTimeAlw).ToString(Misc.FormatNumber(false, Global.DecimalPlaces)), txtOverTime);                  
                    grdPaySlips[i, (int)GridColumns.Allow_OverTime177].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Allow_OverTime177].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
              

                    decimal PostAlw = 0;
                    if (!dr.IsNull("PostAlw"))
                        PostAlw = Convert.ToDecimal(dr["PostAlw"].ToString());

                    decimal GeneralAlw = 0;
                    GeneralAlw = InflationAlw + AdmistrativeAlw + AcademicAlw + PostAlw;

          
                    grdPaySlips[i, (int)GridColumns.Allow_General15] = new SourceGrid.Cells.Cell(GeneralAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Allow_General15].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Allow_General15].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal FestivalAlw = 0;
                  

                    SourceGrid.Cells.Editors.TextBox txtMiscAllowance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    grdPaySlips[i, (int)GridColumns.Allow_Misc17] = new SourceGrid.Cells.Cell("", txtMiscAllowance);
                
                    grdPaySlips[i, (int)GridColumns.Allow_Misc17].Value = (isSavedData ? Convert.ToDecimal(dr["miscAllowance"].ToString()) : 0).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    grdPaySlips[i, (int)GridColumns.Allow_Misc17].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Allow_Misc17].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal grossAmount = 0, miscAllowance = 0, overTimeAlw = 0;

                    if (grdPaySlips[i, (int)GridColumns.Allow_OverTime177].Value.ToString() != "")
                        overTimeAlw = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Allow_OverTime177].Value);
                    grdPaySlips[i, (int)GridColumns.Allow_OverTime177].Editor.EnableEdit = false;
                    grdPaySlips[i, (int)GridColumns.Allow_Misc17].Editor.EnableEdit = false;

                    if (isSavedData && !isChkFestiveChanged)
                    {
                        grossAmount = Convert.ToDecimal(dr["grossAmount"].ToString());
                    }
                    else
                    {
                        if (grdPaySlips[i, (int)GridColumns.Allow_Misc17].Value.ToString() != "")
                            miscAllowance = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Allow_Misc17].Value);
                        grossAmount = basicSalary + gradeAmount + pfAmount + pensionFAmt + InflationAlw + AdmistrativeAlw + AcademicAlw + PostAlw + miscAllowance + FestivalAlw + overTimeAlw;
                    }
                    grdPaySlips[i, (int)GridColumns.Gross_Amt18] = new SourceGrid.Cells.Cell(grossAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Gross_Amt18].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Gross_Amt18].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal pfDedection = 0;

                    pfDedection = isSavedData ? Convert.ToDecimal(dr["pfDeduct"].ToString()) : (pfAmount * 2);
                    grdPaySlips[i, (int)GridColumns.PF_Deduct19] = new SourceGrid.Cells.Cell(pfDedection.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.PF_Deduct19].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.PF_Deduct19].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal PensionFAmtDeduct = 0;
                    PensionFAmtDeduct = isSavedData ? Convert.ToDecimal(dr["PensionFDeduct"].ToString()) : pensionFAmt * 2;
                    grdPaySlips[i, (int)GridColumns.PensionF_Deduct20] = new SourceGrid.Cells.Cell((PensionFAmtDeduct).ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.PensionF_Deduct20].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.PensionF_Deduct20].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal KalyankariAmt = 0;
                    if (!dr.IsNull("KalyankariAmt"))
                        KalyankariAmt = Convert.ToDecimal(dr["KalyankariAmt"].ToString());

                    grdPaySlips[i, (int)GridColumns.Employee_Welfare21] = new SourceGrid.Cells.Cell(KalyankariAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Employee_Welfare21].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Employee_Welfare21].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal NLKos = 0;
                    if (!dr.IsNull("NLKoshDeduct"))
                        NLKos = Convert.ToDecimal(dr["NLKoshDeduct"].ToString());

                    grdPaySlips[i, (int)GridColumns.CIT22] = new SourceGrid.Cells.Cell(NLKos.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.CIT22].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.CIT22].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal Accommodation = 0;
                    if (!dr.IsNull("Accommodation"))
                        Accommodation = Convert.ToDecimal(dr["Accommodation"].ToString());

                    grdPaySlips[i, (int)GridColumns.QC_Accomodation23] = new SourceGrid.Cells.Cell(Accommodation.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.QC_Accomodation23].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.QC_Accomodation23].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal ElectricityCharge = 0;
                    if (!dr.IsNull("ElectricityCharge"))
                        ElectricityCharge = Convert.ToDecimal(dr["ElectricityCharge"].ToString());

                    SourceGrid.Cells.Editors.TextBox txtElectricityAllowance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    grdPaySlips[i, (int)GridColumns.QC_Electricity24] = new SourceGrid.Cells.Cell("", txtElectricityAllowance);
                   
                    grdPaySlips[i, (int)GridColumns.QC_Electricity24].Value = (isSavedData ? Convert.ToDecimal(dr["ElectricityDeduct"].ToString()) : ElectricityCharge).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    grdPaySlips[i, (int)GridColumns.QC_Electricity24].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.QC_Electricity24].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal LoanMthPremium = 0;
                    if (isSavedData)
                    {
                        LoanMthPremium = Convert.ToDecimal(dr["LoanMthPremium"].ToString());
                    }
                    else
                    {
                        
                        if (!dr.IsNull("LoanPremium"))
                            LoanMthPremium = Convert.ToDecimal(dr["LoanPremium"].ToString());
                        DataTable dtLoan = Doctor.GetDoctorLoan(Convert.ToInt32(dr["DoctorID"].ToString()), true);
                        decimal totalLoan = 0;
                        grdPaySlips[i, (int)GridColumns.Loan1_255] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdPaySlips[i, (int)GridColumns.Loan2_255] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdPaySlips[i, (int)GridColumns.Loan3_255] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdPaySlips[i, (int)GridColumns.Loan4_255] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdPaySlips[i, (int)GridColumns.Loan5_255] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                        grdPaySlips[i, (int)GridColumns.Loan1_255].View =
                        grdPaySlips[i, (int)GridColumns.Loan2_255].View =
                        grdPaySlips[i, (int)GridColumns.Loan3_255].View =
                        grdPaySlips[i, (int)GridColumns.Loan4_255].View =
                        grdPaySlips[i, (int)GridColumns.Loan5_255].View =
                        new SourceGrid.Cells.Views.Cell(AlternateColor);

                        for (int count = 0; count < dtLoan.Rows.Count; count++)
                        {
                            DataRow drLoan = dtLoan.Rows[count];

                            for (int cnt = 0; cnt < 5; cnt++)
                            {
                                decimal loanAmt = Convert.ToDecimal(grdPaySlips[i, ((int)GridColumns.Loan1_255 + cnt)].Value);
                                if (drLoan["LoanName"].ToString() == grdPaySlips[1, ((int)GridColumns.Loan1_255 + cnt)].Value.ToString())
                                {
                                    loanAmt = Convert.ToDecimal(drLoan["LoanMthPremium"].ToString());
                                }

                                grdPaySlips[i, ((int)GridColumns.Loan1_255 + cnt)].Value = loanAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                               
                                totalLoan += loanAmt;

                            }
                        }
                        
                    }
                    grdPaySlips[i, (int)GridColumns.Loan_Deduct25] = new SourceGrid.Cells.Cell(LoanMthPremium.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Loan_Deduct25].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Loan_Deduct25].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal AdvMthInstallment = 0;
                    if (isSavedData)
                    {
                        AdvMthInstallment = Convert.ToDecimal(dr["AdvMthInstallment"].ToString());
                    }
                    else
                    {
                        if (!dr.IsNull("AdvInsatallment") || !dr.IsNull("AdvRemaining"))
                        {
                            if (Convert.ToDecimal(dr["AdvRemaining"].ToString()) > Convert.ToDecimal(dr["AdvInsatallment"].ToString()))
                            {
                                if (!dr.IsNull("AdvInsatallment"))
                                    AdvMthInstallment = Convert.ToDecimal(dr["AdvInsatallment"].ToString());
                            }
                            else
                            {
                                AdvMthInstallment = Convert.ToDecimal(dr["AdvRemaining"].ToString());
                            }
                        }
                    }
                    grdPaySlips[i, (int)GridColumns.Advance_Deduct26] = new SourceGrid.Cells.Cell(AdvMthInstallment.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Advance_Deduct26].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Advance_Deduct26].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    SourceGrid.Cells.Editors.TextBox txtMiscDeduct = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                 
                    grdPaySlips[i, (int)GridColumns.Misc_Deduct27] = new SourceGrid.Cells.Cell("", txtMiscDeduct);
                   
                    grdPaySlips[i, (int)GridColumns.Misc_Deduct27].Value = (isSavedData ? Convert.ToDecimal(dr["MiscDeduct"].ToString()) : 0).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    grdPaySlips[i, (int)GridColumns.Misc_Deduct27].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Misc_Deduct27].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                   
                    decimal g_taxAmt = 0;
                    g_taxAmt = isSavedData ? Convert.ToDecimal(dr["taxDeduct"].ToString()) : 0;
                    grdPaySlips[i, (int)GridColumns.Tax_Deduct28] = new SourceGrid.Cells.Cell(g_taxAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Tax_Deduct28].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Tax_Deduct28].View = new SourceGrid.Cells.Views.Cell(AlternateColor);


                    decimal totalDeduct = 0, ElectricityDeduct = 0;
                    if (isSavedData && !isChkFestiveChanged)
                    {
                        totalDeduct = Convert.ToDecimal(dr["TotalDeduct"].ToString());
                    }
                    else
                    {
                        decimal taxDeduct = 0, MiscDeduct = 0;
                        if (grdPaySlips[i, (int)GridColumns.Tax_Deduct28].Value.ToString() != "")
                            taxDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.PensionF_Deduct20].Value);
                        if (grdPaySlips[i, (int)GridColumns.Misc_Deduct27].Value.ToString() != "")
                            MiscDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.Misc_Deduct27].Value);
                        if (grdPaySlips[i, (int)GridColumns.QC_Electricity24].Value.ToString() != "")
                            ElectricityDeduct = Convert.ToDecimal(grdPaySlips[i, (int)GridColumns.QC_Electricity24].Value);

                        totalDeduct = pfDedection + PensionFAmtDeduct + KalyankariAmt + NLKos + Accommodation + LoanMthPremium + AdvMthInstallment + ElectricityDeduct;// + taxDeduct // misc deduct removed from total deduct

                        
                    }

                    decimal startingSalary = Convert.ToDecimal(dr["StartingSalary"]);
                    grdPaySlips[i, (int)GridColumns.Starting_Salary34] = new SourceGrid.Cells.Cell(startingSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));

                    if (isSavedData && !isChkFestiveChanged)
                    {
                        if (!dr.IsNull("FestivalAlw"))
                            FestivalAlw = Convert.ToDecimal(dr["FestivalAlw"].ToString());
                    }
                    else
                    {
                        if (chkFestiveMonth.Checked)
                        {
                           

                            FestivalAlw = startingSalary + gradeAmount;
                        }
                    }
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16] = new SourceGrid.Cells.Cell(FestivalAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    grdPaySlips[i, (int)GridColumns.OnePercentTax35] = new SourceGrid.Cells.Cell((0).ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.OnePercentTax35].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.OnePercentTax35].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    grdPaySlips[i, (int)GridColumns.PFAdjust] = new SourceGrid.Cells.Cell((PFAdjust).ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.PFAdjust].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.PFAdjust].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    grdPaySlips[i, (int)GridColumns.PensionAdjust] = new SourceGrid.Cells.Cell((PensionAdjust).ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.PensionAdjust].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.PensionAdjust].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    if (!isSavedData || isChkFestiveChanged)
                    {
                        decimal tempTax = CalculateTax(Convert.ToInt32(dr["DoctorID"].ToString()), ((grossAmount - basicSalary + startingSalary) + Accommodation + ElectricityDeduct + LoanMthPremium + AdvMthInstallment) - totalDeduct - startingSalary - gradeAmount - FestivalAlw - overTimeAlw, startingSalary + gradeAmount);//Accommodation,Electricity,loan,advance does not have any effect on Tax. Tax calculation based on starting salary instead of basic salary

                        //decimal tempTax = CalculateTax(Convert.ToInt32(dr["EmployeeID"].ToString()), (grossAmount + Accommodation + ElectricityDeduct + LoanMthPremium + AdvMthInstallment) - totalDeduct - basicSalary - gradeAmount - FestivalAlw, basicSalary + gradeAmount);//Accommodation,Electricity,loan,advance does not have any effect on Tax.
                        totalDeduct = totalDeduct + tempTax;
                        grdPaySlips[i, (int)GridColumns.Tax_Deduct28].Value = tempTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    }

                    grdPaySlips[i, (int)GridColumns.Total_Deduct29] = new SourceGrid.Cells.Cell(totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Total_Deduct29].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Total_Deduct29].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    
                    grdPaySlips[i, (int)GridColumns.Insurence_Amt30] = new SourceGrid.Cells.Cell(curInsurenceAmt);
                    grdPaySlips[i, (int)GridColumns.Insurence_Amt30].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    decimal netSalary = 0;
                    netSalary = isSavedData && !isChkFestiveChanged ? Convert.ToDecimal(dr["netSalary"].ToString()) : grossAmount - totalDeduct;
                    grdPaySlips[i, (int)GridColumns.Net_Salary31] = new SourceGrid.Cells.Cell(netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Net_Salary31].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Net_Salary31].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                    bool isPresent = true;
                    SourceGrid.Cells.CheckBox chkPresence = new SourceGrid.Cells.CheckBox();
                    if (isSavedData)
                    {
                        isPresent = Convert.ToBoolean(dr["EmpPresence"].ToString());
                    }
                    chkPresence.Checked = isPresent;
                    
                    grdPaySlips[i, (int)GridColumns.Presence32] = chkPresence;               
                    grdPaySlips[i, (int)GridColumns.Presence32].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                 
                    int primaryID = 0;
                    if (isSavedData == true)
                    {
                        primaryID = Convert.ToInt32(dr["ID"].ToString());
                    }
                    DataColumnCollection colCollection = drFound.CopyToDataTable().Columns;

                    primaryID = (colCollection.Contains("PrimaryID")) ? Convert.ToInt32(dr["PrimaryID"].ToString()) : 0;
                    grdPaySlips[i, (int)GridColumns.PrimaryID33] = new SourceGrid.Cells.Cell(primaryID);
                    grdPaySlips[i, (int)GridColumns.PrimaryID33].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.PrimaryID33].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                }

                #endregion

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
              CalculateTotal();
        }
        private void CalculateTotal()
        {
            decimal TgrossAmount = 0, TmiscAllowance = 0, TbasicSalary = 0, TgradeAmount = 0, TpfAmount = 0, Tpension = 0, TinflationAllowance = 0, TAdmAllowance = 0, TOverTimeAllowance = 0, Tacademic = 0, Tpost = 0, Tfestival = 0, TnetSalary = 0;
            decimal TtotalDeduct = 0, TtaxDeduct = 0, TpensionDeduct = 0, TKKDeduct = 0, Taccommodation = 0, Telectricity = 0, Tloan = 0, TMiscDeduct = 0, TpfDeduct = 0,
                TNLKos = 0, Tadvance = 0, TInsurence = 0, TOnePercentTax = 0, TpensionAdjust = 0, TpFAdjust = 0, TLoan1 = 0, TLoan2 = 0, TLoan3 = 0, TLoan4 = 0, TLoan5 = 0;
            int CurrRow = 0;
            for (int i = 3; i <= drFound.Count() + 2; i++)
            {
                CurrRow = i;
                TbasicSalary += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Basic_Salary7].Value);
                TgradeAmount += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Grade_Amt9].Value);
                TpfAmount += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.PF_Add10].Value);
                Tpension += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Pension_Add11].Value);
                TinflationAllowance += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_Dearness12].Value);
                TAdmAllowance += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_Administrative13].Value);
                Tacademic += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_Academic14].Value);
                Tpost += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_General15].Value);
                Tfestival += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_Festival16].Value);
                TmiscAllowance += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_Misc17].Value);

                TOverTimeAllowance += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_OverTime177].Value);

                TgrossAmount += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Gross_Amt18].Value);

                TpfDeduct += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.PF_Deduct19].Value);
                TpensionDeduct += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.PensionF_Deduct20].Value);

                TKKDeduct += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Employee_Welfare21].Value);
                TNLKos += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.CIT22].Value);
                Taccommodation += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.QC_Accomodation23].Value);
                Telectricity += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.QC_Electricity24].Value);
                Tloan += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Loan_Deduct25].Value);
                Tadvance += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Advance_Deduct26].Value);
                TMiscDeduct += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Misc_Deduct27].Value);
                TtaxDeduct += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Tax_Deduct28].Value);
                TtotalDeduct += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Total_Deduct29].Value);
                TInsurence += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Insurence_Amt30].Value);
                TnetSalary += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Net_Salary31].Value);

                TOnePercentTax += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.OnePercentTax35].Value);

                TpensionAdjust += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.PensionAdjust].Value);
                TpFAdjust += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.PFAdjust].Value);

                TLoan1 += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Loan1_255].Value);
                TLoan2 += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Loan2_255].Value);
                TLoan3 += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Loan3_255].Value);
                TLoan4 += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Loan4_255].Value);
                TLoan5 += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Loan5_255].Value);

            }

            SourceGrid.Cells.Views.Cell tAlternateColor = new SourceGrid.Cells.Views.Cell();
            tAlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(ColorTranslator.FromHtml("#e6f7ff"));
            //write last row #86B0AE
            CurrRow = CurrRow + 1;

            grdPaySlips[CurrRow, 0] = new SourceGrid.Cells.Cell("");
            //grdPaySlip[CurrRow, 1] = new SourceGrid.Cells.Cell((CurrRow - 1).ToString());
            grdPaySlips[CurrRow, 1] = new SourceGrid.Cells.Cell("");
            grdPaySlips[CurrRow, 2] = new SourceGrid.Cells.Cell("");
            grdPaySlips[CurrRow, 3] = new SourceGrid.Cells.Cell("");
            grdPaySlips[CurrRow, 4] = new SourceGrid.Cells.Cell("Total");
            grdPaySlips[CurrRow, 4].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            grdPaySlips[CurrRow, 5] = new SourceGrid.Cells.Cell("");
            grdPaySlips[CurrRow, 6] = new SourceGrid.Cells.Cell("");
            grdPaySlips[CurrRow, (int)GridColumns.Basic_Salary7] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Basic_Salary7].Value = TbasicSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Grade8] = new SourceGrid.Cells.Cell("");
            grdPaySlips[CurrRow, (int)GridColumns.Grade_Amt9] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Grade_Amt9].Value = TgradeAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.PF_Add10] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.PF_Add10].Value = TpfAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Pension_Add11] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Pension_Add11].Value = Tpension.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Dearness12] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Dearness12].Value = TinflationAllowance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Administrative13] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Administrative13].Value = TAdmAllowance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Academic14] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Academic14].Value = Tacademic.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Allow_General15] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Allow_General15].Value = Tpost.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Festival16] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Festival16].Value = Tfestival.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Misc17] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Misc17].Value = TmiscAllowance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Allow_OverTime177] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Allow_OverTime177].Value = TOverTimeAllowance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Gross_Amt18] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Gross_Amt18].Value = TgrossAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.PF_Deduct19] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.PF_Deduct19].Value = TpfDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.PensionF_Deduct20] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.PensionF_Deduct20].Value = TpensionDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Employee_Welfare21] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Employee_Welfare21].Value = TKKDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.CIT22] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.CIT22].Value = TNLKos.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.QC_Accomodation23] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.QC_Accomodation23].Value = Taccommodation.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.QC_Electricity24] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.QC_Electricity24].Value = Telectricity.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Loan_Deduct25] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Loan_Deduct25].Value = Tloan.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Advance_Deduct26] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Advance_Deduct26].Value = Tadvance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Misc_Deduct27] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Misc_Deduct27].Value = TMiscDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Tax_Deduct28] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Tax_Deduct28].Value = TtaxDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Total_Deduct29] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Total_Deduct29].Value = TtotalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Insurence_Amt30] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Insurence_Amt30].Value = TInsurence.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Net_Salary31] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Net_Salary31].Value = TnetSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRow, (int)GridColumns.Presence32] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Presence32].Value = "";


            grdPaySlips[CurrRow, (int)GridColumns.OnePercentTax35] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.OnePercentTax35].Value = TOnePercentTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.PFAdjust] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.PFAdjust].Value = TpFAdjust.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.PensionAdjust] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.PensionAdjust].Value = TpensionAdjust.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Loan1_255] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Loan1_255].Value = TLoan1.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Loan2_255] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Loan2_255].Value = TLoan2.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Loan3_255] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Loan3_255].Value = TLoan3.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Loan4_255] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Loan4_255].Value = TLoan4.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, (int)GridColumns.Loan5_255] = new SourceGrid.Cells.Cell();
            grdPaySlips[CurrRow, (int)GridColumns.Loan5_255].Value = TLoan5.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

            grdPaySlips[CurrRow, 0].View =
            grdPaySlips[CurrRow, (int)GridColumns.SN1].View =
            grdPaySlips[CurrRow, (int)GridColumns.DoctorID2].View =
            grdPaySlips[CurrRow, (int)GridColumns.Code3].View =
            grdPaySlips[CurrRow, (int)GridColumns.Name4].View =
            grdPaySlips[CurrRow, (int)GridColumns.Name4].View =
            grdPaySlips[CurrRow, (int)GridColumns.Designation5].View =
            grdPaySlips[CurrRow, (int)GridColumns.Level6].View =
            grdPaySlips[CurrRow, (int)GridColumns.Basic_Salary7].View =
            grdPaySlips[CurrRow, (int)GridColumns.Grade8].View =
            grdPaySlips[CurrRow, (int)GridColumns.Grade_Amt9].View =
            grdPaySlips[CurrRow, (int)GridColumns.PF_Add10].View =
            grdPaySlips[CurrRow, (int)GridColumns.Pension_Add11].View =
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Dearness12].View =
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Administrative13].View =
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Academic14].View =
            grdPaySlips[CurrRow, (int)GridColumns.Allow_General15].View =
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Festival16].View =
            grdPaySlips[CurrRow, (int)GridColumns.Allow_Misc17].View =
            grdPaySlips[CurrRow, (int)GridColumns.Allow_OverTime177].View =
            grdPaySlips[CurrRow, (int)GridColumns.Gross_Amt18].View =
            grdPaySlips[CurrRow, (int)GridColumns.PF_Deduct19].View =
            grdPaySlips[CurrRow, (int)GridColumns.PensionF_Deduct20].View =
            grdPaySlips[CurrRow, (int)GridColumns.Employee_Welfare21].View =
            grdPaySlips[CurrRow, (int)GridColumns.CIT22].View =
            grdPaySlips[CurrRow, (int)GridColumns.QC_Accomodation23].View =
            grdPaySlips[CurrRow, (int)GridColumns.QC_Electricity24].View =
            grdPaySlips[CurrRow, (int)GridColumns.Loan_Deduct25].View =
            grdPaySlips[CurrRow, (int)GridColumns.Advance_Deduct26].View =
            grdPaySlips[CurrRow, (int)GridColumns.Misc_Deduct27].View =
            grdPaySlips[CurrRow, (int)GridColumns.Tax_Deduct28].View =
            grdPaySlips[CurrRow, (int)GridColumns.Total_Deduct29].View =
            grdPaySlips[CurrRow, (int)GridColumns.Insurence_Amt30].View =

            grdPaySlips[CurrRow, (int)GridColumns.Net_Salary31].View =
            grdPaySlips[CurrRow, (int)GridColumns.PFAdjust].View =
            grdPaySlips[CurrRow, (int)GridColumns.PensionAdjust].View =
            grdPaySlips[CurrRow, (int)GridColumns.Loan1_255].View =
            grdPaySlips[CurrRow, (int)GridColumns.Loan2_255].View =
            grdPaySlips[CurrRow, (int)GridColumns.Loan3_255].View =
            grdPaySlips[CurrRow, (int)GridColumns.Loan4_255].View =
            grdPaySlips[CurrRow, (int)GridColumns.Loan5_255].View =

            new SourceGrid.Cells.Views.Cell(tAlternateColor);
            //grdPaySlip[CurrRow, 33].View = new SourceGrid.Cells.Views.Cell(tAlternateColor);

        }
        private decimal CalculateTax(int DocId, decimal netSalary, decimal basicSalaryGrade)
        {
            try
            {
                decimal OnePercentTax = 0;
                decimal taxAmount = 0;
                decimal netSalaryAfterInsurance = 0, annualNetSalary = 0;
                DataTable dt = Doctor.GetDoctorForTax(DocId);
                DataRow dr = dt.Rows[0];
                //decimal insurenceAmt = (isInsurenceApplied ? Convert.ToDecimal(dr["InsurancePremium"].ToString()) / 12 : 0);
                decimal OverTimeAlw = Convert.ToDecimal(dr["OverTimeAlw"].ToString());

                decimal insurenceAmt = Convert.ToDecimal(dr["InsurancePremium"].ToString()) / 12; // monthly insurence premium                
                curInsurenceAmt = insurenceAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                netSalaryAfterInsurance = netSalary - insurenceAmt;
                //annualNetSalary = netSalaryAfterInsurance * 12;
                annualNetSalary = (basicSalaryGrade * 13) + (netSalaryAfterInsurance * 12) + (OverTimeAlw * 12);

                netSalaryAfterInsurance = netSalaryAfterInsurance + basicSalaryGrade + (basicSalaryGrade / 12) + OverTimeAlw;

                if (dr["DoctorType"].ToString() == "Normal")
                {
                    if (dr["IsSingle"].ToString() == "True" )
                    {
                        if (annualNetSalary <= 350000)
                        {
                            taxAmount = netSalaryAfterInsurance * (decimal)0.01;
                            OnePercentTax = taxAmount;
                        }
                        else if (annualNetSalary <= 450000)
                        {
                            taxAmount = (decimal)291.67 + ((netSalaryAfterInsurance - (decimal)29166.67) * (decimal)0.15);
                            OnePercentTax = (decimal)291.67;
                        }
                        else
                        {
                            taxAmount = (decimal)1541.66 + ((netSalaryAfterInsurance - (decimal)37500) * (decimal)0.25);
                            OnePercentTax = (decimal)291.67;
                        }
                    }
                    else if (dr["IsSingle"].ToString() == "False" )
                    {
                        if (annualNetSalary <= 400000)
                        {
                            taxAmount = netSalaryAfterInsurance * (decimal)0.01;
                            OnePercentTax = taxAmount;
                        }
                        else if (annualNetSalary <= 500000)
                        {
                            taxAmount = (decimal)333.33 + ((netSalaryAfterInsurance - (decimal)33333.33) * (decimal)0.15);
                            OnePercentTax = (decimal)333.33;
                        }
                        else
                        {
                            taxAmount = (decimal)1583.33 + ((netSalaryAfterInsurance - (decimal)41666.66) * (decimal)0.25);
                            OnePercentTax = (decimal)333.33;
                        }
                    }
                }
                else if (dr["DoctorType"].ToString() == "Disable")
                {
                    if (dr["IsSingle"].ToString() == "True" )
                    {
                        if (annualNetSalary <= 525000)
                        {
                            taxAmount = netSalaryAfterInsurance * (decimal)0.01;
                            OnePercentTax = taxAmount;
                        }
                        else if (annualNetSalary <= 675000)
                        {
                            taxAmount = (decimal)437.5 + ((netSalaryAfterInsurance - (decimal)43750) * (decimal)0.15);
                            OnePercentTax = (decimal)437.5;
                        }
                        else
                        {
                            taxAmount = (decimal)2312.5 + ((netSalaryAfterInsurance - (decimal)56250) * (decimal)0.25);
                            OnePercentTax = (decimal)437.5;
                        }
                    }
                    else if (dr["IsSingle"].ToString() == "False" )
                    {
                        if (annualNetSalary <= 600000)
                        {
                            taxAmount = netSalaryAfterInsurance * (decimal)0.01;
                            OnePercentTax = taxAmount;
                        }
                        else if (annualNetSalary <= 750000)
                        {
                            taxAmount = (decimal)500 + ((netSalaryAfterInsurance - (decimal)50000) * (decimal)0.15);
                            OnePercentTax = (decimal)500;
                        }
                        else
                        {
                            taxAmount = (decimal)2375 + ((netSalaryAfterInsurance - (decimal)62500) * (decimal)0.25);
                            OnePercentTax = (decimal)500;
                        }
                    }
                }

                if (dr["Gender"].ToString() == "2" )
                {
                    decimal tempTax = taxAmount - (taxAmount * (decimal)0.1);
                    taxAmount = tempTax;
                }

                grdPaySlips[CurrRowPos, (int)GridColumns.OnePercentTax35].Value = OnePercentTax.ToString();
                return taxAmount;

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return 0;
            }

        }
        private void LoadComboboxItems(ComboBox comboBoxItems, bool isBank)
        {
            comboBoxItems.Items.Clear();
            if (comboBoxItems == cmbBankName && isBank == true)
            {
                DataTable dtbankname = Ledger.GetAllLedger(7);
                foreach (DataRow drbank in dtbankname.Rows)
                {
                    comboBoxItems.Items.Add(new ListItem((int)drbank["LedgerID"], drbank["EngName"].ToString()));

                }
                comboBoxItems.SelectedIndex = -1;
                comboBoxItems.DisplayMember = "value";
                comboBoxItems.ValueMember = "id";

            }
            else if (comboBoxItems == cmbBankName && isBank == false)
            {
                DataTable dtCashLedgers = Ledger.GetAllLedger(102);
                foreach (DataRow drbank in dtCashLedgers.Rows)
                {
                    comboBoxItems.Items.Add(new ListItem((int)drbank["LedgerID"], drbank["EngName"].ToString()));

                }
                comboBoxItems.SelectedIndex = -1;
                comboBoxItems.DisplayMember = "value";
                comboBoxItems.ValueMember = "id";

            }
        }
        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMonth.SelectedIndex != -1 && cmbYear.SelectedIndex != -1 && cmbDepartment.SelectedIndex != -1)
                {
                    ListItem SelItem = (ListItem)cmbMonth.SelectedItem;
                    CompanyDetails CompDetails = new CompanyDetails();
                    CompDetails = CompanyInfo.GetInfo();
                    //DateTime FYStartDate = new DateTime();
                    //if (CompDetails.FYFrom != null)
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
                    if (Date.DefaultDate == Date.DateType.Nepali)
                    {
                        //Get Last Day
                        DataTable LastDay = Date.LastDayofMonthNep(FYYear, SelItem.ID);
                        FinalDate = Date.NepToEng(FYYear, SelItem.ID, Convert.ToInt16(LastDay.Rows[0][0]));
                        lblnumberofdays.Text = Convert.ToString(LastDay.Rows[0][0]);
                    }
                    else
                    {
                        FinalDate = new DateTime(FYYear, SelItem.ID, DateTime.DaysInMonth(FYYear, SelItem.ID));
                        lblnumberofdays.Text = DateTime.DaysInMonth(FYYear, SelItem.ID).ToString();
                    }

                    isChkFestiveChanged = false;
                    //isInsurenceApplied = false;
                    //LoadPaySlip();
                    // txtToDate.Text = Date.ToSystem(FinalDate);

                    LoadSavedGrid();
                }
            }
            catch
            {
                //Ignore
            }
        }

        private void btnJournalEntry_Click(object sender, EventArgs e)
        {

        }

        private void chkByCash_CheckedChanged(object sender, EventArgs e)
        {
            if (chkByCash.Checked)
            {
                lblCashParty.Text = "Cash A/C:";
                LoadComboboxItems(cmbBankName, false);
            }
            else
            {
                lblCashParty.Text = "Bank Name:";
                LoadComboboxItems(cmbBankName, true);
            }
        }

        private void btndate_Click(object sender, EventArgs e)
        {
            DateTime dtDate = Date.ToDotNet(txtdate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }
    }
}
