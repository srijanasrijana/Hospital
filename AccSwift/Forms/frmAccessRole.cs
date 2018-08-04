using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Collections;

namespace AccSwift
{

    public partial class frmAccessRole : Form
    {

        private EntryMode m_mode = EntryMode.NORMAL;
        List<int> _accessid = new List<int>();

        public frmAccessRole()
        {
            InitializeComponent();
        }

        private void frmAccessRole_Load(object sender, EventArgs e)
        {
            txtAccessRoleID.Clear();
            //Load Existing Access Roles
            LoadAccessRoles(lstAccessRoles);

            //Load Role Details in treeview
            treeAccessRole.Nodes.Clear();
            ShowAccessLevelInTreeView(treeAccessRole, null, 0);

            cboCopyAccessRole.Text = "";
            ChangeState(EntryMode.NORMAL);
        }

        private bool ValidateAccessRole()
        {
            bool m_Validate = true;
            FormHandle frmHandle = new FormHandle();
            frmHandle.AddValidate(txtRoleName, DType.NAME, "Invalid Access Role Name");
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

        private void EnableControls(bool Enable)
        {
            txtRoleName.Enabled = Enable;
            cboCopyAccessRole.Enabled = Enable;
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
               //if (Access_ID == null)
              //  Global.Msg("here is 179");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                TreeNode t = new TreeNode(dr[LangField].ToString());
                t.Tag = dr["AccessID"].ToString();




                ShowAccessLevelInTreeView(tv, t, Convert.ToInt16(dr["AccessID"].ToString()));

                if (n == null)

                    tv.Nodes.Add(t); //Primary Group

                else

                    n.Nodes.Add(t); //Secondary Group

            }


        }

        //Loads the Existing access roles in the listbox and copy list items combobox
        private void LoadAccessRoles(ListBox lstAccessRoles)
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

            lstAccessRoles.Items.Clear();
            cboCopyAccessRole.Items.Clear();
            DataTable dtAccessRoles = User.GetAcessRoleInfo(0);

            //Load blank in Access Role Template
            cboCopyAccessRole.Items.Add("");
            foreach (DataRow drAccessRoles in dtAccessRoles.Rows)
            {
                lstAccessRoles.Items.Add(new ListItem(Convert.ToInt32(drAccessRoles["RoleID"]), drAccessRoles[LangField].ToString()));
                cboCopyAccessRole.Items.Add(new ListItem(Convert.ToInt32(drAccessRoles["RoleID"]), drAccessRoles[LangField].ToString()));
            }

        }

        
        /// <summary>
        /// Recursive function to load the access role in the tree view and also check the child node if it is selected
        /// </summary>
        /// <param name="AccessRoleID"></param>
        /// <param name="tn"></param>
        /// <param name="CheckedIDs"></param>
        /// <param name="tvAccess"></param>
        private void LoadRolesInfo(int AccessRoleID, TreeNode tn, int[] CheckedIDs, TreeView tvAccess)
        {
            //First check if the treenode is checked
            foreach (int id in CheckedIDs)
            {
                if (Convert.ToInt32(tn.Tag) == id)
                    tn.Checked = true;
            }

            foreach (TreeNode nd in tn.Nodes)
            {
                nd.Checked = false; //first clear the checkmark if anything is checked previously
                LoadRolesInfo(AccessRoleID, nd, CheckedIDs, tvAccess);

                foreach (int id in CheckedIDs)
                    if (Convert.ToInt32(nd.Tag) == id)
                        nd.Checked = true;
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            
            txtRoleName.Clear();

            ChangeState(EntryMode.NEW);

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
           
            if (txtAccessRoleID.Text.Trim() == "")
            {
                //If access role hasnt been selected.
                Global.Msg("Please select an access role first!");
                return;
            }

            //Now dont allow if it is System Administrator
            if (((ListItem)(lstAccessRoles.SelectedItem)).ID == 37)//System Administrator
            {
                Global.Msg("The System Administrator Access Role cannot be edited. Please edit other access role, or create a new one.");
                return;
            }

            ChangeState(EntryMode.EDIT);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cboCopyAccessRole.Text = "";

            ChangeState(EntryMode.NORMAL);
        }

        
        private void ListTreeNodes(TreeView tv, TreeNode tn, List<TreeNode> tnReturn)
        {

            foreach (TreeNode nd in tn.Nodes)
                ListTreeNodes(tv, nd, tnReturn);


            tnReturn.Add(tn);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Check Validation
            if (!ValidateAccessRole())
                return;
  
            ArrayList arrNode = treeAccessRole.GetCheckedNodes(true);
            foreach (string tag in arrNode)
            {
                _accessid.Add(Convert.ToInt32(tag));
            }
            //if Accesrole doesnot select access then dont allow to save it
            if (_accessid.Count == 0)
            {
                Global.Msg("Please Select required Access");
                treeAccessRole.Focus();
                return;
            }
            switch (m_mode)
            {
                case EntryMode.NEW:

          
                    try
                    {
                        User.AddAccessRole(txtRoleName.Text, _accessid.ToArray());
                        Global.Msg("Access Role Created Successfully!");
                        //Reload all access roles
                        frmAccessRole_Load(sender, e);


                        ChangeState(EntryMode.NORMAL);
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;

                case EntryMode.EDIT:
                    try
                    {
                        User.ModifyAccessRole(txtRoleName.Text, Convert.ToInt32(txtAccessRoleID.Text), _accessid.ToArray<int>());
                        Global.Msg("Access Role Modified Successfully!");
                        ChangeState(EntryMode.NORMAL);

                        //Reload all access roles
                        frmAccessRole_Load(sender, e);
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError("Error while saving Access Role - " + ex.Message);
                    }
                    break;

            }

        }

        private void lstAccessRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem liItem = (ListItem)lstAccessRoles.SelectedItem;
                txtAccessRoleID.Text = liItem.ID.ToString();
                txtRoleName.Text = liItem.Value;

                LoadAccessRolesInTree(liItem.ID);

                cboCopyAccessRole.Text = "";
                ChangeState(EntryMode.NORMAL);
            }
            catch
            { //Do Nothing
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (txtAccessRoleID.Text != "")
            {
                //First check this AccessRole is being used by user or not???if yes then give message first delete the user then only permit it
                DataTable chkAccessRoleUsedByUser = User.CheckAccessRoleUsedByUser(txtAccessRoleID.Text);
                if (chkAccessRoleUsedByUser.Rows.Count>0)
                {
                    Global.Msg("This Access Role is being used by User. To delete the access role, you must first unassign it with the user which has been using the role and try again!");
                    return;
                }


                if (Global.MsgQuest("Are you sure you want to delete the Access Role - " + txtRoleName.Text.Trim() + "?") == DialogResult.Yes)
                {
                    User mUser = new User();
                    mUser.DeleteAccessRole(txtAccessRoleID.Text);
                    Global.Msg("Access Role - " + txtRoleName.Text.Trim() + " deleted successfully!");


                    //Reload everything
                    frmAccessRole_Load(sender, e);
                }
            }
            else
            {

                Global.Msg("First select the Access Role from List and try to delete!");
            }
        }

        private void frmAccessRole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void cboCopyAccessRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem liItem = (ListItem)cboCopyAccessRole.SelectedItem;
                
                LoadAccessRolesInTree(liItem.ID);

            }
            catch
            { //Do Nothing
            }
        }


        /// <summary>
        /// Loads the Access Roles with checkmark for the given AccessRoleID
        /// </summary>
        /// <param name="AccessRoleID"></param>
        /// <returns></returns>
        private void LoadAccessRolesInTree(int AccessRoleID)
        {

            try
            {
                DataTable dtAccessRoles = User.GetAccessRoleDetails(AccessRoleID);

                List<int> AccessIDs = new List<int>();
                foreach (DataRow dr in dtAccessRoles.Rows)
                    AccessIDs.Add(Convert.ToInt32(dr["AccessID"]));



                //expand the treeview
                treeAccessRole.ExpandAll();
                //treeAccessRole.LabelEdit = false;
                //treeAccessRole.Enabled = false;
                //Check for the treeview if it has access roles
                foreach (TreeNode tn in treeAccessRole.Nodes)
                {

                    //Search the array to be checked, if the treenode itself is found to be checked, checkmark it
                    //int result = AccessIDs.Find(x => x == (Convert.ToInt32(tn.Tag)));
                    //if (result > 0)
                    //    tn.Checked = true;

                    //Load child nodes using recursive functions
                    LoadRolesInfo(AccessRoleID, tn, AccessIDs.ToArray<int>(), treeAccessRole);
                }

                
            }
            catch
            {
                throw;
            }
        }

    }
}
