using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class frmPurchaseVoucherConfig : Form
    {
        public frmPurchaseVoucherConfig()
        {
            InitializeComponent();
        }

        private void frmPurchaseVoucherConfig_Load(object sender, EventArgs e)
        {

        }

        private void frmPurchaseVoucherConfig_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
