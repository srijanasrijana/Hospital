using Inventory.POSInventory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS.POSLogIn
{
    public partial class frmposlogin : Form
    {
        public frmposlogin()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 1;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 2;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 3;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 4;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 5;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 6;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 7;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 8;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 9;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtpin.Text = txtpin.Text + 0;
        }

        private void btnBS_Click(object sender, EventArgs e)
        {
            if (txtpin.Text.Length > 0)
            {
                txtpin.Text = txtpin.Text.Substring(0, txtpin.Text.Length - 1);
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            frmpossalesinvoice si = new frmpossalesinvoice();
            si.ShowDialog();
            
        }
    }
}
