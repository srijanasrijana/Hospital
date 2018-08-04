using BusinessLogic;
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
    public partial class frmHosSpecialization : Form
    {
        public frmHosSpecialization()
        {
            InitializeComponent();
        }

        private EntryMode e_mode = EntryMode.NEW;
        private bool IsFieldChanged;
        private bool IsSelected = false;
        DataRow[] drFound;
        DataTable dt;
        private string filterString = "";
        BusinessLogic.HOS.Doctor doctor = new BusinessLogic.HOS.Doctor();

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
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnSpecilizationNew.Enabled = New;
            btnSpecilizationEdit.Enabled = Edit;
            btnSpecilizationSave.Enabled = Save;
            btnSpecilizationDelete.Enabled = Delete;
            btnSpecilizationCancel.Enabled = Cancel;
        }
        private void EnableControls(bool Enable)
        {
            txtSpecilizationcode.Enabled = txtSpecilizationname.Enabled = Enable;

            btnSpecilizationNew.Enabled = btnSpecilizationSave.Enabled = btnSpecilizationDelete.Enabled = btnSpecilizationEdit.Enabled = true;
        }
        private void cleartextbox()
        {
            txtSpecilizationname.Clear();
            txtSpecilizationcode.Clear();
        }

        private void frmHosSpecialization_Load(object sender, EventArgs e)
        {
            dt = BusinessLogic.HOS.Hos.getSpecilization();
            drFound = dt.Select(filterString);
            fillgrid();
            ChangeState(EntryMode.NEW);

        }

        private void btnSpecilizationNew_Click(object sender, EventArgs e)
        {
            cleartextbox();
            txtSpecilizationcode.Enabled = true;
            txtSpecilizationname.Enabled = true;
            ChangeState(EntryMode.NEW);
        }

        private void fillSpelizationGrp(int ID)
        {
            DataTable dt = BusinessLogic.HOS.Hos.gettblSpecilizationByID(ID);
            DataRow dr = dt.Rows[0];
            txtSpecilization_id.Text = dr["SpecilizationID"].ToString();
            txtSpecilizationcode.Text = dr["SpecilizationCode"].ToString();
            txtSpecilizationname.Text = dr["SpecilizationName"].ToString();
            btnSpecilizationEdit.Enabled = true;
            btnSpecilizationDelete.Enabled = true;
            btnSpecilizationNew.Enabled = false;


        }

        private void fillgrid()
        {
            grdListview1.Rows.Clear();
            grdListview1.Redim(drFound.Count() + 1, 3);
            writeHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {
                DataRow dr = drFound[i - 1];
                grdListview1[i, 0] = new SourceGrid.Cells.Cell(dr["SpecilizationID"]);
                grdListview1[i, 1] = new SourceGrid.Cells.Cell(dr["SpecilizationCode"]);
                grdListview1[i, 2] = new SourceGrid.Cells.Cell(dr["SpecilizationName"]);

            }

        }
        private void writeHeader()
        {
            grdListview1[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
            grdListview1[0, 1] = new SourceGrid.Cells.ColumnHeader("Code");
            grdListview1[0, 2] = new SourceGrid.Cells.ColumnHeader("Department Name");

            grdListview1[0, 0].Column.Width = 40;
            grdListview1[0, 1].Column.Width = 70;
            grdListview1[0, 2].Column.Width = 100;
            grdListview1[0, 0].Column.Visible = false;

        }


        private void grdListView1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grdListView1_Click(object sender, EventArgs e)
        {
            IsSelected = true;
            int currentRow = grdListview1.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext cell = new SourceGrid.CellContext(grdListview1, new SourceGrid.Position(currentRow, 0));
            //int id = Convert.ToInt32(cell.Value);
            int id = Convert.ToInt32(grdListview1[currentRow, 0].Value.ToString());
            fillSpelizationGrp(id);
            ButtonState(true, true, false, true, true);

        }

        private void btnSpecilizationSave_Click_1(object sender, EventArgs e)
        {
            if (txtSpecilizationcode.Text == "" && txtSpecilizationname.Text == "")
            {

                MessageBox.Show("all field reqired");
            }
            switch (e_mode)
            {
                case EntryMode.NEW:
                    DataTable dt = BusinessLogic.HOS.Hos.insertIntoSpecilization(txtSpecilizationcode.Text, txtSpecilizationname.Text);
                    MessageBox.Show("Record saved");
                    break;
                case EntryMode.EDIT:
                    DataTable dt1 = BusinessLogic.HOS.Hos.editSpecilization(Convert.ToInt32(txtSpecilization_id.Text), txtSpecilizationcode.Text, txtSpecilizationname.Text);
                    MessageBox.Show("Record saved");
                    break;

            }
            ChangeState(EntryMode.NORMAL);
            frmHosSpecialization_Load(sender, e);
            txtSpecilizationcode.Text = "";
            txtSpecilizationname.Text = "";
        }

        private void btnSpecilizationEdit_Click(object sender, EventArgs e)
        {

            txtSpecilizationcode.Enabled = true;
            txtSpecilizationname.Enabled = true;
            ChangeState(EntryMode.EDIT);
        }

        private void btnSpecilizationDelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                int departId = Convert.ToInt32(txtSpecilization_id.Text);
                //if (!doctor.CheckDepartment(departId))
                //{
                //    Global.Msg("The department you are about to delete has records on Employee's records, so the department record can not be deleted.");
                //    return;
                //}
                DataTable dt = BusinessLogic.HOS.Hos.deleteFromSpecilization(departId);
                MessageBox.Show("Record Deleted");
                cleartextbox();
                ChangeState(EntryMode.NORMAL);
                frmHosSpecialization_Load(sender, e);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnSpecilizationCancel_Click_1(object sender, EventArgs e)
        {

           cleartextbox();
            txtSpecilizationcode.Enabled = false;
            txtSpecilizationname.Enabled = false;
            ChangeState(EntryMode.NORMAL);
        
        }

       

    }
}
