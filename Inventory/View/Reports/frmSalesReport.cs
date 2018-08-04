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
using Common;
using DateManager;
using System.Threading;
using CrystalDecisions.Shared;
using Inventory.Reports;


namespace Inventory
{
    public partial class frmSalesReport : Form
    {
        double m_totalAmount = 0;
        ArrayList AccClassID = new ArrayList();
        private Inventory.Model.dsSalesReport dsSalesReport = new Model.dsSalesReport();
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        private SalesReportSettings m_SalesReport;
        private Sales m_Sales = new Sales();
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

        public frmSalesReport(SalesReportSettings SalesReport)
        {
            try
            {
                m_SalesReport = new SalesReportSettings();
                //For Voucher_No
                m_SalesReport.VoucherNo = SalesReport.VoucherNo;
                m_SalesReport.VoucherNoAll = SalesReport.VoucherNoAll;
                m_SalesReport.VocherNoSingle = SalesReport.VocherNoSingle;
                m_SalesReport.VoucherNoSingleValue = SalesReport.VoucherNoSingleValue;

                //For Product
                m_SalesReport.IsProductwise = SalesReport.IsProductwise;
                m_SalesReport.IsProductAll = SalesReport.IsProductAll;
                m_SalesReport.ProductSingleID = SalesReport.ProductSingleID;
                m_SalesReport.ProductGroupID = SalesReport.ProductGroupID;

                //For Party
                m_SalesReport.IsPartywise = SalesReport.IsPartywise;
                m_SalesReport.IsPartywise = SalesReport.IsPartywise;
                m_SalesReport.IsPartyAll = SalesReport.IsProductAll;
                m_SalesReport.PartySingleID = SalesReport.PartySingleID;
                m_SalesReport.PartyGroupID = SalesReport.PartyGroupID;

                //For DateRange
                m_SalesReport.IsDateRange = SalesReport.IsDateRange;
                m_SalesReport.FromDate = SalesReport.FromDate;
                m_SalesReport.ToDate = SalesReport.ToDate;

                //For SalesAccount
                m_SalesReport.SalesLedgerID = SalesReport.SalesLedgerID;

                //for Project
                m_SalesReport.ProjectID = SalesReport.ProjectID;

                //for Depot
                m_SalesReport.DepotID = SalesReport.DepotID;
                m_SalesReport.AccClassID = SalesReport.AccClassID;

                InitializeComponent();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private string ReadAllAccClassID()
        {
            #region  AccountingClassID
            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_SalesReport.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_SalesReport.AccClassID.AddRange(arrChildAccClassIDs);

            #endregion
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("SALESREPORT");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("AccClassIDSettings");
                    foreach (string tag in m_SalesReport.AccClassID)
                    {
                        AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccClassID", Convert.ToInt32(tag).ToString());
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

        private void DisplaySalesReport(bool IsCrystalReport)
        {

            #region BLOCK FOR ORIENTATION PURPOSE
            if (!IsCrystalReport)//Orientaion Purpose is essential only for SOurcegrid not for Crystal report
            {
                DisplayBannar();
                //Let the whole row to be selected
                grdSalesReport.SelectionMode = SourceGrid.GridSelectionMode.Row;

                //Disable multiple selection
                grdSalesReport.Selection.EnableMultiSelection = false;
                //Disable multiple selection
                grdSalesReport.Redim(1, 9);
                int rows = grdSalesReport.Rows.Count;
                grdSalesReport.Rows.Insert(rows);
                //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                dblClick.DoubleClick += new EventHandler(grdSalesReport_DoubleClick);
            }
            #endregion
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
            //IF Sales Report has to be shown according to Productwise
            if (m_SalesReport.IsProductwise)//Sales Report According to productwise
            {
                if (!IsCrystalReport)//DOnt show header for Crystal Report
                    MakeHeader(false);
                DataTable dtSalesReportByProduct = Sales.GetSalesReport(m_SalesReport.SalesLedgerID, m_SalesReport.ProductGroupID, m_SalesReport.ProductSingleID, m_SalesReport.PartyGroupID, m_SalesReport.PartySingleID, m_SalesReport.DepotID, m_SalesReport.ProjectID, m_SalesReport.FromDate, m_SalesReport.ToDate, InventoryReportType.ProductWise, AccClassIDsXMLString, ProjectIDsXMLString);
                WriteSalesReport(dtSalesReportByProduct, false, IsCrystalReport);

            }
            else if (m_SalesReport.IsPartywise)
            {
                MakeHeader(true);
                DataTable dtSalesReportByProduct = Sales.GetSalesReport(m_SalesReport.SalesLedgerID, m_SalesReport.ProductGroupID, m_SalesReport.ProductSingleID, m_SalesReport.PartyGroupID, m_SalesReport.PartySingleID, m_SalesReport.DepotID, m_SalesReport.ProjectID, m_SalesReport.FromDate, m_SalesReport.ToDate, InventoryReportType.PartyWise, AccClassIDsXMLString, ProjectIDsXMLString);
                WriteSalesReport(dtSalesReportByProduct, true, IsCrystalReport);
            }
        }


        private void frmSalesReport_Load(object sender, EventArgs e)
        {
            DisplaySalesReport(false);
        }

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            //lblCompanyName.Text = m_CompanyDetails.CompanyName;
            //lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            //lblContact.Text = "Contact: " + m_CompanyDetails.Telephone + "Website: " + m_CompanyDetails.Website;
            //lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;

            if (m_SalesReport.IsPartywise)
            {
                lblReportType.Text = "Sales-Partywise Report";

            }
            else if (m_SalesReport.IsProductwise)
            {
                lblReportType.Text = "Sales-Productwise Report";
            }
            //If it has a date range
            if (m_SalesReport.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_SalesReport.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FYFrom));

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_SalesReport.ProjectID), LangMgr.DefaultLanguage);
            if (m_SalesReport.ProjectID != null)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
        }

        private void WriteSalesReport(DataTable dt, bool IsPartywise, bool IsCrystalReport)
        {
            SourceGrid.Cells.Views.Cell rightAlign = null;
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i - 1];

                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                rightAlign = new SourceGrid.Cells.Views.Cell();

                if (i % 2 == 0)
                {
                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    rightAlign.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);

                }
                rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                string ProdName, PartyName,unit;
                ProdName = unit = PartyName = "";
                decimal salesRate = 0;
                if (!IsPartywise)//IF Product wise
                {
                    ProdName = dr["productName"].ToString();
                    unit = dr["Unit"].ToString();
                    salesRate = Convert.ToDecimal(dr["SalesRate"]);
                }
                decimal m_NetSalesQty = 0,outbound =0, inbound =0;
                //m_NetSalesQty = Convert.ToDecimal(dr["InBound"]) - Convert.ToDecimal(dr["OutBound"]);
                outbound = Convert.ToDecimal(dr["OutBound"]);
                inbound = Convert.ToDecimal(dr["InBound"]);
                m_NetSalesQty = outbound - inbound;

                //m_NetSalesQty =Convert.ToDecimal(dr["OutBound"]);
                double Amount = Convert.ToDouble(dr["Amount"]);
                m_totalAmount += Amount;
                //for writting on Grid
                if (!IsCrystalReport)
                {

                    grdSalesReport.Rows.Insert(i);
                    if (!IsPartywise)//IF Product wise
                    {
                        grdSalesReport[i, 0] = new SourceGrid.Cells.Cell(ProdName);
                        grdSalesReport[i, 0].AddController(dblClick);
                        grdSalesReport[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                        grdSalesReport[i, 1] = new SourceGrid.Cells.Cell(unit);
                        grdSalesReport[i, 1].AddController(dblClick);
                        grdSalesReport[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                        grdSalesReport[i, 2] = new SourceGrid.Cells.Cell(salesRate.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdSalesReport[i, 2].AddController(dblClick);
                        grdSalesReport[i, 2].View = new SourceGrid.Cells.Views.Cell(rightAlign);
                    }
                    else//IF party wise
                    {

                        DataTable dtPartytInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                        DataRow drPartyInfo = dtPartytInfo.Rows[0];
                        PartyName = drPartyInfo["LedName"].ToString();
                        grdSalesReport[i, 0] = new SourceGrid.Cells.Cell(PartyName);
                        grdSalesReport[i, 0].AddController(dblClick);
                        grdSalesReport[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                        grdSalesReport[i, 1] = new SourceGrid.Cells.Cell("unit");
                        grdSalesReport[i, 1].AddController(dblClick);
                        grdSalesReport[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                    }

                    grdSalesReport[i, 3] = new SourceGrid.Cells.Cell(outbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdSalesReport[i, 3].AddController(dblClick);
                    grdSalesReport[i, 3].View = new SourceGrid.Cells.Views.Cell(rightAlign);
                    grdSalesReport[i, 4] = new SourceGrid.Cells.Cell(inbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdSalesReport[i, 4].AddController(dblClick);
                    grdSalesReport[i, 4].View = new SourceGrid.Cells.Views.Cell(rightAlign);

                    grdSalesReport[i, 5] = new SourceGrid.Cells.Cell(m_NetSalesQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdSalesReport[i, 5].AddController(dblClick);
                    grdSalesReport[i, 5].View = new SourceGrid.Cells.Views.Cell(rightAlign);
                    grdSalesReport[i, 6] = new SourceGrid.Cells.Cell((Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
                    grdSalesReport[i, 6].AddController(dblClick);
                    grdSalesReport[i, 6].View = new SourceGrid.Cells.Views.Cell(rightAlign);
                    if (!IsPartywise)//for productwise
                    {

                        grdSalesReport[i, 7] = new SourceGrid.Cells.Cell(dr["ProductID"].ToString());
                        grdSalesReport[i, 8] = new SourceGrid.Cells.Cell(InventoryReportType.ProductWise.ToString());
                    }
                    else//for partywise
                    {
                        grdSalesReport[i, 7] = new SourceGrid.Cells.Cell(dr["PartyID"].ToString());
                        grdSalesReport[i, 8] = new SourceGrid.Cells.Cell(InventoryReportType.PartyWise.ToString());
                    }
                }
                else //for writting on Crystal Report
                {
                    if (IsPartywise)//According to partywise
                    {
                        DataTable dtPartytInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                        DataRow drPartyInfo = dtPartytInfo.Rows[0];
                        PartyName = drPartyInfo["LedName"].ToString();
                        dsSalesReport.Tables["tblParty"].Rows.Add(PartyName, "Unit", outbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), inbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_NetSalesQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));

                    }
                    else//According to productwise
                    {
                        dsSalesReport.Tables["tblProduct"].Rows.Add(ProdName, unit, outbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), inbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_NetSalesQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))), salesRate.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    }

                }
            }
            int no = dt.Rows.Count;
            grdSalesReport[no + 1, 0] = new SourceGrid.Cells.Cell("Total");
            grdSalesReport[no + 1, 1] = new SourceGrid.Cells.Cell("");
            grdSalesReport[no + 1, 2] = new SourceGrid.Cells.Cell("");
            grdSalesReport[no + 1, 3] = new SourceGrid.Cells.Cell("");
            grdSalesReport[no + 1, 4] = new SourceGrid.Cells.Cell("");
            grdSalesReport[no + 1, 5] = new SourceGrid.Cells.Cell("");
            grdSalesReport[no + 1, 6] = new SourceGrid.Cells.Cell(m_totalAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
            rightAlign.BackColor = Color.White;
            grdSalesReport[no + 1, 6].View = new SourceGrid.Cells.Views.Cell(rightAlign);
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

        private void MakeHeader(bool IsPartywise)
        {
            //Defining the HeaderPart
            if (IsPartywise)
                grdSalesReport[0, 0] = new MyHeader("Party Details");
            else
                grdSalesReport[0, 0] = new MyHeader("Product Details");
            grdSalesReport[0, 1] = new MyHeader("Unit");
            grdSalesReport[0, 2] = new MyHeader("Rate");
            grdSalesReport[0, 3] = new MyHeader("Sales Qty");
            grdSalesReport[0, 4] = new MyHeader("Return Qty");
            grdSalesReport[0, 5] = new MyHeader("Net Sales Qty");
            grdSalesReport[0, 6] = new MyHeader("Net Sales Amt.");
            if (IsPartywise)
                grdSalesReport[0, 7] = new MyHeader("Party ID");
            else
                grdSalesReport[0, 7] = new MyHeader("Product ID");

            grdSalesReport[0, 8] = new MyHeader("Report Type");
            //Define size of column
            grdSalesReport[0, 0].Column.Width = 390;
            grdSalesReport[0, 1].Column.Width = 80;
            if (IsPartywise)
            {
                grdSalesReport[0, 1].Column.Visible = false;
                grdSalesReport[0, 2].Column.Visible = false;
            }
            grdSalesReport[0, 2].Column.Width = 100;
            grdSalesReport[0, 3].Column.Width = 100;
            grdSalesReport[0, 4].Column.Width = 100;
            grdSalesReport[0, 5].Column.Width = 100;
            grdSalesReport[0, 6].Column.Width = 200;
            grdSalesReport[0, 7].Column.Width = 50;
            grdSalesReport[0, 8].Column.Width = 80;
            grdSalesReport[0, 7].Column.Visible = false;
            grdSalesReport[0, 8].Column.Visible = false;

        }

        private void grdSalesReport_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //Get the Selected Row           
                int CurRow = grdSalesReport.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(grdSalesReport, new SourceGrid.Position(CurRow, 7));
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdSalesReport, new SourceGrid.Position(CurRow, 8));
                string Type = (cellType.Value).ToString();
                //read id                       
                string ID = (cellTypeID.Value).ToString();//
                if (ID == "0")//IF ID is blank        
                    return;
                SalesPartyTransactSettings m_SalesParty = new SalesPartyTransactSettings();
                if (Type == InventoryReportType.ProductWise.ToString())
                {
                    m_SalesParty.ProductID = Convert.ToInt32(ID);
                }
                else if (Type == InventoryReportType.PartyWise.ToString())
                {
                    m_SalesParty.PartyID = Convert.ToInt32(ID);
                }

                m_SalesParty.ReportType = Type;
                m_SalesParty.SalesLedgerID = m_SalesReport.SalesLedgerID;
                m_SalesParty.DepotID = m_SalesReport.DepotID;
                m_SalesParty.ProjectID = m_SalesReport.ProjectID;
                m_SalesParty.FromDate = m_SalesReport.FromDate;
                m_SalesParty.ToDate = m_SalesReport.ToDate;
                m_SalesParty.AccClassID = m_SalesReport.AccClassID;
                frmSalesProductPartyTransaction frm = new frmSalesProductPartyTransaction(m_SalesParty);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }


        private void frmSalesReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
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

            dsSalesReport.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

            //otherwise it populate the records again and again
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;

            //rptBalanceSheet rpt = new rptBalanceSheet();
            rptSalesReportByProduct m_rptSalesReportByProduct = new rptSalesReportByProduct();
            rptSalesReportByParty m_rptSalesReportByParty = new rptSalesReportByParty();


            //Fill the logo on the report
            //Misc.WriteLogo(dsSalesReport, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            //rpt.SetDataSource(dsSalesReport);
            if (m_SalesReport.IsProductwise)
            {
                //Fill the logo on the report
                Misc.WriteLogo(dsSalesReport, "tblImage");
                m_rptSalesReportByProduct.SetDataSource(dsSalesReport);
            }
            else if (m_SalesReport.IsPartywise)
            {
                //Fill the logo on the report
                Misc.WriteLogo(dsSalesReport, "tblImage");
                m_rptSalesReportByParty.SetDataSource(dsSalesReport);
            }

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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvSalesTotalAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();

            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            if (m_SalesReport.IsProductwise)
                m_rptSalesReportByProduct.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);
            else if (m_SalesReport.IsPartywise)
                m_rptSalesReportByParty.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);


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
                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);
                pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);

                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);

                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);

                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
            
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
                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);
                pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = companypan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);

                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);

                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);

                if (m_SalesReport.IsProductwise)
                    m_rptSalesReportByProduct.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                else if (m_SalesReport.IsPartywise)
                    m_rptSalesReportByParty.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
            

            }

            if (m_SalesReport.IsProductwise)
            {
                pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvPreparedBy);
                m_rptSalesReportByProduct.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

                pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvCheckedBy);
                m_rptSalesReportByProduct.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

                pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvApprovedBy);
                m_rptSalesReportByProduct.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);

                pdvPrintDate.Value = Date.ToSystem(DateTime.Now);
                pvCollection.Clear();
                pvCollection.Add(pdvPrintDate);
                m_rptSalesReportByProduct.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);
            }
            else if (m_SalesReport.IsPartywise)
            {
                pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvPreparedBy);
                m_rptSalesReportByParty.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

                pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvCheckedBy);
                m_rptSalesReportByParty.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

                pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvApprovedBy);
                m_rptSalesReportByParty.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);

                pdvPrintDate.Value = Date.ToSystem(DateTime.Now);
                pvCollection.Clear();
                pvCollection.Add(pdvPrintDate);
                m_rptSalesReportByParty.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);
            }
           

            if (m_SalesReport.IsProductwise)
            {
                pdvSalesTotalAmount.Value = m_totalAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                pvCollection.Clear();
                pvCollection.Add(pdvSalesTotalAmount);
                m_rptSalesReportByProduct.DataDefinition.ParameterFields["TotalAmount"].ApplyCurrentValues(pvCollection);
            }
            else if (m_SalesReport.IsPartywise)
            {
                pdvSalesTotalAmount.Value = m_totalAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                pvCollection.Clear();
                pvCollection.Add(pdvSalesTotalAmount);
                m_rptSalesReportByParty.DataDefinition.ParameterFields["TotalAmount"].ApplyCurrentValues(pvCollection);
            }

            if (m_SalesReport.IsProductwise)
            {
                pdvReport_Head.Value = "Sales-Productwise Report";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                m_rptSalesReportByProduct.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
            }
            else if (m_SalesReport.IsPartywise)
            {
                pdvReport_Head.Value = "Sales-Partywise Report";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                m_rptSalesReportByParty.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
            }

            pdvFiscal_Year.Value = "Fiscal Year:" + (m_CompanyDetails.FiscalYear);
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);

            if (m_SalesReport.IsProductwise)
                m_rptSalesReportByProduct.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);
            else if (m_SalesReport.IsPartywise)
                m_rptSalesReportByParty.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);


            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_SalesReport.FromDate != null && m_SalesReport.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SalesReport.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_SalesReport.ToDate));
            }
            if (m_SalesReport.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_SalesReport.ToDate));
            }
            if (m_SalesReport.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SalesReport.FromDate));
            }
            if (m_SalesReport.FromDate == null && m_SalesReport.ToDate == null)
            {
                pdvReport_Date.Value = "";

            }

            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            if (m_SalesReport.IsProductwise)
                m_rptSalesReportByProduct.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);
            else if (m_SalesReport.IsPartywise)
                m_rptSalesReportByParty.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            //Reset the total variables
            m_totalAmount = 0;
            DisplaySalesReport(true);

            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

            //Finally, show the report form
            Common.frmReportViewer frm = new Common.frmReportViewer();

            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");

            // Close the dialog
            ProgressForm.CloseForm();

            if (m_SalesReport.IsPartywise)
            {
                frm.SetReportSource(m_rptSalesReportByParty);

                switch (myPrintType)
                {
                    case PrintType.DirectPrint: //Direct Printer
                        m_rptSalesReportByParty.PrintOptions.PrinterName = "";
                        m_rptSalesReportByParty.PrintToPrinter(1, false, 0, 0);
                        return;
                    case PrintType.Excel: //Excel
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = m_rptSalesReportByParty.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        m_rptSalesReportByParty.Export();
                        m_rptSalesReportByParty.Close();
                        return;
                    case PrintType.PDF: //PDF
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = m_rptSalesReportByParty.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        m_rptSalesReportByParty.Export();
                        m_rptSalesReportByParty.Close();
                        return;
                    case PrintType.Email:
                        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                        CrExportOptions = m_rptSalesReportByParty.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                        m_rptSalesReportByParty.Export();
                        Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        m_rptSalesReportByParty.Close();
                        return;
                    default: //Crystal Report
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }

            }
            else if (m_SalesReport.IsProductwise)
            {
                frm.SetReportSource(m_rptSalesReportByProduct);

                switch (myPrintType)
                {
                    case PrintType.DirectPrint: //Direct Printer
                        m_rptSalesReportByProduct.PrintOptions.PrinterName = "";
                        m_rptSalesReportByProduct.PrintToPrinter(1, false, 0, 0);
                        return;
                    case PrintType.Excel: //Excel
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = m_rptSalesReportByProduct.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        m_rptSalesReportByProduct.Export();
                        m_rptSalesReportByProduct.Close();
                        return;
                    case PrintType.PDF: //PDF
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = m_rptSalesReportByProduct.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        m_rptSalesReportByProduct.Export();
                        m_rptSalesReportByProduct.Close();
                        return;
                    case PrintType.Email:
                        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                        CrExportOptions = m_rptSalesReportByParty.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                        m_rptSalesReportByParty.Export();
                        Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        m_rptSalesReportByParty.Close();
                        return;
                    default: //Crystal Report
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }

            }

            this.Cursor = Cursors.Default;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
           // Clear all rows
              grdSalesReport.Redim(0, 0);
              //Reset the total variables
              m_totalAmount = 0;
            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmSalesReport_Load(sender, e);
            grdSalesReport.Refresh();
         
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
                    //prntDirect = 4;
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_SalesReport.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_SalesReport.ProjectID + "'";

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
            tw.WriteStartElement("SALESREPORT");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ProjectIDSettings");
                    tw.WriteElementString("ProjectID", Convert.ToInt32(m_SalesReport.ProjectID).ToString());
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

    }
}
