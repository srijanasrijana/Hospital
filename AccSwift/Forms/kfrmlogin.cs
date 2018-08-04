using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace AccSwift.Forms
{
    public partial class kfrmlogin : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public kfrmlogin()
        {
            InitializeComponent();
        }

        private void kryptonLabel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kbtncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        //    Application.Exit();
        }
    }
}