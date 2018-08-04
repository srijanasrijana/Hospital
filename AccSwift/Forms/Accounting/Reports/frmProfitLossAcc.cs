using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Inventory.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using BusinessLogic;
using System.Collections;
using Inventory.DataSet;
using DateManager;
using System.Threading;
using Inventory.Forms;



namespace Inventory
{
    public partial class frmProfitLossAcc : Form
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for Income
        private ProfitLossAccSettings m_ProfitLoss;
        private DataSet.dsProfitandLoss dsProfitLoss = new DataSet.dsProfitandLoss();
        ArrayList AccClassID = new ArrayList();
        string AccClassIDsXMLString = "";
        string ProjectIDsXMLString = "";
        private int IncomeRowsCount, ExpenditureRowsCount;
        //Different grid views
        private SourceGrid.Cells.Views.Cell HeaderView;
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        private SourceGrid.Cells.Views.Cell SubGroupView;
        private int prntDirect = 0;
        private string FileName = "";
        double CheckIncome = 0;
        Product allproduct = new Product();
        DataTable dtGetAllProduct = new DataTable();
        //For Export Menu
        ContextMenu Menu_Export;

        public frmProfitLossAcc()
        {
            InitializeComponent();
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

        public frmProfitLossAcc(ProfitLossAccSettings ProfitLoss)//Constructor having class as argumenet
        {
            // Try catch block for error handling
            try
            {
                // Initilizing the constructor's parameter
                InitializeComponent();
                m_ProfitLoss = new ProfitLossAccSettings();//dynamic memory allocation of object of class  BalanceSheetSettings object
                m_ProfitLoss.FromDate = ProfitLoss.FromDate;
                m_ProfitLoss.ToDate = ProfitLoss.ToDate;
                m_ProfitLoss.AccClassID = ProfitLoss.AccClassID;
                m_ProfitLoss.HasDateRange = ProfitLoss.HasDateRange;
                m_ProfitLoss.ShowZeroBalance = ProfitLoss.ShowZeroBalance;
                m_ProfitLoss.Summary = ProfitLoss.Summary;
                m_ProfitLoss.Detail = ProfitLoss.Detail;
                m_ProfitLoss.ShowSecondLevelDtl = ProfitLoss.ShowSecondLevelDtl;
                //m_ProfitLoss.AllGroups = ProfitLoss.AllGroups;
                //m_ProfitLoss.OnlyPrimaryGroups = ProfitLoss.OnlyPrimaryGroups;
                m_ProfitLoss.DispFormat = ProfitLoss.DispFormat;
                m_ProfitLoss.ProjectID = ProfitLoss.ProjectID;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// This method is for calculating total income and expenditure for calculating either profit or loss
        /// This method is required for adding this value in balancesheet
        /// </summary>
        /// <param name="TotalIncome"></param>
        /// <param name="TotalExpenditure"></param>
        public void Cal_TotalProfitLoss(ref double TotalProfit, ref double TotalLoss,int IncomeGrpID,int ExpenditureGrpID,bool HasDateRange,DateTime? FromDate,DateTime? ToDate,ArrayList AccClassID,bool ShowZeroBalance,bool IsCrystalReport,int ProjectID)
        {
            double IncomeSum, ExpenditureSum;
            IncomeSum = ExpenditureSum = 0;
            m_ProfitLoss = new ProfitLossAccSettings();//dynamic memory allocation of object of class  BalanceSheetSettings object
            m_ProfitLoss.FromDate = FromDate;
            m_ProfitLoss.ToDate = ToDate;
            m_ProfitLoss.HasDateRange = HasDateRange;
            m_ProfitLoss.ShowZeroBalance = ShowZeroBalance;
            m_ProfitLoss.AccClassID = AccClassID;
            m_ProfitLoss.ProjectID = ProjectID;
            #region INCOME COLUMN PROCESSING
            //AccountGroup processing starts for Asset
            DataTable dt = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Income));//GroupID contain 3 for Income
            int Sno = 1;
            foreach (DataRow dr in dt.Rows)
            {
                double m_dbal = 0;
                double m_cbal = 0;
                // Block for DateTime range selection
                if (HasDateRange == true)//When datetime is selected
                    Transaction.GetGroupBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                else//Otherwise
                    Transaction.GetGroupBalance(null,null,Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                if (ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                {
                    //Do nothing
                }
                else
                {
                    string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                    double Balance = (m_cbal - m_dbal);
                    IncomeSum += Balance;

                }//End of zero balance check
                Sno++;
            }//End for loop

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
                    DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_ProfitLoss.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                    DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(drProduct["ProductID"].ToString()), "", m_ProfitLoss.ToDate, true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
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
                                    ProductValue += ProdPrice * closingQuantity;
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
                        DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_ProfitLoss.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                        if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                        {
                            DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                            closingQuantity = Convert.ToInt32(dropen["Quantity"].ToString());
                            double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(dropen["productID"].ToString()), Global.ParentAccClassID);
                            ProductValue += ProdPrice * closingQuantity;
                        }
                    }
                }

            }
           // WriteVerticalIncome(0, "Closing Stock ", 0, ProductValue.ToString(), "GROUP");
            IncomeSum += ProductValue;


            //Ledgers just beneath the Income
            double incomebal, expensebal;
            incomebal = expensebal = 0;
            WriteLedger(AccountGroup.GetIDFromType(AccountType.Income),AccountType.Income, ref incomebal, ref expensebal, false,false);   
        
            IncomeSum += incomebal;
            #endregion

            #region EXPENDITURE COLUMN PROCESSING
            DataTable dtExp = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Expenditure));//
            int SnoExp = 1;
            foreach (DataRow dr in dtExp.Rows)
            {
                double m_dbal = 0;
                double m_cbal = 0;

                // Block for DateTime range selection
                if (m_ProfitLoss.HasDateRange == true)//When datetime is selected
                    Transaction.GetGroupBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                else//Otherwise
                    Transaction.GetGroupBalance(null,null,Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);

                if (m_ProfitLoss.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                {
                    //Do nothing
                }
                else
                {
                    string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                    double Balance = (m_dbal - m_cbal);
                    ExpenditureSum += Balance;

                }//End of zero balance check
                SnoExp++;
            }//End for loop

            double ProductValue1 = 0;
            //Product allproduct = new Product();
            //DataTable dtGetAllProduct = allproduct.getProductByGroupID();
            //string AccClassIDsXMLString = ReadAllAccClassID();
            //string ProjectIDsXMLString = ReadAllProjectID();
            int closingQuantity1 = 0;
            foreach (DataRow drProduct in dtGetAllProduct.Rows)
            {
                DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_ProfitLoss.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                if (dtOpeningStockStatusInfo1.Rows.Count>0)
                {
                DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
                double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                ProductValue1 += ProdPrice * closingQuantity1;
                }
            }
            ExpenditureSum += ProductValue1;
            //if (IsCrystalReport)//Crystal report is only shown according to Vertical format
            //{
            //    WriteProfitLossOnCrystalRpt(0, "Expenditure", "Opening Stock", ProductValue1.ToString(), "Group");
            //    ExpenditureSum += ProductValue1;
            //}
            //else
            //{
            //    WriteVerticalExpenditure(SnoExp, "Opening Stock ", 0, ProductValue1.ToString(), "GROUP");
            //    ExpenditureSum += ProductValue1;
            //}


            //Display the ledger just under the account
            WriteLedger(AccountGroup.GetIDFromType(AccountType.Expenditure),AccountType.Expenditure, ref incomebal, ref expensebal, false,false);//not required to write on grid so ISgrid making false
            ExpenditureSum += expensebal;
            #endregion

            double DrOpBal = 0;
            double CrOpBal = 0;

            
            #region DIFFERENCE IN OPENENING BALANCE
            //Display Difference in Opening Balance
            Transaction.GetOpeningBalanceFromGroup(AccountGroup.GetIDFromType(AccountType.Income), m_ProfitLoss.AccClassID,ref DrOpBal,ref CrOpBal);
            double OpBalIncome = CrOpBal - DrOpBal;
            //Display Difference in Opening Balance
            Transaction.GetOpeningBalanceFromGroup(AccountGroup.GetIDFromType(AccountType.Expenditure), m_ProfitLoss.AccClassID, ref DrOpBal, ref CrOpBal);
            double OpBalExpenditue= DrOpBal - CrOpBal;
            if (OpBalIncome > OpBalExpenditue)
            {
                double OpBalDiff = OpBalIncome - OpBalExpenditue;
                IncomeSum += OpBalDiff;
            }
            else if (OpBalIncome < OpBalExpenditue)
            {
                double OpBalDiff = OpBalExpenditue - OpBalIncome;
                ExpenditureSum = +OpBalDiff;
            }

            #endregion

            //write code for wheter it is Profit or Loss??? IF Profit then add this balance to Expenditure side and if loss the add this balance to Income side
            if (IncomeSum > ExpenditureSum)
            {
                TotalProfit= (IncomeSum - ExpenditureSum);
            }
            else if (ExpenditureSum > IncomeSum)
            {
                TotalLoss += (ExpenditureSum - IncomeSum);
            }
        }
         //Writes the header on the grid
         private void MakeHeader()
         {
             #region BLOCK FOR INCOME HEDADER PART
             //Write header part
             grdProfitLossAcc.Rows.Insert(0);
             grdProfitLossAcc.Rows.Insert(1);
             grdProfitLossAcc[0, 0] = new MyHeader("Income");
             grdProfitLossAcc[0, 0].ColumnSpan = 3;
             grdProfitLossAcc[0, 5] = new MyHeader("Expenditure");
             grdProfitLossAcc[0, 5].ColumnSpan = 3;

             grdProfitLossAcc[1, 0] = new MyHeader("S.No.");
             grdProfitLossAcc[1, 1] = new MyHeader("Account Name");
             grdProfitLossAcc[1, 2] = new MyHeader("Amount");
             grdProfitLossAcc[1, 3] = new MyHeader("ID");
             grdProfitLossAcc[1, 4] = new MyHeader("Type");

             //Define the width of column size

             grdProfitLossAcc[1, 0].Column.Width = 45;
             grdProfitLossAcc[1, 1].Column.Width = 350;
             grdProfitLossAcc[1, 2].Column.Width = 90;
             grdProfitLossAcc[1, 3].Column.Width = 150;

             //Code for making column invisible
             grdProfitLossAcc.Columns[3].Visible = false;// making third column invisible and using it in  programming     
             grdProfitLossAcc.Columns[4].Visible = false;
             #endregion

             #region BLOCK FOR EXPENDITURE HEADER PART
             //FOR EXPENDITURE
             grdProfitLossAcc[1, 5] = new MyHeader("S.No.");
             grdProfitLossAcc[1, 6] = new MyHeader("Account Name");
             grdProfitLossAcc[1, 7] = new MyHeader("Amount");
             grdProfitLossAcc[1, 8] = new MyHeader("ID");
             grdProfitLossAcc[1, 9] = new MyHeader("Type");

             //Define width of column size
             grdProfitLossAcc[1, 5].Column.Width = 45;
             grdProfitLossAcc[1, 6].Column.Width = 350;
             grdProfitLossAcc[1, 7].Column.Width = 90;

             //Code for making column invisible
             grdProfitLossAcc.Columns[8].Visible = false;// making forth column invisible and using it in programming     
             grdProfitLossAcc.Columns[9].Visible = false;
             #endregion
         }

        //Block for making header part of Income for Vertical Format
         private void MakeIncomeVerticalHeader()
         {
             int row = grdProfitLossAcc.RowsCount;
             //grdProfitLossAcc.Rows.Insert(0);
             grdProfitLossAcc.Rows.Insert(row-1);
             grdProfitLossAcc[row-2, 0] = new MyHeader("S.No.");
             grdProfitLossAcc[row-2, 1] = new MyHeader("Income");
             grdProfitLossAcc[row-2, 2] = new MyHeader("Amount");
             grdProfitLossAcc[row-2, 3] = new MyHeader("ID");
             grdProfitLossAcc[row-2, 4] = new MyHeader("Type");

             //Define the width of column size
             grdProfitLossAcc[row-2, 0].Column.Width = 50;
             grdProfitLossAcc[row-2, 1].Column.Width = 720;
             grdProfitLossAcc[row-2, 2].Column.Width = 185;
             grdProfitLossAcc[row-2, 3].Column.Width = 150;
             grdProfitLossAcc[row-2, 4].Column.Width = 150;

             //Code for making column invisible
             grdProfitLossAcc.Columns[3].Visible = false;// making third column invisible and using it in  programming     
             grdProfitLossAcc.Columns[4].Visible = false;
         
         }
         //Block for making header part of Expenditure for Vertical Format
         private void MakeExpenditureVerticalHeader()
         {
             grdProfitLossAcc.Rows.Insert(0);
             grdProfitLossAcc[0, 0] = new MyHeader("S.No.");
             grdProfitLossAcc[0, 1] = new MyHeader("Expenditure");
             grdProfitLossAcc[0, 2] = new MyHeader("Amount");
             grdProfitLossAcc[0, 3] = new MyHeader("ID");
             grdProfitLossAcc[0, 4] = new MyHeader("Type");

             //Define the width of column size
             grdProfitLossAcc[0, 0].Column.Width = 50;
             grdProfitLossAcc[0, 1].Column.Width = 720;
             grdProfitLossAcc[0, 2].Column.Width = 185;
             grdProfitLossAcc[0, 3].Column.Width = 150;
             grdProfitLossAcc[0, 4].Column.Width = 150;

             //Code for making column invisible
             grdProfitLossAcc.Columns[3].Visible = false;// making third column invisible and using it in  programming     
             grdProfitLossAcc.Columns[4].Visible = false;
         
         
         }

         private void WriteIncome(int SNo, string Name, int GroupID, string Balance, string Type)
         {
             if (Type == "HEADER")
             {
                 HeaderView = new SourceGrid.Cells.Views.Cell();

                 if (IncomeRowsCount % 2 == 0)
                 {
                     HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
             }
             if (Type == "GROUP")
             {
                 GroupView = new SourceGrid.Cells.Views.Cell();
                 if (IncomeRowsCount % 2 == 0)
                 {
                     GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

             }
             //Text format for Ledgers
             if (Type == "LEDGER")
             {
                 LedgerView = new SourceGrid.Cells.Views.Cell();
                 if (IncomeRowsCount % 2 == 0)
                 {
                     LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                 LedgerView.ForeColor = Color.Blue;

             }
             if (Type == "SUBGROUP")
             {
                 //Text format for SubGroup
                 SubGroupView = new SourceGrid.Cells.Views.Cell();
                 if (IncomeRowsCount % 2 == 0)
                 {
                     SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

             }
             //Insert a row to the grid for a new row to be written
             if (grdProfitLossAcc.RowsCount <= IncomeRowsCount)
                 grdProfitLossAcc.Rows.Insert(grdProfitLossAcc.RowsCount);
             //SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
             //if (IncomeRowsCount % 2 == 0)
             //{
             //    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
             //}
             //else
             //{
             //    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
             //}
             // Block for getting GroupName 
             string strSNo = (SNo == 0 ? "" : SNo.ToString());
             grdProfitLossAcc[IncomeRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
             grdProfitLossAcc[IncomeRowsCount, 1] = new SourceGrid.Cells.Cell(Name);
             grdProfitLossAcc[IncomeRowsCount, 3] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use
             grdProfitLossAcc[IncomeRowsCount, 4] = new SourceGrid.Cells.Cell(Type);
             grdProfitLossAcc[IncomeRowsCount, 2] = new SourceGrid.Cells.Cell(Balance.ToString());
             grdProfitLossAcc[IncomeRowsCount, 2].View = new SourceGrid.Cells.Views.Cell();
             grdProfitLossAcc[IncomeRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

             //To store the current view types accourding to the row type(Ledger, Group etc)
             SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();
             
             switch (Type)
             {
                 case "GROUP":
                     CurrentView = HeaderView;
                     break;
                 case "LEDGER":
                     CurrentView = LedgerView;
                     grdProfitLossAcc[IncomeRowsCount, 1].Value = "    " + grdProfitLossAcc[IncomeRowsCount, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                     break;
                 case "SUBGROUP":
                     CurrentView = SubGroupView;
                     grdProfitLossAcc[IncomeRowsCount, 1].Value = "  " + grdProfitLossAcc[IncomeRowsCount, 1].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                     break;
                 case "HEADER":
                     CurrentView = HeaderView;
                     grdProfitLossAcc[IncomeRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     grdProfitLossAcc[IncomeRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     grdProfitLossAcc[IncomeRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     break;
                 default:
                     CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                     break;

             }

             grdProfitLossAcc[IncomeRowsCount, 0].AddController(dblClick);
             grdProfitLossAcc[IncomeRowsCount, 0].View = CurrentView;
             //grdProfitLossAcc[IncomeRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

             grdProfitLossAcc[IncomeRowsCount, 1].AddController(dblClick);
             grdProfitLossAcc[IncomeRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
             grdProfitLossAcc[IncomeRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
             //grdProfitLossAcc[IncomeRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

             grdProfitLossAcc[IncomeRowsCount, 2].AddController(dblClick);
             grdProfitLossAcc[IncomeRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
             grdProfitLossAcc[IncomeRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
             //grdProfitLossAcc[IncomeRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
             //Increment assets rows count
             IncomeRowsCount++;
         }

        /// <summary>
        /// This method is used after wrtting the grid of Expenditure soo we used the ExpenditureROwscount becuse
        /// Income is written after expenditure soo for rows count Expediturerows count is used
        /// </summary>
        /// <param name="SNo"></param>
        /// <param name="Name"></param>
        /// <param name="GroupID"></param>
        /// <param name="Balance"></param>
        /// <param name="Type"></param>
         private void WriteVerticalIncome(int SNo, string Name, int GroupID, string Balance, string Type)
         {
             grdProfitLossAcc.Rows.Insert(grdProfitLossAcc.RowsCount);
             if (Type == "HEADER")
             {
                 HeaderView = new SourceGrid.Cells.Views.Cell();
                 HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
             }
             if (Type == "GROUP")
             {
                 GroupView = new SourceGrid.Cells.Views.Cell();
                 GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
             }
             //Text format for Ledgers
             if (Type == "LEDGER")
             {
                 LedgerView = new SourceGrid.Cells.Views.Cell();
                 LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                 LedgerView.ForeColor = Color.Blue;
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
             }
             if (Type == "SUBGROUP")
             {
                 //Text format for SubGroup
                 SubGroupView = new SourceGrid.Cells.Views.Cell();
                 SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
             }

             string strSNo = (SNo == 0 ? "" : SNo.ToString());
             SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
             //if (ExpenditureRowsCount % 2 == 0)
             //{
             //    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
             //}
             //else
             //{
             //    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
             //}
             grdProfitLossAcc[ExpenditureRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
             grdProfitLossAcc[ExpenditureRowsCount, 1] = new SourceGrid.Cells.Cell(Name);
            
             grdProfitLossAcc[ExpenditureRowsCount, 3] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use
             grdProfitLossAcc[ExpenditureRowsCount, 4] = new SourceGrid.Cells.Cell(Type);
             grdProfitLossAcc[ExpenditureRowsCount, 2] = new SourceGrid.Cells.Cell(Balance.ToString());
             grdProfitLossAcc[ExpenditureRowsCount, 2].View = new SourceGrid.Cells.Views.Cell();
             grdProfitLossAcc[ExpenditureRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

             //To store the current view types accourding to the row type(Ledger, Group etc)
             SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();
             CurrentView = HeaderView;
           
             switch (Type)
             {
                 case "GROUP":
                     CurrentView = HeaderView;
                     break;
                 case "LEDGER":
                     CurrentView = LedgerView;
                     grdProfitLossAcc[ExpenditureRowsCount, 1].Value = "    " + grdProfitLossAcc[ExpenditureRowsCount, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                     break;
                 case "SUBGROUP":
                     CurrentView = SubGroupView;
                     grdProfitLossAcc[ExpenditureRowsCount, 1].Value = "  " + grdProfitLossAcc[ExpenditureRowsCount, 1].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                     break;
                 case "HEADER":
                     CurrentView = HeaderView;
                     grdProfitLossAcc[ExpenditureRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     grdProfitLossAcc[ExpenditureRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     grdProfitLossAcc[ExpenditureRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     break;
                 default:
                     CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                     break;

             }            
             grdProfitLossAcc[ExpenditureRowsCount, 0].AddController(dblClick);
             grdProfitLossAcc[ExpenditureRowsCount, 0].View = CurrentView;
             //grdProfitLossAcc[ExpenditureRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
             

             grdProfitLossAcc[ExpenditureRowsCount, 1].AddController(dblClick);
             grdProfitLossAcc[ExpenditureRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
             //grdProfitLossAcc[ExpenditureRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
             grdProfitLossAcc[ExpenditureRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopLeft;
            

             grdProfitLossAcc[ExpenditureRowsCount, 2].AddController(dblClick);
             grdProfitLossAcc[ExpenditureRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
            // grdProfitLossAcc[ExpenditureRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
             grdProfitLossAcc[ExpenditureRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            
             //Increment assets rows count
             ExpenditureRowsCount++;
         
         }

         private void WriteDetails(int GroupID, AccountType Type, bool IsCrystalReport)
         {
             try
             {
                 DataTable dtDtlLedgerID = AccountGroup.GetDetailLedgerID(GroupID, true);
                 foreach (DataRow drDtlLedgerID in dtDtlLedgerID.Rows)
                 {
                     double DebBal = 0;
                     double CreBal = 0;
                     if (m_ProfitLoss.HasDateRange == true)//If DateRange is checked
                     {
                         Transaction.GetLedgerBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_ProfitLoss.AccClassID,m_ProfitLoss.ProjectID);
                     }
                     else //Otherwise
                     {
                         Transaction.GetLedgerBalance(null,null,Convert.ToInt32(drDtlLedgerID["LedgerID"]), ref  DebBal, ref CreBal, m_ProfitLoss.AccClassID,m_ProfitLoss.ProjectID);
                     }
                     if (m_ProfitLoss.ShowZeroBalance == false && DebBal == 0 && CreBal == 0)
                         continue;
                        // return;
                     double ExpenseBal = (DebBal - CreBal);//for Asset[ Balance =(Debit Balance-Credit Balance)]
                     double IncomeBal = (CreBal - DebBal);
                     if (Type == AccountType.Income && (m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.TFormat) && (!IsCrystalReport))//Display this  only on Grid not in Crystal Report
                     {
                         WriteIncome(0, "- " + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), IncomeBal.ToString(), "LEDGER");
                     }
                     if (Type == AccountType.Income && (m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.Vertical))
                     {
                         if(IsCrystalReport)//Crystal report is shown only for Vertical format
                         {
                             WriteProfitLossOnCrystalRpt(0,"Income",drDtlLedgerID["EngName"].ToString(),IncomeBal.ToString(),"Ledger");
                         }
                         else
                         {
                             WriteVerticalIncome(0, "- " + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), IncomeBal.ToString(), "LEDGER"); 
                         }
                        
                     }
                     else if (Type == AccountType.Expenditure && (m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.TFormat) && (!IsCrystalReport))
                     {
                         WriteExpenditure(0, "- " + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), ExpenseBal.ToString(), "LEDGER");
                     }
                     else if (Type == AccountType.Expenditure && (m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.Vertical))
                     {
                         if(IsCrystalReport)//Crystal report is only shown according to Vertical format
                         {
                             WriteProfitLossOnCrystalRpt(0, "Expenditure", drDtlLedgerID["EngName"].ToString(), ExpenseBal.ToString(), "Ledger");
                         }
                         else
                         {
                             WriteVerticalExpenditure(0, "- " + drDtlLedgerID["EngName"].ToString(), Convert.ToInt32(drDtlLedgerID["LedgerID"]), ExpenseBal.ToString(), "LEDGER");
                         }

                     
                     }
                 }
             }
             catch (Exception ex)
             {

                 MessageBox.Show(ex.Message);
             }
         }

         private void WriteSecondLevel(int GroupID, AccountType Type,bool IsCrystalReport)
         {
             DataTable dtSecDtl = AccountGroup.GetGroupTable(GroupID);
             foreach (DataRow dr1 in dtSecDtl.Rows)
             {
                 double m_dbal1 = 0;
                 double m_cbal1 = 0;
                 //Check whether DateRange is checked or not?
                 if (m_ProfitLoss.HasDateRange == true)//If DateRange is checked
                 {
                     Transaction.GetGroupBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1, ref m_cbal1, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                 }
                 else //Otherwise
                 {
                     Transaction.GetGroupBalance(null,null,Convert.ToInt32(dr1["GroupID"]), true, ref m_dbal1, ref m_cbal1, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                 }
                 if (m_ProfitLoss.ShowZeroBalance == false && m_dbal1 == 0 && m_cbal1 == 0)
                     continue;

                 //Checking whether debit balance or credit balance?
                 //double Balance1;
                 //Balance1 = m_dbal1 - m_cbal1;
                 double IncomeBal = (m_cbal1 - m_dbal1);
                 double ExpenseBal = (m_dbal1 - m_cbal1);

                 // Block for getting GroupName 
                 string EngName1 = AccountGroup.GetEngName((dr1["GroupID"].ToString()));  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID

                 if (Type == AccountType.Income && ( m_ProfitLoss.DispFormat==ProfitLossAccSettings.DisplayFormat.TFormat)&&(!IsCrystalReport))
                 {
                     WriteIncome(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), IncomeBal.ToString(), "GROUP");
                 }
                 if (Type == AccountType.Income && (m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.Vertical))
                 {
                     if(IsCrystalReport)//Crystal report is only shown for vertical format
                     {
                         WriteProfitLossOnCrystalRpt(0, "Income", EngName1, IncomeBal.ToString(),"Sub_Group"); 
                     }
                     else
                     {
                         WriteVerticalIncome(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), IncomeBal.ToString(), "GROUP");
                     }
                
                 }
                 else if (Type == AccountType.Expenditure && (m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.TFormat) && (!IsCrystalReport))
                 {
                     WriteExpenditure(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), ExpenseBal.ToString(), "GROUP");
                 }
                 else if (Type == AccountType.Expenditure && (m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.Vertical))
                 {
                     if(IsCrystalReport)
                     {
                         WriteProfitLossOnCrystalRpt(0, "Expenditure", EngName1, IncomeBal.ToString(), "Sub_Group"); 
                     }
                     else
                     {
                         WriteVerticalExpenditure(0, "- " + EngName1, Convert.ToInt32(dr1["GroupID"]), ExpenseBal.ToString(), "GROUP");
                     }

                 }
             }
         }

         private void WriteLedger(int GroupID, AccountType Type, ref double incomebal, ref double expensebal, bool IsGrid,bool IsCrystalReport)
         {
             //Ledger processing starts for Asset
             Transaction Transaction = new Transaction();
             DataTable dtledg = Ledger.GetLedgerTable(GroupID);
             foreach (DataRow drledger in dtledg.Rows)
             {
                 double m_dbal1 = 0;
                 double m_cbal1 = 0;
                 // Whether DateRange is checked or not?
                 if (m_ProfitLoss.HasDateRange == true)// when DateRange is checked
                 {
                     Transaction.GetLedgerBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(drledger["LedgerID"]), ref m_dbal1, ref m_cbal1, m_ProfitLoss.AccClassID,m_ProfitLoss.ProjectID);
                 }
                 else // otherwise
                 {
                     Transaction.GetLedgerBalance(null,null,Convert.ToInt32(drledger["LedgerID"]), ref m_dbal1, ref m_cbal1, m_ProfitLoss.AccClassID,m_ProfitLoss.ProjectID);
                 }
                 if (m_ProfitLoss.ShowZeroBalance == false && m_dbal1 == 0 && m_cbal1 == 0)
                     continue;
                 Global.Msg(grdProfitLossAcc.Rows.Count.ToString());
                 grdProfitLossAcc.Rows.Insert(IncomeRowsCount);
                 // Block for getting LedgerName
                 DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drledger["LedgerID"]), LangMgr.DefaultLanguage);
                 DataRow drLedgerInfo = dtLedgerInfo.Rows[0];

                 if (Type == AccountType.Income && m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.TFormat)//The position of grid in Income is similar for T-format and Vertical format
                 {
                     double IncomeBal = (m_cbal1 - m_dbal1);
                     incomebal = IncomeBal;
                     if (IsGrid == true)
                     {
                         WriteIncome(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), IncomeBal.ToString(), "LEDGER");
                     }
                 }
                 if (Type == AccountType.Income && m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.Vertical)//The position of grid in Income is similar for T-format and Vertical format
                 {
                     double IncomeBal = (m_cbal1 - m_dbal1);
                     incomebal = IncomeBal;
                     if(IsCrystalReport)
                     {
                         WriteProfitLossOnCrystalRpt(0, "Income", drLedgerInfo["LedName"].ToString(), IncomeBal.ToString(),"Ledger"); 
                     }
                     else
                     {

                         if (IsGrid == true)
                         {
                             WriteVerticalIncome(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), IncomeBal.ToString(), "LEDGER");
                         }
                     }
                    
                 }
                 else if (Type == AccountType.Expenditure && m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.TFormat)//For the T-format Expenditure
                 {
                     double ExpesneBal = (m_dbal1 - m_cbal1);
                     expensebal = ExpesneBal;
                     if (IsGrid == true)
                     {
                         WriteExpenditure(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), ExpesneBal.ToString(), "LEDGER");
                     }
                 }
                 else if (Type == AccountType.Expenditure && m_ProfitLoss.DispFormat == ProfitLossAccSettings.DisplayFormat.Vertical)//For the Vertical Format
                 {
                     double ExpesneBal = (m_dbal1 - m_cbal1);
                     expensebal = ExpesneBal;
                     if(IsCrystalReport)
                     {

                         WriteProfitLossOnCrystalRpt(0, "Expenditure", drLedgerInfo["LedName"].ToString(), ExpesneBal.ToString(), "Ledger"); 
                     }
                     else
                     {
                         if (IsGrid == true)//When has to be written in Grid
                         {
                             WriteVerticalExpenditure(0, drLedgerInfo["LedName"].ToString(), Convert.ToInt32(drledger["LedgerID"]), ExpesneBal.ToString(), "LEDGER");

                         }
                     }
                    
                 }
             }
         }

         private void WriteExpenditure(int SNo, string Name, int GroupID, string Balance, string Type)
         {

             if (Type == "HEADER")
             {
                 HeaderView = new SourceGrid.Cells.Views.Cell();

                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
             }
             if (Type == "GROUP")
             {
                 GroupView = new SourceGrid.Cells.Views.Cell();
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

             }
             //Text format for Ledgers
             if (Type == "LEDGER")
             {
                 LedgerView = new SourceGrid.Cells.Views.Cell();
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                 LedgerView.ForeColor = Color.Blue;

             }
             if (Type == "SUBGROUP")
             {
                 //Text format for SubGroup
                 SubGroupView = new SourceGrid.Cells.Views.Cell();
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);

             }
             //Insert a row to the grid for a new row to be written
             if (grdProfitLossAcc.RowsCount <= ExpenditureRowsCount)
                 grdProfitLossAcc.Rows.Insert(grdProfitLossAcc.RowsCount);

             // Block for getting GroupName 

             string strSNo = (SNo == 0 ? "" : SNo.ToString());//Show blank if sno is 0
             grdProfitLossAcc[ExpenditureRowsCount, 5] = new SourceGrid.Cells.Cell(strSNo);
             grdProfitLossAcc[ExpenditureRowsCount, 6] = new SourceGrid.Cells.Cell(Name);
             grdProfitLossAcc[ExpenditureRowsCount, 8] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use
             grdProfitLossAcc[ExpenditureRowsCount, 9] = new SourceGrid.Cells.Cell(Type);
             grdProfitLossAcc[ExpenditureRowsCount, 7] = new SourceGrid.Cells.Cell(Balance.ToString());
             grdProfitLossAcc[ExpenditureRowsCount, 7].View = new SourceGrid.Cells.Views.Cell();
             grdProfitLossAcc[ExpenditureRowsCount, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

             SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
             if (ExpenditureRowsCount % 2 == 0)
             {
                 alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
             }
             else
             {
                 alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
             }
             //To store the current view types accourding to the row type(Ledger, Group etc)
             SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

             switch (Type)
             {
                 case "GROUP":
                     CurrentView = GroupView;
                     break;
                 case "LEDGER":
                     CurrentView = LedgerView;
                     grdProfitLossAcc[ExpenditureRowsCount, 6].Value = "    " + grdProfitLossAcc[ExpenditureRowsCount, 6].Value; //Give a little space for ledger so that it appears it is inside its parent group
                     break;
                 case "SUBGROUP":
                     CurrentView = SubGroupView;
                     grdProfitLossAcc[ExpenditureRowsCount, 6].Value = "  " + grdProfitLossAcc[ExpenditureRowsCount, 6].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                     break;
                 case "HEADER":
                     CurrentView = HeaderView;
                     grdProfitLossAcc[ExpenditureRowsCount, 5].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     grdProfitLossAcc[ExpenditureRowsCount, 6].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     grdProfitLossAcc[ExpenditureRowsCount, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     break;
                 default:
                     CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                     break;

             }


             grdProfitLossAcc[ExpenditureRowsCount, 5].AddController(dblClick);
             grdProfitLossAcc[ExpenditureRowsCount, 5].View = CurrentView;
             //grdProfitLossAcc[ExpenditureRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(alternate);

             grdProfitLossAcc[ExpenditureRowsCount, 6].AddController(dblClick);
             grdProfitLossAcc[ExpenditureRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(CurrentView);
             //grdProfitLossAcc[ExpenditureRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
             grdProfitLossAcc[ExpenditureRowsCount, 6].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

             grdProfitLossAcc[ExpenditureRowsCount, 7].AddController(dblClick);
             grdProfitLossAcc[ExpenditureRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(CurrentView);
            // grdProfitLossAcc[ExpenditureRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
             grdProfitLossAcc[ExpenditureRowsCount, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
             //Increment Liablities rows count
             ExpenditureRowsCount++;
         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SNo"></param>
        /// <param name="Name"></param>

        /// 
        /// 
        /// <param name="GroupID"></param>
        /// <param name="Balance"></param>
        /// <param name="Type"></param>
         private void WriteVerticalExpenditure(int SNo, string Name, int GroupID, string Balance, string Type)
         {
             grdProfitLossAcc.Rows.Insert(grdProfitLossAcc.RowsCount);
             if (Type == "HEADER")
             {
                 HeaderView = new SourceGrid.Cells.Views.Cell();
                
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
             }
             if (Type == "GROUP")
             {
                 GroupView = new SourceGrid.Cells.Views.Cell();
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     GroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
                
             }
             //Text format for Ledgers
             if (Type == "LEDGER")
             {
                 LedgerView = new SourceGrid.Cells.Views.Cell();
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                 LedgerView.ForeColor = Color.Blue;
                 
             }
             if (Type == "SUBGROUP")
             {
                 //Text format for SubGroup
                 SubGroupView = new SourceGrid.Cells.Views.Cell();
                 if (ExpenditureRowsCount % 2 == 0)
                 {
                     SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                 }
                 else
                 {
                     SubGroupView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                 }
                 SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
                
             }
             string strSNo = (SNo == 0 ? "" : SNo.ToString());//Show blank if sno is 0
             SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
             if (ExpenditureRowsCount % 2 == 0)
             { 
                 //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
             }
             else
             {
                 alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
             }
             grdProfitLossAcc[ExpenditureRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
             grdProfitLossAcc[ExpenditureRowsCount, 1] = new SourceGrid.Cells.Cell(Name);
             grdProfitLossAcc[ExpenditureRowsCount, 3] = new SourceGrid.Cells.Cell(GroupID.ToString());//Adding GroupID of each row in fourth column as invisible for further use
             grdProfitLossAcc[ExpenditureRowsCount, 4] = new SourceGrid.Cells.Cell(Type);
             grdProfitLossAcc[ExpenditureRowsCount, 2] = new SourceGrid.Cells.Cell(Balance.ToString());
             grdProfitLossAcc[ExpenditureRowsCount, 2].View = new SourceGrid.Cells.Views.Cell();
             grdProfitLossAcc[ExpenditureRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

             //To store the current view types accourding to the row type(Ledger, Group etc)
             SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();
             switch (Type)
             {
                 case "GROUP":
                     CurrentView = HeaderView;
                     break;
                 case "LEDGER":
                     CurrentView = LedgerView;
                     //grdProfitLossAcc[ExpenditureRowsCount, 1].Value = "    " + grdProfitLossAcc[IncomeRowsCount, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                     grdProfitLossAcc[ExpenditureRowsCount, 1].Value = "    " + grdProfitLossAcc[ExpenditureRowsCount, 1].Value; //Give a little space for ledger so that it appears it is inside its parent group
                     break;
                 case "SUBGROUP":
                     CurrentView = SubGroupView;
                     grdProfitLossAcc[ExpenditureRowsCount, 1].Value = "  " + grdProfitLossAcc[ExpenditureRowsCount, 1].Value; //Give a little space for subgroup so that it appears it is inside its parent group
                     break;
                 case "HEADER":
                     CurrentView = HeaderView;
                     grdProfitLossAcc[ExpenditureRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     grdProfitLossAcc[ExpenditureRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     grdProfitLossAcc[ExpenditureRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopCenter;
                     break;
                 default:
                     CurrentView = SubGroupView; //Because it is the normal formatting without any makeups
                     break;
             }

             grdProfitLossAcc[ExpenditureRowsCount, 0].AddController(dblClick);
             grdProfitLossAcc[ExpenditureRowsCount, 0].View = CurrentView;
            // grdProfitLossAcc[ExpenditureRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

             grdProfitLossAcc[ExpenditureRowsCount, 1].AddController(dblClick);
             grdProfitLossAcc[ExpenditureRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
             //grdProfitLossAcc[ExpenditureRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
             grdProfitLossAcc[ExpenditureRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.TopLeft;
            

             grdProfitLossAcc[ExpenditureRowsCount, 2].AddController(dblClick);
             grdProfitLossAcc[ExpenditureRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
             //grdProfitLossAcc[ExpenditureRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
             grdProfitLossAcc[ExpenditureRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            
             //Increment Expenditure rows count
             ExpenditureRowsCount++;
         }

         private void WriteProfitLossOnCrystalRpt(int SNo, string GroupType,string AccName, string Amount, string AccType)
         {
         
             //string strSNo = (SNo == 0 ? "" : SNo.ToString());
             switch(GroupType)
             {
                 case "Income"://Income is displayed below Expenditure soo its GroupNumber is 2
                     if(AccType=="Group")
                     {
                         dsProfitLoss.Tables["tblProfitLoss"].Rows.Add(SNo, AccName, Amount, 2, "Group");

                     }
                     else if(AccType=="Sub_Group")
                     {
                         dsProfitLoss.Tables["tblProfitLoss"].Rows.Add(SNo, AccName, Amount, 2, "Sub_Group"); 
                     }
                     else if(AccType=="Ledger")
                     {
                         dsProfitLoss.Tables["tblProfitLoss"].Rows.Add(SNo, AccName, Amount, 2, "Ledger");
                     }

                     break;
                 case "Expenditure":
                     if (AccType == "Group")
                     {
                         dsProfitLoss.Tables["tblProfitLoss"].Rows.Add(SNo, AccName, Amount, 1, "Group");
                     }
                     else if(AccType=="Sub_Group")
                     {
                         dsProfitLoss.Tables["tblProfitLoss"].Rows.Add(SNo, AccName, Amount, 1, "Sub_Group");
                     }
                     else if (AccType == "Ledger")
                     {
                         dsProfitLoss.Tables["tblProfitLoss"].Rows.Add(SNo, AccName, Amount, 1, "Ledger");
                     }
                     break;

             }

         }


         /// <summary>
         /// Gets the selected root accounting class ID
         /// </summary>
         /// <returns></returns>
         private int GetRootAccClassID()
         {
             if (m_ProfitLoss.AccClassID.Count > 0)
             {
                 //Find Root Class
                 DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(m_ProfitLoss.AccClassID[0]));
                 return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);
                 
             }

             return 1;//The default root class ID
         }
       

        /// <summary>
        /// This method is used to display vertical Profit and Loss A/C.It is also used to show in both case like IN load_condition and Print_Preview
        /// </summary>
        /// <param name="IsCrystalReport"></param>
        private void DisplayVerticalProfitLoss(bool IsCrystalReport)
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

            if(!IsCrystalReport)
            {   
                ExpenditureRowsCount = 1;
                grdProfitLossAcc.Redim(1, 5);
                grdProfitLossAcc.FixedRows = 1;
                MakeExpenditureVerticalHeader();

                //Let the whole row to be selected
                grdProfitLossAcc.SelectionMode = SourceGrid.GridSelectionMode.Row;
            }

            double IncomeSum, ExpenditureSum;
            IncomeSum = ExpenditureSum = 0;
            double incomebal, expensebal;
            incomebal = expensebal = 0;

            ProgressForm.UpdateProgress(40, "Calculating expenditure...");

            #region EXPENDITURE COLUMN PROCESSING
            DataTable dtExp1 = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Expenditure));//
            int SnoExp = 1;
            foreach (DataRow dr in dtExp1.Rows)
            {
                double m_dbal = 0;
                double m_cbal = 0;
                // Block for DateTime range selection
                if (m_ProfitLoss.HasDateRange == true)//When datetime is selected
                    Transaction.GetGroupBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                else//Otherwise
                    // Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                    Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);

                if (m_ProfitLoss.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                {
                    //Do nothing
                }
                else
                {
                    string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                    //double Balance = (m_cbal - m_dbal);//For Asset[Debit Balance - Credit Balance]
                    double Balance = (m_dbal - m_cbal);
                    //If crystal report has to be shown
                    if(IsCrystalReport)//write on Crystal report
                    {
                        WriteProfitLossOnCrystalRpt(SnoExp, "Expenditure", EngName, Balance.ToString(),"Group");
                    }
                    else//if P/L account has to be shown in grid
                    {
                        WriteVerticalExpenditure(SnoExp, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
                    }
                   
                    ExpenditureSum += Balance;
                    //If details is selected, show details i.e. ledgers present inside
                    //If Second level group is selected, show them
                    if (m_ProfitLoss.ShowSecondLevelDtl == true)
                        WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), AccountType.Expenditure, IsCrystalReport);

                    if (m_ProfitLoss.Detail == true)
                        WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Expenditure,IsCrystalReport);
               
                }//End of zero balance check
                SnoExp++;
            }//End for loop

            //Display the ledger just under the account
            if(!IsCrystalReport)
            WriteLedger(AccountGroup.GetIDFromType(AccountType.Expenditure), AccountType.Expenditure, ref incomebal, ref expensebal, true,IsCrystalReport);
            ExpenditureSum += expensebal;
           
            // MessageBox.Show(CheckIncome.ToString());
            double ProductValue1 = 0;
            int closingQuantity1 = 0;
            foreach (DataRow drProduct in dtGetAllProduct.Rows)
            {
                DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_ProfitLoss.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                {
                    DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                    closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
                    double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                    ProductValue1 += ProdPrice * closingQuantity1;
                }
            }
            if (IsCrystalReport)//Crystal report is only shown according to Vertical format
            {
                WriteProfitLossOnCrystalRpt(0, "Expenditure", "Opening Stock", ProductValue1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "Group");
                ExpenditureSum += ProductValue1;
            }
            else
            {
                WriteVerticalExpenditure(SnoExp, "Opening Stock ", 0, ProductValue1.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
                ExpenditureSum += ProductValue1;
            }

            #region DIFFERENCE IN OPENENING BALANCE OF EXPENDITURE

            double DrOpBal=0;
            double CrOpBal=0;
            //Display Difference in Opening Balance
            Transaction.GetOpeningBalanceFromGroup(AccountGroup.GetIDFromType(AccountType.Income), m_ProfitLoss.AccClassID, ref DrOpBal, ref CrOpBal);

            double OpBalIncome1 = CrOpBal - DrOpBal;
            Transaction.GetOpeningBalanceFromGroup(AccountGroup.GetIDFromType(AccountType.Expenditure), m_ProfitLoss.AccClassID, ref DrOpBal, ref CrOpBal);

            //Display Difference in Opening Balance
            double OpBalExpenditue1 = DrOpBal - CrOpBal;
            if (OpBalIncome1 < OpBalExpenditue1)
            {
                double OpBalDiff = OpBalExpenditue1 - OpBalIncome1;
                ExpenditureSum = +OpBalDiff;
                if(IsCrystalReport)
                {
                    WriteProfitLossOnCrystalRpt(0, "Expenditure", "Difference in Opening Balance", OpBalDiff.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
                }
                else
                {
                    WriteVerticalExpenditure(0, "Difference in Opening Balance", 0, OpBalDiff.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
                    
                }

            }

            #endregion

            #region BLOCK FOR CALCULATING THE INCOME SUM IN EXPENDITURE PORTION
            DataTable dtExp = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Income));//G
            foreach (DataRow dr in dtExp.Rows)
            {
                double m_dbal = 0;
                double m_cbal = 0;
                // Block for DateTime range selection
                if (m_ProfitLoss.HasDateRange == true)//When datetime is selected
                    Transaction.GetGroupBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                else//Otherwise
                    // Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                    Transaction.GetGroupBalance(null,null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);

                if (m_ProfitLoss.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                {
                    //Do nothing
                }
                else
                {
                    double Balance = (m_cbal - m_dbal);//Remember asset and Liabilities are always credit so (credit-debit)
                    IncomeSum += Balance;

                }//End of zero balance check
            }//End for loop
            //Display the ledger just under the account
            #endregion

          //  MessageBox.Show("IncomeSum in Expenditure With Out Closing Stock" + IncomeSum.ToString());


            #region  //Closing Stock Values Check For Closing Stock Quantity and Make Closing Stock Amount
            double ProductValue = 0;
            int closingQuantity = 0;
            //bool isTrans = false;
            foreach (DataRow drProduct in dtGetAllProduct.Rows)
            {
                bool isTrans = false;
                if (drProduct["IsInventoryApplicable"].ToString() == "1")
                {
                    DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_ProfitLoss.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                    DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(drProduct["ProductID"].ToString()), "", m_ProfitLoss.ToDate, true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
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
                                    ProductValue += ProdPrice * closingQuantity;
                                  //  ProductValue += AddtionalCost;
                                    
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
                        DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_ProfitLoss.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
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
            IncomeSum += ProductValue;
            #endregion
          //  MessageBox.Show("Closing Stock" + ProductValue.ToString());
            if (IncomeSum > ExpenditureSum)
            {
                if (IsCrystalReport)
                {
                    WriteProfitLossOnCrystalRpt(0, "Expenditure", "Total Profit: ", (IncomeSum - ExpenditureSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "Group");
                }
                else
                {
                    WriteVerticalExpenditure(0, "Total Profit: ", 0, (IncomeSum - ExpenditureSum).ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces)), "GROUP");
                }

                ExpenditureSum += (IncomeSum - ExpenditureSum);
            }
           
            ////////////////////

            //just for testing purpose
            if(IsCrystalReport)
            {
                //WriteProfitLossOnCrystalRpt(0,"Expenditure","Expenditure Total : ",ExpenditureSum.ToString(),"Group");
            }
            else
            {
                WriteVerticalExpenditure(0, "EXPENDITURE TOTAL", 0, ExpenditureSum.ToString(), "GROUP");               
            }
           
            #endregion         

            //This code is just for separating Income and Expenditure section
            if(!IsCrystalReport)//Just only purpose for showing on Grid not for Crystal report
            {
                WriteVerticalIncome(0, "", 0, "", "EXPENDITURE_END");//fOR separaton the area of  Income and Expenditure Section just posting Blank space
                //WriteVerticalIncome(0, "Income ", 0, "Amount","HEADER");
                MakeIncomeVerticalHeader();
               
            }

            ProgressForm.UpdateProgress(80, "Calculating Income...");
        
            #region INCOME COLUMN PROCESSING
            //AccountGroup processing starts for Income
            DataTable dt1 = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Income));//GroupID contain 3 for Income
            int Sno = 1;
            foreach (DataRow dr in dt1.Rows)
            {
                double m_dbal = 0;
                double m_cbal = 0;
                // Block for DateTime range selection
                if (m_ProfitLoss.HasDateRange == true)//When datetime is selected
                    Transaction.GetGroupBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                else//Otherwise
                    //Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                    Transaction.GetGroupBalance(null, null, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);

                if (m_ProfitLoss.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                {
                    //Do nothing
                }
                else
                {
                    string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID 
                    double Balance = (m_cbal - m_dbal);
                    if(IsCrystalReport)//For crystal report
                    {
                        WriteProfitLossOnCrystalRpt(Sno, "Income", EngName, Balance.ToString(),"Group"); 
                    }
                    else//For writting on grid
                    {
                        WriteVerticalIncome(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
                    }

                    //IncomeSum += Balance;

                    //If Second level group is selected, show them
                    if (m_ProfitLoss.ShowSecondLevelDtl == true)
                        WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), AccountType.Income, IsCrystalReport);
                    //If details is selected, show details i.e. ledgers present inside
                    if (m_ProfitLoss.Detail == true)
                        WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Income,IsCrystalReport);       

                }//End of zero balance check
                Sno++;
            }//End for loop
            //Ledgers just beneath the Income

         
            if (IsCrystalReport)//Crystal report is only shown according to Vertical format
            {
                WriteProfitLossOnCrystalRpt(0, "Income", "Closing Stock", ProductValue.ToString(), "Group");
               // IncomeSum += ProductValue;
            }
            else
            {
                WriteVerticalIncome(0, "Closing Stock ", 0, ProductValue.ToString(), "GROUP");
                //IncomeSum += ProductValue;
            }

            //WriteLedger(m_ProfitLoss.IncomeGrpID, AccountType.VerticalIncome, ref incomebal, ref expensebal,true);
            //IncomeSum += incomebal;//Here this code is not essential because this addition is done already in Expenditure section
            //In case of opening balance of Income 
            #region DIFFERENCE IN OPENENING BALANCE
            if (OpBalIncome1 < OpBalExpenditue1)
            {
                double OpBalDiff = OpBalIncome1 - OpBalExpenditue1;
                IncomeSum += OpBalDiff;

                if(IsCrystalReport)//for crystal report
                {
                    WriteProfitLossOnCrystalRpt(0, "Income", "Difference in Opening Balance: ", OpBalDiff.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
                }
                else//for grid
                {
                    WriteVerticalIncome(0, "Difference in Opening Balance", 0, OpBalDiff.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");  
                }

            }
            #endregion

            if (IncomeSum < ExpenditureSum)
            {
              
                if(IsCrystalReport)
                {
                    WriteProfitLossOnCrystalRpt(0, "Income", "Total Loss: ", (ExpenditureSum - IncomeSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "Group"); 
                }
                else
                {
                    WriteVerticalIncome(0, "Total Loss: ", 0, (ExpenditureSum - IncomeSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
                }
                IncomeSum += (ExpenditureSum - IncomeSum);
            }
            //Block for Getting Expenditure Total Value...It need here because we have to calculate either profit or loss at the end of Income 
            //Here we just do calculation part rather than writing on grid
            //Block for writting Income total
            if(IsCrystalReport)
            {
                //WriteProfitLossOnCrystalRpt(0, "Income", "Income Total: ", IncomeSum.ToString(), "Group"); 
            }
            else
            {
                WriteVerticalIncome(0, "INCOME TOTAL", 0, IncomeSum.ToString(), "GROUP");
            }
   

            #endregion                 

            ProgressForm.UpdateProgress(100, "Preparing report for display...");

            // Close the dialog if it hasn't been already

            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
              

        }
        private void frmProfitLossAcc_Load(object sender, EventArgs e)
         {          
              DisplayBannar();
              AccClassIDsXMLString = ReadAllAccClassID();
              ProjectIDsXMLString = ReadAllProjectID();
              dtGetAllProduct = allproduct.getProductByGroupID(); 
             #region BLOCK FOR ORIENTATION PURPOSE FOR INCOME AND EXPENDITURE

             //Text format for Total
             HeaderView = new SourceGrid.Cells.Views.Cell();
             HeaderView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

             GroupView = new SourceGrid.Cells.Views.Cell();
             GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

             //Text format for Ledgers
             LedgerView = new SourceGrid.Cells.Views.Cell();
             LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
             LedgerView.ForeColor = Color.Blue;

             //Text format for SubGroup
             SubGroupView = new SourceGrid.Cells.Views.Cell();
             SubGroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
             //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code

             //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code

             // double click for Asset

             dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
             dblClick.DoubleClick += new EventHandler(grdProfitLoss_DoubleClick);



             //Disable multiple selection
             grdProfitLossAcc.Selection.EnableMultiSelection = false;


             #endregion

            //Here are two format for displaying P/L account

             #region BLOCK FOR T-FORMAT
             if (m_ProfitLoss.DispFormat== ProfitLossAccSettings.DisplayFormat.TFormat)
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

                double IncomeOpBalDifference, ExpenditureOpBalDifference;
                IncomeOpBalDifference = ExpenditureOpBalDifference = 0;
                AccountGroup AccountGroup = new AccountGroup();
                Transaction Transaction = new Transaction();

                double IncomeSum, ExpenditureSum;
                IncomeSum = ExpenditureSum = 0;
                double incomebal, expensebal;
                incomebal = expensebal = 0;
                IncomeRowsCount = 2;
                ExpenditureRowsCount = 2;
                grdProfitLossAcc.Redim(2, 10);
                grdProfitLossAcc.FixedRows = 2;
                MakeHeader();

                ProgressForm.UpdateProgress(40, "Calculating Income...");

                #region INCOME COLUMN PROCESSING
                //AccountGroup processing starts for Asset
                DataTable dt = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Income));//GroupID contain 3 for Income
                int Sno = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    double m_dbal = 0;
                    double m_cbal = 0;
                    // Block for DateTime range selection
                    if (m_ProfitLoss.HasDateRange == true)//When datetime is selected
                        Transaction.GetGroupBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                    else//Otherwise
                        //Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);                       
                        Transaction.GetGroupBalance(Convert.ToDateTime("01 / 01 / 1900"), Convert.ToDateTime("01 / 01 / 1900"), Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                    if (m_ProfitLoss.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                    {
                        //Do nothing
                    }
                    else
                    {
                        string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                        //double Balance = (m_dbal - m_cbal);//For Income[Debit Balance - Credit Balance]
                        //For testing 
                        double Balance = (m_cbal - m_dbal);
                        WriteIncome(Sno, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
                        IncomeSum += Balance;

                        //If details is selected, show details i.e. ledgers present inside
                        if (m_ProfitLoss.Detail == true)
                            WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Income,false);

                        //If Second level group is selected, show them
                        if (m_ProfitLoss.ShowSecondLevelDtl == true)
                            WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), AccountType.Income,false);

                    }//End of zero balance check
                    Sno++;
                }//End for loop
                //Ledgers just beneath the Income

                WriteLedger(AccountGroup.GetIDFromType(AccountType.Income), AccountType.Income, ref incomebal, ref expensebal,true,false);//here has to write on grid so making true
                IncomeSum += incomebal;

                #endregion

                 #region For Closing Stock Value
               // Product allproduct = new Product();
                double ProductValue = 0;
               // DataTable dtGetAllProduct = allproduct.getProductByGroupID();
                 //AccClassIDsXMLString = ReadAllAccClassID();
                 //ProjectIDsXMLString = ReadAllProjectID();
                int closingQuantity = 0;
               
                foreach (DataRow drProduct in dtGetAllProduct.Rows)
                {
                    bool isTrans = false;
                    if (drProduct["IsInventoryApplicable"].ToString() == "1")
                    {
                        //if (drProduct["ProductID"].ToString() == "398")
                        //{
                        //    MessageBox.Show("Pen");
                        //}
                        //if (drProduct["ProductID"].ToString() == "399")
                        //{
                        //    MessageBox.Show("Copy");
                        //}
                        DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_ProfitLoss.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                        DataTable dtTransactionStockStatusInfo = StockStatusBook.GetStockStatusBook1(null, Convert.ToInt32(drProduct["ProductID"].ToString()), "", m_ProfitLoss.ToDate, true, StockStatusType.ClosingStock, AccClassIDsXMLString, ProjectIDsXMLString);
                        if (dtTransactionStockStatusInfo.Rows.Count != 0)
                        {
                            foreach (DataRow drOpeningStockStatusInfo in dtOpeningStockStatusInfo.Rows)
                            {
                                foreach (DataRow drTransactionStockStatusInfo in dtTransactionStockStatusInfo.Rows)
                                {
                                    if (Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]) == Convert.ToInt32(drOpeningStockStatusInfo["ProductID"]))
                                    {
                                        isTrans = true;
                                        closingQuantity = Convert.ToInt32(drTransactionStockStatusInfo["Quantity"]) + Convert.ToInt32(drOpeningStockStatusInfo["Quantity"]);
                                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drTransactionStockStatusInfo["ProductID"]), Global.ParentAccClassID);

                                       // double AddtionalCost = Product.GetFreightandCustomDuty(Convert.ToInt32( drTransactionStockStatusInfo["rowid"]));
                                        ProductValue += ProdPrice * closingQuantity;
                                      //  ProductValue += AddtionalCost;
                                        //break;
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
                            DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_ProfitLoss.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
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
                WriteIncome(Sno, "Closing Stock ", 29, ProductValue.ToString(), "GROUP");

               // WriteVerticalIncome(0, "Closing Stock: ", 0, ProductValue.ToString(), "GROUP");
                IncomeSum += ProductValue;

                #endregion

                ProgressForm.UpdateProgress(60, "Calculating Expenditure...");

                #region EXPENDITURE COLUMN PROCESSING
                DataTable dtExp = AccountGroup.GetGroupTable(AccountGroup.GetIDFromType(AccountType.Expenditure));//GroupID contain 1 for Asset
                int SnoExp = 1;
                foreach (DataRow dr in dtExp.Rows)
                {
                    double m_dbal = 0;
                    double m_cbal = 0;

                    // Block for DateTime range selection
                    if (m_ProfitLoss.HasDateRange == true)//When datetime is selected
                        Transaction.GetGroupBalance(m_ProfitLoss.FromDate, m_ProfitLoss.ToDate, Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                    else//Otherwise
                        //Transaction.GetGroupBalance(Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                        Transaction.GetGroupBalance(Convert.ToDateTime("01 / 01 / 1900"), Convert.ToDateTime("01 / 01 / 1900"), Convert.ToInt32(dr["GroupID"]), true, ref m_dbal, ref m_cbal, m_ProfitLoss.AccClassID, m_ProfitLoss.ProjectID);
                    string EngName1 = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());
                    if (m_ProfitLoss.ShowZeroBalance == false && m_dbal == 0 && m_cbal == 0) //In case of zero balance
                    {
                        //Do nothing
                    }
                    else
                    {
                        string EngName = AccountGroup.GetEngName(Convert.ToInt32(dr["GroupID"]).ToString());  //calling this funtion for getting EngName from tblGroup corresponding to GroupID having corresponding GroupID
                        //double Balance = (m_cbal - m_dbal);//For Asset[Debit Balance - Credit Balance]
                        double Balance = (m_dbal - m_cbal);
                        WriteExpenditure(SnoExp, EngName, Convert.ToInt32(dr["GroupID"]), Balance.ToString(), "GROUP");
                        ExpenditureSum += Balance;

                        //If details is selected, show details i.e. ledgers present inside
                        if (m_ProfitLoss.Detail == true)
                            WriteDetails(Convert.ToInt32(dr["GroupID"]), AccountType.Expenditure,false);

                        //If Second level group is selected, show them
                        if (m_ProfitLoss.ShowSecondLevelDtl == true)
                            WriteSecondLevel(Convert.ToInt32(dr["GroupID"]), AccountType.Expenditure,false);

                    }//End of zero balance check
                    SnoExp++;
                }//End for loop

                //Display the ledger just under the account
                WriteLedger(AccountGroup.GetIDFromType( AccountType.Expenditure), AccountType.Expenditure, ref incomebal, ref expensebal,true,false);
                ExpenditureSum += expensebal;
                #endregion

                double ProductValue1 = 0;
               // Product allproduct = new Product();
              //  DataTable dtGetAllProduct = allproduct.getProductByGroupID();
               // string AccClassIDsXMLString = ReadAllAccClassID();
               // string ProjectIDsXMLString = ReadAllProjectID();
                int closingQuantity1 = 0;
                foreach (DataRow drProduct in dtGetAllProduct.Rows)
                {
                    DataTable dtOpeningStockStatusInfo1 = StockStatusBook.GetOpeningStockStatusBook(null, Convert.ToInt32(drProduct["ProductID"].ToString()), " ", m_ProfitLoss.ToDate, true, StockStatusType.OpeningStock, AccClassIDsXMLString);
                    if (dtOpeningStockStatusInfo1.Rows.Count > 0)
                    {
                        DataRow dropen = dtOpeningStockStatusInfo1.Rows[0];
                        closingQuantity1 = Convert.ToInt32(dropen["Quantity"].ToString());
                        double ProdPrice = Product.GetProductPurchasePrice(Convert.ToInt32(drProduct["ProductID"].ToString()), Global.ParentAccClassID);
                        ProductValue1 += ProdPrice * closingQuantity1;
                    }
                }
                //if (IsCrystalReport)//Crystal report is only shown according to Vertical format
                //{
                //    WriteProfitLossOnCrystalRpt(0, "Expenditure", "Opening Stock", ProductValue1.ToString(), "Group");
                //    ExpenditureSum += ProductValue1;
                //}
                //else
                //{
                    WriteExpenditure(SnoExp, "Opening Stock ", 0, ProductValue1.ToString(), "GROUP");
                    ExpenditureSum += ProductValue1;
                //}
                #region DIFFERENCE IN OPENENING BALANCE
                //Display Difference in Opening Balance
                double DrOpBal=0;
                double CrOpBal=0;
                Transaction.GetOpeningBalanceFromGroup(AccountGroup.GetIDFromType(AccountType.Income), m_ProfitLoss.AccClassID, ref DrOpBal, ref CrOpBal);
                double OpBalIncome = CrOpBal - DrOpBal;
                

                //Display Difference in Opening Balance
                Transaction.GetOpeningBalanceFromGroup(AccountGroup.GetIDFromType(AccountType.Expenditure), m_ProfitLoss.AccClassID, ref DrOpBal, ref CrOpBal);
                double OpBalExpenditue = DrOpBal - CrOpBal;
                if (OpBalIncome > OpBalExpenditue)
                {
                    double OpBalDiff = OpBalIncome - OpBalExpenditue;

                    IncomeSum += OpBalDiff;

                    WriteIncome(0, "Difference in Opening Balance", 0, OpBalDiff.ToString(), "GROUP");
                }
                else if (OpBalIncome < OpBalExpenditue)
                {
                    double OpBalDiff = OpBalExpenditue - OpBalIncome;
                    ExpenditureSum = +OpBalDiff;
                    WriteExpenditure(0, "Difference in Opening Balance", 0, OpBalDiff.ToString(), "GROUP");
                }

                #endregion

                ProgressForm.UpdateProgress(80, "Calculating difference...");

                //write code for wheter it is Profit or Loss??? IF Profit then add this balance to Expenditure side and if loss the add this balance to Income side
                if (IncomeSum > ExpenditureSum)
                {
                    WriteExpenditure(0, "Total Profit: ", 0, (IncomeSum - ExpenditureSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
                    ExpenditureSum += (IncomeSum - ExpenditureSum);
                }
                else if(ExpenditureSum>IncomeSum)
                {
                    WriteIncome(0, "Total Loss: ", 0, (ExpenditureSum - IncomeSum).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
                    IncomeSum += (ExpenditureSum - IncomeSum);
                }
                #region BALANCE INCOME AND EXPENDITURE ROWS FOR BLANK CELLS
                //Balance the Income and Expenditue rows using blank cells. Income and Expenditue rows may not be same. So need to insert blank cells
                if (IncomeRowsCount > ExpenditureRowsCount)
                {
                    while (IncomeRowsCount > ExpenditureRowsCount)
                    {
                        grdProfitLossAcc[ExpenditureRowsCount, 5] = new SourceGrid.Cells.Cell("");
                        grdProfitLossAcc[ExpenditureRowsCount, 6] = new SourceGrid.Cells.Cell("");
                        grdProfitLossAcc[ExpenditureRowsCount, 7] = new SourceGrid.Cells.Cell("");
                        grdProfitLossAcc[ExpenditureRowsCount, 8] = new SourceGrid.Cells.Cell("");
                        grdProfitLossAcc[ExpenditureRowsCount, 9] = new SourceGrid.Cells.Cell("");
                        ExpenditureRowsCount++;
                    }
                }
                else if (ExpenditureRowsCount > IncomeRowsCount)
                {
                    while (ExpenditureRowsCount > IncomeRowsCount)
                    {
                        grdProfitLossAcc[IncomeRowsCount, 0] = new SourceGrid.Cells.Cell("");
                        grdProfitLossAcc[IncomeRowsCount, 1] = new SourceGrid.Cells.Cell("");
                        grdProfitLossAcc[IncomeRowsCount, 2] = new SourceGrid.Cells.Cell("");
                        grdProfitLossAcc[IncomeRowsCount, 3] = new SourceGrid.Cells.Cell("");
                        grdProfitLossAcc[IncomeRowsCount, 4] = new SourceGrid.Cells.Cell("");
                        IncomeRowsCount++;
                    }
                }
                #endregion

                #region BLOCK FOR TOTAL AMOUNT CALCULATION FOR INCOME AND EXPENDITURE
                WriteIncome(0, "INCOME TOTAL", 0, IncomeSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
                WriteExpenditure(0, "EXPENDITURE TOTAL", 0, ExpenditureSum.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP");
                #endregion

                ProgressForm.UpdateProgress(100, "Preparing report for display...");

                // Close the dialog if it hasn't been already

                if (ProgressForm.InvokeRequired)
                    ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            }
             #endregion

             #region BLOCK FOR VERTICAL FORMAT
             else if (m_ProfitLoss.DispFormat== ProfitLossAccSettings.DisplayFormat.Vertical)//if format is vertical
             {
                DisplayVerticalProfitLoss(false);
             }
             #endregion

         }

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            //lblCompanyName.Text = m_CompanyDetails.CompanyName;
            //lblCompanyAddress.Text = m_CompanyDetails.Address1 + " " + m_CompanyDetails.City + " " + m_CompanyDetails.District;
            //lblContact.Text = "Contact: " + m_C     ompanyDetails.Telephone;
            //lblPanNo.Text = "PAN No.: " + m_CompanyDetails.PAN;
            //lblWebsite.Text = "Web: " + m_CompanyDetails.Website;
            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_ProfitLoss.ProjectID), LangMgr.DefaultLanguage);

              
            if (m_ProfitLoss.HasDateRange)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_ProfitLoss.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }

            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if(m_CmpDtl.FYFrom != null)
            lblAllSettings.Text = "Fiscal Year: " + Convert.ToDateTime(m_CmpDtl.FYFrom).ToShortDateString();

            if (m_ProfitLoss.ProjectID != 0)
            {        
                DataRow drProjectInfo = dtProjectInfo.Rows[0];
                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
        }

        private void grdProfitLoss_DoubleClick(object sender, EventArgs e)
        {
            int curColumn = grdProfitLossAcc.Selection.GetSelectionRegion().GetColumnsIndex()[0];
            int curRow = grdProfitLossAcc.Selection.GetSelectionRegion().GetRowsIndex()[0];
            string Type = "";
            int groupid, ledgerid;
            groupid = ledgerid = 0;
            if (m_ProfitLoss.DispFormat== ProfitLossAccSettings.DisplayFormat.Vertical)//for vertical ProfitLoss A/C           
            {
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdProfitLossAcc, new SourceGrid.Position(curRow, 4));
                Type = (cellType.Value).ToString();
                if (Type == "GROUP")
                {
                    SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdProfitLossAcc, new SourceGrid.Position(curRow, 3));
                    groupid = Convert.ToInt32(cellid.Value);
                }
                else if (Type == "LEDGER")
                {
                    SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdProfitLossAcc, new SourceGrid.Position(curRow, 3));
                    ledgerid = Convert.ToInt32(cellid.Value);
                }

            }
            else if (m_ProfitLoss.DispFormat== ProfitLossAccSettings.DisplayFormat.TFormat)//For T- format Profitloss A/C
            {
                //find colulmn position at first

                if (curColumn <= 4)//This is the portion of Income side
                {
                    SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdProfitLossAcc, new SourceGrid.Position(curRow, 4));
                    Type = (cellType.Value).ToString();
                    if (Type == "GROUP")
                    {
                        SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdProfitLossAcc, new SourceGrid.Position(curRow, 3));
                        groupid = Convert.ToInt32(cellid.Value);
                    }
                    else if (Type == "LEDGER")
                    {
                        SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdProfitLossAcc, new SourceGrid.Position(curRow, 3));
                        ledgerid = Convert.ToInt32(cellid.Value);
                    }


                }
                else if (curColumn >= 5)//This is the portion of Expenditure
                {
                    SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdProfitLossAcc, new SourceGrid.Position(curRow, 9));
                    Type = (cellType.Value).ToString();
                    if (Type == "GROUP")
                    {
                        SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdProfitLossAcc, new SourceGrid.Position(curRow, 8));
                        groupid = Convert.ToInt32(cellid.Value);
                    }
                    else if (Type == "LEDGER")
                    {
                        SourceGrid.CellContext cellid = new SourceGrid.CellContext(grdProfitLossAcc, new SourceGrid.Position(curRow, 8));
                        ledgerid = Convert.ToInt32(cellid.Value);
                    }
                }

           }

            //Check which type is clicked whether Group or Ledger?
            if ((groupid > 0) || (ledgerid > 0))//Double cick event is only for either Group account or Ledger account ...not for Total amount or Opening balance
            {
                if (Type == "GROUP")// If GroupType is clicked
                {
                    GroupBalanceSettings m_GBS = new GroupBalanceSettings();
                    m_GBS.HasDateRange = m_ProfitLoss.HasDateRange;
                    m_GBS.ShowZeroBalance = m_ProfitLoss.ShowZeroBalance;
                    m_GBS.FromDate = m_ProfitLoss.FromDate;
                    m_GBS.ToDate = m_ProfitLoss.ToDate;
                    m_GBS.AccClassID = m_ProfitLoss.AccClassID;
                    m_GBS.GroupID = groupid;//Store the GroupID value on object which achieve while double clicking the corresponding row of cell
                    frmGroupBalance m_GrpBal = new frmGroupBalance(m_GBS);
                    m_GrpBal.ShowDialog();
                }
                else if (Type == "LEDGER")//If LedgerType is clicked
                {
                    TransactSettings m_TS = new TransactSettings();
                    m_TS.HasDateRange = m_ProfitLoss.HasDateRange;
                    m_TS.FromDate = m_ProfitLoss.FromDate;
                    m_TS.ToDate = m_ProfitLoss.ToDate;
                    m_TS.AccClassID = m_ProfitLoss.AccClassID;
                    m_TS.LedgerID = ledgerid;
                    frmTransaction m_Transact = new frmTransaction(m_TS);
                    m_Transact.ShowDialog();
                }
            }
            
        }

     
        private void btnPrintPreview_Click_1(object sender, EventArgs e)
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

            dsProfitLoss.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed                    
            //otherwise it populate the records again and again

            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            rptProfitLoss rpt = new rptProfitLoss();
            //Fill the logo on the report
            Misc.WriteLogo(dsProfitLoss, "tblImage1");
            //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsProfitLoss);
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

            pdvReport_Head.Value = "Profit And Loss A/C";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FYFrom;
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);
            rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            if(m_ProfitLoss.ToDate!=null)
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem((DateTime)m_ProfitLoss.ToDate);
            else
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem(Date.GetServerDate());
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            //Real programming start from here
            dsProfitLoss.Tables["tblGroup"].Rows.Add(1, "Expenditure");
            dsProfitLoss.Tables["tblGroup"].Rows.Add(2, "Income");
            DisplayVerticalProfitLoss(true);

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
                    frmemail sendemail = new frmemail(FileName, 1);
                    sendemail.ShowDialog();
                    rpt.Close();
                    return;
                default:
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }                     
            frm.WindowState = FormWindowState.Maximized;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows
            grdProfitLossAcc.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmProfitLossAcc_Load(sender, e);

            this.Cursor = Cursors.Default;
            //Show complete notification
            //Global.Msg("Reload Complete!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmProfitLossAcc_KeyDown(object sender, KeyEventArgs e)
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
                    prntDirect = 2;
                    btnPrintPreview_Click_1(sender, e);
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
                    btnPrintPreview_Click_1(sender, e);
                    break;
                case "mnuEmail":
                    //Code for pdf export
                    SaveFileDialog SaveFDExcelEmail = new SaveFileDialog();
                    SaveFDExcelEmail.InitialDirectory = "D:";
                    SaveFDExcelEmail.Title = "Enter Filename:";
                    SaveFDExcelEmail.Filter = "*.xls|*.xls";
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
                    btnPrintPreview_Click_1(sender, e);
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prntDirect = 1;
            btnPrintPreview_Click_1(sender, e);
        }
        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_ProfitLoss.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_ProfitLoss.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in m_ProfitLoss.AccClassID)
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
            Project.GetChildProjects(Convert.ToInt32(m_ProfitLoss.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_ProfitLoss.ProjectID + "'";

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
                    tw.WriteElementString("PID", Convert.ToInt32(m_ProfitLoss.ProjectID).ToString());
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

     
    }
}
