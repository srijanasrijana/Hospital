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
using CrystalDecisions.Shared;
using System.Threading;
using Common;
using Accounts.Reports;

namespace Accounts
{
    public partial class frmCreditNoteRegister : Form
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        private CreditNoteRegisterSettings m_CreditNoteRegister;
        private Accounts.Model.dsCreditNote dscreditNote = new Model.dsCreditNote();
        private string FileName = "";
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        //For Export Menu
        ContextMenu Menu_Export;
        public frmCreditNoteRegister(CreditNoteRegisterSettings CredNoteRegister)
        {
            InitializeComponent();
            m_CreditNoteRegister = new CreditNoteRegisterSettings();
            m_CreditNoteRegister.FromDate = CredNoteRegister.FromDate;
            m_CreditNoteRegister.ToDate = CredNoteRegister.ToDate;
            m_CreditNoteRegister.AccClassID = CredNoteRegister.AccClassID;
            m_CreditNoteRegister.ProjectID = CredNoteRegister.ProjectID;
            m_CreditNoteRegister.HasDateRange = CredNoteRegister.HasDateRange;

           
        }

        private void frmCreditNoteRegister_Load(object sender, EventArgs e)
        {

       
            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code

            // double click for DebitNoteRegister
            DisplayBannar();
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(CreditNoteRegister_DoubleClick);

            //Disable multiple selection

            grdCreditNoteRegister.Selection.EnableMultiSelection = false;

            //Disable multiple selection
            grdCreditNoteRegister.Selection.EnableMultiSelection = false;

            grdCreditNoteRegister.Redim(1, 6);

            WriteHeader();//Calling this function for Writting Header of SourceGridView

            DataTable dtCreditNoteMasterInfo = CreditNote.GetCreditNoteMasterInfo(m_CreditNoteRegister.FromDate, m_CreditNoteRegister.ToDate, m_CreditNoteRegister.AccClassID, "CR_NOT");

            for (int i = 0; i < dtCreditNoteMasterInfo.Rows.Count; i++)
            {
                DataRow drCreditNoteMasterInfo = dtCreditNoteMasterInfo.Rows[i];

                int rows = grdCreditNoteRegister.Rows.Count;

                grdCreditNoteRegister.Rows.Insert(rows);

                //Display VoucherNo on grid
                grdCreditNoteRegister[rows, 0] = new SourceGrid.Cells.Cell(rows.ToString());
                grdCreditNoteRegister[rows, 0].AddController(dblClick);

                grdCreditNoteRegister[rows, 1] = new SourceGrid.Cells.Cell(Date.DBToSystem(drCreditNoteMasterInfo["CreditNote_Date"].ToString()));
                //grdDebitNoteRegister[rows, 1] = new SourceGrid.Cells.Cell(Date.DBToSystem(drDebNoteMasterInfo["DebitNote_Date"].ToString()));
                grdCreditNoteRegister[rows, 1].AddController(dblClick);

                grdCreditNoteRegister[rows, 2] = new SourceGrid.Cells.Cell(drCreditNoteMasterInfo["Voucher_No"].ToString());
                grdCreditNoteRegister[rows, 2].AddController(dblClick);

                grdCreditNoteRegister[rows, 5] = new SourceGrid.Cells.Cell(drCreditNoteMasterInfo["CreditNoteID"].ToString());
                grdCreditNoteRegister[rows, 5].Column.Visible = false;
                grdCreditNoteRegister[rows, 5].Column.Visible = false;

                string LedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drCreditNoteMasterInfo["LedgerID"]));
                grdCreditNoteRegister[rows, 3] = new SourceGrid.Cells.Cell(LedgerName);
                grdCreditNoteRegister[rows, 3].AddController(dblClick);

                //Block for getting the information of tblCreditNoteMaster according to CreditNoteID

                DataTable dtCreditNoteDtlInfo = CreditNote.GetCredNoteDtlInfo(Convert.ToInt32(drCreditNoteMasterInfo["CreditNoteID"]));   //According to CreditNoteID find the information of tblDebitNoteDetail where DeCr=Debit
                double TotalCreditAmt = 0;
                for (int k = 1; k <= dtCreditNoteDtlInfo.Rows.Count; k++)
                {

                    DataRow dr = dtCreditNoteDtlInfo.Rows[k - 1];
                    TotalCreditAmt += Convert.ToDouble(dr["Amount"]);

                }

                grdCreditNoteRegister[rows, 4] = new SourceGrid.Cells.Cell(TotalCreditAmt.ToString());
                grdCreditNoteRegister[rows, 4].AddController(dblClick);



            }
        }
        //Function for Writting Header Part of SourceGridView
        private void WriteHeader()
        {

            //Define the HeaderPart of sourceGridView

            grdCreditNoteRegister[0, 0] = new MyHeader("S. No.");
            grdCreditNoteRegister[0, 1] = new MyHeader("Date");
            grdCreditNoteRegister[0, 2] = new MyHeader("Voucher No");
            grdCreditNoteRegister[0, 3] = new MyHeader("Account");
            grdCreditNoteRegister[0, 4] = new MyHeader("Total Amount");
            grdCreditNoteRegister[0, 5] = new MyHeader("CreditNoteID");

            //Define size of column of Grid
            grdCreditNoteRegister[0, 0].Column.Width = 50;
            grdCreditNoteRegister[0, 1].Column.Width = 150;
            grdCreditNoteRegister[0, 2].Column.Width = 150;
            grdCreditNoteRegister[0, 3].Column.Width = 200;
            grdCreditNoteRegister[0, 4].Column.Width = 150;
            grdCreditNoteRegister[0, 5].Column.Width = 100;
            grdCreditNoteRegister[0, 5].Column.Visible = false;


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
        private void DisplayBannar()
        {
            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                // lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FiscalYear));
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear;
            if (m_CreditNoteRegister.HasDateRange == true)
            {
                lblFromDate.Text = Date.ToSystem(m_CreditNoteRegister.FromDate);
                lblToDate.Text = Date.ToSystem(m_CreditNoteRegister.ToDate);
                lblAsonDate.Visible = false;
            }
            else
            {
                lblFromDate.Visible = false;
                lblToDate.Visible = false;
                label2.Visible = false;
                label4.Visible = false;
                lblAsonDate.Text = "As On Date" + " " + Date.ToSystem(Date.GetServerDate());
            }
            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_CreditNoteRegister.ProjectID), LangMgr.DefaultLanguage);
            if (m_CreditNoteRegister.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }

            
        }   

        private void CreditNoteRegister_DoubleClick(object sender, EventArgs e)
        {
            try
            {


                //Get the Selected Row

                int CurRow = grdCreditNoteRegister.Selection.GetSelectionRegion().GetRowsIndex()[0];//

                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdCreditNoteRegister, new SourceGrid.Position(CurRow, 5));

                int CreditNoteID = Convert.ToInt32(cellType.Value);
                frmCreditNote frm = new frmCreditNote(CreditNoteID);
                frm.Show();

            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }

        private void frmCreditNoteRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void LoadDatainDataSet()
        {
            for (int i = 0; i < grdCreditNoteRegister.Rows.Count - 1; i++)
            {
                dscreditNote.Tables["tblCreditNote"].Rows.Add(grdCreditNoteRegister[i + 1, 0].Value, grdCreditNoteRegister[i + 1, 1].Value, grdCreditNoteRegister[i + 1, 2].Value, grdCreditNoteRegister[i + 1, 3].Value, grdCreditNoteRegister[i + 1, 4].Value);
            }
        }
        private void lblAsonDate_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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

            dscreditNote.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed


            rptCreditNote rpt = new rptCreditNote();
          
            //Fill the logo on the report

            Misc.WriteLogo(dscreditNote, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dscreditNote);

            LoadDatainDataSet();


            //Provide values to the parameters on the report
            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFiscal_Year = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
           

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

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

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

                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            }
                pdvReport_Head.Value = "Credit Note";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
                if (m_CompanyDetails.FYFrom != null)
                    //pdvFiscal_Year.Value = "Fiscal Year:" + Date.ToSystem(Convert.ToDateTime(m_CompanyDetails.FYFrom));
                    pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;

                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                if (m_CreditNoteRegister.HasDateRange == true)
                {
                    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_CreditNoteRegister.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_CreditNoteRegister.ToDate));
                }
                else
                {
                    pdvReport_Date.Value = "As of now";
                }
                //if (m_CreditNoteRegister.FromDate != null && m_CreditNoteRegister.ToDate != null)
                //{
                //    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_CreditNoteRegister.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_CreditNoteRegister.ToDate));
                //}
                //if (m_CreditNoteRegister.ToDate != null)
                //{
                //    pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_CreditNoteRegister.ToDate));
                //}
                //if (m_CreditNoteRegister.FromDate != null)
                //{
                //    //This is actually not applicable
                //    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_CreditNoteRegister.FromDate));
                //}
                //else//both are null
                //{
                //    pdvReport_Date.Value = "As of now";

                //}


                pvCollection.Clear();

                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);


          

            //dsTrialPY.Tables["tblGroup"].Rows.Add("1", "Test", "200", "300", "CrystalReport");


            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;
            //rpt.Export(opt);
            //return;
            //Finally, show the report form
            frmReportViewer frm = new frmReportViewer();
          
            frm.SetReportSource(rpt);
          

            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");

            // Close the dialog
            ProgressForm.CloseForm();

            switch (myPrintType)
            {
                case PrintType.DirectPrint: //Direct Printer
                  
                        rpt.PrintOptions.PrinterName = "";
                        rpt.PrintToPrinter(1, false, 0, 0);
                  
                    return;
                case PrintType.Excel: //Excel
                   
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        rpt.Export();
                        rpt.Close();
                 
                    return;
                case PrintType.PDF: //PDF
                   
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = rpt.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        rpt.Export();
                  
                    return;
                case PrintType.Email: //Excel
                   
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
                default: //Crystal Report

                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;

                    break;
            }

            this.Cursor = Cursors.Default;
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

            Menu_Export.MenuItems.Add(mnuExcel);
            Menu_Export.MenuItems.Add(mnuPDF);
            Menu_Export.MenuItems.Add(mnuEmail);
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
                    PrintPreviewCR(PrintType.PDF);
                    break;
                case "mnuEmail":
                    //Code for excel export
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
                    PrintPreviewCR(PrintType.Email);
                    break;

            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            //Clear all rows
            grdCreditNoteRegister.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmCreditNoteRegister_Load(sender, e);

            this.Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.DirectPrint);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
