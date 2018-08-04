using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SmartSolutions.Controls;
using BusinessLogic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;


namespace AccSwift
{
    public partial class frmLoginCreate : Form
    {
        private EntryMode m_mode = EntryMode.NORMAL;
        ListItem liAccessRoleID = new ListItem();
        private string Prefix = "";
        public frmLoginCreate()
        {
            InitializeComponent();
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
                DataTable dtUser = User.GetUserInfo(0);
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

        private void frmLoginCreate_Load(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
            DataTable dt = User.GetAcessRoleInfo(0);
            foreach (DataRow dr in dt.Rows)
            {
                cboRole.Items.Add(new ListItem((int)dr["RoleID"], dr["EngName"].ToString()));//It adds LedgerID as well as LedgerName in combobox            
            }

            lvUser.Columns.Add("User Name", 70);
            lvUser.Columns.Add("Name", 100);
            lvUser.Columns.Add("Role", 60);
            lvUser.Columns.Add("Department");

            lvUser.FullRowSelect = true;
            LoadCombobox(cboAccountClass, 0);
            ListUserInfo();

        }

        private void LoadCombobox(ComboBox cboAccClass, int AccClassID)
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
            DataTable dt = AccountClass.GetAccClassTable(AccClassID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                cboAccClass.Items.Add(new ListItem((int)dr["AccClassID"], Prefix + " " + dr[LangField].ToString()));
                Prefix += "----";
                LoadCombobox(cboAccClass, Convert.ToInt16(dr["AccClassID"].ToString()));
            }
            //Prefix = "--";
            if (Prefix.Length > 1)
            {
                Prefix = Prefix.Remove(Prefix.Length - 4, 4);
            }
            cboAccClass.SelectedIndex = 0;
        }

        private bool ValidateAccessRole()
        {
            bool m_Validate = true;
            FormHandle frmHandle = new FormHandle();
            m_Validate = frmHandle.Validate();
            return m_Validate;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;

            switch (Mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false);
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    break;
            }
        }

        private void EnableControls(bool Enabled)
        {
            txtName.Enabled = txtAddress.Enabled = txtPhone.Enabled = txtEmail.Enabled = txtDepartment.Enabled = cboRole.Enabled = txtUserName.Enabled = txtPassword1.Enabled = txtPassword2.Enabled = cboAccountClass.Enabled=Enabled;
        }

        //Enables and disables the button states
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
        }

        //Recursive Function to Show Access Level in Treeview
        private void ShowAccessLevelInTreeView(TreeView tv, TreeNode n, int Access_ID)
        {

            #region Language Management

            tv.Font = LangMgr.GetFont();

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

            DataTable dt;

            dt = User.GetAccessInfo(Access_ID);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                TriStateTreeNode t = new TriStateTreeNode(dr[LangField].ToString());

                //Check if it is a parent Or if it has childs
                try
                {

                    if (ChildCount((int)dr["AccessID"]) > 0)
                    {
                        t.IsContainer = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                ShowAccessLevelInTreeView(tv, t, Convert.ToInt16(dr["AccessID"].ToString()));

                if (n == null)

                    tv.Nodes.Add(t); //Primary Group

                else

                    n.Nodes.Add(t); //Secondary Group

            }
        }

        private int ChildCount(int Access_ID)
        {
            try
            {
                int m_RecCount = (int)User.GetAccessInfo(Access_ID).Rows.Count;
                return m_RecCount;
            }
            catch (Exception ex)
            {
                throw;
                return 0;
            }
        }


        private bool Validate()
        {

            bool bValidate = false;
            FormHandle m_FHandle = new FormHandle();
            m_FHandle.AddValidate(txtName, DType.NAME, "Invalid  Name. Please Post a valid  Name and try again.");
            m_FHandle.AddValidate(txtUserName, DType.USERNAME, "Invalid User Name. Please post a valid User name and try again.");
            if(txtPassword1.Text.Trim().Length>1)
                m_FHandle.AddValidate(txtPassword1, DType.NAME, "Invalid Password. Please Post a valid  Password and try again.");
            bValidate = m_FHandle.Validate();
         

            if (!bValidate)
            {
                return false;
            }

            if (txtEmail.Text != "")
            {
                if (!isValidEmail(txtEmail.Text))
                {
                    Global.MsgError("Invalid Email Address!");
                    txtEmail.Focus();
                    return false;
                }
            }

            if (cboRole.Text =="")
            {
                Global.MsgError("Invalid Role!");
                cboRole.Focus();
                return false;

            }

            if(txtUserName.Text == "")
            {
                Global.MsgError("Please provide a Username.");
                txtUserName.Focus();
                return false;
            }

            if(txtPassword1.Text == "" || txtPassword2.Text == "")
            {
                Global.MsgError("Please provide the password.");
                return false;
            }
            else if (txtPassword1.Text.Trim() != txtPassword2.Text.Trim())
            {
                Global.MsgError("Password and Verify Password doesnot match!");
                txtPassword1.Focus();
                return false;

            }

            return bValidate;
        }

        /// <summary>
        /// this block of code for validating of email..
        /// must provide emain in the form of xx@xx.xx
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtDepartment.Text = "";
            cboRole.Text = "";
            cboAccountClass.SelectedIndex = 0;
            txtUserName.Text = "";
            txtPassword1.Text = "";
            txtPassword2.Text = "";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            ChangeState(EntryMode.NEW);
            ClearForm();
            txtUserID.Clear();
            txtName.Focus();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(txtUserID.Text))
            {
                ChangeState(EntryMode.EDIT);
                txtUserName.Enabled = false;//Username is not allowed to be modified.
            }
            else
                Global.Msg("Please select a user to modify");

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //if edit mode is on and fields are edited, cancel button loads the data again.
            ClearForm();
            if(m_mode == EntryMode.EDIT)
                LoadForm(Convert.ToInt32(txtUserID.Text));

            ChangeState(EntryMode.NORMAL);
        }


        private void lvUser_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
            try
            {
                if (lvUser.SelectedItems.Count > 0)
                    txtUserID.Text = ((ListView)sender).SelectedItems[0].Tag.ToString();

                LoadForm(Convert.ToInt32(txtUserID.Text));
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        /// <summary>
        /// Loads all the user info to the form according to userId.
        /// </summary>
        /// <param name="userID"></param>
        private void LoadForm(int userID)
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

            DataTable dtUserInfo = User.GetUserInfo(userID);
            DataRow drUserInfo = dtUserInfo.Rows[0];

            txtName.Text = drUserInfo["Name"].ToString();
            txtAddress.Text = drUserInfo["Address"].ToString();
            txtDepartment.Text = drUserInfo["Department"].ToString();
            txtEmail.Text = drUserInfo["Email"].ToString();
            txtPhone.Text = drUserInfo["Contact"].ToString();
            txtUserName.Text = drUserInfo["UserName"].ToString();
            txtPassword1.Text = txtPassword2.Text = "";
            cboRole.Text = User.GetAcessRoleInfo(Convert.ToInt32(drUserInfo["AccessRoleID"])).Rows[0][LangField].ToString();
            foreach (ListItem lst in cboAccountClass.Items)
            {
                if (lst.Value.Replace('-', ' ').Trim() == AccountClass.GetNameFromID(Convert.ToInt32(drUserInfo["AccClassID"])))
                {
                    cboAccountClass.Text = lst.Value;
                    break;
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validate()) 
            {
                return;
            }
            liAccessRoleID = (ListItem)cboRole.SelectedItem;
            ListItem liAccClassID = new ListItem();
            liAccClassID = (ListItem)cboAccountClass.SelectedItem;
            switch (m_mode)
            {                   
                case EntryMode.NEW:
                    try
                    {
                        User user = new User();
                        user.save(txtUserName.Text,txtPassword1.Text,txtName.Text,txtAddress.Text,txtPhone.Text,txtEmail.Text,txtDepartment.Text,Convert.ToInt32(liAccessRoleID.ID),liAccClassID.ID,User.CurrentUserName);
                        Global.Msg("User Created Successfully!");
                        lvUser.Items.Clear();
                        ListUserInfo();
                        ChangeState(EntryMode.NORMAL);
                        txtUserName.Focus();
                    }
                    catch (Exception ex)
                    {
                        if(ex.Message.Contains("UNIQUE KEY"))
                        {
                            MessageBox.Show("User name must be unique.");
                        }
                        else
                            MessageBox.Show(ex.Message);
                    }
                    break;

                case EntryMode.EDIT:
                    DialogResult dlgResult = MessageBox.Show("Do you want to modify the user?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlgResult == DialogResult.Yes)
                    {
                        try
                        {
                            int UserID = Convert.ToInt32(txtUserID.Text);
                            User user = new User();
                            user.edit(txtUserName.Text, txtPassword1.Text.Trim(), UserID, txtName.Text, txtAddress.Text, txtPhone.Text, txtEmail.Text, txtDepartment.Text, Convert.ToInt32(liAccessRoleID.ID),liAccClassID.ID,User.CurrentUserName);
                            MessageBox.Show("User Modified Successfully!","Message",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                            ChangeState(EntryMode.NORMAL);
                            lvUser.Items.Clear();
                            ListUserInfo();
                            //ClearForm();
                        }
                    
                        catch(Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
            ChangeState(EntryMode.NORMAL);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUserID.Text))
            {
                if (txtUserID.Text.Trim() == "1")//User 'root' cannot be deleted.
                {
                    Global.Msg("Sorry! 'root' can not be deleted.");
                    return;
                }

                if (Convert.ToInt32(txtUserID.Text.Trim()) == User.CurrUserID)//A logged in user can not be deleted.
                {
                    Global.Msg("Sorry! can not delete logged in user.");
                    return;
                }

                DialogResult dlgResult = MessageBox.Show("Do you want to delete user permanently?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgResult == DialogResult.Yes)
                {
                    int userid = Convert.ToInt32(txtUserID.Text);
                    User user = new User();
                    user.delete(userid);
                    Global.Msg("User deleted sucessfully...");
                    lvUser.Items.Clear();
                    ListUserInfo();
                    ChangeState(EntryMode.NORMAL);
                    ClearForm();
                }
                else
                {
                    return;
                }
            }
            else
                Global.Msg("Please select a user to Delete");
        }

        private void frmLoginCreate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void lvUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //private void ConfigureTreeView()
        //{
        //    TriStateTreeNode rootNode = new TriStateTreeNode("Home - \"CheckboxVisible = false\".");
        //    rootNode.CheckboxVisible = false;
        //    rootNode.IsContainer = true;

        //    for (int i = 0; i < 4; i++)
        //    {
        //        TriStateTreeNode folderNode = new TriStateTreeNode(string.Format("Foldernode {0}, can show 3 states, as shown here.", i), 0, 1);
        //        folderNode.IsContainer = true;
        //        rootNode.Nodes.Add(folderNode);
        //    }

        //    TriStateTreeNode firstFolder = rootNode.FirstNode as TriStateTreeNode;
        //    for (int i = 0; i < 2; i++)
        //    {
        //        TriStateTreeNode itemNode = new TriStateTreeNode(string.Format("Item node {0}", i), 2, 2);
        //        firstFolder.Nodes.Add(itemNode);
        //    }

        //    TriStateTreeNode secondFolder = firstFolder.NextNode as TriStateTreeNode;
        //    for (int i = 0; i < 2; i++)
        //    {
        //        TriStateTreeNode itemNode = new TriStateTreeNode(string.Format("Item node {0}", i), 2, 2);
        //        secondFolder.Nodes.Add(itemNode);
        //    }

        //    TriStateTreeNode thirdFolder = secondFolder.NextNode as TriStateTreeNode;
        //    for (int i = 0; i < 2; i++)
        //    {
        //        TriStateTreeNode itemNode = new TriStateTreeNode(string.Format("Item node {0}", i), 2, 2);
        //        thirdFolder.Nodes.Add(itemNode);
        //    }

        //    TriStateTreeNode fourthFolder = thirdFolder.NextNode as TriStateTreeNode;
        //    fourthFolder.CheckboxVisible = false;
        //    for (int i = 0; i < 2; i++)
        //    {
        //        TriStateTreeNode itemNode = new TriStateTreeNode(string.Format("Item node {0}", i), 2, 2);
        //        itemNode.CheckboxVisible = false;
        //        fourthFolder.Nodes.Add(itemNode);
        //    }

        //    this.treeAccessLevel.SuspendLayout();
        //    this.treeAccessLevel.Nodes.Add(rootNode);
        //    this.treeAccessLevel.ResumeLayout();

        //    secondFolder.FirstNode.Checked = true;
        //    thirdFolder.Checked = true;
        //}

    }
}