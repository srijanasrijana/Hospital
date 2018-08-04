using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace AccSwift.Forms
{
    public  partial class frmbulksms : Form
    {
        public frmbulksms()
        {
            InitializeComponent();
        }

        private void btnsend_Click(object sender, EventArgs e)
        {
            if(txtto.Text=="")
            {
                MessageBox.Show("Please Enter Receipant Phone Number");
                return;
            }
            try
            {
                WebClient client = new WebClient();
                string to, msg;
                to = txtto.Text;
                msg = txtmessage.Text;
                string baseurl = "";//Need Https URL of the providers
                client.OpenRead(baseurl);
                MessageBox.Show("Successfully Sent Message");
            }
            catch (Exception ex)
            {  
                MessageBox.Show(ex.Message);
            }
        }
    }
}
