using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic.Leave;
using DateManager;

namespace Attendance_And_Leave
{
    public partial class frmleaveapproval : Form
    {
        private string FilterString = "";
        private DataRow[] drFound;
        private DataTable dTable;
        SourceGrid.Cells.CheckBox ishalfleave;
        SourceGrid.Cells.Controllers.CustomEvents evtSave = new SourceGrid.Cells.Controllers.CustomEvents();
        public frmleaveapproval()
        {
            InitializeComponent();
        }

        private void frmleaveapproval_Load(object sender, EventArgs e)
        {
            dTable = LeaveApproval.getLeaveRequest();
            drFound = dTable.Select(FilterString);
            evtSave.Click += new EventHandler(Save_Row_Click);
            fillGrid();
        }
        private void Save_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Save the Leave Request?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
             bool isapp=false;
             if (grdleaveapproval[ctx.Position.Row, 8].Value == "Approved")
                 isapp = true;
             else
                 isapp = false;
                   
            DataTable dtleaverequest = LeaveApproval.updateLeaveRequest(Convert.ToInt32(grdleaveapproval[ctx.Position.Row, 1].Value), isapp);
            MessageBox.Show("Leave Request Saved Successfully");
           // frmleaveapproval_Load(sender,e);
          
        }
        private void fillGrid()
        {
            grdleaveapproval.Rows.Clear();
            grdleaveapproval.Redim(1,10);
            writeHeader();
            FillAllRequest();
        }
        private void FillAllRequest()
        {
            for (int i = 1; i <= drFound.Count(); i++)
            {
                DataRow drleaverequest = drFound[i - 1];
                int rowCount = grdleaveapproval.Rows.Count;
                grdleaveapproval.Rows.Insert(rowCount);

                grdleaveapproval[i , 0] = new SourceGrid.Cells.Cell(drleaverequest["LeaveRequestID"].ToString());
                grdleaveapproval[i , 1] = new SourceGrid.Cells.Cell(drleaverequest["EmployeeID"].ToString());
                grdleaveapproval[i , 2] = new SourceGrid.Cells.Cell(drleaverequest["EmployeeName"].ToString());
                grdleaveapproval[i , 3] = new SourceGrid.Cells.Cell(drleaverequest["LeaveTypeName"].ToString());

                ishalfleave = new SourceGrid.Cells.CheckBox();
                grdleaveapproval[i , 4] = ishalfleave;
                if (drleaverequest["IsHalfLeave"].ToString() == "True")
                {
                    grdleaveapproval[i , 4].Value = true;
                }
                else if (drleaverequest["IsHalfLeave"].ToString() == "False")
                {
                    grdleaveapproval[i , 4].Value = false;
                }

                grdleaveapproval[i, 5] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime( drleaverequest["FromDate"].ToString())));
                grdleaveapproval[i, 6] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime(drleaverequest["ToDate"].ToString())));
                grdleaveapproval[i, 7] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime(drleaverequest["LeaveAppliedDate"].ToString())));

                SourceGrid.Cells.Editors.ComboBox txtStatus = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                txtStatus.StandardValues = new string[] { "Approved", "Declined" };
                txtStatus.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                txtStatus.EditableMode = SourceGrid.EditableMode.Focus;
                string status = ""; 
                if (drleaverequest["isapproved"].ToString() == "True")
                {
                    status = "Approved";
              
                }
                else if (drleaverequest["isapproved"].ToString() == "False")
                {
                    status = "Declined";
         
                }
                grdleaveapproval[i, 8] = new SourceGrid.Cells.Cell(status, txtStatus);
               //grdleaveapproval[i, 8].AddController(evtStatusFocusLost);

                SourceGrid.Cells.Button btnSave = new SourceGrid.Cells.Button("Save");
                grdleaveapproval[i, 9] = btnSave;
                grdleaveapproval[i, 9].AddController(evtSave);
            }
         
        }
        private void writeHeader()
        {
            grdleaveapproval[0, 0] = new MyHeader("LeaveRequestID");
            grdleaveapproval[0, 1] = new MyHeader("EmployeeID");
            grdleaveapproval[0, 2] = new MyHeader("Staff Name");
            grdleaveapproval[0, 3] = new MyHeader("Leave Type");
            grdleaveapproval[0, 4] = new MyHeader("Half Leave");
            grdleaveapproval[0, 5] = new MyHeader("From Date");
            grdleaveapproval[0, 6] = new MyHeader("To Date");
            grdleaveapproval[0, 7] = new MyHeader("Applied Date");
            grdleaveapproval[0, 8] = new MyHeader("Status");
            grdleaveapproval[0, 9] = new MyHeader(" ");


            grdleaveapproval[0, 0].Column.Width = 1;
            grdleaveapproval[0, 1].Column.Width = 1;
            grdleaveapproval[0, 2].Column.Width = 100;
            grdleaveapproval[0, 3].Column.Width = 100;
            grdleaveapproval[0, 4].Column.Width = 100;
            grdleaveapproval[0, 5].Column.Width = 100;
            grdleaveapproval[0, 6].Column.Width = 100;
            grdleaveapproval[0, 7].Column.Width = 100;
            grdleaveapproval[0, 8].Column.Width = 100;
            grdleaveapproval[0, 9].Column.Width = 50;

            grdleaveapproval[0, 0].Column.Visible = false;
            grdleaveapproval[0, 1].Column.Visible = false;


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
    }
}
