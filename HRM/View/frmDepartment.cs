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
    public partial class frmDepartment : Form
    {
        DataTable dt;
        DataRow[] drFound;
        private string filterString = "";
        private EntryMode e_mode = EntryMode.NEW;
        private bool IsSelected = false;
        private bool IsFieldChanged;
        private bool IsNew;
        BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
        public frmDepartment()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void frmDepartment_Load(object sender, EventArgs e)
        {
            dt = BusinessLogic.Hrm.getdep();
            drFound = dt.Select(filterString);
            fillgrid();
            ChangeState(EntryMode.NEW);
            
        }
        private void fillgrid()
        {
            grdListview1.Rows.Clear();
            grdListview1.Redim(drFound.Count() + 1, 3);
            writeHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {
                DataRow dr = drFound[i - 1];
                grdListview1[i, 0] = new SourceGrid.Cells.Cell(dr["DepartmentID"]);
                grdListview1[i, 1] = new SourceGrid.Cells.Cell(dr["DepartmentCode"]);
                grdListview1[i, 2] = new SourceGrid.Cells.Cell(dr["DepartmentName"]);

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

        private void grdListview1_DoubleClick(object sender, EventArgs e)
        {


        }
        private void fillDepartmentGrp(int ID)
        {
            DataTable dt = BusinessLogic.Hrm.getDepartmentByID(ID);
            DataRow dr = dt.Rows[0];
            txtd_id.Text = dr["DepartmentID"].ToString();
            txtdepcode.Text = dr["DepartmentCode"].ToString();
            txtdname.Text = dr["DepartmentName"].ToString();
            btnProductEdit.Enabled = true;
            btnProductDelete.Enabled = true;
            btnProductNew.Enabled = false;


        }

        private void grdListview1_Click(object sender, EventArgs e)
        {
            IsSelected = true;
            int currentRow = grdListview1.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext cell = new SourceGrid.CellContext(grdListview1, new SourceGrid.Position(currentRow, 0));
            //int id = Convert.ToInt32(cell.Value);
            int id = Convert.ToInt32(grdListview1[currentRow, 0].Value.ToString());
            fillDepartmentGrp(id);
            ButtonState(true, true, false, true, true);
            
        }

        private void btnProductNew_Click(object sender, EventArgs e)
        {
            cleartextbox();
            txtdepcode.Enabled = true;
            txtdname.Enabled = true;
            ChangeState(EntryMode.NEW);

        }
        private void cleartextbox()
        {
            txtdname.Clear();
            txtdepcode.Clear();
        }

        private void btnProductSave_Click(object sender, EventArgs e)
        {
            if (txtdepcode.Text == "" && txtdname.Text == "")
                {

                  MessageBox.Show("all field reqired");
                }
            switch (e_mode)
            {
                case EntryMode.NEW:
                    DataTable dt = BusinessLogic.Hrm.insertIntoDepartment(txtdepcode.Text, txtdname.Text);
                    MessageBox.Show("Record saved");
                    break;
                case EntryMode.EDIT:
                    DataTable dt1 = BusinessLogic.Hrm.editdepartment(Convert.ToInt32(txtd_id.Text), txtdepcode.Text, txtdname.Text);
                   MessageBox.Show("Record saved");
                   break;

            }
            ChangeState(EntryMode.NORMAL);
            frmDepartment_Load(sender, e);
            txtdepcode.Text = "";
            txtdname.Text = "";
            //if (txtdepcode.Text == "" && txtdname.Text == "")
            //{

            //    MessageBox.Show("all field reqired");
            //}
            //else
            //{



            //    DataTable dt1 = BusinessLogic.Hrm.editdepartment(Convert.ToInt32(txtd_id.Text), txtdepcode.Text, txtdname.Text);
            //    MessageBox.Show("Record saved");




            //}

        }
        //private void getid(int id)
        //{
        //    switch (e_mode)
        //    {

        //        case EntryMode.NEW:
        //            IsNew = true;
        //            DataTable dt = BusinessLogic.Hrm.insertIntoDepartment(txtdepcode.Text, txtdname.Text);
        //            MessageBox.Show("Record saved");
        //            break;
        //        case EntryMode.EDIT:
        //            IsNew = false;
        //            DataTable dt1 = BusinessLogic.Hrm.editdepartment(id, txtdepcode.Text, txtdname.Text);
        //            MessageBox.Show("Record saved");
        //            break;
        //    }

        //}

        private void btnProductEdit_Click(object sender, EventArgs e)
        {
            txtdepcode.Enabled = true;
            txtdname.Enabled = true;
            ChangeState(EntryMode.EDIT);

        }

        private void btnProductCancel_Click(object sender, EventArgs e)
        {
            cleartextbox();
            txtdepcode.Enabled = false;
            txtdname.Enabled = false;
            ChangeState(EntryMode.NORMAL);
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
            txtdepcode.Enabled = txtdname.Enabled = Enable;

            btnProductNew.Enabled = btnProductSave.Enabled = btnProductDelete.Enabled = btnProductEdit.Enabled = true;
        }
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnProductNew.Enabled  = New;
            btnProductEdit.Enabled = Edit;
            btnProductSave.Enabled = Save;
            btnProductDelete.Enabled = Delete;
            btnProductCancel.Enabled = Cancel;
        }
        
        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int departId = Convert.ToInt32(txtd_id.Text);
                if (!employees.CheckDepartment(departId))
                {
                    Global.Msg("The department you are about to delete has records on Employee's records, so the department record can not be deleted.");
                    return;
                }
                DataTable dt = BusinessLogic.Hrm.deleteFromDepartment(departId);
                MessageBox.Show("Record Deleted");
                cleartextbox();
                ChangeState(EntryMode.NORMAL);
                frmDepartment_Load(sender, e);
            }
            catch(Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void grdListview1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
