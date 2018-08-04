using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace Accounts
{
    public partial class frmSlabs : Form
    {
        Slabs m_Slabs = new Slabs();
        SlabType SlabCode=SlabType.None;
        DataTable dtSlabInfo = new DataTable();
        private string OldSlab = " ";
        private string NewSlab = " ";
        public frmSlabs()
        {
            InitializeComponent();
        }

        public frmSlabs(SlabType SlabCode)
        {
            this.SlabCode = SlabCode;
            InitializeComponent();
        }

        private void frmSlabs_Load(object sender, EventArgs e)
        {
                
                dtSlabInfo = Slabs.GetSlabInfo(SlabCode);
                

                for (int i = 0; i < dtSlabInfo.Rows.Count; i++)
                {
                    DataRow drSlabsInfo = dtSlabInfo.Rows[i];
                    txtName.Text = drSlabsInfo["Name"].ToString();
                    txtRate.Text = drSlabsInfo["Rate"].ToString();
                    txtDescription.Text = drSlabsInfo["Remarks"].ToString();                              
                }
                OldSlab = " ";
                OldSlab = OldSlab + " Name" + txtName + "Rate" + txtRate.Text + "Desc" + txtDescription.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NewSlab = " ";

            NewSlab = NewSlab + " Name" + txtName + "Rate" + txtRate.Text + "Desc" + txtDescription.Text;
            //Write the validation code before saving
            m_Slabs.Modify(SlabCode, txtName.Text,Convert.ToDouble(txtRate.Text), txtDescription.Text,OldSlab,NewSlab);
            switch (SlabCode)
            {
                case SlabType.TAX1:
                    Global.Default_Tax1 = Convert.ToDouble(txtRate.Text);
                    break;
                case SlabType.TAX2:
                    Global.Default_Tax2 = Convert.ToDouble(txtRate.Text);
                    break;
                case SlabType.TAX3:
                    Global.Default_Tax3 = Convert.ToDouble(txtRate.Text);
                    break;
                case SlabType.CUSTOMDUTY:
                    Global.Default_CustomDuty = Convert.ToDouble(txtRate.Text);
                    break;
                case SlabType.None:
                    Global.Default_Vat = Convert.ToDouble(txtRate.Text);
                    break;

            }
            //Updating slab's value on Global variable too.

            Global.Msg("Slabs Modified Successfully!");
        }

        private void frmSlabs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnGrpCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
