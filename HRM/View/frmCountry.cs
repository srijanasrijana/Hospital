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
    public partial class frmCountry : Form
    {
        DataTable dt;
        DataRow[] drFound;
        private string filterString = "";
        private EntryMode e_mode = EntryMode.NEW;
        private bool IsSelected = false;
        private bool IsFieldChanged;
        private bool IsNew;
        public frmCountry()
        {
            InitializeComponent();
        }

        private void grdListViewDesignation_Click(object sender, EventArgs e)
        {

        }

        private void frmCountry_Load(object sender, EventArgs e)
        {
            dt = BusinessLogic.Nationality.getNationality();
            drFound = dt.Select(filterString);
            fillgrid();
            ChangeState(EntryMode.NEW);
        }

        private void fillgrid()
        {
            grdListViewNationality.Rows.Clear();
            grdListViewNationality.Redim(drFound.Count() + 1, 2);
            writeHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {
                DataRow dr = drFound[i - 1];
                grdListViewNationality[i, 0] = new SourceGrid.Cells.Cell(dr["NationalityId"]);
                grdListViewNationality[i, 1] = new SourceGrid.Cells.Cell(dr["NationalityName"]);
               

            }

        }

        private void writeHeader()
        {
            grdListViewNationality[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
            grdListViewNationality[0, 1] = new SourceGrid.Cells.ColumnHeader("Nationality");


            grdListViewNationality[0, 0].Column.Width = 40;
            grdListViewNationality[0, 1].Column.Width = 70;

            grdListViewNationality[0, 0].Column.Visible = false;

        }


        private void fillCountryForm(int ID)
        {
            DataTable dt = BusinessLogic.Nationality.getNationalityByID(ID);
            DataRow dr = dt.Rows[0];
            txtNationalityID.Text = dr["NationalityId"].ToString();

            txtNationalityName.Text = dr["NationalityName"].ToString();
            btnedit.Enabled = true;
            btnDelete.Enabled = true;
            btnnew.Enabled = false;


        }

        private void grdListViewNationality_Click(object sender, EventArgs e)
        {
            IsSelected = true;
            int currentRow = grdListViewNationality.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext cell = new SourceGrid.CellContext(grdListViewNationality, new SourceGrid.Position(currentRow, 0));
            int id = Convert.ToInt32(cell.Value);
            fillCountryForm(id);
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
                    ButtonState(true, true, true, true, true);
                    IsFieldChanged = false;

                    break;
            }
        }

        private void EnableControls(bool Enable)
        {
            txtNationalityName.Enabled = Enable;

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

        private void cleartextbox()
        {
            txtNationalityName.Clear();
           
        }

        private void btnnew_Click(object sender, EventArgs e)
        {
            cleartextbox();
            ChangeState(EntryMode.NEW);
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (txtNationalityName.Text == "")
            {

                MessageBox.Show("all field reqired");
            }
            switch (e_mode)
            {
                case EntryMode.NEW:
                    int i = BusinessLogic.Nationality.insertIntoNationality(txtNationalityName.Text);
                    MessageBox.Show("Record saved");
                    cleartextbox();
                    break;
                case EntryMode.EDIT:
                    int j = BusinessLogic.Nationality.updateNationality(Convert.ToInt32(txtNationalityID.Text), txtNationalityName.Text);
                    MessageBox.Show("Record saved");
                    cleartextbox();
                    break;

            }
            frmCountry_Load(sender, e);
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.EDIT);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = BusinessLogic.Nationality.deletefromNationality(Convert.ToInt32(txtNationalityID.Text));
            MessageBox.Show("record Deleted");
            cleartextbox();
            frmCountry_Load(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cleartextbox();
            ChangeState(EntryMode.NORMAL);
        }
    }
}
