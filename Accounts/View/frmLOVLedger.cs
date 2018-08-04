using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace Accounts
{
    public interface ILOVLedger
    {
        void AddLedger(string LedgerName, int LedgerID, string CurrentBalance, bool IsSpecialKey, string ActualBal, string LedgerType);
    }

    public partial class frmLOVLedger : Form,IfrmAccountLedger //, SourceGrid.Cells.Controllers.ControllerBase
    {
        private string Contra = " ";
        private ArrayList aryTest = new ArrayList();
        private Form m_Parent; //Holds the parent form
        //private bool IsSpecialKey = false;
        private ILOVLedger m_ParentForm; //holds the selected CCY Code
        private int DefaultAccClass;
        ////Check to see if something has been successfully selected
        private bool isSelected = false;
        private Accounts.Model.dsLOVDetails dsLOVDetails = new Accounts.Model.dsLOVDetails();

        private string FilterString = "";
        private DataRow[] drFound;
        private DataTable tempTable;
        private DataTable dTable;
        
        private DataView mView;

        private SourceGrid.Cells.Controllers.CustomEvents dblClick; //Double click event holder
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;
        DataTable dtGetOpeningBalance = new DataTable();

        //private extern SourceGrid.Cells.Controllers.ControllerBase KeyPressController();
        public frmLOVLedger()
        {
            InitializeComponent();

        }

        public frmLOVLedger(Form ParentForm)
        {
            InitializeComponent();
            m_ParentForm = (ILOVLedger)ParentForm;           
            ////Set the selected font to everything
            this.Font = LangMgr.GetFont();
        }

        public frmLOVLedger(Form ParentForm,string contratext)
        {
            Contra = contratext;
            InitializeComponent();
            Contra = contratext;
            m_ParentForm = (ILOVLedger)ParentForm;
            ////Set the selected font to everything
            this.Font = LangMgr.GetFont();

        }

        public frmLOVLedger(Form ParentForm, KeyEventArgs e)
        {
            InitializeComponent();
            m_ParentForm = (ILOVLedger)ParentForm;
            txtLedgerName.Focus();
            string[] test = { "1" };
            dTable = Ledger.GetLedgerDetail(test[0].ToString(), null, null,0);
            //dTable = Currency.GetCurrencyTable();
            drFound = dTable.Select(FilterString);

            if (e.KeyData.ToString().Length == 1)
            {
                txtLedgerName.Text += e.KeyData.ToString().ToUpper();
                txtLedgerName.Text = txtLedgerName.Text.ToUpper();
                txtLedgerName.SelectionStart = txtLedgerName.Text.Length + 1;
            }
            //Set the selected font to everything
            this.Font = LangMgr.GetFont();           
        }

        public frmLOVLedger(Form ParentForm, KeyEventArgs e,string contratext)
        {
            InitializeComponent();
            Contra = contratext;
            m_ParentForm = (ILOVLedger)ParentForm;
            txtLedgerName.Focus();
            string[] test = { "1" };
            dTable = Ledger.GetLedgerDetail(test[0].ToString(), null, null, 0);
            //dTable = Currency.GetCurrencyTable();
            drFound = dTable.Select(FilterString);

            if (e.KeyData.ToString().Length == 1)
            {
                txtLedgerName.Text += e.KeyData.ToString().ToUpper();
                txtLedgerName.Text = txtLedgerName.Text.ToUpper();
                txtLedgerName.SelectionStart = txtLedgerName.Text.Length + 1;
            }
            //Set the selected font to everything
            this.Font = LangMgr.GetFont();
        }
        
        //DataTable dt = new DataTable("TblListOfLedger");
        private void frmLOVLedger_Load(object sender, EventArgs e)
        {
           
            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(dgListOfLedger_DoubleClick);
            gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();          
            gridKeyDown.KeyDown += new KeyEventHandler(Handle_KeyDown);
            //LOVGrid.Controller.AddController(new KeyPressController());
            dgListOfLedger.Controller.AddController(gridKeyDown);
            //Let the whole row to be selected
            dgListOfLedger.SelectionMode = SourceGrid.GridSelectionMode.Row;
            
            //Disable multiple selection
            dgListOfLedger.Selection.EnableMultiSelection = false;

            FillLedgerDetails();
            this.ActiveControl = txtLedgerName;
        }


        private void FillLedgerDetails()
         {
            #region NEW ONE
            try
            {
                    //string AccClassIDsXMLString = ReadAllAccClassID();
                   // string ProjectIDsXMLString = ReadAllProjectID();
                    string AccClassId = "<AccClassIDSettings><AccClassID>1</AccClassID></AccClassIDSettings>";
                    string ProjectId = "<ProjectIDCollection><ProjectID>1</ProjectID></ProjectIDCollection>";
                    DateTime dtime = Convert.ToDateTime("2014/02/05");
                    DataTable dtLedgerDetails = Ledger.GetLedgerDetails1(AccClassId, ProjectId, null, null, null,0, null);

#region  Temporary table to hold data with calculation Current balance
                    tempTable = new DataTable();
                    tempTable.Columns.Add("LedgerCode", typeof(string)); 
                    tempTable.Columns.Add("LedgerName", typeof(string));
                    tempTable.Columns.Add("LedgerID", typeof(int));
                    tempTable.Columns.Add("GroupName", typeof(string));
                    tempTable.Columns.Add("GroupID", typeof(int));
                    tempTable.Columns.Add("Balance", typeof(string));
                    tempTable.Columns.Add("ActualBalance", typeof(string));
                    tempTable.Columns.Add("LedgerType", typeof(string));


 #endregion

#region loop for currentbalance calculation
                    foreach (DataRow dr in dtLedgerDetails.Rows)
                    {
                        //decimal Debit = 0;
                        //decimal Credit = 0;
                        //decimal DebitOpeningBal = 0;
                        //decimal CreditOpeningBal = 0;
                        //if (!(dr["DebitTotal"] is DBNull))
                        //{
                        //    Debit = Convert.ToDecimal(dr["DebitTotal"]);
                        //}
                        //else
                        //    Debit = 0;

                        //if (!(dr["CreditTotal"] is DBNull))
                        //{
                        //    Credit = Convert.ToDecimal(dr["CreditTotal"]);
                        //}
                        //else
                        //    Credit = 0;

                        //if (!(dr["OpenBalDr"] is DBNull))
                        //{
                        //    DebitOpeningBal = Convert.ToDecimal(dr["OpenBalDr"]);
                        //}
                        //else
                        //    DebitOpeningBal = 0;

                        //if (!(dr["OpenBalCr"] is DBNull))
                        //{
                        //    CreditOpeningBal = Convert.ToDecimal(dr["OpenBalCr"]);
                        //}
                        //else
                        //    CreditOpeningBal = 0;

                        //decimal DebitTotal = Debit + DebitOpeningBal;
                        //decimal CreditTotal = Credit + CreditOpeningBal;
                        string strBalance = "";
                        decimal Balance = Convert.ToDecimal(dr["FinalBal"]);
                        if(dr["DrCr"].ToString()=="DEBIT")
                        {
                            strBalance=Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) +" (Dr.)";
                            if (dr["LedgerType"].ToString() == "CR")
                                Balance = 0 - Balance;
                        }
                        else if(dr["DrCr"].ToString()=="CREDIT")
                        {
                         strBalance=Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)) +" (Cr.)";
                         if (dr["LedgerType"].ToString() == "DR")
                             Balance = 0 - Balance;
                        }
                        else
                        {
                          strBalance=Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        }

                        //If +ve is present, show as Dr
                        //strBalance = ((Balance < 0) ? Balance * -1 : Balance).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                        //if (Balance >= 0)
                        //    strBalance = strBalance + " (Dr.)";
                        //else //If balance is -ve, its Cr.
                        //    strBalance = strBalance + " (Cr.)";

                        tempTable.Rows.Add(dr["LedgerCode"].ToString(), dr["LedgerName"].ToString(), Convert.ToInt32(dr["LedgerID"]), dr["GroupName"].ToString(), Convert.ToInt32(dr["GroupID"]), strBalance, Balance.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)), dr["LedgerType"].ToString());
                    }
 #endregion

                    System.Data.DataView view = new System.Data.DataView(tempTable);
                    System.Data.DataTable selected = view.ToTable("tblLOVLedgerDetails", false,"LedgerCode", "LedgerName", "LedgerID", "GroupName", "GroupID", "Balance","ActualBalance","LedgerType");
                        
                
                // Create a table from the query.
             // DataTable boundTable = query.CopyToDataTable<DataRow>();
        
                #region TEST for datagrid binding
              
                mView = selected.DefaultView;
               dgListOfLedger.FixedRows =1; //Allocated for Header
               dgListOfLedger.FixedColumns =0;

                //Header row
              //  dgListOfLedger.Columns.Insert( 0,SourceGrid.DataGridColumn.CreateRowHeader(dgListOfLedger));//this Create one balnk extra column in the very first of a table
                DevAge.ComponentModel.IBoundList bindList = new DevAge.ComponentModel.BoundDataView(mView);

                //Create default columns
                CreateColumns(dgListOfLedger.Columns, bindList);
                dgListOfLedger.DataSource = bindList;

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw ex;
            }
            #endregion  

        }

        private void CreateColumns(SourceGrid.DataGridColumns columns,
                                   DevAge.ComponentModel.IBoundList bindList)
        {
            SourceGrid.Cells.Editors.TextBoxNumeric numericEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(decimal));
            //numericEditor.TypeConverter = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(decimal), "N");
            numericEditor.AllowNull = true;

            //Borders
            DevAge.Drawing.RectangleBorder border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Azure), new DevAge.Drawing.BorderLine(Color.Azure));

            //Standard Views
            SourceGrid.Cells.Views.Link viewLink = new SourceGrid.Cells.Views.Link();
            viewLink.BackColor = Color.Aqua;
            viewLink.Border = border;
          
            viewLink.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            viewLink.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            SourceGrid.Cells.Views.Cell viewString = new SourceGrid.Cells.Views.Cell();
            viewString.BackColor = Color.Aqua;
            viewString.Border = border;
            viewString.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            SourceGrid.Cells.Views.Cell viewNumeric = new SourceGrid.Cells.Views.Cell();
            viewNumeric.BackColor = Color.Aqua;
            viewNumeric.Border = border;
            viewNumeric.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            SourceGrid.Cells.Views.Cell viewImage = new SourceGrid.Cells.Views.Cell();
            viewImage.BackColor = Color.Aqua;
            viewImage.Border = border; 
            viewImage.ImageStretch = false;
            viewImage.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            //Create columns
            SourceGrid.DataGridColumn gridColumn;



            gridColumn = dgListOfLedger.Columns.Add("LedgerCode", "Ledger Code", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 85;


            gridColumn = dgListOfLedger.Columns.Add("LedgerName", "Ledger Name", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 157;




            gridColumn = dgListOfLedger.Columns.Add("LedgerID", "LedgerID", typeof(int));
            gridColumn.DataCell.Editor = numericEditor;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 70;
            gridColumn.Visible = false;


            gridColumn = dgListOfLedger.Columns.Add("GroupName", "Group Name", typeof(string));
            gridColumn.DataCell.View = viewString;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 110;

       
            gridColumn = dgListOfLedger.Columns.Add("GroupID", "GroupID", typeof(int));
            gridColumn.DataCell.Editor = numericEditor;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 70;
            gridColumn.Visible = false;


            gridColumn = dgListOfLedger.Columns.Add("Balance", "Current Balance", typeof(decimal));
            gridColumn.DataCell.Editor = numericEditor;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 120;

            gridColumn = dgListOfLedger.Columns.Add("ActualBalance", "Actual Balance", typeof(decimal));
            gridColumn.DataCell.Editor = numericEditor;
            gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 120;
            gridColumn.Visible = false;

            gridColumn = dgListOfLedger.Columns.Add("LedgerType", "LedgerType", typeof(string));
           // gridColumn.DataCell.Editor = numericEditor;
          //  gridColumn.DataCell.View = viewNumeric;
            gridColumn.DataCell.AddController(dblClick);
            gridColumn.DataCell.AddController(gridKeyDown);
            gridColumn.Width = 70;
           gridColumn.Visible = false;


            //Create a conditional view
            foreach (SourceGrid.DataGridColumn col in columns)
            {
                SourceGrid.Conditions.ICondition condition =
                    SourceGrid.Conditions.ConditionBuilder.AlternateView(col.DataCell.View,
                                                                         Color.LightGray, Color.Black);
                col.Conditions.Add(condition);
            }
        }



        //Filters the datatable with the parameter name
        private void Filter()
        {

            this.FilterString = "LedgerName LIKE '" + txtLedgerName.Text + "%' AND LedgerCode LIKE '" + txtLedgerCode.Text + "%' AND GroupName LIKE '" + txtGroupName.Text + "%'";

            try
            {

                drFound = tempTable.Select(this.FilterString);
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

            //Temporary table to hold data with calculation Current balance
            DataTable filterTable = new DataTable();
            filterTable.Columns.Add("LedgerCode", typeof(string));
            filterTable.Columns.Add("LedgerName", typeof(string));
            filterTable.Columns.Add("LedgerID", typeof(int));
            filterTable.Columns.Add("GroupName", typeof(string));
            filterTable.Columns.Add("GroupID", typeof(int));
            filterTable.Columns.Add("Balance", typeof(string));
            filterTable.Columns.Add("ActualBalance", typeof(string));
            filterTable.Columns.Add("LedgerType", typeof(string));

            foreach(DataRow dr in drFound)
            {
                filterTable.ImportRow(dr);
            }
            //LOVGrid.Rows.Clear();

            System.Data.DataView view = new System.Data.DataView(filterTable);
            System.Data.DataTable selected = view.ToTable("tblLOVLedgerDetails", false, "LedgerCode", "LedgerName", "LedgerID", "GroupName", "GroupID", "Balance", "ActualBalance", "LedgerType");


            //DataView mView;
            mView = selected.DefaultView;

            //Header row
           // dgListOfLedger.Columns.Insert(0, SourceGrid.DataGridColumn.CreateRowHeader(dgListOfLedger));
            DevAge.ComponentModel.IBoundList bindList = new DevAge.ComponentModel.BoundDataView(mView);


            dgListOfLedger.DataSource = bindList;


        }
    
        private void txtLedgerNo_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void txtLedgerCode_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void txtLedgerName_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void txtGroupID_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }


        private void frmLOVLedger_KeyDown(object sender, KeyEventArgs e)
        {
            Handle_KeyDown(sender, e);
        }

        public void Handle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                isSelected = false;
               // m_ParentForm.AddLedger("", 0, isSelected);
                this.Close();
                //m_ParentForm.AddLedger("");
            }

            if (e.KeyCode == Keys.Down)
            {
                
                //Focus is on textboxes, select the gridview's first row
                if(!this.dgListOfLedger.Focused)
                    this.dgListOfLedger.Selection.Focus(new SourceGrid.Position(1, 1), true);


                
            }

            else if (e.KeyCode == Keys.Up)
            {
                if (!this.dgListOfLedger.Focused)
                    this.dgListOfLedger.Selection.Focus(new SourceGrid.Position(1, 1), true);
               
            }
            else
            {
                char key = (char)e.KeyData;

                if (char.IsLetterOrDigit(key))
                {
                    if (this.txtLedgerCode.Focused || this.txtGroupName.Focused)
                    {
                        return;
                    }
                    if (this.txtLedgerCode.Text.Trim().Length > 0)
                    {
                        this.txtLedgerCode.Focus();
                        this.txtLedgerCode.Text += key.ToString().ToUpper();
                        txtLedgerCode.Text = txtLedgerCode.Text.ToUpper();
                        this.txtLedgerCode.SelectionStart = this.txtLedgerCode.Text.Length + 1;
                        return;
                    }
                    if (this.txtGroupName.Text.Trim().Length > 0)
                    {
                        this.txtGroupName.Focus();
                        this.txtGroupName.Text += key.ToString().ToUpper();
                        txtGroupName.Text = txtGroupName.Text.ToUpper();
                        this.txtGroupName.SelectionStart = this.txtGroupName.Text.Length + 1;
                        return;
                    }                  
                    if (!this.txtLedgerName.Focused)
                    {
                        this.txtLedgerName.Focus();                        
                        this.txtLedgerName.Text += key.ToString().ToUpper();
                        txtLedgerName.Text = txtLedgerName.Text.ToUpper();
                        this.txtLedgerName.SelectionStart = this.txtLedgerName.Text.Length + 1;
                    }
                }                                          
            }
        }

        private void txtLedgerCode_KeyDown(object sender, KeyEventArgs e)
        {
            Handle_KeyDown(sender, e);
        }

        private void txtLedgerName_KeyDown(object sender, KeyEventArgs e)
        {
            Handle_KeyDown(sender, e);
        }


        private void txtGroupName_KeyDown(object sender, KeyEventArgs e)
        {
            Handle_KeyDown(sender, e);
        }

        private void txtLedgerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void txtGroupName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void txtLedgerCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void chkDisplayZeroBalance_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDisplayZeroBalance.Checked)
                Filter();
            if (!chkDisplayZeroBalance.Checked)
                Filter();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            frmAccountSetup frm = new frmAccountSetup(this);
            frm.Show();
        }

        public void AddAccountLedger(string ledgerName)
        {
            try
            {
                //string strTest = "";
                //aryTest.Add(1.ToString());
                //strTest = string.Join("," , aryTest.ToArray(GetType(string))); //GetType(string))); /// String.Join(",", aryTest.ToArray(GetType(string)));
                string[] test = { "1" };
                dTable = Ledger.GetLedgerDetail(test[0].ToString(), null, null, 0);

                drFound = dTable.Select(FilterString);

                //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
                gridKeyDown.KeyDown += new KeyEventHandler(Handle_KeyDown);
                //LOVGrid.Controller.AddController(new KeyPressController());
                this.dgListOfLedger.Controller.AddController(gridKeyDown);
                //Let the whole row to be selected
                dgListOfLedger.SelectionMode = SourceGrid.GridSelectionMode.Row;

                //Disable multiple selection
                dgListOfLedger.Selection.EnableMultiSelection = false;

                //Finally fill all the values in the grid with no filter applied
                //FillGrid();

                this.ActiveControl = txtLedgerName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int GetRootAccClassID()
        {
            if (DefaultAccClass > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(DefaultAccClass));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);

            }
            return 1;//The default root class ID
        }

        private void dgListOfLedger_DoubleClick(object sender, EventArgs e)
            {
            try
            {
                isSelected = true;
                // IsSpecialKey = true;
                //Get the Selected Row
                int CurRow = dgListOfLedger.Selection.GetSelectionRegion().GetRowsIndex()[0];  
                SourceGrid.CellContext LedgerName = new SourceGrid.CellContext(dgListOfLedger, new SourceGrid.Position(CurRow, 1));
                SourceGrid.CellContext LedgerID = new SourceGrid.CellContext(dgListOfLedger, new SourceGrid.Position(CurRow, 2));
                SourceGrid.CellContext DebitTotal = new SourceGrid.CellContext(dgListOfLedger, new SourceGrid.Position(CurRow, 5));
                SourceGrid.CellContext ActualBalance = new SourceGrid.CellContext(dgListOfLedger, new SourceGrid.Position(CurRow, 6));
                SourceGrid.CellContext LedgerType = new SourceGrid.CellContext(dgListOfLedger, new SourceGrid.Position(CurRow, 7));
                string ActualBal = "0";
                string LdgType = " ";
                if(ActualBalance.Value==null)
                {
                    ActualBal = "0";
                }
                else
                {
                    ActualBal = ActualBalance.Value.ToString();
                }
                if (LedgerType.Value == null)
                {
                    LdgType = " ";
                }
                else
                {
                    LdgType = LedgerType.Value.ToString();
                }

                if (DebitTotal.Value == null)
                {
                    string Debittotal = "0";
                    if (LedgerName.Value == null && LedgerID.Value == null && Debittotal == "0")
                    {

                        return;

                    }
                    m_ParentForm.AddLedger(LedgerName.Value.ToString(), Convert.ToInt32(LedgerID.Value), Debittotal, isSelected, ActualBal, LdgType);
                }
                else
                //Call the interface function to add the text in the parent form container
                m_ParentForm.AddLedger(LedgerName.Value.ToString(), Convert.ToInt32(LedgerID.Value), DebitTotal.Value.ToString(), isSelected,ActualBal,LdgType);
                this.Close();
            }
            catch (Exception ex)
            {
                Global.Msg("Invalid selection. Message:" + ex.Message);
            }
        }

        private void frmLOVLedger_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSelected)
            {
                isSelected = false;
                m_ParentForm.AddLedger("", 0, "", isSelected,"0"," ");
                this.Hide();
                this.Parent = null;
                e.Cancel = true;  
            }
        }

        private void dgListOfLedger_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                dgListOfLedger_DoubleClick(sender, null);  
        }

        private void dgListOfLedger_Paint(object sender, PaintEventArgs e)
        {

        }

       
       
    }
   
}
