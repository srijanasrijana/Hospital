using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public partial class frmProgress : Form
    {
        public frmProgress()
        {
            InitializeComponent();
        }


        public void UpdateProgress(int progress, string Message)
        {
            if (progressBar1.InvokeRequired)
                progressBar1.BeginInvoke(new Action(() => progressBar1.Value = progress));
            else
                progressBar1.Value = progress;

            if (lblMessage.InvokeRequired)
                lblMessage.BeginInvoke(new Action(() => lblMessage.Text = Message));
            else
                lblMessage.Text = Message;
        }

        /// <summary>
        /// Simply closes the form. It also handles itself whether it is called from a thread or not
        /// </summary>
        public void CloseForm()
        {
            if(this.InvokeRequired)
                this.BeginInvoke(new Action(()=>this.Close()));
            else
                this.Close();
        }

        private void frmProgress_Load(object sender, EventArgs e)
        {

        }

        private void frmProgress_Deactivate(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }
    }
}
