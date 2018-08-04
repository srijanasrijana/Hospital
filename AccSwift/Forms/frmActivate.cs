using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DateManager;
using Cryptography;
using RegistryManager;

namespace AccSwift
{
    public partial class frmActivate : Form
    {
        public frmActivate()
        {
            InitializeComponent();
        }

        

        private void frmActivate_Load(object sender, EventArgs e)
        {
              txtID.Text = Crypto.GetHardwareKey();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            //Write in registry
            RegManager.ActivationCode=txtActivationCode.Text;
            //Check if the Activation Code is OK
            if (!Crypto.isActivated())
            {
                MessageBox.Show("Invalid Activation Code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else //Activation code is valid
            {
                MessageBox.Show("Activation Successful. You need to restart the application to take effect!");
            }



        }

        private void txtID_MouseClick(object sender, MouseEventArgs e)
        {
            txtID.SelectAll();
        }

        private void frmActivate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}