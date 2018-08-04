using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;

namespace AccSwift
{
    public partial class frmUnitMaitenace : Form
    {

        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        UnitMaintenance m_UnitMaintenance = new UnitMaintenance();
        public frmUnitMaitenace()
        {
            InitializeComponent();
        }

        private void frmUnitMaitenace_Load(object sender, EventArgs e)
        {
            txtUnitMaintenanceID.Visible = false;
            ChangeState(EntryMode.NORMAL);

            //Display the all records saved in System.tblUnitMaintenance in listview
            ListUnitInfo();
            

        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            bool chkUserPermission=false;
            if(m_mode==EntryMode.NEW)
            chkUserPermission= UserPermission.ChkUserPermission("UNIT_CREATE");

            else if (m_mode == EntryMode.EDIT)
                chkUserPermission = UserPermission.ChkUserPermission("UNIT_MODIFY");

            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Save. Please contact your administrator for permission.");
                return;
            }

            //Check Validation
            if (!Validate())
                return;


            switch (m_mode)
            {

                #region NEW
                case EntryMode.NEW: //if new button is pressed

                    try
                    {


                        

                     
                        m_UnitMaintenance.Save(txtUnitName.Text.Trim(), txtSymbol.Text.Trim(), txtRemarks.Text.Trim());
                       
                        Global.Msg("Unit created successfully!");
                        ListUnitInfo();
                        ChangeState(EntryMode.NORMAL);


                        //Do not close the form if do not close is checked
                        //if (!chkDoNotClose.Checked)
                        //    this.Close();


                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        #region SQLExceptions
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
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }

                    break;

                #endregion

                #region EDIT
                case EntryMode.EDIT: //if edit button is pressed

                    try
                    {


                       
                        m_UnitMaintenance.Modify(Convert.ToInt32(txtUnitMaintenanceID.Text), txtUnitName.Text.Trim(), txtSymbol.Text.Trim(),txtRemarks.Text.Trim());

                        Global.Msg("Unit modified successfully!");
                        ListUnitInfo();
                        ChangeState(EntryMode.NORMAL);


                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        #region SQLExceptions
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
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }

                    break;

                #endregion

                    
            }

        }

       private bool  Validate()
        {

            bool bValidate = true;
            FormHandle m_FHandle = new FormHandle();
            m_FHandle.AddValidate(txtUnitName, DType.NAME, "Invalid Unit name. Please choose a valid Unit name and try again.");
            m_FHandle.AddValidate(txtSymbol, DType.NAME, "Invalid Symbol. Please choose a valid Symbol and try again.");
      
            bValidate = m_FHandle.Validate();


            return bValidate;
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
           
            txtUnitName.Enabled = txtSymbol.Enabled = txtRemarks.Enabled = Enable;
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("UNIT_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to maintain units. Please contact your administrator for permission.");
                return;
            }
            
            ClearUnitMaintenanceForm();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
        }

        private void lvUnitMaintenance_SelectedIndexChanged(object sender, EventArgs e)
        {

            ChangeState(EntryMode.NORMAL);
            try
            {

                ClearUnitMaintenanceForm();
             
                int UnitMaintenaceID = Convert.ToInt32(lvUnitMaintenance.SelectedItems[0].SubItems[4].Text);

                DataTable dt = UnitMaintenance.GetUnitMaintenaceInfo(UnitMaintenaceID);
                foreach (DataRow dr in dt.Rows)
                {

                    txtUnitName.Text = dr["UnitName"].ToString();
                    txtSymbol.Text = dr["Symbol"].ToString();
                    txtRemarks.Text = dr["Remarks"].ToString();
                    txtUnitMaintenanceID.Text = dr["UnitMaintenanceID"].ToString();
                
            
                }

            }
            catch (Exception ex)
            {
            }

       
            

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("UNIT_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to maintain units. Please contact your administrator for permission.");
                return;

            }
            if (txtUnitMaintenanceID.Text.Length <= 0)
            {
                Global.MsgError("Please select to existing ListView first and then try again!");
                return;
            }
            EnableControls(true);
            ChangeState(EntryMode.EDIT);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("UNIT_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to maintain units. Please contact your administrator for permission.");
                return;

            }
            //DELETING THE SELECTED SERIES NAME
         
            if (Global.MsgQuest("Are you sure you want to delete the Unit -" +txtUnitName.Text +"?") == DialogResult.No)
                return;

            m_UnitMaintenance.Delete(Convert.ToInt32(txtUnitMaintenanceID.Text));

            Global.Msg("Unit - " + txtUnitName.Text + " deleted successfully!");

            ListUnitInfo();

            ClearUnitMaintenanceForm();
        }

        private void ListUnitInfo()
        {
            lvUnitMaintenance.Items.Clear();

            //Get the information of system.tblUnitMaintenace

            DataTable dtUnitMaintenanceInfo = UnitMaintenance.GetUnitMaintenaceInfo(-1);
            int sno = 1;
            foreach (DataRow dr in dtUnitMaintenanceInfo.Rows)
            {

                ListViewItem lv = new ListViewItem(sno.ToString());
                lv.SubItems.Add(dr["UnitName"].ToString());
                lv.SubItems.Add(dr["Symbol"].ToString());
                lv.SubItems.Add(dr["Remarks"].ToString());
                lv.SubItems.Add(dr["UnitMaintenanceID"].ToString());
                lvUnitMaintenance.Items.Add(lv);

                sno++;
            }
        
        
        }

        private void ClearUnitMaintenanceForm()
        {
            txtUnitMaintenanceID.Text = "";
            txtUnitName.Text = "";
            txtSymbol.Text = "";
            txtRemarks.Text = "";
    
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void frmUnitMaitenace_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

      
    }
}
