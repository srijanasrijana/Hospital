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
    public partial class frmHosMasterSetup : Form
    {
        public frmHosMasterSetup()
        {
            InitializeComponent();
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
           
            frmHosDepartment department = new frmHosDepartment();
            department.ShowDialog();
        }

        private void btnspecialization_Click(object sender, EventArgs e)
        {
            frmHosSpecialization specilization = new frmHosSpecialization();
            specilization.ShowDialog();
        }

        private void btnEmpFaculty_Click(object sender, EventArgs e)
        {
            frmDoctorFaculty faculty = new frmDoctorFaculty();
            faculty.ShowDialog();

        }

        private void btnLoan_Click(object sender, EventArgs e)
        {
        
        }
        private void btnLevel_Click(object sender, EventArgs e)
        {
            frmDoctorLevel level = new frmDoctorLevel();
            level.ShowDialog();
        }

        private void btnDiseases_Click(object sender, EventArgs e)
        {
          //  frmHosDiseases dis = new frmHosDiseases();
          //  dis.ShowDialog();
        }

        private void btncountry_Click(object sender, EventArgs e)
        {
           
        }

        private void frmHosMasterSetup_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                frmDoctorList list = new frmDoctorList();
                list.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

    
    }
}
