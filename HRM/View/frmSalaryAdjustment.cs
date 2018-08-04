using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HRM.View
{
    public partial class frmSalaryAdjustment : Form
    {
        int m_EmployeeID = 0;
        string m_Name = "";
        string m_Designation = "";
        string m_StaffCode = "";
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        frmPaySlip m_paySlip = null;
        public enum GridColumns { SN, Del, Title, Task, Amount, Remarks }

        public frmSalaryAdjustment()
        {
            InitializeComponent();
        }
        public frmSalaryAdjustment(DataTable dtAdjsut, int employeeID, string name, string designation, string staffCode, frmPaySlip paySlip)
        {
            InitializeComponent();
            m_dtAdjustment = dtAdjsut;
            m_EmployeeID = employeeID;
            m_Name = name;
            m_StaffCode = staffCode;
            m_Designation = designation;
            m_paySlip = paySlip;
        }

        private void frmSalaryAdjustment_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_EmployeeID > 0)
                {
                    txtDesignation.Text = m_Designation;
                    txtStaffCode.Text = m_StaffCode;
                    txtStaffName.Text = m_Name;
                }
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                WriteHeader();
                AddRowAdjustment(1);
                if (m_dtAdjustment.Rows.Count > 0)
                {
                    for (int count = 1; count <= m_dtAdjustment.Rows.Count; count++)
                    {
                        DataRow dr = m_dtAdjustment.Rows[count - 1];
                        grdSalaryAdjustment[count, (int)GridColumns.SN].Value = count.ToString();
                        grdSalaryAdjustment[count, (int)GridColumns.Title].Value = dr["Name"].ToString();

                        decimal amount = Convert.ToDecimal(dr["Amount"]);

                        grdSalaryAdjustment[count, (int)GridColumns.Task].Value = amount >=0?"Adjust(+)":"Adjust(-)";
                        grdSalaryAdjustment[count, (int)GridColumns.Amount].Value = amount < 0 ? amount *-1 : amount;
                        grdSalaryAdjustment[count, (int)GridColumns.Remarks].Value = dr["Remarks"].ToString();
                        AddRowAdjustment(grdSalaryAdjustment.Rows.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public void WriteHeader()
        {
            grdSalaryAdjustment.Redim(1, 6);
            grdSalaryAdjustment[0, (int)GridColumns.SN] = new SourceGrid.Cells.Cell("SN");
            grdSalaryAdjustment[0, (int)GridColumns.SN].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdSalaryAdjustment[0, (int)GridColumns.SN].Column.Width = 30;

            grdSalaryAdjustment[0, (int)GridColumns.Del] = new SourceGrid.Cells.Cell("Del");
            grdSalaryAdjustment[0, (int)GridColumns.Del].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdSalaryAdjustment[0, (int)GridColumns.Del].Column.Width = 30;

            grdSalaryAdjustment[0, (int)GridColumns.Title] = new SourceGrid.Cells.Cell("Title");
            grdSalaryAdjustment[0, (int)GridColumns.Title].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            grdSalaryAdjustment[0, (int)GridColumns.Title].Column.Width = 100;

            grdSalaryAdjustment[0, (int)GridColumns.Task] = new SourceGrid.Cells.Cell("Task");
            grdSalaryAdjustment[0, (int)GridColumns.Task].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdSalaryAdjustment[0, (int)GridColumns.Task].Column.Width = 90;

            grdSalaryAdjustment[0, (int)GridColumns.Amount] = new SourceGrid.Cells.Cell("Amount");
            grdSalaryAdjustment[0, (int)GridColumns.Amount].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdSalaryAdjustment[0, (int)GridColumns.Amount].Column.Width = 70;

            grdSalaryAdjustment[0, (int)GridColumns.Remarks] = new SourceGrid.Cells.Cell("Remarks");
            grdSalaryAdjustment[0, (int)GridColumns.Remarks].Column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
            grdSalaryAdjustment[0, (int)GridColumns.Remarks].Column.Width = 180;

            grdSalaryAdjustment.AutoStretchColumnsToFitWidth = true;
        }

        public void AddRowAdjustment(int RowCount)
        {
            //Add a new row
            try
            {
                grdSalaryAdjustment.Redim(Convert.ToInt32(RowCount + 1), grdSalaryAdjustment.ColumnsCount);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("X");
                int i = RowCount;
                grdSalaryAdjustment[i, (int)GridColumns.Del] = btnDelete;
                grdSalaryAdjustment[i, (int)GridColumns.Del].AddController(evtDelete);
                grdSalaryAdjustment[i, (int)GridColumns.SN] = new SourceGrid.Cells.Cell(i.ToString());

                SourceGrid.Cells.Editors.TextBox txtTask = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtTask.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdSalaryAdjustment[i, (int)GridColumns.Title] = new SourceGrid.Cells.Cell("(NEW)", txtTask);
                    
                SourceGrid.Cells.Editors.ComboBox cmbAdjustTask = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                cmbAdjustTask.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                cmbAdjustTask.Control.DropDownStyle = ComboBoxStyle.DropDown;
                cmbAdjustTask.StandardValues = new string[] { "Adjust(+)", "Adjust(-)" };
                grdSalaryAdjustment[i, (int)GridColumns.Task] = new SourceGrid.Cells.Cell("", cmbAdjustTask);
                   
                SourceGrid.Cells.Editors.TextBox txtAmt = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtAmt.Control.LostFocus += new EventHandler(Amount_Modified);
                txtAmt.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdSalaryAdjustment[i, (int)GridColumns.Amount] = new SourceGrid.Cells.Cell("0", txtAmt);
                   

                grdSalaryAdjustment[i, (int)GridColumns.Remarks] = new SourceGrid.Cells.Cell("");

                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdSalaryAdjustment[i, (int)GridColumns.Remarks] = new SourceGrid.Cells.Cell("", txtRemarks);
                   

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Amount_Modified(object sender, EventArgs e)
        {
            try
            {
                int curRow = grdSalaryAdjustment.Selection.GetSelectionRegion().GetRowsIndex()[0];
                decimal qty = Convert.ToDecimal(((TextBox)sender).Text);
                if (!Misc.IsNumeric(qty))
                {
                    Global.MsgError("Amount is not in correct format ! ");

                    ((TextBox)sender).Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
                }

                if(curRow  == (grdSalaryAdjustment.Rows.Count -1))
                    AddRowAdjustment(grdSalaryAdjustment.Rows.Count);
            }
            catch (Exception ex)
            {
                Global.MsgError("Amount is not in correct format ! " +ex.Message);
                ((TextBox)sender).Text = (0).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            }
        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;

                //Do not delete if its the last Row because it contains (NEW)
                if (ctx.Position.Row <= grdSalaryAdjustment.RowsCount - 2)
                    grdSalaryAdjustment.Rows.Remove(ctx.Position.Row);           
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        DataTable m_dtAdjustment = null;
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                m_dtAdjustment = new DataTable();
                m_dtAdjustment.Columns.Add("EmployeeID", typeof(int));
                m_dtAdjustment.Columns.Add("Name", typeof(string));
                m_dtAdjustment.Columns.Add("Amount", typeof(decimal));
                m_dtAdjustment.Columns.Add("Remarks", typeof(string));
                //dtAdjustment.Columns.Add("PaySlipID");

                for (int i = 1; i <= grdSalaryAdjustment.Rows.Count - 2; i++)
                {
                   decimal amount = Convert.ToDecimal(grdSalaryAdjustment[i, (int)GridColumns.Amount].Value);
                   string task = grdSalaryAdjustment[i, (int)GridColumns.Task].Value.ToString();
                   string name = grdSalaryAdjustment[i, (int)GridColumns.Title].Value.ToString();
                   if (name == "(NEW)" || amount == 0)
                       continue;

                   amount = task == "Adjust(+)"? amount: amount * (-1);
                   m_dtAdjustment.Rows.Add(m_EmployeeID, name, amount, grdSalaryAdjustment[i, (int)GridColumns.Remarks].Value);
                }

                m_paySlip.GetAdjustment(m_dtAdjustment);

                this.Close();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
