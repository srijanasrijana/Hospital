using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using SmartSolutions.Controls;
using BusinessLogic;
using System.IO;


namespace Common
{
    
    public interface IfrmSelectAccClassID
    {
       
        void AddSelectedAccClassID(DataTable AccClassID);

    }
 
    public partial class frmSelectAccClass : Form
    {
        private IfrmSelectAccClassID m_ParentForm;//Holds the datatable
        private ArrayList m_AccClassID = new ArrayList();
        private int loopCounter = 0;
        public frmSelectAccClass(Form ParentForm)
        {
            InitializeComponent();
            m_ParentForm = (IfrmSelectAccClassID)ParentForm;
        }


        public frmSelectAccClass(Form ParentForm,ArrayList AccClassID)
        {
            InitializeComponent();
            m_ParentForm = (IfrmSelectAccClassID)ParentForm;
            m_AccClassID = AccClassID;
        }

        /// <summary>
        /// Gets the selected root accounting class ID
        /// </summary>
        /// <returns></returns>
        private static int GetRootAccClassID(ArrayList AccClassID)
        {
            if (AccClassID.Count > 0)
            {
                //Find Root Class
                DataTable dtTemp = AccountClass.GetRootAccClass(Convert.ToInt32(AccClassID[0]));
                return Convert.ToInt32(dtTemp.Rows[0]["AccClassID"]);
            }
            return 1;//The default root class ID
        }
   

        //Loads the access roles of the specific role id
        private void LoadAccClassInfo( TreeNode tn, int[] CheckedIDs, TreeView tvAccess)
        {
            //First check if the treenode is checked
            foreach (int id in CheckedIDs)
            {
                if (Convert.ToInt32(tn.Tag) == id)
                    tn.Checked = true;
            }

            //Now check the child nodes
            foreach (TreeNode nd in tn.Nodes)
            {
                nd.Checked = false; //first clear the checkmark if anything is checked previously
                LoadAccClassInfo(nd, CheckedIDs, tvAccess);
                foreach (int id in CheckedIDs)
                {
                    tvAccess.Enabled = false;
                    if (Convert.ToInt32(nd.Tag) == id)
                        nd.Checked = true;
                }
            }

        }

        private void frmSelectAccClass_Load(object sender, EventArgs e)
        {
            ShowAccClassInTreeView(treeAccClass, null, 0);           
           LoadComboBox(cboParentClass);
        }

        private void LoadComboBox(ComboBox cboParent)
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

            cboParent.Items.Clear();
            DataTable dt;
            DataTable dtgetparentaccclass = AccountClass.GetRootAccClass(Global.GlobalAccClassID);
            DataRow drgetid = dtgetparentaccclass.Rows[0];

            if (Global.GlobalAccClassID == 1)
            {
                dt = AccountClass.GetAccClassTable(0);
            }
            else
            {
                dt = AccountClass.GetAccClassTable1(Convert.ToInt32( drgetid["AccClassID"].ToString()));
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                cboParent.Items.Add(new ListItem((int)dr["AccClassID"], dr[LangField].ToString()));                              
            }
            cboParent.SelectedIndex = 0;     
        }

        //Recursive Function to Show Access Level in Treeview
        private void ShowAccClassInTreeView(TreeView tv, TreeNode n, int AccClassID)
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

            if (Global.GlobalAccClassID == 1 && Global.GlobalAccessRoleID == 37)
            {
                DataTable dt = new DataTable();
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

                    TreeNode t = new TreeNode(dr[LangField].ToString());
                    t.Tag = dr["AccClassID"].ToString();
                    //Check if it is a parent Or if it has childs
                    try
                    {
                        if (ChildCount((int)dr["AccClassID"]) > 0)
                        {
                            //t.IsContainer = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                    if (n == null)
                    {
                        t.Checked = true;
                        tv.Nodes.Add(t); //Primary Group
                    }
                    else
                    {
                        t.Checked = true;
                        n.Nodes.Add(t); //Secondary Group
                    }
                }
            }
            else
            {

                DataTable dtUserInfo = User.GetUserInfo(User.CurrUserID); //user id must be read from  global i.e current user id
                DataRow drUserInfo = dtUserInfo.Rows[0];
                ArrayList AccClassChildIDs = new ArrayList();
                ArrayList tempParentAccClassChildIDs = new ArrayList();
                AccClassChildIDs.Clear();
                AccClassChildIDs.Add(Convert.ToInt32(drUserInfo["AccClassID"]));
                AccountClass.GetChildIDs(Convert.ToInt32(drUserInfo["AccClassID"]), ref AccClassChildIDs);
                DataTable dt = new DataTable();
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
                    TreeNode t = new TreeNode(dr[LangField].ToString());
                    t.Tag = dr["AccClassID"].ToString();
                    tempParentAccClassChildIDs.Clear();
                    AccountClass.GetChildIDs(Convert.ToInt32(t.Tag), ref tempParentAccClassChildIDs);
                    //Check if it is a parent Or if it has childs
                    try
                    {
                        if (ChildCount((int)dr["AccClassID"]) > 0)
                        {
                            //t.IsContainer = true;
                        }

                        foreach (int itemIDs in AccClassChildIDs)  //To check if 
                        {
                            if (Convert.ToInt32(t.Tag) == itemIDs)
                            {
                                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                                loopCounter--;
                                t.Checked = true;
                                if (n == null)
                                {
                                    tv.Nodes.Add(t); //Primary Group
                                    return;
                                }
                                else if (Convert.ToInt32(t.Tag) == Convert.ToInt32(drUserInfo["AccClassID"]))
                                {
                                    t.Checked = true;
                                    tv.Nodes.Add(t);
                                    return;
                                }
                                else
                                {
                                    n.Nodes.Add(t); //Secondary Group
                                }
                            }
                            if (tempParentAccClassChildIDs.Contains(itemIDs) && loopCounter == 0)
                            {
                                ShowAccClassInTreeView(tv, t, Convert.ToInt16(dr["AccClassID"].ToString()));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            treeAccClass.ExpandAll();
        }

        private int ChildCount(int AccClassID)
        {
            try
            {
                int m_RecCount = (int)User.GetAccessInfo(AccClassID).Rows.Count;
                return m_RecCount;
            }
            catch
            {
                throw;
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList Ids = treeAccClass.GetCheckedNodes(true);
                DataTable dtSelectedAccClassID = new DataTable();
                dtSelectedAccClassID.Columns.Add("AccClassID", typeof(int));

                //If nothing is checked but root class id is selected, send only the selected class id
                if (Ids.Count == 0)
                {
                    if (cboParentClass.SelectedIndex >= 0)//Something is selected
                    {
                        //if root class is selected then collects it child IDs too
                        int ParentID = Convert.ToInt32(((ListItem)cboParentClass.SelectedItem).ID);
                        ArrayList arrchildAccClassIds = new ArrayList();
                        AccountClass.GetChildIDs(ParentID,ref arrchildAccClassIds);

                      
                        dtSelectedAccClassID.Rows.Add(ParentID.ToString());

                        //just for test
                        foreach(object obj in arrchildAccClassIds)
                        {
                            int i = (int) obj;
                            dtSelectedAccClassID.Rows.Add(i.ToString());
                        }
                    }
                }
                //if(Global.CheckAcc=="STOCKSTATUS")
                //{


                //new change  comment following if condition so that parent id is not returned if not selected but other is selected 

                    //if (cboParentClass.SelectedIndex >= 0)//Something is selected
                    //{
                    //    int ParentID = Convert.ToInt32(((ListItem)cboParentClass.SelectedItem).ID);
                    //    dtSelectedAccClassID.Rows.Add(ParentID.ToString());
                    //}


                //}
                foreach (string arr in Ids)
                {
                    dtSelectedAccClassID.Rows.Add(arr);
                }

                //Lastly check if there are no ids, if there arent any, simply insert the root class ID
                if (dtSelectedAccClassID.Rows.Count == 0)
                    dtSelectedAccClassID.Rows.Add("1");

                //Call the interface function to add the text in the parent form container
                m_ParentForm.AddSelectedAccClassID(dtSelectedAccClassID);
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
        private void cboParentClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListItem liAccClassID = new ListItem();
                liAccClassID = (ListItem)cboParentClass.SelectedItem;
                treeAccClass.Nodes.Clear();
                //adding root node too to the tree view
                TreeNode t = new TreeNode(cboParentClass.Text);
                t.Tag = liAccClassID.ID.ToString();
                t.Checked = true;
                treeAccClass.Nodes.Add(t);

                ShowAccClassInTreeView(treeAccClass, t, liAccClassID.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }      
        }
        private void frmSelectAccClass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        
    }
}
