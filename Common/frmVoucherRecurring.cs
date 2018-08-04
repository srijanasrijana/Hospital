using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace Common
{
    public partial class frmVoucherRecurring : Form
    {
        string m_VoucherID = "", m_VoucherType = "";
        int RVID = 0;
        DataTable m_dtRecurringSetting = null;
        Form m_callingForm;
        public enum Months { Baisakh = 1, Jestha, Asar, Shrawan, Bhadra, Aswin, Kartik, Mangsir, Poush, Magh, Falgun, Chaitra }

        public enum RecurringType { DAILY = 1, MONTHLY, YEARLY }

        public enum Days { SUN = 1, MON, TUE, WED, THURS, FRI, SAT }
        public frmVoucherRecurring()
        {
            InitializeComponent();
        }
        public frmVoucherRecurring(IVoucherRecurring callingForm, int RVID)
        {
            InitializeComponent();
            m_callingForm1 = (IVoucherRecurring)callingForm;
            this.RVID = RVID;
            m_dtRecurringSetting = RecurringVoucher.GetRecurringSetting(RVID);
            m_VoucherID = m_dtRecurringSetting.Rows[0]["VoucherID"].ToString();
            m_VoucherType = m_dtRecurringSetting.Rows[0]["VoucherType"].ToString();

        }
        //public frmVoucherRecurring(string voucherID, string voucherType)
        //{
        //    InitializeComponent();
        //    this.m_VoucherID = voucherID;
        //    this.m_VoucherType = voucherType;
        //}
        //public frmVoucherRecurring(Object CallingForm)
        //{
        //    InitializeComponent();
        //    this.m_callingForm = CallingForm;
        //}

        //public frmVoucherRecurring(Form callingForm , string voucherType , DataTable recurringSetting)
        //{ 
        //    InitializeComponent();
        //    m_callingForm = callingForm;
        //    m_VoucherType = voucherType;
        //    m_recurringSetting = recurringSetting;          
        //}
        IVoucherRecurring m_callingForm1 = null;
        public frmVoucherRecurring(IVoucherRecurring callingForm, string voucherType, DataTable recurringSetting)
        {
            InitializeComponent();
            m_callingForm1 = (IVoucherRecurring)callingForm;
            m_VoucherType = voucherType;
            m_dtRecurringSetting = recurringSetting;
        }
        //public frmVoucherRecurring(Form callingForm , string voucherType)
        //{
        //    InitializeComponent();
        //    if(voucherType == "SALES")
        //        m_callingForm = callingForm;
        //    m_VoucherType = voucherType;
        //}

        private void frmRecurring_Load(object sender, EventArgs e)
        {
            rdoDaily.Checked = true;
            cmboMonth.DataSource = Enum.GetValues(typeof(Months));

            for (int i = 1; i <= 32; i++)
            {
                cmboDayY.Items.Add(i.ToString());
                cmboDayM.Items.Add(i.ToString());
            }
            cmboDayM.SelectedIndex = 0;
            cmboDayY.SelectedIndex = 0;

            if (m_dtRecurringSetting.Rows.Count > 0)
            {
                LoadSettings();
            }

        }
        private void LoadSettings()
        {
            try
            {
                if (m_dtRecurringSetting.Rows[0]["RecurringType"].ToString() == "DAILY")
                {
                    EnableGroupBoxes(true, false, false);
                    //if (m_dtRecurringSetting.Rows[0]["Unit1"].ToString() != "")
                    //{
                    foreach (char ch in m_dtRecurringSetting.Rows[0]["Unit1"].ToString())
                    {
                        if (ch == '1')
                            chkSunday.Checked = true;
                        if (ch == '2')
                            chkMonday.Checked = true;
                        if (ch == '3')
                            chkTuesday.Checked = true;
                        if (ch == '4')
                            chkWednesday.Checked = true;
                        if (ch == '5')
                            chkThursday.Checked = true;
                        if (ch == '6')
                            chkFriday.Checked = true;
                        if (ch == '7')
                            chkSaturday.Checked = true;
                        //}
                    }

                }
                else if (m_dtRecurringSetting.Rows[0]["RecurringType"].ToString() == "MONTHLY")
                {
                    EnableGroupBoxes(false, true, false);
                    if (m_dtRecurringSetting.Rows[0]["Unit1"].ToString() == "100")
                    {
                        chkIsLastM.Checked = true;
                        chkIsLastM_CheckedChanged(null, null);
                    }
                    else
                    {
                        cmboDayM.Text = m_dtRecurringSetting.Rows[0]["Unit1"].ToString();
                    }
                }
                else if (m_dtRecurringSetting.Rows[0]["RecurringType"].ToString() == "YEARLY")
                {
                    EnableGroupBoxes(false, false, true);
                    if (m_dtRecurringSetting.Rows[0]["Unit1"].ToString() == "100")
                    {
                        chkIsLastY.Checked = true;
                        chkIsLastY_CheckedChanged(null, null);
                    }
                    else
                    {
                        cmboMonth.Text = ((Months)Convert.ToInt32(m_dtRecurringSetting.Rows[0]["Unit1"])).ToString();
                        cmboDayY.Text = m_dtRecurringSetting.Rows[0]["Unit2"].ToString();
                    }
                }
                txtDescription.Text = m_dtRecurringSetting.Rows[0]["Description"].ToString();

            }
            catch (Exception)
            {

                Global.MsgError("Recurring setting is not added for this voucher.");
            }
        }


        private void EnableGroupBoxes(bool Daily, bool Monthly, bool Yearly)
        {
            // for controls of groupbox "daily"
            rdoDaily.Checked = Daily;
            chkSunday.Enabled = chkMonday.Enabled = chkTuesday.Enabled = chkWednesday.Enabled = chkThursday.Enabled = chkFriday.Enabled = chkSaturday.Enabled = Daily;

            // for controls of groupbox "monthly"
            rdoMonthly.Checked = Monthly;
            cmboDayM.Enabled = chkIsLastM.Enabled = Monthly;

            // for controls of groupbox "Yearly"
            rdoYearly.Checked = Yearly;
            cmboDayY.Enabled = cmboMonth.Enabled = chkIsLastY.Enabled = Yearly;
        }

        private void rdoDaily_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDaily.Checked)
                EnableGroupBoxes(true, false, false);
        }

        private void rdoMonthly_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMonthly.Checked)
                EnableGroupBoxes(false, true, false);
        }

        private void rdoYearly_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoYearly.Checked)
                EnableGroupBoxes(false, false, true);
        }

        private void chkIsLastY_CheckedChanged(object sender, EventArgs e)
        {
            cmboDayY.Enabled = cmboMonth.Enabled = !chkIsLastY.Checked;
        }

        private void chkIsLastM_CheckedChanged(object sender, EventArgs e)
        {
            cmboDayM.Enabled = !chkIsLastM.Checked;
        }
        DataTable dt = new DataTable();
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string unit1 = "", unit2 = "", recurringType = "";
                dt.Columns.Clear();
                dt.Columns.Add("RVID");
                dt.Columns.Add("VoucherID");
                dt.Columns.Add("VoucherType");
                dt.Columns.Add("RecurringType");
                dt.Columns.Add("Description");
                dt.Columns.Add("Unit1");
                dt.Columns.Add("Unit2");
                dt.Columns.Add("Date");
                if (rdoDaily.Checked)
                {
                    recurringType = "DAILY";
                    unit2 = "0";
                    if (chkSunday.Checked)
                    {
                        unit1 += "1";
                    }
                    if (chkMonday.Checked)
                    {
                        unit1 += "2";
                    }
                    if (chkTuesday.Checked)
                    {
                        unit1 += "3";
                    }
                    if (chkWednesday.Checked)
                    {
                        unit1 += "4";
                    }
                    if (chkThursday.Checked)
                    {
                        unit1 += "5";
                    }
                    if (chkFriday.Checked)
                    {
                        unit1 += "6";
                    }
                    if (chkSaturday.Checked)
                    {
                        unit1 += "7";
                    }
                    if (unit1 == "")    // check if none of the days have been selected and display error message 
                    {
                        Global.MsgError("You must select a day under \"Daily\" to proceed.");
                        return;
                    }
                }
                else if (rdoMonthly.Checked)
                {
                    recurringType = "MONTHLY";
                    unit2 = "0";
                    if (!chkIsLastM.Checked)
                    {
                        unit1 = cmboDayM.Text;
                    }
                    else
                    {
                        unit1 = "100";
                    }
                }

                else if (rdoYearly.Checked)
                {
                    recurringType = "YEARLY";
                    if (!chkIsLastY.Checked)
                    {
                        unit1 = (Convert.ToInt32(cmboMonth.SelectedValue)).ToString();
                        unit2 = cmboDayY.Text;
                    }
                    else
                    {
                        unit1 = "100";
                        unit2 = "100";
                    }
                }

                // adding setting values to dataTable
                DataRow dr = dt.NewRow();
                dr["RVID"] = "";
                dr["VoucherID"] = m_VoucherID;
                dr["VoucherType"] = m_VoucherType;
                dr["RecurringType"] = recurringType;
                dr["Description"] = txtDescription.Text;
                dr["Unit1"] = unit1;
                dr["Unit2"] = unit2;
                dr["Date"] = System.DateTime.Today;
                dt.Rows.Add(dr);

                m_callingForm1.GetRecurringSetting(dt);
                this.Dispose();
                //DataRow dr1 = dt.NewRow();
            }
            catch (Exception ex)
            {

                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //frmVoucherRecurring_FormClosing(sender, null);
            // if cancel button is clicked and previously saved settings are not available then donot send settings table
            this.Dispose();
        }
    }

}

