using BusinessLogic;
using Common.Model;
using Common.Reports;
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

namespace Common
{
    public partial class frmVoucherList : Form, IfrmDateConverter
    {
        SourceGrid.Cells.Controllers.CustomEvents evtDblClick = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Views.Cell alternate = null;
        SourceGrid.Cells.Views.Cell rightAlign = null;
     
        bool isToDate = false;
        bool isEnglishDate = false;
        IVoucherList m_form = null;
        string[] m_VoucherValues = new string[5];
        //string ColumnName = "Voucher_No";
        public int MasterID;
        public static int RecordCount;
        //string FilterStr = "";
        ContextMenu Menu_Export;
        private int prntDirectForPOS = 0;
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
        dsVoucherList dsVoucherList = new dsVoucherList();

        public enum GridColumns { MasterID=0, SN, Transact_Date, Voucher_Type, Voucher_No, Name, Description, Gross_Amt}
        public frmVoucherList()
        {
            InitializeComponent();
        }
        public frmVoucherList(IVoucherList form, string[] VouchValues)
        {
            InitializeComponent();
            m_form = form;
            VouchValues.CopyTo(m_VoucherValues, 0);

        }
        public void LoadCombobox()
        {
            try
            {
                DataTable dtCreatedBy = User.GetUserInfo(0);
                DataRow dr = dtCreatedBy.NewRow();
                dr["UserID"] = 0;
                dr["UserName"] = "Select All";
                dtCreatedBy.Rows.InsertAt(dr, 0);
                cboCreatedBy.DataSource = dtCreatedBy;
                cboCreatedBy.ValueMember = "UserID";
                cboCreatedBy.DisplayMember = "UserName";
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void frmVoucherList_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCombobox();
         
                txtFromDate.Mask = Date.FormatToMask();
                txtToDate.Mask = Date.FormatToMask();

                isEnglishDate = (Global.Default_Date == Date.DateType.English) ? true : false;
                RecordCount = 0;

                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());

                this.Text += " - " + m_VoucherValues[0].Replace('_', ' '); // remove underscore (_) from Voucher name i.e. changes SALES_INVOICE to SALES INVOICE
                if (m_VoucherValues[0].ToString() != "SALES_INVOICE")
                {
                    cboCreatedBy.Visible = false;
                    lblCreatedBy.Visible = false;
                }
                DataTable dt = VoucherList.GetVoucherList("0", m_VoucherValues, Navigate.First.ToString(), "", "", 
                    Date.ToDotNet(txtToDate.Text).ToShortDateString() + " 23:59:59:999",Convert.ToInt32(cboCreatedBy.SelectedValue));
                
                if (dt.Rows.Count == 0)
                {
                    Global.MsgError("No record found.");
                    txtFromDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());

                }

                else
                    txtFromDate.Text = Date.DBToSystem(VoucherList.GetFromDate().ToString());

                LoadVoucherList(dt);
                evtDblClick.DoubleClick += new EventHandler(Row_Double_Click);
                txtSearchValue.Select(); // focus remains on this text box

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void WriteHeaderVouherList()
        {
            grdVoucherList[0, (int)GridColumns.MasterID] = new MyHeader("MasterID");
            grdVoucherList[0, (int)GridColumns.MasterID].Column.Visible = false;

            grdVoucherList[0, (int)GridColumns.SN] = new MyHeader("SN");
            grdVoucherList[0, (int)GridColumns.SN].Column.Width = 40;
            grdVoucherList[0, (int)GridColumns.SN].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdVoucherList[0, (int)GridColumns.Transact_Date] = new MyHeader("Transact Date");
            grdVoucherList[0, (int)GridColumns.Transact_Date].Column.Width = 100;
            grdVoucherList[0, (int)GridColumns.Transact_Date].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdVoucherList[0, (int)GridColumns.Voucher_Type] = new MyHeader("Voucher Type");
            grdVoucherList[0, (int)GridColumns.Voucher_Type].Column.Width = 75;
            grdVoucherList[0, (int)GridColumns.Voucher_Type].Column.Visible = false;

            if (m_VoucherValues[0] == "SALES_ORDER" || m_VoucherValues[0] == "PURCHASE_ORDER")
                grdVoucherList[0, (int)GridColumns.Voucher_No] = new MyHeader("Order No.");

            else
                grdVoucherList[0, (int)GridColumns.Voucher_No] = new MyHeader("Voucher No.");

            grdVoucherList[0, (int)GridColumns.Voucher_No].Column.Width = 75;
            grdVoucherList[0, (int)GridColumns.Voucher_No].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            grdVoucherList[0, (int)GridColumns.Name] = new MyHeader("Name");
            grdVoucherList[0, (int)GridColumns.Name].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            if (m_VoucherValues[0] == "JOURNAL" || m_VoucherValues[0] == "CONTRA")
                grdVoucherList[0, (int)GridColumns.Name].Column.Visible = false;

            grdVoucherList[0, (int)GridColumns.Description] = new MyHeader("Description");
            grdVoucherList[0, (int)GridColumns.Description].Column.Width = 325;
            grdVoucherList[0, (int)GridColumns.Description].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch; //| SourceGrid.AutoSizeMode.Default;

            grdVoucherList[0, (int)GridColumns.Gross_Amt] = new MyHeader("Gross Amt");
            grdVoucherList[0, (int)GridColumns.Gross_Amt].Column.Width = 75;
            grdVoucherList[0, (int)GridColumns.Gross_Amt].Column.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            if (m_VoucherValues[0] == "JOURNAL" || m_VoucherValues[0] == "CONTRA")
            {
                grdVoucherList[0, (int)GridColumns.Name].Column.Visible = false;
                grdVoucherList[0, (int)GridColumns.Description].Column.Width = 435;
            }
            else
                grdVoucherList[0, (int)GridColumns.Name].Column.Width = 110;

            grdVoucherList.AutoStretchColumnsToFitWidth = true;

        }
        private void LoadVoucherList(DataTable dtv)
        {
            try
            {
                decimal amount = 0;
                int rowsCount = 0;
                rowsCount = dtv.Rows.Count;
                grdVoucherList.Rows.Clear();

                grdVoucherList.SelectionMode = SourceGrid.GridSelectionMode.Row;
                grdVoucherList.Redim(rowsCount + 1, 8);

                WriteHeaderVouherList();
                for (int i = 1; i <= rowsCount; i++)
                {
                    alternate = new SourceGrid.Cells.Views.Cell();
                    rightAlign = new SourceGrid.Cells.Views.Cell();                          // right alignment for Gross_Amount
                    rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.BottomRight;

                    if (i % 2 != 0)
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
                        rightAlign.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
                    }
                    DataRow dr = dtv.Rows[i - 1];
                    grdVoucherList[i, (int)GridColumns.MasterID] = new SourceGrid.Cells.Cell(dr["ID"].ToString());
                    grdVoucherList[i, (int)GridColumns.MasterID].View = alternate;

                    grdVoucherList[i, (int)GridColumns.SN] = new SourceGrid.Cells.Cell((i + RecordCount).ToString());
                    grdVoucherList[i, (int)GridColumns.SN].View = alternate;
                    grdVoucherList[i, (int)GridColumns.SN].AddController(evtDblClick);

                    grdVoucherList[i, (int)GridColumns.Transact_Date] = new SourceGrid.Cells.Cell(dr["Created_Date"].ToString());  //.ToString("yyyy-MM-dd"));

                    grdVoucherList[i, (int)GridColumns.Transact_Date].View = alternate;
                    grdVoucherList[i, (int)GridColumns.Transact_Date].AddController(evtDblClick);


                    grdVoucherList[i, (int)GridColumns.Voucher_No] = new SourceGrid.Cells.Cell(dr["Voucher_No"].ToString());
                    grdVoucherList[i, (int)GridColumns.Voucher_No].View = alternate;
                    grdVoucherList[i, (int)GridColumns.Voucher_No].AddController(evtDblClick);


                    grdVoucherList[i, (int)GridColumns.Name] = new SourceGrid.Cells.Cell(dr["EngName"].ToString());
                    grdVoucherList[i, (int)GridColumns.Name].View = alternate;
                    grdVoucherList[i, (int)GridColumns.Name].AddController(evtDblClick);

                    grdVoucherList[i, (int)GridColumns.Description] = new SourceGrid.Cells.Cell(dr["Remarks"].ToString());
                    grdVoucherList[i, (int)GridColumns.Description].View = alternate;
                    grdVoucherList[i, (int)GridColumns.Description].AddController(evtDblClick);

                    amount = (dr["Gross_Amount"] == DBNull.Value) ? 0 : Convert.ToDecimal(dr["Gross_Amount"].ToString());

                    grdVoucherList[i, (int)GridColumns.Gross_Amt] = new SourceGrid.Cells.Cell(amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdVoucherList[i, (int)GridColumns.Gross_Amt].View = rightAlign;
                    grdVoucherList[i, (int)GridColumns.Gross_Amt].AddController(evtDblClick);


                    grdVoucherList.Rows.SetHeight(i, 22);

                }
                
                grdVoucherList.AutoStretchColumnsToFitWidth = true;

                grdVoucherList.Columns.StretchToFit();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        public void Row_Double_Click(object sender, EventArgs e)
        {
            int CurRow = grdVoucherList.Selection.GetSelectionRegion().GetRowsIndex()[0];

            SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdVoucherList, new SourceGrid.Position(CurRow, 0));
            MasterID = Convert.ToInt32(cellType.Value);
            m_form.GetVoucher(MasterID);
            this.Close();
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                RecordCount = 0;
                LoadVoucherList(VoucherList.GetVoucherList("0", m_VoucherValues, Navigate.First.ToString(), txtSearchValue.Text, Date.ToDotNet(txtFromDate.Text).ToShortDateString(), Date.ToDotNet(txtToDate.Text).ToShortDateString() + " 23:59:59:999"));
            }
            catch
            {
                Global.MsgError("Invalid date format.");
                txtFromDate.Text = Date.DBToSystem(VoucherList.GetFromDate().ToString());
                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdVoucherList, new SourceGrid.Position(1, 0));
                MasterID = Convert.ToInt32(cellType.Value);

                DataTable dt = VoucherList.GetVoucherList(MasterID.ToString(), m_VoucherValues, Navigate.Prev.ToString(), txtSearchValue.Text, Date.ToDotNet(txtFromDate.Text).ToShortDateString(), Date.ToDotNet(txtToDate.Text).ToShortDateString() + " 23:59:59:999");

                if (dt.Rows.Count == 0)  // if no more data are available, keep displaying the first records
                {
                    //btnFirst_Click(null, null); // i.e. do nothing as the record is already loaded in the grid, so no need to reload it
                }

                else
                {
                    RecordCount -= 30;
                    LoadVoucherList(dt);
                }
            }
            catch
            {
                Global.MsgError("Invalid date format.");
                txtFromDate.Text = Date.DBToSystem(VoucherList.GetFromDate().ToString());
                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdVoucherList, new SourceGrid.Position(grdVoucherList.RowsCount - 1, 0));
                MasterID = Convert.ToInt32(cellType.Value);

                DataTable dt = VoucherList.GetVoucherList(MasterID.ToString(), m_VoucherValues, Navigate.Next.ToString(), txtSearchValue.Text, Date.ToDotNet(txtFromDate.Text).ToShortDateString(), Date.ToDotNet(txtToDate.Text).ToShortDateString() + " 23:59:59:999");

                if (dt.Rows.Count == 0) // if no more data are available, keep displaying the last records
                {
                    //btnLast_Click(null, null); // i.e. do nothing as the record is already loaded in the grid, so no need to reload it
                }

                else
                {
                    RecordCount += 30;
                    LoadVoucherList(dt);
                }
            }
            catch
            {
                Global.MsgError("Invalid date format.");
                txtFromDate.Text = Date.DBToSystem(VoucherList.GetFromDate().ToString());
                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = VoucherList.GetVoucherList("0", m_VoucherValues, Navigate.Last.ToString(), txtSearchValue.Text, Date.ToDotNet(txtFromDate.Text).ToShortDateString(), Date.ToDotNet(txtToDate.Text).ToShortDateString() + " 23:59:59:999");
                RecordCount = VoucherList.GetRecordCount() - VoucherList.GetRecordCount() % 30;

                LoadVoucherList(dt);
            }
            catch
            {
                Global.MsgError("Invalid date format.");
                txtFromDate.Text = Date.DBToSystem(VoucherList.GetFromDate().ToString());
                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            }
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

                dsVoucherList.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

                //otherwise it populate the records again and again

                rptVoucherList rpt = new rptVoucherList();

                //Fill the logo on the report
                Misc.WriteLogo(dsVoucherList, "tblImage");
                rpt.SetDataSource(dsVoucherList);


                try
                {
                    dsVoucherList.Tables.Remove("tblVoucherList");//this table was just for documentation that what field is gonna require
                    DataTable dt = VoucherList.GetVoucherList("0", m_VoucherValues, "ALL", txtSearchValue.Text, Date.ToDotNet(txtFromDate.Text).ToShortDateString(), Date.ToDotNet(txtToDate.Text).ToShortDateString() + " 23:59:59:999");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Gross_Amount"] = Convert.ToDecimal(dt.Rows[i]["Gross_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }

                    System.Data.DataView view = new System.Data.DataView(dt);
                    DataTable selected = view.ToTable("tblVoucherList", false, "Created_Date", "Voucher_No", "EngName", "Remarks", "Gross_Amount");

                    dsVoucherList.Tables.Add(selected);

                    if (m_VoucherValues.ToString() == "JOURNAL" || m_VoucherValues.ToString() == "CONTRA")
                    {
                        rpt.Section2.ReportObjects["Text6"].Left -= 3700;
                        rpt.Section3.ReportObjects["Remarks1"].Left -= 3700;
                        rpt.Section3.ReportObjects["Remarks1"].Width += 3700;
                    }
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
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvFiscal_Year = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvFrom_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvTo_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();


                    //Update the progressbar
                    ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

                    pdvFont.Value = "Arial";
                    pvCollection.Clear();
                    pvCollection.Add(pdvFont);
                    rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

                    CompanyDetails m_CompanyDetails = null;
                    int uid = User.CurrUserID;
                    //DataTable dtroleinfo = User.GetUserInfo(uid);
                    //DataRow drrole = dtroleinfo.Rows[0];
                    //int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());

                    if (Global.GlobalAccessRoleID == 37)//if user is root (System Adminstrator), get information from tblCompany
                    {
                        m_CompanyDetails = CompanyInfo.GetInfo();

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

                        pdvCompany_Slogan.Value = " ";
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

                        pdvCompany_Slogan.Value = (companyslogan == "" && companyslogan == null) ? " " : companyslogan;
                        pvCollection.Clear();
                        pvCollection.Add(pdvCompany_Slogan);
                        rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                    }

                    pdvReport_Head.Value = "Voucher List : " + m_VoucherValues[0].Replace('_', ' '); ;
                    pvCollection.Clear();
                    pvCollection.Add(pdvReport_Head);
                    rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

                    pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
                    pvCollection.Clear();
                    pvCollection.Add(pdvFiscal_Year);
                    rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                    //Update the progressbar
                    ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                    pdvReport_Date.Value = "As On Date : " + (isEnglishDate ? DateTime.Today.ToString("yyyy/MM/dd") : Date.ToSystem(Date.GetServerDate()));
                    pvCollection.Clear();
                    pvCollection.Add(pdvReport_Date);
                    rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                    pdvFrom_Date.Value = "From :" + (isEnglishDate ? Date.ToDotNet(txtFromDate.Text).ToString("yyyy/MM/dd") : txtFromDate.Text.Replace('-', '/'));
                    pvCollection.Clear();
                    pvCollection.Add(pdvFrom_Date);
                    rpt.DataDefinition.ParameterFields["From_Date"].ApplyCurrentValues(pvCollection);

                    pdvTo_Date.Value = "To :" + (isEnglishDate ? Date.ToDotNet(txtToDate.Text).ToShortDateString() : txtToDate.Text.Replace('-', '/'));
                    pvCollection.Clear();
                    pvCollection.Add(pdvTo_Date);
                    rpt.DataDefinition.ParameterFields["To_Date"].ApplyCurrentValues(pvCollection);

                    Common.frmReportViewer frm = new Common.frmReportViewer();
                    frm.SetReportSource(rpt);
                    CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    CrDiskFileDestinationOptions.DiskFileName = FileName;

                    //Update the progressbar
                    ProgressForm.UpdateProgress(100, "Showing Report...");

                    // Close the dialog
                    ProgressForm.CloseForm();

                    switch (prntDirect)
                    {
                        //case 1:
                        //    rpt.PrintOptions.PrinterName = "";
                        //    rpt.PrintToPrinter(1, false, 0, 0);
                        //    prntDirect = 0;
                        //    return;
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
                            Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                            sendemail.ShowDialog();
                            rpt.Close();
                            return;
                        default:
                            frm.Show();
                            frm.WindowState = FormWindowState.Maximized;
                            break;
                    }

                    // frm.Show();
                    frm.WindowState = FormWindowState.Maximized;

                }
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            prntDirect = 0;
            PrintPreviewCR(PrintType.CrystalReport);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtSearchValue != null && txtSearchValue.Text != "")  // perform search operation only when the search value is given
                //{
                RecordCount = 0;
                LoadVoucherList(VoucherList.GetVoucherList("0", m_VoucherValues, Navigate.First.ToString(), txtSearchValue.Text, Date.ToDotNet(txtFromDate.Text).ToShortDateString(), Date.ToDotNet(txtToDate.Text).ToShortDateString() + " 23:59:59:999"));
                //}
            }
            catch (Exception)
            {
                Global.MsgError("Invalid date format.");
                txtFromDate.Text = Date.DBToSystem(VoucherList.GetFromDate().ToString());
                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            }
        }

        private void btnForgetSearch_Click(object sender, EventArgs e)
        {
            txtSearchValue.Text = "";
            txtFromDate.Text = Date.DBToSystem(VoucherList.GetFromDate().ToString());

            btnFirst.PerformClick();
        }

        private void txtSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
            if (e.KeyCode == Keys.Escape)
            {
                btnForgetSearch.PerformClick();
            }
        }
        public static void SetRecordCount(int recordCount)
        {
            RecordCount = recordCount;
        }

        public void DateConvert(DateTime ReturnDotNetDate)
        {
            if (!isToDate)
                txtFromDate.Text = Date.ToSystem(ReturnDotNetDate);
            else
                txtToDate.Text = Date.ToSystem(ReturnDotNetDate);

        }

        private void btnFromDate_Click(object sender, EventArgs e)
        {
            try
            {
                isToDate = false;
                Common.frmDateConverter frm = new Common.frmDateConverter(this, Date.ToDotNet(txtFromDate.Text));
                frm.ShowDialog();
            }
            catch (Exception)
            {
                Global.MsgError("Invalid date format.");
                txtFromDate.Text = Date.DBToSystem(VoucherList.GetFromDate().ToString());
                //txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            }
        }

        private void btnToDate_Click(object sender, EventArgs e)
        {
            try
            {
                isToDate = true;
                Common.frmDateConverter frm = new Common.frmDateConverter(this, Date.ToDotNet(txtToDate.Text));
                frm.ShowDialog();
            }
            catch (Exception)
            {
                Global.MsgError("Invalid date format.");
                //txtFromDate.Text = Date.DBToSystem(VoucherList.GetFromDate().ToString());
                txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            }
        }

        private void frmVoucherList_Resize(object sender, EventArgs e)
        {
            grdVoucherList.Width = this.Width - 40;
        }

        private void txtFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtFromDate.MaskCompleted && txtToDate.MaskCompleted)
            {
                btnSearch.PerformClick();
            }
        }

        private void txtToDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtToDate.MaskCompleted && txtFromDate.MaskCompleted)
            {
                btnSearch.PerformClick();
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
                    prntDirect = 2;
                    //btnPrint_Click(sender, e);
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
                    //btnPrint_Click(sender, e);
                    PrintPreviewCR(PrintType.PDF);
                    break;
                case "mnuEmail":
                    //Code for email export
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
                    //btnPrint_Click(sender, e);
                    PrintPreviewCR(PrintType.Email);
                    break;
            }
        }

    }
    class MyHeader : SourceGrid.Cells.ColumnHeader
    {
        public MyHeader(object value)
            : base(value)
        {

            SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
            view.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            View = view;

            AutomaticSortEnabled = false;
        }

    }
}
