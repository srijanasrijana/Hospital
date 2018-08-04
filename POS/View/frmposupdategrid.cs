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
    public interface ISalesInvoice
    {
        void UpdateQuantity(int Quantity,int rindex,double FinalValue);
       
    }
    public partial class frmposupdategrid : Form
    {
        private string productName;
        private int qty;
        private int index;
        private double total;
        private ISalesInvoice m_ParentForm;
        private bool isFocus;
        public frmposupdategrid()
        {
            InitializeComponent();
        }
        public frmposupdategrid(int rowindex, Form ParentForm, string prodname, int pqty,double value)
        {
            
            index = rowindex;
            m_ParentForm = (ISalesInvoice)ParentForm;
            productName = prodname;
            qty = pqty;
            total = value;
            InitializeComponent();
            //txtquantity.Focus();
        }
        public frmposupdategrid(string prodname,int pqty)
        {
            productName = prodname;
            //lblproductname.Text = productName;
           
            qty = pqty;
            //txtquantity.Text = Convert.ToString(qty);
            InitializeComponent();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (isFocus == true)
            {
                txtquantity.Text = "1";
            }
            else
            {
                txtquantity.Text = txtquantity.Text + 1;
            }
            this.ActiveControl = btnupdate;
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            if (txtquantity.Text.Length > 0)
            {
                txtquantity.Text = txtquantity.Text.Substring(0, txtquantity.Text.Length - 1);
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
           
            if (isFocus == true)
            {
                txtquantity.Text = "2";
            }
            else
            {
                txtquantity.Text = txtquantity.Text + 2;
            }
            this.ActiveControl = btnupdate;
           // e.SuppressKeyPress = true;
            
            
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (isFocus == true)
            {
                txtquantity.Text = "3";
            }
            else
            {
                txtquantity.Text = txtquantity.Text + 3;
            }
            this.ActiveControl = btnupdate;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            if (isFocus == true)
            {
                txtquantity.Text = "4";
            }
            else
            {
                txtquantity.Text = txtquantity.Text + 4;
            }
            this.ActiveControl = btnupdate;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (isFocus == true)
            {
                txtquantity.Text = "5";
            }
            else
            {
                txtquantity.Text = txtquantity.Text + 5;
            }
            this.ActiveControl = btnupdate;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            if (isFocus == true)
            {
                txtquantity.Text = "6";
            }
            else
            {
                txtquantity.Text = txtquantity.Text + 6;
            }
            this.ActiveControl = btnupdate;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            if (isFocus == true)
            {
                txtquantity.Text = "7";
            }
            else
            {
                txtquantity.Text = txtquantity.Text + 7;
            }
            this.ActiveControl = btnupdate;
            
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            if (isFocus == true)
            {
                txtquantity.Text = "8";
            }
            else
            {
                txtquantity.Text = txtquantity.Text + 8;
            }
            this.ActiveControl = btnupdate;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            if (isFocus == true)
            {
                txtquantity.Text = "9";
            }
            else
            {
                txtquantity.Text = txtquantity.Text + 9;
            }
            this.ActiveControl = btnupdate;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtquantity.Text = txtquantity.Text + 0;
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            txtquantity.Text = "0";
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            double FinalValue = Convert.ToDouble(txtquantity.Text) * total;
            m_ParentForm.UpdateQuantity( Convert.ToInt32( txtquantity.Text),index,FinalValue);
            this.Dispose();
        }

        private void frmposupdategrid_Load(object sender, EventArgs e)
        {
            //panel1.Controls[txtquantity].Text = "test";
            //panel1.Controls[txtquantity.Text].Focus();
            //panel1.Focus();
            txtquantity.Focus();
            this.ActiveControl = txtquantity;
            lblproductname.Text = productName;
            txtquantity.Text =Convert.ToString(qty);
            
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            //frmpossalesinvoice gridrowdelete=new frmpossalesinvoice();
            //gridrowdelete.deletegridrow();
            //orderTab.Rows[index].Delete();
            //RefreshDataGrid();

        }

        private void txtquantity_Enter(object sender, EventArgs e)
        {
            isFocus = true;
        }

        private void btnupdate_Enter(object sender, EventArgs e)
        {
            isFocus =false;
        }
    }
}
