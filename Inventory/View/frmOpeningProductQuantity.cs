using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using BusinessLogic.Inventory;

namespace Inventory
{
    public interface IfrmOpeningQuantity
    {
        void AddOpeningQuantity(DataTable AllOpeningQuantity);
    }
    public partial class frmOpeningProductQuantity : Form
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for OpeningBalance
        private int AccClass = 0;
        private int ProductID = 0;
        private int ParentGroupID = 0;
        private IfrmOpeningQuantity m_ParentForm;
        private OpeningQuantitySetting m_OBS = new OpeningQuantitySetting();
        public frmOpeningProductQuantity()
        {
            InitializeComponent();
        }
        public frmOpeningProductQuantity(Form ParentForm, OpeningQuantitySetting OBS)
        {
            InitializeComponent();
            m_ParentForm = (IfrmOpeningQuantity)ParentForm;
           //Set the selected font to everything
           // this.Font = LangMgr.GetFont();  


            this.ProductID = OBS.ProductID;
            this.ParentGroupID = OBS.ParentGroupID;
            if (this.ProductID == 0 && this.ParentGroupID <= 0)
            {
                //If it is in new mode, user has to select Parent Group ID
                Global.Msg("You need to select Parent Group first");
                this.Dispose();
            }
        }

        private void frmOpeningProductQuantity_Load(object sender, EventArgs e)
        {
            //Add a double click handler. When user dblclicks the cell, will use this function to send the parent form the Currency code
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            //Disable multiple selection
            grdOpeningQuantity.Selection.EnableMultiSelection = false;
            grdOpeningQuantity.Redim(1, 6);
            //grdOpeningQuantity.FixedRows = 1;
            #region BLOCK OF SHOWING DEPOT IN COMBOBOX
            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            foreach (DataRow dr in dtDepotInfo.Rows)
            {
                cmbDepotLocation.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            cmbDepotLocation.SelectedIndex = 0;

            #endregion
            //Prepare the header part for grid
            AddGridHeader();
            AddRowOpeningBalance(1);
            LoadAccountClassWithOpeningQuantity();
        }

        private void AddGridHeader()
        {
            grdOpeningQuantity[0, 0] = new SourceGrid.Cells.ColumnHeader("S.N.");
            grdOpeningQuantity[0, 1] = new SourceGrid.Cells.ColumnHeader("Account Class");
            grdOpeningQuantity[0, 2] = new SourceGrid.Cells.ColumnHeader("Quantity");
            grdOpeningQuantity[0, 3] = new SourceGrid.Cells.ColumnHeader("Purchase Rate");
            grdOpeningQuantity[0, 4] = new SourceGrid.Cells.ColumnHeader("Sales Rate");
            grdOpeningQuantity[0, 5] = new SourceGrid.Cells.ColumnHeader("AccClassID");
            grdOpeningQuantity[0, 0].Column.Width = 50;
            grdOpeningQuantity[0, 1].Column.Width = 100;
            grdOpeningQuantity[0, 2].Column.Width = 100;
            grdOpeningQuantity[0, 3].Column.Width = 100;
            grdOpeningQuantity[0, 4].Column.Width = 100;
            grdOpeningQuantity.Columns[5].Visible = false;
        }
        private void AddRowOpeningBalance(int RowCount)
        {
            try
            {
                //Add a new row
                grdOpeningQuantity.Redim(Convert.ToInt32(RowCount + 1), grdOpeningQuantity.ColumnsCount);
                int i = RowCount;
                grdOpeningQuantity[i, 0] = new SourceGrid.Cells.Cell(i.ToString());

                SourceGrid.Cells.Editors.TextBox txtAccountClass = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtAccountClass.EditableMode = SourceGrid.EditableMode.None;
                grdOpeningQuantity[i, 1] = new SourceGrid.Cells.Cell("", txtAccountClass);

                SourceGrid.Cells.Editors.TextBox txtPurchaseQuantity = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseQuantity.EditableMode = SourceGrid.EditableMode.Focus;
                grdOpeningQuantity[i, 2] = new SourceGrid.Cells.Cell("", txtPurchaseQuantity);

                SourceGrid.Cells.Editors.TextBox txtPurchaseRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtPurchaseRate.EditableMode = SourceGrid.EditableMode.Focus;
                grdOpeningQuantity[i, 3] = new SourceGrid.Cells.Cell("", txtPurchaseRate);

                SourceGrid.Cells.Editors.TextBox txtSalesRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtSalesRate.EditableMode = SourceGrid.EditableMode.Focus;
                grdOpeningQuantity[i, 4] = new SourceGrid.Cells.Cell("", txtSalesRate);

                SourceGrid.Cells.Editors.TextBox txtAccClassID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtAccClassID.EditableMode = SourceGrid.EditableMode.None;
                grdOpeningQuantity[i, 5] = new SourceGrid.Cells.Cell("", txtAccClassID);

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void LoadAccountClassWithOpeningQuantity()
        {
            //DataTable dt =  AccountClass.GetAccClassTable(0);           

            #region Language Management

            grdOpeningQuantity.Font = LangMgr.GetFont();

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
                DataTable dt, dtOpBal;
                if (((frmProduct)m_ParentForm).FromOpeningQuantity.Rows.Count > 0)
                {
                    //Opening balance is inputed manually but not saved
                    //Load Opening balance from DataTable
                    dtOpBal = ((frmProduct)m_ParentForm).FromOpeningQuantity;
                }
                //else
                dt = AccountClass.GetRootAccClass(-1);
                int Sno = 1;
                // string OpeningBalanceAmount = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    //Serial No.
                    grdOpeningQuantity[i + 1, 0].Value = (i + 1).ToString();

                    //Accounting Class Name
                    grdOpeningQuantity[i + 1, 1].Value = dr[LangField].ToString();

                    //Opening Balance
                    if (((frmProduct)m_ParentForm).FromOpeningQuantity.Rows.Count > 0)//If Opening balances has already been entered
                    {
                        DataRow drOpBal = GetOpeningBalanceRow(Convert.ToInt32(dr["AccClassID"]));
                        if (drOpBal != null)//The entry for that AccClass has been made
                        {
                            grdOpeningQuantity[i + 1, 2].Value = Convert.ToInt32(drOpBal["OpenPurchaseQty"]).ToString();
                            grdOpeningQuantity[i + 1, 3].Value = Convert.ToDecimal(drOpBal["OpenPurchaseRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            grdOpeningQuantity[i + 1, 4].Value = Convert.ToDecimal(drOpBal["OpenSalesRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                          
                        }
                    }
                    else//No, opening balances hasnt been entered
                    {
                        DataTable dt1 = OpeningBalance.GetAccClassOpeningQuantity(Convert.ToInt32(dr["AccClassID"]), ProductID);

                        if (dt1.Rows.Count != 0)//If the ledger has been edited
                        {
                            DataRow drOpBal = dt1.Rows[0];
                            // grdOpeningQuantity[i + 1, 2].Value = drOpBal["OpeningBalance"].ToString();
                           // grdOpeningQuantity[i + 1, 2].Value = Misc.GetDecimalValueByDecimalPlaces(Convert.ToDecimal(drOpBal["OpenPurchaseQty"])).ToString();
                            grdOpeningQuantity[i + 1, 2].Value =Convert.ToInt32(drOpBal["OpenPurchaseQty"]).ToString();
                            grdOpeningQuantity[i + 1, 3].Value = Convert.ToDecimal(drOpBal["OpenPurchaseRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                            grdOpeningQuantity[i + 1, 4].Value = Convert.ToDecimal(drOpBal["OpenSalesRate"]).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                           
                        }
                        else//This is a new mode
                        {
                            //Get the opening balance from selected accounting group
                            DataTable dtGroup = AccountGroup.GetRootGroup(this.ParentGroupID);

                            //Assign '0' by default to all the value cells
                            grdOpeningQuantity[i + 1, 2].Value = 0;
                            grdOpeningQuantity[i + 1, 3].Value = 0;
                            grdOpeningQuantity[i + 1, 4].Value = 0;    

                            //string DrCr = "Debit";
                            //if (dtGroup.Rows.Count > 0)
                            //{
                            //    DrCr = dtGroup.Rows[0]["DrCr"].ToString();
                            //    if (DrCr.ToUpper() == "DR" || DrCr.ToUpper() == "DEBIT" || DrCr.ToUpper() == "D")
                            //        DrCr = "Debit";
                            //    else
                            //        DrCr = "Credit";
                            //}
                            //grdOpeningQuantity[i + 1, 2].Value = 0;
                            //grdOpeningQuantity[i + 1, 3].Value = DrCr;
                        }
                    }


                    //In both cases, ID are same
                    grdOpeningQuantity[i + 1, 5].Value = dr["AccClassID"].ToString();
                    if (i < dt.Rows.Count - 1)
                        AddRowOpeningBalance(grdOpeningQuantity.RowsCount);

                    Sno++;
                }//End for loop               
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }
        private DataRow GetOpeningBalanceRow(int AccClassID)
        {
            foreach (DataRow dr in ((frmProduct)m_ParentForm).FromOpeningQuantity.Rows)
            {
                if (Convert.ToInt32(dr["AccClassID"]) == AccClassID)
                    return dr;
            }
            return null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetQuantity();
            this.Close();
        }
        private DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("AccClassID", typeof(int));
            table.Columns.Add("AccClass", typeof(string));
            table.Columns.Add("PurchaseQuantity", typeof(float));
            table.Columns.Add("PurchaseRate", typeof(float));
            table.Columns.Add("SaleRate", typeof(float));
            FormHandle m_FHandle = new FormHandle();
            for (int i = 0; i < grdOpeningQuantity.Rows.Count - 1; i++) //Skip the first row(being header) and last row(being (NEW))
            {
                if ((grdOpeningQuantity[i + 1, 2].ToString() != "") && (grdOpeningQuantity[i + 1, 0].ToString() != ""))
                {
                    try
                    {
                        if (!Misc.IsNumeric(grdOpeningQuantity[i + 1, 2]))
                        {
                            MessageBox.Show("Invalid Purchase Quantity.");
                        }

                        table.Rows.Add(grdOpeningQuantity[i + 1, 5].ToString(), grdOpeningQuantity[i + 1, 1].ToString(), grdOpeningQuantity[i + 1, 2].ToString(), grdOpeningQuantity[i + 1, 3].ToString(), grdOpeningQuantity[i + 1, 4].ToString());
                    }catch(Exception ex)
                    {
                        Global.MsgError("Atleast 0 must be provided to all the cells of a row");

                    }
                }
            }
            return table;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TextBox txtpurchaserate = new TextBox();
            TextBox txtsalerate = new TextBox();

            FormHandle m_FHandle = new FormHandle();
            for (int i = 1; i < grdOpeningQuantity.RowsCount; i++)
            {
                txtValidateOpeningBalance.Text = grdOpeningQuantity[i, 2].ToString();
                if (txtValidateOpeningBalance.Text.Trim().Length == 0)
                    txtValidateOpeningBalance.Text = "0";

                txtpurchaserate.Text = grdOpeningQuantity[i, 3].ToString();
                if (txtpurchaserate.Text.Trim().Length == 0)
                    txtpurchaserate.Text = "0";

                txtsalerate.Text = grdOpeningQuantity[i, 4].ToString();
                if (txtsalerate.Text.Trim().Length == 0)
                    txtsalerate.Text = "0";

                m_FHandle.AddValidate(txtValidateOpeningBalance, DType.FLOAT, "Invalid Quantity. The Quantity can only be a number.");
                m_FHandle.AddValidate(txtpurchaserate, DType.FLOAT, "Invalid Purchase Rate. The Purchase Rate can only be a number.");
                m_FHandle.AddValidate(txtsalerate, DType.FLOAT, "Invalid Sales Rate. The Sales Rate can only be a number.");
                if (!m_FHandle.Validate())
                    return;
            }

            SetQuantity();
        }

        /// <summary>
        /// Method to add opening quantity to a DataTable in parent form.
        /// It is used to check if the quantity of a product is provided.
        /// </summary>
        private void SetQuantity()
        {
            DataTable dt = GetTable();
            m_ParentForm.AddOpeningQuantity(dt);
            this.Hide();
        }
    }
}
