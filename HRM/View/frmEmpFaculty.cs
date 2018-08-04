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
    public partial class frmEmpFaculty : Form
    {
        public enum EntryMode { NORMAL, NEW, EDIT }; //Used in entry forms. To find which mode(new, edit or normal) is the form currently running

        DataTable dt;
        DataRow[] drFound;
        private string filterString = "";
        private EntryMode e_mode = EntryMode.NEW;
        private bool IsFieldChanged;

        private SourceGrid.Cells.Controllers.CustomEvents m_Click; //click event holder
        public frmEmpFaculty()
        {
            InitializeComponent();
        }

        private void EnableControls(bool Enable)
        {
            txtCode.Enabled = txtName.Enabled = txtRemarks.Enabled = Enable;

            btnnew.Enabled = btnsave.Enabled = btnDelete.Enabled = btnedit.Enabled = true;
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnnew.Enabled = New;
            btnedit.Enabled = Edit;
            btnsave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
        }

        private void ChangeState(EntryMode Mode)
        {
            //On every changestate, clear the Opening Balance table

            e_mode = Mode;

            switch (e_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, false, false, false, true);
                    IsFieldChanged = false;

                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    IsFieldChanged = false;

                    break;
            }
        }
        private void frmEmpFaculty_Load(object sender, EventArgs e)
        {
            try
            {
                dt = BusinessLogic.Hrm.GetFaculty();
                drFound = dt.Select(filterString);

                m_Click = new SourceGrid.Cells.Controllers.CustomEvents();
                m_Click.Click += new EventHandler(grdListView_Click);
                fillgrid();
                ChangeState(EntryMode.NEW);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void grdListView_Click(object sender, EventArgs e)
        {
            try
            {
                //IsSelected = true;
                int currentRow = grdListView.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext cell = new SourceGrid.CellContext(grdListView, new SourceGrid.Position(currentRow, 0));
                //int id = Convert.ToInt32(cell.Value);
                int id = Convert.ToInt32(grdListView[currentRow, 0].Value.ToString());
                fillForm(id);
                EnableControls(false);
                ButtonState(true, true, false, true, true);
                //ChangeState(EntryMode.NORMAL);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void fillgrid()
        {
            try
            {
                grdListView.Rows.Clear();
                grdListView.Redim(drFound.Count() + 1, 3);
                writeHeader();
                for (int i = 1; i <= drFound.Count(); i++)
                {
                    DataRow dr = drFound[i - 1];
                    grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["FID"]);
                    grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["FacultyCode"]);
                    grdListView[i, 2] = new SourceGrid.Cells.Cell(dr["FacultyName"]);

                    grdListView[i, 1].AddController(m_Click);
                    grdListView[i, 2].AddController(m_Click);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void writeHeader()
        {
            grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
            grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Code");
            grdListView[0, 2] = new SourceGrid.Cells.ColumnHeader("Name");

            grdListView[0, 0].Column.Width = 40;
            grdListView[0, 1].Column.Width = 70;
            grdListView[0, 2].Column.Width = 100;
            grdListView[0, 0].Column.Visible = false;

        }
        private void fillForm(int ID)
        {

            try
            {
                DataTable dt = BusinessLogic.Hrm.GetFaculty(ID);
                DataRow dr = dt.Rows[0];
                txtFacultyID.Text = dr["FID"].ToString();
                txtCode.Text = dr["FacultyCode"].ToString();
                txtName.Text = dr["FacultyName"].ToString();
                txtRemarks.Text = dr["Remarks"].ToString();

                btnedit.Enabled = true;
                btnDelete.Enabled = true;
                btnnew.Enabled = false;

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void ClearForm()
        {
            txtFacultyID.Clear();
            txtCode.Clear();
            txtName.Clear();
            txtRemarks.Clear();
        }
        private void btnnew_Click(object sender, EventArgs e)
        {
            ClearForm();
            ChangeState(EntryMode.NEW);
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            if (txtFacultyID.Text == "")
            {
                Global.Msg("Please select an item from the list.");
                return;
            }
            ChangeState(EntryMode.EDIT);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFacultyID.Text == "")
                {
                    Global.Msg("Please select an item from the list.");
                    return;
                }
                int Id = 0;
                Id = Convert.ToInt32(txtFacultyID.Text);
                if (Global.MsgQuest("Are you sure you want to delete the loan?") == DialogResult.Yes)
                {
                    int i = BusinessLogic.Hrm.DeleteFaculty(Id);
                    MessageBox.Show("Record Deleted");
                    ClearForm();
                    ChangeState(EntryMode.NORMAL);
                    frmEmpFaculty_Load(sender, e);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            ChangeState(EntryMode.NORMAL);
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCode.Text == "")
                {
                    Global.Msg("Please enter a faculty code.");
                    txtCode.Select();
                    return;
                }

                if (txtName.Text == "")
                {
                    Global.Msg("Please enter a faculty name.");
                    txtName.Select();
                    return;
                }


                switch (e_mode)
                {
                    case EntryMode.NEW:
                        int i = BusinessLogic.Hrm.CreateFaculty(txtCode.Text.Trim(), txtName.Text, txtRemarks.Text);
                        if (i == -100)
                        {
                            Global.Msg("The code already exists.");
                            return;
                        }
                        else if (i > 0)
                        {
                            MessageBox.Show("Record saved");
                        }
                        break;
                    case EntryMode.EDIT:
                        int j = BusinessLogic.Hrm.UpdateFaculty(Convert.ToInt32(txtFacultyID.Text), txtCode.Text.Trim(), txtName.Text, txtRemarks.Text);
                        if (j == -100)
                        {
                            Global.Msg("The code already exists.");
                            return;
                        }
                        else if (j > 0)
                        {
                            MessageBox.Show("Record saved");
                        }
                        break;

                }


                ClearForm();
                frmEmpFaculty_Load(sender, e);
                ChangeState(EntryMode.NORMAL);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
    }
}
