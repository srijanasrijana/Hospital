using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using Common;

namespace Inventory.Forms
{
    public partial class frmOutStandingDueToDebtors : Form, IfrmDateConverter
    {
        BusinessLogic.AccountGroup acc = new AccountGroup();
        public frmOutStandingDueToDebtors()
        {
            InitializeComponent();
        }

        private void frmOutStandingDueToDebtors_Load(object sender, EventArgs e)
        {
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.ToSystem(Date.GetServerDate());
            loadParty(cmbParty);
        }
        private void loadParty(ComboBox combo)
        {
            combo.Items.Clear();
            DataTable dt = acc.getPartyByID();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                combo.Items.Add(new ListItem((int)dr["LedgerID"], dr["EngName"].ToString()));
            }
            combo.DisplayMember = "value";
            combo.ValueMember = "id";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                cmbParty.Enabled = false;
            }
            else
            {
                cmbParty.Enabled = true;
            }
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            DateTime dtDate1 = Date.ToDotNet(txtDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate1);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }
        public void DateConvert(DateTime DotNetDate)
        {
            txtDate.Text = Date.ToSystem(DotNetDate);
        }
    }
}
