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
using System.Threading;
using DateManager;
using Inventory.Forms;
using SourceGrid.Selection;
using Inventory;



namespace Inventory
{
    public partial class frmDayBook : Form
    {
        private int RowID;
        private DayBookSettings m_DayBook;
        private int AddressBookTransactRowsCount;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private int AddressDrCashRowsCount, AddressCrCashRowsCount;
        DataTable dtTransact = new DataTable();
        DataTable dt = new DataTable();
        Journal m_journal = new Journal();
        ArrayList AccClassID = new ArrayList();
        CashReceipt m_CashRecipt = new CashReceipt();
        CashPayment m_CashPayment = new CashPayment();
        BankReceipt m_BankReceipt = new BankReceipt();
        BankPayment m_BankPayment = new BankPayment();
        Contra m_Contra = new Contra();
        DebitNote m_DebitNote = new DebitNote();
        CreditNote m_CreditNote = new CreditNote();
        Sales m_Sales = new Sales();
        string VoucherType = "";
        private DataSet.dsDayBookTransasct dsDayBookTransact = new DataSet.dsDayBookTransasct();
        private DataSet.dsDayBook dsDayBookCash = new DataSet.dsDayBook();
        AccountGroup m_AccountGrp = new AccountGroup();
        private int prntDirect = 0;
        private string FileName = "";
        private string m_DebitTotal = "";
        private string m_CreditTotal = "";
        private string m_Total = "";
        //For Export Menu
        ContextMenu Menu_Export;
        ArrayList ProjectIDs = new ArrayList();
        //Different grid views
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell subGroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;

        private DataRow[] drFound;
        private DataRow[] drFound1;
        private DataTable dTable;
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;


        private enum PrintType
        {
            None,
            DirectPrint,
            Excel,
            PDF,
            CrystalReport,
            Email
        }


        private enum GridColumn : int
        {
            LedgerDate, VoucherNo, VoucherType, AccountName, DebitAmt, CreditAmt, RowID , Ledger_ID
           // LedgerDate , VoucherNo, AccountName, DebitAmt, CreditAmt, VoucherType, RowID, Ledger_ID
        };
        //Temporarily hold old Voucher number and Voucher type
        private string OldVoucherNo = "";
        private string OldVoucherType = "";
        private string OldRowID = "";

        public frmDayBook()
        {
            InitializeComponent();
        }

        public frmDayBook(DayBookSettings DayBook)//Constructor having class object as argument
        {
            try
            {
                InitializeComponent();
                m_DayBook = new DayBookSettings();
                m_DayBook.FromDate = DayBook.FromDate;
                m_DayBook.ToDate = DayBook.ToDate;
                m_DayBook.CashBalanceWise = DayBook.CashBalanceWise;
                m_DayBook.TransactionWise = DayBook.TransactionWise;
                m_DayBook.HasDateRange = DayBook.HasDateRange;
                m_DayBook.AccClassID = DayBook.AccClassID;
                m_DayBook.ProjectID = DayBook.ProjectID;
               
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
                view.Font = new Font("Arial", 9, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;

                AutomaticSortEnabled = false;
            }

        }


        /// <summary>
        /// Create and Assign columns to the grid
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="bindList"></param>
        private void CreateColumns(SourceGrid.DataGridColumns columns,
                                   DevAge.ComponentModel.IBoundList bindList)
        {
            //Borders
            DevAge.Drawing.RectangleBorder border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.ForestGreen), new DevAge.Drawing.BorderLine(Color.ForestGreen));

            border.SetWidth(1);

            //Standard Views

            SourceGrid.Cells.Views.Cell viewString = new SourceGrid.Cells.Views.Cell();

            viewString.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
            //viewNumeric.Border = border;
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

            SourceGrid.Cells.Views.Cell viewImage = new SourceGrid.Cells.Views.Cell();
            //viewImage.Border = border;
            viewImage.ImageStretch = false;
            viewImage.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            //Create columns
            SourceGrid.DataGridColumn gridColumn;



            gridColumn = dgDayBook.Columns.Add("LedgerNepDate", "Transact Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 100;


            gridColumn = dgDayBook.Columns.Add("VoucherType", "Voucher Type", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 130;

            gridColumn = dgDayBook.Columns.Add("VoucherNumber", "Voucher No", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 140;


            gridColumn = dgDayBook.Columns.Add("Account", "Account Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 200;

            gridColumn = dgDayBook.Columns.Add("Debit", "Debit Amount", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 150;

            gridColumn = dgDayBook.Columns.Add("Credit", "Credit Amount", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 150;


            //Hidden columns for storing required information, sp. while doubleclicking
            gridColumn = dgDayBook.Columns.Add("RowID", "Credit Amount", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.Visible = false;

            gridColumn = dgDayBook.Columns.Add("LedgerID", "Credit Amount", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.Visible = false;
        }



        private void WriteHeaderTransactWise()
        {
            grdDayBook.Rows.Insert(0);
            grdDayBook[0, (int)GridColumn.LedgerDate] = new MyHeader("Date");
            grdDayBook[0, (int)GridColumn.LedgerDate].Column.Width = 100;

            grdDayBook[0, (int)GridColumn.VoucherType] = new MyHeader("Type");
            grdDayBook[0, (int)GridColumn.VoucherType].Column.Width = 100;

            grdDayBook[0, (int)GridColumn.VoucherNo] = new MyHeader("Voucher No");
            grdDayBook[0, (int)GridColumn.VoucherNo].Column.Width = 80;

            grdDayBook[0, (int)GridColumn.AccountName] = new MyHeader("Account Name");
            grdDayBook[0, (int)GridColumn.AccountName].Column.Width = 490;

            grdDayBook[0, (int)GridColumn.DebitAmt] = new MyHeader("Debit Amount");
            grdDayBook[0, (int)GridColumn.DebitAmt].Column.Width = 100;

            grdDayBook[0, (int)GridColumn.CreditAmt] = new MyHeader("Credit Amount");
            grdDayBook[0, (int)GridColumn.CreditAmt].Column.Width = 100;

            grdDayBook[0, (int)GridColumn.RowID] = new MyHeader("RowID");
            grdDayBook[0, (int)GridColumn.RowID].Column.Width = 20;
            grdDayBook[0, (int)GridColumn.RowID].Column.Visible = false;


        }
        private void WriteHeaderTransactWise1()
        {
            grdDayBook.Rows.Insert(0);
            grdDayBook[0, (int)GridColumn.LedgerDate] = new SourceGrid.Cells.ColumnHeader("Transcate Date");
            grdDayBook[0, (int)GridColumn.VoucherNo] = new SourceGrid.Cells.ColumnHeader("VoucherNo");
            grdDayBook[0, (int)GridColumn.AccountName] = new SourceGrid.Cells.ColumnHeader("Account Name");
            grdDayBook[0, (int)GridColumn.DebitAmt] = new SourceGrid.Cells.ColumnHeader("Debit Amount");
            grdDayBook[0, (int)GridColumn.CreditAmt] = new SourceGrid.Cells.ColumnHeader("Credit Amount");
            grdDayBook[0, (int)GridColumn.VoucherType] = new SourceGrid.Cells.ColumnHeader("Voucher Type");
            grdDayBook[0, (int)GridColumn.RowID] = new MyHeader("RowID");
            grdDayBook[0, (int)GridColumn.Ledger_ID] = new SourceGrid.Cells.ColumnHeader("LedgerID");
            //  grdDayBook[0, 8] = new MyHeader("VouType");//This column is used only for retriving value for further processing while double click evnet bt it is hidden

            grdDayBook[0, (int)GridColumn.LedgerDate].Column.Width = 130;
            grdDayBook[0, (int)GridColumn.VoucherNo].Column.Width = 130;
            grdDayBook[0, (int)GridColumn.AccountName].Column.Width = 370;
            grdDayBook[0, (int)GridColumn.DebitAmt].Column.Width = 100;
            grdDayBook[0, (int)GridColumn.CreditAmt].Column.Width = 100;
            grdDayBook[0, (int)GridColumn.VoucherType].Column.Width = 100;
            grdDayBook[0, (int)GridColumn.RowID].Column.Width = 100;
            grdDayBook[0, (int)GridColumn.Ledger_ID].Column.Width = 20;

            grdDayBook[0, (int)GridColumn.Ledger_ID].Column.Visible = false;
            grdDayBook[0, (int)GridColumn.RowID].Column.Visible = false;
            //  grdDayBook[0, 8].Column.Visible = false;
        }

        private void WriteHeaderCashwise()
        {
            #region BLOCK FOR DEBIT CASH_BALANCEWISE  HEDADER PART
            //Write header part
            grdDayBook.Rows.Insert(0);
            grdDayBook.Rows.Insert(1);

            grdDayBook[0, 0] = new MyHeader("Debit");
            grdDayBook[0, 0].ColumnSpan = 5;
            grdDayBook[0, 6] = new MyHeader("Credit");
            grdDayBook[0, 6].ColumnSpan = 5;

            grdDayBook[1, 0] = new MyHeader("Date");
            grdDayBook[1, 1] = new MyHeader("Type");
            grdDayBook[1, 2] = new MyHeader("VoucherNo");
            grdDayBook[1, 3] = new MyHeader("Account Name");
            grdDayBook[1, 4] = new MyHeader("Debit Amount");
            grdDayBook[1, 5] = new MyHeader("RowID");

            //Define the width of column size
            grdDayBook[1, 0].Column.Width = 70;
            grdDayBook[1, 1].Column.Width = 60;
            grdDayBook[1, 2].Column.Width = 75;
            grdDayBook[1, 3].Column.Width = 190;
            grdDayBook[1, 4].Column.Width = 100;
            grdDayBook[1, 5].Column.Width = 25;
            grdDayBook[1, 5].Column.Visible = false;
            #endregion

            #region BLOCK FOR CREDIT CASH_BALANCEWISE HEADER PART
            //FOR CREDIT
            grdDayBook[1, 6] = new MyHeader("Date");
            grdDayBook[1, 7] = new MyHeader("Type");
            grdDayBook[1, 8] = new MyHeader("VoucherNo");
            grdDayBook[1, 9] = new MyHeader("Account Name");
            grdDayBook[1, 10] = new MyHeader("Credit Amount");
            grdDayBook[1, 11] = new MyHeader("RowID");

            //Define width of column size
            grdDayBook[1, 6].Column.Width = 70;
            grdDayBook[1, 7].Column.Width = 60;
            grdDayBook[1, 8].Column.Width = 75;
            grdDayBook[1, 9].Column.Width = 190;
            grdDayBook[1, 10].Column.Width = 100;
            grdDayBook[1, 11].Column.Width = 25;
            grdDayBook[1, 11].Column.Visible = false;
            #endregion



            #region
            //grdDayBook.Rows.Insert(0);
            //grdDayBook.Rows.Insert(1);

            ////grdDayBook[0, 0] = new MyHeader("Debit");
            ////grdDayBook[0, 0].ColumnSpan = 5;


            //grdDayBook[0, 0] = new MyHeader("Date");
            //grdDayBook[0, 1] = new MyHeader("VoucherNo");
            //grdDayBook[0, 2] = new MyHeader("Account Name");
            //grdDayBook[0, 3] = new MyHeader("Debit Amount");
            //grdDayBook[0, 5] = new MyHeader("VoucherType");
            //grdDayBook[0, 4] = new MyHeader("RowID");

            ////grdDayBook[0, 6] = new MyHeader("Credit");
            ////grdDayBook[0, 6].ColumnSpan = 5;

            ////Define the width of column size
            //grdDayBook[0, 0].Column.Width = 70;
            //grdDayBook[0, 1].Column.Width = 60;
            //grdDayBook[0, 2].Column.Width = 75;
            //grdDayBook[0, 3].Column.Width = 190;
            //grdDayBook[0, 4].Column.Width = 100;
            //grdDayBook[0, 5].Column.Width = 25;
            //grdDayBook[0, 4].Column.Visible = false;
            //#endregion

            //#region BLOCK FOR CREDIT CASH_BALANCEWISE HEADER PART
            ////FOR CREDIT


            //grdDayBook[0, 6] = new MyHeader("Date");
            //grdDayBook[0, 7] = new MyHeader("VoucherNo");
            //grdDayBook[0, 8] = new MyHeader("Account Name");
            //grdDayBook[0, 9] = new MyHeader("Credit Amount");
            //grdDayBook[0, 10] = new MyHeader("VoucherType");
            //grdDayBook[0, 11] = new MyHeader("RowID");

            ////Define width of column size
            //grdDayBook[0, 6].Column.Width = 70;
            //grdDayBook[0, 7].Column.Width = 60;
            //grdDayBook[0, 8].Column.Width = 75;
            //grdDayBook[0, 9].Column.Width = 190;
            //grdDayBook[0, 10].Column.Width = 100;
            //grdDayBook[0, 11].Column.Width = 25;
            //grdDayBook[0, 11].Column.Visible = false;
            #endregion


        }

        private void WriteDayBookTransact(string TransactDate, string VoucherType, string VoucherNo, string LedgerName, string DrAmt, string CrAmt, string AccType, string RowID, bool IsCrystalReport)
        {
            if (!IsCrystalReport)//when need to write records on grid
            {
                grdDayBook.Rows.Insert(grdDayBook.RowsCount);
                // Block for getting GroupName             
                grdDayBook[AddressBookTransactRowsCount, 0] = new SourceGrid.Cells.Cell(TransactDate);
                grdDayBook[AddressBookTransactRowsCount, 1] = new SourceGrid.Cells.Cell(VoucherNo);
                grdDayBook[AddressBookTransactRowsCount, 2] = new SourceGrid.Cells.Cell(LedgerName);
                grdDayBook[AddressBookTransactRowsCount, 3] = new SourceGrid.Cells.Cell(DrAmt);
                grdDayBook[AddressBookTransactRowsCount, 4] = new SourceGrid.Cells.Cell(CrAmt);
                grdDayBook[AddressBookTransactRowsCount, 5] = new SourceGrid.Cells.Cell(VoucherType);
                grdDayBook[AddressBookTransactRowsCount, 6] = new SourceGrid.Cells.Cell(RowID);
                grdDayBook[AddressBookTransactRowsCount, 7] = new SourceGrid.Cells.Cell("");



                //To store the current view types accourding to the row type(Ledger, Group etc)
                SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                switch (AccType)
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

                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (AddressBookTransactRowsCount % 2 == 0)
                {
                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                grdDayBook[AddressBookTransactRowsCount, 0].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                grdDayBook[AddressBookTransactRowsCount, 1].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdDayBook[AddressBookTransactRowsCount, 2].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdDayBook[AddressBookTransactRowsCount, 3].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                grdDayBook[AddressBookTransactRowsCount, 4].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, 4].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdDayBook[AddressBookTransactRowsCount, 5].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, 5].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, 5].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;




                AddressBookTransactRowsCount++;

            }
            else if (IsCrystalReport)//when need to write records on Crystal Reports
            {
                if (LedgerName == "TOTAL AMOUNT")
                {
                    m_DebitTotal = DrAmt;
                    m_CreditTotal = CrAmt;
                }
                else
                    dsDayBookTransact.Tables["tblDayBookTransact"].Rows.Add(TransactDate, VoucherNo, VoucherType, LedgerName, DrAmt, CrAmt);
            }

        }

        private void WriteDayBookTransact1(string TransactDate, string VoucherType, string VoucherNo, string LedgerName, string LedgerID, string DrAmt, string CrAmt, string AccType, string RowID, bool IsCrystalReport)
        {
            if (!IsCrystalReport)//when need to write records on grid
            {
                grdDayBook.Rows.Insert(grdDayBook.RowsCount);
                // Block for getting GroupName             

                string _TransactDate = TransactDate;
                if (OldVoucherNo == VoucherNo && OldVoucherType == VoucherType)
                    _TransactDate = "";

                // string _VoucherType = VoucherType;
                string _VoucherNo = VoucherNo;
                if (OldRowID == RowID)
                    _VoucherNo = "";
                //  _VoucherType = "";

                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.LedgerDate] = new SourceGrid.Cells.Cell(_TransactDate);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherNo] = new SourceGrid.Cells.Cell(_VoucherNo);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.AccountName] = new SourceGrid.Cells.Cell(LedgerName);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.DebitAmt] = new SourceGrid.Cells.Cell(DrAmt);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.CreditAmt] = new SourceGrid.Cells.Cell(CrAmt);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherType] = new SourceGrid.Cells.Cell(VoucherType);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.RowID] = new SourceGrid.Cells.Cell(RowID);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.Ledger_ID] = new SourceGrid.Cells.Cell(LedgerID);

                //To store the current view types accourding to the row type(Ledger, Group etc)
                SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                switch (AccType)
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

                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (AddressBookTransactRowsCount % 2 == 0)
                {
                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.LedgerDate].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.LedgerDate].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.LedgerDate].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.LedgerDate].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherNo].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherNo].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherNo].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherNo].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.AccountName].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.AccountName].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.AccountName].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.AccountName].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.DebitAmt].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.DebitAmt].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.DebitAmt].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.DebitAmt].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.CreditAmt].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.CreditAmt].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.CreditAmt].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.CreditAmt].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherType].AddController(dblClick);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherType].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherType].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressBookTransactRowsCount, (int)GridColumn.VoucherType].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                AddressBookTransactRowsCount++;

            }
            else if (IsCrystalReport)//when need to write records on Crystal Reports
            {
                if (LedgerName == "TOTAL AMOUNT")
                {
                    m_DebitTotal = DrAmt;
                    m_CreditTotal = CrAmt;
                }
                else
                    dsDayBookTransact.Tables["tblDayBookTransact"].Rows.Add(TransactDate, VoucherNo, VoucherType, LedgerName, DrAmt, CrAmt);
            }

        }

        private void WriteDayBookDrCash(string Date, string Type, string VoucherNo, string AccountName, string DrAmt, string AccType, string RowID, bool IsCrystalReport)
        {
            if (!IsCrystalReport)//IF need to write on grid
            {
                //Insert a row to the grid for a new row to be written
                if (grdDayBook.RowsCount <= AddressDrCashRowsCount)
                    grdDayBook.Rows.Insert(grdDayBook.RowsCount);

                string _Date = Date;
                if (OldVoucherNo == VoucherNo && OldVoucherType == VoucherType)
                    _Date = "";

                // string _VoucherType = VoucherType;
                string _VoucherNo = VoucherNo;
                if (OldRowID == RowID)
                    _VoucherNo = "";
                //  _VoucherType = "";

                grdDayBook[AddressDrCashRowsCount, 0] = new SourceGrid.Cells.Cell(_Date);
                grdDayBook[AddressDrCashRowsCount, 1] = new SourceGrid.Cells.Cell(Type);
                grdDayBook[AddressDrCashRowsCount, 2] = new SourceGrid.Cells.Cell(_VoucherNo);
                grdDayBook[AddressDrCashRowsCount, 3] = new SourceGrid.Cells.Cell(AccountName);
                grdDayBook[AddressDrCashRowsCount, 4] = new SourceGrid.Cells.Cell(DrAmt);
                grdDayBook[AddressDrCashRowsCount, 5] = new SourceGrid.Cells.Cell(RowID);

                //To store the current view types accourding to the row type(Ledger, Group etc)
                SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                switch (AccType)
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
                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (AddressDrCashRowsCount % 2 == 0)
                {
                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }

                grdDayBook[AddressDrCashRowsCount, 0].AddController(dblClick);
                grdDayBook[AddressDrCashRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressDrCashRowsCount, 0].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressDrCashRowsCount, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                grdDayBook[AddressDrCashRowsCount, 1].AddController(dblClick);
                grdDayBook[AddressDrCashRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressDrCashRowsCount, 1].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressDrCashRowsCount, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                grdDayBook[AddressDrCashRowsCount, 2].AddController(dblClick);
                grdDayBook[AddressDrCashRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressDrCashRowsCount, 2].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressDrCashRowsCount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                grdDayBook[AddressDrCashRowsCount, 3].AddController(dblClick);
                grdDayBook[AddressDrCashRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressDrCashRowsCount, 3].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressDrCashRowsCount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                grdDayBook[AddressDrCashRowsCount, 4].AddController(dblClick);
                grdDayBook[AddressDrCashRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressDrCashRowsCount, 4].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressDrCashRowsCount, 4].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                //Increment Liablities rows count
                AddressDrCashRowsCount++;

            }
            else if (IsCrystalReport)
            {
                if (AccountName == "Total Debit Amount:")
                    m_Total = DrAmt;
                else
                    dsDayBookCash.Tables["tblDayBookCash"].Rows.Add(Date, Type, VoucherNo, AccountName, DrAmt, 1);
            }


        }

        private void WriteDayBookCrCash(string Date, string Type, string VoucherNo, string AccountName, string CrAmt, string AccType, string RowID, bool IsCrystalReport)
        {

            if (!IsCrystalReport)
            {
                //Insert a row to the grid for a new row to be written
                if (grdDayBook.RowsCount <= AddressCrCashRowsCount)
                    grdDayBook.Rows.Insert(grdDayBook.RowsCount);

                string _Date = Date;
                if (OldVoucherNo == VoucherNo && OldVoucherType == VoucherType)
                    _Date = "";

                // string _VoucherType = VoucherType;
                string _VoucherNo = VoucherNo;
                if (OldRowID == RowID)
                    _VoucherNo = "";
                //  _VoucherType = "";

                grdDayBook[AddressCrCashRowsCount, 6] = new SourceGrid.Cells.Cell(Date);
                grdDayBook[AddressCrCashRowsCount, 7] = new SourceGrid.Cells.Cell(Type);
                grdDayBook[AddressCrCashRowsCount, 8] = new SourceGrid.Cells.Cell(VoucherNo);
                grdDayBook[AddressCrCashRowsCount, 9] = new SourceGrid.Cells.Cell(AccountName);
                grdDayBook[AddressCrCashRowsCount, 10] = new SourceGrid.Cells.Cell(CrAmt);
                grdDayBook[AddressCrCashRowsCount, 11] = new SourceGrid.Cells.Cell(RowID);

                //To store the current view types accourding to the row type(Ledger, Group etc)
                SourceGrid.Cells.Views.Cell CurrentView = new SourceGrid.Cells.Views.Cell();

                switch (AccType)
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

                SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();
                if (AddressCrCashRowsCount % 2 == 0)
                {
                    //alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCoral);
                }
                else
                {
                    alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }

                grdDayBook[AddressCrCashRowsCount, 6].AddController(dblClick);
                grdDayBook[AddressCrCashRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressCrCashRowsCount, 6].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressCrCashRowsCount, 6].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                grdDayBook[AddressCrCashRowsCount, 7].AddController(dblClick);
                grdDayBook[AddressCrCashRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressCrCashRowsCount, 7].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressCrCashRowsCount, 7].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                grdDayBook[AddressCrCashRowsCount, 8].AddController(dblClick);
                grdDayBook[AddressCrCashRowsCount, 8].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressCrCashRowsCount, 8].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressCrCashRowsCount, 8].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                grdDayBook[AddressCrCashRowsCount, 9].AddController(dblClick);
                grdDayBook[AddressCrCashRowsCount, 9].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressCrCashRowsCount, 9].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressCrCashRowsCount, 9].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                grdDayBook[AddressCrCashRowsCount, 10].AddController(dblClick);
                grdDayBook[AddressCrCashRowsCount, 10].View = new SourceGrid.Cells.Views.Cell(CurrentView);
                grdDayBook[AddressCrCashRowsCount, 10].View = new SourceGrid.Cells.Views.Cell(alternate);
                grdDayBook[AddressCrCashRowsCount, 10].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                AddressCrCashRowsCount++;
            }
            else if (IsCrystalReport)
            {
                if (AccountName == "Total Credit Amount:")
                    m_Total = CrAmt;
                else
                    dsDayBookCash.Tables["tblDayBookCash"].Rows.Add(Date, Type, VoucherNo, AccountName, CrAmt, 2);
            }


        }

        private void OrientationForCashwise()
        {
            AddressDrCashRowsCount = 2;
            AddressCrCashRowsCount = 2;
            ////Let the whole row to be selected
            //grdDayBook.SelectionMode = SourceGrid.GridSelectionMode.Row;

            //Disable multiple selection
            grdDayBook.Selection.EnableMultiSelection = false;
            grdDayBook.Redim(2, 12);
            grdDayBook.FixedRows = 2;
            WriteHeaderCashwise();
        }

        private void OrientationForTranwise()
        {
            //Let the whole row to be selected
            grdDayBook.SelectionMode = SourceGrid.GridSelectionMode.Row;
            //Disable multiple selection
            grdDayBook.Selection.EnableMultiSelection = false;
            grdDayBook.Redim(1, 7);
            grdDayBook.FixedRows = 1;
            int rows = grdDayBook.Rows.Count;
            WriteHeaderTransactWise();

        }

        private void OrientationForTranwise1()
        {
            //Let the whole row to be selected
            grdDayBook.SelectionMode = SourceGrid.GridSelectionMode.Row;
            //Disable multiple selection
            grdDayBook.Selection.EnableMultiSelection = false;
            grdDayBook.Redim(1, 8);
            grdDayBook.FixedRows = 1;
            int rows = grdDayBook.Rows.Count;
            WriteHeaderTransactWise1();

        }

        private void DisplayTransactWiseDayBook(bool IsCrystalReport)
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

            if (!IsCrystalReport)
                OrientationForTranwise(); //This function for oreintation purpose like header,font,style etc.    
            DataTable dtDistinctRowid = m_AccountGrp.GetDistinctRowID();
            try
            {
                double totalDrAmt, totalCrAmt;
                totalDrAmt = totalCrAmt = 0;
                ProgressForm.UpdateProgress(40, "Calculating ledger balance...");

                foreach (DataRow drDistinctRowID in dtDistinctRowid.Rows)
                {
                    Transaction m_Transaction = new Transaction();
                    DataTable dtTransactInfo = new DataTable();
                    if (m_DayBook.HasDateRange == true)
                    {
                        dtTransactInfo = m_Transaction.GetTransactionInfoByAccClassAndProjectID(drDistinctRowID["RowID"].ToString(), m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);

                    }
                    else
                    {
                        dtTransactInfo = m_Transaction.GetTransactionInfoByAccClassAndProjectID(drDistinctRowID["RowID"].ToString(), null, null, m_DayBook.AccClassID, ProjectIDs);
                    }

                    foreach (DataRow drTransactInfo in dtTransactInfo.Rows)
                    {
                        VoucherType = drTransactInfo["VoucherType"].ToString();
                        GetVoucherMasterInfo(drTransactInfo["RowID"].ToString(), drTransactInfo["VoucherType"].ToString());
                        string VoucherNumber = "";
                        foreach (DataRow drVoucherMasterInfo in dt.Rows)
                        {
                            VoucherNumber = drVoucherMasterInfo["Voucher_No"].ToString(); ;
                        }
                        //Code for getting AccountName with help of ledgerID
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactInfo["LedgerID"]), LangMgr.DefaultLanguage);

                        foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                        {
                            // txtDateBookBegin.Text = Date.ToSystem(CompDetails.BookBeginFrom); 
                            DateTime transactDate = Convert.ToDateTime(drTransactInfo["TransactDate"]);

                            totalDrAmt += Convert.ToDouble(drTransactInfo["Debit_Amount"]);
                            totalCrAmt += Convert.ToDouble(drTransactInfo["Credit_Amount"]);
                            string dramount = Convert.ToDecimal(drTransactInfo["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            string cramount = Convert.ToDecimal(drTransactInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            if (dramount == "0.00")
                            {
                                WriteDayBookTransact(Date.ToSystem(transactDate), drTransactInfo["VoucherType"].ToString(), VoucherNumber, drLedgerInfo["LedName"].ToString(), " ", cramount, "LEDGER", drTransactInfo["RowID"].ToString(), IsCrystalReport);
                            }
                            else if (cramount == "0.00")
                            {
                                WriteDayBookTransact(Date.ToSystem(transactDate), drTransactInfo["VoucherType"].ToString(), VoucherNumber, drLedgerInfo["LedName"].ToString(), dramount, " ", "LEDGER", drTransactInfo["RowID"].ToString(), IsCrystalReport);
                            }
                            else
                            {
                                // WriteDayBookTransact(transactDate.ToShortDateString(), drTransactInfo["VoucherType"].ToString(), VoucherNumber, drLedgerInfo["LedName"].ToString(), drTransactInfo["Debit_Amount"].ToString(), drTransactInfo["Credit_Amount"].ToString(), "LEDGER", drTransactInfo["RowID"].ToString(), IsCrystalReport);
                                WriteDayBookTransact(Date.ToSystem(transactDate), drTransactInfo["VoucherType"].ToString(), VoucherNumber, drLedgerInfo["LedName"].ToString(), dramount, cramount, "LEDGER", drTransactInfo["RowID"].ToString(), IsCrystalReport);
                            }
                        }

                    }

                }
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                ////adding one grid for total amount
                //if (!IsCrystalReport)
                WriteDayBookTransact("", "", "", "TOTAL AMOUNT", totalDrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), totalCrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "", IsCrystalReport);

                ProgressForm.UpdateProgress(100, "Preparing report for display...");
                if (ProgressForm.InvokeRequired)
                    ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DisplayTransactWiseDayBook1(bool IsCrystalReport)
        {
            #region old one
            frmProgress ProgressForm = new frmProgress();
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    ProgressForm.ShowDialog();
                }
            ));

            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();

            backgroundThread.Start();
            //Update the progressbar
            ProgressForm.UpdateProgress(20, "Initializing Report Viewer...");

            if (!IsCrystalReport)
                OrientationForTranwise1(); //This function for oreintation purpose like header,font,style etc.    
            try
            {
                double totalDrAmt, totalCrAmt;
                totalDrAmt = totalCrAmt = 0;
                ProgressForm.UpdateProgress(40, "Calculating ledger balance...");
                //Transaction m_Transaction = new Transaction();
                DataTable dtTransactInfo = new DataTable();

                if (m_DayBook.HasDateRange == true)
                {
                    dtTransactInfo = Transaction.GetTransactionDetails(m_DayBook.FromDate, m_DayBook.ToDate, AccClassIDsXMLString, ProjectIDsXMLString);
                }
                else
                {
                    dtTransactInfo = Transaction.GetTransactionDetails(null, null, AccClassIDsXMLString, ProjectIDsXMLString);
                }

                foreach (DataRow drTransactInfo in dtTransactInfo.Rows)
                {
                    DateTime LedgerDate = Convert.ToDateTime(drTransactInfo["LedgerDate"]);

                    string DebitAmount, CreditAmount;
                    DebitAmount = CreditAmount = "0";
                    double dblDebitAmount, dblCreditAmount;
                    dblDebitAmount = dblCreditAmount = 0;
                    if (!String.IsNullOrEmpty(drTransactInfo["Debit"].ToString()))//Because sometimes conversion generates error if its null
                    {
                        dblDebitAmount = Convert.ToDouble(drTransactInfo["Debit"]); ;
                    }
                    if (!String.IsNullOrEmpty(drTransactInfo["Credit"].ToString()))
                    {
                        dblCreditAmount = Convert.ToDouble(drTransactInfo["Credit"]);
                    }
                    totalDrAmt += dblDebitAmount;
                    DebitAmount = Convert.ToDecimal(dblDebitAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    totalCrAmt += Convert.ToDouble(dblCreditAmount);
                    CreditAmount = Convert.ToDecimal(dblCreditAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                    if (Convert.ToDecimal(dblCreditAmount) > 0)
                    {
                        WriteDayBookTransact1(Date.ToSystem(LedgerDate), drTransactInfo["VoucherType"].ToString(), drTransactInfo["VoucherNumber"].ToString(), drTransactInfo["Account"].ToString(), drTransactInfo["LedgerID"].ToString(), " ", CreditAmount, "LEDGER", drTransactInfo["RowID"].ToString(), IsCrystalReport);
                    }
                    else if (Convert.ToDecimal(dblDebitAmount) > 0)
                    {
                        WriteDayBookTransact1(Date.ToSystem(LedgerDate), drTransactInfo["VoucherType"].ToString(), drTransactInfo["VoucherNumber"].ToString(), drTransactInfo["Account"].ToString(), drTransactInfo["LedgerID"].ToString(), DebitAmount, "", "LEDGER", drTransactInfo["RowID"].ToString(), IsCrystalReport);
                    }
                    else
                    {
                        WriteDayBookTransact1(Date.ToSystem(LedgerDate), drTransactInfo["VoucherType"].ToString(), drTransactInfo["VoucherNumber"].ToString(), drTransactInfo["Account"].ToString(), drTransactInfo["LedgerID"].ToString(), DebitAmount, "", "LEDGER", drTransactInfo["RowID"].ToString(), IsCrystalReport);
                    }
                    //Assign Old data for next loop
                    OldVoucherNo = drTransactInfo["VoucherNumber"].ToString();
                    OldVoucherType = drTransactInfo["VoucherType"].ToString();
                    OldRowID = drTransactInfo["RowID"].ToString();
                }
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");
                ////adding one grid for total amount
                //if (!IsCrystalReport)
                // WriteDayBookTransact("", "", "", "TOTAL AMOUNT", totalDrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), totalCrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "", IsCrystalReport);
                ProgressForm.UpdateProgress(100, "Preparing report for display...");
                if (ProgressForm.InvokeRequired)
                    ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion


        }

        private void DisplayCashWiseDayBook(bool IsCrystlalReport)
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


            double totalDrAmt, totalCrAmt;
            totalDrAmt = totalCrAmt = 0;
            try
            {
                if (!IsCrystlalReport)
                    OrientationForCashwise();//FOR HEADER AND OREINTATION
                Transaction m_Transaction = new Transaction();
                ProgressForm.UpdateProgress(40, "Calculating ledger balance...");
                //BLOCK FOR DEBIT CASH BALANCE WISE DAY BOOK
                #region BLOCK FOR DEBIT CASH BALANCE WISE DAY BOOK
                if (m_DayBook.HasDateRange == true)
                {
                    try
                    {
                        string VoucherNo = "";

                        dtTransact = m_Transaction.GetDebitTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);

                        foreach (DataRow drTransact in dtTransact.Rows)
                        {
                            GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                            foreach (DataRow drVoucherMasterInfo in dt.Rows)
                            {
                                VoucherNo = drVoucherMasterInfo["Voucher_No"].ToString();
                            }
                            DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                DateTime transactDate = Convert.ToDateTime(drTransact["TransactDate"]);

                                //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                string dramount = Convert.ToDecimal(drTransact["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                totalDrAmt += Convert.ToDouble(drTransact["Debit_Amount"]);
                                // Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces
                                //WriteDayBookDrCash(transactDate.ToShortDateString(), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), drTransact["Debit_Amount"].ToString(), "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                                WriteDayBookDrCash(Date.ToSystem(transactDate), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), dramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Global.Msg(ex.Message);
                    }
                }
                else
                {
                    DataTable dtTransact = m_Transaction.GetDebitTransactionInfo(null, null, m_DayBook.AccClassID, ProjectIDs);
                    int Sno = 1;
                    string VoucherNo = "";
                    foreach (DataRow drTransact in dtTransact.Rows)
                    {
                        GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                        foreach (DataRow drVoucherMasterInfo in dt.Rows)
                        {
                            VoucherNo = drVoucherMasterInfo["Voucher_No"].ToString();
                        }
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                        foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                        {
                            DateTime transactDate = Convert.ToDateTime(drTransact["TransactDate"]);
                            totalDrAmt += Convert.ToDouble(drTransact["Debit_Amount"]);
                            string dramount = Convert.ToDecimal(drTransact["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            //WriteDayBookDrCash(transactDate.ToShortDateString(), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), drTransact["Debit_Amount"].ToString(), "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                            WriteDayBookDrCash(Date.ToSystem(transactDate), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), dramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                        }
                        Sno++;
                    }
                }
                #endregion

                #region BLOCK FOR CREDIT CASH BALANCE WISE DAY BOOK

                if (m_DayBook.HasDateRange == true)
                {
                    int Sno = 1;
                    // DataTable dtTransact = m_Transaction.GetCreditTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID);
                    DataTable dtTransact = m_Transaction.GetCreditTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);
                    foreach (DataRow drTransact in dtTransact.Rows)
                    {

                        GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                        string VoucherNoCrCash = "";
                        foreach (DataRow drVoucherMasterInfo in dt.Rows)
                        {
                            VoucherNoCrCash = drVoucherMasterInfo["Voucher_No"].ToString();

                        }
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                        foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                        {
                            DateTime dtTransactDate = Convert.ToDateTime(drTransact["TransactDate"]);
                            totalCrAmt += Convert.ToDouble(drTransact["Credit_Amount"]);
                            string cramount = Convert.ToDecimal(drTransact["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            WriteDayBookCrCash(Date.ToSystem(dtTransactDate), drTransact["VoucherType"].ToString(), VoucherNoCrCash, drLedgerInfo["LedName"].ToString(), cramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);

                        }

                        Sno++;
                    }

                }
                else
                {
                    DataTable dtTransact = m_Transaction.GetCreditTransactionInfo(null, null, m_DayBook.AccClassID, ProjectIDs);
                    int Sno = 1;
                    foreach (DataRow drTransact in dtTransact.Rows)
                    {

                        GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                        string VoucherNoCrCash = "";
                        foreach (DataRow drVoucherMasterInfo in dt.Rows)
                        {
                            VoucherNoCrCash = drVoucherMasterInfo["Voucher_No"].ToString();

                        }
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                        foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                        {
                            DateTime dtTransactDate = Convert.ToDateTime(drTransact["TransactDate"]);
                            totalCrAmt += Convert.ToDouble(drTransact["Credit_Amount"]);
                            string cramount = Convert.ToDecimal(drTransact["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            WriteDayBookCrCash(Date.ToSystem(dtTransactDate), drTransact["VoucherType"].ToString(), VoucherNoCrCash, drLedgerInfo["LedName"].ToString(), cramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);

                        }
                        Sno++;
                    }

                }

                #endregion

            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            #region BALANCE DEBIT AND CREDIT ROWS FOR BLANK CELLS
            //Balance the DEBIT and CREDIT rows using blank cells. DEBIT and CREDIT rows may not be same. So need to insert blank cells
            if (AddressDrCashRowsCount > AddressCrCashRowsCount)
            {
                while (AddressDrCashRowsCount > AddressCrCashRowsCount)
                {
                    grdDayBook[AddressCrCashRowsCount, 6] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 7] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 8] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 9] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 10] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 11] = new SourceGrid.Cells.Cell("");
                    AddressCrCashRowsCount++;
                }
            }
            else if (AddressCrCashRowsCount > AddressDrCashRowsCount)
            {
                while (AddressCrCashRowsCount > AddressDrCashRowsCount)
                {
                    grdDayBook[AddressDrCashRowsCount, 0] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 1] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 2] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 3] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 4] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 5] = new SourceGrid.Cells.Cell("");
                    AddressDrCashRowsCount++;
                }
            }
            #endregion

            ProgressForm.UpdateProgress(100, "Preparing report for display...");
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

            //write code for total.....
            //if (!IsCrystlalReport)
            //{
            WriteDayBookDrCash("", "", "", "Total Debit Amount:", totalDrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "", IsCrystlalReport);
            WriteDayBookCrCash("", "", "", "Total Credit Amount:", totalCrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "", IsCrystlalReport);
            //}

        }

        private void DisplayBannar()
        {
            CompanyDetails m_CmpDtl = CompanyInfo.GetInfo();
            if (m_CmpDtl.FYFrom != null)
                // lblAllSettings.Text = "Fiscal Year: " + Date.ToSystem(Convert.ToDateTime(m_CmpDtl.FiscalYear));
                lblAllSettings.Text = "Fiscal Year: " + m_CmpDtl.FiscalYear;
            lblFromDate.Text = Date.ToSystem(m_DayBook.FromDate);
            lblToDate.Text = Date.ToSystem(m_DayBook.ToDate);
            DataTable dtProjectInfo = Project.GetProjectByID(Convert.ToInt32(m_DayBook.ProjectID), LangMgr.DefaultLanguage);
            if (m_DayBook.ProjectID != 0)
            {
                DataRow drProjectInfo = dtProjectInfo.Rows[0];

                lblProjectName.Text = " Project: " + drProjectInfo["Name"].ToString();
            }
            else
            {
                lblProjectName.Text = " Project: " + "All";
            }



        }

        public SelectionBase Selection
        {
            get
            {
                return grdDayBook.Selection as SelectionBase;
            }
        }


        private void frmDayBook_Load(object sender, EventArgs e)
        {
            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grdDayBook_DoubleClick_1);
            dblClick.DoubleClick += new EventHandler(dgDayBook_DoubleClick);
            gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
            grdDayBook.Controller.AddController(gridKeyDown);
            dgDayBook.Controller.AddController(gridKeyDown);

            //Let the whole row to be selected
            grdDayBook.SelectionMode = SourceGrid.GridSelectionMode.Row;
            dgDayBook.SelectionMode = SourceGrid.GridSelectionMode.Row;

            //Set Border
            DevAge.Drawing.RectangleBorder b = Selection.Border;
            b.SetWidth(0);

            Selection.Border = b;

            //Disable multiple selection
            grdDayBook.Selection.EnableMultiSelection = false;
            dgDayBook.Selection.EnableMultiSelection = false;

            ArrayList arrchildProjectIds = new ArrayList();
            Project.GetChildProjects(Convert.ToInt32(m_DayBook.ProjectID), ref arrchildProjectIds);
            ProjectIDs.Add(m_DayBook.ProjectID.ToString());
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDs.Add(p.ToString());
            }

            DisplayBannar();
            //Text format for Total
            GroupView = new SourceGrid.Cells.Views.Cell();
            GroupView.Font = new Font(LangMgr.GetFont(), FontStyle.Bold);
            //Text format for Ledgers
            LedgerView = new SourceGrid.Cells.Views.Cell();
            LedgerView.Font = new Font(LangMgr.GetFont(), FontStyle.Italic);
            LedgerView.ForeColor = Color.Blue;
            AddressBookTransactRowsCount = 1;

            #region BLOCK FOR CASH BALANCE WISE DAYBOOK SHOW

            dgDayBook.Visible = true;
            Journal m_JournalMasterDtl = new Journal();
            if (m_DayBook.CashBalanceWise == true)//BLOCK FOR CASH BALANCE WISE DAY BOOK
            {
                grdDayBook.Visible = true;
                dgDayBook.Visible = false;

                DisplayCashWiseDayBook(false);
            }
            #endregion

            #region BLOCK FOR TRANSACTION WISE DAYBOOK SHOW
            else if (m_DayBook.TransactionWise == true)//BLOCK FOR THE TRANSACTIONWISE
            {
                grdDayBook.Visible = false;
                dgDayBook.Visible = true;

                DisplayTransactWiseBok(false);

            }
            #endregion
        }


        private void DisplayCashWiseDayBook1(bool IsCrystlalReport)
        {
            #region old code
            /*
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


            double totalDrAmt, totalCrAmt;
            totalDrAmt = totalCrAmt = 0;
            try
            {
                if (!IsCrystlalReport)
                    OrientationForCashwise();//FOR HEADER AND OREINTATION
                Transaction m_Transaction = new Transaction();
                ProgressForm.UpdateProgress(40, "Calculating ledger balance...");
                //BLOCK FOR DEBIT CASH BALANCE WISE DAY BOOK
                #region BLOCK FOR DEBIT CASH BALANCE WISE DAY BOOK
                if (m_DayBook.HasDateRange == true)
                {
                    try
                    {
                        string VoucherNo = "";

                        dtTransact = m_Transaction.GetDebitTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);

                        foreach (DataRow drTransact in dtTransact.Rows)
                        {
                            GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                            foreach (DataRow drVoucherMasterInfo in dt.Rows)
                            {
                                VoucherNo = drVoucherMasterInfo["Voucher_No"].ToString();
                            }
                            DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                DateTime transactDate = Convert.ToDateTime(drTransact["TransactDate"]);

                                //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                string dramount = Convert.ToDecimal(drTransact["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                totalDrAmt += Convert.ToDouble(drTransact["Debit_Amount"]);
                                // Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces
                                //WriteDayBookDrCash(transactDate.ToShortDateString(), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), drTransact["Debit_Amount"].ToString(), "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                                WriteDayBookDrCash(Date.ToSystem(transactDate), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), dramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Global.Msg(ex.Message);
                    }
                }
                else
                {
                    DataTable dtTransact = m_Transaction.GetDebitTransactionInfo(null, null, m_DayBook.AccClassID, ProjectIDs);
                    int Sno = 1;
                    string VoucherNo = "";
                    foreach (DataRow drTransact in dtTransact.Rows)
                    {
                        GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                        foreach (DataRow drVoucherMasterInfo in dt.Rows)
                        {
                            VoucherNo = drVoucherMasterInfo["Voucher_No"].ToString();
                        }
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                        foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                        {
                            DateTime transactDate = Convert.ToDateTime(drTransact["TransactDate"]);
                            totalDrAmt += Convert.ToDouble(drTransact["Debit_Amount"]);
                            string dramount = Convert.ToDecimal(drTransact["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            //WriteDayBookDrCash(transactDate.ToShortDateString(), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), drTransact["Debit_Amount"].ToString(), "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                            WriteDayBookDrCash(Date.ToSystem(transactDate), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), dramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                        }
                        Sno++;
                    }
                }
                #endregion

                #region BLOCK FOR CREDIT CASH BALANCE WISE DAY BOOK

                if (m_DayBook.HasDateRange == true)
                {
                    int Sno = 1;
                    // DataTable dtTransact = m_Transaction.GetCreditTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID);
                    DataTable dtTransact = m_Transaction.GetCreditTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);
                    foreach (DataRow drTransact in dtTransact.Rows)
                    {

                        GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                        string VoucherNoCrCash = "";
                        foreach (DataRow drVoucherMasterInfo in dt.Rows)
                        {
                            VoucherNoCrCash = drVoucherMasterInfo["Voucher_No"].ToString();

                        }
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                        foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                        {
                            DateTime dtTransactDate = Convert.ToDateTime(drTransact["TransactDate"]);
                            totalCrAmt += Convert.ToDouble(drTransact["Credit_Amount"]);
                            string cramount = Convert.ToDecimal(drTransact["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            WriteDayBookCrCash(Date.ToSystem(dtTransactDate), drTransact["VoucherType"].ToString(), VoucherNoCrCash, drLedgerInfo["LedName"].ToString(), cramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);

                        }

                        Sno++;
                    }

                }
                else
                {
                    DataTable dtTransact = m_Transaction.GetCreditTransactionInfo(null, null, m_DayBook.AccClassID, ProjectIDs);
                    int Sno = 1;
                    foreach (DataRow drTransact in dtTransact.Rows)
                    {

                        GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                        string VoucherNoCrCash = "";
                        foreach (DataRow drVoucherMasterInfo in dt.Rows)
                        {
                            VoucherNoCrCash = drVoucherMasterInfo["Voucher_No"].ToString();

                        }
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                        foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                        {
                            DateTime dtTransactDate = Convert.ToDateTime(drTransact["TransactDate"]);
                            totalCrAmt += Convert.ToDouble(drTransact["Credit_Amount"]);
                            string cramount = Convert.ToDecimal(drTransact["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            WriteDayBookCrCash(Date.ToSystem(dtTransactDate), drTransact["VoucherType"].ToString(), VoucherNoCrCash, drLedgerInfo["LedName"].ToString(), cramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);

                        }
                        Sno++;
                    }

                }

                #endregion

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            #region BALANCE DEBIT AND CREDIT ROWS FOR BLANK CELLS
            //Balance the DEBIT and CREDIT rows using blank cells. DEBIT and CREDIT rows may not be same. So need to insert blank cells
            if (AddressDrCashRowsCount > AddressCrCashRowsCount)
            {
                while (AddressDrCashRowsCount > AddressCrCashRowsCount)
                {
                    grdDayBook[AddressCrCashRowsCount, 6] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 7] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 8] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 9] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 10] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 11] = new SourceGrid.Cells.Cell("");
                    AddressCrCashRowsCount++;
                }
            }
            else if (AddressCrCashRowsCount > AddressDrCashRowsCount)
            {
                while (AddressCrCashRowsCount > AddressDrCashRowsCount)
                {
                    grdDayBook[AddressDrCashRowsCount, 0] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 1] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 2] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 3] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 4] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 5] = new SourceGrid.Cells.Cell("");
                    AddressDrCashRowsCount++;
                }
            }
            #endregion

            ProgressForm.UpdateProgress(100, "Preparing report for display...");
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

            //write code for total.....
            //if (!IsCrystlalReport)
            //{
            WriteDayBookDrCash("", "", "", "Total Debit Amount:", totalDrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "", IsCrystlalReport);
            WriteDayBookCrCash("", "", "", "Total Credit Amount:", totalCrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "", IsCrystlalReport);
            //}
             * */
            #endregion

            frmProgress ProgressForm = new frmProgress();
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    ProgressForm.ShowDialog();
                }
            ));

            string AccClassIDsXMLString = ReadAllAccClassID();
            string ProjectIDsXMLString = ReadAllProjectID();
            string[] test = { "1" };
            backgroundThread.Start();
            // Update the progressbar
            ProgressForm.UpdateProgress(20, "Initializing Report Viewer...");


            Transaction m_Transaction = new Transaction();
            DataTable dtTransact = new DataTable();
            DataTable dtTransact1 = new DataTable();
            dtTransact = m_Transaction.GetDebitTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);
            dtTransact1 = m_Transaction.GetCreditTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);
            drFound = dtTransact.Select();
            drFound = dtTransact1.Select();

            double totalDrAmt, totalCrAmt;
            totalDrAmt = totalCrAmt = 0;
            try
            {

                ProgressForm.UpdateProgress(40, "Calculating ledger balance...");
                //BLOCK FOR DEBIT CASH BALANCE WISE DAY BOOK

                grdDayBook.Rows.Clear();
                grdDayBook.Redim(drFound.Count() + 1, 12);
                WriteHeaderCashwise();

                string VoucherNo = "";
                string LedName = "";

                #region BLOCK FOR DEBIT CASH BALANCE WISE DAY BOOK
                if (m_DayBook.HasDateRange == true)
                {
                    try
                    {


                        // dtTransact = m_Transaction.GetDebitTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);

                        for (int i = 1; i <= drFound.Count(); i++)
                        {
                            // int Sno = 1;
                            DataRow drTransact = drFound[i - 1];


                            string TransactDate = (drTransact["TransactDate"].ToString());
                            //Is used only to display english date converting to nepali for report
                            TransactDate = BusinessLogic.NepDateConverter.EngToNep(Convert.ToDateTime(TransactDate)).ToString();

                            DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                            GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());

                            VoucherType = drTransact["VoucherType"].ToString();

                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                LedName = drLedgerInfo["LedName"].ToString();
                            }

                            foreach (DataRow drVoucherMasterInfo in dt.Rows)
                            {
                                VoucherNo = drVoucherMasterInfo["Voucher_No"].ToString();
                            }




                            grdDayBook[i, (int)GridColumn.LedgerDate] = new SourceGrid.Cells.Cell(TransactDate);
                            grdDayBook[i, (int)GridColumn.LedgerDate].AddController(dblClick);
                            grdDayBook[i, (int)GridColumn.LedgerDate].AddController(gridKeyDown);

                            grdDayBook[i, (int)GridColumn.VoucherType] = new SourceGrid.Cells.Cell(VoucherType);
                            grdDayBook[i, (int)GridColumn.VoucherType].AddController(dblClick);
                            grdDayBook[i, (int)GridColumn.VoucherType].AddController(gridKeyDown);

                            grdDayBook[i, (int)GridColumn.VoucherNo] = new SourceGrid.Cells.Cell(VoucherNo);
                            grdDayBook[i, (int)GridColumn.VoucherNo].AddController(dblClick);
                            grdDayBook[i, (int)GridColumn.VoucherNo].AddController(gridKeyDown);

                            grdDayBook[i, (int)GridColumn.AccountName] = new SourceGrid.Cells.Cell(LedName);
                            grdDayBook[i, (int)GridColumn.AccountName].AddController(dblClick);
                            grdDayBook[i, (int)GridColumn.AccountName].AddController(gridKeyDown);


                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                DateTime transactDate = Convert.ToDateTime(drTransact["TransactDate"]);

                                //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                string dramount = Convert.ToDecimal(drTransact["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                totalDrAmt += Convert.ToDouble(drTransact["Credit_Amount"]);

                                grdDayBook[i, (int)GridColumn.DebitAmt] = new SourceGrid.Cells.Cell(dramount);
                                grdDayBook[i, (int)GridColumn.DebitAmt].AddController(dblClick);
                                grdDayBook[i, (int)GridColumn.DebitAmt].AddController(gridKeyDown);
                            }

                            //grdDayBook[i, (int)GridColumn.DebitAmt] = new SourceGrid.Cells.Cell(dramount);
                            //grdDayBook[i, (int)GridColumn.DebitAmt].AddController(dblClick);
                            //grdDayBook[i, (int)GridColumn.DebitAmt].AddController(gridKeyDown);

                            //grdDayBook[i, (int)GridColumn.RowID] = new SourceGrid.Cells.Cell(drTransact["RowID"].ToString());
                            //grdDayBook[i, (int)GridColumn.RowID].AddController(dblClick);


                            // WriteDayBookDrCash(Date.ToSystem(transactDate), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(),     , "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Msg(ex.Message);
                    }
                }
                else
                    try
                    {
                        // dtTransact = m_Transaction.GetDebitTransactionInfo(null, null, m_DayBook.AccClassID, ProjectIDs);

                        for (int i = 1; i <= drFound.Count(); i++)
                        {
                            // int Sno = 1;
                            DataRow drTransact = drFound[i - 1];

                            string TransactDate = (drTransact["TransactDate"].ToString());
                            //Is used only to display english date converting to nepali for report
                            TransactDate = BusinessLogic.NepDateConverter.EngToNep(Convert.ToDateTime(TransactDate)).ToString();

                            DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]), LangMgr.DefaultLanguage);
                            GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                            VoucherType = drTransact["VoucherType"].ToString();
                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                LedName = drLedgerInfo["LedName"].ToString();
                            }

                            foreach (DataRow drVoucherMasterInfo in dt.Rows)
                            {
                                VoucherNo = drVoucherMasterInfo["Voucher_No"].ToString();
                            }


                            grdDayBook[i, (int)GridColumn.LedgerDate] = new SourceGrid.Cells.Cell(TransactDate);
                            grdDayBook[i, (int)GridColumn.LedgerDate].AddController(dblClick);
                            grdDayBook[i, (int)GridColumn.LedgerDate].AddController(gridKeyDown);

                            grdDayBook[i, (int)GridColumn.VoucherType] = new SourceGrid.Cells.Cell(VoucherType);
                            grdDayBook[i, (int)GridColumn.VoucherType].AddController(dblClick);
                            grdDayBook[i, (int)GridColumn.VoucherType].AddController(gridKeyDown);

                            grdDayBook[i, (int)GridColumn.VoucherNo] = new SourceGrid.Cells.Cell(VoucherNo);
                            grdDayBook[i, (int)GridColumn.VoucherNo].AddController(dblClick);
                            grdDayBook[i, (int)GridColumn.VoucherNo].AddController(gridKeyDown);

                            grdDayBook[i, (int)GridColumn.AccountName] = new SourceGrid.Cells.Cell(LedName);
                            grdDayBook[i, (int)GridColumn.AccountName].AddController(dblClick);
                            grdDayBook[i, (int)GridColumn.AccountName].AddController(gridKeyDown);

                            string dramount = "";
                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                DateTime transactDate = Convert.ToDateTime(drTransact["TransactDate"]);

                                //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                dramount = Convert.ToDecimal(drTransact["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                totalDrAmt += Convert.ToDouble(drTransact["Debit_Amount"]);

                            }

                            grdDayBook[i, (int)GridColumn.DebitAmt] = new SourceGrid.Cells.Cell(dramount);
                            grdDayBook[i, (int)GridColumn.DebitAmt].AddController(dblClick);
                            grdDayBook[i, (int)GridColumn.DebitAmt].AddController(gridKeyDown);

                            //  grdDayBook[i, (int)GridColumn.RowID] = new SourceGrid.Cells.Cell(drTransact["RowID"].ToString());



                            //    WriteDayBookDrCash(Date.ToSystem(transactDate), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), dramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Msg(ex.Message);
                    }



                #endregion


                #region BLOCK FOR CREDIT CASH BALANCE WISE DAY BOOK

                if (m_DayBook.HasDateRange == true)
                {
                    try
                    {

                        // dtTransact1 = m_Transaction.GetCreditTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);

                        for (int i = 1; i <= drFound.Count(); i++)
                        {// int Sno = 1;
                            DataRow drTransact1 = drFound[i - 1];

                            // DateTime dtTransactDate = Convert.ToDateTime(drTransact1["TransactDate"]);
                            string TransactDate = (drTransact1["TransactDate"].ToString());
                            //Is used only to display english date converting to nepali for report
                            TransactDate = BusinessLogic.NepDateConverter.EngToNep(Convert.ToDateTime(TransactDate)).ToString();

                            DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact1["LedgerID"]), LangMgr.DefaultLanguage);
                            GetVoucherMasterInfo(drTransact1["RowID"].ToString(), drTransact1["VoucherType"].ToString());

                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                LedName = drLedgerInfo["LedName"].ToString();
                            }

                            foreach (DataRow drVoucherMasterInfo in dt.Rows)
                            {
                                VoucherNo = drVoucherMasterInfo["Voucher_No"].ToString();
                            }


                            grdDayBook[i, 6] = new SourceGrid.Cells.Cell(TransactDate);
                            grdDayBook[i, 6].AddController(dblClick);
                            //   grdDayBook[i, (int)GridColumn.LedgerDate].AddController(gridKeyDown);

                            grdDayBook[i, 10] = new SourceGrid.Cells.Cell(drTransact1["VoucherType"].ToString());
                            grdDayBook[i, 10].AddController(dblClick);
                            //grdDayBook[i, (int)GridColumn.VoucherType].AddController(gridKeyDown);

                            grdDayBook[i, 7] = new SourceGrid.Cells.Cell(VoucherNo);
                            grdDayBook[i, 7].AddController(dblClick);
                            // grdDayBook[i, (int)GridColumn.VoucherNo].AddController(gridKeyDown);

                            grdDayBook[i, 8] = new SourceGrid.Cells.Cell(LedName);
                            grdDayBook[i, 8].AddController(dblClick);
                            // grdDayBook[i, (int)GridColumn.AccountName].AddController(gridKeyDown);

                            string cramount = "";
                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                DateTime transactDate = Convert.ToDateTime(drTransact1["TransactDate"]);

                                //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                cramount = Convert.ToDecimal(drTransact1["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                totalCrAmt += Convert.ToDouble(drTransact1["Credit_Amount"]);


                            }
                            grdDayBook[i, 9] = new SourceGrid.Cells.Cell(cramount);
                            grdDayBook[i, 9].AddController(dblClick);
                            // grdDayBook[i, (int)GridColumn.CreditAmt].AddController(gridKeyDown);

                            //grdDayBook[i, (int)GridColumn.RowID] = new SourceGrid.Cells.Cell(drTransact1["RowID"].ToString());
                            //grdDayBook[i, (int)GridColumn.RowID].AddController(dblClick);
                            //grdDayBook[i, (int)GridColumn.RowID].AddController(gridKeyDown);

                            // WriteDayBookDrCash(Date.ToSystem(transactDate), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), dramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Msg(ex.Message);
                    }
                }
                else
                    try
                    {

                        // dtTransact1 = m_Transaction.GetCreditTransactionInfo(null, null, m_DayBook.AccClassID, ProjectIDs);

                        for (int i = 1; i <= drFound.Count(); i++)
                        {
                            // int Sno = 1;
                            DataRow drTransact1 = drFound[i - 1];

                            string TransactDate = (drTransact1["TransactDate"].ToString());
                            //Is used only to display english date converting to nepali for report
                            TransactDate = BusinessLogic.NepDateConverter.EngToNep(Convert.ToDateTime(TransactDate)).ToString();

                            DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact1["LedgerID"]), LangMgr.DefaultLanguage);
                            GetVoucherMasterInfo(drTransact1["RowID"].ToString(), drTransact1["VoucherType"].ToString());

                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                LedName = drLedgerInfo["LedName"].ToString();
                            }

                            foreach (DataRow drVoucherMasterInfo in dt.Rows)
                            {
                                VoucherNo = drVoucherMasterInfo["Voucher_No"].ToString();
                            }

                            grdDayBook[i, 6] = new SourceGrid.Cells.Cell(TransactDate);
                            grdDayBook[i, 6].AddController(dblClick);
                            //   grdDayBook[i, (int)GridColumn.LedgerDate].AddController(gridKeyDown);

                            grdDayBook[i, 10] = new SourceGrid.Cells.Cell(drTransact1["VoucherType"].ToString());
                            grdDayBook[i, 10].AddController(dblClick);
                            //grdDayBook[i, (int)GridColumn.VoucherType].AddController(gridKeyDown);

                            grdDayBook[i, 7] = new SourceGrid.Cells.Cell(VoucherNo);
                            grdDayBook[i, 7].AddController(dblClick);
                            // grdDayBook[i, (int)GridColumn.VoucherNo].AddController(gridKeyDown);

                            grdDayBook[i, 8] = new SourceGrid.Cells.Cell(LedName);
                            grdDayBook[i, 8].AddController(dblClick);
                            // grdDayBook[i, (int)GridColumn.AccountName].AddController(gridKeyDown);

                            string cramount = "";
                            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                            {
                                DateTime transactDate = Convert.ToDateTime(drTransact1["TransactDate"]);

                                //closingamount = Convert.ToDecimal(dr["Credit"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                cramount = Convert.ToDecimal(drTransact1["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                                totalCrAmt += Convert.ToDouble(drTransact1["Credit_Amount"]);


                            }
                            grdDayBook[i, 9] = new SourceGrid.Cells.Cell(cramount);
                            grdDayBook[i, 9].AddController(dblClick);
                            // grdDayBook[i, (int)GridColumn.CreditAmt].AddController(gridKeyDown);

                            //grdDayBook[i, (int)GridColumn.RowID] = new SourceGrid.Cells.Cell(drTransact1["RowID"].ToString());
                            //grdDayBook[i, (int)GridColumn.RowID].AddController(dblClick);
                            //grdDayBook[i, (int)GridColumn.RowID].AddController(gridKeyDown);

                            //    WriteDayBookDrCash(Date.ToSystem(transactDate), drTransact["VoucherType"].ToString(), VoucherNo, drLedgerInfo["LedName"].ToString(), dramount, "LEDGER", drTransact["RowID"].ToString(), IsCrystlalReport);
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Msg(ex.Message);
                    }

                #endregion

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

            #region BALANCE DEBIT AND CREDIT ROWS FOR BLANK CELLS
            //Balance the DEBIT and CREDIT rows using blank cells. DEBIT and CREDIT rows may not be same. So need to insert blank cells
            if (AddressDrCashRowsCount > AddressCrCashRowsCount)
            {
                while (AddressDrCashRowsCount > AddressCrCashRowsCount)
                {
                    grdDayBook[AddressCrCashRowsCount, 6] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 7] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 8] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 9] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 10] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressCrCashRowsCount, 11] = new SourceGrid.Cells.Cell("");
                    AddressCrCashRowsCount++;
                }
            }
            else if (AddressCrCashRowsCount > AddressDrCashRowsCount)
            {
                while (AddressCrCashRowsCount > AddressDrCashRowsCount)
                {
                    grdDayBook[AddressDrCashRowsCount, 0] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 1] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 2] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 3] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 4] = new SourceGrid.Cells.Cell("");
                    grdDayBook[AddressDrCashRowsCount, 5] = new SourceGrid.Cells.Cell("");
                    AddressDrCashRowsCount++;
                }
            }
            #endregion

            ProgressForm.UpdateProgress(100, "Preparing report for display...");
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

            //write code for total.....
            //if (!IsCrystlalReport)
            //{
            //WriteDayBookDrCash("", "", "", "Total Debit Amount:", totalDrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "GROUP", "", IsCrystlalReport);
            //WriteDayBookCrCash("", "", "", "Total Credit Amount:", totalCrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.Decimal  Places)), "GROUP", "", IsCrystlalReport);
            //}
        }

        private void DisplayTransactWiseBok(bool IsCrystalReport)
        {
            string strXML = "";
            try
            {
                System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
                tw.WriteStartDocument();

                #region  Accountclass
                tw.WriteStartElement("AccClassIDSettings");
                //Write Checked Accounting class ID
                try
                {
                    foreach (string tag in m_DayBook.AccClassID)
                    {
                        tw.WriteElementString("AccClassID", Convert.ToInt32(tag).ToString());
                    }
                }
                catch
                { }
                tw.WriteFullEndElement();
                #endregion

                tw.WriteEndDocument();
                tw.Flush();
                tw.Close();
                strXML = AEncoder.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
            }

            //For ProjectId
            string strXMLP = "";
            try
            {
                System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
                tw.WriteStartDocument();

                #region  Project

                tw.WriteStartElement("ProjectIDCollection");
                //Write Checked Project class ID
                try
                {
                    for (int i = 0; i <= m_DayBook.ProjectID; i++)
                    {
                        tw.WriteElementString("ProjectID", Convert.ToInt32(i).ToString());
                    }
                }
                catch
                { }
                tw.WriteFullEndElement();
                #endregion

                tw.WriteEndDocument();
                tw.Flush();
                tw.Close();
                strXMLP = AEncoder.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
            }
            string AccClassIDsXMLString = strXML;
            string ProjectIDsXMLString = strXMLP;



            #region NEW ONE
            //call spGetLedgerTransaction
            try
            {

                Global.m_db.ClearParameter();
                Global.m_db.setCommandType(CommandType.StoredProcedure);
                Global.m_db.setCommandText("Acc.spGetTransactionDetails");

                // if(FromDate!=null)
                Global.m_db.AddParameter("@FromDate", SqlDbType.DateTime, m_DayBook.FromDate);
                //if(ToDate!=null)
                Global.m_db.AddParameter("@ToDate", SqlDbType.DateTime, m_DayBook.ToDate);
                Global.m_db.AddParameter("@AccountClassIDsSettings", SqlDbType.Xml, AccClassIDsXMLString);
                Global.m_db.AddParameter("@ProjectIDsSettings", SqlDbType.Xml, ProjectIDsXMLString);
                Global.m_db.AddParameter("@Settings", SqlDbType.Xml, null);

                DataTable dtTransactionDetails = Global.m_db.GetDataTable();
                System.Data.DataView view = new System.Data.DataView(dtTransactionDetails);

                Decimal sumD = 0;
                var sd = "";
                Decimal sumC = 0;

                foreach (DataRow dr in dtTransactionDetails.Rows)
                {
                    sumD = Convert.ToDecimal(dr["Debit"]);
                    sd = sumD.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    sumC = Convert.ToDecimal(dr["Credit"]);
                }

                //drFound1 = dtTransactionDetails.Select();
                // DataRow drTransactInfoo = drFound1[1];


               
                System.Data.DataTable selected = view.ToTable("tblDayBookTransact", false, "LedgerNepDate", "VoucherType", "VoucherNumber", "Account", "Debit", "Credit", "RowID", "LedgerID");
                //Global.m_db.DataAdaptarFill(dsDayBookTransact,"tblDayBookTransact", "");

                //System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
                //da.Fill(dsDayBookTransact.Tables["tblDayBookTransact"]);

                dsDayBookTransact.Tables.Remove("tblDayBookTransact");

                dsDayBookTransact.Tables.Add(selected);
                IEnumerable<DataRow> query = from dtTranactionDetail in dtTransactionDetails.AsEnumerable()
                                             where dtTranactionDetail.Field<String>("VoucherType") == "JRNL"
                                             select dtTranactionDetail;


                //Test
                //Decimal sumDr = ((from s in dtTransactionDetails.AsEnumerable()
                //                  select decimal.Parse(s[4].ToString())) as IEnumerable<decimal>).Sum();
                //Decimal sumCr = ((from s in dtTransactionDetails.AsEnumerable()
                //                  select decimal.Parse(s[5].ToString())) as IEnumerable<decimal>).Sum();


                Decimal sumDr = 0;
                Decimal sumCr = 0;

                foreach (DataRow dr in dtTransactionDetails.Rows)
                {
                    sumDr += Convert.ToDecimal(dr["Debit"]);
                    sumCr += Convert.ToDecimal(dr["Credit"]);
                }
                label1.Text = sumDr.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                label7.Text = sumCr.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));


                // Create a table from the query.
                DataTable boundTable = query.CopyToDataTable<DataRow>();

                #region TEST for datagrid binding
                DataView mView;
                mView = selected.DefaultView;

                dgDayBook.FixedRows = 1; //Allocated for Header

                //Header row
                dgDayBook.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dgDayBook));

                DevAge.ComponentModel.IBoundList bindList = new DevAge.ComponentModel.BoundDataView(mView);

                //Create default columns
                CreateColumns(dgDayBook.Columns, bindList);
                dgDayBook.DataSource = bindList;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;

            }

            //Put them into DataSet
            //Show them in Grid

            #endregion
            return;

            frmProgress ProgressForm = new frmProgress();
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    ProgressForm.ShowDialog();
                }
            ));

            backgroundThread.Start();
            //Update the progressbar
            ProgressForm.UpdateProgress(20, "Initializing Report Viewer...");

            DataTable dtTransactInfo = new DataTable();
            dtTransactInfo = Transaction.GetTransactionDetails(m_DayBook.FromDate, m_DayBook.ToDate, AccClassIDsXMLString, ProjectIDsXMLString);
            drFound = dtTransactInfo.Select();

            if (!IsCrystalReport)
                OrientationForTranwise1(); //This function for oreintation purpose like header,font,style etc.    
            try
            {
                double totalDrAmt, totalCrAmt;
                totalDrAmt = totalCrAmt = 0;
                ProgressForm.UpdateProgress(40, "Calculating ledger balance...");


                grdDayBook.Rows.Clear();
                grdDayBook.Redim(drFound.Count() + 1, 8);
                WriteHeaderTransactWise1();


                if (m_DayBook.HasDateRange == true)
                {
                    dtTransactInfo = Transaction.GetTransactionDetails(m_DayBook.FromDate, m_DayBook.ToDate, AccClassIDsXMLString, ProjectIDsXMLString);
                }
                else
                {
                    dtTransactInfo = Transaction.GetTransactionDetails(null, null, AccClassIDsXMLString, ProjectIDsXMLString);
                }

                for (int i = 1; i <= drFound.Count(); i++)
                {
                    DataRow drTransactInfo = drFound[i - 1];

                    string LedgerDate = (drTransactInfo["LedgerNepDate"].ToString());

                    //Is used only to display english date converting to nepali for report
                    LedgerDate = BusinessLogic.NepDateConverter.EngToNep(Convert.ToDateTime(LedgerDate)).ToString();


                    grdDayBook[i, (int)GridColumn.LedgerDate] = new SourceGrid.Cells.Cell(LedgerDate);
                    grdDayBook[i, (int)GridColumn.LedgerDate].AddController(dblClick);
                    grdDayBook[i, (int)GridColumn.LedgerDate].AddController(gridKeyDown);

                    grdDayBook[i, (int)GridColumn.VoucherNo] = new SourceGrid.Cells.Cell(drTransactInfo["VoucherNumber"].ToString());
                    grdDayBook[i, (int)GridColumn.VoucherNo].AddController(dblClick);

                    grdDayBook[i, (int)GridColumn.AccountName] = new SourceGrid.Cells.Cell(drTransactInfo["Account"].ToString());
                    grdDayBook[i, (int)GridColumn.AccountName].AddController(dblClick);
                    grdDayBook[i, (int)GridColumn.AccountName].AddController(gridKeyDown);

                    grdDayBook[i, (int)GridColumn.DebitAmt] = new SourceGrid.Cells.Cell(drTransactInfo["Debit"].ToString());
                    grdDayBook[i, (int)GridColumn.DebitAmt].AddController(dblClick);
                    grdDayBook[i, (int)GridColumn.DebitAmt].AddController(gridKeyDown);

                    grdDayBook[i, (int)GridColumn.CreditAmt] = new SourceGrid.Cells.Cell(drTransactInfo["Credit"].ToString());
                    grdDayBook[i, (int)GridColumn.CreditAmt].AddController(dblClick);
                    grdDayBook[i, (int)GridColumn.CreditAmt].AddController(gridKeyDown);

                    grdDayBook[i, (int)GridColumn.VoucherType] = new SourceGrid.Cells.Cell(drTransactInfo["VoucherType"].ToString());
                    grdDayBook[i, (int)GridColumn.VoucherType].AddController(dblClick);
                    grdDayBook[i, (int)GridColumn.VoucherType].AddController(gridKeyDown);

                    grdDayBook[i, (int)GridColumn.RowID] = new SourceGrid.Cells.Cell(drTransactInfo["RowID"].ToString());
                    grdDayBook[i, (int)GridColumn.RowID].AddController(dblClick);
                    grdDayBook[i, (int)GridColumn.RowID].AddController(gridKeyDown);

                    grdDayBook[i, (int)GridColumn.Ledger_ID] = new SourceGrid.Cells.Cell(drTransactInfo["LedgerID"].ToString());
                    grdDayBook[i, (int)GridColumn.Ledger_ID].AddController(dblClick);
                    grdDayBook[i, (int)GridColumn.Ledger_ID].AddController(gridKeyDown);

                    string DebitAmount, CreditAmount;
                    DebitAmount = CreditAmount = "0";
                    double dblDebitAmount, dblCreditAmount;
                    dblDebitAmount = dblCreditAmount = 0;

                    if (!String.IsNullOrEmpty(drTransactInfo["Debit"].ToString()))
                    {
                        dblDebitAmount = Convert.ToDouble(drTransactInfo["Debit"]);
                    }

                    if (!String.IsNullOrEmpty(drTransactInfo["Debit"].ToString()))//Because sometimes conversion generates error if its null
                    {
                        dblDebitAmount = Convert.ToDouble(drTransactInfo["Debit"]);
                    }

                    if (!String.IsNullOrEmpty(drTransactInfo["Credit"].ToString()))
                    {
                        dblCreditAmount = Convert.ToDouble(drTransactInfo["Credit"]);
                    }
                    totalDrAmt += dblDebitAmount;
                    DebitAmount = Convert.ToDecimal(dblDebitAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    totalCrAmt += Convert.ToDouble(dblCreditAmount);
                    CreditAmount = Convert.ToDecimal(dblCreditAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

                    if (Convert.ToDecimal(dblCreditAmount) > 0)
                    {
                        grdDayBook[i, (int)GridColumn.DebitAmt] = new SourceGrid.Cells.Cell("");

                    }
                    else if (Convert.ToDecimal(dblDebitAmount) > 0)
                    {
                        grdDayBook[i, (int)GridColumn.CreditAmt] = new SourceGrid.Cells.Cell("");
                    }
                    else
                    {
                        grdDayBook[i, (int)GridColumn.DebitAmt] = new SourceGrid.Cells.Cell((0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                        grdDayBook[i, (int)GridColumn.CreditAmt] = new SourceGrid.Cells.Cell("");

                    }
                    // Assign Old data for next loop
                    OldVoucherNo = drTransactInfo["VoucherNumber"].ToString();
                    OldVoucherType = drTransactInfo["VoucherType"].ToString();
                    OldRowID = drTransactInfo["RowID"].ToString();

                    //To display for report printing
                    if (IsCrystalReport)
                    {
                        dsDayBookTransact.Tables["tblDayBookTransact"].Rows.Add(LedgerDate, drTransactInfo["VoucherType"].ToString(), drTransactInfo["VoucherNumber"].ToString(), drTransactInfo["Account"].ToString(), DebitAmount, CreditAmount, drTransactInfo["RowID"].ToString(), drTransactInfo["LedgerID"].ToString());
                    }
                }
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");
                label1.Text = totalDrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                label7.Text = totalCrAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));


                m_DebitTotal = totalDrAmt.ToString();
                m_CreditTotal = totalCrAmt.ToString();


                ProgressForm.UpdateProgress(100, "Preparing report for display...");
                if (ProgressForm.InvokeRequired)
                    ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string ReadAllAccClassID()
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in m_DayBook.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            m_DayBook.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in m_DayBook.AccClassID)
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
            Project.GetChildProjects(Convert.ToInt32(m_DayBook.ProjectID), ref arrchildProjectIds);
            ArrayList ProjectIDCollection = new ArrayList();
            foreach (object obj in arrchildProjectIds)
            {
                int p = (int)obj;
                ProjectIDCollection.Add(p.ToString());
            }

            string ProjectIDS = "'" + m_DayBook.ProjectID + "'";

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
                    tw.WriteElementString("PID", Convert.ToInt32(m_DayBook.ProjectID).ToString());
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

        private void GetVoucherMasterInfo(string RowID, string VoucherType)
        {
            try
            {
                //Use switch case for each voucherType
                switch (VoucherType)
                {
                    case "JRNL":
                        dt = m_journal.GetJournalMasterDtl(RowID);
                        break;
                    case "CASH_RCPT":
                        dt = m_CashRecipt.GetCashReceiptMaster(Convert.ToInt32(RowID));
                        break;
                    case "CASH_PMNT":
                        dt = m_CashPayment.GetCashPaymentMaster(Convert.ToInt32(RowID));
                        break;
                    case "BANK_RCPT":
                        dt = m_BankReceipt.GetBankReceiptMaster(Convert.ToInt32(RowID));
                        break;
                    case "BANK_PMNT":
                        dt = m_BankPayment.GetBankPaymentMaster(Convert.ToInt32(RowID));
                        break;
                    case "CNTR":
                        dt = m_Contra.GetContraMasterInfo(Convert.ToInt32(RowID));
                        break;
                    case "SALES":
                        dt = m_Sales.GetSalesInvoiceMasterInfo(RowID);
                        break;
                    case "DR_NOT":
                        dt = m_DebitNote.GetDebitNoteMaster(Convert.ToInt32(RowID));
                        break;
                    case "CR_NOT":
                        dt = m_CreditNote.GetCreditNoteMaster(Convert.ToInt32(RowID));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void grdDayBook_DoubleClick(object sender, EventArgs e)
        {
            //grdDayBook.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            int CurRow = dgDayBook.Selection.GetSelectionRegion().GetRowsIndex()[0];
            string VoucherType = "";
            if (m_DayBook.TransactionWise == true)//for Transactionwise daybook           
            {
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, (int)GridColumn.RowID));
                if (cellType.Value != "")
                {
                    RowID = Convert.ToInt32(cellType.Value);
                    SourceGrid.CellContext cellVouTypeTran = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, (int)GridColumn.VoucherType));
                    VoucherType = cellVouTypeTran.Value.ToString();
                }

            }
            else if (m_DayBook.CashBalanceWise == true)//For CashBalancewise daybook
            {
                //find colulmn position at first
                int CurColumn = grdDayBook.Selection.GetSelectionRegion().GetColumnsIndex()[0];
                if (CurColumn <= 5)//This is the portion of Debit side
                {
                    SourceGrid.CellContext cellType1 = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, 5));
                    if (cellType1.Value != "")
                    {
                        RowID = Convert.ToInt32(cellType1.Value);
                        SourceGrid.CellContext cellVouTypeTran = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, 1));
                        VoucherType = cellVouTypeTran.Value.ToString();
                    }
                }
                else if (CurColumn >= 6)//This is the portion of Credit Side
                {
                    SourceGrid.CellContext cellType2 = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, 11));
                    if (cellType2.Value != "")
                    {
                        RowID = Convert.ToInt32(cellType2.Value);
                        SourceGrid.CellContext cellVouTypeTran = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, 7));
                        VoucherType = cellVouTypeTran.Value.ToString();
                    }
                }

            }

            switch (VoucherType)
            {
                case "JRNL":
                    frmJournal frm = new frmJournal(RowID);
                    frm.Show();
                    break;
                case "DR_NOT":
                    frmDebitNote frm1 = new frmDebitNote(RowID);
                    frm1.Show();
                    break;
                case "CR_NOT":
                    frmCreditNote frm2 = new frmCreditNote(RowID);
                    frm2.Show();
                    break;
                case "CASH_PMNT":
                    frmCashPayment frm3 = new frmCashPayment(RowID);
                    frm3.Show();
                    break;
                case "CASH_RCPT":
                    frmCashReceipt frm4 = new frmCashReceipt(RowID);
                    frm4.Show();

                    break;
                case "BANK_PMNT":
                    frmBankPayment frm5 = new frmBankPayment(RowID);
                    frm5.Show();
                    break;
                case "BANK_RCPT":
                    frmBankReceipt frm6 = new frmBankReceipt(RowID);
                    frm6.Show();
                    break;
                case "CNTR":
                    frmContra frm7 = new frmContra(RowID);
                    frm7.Show();
                    break;
                case "SALES":
                    frmSalesInvoice frm8 = new frmSalesInvoice(RowID);
                    frm8.Show();
                    break;
                case "SLS_RTN":
                    frmSalesReturn frm9 = new frmSalesReturn(RowID);
                    frm9.Show();
                    break;
                case "PURCH":
                    frmPurchaseInvoice frm10 = new frmPurchaseInvoice(RowID);
                    frm10.Show();
                    break;
                case "PURCH_RTN":
                    frmPurchaseReturn frm11 = new frmPurchaseReturn(RowID);
                    frm11.Show();
                    break;
            }

        }

        //when enter key is pressed
        private void grdDayBook_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                grdDayBook_DoubleClick_1(sender, null);
        }
        //whenever escape button is pressed,window form will be closed....
        private void frmDayBook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }


        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
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

            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

            if (m_DayBook.TransactionWise)//for transact wise DayBOok
            {
                dsDayBookTransact.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

                //otherwise it populate the records again and again
                double DebitSum, CreditSum;
                DebitSum = CreditSum = 0;
                rptDayBookTransact rpt = new rptDayBookTransact();

                //Fill the logo on the report
                Misc.WriteLogo(dsDayBookTransact, "tblImage");

                rpt.SetDataSource(dsDayBookTransact);


                DisplayTransactWiseBok(true);




                #region TEST for datagrid binding
                DataView mView;
                mView = dsDayBookTransact.Tables[0].DefaultView;


                dgDayBook.FixedRows = 1;
                dgDayBook.FixedColumns = 1;

                //Header row
                dgDayBook.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dgDayBook));

                DevAge.ComponentModel.IBoundList bindList = new DevAge.ComponentModel.BoundDataView(mView);

                //Create default columns
                CreateColumns(dgDayBook.Columns, bindList);

                dgDayBook.DataSource = bindList;

                #endregion




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
                CrystalDecisions.Shared.ParameterDiscreteValue pdvDebit_Total = new CrystalDecisions.Shared.ParameterDiscreteValue();
                CrystalDecisions.Shared.ParameterDiscreteValue pdvCredit_Total = new CrystalDecisions.Shared.ParameterDiscreteValue();
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


                pdvReport_Head.Value = "Day Book";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);


                pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);


                // pdvReport_Date.Value = "As On Date:" + m_DayBook.ToDate.ToString("yyyy/MM/dd");
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem(m_DayBook.ToDate);
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);


                pdvDebit_Total.Value = m_DebitTotal;
                pvCollection.Clear();
                pvCollection.Add(pdvDebit_Total);
                rpt.DataDefinition.ParameterFields["Debit_Total"].ApplyCurrentValues(pvCollection);

                pdvCredit_Total.Value = m_CreditTotal;
                pvCollection.Clear();
                pvCollection.Add(pdvCredit_Total);
                rpt.DataDefinition.ParameterFields["Credit_Total"].ApplyCurrentValues(pvCollection);
                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                //DisplayTransactWiseDayBook(true);
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
                /// frm.WindowState = FormWindowState.Maximized;

            }
            else if (m_DayBook.CashBalanceWise)
            {
                dsDayBookCash.Clear();//It clear the previous records of dataset on crystal report...when this button is pressed

                //otherwise it populate the records again and again
                double DebitSum, CreditSum;
                DebitSum = CreditSum = 0;
                rptDayBookCash rpt = new rptDayBookCash();

                //Fill the logo on the report
                Misc.WriteLogo(dsDayBookCash, "tblImage");

                rpt.SetDataSource(dsDayBookCash);
                dsDayBookCash.Tables["tblGroup"].Rows.Add("Debit", 1);
                dsDayBookCash.Tables["tblGroup"].Rows.Add("Credit", 2);
                DisplayCashWiseDayBook(true);
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
                CrystalDecisions.Shared.ParameterDiscreteValue pdvAmount_Total = new CrystalDecisions.Shared.ParameterDiscreteValue();
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

                pdvReport_Head.Value = "Day Book";
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Head);
                rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);


                pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
                pvCollection.Clear();
                pvCollection.Add(pdvFiscal_Year);
                rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

                pdvAmount_Total.Value = m_Total;
                pvCollection.Clear();
                pvCollection.Add(pdvAmount_Total);
                rpt.DataDefinition.ParameterFields["Amount_Total"].ApplyCurrentValues(pvCollection);


                // pdvReport_Date.Value = "As On Date:" + m_DayBook.ToDate.ToString("yyyy/MM/dd");
                pdvReport_Date.Value = "As On Date:" + Date.ToSystem(m_DayBook.ToDate);
                pvCollection.Clear();
                pvCollection.Add(pdvReport_Date);
                rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                //Update the progressbar
                ProgressForm.UpdateProgress(80, "Calculating Parameters...");

                //dsDayBookCash.Tables["tblGroup"].Rows.Add("Debit", 1);
                //dsDayBookCash.Tables["tblGroup"].Rows.Add("Credit", 2);
                // DisplayCashWiseDayBook(true);
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
                    default:
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }
                frm.WindowState = FormWindowState.Maximized;
            }

        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // btnPrintPreview_Click(sender, e);
            //Clear all rows
            grdDayBook.Redim(0, 0);

            this.Cursor = Cursors.WaitCursor;
            //Load all over again;
            frmDayBook_Load(sender, e);

            this.Cursor = Cursors.Default;
            //Show complete notification
            //Global.Msg("Reload Complete!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
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
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void grdDayBook_DoubleClick_1(object sender, EventArgs e)
        {
            int CurRow =grdDayBook. Selection.GetSelectionRegion().GetRowsIndex()[0];
            string VoucherType = "";
             if (m_DayBook.CashBalanceWise == true)//For CashBalancewise daybook
            {
                //find colulmn position at first
                int CurColumn = grdDayBook.Selection.GetSelectionRegion().GetColumnsIndex()[0];
                if (CurColumn <= 5)//This is the portion of Debit side
                {
                    SourceGrid.CellContext cellType1 = new SourceGrid.CellContext(grdDayBook, new SourceGrid.Position(CurRow, 5));
                    if (cellType1.Value != "")
                    {
                        RowID = Convert.ToInt32(cellType1.Value);
                        SourceGrid.CellContext cellVouTypeTran = new SourceGrid.CellContext(grdDayBook, new SourceGrid.Position(CurRow, 1));
                        VoucherType = cellVouTypeTran.Value.ToString();
                    }
                }
                else if (CurColumn >= 6)//This is the portion of Credit Side
                {
                    SourceGrid.CellContext cellType2 = new SourceGrid.CellContext(grdDayBook, new SourceGrid.Position(CurRow, 11));
                    if (cellType2.Value != "")
                    {
                        RowID = Convert.ToInt32(cellType2.Value);
                        SourceGrid.CellContext cellVouTypeTran = new SourceGrid.CellContext(grdDayBook, new SourceGrid.Position(CurRow, 7));
                        VoucherType = cellVouTypeTran.Value.ToString();
                    }
                }

            }

            switch (VoucherType)
            {
                case "JRNL":
                    frmJournal frm = new frmJournal(RowID);
                    frm.Show();
                    break;
                case "DR_NOT":
                    frmDebitNote frm1 = new frmDebitNote(RowID);
                    frm1.Show();
                    break;
                case "CR_NOT":
                    frmCreditNote frm2 = new frmCreditNote(RowID);
                    frm2.Show();
                    break;
                case "CASH_PMNT":
                    frmCashPayment frm3 = new frmCashPayment(RowID);
                    frm3.Show();
                    break;
                case "CASH_RCPT":
                    frmCashReceipt frm4 = new frmCashReceipt(RowID);
                    frm4.Show();

                    break;
                case "BANK_PMNT":
                    frmBankPayment frm5 = new frmBankPayment(RowID);
                    frm5.Show();
                    break;
                case "BANK_RCPT":
                    frmBankReceipt frm6 = new frmBankReceipt(RowID);
                    frm6.Show();
                    break;
                case "CNTR":
                    frmContra frm7 = new frmContra(RowID);
                    frm7.Show();
                    break;
                case "SALES":
                    frmSalesInvoice frm8 = new frmSalesInvoice(RowID);
                    frm8.Show();
                    break;
                case "SLS_RTN":
                    frmSalesReturn frm9 = new frmSalesReturn(RowID);
                    frm9.Show();
                    break;
                case "PURCH":
                    frmPurchaseInvoice frm10 = new frmPurchaseInvoice(RowID);
                    frm10.Show();
                    break;
                case "PURCH_RTN":
                    frmPurchaseReturn frm11 = new frmPurchaseReturn(RowID);
                    frm11.Show();
                    break;
            }
        }

        private void dgDayBook_DoubleClick(object sender, EventArgs e)
        {
            //grdDayBook.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            int CurRow = dgDayBook. Selection.GetSelectionRegion().GetRowsIndex()[0];
            string VoucherType = "";
            if (m_DayBook.TransactionWise == true)//for Transactionwise daybook           
            {
                SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, 7));
                RowID = Convert.ToInt32(cellType.Value);
                SourceGrid.CellContext cellVouTypeTran = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, (int)GridColumn.VoucherType));
                VoucherType = cellVouTypeTran.Value.ToString();
            }

            switch (VoucherType)
            {
                case "JRNL":
                    frmJournal frm = new frmJournal(RowID);
                    frm.Show();
                    break;
                case "DR_NOT":
                    frmDebitNote frm1 = new frmDebitNote(RowID);
                    frm1.Show();
                    break;
                case "CR_NOT":
                    frmCreditNote frm2 = new frmCreditNote(RowID);
                    frm2.Show();
                    break;
                case "CASH_PMNT":
                    frmCashPayment frm3 = new frmCashPayment(RowID);
                    frm3.Show();
                    break;
                case "CASH_RCPT":
                    frmCashReceipt frm4 = new frmCashReceipt(RowID);
                    frm4.Show();

                    break;
                case "BANK_PMNT":
                    frmBankPayment frm5 = new frmBankPayment(RowID);
                    frm5.Show();
                    break;
                case "BANK_RCPT":
                    frmBankReceipt frm6 = new frmBankReceipt(RowID);
                    frm6.Show();
                    break;
                case "CNTR":
                    frmContra frm7 = new frmContra(RowID);
                    frm7.Show();
                    break;
                case "SALES":
                    frmSalesInvoice frm8 = new frmSalesInvoice(RowID);
                    frm8.Show();
                    break;
                case "SLS_RTN":
                    frmSalesReturn frm9 = new frmSalesReturn(RowID);
                    frm9.Show();
                    break;
                case "PURCH":
                    frmPurchaseInvoice frm10 = new frmPurchaseInvoice(RowID);
                    frm10.Show();
                    break;
                case "PURCH_RTN":
                    frmPurchaseReturn frm11 = new frmPurchaseReturn(RowID);
                    frm11.Show();
                    break;
            }

        }

        private void dgDayBook_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                dgDayBook_DoubleClick(sender, null);
        }

    }
}