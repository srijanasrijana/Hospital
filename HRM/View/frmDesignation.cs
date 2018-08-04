using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BusinessLogic;

namespace HRM
{
    public partial class frmDesignation : Form
    {
        DataTable dt;
        DataRow[] drFound;
        private string filterString = "";
        private EntryMode e_mode = EntryMode.NEW;
        private bool IsSelected = false;
        private bool IsFieldChanged;
        private bool IsNew;
        BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
        public frmDesignation()
        {
            InitializeComponent();
        }

        private void frmDesignation_Load(object sender, EventArgs e)
        {
            dt = BusinessLogic.Hrm.getDesignation();
            drFound = dt.Select(filterString);
            fillgrid();
            ChangeState(EntryMode.NEW);
        }

        private void fillgrid()
        {
            grdListViewDesignation.Rows.Clear();
            grdListViewDesignation.Redim(drFound.Count() + 1, 3);
            writeHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {
                DataRow dr = drFound[i - 1];
                grdListViewDesignation[i, 0] = new SourceGrid.Cells.Cell(dr["DesignationID"]);
                grdListViewDesignation[i, 1] = new SourceGrid.Cells.Cell(dr["DesignationCode"]);
                grdListViewDesignation[i, 2] = new SourceGrid.Cells.Cell(dr["DesignationName"]);

            }

        }
        private void writeHeader()
        {
            grdListViewDesignation[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
            grdListViewDesignation[0, 1] = new SourceGrid.Cells.ColumnHeader("Designation Code");
            grdListViewDesignation[0, 2] = new SourceGrid.Cells.ColumnHeader("Designation Name");

            grdListViewDesignation[0, 0].Column.Width = 40;
            grdListViewDesignation[0, 1].Column.Width = 70;
            grdListViewDesignation[0, 2].Column.Width = 100;
            grdListViewDesignation[0, 0].Column.Visible = false;

        }

        private void fillDesignationForm(int ID)
        {
            DataTable dt = BusinessLogic.Hrm.getDesignationByID(ID);
            DataRow dr = dt.Rows[0];
            txtdesigid.Text =  dr["DesignationID"].ToString();
            txtdesigcode.Text = dr["DesignationCode"].ToString();
            txtdesigname.Text = dr["DesignationName"].ToString();
            btnedit.Enabled = true;
            btnDelete.Enabled = true;
            btnnew.Enabled = false;


        }

        private void grdListViewDesignation_Click(object sender, EventArgs e)
        {
            IsSelected = true;
            int currentRow = grdListViewDesignation.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext cell = new SourceGrid.CellContext(grdListViewDesignation, new SourceGrid.Position(currentRow, 0));
            //int id = Convert.ToInt32(cell.Value);
            int id = Convert.ToInt32(grdListViewDesignation[currentRow, 0].Value.ToString());
            fillDesignationForm(id);
            ButtonState(true, true, false, true, true);
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

        private void EnableControls(bool Enable)
        {
           txtdesigcode.Enabled=txtdesigname.Enabled = Enable;

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

        private void btnnew_Click(object sender, EventArgs e)
        {
            cleartextbox();
            ChangeState(EntryMode.NEW);
        }


        private void cleartextbox()
        {
            txtdesigname.Clear();
            txtdesigcode.Clear();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (txtdesigcode.Text == "" && txtdesigname.Text == "")
            {

                MessageBox.Show("all field reqired");
            }
            switch (e_mode)
            {
                case EntryMode.NEW:
                    DataTable dt = BusinessLogic.Hrm.insertIntoDesignation(txtdesigcode.Text, txtdesigname.Text);
                    MessageBox.Show("Record saved");
                    break;
                case EntryMode.EDIT:
                    DataTable dt1 = BusinessLogic.Hrm.updateDesignation(Convert.ToInt32(txtdesigid.Text), txtdesigcode.Text, txtdesigname.Text);
                    MessageBox.Show("Record saved");
                    break;

            }
            frmDesignation_Load(sender, e);
            txtdesigcode.Text = "";
            txtdesigname.Text = "";
            ChangeState(EntryMode.NORMAL);
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.EDIT);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int desigId = 0;
                desigId = Convert.ToInt32(txtdesigid.Text);
                if (!employees.CheckDesignation(desigId))
                {
                    Global.Msg("The designation you are about to delete has records on Employee's records, so the designation record can not be deleted.");
                    return;
                }
                DataTable dt = BusinessLogic.Hrm.deleteFromDesignation(desigId);
                MessageBox.Show("Record Deleted");
                cleartextbox();
                ChangeState(EntryMode.NORMAL);
                frmDesignation_Load(sender, e);
            }
            catch(Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cleartextbox();
            ChangeState(EntryMode.NORMAL);
        }
    }
}
