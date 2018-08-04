using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Accounts.View
{
    public partial class frmBudgetBalanceEntry : Form
    {
        public frmBudgetBalanceEntry()
        {
            InitializeComponent();
        }
        int budgetMasterID = 0;
        int budgetID=0;
        int accID=0;
        string accType="";
      DataTable dtGlobal=new DataTable();
        DataTable dtbudgetdetail = new DataTable();
        BusinessLogic.Accounting.Budget bgt = new BusinessLogic.Accounting.Budget();


        public frmBudgetBalanceEntry(int budgetMasterID, int budgetID, int accID, string accType)
        {
            InitializeComponent();
            this.budgetMasterID = budgetMasterID;
            this.budgetID = budgetID;
            this.accID = accID;
            this.accType = accType;

        }

        private void frmBudgetBalanceEntry_Load(object sender, EventArgs e)
        {
            //Disable multiple selection
            grdBudgetBalnce.Selection.EnableMultiSelection = false;
            grdBudgetBalnce.Redim(1, 5);


            //Prepare the header part for grid
            AddGridHeader();
            AddRowBudgetBalance(1);
            LoadAccountClassWithBudgetAmount();
            addColumnToBudgetDetailTable();
        }

        private void AddGridHeader()
        {
            grdBudgetBalnce[0, 0] = new SourceGrid.Cells.ColumnHeader("S.N.");
            grdBudgetBalnce[0, 1] = new SourceGrid.Cells.ColumnHeader("Account Class");
            grdBudgetBalnce[0, 2] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdBudgetBalnce[0, 3] = new SourceGrid.Cells.ColumnHeader("AccClassID");
            grdBudgetBalnce[0, 0].Column.Width = 50;
            grdBudgetBalnce[0, 1].Column.Width = 200;
            grdBudgetBalnce[0, 2].Column.Width = 120;
          //  grdBudgetBalnce[0, 3].Column.Width = 100;
            grdBudgetBalnce.Columns[3].Visible = false;
        }

        private void AddRowBudgetBalance(int RowCount)
        {
            try
            {

                //Add a new row
                grdBudgetBalnce.Redim(Convert.ToInt32(RowCount + 1), grdBudgetBalnce.ColumnsCount);
                int i = RowCount;
                grdBudgetBalnce[i, 0] = new SourceGrid.Cells.Cell(i.ToString());
                grdBudgetBalnce[i, 1] = new SourceGrid.Cells.Cell("");

                SourceGrid.Cells.Editors.TextBox txtBudgetAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtBudgetAmount.EditableMode = SourceGrid.EditableMode.Focus;
                grdBudgetBalnce[i, 2] = new SourceGrid.Cells.Cell("0", txtBudgetAmount);

                grdBudgetBalnce[i, 3] = new SourceGrid.Cells.Cell("");

            }
            catch (Exception ex)
            {
                
              Global.Msg(ex.Message);

            }

        }
        private void LoadAccountClassWithBudgetAmount()
        {
            #region Language Management

            grdBudgetBalnce.Font = LangMgr.GetFont();

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


            try
            {
                DataTable dt;
                
                dt = AccountClass.GetRootAccClass(-1);
                if(budgetMasterID>0)
                {
                    dtGlobal = bgt.GetClassIDnAmount(budgetMasterID);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //Accounting Class Name
                    grdBudgetBalnce[i + 1, 1].Value = dr[LangField].ToString();
                    grdBudgetBalnce[i + 1, 3].Value = dr["AccClassID"].ToString();

                    if(dtGlobal.Rows.Count>0)
                    {
                        for (int j = 0; j < dtGlobal.Rows.Count; j++)
                        {
                            if (dr["AccClassID"].ToString() == dtGlobal.Rows[j][0].ToString())
                            {
                                grdBudgetBalnce[i + 1, 2].Value = dtGlobal.Rows[j][1].ToString();
                            }
                        }
                    }
                    // we don't need to add new row after last row is added
                    if (i < dt.Rows.Count - 1)
                        AddRowBudgetBalance(grdBudgetBalnce.RowsCount);  
              

                }

            }
            catch (Exception ex)
            {
              Global.Msg(ex.Message); 
            }

        }


        private void addColumnToBudgetDetailTable()
        {
            dtbudgetdetail.Columns.Add("ClassID");
            dtbudgetdetail.Columns.Add("Amount");
            

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal sum = 0;
            try
            {
                FormHandle m_FHandle = new FormHandle();

                for(int i=1;i<grdBudgetBalnce.Rows.Count;i++)
                {
                    //if cell is empty then assign value 0
                    if (grdBudgetBalnce[i,2].Value.ToString().Length == 0)
                        grdBudgetBalnce[i, 2].Value = "0";

                    //validating all amount cell
                    txtValidateAmount.Text = grdBudgetBalnce[i, 2].ToString();
                    if (txtValidateAmount.Text.Trim().Length == 0)
                        txtValidateAmount.Text = "0";


                    m_FHandle.AddValidate(txtValidateAmount, DType.FLOAT, "Invalid Budget Amount. The Budget Amount can only be a number.");
                    if (!m_FHandle.Validate())
                        return;

                    //filling datatable with class id and amount
                    dtbudgetdetail.Rows.Add(Convert.ToInt32(grdBudgetBalnce[i, 3].Value), Convert.ToDecimal(grdBudgetBalnce[i, 2].Value));
                    sum += Convert.ToDecimal(grdBudgetBalnce[i, 2].Value);
                }

                if (budgetMasterID > 0)
                {
                    if(dtGlobal.Rows.Count>0)
                    {
                       bgt.UpdateBudgetAllocation(budgetMasterID,sum,dtbudgetdetail);
                      
                    }
                    else
                    {
                        Global.Msg("Updation not allowed id problem");
                        
                    }
                    this.Dispose();
                }

                else
                {
                    //save the new budget allocation
                    bgt.saveBudgetAllocation(budgetID, accID, accType, sum, dtbudgetdetail);
                    this.Dispose();
                }

            }
            catch(Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}
