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
    public partial class frmStockStatus : Form
    {
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell HeaderView;
        private StockStatusSettings m_StockStatus;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private Inventory.Model.dsStockStatusReport dsStockStatusReport = new Model.dsStockStatusReport();
        private string FileName = "";
        ArrayList AccClassID = new ArrayList();
        private Inventory.Model.dsStock dsStock = new Model.dsStock();
        private decimal _TotalQty ;

        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport
        }


        //For Export Menu
        ContextMenu Menu_Export;

        IMDIMainForm M_MDIMain;
        public frmStockStatus(StockStatusSettings StockStatus,IMDIMainForm frmMDI)
        {
            // Try catch block for error handling
            try
            {
                M_MDIMain = frmMDI;

                #region BLOCK FOR INITIALIZING THE CONSTRUCTOR PARAMETER
                // Initilizing the constructor's parameter

                InitializeComponent();
                m_StockStatus = new StockStatusSettings();
                m_StockStatus.AtTheEndDate =StockStatus.AtTheEndDate;
                
                m_StockStatus.ShowZeroQunatity = StockStatus.ShowZeroQunatity;
                m_StockStatus.ProductGroupID = StockStatus.ProductGroupID;
                m_StockStatus.ProductID = StockStatus.ProductID;
                m_StockStatus.Depot = StockStatus.Depot;
                m_StockStatus.AccClassID = StockStatus.AccClassID;
                m_StockStatus.OpeningStock = StockStatus.OpeningStock;
                m_StockStatus.ClosingStock = StockStatus.ClosingStock;
                m_StockStatus.ProjectID = StockStatus.ProjectID;
            #endregion


            }
            catch (Exception e)
            { 
            
            }
        }

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            //lblCompanyName.Text = m_CompanyDetails.CompanyName;
            //lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            //lblContact.Text = "Contact: " + m_CompanyDetails.Telephone + "    Website: " + m_CompanyDetails.Website;
            //lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;

            lblAsonDate.Text = "As Till Date: " + Date.ToSystem((DateTime)m_StockStatus.AtTheEndDate);

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear; //+ Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FiscalYear));
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
            foreach (object j in m_StockStatus.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);
            }
            m_StockStatus.AccClassID.AddRange(arrChildAccClassIDs);

            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();

            #region  Accountclass
            tw.WriteStartElement("STOCKSTATUS");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("AccClassIDSettings");
                    foreach (string tag in m_StockStatus.AccClassID)
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

        #region oldcode

        //Writes the header on the grid
        //private void MakeHeader()
        //{

        //    //Write header part
        //    grdStockStatus.Rows.Insert(0);
        //    grdStockStatus[0, 0] = new SourceGrid.Cells.ColumnHeader("Date");
        //    grdStockStatus[0, 1] = new SourceGrid.Cells.ColumnHeader("Item");
        //    grdStockStatus[0, 2] = new SourceGrid.Cells.ColumnHeader("Depot");
        //    grdStockStatus[0, 3] = new SourceGrid.Cells.ColumnHeader("Quantity");
        //    grdStockStatus[0, 4] = new SourceGrid.Cells.ColumnHeader("Unit");
        //    grdStockStatus[0, 5] = new SourceGrid.Cells.ColumnHeader("ProductID");

        //    HeaderView = new SourceGrid.Cells.Views.Cell();
        //    HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
        //    //Define the width of column size
        //    grdStockStatus[0, 0].Column.Width = 150;
        //    grdStockStatus[0, 0].View = HeaderView;
        //    grdStockStatus[0, 1].Column.Width = 400;
        //    grdStockStatus[0, 1].View = HeaderView;
        //    grdStockStatus[0, 2].Column.Width = 150;
        //    grdStockStatus[0, 2].View = HeaderView;
        //    grdStockStatus[0, 3].Column.Width = 100;
        //    grdStockStatus[0, 3].View = HeaderView;
        //    grdStockStatus[0, 4].Column.Width = 100;
        //    grdStockStatus[0, 4].View = HeaderView;
        //    grdStockStatus[0, 5].Column.Visible = false;
        //}


        //private void DisplayStockStatus(bool IsCrystalReport)
        //{
        //    frmProgress ProgressForm = new frmProgress();
        //    // Initialize the thread that will handle the background process
        //    Thread backgroundThread = new Thread(
        //        new ThreadStart(() =>
        //        {

        //            ProgressForm.ShowDialog();
        //        }
        //    ));

        //    backgroundThread.Start();

        //    ProgressForm.UpdateProgress(20, "Initializing report...");

        //    //for orientation purpose
        //    string AccClassIDsXMLString = ReadAllAccClassID();
        //    string ProjectIDsXMLString = ReadAllProjectID();

        //    #region BLOCK FOR ORIENTATION PURPOSE
        //    if (!IsCrystalReport)//Orientaion Purpose is essential only for SOurcegrid not for Crystal report
        //    {
        //        //Text format 
        //        GroupView = new SourceGrid.Cells.Views.Cell();
        //        GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

        //        ProgressForm.UpdateProgress(40, "Displaying  report head...");
        //        DisplayBannar();

        //        //Let the whole row to be selected
        //        grdStockStatus.SelectionMode = SourceGrid.GridSelectionMode.Row;

        //        //Disable multiple selection
        //        grdStockStatus.Selection.EnableMultiSelection = false;
        //        //Disable multiple selection
        //        grdStockStatus.Redim(1, 11);
        //        int rows = grdStockStatus.Rows.Count;
        //        grdStockStatus.Rows.Insert(rows);
        //        //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
        //        dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
        //       // dblClick.DoubleClick += new EventHandler(grdStockStatusr_DoubleClick);
        //    }
        //    #endregion

        //    if (m_StockStatus.OpeningStock)
        //    {
        //        DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(m_StockStatus.ProductGroupID, m_StockStatus.ProductID, m_StockStatus.Depot, m_StockStatus.AtTheEndDate, m_StockStatus.ShowZeroQunatity, StockStatusType.OpeningStock, AccClassIDsXMLString);//use InventoyBookType becuase its index is zero soo it looks for all VoucherType and its difference than InventoryBook becuase it is filtered by Product
        //        WriteStockLedger(dtOpeningStockStatusInfo, IsCrystalReport);
        //    }

        //    else if(m_StockStatus.ClosingStock)
        //    {
        //        DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(m_StockStatus.ProductGroupID, m_StockStatus.ProductID, m_StockStatus.Depot, m_StockStatus.AtTheEndDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);//use InventoyBookType becuase its index is zero soo it looks for all VoucherType and its difference than InventoryBook becuase it is filtered by Product
              
        //        DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(m_StockStatus.ProductGroupID, m_StockStatus.ProductID, m_StockStatus.Depot, m_StockStatus.AtTheEndDate, false, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);//use InventoyBookType becuase its index is zero soo it looks for all VoucherType and its difference than InventoryBook becuase it is filtered by Product
                
        //        DataTable dtClosingStockStatusInfo = new DataTable();
        //        dtClosingStockStatusInfo.Columns.Add("CreatedDate");
        //        dtClosingStockStatusInfo.Columns.Add("ProductID");
        //        dtClosingStockStatusInfo.Columns.Add("ProductName");
        //        dtClosingStockStatusInfo.Columns.Add("Quantity");
        //        dtClosingStockStatusInfo.Columns.Add("Unit");
        //        dtClosingStockStatusInfo.Columns.Add("Depot");
        //       // DataTable dtOpeningStockStatusInfo = Sales.GetProductQtyInStockForRptSingleProduct(accid, m_StockStatus.ProductID, m_StockStatus.AtTheEndDate);
        //        if (dtTransactionStockStatusInfo.Rows.Count != 0)
        //        {
        //            foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
        //            {
        //               // if (dtTransactionStockStatusInfo.Rows.Count != 0)
        //                //{
        //                    int closingQuantity = 0;
        //                    bool isZero = true;
        //                    foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
        //                    {
        //                        if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
        //                        {
        //                            isZero = false;
        //                             closingQuantity = Convert.ToInt32(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
        //                             dtClosingStockStatusInfo.Rows.Add(drOpeningStockStatusInfo["CreatedDate"].ToString(), drOpeningStockStatusInfo["ProductID"].ToString(), drOpeningStockStatusInfo["ProductName"].ToString(), closingQuantity.ToString(), drOpeningStockStatusInfo["Unit"].ToString(), drOpeningStockStatusInfo["Depot"].ToString());
        //                            break;
        //                        }
        //                        //else
        //                        //{
        //                        //    dtClosingStockStatusInfo.Rows.Add(drOpeningStockStatusInfo["CreatedDate"].ToString(), drOpeningStockStatusInfo["ProductID"].ToString(), drOpeningStockStatusInfo["ProductName"].ToString(), drOpeningStockStatusInfo["Quantity"].ToString(), drOpeningStockStatusInfo["Unit"].ToString(), drOpeningStockStatusInfo["Depot"].ToString());
                                
        //                        //    break;
        //                        //}
        //                    }
        //                    if (isZero == true)
        //                    {
        //                        dtClosingStockStatusInfo.Rows.Add(drOpeningStockStatusInfo["CreatedDate"].ToString(), drOpeningStockStatusInfo["ProductID"].ToString(), drOpeningStockStatusInfo["ProductName"].ToString(), drOpeningStockStatusInfo["Quantity"].ToString(), drOpeningStockStatusInfo["Unit"].ToString(), drOpeningStockStatusInfo["Depot"].ToString());
        //                    }
        //                //}
                        
        //                //else

        //                //    dtClosingStockStatusInfo.Rows.Add(drOpeningStockStatusInfo["CreatedDate"].ToString(), drOpeningStockStatusInfo["ProductID"].ToString(), drOpeningStockStatusInfo["ProductName"].ToString(), drOpeningStockStatusInfo["Quantity"].ToString(), drOpeningStockStatusInfo["Unit"].ToString(), drOpeningStockStatusInfo["Depot"].ToString());
        //            }
        //            WriteStockLedger(dtClosingStockStatusInfo, IsCrystalReport);
        //        }
        //        else
        //        {
        //            DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(m_StockStatus.ProductGroupID, m_StockStatus.ProductID, m_StockStatus.Depot, m_StockStatus.AtTheEndDate, m_StockStatus.ShowZeroQunatity, StockStatusType.OpeningStock, AccClassIDsXMLString);//use InventoyBookType becuase its index is zero soo it looks for all VoucherType and its difference than InventoryBook becuase it is filtered by Product
        //            WriteStockLedger(dtOpeningStockStatusInfo1, IsCrystalReport);
        //        }
        //    }
        //    lblTotalQty.Text = _TotalQty.ToString();
        //}

        //private void WriteStockLedger(DataTable dt, bool IsCrystalReport)
        //{

        //    frmProgress ProgressForm = new frmProgress();
        //    // Initialize the thread that will handle the background process
        //    Thread backgroundThread = new Thread(
        //        new ThreadStart(() =>
        //        {

        //            ProgressForm.ShowDialog();
        //        }
        //    ));

        //    backgroundThread.Start();

        //    ProgressForm.UpdateProgress(60, "Getting the Records ...");

        //    try
        //    {
        //        grdStockStatus.Rows.Clear();
        //        grdStockStatus.Redim(dt.Select().Count() + 1, 6);
        //        MakeHeader();

        //        _TotalQty = 0; //Reset the qty counter

        //        if (dt.Rows.Count != 0)
        //        {
        //            for (int i = 1; i <= dt.Rows.Count; i++)
        //            {
        //                DataRow dr = dt.Rows[i - 1];
        //                string ProductName = "";
        //                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
        //                if (i % 2 == 0)
        //                {
        //                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
        //                }
        //                else
        //                {
        //                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
        //                }
                      
        //                if (!IsCrystalReport)
        //                {
        //                    grdStockStatus.Rows.Insert(i);

        //                    DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
        //                    DataRow drProductInfo = dtProductInfo.Rows[0];
        //                    ProductName = drProductInfo["ProdName"].ToString();

        //                    //Write Created Date
        //                    grdStockStatus[i, 0] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime(dr["CreatedDate"].ToString())));
        //                    grdStockStatus[i, 0].AddController(dblClick);
        //                    grdStockStatus[i, 0].View = GroupView;
        //                    grdStockStatus[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    //Product Name column
        //                    grdStockStatus[i, 1] = new SourceGrid.Cells.Cell(ProductName);
        //                    grdStockStatus[i, 1].AddController(dblClick);
        //                    grdStockStatus[i, 1].View = GroupView;
        //                    grdStockStatus[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockStatus[i, 2] = new SourceGrid.Cells.Cell(dr["Depot"].ToString());
        //                    grdStockStatus[i, 2].AddController(dblClick);
        //                    grdStockStatus[i, 2].View = GroupView;
        //                    grdStockStatus[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockStatus[i, 3] = new SourceGrid.Cells.Cell(dr["Quantity"].ToString());
        //                    grdStockStatus[i, 3].AddController(dblClick);
        //                    grdStockStatus[i, 3].View = GroupView;
        //                    grdStockStatus[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
        //                    _TotalQty+=Convert.ToDecimal(dr["Quantity"]);

        //                    grdStockStatus[i, 4] = new SourceGrid.Cells.Cell(dr["Unit"].ToString());
        //                    grdStockStatus[i, 4].AddController(dblClick);
        //                    grdStockStatus[i, 4].View = GroupView;
        //                    grdStockStatus[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    //Hidden product ID column
        //                    grdStockStatus[i, 5] = new SourceGrid.Cells.Cell(dr["ProductID"].ToString());
        //                    grdStockStatus[i, 5].AddController(dblClick);
        //                    grdStockStatus[i, 5].View = GroupView;
        //                    grdStockStatus[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    ProgressForm.UpdateProgress(100, "Preparing Report for display ...");
        //                    if (ProgressForm.InvokeRequired)
        //                        ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
        //                }
        //                else //for writting on Crystal Report
        //                {
        //                    grdStockStatus.Rows.Insert(i);

        //                    DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
        //                    DataRow drProductInfo = dtProductInfo.Rows[0];
        //                    ProductName = drProductInfo["ProdName"].ToString();

        //                    grdStockStatus[i, 0] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime(dr["CreatedDate"].ToString())));
        //                    grdStockStatus[i, 0].AddController(dblClick);
        //                    grdStockStatus[i, 0].View = GroupView;
        //                    grdStockStatus[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockStatus[i, 1] = new SourceGrid.Cells.Cell(ProductName);
        //                    grdStockStatus[i, 1].AddController(dblClick);
        //                    grdStockStatus[i, 1].View = GroupView;
        //                    grdStockStatus[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockStatus[i, 2] = new SourceGrid.Cells.Cell(dr["Depot"].ToString());
        //                    grdStockStatus[i, 2].AddController(dblClick);
        //                    grdStockStatus[i, 2].View = GroupView;
        //                    grdStockStatus[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockStatus[i, 3] = new SourceGrid.Cells.Cell(dr["Quantity"].ToString());
        //                    grdStockStatus[i, 3].AddController(dblClick);
        //                    grdStockStatus[i, 3].View = GroupView;
        //                    grdStockStatus[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    grdStockStatus[i, 4] = new SourceGrid.Cells.Cell(dr["Unit"].ToString());
        //                    grdStockStatus[i, 4].AddController(dblClick);
        //                    grdStockStatus[i, 4].View = GroupView;
        //                    grdStockStatus[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                    //DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
        //                    //DataRow drProductInfo = dtProductInfo.Rows[0];
        //                    //ProductName = drProductInfo["ProdName"].ToString();

        //                    dsStockStatusReport.Tables["tblStockStatus"].Rows.Add(Date.ToSystem(Convert.ToDateTime(dr["CreatedDate"].ToString())), ProductName, dr["Depot"].ToString(), Convert.ToInt32(dr["Quantity"]), dr["Unit"].ToString());
        //                    ProgressForm.UpdateProgress(100, "Preparing Report for display ...");

        //                    if (ProgressForm.InvokeRequired)
        //                        ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
        //                }
        //            }
        //        }
        //        else 
        //        {
        //            if (ProgressForm.InvokeRequired)
        //                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

        //            MessageBox.Show("Sorry! No records were found.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

          
        //private void WriteStockStatusOnCrystalReport(string Product, string GroupID, string Group, string Depot, string Quantity, string Unit, string Amount)
        //{
        //    dsStock.Tables["tblStock"].Rows.Add(Product, Group, Depot, Quantity, Unit, Amount);
        //}


        //Function for Double click event
        //private void grdStockStatus_DoubleClick(object sender, EventArgs e)
        //{

        //    try
        //    {

        //        //Get the Selected Row           
        //        int CurRow = grdStockStatus.Selection.GetSelectionRegion().GetRowsIndex()[0];
        //        SourceGrid.CellContext cellProductID = new SourceGrid.CellContext(grdStockStatus, new SourceGrid.Position(CurRow, 5));

        //        string ProductID = (cellProductID.Value).ToString();
        //        if (ProductID.Trim().Length>0)//Dont Call the voucher form if there is no Ledger...no need to call Voucher form for Op. Bal/Total Amount etc
        //        {
        //            //Write code to display Stock Ledger
        //            InventoryBookSettings m_SLS = new InventoryBookSettings();//for stock ledger settings

        //            //for single Product
        //            m_SLS.ProductID = Convert.ToInt32(ProductID);

        //            //for date range
        //            m_SLS.FromDate = null;
        //            m_SLS.ToDate = null;


        //            //for Depot
        //            m_SLS.DepotID = null;


        //            //for Project 
        //            m_SLS.ProjectID = m_StockStatus.ProjectID;

        //            m_SLS.AccClassID = m_StockStatus.AccClassID;
        //            frmStockLedger frm = new frmStockLedger(m_SLS, M_MDIMain);
        //            frm.Show();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Msg(ex.Message);
        //    }


        //}
        #endregion

        DataTable dtTemp;
        DataTable dt=new DataTable();
        private void CreateTempTableColumns()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("SN",typeof(int));
            dtTemp.Columns.Add("ProductID", typeof(int));
            dtTemp.Columns.Add("ProductName", typeof(string));
            dtTemp.Columns.Add("OpeningQty", typeof(string));
            dtTemp.Columns.Add("InBound", typeof(string));
            dtTemp.Columns.Add("OutBound", typeof(string));
            dtTemp.Columns.Add("ClosingQty", typeof(string));

        }
        private void FillGridWithData()
        {
            try
            {
                decimal TotalClosingQuantity = 0;
                grdStockStatus.Visible = false;
                dgStockStatus.Visible = true;
                decimal OpeningStock = 0;
                decimal ClosingStock = 0;
                dt = Product.GetAllProductOpnNCloStock("<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", 
                    "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>",
                    null, m_StockStatus.AtTheEndDate,m_StockStatus.ProductID,m_StockStatus.ProductGroupID, 
                    ref OpeningStock, ref ClosingStock);
                CreateTempTableColumns();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToDecimal(dr["ClosingQty"]) == 0)
                        {
                            if (m_StockStatus.ShowZeroQunatity)
                                dtTemp.Rows.Add(Convert.ToInt32(dr["SN"]), Convert.ToInt32(dr["ProductID"]), dr["ProductName"].ToString(), 
                                    Convert.ToDecimal(dr["OpeningQty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 
                                    Convert.ToDecimal(dr["InBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 
                                    Convert.ToDecimal(dr["OutBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),
                                    Convert.ToDecimal(dr["ClosingQty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));

                        }
                        else
                        {
                            dtTemp.Rows.Add(Convert.ToInt32(dr["SN"]), Convert.ToInt32(dr["ProductID"]), dr["ProductName"].ToString(), 
                                Convert.ToDecimal(dr["OpeningQty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 
                                Convert.ToDecimal(dr["InBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), 
                                Convert.ToDecimal(dr["OutBound"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),
                                Convert.ToDecimal(dr["ClosingQty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            TotalClosingQuantity += Convert.ToDecimal(dr["ClosingQty"]);
                        }
                    }
                }
                #region databinding
                DataView mView = new DataView(dtTemp);
                mView.AllowDelete = false;
                mView.AllowNew = false;
                dgStockStatus.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
                dgStockStatus.FixedRows = 1;
                DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

                CreateDataGridColumns(dgStockStatus.Columns, bd);
                dgStockStatus.DataSource = bd;

                lblTotalQty.Text = TotalClosingQuantity.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                dgStockStatus.Width = dgStockStatus.Width - 5;
                dgStockStatus.Width = this.Width - 25;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
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
            gridColumn = dgStockStatus.Columns.Add("SN", "S.N.", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            gridColumn.Width = 50;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgStockStatus.Columns.Add("ProductID", "ProductID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Visible = false;

            gridColumn = dgStockStatus.Columns.Add("ProductName", "Product Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            gridColumn = dgStockStatus.Columns.Add("OpeningQty", "Opening Quantity", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = rightAlign;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgStockStatus.Columns.Add("InBound", "InBound", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = rightAlign;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgStockStatus.Columns.Add("OutBound", "OutBound", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = rightAlign;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgStockStatus.Columns.Add("ClosingQty", "Closing Quantity", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = rightAlign;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            dgStockStatus.AutoStretchColumnsToFitWidth = true;


        }
        private void dgStockStatusr_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                dgStockStatusr_DoubleClick(sender, e);
        }

        public void dgStockStatusr_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int curRow = dgStockStatus.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext productID = new SourceGrid.CellContext(dgStockStatus, new SourceGrid.Position(curRow, 1));
                SourceGrid.CellContext productName = new SourceGrid.CellContext(dgStockStatus, new SourceGrid.Position(curRow, 2));
               // SourceGrid.CellContext OpeningQty = new SourceGrid.CellContext(dgStockStatus, new SourceGrid.Position(curRow, 3));

                InventoryBookSettings M_IBT = new InventoryBookSettings();
                M_IBT.ProductID = Convert.ToInt32(productID.Value);
                M_IBT.ProductName = productName.Value.ToString();
                M_IBT.OpeningQty = 0;//no need to pass for stock ledger


                M_IBT.ProductGroupID = m_StockStatus.ProductGroupID;
                M_IBT.ProjectID = m_StockStatus.ProjectID;
                M_IBT.FromDate = null;
                 
                M_IBT.ToDate = m_StockStatus.AtTheEndDate;
                M_IBT.AccClassID = m_StockStatus.AccClassID;
                frmStockLedger frmSL = new frmStockLedger(M_IBT, M_MDIMain);
                frmSL.ShowDialog();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);    
            }


        }
        private void frmStockStatus_Load(object sender, EventArgs e)
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

            ProgressForm.UpdateProgress(20, "Initializing report...");
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
            ProgressForm.UpdateProgress(40, "Displaying  report head...");
            DisplayBannar();
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(dgStockStatusr_DoubleClick);
            dblClick.KeyUp += new KeyEventHandler(dgStockStatusr_KeyUp);

            FillGridWithData();
            //Let the whole row to be selected
            dgStockStatus.SelectionMode = SourceGrid.GridSelectionMode.Row;

            //Disable multiple selection
            dgStockStatus.Selection.EnableMultiSelection = false;
            dgStockStatus.EnableSort = false;

         // DisplayStockStatus(false)
           if (ProgressForm.InvokeRequired)
               ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
    
        }
        private void frmStockStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

        private void btnPrintPreview_Click(object sender, EventArgs e) 
        {
            PrintPreviewCR(PrintType.CrystalReport);
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

                dsStockStatusReport.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed


                //rptBalanceSheet rpt = new rptBalanceSheet();
                rptStockStatusReport m_rptStockStatus = new rptStockStatusReport();


                //Fill the logo on the report
                Misc.WriteLogo(dsStockStatusReport, "tblImage");

                //Set DataSource to be dsStockstatusreport dataset on the report
                m_rptStockStatus.SetDataSource(dsStockStatusReport);

                try
                {
                    dsStockStatusReport.Tables.Remove("tblStockStatus");
                }
                catch (Exception ex)
                {

                }
                DataView dview = new DataView(dtTemp);
                DataTable selected = dview.ToTable("tblStockStatus", false, "SN", "ProductID", "ProductName", "OpeningQty", "InBound", "OutBound", "ClosingQty");
                dsStockStatusReport.Tables.Add(selected);


                //Provide values to the parameters on the report
                CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCity = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvDistrict= new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvFiscal_Year = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTill_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvTotalQuantity = new CrystalDecisions.Shared.ParameterDiscreteValue();


                CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

                //Update the progressbar
                ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

                pdvFont.Value = "Arial";
                pvCollection.Clear();
                pvCollection.Add(pdvFont);
                m_rptStockStatus.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);
               
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
                m_rptStockStatus.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);


                pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                m_rptStockStatus.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);
                //pdvCompany_Address.Value = m_CompanyDetails.Address1;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_Address);


                pdvCity.Value = m_CompanyDetails.City;
                pvCollection.Clear();
                pvCollection.Add(pdvCity);
                m_rptStockStatus.DataDefinition.ParameterFields["City"].ApplyCurrentValues(pvCollection);


                pdvDistrict.Value = m_CompanyDetails.District;
                pvCollection.Clear();
                pvCollection.Add(pdvDistrict);
                m_rptStockStatus.DataDefinition.ParameterFields["District"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = m_CompanyDetails.PAN;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                m_rptStockStatus.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);


                pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);


                m_rptStockStatus.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);


                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                m_rptStockStatus.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
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
                m_rptStockStatus.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Address);
                m_rptStockStatus.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                pdvCompany_PAN.Value = companypan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_PAN);
                m_rptStockStatus.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Phone);
                m_rptStockStatus.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                m_rptStockStatus.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            }
                String ReportType = "Stock Status";
                //if (m_StockStatus.OpeningStock)
                //    ReportType = "Opening Stock Report";
                //else
                //    ReportType = "Closing Stock Report";

                pdvReport_Head.Value = ReportType;
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                m_rptStockStatus.DataDefinition.ParameterFields["Report_Type"].ApplyCurrentValues(pvCollection);

                pdvFiscal_Year.Value = (m_CompanyDetails.FiscalYear);
                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);

                m_rptStockStatus.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);


                pdvTotalQuantity.Value = lblTotalQty.Text.Trim();
                pvCollection.Clear();
                pvCollection.Add(pdvTotalQuantity);
                m_rptStockStatus.DataDefinition.ParameterFields["TotalQuantity"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

               // pdvTill_Date.Value = Date.ToSystem((DateTime)m_StockStatus.AtTheEndDate);
                pdvTill_Date.Value = Date.ToSystem(Convert.ToDateTime( m_StockStatus.AtTheEndDate));
                pvCollection.Clear();
                pvCollection.Add(pdvTill_Date);

                m_rptStockStatus.DataDefinition.ParameterFields["Till_Date"].ApplyCurrentValues(pvCollection);

                //DisplayStockStatus(true);
                //grdStockStatus.Refresh();

                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;

                //Finally, show the report form
                Common.frmReportViewer frm = new Common.frmReportViewer();
                frm.SetReportSource(m_rptStockStatus);

                //Update the progressbar
                ProgressForm.UpdateProgress(100, "Showing Report...");

                // Close the dialog
                ProgressForm.CloseForm();

            switch (myPrintType)
            {
                case PrintType.DirectPrint: //Direct Printer
                    m_rptStockStatus.PrintOptions.PrinterName = "";
                    m_rptStockStatus.PrintToPrinter(1, false, 0, 0);
                    return;
                case PrintType.Excel: //Excel
                    ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                    CrExportOptions = m_rptStockStatus.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    m_rptStockStatus.Export();
                    m_rptStockStatus.Close();
                    return;
                case PrintType.PDF: //PDF
                    PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                    CrExportOptions = m_rptStockStatus.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                    m_rptStockStatus.Export();
                    m_rptStockStatus.Close();
                    return;
                default: //Crystal Report
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }

            this.Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.DirectPrint);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgStockStatus.Columns.Clear();

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmStockStatus_Load(sender, e);

            this.Cursor = Cursors.Default;
            this.WindowState = FormWindowState.Maximized;
        }
        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_StockStatus.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_StockStatus.ProjectID + "'";

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
            tw.WriteStartElement("STOCKSTATUS");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ProjectIDSettings");
                    tw.WriteElementString("ProjectID", Convert.ToInt32(m_StockStatus.ProjectID).ToString());
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
