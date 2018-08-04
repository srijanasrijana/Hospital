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

namespace AccSwift
{
    public partial class frmCompanyInfo : Form
    {

        private byte[] imgLogo=null;
        public Boolean companyExist = false;

        public frmCompanyInfo()
        {
            InitializeComponent();
        }

        public frmCompanyInfo(Boolean exist)
        {
            InitializeComponent();
            companyExist = exist;
        }

        //Enable or disable controls for edit button
        private void EnableControls(bool enable)
        {
            txtCompanyName.Enabled = txtAddress1.Enabled = txtAddress2.Enabled = txtCity.Enabled = txtDistrict.Enabled = txtZone.Enabled = txtTel.Enabled = txtEmail.Enabled = txtWebsite.Enabled
                = txtPOBox.Enabled = txtPan.Enabled = txtDateBookBegin.Enabled = txtDateFY.Enabled = txtfiscalyearstyle.Enabled = btnBrowse.Enabled = btnSave.Enabled = enable;
        }
        private bool Validate()
        {
            FormHandle m_FHandle = new FormHandle();           
            m_FHandle.AddValidate(txtCompanyName, DType.NAME, "Invalid Company Name!");
            return m_FHandle.Validate();        
        }



        private void btnBrowse_Click(object sender, EventArgs e)
        {            
            OpenFileDialog openIMG = new OpenFileDialog();
            openIMG.Filter = "Known graphics format (*.bmp,*.jpg,*.gif,*.png)|*.bmp;*.jpg;*.gif;*.jpeg;*.png";
            openIMG.ShowDialog();
            string imgPath = openIMG.FileName;
            try
            {
                picLogo.Image = Image.FromFile(imgPath);
                imgLogo = Misc.ReadBitmap2ByteArray(imgPath.ToString());
            }
            catch (Exception ex)
            {
                //Probably cancelled the selected
            }

            
        }

        private void LoadData()
        {
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picLogo.BorderStyle = BorderStyle.FixedSingle;
            //check if company exist or not
            //if not then only display few contents

            //txtDateFY.Mask = Date.FormatToMask();
            //txtDateFY.Text = Date.ToSystem(Date.GetServerDate());
            //txtDateBookBegin.Mask = Date.FormatToMask();
            //txtDateBookBegin.Text = Date.ToSystem(Date.GetServerDate());

            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();
            if (CompDetails == null)
            {
                return;
            }
            txtCompanyName.Text = CompDetails.CompanyName;
            // txtCompanyName.Text = CompDetails.CompanyName;
            txtAddress1.Text = CompDetails.Address1;
            txtAddress2.Text = CompDetails.Address2;
            txtCity.Text = CompDetails.City;
            txtDistrict.Text = CompDetails.District;
            txtEmail.Text = CompDetails.Email;
            txtPan.Text = CompDetails.PAN;
            txtPOBox.Text = CompDetails.POBox;
            txtTel.Text = CompDetails.Telephone;
            txtWebsite.Text = CompDetails.Website;
            txtZone.Text = CompDetails.Zone;
            //txtDateFY.Mask = Date.FormatToMask();
            txtDateFY.Text = Date.ToSystem(CompDetails.FYFrom);
            //txtDateBookBegin.Mask = Date.FormatToMask();
            txtDateBookBegin.Text = Date.ToSystem(CompDetails.BookBeginFrom);
            picLogo.Image = Misc.GetImageFromByte(CompDetails.Logo);
            txtfiscalyearstyle.Text = CompDetails.FiscalYear;

            EnableControls(false);
        }
        private void frmCompanyInfo_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            
            
            //if (!Validate())
            //    return;

            //Checks if there exists a company
            DataTable dtCompanyInfo = CompanyDetails.GetCompanyInfo();
            //DataRow drCompanyInfo = dtCompanyInfo.Rows[0];           
            try
            {
                CompanyDetails CompDetails = new CompanyDetails();
                CompDetails.CompanyName = txtCompanyName.Text;
                CompDetails.Address1 = txtAddress1.Text;
                CompDetails.Address2 = txtAddress2.Text;
                CompDetails.City = txtCity.Text;
                CompDetails.District = txtDistrict.Text;
                CompDetails.Email = txtEmail.Text;
                CompDetails.PAN = txtPan.Text;
                CompDetails.POBox = txtPOBox.Text;
                CompDetails.Telephone = txtTel.Text;
                CompDetails.Website = txtWebsite.Text;
                CompDetails.Zone = txtZone.Text;
                CompDetails.FYFrom =Date.ToDotNet(txtDateFY.Text);
                CompDetails.BookBeginFrom =Date.ToDotNet(txtDateBookBegin.Text);
                CompDetails.FiscalYear = txtfiscalyearstyle.Text;
                CompDetails.DBName = txtDBName.Text;
                if(imgLogo!=null)
                    CompDetails.Logo = imgLogo;
                string Return="";
                if (dtCompanyInfo.Rows.Count != 0)
                {


                    bool chkUserPermission = UserPermission.ChkUserPermission("COMPANY_CREATE");
                    if (chkUserPermission == false)
                    {
                        Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                        return;
                    }
                    CompanyInfo CompInfo = new CompanyInfo();
                    Return = CompInfo.Update(CompDetails);
                }
                else
                {
                    bool chkUserPermission = UserPermission.ChkUserPermission("COMPANY_MODIFY");
                    if (chkUserPermission == false)
                    {
                        Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                        return;
                    }

                    CompanyInfo CompInfo = new CompanyInfo();
                     Return = CompInfo.Insert(CompDetails); 
                }
                if(Return=="SUCCESS")
                    Global.Msg("Information Updated Successfully!");
                else
                    Global.MsgError("Some Unknown Error Occured!");

                this.Dispose();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            if (btnSave.Enabled == true)
            {
                LoadData();
                btnEdit.Enabled = true;
            }
            else
            {
                this.Dispose();
            }
        }

        private void txtCompanyName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtAddress1.Focus();
            }
        }

        private void txtAddress1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtAddress2.Focus();
            }
        }

        private void txtAddress2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtCity.Focus();
            }
        }

        private void txtCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDistrict.Focus();
            }
        }

        private void txtDistrict_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtZone.Focus();
            }
        }

        private void txtZone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtTel.Focus();
            }
        }

        private void txtTel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtEmail.Focus();
            }
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtWebsite.Focus();
            }
        }

        private void txtWebsite_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtPOBox.Focus();
            }
        }

        private void txtPOBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtPan.Focus();
            }
        }

        private void txtPan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDateFY.Focus();
            }
        }

        private void btnBrowse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnCreateUser.Focus();
            }
        }

        private void btnAdvance_Click(object sender, EventArgs e)
        {
            if (Global.MsgQuest("Do you want to use default company setting?") == DialogResult.No)
            {
                frmSettings frm = new frmSettings();
                frm.ShowDialog();
            }
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            
            if (Global.GlobalAccessRoleID != 37)
            {   
                Global.MsgError("Sorry! only admin has the permission to create new user. Please contact your administrator for permission.");
                return;
            }
            if (Global.MsgQuest("Do you want to create new user?") == DialogResult.Yes)
            {
                frmLoginCreate lc = new frmLoginCreate();
                lc.ShowDialog();
            }
            else
            {
                Global.Msg("Default user root is set.");
            }
        }

        private void btnCreateUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnAdvance.Focus();
            }
        }

        private void txtDateFY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDateBookBegin.Focus();
            }
        }

        private void txtDateBookBegin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnBrowse.Focus();
            }
        }

        private void txtCompanyName_Leave(object sender, EventArgs e)
        {
            txtDBName.Text = txtCompanyName.Text + 21;
        }

        private void frmCompanyInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EnableControls(true);
            btnEdit.Enabled = false;
        }
    }
}
