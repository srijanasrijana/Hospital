using BusinessLogic;
using BusinessLogic.HRM.Report;
using Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DateManager;
using HRM.Reports;
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
    /// <summary>
    /// This form is used to show Pension Fund, CIT and Tax report of employee
    /// </summary>
    public partial class frmCommonReport : Form
    {
        DataTable dtFound = new DataTable();
        BusinessLogic.HRM.Employee emp = new BusinessLogic.HRM.Employee();
        private DataSet dsEmployee;
        public enum GridColumns
        {
            Particular, Sharwan, Bhardra, Ashoj, Kartik, Mansir, Poush, Magh, Falgun, Chaitra, Baishak, Jestha, Ashar
        }
        public enum GridColumnsTaxAdjust
        {
            Name, Designation, PanNo,GrossAmount, PFDeduct, PensionDeduct, CIT, InsurancePremium, TotalDeduction, TaxableAmount, TotalTaxDeduction, ValidTax, TaxForAdjustment
        }
        //int m_paySlipID = 0;
        //int[] m_paySlipIDs = null;

        public enum reportType { Pension, CIT, TaxOnePercent, Tax15and25Percent, Wefare, Annual, TaxAdjust }
        public reportType m_reportType;
        private string FileName = "";
        private string m_PfTotal = "";
        private string m_PfAmtTotal = "";
        private string m_Faculty = "";
        private string m_ReportTitle = "";
        private string m_AmtFieldName = "";
        private string m_AmtFieldHeaderName = "";
        private string m_NoFieldName = "";
        private string m_NoFieldHeaderName = "";
        private string m_DesignationName = "";

        private string m_TableName = "";
        private int prntDirect = 0;
        private string[] colNames = null;
        //private bool m_isRemaining = false;
        //private string m_Date = "";

        //For Export Menu
        ContextMenu Menu_Export;
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        public frmCommonReport()
        {
            InitializeComponent();
        }
        //public frmCommonReport(int paySlipId, string faculty, reportType reportType)
        //{
        //    InitializeComponent();
        //    m_paySlipID = paySlipId;
        //    m_Faculty = faculty;
        //    m_reportType = reportType;
        //}
        //public frmCommonReport(int[] paySlipId, string faculty, reportType reportType, bool isRemaining, string date, DateTime fromDate, DateTime toDate)
        //{
        //    InitializeComponent();
        //    m_paySlipIDs = paySlipId;
        //    m_Faculty = faculty;
        //    m_reportType = reportType;
        //    m_isRemaining = isRemaining;
        //    m_Date = date;
        //}
        public EmployeeReportSettings m_settings;
        public frmCommonReport(EmployeeReportSettings settings, reportType reportType)
        {
            m_settings = settings;
            m_reportType = reportType;
            InitializeComponent();

        }
        EmpAnnualReportSettings m_AnnualReportSettings = null;
        public frmCommonReport(EmpAnnualReportSettings Settings, reportType reportType)
        {
            m_AnnualReportSettings = Settings;
            m_reportType = reportType;
            InitializeComponent();
        }
        private void frmPensionFundReport_Load(object sender, EventArgs e)
        {
            try
            {

                switch (m_reportType)
                {
                    case reportType.Pension:
                        WriteBanner();

                        m_NoFieldName = "PensionNumber";
                        m_AmtFieldName = "PensionAmt";
                        m_AmtFieldHeaderName = "";
                        m_NoFieldHeaderName = "Pension No.";
                        dtFound = BusinessLogic.HRM.Employee.GetPensionReport(m_settings);//(m_paySlipIDs, m_isRemaining, m_Date);
                        WriteHeader();
                        LoadGrid(false);
                        m_ReportTitle = "Employee's Pension Fund Report" + (m_settings.isRemaining ? "(Remaining)" : "");
                        this.Text = "Pension Fund Report";
                        break;

                    case reportType.CIT:
                        WriteBanner();

                        m_NoFieldName = "CITNumber";
                        m_AmtFieldName = "CITAmt";
                        m_AmtFieldHeaderName = "";
                        m_NoFieldHeaderName = "CIT No.";
                        dtFound = BusinessLogic.HRM.Employee.GetCITReport(m_settings);//(m_paySlipIDs, m_isRemaining, m_Date);
                        WriteHeader();
                        LoadGrid(false);
                        m_ReportTitle = "Employee's CIT Report" + (m_settings.isRemaining ? "(Remaining)" : "");
                        this.Text = "CIT Report";
                        break;

                    case reportType.TaxOnePercent:
                        WriteBanner();

                        m_NoFieldName = "TaxNumber";
                        m_AmtFieldName = "TaxAmt";
                        m_AmtFieldHeaderName = "";
                        m_NoFieldHeaderName = "PAN No.";
                        dtFound = BusinessLogic.HRM.Employee.GetTaxReport(m_settings);//(m_paySlipIDs, m_isRemaining, m_Date);
                        WriteTaxHeader();
                        LoadTaxGrid(true);
                        m_ReportTitle = "Employee's Tax Report( 1 %) " + (m_settings.isRemaining ? "(Remaining)" : "");
                        this.Text = "Tax Report";
                        break;

                    case reportType.Tax15and25Percent:
                        WriteBanner();

                        m_NoFieldName = "TaxNumber";
                        m_AmtFieldName = "TaxAmt";
                        m_AmtFieldHeaderName = "";
                        m_NoFieldHeaderName = "PAN No.";
                        dtFound = BusinessLogic.HRM.Employee.GetTaxReport(m_settings);//(m_paySlipIDs, m_isRemaining, m_Date);
                        WriteTaxHeader();
                        LoadTaxGrid(false);
                        m_ReportTitle = "Employee's Tax Report( 15 % && 25 %)" + (m_settings.isRemaining ? "(Remaining)" : "");
                        this.Text = "Tax Report";
                        break;
                    case reportType.Wefare:
                        WriteBanner();

                        m_NoFieldName = "KalyankariNo";
                        m_NoFieldHeaderName = "Welfare No.";
                        m_AmtFieldName = "KalyankariAmt";
                        dtFound = BusinessLogic.HRM.Employee.GetWelfareReport(m_settings);//(m_paySlipIDs, m_isRemaining, m_Date);
                        WriteHeader();
                        LoadGrid(false);
                        m_ReportTitle = "Employee Welfare Report" + (m_settings.isRemaining ? "(Remaining)" : "");
                        this.Text = "Tax Report";
                        break;

                    case reportType.Annual:
                        CreateData();
                        m_ReportTitle = "Annual Employee Report";
                        WriteEmpAnnualHeader();
                        LoadAnnualGrid();
                        this.Text = "Annual Employee Report";
                        lblReportTitle.Text = m_ReportTitle;
                        lblEmployeeName.Text = "Employee Name:   " + m_AnnualReportSettings.EmployeeName;
                        lblDesignation.Text = "Designation:   " + m_DesignationName;
                        lblFiscalYear.Text ="Fiscal Year:   "+ m_AnnualReportSettings.Year;
                        return;

                    case reportType.TaxAdjust:
                        m_ReportTitle = "Annual Tax Adjust Report";
                        dtFound = BusinessLogic.HRM.Employee.GetAnnualTaxAdjustReport(m_AnnualReportSettings.Year, m_AnnualReportSettings.FromMonth, m_AnnualReportSettings.ToMonth, m_AnnualReportSettings.FacultyID);//(m_paySlipIDs, m_isRemaining, m_Date);
                        //WriteTaxAdjustHeader();
                        LoadTaxAdjustGrid();
                        this.Text = "Annual Tax Adjust Report";
                        lblReportTitle.Text = m_ReportTitle;
                        lblEmployeeName.Text ="";// "Employee Name:   " + m_AnnualReportSettings.EmployeeName;
                        lblDesignation.Text = "Designation:   " + m_DesignationName;
                        lblFiscalYear.Text = "Fiscal Year:   " + m_AnnualReportSettings.Year;
                        lblFaculty.Text = "Faculty: "+ m_AnnualReportSettings.FacultyName;
                        return;
                }

                lblReportTitle.Text = m_ReportTitle;

                if (m_settings.paySlipDate != null)
                {
                    lblPaySlipDate.Text = Date.DBToSystem(m_settings.paySlipDate.ToString());
                }

                if (m_settings.fromDate != null && m_settings.toDate != null)
                {
                    lblFromDate.Text = "From: " + Date.DBToSystem(m_settings.fromDate.ToString());
                }
            }
            catch (Exception ex)
            {

                Global.MsgError(ex.Message);
            }
        }


        private void WriteBanner()
        {
            DataTable dt = emp.GetSalaryMaster(m_settings.paySlipIds[0]);
            if (dt.Rows.Count > 0)
            {
                lblMonth.Text += dt.Rows[0]["year"].ToString() + " " + dt.Rows[0]["monthName"].ToString();
                lblDate.Text += DateManager.Date.ToSystem(DateManager.Date.GetServerDate()).ToString();
                lblFaculty.Text += m_Faculty;
            }
        }
        bool isTaxReport;
        public enum TaxColumns { SN, PANNo, Name, TDate, DateType, PaymentAmount, TDSAmount, TDSType }
        private void WriteTaxHeader()
        {
            int rowsCount = 0;
            rowsCount = dtFound.Rows.Count;
            grdEmployeePF.Rows.Clear();

            grdEmployeePF.Selection.EnableMultiSelection = false;
            grdEmployeePF.Redim(rowsCount + 2, 8);

            grdEmployeePF[0, (int)TaxColumns.SN] = new MyHeader("SN");
            grdEmployeePF[0, (int)TaxColumns.Name] = new MyHeader("Name");

            grdEmployeePF[0, (int)TaxColumns.PANNo] = new MyHeader("PAN");
            grdEmployeePF[0, (int)TaxColumns.TDate] = new MyHeader("T Date");
            grdEmployeePF[0, (int)TaxColumns.DateType] = new MyHeader("Date Type");
            grdEmployeePF[0, (int)TaxColumns.PaymentAmount] = new MyHeader("Payment Amount");
            grdEmployeePF[0, (int)TaxColumns.TDSAmount] = new MyHeader("TDS Amount");
            grdEmployeePF[0, (int)TaxColumns.TDSType] = new MyHeader("TDS Type");

            grdEmployeePF[0, (int)TaxColumns.SN].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;
            grdEmployeePF[0, (int)TaxColumns.TDSAmount].Column.Width = 100;
            grdEmployeePF[0, (int)TaxColumns.DateType].Column.Width = 30;
            grdEmployeePF[0, (int)TaxColumns.TDate].Column.Width = 100;
            grdEmployeePF[0, (int)TaxColumns.PaymentAmount].Column.Width =
            grdEmployeePF[0, (int)TaxColumns.TDSAmount].Column.Width =
            grdEmployeePF[0, (int)TaxColumns.TDSType].Column.Width = 100;
        }
        string TDSType = "";
        public void LoadTaxGrid(bool isOnePercent)
        {
            dtFound.Columns.Add("DateType", typeof(string));
            dtFound.Columns.Add("TDSType", typeof(string));

            int rowsCount = 0;
            decimal totalPaymentAmt = 0, tTotalTaxAmt = 0;//, totalRemainingTax = 0;
            int lastRow = 0;
            rowsCount = dtFound.Rows.Count;
            TDSType = isOnePercent ? "33" : "20";
            string DateType = "BS";

            //string paymentAmtColName = isOnePercent ? "OnePercentPayment" : "GrossAmount";
            string colTaxAmount = isOnePercent ? "OnePercentTax" : "RemainingTax";
            for (int i = 1; i <= rowsCount; i++)
            {

                DataRow dr = dtFound.Rows[i - 1];
                string StaffName = dr["StaffName"].ToString();
                string PanNumber = dr["TaxNumber"].ToString();

                if (isOnePercent)
                {
                    dr["GrossAmt"] = (0.00);
                }
                else
                {
                    dr["OnePercentTax"] = dr["RemainingTax"];
                }

                decimal paymentAmt = Convert.ToDecimal(dr["GrossAmt"]);
                decimal TaxAmt = Convert.ToDecimal(dr[colTaxAmount]);
                decimal TotalTaxAmt = Convert.ToDecimal(dr["TaxAmt"]);
                //decimal remainingTax = Convert.ToDecimal(dr["RemainingTax"]);

                dr["TDSType"] = TDSType;
                dr["DateType"] = DateType;
                //decimal TaxAmtt = 0;
                if (TaxAmt <= 0 || TaxAmt == TotalTaxAmt)
                {
                    if (isOnePercent)
                    {
                        TaxAmt = Convert.ToDecimal(CalculateOnePercentTax(Convert.ToInt32(dr["EmployeeID"]), TotalTaxAmt));
                        dr[colTaxAmount] = TaxAmt;
                    }
                    else
                    {
                        TaxAmt = TotalTaxAmt - Convert.ToDecimal(CalculateOnePercentTax(Convert.ToInt32(dr["EmployeeID"]), TotalTaxAmt));
                        dr[colTaxAmount] = TaxAmt;
                    }
                }

                totalPaymentAmt += Convert.ToDecimal(paymentAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                tTotalTaxAmt += Convert.ToDecimal(TaxAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                //totalRemainingTax += remainingTax;

                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (i % 2 == 0)
                {
                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }

                grdEmployeePF[i, (int)TaxColumns.SN] = new SourceGrid.Cells.Cell(i.ToString());
                grdEmployeePF[i, (int)TaxColumns.Name] = new SourceGrid.Cells.Cell(StaffName);
                grdEmployeePF[i, (int)TaxColumns.PANNo] = new SourceGrid.Cells.Cell(PanNumber);
                grdEmployeePF[i, (int)TaxColumns.TDate] = new SourceGrid.Cells.Cell(dr["Date"]);
                grdEmployeePF[i, (int)TaxColumns.DateType] = new SourceGrid.Cells.Cell(DateType);

                grdEmployeePF[i, (int)TaxColumns.PaymentAmount] = new SourceGrid.Cells.Cell(paymentAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                grdEmployeePF[i, (int)TaxColumns.TDSAmount] = new SourceGrid.Cells.Cell(TaxAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));

                grdEmployeePF[i, (int)TaxColumns.TDSType] = new SourceGrid.Cells.Cell(TDSType);

                grdEmployeePF[i, (int)TaxColumns.SN].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdEmployeePF[i, (int)TaxColumns.TDate].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdEmployeePF[i, (int)TaxColumns.Name].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdEmployeePF[i, (int)TaxColumns.DateType].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdEmployeePF[i, (int)TaxColumns.PANNo].View = new SourceGrid.Cells.Views.Cell(alternate);

                alternate.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                grdEmployeePF[i, (int)TaxColumns.TDSAmount].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdEmployeePF[i, (int)TaxColumns.TDSType].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdEmployeePF[i, (int)TaxColumns.PaymentAmount].View = new SourceGrid.Cells.Views.Cell(alternate);

                lastRow = i;
            }

            #region View for Last row i.e row of total
            SourceGrid.Cells.Views.Cell RowTotal = new SourceGrid.Cells.Views.Cell();
            RowTotal.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            RowTotal.Font = new Font("Arial", 9, FontStyle.Bold);
            RowTotal.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
            #endregion

            grdEmployeePF[lastRow + 1, (int)TaxColumns.SN] = new SourceGrid.Cells.Cell("Total");
            grdEmployeePF[lastRow + 1, (int)TaxColumns.SN].ColumnSpan = 5;
            grdEmployeePF[lastRow + 1, (int)TaxColumns.PaymentAmount] = new SourceGrid.Cells.Cell(totalPaymentAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
            grdEmployeePF[lastRow + 1, (int)TaxColumns.TDSAmount] = new SourceGrid.Cells.Cell(tTotalTaxAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
            grdEmployeePF[lastRow + 1, (int)TaxColumns.TDSType] = new SourceGrid.Cells.Cell("");

            grdEmployeePF[lastRow + 1, (int)TaxColumns.SN].View = new SourceGrid.Cells.Views.Cell(RowTotal);
            grdEmployeePF[lastRow + 1, (int)TaxColumns.PaymentAmount].View = new SourceGrid.Cells.Views.Cell(RowTotal);
            grdEmployeePF[lastRow + 1, (int)TaxColumns.TDSAmount].View = new SourceGrid.Cells.Views.Cell(RowTotal);
            grdEmployeePF[lastRow + 1, (int)TaxColumns.TDSType].View = new SourceGrid.Cells.Views.Cell(RowTotal);
        }
        private void WriteHeader()
        {
            int rowsCount = 0;
            rowsCount = dtFound.Rows.Count;
            grdEmployeePF.Rows.Clear();
            //grdEmployeePF.FixedColumns = 7;
            //grdEmployeePF.FixedRows = 1;
            grdEmployeePF.Selection.EnableMultiSelection = false;
            grdEmployeePF.Redim(rowsCount + 2, 7);

            grdEmployeePF[0, 0] = new MyHeader("SN");
            grdEmployeePF[0, 1] = new MyHeader("Designation");
            grdEmployeePF[0, 2] = new MyHeader("Employee's Name");
            grdEmployeePF[0, 3] = new MyHeader(m_NoFieldHeaderName);
            grdEmployeePF[0, 4] = new MyHeader((m_reportType == reportType.Pension ? "Total Fund Deducted" : "Gross Amount"));
            grdEmployeePF[0, 5] = new MyHeader(m_reportType == reportType.Pension ? "Pension Fund Contribution by Employer" : (m_reportType == reportType.CIT ? "CIT Amount" : (m_reportType == reportType.Wefare) ? "Welfare Amount" : "Tax Amount"));
            grdEmployeePF[0, 6] = new MyHeader("Pension Fund Deducted from Employee");
            grdEmployeePF[0, 6].Column.Visible = (m_reportType == reportType.Pension ? true : false);
            isTaxReport = (m_reportType == reportType.TaxOnePercent) ? true : false;
            grdEmployeePF[0, 4].Column.Visible = isTaxReport | (m_reportType == reportType.Pension); // if related to tax report then hide the CIT/Pension No field

            grdEmployeePF[0, 0].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;
            grdEmployeePF[0, 0].Column.Width = 30;
            grdEmployeePF[0, 1].Column.Width = 200;
            grdEmployeePF[0, 2].Column.Width = 250;
            grdEmployeePF[0, 3].Column.Width =
            grdEmployeePF[0, 4].Column.Width =
            grdEmployeePF[0, 6].Column.Width =

            grdEmployeePF[0, 5].Column.Width = 100;
        }

        private void LoadGrid(bool isCrystalRpt)
        {
            try
            {
                int rowsCount = 0;
                decimal totalPF = 0, totalPFT = 0, totalGross = 0;
                int lastRow = 0;
                rowsCount = dtFound.Rows.Count;
                if (rowsCount > 0)
                {

                    for (int i = 1; i <= rowsCount; i++)
                    {

                        DataRow dr = dtFound.Rows[i - 1];
                        string designation = dr["DesignationName"].ToString();
                        string StaffName = dr["StaffName"].ToString();
                        string PfNumber = dr[m_NoFieldName].ToString();
                        decimal amt = Convert.ToDecimal(dr[m_AmtFieldName]);
                        decimal GrossAmt = isTaxReport ? Convert.ToDecimal(dr["GrossAmount"]) : amt * 2;
                        if (!isCrystalRpt)
                        {
                            SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                            if (i % 2 == 0)
                            {
                                //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                            }
                            else
                            {
                                alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                            }

                            grdEmployeePF[i, 0] = new SourceGrid.Cells.Cell(i.ToString());
                            grdEmployeePF[i, 1] = new SourceGrid.Cells.Cell(designation);
                            grdEmployeePF[i, 2] = new SourceGrid.Cells.Cell(StaffName);
                            grdEmployeePF[i, 3] = new SourceGrid.Cells.Cell(PfNumber);
                            grdEmployeePF[i, 4] = new SourceGrid.Cells.Cell(GrossAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));

                            grdEmployeePF[i, 5] = new SourceGrid.Cells.Cell(amt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                            grdEmployeePF[i, 6] = new SourceGrid.Cells.Cell(amt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                            //grdEmployeePF[i, 7] = new SourceGrid.Cells.Cell(amt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));


                            grdEmployeePF[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                            alternate.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                            grdEmployeePF[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
                            //grdEmployeePF[i, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);

                        }
                        else
                        {
                            dsEmployee.Tables["tblEmpPfDetails"].Rows.Add(StaffName, designation, PfNumber, amt);
                        }
                        totalPF += Convert.ToDecimal(amt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        totalGross += Convert.ToDecimal(GrossAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        lastRow = i;
                    }
                    if (!isCrystalRpt)
                    {
                        #region View for Last row i.e row of total
                        SourceGrid.Cells.Views.Cell RowTotal = new SourceGrid.Cells.Views.Cell();
                        RowTotal.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        RowTotal.Font = new Font("Arial", 9, FontStyle.Bold);
                        RowTotal.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        #endregion

                        grdEmployeePF[lastRow + 1, 0] = new SourceGrid.Cells.Cell("Total");
                        grdEmployeePF[lastRow + 1, 0].ColumnSpan = 4;
                        grdEmployeePF[lastRow + 1, 4] = new SourceGrid.Cells.Cell(totalGross.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdEmployeePF[lastRow + 1, 5] = new SourceGrid.Cells.Cell(totalPF.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));
                        grdEmployeePF[lastRow + 1, 6] = new SourceGrid.Cells.Cell(totalPF.ToString(Misc.FormatNumber(false, Global.DecimalPlaces)));

                        grdEmployeePF[lastRow + 1, 0].View = new SourceGrid.Cells.Views.Cell(RowTotal);
                        grdEmployeePF[lastRow + 1, 4].View = new SourceGrid.Cells.Views.Cell(RowTotal);
                        grdEmployeePF[lastRow + 1, 5].View = new SourceGrid.Cells.Views.Cell(RowTotal);
                        grdEmployeePF[lastRow + 1, 6].View = new SourceGrid.Cells.Views.Cell(RowTotal);

                    }
                    else
                    {
                        //add total pf amount and total of total pf as parameter
                        m_PfTotal = totalPF.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                        m_PfAmtTotal = totalPFT.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));

                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
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

                //dsEmployee.Clear();
                //LoadGrid(true);
                ReportClass rpt = null;//= new HRM.Reports.rptPensionFund();

                //Set DataSource to be dsEmployee dataset on the report


                //dsStudent.Tables.Remove("tblStudent");

               string reportTitle = "";
                colNames = new string[]{
                        "DesignationName", "StaffName", m_NoFieldName, m_AmtFieldName
                    };

                if (m_reportType == reportType.Pension)
                {
                    dsEmployee = new Model.dsPensionFund();
                    m_TableName = "tblPensionFund";
                    reportTitle = "Employee Pension Fund";
                    rpt = new HRM.Reports.rptPensionFund();

                }

                else if (m_reportType == reportType.CIT)
                {
                    dsEmployee = new Model.dsEmployeeCIT();
                    m_TableName = "tblEmployeeCIT";
                    reportTitle = "Employee CIT";
                    rpt = new HRM.Reports.rptEmployeeCIT();

                }

                else if (m_reportType == reportType.TaxOnePercent || m_reportType == reportType.Tax15and25Percent)
                {
                    dsEmployee = new Model.dsEmployeeTax();
                    m_TableName = "tblTaxReport";
                    reportTitle = "Employee Tax";
                    rpt = new HRM.Reports.rptEmployeeTax();

                    colNames = new string[]{
                         "TaxNumber", "StaffName", "Date", "DateType","GrossAmt","OnePercentTax","TDSType", "RemainingTax"
                    };

                    //((rptEmployeeTax)rpt).Section2.ReportObjects[m_reportType == reportType.TaxOnePercent ? "RemainingTax1" : "OnePercentTax1"].Width = 0;

                    isTaxReport = true;
                }
                else if (m_reportType == reportType.Wefare)
                {
                    dsEmployee = new Model.dsEmployeeWelfare();
                    m_TableName = "tblEmployeeWelfare";
                    reportTitle = "Employee Welfare";
                    rpt = new HRM.Reports.rptEmployeeWelfare();

                }

                else if (m_reportType == reportType.Annual)
                {
                    dsEmployee = new Model.dsEmpAnnualReport();
                    m_TableName = "tblDetails";
                    reportTitle = "Annual Employee Report";
                    rpt = new HRM.Reports.rptEmpAnnualReport();
                    colNames = new string[]{
                    "Baishak",
                    "Jestha",
                    "Ashar",
                    "Shrawan",
                    "Bhadra",
                    "Ashwin",
                    "Kartik",
                    "Mansir",
                    "Poush",
                    "Magh",
                    "Falgun",
                    "Chaitra",
                    "Particulars"
                    };

                    dtFound = dtData.Copy();
                    dtFound.Rows.RemoveAt(dtFound.Rows.Count - 1);
                    dtFound.Rows.RemoveAt(dtFound.Rows.Count - 1); 

                }
                else if (m_reportType == reportType.TaxAdjust)
                {
                    dsEmployee = new Model.dsTaxAdjust();
                    m_TableName = "tblTaxAdjust";
                    reportTitle = "Tax Adjustment Report";
                    rpt = new HRM.Reports.rptTaxAdjustReport();
                    colNames = new string[]{
                    "Name",
                    "DesignationName",
                    "PAN",
                    "PFDeduct",
                    "PensionFDeduct",
                    "CIT",
                    "InsurancePremium",
                    "TotalDeduction",
                    "TaxableAmount",
                    "TotalTax",
                    "ValidTax",
                    "TaxForAdjustment"
                    };


                    //dtFound = dtData.Copy();
                }

                rpt.SetDataSource(dsEmployee);
                try
                {
                    dsEmployee.Tables.Remove(m_TableName);

                }
                catch
                {

                }


                System.Data.DataView view = new System.Data.DataView(dtFound);

                DataTable selected = view.ToTable(m_TableName, false, colNames);

                dsEmployee.Tables.Add(selected);

                //Fill the logo on the report
                Misc.WriteLogo(dsEmployee, "tblImage");

                //Provide values to the parameters on the report
                CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
                //CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvMonth = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvFaculty = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvPfTotal = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvPfTotalGross = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_PaySlipDate = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvPfEmployeeName = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvDesignation = new CrystalDecisions.Shared.ParameterDiscreteValue();


                //CrystalDecisions.Shared.ParameterDiscreteValue pdvPfAmtTotal = new CrystalDecisions.Shared.ParameterDiscreteValue();
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
                pdvReport_Head.Value = m_ReportTitle;
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);


                pdvMonth.Value = lblMonth.Text;
                pvCollection.Clear();
                pvCollection.Add(pdvMonth);
                rpt.DataDefinition.ParameterFields["Month"].ApplyCurrentValues(pvCollection);


                // pdvReport_Date.Value = "As On Date:" + m_DayBook.ToDate.ToString("yyyy/MM/dd");
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem(DateTime.Today);
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                pdvReport_PaySlipDate.Value = lblPaySlipDate.Text;
                pvCollection.Clear();
                pvCollection.Add(pdvReport_PaySlipDate);
                rpt.DataDefinition.ParameterFields["PaySlip_Date"].ApplyCurrentValues(pvCollection);


                pdvFaculty.Value = lblFaculty.Text;
                pvCollection.Clear();
                pvCollection.Add(pdvFaculty);
                rpt.DataDefinition.ParameterFields["Faculty"].ApplyCurrentValues(pvCollection);

                pdvPfTotal.Value = " ";
                if (m_reportType != reportType.Annual && m_reportType!= reportType.TaxAdjust)
                    pdvPfTotal.Value = Convert.ToDecimal(grdEmployeePF[grdEmployeePF.Rows.Count - 1, isTaxReport ? (int)TaxColumns.TDSAmount : 5].Value).ToString(Misc.FormatNumber(true, Global.DecimalPlaces));
                pvCollection.Clear();
                pvCollection.Add(pdvPfTotal);
                rpt.DataDefinition.ParameterFields["TotalPF"].ApplyCurrentValues(pvCollection);

                if (isTaxReport)
                {
                    pdvPfTotalGross.Value = Convert.ToDecimal(grdEmployeePF[grdEmployeePF.Rows.Count - 1, (int)TaxColumns.PaymentAmount].Value).ToString(Misc.FormatNumber(true, Global.DecimalPlaces));
                    pvCollection.Clear();
                    pvCollection.Add(pdvPfTotalGross);
                    rpt.DataDefinition.ParameterFields["TotalGross"].ApplyCurrentValues(pvCollection);

                }
                if (m_reportType == reportType.Annual)
                {
                    pdvPfEmployeeName.Value = m_AnnualReportSettings.EmployeeName;
                    pvCollection.Clear();
                    pvCollection.Add(pdvPfEmployeeName);
                    rpt.DataDefinition.ParameterFields["Employee_Name"].ApplyCurrentValues(pvCollection);


                    pdvDesignation.Value = m_DesignationName;
                    pvCollection.Clear();
                    pvCollection.Add(pdvDesignation);
                    rpt.DataDefinition.ParameterFields["Employee_Designation"].ApplyCurrentValues(pvCollection);


                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Baisakh = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Jestha = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Asar = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Shrawan = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Bhadra = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Asoj = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Kartik = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Mangsir = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Poush = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Magh = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Falgun = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvTotal_Chaitra = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvFiscalYear = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    pdvFiscalYear.Value = lblFiscalYear.Text;
                    pvCollection.Clear();
                    pvCollection.Add(pdvFiscalYear);
                    rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                    #region total not required
                    //pdvTotal_Baisakh.Value =total_Baisakh;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Baisakh);
                    //rpt.DataDefinition.ParameterFields["Total_Baisakh"].ApplyCurrentValues(pvCollection);


                    //pdvTotal_Jestha.Value = total_Jesth;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Jestha);
                    //rpt.DataDefinition.ParameterFields["Total_Jestha"].ApplyCurrentValues(pvCollection);

                    //pdvTotal_Asar.Value =total_Asar;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Asar);
                    //rpt.DataDefinition.ParameterFields["Total_Asar"].ApplyCurrentValues(pvCollection);


                    //pdvTotal_Shrawan.Value = total_Shrawan;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Shrawan);
                    //rpt.DataDefinition.ParameterFields["Total_Shrawan"].ApplyCurrentValues(pvCollection);

                    //pdvTotal_Bhadra.Value =total_Bhadra;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Bhadra);
                    //rpt.DataDefinition.ParameterFields["Total_Bhadra"].ApplyCurrentValues(pvCollection);


                    //pdvTotal_Asoj.Value = total_Asoj;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Asoj);
                    //rpt.DataDefinition.ParameterFields["Total_Asoj"].ApplyCurrentValues(pvCollection);


                    //pdvTotal_Kartik.Value =total_Kartik;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Kartik);
                    //rpt.DataDefinition.ParameterFields["Total_Kartik"].ApplyCurrentValues(pvCollection);


                    //pdvTotal_Mangsir.Value = total_Mangsir;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Mangsir);
                    //rpt.DataDefinition.ParameterFields["Total_Mangsir"].ApplyCurrentValues(pvCollection);


                    //pdvTotal_Poush.Value = total_Poush;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Poush);
                    //rpt.DataDefinition.ParameterFields["Total_Poush"].ApplyCurrentValues(pvCollection);


                    //pdvTotal_Magh.Value = total_Magh;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Magh);
                    //rpt.DataDefinition.ParameterFields["Total_Magh"].ApplyCurrentValues(pvCollection);


                    //pdvTotal_Falgun.Value =total_Falgun;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Falgun);
                    //rpt.DataDefinition.ParameterFields["Total_Falgun"].ApplyCurrentValues(pvCollection);


                    //pdvTotal_Chaitra.Value = total_Chaitra;
                    //pvCollection.Clear();
                    //pvCollection.Add(pdvTotal_Chaitra);
                    //rpt.DataDefinition.ParameterFields["Total_Chaitra"].ApplyCurrentValues(pvCollection); 
                    #endregion

                }
                //pdvPfAmtTotal.Value = m_PfAmtTotal;
                //pvCollection.Clear();
                //pvCollection.Add(pdvPfAmtTotal);
                //rpt.DataDefinition.ParameterFields["TotalPFofTotal"].ApplyCurrentValues(pvCollection);
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
            frmPensionFundReport_Load(sender, e);

            this.Cursor = Cursors.Default;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            prntDirect = 0;
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();

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

        public DataTable InitColumns(DataTable dt)
        {
            dt = new DataTable();
            dt.Columns.Add("Baishak");
            dt.Columns.Add("Jestha");
            dt.Columns.Add("Ashar");
            dt.Columns.Add("Shrawan");
            dt.Columns.Add("Bhadra");
            dt.Columns.Add("Ashwin");
            dt.Columns.Add("Kartik");
            dt.Columns.Add("Mansir");
            dt.Columns.Add("Poush");
            dt.Columns.Add("Magh");
            dt.Columns.Add("Falgun");
            dt.Columns.Add("Chaitra");
            dt.Columns.Add("Particulars");

            return dt;
        }

        public void WriteEmpAnnualHeader()
        {

            int rowsCount = 0;
            rowsCount = dtData.Rows.Count;
            grdEmployeePF.Rows.Clear();
            //grdEmployeePF.FixedColumns = 7;
            grdEmployeePF.FixedRows = 1;
            grdEmployeePF.Selection.EnableMultiSelection = false;
            grdEmployeePF.Redim(rowsCount + 3, 13);

            grdEmployeePF[0, (int)GridColumns.Particular] = new MyHeader("Particular/Month");
            grdEmployeePF[0, (int)GridColumns.Baishak] = new MyHeader("Baishak");
            grdEmployeePF[0, (int)GridColumns.Jestha] = new MyHeader("Jestha");
            grdEmployeePF[0, (int)GridColumns.Ashar] = new MyHeader("Ashar");
            grdEmployeePF[0, (int)GridColumns.Sharwan] = new MyHeader("Sharwan");
            grdEmployeePF[0, (int)GridColumns.Bhardra] = new MyHeader("Bhardra");
            grdEmployeePF[0, (int)GridColumns.Ashoj] = new MyHeader("Ashoj");
            grdEmployeePF[0, (int)GridColumns.Kartik] = new MyHeader("Kartik");
            grdEmployeePF[0, (int)GridColumns.Mansir] = new MyHeader("Mansir");
            grdEmployeePF[0, (int)GridColumns.Poush] = new MyHeader("Poush");
            grdEmployeePF[0, (int)GridColumns.Magh] = new MyHeader("Magh");
            grdEmployeePF[0, (int)GridColumns.Falgun] = new MyHeader("Falgun");
            grdEmployeePF[0, (int)GridColumns.Chaitra] = new MyHeader("Chaitra");

            grdEmployeePF[0, 0].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdEmployeePF[0, (int)GridColumns.Particular].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Baishak].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Jestha].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Ashar].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Sharwan].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Bhardra].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Ashoj].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Kartik].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Mansir].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Poush].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Magh].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Falgun].Column.Width =
            grdEmployeePF[0, (int)GridColumns.Chaitra].Column.Width = 250;

            //for (int i = 1; i < 13; i++)
            //{
            //    if (i >= m_AnnualReportSettings.FromMonth && i <= m_AnnualReportSettings.ToMonth)
            //    {

            //    }
            //    else
            //    {
            //        grdEmployeePF.Columns[i+3].Visible = false;
            //    }
            //}
        }
        public void WriteTaxAdjustHeader()
        {

            int rowsCount = 0;
            rowsCount = dtFound.Rows.Count;
            grdEmployeePF.Rows.Clear();
            //grdEmployeePF.FixedColumns = 7;
            grdEmployeePF.FixedRows = 1;
            grdEmployeePF.Selection.EnableMultiSelection = false;
            grdEmployeePF.Redim(rowsCount + 3, 13);

            grdEmployeePF[0, (int)GridColumnsTaxAdjust.Name] = new MyHeader("Name");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.Designation] = new MyHeader("Designation");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.PanNo] = new MyHeader("PanNo");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.GrossAmount] = new MyHeader("Gross Amount");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.PFDeduct] = new MyHeader("PF Adjust");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.PensionDeduct] = new MyHeader("Pension F Adjust");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.CIT] = new MyHeader("CIT");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.InsurancePremium] = new MyHeader("Insurance Premium");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.TotalDeduction] = new MyHeader("Total Deduction");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.TaxableAmount] = new MyHeader("Taxable Amount");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.TotalTaxDeduction] = new MyHeader("Total Tax Ded");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.ValidTax] = new MyHeader("Valid Tax");
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.TaxForAdjustment] = new MyHeader("Tax for Adjustment");

            grdEmployeePF[0, 0].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.Name].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.GrossAmount].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.PanNo].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.PFDeduct].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.PensionDeduct].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.CIT].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.InsurancePremium].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.TotalDeduction].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.TaxableAmount].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.TotalTaxDeduction].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.ValidTax].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.TaxForAdjustment].Column.Width =
            grdEmployeePF[0, (int)GridColumnsTaxAdjust.Designation].Column.Width = 250;

        }
        private void LoadTaxAdjustGrid()
        {
            try
            {
                //total_Baisakh = total_Jesth = total_Asar = total_Shrawan = total_Bhadra = total_Asoj = total_Kartik = 
                //    total_Mangsir = total_Poush = total_Magh = total_Falgun = total_Chaitra = 0;

                int rowsCount = 0;
                rowsCount = dtFound.Rows.Count;
                grdEmployeePF.Rows.Clear();

                grdEmployeePF.SelectionMode = SourceGrid.GridSelectionMode.Row;
                // grdEmployeePF.Redim(rowsCount + 2, 13);

                WriteTaxAdjustHeader();
                int i = 0;
                for (i = 1; i <= rowsCount - 2; i++)
                {
                    SourceGrid.Cells.Views.Cell alternate = null;
                    SourceGrid.Cells.Views.Cell rightAlign = null;

                    alternate = new SourceGrid.Cells.Views.Cell();
                    rightAlign = new SourceGrid.Cells.Views.Cell();
                    rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;

                    if (i % 2 != 0)
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
                        rightAlign.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
                    }

                    DataRow dr = dtFound.Rows[i - 1];
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.Name] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.Name].View = alternate;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.Designation] = new SourceGrid.Cells.Cell(dr["DesignationName"].ToString());
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.Designation].View = rightAlign;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.PanNo] = new SourceGrid.Cells.Cell(dr["PAN"].ToString());
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.PanNo].View = rightAlign;

                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.GrossAmount] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["GrossAmount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.GrossAmount].View = rightAlign;

                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.PFDeduct] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["PFDeduct"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.PFDeduct].View = rightAlign;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.PensionDeduct] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["PensionFDeduct"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.PensionDeduct].View = rightAlign;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.CIT] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["CIT"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.CIT].View = rightAlign;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.InsurancePremium] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["InsurancePremium"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.InsurancePremium].View = rightAlign;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.TotalDeduction] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["TotalDeduction"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.TotalDeduction].View = rightAlign;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.TaxableAmount] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["TaxableAmount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.TaxableAmount].View = rightAlign;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.TotalTaxDeduction] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["TotalTax"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.TotalTaxDeduction].View = rightAlign;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.ValidTax] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["ValidTax"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.ValidTax].View = rightAlign;


                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.TaxForAdjustment] = new SourceGrid.Cells.Cell(Convert.ToDecimal(dr["TaxForAdjustment"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumnsTaxAdjust.TaxForAdjustment].View = rightAlign;

                    grdEmployeePF.Rows.SetHeight(i, 22);

                }
                // 
                SourceGrid.Cells.Views.Cell tAlternateColor = new SourceGrid.Cells.Views.Cell();
                tAlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(ColorTranslator.FromHtml("#e6f7ff"));
                tAlternateColor.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;


                grdEmployeePF.AutoStretchColumnsToFitWidth = true;

                grdEmployeePF.Columns.StretchToFit();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        //decimal total_Baisakh, total_Jesth, total_Asar, total_Shrawan, total_Bhadra, total_Asoj, total_Kartik, total_Mangsir, total_Poush, total_Magh, total_Falgun, total_Chaitra = 0;
        private void LoadAnnualGrid()
        {
            try
            {
                //total_Baisakh = total_Jesth = total_Asar = total_Shrawan = total_Bhadra = total_Asoj = total_Kartik = 
                //    total_Mangsir = total_Poush = total_Magh = total_Falgun = total_Chaitra = 0;

                int rowsCount = 0;
                rowsCount = dtData.Rows.Count;
                grdEmployeePF.Rows.Clear();

                grdEmployeePF.SelectionMode = SourceGrid.GridSelectionMode.Row;
                // grdEmployeePF.Redim(rowsCount + 2, 13);

                WriteEmpAnnualHeader();
                int i = 0;
                for (i = 1; i <= rowsCount - 2; i++)
                {
                    SourceGrid.Cells.Views.Cell alternate = null;
                    SourceGrid.Cells.Views.Cell rightAlign = null;

                    alternate = new SourceGrid.Cells.Views.Cell();
                    rightAlign = new SourceGrid.Cells.Views.Cell();
                    rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;

                    if (i % 2 != 0)
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
                        rightAlign.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
                    }

                    DataRow dr = dtData.Rows[i - 1];
                    grdEmployeePF[i, (int)GridColumns.Particular] = new SourceGrid.Cells.Cell(dr["Particulars"].ToString());
                    grdEmployeePF[i, (int)GridColumns.Particular].View = alternate;

                    decimal amtBaisakh = Convert.ToDecimal(dr["Baishak"]);
                    dr["Baishak"] = amtBaisakh.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Baishak] = new SourceGrid.Cells.Cell(amtBaisakh.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Baishak].View = rightAlign;
                    // total_Baisakh += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Baishak].Value);

                    decimal amtJestha = Convert.ToDecimal(dr["Jestha"]);
                    dr["Jestha"] = amtJestha.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Jestha] = new SourceGrid.Cells.Cell(amtJestha.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Jestha].View = rightAlign;
                    //total_Jesth += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Jestha].Value);

                    decimal amtAsar = Convert.ToDecimal(dr["Ashar"]);
                    dr["Ashar"] = amtAsar.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Ashar] = new SourceGrid.Cells.Cell(amtAsar.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Ashar].View = rightAlign;
                    //total_Asar += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Ashar].Value);

                    decimal amtShrawan = Convert.ToDecimal(dr["Shrawan"]);
                    dr["Shrawan"] = amtShrawan.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Sharwan] = new SourceGrid.Cells.Cell(amtShrawan.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Sharwan].View = rightAlign;
                    //total_Shrawan += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Sharwan].Value);

                    decimal amtBhadra = Convert.ToDecimal(dr["Bhadra"]);
                    dr["Bhadra"] = amtBhadra.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Bhardra] = new SourceGrid.Cells.Cell(amtBhadra.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Bhardra].View = rightAlign;
                    //total_Bhadra += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Bhardra].Value);

                    decimal amtAshwin = Convert.ToDecimal(dr["Ashwin"]);
                    dr["Ashwin"] = amtAshwin.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Ashoj] = new SourceGrid.Cells.Cell(amtAshwin.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Ashoj].View = rightAlign;
                    //total_Asoj += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Ashoj].Value);

                    decimal amtKartik = Convert.ToDecimal(dr["Kartik"]);
                    dr["Kartik"] = amtKartik.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Kartik] = new SourceGrid.Cells.Cell(amtKartik.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Kartik].View = rightAlign;
                    //total_Kartik += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Kartik].Value);

                    decimal amtMangsir = Convert.ToDecimal(dr["Mansir"]);
                    dr["Mansir"] = amtMangsir.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Mansir] = new SourceGrid.Cells.Cell(amtMangsir.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Mansir].View = rightAlign;
                    //total_Mangsir += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Mansir].Value);


                    decimal amtPoush = Convert.ToDecimal(dr["Poush"]);
                    dr["Poush"] = amtPoush.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Poush] = new SourceGrid.Cells.Cell(amtPoush.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Poush].View = rightAlign;
                    //total_Poush += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Poush].Value);

                    decimal amtMagh = Convert.ToDecimal(dr["Magh"]);
                    dr["Magh"] = amtMagh.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Magh] = new SourceGrid.Cells.Cell(amtMagh.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Magh].View = rightAlign;
                    //total_Magh += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Magh].Value);

                    decimal amtFalgun = Convert.ToDecimal(dr["Falgun"]);
                    dr["Falgun"] = amtFalgun.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Falgun] = new SourceGrid.Cells.Cell(amtFalgun.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Falgun].View = rightAlign;
                    //total_Falgun += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Falgun].Value);

                    decimal amtChaitra = Convert.ToDecimal(dr["Chaitra"]);
                    dr["Chaitra"] = amtChaitra.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    grdEmployeePF[i, (int)GridColumns.Chaitra] = new SourceGrid.Cells.Cell(amtChaitra.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdEmployeePF[i, (int)GridColumns.Chaitra].View = rightAlign;
                    //total_Chaitra += Convert.ToDecimal(grdEmployeePF[i, (int)GridColumns.Chaitra].Value);

                    grdEmployeePF.Rows.SetHeight(i, 22);

                }
                // 
                DataRow dr1 = dtData.Rows[i - 1];
                DataRow drMonthName = dtData.Rows[i];

                for (int c = 1; c <= 12; c++)
                {
                    int monthID = Convert.ToInt32(dr1[c - 1]);
                    string monthName = drMonthName[c - 1].ToString();
                    string colName = grdEmployeePF[0, c].Value.ToString();

                    // when frommonth is less than to month
                    if (m_AnnualReportSettings.ToMonth < m_AnnualReportSettings.FromMonth)
                    {
                        if (monthID > m_AnnualReportSettings.ToMonth && monthID < m_AnnualReportSettings.FromMonth)
                        {
                            for (int j = 0; j <= 12; j++)
                            {
                                if (monthName == grdEmployeePF[0, j].Value.ToString())
                                {
                                    grdEmployeePF.Columns[j].Visible = false;
                                    break;
                                }
                            }
                        } 
                    }
                    else if (monthID >= m_AnnualReportSettings.FromMonth && monthID <= m_AnnualReportSettings.ToMonth)
                    {

                    }
                    else
                    {
                        for (int j = 0; j <= 12; j++)
                        {
                            if (monthName == grdEmployeePF[0, j].Value.ToString())
                            {
                                grdEmployeePF.Columns[j].Visible = false;
                                break;
                            }
                        }
                    }

                }

                SourceGrid.Cells.Views.Cell tAlternateColor = new SourceGrid.Cells.Views.Cell();
                tAlternateColor.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(ColorTranslator.FromHtml("#e6f7ff"));
                tAlternateColor.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;

                // for total
                #region not required to calculate total
                //grdEmployeePF[i, (int)GridColumns.Particular] = new SourceGrid.Cells.Cell("Total");

                //grdEmployeePF[i, (int)GridColumns.Baishak] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Baisakh).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Jestha] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Jesth).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Ashar] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Asar).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Sharwan] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Shrawan).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Bhardra] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Bhadra).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Ashoj] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Asoj).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Kartik] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Kartik).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Mansir] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Mangsir).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Poush] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Poush).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Magh] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Magh).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Falgun] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Falgun).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Chaitra] = new SourceGrid.Cells.Cell(Convert.ToDecimal(total_Chaitra).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                //grdEmployeePF[i, (int)GridColumns.Particular].View =
                //grdEmployeePF[i, (int)GridColumns.Baishak].View =
                //grdEmployeePF[i, (int)GridColumns.Jestha].View =
                //grdEmployeePF[i, (int)GridColumns.Ashar].View =
                //grdEmployeePF[i, (int)GridColumns.Sharwan].View =
                //grdEmployeePF[i, (int)GridColumns.Bhardra].View =
                //grdEmployeePF[i, (int)GridColumns.Ashoj].View =
                //grdEmployeePF[i, (int)GridColumns.Kartik].View =
                //grdEmployeePF[i, (int)GridColumns.Mansir].View =
                //grdEmployeePF[i, (int)GridColumns.Poush].View =
                //grdEmployeePF[i, (int)GridColumns.Magh].View =
                //grdEmployeePF[i, (int)GridColumns.Falgun].View =
                //grdEmployeePF[i, (int)GridColumns.Chaitra].View =

                //tAlternateColor;

                #endregion

                grdEmployeePF.AutoStretchColumnsToFitWidth = true;

                grdEmployeePF.Columns.StretchToFit();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public DataTable dtData = null;
        //public int m_EmployeeID =0, m_Year =0;
        public void CreateData()
        {
            try
            {
                DataTable DT = BusinessLogic.HRM.Employee.GetAnnualEmpReport(m_AnnualReportSettings.EmployeeID, m_AnnualReportSettings.Year);
                dtData = InitColumns(dtData);
                m_DesignationName = DT.Rows[0]["Designation"].ToString();

                for (int count = 0; count <= DT.Columns.Count - 2; count++)
                {
                    //int monthID = Convert.ToInt32(DT.Rows[count]["monthID"]);
                    DataColumn dCol = DT.Columns[count];
                    DataRow dr = dtData.NewRow();
                    dr["Particulars"] = dCol.ColumnName;
                    dtData.Rows.Add(dr);
                    //dtData.Rows.InsertAt(dr, monthID);

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        int monthID = Convert.ToInt32(DT.Rows[i]["monthID"]);

                        dtData.Rows[count][i] = DT.Rows[i][count];
                        //monthID++;
                    }
                }

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }


    }
}
