using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic.HRM.Report;

namespace HRM.View.Reports
{
    public partial class frmDetailEmployeeReportSettings : Form
    {
        public enum CmbType
        {
            Department, Designation, Faculty, Level, Status, JobType
        }
        
        /// <summary>
        /// Loads Combobox according to CmbType
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="type"></param>
        private void LoadComboBox(ComboBox cbo, CmbType type)
        {
            try
            {
                cbo.Items.Clear();
                DataTable dt = new DataTable();
                string valueMember = "ID";
                string displayMember = "Value";

                switch (type)
                {
                    case CmbType.Department:
                        dt = Hrm.getdep();
                        valueMember = "DepartmentID";
                        displayMember = "DepartmentName";
                        break;
                    case CmbType.Designation:
                        dt = Hrm.getDesignation();
                        valueMember = "DesignationID";
                        displayMember = "DesignationName";
                        break;
                    case CmbType.Faculty:
                        dt = Hrm.GetEmpFacultyForCmb();
                        break;
                    case CmbType.Level:
                        dt = Hrm.GetEmpLevelForCmb();
                        break;
                }
                if (dt.Rows.Count > 0)
                {
                    //DataRow dr = dt.NewRow();
                    //dr[valueMember] = 0;
                    //dr[displayMember] = "All";
                    //dt.Rows.InsertAt(dr, 0);

                    cbo.DataSource = dt;
                    cbo.DisplayMember = displayMember;
                    cbo.ValueMember = valueMember;

                    cbo.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        public frmDetailEmployeeReportSettings()
        {
            InitializeComponent();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ShowReport();
        }

        private void ShowReport()
        {
            try
            {
                DetailEmployeeReportSettings report = new DetailEmployeeReportSettings();
                if (chkDepartment.Checked && cboDepartment.SelectedIndex > -1)
                {
                    report.Department = cboDepartment.Text;
                    report.DepartmentID = Convert.ToInt32(cboDepartment.SelectedValue);
                    //report.IsDepartment = false;
                }

                if (chkDesignation.Checked && cboDesignation.SelectedIndex > -1)
                {
                    report.Designation = cboDesignation.Text;
                    report.DesignationID = Convert.ToInt32(cboDesignation.SelectedValue);
                    //report.IsDesignation = false;
                }

                if (chkFaculty.Checked && cboFaculty.SelectedIndex > -1)
                {
                    report.Faculty = cboFaculty.Text;
                    report.FacultyID = Convert.ToInt32(cboFaculty.SelectedValue);
                    //report.IsFaculty = false;
                }
                if (chkJobType.Checked && cboJobType.SelectedIndex > -1)
                {
                    report.JobType = cboJobType.Text;
                    report.JobTypeID = cboJobType.SelectedIndex;
                    //report.IsJobType = false;
                }
                if (chkLevel.Checked && cboLevel.SelectedIndex > -1)
                {
                    report.Level = cboLevel.Text;
                    report.LevelID = Convert.ToInt32(cboLevel.SelectedValue);
                    //report.IsLevel = false;
                }
                if (chkStatus.Checked && cboStatus.SelectedIndex > -1)
                {
                    report.Status = cboStatus.Text;
                    report.StatusID = cboStatus.SelectedIndex;
                    //report.IsStatus = false;
                }


                // check if any instance of frmDetailEmployeeReport is running or open, if so close the open form and reopen the form with currently chosen settings
                Form fc = Application.OpenForms["frmDetailEmployeeReport"];

                if (fc != null)
                    fc.Close();
                if (chkIsPatreon.Checked)
                {
                    report.RptType = "Patreon";
                }
                frmDetailEmployeeReport frmReport = new frmDetailEmployeeReport(report);
                frmReport.Show();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        public void LoadForm()
        {
            try
            {
                cboStatus.SelectedIndex = 0;
                cboJobType.SelectedIndex = 0;
                LoadComboBox(cboDepartment, CmbType.Department);
                LoadComboBox(cboDesignation, CmbType.Designation);
                LoadComboBox(cboFaculty, CmbType.Faculty);
                LoadComboBox(cboLevel, CmbType.Level);

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void chkDepartment_CheckedChanged(object sender, EventArgs e)
        {
            //if(chkDepartment.Checked)
                cboDepartment.Enabled = chkDepartment.Checked;
        }

        private void chkFaculty_CheckedChanged(object sender, EventArgs e)
        {
            cboFaculty.Enabled = chkFaculty.Checked;
        }

        private void chkDesignation_CheckedChanged(object sender, EventArgs e)
        {
            cboDesignation.Enabled = chkDesignation.Checked;
        }

        private void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            cboStatus.Enabled = chkStatus.Checked;
        }

        private void chkLevel_CheckedChanged(object sender, EventArgs e)
        {
            cboLevel.Enabled = chkLevel.Checked;
        }

        private void chkJobType_CheckedChanged(object sender, EventArgs e)
        {
            cboJobType.Enabled = chkJobType.Checked;
        }

        private void frmDetailEmployeeReportSettings_Load(object sender, EventArgs e)
        {
            LoadForm();
        }

    }
}
