using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inventory
{
    public partial class frmtenderamount : Form
    {
        private double totalamount = 0;
        public frmtenderamount()
        {
            InitializeComponent();
        }

        public frmtenderamount(double total)
        {
            totalamount = total;
            InitializeComponent();
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_paidamount_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue==13)
            {
                double returnamount;
                if (txt_paidamount.Text != "" && txt_tenderamount.Text != "0")
                {
                    returnamount = (Convert.ToDouble(txt_paidamount.Text)) - (Convert.ToDouble(txt_tenderamount.Text));
                    txt_returnamount.Text = returnamount.ToString();
                }
                this.Close();
            }
            
        }

        private void frmtenderamount_Load(object sender, EventArgs e)
        {
            txt_tenderamount.Text = totalamount.ToString();
            txt_paidamount.Focus();
            txt_paidamount.Select();
        }

        private void txt_paidamount_Leave(object sender, EventArgs e)
        {
          
                //double returnamount;
                //returnamount = (Convert.ToDouble(txt_paidamount.Text)) - (Convert.ToDouble(txt_tenderamount.Text));
                //txt_returnamount.Text = returnamount.ToString();

           
        }

        private void txt_paidamount_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (txt_paidamount.Text == "")
                {
                    txt_returnamount.Text = "0";
                    return;
                }
                double returnamount;
                returnamount = (Convert.ToDouble(txt_paidamount.Text)) - (Convert.ToDouble(txt_tenderamount.Text));
                txt_returnamount.Text = returnamount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
