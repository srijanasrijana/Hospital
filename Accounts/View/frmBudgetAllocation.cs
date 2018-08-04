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
using Accounts;
using Accounts.View;
using DateManager;
using BusinessLogic.Accounting;
namespace Accounts.View
{
    public partial class frmBudgetAllocation : Form
    {
        public frmBudgetAllocation()
        {
            InitializeComponent();
        }
        SourceGrid.Cells.Controllers.CustomEvents rowSelect = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtGrdAlloctDelete = new SourceGrid.Cells.Controllers.CustomEvents();

        int var;
        int masterBudgetID = 0;
        DataTable dt = new DataTable();
        DataTable dtledger;
        AccountGroup ag = new AccountGroup();
        int rowcount;
        frmAccountSetup fas = new frmAccountSetup();
        DataTable dtbudgetdetail = new DataTable();
        decimal fixedvalue = 0;
        decimal amount = 0;
        bool IsExpand = true;
        private SearchIn m_SearchIn = SearchIn.Account_Groups; //Default SearchIN Holder



        BusinessLogic.Accounting.Budget bgt = new BusinessLogic.Accounting.Budget();
        SourceGrid.Cells.Button btnDocDelete = new SourceGrid.Cells.Button("X");

        public void ShowAccountHeadInTreeView(TreeView tv)//TreeView tv, TreeNode n, int Group_ID)
        {
            try
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


                DataSet ds = AccountGroup.GetDataSetForAccountTreeView();
                foreach (DataRow drParentRow in ds.Tables[0].Rows)
                {
                    if (String.IsNullOrEmpty(drParentRow["ParentID"].ToString()) && Convert.ToInt32(drParentRow["GroupID"].ToString()) == 4)
                    {
                        TreeNode tnParentnode = new TreeNode();
                        tnParentnode.Text = drParentRow["GroupName"].ToString();
                        tnParentnode.Tag = Convert.ToInt32(drParentRow["GroupID"]);

                        SetTreeviewChilds(drParentRow, tnParentnode);
                        foreach (DataRow childLedger in drParentRow.GetChildRows("ChildLedger"))
                        {
                            TreeNode childLedgerNode = new TreeNode();
                            childLedgerNode.Text = childLedger["LedgerName"].ToString();
                            childLedgerNode.Tag = Convert.ToInt32(childLedger["LedgerID"]);
                            childLedgerNode.ForeColor = System.Drawing.Color.DarkBlue;
                            tnParentnode.Nodes.Add(childLedgerNode);
                        }
                        tv.Nodes.Add(tnParentnode);
                    }
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }

        }

        public void SetTreeviewChilds(DataRow dtRow, TreeNode tn)
        {
            DataRow[] ChildRows = dtRow.GetChildRows("ChildGroup");
            foreach (DataRow childrow in ChildRows)
            {
                TreeNode childnode = new TreeNode();
                childnode.Text = childrow["GroupName"].ToString();
                childnode.Tag = Convert.ToInt32(childrow["GroupID"]);
                if (childrow.GetChildRows("ChildGroup").Length > 0)
                {
                    SetTreeviewChilds(childrow, childnode);
                }
                foreach (DataRow childLedger in childrow.GetChildRows("ChildLedger"))
                {
                    TreeNode childLedgerNode = new TreeNode();
                    childLedgerNode.Text = childLedger["LedgerName"].ToString();
                    childLedgerNode.Tag = Convert.ToInt32(childLedger["LedgerID"]);
                    childLedgerNode.ForeColor = System.Drawing.Color.DarkBlue;
                    childnode.Nodes.Add(childLedgerNode);
                }
                tn.Nodes.Add(childnode);
            }

        }


        private void Budget_Load(object sender, EventArgs e)
        {


            //fas.ShowAccountHeadInTreeView(tvAccountHead);//, null, 4);
            ShowAccountHeadInTreeView(tvAccountHead);
            // getting all ledger id and name under expenditure group         
            createGridColumnHeader();
            btnEdit.Visible = false;
            btnCancel.Visible = false;
            LoadCurrentNUpcommingBudget();
            LoadAllBudgets();

            Search(SearchIn.Account_Groups, SearchOp.Begins_With, "", SearchOp.Equals, "");
            //Set the default search selection
            cboSrchSearchIn1.Items.Clear();
            cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Account_Groups, LangMgr.Translate("ACCOUNT_GROUP", "Account Groups")));
            cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Ledgers, LangMgr.Translate("ACCOUNT_LEDGER", "Ledgers")));
            cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Accounts_Under, LangMgr.Translate("ACCOUNT_UNDER", "Accout Under")));
            cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Ledgers_Under, LangMgr.Translate("LEDGER_UNDER", "Ledger Under")));

            cboSrchSearchIn1.SelectedIndex = 0;

            cboSrchOP1.Items.Clear();
            cboSrchOP2.Items.Clear();

            cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Begins_With, LangMgr.Translate("BEGINS_WITH", "Begins With")));
            cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Contains, LangMgr.Translate("CONTAINS", "Contains")));
            cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Equals, LangMgr.Translate("EQUALS", "Equals")));

            cboSrchOP1.SelectedIndex = 0;

            cboSrchOP2.Items.Add(new ListItem((int)SearchOp.Equals, LangMgr.Translate("EQUALS", "Equals")));
            cboSrchOP2.Items.Add(new ListItem((int)SearchOp.Greater_Or_Equals, LangMgr.Translate("GREATER_OR_EQUALS", "Greater or Equals")));
            cboSrchOP2.Items.Add(new ListItem((int)SearchOp.Smaller_Or_Equals, LangMgr.Translate("SMALLER_OR_EQUALS", "Smaller or Equals")));

            cboSrchOP2.SelectedIndex = -1;

        }
        private void Search(SearchIn m_SearchIn, SearchOp SrchOP1, string SearchParam1, SearchOp SrchOP2, string SearchParam2)
        {
            grdListView.SelectionMode = SourceGrid.GridSelectionMode.Row;
            //Disable multiple selection
            grdListView.Selection.EnableMultiSelection = false;
            string FilterString = "";
            switch (m_SearchIn)
            {
                //for both cases  accounts_under and account_groups same body is defined
                #region Accounts Under Search
                case SearchIn.Accounts_Under:
                #endregion

                #region Account Groups Search
                case SearchIn.Account_Groups:
                    DataTable grouplist = AccountGroup.Search(m_SearchIn, SrchOP1, SearchParam1, SrchOP2, SearchParam2, LangMgr.DefaultLanguage, true);//Search a blank text with begins with so that all the available data is listed
                    FilterString = "";
                    DataRow[] drgrouplist = grouplist.Select(FilterString);
                    FillAccountGroupInGrid(drgrouplist);
                    break;
                #endregion

                //for both ledger_under and leadgers same body is defined
                #region Ledger Under Search
                case SearchIn.Ledgers_Under:
                #endregion

                #region Ledger Search
                case SearchIn.Ledgers:
                    try
                    {
                        DataTable dtSrchLedger = Ledger.Search(m_SearchIn, SrchOP1, SearchParam1, SrchOP2, SearchParam2, LangMgr.DefaultLanguage, true);
                        //if (dtSrchLedger!=null)
                        //{
                        FilterString = "";
                        DataRow[] drFound2 = dtSrchLedger.Select(FilterString);
                        //Finally fill all the values in the grid with no filter applied
                        FillLedgerInGrid(drFound2);
                        //}
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion
            }
        }
        private void FillAccountGroupInGrid(DataRow[] drFound)
        {
            try
            {
                grdListView.Rows.Clear();
                switch (m_SearchIn)
                {
                    case SearchIn.Account_Groups:
                    case SearchIn.Accounts_Under:
                        grdListView.Redim(drFound.Count() + 1, 4);
                        grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
                        grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Acc. Group");
                        grdListView[0, 2] = new SourceGrid.Cells.ColumnHeader("Parent Group");
                        grdListView[0, 3] = new SourceGrid.Cells.ColumnHeader("Type");
                        grdListView[0, 0].Column.Width = 40;
                        grdListView[0, 1].Column.Width = 100;
                        grdListView[0, 2].Column.Width = 100;
                        grdListView[0, 3].Column.Width = 50;
                        break;
                }
                //Initialise the event handler
                SourceGrid.Cells.Controllers.CustomEvents CellClick = new SourceGrid.Cells.Controllers.CustomEvents();
                CellClick.Click += new EventHandler(Grid_Click);
                SourceGrid.Cells.Controllers.CustomEvents CellMove = new SourceGrid.Cells.Controllers.CustomEvents();
                CellMove.FocusEntered += new EventHandler(Grid_Click);

                for (int i = 1; i <= drFound.Count(); i++)
                {
                    DataRow dr = drFound[i - 1];
                    grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["GroupID"].ToString());
                    grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["Name"].ToString());
                    grdListView[i, 1].AddController(CellClick);
                    grdListView[i, 1].AddController(CellMove);

                    string Parent = dr["Parent"].ToString();
                    if (Parent == null || Parent == "")
                        Parent = ("(PRIMARY)");

                    grdListView[i, 2] = new SourceGrid.Cells.Cell(Parent);
                    grdListView[i, 2].AddController(CellClick);
                    grdListView[i, 2].AddController(CellMove);

                    string DebitCredit;
                    if (dr["Type"].ToString() == "DR")
                        DebitCredit = "Debit";
                    else if (dr["Type"].ToString() == "CR")
                        DebitCredit = "Credit";
                    else
                        DebitCredit = "Unknown";

                    grdListView[i, 3] = new SourceGrid.Cells.Cell(DebitCredit);
                    grdListView[i, 0].AddController(CellClick);
                    grdListView[i, 0].AddController(CellMove);
                }
                grdListView.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }
        private void FillLedgerInGrid(DataRow[] drFound)
        {
            try
            {
                grdListView.Rows.Clear();
                //WriteHeader();
                grdListView.Redim(drFound.Count() + 1, 5);
                grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
                grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Ledger");
                grdListView[0, 2] = new SourceGrid.Cells.ColumnHeader("Acc. Group");
                grdListView[0, 3] = new SourceGrid.Cells.ColumnHeader("Type");
                grdListView[0, 0].Column.Width = 40;
                grdListView[0, 1].Column.Width = 100;
                grdListView[0, 2].Column.Width = 100;
                //grdListView[0, 4].Column.Width = 150;
                //grdListView[0, 5].Column.Width = 60;

                //Initialise the event handler
                SourceGrid.Cells.Controllers.CustomEvents CellClick = new SourceGrid.Cells.Controllers.CustomEvents();
                CellClick.Click += new EventHandler(Grid_Click);

                SourceGrid.Cells.Controllers.CustomEvents CellMove = new SourceGrid.Cells.Controllers.CustomEvents();
                CellMove.FocusEntered += new EventHandler(Grid_Click);
                for (int i = 1; i <= drFound.Count(); i++)
                {
                    DataRow dr = drFound[i - 1];
                    grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["ID"].ToString());
                    grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["LedName"].ToString());
                    grdListView[i, 1].AddController(CellClick);
                    grdListView[i, 1].AddController(CellMove);

                    grdListView[i, 2] = new SourceGrid.Cells.Cell(dr["GroupName"].ToString());
                    grdListView[i, 2].AddController(CellClick);
                    grdListView[i, 2].AddController(CellMove);

                    string DebitCredit;
                    if (dr["DrCr"].ToString() == "DEBIT")
                        DebitCredit = "Debit";
                    else if (dr["DrCr"].ToString() == "CREDIT")
                        DebitCredit = "Credit";
                    else
                        DebitCredit = "Unknown";

                    grdListView[i, 3] = new SourceGrid.Cells.Cell(DebitCredit);
                    grdListView[i, 3].AddController(CellClick);
                    grdListView[i, 3].AddController(CellMove);
                }
                //Hide the ID column
                grdListView.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }
        private void Grid_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the Selected Row
                int CurRow = grdListView.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cell = new SourceGrid.CellContext(grdListView, new SourceGrid.Position(CurRow, 1));
                if (cell == null)
                    return;
                //Call the interface function to add the text in the parent form container
                switch (m_SearchIn)
                {
                    case SearchIn.Accounts_Under:
                    case SearchIn.Account_Groups:
                        txtGroupName.Text = cell.Value.ToString();
                        txtLedgerName.Text = "";
                        break;

                    case SearchIn.Ledgers_Under:
                    case SearchIn.Ledgers:
                        txtLedgerName.Text = cell.Value.ToString();
                        txtGroupName.Text = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }
        private void LoadAllBudgets()
        {
            DataTable dt = bgt.GetBudgetIDnNameFromAllocation();
            if (dt.Rows.Count > 0)
            {

                DataRow dr = dt.NewRow();
                dr["budgetName"] = "Copy From";
                dt.Rows.InsertAt(dr, 0);

                cmbCopyBudget.DataSource = dt;
                cmbCopyBudget.ValueMember = "budgetID";
                cmbCopyBudget.DisplayMember = "budgetName";
            }
        }
        private void LoadCurrentNUpcommingBudget()
        {
            try
            {
                if (dt.Rows.Count > 0)
                    dt.Rows.Clear();
                dt = bgt.GetCurrentNUpcommingBudgetData();
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["budgetName"] = "Select Budget Name";
                    dt.Rows.InsertAt(dr, 0);

                    cmbBudgetName.DataSource = dt;
                    cmbBudgetName.ValueMember = "budgetID";
                    cmbBudgetName.DisplayMember = "budgetName";
                    txtAllocationTotal.Text = "0";
                    txtAmountAssigned.Text = "0";
                }
                else
                {
                    cmbBudgetName.Text = "No budgets are avilable";
                }
            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }
        /// <summary>
        /// adding and styling grid column header
        /// </summary>
        private void createGridColumnHeader()
        {
            //DevAge.Drawing.RectangleBorder boader = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Black), new DevAge.Drawing.BorderLine(Color.Black), new DevAge.Drawing.BorderLine(Color.Black), new DevAge.Drawing.BorderLine(Color.Black));
            //   DevAge.Drawing.VisualElements.ColumnHeader colhead = new DevAge.Drawing.VisualElements.ColumnHeader();
            //  colhead.Border = boader;

            // colhead.BackColor = Color.Gray;
            // colhead.BackgroundColorStyle = DevAge.Drawing.BackgroundColorStyle.Solid;
            SourceGrid.Cells.Views.ColumnHeader headview = new SourceGrid.Cells.Views.ColumnHeader();
            headview.Font = new Font("Arial", 8, FontStyle.Bold);
            headview.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            //  headview.Background = colhead;

            grdAllocation.EnableSort = false;
            grdAllocation.ColumnsCount = 8;
            grdAllocation.FixedRows = 1;
            grdAllocation.Rows.Insert(0);

            grdAllocation[0, 0] = new SourceGrid.Cells.ColumnHeader();
            grdAllocation[0, 0].View = headview;

            grdAllocation[0, 1] = new SourceGrid.Cells.ColumnHeader("Del.");
            grdAllocation[0, 1].View = headview;

            grdAllocation[0, 2] = new SourceGrid.Cells.ColumnHeader("S.NO");
            grdAllocation[0, 2].View = headview;

            grdAllocation[0, 3] = new SourceGrid.Cells.ColumnHeader("BudgetMasterID");
            grdAllocation[0, 3].Column.Visible = false;

            grdAllocation[0, 4] = new SourceGrid.Cells.ColumnHeader("Account_id");
            grdAllocation[0, 4].Column.Visible = false;

            grdAllocation[0, 5] = new SourceGrid.Cells.ColumnHeader("Account Head/Ledger Name");
            grdAllocation[0, 5].View = headview;

            grdAllocation[0, 6] = new SourceGrid.Cells.ColumnHeader("Type ofA/C");
            grdAllocation[0, 6].View = headview;

            grdAllocation[0, 7] = new SourceGrid.Cells.ColumnHeader("Amount Allocated");
            grdAllocation[0, 7].View = headview;
            //  grdAllocation[0, 7].Column.Visible = false;

            grdAllocation[0, 0].Column.Width = 15;
            grdAllocation[0, 1].Column.Width = 30;
            grdAllocation[0, 2].Column.Width = 40;
            grdAllocation[0, 3].Column.Width = 100;
            grdAllocation[0, 4].Column.Width = 100;
            grdAllocation[0, 5].Column.Width = 200;
            grdAllocation[0, 6].Column.Width = 100;
            grdAllocation[0, 7].Column.Width = 150;

            //defining event handler to the custom event
            rowSelect.Click += new EventHandler(Row_Click);
            evtGrdAlloctDelete.Click += new EventHandler(GridDelButton_Click);
        }
        //defining function for custom event rowselect of gridview
        private void Row_Click(object sender, EventArgs e)
        {
            //set grid values to the text boxes when clicked at row
            SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
            int cr = context.Position.Row;
            masterBudgetID = Convert.ToInt32(grdAllocation[cr, 3].Value);
            if (grdAllocation[cr, 6].Value.ToString() == "Ledger")
            {
                txtGroupName.Text = "";
                txtLedgerName.Text = grdAllocation[cr, 5].Value.ToString();
            }
            else
            {
                txtLedgerName.Text = "";
                txtGroupName.Text = grdAllocation[cr, 5].Value.ToString();
            }
            //unuseable in this form
            // txtAmountAssigned.Text = grdAllocation[cr, 6].Value.ToString();
            var = cr;
        }
        //unuseable now
        #region Old BtnAllocate Click code
        private void btnAllocate_Click(object sender, EventArgs e)
        {
            //int ledgerid;
            //int groupid;
            //if (cmbBudgetName.SelectedIndex<=0)
            //{
            //    MessageBox.Show("NO budget selected.");
            //    return;
            //}
            //if(txtGroupName.Text=="" &&txtLedgerName.Text=="")
            //{
            //    MessageBox.Show("Please select ledger or group to allocate.");
            //    return;
            //}
            //if(txtAmountAssigned.Text=="")
            //{
            //    MessageBox.Show("Amount to be allocated is required.");
            //    return;
            //}
            //for (int i = 1; i < grdAllocation.Rows.Count;i++)
            //{
            //    if(txtGroupName.Text==grdAllocation[i,4].Value.ToString() || txtLedgerName.Text==grdAllocation[i,4].Value.ToString())
            //    {
            //        MessageBox.Show("This ledger or group is already allocated to the budget "+" "+cmbBudgetName.Text);
            //        return;
            //    }
            //}
            //if (!UserValidation.validDecimal(txtAmountAssigned.Text.Trim()))
            //{
            //    MessageBox.Show("Invalid amount assigned.");
            //    return;
            //}

            //rowcount = grdAllocation.RowsCount;
            //grdAllocation.Rows.Insert(rowcount);
            //grdAllocation[rowcount, 0] = new SourceGrid.Cells.Cell();
            //grdAllocation[rowcount, 1] = new SourceGrid.Cells.Cell(btnDocDelete);
            //grdAllocation[rowcount, 1].AddController(evtGrdAlloctDelete);

            //grdAllocation[rowcount, 2] = new SourceGrid.Cells.Cell(rowcount);
            //grdAllocation[rowcount, 2].AddController(rowSelect);

            ////if ledgername text has a ledger than get ledger id from the name else get group id
            //if (txtLedgerName.Text != "" && txtGroupName.Text == "")
            //{
            //    ledgerid = Ledger.GetLedgerIdFromName(txtLedgerName.Text.Trim(), Lang.English);
            //    //now add ledger id,name and type to source grid
            //    grdAllocation[rowcount, 3] = new SourceGrid.Cells.Cell("Ledger");
            //    grdAllocation[rowcount, 3].AddController(rowSelect);

            //    grdAllocation[rowcount, 4] = new SourceGrid.Cells.Cell(txtLedgerName.Text.Trim());
            //    grdAllocation[rowcount, 4].AddController(rowSelect);

            //    grdAllocation[rowcount, 5] = new SourceGrid.Cells.Cell(ledgerid);
            //    grdAllocation[rowcount, 5].AddController(rowSelect);


            //}
            //else if (txtGroupName.Text != "" && txtLedgerName.Text == "")
            //{
            //    groupid = AccountGroup.GetIDFromName(txtGroupName.Text.Trim(), Lang.English);
            //    //now add group id,name and type to source grid
            //    grdAllocation[rowcount, 3] = new SourceGrid.Cells.Cell("Group");
            //    grdAllocation[rowcount, 3].AddController(rowSelect);
            //    grdAllocation[rowcount, 4] = new SourceGrid.Cells.Cell(txtGroupName.Text.Trim());
            //    grdAllocation[rowcount, 4].AddController(rowSelect);
            //    grdAllocation[rowcount, 5] = new SourceGrid.Cells.Cell(groupid);
            //    grdAllocation[rowcount, 5].AddController(rowSelect);

            //}

            ////grdAllocation[rowcount, 2] = new SourceGrid.Cells.Cell("Ledger");
            ////if (cmbLedgerName.SelectedIndex >= 0)
            ////{
            ////    grdAllocation[rowcount, 3] = new SourceGrid.Cells.Cell((cmbLedgerName.Text).ToString());
            ////    grdAllocation[rowcount, 4] = new SourceGrid.Cells.Cell(cmbLedgerName.SelectedValue);
            ////}


            //grdAllocation[rowcount, 6] = new SourceGrid.Cells.Cell(txtAmountAssigned.Text.Trim());
            //grdAllocation[rowcount, 6].AddController(rowSelect);

            //grdAllocation[rowcount, 7] = new SourceGrid.Cells.Cell("0");
            //grdAllocation[rowcount, 7].AddController(rowSelect);

            ////txtAllocationTotal.Text = (Convert.ToDouble(txtAmountAssigned.Text) + Convert.ToDouble(txtAllocationTotal.Text)).ToString();

            //txtGroupName.Text = "";
            //txtLedgerName.Text = "";
            //txtAmountAssigned.Text = "0";

            //CalculateAllocationTotal();
        }
        #endregion
        //this function is updated so it might crete problem with previous references
        private void CalculateAllocationTotal()
        {
            try
            {
                if (grdAllocation.Rows.Count > 1)
                {
                    decimal total = 0;
                    for (int i = 1; i < grdAllocation.Rows.Count(); i++)
                    {
                        total += Convert.ToDecimal(grdAllocation[i, 7].Value);
                    }
                    txtAllocationTotal.Text = total.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                }
                else
                {
                    txtAllocationTotal.Text = "0";
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnToggleExpand_Click(object sender, EventArgs e)
        {
            if (IsExpand)
            {
                tvAccountHead.CollapseAll();
                IsExpand = false;
                btnToggleExpand.Text = "Expand";
            }
            else
            {
                tvAccountHead.ExpandAll();
                IsExpand = true;
                btnToggleExpand.Text = "Collapse";
            }
        }
        private void tvAccountHead_AfterSelect(object sender, TreeViewEventArgs e)
        {
            masterBudgetID = 0;
            if (e.Node.ForeColor == Color.DarkBlue)
            {
                txtGroupName.Text = "";
                txtLedgerName.Text = tvAccountHead.SelectedNode.Text;
                txtLedgerName.Focus();
            }
            else
            {
                txtLedgerName.Text = "";
                txtGroupName.Text = tvAccountHead.SelectedNode.Text;
                txtGroupName.Focus();
            }
        }

        //unusable now
        #region Old save Button code
        private void btnSave_Click(object sender, EventArgs e)
        {
            // if (cmbBudgetName.SelectedIndex < 1)
            // {
            //     MessageBox.Show("Please select budget name...");
            //     return;
            // }
            // //new change
            // int mid;
            // //get masterid of budget allocation from the selected budget name and start date
            // mid = bgt.getMasterBudgetIDByBgtID(Convert.ToInt32(cmbBudgetName.SelectedValue));

            // if (mid == 0)
            // {

            //     DateTime dtt = DateTime.Now;

            //     //check if ledgers or groups are added to the grid or not
            //     if (grdAllocation.Rows.Count > 1)
            //     {
            //         for (int i = 1; i < grdAllocation.Rows.Count; i++)
            //         {
            //             if (grdAllocation[i, 7].Value != "")
            //             {
            //                 fixedvalue = Convert.ToDecimal(grdAllocation[i, 7].Value);
            //             }
            //             dtbudgetdetail.Rows.Add(grdAllocation[i, 3].Value.ToString(), Convert.ToInt32(grdAllocation[i, 5].Value), Convert.ToDecimal(grdAllocation[i, 6].Value), Convert.ToDecimal(fixedvalue));
            //         }
            //     }
            //     else
            //     {
            //         MessageBox.Show("No ledgers and groups are allocated to budget...");
            //         return;
            //     }
            //     //this function is updated  and unusable in this form

            //     //save the budget allocation
            ////  bgt.saveBudgetAllocation(Convert.ToInt32(cmbBudgetName.SelectedValue), Convert.ToDecimal(txtAllocationTotal.Text), dtbudgetdetail);
            //     dtbudgetdetail.Clear();

            // }
            //     //editing the budget
            // else
            // {               
            //         //check if ledgers or groups are added to the grid or not
            //         if (grdAllocation.Rows.Count > 1)
            //         {
            //             for (int i = 1; i < grdAllocation.Rows.Count; i++)
            //             {
            //                 if (grdAllocation[i, 7].Value != "")
            //                 {
            //                     fixedvalue = Convert.ToDecimal(grdAllocation[i, 7].Value);
            //                 }
            //                 dtbudgetdetail.Rows.Add(grdAllocation[i, 3].Value.ToString(), Convert.ToInt32(grdAllocation[i, 5].Value), Convert.ToDecimal(grdAllocation[i, 6].Value), Convert.ToDecimal(fixedvalue));
            //             }
            //         }
            //         else
            //         {
            //             MessageBox.Show("No ledgers and groups are allocated to budget...");
            //             return;
            //         }

            //         DialogResult result = MessageBox.Show("Are you sure about updating the budget...,all old budget information will be deleted and new infromation will be saved...?", "CONFIRMATION", MessageBoxButtons.YesNo);
            //         if (result == DialogResult.Yes)
            //         {
            //             //unusable  after updation
            //             //update the budget
            //// bgt.UpdateBudgetAllocation(mid, Convert.ToInt32(cmbBudgetName.SelectedValue), Convert.ToDecimal(txtAllocationTotal.Text), dtbudgetdetail);
            //             dtbudgetdetail.Clear();

            //         }
            //     }

        }

        #endregion
        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbBudgetName.SelectedIndex = 0;
            cmbCopyBudget.SelectedIndex = 0;
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            Clear();

        }
        private void Clear()
        {
            txtAmountAssigned.Text = "";
            txtGroupName.Text = "";
            txtLedgerName.Text = "";
            txtAllocationTotal.Text = "0";
            masterBudgetID = 0;

            if (grdAllocation.Rows.Count > 1)
            {
                int j = grdAllocation.Rows.Count;
                for (int i = j - 1; i > 0; i--)
                {
                    grdAllocation.Rows.Remove(i);
                }
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        private void cmbBudgetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBudgetName.SelectedIndex > 0)
                {
                    Clear();
                    DataTable dtdate = bgt.getStartNEndDate(Convert.ToInt32(cmbBudgetName.SelectedValue));

                    if (dtdate.Rows.Count > 0)
                    {
                        int styear = 0;
                        int stmonth = 0;
                        int stday = 0;
                        int enyear = 0;
                        int enmonth = 0;
                        int enday = 0;
                        DateTime stdate = Convert.ToDateTime(dtdate.Rows[0][0].ToString());
                        Date.EngToNep(stdate, ref styear, ref stmonth, ref stday);
                        txtStartDate.Mask = Date.FormatToMask();
                        txtStartDate.Text = styear.ToString().PadLeft(4, '0') + "/" + stmonth.ToString().PadLeft(2, '0') + "/" + stday.ToString().PadLeft(2, '0');
                        DateTime endate = Convert.ToDateTime(dtdate.Rows[0][1].ToString());
                        Date.EngToNep(endate, ref enyear, ref enmonth, ref enday);
                        txtEndDate.Mask = Date.FormatToMask();
                        txtEndDate.Text = enyear.ToString().PadLeft(4, '0') + "/" + enmonth.ToString().PadLeft(2, '0') + "/" + enday.ToString().PadLeft(2, '0');
                    }
                    FillGridWithData();
                }
                else
                {
                    Clear();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void FillGridWithData()
        {
            //dt.Clear();
            Clear();
            dt = bgt.GetAllocationDataFromBudgetID(Convert.ToInt32(cmbBudgetName.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                //txtAllocationTotal.Text = dt.Rows[0][2].ToString();
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    grdAllocation.Rows.Insert(i);
                    grdAllocation[i, 1] = new SourceGrid.Cells.Cell(btnDocDelete);
                    grdAllocation[i, 1].AddController(evtGrdAlloctDelete);
                    grdAllocation[i, 2] = new SourceGrid.Cells.Cell(grdAllocation.Rows.Count - 1);
                    grdAllocation[i, 2].AddController(rowSelect);
                    grdAllocation[i, 3] = new SourceGrid.Cells.Cell(dt.Rows[i - 1][0]);
                    grdAllocation[i, 3].AddController(rowSelect);
                    grdAllocation[i, 4] = new SourceGrid.Cells.Cell(dt.Rows[i - 1][1]);
                    grdAllocation[i, 4].AddController(rowSelect);
                    grdAllocation[i, 5] = new SourceGrid.Cells.Cell(dt.Rows[i - 1][2]);
                    grdAllocation[i, 5].AddController(rowSelect);
                    grdAllocation[i, 6] = new SourceGrid.Cells.Cell(dt.Rows[i - 1][3]);
                    grdAllocation[i, 6].AddController(rowSelect);
                    grdAllocation[i, 7] = new SourceGrid.Cells.Cell(dt.Rows[i - 1][4]);
                    grdAllocation[i, 7].AddController(rowSelect);
                }
                CalculateAllocationTotal();
            }

            cmbCopyBudget.SelectedIndex = 0;
        }
        private void GridDelButton_Click(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
                int cr = context.Position.Row;
                DialogResult res = MessageBox.Show("Are you sure you want to delete '" + grdAllocation[cr, 5].Value.ToString() + "' Account From the budget '" + cmbBudgetName.Text + "'", "CONFIRMAYIN", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    bgt.DeleteAccountFromAllocation(Convert.ToInt32(grdAllocation[cr, 3].Value));
                    Clear();
                    FillGridWithData();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        #region Old cancel Button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //btnAllocate.Visible = true;
            //btnEdit.Visible = false;
            //btnCancel.Visible = false;
            //txtGroupName.Text = "";
            //txtLedgerName.Text = "";
            //txtAmountAssigned.Text = "";
        }
        #endregion
        //unuseable now
        #region old Delete code
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if(cmbBudgetName.SelectedIndex<1)
            //{
            //    MessageBox.Show("Budget name not selected...","MESSAGE");
            //    return;
            //}
            //bgt.DeleteBudget(Convert.ToInt32(cmbBudgetName.SelectedValue),cmbBudgetName.Text);
            //Clear();
        }
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (grdAllocation.Rows.Count > 1)
            {
                DialogResult res = MessageBox.Show("Do you want to continue without saving the changes...", "CONFIRM", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    this.Dispose();
                }
            }
            else
            {
                this.Dispose();
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {

            frmBudget fbgt = new frmBudget();
            fbgt.ShowDialog();
            cmbBudgetName.SelectedIndex = 0;
            LoadCurrentNUpcommingBudget();
        }
        private void grdAllocation_Paint(object sender, PaintEventArgs e)
        {

        }
        //unuseable now
        #region Old btnEdit Code
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //int ledgerid;
            //int groupid;
            //rowcount = var;
            ////checking wheather the edited group or ledger already exist under budget
            //for (int i = 1; i < grdAllocation.Rows.Count; i++)
            //{
            //    if (i == rowcount)
            //    {
            //        continue;
            //    }
            //    if (txtGroupName.Text == grdAllocation[i, 4].Value.ToString() || txtLedgerName.Text == grdAllocation[i, 4].Value.ToString())
            //    {
            //        MessageBox.Show("this ledger or group is already allocated to the budget " + " " + cmbBudgetName.Text);
            //        return;
            //    }
            //}
            ////validating the amount assigned
            //if (!UserValidation.validDecimal(txtAmountAssigned.Text.Trim()))
            //{
            //    MessageBox.Show("Invalid amount assigned.");
            //    return;
            //}
            //double prveamount = Convert.ToDouble(grdAllocation[var, 5].Value.ToString());

            ////if ledgername text has a ledger than get ledger id from the name else get group id
            //if (txtLedgerName.Text != "" && txtGroupName.Text == "")
            //{
            //    ledgerid = Ledger.GetLedgerIdFromName(txtLedgerName.Text.Trim(), Lang.English);
            //    //now add ledger id,name and type to source grid
            //    grdAllocation[rowcount, 3].Value = "Ledger";
            //    grdAllocation[rowcount, 4].Value = txtLedgerName.Text.Trim();
            //    grdAllocation[rowcount, 5].Value = ledgerid;

            //}
            //else if (txtGroupName.Text != "" && txtLedgerName.Text == "")
            //{
            //    groupid = AccountGroup.GetIDFromName(txtGroupName.Text.Trim(), Lang.English);
            //    //now add group id,name and type to source grid
            //    grdAllocation[rowcount, 3].Value = "Group";
            //    grdAllocation[rowcount, 4].Value = txtGroupName.Text.Trim();
            //    grdAllocation[rowcount, 5].Value = groupid;

            //}
            //grdAllocation[rowcount, 6].Value = txtAmountAssigned.Text.Trim();
            //grdAllocation[rowcount, 7].Value = "0";
            ////txtAllocationTotal.Text = "";
            ////for (int i = 1; i < grdAllocation.Rows.Count; i++)
            ////{
            ////    amount += Convert.ToDecimal(grdAllocation[i, 6].Value);
            ////}
            ////txtAllocationTotal.Text = amount.ToString();
            //btnAllocate.Visible = true;
            //btnEdit.Visible = false;
            //btnCancel.Visible = false;
            //txtGroupName.Text = "";
            //txtLedgerName.Text = "";
            //txtAmountAssigned.Text = "";

            //CalculateAllocationTotal();
        }
        #endregion

        private void grdListView_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ListItem m_SearchInItem = (ListItem)cboSrchSearchIn1.SelectedItem;
            ListItem li1 = (ListItem)cboSrchOP1.SelectedItem;
            ListItem li2 = (ListItem)cboSrchOP2.SelectedItem;
            if (li2 == null)
                li2 = new ListItem(0, "");//Begins_With
            m_SearchIn = (SearchIn)m_SearchInItem.ID; //Set the private function searchIn so that gridclick() may know what is the current mode.
            try
            {
                Search((SearchIn)m_SearchInItem.ID, (SearchOp)li1.ID, txtSrchParam1.Text, (SearchOp)li2.ID, txtSrchParam2.Text);
                tabDisplay.SelectedIndex = 1; //Select the listview
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void btnForgetSearch_Click(object sender, EventArgs e)
        {
            m_SearchIn = SearchIn.Account_Groups;
            Search(SearchIn.Account_Groups, SearchOp.Begins_With, "", SearchOp.Equals, "");
            cboSrchSearchIn1.SelectedIndex = 0;
            cboSrchOP1.SelectedIndex = 0;
            txtSrchParam1.Text = "";
        }

        private void groupBox3_Enter_1(object sender, EventArgs e)
        {

        }
        private void btnAmount_Click(object sender, EventArgs e)
        {
            int budgetID = 0;
            int accID = 0;
            string accType = "";
            try
            {
                if (cmbBudgetName.SelectedIndex > 0)
                {
                    if (masterBudgetID <= 0)
                    {
                        for (int i = 1; i < grdAllocation.Rows.Count; i++)
                        {
                            if (txtGroupName.Text == grdAllocation[i, 5].Value.ToString() && grdAllocation[i, 6].Value.ToString() == "Group" || txtLedgerName.Text == grdAllocation[i, 5].Value.ToString() && grdAllocation[i, 6].Value.ToString() == "Ledger")
                            {
                                MessageBox.Show("This ledger or group is already allocated to the budget " + " " + cmbBudgetName.Text);
                                txtGroupName.Text = "";
                                txtLedgerName.Text = "";
                                return;
                            }
                        }
                    }

                    budgetID = Convert.ToInt32(cmbBudgetName.SelectedValue);
                    //if ledgername text has a ledger than get ledger id from the name else get group id
                    if (txtLedgerName.Text != "" && txtGroupName.Text == "")
                    {
                        accID = Ledger.GetLedgerIdFromName(txtLedgerName.Text.Trim(), Lang.English);
                        accType = "Ledger";
                    }
                    else if (txtLedgerName.Text == "" && txtGroupName.Text != "")
                    {
                        accID = AccountGroup.GetIDFromName(txtGroupName.Text.Trim(), Lang.English);
                        accType = "Group";

                    }
                    else
                    {
                        Global.MsgError("Please select a Ledger or Group in the Tree View first.");
                        return;
                    }
                    frmBudgetBalanceEntry bbe = new frmBudgetBalanceEntry(masterBudgetID, budgetID, accID, accType);
                    bbe.ShowDialog();
                    Clear();
                    FillGridWithData();
                }
                else
                {
                    Global.Msg("No Budget name selected ");
                    return;
                }
            }

            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        private void cmbCopyBudget_SelectedIndexChanged(object sender, EventArgs e)
        {
            
          
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (cmbCopyBudget.SelectedIndex > 0)
            {
                if (cmbBudgetName.SelectedIndex <= 0)
                {
                    Global.Msg("No Budget Name is selected");
                    return;
                }

            DialogResult result= Global.MsgQuest("All records of budget " + cmbBudgetName.Text + " will be replaced by the records of " + cmbCopyBudget.Text + "\n Are you sure you want to continue? ");
            if (result == DialogResult.Yes)
            {
                if (Convert.ToInt32(cmbCopyBudget.SelectedValue) == Convert.ToInt32(cmbBudgetName.SelectedValue))
                    return;
                try
                {
                    bool i = Budget.CopyBudget(Convert.ToInt32(cmbCopyBudget.SelectedValue), Convert.ToInt32(cmbBudgetName.SelectedValue));
                    if (i)
                    {
                        FillGridWithData();
                    }
                }
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
                }
            }
            }

        }

        private void btnDeleteAllAlloc_Click(object sender, EventArgs e)
        {
            if (cmbBudgetName.SelectedIndex <= 0)
            {
                Global.Msg("No Budget name selected.");
                return;
            }
            DialogResult result = Global.MsgQuest("Are you sure you want to delete all allocation account of budget " + cmbBudgetName.Text);
            if (result == DialogResult.Yes)
            {
                try
                {
                    bgt.DeleteBudgetAllAllocation(Convert.ToInt32(cmbBudgetName.SelectedValue));
                    Global.Msg("Budget " + cmbBudgetName.Text + " deleted successfully");
                }
                catch (Exception ex)
                {
                    Global.Msg("Problem while deleting budget");
                    return;
                }
                FillGridWithData();
            }
        }

    }
}
