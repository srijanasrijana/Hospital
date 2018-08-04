using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace AccSwift
{
    public partial class frmCashReceipt : Form
    {

        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked

        SourceGrid.Cells.Button btnRowDelete = new SourceGrid.Cells.Button("");
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAccount = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtAmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        CashReceipt m_CashReceipt = new CashReceipt();
        public frmCashReceipt()
        {
            InitializeComponent();
        }

        private void frmCashReceipt_Load(object sender, EventArgs e)
        {


            m_mode = EntryMode.NEW;

            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();

            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.

            try
            {
                //Displaying the all ledgers associated with Cash in hand AccountGroup in DropDownList

                int CashInHand_ID = 102;//Manually inserting the value of GroupID of Cash-In-Hand

                DataTable dtCashHandLedgers = Ledger.GetAllLedger(CashInHand_ID);
                for (int i = 1; i <= dtCashHandLedgers.Rows.Count; i++)
                {

                    DataRow drCashHandLedgers = dtCashHandLedgers.Rows[i-1];
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drCashHandLedgers["LedgerID"]), LangMgr.Language);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cmboCashAccount.Items.Add(new ListItem((int)drCashHandLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
             
                
                
                
                }
                cmboCashAccount.DisplayMember = "value";//This value is  for showing at Load condition
                cmboCashAccount.ValueMember = "id";//This value is stored only not to be shown at Load condition  

               


                //Event trigerred when delete button is clicked

                evtDelete.Click += new EventHandler(Delete_Row_Click);

                //Event when account is selected
                evtAccount.FocusLeft += new EventHandler(Account_Selected);

                evtAmountFocusLost.FocusLeft += new EventHandler(Amount_Focus_Lost);

                grdCashReceipt.Redim(2, 5);
                btnRowDelete.Image = global::AccSwift.Properties.Resources.gnome_window_close;

                //Prepare the header part for grid
                AddGridHeader();

                AddRowCashReceipt(1);
                //grid1.AutoSizeCells();
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdCashReceipt.RowsCount - 2)
                grdCashReceipt.Rows.Remove(ctx.Position.Row);


        }


        private void Account_Selected(object sender, EventArgs e)
        {

            SourceGrid.CellContext ct = (SourceGrid.CellContext)sender;
            if (ct.DisplayText == "")
                return;


            int RowCount = grdCashReceipt.RowsCount;
            //Add a new row



            string CurRow = (string)grdCashReceipt[RowCount - 1, 2].Value;

            //Check whether the new row is already added
            if (CurRow != "(NEW)")
            {
                AddRowCashReceipt(RowCount);
                //Clear (NEW) on other colums as well
                ClearNew(RowCount - 1);
            }


        }



        /// <summary>
        /// Clears the (NEW) in the newly created row just after insertion of new Data in grid
        /// </summary>
        /// <param name="RowCount"></param>
        private void ClearNew(int RowCount)
        {
            if (grdCashReceipt[RowCount, 2].Value == "(NEW)")
                grdCashReceipt[RowCount, 2].Value = "";
            if (grdCashReceipt[RowCount, 3].Value == "(NEW)")
                grdCashReceipt[RowCount, 3].Value = "";
            if (grdCashReceipt[RowCount, 4].Value == "(NEW)")
                grdCashReceipt[RowCount, 4].Value = "";
           

        }


        private void Amount_Focus_Lost(object sender, EventArgs e)
        {
            
        }


        /// <summary>
        /// Writes the header part forgrdCashReceipt
        /// </summary>
        private void AddGridHeader()
        {
            grdCashReceipt[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
            grdCashReceipt[0, 1] = new SourceGrid.Cells.ColumnHeader("S.No.");
            grdCashReceipt[0, 2] = new SourceGrid.Cells.ColumnHeader("Account");
            grdCashReceipt[0, 3] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdCashReceipt[0, 4] = new SourceGrid.Cells.ColumnHeader("Remarks");
            grdCashReceipt[0, 0].Column.Width = 25;
            grdCashReceipt[0, 2].Column.Width = 120;
            grdCashReceipt[0, 4].Column.Width = 140;

        }


        /// <summary>
        /// Adds the row in the Journal field
        /// </summary>
        private void AddRowCashReceipt(int RowCount)
        {


            //Add a new row

            grdCashReceipt.Redim(Convert.ToInt32(RowCount + 1), grdCashReceipt.ColumnsCount);

            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
            btnDelete.Image = global::AccSwift.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;

            int i = RowCount;

            grdCashReceipt[i, 0] = btnDelete;

            grdCashReceipt[i, 0].AddController(evtDelete);

            grdCashReceipt[i, 1] = new SourceGrid.Cells.Cell(i.ToString());


            SourceGrid.Cells.Editors.ComboBox cboAccount = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboAccount.StandardValues = Ledger.GetLedgerList(0);

            cboAccount.Control.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboAccount.Control.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboAccount.Control.LostFocus += new EventHandler(Account_Selected);

            cboAccount.EditableMode = SourceGrid.EditableMode.Focus;


            grdCashReceipt[i, 2] = new SourceGrid.Cells.Cell("", cboAccount);
            grdCashReceipt[i, 2].AddController(evtAccount);
            grdCashReceipt[i, 2].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtAmount.EditableMode = SourceGrid.EditableMode.Focus;

            grdCashReceipt[i, 3] = new SourceGrid.Cells.Cell("", txtAmount);
            grdCashReceipt[i, 3].AddController(evtAmountFocusLost);
            grdCashReceipt[i, 3].Value = "(NEW)";

            SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;

            grdCashReceipt[i, 4] = new SourceGrid.Cells.Cell("", txtRemarks);
            grdCashReceipt[i, 4].Value = "(NEW)";

        }

        private void btnSave_Click(object sender, EventArgs e)
        {




            //Check Validation
            if (!Validate())
                return;



            switch (m_mode)
            {

                #region NEW
                case EntryMode.NEW: //if new button is pressed

                    try
                    {

                        //Read from sourcegrid and store it to table
                        DataTable CashReceiptDetails = new DataTable();
                        CashReceiptDetails.Columns.Add("Ledger");
                        CashReceiptDetails.Columns.Add("Amount");
                        CashReceiptDetails.Columns.Add("Remarks");

                        for (int i = 0; i < grdCashReceipt.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {

                            CashReceiptDetails.Rows.Add(grdCashReceipt[i + 1, 2].Value, grdCashReceipt[i + 1, 3].Value, grdCashReceipt[i + 1, 4].Value);
                        }

                        CashReceipt m_CashReceipt = new  CashReceipt();
                        DateTime CashReceipt_Date = Date.ToDotNet(txtDate.Text);
                        ListItem liLedgerID = new ListItem();
                        liLedgerID = (ListItem)cmboCashAccount.SelectedItem;
                        m_CashReceipt.Create("0", liLedgerID.ID, txtVchNo.Text, CashReceipt_Date, txtRemarks.Text, CashReceiptDetails);
                        
                       

                        Global.Msg("CashReceipt created successfully!");
                        ChangeState(EntryMode.NORMAL);


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

                    try
                    {

                        //Read from sourcegrid and store it to table
                        DataTable CashReceiptDetails = new DataTable();
                        CashReceiptDetails.Columns.Add("Ledger");
                        CashReceiptDetails.Columns.Add("Amount");
                        CashReceiptDetails.Columns.Add("Remarks");

                        for (int i = 0; i < grdCashReceipt.Rows.Count - 2; i++) //Skip the first row(being header) and last row(being (NEW))
                        {
                            CashReceiptDetails.Rows.Add(grdCashReceipt[i + 1, 2].Value, grdCashReceipt[i + 1, 3].Value, grdCashReceipt[i + 1, 4].Value);
                        }

                        //IJournal m_Journal = new Journal();
                        DateTime CashReceipt_Date = Date.ToDotNet(txtDate.Text);
                        m_CashReceipt.Modify(Convert.ToInt32(txtCashReceiptID.Text), "0", txtVchNo.Text, CashReceipt_Date, txtRemarks.Text, CashReceiptDetails);


                        Global.Msg("Cash Receipt modified successfully!");
                        ChangeState(EntryMode.NORMAL);


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
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    break;

            }
        }

        private void EnableControls(bool Enable)
        {
            txtVchNo.Enabled = txtDate.Enabled = txtRemarks.Enabled = grdCashReceipt.Enabled = Enable;
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                //CashReceipt m_CashReceipt = new CashReceipt();

                ChangeState(EntryMode.NORMAL);

                //Get the one step previous voucher
                int VouchID = 0;
                try
                {
                    VouchID = Convert.ToInt32(txtCashReceiptID.Text);
                }
                catch (Exception)
                {
                    VouchID = 999999999; //set to maximum so that it automatically gets the highest
                }

                DataTable dtCashReceiptMaster = m_CashReceipt.NavigateCashReceiptMaster(VouchID, Navigate.First);

                if (dtCashReceiptMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    return;
                }

                //Clear everything in the form
                ClearVoucher();


                //Write the corresponding textboxes
                DataRow drCashReceiptMaster = dtCashReceiptMaster.Rows[0]; //There is only one row. First row is the required record
                txtVchNo.Text = drCashReceiptMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drCashReceiptMaster["CashReceipt_Date"].ToString());
                txtRemarks.Text = drCashReceiptMaster["Remarks"].ToString();


                txtCashReceiptID.Text = drCashReceiptMaster["CashReceiptID"].ToString();


                DataTable dtCashReceiptDetail = m_CashReceipt.GetCashReceiptDetail(Convert.ToInt32(txtCashReceiptID.Text));


                for (int i = 1; i <= dtCashReceiptDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtCashReceiptDetail.Rows[i - 1];
                   grdCashReceipt[i, 1].Value = i.ToString();
                   grdCashReceipt[i, 2].Value = drDetail["LedgerName"].ToString();
                   grdCashReceipt[i, 3].Value = drDetail["Amount"].ToString();
                   grdCashReceipt[i, 4].Value = drDetail["Remarks"].ToString();
                   AddRowCashReceipt(grdCashReceipt.RowsCount);
                }



            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }

        private void ClearVoucher()
        {
            ClearCashReceipt();

            grdCashReceipt.Redim(2, 6);
            AddGridHeader(); //Write header part
            AddRowCashReceipt(1);
        }

        private void ClearCashReceipt()
        {
            txtVchNo.Clear(); //actually generate a new voucher no.
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            txtRemarks.Clear();
            grdCashReceipt.Rows.Clear();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
               
                ChangeState(EntryMode.NORMAL);
                //Get the one step previous voucher
                int vouchID = 0;
                try
                {
                    vouchID = Convert.ToInt32(txtCashReceiptID.Text);
                }
                catch (Exception)
                {
                    vouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
             
               
                DataTable dtCashReceiptMaster = m_CashReceipt.NavigateCashReceiptMaster(vouchID, Navigate.Prev);

                if (dtCashReceiptMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    return;
                }

                //Clear everything in the form
                ClearVoucher();

                //Write the corresponding textboxes
                DataRow drCashReceiptMaster = dtCashReceiptMaster.Rows[0]; //There is only one row. First row is the required record
                txtVchNo.Text = drCashReceiptMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drCashReceiptMaster["CashReceipt_Date"].ToString());
                txtRemarks.Text = drCashReceiptMaster["Remarks"].ToString();


                txtCashReceiptID.Text = drCashReceiptMaster["CashReceiptID"].ToString();


                DataTable dtCashReceiptDetail = m_CashReceipt.GetCashReceiptDetail(Convert.ToInt32(txtCashReceiptID.Text));


                for (int i = 1; i <= dtCashReceiptDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtCashReceiptDetail.Rows[i - 1];
                    grdCashReceipt[i, 1].Value = i.ToString();
                    grdCashReceipt[i, 2].Value = drDetail["LedgerName"].ToString();                   
                    grdCashReceipt[i, 3].Value = drDetail["Amount"].ToString();
                    grdCashReceipt[i, 4].Value = drDetail["Remarks"].ToString();
                    AddRowCashReceipt(grdCashReceipt.RowsCount);
                }


            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            try
            {
                ChangeState(EntryMode.NORMAL);

                //Get the one step previous voucher
                int vouchID = 0;
                try
                {
                    vouchID = Convert.ToInt32(txtCashReceiptID.Text);
                }
                catch (Exception)
                {
                    vouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
               
                DataTable dtCashReceiptMaster = m_CashReceipt.NavigateCashReceiptMaster(vouchID, Navigate.Next);

                if (dtCashReceiptMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    return;
                }

                //Clear everything in the form
                ClearVoucher();

                //Write the corresponding textboxes
                DataRow drCashReceiptMaster = dtCashReceiptMaster.Rows[0]; //There is only one row. First row is the required record
                txtVchNo.Text = drCashReceiptMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drCashReceiptMaster["CashReceipt_Date"].ToString());
                txtRemarks.Text = drCashReceiptMaster["Remarks"].ToString();


               txtCashReceiptID.Text = drCashReceiptMaster["CashReceiptID"].ToString();


               DataTable dtCashReciptDetail = m_CashReceipt.GetCashReceiptDetail(Convert.ToInt32(txtCashReceiptID.Text));


                for (int i = 1; i <= dtCashReciptDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtCashReciptDetail.Rows[i - 1];
                    grdCashReceipt[i, 1].Value = i.ToString();
                    grdCashReceipt[i, 2].Value = drDetail["LedgerName"].ToString();
                    grdCashReceipt[i, 3].Value = drDetail["Amount"].ToString();
                    grdCashReceipt[i, 4].Value = drDetail["Remarks"].ToString();
                    AddRowCashReceipt(grdCashReceipt.RowsCount);
                }

                //Calculate the Debit and Credit totals
                //CalculateDrCr();


            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }


        }

        private void btnLast_Click(object sender, EventArgs e)
        {


            try
            {
                ChangeState(EntryMode.NORMAL);

                //Get the one step previous voucher
                int vouchID = 0;
                try
                {
                    vouchID = Convert.ToInt32(txtCashReceiptID.Text);
                }
                catch (Exception)
                {
                    vouchID = 999999999; //set to maximum so that it automatically gets the highest
                }
              

                DataTable dtCashReceiptMaster = m_CashReceipt.NavigateCashReceiptMaster(vouchID, Navigate.Last);

                if (dtCashReceiptMaster.Rows.Count <= 0)//this is the first record
                {
                    Global.Msg("No more records found!");
                    return;
                }

                //Clear everything in the form
                ClearVoucher();

                //Write the corresponding textboxes
                DataRow drCashReceiptMaster = dtCashReceiptMaster.Rows[0]; //There is only one row. First row is the required record
                txtVchNo.Text = drCashReceiptMaster["Voucher_No"].ToString();
                txtDate.Text = Date.DBToSystem(drCashReceiptMaster["CashReceipt_Date"].ToString());
                txtRemarks.Text = drCashReceiptMaster["Remarks"].ToString();


                txtCashReceiptID.Text = drCashReceiptMaster["CashReceiptID"].ToString();


                DataTable dtCashReceiptDetail = m_CashReceipt.GetCashReceiptDetail(Convert.ToInt32(txtCashReceiptID.Text));


                for (int i = 1; i <= dtCashReceiptDetail.Rows.Count; i++)
                {
                    DataRow drDetail = dtCashReceiptDetail.Rows[i - 1];
                    grdCashReceipt[i, 1].Value = i.ToString();
                    grdCashReceipt[i, 2].Value = drDetail["LedgerName"].ToString();
                    grdCashReceipt[i, 3].Value = drDetail["Amount"].ToString();
                    grdCashReceipt[i, 4].Value = drDetail["Remarks"].ToString();
                    AddRowCashReceipt(grdCashReceipt.RowsCount);
                }


                //Calculate the Debit and Credit totals
                //CalculateDrCr();

            }
            catch (Exception ex)
            {

                Global.Msg(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EnableControls(true);
            ChangeState(EntryMode.EDIT);

        }

    }
}
