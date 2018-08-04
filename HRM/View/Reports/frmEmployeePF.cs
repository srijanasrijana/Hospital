using BusinessLogic;
using BusinessLogic.HRM.Report;
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
    public partial class frmEmployeePF : Form
    {
        BusinessLogic.HRM.PFReport pfr = new BusinessLogic.HRM.PFReport();
        BusinessLogic.HRM.Employee emp = new BusinessLogic.HRM.Employee();
        private Model.dsEmployeePF dsEmployeePf = new Model.dsEmployeePF();
        int m_paySlipID=0;
        int[] m_paySlipIDs = null;

        DataTable dtFound = new DataTable();
        private string FileName = "";
        private string m_PfEmployeeTotal = "";
        private string m_PfEmployerTotal = "";

        private string m_PfAmtTotal = "";
        private string m_Faculty = "";
        private int prntDirect = 0;
        //private bool m_isRemaining = false;
        //private string m_Date = "";
        private EmployeeReportSettings m_Settings;
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
        public frmEmployeePF()
        {
            InitializeComponent();
        }

        public frmEmployeePF(int paySlipId,string faculty)
        {
            InitializeComponent();
            m_paySlipID = paySlipId;
            m_Faculty = faculty;
        }
        //public frmEmployeePF(int[] paySlipId, string faculty, bool isRemaining, string date, DateTime fromDate, DateTime toDate)
        //{
        //    InitializeComponent();
        //    m_paySlipIDs = paySlipId;
        //    m_Faculty = faculty;
        //    m_isRemaining = isRemaining;
        //    m_Date = date;

        //}

        public frmEmployeePF(EmployeeReportSettings settings)
        {
            m_Settings = settings;
            InitializeComponent();

        }
        private void frmEmployeePF_Load(object sender, EventArgs e)
        {
            try
            {
                WriteBanner();
                WriteHeader();
                LoadGrid(false);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void WriteBanner()
        {
            DataTable dt = emp.GetSalaryMaster(m_Settings.paySlipIds[0]);
            lblReportTitle.Text = "Employee's Providend Fund" + (m_Settings.isRemaining ? "(Remaining)" : "");
            if(dt.Rows.Count >0)
            {
                lblMonth.Text += dt.Rows[0]["year"].ToString() +" "+ dt.Rows[0]["monthName"].ToString();
                lblDate.Text += DateManager.Date.ToSystem(DateManager.Date.GetServerDate()).ToString();
                lblFaculty.Text += m_Faculty;
            }


            if (m_Settings.paySlipDate != null )//&& m_Settings.paySlipDate != null)
            {
                lblPaySlipDate.Text = "Pay Slip Date : " + Date.DBToSystem(m_Settings.paySlipDate.ToString());
            }

            if (m_Settings.fromDate != null && m_Settings.toDate != null)
            {
                lblFromDate.Text = "From: " + Date.DBToSystem(m_Settings.fromDate.ToString());
            }
        }

        //Customized header
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
        private void WriteHeader()
        {
            dtFound = pfr.GetPFData(m_Settings);//(m_paySlipIDs, m_isRemaining,m_Date);
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
            grdEmployeePF[0, 3] = new MyHeader("EPF No.");
            grdEmployeePF[0, 4] = new MyHeader("Total Fund Deducted");
            grdEmployeePF[0, 5] = new MyHeader("PF Contribution By Employer");
            grdEmployeePF[0, 6] = new MyHeader("PF Deducted From Employee");

            grdEmployeePF[0, 0].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;
            grdEmployeePF[0, 0].Column.Width = 30;
            grdEmployeePF[0, 1].Column.Width = 200;
            grdEmployeePF[0, 2].Column.Width = 250;
            grdEmployeePF[0, 3].Column.Width = 
            grdEmployeePF[0, 4].Column.Width =
            grdEmployeePF[0, 5].Column.Width = 100;
            grdEmployeePF[0, 6].Column.Width = 130;
        }
        private void LoadGrid(bool isCrystalRpt)
        {
            try
            {
                int rowsCount = 0;
                decimal totalPFEmployee = 0, totalPFT = 0, totalPFEmployer = 0;
                int lastRow = 0;
                rowsCount = dtFound.Rows.Count;
                if (rowsCount > 0)
                {

                    for (int i = 1; i <= rowsCount; i++)
                    {
                        
                        DataRow dr = dtFound.Rows[i - 1];
                        string designation = dr["DesignationName"].ToString();
                        string StaffName = dr["StaffName"].ToString();
                        string PfNumber = dr["PFNumber"].ToString();
                        decimal amtEmployee = Convert.ToDecimal(Convert.ToDecimal(dr["pf"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        decimal amtEmployer = amtEmployee; //Convert.ToDecimal(amtEmployee - Convert.ToDecimal(dr["PFAdjust"]));

                        decimal amtTotal = Convert.ToDecimal(Convert.ToDecimal(dr["pfTotal"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

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

                            grdEmployeePF[i, 4] = new SourceGrid.Cells.Cell(amtTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            grdEmployeePF[i, 5] = new SourceGrid.Cells.Cell(amtEmployee.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                            grdEmployeePF[i, 6] = new SourceGrid.Cells.Cell(amtEmployer.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                            grdEmployeePF[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                            alternate.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                            grdEmployeePF[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdEmployeePF[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
                        }
                        else
                        {
                            dsEmployeePf.Tables["tblEmpPfDetails"].Rows.Add(StaffName, designation, PfNumber, amtEmployee, amtTotal, amtEmployer.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        }
                        totalPFEmployee += amtEmployee;
                        totalPFEmployer += amtEmployer;
                        totalPFT += amtTotal;
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
                        grdEmployeePF[lastRow + 1, 4] = new SourceGrid.Cells.Cell(totalPFT.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdEmployeePF[lastRow + 1, 5] = new SourceGrid.Cells.Cell(totalPFEmployer.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdEmployeePF[lastRow + 1, 6] = new SourceGrid.Cells.Cell(totalPFEmployee.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdEmployeePF[lastRow + 1, 0].View = new SourceGrid.Cells.Views.Cell(RowTotal);
                        grdEmployeePF[lastRow + 1, 4].View = new SourceGrid.Cells.Views.Cell(RowTotal);
                        grdEmployeePF[lastRow + 1, 5].View = new SourceGrid.Cells.Views.Cell(RowTotal);
                        grdEmployeePF[lastRow + 1, 6].View = new SourceGrid.Cells.Views.Cell(RowTotal);

                    }
                    else
                    {
                        //add total pf amount and total of total pf as parameter
                        m_PfEmployeeTotal = totalPFEmployee.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        m_PfAmtTotal = totalPFT.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        m_PfEmployerTotal = totalPFEmployer.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                }
            }
            catch(Exception ex)
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

                dsEmployeePf.Clear();
                LoadGrid(true);
                HRM.Reports.rptEmployeePF rpt = new HRM.Reports.rptEmployeePF();

                //Fill the logo on the report
                Misc.WriteLogo(dsEmployeePf, "tblImage");

                rpt.SetDataSource(dsEmployeePf);


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
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_PaySlipDate = new CrystalDecisions.Shared.ParameterDiscreteValue();

                CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvPfTotal = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvPfAmtEmployeeTotal = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvPfAmtEmployerTotal = new CrystalDecisions.Shared.ParameterDiscreteValue();

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


                pdvPfTotal.Value = m_PfAmtTotal;
                pvCollection.Clear();
                pvCollection.Add(pdvPfTotal);
                rpt.DataDefinition.ParameterFields["TotalPFofTotal"].ApplyCurrentValues(pvCollection);

                pdvPfAmtEmployeeTotal.Value = m_PfEmployeeTotal;
                pvCollection.Clear();
                pvCollection.Add(pdvPfAmtEmployeeTotal);
                rpt.DataDefinition.ParameterFields["TotalPFEmployee"].ApplyCurrentValues(pvCollection);


                pdvPfAmtEmployerTotal.Value = m_PfEmployerTotal;
                pvCollection.Clear();
                pvCollection.Add(pdvPfAmtEmployerTotal);
                rpt.DataDefinition.ParameterFields["TotalPFEmployer"].ApplyCurrentValues(pvCollection);
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
        private void button3_Click(object sender, EventArgs e)
        {
            prntDirect = 0;
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //Load all over again;
            frmEmployeePF_Load(sender, e);

            this.Cursor = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
