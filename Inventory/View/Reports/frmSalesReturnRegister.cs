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
using Common;
using System.Threading;
using CrystalDecisions.Shared;
using System.Collections;
using Inventory.Reports;
using SourceGrid.Selection;


namespace Inventory
{
    public partial class frmSalesReturnRegister : Form
    {
        public frmSalesReturnRegister()
        {
            InitializeComponent();
        }
        public frmSalesReturnRegister(InventoryBookSettings SRR)
        {
            InitializeComponent();
            m_SRR = new InventoryBookSettings();
            m_SRR = SRR;
        }
        private Inventory.Model.dsInventoryBookReport dsInventoryBookReport = new Model.dsInventoryBookReport();
        private string FileName = "";
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private InventoryBookSettings m_SRR;
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell subGroupView;
        ArrayList AccClassID = new ArrayList();
        Decimal Amount = 0;
        int TotalInBound = 0;
        int TotalOutBound = 0;
        private DataTable tblBufferSalesRegister;
        private decimal OpeningQty = 0;
        bool IsCrystalReport;
        private string UnitSymbol = "";
        private SourceGrid.Cells.Views.Cell LedgerView;
        private int AddressBookTransactRowsCount;    
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;
        string totalRptAmount = "";
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
        private void frmSalesReturnRegister_Load(object sender, EventArgs e)
        {
            try
            {
                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                dblClick.DoubleClick += new EventHandler(dgSalesReturnRegister_DoubleClick);

                gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
                dgSalesReturnRegister.Controller.AddController(gridKeyDown);
                //Let the whole row to be selected
                dgSalesReturnRegister.SelectionMode = SourceGrid.GridSelectionMode.Row;
                //Set Border
                DevAge.Drawing.RectangleBorder b = Selection.Border;
                b.SetWidth(0);
                Selection.Border = b;
                //Disable multiple selection
                dgSalesReturnRegister.Selection.EnableMultiSelection = false;
                DisplayBannar();
                ////Text format for Total
                GroupView = new SourceGrid.Cells.Views.Cell();
                GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                ////Text format for Ledgers
                LedgerView = new SourceGrid.Cells.Views.Cell();
                LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                LedgerView.ForeColor = Color.Blue;
                AddressBookTransactRowsCount = 1;
                dgSalesReturnRegister.Visible = true;
                //#endregion
                tblBufferSalesRegister = new DataTable();
                tblBufferSalesRegister.Columns.Add("SN", typeof(int));
                tblBufferSalesRegister.Columns.Add("Date", typeof(string));
                tblBufferSalesRegister.Columns.Add("VoucherType", typeof(string));
                tblBufferSalesRegister.Columns.Add("VoucherNo", typeof(string));
                tblBufferSalesRegister.Columns.Add("ProductOrParty", typeof(string));
                tblBufferSalesRegister.Columns.Add("ProductName", typeof(string));
                tblBufferSalesRegister.Columns.Add("InBoundQty", typeof(string));
                tblBufferSalesRegister.Columns.Add("OutBoundQty", typeof(string));
                tblBufferSalesRegister.Columns.Add("Unit", typeof(string));
                tblBufferSalesRegister.Columns.Add("Amount", typeof(string));
                tblBufferSalesRegister.Columns.Add("RowID", typeof(int));
                tblBufferSalesRegister.Columns.Add("PartyID", typeof(int));

                #region BLOCK FOR Inventory Sales register SHOW

                dgSalesReturnRegister.Visible = true;
                DisplayInventorySalesReturnRegister(false);
                #endregion
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            lblCompanyName.Text = m_CompanyDetails.CompanyName;
            lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            lblContact.Text = "Contact: " + m_CompanyDetails.Telephone; 
            lblWebsite.Text=  "Website: " + m_CompanyDetails.Website;
            lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;

            //If it has a date range
            if (m_SRR.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_SRR.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear; 

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_SRR.ProjectID), LangMgr.DefaultLanguage);
            if (m_SRR.ProjectID != null)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = "Project Name: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = "Project Name: "+"All";

            }
        }    
        private string ReadAllAccClassID()
        {
            #region  AccountingClassID
            
            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_SRR.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_SRR.AccClassID.AddRange(arrChildAccClassIDs);

            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("INVENTORYBOOK");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("AccClassIDSettings");
                    foreach (string tag in m_SRR.AccClassID)
                    {
                        AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccClassID", Convert.ToInt32(tag).ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { 
                }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            return strXML;
        }
     
        public SelectionBase Selection
        {
            get
            {
                return dgSalesReturnRegister.Selection as SelectionBase;
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
            //viewNumeric.Border = border;
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            SourceGrid.Cells.Views.Cell viewImage = new SourceGrid.Cells.Views.Cell();
            viewImage.Border = border;
            viewImage.ImageStretch = false;
            viewImage.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            viewColumnHeader.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);
            viewColumnHeader.BackColor = Color.LightGray;

            //Create columns
            SourceGrid.DataGridColumn gridColumn;
            gridColumn = dgSalesReturnRegister.Columns.Add("SN", "SN", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 80;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgSalesReturnRegister.Columns.Add("Date", "Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 100;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgSalesReturnRegister.Columns.Add("VoucherType", "Voucher Type", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 100;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgSalesReturnRegister.Columns.Add("VoucherNo", "Voucher No", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 100;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgSalesReturnRegister.Columns.Add("ProductOrParty", "Particulars", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 200;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgSalesReturnRegister.Columns.Add("ProductName", "Product Details ", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 150;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgSalesReturnRegister.Columns.Add("InBoundQty", "SalesReturn Qty", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 120;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;
            gridColumn.HeaderCell.View = viewColumnHeader;
           
            gridColumn = dgSalesReturnRegister.Columns.Add("OutBoundQty", "OutBound Qty", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.Visible = false;

            gridColumn = dgSalesReturnRegister.Columns.Add("Unit", "Unit", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 80;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;
            gridColumn.HeaderCell.View = viewColumnHeader;

            gridColumn = dgSalesReturnRegister.Columns.Add("Amount", "Amount", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 100;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.HeaderCell.View = viewColumnHeader;

            //Hidden columns for storing required information, sp. while doubleclicking
            gridColumn = dgSalesReturnRegister.Columns.Add("RowID", "RowID", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.Visible = false;

            gridColumn = dgSalesReturnRegister.Columns.Add("PartyID", "PartyID", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.Visible = false;

            dgSalesReturnRegister.AutoStretchColumnsToFitWidth = true;
        }
        private void DisplayInventorySalesReturnRegister(bool IsCrystalReport)
        {
            try
            {
                string AccClassIDsXMLString = ReadAllAccClassID();
                string ProjectIDsXMLString = ReadAllProjectID();

                #region NEW ONE
                DataTable dtInventoryDayBookInfo = InventoryBook.GetInventoryBook(m_SRR.partyGroupID, m_SRR.PartyID, m_SRR.ProductGroupID,
                m_SRR.ProductID, m_SRR.DepotID, m_SRR.ProjectID, m_SRR.FromDate, m_SRR.ToDate, InvenotryBookType.SalesReturnRegister,
                AccClassIDsXMLString, ProjectIDsXMLString,ref OpeningQty);
               string DateField = "Date";
                if (Global.Default_Date == Date.DateType.Nepali)
                    DateField = "NepDate";

                var query = from o in dtInventoryDayBookInfo.AsEnumerable()
                            select new
                            {
                                SN = Convert.ToInt32(o.Field<int>("SN")),
                                VoucherDate = o.Field<string>(DateField),
                                //VoucherDate = Convert.ToDateTime(o.Field<DateTime>("Date")).ToShortDateString(),
                                VoucherType = o.Field<string>("VoucherType"),
                                VoucherNumber = o.Field<string>("VoucherNo"),
                                PartyName = Convert.ToString(o.Field<string>("ProductOrParty")),
                                ProductName = Convert.ToString(o.Field<string>("ProductName")),
                                InBound = Convert.ToInt32(o.Field<int>("InBoundQty")),
                                OutBound = Convert.ToInt32(o.Field<int>("OutBoundQty")),
                                Unit = "Unit",
                                Amount = Convert.ToDecimal(o.Field<decimal>("Amount")).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),
                                RowID = o.Field<int>("RowID"),
                                PartyID = o.Field<int>("PartyID")
                            };

                foreach (var dr in query)
                {
                    Amount += Convert.ToDecimal(dr.Amount);
                    TotalInBound += Convert.ToInt32(dr.InBound);
                    TotalOutBound += Convert.ToInt32(dr.OutBound);
                    tblBufferSalesRegister.Rows.Add(dr.SN,dr.VoucherDate, dr.VoucherType, dr.VoucherNumber, dr.PartyName, dr.ProductName, dr.InBound, dr.OutBound, dr.Unit, dr.Amount, dr.RowID, dr.PartyID);
                }
                lblOutBoundQty.Text = "OutBound Qty=" + TotalOutBound.ToString();
                lbInboundQty.Text = "SaleReturn Qty=" + TotalInBound.ToString();
                lblTotalAmt.Text = "Amount= " + "Rs. " + Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblClosingQty.Text = "Closing Qty= " + (OpeningQty + TotalInBound - TotalOutBound).
                                     ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblAmountInWord.Text = "Amount In word:  " + AmountToWords.ConvertNumberAsText(Convert.ToString(Amount));
                lblOpeningQty.Text = "Opening Qty= " + OpeningQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                #endregion
                #region datagrid binding
                DataView mView;
                mView = tblBufferSalesRegister.DefaultView;
                dgSalesReturnRegister.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
                dgSalesReturnRegister.FixedRows = 1; //Al/Allocated for Header
                //Fixed Column at first
                dgSalesReturnRegister.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dgSalesReturnRegister));
                DevAge.ComponentModel.IBoundList bindList = new DevAge.ComponentModel.BoundDataView(mView);
                //Create default columns
                CreateColumns(dgSalesReturnRegister.Columns, bindList);
                dgSalesReturnRegister.DataSource = bindList;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
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
            dsInventoryBookReport.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
            //otherwise it populate the records again and again              
            rptInventoryBookReport m_rptInventoryBook = new rptInventoryBookReport();
            //Fill the logo on the report
            Misc.WriteLogo(dsInventoryBookReport, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            m_rptInventoryBook.SetDataSource(dsInventoryBookReport);
            try
            {
                dsInventoryBookReport.Tables.Remove("tblInventoryBook");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            System.Data.DataView view = new System.Data.DataView(tblBufferSalesRegister);
            DataTable selected = view.ToTable("tblInventoryBook", false, "PartyID", "Date", "VoucherType", "VoucherNo", "ProductOrParty","ProductName", "InBoundQty", "OutBoundQty", "Amount");
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
 
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvInBoundQty = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvOutBoundQty = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPartyProductName = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPrintDate = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvProductName = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvClosingQuantiy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvOpeningQuantity = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvTotalAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            // if (m_PurchaseReport.IsProductwise)
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
            pdvReport_Head.Value = "Sales Return Register Report";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            m_rptInventoryBook.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            pdvFiscal_Year.Value = "Fiscal Year:" + (m_CompanyDetails.FiscalYear);
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);
           m_rptInventoryBook.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            pdvProductName.Value = "Sales Return Register Product";
            pvCollection.Clear();
            pvCollection.Add(pdvProductName);
            m_rptInventoryBook.DataDefinition.ParameterFields["ProductName"].ApplyCurrentValues(pvCollection);

            pdvPartyProductName.Value = "Particulars";
            pvCollection.Clear();
            pvCollection.Add(pdvPartyProductName);
            m_rptInventoryBook.DataDefinition.ParameterFields["PartyProductName"].ApplyCurrentValues(pvCollection);

            pdvInBoundQty.Value = TotalInBound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvInBoundQty);
            m_rptInventoryBook.DataDefinition.ParameterFields["InBoundQty"].ApplyCurrentValues(pvCollection);

            pdvOutBoundQty.Value = TotalOutBound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvOutBoundQty);
            m_rptInventoryBook.DataDefinition.ParameterFields["OutBoundQty"].ApplyCurrentValues(pvCollection);

            pdvClosingQuantiy.Value = "Closing Quantity=" + (OpeningQty + TotalInBound - TotalOutBound).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvClosingQuantiy);
            m_rptInventoryBook.DataDefinition.ParameterFields["ClosingQuantity"].ApplyCurrentValues(pvCollection);

            pdvOpeningQuantity.Value = "Opening Quantity=" + OpeningQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvOpeningQuantity);
            m_rptInventoryBook.DataDefinition.ParameterFields["OpeningQuantity"].ApplyCurrentValues(pvCollection);

            pdvPrintDate.Value = Convert.ToDateTime(DateTime.Now).ToShortDateString();
            pvCollection.Clear();
            pvCollection.Add(pdvPrintDate);
            m_rptInventoryBook.DataDefinition.ParameterFields["PrintDate"].ApplyCurrentValues(pvCollection);

            pdvTotalAmount.Value = Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvTotalAmount);
            m_rptInventoryBook.DataDefinition.ParameterFields["Total_Amt"].ApplyCurrentValues(pvCollection);

            m_rptInventoryBook.SetParameterValue("Total_Amt", Amount);
            string inwords = AmountToWords.ConvertNumberAsText(Convert.ToString(Amount));
            m_rptInventoryBook.SetParameterValue("AmtInWords", inwords);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");
            //Display the date in crystal report according to available from and to dates
            if (m_SRR.FromDate != null && m_SRR.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SRR.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_SRR.ToDate));
            }
            if (m_SRR.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_SRR.ToDate));
            }
            if (m_SRR.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SRR.FromDate));
            }
            if (m_SRR.FromDate == null && m_SRR.ToDate == null)
            {
                pdvReport_Date.Value = "";

            }
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);

            m_rptInventoryBook.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);
            DisplayInventorySalesReturnRegister(true);

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
                case PrintType.DirectPrint: //Direct Printer
                    m_rptInventoryBook.PrintOptions.PrinterName = "";
                    m_rptInventoryBook.PrintToPrinter(1, false, 0, 0);
                    return;
                case PrintType.Excel: //Excel
                    ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                    CrExportOptions = m_rptInventoryBook.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    m_rptInventoryBook.Export();
                    m_rptInventoryBook.Close();
                    return;
                case PrintType.PDF: //PDF
                    PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                    CrExportOptions = m_rptInventoryBook.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                    m_rptInventoryBook.Export();
                    m_rptInventoryBook.Close();
                    return;
                case PrintType.Email:
                    ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                    CrExportOptions = m_rptInventoryBook.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                    m_rptInventoryBook.Export();
                    Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                    sendemail.ShowDialog();
                    m_rptInventoryBook.Close();
                    return;
                default: //Crystal Report
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }

            this.Cursor = Cursors.Default;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows       
            dgSalesReturnRegister.Columns.Clear();
            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmSalesReturnRegister_Load(sender, e);
            dgSalesReturnRegister.Refresh();
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

        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_SRR.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }
            string ProjectIDS = "'" + m_SRR.ProjectID + "'";
            for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
            {
                ProjectIDS += "," + "'" + (ProjectIDCollection[iproject].ToString()) + "'";
            }
            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("INVENTORYBOOK");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ProjectIDSettings");
                    tw.WriteElementString("ProjectID", Convert.ToInt32(m_SRR.ProjectID).ToString());
                    foreach (string tag in ProjectIDCollection)
                    {
                        //AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("ProjectID", Convert.ToInt32(tag).ToString());
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
        private void dgSalesReturnRegister_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                int CurRow = dgSalesReturnRegister.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(dgSalesReturnRegister, new SourceGrid.Position(CurRow, 11));
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgSalesReturnRegister, new SourceGrid.Position(CurRow, 3));
                string Type = (cellType.Value).ToString();
                if ((cellType.Value.ToString()) != "")//Dont Call the voucher form if there is no Ledger...no need to call Voucher form for Op. Bal/Total Amount etc
                {
                    int RowID = Convert.ToInt32(cellTypeID.Value);
                    string VoucherType = cellType.Value.ToString();
                    switch (VoucherType)
                    {

                        case "SALES":
                            frmSalesInvoice frm = new frmSalesInvoice(RowID);
                            frm.ShowDialog();
                            break;
                        case "SALES_RTN":
                            frmSalesReturn frm1 = new frmSalesReturn(RowID);
                            frm1.ShowDialog();
                            break;
                        case "PURCH":
                            frmPurchaseInvoice frm2 = new frmPurchaseInvoice(RowID);
                            frm2.ShowDialog();
                            break;
                        case "PURCH_RTN":
                            frmPurchaseReturn frm3 = new frmPurchaseReturn(RowID);
                            frm3.ShowDialog();
                            break;

                    }


                }
            }

            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.DirectPrint);

        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }   
    }
}
