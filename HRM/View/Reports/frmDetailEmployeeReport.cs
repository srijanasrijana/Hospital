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
using BusinessLogic.HRM;
using Common;
using System.Threading;
using CrystalDecisions.CrystalReports.Engine;
using HRM.Reports;
using DateManager;
using CrystalDecisions.Shared;
using HRM.Model;

namespace HRM.View.Reports
{
    public partial class frmDetailEmployeeReport : Form
    {
        DataTable dtReport = null;
        DetailEmployeeReportSettings m_ReportSettings = null;
        private Model.dsDetailEmployeeReport dsEmployee = new Model.dsDetailEmployeeReport();
        //DataTable dtTemp;
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
        public frmDetailEmployeeReport()
        {
            InitializeComponent();
        }

        public frmDetailEmployeeReport(DetailEmployeeReportSettings ReportSettings)
        {
            m_ReportSettings = ReportSettings;
            InitializeComponent();
        }
        private void frmDetailEmployeeReport_Load(object sender, EventArgs e)
        {
            ShowReport();
        }

        public void ShowReport()
        {
            try
            {
                lblAsOnDate.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(Date.GetServerDate()));

                lblDepartment.Text = m_ReportSettings.Department;
                lblDesignation.Text = m_ReportSettings.Designation;
                lblFaculty.Text = m_ReportSettings.Faculty;
                lblJobType.Text = m_ReportSettings.JobType;
                lblLevel.Text = m_ReportSettings.Level;
                lblStatus.Text = m_ReportSettings.Status;

                dtReport = Employee.GetDetailedEmployeeReport(m_ReportSettings);
                if (m_ReportSettings.RptType == "Details")
                    LoadGrid();
                else if (m_ReportSettings.RptType == "Patreon")
                {
                    lblReportTitle.Text = "Employee Patreon Report";
                    LoadGridPatreon();
                }
                this.WindowState = FormWindowState.Minimized;
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        #region Related to grid

        public void LoadGrid()
        {
            #region Bind Data to Datagrid

            DataView mView = new DataView(dtReport);
            mView.AllowDelete = false;
            mView.AllowNew = false;
            dgEmployeeReport.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
            dgEmployeeReport.FixedRows = 1;
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

            CreateDataGridColumnsGnrl(dgEmployeeReport.Columns, bd);
            dgEmployeeReport.DataSource = bd;

            #endregion
        }

        public void LoadGridPatreon()
        {
            #region Bind Data to Datagrid
            FillData();
            DataView mView = new DataView(dtTemp);
            mView.AllowDelete = false;
            mView.AllowNew = false;
            dgEmployeeReport.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
            dgEmployeeReport.FixedRows = 1;
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

            CreateDataGridColumnsPatreon(dgEmployeeReport.Columns, bd);
            dgEmployeeReport.DataSource = bd;

            #endregion
        }

        DataTable dtTemp = null;
        private void CreateColumnPateron()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("EmployeeID", typeof(string));
            dtTemp.Columns.Add("CardNumber", typeof(string));
            dtTemp.Columns.Add("SurName", typeof(string));
            dtTemp.Columns.Add("FirstName", typeof(string));
            dtTemp.Columns.Add("Address", typeof(string));
            dtTemp.Columns.Add("Email", typeof(string));
            dtTemp.Columns.Add("ContactNo", typeof(string));
            dtTemp.Columns.Add("DOB", typeof(string));
        }
        public void FillData()
        {
            CreateColumnPateron();
            if (dtReport.Rows.Count > 0)
            {
                for (int i = 0; i < dtReport.Rows.Count; i++)
                {
                    dtTemp.Rows.Add(dtReport.Rows[i]["ID"].ToString(),
                         dtReport.Rows[i]["StaffCode"].ToString(),
                         dtReport.Rows[i]["FirstName"].ToString(),
                         dtReport.Rows[i]["LastName"].ToString(),
                         dtReport.Rows[i]["PermAddress"].ToString(),
                         dtReport.Rows[i]["Email"].ToString(),
                         dtReport.Rows[i]["Phone"].ToString(),
                         Convert.ToDateTime(dtReport.Rows[i]["DOB"]).ToShortDateString());
                }
            }
        }
        private void CreateDataGridColumnsGnrl(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
        {
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.Font = new Font("Arial", 10, FontStyle.Bold);
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            SourceGrid.Cells.Views.Cell cellView = new SourceGrid.Cells.Views.Cell();
            cellView.Font = new Font(LangMgr.GetFont().ToString(), 10, FontStyle.Regular);
            // cellView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            cellView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            dgEmployeeReport.FixedRows = 1;

            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dgEmployeeReport.Columns.Add("SN", "S.N.", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.Width = 45;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            gridColumn = dgEmployeeReport.Columns.Add("StaffCode", "Code", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.Width = 45;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            gridColumn = dgEmployeeReport.Columns.Add("EmployeeName", "Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 200;

            gridColumn = dgEmployeeReport.Columns.Add("Designation", "Designation", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.Width = 70;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            gridColumn = dgEmployeeReport.Columns.Add("BirthDate", "DOB", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("StartDate", "Start Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("LevelName", "Level", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 50;
            gridColumn.Visible = m_ReportSettings.IsLevel;

            gridColumn = dgEmployeeReport.Columns.Add("Faculty", "Faculty", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 100;
            gridColumn.Visible = m_ReportSettings.IsFaculty;

            gridColumn = dgEmployeeReport.Columns.Add("Department", "Department", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 100;
            gridColumn.Visible = m_ReportSettings.IsDepartment;

            gridColumn = dgEmployeeReport.Columns.Add("Designation", "Designation", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 100;
            gridColumn.Visible = m_ReportSettings.IsDesignation;

            gridColumn = dgEmployeeReport.Columns.Add("Status", "Status", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 50;
            gridColumn.Visible = m_ReportSettings.IsStatus;

            gridColumn = dgEmployeeReport.Columns.Add("JobType", "JobType", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;
            gridColumn.Visible = m_ReportSettings.IsJobType;

            gridColumn = dgEmployeeReport.Columns.Add("Phone", "Phone", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("Email", "Email", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 100;

            gridColumn = dgEmployeeReport.Columns.Add("TempAddress", "Temp. Address", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("PermAddress", "Perm. Address", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("DistrictName", "District", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("Gender", "Gender", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("Nationality", "Nationality", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("Ethnicity", "Ethnicity", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("EmployeeType", "EmployeeType", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            //gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            dgEmployeeReport.AutoStretchColumnsToFitWidth = true;

            foreach (SourceGrid.DataGridColumn col in columns)
            {
                SourceGrid.Conditions.ICondition condition =
                    SourceGrid.Conditions.ConditionBuilder.AlternateView(col.DataCell.View,
                                                                         Global.Grid_Color, Color.Black);
                col.Conditions.Add(condition);
            }
        }
        private void CreateDataGridColumnsPatreon(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
        {
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.Font = new Font("Arial", 10, FontStyle.Bold);
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            SourceGrid.Cells.Views.Cell cellView = new SourceGrid.Cells.Views.Cell();
            cellView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            cellView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dgEmployeeReport.Columns.Add("EmployeeID", "EmployeeID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.Visible = false;
            gridColumn.Width = 30;

            gridColumn = dgEmployeeReport.Columns.Add("CardNumber", "CardNumber", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.Width = 45;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            gridColumn = dgEmployeeReport.Columns.Add("SurName", "SurName", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 85;

            gridColumn = dgEmployeeReport.Columns.Add("FirstName", "FirstName", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 430;

            gridColumn = dgEmployeeReport.Columns.Add("Address", "Address", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgEmployeeReport.Columns.Add("Email", "Email", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 60;

            gridColumn = dgEmployeeReport.Columns.Add("DOB", "DOB", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 170;

            dgEmployeeReport.AutoStretchColumnsToFitWidth = true;

            foreach (SourceGrid.DataGridColumn col in columns)
            {
                SourceGrid.Conditions.ICondition condition =
                    SourceGrid.Conditions.ConditionBuilder.AlternateView(col.DataCell.View,
                                                                         Global.Grid_Color, Color.Black);
                col.Conditions.Add(condition);
            }
        }

        #endregion

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

                DataTable dtEmployeeCopy = null;
                dtEmployeeCopy = dtReport.Copy();

                dsEmployee.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
                ReportClass rpt;
                rpt = new rptDetailEmployee();
                //Fill the logo on the report
                Misc.WriteLogo(dsEmployee, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
                if (m_ReportSettings.RptType != "Patreon")
                {
                    rpt.SetDataSource(dsEmployee);
                    try
                    {
                        dsEmployee.Tables.Remove("tblEmployeeDetail");

                    }
                    catch
                    {

                    }
                    System.Data.DataView view = new System.Data.DataView(dtEmployeeCopy);

                    DataTable selected = view.ToTable("tblEmployeeDetail", false, "EmployeeName", "BirthDate", "StartDate", "LevelName", "Faculty", "Designation", 
                        "Department", "Status", "JobType", "Phone", "Email", "TempAddress", "PermAddress", "Nationality", "Ethnicity", "EmployeeType", "Gender", 
                        "DistrictName");

                    //dsStudent.Tables.Remove("tblStudent");
                    dsEmployee.Tables.Add(selected);
                    
                }
                else //if (m_ReportSettings.RptType == "Patreon")
                {
                    dsEmployeePatreon dsPatreon = new dsEmployeePatreon();
                    rpt = new rptEmployeePatreonReport();
                    //Fill the logo on the report
                    Misc.WriteLogo(dsPatreon, "tblImage");
                    //Set DataSource to be dsTrial dataset on the report
                    try
                    {
                        dsPatreon.Tables.Remove("tblPatreon");

                    }
                    catch
                    {

                    }
                    System.Data.DataView view = new System.Data.DataView(dtTemp);

                    DataTable selected = view.ToTable("tblPatreon", false,
                        "EmployeeID",
                        "CardNumber",
                        "SurName",
                        "FirstName",
                        "Address",
                        "Email",
                        "ContactNo",
                        "DOB"
                        );

                    //dsStudent.Tables.Remove("tblStudent");
                    dsPatreon.Tables.Add(selected);
                    rpt.SetDataSource(dsPatreon);

                }
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

                CrystalDecisions.Shared.ParameterDiscreteValue pdvDep = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvDes = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvFac = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvStatus = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvJobType = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvLevel = new CrystalDecisions.Shared.ParameterDiscreteValue();
                //CrystalDecisions.Shared.ParameterDiscreteValue pdvAcademicYear = new CrystalDecisions.Shared.ParameterDiscreteValue();


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
                pdvReport_Head.Value =lblReportTitle.Text;
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                pdvReport_Date.Value = "As On Date: " + Date.ToSystem(Convert.ToDateTime(Date.GetServerDate()));
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                pdvDep.Value = m_ReportSettings.Department;
                pvCollection.Clear();
                pvCollection.Add(pdvDep);
                rpt.DataDefinition.ParameterFields["paramDepartment"].ApplyCurrentValues(pvCollection);

                pdvDes.Value = m_ReportSettings.Designation;
                pvCollection.Clear();
                pvCollection.Add(pdvDes);
                rpt.DataDefinition.ParameterFields["paramDesignation"].ApplyCurrentValues(pvCollection);

                pdvFac.Value = m_ReportSettings.Faculty;
                pvCollection.Clear();
                pvCollection.Add(pdvFac);
                rpt.DataDefinition.ParameterFields["paramFaculty"].ApplyCurrentValues(pvCollection);

                pdvJobType.Value = m_ReportSettings.JobType;
                pvCollection.Clear();
                pvCollection.Add(pdvJobType);
                rpt.DataDefinition.ParameterFields["paramJobType"].ApplyCurrentValues(pvCollection);

                pdvStatus.Value = m_ReportSettings.Status;
                pvCollection.Clear();
                pvCollection.Add(pdvStatus);
                rpt.DataDefinition.ParameterFields["paramStatus"].ApplyCurrentValues(pvCollection);

                pdvLevel.Value = m_ReportSettings.Level;
                pvCollection.Clear();
                pvCollection.Add(pdvLevel);
                rpt.DataDefinition.ParameterFields["paramLevel"].ApplyCurrentValues(pvCollection);



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
        
        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowReport();
            this.Width = this.Width - 1;
            this.Width = this.Width + 1;
        }
    }
}
