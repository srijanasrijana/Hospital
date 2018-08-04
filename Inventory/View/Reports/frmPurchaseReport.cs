using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Threading;
using DateManager;
using CrystalDecisions.Shared;
using System.Collections;
using Common;
using Inventory.Reports;
namespace Inventory
{
    public partial class frmPurchaseReport : Form
    {
        double Amount = 0;
        double m_totalAmount = 0;
        ArrayList AccClassID = new ArrayList();
        private Inventory.Model.dsPurchaseReport dsPurchaseReport = new Model.dsPurchaseReport();
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        private PurchaseReportSettings m_PurchaseReport;
        private Purchase m_Purchase = new Purchase();
        private string FileName = "";
        private DataTable dtTemp;
        decimal TotalAmount = 0;

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

        public frmPurchaseReport(PurchaseReportSettings PurchaseReport)
        {
            InitializeComponent();
            try
            {
                m_PurchaseReport = PurchaseReport;
                //m_PurchaseReport = new PurchaseReportSettings();
                ////For Voucher_No
                //m_PurchaseReport.VoucherNo = PurchaseReport.VoucherNo;
                //m_PurchaseReport.VoucherNoAll = PurchaseReport.VoucherNoAll;
                //m_PurchaseReport.VocherNoSingle = PurchaseReport.VocherNoSingle;
                //m_PurchaseReport.VoucherNoSingleValue = PurchaseReport.VoucherNoSingleValue;

                ////For Product
                //m_PurchaseReport.IsProductwise = PurchaseReport.IsProductwise;
                //m_PurchaseReport.IsProductAll = PurchaseReport.IsProductAll;
                //m_PurchaseReport.ProductSingleID = PurchaseReport.ProductSingleID;
                //m_PurchaseReport.ProductGroupID = PurchaseReport.ProductGroupID;

                ////For Party
                //m_PurchaseReport.IsPartywise = PurchaseReport.IsPartywise;
                //m_PurchaseReport.IsPartywise = PurchaseReport.IsPartywise;
                //m_PurchaseReport.IsPartyAll = PurchaseReport.IsProductAll;
                //m_PurchaseReport.PartySingleID = PurchaseReport.PartySingleID;
                //m_PurchaseReport.PartyGroupID = PurchaseReport.PartyGroupID;

                ////For DateRange
                //m_PurchaseReport.IsDateRange = PurchaseReport.IsDateRange;
                //m_PurchaseReport.FromDate = PurchaseReport.FromDate;
                //m_PurchaseReport.ToDate = PurchaseReport.ToDate;

                ////For PurchaseAccount
                //m_PurchaseReport.PurchaseLedgerID = PurchaseReport.PurchaseLedgerID;

                ////for Project
                //m_PurchaseReport.ProjectID = PurchaseReport.ProjectID;

                ////for Depot
                //m_PurchaseReport.DepotID = PurchaseReport.DepotID;

                //m_PurchaseReport.AccClassID = PurchaseReport.AccClassID;
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            

        }
        private void DisplayBannar()
        {
            //CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            //lblCompanyName.Text = m_CompanyDetails.CompanyName;
            //lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            //lblContact.Text = "Contact: " + m_CompanyDetails.Telephone + "Website: " + m_CompanyDetails.Website;
            //lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;

            if (m_PurchaseReport.IsPartywise)
            {
                lblReortType.Text = "Purchase-Partywise Report";
                this.ClientSize = new System.Drawing.Size(994, 633);
            }
            else if (m_PurchaseReport.IsProductwise)
            {
                lblReortType.Text = "Purchase-Productwise Report";
            }
            
            //If it has a date range
            if (m_PurchaseReport.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_PurchaseReport.ToDate);
            }
            else
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }
            
            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear.ToString();
           // lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FYFrom));


            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_PurchaseReport.ProjectID), LangMgr.DefaultLanguage);
            if (m_PurchaseReport.ProjectID != null)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_PurchaseReport.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_PurchaseReport.AccClassID.AddRange(arrChildAccClassIDs);

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
                    tw.WriteStartElement("AccClassIDSettings");
                    foreach (string tag in m_PurchaseReport.AccClassID)
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

        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_PurchaseReport.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_PurchaseReport.ProjectID + "'";

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
            tw.WriteStartElement("PURCHASEREPORT");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ProjectIDSettings");
                    tw.WriteElementString("ProjectID", Convert.ToInt32(m_PurchaseReport.ProjectID).ToString());
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

        private void CreateColumnsforTemp()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("ID", typeof(string));
            dtTemp.Columns.Add("Details",typeof(string));
            dtTemp.Columns.Add("Unit", typeof(string));
            dtTemp.Columns.Add("Rate", typeof(string));
            dtTemp.Columns.Add("PurchQty", typeof(string));
            dtTemp.Columns.Add("ReturnQty", typeof(string));
            dtTemp.Columns.Add("NetPurchQty", typeof(string));
            dtTemp.Columns.Add("NetPurchAmt", typeof(string));
        }

        private void CreateData()
        {
            try
            {
                TotalAmount = 0;
                CreateColumnsforTemp();
                string AccClassIDsXMLString = ReadAllAccClassID();
                string ProjectIDsXMLString = ReadAllProjectID();
                DataTable dtPurcahseReportByProduct;
                if (m_PurchaseReport.IsProductwise)//Purchase Report According to productwise
                {
                    dtPurcahseReportByProduct = Purchase.GetPurchaseReport(m_PurchaseReport.PurchaseLedgerID, m_PurchaseReport.ProductGroupID,
                        m_PurchaseReport.ProductSingleID, m_PurchaseReport.PartyGroupID, m_PurchaseReport.PartySingleID, m_PurchaseReport.DepotID, 
                        m_PurchaseReport.ProjectID, m_PurchaseReport.FromDate, m_PurchaseReport.ToDate, InventoryReportType.ProductWise, AccClassIDsXMLString, 
                        ProjectIDsXMLString);

                    if (dtPurcahseReportByProduct.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPurcahseReportByProduct.Rows)
                        {
                            dtTemp.Rows.Add(dr["productID"].ToString(), dr["ProductName"].ToString(), dr["Unit"].ToString(), Convert.ToDecimal(dr["PurchaseRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["InBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["OutBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Convert.ToInt32(dr["InBound"]) - Convert.ToInt32(dr["OutBound"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            TotalAmount += Convert.ToDecimal(dr["Amount"]);
                        }
                    }
                }
                else if (m_PurchaseReport.IsPartywise)
                {
                    dtPurcahseReportByProduct = Purchase.GetPurchaseReport(m_PurchaseReport.PurchaseLedgerID, m_PurchaseReport.ProductGroupID, m_PurchaseReport.ProductSingleID, m_PurchaseReport.PartyGroupID, m_PurchaseReport.PartySingleID, m_PurchaseReport.DepotID, m_PurchaseReport.ProjectID, m_PurchaseReport.FromDate, m_PurchaseReport.ToDate, InventoryReportType.PartyWise, AccClassIDsXMLString, ProjectIDsXMLString);

                    if (dtPurcahseReportByProduct.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPurcahseReportByProduct.Rows)
                        {
                            dtTemp.Rows.Add(dr["PartyID"].ToString(), dr["PartyName"].ToString(), " ", " ", Convert.ToDecimal(dr["InBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["OutBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Convert.ToInt32(dr["InBound"]) - Convert.ToInt32(dr["OutBound"])).ToString(), Convert.ToDecimal(dr["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            TotalAmount += Convert.ToDecimal(dr["Amount"]);

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }


        }
       private void  FillGridWithData()
        {
            CreateData();
            grdPurchaseReport.Visible = false;
            dgPurchaseReport.Visible = true;
            dgPurchaseReport.SelectionMode = SourceGrid.GridSelectionMode.Row;
            dgPurchaseReport.Selection.EnableMultiSelection = false;

            #region datagrid binding
            DataView mView = new DataView(dtTemp);
            mView.AllowDelete = false;
            mView.AllowNew = false;
            dgPurchaseReport.Columns.Clear(); // first clear all columns to reload the data in dgDayBook

            // dataGrid.Columns.AutoSizeView();
            dgPurchaseReport.FixedRows = 1;
            //  dataGrid.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dataGrid));
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);
            //Create default columns          
            CreateDataGridColumns(dgPurchaseReport.Columns, bd);
            dgPurchaseReport.DataSource = bd;
            dgPurchaseReport.Width = dgPurchaseReport.Width - 5;
            dgPurchaseReport.Width = this.Width - 25;
            #endregion



        }

       private void CreateDataGridColumns(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
       {
           SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
           viewColumnHeader.Font = new Font("Arial", 10, FontStyle.Bold);
           viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

           SourceGrid.Cells.Views.Cell amountview = new SourceGrid.Cells.Views.Cell();
           amountview.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
           amountview.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
          

           string Detail = " ";
           if (m_PurchaseReport.IsProductwise)
           {
               Detail = "Product Details";
           }
           else if(m_PurchaseReport.IsPartywise)
               Detail = "Party Details";

           SourceGrid.DataGridColumn gridColumn;

           gridColumn = dgPurchaseReport.Columns.Add("ID", "ID", new SourceGrid.Cells.DataGrid.Cell());
           gridColumn.HeaderCell.View = viewColumnHeader;
           gridColumn.Width = 50;
           gridColumn.Visible = false;

           gridColumn = dgPurchaseReport.Columns.Add("Details", Detail.ToString(), new SourceGrid.Cells.DataGrid.Cell());
           gridColumn.HeaderCell.View = viewColumnHeader;
          // gridColumn.Width = 150;
           gridColumn.DataCell.AddController(dblClick);
           gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;


           gridColumn = dgPurchaseReport.Columns.Add("Unit", "Unit", new SourceGrid.Cells.DataGrid.Cell());
           gridColumn.HeaderCell.View = viewColumnHeader;
           gridColumn.Width = 100;
           gridColumn.DataCell.AddController(dblClick);
           if (m_PurchaseReport.IsPartywise)
               gridColumn.Visible = false;
           gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


           gridColumn = dgPurchaseReport.Columns.Add("Rate", "Purchase Rate", new SourceGrid.Cells.DataGrid.Cell());
           gridColumn.HeaderCell.View = viewColumnHeader;
           gridColumn.Width = 120;
           gridColumn.DataCell.View = amountview;
           if (m_PurchaseReport.IsPartywise)
               gridColumn.Visible = false;
           gridColumn.DataCell.AddController(dblClick);
           gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


           gridColumn = dgPurchaseReport.Columns.Add("PurchQty", "Purch Qty", new SourceGrid.Cells.DataGrid.Cell());
           gridColumn.HeaderCell.View = viewColumnHeader;
           gridColumn.Width = 100;
           gridColumn.DataCell.AddController(dblClick);
           gridColumn.DataCell.View = amountview;
           gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

           gridColumn = dgPurchaseReport.Columns.Add("ReturnQty", "Return Qty", new SourceGrid.Cells.DataGrid.Cell());
           gridColumn.HeaderCell.View = viewColumnHeader;
           gridColumn.Width = 100;
            gridColumn.DataCell.AddController(dblClick);
           gridColumn.DataCell.View = amountview;
           gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


           gridColumn = dgPurchaseReport.Columns.Add("NetPurchQty", "Net Purch Qty", new SourceGrid.Cells.DataGrid.Cell());
           gridColumn.HeaderCell.View = viewColumnHeader;
           gridColumn.Width = 120;
           gridColumn.DataCell.AddController(dblClick);
           gridColumn.DataCell.View = amountview;
           gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

           gridColumn = dgPurchaseReport.Columns.Add("NetPurchAmt", "Net Purch Amt", new SourceGrid.Cells.DataGrid.Cell());
           gridColumn.HeaderCell.View = viewColumnHeader;
           gridColumn.Width = 120;
           gridColumn.DataCell.View = amountview;
           gridColumn.DataCell.AddController(dblClick);
           gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

           dgPurchaseReport.AutoStretchColumnsToFitWidth = true;


           foreach (SourceGrid.DataGridColumn col in columns)
           {
               SourceGrid.Conditions.ICondition condition =
                   SourceGrid.Conditions.ConditionBuilder.AlternateView(col.DataCell.View,
                                                                          Global.Grid_Color, Color.Black);
               col.Conditions.Add(condition);
           }

       }

        private void DisplayPurchaseReport(bool IsCrystalReport)
        {
            #region BLOCK FOR ORIENTATION PURPOSE
            if (!IsCrystalReport)//Orientaion Purpose is essential only for SOurcegrid not for Crystal report
            {
                DisplayBannar();
                //Let the whole row to be selected
                grdPurchaseReport.SelectionMode = SourceGrid.GridSelectionMode.Row;

                //Disable multiple selection
                grdPurchaseReport.Selection.EnableMultiSelection = false;
                //Disable multiple selection
                grdPurchaseReport.Redim(2, 9);
                int rows = grdPurchaseReport.Rows.Count;
                grdPurchaseReport.Rows.Insert(rows);
                //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
                //dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                //dblClick.DoubleClick += new EventHandler(grdPurchaseReport_DoubleClick);
            }
            #endregion
            string AccClassIDsXMLString = ReadAllAccClassID();

            string ProjectIDsXMLString = ReadAllProjectID();
            //IF Purchase Report has to be shown according to Productwise
            if (m_PurchaseReport.IsProductwise)//Purchase Report According to productwise
            {
                if (!IsCrystalReport)//DOnt show header for Crystal Report
                    MakeHeader(false);
                DataTable dtPurcahseReportByProduct = Purchase.GetPurchaseReport(m_PurchaseReport.PurchaseLedgerID, m_PurchaseReport.ProductGroupID, m_PurchaseReport.ProductSingleID, m_PurchaseReport.PartyGroupID, m_PurchaseReport.PartySingleID, m_PurchaseReport.DepotID, m_PurchaseReport.ProjectID, m_PurchaseReport.FromDate, m_PurchaseReport.ToDate, InventoryReportType.ProductWise, AccClassIDsXMLString, ProjectIDsXMLString);
                WritePurchaseReport(dtPurcahseReportByProduct, false, IsCrystalReport);

            }
            else if (m_PurchaseReport.IsPartywise)
            {
                if (!IsCrystalReport)
                    MakeHeader(true);
                DataTable dtPurcahseReportByProduct = Purchase.GetPurchaseReport(m_PurchaseReport.PurchaseLedgerID, m_PurchaseReport.ProductGroupID, m_PurchaseReport.ProductSingleID, m_PurchaseReport.PartyGroupID, m_PurchaseReport.PartySingleID, m_PurchaseReport.DepotID, m_PurchaseReport.ProjectID, m_PurchaseReport.FromDate, m_PurchaseReport.ToDate, InventoryReportType.PartyWise, AccClassIDsXMLString, ProjectIDsXMLString);
                if (dtPurcahseReportByProduct == null)
                    return;
                WritePurchaseReport(dtPurcahseReportByProduct, true, IsCrystalReport);
            }
        }

        private void frmPurchaseReport_Load(object sender, EventArgs e)
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

            DisplayBannar();

            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(dgPurchaseReport_DoubleClick);

            ProgressForm.UpdateProgress(20, "Initializing report...");

            FillGridWithData();
            //DisplayPurchaseReport(false);

            //For Closig Progress BAR
            ProgressForm.UpdateProgress(100, "Preparing report for display...");
            // Close the dialog if it hasn't been already
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            this.WindowState = FormWindowState.Minimized;

            this.WindowState = FormWindowState.Maximized;

        }


        private void WritePurchaseReport(DataTable dt, bool IsPartywise, bool IsCrystalReport)
        {
            
            for (int i = 1; i <= dt.Rows.Count; i++)
            {


                DataRow dr = dt.Rows[i - 1];
                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (i % 2 == 0)
                {
                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                string ProdName, PartyName;
                ProdName = PartyName = "";
                decimal m_NetPurchaseQty = 0;
                decimal inBound = Convert.ToDecimal(dr["InBound"]);
                decimal outBound = Convert.ToDecimal(dr["OutBound"]);
                m_NetPurchaseQty =  outBound - inBound ;
                Amount = Convert.ToDouble(dr["Amount"]);
                m_totalAmount += Amount;
                //double Amount = Convert.ToDouble(dr["Amount"]);
                //for writting on Grid
                if (!IsCrystalReport)
                {
                    
                    grdPurchaseReport.Rows.Insert(i+1);
                    if (!IsPartywise)//IF Product wise
                    {
                        ProdName = dr["productName"].ToString();
                        grdPurchaseReport[i, 0] = new SourceGrid.Cells.Cell(ProdName);
                        grdPurchaseReport[i, 0].AddController(dblClick);
                        grdPurchaseReport[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                        
                        grdPurchaseReport[i, 1] = new SourceGrid.Cells.Cell(dr["Unit"].ToString());
                        grdPurchaseReport[i, 1].AddController(dblClick);
                        grdPurchaseReport[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

                        //Purchase rate is only displayed for Product wise Report
                        decimal PurchaseRate = Convert.ToDecimal(dr["PurchaseRate"]);
                
                        grdPurchaseReport[i, 2] = new SourceGrid.Cells.Cell(PurchaseRate.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdPurchaseReport[i, 2].AddController(dblClick);
                        grdPurchaseReport[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                    }
                    else//IF party wise
                    {

                        DataTable dtPartytInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                        DataRow drPartyInfo = dtPartytInfo.Rows[0];
                        PartyName = drPartyInfo["LedName"].ToString();
                        grdPurchaseReport[i, 0] = new SourceGrid.Cells.Cell(PartyName);
                        grdPurchaseReport[i, 0].AddController(dblClick);
                        grdPurchaseReport[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                        grdPurchaseReport[i, 1] = new SourceGrid.Cells.Cell("unit");
                        grdPurchaseReport[i, 1].AddController(dblClick);
                        grdPurchaseReport[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

                    }
                    grdPurchaseReport[i, 3] = new SourceGrid.Cells.Cell(outBound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdPurchaseReport[i, 3].AddController(dblClick);
                    grdPurchaseReport[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                    grdPurchaseReport[i, 4] = new SourceGrid.Cells.Cell(inBound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdPurchaseReport[i, 4].AddController(dblClick);
                    grdPurchaseReport[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);

                    grdPurchaseReport[i, 5] = new SourceGrid.Cells.Cell(m_NetPurchaseQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grdPurchaseReport[i, 5].AddController(dblClick);
                    grdPurchaseReport[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
                    grdPurchaseReport[i, 6] = new SourceGrid.Cells.Cell((Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
                    grdPurchaseReport[i, 6].AddController(dblClick);
                    grdPurchaseReport[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
                    if (!IsPartywise)//for productwise
                    {
                        grdPurchaseReport[i, 7] = new SourceGrid.Cells.Cell(dr["ProductID"].ToString());
                        grdPurchaseReport[i, 8] = new SourceGrid.Cells.Cell(InventoryReportType.ProductWise.ToString());
                        
                    }
                    else//for partywise
                    {
                        grdPurchaseReport[i, 7] = new SourceGrid.Cells.Cell(dr["PartyID"].ToString());
                        grdPurchaseReport[i, 8] = new SourceGrid.Cells.Cell(InventoryReportType.PartyWise.ToString());
                    }
                    
                }

                else //for writting on Crystal Report
                {
                    if (m_PurchaseReport.IsPartywise)//According to partywise
                    {

                        DataTable dtPartytInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                        DataRow drPartyInfo = dtPartytInfo.Rows[0];
                        PartyName = drPartyInfo["LedName"].ToString();
                        dsPurchaseReport.Tables["tblParty"].Rows.Add(PartyName, "Unit", dr["OutBound"].ToString(), dr["InBound"].ToString(), m_NetPurchaseQty.ToString(), (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));

                    }
                    else if (m_PurchaseReport.IsProductwise) //Accordint to productwise
                    {
                        m_totalAmount += Amount;
                        ProdName = dr["productName"].ToString();
                        string Unit = dr["Unit"].ToString();
                        decimal PurchaseRate = Convert.ToDecimal(dr["PurchaseRate"]);
                        dsPurchaseReport.Tables["tblProduct"].Rows.Add(ProdName, Unit, dr["OutBound"].ToString(), dr["InBound"].ToString(), m_NetPurchaseQty.ToString(), (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))), PurchaseRate.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    }

                }
            }
            
            
           // double TAmount = Convert.ToDouble(drT["Amount"]);
            int no = dt.Rows.Count;
            grdPurchaseReport[no + 1, 0] = new SourceGrid.Cells.Cell("Total");
            grdPurchaseReport[no + 1, 1] = new SourceGrid.Cells.Cell("");
            grdPurchaseReport[no + 1, 2] = new SourceGrid.Cells.Cell("");
            grdPurchaseReport[no + 1, 3] = new SourceGrid.Cells.Cell("");
            grdPurchaseReport[no + 1, 4] = new SourceGrid.Cells.Cell("");
            grdPurchaseReport[no + 1, 5] = new SourceGrid.Cells.Cell("");
             grdPurchaseReport[no + 1, 6] = new SourceGrid.Cells.Cell(m_totalAmount.ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces)));
            
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
                grdPurchaseReport[0, 0] = new MyHeader("Party Details");
            else
                grdPurchaseReport[0, 0] = new MyHeader("Product Details");
                
            grdPurchaseReport[0, 1] = new MyHeader("Unit");
            grdPurchaseReport[0, 2] = new MyHeader("Rate");
            grdPurchaseReport[0, 3] = new MyHeader("Purch Qty");
            grdPurchaseReport[0, 4] = new MyHeader("Return Qty");
            grdPurchaseReport[0, 5] = new MyHeader("Net Purch Qty");
            grdPurchaseReport[0, 6] = new MyHeader("Net Purch Amt.");
            if (IsPartywise)
                grdPurchaseReport[0, 7] = new MyHeader("Party ID");
            else
                grdPurchaseReport[0, 7] = new MyHeader("Product ID");

            grdPurchaseReport[0, 8] = new MyHeader("Report Type");

            //Define size of column
            grdPurchaseReport[0, 0].Column.Width = 390;
            grdPurchaseReport[0, 1].Column.Width = 80;
            grdPurchaseReport[0, 2].Column.Width = 100;

            if (IsPartywise)
            {
                grdPurchaseReport[0, 1].Column.Visible = false;
                grdPurchaseReport[0, 2].Column.Visible = false;

                grdPurchaseReport[0, 3].Column.Width = 120;
                grdPurchaseReport[0, 4].Column.Width = 120;
                grdPurchaseReport[0, 5].Column.Width = 120;
                grdPurchaseReport[0, 6].Column.Width = 220;
            }
            else
            {
                grdPurchaseReport[0, 3].Column.Width = 100;
                grdPurchaseReport[0, 4].Column.Width = 100;
                grdPurchaseReport[0, 5].Column.Width = 100;
                grdPurchaseReport[0, 6].Column.Width = 200;
            }
            grdPurchaseReport[0, 7].Column.Width = 50;
            grdPurchaseReport[0, 8].Column.Width = 80;
            grdPurchaseReport[0, 7].Column.Visible = false;
            grdPurchaseReport[0, 8].Column.Visible = false;
            

        }

        private void dgPurchaseReport_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //Get the Selected Row           
                int CurRow = dgPurchaseReport.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(dgPurchaseReport, new SourceGrid.Position(CurRow, 0));
                //read id                       
                string ID = (cellTypeID.Value).ToString();//
                if (ID == "0")//IF ID is blank        
                    return;
                PurchasePartyTransactSettings m_PurchaseParty = new PurchasePartyTransactSettings();
                if (m_PurchaseReport.IsProductwise)
                {
                    m_PurchaseParty.ProductID = Convert.ToInt32(ID);
                    m_PurchaseParty.ReportType=InventoryReportType.ProductWise.ToString();
                }
                else if( m_PurchaseReport.IsPartywise)
                {
                    m_PurchaseParty.PartyID = Convert.ToInt32(ID);
                    m_PurchaseParty.ReportType=InventoryReportType.PartyWise.ToString();
                }
                m_PurchaseParty.PurchaseLedgerID = m_PurchaseReport.PurchaseLedgerID;
                m_PurchaseParty.DepotID = m_PurchaseReport.DepotID;
                m_PurchaseParty.ProjectID = m_PurchaseReport.ProjectID;
                m_PurchaseParty.FromDate = m_PurchaseReport.FromDate;
                m_PurchaseParty.ToDate = m_PurchaseReport.ToDate;
                m_PurchaseParty.AccClassID = m_PurchaseReport.AccClassID;
                frmProductPartyTransaction frm = new frmProductPartyTransaction(m_PurchaseParty);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
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

            dsPurchaseReport.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed


            //otherwise it populate the records again and again
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;

            //rptBalanceSheet rpt = new rptBalanceSheet();
           rptPurchaseReportByProduct m_rptPurchaseReportByProduct = new rptPurchaseReportByProduct();
            rptPurchaseReportByParty m_rptPurchaseReportByParty = new rptPurchaseReportByParty();


            //Fill the logo on the report
            //Misc.WriteLogo(dsPurchaseReport, "tblImage");
           // Set DataSource to be dsTrial dataset on the report
           // rpt.SetDataSource(dsPurchaseReport);
            if (m_PurchaseReport.IsProductwise)
            {
                Misc.WriteLogo(dsPurchaseReport, "tblImage");
                m_rptPurchaseReportByProduct.SetDataSource(dsPurchaseReport);
                try
                {
                    dsPurchaseReport.Tables.Remove("tblProduct");
                }
                catch
                {
                }
                System.Data.DataView view = new System.Data.DataView(dtTemp);
                DataTable selected = view.ToTable("tblProduct", false, "Details", "Unit", "Rate", "PurchQty", "ReturnQty", "NetPurchQty","NetPurchAmt");

                dsPurchaseReport.Tables.Add(selected);
            }
            else if (m_PurchaseReport.IsPartywise)
            {
                Misc.WriteLogo(dsPurchaseReport, "tblImage");
                m_rptPurchaseReportByParty.SetDataSource(dsPurchaseReport);

                try
                {
                    dsPurchaseReport.Tables.Remove("tblParty");
                }
                catch
                {
                }
                System.Data.DataView view = new System.Data.DataView(dtTemp);
                DataTable selected = view.ToTable("tblParty", false, "Details", "PurchQty", "ReturnQty", "NetPurchQty", "NetPurchAmt");
                dsPurchaseReport.Tables.Add(selected);

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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvTotalAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();

            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            if (m_PurchaseReport.IsProductwise)
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);
            else if (m_PurchaseReport.IsPartywise)
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);


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
                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = m_CompanyDetails.Telephone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);

                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);

                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

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
                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);

                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = companypan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);

                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);

                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);
                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                if (m_PurchaseReport.IsProductwise)
                    m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                else if (m_PurchaseReport.IsPartywise)
                    m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);


            }
            if (m_PurchaseReport.IsProductwise)
            {
                pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvPreparedBy);
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

                pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvCheckedBy);
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

                pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvApprovedBy);
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);

                pdvPrintDate.Value = Date.ToSystem(DateTime.Now);
                pvCollection.Clear();
                pvCollection.Add(pdvPrintDate);
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);
            }
            else if (m_PurchaseReport.IsPartywise)
            {
                pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvPreparedBy);
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

                pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvCheckedBy);
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

                pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
                pvCollection.Clear();
                pvCollection.Add(pdvApprovedBy);
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);

                pdvPrintDate.Value = Date.ToSystem(DateTime.Now);
                pvCollection.Clear();
                pvCollection.Add(pdvPrintDate);
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);
            }
           

          

            if (m_PurchaseReport.IsProductwise)
            {
                pdvTotalAmount.Value = TotalAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                pvCollection.Clear();
                pvCollection.Add(pdvTotalAmount);
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["TotalAmount"].ApplyCurrentValues(pvCollection);
            }
            else if (m_PurchaseReport.IsPartywise)
            {
                pdvTotalAmount.Value = TotalAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                pvCollection.Clear();
                pvCollection.Add(pdvTotalAmount);
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["partyWiseTotal"].ApplyCurrentValues(pvCollection);
            }
            pdvCompany_Slogan.Value = m_CompanyDetails.Website;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Slogan);
            if (m_PurchaseReport.IsProductwise)
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
            else if (m_PurchaseReport.IsPartywise)
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            if (m_PurchaseReport.IsProductwise)
            {
                pdvReport_Head.Value = "Purchase-Productwise Report";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
            }
            else if (m_PurchaseReport.IsPartywise)
            {
                pdvReport_Head.Value = "Purchase-Partywise Report";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);
            }

            pdvFiscal_Year.Value = "Fiscal Year:" + (m_CompanyDetails.FiscalYear);
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);

            if (m_PurchaseReport.IsProductwise)
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);
               
            else if (m_PurchaseReport.IsPartywise)
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);


            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_PurchaseReport.FromDate != null && m_PurchaseReport.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseReport.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseReport.ToDate));
            }
            if (m_PurchaseReport.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseReport.ToDate));
            }
            if (m_PurchaseReport.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseReport.FromDate));
            }
            if (m_PurchaseReport.FromDate == null && m_PurchaseReport.ToDate == null)
            {
                pdvReport_Date.Value = "";

            }

            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            if (m_PurchaseReport.IsProductwise)
                m_rptPurchaseReportByProduct.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);
            else if (m_PurchaseReport.IsPartywise)
                m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);


          //  DisplayPurchaseReport(true);

            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

            //Finally, show the report form
            Common.frmReportViewer frm = new Common.frmReportViewer();

            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");

            // Close the dialog
            ProgressForm.CloseForm();

            if (m_PurchaseReport.IsPartywise)
            {
                frm.SetReportSource(m_rptPurchaseReportByParty);
                switch (myPrintType)
                {
                    case PrintType.DirectPrint: //Direct Printer
                        m_rptPurchaseReportByParty.PrintOptions.PrinterName = "";
                        m_rptPurchaseReportByParty.PrintToPrinter(1, false, 0, 0);
                        return;
                    case PrintType.Excel: //Excel
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = m_rptPurchaseReportByParty.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        m_rptPurchaseReportByParty.Export();
                        m_rptPurchaseReportByParty.Close();
                        return;
                    case PrintType.PDF: //PDF
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = m_rptPurchaseReportByParty.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        m_rptPurchaseReportByParty.Export();
                        m_rptPurchaseReportByParty.Close();
                        return;
                    case PrintType.Email:// saves as excel document(by default) and sends via email
                        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                        CrExportOptions = m_rptPurchaseReportByParty.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                        m_rptPurchaseReportByParty.Export();
                        Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        m_rptPurchaseReportByParty.Close();
                        return;
                    default: //Crystal Report
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }


            }
            else if (m_PurchaseReport.IsProductwise)
            {
                frm.SetReportSource(m_rptPurchaseReportByProduct);
                switch (myPrintType)
                {
                    case PrintType.DirectPrint: //Direct Printer
                        m_rptPurchaseReportByProduct.PrintOptions.PrinterName = "";
                        m_rptPurchaseReportByProduct.PrintToPrinter(1, false, 0, 0);
                        return;
                    case PrintType.Excel: //Excel
                        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                        CrExportOptions = m_rptPurchaseReportByProduct.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        m_rptPurchaseReportByProduct.Export();
                        m_rptPurchaseReportByProduct.Close();
                        return;
                    case PrintType.PDF: //PDF
                        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                        CrExportOptions = m_rptPurchaseReportByProduct.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                        m_rptPurchaseReportByProduct.Export();
                        m_rptPurchaseReportByProduct.Close();
                        return;
                    case PrintType.Email:
                        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                        CrExportOptions = m_rptPurchaseReportByProduct.ExportOptions;
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                        m_rptPurchaseReportByProduct.Export();
                        Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        m_rptPurchaseReportByProduct.Close();
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
            //Clear all rows
           // grdPurchaseReport.Redim(0, 0);

            //Reset the total variables
            grdPurchaseReport.Columns.Clear();
            m_totalAmount = 0;
            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmPurchaseReport_Load(sender, e);
             
            grdPurchaseReport.Refresh();
          
            this.Cursor = Cursors.Default;
            this.WindowState = FormWindowState.Maximized;
            
            //Show complete notification
            //Global.Msg("Reload Complete!");
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void grdPurchaseReport_Resize(object sender, EventArgs e)
        {
        }

        private void grdPurchaseReport_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgPurchaseReport_Paint(object sender, PaintEventArgs e)
        {

        }       

    }
}
