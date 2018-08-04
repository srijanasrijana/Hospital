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
    public partial class frmFreeze : Form, IfrmDateConverter
    {        
        public bool IsStartDate = false;
        public frmFreeze()
        {
            InitializeComponent();            
        }

        public void DateConvert(DateTime DotNetDate)
        {
            if(IsStartDate)
            txtFreezeStartDate.Text = Date.ToSystem(DotNetDate);
            if (!IsStartDate)
                txtFreezeEndDate.Text = Date.ToSystem(DotNetDate);
        }

        private void frmFreeze_Load(object sender, EventArgs e)
        {
            if (Settings.GetSettings("FREEZE_STATUS") != "0") //Freeze date is set
            {
                chkAllowFreeze.Checked = true;
                txtFreezeStartDate.Mask = Date.FormatToMask();
                txtFreezeStartDate.Text = Settings.GetSettings("FREEZE_START_DATE");
                txtFreezeEndDate.Mask = Date.FormatToMask();
                txtFreezeEndDate.Text = Settings.GetSettings("FREEZE_END_DATE");
                btnFreeze.Enabled = false;
                btnDefreeze.Enabled = true;
            }
            else
            {
                chkAllowFreeze.Checked = false;
                txtFreezeStartDate.Mask = Date.FormatToMask();
                txtFreezeStartDate.Text = Date.ToSystem(Date.GetServerDate());
                txtFreezeEndDate.Mask = Date.FormatToMask();
                txtFreezeEndDate.Text = Date.ToSystem(Date.GetServerDate());
                btnFreeze.Enabled = true;
                btnDefreeze.Enabled = false;
            }
        }

        private void chkAllowFreeze_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllowFreeze.Checked)
            {
                txtFreezeStartDate.Enabled = true;
                txtFreezeEndDate.Enabled = true;
                btnFreezeStartDate.Enabled = true;
                btnFreezeEndDate.Enabled = true;
                btnFreeze.Enabled = true;

                if (Settings.GetSettings("FREEZE_STATUS") != "0")
                {
                    txtFreezeStartDate.Mask = Date.FormatToMask();
                    txtFreezeStartDate.Text = Settings.GetSettings("FREEZE_START_DATE");
                    txtFreezeEndDate.Mask = Date.FormatToMask();
                    txtFreezeEndDate.Text = Settings.GetSettings("FREEZE_END_DATE");
                    btnFreeze.Enabled = false;
                    btnDefreeze.Enabled = true;
                }
                else
                {
                    txtFreezeStartDate.Mask = Date.FormatToMask();
                    txtFreezeStartDate.Text = Date.ToSystem(Date.GetServerDate());
                    txtFreezeEndDate.Mask = Date.FormatToMask();
                    txtFreezeEndDate.Text = Date.ToSystem(Date.GetServerDate());
                    btnFreeze.Enabled = true;
                    btnDefreeze.Enabled = false;
                }
            }
            if (!chkAllowFreeze.Checked)
            {
                txtFreezeStartDate.Enabled = false;
                txtFreezeEndDate.Enabled = false;
                btnFreezeStartDate.Enabled = false;
                btnFreezeEndDate.Enabled = false;
                btnFreeze.Enabled = true;
                btnDefreeze.Enabled = false;
            }
           
        }

        private void btnFreezeStartDate_Click(object sender, EventArgs e)
        {
            IsStartDate = true;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtFreezeStartDate.Text));
            frm.ShowDialog();
        }

        private void btnFreezeEndDate_Click(object sender, EventArgs e)
        {
            IsStartDate = false;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtFreezeEndDate.Text));
            frm.ShowDialog();
        }

        private void btnFreezeDefreeze_Click(object sender, EventArgs e)
        {
            Settings m_Settings = new Settings();
            string Code, Value;

            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();

            if (Date.ToDotNet(txtFreezeStartDate.Text) < Date.ToDotNet(Date.DBToSystem(CompDetails.FYFrom.ToString())))
            {
                MessageBox.Show("Invalid Start date!");
                txtFreezeStartDate.Focus();
                return;
            }
            if (Date.ToDotNet(txtFreezeEndDate.Text) < Date.ToDotNet(Date.DBToSystem(CompDetails.FYFrom.ToString())))
            {
                MessageBox.Show("Invalid End date!");
                txtFreezeEndDate.Focus();
                return;
            }

            if (Date.ToDotNet(txtFreezeStartDate.Text) > Date.ToDotNet(txtFreezeEndDate.Text))
            {
                MessageBox.Show("Invalid Start date!");
                txtFreezeStartDate.Focus();
                return;
            }
                Code = "FREEZE_STATUS";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Code = "FREEZE_START_DATE";
                Value = Date.ToSystem(Date.ToDotNet(txtFreezeStartDate.Text));
                m_Settings.SetSettings(Code, Value);
                Code = "FREEZE_END_DATE";
                Value = Date.ToSystem(Date.ToDotNet(txtFreezeEndDate.Text));
                m_Settings.SetSettings(Code, Value);                  
                btnFreeze.Enabled = false;
                btnDefreeze.Enabled = true;        
        }

        private void btnDefreeze_Click(object sender, EventArgs e)
        {
            Settings m_Settings = new Settings();
            string Code, Value;

            Code = "FREEZE_STATUS";
            Value = "0";
            m_Settings.SetSettings(Code, Value);
            chkAllowFreeze.Checked = false;
            btnDefreeze.Enabled = false;
            btnFreeze.Enabled = true;                             
        }
    }
}
