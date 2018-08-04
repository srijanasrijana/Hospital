using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HRM
{
    public partial class frmhrmmastersetup : Form
    {
        public frmhrmmastersetup()
        {
            InitializeComponent();
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("HRM_DEPARTMENT");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to Department. Please contact your administrator for permission.");
            //    return;
            //}
            frmDepartment department = new frmDepartment();
            department.ShowDialog();
        }

        private void btndesignation_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("HRM_DESIGNATION");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to Designation. Please contact your administrator for permission.");
            //    return;
            //}
            frmDesignation designation = new frmDesignation();
            designation.ShowDialog();
        }

        private void btncountry_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("HRM_NATIONALITY");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to Nationality. Please contact your administrator for permission.");
            //    return;
            //}
            frmCountry country = new frmCountry();
            country.ShowDialog();
        }

        private void frmhrmmastersetup_Load(object sender, EventArgs e)
        {

        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("HRM_LEVEL");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to Level. Please contact your administrator for permission.");
            //    return;
            //}
            View.frmEmpLevel em = new View.frmEmpLevel();
            em.ShowDialog();
        }

        private void btnLoan_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("HRM_LOAN");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to Loan. Please contact your administrator for permission.");
            //    return;
            //}
            View.frmLoan fl = new View.frmLoan();
            fl.ShowDialog();
        }

        private void btnEmpFaculty_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("HRM_FACULTY");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to employee faculty. Please contact your administrator for permission.");
            //    return;
            //}
            View.frmEmpFaculty fef = new View.frmEmpFaculty();
            fef.ShowDialog();
        }
    }
}
