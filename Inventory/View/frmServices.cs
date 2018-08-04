using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections;
using BusinessLogic;
using DateManager;

namespace Inventory
{
    public partial class frmServices : Form
    {
        Services m_Services = new Services();


        private string OldService = " ";
        private string NewServices = " ";
        private bool isNew;
        private SearchInServices m_SearchInServices = SearchInServices.Service_Name; //Default SearchIN Holder

        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        public frmServices()
        {
            InitializeComponent();
        }

        private void frmServices_Load(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NEW);
            txtServicesID.Visible = false;//Making ServiceID's textbox invisible
            ListServicesGroup(cmboParentServiceName);

            //AccountGroup.Search(SearchIn.Account_Groups, SearchOp.Begins_With, "", SearchOp.Begins_With, "", LangMgr.Language);//Search a blank text with begins with so that all the available data is listed
            ////Set the default search selection
            //cboSrchSearchIn1.Items.Clear();
            cmboSearchin.Items.Clear();
            cmboSearchin.Items.Add(new ListItem((int)SearchInServices.Service_Code,LangMgr.Translate("SERVICE_CODE","Service Code")));
            cmboSearchin.Items.Add(new ListItem((int)SearchInServices.Service_Name, LangMgr.Translate("SERVICE_NAME", "Service Name")));
            

            ////Fill the account head name in the treeview
            ShowServicesGroupsInTreeView(tvServices, null, 0);

        }
        //Fill the cboUnder List box with Service Head
        private void ListServicesGroup(ComboBox ComboBoxControl)
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
            DataTable dt = Services.GetServicesTable(-1);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ServicesID"], dr[LangField].ToString()));

            }

            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";

        }

        private void ClearServiceForm()
        {
            txtServiceCode.Text = string.Empty;
            txtServiceName.Text = string.Empty;
            txtSalesRate.Text = string.Empty;
            txtPurchaseRate.Text = string.Empty;
            txtDescription.Text = string.Empty;           
        
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            ////Check Validation
            //if (!ValidateGroup())
            //    return;

            switch (m_mode)
            {
                    #region NEW
                case EntryMode.NEW: //if new button is pressed
                    //if (tabAccountSetup.SelectedIndex == 0) //Account Head
                    //{
                    isNew = true;
                    NewServices = " ";
                    OldService = " ";
                    NewServices = NewServices + "ServiceCode" + txtServiceCode.Text + "ServiceName" + txtServiceName.Text + "Parent" + cmboParentServiceName.Text + "SalesRate" + txtSalesRate.Text + "PurchaseRate" + txtPurchaseRate.Text;
                        try
                        {
                         
                            string Curent_Date = (Date.DBToSystem(Date.GetServerDate().ToString()));
                            Ledger m_Ledger = new Ledger();
                            m_Services.Create(txtServiceCode.Text.Trim(), txtServiceName.Text.Trim(), cmboParentServiceName.Text.Trim(), txtDescription.Text.Trim(),Convert.ToDouble(txtSalesRate.Text),Convert.ToDouble(txtPurchaseRate.Text),OldService,NewServices,isNew);
                            Global.Msg("Services created successfully");
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

                    //EDIT BLOCK
                    #region EDIT
                case EntryMode.EDIT:
                    isNew = false;
                    NewServices = " ";
                    NewServices = NewServices + "ServiceCode" + txtServiceCode.Text + "ServiceName" + txtServiceName.Text + "Parent" + cmboParentServiceName.Text + "SalesRate" + txtSalesRate.Text + "PurchaseRate" + txtPurchaseRate.Text;
                  //Check whether the modified parent Service is not under our own group
                    ArrayList ReturnIDs = new ArrayList();
                    Services.GetServicesUnder(Convert.ToInt32(txtServicesID.Text.Trim()), ReturnIDs);
                    ListItem liServicesID = new ListItem();
                    liServicesID = (ListItem)cmboParentServiceName.SelectedItem;

                    if (ReturnIDs.BinarySearch(liServicesID.ID) > -1) //if found returns greater index than -1
                    {
                        Global.Msg("The Service group name selected is the child of its own group");
                        return;
                    }

                    try
                    {

                        m_Services.Modify(Convert.ToInt32(txtServicesID.Text), txtServiceCode.Text.Trim(), txtServiceName.Text.Trim(), cmboParentServiceName.Text.Trim(), txtDescription.Text.Trim(), Convert.ToDouble(txtSalesRate.Text),Convert.ToDouble(txtPurchaseRate.Text),OldService,NewServices,isNew);
                        Global.Msg("Service Group modified successfully!");
                        ChangeState(EntryMode.NORMAL);
                        ListServicesGroup(cmboParentServiceName);
                   
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

            }//end switch(m_mode)

            ClearServiceForm();
            UpdateServicesTree();
            ListServicesGroup(cmboParentServiceName);
            ChangeState(EntryMode.NORMAL);
          

        }


        private void UpdateServicesTree()
        {
            tvServices.Nodes.Clear();
            ShowServicesGroupsInTreeView(tvServices, null, 0);
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
                    ButtonState(false, false,  true, false, true);
                    break;

            }
        }


        private void EnableControls(bool Enable)
        {
           txtServiceCode.Enabled=txtServiceName.Enabled=txtSalesRate.Enabled=txtDescription.Enabled = txtPurchaseRate.Enabled=cmboParentServiceName.Enabled=Enable;
            
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnServicesNew.Enabled  = New;
            btnServicesEdit.Enabled = Edit;
            btnServicesSave.Enabled = Save;
            btnServicesDelete.Enabled = Delete;
            btnServicesCancel.Enabled = Cancel;
        }


        private void btnGrpNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("SERVICE_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearServiceForm();
            ChangeState(EntryMode.NEW);
            
            
        }

        private void btnServicesEdit_Click(object sender, EventArgs e)
        {

        }

      

        //Recursive Function to Show Account Group in Treeview
        public void ShowServicesGroupsInTreeView(TreeView tv, TreeNode n, int Services_ID)
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
            dt = Services.GetServicesTable(Services_ID);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                TreeNode t = new TreeNode(dr[LangField].ToString(), 0, 0);

                ShowServicesGroupsInTreeView(tv, t, Convert.ToInt16(dr["ServicesID"].ToString()));

                if (n == null)

                    tv.Nodes.Add(t); //Primary Group

                else
                {
                    n.Nodes.Add(t); //Secondary Group


                }


            }

        }

        private void tvServices_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearServiceForm();
            try
            {
                //Show the selected value in respective fields
          
               //Services group is being selected
                
                    //tabAccountSetup.SelectedIndex = 0;

                object m_ServicesID = Services.GetIDFromName(e.Node.Text, LangMgr.DefaultLanguage);

                    //Fill the textboxes and other fields
                    FillServicesGroupForm(Convert.ToInt32(m_ServicesID));


                

                tvAccount.Focus();//Let the focus remain to treeview
                ChangeState(EntryMode.NORMAL);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                //throw;

            }
        }

        /// <summary>
        /// Fills the Account fields from account group id
        /// </summary>
        /// <param name="ID"></param>
        private void FillServicesGroupForm(int ID)
        {

            //tabAccountSetup.SelectedIndex = 0;

            //Get details from ID
            DataTable dtServices = Services.GetServicesByID(ID, LangMgr.DefaultLanguage);

            DataRow drServices = dtServices.Rows[0];
            txtServicesID.Text = drServices["ID"].ToString();
            txtServiceCode.Text = drServices["Code"].ToString();
            txtServiceName.Text = drServices["Name"].ToString();
            cmboParentServiceName.Text = drServices["Parent"].ToString();
            txtDescription.Text = drServices["Description"].ToString();
            txtSalesRate.Text = drServices["SalesRate"].ToString();
            txtPurchaseRate.Text = drServices["PurchaseRate"].ToString();
            string strUnder = drServices["Parent"].ToString();
            if (strUnder == null)
                cmboParentServiceName.Text = "(MAIN ACCOUNT)";
            else
                cmboParentServiceName.Text = strUnder;
        }
        //Clears the text of every field of Account Head form
        //private void ClearServicesHeadForm()
        //{
        //    txtServiceCode.Text = "";
        //    txtServiceName.Clear();
        //    txtSalesRate.Text = "";
        //    txtDescription.Clear();
        //    cmboParentServiceName.Text = "";
           
        //}

        private void btnServicesEdit_Click_1(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SERVICE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            isNew = false;
            OldService = " ";
            OldService = OldService + "ServiceCode" + txtServiceCode.Text + "ServiceName" + txtServiceName.Text + "Parent" + cmboParentServiceName.Text + "SalesRate" + txtSalesRate.Text + "PurchaseRate" + txtPurchaseRate.Text;
            ChangeState(EntryMode.EDIT);
        }

        private void btnServicesDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("SERVICE_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            //Ask if the Services head is to be deleted
            if (Global.MsgQuest("Are you sure you want to delete the Service head - " + txtServiceName.Text.Trim() + "?") == DialogResult.No)
                return;
            //If the user confirms deletion
            try
            {
                string delServiceHead = txtServiceName.Text.Trim();
                m_Services.Delete(delServiceHead);
                Global.Msg("Service Group - " + delServiceHead + " deleted successfully!");
                ChangeState(EntryMode.NORMAL);
                tvServices.Nodes.Clear();
                ShowServicesGroupsInTreeView(tvServices, null, 0);
                ClearServiceForm();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        //private void btnServicesCancel_Click(object sender, EventArgs e)
        //{

        //    ChangeState(EntryMode.NORMAL);

        //}

        private void SearchService(SearchInServices m_SearchInServices, SearchOpServices SrchOP, string SearchParam)
        {


        //    //Let the whole row to be selected
        //    grdListViesServices.SelectionMode = SourceGrid.GridSelectionMode.Row;


        //    //Set the border width of the selection to thin
        //    DevAge.Drawing.RectangleBorder b = grdListViesServices.Selection.Border;
        //    b.SetWidth(1);
        //    grdListViesServices.Selection.Border = b;

        //    //Disable multiple selection
        //    grdListViesServices.Selection.EnableMultiSelection = false;

        //    string FilterString = "";
        //    switch (m_SearchInServices)
        //    {
        //        #region Account Groups Search
        //        case SearchIn.Account_Groups:


        //        #endregion

        //#endregion
        //    }
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //ListItem m_SearchInServicesItem = (ListItem)cmboSearchin.SelectedItem;
            //ListItem li = (ListItem)cboSrchOP.SelectedItem;

            //m_SearchInServices = (SearchInServices)m_SearchInServicesItem.ID; //Set the private function searchIn so that gridclick() may know what is the current mode.
            //try
            //{

            ////    SearchService((SearchInServices)m_SearchInServicesItem.ID, (SearchOpServices)li.ID, txtSrchParam.Text);

            //    tabDisplay.SelectedIndex = 1; //Select the listview

            //}
            //catch (Exception ex)
            //{

            //    Global.MsgError(ex.Message);
            //}

        }

        private void frmServices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnServicesCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }
    }
}
