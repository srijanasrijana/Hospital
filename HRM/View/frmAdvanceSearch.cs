using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DateManager;
using BusinessLogic;

namespace HRM
{
    public partial class frmAdvanceSearch : Form
    {
        private string FilterString = "";
        private DataRow[] drFound;
        private DataTable dTable;
        public frmAdvanceSearch()
        {
            InitializeComponent();
        }

        private void frmAdvanceSearch_Load(object sender, EventArgs e)
        {
            BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
            dTable = employees.searchEmployeeDetails();
            drFound = dTable.Select(FilterString);
            fillGrid();
        }

        private void fillGrid()
        {
            grdAdvanceSearch.Rows.Clear();
            grdAdvanceSearch.Redim(drFound.Count() + 1, 16);
            writeHeader();
            SourceGrid.Cells.Views.Cell HeaderView = new SourceGrid.Cells.Views.Cell();
            for (int i = 1; i <=drFound.Count(); i++)
            {
                
                if (i % 2 == 0)
                {
                    HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
                }
                else
                {
                    HeaderView.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Global.Grid_Color);
                }
                DataRow dr = drFound[i - 1];
                cmbstatus.SelectedIndex = Convert.ToInt32(dr["Status"]);
                string sts = cmbstatus.SelectedItem.ToString();
                cmbtype.SelectedIndex = Convert.ToInt32(dr["type"]);
                string typ = cmbtype.SelectedItem.ToString();
                grdAdvanceSearch[i, 0] = new SourceGrid.Cells.Cell(dr["ID"].ToString());
                grdAdvanceSearch[i, 1] = new SourceGrid.Cells.Cell(dr["FirstName"].ToString() +" "+ dr["LastName"].ToString());
                grdAdvanceSearch[i, 1].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 2] = new SourceGrid.Cells.Cell(dr["LastName"].ToString());
                grdAdvanceSearch[i, 2].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 3] = new SourceGrid.Cells.Cell(dr["StaffCode"].ToString());
                grdAdvanceSearch[i, 3].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 4] = new SourceGrid.Cells.Cell(dr["Address1"].ToString());
                grdAdvanceSearch[i, 4].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 5] = new SourceGrid.Cells.Cell(dr["Address2"].ToString());
                grdAdvanceSearch[i, 5].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 6] = new SourceGrid.Cells.Cell(Date.ToSystem( Convert.ToDateTime( dr["BirthDate"].ToString())));
                grdAdvanceSearch[i, 6].View = new SourceGrid.Cells.Views.Cell(HeaderView);

                if (dr["IsMale"].ToString() == "True")
                {
                    grdAdvanceSearch[i, 7] = new SourceGrid.Cells.Cell("Male");
                    grdAdvanceSearch[i, 7].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                }
                else if (dr["IsMale"].ToString() == "False")
                {
                    grdAdvanceSearch[i, 7] = new SourceGrid.Cells.Cell("Female");
                    grdAdvanceSearch[i, 7].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                }
                grdAdvanceSearch[i, 8] = new SourceGrid.Cells.Cell(dr["City"].ToString());
                grdAdvanceSearch[i, 8].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 9] = new SourceGrid.Cells.Cell(dr["NationalityName"].ToString());
                grdAdvanceSearch[i, 9].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 10] = new SourceGrid.Cells.Cell(Date.ToSystem( Convert.ToDateTime( dr["JoinDate"].ToString())));
                grdAdvanceSearch[i, 10].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 11] = new SourceGrid.Cells.Cell(Date.ToSystem( Convert.ToDateTime(dr["RetireMentDate"].ToString())));
                grdAdvanceSearch[i, 11].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 12] = new SourceGrid.Cells.Cell(dr["DepartmentName"].ToString());
                grdAdvanceSearch[i, 12].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 13] = new SourceGrid.Cells.Cell(dr["DesignationName"].ToString());
                grdAdvanceSearch[i, 13].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 14] = new SourceGrid.Cells.Cell(sts);
                grdAdvanceSearch[i, 14].View = new SourceGrid.Cells.Views.Cell(HeaderView);
                grdAdvanceSearch[i, 15] = new SourceGrid.Cells.Cell(typ);
                grdAdvanceSearch[i, 15].View = new SourceGrid.Cells.Views.Cell(HeaderView);
            }
        }
        private void writeHeader()
        {
            grdAdvanceSearch[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
            grdAdvanceSearch[0, 1] = new SourceGrid.Cells.ColumnHeader("Staff Name");
            grdAdvanceSearch[0, 2] = new SourceGrid.Cells.ColumnHeader("Last Name");
            grdAdvanceSearch[0, 3] = new SourceGrid.Cells.ColumnHeader("StaffCode");
            grdAdvanceSearch[0, 4] = new SourceGrid.Cells.ColumnHeader("Address1");
            grdAdvanceSearch[0, 5] = new SourceGrid.Cells.ColumnHeader("Address2");
            grdAdvanceSearch[0, 6] = new SourceGrid.Cells.ColumnHeader("BirthDate");
            grdAdvanceSearch[0, 7] = new SourceGrid.Cells.ColumnHeader("Gender");
            grdAdvanceSearch[0, 8] = new SourceGrid.Cells.ColumnHeader("City");
            grdAdvanceSearch[0, 9] = new SourceGrid.Cells.ColumnHeader("Nationality");
            grdAdvanceSearch[0, 10] = new SourceGrid.Cells.ColumnHeader("JoinDate");
            grdAdvanceSearch[0, 11] = new SourceGrid.Cells.ColumnHeader("End Date");
            grdAdvanceSearch[0, 12] = new SourceGrid.Cells.ColumnHeader("Department");
            grdAdvanceSearch[0, 13] = new SourceGrid.Cells.ColumnHeader("Designation");
            grdAdvanceSearch[0, 14] = new SourceGrid.Cells.ColumnHeader("Status");
            grdAdvanceSearch[0, 15] = new SourceGrid.Cells.ColumnHeader("Type");




            grdAdvanceSearch[0, 0].Column.Width = 30;
            grdAdvanceSearch[0, 1].Column.Width = 100;
            grdAdvanceSearch[0, 2].Column.Width = 90;
            grdAdvanceSearch[0, 3].Column.Width = 70;
            grdAdvanceSearch[0, 4].Column.Width = 100;
            grdAdvanceSearch[0, 5].Column.Width = 100;
            grdAdvanceSearch[0, 6].Column.Width = 100;
            grdAdvanceSearch[0, 7].Column.Width = 55;
            grdAdvanceSearch[0, 8].Column.Width = 100;
            grdAdvanceSearch[0, 9].Column.Width = 70;
            grdAdvanceSearch[0, 10].Column.Width = 80;
            grdAdvanceSearch[0, 11].Column.Width = 80;
            grdAdvanceSearch[0, 12].Column.Width = 80;
            grdAdvanceSearch[0, 13].Column.Width = 80;
            grdAdvanceSearch[0, 14].Column.Width = 80;
            grdAdvanceSearch[0, 15].Column.Width = 80;

            grdAdvanceSearch[0, 0].Column.Visible = false;
            grdAdvanceSearch[0, 2].Column.Visible = false;
           // grdAdvanceSearch[0, 3].Column.Visible = false;
            grdAdvanceSearch[0, 4].Column.Visible = false;
            grdAdvanceSearch[0, 5].Column.Visible = false;

        }
        //private void Filter()
        //{
        //    if (cboSrchSearchIn1.SelectedItem.ToString() == "Staff Name")
        //    {
        //        this.FilterString = "FirstName Like '" + txtSrchParam1 + "%' ";
        //        try
        //        {
        //            drFound = dTable.Select(this.FilterString);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //        fillGrid();
        //    }
        //}

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            //Filter();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cboSrchSearchIn1.SelectedIndex ==-1 || txtSrchParam1.Text=="")
            {
                MessageBox.Show("Please select the Search Category","Search",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            else if (cboSrchSearchIn1.Text == "Staff Name")
            {

                this.FilterString = "Firstname like '" + txtSrchParam1.Text + "%'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (cboSrchSearchIn1.Text == "Staff Code")
            {

                this.FilterString = "StaffCode like '" + txtSrchParam1.Text + "%'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (cboSrchSearchIn1.Text == "Gender")
            {
                bool id=true;
                if (txtSrchParam1.Text == "Male" || txtSrchParam1.Text == "male")
                {
                     id = true;
                }
                else if (txtSrchParam1.Text == "Female" || txtSrchParam1.Text == "female")
                {
                     id = false;
                }
                this.FilterString = "IsMale = '"+id+"'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (cboSrchSearchIn1.Text == "City")
            {

                this.FilterString = "City like '" + txtSrchParam1.Text + "%'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (cboSrchSearchIn1.Text == "Join Date")
            {

                this.FilterString = "JoinDate = '" +Date.ToDotNet(txtSrchParam1.Text )+ "'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }

            else if (cboSrchSearchIn1.Text == "End Date")
            {

                this.FilterString = "RetirementDate = '" + Date.ToDotNet(txtSrchParam1.Text) + "'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (cboSrchSearchIn1.Text == "Department")
            {

                this.FilterString = "DepartmentName like '" + txtSrchParam1.Text + "%'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (cboSrchSearchIn1.Text == "Designation")
            {

                this.FilterString = "DesignationName like '" + txtSrchParam1.Text + "%'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (cboSrchSearchIn1.Text == "Status")
            {
                int id=-1;
                if (txtSrchParam1.Text == "Continue" || txtSrchParam1.Text == "continue")
                {
                    id = 0;
                }
                else if (txtSrchParam1.Text == "Leave" || txtSrchParam1.Text == "leave")
                {
                    id = 1; 
                }
                else if (txtSrchParam1.Text == "Break" || txtSrchParam1.Text == "break")
                {
                    id = 2; 
                }
                else if (txtSrchParam1.Text == "Retired" || txtSrchParam1.Text == "retired")
                {
                    id = 3; 
                }
                this.FilterString = "Status = '" + id + "'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }
            else if (cboSrchSearchIn1.Text == "Type")
            {

                int id=-1;
                if (txtSrchParam1.Text == "Contract" || txtSrchParam1.Text == "contract")
                {
                    id = 0;
                }
                else if (txtSrchParam1.Text == "Probation" || txtSrchParam1.Text == "probation")
                {
                    id = 1; 
                }
                else if (txtSrchParam1.Text == "Permanent" || txtSrchParam1.Text == "permanent")
                {
                    id = 2; 
                }
                this.FilterString = "Type = '" + id + "'";
                try
                {
                    drFound = dTable.Select(this.FilterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                fillGrid();
            }

           
        }

        private void btnForgetSearch_Click(object sender, EventArgs e)
        {
            cboSrchSearchIn1.SelectedIndex = -1;
            txtSrchParam1.Text = "";
            this.FilterString = "";
            try
            {
                drFound = dTable.Select(this.FilterString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            fillGrid();

        }
    }
}
