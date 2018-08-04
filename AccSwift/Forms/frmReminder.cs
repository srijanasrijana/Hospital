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
using DateManager;
using Common;

namespace AccSwift
{
    public partial class frmReminder : Form, IfrmDateConverter, IfrmRecurrence
    {
        private bool isStartDate = false;
        private EntryMode m_mode = EntryMode.NORMAL;
        private int ReminderID = 0;
        DataTable dtFromRecurrence = new DataTable();
        ArrayList listOfSelectedUsers = new ArrayList();
        ListItem listItemOfSelectedUsers = new ListItem();
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register

        public frmReminder()
        {
            InitializeComponent();
        }

        public void Recurrence(DataTable retRecurrenceTable)
        {
            dtFromRecurrence = retRecurrenceTable;
            if (dtFromRecurrence.Rows.Count == 0)
                chkReoccurance.Checked = true;
            else
                chkReoccurance.Checked = true;

            //DataRow dr = dtFromRecurrence.Rows[0];
            //MessageBox.Show(dr["OccurenceDateStart"].ToString() + " " + dr["OccurenceDateEnd"].ToString() + " " + dr["OccurencePattern"].ToString());
        }

        public void DateConvert(DateTime DotNetDate)
        {
            if (isStartDate)
                txtDate.Text = Date.ToSystem(DotNetDate);
        }

        private void frmReminder_Load(object sender, EventArgs e)
        {
            //LoadCombobox
            ChangeState(EntryMode.NEW);
            LoadcboStatus(cboStatus);
            LoadcboPriority(cboPriority);
            LoadcboAsignTo(cboAsignTo);
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.ToSystem(Date.GetServerDate());

            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grdReminderList_DoubleClick);
            grdReminderList.SelectionMode = SourceGrid.GridSelectionMode.Row;


            //Disable multiple selection
            grdReminderList.Selection.EnableMultiSelection = false;
            grdReminderList.Redim(1, 7);
            grdReminderList.FixedRows = 1;
            //Prepare the header part for grid
            AddGridHeader();
            //AddRowReminder(1);           
            LoadAllReminder();

        }

        private void LoadcboStatus(ComboBox cboBox)
        {
            cboBox.Items.Clear();
            cboBox.Items.Add(new ListItem((0), "Inactive"));
            cboBox.Items.Add(new ListItem((1), "Active"));
            cboBox.SelectedIndex = 1;
            cboBox.DisplayMember = "value";
            cboBox.ValueMember = "id";
        }

        private void LoadcboPriority(ComboBox cboBox)
        {
            cboBox.Items.Clear();
            cboBox.Items.Add(new ListItem((0), "Low"));
            cboBox.Items.Add(new ListItem((1), "Normal"));
            cboBox.Items.Add(new ListItem((2), "High"));
            cboBox.SelectedIndex = 1;
            cboBox.DisplayMember = "value";
            cboBox.ValueMember = "id";
        }

        private void LoadAllUser()
        {
            chkUserList.Items.Clear();
            DataTable dtAllUser = User.GetUserInfo(0);
            foreach (DataRow drUser in dtAllUser.Rows)
            {
                if (cboAsignTo.SelectedIndex == 0)
                    chkUserList.Items.Add(new ListItem(Convert.ToInt32(drUser["UserID"]), drUser["UserName"].ToString()), true);
                else
                    chkUserList.Items.Add(new ListItem(Convert.ToInt32(drUser["UserID"]), drUser["UserName"].ToString()));
            }


        }

        private void LoadcboAsignTo(ComboBox cboAssignTo)
        {
            cboAsignTo.Items.Clear();
            cboAsignTo.Items.Add(new ListItem((0), "All"));
            cboAsignTo.Items.Add(new ListItem((1), "Self"));
            cboAsignTo.Items.Add(new ListItem((0), "Selected"));
            cboAsignTo.SelectedIndex = 1;
            cboAsignTo.DisplayMember = "value";
            cboAsignTo.ValueMember = "id";
        }

        /// <summary>
        /// Writes the header part for grdCompanyList
        /// </summary>
        private void AddGridHeader()
        {
            grdReminderList[0, 0] = new SourceGrid.Cells.ColumnHeader("S.N.");
            grdReminderList[0, 1] = new SourceGrid.Cells.ColumnHeader("Subject");
            grdReminderList[0, 2] = new SourceGrid.Cells.ColumnHeader("Status");
            grdReminderList[0, 3] = new SourceGrid.Cells.ColumnHeader("Priority");
            grdReminderList[0, 4] = new SourceGrid.Cells.ColumnHeader("Date");
            grdReminderList[0, 5] = new SourceGrid.Cells.ColumnHeader("Assigned To");
            grdReminderList[0, 6] = new SourceGrid.Cells.ColumnHeader("ReminderID");
            //grdReminderList[0, 7] = new SourceGrid.Cells.ColumnHeader("Category");            

            grdReminderList[0, 0].Column.Width = 50;
            grdReminderList[0, 1].Column.Width = 200;
            grdReminderList[0, 2].Column.Width = 100;
            grdReminderList[0, 3].Column.Width = 100;
            grdReminderList[0, 4].Column.Width = 100;
            grdReminderList[0, 5].Column.Width = 150;
            grdReminderList[0, 6].Column.Width = 50;
            //grdReminderList[0, 7].Column.Width = 75;
            grdReminderList.Columns[6].Visible = false;
        }

        /// <summary>
        /// Add a single row in the grid
        /// </summary>
        /// <param name="RowCount"></param>
        private void AddRowReminder(int RowCount)
        {
            //Add a new row
            grdReminderList.Redim(Convert.ToInt32(RowCount + 1), grdReminderList.ColumnsCount);
            int i = RowCount;
            grdReminderList[i, 0] = new SourceGrid.Cells.Cell(i.ToString());

            SourceGrid.Cells.Editors.TextBox txtSubject = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtSubject.EditableMode = SourceGrid.EditableMode.None;
            grdReminderList[i, 1] = new SourceGrid.Cells.Cell("", txtSubject);

            SourceGrid.Cells.Editors.ComboBox cboStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboStatus.StandardValues = new string[] { "Active", "Inactive" };
            cboStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.EditableMode = SourceGrid.EditableMode.Focus;
            string strStatus = "Active";
            grdReminderList[i, 2] = new SourceGrid.Cells.Cell(strStatus, cboStatus);

            SourceGrid.Cells.Editors.ComboBox cboPriority = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            cboPriority.StandardValues = new string[] { "Low", "Normal", "High" };
            cboPriority.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPriority.EditableMode = SourceGrid.EditableMode.Focus;
            string strPriority = "Normal";
            grdReminderList[i, 3] = new SourceGrid.Cells.Cell(strPriority, cboPriority);

            SourceGrid.Cells.Editors.TextBox txtDate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtDate.EditableMode = SourceGrid.EditableMode.None;
            grdReminderList[i, 4] = new SourceGrid.Cells.Cell("", txtDate);

            SourceGrid.Cells.Editors.TextBox txtModified = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtModified.EditableMode = SourceGrid.EditableMode.None;
            grdReminderList[i, 5] = new SourceGrid.Cells.Cell("", txtModified);

            SourceGrid.Cells.Editors.TextBox txtReminderID = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtReminderID.EditableMode = SourceGrid.EditableMode.None;
            grdReminderList[i, 6] = new SourceGrid.Cells.Cell("", txtReminderID);

            //SourceGrid.Cells.Editors.TextBox txtCategory = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            //txtCategory.EditableMode = SourceGrid.EditableMode.None;
            //grdReminderList[i, 7] = new SourceGrid.Cells.Cell("", txtCategory);
        }

        private void LoadAllReminder()
        {
            DataTable dt = Reminder.SelectReminder(0);
            DataTable dtCheckTotalUser = User.GetUserInfo(0);
            if (dt.Rows.Count == 0)
                return;
            foreach (DataRow dr in dt.Rows)
            {
                int i = grdReminderList.Rows.Count;
                AddRowReminder(i);
                grdReminderList[i, 0].Value = i.ToString();
                grdReminderList[i, 1].Value = dr["Subject"].ToString();
                int statusValue = Convert.ToInt32(dr["Status"]);
                switch (statusValue)
                {
                    case 0: grdReminderList[i, 2].Value = "Inactive";
                        break;
                    case 1: grdReminderList[i, 2].Value = "Active";
                        break;
                }
                int priorityValue = Convert.ToInt32(dr["Priority"]);
                switch (priorityValue)
                {
                    case 0: grdReminderList[i, 3].Value = "Low";
                        break;
                    case 1: grdReminderList[i, 3].Value = "Normal";
                        break;
                    case 2: grdReminderList[i, 3].Value = "High";
                        break;
                }
               // grdReminderList[i, 4].Value =dr["Date"].ToString();
                grdReminderList[i, 4].Value =Date.ToSystem( Convert.ToDateTime( dr["Date"].ToString())).ToString();
                grdReminderList[i, 6].Value = dr["ReminderID"].ToString();
                // Get the user list
                ArrayList checkSelectedUser = Reminder.GetReminderUserDetail(Convert.ToInt32(dr["ReminderID"]));
                foreach (int UserIDList in checkSelectedUser)
                {
                    //Do unique task for the current user 
                    //ie. if successful login username and id must be in global 
                    // so read it from global
                }
                //if (checkSelectedUser.Count == 1)
                //{
                //    //check current user
                //    foreach (int currentUser in checkSelectedUser)
                //    {
                //        //if (currentUser == User.GetCurrUser())
                //        //{
                //        //    grdReminderList[i, 5].Value = "Self";
                //        //}
                //    }
                //}
                if (checkSelectedUser.Count == dtCheckTotalUser.Rows.Count)
                {
                    grdReminderList[i, 5].Value = "All";
                }

                else
                {
                    grdReminderList[i, 5].Value = "Selected";
                }
                //grdReminderList[i, 5].Value = dr["UserName"].ToString();              

            }//End for loop   

        }

        private void grdReminderList_DoubleClick(object sender, EventArgs e)
        {

            //try
            //{
            //    //Get the Selected Row           
            //    int CurRow = grdReminderList.Selection.GetSelectionRegion().GetRowsIndex()[0];
            //    SourceGrid.CellContext ReminderID = new SourceGrid.CellContext(grdReminderList, new SourceGrid.Position(CurRow, 6));
            //    int ID = Convert.ToInt32(ReminderID.Value);
            //    DataTable dt = Reminder.SelectReminder(ID);
            //    DataRow dr = dt.Rows[0];
            //    txtSubject.Text = dr["Subject"].ToString();
            //   // txtStartDate.Text = Date.ToSystem(dr["StartDate"]);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void chkReoccurance_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (m_mode == EntryMode.NEW)
                ReminderID = 0;
            else
                ReminderID = Convert.ToInt32(txtReminderID.Text);
            //Get the datatable from the recurrence table
            dt = Reminder.GetRecurrenceDetail(ReminderID);
            frmReoccurance frm = new frmReoccurance(this, dt);
            if (!frm.IsDisposed)
                frm.ShowDialog();
        }

        private void btnStartDate_Click(object sender, EventArgs e)
        {
            isStartDate = true;
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtDate.Text));
            frm.ShowDialog();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("REMINDER_CREATE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create Reminder. Please contact your administrator for permission.");
                return;
            }
            this.Height = 500;
            ClearReminderDetail();
            ChangeState(EntryMode.NEW);
        }

        //Clears the text of every field of reminder form
        private void ClearReminderDetail()
        {
            txtSubject.Text = string.Empty;
            txtDescription.Text = string.Empty;
            LoadcboStatus(cboStatus);
            LoadcboPriority(cboPriority);
            LoadcboAsignTo(cboAsignTo);
            txtDate.Mask = Date.FormatToMask();
            txtDate.Text = Date.ToSystem(Date.GetServerDate());
            grdReminderList.Redim(1, 7);
            AddGridHeader();
            LoadAllReminder();
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
            txtSubject.Enabled = txtDate.Enabled = txtDescription.Enabled = btnStartDate.Enabled = cboStatus.Enabled = cboPriority.Enabled = chkReoccurance.Enabled = chkUserList.Enabled = cboAsignTo.Enabled = Enable;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("REMINDER_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify Reminder. Please contact your administrator for permission.");
                return;
            }
            EnableControls(true);
            ChangeState(EntryMode.EDIT);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //validate all fields
            if (txtSubject.Text == "")
            {
                Global.MsgError("Please enter subject name!");
                txtSubject.Focus();
                return;
            }
            //Save to the system.tblReminder with recurrence ID
            ListItem liStatus = new ListItem();
            liStatus = (ListItem)cboStatus.SelectedItem;
            ListItem liPriority = new ListItem();
            liPriority = (ListItem)cboPriority.SelectedItem;
            listOfSelectedUsers.Clear();
            for (int i = 0; i < chkUserList.CheckedItems.Count; i++)
            {
                listItemOfSelectedUsers = (ListItem)chkUserList.CheckedItems[i];
                listOfSelectedUsers.Add(listItemOfSelectedUsers.ID);
            }
            int ReminderID = 0;
            //there must be one user checked
            if (listOfSelectedUsers.Count == 0)
            {
                MessageBox.Show("You must have to asign atleast one user!!");
                return;
            }
            //default recurrence may be  null

            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW: //if new button is pressed
                    ReminderID = Reminder.CreateReminder(txtSubject.Text.Trim(), liStatus.ID, liPriority.ID, Date.ToDotNet(txtDate.Text), txtDescription.Text.Trim());
                   
                    //save the user in tblUserReminder                       
                    Reminder.CreateRecurrenceUser(listOfSelectedUsers, ReminderID);
                    //now save the reminder here
                    string result = Reminder.CreateRecurrence(dtFromRecurrence, ReminderID);
                    if (result == "SUCCESS")
                        MessageBox.Show("Reminder is successfully set!!");
                    if (result == "FAILURE")
                        MessageBox.Show("Failed to set reminder!!");
                    break;
                #endregion

                #region EDIT
                case EntryMode.EDIT:
                    ReminderID = Reminder.ModifyReminder(Convert.ToInt32(txtReminderID.Text), txtSubject.Text.Trim(), liStatus.ID, liPriority.ID, Date.ToDotNet(txtDate.Text), txtDescription.Text.Trim());
                    Reminder.DeleteReminderUser(ReminderID);
                    Reminder.DeleteRecurrence(ReminderID);
                    Reminder.CreateRecurrenceUser(listOfSelectedUsers, ReminderID);
                    string editResult = Reminder.CreateRecurrence(dtFromRecurrence, ReminderID);
                    if (editResult == "SUCCESS")
                        MessageBox.Show("Reminder is successfully set!!");
                    if (editResult == "FAILURE")
                        MessageBox.Show("Failed to set reminder!!");
                    break;
                #endregion
            }

            ClearReminderDetail();
            //reload the grdReminderList           
            //LoadAllReminder();
        }

        private void grdReminderList_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the Selected Row  
                ChangeState(EntryMode.NORMAL);
                if (grdReminderList.Rows.Count == 1)
                    return;
                int CurRow = grdReminderList.Selection.GetSelectionRegion().GetRowsIndex()[0];
                SourceGrid.CellContext ReminderID = new SourceGrid.CellContext(grdReminderList, new SourceGrid.Position(CurRow, 6));
                int ID = Convert.ToInt32(ReminderID.Value);
                DataTable dt = Reminder.SelectReminder(ID);
                DataRow dr = dt.Rows[0];
                txtReminderID.Text = dr["ReminderID"].ToString();
                txtSubject.Text = dr["Subject"].ToString();
                txtDate.Text = Date.DBToSystem(dr["Date"].ToString());
                cboStatus.SelectedIndex = Convert.ToInt32(dr["Status"]);
                cboPriority.SelectedIndex = Convert.ToInt32(dr["Priority"]);
                txtDescription.Text = dr["Description"].ToString();
                //Aslo get the User List
                listOfSelectedUsers = Reminder.GetReminderUserDetail(Convert.ToInt32(txtReminderID.Text));
                cboAsignTo.SelectedIndex = 2;
                chkUserList.ClearSelected();
                foreach (int listofuserToCheck in listOfSelectedUsers)
                {
                    for (int i = 0; i < chkUserList.Items.Count; i++)
                    {
                        ListItem listOfAllUsers = (ListItem)chkUserList.Items[i];
                        if (listOfAllUsers.ID == listofuserToCheck)
                        {
                            chkUserList.SetItemChecked(i, true);
                        }
                    }
                }
                //Also get the recurrence ID if exist
                if (Reminder.IsRecurrenceExist(Convert.ToInt32(dr["ReminderID"])))
                {
                    //chkReoccurance.Checked = true;
                    dtFromRecurrence = Reminder.GetRecurrenceDetail(Convert.ToInt32(txtReminderID.Text));
                    //if (dtFromRecurrence.Rows.Count == 0)
                    //    chkReoccurance.Checked = false;
                    //else
                    //    chkReoccurance.Checked = true;
                }
                else
                {
                    // chkReoccurance.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void grdReminderList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("REMINDER_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete Reminder. Please contact your administrator for permission.");
                return;
            }

            if (Global.MsgQuest("Are you sure to delete reminder?") == DialogResult.Yes)
            {
                Reminder.DeleteRecurrence(Convert.ToInt32(txtReminderID.Text));
                Reminder.DeleteReminderUser(Convert.ToInt32(txtReminderID.Text));
                Reminder.DeleteReminder(Convert.ToInt32(txtReminderID.Text));
                ClearReminderDetail();
            }
        }

        private void cboAsignTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAsignTo.SelectedIndex == 0)
            {
                LoadAllUser();
                chkUserList.Enabled = false;
            }
            if (cboAsignTo.SelectedIndex == 1)
            {
                chkUserList.Items.Clear();
                // User.getCurrenctUserInfo();
                //chkUserList.Items.Add(new ListItem(Convert.ToInt32(drUser["UserID"]), drUser["UserName"].ToString()), true);
                chkUserList.Items.Add(new ListItem(User.CurrUserID, User.GetCurrUser()), true);
                chkUserList.Enabled = false;
            }
            if (cboAsignTo.SelectedIndex == 2)
            {
                LoadAllUser();
                chkUserList.Enabled = true;
            }
        }

        private void grdReminderList_DoubleClick_1(object sender, EventArgs e)
        {
            this.Height = 500;
            if (grdReminderList.Rows.Count == 1)
                return;
        }

    }
}
