using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using Common;

namespace Accounts
{
    public partial class frmLedgerConfiguration : Form, IfrmVoucherFormat
    {
        private DataTable dtNumFormat = new DataTable();

        public frmLedgerConfiguration()
        {
            InitializeComponent();
        }

        public void AddNumberingFormat(DataTable NumberFormat)
        {
            dtNumFormat = NumberFormat;
        }

        private void LoadcmbLdrNumberingType(object sender, EventArgs e)
        {
            cmbLdrNumberingType.Items.Add(new ListItem(0, "Manual"));
            cmbLdrNumberingType.Items.Add(new ListItem(1, "Automatic"));
            cmbLdrNumberingType.SelectedIndex = LedgerCodeConfig.GetLedgerNumberingType();


        }



        private void frmLedgerConfiguration_Load(object sender, EventArgs e)
        {
            //cmbLdrNumberingType.Items.Clear();
            LoadcmbLdrNumberingType(sender, e);
            LoadcmbGrpNumberingType(sender, e);

            //Load the existing dtNumFormat if btn is not pressed


        }

        private void btnNumberingFormat_Click(object sender, EventArgs e)
        {
            frmVoucherFormat frm = new frmVoucherFormat(this, -1);
            frm.ShowDialog();
        }

        private void cmbNumberingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLdrNumberingType.SelectedItem.ToString() == "Automatic")
            {
                btnLdrNumberingFormat.Enabled = true;
                chkLdrCompulsory.Enabled = false;
                lblLedgerCodeFormat.Visible = true;


            }
            //for making manual block enable true and other false
            else
            {
                btnLdrNumberingFormat.Enabled = false;
                chkLdrCompulsory.Enabled = true;
                lblLedgerCodeFormat.Visible = false;

            }

        }

        private void btnSaveLedgerFormat_Click(object sender, EventArgs e)
        {
            if (cmbLdrNumberingType.SelectedItem.ToString() == "Automatic")
            {

                //save Ledger configuration to tblLedgerCodeConfig
                //While saving save the numbering format to tblLedgerCodeFormat table also
                LedgerCodeConfig.InsertLedgerCodeConfiguration(cmbLdrNumberingType.SelectedIndex, chkLdrCompulsory.Checked ? 1 : 0, dtNumFormat);
                Global.Msg("The numbering format is changed successfully .");
                this.Close();
            }
            else
            {
                Global.m_db.InsertUpdateQry("UPDATE acc.tblLedgerCodeConfig SET NumberingType=" + cmbLdrNumberingType.SelectedIndex + " ,IsCompulsory=" + (chkLdrCompulsory.Checked ? 1 : 0).ToString());
                Global.Msg("The numbering format is changed successfully .");
                this.Close();
            }
        }
        //Group Tab region
        #region Grouptab

        private void LoadcmbGrpNumberingType(object sender, EventArgs e)
        {
            cmbGrpNumberingType.Items.Add(new ListItem(0, "Manual"));
            cmbGrpNumberingType.Items.Add(new ListItem(1, "Automatic"));
            cmbGrpNumberingType.SelectedIndex = LedgerCodeConfig.GetLedgerGrpNumberingType();


        }

        private void btnGrpSave_Click(object sender, EventArgs e)
        {
            if (cmbGrpNumberingType.SelectedItem.ToString() == "Automatic")
            {

                //save Ledger configuration to tblLedgerCodeConfig
                //While saving save the numbering format to tblLedgerCodeFormat table also
                LedgerCodeConfig.InsertGroupCodeConfiguration(cmbGrpNumberingType.SelectedIndex, cboGrpCompulsory.Checked ? 1 : 0, dtNumFormat);
                Global.Msg("The numbering format is changed successfully .");
                this.Close();
            }
            else
            {
                Global.m_db.InsertUpdateQry("UPDATE acc.tblGroupCodeConfig SET NumberingType=" + cmbGrpNumberingType.SelectedIndex + " ,IsCompulsory=" + (cboGrpCompulsory.Checked ? 1 : 0).ToString());
                Global.Msg("The numbering format is changed successfully .");
                this.Close();
            }
        }

        private void cmbGrpNumberingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGrpNumberingType.SelectedItem.ToString() == "Automatic")
            {
                btnGrpNumberingFormat.Enabled = true;
                cboGrpCompulsory.Enabled = false;
                lblGrpFormat.Visible = true;


            }
            //for making manual block enable true and other false
            else
            {
                btnGrpNumberingFormat.Enabled = false;
                cboGrpCompulsory.Enabled = true;
                lblGrpFormat.Visible = false;

            }

        }

        private void btnGrpNumberingFormat_Click(object sender, EventArgs e)
        {
            frmVoucherFormat frm = new frmVoucherFormat(this, 0);
            frm.ShowDialog();
        }
        #endregion
    }
}
