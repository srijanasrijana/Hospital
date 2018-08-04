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
using System.IO;
using System.Data.SqlClient;
using Common;
using BusinessLogic.HRM;
using HRM.View;

namespace HRM
{
    public partial class frmPaySlip : Form, IfrmDateConverter, IPaySlip
    {
        private string FilterString = "";

        TextBox txtVchNo = new TextBox();
        private int OnlyReqdDetailRows = 0;
        DataTable dtaccclass = new DataTable();
        private DataRow[] drFound;
        private DataTable dTable;
        private DataRow[] drFoundemployee;
        private DataTable dTableemployee;
        private DataRow[] drFoundaddtion;
        private DataTable dTableaddition;
        private double AdditionalAllowance = 0;
        int CurrRowPos = 0;
        int CurrRowPosAddition = 0;
        string pf = "";
        int TotalCount = 0;
        int TotalCountDeduction = 0;
        SourceGrid.Cells.Controllers.CustomEvents evtedit = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDeductionFocuslost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDayFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtMiscAllowanceFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtElectricityDeductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtTaxDeductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();

        SourceGrid.Cells.Controllers.CustomEvents evtOverTimeFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();

        //SourceGrid.Cells.Controllers.CustomEvents evtKKShortFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        //SourceGrid.Cells.Controllers.CustomEvents evtKKLongFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        //SourceGrid.Cells.Controllers.CustomEvents evtKKIntFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        //SourceGrid.Cells.Controllers.CustomEvents evtSLInstalFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        //SourceGrid.Cells.Controllers.CustomEvents evtSLInterestFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtMisDeductFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtPresenceValueChanged = new SourceGrid.Cells.Controllers.CustomEvents();

        SourceGrid.Cells.Controllers.CustomEvents evtAllowanceFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
        private DataRow[] drFoundDeduction;
        private double OtherDeduction = 0;
        private DataTable dTableDeduction;
        int CurrRowPosDeduction = 0;

        bool IsDeactivateFestive = false;
        private IMDIMainForm m_MDIForm;
        
        int CheckRow;

        public enum GridColumns
        {
            SN1 = 1, EmployeeID2, Code3, Name4, Designation5, Level6, Basic_Salary7, Grade8, Grade_Amt9, PF_Add10,
            Pension_Add11, Allow_Dearness12, Allow_Administrative13, Allow_Academic14, Allow_General15, Allow_Festival16, Allow_Misc17, Allow_OverTime177, Gross_Amt18, PF_Deduct19, PensionF_Deduct20,
            Employee_Welfare21, CIT22, QC_Accomodation23, QC_Electricity24, Loan1_255, Loan2_255, Loan3_255, Loan4_255, Loan5_255, Loan_Deduct25, Advance_Deduct26, Misc_Deduct27, Tax_Deduct28, Total_Deduct29, Insurence_Amt30, Net_Salary31,
            Presence32, PrimaryID33, Starting_Salary34, OnePercentTax35, PFAdjust, PensionAdjust
        }
        public frmPaySlip()
        {
            InitializeComponent();
        }

        public void InsertJournalId(int journalID)
        {
            employees.VoucherEntered(_paySlipID, journalID);
            //Load the saved value and other changes
            LoadSavedGrid();
        }

        public void DeleteJournalId(int journalID)
        {
            employees.VoucherDeleted(_paySlipID,journalID);
            //Load the saved value and other changes
            LoadSavedGrid();

        }
        public frmPaySlip(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;
        }
        public void DateConvert(DateTime DotNetDate)
        { 
            txtdate.Text = Date.ToSystem(DotNetDate);
        }

        private void frmPaySlip_Load(object sender, EventArgs e)
        {
            Employee.UpdateEmployeeGrade(); // this method is called before the salary payslip is loaded for grade of all employee to be updated
            dtSalaryAdjust = InitSalaryAdjsutTable();
          //  chkFestiveMonth.Checked = false;
         //   chkFestiveMonth.Enabled = IsDeactivateFestive = !Convert.ToBoolean(Convert.ToInt32(Settings.GetSettings("DEACT_FESTIVE_MNTH")));
            //if present month is selected then no need to LoadForm() here as it will be loaded in selectedindex changed.
            LoadYears();
            LoadMonths();
            LoadDepartment();
            LoadFaculty();
            cmbMonth_SelectedIndexChanged(sender, e);
            evtPresenceValueChanged.ValueChanged += new EventHandler(PresenceChanged_ValueChanged);
           
        }

        private bool isSavedData = false;

        private void LoadForm()
        {
            chkFestiveMonth.Checked = false;
            isSavedData = false;
            //These tabs are removed for bisal bazars
            tabControl1.TabPages.Remove(tpadditionalallowances);
            tabControl1.TabPages.Remove(tpotherdeduction);

            txtdate.Mask = Date.FormatToMask();
            txtdate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            LoadComboboxItems(cmbBankName,true);

            int departmentID = 0;
            departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
            int facultyID = 0;
            facultyID = Convert.ToInt32(cmbFaculty.SelectedValue);

            dTable = employees.salaryDetails(departmentID, facultyID, _paySlipID);
            drFound = dTable.Select(FilterString);

            dTableDeduction = employees.getDeductionOnly();
            drFoundDeduction = dTableDeduction.Select(FilterString);
            
          
            fillGrid();

           

        }

        private void evtOverTime_FocusLost(object sender, EventArgs e)
        {

        }

        private void LoadSavedSalary(int paySlipId)
        {
            isSavedData = true;
            //These tabs are removed for bisal bazars
            tabControl1.TabPages.Remove(tpadditionalallowances);
            tabControl1.TabPages.Remove(tpotherdeduction);

            txtdate.Mask = Date.FormatToMask();
            txtdate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            LoadComboboxItems(cmbBankName,!chkByCash.Checked);

            dTable = employees.savedSalaryDetails(paySlipId);
            drFound = dTable.Select(FilterString);


            fillGrid();
            DataTable dtN = employees.GetSalaryMaster(paySlipId);
            //DataTable dtN = employees.salaryDetails(0, 0, paySlipId);
            txtnarration.Text = dtN.Rows[0]["Narration"].ToString();
        }

        /// <summary>
        /// Loads grid if there is already datasaved for the month
        /// </summary>
        private int _paySlipID = 0;
        private bool isJournalEdit = false;
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
                    DataTable dt = Employee.GetPayslipMaster(month, year,departmentID,FacultyID, false);


                    // load employee list for easier search functionality
                    DataTable dtEmployeeList = Employee.GetEmployeeList(FacultyID);
                    cmbEmployeeList.DataSource = null;
                    cmbEmployeeList.DataSource = dtEmployeeList;
                    cmbEmployeeList.DisplayMember = "Name";
                    cmbEmployeeList.ValueMember = "ID";


                    if(dt.Rows.Count > 0)
                    {
                        chkByCash.Checked = false;
                        _paySlipID = Convert.ToInt16(dt.Rows[0]["salaryPaySlipID"].ToString());
                        if(Convert.ToInt16(dt.Rows[0]["isVoucherEntered"].ToString()) == 0)
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
                            if(IsDeactivateFestive)
                                chkFestiveMonth.Enabled = true;

                            //departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                            //int facultyID = 0;
                            //facultyID = Convert.ToInt32(cmbFaculty.SelectedValue);

                            dTable = employees.salaryDetails(departmentID, FacultyID, _paySlipID);
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void evtAllowanceFocusLost_FocusLeft(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int pos = CurrRowPosAddition;
            CurrRowPosAddition = ctx.Position.Row;

            AdditionalAllowance = 0;
          
            for (int i = 1; i <= drFoundaddtion.Count(); i++)
            {
                double AddValue = Convert.ToDouble(grdaddition[CurrRowPosAddition, i + 2].Value);
                grdaddition[CurrRowPosAddition, i + 2].Value = AddValue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                AdditionalAllowance = AdditionalAllowance + Convert.ToDouble(grdaddition[CurrRowPosAddition, i + 2].Value);

            }
            int TotalAllowance = drFoundaddtion.Count() + 3;
            grdaddition[CurrRowPosAddition, TotalAllowance].Value = AdditionalAllowance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

        }

        private void evtMiscAllowanceFocusLost_FocusLeft(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
                CurrRowPos = ctx.Position.Row;

                decimal grossAmount = 0, miscAllowance = 0, basicSalary = 0, gradeAmount = 0, pfAmount = 0, pensionFAmt = 0, InflationAllowance = 0, AdministrativeAllowance = 0, AcademicAllowance = 0, PostAllownace = 0, FestivalAllowance = 0, totalDeduction = 0, startingSalary =0;
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value.ToString() != "")
                {
                    if (!UserValidation.validDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value.ToString()))
                    {
                        MessageBox.Show("Invalid amount, Please enter again.");
                        return;
                    }
                    miscAllowance = Convert.ToDecimal(grdPaySlips[CurrRowPos, 17].Value);
                }
                else
                {
                    grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value = 0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    return;
                }

                if (grdPaySlips[CurrRowPos, (int)GridColumns.Basic_Salary7].Value.ToString() != "")
                {
                    basicSalary = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Basic_Salary7].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Grade_Amt9].Value.ToString() != "")
                {
                    gradeAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Grade_Amt9].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.PF_Add10].Value.ToString() != "")
                {
                    pfAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Add10].Value);
                }

                if (grdPaySlips[CurrRowPos, (int)GridColumns.Pension_Add11].Value.ToString() != "")
                {
                    pensionFAmt = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Pension_Add11].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Dearness12].Value.ToString() != "")
                {
                    InflationAllowance = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Dearness12].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Administrative13].Value.ToString() != "")
                {
                    AdministrativeAllowance = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Administrative13].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Academic14].Value.ToString() != "")
                {
                    AcademicAllowance = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Academic14].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value.ToString() != "")
                {
                    PostAllownace = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Festival16].Value.ToString() != "")
                {
                    FestivalAllowance = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Festival16].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Total_Deduct29].Value.ToString() != "")
                {
                    decimal subTax = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Tax_Deduct28].Value);
                    totalDeduction = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Total_Deduct29].Value) - subTax;//Subtract previous tax to add new calculated tax
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Starting_Salary34].Value.ToString() != "")
                {
                    startingSalary = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Starting_Salary34].Value);
                }
                grossAmount = basicSalary + gradeAmount + pfAmount + pensionFAmt + PostAllownace + FestivalAllowance + miscAllowance; //+ InflationAllowance + AdministrativeAllowance + AcademicAllowance // These fields are removed as this fields are added to postAllowance as General Allowance

                decimal accomodation = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value);
                decimal electricity = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value);
                decimal LoanInstallment = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Loan_Deduct25].Value);
                decimal AdvInstallment = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Advance_Deduct26].Value);
                decimal OverTimeAlw = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_OverTime177].Value);

                //Calculate tax if there is any change in any field of the Salary sheet
                decimal tempTax = CalculateTax(Convert.ToInt32(grdPaySlips[CurrRowPos, (int)GridColumns.EmployeeID2].Value), (grossAmount - basicSalary + startingSalary) + accomodation + electricity  + LoanInstallment + AdvInstallment - totalDeduction - startingSalary - gradeAmount - FestivalAllowance, startingSalary + gradeAmount);
                totalDeduction = totalDeduction + tempTax;
                grdPaySlips[CurrRowPos, (int)GridColumns.Tax_Deduct28].Value = tempTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                decimal netSalary = 0;
                netSalary = grossAmount - totalDeduction;
                //grdPaySlip[i, 14] = new SourceGrid.Cells.Cell(grossAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                //grdPaySlip[i, 14].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                //grdPaySlip[i, 14].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value = grossAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlips[CurrRowPos, (int)GridColumns.Total_Deduct29].Value = totalDeduction.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlips[CurrRowPos, (int)GridColumns.Insurence_Amt30].Value = curInsurenceAmt;//ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlips[CurrRowPos, (int)GridColumns.Net_Salary31].Value = netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                CalculateTotal();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void evtTaxDeduct_FocusLost(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;

            decimal totalDeduct = 0, taxDeduct = 0, KK = 0,Accommodation = 0, Electricity = 0, LoanDeduct = 0, AdvanceDeduct = 0, MiscDeduct = 0, pfDeduct = 0, pensionFDeduct = 0, NLKos = 0, grossAmount = 0;
            if (grdPaySlips[CurrRowPos, 20].Value.ToString() != "")
            {
                if (!UserValidation.validDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value.ToString()))
                {
                    //grdPaySlip[CurrRowPos, 16].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    MessageBox.Show("Invalid amount, Please enter again.");
                    
                    return;
                }
                taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value);
            }
            else
            {
                grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value = 0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                return;
            }
            //if (grdPaySlip[CurrRowPos, 16].Value.ToString() != "")
            //    taxDeduct = Convert.ToDecimal(grdPaySlip[CurrRowPos, 16].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value.ToString() != "")
                pfDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value.ToString() != "")
                pensionFDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value.ToString() != "")
                taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value.ToString() != "")
                KK = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value.ToString() != "")
                NLKos = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value.ToString() != "")
                Accommodation = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value.ToString() != "")
                Electricity = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value);

            if (grdPaySlips[CurrRowPos, (int)GridColumns.Loan_Deduct25].Value.ToString() != "")
                LoanDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Loan_Deduct25].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Advance_Deduct26].Value.ToString() != "")
                AdvanceDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Advance_Deduct26].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Misc_Deduct27].Value.ToString() != "")
                MiscDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Misc_Deduct27].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value.ToString() != "")
                grossAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value);

            //totalDeduct = pfDedection + taxDeduct + KKShort + KKLong + KKInterest + SLInstalment + SLInterest + MiscDeduct + NLKos;
            totalDeduct = pfDeduct + pensionFDeduct + taxDeduct + KK + NLKos + Accommodation + Electricity + LoanDeduct + AdvanceDeduct + MiscDeduct;
            decimal netSalary = 0;
            netSalary = grossAmount - totalDeduct;

            grdPaySlips[CurrRowPos, (int)GridColumns.Tax_Deduct28].Value = totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRowPos, (int)GridColumns.Total_Deduct29].Value = netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            CalculateTotal();
        }


        private void evtKKShort_FocusLost(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;

            decimal totalDeduct = 0, taxDeduct = 0, KKShort = 0, KKLong = 0, KKInterest = 0, SLInstalment = 0, SLInterest = 0, MiscDeduct = 0, pfDedection = 0, NLKos = 0, grossAmount = 0;
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value.ToString() != "")
            {
                if (!UserValidation.validDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value.ToString()))
                {
                    MessageBox.Show("Invalid amount, Please enter again.");
                    //grdPaySlip[CurrRowPos, 17].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    return;
                }
                KKShort = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value);
            }
            else
            {
                grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value = 0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                return;
            }
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Allow_Festival16].Value.ToString() != "")
                taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, 16].Value);
            //if (grdPaySlip[CurrRowPos, 17].Value.ToString() != "")
            //    KKShort = Convert.ToDecimal(grdPaySlip[CurrRowPos, 17].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Gross_Amt18].Value.ToString() != "")
                KKLong = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value.ToString() != "")
                KKInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.PensionF_Deduct20].Value.ToString() != "")
                SLInstalment = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Employee_Welfare21].Value.ToString() != "")
                SLInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value.ToString() != "")
                MiscDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value.ToString() != "")
                pfDedection = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.CIT22].Value.ToString() != "")
                NLKos = Convert.ToDecimal(grdPaySlips[CurrRowPos,(int)GridColumns.CIT22].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Allow_Academic14].Value.ToString() != "")
                grossAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Academic14].Value);

            totalDeduct = pfDedection + taxDeduct + KKShort + KKLong + KKInterest + SLInstalment + SLInterest + MiscDeduct + NLKos;
            decimal netSalary = 0;
            netSalary = grossAmount - totalDeduct;

            grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value = totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRowPos,(int)GridColumns.Loan_Deduct25].Value = netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            CalculateTotal();
        }

        private void evtKKLong_FocusLost(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;

            decimal totalDeduct = 0, taxDeduct = 0, KKShort = 0, KKLong = 0, KKInterest = 0, SLInstalment = 0, SLInterest = 0, MiscDeduct = 0, pfDedection = 0, NLKos = 0, grossAmount = 0;
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Gross_Amt18].Value.ToString() != "")
            {
                if (!UserValidation.validDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value.ToString()))
                {
                    MessageBox.Show("Invalid amount, Please enter again.");
                    //grdPaySlip[CurrRowPos, 18].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    return;
                }
                KKLong = Convert.ToDecimal(grdPaySlips[CurrRowPos, 18].Value);
            }
            else
            {
                grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value = 0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                return;
            }
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Festival16].Value.ToString() != "")
                taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos,(int)GridColumns.Allow_Festival16].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value.ToString() != "")
                KKShort = Convert.ToDecimal(grdPaySlips[CurrRowPos, 17].Value);
            //if (grdPaySlip[CurrRowPos, 18].Value.ToString() != "")
            //    KKLong = Convert.ToDecimal(grdPaySlip[CurrRowPos, 18].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value.ToString() != "")
                KKInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value.ToString() != "")
                SLInstalment = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value.ToString() != "")
                SLInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value.ToString() != "")
                MiscDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value.ToString() != "")
                pfDedection = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value.ToString() != "")
                NLKos = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Allow_Academic14].Value.ToString() != "")
                grossAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Academic14].Value);

            totalDeduct = pfDedection + taxDeduct + KKShort + KKLong + KKInterest + SLInstalment + SLInterest + MiscDeduct + NLKos;
            decimal netSalary = 0;
            netSalary = grossAmount - totalDeduct;

            grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value = totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRowPos, (int)GridColumns.Loan_Deduct25].Value = netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            CalculateTotal();
        }

        private void evtKKInt_FocusLost(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;

            decimal totalDeduct = 0, taxDeduct = 0, KKShort = 0, KKLong = 0, KKInterest = 0, SLInstalment = 0, SLInterest = 0, MiscDeduct = 0, pfDedection = 0, NLKos = 0, grossAmount = 0;
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value.ToString() != "")
            {
                if (!UserValidation.validDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value.ToString()))
                {
                    MessageBox.Show("Invalid amount, Please enter again.");
                    //grdPaySlip[CurrRowPos, 19].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    return;
                }
                KKInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value);
            }
            else
            {
                grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value = 0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                return;
            }
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Allow_Festival16].Value.ToString() != "")
                taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Festival16].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value.ToString() != "")
                KKShort = Convert.ToDecimal(grdPaySlips[CurrRowPos,(int)GridColumns.Allow_Misc17].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value.ToString() != "")
                KKLong = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value);
            //if (grdPaySlip[CurrRowPos, 19].Value.ToString() != "")
            //    KKInterest = Convert.ToDecimal(grdPaySlip[CurrRowPos, 19].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value.ToString() != "")
                SLInstalment = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value.ToString() != "")
                SLInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value.ToString() != "")
                MiscDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value.ToString() != "")
                pfDedection = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.CIT22].Value.ToString() != "")
                NLKos = Convert.ToDecimal(grdPaySlips[CurrRowPos,(int)GridColumns.CIT22].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Allow_Academic14].Value.ToString() != "")
                grossAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Academic14].Value);

            totalDeduct = pfDedection + taxDeduct + KKShort + KKLong + KKInterest + SLInstalment + SLInterest + MiscDeduct + NLKos;
            decimal netSalary = 0;
            netSalary = grossAmount - totalDeduct;

            grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value = totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRowPos, (int)GridColumns.Loan_Deduct25].Value = netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            CalculateTotal();
        }

        private void evtSLInstal_FocusLost(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;

            decimal totalDeduct = 0, taxDeduct = 0, KKShort = 0, KKLong = 0, KKInterest = 0, SLInstalment = 0, SLInterest = 0, MiscDeduct = 0, pfDedection = 0, NLKos = 0, grossAmount = 0;
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value.ToString() != "")
            {
                if (!UserValidation.validDecimal(grdPaySlips[CurrRowPos, 20].Value.ToString()))
                {
                    MessageBox.Show("Invalid amount, Please enter again.");
                    //grdPaySlip[CurrRowPos, 20].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    return;
                }
                SLInstalment = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value);
            }
            else
            {
                grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value = 0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                return;
            }
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Festival16].Value.ToString() != "")
                taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Festival16].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value.ToString() != "")
                KKShort = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Gross_Amt18].Value.ToString() != "")
                KKLong = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value.ToString() != "")
                KKInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value);
            //if (grdPaySlip[CurrRowPos, 20].Value.ToString() != "")
            //    SLInstalment = Convert.ToDecimal(grdPaySlip[CurrRowPos, 20].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value.ToString() != "")
                SLInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value.ToString() != "")
                MiscDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value.ToString() != "")
                pfDedection = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value.ToString() != "")
                NLKos = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Academic14].Value.ToString() != "")
                grossAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Academic14].Value);

            totalDeduct = pfDedection + taxDeduct + KKShort + KKLong + KKInterest + SLInstalment + SLInterest + MiscDeduct + NLKos;
            decimal netSalary = 0;
            netSalary = grossAmount - totalDeduct;

            grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value = totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRowPos, (int)GridColumns.Loan_Deduct25].Value = netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            CalculateTotal();
        }
        private void evtSLInterest_FocusLost(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;

            decimal totalDeduct = 0, taxDeduct = 0, KKShort = 0, KKLong = 0, KKInterest = 0, SLInstalment = 0, SLInterest = 0, MiscDeduct = 0, pfDedection = 0, NLKos = 0, grossAmount = 0;
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Employee_Welfare21].Value.ToString() != "")
            {
                if (!UserValidation.validDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value.ToString()))
                {
                    MessageBox.Show("Invalid amount, Please enter again.");
                    //grdPaySlip[CurrRowPos, 21].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    return;
                }
                SLInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value);
            }
            else
            {
                grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value = 0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                return;
            }
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Allow_Festival16].Value.ToString() != "")
                taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Festival16].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value.ToString() != "")
                KKShort = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Misc17].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value.ToString() != "")
                KKLong = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.PF_Deduct19].Value.ToString() != "")
                KKInterest = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value.ToString() != "")
                SLInstalment = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value);
            //if (grdPaySlip[CurrRowPos, 21].Value.ToString() != "")
            //    SLInterest = Convert.ToDecimal(grdPaySlip[CurrRowPos, 21].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value.ToString() != "")
                MiscDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value.ToString() != "")
                pfDedection = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_General15].Value);
            if (grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value.ToString() != "")
                NLKos = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value);
            if (grdPaySlips[CurrRowPos,(int)GridColumns.Allow_Academic14].Value.ToString() != "")
                grossAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Academic14].Value);

            totalDeduct = pfDedection + taxDeduct + KKShort + KKLong + KKInterest + SLInstalment + SLInterest + MiscDeduct + NLKos;
            decimal netSalary = 0;
            netSalary = grossAmount - totalDeduct;

            grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value = totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            grdPaySlips[CurrRowPos, (int)GridColumns.Loan_Deduct25].Value = netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            CalculateTotal();
        }

        private void evtElectricityDeduct_FocusLost(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
                CurrRowPos = ctx.Position.Row;

                decimal totalDeduct = 0, taxDeduct = 0, KK = 0, Accommodation = 0, Electricity = 0, LoanDeduct = 0, AdvanceDeduct = 0, MiscDeduct = 0, pfDeduct = 0, pensionFDeduct = 0, NLKos = 0, grossAmount = 0;
                if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value.ToString() != "")
                {
                    if (!UserValidation.validDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value.ToString()))
                    {
                        //grdPaySlip[CurrRowPos, 16].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        MessageBox.Show("Invalid amount, Please enter again.");

                        return;
                    }
                    Electricity = Convert.ToDecimal(grdPaySlips[CurrRowPos, 24].Value);
                }
                else
                {
                    grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value = 0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    return;
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Tax_Deduct28].Value.ToString() != "")
                    taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Tax_Deduct28].Value);
                if (grdPaySlips[CurrRowPos,(int)GridColumns.PF_Deduct19].Value.ToString() != "")
                    pfDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value.ToString() != "")
                    pensionFDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Tax_Deduct28].Value.ToString() != "")
                    taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Tax_Deduct28].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value.ToString() != "")
                    KK = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value.ToString() != "")
                    NLKos = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value.ToString() != "")
                    Accommodation = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value);
                //if (grdPaySlip[CurrRowPos, 24].Value.ToString() != "")
                //    Electricity = Convert.ToDecimal(grdPaySlip[CurrRowPos, 24].Value);

                if (grdPaySlips[CurrRowPos,(int)GridColumns.Loan_Deduct25].Value.ToString() != "")
                    LoanDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Loan_Deduct25].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Advance_Deduct26].Value.ToString() != "")
                    AdvanceDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Advance_Deduct26].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Misc_Deduct27].Value.ToString() != "")
                    MiscDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Misc_Deduct27].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value.ToString() != "")
                    grossAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value);

                //totalDeduct = pfDedection + taxDeduct + KKShort + KKLong + KKInterest + SLInstalment + SLInterest + MiscDeduct + NLKos;
                totalDeduct = pfDeduct + pensionFDeduct + taxDeduct + KK + NLKos + Accommodation + Electricity + LoanDeduct + AdvanceDeduct + MiscDeduct;

                ////Calculate tax if there is any change in any field of the Salary sheet
                //decimal tempTax = CalculateTax(Convert.ToInt32(grdPaySlip[CurrRowPos, 2].Value), grossAmount - totalDeduct);
                //totalDeduct = totalDeduct + tempTax;
                //grdPaySlip[CurrRowPos, 20].Value = tempTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                decimal netSalary = 0;
                netSalary = grossAmount - totalDeduct;

                grdPaySlips[CurrRowPos, (int)GridColumns.Total_Deduct29].Value = totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlips[CurrRowPos, (int)GridColumns.Net_Salary31].Value = netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                CalculateTotal();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void evtMisDeduct_FocusLost(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
                CurrRowPos = ctx.Position.Row;

                decimal basicSalary = 0, gradeAmount = 0;
                decimal totalDeduct = 0, taxDeduct = 0, KK = 0, Accommodation = 0, Electricity = 0, LoanDeduct = 0, AdvanceDeduct = 0, MiscDeduct = 0, pfDeduct = 0, pensionFDeduct = 0, NLKos = 0, grossAmount = 0, FestivalAllowance = 0, startingSalary = 0;
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Misc_Deduct27].Value.ToString() != "")
                {
                    if (!UserValidation.validDecimal(grdPaySlips[CurrRowPos,(int)GridColumns.Misc_Deduct27].Value.ToString()))
                    {
                        //grdPaySlip[CurrRowPos, 16].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        MessageBox.Show("Invalid amount, Please enter again.");

                        return;
                    }
                    MiscDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Misc_Deduct27].Value);
                }
                else
                {
                    grdPaySlips[CurrRowPos, (int)GridColumns.Misc_Deduct27].Value = 0.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    return;
                }

                if (grdPaySlips[CurrRowPos,(int)GridColumns.Basic_Salary7].Value.ToString() != "")
                {
                    basicSalary = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Basic_Salary7].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Grade_Amt9].Value.ToString() != "")
                {
                    gradeAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Grade_Amt9].Value);
                }
                if (grdPaySlips[CurrRowPos,(int)GridColumns.PF_Deduct19].Value.ToString() != "")
                    pfDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PF_Deduct19].Value);
                if (grdPaySlips[CurrRowPos,(int)GridColumns.PensionF_Deduct20].Value.ToString() != "")
                    pensionFDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.PensionF_Deduct20].Value);
                if (grdPaySlips[CurrRowPos,(int)GridColumns.Tax_Deduct28].Value.ToString() != "")
                    taxDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Tax_Deduct28].Value);
                if (grdPaySlips[CurrRowPos,(int)GridColumns.Employee_Welfare21].Value.ToString() != "")
                    KK = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Employee_Welfare21].Value);
                if (grdPaySlips[CurrRowPos,(int)GridColumns.CIT22].Value.ToString() != "")
                    NLKos = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.CIT22].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value.ToString() != "")
                    Accommodation = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Accomodation23].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value.ToString() != "")
                    Electricity = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.QC_Electricity24].Value);

                if (grdPaySlips[CurrRowPos,(int)GridColumns.Loan_Deduct25].Value.ToString() != "")
                    LoanDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Loan_Deduct25].Value);
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Advance_Deduct26].Value.ToString() != "")
                    AdvanceDeduct = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Advance_Deduct26].Value);
                //if (grdPaySlip[CurrRowPos, 27].Value.ToString() != "")
                //    MiscDeduct = Convert.ToDecimal(grdPaySlip[CurrRowPos, 27].Value);

                if (grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Festival16].Value.ToString() != "")
                {
                    FestivalAllowance = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Allow_Festival16].Value);
                }
                if (grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value.ToString() != "")
                    grossAmount = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Gross_Amt18].Value);

                if (grdPaySlips[CurrRowPos, (int)GridColumns.Starting_Salary34].Value.ToString() != "")
                    startingSalary = Convert.ToDecimal(grdPaySlips[CurrRowPos, (int)GridColumns.Starting_Salary34].Value);

                //totalDeduct = pfDedection + taxDeduct + KKShort + KKLong + KKInterest + SLInstalment + SLInterest + MiscDeduct + NLKos;
                //totalDeduct = pfDeduct + pensionFDeduct + KK + NLKos + Accommodation + Electricity + LoanDeduct + AdvanceDeduct ;

                totalDeduct = pfDeduct + pensionFDeduct + KK + NLKos + Accommodation + Electricity + LoanDeduct + AdvanceDeduct + MiscDeduct;

                //Calculate tax if there is any change in any field of the Salary sheet
                decimal tempTax = CalculateTax(Convert.ToInt32(grdPaySlips[CurrRowPos, (int)GridColumns.EmployeeID2].Value), (grossAmount - basicSalary + startingSalary) + Accommodation + Electricity + LoanDeduct + AdvanceDeduct - (totalDeduct - MiscDeduct) - startingSalary - gradeAmount - FestivalAllowance, startingSalary + gradeAmount);

                //decimal tempTax = CalculateTax(Convert.ToInt32(grdPaySlip[CurrRowPos, 2].Value), grossAmount + Accommodation + Electricity + LoanDeduct + AdvanceDeduct - totalDeduct - basicSalary - gradeAmount-FestivalAllowance,basicSalary+gradeAmount);
                totalDeduct = totalDeduct + tempTax;
                grdPaySlips[CurrRowPos, (int)GridColumns.Tax_Deduct28].Value = tempTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                decimal netSalary = 0;
                netSalary = grossAmount - totalDeduct;

                grdPaySlips[CurrRowPos, (int)GridColumns.Total_Deduct29].Value = totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlips[CurrRowPos, (int)GridColumns.Insurence_Amt30].Value = curInsurenceAmt;

                grdPaySlips[CurrRowPos, (int)GridColumns.Net_Salary31].Value = netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                CalculateTotal();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            
        }
        /// <summary>
        /// not really used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void evtDayFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            if (lblnumberofdays.Text == "0")
            {
                MessageBox.Show("Please Select Month", "Select Month", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            double BasicSalary = Convert.ToDouble(grdPaySlips[CurrRowPos,(int)GridColumns.Level6].ToString());
            int DaysinMonth = Convert.ToInt32(lblnumberofdays.Text);
            int AbsentDays = Convert.ToInt32(grdPaySlips[CurrRowPos, 7].ToString());
            int PresentDays = DaysinMonth - AbsentDays;
            double PayableSalary = (BasicSalary * PresentDays) / DaysinMonth;
            grdPaySlips[CurrRowPos, 9].Value = PayableSalary.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdPaySlips[CurrRowPos, 10].Value = (PayableSalary * .10).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdPaySlips[CurrRowPos, 16].Value = (PayableSalary * .10 * 2).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            double tdsvalue = 0;
            double netpayable = 0;
            tdsvalue = (Convert.ToDouble(grdPaySlips[CurrRowPos, 9].Value) + Convert.ToDouble(grdPaySlips[CurrRowPos, 10].Value) + Convert.ToDouble(grdPaySlips[CurrRowPos, 15].Value) - Convert.ToDouble(grdPaySlips[CurrRowPos, 16].Value) - Convert.ToDouble(grdPaySlips[CurrRowPos, 18].Value)) * .01;
            grdPaySlips[CurrRowPos, 20].Value = tdsvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            netpayable = (Convert.ToDouble(grdPaySlips[CurrRowPos, 9].Value) + Convert.ToDouble(grdPaySlips[CurrRowPos, 10].Value) + Convert.ToDouble(grdPaySlips[CurrRowPos, 15].Value) - Convert.ToDouble(grdPaySlips[CurrRowPos, 16].Value) - Convert.ToDouble(grdPaySlips[CurrRowPos, 18].Value)) - tdsvalue;
            grdPaySlips[CurrRowPos, 21].Value = netpayable.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
        }

        /// <summary>
        /// Calculates total of the grid and prints the value to the last row.
        /// </summary>
        private void CalculateTotal()
        {
            decimal TgrossAmount = 0, TmiscAllowance = 0, TbasicSalary = 0, TgradeAmount = 0, TpfAmount = 0,Tpension = 0, TinflationAllowance = 0, TAdmAllowance = 0, TOverTimeAllowance = 0,Tacademic = 0,Tpost = 0,Tfestival=0, TnetSalary = 0;
            decimal TtotalDeduct = 0, TtaxDeduct = 0, TpensionDeduct = 0, TKKDeduct = 0, Taccommodation = 0, Telectricity = 0, Tloan = 0, TMiscDeduct = 0, TpfDeduct = 0, 
                TNLKos = 0, Tadvance = 0, TInsurence = 0, TOnePercentTax = 0, TpensionAdjust = 0, TpFAdjust = 0, TLoan1 = 0, TLoan2 = 0, TLoan3 = 0, TLoan4 =0 , TLoan5 =0;
            int CurrRow = 0;
            for (int i = 3; i <= drFound.Count() +2 ; i++)
            {
                CurrRow = i;
                TbasicSalary += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Basic_Salary7].Value);
                TgradeAmount += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Grade_Amt9].Value);
                TpfAmount += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.PF_Add10].Value);
                Tpension += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Pension_Add11].Value);
                TinflationAllowance += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_Dearness12].Value);
                TAdmAllowance += Convert.ToDecimal(grdPaySlips[CurrRow,(int)GridColumns.Allow_Administrative13].Value);
                Tacademic += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_Academic14].Value);
                Tpost += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_General15].Value);
                Tfestival += Convert.ToDecimal(grdPaySlips[CurrRow,  (int)GridColumns.Allow_Festival16].Value);
                TmiscAllowance += Convert.ToDecimal(grdPaySlips[CurrRow,  (int)GridColumns.Allow_Misc17].Value);

                TOverTimeAllowance += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Allow_OverTime177].Value);

                TgrossAmount += Convert.ToDecimal(grdPaySlips[CurrRow, (int)GridColumns.Gross_Amt18].Value);

                TpfDeduct += Convert.ToDecimal(grdPaySlips[CurrRow,  (int)GridColumns.PF_Deduct19].Value);
                TpensionDeduct += Convert.ToDecimal(grdPaySlips[CurrRow,  (int)GridColumns.PensionF_Deduct20].Value);
                
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
            grdPaySlips[CurrRow, (int)GridColumns.EmployeeID2].View = 
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
        //string savedFaculty = "";
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
            // SourceGrid.Cells.CheckBox chkMatch = new SourceGrid.Cells.CheckBox();
            SourceGrid.Cells.Views.Cell AlternateColor = new SourceGrid.Cells.Views.Cell();

            //savedFaculty = Employee.GetSavedFacultyInfo(SelItem.ID, Convert.ToInt32(cmbYear.Text));
            //if (savedFaculty != null && savedFaculty != "")
            //{
            //    DataTable dtTemp = drFound.CopyToDataTable();
            //    drFound = dtTemp.Select("[FacultyID] not in (" + savedFaculty + ") ");
            //}

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

                grdPaySlips[i, (int)GridColumns.EmployeeID2] = new SourceGrid.Cells.Cell(dr["EmployeeID"].ToString());


                grdPaySlips[i, (int)GridColumns.Code3] = new SourceGrid.Cells.Cell(dr["StaffCode"].ToString());
                grdPaySlips[i, (int)GridColumns.Code3].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Name4] = new SourceGrid.Cells.Cell(dr["StaffName"].ToString());
                grdPaySlips[i, (int)GridColumns.Name4].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Designation5] = new SourceGrid.Cells.Cell(dr["DesignationName"].ToString());
                grdPaySlips[i, (int)GridColumns.Designation5].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Level6] = new SourceGrid.Cells.Cell(isSavedData ? dr["Level"].ToString() : dr["LevelName"].ToString());
                grdPaySlips[i, (int)GridColumns.Level6].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                decimal basicSalary = 0;
                if (!dr.IsNull("BasicSalary"))
                    basicSalary = Convert.ToDecimal(dr["BasicSalary"]);

                object AdjustSum = null;
                if (dtSalaryAdjust.Rows.Count > 0)
                {
                    DataRow[] drs = dtSalaryAdjust.Select("[EmployeeID] = " + dr["EmployeeID"]);
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
                        grdPaySlips[i, (int)GridColumns.PF_Add10] = new SourceGrid.Cells.Cell((pfAmount) .ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
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
                //txtOverTime.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdPaySlips[i, (int)GridColumns.Allow_OverTime177] = new SourceGrid.Cells.Cell((OverTimeAlw).ToString(Misc.FormatNumber(false, Global.DecimalPlaces)), txtOverTime);
                //grdPaySlip[i, (int)GridColumns.Allow_OverTime177] = new SourceGrid.Cells.Cell();
                grdPaySlips[i, (int)GridColumns.Allow_OverTime177].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Allow_OverTime177].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                //grdPaySlip[i, (int)GridColumns.Allow_OverTime177].

                decimal PostAlw = 0;
                if (!dr.IsNull("PostAlw"))
                    PostAlw = Convert.ToDecimal(dr["PostAlw"].ToString());

                decimal GeneralAlw = 0;
                GeneralAlw = InflationAlw + AdmistrativeAlw + AcademicAlw + PostAlw;

                //grdPaySlip[i, 15] = new SourceGrid.Cells.Cell(PostAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                grdPaySlips[i, (int)GridColumns.Allow_General15] = new SourceGrid.Cells.Cell(GeneralAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                grdPaySlips[i, (int)GridColumns.Allow_General15].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Allow_General15].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                decimal FestivalAlw = 0;
                //if (isSavedData && !isChkFestiveChanged)
                //{
                //    if (!dr.IsNull("FestivalAlw"))
                //        FestivalAlw = Convert.ToDecimal(dr["FestivalAlw"].ToString());
                //}
                //else
                //{
                //    if (chkFestiveMonth.Checked)
                //    {
                //        FestivalAlw = basicSalary + gradeAmount;
                //    }
                //}
             
                SourceGrid.Cells.Editors.TextBox txtMiscAllowance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                grdPaySlips[i, (int)GridColumns.Allow_Misc17] = new SourceGrid.Cells.Cell("", txtMiscAllowance);
                //grdPaySlip[i, (int)GridColumns.Allow_Misc17].AddController(evtMiscAllowanceFocusLost);
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
                    if (grdPaySlips[i,(int)GridColumns.Allow_Misc17].Value.ToString() != "")
                        miscAllowance = Convert.ToDecimal(grdPaySlips[i,(int)GridColumns.Allow_Misc17].Value);
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
                grdPaySlips[i, (int)GridColumns.QC_Electricity24].AddController(evtElectricityDeductFocusLost);
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
                    //if (Convert.ToInt32(dr["LoanRemainingMth"].ToString()) > 0)
                    //{
                        if (!dr.IsNull("LoanPremium"))
                            LoanMthPremium = Convert.ToDecimal(dr["LoanPremium"].ToString());
                        DataTable dtLoan = employees.GetEmployeeLoan(Convert.ToInt32(dr["EmployeeID"].ToString()), true);
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

                                grdPaySlips[i, ((int)GridColumns.Loan1_255 + cnt)].Value  = loanAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                //grdPaySlip[i, ((int)GridColumns.Loan1_255 + cnt)].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                                totalLoan += loanAmt;

                            } 
                        }
                    //}
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
                //txtbasicallowance.EditableMode = SourceGrid.EditableMode.None;
                grdPaySlips[i, (int)GridColumns.Misc_Deduct27] = new SourceGrid.Cells.Cell("", txtMiscDeduct);
                grdPaySlips[i, (int)GridColumns.Misc_Deduct27].AddController(evtMisDeductFocusLost);
                grdPaySlips[i, (int)GridColumns.Misc_Deduct27].Value = (isSavedData ? Convert.ToDecimal(dr["MiscDeduct"].ToString()) : 0).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlips[i, (int)GridColumns.Misc_Deduct27].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Misc_Deduct27].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                //SourceGrid.Cells.Editors.TextBox txtTaxDeduct = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                ////txtbasicallowance.EditableMode = SourceGrid.EditableMode.None;
                //grdPaySlip[i, 20] = new SourceGrid.Cells.Cell("", txtTaxDeduct);
                //grdPaySlip[i, 20].AddController(evtTaxDeductFocusLost);
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
                    if (grdPaySlips[i,(int)GridColumns.Misc_Deduct27].Value.ToString() != "")
                        MiscDeduct = Convert.ToDecimal(grdPaySlips[i,(int)GridColumns.Misc_Deduct27].Value);
                    if (grdPaySlips[i,(int)GridColumns.QC_Electricity24].Value.ToString() != "")
                        ElectricityDeduct = Convert.ToDecimal(grdPaySlips[i,(int)GridColumns.QC_Electricity24].Value);

                    totalDeduct = pfDedection + PensionFAmtDeduct + KalyankariAmt + NLKos + Accommodation + LoanMthPremium + AdvMthInstallment + ElectricityDeduct;// + taxDeduct // misc deduct removed from total deduct

                    //totalDeduct = pfDedection + PensionFAmtDeduct + KalyankariAmt + MiscDeduct + NLKos + Accommodation + LoanMthPremium + AdvMthInstallment + ElectricityDeduct;// + taxDeduct
                }

                decimal startingSalary =Convert.ToDecimal(dr["StartingSalary"]);
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
                        //FestivalAlw = basicSalary + gradeAmount;

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
                  

                if(!isSavedData || isChkFestiveChanged)
                {
                    decimal tempTax = CalculateTax(Convert.ToInt32(dr["EmployeeID"].ToString()), ((grossAmount - basicSalary + startingSalary) + Accommodation + ElectricityDeduct + LoanMthPremium + AdvMthInstallment) - totalDeduct - startingSalary - gradeAmount - FestivalAlw - overTimeAlw, startingSalary + gradeAmount);//Accommodation,Electricity,loan,advance does not have any effect on Tax. Tax calculation based on starting salary instead of basic salary

                    //decimal tempTax = CalculateTax(Convert.ToInt32(dr["EmployeeID"].ToString()), (grossAmount + Accommodation + ElectricityDeduct + LoanMthPremium + AdvMthInstallment) - totalDeduct - basicSalary - gradeAmount - FestivalAlw, basicSalary + gradeAmount);//Accommodation,Electricity,loan,advance does not have any effect on Tax.
                    totalDeduct = totalDeduct + tempTax;
                    grdPaySlips[i, (int)GridColumns.Tax_Deduct28].Value = tempTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                }

                grdPaySlips[i,(int)GridColumns.Total_Deduct29] = new SourceGrid.Cells.Cell(totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                grdPaySlips[i,(int)GridColumns.Total_Deduct29].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i,(int)GridColumns.Total_Deduct29].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                //decimal InsurenceAmt = isSavedData? Convert.ToDecimal(dr["InsurencePremium"].ToString()):0;
                grdPaySlips[i,(int)GridColumns.Insurence_Amt30] = new SourceGrid.Cells.Cell(curInsurenceAmt);
                grdPaySlips[i,(int)GridColumns.Insurence_Amt30].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

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
                //grdPaySlip[i, 31].View = new SourceGrid.Cells.Cell(isPresent,chkPresence);
                grdPaySlips[i,(int)GridColumns.Presence32] = chkPresence;
                //grdPaySlip[i, 9].AddController(evtChk);
                grdPaySlips[i, (int)GridColumns.Presence32].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                grdPaySlips[i, (int)GridColumns.Presence32].AddController(evtPresenceValueChanged);
                int primaryID = 0;
                if(isSavedData == true)
                {
                    primaryID = Convert.ToInt32( dr["ID"].ToString());
                }
                DataColumnCollection colCollection = drFound.CopyToDataTable().Columns;

                primaryID = (colCollection.Contains("PrimaryID")) ? Convert.ToInt32(dr["PrimaryID"].ToString()) : 0;
                grdPaySlips[i, (int)GridColumns.PrimaryID33] = new SourceGrid.Cells.Cell(primaryID);
                grdPaySlips[i, (int)GridColumns.PrimaryID33].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.PrimaryID33].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

            }



            CalculateTotal();
        }

        ////Previous Method to calculate tax
        //private decimal CalculateTax(int EmpId,decimal netSalary)
        //{
        //    try
        //    {
        //        decimal taxAmount = 0;
        //        decimal netSalaryAfterInsurance = 0, annualNetSalary = 0 ;
        //        DataTable dt = employees.GetEmployeeForTax(EmpId);
        //        DataRow dr = dt.Rows[0];
        //        netSalaryAfterInsurance = netSalary - Convert.ToDecimal(dr["InsurancePremium"].ToString());
        //        annualNetSalary = netSalaryAfterInsurance * 12;
        //        if (dr["EmpType"].ToString() == "Normal")
        //        {
        //            if (dr["IsSingle"].ToString() == "True" || dr["IsCoupleWorking"].ToString() == "True")
        //            {
        //                if(annualNetSalary <= 350000)
        //                {
        //                    taxAmount = netSalaryAfterInsurance * (decimal)0.01;
        //                }
        //                else if(annualNetSalary <= 450000)
        //                {
        //                    taxAmount = (decimal)291.66 + ((netSalaryAfterInsurance - (decimal)29166.66) * (decimal)0.15);
        //                }
        //                else
        //                {
        //                    taxAmount = (decimal)1541.66 + ((netSalaryAfterInsurance - (decimal)37500) * (decimal)0.25);
        //                }
        //            }
        //            else if (dr["IsSingle"].ToString() == "False" && dr["IsCoupleWorking"].ToString() == "False")
        //            {
        //                if (annualNetSalary <= 400000)
        //                {
        //                    taxAmount = netSalaryAfterInsurance * (decimal)0.01;
        //                }
        //                else if (annualNetSalary <= 500000)
        //                {
        //                    taxAmount = (decimal)333.33 + ((netSalaryAfterInsurance - (decimal)33333.33) * (decimal)0.15);
        //                }
        //                else
        //                {
        //                    taxAmount = (decimal)1583.33 + ((netSalaryAfterInsurance - (decimal)41666.66) * (decimal)0.25);
        //                }
        //            }
        //        }
        //        else if(dr["EmpType"].ToString() == "Disable")
        //        {
        //            if (dr["IsSingle"].ToString() == "True" || dr["IsCoupleWorking"].ToString() == "True")
        //            {
        //                if (annualNetSalary <= 525000)
        //                {
        //                    taxAmount = netSalaryAfterInsurance * (decimal)0.01;
        //                }
        //                else if (annualNetSalary <= 675000)
        //                {
        //                    taxAmount = (decimal)437.5 + ((netSalaryAfterInsurance - (decimal)43750) * (decimal)0.15);
        //                }
        //                else
        //                {
        //                    taxAmount = (decimal)2312.5 + ((netSalaryAfterInsurance - (decimal)56250) * (decimal)0.25);
        //                }
        //            }
        //            else if (dr["IsSingle"].ToString() == "False" && dr["IsCoupleWorking"].ToString() == "False")
        //            {
        //                if (annualNetSalary <= 600000)
        //                {
        //                    taxAmount = netSalaryAfterInsurance * (decimal)0.01;
        //                }
        //                else if (annualNetSalary <= 750000)
        //                {
        //                    taxAmount = (decimal)500 + ((netSalaryAfterInsurance - (decimal)50000) * (decimal)0.15);
        //                }
        //                else
        //                {
        //                    taxAmount = (decimal)2375 + ((netSalaryAfterInsurance - (decimal)62500) * (decimal)0.25);
        //                }
        //            }
        //        }
        //        return taxAmount;

        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MsgError(ex.Message);
        //        return 0;
        //    }

        //}
        string curInsurenceAmt = "0";
        private decimal CalculateTax(int EmpId, decimal netSalary,decimal basicSalaryGrade)
        {
            try
            {
                decimal OnePercentTax = 0;
                decimal taxAmount = 0;
                decimal netSalaryAfterInsurance = 0, annualNetSalary = 0;
                DataTable dt = employees.GetEmployeeForTax(EmpId);
                DataRow dr = dt.Rows[0];
                //decimal insurenceAmt = (isInsurenceApplied ? Convert.ToDecimal(dr["InsurancePremium"].ToString()) / 12 : 0);
                decimal OverTimeAlw = Convert.ToDecimal(dr["OverTimeAlw"].ToString());

                decimal insurenceAmt =  Convert.ToDecimal(dr["InsurancePremium"].ToString()) / 12 ; // monthly insurence premium                
                curInsurenceAmt = insurenceAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                netSalaryAfterInsurance = netSalary - insurenceAmt;
                //annualNetSalary = netSalaryAfterInsurance * 12;
                annualNetSalary = (basicSalaryGrade * 13) + (netSalaryAfterInsurance * 12)+(OverTimeAlw * 12);

                netSalaryAfterInsurance = netSalaryAfterInsurance + basicSalaryGrade + (basicSalaryGrade / 12) + OverTimeAlw;
                
                if (dr["EmpType"].ToString() == "Normal")
                {
                    if (dr["IsSingle"].ToString() == "True" || dr["IsCoupleWorking"].ToString() == "True")
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
                    else if (dr["IsSingle"].ToString() == "False" && dr["IsCoupleWorking"].ToString() == "False")
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
                else if (dr["EmpType"].ToString() == "Disable")
                {
                    if (dr["IsSingle"].ToString() == "True" || dr["IsCoupleWorking"].ToString() == "True")
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
                    else if (dr["IsSingle"].ToString() == "False" && dr["IsCoupleWorking"].ToString() == "False")
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

                if (dr["Gender"].ToString() == "2" && dr["IsSingle"].ToString() == "True")
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

        //this method is not used for PN
        private void fillGrid(bool checkAll)
        {
            grdPaySlips.Rows.Clear();
            grdPaySlips.Redim(drFound.Count() + 1, 24);
            writeHeader();
            // SourceGrid.Cells.CheckBox chkMatch = new SourceGrid.Cells.CheckBox();
            SourceGrid.Cells.Views.Cell AlternateColor = new SourceGrid.Cells.Views.Cell();
            for (int i = 1; i <= drFound.Count(); i++)
            {
                if (i % 2 == 0)
                    AlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                else
                    AlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);

                DataRow dr = drFound[i - 1];
                SourceGrid.Cells.CheckBox checkView = new SourceGrid.Cells.CheckBox(null, checkAll);

                grdPaySlips[i, 0] = checkView;

                grdPaySlips[i, (int)GridColumns.SN1] = new SourceGrid.Cells.Cell(i.ToString());
                grdPaySlips[i, (int)GridColumns.SN1].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.EmployeeID2] = new SourceGrid.Cells.Cell(dr["EmployeeID"].ToString());


                grdPaySlips[i, (int)GridColumns.Code3] = new SourceGrid.Cells.Cell(dr["StaffCode"].ToString());
                grdPaySlips[i, (int)GridColumns.Code3].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Name4] = new SourceGrid.Cells.Cell(dr["StaffName"].ToString());
                grdPaySlips[i, (int)GridColumns.Name4].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Designation5] = new SourceGrid.Cells.Cell(dr["DesignationName"].ToString());
                grdPaySlips[i, (int)GridColumns.Designation5].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                string basicsalary = Convert.ToDouble(dr["BasicSalary"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                grdPaySlips[i, (int)GridColumns.Level6] = new SourceGrid.Cells.Cell(basicsalary);
                grdPaySlips[i, (int)GridColumns.Level6].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Level6].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                SourceGrid.Cells.Editors.TextBox txtabsentday = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtabsentday.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdPaySlips[i, (int)GridColumns.Basic_Salary7] = new SourceGrid.Cells.Cell("", txtabsentday);
                grdPaySlips[i, (int)GridColumns.Basic_Salary7].AddController(evtDayFocusLost);
                grdPaySlips[i, (int)GridColumns.Basic_Salary7].Value = "0";
                grdPaySlips[i, (int)GridColumns.Basic_Salary7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                grdPaySlips[i, (int)GridColumns.Basic_Salary7].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Grade_Amt9] = new SourceGrid.Cells.Cell(basicsalary);
                grdPaySlips[i, (int)GridColumns.Grade_Amt9].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Grade_Amt9].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Grade8] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdPaySlips[i, (int)GridColumns.Grade8].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Grade8].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                //string test = dr["IsPF"].ToString();
                //MessageBox.Show(dr["IsPF"].ToString());
                if (dr["IsPF"].ToString() == "False")
                {
                    grdPaySlips[i, (int)GridColumns.PF_Add10] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.PF_Add10].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.PF_Add10].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                }
                else if (dr["IsPF"].ToString() == "True")
                {
                    double pfvalue = (Convert.ToDouble(grdPaySlips[i, 9].ToString()) * 0.10);
                    SourceGrid.Cells.Editors.TextBox txtpf = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    txtpf.EditableMode = SourceGrid.EditableMode.None;
                    grdPaySlips[i, (int)GridColumns.PF_Add10] = new SourceGrid.Cells.Cell("", txtpf);
                    grdPaySlips[i, (int)GridColumns.PF_Add10].AddController(evtDayFocusLost);
                    grdPaySlips[i, (int)GridColumns.PF_Add10].Value = pfvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdPaySlips[i, (int)GridColumns.PF_Add10].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    grdPaySlips[i, (int)GridColumns.PF_Add10].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                }

                SourceGrid.Cells.Editors.TextBox txtbasicallowance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtbasicallowance.EditableMode = SourceGrid.EditableMode.None;
                grdPaySlips[i, (int)GridColumns.Pension_Add11] = new SourceGrid.Cells.Cell("", txtbasicallowance);
                grdPaySlips[i, (int)GridColumns.Pension_Add11].Value = Convert.ToDouble(dr["BasicAllowance"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                grdPaySlips[i, (int)GridColumns.Pension_Add11].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                grdPaySlips[i, (int)GridColumns.Pension_Add11].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Allow_Dearness12] = new SourceGrid.Cells.Cell(Convert.ToDouble(dr["Bonus"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdPaySlips[i, (int)GridColumns.Allow_Dearness12].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Allow_Dearness12].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Allow_Administrative13] = new SourceGrid.Cells.Cell(Convert.ToDouble(dr["TADA"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdPaySlips[i, (int)GridColumns.Allow_Administrative13].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Allow_Administrative13].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Allow_Academic14] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdPaySlips[i, (int)GridColumns.Allow_Academic14].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Allow_Academic14].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                double TotalAllowances = Convert.ToDouble(grdPaySlips[i, 11].ToString()) + Convert.ToDouble(grdPaySlips[i, 12].Value) + Convert.ToDouble(grdPaySlips[i, 13].Value) + Convert.ToDouble(grdPaySlips[i, 14].Value);
                grdPaySlips[i, (int)GridColumns.Allow_General15] = new SourceGrid.Cells.Cell(TotalAllowances.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdPaySlips[i, (int)GridColumns.Allow_General15].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Allow_General15].View = new SourceGrid.Cells.Views.Cell(AlternateColor);


                if (dr["IsPF"].ToString() == "0")
                {
                    grdPaySlips[i,  (int)GridColumns.Allow_Festival16] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                }
                else
                {
                    double pfvalue = (Convert.ToDouble(grdPaySlips[i, 8].ToString()) * 0.10 * 2);
                    SourceGrid.Cells.Editors.TextBox txt2pf = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    txt2pf.EditableMode = SourceGrid.EditableMode.None;
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16] = new SourceGrid.Cells.Cell("", txt2pf);
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16].AddController(evtDayFocusLost);
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16].Value = pfvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    grdPaySlips[i, (int)GridColumns.Allow_Festival16].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                }

                grdPaySlips[i, (int)GridColumns.Allow_Misc17] = new SourceGrid.Cells.Cell(dr["CIFNumber"].ToString());
                grdPaySlips[i, (int)GridColumns.Allow_Misc17].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Allow_Misc17].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.Gross_Amt18] = new SourceGrid.Cells.Cell(Convert.ToDouble(dr["CITAmount"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdPaySlips[i, (int)GridColumns.Gross_Amt18].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.Gross_Amt18].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                grdPaySlips[i, (int)GridColumns.PF_Deduct19] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdPaySlips[i, (int)GridColumns.PF_Deduct19].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdPaySlips[i, (int)GridColumns.PF_Deduct19].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                double TDS = 0;
                double NetPayable = 0;
                TDS = (Convert.ToDouble(grdPaySlips[i,(int)GridColumns.Grade8].Value) + Convert.ToDouble(grdPaySlips[i, 10].Value) + Convert.ToDouble(grdPaySlips[i, 15].Value) - Convert.ToDouble(grdPaySlips[i, 16].Value) - Convert.ToDouble(grdPaySlips[i, 18].Value)) * .01;
                SourceGrid.Cells.Editors.TextBox txttds = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txttds.EditableMode = SourceGrid.EditableMode.None;
                grdPaySlips[i, (int)GridColumns.PensionF_Deduct20] = new SourceGrid.Cells.Cell("", txttds);
                grdPaySlips[i, (int)GridColumns.PensionF_Deduct20].Value = TDS.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                grdPaySlips[i, (int)GridColumns.PensionF_Deduct20].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                grdPaySlips[i, (int)GridColumns.PensionF_Deduct20].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                NetPayable = Convert.ToDouble(grdPaySlips[i, (int)GridColumns.Grade8].Value) + Convert.ToDouble(grdPaySlips[i, 10].Value) + Convert.ToDouble(grdPaySlips[i, 15].Value) - Convert.ToDouble(grdPaySlips[i, 16].Value) - Convert.ToDouble(grdPaySlips[i, 18].Value) - TDS;
                SourceGrid.Cells.Editors.TextBox txtnetpayable = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtnetpayable.EditableMode = SourceGrid.EditableMode.None;
                grdPaySlips[i, (int)GridColumns.Employee_Welfare21] = new SourceGrid.Cells.Cell("", txtnetpayable);
                grdPaySlips[i, (int)GridColumns.Employee_Welfare21].Value = NetPayable.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                grdPaySlips[i, (int)GridColumns.Employee_Welfare21].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                grdPaySlips[i, (int)GridColumns.Employee_Welfare21].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

            }
        }

        DataTable dtLoanNames = null;
        private void writeHeader()
        {
            grdPaySlips[0, 0] = new MyHeader("");
            grdPaySlips[0, 0].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.SN1] = new MyHeader("SN");
            grdPaySlips[0, (int)GridColumns.SN1].RowSpan = 2;
            //grdPaySlip[0, 1].Column.Grid.ActualFixedColumns = SourceGrid.Grid.ScrollStateHScrollVisible;
            grdPaySlips[0, (int)GridColumns.EmployeeID2] = new MyHeader("Employee ID");
            grdPaySlips[0, (int)GridColumns.EmployeeID2].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Code3] = new MyHeader("Code");
            grdPaySlips[0, (int)GridColumns.Code3].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Name4] = new MyHeader("Name");
            grdPaySlips[0, (int)GridColumns.Name4].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Designation5] = new MyHeader("Designation");
            grdPaySlips[0, (int)GridColumns.Designation5].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Level6] = new MyHeader("Level");
            grdPaySlips[0, (int)GridColumns.Level6].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Basic_Salary7] = new MyHeader("Basic Salary");
            grdPaySlips[0, (int)GridColumns.Basic_Salary7].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Grade8] = new MyHeader("Grade");
            grdPaySlips[0, (int)GridColumns.Grade8].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Grade_Amt9] = new MyHeader("Grade Amount");
            grdPaySlips[0, (int)GridColumns.Grade_Amt9].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.PF_Add10] = new MyHeader("PF(Add)");
            grdPaySlips[0, (int)GridColumns.PF_Add10].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Pension_Add11] = new MyHeader("Pension F(Add)");
            grdPaySlips[0, (int)GridColumns.Pension_Add11].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Allow_Dearness12] = new MyHeader("Allowance");
            grdPaySlips[0, (int)GridColumns.Allow_Dearness12].ColumnSpan = 7;
            grdPaySlips[1, (int)GridColumns.Allow_Dearness12] = new MyHeader("Dearness");
            grdPaySlips[1, (int)GridColumns.Allow_Administrative13] = new MyHeader("Administrative");
            grdPaySlips[1, (int)GridColumns.Allow_Academic14] = new MyHeader("Academic");
            //grdPaySlip[1, 15] = new MyHeader("Post");
            grdPaySlips[1, (int)GridColumns.Allow_General15] = new MyHeader("General");
            grdPaySlips[1, (int)GridColumns.Allow_Festival16] = new MyHeader("Festival");
            grdPaySlips[1, (int)GridColumns.Allow_Misc17] = new MyHeader("Miscellaneous");
            //grdPaySlip[1, (int)GridColumns.Allow_OverTime177] = new MyHeader("Miscellaneous");

            grdPaySlips[1, (int)GridColumns.Allow_OverTime177] = new MyHeader("OverTime");
            //grdPaySlip[1, (int)GridColumns.Allow_OverTime177].Column.Ed
            //grdPaySlip[1, (int)GridColumns.Allow_OverTime177].Column.Visible = false;

            //grdPaySlip[1, (int)GridColumns.Allow_OverTime177].RowSpan = 2;
            //grdPaySlip[1, (int)GridColumns.Allow_OverTime177].Column.Visible = false;

            grdPaySlips[0, (int)GridColumns.Gross_Amt18] = new MyHeader("Gross Amount");
            grdPaySlips[0, (int)GridColumns.Gross_Amt18].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.PF_Deduct19] = new MyHeader("PF (Deduct)");
            grdPaySlips[0, (int)GridColumns.PF_Deduct19].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.PensionF_Deduct20] = new MyHeader("Pension F(Deduct)");
            grdPaySlips[0, (int)GridColumns.PensionF_Deduct20].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.Employee_Welfare21] = new MyHeader("Employee Welfare");
            grdPaySlips[0, (int)GridColumns.Employee_Welfare21].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.CIT22] = new MyHeader("CIT");
            grdPaySlips[0, (int)GridColumns.CIT22].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.QC_Accomodation23] = new MyHeader("Quarter Charge");
            grdPaySlips[0, (int)GridColumns.QC_Accomodation23].ColumnSpan = 2;
            grdPaySlips[1, (int)GridColumns.QC_Accomodation23] = new MyHeader("Accommodation");
            //grdPaySlip[1, 23].RowSpan = 2;
            grdPaySlips[1, (int)GridColumns.QC_Electricity24] = new MyHeader("Electricity");
            //grdPaySlip[1, 24].RowSpan = 2;

            dtLoanNames = Hrm.GetLoanForCmb();
            dtLoanNames.DefaultView.Sort = "ID";  // sort the datatable according to MasterID in ascending order
            dtLoanNames = dtLoanNames.DefaultView.ToTable(); 
            
            grdPaySlips[0, (int)GridColumns.Loan1_255] = new MyHeader("Loan");
            grdPaySlips[0, (int)GridColumns.Loan1_255].ColumnSpan = 6;

            grdPaySlips[1, (int)GridColumns.Loan1_255] = new MyHeader(dtLoanNames.Rows[0]["Value"].ToString());
            grdPaySlips[2, (int)GridColumns.Loan1_255] = new MyHeader(dtLoanNames.Rows[0]["ID"].ToString());

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

            //grdPaySlip[1, (int)GridColumns.Loan5_255].Column.Visible =
            //grdPaySlip[1, (int)GridColumns.Loan4_255].Column.Visible =
            //grdPaySlip[1, (int)GridColumns.Loan3_255].Column.Visible =
            //grdPaySlip[1, (int)GridColumns.Loan2_255].Column.Visible =
            //grdPaySlip[1, (int)GridColumns.Loan1_255].Column.Visible = false;

            grdPaySlips[2, (int)GridColumns.Loan1_255].Row.Visible = false;
            grdPaySlips[1, (int)GridColumns.Loan_Deduct25] = new MyHeader("Total Loan Deduction");
            grdPaySlips[1, (int)GridColumns.Loan_Deduct25].Column.Width = 70;

           // grdPaySlip[0, (int)GridColumns.Loan_Deduct25].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Advance_Deduct26] = new MyHeader("Advance Deduction");
            grdPaySlips[0, (int)GridColumns.Advance_Deduct26].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.Misc_Deduct27] = new MyHeader("Misc Deduction");
            grdPaySlips[0, (int)GridColumns.Misc_Deduct27].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Tax_Deduct28] = new MyHeader("Tax Deduction");
            grdPaySlips[0, (int)GridColumns.Tax_Deduct28].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Total_Deduct29] = new MyHeader("Total Deduction");
            grdPaySlips[0, (int)GridColumns.Total_Deduct29].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.Insurence_Amt30] = new MyHeader("Insurence Amt");
            grdPaySlips[0, (int)GridColumns.Insurence_Amt30].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.Net_Salary31] = new MyHeader("Net Salary");
            grdPaySlips[0, (int)GridColumns.Net_Salary31].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Presence32] = new MyHeader("Presence");
            grdPaySlips[0, (int)GridColumns.Presence32].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.PrimaryID33] = new MyHeader("PrimaryID");
            grdPaySlips[0, (int)GridColumns.PrimaryID33].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.Starting_Salary34] = new MyHeader("StartingSalary");
            grdPaySlips[0, (int)GridColumns.Starting_Salary34].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.Starting_Salary34].Column.Visible = false;

            grdPaySlips[0, (int)GridColumns.OnePercentTax35] = new MyHeader("One PercentTax");
            grdPaySlips[0, (int)GridColumns.OnePercentTax35].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.PensionAdjust] = new MyHeader("PensionAdjust");
            grdPaySlips[0, (int)GridColumns.PensionAdjust].RowSpan = 2;

            grdPaySlips[0, (int)GridColumns.PFAdjust] = new MyHeader("PFAdjust");
            grdPaySlips[0, (int)GridColumns.PFAdjust].RowSpan = 2;
            grdPaySlips[0, (int)GridColumns.OnePercentTax35].Column.Visible = 
            grdPaySlips[0, (int)GridColumns.PFAdjust].Column.Visible = 
            grdPaySlips[0, (int)GridColumns.PensionAdjust].Column.Visible= false;

            grdPaySlips[0, 0].Column.Width = 30;
            grdPaySlips[0, (int)GridColumns.SN1].Column.Width = 30;
            grdPaySlips[0, (int)GridColumns.EmployeeID2].Column.Width = 1;
            grdPaySlips[0, (int)GridColumns.Code3].Column.Width = 40;
            grdPaySlips[0, (int)GridColumns.Name4].Column.Width = 120;
            grdPaySlips[0, (int)GridColumns.Designation5].Column.Width = 100;
            grdPaySlips[0, (int)GridColumns.Level6].Column.Width = 100;
            grdPaySlips[0, (int)GridColumns.Basic_Salary7].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.Grade8].Column.Width = 30;
            grdPaySlips[0, (int)GridColumns.Grade_Amt9].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.PF_Add10].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.Pension_Add11].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.Allow_Dearness12].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.Allow_Administrative13].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.Allow_Academic14].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.Allow_General15].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.Allow_Festival16].Column.Width = 80;
           // grdPaySlip[0, 14].Column.Width = 100;
            //grdPaySlip[0, 15].Column.Width = 80;
            //grdPaySlip[0, 16].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.Allow_Misc17].Column.Width = 80;

            grdPaySlips[0, (int)GridColumns.Gross_Amt18].Column.Width = 100;
            grdPaySlips[0, (int)GridColumns.PF_Deduct19].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.PensionF_Deduct20].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.Employee_Welfare21].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.CIT22].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.QC_Accomodation23].Column.Width = 80;
            grdPaySlips[1, (int)GridColumns.QC_Electricity24].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.Loan_Deduct25].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.Advance_Deduct26].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.Misc_Deduct27].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.Tax_Deduct28].Column.Width = 80;
            grdPaySlips[0, (int)GridColumns.Total_Deduct29].Column.Width = 100;
            grdPaySlips[0, (int)GridColumns.Net_Salary31].Column.Width = 100;
            grdPaySlips[0, (int)GridColumns.Presence32].Column.Width = 70;
            grdPaySlips[0, (int)GridColumns.PrimaryID33].Column.Width = 70;

            //grdPaySlip[0, 17].Column.Visible = false;//CIF number
            grdPaySlips[0, (int)GridColumns.EmployeeID2].Column.Visible = false;//Employee ID
            grdPaySlips[0, 0].Column.Visible = false;//checkbox hidden for bishal bazar
            grdPaySlips[0, (int)GridColumns.Code3].Column.Visible = false;
            grdPaySlips[0, (int)GridColumns.Level6].Column.Visible = false;
            grdPaySlips[0, (int)GridColumns.Basic_Salary7].Column.Visible = false;
            //grdPaySlip[0, 11].Column.Visible = false;
            grdPaySlips[1, (int)GridColumns.Allow_Dearness12].Column.Visible = false;
            grdPaySlips[1, (int)GridColumns.Allow_Administrative13].Column.Visible = false;
            grdPaySlips[1, (int)GridColumns.Allow_Academic14].Column.Visible = false;
            //grdPaySlip[1, 32].Column.Visible = false;
            grdPaySlips[1, (int)GridColumns.PrimaryID33].Column.Visible = false;

            //grdPaySlip.FixedColumns = 1;
            
        }

        //Customized header
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Check All")
            {
                check();
            }
            else if (button1.Text == "Uncheck All")
            {
                uncheck();
            }
        }

        private void uncheck()
        {
            //for (int i = 1; i <= drFound.Count(); i++)
            //{
            //    SourceGrid.Cells.CheckBox checkView = new SourceGrid.Cells.CheckBox();
            //    checkView.Checked = false;
            //    grdPaySlip[i, 0] = checkView;
            //    grdPaySlip[i, 0].Value = true;

            //}
            fillGrid();
            button1.Text = "Check All";
        }
        private void check()
        {
            //for (int i = 1; i <= drFound.Count(); i++)
            //{
            //SourceGrid.Cells.CheckBox checkView = new SourceGrid.Cells.CheckBox(null, true);
            //checkView.Checked = true;
            //grdPaySlip[i, 0] = checkView;
            //grdPaySlip[i, 0] = new SourceGrid.Cells.CheckBox(null, true);
            //grdPaySlip[i, 0].Value = false;
            //grdPaySlip.Controls.Find("", true)[0]).Checked = true;
            //}
            fillGrid(true);
            button1.Text = "Uncheck All";
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void getMonth(ComboBox combo)
        {
            combo.Items.Clear();
            BusinessLogic.HRM.Employee emp = new BusinessLogic.HRM.Employee();
            dTable = emp.getMonth();
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                DataRow dr = dTable.Rows[i];
                combo.Items.Add(new ListItem((int)dr["monthId"], dr["monthName"].ToString()));
            }
            combo.SelectedIndex = 0;
            combo.DisplayMember = "value";
            combo.ValueMember = "id";
        }
        private void grdPaySlip_Click(object sender, EventArgs e)
        {

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

        private void btndate_Click(object sender, EventArgs e)
        {
            DateTime dtDate = Date.ToDotNet(txtdate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
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

        private void tppayslip_Click(object sender, EventArgs e)
        {

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
            cmbMonth.SelectedItem = ListDate[FYMonth-1];

        }

        private void LoadDepartment()
        {
            DataTable dtData = null;
            dtData = Hrm.GetDepartmentForCmb();
            DataRow dr = dtData.NewRow();
            dr["ID"] = 0; 
            dr["Value"] = "All"; 
            dtData.Rows.InsertAt(dr, 0);
            if(dtData.Rows.Count > 0)
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
            //dr["ID"] = 0;
            //dr["Value"] = "All";
            //dtData.Rows.InsertAt(dr, 0);
            if (dtData.Rows.Count > 0)
            {
                cmbFaculty.DataSource = null;
                cmbFaculty.DataSource = dtData;
                cmbFaculty.DisplayMember = "Value";
                cmbFaculty.ValueMember = "ID";
                cmbFaculty.SelectedIndex = 0;
            }
        }
        private void fillAdditionDed()
        {
            dTableaddition = employees.getAdditionOnly();
            drFoundaddtion = dTableaddition.Select(FilterString);

            dTableemployee = employees.getEmployee();
            drFoundemployee = dTableemployee.Select(FilterString);


            writeAdditionDed();
            addRowAddition();
        }

        private void addRowAddition()
        {
            grdaddition.Redim(drFoundemployee.Count() + 1, drFoundaddtion.Count() + 4);
            int count = drFoundaddtion.Count() + 3;
            for (int i = 1; i <= drFoundemployee.Count(); i++)
            {
                DataRow dr = drFoundemployee[i - 1];
                grdaddition[i, 0] = new SourceGrid.Cells.Cell(dr["ID"].ToString());
                grdaddition[i, 1] = new SourceGrid.Cells.Cell(dr["StaffCode"].ToString());
                grdaddition[i, 2] = new SourceGrid.Cells.Cell(dr["Name"].ToString());

                for (int ir = 1; ir <= drFoundaddtion.Count(); ir++)
                {
                    SourceGrid.Cells.Editors.TextBox txtallowances1 = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    txtallowances1.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                    grdaddition[i, ir + 2] = new SourceGrid.Cells.Cell("", txtallowances1);
                    grdaddition[i, ir + 2].AddController(evtAllowanceFocusLost);
                    grdaddition[i, ir + 2].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                }
                SourceGrid.Cells.Editors.TextBox txttotal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txttotal.EditableMode = SourceGrid.EditableMode.None;
                grdaddition[i, count] = new SourceGrid.Cells.Cell("", txttotal);
                grdaddition[i, count].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            }
        }

        private void writeAdditionDed()
        {
            grdaddition.Rows.Clear();
            grdaddition.Redim(1, drFoundaddtion.Count() + 4);

            grdaddition[0, 0] = new MyHeader("ID");
            grdaddition[0, 1] = new MyHeader("Code");
            grdaddition[0, 2] = new MyHeader("Employee Name");

            for (int i = 1; i <= drFoundaddtion.Count(); i++)
            {
                DataRow dr = drFoundaddtion[i - 1];
                grdaddition[0, i + 2] = new MyHeader(dr["Name"].ToString());
                grdaddition[0, i + 2].Column.Width = 140;
            }
            int ii = drFoundaddtion.Count() + 3;
            TotalCount = ii;
            grdaddition[0, ii] = new MyHeader("Total Addition");

            grdaddition[0, 0].Column.Width = 1;
            grdaddition[0, 1].Column.Width = 60;
            grdaddition[0, 2].Column.Width = 130;
            grdaddition[0, ii].Column.Width = 120;

            grdaddition[0, 0].Column.Visible = false;
        }

        private void btnpay_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.MsgQuest(" तपाइँले  " + cmbMonth.Text + "  महिनाको तलब बनाउँदै हुनुहुन्छ । के यो साँचो हो? \n यदि ठिक बटन थिच्नुभयो भने त्यसको जिम्मेवार तपाई  आफैँ हुनुपर्ने छ । \n  ठिक वा बेठिक बटन थिच्नुहोस् ।") != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                bool chkUserPermission = UserPermission.ChkUserPermission("HRM_SALARY_SAVE");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to save salary sheet. Please contact your administrator for permission.");
                    return;
                }
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
                dtLoan.Columns.Add("EmployeeID");
                dtLoan.Columns.Add("LoanID");
                //dtLoan.Columns.Add("PaySlipID");
                dtLoan.Columns.Add("LoanMthPremium");
               

                //For MAsterPaySlip
                DataTable dtabledesignation = new DataTable();
                DataTable dtEmployeeMasterDetails = new DataTable();
                dtEmployeeMasterDetails.Columns.Add("employeeID");
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

                        ps.employeeID = Convert.ToInt32(grdPaySlips[i, (int)GridColumns.EmployeeID2].Value);
                        ps.employeeCode = grdPaySlips[i, (int)GridColumns.Code3].ToString();
                        ps.employeeName = grdPaySlips[i, (int)GridColumns.Name4].ToString();
                        dtabledesignation = employees.getDesigIdByName(grdPaySlips[i, (int)GridColumns.Designation5].ToString());
                        DataRow drdesignation = dtabledesignation.Rows[0];
                        ps.designationID = Convert.ToInt32(drdesignation["DesignationID"]);
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
                                    dtLoan.Rows.Add(Convert.ToInt32(grdPaySlips[i, (int)GridColumns.EmployeeID2].Value),
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
                            dtEmployeeMasterDetails.Rows.Add(ps.employeeID, ps.employeeCode, ps.employeeName, ps.designationID, ps.empLevel, ps.basicSalary, ps.grade, ps.gradeAmount,
                                ps.pfAmount, ps.pensionfAmount, ps.inflationAlw, ps.admAlw, ps.academicAlw, ps.postAlw, ps.festivalAlw, ps.miscAllowance, ps.overTimeAlw, ps.grossAmount, ps.pfDeduct, ps.pensionfDeduct,
                                ps.taxDeduct, ps.KKDeduct, ps.NLKoshDeduct, ps.accommodation, ps.electricity, ps.loan, ps.advanceDeduct, ps.MiscDeduct, 
                                ps.totalDeduct, ps.netSalary, ps.EmpPresence, ps.OnePercentTax, ps.PFAdjust,ps.PensionAdjust,ps.InsuranceAmt);
                        }
                        //ps.absentDays = Convert.ToInt32(grdPaySlip[i, 7].ToString());
                        //ps.payableSalary = Convert.ToDouble(grdPaySlip[i, 9].ToString());
                        //ps.miscDeduction = Convert.ToDouble(grdPaySlip[i, 8].ToString());
                        //ps.pfAmount = Convert.ToDouble(grdPaySlip[i, 10].ToString());
                        //ps.basicAllowance = Convert.ToDouble(grdPaySlip[i, 11].ToString());
                        //ps.bonus = Convert.ToDouble(grdPaySlip[i, 12].ToString());
                        //ps.tada = Convert.ToDouble(grdPaySlip[i, 13].ToString());
                        //ps.otherAllowances = Convert.ToDouble(grdPaySlip[i, 14].ToString());
                        //ps.totalAllowances = Convert.ToDouble(grdPaySlip[i, 15].ToString());
                        //ps.pfDeduction = Convert.ToDouble(grdPaySlip[i, 16].ToString());
                        //ps.cifNo = Convert.ToDouble(grdPaySlip[i, 17].ToString());
                        //ps.citamount = Convert.ToDouble(grdPaySlip[i, 18].ToString());
                        //ps.advance = Convert.ToDouble(grdPaySlip[i, 19].ToString());
                        //ps.TDS = Convert.ToDouble(grdPaySlip[i, 20].ToString());
                        //ps.netPayableAmount = Convert.ToDouble(grdPaySlip[i, 21].ToString());
                        //dtEmployeeMasterDetails.Rows.Add(ps.employeeID, ps.employeeCode, ps.employeeName, ps.designationID, ps.empLevel, ps.basicSalary, ps.grade, ps.gradeAmount, ps.pfAmount, ps.basicAllowance, ps.foodAllowance, ps.miscAllowance, ps.grossAmount, ps.pfDeduct, ps.taxDeduct, ps.KKShort, ps.KKLong, ps.KKInterest, ps.SLInstalment, ps.SLInterest, ps.NLKoshDeduct, ps.MiscDeduct, ps.totalDedcut, ps.netSalary);

                        //not for bishal bazar                                                                                                            
                        ////Checking if a pay slip has been already been generated for an employee for the month                                                                   
                        //if(BusinessLogic.HRM.Employee.CheckPayMonth(ID,ps.employeeID,FYStartDate))                                                                               
                        //{                                                                                                                                                        
                        //    MessageBox.Show("Pay Slip for " + ps.employeeName + " has already been generated for the month of " + monthName1+".\nPlease uncheck the employee."); 
                        //    return;                                                                                                                                              
                        //}                                                                                                                                                        


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
                //if(checkEmpSelect==false)
                //{
                //    Global.Msg("Please select an employee");
                //    return;
                //}
                //For Allowances
                // DataTable dtallowances = new DataTable();
                // dtallowances.Columns.Add("paysliID");
                // dtallowances.Columns.Add("empId");
                // dtallowances.Columns.Add("empname");
                // dtallowances.Columns.Add("allowanceID");
                // dtallowances.Columns.Add("amount");
                // for (int j = 1; j <= drFoundemployee.Count(); j++)
                // {
                //     if (grdPaySlip[j, 0].Value.ToString() == "True")
                //     {
                //         int paysliID = Convert.ToInt32(grdPaySlip[j, 1].Value);
                //         int empId = Convert.ToInt32(grdaddition[j, 0].Value);
                //         string empname = grdaddition[j, 2].Value.ToString();
                //         for (int i = 1; i <= drFoundaddtion.Count(); i++)
                //         {
                //             string allowancesName = employees.GetAddDedIdByName(grdaddition[0, i + 2].Value.ToString());
                //             double amount = Convert.ToDouble(grdaddition[j, i + 2].Value);
                //             //DataTable dt = employees.insertIntoPaySlipAllowances(paysliID, empId, empname, allowancesName, amount);
                //             dtallowances.Rows.Add(paysliID, empId, empname, allowancesName, amount);
                //         }
                //     }
                // }
                ////For Deduct
                // DataTable dtdeduct = new DataTable();
                // dtdeduct.Columns.Add("empID");
                // dtdeduct.Columns.Add("paySlipID");
                // dtdeduct.Columns.Add("empname");
                // dtdeduct.Columns.Add("deductionID");
                // dtdeduct.Columns.Add("amount");
                // for (int i = 1; i <= drFoundemployee.Count(); i++)
                // {
                //     if (grdPaySlip[i, 0].Value.ToString() == "True")
                //     {
                //         int paySlipID = Convert.ToInt32(grdPaySlip[i, 1].Value);
                //         int empID = Convert.ToInt32(grdDeduction[i, 0].Value);
                //         string empname = grdDeduction[i, 2].Value.ToString();
                //         for (int j = 1; j <= drFoundDeduction.Count(); j++)
                //         {
                //             string deductName = employees.GetAddDedIdByName(grdDeduction[0, j + 2].Value.ToString());
                //             double amount = Convert.ToDouble(grdDeduction[i, j + 2].Value);
                //             //DataTable dt = employees.insertIntoPaySlipDeduction(empID, paySlipID, empname, deductName, amount);
                //             dtdeduct.Rows.Add(empID, paySlipID, empname, deductName, amount);
                //         }
                //     }
                // }
                if (_paySlipID > 0)
                    BusinessLogic.HRM.Employee.UpdateSavedPaySlip(_paySlipID, ID, monthName1, Created_Date, Created_By, Modified_by, selectedYear, DepartID, FacultyID, 0, tSalary, tGrade, tPF, tPensionF, tInflationAlw, tAdmAlw, tAcademicAlw, tPostAlw, tFestivalAlw, tMiscAllow, tGrossAmount, tPFDeduct, tPensionFDeduct, tTaxDeduct, tKK, tNLK, tAccommodation, tElectricity, tLoan, tAdvance, tMiscDeduct, tTotalDeduct, tNetSalary, chkFestiveMonth.Checked, txtnarration.Text.Trim(), dtEmployeeMasterDetails, tOverTimeAlw, tOnePercentTax, tPFAdjust, tPensionAdjust, dtLoan,tInsuranceAmt, dtSalaryAdjust);//, dtallowances, dtdeduct);
                else
                    BusinessLogic.HRM.Employee.SavePaySlip(0, ID, monthName1, Created_Date, Created_By, Modified_by, selectedYear, DepartID, FacultyID, 0, tSalary, tGrade, tPF, tPensionF, tInflationAlw, tAdmAlw, tAcademicAlw, tPostAlw, tFestivalAlw, tMiscAllow, tGrossAmount, tPFDeduct, tPensionFDeduct, tTaxDeduct, tKK, tNLK, tAccommodation, tElectricity, tLoan, tAdvance, tMiscDeduct, tTotalDeduct, tNetSalary, chkFestiveMonth.Checked, txtnarration.Text.Trim(), dtEmployeeMasterDetails, tOverTimeAlw, tOnePercentTax, tPFAdjust, tPensionAdjust, dtLoan, tInsuranceAmt, dtSalaryAdjust);//, dtallowances, dtdeduct);
                //Post The Journal For Respective Transaction
                //Load the saved value and other changes
                LoadSavedGrid();
                dtSalaryAdjust = InitSalaryAdjsutTable();
                //not for bishal bazar
                //BusinessLogic.HRM.PaySlipGenerate PaySlip = new BusinessLogic.HRM.PaySlipGenerate();
                //VoucherConfiguration m_VouConfig = new VoucherConfiguration();



                //for (CheckRow = 0; CheckRow < grdPaySlip.Rows.Count - 1; CheckRow++)
                //{
                //    if (grdPaySlip[CheckRow + 1, 0].Value.ToString() == "True")
                //    {
                //        int serieid = 284;//Journal Main Series ID
                //        string NumberingType = m_VouConfig.GetVouNumberingType(serieid);
                //        if (NumberingType == "AUTOMATIC")
                //        {
                //            object m_vounum = m_VouConfig.GenerateVouNumType(serieid);
                //            if (m_vounum == null)
                //            {
                //                MessageBox.Show("Your voucher numbers are totally finished!");
                //                return;
                //            }
                //            txtVchNo.Text = m_vounum.ToString();
                //            txtVchNo.Enabled = false;
                //        }

                //        string JournalXMLString = ReadAllJournalEntry();
                //        if (JournalXMLString == string.Empty)
                //        {
                //            MessageBox.Show("Unable to cast PaySlip entry to XML!");
                //            return;
                //        }
                //        #region Save xml data to Database
                //        using (System.IO.StringWriter swStringWriter = new StringWriter())
                //        {
                //            using (SqlCommand dbCommand = new SqlCommand("Acc.xmlJournalInsert", Global.m_db.cn))
                //            {
                //                // we are going to use store procedure  
                //                dbCommand.CommandType = CommandType.StoredProcedure;
                //                // Add input parameter and set its properties.
                //                SqlParameter parameter = new SqlParameter();
                //                // Store procedure parameter name  
                //                parameter.ParameterName = "@journal";
                //                // Parameter type as XML 
                //                parameter.DbType = DbType.Xml;
                //                parameter.Direction = ParameterDirection.Input; // Input Parameter  
                //                parameter.Value = JournalXMLString; // XML string as parameter value  
                //                // Add the parameter in Parameters collection.
                //                dbCommand.Parameters.Add(parameter);
                //                if (Global.m_db.cn.State == ConnectionState.Closed)
                //                {
                //                    Global.m_db.cn.Open();
                //                }
                //               // int intRetValue = dbCommand.ExecuteNonQuery();
                //                //MessageBox.Show(intRetValue.ToString());                                
                //            }
                //        }
                //        #endregion
                //    }
                //}
            }
            catch (SqlException ex)
            {
                Global.MsgError(ex.Message + " " + ex.LineNumber);
                dtSalaryAdjust.Columns.Remove("PaySlipID");
            } 
        }

        private void btndone_Click(object sender, EventArgs e)
        {
            DataTable dtaddition = new DataTable();
            dtaddition.Columns.Add("EmployeeID");
            dtaddition.Columns.Add("TotalAdditionAllowance");

            for (int i = 1; i < grdaddition.Rows.Count; i++)
            {
                dtaddition.Rows.Add(grdaddition[i, 0].Value, grdaddition[i, TotalCount].Value);
            }

            for (int j = 1; j < grdPaySlips.Rows.Count; j++)
            {
                foreach (DataRow dr in dtaddition.Rows)
                {
                    if (dr["EmployeeID"].ToString() == grdPaySlips[j, 2].Value.ToString())
                    {
                        grdPaySlips[j, 14].Value = dr["TotalAdditionAllowance"].ToString();
                        break;
                    }
                }
            }
            for (int p = 1; p < grdPaySlips.Rows.Count; p++)
            {
                double BasicSalary = Convert.ToDouble(grdPaySlips[p, 6].ToString());
                int DaysinMonth = Convert.ToInt32(lblnumberofdays.Text);
                int AbsentDays = Convert.ToInt32(grdPaySlips[p, 7].ToString());
                int PresentDays = DaysinMonth - AbsentDays;
                double PayableSalary1 = (BasicSalary * PresentDays) / DaysinMonth;
                double PayableSalary = ((BasicSalary * PresentDays) / DaysinMonth) - Convert.ToDouble(grdPaySlips[p, 8].Value);
                grdPaySlips[p, 9].Value = PayableSalary.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                grdPaySlips[p, 10].Value = (PayableSalary * .10).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                grdPaySlips[p, 16].Value = (PayableSalary * .10 * 2).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                double tdsvalue = 0;
                double netpayable = 0;
                grdPaySlips[p, 15].Value = (Convert.ToDouble(grdPaySlips[p, 11].Value) + Convert.ToDouble(grdPaySlips[p, 12].Value) + Convert.ToDouble(grdPaySlips[p, 13].Value) + Convert.ToDouble(grdPaySlips[p, 14].Value)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)); ;
                tdsvalue = (Convert.ToDouble(grdPaySlips[p, 9].Value) + Convert.ToDouble(grdPaySlips[p, 10].Value) + Convert.ToDouble(grdPaySlips[p, 15].Value) - Convert.ToDouble(grdPaySlips[p, 16].Value) - Convert.ToDouble(grdPaySlips[p, 18].Value)) * .01;
                grdPaySlips[p, 20].Value = tdsvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                netpayable = (Convert.ToDouble(grdPaySlips[p, 9].Value) + Convert.ToDouble(grdPaySlips[p, 10].Value) + Convert.ToDouble(grdPaySlips[p, 15].Value) - Convert.ToDouble(grdPaySlips[p, 16].Value) - Convert.ToDouble(grdPaySlips[p, 18].Value)) - tdsvalue;
                grdPaySlips[p, 21].Value = netpayable.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            }

            tabControl1.SelectedIndex = 0;

        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void evtDeductionFocuslost_FocusLeft(object sender, EventArgs e)
        {
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int pos = CurrRowPosDeduction;
            CurrRowPosDeduction = ctx.Position.Row;

            OtherDeduction = 0;
            //AdditionalAllowance = 0;
            for (int i = 1; i <= drFoundDeduction.Count(); i++)
            {
                double AddValue = Convert.ToDouble(grdDeduction[CurrRowPosDeduction, i + 2].Value);
                grdDeduction[CurrRowPosDeduction, i + 2].Value = AddValue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                OtherDeduction = OtherDeduction + Convert.ToDouble(grdDeduction[CurrRowPosDeduction, i + 2].Value);

            }
            int totalDeduction = drFoundDeduction.Count() + 3;
            grdDeduction[CurrRowPosDeduction, totalDeduction].Value = OtherDeduction.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

        }
        private void fillDeduction()
        {

            grdDeduction.Rows.Clear();

            writeDeduction();
            addRowDeduction();
        }
        private void addRowDeduction()
        {
            grdDeduction.Redim(drFoundemployee.Count() + 1, drFoundDeduction.Count() + 4);
            int count = drFoundDeduction.Count() + 3;
            for (int i = 1; i <= drFoundemployee.Count(); i++)
            {
                DataRow dr = drFoundemployee[i - 1];
                grdDeduction[i, 0] = new SourceGrid.Cells.Cell(dr["ID"].ToString());
                grdDeduction[i, 1] = new SourceGrid.Cells.Cell(dr["StaffCode"].ToString());
                grdDeduction[i, 2] = new SourceGrid.Cells.Cell(dr["Name"].ToString());

                for (int ir = 1; ir <= drFoundDeduction.Count(); ir++)
                {
                    SourceGrid.Cells.Editors.TextBox txtallowances1 = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    txtallowances1.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                    grdDeduction[i, ir + 2] = new SourceGrid.Cells.Cell("", txtallowances1);
                    grdDeduction[i, ir + 2].AddController(evtDeductionFocuslost);
                    //grdDeduction[i, ir + 2].AddController(evtdeductTotal);
                    grdDeduction[i, ir + 2].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                    //for total
                    //SourceGrid.Cells.Editors.TextBox txttotalded = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    //txttotalded.EditableMode = SourceGrid.EditableMode.None;
                    //grdDeduction[i + 1, ir+2] = new SourceGrid.Cells.Cell("",txttotalded);
                    //grdDeduction[i + 1, ir + 2].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                    //SourceGrid.Cells.Editors.TextBox txttotalall = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    //txttotalall.EditableMode = SourceGrid.EditableMode.None;
                    //grdDeduction[i, ir + 3] = new SourceGrid.Cells.Cell("", txttotalall);
                    //grdDeduction[i, ir + 3].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }
                SourceGrid.Cells.Editors.TextBox txttotal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txttotal.EditableMode = SourceGrid.EditableMode.None;
                grdDeduction[i, count] = new SourceGrid.Cells.Cell("", txttotal);
                grdDeduction[i, count].Value = 0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                //grdDeduction[i + 1, 1] = new SourceGrid.Cells.Cell("Total");

            }
        }
        private void writeDeduction()
        {
            grdDeduction.Rows.Clear();
            grdDeduction.Redim(1, drFoundDeduction.Count() + 4);

            grdDeduction[0, 0] = new MyHeader("ID");
            grdDeduction[0, 1] = new MyHeader("Code");
            grdDeduction[0, 2] = new MyHeader("Employee Name");

            for (int i = 1; i <= drFoundDeduction.Count(); i++)
            {
                DataRow dr = drFoundDeduction[i - 1];
                grdDeduction[0, i + 2] = new MyHeader(dr["Name"].ToString());
                grdDeduction[0, i + 2].Column.Width = 140;
            }
            int ii = drFoundDeduction.Count() + 3;
            TotalCountDeduction = ii;
            grdDeduction[0, ii] = new MyHeader("Total Addition");
            grdDeduction[0, 0].Column.Width = 1;
            grdDeduction[0, 1].Column.Width = 60;
            grdDeduction[0, 2].Column.Width = 130;
            grdDeduction[0, ii].Column.Width = 120;

            grdDeduction[0, 0].Column.Visible = false;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (cmbMonth.SelectedIndex == -1)
            {
                MessageBox.Show("please select month first", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                DataTable dtdeduction = new DataTable();
                dtdeduction.Columns.Add("EmployeeID");
                dtdeduction.Columns.Add("TotalDeduction");

                for (int i = 1; i < grdDeduction.Rows.Count; i++)
                {
                    dtdeduction.Rows.Add(grdDeduction[i, 0].Value, grdDeduction[i, TotalCountDeduction].Value);
                }
                if (cmbMonth.SelectedIndex == -1)
                {

                }
                for (int j = 1; j < grdPaySlips.Rows.Count; j++)
                {
                    foreach (DataRow dr in dtdeduction.Rows)
                    {
                        if (dr["EmployeeID"].ToString() == grdPaySlips[j, 2].Value.ToString())
                        {
                            grdPaySlips[j, 8].Value = dr["TotalDeduction"].ToString();
                            break;
                        }
                    }
                }
                for (int p = 1; p < grdPaySlips.Rows.Count; p++)
                {
                    double BasicSalary = Convert.ToDouble(grdPaySlips[p, 6].ToString());
                    int DaysinMonth = Convert.ToInt32(lblnumberofdays.Text);
                    int AbsentDays = Convert.ToInt32(grdPaySlips[p, 7].ToString());
                    int PresentDays = DaysinMonth - AbsentDays;
                    double PayableSalary1 = (BasicSalary * PresentDays) / DaysinMonth;
                    double PayableSalary = ((BasicSalary * PresentDays) / DaysinMonth) - Convert.ToDouble(grdPaySlips[p, 8].Value);
                    grdPaySlips[p, 9].Value = PayableSalary.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdPaySlips[p, 10].Value = (PayableSalary * .10).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdPaySlips[p, 16].Value = (PayableSalary * .10 * 2).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    double tdsvalue = 0;
                    double netpayable = 0;
                    grdPaySlips[p, 15].Value = (Convert.ToDouble(grdPaySlips[p, 11].Value) + Convert.ToDouble(grdPaySlips[p, 12].Value) + Convert.ToDouble(grdPaySlips[p, 13].Value) + Convert.ToDouble(grdPaySlips[p, 14].Value)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)); ;
                    tdsvalue = (Convert.ToDouble(grdPaySlips[p, 9].Value) + Convert.ToDouble(grdPaySlips[p, 10].Value) + Convert.ToDouble(grdPaySlips[p, 15].Value) - Convert.ToDouble(grdPaySlips[p, 16].Value) - Convert.ToDouble(grdPaySlips[p, 18].Value)) * .01;
                    grdPaySlips[p, 20].Value = tdsvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    netpayable = (Convert.ToDouble(grdPaySlips[p, 9].Value) + Convert.ToDouble(grdPaySlips[p, 10].Value) + Convert.ToDouble(grdPaySlips[p, 15].Value) - Convert.ToDouble(grdPaySlips[p, 16].Value) - Convert.ToDouble(grdPaySlips[p, 18].Value)) - tdsvalue;
                    grdPaySlips[p, 21].Value = netpayable.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }

                tabControl1.SelectedIndex = 0;

            }
        }

        private void LoadPaySlip()
        {
            for (int p = 1; p < grdPaySlips.Rows.Count; p++)
            {
                double BasicSalary = Convert.ToDouble(grdPaySlips[p, 6].ToString());
                int DaysinMonth = Convert.ToInt32(lblnumberofdays.Text);
                int AbsentDays = Convert.ToInt32(grdPaySlips[p, 7].ToString());
                int PresentDays = DaysinMonth - AbsentDays;
                double PayableSalary1 = (BasicSalary * PresentDays) / DaysinMonth;
                double PayableSalary = ((BasicSalary * PresentDays) / DaysinMonth) - Convert.ToDouble(grdPaySlips[p, 8].Value);
                grdPaySlips[p, 9].Value = PayableSalary.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                int employeeid = Convert.ToInt32(grdPaySlips[p, 2].Value);
                bool PF = Hrm.CheckPF(employeeid);
                if (PF == true)
                {
                    grdPaySlips[p, 10].Value = (PayableSalary * .10).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdPaySlips[p, 16].Value = (PayableSalary * .10 * 2).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }
                else
                {
                    grdPaySlips[p, 10].Value = 0.00;
                    grdPaySlips[p, 16].Value = 0.00;
                }
                double tdsvalue = 0;
                double netpayable = 0;

                grdPaySlips[p, 15].Value = (Convert.ToDouble(grdPaySlips[p, 11].Value) + Convert.ToDouble(grdPaySlips[p, 12].Value) + Convert.ToDouble(grdPaySlips[p, 13].Value) + Convert.ToDouble(grdPaySlips[p, 14].Value)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                tdsvalue = (Convert.ToDouble(grdPaySlips[p, 9].Value) + Convert.ToDouble(grdPaySlips[p, 10].Value) + Convert.ToDouble(grdPaySlips[p, 15].Value) - Convert.ToDouble(grdPaySlips[p, 16].Value) - Convert.ToDouble(grdPaySlips[p, 18].Value)) * .01;
                grdPaySlips[p, 20].Value = tdsvalue.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                netpayable = (Convert.ToDouble(grdPaySlips[p, 9].Value) + Convert.ToDouble(grdPaySlips[p, 10].Value) + Convert.ToDouble(grdPaySlips[p, 15].Value) - Convert.ToDouble(grdPaySlips[p, 16].Value) - Convert.ToDouble(grdPaySlips[p, 18].Value)) - tdsvalue;
                grdPaySlips[p, 21].Value = netpayable.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            }
        }
        private void paySlipDeduct()
        {
            for (int i = 1; i <= drFoundemployee.Count(); i++)
            {
                if (grdPaySlips[i, 0].Value.ToString() == "True")
                {
                    int paySlipID = Convert.ToInt32(grdPaySlips[i, 1].Value);
                    int empID = Convert.ToInt32(grdDeduction[i, 0].Value);
                    string empname = grdDeduction[i, 2].Value.ToString();
                    for (int j = 1; j <= drFoundDeduction.Count(); j++)
                    {
                        string deductName = grdDeduction[0, j + 2].Value.ToString();
                        double amount = Convert.ToDouble(grdDeduction[i, j + 2].Value);
                        DataTable dt = employees.insertIntoPaySlipDeduction(empID, paySlipID, empname, deductName, amount);
                    }
                }
            }
        }
        private void paySlipAllowances()
        {
            for (int j = 1; j <= drFoundemployee.Count(); j++)
            {
                if (grdPaySlips[j, 0].Value.ToString() == "True")
                {
                    int paysliID = Convert.ToInt32(grdPaySlips[j, 1].Value);
                    int empId = Convert.ToInt32(grdaddition[j, 0].Value);
                    string empname = grdaddition[j, 2].Value.ToString();
                    for (int i = 1; i <= drFoundaddtion.Count(); i++)
                    {
                        string allowancesName = grdaddition[0, i + 2].Value.ToString();
                        double amount = Convert.ToDouble(grdaddition[j, i + 2].Value);
                        DataTable dt = employees.insertIntoPaySlipAllowances(paysliID, empId, empname, allowancesName, amount);
                    }
                }
            }
        }
        private void SalaryPaySlipMaster()
        {
            ListItem li = (ListItem)cmbMonth.SelectedItem;
            int ID = li.ID;
            string monthName1 = li.Value;
            DateTime Created_Date = System.Convert.ToDateTime(Date.GetServerDate());
            string Created_By = User.CurrentUserName;
            string Modified_by = User.CurrentUserName;
            DataTable dt = employees.insertIntoMasterPaySlip(ID, monthName1, Created_Date, Created_By, Modified_by);
        }
        //private void paySlipMasterDetails()
        //{
        //    DataTable dtabledesignation = new DataTable();
        //    for (int i = 1; i <= drFoundemployee.Count(); i++)
        //    {
        //        string st = grdPaySlip[i, 0].Value.ToString();
        //        if (grdPaySlip[i, 0].Value.ToString() == "True")
        //        {
        //            try
        //            {
        //                BusinessLogic.HRM.paySlipDetails ps = new BusinessLogic.HRM.paySlipDetails();
        //                ps.employeeID = Convert.ToInt32(grdPaySlip[i, 2].Value);
        //                ps.employeeCode = grdPaySlip[i, 3].ToString();
        //                ps.employeeName = grdPaySlip[i, 4].ToString();
        //                dtabledesignation = employees.getDesigIdByName(grdPaySlip[i, 5].ToString());
        //                DataRow drdesignation = dtabledesignation.Rows[0];
        //                ps.designationID = Convert.ToInt32(drdesignation["DesignationID"]);
        //                ps.basicSalary = Convert.ToDouble(grdPaySlip[i, 6].ToString());
        //                ps.absentDays = Convert.ToInt32(grdPaySlip[i, 7].ToString());
        //                ps.payableSalary = Convert.ToDouble(grdPaySlip[i, 9].ToString());
        //                ps.miscDeduction = Convert.ToDouble(grdPaySlip[i, 8].ToString());
        //                ps.pfAmount = Convert.ToDouble(grdPaySlip[i, 10].ToString());
        //                ps.basicAllowance = Convert.ToDouble(grdPaySlip[i, 11].ToString());
        //                ps.bonus = Convert.ToDouble(grdPaySlip[i, 12].ToString());
        //                ps.tada = Convert.ToDouble(grdPaySlip[i, 13].ToString());
        //                ps.otherAllowances = Convert.ToDouble(grdPaySlip[i, 14].ToString());
        //                ps.totalAllowances = Convert.ToDouble(grdPaySlip[i, 15].ToString());
        //                ps.pfDeduction = Convert.ToDouble(grdPaySlip[i, 16].ToString());
        //                ps.cifNo = Convert.ToDouble(grdPaySlip[i, 17].ToString());
        //                ps.citamount = Convert.ToDouble(grdPaySlip[i, 18].ToString());
        //                ps.advance = Convert.ToDouble(grdPaySlip[i, 19].ToString());
        //                ps.TDS = Convert.ToDouble(grdPaySlip[i, 20].ToString());
        //                ps.netPayableAmount = Convert.ToDouble(grdPaySlip[i, 21].ToString());
        //                ps.paySlipid = Convert.ToInt32(grdPaySlip[i, 1].ToString());
        //                DataTable dt = employees.insertIntoPaySlipMasterDetails(ps);
        //            }
        //            catch (Exception ex)
        //            {
        //                Global.MsgError(ex.Message);
        //            }

        //        }
        //    }
        //}

        private string ReadAllJournalEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            int liProjectID = 1;

            ////validate grid entry
            //if (!ValidateGrid())
            //    return string.Empty;

            tw.WriteStartDocument();
            #region  Journal
            tw.WriteStartElement("JOURNAL");
            {
                ///For Journal Master Section
                int SeriesID = 284;
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime Journal_Date = Date.ToDotNet(txtdate.Text);
                // string Remarks = System.Convert.ToString(txtRemarks.Text);
                string Remarks = "Pay Slip Generator";
                int ProjectID = 1;
                DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                string Created_By = System.Convert.ToString("Admin");
                DateTime Modified_Date = System.Convert.ToDateTime(DateTime.Now);
                string Modified_By = System.Convert.ToString("Admin");
                tw.WriteStartElement("JOURNALMASTER");
                tw.WriteElementString("SeriesID", SeriesID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("Journal_Date", Date.ToDB(Journal_Date));
                tw.WriteElementString("Remarks", Remarks.ToString());
                tw.WriteElementString("ProjectID", ProjectID.ToString());
                tw.WriteElementString("Created_Date", Date.ToDB(Created_Date));
                tw.WriteElementString("Created_By", Created_By.ToString());
                tw.WriteElementString("Modified_Date", Date.ToDB(Modified_Date));
                tw.WriteElementString("Modified_By", Modified_By.ToString());
                tw.WriteEndElement();
                ///For journal Detail Section             
                int JournalID = 0;
                int LedgerID = 0;
                Decimal Amount = 0;
                string DrCr = "";
                string RemarksDetail = "";
                tw.WriteStartElement("JOURNALDETAIL");

                //For Basic Salary
                LedgerID = 487;
                Amount = System.Convert.ToDecimal(grdPaySlips[CheckRow + 1, 6].Value);
                DrCr = "Debit";
                RemarksDetail = "Salary For Month Of" + " " + cmbMonth.Text;
                tw.WriteStartElement("DETAIL");
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Amount", Amount.ToString());
                tw.WriteElementString("DrCr", DrCr.ToString());
                tw.WriteElementString("Remarks", RemarksDetail.ToString());
                tw.WriteEndElement();
                //For PF Contribution
                LedgerID = 495;
                Amount = System.Convert.ToDecimal(grdPaySlips[CheckRow + 1, 10].Value);
                DrCr = "Debit";
                RemarksDetail = "Salary For Month Of" + " " + cmbMonth.Text;
                tw.WriteStartElement("DETAIL");
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Amount", Amount.ToString());
                tw.WriteElementString("DrCr", DrCr.ToString());
                tw.WriteElementString("Remarks", RemarksDetail.ToString());
                tw.WriteEndElement();
                //For Allowances
                LedgerID = 496;
                Amount = System.Convert.ToDecimal(grdPaySlips[CheckRow + 1, 15].Value);
                DrCr = "Debit";
                RemarksDetail = "Salary For Month Of" + " " + cmbMonth.Text;
                tw.WriteStartElement("DETAIL");
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Amount", Amount.ToString());
                tw.WriteElementString("DrCr", DrCr.ToString());
                tw.WriteElementString("Remarks", RemarksDetail.ToString());
                tw.WriteEndElement();
                //For PF Payable
                LedgerID = 493;
                Amount = System.Convert.ToDecimal(grdPaySlips[CheckRow + 1, 16].Value);
                DrCr = "Credit";
                RemarksDetail = "Salary For Month Of" + " " + cmbMonth.Text;
                tw.WriteStartElement("DETAIL");
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Amount", Amount.ToString());
                tw.WriteElementString("DrCr", DrCr.ToString());
                tw.WriteElementString("Remarks", RemarksDetail.ToString());
                tw.WriteEndElement();
                //For CIF Payable
                LedgerID = 494;
                Amount = System.Convert.ToDecimal(grdPaySlips[CheckRow + 1, 18].Value);
                DrCr = "Credit";
                RemarksDetail = "Salary For Month Of" + " " + cmbMonth.Text;
                tw.WriteStartElement("DETAIL");
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Amount", Amount.ToString());
                tw.WriteElementString("DrCr", DrCr.ToString());
                tw.WriteElementString("Remarks", RemarksDetail.ToString());
                tw.WriteEndElement();
                //For TDS
                LedgerID = 492;
                Amount = System.Convert.ToDecimal(grdPaySlips[CheckRow + 1, 20].Value);
                DrCr = "Credit";
                RemarksDetail = "Salary For Month Of" + " " + cmbMonth.Text;
                tw.WriteStartElement("DETAIL");
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Amount", Amount.ToString());
                tw.WriteElementString("DrCr", DrCr.ToString());
                tw.WriteElementString("Remarks", RemarksDetail.ToString());
                tw.WriteEndElement();
                //For Employee

                string LedgerName = grdPaySlips[CheckRow + 1, 3].Value.ToString() + "-" + grdPaySlips[CheckRow + 1, 4].Value.ToString();
                LedgerID = Ledger.GetLedgerIdFromName(LedgerName, Lang.English);
                Amount = System.Convert.ToDecimal(grdPaySlips[CheckRow + 1, 21].Value);
                DrCr = "Credit";
                RemarksDetail = "Salary For Month Of" + " " + cmbMonth.Text;
                tw.WriteStartElement("DETAIL");
                tw.WriteElementString("JournalID", JournalID.ToString());
                tw.WriteElementString("LedgerID", LedgerID.ToString());
                tw.WriteElementString("Amount", Amount.ToString());
                tw.WriteElementString("DrCr", DrCr.ToString());
                tw.WriteElementString("Remarks", RemarksDetail.ToString());
                tw.WriteEndElement();

                tw.WriteEndElement();//End of Tag Journal Details
                //Write Checked Accounting class ID
                try
                {
                    int[] a = new int[] { 1, 2 };
                    tw.WriteStartElement("ACCCLASSIDS");
                    foreach (int _AccClassID in a)
                    {
                        tw.WriteElementString("AccID", _AccClassID.ToString());
                    }
                    //for (int i = 0; i < dtaccclass.Rows.Count; i++)
                    //{
                    //    DataRow dr = dtaccclass.Rows[i];
                    //    tw.WriteElementString("AccID", dr["AccClassID"].ToString());
                    //}

                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            // MessageBox.Show(strXML);       
            return strXML;
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            //bool prevCommaSeperator = Global.Comma_Separated;
            //int prevDecimalPlaces = Global.DecimalPlaces;
            //Global.Comma_Separated = false;
            //Global.DecimalPlaces = 0;
            try
            {
                string l_Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "CsvFile.csv");
                //string l_Path = System.IO.Path.Combine("C:\\bike\\", "CsvFile.csv");

                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(l_Path,false, System.Text.Encoding.Default))
                {
                    SourceGrid.Exporter.CSV csv = new SourceGrid.Exporter.CSV();
                    csv.Export(grdPaySlips, writer);
                    writer.Close();
                }

                //DevAge.Shell.Utilities.OpenFile(l_Path);
                //File.Open(l_Path,FileMode.Open);
                System.Diagnostics.Process.Start(@l_Path);
                MessageBox.Show("Use 'Save as' to save the document to new location.");
            }
            catch (Exception err)
            {
                DevAge.Windows.Forms.ErrorDialog.Show(this, err, "CSV Export Error");
            }

            //Global.Comma_Separated = prevCommaSeperator;
            //Global.DecimalPlaces = prevDecimalPlaces;
        }

        private void btnJournalEntry_Click(object sender, EventArgs e)
        {
            try
            {
                bool chkUserPermission = UserPermission.ChkUserPermission("HRM_SALARY_JOURNAL");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to enter journal for salary sheet. Please contact your administrator for permission.");
                    return;
                }
                if (isJournalEdit)
                {
                    if (_paySlipID != 0)
                    {
                        if (cmbBankName.SelectedIndex == -1)
                        {
                            Global.Msg("Please select a bank or a cash account for journal voucher");
                            return;
                        }
                        if (DialogResult.Yes == Global.MsgQuest("Please check the salary sheet properly before proceeding. You will not be able to edit this form after the voucher posting has been completed.\nDo you want to continue without checking again?"))
                        {
                            #region previous method and form for salary voucher entry
                            //HRM.View.frmSalaryJournalEntry sje = new HRM.View.frmSalaryJournalEntry(_paySlipID, isJournalEdit);
                            //sje.ShowDialog();
                            //if (sje.DialogResult == DialogResult.Yes)
                            //    LoadSavedGrid();
                            //else
                            //    LoadSavedGrid();
                            #endregion

                            decimal totalSalary = 0;
                            decimal totalAllowance = 0;
                            decimal tFestivalAllowance = 0;
                            decimal tPF = 0, tPension = 0, tPFDeduct = 0, tPensionDeduct = 0, tTaxDeduct = 0, tLoan = 0, tNLKosh = 0, tAdvance = 0, tNetSalary = 0, tMiscDeduct = 0;

                            DataTable dtFound = employees.GetSalaryMaster(_paySlipID);
                            DataRow drFound = dtFound.Rows[0];
                            decimal tSalary = Convert.ToDecimal(Convert.ToDecimal(drFound["tSalary"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tGrade = Convert.ToDecimal(Convert.ToDecimal(drFound["tGrade"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tPF = Convert.ToDecimal(Convert.ToDecimal(drFound["tPF"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tPension = Convert.ToDecimal(Convert.ToDecimal(drFound["tPension"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tInflationAlw = Convert.ToDecimal(Convert.ToDecimal(drFound["tInflationAlw"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tAdmAlw = Convert.ToDecimal(Convert.ToDecimal(drFound["tAdmAlw"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tAcademicAlw = Convert.ToDecimal(Convert.ToDecimal(drFound["tAcademicAlw"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tPostAlw = Convert.ToDecimal(Convert.ToDecimal(drFound["tPostAlw"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tFestivalAllowance = Convert.ToDecimal(Convert.ToDecimal(drFound["tFestivalAlw"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tMiscAllow = Convert.ToDecimal(Convert.ToDecimal(drFound["tMiscAllow"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                           
                            object TOverTimeAllowance = drFound["tOverTimeAlw"] ;
                            decimal tOverTimeAlw = Convert.ToDecimal(TOverTimeAllowance == DBNull.Value ? "0.00" :Convert.ToDecimal(TOverTimeAllowance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                            decimal tGrossAmount = Convert.ToDecimal(drFound["tGrossAmount"].ToString());
                            
                            tPFDeduct = Convert.ToDecimal(Convert.ToDecimal(drFound["tPFDeduct"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tPensionDeduct = Convert.ToDecimal(Convert.ToDecimal(drFound["tPensionDeduct"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tTaxDeduct = Convert.ToDecimal(Convert.ToDecimal(drFound["tTaxDeduct"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tKK = Convert.ToDecimal(Convert.ToDecimal(drFound["tKK"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tNLKosh = Convert.ToDecimal(Convert.ToDecimal(drFound["tNLKosh"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tAccommodation = Convert.ToDecimal(Convert.ToDecimal(drFound["tAccommodation"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tElectricity = Convert.ToDecimal(Convert.ToDecimal(drFound["tElectricity"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tLoan = Convert.ToDecimal(Convert.ToDecimal(drFound["tLoan"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tAdvance = Convert.ToDecimal(Convert.ToDecimal(drFound["tAdvance"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tMiscDeduct = Convert.ToDecimal(Convert.ToDecimal(drFound["tMiscDeduct"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            decimal tTotalDeduct = Convert.ToDecimal(Convert.ToDecimal(drFound["tTotalDeduct"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            tNetSalary = Convert.ToDecimal(Convert.ToDecimal(drFound["tNetSalary"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            
                            DataTable dtJournalDetails = new DataTable();
                            dtJournalDetails.Columns.Add("LedgerID");
                            dtJournalDetails.Columns.Add("LedgerName");
                            dtJournalDetails.Columns.Add("DrCr");
                            dtJournalDetails.Columns.Add("Amount");

                            string ledgerName = "";

                            if (tSalary != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.SalaryAcID);
                                totalSalary = tSalary + tGrade;
                                dtJournalDetails.Rows.Add(Global.SalaryAcID, ledgerName, "Debit", totalSalary);
                                
                            }

                            if (tPF != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.PFContributionID);

                                dtJournalDetails.Rows.Add(Global.PFContributionID, ledgerName, "Debit", tPF);
                                
                            }

                            if (tPension != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.PensionFContributionID);

                                dtJournalDetails.Rows.Add(Global.PensionFContributionID, ledgerName, "Debit", tPension);

                            }

                            //if (tInflationAlw != 0 || tAdmAlw != 0 || tAcademicAlw != 0 || tPostAlw != 0 || tMiscAllow != 0)
                            //{
                            //    ledgerName = Ledger.GetLedgerNameFromID(Global.BasicAllowanceID);
                            //    totalAllowance = tInflationAlw + tAdmAlw + tAcademicAlw + tPostAlw + tMiscAllow;

                            //    dtJournalDetails.Rows.Add(Global.BasicAllowanceID, ledgerName, "Debit", totalAllowance);

                            //}

                            if (tPostAlw != 0 || tMiscAllow != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.BasicAllowanceID);
                                totalAllowance = tPostAlw + tMiscAllow + tOverTimeAlw;

                                dtJournalDetails.Rows.Add(Global.BasicAllowanceID, ledgerName, "Debit", totalAllowance);

                            }

                            if(tFestivalAllowance != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.FestivalAllowanceID);
                                dtJournalDetails.Rows.Add(Global.FestivalAllowanceID, ledgerName, "Debit", tFestivalAllowance);
                            }

                            if (tPFDeduct != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.PFPayableID);

                                dtJournalDetails.Rows.Add(Global.PFPayableID, ledgerName, "Credit", tPFDeduct);

                            }

                            if (tPensionDeduct != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.PensionFPayableID);

                                dtJournalDetails.Rows.Add(Global.PensionFPayableID, ledgerName, "Credit", tPensionDeduct);

                            }

                            if (tTaxDeduct != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.TDSonSalaryID);

                                dtJournalDetails.Rows.Add(Global.TDSonSalaryID, ledgerName, "Credit", tTaxDeduct);

                            }

                            if (tKK != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.KalyankariFundID);

                                dtJournalDetails.Rows.Add(Global.KalyankariFundID, ledgerName, "Credit", tKK);

                            }

                            if (tNLKosh != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.NagarikLaganiFundID);

                                dtJournalDetails.Rows.Add(Global.NagarikLaganiFundID, ledgerName, "Credit", tNLKosh);

                            }

                            if (tAccommodation != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.QuarterAccommodationID);

                                dtJournalDetails.Rows.Add(Global.QuarterAccommodationID, ledgerName, "Credit", tAccommodation);

                            }

                            if (tElectricity != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.QuarterElectricityID);

                                dtJournalDetails.Rows.Add(Global.QuarterElectricityID, ledgerName, "Credit", tElectricity);

                            }

                            if (tLoan != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.StaffLoanAcID);

                                dtJournalDetails.Rows.Add(Global.StaffLoanAcID, ledgerName, "Credit", tLoan);

                            }

                            if (tAdvance != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.StaffAdvanceID);

                                dtJournalDetails.Rows.Add(Global.StaffAdvanceID, ledgerName, "Credit", tAdvance);

                            }

                            if (tMiscDeduct != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(Global.MiscDeductionID);

                                dtJournalDetails.Rows.Add(Global.MiscDeductionID, ledgerName, "Credit", tMiscDeduct);
                            }
                            //Selects ledger id for cash account if 'By Cash' is selected or for bank if 'By Cash' is not selected
                            ListItem liLedgerID = new ListItem();
                            liLedgerID = (ListItem)cmbBankName.SelectedItem;
                            int bankId = Convert.ToInt32(liLedgerID.ID);

                            if (tNetSalary != 0)
                            {
                                ledgerName = Ledger.GetLedgerNameFromID(bankId);

                                dtJournalDetails.Rows.Add(bankId, ledgerName, "Credit", tNetSalary);

                            }
                            object[] param = new object[2];
                            param[0] = dtJournalDetails;
                            param[1] = this;
                            m_MDIForm.OpenFormArrayParam("frmJournalNewSalary", param);
                            //frmJournal jrnl = new frmJournal()
                        }
                    }
                    else
                    {
                        Global.Msg("You need to save the salary sheet first");
                    }
                }
                else
                {
                    int journalId = 0;
                    DataTable dtFound = employees.GetSalaryMaster(_paySlipID);

                    if (dtFound.Rows.Count > 0 && dtFound.Rows[0]["JournalID"] != DBNull.Value)
                    {
                        journalId = Convert.ToInt32(dtFound.Rows[0]["JournalID"].ToString());
                        object[] param = new object[2];
                        param[0] = journalId;
                        param[1] = this;
                        m_MDIForm.OpenFormArrayParam("frmJournalEditSalary", param);

                    }
                    else
                    {
                        Global.Msg("Journal voucher not found.");
                        return;
                    }
                    //HRM.View.frmSalaryJournalEntry sje = new HRM.View.frmSalaryJournalEntry(_paySlipID, isJournalEdit);
                    //sje.ShowDialog();
                    //if (sje.DialogResult == DialogResult.Yes)
                    //    LoadSavedGrid();
                    //else
                    //    LoadSavedGrid();
                }
            }
            catch(Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void chkbycash_CheckedChanged(object sender, EventArgs e)
        {
            if(chkByCash.Checked)
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

        private bool isChkFestiveChanged = false;
        private void chkFestiveMonth_CheckedChanged(object sender, EventArgs e)
        {
            isChkFestiveChanged = true;
            LoadSavedGrid();
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private bool isInsurenceApplied = true;
        private void chkApplyInsurance_CheckedChanged(object sender, EventArgs e)
        {
            isInsurenceApplied = true;
            LoadSavedGrid();
        }
        DataTable dtSalaryAdjust = null;

        public DataTable InitSalaryAdjsutTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EmployeeID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));

            return dt;
        }
        public void GetAdjustment(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    DataRow[] dataRows = dtSalaryAdjust.Select("[EmployeeID] = " + CurEmployeeID);
                    
                    foreach (DataRow dataRow in dataRows)
                    {
                        dtSalaryAdjust.Rows.Remove(dataRow);
                    }

                    dtSalaryAdjust.Merge(dt);
                    cmbMonth_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        int CurEmployeeID = 0;
        private void btnAdjust_Click(object sender, EventArgs e)
        {
            try
            {
                int curRow = grdPaySlips.Selection.GetSelectionRegion().GetRowsIndex()[0];
                int EmpID = Convert.ToInt32(grdPaySlips[curRow, (int)GridColumns.EmployeeID2].Value);
                string designation = grdPaySlips[curRow, (int)GridColumns.Designation5].Value.ToString();
                string staffName = grdPaySlips[curRow, (int)GridColumns.Name4].Value.ToString();
                string staffCode = grdPaySlips[curRow, (int)GridColumns.Code3].Value.ToString();
                string presence = grdPaySlips[curRow, (int)GridColumns.Presence32].Value.ToString();
                if (presence == "False")
                {
                    Global.MsgError("Employee Presence for this employee must be true to access salary adjustment !");
                    return; 
                }

                DataRow[] drs = dtSalaryAdjust.Select("[EmployeeID] = " + EmpID);
                CurEmployeeID = EmpID;
                DataTable dt = InitSalaryAdjsutTable();


                if (drs.Count() > 0)
                {
                    dt = drs.CopyToDataTable();
                }

                frmSalaryAdjustment frmSa = new frmSalaryAdjustment(dt,EmpID, staffName, designation, staffCode, this);
                frmSa.ShowDialog();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        bool isPresenceChanged = false;

        private void PresenceChanged_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!isPresenceChanged)
                {
                    SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
                    CurrRowPos = ctx.Position.Row;

                    string EmployeeID = grdPaySlips[CurrRowPos, (int)GridColumns.EmployeeID2].Value.ToString();
                    DataRow[] dataRows = dtSalaryAdjust.Select("[EmployeeID] = " + EmployeeID);
                    if (dataRows.Count() > 0)
                    {
                        if (Global.MsgQuest("If you change the presence of employee, adjusted salary info will be deleted ! \n Are you sure you want to keep the changes ?") == System.Windows.Forms.DialogResult.No)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = drFound.CopyToDataTable();
                int EmployeeID = Convert.ToInt32(cmbEmployeeList.SelectedValue);
                DataRow dr = dt.Select("ID = " + EmployeeID)[0];
                int index = dt.Rows.IndexOf(dr);

                grdPaySlips.Selection.FocusRow(index + 3);

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message); 
            }
        }

        private void chkJournalPosting_CheckedChanged(object sender, EventArgs e)
        {
            cmbBankName.Enabled = chkByCash.Enabled = btnJournalEntry.Enabled = chkJournalPosting.Checked;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

 
    }
}
