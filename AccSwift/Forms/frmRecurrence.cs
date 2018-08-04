using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DateManager;
using BusinessLogic;
using Common;

namespace AccSwift
{
    public interface IfrmRecurrence
    {
        void Recurrence(DataTable RecurrenceTable);
    }

    public partial class frmReoccurance : Form, IfrmDateConverter
    {
        private bool isStartDate = false;
        public IfrmRecurrence m_ParentForm;
        private int ReminderID = 0;
        DataTable dtFromReminder = new DataTable();

        public frmReoccurance()
        {
            InitializeComponent();
        }

        public frmReoccurance(Form ParentForm, DataTable existingRecurrence)
        {
            InitializeComponent();
            //m_ParentForm = frm;
            m_ParentForm = (IfrmRecurrence)ParentForm;
            if (existingRecurrence.Rows.Count != 0)
            {
                dtFromReminder = existingRecurrence;
                DataRow dr = existingRecurrence.Rows[0];
                ReminderID = Convert.ToInt32(dr["ReminderID"]);
                int test = Convert.ToInt32(dr["OccurencePattern"]);
                switch (test)
                {
                    case 0: rdbDaily.Checked = true;
                        break;
                    case 1: rdbWeekly.Checked = true;
                        break;
                    case 2: rdbMonthly.Checked = true;
                        break;
                    case 3: rdbYearly.Checked = true;
                        break;
                }
                //txtStartDate.Text = Date.DBToSystem(dr["OccurenceDateStart"].ToString());
                //if (dr["OccurenceDateEnd"].ToString() != "1990/04/05")
                //{
                //    txtEndDate.Text = Date.DBToSystem(dr["OccurenceDateEnd"].ToString());
                //}
                //else
                //{
                //    chkNoEndDate.Checked = true;
                //}
            }          
        }

        public void DateConvert(DateTime DotNetDate)
        {
            if (isStartDate)
                txtStartDate.Text = Date.ToSystem(DotNetDate);
            if (!isStartDate)
                txtEndDate.Text = Date.ToSystem(DotNetDate);
        }

        private void frmReoccurance_Load(object sender, EventArgs e)
        {
            //Load Start/Due Date
            txtStartDate.Mask = Date.FormatToMask();
            txtStartDate.Text = Date.ToSystem(Date.GetServerDate());
            txtEndDate.Mask = Date.FormatToMask();
            txtEndDate.Text = Date.ToSystem(Date.GetServerDate());
        }

        private void btnStartDate_Click(object sender, EventArgs e)
        {
            isStartDate = true;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtStartDate.Text));
            frm.ShowDialog();
        }

        private void btnEndDate_Click(object sender, EventArgs e)
        {
            isStartDate = true;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtEndDate.Text));
            frm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_ParentForm.Recurrence(dtFromReminder);
            this.Close();          
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //FormHandle m_FHandle = new FormHandle();
            //m_FHandle.AddValidate(txtStartDate, DType.DATE, "Invalid start date!!");
            //m_FHandle.AddValidate(txtEndDate, DType.DATE, "Invalid end date!!");
            //if (!m_FHandle.Validate())
            //    return;          

             DataTable  dt = GetTable();                     
            m_ParentForm.Recurrence(dt);
            this.Close();
        }

        /// <summary>
        /// This example method generates a DataTable.
        /// </summary>
        private DataTable GetTable()
        {
            int RecurrencePattern = 0;
            if (rdbDaily.Checked) RecurrencePattern = 0;
            if (rdbWeekly.Checked) RecurrencePattern = 1;
            if (rdbMonthly.Checked) RecurrencePattern = 2;
            if (rdbYearly.Checked) RecurrencePattern = 3;
            DataTable table = new DataTable();
            //table.Columns.Add("OccurenceDateStart", typeof(DateTime));
            //table.Columns.Add("OccurenceDateEnd", typeof(DateTime));
            table.Columns.Add("OccurencePattern", typeof(int));
           // if (chkNoEndDate.Checked)
                table.Rows.Add(RecurrencePattern);
            // table.Rows.Add(txtStartDate.Text,"2021/01/01", RecurrencePattern);  
            //else
            //    table.Rows.Add(txtStartDate.Text, txtEndDate.Text, RecurrencePattern);                      
            return table;
        }

        private void chkNoEndDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoEndDate.Checked)
                txtEndDate.Enabled = false;
            if (!chkNoEndDate.Checked)
                txtEndDate.Enabled = true;
        }

        private void btnClearReminder_Click(object sender, EventArgs e)
        {
            Reminder.DeleteRecurrence(Convert.ToInt32(ReminderID));
            MessageBox.Show("Recurrence not set!!");
            dtFromReminder.Clear();
            m_ParentForm.Recurrence(dtFromReminder);
            this.Close();
        }
    }
}
