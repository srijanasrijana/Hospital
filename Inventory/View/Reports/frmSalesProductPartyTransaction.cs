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
using System.Threading;
using CrystalDecisions.Shared;
using System.Collections;
using Common;
using Inventory.Reports;

namespace Inventory
{
    public partial class frmSalesProductPartyTransaction : Form
    {
        ArrayList AccClassID = new ArrayList();
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        private SalesPartyTransactSettings m_SalesParty;
        private Inventory.Model.dsSalesReport dsSalesReport = new Model.dsSalesReport();
        private string FileName = "";
        private string ProductParty_Pivot = "";
        public frmSalesProductPartyTransaction()
        {
            InitializeComponent();
        }

        public frmSalesProductPartyTransaction(SalesPartyTransactSettings Salesparty)
        {
            InitializeComponent();
            m_SalesParty = new SalesPartyTransactSettings();
            m_SalesParty.PartyID = Salesparty.PartyID;
            m_SalesParty.ProductID = Salesparty.ProductID;
            m_SalesParty.ReportType = Salesparty.ReportType;
            m_SalesParty.SalesLedgerID = Salesparty.SalesLedgerID;
            m_SalesParty.DepotID = Salesparty.DepotID;
            m_SalesParty.FromDate = Salesparty.FromDate;
            m_SalesParty.ToDate = Salesparty.ToDate;
            m_SalesParty.AccClassID = Salesparty.AccClassID;
        }

        private void MakeHeader()
        {
            //Defining the HeaderPart
            grdSalesProductTransaction[0, 0] = new MyHeader("Date");
            grdSalesProductTransaction[0, 1] = new MyHeader("Party");
            grdSalesProductTransaction[0, 2] = new MyHeader("Voucher Num.");
            grdSalesProductTransaction[0, 3] = new MyHeader("Net Sales. Qty.");
            grdSalesProductTransaction[0, 4] = new MyHeader("Net Return Qty.");
            grdSalesProductTransaction[0, 5] = new MyHeader("Unit");
            grdSalesProductTransaction[0, 6] = new MyHeader("Amount");
            grdSalesProductTransaction[0, 7] = new MyHeader("RowID");
            grdSalesProductTransaction[0, 8] = new MyHeader("VoucherType");

            //Define size of column
            grdSalesProductTransaction[0, 0].Column.Width = 100;
            grdSalesProductTransaction[0, 1].Column.Width = 150;
            if (m_SalesParty.ReportType == InventoryReportType.PartyWise.ToString())
            {
                grdSalesProductTransaction[0, 1].Column.Visible = false;
                grdSalesProductTransaction[0, 2].Column.Width = 180;
                grdSalesProductTransaction[0, 6].Column.Width = 220;
            }
            else
            {
                grdSalesProductTransaction[0, 2].Column.Width = 150;
                grdSalesProductTransaction[0, 6].Column.Width = 190;
            }
            grdSalesProductTransaction[0, 3].Column.Width = 100;
            grdSalesProductTransaction[0, 4].Column.Width = 100;
            grdSalesProductTransaction[0, 5].Column.Width = 80;
            grdSalesProductTransaction[0, 7].Column.Width = 50;
            grdSalesProductTransaction[0, 8].Column.Width = 50;
            grdSalesProductTransaction[0, 5].Column.Visible = false;
            grdSalesProductTransaction[0, 7].Column.Visible = false;
            grdSalesProductTransaction[0, 8].Column.Visible = false;

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

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            lblCompanyName.Text = m_CompanyDetails.CompanyName;
            //lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            //lblContact.Text = "Contact: " + m_CompanyDetails.Telephone + "Website: " + m_CompanyDetails.Website;
            lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;
            //If it has a date range
            if (m_SalesParty.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_SalesParty.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }
            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FYFrom));

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_SalesParty.ProjectID), LangMgr.DefaultLanguage);
            if (m_SalesParty.ProjectID != null)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];
                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }

            //Display pivot value on control
            if (m_SalesParty.ProductID > 0)
            {
                DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(m_SalesParty.ProductID), LangMgr.DefaultLanguage);
                DataRow drProductInfo = dtProductInfo.Rows[0];
                ProductParty_Pivot = "Product : " + drProductInfo["ProdName"].ToString();
                lblProductPartyPivot.Text = ProductParty_Pivot;
            }
            else if (m_SalesParty.PartyID > 0)
            {
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(m_SalesParty.PartyID),LangMgr.DefaultLanguage);
                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                ProductParty_Pivot ="Party : "+drLedgerInfo["LedName"].ToString();
                lblProductPartyPivot.Text = ProductParty_Pivot;
            }
        }

        private void WriteProductPartyTransactionOnGrid(string SalesReportType1, DataTable dt,bool IsCrystalReport)
        {
            SourceGrid.Cells.Views.Cell rightAlign = null;

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i - 1];

                decimal outbound = 0, inbound = 0;
                //m_NetSalesQty = Convert.ToDecimal(dr["InBound"]) - Convert.ToDecimal(dr["OutBound"]);
                outbound = Convert.ToDecimal(dr["OutBound"]);
                inbound = Convert.ToDecimal(dr["InBound"]);

                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                rightAlign = new SourceGrid.Cells.Views.Cell();
                rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                if (i % 2 == 0)
                {
                    
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    rightAlign.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);

                }
                string PartyName = "";
                double Amount = Convert.ToDouble(dr["Amount"]);
                if (!IsCrystalReport)//for writting on Grid
                {                 
                    grdSalesProductTransaction.Rows.Insert(i);
                    DateTime dtVoucherDate = Convert.ToDateTime(dr["VoucherDate"]);
                    grdSalesProductTransaction[i, 0] = new SourceGrid.Cells.Cell(Date.ToSystem(dtVoucherDate));
                    grdSalesProductTransaction[i, 0].AddController(dblClick);
                    grdSalesProductTransaction[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                    if (SalesReportType1 == InventoryReportType.ProductTransactWise.ToString())
                    {
                        //Incase of Productwise Transaction,need to show Party account
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                        DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                        PartyName = drLedgerInfo["LedName"].ToString();
                        grdSalesProductTransaction[i, 1] = new SourceGrid.Cells.Cell(PartyName);
                        grdSalesProductTransaction[i, 1].AddController(dblClick);
                        grdSalesProductTransaction[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

                    }
                    else if (SalesReportType1 == InventoryReportType.PartyTransactWise.ToString())
                    {
                        grdSalesProductTransaction[i, 1] = new SourceGrid.Cells.Cell("");
                    }
                    grdSalesProductTransaction[i, 2] = new SourceGrid.Cells.Cell(dr["VoucherNo"].ToString());
                    grdSalesProductTransaction[i, 2].AddController(dblClick);
                    grdSalesProductTransaction[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);

                    grdSalesProductTransaction[i, 3] = new SourceGrid.Cells.Cell(outbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdSalesProductTransaction[i, 3].AddController(dblClick);
                    grdSalesProductTransaction[i, 3].View = new SourceGrid.Cells.Views.Cell(rightAlign);

                    grdSalesProductTransaction[i, 4] = new SourceGrid.Cells.Cell(inbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdSalesProductTransaction[i, 4].AddController(dblClick);
                    grdSalesProductTransaction[i, 4].View = new SourceGrid.Cells.Views.Cell(rightAlign);

                    grdSalesProductTransaction[i, 5] = new SourceGrid.Cells.Cell("Unit");
                    grdSalesProductTransaction[i, 5].AddController(dblClick);
                    grdSalesProductTransaction[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);

                    grdSalesProductTransaction[i, 6] = new SourceGrid.Cells.Cell((Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
                    grdSalesProductTransaction[i, 6].AddController(dblClick);
                    grdSalesProductTransaction[i, 6].View = new SourceGrid.Cells.Views.Cell(rightAlign);

                    grdSalesProductTransaction[i, 7] = new SourceGrid.Cells.Cell(dr["RowID"].ToString());
                    grdSalesProductTransaction[i, 8] = new SourceGrid.Cells.Cell(dr["VoucherType"].ToString());

                }

                else//for writting on Crystal report
                {
                    
                    DateTime dtVoucherDate = Convert.ToDateTime(dr["VoucherDate"]);
                    if (SalesReportType1 == InventoryReportType.ProductTransactWise.ToString())
                    {
                        //Incase of Productwise Transaction,need to show Party account
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                        DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                        PartyName = drLedgerInfo["LedName"].ToString();
                        dsSalesReport.Tables["tblProductTransact"].Rows.Add(Date.ToSystem(dtVoucherDate), PartyName, dr["VoucherNo"].ToString(), outbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), inbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "Unit", (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));

                    }
                    else if (SalesReportType1 == InventoryReportType.PartyTransactWise.ToString())
                    {
                        dsSalesReport.Tables["tblPartyTransact"].Rows.Add(Date.ToSystem(dtVoucherDate), dr["VoucherNo"].ToString(), outbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), inbound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "Unit", (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
                    }


                }
            }
          
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID
            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_SalesParty.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_SalesParty.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in m_SalesParty.AccClassID)
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

        private void DisplaySalesReportProductPartyTransaction(bool IsCrystalReport)
        {
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
            if (!IsCrystalReport)//only for writting on Grid
            {
                DisplayBannar();
                //Let the whole row to be selected
                grdSalesProductTransaction.SelectionMode = SourceGrid.GridSelectionMode.Row;

                //Disable multiple selection
                grdSalesProductTransaction.Selection.EnableMultiSelection = false;
                //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                dblClick.DoubleClick += new EventHandler(grdSalesProductPartyTransaction_DoubleClick);
                //Disable multiple selection
                grdSalesProductTransaction.Redim(1, 9);
                int rows = grdSalesProductTransaction.Rows.Count;
                grdSalesProductTransaction.Rows.Insert(rows);
                MakeHeader();
            }
            DataTable dt = new DataTable();
            Sales m_Sales = new Sales();
            if (m_SalesParty.ReportType == InventoryReportType.ProductWise.ToString())
            {
                lblReportName.Text = "Sales-Productwise Transaction Report";
                dt = Sales.GetSalesReport(m_SalesParty.SalesLedgerID, m_SalesParty.ProductGroupID, m_SalesParty.ProductID, m_SalesParty.PartyGroupID, m_SalesParty.PartyID, m_SalesParty.DepotID, m_SalesParty.ProjectID, m_SalesParty.FromDate, m_SalesParty.ToDate, InventoryReportType.ProductTransactWise, AccClassIDsXMLString, ProjectIDsXMLString);
                lblProductUnit.Text = "Unit: " + Product.GetProductUnit(Convert.ToInt32(m_SalesParty.ProductID));
                lblProductUnit.Visible = true;
                WriteProductPartyTransactionOnGrid(InventoryReportType.ProductTransactWise.ToString(), dt,IsCrystalReport);
            }
            else if (m_SalesParty.ReportType == InventoryReportType.PartyWise.ToString())
            {
                lblReportName.Text = "Sales-Partywise Transaction Report";
                dt = Sales.GetSalesReport(m_SalesParty.SalesLedgerID, m_SalesParty.ProductGroupID, m_SalesParty.ProductID, m_SalesParty.PartyGroupID, m_SalesParty.PartyID, m_SalesParty.DepotID, m_SalesParty.ProjectID, m_SalesParty.FromDate, m_SalesParty.ToDate, InventoryReportType.PartyTransactWise, AccClassIDsXMLString, ProjectIDsXMLString);
                WriteProductPartyTransactionOnGrid(InventoryReportType.PartyTransactWise.ToString(), dt,IsCrystalReport);

            }
        }

        private void frmSalesProductPartyTransaction_Load(object sender, EventArgs e)
        {
            DisplaySalesReportProductPartyTransaction(false);
        }


        private void grdSalesProductPartyTransaction_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //Get the Selected Row
                int CurRow = grdSalesProductTransaction.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellID = new SourceGrid.CellContext(grdSalesProductTransaction, new SourceGrid.Position(CurRow, 7));
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdSalesProductTransaction, new SourceGrid.Position(CurRow, 8));
                if ((cellType.Value.ToString()) != "")//Dont Call the voucher form if there is no Ledger...no need to call Voucher form for Op. Bal/Total Amount etc
                {
                    int RowID = Convert.ToInt32(cellID.Value);
                    string VoucherType = cellType.Value.ToString();
                    switch (VoucherType)
                    {
                       case "SALES":
                            frmSalesInvoice frm = new frmSalesInvoice(RowID);
                            frm.Show();
                            break;
                        case "SLS_RTN":
                            frmSalesReturn frm1 = new frmSalesReturn(RowID);
                            frm1.Show();
                            break;
                        case "PURCH":
                            frmPurchaseInvoice frm2 = new frmPurchaseInvoice(RowID);
                            frm2.Show();
                            break;
                        case "PURCH_RTN":
                            frmPurchaseReturn frm3 = new frmPurchaseReturn(RowID);
                            frm3.Show();
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
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

            rptSalesReportByProductTransact m_rptSalesReportyByProductTransact = new rptSalesReportByProductTransact();
            rptSalesReportByPartyTransact m_rptSalesReportByPartyTransact = new rptSalesReportByPartyTransact();
            
            //Fill the logo on the report
            //Misc.WriteLogo(dsSalesReport, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            //rpt.SetDataSource(dsSalesReport);
            if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                m_rptSalesReportyByProductTransact.SetDataSource(dsSalesReport);
            else if (m_SalesParty.PartyID > 0)
                m_rptSalesReportByPartyTransact.SetDataSource(dsSalesReport);

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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvProductParty_Pivot = new CrystalDecisions.Shared.ParameterDiscreteValue();


            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);
            else if (m_SalesParty.PartyID > 0)
                m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);


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
                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = m_CompanyDetails.Address1;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);

                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);

                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);

                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);

                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
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
                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = companyaddress;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);

                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = companypan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);

                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);

                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);

                if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                    m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                else if (m_SalesParty.PartyID > 0)
                    m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            }
            if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
            {
                pdvReport_Head.Value = "Sales-Productwise Transaction Report";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
            }
            else if (m_SalesParty.PartyID > 0)
            {
                pdvReport_Head.Value = "Sales-Partywise Transaction Report";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
            }

            //for writting Pivot value
            if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
            {
                pdvProductParty_Pivot.Value = ProductParty_Pivot;
                pvCollection.Clear();
                pvCollection.Add(pdvProductParty_Pivot);
                m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["ProductParty_Pivot"].ApplyCurrentValues(pvCollection);
            }
            else if (m_SalesParty.PartyID > 0)
            {
                pdvProductParty_Pivot.Value = ProductParty_Pivot;
                pvCollection.Clear();
                pvCollection.Add(pdvProductParty_Pivot);
                m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["ProductParty_Pivot"].ApplyCurrentValues(pvCollection);
            }


            pdvFiscal_Year.Value = "Fiscal Year:" +(m_CompanyDetails.FiscalYear);
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);


            if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);
            else if (m_SalesParty.PartyID > 0)
                m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);


            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_SalesParty.FromDate != null && m_SalesParty.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SalesParty.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_SalesParty.ToDate));
            }
            if (m_SalesParty.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_SalesParty.ToDate));
            }
            if (m_SalesParty.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SalesParty.FromDate));
            }
            if (m_SalesParty.FromDate == null && m_SalesParty.ToDate == null)
            {
                pdvReport_Date.Value = "";
            }

            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);

            if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                m_rptSalesReportyByProductTransact.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);
            else if (m_SalesParty.PartyID > 0)
                m_rptSalesReportByPartyTransact.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);
            DisplaySalesReportProductPartyTransaction(true);          
            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

            //Finally, show the report form
            Common.frmReportViewer frm = new Common.frmReportViewer();
            if (m_SalesParty.ProductID > 0)//According to ProductTransact wise
                frm.SetReportSource(m_rptSalesReportyByProductTransact);
            else if (m_SalesParty.PartyID > 0)
                frm.SetReportSource(m_rptSalesReportByPartyTransact);
            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");
            // Close the dialog
            ProgressForm.CloseForm();
            frm.Show();
        }

        

        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_SalesParty.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_SalesParty.ProjectID + "'";

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
                    tw.WriteElementString("ProjectID", Convert.ToInt32(m_SalesParty.ProjectID).ToString());
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

        private void lblProductPartyPivot_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
