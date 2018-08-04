using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using RegistryManager;
using DateManager;
using System.Collections;
using Common;

namespace AccSwift
{
    public partial class frmSnoozeReminder : Form,IfrmDateConverter
    {
        
        public frmSnoozeReminder()
        {
            InitializeComponent();
        }

        public frmSnoozeReminder(string reminderId)
        {
            InitializeComponent();
            ReminderIdTextBox.Text = reminderId;
            
        }

       

        private void frmSnoozeReminder_Load(object sender, EventArgs e)
        {
            //Set the date style to whatever is set in the settings e.g. Nepali, English, MM_DD_YYYY etc.
            //txtToDate.Mask = Date.FormatToMask();
            LoadComboBox(sender, e);
            txtToDate.Mask = Date.FormatToMask();

            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
                 
        }

        private void LoadComboBox(object sender, EventArgs e)
        {
            cboSnoozeDay.Items.Add(new ListItem(0,"none"));
            cboSnoozeDay.Items.Add(new ListItem(5,"5 Days"));
            cboSnoozeDay.Items.Add(new ListItem(10,"10 Days"));
            cboSnoozeDay.Items.Add(new ListItem(15,"15 Days"));
            cboSnoozeDay.Items.Add(new ListItem(30,"30 Days"));
            cboSnoozeDay.SelectedIndex = 0;
        }

        private void cboSnoozeDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            int SelectedID=((ListItem)cboSnoozeDay.SelectedItem).ID;
            txtToDate.Text = Date.ToSystem(Date.GetServerDate().AddDays(SelectedID));
            
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "UPDATE system.tblReminder set date='" + Date.ToDotNet(txtToDate.Text) + "' , modified_by=" + User.CurrUserID + " where reminderid=" + ReminderIdTextBox.Text.ToInt();

                Global.m_db.InsertUpdateQry(query);
                
                this.Close();
                  
                        
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            
        }

        public void DateConvert(DateTime DotNetDate)
        {
           this. txtToDate.Text = Date.ToSystem(DotNetDate);
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtToDate.Text));
            frm.ShowDialog();
        }

     
        private void frmSnoozeReminder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        
    }
}


        
       