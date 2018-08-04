using Accounts.Model;
using Accounts.Reports;
using BusinessLogic;
using BusinessLogic.Accounting.Reports;
using Common;
using CrystalDecisions.Shared;
using DateManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Accounts.View.Reports
{
    public partial class frmBudgetReport : Form
    {
        public frmBudgetReport()
        {
            InitializeComponent();
        }
        BudgetReportSetting M_BudgetReport;      
        DataTable tblBgtReportData;
        DataTable tblDataForCR;
        private string FileName = "";
        private int prntDirect = 0;
        ContextMenu Menu_Export;
        private SourceGrid.Cells.Views.Cell CommonView=new SourceGrid.Cells.Views.Cell();

        dsBudgetReport dsBgtReport = new dsBudgetReport();
        //double BudgetTotal = 0;
        //double ActualTotal = 0;
        //double varianceTotal = 0;

        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }


        public frmBudgetReport(BudgetReportSetting M_BudgetReport)
        {
            InitializeComponent();
            this.M_BudgetReport = M_BudgetReport;
        }
        private void frmBudgetReport_Load(object sender, EventArgs e)
        {         
            DisplayBannerForSummary();
            
            ////Disable multiple selection
            grdBudget.Selection.EnableMultiSelection = false;
            grdBudget.Redim(0,7);
           // grdBudget.FixedRows = 1;
           // MakeHeaderForSummary();
            MakeHeaderForDetail();

            //table for storing source grid data for Crystal Report at load time so that the roport will be fast
            tblDataForCR = new DataTable();
            tblDataForCR.Columns.Add("AccountName", typeof(string));
            tblDataForCR.Columns.Add("AccountType", typeof(string));
            tblDataForCR.Columns.Add("PeriodBudget", typeof(double));
            tblDataForCR.Columns.Add("PeriodActual", typeof(double));
            tblDataForCR.Columns.Add("Variance", typeof(double));
            fillGridWithData();
        }        
        private void fillGridWithData()
        {
            //create a string value by concatinating all class ids
            string ClassIDs = "";
            if (M_BudgetReport.AccClassID != null)
            {
                foreach (object obj in M_BudgetReport.AccClassID)
                {
                    if (ClassIDs == "")
                    {
                        ClassIDs = obj.ToString();
                    }
                    else
                    {
                        ClassIDs += "," + obj.ToString();
                    }
                }
            }

            else
            {
                //if class no class id is found set root as default class id 
                ClassIDs = "1";
            }                   
                               //get budget report data for summary
                    tblBgtReportData = BudgetReport.GetBudgetReportData(M_BudgetReport.BudgetID, ClassIDs, M_BudgetReport.ProjectID);
                    SourceGrid.Cells.Views.Cell amountView = new SourceGrid.Cells.Views.Cell();
                    amountView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            //fill data in source grid
            if (tblBgtReportData.Rows.Count > 0)
            {              
                foreach (DataRow dr in tblBgtReportData.Rows)
                {
                    int i= Convert.ToInt32( grdBudget.Rows.Count);
                    AddNewRowToGrid(i);
                    grdBudget[i, 1].Value = dr[2].ToString();
//grdBudget[i, 1].View = CommonView;
                   // grdBudget[i, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                    grdBudget[i, 2].Value = dr[3].ToString();
                  //  grdBudget[i, 2].View = CommonView;
                  //  grdBudget[i, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;


                    grdBudget[i, 3].Value = dr[4].ToString();
                   // grdBudget[i, 3].View = CommonView;
                  //  grdBudget[i, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;


                    grdBudget[i, 4].Value = dr[1].ToString();
                  //  grdBudget[i, 4].View = CommonView;
                    grdBudget[i, 4].View = new SourceGrid.Cells.Views.Cell(amountView);
                    grdBudget[i, 5].Value = dr[0].ToString();
                   // grdBudget[i, 5].View = CommonView;
                    grdBudget[i, 5].View = new SourceGrid.Cells.Views.Cell(amountView);

                    grdBudget[i, 6].Value = (Convert.ToDecimal(dr[1].ToString()) - Convert.ToDecimal(dr[0].ToString())).ToString();
                    //grdBudget[i, 6].View = CommonView;
                    grdBudget[i, 6].View = new SourceGrid.Cells.Views.Cell(amountView);


                   
                    tblDataForCR.Rows.Add(dr[3].ToString(), dr[4].ToString(), Convert.ToDouble(dr[1].ToString()), Convert.ToDouble(dr[0].ToString()), (Convert.ToDecimal(dr[1].ToString()) - Convert.ToDecimal(dr[0].ToString())));
                    
                }
            }
            else
            {
                Global.Msg("No data found");
            }         
        }

        private void AddNewRowToGrid( int i)
        {
            grdBudget.Rows.Insert(i);
            grdBudget[i, 0] = new SourceGrid.Cells.Cell(i.ToString());
            grdBudget[i, 1] = new SourceGrid.Cells.Cell("");
            grdBudget[i, 2] = new SourceGrid.Cells.Cell("");
            grdBudget[i, 3] = new SourceGrid.Cells.Cell("");
            grdBudget[i, 4] = new SourceGrid.Cells.Cell("");
            grdBudget[i, 5] = new SourceGrid.Cells.Cell("");
            grdBudget[i, 6] = new SourceGrid.Cells.Cell("");


        }
        private void MakeHeaderForSummary()
        {
            //To make Grid Header            
            grdBudget[0, 0] = new MyHeader("S.No.");
            grdBudget[0, 1] = new MyHeader("Budget ID");
            grdBudget[0, 2] = new MyHeader("Budget Name");
            grdBudget[0, 3] = new MyHeader("Start Date");
            grdBudget[0, 4] = new MyHeader("End Date");
            grdBudget[0, 5] = new MyHeader("Period Budget");
            grdBudget[0, 6] = new MyHeader("Period Actual");
            grdBudget[0, 7] = new MyHeader("Variance");

            grdBudget[0, 0].Column.Width = 50;
            grdBudget[0, 1].Column.Width = 50;
            grdBudget.Columns[1].Visible = false;
            grdBudget[0, 2].Column.Width = 300;
            grdBudget[0, 3].Column.Width = 200;
            grdBudget[0, 4].Column.Width = 200;   
            grdBudget[0, 5].Column.Width = 150;
            grdBudget[0, 6].Column.Width = 150;
            grdBudget[0, 7].Column.Width = 150;
        }

          private void MakeHeaderForDetail()
        {
            grdBudget.Rows.Insert(0);
            grdBudget[0, 0] = new MyHeader("S.No.");
            grdBudget.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdBudget[0, 1] = new MyHeader("Account ID");
            grdBudget.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdBudget[0, 2] = new MyHeader("Account Name");
            grdBudget.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdBudget[0, 3] = new MyHeader("Account Type");
            grdBudget.Columns[3].AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdBudget[0, 4] = new MyHeader("Period Budget");
            grdBudget.Columns[4].AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdBudget[0, 5] = new MyHeader("Period Actual");
            grdBudget.Columns[5].AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdBudget[0, 6] = new MyHeader("Variance");
            grdBudget.Columns[6].AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdBudget[0, 0].Column.Width = 50;
            grdBudget[0, 1].Column.Width = 50;
            grdBudget.Columns[1].Visible = false;
            grdBudget[0, 2].Column.Width = 250;
            grdBudget[0, 3].Column.Width = 150;
            grdBudget[0, 4].Column.Width = 150;
            grdBudget[0, 5].Column.Width = 150;
            grdBudget[0, 6].Column.Width = 150;
            grdBudget.AutoStretchColumnsToFitWidth = true;


        }
       
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 10, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;
                AutomaticSortEnabled = false;
            }
        }
        private void DisplayBannerForSummary()

        {
            try
            {
                DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(M_BudgetReport.ProjectID), LangMgr.DefaultLanguage);
                if (M_BudgetReport.ProjectID != 0)
                {
                    DataRow drProjectInfo = dtProjectInfo.Rows[0];

                    lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
                }
                else
                {
                    lblProjectName.Text = " Project: " + "All";
                }

                lblBudgetName.Text = M_BudgetReport.BudgetName.ToString();
                if (M_BudgetReport.FromDate != null && M_BudgetReport.ToDate != null)
                {
                    lblAllSettings.Text = "From : " +Date.ToSystem (M_BudgetReport.FromDate).ToString() + "    To : " +Date.ToSystem(M_BudgetReport.ToDate).ToString();
                }

                lblAsOnDate.Text = "As On Date : " + Date.ToSystem(Date.GetServerDate());
                //if (bds.ToDate != null)
                //{
                //    lblAllSettings.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(bds.ToDate));
                //}
            }
            catch (Exception ex)
            {             
               Global.Msg(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows
            grdBudget.Redim(0, 0);
            this.Cursor = Cursors.WaitCursor;
            //Load all over again;
            frmBudgetReport_Load(sender, e);
            this.Cursor = Cursors.Default;
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);

        }
        private void PrintPreviewCR(PrintType myPrintType)
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

           dsBgtReport.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

            //otherwise it populate the records again and again


           rptBudgetReport rpt = new rptBudgetReport();

            //Fill the logo on the report
            Misc.WriteLogo(dsBgtReport, "tblImage");
            rpt.SetDataSource(dsBgtReport);


            try
            {
                dsBgtReport.Tables.Remove("tblBudgetReport");//this table was just for documentation that what field is gonna require
                System.Data.DataView view = new System.Data.DataView(tblDataForCR);
                DataTable selected = view.ToTable("tblBudgetReport", false, "AccountName", "AccountType", "PeriodBudget", "PeriodActual", "Variance");

                dsBgtReport.Tables.Add(selected);


                //Provide values to the parameters on the report
                CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvBudget_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

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

                    pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Phone);
                    rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                    //pdvCompany_Slogan.Value = m_CompanyDetails.Website;
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

                pdvReport_Head.Value = "Budget Report of " + lblBudgetName.Text.Trim();
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                pdvReport_Date.Value = "As On Date : " + Date.ToSystem(Date.GetServerDate());
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                pdvBudget_Date.Value = lblAllSettings.Text.Trim();
                pvCollection.Clear();
                pvCollection.Add(pdvBudget_Date);
                rpt.DataDefinition.ParameterFields["Budget_Date"].ApplyCurrentValues(pvCollection);

                //DisplayTransactWiseDayBook(true);
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
                /// frm.WindowState = FormWindowState.Maximized;


            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
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
            try
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
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void grdBudget_Paint(object sender, PaintEventArgs e)
        {

        }

     
    }
}
