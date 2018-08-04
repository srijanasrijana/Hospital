using BusinessLogic;
using BusinessLogic.HOS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hospital.View
{
    public partial class frmDiseasesSetUp : Form
    {
        public frmDiseasesSetUp()
        {
            InitializeComponent();
        }
        int doctorid;
        public frmDiseasesSetUp(int doctorid)
        {
            InitializeComponent();
            this.doctorid = doctorid;
        }
        private byte[] imgDisease = null;
        private EntryMode m_mode = EntryMode.NORMAL;
        private DataRow[] drFound;
        private DataTable dtTable;
        private string filterString = "";
        private DiseasesSearchIn m_SearchIn = DiseasesSearchIn.Disease_Group;
        private bool IsExpand = false;
        private void lblParentGroup_Click(object sender, EventArgs e)
        {

        }
        DoctorInfo doc = new DoctorInfo();
        Doctor doctor = new Doctor();
        public void GetListViewDataToSetup()
        {
            doc = new DoctorInfo();
            doctor=new Doctor();
          
                if (doctorid > 0)
                {
                    txtDiseaseID.Text = doctorid.ToString();
                    doc = doctor.GetDoctorDetailInListViewByID(doctorid);
                   

                }        
        }


       
        private void ListDiseaseGroup(ComboBox ComboBoxControl)
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
            DataTable dt = Diseases.GetGroupTable(-1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["GroupID"], dr[LangField].ToString()));

            }

            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";

        }
        private void btnGrpSave_Click(object sender, EventArgs e)
        {
            switch (m_mode)
            {
              #region NEW
               case EntryMode.NEW:
                    if (tabDiseaseSetup.SelectedIndex == 0) //Disease Head
                    {

                        try
                        {
                            IDiseases diseaseHead = new Disease1();
                            diseaseHead.CreateGrp(txtGroupName.Text.Trim(), cboParentGrp.Text.Trim(), txtDescription.Text.Trim());

                            Global.Msg("Diseases head created successfully!");

                             ChangeState(EntryMode.NORMAL);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else if (tabDiseaseSetup.SelectedIndex == 2) //Disease Selected
                    {
              
                        try
                        {

                            IDiseases diseaseHead = new Disease1();
                            diseaseHead.CreateGrp(txtGroupName.Text.Trim(), cboParentGrp.Text.Trim(), txtDescription.Text.Trim());

                            Global.Msg("Disease head created successfully!");
                            ChangeState(EntryMode.NORMAL);

                            ListDiseaseGroup(cboParentGrp);

                        }
                        catch (Exception ex)
                        {
                            Global.MsgError(ex.Message);
                        }
                    }
                break;
               #endregion

               #region EDIT
               case EntryMode.EDIT:

                //Check whether the modified parent group is not under our own group
                ArrayList ReturnIDs = new ArrayList();

                Diseases.GetDiseaseUnder(Convert.ToInt32(txtGroupID.Text.Trim()), ReturnIDs);
                ListItem liGroupID = new ListItem();
                liGroupID = (ListItem)cboParentGrp.SelectedItem;

                if (ReturnIDs.BinarySearch(liGroupID.ID) > -1) //if found returns greater index than -1
                {
                    Global.Msg("The parent group name selected is the child of its own group");
                    return;
                }
                else if (Convert.ToInt32(txtGroupID.Text.Trim()) == liGroupID.ID)
                {
                    Global.Msg("The parent group name selected can not be itself.");
                    return;
                }


                try
                {

                    IDiseases DisHead = new Disease1();

                    DisHead.ModifyGrp(Convert.ToInt32(txtGroupID.Text.Trim()), txtGroupName.Text.Trim(), cboParentGrp.Text.Trim(), txtDescription.Text.Trim());
                    Global.Msg("Disease Group modified successfully!");
                    ChangeState(EntryMode.NORMAL);


                    ListDiseaseGroup(cboParentGrp);
                }
                
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
                }
                break;
               #endregion

            }
            ClearDiseaseGroupHeadForm();
            UpdateDiseaseTree();
            ListDiseaseGroup(cboParentGrp);
            ListDiseaseGroup(cboDiseaseGroup);
                        
        }
        private void UpdateDiseaseTree()
        {

            tvDisease.Nodes.Clear();
            ShowDiseaseHeadInTreeView(tvDisease, null, 0);
        }

        private void HosDiseasesSetUp_Load(object sender, EventArgs e)
        {
          //  GetListViewDataToSetup();

            dtTable = Diseases.getDisease();
            drFound = dtTable.Select(filterString);

            cboSrchSearchIn1.Items.Clear();
            cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Account_Groups, LangMgr.Translate("DISEASES_GROUP", "Disease Groups")));
            cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Ledgers, LangMgr.Translate("DISEASE", "Disease")));
           // cboSrchSearchIn1.Items.Clear();
          //  cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Disease_Group, LangMgr.Translate("DISEASES_GROUP", "Disease Groups")));
          //  cboSrchSearchIn1.Items.Add(new ListItem((int)SearchIn.Disease, LangMgr.Translate("DISEASE", "Disease")));

            cboSrchSearchIn1.SelectedIndex = 0;
            cboSrchOP1.SelectedIndex = 0;

            ShowDiseaseHeadInTreeView(tvDisease, null, 0);
            ListDiseaseGroup(cboParentGrp);          
            ListDiseaseGroup(cboDiseaseGroup);

           
        
            ChangeState(EntryMode.NORMAL);
          
            if (Global.disease == true)
            {
                tabDiseaseSetup.SelectedIndex = 1;
                btnDiseaseNew_Click(sender, e);
                
            }
            
          
            ChangeState(EntryMode.NEW);

        }


        public void ShowDiseaseHeadInTreeView(TreeView tv, TreeNode n, int Disease_ID)
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

            dt = Diseases.GetGroupTable(Disease_ID);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                TreeNode t = new TreeNode(dr[LangField].ToString(), 0, 0);

                ShowDiseaseHeadInTreeView(tv, t, Convert.ToInt16(dr["GroupID"].ToString()));

                if (n == null)

                    tv.Nodes.Add(t); //Primary Group

                else
                {
                    n.Nodes.Add(t); //Secondary Group
                }
                //Also add Disease
                DataTable dtDisease = Diseases.GetDiseaseTable(Convert.ToInt32(dr["GroupID"]));

                for (int j = 0; j < dtDisease.Rows.Count; j++)
                {
                    DataRow drDisease = dtDisease.Rows[j];
                    TreeNode tnDisease = new TreeNode(drDisease[LangField].ToString());
                    tnDisease.ForeColor = Color.DarkBlue;
                    t.Nodes.Add(tnDisease);
                }

            }

        }
        private void btnDiseaseSave_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (txtDiseaseName.Text == "")
                {
                    Global.Msg("Please Insert Proper Diseases Name and Try Again!");
                    txtDiseaseName.Focus();
                    return;
                }
                else if (cboDiseaseGroup.Text == "")
                {
                    Global.Msg("Please Select Proper Diseases Group and Try Again!");
                    cboDiseaseGroup.Focus();
                    return;
                }
               
              
                IDiseases Disease = new Disease1();
                ListItem lstDiseseGrp = (ListItem)cboDiseaseGroup.SelectedItem;

                switch (m_mode)
                {
                    #region NEW
                    case EntryMode.NEW:
                        if (tabDiseaseSetup.SelectedIndex == 1) //Disease Selected
                        {
                          


                            bool result = Disease.CreateDiseases(txtDiseaseName.Text.Trim(), Convert.ToInt32(lstDiseseGrp.ID), txtDiseaseRemarks.Text.Trim(), imgDisease);

                            if (result)
                                Global.Msg("Diseases created successfully!");
                            else
                                Global.Msg("There is some problem in diseases creation");
                        }
                        break;
                    #endregion
                    #region EDIT
                    case EntryMode.EDIT:
                        if (tabDiseaseSetup.SelectedIndex == 1) //Disease Selected
                        {
                            try
                            {
                                bool result = Disease.ModifyDiseases(Convert.ToInt32(txtDiseaseID.Text), txtDiseaseName.Text.Trim(), Convert.ToInt32(lstDiseseGrp.ID), txtDiseaseRemarks.Text.Trim(), imgDisease);
                                if (result)
                                    Global.Msg("Diseases modified successfully!");
                                else
                                    Global.Msg("There is some problem in Diseases modification");
                                ChangeState(EntryMode.NORMAL);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                
                            }
                        
                        }
                        break;
                    #endregion
                }
                ClearDiseaseForm();
                UpdateDiseaseTree();
               
                if (Global.disease == true)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {

               Global.MsgError(ex.Message);
            }


        }

        private void ClearDiseaseGroupHeadForm()
        {
            txtGroupName.Clear();
            cboParentGrp.Text =  "";
            txtDescription.Clear();
        }

        private void FillDiseaseForm(int ID)
        {
            try
            {

                //Get details from ID
                DataTable dtDisease = Diseases.GetDiseaseInfo(ID, LangMgr.DefaultLanguage);
                foreach (DataRow drDisease in dtDisease.Rows)
                {
                    ClearDiseaseGroupHeadForm();
                    txtDiseaseID.Text = drDisease["ID"].ToString();
                    txtDiseaseName.Text = drDisease["DisName"].ToString();
                    txtDiseaseRemarks.Text = drDisease["Remarks"].ToString();              
                    cboDiseaseGroup.Text = drDisease["GroupName"].ToString();
                   

                                      
                    if (drDisease["Image"].ToString() == "")
                    {
                        PicDisease.Image = null;
                    }

                    else
                    {
                        PicDisease.Image = Misc.GetImageFromByte((byte[])drDisease["Image"]);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillDisGroupForm(int ID)
        {

            tabDiseaseSetup.SelectedIndex = 0;

            //Get details from ID

            DataTable dtGroup = Diseases.GetGroupByID(ID, LangMgr.DefaultLanguage);
            DataRow drGroup = dtGroup.Rows[0];
            txtGroupID.Text = drGroup["ID"].ToString();
            txtGroupName.Text = drGroup["Name"].ToString();
            txtDescription.Text = drGroup["Remarks"].ToString();
            string strUnder = drGroup["Parent"].ToString();
            if (strUnder == null)
                cboParentGrp.Text = "(MAIN ACCOUNT)";
            else
                cboParentGrp.Text = strUnder;
        }

        private void tvDisease_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearDiseaseGroupHeadForm();
            try
            {
                //Show the selected value in respective fields
                if (e.Node.ForeColor == Color.DarkBlue)//Disease is being Selected
                {
                    tabDiseaseSetup.SelectedIndex = 1;
                    txtDiseaseName.Text = e.Node.Text;

                    int ID = Diseases.GetDiseaseIDFromName(e.Node.Text, LangMgr.DefaultLanguage);
                    if (String.IsNullOrEmpty(ID.ToString())) //if no data is found                
                    {
                        Global.Msg("ERROR: Please select the Disease properly and then try again");
                        ClearDiseaseGroupHeadForm();
                        return;
                    }
                    FillDiseaseForm(ID);
               }
                else//Disease group is being selected
                {
                   tabDiseaseSetup.SelectedIndex = 0;
                   object m_GroupID = Diseases.GetGroupIDFromName(e.Node.Text, LangMgr.DefaultLanguage);
                    //Fill the textboxes and other fields
                    FillDisGroupForm(Convert.ToInt32(m_GroupID));
                }
                tvDisease.Focus();//Let the focus remain to treeview
                ChangeState(EntryMode.NORMAL);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            //btnOpeningBalance.Enabled = false;


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
                  //  btncolor.Enabled = true;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    break;
            }
        }
        private void EnableControls(bool Enable)
        {

            txtGroupName.Enabled = cboParentGrp.Enabled = txtDescription.Enabled =txtDiseaseName.Enabled = cboDiseaseGroup.Enabled = txtDiseaseRemarks.Enabled = 
                txtImage.Enabled = btnBrowse.Enabled = Enable;        
        }

        //Enables and disables the button states
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnGrpNew.Enabled = btnDiseaseNew.Enabled = New;
            btnGrpEdit.Enabled = btnDiseaseEdit.Enabled = Edit;
            btnGrpSave.Enabled = btnDiseaseSave.Enabled = Save;
            btnGrpDelete.Enabled = btnDiseaseDelete.Enabled = Delete;
            btnGrpCancel.Enabled = btnDiseaseCancel.Enabled = Cancel;
        }
        private void ClearDiseaseForm()
        {
            txtDiseaseName.Clear();
            cboDiseaseGroup.Text = "";
      
            txtDiseaseRemarks.Clear();
            
            txtImage.Text = string.Empty;
            PicDisease.Image = null;
            
            
        }
        private void btnGrpNew_Click(object sender, EventArgs e)
        {
            ClearDiseaseGroupHeadForm();
            ChangeState(EntryMode.NEW);
           
        }

        private void btnDiseaseNew_Click(object sender, EventArgs e)
        {
            ClearDiseaseForm();
            ChangeState(EntryMode.NEW);
        }

        private void btnGrpEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtGroupID.Text))
            {
                Global.MsgError("Please select a Group in the Tree View first.");
                return;
            }
            ChangeState(EntryMode.EDIT);
        }

        private void btnDiseaseEdit_Click(object sender, EventArgs e)
        {
            //Not editable untill a node is selected in the tvDisease treeView
            if (string.IsNullOrEmpty(txtDiseaseID.Text))
            {
                Global.MsgError("Please select a Disease in the Tree View first.");
                return;
            }
            ChangeState(EntryMode.EDIT);
        }

        private void btnGrpDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtGroupID.Text))
            {
                Global.MsgError("Please select a Group in the Tree View first.");
                return;
            }
            int groupId = Convert.ToInt32(txtGroupID.Text.Trim());
            DataTable dtGroup = Diseases.GetGroupTable(groupId);
            DataTable dtDisease = Diseases.GetDiseaseTable(groupId);
            if (dtGroup.Rows.Count > 0)
            {
                Global.MsgError("Sorry! you can not delete a Disease Group that has child Disease Group. First delete the child Disease Group.");
                return;
            }
            else if (dtDisease.Rows.Count > 0)
            {
                Global.MsgError("Sorry! you can not delete a Disease Group");
                return;
            }
            if (Global.MsgQuest("Are you sure you want to delete the Disease  head - " + txtGroupName.Text.Trim() + "?") == DialogResult.No)
                return;
            IDiseases disHead = new Disease1();
            string delDisHead = txtGroupName.Text.Trim();
            disHead.Delete(delDisHead);

            Global.Msg("Disease Group - " + delDisHead + " deleted successfully!");

            ChangeState(EntryMode.NORMAL);
            tvDisease.Nodes.Clear();
            ShowDiseaseHeadInTreeView(tvDisease, null, 0);
            ClearDiseaseGroupHeadForm();
        }

        private void btnDiseaseDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDiseaseID.Text))
            {
                Global.MsgError("Please select a Disease in the Tree View first.");
                return;
            }

            if (Global.MsgQuest("Are you sure you want to delete the Disease -" + txtDiseaseName.Text + "?") == DialogResult.No)
                return;
            IDiseases m_Disease = new Disease1();
            m_Disease.DeleteDisease(Convert.ToInt32(txtDiseaseID.Text));

            Global.Msg("Disease - " + txtDiseaseName.Text + " deleted successfully!");

            ChangeState(EntryMode.NORMAL);

            tvDisease.Nodes.Clear();

            ShowDiseaseHeadInTreeView(tvDisease, null, 0);

            ClearDiseaseForm();
        }

        private void btnDiseaseCancel_Click(object sender, EventArgs e)
        {
            ClearDiseaseForm();
            ChangeState(EntryMode.NORMAL);
        }

        private void btnGrpCancel_Click(object sender, EventArgs e)
        {
            ClearDiseaseGroupHeadForm();
            ChangeState(EntryMode.NORMAL);
        }

        private void tabDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDiseaseGroupInGrid(drFound);
        }
        private void FillDiseaseGroupInGrid(DataRow[] drFound)
        {
            try
            {
                switch (m_SearchIn)
                {
                    case DiseasesSearchIn.Disease_Group:
                    case DiseasesSearchIn.Disease:
                        grdListView.Rows.Clear();
                        grdListView.Redim(drFound.Count() + 1, 2);
                        grdListView[0, 0] = new SourceGrid.Cells.ColumnHeader("Disease Group");
                        grdListView[0, 1] = new SourceGrid.Cells.ColumnHeader("Disease Name");
                        grdListView[0, 0].Column.Width = 100;
                        grdListView[0, 1].Column.Width = 100;
                        break;

                }
                SourceGrid.Cells.Controllers.CustomEvents CellClick = new SourceGrid.Cells.Controllers.CustomEvents();
                SourceGrid.Cells.Controllers.CustomEvents CellMove = new SourceGrid.Cells.Controllers.CustomEvents();
                for (int i = 1; i <= drFound.Count(); i++)
                {
                    DataRow dr = drFound[i - 1];
                    grdListView[i, 0] = new SourceGrid.Cells.Cell(dr["DiseaseGroup"].ToString());
                    grdListView[i, 1] = new SourceGrid.Cells.Cell(dr["DiseaseName"].ToString());
                    grdListView[i, 1].AddController(CellClick);
                    grdListView[i, 1].AddController(CellMove);
                    grdListView[i, 0].AddController(CellClick);
                    grdListView[i, 0].AddController(CellMove);


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void btnToggleExpand_Click(object sender, EventArgs e)
        {
            if (IsExpand)
            {
                tvDisease.CollapseAll();
                IsExpand = false;
                btnToggleExpand.Text = "Expand";
            }
            else
            {
                tvDisease.ExpandAll();
                IsExpand = true;
                btnToggleExpand.Text = "Collapse";
            }
        }

        private void cboSrchSearchIn1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSrchOP1.Items.Clear();
            cboSrchOP2.Items.Clear();

            switch ((DiseasesSearchIn)cboSrchSearchIn1.SelectedIndex)
            {
                case DiseasesSearchIn.Disease_Group:

                // case DiseaseSearchIn.Disease_Group_Under:
                case DiseasesSearchIn.Disease:
                case DiseasesSearchIn.Debtors_Name:
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Begins_With, LangMgr.Translate("BEGINS_WITH", "Begins With")));
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Contains, LangMgr.Translate("CONTAINS", "Contains")));
                    cboSrchOP1.Items.Add(new ListItem((int)SearchOp.Equals, LangMgr.Translate("EQUALS", "Equals")));
                    cboSrchOP2.Enabled = false;
                    txtSrchParam2.Enabled = false;
                    cboSrchOP1.SelectedIndex = 0;
                    break;

            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {

            ListItem li1 = (ListItem)cboSrchOP1.SelectedItem;
            ListItem m_SearchInItem = (ListItem)cboSrchSearchIn1.SelectedItem;
            ListItem li2 = (ListItem)cboSrchOP2.SelectedItem;
            if (li2 == null)
                li2 = new ListItem(0, "");//Begins_With
            m_SearchIn = (DiseasesSearchIn)m_SearchInItem.ID; //Set the private function searchIn so that gridclick() may know what is the current mode.
            try
            {
                Search((DiseasesSearchIn)m_SearchInItem.ID, (SearcjOpDisease)li1.ID, txtSrchParam1.Text, (SearcjOpDisease)li2.ID, txtSrchParam2.Text);
                tabDisplay.SelectedIndex = 1; //Select the listview
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void Search(DiseasesSearchIn m_SearchIn, SearcjOpDisease SrchOP1, string SearchParam1, SearcjOpDisease SrchOP2, string SearchParam2)
        {

            //Let the whole row to be selected
            grdListView.SelectionMode = SourceGrid.GridSelectionMode.Row;
            //Disable multiple selection
            grdListView.Selection.EnableMultiSelection = false;
            switch (m_SearchIn)
            {
                #region Disease Groups Search
                case DiseasesSearchIn.Disease_Group:
                    this.filterString = "DiseaseGroup  Like '" + SearchParam1 + "%'";
                    drFound = dtTable.Select(this.filterString);
                    FillDiseaseGroupInGrid(drFound);
                    break;

                #endregion

                case DiseasesSearchIn.Disease:
                    filterString = "DiseaseName  like '" + SearchParam1 + "%'";
                    drFound = dtTable.Select(filterString);
                    FillDiseaseGroupInGrid(drFound);
                    break;


            }
        }

        private void txtSrchParam1_TextChanged(object sender, EventArgs e)
        {
            if (txtSrchParam1.Text == "")
            {
                filterString = "";
                drFound = dtTable.Select(filterString);

                FillDiseaseGroupInGrid(drFound);
            }
        }


    }
}
