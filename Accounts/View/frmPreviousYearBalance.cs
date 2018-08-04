using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;

namespace Accounts


{

    public interface IfrmPreviousYearBalance
    {
        void AddPreYearBalance(DataTable AllPreviousYearBalance);
    }
    
    public partial class frmPreviousYearBalance : Form
    {
        

        private int AccClass = 0;
        private int LedgerID = 0;
        private int ParentGroupID = 0;
        private IfrmPreviousYearBalance m_ParentForm;
        private OpeningBalanceSettings m_OBS = new OpeningBalanceSettings();
        public frmPreviousYearBalance()
        {
            InitializeComponent();
        }

        public frmPreviousYearBalance(Form ParentForm, OpeningBalanceSettings OBS)
        {
            InitializeComponent();
            m_ParentForm = (IfrmPreviousYearBalance)ParentForm;
           // //Set the selected font to everything
           //// this.Font = LangMgr.GetFont();
            this.LedgerID = OBS.LedgerID;
            this.ParentGroupID = OBS.ParentGroupID;
            if (this.LedgerID == 0 && this.ParentGroupID <= 0)
            {
                //If it is in new mode, user has to select Parent Group ID
                Global.Msg("You need to select Parent Group first");
                this.Dispose();
            }



        }

        private void frmPreviousYearBalance_Load(object sender, EventArgs e)
        {
            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
         //   dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            //Disable multiple selection
            grdPreYearBalnce.Selection.EnableMultiSelection = false;
            grdPreYearBalnce.Redim(1, 5);
            //grdOpeningBalnce.FixedRows = 1;

            //Prepare the header part for grid
            AddGridHeader();
            AddRowPreYearBalance(1);
            LoadAccountClassWithPreYearBalance();

        }

        /// <summary>
        /// Writes the header of theGrid
        /// </summary>
       private void AddGridHeader()
        {
            grdPreYearBalnce[0, 0] = new SourceGrid.Cells.ColumnHeader("S.N.");
            grdPreYearBalnce[0, 1] = new SourceGrid.Cells.ColumnHeader("Account Class");
            grdPreYearBalnce[0, 2] = new SourceGrid.Cells.ColumnHeader("Closing Balance");
            grdPreYearBalnce[0, 3] = new SourceGrid.Cells.ColumnHeader("Dr/Cr");
            grdPreYearBalnce[0, 4] = new SourceGrid.Cells.ColumnHeader("AccClassID");
            grdPreYearBalnce[0, 0].Column.Width = 50;
            grdPreYearBalnce[0, 1].Column.Width = 200;
            grdPreYearBalnce[0, 2].Column.Width = 140;
            grdPreYearBalnce[0, 3].Column.Width = 100;
            grdPreYearBalnce.Columns[4].Visible = false;
        }

       /// <summary>
       /// Add a single row in the grid
       /// </summary>
       /// <param name="RowCount"></param>
       private void AddRowPreYearBalance(int RowCount)
       {
           try
           {
               //Add a new row
               grdPreYearBalnce.Redim(Convert.ToInt32(RowCount + 1), grdPreYearBalnce.ColumnsCount);
               int i = RowCount;
               grdPreYearBalnce[i, 0] = new SourceGrid.Cells.Cell(i.ToString());

               SourceGrid.Cells.Editors.TextBox txtAccountClass = new SourceGrid.Cells.Editors.TextBox(typeof(string));
               txtAccountClass.EditableMode = SourceGrid.EditableMode.None;
               grdPreYearBalnce[i, 1] = new SourceGrid.Cells.Cell("", txtAccountClass);

               SourceGrid.Cells.Editors.TextBox txtPreYearBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
               txtPreYearBalance.EditableMode = SourceGrid.EditableMode.Focus;
               grdPreYearBalnce[i, 2] = new SourceGrid.Cells.Cell("", txtPreYearBalance);



               SourceGrid.Cells.Editors.ComboBox cboDrCr = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
               cboDrCr.StandardValues = new string[] { "Debit", "Credit" };
               cboDrCr.Control.DropDownStyle = ComboBoxStyle.DropDownList;

               cboDrCr.EditableMode = SourceGrid.EditableMode.Focus;


               string strDrCr = "Debit";

               //Find Root Accounting Class
               //Get Group from the Ledger

               if (LedgerID > 0)//User is editing some Ledger
               {
                   int GroupID = AccountGroup.GetGroupFromLedgerID(LedgerID);
                   DataTable dtRoot = AccountGroup.GetRootGroup(GroupID);
                   if (dtRoot.Rows.Count > 0)
                   {
                       strDrCr = dtRoot.Rows[0]["DrCr"].ToString();
                       if (strDrCr == "DR")
                           strDrCr = "Debit";
                       else
                           strDrCr = "Credit";
                   }
               }
               else if (ParentGroupID > 0)//It is in New mode and there is a parent group ID selected
               {
                   DataTable dtRoot = AccountGroup.GetRootGroup(ParentGroupID);
                   if (dtRoot.Rows.Count > 0)
                   {
                       strDrCr = dtRoot.Rows[0]["DrCr"].ToString();
                       if (strDrCr == "DR")
                           strDrCr = "Debit";
                       else
                           strDrCr = "Credit";
                   }
               }


               grdPreYearBalnce[i, 3] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);




               SourceGrid.Cells.Editors.TextBox txtAccClassID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
               txtAccClassID.EditableMode = SourceGrid.EditableMode.None;
               grdPreYearBalnce[i, 4] = new SourceGrid.Cells.Cell("", txtAccClassID);

           }
           catch (Exception ex)
           {
               Global.MsgError(ex.Message);
           }



       }

       private void WriteAccountList(int SNo, string AccountClass, string OpeningBalance)
       {
           //grdOpeningBalnce.Rows.Insert(grdOpeningBalnce.RowsCount);
           AccClass = grdPreYearBalnce.RowsCount - 1;
           grdPreYearBalnce[AccClass, 0] = new SourceGrid.Cells.Cell(SNo);
           grdPreYearBalnce[AccClass, 1] = new SourceGrid.Cells.Cell(AccountClass);
           grdPreYearBalnce[AccClass, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
           grdPreYearBalnce[AccClass, 2] = new SourceGrid.Cells.Cell(OpeningBalance);
           grdPreYearBalnce[AccClass, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

           // grdOpeningBalnce[AccClass, 2].AddController(dblClick);
           //Increment company rows count
           AddRowPreYearBalance(grdPreYearBalnce.RowsCount);
           AccClass++;
       }

       private void LoadAccountClassWithPreYearBalance()
       {
           //DataTable dt =  AccountClass.GetAccClassTable(0);           

           #region Language Management

           grdPreYearBalnce.Font = LangMgr.GetFont();

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
               DataTable dt, dtPreYearBal;
               if (((frmAccountSetup)m_ParentForm).FromPreYearBalance.Rows.Count > 0)
               {
                   //Opening balance is inputed manually but not saved
                   //Load Opening balance from DataTable
                   dtPreYearBal = ((frmAccountSetup)m_ParentForm).FromPreYearBalance;
               }
               //else
               dt = AccountClass.GetRootAccClass(-1);
               int Sno = 1;
               // string OpeningBalanceAmount = "";
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   DataRow dr = dt.Rows[i];

                   //Serial No.
                   grdPreYearBalnce[i + 1, 0].Value = (i + 1).ToString();

                   //Accounting Class Name
                   grdPreYearBalnce[i + 1, 1].Value = dr[LangField].ToString();

                   //Opening Balance
                   if (((frmAccountSetup)m_ParentForm).FromPreYearBalance.Rows.Count > 0)//If Opening balances has already been entered
                   {
                       DataRow drPreYearBal = GetPreYearBalanceRow(Convert.ToInt32(dr["AccClassID"]));
                       if (drPreYearBal != null)//The entry for that AccClass has been made
                       {
                           grdPreYearBalnce[i + 1, 2].Value = drPreYearBal["PreYearBalance"].ToString();
                           grdPreYearBalnce[i + 1, 3].Value = drPreYearBal["DrCr"].ToString();
                       }
                   }
                   else//No, opening balances hasnt been entered
                   {
                       DataTable dt1 = OpeningBalance.GetAccClassPreYearBalance(Convert.ToInt32(dr["AccClassID"]), LedgerID);

                       if (dt1.Rows.Count != 0)//If the ledger has been edited
                       {
                           DataRow drPreYearBal = dt1.Rows[0];
                           // grdOpeningBalnce[i + 1, 2].Value = drOpBal["OpeningBalance"].ToString();
                           grdPreYearBalnce[i + 1, 2].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drPreYearBal["PreYearBalance"])).ToString();

                           string DrCr = "Debit";
                           if (drPreYearBal["PreYearBalanceDrCr"].ToString().Trim()== "DEBIT")
                               DrCr = "Debit";
                           else
                               DrCr = "Credit";
                           grdPreYearBalnce[i + 1, 3].Value = DrCr;
                       }
                       else//This is a new mode
                       {
                           //Get the opening balance from selected accounting group
                           DataTable dtGroup = AccountGroup.GetRootGroup(this.ParentGroupID);
                           string DrCr = "Debit";
                           if (dtGroup.Rows.Count > 0)
                           {
                               DrCr = dtGroup.Rows[0]["DrCr"].ToString();
                               if (DrCr.ToUpper() == "DR" || DrCr.ToUpper() == "DEBIT" || DrCr.ToUpper() == "D")
                                   DrCr = "Debit";
                               else
                                   DrCr = "Credit";
                           }
                           grdPreYearBalnce[i + 1, 2].Value = 0;
                           grdPreYearBalnce[i + 1, 3].Value = DrCr;
                       }
                   }


                   //In both cases, ID are same
                   grdPreYearBalnce[i + 1, 4].Value = dr["AccClassID"].ToString();
                   if (i < dt.Rows.Count - 1)
                       AddRowPreYearBalance(grdPreYearBalnce.RowsCount);

                   Sno++;
               }//End for loop               
           }
           catch (Exception ex)
           {
               Global.Msg(ex.Message);
           }
       }

       private DataRow GetPreYearBalanceRow(int AccClassID)
       {
           foreach (DataRow dr in ((frmAccountSetup)m_ParentForm).FromPreYearBalance.Rows)
           {
               if (Convert.ToInt32(dr["AccClassID"]) == AccClassID)
                   return dr;
           }
           return null;
       }

       private void btnGrpSave_Click(object sender, EventArgs e)
       {
           FormHandle m_FHandle = new FormHandle();
           for (int i = 1; i < grdPreYearBalnce.RowsCount; i++)
           {
               txtValidateOpeningBalance.Text = grdPreYearBalnce[i, 2].ToString();
               if (txtValidateOpeningBalance.Text.Trim().Length == 0)
               {
                   txtValidateOpeningBalance.Text = "0";
                   grdPreYearBalnce[i, 2].Value = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
               }
                   
               m_FHandle.AddValidate(txtValidateOpeningBalance, DType.FLOAT, "Invalid Previous Year balance. The Previous Year balance can only be a number.");
               if (!m_FHandle.Validate())
                   return;

           }

           DataTable dt = GetTable();
           m_ParentForm.AddPreYearBalance(dt);
           this.Hide();
       }

       /// <summary>
       /// This example method generates a DataTable.
       /// </summary>
       private DataTable GetTable()
       {
           DataTable table = new DataTable();
           table.Columns.Add("AccClassID", typeof(int));
           table.Columns.Add("AccClass", typeof(string));
           table.Columns.Add("PreYearBalance", typeof(string));
           table.Columns.Add("DrCr", typeof(string));

           FormHandle m_FHandle = new FormHandle();
           for (int i = 0; i < grdPreYearBalnce.Rows.Count - 1; i++) //Skip the first row(being header) and last row(being (NEW))
           {
               if ((grdPreYearBalnce[i + 1, 2].ToString() != "") && (grdPreYearBalnce[i + 1, 0].ToString() != ""))
               {
                   if (!Misc.IsNumeric(grdPreYearBalnce[i + 1, 2]))
                   {
                       MessageBox.Show("Invalid Previous Year Balance.");
                   }
                   table.Rows.Add(grdPreYearBalnce[i + 1, 4].ToString(), grdPreYearBalnce[i + 1, 1].ToString(), grdPreYearBalnce[i + 1, 2].ToString(), grdPreYearBalnce[i + 1, 3].ToString());
               }
           }
           return table;
       }

       private void btnCancel_Click(object sender, EventArgs e)
       {
           this.Close();
       }

       private void frmPreYearBalance_KeyDown(object sender, KeyEventArgs e)
       {
           if (e.KeyCode == Keys.Escape)
           {
               this.Close();
           }
       }
    }
}
