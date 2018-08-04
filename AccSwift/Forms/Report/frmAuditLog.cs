using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Inventory.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using BusinessLogic;
using System.Collections;
using Inventory.DataSet;
using System.Threading;
using DateManager;
using Common;
using AccSwift.CrystalReports;


namespace AccSwift.Forms.Report
{
    public partial class frmAuditLogReport : Form
    {
        
        private int prntDirect = 0;
        //For Export Menu
        ContextMenu Menu_Export;
        string FileName = " ";
        DataTable dtaudit = new DataTable();
        private DataSet.dsAuditLog dsAuditLog1 = new DataSet.dsAuditLog();
        DateTime fromDate;
        DateTime toDate;
        public frmAuditLogReport()
        {
            InitializeComponent();
        }
        public frmAuditLogReport(DataTable dt,DateTime fromDate,DateTime toDate)
        {
            dtaudit = dt;
            InitializeComponent();
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            lblAsOnDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            lblFromToDate.Text = "From : " + Date.ToSystem(fromDate) + " To : " + Date.ToSystem(toDate);
            //lblCompanyName.Text = m_CompanyDetails.CompanyName;
            //lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            //lblContact.Text = "Contact: " + m_CompanyDetails.Telephone;
            //lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;
        }

        private void frmAuditLogReport_Load(object sender, EventArgs e)
        {
            
            //frmProgress ProgressForm = new frmProgress();
            //ProgressForm.CloseForm();
            DisplayBannar();
           // FillGrid();

            FillFridWithData();
            dgAuditLog.EnableSort = false;

          //  this.WindowState = FormWindowState.Minimized;
           // this.WindowState = FormWindowState.Maximized;
        }

        private void FillFridWithData()
        {
            grdAuditLog.Visible = false;
            dgAuditLog.Visible = true;
            dgAuditLog.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            dgAuditLog.Selection.EnableMultiSelection = false;

            #region Data binding
            DataView view = new DataView(dtaudit);
            view.AllowNew = false;
            view.AllowDelete = false;
            view.AllowEdit = false;

            
            
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(view);
            CreateDataGridColumns(dgAuditLog.Columns, bd);
            dgAuditLog.DataSource = bd;
            //dgAuditLog.Width = dgAuditLog.Width - 5;
            //dgAuditLog.Width = this.Width - 25;

            #endregion
        }

        private void CreateDataGridColumns(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
        {

            //text format for column header
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            viewColumnHeader.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);
            viewColumnHeader.BackColor = Color.LightGray;

            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dgAuditLog.Columns.Add("RowNumber", "S.No", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
           gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgAuditLog.Columns.Add("VoucherDate", "Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 90;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgAuditLog.Columns.Add("ComputerName", "Computer Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 170;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgAuditLog.Columns.Add("UserName", "User Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 110;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgAuditLog.Columns.Add("Voucher_Type", "Voucher Type", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 100;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgAuditLog.Columns.Add("Action", "Action", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 90;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

          //  gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgAuditLog.Columns.Add("Description", "Description", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.HeaderCell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            gridColumn.Width = 700;
             gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            dgAuditLog.AutoStretchColumnsToFitWidth = true;


        }
        private void FillGrid()
        {
            //int count = dtaudit.Rows.Count;
                grdAuditLog.Rows.Clear();
                grdAuditLog.Redim(dtaudit.Rows.Count + 1, 8);
                WriteHeader();
                for (int i = 0; i <= (dtaudit.Rows.Count - 1); i++)
                {
                    DataRow dr = dtaudit.Rows[i];
                    SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                    if (i % 2 == 0)
                    {
                        //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                    }
                    else
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }

                    grdAuditLog[i+1, 0] = new SourceGrid.Cells.Cell((i+1).ToString());
                    grdAuditLog[i+1, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                    
                    grdAuditLog[i+1, 1] = new SourceGrid.Cells.Cell(Date.ToSystem( Convert.ToDateTime(dr["voucherdate"].ToString())));
                    grdAuditLog[i + 1, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                    //string cname = dr["Computername"].ToString();
                    //string substringcname = cname.Substring(30,10);

                    grdAuditLog[i + 1, 2] = new SourceGrid.Cells.Cell(dr["Computername"].ToString());
                    grdAuditLog[i + 1, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                    //grdAuditLog[i, 2].AddController(gridKeyDown);

                    grdAuditLog[i+1, 3] = new SourceGrid.Cells.Cell(dr["username"].ToString());
                    grdAuditLog[i + 1, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                    //grdAuditLog[i, 3].AddController(gridKeyDown);

                    grdAuditLog[i+1, 4] = new SourceGrid.Cells.Cell(dr["voucher_type"].ToString());
                    grdAuditLog[i + 1, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                    //grdAuditLog[i, 4].AddController(gridKeyDown);

                    grdAuditLog[i+1, 5] = new SourceGrid.Cells.Cell(dr["action"].ToString());
                    grdAuditLog[i + 1, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
                    grdAuditLog[i+1, 6] = new SourceGrid.Cells.Cell(dr["description"].ToString());
                    grdAuditLog[i + 1, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
                    grdAuditLog[i+1, 7] = new SourceGrid.Cells.Cell(dr["rowid"].ToString());
                    grdAuditLog[i + 1, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
                    
                  }            
        }
        private void WriteHeader()
        {
            grdAuditLog[0, 0] = new MyHeader("S No.");
            grdAuditLog[0, 1] = new MyHeader("Date");
            grdAuditLog[0, 2] = new MyHeader("ComputerName");
            grdAuditLog[0, 3] = new MyHeader("User");
            grdAuditLog[0, 4] = new MyHeader("VoucherType");
            grdAuditLog[0, 5] = new MyHeader("Action");
            grdAuditLog[0, 6] = new MyHeader("Description");
            grdAuditLog[0, 7] = new MyHeader("RowID");

            grdAuditLog[0, 0].Column.Width = 40;
            grdAuditLog[0, 1].Column.Width = 80;
            grdAuditLog[0, 2].Column.Width = 150;
            grdAuditLog[0, 3].Column.Width = 100;
            grdAuditLog[0, 4].Column.Width = 80;
            grdAuditLog[0, 5].Column.Width = 70;
            grdAuditLog[0, 6].Column.Width = 450;
            grdAuditLog[0, 7].Column.Width = 10;

            grdAuditLog[0, 7].Column.Visible = false;
          
        }
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 9, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;

                AutomaticSortEnabled = false;
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
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

            dsAuditLog1.Clear();
           // rptDayBookTransact rpt = new rptDayBookTransact();
            rptAuditLog rpt = new rptAuditLog();

            //Fill the logo on the report
            Misc.WriteLogo(dsAuditLog1, "tblImage");
            rpt.SetDataSource(dsAuditLog1);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            CrystalDecisions.Shared.ParameterDiscreteValue pdvFromToDate = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvAsOnDate = new CrystalDecisions.Shared.ParameterDiscreteValue();
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


            pdvCompany_Address.Value = m_CompanyDetails.Address1;
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


            pdvCompany_Slogan.Value = m_CompanyDetails.Website;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Slogan);
            rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
               
          }
          else //if user is not root, take information from tblUserPreference
          {
                 string companyname=UserPreference.GetValue("COMPANY_NAME", uid); 
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

                 pdvCompany_Slogan.Value = companyslogan;
                 pvCollection.Clear();
                 pvCollection.Add(pdvCompany_Slogan);
                 rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

          }
            DateTime rptdate;
            rptdate=DateTime.Now;
            pdvReport_Date.Value =Date.ToSystem(rptdate); 
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);

            pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvPreparedBy);
            rpt.DataDefinition.ParameterFields["PreparedBy"].ApplyCurrentValues(pvCollection);

            pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvCheckedBy);
            rpt.DataDefinition.ParameterFields["CheckedBy"].ApplyCurrentValues(pvCollection);

            pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvApprovedBy);
            rpt.DataDefinition.ParameterFields["ApprovedBy"].ApplyCurrentValues(pvCollection);

            pdvFromToDate.Value = "From : " + Date.ToSystem(fromDate) + " To : " + Date.ToSystem(toDate);
            pvCollection.Clear();
            pvCollection.Add(pdvFromToDate);
            rpt.DataDefinition.ParameterFields["FromToDate"].ApplyCurrentValues(pvCollection);

            pdvAsOnDate.Value = "As on Date: " + Date.ToSystem(DateTime.Today);
            pvCollection.Clear();
            pvCollection.Add(pdvAsOnDate);
            rpt.DataDefinition.ParameterFields["AsOnDate"].ApplyCurrentValues(pvCollection);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //FillReportData();
            try
            {
                dsAuditLog1.Tables.Remove("tblAuditLog");
            }
            catch(Exception ex)
            {

            }
            DataView dview = new DataView(dtaudit);
            DataTable selected = dview.ToTable("tblAuditLog",false,"ComputerName","UserName","Voucher_Type","Action","VoucherDate","Description");

            dsAuditLog1.Tables.Add(selected);


            frmReportViewer frm = new frmReportViewer();
            frm.SetReportSource(rpt);
            //frm.Show();
           // frm.WindowState = FormWindowState.Maximized;
            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");

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

           // frm.WindowState = FormWindowState.Maximized;

        }
        private void FillReportData()
        {
            //int count = dtaudit.Rows.Count;
            foreach(DataRow dr in dtaudit.Rows)
            {
               // DateTime vdate =Date.ToSystem( dr["VoucherDate"].ToString());
                DateTime logdate = Convert.ToDateTime(dr["voucherdate"]);
                //string cname = dr["Computername"].ToString();
                //string substringcname = cname.Substring(30, 10);
                dsAuditLog1.Tables["tblAuditLog"].Rows.Add(dr["ComputerName"].ToString(), dr["UserName"].ToString(), dr["Voucher_Type"].ToString(), dr["Action"].ToString(), Date.ToSystem(logdate), dr["Description"].ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgAuditLog.Columns.Clear();
            this.Cursor = Cursors.WaitCursor;

            frmAuditLogReport_Load(sender, e);
            this.Cursor = Cursors.Default;

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void dgAuditLog_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
