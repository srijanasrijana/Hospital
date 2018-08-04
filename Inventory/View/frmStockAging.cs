using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace Inventory
{
    public partial class frmStockAging : Form
    {
        public frmStockAging()
        {
            InitializeComponent();
        }

        private void frmStockAging_Load(object sender, EventArgs e)
        {
            loadProduct(cmbProduct);
            loadGroup(cmbGroup);
        }
        private void loadProduct(ComboBox combo)
        {
            Product pr=new Product();
            DataTable dt = pr.getProductByGroupID();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                combo.Items.Add(new ListItem((int)dr["ProductID"], dr["EngName"].ToString()));
            }
            combo.DisplayMember = "value";
            combo.ValueMember = "id";
        }
        private void loadGroup(ComboBox combo)
        {
            Product pr = new Product();
            DataTable dt = pr.getGroup();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                combo.Items.Add(new ListItem((int)dr["GroupID"], dr["EngName"].ToString()));
            }
            combo.DisplayMember = "value";
            combo.ValueMember = "id";
        }

        private void chkshowalldebtors_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProduct.Checked == true)
            {
                cmbProduct.Enabled = false;
            }
            else
            {
                cmbProduct.Enabled = true;
            }
        }

        private void chkgroup_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void sTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
