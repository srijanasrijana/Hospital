using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Collections;
using DateManager;
using System.Threading;
using Inventory.CrystalReports;
using CrystalDecisions.Shared;
using Inventory.Forms;

namespace Inventory
{
    public partial class frmDebitNoteRegister : Form
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        private DebitNoteRegisterSettings m_DebNoteRegister;
        private DataSet.dsDebitNote dsDebitNote = new DataSet.dsDebitNote();
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
        public frmDebitNoteRegister(DebitNoteRegisterSettings DebNoteRegister)
        {
            InitializeComponent();

            m_DebNoteRegister = new DebitNoteRegisterSettings();
            m_DebNoteRegister.FromDate = DebNoteRegister.FromDate;
            m_DebNoteRegister.ToDate = DebNoteRegister.ToDate;
            m_DebNoteRegister.AccClassID = DebNoteRegister.AccClassID;
            m_DebNoteRegister.ProjectID = DebNoteRegister.ProjectID;
            m_DebNoteRegister.HasDateRange = DebNoteRegister.HasDateRange;

        }

        private void frmDebitNoteRegister_Load(object sender, EventArgs e)
        {

            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code

            // double click for DebitNoteRegister
            DisplayBannar();
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(DebitNoteRegister_DoubleClick);

            //Disable multiple selection

            grdDebitNoteRegister.Selection.EnableMultiSelection = false;

            //Disable multiple selection
            grdDebitNoteRegister.Selection.EnableMultiSelection = false;

            grdDebitNoteRegister.Redim(1, 6);

            WriteHeader();

          

            //Getting information from tblDebitNoteMaster according to DateRange

            //Collect the DebitNoteID according to the Date Range and AccClassID which are being  selected by users

            DataTable dtDebitNoteMasterInfo = DebitNote.GetDebitNoteMasterInfo(m_DebNoteRegister.FromDate, m_DebNoteRegister.ToDate,m_DebNoteRegister.AccClassID, "DR_NOT");
            for (int i = 0; i < dtDebitNoteMasterInfo.Rows.Count; i++)
            {
                DataRow drDebitNoteMasterInfo = dtDebitNoteMasterInfo.Rows[i];

                int rows = grdDebitNoteRegister.Rows.Count;

                grdDebitNoteRegister.Rows.Insert(rows);

                //Display VoucherNo on grid
                grdDebitNoteRegister[rows, 0] = new SourceGrid.Cells.Cell(rows.ToString());
                grdDebitNoteRegister[rows, 0].AddController(dblClick);

                grdDebitNoteRegister[rows, 1] = new SourceGrid.Cells.Cell(Date.DBToSystem(drDebitNoteMasterInfo["DebitNote_Date"].ToString()));
                grdDebitNoteRegister[rows, 1].AddController(dblClick);

                grdDebitNoteRegister[rows, 2] = new SourceGrid.Cells.Cell(drDebitNoteMasterInfo["Voucher_No"].ToString());
                grdDebitNoteRegister[rows, 2].AddController(dblClick);

                string LedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drDebitNoteMasterInfo["LedgerID"]));
                grdDebitNoteRegister[rows, 3] = new SourceGrid.Cells.Cell(LedgerName);
                grdDebitNoteRegister[rows, 3].AddController(dblClick);

                grdDebitNoteRegister[rows, 5] = new SourceGrid.Cells.Cell(drDebitNoteMasterInfo["DebitNoteID"].ToString());
                grdDebitNoteRegister[rows, 5].Column.Visible = false;

                //According to DebitNoteID find the information of tblDebitNoteDetail where DeCr=Debit

                DataTable dtDebitNoteDtlInfo = DebitNote.GetDebNoteDtlInfo(Convert.ToInt32(drDebitNoteMasterInfo["DebitNoteID"]));
                double TotalDebitAmt = 0;
                for (int k = 1; k <= dtDebitNoteDtlInfo.Rows.Count; k++)
                {

                    DataRow dr = dtDebitNoteDtlInfo.Rows[k - 1];
                    TotalDebitAmt += Convert.ToDouble(dr["Amount"]);
                }

                grdDebitNoteRegister[rows, 4] = new SourceGrid.Cells.Cell(TotalDebitAmt.ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces)));
                grdDebitNoteRegister[rows, 4].AddController(dblClick);



                ////Among All ledgers where DrCr= Debit,collect only thoes ledgers which consists under Creditor AccountGroup 
                //for (int j = 1; j <= dtDebitNoteDtlInfo.Rows.Count; j++)
                //{

                //    DataRow drDebitNoteDtlInfo = dtDebitNoteDtlInfo.Rows[j - 1];

                //    bool Found = false;
                    //try
                    //{

                      //  Found = AccountGroup.IsLedgerUnderGroup(Convert.ToInt32(drDebitNoteDtlInfo["LedgerID"]), 118);//Manually passing the GroupID of Creditor Which is equivalent to 118
                        //if (Found == true)
                        //{
                            //Write the name in the grid
                            //DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drDebitNoteDtlInfo["LedgerID"]), LangMgr.DefaultLanguage);

                            //DataRow dr = dt.Rows[0];
                            
                            //grdDebitNoteRegister[rows, 3] = new SourceGrid.Cells.Cell(dr["LedgerName"]);
                            //grdDebitNoteRegister[rows, 3].AddController(dblClick);

                        //    j = dtDebitNoteDtlInfo.Rows.Count;//For showing only one LedgerName of Creditor on grid and to be out from this loop
                        //}

                    //}
                    //catch (Exception ex)
                    //{

                    //    MessageBox.Show(ex.Message);
                    //}

                //}

            }

            #region ANOTHER METHOD

            //DataTable dtDebNoteMasterInfo = DebitNote.GetDebNoteMasterInfo(m_DebNoteRegister.FromDate, m_DebNoteRegister.ToDate);
            //for (int i = 1; i <=dtDebNoteMasterInfo.Rows.Count; i++)
            //{

            //    DataRow drDebNoteMasterInfo = dtDebNoteMasterInfo.Rows[i - 1];

            //    int rows = grdDebitNoteRegister.Rows.Count;

            //    grdDebitNoteRegister.Rows.Insert(rows);

            //    //Display VoucherNo on grid
            //    grdDebitNoteRegister[rows, 0] = new SourceGrid.Cells.Cell(rows.ToString());
            //    grdDebitNoteRegister[rows, 0].AddController(dblClick);

            //    grdDebitNoteRegister[rows, 1] = new SourceGrid.Cells.Cell(Date.DBToSystem(drDebNoteMasterInfo["DebitNote_Date"].ToString()));
            //    grdDebitNoteRegister[rows, 1].AddController(dblClick);

            //    grdDebitNoteRegister[rows, 2] = new SourceGrid.Cells.Cell(drDebNoteMasterInfo["Voucher_No"].ToString());
            //    grdDebitNoteRegister[rows, 2].AddController(dblClick);

            //    grdDebitNoteRegister[rows, 5] = new SourceGrid.Cells.Cell(drDebNoteMasterInfo["DebitNoteID"].ToString());
            //    grdDebitNoteRegister[rows, 5].Column.Visible = false;

            //    //According to DebitNoteID find the information of tblDebitNoteDetail where DeCr=Debit
                

            //    DataTable dtDebitNoteDtlInfo = DebitNote.GetDebNoteDtlInfo(Convert.ToInt32(drDebNoteMasterInfo["DebitNoteID"]));
            //    double TotalDebitAmt = 0;
            //    for (int k = 1; k <= dtDebitNoteDtlInfo.Rows.Count; k++)
            //    { 
                
            //        DataRow dr = dtDebitNoteDtlInfo.Rows[k-1];
            //        TotalDebitAmt += Convert.ToDouble(dr["Amount"]);
                
                
                
            //    }

            //    grdDebitNoteRegister[rows, 4] = new SourceGrid.Cells.Cell(TotalDebitAmt.ToString());
            //    grdDebitNoteRegister[rows, 4].AddController(dblClick);
              


            //        //Among All ledgers where DrCr= Debit,collect only thoes ledgers which consists under Creditor AccountGroup 
            //        for (int j = 1; j <= dtDebitNoteDtlInfo.Rows.Count; j++)
            //        {

            //            DataRow drDebitNoteDtlInfo = dtDebitNoteDtlInfo.Rows[j - 1];

            //            bool Found = false;
            //            try
            //            {

            //                Found = AccountGroup.IsLedgerUnderGroup(Convert.ToInt32(drDebitNoteDtlInfo["LedgerID"]), 118);//Manually passing the GroupID of Creditor Which is equivalent to 118
            //                if (Found == true)
            //                {


            //                    //Write the name in the grid
            //                    DataTable dt = Ledger.GetLedgerInfo(Convert.ToInt32(drDebitNoteDtlInfo["LedgerID"]), LangMgr.Language);

            //                    DataRow dr = dt.Rows[0];

            //                    grdDebitNoteRegister[rows, 3] = new SourceGrid.Cells.Cell(dr["LedName"].ToString());
            //                    grdDebitNoteRegister[rows, 3].AddController(dblClick);

            //                    j = dtDebitNoteDtlInfo.Rows.Count;//For showing only one LedgerName of Creditor on grid and to be out from this loop



            //                }


            //            }
            //            catch (Exception ex)
            //            {

            //                MessageBox.Show(ex.Message);
            //            }

                       

            //        }

            //}

            #endregion 

        }

        private void WriteHeader()
        { 

            //Define the HeaderPart of sourceGridView

            grdDebitNoteRegister[0, 0] = new MyHeader("S. No.");
            grdDebitNoteRegister[0, 1] = new MyHeader("Date");
            grdDebitNoteRegister[0, 2] = new MyHeader("Voucher No");
            grdDebitNoteRegister[0, 3] = new MyHeader("Account");
            grdDebitNoteRegister[0, 4] = new MyHeader("Total Amount");
            grdDebitNoteRegister[0, 5] = new MyHeader("DebitNoteID");
            
            //Define size of column of Grid
            grdDebitNoteRegister[0, 0].Column.Width = 50;
            grdDebitNoteRegister[0, 1].Column.Width = 150;
            grdDebitNoteRegister[0, 2].Column.Width = 150;
            grdDebitNoteRegister[0, 3].Column.Width = 300;
            grdDebitNoteRegister[0, 4].Column.Width = 150;
            grdDebitNoteRegister[0, 5].Column.Width = 100;

            grdDebitNoteRegister[0, 5].Column.Visible = false;
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
            if (m_DebNoteRegister.HasDateRange == true)
            {
                lblFromDate.Text = Date.ToSystem(m_DebNoteRegister.FromDate);
                lblToDate.Text = Date.ToSystem(m_DebNoteRegister.ToDate);
                lblAsonDate.Visible = false;
            }
            else
            {
                lblFromDate.Visible = false;
                lblToDate.Visible = false;
                label2.Visible = false;
                label4.Visible = false;
                lblAsonDate.Text ="As On Date"+" "+ Date.ToSystem( Date.GetServerDate());
            }
            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_DebNoteRegister.ProjectID), LangMgr.DefaultLanguage);
            if (m_DebNoteRegister.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }




        }   
        private void DebitNoteRegister_DoubleClick(object sender, EventArgs e)
        {
            try
            {


                //Get the Selected Row

                int CurRow = grdDebitNoteRegister.Selection.GetSelectionRegion().GetRowsIndex()[0];

                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdDebitNoteRegister, new SourceGrid.Position(CurRow, 5));

                int DebitNoteID = Convert.ToInt32(cellType.Value);
                frmDebitNote frm = new frmDebitNote(DebitNoteID);
                frm.Show();
               
            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }

        private void frmDebitNoteRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
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

            dsDebitNote.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed


            rptDebitNote rpt = new rptDebitNote();
          
            //Fill the logo on the report

            Misc.WriteLogo(dsDebitNote, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsDebitNote);

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



                pdvReport_Head.Value = "Debit Note";
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

                if (m_DebNoteRegister.HasDateRange == true)
                {
                    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_DebNoteRegister.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_DebNoteRegister.ToDate));
                }
                else
                {
                    pdvReport_Date.Value = "As of now";
                }
                //if (m_DebNoteRegister.FromDate != null && m_DebNoteRegister.ToDate != null)
                //{
                //    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_DebNoteRegister.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_DebNoteRegister.ToDate));
                //}
                //if (m_DebNoteRegister.ToDate != null)
                //{
                //    pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_DebNoteRegister.ToDate));
                //}
                //if (m_DebNoteRegister.FromDate != null)
                //{
                //    //This is actually not applicable
                //    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_DebNoteRegister.FromDate));
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
        private void LoadDatainDataSet()
        {
            for (int i = 0; i < grdDebitNoteRegister.Rows.Count - 1; i++)
            {
                dsDebitNote.Tables["tblDebitNote"].Rows.Add(grdDebitNoteRegister[i+1,0].Value,grdDebitNoteRegister[i+1,1].Value,grdDebitNoteRegister[i+1,2].Value,grdDebitNoteRegister[i+1,3].Value,grdDebitNoteRegister[i+1,4].Value);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            
            //Clear all rows
            grdDebitNoteRegister.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmDebitNoteRegister_Load(sender,e);

            this.Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.DirectPrint);
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
      
    }
}
