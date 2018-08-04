using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using ErrorManager;
using System.IO;
using Inventory.CrystalReports;
using Inventory.DataSet;
using System.Data.SqlClient;
using Common;


namespace Inventory
{
    public partial class frmBankReconciliation : Form, IfrmAddAccClass, ILOVLedger, IfrmDateConverter, IfrmSelectAccClassID
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield; 
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        public string changedate;
       // bool hasChanged = false;
        private int OnlyReqdDetailRows = 0;
        private bool IsFromDate = false;
        private bool IsToDate = false;
        private bool IsDate = false;
        DevAge.Windows.Forms.DevAgeTextBox ctx;

        private bool IsChequeDateButton = false;
        SourceGrid.CellContext ctx1 = new SourceGrid.CellContext();

        double m_DebitAmount = 0;//Holds the total debit amount on the Voucher
        double m_CreditAmount = 0;
        ArrayList AccClassID = new ArrayList();
        //ArrayList AccountClassID = new ArrayList();
        List<int> AccClassIDD = new List<int>();
        private int loopCounter = 0;
        int m_BankReconciliationID = 0;
        private bool IsFieldChanged = false;
        private SourceGrid.Cells.Views.Cell CheckedView;
        bool hasChanged = false;
        private int CurrAccLedgerID = 0;
        private string CurrBal = "";
        private int CurrRowPos = 0;
        ListItem SeriesID = new ListItem();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();

        Transaction m_Transaction = new Transaction();

        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtEdit = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtchkMatch = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
       // SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtcboDrCrSelectedIndexChanged = new SourceGrid.Cells.Controllers.CustomEvents();
       // SourceGrid.Cells.Controllers.CustomEvents evtcboDrCrSelectedIndexChanged = new SourceGrid.Cells.Controllers.CustomEvents();   
        //Transaction m_Transaction = new Transaction();
        BankReconciliation m_BankReconciliation = new BankReconciliation();

        public frmBankReconciliation()
        {
            InitializeComponent();
        }

        public void AddAccClass()
        {
            try
            {
                //Clear the checked nodes of Treeview and relaoding the tree view
                treeAccClass.Nodes.Clear();
                ShowAccClassInTreeView(treeAccClass, null, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BANK_RECONCILATION_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            try
            {
                AccountLedgerSettings m_AccountLedger = new AccountLedgerSettings();
                ListItem liLedgerID = new ListItem();
                ListItem liGroupID = new ListItem();
                liLedgerID = (ListItem)cmboBanks.SelectedItem;
                int LedgerID = liLedgerID.ID;
                // liGroupID = (ListItem)cmbChooseAccountGroup.SelectedItem;
                m_AccountLedger.LedgerID = liLedgerID.ID;//Pass the ledgerID of selected item of combobox
                m_AccountLedger.AccountGroupID = liGroupID.ID;
                // m_AccountLedger.ChooseLedger = cmboBanks.Checked;
                // m_AccountLedger.ChooseAccountGrp = rbtnChooseAccountGrp.Checked;
                //m_AccountLedger.HasDateRange = chkDate.Checked;
                m_AccountLedger.FromDate = Date.ToDotNet(txtFromDate.Text);// Converting  datetime came via controls into DonNet datetime formate 
                m_AccountLedger.ToDate = Date.ToDotNet(txtToDate.Text);
                //Go to the transaction form directly when particular ledgerID is selected

                TransactSettings m_transactSetting = new TransactSettings();
                //m_transactSetting.HasDateRange = chkDate.Checked;
                m_transactSetting.FromDate = Date.ToDotNet(txtFromDate.Text);
                m_transactSetting.ToDate = Date.ToDotNet(txtToDate.Text);
                m_transactSetting.LedgerID = liLedgerID.ID;
                m_transactSetting.AccountGroupID = liGroupID.ID;
                m_transactSetting.AccClassID = AccClassID;
               
                //if (cmbChooseLedger.SelectedIndex != 0)
                //{
                //DataTable dt = Transaction.GetLedgerTransaction(LedgerID, AccClassIDsXMLString, m_TS.FromDate, m_TS.ToDate);
                //DataTable dtLedgerName = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                //foreach (DataRow drLedgerName in dtLedgerName.Rows)
                //{
                //    WriteTransaction(0, drLedgerName["LedName"].ToString(), "", "", "", "", "", "", "LEDGERHEAD", "", IsCrystalReport);
                //    ShowLedgerTransaction(dt, LedgerID, IsCrystalReport);
                //}
                grdBankReconciliation.Selection.EnableMultiSelection = false;
                grdBankReconciliation.Redim(1, 12);
                int rows = grdBankReconciliation.Rows.Count;
                string AccClassIDsXMLString = ReadAllAccClassID(m_transactSetting);
                WriteHeader();
               // MessageBox.Show(AccClassIDsXMLString);
                //DataTable dt = m_Transaction.GetLedgerTransact(LedgerID, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                DataTable dt = Transaction.GetLedgerTransaction1(LedgerID, AccClassIDsXMLString, m_transactSetting.FromDate, m_transactSetting.ToDate);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                ShowLedgerTransaction(dt, LedgerID);
                //DataTable dtLedgerName = Ledger.GetLedgerInfo(LedgerID, LangMgr.DefaultLanguage);
                //foreach (DataRow drLedgerName in dtLedgerName.Rows)
                //{
                    //WriteTransaction(0, drLedgerName["LedName"].ToString(), "", "", "", "", "", "", "LEDGERHEAD", "");
                   // ShowLedgerTransaction(dt, LedgerID);
                    //ShowVoucherTransaction(dt, LedgerID);
                //}

                //DataTable dt = m_Transaction.GetLedgerTransact(LedgerID,Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                //DataTable dt = m_Transaction.GetLedgerTransaction1(LedgerID, AccClassIDsXMLString, m_transactSetting.FromDate, m_transactSetting.ToDate);//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                //frmTransaction frm = new frmTransaction(m_transactSetting);
                //frm.Show();
                    //}
                    //else//This is 
                    //{
                    //    frmTransaction frm = new frmTransaction(m_transactSetting, "ALL_LEDGERS");
                    //    frm.Show();
                    //}
               

                //if (rbtnChooseAccountGrp.Checked)
                //{
                //    if (cmbChooseAccountGroup.SelectedIndex != 0)
                //    {
                //        frmTransaction frm = new frmTransaction(m_transactSetting, "PARTICULAR_GROUP");
                //        frm.Show();
                //    }
                //    else
                //    {
                                //frmTransaction frm1 = new frmTransaction(m_transactSetting, "ALL_LEDGERS");
                                 //frm1.Show();
                //    }
                //}
                //frmAccountLedger frm1 = new frmAccountLedger(m_AccountLedger);//Passing an object of class as argmument of constructor
                //frm1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //try
            //{

            //    ListItem liLedgerID = new ListItem();
            //    liLedgerID = (ListItem)cmboBanks.SelectedItem;
            //    int LedgerID = liLedgerID.ID;                            

            //    grdBankReconciliation.Selection.EnableMultiSelection = false;              
            //    grdBankReconciliation.Redim(1, 11);                                  
            //    int rows = grdBankReconciliation.Rows.Count;//Declaring and initializing the int type of variable rows which gives the Rowcounts of sourcegridview
            //    grdBankReconciliation.Rows.Insert(rows);
            //    WriteHeader();//Calling the function for Header purpose of sourcegridview

                //DataTable dt = m_Transaction.GetLedgerTransact(LedgerID, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));//Get the Transact Inforamtion of corresponding LedgerID according to datewise
                    
            //    ShowVoucherTransaction(dt, LedgerID);
                           
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
        }


       // private void WriteTransaction(int Sno,string TransactDate,string Ledger,string DrBal, string CrBal, string ChequeNumber, string ChequeBank,string ChequeDate,string VoucherType, int RowID)
        private void WriteTransaction(int Sno, string TransactDate, string Ledger, string DrBal, string CrBal, string ChequeNumber, string ChequeBank, DateTime? ChequeDate, string VoucherType, int RowID)
        {           
        ////Get information of corresponding LedgerID  from Acc.tblTransaction 
        //  DataTable dtAccountLedger = m_Transaction.GetAccountLedgerTransact(309.ToString());//Calling this function for obtaining the datatable which provides the information of Ledger transaction according to LedgerID
        //  for (int i = 1; i <= dtAccountLedger.Rows.Count; i++)
        //  {
            //DataRow drAccountLedger = dtAccountLedger.Rows[i - 1];                     
            grdBankReconciliation.Rows.Insert(Sno);
            
            grdBankReconciliation[Sno, 0] = new SourceGrid.Cells.Cell(Sno.ToString());
            grdBankReconciliation[Sno, 1] = new SourceGrid.Cells.Cell(TransactDate);
            grdBankReconciliation[Sno, 2] = new SourceGrid.Cells.Cell(Ledger);
            //grdBankReconciliation[Sno, 3] = new SourceGrid.Cells.Cell(5000);
            if (Convert.ToDecimal(DrBal) > 0)
            {
                    //grdBankReconciliation[Sno, 3] = new SourceGrid.Cells.Cell("Dr-" + DrBal);
                grdBankReconciliation[Sno, 3] = new SourceGrid.Cells.Cell(DrBal);
                grdBankReconciliation[Sno, 4] = new SourceGrid.Cells.Cell(0);
            }
            else if (Convert.ToDecimal(CrBal) > 0)
            { 
                    grdBankReconciliation[Sno, 3] = new SourceGrid.Cells.Cell(0);
                    grdBankReconciliation[Sno, 4] = new SourceGrid.Cells.Cell(CrBal);
            }
            try
            {
                grdBankReconciliation[Sno, 5] = new SourceGrid.Cells.Cell(ChequeNumber);                          
            }
            catch
            {
                grdBankReconciliation[Sno, 5] = new SourceGrid.Cells.Cell("");
            }
            try
            {
                grdBankReconciliation[Sno, 6] = new SourceGrid.Cells.Cell(ChequeBank);                           
            }
            catch
            {
                grdBankReconciliation[Sno, 6] = new SourceGrid.Cells.Cell("");
            }
            try
            {
                //changedate = Date.DBToSystem(dr[9].ToString());
                //grdBankReconciliation[Sno, 7] = new SourceGrid.Cells.Cell(ChequeDate);
                grdBankReconciliation[Sno, 7] = new SourceGrid.Cells.Cell(Date.DBToSystem(ChequeDate.ToString()));
            }
            catch
            {
                grdBankReconciliation[Sno, 7] = new SourceGrid.Cells.Cell(" ");
            }

            SourceGrid.Cells.CheckBox chkMatch = new SourceGrid.Cells.CheckBox();
            //chkMatch.Checked = true;
            grdBankReconciliation[Sno, 8] = chkMatch;
            grdBankReconciliation[Sno, 8].AddController(evtchkMatch);                      
                        
            SourceGrid.Cells.Button btnEdit = new SourceGrid.Cells.Button("Edit");
            btnEdit.Image = global::Inventory.Properties.Resources.edit_add;
            grdBankReconciliation[Sno, 9] = btnEdit;
            grdBankReconciliation[Sno, 9].AddController(evtEdit);

            grdBankReconciliation[Sno, 10] = new SourceGrid.Cells.Cell(VoucherType);
            grdBankReconciliation[Sno, 11] = new SourceGrid.Cells.Cell(RowID);

            Sno++;                     
        }
        private void WriteTransaction1(int Sno, string TransactDate, string Ledger, string DrBal, string CrBal, string ChequeNumber, string ChequeBank, DateTime? ChequeDate, string VoucherType, int RowID,string matched)
        {
            ////Get information of corresponding LedgerID  from Acc.tblTransaction 
            //  DataTable dtAccountLedger = m_Transaction.GetAccountLedgerTransact(309.ToString());//Calling this function for obtaining the datatable which provides the information of Ledger transaction according to LedgerID
            //  for (int i = 1; i <= dtAccountLedger.Rows.Count; i++)
            //  {
            //DataRow drAccountLedger = dtAccountLedger.Rows[i - 1];                     
            grdBankReconciliation.Rows.Insert(Sno);

            grdBankReconciliation[Sno, 0] = new SourceGrid.Cells.Cell(Sno.ToString());
            grdBankReconciliation[Sno, 1] = new SourceGrid.Cells.Cell(TransactDate);
            grdBankReconciliation[Sno, 2] = new SourceGrid.Cells.Cell(Ledger);
            //grdBankReconciliation[Sno, 3] = new SourceGrid.Cells.Cell(5000);
            if (Convert.ToDecimal(DrBal) > 0)
            {
                //grdBankReconciliation[Sno, 3] = new SourceGrid.Cells.Cell("Dr-" + DrBal);
                grdBankReconciliation[Sno, 3] = new SourceGrid.Cells.Cell(DrBal);
                grdBankReconciliation[Sno, 4] = new SourceGrid.Cells.Cell(0);
            }
            else if (Convert.ToDecimal(CrBal) > 0)
            {
                grdBankReconciliation[Sno, 3] = new SourceGrid.Cells.Cell(0);
                grdBankReconciliation[Sno, 4] = new SourceGrid.Cells.Cell(CrBal);
            }
            try
            {
                grdBankReconciliation[Sno, 5] = new SourceGrid.Cells.Cell(ChequeNumber);
            }
            catch
            {
                grdBankReconciliation[Sno, 5] = new SourceGrid.Cells.Cell("");
            }
            try
            {
                grdBankReconciliation[Sno, 6] = new SourceGrid.Cells.Cell(ChequeBank);
            }
            catch
            {
                grdBankReconciliation[Sno, 6] = new SourceGrid.Cells.Cell("");
            }
            try
            {
                grdBankReconciliation[Sno, 7] = new SourceGrid.Cells.Cell(ChequeDate);
            }
            catch
            {
                grdBankReconciliation[Sno, 7] = new SourceGrid.Cells.Cell(" ");
            }

            SourceGrid.Cells.CheckBox chkMatch = new SourceGrid.Cells.CheckBox();
            if (matched == "Y")
            {
                chkMatch.Checked = true;
            }
            else
                chkMatch.Checked = false;
            grdBankReconciliation[Sno, 8] = chkMatch;
            grdBankReconciliation[Sno, 8].AddController(evtchkMatch);

            SourceGrid.Cells.Button btnEdit = new SourceGrid.Cells.Button("Edit");
            btnEdit.Image = global::Inventory.Properties.Resources.edit_add;
            grdBankReconciliation[Sno, 9] = btnEdit;
            grdBankReconciliation[Sno, 9].AddController(evtEdit);

            grdBankReconciliation[Sno, 10] = new SourceGrid.Cells.Cell(VoucherType);
            grdBankReconciliation[Sno, 11] = new SourceGrid.Cells.Cell(RowID);

            Sno++;
        }
        private void frmBankReconciliation_Load(object sender, EventArgs e)
        {
            Global.acc = "B";
            ChangeState(EntryMode.NEW);
            m_mode = EntryMode.NEW;
            //By default checked on All radiobutton on formLoad condition
            ShowAccClassInTreeView(treeAccClass, null, 0);
           

            //DataTable dtUserInfo = User.GetUserInfo(User.CurrUser); //user id must be read from  global i.e current user id
            //DataRow drUserInfo = dtUserInfo.Rows[0];
            //AccClassID.Clear();
            //AccClassID.Add(Convert.ToInt32(drUserInfo["AccClassID"]));
            AccClassID.Add("1");

            cboSeriesName.SelectedIndexChanged += new EventHandler(Text_Change);
            txtVchNo.TextChanged += new EventHandler(Text_Change);

            rbtnAll.Checked = true;
            txtDate.Mask = Date.FormatToMask();           
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            txtFromDate.Mask = Date.FormatToMask();
            txtFromDate.Text = Date.ToSystem(new DateTime(2009, 01, 24));          //************** must be the last reconcile date
            txtToDate.Mask = Date.FormatToMask();
            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
            //For Loading The Optional Fields
            OptionalFields();


            grdBankReconciliation.Selection.EnableMultiSelection = false;           

            grdBankReconciliation.Redim(1, 12);
            LoadcboBanks(cmboBanks);
            LoadcboParty(cmboParty);
           
            WriteHeader();

            evtEdit.Click += new EventHandler(Edit_Row_Click);
            evtchkMatch.Click += new EventHandler(ChkMatch_Row_Click);
            
            //Event when account is selected
            evtAccount.FocusLeft += new EventHandler(Account_Selected);
            evtAmountFocusLost.FocusLeft += new EventHandler(Amount_Focus_Lost);
            evtAccountFocusLost.FocusLeft += new EventHandler(evtAccountFocusLost_FocusLeft);
            grdAdjustment.Redim(2, 7);
            btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //Prepare the header part for grid
            AddGridHeader();
            //AddRowBankConciliation(1);
            AddRowBankConciliation1(1);
            if (m_BankReconciliationID > 0)
            {
                //Show the values in fields
                try
                {
                    ChangeState(EntryMode.NORMAL);
                    int vouchID = 0;
                    try
                    {
                        vouchID = m_BankReconciliationID;
                    }
                    catch (Exception)
                    {
                        vouchID = 999999999; //set to maximum so that it automatically gets the highest
                    }
                    BankReconciliation m_bankreconciliation = new BankReconciliation();
                    int SeriesIDD = m_bankreconciliation.GetSeriesIDFromMasterID(vouchID);
                    DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                    if (dt.Rows.Count <= 0)
                    {
                        Global.Msg("There is no any SeriesName in this Journal");
                        cboSeriesName.Text = "";
                    }
                    else
                    {
                        DataRow dr = dt.Rows[0];
                        cboSeriesName.Text = dr["EngName"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Recursive Function to Show Access Level in Treeview
        private void ShowAccClassInTreeView(TreeView tv, TreeNode n, int AccClassID)
        {
            #region Language Management
            tv.Font = LangMgr.GetFont();
            string LangField = "EngName";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;
            }
            #endregion

            if (Global.GlobalAccClassID == 1 && Global.GlobalAccessRoleID == 37)
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = AccountClass.GetAccClassTable(AccClassID);
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    TreeNode t = new TreeNode(dr[LangField].ToString());
                    t.Tag = dr["AccClassID"].ToString();
                    //Check if it is a parent Or if it has childs
                    try
                    {
                        if (ChildCount((int)dr["AccClassID"]) > 0)
                        {
                            //t.IsContainer = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                    if (n == null)
                    {
                        t.Checked = true;
                        tv.Nodes.Add(t); //Primary Group
                    }
                    else
                    {
                        t.Checked = true;
                        n.Nodes.Add(t); //Secondary Group
                    }
                }
            }
            else
            {

                DataTable dtUserInfo = User.GetUserInfo(User.CurrUserID); //user id must be read from  global i.e current user id
                DataRow drUserInfo = dtUserInfo.Rows[0];
                ArrayList AccClassChildIDs = new ArrayList();
                ArrayList tempParentAccClassChildIDs = new ArrayList();
                AccClassChildIDs.Clear();
                AccClassChildIDs.Add(Convert.ToInt32(drUserInfo["AccClassID"]));
                AccountClass.GetChildIDs(Convert.ToInt32(drUserInfo["AccClassID"]), ref AccClassChildIDs);
                DataTable dt = new DataTable();
                try
                {
                    dt = AccountClass.GetAccClassTable(AccClassID);
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    TreeNode t = new TreeNode(dr[LangField].ToString());
                    t.Tag = dr["AccClassID"].ToString();
                    tempParentAccClassChildIDs.Clear();
                    AccountClass.GetChildIDs(Convert.ToInt32(t.Tag), ref tempParentAccClassChildIDs);
                    //Check if it is a parent Or if it has childs
                    try
                    {
                        if (ChildCount((int)dr["AccClassID"]) > 0)
                        {
                            //t.IsContainer = true;
                        }

                        foreach (int itemIDs in AccClassChildIDs)  //To check if 
                        {
                            if (Convert.ToInt32(t.Tag) == itemIDs)
                            {
                                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                                loopCounter--;
                                t.Checked = true;
                                if (n == null)
                                {
                                    tv.Nodes.Add(t); //Primary Group
                                    return;
                                }
                                else if (Convert.ToInt32(t.Tag) == Convert.ToInt32(drUserInfo["AccClassID"]))
                                {
                                    t.Checked = true;
                                    tv.Nodes.Add(t);
                                    return;
                                }
                                else
                                {
                                    n.Nodes.Add(t); //Secondary Group
                                }
                            }
                            if (tempParentAccClassChildIDs.Contains(itemIDs) && loopCounter == 0)
                            {
                                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            //DataTable dtUserInfo = User.GetUserInfo(User.CurrUserID); //user id must be read from  global i.e current user id
            //DataRow drUserInfo = dtUserInfo.Rows[0];
            //ArrayList AccClassChildIDs = new ArrayList();
            //ArrayList tempParentAccClassChildIDs = new ArrayList();
            //AccClassChildIDs.Clear();
            //AccClassChildIDs.Add(Convert.ToInt32(drUserInfo["AccClassID"]));            
            //AccountClass.GetChildIDs(Convert.ToInt32(drUserInfo["AccClassID"]), ref AccClassChildIDs);
            //DataTable dt = new DataTable();
            //try
            //{
            //    dt = AccountClass.GetAccClassTable(AccClassID);
            //}
            //catch (Exception ex)
            //{
            //    Global.Msg(ex.Message);
            //}

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dt.Rows[i];
            //    TreeNode t = new TreeNode(dr[LangField].ToString());
            //    t.Tag = dr["AccClassID"].ToString();
            //    tempParentAccClassChildIDs.Clear();
            //    AccountClass.GetChildIDs(Convert.ToInt32(t.Tag), ref tempParentAccClassChildIDs);
            //    //Check if it is a parent Or if it has childs
            //    try
            //    {
            //        if (ChildCount((int)dr["AccClassID"]) > 0)
            //        {
            //            //t.IsContainer = true;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    foreach (int itemIDs in AccClassChildIDs)  //To check if 
            //    {
            //        if (Convert.ToInt32(t.Tag) == itemIDs)
            //        {
            //            ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //            loopCounter--;
            //            t.Checked = true;
            //            if (n == null)
            //            {
            //                tv.Nodes.Add(t); //Primary Group
            //                return;
            //            }
            //            else if (Convert.ToInt32(t.Tag) == Convert.ToInt32(drUserInfo["AccClassID"]))
            //            {
            //                t.Checked = true;
            //                tv.Nodes.Add(t);
            //                return;
            //            }
            //            else
            //            {
            //                n.Nodes.Add(t); //Secondary Group
            //            }
            //        }
            //        if (tempParentAccClassChildIDs.Contains(itemIDs) && loopCounter == 0)
            //        {
            //            ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
            //        }
            //    }
            //}
            //Insert the tag on the selected node to carry AccClassID
        }

        private int ChildCount(int AccClassID)
        {
            try
            {
                int m_RecCount = (int)User.GetChildTable(AccClassID).Rows.Count;
                return m_RecCount;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void LoadcboBanks(ComboBox cboBanks)
        {
            try
            {
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                int BankID = AccountGroup.GetGroupIDFromGroupNumber(7);
                DataTable dtBankLedgers = Ledger.GetAllLedger(BankID);
                foreach (DataRow drBankLedgers in dtBankLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drBankLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboBanks.Items.Add(new ListItem((int)drBankLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cboBanks.DisplayMember = "value";//This value is  for showing at Load condition
                cboBanks.ValueMember = "id";//This value is stored only not to be shown at Load condition  
                foreach (ListItem lst in cboBanks.Items)
                {
                    if (lst.ID == Convert.ToInt32(Settings.GetSettings("DEFAULT_BANK_ACCOUNT")))
                    {
                        cboBanks.Text = lst.Value;
                        break;
                    }
                }
              }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void LoadcboParty(ComboBox cboParty)
        {
            try
            {
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList
                cboParty.Items.Add(new ListItem(0, "All"));
                int CreditorID = AccountGroup.GetGroupIDFromGroupNumber(114);
                DataTable dtCreditorLedgers = Ledger.GetAllLedger(CreditorID);
                foreach (DataRow drCreditorLedgers in dtCreditorLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCreditorLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboParty.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }

                int DebtorID = AccountGroup.GetGroupIDFromGroupNumber(29);
                DataTable dtDebtorLedgers = Ledger.GetAllLedger(DebtorID);
                foreach (DataRow drDebtorLedgers in dtDebtorLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drDebtorLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboParty.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cboParty.SelectedIndex = 0;
                cboParty.DisplayMember = "value";//This value is  for showing at Load condition
                cboParty.ValueMember = "id";//This value is stored only not to be shown at Load condition                 
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Account_Selected(object sender, EventArgs e)
        {

            SourceGrid.CellContext ct = (SourceGrid.CellContext)sender;
            if (ct.DisplayText == "")
                return;
            int RowCount = grdAdjustment.RowsCount;
            ////Add a new row
            string CurRow = (string)grdAdjustment[RowCount - 1, 2].Value;

            ////Check whether the new row is already added
            if (CurRow != "(NEW)")
            {
                //AddRowBankConciliation(RowCount);
                AddRowBankConciliation1(RowCount);
                //Clear (NEW) on other colums as well
                //ClearNew(RowCount - 1);
            }
        }

        private void ClearNew(int RowCount)
        {
            if (grdAdjustment[RowCount, 2].Value == "(NEW)")
                grdAdjustment[RowCount, 2].Value = "";
            //if (grdAdjustment[RowCount, 3].Value == "(NEW)")
            //    grdAdjustment[RowCount, 3].Value = "";
            if (grdAdjustment[RowCount, 4].Value == "(NEW)")
                grdAdjustment[RowCount, 4].Value = "";
            if (grdAdjustment[RowCount, 5].Value == "(NEW)")
                grdAdjustment[RowCount, 5].Value = "";

        }

        /// <summary>
        /// Adds the row in the Journal field
        /// </summary>
        /// 

        private void AddRowBankConciliation1(int RowCount)
        {
            //Add a new row
            grdAdjustment.Redim(Convert.ToInt32(RowCount + 1), grdAdjustment.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            grdAdjustment[i, 0] = btnDelete;
            grdAdjustment[i, 0].AddController(evtEdit);

            grdAdjustment[i, 1] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdAdjustment[i, 2] = new SourceGrid.Cells.Cell("", txtAccount);
            txtAccount.Control.GotFocus += new EventHandler(Account_Focused);
            txtAccount.Control.LostFocus += new EventHandler(Account_Leave);
            txtAccount.Control.KeyDown += new KeyEventHandler(Account_KeyDown);
            txtAccount.Control.TextChanged += new EventHandler(Text_Change);
            grdAdjustment[i, 2].AddController(evtAccountFocusLost);
            grdAdjustment[i, 2].Value = "(NEW)";

            //SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            //cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
            //cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            //cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;
            //string strDrCr = "Debit";
            //if (grdAdjustment[i - 1, 3].Value.ToString() == "Debit")
            //    strDrCr = "Credit";
            //grdAdjustment[i, 3] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);

            //SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            //txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            //grdJournal[i, 4] = new SourceGrid.Cells.Cell("", txtAmount);
            //txtAmount.Control.TextChanged += new EventHandler(Text_Change);
            //grdJournal[i, 4].AddController(evtAmountFocusLost);

            //SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            //txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            //grdAdjustment[i, 3] = new SourceGrid.Cells.Cell("", txtAmount);
            //grdAdjustment[i, 3].AddController(evtAmountFocusLost);
            //grdAdjustment[i, 3].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdAdjustment[i, 4] = new SourceGrid.Cells.Cell("", txtAmount);
            txtAmount.Control.TextChanged += new EventHandler(Text_Change);
            grdAdjustment[i, 4].AddController(evtAmountFocusLost);
            grdAdjustment[i, 4].Value = "(NEW)";
            //grdAdjustment[i, 4].Value = lblBankReconcilationDiff.Text;

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdAdjustment[i, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
            grdAdjustment[i, 5].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdAdjustment[i, 6] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdAdjustment[i, 6].Value = "";
 
        }
        private void AddRowBankConciliation(int RowCount)
        {
           
            grdAdjustment.Redim(Convert.ToInt32(RowCount + 1), grdAdjustment.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;
            grdAdjustment[i, 0] = btnDelete;
            grdAdjustment[i, 0].AddController(evtEdit);
            grdAdjustment[i, 1] = new SourceGrid.Cells.Cell(i.ToString());
            SourceGrid.Cells.Editors.ComboBox cboAccount = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboAccount.StandardValues = Ledger.GetLedgerList(0);
            cboAccount.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboAccount.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
            //cboAccount.Control.LostFocus += new EventHandler(Account_Selected);
            cboAccount.EditableMode = SourceGrid.EditableMode.Focus;
            grdAdjustment[i, 2] = new SourceGrid.Cells.Cell("", cboAccount);
            grdAdjustment[i, 2].AddController(evtAccount);
            grdAdjustment[i, 2].Value = "(NEW)";

            SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
            cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;
            string strDrCr = "Debit";
            if (grdAdjustment[i - 1, 3].Value.ToString() == "Debit")
                strDrCr = "Credit";
            grdAdjustment[i, 3] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdAdjustment[i, 4] = new SourceGrid.Cells.Cell("", txtAmount);
            grdAdjustment[i, 4].AddController(evtAmountFocusLost);
            grdAdjustment[i, 4].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdAdjustment[i, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
            grdAdjustment[i, 5].Value = "(NEW)";
        }

        private void Edit_Row_Click(object sender, EventArgs e)
        {           
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            if (ctx.Position.Row <= grdBankReconciliation.RowsCount - 2)
            {
                try
                {
                    //Get the Selected Row
                    int CurRow = grdBankReconciliation.Selection.GetSelectionRegion().GetRowsIndex()[0];
                    SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdBankReconciliation, new SourceGrid.Position(CurRow, 9));
                    SourceGrid.CellContext RowIDFromCell = new SourceGrid.CellContext(grdBankReconciliation, new SourceGrid.Position(CurRow, 10));                   
                    int RowID = Convert.ToInt32(RowIDFromCell.Value);
                    string VoucherType = (cellType.Value).ToString();

                    switch (VoucherType)
                    {

                        case "JNL":
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
                    }
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
            }          
        }

        private void ChkMatch_Row_Click(object sender, EventArgs e)
        {
            //SourceGrid.Cells.CheckBox ctx = (SourceGrid.Cells.CheckBox)sender;
            //for(int i = 1; i<= grdBankReconciliation.Rows.Count; i++)
            //{
            //    if (ctx.Checked == true)
            //    {
            //        CheckedView = new SourceGrid.Cells.Views.Cell();
            //        CheckedView.BackColor = Color.AliceBlue;
            //        ////grdBankReconciliation.Rows.Select(
            //        //SourceGrid.Cells.Views.Cell.DefaultBackColor = Color.AliceBlue;
            //        ////grdBankReconciliation.Rows[ctx.Row.Index].b
            //    }
            //}
            ////grdBankReconciliation.Rows[ctx.Position.Row].Grid.BackColor = Color.Red;

            //if (ctx.Position.Row <= grdBankReconciliation.RowsCount - 2)
            //{
            //    CheckBox chkBox = (CheckBox)sender;
            //    if (chkBox.Checked)
            //    {
            //        SourceGrid.RowInfo row = grdBankReconciliation.Rows[];
            //        row.Grid.BackColor = Color.LightYellow;
            //    }
            //}
            ////CalculateDrCr();
        }

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdAdjustment.RowsCount;
            string AccName = (string)(grdAdjustment[RowCount - 1, 2].Value);
            //SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //int CurRow = ctx.Position.Row;
           
            ////Check if the input value is correct
            //if (grdAdjustment[Convert.ToInt32(CurRow), 4].Value == "" || grdAdjustment[Convert.ToInt32(CurRow), 4].Value == null)
            //    grdAdjustment[Convert.ToInt32(CurRow), 4].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            //if (!Misc.IsNumeric(grdAdjustment[Convert.ToInt32(CurRow), 4].Value))
            //{
            //    Global.MsgError("Invalid Amount!");
            //    ctx.Value = "";
            //    return;
            //}

            //FillGridRowExceptLedgerID(CurRow, CurrAccLedgerID, CurrBal);
           // CalculateDrCr();
            if (AccName != "(NEW)")
            {
                //AddRowBankConciliation(RowCount);
                AddRowBankConciliation1(RowCount); 
            }
            //CalculateDrCr();
        }

        //private void Amount_Focus_Lost(object sender, EventArgs e)
        //{
        //    //CalculateDrCr();
        //}


        private void AddGridHeader()
        {
            grdAdjustment[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
            grdAdjustment[0, 1] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdAdjustment[0, 2] = new SourceGrid.Cells.ColumnHeader("Account");
            grdAdjustment[0, 3] = new SourceGrid.Cells.ColumnHeader("Dr/Cr");
            grdAdjustment[0, 4] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdAdjustment[0, 5] = new SourceGrid.Cells.ColumnHeader("Remarks");
            grdAdjustment[0, 6] = new SourceGrid.Cells.ColumnHeader("LedgerID");
            grdAdjustment[0, 0].Column.Width = 25;
            grdAdjustment[0, 2].Column.Width = 120;
            grdAdjustment[0, 5].Column.Width = 140;
            grdAdjustment[0, 6].Column.Visible = false;
            grdAdjustment[0, 3].Column.Visible = false;

        }

        private void WriteHeader()
        {
            //Define Header Part
            grdBankReconciliation[0, 0] = new SourceGrid.Cells.ColumnHeader("S.N.");
            grdBankReconciliation[0, 1] = new SourceGrid.Cells.ColumnHeader("Date");
            grdBankReconciliation[0, 2] = new SourceGrid.Cells.ColumnHeader("Party Name");
            //grdBankReconciliation[0, 3] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdBankReconciliation[0, 3] = new SourceGrid.Cells.ColumnHeader("Debit Amount");
            grdBankReconciliation[0, 4] = new SourceGrid.Cells.ColumnHeader("Credit Amount");
            grdBankReconciliation[0, 5] = new SourceGrid.Cells.ColumnHeader("Cheque Number");
            grdBankReconciliation[0, 6] = new SourceGrid.Cells.ColumnHeader("Cheque Bank");
            grdBankReconciliation[0, 7] = new SourceGrid.Cells.ColumnHeader("Cheque Date");
            grdBankReconciliation[0, 8] = new SourceGrid.Cells.ColumnHeader("Matched");
            grdBankReconciliation[0, 9] = new SourceGrid.Cells.ColumnHeader("Edit");
            grdBankReconciliation[0, 10] = new SourceGrid.Cells.ColumnHeader("VoucherType");
            grdBankReconciliation[0, 11] = new SourceGrid.Cells.ColumnHeader("RowID");

            //Define width of column size
            grdBankReconciliation[0, 0].Column.Width = 30;
            grdBankReconciliation[0, 1].Column.Width = 70;
            grdBankReconciliation[0, 2].Column.Width = 150;
            grdBankReconciliation[0, 3].Column.Width = 100;
            grdBankReconciliation[0, 4].Column.Width = 100;
            grdBankReconciliation[0, 5].Column.Width = 100;
            grdBankReconciliation[0, 6].Column.Width = 100;
            grdBankReconciliation[0, 7].Column.Width = 70;
            grdBankReconciliation[0, 8].Column.Width = 60;
            grdBankReconciliation[0, 9].Column.Width = 60;
            grdBankReconciliation[0, 10].Column.Width = 10;
            grdBankReconciliation[0, 11].Column.Width = 10;
            grdBankReconciliation[0, 10].Column.Visible = false;
            grdBankReconciliation[0, 11].Column.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (drdtadditionalfield["IsField1Required"].ToString() == "True")
            {
                if (txtfirst.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field1"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField2Required"].ToString() == "True")
            {
                if (txtsecond.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field2"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField3Required"].ToString() == "True")
            {
                if (txtthird.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field3"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            if (drdtadditionalfield["IsField4Required"].ToString() == "True")
            {
                if (txtfourth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field4"].ToString() + " " + "is Required Field");
                    return;
                }

            }
            if (drdtadditionalfield["IsField5Required"].ToString() == "True")
            {
                if (txtfifth.Text == "")
                {
                    MessageBox.Show(drdtadditionalfield["Field5"].ToString() + " " + "is Required Field");
                    return;
                }
            }
            switch (m_mode)
            {

             #region NEW
                case EntryMode.NEW: //if new button is pressed
                    try
                    {
                        //string BankReconciliationXMLString = ReadAllBankReconciliationEntry();
                        //if (BankReconciliationXMLString == string.Empty)
                        //{
                        //    MessageBox.Show("Unable to cast bank Reconciliation entry to XML!");
                        //    return;
                        //}
                        //#region Save xml data to Database
                        //using (System.IO.StringWriter swStringWriter = new StringWriter())
                        //{
                        //    Global.m_db.ClearParameter();
                        //    Global.m_db.setCommandType(CommandType.StoredProcedure);
                        //    Global.m_db.setCommandText("Acc.[xmlBankReconciliationInsert]");
                        //    Global.m_db.AddParameter("@bankreconciliation",SqlDbType.Xml,BankReconciliationXMLString);
                        //    //System.Data.SqlClient.SqlParameter objReturn = Global.m_db.AddOutputParameter("@Return", SqlDbType.NVarChar, 20);
                        //    Global.m_db.ProcessParameter();
                            

                            //using (SqlCommand dbCommand = new SqlCommand("Acc.[xmlBankReconciliationInsert]", Global.m_db.cn))
                            //{
                            //    // we are going to use store procedure  
                            //    dbCommand.CommandType = CommandType.StoredProcedure;
                            //    // Add input parameter and set its properties.
                            //    SqlParameter parameter = new SqlParameter();
                            //    // Store procedure parameter name  
                            //    parameter.ParameterName = "@bankreconciliation";
                            //    // Parameter type as XML 
                            //    parameter.DbType = DbType.Xml;
                            //    parameter.Direction = ParameterDirection.Input; // Input Parameter  
                            //    parameter.Value = BankReconciliationXMLString; // XML string as parameter value  
                            //    // Add the parameter in Parameters collection.
                            //    dbCommand.Parameters.Add(parameter);
                            //    // Global.m_db.cn.Close();
                            //    Global.m_db.cn.Open();
                            //    int intRetValue = dbCommand.ExecuteNonQuery();
                            //    Global.m_db.cn.Close();
                            //    //MessageBox.Show(intRetValue.ToString());                                
                            //}
                       // }
                      //  #endregion

                        
                        //Read from sourcegrid and store it to table
                       DataTable BankReConciliationDetails = new DataTable();
                        BankReConciliationDetails.Columns.Add("Ledger");
                        BankReConciliationDetails.Columns.Add("DrCr");
                        BankReConciliationDetails.Columns.Add("Amount");
                        BankReConciliationDetails.Columns.Add("Remarks");
                        BankReConciliationDetails.Columns.Add("LedgerID");
                        for (int i = 0; i < grdAdjustment.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            if (rbtnreceived.Checked)
                            {
                                if (grdAdjustment[i + 1, 2].Value.ToString() == label15.Text)
                                {
                                    //BankReConciliationDetails.Rows.Add(grdAdjustment[i + 1, 2].Value, grdAdjustment[i + 1, 3].Value, grdAdjustment[i + 1, 4].Value, grdAdjustment[i + 1, 5].Value);
                                    BankReConciliationDetails.Rows.Add(grdAdjustment[i + 1, 2].Value, "Debit", grdAdjustment[i + 1, 4].Value, grdAdjustment[i + 1, 5].Value, grdAdjustment[i + 1, 6].Value);
                                }
                                else
                                {
                                    BankReConciliationDetails.Rows.Add(grdAdjustment[i + 1, 2].Value, "Credit", grdAdjustment[i + 1, 4].Value, grdAdjustment[i + 1, 5].Value, grdAdjustment[i + 1, 6].Value);
                                }
                            }
                            if (rbtnpayments.Checked)
                            {
                                if (grdAdjustment[i + 1, 2].Value.ToString() == label15.Text)
                                {
                                    //BankReConciliationDetails.Rows.Add(grdAdjustment[i + 1, 2].Value, grdAdjustment[i + 1, 3].Value, grdAdjustment[i + 1, 4].Value, grdAdjustment[i + 1, 5].Value);
                                    BankReConciliationDetails.Rows.Add(grdAdjustment[i + 1, 2].Value, "Credit", grdAdjustment[i + 1, 4].Value, grdAdjustment[i + 1, 5].Value, grdAdjustment[i + 1, 6].Value);
                                }
                                else
                                {
                                    BankReConciliationDetails.Rows.Add(grdAdjustment[i + 1, 2].Value, "Debit", grdAdjustment[i + 1, 4].Value, grdAdjustment[i + 1, 5].Value, grdAdjustment[i + 1, 6].Value);
                                }
                            }
                        }
                        IJournal m_Journal = new Journal();
                        DateTime BankReconciliationDate = Date.ToDotNet(txtDate.Text);
                        ListItem liLedgerID = new ListItem();
                        ListItem liGroupID = new ListItem();

                        liGroupID = (ListItem)cboSeriesName.SelectedItem;
                        int seriesid = liGroupID.ID;
                        string seriesname = liGroupID.Value.ToString();

                        liLedgerID = (ListItem)cmboBanks.SelectedItem;
                        int LedgerID = liLedgerID.ID;
                        string ledgername = liLedgerID.Value;
                        ArrayList arrNode = treeAccClass.GetCheckedNodes(true);

                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        foreach (string tag in arrNode)
                        {
                            AccClassIDD.Add(Convert.ToInt32(tag));
                        }
                        if (AccClassIDD.Count != 0)
                        {
                            m_BankReconciliation.Create(seriesname, LedgerID, BankReconciliationDate, BankReConciliationDetails, txtVchNo.Text, 1, ledgername, seriesid, AccClassIDD.ToArray(), OF);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                           // m_Contra.Create(Convert.ToInt32(SeriesID.ID), txtVchNo.Text, ContraDate, txtRemarks.Text, ContraDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID));
                            m_BankReconciliation.Create(seriesname, LedgerID, BankReconciliationDate, BankReConciliationDetails, txtVchNo.Text, 1, ledgername, seriesid, a.ToArray(), OF);


                        }
                        
                        //m_BankReconciliation.Create(seriesname, LedgerID, BankReconciliationDate, BankReConciliationDetails, txtVchNo.Text, 1);
                        //m_Journal.Create("0",BankReconciliationDate,LedgerID, BankConciliationDetails);
                        ClearVoucher();
                        Global.Msg("BankReconciliation created successfully!");
                        //ChangeState(EntryMode.NORMAL);
                        ChangeState(EntryMode.NEW);

                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        #region SQLExceptions
                        switch (ex.Number)
                        {
                            case 4060: // Invalid Database 
                                Global.Msg("Invalid Database", MBType.Error, "Error");
                                break;

                            case 18456: // Login Failed 
                                Global.Msg("Login Failed!", MBType.Error, "Error");
                                break;

                            case 547: // ForeignKey Violation , Check Constraint
                                Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                                break;

                            case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                                Global.Msg("ERROR: The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                                break;

                            case 2601: // Unique Index/Constriant Violation 
                                Global.Msg("Unique index violation!", MBType.Warning, "Error");
                                break;

                            case 5000: //Trigger violation
                                Global.Msg("Trigger violation!", MBType.Warning, "Error");
                                break;

                            default:
                                Global.MsgError(ex.Message);
                                break;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
             #endregion
          }


        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void frmBankReconciliation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
           
        }


         /// <summary>
        /// This block shows the transaction of particular Ledger with others except ownself
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="LedgerID"></param>
        /// <param name="IsCrystalReport"></param>
        private void ShowVoucherTransaction(DataTable dt,int LedgerID)
        {
            double Balance = 0;
            double TotalDrAmt, TotalCrAmt;
            TotalDrAmt = TotalCrAmt = 0;           
            string VouType = "";
            string ChequeNumber = "";
            string ChequeBank = "";
            DateTime? ChequeDate = new DateTime();           
            DataTable dtBankPaymentDetail = new DataTable();
            DataTable dtBankReceiptDetail = new DataTable();            

            #region BLOCK FOR OPENING BALANCE 
            //Show the opening Balance of corresponding Ledger
            double DrOpBal, CrOpBal;
            DrOpBal = CrOpBal = 0;
            DataTable dtLedgerInfo =OpeningBalance.GetAccClassOpeningBalance(GetRootAccClassID(),LedgerID);          
            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
            {
                if (drLedgerInfo["OpenBalDrCr"].ToString() == "DEBIT")//IF ledger has Debit openig balance
                {
                    DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                    Balance = DrOpBal;
                    //WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);                                                       
                }
                else if (drLedgerInfo["OpenBalDrCr"].ToString() == "CREDIT")//IF ledger has credit Opening balance
                {
                    CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                    Balance = (-CrOpBal);
                    //WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);               
                }
            }
            #endregion

            int Sno = 1;
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i - 1];
                ListItem liPartyID = new ListItem();
                liPartyID = (ListItem)cmboParty.SelectedItem;

                #region BLOCK FOR GETTING THE INFORMATION OF TRANSACTION ACCORDING TO ROWID AND VOUCHERTYPE
                DataTable dtTransactInfo = new DataTable();
                if (rbtnAll.Checked)
                {
                    dtTransactInfo = m_Transaction.GetTransactionInfoAlongWithParty(dr["RowID"].ToString(), dr["VoucherType"].ToString(),liPartyID.ID, AccClassID);//Get information from tblTransaction according to RowID and voucher type
                }
                if (rbtnReceipt.Checked)
                {
                    if (dr["VoucherType"].ToString() == "BANK_RCPT")
                    {
                        dtTransactInfo = m_Transaction.GetTransactionInfoAlongWithParty(dr["RowID"].ToString(), dr["VoucherType"].ToString(),liPartyID.ID, AccClassID);//Get information from tblTransaction according to RowID and voucher type
                    }
                    else
                    {
                        continue;
                    }
                }
                if (rbtnPayment.Checked)
                {
                    if (dr["VoucherType"].ToString() == "BANK_PMNT")
                    {
                        dtTransactInfo = m_Transaction.GetTransactionInfoAlongWithParty(dr["RowID"].ToString(), dr["VoucherType"].ToString(),liPartyID.ID, AccClassID);//Get information from tblTransaction according to RowID and voucher type
                    }
                    else
                    {
                        continue;
                    }
                }
                //if (liPartyID.ID != LedgerID && liPartyID.ID != 0)
                //{
                //    continue;
                //}
                for (int j = 0; j < dtTransactInfo.Rows.Count; j++)
                {
                    DataRow drTransactInfo = dtTransactInfo.Rows[j];
                    DataTable dtLedgerAllInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactInfo["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerAllInfo = dtLedgerAllInfo.Rows[0];                  
                    
                    //Use switch case for each Voucher Type
                    string VoucherType = drTransactInfo["VoucherType"].ToString();

                    switch (VoucherType)
                    {
                        case "BANK_PMNT":
                        case "CASH_PMNT":
                        case "CR_NOT":
                        case "SALES":

                            #region BLOCK FOR BANK PAYMENT,CASH PAYMENT,CREDIT NOTE AND SALES INVOICE
                            BankPayment m_BankPayment = new BankPayment();
                            CashPayment m_CashPayment = new CashPayment();
                            CreditNote m_CreditNote = new CreditNote();
                            Sales m_Sales = new Sales();
                  
                    if (LedgerID == Convert.ToInt32(drTransactInfo["LedgerID"]))//This is the transaction done by specified Ledger hit by user to other ledgers except own
                            {
                                if (Convert.ToDouble(drTransactInfo["Debit_Amount"]) == 0)//If ledger has Credit amount
                                {                                    
                                    DataTable dtTransactDtlInfo = m_Transaction.GetTransactionInfo(drTransactInfo["RowID"].ToString(), VoucherType,AccClassID);
                                    for (int k1 = 0; k1 < dtTransactDtlInfo.Rows.Count; k1++)
                                    {                           
                                        DataRow drTransactDtlInfo = dtTransactDtlInfo.Rows[k1];
                                        if (Convert.ToDouble(drTransactDtlInfo["Credit_Amount"]) == 0)
                                        {                                           
                                            DataTable dtledgName = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactDtlInfo["LedgerID"]), LangMgr.DefaultLanguage);
                                            DataRow drLedName = dtledgName.Rows[0];                                            
                                            //Get the voucher Name from tbl.BankPaymentMaster
                                            DataTable dtVoucherName = new DataTable();                                           
                                            
                                            if (VoucherType == "BANK_PMNT")
                                                dtVoucherName = m_BankPayment.GetBankPaymentMaster(Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                            if (dtVoucherName.Rows.Count > 0)
                                            {
                                                //Get bank payment detail from bank payment master using bankpaymentID
                                                dtBankPaymentDetail = m_BankPayment.GetBankPaymentDetail(Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                               foreach(  DataRow drBankPaymentDetail in dtBankPaymentDetail.Rows)
                                                {
                                                    if (Convert.ToInt32(drBankPaymentDetail["LedgerID"]) == Convert.ToInt32(drTransactDtlInfo["LedgerID"]))
                                                    {
                                                        if (drBankPaymentDetail["ChequeDate"] != DBNull.Value)
                                                        {
                                                            ChequeNumber = drBankPaymentDetail["ChequeNumber"].ToString();
                                                            ChequeBank = "";
                                                            ChequeDate =Convert.ToDateTime(Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()));
                                                        }
                                                        else
                                                        {
                                                            ChequeNumber = "";
                                                            ChequeBank = "";
                                                            ChequeDate = null;
                                                        }
                                                    }                                                   
                                                }
                                            }
                                            else if (VoucherType == "CASH_PMNT")
                                                dtVoucherName = m_CashPayment.GetCashPaymentMaster(Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                            else if (VoucherType == "CR_NOT")
                                                dtVoucherName = m_CreditNote.GetCreditNoteMaster(Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                            else if (VoucherType == "SALES")
                                                dtVoucherName = m_Sales.GetSalesInvoiceMasterInfo(drTransactDtlInfo["RowID"].ToString());
                                            DataRow drVoucherName = dtVoucherName.Rows[0];
                                            //posting the Debit and Credit amount of particular ledger in frontend just reversal of Database's value eg. if ledger has Dr amount in database then post it in Credit side in frontend
                                            TotalCrAmt += (Convert.ToDouble(drTransactDtlInfo["Debit_Amount"]));
                                            //While calculating Balance we just apply cumulative addition
                                            //we will add the the deduction value of frontend dr and cr(Debit-Credit) balance to Previous balance
                                            //Remember fronend Dr and Cr value is just reversal of Database's value     
                                            string camt =drTransactDtlInfo["Credit_Amount"].ToString();
                                            string damt = drTransactDtlInfo["Debit_Amount"].ToString();
                                          
                                            //Balance += (Convert.ToDouble(drTransactDtlInfo["Credit_Amount"]) - Convert.ToDouble(drTransactDtlInfo["Debit_Amount"]));
                                            Balance +=   (Convert.ToDouble(drTransactDtlInfo["Debit_Amount"]))-(Convert.ToDouble(drTransactDtlInfo["Credit_Amount"]));
                                            if (Balance > 0)//If balance is greater than zero  then just it represt Debit Balance
                                            {
                                               // WriteTransaction(Sno, Date.DBToSystem(drTransactDtlInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), damt, (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactDtlInfo["RowID"]));                                               
                                                WriteTransaction(Sno, Date.DBToSystem(drTransactDtlInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), Convert.ToDouble(drTransactDtlInfo["Debit_Amount"]).ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactDtlInfo["RowID"]));                                               
                                            }
                                            else if (Balance < 0)
                                            {
                                               //WriteTransaction(Sno, Date.DBToSystem(drTransactDtlInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)),camt, ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                                WriteTransaction(Sno, Date.DBToSystem(drTransactDtlInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDouble(drTransactDtlInfo["Credit_Amount"]).ToString(), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                            }
                                            Sno++;
                                        }
                                    }
                                }
                                else
                                {
                                    DataTable dtTransactDtlInfo = m_Transaction.GetTransactionInfo(drTransactInfo["RowID"].ToString(), VoucherType,AccClassID);
                                    foreach (DataRow drTransactDtlInfo in dtTransactDtlInfo.Rows)
                                    {
                                        if (Convert.ToDouble(drTransactDtlInfo["Debit_Amount"]) == 0)
                                        {
                                            DataTable dtledgName = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactDtlInfo["LedgerID"]), LangMgr.DefaultLanguage);
                                            DataRow drLedName = dtledgName.Rows[0];  

                                            DataTable dtVoucherName = new DataTable();
                                            if (VoucherType == "BANK_PMNT")
                                                dtVoucherName = m_BankPayment.GetBankPaymentMaster(Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                            if (dtVoucherName.Rows.Count > 0)
                                            {
                                                //Get bank payment detail from bank payment master using bankpaymentID
                                                dtBankPaymentDetail = m_BankPayment.GetBankPaymentDetail(Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                                foreach (DataRow drBankPaymentDetail in dtBankPaymentDetail.Rows)
                                                {
                                                    if (Convert.ToInt32(drBankPaymentDetail["LedgerID"]) == Convert.ToInt32(drTransactDtlInfo["LedgerID"]))
                                                    {
                                                        if (drBankPaymentDetail["ChequeDate"] != DBNull.Value)
                                                        {
                                                            ChequeNumber = drBankPaymentDetail["ChequeNumber"].ToString();
                                                            ChequeBank = "";
                                                            ChequeDate = Convert.ToDateTime(Date.DBToSystem(drBankPaymentDetail["ChequeDate"].ToString()));
                                                        }
                                                        else
                                                        {
                                                            ChequeNumber = "";
                                                            ChequeBank = "";
                                                            ChequeDate = null;
                                                        }
                                                    }                                                  
                                                }
                                            }

                                            else if (VoucherType == "CASH_PMNT")
                                                dtVoucherName = m_CashPayment.GetCashPaymentMaster(Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                            else if (VoucherType == "CR_NOT")
                                                dtVoucherName = m_CreditNote.GetCreditNoteMaster(Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                            else if (VoucherType == "SALES")
                                                dtVoucherName = m_Sales.GetSalesInvoiceMasterInfo(drTransactDtlInfo["RowID"].ToString());
                                            DataRow drVoucherName = dtVoucherName.Rows[0];
                                            //posting the Debit and Credit amount of particular ledger in frontend just reversal of Database's value eg. if ledger has Dr amount in database then post it in Credit side in frontend
                                            //TotalDrAmt += (Convert.ToDouble(drTransactInfo["Credit_Amount"]) + DrOpBal);
                                            TotalCrAmt += (Convert.ToDouble(drTransactInfo["Debit_Amount"]));
                                            //While calculating Balance we just apply cumulative addition
                                            //we will add the the deduction value of frontend dr and cr(Debit-Credit) balance to Previous balance
                                            //Remember fronend Dr and Cr value is just reversal of Database's value
                                            double camt = Convert.ToDouble(drTransactDtlInfo["Credit_Amount"]);
                                            double damt = Convert.ToDouble(drTransactDtlInfo["Debit_Amount"]);
                                            Balance += (Convert.ToDouble(drTransactInfo["Credit_Amount"]) - Convert.ToDouble(drTransactInfo["Debit_Amount"]));
                                            if (Balance > 0)//If balance is greater than zero  then just it represt Debit Balance
                                            {
                                                WriteTransaction(Sno, Date.DBToSystem(drTransactDtlInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), Convert.ToDecimal(drTransactDtlInfo["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactDtlInfo["RowID"]));                                                
                                               // WriteTransaction(Sno, Date.DBToSystem(drTransactDtlInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), drTransactDtlInfo["Debit_Amount"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactDtlInfo["RowID"]));                                                
                                            }
                                            else if (Balance < 0)
                                            {
                                                WriteTransaction(Sno, Date.DBToSystem(drTransactDtlInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(drTransactDtlInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                                //WriteTransaction(Sno, Date.DBToSystem(drTransactDtlInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drTransactDtlInfo["Credit_Amount"].ToString(), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactDtlInfo["RowID"]));
                                            }       
                                            Sno++;
                                        }
                                    }                                
                                }
                            }

                            #endregion
                            break;

                        case "BANK_RCPT":
                        case "CASH_RCPT":
                        case "DR_NOT":
                        case "PURCHASE":
                            #region BLOCK FOR BANK RECEIPT,CASH RECEIPT AND DEBIT NOTE
                            BankReceipt m_BankReceipt = new BankReceipt();
                            CashReceipt m_CashReceipt = new CashReceipt();
                            DebitNote m_DebitNote = new DebitNote();
                            //if (liPartyID.ID != LedgerID && liPartyID.ID != 0)
                            //{
                            //    continue;
                            //}
                            if (LedgerID!= Convert.ToInt32(drTransactInfo["LedgerID"]))//Show the transaction of particular Ledger with others except ownself
                            {                         
                                if (Convert.ToDouble(drTransactInfo["Credit_Amount"]) == 0)//This is the information of Master,Incase of receipt,master ledgers have only Debit amount
                                {
                                    DataTable dtledgName = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactInfo["LedgerID"]), LangMgr.DefaultLanguage);
                                    DataRow drLedName = dtledgName.Rows[0];
                                       
                                    //Get the voucher Name from tbl.BankPaymentMaster
                                    DataTable dtVoucherName = new DataTable();
                                    if (VoucherType == "BANK_RCPT")
                                        dtVoucherName = m_BankReceipt.GetBankReceiptMaster(Convert.ToInt32(drTransactInfo["RowID"]));

                                    if (dtVoucherName.Rows.Count > 0)
                                    {
                                        //Get bank payment detail from bank payment master using bankpaymentID
                                        dtBankReceiptDetail = m_BankReceipt.GetBankReceiptDetail(Convert.ToInt32(drTransactInfo["RowID"]));
                                        foreach (DataRow drBankReceiptDetail in dtBankReceiptDetail.Rows)
                                        {
                                            if (Convert.ToInt32(drBankReceiptDetail["LedgerID"]) == Convert.ToInt32(drTransactInfo["LedgerID"]))
                                            {
                                                if (drBankReceiptDetail["ChequeDate"] != DBNull.Value)
                                                {
                                                    ChequeNumber = drBankReceiptDetail["ChequeNumber"].ToString();
                                                    ChequeBank = drBankReceiptDetail["ChequeBank"].ToString();
                                                    ChequeDate = Convert.ToDateTime(Date.DBToSystem(drBankReceiptDetail["ChequeDate"].ToString()));
                                                }
                                                else
                                                {
                                                    ChequeNumber = "";
                                                    ChequeBank = "";
                                                    ChequeDate = null;
                                                }
                                            }                                          
                                        }
                                    }

                                    else if (VoucherType == "CASH_RCPT")
                                        dtVoucherName = m_CashReceipt.GetCashReceiptMaster(Convert.ToInt32(drTransactInfo["RowID"]));
                                    else if (VoucherType == "DR_NOT")
                                        dtVoucherName = m_DebitNote.GetDebitNoteMaster(Convert.ToInt32(drTransactInfo["RowID"]));
                                    DataRow drVoucherName = dtVoucherName.Rows[0];
                                    //posting the Debit and Credit amount of particular ledger in frontend just reversal of Database's value eg. if ledger has Dr amount in database then post it in Credit side in frontend
                                    TotalCrAmt += (Convert.ToDouble(drTransactInfo["Debit_Amount"]));
                                    //While calculating Balance we just apply cumulative addition
                                    //we will add the the deduction value of frontend dr and cr(Debit-Credit) balance to Previous balance
                                    //Remember fronend Dr and Cr value is just reversal of Database's value
                                    Balance += (Convert.ToDouble(drTransactInfo["Credit_Amount"]) - Convert.ToDouble(drTransactInfo["Debit_Amount"]));
                                    if (Balance > 0)  //If balance is greater than zero  then just it represt Debit Balance 
                                    {
                                        WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), Convert.ToDecimal(drTransactInfo["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                        //WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), drTransactInfo["Debit_Amount"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                         
                                    }
                                    else if (Balance < 0)
                                    {
                                        //WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drTransactInfo["Credit_Amount"].ToString(), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                        WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(drTransactInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                    }
                                    Sno++;                 
                                }
                                else
                                {
                                    DataTable dtledgName = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactInfo["LedgerID"]), LangMgr.DefaultLanguage);
                                    DataRow drLedName = dtledgName.Rows[0];
                                    //Get the voucher Name from tbl.BankPaymentMaster
                                    DataTable dtVoucherName = new DataTable();
                                    if (VoucherType == "BANK_RCPT")
                                        dtVoucherName = m_BankReceipt.GetBankReceiptMaster(Convert.ToInt32(drTransactInfo["RowID"]));
                                    if (dtVoucherName.Rows.Count > 0)
                                    {
                                        //Get bank payment detail from bank payment master using bankpaymentID
                                        dtBankReceiptDetail = m_BankReceipt.GetBankReceiptDetail(Convert.ToInt32(drTransactInfo["RowID"]));
                                        foreach (DataRow drBankReceiptDetail in dtBankReceiptDetail.Rows)
                                        {
                                            if (Convert.ToInt32(drBankReceiptDetail["LedgerID"]) == Convert.ToInt32(drTransactInfo["LedgerID"]))
                                            {
                                                if (drBankReceiptDetail["ChequeDate"] != DBNull.Value)
                                                {
                                                    ChequeNumber = drBankReceiptDetail["ChequeNumber"].ToString();
                                                    ChequeBank = drBankReceiptDetail["ChequeBank"].ToString();
                                                    ChequeDate = Convert.ToDateTime(Date.DBToSystem(drBankReceiptDetail["ChequeDate"].ToString()));
                                                }
                                                else
                                                {
                                                    ChequeNumber = "";
                                                    ChequeBank = "";
                                                    ChequeDate = null;
                                                }
                                            }                                           
                                        }
                                    }
                                    else if (VoucherType == "CASH_RCPT")

                                        dtVoucherName = m_CashReceipt.GetCashReceiptMaster(Convert.ToInt32(drTransactInfo["RowID"]));
                                    else if (VoucherType == "DR_NOT")
                                        dtVoucherName = m_DebitNote.GetDebitNoteMaster(Convert.ToInt32(drTransactInfo["RowID"]));

                                    DataRow drVoucherName = dtVoucherName.Rows[0];
                                    //posting the Debit and Credit amount of particular ledger in frontend just reversal of Database's position eg. if ledger has Dr amount in database then post it in Credit side in frontend 
                                    TotalDrAmt += (Convert.ToDouble(drTransactInfo["Credit_Amount"]));
                                    //While calculating Balance we just apply cumulative addition
                                    //we will add the the deduction value of frontend dr and cr(Debit-Credit) balance to Previous balance
                                    //Remember fronend Dr and Cr value is just reversal of Database's value
                                    Balance +=(Convert.ToDouble(drTransactInfo["Credit_Amount"])-Convert.ToDouble(drTransactInfo["Debit_Amount"]));
                                    if (Balance > 0)//If balance is greater than zero  then just it represt Debit Balance
                                    {
                                       // MessageBox.Show(Convert.ToDecimal(drTransactInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                                        WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(drTransactInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                    }
                                    else if (Balance < 0)
                                    {
                                        WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedName["LedName"].ToString(), Convert.ToDecimal(drTransactInfo["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), ChequeNumber, ChequeBank, ChequeDate, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                       // WriteTransaction(Sno, drLedgerAllInfo["LedName"].ToString(), Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drVoucherName["Voucher_No"].ToString(),Convert.ToDecimal(drTransactInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drTransactInfo["VoucherType"].ToString(), (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "LEDGER", drTransactInfo["RowID"].ToString(), IsCrystalReport);                                        
                                    }
                                    Sno++;
                                }
                            }

                            #endregion
                            break;

                        case "JNL":
                        case "CNTR":
                            VouType = "JNL";
                            #region BLOCK FOR JOURNAL AND CONTRA
                            Journal m_Journal = new Journal();
                            Contra m_Contra = new Contra();
                            //if (liPartyID.ID != LedgerID && liPartyID.ID != 0)
                            //{
                            //    continue;
                            //}
                            if (LedgerID != Convert.ToInt32(drTransactInfo["LedgerID"]))//Show the transaction of particular Ledger with others except ownself
                            {
                                DataTable dtVoucherName = new DataTable();
                                if (VoucherType == "JNL")
                                    dtVoucherName = m_Journal.GetJournalMasterDtl(drTransactInfo["RowID"].ToString());
                                else if (VoucherType == "CNTR")
                                    dtVoucherName = m_Contra.GetContraMasterInfo(Convert.ToInt32(drTransactInfo["RowID"]));

                                DataTable dtLedgerName = Ledger.GetLedgerInfo(Convert.ToInt32(drTransactInfo["LedgerID"]), LangMgr.DefaultLanguage);
                                DataRow drLedgerName = dtLedgerName.Rows[0];
                                DataRow drVoucherName = dtVoucherName.Rows[0];
                                //posting the Debit and Credit amount of particular ledger in frontend just reversal of Database's value eg. if ledger has Dr amount in database then post it in Credit side in frontend
                              
                                //While calculating Balance we just apply cumulative addition
                                //we will add the the deduction value of frontend dr and cr(Debit-Credit) balance to Previous balance
                                //Remember fronend Dr and Cr value is just reversal of Database's value
                                Balance += (Convert.ToDouble(drTransactInfo["Credit_Amount"]) - Convert.ToDouble(drTransactInfo["Debit_Amount"]));
                                //Balance += (Convert.ToDouble(drTransactInfo["Debit_Amount"]) - Convert.ToDouble(drTransactInfo["Credit_Amount"]));
                                TotalDrAmt+=Convert.ToDouble(drTransactInfo["Credit_Amount"]);
                                TotalCrAmt+= Convert.ToDouble(drTransactInfo["Debit_Amount"]);
                                //For total Debit and Credit Balance

                                if (VoucherType == "JNL")//Incase  of Journal Voucher type
                                {
                                    if (Balance >= 0)//When Balance is greater than 0 then add DR in the end of balance
                                    {
                                        WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedgerName["LedName"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(drTransactInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", null, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                        //WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedgerName["LedName"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), drTransactInfo["Credit_Amount"].ToString(), "", "", null, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));                                       
                                    }

                                    else if (Balance < 0)//when balance is less than 0 it represt the Credit amount
                                    {
                                        WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), "LedgerNameHere", Convert.ToDecimal(drTransactInfo["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", null, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));                                         
                                        //WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), "LedgerNameHere", drTransactInfo["Debit_Amount"].ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", null, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));                                         
                                    }
                                }
                                else if (VoucherType == "CNTR")//Incase of Contra Voucher type 
                                {
                                    if (Balance > 0)
                                    {

                                        WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedgerName["LedName"].ToString(), Convert.ToDecimal(drTransactInfo["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(drTransactInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", null, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                       // WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedgerName["LedName"].ToString(), drTransactInfo["Debit_Amount"].ToString(),drTransactInfo["Credit_Amount"].ToString(), "", "", null, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                        
                                    }

                                    else if (Balance < 0)
                                    {
                                        WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedgerName["LedName"].ToString(), Convert.ToDecimal(drTransactInfo["Debit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), Convert.ToDecimal(drTransactInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", ":", null, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));
                                        //WriteTransaction(Sno, Date.DBToSystem(drTransactInfo["TransactDate"].ToString()), drLedgerName["LedName"].ToString(), drTransactInfo["Debit_Amount"].ToString(), Convert.ToDecimal(drTransactInfo["Credit_Amount"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", ":", null, VoucherType, Convert.ToInt32(drTransactInfo["RowID"]));

                                    }
                                }

       

                                Sno++;
                            }

                            #endregion
                            break;
                           
                    }
                }

                #endregion
            }

            //fOR Journal and Contra voucherType
            if (VouType == "JNL" || VouType=="CNTR")
            {
                //TotalDrAmt += DrOpBal;
                //TotalCrAmt += CrOpBal;
            
            }
            #region BLOCK FOR TOTAL AMOUNT CALCULATION
            
            if(DrOpBal>0)
            {
                TotalDrAmt += DrOpBal;
            }
            else if(CrOpBal>0)
            {
                TotalCrAmt += CrOpBal;
            }          
            #endregion

            //for closing balance
            txtBankStatementBalance.Text= txtBankStatementBalance.Text == "" ? "0" : txtBankStatementBalance.Text;
            lblBankStatementBalance.Text = (Convert.ToDecimal(Balance)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            //if (Convert.ToDecimal(txtBankStatementBalance.Text) > Convert.ToDecimal(Balance))
            //    lblBankReconcilationDiff.Text = (Convert.ToDecimal(txtBankStatementBalance.Text) - Convert.ToDecimal(Balance)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //else
                lblBankReconcilationDiff.Text = (  Convert.ToDecimal(Balance) - Convert.ToDecimal(txtBankStatementBalance.Text)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //closing balance...
            if(Balance>0)
            {
               
               // WriteTransaction(0, "***Closing Balance***", "", "", "", "", "", Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
            }
            else if(Balance<=0)
            {
               // WriteTransaction(0, "***Closing Balance***", "", "", "", "", "", (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);
            }
          
        }  

           /// <summary>
        /// Gets the selected root accounting class ID
        /// </summary>
        /// <returns></returns>
        private int GetRootAccClassID()
        {
            if (AccClassID.Count > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(AccClassID[0]));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);
            }
            return 1;//The default root class ID
        }

        private void grdBankReconciliation_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;

            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false);
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    LoadSeriesNo();
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    break;

            }
        }
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            //btnNew.Enabled = New;
           // btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            //btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
        }

        private void EnableControls(bool Enable)
        {
            txtVchNo.Enabled = txtDate.Enabled = cboSeriesName.Enabled = Enable;
        }
        private void Text_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
        }

        private void cboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    txtVchNo.Enabled = true;
                    SeriesID = (ListItem)cboSeriesName.SelectedItem;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
                    if (NumberingType == "AUTOMATIC")
                    {
                        object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
                        if (m_vounum == null)
                        {
                            MessageBox.Show("Your voucher numbers are totally finished!");
                            return;
                        }
                        txtVchNo.Text = m_vounum.ToString();
                        txtVchNo.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }         
            //try
            //{
            //    //Do not check if the form is loading or data is loading due to some navigation key pressed
            //    if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
            //    {
            //        txtVchNo.Enabled = true;
            //        SeriesID = (ListItem)cboSeriesName.SelectedItem;
            //        string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));//If NumberingType is blank it means NumberingType is "Main" because tblVouNumConfig doesnot contain the "Main"
            //        if (NumberingType == "AUTOMATIC")
            //        {
            //            object m_vounum = m_VouConfig.GenerateVouNumType(Convert.ToInt32(SeriesID.ID));
            //            if (m_vounum == null)
            //            {
            //                MessageBox.Show("Your voucher numbers are totally finished!");
            //                return;
            //            }
            //            txtVchNo.Text = m_vounum.ToString();
            //            txtVchNo.Enabled = false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //} 
        }
        private void LoadSeriesNo()
        {
            //DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("JNL");
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("BRECON");
            cboSeriesName.Items.Clear();
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                cboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            cboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
            cboSeriesName.SelectedIndex = 0;
        }

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {

        }
       
        private void Account_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                frmLOVLedger frm = new frmLOVLedger(this);
                frm.ShowDialog();
                SendKeys.Send("{Tab}");
               // grdAdjustment[1, 4].Value = lblBankReconcilationDiff.Text;
            }
        }

        private void Account_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
        }
        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
            frmLOVLedger frm = new frmLOVLedger(this, e);
            frm.ShowDialog();
        }
        public void AddLedger(string LedgerName, int LedgerID, string CurrentBalance, bool IsSelected)
        {
            if (IsSelected)
            {
                ctx.Text = LedgerName;
                CurrAccLedgerID = LedgerID;
                CurrBal = CurrentBalance;
            }
            hasChanged = true;
        }
        private void FillGridRowExceptLedgerID(int RowPosition, int LdrID, string CurrBal)
        {
            decimal temporary = 0;
            decimal TempAmount = 0;
            string CurrentLedgerBalance = "";
            string[] CurrentBalance = new string[2];
            if (CurrBal == "")
            {
                CurrentLedgerBalance = grdAdjustment[Convert.ToInt32(RowPosition), 8].ToString();
                CurrentBalance = grdAdjustment[Convert.ToInt32(RowPosition), 8].ToString().Split('(');
            }
            else
            {
                CurrentLedgerBalance = CurrBal.ToString();
                CurrentBalance = CurrBal.ToString().Split('(');
            }
            string DrCr = grdAdjustment[Convert.ToInt32(RowPosition), 3].Value.ToString();

            try
            {
                TempAmount = Convert.ToDecimal(grdAdjustment[Convert.ToInt32(RowPosition), 4].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                if (DrCr == "Debit")
                {
                    grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
                if (DrCr == "Credit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0]) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                    if (temporary > 0)
                    {
                        grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                if (DrCr == "Credit")
                {
                    grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0].ToString()) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                    if (temporary > 0)
                    {
                        grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                if (DrCr == "Credit")
                {
                    grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    grdAdjustment[Convert.ToInt32(RowPosition), 5].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
            }
            else
            {
                grdAdjustment[RowPosition, 5].Value = CurrBal;
            }
        }

        /// <summary>
        /// Sums up all the debit and credit amounts on the voucher grid
        /// </summary>
        private void CalculateDrCr()
        {
            try
            {
                double dr, cr;
                dr = cr = 0;
                for (int i = 1; i < grdAdjustment.RowsCount; i++)
                {
                    //Check for empty Amount
                    object objAmount = grdAdjustment[i, 4].Value;
                    if (objAmount == null)
                        continue;
                    ////Check if it is the (NEW) row. If it is, it must be the last row.
                    if ((i == grdAdjustment.Rows.Count) || (grdAdjustment[i, 4].Value.ToString().ToUpper() == "(NEW)"))
                        continue;
                    double m_Amount = 0;

                    string m_Value = Convert.ToString(grdAdjustment[i, 4].Value);//Had to do this because it showed error when the cell was left blank
                    if (m_Value.Length == 0)
                        m_Amount = 0;
                    else
                    {
                        Double.TryParse(grdAdjustment[i, 4].Value.ToString(), out m_Amount);
                        try
                        {
                            Convert.ToDouble(grdAdjustment[i, 4].Value);
                        }
                        catch
                        {
                            MessageBox.Show("Please enter valid amount!");
                        }
                    }

                    if (grdAdjustment[i, 3].Value.ToString() == "Debit")
                        dr += m_Amount;
                    else if (grdAdjustment[i, 3].Value.ToString() == "Credit")
                        cr += m_Amount;
                }
                m_DebitAmount = dr;
                m_CreditAmount = cr;

                //lblDebitTotal.Text = m_DebitAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //lblCreditTotal.Text = m_CreditAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                //lblDifferenceAmount.Text = (m_DebitAmount - m_CreditAmount).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            }
            catch (Exception ex)
            {
                //lblDebitTotal.Text = "-";
                //lblCreditTotal.Text = "-";
                //lblDifferenceAmount.Text = "-";
                //Just ignore any errors
            }
        }

        private void grpSelect_Enter(object sender, EventArgs e)
        {

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
            IsChequeDateButton = false;
            //frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            //frm.ShowDialog();
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            IsFromDate = true;//this variable is used as flag to notify which date is selected to change the date converter...coz same funtion is used to change the date  
            IsToDate = false;
            IsDate = false;
            DateTime dtDate = Date.ToDotNet(txtFromDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
            IsChequeDateButton = false;
            //frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtFromDate.Text));
            //frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IsFromDate = false;
            IsToDate = true;
            IsDate = false;
            DateTime dtDate = Date.ToDotNet(txtToDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
            IsChequeDateButton = false;
            //frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtToDate.Text));
            //frm.ShowDialog();
        }

        public void DateConvert(DateTime DotNetDate)
        {
            if (IsFromDate)//If form date is selected
            {
                txtFromDate.Text = Date.ToSystem(DotNetDate);
            }
            else if (IsToDate)//IF TO date is selected
            {
                txtToDate.Text = Date.ToSystem(DotNetDate);
            }
            else
            {
                txtDate.Text = Date.ToSystem(DotNetDate);
            }

            //if (!IsChequeDateButton)
            //    txtDate.Text = Date.ToSystem(DotNetDate);
            //if (IsChequeDateButton)
            //    ctx1.Value = Date.ToSystem(DotNetDate);
        }

        private void cboSeriesName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDate.Focus();
            }
        }

        private void btnSelectAccClass_Click_1(object sender, EventArgs e)
        {
            AccountLedgerSettings ALS = new AccountLedgerSettings();
            try
            {
                ALS.AccClassID = AccClassID;

            }
            catch
            {
                //Ignore 
            }
            frmSelectAccClass frm = new frmSelectAccClass(this, ALS.AccClassID);

            if (!frm.IsDisposed)
                frm.ShowDialog();
        }
        public void AddSelectedAccClassID(DataTable AccClassID1)
        {
            try
            {
                AccClassID.Clear();
                ////If nothing is selected, simply send the root class id
                if (AccClassID1.Rows.Count == 0)
                {
                    AccClassID.Add("0");
                }

                else
                {
                    for (int i = 0; i < AccClassID1.Rows.Count; i++)
                    {
                        DataRow drAccClassID = AccClassID1.Rows[i];
                        AccClassID.Add(drAccClassID["AccClassID"].ToString());
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        private string ReadAllAccClassID(TransactSettings TS)
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in TS.AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            TS.AccClassID.AddRange(arrChildAccClassIDs);

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
                    foreach (string tag in TS.AccClassID)
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
            //MessageBox.Show(strXML);
            return strXML;
        }
        private void ShowLedgerTransaction(DataTable dt, int? LedgerID)
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
            DataTable dtLedgerInfo = OpeningBalance.GetAccClassOpeningBalance(GetRootAccClassID(), LedgerID);
            foreach (DataRow drLedgerInfo in dtLedgerInfo.Rows)
            {
                if (drLedgerInfo["OpenBalDrCr"].ToString() == "DEBIT")//IF ledger has Debit openig balance
                {
                    DrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                    Balance = DrOpBal;
                    //WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", DrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);                                                       
                }
                else if (drLedgerInfo["OpenBalDrCr"].ToString() == "CREDIT")//IF ledger has credit Opening balance
                {
                    CrOpBal += (Convert.ToDouble(drLedgerInfo["OpenBal"]));
                    Balance = (-CrOpBal);
                    //WriteTransaction(0, "Opening Balance B/F", Date.DBToSystem(drLedgerInfo["OpenBalDate"].ToString()), "", (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", CrOpBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);               
                }
            }
            #endregion

            double DrBal, CrBal;
            DrBal = CrBal = 0;
            int Count = 1;
            int Sno = 1;
            foreach (DataRow dr in dt.Rows)
            {

                DrBal = Convert.ToDouble(dr["Debit"]);
                CrBal = Convert.ToDouble(dr["Credit"]);
                TotalDrAmt += Convert.ToDouble(dr["Debit"]);
                TotalCrAmt += Convert.ToDouble(dr["Credit"]);
                //Balance += (DrBal - CrBal);
                Balance += (CrBal - DrBal);
                if (rbtnAll.Checked)
                {
                   
                    if (Balance > 0)//IT represets Debit
                    {
                        //(sno,transactdate,ledger,drbal,crbal,chequenumber,chequebank,chequedate,vouchertyee,rowid)
                        //WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
                        //WriteTransaction(Sno, Date.DBToSystem(dr["TransactDate"].ToString()), dr["LedName"].ToString(), Convert.ToDouble(dr["Debit_Amount"]).ToString(), (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["ChequeNumber"], dr["ChequeBank"], dr["ChequeDate"], dr["VoucherType"], Convert.ToInt32(dr["RowID"]));                                               
                        // WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), (0).ToString(), dr[8].ToString(), dr["ChequeBank"].ToString(), Convert.ToDateTime(Date.DBToSystem(dr[9].ToString())), dr[5].ToString(), Convert.ToInt32(dr[7]));                                               
                        //string transactdatetest = Date.DBToSystem(dr[0].ToString());
                        //string chequedatetest = dr[9].ToString();
                       // DateTime? dt = Convert.ToDateTime(date ?? null);
                        //DateTime chequedatetest1 =Convert.ToDateTime(Date.DBToSystem((dr[9].ToString())));
                        //yyyy-MM-dd

                        string cdate = dr[9].ToString();
                        if (dr[9].ToString() == " " || dr[9].ToString() == null || dr[9].ToString() == "0" || dr[9].ToString() == "")
                        {
                            //string date = null;
                            //DateTime? dtime = Convert.ToDateTime(date ?? null);

                            WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), null, dr[5].ToString(), Convert.ToInt32(dr[7]));
                        }
                        else
                        {
                            //DateConvert(Convert.ToDateTime(dr[9].ToString()));
                            changedate = Date.DBToSystem(dr[9].ToString());
                            //DateTime changedate1 = Convert.ToDateTime(dr[9].ToString());
                            //changedate = Convert.ToString(changedate1);
                            DateTime dateT = Convert.ToDateTime(changedate);
                            //WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), Convert.ToDateTime(dr[9].ToString()), dr[5].ToString(), Convert.ToInt32(dr[7]));
                            WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), Convert.ToDateTime(dr[9].ToString()), dr[5].ToString(), Convert.ToInt32(dr[7]));
                            //WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), Date.DBToSystem(dr[9].ToString()), dr[5].ToString(), Convert.ToInt32(dr[7]));
                        }
                    }
                    else//It represents Credit
                    {
                        if (dr[9].ToString() == " " || dr[9].ToString() == null || dr[9].ToString() == "0" || dr[9].ToString() == "")
                        {
                            string date = null;
                            DateTime? dtime = Convert.ToDateTime(date ?? null);
                            WriteTransaction(Sno,  Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), null, dr[5].ToString(), Convert.ToInt32(dr[7]));
                        }
                        else
                        {
                            // WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
                            WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), Convert.ToDateTime(Date.DBToSystem(dr[9].ToString())), dr[5].ToString(), Convert.ToInt32(dr[7]));
                        }
                    }
                    Count++;
                    Sno++;
                }
                if (rbtnReceipt.Checked)
                {
                    if (dr[5].ToString()== "BANK_RCPT")
                    {
                      if (Balance > 0)//IT represets Debit
                        {
                            if (dr[9].ToString() == " " || dr[9].ToString() == null || dr[9].ToString() == "0" || dr[9].ToString() == "")
                            {
                                //string date = null;
                                //DateTime? dtime = Convert.ToDateTime(date ?? null);
                                WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), null, dr[5].ToString(), Convert.ToInt32(dr[7]));
                            }
                            else
                            {
                                WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), Convert.ToDateTime(Date.DBToSystem(dr[9].ToString())), dr[5].ToString(), Convert.ToInt32(dr[7]));
                            }
                        //WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), Convert.ToDateTime(Date.DBToSystem(dr[9].ToString())), dr[5].ToString(), Convert.ToInt32(dr[7]));
                        }
                        else//It represents Credit
                        {
                        // WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
                            if (dr[9].ToString() == " " || dr[9].ToString() == null || dr[9].ToString() == "0" || dr[9].ToString() == "")
                            {
                                //string date = null;
                                //DateTime? dtime = Convert.ToDateTime(date ?? null);
                                WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), null, dr[5].ToString(), Convert.ToInt32(dr[7]));
                            }
                            else
                            {
                                WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), Convert.ToDateTime(Date.DBToSystem(dr[9].ToString())), dr[5].ToString(), Convert.ToInt32(dr[7]));
                            }
                        }
                        Count++;
                        Sno++;
                    }
                  
                }

                if (rbtnPayment.Checked)
                {
                    if (dr[5].ToString() == "BANK_PMNT")
                    {
                        if (Balance > 0)//IT represets Debit
                        {
                            if (dr[9].ToString() == " " || dr[9].ToString() == null || dr[9].ToString() == "0" || dr[9].ToString() == "")
                            {
                                //string date = null;
                                //DateTime? dtime = Convert.ToDateTime(date ?? null);
                                WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), null, dr[5].ToString(), Convert.ToInt32(dr[7]));
                            }
                            else
                            {
                                WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), Convert.ToDateTime(Date.DBToSystem(dr[9].ToString())), dr[5].ToString(), Convert.ToInt32(dr[7]));
                            }
                        }
                        else//It represents Credit
                        {
                            if (dr[9].ToString() == " " || dr[9].ToString() == null || dr[9].ToString() == "0" || dr[9].ToString() == "")
                            {
                                //string date = null;
                                //DateTime? dtime = Convert.ToDateTime(date ?? null);
                                WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), null, dr[5].ToString(), Convert.ToInt32(dr[7]));
                            }
                            else
                            {
                                // WriteTransaction(Count, dr["Account"].ToString(), Date.DBToSystem(dr["LedgerDate"].ToString()), dr["VoucherNumber"].ToString(), DrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), CrBal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["VoucherType"].ToString(), (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "LEDGER", dr["RowID"].ToString(), IsCrystalReport);
                                WriteTransaction(Sno, Date.DBToSystem(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[3]).ToString(), dr[4].ToString(), dr[8].ToString(), (0).ToString(), Convert.ToDateTime(Date.DBToSystem(dr[9].ToString())), dr[5].ToString(), Convert.ToInt32(dr[7]));
                            }
                        }
                        Count++;
                        Sno++;
                    }

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
               // WriteTransaction(0, "TOTAL AMOUNT", "", "", (TotalDrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), (TotalCrAmt).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), "", "", "GROUP", "", IsCrystalReport);


            #endregion
            txtBankStatementBalance.Text = txtBankStatementBalance.Text == "" ? "0" : txtBankStatementBalance.Text;
            lblBankStatementBalance.Text = (Convert.ToDecimal(Balance)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            //if (Convert.ToDecimal(txtBankStatementBalance.Text) > Convert.ToDecimal(Balance))
            //    lblBankReconcilationDiff.Text = (Convert.ToDecimal(txtBankStatementBalance.Text) - Convert.ToDecimal(Balance)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //else
            lblBankReconcilationDiff.Text = (Convert.ToDecimal(Balance) - Convert.ToDecimal(txtBankStatementBalance.Text)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            #region BLOCK FOR CLOSING BALANCE
            if (Balance > 0)
            {
                //WriteTransaction(0, "Closing Balance", "", "", "", "", "", Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Dr", "GROUP", "", IsCrystalReport);
            }
            else if (Balance <= 0)
            {
               // WriteTransaction(0, "Closing Balance", "", "", "", "", "", (-Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + "Cr", "GROUP", "", IsCrystalReport);
            }
            #endregion

        }
        private void ClearVoucher()
        {
            clearBankreconciliation();
            ShowAccClassInTreeView(treeAccClass, null, 0);
            grdAdjustment.Redim(2, 7);
            AddGridHeader();
            AddRowBankConciliation1(1);
            
        }
        private void clearBankreconciliation()
        {
            txtVchNo.Clear(); //actually generate a new voucher no.
            txtDate.Text = Date.ToSystem(Date.GetServerDate());
            grdAdjustment.Rows.Clear();
            cboSeriesName.Text = string.Empty;
           
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem liLedgerID = new ListItem();
            liLedgerID = (ListItem)cmboBanks.SelectedItem;
            int LedgerID = liLedgerID.ID;
            string ledgername = liLedgerID.Value;
            label15.Text = ledgername;
            label16.Text = Convert.ToString(LedgerID);
            rbtnreceived.Checked = true;
            //grdAdjustment[1, 4].Value = lblBankReconcilationDiff.Text;
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //label15.Text=
        }

        private string ReadAllBankReconciliationEntry()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            SeriesID = (ListItem)cboSeriesName.SelectedItem;
           // liProjectID = (ListItem)cboProjectName.SelectedItem;
            //liBankID = (ListItem)comboBankAccount.SelectedItem;
            int libankid = Convert.ToInt32(label16.Text);

            //validate grid entry
            if (!ValidateGrid())
                return string.Empty;

            tw.WriteStartDocument();
            #region  Journal
            tw.WriteStartElement("BANKRECONCILIATION");
            {
                ///For Bank Receipt Master Section                   
                int SID = System.Convert.ToInt32(SeriesID.ID);
                int BankID = libankid;
                string Voucher_No = System.Convert.ToString(txtVchNo.Text);
                DateTime BankReconciliation_Date = Date.ToDotNet(txtDate.Text);
                tw.WriteStartElement("BANKRECECONCILIATIONMASTER");
                tw.WriteElementString("SeriesID", SID.ToString());
                tw.WriteElementString("LedgerID", BankID.ToString());
                tw.WriteElementString("Voucher_No", Voucher_No.ToString());
                tw.WriteElementString("BankReconciliation_Date", Date.ToDB(BankReconciliation_Date));
                tw.WriteEndElement();
                ///For journal Detail Section             
                int BankReconciliationID = 0;
                int LedgerID = 0;
                Decimal Amount = 0;
                string RemarksDetail = "";
                tw.WriteStartElement("BANKRECECONCILIATIONDETAIL");
                for (int i = 0; i < OnlyReqdDetailRows; i++)
                {
                    LedgerID = System.Convert.ToInt32(grdAdjustment[i + 1, 6].Value);
                    Amount = System.Convert.ToDecimal(grdAdjustment[i + 1, 4].Value);
                    RemarksDetail = System.Convert.ToString(grdAdjustment[i + 1, 5].Value);
                    tw.WriteStartElement("DETAIL");
                    tw.WriteElementString("BankReconciliationID", BankReconciliationID.ToString());
                    tw.WriteElementString("LedgerID", LedgerID.ToString());
                    tw.WriteElementString("Amount", Amount.ToString());
                    tw.WriteElementString("Remarks", RemarksDetail.ToString());
                    tw.WriteEndElement();
                }
                tw.WriteEndElement();
                //Write Checked Accounting class ID
                try
                {
                    ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
                    tw.WriteStartElement("ACCCLASSIDS");
                    foreach (string tag in arrNode)
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
        private bool ValidateGrid()
        {
            int[] LdrID = new int[20];
            decimal[] Amt = new decimal[20];

            //Validate input grid record
            for (int i = 0; i < grdAdjustment.Rows.Count - 2; i++)
            {
                try
                {
                    //if ledger ID repeats then message it
                    // if LedgerID is not present in between them
                    int tempValue = 0;
                    decimal tempDecValue = 0;
                    try
                    {
                        tempValue = System.Convert.ToInt32(grdAdjustment[i + 1, 6].Value);
                    }
                    catch (Exception ex)
                    {
                        tempValue = 0;
                    }
                    try
                    {
                        tempDecValue = System.Convert.ToDecimal(grdAdjustment[i + 1, 4].Value);
                    }
                    catch (Exception ex)
                    {
                        tempDecValue = 0;
                    }

                    if (tempValue != 0 && tempDecValue == 0)
                    {
                        return false;
                    }

                    if (LdrID.Contains(tempValue))
                    {
                        if (i + 2 == grdAdjustment.Rows.Count && grdAdjustment[i + 1, 2].Value.ToString() == "(NEW)")
                        {
                            //Do Nothing
                        }
                        else
                            return false;
                    }
                    else
                        LdrID[i] = tempValue;

                    if (i + 2 == grdAdjustment.Rows.Count && grdAdjustment[i + 1, 2].Value.ToString() == "(NEW)")
                    {
                        //Donothing
                    }
                    else
                        Amt[i] = tempDecValue;

                   // liBankID = (ListItem)comboBankAccount.SelectedItem;
                    int libankid = Convert.ToInt32(label16.Text);
                    if (LdrID.Contains(libankid))
                    {
                        MessageBox.Show("Same bank transaction is invalid!");
                        return false;
                    }
                }

                catch
                {
                    return false;
                }
            }
            OnlyReqdDetailRows = LdrID.Count(i => i != 0);
            return true;
        }

        private void evtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            int ledgerID = 0;
            string CurrentBal = "";
            try
            {
                if (grdAdjustment[CurrRowPos, 2].Value.ToString() == "(NEW)" || grdAdjustment[CurrRowPos, 2].Value.ToString() == "")
                    return;
                try
                {
                    ledgerID = Convert.ToInt32(grdAdjustment[CurrRowPos, 6].Value);
                    //CurrentBal = grdAdjustment[CurrRowPos, 10].Value.ToString();
                }
                catch
                {
                    ledgerID = 0;
                }
                if (ledgerID != 0 && CurrAccLedgerID == 0)
                {
                    CurrAccLedgerID = ledgerID;
                    CurrBal = CurrentBal;
                }
            }
            catch
            {
                return;
            }
            FillAllGridRow(CurrRowPos, CurrAccLedgerID, CurrBal);
        }
        private void FillAllGridRow(int RowPosition, int LdrID, string CurrBalance)
        {
            decimal TempAmount = 0;
            string CurrentLedgerBalance = CurrBalance.ToString();
            string[] CurrentBalance = CurrBalance.ToString().Split('(');

            try
            {
                TempAmount = Convert.ToDecimal(grdAdjustment[Convert.ToInt32(RowPosition), 4].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            //if (CurrentLedgerBalance.Contains("Dr"))
            //{
            //    grdAdjustment[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
            //}

            //else if (CurrentLedgerBalance.Contains("Cr"))
            //{
            //    grdAdjustment[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            //}
            //else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            //{
            //    grdAdjustment[Convert.ToInt32(RowPosition), 4].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
            //}
            //else
            //{
            //    grdAdjustment[RowPosition, 5].Value = CurrBalance;
            //}
            grdAdjustment[RowPosition, 6].Value = LdrID;
            //grdAdjustment[RowPosition, 10].Value = CurrBalance;
            CurrAccLedgerID = 0;
            CurrBal = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BusinessLogic.Accounting.SaveState ss = new BusinessLogic.Accounting.SaveState();
            DataTable SaveState = new DataTable();
            SaveState.Columns.Add("Sn");
            SaveState.Columns.Add("Date");
            SaveState.Columns.Add("PartyName");
            SaveState.Columns.Add("DebitAmount");
            SaveState.Columns.Add("CreditAmount");
            SaveState.Columns.Add("ChequeNumber");
            SaveState.Columns.Add("ChequeBank");
            SaveState.Columns.Add("ChequeDate");
            SaveState.Columns.Add("Matched");
            SaveState.Columns.Add("VoucherType");
            SaveState.Columns.Add("Rowid");
            SaveState.Columns.Add("Userid");
            SaveState.Columns.Add("Ledgerid");
            ListItem liLedgerID = new ListItem();
            liLedgerID = (ListItem)cmboBanks.SelectedItem;
            int LedgerID = liLedgerID.ID;


            for (int i = 0; i < grdBankReconciliation.Rows.Count - 1; i++)
            {
                object myobj = grdBankReconciliation[i + 1, 8].Value;

                //if (grdBankReconciliation[i + 1, 8].Value.ToString() != "false")
                //{
                    SaveState.Rows.Add(grdBankReconciliation[i + 1, 0].Value, grdBankReconciliation[i + 1, 1].Value, grdBankReconciliation[i + 1, 2].Value, grdBankReconciliation[i + 1, 3].Value, grdBankReconciliation[i + 1, 4].Value, grdBankReconciliation[i + 1, 5].Value, grdBankReconciliation[i + 1, 6].Value, grdBankReconciliation[i + 1, 7].Value, grdBankReconciliation[i + 1, 8].Value.ToString(), grdBankReconciliation[i + 1, 10].Value, grdBankReconciliation[i + 1, 11].Value, User.CurrUserID, LedgerID);
                //}
                //else
                //{
                //    SaveState.Rows.Add(grdBankReconciliation[i + 1, 0].Value, grdBankReconciliation[i + 1, 1].Value, grdBankReconciliation[i + 1, 2].Value, grdBankReconciliation[i + 1, 3].Value, grdBankReconciliation[i + 1, 4].Value, grdBankReconciliation[i + 1, 5].Value, grdBankReconciliation[i + 1, 6].Value, grdBankReconciliation[i + 1, 7].Value, "N", grdBankReconciliation[i + 1, 10].Value, grdBankReconciliation[i + 1, 11].Value, User.CurrUser, LedgerID);
                //}
            }
            ss.BankReconciliationSavestate(SaveState, txtFromDate.Text, txtToDate.Text, LedgerID);
            MessageBox.Show("Data Saved Successfully");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ListItem liLedgerID = new ListItem();
            liLedgerID = (ListItem)cmboBanks.SelectedItem;
            int LedgerID = liLedgerID.ID;
            string ledgername = liLedgerID.Value;
            BusinessLogic.Accounting.SaveState ss = new BusinessLogic.Accounting.SaveState();
            string fromdate = txtFromDate.Text;
            string todate = txtToDate.Text;
            DataTable dtsavestate = ss.CheckLedgerState(LedgerID,fromdate,todate);
            DataRow dr1 = dtsavestate.Rows[0];
            txtFromDate.Text = txtToDate.Text = "";
            txtFromDate.Text = Date.DBToSystem(dr1[14].ToString());
            txtToDate.Text = Date.DBToSystem(dr1[15].ToString());
            for(int i=0; i<dtsavestate.Rows.Count;i++)
            {
                DataRow dr=dtsavestate.Rows[i];
                object myobj = dr["chequedate"].ToString();
                if (myobj == "")
                {
                    WriteTransaction1(Convert.ToInt32(dr["sn"].ToString()), Date.DBToSystem(dr["Date"].ToString()), dr["partyname"].ToString(), dr["debit_amount"].ToString(), dr["credit_amount"].ToString(), dr["chequenumber"].ToString(), dr["chequebank"].ToString(), null, dr["vouchertype"].ToString(), Convert.ToInt32(dr["rowid"].ToString()), dr["matched"].ToString());
                }
                else
                {
                    WriteTransaction1(Convert.ToInt32(dr["sn"].ToString()), Date.DBToSystem(dr["Date"].ToString()), dr["partyname"].ToString(), dr["debit_amount"].ToString(), dr["credit_amount"].ToString(), dr["chequenumber"].ToString(), dr["chequebank"].ToString(), null, dr["vouchertype"].ToString(), Convert.ToInt32(dr["rowid"].ToString()), dr["matched"].ToString());
                }
                //WriteTransaction(Convert.ToInt32(dr["sn"].ToString()),Date.DBToSystem(dr["Date"].ToString()), dr["partyname"].ToString(),dr["debit_amount"].ToString(),dr["credit_amount"].ToString(),dr["chequenumber"].ToString(),dr["chequebank"].ToString(),Convert.ToDateTime(dr["chequedate"].ToString()),dr["vouchertype"].ToString(),Convert.ToInt32(dr["rowid"].ToString()));
           
            } 
        }
        //public void DateConvert(DateTime DotNetDate)
        //{
        //    txtDate.Text = Date.ToSystem(DotNetDate);
        //}
        private void OptionalFields()
        {
            SeriesID = (ListItem)cboSeriesName.SelectedItem;
            DataTable dtadditionalfield = Sales.GetAdditionalFields(SeriesID.ID);
            drdtadditionalfield = dtadditionalfield.Rows[0];
            NumberOfFields = Convert.ToInt32(drdtadditionalfield["NumberOfField"].ToString());
            if (NumberOfFields > 0)
            {
                if (NumberOfFields == 1)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = false;
                    txtsecond.Visible = false;
                    lblthird.Visible = false;
                    txtthird.Visible = false;
                    lblfourth.Visible = false;
                    txtfourth.Visible = false;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                }
                else if (NumberOfFields == 2)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = false;
                    txtthird.Visible = false;
                    lblfourth.Visible = false;
                    txtfourth.Visible = false;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                }
                else if (NumberOfFields == 3)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = true;
                    txtthird.Visible = true;
                    lblfourth.Visible = false;
                    txtfourth.Visible = false;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    lblthird.Text = drdtadditionalfield["Field3"].ToString();

                }
                else if (NumberOfFields == 4)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = true;
                    txtthird.Visible = true;
                    lblfourth.Visible = true;
                    txtfourth.Visible = true;
                    lblfifth.Visible = false;
                    txtfifth.Visible = false;
                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();

                }
                else if (NumberOfFields == 5)
                {
                    lblfirst.Visible = true;
                    txtfirst.Visible = true;
                    lblsecond.Visible = true;
                    txtsecond.Visible = true;
                    lblthird.Visible = true;
                    txtthird.Visible = true;
                    lblfourth.Visible = true;
                    txtfourth.Visible = true;
                    lblfifth.Visible = true;
                    txtfifth.Visible = true;

                    lblfirst.Text = drdtadditionalfield["Field1"].ToString();
                    lblsecond.Text = drdtadditionalfield["Field2"].ToString();
                    lblthird.Text = drdtadditionalfield["Field3"].ToString();
                    lblfourth.Text = drdtadditionalfield["Field4"].ToString();
                    lblfifth.Text = drdtadditionalfield["Field5"].ToString();
                }
            }
            else
            {
                lblfirst.Visible = false;
                txtfirst.Visible = false;
                lblsecond.Visible = false;
                txtsecond.Visible = false;
                lblthird.Visible = false;
                txtthird.Visible = false;
                lblfourth.Visible = false;
                txtfourth.Visible = false;
                lblfifth.Visible = false;
                txtfifth.Visible = false;
            }

        }
    }
}
