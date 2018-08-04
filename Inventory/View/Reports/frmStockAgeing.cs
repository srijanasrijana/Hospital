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
using CrystalDecisions.Shared;
using System.Threading;
using BusinessLogic.Inventory.Reports;
using Inventory;
using Common;
namespace Inventory.View.Reports
{
    public partial class frmStockAgeing : Form
    { 
        //For Export Menu
        ContextMenu Menu_Export;
        BusinessLogic.Inventory.Reports.StockAgingSetting m_stockAging = new BusinessLogic.Inventory.Reports.StockAgingSetting();
        ArrayList ProjectIDs = new ArrayList();
        private int prntDirect = 0;
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
        public frmStockAgeing(StockAgingSetting stockAging)
        {
            InitializeComponent();
            m_stockAging = stockAging; 
        }
       
        public frmStockAgeing()
        {
            InitializeComponent();
        }

        private void frmStockAgeing_Load(object sender, EventArgs e)
        {
            DisplayStockAgeing(false);
        }
        private void DisplayStockAgeing(bool IsCrystalReport)
        {
            if (!IsCrystalReport)//Orientaion Purpose is essential only for SOurcegrid not for Crystal report
            {
                DisplayBannar();
                //Let the whole row to be selected
                grdstockageing.SelectionMode = SourceGrid.GridSelectionMode.Row;

                //Disable multiple selection
                grdstockageing.Selection.EnableMultiSelection = false;
                //Disable multiple selection
                grdstockageing.Redim(2, 8);
                int rows = grdstockageing.Rows.Count;
                // grdstockageing.Rows.Insert(rows);
                MakeHeader();

            }
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void DisplayBannar()
        {
            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear;


            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_stockAging.ProjectID), LangMgr.DefaultLanguage);
            if (m_stockAging.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
            lblasondate.Text = "As" + " " + "On" + " " + Date.ToSystem(m_stockAging.AtTheEndDate);
        }
        private void MakeHeader()
        {
            //Defining the HeaderPart       
            grdstockageing[0, 0] = new MyHeader("Code");
            grdstockageing[0, 1] = new MyHeader("Product Name");
            grdstockageing[0, 2] = new MyHeader("<15Days");
            grdstockageing[0, 3] = new MyHeader("<30Days");
            grdstockageing[0, 4] = new MyHeader("<45Days");
            grdstockageing[0, 5] = new MyHeader("<60Days");
            grdstockageing[0, 6] = new MyHeader(">60Days");
            grdstockageing[0, 7] = new MyHeader("Total");

            //Define size of column
            grdstockageing[0, 0].Column.Width = 100;
            grdstockageing[0, 1].Column.Width = 180;
            grdstockageing[0, 2].Column.Width = 80;
            grdstockageing[0, 3].Column.Width = 80;
            grdstockageing[0, 4].Column.Width = 80;
            grdstockageing[0, 5].Column.Width = 80;
            grdstockageing[0, 6].Column.Width = 80;
            grdstockageing[0, 7].Column.Width = 100;
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

                    PrintPreviewCR(PrintType.Email);
                    break;

            }

        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.DirectPrint);
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

            //  dsPurchaseReport.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed


            //rptBalanceSheet rpt = new rptBalanceSheet();
            // rptPurchaseReportByProduct m_rptPurchaseReportByProduct = new rptPurchaseReportByProduct();



            //Fill the logo on the report
            //Misc.WriteLogo(dsPurchaseReport, "tblImage");
            // Set DataSource to be dsTrial dataset on the report
            // rpt.SetDataSource(dsPurchaseReport);
            //if (m_PurchaseReport.IsProductwise)
            //{
            //    Misc.WriteLogo(dsPurchaseReport, "tblImage");
            //    m_rptPurchaseReportByProduct.SetDataSource(dsPurchaseReport);
            //}


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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPrintDate = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvTotalAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();

            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);

            //m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);



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

            //m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

            pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are availablepdvCompany_Address.Value = m_CompanyDetails.Address1;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Address);
            //m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);


            pdvCompany_PAN.Value = m_CompanyDetails.PAN;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_PAN);

            //m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);
            pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Phone);

            // m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);




            pdvCompany_Slogan.Value = m_CompanyDetails.Website;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Slogan);

            //m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

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
           // rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

            pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Address);
            //rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

            pdvCompany_PAN.Value = companypan;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_PAN);
           // rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

            pdvCompany_Phone.Value = "Phone No.: " + companyphone;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Phone);
           // rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

            pdvCompany_Slogan.Value = companyslogan;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Slogan);
           // rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

        }

            pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvPreparedBy);
            //  m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

            pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvCheckedBy);
            //  m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

            pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvApprovedBy);
            // m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);

            pdvPrintDate.Value = Date.ToSystem(DateTime.Now);
            pvCollection.Clear();
            pvCollection.Add(pdvPrintDate);
            //  m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);


            pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Phone);

            // m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);




            pdvCompany_Slogan.Value = m_CompanyDetails.Website;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Slogan);

            //m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);



            pdvReport_Head.Value = "Stock Ageing";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            //  m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);



            pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);

            // m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);



            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");



            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);

            // m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);



            // DisplayPurchaseReport(true);

            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

            //Finally, show the report form
            Common.frmReportViewer frm = new Common.frmReportViewer();

            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");

            // Close the dialog
            ProgressForm.CloseForm();



            this.Cursor = Cursors.Default;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows
            grdstockageing.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmStockAgeing_Load(sender, e);
            grdstockageing.Refresh();

            this.Cursor = Cursors.Default;
        }
        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            //foreach (object j in m_PurchaseReport.AccClassID)
            //{
            //    AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            //}
            //m_PurchaseReport.AccClassID.AddRange(arrChildAccClassIDs);

            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("PURCHASEREPORT");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ACCCLASSIDS");
                    //foreach (string tag in m_PurchaseReport.AccClassID)
                    //{
                    //    AccClassID.Add(Convert.ToInt32(tag));
                    //    tw.WriteElementString("AccID", Convert.ToInt32(tag).ToString());
                    //}
                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            return strXML;
        }

        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            //  Project.GetChildProjects(Convert.ToInt32(m_PurchaseReport.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            // string ProjectIDS = "'" + m_PurchaseReport.ProjectID + "'";

            for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
            {
                //ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
            }
            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("PURCHASEREPORT");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("PROJECTIDS");
                    //tw.WriteElementString("PID", Convert.ToInt32(m_PurchaseReport.ProjectID).ToString());
                    foreach (string tag in ProjectIDCollection)
                    {
                        //AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("PID", Convert.ToInt32(tag).ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            return strXML;

        }

        private void WriteStockAgeingReport(bool isCrystalReport)
        {
            if (!isCrystalReport)
            {

            }
            else
            {

            }
        }
    }
}
