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
    public partial class frmPartyLedger : Form
    {

        private Inventory.Model.dsInventoryBookReport dsInventoryBookReport = new Model.dsInventoryBookReport();
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell subGroupView;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        ArrayList AccClassID = new ArrayList();
        private InventoryBookSettings m_IDBS;
        private string FileName = "";
        private DataTable dtTemp;
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

        public frmPartyLedger()
        {
            InitializeComponent();
        }

        public frmPartyLedger(InventoryBookSettings IDBS)
        {
            InitializeComponent();
            m_IDBS = new InventoryBookSettings();
            m_IDBS.partyGroupID = IDBS.partyGroupID;
            m_IDBS.PartyID = IDBS.PartyID;
            m_IDBS.DepotID = IDBS.DepotID;
            m_IDBS.ProjectID = IDBS.ProjectID;
            m_IDBS.FromDate = IDBS.FromDate;
            m_IDBS.ToDate = IDBS.ToDate;
            m_IDBS.AccClassID = IDBS.AccClassID;
            m_IDBS.VoucherType = IDBS.VoucherType;
        }

        private void DisplayBannar()
        {
            
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            //lblCompanyName.Text = m_CompanyDetails.CompanyName;
            //lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            //lblContact.Text = "Contact: " + m_CompanyDetails.Telephone + "Website: " + m_CompanyDetails.Website;
            //lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;

            //If it has a date range
            if (m_IDBS.FromDate != null && m_IDBS.ToDate != null)
            {
                lblAsonDate.Text = "From : " + Date.ToSystem((DateTime)m_IDBS.FromDate) + " To : "+ Date.ToSystem((DateTime)m_IDBS.ToDate);
            }
            else if (m_IDBS.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_IDBS.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FYFrom));

            //DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_IDBS.ProjectID), LangMgr.DefaultLanguage);
            //if (m_IDBS.ProjectID != null)
            //{
            //    DataRow drProjectInfo = dtProjectInfo.Rows[0];
            //    lblProjectName.Text =  drProjectInfo["Name"].ToString();
            //}
            //else
            //{
            //    lblProjectName.Text = "All";
            //}
            lblProject.Text = "Project : All";
        }

        //Customized header
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 10, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;
                AutomaticSortEnabled = false;
            }
        }

        private void MakeHeader(bool IsPartywise)
        {
            //Defining the HeaderPart

            grdInventoryDayBook[0, 0] = new MyHeader("Date");
            grdInventoryDayBook[0, 1] = new MyHeader("VoucherType");
            grdInventoryDayBook[0, 2] = new MyHeader("Voucher No");
            grdInventoryDayBook[0, 3] = new MyHeader("Party");
            grdInventoryDayBook[0, 4] = new MyHeader("Product Details");
            grdInventoryDayBook[0, 5] = new MyHeader("InBound Qty");
            grdInventoryDayBook[0, 6] = new MyHeader("OutBound Qty");
            grdInventoryDayBook[0, 7] = new MyHeader("Unit");
            grdInventoryDayBook[0, 8] = new MyHeader("Amount");
            grdInventoryDayBook[0, 9] = new MyHeader("RowID");
            grdInventoryDayBook[0, 10] = new MyHeader("VouType");//This column is used only for retriving value for further processing while double click evnet bt it is hidden

            //Define size of column
            grdInventoryDayBook[0, 0].Column.Width = 80;
            grdInventoryDayBook[0, 1].Column.Width = 95;
            grdInventoryDayBook[0, 2].Column.Width = 100;
            grdInventoryDayBook[0, 3].Column.Width = 170;
            grdInventoryDayBook[0, 4].Column.Width = 170;
            grdInventoryDayBook[0, 5].Column.Width = 100;
            grdInventoryDayBook[0, 6].Column.Width = 100;
            grdInventoryDayBook[0, 7].Column.Width = 80;
            grdInventoryDayBook[0, 8].Column.Width = 100;
            grdInventoryDayBook[0, 9].Column.Width = 80;
            grdInventoryDayBook[0, 9].Column.Visible = false;
            grdInventoryDayBook[0, 10].Column.Visible = false;
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_IDBS.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_IDBS.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in m_IDBS.AccClassID)
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

        #region old code
        ////private void DisplayInventoryDayBook(bool IsCrystalReport)
#endregion

        private void WriteInventoryDayBook(DataTable dt,bool IsCrystalReport)
        {
            try
            {
                int count = 0;
                string RowID, VoucherType;
                DateTime VouDate = new DateTime();
                RowID = VoucherType = "";

                for (int i = 1; i <=dt.Rows.Count; i++)
                {
                    SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                    if (i % 2 == 0)
                    {
                        //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                    }
                    else
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    DataRow dr = dt.Rows[i - 1];
                    string PartyName, ProductName;
                    ProductName = PartyName = "";
                    decimal m_NetSalesQty = 0;
                    //for writting on Grid
                    if (!IsCrystalReport)
                    {
                        grdInventoryDayBook.Rows.Insert(i);
                        DataTable dtPartyInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                        DataRow drPartyInfo = dtPartyInfo.Rows[0];
                        PartyName = drPartyInfo["LedName"].ToString();
                        VouDate= Convert.ToDateTime(dr["VoucherDate"]);
                        string myRowID =  dr["RowID"].ToString();
                        string myVchType = dr["VoucherType"].ToString();

                        //if Repeated Voucher No and Party name than wirte once only
                        if (RowID != myRowID || VoucherType != myVchType)//
                        {
                            grdInventoryDayBook[i, 0] = new SourceGrid.Cells.Cell(Date.ToSystem(VouDate));
                            grdInventoryDayBook[i, 0].AddController(dblClick);
                            grdInventoryDayBook[i, 0].View = GroupView;
                            grdInventoryDayBook[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                           
                            grdInventoryDayBook[i, 1] = new SourceGrid.Cells.Cell(dr["VoucherType"].ToString());
                            grdInventoryDayBook[i, 1].AddController(dblClick);
                            grdInventoryDayBook[i, 1].View = GroupView;
                            grdInventoryDayBook[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                           
                            grdInventoryDayBook[i, 2] = new SourceGrid.Cells.Cell(dr["VoucherNo"].ToString());
                            grdInventoryDayBook[i, 2].AddController(dblClick);
                            grdInventoryDayBook[i, 2].View = GroupView;
                            grdInventoryDayBook[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                           
                            grdInventoryDayBook[i, 3] = new SourceGrid.Cells.Cell(PartyName);
                            grdInventoryDayBook[i, 3].AddController(dblClick);
                            grdInventoryDayBook[i, 3].View = GroupView;
                            grdInventoryDayBook[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                          
                            DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
                            DataRow drProductInfo = dtProductInfo.Rows[0];
                            ProductName = drProductInfo["ProdName"].ToString();

                            grdInventoryDayBook[i, 4] = new SourceGrid.Cells.Cell(ProductName);
                            grdInventoryDayBook[i, 4].AddController(dblClick);
                            grdInventoryDayBook[i, 4].View = GroupView;
                            grdInventoryDayBook[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                          
                            grdInventoryDayBook[i, 5] = new SourceGrid.Cells.Cell(dr["InBound"].ToString());
                            grdInventoryDayBook[i, 5].AddController(dblClick);
                            grdInventoryDayBook[i, 5].View = GroupView;
                            grdInventoryDayBook[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
                           
                            grdInventoryDayBook[i, 6] = new SourceGrid.Cells.Cell(dr["OutBound"].ToString());
                            grdInventoryDayBook[i, 6].AddController(dblClick);
                            grdInventoryDayBook[i, 6].View = GroupView;
                            grdInventoryDayBook[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdInventoryDayBook[i, 7] = new SourceGrid.Cells.Cell("Unit");
                            grdInventoryDayBook[i, 7].AddController(dblClick);
                            grdInventoryDayBook[i, 7].View = GroupView;
                            grdInventoryDayBook[i, 7].View = new SourceGrid.Cells.Views.Cell(alternate);

                            double Amount = Convert.ToDouble(dr["Amount"]);
                            grdInventoryDayBook[i, 8] = new SourceGrid.Cells.Cell(Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            grdInventoryDayBook[i, 8].AddController(dblClick);
                            grdInventoryDayBook[i, 8].View = GroupView;
                            grdInventoryDayBook[i, 8].View = new SourceGrid.Cells.Views.Cell(alternate);    

                            RowID = dr["RowID"].ToString();
                            VoucherType = dr["VoucherType"].ToString();
                        }
                        else
                        {
                            grdInventoryDayBook[i, 0] = new SourceGrid.Cells.Cell();
                            grdInventoryDayBook[i, 0].AddController(dblClick);
                            grdInventoryDayBook[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                            string  VType = dr["VoucherType"].ToString();
                            grdInventoryDayBook[i, 1] = new SourceGrid.Cells.Cell();
                            
                            grdInventoryDayBook[i, 1].AddController(dblClick);
                            grdInventoryDayBook[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                           
                            grdInventoryDayBook[i, 2] = new SourceGrid.Cells.Cell();
                            grdInventoryDayBook[i, 2].AddController(dblClick);
                            grdInventoryDayBook[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                         
                            grdInventoryDayBook[i, 3] = new SourceGrid.Cells.Cell();
                            grdInventoryDayBook[i, 3].AddController(dblClick);
                            grdInventoryDayBook[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);

                            DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
                            DataRow drProductInfo = dtProductInfo.Rows[0];
                            ProductName = drProductInfo["ProdName"].ToString();

                            grdInventoryDayBook[i, 4] = new SourceGrid.Cells.Cell(ProductName);
                            grdInventoryDayBook[i, 4].AddController(dblClick);
                            grdInventoryDayBook[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdInventoryDayBook[i, 4].View = subGroupView;

                            grdInventoryDayBook[i, 5] = new SourceGrid.Cells.Cell(dr["InBound"].ToString());
                            grdInventoryDayBook[i, 5].AddController(dblClick);
                            grdInventoryDayBook[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdInventoryDayBook[i, 5].View = subGroupView;

                            grdInventoryDayBook[i, 6] = new SourceGrid.Cells.Cell(dr["OutBound"].ToString());
                            grdInventoryDayBook[i, 6].AddController(dblClick);
                            grdInventoryDayBook[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdInventoryDayBook[i, 6].View = subGroupView;

                            grdInventoryDayBook[i, 7] = new SourceGrid.Cells.Cell("Unit");
                            grdInventoryDayBook[i, 7].AddController(dblClick);
                            grdInventoryDayBook[i, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdInventoryDayBook[i, 7].View = subGroupView;

                            double Amount = Convert.ToDouble(dr["Amount"]);
                            grdInventoryDayBook[i, 8] = new SourceGrid.Cells.Cell(Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                            grdInventoryDayBook[i, 8].AddController(dblClick);
                            grdInventoryDayBook[i, 8].View = new SourceGrid.Cells.Views.Cell(alternate);
                            grdInventoryDayBook[i, 8].View = subGroupView;
                        }

                        grdInventoryDayBook[i, 9] = new SourceGrid.Cells.Cell(RowID);
                        grdInventoryDayBook[i, 10] = new SourceGrid.Cells.Cell(VoucherType);
                    }
                    else //for writting on Crystal Report
                    {
                        if (RowID != dr["RowID"].ToString() || VoucherType != dr["VoucherType"].ToString())//
                        {
                            VoucherType = dr["VoucherType"].ToString();
                            DataTable dtPartytInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["PartyID"]), LangMgr.DefaultLanguage);
                            DataRow drPartyInfo = dtPartytInfo.Rows[0];
                            PartyName = drPartyInfo["LedName"].ToString();
                            DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
                            DataRow drProductInfo = dtProductInfo.Rows[0];
                            ProductName = drProductInfo["ProdName"].ToString();
                            dsInventoryBookReport.Tables["tblInventoryBook"].Rows.Add(VouDate.ToShortDateString(), VoucherType, dr["VoucherNo"].ToString(), PartyName, ProductName, dr["InBound"].ToString(), dr["OutBound"].ToString(), "Unit", dr["Amount"].ToString());
                            RowID = dr["RowID"].ToString();
                            VoucherType = dr["VoucherType"].ToString();
                        }
                        else
                        {
                            DataTable dtProductInfo = Product.GetProductInfo(Convert.ToInt32(dr["ProductID"]), LangMgr.DefaultLanguage);
                            DataRow drProductInfo = dtProductInfo.Rows[0];
                            ProductName = drProductInfo["ProdName"].ToString();
                            dsInventoryBookReport.Tables["tblInventoryBook"].Rows.Add("", "", "", "", ProductName, dr["InBound"].ToString(), dr["OutBound"].ToString(), "Unit", dr["Amount"].ToString());
                        }
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        private void CreateColumnsforTempTable()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("PartyID", typeof(string));
            dtTemp.Columns.Add("Date", typeof(string));
            dtTemp.Columns.Add("VoucherType", typeof(string));
            dtTemp.Columns.Add("VoucherNo", typeof(string));
            dtTemp.Columns.Add("ProductOrParty", typeof(string));
            dtTemp.Columns.Add("InBoundQty", typeof(string));
            dtTemp.Columns.Add("OutBoundQty", typeof(string));
            dtTemp.Columns.Add("Amount", typeof(string));
            dtTemp.Columns.Add("RowID", typeof(string));


        }
        private void frmInventoryDayBook_Load(object sender, EventArgs e)
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
          // DisplayInventoryDayBook(false);
            DisplayBannar();
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grdInventoryDayBook_DoubleClick);
            dblClick.KeyUp += new KeyEventHandler(grdInventoryDayBook_keyUp);

            grdInventoryDayBook.Visible = false;
            dgPartyLedger.Visible = true;
            ProgressForm.UpdateProgress(40, "Initializing Report Viewer...");

            FillGridWithData();
            ProgressForm.UpdateProgress(80, "Initializing Report Viewer...");

            dgPartyLedger.SelectionMode = SourceGrid.GridSelectionMode.Row;
            dgPartyLedger.AllowDrop = false;
            dgPartyLedger.EnableSort = false;
            ProgressForm.UpdateProgress(100, "Preparing report for display...");

            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

        }

        DataTable dtData = new DataTable();
        decimal totalInboundQty = 0;
        decimal totalOutboundQty = 0;
        decimal totalAmount = 0;
        private void FillGridWithData()
        {
            CreateColumnsforTempTable();
            dtData = InventoryBook.GetInvPartyLedgerReport(m_IDBS.partyGroupID, m_IDBS.PartyID, m_IDBS.FromDate, m_IDBS.ToDate, "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>",m_IDBS.VoucherType);
            foreach(DataRow dr in dtData.Rows)
            {
              if(Convert.ToInt32(dr["PartyID"])!=0)
              {
                  dtTemp.Rows.Add(dr["PartyID"].ToString()," "," "," ",dr["ProductName"].ToString()," "," "," "," ");
              }
              else
              {
                  dtTemp.Rows.Add(dr["PartyID"].ToString(), dr["TransNepDate"].ToString(), dr["VoucherType"].ToString(), dr["VoucherNo"].ToString(), dr["ProductName"].ToString(), Convert.ToDecimal(dr["InBoundQty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["OutBoundQty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Convert.ToDecimal(dr["Amount"]) > 0 && (dr["VoucherType"].ToString() == "PURCH_RTN" || dr["VoucherType"].ToString() == "SLS_RTN")) ? "(" + Convert.ToDecimal(dr["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + ")" : Convert.ToDecimal(dr["Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["RowID"].ToString());
                  totalInboundQty += Convert.ToDecimal(dr["InBoundQty"]);
                  totalOutboundQty += Convert.ToDecimal(dr["OutBoundQty"]);
                      if(dr["VoucherType"].ToString() == "PURCH_RTN" || dr["VoucherType"].ToString() == "SLS_RTN")//only purch_rtn or sls_rtn exits at time because only purchase or sales voucher report is shown at a time
                  {
                       totalAmount=totalAmount-Convert.ToDecimal(dr["Amount"]);
                  }
                  else
                       totalAmount=totalAmount+Convert.ToDecimal(dr["Amount"]);

              }
            }

            #region datagrid binding
            DataView mView = new DataView(dtTemp);
            mView.AllowDelete = false;
            mView.AllowNew = false;
            dgPartyLedger.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
            dgPartyLedger.FixedRows = 1;
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

            CreateDataGridColumns(dgPartyLedger.Columns, bd);
            dgPartyLedger.DataSource = bd;
            dgPartyLedger.Width = dgPartyLedger.Width - 5;
            dgPartyLedger.Width = this.Width - 25;

            #endregion

            if (m_IDBS.VoucherType == "PURCHASE")
            {
                lblInboundQty.Text = "Total Purchase Qty = " + totalInboundQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblOutboundQty.Text = "Purchase Return Qty = " + totalOutboundQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            }
            else if (m_IDBS.VoucherType == "SALES")
            {
                //in sales sales means outbound n sales returns means inbound
                lblInboundQty.Text = "Total Sales Qty = " + totalOutboundQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblOutboundQty.Text = "Sales Return Qty = " + totalInboundQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            }
            lblTotalAmount.Text = "Total Amount = " + totalAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

        }
        private void CreateDataGridColumns(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
        {
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.Font = new Font("Arial", 10, FontStyle.Bold);
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            //View for party header
            SourceGrid.Cells.Views.Cell PartyldgHead = new SourceGrid.Cells.Views.Cell();
            PartyldgHead.Font = new Font(dgPartyLedger.Font, FontStyle.Bold);
            PartyldgHead.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightGray);
            PartyldgHead.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            PartyldgHead.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            PartyldgHead.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            PartyldgHead.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);

            //party header condition
            SourceGrid.Conditions.ConditionView con_PldgHead = new SourceGrid.Conditions.ConditionView(PartyldgHead);
            con_PldgHead.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    DataRowView row = (DataRowView)itemRow;
                    return Convert.ToInt32(row["PartyID"]) !=0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dgPartyLedger.Columns.Add("PartyID", "PartyID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Visible = false;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            gridColumn = dgPartyLedger.Columns.Add("Date", "Transact Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Conditions.Add(con_PldgHead);
            gridColumn.Width = 130;
            //gridColumn.DataCell.View = cellView;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None; 


            gridColumn = dgPartyLedger.Columns.Add("VoucherType", "Voucher Type", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Conditions.Add(con_PldgHead);
            gridColumn.Width = 130;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgPartyLedger.Columns.Add("VoucherNo", "VoucherNo", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Conditions.Add(con_PldgHead);
            gridColumn.Width = 130;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgPartyLedger.Columns.Add("ProductOrParty", "Product Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Conditions.Add(con_PldgHead);
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            gridColumn = dgPartyLedger.Columns.Add("InBoundQty", "InBound Quantity", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Conditions.Add(con_PldgHead);
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgPartyLedger.Columns.Add("OutBoundQty", "OutBound Quantity", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Conditions.Add(con_PldgHead);
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgPartyLedger.Columns.Add("Amount", "Amount", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Conditions.Add(con_PldgHead);
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.Width = 150;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgPartyLedger.Columns.Add("RowID", "RowID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Visible = false;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            dgPartyLedger.AutoStretchColumnsToFitWidth = true;

        }

        private void grdInventoryDayBook_keyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                grdInventoryDayBook_DoubleClick(sender, null);
        }
        private void grdInventoryDayBook_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //Get the Selected Row           
                int CurRow = dgPartyLedger.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(dgPartyLedger, new SourceGrid.Position(CurRow, 8));
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgPartyLedger, new SourceGrid.Position(CurRow, 2));
                string Type = (cellType.Value).ToString();
                if (cellType.Value.ToString() == " " || cellType.Value.ToString() == "")
                    return;
                if ((cellType.Value.ToString()) != "")//Dont Call the voucher form if there is no Ledger...no need to call Voucher form for Op. Bal/Total Amount etc
                {
                    int RowID = Convert.ToInt32(cellTypeID.Value);
                    string VoucherType = cellType.Value.ToString();

                    switch (VoucherType)
                    {

                        case "SALES":
                            frmSalesInvoice frm = new frmSalesInvoice(RowID);
                            frm.Show();
                            break;
                        case "SALES_RTN":
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

            Misc.WriteLogo(dsInventoryBookReport, "tblImage");

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

            //Fill the logo on the report
            //Misc.WriteLogo(dsPurchaseReport, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            //rpt.SetDataSource(dsPurchaseReport);
            // if (m_PurchaseReport.IsProductwise)
           // m_rptInventoryBook.SetDataSource(dsInventoryBookReport);
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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_ProductPartyName = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_ProductName = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_OpeningQty = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_ClosingQty = new CrystalDecisions.Shared.ParameterDiscreteValue();
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


            pdvCompany_Address.Value = m_CompanyDetails.Address1;
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
                 string companyname=UserPreference.GetValue("COMPANY_NAME", uid); 
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

            pdvReport_Head.Value = "Inventory Party Ledger";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            m_rptInventoryBook.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            pdvReport_ProductName.Value = " ";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_ProductName);
            m_rptInventoryBook.DataDefinition.ParameterFields["ProductName"].ApplyCurrentValues(pvCollection);

            pdvReport_OpeningQty.Value = 0;
            pvCollection.Clear();
            pvCollection.Add(pdvReport_OpeningQty);
            m_rptInventoryBook.DataDefinition.ParameterFields["OpeningQuantity"].ApplyCurrentValues(pvCollection);

            pdvReport_ClosingQty.Value = 0;
            pvCollection.Clear();
            pvCollection.Add(pdvReport_ClosingQty);
            m_rptInventoryBook.DataDefinition.ParameterFields["ClosingQuantity"].ApplyCurrentValues(pvCollection);

            pdvReport_ProductPartyName.Value = "Product Name";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_ProductPartyName);
            m_rptInventoryBook.DataDefinition.ParameterFields["PartyProductName"].ApplyCurrentValues(pvCollection);

            pdvFiscal_Year.Value = "Fiscal Year:" + ( m_CompanyDetails.FiscalYear);
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);

            m_rptInventoryBook.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_IDBS.FromDate != null && m_IDBS.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_IDBS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_IDBS.ToDate));
            }
            if (m_IDBS.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_IDBS.ToDate));
            }
            if (m_IDBS.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_IDBS.FromDate)) + " To " + Date.ToSystem(DateTime.Today);
            }
            if (m_IDBS.FromDate == null && m_IDBS.ToDate == null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(DateTime.Today);
            }
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);

            m_rptInventoryBook.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            pdvReport_PrintDate.Value = Date.ToSystem(DateTime.Today); ;
            pvCollection.Clear();
            pvCollection.Add(pdvReport_PrintDate);
            m_rptInventoryBook.DataDefinition.ParameterFields["PrintDate"].ApplyCurrentValues(pvCollection); 

          //  DisplayInventoryDayBook(true);

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
                case PrintType.Email://Email the Document
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

        private void frmInventoryDayBook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows
           // grdInventoryDayBook.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmInventoryDayBook_Load(sender, e); 
           // grdInventoryDayBook.Refresh();

            this.Cursor = Cursors.Default;
            this.WindowState = FormWindowState.Maximized;
            //Show complete notification
            //Global.Msg("Reload Complete!");
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
            Project.GetChildProjects(Convert.ToInt32(m_IDBS.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_IDBS.ProjectID + "'";

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
            tw.WriteStartElement
                ("INVENTORYBOOK");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ProjectIDSettings");
                    tw.WriteElementString("ProjectID", Convert.ToInt32(m_IDBS.ProjectID).ToString());
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
