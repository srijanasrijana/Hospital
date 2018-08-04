using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Common;

namespace Common
{
    public partial class frmReportViewer : Form
    {
        public frmReportViewer()
        {
            InitializeComponent();
        }
       
        private void frmReportViewer_Load(object sender, EventArgs e)
        {

        }

        public void SetReportSource(ReportClass rptclass)
        {
            rptViewer.ReportSource = rptclass;

        }

        private void frmReportViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
