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
using System.Threading;
using DateManager;
using Inventory.Forms;

namespace Inventory
{
    public partial class frmCashFlow : Form
    {
        private DataSet.dsCashFlow dsCashFLow = new DataSet.dsCashFlow();
        Transaction m_Transaction = new Transaction();
        private int CashFlowRowsCount;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
       // ArrayList ProjectIDs = new ArrayList();
        private CashFlowSettings m_CashFlow;
             //Different grid views
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        private int prntDirect = 0;
        private string FileName = "";
        private string ProjectIDS;
        private string TotalAccClassIDs;
        private string TotalInFlow = "";
        private string TotalOutFlow = "";
        int sno = 1;
        //For Export Menu
        ContextMenu Menu_Export;

        public frmCashFlow()
        {
            InitializeComponent();
        }

        public frmCashFlow(CashFlowSettings CashFlow)
        {
            InitializeComponent();
            m_CashFlow = new CashFlowSettings();
            m_CashFlow.Details = CashFlow.Details;
            m_CashFlow.FromDate = CashFlow.FromDate;
            m_CashFlow.ToDate = CashFlow.ToDate;
            m_CashFlow.CashAccID = CashFlow.CashAccID;
            m_CashFlow.Summary = CashFlow.Summary;
            m_CashFlow.AccountWise = CashFlow.AccountWise;
            m_CashFlow.GroupWise = CashFlow.GroupWise;
            m_CashFlow.GroupID = CashFlow.GroupID;
            m_CashFlow.LedgerID = CashFlow.LedgerID;
            m_CashFlow.AccClassID = CashFlow.AccClassID;
            m_CashFlow.ProjectID = CashFlow.ProjectID;
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

        private void MakeHeader()
        {
            //Write header part
            grdCashFlow.Rows.Insert(0);
            grdCashFlow[0, 0] = new MyHeader("S.No.");
            grdCashFlow[0, 1] = new MyHeader("Account Name");
            grdCashFlow[0, 2] = new MyHeader("Inflow Amount");
            grdCashFlow[0, 3] = new MyHeader("OutFlow Amount");
            grdCashFlow[0, 4] = new MyHeader("RowID");
            grdCashFlow[0, 5] = new MyHeader("AccountType");
            grdCashFlow[0, 6] = new MyHeader("GroupID");

            //Define the width of column size
            grdCashFlow[0, 0].Column.Width = 75;
            grdCashFlow[0, 1].Column.Width = 500;
            grdCashFlow[0, 2].Column.Width = 200;
            grdCashFlow[0, 3].Column.Width = 200;
            grdCashFlow[0, 4].Column.Width = 50;
            grdCashFlow[0, 5].Column.Width = 50;
            grdCashFlow[0, 6].Column.Width = 50;

            //Code for making column invisible
            grdCashFlow.Columns[4].Visible = false;// making third column invisible and using it in  programming    
            grdCashFlow.Columns[5].Visible = false;// making third column invisible and using it in  programming  
            grdCashFlow.Columns[6].Visible = false;// making third column invisible and using it in  programming  

        }

        private void WriteCashFlow(int Sno, string Ledger, string Inflow, string Outflow, string ledgerID,string AccountType,string GroupID,bool IsCrystalReport)
        {
            if (!IsCrystalReport)//just to display for grid
            {
                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (CashFlowRowsCount % 2 == 0)
                {
                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                grdCashFlow.Rows.Insert(grdCashFlow.RowsCount);
                string strSNo = (Sno == 0 ? "" : Sno.ToString());//Show blank if sno is 0
                grdCashFlow[CashFlowRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
                grdCashFlow[CashFlowRowsCount, 1] = new SourceGrid.Cells.Cell(Ledger);
                grdCashFlow[CashFlowRowsCount, 2] = new SourceGrid.Cells.Cell(Inflow);
                grdCashFlow[CashFlowRowsCount, 3] = new SourceGrid.Cells.Cell(Outflow);
                grdCashFlow[CashFlowRowsCount, 4] = new SourceGrid.Cells.Cell(ledgerID);
                grdCashFlow[CashFlowRowsCount, 5] = new SourceGrid.Cells.Cell(AccountType);
                grdCashFlow[CashFlowRowsCount, 6] = new SourceGrid.Cells.Cell(GroupID);
     

                //To store the current view types accourding to the row type(Ledger, Group etc)
                SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                switch (AccountType)
                {
                    case "GROUP":
                        CurrentView = GroupView;
                        break;
                    case "LEDGER":
                        CurrentView = LedgerView;
                        break;
                    default:
                        CurrentView = GroupView; //Because it is the normal formatting without any makeups
                        break;

                }

                grdCashFlow[CashFlowRowsCount, 0].AddController(dblClick);
                grdCashFlow[CashFlowRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdCashFlow[CashFlowRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdCashFlow[CashFlowRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdCashFlow[CashFlowRowsCount, 1].AddController(dblClick);
                grdCashFlow[CashFlowRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdCashFlow[CashFlowRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdCashFlow[CashFlowRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdCashFlow[CashFlowRowsCount, 2].AddController(dblClick);
                grdCashFlow[CashFlowRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdCashFlow[CashFlowRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdCashFlow[CashFlowRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdCashFlow[CashFlowRowsCount, 3].AddController(dblClick);
                grdCashFlow[CashFlowRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdCashFlow[CashFlowRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdCashFlow[CashFlowRowsCount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                CashFlowRowsCount++;
            }
            else if (IsCrystalReport)//just to display for crystal report
            {
                if (Ledger == "TOTAL INFLOW AND OUTFLOW:")
                {
                    TotalInFlow = Inflow;
                    TotalOutFlow = Outflow;
                }
                else
                    dsCashFLow.Tables["tblCashFLow"].Rows.Add(Ledger, Inflow, Outflow);
            }
        }

        private bool IsLedgerFallsInCashBank(ArrayList tmpCashBankLedgers,int LedgerID)
        {
            foreach (int CashBankLedger in tmpCashBankLedgers)
            {
                if (CashBankLedger == LedgerID)
                {
                    return true;//only return true when particular ledger falls under CashBank
                }
            }
            return false;//Only return false when particular ledger doesnot fall under CashBank
        }

        /// <summary>
        /// This method returns Cashflow result after calculation
        /// </summary>
        /// <param name="SelfLedgerExceptCashBank"></param>
        /// <param name="dtRowID"></param>
        /// <param name="VoucherType"></param>
        /// <param name="tmpCashBankLedgers"></param>
        /// <param name="m_CashInflowAmt"></param>
        /// <param name="totalCashOutflow"></param>
        /// <param name="ledgerID"></param>
        private void CalculateCashflow(string SelfLedgerExceptCashBank, DataTable dtRowID, string VoucherType, ArrayList tmpCashBankLedgers,ref double  totalCashInflow,ref double totalCashOutflow)
        {
            bool isCashBankDr = false;
            bool isSelfLedgerDr = false;
            double selfLedgerDrAmt = 0;
            double selfLedgerCrAmt = 0;
            foreach (DataRow drRowID in dtRowID.Rows)//Loop of RowIDs of particular LedgerExceptCashBank
            {
               DataTable dtCashPmtTransInfo = m_Transaction.GetTransactionInfo(drRowID["RowID"].ToString(), VoucherType,m_CashFlow.AccClassID);
                foreach (DataRow drCashPmtTransInfo in dtCashPmtTransInfo.Rows)//This the loop of Particular RowID with corresponding Vouchertype
                {

                    //Checking Particular LedgerID falls under CashBank Arraylist???
                    if (tmpCashBankLedgers.Contains(drCashPmtTransInfo["LedgerID"]))//this is a cash/bank ledger
                    {
                        //check if this ledger is debit or credit
                        if (Convert.ToDouble(drCashPmtTransInfo["Debit_Amount"]) > 0)
                        {
                            isCashBankDr = true;
                        }
                    }
                    //This block check the Self ledger is in either in Debitside or Creditside
                    if (SelfLedgerExceptCashBank == drCashPmtTransInfo["LedgerID"].ToString())//Checking the status of Self Ledger
                    {
                        //If self ledger falls in Debitside
                        if (Convert.ToDouble(drCashPmtTransInfo["Debit_Amount"]) > 0)
                        {
                            isSelfLedgerDr = true;//making this variable for checking purpose...whether Self ledger and CashBank Ledgers are on same side or not???                                          
                        }
                        //we have to post the self ledger Debit or credit value on grid soo this purpose ,assigning this value in corresponding variable for writing on grid
                        selfLedgerDrAmt = Convert.ToDouble(drCashPmtTransInfo["Debit_Amount"]);
                           selfLedgerCrAmt = Convert.ToDouble(drCashPmtTransInfo["Credit_Amount"]);
                    }

                }

                //After finishing the loop of Particular block of row then check about whether Self ledger and CashBank ledger falls on same side or not???
                if (isCashBankDr != isSelfLedgerDr)  //They both doesnt fall in same column
                {
                    if (isSelfLedgerDr == true)//Self Ledger is Debit...It means Cashoutflow
                    {
                        totalCashOutflow += selfLedgerDrAmt;//assigning DebitAmount onto CashOutflow
                        totalCashInflow = 0;

                    }
                    else//Self Ledger is Credit...it means cashinflow
                    {
                        totalCashInflow += selfLedgerCrAmt;
                        totalCashOutflow = 0;
                    }

                }
            }               
        }

        /// <summary>
        /// 
        /// This is the method to find the cash flow according to account type...if account type is Group-wise it will just give the information of total inflow and outflow bt if 
        /// //account type is Account Head-wise..it also writes cashflow amount to its correspodnding field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        //

        private void GetCashflowByAccType(int m_GroupID, ref double m_GrandTotalCashInflow, ref double m_GrandTotalCashOutflow, bool iSAccTypGroup,DataTable  dtGrpLedgerDtl,bool IsCrystalReport)
        {
            #region BLOCK FOR COLLECTING ALL CASH BANK LEDGERS
            ArrayList tmpCashBankLedgers = new ArrayList();//Collecting All Cash and bank Ledgers in Temporary Arraylist
            int CashGrpID = AccountGroup.GetGroupIDFromGroupNumber(102);
            int BankGrpID = AccountGroup.GetGroupIDFromGroupNumber(7);//Banks
            DataTable dtCashLedgers = Ledger.GetAllLedger(CashGrpID);//Collecting all ledgers falls under Cash Group Account
            DataTable dtBankLedgers = Ledger.GetAllLedger(BankGrpID);
            foreach (DataRow drCashLedgers in dtCashLedgers.Rows)//Adding All Cash LedgersID in Temporary Arraylist
            {
                tmpCashBankLedgers.Add(Convert.ToInt32(drCashLedgers["LedgerID"]));
            }
            foreach (DataRow drBankLedgers in dtBankLedgers.Rows)//Adding all Bank LedgersID in Temporary Arraylist
            {
                tmpCashBankLedgers.Add(Convert.ToInt32(drBankLedgers["LedgerID"]));
            }
            string strCashBankLedgers = "";
            int i = 0;
            foreach (object j in tmpCashBankLedgers)
            {
                if (i == 0)// for first LedgerID
                    strCashBankLedgers = "'" + j.ToString() + "'";
                else  //Separating Other LedgerID by commas
                    strCashBankLedgers += "," + "'" + j.ToString() + "'";
                i++;
            }
            #endregion
          
            DataTable dtAllLedgerExceptCashBank = new DataTable(); //It holds the all ledgers except cash bank from Transaction table according to condition
            if(!iSAccTypGroup)//This section if account type is according to Account Head-wise
            {
               if(dtGrpLedgerDtl.Rows.Count>0)//TO show the Cashflow of end Account group which have direct Ledgers under this group
               {
                   dtAllLedgerExceptCashBank = Transaction.GetAllLedgersExeptCashBank(strCashBankLedgers, dtGrpLedgerDtl);
               }
               else if(dtGrpLedgerDtl.Rows.Count<=0)//To show the cashflow according to Account Headwise,Which contains all ledgers except cashbank from transaction table
               {
                   dtAllLedgerExceptCashBank = Transaction.GetAllLedgersExeptCashBank(strCashBankLedgers, dtGrpLedgerDtl);//This table contains all ledgers excepts CashBank
               }               
            }
            else if(iSAccTypGroup)//This section is for if Account Type is according to Groupwise where it doesnot have direct ledgers under this group
            {
                //To show the cash flow according to groupwise...we have to find out the all ledgers associated with this group
                //Group may contain sub group too... soo,we have to consider this part too
                //Making the method which take GroupID as input and collect sub groupID of corresponding GroupID and ultimately find out the All Ledgers excepet CashBank associated with this group using proper query
                dtAllLedgerExceptCashBank = CashFlow.GetAllLedgerExceptCashBankByGroupID(m_GroupID, strCashBankLedgers);                                
            }             
                foreach (DataRow drAllLedgerExceptCashBank in dtAllLedgerExceptCashBank.Rows)//All Ledgers excepts CashBank Ledgers
                {
                    double m_TotalCashInflowAmt = 0;
                    double m_TotalCashOutflowAmt = 0;
                   
                    //with help of ledgerid collect each vouchertype of corresponding ledgerid
                    DataTable dtLedgerTransactInfo = Transaction.GetDistinctVouTypeFromTransaction(drAllLedgerExceptCashBank["LedgerID"].ToString());
                    foreach (DataRow drLedgerTransactInfo in dtLedgerTransactInfo.Rows)
                    {
                        double cashInflowAmt = 0;
                        double cashOutflowAmt = 0;
                        string VoucherType = drLedgerTransactInfo["VoucherType"].ToString();   //Hold the VoucherType                                                           
                        switch (VoucherType)//Using Switch case for each voucher
                        {
                            case "CASH_RCPT":
                                DataTable dtCashPmtRowID = new DataTable();//This datatable hold the RowID of Particular Ledger which doesnot belongs to Cashbank and also according to particular VoucherType
                                //dtCashPmtRowID = Transaction.GetCashFlowTransactInfo(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers);
                                dtCashPmtRowID = Transaction.GetCashFlowTransactInfoByAccClassAndProjectID(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers, TotalAccClassIDs, ProjectIDS);
                                CalculateCashflow(drAllLedgerExceptCashBank["LedgerID"].ToString(), dtCashPmtRowID, VoucherType, tmpCashBankLedgers, ref cashInflowAmt, ref cashOutflowAmt);
                                m_TotalCashInflowAmt += cashInflowAmt;
                                m_TotalCashOutflowAmt += cashOutflowAmt;
                                break;
                            case "CASH_PMNT":
                                DataTable dtCashRcptRowID = new DataTable();
                                //dtCashRcptRowID = Transaction.GetCashFlowTransactInfo(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers);
                                dtCashRcptRowID = Transaction.GetCashFlowTransactInfoByAccClassAndProjectID(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers, TotalAccClassIDs, ProjectIDS);
                                CalculateCashflow(drAllLedgerExceptCashBank["LedgerID"].ToString(), dtCashRcptRowID, VoucherType, tmpCashBankLedgers, ref cashInflowAmt, ref cashOutflowAmt);
                                m_TotalCashInflowAmt += cashInflowAmt;
                                m_TotalCashOutflowAmt += cashOutflowAmt;
                                break;
                            case "JRNL":
                                DataTable dtJnlRowID = new DataTable();
                               // dtJnlRowID = Transaction.GetCashFlowTransactInfo(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers);
                                dtJnlRowID = Transaction.GetCashFlowTransactInfoByAccClassAndProjectID(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers, TotalAccClassIDs, ProjectIDS);
                                CalculateCashflow(drAllLedgerExceptCashBank["LedgerID"].ToString(), dtJnlRowID, VoucherType, tmpCashBankLedgers, ref cashInflowAmt, ref cashOutflowAmt);
                                m_TotalCashInflowAmt += cashInflowAmt;
                                m_TotalCashOutflowAmt += cashOutflowAmt;
                                break;
                            case "BANK_RCPT":
                                DataTable dtBankRcptRowID = new DataTable();
                                //dtBankRcptRowID = Transaction.GetCashFlowTransactInfo(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers);
                                dtBankRcptRowID = Transaction.GetCashFlowTransactInfoByAccClassAndProjectID(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers, TotalAccClassIDs, ProjectIDS);
                                CalculateCashflow(drAllLedgerExceptCashBank["LedgerID"].ToString(), dtBankRcptRowID, VoucherType, tmpCashBankLedgers, ref cashInflowAmt, ref cashOutflowAmt);
                                m_TotalCashInflowAmt += cashInflowAmt;
                                m_TotalCashOutflowAmt += cashOutflowAmt;
                                break;
                            case "BANK_PMNT":
                                DataTable dtBankPmntRowID = new DataTable();
                               // dtBankPmntRowID = Transaction.GetCashFlowTransactInfo(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers);
                                dtBankPmntRowID = Transaction.GetCashFlowTransactInfoByAccClassAndProjectID(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers, TotalAccClassIDs, ProjectIDS);
                                CalculateCashflow(drAllLedgerExceptCashBank["LedgerID"].ToString(), dtBankPmntRowID, VoucherType, tmpCashBankLedgers, ref cashInflowAmt, ref cashOutflowAmt);
                                m_TotalCashInflowAmt += cashInflowAmt;
                                m_TotalCashOutflowAmt += cashOutflowAmt;
                                break;
                            case "CNTR":
                                DataTable dtCntrRowID = new DataTable();
                                //dtCntrRowID = Transaction.GetCashFlowTransactInfo(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers);
                                dtCntrRowID = Transaction.GetCashFlowTransactInfoByAccClassAndProjectID(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers, TotalAccClassIDs, ProjectIDS);
                                CalculateCashflow(drAllLedgerExceptCashBank["LedgerID"].ToString(), dtCntrRowID, VoucherType, tmpCashBankLedgers, ref cashInflowAmt, ref cashOutflowAmt);
                                m_TotalCashInflowAmt += cashInflowAmt;
                                m_TotalCashOutflowAmt += cashOutflowAmt;
                                break;
                            case "SALES":
                                DataTable dtSalesRowID = new DataTable();
                               // dtSalesRowID = Transaction.GetCashFlowTransactInfo(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers);
                                dtSalesRowID = Transaction.GetCashFlowTransactInfoByAccClassAndProjectID(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers, TotalAccClassIDs, ProjectIDS);
                                CalculateCashflow(drAllLedgerExceptCashBank["LedgerID"].ToString(), dtSalesRowID, VoucherType, tmpCashBankLedgers, ref cashInflowAmt, ref cashOutflowAmt);
                                m_TotalCashInflowAmt += cashInflowAmt;
                                m_TotalCashOutflowAmt += cashOutflowAmt;
                                break;
                            case "PURCH":
                                DataTable dtPurchRowID = new DataTable();
                                //dtPurchRowID = Transaction.GetCashFlowTransactInfo(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers);
                                dtPurchRowID = Transaction.GetCashFlowTransactInfoByAccClassAndProjectID(drAllLedgerExceptCashBank["LedgerID"].ToString(), m_CashFlow.FromDate, m_CashFlow.ToDate, VoucherType, strCashBankLedgers, TotalAccClassIDs, ProjectIDS);
                                CalculateCashflow(drAllLedgerExceptCashBank["LedgerID"].ToString(), dtPurchRowID, VoucherType, tmpCashBankLedgers, ref cashInflowAmt, ref cashOutflowAmt);
                                m_TotalCashInflowAmt += cashInflowAmt;
                                m_TotalCashOutflowAmt += cashOutflowAmt;
                                break;
                        }
                    }
                    DataTable dtLedgerName = Ledger.GetLedgerInfo(Convert.ToInt32(drAllLedgerExceptCashBank["LedgerID"]), LangMgr.DefaultLanguage);
                    if (!iSAccTypGroup)//IF cash flow is according to Account Head-wise
                    {
                       
                        foreach (DataRow drLedgerName in dtLedgerName.Rows)
                        {
                            if (m_TotalCashInflowAmt > 0 || m_TotalCashOutflowAmt > 0)
                            {
                                WriteCashFlow(sno, drLedgerName["LedName"].ToString(), m_TotalCashInflowAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_TotalCashOutflowAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (drAllLedgerExceptCashBank["LedgerID"].ToString()), "LEDGER", "", IsCrystalReport);
                                sno++;
                            }
                        }
                    }
                    //Write Total Amount on the end of grid
                    m_GrandTotalCashInflow += m_TotalCashInflowAmt;
                    m_GrandTotalCashOutflow += m_TotalCashOutflowAmt;
                }
        }

        private void DisplayCashFlow(bool IsCrystalReport)
        {                     
           
            if(!IsCrystalReport)
            {
                CashFlowRowsCount = 1;
                grdCashFlow.Redim(1, 7);
                grdCashFlow.FixedRows = 1;
                MakeHeader();  //For Header part 
            }
      
            #region BLOCK FOR DETAILS CASH FLOW
            if (m_CashFlow.Details)//Incase of Details Cash Flow
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

                //There is two section in details part...a)according to account head-wise b)Group-wise
                //according to Group-wise
                //********ACCORDING TO GROUP-WISE***************
                DataTable dtGrpLedgerDtls = new DataTable();
                if (m_CashFlow.GroupWise)//cash flow according to Group-wise
                {
                    ProgressForm.UpdateProgress(40, "Preparing groupwise..."); 
                    DataTable dtAccGrp = AccountGroup.GetGroupTable(m_CashFlow.GroupID);//Collecting all Parent Group  
                    double GrandCashInflow, GrandCashOutflow;
                    GrandCashInflow = GrandCashOutflow = 0;
                    foreach (DataRow drAccGrp in dtAccGrp.Rows)
                    {
                        double m_TotalCashInflow, m_TotalCashOutflow;
                        m_TotalCashInflow = m_TotalCashOutflow = 0;
                        GetCashflowByAccType(Convert.ToInt32(drAccGrp["GroupID"]), ref m_TotalCashInflow, ref m_TotalCashOutflow, true, dtGrpLedgerDtls,IsCrystalReport);
                        GrandCashInflow += m_TotalCashInflow;
                        GrandCashOutflow += m_TotalCashOutflow;
                        if (m_TotalCashInflow > 0 || m_TotalCashOutflow > 0)//only show thoes type of account group which have inflow or outlfow otherwise just ignore
                        {
                            WriteCashFlow(sno, drAccGrp["EngName"].ToString(), m_TotalCashInflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_TotalCashOutflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "GROUP", drAccGrp["GroupID"].ToString(), IsCrystalReport);
                            sno++;
                        }
                    }

                    //show  direct Ledgers under end group
                    dtGrpLedgerDtls = Ledger.GetLedgerTable(Convert.ToInt32(m_CashFlow.GroupID));
                    if (dtGrpLedgerDtls.Rows.Count > 0)//When Account Group have direct ledgers,show its ledgers and its corresponding cash flow and cash outflow and total too.
                    {
                        double m_GrandTotalCashInflow, m_GrandTotalCashOutflow;
                        m_GrandTotalCashInflow = m_GrandTotalCashOutflow = 0;
                        GetCashflowByAccType(-1, ref m_GrandTotalCashInflow, ref m_GrandTotalCashOutflow, false, dtGrpLedgerDtls,IsCrystalReport);//show the cash flow of end group where direct leder consist
                        WriteCashFlow(0, "TOTAL INFLOW AND OUTFLOW:", m_GrandTotalCashInflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_GrandTotalCashOutflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "GROUP", "", IsCrystalReport);
                    }
                    else//Just show the Account Group and its total Inflow and total Outflow amount
                    {
                        WriteCashFlow(0, "TOTAL INFLOW AND OUTFLOW:", GrandCashInflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), GrandCashOutflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "GROUP", "", IsCrystalReport);
                    }
                    ProgressForm.UpdateProgress(100, "Preparing report for display...");
                    if (ProgressForm.InvokeRequired)
                        ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
                }
                else if (m_CashFlow.AccountWise)//Showing cashflow according to *****ACCOUNT HEAD-WISE****
                {
                    ProgressForm.UpdateProgress(40, "Preparing groupwise...");
                    double m_GrandTotalCashInflow, m_GrandTotalCashOutflow;
                    m_GrandTotalCashInflow = m_GrandTotalCashOutflow = 0;
                    GetCashflowByAccType(-1, ref m_GrandTotalCashInflow, ref m_GrandTotalCashOutflow, false, dtGrpLedgerDtls,IsCrystalReport);
                    WriteCashFlow(0, "TOTAL INFLOW AND OUTFLOW:", m_GrandTotalCashInflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_GrandTotalCashOutflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "GROUP", "", IsCrystalReport);
                    ProgressForm.UpdateProgress(100, "Preparing report for display...");
                    if (ProgressForm.InvokeRequired)
                        ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
                }
            }
            #endregion

            #region BLOCK FOR SUMMARY CASH FLOW
            else if (m_CashFlow.Summary)//Block for summary Cashflow
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

                DataTable dtGrpLedgerDtls = new DataTable();
                DataTable dtAccGrp = AccountGroup.GetGroupTable(m_CashFlow.GroupID);//Collecting all Parent Group  
                double m_GrandTotalCashInflow, m_GrandTotalCashOutflow;
                m_GrandTotalCashInflow = m_GrandTotalCashOutflow = 0;

                //Update the progressbar
                ProgressForm.UpdateProgress(40, "Managing groupwise...");

                foreach (DataRow drAccGrp in dtAccGrp.Rows)
                {
                    double m_TotalCashInflow, m_TotalCashOutflow;
                    m_TotalCashInflow = m_TotalCashOutflow = 0;
                    GetCashflowByAccType(Convert.ToInt32(drAccGrp["GroupID"]), ref m_TotalCashInflow, ref m_TotalCashOutflow, true, dtGrpLedgerDtls,IsCrystalReport);
                    m_GrandTotalCashInflow += m_TotalCashInflow;
                    m_GrandTotalCashOutflow += m_TotalCashOutflow;
                }

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Collecting all information...");

                //Write total inflow and outflow on grid
                WriteCashFlow(1, "GRAND TOTAL INFLOW AND OUTFLOW", m_GrandTotalCashInflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), m_GrandTotalCashOutflow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", "", IsCrystalReport);
                ProgressForm.UpdateProgress(100, "Preparing report for display...");
                if (ProgressForm.InvokeRequired)
                    ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            }    
            #endregion
        }

        private void DisplayBannar()
        {
            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                // lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FiscalYear));
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear;
            lblFromDate.Text = Date.ToSystem(m_CashFlow.FromDate);
            lblToDate.Text = Date.ToSystem(m_CashFlow.ToDate);
            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_CashFlow.ProjectID), LangMgr.DefaultLanguage);
            if (m_CashFlow.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All"; 
            }
           
        }
        private void frmCashFlow_Load(object sender, EventArgs e)
        {
             TotalAccClassIDs = "";
            TotalAccClassIDs = "'" + m_CashFlow.AccClassID[0] + "'";
            for (int i = 0; i < m_CashFlow.AccClassID.Count - 1; i++)
            {
                TotalAccClassIDs += "," + "'" + (m_CashFlow.AccClassID[i + 1].ToString()) + "'";
            }

            //For ProjectID

            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_CashFlow.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }
            //Collect all Project  Which parent id is given projectid

            ProjectIDS = "";

            ProjectIDS = "'" + m_CashFlow.ProjectID + "'";

            for (int iproject = 0; iproject < ProjectIDCollection.Count; iproject++)
            {
                ProjectIDS += "," +    "'" + (ProjectIDCollection[iproject].ToString()) + "'";
            }

            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grdCashFlow_DoulbeClick);
                   //Text format for Total
            GroupView = new SourceGrid.Cells.Views.Cell();
            GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            //Text format for Ledgers
            LedgerView = new SourceGrid.Cells.Views.Cell();
            LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
            LedgerView.ForeColor = Color.Blue;
            DisplayBannar();
            DisplayCashFlow(false);
        }

        private void grdCashFlow_DoulbeClick(object sender, EventArgs e)
            {
            //Get the Selected Row   
             CashFlowSettings m_CFS = new CashFlowSettings();
            int CurRow = grdCashFlow.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdCashFlow, new SourceGrid.Position(CurRow, 5));
            string Type = (cellType.DisplayText).ToString();
            if (Type == "GROUP")
            {

                int CurRow1 = grdCashFlow.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellID = new SourceGrid.CellContext(grdCashFlow, new SourceGrid.Position(CurRow1, 6));
                if (cellID.DisplayText != "")
                {

                    GroupBalanceSettings m_GBS = new GroupBalanceSettings();
                    m_CFS.Summary = m_CashFlow.Summary;
                    m_CFS.Details = m_CashFlow.Details;
                    m_CFS.FromDate = m_CashFlow.FromDate;
                    m_CFS.ToDate = m_CashFlow.ToDate;
                    m_CFS.CashAccID = m_CashFlow.CashAccID;
                    m_CFS.AccountWise = m_CashFlow.AccountWise;
                    m_CFS.GroupWise = m_CashFlow.GroupWise;
                    m_CFS.AccClassID = m_CashFlow.AccClassID;
                    m_CFS.GroupID = Convert.ToInt32(cellID.Value);//Store the GroupID value on object which achieve while double clicking the corresponding row of cell
                    frmCashFlow frm = new frmCashFlow(m_CFS);
                    frm.ShowDialog();
                }
            }
            else if (Type == "LEDGER")
            {
                  SourceGrid.CellContext cellID = new SourceGrid.CellContext(grdCashFlow, new SourceGrid.Position(CurRow, 4));
                  if (cellID.Value != "")
                  {
                      TransactSettings m_TS = new TransactSettings();
                      m_TS.FromDate = m_CashFlow.FromDate;
                      m_TS.ToDate = m_CashFlow.ToDate;
                      m_TS.LedgerID = Convert.ToInt32(cellID.Value);
                      m_TS.AccClassID = m_CashFlow.AccClassID;
                      frmTransaction m_Transact = new frmTransaction(m_TS);
                      m_Transact.ShowDialog();
                  }
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

            dsCashFLow.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed
            //otherwise it populate the records again and again
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            rptCashFlow rpt = new rptCashFlow();
            //Fill the logo on the report
            Misc.WriteLogo(dsCashFLow, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsCashFLow);
            DisplayCashFlow(true);
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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvInFlow = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvOutFlow = new CrystalDecisions.Shared.ParameterDiscreteValue();
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

            pdvReport_Head.Value = "Cash Flow";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

        
            pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);
            rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

           // pdvReport_Date.Value = "As On Date:" + m_CashFlow.ToDate.ToString("yyyy/MM/dd");
            pdvReport_Date.Value = "As On Date:" +Date.ToSystem( m_CashFlow.ToDate);
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            pdvInFlow.Value = TotalInFlow;
            pvCollection.Clear();
            pvCollection.Add(pdvInFlow);
            rpt.DataDefinition.ParameterFields["InFlow"].ApplyCurrentValues(pvCollection);

            pdvOutFlow.Value = TotalOutFlow;
            pvCollection.Clear();
            pvCollection.Add(pdvOutFlow);
            rpt.DataDefinition.ParameterFields["OutFlow"].ApplyCurrentValues(pvCollection);

            //DisplayCashFlow(true);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

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
            grdCashFlow.Redim(0, 0);
            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmCashFlow_Load(sender, e);
            this.Cursor = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCashFlow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void grdCashFlow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                grdCashFlow_DoulbeClick(sender, null);
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
                    btnPrintPreview_Click(sender, e);
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
                    prntDirect = 4;
                    btnPrintPreview_Click(sender, e);
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

    }
}

     


