using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace POS.POSInventory
{
    public partial class frmprintingtemplate : Form
    {
        public frmprintingtemplate()
        {
            InitializeComponent();
        }

        private void frmprintingtemplate_Load(object sender, EventArgs e)
        {
            cmbprintstyle.SelectedIndex = 0;
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            if(cmbprintstyle.SelectedIndex==0)
            {
                Global.templateOption = "POS";
            }
            if (cmbprintstyle.SelectedIndex ==1)
            {
                Global.templateOption = "DOTMATRIX";
            }
        }
    }
}
