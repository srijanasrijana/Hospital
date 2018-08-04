using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using BusinessLogic;
using System.Collections;
using Inventory.DataSet;
using System.Threading;
using DateManager;
using Common;
using SourceGrid.Selection;
using Accounts.Reports;

namespace Accounts
{
    public partial class frmDayBook : Form
    {
        private int RowID;
        private DayBookSettings m_DayBook;
        private int AddressBookTransactRowsCount;
        private DataTable tblBufferDayBook; //For temporary storing daybook for crystal report as well as grid 

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
        Purchase m_Purchase = new Purchase();
        PurchaseReturn m_PurchaseRtn = new PurchaseReturn();
        string VoucherType = "";
        private Accounts.Model.dsDayBookTransasct dsDayBookTransact = new Model.dsDayBookTransasct();
        private Accounts.Model.dsDayBook dsDayBookCash = new Model.dsDayBook();
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
        private IMDIMainForm m_MDIForm;
        private DataRow[] drFound;
        private DataRow[] drFound1;
        private DataTable dTable;

        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
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
            LedgerDate, VoucherNo, VoucherType, AccountName, DebitAmt, CreditAmt, RowID, Ledger_ID
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
        public frmDayBook(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;

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

        public frmDayBook(DayBookSettings DayBook, IMDIMainForm frmMDI)//Constructor having class object as argument
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



            gridColumn = dgDayBook.Columns.Add("TransactDate", "Transact Date", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 100;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgDayBook.Columns.Add("VoucherType", "Voucher Type", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 130;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgDayBook.Columns.Add("VoucherNumber", "Voucher No", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 140;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            gridColumn = dgDayBook.Columns.Add("AccountName", "Account Name", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 200;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            gridColumn = dgDayBook.Columns.Add("Debit_Amount", "Debit Amount", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 150;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;

            gridColumn = dgDayBook.Columns.Add("Credit_Amount", "Credit Amount", new SourceGrid.Cells.DataGrid.Cell());
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 150;
            gridColumn.AutoSizeMode = SourceGrid.AutoSizeMode.None;


            //Hidden columns for storing required information, sp. while doubleclicking
            gridColumn = dgDayBook.Columns.Add("RowID", "Credit Amount", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.Visible = false;

            gridColumn = dgDayBook.Columns.Add("LedgerID", "Credit Amount", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.Visible = false;

            //grdDayBook.Columns.StretchToFit();

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
            grdDayBook[0, (int)GridColumn.AccountName].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdDayBook[0, (int)GridColumn.DebitAmt] = new MyHeader("Debit Amount");
            grdDayBook[0, (int)GridColumn.DebitAmt].Column.Width = 100;

            grdDayBook[0, (int)GridColumn.CreditAmt] = new MyHeader("Credit Amount");
            grdDayBook[0, (int)GridColumn.CreditAmt].Column.Width = 100;

            grdDayBook[0, (int)GridColumn.RowID] = new MyHeader("RowID");
            grdDayBook[0, (int)GridColumn.RowID].Column.Width = 20;
            grdDayBook[0, (int)GridColumn.RowID].Column.Visible = false;

            grdDayBook.AutoStretchColumnsToFitWidth = true;

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
                grdDayBook.SelectionMode = SourceGrid.GridSelectionMode.Cell;
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

        double totalDrAmt, totalCrAmt;

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


            totalDrAmt = totalCrAmt = 0;
            try
            {
                if (!IsCrystlalReport)
                    OrientationForCashwise();//FOR HEADER AND OREINTATION
                Transaction m_Transaction = new Transaction();
                ProgressForm.UpdateProgress(40, "Calculating ledger balance...");
                //BLOCK FOR DEBIT CASH BALANCE WISE DAY BOOK
                DataTable dtTransact;

                #region BLOCK FOR DEBIT CASH BALANCE WISE DAY BOOK


                string VoucherNo = "";
                if (m_DayBook.HasDateRange == true)
                {
                    dtTransact = m_Transaction.GetDebitTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID, ProjectIDs);
                }
                else
                {
                    dtTransact = m_Transaction.GetDebitTransactionInfo(null, null, m_DayBook.AccClassID, ProjectIDs);
                }

                foreach (DataRow drTransact in dtTransact.Rows)
                {
                    VoucherNo = "";

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

                lblTotalDebitAmount.Text = totalDrAmt.ToString();



                #endregion

                #region BLOCK FOR CREDIT CASH BALANCE WISE DAY BOOK


                if (m_DayBook.HasDateRange == true)
                {

                    // DataTable dtTransact = m_Transaction.GetCreditTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate, m_DayBook.AccClassID);
                    dtTransact = m_Transaction.GetCreditTransactionInfo(m_DayBook.FromDate, m_DayBook.ToDate,
                        m_DayBook.AccClassID, ProjectIDs);
                }
                else
                {
                    dtTransact = m_Transaction.GetCreditTransactionInfo(null, null, m_DayBook.AccClassID,
                        ProjectIDs);
                }
                foreach (DataRow drTransact in dtTransact.Rows)
                {

                    GetVoucherMasterInfo(drTransact["RowID"].ToString(), drTransact["VoucherType"].ToString());
                    string VoucherNoCrCash = "";
                    foreach (DataRow drVoucherMasterInfo in dt.Rows)
                    {
                        VoucherNoCrCash = drVoucherMasterInfo["Voucher_No"].ToString();

                    }
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransact["LedgerID"]),
                        LangMgr.DefaultLanguage);
                    foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
                    {
                        DateTime dtTransactDate = Convert.ToDateTime(drTransact["TransactDate"]);
                        totalCrAmt += Convert.ToDouble(drTransact["Credit_Amount"]);
                        string cramount =
                            Convert.ToDecimal(drTransact["Credit_Amount"])
                                .ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        WriteDayBookCrCash(Date.ToSystem(dtTransactDate), drTransact["VoucherType"].ToString(),
                            VoucherNoCrCash, drLedgerInfo["LedName"].ToString(), cramount, "LEDGER",
                            drTransact["RowID"].ToString(), IsCrystlalReport);

                    }


                }


                lblTotalCreditAmount.Text = totalCrAmt.ToString();
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
            dblClick.DoubleClick += new EventHandler(dgDayBook_DoubleClick);

            gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
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


            //Temporary Table for buffer DayBook
            tblBufferDayBook = new DataTable();
            tblBufferDayBook.Columns.Add("TransactDate", typeof(string));
            tblBufferDayBook.Columns.Add("VoucherType", typeof(string));
            tblBufferDayBook.Columns.Add("VoucherNumber", typeof(string));
            tblBufferDayBook.Columns.Add("AccountName", typeof(string));
            tblBufferDayBook.Columns.Add("Debit_Amount", typeof(string));
            tblBufferDayBook.Columns.Add("Credit_Amount", typeof(string));
            tblBufferDayBook.Columns.Add("RowID", typeof(int));
            tblBufferDayBook.Columns.Add("LedgerID", typeof(int));



            if (m_DayBook.CashBalanceWise == true)//BLOCK FOR CASH BALANCE WISE DAY BOOK
            {
                grdDayBook.Visible = true;
                dgDayBook.Visible = false;

                DisplayCashWiseDayBook(false);
                grdDayBook.Rows[grdDayBook.RowsCount - 1].Visible = false; // hide the last row of the grid which contains debit and credit amount
                // as debit and credit amount are shown in the labels
                frmDayBook_Resize(null, null);
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
                            TransactDate = DateManager.NepDateConverter.EngToNep(Convert.ToDateTime(TransactDate)).ToString();

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
                            TransactDate = DateManager.NepDateConverter.EngToNep(Convert.ToDateTime(TransactDate)).ToString();

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
                            TransactDate = DateManager.NepDateConverter.EngToNep(Convert.ToDateTime(TransactDate)).ToString();

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
                            TransactDate = DateManager.NepDateConverter.EngToNep(Convert.ToDateTime(TransactDate)).ToString();

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

            string AccClassIDsXMLString = AccountingClass.GetXMLAccClass(m_DayBook.AccClassID);

            ArrayList arrProjectID = new ArrayList();
            arrProjectID.Add(m_DayBook.ProjectID);
            string ProjectIDsXMLString = Project.GetXMLProject(arrProjectID);



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





                //LINQ query to select all data from dataTable
                var query = from o in dtTransactionDetails.AsEnumerable()
                            select new
                            {

                                TransactDate = o.Field<string>("LedgerNepDate"),
                                VoucherType = o.Field<string>("VoucherType"),
                                VoucherNumber = o.Field<string>("VoucherNumber"),
                                AccountName = o.Field<string>("Account"),
                                Debit_Amount = Convert.ToDecimal(o.Field<decimal>("Debit")).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),
                                Credit_Amount = Convert.ToDecimal(o.Field<decimal>("Credit")).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),
                                RowID = o.Field<int>("RowID"),
                                LedgerID = o.Field<int>("LedgerID")

                            };





                Decimal sumDr = 0;
                Decimal sumCr = 0;

                //Add all rows to the DataTable
                foreach (var dr in query)
                {
                    sumDr += Convert.ToDecimal(dr.Debit_Amount);
                    sumCr += Convert.ToDecimal(dr.Credit_Amount);
                    tblBufferDayBook.Rows.Add(dr.TransactDate, dr.VoucherType, dr.VoucherNumber, dr.AccountName, dr.Debit_Amount, dr.Credit_Amount, dr.RowID, dr.LedgerID);
                }




                //Show the total in desired Decimal Places
                lblTotalDebitAmount.Text = m_DebitTotal = sumDr.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                lblTotalCreditAmount.Text = m_CreditTotal = sumCr.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));


                // Create a table from the query.
                //DataTable boundTable = query.CopyToDataTable<DataRow>();

                #region datagrid binding
                DataView mView;
                mView = tblBufferDayBook.DefaultView;
                dgDayBook.Columns.Clear(); // first clear all columns to reload the data in dgDayBook
                dgDayBook.FixedRows = 1; //Allocated for Header

                //Fixed Column at first
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
                    case "PURCH":
                        dt = m_Purchase.GetPurchaseInvoiceMasterInfo(RowID);
                        break;
                    case "PURCH_RTN":
                        dt = m_PurchaseRtn.GetPurchaseReturnMasterInfo(RowID);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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


                rptDayBookTransact rpt = new rptDayBookTransact();

                //Fill the logo on the report
                Misc.WriteLogo(dsDayBookTransact, "tblImage");

                rpt.SetDataSource(dsDayBookTransact);
                //DisplayTransactWiseBok(true);



                try
                {
                    dsDayBookTransact.Tables.Remove("tblDayBookTransact");


                }
                catch
                {

                }
                System.Data.DataView view = new System.Data.DataView(tblBufferDayBook);


                DataTable selected = view.ToTable("tblDayBookTransact", false, "TransactDate", "VoucherType", "VoucherNumber", "AccountName", "Debit_Amount", "Credit_Amount", "RowID", "LedgerID");


                dsDayBookTransact.Tables.Add(selected);


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

        private void dgDayBook_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                dgDayBook_DoubleClick(sender, null);
        }

        private void dgDayBook_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string VoucherType = "";

                if (m_DayBook.TransactionWise == true)//for Transactionwise daybook           
                {
                    //grdDayBook.SelectionMode = SourceGrid.GridSelectionMode.Cell;
                    int CurRow = dgDayBook.Selection.GetSelectionRegion().GetRowsIndex()[0];


                    SourceGrid.CellContext cellType = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, 7));
                    RowID = Convert.ToInt32(cellType.Value);
                    SourceGrid.CellContext cellVouTypeTran = new SourceGrid.CellContext(dgDayBook, new SourceGrid.Position(CurRow, (int)GridColumn.VoucherType));
                    VoucherType = cellVouTypeTran.Value.ToString();
                }

                else if (m_DayBook.TransactionWise == false) // for CashWise DayBook
                {
                    int st_col = 0, end_col = 0, vouch_col = 0, rowID_Col = 0;
                    int CurRow = grdDayBook.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    int CurCol = grdDayBook.Selection.GetSelectionRegion().GetColumnsIndex()[0];
                    if (CurCol > 5) // if selected cell is in the credit side, select all columns in credit side
                    {
                        st_col = 6;
                        end_col = grdDayBook.Columns.Count - 1;
                        vouch_col = 7;
                        rowID_Col = 11;
                    }
                    else   // if selected cell is in the debit side, select all columns in debit side
                    {
                        end_col = 5;
                        vouch_col = 1;
                        rowID_Col = 5;
                    }
                    grdDayBook.Selection.SelectRange(new SourceGrid.Range(CurRow, st_col, CurRow, end_col), true);
                    SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdDayBook, new SourceGrid.Position(CurRow, rowID_Col));
                    RowID = Convert.ToInt32(cellType.Value);

                    SourceGrid.CellContext cellVouTypeTran = new SourceGrid.CellContext(grdDayBook, new SourceGrid.Position(CurRow, vouch_col));
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
            catch (Exception)
            {
                Global.MsgError("Invalid record selected !");
            }
        }

        private void frmDayBook_Resize(object sender, EventArgs e)
        {
            grdDayBook.Width = this.Width - 40;
            dgDayBook.Width = this.Width - 40;
        }

        private void dgDayBook_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}