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
    public partial class frmStockLedger : Form
    {
        //private SourceGrid.Cells.Views.Cell GroupView;
        //private SourceGrid.Cells.Views.Cell subGroupView;
        private Inventory.Model.dsInventoryBookReport dsInventoryBookReport = new Model.dsInventoryBookReport();
        private InventoryBookSettings m_SL;
        private string FileName = "";
        ArrayList AccClassID = new ArrayList();

        //private int CountInboundQty=0;
        //private int CountOutboundQty=0;
        private int OpeningQty = 0;

        private string UnitSymbol = ""; //Stores the symbol of the Unit for the product E.g. pcs, kg etc.
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

        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        public frmStockLedger()
        {
            InitializeComponent();
        }

        IMDIMainForm m_MDIForm;
        public frmStockLedger(InventoryBookSettings SL,IMDIMainForm frmMAIN)
        {
            try
            {
                m_MDIForm = frmMAIN;
                m_SL = new InventoryBookSettings();
                InitializeComponent();
                m_SL.ProductGroupID = SL.ProductGroupID;
                m_SL.ProductID = SL.ProductID;
                m_SL.partyGroupID = SL.partyGroupID;
                m_SL.PartyID = SL.PartyID;
                m_SL.ProductName = SL.ProductName;
                m_SL.DepotID = SL.DepotID;
                m_SL.ProjectID = SL.ProjectID;
                m_SL.FromDate = SL.FromDate;
                m_SL.ToDate = SL.ToDate;
                m_SL.AccClassID = SL.AccClassID;
                m_SL.OpeningQty = SL.OpeningQty;
            }
            catch (Exception ex)
            {
                //ignore
            }
        }

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            //If it has a date range
            if(m_SL.FromDate !=null && m_SL.ToDate!=null)
            {
                lblAsonDate.Text = "From  " + Date.ToSystem((DateTime)m_SL.FromDate) + "  To  " + Date.ToSystem((DateTime)m_SL.ToDate);
            }
          else  if (m_SL.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_SL.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FYFrom));

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_SL.ProjectID), LangMgr.DefaultLanguage);
            if (m_SL.ProjectID != null)
            {                                                           
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = "Project: " + drProjectInfo["Name"].ToString();        
            }
            else
            {
                lblProjectName.Text = "Project: " + "All";
            }
            
            //Show Opening Quantity
            //try
            //{
            //    DisplayOpeningQty();
            //}
            //catch (Exception ex)
            //{
            //    Global.MsgError("Error in displaying Opening Balance. Message: " + ex.Message);
            //}

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



        private string ReadAllAccClassID()
        {
            #region  AccountingClassID
            
            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_SL.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }

            
            m_SL.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in m_SL.AccClassID)
                    {
                        AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccClassID", Convert.ToInt32(tag).ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }

            }
           // tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            return strXML;
        }

        #region oldcode

        //private void MakeHeader()
        //{
        //    //Defining the HeaderPart
        //    grdStockLedger[0, 0] = new MyHeader("Date");
        //    grdStockLedger[0, 1] = new MyHeader("VoucherType");
        //    grdStockLedger[0, 2] = new MyHeader("Voucher No");
        //    grdStockLedger[0, 3] = new MyHeader("Party");
        //    grdStockLedger[0, 4] = new MyHeader("Product Details");
        //    grdStockLedger[0, 5] = new MyHeader("InBound Qty");
        //    grdStockLedger[0, 6] = new MyHeader("OutBound Qty");
        //    grdStockLedger[0, 7] = new MyHeader("Unit");
        //    grdStockLedger[0, 8] = new MyHeader("Amount");
        //    grdStockLedger[0, 9] = new MyHeader("RowID");
        //    grdStockLedger[0, 10] = new MyHeader("VouType");

        //    //Define size of column
        //    grdStockLedger.Columns[0].Width = 80;
        //    grdStockLedger.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

        //    grdStockLedger.Columns[1].Width = 80;
        //    grdStockLedger.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

        //    grdStockLedger.Columns[2].Width = 100;
        //    grdStockLedger.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

        //    grdStockLedger.Columns[3].Width = 100;
        //    grdStockLedger.Columns[3].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

        //    grdStockLedger.Columns[4].Width = 100;
        //    grdStockLedger.Columns[4].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

        //    grdStockLedger.Columns[5].Width = 100;
        //    grdStockLedger.Columns[5].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

        //    grdStockLedger.Columns[6].Width = 100;
        //    grdStockLedger.Columns[6].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

        //    grdStockLedger.Columns[7].Width = 100;
        //    grdStockLedger.Columns[7].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

        //    grdStockLedger.Columns[8].Width = 80;
        //    grdStockLedger.Columns[8].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

        //    //Make 9 and 10th Column Invisible
        //    grdStockLedger[0, 9].Column.Visible = false;
        //    grdStockLedger[0, 10].Column.Visible = false;


        //    grdStockLedger.AutoStretchColumnsToFitWidth = true;
        //    grdStockLedger.AutoSizeCells();
        //    grdStockLedger.Columns.StretchToFit();

        //}

        //private void DisplayStockLedger(bool IsCrystalReport)
        //{
        //    //for orientation purpose
        //    string AccClassIDsXMLString = ReadAllAccClassID();
        //    string ProjectIDsXMLString = ReadAllProjectID();

        //        #region BLOCK FOR ORIENTATION PURPOSE
        //        if (!IsCrystalReport)//Orientaion Purpose is essential only for SOurcegrid not for Crystal report
        //        {
        //            //Text format for Total
        //            GroupView = new SourceGrid.Cells.Views.Cell();
        //            GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

        //            //Text format for Ledgers
        //            subGroupView = new SourceGrid.Cells.Views.Cell();
        //            subGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
        //            subGroupView.ForeColor = Color.Blue;
        //            DisplayBannar();
        //            //Let the whole row to be selected
        //            grdStockLedger.SelectionMode = SourceGrid.GridSelectionMode.Row;

        //            //Disable multiple selection
        //            grdStockLedger.Selection.EnableMultiSelection = false;
        //            //Disable multiple selection
        //            grdStockLedger.Redim(1, 11);
        //            int rows = grdStockLedger.Rows.Count;
        //            grdStockLedger.Rows.Insert(rows);
        //            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
        //            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
        //            dblClick.DoubleClick += new EventHandler(grdStockLedger_DoubleClick);
        //        }
        //        #endregion
        //        if (!IsCrystalReport)//DOnt show header for Crystal Report
        //            MakeHeader();
        //        DataTable dtStockLedgerInfo = InventoryBook.GetInventoryBook(m_SL.partyGroupID, m_SL.PartyID, m_SL.ProductGroupID, m_SL.ProductID, m_SL.DepotID, m_SL.ProjectID, m_SL.FromDate, m_SL.ToDate, InvenotryBookType.InventoyDayBook, AccClassIDsXMLString, ProjectIDsXMLString);//use InventoyBookType becuase its index is zero soo it looks for all VoucherType and its difference than InventoryBook becuase it is filtered by Product
        //        WriteStockLedger(dtStockLedgerInfo, IsCrystalReport);      
        //}

        //private void WriteStockLedger(DataTable dt, bool IsCrystalReport)
        //{
        //    try
        //    {
        //        string RowID, VoucherType;
        //        RowID = VoucherType = "";
        //        for (int i = 1; i <= dt.Rows.Count; i++)
        //        {
        //            SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
        //            if (i % 2 == 0)
        //            {
        //                //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
        //            }
        //            else
        //            {
        //                alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
        //            }
        //            DataRow dr = dt.Rows[i - 1];
        //            string PartyName, ProductName;
        //            DateTime VouDate = new DateTime();
        //            ProductName = PartyName = "";
        //            decimal m_NetSalesQty = 0;
        //            string myRowID = dr["RowID"].ToString();
        //            string myVoucherType = dr["VoucherType"].ToString();
        //            double Amount = Convert.ToDouble(dr["Amount"]);


        //            //Count the inbound and outbound quantity
        //            CountInboundQty += Convert.ToInt32(dr["InBound"]);
        //            CountOutboundQty += Convert.ToInt32(dr["OutBound"]);


        //            if (!IsCrystalReport) //Writing on Grid
        //            {
        //                grdStockLedger.Rows.Insert(i); //Insert the new Row to write
        //                if (RowID != dr["RowID"].ToString() || VoucherType != dr["VoucherType"].ToString())
        //                {
        //                    DataTable dtPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
        //                    DataRow drPartyInfo = dtPartyInfo.Rows[0];
        //                    PartyName = drPartyInfo["LedName"].ToString();
        //                    string str = dr["VoucherDate"].ToString();
        //                    VouDate =Convert.ToDateTime(dr["VoucherDate"]);
        //                    grdStockLedger[i, 0] = new SourceGrid.Cells.Cell(Date.ToSystem( VouDate));
        //                    grdStockLedger[i, 0].AddController(dblClick);
        //                    grdStockLedger[i, 0].View = GroupView;
        //                    grdStockLedger[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockLedger[i, 2] = new SourceGrid.Cells.Cell(dr["VoucherNo"].ToString());
        //                    grdStockLedger[i, 2].AddController(dblClick);
        //                    grdStockLedger[i, 2].View = GroupView;
        //                    grdStockLedger[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    //if Repeated Voucher No and Party name than wirte once only
        //                    VoucherType = dr["VoucherType"].ToString();
        //                    RowID = dr["RowID"].ToString();

        //                    //Reset count when VoucherType and RowID changes

        //                    grdStockLedger[i, 1] = new SourceGrid.Cells.Cell(VoucherType);
        //                    grdStockLedger[i, 1].AddController(dblClick);
        //                    grdStockLedger[i, 1].View = GroupView;
        //                    grdStockLedger[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockLedger[i, 3] = new SourceGrid.Cells.Cell(PartyName);
        //                    grdStockLedger[i, 3].AddController(dblClick);
        //                    grdStockLedger[i, 3].View = GroupView;
        //                    grdStockLedger[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
        //                    DataRow drProductInfo = dtProductInfo.Rows[0];
        //                    ProductName = drProductInfo["ProdName"].ToString();

        //                    grdStockLedger[i, 4] = new SourceGrid.Cells.Cell(ProductName);
        //                    grdStockLedger[i, 4].AddController(dblClick);
        //                    grdStockLedger[i, 4].View = GroupView;
        //                    grdStockLedger[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockLedger[i, 5] = new SourceGrid.Cells.Cell(dr["InBound"].ToString());
        //                    grdStockLedger[i, 5].AddController(dblClick);
        //                    grdStockLedger[i, 5].View = GroupView;
        //                    grdStockLedger[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockLedger[i, 6] = new SourceGrid.Cells.Cell(dr["OutBound"].ToString());
        //                    grdStockLedger[i, 6].AddController(dblClick);
        //                    grdStockLedger[i, 6].View = GroupView;
        //                    grdStockLedger[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockLedger[i, 7] = new SourceGrid.Cells.Cell("Unit");
        //                    grdStockLedger[i, 7].AddController(dblClick);
        //                    grdStockLedger[i, 7].View = GroupView;
        //                    grdStockLedger[i, 7].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockLedger[i, 8] = new SourceGrid.Cells.Cell((Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
        //                    grdStockLedger[i, 8].AddController(dblClick);
        //                    grdStockLedger[i, 8].View = GroupView;
        //                    grdStockLedger[i, 8].View = new SourceGrid.Cells.Views.Cell(alternate);                      
        //                }
        //                else
        //                {
        //                    grdStockLedger[i, 0] = new SourceGrid.Cells.Cell();
        //                    grdStockLedger[i, 0].AddController(dblClick);
        //                    grdStockLedger[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    grdStockLedger[i, 1] = new SourceGrid.Cells.Cell();
        //                    grdStockLedger[i, 1].AddController(dblClick);
        //                    grdStockLedger[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    grdStockLedger[i, 2] = new SourceGrid.Cells.Cell();
        //                    grdStockLedger[i, 2].AddController(dblClick);
        //                    grdStockLedger[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    grdStockLedger[i, 3] = new SourceGrid.Cells.Cell();
        //                    grdStockLedger[i, 3].AddController(dblClick);
        //                    grdStockLedger[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
        //                    DataRow drProductInfo = dtProductInfo.Rows[0];
        //                    ProductName = drProductInfo["ProdName"].ToString();

        //                    grdStockLedger[i, 4] = new SourceGrid.Cells.Cell(ProductName);
        //                    grdStockLedger[i, 4].AddController(dblClick);
        //                    grdStockLedger[i, 4].View = subGroupView;
        //                    grdStockLedger[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    grdStockLedger[i, 5] = new SourceGrid.Cells.Cell(dr["InBound"].ToString());
        //                    grdStockLedger[i, 5].AddController(dblClick);
        //                    grdStockLedger[i, 5].View = subGroupView;
        //                    grdStockLedger[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    grdStockLedger[i, 6] = new SourceGrid.Cells.Cell(dr["OutBound"].ToString());
        //                    grdStockLedger[i, 6].AddController(dblClick);
        //                    grdStockLedger[i, 6].View = subGroupView;
        //                    grdStockLedger[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    grdStockLedger[i, 7] = new SourceGrid.Cells.Cell("Unit");
        //                    grdStockLedger[i, 7].AddController(dblClick);
        //                    grdStockLedger[i, 7].View = subGroupView;
        //                    grdStockLedger[i, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    grdStockLedger[i, 8] = new SourceGrid.Cells.Cell((Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
        //                    grdStockLedger[i, 8].AddController(dblClick);
        //                    grdStockLedger[i, 8].View = subGroupView;
        //                    grdStockLedger[i, 8].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    grdStockLedger[i, 9] = new SourceGrid.Cells.Cell(RowID);
        //                    grdStockLedger[i, 9].AddController(dblClick);


        //                }

        //                grdStockLedger[i, 9] = new SourceGrid.Cells.Cell(RowID);
        //                grdStockLedger[i, 9].AddController(dblClick);

        //                grdStockLedger[i, 10] = new SourceGrid.Cells.Cell(VoucherType);
        //                grdStockLedger[i, 10].AddController(dblClick);
        //            }
        //            else //for writting on Crystal Report
        //            {

        //                VoucherType = dr["VoucherType"].ToString();
        //                DataTable dtPartytInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
        //                DataRow drPartyInfo = dtPartytInfo.Rows[0];
        //                PartyName = drPartyInfo["LedName"].ToString();
        //                DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
        //                DataRow drProductInfo = dtProductInfo.Rows[0];
        //                ProductName = drProductInfo["ProdName"].ToString();
        //                if (RowID != dr["RowID"].ToString() || VoucherType != dr["VoucherType"].ToString())
        //                {
        //                    VouDate = Convert.ToDateTime(dr["VoucherDate"]);
        //                    //string str1 =Date.ToSystem( Convert.ToDateTime(dr["VouDate"].ToString()));
        //                    dsInventoryBookReport.Tables["tblInventoryBook"].Rows.Add(Date.ToSystem( VouDate), VoucherType, dr["VoucherNo"].ToString(), PartyName, ProductName, dr["InBound"].ToString(), dr["OutBound"].ToString(), "Unit", (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
        //                    RowID = dr["RowID"].ToString();
        //                    VoucherType = dr["VoucherType"].ToString();
        //                }
        //                else
        //                {
        //                    dsInventoryBookReport.Tables["tblInventoryBook"].Rows.Add("", "", "", "", ProductName, dr["InBound"].ToString(), dr["OutBound"].ToString(), "Unit", (Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
        //                    //ClosingInbound = dr["InBound"].ToString();
        //                    //ClosingOutbound = dr["OutBound"].ToString();

        //                }
                  
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Global.MsgError(ex.Message);
        //    }
        //}

        //private void grdStockLedger_DoubleClick(object sender, EventArgs e)
        //{
        //    try
        //    {
                
        //        //Get the Selected Row           
        //        int CurRow = grdStockLedger.Selection.GetSelectionRegion().GetRowsIndex()[0];
        //        SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(grdStockLedger, new SourceGrid.Position(CurRow, 9));
        //        SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdStockLedger, new SourceGrid.Position(CurRow, 10));
        //        string Type = (cellType.Value).ToString();
        //        if ((cellType.Value.ToString()) != "")//Dont Call the voucher form if there is no Ledger...no need to call Voucher form for Op. Bal/Total Amount etc
        //        {
        //            int RowID = Convert.ToInt32(cellTypeID.Value);
        //            string VoucherType = cellType.Value.ToString();

        //            switch (VoucherType)
        //            {

        //                case "SALES":
        //                    frmSalesInvoice frm = new frmSalesInvoice(RowID);
        //                    frm.Show();
        //                    break;
        //                case "SALES_RTN":
        //                    frmSalesReturn frm1 = new frmSalesReturn(RowID);
        //                    frm1.Show();
        //                    break;
        //                case "PURCH":
        //                    frmPurchaseInvoice frm2 = new frmPurchaseInvoice(RowID);
        //                    frm2.Show();
        //                    break;
        //                case "PURCH_RTN":
        //                    frmPurchaseReturn frm3 = new frmPurchaseReturn(RowID);
        //                    frm3.Show();
        //                    break;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Msg(ex.Message);
        //    }
        //}

        #endregion

        DataTable dtTemp;
        private void CreateTempTableColumn()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("Date",typeof(string));
            dtTemp.Columns.Add("VoucherType",typeof(string));
            dtTemp.Columns.Add("VoucherNo",typeof(string));
            dtTemp.Columns.Add("ProductOrParty",typeof(string));
            dtTemp.Columns.Add("InBoundQty",typeof(string));
            dtTemp.Columns.Add("OutBoundQty",typeof(string));
            dtTemp.Columns.Add("Amount",typeof(string));
            dtTemp.Columns.Add("RowID", typeof(string));

            //dtTemp.Columns.Add("Date", typeof(string));

        }
        decimal openingStock = 0;
        decimal TotalInBound = 0;
        decimal TotalOutBound = 0;
        private void FillGridWithData()
        {
             openingStock = 0;
             TotalInBound = 0;
             TotalOutBound = 0;
             openingStock = 0;

            try
            {
                DataTable dt = Product.GetStockLedgerReport("<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>", m_SL.FromDate, m_SL.ToDate, m_SL.ProductID,ref openingStock);
                CreateTempTableColumn();
                
                foreach (DataRow dr in dt.Rows)
                {
                    dtTemp.Rows.Add(dr["NepTransactDate"].ToString(), dr["VoucherType"].ToString(), dr["VoucherNo"].ToString(), dr["Party"].ToString(), Convert.ToDecimal(dr["InBoundQty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["OutBoundQty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["RowID"].ToString());
                    TotalInBound += Convert.ToDecimal(dr["InBoundQty"]);
                    TotalOutBound += Convert.ToDecimal(dr["OutBoundQty"]);
                }

                lblOpeningQty.Text = "Opening Qty: " + openingStock.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblClosingQty.Text = "Closing Qty: " + (openingStock + TotalInBound - TotalOutBound).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                #region databinding
                DataView mView = new DataView(dtTemp);
                mView.AllowDelete = false;
                mView.AllowNew = false;
                dgStockLedger.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
                dgStockLedger.FixedRows = 1;
                DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

                CreateDataGridColumns(dgStockLedger.Columns, bd);
                dgStockLedger.DataSource = bd;
                dgStockLedger.Width = dgStockLedger.Width - 5;
                dgStockLedger.Width = this.Width - 25;
                #endregion
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);    
            }

        }

        private void CreateDataGridColumns(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
        {

            //text format for column header
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            viewColumnHeader.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);
            viewColumnHeader.BackColor = Color.LightGray;

            SourceGrid.Cells.Views.Cell rightAlign = new SourceGrid.Cells.Views.Cell();
            rightAlign.Font = new Font("Arial", 8, FontStyle.Regular);
            rightAlign.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            SourceGrid.DataGridColumn gridColumn;
   

            gridColumn = dgStockLedger.Columns.Add("Date", "Transact Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgStockLedger.Columns.Add("VoucherType", "Voucher Type", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            gridColumn.Width = 150;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgStockLedger.Columns.Add("VoucherNo", "Voucher Number", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            gridColumn = dgStockLedger.Columns.Add("ProductOrParty", "Cash Party", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            gridColumn = dgStockLedger.Columns.Add("InBoundQty", "InBound Quantity", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = rightAlign;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgStockLedger.Columns.Add("OutBoundQty", "OutBound Quantity", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = rightAlign;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgStockLedger.Columns.Add("Amount", "Amount", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = rightAlign;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgStockLedger.Columns.Add("RowID", "RowID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Visible = false;


            dgStockLedger.AutoStretchColumnsToFitWidth = true;


        }

        private void dgStockLedger_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                dgStockLedger_DoubleClick(sender, e);
        }
        public void dgStockLedger_DoubleClick(object sender, EventArgs e)
        {
            int curRow = dgStockLedger.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext paramRowID = new SourceGrid.CellContext(dgStockLedger, new SourceGrid.Position(curRow, 7));
            SourceGrid.CellContext paramVocuher=new SourceGrid.CellContext(dgStockLedger,new SourceGrid.Position(curRow,1));

            if(paramVocuher.Value==null && paramVocuher.Value==" " && paramVocuher.Value=="")
                return;

            
            int RowID = Convert.ToInt32(paramRowID.Value);
            string VoucherType = (paramVocuher.Value).ToString();
            switch(VoucherType)
            {
                        case "SALES":
                            object[] param = new object[1];
                            param[0] = (RowID);
                            m_MDIForm.OpenFormArrayParam("frmSalesInvoice", param);
                            break;

                        case "PURCH":
                            object[] param1 = new object[1];
                            param1[0] = (RowID);
                            m_MDIForm.OpenFormArrayParam("frmPurchaseInvoice", param1);
                            break;

                        case "SLS_RTN":
                            object[] param2 = new object[1];
                            param2[0] = (RowID);
                            m_MDIForm.OpenFormArrayParam("frmSalesReturn", param2);
                            break;

                        case "PURCH_RTN":
                            object[] param3 = new object[1];
                            param3[0] = (RowID);
                            m_MDIForm.OpenFormArrayParam("frmPurchaseReturn", param3);
                            break;  
                default:
                    return;
            }

        }
        private void frmStockLedger_Load(object sender, EventArgs e)
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
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(dgStockLedger_DoubleClick);
            dblClick.KeyUp += new KeyEventHandler(dgStockLedger_KeyUp);


            ProgressForm.UpdateProgress(20, "Initializing report...");

            grdStockLedger.Visible = false;
            dgStockLedger.Visible = true;
            DisplayBannar();
            lblProductName.Text = "Product Name : " + m_SL.ProductName?? " ";
            ProgressForm.UpdateProgress(40, "Displaying  report head...");

            FillGridWithData();
            ProgressForm.UpdateProgress(80, "Displaying  report ...");
            dgStockLedger.EnableSort = false;
            dgStockLedger.SelectionMode = SourceGrid.GridSelectionMode.Row;
         //   DisplayStockStatus(false);
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            //Get Unit Symbol
           // try
           // {
               // DataTable dtProduct = Product.GetProductByID(Convert.ToInt32(m_SL.ProductID));
               // DataTable UnitName = UnitMaintenance.GetUnitMaintenaceInfo(Convert.ToInt32(dtProduct.Rows[0]["UnitMaintenanceID"]));
              //  UnitSymbol = UnitName.Rows[0]["Symbol"].ToString();
          //  }
          //  catch
          //  {
                //Whatever be the problem simply ignore
                //UnitSymbol = "";
          //  }

            //Get the details and Write on the grid
            //DisplayStockLedger(false);

            //lblClosingQty.Text = "Closing Qty: " + (OpeningQty + CountInboundQty - CountOutboundQty).ToString() + " " + UnitSymbol;
        }

        /// <summary>
        /// Display Opening Quantity
        /// </summary>
        private void DisplayOpeningQty()

        {

            #region Display Opening Quantity on the top
            //Get root accounting class
            int RootAccClassID;
            try
            {
                DataTable dtRootAccClass = AccountClass.GetRootAccClass(Convert.ToInt32(m_SL.AccClassID[0]));
                RootAccClassID = Convert.ToInt32(dtRootAccClass.Rows[0]["AccClassID"]);
            }
            catch
            {
                throw new Exception("Invalid Accounting Class selected");
            }
            //Write Opening Quantity
            try
            {
               // OpeningQty = InventoryBook.GetOpeningQty(RootAccClassID, Convert.ToInt32(m_SL.ProductID));

                if(m_SL.ProductID!=null)
                    OpeningQty =  Product.GetOpeningQty(Convert.ToInt32(m_SL.AccClassID[0]), ProductID: Convert.ToInt32(m_SL.ProductID));
                else if (m_SL.ProductGroupID!=null)
                    OpeningQty = Product.GetOpeningQty(Convert.ToInt32(m_SL.AccClassID[0]), GroupID: Convert.ToInt32(m_SL.ProductGroupID));
                else //When both are not given that means All products is selected
                    OpeningQty = Product.GetOpeningQty(Convert.ToInt32(m_SL.AccClassID[0]));

            }
            catch (Exception ex)
            {
                Global.MsgError("Error getting Opening Quantity " + ex.Message);
                OpeningQty = 0;
            }

            //Write on the label
            lblOpeningQty.Text = "Opening Qty: " + OpeningQty.ToString() + " " + UnitSymbol;
            #endregion

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
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;

            //rptBalanceSheet rpt = new rptBalanceSheet();
            rptInventoryBookReport m_rptInventoryBook = new rptInventoryBookReport();

            //Fill the logo on the report
            Misc.WriteLogo(dsInventoryBookReport, "tblImage");

            //Fill the logo on the report
            //Misc.WriteLogo(dsPurchaseReport, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            //rpt.SetDataSource(dsPurchaseReport);
            // if (m_PurchaseReport.IsProductwise)

            try
            {
                dsInventoryBookReport.Tables.Remove("tblInventoryBook");
            }
            catch (Exception ex)
            {

            }
            DataView dview = new DataView(dtTemp);
            DataTable selected = dview.ToTable("tblInventoryBook", false, "Date", "VoucherType", "VoucherNo", "ProductOrParty", "InBoundQty", "OutBoundQty", "Amount");
            dsInventoryBookReport.Tables.Add(selected);
            m_rptInventoryBook.SetDataSource(dsInventoryBookReport);
            // else if (m_PurchaseReport.IsPartywise)
            //     m_rptPurchaseReportByParty.SetDataSource(dsInventoryBook);

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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_ProductName = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_OpeningQuantity = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_ClosingQuantity = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_ProductPartyName = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_PrintDate = new CrystalDecisions.Shared.ParameterDiscreteValue();

            
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            // if (m_PurchaseReport.IsProductwise)
            m_rptInventoryBook.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);
            // else if (m_PurchaseReport.IsPartywise)
            //     m_rptPurchaseReportByParty.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);


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
            pdvReport_Head.Value = "Stock Ledger Report";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            m_rptInventoryBook.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            pdvReport_PrintDate.Value = Date.ToSystem(DateTime.Today); ;
            pvCollection.Clear();
            pvCollection.Add(pdvReport_PrintDate);
            m_rptInventoryBook.DataDefinition.ParameterFields["PrintDate"].ApplyCurrentValues(pvCollection); 

            pdvReport_ProductName.Value = m_SL.ProductName;
            pvCollection.Clear();
            pvCollection.Add(pdvReport_ProductName);
            m_rptInventoryBook.DataDefinition.ParameterFields["ProductName"].ApplyCurrentValues(pvCollection);

            pdvReport_OpeningQuantity.Value ="Opening Quantity : "+ openingStock.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvReport_OpeningQuantity);
            m_rptInventoryBook.DataDefinition.ParameterFields["OpeningQuantity"].ApplyCurrentValues(pvCollection);

            pdvReport_ProductPartyName.Value = "Cash Party";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_ProductPartyName);
            m_rptInventoryBook.DataDefinition.ParameterFields["PartyProductName"].ApplyCurrentValues(pvCollection);

            pdvReport_ClosingQuantity.Value ="Closing Quantity : "+ (openingStock + TotalInBound - TotalOutBound).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvReport_ClosingQuantity);
            m_rptInventoryBook.DataDefinition.ParameterFields["ClosingQuantity"].ApplyCurrentValues(pvCollection);

            pdvFiscal_Year.Value = "Fiscal Year:" + (m_CompanyDetails.FiscalYear);
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);

            m_rptInventoryBook.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);


            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_SL.FromDate != null && m_SL.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SL.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_SL.ToDate));
            }
            if (m_SL.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_SL.ToDate));
            }
            if (m_SL.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_SL.FromDate)) + " To " + Date.ToSystem(DateTime.Today);
            }
            if (m_SL.FromDate == null && m_SL.ToDate == null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(DateTime.Today);

            }
           // pdvReport_Date.Value = Date.ToSystem(Date.GetServerDate());
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            m_rptInventoryBook.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);
           
            //DisplayStockLedger(true);

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
            grdStockLedger.Redim(0, 0);
            dgStockLedger.Columns.Clear();
            //CountInboundQty = 0;
            //CountOutboundQty = 0;
            OpeningQty = 0;
            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmStockLedger_Load(sender, e);
            //grdStockLedger.Refresh();

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


        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_SL.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_SL.ProjectID + "'";

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
                    tw.WriteElementString("ProjectID", Convert.ToInt32(m_SL.ProjectID).ToString());
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
           // tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            return strXML;
        }

        private void lblProjectName_Click(object sender, EventArgs e)
        {

        }

      
    }
}
