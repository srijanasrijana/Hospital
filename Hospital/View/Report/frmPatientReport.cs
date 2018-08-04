using BusinessLogic;
using BusinessLogic.HOS;
using BusinessLogic.HOS.Report;
using Common;
using DateManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using SourceGrid.Selection;
using System.Windows.Forms;
using Hospital.Report;
using CrystalDecisions.Shared;

namespace Hospital.View.Report
{
    public partial class frmPatientReport : Form
    {
        public frmPatientReport()
        {
            InitializeComponent();
        }

        DataTable dtTemp;
        DataTable dtPatient;
        private DataTable tblBufferSalesRegister;
        private PatientReportSetting m_SR;
        DataView mView;
        public frmPatientReport(PatientReportSetting SR)
        {
            InitializeComponent();
            m_SR = new PatientReportSetting();
            m_SR = SR;

        }
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        private Hospital.Model.dsPatientGeneralReport dsInventoryBookReport = new Model.dsPatientGeneralReport();
        private string FileName = "";
        private int myPrintType = 0;
        public SelectionBase Selection
        {
            get
            {
                return dgPatientRegister.Selection as SelectionBase;
            }
        }
        private void frmPatientReport_Load(object sender, EventArgs e)
        {

            try
            {

                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                //    dblClick.DoubleClick += new EventHandler(dgSalesRegister_DoubleClick);

                gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
                dgPatientRegister.Controller.AddController(gridKeyDown);
                //Let the whole row to be selected
                dgPatientRegister.SelectionMode = SourceGrid.GridSelectionMode.Row;
                //Set Border
                DevAge.Drawing.RectangleBorder b = Selection.Border;
                b.SetWidth(0);
                Selection.Border = b;
                //Disable multiple selection
                dgPatientRegister.Selection.EnableMultiSelection = false;
                DisplayBannar();
                //Text format for Total
                GroupView = new SourceGrid.Cells.Views.Cell();
                GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                ////Text format for Ledgers
                LedgerView = new SourceGrid.Cells.Views.Cell();
                LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                LedgerView.ForeColor = Color.Blue;

                dgPatientRegister.Visible = true;

                tblBufferSalesRegister = new DataTable();
                //tblBufferSalesRegister.Columns.Add("SN", typeof(int));
                tblBufferSalesRegister.Columns.Add("Date", typeof(string));
                tblBufferSalesRegister.Columns.Add("Name", typeof(string));
                tblBufferSalesRegister.Columns.Add("Address", typeof(string));
                tblBufferSalesRegister.Columns.Add("Gender", typeof(string));
                tblBufferSalesRegister.Columns.Add("Age", typeof(string));
                tblBufferSalesRegister.Columns.Add("ContactNo", typeof(string));
                tblBufferSalesRegister.Columns.Add("DoctorConsultant", typeof(string));
                tblBufferSalesRegister.Columns.Add("PatientID", typeof(int));

                #region BLOCK FOR Inventory Sales register SHOW

                dgPatientRegister.Visible = true;

                DisplayPatientRegister(false);
                #endregion
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }



        }
        public void DisplayPatientRegister(bool IsCrystalReport)
        {
            #region datagrid binding

            DataTable dtPatientInfo = Patient.GetPatientInfoReport(m_SR.patientID, m_SR.FromDate, m_SR.ToDate);
            string DateField = "Date";
            if (Global.Default_Date == Date.DateType.Nepali)
                DateField = "Date";
            string text = "";
            //int Gender = Convert.ToInt32("Gender");
            //if(Gender == 1)
            //{
            //    text = "Male";
            //}
            //else if(Gender==2)
            //{
            //    text = "Female";
            //}


            var query = from o in dtPatientInfo.AsEnumerable()
                        select new
                        {
                            // SN = Convert.ToInt32(o.Field<int>("SN")),
                            Date = o.Field<string>(DateField),
                            Name = o.Field<string>("Name"),
                            Address = o.Field<string>("Address"),
                            Gender = Convert.ToInt32(o.Field<int>("Gender")),
                            Age = Convert.ToInt32(o.Field<int>("Age")),
                            ContactNo = Convert.ToString(o.Field<string>("ContactNo")),
                            DoctorConsultant = Convert.ToString(o.Field<string>("DoctorConsultant")),
                            PatientID = o.Field<int>("PatientID")

                        };
            foreach (var dr in query)
            {

                if (dr.Gender == 1)
                {
                    text = "Male";
                }
                else if (dr.Gender == 2)
                {
                    text = "Female";
                }
                else
                {
                    text = "Other";
                }
                tblBufferSalesRegister.Rows.Add(dr.Date, dr.Name, dr.Address, text, dr.Age, dr.ContactNo, dr.DoctorConsultant, dr.PatientID);
            }


            mView = tblBufferSalesRegister.DefaultView;
            dgPatientRegister.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
            dgPatientRegister.FixedRows = 1; //Al/Allocated for Header

            //Fixed Column at first
            dgPatientRegister.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dgPatientRegister));

            DevAge.ComponentModel.IBoundList bindList = new DevAge.ComponentModel.BoundDataView(mView);
            //Create default columns
            CreateColumns(dgPatientRegister.Columns, bindList);
            dgPatientRegister.DataSource = bindList;

            #endregion
        }

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            lblCompanyName.Text = m_CompanyDetails.CompanyName;
            lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            lblContact.Text = "Contact: " + m_CompanyDetails.Telephone;
            lblWebsite.Text = "Website: " + m_CompanyDetails.Website;
            lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;

            //If it has a date range
            if (m_SR.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_SR.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }
            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear;

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_SR.patientID), LangMgr.DefaultLanguage);
            if (m_SR.patientID != null)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = "Project Name: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = "Project Name: " + "All";
            }
        }


        private void CreateColumns(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList bindList)
        {
            //Borders
            DevAge.Drawing.RectangleBorder border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.ForestGreen),
                new DevAge.Drawing.BorderLine(Color.ForestGreen));
            border.SetWidth(1);

            //Standard Views
            SourceGrid.Cells.Views.Cell viewString = new SourceGrid.Cells.Views.Cell();
            viewString.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            viewColumnHeader.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);
            viewColumnHeader.BackColor = Color.LightGray;

            //Create columns
            SourceGrid.DataGridColumn gridColumn;

            //gridColumn = dgSalesRegister.Columns.Add("SN", "SN", new SourceGrid.Cells.DataGrid.Cell());
            //gridColumn.DataCell.View = viewString;
            //gridColumn.DataCell.AddController(dblClick);
            //gridColumn.DataCell.AddController(gridKeyDown);
            //gridColumn.Width = 80;
            //gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            //gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgPatientRegister.Columns.Add("Date", "Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 120;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch; ;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgPatientRegister.Columns.Add("Name", "Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.Width = 130;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgPatientRegister.Columns.Add("Address", "Address ", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.Width = 130;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.HeaderCell.View = viewColumnHeader;


            gridColumn = dgPatientRegister.Columns.Add("Gender", "Gender ", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.Width = 60;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.HeaderCell.View = viewColumnHeader;


            gridColumn = dgPatientRegister.Columns.Add("Age", "Age ", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.Width = 40;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.HeaderCell.View = viewColumnHeader;


            gridColumn = dgPatientRegister.Columns.Add("ContactNo", "ContactNo. ", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.Width = 130;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgPatientRegister.Columns.Add("DoctorConsultant", "DoctorConsultant. ", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.Width = 130;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgPatientRegister.Columns.Add("PatientID", "PatientID", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.Width = 150;
            gridColumn.Visible = false;

            dgPatientRegister.AutoStretchColumnsToFitWidth = true;

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows       
            dgPatientRegister.Columns.Clear();
            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmPatientReport_Load(sender, e);
            dgPatientRegister.Refresh();
            this.Cursor = Cursors.Default;
        }
        public void PrintPatient()
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

                dsInventoryBookReport.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
                //otherwise it populate the records again and again              
                rptPatient m_rptInventoryBook = new rptPatient();
                //Fill the logo on the report
                Misc.WriteLogo(dsInventoryBookReport, "tblImage");
                //Set DataSource to be dsTrial dataset on the report
                m_rptInventoryBook.SetDataSource(dsInventoryBookReport);


                try
                {
                    dsInventoryBookReport.Tables.Remove("tblPatient");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


                System.Data.DataView view = new System.Data.DataView(tblBufferSalesRegister);

                DataTable selected = view.ToTable("tblPatient", false, "PatientID", "Date", "Name", "Address", "Gender", "Age", "ContactNo", "DoctorConsultant");
                dsInventoryBookReport.Tables.Add(selected);


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

                CrystalDecisions.Shared.ParameterDiscreteValue pdvProductName = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvInBoundQty = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvOutBoundQty = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvClosingQuantiy = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvOpeningQuantity = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvPartyProductName = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvPrintDate = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTotalAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

                //Update the progressbar
                ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

                pdvFont.Value = "Arial";
                pvCollection.Clear();
                pvCollection.Add(pdvFont);
                m_rptInventoryBook.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);


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
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                    pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Address);
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                    pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_PAN);
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                    pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Phone);
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                    pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Slogan);
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

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
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                    pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Address);
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                    pdvCompany_PAN.Value = companypan;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_PAN);
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                    pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Phone);
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                    pdvCompany_Slogan.Value = companyslogan;
                    pvCollection.Clear();
                    pvCollection.Add(pdvCompany_Slogan);
                    m_rptInventoryBook.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

                }

                pdvReport_Head.Value = "Patient Register Report";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                m_rptInventoryBook.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

                pdvFiscal_Year.Value = "Fiscal Year:" + Date.ToSystem(m_CompanyDetails.FYFrom);
                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                m_rptInventoryBook.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                pdvProductName.Value = "Patient Register Name";
                pvCollection.Clear();
                pvCollection.Add(pdvProductName);
                m_rptInventoryBook.DataDefinition.ParameterFields["Name"].ApplyCurrentValues(pvCollection);



                pdvPrintDate.Value = Convert.ToDateTime(DateTime.Now).ToShortDateString();
                pvCollection.Clear();
                pvCollection.Add(pdvPrintDate);
                m_rptInventoryBook.DataDefinition.ParameterFields["PrintDate"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                //Display the date in crystal report according to available from and to dates
                if (m_SR.FromDate != null && m_SR.ToDate != null)
                {
                    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SR.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_SR.ToDate));
                }
                if (m_SR.ToDate != null)
                {
                    pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_SR.ToDate));
                }
                if (m_SR.FromDate != null)
                {
                    //This is actually not applicable
                    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SR.FromDate));
                }
                if (m_SR.FromDate == null && m_SR.ToDate == null)
                {
                    pdvReport_Date.Value = "";

                }
                pvCollection.Clear();

                pvCollection.Add(pdvReport_Date);

                m_rptInventoryBook.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;

                //Finally, show the report form
                Common.frmReportViewer frm = new Common.frmReportViewer();

                frm.SetReportSource(m_rptInventoryBook);
                //Update the progressbar
                ProgressForm.UpdateProgress(100, "Showing Report...");

                // Close the dialog
                ProgressForm.CloseForm();
                switch (myPrintType)
                {
                    case 1:
                        m_rptInventoryBook.PrintOptions.PrinterName = "";
                        m_rptInventoryBook.PrintToPrinter(1, false, 0, 0);
                        myPrintType = 0;
                        return;
                    case 2:
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = m_rptInventoryBook.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        m_rptInventoryBook.Export();
                        m_rptInventoryBook.Close();
                        return;
                    case 3:
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = m_rptInventoryBook.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        m_rptInventoryBook.Export();
                        m_rptInventoryBook.Close();
                        return;
                    case 4:
                        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                        CrExportOptions = m_rptInventoryBook.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                        m_rptInventoryBook.Export();
                        frmemail sendemail = new frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        m_rptInventoryBook.Close();
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

                throw ex;
            }




        }
        private void btnPrintPreview_Click(object sender, EventArgs e)
        {

            //PrintPatient();


        }
    }
      
    }

