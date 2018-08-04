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
    public partial class frmDepot : Form
    {

        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        Depot m_Depot = new Depot();
        private string OldDepot = " ";
        private string NewDepot = " ";
        private bool isNew;

        public frmDepot()
        {
            InitializeComponent();
        }

        private void frmDepot_Load(object sender, EventArgs e)
        {
            txtDepotID.Visible = false;
            ChangeState(EntryMode.NORMAL);

            //Display the all records saved in System.tblUnitMaintenance in listview
            ListDepotInfo();
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

        //Enables and disables the button states
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnCancel.Enabled = Cancel;
        }

        private void EnableControls(bool Enable)
        {

       
            txtDepotName.Enabled = txtCity.Enabled = txtTelephone.Enabled = txtContactPerson.Enabled = txtLicenceNo.Enabled = txtAddress.Enabled = txtPostalCode.Enabled = txtMobile.Enabled = txtRegNo.Enabled = txtRemarks.Enabled = Enable;
        }


        private void ListDepotInfo()
        {

            lvDepot.Items.Clear();

            //Get the information of Inv.tblDepot

            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            int sno = 1;
            foreach (DataRow dr in dtDepotInfo.Rows)
            {
                
                ListViewItem lv = new ListViewItem(sno.ToString());
                lv.SubItems.Add(dr["DepotName"].ToString());
                lv.SubItems.Add(dr["DepotID"].ToString());
                lvDepot.Items.Add(lv);

                sno++;
            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        
        {
            //Check Validation
            if (!Validate())
                return;


            switch (m_mode)
            {

                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    isNew = true;
                    NewDepot = " ";
                    OldDepot = " ";
                    NewDepot = NewDepot + "DepotName" + txtDepotName.Text + "City" + txtCity.Text + "Telephone" + txtTelephone.Text + "ContactPerson" + txtContactPerson.Text + "Address" + txtAddress.Text + "LiscenceNo" + txtLicenceNo.Text + "PostalCode" + txtPostalCode.Text + "Mobile" + txtMobile.Text + "RegNo" + txtRegNo.Text;
                    try
                    {
                        m_Depot.Save(txtDepotName.Text.Trim(), txtCity.Text.Trim(), txtTelephone.Text.Trim(), txtContactPerson.Text.Trim(), txtLicenceNo.Text.Trim(), txtAddress.Text.Trim(), txtPostalCode.Text.Trim(), txtMobile.Text.Trim(), txtRegNo.Text.Trim(), txtRemarks.Text.Trim(),OldDepot,NewDepot,isNew);
                        Global.Msg("Depot created successfully!");
                        ListDepotInfo();
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
                    isNew = false;
                    NewDepot = " ";
                    NewDepot = NewDepot + "DepotName" + txtDepotName.Text + "City" + txtCity.Text + "Telephone" + txtTelephone.Text + "ContactPerson" + txtContactPerson.Text + "Address" + txtAddress.Text + "LiscenceNo" + txtLicenceNo.Text + "PostalCode" + txtPostalCode.Text + "Mobile" + txtMobile.Text + "RegNo" + txtRegNo.Text;
                    try
                    {
                        m_Depot.Modify(Convert.ToInt32(txtDepotID.Text), txtDepotName.Text.Trim(), txtCity.Text.Trim(), txtTelephone.Text.Trim(), txtContactPerson.Text.Trim(), txtLicenceNo.Text.Trim(), txtAddress.Text.Trim(), txtPostalCode.Text.Trim(), txtMobile.Text.Trim(), txtRegNo.Text.Trim(), txtRemarks.Text.Trim(),OldDepot,NewDepot,isNew);

                        Global.Msg("Depot modified successfully!");
                        ListDepotInfo();
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


        private bool Validate()
        {

            bool bValidate = true;
            FormHandle m_FHandle = new FormHandle();
            m_FHandle.AddValidate(txtDepotName, DType.NAME, "Invalid Depot name. Please choose a valid Depot name and try again.");

            bValidate = m_FHandle.Validate();


            return bValidate;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DEPOTE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }

            if (txtDepotID.Text.Length <= 0)
            {
                Global.MsgError("Please select to existing ListView first and then try again!");
                return;
            }
            isNew = false;
            OldDepot = " ";
            OldDepot = OldDepot + "DepotName" +txtDepotName.Text+ "City" +txtCity.Text+ "Telephone" +txtTelephone.Text+ "ContactPerson" +txtContactPerson.Text+ "Address" +txtAddress.Text+ "LiscenceNo" + txtLicenceNo.Text+"PostalCode" +txtPostalCode.Text+ "Mobile" +txtMobile.Text+ "RegNo" + txtRegNo.Text;
            EnableControls(true);
            ChangeState(EntryMode.EDIT);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            if(isNew == false)
            {
                LoadDepotInfo(Convert.ToInt32(txtDepotID.Text));
            }
            else
            {
                ClearDepotForm();
            }
            ChangeState(EntryMode.NORMAL);
        }

        private void lvDepot_SelectedIndexChanged(object sender, EventArgs e)
        {


            try
            {

                ClearDepotForm();

                int DepotID = Convert.ToInt32(lvDepot.SelectedItems[0].SubItems[2].Text);

                LoadDepotInfo(DepotID);
                ChangeState(EntryMode.NORMAL);

            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// Loads Depot info according to deport id
        /// </summary>
        /// <param name="depotId"></param>
        private void LoadDepotInfo(int depotId)
        {
            DataTable dt = Depot.GetDepotInfo(depotId);

            foreach (DataRow dr in dt.Rows)
            {

                txtDepotName.Text = dr["DepotName"].ToString();
                txtDepotID.Text = dr["DepotID"].ToString();
                txtCity.Text = dr["City"].ToString();
                txtTelephone.Text = dr["Telephone"].ToString();
                txtContactPerson.Text = dr["ContactPerson"].ToString();
                txtLicenceNo.Text = dr["LicenceNo"].ToString();
                txtAddress.Text = dr["DepotAddress"].ToString();
                txtPostalCode.Text = dr["PostalCode"].ToString();
                txtMobile.Text = dr["Mobile"].ToString();
                txtRegNo.Text = dr["RegNo"].ToString();
                txtRemarks.Text = dr["Remarks"].ToString();


            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("DEPOTE_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }

            //DELETING THE SELECTED Depot

            if (Global.MsgQuest("Are you sure you want to delete the Depot -" + txtDepotName.Text + "?") == DialogResult.No)
                return;

            DataTable DepotTrans = Depot.FindTransactPresent(Convert.ToInt32(txtDepotID.Text));
            if (DepotTrans.Rows.Count != 0)
            {
                Global.Msg("Sorry, Transaction to the depot " + txtDepotName.Text + " has been found. Cannot delete the depot !");
                return;
            }
            else
            {
                m_Depot.Delete(Convert.ToInt32(txtDepotID.Text));
                Global.Msg("Depot - " + txtDepotName.Text + " deleted successfully!");
                ListDepotInfo();
                ClearDepotForm();
            }

        }

        private void ClearDepotForm()
        {

            txtDepotID.Text = "";
            txtDepotName.Text = "";
            txtCity.Text = "";
            txtTelephone.Text = "";
            txtContactPerson.Text = "";
            txtLicenceNo.Text = "";
            txtAddress.Text = "";
            txtPostalCode.Text = "";
            txtMobile.Text = "";
            txtRegNo.Text = "";
            txtRemarks.Text = "";

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            bool chkUserPermission = UserPermission.ChkUserPermission("DEPOTE_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            ClearDepotForm();
            EnableControls(true);
            ChangeState(EntryMode.NEW);
        }

        private void frmDepot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
