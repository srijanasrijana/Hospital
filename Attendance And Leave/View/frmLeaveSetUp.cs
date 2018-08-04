using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using BusinessLogic.Leave;

namespace Attendance_And_Leave
{
    public partial class frmLeaveSetUp : Form
    {
        int leavesetupid;
        private string FilterString = "";
        private DataRow[] drFound;
        private DataTable dTable;
        private bool IsSelected = false;
        private EntryMode m_mode = EntryMode.NORMAL;
        int isAddition;
        public frmLeaveSetUp()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (m_mode)
            {
                case EntryMode.NEW:
                    bool isAccoumulated=false;
                    bool Istransfer=false;
                    if(chkisaccumulated.Checked)
                        isAccoumulated=true;
                    else
                        isAccoumulated=false;

                    if(chktransfernextyear.Checked)
                        Istransfer=true;
                    else
                        Istransfer=false;
                    DataTable dt = LeaveSetup.createLeavesetup(txtleavename.Text,txtcode.Text,cmbleavetype.Text,Convert.ToInt32( txtlimit.Text),cmblimittype.Text,isAccoumulated,Istransfer);
                  //  MessageBox.Show(dt.Rows.Count.ToString());
                    MessageBox.Show("Leave Name Created Successfully");
                    break;

                case EntryMode.EDIT:
                       bool isAccoumulated1=false;
                    bool Istransfer1=false;
                    if(chkisaccumulated.Checked)
                        isAccoumulated=true;
                    else
                        isAccoumulated=false;

                    if(chktransfernextyear.Checked)
                        Istransfer=true;
                    else
                        Istransfer=false;
                    DataTable dt1 = LeaveSetup.updateLeaveSetUP(leavesetupid,txtleavename.Text, txtcode.Text, cmbleavetype.Text, Convert.ToInt32(txtlimit.Text), cmblimittype.Text, isAccoumulated1, Istransfer1);
                  //  MessageBox.Show(dt.Rows.Count.ToString());
                    MessageBox.Show("Leave Name Updated Successfully");
                    break;
                  
            }
            ChangeState(EntryMode.NEW);
            cleartextbox();
            frmLeaveSetUp_Load(sender, e);
        }

        private void frmLeaveSetUp_Load(object sender, EventArgs e)
        {

            dTable = LeaveSetup.getLeaveMasterSetup();
            drFound = dTable.Select(FilterString);
            fillGrid();
            ChangeState(EntryMode.NEW);
            cleartextbox();
            txtleavename.Focus();
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
                    ButtonState(true, false, true, false, true);

                    break;
            }
        }
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
          
        }

        private void EnableControls(bool Enable)
        {
            txtleavename.Enabled = txtcode.Enabled = txtlimit.Enabled = cmbleavetype.Enabled = cmblimittype.Enabled = chkisaccumulated.Enabled = chktransfernextyear.Enabled = Enable;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            cleartextbox();
            ChangeState(EntryMode.NEW);
        }
        private void cleartextbox()
        {
            txtleavename.Clear();
            txtcode.Clear();
            txtlimit.Clear();
        }
         
        private void btnEdit_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.EDIT);
        }

        private void fillGrid()
        {
            grdleavesetup.Rows.Clear();
            grdleavesetup.Redim(drFound.Count() + 1, 5);
            writeHeader();

            for (int i = 1; i <= drFound.Count(); i++)
            {

                DataRow dr = drFound[i - 1];
                grdleavesetup[i, 0] = new SourceGrid.Cells.Cell(dr["LeaveSetUpId"].ToString());
                grdleavesetup[i, 1] = new SourceGrid.Cells.Cell(dr["LeaveName"].ToString());
                grdleavesetup[i, 2] = new SourceGrid.Cells.Cell(dr["Code"].ToString());
                grdleavesetup[i, 3] = new SourceGrid.Cells.Cell(dr["LeaveType"].ToString());
                grdleavesetup[i, 4] = new SourceGrid.Cells.Cell(dr["Limit"].ToString() + " " + dr["LimitType"].ToString());
             

            }
        }
        private void writeHeader()
        {
            grdleavesetup[0, 0] = new MyHeader("LeaveSetupid");
            grdleavesetup[0, 1] = new MyHeader("LeaveName");
            grdleavesetup[0, 2] = new MyHeader("Code");
            grdleavesetup[0, 3] = new MyHeader("LeaveType");
            grdleavesetup[0, 4] = new MyHeader("Limit");
            


            grdleavesetup[0, 0].Column.Width = 1;
            grdleavesetup[0, 1].Column.Width = 100;
            grdleavesetup[0, 2].Column.Width = 80;
            grdleavesetup[0, 3].Column.Width = 100;
            grdleavesetup[0, 4].Column.Width = 100;

            grdleavesetup[0, 0].Column.Visible = false;
           

        }
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 9, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;
                AutomaticSortEnabled = false;
            }
        }
      
        private void fillLeaveInformation(int ID)
        {
            DataTable dt = LeaveSetup.getLeaveSetUpByID(ID);
            DataRow dr = dt.Rows[0];
            txtleavename.Text = dr["LeaveName"].ToString();
            txtcode.Text = dr["Code"].ToString();
            cmbleavetype.Text = dr["LeaveType"].ToString();
            txtlimit.Text = dr["Limit"].ToString();
            cmblimittype.Text = dr["LimitType"].ToString();
            if (dr["IsAccumulated"].ToString() == "True")
            {
                chkisaccumulated.Checked = true;
            }
            else
            {
                chkisaccumulated.Checked = false;
            }
            if (dr["IsNextYearTransfer"].ToString() == "True")
            {
                chktransfernextyear.Checked = true;
            }
            else
            {
                chktransfernextyear.Checked = false;
            }
        }

        private void grdleavesetup_Click_1(object sender, EventArgs e)
        {
            IsSelected = true;
            int currentRow = grdleavesetup.Selection.GetSelectionRegion().GetRowsIndex()[0];
            SourceGrid.CellContext cell = new SourceGrid.CellContext(grdleavesetup, new SourceGrid.Position(currentRow, 0));
            int id = Convert.ToInt32(grdleavesetup[currentRow, 0].Value.ToString());
            leavesetupid = id;
            fillLeaveInformation(id);
            ButtonState(true, true, false, true, true);
            ChangeState(EntryMode.NORMAL);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = LeaveSetup.deleteFromLeaveSetup(leavesetupid);
            MessageBox.Show("record Deleted");
            cleartextbox();
            frmLeaveSetUp_Load(sender, e);
        }

    }
}
