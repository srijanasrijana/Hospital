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
    public partial class frmPurchaseRtnRegister : Form
    {
        ArrayList AccClassID = new ArrayList();
        private Inventory.Model.dsInventoryBookReport dsInventoryBookReport = new Model.dsInventoryBookReport();
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        private InventoryBookSettings m_PRR;
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell subGroupView;
        private string FileName = "";
      
        private DataTable tblBufferPurchaseRegister;
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;
        bool IsCrystalReport;

        Decimal Amount = 0;
        int TotalInBound = 0;
        int TotalOutBound = 0;
        private decimal OpeningQty = 0;
        ContextMenu Menu_Export;
        private string UnitSymbol = "";
        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }
        public frmPurchaseRtnRegister()
        {
            InitializeComponent();
        }

        public frmPurchaseRtnRegister(InventoryBookSettings PRR)
        {
            InitializeComponent();
            m_PRR = new InventoryBookSettings();//using for purpose of Purchase Return Register Settings
            m_PRR = PRR;
           
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

                AutomaticSortEnabled = true;
            }
        }
        private enum PurchaseReturnInvoiceGridColumn : int
        {
           SN, Date, VoucherType, VoucherNo, ProductOrParty, ProductName, InBoundQty, OutBoundQty, Unit, Amont, RowID, VoType
        };
        private void MakeHeader()
        {
            SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
            view.Font = new Font("Arial", 11, FontStyle.Bold);
            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.SN] = new SourceGrid.Cells.ColumnHeader("SN");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.SN].Column.Width = 80;
            grdPurchaseReturnRegister.Columns[(int)PurchaseReturnInvoiceGridColumn.SN].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.Date] = new SourceGrid.Cells.ColumnHeader("Date");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.Date].Column.Width = 100;
            grdPurchaseReturnRegister.Columns[(int)PurchaseReturnInvoiceGridColumn.Date].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.VoucherType] = new SourceGrid.Cells.ColumnHeader("VoucherType");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.VoucherType].Column.Width = 80;
            grdPurchaseReturnRegister.Columns[(int)PurchaseReturnInvoiceGridColumn.VoucherType].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.VoucherNo] = new SourceGrid.Cells.ColumnHeader("VoucherNo");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.VoucherNo].Column.Width = 80;
            grdPurchaseReturnRegister.Columns[(int)PurchaseReturnInvoiceGridColumn.VoucherNo].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.ProductOrParty] = new SourceGrid.Cells.ColumnHeader("Particulars");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.ProductOrParty].Column.Width = 150;
            grdPurchaseReturnRegister.Columns[(int)PurchaseReturnInvoiceGridColumn.ProductOrParty].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch; ;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.ProductName] = new SourceGrid.Cells.ColumnHeader("ProductName");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.ProductName].Column.Width = 150;
            grdPurchaseReturnRegister.Columns[(int)PurchaseReturnInvoiceGridColumn.ProductName].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch; ;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.InBoundQty] = new SourceGrid.Cells.ColumnHeader("InBound Qty");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.InBoundQty].Column.Visible = false;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.OutBoundQty] = new SourceGrid.Cells.ColumnHeader("PurchaseReturn Qty");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.OutBoundQty].Column.Width = 120;
            grdPurchaseReturnRegister.Columns[(int)PurchaseReturnInvoiceGridColumn.OutBoundQty].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
   
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.Unit] = new SourceGrid.Cells.ColumnHeader("Unit");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.Unit].Column.Width = 80;
            grdPurchaseReturnRegister.Columns[(int)PurchaseReturnInvoiceGridColumn.Unit].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.Amont] = new SourceGrid.Cells.ColumnHeader("Amont");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.Amont].Column.Width = 100;
            grdPurchaseReturnRegister.Columns[(int)PurchaseReturnInvoiceGridColumn.Amont].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.RowID] = new SourceGrid.Cells.ColumnHeader("RowID");
            grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.RowID].Column.Visible = false;

            //grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.VoType] = new SourceGrid.Cells.ColumnHeader("PartyID");
            //grdPurchaseReturnRegister[0, (int)PurchaseReturnInvoiceGridColumn.VoType].Column.Visible = false;

            grdPurchaseReturnRegister.AutoStretchColumnsToFitWidth = true;
        }

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            lblCompanyName.Text = m_CompanyDetails.CompanyName;
            lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            lblContact.Text = "Contact: " + m_CompanyDetails.Telephone ;
            lblWebsite.Text = "Website: " + m_CompanyDetails.Website;
            lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;


            //If it has a date range
            if (m_PRR.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_PRR.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear; 

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_PRR.ProjectID), LangMgr.DefaultLanguage);
            if (m_PRR.ProjectID != null)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = "Project Name " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text ="Project Name "+  "All";

            }
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID
            
            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_PRR.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_PRR.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in m_PRR.AccClassID)
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

        private void DisplayPurchaseReturnRegister(bool IsCrystalReport)
        {
                string AccClassIDsXMLString = ReadAllAccClassID();
                string ProjectIDsXMLString = ReadAllProjectID(); 
                          
                DataTable dtInventoryDayBookInfo = InventoryBook.GetInventoryBook(m_PRR.partyGroupID, m_PRR.PartyID, m_PRR.ProductGroupID, m_PRR.ProductID,
                 m_PRR.DepotID, m_PRR.ProjectID, m_PRR.FromDate, m_PRR.ToDate, InvenotryBookType.PurchaseReturnRegister,
                 AccClassIDsXMLString, ProjectIDsXMLString,ref OpeningQty);

                tblBufferPurchaseRegister = new DataTable();
                tblBufferPurchaseRegister.Columns.Add("SN", typeof(int));        
                tblBufferPurchaseRegister.Columns.Add("Date", typeof(string));
                tblBufferPurchaseRegister.Columns.Add("VoucherType", typeof(string));
                tblBufferPurchaseRegister.Columns.Add("VoucherNo", typeof(string));
                tblBufferPurchaseRegister.Columns.Add("ProductOrParty", typeof(string));
                tblBufferPurchaseRegister.Columns.Add("ProductName", typeof(string));
                tblBufferPurchaseRegister.Columns.Add("InBoundQty", typeof(string));
                tblBufferPurchaseRegister.Columns.Add("OutBoundQty", typeof(string));
                tblBufferPurchaseRegister.Columns.Add("Unit", typeof(string));
                tblBufferPurchaseRegister.Columns.Add("Amount", typeof(string));
                tblBufferPurchaseRegister.Columns.Add("RowID", typeof(int));
                tblBufferPurchaseRegister.Columns.Add("VouType", typeof(int));
                tblBufferPurchaseRegister.Columns.Add("PartyID", typeof(int));

                string DateField = "Date";
                if (Global.Default_Date == Date.DateType.Nepali)
                    DateField = "NepDate";

                var query = from o in dtInventoryDayBookInfo.AsEnumerable()
                            select new
                            {
                                //  VoucherDate = Convert.ToDateTime(o.Field<DateTime>("Date")).ToShortDateString(),
                                SN = Convert.ToInt32(o.Field<int>("SN")),
                                VoucherDate = o.Field<string>(DateField),
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
                    tblBufferPurchaseRegister.Rows.Add(dr.SN,dr.VoucherDate, dr.VoucherType, dr.VoucherNumber, dr.PartyName, dr.ProductName, dr.InBound, dr.OutBound,
                          dr.Unit, dr.Amount, dr.RowID, dr.PartyID);
                }

                lblOutBoundQty.Text = "PurchaseReturn Qty= " + TotalOutBound.ToString();
                lbInboundQty.Text = "Inbound Qty= " + TotalInBound.ToString();
                lblTotalAmt.Text = "Total Amount= " + Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                lblClosingQty.Text = "Closing Qty= " + (OpeningQty + TotalInBound - TotalOutBound).
                                      ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblAmountInWord.Text = "Amount In word:  " + AmountToWords.ConvertNumberAsText(Convert.ToString(Amount));

                WritePurchaseReturnRegister(tblBufferPurchaseRegister);
          
        }

        private void WritePurchaseReturnRegister(DataTable dt)
        {
            try
            {
                int count = 0;
                string RowID, VoucherType;
                RowID = VoucherType = "";

                int PartyID = 0;
                for (int i = 1; i <= dt.Rows.Count; i++)
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
                    double Amount = Convert.ToDouble(dr["Amount"]);
                   
                   // if (!IsCrystalReport)
                   // {
                        grdPurchaseReturnRegister.Rows.Insert(i);
                        if (RowID != dr["RowID"].ToString() || VoucherType != dr["VoucherType"].ToString())
                        {
                            grdPurchaseReturnRegister[i, 0] = new SourceGrid.Cells.Cell(dr["SN"].ToString());
                            grdPurchaseReturnRegister[i, 0].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 0].View = GroupView;
                            grdPurchaseReturnRegister[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);                      

                         
                            grdPurchaseReturnRegister[i, 1] = new SourceGrid.Cells.Cell(dr["Date"].ToString());
                            grdPurchaseReturnRegister[i, 1].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 1].View = GroupView;
                            grdPurchaseReturnRegister[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);                      

                            grdPurchaseReturnRegister[i, 3] = new SourceGrid.Cells.Cell(dr["VoucherNo"].ToString());
                            grdPurchaseReturnRegister[i, 3].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 3].View = GroupView;
                            grdPurchaseReturnRegister[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                           
                            grdPurchaseReturnRegister[i, 2] = new SourceGrid.Cells.Cell(dr["VoucherType"].ToString());
                            grdPurchaseReturnRegister[i, 2].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 2].View = GroupView;
                            grdPurchaseReturnRegister[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                      
                            grdPurchaseReturnRegister[i, 4] = new SourceGrid.Cells.Cell(dr["ProductOrParty"].ToString());
                            grdPurchaseReturnRegister[i, 4].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 4].View = GroupView;
                            grdPurchaseReturnRegister[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                            
                            grdPurchaseReturnRegister[i, 5] = new SourceGrid.Cells.Cell(dr["ProductName"].ToString());
                            grdPurchaseReturnRegister[i, 5].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 5].View = GroupView;
                            grdPurchaseReturnRegister[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 6] = new SourceGrid.Cells.Cell(dr["InBoundQty"].ToString());
                            grdPurchaseReturnRegister[i, 6].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 6].View = GroupView;
                            grdPurchaseReturnRegister[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 6] = new SourceGrid.Cells.Cell(dr["OutBoundQty"].ToString());
                            grdPurchaseReturnRegister[i, 6].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 6].View = GroupView;
                            grdPurchaseReturnRegister[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 8] = new SourceGrid.Cells.Cell("Unit");
                            grdPurchaseReturnRegister[i, 8].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 8].View = GroupView;
                            grdPurchaseReturnRegister[i, 8].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 9] = new SourceGrid.Cells.Cell((Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
                            grdPurchaseReturnRegister[i, 9].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 9].View = GroupView;
                            grdPurchaseReturnRegister[i, 9].View = new SourceGrid.Cells.Views.Cell(alternate);

                            RowID = dr["RowID"].ToString();
                            VoucherType = dr["VoucherType"].ToString();
                        }
                        else
                        {
                            grdPurchaseReturnRegister[i, 0] = new SourceGrid.Cells.Cell();
                            grdPurchaseReturnRegister[i, 0].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 1] = new SourceGrid.Cells.Cell();
                            grdPurchaseReturnRegister[i, 1].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 1].View = new SourceGrid.Cells.Views.Cell(alternate);


                            grdPurchaseReturnRegister[i, 2] = new SourceGrid.Cells.Cell();
                            grdPurchaseReturnRegister[i, 2].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 2].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 3] = new SourceGrid.Cells.Cell();
                            grdPurchaseReturnRegister[i, 3].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 3].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 4] = new SourceGrid.Cells.Cell();
                            grdPurchaseReturnRegister[i, 4].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                            
                            grdPurchaseReturnRegister[i, 5] = new SourceGrid.Cells.Cell(dr["ProductName"].ToString());
                            grdPurchaseReturnRegister[i, 5].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 5].View = subGroupView;
                            grdPurchaseReturnRegister[i, 5].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 6] = new SourceGrid.Cells.Cell(dr["InBoundQty"].ToString());
                            grdPurchaseReturnRegister[i, 6].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 6].View = subGroupView;
                            grdPurchaseReturnRegister[i, 6].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 7] = new SourceGrid.Cells.Cell(dr["OutBoundQty"].ToString());
                            grdPurchaseReturnRegister[i, 7].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 7].View = subGroupView;
                            grdPurchaseReturnRegister[i, 7].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 8] = new SourceGrid.Cells.Cell("Unit");
                            grdPurchaseReturnRegister[i, 8].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 8].View = subGroupView;
                            grdPurchaseReturnRegister[i, 8].View = new SourceGrid.Cells.Views.Cell(alternate);

                            grdPurchaseReturnRegister[i, 9] = new SourceGrid.Cells.Cell((Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))));
                            grdPurchaseReturnRegister[i, 9].AddController(dblClick);
                            grdPurchaseReturnRegister[i, 9].View = subGroupView;
                            grdPurchaseReturnRegister[i, 9].View = new SourceGrid.Cells.Views.Cell(alternate);
                        }

                        grdPurchaseReturnRegister[i, 10] = new SourceGrid.Cells.Cell(RowID);
                        grdPurchaseReturnRegister[i, 10].AddController(dblClick);

                       

                      
                    }
                    
                    count++;
                }

          //  }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void DisplayOpeningQty()
        {

            #region Display Opening Quantity on the top
            //Get root accounting class
            int RootAccClassID;
            try
            {
                DataTable dtRootAccClass = AccountClass.GetRootAccClass(Convert.ToInt32(m_PRR.AccClassID[0]));
                RootAccClassID = Convert.ToInt32(dtRootAccClass.Rows[0]["AccClassID"]);
            }
            catch
            {
                throw new Exception("Invalid Accounting Class selected");
            }
            //Write Opening Quantity
            try
            {

                if (m_PRR.ProductID != null)
                    OpeningQty = Product.GetOpeningQty(Convert.ToInt32(m_PRR.AccClassID[0]), ProductID: Convert.ToInt32(m_PRR.ProductID));
                else if (m_PRR.ProductGroupID != null)
                    OpeningQty = Product.GetOpeningQty(Convert.ToInt32(m_PRR.AccClassID[0]), GroupID: Convert.ToInt32(m_PRR.ProductGroupID));
                else //When both are not given that means All products is selected
                    OpeningQty = Product.GetOpeningQty(Convert.ToInt32(m_PRR.AccClassID[0]));

            }
            catch (Exception ex)
            {
                Global.MsgError("Error getting Opening Quantity " + ex.Message);
                OpeningQty = 0;
            }

            //Write on the label
            lblOpeningQty.Text = "Opening Qty= " + OpeningQty.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " " + UnitSymbol;
            #endregion

        }

        private void frmPurchaseReturnRegister_Load(object sender, EventArgs e)
        {


            #region BLOCK FOR ORIENTATION PURPOSE
                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                dblClick.DoubleClick += new EventHandler(grdPurchaseReturnRegister_DoubleClick);     
                //Text format for Total
                GroupView = new SourceGrid.Cells.Views.Cell();
                GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                //Text format for Ledgers
                subGroupView = new SourceGrid.Cells.Views.Cell();
                subGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                subGroupView.ForeColor = Color.Blue;

                //Let the whole row to be selected
                grdPurchaseReturnRegister.SelectionMode = SourceGrid.GridSelectionMode.Row;

                gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
                grdPurchaseReturnRegister.Controller.AddController(gridKeyDown);

                //Disable multiple selection
                grdPurchaseReturnRegister.Selection.EnableMultiSelection = false;
                //Disable multiple selection
                grdPurchaseReturnRegister.Redim(1, 11);

                int rows = grdPurchaseReturnRegister.Rows.Count;
                grdPurchaseReturnRegister.Rows.Insert(rows);
                //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
                MakeHeader();

                DisplayBannar();

                DisplayOpeningQty();
           
            #endregion
                DisplayPurchaseReturnRegister(false);
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
            rptInventoryBookReport m_rptInventoryBook = new rptInventoryBookReport();
            //Fill the logo on the report
            Misc.WriteLogo(dsInventoryBookReport, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            m_rptInventoryBook.SetDataSource(dsInventoryBookReport);
            
            m_rptInventoryBook.SetDataSource(dsInventoryBookReport);
            
            try
            {
                dsInventoryBookReport.Tables.Remove("tblInventoryBook");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            System.Data.DataView view = new System.Data.DataView(tblBufferPurchaseRegister);
            DataTable selected = view.ToTable("tblInventoryBook", false, "PartyID", "Date", "VoucherType", "VoucherNo", "ProductOrParty", "InBoundQty", "OutBoundQty", "Amount");
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

            pdvReport_Head.Value = "Purchase Return Register Report";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            m_rptInventoryBook.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            pdvFiscal_Year.Value = "Fiscal Year:" + (m_CompanyDetails.FiscalYear);
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);

            m_rptInventoryBook.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            pdvProductName.Value = "Sales Return Product";
            pvCollection.Clear();
            pvCollection.Add(pdvProductName);
            m_rptInventoryBook.DataDefinition.ParameterFields["ProductName"].ApplyCurrentValues(pvCollection);

            pdvPartyProductName.Value = "Particulars";
            pvCollection.Clear();
            pvCollection.Add(pdvPartyProductName);
            m_rptInventoryBook.DataDefinition.ParameterFields["PartyProductName"].ApplyCurrentValues(pvCollection);

            pdvInBoundQty.Value =TotalInBound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvInBoundQty);
            m_rptInventoryBook.DataDefinition.ParameterFields["InBoundQty"].ApplyCurrentValues(pvCollection);

            pdvOutBoundQty.Value =TotalOutBound.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
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

            pdvTotalAmount.Value =Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            pvCollection.Clear();
            pvCollection.Add(pdvTotalAmount);
            m_rptInventoryBook.DataDefinition.ParameterFields["Total_Amt"].ApplyCurrentValues(pvCollection);

            m_rptInventoryBook.SetParameterValue("Total_Amt", Amount);


            string inwords = AmountToWords.ConvertNumberAsText(Convert.ToString(Amount));
            m_rptInventoryBook.SetParameterValue("AmtInWords", inwords);

          
          
            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_PRR.FromDate != null && m_PRR.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_PRR.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_PRR.ToDate));
            }
            if (m_PRR.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_PRR.ToDate));
            }
            if (m_PRR.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_PRR.FromDate));
            }
            if (m_PRR.FromDate == null && m_PRR.ToDate == null)
            {
                pdvReport_Date.Value = "";

            }

            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);

            m_rptInventoryBook.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            DisplayPurchaseReturnRegister(true);

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
           grdPurchaseReturnRegister.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmPurchaseReturnRegister_Load(sender, e);
            grdPurchaseReturnRegister.Refresh();

            this.Cursor = Cursors.Default;
           
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }
        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_PRR.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_PRR.ProjectID + "'";

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
                    tw.WriteElementString("ProjectID", Convert.ToInt32(m_PRR.ProjectID).ToString());
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
        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.DirectPrint);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdPurchaseReturnRegister_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                //Get the Selected Row           
                int CurRow = grdPurchaseReturnRegister.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellTypeID = new SourceGrid.CellContext(grdPurchaseReturnRegister, new SourceGrid.Position(CurRow, 10));
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdPurchaseReturnRegister, new SourceGrid.Position(CurRow, 2));
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

        private void grdPurchaseReturnRegister_Paint(object sender, PaintEventArgs e)
        {

        }

      
    }
}
