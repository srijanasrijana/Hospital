using BusinessLogic;
using BusinessLogic.HRM;
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
    public partial class frmEmployeeAdvance : Form
    {
        EmployeeAdvanceSettings m_ReportSettings = null;
        DataTable dtAdvanceReport = null;
        private Model.dsEmployeeAdvance dsEmployeeAdv = new Model.dsEmployeeAdvance();
        DataTable dtTemp;
        private string FileName = "";
        private int prntDirect = 0;
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
        public frmEmployeeAdvance()
        {
            InitializeComponent();
        }
        public frmEmployeeAdvance(EmployeeAdvanceSettings EAS)
        {
            m_ReportSettings = EAS;
            InitializeComponent();
        }
        private void frmEmployeeAdvance_Load(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void LoadForm()
        {
            try
            {
                dtAdvanceReport = Employee.GetAdvanceReport(m_ReportSettings.MonthID, m_ReportSettings.Year);

                lblMonth.Text = m_ReportSettings.Month;
                lblYear.Text = m_ReportSettings.Year.ToString();
                lblAsOnDate.Text = Date.ToSystem(Convert.ToDateTime(Date.GetServerDate()));
                FormatData();
                LoadGrid();

                this.WindowState = FormWindowState.Minimized;
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        public void FormatData()
        {
            CreateColumn();
            for (int i = 0; i < dtAdvanceReport.Rows.Count; i++)
            {
                dtTemp.Rows.Add(dtAdvanceReport.Rows[i]["EmployeeID"].ToString(), 
                                dtAdvanceReport.Rows[i]["SN"].ToString(), 
                                dtAdvanceReport.Rows[i]["StaffCode"].ToString(), 
                                dtAdvanceReport.Rows[i]["StaffName"].ToString(), 
                                dtAdvanceReport.Rows[i]["AdvTitle"].ToString(), 
                                dtAdvanceReport.Rows[i]["TakenDate"].ToString(), 
                                Convert.ToDecimal(dtAdvanceReport.Rows[i]["Installment"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 
                                Convert.ToDecimal(dtAdvanceReport.Rows[i]["TotalAmt"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))
                                );
            }
        }

        private void CreateColumn()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("EmployeeID", typeof(string));
            dtTemp.Columns.Add("SN", typeof(string));
            dtTemp.Columns.Add("StaffCode", typeof(string));
            dtTemp.Columns.Add("StaffName", typeof(string));
            dtTemp.Columns.Add("AdvTitle", typeof(string));
            dtTemp.Columns.Add("TakenDate", typeof(string));
            dtTemp.Columns.Add("Installment", typeof(decimal));
            dtTemp.Columns.Add("TotalAmt", typeof(decimal));

        }

        private void LoadGrid()
        {
            DataView mView = new DataView(dtTemp);
            mView.AllowDelete = false;
            mView.AllowNew = false;
            dgLoan.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
            dgLoan.FixedRows = 1;
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

            CreateDataGridColumns(dgLoan.Columns, bd);
            dgLoan.DataSource = bd;
        }

        private void CreateDataGridColumns(SourceGrid.DataGridColumns Columns, DevAge.ComponentModel.IBoundList bd)
        {

            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.Font = new Font("Arial", 10, FontStyle.Bold);
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            SourceGrid.Cells.Views.Cell cellView = new SourceGrid.Cells.Views.Cell();
            cellView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
            // cellView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            cellView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dgLoan.Columns.Add("EmployeeID", "EmpID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.Visible = false;

            gridColumn = dgLoan.Columns.Add("SN", "S.N.", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            //gridColumn.DataCell.AddController(dblClick);
            gridColumn.Width = 45;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgLoan.Columns.Add("StaffCode", "Code", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.Width = 150;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            gridColumn = dgLoan.Columns.Add("StaffName", "Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 300;

            gridColumn = dgLoan.Columns.Add("AdvTitle", "Advance Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 200;

            gridColumn = dgLoan.Columns.Add("TakenDate", "Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 100;

            gridColumn = dgLoan.Columns.Add("Installment", "Installment", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 100;

            gridColumn = dgLoan.Columns.Add("TotalAmt", "Total Amt", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 100;

            dgLoan.AutoStretchColumnsToFitWidth = true;

            foreach (SourceGrid.DataGridColumn col in Columns)
            {
                SourceGrid.Conditions.ICondition condition =
                    SourceGrid.Conditions.ConditionBuilder.AlternateView(col.DataCell.View,
                                                                         Global.Grid_Color, Color.Black);
                col.Conditions.Add(condition);
            }

        }


        #region Related to printing
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

                DataTable dtAdvanceCopy = null;
                dtAdvanceCopy = dtTemp.Copy();

                dsEmployeeAdv.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
                ReportClass rpt;
                rpt = new rptEmployeeAdvance();
                //Fill the logo on the report
                Misc.WriteLogo(dsEmployeeAdv, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
                rpt.SetDataSource(dsEmployeeAdv);
                try
                {
                    dsEmployeeAdv.Tables.Remove("tblAdvance");

                }
                catch
                {

                }
                System.Data.DataView view = new System.Data.DataView(dtAdvanceCopy);

                DataTable selected = view.ToTable("tblAdvance", false, "EmployeeID", "StaffCode", "StaffName", "AdvTitle", "TakenDate", "Installment", "TotalAmt");

                //dsStudent.Tables.Remove("tblStudent");
                dsEmployeeAdv.Tables.Add(selected);

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
                pdvReport_Head.Value = "Detailed Employee Report";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                pdvReport_Date.Value = Date.ToSystem(Convert.ToDateTime(Date.GetServerDate()));
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                pdvMonth.Value = m_ReportSettings.Month;
                pvCollection.Clear();
                pvCollection.Add(pdvMonth);
                rpt.DataDefinition.ParameterFields["paramMonth"].ApplyCurrentValues(pvCollection);

                pdvYear.Value = m_ReportSettings.Year;
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

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            prntDirect = 0;
            PrintPreviewCR(PrintType.CrystalReport);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            prntDirect = 1;
            PrintPreviewCR(PrintType.CrystalReport);
        }
        #endregion

        #region related to export menu
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

        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadForm();
            this.Width = this.Width - 1;
            this.Width = this.Width + 1;
        }
    }
}
