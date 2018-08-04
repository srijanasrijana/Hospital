using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AccSwift
{
    public partial class frmTestCalculator : Form
    {
        private string displayinput;
        private decimal result;
        private bool Trigger = false;
        private string strOperator = "";
        private string FristNumber = "";
        public frmTestCalculator()
        {
            InitializeComponent();
        }

        private void frmTestCalculator_Load(object sender, EventArgs e)
        {


        }


    

        private void btnone_Click(object sender, EventArgs e)
        {
            Button btnNumbers = (Button)sender;
            txtInputResult.Text += (btnNumbers.Text);
        }

 

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Button btnOperators = (Button)sender;
            strOperator = btnOperators.Text;
            FristNumber = txtInputResult.Text;
            Trigger = true;
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {

        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            //if (txtInputResult.Text.Contains == ".")
            //    return;
        }
    }
}
