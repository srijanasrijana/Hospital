using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace AccSwift.Forms
{
    public partial class frmchangepassword : Form
    {
        public int userid;
        public int check;
        Control _lastEnteredControl;
        public frmchangepassword(int id)
        {
            userid = id;
            InitializeComponent();
        }
        //public frmchangepassword(int id)
        //{
        //    userid = id;
        //}

        private void frmchangepassword_Load(object sender, EventArgs e)
        {
            DataTable dtUserinfo = User.GetUserInfo(userid);
            if (dtUserinfo.Rows.Count > 0)
            {
                txtUserName.Text = dtUserinfo.Rows[0]["UserName"].ToString();
            }
            txtPassword1.Focus();
           // _lastEnteredControl.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //check = userid;  
                    DialogResult dlgResult = MessageBox.Show("Do you want to modify the user?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlgResult == DialogResult.Yes)
                    {
                        if (txtPassword1.Text != txtPassword2.Text)
                        {
                            DialogResult dlgResult1 = MessageBox.Show("Incorrect Password and Retyped Password", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Question);
                            if (dlgResult1 == DialogResult.OK)
                            {
                                _lastEnteredControl.Focus();
                            }

                        }
                        else
                        {
                            try
                            {
                                User user = new User();
                                user.changepassword(userid, txtPassword1.Text);
                                MessageBox.Show("User Modified Successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                this.Close();
                            }

                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
            //ChangeState(EntryMode.NORMAL);
        }

        private void txtPassword1_Enter(object sender, EventArgs e)
        {
            _lastEnteredControl = (Control)sender;
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
