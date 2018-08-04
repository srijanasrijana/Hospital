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
using Inventory.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DateManager;
using System.Threading;
using Common;
using AccSwift.CrystalReports;

namespace AccSwift.Forms
{
    public partial class frmLedgerDetails : Form
    {
        private int RowCount = 0;
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        private DataSet.dsLedgerDetails dsLedgerDetails = new DataSet.dsLedgerDetails();
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport
        }

        private string FileName = "";
        public frmLedgerDetails()
        {
            InitializeComponent();
        }

        private void frmLedgerDetails_Load(object sender, EventArgs e)
        {
            MakeHeader();
            GetDetails(false);
        }
        private void GetDetails(bool isCrystalReport)
        {
            //For Current Assests Information
            DataTable dt = AccountGroup.GetGroupTable(6);
            foreach (DataRow dr in dt.Rows)
            {
                string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());

                WriteGroupHead(EngName, isCrystalReport);
                DataTable dtDtlLedgerID = AccountGroup.GetDetailLedgerID(Convert.ToInt32(dr["GroupID"].ToString()), true);
               // RowCount++;
                foreach (DataRow drledger in dtDtlLedgerID.Rows)
                {
                    WriteLedger(drledger["EngName"].ToString(), drledger["PersonName"].ToString(), drledger["Phone"].ToString(), drledger["Email"].ToString(), drledger["VatPanNo"].ToString(), isCrystalReport);
                }
            }
            string OwnersFund = AccountGroup.GetEngName("27");
            WriteGroupHead(OwnersFund,isCrystalReport);
            DataTable dtDtlLedgerID1 = AccountGroup.GetDetailLedgerID(27, true);
            foreach (DataRow drledger in dtDtlLedgerID1.Rows)
            {
                if(drledger["LedgerID"].ToString()!="477")
                WriteLedger(drledger["EngName"].ToString(), drledger["PersonName"].ToString(), drledger["Phone"].ToString(), drledger["Email"].ToString(), drledger["VatPanNo"].ToString(),isCrystalReport);
            }
            DataTable dt1 = AccountGroup.GetGroupTable(118);
            foreach (DataRow dr1 in dt1.Rows)
            {
                string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr1["GroupID"]).ToString());
                if (dr1["GroupID"].ToString() != "140")
                {
                    WriteGroupHead(EngName,isCrystalReport);
                    DataTable dtDtlLedgerID = AccountGroup.GetDetailLedgerID(Convert.ToInt32(dr1["GroupID"].ToString()), true);
                    // RowCount++;
                    foreach (DataRow drledger1 in dtDtlLedgerID.Rows)
                    {
                        WriteLedger(drledger1["EngName"].ToString(), drledger1["PersonName"].ToString(), drledger1["Phone"].ToString(), drledger1["Email"].ToString(), drledger1["VatPanNo"].ToString(),isCrystalReport);
                    }
                }
            }

            //string CurrentLiabilities = AccountGroup.GetEngName("118");
            //WriteGroupHead(OwnersFund);
            //DataTable dtDtlLedgerID2 = AccountGroup.GetDetailLedgerID(118, true);
            //foreach (DataRow drledger in dtDtlLedgerID1.Rows)
            //{
            //    if (drledger["LedgerID"].ToString() != "477")
            //        WriteLedger(drledger["EngName"].ToString(), drledger["PersonName"].ToString(), drledger["Phone"].ToString(), drledger["Email"].ToString(), drledger["VatPanNo"].ToString());
            //}
        }
        private void WriteGroupHead(string GroupName,bool isCrystalReport)
        {
            if (!isCrystalReport)
            {
                GroupView = new SourceGrid.Cells.Views.Cell();
                //GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
                GroupView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                GroupView.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
                GroupView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                GroupView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                grdledgerdetails.Rows.Insert(grdledgerdetails.RowsCount);
                // Block for getting GroupName            

                grdledgerdetails[RowCount + 1, 0] = new SourceGrid.Cells.Cell(GroupName);
                grdledgerdetails[RowCount + 1, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
                grdledgerdetails[RowCount + 1, 0].ColumnSpan = grdledgerdetails.ColumnsCount;
                //grdledgerdetails[RowCount + 1, 1] = new SourceGrid.Cells.Cell("");
                //grdledgerdetails[RowCount + 1, 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
                //grdledgerdetails[RowCount + 1, 2] = new SourceGrid.Cells.Cell("");
                //grdledgerdetails[RowCount + 1, 2].View = new SourceGrid.Cells.Views.Cell(GroupView);
                //grdledgerdetails[RowCount + 1, 3] = new SourceGrid.Cells.Cell("");
                //grdledgerdetails[RowCount + 1, 3].View = new SourceGrid.Cells.Views.Cell(GroupView);
                //grdledgerdetails[RowCount + 1, 4] = new SourceGrid.Cells.Cell("");
                //grdledgerdetails[RowCount + 1, 4].View = new SourceGrid.Cells.Views.Cell(GroupView);
                RowCount++;
            }
            else
            {
                dsLedgerDetails.Tables["tblLedgerDetails"].Rows.Add("", "", "   "+GroupName.ToUpper(), "", "");
            }
        }
        private void WriteLedger(string LedgerName,string ContactPerson,string Phone,string Email,string pannumber,bool isCrystalReport)
        {
            if (!isCrystalReport)
            {
                LedgerView = new SourceGrid.Cells.Views.Cell();
                LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                LedgerView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
                LedgerView.ForeColor = Color.Blue;
                int grdrow = grdledgerdetails.Rows.Count - 1;
                grdledgerdetails.Rows.Insert(grdledgerdetails.RowsCount);

                grdledgerdetails[grdrow, 0] = new SourceGrid.Cells.Cell(LedgerName);
                grdledgerdetails[grdrow, 0].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                grdledgerdetails[grdrow, 1] = new SourceGrid.Cells.Cell(ContactPerson);
                grdledgerdetails[grdrow, 1].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                grdledgerdetails[grdrow, 2] = new SourceGrid.Cells.Cell(Phone);
                grdledgerdetails[grdrow, 2].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                grdledgerdetails[grdrow, 3] = new SourceGrid.Cells.Cell(Email);
                grdledgerdetails[grdrow, 3].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                grdledgerdetails[grdrow, 4] = new SourceGrid.Cells.Cell(pannumber);
                grdledgerdetails[grdrow, 4].View = new SourceGrid.Cells.Views.Cell(LedgerView);
                RowCount++;
            }
            else
            {
                dsLedgerDetails.Tables["tblLedgerDetails"].Rows.Add(LedgerName, ContactPerson, Phone, Email, pannumber);
            }
        }
        private void MakeHeader()
        {
            grdledgerdetails.Rows.Clear();
            grdledgerdetails.Redim(1, 5);
            grdledgerdetails.Rows.Insert(0);
            grdledgerdetails[0, 0] = new MyHeader("Ledger Name");
            grdledgerdetails[0, 1] = new MyHeader("Contact Person");
            grdledgerdetails[0, 2] = new MyHeader("Phone");
            grdledgerdetails[0, 3] = new MyHeader("Email");
            grdledgerdetails[0, 4] = new MyHeader("PAN Number");
           

            //Define the width of column size
            grdledgerdetails[0, 0].Column.Width = 100;
            grdledgerdetails[0, 1].Column.Width = 100;
            grdledgerdetails[0, 2].Column.Width = 120;
            grdledgerdetails[0, 3].Column.Width = 250;
            grdledgerdetails[0, 4].Column.Width = 100;
         
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

            dsLedgerDetails.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

            rptLedgerDetails rpt = new rptLedgerDetails();
            //Fill the logo on the report

          //  Misc.WriteLogo(dsLedgerDetails, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsLedgerDetails);
          
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

                pdvReport_Head.Value = "Contact Details";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
                if (m_CompanyDetails.FYFrom != null)
                    pdvFiscal_Year.Value = "Fiscal Year:" + Date.ToSystem(Convert.ToDateTime(m_CompanyDetails.FYFrom));

                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                pvCollection.Clear();

                DateTime rptdate;
                rptdate = DateTime.Now;
                pdvReport_Date.Value = Date.ToSystem(rptdate);
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);
               
                GetDetails(true);
           // DisplayTrialBlance(true);
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
                        rpt.Close();
                 
                    return;
                default: //Crystal Report

                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;

                    break;
            }

            this.Cursor = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
