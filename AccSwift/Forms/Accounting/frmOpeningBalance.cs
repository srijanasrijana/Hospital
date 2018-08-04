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

namespace Inventory
{
    public interface IfrmOpeningBalance
    {
        void AddOpeningBalance(DataTable AllOpeningBalance);
    }

    public partial class frmOpeningBalance : Form
    {       
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for OpeningBalance
        private int AccClass = 0;
        private int LedgerID = 0;
        private int ParentGroupID = 0;
        private IfrmOpeningBalance m_ParentForm;
        private OpeningBalanceSettings m_OBS=new OpeningBalanceSettings();

        public frmOpeningBalance()
        {
            InitializeComponent();
        }

        public frmOpeningBalance(Form ParentForm, OpeningBalanceSettings OBS)
        {
            InitializeComponent();
            m_ParentForm = (IfrmOpeningBalance)ParentForm;
            //Set the selected font to everything
           // this.Font = LangMgr.GetFont();
            this.LedgerID = OBS.LedgerID;
            this.ParentGroupID = OBS.ParentGroupID;
            if (this.LedgerID == 0 && this.ParentGroupID <= 0)
            {
                //If it is in new mode, user has to select Parent Group ID
                Global.Msg("You need to select Parent Group first");
                this.Dispose();
            }



        }

        private void frmOpeningBalance_Load(object sender, EventArgs e)
        {

            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            //Disable multiple selection
            grdOpeningBalnce.Selection.EnableMultiSelection = false;
            grdOpeningBalnce.Redim(1, 5);
            //grdOpeningBalnce.FixedRows = 1;

            //Prepare the header part for grid
            AddGridHeader();
            AddRowOpeningBalance(1);
            LoadAccountClassWithOpeningBalance();
        }


        /// <summary>
        /// Writes the header of theGrid
        /// </summary>
        private void AddGridHeader()
        {
            grdOpeningBalnce[0, 0] = new SourceGrid.Cells.ColumnHeader("S.N.");
            grdOpeningBalnce[0, 1] = new SourceGrid.Cells.ColumnHeader("Account Class");
            grdOpeningBalnce[0, 2] = new SourceGrid.Cells.ColumnHeader("Opening Balance");
            grdOpeningBalnce[0, 3] = new SourceGrid.Cells.ColumnHeader("Dr/Cr");
            grdOpeningBalnce[0, 4] = new SourceGrid.Cells.ColumnHeader("AccClassID");
            grdOpeningBalnce[0, 0].Column.Width = 50;
            grdOpeningBalnce[0, 1].Column.Width = 200;
            grdOpeningBalnce[0, 2].Column.Width = 120;
            grdOpeningBalnce[0, 3].Column.Width = 100;
            grdOpeningBalnce.Columns[4].Visible = false;
        }

        /// <summary>
        /// Add a single row in the grid
        /// </summary>
        /// <param name="RowCount"></param>
        private void AddRowOpeningBalance(int RowCount)
        {
            try
            {
                //Add a new row
                grdOpeningBalnce.Redim(Convert.ToInt32(RowCount + 1), grdOpeningBalnce.ColumnsCount);
                int i = RowCount;
                grdOpeningBalnce[i, 0] = new SourceGrid.Cells.Cell(i.ToString());

                SourceGrid.Cells.Editors.TextBox txtAccountClass = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtAccountClass.EditableMode = SourceGrid.EditableMode.None;
                grdOpeningBalnce[i, 1] = new SourceGrid.Cells.Cell("", txtAccountClass);

                SourceGrid.Cells.Editors.TextBox txtOpeningBalance = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtOpeningBalance.EditableMode = SourceGrid.EditableMode.Focus;
                grdOpeningBalnce[i, 2] = new SourceGrid.Cells.Cell("", txtOpeningBalance);



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


                grdOpeningBalnce[i, 3] = new SourceGrid.Cells.Cell(strDrCr, cboDrCr);




                SourceGrid.Cells.Editors.TextBox txtAccClassID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtAccClassID.EditableMode = SourceGrid.EditableMode.None;
                grdOpeningBalnce[i, 4] = new SourceGrid.Cells.Cell("", txtAccClassID);

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }



        }

        private void OpeningBalance_Focus_Lost(object sender, EventArgs e)
        {
            //validate the string
           

        }

        private void WriteAccountList(int SNo, string AccountClass, string OpeningBalance)
        {
            //grdOpeningBalnce.Rows.Insert(grdOpeningBalnce.RowsCount);
            AccClass = grdOpeningBalnce.RowsCount - 1;
            grdOpeningBalnce[AccClass, 0] = new SourceGrid.Cells.Cell(SNo);
            grdOpeningBalnce[AccClass, 1] = new SourceGrid.Cells.Cell(AccountClass);
            grdOpeningBalnce[AccClass, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
            grdOpeningBalnce[AccClass, 2] = new SourceGrid.Cells.Cell(OpeningBalance);
            grdOpeningBalnce[AccClass, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            // grdOpeningBalnce[AccClass, 2].AddController(dblClick);
            //Increment company rows count
            AddRowOpeningBalance(grdOpeningBalnce.RowsCount);
            AccClass++;
        }


        /// <summary>
        /// Loads All Account class and its corresponding Opening Balance
        /// </summary>
        private void LoadAccountClassWithOpeningBalance()
        {
            //DataTable dt =  AccountClass.GetAccClassTable(0);           

            #region Language Management

            grdOpeningBalnce.Font = LangMgr.GetFont();

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
                DataTable dt,dtOpBal;
                if (((frmAccountSetup)m_ParentForm).FromOpeningBalance.Rows.Count > 0)
                {
                    //Opening balance is inputed manually but not saved
                    //Load Opening balance from DataTable
                    dtOpBal = ((frmAccountSetup)m_ParentForm).FromOpeningBalance;
                }
                //else
                dt= AccountClass.GetRootAccClass(-1);
                int Sno = 1;
                // string OpeningBalanceAmount = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                   
                    //Serial No.
                    grdOpeningBalnce[i + 1, 0].Value = (i + 1).ToString();

                    //Accounting Class Name
                    grdOpeningBalnce[i + 1, 1].Value = dr[LangField].ToString();

                    //Opening Balance
                    if (((frmAccountSetup)m_ParentForm).FromOpeningBalance.Rows.Count > 0)//If Opening balances has already been entered
                    {
                        DataRow drOpBal = GetOpeningBalanceRow(Convert.ToInt32(dr["AccClassID"]));
                        if (drOpBal != null)//The entry for that AccClass has been made
                        {
                            grdOpeningBalnce[i + 1, 2].Value = drOpBal["OpeningBalance"].ToString();
                            grdOpeningBalnce[i + 1, 3].Value = drOpBal["DrCr"].ToString();
                        }
                    }
                    else//No, opening balances hasnt been entered
                    {
                        DataTable dt1 = OpeningBalance.GetAccClassOpeningBalance(Convert.ToInt32(dr["AccClassID"]), LedgerID);

                        if (dt1.Rows.Count != 0)//If the ledger has been edited
                        {
                            DataRow drOpBal = dt1.Rows[0];
                            // grdOpeningBalnce[i + 1, 2].Value = drOpBal["OpeningBalance"].ToString();
                            grdOpeningBalnce[i + 1, 2].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drOpBal["OpenBal"])).ToString();

                            string DrCr = "Debit";
                            if (drOpBal["OpenBalDrCr"].ToString() == "DEBIT")
                                DrCr = "Debit";
                            else
                                DrCr = "Credit";
                            grdOpeningBalnce[i + 1, 3].Value = DrCr;
                        }
                        else//This is a new mode
                        {
                            //Get the opening balance from selected accounting group
                            DataTable dtGroup=AccountGroup.GetRootGroup(this.ParentGroupID);
                            string DrCr="Debit";
                            if (dtGroup.Rows.Count > 0)
                            {
                                DrCr = dtGroup.Rows[0]["DrCr"].ToString();
                                if (DrCr.ToUpper() == "DR" || DrCr.ToUpper() == "DEBIT" || DrCr.ToUpper() == "D")
                                    DrCr = "Debit";
                                else
                                    DrCr = "Credit";
                            }
                            grdOpeningBalnce[i + 1, 2].Value = 0;
                            grdOpeningBalnce[i + 1, 3].Value = DrCr;
                        }
                    }


                    //In both cases, ID are same
                    grdOpeningBalnce[i + 1, 4].Value = dr["AccClassID"].ToString();
                    if( i < dt.Rows.Count-1)
                        AddRowOpeningBalance(grdOpeningBalnce.RowsCount);  
              
                    Sno++;
                }//End for loop               
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

        /// <summary>
        /// Gets the dataRow from FromOpeningBalance DataTable
        /// </summary>
        /// <param name="AccClassID"></param>
        /// <returns></returns>
        private DataRow GetOpeningBalanceRow(int AccClassID)
        {
            foreach (DataRow dr in ((frmAccountSetup)m_ParentForm).FromOpeningBalance.Rows)
            {
                if (Convert.ToInt32(dr["AccClassID"]) == AccClassID)
                    return dr;
            }
            return null;
        }

        private void btnGrpSave_Click(object sender, EventArgs e)
        {
            FormHandle m_FHandle = new FormHandle();
            for (int i = 1; i < grdOpeningBalnce.RowsCount; i++)
            {
                txtValidateOpeningBalance.Text = grdOpeningBalnce[i, 2].ToString();
                if (txtValidateOpeningBalance.Text.Trim().Length == 0)
                    txtValidateOpeningBalance.Text = "0";

                
                 m_FHandle.AddValidate(txtValidateOpeningBalance, DType.FLOAT, "Invalid opening balance. The opening balance can only be a number.");
                 if( !m_FHandle.Validate())
                    return;
                
            }

            DataTable dt = GetTable();
            m_ParentForm.AddOpeningBalance(dt);
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
	        table.Columns.Add("OpeningBalance", typeof(string));
            table.Columns.Add("DrCr", typeof(string));

             FormHandle m_FHandle = new FormHandle();
	        for (int i = 0; i < grdOpeningBalnce.Rows.Count - 1; i++) //Skip the first row(being header) and last row(being (NEW))
            {
                if ((grdOpeningBalnce[i + 1, 2].ToString() != "") && (grdOpeningBalnce[i + 1, 0].ToString() != ""))
                {
                    if (!Misc.IsNumeric(grdOpeningBalnce[i + 1, 2]))
                    {
                        MessageBox.Show("Invalid Opening Balance.");                       
                    }
                    table.Rows.Add(grdOpeningBalnce[i + 1, 4].ToString(), grdOpeningBalnce[i + 1, 1].ToString(), grdOpeningBalnce[i + 1, 2].ToString(),grdOpeningBalnce[i+1,3].ToString());
                }
            }
	        return table;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmOpeningBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
      }
    }

