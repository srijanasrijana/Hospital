using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Data.SqlClient;
using System.Collections;

namespace Accounts
{
    public partial class frmProject : Form
    {
        private EntryMode m_mode = EntryMode.NORMAL;
        Project m_Project = new Project();
        public frmProject()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void frmProject_Load(object sender, EventArgs e)
        {
            txtProjectID.Visible = false;
            ShowProjectHeadInTreeView(tvProject, null, 0);
            ChangeState(EntryMode.NEW);
            ListProject(cboParentProjectName);
            LoadProjectList();

        }

        /// <summary>
        /// Loads the grdListView
        /// </summary>
        private void LoadProjectList()
        {
            try
            {
                DataTable dt = Project.GetAllProject(LangMgr.DefaultLanguage);
                grdListView.Redim(dt.Rows.Count + 1, 3);
                grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("ProjectID");
                grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Project");
                grdListView[0, 2] = new SourceGrid.Cells.ColumnHeader("Parent Project");

                grdListView[0, 0].Column.Width = 100;
                grdListView[0, 0].Column.Visible = false;
                grdListView[0, 1].Column.Width = 150;
                grdListView[0, 2].Column.Width = 150;

                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i-1];
                    grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["ProjectID"].ToString());
                    grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["Project"].ToString());
                    grdListView[i, 2] = new SourceGrid.Cells.Cell(dr["ParentProject"].ToString());
                }
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
        }

          //Validate Form inputs under Group Tab
        private bool ValidateProject()
        {
            bool bValidate = false;

            FormHandle m_FHandle = new FormHandle();
            //m_FHandle.AddValidate(txtGroupName, DType.NAME, "Invalid account group name. Please choose a valid group name");

            bValidate = m_FHandle.Validate();

            if (cboParentProjectName.SelectedItem == null)
            {
                Global.MsgError("Invalid Parent Project Name Selected");
                bValidate = false;
            }

            return bValidate;
        }

        private void EnableControls(bool Enable)
        {
            txtProjectName.Enabled = cboParentProjectName.Enabled = txtDescription.Enabled = Enable;
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

        //Fill the cboUnder List box with Project Head
        private void ListProject(ComboBox ComboBoxControl)
        {
            #region Language Management
            ComboBoxControl.Font = LangMgr.GetFont();
            string LangField = "English";
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

            ComboBoxControl.Items.Clear();
            DataTable dt = Project.GetProjectTable(-1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], dr[LangField].ToString()));

            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";

        }

        //Recursive Function to Show Account Group in Treeview
        public void ShowProjectHeadInTreeView(TreeView tv, TreeNode n, int ProjectID)
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
            dt = Project.GetProjectTable(ProjectID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                TreeNode t = new TreeNode(dr[LangField].ToString(), 0, 0);
                ShowProjectHeadInTreeView(tv, t, Convert.ToInt16(dr["ProjectID"].ToString()));             
                if (n == null)
                    tv.Nodes.Add(t); //Primary Group
                else
                {
                    n.Nodes.Add(t); //Secondary Group

                }          
            }
        }

        private void UpdateProjectTree()
        {
            tvProject.Nodes.Clear();
            ShowProjectHeadInTreeView(tvProject, null, 0);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = false;
            if (m_mode == EntryMode.NEW)
                chkUserPermission = UserPermission.ChkUserPermission("PROJECT_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("PROJECT_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to save. Please contact your administrator for permission.");
                return;
            }

            //Check Validation
            if (!ValidateProject())
                return;

            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
             

                        try
                        {
                        
                            m_Project.Create(txtProjectName.Text.Trim(), cboParentProjectName.Text.Trim(), txtDescription.Text.Trim());
                            Global.Msg("Project created successfully!");
                            ChangeState(EntryMode.NORMAL);


                        }
                        catch (SqlException ex)
                        {

                            switch (ex.Number)
                            {
                                case 4060: // Invalid Database 
                                    Global.Msg("Invalid Database", MBType.Error, "Error");
                                    break;

                                case 18456: // Login Failed 
                                    Global.Msg("Login Failed!", MBType.Error, "Error");
                                    break;

                                case 547: // ForeignKey Violation , Check Constraint
                                    Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                                    break;

                                case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                                    Global.Msg("ERROR: The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                                    break;

                                case 2601: // Unique Index/Constriant Violation 
                                    Global.Msg("Unique index violation!", MBType.Warning, "Error");
                                    break;

                                case 5000: //Trigger violation
                                    Global.Msg("Trigger violation!", MBType.Warning, "Error");
                                    break;

                                default:
                                    Global.MsgError(ex.Message);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Global.MsgError(ex.Message);
                        }
                

              

                    break;

                #endregion

                #region EDIT
                case EntryMode.EDIT:

                    //Check whether the modified parent group is not under our own group
                    ArrayList ReturnIDs = new ArrayList();
                    Project.GetProjectsUnder(Convert.ToInt32(txtProjectID.Text.Trim()), ReturnIDs);                    
                    ListItem liProjectID = new ListItem();
                    liProjectID = (ListItem)cboParentProjectName.SelectedItem;
                    if (ReturnIDs.BinarySearch(liProjectID.ID) >= 0) //if found returns greater index than -1
                    {
                        Global.Msg("The parent Project name selected can not be the child of its own Project");
                        return;
                    }
                    else if (Convert.ToInt32(txtProjectID.Text.Trim()) == liProjectID.ID)  //Project name and its parentProject cannot be the same.
                    {
                        Global.Msg("The parent Project name selected can not be itself");
                        return;
                    }


                    try
                    {
                        m_Project.Modify(Convert.ToInt32(txtProjectID.Text.Trim()), txtProjectName.Text.Trim(), cboParentProjectName.Text.Trim(), txtDescription.Text.Trim());
                        Global.Msg("Project modified successfully!");
                        ChangeState(EntryMode.NORMAL);
                        ListProject(cboParentProjectName);
                    }
                    catch (SqlException ex)
                    {

                        switch (ex.Number)
                        {
                            case 4060: // Invalid Database 
                                Global.Msg("Invalid Database", MBType.Error, "Error");
                                break;

                            case 18456: // Login Failed 
                                Global.Msg("Login Failed!", MBType.Error, "Error");
                                break;

                            case 547: // ForeignKey Violation , Check Constraint
                                Global.Msg("Invalid parent group! Check the parent group and try again!", MBType.Warning, "Error");
                                break;

                            case 2627: // Unique Index/ Primary key Violation/ Constriant Violation 
                                Global.Msg("The group name already exists! Please choose another group names!", MBType.Warning, "Error");
                                break;

                            case 2601: // Unique Index/Constriant Violation 
                                Global.Msg("Unique index violation!", MBType.Warning, "Error");
                                break;

                            case 5000: //Trigger violation
                                Global.Msg("Trigger violation!", MBType.Warning, "Error");
                                break;

                            default:
                                Global.MsgError(ex.Message);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

                //}//end switch(m_mode)
            }
            UpdateProjectTree();
            ListProject(cboParentProjectName);
            LoadProjectList();
        }

        private void ClearProjectHeadForm()
        {
            txtProjectName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            cboParentProjectName.SelectedIndex = 0;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PROJECT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearProjectHeadForm();
            ChangeState(EntryMode.NEW); 
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PROJECT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }

            //'All Project' is not allowed to be edited.
            if(txtProjectName.Text.Trim() == "All Project")
            {
                Global.MsgError("Sorry! 'All Project' can not be edited.");
                return;
            }

            //Edit not allowed untill a node is selected in the tvProduct treeView
            if (string.IsNullOrEmpty(txtProjectID.Text))
            {
                Global.MsgError("Please select a Project in the Tree View first.");
                return;
            }

            ChangeState(EntryMode.EDIT);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearProjectHeadForm();
            if (m_mode == EntryMode.EDIT)
                FillProjectForm(Convert.ToInt32(txtProjectID.Text));
            else
                txtProjectID.Clear();

            ChangeState(EntryMode.NORMAL);
        }

        private void tvProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearProjectHeadForm();
            try
            {
                    txtProjectName.Text = e.Node.Text;
                    int ID = Project.GetIDFromName(e.Node.Text, LangMgr.DefaultLanguage);
                    if (ID == null) //if no data is found
                    {
                        Global.Msg("ERROR: Please select the Project  properly and then try again");
                        return;
                    }

                    //Fill the Project form 
                    //FillLedgerForm(ID);
                    FillProjectForm(ID);



                tvProject.Focus();//Let the focus remain to treeview
                ChangeState(EntryMode.NORMAL);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                //throw;

            }
        }



        /// <summary>
        /// Fills the Project fields from Project id
        /// </summary>
        /// <param name="ID"></param>
        private void FillProjectForm(int ID)
        {

            //Get details from ID
            DataTable dtProject = Project.GetProjectByID(ID, LangMgr.DefaultLanguage);
            
            DataRow drProject = dtProject.Rows[0];
            txtProjectID.Text = drProject["ID"].ToString();
            txtProjectName.Text = drProject["Name"].ToString();
            txtDescription.Text = drProject["Description"].ToString();
            string strUnder = drProject["Parent"].ToString();
            if (strUnder == "")
                cboParentProjectName.Text = "(MAIN PROJECT)";
                
            else
                cboParentProjectName.Text = strUnder;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("PROJECT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }

            //Delete not allowed untill a node is selected in the tvProduct treeView
            if (string.IsNullOrEmpty(txtProjectID.Text))
            {
                Global.MsgError("Please select a Project in the Tree View first.");
                return;
            }

            //'All Project' is not allowed to be deleted.
            if (txtProjectName.Text.Trim() == "All Project")
            {
                Global.MsgError("Sorry! 'All Project' can not be deleted.");
                return;
            }

            //Check if the project has any child project
            //Don't allow user to delete project if child project is present.
            DataTable dt = Project.GetProjectTable(Convert.ToInt32(txtProjectID.Text));
            if (dt.Rows.Count > 0)
            {
                Global.MsgError("Sorry! you can not delete a project that has child project. First delete the child project.");
                return;
            }

            //Ask if the account head is to be deleted
            if (Global.MsgQuest("Are you sure you want to delete the Project head - " + txtProjectName.Text.Trim()+ "?") == DialogResult.No)
                return;


            //If the user confirms deletion
            try
            {
                string delProjectHead = txtProjectName.Text.Trim();
                m_Project.Delete(delProjectHead);
                Global.Msg("Project - " + delProjectHead + " deleted successfully!");

                ChangeState(EntryMode.NORMAL);
                tvProject.Nodes.Clear();
                ShowProjectHeadInTreeView(tvProject, null, 0);
                LoadProjectList();
                ClearProjectHeadForm();


            }
            catch (Exception ex)
            {
                Global.MsgError("Error deleting the project. This may be because you have another project under it. Please delete that project first");
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            ////tabDisplay.SelectedIndex = 1; //Select the listview           
            ////grdListView.Rows.Clear();
            ////DataTable dt = new DataTable();
            ////if (cboCertificate.Text != "")
            ////{
            ////    lstCertificateNo = (ListItem)cboCertificate.SelectedItem;
            ////    if (lstCertificateNo == null)
            ////    {
            ////        Global.Msg("Invalid Certificate Number was posted! please select from list");
            ////        cboCertificate.Focus();
            ////        return;
            ////    }
            ////    CertificateNo = lstCertificateNo.Value.ToString();
            ////}
            ////else
            ////{
            ////    CertificateNo = "";
            ////}

            ////if (cboCitizenshipNo.Text != string.Empty)
            ////{
            ////    CitizenshipNo = cboCitizenshipNo.Text;
            ////}
            ////else
            ////{
            ////    CitizenshipNo = "";
            ////}
            ////if (cboDistrict.Text != "")
            ////{
            ////    lstDistrictID = (ListItem)cboDistrict.SelectedItem;
            ////    if (lstDistrictID == null)
            ////    {
            ////        Global.Msg("Invalid District was posted! please select from list");
            ////        cboDistrict.Focus();
            ////        return;
            ////    }
            ////    DistrictID = Convert.ToInt32(lstDistrictID.ID);
            ////}
            ////else
            ////{
            ////    DistrictID = 0;
            ////}
            ////dt = PesticideCertificate.Search(CertificateNo, txtCertificateReceiver.Text, txtPesticideStore.Text, CitizenshipNo, DistrictID);
            ////int count = dt.Rows.Count;
            ////txtRecords.Text = count.ToString();
            ////for (int i = 0; i < count; i++)
            ////{
            ////    grdCertificate.Rows.Add();
            ////    grdCertificate.Rows[i].Cells["CertificateID"].Value = dt.Rows[i]["CertificateID"].ToString();
            ////    grdCertificate.Rows[i].Cells["CertificateNo"].Value = dt.Rows[i]["CertificateNo"].ToString();
            ////    grdCertificate.Rows[i].Cells["CertificateReceiver"].Value = dt.Rows[i]["CertificateReceiver"].ToString();
            ////    grdCertificate.Rows[i].Cells["PesticideStoreName"].Value = dt.Rows[i]["PesticideStoreName"].ToString();
            ////    grdCertificate.Rows[i].Cells["Address"].Value = dt.Rows[i]["Address"].ToString();
            ////    grdCertificate.Rows[i].Cells["District"].Value = dt.Rows[i]["NepName"].ToString();
            ////    if (dt.Rows[i]["ExamDate"].ToString() != "")
            ////    {
            ////        grdCertificate.Rows[i].Cells["ExamDate"].Value = Date.DBToSystem(dt.Rows[i]["ExamDate"].ToString());
            ////    }
            ////    grdCertificate.Rows[i].Cells["ExamDistrict"].Value = dt.Rows[i]["ExamNepName"].ToString();
            ////    if (dt.Rows[i]["CertificateDistributeDate"].ToString() != "")
            ////    {
            ////        grdCertificate.Rows[i].Cells["CertificateDistributeDate"].Value = Date.DBToSystem(dt.Rows[i]["CertificateDistributeDate"].ToString());
            ////    }
            ////    grdCertificate.Rows[i].Cells["CitizenshipNo"].Value = dt.Rows[i]["CitizenshipNo"].ToString();
            ////    grdCertificate.Rows[i].Cells["BillNo"].Value = dt.Rows[i]["BillNo"].ToString();
            ////}
        }

        private void txtProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cboParentProjectName.Focus();
            }
        }

        private void cboParentProjectName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDescription.Focus();
            }
        }

        private void frmProject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
     



      
    }
}
