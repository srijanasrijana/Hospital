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
using ErrorManager;
using System.IO;
using Inventory.CrystalReports;
using Inventory.DataSet;
using System.Data.SqlClient;
using System.Collections;
using System.Threading;
using RegistryManager;
using DBLogic;
using Common;

namespace AccSwift.Forms
{
    public partial class frmfiscalyearclosing : Form, IfrmDateConverter, IfrmCompanyFiscalYear
    {
        //For Writing Opening Balance in newly created DB
        CompanyDetails CompDetails = new CompanyDetails();
        private bool IsFromDate = false;
        private bool IsToDate = false;
        private bool IsDate = false;
        DataTable dtAssestsLedgers = new DataTable();
        DataTable dtLiabilitiesLedgers = new DataTable();
        DataTable dtFinalResult = new DataTable();
        DataTable dtInventoryStock = new DataTable();
        DataTable dtInventory = null;
        decimal OpeningStock = 0;
        decimal ClosingStock = 0;
        private int rows = 0;
        private string FilterString = "";
        private string closingamount;
        ListItem liAccClass = new ListItem();
        private SourceGrid.Cells.Views.Cell LedgerHeadView;
        private int sno = 1;
        private int snoinsert = 2;
        private int count;
        private int snoinv = 1;
        private int snoinsertinv = 2;
        private int countinv;
        private string DrCr;
        private ArrayList AccClassIDs = new ArrayList();
        ArrayList AccClassID = new ArrayList();
        DataTable dtdepreciation = new DataTable();
        Form ParentForm = null;

        private string Code="";
        private string Value = "";

        private string prevDBName = "";
        string newDBName = "";
        public frmfiscalyearclosing()
        {
            InitializeComponent();
        }
        public frmfiscalyearclosing(Form ParentForm)
        {
            this.ParentForm = ParentForm;
            InitializeComponent();
        }
        public void DateConvert(DateTime DotNetDate)
        {
            if (IsFromDate)//If form date is selected
            {
                txtStartDate.Text = Date.ToSystem(DotNetDate);
            }
            else if (IsToDate)//IF TO date is selected
            {
                txtEndDate.Text = Date.ToSystem(DotNetDate);
            }
            else
            {
                txtDate.Text = Date.ToSystem(DotNetDate);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            IsFromDate = false;
            IsToDate = false;
            IsDate = true;
            DateTime dtDate = Date.ToDotNet(txtDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
         
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            IsFromDate = true;//this variable is used as flag to notify which date is selected to change the date converter...coz same funtion is used to change the date  
            IsToDate = false;
            IsDate = false;
            DateTime dtDate = Date.ToDotNet(txtStartDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
             IsFromDate = false;
            IsToDate = true;
            IsDate = false;
            DateTime dtDate = Date.ToDotNet(txtEndDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }
        DataTable dttt = new DataTable();
        decimal opnstock = 0;
            decimal clostock=0;

            DataTable dtLedgerData;
            DataTable dtTempLedger;
            DataTable dtTempProduct;
        private void CreateColumnForTempLedger()
            {
                dtTempLedger = new DataTable();
                dtTempLedger.Columns.Add("LedgerID", typeof(string));
                dtTempLedger.Columns.Add("LedgerName", typeof(string));
                dtTempLedger.Columns.Add("FinalBal", typeof(string));
                dtTempLedger.Columns.Add("DrCr", typeof(string));              
            }

        private void CreateColumnForTempProduct()
        {
            dtTempProduct = new DataTable();
            dtTempProduct.Columns.Add("productID", typeof(string));
            dtTempProduct.Columns.Add("ProductName", typeof(string));
            dtTempProduct.Columns.Add("ClosingStock", typeof(string));
            dtTempProduct.Columns.Add("OpenPurchaseRate", typeof(string));
            dtTempProduct.Columns.Add("SalesRate", typeof(string)) ;

        }
        private void frmfiscalyearclosing_Load(object sender, EventArgs e)
        {
            try
            {
                dtInventory = Product.GetAllProductOpnNCloStock("<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>",null, DateTime.Now,0,0, ref opnstock, ref clostock);
                dtLedgerData = Ledger.GetAllGroupNLedgerBal("<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>", null,DateTime.Now, 0, null);

                FillLedgerGrid();
                FillInventoryGrid();
                //ListAccountClass(cboAccountClass);
                txtDate.Mask = Date.FormatToMask();
                txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
                txtStartDate.Mask = Date.FormatToMask();
                txtEndDate.Mask = Date.FormatToMask();
                txtStartDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
                txtEndDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());

                #region calculate income/loss
                //DataTable dtIncome = Ledger.GetLedgerDetails1(null, null, null, null, null, 3, null); // for assets
                //DataTable dtExpenditure = Ledger.GetLedgerDetails1(null, null, null, null, null, 4, null); // for liabilities

                //object strincomeDR = dtIncome.Compute("Sum(FinalBal)", "DrCr = 'DEBIT'");
                //object strincomeCR = dtIncome.Compute("Sum(FinalBal)", "DrCr = 'CREDIT'");

                //object strExpDR = dtExpenditure.Compute("Sum(FinalBal)", "DrCr = 'DEBIT'");
                //object strExpCR = dtExpenditure.Compute("Sum(FinalBal)", "DrCr = 'CREDIT'");

                //decimal income =  Convert.ToDecimal(strincomeCR == DBNull.Value ? 0 : strincomeCR) - Convert.ToDecimal(strincomeDR == DBNull.Value ? 0 : strincomeDR);
                //decimal expenses = Convert.ToDecimal(strExpDR == DBNull.Value ? 0 : strExpDR) - Convert.ToDecimal(strExpCR == DBNull.Value ? 0 : strExpCR);

                //decimal income = Convert.ToDecimal((dtIncome.Rows[0]["DrCr"].ToString() == "CREDIT") ? (Convert.ToDecimal(stringcome) * -1).ToString() : stringcome);
                //decimal expenses = Convert.ToDecimal((dtExpenditure.Rows[0]["DrCr"].ToString() == "CREDIT") ? (Convert.ToDecimal(stringExpenses) * -1).ToString() : stringExpenses);

                //object sumObject;
                //sumObject = dtExpenditure.Compute("Sum(FinalBal) where DrCr = 'DEBIT'", "");

                //OpeningStock = Convert.ToDecimal(dtInventory.Compute(" OpeningStock * OpenPurchaseRate ", ""));
                //ClosingStock = Convert.ToDecimal(dtInventory.Compute(" ClosingStock * OpenPurchaseRate ", ""));

                //decimal profit = income - expenses;
                //foreach (DataRow dr in dtInventory.Rows)
                //{
                //    OpeningStock += (Convert.ToDecimal(dr["OpeningStock"]) * Convert.ToDecimal(dr["OpenPurchaseRate"]));
                //    ClosingStock += (Convert.ToDecimal(dr["ClosingStock"]) * Convert.ToDecimal(dr["OpenPurchaseRate"]));
                //}
                decimal profit = (incomeTotal + clostock) - (expTotal + opnstock);
                lblProfit.Text = profit.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                #endregion

                //// write the columns of table to fill in the grid
                //dtFinalResult.Columns.Add("LedgerID");
                //dtFinalResult.Columns.Add("LedgerName");
                //dtFinalResult.Columns.Add("Amount");
                //dtFinalResult.Columns.Add("DrCr");
                //dtFinalResult.Columns.Add("AccClassID");

                ////Write for the inventory stock
                //dtInventoryStock.Columns.Add("ProductID");
                //dtInventoryStock.Columns.Add("ProductName");
                //dtInventoryStock.Columns.Add("AccClassID");
                //dtInventoryStock.Columns.Add("OpenPurchaseQty");
                //dtInventoryStock.Columns.Add("OpenPurchaseRate");
                //dtInventoryStock.Columns.Add("OpenSalesRate");

                //BusinessLogic.FiscalYearClosing fyc = new FiscalYearClosing(); 
                //DataTable dt = fyc.GetAccountClasses();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    DataRow dr = dt.Rows[i];
                //    ShowClosingBalance(Convert.ToInt32(dr["AccClassID"].ToString()),dr["EngName"].ToString());
                //    ShowClosingQuantity(Convert.ToInt32(dr["AccClassID"].ToString()), dr["EngName"].ToString());
                //}
                //txtdate.Focus();

                DataTable dt = AccountClass.GetAccClassTable(0);
                cboAccountClass.DataSource = dt;
                cboAccountClass.DisplayMember = "EngName";
                cboAccountClass.ValueMember = "AccClassID";
                cboAccountClass.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
          
        }

        #region old code

        private void AddGridHeader()
        {
            grdfiscalyear[0, 0] = new MyHeader("LedgerID");
            grdfiscalyear[0, 1] = new MyHeader("Ledger Name");
            grdfiscalyear[0, 2] = new MyHeader("DrCr");
            grdfiscalyear[0, 3] = new MyHeader("Closing Balance");
            grdfiscalyear[0, 4] = new MyHeader("Opening Balance");
            grdfiscalyear[0, 5] = new MyHeader("AccClassID");

            grdfiscalyear[0, 0].Column.Width = 5;
            grdfiscalyear[0, 1].Column.Width = 160;
            grdfiscalyear[0, 2].Column.Width = 60;
            grdfiscalyear[0, 3].Column.Width = 130;
            grdfiscalyear[0, 4].Column.Width = 130;
            grdfiscalyear[0, 5].Column.Width = 5;

            grdfiscalyear[0, 0].Column.Visible = false;
            grdfiscalyear[0, 5].Column.Visible = false;
        }
        private void AddGridHeaderInventory()
        {
            grdinventoryfiscalyear[0, 0] = new MyHeader("ProductID");
            grdinventoryfiscalyear[0, 1] = new MyHeader("ProductName");
            grdinventoryfiscalyear[0, 2] = new MyHeader("Closing Qty");
            grdinventoryfiscalyear[0, 3] = new MyHeader("Opening Qty");
            grdinventoryfiscalyear[0, 4] = new MyHeader("Purchase Rate");
            grdinventoryfiscalyear[0, 5] = new MyHeader("Sales Rate");
            grdinventoryfiscalyear[0, 6] = new MyHeader("AccClassID");

            grdinventoryfiscalyear[0, 0].Column.Width = 5;
            grdinventoryfiscalyear[0, 1].Column.Width = 130;
            grdinventoryfiscalyear[0, 2].Column.Width = 100;
            grdinventoryfiscalyear[0, 3].Column.Width = 100;
            grdinventoryfiscalyear[0, 4].Column.Width = 100;
            grdinventoryfiscalyear[0, 5].Column.Width = 100;
            grdinventoryfiscalyear[0, 6].Column.Width = 5;

            grdinventoryfiscalyear[0, 0].Column.Visible = false;
            grdinventoryfiscalyear[0, 6].Column.Visible = false;
        }
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

        public void FillGrid(DataTable dt, string AccountClassName)
        {
            try
            {
                //grdfiscalyear.Rows.Clear();
                grdfiscalyear.Redim(count + 5, 7);
                AddGridHeader();
                SourceGrid.Cells.Views.Cell header = new SourceGrid.Cells.Views.Cell();
                header.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Cornsilk);
                header.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                header.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                header.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                header.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);

                SourceGrid.Cells.Editors.TextBox root = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                root.EditableMode = SourceGrid.EditableMode.None;
                grdfiscalyear[sno, 1] = new SourceGrid.Cells.Cell("", root);
                grdfiscalyear[sno, 1].Value = AccountClassName;
                grdfiscalyear[sno, 1].ColumnSpan = grdfiscalyear.ColumnsCount - 2;
                grdfiscalyear[sno, 1].View = new SourceGrid.Cells.Views.Cell(header);
                // grdTransaction[TransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                for (int i = 1; i <= dtFinalResult.Rows.Count; i++)
                {
                    DataRow dr = dtFinalResult.Rows[i - 1];
                    //grdfiscalyear.Rows.Insert(i);
                    SourceGrid.Cells.Editors.TextBox LedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    LedgerID.EditableMode = SourceGrid.EditableMode.None;
                    grdfiscalyear[snoinsert, 0] = new SourceGrid.Cells.Cell("", LedgerID);
                    grdfiscalyear[snoinsert, 0].Value = dr["LedgerID"].ToString();

                    SourceGrid.Cells.Editors.TextBox LedgerName = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    LedgerName.EditableMode = SourceGrid.EditableMode.None;
                    grdfiscalyear[snoinsert, 1] = new SourceGrid.Cells.Cell("", LedgerName);
                    grdfiscalyear[snoinsert, 1].Value = dr["LedgerName"].ToString();

                    SourceGrid.Cells.Editors.TextBox DrCr = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    DrCr.EditableMode = SourceGrid.EditableMode.None;
                    grdfiscalyear[snoinsert, 2] = new SourceGrid.Cells.Cell("", DrCr);
                    grdfiscalyear[snoinsert, 2].Value = dr["DrCr"].ToString();

                    SourceGrid.Cells.Editors.TextBox ClosingAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    ClosingAmount.EditableMode = SourceGrid.EditableMode.None;
                    grdfiscalyear[snoinsert, 3] = new SourceGrid.Cells.Cell("", ClosingAmount);
                    grdfiscalyear[snoinsert, 3].Value = dr["Amount"].ToString();

                    SourceGrid.Cells.Editors.TextBox OpeningAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    OpeningAmount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                    grdfiscalyear[snoinsert, 4] = new SourceGrid.Cells.Cell("", OpeningAmount);

                    SourceGrid.Cells.Editors.TextBox AccID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    AccID.EditableMode = SourceGrid.EditableMode.None;
                    grdfiscalyear[snoinsert, 5] = new SourceGrid.Cells.Cell("", AccID);
                    grdfiscalyear[snoinsert, 5].Value = dr["AccClassID"].ToString();

                    BusinessLogic.Depreciation dep = new Depreciation();
                    dtdepreciation = dep.GetDataFromDepreciation(Convert.ToInt32(dr["LedgerID"].ToString()));
                    if (dtdepreciation.Rows.Count <= 0)
                    {
                        grdfiscalyear[snoinsert, 4].Value = dr["Amount"].ToString();
                    }
                    else
                    {
                        DataRow drdepreciation = dtdepreciation.Rows[0];
                        double depvalue = Convert.ToDouble(drdepreciation["DepreciationPercentage"].ToString());
                        double amt = Convert.ToDouble(dr["Amount"].ToString());
                        double result = amt - (amt * (depvalue / 100));
                        grdfiscalyear[snoinsert, 4].Value = result;
                    }
                    snoinsert += 1;
                }
                snoinsert += 1;
                sno += dtFinalResult.Rows.Count + 1;
                dtFinalResult.Rows.Clear();
            }

            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        } 
        #endregion

        #region new methods to load all ledgers with closing balance
        DataTable dtledgerDetails = new DataTable();
        System.Data.DataSet ds=new System.Data.DataSet();
        decimal expTotal = 0;
        decimal incomeTotal = 0;
        public void FillLedgerGrid() //(DataTable dt, string AccountClassName)
        {
            try
            {

                CreateColumnForTempLedger();
                //dtledgerDetails.Columns.Add("LedgerName", typeof(string));
                //dtledgerDetails.Columns.Add("LedgerCode", typeof(string));
                //dtledgerDetails.Columns.Add("LedgerID", typeof(string));
                //dtledgerDetails.Columns.Add("GroupName", typeof(string));
                //dtledgerDetails.Columns.Add("GroupID", typeof(string));
                //dtledgerDetails.Columns.Add("DebitTotal", typeof(string));
                //dtledgerDetails.Columns.Add("CreditTotal", typeof(string));
                //dtledgerDetails.Columns.Add("OpenBalDr", typeof(string));
                //dtledgerDetails.Columns.Add("OpenBalCr", typeof(string));
                //dtledgerDetails.Columns.Add("FinalBal", typeof(string));
                //dtledgerDetails.Columns.Add("DrCr", typeof(string));

                //dtledgerDetails = Ledger.GetLedgerDetails1(null, null, null, null, null, 1, null); // for assets
                //DataTable dtLiabilityledgerDetails = Ledger.GetLedgerDetails1(null, null, null, null, null, 8, null); // for liabilities

                //dtledgerDetails.Merge(dtLiabilityledgerDetails);

                //foreach (DataRow dr in dtledgerDetails.Rows)
                //{
                //    string finalBal =  Convert.ToDecimal(dr["FinalBal"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //    dr["FinalBal"] = finalBal;
                //}

                //add relation between column accountid and parentgroupid
                if (dtLedgerData.Rows.Count > 0)
                {
                    ds.Tables.Add(dtLedgerData);
                    ds.Relations.Add("ChildRows", ds.Tables[0].Columns["AccountID"], ds.Tables[0].Columns["ParentGroupID"], false);
                    foreach (DataRow dr in dtLedgerData.Rows)
                    {
                        //for Assets account get all of its child group and ledger
                        if ((Convert.ToInt32(dr["ParentGroupID"]) == 0 && Convert.ToInt32(dr["AccountID"]) == 1) || (Convert.ToInt32(dr["ParentGroupID"]) == 0 && Convert.ToInt32(dr["AccountID"]) == 8))
                        {
                            DataRow[] AChildRows = dr.GetChildRows("ChildRows");
                            foreach (DataRow Arow in AChildRows)
                            {
                                if (Arow["AccountType"].ToString() == "GROUP")
                                {
                                    FormatDataInTable(Arow);
                                }
                                //else if (Arow["AccountType"].ToString() == "LEDGER")
                                //{
                                //    dtTempLedger.Rows.Add(dr["AccountID"].ToString(),dr["AccountName"].ToString())
                                //}
                            }
                        }

                        else if (Convert.ToInt32(dr["ParentGroupID"]) == 0 && Convert.ToInt32(dr["AccountID"]) == 4)//expenditure
                        {
                            expTotal = (Convert.ToDecimal(dr["OpenBalDr"]) + Convert.ToDecimal(dr["DebitTotal"])) - (Convert.ToDecimal(dr["OpenBalCr"]) + Convert.ToDecimal(dr["CreditTotal"]));
                        }

                        else if (Convert.ToInt32(dr["ParentGroupID"]) == 0 && Convert.ToInt32(dr["AccountID"]) == 3)//income
                        {
                            incomeTotal = (Convert.ToDecimal(dr["OpenBalCr"]) + Convert.ToDecimal(dr["CreditTotal"])) - (Convert.ToDecimal(dr["OpenBalDr"]) + Convert.ToDecimal(dr["DebitTotal"]));
                        }
                    }
                }
                DataView mView = new DataView(dtTempLedger);
                mView.AllowDelete = false;
                mView.AllowNew = false;
                mView.AllowEdit = true;

                dgLedger.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
                dgLedger.FixedRows = 1;
                DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

                CreateLedgerDataGridColumns(dgLedger.Columns, bd);
                dgLedger.DataSource = bd;
            }




            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }


        private void FormatDataInTable(DataRow dr)//level is for adding space infront of name and flag is for deciding wheather it is debit or credit balance to calculate
        {
            decimal balance = 0;
            string drcrType = "";
            //get all immediate child rows of current row
            DataRow[] ChildRows = dr.GetChildRows("ChildRows");
            foreach (DataRow childRow in ChildRows)
            {
                balance = 0;
                if (childRow["AccountType"].ToString() == "GROUP")
                {
                    FormatDataInTable(childRow);
                }
                else if(childRow["AccountType"].ToString() == "LEDGER")
                {
                    balance = (Convert.ToDecimal(childRow["OpenBalDr"]) + Convert.ToDecimal(childRow["DebitTotal"])) - (Convert.ToDecimal(childRow["OpenBalCr"]) + Convert.ToDecimal(childRow["CreditTotal"]));
                    if(balance>=0)
                    {
                        drcrType = "DEBIT";
                    }
                    else
                    {
                        drcrType="CREDIT";
                    }
                    dtTempLedger.Rows.Add(childRow["AccountID"].ToString(), childRow["AccountName"].ToString(), Math.Abs(balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drcrType);
                }
            }
        }
        private void CreateLedgerDataGridColumns(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
        {
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            SourceGrid.Cells.Views.Cell cellView = new SourceGrid.Cells.Views.Cell();
            cellView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

            SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            viewNumeric.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dgLedger.Columns.Add("LedgerID", "LedgerID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.Visible = false;

            gridColumn = dgLedger.Columns.Add("LedgerName", "LedgerName", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 200;

            gridColumn = dgLedger.Columns.Add("FinalBal", "Closing Balance", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 100;

            gridColumn = dgLedger.Columns.Add("FinalBal", "Opening Balance", new SourceGrid.Cells.Editors.TextBox(typeof(string)));
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 120;
            //gridColumn.DataCell.Editor =  
            gridColumn = dgLedger.Columns.Add("DrCr", "DrCr", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 90;

            dgLedger.AutoStretchColumnsToFitWidth = true;

        } 
        #endregion

        #region new methods to load inventory grid with closing stock

        private void FillInventoryGrid()
        {
            CreateColumnForTempProduct();
            try
            {
                if(dtInventory.Rows.Count>0)
                {
                    foreach (DataRow dr in dtInventory.Rows) 
                    {
                        dtTempProduct.Rows.Add(dr["ProductID"].ToString(), dr["ProductName"].ToString(), Convert.ToDecimal(dr["ClosingQty"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["AveragePurchaseRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(dr["SalesRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    }
                }
          //      dtInventory = Product.GetAllProduct(null, 0, " ", DateTime.Today, true, StockStatusType.DiffInStock);
                DataView mView = new DataView(dtTempProduct);
                mView.AllowDelete = false;
                mView.AllowNew = false;
                mView.AllowEdit = true;

                dgInventory.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
                dgInventory.FixedRows = 1;
                DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);

                CreateInventoryDataGridColumns(dgInventory.Columns, bd);
                dgInventory.DataSource = bd;
            }

            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void CreateInventoryDataGridColumns(SourceGrid.DataGridColumns columns, DevAge.ComponentModel.IBoundList boundList)
        {
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            viewColumnHeader.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            SourceGrid.Cells.Views.Cell cellView = new SourceGrid.Cells.Views.Cell();
            cellView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

            SourceGrid.Cells.Views.Cell cellNewView = new SourceGrid.Cells.Views.Cell();
            cellNewView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

            SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            viewNumeric.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

            SourceGrid.DataGridColumn gridColumn;

            gridColumn = dgInventory.Columns.Add("productID", "productID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.Visible = false;

            gridColumn = dgInventory.Columns.Add("ProductName", "ProductName", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 200;

            //gridColumn.DataCell.Editor =  
            gridColumn = dgInventory.Columns.Add("ClosingStock", "Closing Qty", new SourceGrid.Cells.DataGrid.Cell()); //new SourceGrid.Cells.Editors.TextBox(typeof(decimal)));
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 90;

            gridColumn = dgInventory.Columns.Add("ClosingStock", "Opening Qty", new SourceGrid.Cells.Editors.TextBox(typeof(decimal))); // textbox is used to allow editing of the column value
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = cellView;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            gridColumn.Width = 50;

            gridColumn = dgInventory.Columns.Add("OpenPurchaseRate", "Purchase Rate", new SourceGrid.Cells.Editors.TextBox(typeof(decimal)));
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            gridColumn = dgInventory.Columns.Add("SalesRate", "Sales Rate", new SourceGrid.Cells.Editors.TextBox(typeof(decimal)));
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            gridColumn.Width = 70;

            dgLedger.AutoStretchColumnsToFitWidth = true;

        }
        
        #endregion

        private void ListAccountClass(ComboBox ComboBoxControl)
        {
            BusinessLogic.FiscalYearClosing fyc = new FiscalYearClosing();
            //ComboBoxControl.Items.Clear();
            DataTable dt = fyc.GetAccountClasses();
            ComboBoxControl.Items.Add(new ListItem((0), "None"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["AccClassid"], dr["EngName"].ToString()));
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }
                                                                                 
        #region old code
        private void cboAccountClass_SelectedIndexChanged(object sender, EventArgs e)
        {

            //liAccClass = (ListItem)cboAccountClass.SelectedItem;
            //int AccClassID = liAccClass.ID;
            //if(AccClassID!=0)
            //{
            //    ShowClosingBalance(AccClassID);
            //}

        }

        private void ShowClosingBalance(int id, string AccountClassName)
        {
            AccClassIDs.Clear();
            AccClassIDs.Add(id.ToString());
            ArrayList arrchildAccClassIds = new ArrayList();
            //AccountClass.GetChildIDs(1, ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
            AccountClass.GetChildIDs(id, ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
            foreach (object obj in arrchildAccClassIds)
            {
                int i = (int)obj;
                AccClassIDs.Add(i.ToString());
            }

            DataTable dtledgerinfo = new DataTable();
            //For List OF Ledgers Under Assests
            int Assest = AccountGroup.GetGroupIDFromGroupNumber(1);
            dtAssestsLedgers = Ledger.GetAllLedger(Assest);
            for (int a = 0; a < dtAssestsLedgers.Rows.Count; a++)
            {
                DataRow drassest = dtAssestsLedgers.Rows[a];
                double m_dbal = 0;
                double m_cbal = 0;
                Transaction.GetLedgerBalance(null, null, Convert.ToInt32(drassest["LedgerID"].ToString()), ref m_dbal, ref m_cbal, AccClassIDs, 1);
                double Balance = m_dbal - m_cbal;
                if (Balance >= 0)
                {
                    dtFinalResult.Rows.Add(drassest["LedgerID"].ToString(), drassest["EngName"].ToString(), Balance, "DEBIT", id);
                }
                else if (Balance < 0)
                {
                    dtFinalResult.Rows.Add(drassest["LedgerID"].ToString(), drassest["EngName"].ToString(), Balance * -1, "CREDIT", id);
                }

                //dtledgerinfo = Ledger.GetLedgerDetail(id.ToString(), null, null, Convert.ToInt32(drassest["LedgerID"].ToString()));
                //if (dtledgerinfo.Rows.Count > 0)
                //{
                //    DataRow dr = dtledgerinfo.Rows[0];
                //    if (dr["Debit"] == DBNull.Value || Convert.ToInt32(dr["Debit"]) == 0)
                //    {
                //        if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                //        {
                //            closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //            closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //            DrCr = "Debit";
                //        }
                //        else
                //        {
                //            //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                //            //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                //            closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //            closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //            DrCr = "Credit";
                //        }
                //    }
                //    else
                //    {
                //        if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                //        {
                //            //closingamount = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                //            //closingamount = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                //            closingamount = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //            closingamount = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //            DrCr = "Debit";
                //        }
                //        else
                //        {
                //            if (Convert.ToDecimal(dr["Debit"]) == Convert.ToDecimal(dr["Credit"]))
                //            {
                //                closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //                closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //                DrCr = "Debit";
                //            }

                //            if (Convert.ToDecimal(dr["Debit"]) > Convert.ToDecimal(dr["Credit"]))
                //            {
                //                //closingamount = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                //                //closingamount = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                //                closingamount = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //                closingamount = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //                DrCr = "Debit";
                //            }
                //            if (Convert.ToDecimal(dr["Debit"]) < Convert.ToDecimal(dr["Credit"]))
                //            {
                //                //closingamount = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                //                //closingamount = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                //                closingamount = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //                closingamount = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //                DrCr = "Credit";
                //            }
                //        }

                //    }
                //    dtFinalResult.Rows.Add(drassest["LedgerID"].ToString(), drassest["EngName"].ToString(), closingamount,DrCr);
                //}
                //else
                //{

                //       closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //       closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //       DrCr = "Debit";
                //       // FilllblCurrentBankBalance();
                //       dtFinalResult.Rows.Add(drassest["LedgerID"].ToString(), drassest["EngName"].ToString(), closingamount,DrCr);
                //        //return;

                //}
            }
            //For List OF Ledgers Under Liabilities
            int Liabilities = AccountGroup.GetGroupIDFromGroupNumber(2);
            dtLiabilitiesLedgers = Ledger.GetAllLedger(Liabilities);
            for (int l = 0; l < dtLiabilitiesLedgers.Rows.Count; l++)
            {

                DataRow drliabilities = dtLiabilitiesLedgers.Rows[l];
                double m_dbal = 0;
                double m_cbal = 0;
                Transaction.GetLedgerBalance(null, null, Convert.ToInt32(drliabilities["LedgerID"].ToString()), ref m_dbal, ref m_cbal, AccClassIDs, 1);
                double Balance = m_dbal - m_cbal;
                if (Balance >= 0)
                {
                    dtFinalResult.Rows.Add(drliabilities["LedgerID"].ToString(), drliabilities["EngName"].ToString(), Balance, "DEBIT", id);
                }
                else if (Balance < 0)
                {
                    dtFinalResult.Rows.Add(drliabilities["LedgerID"].ToString(), drliabilities["EngName"].ToString(), Balance * -1, "CREDIT", id);
                }
                //closingamount = "";
                //DataRow drliabilities = dtLiabilitiesLedgers.Rows[l];
                //dtledgerinfo = Ledger.GetLedgerDetail(id.ToString(), null, null, Convert.ToInt32(drliabilities["LedgerID"].ToString()));
                //if (dtledgerinfo.Rows.Count > 0)
                //{
                //    DataRow dr = dtledgerinfo.Rows[0];
                //    if (dr["Debit"] == DBNull.Value || Convert.ToInt32(dr["Debit"]) == 0)
                //    {
                //        if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                //        {
                //            closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //            closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //            DrCr = "Debit";
                //        }
                //        else
                //        {
                //            //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                //            //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                //            closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //            closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //            DrCr = "Credit";
                //        }
                //    }
                //    else
                //    {
                //        if (dr["Credit"] == DBNull.Value || Convert.ToInt32(dr["Credit"]) == 0)
                //        {
                //            //closingamount = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                //            //closingamount = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                //            closingamount = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //            closingamount = Convert.ToDecimal(dr["Debit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //            DrCr = "Debit";
                //        }
                //        else
                //        {
                //            if (Convert.ToDecimal(dr["Debit"]) == Convert.ToDecimal(dr["Credit"]))
                //            {
                //                closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //                closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //                DrCr = "Debit";
                //            }

                //            if (Convert.ToDecimal(dr["Debit"]) > Convert.ToDecimal(dr["Credit"]))
                //            {
                //                //closingamount = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                //                //closingamount = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Dr.)";
                //                closingamount = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //                closingamount = (Convert.ToDecimal(dr["Debit"]) - Convert.ToDecimal(dr["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //                DrCr = "Debit";
                //            }
                //            if (Convert.ToDecimal(dr["Debit"]) < Convert.ToDecimal(dr["Credit"]))
                //            {
                //                //closingamount = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                //                //closingamount = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "(Cr.)";
                //                closingamount = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //                closingamount = (Convert.ToDecimal(dr["Credit"]) - Convert.ToDecimal(dr["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) ;
                //                DrCr = "Credit";
                //            }
                //        }

                //    }
                //    dtFinalResult.Rows.Add(drliabilities["LedgerID"].ToString(), drliabilities["EngName"].ToString(), closingamount,DrCr);
                //}
                //else
                //{
                //    closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //    closingamount = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //    DrCr = "Debit";
                //    // FilllblCurrentBankBalance();
                //    dtFinalResult.Rows.Add(drliabilities["LedgerID"].ToString(), drliabilities["EngName"].ToString(), closingamount,DrCr);
                //}
            }

            //int count = dtFinalResult.Rows.Count;
            rows = rows + dtAssestsLedgers.Rows.Count;
            rows = rows + dtLiabilitiesLedgers.Rows.Count;
            count = rows;

            //FillGrid(dtFinalResult,AccountClassName);
        }
        
        #endregion

        private void btnadjustdepretiation_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            Global.fclose = true;
            frmdepreciation fdep = new frmdepreciation(this);
            fdep.ShowDialog();
            //dtFinalResult.Columns.Remove("LedgerID");
            //dtFinalResult.Columns.Remove("LedgerName");
            //dtFinalResult.Columns.Remove("DrCr");
            //dtFinalResult.Columns.Remove("Amount");
            //frmfiscalyearclosing_Load(sender, e);
            CalculateDepreciation();
            
        }

        DataTable dtSaveDepreciation = new DataTable();
        public void GetDepreciation(DataTable dtsavedepreciation)
        {
            this.dtSaveDepreciation = dtsavedepreciation;
        }

        public void CalculateDepreciation()
        {
            try
            {
                for (int i = 1; i < dgLedger.Rows.Count; i++)
                {
                    string ledID = new SourceGrid.CellContext(dgLedger, new SourceGrid.Position(i, 0)).Value.ToString();
                    //bool contains = dtSaveDepreciation.AsEnumerable().Any(row => ledID == row.Field<String>("LedgerID"));
                    object strDepreciation = dtSaveDepreciation.Compute("max(DepreciationValue)", "LedgerID = " + ledID + "");
                    if (strDepreciation != null && strDepreciation.ToString() != "")
                    {
                        string closingAmt = new SourceGrid.CellContext(dgLedger, new SourceGrid.Position(i, 3)).Value.ToString();

                        decimal deprAmt = (1 - Convert.ToDecimal(strDepreciation) / 100) * Convert.ToDecimal(closingAmt);

                        dtledgerDetails.Rows[i - 1]["FinalBal"] = deprAmt;
                    }

                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void frmfiscalyearclosing_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.fclose = false;
        }

        private void txtstartdate_Leave(object sender, EventArgs e)
        {
            //string z = txtstartdate.Text;
            if (txtStartDate.Text == "    /  /")
            {

            }
            else
            {
                DataTable dttransactdate = new DataTable();
                FiscalYearClosing fclosing = new FiscalYearClosing();
                dttransactdate = fclosing.GetTransactDateInfo();
                for (int i = 0; i < dttransactdate.Rows.Count; i++)
                {
                    DataRow drtransactdate = dttransactdate.Rows[i];
                    if (Convert.ToDateTime(drtransactdate["TransactDate"].ToString()) >= Convert.ToDateTime(txtStartDate.Text))
                    {
                        Global.Msg("There is a Voucher Which Includes in New Fiscal Year Date Plz Check Transaction");
                        txtStartDate.Focus();
                        return;
                    }
                }
            }
        }

        private void btnfiscalyearclose_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogresult = MessageBox.Show("Ary You Sure You Want To Close Fiscal Year?", "Fiscal Year Close", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogresult == DialogResult.No)
                {
                    return;
                }
                if (txtStartDate.Text == " ")
                {
                    MessageBox.Show("Enter Fiscal Year Start Date");
                    return;
                }
                else if (txtEndDate.Text == " ")
                {
                    MessageBox.Show("Enter Book Begining Date");
                    return;
                }
                Settings m_Settings = new Settings();
                Global.PreviousYearDb = RegManager.DataBase;

                frmProgress ProgressForm = new frmProgress();
                // Initialize the thread that will handle the background process
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        ProgressForm.ShowDialog();
                    }
                ));

                backgroundThread.Start();
                ProgressForm.UpdateProgress(20, "Fiscal Year Closing Progress...");

                /// <summary>
                ///Run our validation rules such as dbname, fiscal year etc.
                /// </summary> 
                /// while determining dbname you must have to check if dbname already exist or not.
                /// validation 1 -- Check if dbname exist or not?  
                /// 
                DataTable CompInfo = FiscalYearClosing.GetCompanyDetails();
                int i = CompInfo.Rows.Count + 1;
                DataRow drcompinfo = CompInfo.Rows[0];
                txtDBName.Text = drcompinfo["CompanyCode"].ToString();
                //prevDBName = drcompinfo["DBName"].ToString();
                prevDBName = Global.m_db.cn.Database.ToString();
                string TestDBName = txtDBName.Text + i.ToString().PadLeft(3, '0');
                while (CreateDB.IsDBExist(TestDBName))
                {
                    i++;
                    TestDBName = txtDBName.Text + i.ToString().PadLeft(3, '0');
                }

                newDBName = TestDBName;
                txtDBName.Text = TestDBName;
                Global.NextYearDb = TestDBName;
                Code = "NEXT_YR_DB";
                Value = Global.NextYearDb;
                m_Settings.SetSettings(Code, Value);

                //Create DB here 
                #region Database Creation With Proper Location
                //for this we have to fix the database directory
                string appPath = null;
                System.IO.DirectoryInfo directoryInfo = null;
                System.IO.DirectoryInfo directoryInfo1 = null;
                string path = null;
                try
                {
                    appPath = Path.GetDirectoryName(Application.ExecutablePath);
                    directoryInfo = System.IO.Directory.GetParent(appPath);
                    directoryInfo1 = System.IO.Directory.GetParent(directoryInfo.FullName);
                    path = directoryInfo1.FullName + @"\Database";
                }
                catch (Exception)
                {
                    
                   path = appPath+ @"\Database";
                }
                //to create directory
                if (Directory.Exists(path))
                {
                    //Do nothing
                }
                else
                {
                    Directory.CreateDirectory(path);
                }

                DatabaseParam DBParam = new DatabaseParam();
                // DBParam.ServerName = "";
                DBParam.DatabaseName = txtDBName.Text;
                DBParam.DataFileGrowth = "4";
                DBParam.DataFileName = txtDBName.Text + "_Data";
                DBParam.DataFileSize = "2";//2MB at the init state
                DBParam.DataPathName = path + "\\" + DBParam.DataFileName + ".mdf";
                DBParam.LogFileGrowth = "4";
                DBParam.LogFileName = txtDBName.Text + "_Log";
                DBParam.LogFileSize = "1";//1MB at the init state
                DBParam.LogPathName = path + "\\" + DBParam.LogFileName + ".ldf";
                CreateDB.CreateDatabase(DBParam);

                #endregion

                ProgressForm.UpdateProgress(40, "Fiscal Year Closing Progress...");

                // After successful creationof database attach our database to that db
                #region Restore .bak file to temporary database
                if (!BackUpRestore.RestoreDatabase(DBParam.DatabaseName, path + "\\Mydb.Bak", DBParam.DataPathName, DBParam.LogPathName))
                {
                    Global.MsgError("Could not restore the database");
                    return;
                }

                #endregion

                ProgressForm.UpdateProgress(80, "Fiscal Year Closing Progress...");


                //Now change the  connection string to newly connected db
                #region Switch Connection String To New Database
                RegManager.DataBase = DBParam.DatabaseName;
                //refresh the whole application           
                SqlDb m_db = new SqlDb();
                //In case of SQL Server connection
                m_db.ServerName = RegManager.ServerName;
                m_db.DbName = RegManager.DataBase;
                m_db.UserName = RegManager.DBUser;
                m_db.Password = RegManager.DBPassword;

                if (m_db.Connect())
                {
                    RegManager.ServerName = m_db.ServerName;
                    RegManager.DataBase = m_db.DbName;
                    RegManager.DBUser = m_db.UserName;
                    RegManager.DBPassword = m_db.Password;
                    Global.m_db.cn = m_db.cn;
                    Global.ConnectionString = Global.m_db.cn.ConnectionString;
                }
                else
                {
                    MessageBox.Show("Connection to new Fiscal Year failed!!");
                    return;
                }
                #endregion

                ///<Summary>
                ///Also insert the general setting in the setting table.
                ///</Summary>
                ///paste latest modify setting from setting form
                ModifySettings();
                FiscalYearClosing.FiscalYearClose(prevDBName, newDBName);

                //Now Insert the Company information
                #region Write Company in Database
                //Entry for fiscal year
                try
                {
                    //frmCompanyFiscalYear frmFY = new frmCompanyFiscalYear(this, CompDetails);
                    //if (!frmFY.IsDisposed)
                    //{
                    //    frmFY.ShowDialog();
                    //    frmFY.BringToFront();
                    //    frmFY.Focus();
                    //}

                    CompDetails.CompanyName = drcompinfo["Name"].ToString();
                    CompDetails.CompanyCode = drcompinfo["CompanyCode"].ToString();
                    CompDetails.Address1 = drcompinfo["Address1"].ToString();
                    CompDetails.Address2 = drcompinfo["Address2"].ToString();
                    CompDetails.City = drcompinfo["City"].ToString();
                    CompDetails.District = drcompinfo["District"].ToString();
                    CompDetails.Email = drcompinfo["Email"].ToString();
                    CompDetails.PAN = drcompinfo["PAN"].ToString();
                    CompDetails.POBox = drcompinfo["POBox"].ToString();
                    CompDetails.Telephone = drcompinfo["Telephone"].ToString();
                    CompDetails.Website = drcompinfo["Website"].ToString();
                    CompDetails.Zone = drcompinfo["Zone"].ToString();

                    CompDetails.FYFrom = Date.ToDotNet(txtStartDate.Text); // Date.ToDotNet(txtDateFY.Text);
                    CompDetails.BookBeginFrom = Date.ToDotNet(txtEndDate.Text); // Date.ToDotNet(txtDateBookBegin.Text);
                    ///<Summary>
                    ///We have to predetermine the database name from above information and create it then insert the company information.
                    ///</Summary>                
                    CompDetails.DBName = txtDBName.Text;

                    //if (drcompinfo["Logo"].ToString() != null)
                    //{
                    //    byte[] logo =Misc.ReadBitmap2ByteArray(drcompinfo["Logo"].ToString());
                    //    CompDetails.Logo = logo;
                    //}
                    string Return = "";

                    CompanyInfo CompInfo1 = new CompanyInfo();
                    Return = CompInfo1.Update(CompDetails);
                    InsertOpeningBalance();
                    InsertOpeningQuantity();
                    //FiscalYearClosing.AddUsersToNewDB(prevDBName, newDBName); // insert username and other details of user into new database
                    string PreviousYearDb = Global.PreviousYearDb;
                    Code = "PREV_YR_DB";
                    Value = PreviousYearDb;
                    m_Settings.SetSettings(Code, Value);
                #endregion

                    ProgressForm.UpdateProgress(90, "Fiscal Year Closing progress...");

                    ///<Summary>
                    ///Write on the registry
                    ///</Summary>
                    #region Write on Registry

                    //string NewCompany = RegManager.CreateNewCompany();
                    string NewCompany = Global.Company_RegCode;
                    string NewFY = RegManager.CreateNewFY(NewCompany);
                    object RegValue = new object();
                    TextBox txtCompanyCode = new TextBox();
                    TextBox txtCompanyName = new TextBox();
                    txtCompanyCode.Text = drcompinfo["CompanyCode"].ToString();
                    txtCompanyName.Text = drcompinfo["Name"].ToString();
                    RegValue = txtCompanyCode.Text.Trim().ToUpper();
                    RegManager.Write(NewCompany, "CODE", RegValue);
                    RegValue = DBParam.DatabaseName;
                    RegManager.Write(NewCompany, "DATABASE", RegValue);
                    RegValue = txtCompanyName.Text.Trim();
                    RegManager.Write(NewCompany, "NAME", RegValue);


                    RegValue = DBParam.DatabaseName;
                    RegManager.Write(NewCompany + "\\" + NewFY, "DATABASE", RegValue);
                    RegValue = "30";
                    RegManager.Write(NewCompany + "\\" + NewFY, "DAY", RegValue);
                    RegValue = "04";
                    RegManager.Write(NewCompany + "\\" + NewFY, "MONTH", RegValue);
                    RegValue = "2013";
                    RegManager.Write(NewCompany + "\\" + NewFY, "YEAR", RegValue);

                    #endregion


                    ProgressForm.UpdateProgress(100, "Fiscal Year Closing progress...");
                    // Close the dialog if it hasn't been already
                    if (ProgressForm.InvokeRequired)
                        ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

                    if (Return == "SUCCESS")
                        Global.Msg("Fiscal Year Closing Successfully!");
                    else
                        Global.MsgError("Some Unknown Error Occured!");
                    this.Dispose();

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
        private void ModifySettings()
        {
            Settings m_Settings = new Settings();
            string Code, Value;

            //GMOptions Settings
            #region GMOptions Setting
           
                Code = "DEFAULT_DATE";
                Value = "Nepali";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Date = Date.DateType.Nepali;
          
                Code = "DEFAULT_DECIMALPLACES";
                Value = 2.ToString();
                m_Settings.SetSettings(Code, Value);
               
                Code = "COMMA_SEPARATED";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Comma_Separated = true;
           
           
                Code = "DECIMAL_FORMAT";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Decimal_Format = "0";
           
            
                Code = "DEFAULT_LANGUAGE";
                Value = "English";
                m_Settings.SetSettings(Code, Value);
              
            #endregion


                // to store profit of prev yr to new yr database
                Code = "PREV_YR_PROFIT";
                Value = lblProfit.Text;
                m_Settings.SetSettings(Code, Value);

        }
        public void FiscalYearCalculate(CompanyDetails CompDetail)
        {
            CompDetails = CompDetail;
        }
        private void InsertOpeningBalance()
        {
            try
            {
                try
                {
                    //FiscalYearClosing.AddLedgerToNewDB(prevDBName, newDBName);
                    DataTable dtopeningbalance = new DataTable();
                    dtopeningbalance.Columns.Add("LedgerID");
                    dtopeningbalance.Columns.Add("AccClassID");
                    dtopeningbalance.Columns.Add("Opening Balance");
                    dtopeningbalance.Columns.Add("DrCr");
                    // dtopeningbalance.Columns.Add("AccID");
                    //for (int f = 2; f < grdfiscalyear.Rows.Count - 3; f++)
                    //{
                    //    string AccName = grdfiscalyear[f, 1].Value.ToString();
                    //    int ledgerid = Ledger.GetLedgerIdFromName(AccName, Lang.English);
                    //    if (ledgerid > 0)
                    //    {
                    //        dtopeningbalance.Rows.Add(grdfiscalyear[f, 0].Value.ToString(), grdfiscalyear[f, 5].Value.ToString(), grdfiscalyear[f, 4].Value.ToString(), grdfiscalyear[f, 2].Value.ToString());
                    //    }
                    //}
                    string accClassID = cboAccountClass.SelectedValue.ToString();
                    for (int f = 1; f < dgLedger.Rows.Count; f++)
                    {
                        string AccName = new SourceGrid.CellContext(dgLedger, new SourceGrid.Position(f, 1)).Value.ToString();
                        int ledgerid = Ledger.GetLedgerIdFromName(AccName, Lang.English);
                        if (ledgerid > 0)
                        {
                            string ledID = new SourceGrid.CellContext(dgLedger, new SourceGrid.Position(f, 0)).Value.ToString();
                            //string accClassID = "1";//new SourceGrid.CellContext(dgLedger, new SourceGrid.Position(f, 5)).Value.ToString();
                            string openBal = new SourceGrid.CellContext(dgLedger, new SourceGrid.Position(f, 3)).Value.ToString();
                            string drCr = new SourceGrid.CellContext(dgLedger, new SourceGrid.Position(f, 4)).Value.ToString();
                            dtopeningbalance.Rows.Add(ledID, accClassID, openBal, drCr);
                        }
                    }
                    CompanyInfo.InsertOpeningBalance(dtopeningbalance);
                    //for (int i = 0; i < dtopeningbalance.Rows.Count;i++ )
                    //{
                    //    DataRow dr=dtopeningbalance.Rows[i];
                    //   // DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                    //    DateTime Created_Date =System.Convert.ToDateTime( Date.GetServerDate());
                    //    Global.m_db.InsertUpdateQry("insert into Acc.tblOpeningBalance(LedgerID,AccClassID,OpenBal,OpenBalDrCr,OpenBalDate,OpenBalCCYID) values('" + dr["LedgerID"].ToString() + "','" + dr["AccClassID"].ToString() + "','" + dr["Opening Balance"].ToString() + "','" + dr["DrCr"].ToString() + "','"+Created_Date+"','"+1+"')");
                    //}

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

        #region old code

        private void ShowClosingQuantity(int id, string AccountClassName)
        {
            string AccClassIDsXMLString = ReadAllAccClassID(id);
            string ProjectIDsXMLString = ReadAllProjectID();
            DateTime TodaysDate =Convert.ToDateTime( Date.GetServerDate());
            DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, null, "", TodaysDate.AddDays(1), true, StockStatusType.OpeningStock, AccClassIDsXMLString);
            DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, null, "", TodaysDate, false, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
            if (dtTransactionStockStatusInfo.Rows.Count != 0)
            {
                
                foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
                {
                    bool isZero = true;
                    foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
                    {
                        if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
                        {
                            isZero = false;
                            int closingQuantity = Convert.ToInt32(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
                            dtInventoryStock.Rows.Add(drOpeningStockStatusInfo["ProductID"].ToString(), drOpeningStockStatusInfo["ProductName"].ToString(), id.ToString(), closingQuantity.ToString(), drOpeningStockStatusInfo["OpenPurchaseRate"].ToString(), drOpeningStockStatusInfo["OpenSalesRate"].ToString());
                            break;
                        }
                       
                    }
                    if (isZero == true)
                    {
                        dtInventoryStock.Rows.Add(drOpeningStockStatusInfo["ProductID"].ToString(), drOpeningStockStatusInfo["ProductName"].ToString(), id.ToString(), drOpeningStockStatusInfo["Quantity"].ToString(), drOpeningStockStatusInfo["OpenPurchaseRate"].ToString(), drOpeningStockStatusInfo["OpenSalesRate"].ToString());
                    }
                }
               
            }
            else
            {
                DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, null, "", TodaysDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo1.Rows)
                {
                    dtInventoryStock.Rows.Add(drOpeningStockStatusInfo["ProductID"].ToString(), drOpeningStockStatusInfo["ProductName"].ToString(), id.ToString(), drOpeningStockStatusInfo["Quantity"].ToString(), drOpeningStockStatusInfo["OpenPurchaseRate"].ToString(), drOpeningStockStatusInfo["OpenSalesRate"].ToString());
                }
            }

            //FillInventoryGrid(dtInventoryStock, AccountClassName);
        }

        private void FillInventoryGrid(DataTable dt, string AccountClassName)
        {
            try
            {
                grdinventoryfiscalyear.Redim(countinv + 9, 8);
                AddGridHeaderInventory();
                SourceGrid.Cells.Views.Cell header = new SourceGrid.Cells.Views.Cell();
                header.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Cornsilk);
                header.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                header.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                header.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                header.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);

                SourceGrid.Cells.Editors.TextBox root = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                root.EditableMode = SourceGrid.EditableMode.None;
                grdinventoryfiscalyear[snoinv, 1] = new SourceGrid.Cells.Cell("", root);
                grdinventoryfiscalyear[snoinv, 1].Value = AccountClassName;
                grdinventoryfiscalyear[snoinv, 1].ColumnSpan = grdinventoryfiscalyear.ColumnsCount - 2;
                grdinventoryfiscalyear[snoinv, 1].View = new SourceGrid.Cells.Views.Cell(header);
                // grdTransaction[TransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                for (int i = 1; i <= dtInventoryStock.Rows.Count; i++)
                {
                    DataRow dr = dtInventoryStock.Rows[i - 1];
                    //grdfiscalyear.Rows.Insert(i);
                    SourceGrid.Cells.Editors.TextBox ProductID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    ProductID.EditableMode = SourceGrid.EditableMode.None;
                    grdinventoryfiscalyear[snoinsertinv, 0] = new SourceGrid.Cells.Cell("", ProductID);
                    grdinventoryfiscalyear[snoinsertinv, 0].Value = dr["ProductID"].ToString();

                    SourceGrid.Cells.Editors.TextBox ProductName = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    ProductName.EditableMode = SourceGrid.EditableMode.None;
                    grdinventoryfiscalyear[snoinsertinv, 1] = new SourceGrid.Cells.Cell("", ProductName);
                    grdinventoryfiscalyear[snoinsertinv, 1].Value = dr["ProductName"].ToString();

                    SourceGrid.Cells.Editors.TextBox ClosingQuantity = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    ClosingQuantity.EditableMode = SourceGrid.EditableMode.None;
                    grdinventoryfiscalyear[snoinsertinv, 2] = new SourceGrid.Cells.Cell("", ClosingQuantity);
                    grdinventoryfiscalyear[snoinsertinv, 2].Value = dr["OpenPurchaseQty"].ToString();

                    SourceGrid.Cells.Editors.TextBox OpeningQuantity = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    OpeningQuantity.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                    grdinventoryfiscalyear[snoinsertinv, 3] = new SourceGrid.Cells.Cell("", OpeningQuantity);
                    grdinventoryfiscalyear[snoinsertinv, 3].Value = dr["OpenPurchaseQty"].ToString();


                    SourceGrid.Cells.Editors.TextBox OpenPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    OpenPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                    grdinventoryfiscalyear[snoinsertinv, 4] = new SourceGrid.Cells.Cell("", OpenPurchaseRate);
                    grdinventoryfiscalyear[snoinsertinv, 4].Value = dr["OpenPurchaseRate"].ToString();

                    SourceGrid.Cells.Editors.TextBox OpenSalesRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    OpenSalesRate.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                    grdinventoryfiscalyear[snoinsertinv, 5] = new SourceGrid.Cells.Cell("", OpenSalesRate);
                    grdinventoryfiscalyear[snoinsertinv, 5].Value = dr["OpenSalesRate"].ToString();

                    SourceGrid.Cells.Editors.TextBox AccID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    AccID.EditableMode = SourceGrid.EditableMode.None;
                    grdinventoryfiscalyear[snoinsertinv, 6] = new SourceGrid.Cells.Cell("", AccID);
                    grdinventoryfiscalyear[snoinsertinv, 6].Value = dr["AccClassID"].ToString();


                    snoinsertinv += 1;
                    countinv = countinv + 1;
                }
                snoinsertinv += 1;
                snoinv += dtInventoryStock.Rows.Count + 1;
                dtInventoryStock.Rows.Clear();
            }

            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        } 
        #endregion


        private string ReadAllAccClassID(int id)
        {
            #region  AccountingClassID
            AccClassIDs.Clear();
            AccClassIDs.Add(id.ToString());
            ArrayList arrchildAccClassIds = new ArrayList();
            AccountClass.GetChildIDs(id, ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
            foreach (object obj in arrchildAccClassIds)
            {
                int i = (int)obj;
                AccClassIDs.Add(i.ToString());
            }
            //AccClassIDs.AddRange(arrchildAccClassIds);

            //ArrayList arrChildAccClassIDs = new ArrayList();
            //foreach (object j in m_StockStatus.AccClassID)
            //{
            //    AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            //}
            //m_StockStatus.AccClassID.AddRange(arrChildAccClassIDs);

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
                    tw.WriteStartElement("ACCCLASSIDS");
                    foreach (string tag in AccClassIDs)
                    {
                       // AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccID", Convert.ToInt32(tag).ToString());
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
            Project.GetChildProjects(1, ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" +1+ "'";

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
                    tw.WriteStartElement("PROJECTIDS");
                    tw.WriteElementString("PID", Convert.ToInt32(1).ToString());
                    foreach (string tag in ProjectIDCollection)
                    {
                        //AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("PID", Convert.ToInt32(tag).ToString());
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

        private void InsertOpeningQuantity()
        {
            try
            {
                //FiscalYearClosing.AddProductToNewDB(prevDBName, newDBName);
                DataTable dtopeningqty = new DataTable();
                dtopeningqty.Columns.Add("ProductID");
                dtopeningqty.Columns.Add("OpeningPurchaseQty");
                dtopeningqty.Columns.Add("OpeningPurchaseRate");
                dtopeningqty.Columns.Add("OpeningSalesRate");
                dtopeningqty.Columns.Add("AccClassID");
                // dtopeningbalance.Columns.Add("AccID");
                Product p = new Product();
                //for (int f = 2; f < grdinventoryfiscalyear.Rows.Count - 3; f++)
                //{
                //    string ProductName = grdinventoryfiscalyear[f, 1].Value.ToString();
                //    int ProductID = p.GetProductIdFromName(ProductName, Lang.English);
                //    if (ProductID > 0)
                //    {
                //        dtopeningqty.Rows.Add(grdinventoryfiscalyear[f, 0].Value.ToString(), grdinventoryfiscalyear[f, 3].Value.ToString(), grdinventoryfiscalyear[f, 4].Value.ToString(), grdinventoryfiscalyear[f, 5].Value.ToString(), grdinventoryfiscalyear[f, 6].Value.ToString());
                //    }
                //}
                string accClassID = cboAccountClass.SelectedValue.ToString();
                for (int f = 1; f < dgInventory.Rows.Count; f++)
                {
                    string ProductName = new SourceGrid.CellContext(dgInventory, new SourceGrid.Position(f, 1)).Value.ToString();
                    int ProductID = p.GetProductIdFromName(ProductName, Lang.English);
                    if (ProductID > 0)
                    {
                        string productID = new SourceGrid.CellContext(dgInventory, new SourceGrid.Position(f, 0)).Value.ToString();
                        string openPurchQty = new SourceGrid.CellContext(dgInventory, new SourceGrid.Position(f, 3)).Value.ToString();
                        string openPurchRate = new SourceGrid.CellContext(dgInventory, new SourceGrid.Position(f, 4)).Value.ToString();
                        string openSalesRate = new SourceGrid.CellContext(dgInventory, new SourceGrid.Position(f, 5)).Value.ToString();
                        //string accClassID = "1";//new SourceGrid.CellContext(dgInventory, new SourceGrid.Position(f, 6)).Value.ToString();

                        dtopeningqty.Rows.Add(productID, openPurchQty, openPurchRate, openSalesRate, accClassID);
                    }
                }
                CompanyInfo.InsertOpeningQty(dtopeningqty);
                //for (int i = 0; i < dtopeningbalance.Rows.Count;i++ )
                //{
                //    DataRow dr=dtopeningbalance.Rows[i];
                //   // DateTime Created_Date = System.Convert.ToDateTime(DateTime.Now);
                //    DateTime Created_Date =System.Convert.ToDateTime( Date.GetServerDate());
                //    Global.m_db.InsertUpdateQry("insert into Acc.tblOpeningBalance(LedgerID,AccClassID,OpenBal,OpenBalDrCr,OpenBalDate,OpenBalCCYID) values('" + dr["LedgerID"].ToString() + "','" + dr["AccClassID"].ToString() + "','" + dr["Opening Balance"].ToString() + "','" + dr["DrCr"].ToString() + "','"+Created_Date+"','"+1+"')");
                //}
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            frmfiscalyearclosing_Load(null, null);
        }
    }
}
