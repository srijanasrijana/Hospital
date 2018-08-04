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
    public partial class frmuserenabledisable : Form
    {
        public frmuserenabledisable()
        {
            InitializeComponent();
        }

        private void frmuserenabledisable_Load(object sender, EventArgs e)
        {
            lvUser.Columns.Add("User Name", 70);
            lvUser.Columns.Add("Name", 100);
            lvUser.Columns.Add("Role", 60);
            lvUser.Columns.Add("Department");

            lvUser.FullRowSelect = true;
            ListUserInfo();
        }

        private void ListUserInfo()
        {
            #region Language Management
            string LangField = "EngName";
            if (LangMgr.DefaultLanguage == Lang.English)
                LangField = "EngName";
            else if (LangMgr.DefaultLanguage == Lang.Nepali)
                LangField = "NepName";
            #endregion
            try
            {
                DataTable dtUser = User.GetUserInfo(-1);
                foreach (DataRow drUser in dtUser.Rows)
                {
                    ListViewItem lvItem = new ListViewItem(drUser["UserName"].ToString());
                    lvItem.SubItems.Add(drUser["Name"].ToString());
                    DataTable dtAccessRole = User.GetAcessRoleInfo(Convert.ToInt32(drUser["AccessRoleID"]));
                    DataRow drAccessRole = dtAccessRole.Rows[0];

                    lvItem.SubItems.Add(drAccessRole[LangField].ToString());

                    lvItem.SubItems.Add(drUser["Department"].ToString());
                    lvItem.Tag = drUser["UserID"];
                    lvUser.Items.Add(lvItem);
                }

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void lvUser_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            #region Language Management



            string LangField = "EngName";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;

            }
            #endregion


            try
            {
                if (lvUser.SelectedItems.Count > 0)
                    txtUserID.Text = ((ListView)sender).SelectedItems[0].Tag.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnenable_Click(object sender, EventArgs e)
        {
            try
            {
                User u = new User();
                if (lvUser.SelectedItems.Count > 0)
                {
                    u.UserStatusEnable(Convert.ToInt32(txtUserID.Text));
                    MessageBox.Show("User Enabled Successfully");
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btndisable_Click(object sender, EventArgs e)
        {
            try
            {
                User u = new User();
                if (lvUser.SelectedItems.Count > 0)
                {
                    u.UserStatusDisable(Convert.ToInt32(txtUserID.Text));
                    MessageBox.Show("User Disabled Successfully");
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
    }
}
