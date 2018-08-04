using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BusinessLogic;
using DateManager;
using Common;

namespace Inventory
{
     

    public interface IfrmVoucherFormat
    {
       
        void AddNumberingFormat(DataTable NumberFormat);

    }
   
    public partial class frmVoucherFormat : Form
    {

        private Form m_Parent; //Holds the parent form

        private IfrmVoucherFormat m_ParentForm;//holds the datatable

        private int SeriesID;

        public frmVoucherFormat(Form ParentForm)
        {
            InitializeComponent();
            m_ParentForm = (IfrmVoucherFormat)ParentForm;
        }
        public frmVoucherFormat(IMDIMainForm frm)
        {
            frm.OpenForm(frm.ToString());
            InitializeComponent();
           
        }
        public frmVoucherFormat()
        {
            InitializeComponent();
           
        }
        public frmVoucherFormat(Form ParentForm, int SeriesID)
        {
            InitializeComponent();
            m_ParentForm = (IfrmVoucherFormat)ParentForm;
            this.SeriesID = SeriesID;
        }

        private void btnAutoNum_Click(object sender, EventArgs e)
        {
           
            ListViewItem lvItem = new ListViewItem((lvNum.Items.Count + 1).ToString());
            lvItem.SubItems.Add("(AutoNumber)");
         
                lvItem.SubItems.Add("(Auto)");
                  
            lvNum.Items.Add(lvItem);
            Sample();
        }

        private void btnSymbol_Click(object sender, EventArgs e)
        {
            if (txtSymbol.Text == "")
            {
                MessageBox.Show("Please fill the required Symbol in the Corresponding field!");
                txtSymbol.Focus();
                return;
            
            }
            ListViewItem lvItem = new ListViewItem((lvNum.Items.Count + 1).ToString());
            lvItem.SubItems.Add("Symbol");
            lvItem.SubItems.Add(txtSymbol.Text);
            lvNum.Items.Add(lvItem);
            Sample();
        }

        private void Sample()
        {
            string str = "";
            for (int i = 0; i < lvNum.Items.Count; i++)
            {
                switch (lvNum.Items[i].SubItems[1].Text)
                {
                    case "(AutoNumber)":
                        if (lvNum.Items[i].SubItems[2].Text == "(Auto)")
                            str += "000253";
                        //else
                        //    str += "253".PadLeft(Convert.ToInt32(lvNum.Items[i].SubItems[2].Text), '0');
                        break;
                    case "Symbol":
                        str += lvNum.Items[i].SubItems[2].Text;
                        break;
                    case "Date":
                        if (lvNum.Items[i].SubItems[2].Text == "Nepali Fiscal Year(eg. 067/068)")
                            str += Global.Fiscal_Nepali_Year;
                        if (lvNum.Items[i].SubItems[2].Text == "English Fiscal Year (eg. 11/12)")
                            str += Global.Fiscal_English_Year;
                        
                        break;
                }
            }//end for loop
            lblSample.Text = str;
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            
            ListViewItem lvItem = new ListViewItem((lvNum.Items.Count + 1).ToString());
            lvItem.SubItems.Add("Date");
            lvItem.SubItems.Add(cboDate.Text);
            lvNum.Items.Add(lvItem);
            Sample();

        }

        private void frmVoucherFormat_Load(object sender, EventArgs e)
        {

            btnSymbol.Enabled = false;
            if (SeriesID > 0) //This is edit mode
            {
                //Load all old records

                //BLOCK FOR FILLING THE RESPECTIVE FIELDS OF NUMBERING FORMAT TAB
                DataTable dtVouFormatInfo = VoucherConfiguration.GetVouFormatInfo(SeriesID);
                for (int i = 0; i < dtVouFormatInfo.Rows.Count; i++)
                {

                    DataRow drVouFormatInfo = dtVouFormatInfo.Rows[i];

                    ListViewItem lvItem = new ListViewItem((lvNum.Items.Count + 1).ToString());
                    lvItem.SubItems.Add(drVouFormatInfo["Type"].ToString());
                    lvItem.SubItems.Add(drVouFormatInfo["Parameter"].ToString());
                    lvNum.Items.Add(lvItem);

                }

            }
            else if (SeriesID == -1) //This is ledger configuration mode
            {
                //Load all old records

                //BLOCK FOR FILLING THE RESPECTIVE FIELDS OF NUMBERING FORMAT TAB
                string sql = "select* from acc.tblLedgerCodeFormat";

                DataTable dtLedFormatInfo = Global.m_db.SelectQry(sql, "tblLedFormat");
                for (int i = 0; i < dtLedFormatInfo.Rows.Count; i++)
                {

                    DataRow drLedFormatInfo = dtLedFormatInfo.Rows[i];

                    ListViewItem lvItem = new ListViewItem((lvNum.Items.Count + 1).ToString());
                    if (drLedFormatInfo["Type"].ToString() == "(AutoNumber)")
                    {
                        lvItem.SubItems.Add(drLedFormatInfo["Type"].ToString());
                        lvItem.SubItems.Add("(Auto)");
                        lvNum.Items.Add(lvItem);
                    }
                    else
                    {
                        lvItem.SubItems.Add(drLedFormatInfo["Type"].ToString());
                        lvItem.SubItems.Add(drLedFormatInfo["Parameter"].ToString());
                        lvNum.Items.Add(lvItem);
                    }

                }

            }
            else //This is new mode
            {
                #region BY DEFAULT DISPLAY THE AUTO IN VOUCHER FORMAT IN FORM LOAD CONDTION



                ListViewItem lvItem1 = new ListViewItem((lvNum.Items.Count + 1).ToString());
                lvItem1.SubItems.Add("(AutoNumber)");


                lvItem1.SubItems.Add("(Auto)");


                lvNum.Items.Add(lvItem1);
                Sample();
                #endregion
            }
            //LIST THE DATE IN COMBOBOX

            cboDate.Items.Add("Nepali Fiscal Year(eg. 067/068)");
            cboDate.Items.Add("English Fiscal Year (eg. 11/12)");


            cboDate.SelectedIndex = 0;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable VoucherFormatDtl = new DataTable();// defining datatable...dynamic memory allocation.
                // VoucherFormatDtl.Columns.Add("Sno", typeof(int));//adding column as Sno(int type)
                VoucherFormatDtl.Columns.Add("Type", typeof(string));
                VoucherFormatDtl.Columns.Add("Param", typeof(string));
                VoucherFormatDtl.Columns.Add("SeriesID", typeof(int));
                foreach (ListViewItem lvItem in lvNum.Items)
                {

                    string DateFormat = "";

                    switch (lvItem.SubItems[2].Text.ToString())
                    {
                        case "Nepali Fiscal Year(eg. 067/068)":
                            DateFormat = "NEPALI_FISCAL_YEAR";
                            break;
                        case "English Fiscal Year (eg. 11/12)":
                            DateFormat = "ENGLISH_FISCAL_YEAR";
                            break;

                    }

                    if (lvItem.SubItems[1].Text.ToString() == "Date")
                    {

                        VoucherFormatDtl.Rows.Add(lvItem.SubItems[1].Text, DateFormat, lvItem.Text);

                    }
                    else
                    {
                        VoucherFormatDtl.Rows.Add(lvItem.SubItems[1].Text, lvItem.SubItems[2].Text, lvItem.Text);
                    }
                }

                //Call the interface function to add the text in the parent form container

                m_ParentForm.AddNumberingFormat(VoucherFormatDtl);

                this.Dispose();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            int count = 0;

            if (lvNum.SelectedItems[0].SubItems[2].Text == "(Auto)")
            {
                for (int i = 0; i < lvNum.Items.Count; i++)
                {
                    ListViewItem Item = lvNum.Items[i];

                    if (Item.SubItems[2].Text.ToString() == "(Auto)")
                        count++;

                } // end for loop



                if (count <= 1)
                {
                    MessageBox.Show("Sorry you are not allowed to delete this one!,There should be atleast one auto number in this field!");
                    return;

                }


            }// end if auto




            //Now proceed for deletion
            if (lvNum.SelectedItems.Count > 0)
            {

                lvNum.Items[lvNum.SelectedItems[0].Index].Remove();
            }

            else
            {
                MessageBox.Show("Please select at least one item in the list");
            }


            Sample();





        }

        private void btnUp_Click(object sender, EventArgs e)
        {


            if (lvNum.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one item in the list");
                return;
            }
            try
            {
                if (lvNum.SelectedItems.Count > 0)
                {
                    ListViewItem selected = lvNum.SelectedItems[0];
                    int indx = selected.Index;
                    int totl = lvNum.Items.Count;

                    if (indx == 0)
                    {
                        lvNum.Items.Remove(selected);
                        lvNum.Items.Insert(totl - 1, selected);
                    }
                    else
                    {
                        lvNum.Items.Remove(selected);
                        lvNum.Items.Insert(indx - 1, selected);
                    }
                }
                else
                {
                    MessageBox.Show("You can only move one item at a time. Please select only one item and try again.",
                        "Item Select", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }





            Sample();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvNum.SelectedItems.Count > 0)
                {
                    ListViewItem selected = lvNum.SelectedItems[0];
                    int indx = selected.Index;
                    int totl = lvNum.Items.Count;

                    if (indx == totl - 1)
                    {
                        lvNum.Items.Remove(selected);
                        lvNum.Items.Insert(0, selected);
                    }
                    else
                    {
                        lvNum.Items.Remove(selected);
                        lvNum.Items.Insert(indx + 1, selected);
                    }
                }
                else
                {
                    MessageBox.Show("You can only move one item at a time. Please select only one item and try again.",
                        "Item Select", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Sample();



        }

        private void txtSymbol_TextChanged(object sender, EventArgs e)
        {
            if (txtSymbol.Text.Length == 0)
                btnSymbol.Enabled = false;
            else
                btnSymbol.Enabled = true;

        }

        private void frmVoucherFormat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
      
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
