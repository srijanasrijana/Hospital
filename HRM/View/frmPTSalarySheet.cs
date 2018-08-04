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

using BusinessLogic.HRM;
using CrystalDecisions.Shared;
using Common;


namespace HRM.View
{
    public partial class frmPTSalarySheet : Form, IfrmDateConverter, IPaySlip, IEmployeeList
    {
        private enum AmtQtyType { Rate, Qty}
        bool hasChanged = false;
        bool IsJournalEdit = false;
        private int m_PartTimeSalaryMasterID = 0;   
        private IMDIMainForm m_MDIForm;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
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

        private Model.dsPartTimeSalary dsPTS = new HRM.Model.dsPartTimeSalary();

        #region Events for SourceGrid
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

                //Do not delete if its the last Row because it contains (NEW)
                if (ctx.Position.Row <= grdSalarySheet.RowsCount - 2)
                    grdSalarySheet.Rows.Remove(ctx.Position.Row);

                CalculateTotal();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void Qty_Modified(object sender, EventArgs e)
        {
            try
            {
                //find the current row of source grid
                int CurRow = grdSalarySheet.Selection.GetSelectionRegion().GetRowsIndex()[0];

                if (isNewRow(CurRow))
                    return;

                if(grdSalarySheet[CurRow,5].Value.ToString() != "")
                {
                    if(!UserValidation.validDecimal(grdSalarySheet[CurRow,5].Value.ToString()))
                    {
                        Global.Msg("Invalid Quantity, Please enter again.");
                        return;
                    }
                }
                else
                {
                    grdSalarySheet[CurRow, 5].Value = "0";
                }
                object Qty = ((TextBox)sender).Text;

                //Check whether the value of quantity is zero or not?
                if (!(Convert.ToDouble(Qty) > 0))
                {
                    Global.MsgError("The Quantity shouldnot be zero. Fill the Quantity first!");
                    grdSalarySheet[CurRow, 5].Value = "1.0";
                    grdSalarySheet[CurRow, 6].Value = "0";
                    ((TextBox)sender).Text = "1";
                    return;
                }

                CalculateAmount(CurRow,Convert.ToDecimal(Qty),AmtQtyType.Qty);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void Rate_Modified(object sender, EventArgs e)
        {
            try
            {
                //find the current row of source grid
                int CurRow = grdSalarySheet.Selection.GetSelectionRegion().GetRowsIndex()[0];

                if (isNewRow(CurRow))
                    return;

                if (grdSalarySheet[CurRow, 6].Value.ToString() != "")
                {
                    if (!UserValidation.validDecimal(grdSalarySheet[CurRow, 6].Value.ToString()))
                    {
                        Global.Msg("Invalid amount, Please enter again.");
                        return;
                    }
                }
                else
                {
                    grdSalarySheet[CurRow, 6].Value = "0";
                }

                object Rate = ((TextBox)sender).Text;

                CalculateAmount(CurRow,Convert.ToDecimal(Rate),AmtQtyType.Rate);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void Employee_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;

                frmListOfEmployee fle = new frmListOfEmployee(this);
                fle.ShowDialog();
                SendKeys.Send("{TAB}{TAB}{TAB}");
            }

        }

        private void Employee_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            int RowCount = grdSalarySheet.RowsCount;
            string LastServicesCell = (string)grdSalarySheet[RowCount - 1, 2].Value;

            if (LastServicesCell != "(NEW)")
            {
                AddRow(RowCount);
            }
        }
        #endregion
        public frmPTSalarySheet()
        {
            InitializeComponent();
        }

        public frmPTSalarySheet(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;
        }

        private void frmPTSalarySheet_Load(object sender, EventArgs e)
        {
            try
            {

                //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
                txtDate.Mask = Date.FormatToMask();
                txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.

                //Event trigerred when delete button is clicked
                evtDelete.Click += new EventHandler(Delete_Row_Click);

                //LoadCboCashBank();
                AddGridHeader();
                AddRow(1);
                chkbycash.Checked = true;
                chkbycash.Checked = false;
                btnJournalEntry.Enabled = false;
                lblSerialHeader.Visible = lblSerialNo.Visible = false;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void DateConvert(DateTime DotNetDate)
        {
            txtDate.Text = Date.ToSystem(DotNetDate);
        }
        
        public void AddEmployee(int EmpID, string Code, string Name, string Designation, string BankAC, bool IsSelected)
        {
            try
            {
                if(IsSelected)
                {
                    int CurRow = grdSalarySheet.Selection.GetSelectionRegion().GetRowsIndex()[0];

                    ctx.Text = Name;
                    //grdSalarySheet[CurRow, 2].Value = "";
                    //grdSalarySheet[CurRow, 2].Value = Name;
                    grdSalarySheet[CurRow, 3].Value = Designation;
                    grdSalarySheet[CurRow, 4].Value = BankAC;
                    grdSalarySheet[CurRow, 11].Value = EmpID;

                    int RowsCount = grdSalarySheet.RowsCount;
                    string LastServicesCell = (string)grdSalarySheet[RowsCount - 1, 2].Value;
                    if (LastServicesCell != "(NEW)" || (CurRow + 1 == RowsCount))
                    {
                        AddRow(RowsCount);
                    }
                }
                hasChanged = true;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void AddGridHeader()
        {
            grdSalarySheet.Rows.Clear();
            grdSalarySheet.Redim(1, 12);
            //grdSalarySheet.AutoSizeMode = SourceGrid.AutoSizeMode.EnableStretch;
            grdSalarySheet [0,0] = new SourceGrid.Cells.ColumnHeader("Del");
            grdSalarySheet[0, 0].Column.Width = 30;
            grdSalarySheet.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 1] = new SourceGrid.Cells.ColumnHeader("S.N.");
            grdSalarySheet.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            //grdSalarySheet[0, 2] = new SourceGrid.Cells.ColumnHeader("Code");
            //grdSalarySheet[0, 2].Column.Width = 150;
            //grdSalarySheet.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 2] = new SourceGrid.Cells.ColumnHeader("Name");
            grdSalarySheet[0, 2].Column.Width = 190;
            grdSalarySheet.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            //grdSalarySheet[0, (int)SalesInvoiceGridColumn.ProductName].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 3] = new SourceGrid.Cells.ColumnHeader("Designation");
            grdSalarySheet[0, 3].Column.Width = 100;
            grdSalarySheet.Columns[3].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 4] = new SourceGrid.Cells.ColumnHeader("Bank A/C No.");
            grdSalarySheet[0, 4].Column.Width = 80;
            grdSalarySheet.Columns[4].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 5] = new SourceGrid.Cells.ColumnHeader("No. of Class");
            grdSalarySheet[0, 5].Column.Width = 70;
            grdSalarySheet.Columns[5].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 6] = new SourceGrid.Cells.ColumnHeader("Rate");
            grdSalarySheet[0, 6].Column.Width = 80;
            grdSalarySheet.Columns[6].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 7] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdSalarySheet[0, 7].Column.Width = 100;
            grdSalarySheet.Columns[7].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 8] = new SourceGrid.Cells.ColumnHeader("Tax(15%)");
            grdSalarySheet[0, 8].Column.Width = 80;
            grdSalarySheet.Columns[8].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 9] = new SourceGrid.Cells.ColumnHeader("Net Amount");
            grdSalarySheet[0, 9].Column.Width = 100;
            grdSalarySheet.Columns[9].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 10] = new SourceGrid.Cells.ColumnHeader("Remarks");
            grdSalarySheet[0, 10].Column.Width = 150;
            grdSalarySheet.Columns[10].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 11] = new SourceGrid.Cells.ColumnHeader("EmployeeID");
            //grdSalarySheet[0, 12] = new SourceGrid.Cells.ColumnHeader("DesignationID");

            grdSalarySheet[0, 11].Column.Visible = false;
            
            grdSalarySheet.AutoStretchColumnsToFitWidth = true;
            //grdSalarySheet.AutoSizeCells();
            //grdSalarySheet.Columns.StretchToFit();
        }

        private void AddRow(int RowCount)
        {
            try
            {
                int i = RowCount;
                grdSalarySheet.Redim(RowCount + 1, grdSalarySheet.ColumnsCount);

                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::HRM.Properties.Resources.gnome_window_close;
                grdSalarySheet[i, 0] = btnDelete;
                grdSalarySheet[i, 0].AddController(evtDelete);

                grdSalarySheet[i, 1] = new SourceGrid.Cells.Cell(i.ToString());

                SourceGrid.Cells.Editors.TextBox txtName = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtName.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                txtName.Control.GotFocus += new EventHandler(Employee_Focused);
                txtName.Control.LostFocus += new EventHandler(Employee_Leave);
                grdSalarySheet[i, 2] = new SourceGrid.Cells.Cell("", txtName);
                grdSalarySheet[i, 2].Value = "(NEW)";

                grdSalarySheet[i, 3] = new SourceGrid.Cells.Cell("");
                grdSalarySheet[i, 4] = new SourceGrid.Cells.Cell("");

                SourceGrid.Cells.Editors.TextBox txtQty = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtQty.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                txtQty.Control.LostFocus += new EventHandler(Qty_Modified);
                grdSalarySheet[i, 5] = new SourceGrid.Cells.Cell("", txtQty);
                grdSalarySheet[i, 5].Value = "1";

                SourceGrid.Cells.Editors.TextBox txtRate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRate.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                txtRate.Control.LostFocus += new EventHandler(Rate_Modified);
                grdSalarySheet[i, 6] = new SourceGrid.Cells.Cell("", txtRate);
                grdSalarySheet[i, 6].Value = "0";

                grdSalarySheet[i, 7] = new SourceGrid.Cells.Cell("0");

                grdSalarySheet[i, 8] = new SourceGrid.Cells.Cell("0");

                grdSalarySheet[i, 9] = new SourceGrid.Cells.Cell("0");

                grdSalarySheet[i, 10] = new SourceGrid.Cells.Cell("");

                grdSalarySheet[i, 11] = new SourceGrid.Cells.Cell("");
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void CalculateAmount(int CurRow,decimal QtyAmt,AmtQtyType AQ)
        {
            try
            {
                decimal qty = 0, rate = 0, amount = 0, tax = 0, netAmt = 0;
                if (AQ == AmtQtyType.Qty)
                    qty = QtyAmt;
                else
                    qty = Convert.ToDecimal(grdSalarySheet[CurRow, 5].Value.ToString());

                if (AQ == AmtQtyType.Rate)
                    rate = QtyAmt;
                else
                    rate = Convert.ToDecimal(grdSalarySheet[CurRow, 6].Value.ToString());

                amount = qty * rate;
                tax = amount * (decimal)0.15;
                netAmt = amount - tax;
                grdSalarySheet[CurRow, 7].Value = amount.ToString();
                grdSalarySheet[CurRow, 8].Value = tax.ToString();
                grdSalarySheet[CurRow, 9].Value = netAmt.ToString();
                
                CalculateTotal();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void CalculateTotal()
        {
            decimal tQty = 0, tRate = 0, tAmt = 0, tTax = 0, tNetAmt = 0;
            int CurrRow = 0;
            for(int i = 1;i < grdSalarySheet.RowsCount -1 ; i ++)
            {
                CurrRow = i;
                tQty += Convert.ToDecimal(grdSalarySheet[CurrRow, 5].Value);
                tRate += Convert.ToDecimal(grdSalarySheet[CurrRow, 6].Value);
                tAmt += Convert.ToDecimal(grdSalarySheet[CurrRow, 7].Value);
                tTax += Convert.ToDecimal(grdSalarySheet[CurrRow, 8].Value);
                tNetAmt += Convert.ToDecimal(grdSalarySheet[CurrRow, 9].Value);
            }

            lblQty.Text = tQty.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            //lblRate.Text = tRate.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            lblAmount.Text = tAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            lblTax.Text = tTax.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            lblNetAmount.Text = tNetAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
        }

        private bool isNewRow(int CurRow)
        {
            if (grdSalarySheet[CurRow, 2].Value.ToString() == "(NEW)")
                return true;
            else
                return false;

        }

        private void LoadComboboxItems(ComboBox comboboxitems, bool isBank)
        {
            comboboxitems.Items.Clear();
            if (comboboxitems == cmbbankname && isBank == true)
            {
                DataTable dtbankname = Hrm.getBankName();
                foreach (DataRow drbank in dtbankname.Rows)
                {
                    comboboxitems.Items.Add(new ListItem((int)drbank["BankID"], drbank["BankName"].ToString()));

                }
                comboboxitems.SelectedIndex = -1;
                comboboxitems.DisplayMember = "value";
                comboboxitems.ValueMember = "id";
            }
            else if (comboboxitems == cmbbankname && isBank == false)
            {
                DataTable dtCashLedgers = Ledger.GetAllLedger(102);
                foreach (DataRow drbank in dtCashLedgers.Rows)
                {
                    comboboxitems.Items.Add(new ListItem((int)drbank["LedgerID"], drbank["EngName"].ToString()));

                }
                comboboxitems.SelectedIndex = -1;
                comboboxitems.DisplayMember = "value";
                comboboxitems.ValueMember = "id";
            }
        }
        private void chkbycash_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbycash.Checked)
            {
                lblCashParty.Text = "Cash A/C:";
                LoadComboboxItems(cmbbankname, false);
            }
            else
            {
                lblCashParty.Text = "Bank Name:";
                LoadComboboxItems(cmbbankname, true);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               bool chkUserPermission = UserPermission.ChkUserPermission("HRM_PTSS_SAVE");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to save salary sheet. Please contact your administrator for permission.");
                   return;
                }

                if(txtDate.MaskCompleted == false)
                {
                    Global.Msg("Please fill the date.");
                    return;
                }

                BusinessLogic.HRM.PartTimeSalaryDetails ps = new BusinessLogic.HRM.PartTimeSalaryDetails();
                CompanyDetails CompDetails = new CompanyDetails();
                CompDetails = CompanyInfo.GetInfo();

                ps.CreatedDate = System.Convert.ToDateTime(Date.GetServerDate());
                ps.CreatedBy = User.CurrentUserName;
                ps.Date = Date.ToDotNet(txtDate.Text);
                ps.Narration = txtRemarks.Text;
                ps.Quantity = Convert.ToDecimal(lblQty.Text);
                ps.Amount = Convert.ToDecimal(lblAmount.Text);
                ps.Tax = Convert.ToDecimal(lblTax.Text);
                ps.NetAmount = Convert.ToDecimal(lblNetAmount.Text);
                ps.PartTimeSalaryMasterID = m_PartTimeSalaryMasterID;
                ps.SN = m_PartTimeSalaryMasterID == 0? 0: Convert.ToInt32(lblSerialNo.Text);

                DataTable dtSalaryDetails = new DataTable();
                //dtSalaryDetails.Columns.Add("Code");
                dtSalaryDetails.Columns.Add("Name");
                dtSalaryDetails.Columns.Add("Designation");
                dtSalaryDetails.Columns.Add("BankAC");
                dtSalaryDetails.Columns.Add("QtyClass");
                dtSalaryDetails.Columns.Add("Rate");
                dtSalaryDetails.Columns.Add("Amount");
                dtSalaryDetails.Columns.Add("Tax");
                dtSalaryDetails.Columns.Add("NetAmount");
                dtSalaryDetails.Columns.Add("Remarks");
                dtSalaryDetails.Columns.Add("EmployeeID");

                if(grdSalarySheet.Rows.Count() > 2)
                {
                    for(int i = 1; i <= grdSalarySheet.Rows.Count() - 2; i ++)
                    {
                        dtSalaryDetails.Rows.Add(grdSalarySheet[i, 2].ToString(), grdSalarySheet[i, 3].ToString(), grdSalarySheet[i, 4].ToString(),Convert.ToDecimal( grdSalarySheet[i, 5].ToString()),Convert.ToDecimal( grdSalarySheet[i, 6].ToString()), Convert.ToDecimal(grdSalarySheet[i, 7].ToString()), Convert.ToDecimal(grdSalarySheet[i, 8].ToString()), Convert.ToDecimal(grdSalarySheet[i, 9].ToString()), grdSalarySheet[i, 10].ToString(),Convert.ToInt32(grdSalarySheet[i, 11].ToString()));
                    }
                }
                else
                {
                    Global.Msg("Please fill the form at least with an employee.");
                    return;
                }

                BusinessLogic.HRM.Employee.SavePartTimeSalary(ps, dtSalaryDetails);
                m_PartTimeSalaryMasterID = 0;
                ClearForm();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //Clear Form for new entry

            m_PartTimeSalaryMasterID = 0;
            ClearForm();
            
        }

        /// <summary>
        /// Clears Form like new
        /// </summary>
        private  void ClearForm()
        {
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.

            //Event trigerred when delete button is clicked
            evtDelete.Click += new EventHandler(Delete_Row_Click);

            //LoadCboCashBank();
            AddGridHeader();
            AddRow(1);
            chkbycash.Checked = true;
            chkbycash.Checked = false;
            lblSerialHeader.Visible = lblSerialNo.Visible = btnJournalEntry.Enabled = false;
            lblMsg.Text = "";
            lblQty.Text = lblAmount.Text = lblTax.Text = lblNetAmount.Text = "0";
        }

        private void Navigation(Navigate NavTo)
        {
            try
            {
                bool chkUserPermission = UserPermission.ChkUserPermission("HRM_PTSS_VIEW");
                if (chkUserPermission == false)
                {
                   Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                    return;
                }

                int VouchID = 0;
                if (m_PartTimeSalaryMasterID > 0)
                {
                    VouchID = m_PartTimeSalaryMasterID;
                }
                else
                {
                    VouchID = 999999999;
                }
                DataTable dtMaster = new DataTable();
                dtMaster = BusinessLogic.HRM.Employee.NavigatePartTimeSalary(VouchID, NavTo);
                if(dtMaster.Rows.Count <= 0)
                {
                    Global.Msg("No more records found!");
                    return;
                }

                ClearForm();

                //Load Form according to dtMaster
                if(Convert.ToInt32(dtMaster.Rows[0]["JournalID"].ToString()) > 0)
                {
                    LoadSavedSalary( dtMaster,false);
                    lblMsg.Text = "Note: Voucher entry has been done for this salary sheet. Thus, the salary sheet can not be modified.";
                    btnSave.Enabled = txtRemarks.Enabled = btnDate.Enabled = false;
                    IsJournalEdit = true;
                    btnJournalEntry.Enabled = true;
                    lblSerialHeader.Visible = lblSerialNo.Visible = true;

                    for (int r = 0; r < grdSalarySheet.RowsCount; r++)
                        for (int c = 0; c < grdSalarySheet.ColumnsCount; c++)
                        {
                            if (grdSalarySheet[r, c].Editor != null)
                                grdSalarySheet[r, c].Editor.EnableEdit = false;
                        }
                }
                else
                {
                    LoadSavedSalary(dtMaster,false);
                    lblMsg.Text = "Note: Voucher entry for this salary sheet is pending.";
                    btnSave.Enabled = txtRemarks.Enabled = btnDate.Enabled = true;
                    IsJournalEdit = false;
                    btnJournalEntry.Enabled = true;
                    lblSerialHeader.Visible = lblSerialNo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void LoadSavedSalary(DataTable dtMaster,bool isCrystal)
        {
            try
            {
                if (!isCrystal)
                {
                    m_PartTimeSalaryMasterID = Convert.ToInt32(dtMaster.Rows[0]["ID"].ToString());
                    txtDate.Text = Date.ToSystem(Convert.ToDateTime(dtMaster.Rows[0]["Date"].ToString()));
                    txtRemarks.Text = dtMaster.Rows[0]["Narration"].ToString();
                    lblSerialNo.Text = dtMaster.Rows[0]["SN"].ToString();

                    lblQty.Text = Convert.ToDecimal(dtMaster.Rows[0]["Quantity"]).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    lblAmount.Text = Convert.ToDecimal(dtMaster.Rows[0]["Amount"]).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    lblTax.Text = Convert.ToDecimal(dtMaster.Rows[0]["Tax"]).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    lblNetAmount.Text = Convert.ToDecimal(dtMaster.Rows[0]["NetAmount"]).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                }
                DataTable dtDetail = BusinessLogic.HRM.Employee.GetPartTimeSalaryDetail(m_PartTimeSalaryMasterID);
                int CurrRow = 1;
                if (dtDetail.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        if (!isCrystal)
                        {
                            grdSalarySheet[CurrRow, 2].Value = dr["EmpName"].ToString();
                            grdSalarySheet[CurrRow, 3].Value = dr["Designation"].ToString();
                            grdSalarySheet[CurrRow, 4].Value = dr["BankACNo"].ToString();
                            grdSalarySheet[CurrRow, 5].Value = Convert.ToDecimal(dr["ClassQty"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 6].Value = Convert.ToDecimal(dr["Rate"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 7].Value = Convert.ToDecimal(dr["Amount"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 8].Value = Convert.ToDecimal(dr["Tax"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 9].Value = Convert.ToDecimal(dr["NetAmount"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 10].Value = dr["Remarks"].ToString();
                            grdSalarySheet[CurrRow, 11].Value = dr["EmployeeID"].ToString();
                            CurrRow = CurrRow + 1;
                            AddRow(CurrRow);
                        }
                        else
                        {
                            dsPTS.Tables["tblPartTimeSalaryDetail"].Rows.Add(dr["EmpName"].ToString(), dr["Designation"].ToString(), dr["BankACNo"].ToString(), Convert.ToDecimal(dr["ClassQty"].ToString()), Convert.ToDecimal(dr["Rate"].ToString()), Convert.ToDecimal(dr["Amount"].ToString()), Convert.ToDecimal(dr["Tax"].ToString()), Convert.ToDecimal(dr["NetAmount"].ToString()), dr["Remarks"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            Navigation(Navigate.Last);
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            Navigation(Navigate.First);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Navigation(Navigate.Next);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Navigation(Navigate.Prev);
        }

        private void btnJournalEntry_Click(object sender, EventArgs e)
        {
            try
            {
                bool chkUserPermission = UserPermission.ChkUserPermission("HRM_PTSS_JOURNAL_ENTRY");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to enter journal for salary sheet. Please contact your administrator for permission.");
                    return;
                }
                
                
                    int journalId = 0;
                    DataTable dtMaster = new DataTable();
                    dtMaster = BusinessLogic.HRM.Employee.NavigatePartTimeSalary(m_PartTimeSalaryMasterID, Navigate.ID);
                    if (dtMaster.Rows.Count > 0)
                    {
                        if (IsJournalEdit)
                        {
                            if (dtMaster.Rows[0]["JournalID"] == DBNull.Value || Convert.ToInt32(dtMaster.Rows[0]["JournalID"]) <= 0)
                            {
                                Global.Msg("Journal voucher not found.");
                                return;
                            }
                            else
                            {
                                journalId = Convert.ToInt32(dtMaster.Rows[0]["JournalID"].ToString());
                                object[] param = new object[2];
                                param[0] = journalId;
                                param[1] = this;
                                m_MDIForm.OpenFormArrayParam("frmJournalEditSalary", param);
                            }
                        }
                        else
                        {
                            if (cmbbankname.SelectedIndex == -1)
                            {
                                Global.Msg("Please select a bank or a cash account for journal voucher");
                                return;
                            }

                            if (DialogResult.Yes == Global.MsgQuest("Please check the salary sheet properly before proceeding. You will not be able to edit this form after the voucher posting has been completed.\nDo you want to continue without checking again?"))
                            {
                                decimal tSalary = Convert.ToDecimal(dtMaster.Rows[0]["Amount"].ToString());
                                decimal tTaxDeduct = Convert.ToDecimal(dtMaster.Rows[0]["Tax"].ToString());
                                decimal tNetSalary = Convert.ToDecimal(dtMaster.Rows[0]["NetAmount"].ToString());

                                DataTable dtJournalDetails = new DataTable();
                                dtJournalDetails.Columns.Add("LedgerID");
                                dtJournalDetails.Columns.Add("LedgerName");
                                dtJournalDetails.Columns.Add("DrCr");
                                dtJournalDetails.Columns.Add("Amount");

                                string ledgerName = "";

                                if (tSalary != 0)
                                {
                                    ledgerName = Ledger.GetLedgerNameFromID(Global.SalaryAcID);
                                    dtJournalDetails.Rows.Add(Global.SalaryAcID, ledgerName, "Debit", tSalary);
                                }

                                if (tTaxDeduct != 0)
                                {
                                    ledgerName = Ledger.GetLedgerNameFromID(Global.TDSonSalaryID);

                                    dtJournalDetails.Rows.Add(Global.TDSonSalaryID, ledgerName, "Credit", tTaxDeduct);
                                }

                                //Selects ledger id for cash account if 'By Cash' is selected or for bank if 'By Cash' is not selected
                                ListItem liLedgerID = new ListItem();
                                liLedgerID = (ListItem)cmbbankname.SelectedItem;
                                int bankId = Convert.ToInt32(liLedgerID.ID);

                                if (tNetSalary != 0)
                                {
                                    ledgerName = Ledger.GetLedgerNameFromID(bankId);

                                    dtJournalDetails.Rows.Add(bankId, ledgerName, "Credit", tNetSalary);

                                }
                                object[] param = new object[2];
                                param[0] = dtJournalDetails;
                                param[1] = this;
                                m_MDIForm.OpenFormArrayParam("frmJournalNewSalary", param);
                            }
                        }
                    }
                    else
                    {
                        Global.Msg("No records found.");
                        return;
                    }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void InsertJournalId(int journalID)
        {
            Employee.PTVoucherEntered(m_PartTimeSalaryMasterID, journalID);
            Navigation(Navigate.ID);
        }

        public void DeleteJournalId(int journalID)
        {
            Employee.PTVoucherDeleted(m_PartTimeSalaryMasterID, journalID);
            Navigation(Navigate.ID);
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            DateTime dtDate = Date.ToDotNet(txtDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            prntDirect = 0;
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void PrintPreviewCR(PrintType PrintType)
        {
            try
            {
                
                if(m_PartTimeSalaryMasterID == 0)
                {
                    Global.Msg("Please save the salary sheet first.");
                    return;
                }
                dsPTS.Clear();
                DataTable dtMaster = new DataTable();
                dtMaster = BusinessLogic.HRM.Employee.NavigatePartTimeSalary(m_PartTimeSalaryMasterID, Navigate.ID);
                if(dtMaster.Rows.Count > 0)
                {
                    CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    CrDiskFileDestinationOptions.DiskFileName = FileName;

                    LoadSavedSalary(dtMaster, true);
                    HRM.Reports.rptPartTimeSalary rpt = new HRM.Reports.rptPartTimeSalary();

                    //Fill the logo on the report
                    Misc.WriteLogo(dsPTS, "tblImage");

                    rpt.SetDataSource(dsPTS);

                    //Provide values to the parameters on the report
                    CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    //CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvDate = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();

                    CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvQty = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvTax = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvNetAmount = new CrystalDecisions.Shared.ParameterDiscreteValue();
                    CrystalDecisions.Shared.ParameterDiscreteValue pdvNarration = new CrystalDecisions.Shared.ParameterDiscreteValue();

                    pdvFont.Value = "Arial";
                    pvCollection.Clear();
                    pvCollection.Add(pdvFont);
                    rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

                    CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();
                    int uid = User.CurrUserID;
                    DataTable dtroleinfo = User.GetUserInfo(uid);
                    DataRow drrole = dtroleinfo.Rows[0];
                    int roleid = Convert.ToInt32(drrole["AccessRoleID"].ToString());

                    if (roleid == 37)//if user is root, get information from tblCompany
                    {
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


                        //pdvCompany_Slogan.Value = m_CompanyDetails.CSlogan;
                        //pvCollection.Clear();
                        //pvCollection.Add(pdvCompany_Slogan);
                        //rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);
                    }
                    else //if user is not root, take information from tblUserPreference
                    {
                        string companyname = UserPreference.GetValue("COMPANY_NAME", uid);
                        string companyaddress = UserPreference.GetValue("COMPANY_ADDRESS", uid);
                        string companycity = UserPreference.GetValue("COMPANY_CITY", uid);
                        string companypan = UserPreference.GetValue("COMPANY_PAN", uid);
                        string companyphone = UserPreference.GetValue("COMPANY_PHONE", uid);
                        string companyslogan = UserPreference.GetValue("COMPANY_SLOGAN", uid);

                        pdvCompany_Name.Value = companyname;
                        pvCollection.Clear();
                        pvCollection.Add(pdvCompany_Name);
                        rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

                        pdvCompany_Address.Value = companyaddress + ((companyaddress.Trim().Length > 0) && (companycity.Trim().Length > 0) ? ", " : "") + companycity; //Display comma in the middle only if both are available
                        pvCollection.Clear();
                        pvCollection.Add(pdvCompany_Address);
                        rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

                        pdvCompany_PAN.Value = companypan;
                        pvCollection.Clear();
                        pvCollection.Add(pdvCompany_PAN);
                        rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

                        pdvCompany_Phone.Value = "Phone No.: " + companyphone;
                        pvCollection.Clear();
                        pvCollection.Add(pdvCompany_Phone);
                        rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

                        //pdvCompany_Slogan.Value = companyslogan;
                        //pvCollection.Clear();
                        //pvCollection.Add(pdvCompany_Slogan);
                        //rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

                    }
                    pdvReport_Head.Value = "Part Time Salary Sheet";
                    pvCollection.Clear();
                    pvCollection.Add(pdvReport_Head);
                    rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

                    pdvDate.Value = txtDate.Text;
                    pvCollection.Clear();
                    pvCollection.Add(pdvDate);
                    rpt.DataDefinition.ParameterFields["Date"].ApplyCurrentValues(pvCollection);

                    pdvReport_Date.Value = "As On Date:" + Date.ToSystem(DateTime.Today);
                    pvCollection.Clear();
                    pvCollection.Add(pdvReport_Date);
                    rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);

                    pdvQty.Value = dtMaster.Rows[0]["Quantity"].ToString();
                    pvCollection.Clear();
                    pvCollection.Add(pdvQty);
                    rpt.DataDefinition.ParameterFields["Qty"].ApplyCurrentValues(pvCollection);

                    pdvAmount.Value = dtMaster.Rows[0]["Amount"].ToString();
                    pvCollection.Clear();
                    pvCollection.Add(pdvAmount);
                    rpt.DataDefinition.ParameterFields["Amount"].ApplyCurrentValues(pvCollection);

                    pdvTax.Value = dtMaster.Rows[0]["Tax"].ToString();
                    pvCollection.Clear();
                    pvCollection.Add(pdvTax);
                    rpt.DataDefinition.ParameterFields["Tax"].ApplyCurrentValues(pvCollection);

                    pdvNetAmount.Value = dtMaster.Rows[0]["NetAmount"].ToString();
                    pvCollection.Clear();
                    pvCollection.Add(pdvNetAmount);
                    rpt.DataDefinition.ParameterFields["NetAmount"].ApplyCurrentValues(pvCollection);

                    string narration = dtMaster.Rows[0]["Narration"].ToString();
                    pdvNarration.Value = narration == "" ? "" : "Narration: " + narration;
                    pvCollection.Clear();
                    pvCollection.Add(pdvNarration);
                    rpt.DataDefinition.ParameterFields["Narration"].ApplyCurrentValues(pvCollection);

                    frmReportViewer frm = new frmReportViewer();
                    frm.SetReportSource(rpt);

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
                            frmemail sendemail = new frmemail(FileName, 1);
                            sendemail.ShowDialog();
                            rpt.Close();
                            return;
                        default:
                            frm.Show();
                            frm.WindowState = FormWindowState.Maximized;
                            break;
                    }
                }
                

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            prntDirect = 1;
            PrintPreviewCR(PrintType.CrystalReport);
        }

        private void btnGoto_Click(object sender, EventArgs e)
        {
            GotoSerialNo();
        }

        private void GotoSerialNo()
        {
            try
            {
                if (txtGoto.Text == "")
                {
                    Global.Msg("Please insert a serial number.");
                    return;
                }

                if (!UserValidation.validatecontactnumber(txtGoto.Text))
                {
                    Global.Msg("Please insert a valid number.");
                    return;
                }

                int masterId = Employee.PTGetIDbySN(Convert.ToInt32(txtGoto.Text));
                if(masterId > 0)
                {
                    m_PartTimeSalaryMasterID = masterId;
                    Navigation(Navigate.ID);
                    txtGoto.Clear();
                }
                else
                {
                    Global.Msg("The serial number does not exist.");
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void txtGoto_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
                GotoSerialNo();
        }

        private void txtGoto_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grdSalarySheet_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }
    }
}
