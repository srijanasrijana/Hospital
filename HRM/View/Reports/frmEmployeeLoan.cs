using BusinessLogic.HRM.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using Common;
using System.Threading;
using CrystalDecisions.CrystalReports.Engine;
using HRM.Reports;
using CrystalDecisions.Shared;

namespace HRM.View.Reports
{
    public partial class frmEmployeeLoan : Form
    {
        EmployeeLoanSettings m_ELS = new EmployeeLoanSettings();
        DataTable dtTemp;
        DataTable dtEmp;
        private int prntDirect = 0;
        private Model.dsEmployeeLoan dsLoan = new Model.dsEmployeeLoan();
        private string FileName = "";
        ContextMenu Menu_Export;
        SourceGrid.Cells.Views.Cell alternate = null;
        SourceGrid.Cells.Views.Cell rightAlign = null;

        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        public frmEmployeeLoan(EmployeeLoanSettings els)
        {
            InitializeComponent();
            m_ELS = els;
        }

        private void frmEmployeeLoan_Load(object sender, EventArgs e)
        {
            try
            {
                LoanForm();
                this.WindowState = FormWindowState.Minimized;
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        string InstallmentType = "";
        private void LoanForm()
        {
            try
            {
                InstallmentType = Hrm.GetLoanType(m_ELS.LoanID);
                if (InstallmentType == "Fix Installment")
                {
                    isFixed = true;
                }
                else
                {
                    isFixed = false;
                }

                lblLoan.Text = "Loan : " + m_ELS.Loan;
                lblMonth.Text = "Month : " + m_ELS.Month;
                lblYear.Text = "Year : " + m_ELS.Year;
                lblAsOnDate.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(Date.GetServerDate()));
                DisplayTransaction(false);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void DisplayTransaction(bool IsCrystalReport)
        {
            try
            {
                //TransactRowsCount = 1;
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
                ProgressForm.UpdateProgress(40, "Getting Student Data...");
                if (!IsCrystalReport)//just for  Grid not for Crystal report
                    dg_Orientation();

                FillGrid(IsCrystalReport);
                
                ProgressForm.UpdateProgress(100, "Preparing report for display...");

                if (ProgressForm.InvokeRequired)
                    ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void dg_Orientation()
        {
            //dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            //dblClick.DoubleClick += new EventHandler(dg_Tranasaction_DoubleClick);

            ////Let the whole row to be selected
            dgLoan.SelectionMode = SourceGrid.GridSelectionMode.Row;

            ////Disable multiple selection
            dgLoan.Selection.EnableMultiSelection = false;

        }

        public bool isFixed = false;
        private void FillGrid(bool isCrystal)
        {
            dtEmp = BusinessLogic.HRM.Employee.GetLoanReport(m_ELS.LoanID, m_ELS.MonthID, m_ELS.Year,ref isFixed);

            //grdTransaction.Visible = false;
            if (isFixed)
            {
                CreateColumn();

                if (dtEmp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtEmp.Rows.Count; i++)
                    {
                        dtTemp.Rows.Add(dtEmp.Rows[i]["EmployeeID"].ToString(), 
                            i + 1, 
                            dtEmp.Rows[i]["StaffCode"].ToString(), 
                            dtEmp.Rows[i]["StaffName"].ToString(), 
                            Convert.ToDecimal(dtEmp.Rows[i]["LoanMthPremium"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 
                            dtEmp.Rows[i]["DesignationName"].ToString(), 
                            dtEmp.Rows[i]["PFNumber"].ToString());
                        //dtTemp.Rows.Add(dtStudent.Rows[i]["StudentID"].ToString(), i + 1, dtStudent.Rows[i]["StudentCode"].ToString(), dtStudent.Rows[i]["StudentName"].ToString(), dtStudent.Rows[i]["RollNo"].ToString(), dtStudent.Rows[i]["FatherName"].ToString(), dtStudent.Rows[i]["DOBDefaultDate"].ToString(), dtStudent.Rows[i]["PermAddress"].ToString(), dtStudent.Rows[i]["Gender"].ToString(), dtStudent.Rows[i]["MobileNo"].ToString(), "", dtStudent.Rows[i]["Remarks"].ToString());
                    }
                }

                //#region Bind Data to Datagrid
                //if (!isCrystal)
                //{
                //    DataView mView = new DataView(dtTemp);
                //    mView.AllowDelete = false;
                //    mView.AllowNew = false;
                //    dgLoan.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
                //    dgLoan.FixedRows = 1;
                //    DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

                //    CreateDataGridColumns(dgLoan.Columns, bd);
                //    dgLoan.DataSource = bd;
                //}
                //#endregion
                
                LoadGridFixed(dtTemp);
            }
            else
            {

                CreateColumnForFixed();

                //dtEmp = BusinessLogic.HRM.Employee.GetLoanReport(m_ELS.LoanID, m_ELS.MonthID, m_ELS.Year, isFixed);

                if (dtEmp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtEmp.Rows.Count; i++)
                    {
                        dtTemp.Rows.Add(dtEmp.Rows[i]["EmployeeID"].ToString(), 
                            i + 1, 
                            dtEmp.Rows[i]["StaffCode"].ToString(), 
                            dtEmp.Rows[i]["StaffName"].ToString(), 
                            dtEmp.Rows[i]["DesignationName"].ToString(),
                            Convert.ToDecimal(dtEmp.Rows[i]["LoanMthInstallment"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 
                            dtEmp.Rows[i]["PFNumber"].ToString(),
                            //dtEmp.Rows[i]["LoanPrincipal"].ToString(),
                            dtEmp.Rows[i]["LoanMthPremium"].ToString(),
                            dtEmp.Rows[i]["LoanMthInterest"].ToString(),
                            dtEmp.Rows[i]["Total"].ToString()
                            );

                        //dtTemp.Rows.Add(dtStudent.Rows[i]["StudentID"].ToString(), i + 1, dtStudent.Rows[i]["StudentCode"].ToString(), dtStudent.Rows[i]["StudentName"].ToString(), dtStudent.Rows[i]["RollNo"].ToString(), dtStudent.Rows[i]["FatherName"].ToString(), dtStudent.Rows[i]["DOBDefaultDate"].ToString(), dtStudent.Rows[i]["PermAddress"].ToString(), dtStudent.Rows[i]["Gender"].ToString(), dtStudent.Rows[i]["MobileNo"].ToString(), "", dtStudent.Rows[i]["Remarks"].ToString());
                    }
                }
              
                LoadGrid(dtTemp);
            }

        }

        public void CreateColumnForFixed()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("EmpID", typeof(string));
            dtTemp.Columns.Add("S.N", typeof(string));
            dtTemp.Columns.Add("Code", typeof(string));
            dtTemp.Columns.Add("Name", typeof(string));
            dtTemp.Columns.Add("Designation", typeof(string));
            dtTemp.Columns.Add("Loan", typeof(decimal));
            dtTemp.Columns.Add("PFNo", typeof(string));
            dtTemp.Columns.Add("Principal", typeof(decimal));
            dtTemp.Columns.Add("Interest", typeof(decimal));
            dtTemp.Columns.Add("Total", typeof(decimal));
            //dtTemp.Columns.Add("LoanMthPremium", typeof(decimal));
            //dtTemp.Columns.Add("LoanMthPremium", typeof(decimal));

        }
        private void CreateColumn()
        {
            dtTemp = new DataTable();
            //dtTemp.Columns.Add("EmployeeID", typeof(string));
            //dtTemp.Columns.Add("S.N", typeof(string));
            //dtTemp.Columns.Add("Code", typeof(string));
            //dtTemp.Columns.Add("Name", typeof(string));
            //dtTemp.Columns.Add("Loan", typeof(decimal));

            dtTemp.Columns.Add("EmpID", typeof(string));
            dtTemp.Columns.Add("S.N", typeof(string));
            dtTemp.Columns.Add("Code", typeof(string));
            dtTemp.Columns.Add("Name", typeof(string));
            dtTemp.Columns.Add("Loan", typeof(string));
            dtTemp.Columns.Add("Designation", typeof(string));
            dtTemp.Columns.Add("PFNo", typeof(string));
        }

        public void WriteGridHeader()
        {
            grdLoan[0, 0] = new MyHeader("");
            grdLoan[0, 0].Column.Visible = false;

            grdLoan[0, 1] = new MyHeader("SN");
            grdLoan[0, 1].RowSpan = 2;
            grdLoan[0, 1].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdLoan[0, 2] = new MyHeader("Employee ID");
            grdLoan[0, 2].RowSpan = 2;
            grdLoan[0, 2].Column.Visible = false;
           
            grdLoan[0, 3] = new MyHeader("Designation");
            grdLoan[0, 3].RowSpan = 2;
            grdLoan[0, 3].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdLoan[0, 4] = new MyHeader("Name");
            grdLoan[0, 4].RowSpan = 2;
            grdLoan[0, 4].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdLoan[0, 5] = new MyHeader("EPF No.");
            grdLoan[0, 5].RowSpan = 2;
            grdLoan[0, 5].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdLoan[0, 6] = new MyHeader("Loan Details");
            grdLoan[0, 6].ColumnSpan = 3;
            grdLoan[0, 6].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdLoan[1, 6] = new MyHeader("Principal");
            grdLoan[1, 6].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdLoan[1, 7] = new MyHeader("Interest");
            grdLoan[1, 7].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdLoan[1, 8] = new MyHeader("Total");
            grdLoan[1, 8].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdLoan[0, 1].Column.Width = 30;
            grdLoan[0, 3].Column.Width = 40;
            grdLoan[0, 4].Column.Width = 120;
            grdLoan[0, 5].Column.Width = 150;
            grdLoan[0, 6].Column.Width = 100;
            grdLoan[0, 7].Column.Width = 80;
            grdLoan[0, 8].Column.Width = 30;

            grdLoan.FixedRows = 2;

        }

        private void LoadGrid(DataTable dtv)
        {
            try
            {
                 int rowsCount = 0;
                rowsCount = dtv.Rows.Count;
                grdLoan.Rows.Clear();

              
                grdLoan.SelectionMode = SourceGrid.GridSelectionMode.Row;
                grdLoan.Redim(rowsCount + 3, 9);
                
                WriteGridHeader();

                decimal totalPrincipal = 0;
                decimal totalInterest = 0;
                decimal total = 0;
                decimal pr = 0;
                decimal intr = 0;
                decimal tot = 0;

                for (int i = 2; i <= rowsCount +1; i++)
                {
                    alternate = new SourceGrid.Cells.Views.Cell();
                    rightAlign = new SourceGrid.Cells.Views.Cell();                          // right alignment for Gross_Amount
                    rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;

                    if (i % 2 != 0)
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
                        rightAlign.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
                    }

                    DataRow dr = dtv.Rows[i - 2];

                    grdLoan[i, 1] = new SourceGrid.Cells.Cell(dr["S.N"].ToString());
                    grdLoan[i, 1].View = rightAlign;
                    
                    grdLoan[i, 3] = new SourceGrid.Cells.Cell(dr["Designation"].ToString());
                    grdLoan[i, 3].View = alternate;

                    grdLoan[i, 4] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                    grdLoan[i, 4].View = alternate;

                    grdLoan[i, 5] = new SourceGrid.Cells.Cell(dr["PFNo"].ToString());
                    grdLoan[i, 5].View = rightAlign;

                    pr = Convert.ToDecimal(dr["Principal"]);
                    totalPrincipal += pr;
                    grdLoan[i, 6] = new SourceGrid.Cells.Cell(pr.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdLoan[i, 6].View = rightAlign;

                    intr = Convert.ToDecimal(dr["Interest"]);
                    totalInterest += intr;
                    grdLoan[i, 7] = new SourceGrid.Cells.Cell(intr.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdLoan[i, 7].View = rightAlign;

                    tot = Convert.ToDecimal(dr["Total"]);
                    total += tot;
                    grdLoan[i, 8] = new SourceGrid.Cells.Cell(tot.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdLoan[i, 8].View = rightAlign;
                }

                SourceGrid.Cells.Views.Cell centralAlign = new SourceGrid.Cells.Views.Cell();
                centralAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomCenter;
                centralAlign.Font = new Font("Arial", 9, FontStyle.Bold);

                rightAlign = new SourceGrid.Cells.Views.Cell();                          // right alignment for Gross_Amount
                rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;
                rightAlign.Font = new Font("Arial", 9, FontStyle.Bold);

                grdLoan[rowsCount + 2, 0] = new SourceGrid.Cells.Cell("Total");
                grdLoan[rowsCount + 2, 0].ColumnSpan = 6;
                grdLoan[rowsCount + 2, 0].View = centralAlign;

                grdLoan[rowsCount + 2, 6] = new SourceGrid.Cells.Cell(totalPrincipal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdLoan[rowsCount + 2, 6].View = rightAlign;

                grdLoan[rowsCount + 2, 7] = new SourceGrid.Cells.Cell(totalInterest.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdLoan[rowsCount + 2, 7].View = rightAlign;

                grdLoan[rowsCount + 2, 8] = new SourceGrid.Cells.Cell(total.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdLoan[rowsCount + 2, 8].View = rightAlign;

                grdLoan.AutoStretchColumnsToFitWidth = true;

                grdLoan.Columns.StretchToFit();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void WriteGridHeaderFixed()
        {
            grdLoan[0, 0] = new MyHeader("");
            grdLoan[0, 0].Column.Visible = false;

            grdLoan[0, 1] = new MyHeader("SN");
            grdLoan[0, 1].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdLoan[0, 2] = new MyHeader("Employee ID");
            grdLoan[0, 2].Column.Visible = false;

            grdLoan[0, 3] = new MyHeader("Designation");
            grdLoan[0, 3].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdLoan[0, 4] = new MyHeader("Name");
            grdLoan[0, 4].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdLoan[0, 5] = new MyHeader("EPF No.");
            grdLoan[0, 5].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdLoan[0, 6] = new MyHeader("Deduced Loan");
            grdLoan[0, 6].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdLoan[0, 0].Column.Width = 30;
            grdLoan[0, 1].Column.Width = 30;
            grdLoan[0, 3].Column.Width = 40;
            grdLoan[0, 4].Column.Width = 120;
            grdLoan[0, 5].Column.Width = 150;
            grdLoan[0, 6].Column.Width = 150;

            grdLoan.FixedRows = 1;
        }

        private void LoadGridFixed(DataTable dtv)
        {
            try
            {
                int rowsCount = 0;
                rowsCount = dtv.Rows.Count;
                grdLoan.Rows.Clear();

                grdLoan.SelectionMode = SourceGrid.GridSelectionMode.Row;
                grdLoan.Redim(rowsCount + 3, 7);

                WriteGridHeaderFixed();
                decimal totalLoan = 0;
                decimal loan = 0;
                for (int i = 1; i <= rowsCount; i++)
                {
                    alternate = new SourceGrid.Cells.Views.Cell();
                    rightAlign = new SourceGrid.Cells.Views.Cell();                          // right alignment for Gross_Amount
                    rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;

                    if (i % 2 != 0)
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        rightAlign.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }

                    DataRow dr = dtv.Rows[i - 1];
                    grdLoan[i, 0] = new SourceGrid.Cells.Cell(dr["EmpID"].ToString());
                    grdLoan[i, 0].View = alternate;

                    grdLoan[i, 1] = new SourceGrid.Cells.Cell(dr["S.N"].ToString());
                    grdLoan[i, 1].View = rightAlign;
                   
                    grdLoan[i, 3] = new SourceGrid.Cells.Cell(dr["Designation"].ToString());
                    grdLoan[i, 3].View = alternate;

                    grdLoan[i, 4] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                    grdLoan[i, 4].View = alternate;

                    grdLoan[i, 5] = new SourceGrid.Cells.Cell(dr["PFNo"].ToString());
                    grdLoan[i, 5].View = rightAlign;

                    loan = Convert.ToDecimal(dr["Loan"]);
                    totalLoan += loan;
                    grdLoan[i, 6] = new SourceGrid.Cells.Cell(loan.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdLoan[i, 6].View = rightAlign;

                }
                SourceGrid.Cells.Views.Cell centralAlign = new SourceGrid.Cells.Views.Cell();
                centralAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomCenter;
                centralAlign.Font = new Font("Arial", 9, FontStyle.Bold);

                rightAlign = new SourceGrid.Cells.Views.Cell();                          // right alignment for Gross_Amount
                rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;
                rightAlign.Font = new Font("Arial", 9, FontStyle.Bold);

                grdLoan[rowsCount + 1, 0] = new SourceGrid.Cells.Cell("Total");
                grdLoan[rowsCount + 1, 0].ColumnSpan = 6;
                grdLoan[rowsCount + 1, 0].View = centralAlign;

                grdLoan[rowsCount + 1, 6] = new SourceGrid.Cells.Cell(totalLoan.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                grdLoan[rowsCount + 1, 6].View = rightAlign;

                
                grdLoan.AutoStretchColumnsToFitWidth = true;

                grdLoan.Columns.StretchToFit();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        #region data  grid binding not used
        //private void CreateDataGridColumns(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
        //{
        //    SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
        //    viewColumnHeader.Font = new Font("Arial", 10, FontStyle.Bold);
        //    viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

        //    SourceGrid.Cells.Views.Cell cellView = new SourceGrid.Cells.Views.Cell();
        //    cellView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

        //    SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
        //    viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
        //    cellView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

        //    SourceGrid.DataGridColumn gridColumn;

        //    gridColumn = dgLoan.Columns.Add("EmpID", "EmpID", new SourceGrid.Cells.DataGrid.Cell());
        //    gridColumn.Visible = false;
        //    gridColumn.Width = 30;

        //    gridColumn = dgLoan.Columns.Add("S.N", "S.N.", new SourceGrid.Cells.DataGrid.Cell());
        //    gridColumn.HeaderCell.View = viewColumnHeader;
        //    gridColumn.DataCell.View = cellView;
        //    gridColumn.Width = 45;
        //    gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

        //    gridColumn = dgLoan.Columns.Add("Code", "Code", new SourceGrid.Cells.DataGrid.Cell());
        //    gridColumn.HeaderCell.View = viewColumnHeader;
        //    gridColumn.DataCell.View = cellView;
        //    gridColumn.Width = 150;
        //    gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

        //    gridColumn = dgLoan.Columns.Add("Name", "Name", new SourceGrid.Cells.DataGrid.Cell());
        //    gridColumn.HeaderCell.View = viewColumnHeader;
        //    gridColumn.DataCell.View = cellView;
        //    gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
        //    gridColumn.Width = 300;

        //    gridColumn = dgLoan.Columns.Add("Designation", "Designation", new SourceGrid.Cells.DataGrid.Cell());
        //    gridColumn.HeaderCell.View = viewColumnHeader;
        //    gridColumn.DataCell.View = cellView;
        //    gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
        //    gridColumn.Width = 100;

        //    gridColumn = dgLoan.Columns.Add("PFNo", "PF No.", new SourceGrid.Cells.DataGrid.Cell());
        //    gridColumn.HeaderCell.View = viewColumnHeader;
        //    gridColumn.DataCell.View = cellView;
        //    gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
        //    gridColumn.Width = 100;

        //    gridColumn = dgLoan.Columns.Add("Loan", "Loan Premium", new SourceGrid.Cells.DataGrid.Cell());
        //    gridColumn.HeaderCell.View = viewColumnHeader;
        //    gridColumn.DataCell.View = cellView;
        //    gridColumn.DataCell.View = viewNumeric;
        //    gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;
        //    gridColumn.Width = 150;

        //    dgLoan.AutoStretchColumnsToFitWidth = true;

        //    foreach (SourceGrid.DataGridColumn col in columns)
        //    {
        //        SourceGrid.Conditions.ICondition condition =
        //            SourceGrid.Conditions.ConditionBuilder.AlternateView(col.DataCell.View,
        //                                                                 Global.Grid_Color, Color.Black);
        //        col.Conditions.Add(condition);
        //    }
        //}  
        #endregion

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

                dsLoan.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
                ReportClass rpt;
                    //Fill the logo on the report
                Misc.WriteLogo(dsLoan, "tblImage");
                    //Set DataSource to be dsTrial dataset on the report
                try
                {
                    dsLoan.Tables.Remove("tblEmpLoan");

                }
                catch
                {

                }

                System.Data.DataView view = new System.Data.DataView(dtTemp);

                DataTable selected = null;

                if (isFixed)
                {
                    rpt = new rptEmployeeLoan();
                    selected = view.ToTable("tblEmpLoan", false, "Code", "Name", "Designation", "PFNo", "Loan");
                }
                else
                {
                    rpt = new rptEmployeeLoanDetail();
                    selected = view.ToTable("tblEmpLoan", false, "Code", "Name", "Designation", "PFNo", "Principal", "Interest", "Total");
                }

                //dsStudent.Tables.Remove("tblStudent");
                dsLoan.Tables.Add(selected);

                rpt.SetDataSource(dsLoan);
              

                //DisplayTransaction(true);

                //Provide values to the parameters on the report
                CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvPaySlip_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvLoan = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvLoanType = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvMonth = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvYear = new CrystalDecisions.Shared.ParameterDiscreteValue();
                

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


                }
                else //if user is not root, take information from tblUserPreference
                {
                    try
                    {
                        string companyname = UserPreference.GetValue("COMPANY_NAME", uid);
                        pdvCompany_Name.Value = companyname;
                        string companyaddress = UserPreference.GetValue("COMPANY_ADDRESS", uid);
                        string companycity = UserPreference.GetValue("COMPANY_CITY", uid);
                        pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                     
                    }
                    catch (Exception)
                    {
                    }
                    pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Name);
                    rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                    pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Address);
                    rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                }
                pdvReport_Head.Value = "Loan Report";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                pdvReport_Date.Value = "As On Date: " + Date.ToSystem(Convert.ToDateTime(Date.GetServerDate()));
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                pdvPaySlip_Date.Value = " ";// +Date.ToSystem(Convert.ToDateTime(Date.GetServerDate()));
                pvCollection.Clear();
                pvCollection.Add(pdvPaySlip_Date);
                rpt.DataDefinition.ParameterFields["PaySlip_Date"].ApplyCurrentValues(pvCollection);

                if (isFixed)
                {
                    pdvReport_Head.Value = grdLoan[dtTemp.Rows.Count + 1, 6].Value.ToString();
                    pvCollection.Clear();
                    pvCollection.Add(pdvReport_Head);
                    rpt.DataDefinition.ParameterFields["paramLoanTotal"].ApplyCurrentValues(pvCollection);
                    
                }

                pdvLoan.Value = "Loan : " + m_ELS.Loan;
                pvCollection.Clear();
                pvCollection.Add(pdvLoan);
                rpt.DataDefinition.ParameterFields["paramLoan"].ApplyCurrentValues(pvCollection);

                pdvLoan.Value = "Loan Type : " + InstallmentType;
                pvCollection.Clear();
                pvCollection.Add(pdvLoan);
                rpt.DataDefinition.ParameterFields["LoanType"].ApplyCurrentValues(pvCollection);

                pdvMonth.Value = "Month : " + m_ELS.Month;
                pvCollection.Clear();
                pvCollection.Add(pdvMonth);
                rpt.DataDefinition.ParameterFields["paramMonth"].ApplyCurrentValues(pvCollection);

                pdvYear.Value = "Year : " + m_ELS.Year;
                pvCollection.Clear();
                pvCollection.Add(pdvYear);
                rpt.DataDefinition.ParameterFields["paramYear"].ApplyCurrentValues(pvCollection);

                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;

                //Finally, show the report form
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
                frm.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            prntDirect = 1;
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoanForm();
            this.Width = this.Width - 1;
            this.Width = this.Width + 1;
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
                    btnPrintPreview_Click(sender, e);
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
                    btnPrintPreview_Click(sender, e);
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
                    btnPrintPreview_Click(sender, e);
                    break;

            }
        }

        private void grdLoan_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
    class MyHeader : SourceGrid.Cells.ColumnHeader
    {
        public MyHeader(object value)
            : base(value)
        {
            //1 Header Row
            SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
            //DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
            //backHeader.BackColor = Color.Silver;
            //ackHeader.Border = DevAge.Drawing.RectangleBorder.RectangleBlack1Width;
            //view.Background = backHeader;
            view.Font = new Font("Arial", 9, FontStyle.Bold);
            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            View = view;
            AutomaticSortEnabled = false;
        }
    }

}
