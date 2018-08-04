using BusinessLogic;
using BusinessLogic.HOS;
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
    public partial class frmDoctorLevel : Form
    {
        public frmDoctorLevel()
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


        private void frmDoctorLevel_Load(object sender, EventArgs e)
        {
            dt = Doctor.GetDoctorLevel();
            drFound = dt.Select(filterString);

            m_Click = new SourceGrid.Cells.Controllers.CustomEvents();
            m_Click.Click += new EventHandler(grdListView_Click);
            fillgrid();
            ChangeState(EntryMode.NEW);
        }

        private void EnableControls(bool Enable)
        {
            txtLvlID.Enabled = txtLvlCode.Enabled = txtLvlName.Enabled = txtLvlBasicSal.Enabled = txtRemarks.Enabled = txtGradeNo.Enabled = txtGradeAmt.Enabled = Enable;

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
                DataTable dt = Doctor.GetDocLevelByID(ID);
                DataRow dr = dt.Rows[0];
                txtLvlID.Text = dr["LevelID"].ToString();
                txtLvlCode.Text = dr["LevelCode"].ToString();
                txtLvlName.Text = dr["LevelName"].ToString();
                decimal bSal = Convert.ToDecimal(dr["LevelBasicSalary"].ToString());
                txtLvlBasicSal.Text = bSal.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                txtRemarks.Text = dr["Remarks"].ToString();
                decimal gradeAmt = Convert.ToDecimal(dr["PerGradeAmt"].ToString());
                txtGradeAmt.Text = gradeAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                txtGradeNo.Text = dr["MaxGradeNo"].ToString();
                btnedit.Enabled = true;
                btnDelete.Enabled = true;
                btnnew.Enabled = false;

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }

        private void grdListView_Paint(object sender, PaintEventArgs e)
        {

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

                MessageBox.Show(ex.Message);
            }
        }

        private void fillgrid()
        {
            grdListView.Rows.Clear();
            grdListView.Redim(drFound.Count() + 1, 3);
            writeHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {
                DataRow dr = drFound[i - 1];
                grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["LevelID"]);
                grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["LevelCode"]);
                grdListView[i, 2] = new SourceGrid.Cells.Cell(dr["LevelName"]);

                grdListView[i, 1].AddController(m_Click);
                grdListView[i, 2].AddController(m_Click);
            }

        }
        private void ClearTextbox()
        {
            txtLvlID.Clear();
            txtLvlCode.Clear();
            txtLvlName.Clear();
            txtLvlBasicSal.Text = "0.00";
            txtGradeAmt.Text = "0.00";
            txtGradeNo.Text = "0";
            txtRemarks.Clear();
        }

        private void btnnew_Click(object sender, EventArgs e)
        {
            ClearTextbox();
            ChangeState(EntryMode.NEW);
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            if (txtLvlID.Text == "")
            {
                Global.Msg("Please select a level first");
                return;
            }

            ChangeState(EntryMode.EDIT);
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLvlCode.Text == "")
                {
                    Global.Msg("Please enter a code.");
                    txtLvlCode.Select();
                    return;
                }

                if (txtLvlName.Text == "")
                {
                    Global.Msg("Please enter a Level name.");
                    txtLvlName.Select();
                    return;
                }

                if (UserValidation.validDecimal(txtLvlBasicSal.Text))
                {
                    switch (e_mode)
                    {
                        case EntryMode.NEW:
                            DataTable dt = Doctor.CreateLevel(txtLvlCode.Text.Trim(), txtLvlName.Text.Trim(), txtLvlBasicSal.Text == "" ? 0 : Convert.ToDecimal(txtLvlBasicSal.Text), txtGradeNo.Text == "" ? 0 : Convert.ToInt32(txtGradeNo.Text), txtGradeAmt.Text == "" ? 0 : Convert.ToDecimal(txtGradeAmt.Text), txtRemarks.Text);
                            MessageBox.Show("Record saved");
                            break;
                        case EntryMode.EDIT:
                            DataTable dt1 = Doctor.UpdateLevel(Convert.ToInt32(txtLvlID.Text), txtLvlCode.Text.Trim(), txtLvlName.Text.Trim(), txtLvlBasicSal.Text == "" ? 0 : Convert.ToDecimal(txtLvlBasicSal.Text), txtGradeNo.Text == "" ? 0 : Convert.ToInt32(txtGradeNo.Text), txtGradeAmt.Text == "" ? 0 : Convert.ToDecimal(txtGradeAmt.Text), txtRemarks.Text);
                            MessageBox.Show("Record saved");
                            break;

                    }
                }
                else
                {
                    Global.Msg("The salary is in wrong format.");
                    txtLvlBasicSal.Select();
                    return;
                }

                ClearTextbox();
                frmDoctorLevel_Load(sender, e);
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
                int lvlId = 0;
                if (txtLvlID.Text == "")
                {
                    Global.Msg("Please select a level first.");
                    return;
                }
                lvlId = Convert.ToInt32(txtLvlID.Text);

                if (Global.MsgQuest("Are you sure you want to delete the level?") == DialogResult.Yes)
                {
                    int i = Doctor.DeleteLevel(lvlId);
                    MessageBox.Show("Record Deleted");
                    ClearTextbox();
                    ChangeState(EntryMode.NORMAL);
                    frmDoctorLevel_Load(sender, e);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearTextbox();
            ChangeState(EntryMode.NORMAL);

        }
    }
}
