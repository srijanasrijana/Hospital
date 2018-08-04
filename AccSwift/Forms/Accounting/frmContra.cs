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
using Inventory.CrystalReports;
using Inventory.DataSet;
using DateManager;
using Inventory.Forms;
using CrystalDecisions.Shared;
using Common;


namespace Inventory
{
    public partial class frmContra : Form, IfrmAddAccClass, IfrmDateConverter, ILOVLedger
    {
        private int NumberOfFields = 0;
        DataRow drdtadditionalfield;  
        //For Audit Log
        private string OldGrid = " ";
        private string NewGrid = " ";
        private bool isNew;
        private string Prefix = "";
        bool hasChanged = false;
        private int CurrAccLedgerID = 0;
        private string CurrBal = "";
        private int CurrRowPos = 0;
        private bool IsShortcutKey = false;
        ListItem liProjectID = new ListItem();
        private bool IsFieldChanged = false;
        private bool IsNegativeCash = false;
        private bool IsNegativeBank = false;
        private int ContraIDCopy = 0;
        //For Export Menu
        ContextMenu Menu_Export;
        private int prntDirect = 0;
        private string FileName = "";
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
            Del = 0, Code_No, Particular_Account_Head, Dr_Cr, Amount, Current_Balance, Cheque_No,  ChequeDate,Remarks, Ledger_ID, Current_Bal_Actual
        };
        private int loopCounter = 0;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        SourceGrid.CellContext ctx1 = new SourceGrid.CellContext();
        private bool IsChequeDateButton = false;
        ListItem SeriesID = new ListItem();
        ArrayList AccountClassID = new ArrayList();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        double m_DebitAmount = 0;//Holds the total debit amount on the Voucher
        double m_CreditAmount = 0;
        private DataSet.dsContra dsContra = new DataSet.dsContra();
        List<int> AccClassID = new List<int>();
        int m_ContraID = 0;
        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtChequeDateFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtcboDrCrSelectedIndexChanged = new SourceGrid.Cells.Controllers.CustomEvents();
        Contra m_Contra = new Contra();
       
        public frmContra()
        {
            InitializeComponent();
        }

        public frmContra(int ContraID)
        {
            InitializeComponent();
            this.m_ContraID = ContraID;
        }

        public void DateConvert(DateTime DotNetDate)
        {
            //txtDate.Text = Date.ToSystem(DotNetDate);
             if (!IsChequeDateButton)
                 txtDate.Text = Date.ToSystem(DotNetDate);
             if (IsChequeDateButton)
                 ctx1.Value = Date.ToSystem(DotNetDate);
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

        private int ChildCount(int AccClassID)
        {
            try
            {
                int m_RecCount = (int)User.GetChildTable(AccClassID).Rows.Count;
                return m_RecCount;
            }
            catch
            {
                throw;               
            }
        }

        //Fill the cboUnder List box with Project Head
        private void ListProject(ComboBox ComboBoxControl)
        {
            #region Language Management
            ComboBoxControl.Font = LangMgr.GetFont();
            string LangField = "English";
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

            ComboBoxControl.Items.Clear();
            DataTable dt = Project.GetProjectTable(-1);
            ComboBoxControl.Items.Add(new ListItem((0), "None"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], dr[LangField].ToString()));
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }

        private void frmContra_Load(object sender, EventArgs e)
        {
            chkDoNotClose.Checked = true;
            ChangeState(EntryMode.NEW);          
            txtContraID.Visible = false;
            //ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            ShowAccClassInTreeView(treeAccClass, null, 0);
            m_mode = EntryMode.NEW;
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.
            //For Loading The Optional Fields
            OptionalFields();
            try
            {
                #region Customevents mainly for saving purpose
                cboSeriesName.SelectedIndexChanged += new EventHandler(Text_Change);
                txtVchNo.TextChanged += new EventHandler(Text_Change);
                txtDate.TextChanged += new EventHandler(Text_Change);
                btnDate.Click += new EventHandler(Text_Change);
                cboProjectName.SelectedIndexChanged += new EventHandler(Text_Change);
                txtRemarks.TextChanged += new EventHandler(Text_Change);

                //Event trigerred when delete button is clicked
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                evtAmountFocusLost.FocusLeft += new EventHandler(Amount_Focus_Lost);                
                evtAccountFocusLost.FocusLeft += new EventHandler(evtAccountFocusLost_FocusLeft);

                evtChequeDateFocusLost.Click += new EventHandler(ChequeDate_Click);
                evtChequeDateFocusLost.Click += new EventHandler(Text_Change);

                evtcboDrCrSelectedIndexChanged.ValueChanged += new EventHandler(evtDrCr_Changed);
                #endregion                                

                grdContra.Redim(2,11);
                btnRowDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //Prepare the header part for grid
                AddGridHeader();
                //AddRowContra(1);
                AddRowContra1(1);

                //Replace this block with navigation function later after completion of journal code
                #region BLOCK FOR DISPLAYING THE VALUES IN CORRESPONDING FIELDS ACCORDING TO MASTERID
              
                if (m_ContraID > 0)
                {
                    //Show the values in fields
                    try
                    {
                        ChangeState(EntryMode.NORMAL);
                        int vouchID = 0;
                        try
                        {
                            vouchID = m_ContraID;
                        }
                        catch (Exception)
                        {
                            vouchID = 999999999; //set to maximum so that it automatically gets the highest
                        }
                        Contra m_Contra = new Contra();
                        //Getting SeriesID from MasterID

                        int SeriesIDD = m_Contra.GetSeriesIDFromMasterID(vouchID);

                        DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesIDD);
                        if (dt.Rows.Count <= 0)
                        {
                            Global.Msg("There is no any SeriesName in this Contra");
                            cboSeriesName.Text = "";
                        }
                        else
                        {
                            DataRow dr = dt.Rows[0];
                            cboSeriesName.Text = dr["EngName"].ToString();
                        }
                        DataTable dtContraMasterInfo= m_Contra.GetContraMasterInfo(vouchID);

                        if (dtContraMasterInfo.Rows.Count <= 0)//this is the first record
                        {
                            Global.Msg("No more records found!");
                            return;
                        }
                        DataRow drContraMasterInfo = dtContraMasterInfo.Rows[0];
                        txtVchNo.Text = drContraMasterInfo["Voucher_No"].ToString();
                        txtDate.Text = Date.DBToSystem(drContraMasterInfo["Contra_Date"].ToString());
                        txtRemarks.Text = drContraMasterInfo["Remarks"].ToString();
                        txtContraID.Text = drContraMasterInfo["ContraID"].ToString();

                        dsContra.Tables["tblContraMaster"].Rows.Add(cboSeriesName.Text, txtVchNo.Text, Date.DBToSystem(drContraMasterInfo["Contra_Date"].ToString()),txtRemarks.Text);
                        //For Additional Fields
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

                                txtfirst.Text = drContraMasterInfo["Field1"].ToString();
                                txtsecond.Text = drContraMasterInfo["Field2"].ToString();
                                txtthird.Text = drContraMasterInfo["Field3"].ToString();
                                txtfourth.Text = drContraMasterInfo["Field4"].ToString();
                                txtfifth.Text = drContraMasterInfo["Field5"].ToString();
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

                                txtfirst.Text = drContraMasterInfo["Field1"].ToString();
                                txtsecond.Text = drContraMasterInfo["Field2"].ToString();
                                txtthird.Text = drContraMasterInfo["Field3"].ToString();
                                txtfourth.Text = drContraMasterInfo["Field4"].ToString();
                                txtfifth.Text = drContraMasterInfo["Field5"].ToString();

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

                                txtfirst.Text = drContraMasterInfo["Field1"].ToString();
                                txtsecond.Text = drContraMasterInfo["Field2"].ToString();
                                txtthird.Text = drContraMasterInfo["Field3"].ToString();
                                txtfourth.Text = drContraMasterInfo["Field4"].ToString();
                                txtfifth.Text = drContraMasterInfo["Field5"].ToString();

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

                                txtfirst.Text = drContraMasterInfo["Field1"].ToString();
                                txtsecond.Text = drContraMasterInfo["Field2"].ToString();
                                txtthird.Text = drContraMasterInfo["Field3"].ToString();
                                txtfourth.Text = drContraMasterInfo["Field4"].ToString();
                                txtfifth.Text = drContraMasterInfo["Field5"].ToString();

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

                                txtfirst.Text = drContraMasterInfo["Field1"].ToString();
                                txtsecond.Text = drContraMasterInfo["Field2"].ToString();
                                txtthird.Text = drContraMasterInfo["Field3"].ToString();
                                txtfourth.Text = drContraMasterInfo["Field4"].ToString();
                                txtfifth.Text = drContraMasterInfo["Field5"].ToString();
                            }


                        }
                       
                        DataTable dtContraDetail = m_Contra.GetContraDetail(Convert.ToInt32(drContraMasterInfo["ContraID"]));
                        for (int i = 1; i <= dtContraDetail.Rows.Count; i++)
                        {
                            DataRow drDetail = dtContraDetail.Rows[i - 1];
                            grdContra[i, (int)GridColumn.Code_No].Value = i.ToString();
                            grdContra[i, (int)GridColumn.Particular_Account_Head].Value = drDetail["LedgerName"].ToString();
                            grdContra[i, (int)GridColumn.Dr_Cr].Value = drDetail["DrCr"].ToString();
                            grdContra[i, (int)GridColumn.Amount].Value = drDetail["Amount"].ToString();
                            grdContra[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                           // AddRowContra(grdContra.RowsCount);
                            AddRowContra1(grdContra.RowsCount);
                            dsContra.Tables["tblContraDetails"].Rows.Add(drDetail["LedgerName"].ToString(), drDetail["DrCr"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());

                        }
                        CalculateDrCr();
                    }                  
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                //grid1.AutoSizeCells();
                #endregion

            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        /// <summary>
        /// Check whether user wants to close the form without saving. Returns true if he wants to navigate away without saving.
        /// </summary>
        /// <returns></returns>
        private bool ContinueWithoutSaving()
        {
            if (IsFieldChanged)
            {
                if (MessageBox.Show("Changes has not been saved yet. Are you sure you want to continue without saving?", "Exiting...", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    return true;
                }
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// This is the common text change event for all controls just to track if any changes has been made. This will help to show a save dialogue box while closing the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Text_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
        }
        private void evtDrCr_Changed(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row;
            FillGridRowExceptLedgerID(CurrRowPos, CurrAccLedgerID, CurrBal);
        }
        private void FillGridRowExceptLedgerID(int RowPosition, int LdrID, string CurrBal)
        {
            decimal temporary = 0;
            decimal TempAmount = 0;
            string CurrentLedgerBalance = "";
            string[] CurrentBalance = new string[2];
            if (CurrBal == "")
            {
                CurrentLedgerBalance = grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Bal_Actual].ToString();
                CurrentBalance = grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Bal_Actual].ToString().Split('(');
            }
            else
            {
                CurrentLedgerBalance = CurrBal.ToString();
                CurrentBalance = CurrBal.ToString().Split('(');
            }
            string DrCr = grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Dr_Cr].Value.ToString();

            try
            {
                TempAmount = Convert.ToDecimal(grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Amount].Value);
            }
            catch
            {
                TempAmount = 0;
            }

            if (CurrentLedgerBalance.Contains("Dr"))
            {
                if (DrCr == "Debit")
                {
                    grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
                if (DrCr == "Credit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0]) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                    if (temporary > 0)
                    {
                        grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance.Contains("Cr"))
            {
                if (DrCr == "Credit")
                {
                    grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(CurrentBalance[0]) + Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    temporary = (Convert.ToDecimal(CurrentBalance[0].ToString()) + (-1) * Convert.ToDecimal(TempAmount));
                    if (temporary == 0)
                    {
                        grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    if (temporary < 0)
                    {
                        grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = ((-1) * temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                    }
                    if (temporary > 0)
                    {
                        grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (temporary).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                    }
                }
            }

            else if (CurrentLedgerBalance == (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)))
            {
                if (DrCr == "Credit")
                {
                    grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Cr.)";
                }
                if (DrCr == "Debit")
                {
                    grdContra[Convert.ToInt32(RowPosition), (int)GridColumn.Current_Balance].Value = (Convert.ToDecimal(TempAmount)).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) + " (Dr.)";
                }
            }
            else
            {
                grdContra[RowPosition, (int)GridColumn.Current_Balance].Value = CurrBal;
            }
        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdContra.RowsCount - 2)
                grdContra.Rows.Remove(ctx.Position.Row);
            CalculateDrCr();
        }

        private void evtAccountFocusLost_FocusLeft(object sender, EventArgs e)
        {
            SourceGrid.CellContext ct = new SourceGrid.CellContext();
            try
            {
                ct = (SourceGrid.CellContext)sender;

            }
            catch (Exception ex)
            {
            }

            if (ct.DisplayText == "")
                return;

            int RowCount = grdContra.RowsCount;
            //Add a new row

            string CurRow = (string)grdContra[RowCount - 1, 2].Value;

            //Check whether the new row is already added
            if (CurRow != "(NEW)")
            {
                //AddRowContra(RowCount);
                AddRowContra1(RowCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowCount - 1);
            }
        }

        private void Account_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            //ComboBox cbo = (ComboBox)sender;
            //string ledgerName = cbo.Text.ToString();
            //if (cbo.Text.ToString().Contains('['))
            //{
            //    string[] split = ledgerName.Split(new Char[] { '[' });
            //    ledgerName = split[0].ToString();
            //}
            //else
            //{
            //    ledgerName = cbo.Text.ToString();
            //}

            ////Check if Ledger name is blank or (NEW)
            //if ((ledgerName.ToUpper() == "(NEW)") || String.IsNullOrEmpty(ledgerName))
            //    return;

            ////Check if the ledger exists
            //int LedgerID = Ledger.GetLedgerIdFromName(ledgerName, LangMgr.DefaultLanguage);
            //if (LedgerID <= 0)
            //{
            //    if (Global.MsgQuest("The specified ledger does not seem to exist. Do you want to create a new ledger with the given name?") == DialogResult.Yes)
            //    {
            //        frmAccountSetup frm = new frmAccountSetup(this, ledgerName);
            //        frm.ShowDialog();
            //    }
            //}
        }

        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdContra.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            string AccName = (string)(grdContra[RowCount - 1, 2].Value);
            //Check if the input value is correct
            if (grdContra[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == "" || grdContra[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value == null)
                grdContra[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            if (!Misc.IsNumeric(grdContra[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value))
            {
                Global.MsgError("Invalid Amount!");
                ctx.Value = "";
                return;
            }

            FillGridRowExceptLedgerID(CurRow, CurrAccLedgerID, CurrBal);
            double checkformat = Convert.ToDouble(grdContra[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value.ToString());
            string insertvalue = checkformat.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            grdContra[Convert.ToInt32(CurRow), (int)GridColumn.Amount].Value = insertvalue;
            //lblDebitTotal.Text = m_DebitAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            CalculateDrCr();
            if (AccName != "(NEW)")
            {
                AddRowContra1(RowCount);
            }
           // CalculateDrCr();
        }

        private void AddGridHeader()
        {
            grdContra[0, (int)GridColumn.Del] = new SourceGrid.Cells.ColumnHeader("Del");
            grdContra[0, (int)GridColumn.Del].Column.Width = 50;

            grdContra[0, (int)GridColumn.Code_No] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdContra[0, (int)GridColumn.Code_No].Column.Width = 60;

            grdContra[0,  (int)GridColumn.Particular_Account_Head] = new SourceGrid.Cells.ColumnHeader("Account");
            grdContra[0, (int)GridColumn.Particular_Account_Head].Column.Width = 200;

            grdContra[0, (int)GridColumn.Dr_Cr] = new SourceGrid.Cells.ColumnHeader("Dr/Cr");
            grdContra[0, (int)GridColumn.Dr_Cr].Column.Width = 50;

            grdContra[0, (int)GridColumn.Amount] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdContra[0, (int)GridColumn.Amount].Column.Width = 100;

            grdContra[0, (int)GridColumn.Current_Balance] = new SourceGrid.Cells.ColumnHeader("Current Balance");
            grdContra[0, (int)GridColumn.Current_Balance].Column.Width = 100;

            grdContra[0, (int)GridColumn.Cheque_No] = new SourceGrid.Cells.ColumnHeader("Cheque Number");
            grdContra[0, (int)GridColumn.Cheque_No].Column.Width = 150;

            grdContra[0, (int)GridColumn.ChequeDate] = new SourceGrid.Cells.ColumnHeader("Cheque Date");
            grdContra[0, (int)GridColumn.ChequeDate].Column.Width = 150;

            grdContra[0, (int)GridColumn.Remarks] = new SourceGrid.Cells.ColumnHeader("Remarks");
            grdContra[0, (int)GridColumn.Remarks].Column.Width = 100;

            grdContra[0, (int)GridColumn.Ledger_ID] = new SourceGrid.Cells.ColumnHeader("Ledger ID");
            grdContra[0, (int)GridColumn.Ledger_ID].Column.Visible = false;

            grdContra[0, (int)GridColumn.Current_Bal_Actual] = new SourceGrid.Cells.ColumnHeader("Current Balance");
            grdContra[0, (int)GridColumn.Current_Bal_Actual].Column.Visible = false;

            //grdContra[0, 5].Column.Visible = false;
            //grdContra[0, 7].Column.Visible = false;
            //grdContra[0, 8].Column.Visible = false;

        }

        /// <summary>
        /// Adds the row in the Contra field
        /// </summary>
        private void AddRowContra(int RowCount)
        {
            //Add a new row
            grdContra.Redim(Convert.ToInt32(RowCount + 1), grdContra.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;
            grdContra[i, (int)GridColumn.Del] = btnDelete;
            grdContra[i, (int)GridColumn.Del].AddController(evtDelete);

            grdContra[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell(i.ToString()); 
                   
            string[] Cash = Ledger.GetLedgerList(102);//Getting All Ledger corresponding to Cash Account
            SourceGrid.Cells.Editors.ComboBox cboAccount = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            string[] Bank = Ledger.GetLedgerList(7);
            string[] Cash_Bank = new string[Cash.Length + Bank.Length];
            Cash.CopyTo(Cash_Bank, 0);
            Bank.CopyTo(Cash_Bank, Cash.Length);
            cboAccount.StandardValues = Cash_Bank;           
            cboAccount.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboAccount.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboAccount.Control.LostFocus += new EventHandler(evtAccountFocusLost_FocusLeft);
            cboAccount.Control.LostFocus += new EventHandler(Account_Leave);
            cboAccount.EditableMode = SourceGrid.EditableMode.Focus;

            grdContra[i, (int)GridColumn.Particular_Account_Head] = new SourceGrid.Cells.Cell("", cboAccount);
            grdContra[i, (int)GridColumn.Particular_Account_Head].AddController(evtAccountFocusLost);
            grdContra[i, (int)GridColumn.Particular_Account_Head].Value = "(NEW)";


            SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
            cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;

            cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;

            string strDrCr = "Debit";

            if (grdContra[i - 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
                strDrCr = "Credit";

            grdContra[i, (int)GridColumn.Dr_Cr] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;

            grdContra[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell("", txtAmount);
            grdContra[i, (int)GridColumn.Amount].AddController(evtAmountFocusLost);
            grdContra[i, (int)GridColumn.Amount].Value = "(NEW)";

           

            SourceGrid.Cells.Editors.TextBox txtchequenumber = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtchequenumber.EditableMode = SourceGrid.EditableMode.Focus;

            grdContra[i, (int)GridColumn.Cheque_No] = new SourceGrid.Cells.Cell("", txtchequenumber);
            grdContra[i, (int)GridColumn.Cheque_No].Value = "(NEW)";

            SourceGrid.Cells.Button btnChequeDate = new SourceGrid.Cells.Button(""); //Date.ToSystem(DateTime.Today)
            txtchequenumber.EditableMode = SourceGrid.EditableMode.SingleClick;
           // btnChequeDate.Controller.OnClick += new EventHandler(Text_Change);
            grdContra[i, (int)GridColumn.ChequeDate] = btnChequeDate;
            grdContra[i, (int)GridColumn.ChequeDate].AddController(evtChequeDateFocusLost);

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;

            grdContra[i, (int)GridColumn.Remarks] = new SourceGrid.Cells.Cell("", txtRemarks);
            grdContra[i, (int)GridColumn.Remarks].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdContra[i, (int)GridColumn.Ledger_ID] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdContra[i, (int)GridColumn.Ledger_ID].Value = "";

            SourceGrid.Cells.Editors.TextBox txtCurrBal = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrBal.EditableMode = SourceGrid.EditableMode.None;
            grdContra[i, (int)GridColumn.Current_Bal_Actual] = new SourceGrid.Cells.Cell("", txtCurrBal);
            grdContra[i, (int)GridColumn.Current_Bal_Actual].Value = "";  
        }

        private void AddRowContra1(int RowCount)
        {
            try
            {
            //Add a new row
            grdContra.Redim(Convert.ToInt32(RowCount + 1), grdContra.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;
            grdContra[i, (int)GridColumn.Del] = btnDelete;
            grdContra[i, (int)GridColumn.Del].AddController(evtDelete);

            grdContra[i, (int)GridColumn.Code_No] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox txtAccount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAccount.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdContra[i, (int)GridColumn.Particular_Account_Head] = new SourceGrid.Cells.Cell("", txtAccount);
            txtAccount.Control.GotFocus += new EventHandler(Account_Focused);
            txtAccount.Control.LostFocus += new EventHandler(Account_Leave);
            txtAccount.Control.KeyDown += new KeyEventHandler(Account_KeyDown);
            txtAccount.Control.TextChanged += new EventHandler(Text_Change);
            grdContra[i, (int)GridColumn.Particular_Account_Head].AddController(evtAccountFocusLost);
            grdContra[i, (int)GridColumn.Particular_Account_Head].Value = "(NEW)";

            SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
            cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;
            string strDrCr = "Debit";
            if (grdContra[i - 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
                strDrCr = "Credit";
            //this is for auto debit and credit type
            if (i > 2)
            {
                if ((m_DebitAmount - m_CreditAmount) > 0)
                {
                    strDrCr = "Credit";
                }
                if ((m_DebitAmount - m_CreditAmount) < 0)
                {
                    strDrCr = "Debit";
                }
            }
            grdContra[i, (int)GridColumn.Dr_Cr] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);
            cboDrCr.Control.SelectedIndexChanged += new EventHandler(Text_Change);
            grdContra[i, (int)GridColumn.Dr_Cr].AddController(evtcboDrCrSelectedIndexChanged);

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
            grdContra[i, (int)GridColumn.Amount] = new SourceGrid.Cells.Cell("", txtAmount);
            txtAmount.Control.TextChanged += new EventHandler(Text_Change);
            grdContra[i, (int)GridColumn.Amount].AddController(evtAmountFocusLost);

            SourceGrid.Cells.Editors.TextBox txtCurrentBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtCurrentBalance.EditableMode = SourceGrid.EditableMode.None;
            grdContra[i, (int)GridColumn.Current_Balance] = new SourceGrid.Cells.Cell("", txtCurrentBalance);
            grdContra[i, (int)GridColumn.Current_Balance].Value = "";

            SourceGrid.Cells.Editors.TextBox txtChequeNumber = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtChequeNumber.EditableMode = SourceGrid.EditableMode.Focus;
            grdContra[i, (int)GridColumn.Cheque_No] = new SourceGrid.Cells.Cell("", txtChequeNumber);
            txtChequeNumber.Control.TextChanged += new EventHandler(Text_Change);

            SourceGrid.Cells.Button btnChequeDate = new SourceGrid.Cells.Button(""); //Date.ToSystem(DateTime.Today)
            txtChequeNumber.EditableMode = SourceGrid.EditableMode.SingleClick;
            //btnChequeDate.Controller.OnClick += new EventHandler(Text_Change);
            grdContra[i, (int)GridColumn.ChequeDate] = btnChequeDate;
            grdContra[i, (int)GridColumn.ChequeDate].AddController(evtChequeDateFocusLost);

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
            grdContra[i, (int)GridColumn.Remarks] = new SourceGrid.Cells.Cell("", txtRemarks);
            txtRemarks.Control.TextChanged += new EventHandler(Text_Change);
            // grdJournal[i, 5].Value = "";

            SourceGrid.Cells.Editors.TextBox txtLedgerID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtLedgerID.EditableMode = SourceGrid.EditableMode.None;
            grdContra[i, (int)GridColumn.Ledger_ID] = new SourceGrid.Cells.Cell("", txtLedgerID);
            grdContra[i, (int)GridColumn.Ledger_ID].Value = "";

            SourceGrid.Cells.Editors.TextBox txtCurrentBalance1 = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            grdContra[i, (int)GridColumn.Current_Bal_Actual] = new SourceGrid.Cells.Cell("", txtCurrentBalance1);
            grdContra[i, (int)GridColumn.Current_Bal_Actual].Value = "";
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

         
        }
        private void Account_Focused(object sender, EventArgs e)
        {

            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                frmLOVLedger frm = new frmLOVLedger(this,"CONTRA");
                frm.ShowDialog();
                SendKeys.Send("{Tab}");
            }
        }

        private void Account_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.N)
            {
                txtRemarks.Focus();
            }
            else
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;
                frmLOVLedger frm = new frmLOVLedger(this, e,"CONTRA");
                frm.ShowDialog();
            }
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

        private void CalculateDrCr()
        {
            try
            {
                double dr, cr;
                dr = cr = 0;
                for (int i = 1; i < grdContra.RowsCount - 1; i++)
                {
                    //Check if it is the (NEW) row. If it is, it must be the last row.
                    if (i == grdContra.Rows.Count)
                        return;

                    double m_Amount = 0;
                    string m_Value = Convert.ToString(grdContra[i, (int)GridColumn.Amount].Value);//Had to do this because it showed error when the cell was left blank
                    if (m_Value.Length == 0)
                        m_Amount = 0;
                    else
                        m_Amount = Convert.ToDouble(grdContra[i, (int)GridColumn.Amount].Value);

                    if (grdContra[i, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")
                        dr += m_Amount;
                    else if (grdContra[i, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                        cr += m_Amount;
                }
                m_DebitAmount = dr;
                m_CreditAmount = cr;

                lblDebitTotal.Text = m_DebitAmount.ToString();
                lblCreditTotal.Text = m_CreditAmount.ToString();
                lblDifferenceAmount.Text = (Convert.ToDouble(m_DebitAmount) - Convert.ToDouble(m_CreditAmount)).ToString();
            }
            catch (Exception ex)
            {
                Global.MsgError("Error in Debit/Credit calucation!");
            }
        }

        private void ClearNew(int RowCount)
        {
            if (grdContra[RowCount, (int)GridColumn.Particular_Account_Head].Value == "(NEW)")
                grdContra[RowCount, (int)GridColumn.Particular_Account_Head].Value = "";
            if (grdContra[RowCount, (int)GridColumn.Dr_Cr].Value == "(NEW)")
                grdContra[RowCount, (int)GridColumn.Dr_Cr].Value = "";
            if (grdContra[RowCount, (int)GridColumn.Amount].Value == "(NEW)")
                grdContra[RowCount, (int)GridColumn.Amount].Value = "";
            //if (grdContra[RowCount, 5].Value == "(NEW)")
            //    grdContra[RowCount, 5].Value = "";

        }

        //A function from the Interface IfrmAccClassID. Used to apply the Datatable to this form from AddAccClass Form
        private void btnSave_Click(object sender, EventArgs e)
        {
             bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("Input date is out of range!");
                return;
            }
            ArrayList arrNode = treeAccClass.GetCheckedNodes(true);
            foreach (string tag   in arrNode)
            {
                AccClassID.Add(Convert.ToInt32(tag));
            }
            #region BLOCK FOR MANUAL VOUCHER NUMBERING TYPE
            VoucherConfiguration m_VouConfig = new VoucherConfiguration();
            if (SeriesID.ID > 0)
            {
                DataTable dtVouConfigInfo = m_VouConfig.GetVouNumConfiguration(Convert.ToInt32(SeriesID.ID));
                DataRow drVouConfigInfo = dtVouConfigInfo.Rows[0];
                if (drVouConfigInfo["NumberingType"].ToString() == "Manual")
                {
                  //Enter in this block only when VoucherNumberingType is Manual
                    //Checking for Manual VoucherNumberingType
                    try
                    {

                        string returnStr = m_VouConfig.ValidateManualVouNum(txtVchNo.Text, Convert.ToInt32(SeriesID.ID));
                        switch (returnStr)
                        {
                            case "INVALID_SERIES":
                                {
                                    MessageBox.Show("Invalid Series Name,please select valid Series Name and try again!");
                                    return;
                                }
                                break;
                            case "BLANK_WARN":
                                if (MessageBox.Show("Voucher Number is Blank, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                break;
                            case "BLANK_DONT_ALLOW":
                                MessageBox.Show("Voucher Number is Blank,Please fill the Voucher Number first!");
                                return;
                                break;
                            case "SUCCESS":

                                break;
                            case "DUPLICATE_WARN":
                                if (MessageBox.Show("Voucher Number is Duplicated, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                break;
                            case "DUPLICATE_DONT_ALLOW":
                                {
                                    MessageBox.Show("Voucher Number is Duplicated,Please insert the unique Voucher Number");
                                    return;
                                }
                                break;

                        }

                    }
                    catch (Exception ex)
                    {

                        Global.Msg(ex.Message);
                    }
                }
            }
            #endregion
            //Check Validation
            if (!Validate())
                return;
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
            //Check whether dr. and cr. total amount is equal
            if ((Convert.ToDouble(m_DebitAmount) - Convert.ToDouble(m_CreditAmount)) != 0)
            {
                Global.Msg("Debit and Credit amount are not equal!");
                return;
            }

            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    isNew = true;
                    OldGrid = " ";
                    NewGrid = " ";
                    NewGrid = NewGrid + "Voucher No" + txtVchNo.Text + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text;
            //Collect the Contents of the grid for audit log
                    for (int i = 0; i < grdContra.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                    {

                        string Ledgername = grdContra[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                        string drcr = grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString();
                        string amt = grdContra[i + 1, (int)GridColumn.Amount].Value.ToString();
                        string chequenumber = grdContra[i + 1, (int)GridColumn.Cheque_No].ToString();
                       
                        NewGrid = NewGrid + string.Concat(Ledgername, drcr, chequenumber, amt);
                    }
                NewGrid = "NewGridValues" + NewGrid;
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable ContraDetails = new DataTable();
                        ContraDetails.Columns.Add("Ledger");
                        ContraDetails.Columns.Add("DrCr");
                        ContraDetails.Columns.Add("Amount");
                        ContraDetails.Columns.Add("Remarks");
                        ContraDetails.Columns.Add("ChequeNumber");
                        ContraDetails.Columns.Add("ChequeDate");

                        for (int i = 0; i < grdContra.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdContra[i + 1, (int)GridColumn.Particular_Account_Head].ToString().Split('[');
                            ContraDetails.Rows.Add(ledgerName[0].ToString(), grdContra[i + 1, (int)GridColumn.Dr_Cr].Value, grdContra[i + 1, (int)GridColumn.Amount].Value, grdContra[i + 1, (int)GridColumn.Remarks].Value, grdContra[i + 1, (int)GridColumn.Cheque_No].Value, grdContra[i + 1, (int)GridColumn.ChequeDate].Value);
                            bool isCashAccount = false;
                            double totalDrCash, totalCrCash;
                            totalDrCash = totalCrCash = 0;
                            isCashAccount = Ledger.IsCashAccount(ledgerName[0].ToString());
                            //Block for checking Negative Cash and Negative bank

                            //Check the negative cash and negative Bank  ledger who is responsible for payment...so,If sorcegrid is Debit,no need to check because it will recieve amount bt check negative cash and negative bank in case of Credit
                            if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")//check negative cash and negative bank in case of ledger is responisble of payment otherwise no need to check...
                            {
                                #region BLOCK FOR CHECKING NEGATIVE CASH
                                //execute following code when only if setting of Negative cash is in Warn or Deny
                                if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                                {
                                    if (isCashAccount)//If cash account
                                    {
                                        double mDbalCash = 0;
                                        double mCbalCash = 0;
                                        int cashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);//Getting LedgerID with help of LedgerName
                                        Transaction.GetLedgerBalance(null, null, cashLedgerId, ref mDbalCash, ref mCbalCash, AccountClassID, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                        //Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrCash = Convert.ToDouble(grdContra[i + 1, (int)GridColumn.Amount].Value);//In case of Cash and Bank ...Ledger Balance is Debit
                                        }
                                        else if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                                        {
                                            totalCrCash = Convert.ToDouble(grdContra[i + 1, (int)GridColumn.Amount].Value);//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
                                        }
                                       
                                        //Total amount of ledger is summation of its own amount and amount posted from grid by enduser.
                                        totalDrCash += mDbalCash;
                                        totalCrCash += mCbalCash;

                                        if ((totalDrCash - totalCrCash) >= 0)
                                            IsNegativeCash = false;
                                        else
                                            IsNegativeCash = true;

                                        //If -ve cash not allowed in settings

                                        #region NEGATIVE CASH SETTINGS
                                        switch (Global.Default_NegativeCash)
                                        {
                                            case NegativeCash.Allow:
                                                //if (IsNegativeCash == true)
                                                //Do nothing
                                                break;
                                            case NegativeCash.Warn:
                                                if (IsNegativeCash == true)
                                                {
                                                    if (MessageBox.Show("Your Cash Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                                    {
                                                        return;
                                                    }
                                                }
                                                break;
                                            case NegativeCash.Deny:
                                                if (IsNegativeCash == true)
                                                {
                                                    Global.MsgError("Your Cash amount is negative,you are not allowed to submit this voucher!!!");
                                                    return;
                                                }
                                                break;

                                        }
                                        #endregion
                                    }
                                }
                                #endregion

                                #region BLOCK FOR CHECKING NEGATIVE BANK
                                if ((Global.Default_NegativeBank == NegativeBank.Warn) || (Global.Default_NegativeBank == NegativeBank.Deny))
                                {

                                    bool isBankAccount = false;
                                    isBankAccount = Ledger.IsBankAccount((ledgerName[0].ToString()));
                                    if (isBankAccount)//If Bank account
                                    {
                                        double mDbalBank = 0;
                                        double mCbalBank = 0;
                                        double totalDrBank = 0;
                                        double totalCrBank = 0;

                                        int bankLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);

                                        Transaction.GetLedgerBalance(null,null,bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode,0);//we dont need to check according to project soo ProjecID is kept as zero
                                        //Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrBank = Convert.ToDouble(grdContra[i + 1, (int)GridColumn.Amount].Value);

                                        }
                                        else if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")//If ledger posted in Grid is Credit Type
                                        {
                                            totalCrBank = Convert.ToDouble(grdContra[i + 1, (int)GridColumn.Amount].Value);
                                        }

                                        //Total amount of ledger is summation of its own amount and amount posted from grid by enduser.
                                        totalDrBank += mDbalBank;
                                        totalCrBank += mCbalBank;
                                        if ((totalDrBank - totalCrBank) >= 0)
                                            IsNegativeBank = false;
                                        else
                                            IsNegativeBank = true;

                                        //If -ve Bank not allowed in settings
                                        #region NEGATIVE BANK SETTINNGS
                                        switch (Global.Default_NegativeBank)
                                        {
                                            case NegativeBank.Allow:
                                                //if (IsNegativeCash == true)
                                                //Do nothing
                                                break;
                                            case NegativeBank.Warn:
                                                if (IsNegativeBank == true)
                                                {
                                                    if (MessageBox.Show("Your Bank Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                                    {
                                                        return;
                                                    }
                                                }
                                                break;
                                            case NegativeBank.Deny:
                                                if (IsNegativeBank == true)
                                                {
                                                    Global.MsgError("Your Bank amount is negative,you are not allowed to submit this voucher!!!");
                                                    return;
                                                }
                                                break;
                                        }
                                        #endregion

                                    }
                                }

                                #endregion                                                                     
                            }

                  
                        }
                        DateTime ContraDate = Date.ToDotNet(txtDate.Text);

                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        if (AccClassID.Count != 0)
                        {
                            m_Contra.Create(Convert.ToInt32(SeriesID.ID), txtVchNo.Text, ContraDate, txtRemarks.Text, ContraDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID),OldGrid,NewGrid,isNew,OF);
                        }
                        else
                        {
                            int[] a = new int[] { 1 };
                            m_Contra.Create(Convert.ToInt32(SeriesID.ID), txtVchNo.Text, ContraDate, txtRemarks.Text, ContraDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), OldGrid, NewGrid, isNew,OF);

                        
                        }
                        Global.Msg("Contra created successfully!");
                        AccClassID.Clear();
                        ClearVoucher();
                        ChangeState(EntryMode.NEW);
                        btnNew_Click(sender, e);

                        
                        //Do not close the form if do not close is checked
                        if (!chkDoNotClose.Checked)
                            this.Close();


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

                #region EDIT
                case EntryMode.EDIT: //if edit button is pressed
                    isNew = false;
                    NewGrid = " ";
                    NewGrid = NewGrid + "Voucher No" + txtVchNo.Text + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text;
            //Collect the Contents of the grid for audit log
                    for (int i = 0; i < grdContra.Rows.Count - 2; i++)//ignore the first row being header and last row being new
                    {
                        string Ledgername = grdContra[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                        string drcr = grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString();
                        string amt = grdContra[i + 1, (int)GridColumn.Amount].Value.ToString();
                        string chequenumber = grdContra[i + 1, (int)GridColumn.Cheque_No].ToString();
                        NewGrid = NewGrid + string.Concat(Ledgername, drcr, chequenumber, amt);
                    }
                    NewGrid = "NewGridValues" + NewGrid;
                    try
                    {
                        //Read from sourcegrid and store it to table
                        DataTable ContraDetails = new DataTable();
                        ContraDetails.Columns.Add("Ledger");
                        ContraDetails.Columns.Add("DrCr");
                        ContraDetails.Columns.Add("Amount");
                        ContraDetails.Columns.Add("Remarks");
                        ContraDetails.Columns.Add("ChequeNumber");
                        ContraDetails.Columns.Add("ChequeDate");

                        for (int i = 0; i < grdContra.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            string[] ledgerName = grdContra[i + 1, (int)GridColumn.Particular_Account_Head].ToString().Split('[');
                            ContraDetails.Rows.Add(grdContra[i + 1, (int)GridColumn.Particular_Account_Head].Value, grdContra[i + 1, (int)GridColumn.Dr_Cr].Value, grdContra[i + 1, (int)GridColumn.Amount].Value, grdContra[i + 1, (int)GridColumn.Remarks].Value, grdContra[i + 1, (int)GridColumn.Cheque_No].Value, grdContra[i + 1, (int)GridColumn.ChequeDate].Value);
                            bool isCashAccount = false;
                            double totalDrCash, totalCrCash;
                            totalDrCash = totalCrCash = 0;
                            isCashAccount = Ledger.IsCashAccount(ledgerName[0].ToString());
                            //Block for checking Negative Cash and Negative bank

                            //Check the negative cash and negative Bank  ledger who is responsible for payment...so,If sorcegrid is Debit,no need to check because it will recieve amount bt check negative cash and negative bank in case of Credit
                            if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")//check negative cash and negative bank in case of ledger is responisble of payment otherwise no need to check...
                            {
                                #region BLOCK FOR CHECKING NEGATIVE CASH
                                //execute following code when only if setting of Negative cash is in Warn or Deny
                                if ((Global.Default_NegativeCash == NegativeCash.Warn) || (Global.Default_NegativeCash == NegativeCash.Deny))
                                {
                                    if (isCashAccount)//If cash account
                                    {
                                        double mDbalCash = 0;
                                        double mCbalCash = 0;
                                        int cashLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);//Getting LedgerID with help of LedgerName
                                        Transaction.GetLedgerBalance(null, null, cashLedgerId, ref mDbalCash, ref mCbalCash, AccountClassID, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                        //Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrCash = Convert.ToDouble(grdContra[i + 1, (int)GridColumn.Amount].Value);//In case of Cash and Bank ...Ledger Balance is Debit
                                        }
                                        else if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                                        {
                                            totalCrCash = Convert.ToDouble(grdContra[i + 1, (int)GridColumn.Amount].Value);//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
                                        }
                                        //Total amount of ledger is summation of its own amount and amount posted from grid by enduser.
                                        totalDrCash += mDbalCash;
                                        totalCrCash += mCbalCash;
                                        if ((totalDrCash - totalCrCash) >= 0)
                                            IsNegativeCash = false;
                                        else
                                            IsNegativeCash = true;

                                        //If -ve cash not allowed in settings

                                        #region NEGATIVE CASH SETTINGS
                                        switch (Global.Default_NegativeCash)
                                        {
                                            case NegativeCash.Allow:
                                                //if (IsNegativeCash == true)
                                                //Do nothing
                                                break;
                                            case NegativeCash.Warn:
                                                if (IsNegativeCash == true)
                                                {
                                                    if (MessageBox.Show("Your Cash Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                                    {
                                                        return;
                                                    }
                                                }
                                                break;
                                            case NegativeCash.Deny:
                                                if (IsNegativeCash == true)
                                                {
                                                    Global.MsgError("Your Cash amount is negative,you are not allowed to submit this voucher!!!");
                                                    return;
                                                }
                                                break;

                                        }
                                        #endregion
                                    }
                                }
                                #endregion

                                #region BLOCK FOR CHECKING NEGATIVE BANK
                                if ((Global.Default_NegativeBank == NegativeBank.Warn) || (Global.Default_NegativeBank == NegativeBank.Deny))
                                {

                                    bool isBankAccount = false;
                                    isBankAccount = Ledger.IsBankAccount((ledgerName[0].ToString()));
                                    if (isBankAccount)//If Bank account
                                    {
                                        double mDbalBank = 0;
                                        double mCbalBank = 0;
                                        double totalDrBank = 0;
                                        double totalCrBank = 0;

                                        int bankLedgerId = Ledger.GetLedgerIdFromName(ledgerName[0].ToString(), LangMgr.DefaultLanguage);

                                        Transaction.GetLedgerBalance(null, null, bankLedgerId, ref mDbalBank, ref mCbalBank, arrNode, 0);//we dont need to check according to project soo ProjecID is kept as zero
                                        //Remember Asset and Income are Dr type of Account and Liabilities and Expenditure are Cr type of account
                                        if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Debit")//If ledger posted in Grid is Debit Type
                                        {
                                            totalDrBank = Convert.ToDouble(grdContra[i + 1, (int)GridColumn.Amount].Value);//In case of Cash and Bank ...Ledger Balance is Debit

                                        }
                                        else if (grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString() == "Credit")
                                        {
                                            totalCrBank =  Convert.ToDouble(grdContra[i + 1, (int)GridColumn.Amount].Value);//In case of Credit type of Ledger subract Credit amount posted in Grid From Debit Balance of Corresponding Ledger
                                        }
                                        //Total amount of ledger is summation of its own amount and amount posted from grid by enduser.
                                        totalDrBank += mDbalBank;
                                        totalCrBank += mCbalBank;
                                        if ((totalDrBank - totalCrBank) >= 0)
                                            IsNegativeBank = false;
                                        else
                                            IsNegativeBank = true;

                                        //If -ve Bank not allowed in settings
                                        #region NEGATIVE BANK SETTINNGS
                                        switch (Global.Default_NegativeBank)
                                        {
                                            case NegativeBank.Allow:
                                                //if (IsNegativeCash == true)
                                                //Do nothing
                                                break;
                                            case NegativeBank.Warn:
                                                if (IsNegativeBank == true)
                                                {
                                                    if (MessageBox.Show("Your Bank Amount is negative, are you sure you want to proceed?", "Confirmation!", MessageBoxButtons.YesNo) == DialogResult.No)
                                                    {
                                                        return;
                                                    }
                                                }
                                                break;
                                            case NegativeBank.Deny:
                                                if (IsNegativeBank == true)
                                                {
                                                    Global.MsgError("Your Bank amount is negative,you are not allowed to submit this voucher!!!");
                                                    return;
                                                }
                                                break;
                                        }
                                        #endregion

                                    }
                                }

                                #endregion                                                                     
                            }
                        
                        }

                        DateTime Contra_Date = Date.ToDotNet(txtDate.Text);
                        Contra m_Contra = new Contra();
                        SeriesID = (ListItem)cboSeriesName.SelectedItem;
                        liProjectID = (ListItem)cboProjectName.SelectedItem;
                        OptionalField OF = new OptionalField();

                        OF.First = txtfirst.Text;
                        OF.Second = txtsecond.Text;
                        OF.Third = txtthird.Text;
                        OF.Fourth = txtfourth.Text;
                        OF.Fifth = txtfifth.Text;
                        if (AccClassID.Count != 0)
                        {
                            m_Contra.Modify(Convert.ToInt32(txtContraID.Text), Convert.ToInt32(SeriesID.ID), txtVchNo.Text, Contra_Date, txtRemarks.Text, ContraDetails, AccClassID.ToArray(), Convert.ToInt32(liProjectID.ID), OldGrid, NewGrid, isNew,OF);
                        }
                        else 
                        {
                            int[] a = new int[] { 1 };
                            m_Contra.Modify(Convert.ToInt32(txtContraID.Text), Convert.ToInt32(SeriesID.ID), txtVchNo.Text, Contra_Date, txtRemarks.Text, ContraDetails, a.ToArray(), Convert.ToInt32(liProjectID.ID), OldGrid, NewGrid, isNew,OF);                        
                        }
                        Global.Msg("Contra modified successfully!");
                        AccClassID.Clear();
                        ClearVoucher();
                        ChangeState(EntryMode.NEW);
                        btnNew_Click(sender, e);

                        if (checkBox2.Checked)
                        {
                            prntDirect = 1;
                            Navigation(Navigate.Last);
                            button3_Click(sender, e);
                            ClearVoucher();
                            ChangeState(EntryMode.NEW);
                            btnNew_Click(sender, e);
                        }
                        if (!chkDoNotClose.Checked)
                            this.Close();
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
           
        private bool Validate()
        {
            bool bValidate = false;
            FormHandle m_FH = new FormHandle();
            bValidate = m_FH.Validate();
            if (cboSeriesName.SelectedItem == null)
            {
                Global.MsgError("Invalid Series Name Selected");
                cboSeriesName.Focus();
                bValidate = false;
            }
            if (!(grdContra.Rows.Count > 2))
            {
                Global.MsgError("Invalid Account Ledger Selected in grid");
                grdContra.Focus();
                bValidate = false;
            } 
            return bValidate;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;

            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false);
                    IsFieldChanged = false;
                    break;

                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    LoadSeriesNo();
                    IsFieldChanged = false;
                    break;

                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    break;

            }
        }

        private void LoadSeriesNo()
        {
            DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("CNTR");
            cboSeriesName.Items.Clear();
            for (int i = 1; i <= dtSeriesInfo.Rows.Count; i++)
            {
                DataRow drSeriesInfo = dtSeriesInfo.Rows[i - 1];
                cboSeriesName.Items.Add(new ListItem((int)drSeriesInfo["SeriesID"], drSeriesInfo["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cboSeriesName.SelectedIndex = 0;
            cboSeriesName.DisplayMember = "value";//This value is  for showing at Load condition
            cboSeriesName.ValueMember = "id";//This value is stored only not to be shown at Load condition  
          
        }

        private void EnableControls(bool Enable)
        {
            txtVchNo.Enabled = txtDate.Enabled = txtRemarks.Enabled = grdContra.Enabled = cboSeriesName.Enabled =tabControl1.Enabled= Enable;
        }

        //Enables and disables the button states
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
        }

        private void ClearVoucher()
        {
            ClearContra();
            grdContra.Redim(2, 11);
            AddGridHeader(); //Write header part
            //AddRowContra(1);
            AddRowContra1(1);
        }

        private void ClearContra()
        {
            txtVchNo.Clear(); //actually generate a new voucher no.
            //txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtRemarks.Clear();
            grdContra.Rows.Clear();
            cboProjectName.SelectedIndex = 0;
        }

        private bool Navigation(Navigate NavTo)
        {
            try
            {
               ChangeState(EntryMode.NORMAL);
                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtContraID.Text);
                    if (ContraIDCopy > 0)
                    {
                        VouchID = ContraIDCopy;
                        ContraIDCopy = 0;
                    }
                    else
                    {
                        VouchID = Convert.ToInt32(txtContraID.Text);
                    
                    }
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
                DataTable dtContraMaster = m_Contra.NavigateContraMaster(VouchID, NavTo);
                if (dtContraMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    btnExport.Enabled = false;
                    return false;
                }

                //Clear everything in the form
                ClearVoucher();
                //Write the corresponding textboxes
                DataRow drContraMaster = dtContraMaster.Rows[0]; //There is only one row. First row is the required record
                if (IsShortcutKey)
                {
                    txtRemarks.Text = drContraMaster["Remarks"].ToString();
                    IsShortcutKey = false;
                    txtRemarks.SelectionStart = txtRemarks.Text.Length + 1;
                    return false;
                }        

                DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(drContraMaster["SeriesID"]));
                if (dt.Rows.Count <= 0)
                {
                    Global.Msg("There is no any SeriesName in this Contra");
                    cboSeriesName.Text = "";                    
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    cboSeriesName.Text = dr["EngName"].ToString();
                }
                txtVchNo.Text = drContraMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drContraMaster["Contra_Date"].ToString());
                txtRemarks.Text = drContraMaster["Remarks"].ToString();
                txtContraID.Text = drContraMaster["ContraID"].ToString();
                //For Additional Fields
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

                        txtfirst.Text = drContraMaster["Field1"].ToString();
                        txtsecond.Text = drContraMaster["Field2"].ToString();
                        txtthird.Text = drContraMaster["Field3"].ToString();
                        txtfourth.Text = drContraMaster["Field4"].ToString();
                        txtfifth.Text = drContraMaster["Field5"].ToString();
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

                        txtfirst.Text = drContraMaster["Field1"].ToString();
                        txtsecond.Text = drContraMaster["Field2"].ToString();
                        txtthird.Text = drContraMaster["Field3"].ToString();
                        txtfourth.Text = drContraMaster["Field4"].ToString();
                        txtfifth.Text = drContraMaster["Field5"].ToString();
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

                        txtfirst.Text = drContraMaster["Field1"].ToString();
                        txtsecond.Text = drContraMaster["Field2"].ToString();
                        txtthird.Text = drContraMaster["Field3"].ToString();
                        txtfourth.Text = drContraMaster["Field4"].ToString();
                        txtfifth.Text = drContraMaster["Field5"].ToString();

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

                        txtfirst.Text = drContraMaster["Field1"].ToString();
                        txtsecond.Text = drContraMaster["Field2"].ToString();
                        txtthird.Text = drContraMaster["Field3"].ToString();
                        txtfourth.Text = drContraMaster["Field4"].ToString();
                        txtfifth.Text = drContraMaster["Field5"].ToString();

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

                        txtfirst.Text = drContraMaster["Field1"].ToString();
                        txtsecond.Text = drContraMaster["Field2"].ToString();
                        txtthird.Text = drContraMaster["Field3"].ToString();
                        txtfourth.Text = drContraMaster["Field4"].ToString();
                        txtfifth.Text = drContraMaster["Field5"].ToString();
                    }


                }
                dsContra.Tables["tblContraMaster"].Rows.Add(cboSeriesName.Text, drContraMaster["Voucher_No"].ToString(), Date.DBToSystem(drContraMaster["Contra_Date"].ToString()), drContraMaster["Remarks"].ToString());

                DataTable dtContraDetail = m_Contra.GetContraDetail(Convert.ToInt32(txtContraID.Text));
                for (int i = 1; i <= dtContraDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtContraDetail.Rows[i - 1];
                    grdContra[i, (int)GridColumn.Code_No].Value = i.ToString();
                    grdContra[i, (int)GridColumn.Particular_Account_Head].Value = drDetail["LedgerName"].ToString();
                    grdContra[i, (int)GridColumn.Dr_Cr].Value = drDetail["DrCr"].ToString();
                    grdContra[i, (int)GridColumn.Amount].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString();
                    grdContra[i, (int)GridColumn.Remarks].Value = drDetail["Remarks"].ToString();
                    grdContra[i, (int)GridColumn.Cheque_No].Value = drDetail["ChequeNumber"].ToString();
                    if (drDetail["ChequeDate"].ToString() == "")
                    {
                        grdContra[i, (int)GridColumn.ChequeDate].Value = "";
                    }
                    else
                    {
                        grdContra[i, (int)GridColumn.ChequeDate].Value = Date.DBToSystem(drDetail["ChequeDate"].ToString());
                    }
                   // grdContra[i, 7].Value =Date.DBToSystem(drDetail["ChequeDate"].ToString());
                    //dsContra.Tables["tblContraDetails"].Rows.Add(drDetail["LedgerName"].ToString(), drDetail["DrCr"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                 
                    //Code To Get The Current Balance of the Respective Ledger
                    string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
                    string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
                    DataTable dtLdrInfo = Ledger.GetLedgerDetails(AccClassId, ProjectId, null, null, Convert.ToInt32(drDetail["LedgerID"]), null);
                    if (dtLdrInfo.Rows.Count != 1)
                    if (dtLdrInfo.Rows.Count != 1)
                    {
                        grdContra[i, (int)GridColumn.Current_Balance].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        grdContra[i, (int)GridColumn.Current_Bal_Actual].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                    }
                    else
                    {
                        DataRow drLdrInfo = dtLdrInfo.Rows[0];//Get first record
                        decimal Debit = 0;
                        decimal Credit = 0;
                        decimal DebitOpeningBal = 0;
                        decimal CreditOpeningBal = 0;
                        if (!(drLdrInfo["DebitTotal"] is DBNull))
                        {
                            Debit = Convert.ToDecimal(drLdrInfo["DebitTotal"]);
                        }
                        else
                            Debit = 0;

                        if (!(drLdrInfo["CreditTotal"] is DBNull))
                        {
                            Credit = Convert.ToDecimal(drLdrInfo["CreditTotal"]);
                        }
                        else
                            Credit = 0;

                        if (!(drLdrInfo["OpenBalDr"] is DBNull))
                        {
                            DebitOpeningBal = Convert.ToDecimal(drLdrInfo["OpenBalDr"]);
                        }
                        else
                            DebitOpeningBal = 0;

                        if (!(drLdrInfo["OpenBalCr"] is DBNull))
                        {
                            CreditOpeningBal = Convert.ToDecimal(drLdrInfo["OpenBalCr"]);
                        }
                        else
                            CreditOpeningBal = 0;

                        //Calculate Debit and Credit Totals
                        decimal DebitTotal = Debit + DebitOpeningBal;
                        decimal CreditTotal = Credit + CreditOpeningBal;

                        decimal Balance = DebitTotal - CreditTotal;
                        string strBalance = "";
                        //If +ve is present, show as Dr
                        strBalance = ((Balance < 0) ? Balance * -1 : Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        if (Balance >= 0)
                            strBalance = strBalance + " (Dr.)";

                        else //If balance is -ve, its Cr.
                            strBalance = strBalance + " (Cr.)";


                        //Write balance into the grid
                        grdContra[i, (int)GridColumn.Current_Balance].Value = strBalance;
                        grdContra[i, (int)GridColumn.Current_Bal_Actual].Value = Balance.ToString();

                       
                    }

                   // AddRowContra(grdContra.RowsCount);
                   AddRowContra1(grdContra.RowsCount);
                    dsContra.Tables["tblContraDetails"].Rows.Add(drDetail["LedgerName"].ToString(), drDetail["DrCr"].ToString(), Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drDetail["Amount"])).ToString(), drDetail["Remarks"].ToString());
                }
                //Calculate the Debit and Credit totals
                CalculateDrCr();

                DataTable dtAccClassDtl = AccountClass.GetAccClassInfo(Convert.ToInt32(txtContraID.Text), "CNTR");
                List<int> AccClassIDs = new List<int>();
                foreach (DataRow dr in dtAccClassDtl.Rows)
                {
                    AccClassIDs.Add(Convert.ToInt32(dr["AccClassID"]));
                }
                treeAccClass.ExpandAll();

                //Check for the treeview if it has Use
                foreach (TreeNode tn in treeAccClass.Nodes)
                {
                    LoadAccClassInfo(VouchID, tn, AccClassIDs.ToArray<int>(), treeAccClass);
                    int pid = Convert.ToInt32(tn.Tag);
                    if (AccClassIDs.ToArray<int>().Contains(pid))
                    {
                        tn.Checked = true;
                    }
                    else
                    {
                        tn.Checked = false;
                    }
                }
                btnExport.Enabled = true;
                return true;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
                return false;
            }
        }

        private void LoadAccClassInfo(int AccClassID, TreeNode tn, int[] CheckedIDs, TreeView tvAccClass)
        {
            foreach (TreeNode nd in tn.Nodes)
            {
                nd.Checked = false; //first clear the checkmark if anything is checked previously
                LoadAccClassInfo(AccClassID, nd, CheckedIDs, tvAccClass);

                foreach (int id in CheckedIDs)
                    if (Convert.ToInt32(nd.Tag) == id)
                        nd.Checked = true;
            }
            foreach (int parentid in CheckedIDs)
            {
                if (Convert.ToInt32(tn.Tag) != parentid)
                    tn.Checked = false;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Contra?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}
           
            Navigation(Navigate.First);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Contra?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}
           
            Navigation(Navigate.Prev);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Contra?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}
           
            Navigation(Navigate.Next);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_VIEW");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }

            //if (m_mode == EntryMode.NEW)
            //{
            //    if (Global.MsgQuest("Do you want to save changes to Contra?") == DialogResult.Yes)
            //        btnSave_Click(sender, e);

            //}
           
            Navigation(Navigate.Last);
        }

        private void cboSeriesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionalFields();
            try
            {
                //Do not check if the form is loading or data is loading due to some navigation key pressed
                if (m_mode == EntryMode.NEW || m_mode == EntryMode.EDIT)
                {
                    SeriesID = (ListItem)cboSeriesName.SelectedItem;
                    txtVchNo.Enabled = true;
                    string NumberingType = m_VouConfig.GetVouNumberingType(Convert.ToInt32(SeriesID.ID));                   
                    //if (NumberingType == "")
                    //{
                    //    txtVchNo.Text = "Main";
                    //    txtVchNo.Enabled = false;
                    //}

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
            
        }

        private void btnAddClass_Click(object sender, EventArgs e)
        {
            if (txtContraID.Text == "")
            {
                frmAddAccClass frm = new frmAddAccClass(this);
                frm.Show();
            }
            else
            {
                frmAddAccClass frm = new frmAddAccClass(this, Convert.ToInt32(txtContraID.Text), "CNTR");
                frm.Show();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            EnableControls(true);
            isNew = false;
            OldGrid = " ";
            OldGrid = OldGrid + "Voucher No" + txtVchNo.Text + "Series" + cboSeriesName.Text + "Project" + cboProjectName.Text + "Date" + txtDate.Text;
            //Collect the Contents of the grid for audit log
            for (int i = 0; i < grdContra.Rows.Count - 2; i++)//ignore the first row being header and last row being new
            {
                string Ledgername = grdContra[i + 1, (int)GridColumn.Particular_Account_Head].Value.ToString();
                string drcr = grdContra[i + 1, (int)GridColumn.Dr_Cr].Value.ToString();
                string amt = grdContra[i + 1, (int)GridColumn.Amount].Value.ToString();
                string chequenumber = grdContra[i + 1, (int)GridColumn.Cheque_No].ToString();
                OldGrid = OldGrid + string.Concat(Ledgername, drcr, chequenumber, amt);
            }
            OldGrid = "OldGridValues" + OldGrid;
           
            ChangeState(EntryMode.EDIT);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearVoucher();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
        }

        private void btnAddAccClass_Click(object sender, EventArgs e)
        {
            frmAccountClass frm = new frmAccountClass(this);
            frm.Show();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            ContraIDCopy = Convert.ToInt32(txtContraID.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (ContraIDCopy > 0)
            {
                if (m_mode == EntryMode.NEW)
                {
                    Navigation(Navigate.ID);
                    EnableControls(true);
                    ChangeState(EntryMode.NEW);
                }
                else
                {
                    Global.Msg("Please press New Button for Pasting the copied Voucher");
                }
            }
            else
            {
                Global.Msg("Please navigate to a voucher and press copy button first to specify the source voucher");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONTRA_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            if (Freeze.IsDateFreeze(Date.ToDotNet(txtDate.Text)))
            {
                MessageBox.Show("This voucher has been frozen! For more detail please contact administrator.");
                return;
            }
            //ErrorManager.ErrorManager.Log("ExTest", "ClassTest", "fundtest", "UMtest", 31, "workTEst", ErrorManager.ErrorManager.ErrorSeverity.High);
            try
            {

                //Ask if he really wants to delete and he hasnt mistakely pressing the delete button
                if (Global.MsgQuest("Are you sure you want to delete the contra - " + txtContraID.Text + "?") == DialogResult.Yes)
                {                    
                    Contra DelContra = new Contra();
                    if (DelContra.Delete(Convert.ToInt32(txtContraID.Text)))
                    {
                        Global.Msg("Contra -" + txtContraID.Text + " deleted successfully!");
                        //Navigate to 1 step previous
                        if (!this.Navigation(Navigate.Prev))
                        {
                            //This must be because there are no records or this was the first one

                            //If this was the first, try to navigate to second
                            if (!this.Navigation(Navigate.Next))
                            {
                                //This was the last one, there are no records left. Simply clear the form and stay calm
                                btnNew_Click(sender, e);
                            }
                        }
                    }
                    else
                        Global.MsgError("There was an error while deleting contra -" + txtContraID.Text + "!");
                }
            }
            catch (Exception ex)
            {

            }


        }

        private void frmContra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void ChequeDate_Click(object sender, EventArgs e)
        {
            IsChequeDateButton = true;
            ctx1 = (SourceGrid.CellContext)sender;
            if (ctx1.DisplayText.ToString() != "")
            {
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Convert.ToDateTime(ctx1.Value))));
                frm.ShowDialog();
            }
            else
            {
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(Date.ToSystem(Date.GetServerDate())));
                frm.ShowDialog();
            }
        }
        private void LoadComboboxProject(ComboBox ComboBoxControl, int ProjectID)
        {
            #region Language Management
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
            DataTable dt = Project.GetProjectTable(ProjectID);
            //DataTable dt1 = AccountClass.GetAccClassTable(ProjectID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], Prefix + " " + dr[LangField].ToString()));
                Prefix += "----";
                LoadComboboxProject(ComboBoxControl, Convert.ToInt16(dr["ProjectID"].ToString()));
            }
            //Prefix = "--";
            if (Prefix.Length > 1)
            {
                Prefix = Prefix.Remove(Prefix.Length - 4, 4);
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
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
                    button3_Click(sender, e);
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
                    button3_Click(sender, e);
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
                    button3_Click(sender, e);
                    break;
            }
        }
        //public void DateConvert(DateTime DotNetDate)
        //{
        //    if (!IsChequeDateButton)
        //        txtDate.Text = Date.ToSystem(DotNetDate);
        //    if (IsChequeDateButton)
        //        ctx1.Value = Date.ToSystem(DotNetDate);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            PrintPreviewCR(PrintType.CrystalReport);
        }
        private void PrintPreviewCR(PrintType myPrintType)
        {
            dsContra.Clear();
            rptContra rpt = new rptContra();
            Misc.WriteLogo(dsContra, "tblImage");
            rpt.SetDataSource(dsContra);

            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvTotalAmt = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvPrintDate = new CrystalDecisions.Shared.ParameterDiscreteValue();

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

            pdvPrintDate.Value = Date.ToSystem(DateTime.Now);
            pvCollection.Clear();
            pvCollection.Add(pdvPrintDate);
            rpt.DataDefinition.ParameterFields["Print_Date"].ApplyCurrentValues(pvCollection);

            //  Navigation(Navigate.ID);
            bool empty = Navigation(Navigate.ID);
            if (empty == false)
            {
                return;
            }
            else
            {

                frmReportViewer frm = new frmReportViewer();
                frm.SetReportSource(rpt);
                CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = FileName;
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
                        Common.frmemail sendemail = new Common.frmemail(FileName, 1);
                        sendemail.ShowDialog();
                        rpt.Close();
                        return;
                    default:
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;
                        break;
                }
                // frm.Show();
                frm.WindowState = FormWindowState.Maximized;
            }
        }
    
        

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
