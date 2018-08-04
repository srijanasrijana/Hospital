using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DateManager;
using BusinessLogic.Leave;
using BusinessLogic;
using Common;

namespace Attendance_And_Leave
{
    public partial class frmLeaveRequest : Form, IfrmDateConverter
    {
        private bool IsFromDate = false;
        ListItem liLeaveType = new ListItem();
        private int EmployeeID;
        DataTable dtLeaveSetUp;
        public frmLeaveRequest()
        {
            InitializeComponent();
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            IsFromDate = true;//this variable is used as flag to notify which date is selected to change the date converter...coz same funtion is used to change the date  
            DateTime dtDate = Date.ToDotNet(txtFromDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IsFromDate = false;
            DateTime dtDate = Date.ToDotNet(txtToDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }
        public void DateConvert(DateTime DotNetDate)
        {
            if (IsFromDate)//If form date is selected
            {
                txtFromDate.Text = Date.ToSystem(DotNetDate);
            }
            else//IF TO date is selected
            {
                txtToDate.Text = Date.ToSystem(DotNetDate);
            }
        }

        private void frmLeaveRequest_Load(object sender, EventArgs e)
        {
            txtToDate.Mask = Date.FormatToMask();
            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
            txtFromDate.Mask = Date.FormatToMask();
            //txtFromDate.Text = Date.ToSystem(new DateTime(2009, 01, 24));
            txtFromDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            LoadComboboxItems(cmbleavetype);
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtemployeecode_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void txtemployeecode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataTable dtemployee = LeaveRequest.getEmployeeInfoByCode(txtemployeecode.Text);
                if (dtemployee.Rows.Count > 0)
                {
                    DataRow dremployee = dtemployee.Rows[0];
                    lblemployeename.Text = dremployee["FirstName"].ToString() + " " + dremployee["LastName"].ToString();
                    EmployeeID = Convert.ToInt32(dremployee["ID"].ToString());
                }
                else
                {
                    MessageBox.Show("No Employee Found");
                    return;
                }
            }
        }

        private void txtemployeecode_Leave(object sender, EventArgs e)
        {
            DataTable dtemployee = LeaveRequest.getEmployeeInfoByCode(txtemployeecode.Text);
            if (dtemployee.Rows.Count > 0)
            {
                DataRow dremployee = dtemployee.Rows[0];

                lblemployeename.Text = dremployee["FirstName"].ToString() + " " + dremployee["LastName"].ToString();
                EmployeeID = Convert.ToInt32(dremployee["ID"].ToString());
                dtLeaveSetUp = LeaveRequest.getLeaveType();
                LoadLeaveBalance(dtLeaveSetUp, EmployeeID);

            }
            else
            {
                MessageBox.Show("No Employee Found");
                return;
            }
        }
       
        private void LoadComboboxItems(ComboBox comboboxitems)
        {
            if (comboboxitems == cmbleavetype)
            {
                DataTable dtleavesetup = LeaveRequest.getLeaveType();
                foreach (DataRow drleavereq in dtleavesetup.Rows)
                {
                    comboboxitems.Items.Add(new ListItem((int)drleavereq["LeaveSetUpId"], drleavereq["LeaveName"].ToString()));

                }
                comboboxitems.DisplayMember = "value";
                comboboxitems.ValueMember = "id";
            }
        }

        private void btnrequestleave_Click(object sender, EventArgs e)
        {
            liLeaveType = (ListItem)cmbleavetype.SelectedItem;
            int leavetypeid=liLeaveType.ID;
            string leavetypename=liLeaveType.Value;
            bool ishalfleave;
            if(chkhalfleave.Checked)
                ishalfleave=true;
            else
                ishalfleave=false;
            bool isinserted=LeaveRequest.InsertLeaveRequest(EmployeeID,leavetypeid,leavetypename,Date.ToDotNet(txtFromDate.Text),Date.ToDotNet(txtToDate.Text),ishalfleave,rtxtreason.Text);
            if (isinserted == true)
                MessageBox.Show("Leave Request Applied Successfully ");
            else
                MessageBox.Show("Failed LeaveRequest");
        }
        private void LoadLeaveBalance(DataTable dtleavebalance,int empid)
        {
            grdleaverequest.Rows.Clear();
            grdleaverequest.Redim(1, 4);
            writeHeader();
            DateTime TodaysDate = Convert.ToDateTime(Date.ToSystem(Date.GetServerDate()));
            string Year = TodaysDate.Year.ToString();
            string month = TodaysDate.Month.ToString();
           
            string MonthName = "";
            if (month == "1")
                MonthName = "Baisakh";
            else if(month=="2")
                MonthName = "Jestha";
            else if (month == "3")
                MonthName = "Asadh";
            else if (month == "4")
                MonthName = "Shrawan";
            else if (month == "5")
                MonthName = "Bhadra";
            else if (month == "6")
                MonthName = "Aswin";
            else if (month == "7")
                MonthName = "Kartik";
            else if (month == "8")
                MonthName = "Mangsir";
            else if (month == "9")
                MonthName = "Poush";
            else if (month == "10")
                MonthName = "Magh";
            else if (month == "11")
                MonthName = "Falgun";
            else if (month == "12")
                MonthName = "Chaitra";

            int MonthEndValue = LeaveRequest.GetMonthEnd(Convert.ToInt32(Year),MonthName);
            //MessageBox.Show(MonthEndValue.ToString());
             string StartMonthDate="";
            string EndMonthDate ="";
            if (Convert.ToInt32(month) < 9)
            {
                StartMonthDate = Year + "/" + "0" + month + "/" + "01";
                EndMonthDate = Year + "/" + "0" + month + "/" + MonthEndValue;
            }
            else
            {
                StartMonthDate = Year + "/" + month + "/" + "01";
                EndMonthDate = Year + "/" +  month + "/" + MonthEndValue;
            }
           
            for (int i = 0; i < dtleavebalance.Rows.Count; i++)
            {
                int rowcount = grdleaverequest.Rows.Count;
                grdleaverequest.Rows.Insert(rowcount);
                DataRow drleavebalance = dtleavebalance.Rows[i];
                grdleaverequest[i+1, 0] = new SourceGrid.Cells.Cell(drleavebalance["LeaveSetUpID"].ToString());
                grdleaverequest[i+1, 1] = new SourceGrid.Cells.Cell(drleavebalance["LeaveName"].ToString());

                //string fromdate =Date.ToDB( Date.ToDotNet(StartMonthDate));
                //string todate = Date.ToDB(Date.ToDotNet(EndMonthDate));
               // MessageBox.Show(fromdate);
                DataTable dt = LeaveRequest.getApprovedLeave(empid, Convert.ToInt32(drleavebalance["LeaveSetUpID"].ToString()), Date.ToDotNet(StartMonthDate), Date.ToDotNet(EndMonthDate));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow drleave in dt.Rows)
                    {
                        DateTime FromDate = Convert.ToDateTime(drleave["FromDate"].ToString());
                        DateTime ToDate = Convert.ToDateTime(drleave["ToDate"].ToString());
                        TimeSpan TotalLeave = FromDate - ToDate;
                        double NumbeofDays = TotalLeave.TotalDays + 1;
                        double limit = Convert.ToDouble(drleavebalance["Limit"].ToString());
                        double Remaining = limit - NumbeofDays;
                        grdleaverequest[i + 1, 2] = new SourceGrid.Cells.Cell(Remaining.ToString());
                        grdleaverequest[i + 1, 3] = new SourceGrid.Cells.Cell(NumbeofDays.ToString());

                    }
                }
                else
                {
                    double limit = Convert.ToDouble(drleavebalance["Limit"].ToString());
                    grdleaverequest[i + 1, 2] = new SourceGrid.Cells.Cell(limit.ToString());
                    grdleaverequest[i + 1, 3] = new SourceGrid.Cells.Cell(0.ToString());
                }
            }
        }
        private void writeHeader()
        {
            grdleaverequest[0, 0] = new MyHeader("LeaveTypeID");
            grdleaverequest[0, 1] = new MyHeader("Leave Name");
            grdleaverequest[0, 2] = new MyHeader("Rem. Days");
            grdleaverequest[0, 3] = new MyHeader("Leave Days");

            grdleaverequest[0, 0].Column.Width = 1;
            grdleaverequest[0, 1].Column.Width = 120;
            grdleaverequest[0, 2].Column.Width = 80;
            grdleaverequest[0, 3].Column.Width = 80;


            grdleaverequest[0, 0].Column.Visible = false;
           
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
