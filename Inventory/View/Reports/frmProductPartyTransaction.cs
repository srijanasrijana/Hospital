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
    public partial class frmProductPartyTransaction : Form
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private Inventory.Model.dsPurchaseReport dsPurchaseReport = new Model.dsPurchaseReport();
        private PurchasePartyTransactSettings m_PurchaseParty;
        private string FileName = "";
        private string ProductParty_Pivot = "";
        ArrayList AccClassID = new ArrayList();
        private DataTable dtTemp;
        private decimal TotalAmount = 0;

        ContextMenu Menu_Export;

        public frmProductPartyTransaction()
        {
            InitializeComponent();
        }
        public frmProductPartyTransaction(PurchasePartyTransactSettings PurchaseParty)
        {
            InitializeComponent();
            m_PurchaseParty=new PurchasePartyTransactSettings();
            m_PurchaseParty.PartyID = PurchaseParty.PartyID;
            m_PurchaseParty.ProductID = PurchaseParty.ProductID;
            m_PurchaseParty.ReportType = PurchaseParty.ReportType;
            m_PurchaseParty.PurchaseLedgerID = PurchaseParty.PurchaseLedgerID;
            m_PurchaseParty.DepotID = PurchaseParty.DepotID;
            m_PurchaseParty.PartyID = PurchaseParty.PartyID;
            m_PurchaseParty.FromDate = PurchaseParty.FromDate;
            m_PurchaseParty.ToDate = PurchaseParty.ToDate;
            m_PurchaseParty.AccClassID = PurchaseParty.AccClassID;
        }

        private void MakeHeader()
        {            
            //Defining the HeaderPart
                grdPurchaseProductTransaction[0, 0] = new MyHeader("Date");
                grdPurchaseProductTransaction[0, 1] = new MyHeader("Party");
                grdPurchaseProductTransaction[0, 2] = new MyHeader("Voucher Num.");
                grdPurchaseProductTransaction[0, 3] = new MyHeader("Net Purch. Qty.");
                grdPurchaseProductTransaction[0, 4] = new MyHeader("Net Return Qty.");
                grdPurchaseProductTransaction[0, 5] = new MyHeader("Unit");
                grdPurchaseProductTransaction[0, 6] = new MyHeader("Amount");
                grdPurchaseProductTransaction[0, 7] = new MyHeader("RowID");
                grdPurchaseProductTransaction[0, 8] = new MyHeader("VoucherType");

            //Define size of column
                grdPurchaseProductTransaction[0, 0].Column.Width = 90;
                grdPurchaseProductTransaction[0, 1].Column.Width = 170;
                if (m_PurchaseParty.ReportType == InventoryReportType.PartyWise.ToString())
                {
                    grdPurchaseProductTransaction[0, 1].Column.Visible = false;
                    grdPurchaseProductTransaction[0, 2].Column.Width = 200;
                    grdPurchaseProductTransaction[0, 6].Column.Width = 220;
                }
                else
                {
                    grdPurchaseProductTransaction[0, 2].Column.Width = 170;
                    grdPurchaseProductTransaction[0, 6].Column.Width = 190;
                }
                grdPurchaseProductTransaction[0, 3].Column.Width = 120;
                grdPurchaseProductTransaction[0, 4].Column.Width = 120;
                grdPurchaseProductTransaction[0, 5].Column.Width = 100;
                  
                  grdPurchaseProductTransaction[0, 7].Column.Width = 50;
                  grdPurchaseProductTransaction[0, 8].Column.Width = 50;
                  grdPurchaseProductTransaction[0, 5].Column.Visible = false;
                  grdPurchaseProductTransaction[0, 7].Column.Visible = false;
                  grdPurchaseProductTransaction[0, 8].Column.Visible = false;
                 
        }

        private void lblPanNo_Click(object sender, EventArgs e)
        {

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
            lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            lblContact.Text = "Contact: " + m_CompanyDetails.Telephone + "  Website: " + m_CompanyDetails.Website;
            lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;
            if (m_PurchaseParty.FromDate != null && m_PurchaseParty.ToDate != null)
            {
                lblAllSettings.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseParty.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseParty.ToDate));
            }
            if (m_PurchaseParty.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_PurchaseParty.ToDate);
                lblAllSettings.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseParty.ToDate));
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }
            if (m_PurchaseParty.FromDate != null)
            {
                //This is actually not applicable
                lblAllSettings.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseParty.FromDate));
            }
            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FYFrom));

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_PurchaseParty.ProjectID), LangMgr.DefaultLanguage);
            if (m_PurchaseParty.ProjectID != null)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];
                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
            
            //Display pivot value on control
            if (m_PurchaseParty.ProductID > 0)
            {
                DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(m_PurchaseParty.ProductID), LangMgr.DefaultLanguage);
                DataRow drProductInfo = dtProductInfo.Rows[0];
                ProductParty_Pivot ="Product : " + drProductInfo["ProdName"].ToString();
                lblProductPartyPivot.Text = ProductParty_Pivot ;
            }
            else if (m_PurchaseParty.PartyID > 0)
            {
                DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(m_PurchaseParty.PartyID), LangMgr.DefaultLanguage);
                DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                ProductParty_Pivot="Party : " + drLedgerInfo["LedName"].ToString();
                lblProductPartyPivot.Text = ProductParty_Pivot;
            }
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID
            
            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_PurchaseParty.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_PurchaseParty.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in m_PurchaseParty.AccClassID)
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
        private void WriteProductPartyTransactionOnGrid(string PurchaseReportType1,DataTable dt,bool IsCrystalReport)
        {
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i - 1];
                string PartyName = "";
                double Amount = Convert.ToDouble(dr["Amount"]);
                if (!IsCrystalReport)
                {
                   
                    grdPurchaseProductTransaction.Rows.Insert(i);
                    DateTime dtVoucherDate = Convert.ToDateTime(dr["VoucherDate"]);
                   // grdPurchaseProductTransaction[i, 0] = new SourceGrid.Cells.Cell(dtVoucherDate.ToShortDateString());
                    grdPurchaseProductTransaction[i, 0] = new SourceGrid.Cells.Cell(Date.ToSystem( dtVoucherDate));
                    grdPurchaseProductTransaction[i, 0].AddController(dblClick);
                    if (PurchaseReportType1 == InventoryReportType.ProductTransactWise.ToString())
                    {
                        //Incase of Productwise Transaction,need to show Party account
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                        DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                        PartyName = drLedgerInfo["LedName"].ToString();
                        grdPurchaseProductTransaction[i, 1] = new SourceGrid.Cells.Cell(PartyName);
                        grdPurchaseProductTransaction[i, 1].AddController(dblClick);
                    }
                    else if (PurchaseReportType1 == InventoryReportType.PartyTransactWise.ToString())
                    {
                        grdPurchaseProductTransaction[i, 1] = new SourceGrid.Cells.Cell("");
                    }
                    grdPurchaseProductTransaction[i, 2] = new SourceGrid.Cells.Cell(dr["VoucherNo"].ToString());
                    grdPurchaseProductTransaction[i, 2].AddController(dblClick);

                    grdPurchaseProductTransaction[i, 3] = new SourceGrid.Cells.Cell(dr["OutBound"].ToString());
                    grdPurchaseProductTransaction[i, 3].AddController(dblClick);

                    grdPurchaseProductTransaction[i, 4] = new SourceGrid.Cells.Cell(dr["InBound"].ToString());
                    grdPurchaseProductTransaction[i, 4].AddController(dblClick);

                    grdPurchaseProductTransaction[i, 5] = new SourceGrid.Cells.Cell("Unit");
                    grdPurchaseProductTransaction[i, 5].AddController(dblClick);

                    grdPurchaseProductTransaction[i, 6] = new SourceGrid.Cells.Cell((Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
                    grdPurchaseProductTransaction[i, 6].AddController(dblClick);

                    grdPurchaseProductTransaction[i, 7] = new SourceGrid.Cells.Cell(dr["RowID"].ToString());
                    grdPurchaseProductTransaction[i, 8] = new SourceGrid.Cells.Cell(dr["VoucherType"].ToString());
                }
                else//for writting on crystal report
                {
                    DateTime dtVoucherDate = Convert.ToDateTime(dr["VoucherDate"]);
                    if (PurchaseReportType1 == InventoryReportType.ProductTransactWise.ToString())
                    {                        
                        //Incase of Productwise Transaction,need to show Party account
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                        DataRow drLedgerInfo = dtLedgerInfo.Rows[0];
                        PartyName = drLedgerInfo["LedName"].ToString();
                        dsPurchaseReport.Tables["tblProductTransact"].Rows.Add(Date.ToSystem( dtVoucherDate), PartyName, dr["VoucherNo"].ToString(), dr["OutBound"].ToString(), dr["InBound"].ToString(), "Unit", (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));            

                    }
                    else if (PurchaseReportType1 == InventoryReportType.PartyTransactWise.ToString())
                    {
                        dsPurchaseReport.Tables["tblPartyTransact"].Rows.Add(Date.ToSystem(dtVoucherDate), dr["VoucherNo"].ToString(), dr["OutBound"].ToString(), dr["InBound"].ToString(), "Unit", (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));            
                    }

                }
            }
        }

        private void DisplayPurchaseProductPartyTransaction(bool IsCrystalReport)
        {
            
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(dgProductPartyTransaction_DoubleClick);

            DisplayBannar();
            FillGridWithData();

         //   string AccClassIDsXMLString = ReadAllAccClassID();
         //   string ProjectIDsXMLString = ReadAllProjectID();
         //   if (!IsCrystalReport)
         //   {
             
         //       DisplayBannar();
         //       //Let the whole row to be selected
         //       grdPurchaseProductTransaction.SelectionMode = SourceGrid.GridSelectionMode.Row;

         //       //Disable multiple selection
         //       grdPurchaseProductTransaction.Selection.EnableMultiSelection = false;
         //       //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
         //       dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
         //       dblClick.DoubleClick += new EventHandler(grdProductPartyTransaction_DoubleClick);
         //       //Disable multiple selection
         //       grdPurchaseProductTransaction.Redim(1, 9);
         //       int rows = grdPurchaseProductTransaction.Rows.Count;
         //       grdPurchaseProductTransaction.Rows.Insert(rows);
         //       MakeHeader();
         //   }

         //   DataTable dt = new DataTable();
         ////   Purchase m_Purchase = new Purchase();
         //   if (m_PurchaseParty.ReportType == InventoryReportType.ProductWise.ToString())
         //   {
         //       lblReportName.Text = "Purchase-Product Transaction";
         //       dt = Purchase.GetPurchaseReport(m_PurchaseParty.PurchaseLedgerID, m_PurchaseParty.ProductGroupID, m_PurchaseParty.ProductID, m_PurchaseParty.PartyGroupID, m_PurchaseParty.PartyID, m_PurchaseParty.DepotID, m_PurchaseParty.ProjectID, m_PurchaseParty.FromDate, m_PurchaseParty.ToDate, InventoryReportType.ProductTransactWise,AccClassIDsXMLString,ProjectIDsXMLString);
         //       WriteProductPartyTransactionOnGrid(InventoryReportType.ProductTransactWise.ToString(), dt,IsCrystalReport);
         //   }
         //   else if (m_PurchaseParty.ReportType == InventoryReportType.PartyWise.ToString())
         //   {
         //       this.Size = new Size(800, 600);
         //       lblReportName.Text = "Purchase-Party Transaction";
         //       dt = Purchase.GetPurchaseReport(m_PurchaseParty.PurchaseLedgerID, m_PurchaseParty.ProductGroupID, m_PurchaseParty.ProductID, m_PurchaseParty.PartyGroupID, m_PurchaseParty.PartyID, m_PurchaseParty.DepotID, m_PurchaseParty.ProjectID, m_PurchaseParty.FromDate, m_PurchaseParty.ToDate, InventoryReportType.PartyTransactWise, AccClassIDsXMLString, ProjectIDsXMLString);
         //       WriteProductPartyTransactionOnGrid(InventoryReportType.PartyTransactWise.ToString(), dt,IsCrystalReport);

         //   }
        }

        private void CreateColumnForTemp()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("Date", typeof(string));
            dtTemp.Columns.Add("ID", typeof(string));
            dtTemp.Columns.Add("Details", typeof(string));
            //dtTemp.Columns.Add("Unit", typeof(string));
            //dtTemp.Columns.Add("Rate", typeof(string));
            dtTemp.Columns.Add("VoucherNo", typeof(string));
            dtTemp.Columns.Add("PurchQty", typeof(string));
            dtTemp.Columns.Add("ReturnQty", typeof(string));
            dtTemp.Columns.Add("NetAmount", typeof(string));
             dtTemp.Columns.Add("RowID", typeof(string));
            dtTemp.Columns.Add("VoucherType", typeof(string));
        }

        private void FillGridWithData()
        {
            CreateData();
            grdPurchaseProductTransaction.Visible = false;
            dgPurchaseTransRep.Visible = true;
            dgPurchaseTransRep.SelectionMode = SourceGrid.GridSelectionMode.Row;
            dgPurchaseTransRep.Selection.EnableMultiSelection = false;

            #region datagrid binding
            DataView mView = new DataView(dtTemp);
            mView.AllowDelete = false;
            mView.AllowNew = false;
            dgPurchaseTransRep.Columns.Clear(); // first clear all columns to reload the data in dgDayBook

            // dataGrid.Columns.AutoSizeView();
            dgPurchaseTransRep.FixedRows = 1;
            //  dataGrid.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dataGrid));
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);
            //Create default columns          
            CreateDataGridColumns(dgPurchaseTransRep.Columns, bd);
            dgPurchaseTransRep.DataSource = bd;
            dgPurchaseTransRep.Width = dgPurchaseTransRep.Width - 5;
            dgPurchaseTransRep.Width = this.Width - 25;
            #endregion
        }

        private void CreateData()
        {
            TotalAmount = 0;
            try
            {
                CreateColumnForTemp();
                string AccClassIDsXMLString = ReadAllAccClassID();
                string ProjectIDsXMLString = ReadAllProjectID();

                DataTable dt = new DataTable();
                if (m_PurchaseParty.ReportType == InventoryReportType.ProductWise.ToString())
                {
                    lblReportName.Text = "Purchase-Product Transaction";
                    dt = Purchase.GetPurchaseReport(m_PurchaseParty.PurchaseLedgerID, m_PurchaseParty.ProductGroupID, m_PurchaseParty.ProductID, m_PurchaseParty.PartyGroupID, m_PurchaseParty.PartyID, m_PurchaseParty.DepotID, m_PurchaseParty.ProjectID, m_PurchaseParty.FromDate, m_PurchaseParty.ToDate, InventoryReportType.ProductTransactWise, AccClassIDsXMLString, ProjectIDsXMLString);
                    // WriteProductPartyTransactionOnGrid(InventoryReportType.ProductTransactWise.ToString(), dt, IsCrystalReport);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dtTemp.Rows.Add(dr["NepVoucherDate"].ToString(), dr["PartyID"].ToString(), dr["PartyName"].ToString(), dr["VoucherNo"].ToString(), Convert.ToDecimal(dr["InBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["OutBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["RowID"].ToString(), dr["VoucherType"].ToString());
                            TotalAmount += Convert.ToDecimal(dr["Amount"]);
                        }
                    }

                }
                else if (m_PurchaseParty.ReportType == InventoryReportType.PartyWise.ToString())
                {
                    this.Size = new Size(800, 600);
                    lblReportName.Text = "Purchase-Party Transaction";
                    dt = Purchase.GetPurchaseReport(m_PurchaseParty.PurchaseLedgerID, m_PurchaseParty.ProductGroupID, m_PurchaseParty.ProductID, m_PurchaseParty.PartyGroupID, m_PurchaseParty.PartyID, m_PurchaseParty.DepotID, m_PurchaseParty.ProjectID, m_PurchaseParty.FromDate, m_PurchaseParty.ToDate, InventoryReportType.PartyTransactWise, AccClassIDsXMLString, ProjectIDsXMLString);
                    // WriteProductPartyTransactionOnGrid(InventoryReportType.PartyTransactWise.ToString(), dt, IsCrystalReport);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dtTemp.Rows.Add(dr["NepVoucherDate"].ToString(), dr["ProductID"].ToString(), dr["ProductName"].ToString(), dr["VoucherNo"].ToString(), Convert.ToDecimal(dr["InBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["OutBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["RowID"].ToString(), dr["VoucherType"].ToString());
                            TotalAmount += Convert.ToDecimal(dr["Amount"]);

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
            if (m_PurchaseParty.ReportType == InventoryReportType.ProductWise.ToString())
            {
                Detail = "Parties";
            }
            else if (m_PurchaseParty.ReportType == InventoryReportType.PartyWise.ToString())
                Detail = "Products";

            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dgPurchaseTransRep.Columns.Add("Date", "Voucher Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 120;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            gridColumn = dgPurchaseTransRep.Columns.Add("ID", "ID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
            gridColumn.Visible = false;

            gridColumn = dgPurchaseTransRep.Columns.Add("Details", Detail.ToString(), new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            // gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;


            gridColumn = dgPurchaseTransRep.Columns.Add("VoucherNo", "Voucher No.", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 120;
            //gridColumn.DataCell.View = amountview;
            //gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            gridColumn = dgPurchaseTransRep.Columns.Add("PurchQty", " Purch Qty", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 100;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.View = amountview;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgPurchaseTransRep.Columns.Add("ReturnQty", " Return Qty", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 100;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.View = amountview;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            gridColumn = dgPurchaseTransRep.Columns.Add("NetAmount", "  Amount", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 140;
            gridColumn.DataCell.View = amountview;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgPurchaseTransRep.Columns.Add("RowID", "  RowID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
            gridColumn.Visible = false;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgPurchaseTransRep.Columns.Add("VoucherType", "Voucher Type", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 130;
            gridColumn.Visible = false;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            dgPurchaseTransRep.AutoStretchColumnsToFitWidth = true;


            foreach (SourceGrid.DataGridColumn col in columns)
            {
                SourceGrid.Conditions.ICondition condition =
                    SourceGrid.Conditions.ConditionBuilder.AlternateView(col.DataCell.View,
                                                                           Global.Grid_Color, Color.Black);
                col.Conditions.Add(condition);
            }

        }
        private void frmProductTransaction_Load(object sender, EventArgs e)
        {
            DisplayPurchaseProductPartyTransaction(false);
            this.WindowState = FormWindowState.Maximized;
          
        }

        private void dgProductPartyTransaction_DoubleClick(object sender, EventArgs e)
        {
            try
            {                
                //Get the Selected Row
                int CurRow = dgPurchaseTransRep.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellID = new SourceGrid.CellContext(dgPurchaseTransRep, new SourceGrid.Position(CurRow, 7));
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgPurchaseTransRep, new SourceGrid.Position(CurRow, 8));
                if ((cellType.Value.ToString()) != " "&&(cellType.Value.ToString()) != "")//Dont Call the voucher form if there is no Ledger...no need to call Voucher form for Op. Bal/Total Amount etc
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

        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
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
            rptPurchaseReportByProductTransact m_rptPurchaseReportTransact = new rptPurchaseReportByProductTransact();//for both party and product
            // rptPurchaseReportByPartyTransact m_rptPurchaseReportByPartyTransact = new rptPurchaseReportByPartyTransact();

            //Fill the logo on the report
            //Misc.WriteLogo(dsPurchaseReport, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            //rpt.SetDataSource(dsPurchaseReport);
            //if (m_PurchaseParty.ProductID>0)//According to ProductTransact wise
            Misc.WriteLogo(dsPurchaseReport, "tblImage");
            m_rptPurchaseReportTransact.SetDataSource(dsPurchaseReport);

            try
            {
                dsPurchaseReport.Tables.Remove("tblTransact");
            }
            catch
            {
            }
            System.Data.DataView view = new System.Data.DataView(dtTemp);
            DataTable selected = view.ToTable("tblTransact", false, "Date", "Details", "VoucherNo", "PurchQty", "ReturnQty", "NetAmount", "VoucherType");

            dsPurchaseReport.Tables.Add(selected);



            //else if (m_PurchaseParty.PartyID>0)
            //    m_rptPurchaseReportByPartyTransact.SetDataSource(dsPurchaseReport);

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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_TotalAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Detail = new CrystalDecisions.Shared.ParameterDiscreteValue();


            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            // if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
            m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);
            //  else if (m_PurchaseParty.PartyID > 0)
            // m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);


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
                // if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
                m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);
                // else if (m_PurchaseParty.PartyID > 0)
                //  m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = m_CompanyDetails.Address1;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);

                //if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
                m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);
                // else if (m_PurchaseParty.PartyID > 0)
                //  m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);

                //if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
                m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);
                //else if (m_PurchaseParty.PartyID > 0)
                //m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);

                //if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
                m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);
                // else if (m_PurchaseParty.PartyID > 0)
                //m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);

                // if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
                m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                //  else if (m_PurchaseParty.PartyID > 0)
                //   m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
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
                //  m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                // m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = companypan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                // m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                //m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                //  m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            }
            if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
            {
                pdvReport_Head.Value = "Purchase-Productwise Transaction Report";
                pdvReport_Detail.Value = "Products";
            }
            else if (m_PurchaseParty.PartyID > 0)
            {
                pdvReport_Head.Value = "Purchase-Partywise Transaction Report";
                pdvReport_Detail.Value = "Parties";
            }

            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            pvCollection.Clear();
            pvCollection.Add(pdvReport_Detail);
            m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Detail"].ApplyCurrentValues(pvCollection);


            pdvReport_TotalAmount.Value = TotalAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvReport_TotalAmount);
            m_rptPurchaseReportTransact.DataDefinition.ParameterFields["TotalAmount"].ApplyCurrentValues(pvCollection);

            //for pivot writting purpose
            //if (m_PurchaseParty.ProductID > 0)
            //{
            pdvProductParty_Pivot.Value = ProductParty_Pivot;
            pvCollection.Clear();
            pvCollection.Add(pdvProductParty_Pivot);
            m_rptPurchaseReportTransact.DataDefinition.ParameterFields["ProductParty_Pivot"].ApplyCurrentValues(pvCollection);
            //}
            //else if (m_PurchaseParty.PartyID > 0)
            //{
            //    pdvProductParty_Pivot.Value = ProductParty_Pivot;
            //    pvCollection.Clear();
            //    pvCollection.Add(pdvProductParty_Pivot);
            //    m_rptPurchaseReportTransact.DataDefinition.ParameterFields["ProductParty_Pivot"].ApplyCurrentValues(pvCollection);
            //}

            pdvFiscal_Year.Value = "Fiscal Year:" + (m_CompanyDetails.FiscalYear);
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);


            // if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
            m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);
            //else if (m_PurchaseParty.PartyID > 0)
            // m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);


            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_PurchaseParty.FromDate != null && m_PurchaseParty.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseParty.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseParty.ToDate));
            }
            else if (m_PurchaseParty.FromDate == null && m_PurchaseParty.ToDate == null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(DateTime.Today);
            }
            if (m_PurchaseParty.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseParty.ToDate));
            }
            if (m_PurchaseParty.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_PurchaseParty.FromDate)) + " To: " + Date.ToSystem(DateTime.Today);
            }


            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            m_rptPurchaseReportTransact.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            //if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
            // else if (m_PurchaseParty.PartyID > 0)
            //  m_rptPurchaseReportByPartyTransact.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            //DisplayPurchaseProductPartyTransaction(true);          
            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

            //Finally, show the report form
            Common.frmReportViewer frm = new Common.frmReportViewer();
            // if (m_PurchaseParty.ProductID > 0)//According to ProductTransact wise
            frm.SetReportSource(m_rptPurchaseReportTransact);
            // else if (m_PurchaseParty.PartyID > 0)
            // frm.SetReportSource(m_rptPurchaseReportByPartyTransact);
            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");
            // Close the dialog
            ProgressForm.CloseForm();

            switch (myPrintType)
            {
                case PrintType.DirectPrint: //Direct Printer
                    m_rptPurchaseReportTransact.PrintOptions.PrinterName = "";
                    m_rptPurchaseReportTransact.PrintToPrinter(1, false, 0, 0);
                    return;

                case PrintType.Excel: //Excel
                    ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                    CrExportOptions = m_rptPurchaseReportTransact.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    m_rptPurchaseReportTransact.Export();
                    m_rptPurchaseReportTransact.Close();
                    return;
                case PrintType.PDF: //PDF
                    PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                    CrExportOptions = m_rptPurchaseReportTransact.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                    m_rptPurchaseReportTransact.Export();
                    m_rptPurchaseReportTransact.Close();
                    return;
                case PrintType.Email:
                    ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                    CrExportOptions = m_rptPurchaseReportTransact.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                    m_rptPurchaseReportTransact.Export();
                    Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                    sendemail.ShowDialog();
                    m_rptPurchaseReportTransact.Close();
                    return;
                default: //Crystal Report
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
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
            Project.GetChildProjects(Convert.ToInt32(m_PurchaseParty.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_PurchaseParty.ProjectID + "'";

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
                    tw.WriteElementString("ProjectID", Convert.ToInt32(m_PurchaseParty.ProjectID).ToString());
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

       

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgPurchaseTransRep.Columns.Clear();
            this.Cursor = Cursors.WaitCursor;

            frmProductTransaction_Load( sender, e);
            dgPurchaseTransRep.Refresh();
            this.Cursor = Cursors.Default;
            this.WindowState = FormWindowState.Maximized;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
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

    }
}
