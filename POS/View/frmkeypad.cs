using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS.POSInventory
{
    public interface ICalculateDiscount
    {
        void DiscountAmount(string DiscountAmount);

    }
    public partial class frmkeypad : Form
    {
        private ICalculateDiscount m_parent;
        public frmkeypad(Form Parent)
        {
            m_parent = (ICalculateDiscount)Parent;
            InitializeComponent();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 1;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 2;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 3;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 4;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 5;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 6;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 7;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 8;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 9;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtcalculate.Text = txtcalculate.Text + 0;
        }

        private void btnBS_Click(object sender, EventArgs e)
        {
            if (txtcalculate.Text.Length > 0)
            {
                txtcalculate.Text = txtcalculate.Text.Substring(0, txtcalculate.Text.Length - 1);
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            m_parent.DiscountAmount(txtcalculate.Text);
            this.Dispose();
        }

        private void frmkeypad_Load(object sender, EventArgs e)
        {

        }
    }
}
