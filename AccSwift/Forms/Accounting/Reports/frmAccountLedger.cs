using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace Inventory
{
    public partial class frmAccountLedger : Form
    {
        private AccountLedgerSettings m_AccountLedger;//Declaring the object of class 
        private int AccountLedgerRowsCount;
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//
        private SourceGrid.Cells.Views.Cell GroupView;
        private SourceGrid.Cells.Views.Cell LedgerView;
        public frmAccountLedger(AccountLedgerSettings AccountLedger)
        {
            try
            {
                m_AccountLedger = new AccountLedgerSettings();//Initializing the object of class AccountLedgerSettings
                m_AccountLedger.HasDateRange = AccountLedger.HasDateRange;
                m_AccountLedger.FromDate = AccountLedger.FromDate;
                m_AccountLedger.ToDate = AccountLedger.ToDate;
                m_AccountLedger.LedgerID = AccountLedger.LedgerID;//Store the LedgerID of selected Ledger in combobox 
                m_AccountLedger.AccountGroupID = AccountLedger.AccountGroupID;//Store the GroupID of selected GroupAccount in combobox
                m_AccountLedger.ChooseLedger = AccountLedger.ChooseLedger;//Store the bool type of variable,i.e. When ChooseLedger checkbox is checked then it stores the true value otherwise false bydefault
                m_AccountLedger.ChooseAccountGrp = AccountLedger.ChooseAccountGrp;//Store the bool type of variable,i.e. When ChooseAccountGrp checkbox is checked then it stores the true value otherwise false bydefault
                InitializeComponent();
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

        private void WriteAccountLedger(int SNo, string TransactDate, string VoucherType, string VoucherNo, string AccountName, string DrAmt, string CrAmt, string Balance, int LedgerID)
        {
            try
            {
                grdAccountLedger.Rows.Insert(grdAccountLedger.RowsCount);
                string strSNo = (SNo == 0 ? "***" : SNo.ToString());

                grdAccountLedger[AccountLedgerRowsCount, 0] = new SourceGrid.Cells.Cell(strSNo);
                grdAccountLedger[AccountLedgerRowsCount, 0].AddController(dblClick);

                grdAccountLedger[AccountLedgerRowsCount, 1] = new SourceGrid.Cells.Cell(TransactDate);
                grdAccountLedger[AccountLedgerRowsCount, 1].AddController(dblClick);

                grdAccountLedger[AccountLedgerRowsCount, 2] = new SourceGrid.Cells.Cell(VoucherType);
                grdAccountLedger[AccountLedgerRowsCount, 2].AddController(dblClick);

                grdAccountLedger[AccountLedgerRowsCount, 3] = new SourceGrid.Cells.Cell(VoucherNo);
                grdAccountLedger[AccountLedgerRowsCount, 3].AddController(dblClick);

                grdAccountLedger[AccountLedgerRowsCount, 4] = new SourceGrid.Cells.Cell(AccountName);
                grdAccountLedger[AccountLedgerRowsCount, 4].AddController(dblClick);

                grdAccountLedger[AccountLedgerRowsCount, 5] = new SourceGrid.Cells.Cell(DrAmt);
                grdAccountLedger[AccountLedgerRowsCount, 5].AddController(dblClick);

                grdAccountLedger[AccountLedgerRowsCount, 6] = new SourceGrid.Cells.Cell(CrAmt);
                grdAccountLedger[AccountLedgerRowsCount, 6].AddController(dblClick);

                grdAccountLedger[AccountLedgerRowsCount, 7] = new SourceGrid.Cells.Cell(Balance);
                grdAccountLedger[AccountLedgerRowsCount, 7].AddController(dblClick);

                grdAccountLedger[AccountLedgerRowsCount, 8] = new SourceGrid.Cells.Cell(LedgerID);
                grdAccountLedger[AccountLedgerRowsCount, 8].AddController(dblClick);
                AccountLedgerRowsCount++;
            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }

        /// <summary>
        /// Read all journal Entry
        /// </summary>
        /// <returns></returns>
      
        private void frmAccountLedger_Load(object sender, EventArgs e)
        {
            AccountLedgerRowsCount = 1;

            #region FOR OREINTATION PURPOSE
            AccountGroup m_AccountGrp = new AccountGroup();
            Transaction m_Transaction = new Transaction();

            //Disable multiple selection
            grdAccountLedger.Selection.EnableMultiSelection = false;

            //Disable multiple selection
            grdAccountLedger.Selection.EnableMultiSelection = false;

            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grdAccountLedger_DoubleClick);
            grdAccountLedger.Redim(1, 9);

            //Text format for Ledgers
            SourceGrid.Cells.Views.Cell LedgerView = new SourceGrid.Cells.Views.Cell();
            LedgerView.ForeColor = Color.Blue;
            LedgerView.Font = new Font(Font, FontStyle.Bold);
            #endregion

            //BLOCK FOR "CHOOSE LEDGER" 
            if (m_AccountLedger.ChooseLedger == true)//When this is true then it shows the information of AccountLedger according to Ledger
            {
                //BLOCK FOR ALL LEDGER 
                if (m_AccountLedger.LedgerID == 0)//When selecting  "All" option in combobox,it passes  0 value in m_AccountLedger.Ledger
                {
                    AllAccountLedger();//
                }
                //BLOCK FOR PARTICULAR LEDGER
                else
                {
                    grdAccountLedger.Redim(1, 9);
                    grdAccountLedger.FixedRows = 1;
                    WriteHeader();//Calling this method for writting header part of sourceGridView
                    try
                    {
                        //For getting the name of Account;using LedgerID for finding Ledger information
                        DataTable dtLedgerInfo = Ledger.GetLedgerInfo(m_AccountLedger.LedgerID, LangMgr.DefaultLanguage);
                        DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                        WriteAccountLedger(0, "", "", "", drLedgerInfo["LedName"].ToString(), "", "", "",m_AccountLedger.LedgerID);
                        //Get information of corresponding LedgerID  from Acc.tblTransaction 
                        DataTable dtAccountLedger = m_Transaction.GetAccountLedgerTransact(m_AccountLedger.LedgerID.ToString());//Calling this function for obtaining the datatable which provides the information of Ledger transaction according to LedgerID
                        for (int i1 = 1; i1 < dtAccountLedger.Rows.Count; i1++)
                        {
                            DataRow drAccountLedger = dtAccountLedger.Rows[i1 - 1];
                            WriteAccountLedger(0, drAccountLedger["Date"].ToString(), drAccountLedger["Type"].ToString(), drAccountLedger["VoucherNo"].ToString(), drAccountLedger["Account_Name"].ToString(), drAccountLedger["Debit_Amount"].ToString(), drAccountLedger["Credit_Amount"].ToString(), drAccountLedger["Balance"].ToString(),Convert.ToInt32(drAccountLedger["LedgerID"]));
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else if (m_AccountLedger.ChooseAccountGrp == true)
            {

                if (m_AccountLedger.AccountGroupID == 0)
                {
                    AllAccountLedger();//Calling this function for "All" option of combobox for showing the information of all Account Group Transaction...Remember!!! Transacton is always done on Ledger               
                }
                else if (m_AccountLedger.ChooseAccountGrp == true)
                {
                    grdAccountLedger.Redim(1, 9);
                    grdAccountLedger.FixedRows = 1;
                    WriteHeader();//Calling this method for writting header part of sourceGridView
                    try
                    {
                        DataTable dtLedgerID = m_Transaction.GetDistinctLedgerID(m_AccountLedger.AccountGroupID);
                        for (int i = 1; i < dtLedgerID.Rows.Count; i++)
                        {
                            DataRow drLedgerID = dtLedgerID.Rows[i];
                            //grdAccountLedger.Rows.Insert(rows);
                            DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drLedgerID["LedgerID"]), LangMgr.DefaultLanguage);
                            DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                            //Get information of corresponding LedgerID  from Acc.tblTransaction 

                            WriteAccountLedger(0, "", "", "", drLedgerInfo["LedName"].ToString(), "", "", "",m_AccountLedger.LedgerID);
                            DataTable dtAccountLedger = m_Transaction.GetAccountLedgerTransact(drLedgerID["LedgerID"].ToString());//Calling this function for obtaining the datatable which provides the information of Ledger transaction according to LedgerID
                            for (int i1 = 1; i1 < dtAccountLedger.Rows.Count; i1++)
                            {
                                DataRow drAccountLedger = dtAccountLedger.Rows[i1 - 1];
                                WriteAccountLedger(0, drAccountLedger["Date"].ToString(), drAccountLedger["Type"].ToString(), drAccountLedger["VoucherNo"].ToString(), drAccountLedger["Account_Name"].ToString(), drAccountLedger["Debit_Amount"].ToString(), drAccountLedger["Credit_Amount"].ToString(), drAccountLedger["Balance"].ToString(),m_AccountLedger.LedgerID);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }


        public void AllAccountLedger()
        {
            //BLOCK FOR ALL LEDGER 
            AccountGroup m_AccountGrp = new AccountGroup();
            //Dynamic memory allocation of an  object
            Transaction m_Transaction = new Transaction();
            //Disable multiple selection
            grdAccountLedger.Selection.EnableMultiSelection = false;
            grdAccountLedger.Redim(1, 9);
            grdAccountLedger.FixedRows = 1;
            WriteHeader();//Calling this method for writting header part of sourceGridView
            //Text format for Ledgers
            SourceGrid.Cells.Views.Cell LedgerView = new SourceGrid.Cells.Views.Cell();
            LedgerView.ForeColor = Color.Blue;
            LedgerView.Font = new Font(Font, FontStyle.Bold);
            // Try Catch block for error handling 
            try
            {
                //Collecting Distinct LedgerID on datatable from Acc.tblTransaction
                DataTable dt = m_AccountGrp.GetDistinctLedgerID();
               
                foreach (DataRow dr in dt.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(dr["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    WriteAccountLedger(0, "", "", "", drLedgerInfo["LedName"].ToString(), "", "", "",Convert.ToInt32(dr["LedgerID"]));
                    //Get information of corresponding LedgerID  from Acc.tblTransaction 
                    DataTable dtAccountLedger = m_Transaction.GetAccountLedgerTransact(dr["LedgerID"].ToString());////Calling this function for obtaining the datatable which provides the information of Ledger transaction according to LedgerID
                    int sNo = 1;
                    foreach (DataRow drAccountLedger in dtAccountLedger.Rows)
                    {
                        WriteAccountLedger(sNo, drAccountLedger["Date"].ToString(), drAccountLedger["Type"].ToString(), drAccountLedger["VoucherNo"].ToString(), drAccountLedger["Account_Name"].ToString(), drAccountLedger["Debit_Amount"].ToString(), drAccountLedger["Credit_Amount"].ToString(), drAccountLedger["Balance"].ToString(),Convert.ToInt32(drAccountLedger["LedgerID"]));
                        sNo++;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void WriteHeader()
        {
            grdAccountLedger.Rows.Insert(0);
            //Define Header Part
            grdAccountLedger[0, 0] = new MyHeader("S.No.");
            grdAccountLedger[0, 1] = new MyHeader("Date");
            grdAccountLedger[0, 2] = new MyHeader("Type");
            grdAccountLedger[0, 3] = new MyHeader("Voucher No");
            grdAccountLedger[0, 4] = new MyHeader("Account Name");
            grdAccountLedger[0, 5] = new MyHeader("Debit Amount");
            grdAccountLedger[0, 6] = new MyHeader("Credit Amount");
            grdAccountLedger[0, 7] = new MyHeader("Balance");
            grdAccountLedger[0, 8] = new MyHeader("LedgerID");

            //Define width of column size
            grdAccountLedger[0, 0].Column.Width = 50;
            grdAccountLedger[0, 1].Column.Width = 150;
            grdAccountLedger[0, 2].Column.Width = 100;
            grdAccountLedger[0, 3].Column.Width = 150;
            grdAccountLedger[0, 4].Column.Width = 200;
            grdAccountLedger[0, 5].Column.Width = 150;
            grdAccountLedger[0, 6].Column.Width = 150;
            grdAccountLedger[0, 7].Column.Width = 150;
            grdAccountLedger[0, 8].Column.Width = 150;
        }


        private void grdAccountLedger_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //Get the Selected Row           
                int CurRow = grdAccountLedger.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext CellID = new SourceGrid.CellContext(grdAccountLedger, new SourceGrid.Position(CurRow, 8));
                TransactSettings m_TS = new TransactSettings();
                m_TS.HasDateRange = false;
                m_TS.ShowZeroBalance = false;
                m_TS.FromDate = m_AccountLedger.FromDate;
                m_TS.ToDate = m_AccountLedger.ToDate;
                m_TS.LedgerID = Convert.ToInt32(CellID.Value);
                frmTransaction m_Transact = new frmTransaction(m_TS);
                m_Transact.ShowDialog();

            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        private void frmAccountLedger_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}