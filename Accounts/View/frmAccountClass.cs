using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BusinessLogic;
using Common;


namespace Accounts
{
    public interface IfrmAddAccClass
    {
        void AddAccClass();
    }
    public partial class frmAccountClass : Form
    {
        private bool IsUsingInterface = false;
        string Prefix = "";
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        private IfrmAddAccClass m_ParentForm;
        private string OldAccClass = " ";
        private string NewAccClass = " ";
        private bool isNew;
        private IMDIMainForm m_MDIForm;

        public frmAccountClass(IMDIMainForm frm)
        {
            InitializeComponent();
            m_MDIForm = frm;
        }

        public frmAccountClass()
        {
            InitializeComponent();
        }
        public frmAccountClass(Form ParentForm)
        {
            InitializeComponent();
            IsUsingInterface = true;
            m_ParentForm = (IfrmAddAccClass)ParentForm;
        }
        
        private void frmAccountClass_Load(object sender, EventArgs e)
        {
            FillAccountClassTreeView(treeAccountClass, null, 0);
            treeAccountClass.ExpandAll();
            //FillAccClassCombo(cboParentClass);
            cboParentClass.Items.Clear();
            cboParentClass.Items.Add(new ListItem(0, "======= NO PARENT ======="));
            LoadCombobox(cboParentClass, 0);
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
        
        }
        //Validate Form inputs
        private bool Validate()
        {
            FormHandle m_FHandle = new FormHandle();
            m_FHandle.AddValidate(txtClassName, DType.NAME, "Invalid Class Name. Please choose a valid Class Name");
            return m_FHandle.Validate();
        }
        //Recursive Function to Show Account Group in Treeview
        public void FillAccountClassTreeView(TreeView tv, TreeNode n, int AccClassID)
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

            DataTable dt=new DataTable();
            try
            {
                dt = AccountClass.GetAccClassTable(AccClassID);
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                TreeNode t = new TreeNode(dr[LangField].ToString(), 0, 0);

                FillAccountClassTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));

                if (n == null)

                    tv.Nodes.Add(t); //Primary Group

                else
                {
                    n.Nodes.Add(t); //Secondary Group

                }
            }
        }

        private void treeAccountClass_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearForm();
            try
            {
                //Show the selected value in respective fields
                
                    txtClassName.Text = e.Node.Text;

                    DataTable dt=AccountClass.GetAccClassTable(e.Node.Text);

                    //Fill the ledger form 
                    DataRow dr=dt.Rows[0];
                    txtID.Text = dr["AccClassID"].ToString();
                    txtClassName.Text=dr["EngName"].ToString();
                    if (dr["ParentID"].ToString() == "")
                        cboParentClass.Text = "[No Parent]";
                    else
                    {
                        foreach (ListItem lst in cboParentClass.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["ParentID"]))
                            {
                                cboParentClass.Text = lst.Value;
                                break;
                            }
                        }
                    }
                //cboParentClass.Text=AccountClass.GetNameFromID((int)dr["ParentID"]);
                    txtRemarks.Text=dr["Remarks"].ToString();
                    //if ((bool)dr["Compulsory"] == true)
                    //    chkCompulsory.Checked = true;
                    //else
                    //    chkCompulsory.Checked = false;
                    treeAccountClass.Focus();
                
                ChangeState(EntryMode.NORMAL);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                //throw;
            }
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;

            switch (m_mode)
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
            txtClassName.Enabled= cboParentClass.Enabled = txtRemarks.Enabled = Enable;
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
        }

        private void ClearForm()
        {
            txtClassName.Clear();
            cboParentClass.Text = "";
            txtRemarks.Clear();            
        }


        private void FillAccClassCombo(ComboBox cboAccClass)
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

            cboAccClass.Items.Clear();
            DataTable dtParent = AccountClass.GetAccClassTable(0);
            for (int ii = 0; ii < dtParent.Rows.Count; ii++)
            {
                DataTable dt = new DataTable();
                string Prefix = "";
                DataRow dr = dtParent.Rows[ii];
                if (dr["ParentID"].ToString() == "")
                    cboAccClass.Items.Add(new ListItem((int)dr["AccClassID"], dr[LangField].ToString()));
                dt = AccountClass.GetAccClassTable(Convert.ToInt32(dr["AccClassID"]));
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Prefix = ReturnLevel(Convert.ToInt32((dr["AccClassID"])));
                        cboAccClass.Items.Add(new ListItem((int)dr["AccClassID"], Prefix + dr[LangField].ToString()));
                    }
                }
            }
            //DataTable dt = AccountClass.GetAccClassTable(-1);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{ 
            //    DataRow dr = dt.Rows[i];
            //    //MessageBox.Show(dr["ParentID"].ToString());


            //    string Prefix = CheckParentID(dr["AccClassID"].ToString());               

            //    cboAccClass.Items.Add(new ListItem((int)dr["AccClassID"], Prefix + dr[LangField].ToString()));

            //    //if(dr["ParentID"] == DBNull.Value)
            //    //cboAccClass.Items.Add(new ListItem((int)dr["AccClassID"],dr[LangField].ToString()));
            //    //else if(dr["ParentID"].ToString() == "1")
            //    //    cboAccClass.Items.Add(new ListItem((int)dr["AccClassID"], "--" + dr[LangField].ToString()));
            //}
            cboAccClass.DisplayMember = "value";
            cboAccClass.ValueMember = "id";
                
        }

        private string ReturnLevel(int ParentID)
        {
            string Prefix = "";
            DataTable dt = new DataTable();
            dt = AccountClass.GetAccClassTable(ParentID);           
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr1 = dt.Rows[i];
                Prefix += "--";

              //  cboAccClass.Items.Add(new ListItem((int)dr1["AccClassID"], Prefix + dr1[LangField].ToString()));
            }
            return Prefix;
        
        
        }

        private string CheckParentID(string ParentID)
        {
            string prefix = "";
           //bool parent = AccountClass.IsParent(ParentID);
           //if (ParentID == "" && parent == true)
           //    prefix = "";
           //else if (parent == true)
           //    return prefix;
           //else
           //{
           //    prefix += "--";
           //    CheckParentID(ParentID);
           //}
            return prefix;
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_CLASS_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearForm();
            ChangeState(EntryMode.NEW);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_CLASS_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }

            //Edit not allowed untill a node is selected in the treeAcountClass treeView
            if (string.IsNullOrEmpty(txtID.Text))
            {
                Global.MsgError("Please select a Acounting Class in the Tree View first.");
                return;
            }

            //check is the primary group class
            //DataTable dt = AccountClass.GetAccClassTable(Convert.ToInt32(txtID.Text));
            //if(dt.Rows.Count != 0)
            if (txtID.Text=="1")
            {
                Global.Msg("This is a built in Account Class and cannot be edited!");
                return;
            }
            isNew = false;
            OldAccClass = " ";
            OldAccClass = OldAccClass + "OldValues" + "ClassName" + txtClassName.Text + "Parent Class" + cboParentClass.Text;

            ChangeState(EntryMode.EDIT);     
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_CLASS_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_CLASS_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }
            
            //Check Validation
            if (!Validate())
                return;
            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    isNew = true;
                    NewAccClass = " ";
                    OldAccClass = " ";
                    NewAccClass = NewAccClass + "NewValues" + "ClassName" + txtClassName.Text + "Parent Class" + cboParentClass.Text;
                        try
                        {
                            ListItem liAccClassID = new ListItem();                                                        
                                liAccClassID = (ListItem)cboParentClass.SelectedItem;
                         
                            //Check if liAccClassID is valid
                                if (liAccClassID == null)
                                {
                                    throw new Exception("Parent Accounting Class is not valid. Please select the proper Parent Accounting Class!");
                                }
                            IAccountClass acClass = new AccountClass();                          
                                acClass.Create(txtClassName.Text.Trim(), liAccClassID.ID, txtRemarks.Text.Trim(),OldAccClass,NewAccClass,isNew);                          
                            Global.Msg("Account class created successfully!");
                            ChangeState(EntryMode.NORMAL);
                            //FillAccClassCombo(cboParentClass);
                            cboParentClass.Items.Clear();
                            cboParentClass.Items.Add(new ListItem(0, "======= NO PARENT ======="));
                            LoadCombobox(cboParentClass, 0);
                        }
                        catch (Exception ex)
                        {
                            Global.MsgError(ex.Message);
                        }
                        break;
                #endregion

                #region EDIT
                case EntryMode.EDIT:
                    isNew = false;
                    NewAccClass = " ";
                    NewAccClass = NewAccClass + "NewValues" + "ClassName" + txtClassName.Text + "Parent Class" + cboParentClass.Text;
                    try
                    {   
                        //Get the selected parent class
                        ListItem liAccClassID = new ListItem(); 
                        liAccClassID = (ListItem)cboParentClass.SelectedItem;

                        //check the accClassID of cboparentclass
                        //if same then error
                        if (Convert.ToInt32(txtID.Text) == liAccClassID.ID)
                        {
                            Global.Msg("The parent account class name selected could not be itself.");
                            return;
                        }


                        //if (liAccClassID == null)
                        //{
                        //    liAccClassID.ID = 0;
                        //}
                       //If ID of cboParentID is child of txtid.text then do not save

                        ////////Check whether the modified parent  is not under itself
                        ArrayList ReturnIDs = new ArrayList();
                       AccountClass.GetChildIDs(Convert.ToInt32(txtID.Text.Trim()), ref ReturnIDs);

                       foreach (int id in ReturnIDs)
                       {
                           if (liAccClassID.ID == id)
                           {
                               Global.Msg("The parent account class name selected is the child of its own");
                               return;
                           }
                       }

                        //ReturnIDs.Insert(0, liAccClassID.ID);//Add to itself so that it cannot be created under itself
                        //if (ReturnIDs.BinarySearch(liAccClassID.ID) > 0) //if found returns greater index than 0
                        //{
                        //    Global.Msg("The parent account class name selected is the child of its own");
                        //    return;
                        //}

                        IAccountClass acClass = new AccountClass();                      
                            acClass.Modify(Convert.ToInt32(txtID.Text), txtClassName.Text.Trim(), Convert.ToInt32(liAccClassID.ID), txtRemarks.Text.Trim(),OldAccClass,NewAccClass,isNew);
                        Global.Msg("Account class modified successfully!");
                        ChangeState(EntryMode.NORMAL);

                    }
                   catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

            }//end switch(m_mode)

            treeAccountClass.Nodes.Clear();
            FillAccountClassTreeView(treeAccountClass, null, 0);
            //FillAccClassCombo(cboParentClass);
            cboParentClass.Items.Clear();
            cboParentClass.Items.Add(new ListItem(0, "======= NO PARENT ======="));
            LoadCombobox(cboParentClass, 0);

            if (IsUsingInterface)
            {
                m_ParentForm.AddAccClass();
                IsUsingInterface = false;
                this.Close();
            }
           
        }    

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("ACCOUNT_CLASS_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }

            //Delete not allowed untill a node is selected in the treeAcountClass treeView
            if (string.IsNullOrEmpty(txtID.Text))
            {
                Global.MsgError("Please select a Acounting Class in the Tree View first.");
                return;
            }

            if (txtID.Text == "1")
            {
                Global.Msg("This is a built in Account Class and cannot be deleted!");
                return;
            }

            //Check if the AccClass has any child accClass
            //Don't allow user to delete accClass if child accClass is present.
            DataTable dt = AccountClass.GetAccClassTable(Convert.ToInt32(txtID.Text));
            if (dt.Rows.Count > 0)
            {
                Global.MsgError("Sorry! you can not delete a Accounting Class that has child Accounting Class. First delete the child Accounting Class.");
                return;
            }

            //Ask if the account head is to be deleted
            if (Global.MsgQuest("Are you sure you want to delete the Account Class - " + txtClassName.Text + "?") == DialogResult.No)
                return;

            //If the user confirms deletion
            try
            {
                IAccountClass acClass = new AccountClass();
                acClass.Delete(Convert.ToInt32(txtID.Text));
                Global.Msg("Account Class - " + txtClassName.Text + " deleted successfully!");


                ChangeState(EntryMode.NORMAL);
                treeAccountClass.Nodes.Clear();
                FillAccountClassTreeView(treeAccountClass, null, 0);
                ClearForm();

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            if (isNew == false)
            {
                try
                {
                    DataTable dt = AccountClass.GetAccClassTableByID(Convert.ToInt32(txtID.Text));

                    //Fill the ledger form 
                    DataRow dr = dt.Rows[0];
                    txtID.Text = dr["AccClassID"].ToString();
                    txtClassName.Text = dr["EngName"].ToString();
                    if (dr["ParentID"].ToString() == "")
                        cboParentClass.Text = "[No Parent]";
                    else
                    {
                        foreach (ListItem lst in cboParentClass.Items)
                        {
                            if (lst.ID == Convert.ToInt32(dr["ParentID"]))
                            {
                                cboParentClass.Text = lst.Value;
                                break;
                            }
                        }
                    }
                    txtRemarks.Text = dr["Remarks"].ToString();
                }
                catch(Exception ex)
                {
                    Global.MsgError(ex.Message);
                }
            }
            else
                txtID.Clear();

            ChangeState(EntryMode.NORMAL);
        }

        private void txtClassName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboParentClass.Focus();
            }
        }

        private void cboParentClass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtRemarks.Focus();
            }
        }

        private void cboParentClass_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmAccountClass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
