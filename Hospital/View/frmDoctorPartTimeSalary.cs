using BusinessLogic;
using BusinessLogic.HRM;
using Common;
using DateManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hospital.View
{
    public partial class frmDoctorPartTimeSalary : Form, IfrmDateConverter, IDoctorList
    {
        public frmDoctorPartTimeSalary()
        {
            InitializeComponent();
        }
        private IMDIMainForm m_MDIForm;
        public frmDoctorPartTimeSalary(IMDIMainForm m_diForm)
        {
            m_MDIForm = (IMDIMainForm)m_diForm;
            InitializeComponent();
        }


        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        bool hasChanged = false;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private int m_PartTimeSalaryMasterID = 0;   
        private enum AmtQtyType { Rate, Qty }
        private void frmDoctorPartTimeSalary_Load(object sender, EventArgs e)
        {
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            evtDelete.Click += new EventHandler(Delete_Row_Click);
            AddGridHeader();
            AddRow(1);
            chkbycash.Checked = true;
            chkbycash.Checked = false;
            lblSerialHeader.Visible = lblSerialNo.Visible = false;
        }
        //public void DateConvert(DateTime DotNetDate)
        //{
        //    txtDate.Text = Date.ToSystem(DotNetDate);
        //}
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
        private void CalculateTotal()
        {
            decimal tQty = 0, tRate = 0, tAmt = 0, tTax = 0, tNetAmt = 0;
            int CurrRow = 0;
            for (int i = 1; i < grdSalarySheet.RowsCount - 1; i++)
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

        private void AddGridHeader()
        {
            grdSalarySheet.Rows.Clear();
            grdSalarySheet.Redim(1, 12);
           
            grdSalarySheet[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
            grdSalarySheet[0, 0].Column.Width = 30;
            grdSalarySheet.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 1] = new SourceGrid.Cells.ColumnHeader("S.N.");
            grdSalarySheet.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 2] = new SourceGrid.Cells.ColumnHeader("DoctorName");
            grdSalarySheet[0, 2].Column.Width = 190;
            grdSalarySheet.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
         
            grdSalarySheet[0, 3] = new SourceGrid.Cells.ColumnHeader("Specilization");
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

            grdSalarySheet[0, 11] = new SourceGrid.Cells.ColumnHeader("DoctorID");
           

            grdSalarySheet[0, 11].Column.Visible = false;

            grdSalarySheet.AutoStretchColumnsToFitWidth = true;
           
        }

        private bool isNewRow(int CurRow)
        {
            if (grdSalarySheet[CurRow, 2].Value.ToString() == "(NEW)")
                return true;
            else
                return false;

        }

        private void Qty_Modified(object sender, EventArgs e)
        {
            try
            {
                //find the current row of source grid
                int CurRow = grdSalarySheet.Selection.GetSelectionRegion().GetRowsIndex()[0];

                if (isNewRow(CurRow))
                    return;

                if (grdSalarySheet[CurRow, 5].Value.ToString() != "")
                {
                    if (!UserValidation.validDecimal(grdSalarySheet[CurRow, 5].Value.ToString()))
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

                CalculateAmount(CurRow, Convert.ToDecimal(Qty), AmtQtyType.Qty);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void CalculateAmount(int CurRow, decimal QtyAmt, AmtQtyType AQ)
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
        private void Doctor_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;

              frmDoctorList fle = new frmDoctorList(this);
                fle.ShowDialog();
                SendKeys.Send("{TAB}{TAB}{TAB}");
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

                CalculateAmount(CurRow, Convert.ToDecimal(Rate), AmtQtyType.Rate);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void Doctor_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            int RowCount = grdSalarySheet.RowsCount;
            string LastServicesCell = (string)grdSalarySheet[RowCount - 1, 2].Value;

            if (LastServicesCell != "(NEW)")
            {
                AddRow(RowCount);
            }
        }
        private void AddRow(int RowCount)
        {
            try
            {
                int i = RowCount;
                grdSalarySheet.Redim(RowCount + 1, grdSalarySheet.ColumnsCount);

                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Hospital.Properties.Resources.gnome_window_close;
                grdSalarySheet[i, 0] = btnDelete;
                grdSalarySheet[i, 0].AddController(evtDelete);

                grdSalarySheet[i, 1] = new SourceGrid.Cells.Cell(i.ToString());

                SourceGrid.Cells.Editors.TextBox txtName = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtName.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                txtName.Control.GotFocus += new EventHandler(Doctor_Focused);
                txtName.Control.LostFocus += new EventHandler(Doctor_Leave);
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

        private void btnDate_Click(object sender, EventArgs e)
        {
            DateTime dtDate = Date.ToDotNet(txtDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }


      



        public void AddDoctor(int DocID, string Code, string Name, string Specilization, string BankAC, bool IsSelected)
        {
            try
            {
                if (IsSelected)
                {
                    int CurRow = grdSalarySheet.Selection.GetSelectionRegion().GetRowsIndex()[0];

                    ctx.Text = Name;
                    //grdSalarySheet[CurRow, 2].Value = "";
                    //grdSalarySheet[CurRow, 2].Value = Name;
                    grdSalarySheet[CurRow, 3].Value = Specilization;
                    grdSalarySheet[CurRow, 4].Value = BankAC;
                    grdSalarySheet[CurRow, 11].Value = DocID;

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

        public void DateConvert(DateTime ReturnDotNetDate)
        {
            txtDate.Text = Date.ToSystem(ReturnDotNetDate);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               

                if (txtDate.MaskCompleted == false)
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
                ps.SN = m_PartTimeSalaryMasterID == 0 ? 0 : Convert.ToInt32(lblSerialNo.Text);

                DataTable dtSalaryDetails = new DataTable();
                //dtSalaryDetails.Columns.Add("Code");
                dtSalaryDetails.Columns.Add("Name");
                dtSalaryDetails.Columns.Add("Specilization");
                dtSalaryDetails.Columns.Add("BankAC");
                dtSalaryDetails.Columns.Add("QtyClass");
                dtSalaryDetails.Columns.Add("Rate");
                dtSalaryDetails.Columns.Add("Amount");
                dtSalaryDetails.Columns.Add("Tax");
                dtSalaryDetails.Columns.Add("NetAmount");
                dtSalaryDetails.Columns.Add("Remarks");
                dtSalaryDetails.Columns.Add("DoctorID");

                if (grdSalarySheet.Rows.Count() > 2)
                {
                    for (int i = 1; i <= grdSalarySheet.Rows.Count() - 2; i++)
                    {
                        dtSalaryDetails.Rows.Add(grdSalarySheet[i, 2].ToString(), grdSalarySheet[i, 3].ToString(), grdSalarySheet[i, 4].ToString(), 
                            Convert.ToDecimal(grdSalarySheet[i, 5].ToString()), Convert.ToDecimal(grdSalarySheet[i, 6].ToString()), 
                            Convert.ToDecimal(grdSalarySheet[i, 7].ToString()), Convert.ToDecimal(grdSalarySheet[i, 8].ToString()), 
                            Convert.ToDecimal(grdSalarySheet[i, 9].ToString()), grdSalarySheet[i, 10].ToString(), Convert.ToInt32(grdSalarySheet[i, 11].ToString()));
                    }
                }
                else
                {
                    Global.Msg("Please fill the form at least with an doctor.");
                    return;
                }

                BusinessLogic.HOS.Doctor.SavePartTimeSalary(ps, dtSalaryDetails);
                m_PartTimeSalaryMasterID = 0;
                ClearForm();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void ClearForm()
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            m_PartTimeSalaryMasterID = 0;
            ClearForm();
        }

        private void grdSalarySheet_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
