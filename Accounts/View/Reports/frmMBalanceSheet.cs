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
using Inventory.DataSet;
using Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DateManager;
using System.Threading;



namespace Accounts.Reports
{
    public partial class frmMBalanceSheet : Form
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private bool isMax = true;
        int liabilitiesrowcount = 0;
        int assesstsrowcount = 0;
        private BalanceSheetSettings m_BS;
        private int rowCount = 0;
        private int CheckAddRows = 0;
        private int rowInsert = 0;
        private int sNo = 0;
        DataTable dtLiab;
        private Accounts.Model.dsBalanceSheet dsBalanceSheet = new Model.dsBalanceSheet();
        private int prntDirect = 0;
        private string FileName = "";
        //Different grid views
        private SourceGrid.Cells.Views.Cell HeaderView;
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell SubGroupView;
        private SourceGrid.Cells.Views.Cell DivisionView;
        private SourceGrid.Cells.Views.Cell AmountView;
        ArrayList AccClassID = new ArrayList();
        double AdditionalExpenses = 0;
        //For Export Menu
        ContextMenu Menu_Export;

        public frmMBalanceSheet(BalanceSheetSettings BS)
        {
            try
            {
                #region BLOCK FOR INITIALIZING THE CONSTRUCTOR PARAMETER
                // Initilizing the constructor's parameter
                InitializeComponent();
                m_BS = new BalanceSheetSettings();//dynamic memory allocation of object of class  BalanceSheetSettings object
                m_BS.FromDate = BS.FromDate;
                m_BS.ToDate = BS.ToDate;
                m_BS.AccClassID = BS.AccClassID;
                m_BS.HasDateRange = BS.HasDateRange;
                m_BS.ShowZeroBalance = BS.ShowZeroBalance;
                m_BS.ShowSecondLevelDtl = BS.ShowSecondLevelDtl;
                m_BS.ProjectID = BS.ProjectID;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmMBalanceSheet_Load(object sender, EventArgs e)
        {
            DisplayBannar();
            GetTransactionData(false);

        }
        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            //lblCompanyName.Text = m_CompanyDetails.CompanyName;
            //lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            //lblContact.Text = "Contact: " + m_CompanyDetails.Telephone;
            //lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;
            //lblWebsite.Text = "Web: " + m_CompanyDetails.Website;

            if (m_BS.HasDateRange)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_BS.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                //lblAllSettings.Text = "Fiscal Year: " +Date.ToSystem(m_CmpDtl.FYFrom);
                lblAllSettings.Text = "Fiscal Year: " +m_CmpDtl.FiscalYear;

            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_BS.ProjectID), LangMgr.DefaultLanguage);
            if (m_BS.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];
                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
        }
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 10, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.TopLeft;
                View = view;

                AutomaticSortEnabled = false;
            }

        }

        public void FillGrid(DataTable dtBalanceSheet)
        {
            try
            {
                grdBalanceSheet.Rows.Clear();
                grdBalanceSheet.Redim(dtBalanceSheet.Rows.Count+ 1, 4);
                WriteHeaderShareHolder();

                for (int i = 1; i <= dtBalanceSheet.Rows.Count; i++)
                {

                    DataRow dr = dtBalanceSheet.Rows[rowCount];

                    grdBalanceSheet[rowCount+1, 0] = new SourceGrid.Cells.Cell(dr["LedgerName"].ToString());
                    grdBalanceSheet[rowCount+1, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                    if (Convert.ToDouble(dr["DebitBalance"].ToString()) > 0)
                    {
                        string DebitAmount = dr["DebitBalance"].ToString();
                        grdBalanceSheet[rowCount+1, 1] = new SourceGrid.Cells.Cell(Convert.ToDouble(DebitAmount.ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdBalanceSheet[rowCount+1, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    }
                    else if (Convert.ToDouble(dr["CreditBalance"].ToString()) > 0)
                    {
                        grdBalanceSheet[rowCount+1, 1] = new SourceGrid.Cells.Cell("(" + Convert.ToDouble(dr["CreditBalance"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + ")");
                        grdBalanceSheet[rowCount+1, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        grdBalanceSheet[rowCount+1, 1] = new SourceGrid.Cells.Cell(0.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdBalanceSheet[rowCount+1, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    }

                    grdBalanceSheet[rowCount+1, 2] = new SourceGrid.Cells.Cell(dr["LedgerID"].ToString());
                    grdBalanceSheet[rowCount+1, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    rowCount++;
                   
                }


            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }
        private void WriteHeaderShareHolder()
        {
            grdBalanceSheet[0, 0] = new MyHeader("ShareHolder/Capital");
            grdBalanceSheet[0, 1] = new MyHeader("Balance");
            grdBalanceSheet[0, 2] = new MyHeader("LedgerID");
           


            grdBalanceSheet[0, 0].Column.Width = 400;
            grdBalanceSheet[0, 1].Column.Width = 400;
            grdBalanceSheet[0, 2].Column.Width = 1;
            grdBalanceSheet[0, 2].Column.Visible = false;
        }

        public void GetTransactionData(bool IsCrystalReport)
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
            grdBalanceSheet.Rows.Clear();

            #region LOOK AND FEEL
            //Text format for Total
            HeaderView = new SourceGrid.Cells.Views.Cell();
            HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
            HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Cornsilk);

            GroupView = new SourceGrid.Cells.Views.Cell();
            GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            //Text format for SubGroup
            SubGroupView = new SourceGrid.Cells.Views.Cell();
            SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

            //Text format for Division
            DivisionView = new SourceGrid.Cells.Views.Cell();
            DivisionView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);

            //Text format for Division
            AmountView = new SourceGrid.Cells.Views.Cell();
            AmountView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grd_BalanceSheetDoubleClick);
         
           // Let the whole row to be selected
            grdBalanceSheet.SelectionMode = SourceGrid.GridSelectionMode.Row;
            #endregion

            double AssetSum, LiabilitiesSum;
            AssetSum = LiabilitiesSum = 0; 
            double assetbal, liabilitiesbal;
            assetbal = liabilitiesbal = 0;

            #region Closing Stock Value

            //Closing Stock Values Check For Closing Stock Quantity and Make Closing Stock Amount
            double ProductValue = 0;
            Product allproduct = new Product();

            DataTable dtGetAllProduct = allproduct.getProductByGroupID();
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
            int closingQuantity = 0;

            foreach (DataRow drProduct in dtGetAllProduct.Rows)
            {
                bool isTrans = false;
                if (drProduct["IsInventoryApplicable"].ToString() == "1")
                {
                    DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                    DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(drProduct["ProductID"].ToString()), "", m_BS.ToDate, true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
                    if (dtTransactionStockStatusInfo.Rows.Count != 0)
                    {
                        foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
                        {
                            // if (dtTransactionStockStatusInfo.Rows.Count != 0)
                            //{
                            foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
                            {
                                if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
                                {
                                    isTrans = true;
                                    closingQuantity = Convert.ToInt32(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
                                    double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]), Global.ParentAccClassID);
                                   // double AddtionalCost = Product.GetFreightandCustomDuty(Convert.ToInt32(drTransactionStockStatusInfo["rowid"]));
                                   // AdditionalExpenses = AdditionalExpenses+AddtionalCost;
                                    ProductValue += ProdPrice * closingQuantity;
                                    //ProductValue += AddtionalCost;
                                }

                            }
                            if (isTrans == false)
                            {
                                closingQuantity = Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
                                double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]), Global.ParentAccClassID);
                                ProductValue += ProdPrice * closingQuantity;
                            }

                        }

                    }
                    else
                    {
                        DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                        if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                        {
                            DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                            closingQuantity = Convert.ToInt32(dropen["Quantity"].ToString());
                            double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                            ProductValue += ProdPrice * closingQuantity;
                        }
                    }
                }

            }


            #region For Liabilities/Capital Section
            //Make Heading For Capital
            grdBalanceSheet.Redim(1, 3);
            grdBalanceSheet[0, 0] = new SourceGrid.Cells.Cell("CAPITAL");
            grdBalanceSheet[0, 0].Column.Width = 710;
            grdBalanceSheet[0, 0].View = new SourceGrid.Cells.Views.Cell(HeaderView);
           
            grdBalanceSheet[0, 1] = new SourceGrid.Cells.Cell("AMOUNT");
            //grdBalanceSheet[0, 1] = new SourceGrid.Cells.ColumnHeader("AMOUNT");
            grdBalanceSheet[0, 1].Column.Width = 245;
            grdBalanceSheet[0, 1].View = new SourceGrid.Cells.Views.Cell(HeaderView);

            grdBalanceSheet[0, 2] = new SourceGrid.Cells.Cell("0");
            grdBalanceSheet[0, 2].AddController(dblClick);
            grdBalanceSheet[0, 2].Column.Visible = false;
            
            string GroupName = AccountGroup.GetEngName("27");
            WriteTransactionSection("   " + GroupName, "", "Liabilities", "Bold", "27", IsCrystalReport);
            //grdBalanceSheet[1, 0] = new SourceGrid.Cells.Cell("     " + GroupName);
            //grdBalanceSheet[1, 0].Column.Width = 500;
            //grdBalanceSheet[1, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
            //grdBalanceSheet[1, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
           
            //grdBalanceSheet[1, 1] = new SourceGrid.Cells.Cell(" ");
            //grdBalanceSheet[1, 1].Column.Width = 245;
            //grdBalanceSheet[1, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            //grdBalanceSheet[1, 2] = new SourceGrid.Cells.Cell("27");
            //dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            //dblClick.DoubleClick += new EventHandler(grd_BalanceSheetDoubleClick);
            //grdBalanceSheet[1, 2].AddController(dblClick);
            //grdBalanceSheet[1, 2].Column.Visible = false;

             double authorisedvalue = Convert.ToDouble(Settings.GetSettings("AUTHORISED_CAPITAL"));
             WriteTransactionSection("Authorised Capital", authorisedvalue.ToString(), "Liabilities", "Normal", "0", IsCrystalReport);
            //grdBalanceSheet[2, 0] = new SourceGrid.Cells.ColumnHeader("             Authorised Capital");
           // grdBalanceSheet[2, 0] = new SourceGrid.Cells.Cell("             Authorised Capital");
           // grdBalanceSheet[2, 0].Column.Width = 500;
           // grdBalanceSheet[2, 0].View = new SourceGrid.Cells.Views.Cell(DivisionView);
           // grdBalanceSheet[2, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
           // double authorisedvalue = Convert.ToDouble(Settings.GetSettings("AUTHORISED_CAPITAL"));
           // //grdBalanceSheet[2, 1] = new SourceGrid.Cells.ColumnHeader(authorisedvalue);
           // grdBalanceSheet[2, 1] = new SourceGrid.Cells.Cell(authorisedvalue);
           // grdBalanceSheet[2, 1].Column.Width = 245;
           // grdBalanceSheet[2, 1].View = new SourceGrid.Cells.Views.Cell(DivisionView);
           // grdBalanceSheet[2, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
           // grdBalanceSheet[2, 2] = new SourceGrid.Cells.Cell("0");
           // grdBalanceSheet[2, 2].AddController(dblClick);
           // grdBalanceSheet[2, 2].Column.Visible = false;

            double issuedvalue = Convert.ToDouble(Settings.GetSettings("ISSUED_CAPITAL"));
            WriteTransactionSection("Issued Capital", issuedvalue.ToString(), "Liabilities", "Normal", "0", IsCrystalReport);
           //// grdBalanceSheet[3, 0] = new SourceGrid.Cells.ColumnHeader("             Issued Capital");
           // grdBalanceSheet[3, 0] = new SourceGrid.Cells.Cell("             Issued Capital");
           // grdBalanceSheet[3, 0].Column.Width = 500;
           // grdBalanceSheet[3, 0].View = new SourceGrid.Cells.Views.Cell(DivisionView);
           // grdBalanceSheet[3, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
           // double issuedvalue = Convert.ToDouble(Settings.GetSettings("ISSUED_CAPITAL"));
           // grdBalanceSheet[3, 1] = new SourceGrid.Cells.Cell(issuedvalue);
           // //grdBalanceSheet[3, 1] = new SourceGrid.Cells.ColumnHeader(issuedvalue);
           // grdBalanceSheet[3, 1].Column.Width = 245;
           // grdBalanceSheet[3, 1].View = new SourceGrid.Cells.Views.Cell(DivisionView);
           // grdBalanceSheet[3, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
           // grdBalanceSheet[3, 2] = new SourceGrid.Cells.Cell("0");
           // grdBalanceSheet[3, 2].AddController(dblClick);
           // grdBalanceSheet[3, 2].Column.Visible = false;
         
           

            dtLiab = AccountGroup.GetGroupTable(27);//GroupID Containing For Owner Capital

            rowCount = grdBalanceSheet.RowsCount;
            foreach (DataRow dr in dtLiab.Rows)
            {
                double m_dbal = 0;
                double m_cbal = 0;
                // Block for DateTime range selection
                if (m_BS.HasDateRange == true)//When datetime is selected
                    Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
                else//Otherwise
                    Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);

                if (m_BS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                {
                    //Do nothing
                }
                else
                {
                    string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                   
                        double Balance = (m_cbal - m_dbal);//For Liabilites remember it is always credit soo (credit-debit)
                        WriteTransactionSection(EngName, Balance.ToString(), "Liabilities", "Normal", dr["GroupID"].ToString(), IsCrystalReport);
                        //WriteVerticalLiabilities(SnoLiab, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP", "Liabilities");
                        LiabilitiesSum += Balance;
                }

            }
           
            #region BLOCK FOR ADDING  PROFIT OBTAINING FROM PROFIT AND LOSS ACCOUNT 

            //Calling the method for finding either profit or loss from P/L account
            frmProfitLossAcc m_profitLoss = new frmProfitLossAcc();
            double TotalProfit, TotalLoss;
            TotalProfit = TotalLoss = 0;
            ProfitLossAccSettings m_ProfitLoassSettings = new ProfitLossAccSettings();
            int IncomeGrpID = AccountGroup.GetIDFromType(AccountType.Income);
            int ExpenditureGrpID = AccountGroup.GetIDFromType(AccountType.Expenditure);
            m_profitLoss.Cal_TotalProfitLoss(ref TotalProfit, ref TotalLoss, IncomeGrpID, ExpenditureGrpID, m_BS.HasDateRange, m_BS.FromDate, m_BS.ToDate, m_BS.AccClassID, m_BS.ShowZeroBalance, false, m_BS.ProjectID);
            if (TotalProfit  > 0)//Incase of Profit from p/l account
            {
                //WriteTransactionSection("PaidUp Capital", Balance.ToString(), "Liabilities");
                //If there is Profit then add this profit balance to Liabilities side for balancing puropose         
                WriteTransactionSection("Reserve Fund and Accumulated Profit/Loss: (Current Year) ", TotalProfit.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "Liabilities", "Normal", "0", IsCrystalReport);
                LiabilitiesSum += TotalProfit;
                CheckAddRows = 1;
            }
            //else if (Convert.ToDouble( TotalLoss.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))) -Convert.ToDouble( AdditionalExpenses.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces))) > 0)
            else if(TotalLoss>0)
            {
                #region BLOCK FOR ADDING  LOSS OBTAINING FROM BALANCESHEET
                //If there is LOSS then add this loss balance to asset side for balancing puropose

                WriteTransactionSection("Reserve Fund and Accumulated Profit/Loss: (Current Year) ", "-" + TotalLoss.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "Liabilities", "Normal", "0", IsCrystalReport);
                LiabilitiesSum -= TotalLoss;
                CheckAddRows = 1;
                #endregion
            }
           
            #endregion//end of profit and loss calculation
            //For Mid Term and Long Term Loans
            string LoanGroup = AccountGroup.GetEngName("28");
            WriteTransactionSection("   " + LoanGroup, "", "Liabilities", "Bold", "28", IsCrystalReport);
            //rowInsert = grdBalanceSheet.RowsCount;
            //grdBalanceSheet.Rows.Insert(rowInsert);
            //string LoanGroup = AccountGroup.GetEngName("28");
            //grdBalanceSheet[rowInsert, 0] = new SourceGrid.Cells.Cell("      " + LoanGroup);
            //grdBalanceSheet[rowInsert, 0].Column.Width = 500;
            //grdBalanceSheet[rowInsert, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
            //grdBalanceSheet[rowInsert, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            //grdBalanceSheet[rowInsert, 1] = new SourceGrid.Cells.ColumnHeader("");
            //grdBalanceSheet[rowInsert, 1].Column.Width = 245;
            //grdBalanceSheet[rowInsert, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            //grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("28");
            //grdBalanceSheet[rowInsert, 2].AddController(dblClick);
            //grdBalanceSheet[rowInsert, 2].Column.Visible = false;

            //rowCount += 1;

            //Adding Values For Short Term and Long Term Loan

            dtLiab = AccountGroup.GetGroupTable(28);//GroupID Containing For Owner Capital

            foreach (DataRow dr in dtLiab.Rows)
            {
                double m_dbal = 0;
                double m_cbal = 0;
                // Block for DateTime range selection
                if (m_BS.HasDateRange == true)//When datetime is selected
                    Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
                else//Otherwise
                    Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);

                if (m_BS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                {
                    //Do nothing
                }
                else
                {
                    string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                        double Balance = (m_cbal - m_dbal);//For Liabilites remember it is always credit soo (credit-debit)
                        WriteTransactionSection(EngName, Balance.ToString(), "Liabilities", "Normal", dr["GroupID"].ToString(), IsCrystalReport);
                        //WriteVerticalLiabilities(SnoLiab, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP", "Liabilities");
                        LiabilitiesSum += Balance;
                        CheckAddRows = CheckAddRows + 1;
                   
                }

            }

            CheckAddRows = 1;
            if (IsCrystalReport == false)
            {
                WriteTransactionSection("   TOTAL CAPITAL FUND ", LiabilitiesSum.ToString(), "Liabilities", "Bold", "0", IsCrystalReport);
            }
            else
            {
                //TextBox txttotalcapitalfund = new TextBox();
                //txttotalcapitalfund.Font = new Font(txttotalcapitalfund.Font, FontStyle.Bold);
                //txttotalcapitalfund.Text = "TOTAL CAPITAL FUND";

                WriteTransactionSection("TOTAL CAPITAL FUND", LiabilitiesSum.ToString(), "Liabilities", "Bold", "0", IsCrystalReport);
            }
            #endregion//End Of Capital/ShareHolder Section

            ProgressForm.UpdateProgress(40, "Calculating Represented by section...");
            #region Start Represented By Section
           // WriteTransactionSection("   REPRESENTED BY ", "", "Assests", "Bold", "0", false);
            rowInsert = grdBalanceSheet.RowsCount;
            HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Cornsilk);
            grdBalanceSheet.Rows.Insert(rowInsert);
            grdBalanceSheet[rowInsert, 0] = new SourceGrid.Cells.Cell("REPRESENTED BY");
            grdBalanceSheet[rowInsert, 0].View = new SourceGrid.Cells.Views.Cell(HeaderView);
            grdBalanceSheet[rowInsert, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            grdBalanceSheet[rowInsert, 1] = new SourceGrid.Cells.Cell("");
            grdBalanceSheet[rowInsert, 1].View = new SourceGrid.Cells.Views.Cell(HeaderView);
            grdBalanceSheet[rowInsert, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("0");
            grdBalanceSheet[rowInsert, 2].AddController(dblClick);
            grdBalanceSheet[rowInsert, 2].Column.Visible = false;

            rowCount += 1;
          
            //rowInsert = grdBalanceSheet.RowsCount;
            //grdBalanceSheet.Rows.Insert(rowInsert);
            string currentassest = AccountGroup.GetEngName("6");
            WriteTransactionSection("   " + currentassest, "", "Assests", "Bold", "6", IsCrystalReport);
            //grdBalanceSheet[rowInsert, 0] = new SourceGrid.Cells.Cell("      " + currentassest);
            //grdBalanceSheet[rowInsert, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
            //grdBalanceSheet[rowInsert, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            //grdBalanceSheet[rowInsert, 1] = new SourceGrid.Cells.Cell("");
            //grdBalanceSheet[rowInsert, 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
            //grdBalanceSheet[rowInsert, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            //rowCount += 1;

            //grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("6");
            //grdBalanceSheet[rowInsert, 2].AddController(dblClick);
            //grdBalanceSheet[rowInsert, 2].Column.Visible = false;
                //AccountGroup processing starts for Asset
                double TotalCurrentAssest = 0;
                DataTable dtAssests = AccountGroup.GetGroupTable(6);//GroupID contain 1 for Asset
                foreach (DataRow dr in dtAssests.Rows)
                {
                    double m_dbal = 0;
                    double m_cbal = 0;
                    // Block for DateTime range selection
                    if (m_BS.HasDateRange == true)//When datetime is selected
                        Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
                    else//Otherwise
                        // Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
                        Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);

                    if (m_BS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                    {
                        //Do nothing
                    }
                    else
                    {
                        string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                        double Balance = (m_dbal - m_cbal);//For Asset[Debit Balance - Credit Balance]
                        WriteTransactionSection(EngName, Balance.ToString(), "Assests", "Normal", dr["GroupID"].ToString(), IsCrystalReport);
                       // WriteVerticalAsset(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP", "Asset", IsCrystalReport);
                        AssetSum += Balance;
                        TotalCurrentAssest += Balance;
                        CheckAddRows = CheckAddRows + 1;
                      
                    }//End of zero balance check

                }//End for loop
                //#region Closing Stock Value
                ////Closing Stock Values Check For Closing Stock Quantity and Make Closing Stock Amount
                //double ProductValue = 0;
                //Product allproduct = new Product();      
                //DataTable dtGetAllProduct = allproduct.getProductByGroupID();
                //string AccClassIDsXMLString = ReadAllAccClassID();
                //string ProjectIDsXMLString = ReadAllProjectID();
                //int closingQuantity = 0;
               
                //foreach (DataRow drProduct in dtGetAllProduct.Rows)
                //{     
                //    bool isTrans = false;
                //    if (drProduct["IsInventoryApplicable"].ToString() == "1")
                //    {
                //        DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                //        DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(drProduct["ProductID"].ToString()), "", m_BS.ToDate, true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
                //        if (dtTransactionStockStatusInfo.Rows.Count != 0)
                //        {
                //            foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
                //            {
                //                // if (dtTransactionStockStatusInfo.Rows.Count != 0)
                //                //{
                //                foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
                //                {
                //                    if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
                //                    {
                //                        isTrans = true;
                //                        closingQuantity = Convert.ToInt32(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
                //                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]), Global.ParentAccClassID);
                //                        double AddtionalCost = Product.GetFreightandCustomDuty(Convert.ToInt32(drTransactionStockStatusInfo["rowid"]));
                //                        ProductValue += ProdPrice * closingQuantity;
                //                        ProductValue += AddtionalCost;
                //                    }
                //                }
                //                if (isTrans == false)
                //                {
                //                    closingQuantity = Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
                //                    double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]), Global.ParentAccClassID);
                //                    ProductValue += ProdPrice * closingQuantity;
                //                }

                //            }

                //        }
                //        else
                //        {
                //            DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_BS.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                //            if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                //            {
                //                DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                //                closingQuantity = Convert.ToInt32(dropen["Quantity"].ToString());
                //                double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                //                ProductValue += ProdPrice * closingQuantity;
                //            }
                //        }
                //    }

                //}
                TotalCurrentAssest += ProductValue;
                WriteTransactionSection("Closing Stock", ProductValue.ToString(), "Assests", "Normal", "0", IsCrystalReport);
               // WriteAssets(Sno, "Closing Stock", 0, ProductValue.ToString(), "GROUP");
                // WriteVerticalAsset(Sno, "Closing Stock", 0, ProductValue.ToString(), "GROUP", "Asset", IsCrystalReport);
                AssetSum += ProductValue;
                #endregion




                if (IsCrystalReport == false)
                {
                    WriteTransactionSection("   Net Current Assests", TotalCurrentAssest.ToString(), "Assests", "Bold", "0", IsCrystalReport);
                }
                else
                {
                    WriteTransactionSection("Net Current Assests", TotalCurrentAssest.ToString(), "Assests", "Bold", "0", IsCrystalReport);
                }
                //rowInsert = grdBalanceSheet.RowsCount;
                //grdBalanceSheet.Rows.Insert(rowInsert);
                //grdBalanceSheet[rowInsert, 0] = new SourceGrid.Cells.Cell("      Net Current Assests");
                //grdBalanceSheet[rowInsert, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
                //grdBalanceSheet[rowInsert, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                //grdBalanceSheet[rowInsert, 1] = new SourceGrid.Cells.Cell(TotalCurrentAssest);
                //grdBalanceSheet[rowInsert, 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
                //grdBalanceSheet[rowInsert, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                //grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("0");
                //grdBalanceSheet[rowInsert, 2].AddController(dblClick);
                //grdBalanceSheet[rowInsert, 2].Column.Visible = false;
                //rowCount += 1;
                 string currentliability  = AccountGroup.GetEngName("118");
                 WriteTransactionSection("   Less:" + currentliability, "", "Assests", "Bold", "118", IsCrystalReport);
               // rowInsert = grdBalanceSheet.RowsCount;
               // grdBalanceSheet.Rows.Insert(rowInsert);
               // string currentliability  = AccountGroup.GetEngName("118");
               // grdBalanceSheet[rowInsert, 0] = new SourceGrid.Cells.Cell("      LESS:" + currentliability);
               // grdBalanceSheet[rowInsert, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
               // grdBalanceSheet[rowInsert, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

               // grdBalanceSheet[rowInsert, 1] = new SourceGrid.Cells.Cell("");
               // grdBalanceSheet[rowInsert, 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
               // grdBalanceSheet[rowInsert, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
               // rowCount += 1;
               // grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("118");
               // grdBalanceSheet[rowInsert, 2].AddController(dblClick);
               // grdBalanceSheet[rowInsert, 2].Column.Visible = false;
               DataTable dtCurrentLiab = AccountGroup.GetGroupTable(118);//GroupID Containing For Owner Capital
               double TotalCurrentLiabiltites = 0;
               foreach (DataRow dr in dtCurrentLiab.Rows)
                {
                    double m_dbal = 0;
                    double m_cbal = 0;
                    // Block for DateTime range selection
                    if (m_BS.HasDateRange == true)//When datetime is selected
                        Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
                    else//Otherwise
                        Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
                   
                    if (m_BS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                    {
                        //Do nothing
                    }
                    else
                    {
                        string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                            double Balance = (m_cbal - m_dbal);//For Liabilites remember it is always credit soo (credit-debit)
                            WriteTransactionSection(EngName, Balance.ToString(), "Liabilities", "Normal", dr["GroupID"].ToString(), IsCrystalReport);
                            //WriteVerticalLiabilities(SnoLiab, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP", "Liabilities");
                            AssetSum -= Balance;
                            
                            TotalCurrentLiabiltites += Balance;
                            CheckAddRows = CheckAddRows + 1;
                    }
                  
                }
               if (IsCrystalReport == false)
               {
                   WriteTransactionSection("    Net Current Liabilities", TotalCurrentLiabiltites.ToString(), "Assests", "Bold", "0", IsCrystalReport);
               }
               else
                   WriteTransactionSection("Net Current Liabilities", TotalCurrentLiabiltites.ToString(), "Assests", "Bold", "0", IsCrystalReport);
               //rowInsert = grdBalanceSheet.RowsCount;
               //grdBalanceSheet.Rows.Insert(rowInsert);
               //grdBalanceSheet[rowInsert, 0] = new SourceGrid.Cells.Cell("       Net Current Liabilities");
               //grdBalanceSheet[rowInsert, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

               //grdBalanceSheet[rowInsert, 1] = new SourceGrid.Cells.Cell(TotalCurrentLiabiltites.ToString());
               //grdBalanceSheet[rowInsert, 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

               //grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("0");
               //grdBalanceSheet[rowInsert, 2].AddController(dblClick);
               //grdBalanceSheet[rowInsert, 2].Column.Visible = false;
               //rowCount += 1;
            if(IsCrystalReport==false)
               WriteTransactionSection("    NET WORKING CAPITAL", AssetSum.ToString(), "Assests", "Bold", "0", IsCrystalReport);
            else
                WriteTransactionSection("NET WORKING CAPITAL", AssetSum.ToString(), "Assests", "Bold", "0", IsCrystalReport);
               //rowInsert = grdBalanceSheet.RowsCount;
               //grdBalanceSheet.Rows.Insert(rowInsert);
               //grdBalanceSheet[rowInsert, 0] = new SourceGrid.Cells.Cell("       NET WORKING CAPITAL");
               //grdBalanceSheet[rowInsert, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

               //grdBalanceSheet[rowInsert, 1] = new SourceGrid.Cells.Cell(AssetSum.ToString());
               //grdBalanceSheet[rowInsert, 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

               //grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("0");
               //grdBalanceSheet[rowInsert, 2].AddController(dblClick);
               //grdBalanceSheet[rowInsert, 2].Column.Visible = false;
               //rowCount += 1;
               string fixedassests = AccountGroup.GetEngName("14");
               WriteTransactionSection("    " + fixedassests, "", "Assests", "Bold", "14", IsCrystalReport);
               //rowInsert = grdBalanceSheet.RowsCount;
               //grdBalanceSheet.Rows.Insert(rowInsert);
               //string fixedassests = AccountGroup.GetEngName("14");
               //grdBalanceSheet[rowInsert , 0] = new SourceGrid.Cells.Cell("       " + fixedassests);
               //grdBalanceSheet[rowInsert , 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert , 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

               //grdBalanceSheet[rowInsert , 1] = new SourceGrid.Cells.Cell("");
               //grdBalanceSheet[rowInsert , 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert , 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
               //grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("14");
               //grdBalanceSheet[rowInsert, 2].AddController(dblClick);
               //grdBalanceSheet[rowInsert, 2].Column.Visible = false;

               //rowCount += 1;
               DataTable dtFixedAssests = AccountGroup.GetGroupTable(14);//GroupID Containing For Fixed Assests
               double TotalFixedAssests = 0;
               foreach (DataRow dr in dtFixedAssests.Rows)
               {
                   double m_dbal = 0;
                   double m_cbal = 0;
                   // Block for DateTime range selection
                   if (m_BS.HasDateRange == true)//When datetime is selected
                       Transaction.GetGroupBalance(m_BS.FromDate, m_BS.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);
                   else//Otherwise
                       Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_BS.AccClassID, m_BS.ProjectID);

                   if (m_BS.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                   {
                       //Do nothing
                   }
                   else
                   {
                       string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                       //double Balance = (m_cbal - m_dbal);//For Liabilites remember it is always credit soo (credit-debit)
                       double Balance = (m_dbal-m_cbal);//For Liabilites remember it is always credit soo (credit-debit)
                       WriteTransactionSection(EngName, Balance.ToString(), "Assests", "Normal", dr["GroupID"].ToString(), IsCrystalReport);
                       //WriteVerticalLiabilities(SnoLiab, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP", "Liabilities");
                       AssetSum += Balance;
                       TotalFixedAssests += Balance;
                       CheckAddRows = CheckAddRows + 1;
                   }
                
               }
            if(IsCrystalReport==false)
               WriteTransactionSection("    Net Fixed Assests", TotalFixedAssests.ToString(), "Assests", "Bold", "0", IsCrystalReport);
            else
                WriteTransactionSection("Net Fixed Assests", TotalFixedAssests.ToString(), "Assests", "Bold", "0", IsCrystalReport);
               //rowInsert = grdBalanceSheet.RowsCount;
               //grdBalanceSheet.Rows.Insert(rowInsert);
               //grdBalanceSheet[rowInsert, 0] = new SourceGrid.Cells.Cell("       Net Fixed Assests");
               //grdBalanceSheet[rowInsert, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

               //grdBalanceSheet[rowInsert, 1] = new SourceGrid.Cells.Cell(TotalFixedAssests.ToString());
               //grdBalanceSheet[rowInsert, 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

               //grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("0");
               //grdBalanceSheet[rowInsert, 2].AddController(dblClick);
               //grdBalanceSheet[rowInsert, 2].Column.Visible = false;
               //rowCount += 1;
            if(IsCrystalReport==false)
               WriteTransactionSection("    TOTAL ASSESTS", AssetSum.ToString(), "Assests", "Bold", "0", IsCrystalReport);
            else
                WriteTransactionSection("TOTAL ASSESTS", AssetSum.ToString(), "Assests", "Bold", "0", IsCrystalReport);
               //rowInsert = grdBalanceSheet.RowsCount;
               //grdBalanceSheet.Rows.Insert(rowInsert);
               //grdBalanceSheet[rowInsert , 0] = new SourceGrid.Cells.Cell("       TOTAL ASSESTS");
               //grdBalanceSheet[rowInsert , 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert , 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

               //grdBalanceSheet[rowInsert , 1] = new SourceGrid.Cells.Cell(AssetSum.ToString());
               //grdBalanceSheet[rowInsert , 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
               //grdBalanceSheet[rowInsert , 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
               //grdBalanceSheet[rowInsert, 2] = new SourceGrid.Cells.Cell("0");
               //grdBalanceSheet[rowInsert, 2].AddController(dblClick);
               //grdBalanceSheet[rowInsert, 2].Column.Visible = false;
               //rowCount += 1;

            #endregion
            ProgressForm.UpdateProgress(100, "Preparing report for display...");

            // Close the dialog if it hasn't been already

            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void WriteTransactionSection(string AccName, string Balance, string GroupType, string TextType, string GroupID, bool IsCrystalReport)
        {
            if (!IsCrystalReport)
            {
                if (GroupType == "Liabilities")
                {
                    //Text format for Total
      
                    GroupView = new SourceGrid.Cells.Views.Cell();
                    GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                    //Text format for SubGroup
                    SubGroupView = new SourceGrid.Cells.Views.Cell();
                    SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

                    //Text format for Division
                    DivisionView = new SourceGrid.Cells.Views.Cell();
                    DivisionView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);

                    //Text format for Division
                    AmountView = new SourceGrid.Cells.Views.Cell();
                    AmountView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                    int RowNum = grdBalanceSheet.RowsCount;
                    HeaderView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        HeaderView = new SourceGrid.Cells.Views.Cell();
                        HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                       // HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.CornflowerBlue);
                        HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        HeaderView = new SourceGrid.Cells.Views.Cell();
                        HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                        HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                    GroupView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                    //Text format for Ledgers
                    DivisionView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        DivisionView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        DivisionView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    DivisionView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                    DivisionView.ForeColor = Color.Blue;

                    //Text format for SubGroup
                    SubGroupView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                    SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {

                    }
                    else
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    if (TextType == "Normal")
                    {
                        
                        
                        grdBalanceSheet.Rows.Insert(RowNum);

                        grdBalanceSheet[RowNum, 0] = new SourceGrid.Cells.Cell("              " + AccName);
                        grdBalanceSheet[RowNum, 0].AddController(dblClick);
                        grdBalanceSheet[RowNum, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft; ;
                        grdBalanceSheet[RowNum, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdBalanceSheet[RowNum, 1] = new SourceGrid.Cells.Cell(Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdBalanceSheet[RowNum, 1].AddController(dblClick);
                        grdBalanceSheet[RowNum, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                        grdBalanceSheet[RowNum, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                        grdBalanceSheet[RowNum, 2] = new SourceGrid.Cells.Cell(GroupID);
                        grdBalanceSheet[RowNum, 2].AddController(dblClick);
                        grdBalanceSheet[RowNum, 2].Column.Visible = false;

                        rowCount += 1;
                    }
                    else if (TextType == "Bold")
                    {
                        //int RowNum = grdBalanceSheet.RowsCount;
                        grdBalanceSheet.Rows.Insert(RowNum);
                         HeaderView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                    GroupView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                    //Text format for Ledgers
                    DivisionView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        DivisionView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        DivisionView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    DivisionView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                    DivisionView.ForeColor = Color.Blue;

                    //Text format for SubGroup
                    SubGroupView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                        grdBalanceSheet[RowNum, 0] = new SourceGrid.Cells.Cell(AccName);
                        grdBalanceSheet[RowNum, 0].AddController(dblClick);
                       // grdBalanceSheet[RowNum, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
                        grdBalanceSheet[RowNum, 0].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                        grdBalanceSheet[RowNum, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                        if (Balance == "")
                        {
                            grdBalanceSheet[RowNum, 1] = new SourceGrid.Cells.Cell(Balance);
                        }
                        else
                        {
                            grdBalanceSheet[RowNum, 1] = new SourceGrid.Cells.Cell(Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        }
                        grdBalanceSheet[RowNum, 1].AddController(dblClick);
                       // grdBalanceSheet[RowNum, 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
                        grdBalanceSheet[RowNum, 1].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                        grdBalanceSheet[RowNum, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                        grdBalanceSheet[RowNum, 2] = new SourceGrid.Cells.Cell(GroupID);
                        grdBalanceSheet[RowNum, 2].AddController(dblClick);
                        grdBalanceSheet[RowNum, 2].Column.Visible = false;
                        rowCount += 1;
                    }
                }
                if (GroupType == "Assests")
                {
                    int RowNum = grdBalanceSheet.RowsCount;
                    HeaderView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                    GroupView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

                    //Text format for Ledgers
                    DivisionView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        DivisionView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        DivisionView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    DivisionView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                    DivisionView.ForeColor = Color.Blue;

                    //Text format for SubGroup
                    SubGroupView = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    }
                    else
                    {
                        SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                    SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                    if (RowNum % 2 == 0)
                    {
                        //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                    }
                    else
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                    }
                    if (TextType == "Normal")
                    {

                        
                        grdBalanceSheet.Rows.Insert(RowNum);

                        grdBalanceSheet[RowNum, 0] = new SourceGrid.Cells.Cell("              " + AccName);
                        grdBalanceSheet[RowNum, 0].AddController(dblClick);
                        grdBalanceSheet[RowNum, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
                        grdBalanceSheet[RowNum, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

                        grdBalanceSheet[RowNum, 1] = new SourceGrid.Cells.Cell(Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdBalanceSheet[RowNum, 1].AddController(dblClick);
                        grdBalanceSheet[RowNum, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                        grdBalanceSheet[RowNum, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                        grdBalanceSheet[RowNum, 2] = new SourceGrid.Cells.Cell(GroupID);
                        grdBalanceSheet[RowNum, 2].AddController(dblClick);
                        grdBalanceSheet[RowNum, 2].Column.Visible = false;


                        rowCount += 1;
                    }
                    else if (TextType == "Bold")
                    {
                        // int RowNum = grdBalanceSheet.RowsCount;
                        grdBalanceSheet.Rows.Insert(RowNum);

                        grdBalanceSheet[RowNum, 0] = new SourceGrid.Cells.Cell(AccName);
                        grdBalanceSheet[RowNum, 0].AddController(dblClick);
                      //  grdBalanceSheet[RowNum, 0].View = new SourceGrid.Cells.Views.Cell(GroupView);
                        grdBalanceSheet[RowNum, 0].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                        grdBalanceSheet[RowNum, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                        if (Balance == "")
                        {
                            grdBalanceSheet[RowNum, 1] = new SourceGrid.Cells.Cell(Balance);
                        }
                        else
                        {
                            grdBalanceSheet[RowNum, 1] = new SourceGrid.Cells.Cell(Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        }
                        //grdBalanceSheet[RowNum, 1] = new SourceGrid.Cells.Cell(Balance);
                        grdBalanceSheet[RowNum, 1].AddController(dblClick);
                       // grdBalanceSheet[RowNum, 1].View = new SourceGrid.Cells.Views.Cell(GroupView);
                        grdBalanceSheet[RowNum, 1].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                        grdBalanceSheet[RowNum, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                        grdBalanceSheet[RowNum, 2] = new SourceGrid.Cells.Cell(GroupID);
                        grdBalanceSheet[RowNum, 2].AddController(dblClick);
                        grdBalanceSheet[RowNum, 2].Column.Visible = false;

                        rowCount += 1;
                    }
                }
            }
            else
            { 
                
                if (GroupType == "Liabilities")
                {
                    sNo = sNo + 1;
                    
                    if (Balance=="")
                    {
                        if (Convert.ToInt32(GroupID) > 0 && TextType=="Bold")
                        {
                            liabilitiesrowcount = liabilitiesrowcount + 1;
                            dsBalanceSheet.Tables["tblGroupStandard"].Rows.Add(liabilitiesrowcount, AccName, 1);
                            //liabilitiesrowcount++;
                        }
                        else
                        {
                            dsBalanceSheet.Tables["tblBalanceSheetStandard"].Rows.Add(sNo, AccName, 0, liabilitiesrowcount, "Group");
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(GroupID) > 0 && TextType == "Bold")
                        {
                            liabilitiesrowcount = liabilitiesrowcount + 1;
                            dsBalanceSheet.Tables["tblGroupStandard"].Rows.Add(liabilitiesrowcount, AccName, 1);
                           // liabilitiesrowcount++
                        }
                        else
                        {
                            //TextBox txttotalcapitalfund = new TextBox();

                            //txttotalcapitalfund.Font = new Font(txttotalcapitalfund.Font, FontStyle.Bold);
                            //txttotalcapitalfund.Text = "Total Capital Fund";
                          //  MessageBox.Show(txttotalcapitalfund.Text);
                            //txttotalcapitalfund.Text = AccName;
                            //if (AccName == "TOTAL CAPITAL FUND")
                            //{
                            //    dsBalanceSheet.Tables["tblBalanceSheetStandard"].Rows.Add(sNo, label1.Text, Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), liabilitiesrowcount, "Group");
                            //}
                            //else
                            //{
                                dsBalanceSheet.Tables["tblBalanceSheetStandard"].Rows.Add(sNo, AccName, Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), liabilitiesrowcount, "Group");
                            //}
                        }
                    }
                  
                }
                if (GroupType == "Assests")
                {
                    int AssestSno = 0;
                    AssestSno = AssestSno + 1;
                    if (Balance == "")
                    {
                        if (Convert.ToInt32(GroupID) > 0 && TextType == "Bold")
                        {
                            liabilitiesrowcount += 1;
                            dsBalanceSheet.Tables["tblGroupStandard"].Rows.Add(liabilitiesrowcount, AccName, 2);
                        }
                        else
                        {
                           dsBalanceSheet.Tables["tblBalanceSheetStandard"].Rows.Add(AssestSno, AccName, Balance, liabilitiesrowcount, "Group");
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(GroupID) > 0 && TextType == "Bold")
                        {
                            liabilitiesrowcount += 1;
                            dsBalanceSheet.Tables["tblGroupStandard"].Rows.Add(liabilitiesrowcount, AccName, 2);
                        }
                        else
                        {
                            dsBalanceSheet.Tables["tblBalanceSheetStandard"].Rows.Add(sNo, AccName, Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), liabilitiesrowcount, "Group");
                        }
                    }

                }
            }
        }


        private void grd_BalanceSheetDoubleClick(object sender, EventArgs e)
        {
           // MessageBox.Show("Doubled Clicked");
            int groupid;
            groupid = 0;
            int curColumn = grdBalanceSheet.Selection.GetSelectionRegion().GetColumnsIndex()[0];
            int curRow = grdBalanceSheet.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdBalanceSheet, new SourceGrid.Position(curRow, 2));
            groupid = Convert.ToInt32(cellid.Value);
            //Check which type is clicked whether Group or Ledger?
            if ((groupid > 0))//Double cick event is only for either Group account or Ledger account ...not for Total amount or Opening balance
            {
                GroupBalanceSettings m_GBS = new GroupBalanceSettings();
                m_GBS.HasDateRange = m_BS.HasDateRange;
                m_GBS.ShowZeroBalance = m_BS.ShowZeroBalance;
                m_GBS.FromDate = m_BS.FromDate;
                m_GBS.ToDate = m_BS.ToDate;
                m_GBS.AllGroups = true;
                m_GBS.AccClassID = m_BS.AccClassID;
                m_GBS.OnlyPrimaryGroups = m_BS.OnlyPrimaryGroups;
                m_GBS.GroupID = groupid;//Store the GroupID value on object which achieve while double clicking the corresponding row of cell
                frmGroupBalance m_GrpBal = new frmGroupBalance(m_GBS);
                m_GrpBal.ShowDialog();
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

            dsBalanceSheet.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

            //otherwise it populate the records again and again

            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            //rptBalanceSheet rpt = new rptBalanceSheet();
            rptBalanceSheetStandard rpt = new rptBalanceSheetStandard();
            //Fill the logo on the report
            Misc.WriteLogo(dsBalanceSheet, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsBalanceSheet);
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

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

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

                pdvCompany_Slogan.Value = m_CompanyDetails.Website;
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

                pdvCompany_Slogan.Value = companyslogan;
                pvCollection.Clear();
                pvCollection.Add(pdvCompany_Slogan);
                rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            }
            pdvReport_Head.Value = "Balance Sheet";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            //pdvFiscal_Year.Value = "Fiscal Year:" + Date.ToSystem(m_CompanyDetails.FYFrom);
            pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);
            rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            if (m_BS.ToDate != null)
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem((DateTime)m_BS.ToDate);
            else
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem(Date.GetServerDate());
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            dsBalanceSheet.Tables["tblHeader"].Rows.Add(2, "Represented By");
            dsBalanceSheet.Tables["tblHeader"].Rows.Add(1, "Capital");

            //dsBalanceSheet.Tables["tblGroup"].Rows.Add(2, "Represented By");
            //dsBalanceSheet.Tables["tblGroup"].Rows.Add(1, "Capital And Liabilities");
            GetTransactionData(true);

            DataTable dtheader = dsBalanceSheet.Tables["tblHeader"];
            DataTable dtgroup = dsBalanceSheet.Tables["tblGroupStandard"];
            DataTable dtbalancesheet = dsBalanceSheet.Tables["tblBalanceSheetStandard"];
            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

            //Finally, show the report form
            frmReportViewer frm = new frmReportViewer();
            frm.SetReportSource(rpt);
            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");

            // Close the dialog
            ProgressForm.CloseForm();

            switch (prntDirect)
            {
                case 1:
                    rpt.PrintOptions.PrinterName = "";
                    rpt.PrintToPrinter(1, false, 0, 0);
                    prntDirect = 0;
                    return;
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
                    frmemail sendemail = new frmemail(FileName,1);
                    sendemail.ShowDialog();
                    rpt.Close();
                    return;
                default:
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }
                                                                                  
            frm.WindowState = FormWindowState.Maximized;
            frmMBalanceSheet_Load(sender,e);
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
                    btnPrintPreview_Click(sender, e);
                    frmMBalanceSheet_Load(sender, e);
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
                    btnPrintPreview_Click(sender, e);
                    frmMBalanceSheet_Load(sender, e);
                    break;
                case "mnuEmail":
                    //Code for pdf export
                    SaveFileDialog SaveFDExcelEmail = new SaveFileDialog();
                    SaveFDExcelEmail.InitialDirectory = "D:";
                    SaveFDExcelEmail.Title = "Enter Filename:";
                    SaveFDExcelEmail.Filter = "*.xls|*.xls";;
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
                    btnPrintPreview_Click(sender, e);
                    frmMBalanceSheet_Load(sender, e);
                    break;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows
            grdBalanceSheet.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmMBalanceSheet_Load(sender, e);

            this.Cursor = Cursors.Default;
            //Show complete notification
            //Global.Msg("Reload Complete!");
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_BS.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_BS.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in m_BS.AccClassID)
                    {
                        AccClassID.Add(Convert.ToInt32(tag));
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
            Project.GetChildProjects(Convert.ToInt32(m_BS.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_BS.ProjectID + "'";

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
                    tw.WriteElementString("PID", Convert.ToInt32(m_BS.ProjectID).ToString());
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


        private void frmMBalanceSheet_Resize(object sender, EventArgs e)
        {
            //Text format for Total
            HeaderView = new SourceGrid.Cells.Views.Cell();
            HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
            HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.Cornsilk);
            if (isMax == true)
            {
                grdBalanceSheet[0, 0] = new MyHeader("Capital");
                grdBalanceSheet[0, 1] = new MyHeader("Amount");
                grdBalanceSheet[0, 2] = new MyHeader("LedgerID");

                grdBalanceSheet[0, 0].Column.Width = 1024;
                grdBalanceSheet[0, 1].Column.Width = 400;
                grdBalanceSheet[0, 2].Column.Width = 1;
                grdBalanceSheet[0, 2].Column.Visible = false;

                grdBalanceSheet[0, 0].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdBalanceSheet[0, 1].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                isMax = false;
            }
            else
            {
                grdBalanceSheet[0, 0] = new MyHeader("Capital");
                grdBalanceSheet[0, 1] = new MyHeader("Amount");
                grdBalanceSheet[0, 2] = new MyHeader("LedgerID");

                grdBalanceSheet[0, 0].Column.Width = 710;
                grdBalanceSheet[0, 1].Column.Width = 245;
                grdBalanceSheet[0, 2].Column.Width = 1;
                grdBalanceSheet[0, 2].Column.Visible = false;
                grdBalanceSheet[0, 0].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdBalanceSheet[0, 1].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                isMax = true;
            }
        }

        private void frmMBalanceSheet_ResizeBegin(object sender, EventArgs e)
        {
            //grdBalanceSheet[0, 0] = new MyHeader("ShareHolder/Capital");
            //grdBalanceSheet[0, 1] = new MyHeader("Balance");
            //grdBalanceSheet[0, 2] = new MyHeader("LedgerID");

            //grdBalanceSheet[0, 0].Column.Width = 1024;
            //grdBalanceSheet[0, 1].Column.Width = 400;
            //grdBalanceSheet[0, 2].Column.Width = 1;
            //grdBalanceSheet[0, 2].Column.Visible = false;
        }

        private void frmMBalanceSheet_ResizeEnd(object sender, EventArgs e)
        {
            //grdBalanceSheet[0, 0] = new MyHeader("ShareHolder/Capital");
            //grdBalanceSheet[0, 1] = new MyHeader("Balance");
            //grdBalanceSheet[0, 2] = new MyHeader("LedgerID");

            //grdBalanceSheet[0, 0].Column.Width = 400;
            //grdBalanceSheet[0, 1].Column.Width = 400;
            //grdBalanceSheet[0, 2].Column.Width = 1;
            //grdBalanceSheet[0, 2].Column.Visible = false;
        }
    }
}
