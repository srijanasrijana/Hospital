using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using BusinessLogic;
using DateManager;
using Inventory.DataSet;
using Inventory.CrystalReports;
using ErrorManager;
using System.IO;

namespace Inventory.Forms.Accounting
{
    public partial class frmmultipleledger : Form
    {
         DevAge.Windows.Forms.DevAgeTextBox ctx;
         SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
         SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
         SourceGrid.Cells.Controllers.CustomEvents evtCodeFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
         private enum GridColumn : int
         {
             Del = 0, LedgerCode, LedgerName
         };
        public frmmultipleledger()
        {
            InitializeComponent();
        }

        private void btnLdrCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ListAccGroup(ComboBox ComboBoxControl)
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
            DataTable dt = AccountGroup.GetGroupTable(-1);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                //Check if it is Ledger's tab's parent accounting group
                if (ComboBoxControl == cboLdrAcGroup)
                {
                    //Chop off the Assets, Liabilities, Income and Expenditure
                    try
                    {
                        if (Convert.ToInt32(dr["GroupNumber"]) > 0 && Convert.ToInt32(dr["GroupNumber"]) <= 4)
                            continue;
                    }
                    catch { }
                }


                ComboBoxControl.Items.Add(new ListItem((int)dr["GroupID"], dr[LangField].ToString()));

            }

            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";

        }

        private void frmmultipleledger_Load(object sender, EventArgs e)
        {
            ListAccGroup(cboLdrAcGroup);
            evtDelete.Click += new EventHandler(Delete_Row_Click);
            evtCodeFocusLost.FocusLeft += new EventHandler(Code_Focus_Lost);
            grdmultipleledger.Redim(2, 3);
            AddGridHeader();
            AddRowLedger(1);
        }

        private void btnLdrSave_Click(object sender, EventArgs e)
        {
            DataTable ledgerlist = new System.Data.DataTable();
            ledgerlist.Columns.Add("LedgerCode");
            ledgerlist.Columns.Add("LedgerName");

            //string s = grdmultipleledger[1, 1].Value.ToString();
            for (int i = 0; i < grdmultipleledger.Rows.Count - 2; i++)
            {
                ledgerlist.Rows.Add(grdmultipleledger[i + 1, (int)GridColumn.LedgerCode].Value, grdmultipleledger[i + 1, (int)GridColumn.LedgerName].Value);
            }
            string ledgerhead = cboLdrAcGroup.Text;
            BusinessLogic.Accounting.MultipleLedger ml = new BusinessLogic.Accounting.MultipleLedger();
            ml.CreateMultipleLedger(ledgerlist, ledgerhead);
            //Global.Msg("Ledger created successfully!");
            ClearVoucher();

        }
        private void AddGridHeader()
        {
            grdmultipleledger[0, (int)GridColumn.Del] = new MyHeader("Delete");   
            grdmultipleledger[0, (int)GridColumn.Del].Column.Width = 50;
            grdmultipleledger[0, (int)GridColumn.LedgerCode] = new MyHeader("Ledger Code");    
            grdmultipleledger[0, (int)GridColumn.LedgerCode].Column.Width = 100;
            grdmultipleledger[0, (int)GridColumn.LedgerName] = new MyHeader("Ledger Name");
            grdmultipleledger[0, (int)GridColumn.LedgerName].Column.Width = 100;
            //grdmultipleledger[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
            //grdmultipleledger[0, 1] = new SourceGrid.Cells.ColumnHeader("Ledger Code");
            //grdmultipleledger[0, 2] = new SourceGrid.Cells.ColumnHeader("Ledger Name");
         
     
            
            
        }
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
        public void AddRowLedger(int RowCount)
        {
            grdmultipleledger.Redim(Convert.ToInt32(RowCount + 1), grdmultipleledger.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;
            grdmultipleledger[i, (int)GridColumn.Del] = btnDelete;
            grdmultipleledger[i, (int)GridColumn.Del].AddController(evtDelete);

            SourceGrid.Cells.Editors.TextBox ledgercode = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            grdmultipleledger[i, (int)GridColumn.LedgerCode] = new SourceGrid.Cells.Cell("", ledgercode);
            grdmultipleledger[i, (int)GridColumn.LedgerCode].Value = "(NEW)";
          

            SourceGrid.Cells.Editors.TextBox ledgername = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            grdmultipleledger[i, (int)GridColumn.LedgerName] = new SourceGrid.Cells.Cell("", ledgername);
            grdmultipleledger[i, (int)GridColumn.LedgerName].Value = "";
            grdmultipleledger[i, (int)GridColumn.LedgerCode].AddController(evtCodeFocusLost);
            
        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //Do not delete if its the last Row 
            if (ctx.Position.Row <= grdmultipleledger.RowsCount - 2)
                grdmultipleledger.Rows.Remove(ctx.Position.Row);
        }

        private void Code_Focus_Lost(object sender, EventArgs e)
        {
            int RowCount = grdmultipleledger.RowsCount;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            int CurRow = ctx.Position.Row;
            string AccName = (string)(grdmultipleledger[RowCount - 1, 1].Value);
            if (AccName !="(NEW)")
            {
            AddRowLedger(RowCount);
            }
        }
        private void ClearVoucher()
        {
            ClearLedger();
            grdmultipleledger.Redim(2, 3);
            AddGridHeader(); //Write header part
            AddRowLedger(1);
        }
        private void ClearLedger()
        {
            grdmultipleledger.Rows.Clear();
            cboLdrAcGroup.Text = string.Empty;
        }
    }
}
