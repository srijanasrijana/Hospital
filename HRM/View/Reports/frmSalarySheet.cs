using BusinessLogic;
using Common;
using CrystalDecisions.Shared;
using DateManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HRM.View.Reports
{
    public partial class frmSalarySheet : Form
    {
        int m_paySlipID = 0;
        //int[] m_paySlipIDs = null;
        string m_faculty = "";
        BusinessLogic.HRM.Employee emp = new BusinessLogic.HRM.Employee();
        private Model.dsSalarySheet dsSalarySheet = new Model.dsSalarySheet();

        private string FilterString = "";
        private DataRow[] drFound;
        private DataTable dTable;
        private int prntDirect = 0;
        private string FileName = "";
        ContextMenu Menu_Export;
        //private bool m_isRemaining = false;
        //private string m_Date = "";

       public enum GridColumns {SN1 = 1, EmployeeID2, Code3, Name4, Designation5, Level6, Basic_Salary7, Grade8, Grade_Amt9, PF_Add10,
           Pension_Add11, Allow_Dearness12, Allow_Administrative13, Allow_Academic14, Allow_General15, Allow_Festival16, Allow_Misc17, Allow_OverTime177,
           Gross_Amt18, PF_Deduct19, PensionF_Deduct20, Employee_Welfare21, CIT22, QC_Accomodation23, QC_Electricity24, Loan_Deduct25, 
           Advance_Deduct26, Misc_Deduct27,OnePercentTax, RemTax, Tax_Deduct28, Total_Deduct29, Net_Salary30, 
           Presence31 }
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        public frmSalarySheet()
        {
            InitializeComponent();
        }

        public frmSalarySheet(int paySlipId,string faculty)
        {
            InitializeComponent();
            m_paySlipID = paySlipId;
            m_faculty = faculty;
        }
        //public frmSalarySheet(int[] paySlipId, string faculty, bool isRemaining, string date, DateTime fromDate, DateTime toDate)
        //{
        //    InitializeComponent();
        //    m_paySlipIDs = paySlipId;
        //    m_faculty = faculty;
        //    m_isRemaining = isRemaining;
        //    m_Date = date;
        //}

        public frmSalarySheet(BusinessLogic.HRM.Report.EmployeeReportSettings settings)
        {
            // TODO: Complete member initialization
            this.m_settings = settings;
            InitializeComponent();

        }

        private void WriteBanner()
        {
            DataTable dt = emp.GetSalaryMaster(m_settings.paySlipIds[0]);
            lblReportTitle.Text = "Salary Sheet" + (m_settings.isRemaining ? "(Remaining)" : "");
            if (dt.Rows.Count > 0)
            {
                lblMonth.Text += dt.Rows[0]["year"].ToString() + " " + dt.Rows[0]["monthName"].ToString();// +(m_isRemaining ? "(Remaining)" : "");
                lblDate.Text += DateManager.Date.ToSystem(DateManager.Date.GetServerDate()).ToString();
                lblFaculty.Text += m_settings.faculty;
            }

            if (m_settings.paySlipDate != null)
            {
                lblPaySlipDate.Text = "Pay Slip Date : " + Date.DBToSystem(m_settings.paySlipDate.ToString());
            }
            if (m_settings.fromDate != null && m_settings.toDate != null)
            {
                lblFromDate.Text = "From: " + Date.DBToSystem(m_settings.fromDate.ToString());
            }
           
        }

        private void frmSalarySheet_Load(object sender, EventArgs e)
        {
            WriteBanner();
            LoadGrid(false);
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
            grdPaySlip[0, 0] = new MyHeader("");
            grdPaySlip[0, 0].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.SN1] = new MyHeader("SN");
            grdPaySlip[0, (int)GridColumns.SN1].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.EmployeeID2] = new MyHeader("Employee ID");
            grdPaySlip[0, (int)GridColumns.EmployeeID2].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Code3] = new MyHeader("Code");
            grdPaySlip[0, (int)GridColumns.Code3].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Name4] = new MyHeader("Name");
            grdPaySlip[0, (int)GridColumns.Name4].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Designation5] = new MyHeader("Designation");
            grdPaySlip[0, (int)GridColumns.Designation5].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Level6] = new MyHeader("Level");
            grdPaySlip[0, (int)GridColumns.Level6].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Basic_Salary7] = new MyHeader("Basic Salary");
            grdPaySlip[0, (int)GridColumns.Basic_Salary7].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Grade8] = new MyHeader("Grade");
            grdPaySlip[0, (int)GridColumns.Grade8].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Grade_Amt9] = new MyHeader("Grade Amount");
            grdPaySlip[0, (int)GridColumns.Grade_Amt9].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.PF_Add10] = new MyHeader("PF(Add)");
            grdPaySlip[0, (int)GridColumns.PF_Add10].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Pension_Add11] = new MyHeader("Pension F(Add)");
            grdPaySlip[0, (int)GridColumns.Pension_Add11].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Allow_Dearness12] = new MyHeader("Allowance");
            grdPaySlip[0, (int)GridColumns.Allow_Dearness12].ColumnSpan = 7;
            grdPaySlip[1, (int)GridColumns.Allow_Dearness12] = new MyHeader("Dearness");
            grdPaySlip[1, (int)GridColumns.Allow_Administrative13] = new MyHeader("Administrative");
            grdPaySlip[1, (int)GridColumns.Allow_Academic14] = new MyHeader("Academic");
            //grdPaySlip[1, 15] = new MyHeader("Post");
            grdPaySlip[1, (int)GridColumns.Allow_General15] = new MyHeader("General");
            grdPaySlip[1, (int)GridColumns.Allow_Festival16] = new MyHeader("Festival");
            grdPaySlip[1, (int)GridColumns.Allow_Misc17] = new MyHeader("Miscellaneous");
            grdPaySlip[1, (int)GridColumns.Allow_Misc17].Column.Visible = false;
            grdPaySlip[1, (int)GridColumns.Allow_OverTime177] = new MyHeader("Over Time");

            grdPaySlip[0, (int)GridColumns.Gross_Amt18] = new MyHeader("Gross Amount");
            grdPaySlip[0, (int)GridColumns.Gross_Amt18].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.PF_Deduct19] = new MyHeader("PF (Deduct)");
            grdPaySlip[0, (int)GridColumns.PF_Deduct19].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.PensionF_Deduct20] = new MyHeader("Pension F(Deduct)");
            grdPaySlip[0, (int)GridColumns.PensionF_Deduct20].RowSpan = 2;

            grdPaySlip[0, (int)GridColumns.Employee_Welfare21] = new MyHeader("Employee Welfare");
            grdPaySlip[0, (int)GridColumns.Employee_Welfare21].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.CIT22] = new MyHeader("CIT");
            grdPaySlip[0, (int)GridColumns.CIT22].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.QC_Accomodation23] = new MyHeader("Quarter Charge");
            grdPaySlip[0, (int)GridColumns.QC_Accomodation23].ColumnSpan = 2;
            grdPaySlip[1, (int)GridColumns.QC_Accomodation23] = new MyHeader("Accommodation");
            grdPaySlip[1, (int)GridColumns.QC_Electricity24] = new MyHeader("Electricity");
            grdPaySlip[0, (int)GridColumns.Loan_Deduct25] = new MyHeader("Loan Deduction");
            grdPaySlip[0, (int)GridColumns.Loan_Deduct25].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Advance_Deduct26] = new MyHeader("Advance Deduction");
            grdPaySlip[0, (int)GridColumns.Advance_Deduct26].RowSpan = 2;

            grdPaySlip[0, (int)GridColumns.Misc_Deduct27] = new MyHeader("Misc Deduction");
            grdPaySlip[0, (int)GridColumns.Misc_Deduct27].Column.Visible = false;
 
            grdPaySlip[0, (int)GridColumns.Misc_Deduct27].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.OnePercentTax] = new MyHeader("Tax Deduction");
            grdPaySlip[0, (int)GridColumns.OnePercentTax].ColumnSpan = 3;

            grdPaySlip[1, (int)GridColumns.OnePercentTax] = new MyHeader("1% Tax");
            grdPaySlip[1, (int)GridColumns.RemTax] = new MyHeader("Remaining");

            grdPaySlip[1, (int)GridColumns.Tax_Deduct28] = new MyHeader("Total");
            //grdPaySlip[1, (int)GridColumns.OnePercentTax].Column.Visible = grdPaySlip[1, (int)GridColumns.RemTax].Column.Visible = false;

            grdPaySlip[0, (int)GridColumns.Total_Deduct29] = new MyHeader("Total Deduction");
            grdPaySlip[0, (int)GridColumns.Total_Deduct29].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Net_Salary30] = new MyHeader("Net Salary");
            grdPaySlip[0, (int)GridColumns.Net_Salary30].RowSpan = 2;
            grdPaySlip[0, (int)GridColumns.Presence31] = new MyHeader("Presence");
            grdPaySlip[0, (int)GridColumns.Presence31].RowSpan = 2;


            grdPaySlip[0, 0].Column.Width = 30;
            grdPaySlip[0, (int)GridColumns.SN1].Column.Width = 30;
            grdPaySlip[0, (int)GridColumns.EmployeeID2].Column.Width = 1;
            grdPaySlip[0, (int)GridColumns.Code3].Column.Width = 40;
            grdPaySlip[0, (int)GridColumns.Name4].Column.Width = 120;
            grdPaySlip[0, (int)GridColumns.Designation5].Column.Width = 100;
            grdPaySlip[0, (int)GridColumns.Level6].Column.Width = 100;
            grdPaySlip[0, (int)GridColumns.Basic_Salary7].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.Grade8].Column.Width = 30;
            grdPaySlip[0, (int)GridColumns.Grade_Amt9].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.PF_Add10].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.Pension_Add11].Column.Width = 80;
            grdPaySlip[1, (int)GridColumns.Allow_Dearness12].Column.Width = 80;
            grdPaySlip[1, (int)GridColumns.Allow_Administrative13].Column.Width = 80;
            grdPaySlip[1, (int)GridColumns.Allow_Academic14].Column.Width = 80;
            grdPaySlip[1, (int)GridColumns.Allow_General15].Column.Width = 80;
            grdPaySlip[1, (int)GridColumns.Allow_Festival16].Column.Width = 80;
            grdPaySlip[1, (int)GridColumns.Allow_Misc17].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.Gross_Amt18].Column.Width = 100;
            grdPaySlip[0, (int)GridColumns.PF_Deduct19].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.PensionF_Deduct20].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.Employee_Welfare21].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.CIT22].Column.Width = 80;
            grdPaySlip[1, (int)GridColumns.QC_Accomodation23].Column.Width = 80;
            grdPaySlip[1, (int)GridColumns.QC_Electricity24].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.Loan_Deduct25].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.Advance_Deduct26].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.Misc_Deduct27].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.Tax_Deduct28].Column.Width = 80;
            grdPaySlip[0, (int)GridColumns.Total_Deduct29].Column.Width = 100;
            grdPaySlip[0, (int)GridColumns.Net_Salary30].Column.Width = 100;
            grdPaySlip[0, (int)GridColumns.Presence31].Column.Width = 70;

            grdPaySlip[1, (int)GridColumns.OnePercentTax].Column.Width = 70;
            grdPaySlip[1, (int)GridColumns.RemTax].Column.Width = 70;


            grdPaySlip[0, (int)GridColumns.EmployeeID2].Column.Visible = false;//Employee ID
            grdPaySlip[0, 0].Column.Visible = false;//checkbox hidden for bishal bazar

            grdPaySlip[1, (int)GridColumns.Allow_Dearness12].Column.Visible = false;//column 12,13,14 are for allowance and all these allowances are added to single column 15,so these are hidden
            grdPaySlip[1, (int)GridColumns.Allow_Administrative13].Column.Visible = false;
            grdPaySlip[1, (int)GridColumns.Allow_Academic14].Column.Visible = false;
        }
        private void LoadGrid(bool isCrystalRpt)
        {
            try
            {
                dTable = emp.savedSalaryDetailsPresent(m_settings);//(m_paySlipIDs, m_isRemaining,m_Date);
                drFound = dTable.Select(FilterString);
                if (!isCrystalRpt)
                {
                    grdPaySlip.Rows.Clear();
                    grdPaySlip.Redim(drFound.Count() + 3, 36);
                    grdPaySlip.FixedRows = 2;
                    grdPaySlip.FixedColumns = 5;
                    writeHeader();
                    
                }
                SourceGrid.Cells.Views.Cell AlternateColor = new SourceGrid.Cells.Views.Cell();
                for (int i = 2; i <= drFound.Count() + 1; i++)
                {
                    DataRow dr = drFound[i - 2];
                    string StaffCode = dr["StaffCode"].ToString();
                    string StaffName = dr["StaffName"].ToString();
                    string DesignationName = dr["DesignationName"].ToString();
                    string EmpLevel = dr["EmpLevel"].ToString();
                    decimal basicSalary = 0;
                    if (!dr.IsNull("BasicSalary"))
                        basicSalary = Convert.ToDecimal(dr["BasicSalary"]);
                    int grade = 0;
                    if (!dr.IsNull("Grade"))
                        grade = Convert.ToInt32(dr["Grade"].ToString());
                    decimal gradeAmount = 0;
                    gradeAmount = Convert.ToDecimal(dr["GradeAmount"].ToString());
                    decimal pfAmount = 0;
                    pfAmount = Convert.ToDecimal(dr["PFAmount"].ToString());
                    decimal PensionFAmt = 0;
                    PensionFAmt = Convert.ToDecimal(dr["PensionFAmt"].ToString());
                    decimal InflationAlw = 0;
                    if (!dr.IsNull("InflationAlw"))
                        InflationAlw = Convert.ToDecimal(dr["InflationAlw"].ToString());
                    decimal AdmistrativeAlw = 0;
                    if (!dr.IsNull("AdmistrativeAlw"))
                        AdmistrativeAlw = Convert.ToDecimal(dr["AdmistrativeAlw"].ToString());
                    decimal AcademicAlw = 0;
                    if (!dr.IsNull("AcademicAlw"))
                        AcademicAlw = Convert.ToDecimal(dr["AcademicAlw"].ToString());
                    decimal PostAlw = 0;
                    if (!dr.IsNull("PostAlw"))
                        PostAlw = Convert.ToDecimal(dr["PostAlw"].ToString());
                    decimal festivalAlw = 0;
                    if (!dr.IsNull("FestivalAlw"))
                        festivalAlw = Convert.ToDecimal(dr["FestivalAlw"].ToString());
                    decimal miscAllowance =0;
                    miscAllowance =  Convert.ToDecimal(dr["miscAllowance"]);

                    decimal OverTime = 0;
                    object overTime = dr["OverTimeAlw"];
                    OverTime = Convert.ToDecimal(overTime == DBNull.Value ? 0: overTime);

                    decimal grossAmount = 0;
                    grossAmount = Convert.ToDecimal(dr["grossAmount"].ToString());
                    decimal pfDedection = 0;
                    pfDedection = Convert.ToDecimal(dr["pfDeduct"].ToString());
                    decimal PensionFDeduct = 0;
                    PensionFDeduct = Convert.ToDecimal(dr["PensionFDeduct"].ToString());
                    decimal taxDeduct = 0;
                    taxDeduct =  Convert.ToDecimal(dr["taxDeduct"].ToString());
                    decimal KalyankariAmt = 0;
                    KalyankariAmt = Convert.ToDecimal(dr["KalyankariAmt"].ToString());
                    decimal NLKos = 0;
                    if (!dr.IsNull("NLKoshDeduct"))
                        NLKos = Convert.ToDecimal(dr["NLKoshDeduct"].ToString());
                    decimal Accommodation = 0;
                    Accommodation = Convert.ToDecimal(dr["Accommodation"].ToString());
                    decimal ElectricityDeduct = 0;
                    ElectricityDeduct = Convert.ToDecimal(dr["ElectricityDeduct"].ToString());

                    decimal LoanMthPremium = 0;
                    if (!dr.IsNull("LoanMthPremium"))
                        LoanMthPremium = Convert.ToDecimal(dr["LoanMthPremium"].ToString());
                    decimal AdvMthInstallment = 0;
                    if (!dr.IsNull("AdvMthInstallment"))
                        AdvMthInstallment = Convert.ToDecimal(dr["AdvMthInstallment"].ToString());
                    decimal MiscDeduct = 0;
                    MiscDeduct = Convert.ToDecimal(dr["MiscDeduct"].ToString());
                    decimal totalDeduct = 0;
                    totalDeduct = Convert.ToDecimal(dr["TotalDeduct"].ToString());
                    decimal netSalary = 0;
                    netSalary = Convert.ToDecimal(dr["netSalary"].ToString());
                    string empPresence = "";
                    empPresence = dr["EmpPresence"].ToString();

                    object OnePerTax = dr["OnePercentTax"];
                    decimal onePercentTax = Convert.ToDecimal(OnePerTax == DBNull.Value ? (CalculateOnePercentTax(Convert.ToInt32(dr["EmployeeID"]), taxDeduct)) : OnePerTax);
                    decimal remTax = taxDeduct - onePercentTax;

                    if (!isCrystalRpt)
                    {
                        if (i % 2 == 0)
                            AlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                        else
                            AlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);

                        grdPaySlip[i, 0] = new SourceGrid.Cells.Cell("");

                        grdPaySlip[i, (int)GridColumns.SN1] = new SourceGrid.Cells.Cell((i - 1).ToString());
                        grdPaySlip[i, (int)GridColumns.SN1].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.EmployeeID2] = new SourceGrid.Cells.Cell(dr["EmployeeID"].ToString());


                        grdPaySlip[i, (int)GridColumns.Code3] = new SourceGrid.Cells.Cell(StaffCode);
                        grdPaySlip[i, (int)GridColumns.Code3].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Name4] = new SourceGrid.Cells.Cell(StaffName);
                        grdPaySlip[i, (int)GridColumns.Name4].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Designation5] = new SourceGrid.Cells.Cell(DesignationName);
                        grdPaySlip[i, (int)GridColumns.Designation5].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Level6] = new SourceGrid.Cells.Cell(EmpLevel);
                        grdPaySlip[i, (int)GridColumns.Level6].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Basic_Salary7] = new SourceGrid.Cells.Cell(basicSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Basic_Salary7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Basic_Salary7].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Grade8] = new SourceGrid.Cells.Cell(grade.ToString());
                        grdPaySlip[i, (int)GridColumns.Grade8].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Grade_Amt9] = new SourceGrid.Cells.Cell(gradeAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Grade_Amt9].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Grade_Amt9].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.PF_Add10] = new SourceGrid.Cells.Cell(pfAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.PF_Add10].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.PF_Add10].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Pension_Add11] = new SourceGrid.Cells.Cell(PensionFAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Pension_Add11].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Pension_Add11].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Allow_Dearness12] = new SourceGrid.Cells.Cell(InflationAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Allow_Dearness12].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Allow_Dearness12].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Allow_Administrative13] = new SourceGrid.Cells.Cell(AdmistrativeAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Allow_Administrative13].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Allow_Administrative13].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Allow_Academic14] = new SourceGrid.Cells.Cell(AcademicAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Allow_Academic14].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Allow_Academic14].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Allow_General15] = new SourceGrid.Cells.Cell(PostAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Allow_General15].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Allow_General15].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Allow_Festival16] = new SourceGrid.Cells.Cell(festivalAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Allow_Festival16].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Allow_Festival16].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Allow_Misc17] = new SourceGrid.Cells.Cell(miscAllowance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Allow_Misc17].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Allow_Misc17].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Allow_OverTime177] = new SourceGrid.Cells.Cell(OverTime.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Allow_OverTime177].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Allow_OverTime177].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Gross_Amt18] = new SourceGrid.Cells.Cell(grossAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Gross_Amt18].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Gross_Amt18].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.PF_Deduct19] = new SourceGrid.Cells.Cell(pfDedection.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.PF_Deduct19].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.PF_Deduct19].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.PensionF_Deduct20] = new SourceGrid.Cells.Cell(PensionFDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.PensionF_Deduct20].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.PensionF_Deduct20].View = new SourceGrid.Cells.Views.Cell(AlternateColor);


                        grdPaySlip[i, (int)GridColumns.Employee_Welfare21] = new SourceGrid.Cells.Cell(KalyankariAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Employee_Welfare21].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Employee_Welfare21].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.CIT22] = new SourceGrid.Cells.Cell(NLKos.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.CIT22].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.CIT22].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.QC_Accomodation23] = new SourceGrid.Cells.Cell(Accommodation.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.QC_Accomodation23].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.QC_Accomodation23].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.QC_Electricity24] = new SourceGrid.Cells.Cell(ElectricityDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.QC_Electricity24].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.QC_Electricity24].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Loan_Deduct25] = new SourceGrid.Cells.Cell(LoanMthPremium.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Loan_Deduct25].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Loan_Deduct25].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Advance_Deduct26] = new SourceGrid.Cells.Cell(AdvMthInstallment.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Advance_Deduct26].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Advance_Deduct26].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Misc_Deduct27] = new SourceGrid.Cells.Cell(MiscDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Misc_Deduct27].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Misc_Deduct27].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.OnePercentTax] = new SourceGrid.Cells.Cell(onePercentTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.OnePercentTax].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.OnePercentTax].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.RemTax] = new SourceGrid.Cells.Cell(remTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.RemTax].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.RemTax].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Tax_Deduct28] = new SourceGrid.Cells.Cell(taxDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Tax_Deduct28].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Tax_Deduct28].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Total_Deduct29] = new SourceGrid.Cells.Cell(totalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Total_Deduct29].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Total_Deduct29].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Net_Salary30] = new SourceGrid.Cells.Cell(netSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdPaySlip[i, (int)GridColumns.Net_Salary30].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        grdPaySlip[i, (int)GridColumns.Net_Salary30].View = new SourceGrid.Cells.Views.Cell(AlternateColor);

                        grdPaySlip[i, (int)GridColumns.Presence31] = new SourceGrid.Cells.Cell(empPresence);
                        grdPaySlip[i, (int)GridColumns.Presence31].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                        grdPaySlip[i, (int)GridColumns.Presence31].View = new SourceGrid.Cells.Views.Cell(AlternateColor);
                    }
                    else
                    {
                        dsSalarySheet.Tables["tblSalarySheet"].Rows.Add(StaffCode, StaffName, DesignationName, EmpLevel, basicSalary, grade, gradeAmount, pfAmount, miscAllowance, grossAmount, pfDedection, taxDeduct, NLKos, MiscDeduct, totalDeduct, netSalary, PensionFAmt, InflationAlw, AdmistrativeAlw, AcademicAlw, PostAlw, PensionFDeduct, KalyankariAmt, Accommodation, ElectricityDeduct, LoanMthPremium, AdvMthInstallment, festivalAlw, empPresence, onePercentTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)), remTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)), OverTime.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                    }

                }
                CalculateTotal(isCrystalRpt);
            }
            catch(Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        //variables to calculate total of all rows and to provide parameter to crystal report.
        decimal TgrossAmount = 0, TmiscAllowance = 0, TbasicSalary = 0, TgradeAmount = 0, TpfAmount = 0, Tpension = 0, TinflationAllowance = 0, TAdmAllowance = 0, Tacademic = 0, Tpost = 0,Tfestival =0, TnetSalary = 0;
        decimal TtotalDeduct = 0, TtaxDeduct = 0, TpensionDeduct = 0, TKKDeduct = 0, Taccommodation = 0, Telectricity = 0, Tloan = 0, TMiscDeduct = 0, TpfDeduct = 0, TNLKos = 0, Tadvance = 0, TonePercentTax = 0, TremTax = 0, TOverTimeAlw = 0;
        /// <summary>
        /// Calculates total of the grid and prints the value to the last row.
        /// </summary>
        private void CalculateTotal(bool isCrystalRpt)
        {
            TgrossAmount = TmiscAllowance = TbasicSalary = TgradeAmount = TpfAmount = Tpension = TinflationAllowance = TAdmAllowance = Tacademic = Tpost = Tfestival = TnetSalary = 0;
            TtotalDeduct = TtaxDeduct = TpensionDeduct = TKKDeduct = Taccommodation = Telectricity = Tloan = TMiscDeduct = TpfDeduct = TNLKos = Tadvance = TOverTimeAlw= TtotalDeduct = TonePercentTax = TremTax = 0;            
            int CurrRow = 0;
            for (int i = 2; i <= drFound.Count() + 1; i++)
            {
                CurrRow = i;
                
                TbasicSalary += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Basic_Salary7].Value);
                TgradeAmount += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Grade_Amt9].Value);
                TpfAmount += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.PF_Add10].Value);
                Tpension += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Pension_Add11].Value);
                TinflationAllowance += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Allow_Dearness12].Value);
                TAdmAllowance += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Allow_Administrative13].Value);
                Tacademic += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Allow_Academic14].Value);
                Tpost += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Allow_General15].Value);
                Tfestival += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Allow_Festival16].Value);
                TmiscAllowance += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Allow_Misc17].Value);

                TOverTimeAlw += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Allow_OverTime177].Value);

                TgrossAmount += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Gross_Amt18].Value);

                TpfDeduct += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.PF_Deduct19].Value);
                TpensionDeduct += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.PensionF_Deduct20].Value);

                TKKDeduct += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Employee_Welfare21].Value);
                TNLKos += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.CIT22].Value);
                Taccommodation += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.QC_Accomodation23].Value);
                Telectricity += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.QC_Electricity24].Value);
                Tloan += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Loan_Deduct25].Value);
                Tadvance += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Advance_Deduct26].Value);
                TMiscDeduct += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Misc_Deduct27].Value);
                
                TtaxDeduct += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Tax_Deduct28].Value);
                TonePercentTax += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.OnePercentTax].Value);
                TremTax += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.RemTax].Value);

                TtotalDeduct += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Total_Deduct29].Value);
                TnetSalary += Convert.ToDecimal(grdPaySlip[CurrRow, (int)GridColumns.Net_Salary30].Value);
            }
            if (!isCrystalRpt)
            {
                SourceGrid.Cells.Views.Cell tAlternateColor = new SourceGrid.Cells.Views.Cell();
                tAlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(ColorTranslator.FromHtml("#e6f7ff"));
                //write last row #86B0AE
                CurrRow = CurrRow + 1;

                grdPaySlip[CurrRow, 0] = new SourceGrid.Cells.Cell("");
                //grdPaySlip[CurrRow, 1] = new SourceGrid.Cells.Cell((CurrRow - 1).ToString());
                grdPaySlip[CurrRow, (int)GridColumns.SN1] = new SourceGrid.Cells.Cell("");
                grdPaySlip[CurrRow, (int)GridColumns.EmployeeID2] = new SourceGrid.Cells.Cell("");
                grdPaySlip[CurrRow, (int)GridColumns.Code3] = new SourceGrid.Cells.Cell("");
                grdPaySlip[CurrRow, (int)GridColumns.Name4] = new SourceGrid.Cells.Cell("Total");
                grdPaySlip[CurrRow, (int)GridColumns.Name4].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
                grdPaySlip[CurrRow, (int)GridColumns.Designation5] = new SourceGrid.Cells.Cell("");
                grdPaySlip[CurrRow, (int)GridColumns.Level6] = new SourceGrid.Cells.Cell("");
                grdPaySlip[CurrRow, (int)GridColumns.Basic_Salary7] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Basic_Salary7].Value = TbasicSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Grade8] = new SourceGrid.Cells.Cell("");
                grdPaySlip[CurrRow, (int)GridColumns.Grade_Amt9] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Grade_Amt9].Value = TgradeAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.PF_Add10] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.PF_Add10].Value = TpfAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Pension_Add11] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Pension_Add11].Value = Tpension.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Dearness12] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Dearness12].Value = TinflationAllowance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Administrative13] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Administrative13].Value = TAdmAllowance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Academic14] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Academic14].Value = Tacademic.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Allow_General15] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Allow_General15].Value = Tpost.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Festival16] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Festival16].Value = Tfestival.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Misc17] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Misc17].Value = TmiscAllowance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
               
                grdPaySlip[CurrRow, (int)GridColumns.Allow_OverTime177] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Allow_OverTime177].Value = TOverTimeAlw.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                
                grdPaySlip[CurrRow, (int)GridColumns.Gross_Amt18] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Gross_Amt18].Value = TgrossAmount.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.PF_Deduct19] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.PF_Deduct19].Value = TpfDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.PensionF_Deduct20] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.PensionF_Deduct20].Value = TpensionDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                grdPaySlip[CurrRow, (int)GridColumns.Employee_Welfare21] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Employee_Welfare21].Value = TKKDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.CIT22] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.CIT22].Value = TNLKos.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.QC_Accomodation23] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.QC_Accomodation23].Value = Taccommodation.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.QC_Electricity24] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.QC_Electricity24].Value = Telectricity.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Loan_Deduct25] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Loan_Deduct25].Value = Tloan.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Advance_Deduct26] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Advance_Deduct26].Value = Tadvance.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Misc_Deduct27] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Misc_Deduct27].Value = TMiscDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
               
                grdPaySlip[CurrRow, (int)GridColumns.OnePercentTax] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.OnePercentTax].Value = TonePercentTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.RemTax] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.RemTax].Value = TremTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Tax_Deduct28] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Tax_Deduct28].Value = TtaxDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                grdPaySlip[CurrRow, (int)GridColumns.Total_Deduct29] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Total_Deduct29].Value = TtotalDeduct.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Net_Salary30] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Net_Salary30].Value = TnetSalary.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                grdPaySlip[CurrRow, (int)GridColumns.Presence31] = new SourceGrid.Cells.Cell();
                grdPaySlip[CurrRow, (int)GridColumns.Presence31].Value = "";

                grdPaySlip[CurrRow, 0].View = 
                grdPaySlip[CurrRow, (int)GridColumns.SN1].View = 
                grdPaySlip[CurrRow, (int)GridColumns.EmployeeID2].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Code3].View =
                grdPaySlip[CurrRow, (int)GridColumns.Name4].View =
                grdPaySlip[CurrRow, (int)GridColumns.Designation5].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Level6].View =
                grdPaySlip[CurrRow, (int)GridColumns.Basic_Salary7].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Grade8].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Grade_Amt9].View = 
                grdPaySlip[CurrRow, (int)GridColumns.PF_Add10].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Pension_Add11].View =
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Dearness12].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Administrative13].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Academic14].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Allow_General15].View =
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Festival16].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Allow_Misc17].View =
                grdPaySlip[CurrRow, (int)GridColumns.Gross_Amt18].View = 
                grdPaySlip[CurrRow, (int)GridColumns.PF_Deduct19].View =
                grdPaySlip[CurrRow, (int)GridColumns.PensionF_Deduct20].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Employee_Welfare21].View = 
                grdPaySlip[CurrRow, (int)GridColumns.CIT22].View = 
                grdPaySlip[CurrRow, (int)GridColumns.QC_Accomodation23].View = 
                grdPaySlip[CurrRow, (int)GridColumns.QC_Electricity24].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Loan_Deduct25].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Advance_Deduct26].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Misc_Deduct27].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Tax_Deduct28].View = 
                grdPaySlip[CurrRow, (int)GridColumns.Total_Deduct29].View =
                grdPaySlip[CurrRow, (int)GridColumns.Net_Salary30].View =
                grdPaySlip[CurrRow, (int)GridColumns.OnePercentTax].View =
                grdPaySlip[CurrRow, (int)GridColumns.RemTax].View =
                grdPaySlip[CurrRow, (int)GridColumns.Allow_OverTime177].View =

                grdPaySlip[CurrRow, (int)GridColumns.Presence31].View = new SourceGrid.Cells.Views.Cell(tAlternateColor);
                //grdPaySlip[CurrRow, 31].View = new SourceGrid.Cells.Views.Cell(tAlternateColor);
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            prntDirect = 0;
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void PrintPreviewCR(PrintType myPrintType)
        {
            try
            {
                frmProgress ProgressForm = new frmProgress();
                // Initialize the thread that will handle the background process
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        ProgressForm.ShowDialog();
                    }
                ));

                backgroundThread.Start();

                //Update the progressbar
                ProgressForm.UpdateProgress(20, "Initializing Report Viewer...");

                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;

                dsSalarySheet.Clear();
                LoadGrid(true);
                HRM.Reports.rptSalarySheet rpt = new HRM.Reports.rptSalarySheet();

                //Fill the logo on the report
                Misc.WriteLogo(dsSalarySheet, "tblImage");

                rpt.SetDataSource(dsSalarySheet);


                //Provide values to the parameters on the report
                CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
                //CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvMonth = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvFaculty = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_PaySlipDate = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTbasicSalary = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTgradeAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTpfAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTmiscAllowance = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTgrossAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTpfDedection = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTtaxDeduct = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTNLKos = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTMiscDeduct = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTtotalDeduct = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTnetSalary = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvTpensionFAmt = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTInflation = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTAdmAmt = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTAcademic = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTPost = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTFestival = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTPensionFDeduct = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTKK = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTAccommodation = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTElectricity = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTLoan = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTAdvance = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvSalaryScale = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvOnePercentTax = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvRemTax = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvOverTimeAlw = new CrystalDecisions.Shared.ParameterDiscreteValue();

                //Update the progressbar
                ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

                pdvFont.Value = "Arial";
                pvCollection.Clear();
                pvCollection.Add(pdvFont);
                rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

                CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
                int uid = User.CurrUserID;
                DataTable dtroleinfo = User.GetUserInfo(uid);
                DataRow drrole = dtroleinfo.Rows[0];
                int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());

                if (roleid == 37)//if user is root, get information from tblCompany
                {
                    pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Name);
                    rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);


                    pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Address);
                    rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);


                    pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_PAN);
                    rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);


                    pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Phone);
                    rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);


                    //pdvCompany_Slogan.Value = m_CompanyDetails.CSlogan;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvCompany_Slogan);
                    //rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                }
                else //if user is not root, take information from tblUserPreference
                {
                    string companyname = UserPreference.GetValue("COMPANY_NAME", uid);
                    string companyaddress = UserPreference.GetValue("COMPANY_ADDRESS", uid);
                    string companycity = UserPreference.GetValue("COMPANY_CITY", uid);
                    string companypan = UserPreference.GetValue("COMPANY_PAN", uid);
                    string companyphone = UserPreference.GetValue("COMPANY_PHONE", uid);
                    string companyslogan = UserPreference.GetValue("COMPANY_SLOGAN", uid);

                    pdvCompany_Name.Value = companyname;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Name);
                    rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                    pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Address);
                    rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                    pdvCompany_PAN.Value = companypan;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_PAN);
                    rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                    pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Phone);
                    rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                    //pdvCompany_Slogan.Value = companyslogan;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvCompany_Slogan);
                    //rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

                }
                pdvReport_Head.Value = lblReportTitle.Text;
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);


                pdvMonth.Value = lblMonth.Text;
                pvCollection.Clear();
                pvCollection.Add(pdvMonth);
                rpt.DataDefinition.ParameterFields["Month"].ApplyCurrentValues(pvCollection);

                pdvFaculty.Value = lblFaculty.Text;
                pvCollection.Clear();
                pvCollection.Add(pdvFaculty);
                rpt.DataDefinition.ParameterFields["Department"].ApplyCurrentValues(pvCollection);


                // pdvReport_Date.Value = "As On Date:" + m_DayBook.ToDate.ToString("yyyy/MM/dd");
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem(DateTime.Today);
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                pdvReport_PaySlipDate.Value = lblPaySlipDate.Text;
                pvCollection.Clear();
                pvCollection.Add(pdvReport_PaySlipDate);
                rpt.DataDefinition.ParameterFields["PaySlip_Date"].ApplyCurrentValues(pvCollection);


                #region parameter for total row
                pdvTbasicSalary.Value = TbasicSalary;
                pvCollection.Clear();
                pvCollection.Add(pdvTbasicSalary);
                rpt.DataDefinition.ParameterFields["tBasicSal"].ApplyCurrentValues(pvCollection);

                pdvTgradeAmount.Value = TgradeAmount;
                pvCollection.Clear();
                pvCollection.Add(pdvTgradeAmount);
                rpt.DataDefinition.ParameterFields["tGradeAmt"].ApplyCurrentValues(pvCollection);

                pdvSalaryScale.Value = (TgradeAmount + TbasicSalary).ToString(Misc.FormatNumber(true, Global.DecimalPlaces));
                pvCollection.Clear();
                pvCollection.Add(pdvSalaryScale);
                rpt.DataDefinition.ParameterFields["tTotalSalaryScale"].ApplyCurrentValues(pvCollection);

                pdvTpfAmount.Value = TpfAmount;
                pvCollection.Clear();
                pvCollection.Add(pdvTpfAmount);
                rpt.DataDefinition.ParameterFields["tPFAdd"].ApplyCurrentValues(pvCollection);

                pdvTpensionFAmt.Value = Tpension;
                pvCollection.Clear();
                pvCollection.Add(pdvTpensionFAmt);
                rpt.DataDefinition.ParameterFields["tPensionAmt"].ApplyCurrentValues(pvCollection);

                pdvTInflation.Value = TinflationAllowance;
                pvCollection.Clear();
                pvCollection.Add(pdvTInflation);
                rpt.DataDefinition.ParameterFields["tInflation"].ApplyCurrentValues(pvCollection);

                pdvTAdmAmt.Value = TAdmAllowance;
                pvCollection.Clear();
                pvCollection.Add(pdvTAdmAmt);
                rpt.DataDefinition.ParameterFields["tAdmAmt"].ApplyCurrentValues(pvCollection);

                pdvTAcademic.Value = Tacademic;
                pvCollection.Clear();
                pvCollection.Add(pdvTAcademic);
                rpt.DataDefinition.ParameterFields["tAcademic"].ApplyCurrentValues(pvCollection);

                pdvTPost.Value = Tpost;
                pvCollection.Clear();
                pvCollection.Add(pdvTPost);
                rpt.DataDefinition.ParameterFields["tPost"].ApplyCurrentValues(pvCollection);

                pdvTFestival.Value = Tfestival;
                pvCollection.Clear();
                pvCollection.Add(pdvTFestival);
                rpt.DataDefinition.ParameterFields["tFestival"].ApplyCurrentValues(pvCollection);

                pdvTmiscAllowance.Value = TmiscAllowance;
                pvCollection.Clear();
                pvCollection.Add(pdvTmiscAllowance);
                rpt.DataDefinition.ParameterFields["tMiscAlw"].ApplyCurrentValues(pvCollection);

                pdvTgrossAmount.Value = TgrossAmount;
                pvCollection.Clear();
                pvCollection.Add(pdvTgrossAmount);
                rpt.DataDefinition.ParameterFields["tGrossAmt"].ApplyCurrentValues(pvCollection);

                pdvTpfDedection.Value = TpfDeduct;
                pvCollection.Clear();
                pvCollection.Add(pdvTpfDedection);
                rpt.DataDefinition.ParameterFields["tPFDeduct"].ApplyCurrentValues(pvCollection);

                pdvTtaxDeduct.Value = TtaxDeduct;
                pvCollection.Clear();
                pvCollection.Add(pdvTtaxDeduct);
                rpt.DataDefinition.ParameterFields["tTaxDeduct"].ApplyCurrentValues(pvCollection);

                pdvTPensionFDeduct.Value = TpensionDeduct;
                pvCollection.Clear();
                pvCollection.Add(pdvTPensionFDeduct);
                rpt.DataDefinition.ParameterFields["tPensionFDeduct"].ApplyCurrentValues(pvCollection);

                pdvTKK.Value = TKKDeduct;
                pvCollection.Clear();
                pvCollection.Add(pdvTKK);
                rpt.DataDefinition.ParameterFields["tKK"].ApplyCurrentValues(pvCollection);

                pdvTAccommodation.Value = Taccommodation;
                pvCollection.Clear();
                pvCollection.Add(pdvTAccommodation);
                rpt.DataDefinition.ParameterFields["tAccommodation"].ApplyCurrentValues(pvCollection);

                pdvTElectricity.Value = Telectricity;
                pvCollection.Clear();
                pvCollection.Add(pdvTElectricity);
                rpt.DataDefinition.ParameterFields["tElectricity"].ApplyCurrentValues(pvCollection);

                pdvTLoan.Value = Tloan;
                pvCollection.Clear();
                pvCollection.Add(pdvTLoan);
                rpt.DataDefinition.ParameterFields["tLoan"].ApplyCurrentValues(pvCollection);

                pdvTAdvance.Value = Tadvance;
                pvCollection.Clear();
                pvCollection.Add(pdvTAdvance);
                rpt.DataDefinition.ParameterFields["tAdvance"].ApplyCurrentValues(pvCollection);

                pdvTNLKos.Value = TNLKos;
                pvCollection.Clear();
                pvCollection.Add(pdvTNLKos);
                rpt.DataDefinition.ParameterFields["tNLKoshDeduct"].ApplyCurrentValues(pvCollection);

                pdvTMiscDeduct.Value = TMiscDeduct;
                pvCollection.Clear();
                pvCollection.Add(pdvTMiscDeduct);
                rpt.DataDefinition.ParameterFields["tMiscDeduct"].ApplyCurrentValues(pvCollection);

                pdvTtotalDeduct.Value = TtotalDeduct;
                pvCollection.Clear();
                pvCollection.Add(pdvTtotalDeduct);
                rpt.DataDefinition.ParameterFields["tTotalDeduct"].ApplyCurrentValues(pvCollection);

                pdvTnetSalary.Value = TnetSalary;
                pvCollection.Clear();
                pvCollection.Add(pdvTnetSalary);
                rpt.DataDefinition.ParameterFields["tNetSalary"].ApplyCurrentValues(pvCollection);

                pdvOverTimeAlw.Value = TOverTimeAlw;
                pvCollection.Clear();
                pvCollection.Add(pdvOverTimeAlw);
                rpt.DataDefinition.ParameterFields["tOverTimeAlw"].ApplyCurrentValues(pvCollection);

                pdvOnePercentTax.Value = TonePercentTax;
                pvCollection.Clear();
                pvCollection.Add(pdvOnePercentTax);
                rpt.DataDefinition.ParameterFields["tOnePecentTax"].ApplyCurrentValues(pvCollection);

                pdvRemTax.Value = TremTax;
                pvCollection.Clear();
                pvCollection.Add(pdvRemTax);
                rpt.DataDefinition.ParameterFields["tRemTax"].ApplyCurrentValues(pvCollection);
                #endregion

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                frmReportViewer frm = new frmReportViewer();
                frm.SetReportSource(rpt);
                //Update the progressbar
                ProgressForm.UpdateProgress(100, "Showing Report...");

                // Close the dialog
                ProgressForm.CloseForm();
                switch (prntDirect)
                {
                    case 1:
                        rpt.PrintOptions.PrinterName = "";
                        rpt.PrintToPrinter(1, false, 0, 0);
                        prntDirect = 0;
                        return;
                    case 2:
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        rpt.Export();
                        rpt.Close();
                        return;
                    case 3:
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        rpt.Export();
                        rpt.Close();
                        return;
                    case 4:
                        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                        rpt.Export();
                        frmemail sendemail = new frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        rpt.Close();
                        return;
                    default:
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prntDirect = 0;
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Menu_Export = new ContextMenu();
            MenuItem mnuExcel = new MenuItem();
            mnuExcel.Name = "mnuExcel";
            mnuExcel.Text = "E&xcel";
            MenuItem mnuPDF = new MenuItem();
            mnuPDF.Name = "mnuPDF";
            mnuPDF.Text = "&PDF";
            MenuItem mnuEmail = new MenuItem();
            mnuEmail.Name = "mnuEmail";
            mnuEmail.Text = "E&mail";
            Menu_Export.MenuItems.Add(mnuEmail);
            Menu_Export.MenuItems.Add(mnuExcel);
            Menu_Export.MenuItems.Add(mnuPDF);
            Menu_Export.Show(btnExport, new Point(0, btnExport.Height));

            foreach (MenuItem Item in Menu_Export.MenuItems)
                Item.Click += new EventHandler(Menu_Click);
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            switch (((MenuItem)sender).Name)
            {
                case "mnuExcel":
                    //Code for excel export
                    SaveFileDialog SaveFD = new SaveFileDialog();
                    SaveFD.InitialDirectory = "D:";
                    SaveFD.Title = "Enter Filename:";
                    SaveFD.Filter = "*.xls|*.xls";
                    if (SaveFD.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFD.FileName;
                        FileName = SaveFD.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 2;
                    //btnPrintPreview_Click(sender, e);
                    PrintPreviewCR(PrintType.Excel);
                    break;
                case "mnuPDF":
                    //Code for pdf export
                    SaveFileDialog SaveFDPdf = new SaveFileDialog();
                    SaveFDPdf.InitialDirectory = "D:";
                    SaveFDPdf.Title = "Enter Filename:";
                    SaveFDPdf.Filter = "*.pdf|*.pdf";
                    if (SaveFDPdf.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFDPdf.FileName;
                        FileName = SaveFDPdf.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 3;
                    //btnPrintPreview_Click(sender, e);
                    PrintPreviewCR(PrintType.PDF);
                    break;
                case "mnuEmail":
                    //Code for pdf export
                    SaveFileDialog SaveFDExcelEmail = new SaveFileDialog();
                    SaveFDExcelEmail.InitialDirectory = "D:";
                    SaveFDExcelEmail.Title = "Enter Filename:";
                    SaveFDExcelEmail.Filter = "*.xls|*.xls"; ;
                    if (SaveFDExcelEmail.ShowDialog() != DialogResult.Cancel)
                    {
                        string FileToRestore = SaveFDExcelEmail.FileName;
                        FileName = SaveFDExcelEmail.FileName;
                    }
                    else
                    {
                        return;
                    }
                    prntDirect = 4;
                    //btnPrintPreview_Click(sender, e);
                    PrintPreviewCR(PrintType.Email);
                    break;
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //Load all over again;
            frmSalarySheet_Load(sender, e);

            this.Cursor = Cursors.Default;
        }

        BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
        private BusinessLogic.HRM.Report.EmployeeReportSettings m_settings;

        public decimal CalculateOnePercentTax(int empID, decimal TotaltaxAmt)
        {
            try
            {
                DataTable dt = employees.GetEmployeeForTax(empID);
                DataRow dr = dt.Rows[0];

                decimal OnePercenttaxAmt = 0;
                if (dr["EmpType"].ToString() == "Normal")
                {
                    if (dr["IsSingle"].ToString() == "True" || dr["IsCoupleWorking"].ToString() == "True")
                    {
                        //if (annualNetSalary <= 350000)
                        //{
                        //    taxAmount = netSalaryAfterInsurance * (decimal)0.01;
                        //    OnePercentTax = taxAmount;
                        //}
                        if (TotaltaxAmt <= (decimal)333.333)
                        {
                            OnePercenttaxAmt = TotaltaxAmt;
                        }
                        else
                            OnePercenttaxAmt = (decimal)333.33;
                        //else if (annualNetSalary <= 450000)
                        //{
                        //    taxAmount = (decimal)291.67 + ((netSalaryAfterInsurance - (decimal)29166.67) * (decimal)0.15);
                        //    OnePercentTax = (decimal)291.67;
                        //}
                        //else if(TotaltaxAmt <= )
                        //else
                        //{
                        //    taxAmount = (decimal)1541.66 + ((netSalaryAfterInsurance - (decimal)37500) * (decimal)0.25);
                        //    OnePercentTax = (decimal)291.67;
                        //}
                    }
                    else if (dr["IsSingle"].ToString() == "False" && dr["IsCoupleWorking"].ToString() == "False")
                    {
                        //if (annualNetSalary <= 400000)
                        //{
                        //    taxAmount = netSalaryAfterInsurance * (decimal)0.01;
                        //    OnePercentTax = taxAmount;
                        //}
                        //else if (annualNetSalary <= 500000)
                        //{
                        //    taxAmount = (decimal)333.33 + ((netSalaryAfterInsurance - (decimal)33333.33) * (decimal)0.15);
                        //    OnePercentTax = (decimal)333.33;
                        //}
                        //else
                        //{
                        //    taxAmount = (decimal)1583.33 + ((netSalaryAfterInsurance - (decimal)41666.66) * (decimal)0.25);
                        //    OnePercentTax = (decimal)333.33;
                        //}

                        if (TotaltaxAmt <= (decimal)333.333)
                        {
                            OnePercenttaxAmt = TotaltaxAmt;
                        }
                        else
                            OnePercenttaxAmt = (decimal)333.33;
                    }
                }
                else if (dr["EmpType"].ToString() == "Disable")
                {
                    if (dr["IsSingle"].ToString() == "True" || dr["IsCoupleWorking"].ToString() == "True")
                    {
                        //if (annualNetSalary <= 525000)
                        //{
                        //    taxAmount = netSalaryAfterInsurance * (decimal)0.01;
                        //    OnePercentTax = taxAmount;
                        //}
                        //else if (annualNetSalary <= 675000)
                        //{
                        //    taxAmount = (decimal)437.5 + ((netSalaryAfterInsurance - (decimal)43750) * (decimal)0.15);
                        //    OnePercentTax = (decimal)437.5;
                        //}
                        //else
                        //{
                        //    taxAmount = (decimal)2312.5 + ((netSalaryAfterInsurance - (decimal)56250) * (decimal)0.25);
                        //    OnePercentTax = (decimal)437.5;
                        //}
                        if (TotaltaxAmt <= (decimal)437.5)
                        {
                            OnePercenttaxAmt = TotaltaxAmt;
                        }
                        else
                            OnePercenttaxAmt = (decimal)437.5;
                    }
                    else if (dr["IsSingle"].ToString() == "False" && dr["IsCoupleWorking"].ToString() == "False")
                    {
                        //if (annualNetSalary <= 600000)
                        //{
                        //    taxAmount = netSalaryAfterInsurance * (decimal)0.01;
                        //    OnePercentTax = taxAmount;
                        //}
                        //else if (annualNetSalary <= 750000)
                        //{
                        //    taxAmount = (decimal)500 + ((netSalaryAfterInsurance - (decimal)50000) * (decimal)0.15);
                        //    OnePercentTax = (decimal)500;
                        //}
                        //else
                        //{
                        //    taxAmount = (decimal)2375 + ((netSalaryAfterInsurance - (decimal)62500) * (decimal)0.25);
                        //    OnePercentTax = (decimal)500;
                        //}

                        if (TotaltaxAmt <= (decimal)500)
                        {
                            OnePercenttaxAmt = TotaltaxAmt;
                        }
                        else
                            OnePercenttaxAmt = (decimal)500;
                    }
                }

                //if (dr["Gender"].ToString() == "2" && dr["IsSingle"].ToString() == "True")
                //{
                //    decimal tempTax = taxAmount - (taxAmount * (decimal)0.1);
                //    taxAmount = tempTax;
                //}

                return OnePercenttaxAmt;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                return 0;
            }
        }
    }
}
