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
using Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DateManager;
using System.Threading;
using System.IO;
using System.Data.SqlClient;
using BusinessLogic.Accounting.Reports;
using Accounts.Reports;
using BusinessLogic;



namespace Accounts
{
    public partial class frmTransaction : Form
    {

        private int? LedgerID = null;
        private int? FromDate = null;
        private int? ToDate = null;
        private string LedgerName = "";
        private enum AccountType
        {
            ALL_LEDGERS, PARTICULAR_LEDGER, PARTICULAR_GROUP

        }
        private Accounts.Model.dsTransaction dsTransaction = new Model.dsTransaction();
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for Transaction
        private double TotalDebitAmount = 0;
        private double TotalCreditAmount = 0;
        private int TransactRowsCount;
        DataTable dtLedgerGridData;
        DataTable dtTemp;

        private TransactSettings m_TS;
        string m_AccountType = "";
        Transaction m_Transaction = new Transaction();
        //Different grid views
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        private SourceGrid.Cells.Views.Cell LedgerHeadView;
        ArrayList AccClassID = new ArrayList();
        private int prntDirect = 0;
        private string FileName = "";
        string ProjectIDsXMLString = "";
        //For Export Menu
        ContextMenu Menu_Export;
        private IMDIMainForm m_MDIForm;


        public frmTransaction()
        {
            InitializeComponent();
        }
        public frmTransaction(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;

        }
        public frmTransaction(TransactSettings TS)
        {
            try
            {
                //This is the constructor which handles the transaction of particular Ledger only
                InitializeComponent();
                m_TS = new TransactSettings();
                m_TS.LedgerID = TS.LedgerID;
                m_AccountType = "PARTICULAR_LEDGER";//Set account type as Particular Ledger for generating information of Particular Ledger
                m_TS.FromDate = TS.FromDate;
                m_TS.ToDate = TS.ToDate;
                m_TS.HasDateRange = TS.HasDateRange;
                m_TS.AccClassID = TS.AccClassID;
                m_TS.ProjectID = TS.ProjectID;
                m_TS.ShowRemarks = TS.ShowRemarks;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public frmTransaction(TransactSettings TS, IMDIMainForm frmMDI)
        {
            try
            {
                //This is the constructor which handles the transaction of particular Ledger only
                InitializeComponent();
                m_TS = new TransactSettings();
                m_TS.LedgerID = TS.LedgerID;
                m_AccountType = "PARTICULAR_LEDGER";//Set account type as Particular Ledger for generating information of Particular Ledger
                m_TS.FromDate = TS.FromDate;
                m_TS.ToDate = TS.ToDate;
                m_TS.HasDateRange = TS.HasDateRange;
                m_TS.AccClassID = TS.AccClassID;
                m_TS.ProjectID = TS.ProjectID;
                m_TS.ShowRemarks = TS.ShowRemarks;

                m_MDIForm = frmMDI;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //This is  constructor for the AccountLedger setting form
        public frmTransaction(TransactSettings TS, string AccountType, IMDIMainForm frmMDI)
        {
            try
            {
                InitializeComponent();
                m_TS = new TransactSettings();
                if (AccountType == "ALL_LEDGERS")//If all ledgers the simply pass the 0 ID for LedgerID
                {
                    m_TS.LedgerID = 0;//If 0 ledgerID is passed then simply we understand it call thoes method we concern with all ledgers
                    m_AccountType = AccountType;
                    m_TS.AccClassID = TS.AccClassID;
                }
                else//For particular Group Account
                {
                    m_TS.AccountGroupID = TS.AccountGroupID;
                    m_AccountType = AccountType;
                }
                m_TS.AccClassID = TS.AccClassID;
                m_TS.FromDate = TS.FromDate;
                m_TS.ToDate = TS.ToDate;
                m_TS.HasDateRange = TS.HasDateRange;
                m_TS.ProjectID = TS.ProjectID;
                m_TS.ShowRemarks = TS.ShowRemarks;
                m_TS.ShowZeroBalance = TS.ShowZeroBalance;

                m_MDIForm = frmMDI;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        /// <summary>
        /// Gets the selected root accounting class ID
        /// </summary>
        /// <returns></returns>
        private int GetRootAccClassID()
        {
            if (m_TS.AccClassID.Count > 0)
            {
                //Find Root Class  
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(m_TS.AccClassID[0]));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);
            }
            return 1;//The default root class ID
        }


        private void CreateColumnsforTempTable()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("S.N", typeof(string));
            dtTemp.Columns.Add("LedgerName", typeof(string));
            dtTemp.Columns.Add("LedgerID", typeof(string));
            dtTemp.Columns.Add("TransactDate", typeof(string));
            dtTemp.Columns.Add("VoucherName", typeof(string));
            dtTemp.Columns.Add("DrBal", typeof(string));
            dtTemp.Columns.Add("CrBal", typeof(string));
            dtTemp.Columns.Add("VoucherType", typeof(string));
            dtTemp.Columns.Add("Balance", typeof(string));
            dtTemp.Columns.Add("RowID", typeof(string));


        }
       
        private void CreateData(string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            decimal DrTotal;
            decimal CrTotal;
            string Balance = "";
            decimal DrSumTotal = 0;
            decimal CrSumTotal = 0;
            string date = " ";
            // int scrol = 0;
            int uid = User.CurrUserID;
           string datepref=UserPreference.GetValue("DEFAULT_DATE", uid); 


            DrTotal = CrTotal = 0;
            // DataTable dtLedgerInfo = Ledger.GetLedgerIDnName(m_TS.LedgerID);
            DataTable dtLedgerInfo = Ledger.GetLedgerOpenBal(AccClassIDsXMLString, m_TS.LedgerID, m_TS.AccountGroupID,m_TS.FromDate);

            //pg_AsyncLoad.Maximum = dtLedgerInfo.Rows.Count;

            if (dtLedgerInfo.Rows.Count > 0)
            {
           
                foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                {
                    DrTotal = CrTotal = 0;
                    dtTemp.Rows.Add(" ", " ", " ", " ", drLedgerInfo["LedgerName"].ToString(), " ", " ", " ", " ", " ");
                    if (Convert.ToInt32(drLedgerInfo["OpenBal"]) > 0)
                    {
                        if (drLedgerInfo["OpenBalDrCr"].ToString() == "DEBIT")
                        {
                            dtTemp.Rows.Add("-", "Opening Balance", " ",  drLedgerInfo["OpenBalDate"].ToString(), " ", " ", " ", " ", Convert.ToDecimal(drLedgerInfo["OpenBal"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr)", " ");
                          //  DrSumTotal += Convert.ToDecimal(drLedgerInfo["OpenBal"]);
                            DrTotal += Convert.ToDecimal(drLedgerInfo["OpenBal"]);
                        }
                        else if (drLedgerInfo["OpenBalDrCr"].ToString() == "CREDIT")
                        {
                            dtTemp.Rows.Add("-", "Opening Balance", " ",  drLedgerInfo["OpenBalDate"].ToString(), " ", " ", " ", " ", Convert.ToDecimal(drLedgerInfo["OpenBal"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr)", " ");
                         //   CrSumTotal += Convert.ToDecimal(drLedgerInfo["OpenBal"]);
                            CrTotal += Convert.ToDecimal(drLedgerInfo["OpenBal"]);

                        }
                        else
                        {
                            Global.Msg("Opening balance Dr Cr not match");
                        }
                    }

                    dtLedgerGridData = Transaction.GetLedgerTransaction(Convert.ToInt32(drLedgerInfo["LedgerID"]), AccClassIDsXMLString, m_TS.FromDate, m_TS.ToDate, ProjectIDsXMLString);
                    if (dtLedgerGridData.Rows.Count > 0)
                    {
                        //new change
                        for (int i = 0; i < dtLedgerGridData.Rows.Count; i++)
                        {

                            Balance = "";
                            DrTotal += Convert.ToDecimal(dtLedgerGridData.Rows[i][3]);
                            CrTotal += Convert.ToDecimal(dtLedgerGridData.Rows[i][4]);

                            if (DrTotal > CrTotal)
                                Balance = ((DrTotal - CrTotal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr)");
                            else
                                Balance = ((CrTotal - DrTotal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr)");
                            dtTemp.Rows.Add((i + 1).ToString(), dtLedgerGridData.Rows[i]["Account"].ToString(), dtLedgerGridData.Rows[i]["LedgerID"].ToString(), dtLedgerGridData.Rows[i]["NepLedgerDate"].ToString(), dtLedgerGridData.Rows[i]["VoucherNumber"].ToString(), (Convert.ToDecimal(dtLedgerGridData.Rows[i]["Debit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Convert.ToDecimal(dtLedgerGridData.Rows[i]["Credit"])).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dtLedgerGridData.Rows[i]["VoucherType"].ToString(), Balance.ToString(), dtLedgerGridData.Rows[i]["RowID"].ToString());

                        }
                        dtTemp.Rows.Add("-", "Total Amount", " ", " ", " ", (Convert.ToDecimal(DrTotal)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Convert.ToDecimal(CrTotal)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), " ", " ", " ");
                        dtTemp.Rows.Add("-", "Closing Balance", " ", " ", " ", " ", " ", " ", Balance.ToString(), " ");
                        DrSumTotal += DrTotal;
                        CrSumTotal += CrTotal;

                    }
                }
                // dataGrid.Columns.AutoSizeView();
            }
            lblTotalDebitAmount.Text = DrSumTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            lblTotalCreditAmount.Text = CrSumTotal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));


        }

        private void CreateColumns(DevAge.ComponentModel.IBoundList boundList)
        {
            SourceGrid.Cells.Views.ColumnHeader TranViewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            TranViewColumnHeader.Font = new Font(dgTransaction.Font, FontStyle.Bold);
            TranViewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;


            //Create the views
            SourceGrid.Cells.Views.Cell TranldgHead = new SourceGrid.Cells.Views.Cell();
            TranldgHead.Font = new Font(dgTransaction.Font, FontStyle.Bold);
            TranldgHead.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightGray);
            TranldgHead.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            TranldgHead.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            TranldgHead.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            TranldgHead.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);


            //Text format for Ledgers
            SourceGrid.Cells.Views.Cell TranldgView = new SourceGrid.Cells.Views.Cell();
            TranldgView.Font = new Font(LangMgr.GetFont(), FontStyle.Regular);
            TranldgView.ForeColor = Color.Blue;

            SourceGrid.Cells.Views.Cell TrancBalanceView = new SourceGrid.Cells.Views.Cell();
            TrancBalanceView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
            TrancBalanceView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


            SourceGrid.Cells.Views.Cell TranviewNumeric = new SourceGrid.Cells.Views.Cell();
            TranviewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            //Create selected conditions
            SourceGrid.Conditions.ConditionView Tcon_ldgHead = new SourceGrid.Conditions.ConditionView(TranldgHead);
            Tcon_ldgHead.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    DataRowView row = (DataRowView)itemRow;
                    return (string)row["S.N"].ToString() == " " && row["S.N"] != null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            SourceGrid.Conditions.ConditionView Tcon_LdgView = new SourceGrid.Conditions.ConditionView(TranldgView);
            Tcon_LdgView.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    DataRowView row = (DataRowView)itemRow;
                    return (string)row["S.N"] != " " && (string)row["S.N"] != "-";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            SourceGrid.Conditions.ConditionView Tcon_cBalanceView = new SourceGrid.Conditions.ConditionView(TrancBalanceView);
            Tcon_cBalanceView.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    DataRowView row = (DataRowView)itemRow;
                    return (string)row["S.N"] == "-";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            //Create columns
            SourceGrid.DataGridColumn TgridColumn;

            TgridColumn = dgTransaction.Columns.Add("S.N", "S.N", new SourceGrid.Cells.DataGrid.Cell());
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.Conditions.Add(Tcon_ldgHead);
            TgridColumn.Conditions.Add(Tcon_LdgView);
            //  gridColumn.Conditions.Add(conLdgView);
            TgridColumn.Conditions.Add(Tcon_cBalanceView);
            TgridColumn.DataCell.AddController(dblClick);
            TgridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            TgridColumn.Width = 60;

            TgridColumn = dgTransaction.Columns.Add("LedgerName", "Ledger Name", new SourceGrid.Cells.DataGrid.Cell());
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.Conditions.Add(Tcon_ldgHead);
            TgridColumn.Conditions.Add(Tcon_LdgView);
            TgridColumn.Conditions.Add(Tcon_cBalanceView);
            TgridColumn.DataCell.AddController(dblClick);
            TgridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            TgridColumn.Width = 200;

            TgridColumn = dgTransaction.Columns.Add("LedgerID", "LedgerID", typeof(string));
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.Conditions.Add(Tcon_ldgHead);
            TgridColumn.Visible = false;
            TgridColumn.DataCell.AddController(dblClick);

            TgridColumn = dgTransaction.Columns.Add("TransactDate", "Transact Date", new SourceGrid.Cells.DataGrid.Cell());
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.Conditions.Add(Tcon_ldgHead);
           // TgridColumn.Conditions.Add(Tcon_LdgView);
            TgridColumn.Conditions.Add(Tcon_cBalanceView);
            // gridColumn.Conditions.Add(conLdgView);
            TgridColumn.DataCell.AddController(dblClick);
            // gridColumn.Conditions.Add(selectedConditionBold);
            TgridColumn.Conditions.Add(Tcon_cBalanceView);
            TgridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            TgridColumn.Width = 150;

            TgridColumn = dgTransaction.Columns.Add("VoucherName", "Voucher ID", new SourceGrid.Cells.DataGrid.Cell());
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.Conditions.Add(Tcon_ldgHead);
          //  TgridColumn.Conditions.Add(Tcon_LdgView);
            TgridColumn.Conditions.Add(Tcon_cBalanceView);
            //  gridColumn.Conditions.Add(conLdgView);
            TgridColumn.DataCell.AddController(dblClick);
            // gridColumn.Conditions.Add(selectedConditionBold);
            TgridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            TgridColumn.Width = 170;

            TgridColumn = dgTransaction.Columns.Add("DrBal", "Debit Amount", new SourceGrid.Cells.DataGrid.Cell());
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.DataCell.View = TranviewNumeric;
            TgridColumn.Conditions.Add(Tcon_ldgHead);
           // TgridColumn.Conditions.Add(Tcon_LdgView);
            //  gridColumn.Conditions.Add(conLdgView);
            TgridColumn.Conditions.Add(Tcon_cBalanceView);
            TgridColumn.DataCell.AddController(dblClick);
            // gridColumn.Conditions.Add(selectedConditionBold);
            TgridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            TgridColumn = dgTransaction.Columns.Add("CrBal", "Credit Amount", new SourceGrid.Cells.DataGrid.Cell());
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.DataCell.View = TranviewNumeric;
            TgridColumn.Conditions.Add(Tcon_ldgHead);
           // TgridColumn.Conditions.Add(Tcon_LdgView);
            //  gridColumn.Conditions.Add(conLdgView);
            TgridColumn.Conditions.Add(Tcon_cBalanceView);
            TgridColumn.DataCell.AddController(dblClick);
            TgridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            TgridColumn = dgTransaction.Columns.Add("VoucherType", "Voucher Type", new SourceGrid.Cells.DataGrid.Cell());
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.Conditions.Add(Tcon_ldgHead);
           // TgridColumn.Conditions.Add(Tcon_LdgView);
            TgridColumn.Conditions.Add(Tcon_cBalanceView);
            // gridColumn.Conditions.Add(conLdgView);
            TgridColumn.DataCell.AddController(dblClick);
            TgridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            TgridColumn = dgTransaction.Columns.Add("Balance", "Balance", new SourceGrid.Cells.DataGrid.Cell());
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.Conditions.Add(Tcon_ldgHead);
           // TgridColumn.Conditions.Add(Tcon_LdgView);
            //gridColumn.Conditions.Add(conLdgView);
            TgridColumn.Conditions.Add(Tcon_cBalanceView);
            TgridColumn.DataCell.View = TranviewNumeric;
            TgridColumn.DataCell.AddController(dblClick);
            TgridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            TgridColumn = dgTransaction.Columns.Add("RowID", "RowID", typeof(string));
            TgridColumn.Conditions.Add(Tcon_ldgHead);
            TgridColumn.Visible = false;
            TgridColumn.HeaderCell.View = TranViewColumnHeader;
            TgridColumn.DataCell.AddController(dblClick);

            dgTransaction.AutoStretchColumnsToFitWidth = true;

            //  gridColumn = dataGrid.Columns.Add("Star", "Star", new SourceGrid.Cells.Virtual.CellVirtual());
            // gridColumn.DataCell.Model.AddModel(new SourceGrid.Cells.Models.Image(Properties.Resources.StarOff));
            //gridColumn.Conditions.Add(selectedConditionStar);
        }
        private void FillGridWithData1(string AccClassIDsXMLString, string ProjectIDsXMLString)
        {
            grdTransaction.Visible = false;
            dgTransaction.Visible = true;

            CreateColumnsforTempTable();
            CreateData(AccClassIDsXMLString, ProjectIDsXMLString);

            #region datagrid binding
            DataView mView = new DataView(dtTemp);
            mView.AllowDelete = false;
            mView.AllowNew = false;
            dgTransaction.Columns.Clear(); // first clear all columns to reload the data in dgDayBook

            // dataGrid.Columns.AutoSizeView();
            dgTransaction.FixedRows = 1;
            //  dataGrid.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dataGrid));
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);
            //Create default columns          
            CreateColumns(bd);
            dgTransaction.DataSource = bd;
            dgTransaction.Width = dgTransaction.Width - 5;
            dgTransaction.Width = this.Width - 25;
            #endregion
            //for async

            //// dataGrid.DataSource = new DevAge.ComponentModel.BoundDataView(dtTemp.DefaultView);
            // dataGrid.ClipboardMode = SourceGrid.ClipboardMode.All;

            //  // HACK - disable checkinf of ilegal calls
            //  System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            // dataGrid.DataSource.AllowSort = false;


            ////var thread = new Thread(() => CreateData(AccClassIDsXMLString, ProjectIDsXMLString));
            ////thread.Start();

            // thread.Abort();


        }
        private void DisplayTransaction(bool IsCrystalReport)
        {
            TransactRowsCount = 1;
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
            try
            {

                string AccClassIDsXMLString = AccountingClass.GetXMLAccClass(m_TS.AccClassID);
                ArrayList ProjectIDCollection = new ArrayList();
                ProjectIDCollection.Add(m_TS.ProjectID);
                string ProjectIDsXMLString = Project.GetXMLProject(ProjectIDCollection);
                DataTable dtLedgerTransactInfo = new DataTable();

                //Make both dates null if date range is not selected
                if (!m_TS.HasDateRange)
                {
                    m_TS.FromDate = null;
                    m_TS.ToDate = null;
                }
                double m_dbal = 0;
                double m_cbal = 0;

                #region for All Ledgers
                if (m_AccountType == "ALL_LEDGERS" || m_AccountType == "PARTICULAR_LEDGER" || m_AccountType == "PARTICULAR_GROUP")//for all ledger
                {
                    ProgressForm.UpdateProgress(40, "Calculating ledger balance...");
                    if (!IsCrystalReport)//just for  Grid not for Crystal reports
                        Dg_Orientation();
                    //  Orientation();//orientation purpose for grid only  
                    //use loop
                    FillGridWithData1(AccClassIDsXMLString, ProjectIDsXMLString);
                    dgTransaction.EnableSort = false;

                    #region Old code
                    //DataTable dtLedgerInfo = Ledger.GetLedgerInfo(-1, LangMgr.DefaultLanguage);
                    //foreach (DataRow drLedgerInfol in dtLedgerInfo.Rows)                    
                    //{
                    //    LedgerID = Convert.ToInt32(drLedgerInfol["LedgerID"]);

                    //    //For to write ledger name 
                    //    double DrOpBal, CrOpBal;
                    //    DrOpBal = CrOpBal = 0;

                    //    //Write the name of the Ledger first in yellow row
                    //    WriteTransaction(0, drLedgerInfol["EngName"].ToString(), "", "", "", "", "", "", "LEDGERHEAD", "", IsCrystalReport);

                    //    #region BLOCK FOR OPENING BALANCE
                    //    //Show the opening Balance of corresponding Ledger

                    //    DataTable dtOpLedgerInfo = OpeningBalance.GetAccClassOpeningBalance(GetRootAccClassID(), LedgerID);
                    //    foreach (DataRow drLedgerInfo in dtOpLedgerInfo.Rows)
                    //    {
                    //       //If date range is selected. Get all transaction details before that FROM date.

                    //        if (m_TS.FromDate != null && m_TS.ToDate != null)
                    //        {

                    //            if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                    //            {
                    //        DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                    //    }

                    //    else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                    //    {
                    //        CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                    //    }

                    //#region When datetime is selected

                    //    if (m_TS.HasDateRange == true)
                    //    {
                    //        Transaction.GetLedgerBalanceForAccountLedger(null, m_TS.FromDate, Convert.ToInt32(drLedgerInfo["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TS.AccClassID, m_TS.ProjectID);
                    //    }

                    //    DrOpBal = m_dbal;
                    //    CrOpBal = m_cbal;

                    //    //Write Opening Balance on the grid
                    //    if (DrOpBal > 0 && CrOpBal > 0)
                    //{
                    //    if (DrOpBal > CrOpBal)
                    //    {
                    //        double Balance = DrOpBal - CrOpBal;
                    //        WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
                    //    }

                    //    else if (CrOpBal > DrOpBal)
                    //    {
                    //        double Balance = CrOpBal - DrOpBal;
                    //        WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);
                    //    }
                    //}

                    //else if (DrOpBal > 0)
                    //{
                    //    //Write on Debit Side
                    //    WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
                    //}
                    //else if (CrOpBal > 0)
                    //{
                    //    WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);

                    //}
                    //    m_dbal = 0;
                    //        m_cbal = 0;
                    //    }

                    //    #endregion

                    //    #region When datetime is Not selected

                    //    else if (m_TS.FromDate == null && m_TS.ToDate == null)
                    //    {
                    //        if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                    //        {
                    //            DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));

                    //            WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);

                    //        }
                    //        else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                    //        {
                    //            CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));

                    //            WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);
                    //        }
                    //    }
                    //    #endregion
                    //}

                    //#endregion

                    //#region details of transactions
                    //                   DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_TS.FromDate, m_TS.ToDate, ProjectIDsXMLString);
                    //                   if(dt.Rows.Count>0) //If only it has records
                    //                   {
                    //                       DataTable dtLedgerName = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                    //                       foreach (DataRow drLedgerName in dtLedgerName.Rows)
                    //                       {
                    //                           string OpBalanceDrCr = "DR";
                    //                           double OpeningBal = 0;

                    //                           if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal > CrOpBal)
                    //                           {
                    //                               OpBalanceDrCr = "DR";
                    //                               OpeningBal = (DrOpBal - CrOpBal);
                    //                           }
                    //                           else if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal < CrOpBal)
                    //                           {
                    //                               OpBalanceDrCr = "CR";
                    //                               OpeningBal =(CrOpBal - DrOpBal);
                    //                           }
                    //                           else if (DrOpBal == CrOpBal)
                    //                           {
                    //                               OpBalanceDrCr = "DR";
                    //                               OpeningBal = 0;
                    //                           }
                    //                           else if (DrOpBal > 0)
                    //                           {
                    //                               OpBalanceDrCr = "DR";
                    //                               OpeningBal = DrOpBal;
                    //                           }
                    //                           else if (CrOpBal > 0)
                    //                           {
                    //                               OpBalanceDrCr = "CR";
                    //                               OpeningBal = CrOpBal;
                    //                           }
                    //                           ShowLedgerTransaction(dt, LedgerID, OpeningBal, OpBalanceDrCr, IsCrystalReport);
                    //                       }
                    //                   }
                    //#endregion
                    //               }

                    #endregion
                }
                #endregion

                #region Old Code for Particular Group

                // else if (m_AccountType == "PARTICULAR_GROU")//For generating transaction of Specific Account Group
                //{
                //    if (!IsCrystalReport)//just need to write on grid
                //        Orientation();


                //    DataTable dtLedgerIDsByGrpID = Ledger.GetAllLedger(Convert.ToInt32(m_TS.AccountGroupID));//Get all ledger info according to GroupID...Here,we will obtain all the ledger given groupid and its uder subgroupid
                //    foreach (DataRow drLedgerIDsByGrpID in dtLedgerIDsByGrpID.Rows)
                //    {
                //        LedgerID = Convert.ToInt32(drLedgerIDsByGrpID["LedgerID"]);

                //        ///for to write ledger name

                //        double DrOpBal, CrOpBal;
                //        DrOpBal = CrOpBal = 0;

                //        //Write the name of the Ledger first in yellow row

                //        WriteTransaction(0, drLedgerIDsByGrpID["EngName"].ToString(), "", "", "", "", "", "", "LEDGERHEAD", "", IsCrystalReport);

                //        #region BLOCK FOR OPENING BALANCE
                //        //Show the opening Balance of corresponding Ledger

                //        DataTable dtOpLedgerInfo = OpeningBalance.GetAccClassOpeningBalance(GetRootAccClassID(), LedgerID);
                //        foreach (DataRow drLedgerInfo in dtOpLedgerInfo.Rows)
                //        {
                //            //If date range is selected. Get all transaction details before that FROM date.

                //            if (m_TS.FromDate != null && m_TS.ToDate != null)
                //            {

                //                #region   When datetime is selected
                //                if (m_TS.HasDateRange == true)
                //                {
                //                    Transaction.GetLedgerBalanceForAccountLedger(null, m_TS.FromDate, Convert.ToInt32(drLedgerInfo["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TS.AccClassID, m_TS.ProjectID);
                //                }

                //                if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                //                {
                //                    DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                //                }
                //                else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                //                {
                //                    CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                //                }

                //                DrOpBal = m_dbal;
                //                CrOpBal = m_cbal;

                //                //Write Opening Balance on the grid
                //                if (DrOpBal > 0 && CrOpBal > 0)
                //                {
                //                    if (DrOpBal > CrOpBal)
                //                    {
                //                        double Balance = DrOpBal - CrOpBal;
                //                        WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
                //                    }

                //                    else if (CrOpBal > DrOpBal)
                //                    {
                //                        double Balance = CrOpBal - DrOpBal;
                //                        WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);
                //                    }
                //                }

                //                else if (DrOpBal > 0)
                //                {
                //                    //Write on Debit Side
                //                    WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
                //                }
                //                else if (CrOpBal > 0)
                //                {
                //                    WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);

                //                }
                //                m_dbal = 0;
                //                m_cbal = 0;
                //            }
                //                #endregion

                //            #region For not date range selection

                //            else if (m_TS.FromDate == null && m_TS.ToDate == null)
                //            {
                //                if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                //                {
                //                    DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));

                //                    WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);

                //                }
                //                else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                //                {
                //                    CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));

                //                    WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);
                //                }
                //            }
                //        }

                //            #endregion

                //        #endregion


                //        //Write Ledger Transaction Details
                //        #region Write Ledger transaction details
                //        DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_TS.FromDate, m_TS.ToDate, ProjectIDsXMLString);
                //        if (dt.Rows.Count > 0) //If only it has records
                //        {
                //            DataTable dtLedgerName = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                //            foreach (DataRow drLedgerName in dtLedgerName.Rows)
                //            {
                //                string OpBalanceDrCr = "DR";
                //                double OpeningBal = 0;

                //                if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal > CrOpBal)
                //                {
                //                    OpBalanceDrCr = "DR";
                //                    OpeningBal = (DrOpBal - CrOpBal);
                //                }

                //                else if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal < CrOpBal)
                //                {
                //                    OpBalanceDrCr = "CR";
                //                    OpeningBal = CrOpBal - DrOpBal;
                //                }

                //                else if (DrOpBal == CrOpBal)
                //                {
                //                    OpBalanceDrCr = "DR";
                //                    OpeningBal = 0;
                //                }

                //                else if (DrOpBal > 0)
                //                {
                //                    OpBalanceDrCr = "DR";
                //                    OpeningBal = DrOpBal;
                //                }

                //                else if (CrOpBal > 0)
                //                {
                //                    OpBalanceDrCr = "CR";
                //                    OpeningBal = CrOpBal;
                //                }
                //                ShowLedgerTransaction(dt, LedgerID, OpeningBal, OpBalanceDrCr, IsCrystalReport);
                //            }
                //        }
                //        #endregion
                //    }
                //}
                #endregion

                #region old code for Particular Ledgers
                //else if (m_AccountType == "PARTICULAR_LEDGE")
                //{
                //    if (!IsCrystalReport)//just for grid display
                //    {
                //        //for the orientation purpose..like header defination  
                //        Orientation();
                //        LedgerID = m_TS.LedgerID;
                //    }
                //    //For To write Particular Ledger Name
                //    double DrOpBal, CrOpBal;
                //    DrOpBal = CrOpBal = 0;

                //    DataTable dtt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_TS.FromDate, m_TS.ToDate, ProjectIDsXMLString);
                //    if (dtt.Rows.Count > 0) //If only it has records
                //    {
                //        DataTable dtLedgerNamee = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                //        foreach (DataRow drLedgerNamee in dtLedgerNamee.Rows)
                //        {
                //            WriteTransaction(0, drLedgerNamee["LedName"].ToString(), "", "", "", "", "", "", "LEDGERHEAD", "", IsCrystalReport);
                //            LedgerName = drLedgerNamee["LedName"].ToString();//to catch the ledgername for reporting purpose only
                //        }
                //    }

                //    #region BLOCK FOR OPENING BALANCE
                //    //Show the opening Balance of corresponding Ledger

                //    DataTable dtOpLedgerInfo = OpeningBalance.GetAccClassOpeningBalance(GetRootAccClassID(), LedgerID);
                //    foreach (DataRow drLedgerInfo in dtOpLedgerInfo.Rows)
                //    {
                //        //For Date Range Selection
                //        if (m_TS.FromDate != null && m_TS.ToDate != null)
                //        {
                //            if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                //            {
                //                DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                //            }
                //            else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                //            {
                //                CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                //            }

                //            #region If date range is selected. Get all transaction details before that FROM date.
                //            if (m_TS.HasDateRange == true)//When datetime is selected
                //            {
                //                Transaction.GetLedgerBalanceForAccountLedger(null, m_TS.FromDate, Convert.ToInt32(drLedgerInfo["LedgerID"].ToString()), ref m_dbal, ref m_cbal, m_TS.AccClassID, m_TS.ProjectID);
                //            }
                //            DrOpBal = m_dbal;
                //            CrOpBal = m_cbal;

                //            //Write Opening Balance on the grid
                //            if (DrOpBal > 0 && CrOpBal > 0)
                //            {
                //                if (DrOpBal > CrOpBal)
                //                {
                //                    double Balance = DrOpBal - CrOpBal;
                //                    WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
                //                }
                //                else if (CrOpBal > DrOpBal)
                //                {
                //                    double Balance = CrOpBal - DrOpBal;
                //                    WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);
                //                }
                //                else if (DrOpBal == CrOpBal && DrOpBal == 0 && CrOpBal == 0)
                //                {
                //                    double Balance = 0;
                //                    WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
                //                }
                //            }

                //            else if (DrOpBal > 0)
                //            {
                //                //Write on Debit Side
                //                WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
                //            }
                //            else if (CrOpBal > 0)
                //            {
                //                //Write on credit side
                //                WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);

                //            }

                //        }
                //            #endregion

                //        #region  For DateRange NOt Selection
                //        else if (m_TS.FromDate == null && m_TS.ToDate == null)
                //        {
                //            if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                //            {
                //                DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                //                WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);

                //            }
                //            else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                //            {
                //                CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                //                WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);
                //            }
                //        }
                //    }

                //        #endregion
                //    #endregion
                //    #region old code Display ledger transaction details

                //    DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_TS.FromDate, m_TS.ToDate, ProjectIDsXMLString);
                //    if (dt.Rows.Count > 0) //If only it has records
                //    {
                //        DataTable dtLedgerName = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                //        foreach (DataRow drLedgerName in dtLedgerName.Rows)
                //        {
                //            string OpBalanceDrCr = "DR";
                //            double OpeningBal = 0;

                //            if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal > CrOpBal)
                //            {
                //                OpBalanceDrCr = "DR";
                //                OpeningBal = (DrOpBal - CrOpBal);
                //            }
                //            else if (DrOpBal > 0 && CrOpBal > 0 && DrOpBal < CrOpBal)
                //            {
                //                OpBalanceDrCr = "CR";
                //                OpeningBal = CrOpBal - DrOpBal;
                //            }

                //            else if (DrOpBal == CrOpBal)
                //            {
                //                OpBalanceDrCr = "DR";
                //                OpeningBal = 0;
                //            }

                //            else if (DrOpBal > 0)
                //            {
                //                OpBalanceDrCr = "DR";
                //                OpeningBal = DrOpBal;
                //            }
                //            else if (CrOpBal > 0)
                //            {
                //                OpBalanceDrCr = "CR";
                //                OpeningBal = CrOpBal;
                //            }
                //            ShowLedgerTransaction(dt, LedgerID, OpeningBal, OpBalanceDrCr, IsCrystalReport);
                //        }
                //    }
                //}

                //    #endregion

                #endregion
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ProgressForm.UpdateProgress(100, "Preparing report for display...");

            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
        }

        /// <summary>
        /// This block shows the transaction of particular Ledger with others except ownself
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="LedgerID"></param>
        /// <param name="IsCrystalReport"></param>

        #region Old code for showLedgerTransaction
        //private void ShowLedgerTransaction(DataTable dt, int? LedgerID, double OpeningBalance, string OpeningBalanceDrCr, bool IsCrystalReport)
        //{
        //    DataTable dtVoucherInfo = new DataTable();
        //    DataTable dtledInfo = new DataTable();
        //    double Balance = 0;
        //    double TotalDrAmt, TotalCrAmt;
        //    TotalDrAmt = TotalCrAmt = 0;
        //    double DrBal, CrBal;
        //    DrBal = CrBal = 0;

        //    int Count = 1;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        DrBal = Convert.ToDouble(dr["Debit"]);
        //        CrBal = Convert.ToDouble(dr["Credit"]);
        //        TotalDrAmt += Convert.ToDouble(dr["Debit"]);
        //        TotalCrAmt += Convert.ToDouble(dr["Credit"]);

        //        if (Count == 1) //For to calculate with opening balance with first row either dr or cr
        //        {
        //            if (OpeningBalanceDrCr.ToUpper() == "DR")
        //            {
        //                DrBal += OpeningBalance;
        //            }
        //            else if (OpeningBalanceDrCr.ToUpper() == "CR")
        //            {
        //                CrBal += OpeningBalance;
        //            }
        //        }
        //        Balance += (DrBal - CrBal);
        //        if (Balance > 0)//IT represets Debit
        //        {
        //            if (Count == 1 && OpeningBalanceDrCr.ToUpper() == "DR" && OpeningBalance != 0 && CrBal == 0 && DrBal != 0)
        //            {
        //                DrBal = Balance - OpeningBalance;
        //                WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
        //            }
        //            else if (Count == 1 && OpeningBalanceDrCr.ToUpper() == "DR" && OpeningBalance != 0 && CrBal != 0)
        //            {
        //                WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), Convert.ToDouble("0").ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
        //            }
        //            else if (Count == 1 && OpeningBalanceDrCr.ToUpper() == "CR")
        //            {
        //                WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDouble("0").ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
        //            }
        //            else
        //                WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
        //        }
        //        else if (Balance <= 0)//It represents Credit
        //        {
        //            if (Count == 1 && OpeningBalanceDrCr.ToUpper() == "CR" && OpeningBalance != 0 && CrBal != 0 && DrBal == 0)
        //            {
        //                CrBal = (OpeningBalance + Balance);
        //                if (OpeningBalance > Balance)
        //                {
        //                    WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (-CrBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
        //                }
        //                else
        //                    WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
        //            }

        //            else if (Count == 1 && OpeningBalance != 0)
        //            {
        //                WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), Convert.ToDouble("0").ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
        //            }

        //            else
        //                WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
        //        }

        //        Count++;
        //    }

        //    #region BLOCK FOR TOTAL AMOUNT CALCULATION

        //    if (OpeningBalanceDrCr.ToUpper() == "DR")
        //    {
        //        TotalDrAmt += OpeningBalance;
        //    }
        //    else if (OpeningBalanceDrCr.ToUpper() == "CR")
        //    {
        //        TotalCrAmt += OpeningBalance;
        //    }
        //    if (!IsCrystalReport)//only for Grid not for crystal report
        //        WriteTransaction(0, "TOTAL AMOUNT", "", "", (TotalDrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalCrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", "GROUP", "", IsCrystalReport);

        //    #endregion

        //    #region BLOCK FOR CLOSING BALANCE

        //    if (Balance > 0)
        //    {
        //        WriteTransaction(0, "Closing Balance", "", "", "", "", "", (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
        //    }
        //    else if (Balance <= 0)
        //    {
        //        WriteTransaction(0, "Closing Balance", "", "", "", "", "", (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);
        //    }
        //    #endregion
        //}
        #endregion

        private void DisplayBannar()
        {
            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
            if (m_TS.FromDate != null && m_TS.ToDate != null)
            {
                lblAsOnDate.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_TS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_TS.ToDate));
            }
           else if (m_TS.ToDate != null)
            {
                lblAsOnDate.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_TS.ToDate));
            }
           else if (m_TS.FromDate != null)
            {
                //This is actually not applicable
                lblAsOnDate.Text = "From: " + Date.ToSystem(Convert.ToDateTime(m_TS.FromDate));
            }
           
                else
            {
                lblAsOnDate.Text = "As on Date: " + Date.ToSystem(Convert.ToDateTime(Date.GetServerDate()));

            }

            lblAllSettings.Text = "Fiscal Year: " + m_CompanyDetails.FiscalYear;
            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_TS.ProjectID), LangMgr.DefaultLanguage);
            if (m_TS.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];
                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }
        }


        private void frmTransaction_Load(object sender, EventArgs e)
        {

            // DisplayBannarForSummary();
            DisplayBannar();
            //Text format for Total 
            GroupView = new SourceGrid.Cells.Views.Cell();
            GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);

            //Text format for Ledgers
            LedgerView = new SourceGrid.Cells.Views.Cell();
            LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
            LedgerView.ForeColor = Color.Blue;

            //Ledger header
            LedgerHeadView = new SourceGrid.Cells.Views.Cell();
            LedgerHeadView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightGray);
            LedgerHeadView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            LedgerHeadView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            LedgerHeadView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            LedgerHeadView.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);

            //Finally show the transactions
            DisplayTransaction(false);
          //  this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Maximized;


        }
        #region old code for writeTransaction
        //private void WriteTransaction(int Sno, string Ledger, string TransactDate, string VoucherName, string DrBal, string CrBal, string VoucherType, string Balance, string AccType, string RowID, bool IsCrystalReport)
        //{
        //    if (!IsCrystalReport)//for Grid display purpose
        //    {
        //        try
        //        {
        //            SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
        //            if (TransactRowsCount % 2 == 0)
        //            {
        //                LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
        //            }
        //            else
        //            {
        //                LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
        //            }
        //            string strSNo = (Sno == 0 ? "-" : Sno.ToString());
        //            if (AccType == "LEDGERHEAD")
        //            {
        //                //There should be only one column
        //                grdTransaction[TransactRowsCount, 0] = new SourceGrid.Cells.Cell(Ledger);
        //                grdTransaction[TransactRowsCount, 0].ColumnSpan = grdTransaction.ColumnsCount - 1;
        //            }
        //            else
        //            {
        //                if (Ledger == "TOTAL AMOUNT")
        //                {
        //                    TotalDebitAmount += Convert.ToDouble(DrBal);
        //                    TotalCreditAmount += Convert.ToDouble(CrBal);
        //                }
        //                grdTransaction[TransactRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
        //                grdTransaction[TransactRowsCount, 1] = new SourceGrid.Cells.Cell(Ledger);
        //                grdTransaction[TransactRowsCount, 2] = new SourceGrid.Cells.Cell(TransactDate);
        //                grdTransaction[TransactRowsCount, 3] = new SourceGrid.Cells.Cell(VoucherName);
        //                grdTransaction[TransactRowsCount, 4] = new SourceGrid.Cells.Cell(DrBal);
        //                grdTransaction[TransactRowsCount, 5] = new SourceGrid.Cells.Cell(CrBal);
        //                grdTransaction[TransactRowsCount, 6] = new SourceGrid.Cells.Cell(VoucherType);
        //                grdTransaction[TransactRowsCount, 7] = new SourceGrid.Cells.Cell(Balance);
        //                grdTransaction[TransactRowsCount, 8] = new SourceGrid.Cells.Cell(RowID);
        //            }
        //            //To store the current view types accourding to the row type(Ledger, Group etc)
        //            SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

        //            switch (AccType)
        //            {
        //                case "GROUP":
        //                    CurrentView = GroupView;
        //                    break;
        //                case "LEDGER":
        //                    CurrentView = LedgerView;
        //                    break;
        //                case "LEDGERHEAD":
        //                    CurrentView = LedgerHeadView;

        //                    break;
        //                default:
        //                    CurrentView = GroupView; //Because it is the normal formatting without any makeups
        //                    break;
        //            }


        //            if (AccType != "LEDGERHEAD")//This is not ledgerhead
        //            {
        //                grdTransaction[TransactRowsCount, 0].AddController(dblClick);
        //                grdTransaction[TransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
        //                grdTransaction[TransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                grdTransaction[TransactRowsCount, 1].AddController(dblClick);
        //                grdTransaction[TransactRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
        //                //grdTransaction[TransactRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                grdTransaction[TransactRowsCount, 2].AddController(dblClick);
        //                grdTransaction[TransactRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
        //                // grdTransaction[TransactRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                grdTransaction[TransactRowsCount, 3].AddController(dblClick);
        //                grdTransaction[TransactRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
        //                // grdTransaction[TransactRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                grdTransaction[TransactRowsCount, 4].AddController(dblClick);
        //                grdTransaction[TransactRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(CurrentView);
        //                grdTransaction[TransactRowsCount, 4].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

        //                grdTransaction[TransactRowsCount, 5].AddController(dblClick);
        //                grdTransaction[TransactRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(CurrentView);
        //                grdTransaction[TransactRowsCount, 5].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

        //                grdTransaction[TransactRowsCount, 6].AddController(dblClick);
        //                grdTransaction[TransactRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(CurrentView);
        //                // grdTransaction[TransactRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(alternate);

        //                grdTransaction[TransactRowsCount, 7].AddController(dblClick);
        //                grdTransaction[TransactRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(CurrentView);
        //                grdTransaction[TransactRowsCount, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

        //                grdTransaction[TransactRowsCount, 8].AddController(dblClick);
        //                // grdTransaction[TransactRowsCount, 8].View = new SourceGrid.Cells.Views.Cell(alternate);
        //            }

        //            else
        //            {
        //                //If this is LEDGERHEAD, apply the currentview
        //                grdTransaction[TransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
        //            }
        //            grdTransaction.Rows.Insert(grdTransaction.RowsCount);
        //            TransactRowsCount++;
        //            lblTotalDebitAmount.Text = TotalDebitAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
        //            lblTotalCreditAmount.Text = TotalCreditAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
        //        }
        //        catch (Exception ex)
        //        {
        //            Global.Msg(ex.Message);
        //        }

        //    }
        //    else if (IsCrystalReport)//crystal report purpose
        //    {
        //        dsTransaction.Tables["tblTransaction"].Rows.Add(Ledger, TransactDate, VoucherName, DrBal, CrBal, VoucherType, Balance);
        //    }
        //}
        ///// This is method for Crystal Report.To generate report in printable form.
        #endregion
        //private void WriteTransactionOnCrystalRpt(string LedgerName, string TransactDate, string VoucherName, string DrBal, string CrBal, string VoucherType, string Balance)
        //{
        //    dsTransaction.Tables["tblTransaction"].Rows.Add(LedgerName, TransactDate, VoucherName, DrBal, CrBal, VoucherType, Balance);
        //}

        /// This is the method for Double click event.Call the voucher form accoring to corresponding LedgerID]
        /// no need to go Voucher form for Opening Balance/Total Amount/

        //private void Tranasaction_DoubleClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //Get the Selected Row
        //        int CurRow = grdTransaction.Selection.GetSelectionRegion().GetRowsIndex()[0];
        //        SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdTransaction, new SourceGrid.Position(CurRow, 6));
        //        SourceGrid.CellContext cellType1 = new SourceGrid.CellContext(grdTransaction, new SourceGrid.Position(CurRow, 8));
        //        if ((cellType1.Value.ToString()) != "")//Dont Call the voucher form if there is no Ledger...no need to call Voucher form for Op. Bal/Total Amount etc
        //        {
        //            int RowID = Convert.ToInt32(cellType1.Value);
        //            string VoucherType = (cellType.Value).ToString();

        //            switch (VoucherType)
        //            {
        //                case "JRNL":
        //                    frmJournal frm = new frmJournal(RowID);
        //                    frm.Show();
        //                    break;

        //                case "DR_NOT":
        //                    frmDebitNote frm1 = new frmDebitNote(RowID);
        //                    frm1.Show();
        //                    break;

        //                case "CR_NOT":
        //                    frmCreditNote frm2 = new frmCreditNote(RowID);
        //                    frm2.Show();
        //                    break;

        //                case "CASH_PMNT":
        //                    frmCashPayment frm3 = new frmCashPayment(RowID);
        //                    frm3.Show();
        //                    break;

        //                case "CASH_RCPT":
        //                    frmCashReceipt frm4 = new frmCashReceipt(RowID);
        //                    frm4.Show();
        //                    break;

        //                case "BANK_PMNT":
        //                    frmBankPayment frm5 = new frmBankPayment(RowID);
        //                    frm5.Show();
        //                    break;

        //                case "BANK_RCPT":
        //                    frmBankReceipt frm6 = new frmBankReceipt(RowID);
        //                    frm6.Show();
        //                    break;

        //                case "CNTR":
        //                    frmContra frm7 = new frmContra(RowID);
        //                    frm7.Show();
        //                    break;

        //                case "SALES":
        //                    object[] param = new object[1];
        //                    param[0] = (RowID);
        //                    m_MDIForm.OpenFormArrayParam("frmSalesInvoice", param);
        //                    break;

        //                case "PURCH":
        //                    object[] param1 = new object[1];
        //                    param1[0] = (RowID);
        //                    m_MDIForm.OpenFormArrayParam("frmPurchaseInvoice", param1);
        //                    break;

        //                case "SLS_RTN":
        //                    object[] param2 = new object[1];
        //                    param2[0] = (RowID);
        //                    m_MDIForm.OpenFormArrayParam("frmSalesReturn", param2);
        //                    break;

        //                case "PURCH_RTN":
        //                    object[] param3 = new object[1];
        //                    param3[0] = (RowID);
        //                    m_MDIForm.OpenFormArrayParam("frmPurchaseReturn", param3);
        //                    break;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Do nothing;
        //    }
        //}

        private void Dg_Tranasaction_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                Dg_Tranasaction_DoubleClick(sender, e);
        }

        private void Dg_Tranasaction_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

                //Get the Selected Row
                int CurRow = dgTransaction.Selection.GetSelectionRegion().GetRowsIndex()[0];
                // int CurRow = ctx.Position.Row;
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgTransaction, new SourceGrid.Position(CurRow, 7));
                SourceGrid.CellContext cellType1 = new SourceGrid.CellContext(dgTransaction, new SourceGrid.Position(CurRow, 9));
                if (cellType1.Value == null)
                    return;
                if ((cellType1.Value.ToString()) != " " && cellType1.Value != null)//Dont Call the voucher form if there is no Ledger...no need to call Voucher form for Op. Bal/Total Amount etc
                {
                    int RowID = Convert.ToInt32(cellType1.Value);
                    string VoucherType = (cellType.Value).ToString();

                    switch (VoucherType)
                    {
                        case "JRNL":
                            frmJournal frm = new frmJournal(RowID);
                            frm.ShowDialog();
                            break;

                        case "DR_NOT":
                            frmDebitNote frm1 = new frmDebitNote(RowID);
                            frm1.ShowDialog();
                            break;

                        case "CR_NOT":
                            frmCreditNote frm2 = new frmCreditNote(RowID);
                            frm2.ShowDialog();
                            break;

                        case "CASH_PMNT":
                            frmCashPayment frm3 = new frmCashPayment(RowID);
                            frm3.ShowDialog();
                            break;

                        case "CASH_RCPT":
                            frmCashReceipt frm4 = new frmCashReceipt(RowID);
                            frm4.ShowDialog();
                            break;

                        case "BANK_PMNT":
                            frmBankPayment frm5 = new frmBankPayment(RowID);
                            frm5.ShowDialog();
                            break;

                        case "BANK_RCPT":
                            frmBankReceipt frm6 = new frmBankReceipt(RowID);
                            frm6.ShowDialog();
                            break;

                        case "CNTR":
                            frmContra frm7 = new frmContra(RowID);
                            frm7.ShowDialog();
                            break;

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
                    }

                }
                else
                {
                    //Global.Msg("null");
                }
            }
            catch (Exception ex)
            {
                //Do nothing;
                Global.Msg(ex.Message);

            }
        }

        //private void MakeHeader()
        //{
        //    //rows = grdTransaction.RowsCount;
        //    grdTransaction.Rows.Insert(0);
        //    grdTransaction[0, 0] = new MyHeader("S.No.");
        //    grdTransaction[0, 1] = new MyHeader("Ledger Name");
        //    grdTransaction[0, 2] = new MyHeader("Date");
        //    grdTransaction[0, 3] = new MyHeader("Vch #");
        //    grdTransaction[0, 4] = new MyHeader("Debit Amount");
        //    grdTransaction[0, 5] = new MyHeader("Credit Amount");
        //    grdTransaction[0, 6] = new MyHeader("Type");
        //    grdTransaction[0, 7] = new MyHeader("Balance");
        //    grdTransaction[0, 8] = new MyHeader("RowID");

        //    grdTransaction[0, 0].Column.Width = 50;
        //    grdTransaction[0, 1].Column.Width = 335;
        //    grdTransaction[0, 2].Column.Width = 70;
        //    grdTransaction[0, 3].Column.Width = 80;
        //    grdTransaction[0, 4].Column.Width = 120;
        //    grdTransaction[0, 5].Column.Width = 120;
        //    grdTransaction[0, 6].Column.Width = 80;
        //    grdTransaction[0, 8].Column.Width = 100;
        //    grdTransaction[0, 7].Column.Width = 120;
        //    grdTransaction.Columns[8].Visible = false;
        //}
        private void Dg_Orientation()
        {
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(Dg_Tranasaction_DoubleClick);
            dblClick.KeyUp += new KeyEventHandler(Dg_Tranasaction_KeyUp);

            dgTransaction.SelectionMode = SourceGrid.GridSelectionMode.Row;
            dgTransaction.Selection.EnableMultiSelection = false;
            //  dgTransaction.EnableSort = false;


            //Text format for Total
            SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
            categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
            categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            categoryView.Font = new Font(Font, FontStyle.Bold);

            //Text format for Ledgers
            SourceGrid.Cells.Views.Cell LedgerView1 = new SourceGrid.Cells.Views.Cell();
            LedgerView1.ForeColor = Color.BlueViolet;
            LedgerView1.Font = new Font(Font, FontStyle.Italic);

            SourceGrid.Cells.Views.Cell LedgerView = new SourceGrid.Cells.Views.Cell();
            LedgerView.ForeColor = Color.BlueViolet;
            LedgerView.Font = new Font(Font, FontStyle.Italic);
        }

        //private void Orientation()
        //{
        //    dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
        //    dblClick.DoubleClick += new EventHandler(Tranasaction_DoubleClick);

        //    Transaction m_Transaction = new Transaction();
        //    //Text format for Total
        //    SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
        //    categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.RoyalBlue, Color.LightBlue, 0);
        //    categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
        //    categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
        //    categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
        //    categoryView.Font = new Font(Font, FontStyle.Bold);

        //    //Text format for Ledgers
        //    SourceGrid.Cells.Views.Cell LedgerView1 = new SourceGrid.Cells.Views.Cell();
        //    LedgerView1.ForeColor = Color.BlueViolet;
        //    LedgerView1.Font = new Font(Font, FontStyle.Italic);

        //    ////Let the whole row to be selected
        //    grdTransaction.SelectionMode = SourceGrid.GridSelectionMode.Row;

        //    //Set the border width of the selection to thin
        //    //DevAge.Drawing.RectangleBorder b = grdTransaction.Selection.Border;
        //    //b.SetWidth(1);
        //    //grdTransaction.Selection.Border = b;
        //    //SourceGrid.Cells.Views.Cell LedgerView1 = new SourceGrid.Cells.Views.Cell();
        //    SourceGrid.Cells.Views.Cell LedgerView = new SourceGrid.Cells.Views.Cell();
        //    LedgerView.ForeColor = Color.BlueViolet;
        //    LedgerView.Font = new Font(Font, FontStyle.Italic);

        //    ////Disable multiple selection
        //    grdTransaction.Selection.EnableMultiSelection = false;
        //    grdTransaction.Redim(0, 9);
        //    grdTransaction.FixedRows = 1;
        //    MakeHeader();

        //}

        //private string GetVoucherName(DataTable dtVoucherInfo)
        //{
        //    if (dtVoucherInfo.Rows.Count > 0)
        //    {
        //        DataRow drVoucherName = dtVoucherInfo.Rows[0];
        //        return drVoucherName["Voucher_No"].ToString();
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        //private string GetLedgerName(DataTable dtLedgerInfo)
        //{
        //    if (dtLedgerInfo.Rows.Count > 0)
        //    {
        //        DataRow drLedName = dtLedgerInfo.Rows[0];
        //        return drLedName["LedName"].ToString();
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

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

            dsTransaction.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

            //otherwise it populate the records again and again
            double DebitSum, CreditSum;
            DebitSum = CreditSum = 0;
            rptTransaction rpt = new rptTransaction();
            //Fill the logo on the report
            Misc.WriteLogo(dsTransaction, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsTransaction);
            try
            {
                dsTransaction.Tables.Remove("tblTransaction");

            }
            catch
            {

            }
            System.Data.DataView view = new System.Data.DataView(dtTemp);


            DataTable selected = view.ToTable("tblTransaction", false, "LedgerName", "TransactDate", "VoucherName", "DrBal", "CrBal", "VoucherType", "Balance");


            dsTransaction.Tables.Add(selected);

            //   DisplayTransaction(true);

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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvDebit_Total = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvLedgerName = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvGroupName = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCredit_Total = new CrystalDecisions.Shared.ParameterDiscreteValue();
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
                //string companyname=UserPreference.GetValue("COMPANY_NAME", uid); 
                //string companyaddress = UserPreference.GetValue("COMPANY_ADDRESS", uid);
                //string companycity = UserPreference.GetValue("COMPANY_CITY", uid);
                //string companypan = UserPreference.GetValue("COMPANY_PAN", uid);
                //string companyphone = UserPreference.GetValue("COMPANY_PHONE", uid);
                //string companyslogan = UserPreference.GetValue("COMPANY_SLOGAN", uid);

                //pdvCompany_Name.Value = companyname;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_Name);
                //rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                //pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_Address);
                //rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                //pdvCompany_PAN.Value = companypan;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_PAN);
                //rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                //pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_Phone);
                //rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                //pdvCompany_Slogan.Value = companyslogan;
                //pvCollection.Clear();
                //pvCollection.Add(pdvCompany_Slogan);
                //rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                try
                {
                    string companyname = UserPreference.GetValue("COMPANY_NAME", uid);
                    pdvCompany_Name.Value = companyname;
                    string companyaddress = UserPreference.GetValue("COMPANY_ADDRESS", uid);
                    string companycity = UserPreference.GetValue("COMPANY_CITY", uid);
                    pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                    string companypan = UserPreference.GetValue("COMPANY_PAN", uid);
                    pdvCompany_PAN.Value = companypan;
                    string companyphone = UserPreference.GetValue("COMPANY_PHONE", uid);
                    pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                    string companyslogan = UserPreference.GetValue("COMPANY_SLOGAN", uid);
                    pdvCompany_Slogan.Value = companyslogan;
                }
                catch (Exception)
                {
                }
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
            pdvReport_Head.Value = "Transaction";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);
            rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            pdvDebit_Total.Value = lblTotalDebitAmount.Text;
            pvCollection.Clear();
            pvCollection.Add(pdvDebit_Total);
            rpt.DataDefinition.ParameterFields["Total_DebitAmount"].ApplyCurrentValues(pvCollection);

            pdvCredit_Total.Value = lblTotalCreditAmount.Text;
            pvCollection.Clear();
            pvCollection.Add(pdvCredit_Total);
            rpt.DataDefinition.ParameterFields["Total_CreditAmount"].ApplyCurrentValues(pvCollection);

            //to display the Individual Ledger Name in the Header of Report
            if (LedgerName != "")
            {
                pdvLedgerName.Value = LedgerName;
                pvCollection.Clear();
                pvCollection.Add(pdvLedgerName);
                rpt.DataDefinition.ParameterFields["Ledger_Name"].ApplyCurrentValues(pvCollection);
            }
            else if (LedgerName == "")
            {
                pdvLedgerName.Value = "";
                pvCollection.Clear();
                pvCollection.Add(pdvLedgerName);
                rpt.DataDefinition.ParameterFields["Ledger_Name"].ApplyCurrentValues(pvCollection);
            }
            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            //Display the date in crystal report according to available from and to dates
            if (m_TS.FromDate != null && m_TS.ToDate != null)
            {
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_TS.ToDate));
            }
            if (m_TS.ToDate != null)
            {
                pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_TS.ToDate));
            }
            if (m_TS.FromDate != null)
            {
                //This is actually not applicable
                pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TS.FromDate));
            }
            else //Both FromDate and Todate not provided
            {
                pdvReport_Date.Value = "All";
            }
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

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
                    frm.ShowDialog();
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }
            frm.WindowState = FormWindowState.Maximized;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Clear all rows
         //   grdTransaction.Redim(0, 0);
            // dgTransaction.Columns.Clear();
            dgTransaction.Columns.Clear();
            //Reset the total variables
            TotalCreditAmount = TotalDebitAmount = 0;


            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmTransaction_Load(sender, e);

            this.Cursor = Cursors.Default;
            //Show complete notification
            //Global.Msg("Reload Complete!");
           //this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Maximized;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // frmTransaction
        //    // 
        //    this.ClientSize = new System.Drawing.Size(284, 261);
        //    this.Name = "frmTransaction";
        //    this.Load += new System.EventHandler(this.frmTransaction_Load_1);
        //    this.ResumeLayout(false);

        //}

        private void frmTransaction_KeyDown(object sender, KeyEventArgs e)
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
                    btnPrintPreview_Click_1(sender, e);
                    break;

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

            prntDirect = 1;
            btnPrintPreview_Click_1(sender, e);

            #region Old Code
            //frmProgress ProgressForm = new frmProgress();
            //// Initialize the thread that will handle the background process
            //Thread backgroundThread = new Thread(
            //    new ThreadStart(() =>
            //    {
            //        ProgressForm.ShowDialog();
            //    }
            //));

            //backgroundThread.Start();

            ////Update the progressbar
            //ProgressForm.UpdateProgress(20, "Initializing Report Viewer...");

            //dsTransaction.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

            ////otherwise it populate the records again and again
            //double DebitSum, CreditSum;
            //DebitSum = CreditSum = 0;
            //rptTransaction rpt = new rptTransaction();
            ////Fill the logo on the report
            //Misc.WriteLogo(dsTransaction, "tblImage");
            ////Set DataSource to be dsTrial dataset on the report
            //rpt.SetDataSource(dsTransaction);
            //DisplayTransaction(true);

            ////Provide values to the parameters on the report
            //CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvFiscal_Year = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvDebit_Total = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvCredit_Total = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            ////Update the progressbar
            //ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            //pdvFont.Value = "Arial";
            //pvCollection.Clear();
            //pvCollection.Add(pdvFont);
            //rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

            //CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();

            //pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
            //pvCollection.Clear();
            //pvCollection.Add(pdvCompany_Name);
            //rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

            //pdvCompany_Address.Value = m_CompanyDetails.Address1 + ((m_CompanyDetails.Address1.Trim().Length > 0) && (m_CompanyDetails.City.Trim().Length > 0) ? ", " : "") + m_CompanyDetails.City; //Display comma in the middle only if both are available
            //pvCollection.Clear();
            //pvCollection.Add(pdvCompany_Address);
            //rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

            //pdvCompany_PAN.Value = m_CompanyDetails.PAN;
            //pvCollection.Clear();
            //pvCollection.Add(pdvCompany_PAN);
            //rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

            //pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
            //pvCollection.Clear();
            //pvCollection.Add(pdvCompany_Phone);
            //rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

            //pdvCompany_Slogan.Value = m_CompanyDetails.Website;
            //pvCollection.Clear();
            //pvCollection.Add(pdvCompany_Slogan);
            //rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            //pdvReport_Head.Value = "Transaction";
            //pvCollection.Clear();
            //pvCollection.Add(pdvReport_Head);
            //rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            //pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FYFrom;
            //pvCollection.Clear();
            //pvCollection.Add(pdvFiscal_Year);
            //rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            //pdvDebit_Total.Value = lblTotalDebitAmount.Text;
            //pvCollection.Clear();
            //pvCollection.Add(pdvDebit_Total);
            //rpt.DataDefinition.ParameterFields["Total_DebitAmount"].ApplyCurrentValues(pvCollection);

            //pdvCredit_Total.Value = lblTotalCreditAmount.Text;
            //pvCollection.Clear();
            //pvCollection.Add(pdvCredit_Total);
            //rpt.DataDefinition.ParameterFields["Total_CreditAmount"].ApplyCurrentValues(pvCollection);
            ////Update the progressbar
            //ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            ////Display the date in crystal report according to available from and to dates
            //if (m_TS.FromDate != null && m_TS.ToDate != null)
            //{
            //    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TS.FromDate)) + " To: " + Date.ToSystem(Convert.ToDateTime(m_TS.ToDate));
            //}
            //if (m_TS.ToDate != null)
            //{
            //    pdvReport_Date.Value = "As on Date: " + Date.ToSystem(Convert.ToDateTime(m_TS.ToDate));
            //}
            //if (m_TS.FromDate != null)
            //{
            //    //This is actually not applicable
            //    pdvReport_Date.Value = "From: " + Date.ToSystem(Convert.ToDateTime(m_TS.FromDate));
            //}
            //else //Both FromDate and Todate not provided
            //{
            //    pdvReport_Date.Value = "All";
            //}

            //pvCollection.Clear();
            //pvCollection.Add(pdvReport_Date);
            //rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);



            //CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            //DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            //CrDiskFileDestinationOptions.DiskFileName = FileName;

            ////Finally, show the report form
            //frmReportViewer frm = new frmReportViewer();
            //frm.SetReportSource(rpt);

            ////Update the progressbar
            //ProgressForm.UpdateProgress(100, "Showing Report...");

            //// Close the dialog
            //ProgressForm.CloseForm();

            //switch (prntDirect)
            //{
            //    case 1:
            //        rpt.PrintOptions.PrinterName = "";
            //        rpt.PrintToPrinter(1, false, 0, 0);
            //        prntDirect = 0;
            //        return;
            //    case 2:
            //        ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
            //        CrExportOptions = rpt.ExportOptions;
            //        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
            //        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
            //        CrExportOptions.FormatOptions = CrFormatTypeOptions;
            //        rpt.Export();
            //        rpt.Close();
            //        return;
            //    case 3:
            //        PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
            //        CrExportOptions = rpt.ExportOptions;
            //        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
            //        CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
            //        rpt.Export();
            //        rpt.Close();
            //        return;
            //    case 4:
            //        ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
            //        CrExportOptions = rpt.ExportOptions;
            //        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //        CrExportOptions.ExportFormatType = ExportFormatType.Excel;
            //        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
            //        CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
            //        rpt.Export();
            //        frmemail sendemail = new frmemail(FileName, 1);
            //        sendemail.ShowDialog();
            //        rpt.Close();
            //        return;
            //    default:
            //        frm.Show();
            //        frm.WindowState = FormWindowState.Maximized;
            //        break;
            //}
            //frm.WindowState = FormWindowState.Maximized;
            #endregion
        }

       
    }
}
