using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using BusinessLogic.HRM;
using Common;

namespace HRM.View
{
    public partial class frmListOfEmployee : Form, IEmployeeList
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick; //Double click event holder
        private SourceGrid.Cells.Views.Cell RowView;

        private DataRow[] drFound;
        private DataTable dTable;
        private string FilterString = "";

        private bool isSelected = false;

        ListItem liDesignation = new ListItem();
        ListItem liFaculty = new ListItem();


     
       private  IEmployeeList m_ParentForm;

        public frmListOfEmployee()
        {
            InitializeComponent();
        }

        public frmListOfEmployee(IEmployeeList ParentForm)
        {
            m_ParentForm = (IEmployeeList)ParentForm;
            InitializeComponent();
        }

        private void frmListOfEmployee_Load(object sender, EventArgs e)
        {
            LoadForm();
            txtFName.Select();
        }
        
        private void LoadForm()
        {
            BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
            dTable = employees.GetListOfEmployee(null);
            drFound = dTable.Select(FilterString);

            LoadComboboxItems(cmbDesignation);
            LoadComboboxItems(cmbFaculty);
            cmbType.SelectedIndex = 0;
            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(Grid_DoubleClick);
            FillGrid();

        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                isSelected = true;
                int CurRow = grdEmployee.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext EmpID  = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 0));                
                SourceGrid.CellContext Code = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 2));
                SourceGrid.CellContext Name = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 3));
                SourceGrid.CellContext Designation = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 4));
                SourceGrid.CellContext BankAC = new SourceGrid.CellContext(grdEmployee, new SourceGrid.Position(CurRow, 7));
                AddEmployee(Convert.ToInt32(EmpID.Value.ToString()), Code.Value.ToString(), Name.Value.ToString(), Designation.Value.ToString(),BankAC.Value.ToString(),isSelected);
                this.Dispose();
            }
            catch (Exception)
            {
                Global.Msg("Invalid selection");
            }
        }

        public void AddEmployee(int EmpID,string Code,string Name, string Designation,string BankAC, bool isSelected)
        {
         m_ParentForm.AddEmployee(EmpID, Code, Name, Designation,BankAC,isSelected);
        }
        private void FillGrid()
        {
            try
            {
                grdEmployee.Rows.Clear();
                grdEmployee.Redim(drFound.Count() + 1, 8);
                WriteHeader();
                if (drFound.Count() > 0)
                {
                    for (int i = 1; i <= drFound.Count(); i++)
                    {
                        DataRow dr = drFound[i - 1];
                        RowView = new SourceGrid.Cells.Views.Cell();
                        if (i % 2 == 0)
                        {
                            RowView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                        }
                        else
                        {
                            RowView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                        }
                        int typeIndex = Convert.ToInt32(dr["Type"].ToString());
                        grdEmployee[i, 0] = new SourceGrid.Cells.Cell(dr["ID"].ToString());
                        grdEmployee[i, 0].AddController(dblClick);
                        grdEmployee[i, 0].View = RowView;
                        grdEmployee[i, 1] = new SourceGrid.Cells.Cell(i);
                        grdEmployee[i, 1].AddController(dblClick);
                        grdEmployee[i, 1].View = RowView;
                        grdEmployee[i, 2] = new SourceGrid.Cells.Cell(dr["StaffCode"].ToString());
                        grdEmployee[i, 2].AddController(dblClick);
                        grdEmployee[i, 2].View = RowView;
                        grdEmployee[i, 3] = new SourceGrid.Cells.Cell(dr["EmployeeName"].ToString());
                        grdEmployee[i, 3].AddController(dblClick);
                        grdEmployee[i, 3].View = RowView;
                        grdEmployee[i, 4] = new SourceGrid.Cells.Cell(dr["DesignationName"].ToString());
                        grdEmployee[i, 4].AddController(dblClick);
                        grdEmployee[i, 4].View = RowView;
                        grdEmployee[i, 5] = new SourceGrid.Cells.Cell(dr["FacultyName"].ToString());
                        grdEmployee[i, 5].AddController(dblClick);
                        grdEmployee[i, 5].View = RowView;
                        grdEmployee[i, 6] = new SourceGrid.Cells.Cell(typeIndex == 0 ? "Permanent" : typeIndex == 1 ? "Contract" : "Part Time");
                        grdEmployee[i, 6].AddController(dblClick);
                        grdEmployee[i, 6].View = RowView;
                        grdEmployee[i, 7] = new SourceGrid.Cells.Cell(dr["BankACNumber"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void WriteHeader()
        {
            grdEmployee[0, 0] = new SourceGrid.Cells.ColumnHeader("EmpID");
            grdEmployee[0, 0].Column.Visible = false;
            grdEmployee[0, 1] = new SourceGrid.Cells.ColumnHeader("SN");
            grdEmployee[0, 1].Column.Width = 40;
            grdEmployee[0, 2] = new SourceGrid.Cells.ColumnHeader("Code");
            grdEmployee[0, 2].Column.Width = 70;
            grdEmployee[0, 3] = new SourceGrid.Cells.ColumnHeader("Employee Name");
            grdEmployee[0, 3].Column.Width = 270;
            grdEmployee[0, 4] = new SourceGrid.Cells.ColumnHeader("Designation");
            grdEmployee[0, 4].Column.Width = 120;
            grdEmployee[0, 5] = new SourceGrid.Cells.ColumnHeader("Faculty");
            grdEmployee[0, 5].Column.Width = 95;
            grdEmployee[0, 6] = new SourceGrid.Cells.ColumnHeader("Type");
            grdEmployee[0, 6].Column.Width = 95;
            grdEmployee[0, 7] = new SourceGrid.Cells.ColumnHeader("BankAC");
            grdEmployee[0, 7].Column.Visible = false;
        }

        private void LoadComboboxItems(ComboBox comboboxitems)
        {
            try
            {
                comboboxitems.Items.Clear();
                if (comboboxitems == cmbDesignation)
                {
                    DataTable dtdesignation = Hrm.getDesignation();
                    if (dtdesignation.Rows.Count > 0)
                    {
                        comboboxitems.Items.Add(new ListItem(0, "All"));
                        foreach (DataRow drdesignation in dtdesignation.Rows)
                        {
                            comboboxitems.Items.Add(new ListItem((int)drdesignation["DesignationID"], drdesignation["DesignationName"].ToString()));

                        }
                        
                        comboboxitems.DisplayMember = "value";
                        comboboxitems.ValueMember = "id";
                        comboboxitems.SelectedIndex = 0;
                    }
                }
                else if (comboboxitems == cmbFaculty)
                {
                    DataTable dtFaculty = Hrm.GetEmpFacultyForCmb();
                    if (dtFaculty.Rows.Count > 0)
                    {
                        comboboxitems.Items.Add(new ListItem(0, "All"));
                        foreach (DataRow drFaculty in dtFaculty.Rows)
                        {
                            comboboxitems.Items.Add(new ListItem((int)drFaculty["ID"], drFaculty["Value"].ToString()));
                        }
                        comboboxitems.DisplayMember = "Value";
                        comboboxitems.ValueMember = "ID";
                        comboboxitems.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            frmEmployeeRegistration fer = new frmEmployeeRegistration();
            fer.ShowDialog();
            LoadForm();
        }

        private void evtValueChanged(object sender, EventArgs e)
        {
            FilterGrid();
        }

        private void txtFName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                txtMName.Select();
            }
        }

        private void txtMName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                txtLName.Select();
            }
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
                this.grdEmployee.Focus();
            }

            else if (e.KeyCode == Keys.Up)
            {
                this.grdEmployee.Focus();
            }
            //else
            //{
            //    char key = (char)e.KeyData;

            //    if (char.IsLetterOrDigit(key))
            //    {
            //        if (this.txtproductCode.Focused)
            //        {
            //            return;
            //        }
            //        if (this.txtproductCode.Text.Trim().Length > 0)
            //        {
            //            this.txtproductCode.Focus();
            //            this.txtproductCode.Text += key.ToString().ToUpper();
            //            txtproductCode.Text = txtproductCode.Text.ToUpper();
            //            this.txtproductCode.SelectionStart = this.txtproductCode.Text.Length + 1;
            //            return;
            //        }

            //        if (this.txtproductGroup.Text.Trim().Length > 0)
            //        {
            //            this.txtproductGroup.Focus();
            //            this.txtproductGroup.Text += key.ToString().ToUpper();
            //            txtproductGroup.Text = txtproductGroup.Text.ToUpper();
            //            this.txtproductGroup.SelectionStart = this.txtproductGroup.Text.Length + 1;
            //            return;
            //        }

            //        if (!this.txtProductName.Focused)
            //        {
            //            this.txtProductName.Focus();
            //            this.txtProductName.Text += key.ToString().ToUpper();
            //            txtProductName.Text = txtProductName.Text.ToUpper();
            //            this.txtProductName.SelectionStart = this.txtProductName.Text.Length + 1;
            //        }


            //    }
            //}
        }

        private void frmListOfEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            Handle_KeyDown(sender, e);
        }

        private void FilterGrid()
        {
            try
            {
                int id;
                string str = " and e.StaffCode like '" + txtCode.Text.Trim() + "%' and e.FirstName like '" + txtFName.Text.Trim() + "%' and e.MiddleName like '" + txtMName.Text.Trim() + "%' and e.LastName like '" + txtLName.Text.Trim() + "%' ";
                if (cmbDesignation.SelectedIndex > 0)
                {
                    liDesignation = (ListItem)cmbDesignation.SelectedItem;
                    id = liDesignation.ID;
                    str += " and a.Designation = '" + id + "'";
                }

                if (cmbFaculty.SelectedIndex > 0)
                {
                    liFaculty = (ListItem)cmbFaculty.SelectedItem;
                    id = liFaculty.ID;
                    str += " and a.FacultyID = '" + id + "'";
                }

                if (cmbType.SelectedIndex > 0)
                {
                    id = cmbType.SelectedIndex - 1;
                    str += " and a.Type = '" + id + "'";
                }


                BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
                dTable = employees.GetListOfEmployee(str);
                drFound = dTable.Select(FilterString);

              FillGrid();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void frmListOfEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            isSelected = false;
            AddEmployee(0, "", "(NEW)", "", "", isSelected);
            this.Dispose();
        }

        private void grdEmployee_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtCode_TextAlignChanged(object sender, EventArgs e)
        {

        }
    }
}
