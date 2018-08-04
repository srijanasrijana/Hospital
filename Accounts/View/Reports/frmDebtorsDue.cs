using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic.Accounting;
using System.Threading;
using System.Collections;
using BusinessLogic;
using DateManager;
using CrystalDecisions.Shared;
using Common;
using Accounts.Reports;
using System.Globalization;

namespace Accounts
{
    public partial class frmDebtorsDueDisplay : Form
    {
        string FileName = " ";
        private int prntDirect = 0;
        //For Export Menu
        ContextMenu Menu_Export;
       // DataTable dt = new DataTable();
        private Accounts.Model.dsDebtorsDue dsDebtorsDue = new Accounts.Model.dsDebtorsDue();
        BusinessLogic.AccountGroup acc = new AccountGroup();
        ArrayList AccClassID = new ArrayList();
        DebtorsDueSettings m_DDS = new DebtorsDueSettings();
        private SourceGrid.Cells.Views.Cell LedgerView;
        private SourceGrid.Cells.Views.Cell LedgerHeadView;
        private SourceGrid.Cells.Views.Cell TotalView;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for Transaction
        private int DebtorsRowsCount = 0;
        DataTable dtTemp;
        DataTableReader dtReader;
        IMDIMainForm m_MDIForm;
        decimal Amount = 0;
        decimal TotalAmount = 0;
        public frmDebtorsDueDisplay()
        {
            InitializeComponent();
        }
        public frmDebtorsDueDisplay(DebtorsDueSettings Ds,IMDIMainForm frm)
        {
            m_MDIForm = frm;
            m_DDS = Ds;
            InitializeComponent();
        }

        private void frmDebtorsDueDisplay_Load(object sender, EventArgs e)
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
            dblClick.DoubleClick += new EventHandler(Tranasaction_DoubleClick);
            dblClick.KeyUp += new KeyEventHandler(Tranasaction_KeyUp);

            dgdebtorsdue.SelectionMode = SourceGrid.GridSelectionMode.Row;
            dgdebtorsdue.Selection.EnableMultiSelection = false;
            DisplayBanner();

            ProgressForm.UpdateProgress(20, "Initializing report...");

            FillGridWithData();
            this.WindowState = FormWindowState.Maximized;
          //  DisplayTransaction(false);

            //For Closig Progress BAR
            ProgressForm.UpdateProgress(100, "Preparing report for display...");
            // Close the dialog if it hasn't been already
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
        }

        private void CreateColforTemp()
        {
            dtTemp = new DataTable();
            dtTemp.Columns.Add("S.N", typeof(string));
            dtTemp.Columns.Add("LedgerID", typeof(string));
            dtTemp.Columns.Add("RowID", typeof(string));
            dtTemp.Columns.Add("TransactDate", typeof(string));
            dtTemp.Columns.Add("VoucherType", typeof(string));
            dtTemp.Columns.Add("DueDate", typeof(string));
            dtTemp.Columns.Add("OverDueDay", typeof(string));
            dtTemp.Columns.Add("Amount", typeof(string));
        }

        private void FillGridWithData()
        {
           
            CreateData();
            lblTotalDebtorDue.Text = TotalAmount.ToString();
            grddebtorsdue.Visible = false;
            dgdebtorsdue.Visible = true;

            #region datagrid binding
            DataView mView = new DataView(dtTemp);
            mView.AllowDelete = false;
            mView.AllowNew = false;
            dgdebtorsdue.Columns.Clear(); // first clear all columns to reload the data in dgDayBook

            // dataGrid.Columns.AutoSizeView();
            dgdebtorsdue.FixedRows = 1;
            //  dataGrid.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dataGrid));
            DevAge.ComponentModel.IBoundList bd = new DevAge.ComponentModel.BoundDataView(mView);
            //Create default columns          
            CreateDataGridColumns(dgdebtorsdue.Columns,bd);
            dgdebtorsdue.DataSource = bd;
            dgdebtorsdue.Width = dgdebtorsdue.Width - 5;
            dgdebtorsdue.Width = this.Width - 25;
            #endregion
        }

        int debGroupID = 0;
        DataTable dtDue=new DataTable();
        DataView view;
       
        private void CreateData()
        {
            DataColumn col = new DataColumn("S.N",typeof(int));
            //col.DataType=System.Type.GetType("System.Int32");
            col.AutoIncrement=true;
            col.AutoIncrementSeed=1;
            col.AutoIncrementStep=1;
            //DataTable dtIncremented = new DataTable();
            //dtIncremented.Columns.Add(col);
            DataTable ddd = new DataTable();
            ddd.Columns.Add(col);
            ddd.Columns.Add("LedgerID", typeof(string));
            ddd.Columns.Add("RowID", typeof(string));
            ddd.Columns.Add("TransactDate", typeof(string));
            ddd.Columns.Add("VoucherType", typeof(string));
            ddd.Columns.Add("DueDate", typeof(string));
            ddd.Columns.Add("OverDueDay", typeof(string));
            ddd.Columns.Add("Amount", typeof(string));


            try
            {
                 
            CreateColforTemp();
            if (m_DDS.isAllDebtors)
            {
                debGroupID = 29;
            }   
            DataTable dt = Debtors.GetDebtorsAgeing(null, m_DDS.ToDate, m_DDS.DebtorsID, "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>", "<ProjectIDSettings><ProjectID>1</ProjectID></ProjectIDSettings>", debGroupID);

            var DistinctDebtors = dt.AsEnumerable()
                        .Select(row => new
                        {
                            DebtorID = row.Field<int>("LedgerID"),
                            DebtorName = row.Field<string>("LedgerName")
                        })
                        .Distinct();
            view= new DataView(dt);
            dtDue=view.ToTable(true,"LedgerID","RowID","NepTransactDate","VoucherType","NepDueDate","OverDueDay","Amount");
            foreach (var Deb in DistinctDebtors)
            {
                // var Details=dtDue.AsEnumerable().Where(r => r.Field<int>("LedgerID") == Deb.DebtorID && r.Field<int>("OverDueDay") > 0).Select(row => new{ DebtorID = row.Field<int>("LedgerID"), RowID = row.Field<int>("LedgerID"), DebtorName = row.Field<string>("LedgerName")  })
                ddd.Rows.Clear();
                //ddd.Columns[0].AutoIncrementSeed = 1;
                //ddd.Columns[0].AutoIncrementStep = 1;

                if (m_DDS.OverDueBills)
                {
                    ddd.BeginLoadData();
                    dtReader = new DataTableReader(dtDue.AsEnumerable().Where(r => r.Field<int>("LedgerID") == Deb.DebtorID && r.Field<int>("OverDueDay") > 0).CopyToDataTable());
                    ddd.Load(dtReader);
                    ddd.EndLoadData();
                }
                else
                {
                    ddd.BeginLoadData();
                    dtReader = new DataTableReader(dtDue.AsEnumerable().Where(r => r.Field<int>("LedgerID") == Deb.DebtorID).CopyToDataTable());
                    ddd.Load(dtReader);
                    ddd.EndLoadData();
                }

                // Math.Abs(ddd.Columns["OverDueDays"]);
                //  ddd = dtDue.AsEnumerable().Where(r => r.Field<int>("LedgerID") == Deb.DebtorID && r.Field<int>("OverDueDay") > 0).CopyToDataTable();
                //if (ddd.Rows.Count > 0)
                //{
                //    dtTemp.Rows.Add(" ", " ", " ", Deb.DebtorName, " ", " ", " "," ");

                //   // ddd = dtDue.AsEnumerable().Where(r => r.Field<int>("LedgerID") == Deb.DebtorID && r.Field<int>("OverDueDay") > 0).CopyToDataTable();
                //    dtIncremented.Rows.Clear();
                //    dtIncremented.BeginLoadData();

                //    DataTableReader dtReader = new DataTableReader(dtDue.AsEnumerable().Where(r => r.Field<int>("LedgerID") == Deb.DebtorID && r.Field<int>("OverDueDay") > 0).CopyToDataTable());
                //    dtIncremented.Load(dtReader);

                //    dtIncremented.EndLoadData();


                //}
                // ddd.Columns.Add(col);
                //  col.SetOrdinal(0);

                if (ddd.Rows.Count > 0)
                {
                    Amount = 0;
                    dtTemp.Rows.Add(" ", " ", " ", " ", Deb.DebtorName.ToString(), " ", " ", " ");
                    foreach(DataRow dr in ddd.Rows)
                    {
                        dtTemp.Rows.Add(dr["S.N"].ToString(), dr["LedgerID"].ToString(), dr["RowID"].ToString(), dr["NepTransactDate"].ToString(), dr["VoucherType"].ToString(), dr["NepDueDate"].ToString(),Convert.ToInt32(dr["OverDueDay"]).ToString(),Convert.ToDecimal(dr["Amount"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        Amount += Convert.ToDecimal(dr["Amount"].ToString());
                    }
                    dtTemp.Rows.Add("-"," ", " ", " "," TOTAL"," "," ", Amount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    TotalAmount += Amount;
                }
                
            }
            }
             catch(InvalidOperationException ex)
            {
                MessageBox.Show("No record found");
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

            SourceGrid.Cells.Views.Cell DebtorsHeadView = new SourceGrid.Cells.Views.Cell();
            DebtorsHeadView.Font = new Font(dgdebtorsdue.Font, FontStyle.Bold);
            DebtorsHeadView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightGray);
            DebtorsHeadView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            DebtorsHeadView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            DebtorsHeadView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            DebtorsHeadView.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);


            SourceGrid.Conditions.ConditionView con_DebHead = new SourceGrid.Conditions.ConditionView(DebtorsHeadView);
            con_DebHead.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                try
                {
                    DataRowView row = (DataRowView)itemRow;
                    return (string)row["S.N"].ToString() == " ";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            };

            string dueDay = " ";
            if (m_DDS.OverDueBills)
            {
                dueDay = "Over Due Days";
            }
            else
                dueDay = "Due Days";

            SourceGrid.DataGridColumn gridColumn;
            gridColumn = dgdebtorsdue.Columns.Add("S.N", "S.N.", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 35;
            gridColumn.Conditions.Add(con_DebHead);
            gridColumn.DataCell.AddController(dblClick);
            // gridColumn.Visible = false;
           // gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgdebtorsdue.Columns.Add("LedgerID", "LedgerID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
            gridColumn.Visible = false;

            gridColumn = dgdebtorsdue.Columns.Add("RowID", "RowID", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 50;
            gridColumn.Visible = false;

            gridColumn = dgdebtorsdue.Columns.Add("TransactDate", "     Transaction Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 150;
            //gridColumn.DataCell.AddController(dblClick);
            gridColumn.Conditions.Add(con_DebHead);
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgdebtorsdue.Columns.Add("VoucherType", "          Voucher Type", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 300;
            //gridColumn.DataCell.AddController(dblClick);
            gridColumn.Conditions.Add(con_DebHead);
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            gridColumn = dgdebtorsdue.Columns.Add("DueDate", "     Due Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 150;
            //gridColumn.DataCell.AddController(dblClick);
            gridColumn.Conditions.Add(con_DebHead);
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgdebtorsdue.Columns.Add("OverDueDay"," Over Due Days", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 110;
            //gridColumn.DataCell.AddController(dblClick);
            gridColumn.Conditions.Add(con_DebHead);
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgdebtorsdue.Columns.Add("Amount", "      Balance", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.HeaderCell.View = viewColumnHeader;
            gridColumn.Width = 150;
            gridColumn.DataCell.View = amountview;
           // gridColumn.DataCell.AddController(dblClick);
            gridColumn.Conditions.Add(con_DebHead);
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            dgdebtorsdue.AutoStretchColumnsToFitWidth = true;


            foreach (SourceGrid.DataGridColumn col in columns)
            {
                SourceGrid.Conditions.ICondition condition =
                    SourceGrid.Conditions.ConditionBuilder.AlternateView(col.DataCell.View,
                                                                           Global.Grid_Color, Color.Black);
                col.Conditions.Add(condition);
            }

        }
        private void DisplayTransaction(bool isCrystalReport)
        {
            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
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
            if (!isCrystalReport)
            {
                DisplayBanner();


                //Disable multiple selection
                grddebtorsdue.Selection.EnableMultiSelection = false;

                grddebtorsdue.Redim(1, 11);
                grddebtorsdue.FixedRows = 1;
                //int rows = grddebtorsdue.Rows.Count;
                MakeHeader();
                ProgressForm.UpdateProgress(40, "Initializing report...");
            }
            m_DDS.FromDate = Convert.ToDateTime("01/02-2013");
                    if (m_DDS.isAllDebtors == true)
                    {
                        DataTable dtLedgerIDsByGrpID = Ledger.GetAllLedger(29);//Get All Ledgers Under Debtors Group
                        foreach (DataRow drLedgerIDsByGrpID in dtLedgerIDsByGrpID.Rows)
                        {
                            int LedgerID = Convert.ToInt32(drLedgerIDsByGrpID["LedgerID"]);
                            DataTable dtLedgerInfo = Ledger.GetAllLedgerDetailsByIDForAgeing(LedgerID);
                            DataRow drledgerinfo = dtLedgerInfo.Rows[0];
                            string ledgercode = drledgerinfo["LedgerCode"].ToString();
                            string ledgername = drledgerinfo["EngName"].ToString();
                            DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_DDS.FromDate, m_DDS.ToDate, ProjectIDsXMLString);
                            // WriteTransaction(LedgerID.ToString(), ledgername, "", "", "", "", "", "", "", "", "", "LEDGERHEAD", isCrystalReport);
                            ShowLedgerTransaction(dt, LedgerID, ledgername, isCrystalReport);
                        }
                    }
                    else if (m_DDS.isAllDebtors == false)
                    {
                        int LedgerID = m_DDS.DebtorsID; ;
                        DataTable dtLedgerInfo = Ledger.GetAllLedgerDetailsByIDForAgeing(LedgerID);
                        DataRow drledgerinfo = dtLedgerInfo.Rows[0];
                        string ledgercode = drledgerinfo["LedgerCode"].ToString();
                        string ledgername = drledgerinfo["EngName"].ToString();
                        DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_DDS.FromDate, m_DDS.ToDate, ProjectIDsXMLString);
                        ShowLedgerTransaction(dt, LedgerID, ledgername, isCrystalReport);
                    }
               
                //For Closig Progress BAR
                ProgressForm.UpdateProgress(100, "Preparing report for display...");
                // Close the dialog if it hasn't been already
                if (ProgressForm.InvokeRequired)
                    ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
          //  }
            
        }

        private void WriteTransaction(string ledgerid, string ledgername, string voucherno, string voucherdate, string duedays, string duetime, string ReceivableAmt, string CollectedAmt, string Balance, string rowid, string vouchertype, string AccType, bool isCrystalReport)
        {
            if (!isCrystalReport)
            {
                DebtorsRowsCount = grddebtorsdue.Rows.Count - 1;
                if (DebtorsRowsCount % 2 == 1)
                {
                    //Text format for Ledgers
                    LedgerView = new SourceGrid.Cells.Views.Cell();
                    LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                    LedgerView.ForeColor = Color.Blue;
                    LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    //Text format for Ledgers
                    LedgerView = new SourceGrid.Cells.Views.Cell();
                    LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
                    LedgerView.ForeColor = Color.Blue;
                    LedgerView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                grddebtorsdue.Rows.Insert(grddebtorsdue.RowsCount);
                if (AccType == "LEDGERHEAD")
                {
                    //Ledger header
                    LedgerHeadView = new SourceGrid.Cells.Views.Cell();
                    LedgerHeadView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightYellow);
                    LedgerHeadView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                    LedgerHeadView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    LedgerHeadView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                    LedgerHeadView.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size + 1, FontStyle.Bold);

                    ////There should be only one column
                    //grddebtorsdue[DebtorsRowsCount, 0] = new SourceGrid.Cells.Cell(ledgername);
                    //grddebtorsdue[DebtorsRowsCount, 0].ColumnSpan = grddebtorsdue.ColumnsCount - 1;
                    grddebtorsdue[DebtorsRowsCount, 0] = new SourceGrid.Cells.Cell(voucherno);
                    grddebtorsdue[DebtorsRowsCount, 1] = new SourceGrid.Cells.Cell(voucherdate);
                    grddebtorsdue[DebtorsRowsCount, 2] = new SourceGrid.Cells.Cell(vouchertype);
                    grddebtorsdue[DebtorsRowsCount, 3] = new SourceGrid.Cells.Cell(ledgername);
                    grddebtorsdue[DebtorsRowsCount, 4] = new SourceGrid.Cells.Cell(duedays);
                    if (duetime != "")
                    {
                        grddebtorsdue[DebtorsRowsCount, 5] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime(duetime)));
                    }
                    else
                    {
                        grddebtorsdue[DebtorsRowsCount, 5] = new SourceGrid.Cells.Cell("");
                    }
                    grddebtorsdue[DebtorsRowsCount, 6] = new SourceGrid.Cells.Cell(Convert.ToDouble(ReceivableAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grddebtorsdue[DebtorsRowsCount, 7] = new SourceGrid.Cells.Cell(Convert.ToDouble(CollectedAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    if (Balance != "")
                    {
                        grddebtorsdue[DebtorsRowsCount, 8] = new SourceGrid.Cells.Cell(Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    }
                    else
                        grddebtorsdue[DebtorsRowsCount, 8] = new SourceGrid.Cells.Cell("");
                    grddebtorsdue[DebtorsRowsCount, 9] = new SourceGrid.Cells.Cell(ledgerid);
                    grddebtorsdue[DebtorsRowsCount, 10] = new SourceGrid.Cells.Cell(rowid);
                }
                else if (AccType == "LEDGER")
                {
                    grddebtorsdue[DebtorsRowsCount, 0] = new SourceGrid.Cells.Cell(voucherno);
                    grddebtorsdue[DebtorsRowsCount, 1] = new SourceGrid.Cells.Cell(voucherdate);
                    grddebtorsdue[DebtorsRowsCount, 2] = new SourceGrid.Cells.Cell(vouchertype);
                    grddebtorsdue[DebtorsRowsCount, 3] = new SourceGrid.Cells.Cell(ledgername);
                    grddebtorsdue[DebtorsRowsCount, 4] = new SourceGrid.Cells.Cell(duedays);
                    if (duetime != "")
                    {
                        grddebtorsdue[DebtorsRowsCount, 5] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime(duetime)));
                    }
                    else
                    {
                        grddebtorsdue[DebtorsRowsCount, 5] = new SourceGrid.Cells.Cell("");
                    }
                    grddebtorsdue[DebtorsRowsCount, 6] = new SourceGrid.Cells.Cell(Convert.ToDouble(ReceivableAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grddebtorsdue[DebtorsRowsCount, 7] = new SourceGrid.Cells.Cell(Convert.ToDouble(CollectedAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    if (Balance != "")
                    {
                        grddebtorsdue[DebtorsRowsCount, 8] = new SourceGrid.Cells.Cell(Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    }
                    else
                        grddebtorsdue[DebtorsRowsCount, 8] = new SourceGrid.Cells.Cell("");
                    grddebtorsdue[DebtorsRowsCount, 9] = new SourceGrid.Cells.Cell(ledgerid);
                    grddebtorsdue[DebtorsRowsCount, 10] = new SourceGrid.Cells.Cell(rowid);
                }
                else if (AccType == "TOTAL")
                {
                    TotalView = new SourceGrid.Cells.Views.Cell();
                    TotalView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                    TotalView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
                    TotalView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                    TotalView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
                    TotalView.Font = new Font(LangMgr.GetFont().FontFamily, LangMgr.GetFont().Size, FontStyle.Bold);

                    grddebtorsdue[DebtorsRowsCount, 0] = new SourceGrid.Cells.Cell("");
                    grddebtorsdue[DebtorsRowsCount, 1] = new SourceGrid.Cells.Cell("");
                    grddebtorsdue[DebtorsRowsCount, 2] = new SourceGrid.Cells.Cell("");
                    grddebtorsdue[DebtorsRowsCount, 3] = new SourceGrid.Cells.Cell(ledgername);
                    grddebtorsdue[DebtorsRowsCount, 4] = new SourceGrid.Cells.Cell("");

                    grddebtorsdue[DebtorsRowsCount, 5] = new SourceGrid.Cells.Cell("");

                    grddebtorsdue[DebtorsRowsCount, 6] = new SourceGrid.Cells.Cell(Convert.ToDouble(ReceivableAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grddebtorsdue[DebtorsRowsCount, 7] = new SourceGrid.Cells.Cell(Convert.ToDouble(CollectedAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grddebtorsdue[DebtorsRowsCount, 8] = new SourceGrid.Cells.Cell(Convert.ToDouble(Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                    grddebtorsdue[DebtorsRowsCount, 9] = new SourceGrid.Cells.Cell("");
                    grddebtorsdue[DebtorsRowsCount, 10] = new SourceGrid.Cells.Cell("");
                }

                SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();
                switch (AccType)
                {
                    case "LEDGER":
                        CurrentView = LedgerView;
                        break;
                    case "LEDGERHEAD":
                        CurrentView = LedgerHeadView;
                        break;
                    case "TOTAL":
                        CurrentView = TotalView;
                        break;
                }

                if (AccType == "LEDGER" || AccType == "LEDGERHEAD")
                {
                    grddebtorsdue[DebtorsRowsCount, 0].AddController(dblClick);
                    grddebtorsdue[DebtorsRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    grddebtorsdue[DebtorsRowsCount, 1].AddController(dblClick);
                    grddebtorsdue[DebtorsRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsdue[DebtorsRowsCount, 2].AddController(dblClick);
                    grddebtorsdue[DebtorsRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsdue[DebtorsRowsCount, 3].AddController(dblClick);
                    grddebtorsdue[DebtorsRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsdue[DebtorsRowsCount, 4].AddController(dblClick);
                    grddebtorsdue[DebtorsRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 4].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    grddebtorsdue[DebtorsRowsCount, 5].AddController(dblClick);
                    grddebtorsdue[DebtorsRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 5].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsdue[DebtorsRowsCount, 6].AddController(dblClick);
                    grddebtorsdue[DebtorsRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 6].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    grddebtorsdue[DebtorsRowsCount, 7].AddController(dblClick);
                    grddebtorsdue[DebtorsRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    grddebtorsdue[DebtorsRowsCount, 8].AddController(dblClick);
                    grddebtorsdue[DebtorsRowsCount, 8].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 8].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                }
                //else if (AccType == "LEDGERHEAD")
                //{
                //    grddebtorsdue[DebtorsRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                //}
                else if (AccType == "TOTAL")
                {
                    grddebtorsdue[DebtorsRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    grddebtorsdue[DebtorsRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsdue[DebtorsRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsdue[DebtorsRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                    grddebtorsdue[DebtorsRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 4].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                    grddebtorsdue[DebtorsRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 5].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;


                    grddebtorsdue[DebtorsRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 6].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    grddebtorsdue[DebtorsRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                    grddebtorsdue[DebtorsRowsCount, 8].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                    grddebtorsdue[DebtorsRowsCount, 8].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                }
            }
            else if (isCrystalReport)//writing on crystal reports
            {
                ////string strSNo = (SNo == 0 ? "" : SNo.ToString());
                //if (Name == "TRIAL TOTAL")
                //{
                //    m_DebitTotal = Convert.ToDouble(DrBal);
                //    m_CreditTotal = Convert.ToDouble(CrBal);
                //}
                //else
                //{

                   // string strSNo = SNo;
                if(duetime=="")
                    dsDebtorsDue.Tables["tblDebtorsDue"].Rows.Add(voucherno, voucherdate, vouchertype, ledgername, duedays, "", ReceivableAmt, CollectedAmt, Balance);
                else 
                    dsDebtorsDue.Tables["tblDebtorsDue"].Rows.Add(voucherno, voucherdate, vouchertype, ledgername, duedays, Date.ToSystem(Convert.ToDateTime(duetime)), ReceivableAmt, CollectedAmt, Balance);
                //}

            }
        }

        private void ShowLedgerTransaction(DataTable dt, int LedgerID,string LedgerName, bool IsCrystalReport)
        {

            DataTable dtVoucherInfo = new DataTable();
            DataTable dtledInfo = new DataTable();
            double Balance = 0;
            double TotalDrAmt, TotalCrAmt;
            TotalDrAmt = TotalCrAmt = 0;

            #region BLOCK FOR OPENING BALANCE
            //Show the opening Balance of corresponding Ledger
            double DrOpBal, CrOpBal;
            DrOpBal = CrOpBal = 0;
            DataTable dtOpLedgerInfo = OpeningBalance.GetAccClassOpeningBalance(GetRootAccClassID(), LedgerID);
            foreach (DataRow drLedgerInfo in dtOpLedgerInfo.Rows)
            {
                if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "DEBIT")//IF ledger has Debit openig balance
                {
                    DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                    Balance = DrOpBal;
                   // WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
                    WriteTransaction(LedgerID.ToString(), "Opening Balance B/F", "", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),"0".ToString(),"","LEDGER",IsCrystalReport);
                    
                }
                else if (drLedgerInfo["OpenBalDrCr"].ToString().ToUpper() == "CREDIT")//IF ledger has credit Opening balance
                {
                    CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                    Balance = (-CrOpBal);
                    WriteTransaction(LedgerID.ToString(), "Opening Balance B/F", "", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", "", (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "0".ToString(), "", "LEDGER", IsCrystalReport);
                    
                }
            }
            #endregion

            double DrBal, CrBal;
            DrBal = CrBal = 0;
            int Count = 1;
            foreach (DataRow dr in dt.Rows)
            {
                DrBal = CrBal = 0;
                DrBal = Convert.ToDouble(dr["Debit"]);
                CrBal = Convert.ToDouble(dr["Credit"]);
                TotalDrAmt += Convert.ToDouble(dr["Debit"]);
                TotalCrAmt += Convert.ToDouble(dr["Credit"]); 
                Balance += (DrBal - CrBal);
               
                //Balance = DrBal - CrBal;
                if (DrBal > CrBal)//IT represets Debit
                {
                        int SalesMasterID = Convert.ToInt32(dr["RowID"].ToString());
                        Sales sls = new Sales();
                        DataTable dtDueDate = sls.GetDueDateForDebtors(SalesMasterID, dr["VoucherType"].ToString());
                        DataRow drduedate = dtDueDate.Rows[0];
                        DateTime TodaysDate =Date.GetServerDate();
                        DateTime DueDate =Convert.ToDateTime(drduedate["DueDate"].ToString());
                        TimeSpan DueDays = DueDate - TodaysDate;
                        double DaysRemaining = DueDays.Days;
                        if (m_DDS.OverDueBills == true)
                        {
                            if (DaysRemaining > 0)
                            {
                                break;
                            }
                            else
                            {
                                WriteTransaction(LedgerID.ToString(), LedgerName, dr["VoucherNumber"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), DaysRemaining.ToString(), DueDate.ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", dr["RowID"].ToString(), dr["VoucherType"].ToString(), "LEDGERHEAD", IsCrystalReport);
                            }

                        }
                        if (m_DDS.DueBills == true)
                        {
                            if (DaysRemaining >= 0)
                            {
                                WriteTransaction(LedgerID.ToString(), LedgerName, dr["VoucherNumber"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), DaysRemaining.ToString(), DueDate.ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", dr["RowID"].ToString(), dr["VoucherType"].ToString(), "LEDGERHEAD", IsCrystalReport);
                            }
                            else
                            {
                                break;
                            }
                        }
                        
                }
                else//It represents Credit
                {
                    //CrBal = ReceiptAmount;
                    WriteTransaction(LedgerID.ToString(), dr["Account"].ToString(), dr["VoucherNumber"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), "", "", DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["RowID"].ToString(), dr["VoucherType"].ToString(), "LEDGER", IsCrystalReport);
                }
                double ReceiptAmount = 0;
                double TotalReceiptAmount = 0;
                if (dr["VoucherType"].ToString() == "SALES")
                {
                    DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("SALES", dr["VoucherNumber"].ToString(), LedgerID);
                    if (dtDebtorReceipt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                        {
                            DataRow drcashreceipt = dtDebtorReceipt.Rows[i];
                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                            ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                            Balance = DrBal - TotalReceiptAmount;
                            WriteTransaction(LedgerID.ToString(), TLedgerName, drcashreceipt["Voucher_No"].ToString(), Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "", "", 0.00.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ReceiptAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drcashreceipt["RowID"].ToString(), "CASH_RCPT", "LEDGER", IsCrystalReport);
                        }
                    }
                    DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("SALES", dr["VoucherNumber"].ToString(), LedgerID);
                    if (dtDebtorBankReceipt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                        {
                            //ReceiptAmount = 0;
                            DataRow drbankreceipt = dtDebtorBankReceipt.Rows[i];
                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                            ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                            Balance = DrBal - TotalReceiptAmount;
                            WriteTransaction(LedgerID.ToString(), TLedgerName, drbankreceipt["Voucher_No"].ToString(), Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "", "", 0.00.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ReceiptAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drbankreceipt["RowID"].ToString(), "BANK_RCPT", "LEDGER", IsCrystalReport);
                        }
                    }
                    #region BLOCK FOR TOTAL AMOUNT CALCULATION
                    //if (DrOpBal > 0)
                    //{
                    //    TotalDrAmt += DrOpBal;
                    //}
                    //else if (CrOpBal > 0)
                    //{
                    //    TotalCrAmt += CrOpBal;
                    //}
                    // if (!IsCrystalReport)//only for Grid not for crystal report
                    WriteTransaction("", "TOTAL AMOUNT", "", "", "", "", (DrBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalReceiptAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", "TOTAL", IsCrystalReport);
                    #endregion
            
                }
                if (dr["VoucherType"].ToString() == "JRNL")
                {
                    DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("JRNL", dr["VoucherNumber"].ToString(), LedgerID);
                    if (dtDebtorReceipt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                        {
                           // ReceiptAmount = 0;
                            DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                            ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                            Balance = DrBal - TotalReceiptAmount;
                            WriteTransaction(LedgerID.ToString(), TLedgerName, drcashreceipt["Voucher_No"].ToString(), Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "", "", 0.00.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ReceiptAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drcashreceipt["RowID"].ToString(), "CASH_RCPT", "LEDGER", IsCrystalReport);
                        }
                    }
                    DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("JRNL", dr["VoucherNumber"].ToString(), LedgerID);
                    if (dtDebtorBankReceipt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                        {
                            //ReceiptAmount = 0;
                            DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                            ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                            Balance = DrBal - TotalReceiptAmount;
                            WriteTransaction(LedgerID.ToString(), TLedgerName, drbankreceipt["Voucher_No"].ToString(), Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "", "", 0.00.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ReceiptAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drbankreceipt["RowID"].ToString(), "BANK_RCPT", "LEDGER", IsCrystalReport);
                        }
                    }
                    #region BLOCK FOR TOTAL AMOUNT CALCULATION
                    if (DrOpBal > 0)
                    {
                        TotalDrAmt += DrOpBal;
                    }
                    else if (CrOpBal > 0)
                    {
                        TotalCrAmt += CrOpBal;
                    }
                    // if (!IsCrystalReport)//only for Grid not for crystal report
                    WriteTransaction("", "TOTAL AMOUNT", "", "", "", "", (DrBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalReceiptAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", "TOTAL", IsCrystalReport);
                    #endregion
                }
                if (dr["VoucherType"].ToString() == "CASH_PMNT")
                {
                    DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("CASH_PMNT", dr["VoucherNumber"].ToString(), LedgerID);
                    if (dtDebtorReceipt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                        {
                            //ReceiptAmount = 0;
                            DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                            ReceiptAmount = Convert.ToDouble(drcashreceipt["Amount"].ToString());
                            Balance = DrBal - TotalReceiptAmount;
                            WriteTransaction(LedgerID.ToString(), TLedgerName, drcashreceipt["Voucher_No"].ToString(), Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "", "", 0.00.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ReceiptAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drcashreceipt["RowID"].ToString(), "CASH_RCPT", "LEDGER", IsCrystalReport);
                        }
                    }
                    DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("CASH_PMNT", dr["VoucherNumber"].ToString(), LedgerID);
                    if (dtDebtorBankReceipt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                        {
                            //ReceiptAmount = 0;
                            DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                            ReceiptAmount = Convert.ToDouble(drbankreceipt["Amount"].ToString());
                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                            Balance = DrBal - TotalReceiptAmount;
                            WriteTransaction(LedgerID.ToString(), TLedgerName, drbankreceipt["Voucher_No"].ToString(), Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "", "", 0.00.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ReceiptAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drbankreceipt["RowID"].ToString(), "BANK_RCPT", "LEDGER", IsCrystalReport);
                        }
                    }
                    #region BLOCK FOR TOTAL AMOUNT CALCULATION
                    if (DrOpBal > 0)
                    {
                        TotalDrAmt += DrOpBal;
                    }
                    else if (CrOpBal > 0)
                    {
                        TotalCrAmt += CrOpBal;
                    }
                    // if (!IsCrystalReport)//only for Grid not for crystal report
                    WriteTransaction("", "TOTAL AMOUNT", "", "", "", "", (DrBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalReceiptAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", "TOTAL", IsCrystalReport);
                    #endregion
                }
                if (dr["VoucherType"].ToString() == "BANK_PMNT")
                {
                    DataTable dtDebtorReceipt = Debtors.GetCashReceiptDetails("BANK_PMNT", dr["VoucherNumber"].ToString(), LedgerID);
                    if (dtDebtorReceipt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDebtorReceipt.Rows.Count; i++)
                        {
                           // ReceiptAmount = 0;
                            DataRow drcashreceipt = dtDebtorReceipt.Rows[0];
                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drcashreceipt["LedgerID"].ToString()));
                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drcashreceipt["Amount"].ToString());
                            ReceiptAmount =  Convert.ToDouble(drcashreceipt["Amount"].ToString());
                            Balance = DrBal - TotalReceiptAmount;
                            WriteTransaction(LedgerID.ToString(), TLedgerName, drcashreceipt["Voucher_No"].ToString(), Date.DBToSystem(drcashreceipt["LedgerDate"].ToString()), "", "", 0.00.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ReceiptAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drcashreceipt["RowID"].ToString(), "CASH_RCPT", "LEDGER", IsCrystalReport);
                        }
                    }
                    DataTable dtDebtorBankReceipt = Debtors.GetBankReceiptDetails("BANK_PMNT", dr["VoucherNumber"].ToString(), LedgerID);
                    if (dtDebtorBankReceipt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDebtorBankReceipt.Rows.Count; i++)
                        {
                            //ReceiptAmount = 0;
                            DataRow drbankreceipt = dtDebtorBankReceipt.Rows[0];
                            string TLedgerName = Ledger.GetLedgerNameFromID(Convert.ToInt32(drbankreceipt["LedgerID"].ToString()));
                            TotalReceiptAmount = TotalReceiptAmount + Convert.ToDouble(drbankreceipt["Amount"].ToString());
                            ReceiptAmount =  Convert.ToDouble(drbankreceipt["Amount"].ToString());
                            Balance = DrBal - TotalReceiptAmount;
                            WriteTransaction(LedgerID.ToString(), TLedgerName, drbankreceipt["Voucher_No"].ToString(), Date.DBToSystem(drbankreceipt["LedgerDate"].ToString()), "", "", 0.00.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ReceiptAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drbankreceipt["RowID"].ToString(), "BANK_RCPT", "LEDGER", IsCrystalReport);
                        }
                    }
                    #region BLOCK FOR TOTAL AMOUNT CALCULATION
                    if (DrOpBal > 0)
                    {
                        TotalDrAmt += DrOpBal;
                    }
                    else if (CrOpBal > 0)
                    {
                        TotalCrAmt += CrOpBal;
                    }
                    // if (!IsCrystalReport)//only for Grid not for crystal report
                    WriteTransaction("", "TOTAL AMOUNT", "", "", "", "", (DrBal).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalReceiptAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", "TOTAL", IsCrystalReport);
                    #endregion
                }
                Count++;
            }
          
        }
        private int GetRootAccClassID()
        {
            if (m_DDS.AccClassID.Count > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(m_DDS.AccClassID[0]));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }

            return 1;//The default root class ID
        }
        private void MakeHeader()
        {
            grddebtorsdue.Rows.Insert(0);
            grddebtorsdue[0, 0] = new MyHeader("Voucher No");
            grddebtorsdue[0, 1] = new MyHeader("Date");
            grddebtorsdue[0, 2] = new MyHeader("VoucherType");
            grddebtorsdue[0, 3] = new MyHeader("Ledger Name");
            grddebtorsdue[0, 4] = new MyHeader("DueDays");
            grddebtorsdue[0, 5] = new MyHeader("DueDate");
            grddebtorsdue[0, 6] = new MyHeader("Receivable Amt");
            grddebtorsdue[0, 7] = new MyHeader("Collected Amt");
            grddebtorsdue[0, 8] = new MyHeader("Balance");
            grddebtorsdue[0, 9] = new MyHeader("LedgerID");
            grddebtorsdue[0, 10] = new MyHeader("rowid");

            //Define the width of column size
            grddebtorsdue[0, 0].Column.Width = 90;
            grddebtorsdue[0, 1].Column.Width = 90;
            grddebtorsdue[0, 2].Column.Width = 100;
            grddebtorsdue[0, 3].Column.Width = 150;
            grddebtorsdue[0, 4].Column.Width = 80;
            grddebtorsdue[0, 5].Column.Width = 90;
            grddebtorsdue[0, 6].Column.Width = 120;
            grddebtorsdue[0, 7].Column.Width = 110;
            grddebtorsdue[0, 8].Column.Width = 100;
            grddebtorsdue[0, 9].Column.Width = 1;
            grddebtorsdue[0, 10].Column.Width = 1;

            //Code for making column invisible
            grddebtorsdue.Columns[10].Visible = false;
            grddebtorsdue.Columns[9].Visible = false;
        }

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
        private void DisplayBanner()
        {
            if (m_DDS.ToDate != null)
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem((DateTime)m_DDS.ToDate);
            }
            else//if date range is not selected then siimply pass the current date time
            {
                lblAsonDate.Text = "As on Date: " + Date.ToSystem(DateTime.Today);
            }
            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                lblFiscalYear.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear;
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_DDS.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_DDS.AccClassID.AddRange(arrChildAccClassIDs);

            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("LEDGERTRANSACT");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ACCCLASSIDS");
                    foreach (string tag in m_DDS.AccClassID)
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
            // MessageBox.Show(strXML);
            return strXML;
        }

        private string ReadAllProjectID()
        {
            #region  ProjectID
            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_DDS.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_DDS.ProjectID + "'";

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
            tw.WriteStartElement("LEDGERTRANSACT");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("PROJECTIDS");
                    tw.WriteElementString("PID", Convert.ToInt32(m_DDS.ProjectID).ToString());
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Tranasaction_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                Tranasaction_DoubleClick(sender, e);
        }
        private void Tranasaction_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //Get the Selected Row
                int CurRow = dgdebtorsdue.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgdebtorsdue, new SourceGrid.Position(CurRow, 4));
                SourceGrid.CellContext cellType1 = new SourceGrid.CellContext(dgdebtorsdue, new SourceGrid.Position(CurRow, 2));
                if ((cellType1.Value.ToString()) != " " && (cellType1.Value.ToString()) != null)//Dont Call the voucher form if there is no Ledger...no need to call Voucher form for Op. Bal/Total Amount etc
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


                        default:
                            break;
                        //case "SALES":
                        //    frmSalesInvoice frm8 = new frmSalesInvoice(RowID);
                        //    frm8.Show();
                        //    break;
                        //case "PURCH":
                        //    frmPurchaseInvoice frm9 = new frmPurchaseInvoice(RowID);
                        //    frm9.Show();
                        //    break;
                    }

                }
                else
                    return;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
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

            dsDebtorsDue.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

            //otherwise it populate the records again and again

            //double DebitSum, CreditSum;
            //DebitSum = CreditSum = 0;
            rptDebtorsDue rpt = new rptDebtorsDue();
            //Fill the logo on the report
            Misc.WriteLogo(dsDebtorsDue, "tblImage");
            //Set DataSource to be dsTrial dataset on the report

            rpt.SetDataSource(dsDebtorsDue);

            try
            {
                dsDebtorsDue.Tables.Remove("tblDebtorsDue");
            }
            catch
            {
            }
            System.Data.DataView view = new System.Data.DataView(dtTemp);
            DataTable selected = view.ToTable("tblDebtorsDue", false, "S.N", "TransactDate", "VoucherType", "DueDate", "OverDueDay","Amount");

            dsDebtorsDue.Tables.Add(selected);

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
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvDueDayName = new CrystalDecisions.Shared.ParameterDiscreteValue();


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

            pdvReport_Head.Value = "DebtorsDue";
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            // pdvFiscal_Year.Value = "Fiscal Year:" +Date.ToSystem( m_CompanyDetails.FYFrom);
            pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
            pvCollection.Clear();
            pvCollection.Add(pdvFiscal_Year);
            rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);


            if (m_DDS.OverDueBills)
                pdvDueDayName.Value = "Over Due Days";
            else
                pdvDueDayName.Value = "Due Days";
            pvCollection.Clear();
            pvCollection.Add(pdvDueDayName);
            rpt.DataDefinition.ParameterFields["DueDayName"].ApplyCurrentValues(pvCollection);


            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            if (m_DDS.ToDate != null)
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem((DateTime)m_DDS.ToDate);
            else
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem(Date.GetServerDate());
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

            pdvPreparedBy.Value = Settings.GetSettings("PREPARED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvPreparedBy);
            rpt.DataDefinition.ParameterFields["Prepared_By"].ApplyCurrentValues(pvCollection);

            pdvCheckedBy.Value = Settings.GetSettings("CHECKED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvCheckedBy);
            rpt.DataDefinition.ParameterFields["Checked_By"].ApplyCurrentValues(pvCollection);

            pdvApprovedBy.Value = Settings.GetSettings("APPROVED_BY");
            pvCollection.Clear();
            pvCollection.Add(pdvApprovedBy);
            rpt.DataDefinition.ParameterFields["Approved_By"].ApplyCurrentValues(pvCollection);

           
            //DisplayTransaction(true);

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
            //grddebtorsdue.Redim(0, 0);
            dgdebtorsdue.Columns.Clear();

            this.Cursor = Cursors.WaitCursor;
            //Load all over again
            frmDebtorsDueDisplay_Load(sender, e);

            this.Cursor = Cursors.Default;
            this.WindowState = FormWindowState.Maximized;
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            prntDirect = 1;
            btnPrintPreview_Click(sender, e);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalDebitAmount_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblAsonDate_Click(object sender, EventArgs e)
        {

        }

        private void lblFiscalYear_Click(object sender, EventArgs e)
        {

        }
    }
}
