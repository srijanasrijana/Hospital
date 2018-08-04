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
    public partial class frmLoan : Form
    {
        public frmLoan()
        {
            InitializeComponent();
        }

        public enum EntryMode { NORMAL, NEW, EDIT }; //Used in entry forms. To find which mode(new, edit or normal) is the form currently running

        DataTable dt;
        DataRow[] drFound;
        private string filterString = "";
        private EntryMode e_mode = EntryMode.NEW;
        private bool IsFieldChanged;

        private SourceGrid.Cells.Controllers.CustomEvents m_Click; //click event holder


        private void EnableControls(bool Enable)
        {
            txtLoanID.Enabled = txtLoanName.Enabled = txtRemarks.Enabled = cmbLoanType.Enabled = Enable;

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

        private void writeHeader()
        {
            grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
            grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Name");
            grdListView[0, 2] = new SourceGrid.Cells.ColumnHeader("Type");

            grdListView[0, 0].Column.Width = 40;
            grdListView[0, 1].Column.Width = 70;
            grdListView[0, 2].Column.Width = 100;
            grdListView[0, 0].Column.Visible = false;

        }

        private void fillForm(int ID)
        {

            try
            {
                DataTable dt = BusinessLogic.Hrm.GetLoanByID(ID);
                DataRow dr = dt.Rows[0];
                txtLoanID.Text = dr["LoanID"].ToString();
                txtLoanName.Text = dr["LoanName"].ToString();
                cmbLoanType.Text = dr["LoanType"].ToString();
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
        private void frmLoan_Load(object sender, EventArgs e)
        {
            try
            {
                dt = BusinessLogic.Hrm.GetLoan();
                drFound = dt.Select(filterString);

                m_Click = new SourceGrid.Cells.Controllers.CustomEvents();
                m_Click.Click += new EventHandler(grdListView_Click);
                fillgrid();
                cmbLoanType.SelectedIndex = 0;
                ChangeState(EntryMode.NEW);
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
                    grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["LoanID"]);
                    grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["LoanName"]);
                    grdListView[i, 2] = new SourceGrid.Cells.Cell(dr["LoanType"]);

                    grdListView[i, 1].AddController(m_Click);
                    grdListView[i, 2].AddController(m_Click);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void ClearForm()
        {
            txtLoanID.Clear();
            txtLoanName.Clear();
            cmbLoanType.SelectedIndex = 0;
            txtRemarks.Clear();
        }

        private void btnnew_Click(object sender, EventArgs e)
        {
            ClearForm();
            ChangeState(EntryMode.NEW);
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            if(txtLoanID.Text == "")
            {
                Global.Msg("Please select an item from the list.");
                return;
            }
            ChangeState(EntryMode.EDIT);
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLoanName.Text == "")
                {
                    Global.Msg("Please enter a loan name.");
                    txtLoanName.Select();
                    return;
                }

                if (cmbLoanType.SelectedIndex == -1)
                {
                    Global.Msg("Please select a loan type.");
                    cmbLoanType.DroppedDown = true;
                    return;
                }

                
                    switch (e_mode)
                    {
                        case EntryMode.NEW:
                            int i = BusinessLogic.Hrm.CreateLoan(txtLoanName.Text.Trim(), cmbLoanType.Text, txtRemarks.Text);
                            if(i == -100)
                            {
                                Global.Msg("The Loan name already exists.");
                                return;
                            }
                            else if(i > 0)
                            {
                                MessageBox.Show("Record saved");
                            }
                            break;
                        case EntryMode.EDIT:
                            int j = BusinessLogic.Hrm.UpdateLoan(Convert.ToInt32(txtLoanID.Text), txtLoanName.Text.Trim(), cmbLoanType.Text, txtRemarks.Text);
                            if(j == -100)
                            {
                                Global.Msg("The Loan name already exists.");
                                return;
                            }
                            else if(j > 0)
                            {
                                MessageBox.Show("Record saved");
                            }
                            break;

                    }
               

                ClearForm();
                frmLoan_Load(sender,e);
                ChangeState(EntryMode.NORMAL);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLoanID.Text == "")
                {
                    Global.Msg("Please select an item from the list.");
                    return;
                }
                int loanId = 0;
                loanId = Convert.ToInt32(txtLoanID.Text);
                if (Global.MsgQuest("Are you sure you want to delete the loan?") == DialogResult.Yes)
                {
                    int i = BusinessLogic.Hrm.DeleteLoan(loanId);
                    MessageBox.Show("Record Deleted");
                    ClearForm();
                    ChangeState(EntryMode.NORMAL);
                    frmLoan_Load(sender, e);
                }
            }
            catch(Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            ChangeState(EntryMode.NORMAL);
        }
    }
}
