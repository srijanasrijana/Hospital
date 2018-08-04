using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using System.Threading;
using CrystalDecisions.Shared;

using HRM.Reports;
using BusinessLogic.HRM;
using Common;

namespace HRM
{
    public partial class frmEmployeeRegistration : Form, IfrmDateConverter
    {
        private int earningRowCount;
        private int deductionRowCount;
        int employeeID = 0;
        string FileName = " ";
        private int prntDirect = 0;
        private bool isSalaryGenerated = false;
        private int monthID;
        private double TotalEarning = 0;
        private double TotalDeduction = 0;
        private double NetPayable = 0;
        private string employeeledger = "";
        bool ispicupdate = false;
        private byte[] Picture = null;
        private string FilterString = "";
        private DataRow[] drFound;
        private DataTable dTable;
        private EntryMode m_mode = EntryMode.NORMAL;
        private bool IsFieldChanged = false;
        private string DateStatus = "";
        private string GridType = "";
        DataTable dtQualification=new DataTable();
        DataTable dtExperiences=new DataTable();
        DataTable dtLoan = new DataTable();
        DataTable dtAdvance = new DataTable();
        SourceGrid.Cells.Controllers.CustomEvents evtInstituteFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtCompanyFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDeletee = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtJobHistoryDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtRemoveLoan = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtRemoveAdvance = new SourceGrid.Cells.Controllers.CustomEvents();
        private int CurrRowPos = 0;
        ListItem liNationality = new ListItem();
        ListItem liDesignation = new ListItem();
        ListItem liDepartment = new ListItem();
        ListItem liFaculty = new ListItem();
        ListItem liBank = new ListItem();
        ListItem liEthnicity = new ListItem();
        ListItem liDistrict = new ListItem();
        ListItem liZone = new ListItem();
        ListItem liLoan = new ListItem();
        //ListItem liLevel = new ListItem();

        private SourceGrid.Cells.Controllers.CustomEvents dblClick; //Double click event holder
        private SourceGrid.Cells.Controllers.CustomEvents JHClick;
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;
        HRM.Model.dsPaySlip dsPaySlip = new HRM.Model.dsPaySlip();
      
        TextBox txtemployeeID = new TextBox();
        public DataTable FromOpeningBalance = new DataTable();
        public DataTable FromPreYearBalance = new DataTable();
        BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
        FormHandle m_FHandle = new FormHandle();

        private enum CmbType
        {
            //Nationality,
            District,
            Zone,
            Religion,
            Level,
            //Course,
            //Section,
            //Shift,
            //StudentType,
            Ethnicity,
            Loan
        }

        public frmEmployeeRegistration()
        {
            InitializeComponent();
        }
       
        private void TCEmployee_DrawItem(object sender, DrawItemEventArgs e)
        {
            System.Drawing.Graphics g = e.Graphics;
            TabPage tp = TCEmployee.TabPages[e.Index];
            System.Drawing.Brush br = null;
            System.Drawing.Brush colBorder = null;
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
            System.Drawing.RectangleF r;

            System.Drawing.RectangleF rBorder = new System.Drawing.RectangleF(TCEmployee.Left, TCEmployee.Top, TCEmployee.Width, TCEmployee.Height);

            sf.Alignment = System.Drawing.StringAlignment.Center;

            string strTitle = tp.Text;

            if ((e.State == DrawItemState.None))
            {
                System.Drawing.RectangleF rBack = new System.Drawing.RectangleF(TCEmployee.Left, TCEmployee.Top, TCEmployee.Width, tp.Bounds.Top - 2);
                //

                //this is the background color of the tabpage
                //you could make this a stndard color for the selected page
                br = new System.Drawing.SolidBrush(tp.BackColor);

                //this is the background color of the tab page
                g.FillRectangle(br, rBack);

                br.Dispose();
            }

            //If the current index is the Selected Index, change the color
            if (TCEmployee.SelectedIndex == e.Index)
            {
                r = new System.Drawing.RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2);

                //this is the background color of the tabpage
                //you could make this a stndard color for the selected page
                br = new System.Drawing.SolidBrush(tp.BackColor);

                colBorder = new System.Drawing.SolidBrush(System.Drawing.Color.WhiteSmoke);

                //this is the background color of the tab
                g.FillRectangle(br, e.Bounds);

                //this is the background color of the tab page
                //you could make this a stndard color for the selected page
                br.Dispose();
                tp.ForeColor = Color.Purple;
                br = new System.Drawing.SolidBrush(tp.ForeColor);

                r.Offset(0, 1);

                g.DrawString(strTitle, TCEmployee.Font, br, r, sf);

                br.Dispose();

                colBorder.Dispose();


            }
            else
            {
                //these are the standard colors for the unselected tab pages
                br = new System.Drawing.SolidBrush(tp.BackColor);

                r = new System.Drawing.RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height + 2);

                System.Drawing.RectangleF rBack = new System.Drawing.RectangleF(TCEmployee.Left + 1, TCEmployee.Top + e.Bounds.Y + e.Bounds.Height + 3, TCEmployee.Width - 3, TCEmployee.Height - (e.Bounds.Y + e.Bounds.Height + 4));

                //this is the background color of the tab page
                g.FillRectangle(br, rBack);

                //this is the background color of the tab button
                g.FillRectangle(br, r);

                br.Dispose();
                br = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                r.Offset(0, 3);

                g.DrawString(strTitle, TCEmployee.Font, br, r, sf);

                br.Dispose();

            }

            //Dispose objects
            sf.Dispose();
        }

        /// <summary>
        /// Loads Combobox according to CmbType
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="type"></param>
        private void LoadComboBox(ComboBox cbo, CmbType type)
        {
            try
            {
                cbo.Items.Clear();
                DataTable dt = new DataTable();
                switch (type)
                {
                    //case CmbType.Nationality:
                    //    dt = Nationality.GetCboNationality();
                        //break;
                    case CmbType.Zone:
                        dt = Zone.GetZone();
                        break;
                    case CmbType.District:
                        dt = District.GetDistrict();
                        break;
                    case CmbType.Level:
                        dt = Hrm.GetEmpLevelForCmb();
                        break;
                    //case CmbType.Religion:
                    //    dt = Religion.GetReligionForCmb();
                    //    break;
                    //case CmbType.Course:
                    //    dt = Course.GetCourseForCmb();
                    //    break;
                    //case CmbType.Section:
                    //    dt = Section.GetSectionIdValue();
                    //    break;
                    //case CmbType.Shift:
                    //    dt = Shift.GetShiftIdValue();
                    //    break;
                    //case CmbType.StudentType:
                    //    dt = GeneralSetup.GetStdTypeIdValue();
                    //    break;
                    case CmbType.Ethnicity:
                        dt = Ethnicity.GetEthIdValue();
                        break;
                    case CmbType.Loan:
                        dt = Hrm.GetLoanForCmb();
                        break;
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        cbo.Items.Add(new ListItem((int)dr["ID"], dr["Value"].ToString()));

                    }
                    
                    cbo.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        
        private void frmEmployeeRegistration_Load(object sender, EventArgs e)
        {
             try
            {
                TCEmployee.TabPages.Remove(tppayhistory);//removed for PN Campus

                BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
                dTable = employees.EmployeeDetails(" order by StaffCode");
                drFound = dTable.Select(FilterString);
                dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
                dblClick.DoubleClick += new EventHandler(grdemployee_DoubleClick);
                JHClick = new SourceGrid.Cells.Controllers.CustomEvents();
                JHClick.Click += new EventHandler(Delete_JH_Row_Click);
                gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
                gridKeyDown.KeyDown += new KeyEventHandler(Handle_KeyDown);
                grdemployee.Controller.AddController(gridKeyDown);
                //Let the whole row to be selected
                grdemployee.SelectionMode = SourceGrid.GridSelectionMode.Row;
                LoadComboboxItems(cmbNationality);
                LoadComboboxItems(cmbdepartment);
                LoadComboboxItems(cmbdesignation);
                LoadComboboxItems(cmbFaculty);
                LoadComboBox(cmbEthnicity, CmbType.Ethnicity);
                LoadComboBox(cmbPermDistrict, CmbType.District);
                LoadComboBox(cmbPermZone, CmbType.Zone);
                LoadComboBox(cmbTempDistrict, CmbType.District);
                LoadComboBox(cmbTempZone, CmbType.Zone);
                LoadComboBox(cmbLevel, CmbType.Level);
                LoadComboBox(cmbLoan, CmbType.Loan);


                this.TCEmployee.DrawMode = TabDrawMode.OwnerDrawFixed;
                this.TCEmployee.DrawItem += new DrawItemEventHandler(TCEmployee_DrawItem);
                rbtnMale.ForeColor = Color.Black;
                rbtnFemale.ForeColor = Color.Black;
                rbtnOther.ForeColor = Color.Black;
                rbtnSingle.ForeColor = Color.Black;
                rbtnMarried.ForeColor = Color.Black;
                chkCoupleWork.ForeColor = Color.Black;
                btnBrowse.ForeColor = Color.Black;
                btnSendEmail.ForeColor = Color.Black;

                label27.ForeColor = Color.Black;
                label28.ForeColor = Color.Black;
                label29.ForeColor = Color.Black;
                label30.ForeColor = Color.Black;
                label31.ForeColor = Color.Black;
                label32.ForeColor = Color.Black;
                label33.ForeColor = Color.Black;
                label34.ForeColor = Color.Black;
                label35.ForeColor = Color.Black;
                label36.ForeColor = Color.Black;
                label37.ForeColor = Color.Black;
                label38.ForeColor = Color.Black;
                label39.ForeColor = Color.Black;
                label40.ForeColor = Color.Black;
                label41.ForeColor = Color.Black;
                label42.ForeColor = Color.Black;
                label43.ForeColor = Color.Black;
                label44.ForeColor = Color.Black;
                rbtnyes.ForeColor = Color.Black;
                rbtnno.ForeColor = Color.Black;
                label49.ForeColor = Color.Black;

                txtenddate.Text = Date.ToSystem(Date.GetServerDate());
                txtBirthDate.Text = Date.ToSystem(Date.GetServerDate());
                txtjoindate.Text = Date.ToSystem(Date.GetServerDate());
                txtStartDate.Text = Date.ToSystem(Date.GetServerDate());
                txtGrdIncrmtDate.Text = Date.ToSystem(Date.GetServerDate());
                //txtenddate.Text = Date.ToSystem(Date.GetServerDate().AddYears(2));

                //txtDatetime for Loan
                txtAdvEndDate.Text = Date.ToSystem(Date.GetServerDate().AddYears(1));
                txtAdvStartDate.Text = Date.ToSystem(Date.GetServerDate());
                txtLoanEndDate.Text = Date.ToSystem(Date.GetServerDate());
                txtLoanStartDate.Text = Date.ToSystem(Date.GetServerDate());

                btnNew.Enabled = false;
                btnEdit.Enabled = false;
                //Load Bank List in salary and grade
                LoadComboboxItems(cmbbankname);


                //Load Event Handeller   
                evtInstituteFocusLost.FocusLeft += new EventHandler(evtInstituteFocusLost_FocusLeft);
                evtCompanyFocusLost.FocusLeft += new EventHandler(evtCompanyFocusLost_FocusLeft);
                evtDelete.Click += new EventHandler(Delete_Row_Click);
                evtDeletee.Click += new EventHandler(workexperienceDelete_Row_Click);

                panel3.Visible = false;
                label25.Visible = false;
                grdjobhistory.Visible = false;
                FillEmployee();
                FillEducation();
                FillExperience();
                FillLoan();
                FillAdvance();
                ChangeState(EntryMode.NEW);

                cmbEmpType.SelectedIndex = 0;

            }
             catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            
        }

      
        public static void DrawTabText(TabControl tabControl, DrawItemEventArgs e, string caption)
        {
            Color backColor = (Color)System.Drawing.SystemColors.Control;
            Color foreColor = (Color)System.Drawing.SystemColors.ControlText;
            DrawTabText(tabControl, e, backColor, foreColor, caption);
        }
        public static void DrawTabText(TabControl tabControl, DrawItemEventArgs e, System.Drawing.Color backColor, System.Drawing.Color foreColor, string caption)
        {
            #region setup
            Font tabFont;
            Brush foreBrush = new SolidBrush(foreColor);
            Rectangle r = e.Bounds;
            Brush backBrush = new SolidBrush(backColor);
            string tabName = tabControl.TabPages[e.Index].Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            #endregion

            #region drawing
            e.Graphics.FillRectangle(backBrush, r);


            r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
            if (e.Index == tabControl.SelectedIndex)
            {
                tabFont = new Font(e.Font, FontStyle.Italic);
                tabFont = new Font(tabFont, FontStyle.Bold);
            }
            else
            {
                tabFont = e.Font;
            }

            e.Graphics.DrawString(caption, tabFont, foreBrush, r, sf);
            #endregion

            #region cleanup
            sf.Dispose();
            if (e.Index == tabControl.SelectedIndex)
            {
                tabFont.Dispose();
                backBrush.Dispose();
            }
            else
            {
                backBrush.Dispose();
                foreBrush.Dispose();
            }
            #endregion

        }
        public static void DrawTabText(TabControl tabControl, DrawItemEventArgs e, System.Drawing.Color foreColor, string caption)
        {
            Color backColor = (Color)System.Drawing.SystemColors.Control;
            DrawTabText(tabControl, e, backColor, foreColor, caption);
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }
        private void FillEmployee()
        {
            GridType = "EMPLOYEE";
            grdemployee.Rows.Clear();
            grdemployee.Redim(drFound.Count() + 1, 3);
            WriteHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {
                DataRow dr = drFound[i - 1];
                grdemployee[i, 0] = new SourceGrid.Cells.Cell(dr["ID"].ToString());
                grdemployee[i, 1] = new SourceGrid.Cells.Cell(dr["StaffCode"].ToString());
                grdemployee[i, 1].AddController(dblClick);

                grdemployee[i, 2] = new SourceGrid.Cells.Cell(dr["EmployeeName"].ToString());
                grdemployee[i, 2].AddController(dblClick);
                grdemployee[i, 2].AddController(gridKeyDown);
              }

        
        }
        private void WriteHeader()
        {
            if (GridType == "EMPLOYEE")
            {

                grdemployee[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
                grdemployee[0, 1] = new SourceGrid.Cells.ColumnHeader("Code");
                grdemployee[0, 2] = new SourceGrid.Cells.ColumnHeader("Staff Name");

                grdemployee[0, 0].Column.Width = 1;
                grdemployee[0, 1].Column.Width = 100;
                grdemployee[0, 2].Column.Width = 210;

                grdemployee[0, 0].Column.Visible = false;
            }
            else if (GridType == "EDUCATION")
            {
                grdacademicqualification[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
                grdacademicqualification[0, 1] = new SourceGrid.Cells.ColumnHeader("Name of Institute");
                grdacademicqualification[0, 2] = new SourceGrid.Cells.ColumnHeader("Board");
                grdacademicqualification[0, 3] = new SourceGrid.Cells.ColumnHeader("Level/Course");
                grdacademicqualification[0, 4] = new SourceGrid.Cells.ColumnHeader("Percentage");
                grdacademicqualification[0, 5] = new SourceGrid.Cells.ColumnHeader("Passed Year");

                grdacademicqualification[0, 0].Column.Width = 30;
                grdacademicqualification[0, 1].Column.Width = 350;
                grdacademicqualification[0, 2].Column.Width = 150;
                grdacademicqualification[0, 3].Column.Width = 150;
                grdacademicqualification[0, 4].Column.Width = 70;
                grdacademicqualification[0, 5].Column.Width = 90;
            }
            else if (GridType == "EXPERIENCE")
            {
                grdworkexperience[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
                grdworkexperience[0, 1] = new SourceGrid.Cells.ColumnHeader("Company Name");
                grdworkexperience[0, 3] = new SourceGrid.Cells.ColumnHeader("From Date");
                grdworkexperience[0, 4] = new SourceGrid.Cells.ColumnHeader("To Date");
                grdworkexperience[0, 2] = new SourceGrid.Cells.ColumnHeader("Designation");

                grdworkexperience[0, 0].Column.Width = 30;
                grdworkexperience[0, 1].Column.Width = 350;
                grdworkexperience[0, 3].Column.Width = 100;
                grdworkexperience[0, 4].Column.Width = 100;
                grdworkexperience[0, 2].Column.Width = 150;
            }
            if (GridType == "JOBHISTORY")
            {
                grdjobhistory[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
                grdjobhistory[0, 1] = new SourceGrid.Cells.ColumnHeader("Join Date");
                grdjobhistory[0, 2] = new SourceGrid.Cells.ColumnHeader("End Date");
                grdjobhistory[0, 3] = new SourceGrid.Cells.ColumnHeader("Department");
                grdjobhistory[0, 4] = new SourceGrid.Cells.ColumnHeader("Designation");
                grdjobhistory[0, 5] = new SourceGrid.Cells.ColumnHeader("Faculty");
                grdjobhistory[0, 6] = new SourceGrid.Cells.ColumnHeader("Status");
                grdjobhistory[0, 7] = new SourceGrid.Cells.ColumnHeader("Type");
                grdjobhistory[0, 8] = new SourceGrid.Cells.ColumnHeader("ID");

                grdjobhistory[0, 0].Column.Width = 30;
                grdjobhistory[0, 1].Column.Width = 100;
                grdjobhistory[0, 2].Column.Width = 100;
                grdjobhistory[0, 3].Column.Width = 150;
                grdjobhistory[0, 4].Column.Width = 150;
                grdjobhistory[0, 5].Column.Width = 150;
                grdjobhistory[0, 6].Column.Width = 100;
                grdjobhistory[0, 7].Column.Width = 100;
                grdjobhistory[0, 8].Column.Visible = false;

            }
            if(GridType == "LOAN")
            {
                grdLoan[0, 0] = new SourceGrid.Cells.ColumnHeader("LoanID");
                grdLoan[0, 1] = new SourceGrid.Cells.ColumnHeader("Del");
                grdLoan[0, 2] = new SourceGrid.Cells.ColumnHeader("Name");
                grdLoan[0, 3] = new SourceGrid.Cells.ColumnHeader("Type");
                grdLoan[0, 4] = new SourceGrid.Cells.ColumnHeader("Principal");
                grdLoan[0, 5] = new SourceGrid.Cells.ColumnHeader("Installment");
                grdLoan[0, 6] = new SourceGrid.Cells.ColumnHeader("Interest");
                grdLoan[0, 7] = new SourceGrid.Cells.ColumnHeader("Per Month Decrease Amount");
                grdLoan[0, 8] = new SourceGrid.Cells.ColumnHeader("Total Month");
                grdLoan[0, 9] = new SourceGrid.Cells.ColumnHeader("Remaining Month");
                grdLoan[0, 10] = new SourceGrid.Cells.ColumnHeader("Start Date");
                grdLoan[0, 11] = new SourceGrid.Cells.ColumnHeader("End Date");
                grdLoan[0, 12] = new SourceGrid.Cells.ColumnHeader("Premium");
                grdLoan[0, 13] = new SourceGrid.Cells.ColumnHeader("Initial Installment");
                grdLoan[0, 14] = new SourceGrid.Cells.ColumnHeader("ELID");

                grdLoan[0, 13].Column.Visible = false;
                grdLoan[0, 14].Column.Visible = false;

                grdLoan[0, 0].Column.Visible = false;
                grdLoan[0, 1].Column.Width = 30;
                grdLoan[0, 2].Column.Width = 120;
                grdLoan[0, 3].Column.Width = 140;
                grdLoan[0, 4].Column.Width = 80;
                grdLoan[0, 5].Column.Width = 80;
                grdLoan[0, 6].Column.Width = 80;
                grdLoan[0, 7].Column.Width = 80;
                grdLoan[0, 8].Column.Width = 80;
                grdLoan[0, 9].Column.Width = 80;
                grdLoan[0, 10].Column.Width = 80;
                grdLoan[0, 11].Column.Width = 80;
                grdLoan[0, 12].Column.Width = 80;
            }
            if (GridType == "ADVANCE")
            {
                grdAdvance[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
                grdAdvance[0, 1] = new SourceGrid.Cells.ColumnHeader("Title");
                grdAdvance[0, 2] = new SourceGrid.Cells.ColumnHeader("Amount Total");
                grdAdvance[0, 3] = new SourceGrid.Cells.ColumnHeader("Monthly Installment");
                grdAdvance[0, 4] = new SourceGrid.Cells.ColumnHeader("Taken Date");
                grdAdvance[0, 5] = new SourceGrid.Cells.ColumnHeader("Return within Date");
                grdAdvance[0, 6] = new SourceGrid.Cells.ColumnHeader("Remaining Amount to Pay");
                    
                grdAdvance[0, 0].Column.Width = 30;
                grdAdvance[0, 1].Column.Width = 300;
                grdAdvance[0, 2].Column.Width = 100;
                grdAdvance[0, 3].Column.Width = 100;
                grdAdvance[0, 4].Column.Width = 80;
                grdAdvance[0, 5].Column.Width = 80;
                grdAdvance[0, 6].Column.Width = 100;
            }
            //if (GridType == "SALARYPAYMENT")
            //{
            //    DataTable dt = employees.getAdditionDeduction(); 
            //    grdSalaryPayment[0, 0] = new MyHeader("Code");
            //    grdSalaryPayment[0, 1] = new MyHeader("Name");
            //    grdSalaryPayment[0, 2] = new MyHeader("Basic Salary");
            //    grdSalaryPayment[0, 3] = new MyHeader("Absent Days");
            //    grdSalaryPayment[0, 4] = new MyHeader("Payable Salary");
            //    grdSalaryPayment[0, 5] = new MyHeader("Name");
            //    grdSalaryPayment[0, 6] = new MyHeader("Name");
            //    grdSalaryPayment[0, 7] = new MyHeader("Name");
            //    grdSalaryPayment[0, 8] = new MyHeader("Name");
            //}
           
        }
        private void makeSalaryPaymentHeader()
        {
            //grdSalaryPayment.Redim(2, 4);
            grdSalaryPayment.Rows.Insert(0);
            grdSalaryPayment.Rows.Insert(1);
            grdSalaryPayment[0, 0] = new MyHeader("Earnings");
            grdSalaryPayment[0, 0].ColumnSpan = 2;
            grdSalaryPayment[0, 2] = new MyHeader("Deduction");
            grdSalaryPayment[0, 2].ColumnSpan = 2;

            grdSalaryPayment[1, 0] = new MyHeader("Earnings");
            grdSalaryPayment[1, 1] = new MyHeader("Amount");
            grdSalaryPayment[1, 2] = new MyHeader("Deduction");
            grdSalaryPayment[1, 3] = new MyHeader("Amount");

            grdSalaryPayment[0, 0].Column.Width = 350;
            grdSalaryPayment[0, 1].Column.Width = 150;
            grdSalaryPayment[0, 2].Column.Width = 350;
            grdSalaryPayment[1, 0].Column.Width = 200;
            grdSalaryPayment[1, 1].Column.Width = 200;
            grdSalaryPayment[1, 2].Column.Width = 200;
            grdSalaryPayment[1, 3].Column.Width = 150;
            

        }
        private void fillSalaryPayInfo()
        {
            int CurRows = grdemployee.Selection.GetSelectionRegion().GetRowsIndex()[0]; 
            employeeID = Convert.ToInt32(grdemployee[CurRows, 0].Value.ToString());
            DataTable dt = employees.getSalaryPayInfo(employeeID,monthID);

         grdSalaryPayment.Redim(dt.Rows.Count + 2, 4);
         makeSalaryPaymentHeader();
         DataTable dtemp = employees.getBasicSalary(employeeID,monthID);
         DataRow dremp;
         if (dtemp.Rows.Count > 0)
         {
             btnDeletePayInfo.Visible = true;
              grdSalaryPayment.Visible = true;
              isSalaryGenerated = true;
              dremp = dtemp.Rows[0];
              grdSalaryPayment[2, 0] = new SourceGrid.Cells.Cell("Basic Salary");
              grdSalaryPayment[2, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
              grdSalaryPayment[2, 1] = new SourceGrid.Cells.Cell(Convert.ToDouble( dremp["BasicSalary"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces)));
             // grdSalaryPayment[2, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
              TotalEarning += Convert.ToDouble(dremp["BasicSalary"].ToString());
              earningRowCount++;
              for (int i = 1; i <= dt.Rows.Count; i++)
              {
                  DataRow dr = dt.Rows[i - 1];
                  grdSalaryPayment[i + 2, 0] = new SourceGrid.Cells.Cell(dr["allowanceName"].ToString());
                  grdSalaryPayment[i + 2, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
                  grdSalaryPayment[i + 2, 1] = new SourceGrid.Cells.Cell(Convert.ToDouble(dr["allowanceamount"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                 // grdSalaryPayment[i + 2, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                  TotalEarning += Convert.ToDouble(dr["allowanceamount"].ToString());
                  earningRowCount++;

              }
         }
         else
         {
             isSalaryGenerated = false;
             grdSalaryPayment.Visible = false;
             btnDeletePayInfo.Visible = false;
             MessageBox.Show("Salary Not Generated For the Month of"+" "+cmbMonth.Text,"Salary Not Generated",MessageBoxButtons.OK,MessageBoxIcon.Information);
             return;
         }

       
         //grdSalaryPayment[dt.Rows.Count + 3, 0] = new SourceGrid.Cells.Cell("Total Earning");
         ////grdSalaryPayment[dt.Rows.Count + 3, 1].Value = TotalEarning.ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces));
         //grdSalaryPayment[dt.Rows.Count + 3, 1] = new SourceGrid.Cells.Cell(TotalEarning.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
        

        }
        private void fillDeduction()
        {
            DataTable dt=employees.getPfCfAmount(employeeID,monthID);

           // DataRow dr = dt.Rows[0];
           // grdSalaryPayment[2, 2] = new SourceGrid.Cells.Cell("Provident Fund");
           // grdSalaryPayment[2, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
           // grdSalaryPayment[3, 2] = new SourceGrid.Cells.Cell("CIT");
           // grdSalaryPayment[3, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
           // grdSalaryPayment[4, 2] = new SourceGrid.Cells.Cell("TDS");
           // grdSalaryPayment[4, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
           // grdSalaryPayment[2, 3] = new SourceGrid.Cells.Cell(Convert.ToDouble(dr["pfAmount"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
           //// grdSalaryPayment[2, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
           // TotalDeduction += Convert.ToDouble(dr["pfAmount"].ToString());
           // deductionRowCount++;
           // grdSalaryPayment[3, 3] = new SourceGrid.Cells.Cell(Convert.ToDouble(dr["citamount"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
           //// grdSalaryPayment[3, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
           // TotalDeduction += Convert.ToDouble(dr["citamount"].ToString());
           // deductionRowCount++;
           // grdSalaryPayment[4, 3] = new SourceGrid.Cells.Cell(Convert.ToDouble(dr["tds"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
           //// grdSalaryPayment[4, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
           // TotalDeduction += Convert.ToDouble(dr["tds"].ToString());
           // deductionRowCount++;

           // DataTable dtd = employees.getdeduction(employeeID,monthID);
           // for (int i = 1; i <= dtd.Rows.Count; i++)
           // {
           //     DataRow drd = dtd.Rows[i - 1];
           //     grdSalaryPayment[i+4, 2] = new SourceGrid.Cells.Cell(drd["deductionName"].ToString());
           //     grdSalaryPayment[i + 4, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
           //     grdSalaryPayment[i + 4, 3] = new SourceGrid.Cells.Cell(Convert.ToDouble(drd["deductionAmount"].ToString()).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
           //    // grdSalaryPayment[i + 4, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
           //     TotalDeduction += Convert.ToDouble(drd["deductionAmount"].ToString());
           //     deductionRowCount++;
           // }
            //grdSalaryPayment[dt.Rows.Count + 4, 2] = new SourceGrid.Cells.Cell("Total");
            ////grdSalaryPayment[dt.Rows.Count + 4, 3].Value = TotalDeduction.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //grdSalaryPayment[dt.Rows.Count + 4, 3] = new SourceGrid.Cells.Cell(TotalDeduction.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
            
        }
        private class MyHeader : SourceGrid.Cells.ColumnHeader
        {
            public MyHeader(object value)
                : base(value)
            {
                //1 Header Row
                SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
                view.Font = new Font("Arial", 10, FontStyle.Bold);
                view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                View = view;

                AutomaticSortEnabled = false;
            }

        }
        private void FillEducation()
        {
            GridType = "EDUCATION";
            grdacademicqualification.Rows.Clear();
            grdacademicqualification.Redim(1, 6);
            WriteHeader();
            AddRowEducation(1);
        }
        private void AddRowEducation(int RowCount)
        {
            grdacademicqualification.Redim(Convert.ToInt32(RowCount + 1), grdacademicqualification.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("X");
            //btnDelete.Image = HRM.Properties.Resources.;
         //   btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
            //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            int i = RowCount;

            grdacademicqualification[i, 0] = btnDelete;
            grdacademicqualification[i, 0].AddController(evtDelete);
          
            SourceGrid.Cells.Editors.TextBox txtinstitutename = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtinstitutename.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdacademicqualification[i, 1] = new SourceGrid.Cells.Cell("", txtinstitutename);
            grdacademicqualification[i, 1].AddController(evtInstituteFocusLost);
            grdacademicqualification[i, 1].Value = "Enter Institute Name";

            SourceGrid.Cells.Editors.TextBox txtboard = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtboard.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdacademicqualification[i, 2] = new SourceGrid.Cells.Cell("", txtboard);
            grdacademicqualification[i, 2].Value = "";

            SourceGrid.Cells.Editors.TextBox txtcourse = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtcourse.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdacademicqualification[i, 3] = new SourceGrid.Cells.Cell("", txtcourse);
            
            SourceGrid.Cells.Editors.TextBox txtpercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtpercentage.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdacademicqualification[i, 4] = new SourceGrid.Cells.Cell("", txtpercentage);
            grdacademicqualification[i, 4].Value = "";

            SourceGrid.Cells.Editors.TextBox txtpastyear = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtpastyear.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdacademicqualification[i, 5] = new SourceGrid.Cells.Cell("", txtpastyear);
            grdacademicqualification[i, 5].Value = "(AD)";
          

        }

        private void evtInstituteFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row+1;
            AddRowEducation(CurrRowPos);
        }
        private void evtCompanyFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row + 1;
            AddRowExperience(CurrRowPos);
        }
        private void Delete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdacademicqualification.RowsCount - 2)
                grdacademicqualification.Rows.Remove(ctx.Position.Row);

            
        }
        private void workexperienceDelete_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the Experince row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdworkexperience.RowsCount - 2)
                grdworkexperience.Rows.Remove(ctx.Position.Row);

        }
        

        private void FillExperience()
        {
            GridType = "EXPERIENCE";
            grdworkexperience.Rows.Clear();
            grdworkexperience.Redim(1, 5);
            WriteHeader();
            AddRowExperience(1);
        }

        private void FillLoan()
        {
            GridType = "LOAN";
            grdLoan.Rows.Clear();
            grdLoan.Redim(1, 15);
            evtRemoveLoan.Click += new EventHandler(Remove_Loan_Row_Click);
            WriteHeader();
            
        }

        private void FillAdvance()
        {
            GridType = "ADVANCE";
            grdAdvance.Rows.Clear();
            grdAdvance.Redim(1, 7);
            evtRemoveAdvance.Click += new EventHandler(Remove_Advance_Row_Click);
            WriteHeader();
           
        }
        bool isFirstClick = true;
        private void Remove_Loan_Row_Click(object sender, EventArgs e)
        {
            if (isFirstClick)
            {
                isFirstClick = false;
                if (MessageBox.Show("Are you sure you want to remove the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
                //Do not delete if its the last Row because it contains (NEW)
                if (ctx.Position.Row <= grdLoan.RowsCount - 1)
                {
                    grdLoan.Rows.Remove(ctx.Position.Row);
                }


            }
            else
            {
                isFirstClick = true;
            }

        }

        private void Remove_Advance_Row_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            //Do not delete if its the last Row because it contains (NEW)
            if (ctx.Position.Row <= grdAdvance.RowsCount - 1)
            {
                grdAdvance.Rows.Remove(ctx.Position.Row);
            }

           
        }
        private void AddRowExperience(int RowCount)
        {
            grdworkexperience.Redim(Convert.ToInt32(RowCount + 1), grdworkexperience.ColumnsCount);
            SourceGrid.Cells.Button btnDeletee = new SourceGrid.Cells.Button("X");
          
            int i = RowCount;

            grdworkexperience[i, 0] = btnDeletee;
            grdworkexperience[i, 0].AddController(evtDeletee);

            SourceGrid.Cells.Editors.TextBox txtcompanyname = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtcompanyname.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdworkexperience[i, 1] = new SourceGrid.Cells.Cell("", txtcompanyname);
            grdworkexperience[i, 1].AddController(evtCompanyFocusLost);
            grdworkexperience[i, 1].Value = "EnterCompany Name";

            SourceGrid.Cells.Editors.TextBox txtdesignation = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtdesignation.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdworkexperience[i, 2] = new SourceGrid.Cells.Cell("", txtdesignation);
            grdworkexperience[i, 2].Value = "";

           // grid[currentRow, 1] = new SourceGrid.Cells.Cell("c:\\windows\\System32\\user32.dll", new EditorFile());
            SourceGrid.Cells.Editors.TextBox txtfromdate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtfromdate.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdworkexperience[i, 3] = new SourceGrid.Cells.Cell("", txtfromdate);
            //grdworkexperience[i, 2].Value = "";
            grdworkexperience[i, 3] = new SourceGrid.Cells.Cell("", new EditorFile());

            SourceGrid.Cells.Editors.TextBox txttodate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txttodate.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdworkexperience[i, 4] = new SourceGrid.Cells.Cell("", txttodate);
           // grdworkexperience[i, 3].Value = "";
            grdworkexperience[i, 4] = new SourceGrid.Cells.Cell("", new EditorFile());

           

        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            ispicupdate = true;
            OpenFileDialog openIMG = new OpenFileDialog();
            openIMG.Filter = "Known graphics format (*.bmp,*.jpg,*.gif,*.png)|*.bmp;*.jpg;*.gif;*.jpeg;*.png";
            openIMG.ShowDialog();
            string imgPath = openIMG.FileName;
            try
            {
                txtImageLocation.Text = imgPath;
                pbPhoto.Image = Image.FromFile(imgPath);
                Picture = Misc.ReadBitmap2ByteArray(imgPath.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }

        }

        private void btnbirthdate_Click(object sender, EventArgs e)
        {
            DateStatus = "BIRTHDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtBirthDate.Text));
            frm.ShowDialog();
           
        }
        public void DateConvert(DateTime DotNetDate)
        {
            if (DateStatus == "BIRTHDATE")
                txtBirthDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "JOINDATE")
                txtjoindate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "RETIREMENTDATE")
                txtenddate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "ADVSTARTDATE")
                txtAdvStartDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "ADVENDDATE")
                txtAdvEndDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "LOANSTARTDATE")
                txtLoanStartDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "LOANENDDATE")
                txtLoanEndDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "STARTDATE")
                txtStartDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "GRDINCRMTDATE")
                txtGrdIncrmtDate.Text = Date.ToSystem(DotNetDate);
        }
        
        private void btnsendemail_Click(object sender, EventArgs e)
        {
            frmemail frmemail = new frmemail(txtEmail.Text);
            frmemail.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            switch (m_mode)
            {

                #region NEW
                case EntryMode.NEW:// Insert Employee Information
                    try
                    {
                        btnNew_Click(sender, e);
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

                #region EDIT
                case EntryMode.EDIT: //Update Employee Information
                    try
                    {
                        LoadEmployeeDetails(Convert.ToInt32(txtemployeeID.Text.Trim()));
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion
            }
        }
        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false);
                    IsFieldChanged = false;
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true);
                    IsFieldChanged = false;
                    break;
            }
        }
        private void EnableControls(bool Enable)
        {
            //txtVchNo.Enabled = txtDate.Enabled = btnDate.Enabled = txtRemarks.Enabled = grdJournal.Enabled = cboSeriesName.Enabled = cboProjectName.Enabled = Enable;
        }
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnDelete.Enabled = Delete;
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
        }
        /// <summary>
        /// Checks if the textbox is empty and returns a message in a messageBox.
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="errorMsg"></param>
        private bool CheckEmptyTxt(TextBox txt,string errorMsg)
        {
            if(txt.Text == "")
            {
                MessageBox.Show(errorMsg);
                txt.Focus();
                return true;
            }
            return false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
           bool chkUserPermission = UserPermission.ChkUserPermission("HRM_CREATE_MODIFY");
            if (chkUserPermission == false)
          {
             Global.MsgError("Sorry! you dont have permission to create employee. Please contact your administrator for permission.");
              return;
          }
            switch (m_mode)
            {
                   
                #region NEW
                case EntryMode.NEW:// Insert Employee Information
                    try
                    {
                        //On every changestate, clear the Opening Balance table
                        FromOpeningBalance.Rows.Clear();
                        FromPreYearBalance.Rows.Clear();
                        
                        BusinessLogic.HRM.Employee employee = new BusinessLogic.HRM.Employee();

                        if (CheckEmptyTxt(txtFirstName, "Please provide the first name"))
                            return;
                        if (CheckEmptyTxt(txtLastName, "Please provide the last name"))
                            return;
                        if (CheckEmptyTxt(txtEmployeeCode, "Please provide the employee code"))
                            return;
                        else if (!employee.CheckStaffCode(txtEmployeeCode.Text.Trim()))
                        {
                            MessageBox.Show("Employee Code already in use");
                            txtEmployeeCode.Focus();
                            return;
                        }
                        if (cmbdesignation.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select a designation");
                            TCEmployee.SelectedIndex = 1;
                            cmbdesignation.DroppedDown = true;
                            return;
                        }
                        if (cmbdepartment.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select a department");
                            TCEmployee.SelectedIndex = 1;
                            cmbdepartment.DroppedDown = true;
                            return;
                        }

                        if (cmbLevel.SelectedIndex == -1)
                        {
                            Global.Msg("Please select a Level.");
                            TCEmployee.SelectedIndex = 4;
                            cmbLevel.DroppedDown = true;
                            return;
                        }

                        if (cmbEmpType.SelectedIndex == -1)
                        {
                            Global.Msg("Please select an employee type.");
                            TCEmployee.SelectedIndex = 0;
                            cmbEmpType.DroppedDown = true;
                            return;
                        }
                        if (!UserValidation.validatecontactnumber(txtEmpGrade.Text.Trim()))
                        {
                            TCEmployee.SelectedIndex = 1;
                            MessageBox.Show("Grade of an employee must be a number.");
                            txtEmpGrade.Focus();
                            return;
                        }


                        BusinessLogic.HRM.EmployeeDetails emp = new BusinessLogic.HRM.EmployeeDetails();
                        emp.FirstName = txtFirstName.Text;
                        emp.MiddleName = txtMiddleName.Text;
                        emp.LastName = txtLastName.Text;
                        emp.EmployeeCode = txtEmployeeCode.Text.Trim();
                        emp.BirthDate =Date.ToDotNet( txtBirthDate.Text);
                        emp.StartDate = Date.ToDotNet(txtStartDate.Text);
                        emp.Gender = rbtnMale.Checked ? 1 : rbtnFemale.Checked ? 2 : 0;
                        emp.IsSingle = rbtnSingle.Checked ? 1 : 0;
                        emp.IsCoupleWorking = chkCoupleWork.Checked ? true : false;
                        emp.PermAddress = txtPermAddress.Text;
                        emp.TempAddress = txtTempAddress.Text;

                        if (cmbPermDistrict.SelectedIndex == -1)
                            emp.PermDist = -1;
                        else
                        {
                            liDistrict = (ListItem)cmbPermDistrict.SelectedItem;
                            int PermDist = liDistrict.ID;
                            emp.PermDist = PermDist;
                        }

                        if (cmbPermZone.SelectedIndex == -1)
                            emp.PermZone = -1;
                        else
                        {
                            liZone = (ListItem)cmbPermZone.SelectedItem;
                            int PermZone = liZone.ID;
                            emp.PermZone = PermZone;
                        }

                        if (cmbTempDistrict.SelectedIndex == -1)
                            emp.TempDist = -1;
                        else
                        {
                            liDistrict = (ListItem)cmbTempDistrict.SelectedItem;
                            int TempDist = liDistrict.ID;
                            emp.TempDist = TempDist;
                        }

                        if (cmbTempZone.SelectedIndex == -1)
                            emp.TempZone = -1;
                        else
                        {
                            liZone = (ListItem)cmbTempZone.SelectedItem;
                            int TempZone = liZone.ID;
                            emp.TempZone = TempZone;
                        }
                        
                        liNationality = (ListItem)cmbNationality.SelectedItem;
                        int nationalityid = liNationality.ID;
                        emp.NationalityID = nationalityid;
                        emp.CitizenshipNumber = txtCitizenshipNo.Text;
                        emp.FatherName = txtFatherName.Text;
                        emp.GrandfatherName = txtGFatherName.Text;
                        emp.Religion = cmbReligion.Text;
                        emp.EmpType = cmbEmpType.Text;
                        liEthnicity = (ListItem)cmbEthnicity.SelectedItem;
                        int ethnicityID = liEthnicity.ID;
                        emp.EthnicityID = ethnicityID;
                        emp.Phone1 = txtPhone1.Text;
                        emp.Phone2 = txtPhone2.Text;
                        emp.Email = txtEmail.Text;
                        emp.EmployeeNote = txtEmployeeNote.Text;
                        if (Picture != null && txtImageLocation.Text != "")
                        {
                            emp.EmployeePhoto = Picture;
                            string PhotoDestLocation = Settings.GetSettings("DEFAULT_EMP_PHOTOS_FOLDER");

                            string sourceFile = System.IO.Path.Combine(txtImageLocation.Text, "");
                            string destFile = System.IO.Path.Combine(PhotoDestLocation, txtFirstName.Text + txtMiddleName.Text + txtLastName.Text + txtEmployeeCode.Text + ".jpg");

                            System.IO.File.Copy(sourceFile, destFile, true);

                        }
                        else
                            emp.EmployeePhoto = null;
                        emp.JoinDate =Date.ToDotNet( txtjoindate.Text);
                        emp.EndDate = Date.ToDotNet(txtenddate.Text);
                        liDepartment = (ListItem)cmbdepartment.SelectedItem;
                        int departmentid = liDepartment.ID;
                        emp.DepartmentID = departmentid;
                        liDesignation = (ListItem)cmbdesignation.SelectedItem;
                        int designationid = liDesignation.ID;
                        liFaculty = (ListItem)cmbFaculty.SelectedItem;
                        emp.FacultyID = liFaculty.ID;
                        emp.DesignationID = designationid;
                        emp.Type = cmbtype.SelectedIndex;
                        emp.Status = cmbstatus.SelectedIndex;
                        liLevel = (ListItem)cmbLevel.SelectedItem;
                        int levelID = liLevel.ID;
                        emp.Level = levelID;
                        emp.Grade = Convert.ToInt32(txtEmpGrade.Text);

                        emp.GradeIncrementDate = Date.ToDotNet(txtGrdIncrmtDate.Text);

                        dtQualification.Clear();//Clear datatable before entering new datas
                        //Save Education Grid Data to Datatable
                        dtQualification.Columns.Clear();
                        dtQualification.Columns.Add("InstituteName");
                        dtQualification.Columns.Add("Board");
                        dtQualification.Columns.Add("Course");
                        dtQualification.Columns.Add("Percentage");
                        dtQualification.Columns.Add("PassYear");
                        
                        for (int i = 0; i < grdacademicqualification.Rows.Count - 2; i++)
                        {

                            if (!UserValidation.validatecontactnumber(grdacademicqualification[i + 1, 5].Value.ToString()) || Convert.ToInt32(grdacademicqualification[i + 1, 5].Value) > DateTime.Now.Year || Convert.ToInt32(grdacademicqualification[i + 1, 5].Value) < DateTime.Now.Year-100)
                            {
                                TCEmployee.SelectedIndex = 2;
                                MessageBox.Show("Invalid passed year");
                                return;
                            }
                            dtQualification.Rows.Add(grdacademicqualification[i + 1, 1].Value, grdacademicqualification[i + 1, 2].Value, grdacademicqualification[i + 1, 3].Value, grdacademicqualification[i + 1, 4].Value, grdacademicqualification[i + 1, 5].Value);
                        }

                        dtExperiences.Clear();//Clear datatable before entering new datas
                        //Save the Experience Grid Data to Datatable
                        dtExperiences.Columns.Clear();
                        dtExperiences.Columns.Add("CompanyName");
                        dtExperiences.Columns.Add("FromDate");
                        dtExperiences.Columns.Add("ToDate");
                        dtExperiences.Columns.Add("Designation");
                        for (int j = 0; j < grdworkexperience.Rows.Count - 2; j++)
                        {
                            dtExperiences.Rows.Add(grdworkexperience[j + 1, 1].Value, (Date.ToDotNet(grdworkexperience[j + 1, 3].Value.ToString())).ToString(), (Date.ToDotNet(grdworkexperience[j + 1, 4].Value.ToString())).ToString(), grdworkexperience[j + 1, 2].Value);
                        }
                        #region Salary information
                        //For Saving Salary Information
                        emp.StartingSalary = Convert.ToDouble(txtstartingsalary.Text);
                        emp.Adjusted = Convert.ToDouble(txtadusted.Text);
                        emp.ElectricityCharge = Convert.ToDecimal(txtElectricity.Text);

                        emp.IsPF = rbtnyes.Checked ? true : false;
                        if (rbtnyes.Checked)
                        {
                            if (txtpfnumber.Text == "")
                            {
                                Global.Msg("Please provide a provident fund number or select 'No' for provident fund.");
                                txtpfnumber.Focus();
                                return;
                            }
                            else
                            {
                                emp.PFNumber = Convert.ToInt32(txtpfnumber.Text);
                            }
                        }
                        else
                        {
                            emp.PFNumber = 0;
                        }

                        emp.IsPension = rbtnPensionYes.Checked ? true : false;
                        if (rbtnPensionYes.Checked)
                        {
                            if (txtPensionNumber.Text == "")
                            {
                                Global.Msg("Please provide a Pension Fund number or select 'No' for Pension Fund.");
                                txtPensionNumber.Focus();
                                return;
                            }
                            else
                            {
                                emp.PensionNumber = txtPensionNumber.Text;
                            }
                        }
                        else
                        {
                            emp.PensionNumber = "0";
                        }
                        
                        emp.IsInsurance = rbtnInsYes.Checked ? true : false;
                        if (rbtnInsYes.Checked)
                        {
                            if (txtInsNumber.Text == "" || txtInsAmt.Text  == "" || txtInsPremium.Text == "")
                            {
                                Global.Msg("Please all insurance details or select 'No' for insurance.");
                                txtInsNumber.Focus();
                                return;
                            }
                            else
                            {
                                emp.InsuranceNumber = txtInsNumber.Text;
                                emp.InsuranceAmt = Convert.ToDouble(txtInsAmt.Text);
                                emp.InsurancePremium =Convert.ToDouble( txtInsPremium.Text);
                            }
                        }
                        else
                        {
                            emp.InsuranceNumber = "";
                            emp.InsuranceAmt = 0;
                            emp.InsurancePremium = 0;
                        }
                        
                        if (txtcifno.Text == "")
                            emp.CIFNumber = 0;
                        else
                            emp.CIFNumber = Convert.ToInt32(txtcifno.Text);
                        if (txtcitamount.Text == "")
                            emp.CITAmount = 0;
                        else
                            emp.CITAmount = Convert.ToDouble(txtcitamount.Text);
                        if (cmbbankname.SelectedIndex == -1)
                        {
                            emp.BankID = -1;
                        }
                        else
                        {
                            liBank = (ListItem)cmbbankname.SelectedItem;
                            emp.BankID = liBank.ID;
                        }

                        emp.ACNumber = txtbankacnumber.Text;
                        if (txtAcademicAlw.Text == "")
                        {
                            emp.AcademicAlw = 0;
                        }
                        else
                        {
                            emp.AcademicAlw = Convert.ToDouble(txtAcademicAlw.Text);
                        }
                        emp.BasicSalary = Convert.ToDouble(txtbasicsalary.Text);
                        emp.PAN = txtpan.Text;
                        
                        if (txtInflationAlw.Text == "")
                            emp.inflationAlw = 0;
                        else
                            emp.inflationAlw = Convert.ToDouble(txtInflationAlw.Text);
                        if (txtAdmAlw.Text == "")
                            emp.AdmAlw = 0;
                        else
                            emp.AdmAlw = Convert.ToDouble(txtAdmAlw.Text);
                        emp.PostAlw = txtPostAlw.Text == "" ? 0 : Convert.ToDouble(txtPostAlw.Text);
                        
                        if (txttada.Text == "")
                            emp.TADA = 0;
                        else
                            emp.TADA = Convert.ToDouble(txttada.Text);
                        if (txtmiscpaid.Text == "")
                            emp.MiscAllowance = 0;
                        else
                            emp.MiscAllowance = Convert.ToDouble(txtmiscpaid.Text);

                        if (txtNLKoshdeduct.Text == "")
                            emp.NLKoshDeduct = 0;
                        else
                            emp.NLKoshDeduct = Convert.ToDouble(txtNLKoshdeduct.Text);

                        emp.NLKoshNo = txtNLKoshNo.Text.Trim();

                        emp.KalyankariNo = txtKKNum.Text.Trim();
                        emp.KalyankariAmt = txtKKAmt.Text == "" ? 0 : Convert.ToDouble(txtKKAmt.Text);

                        emp.OverTimeAllow = txtOverTimeAlw.Text == "" ? 0 : Convert.ToDecimal(txtOverTimeAlw.Text);

                        emp.PFAdjust = txtPFAdjust.Text == "" ? 0 : Convert.ToDecimal(txtPFAdjust.Text);

                        emp.PensionAdjust = txtPensionAdjust.Text == "" ? 0 : Convert.ToDecimal(txtPensionAdjust.Text);


                        emp.Remarks = txtremarks.Text;

                        emp.IsQuarter = rbtnQuarterYes.Checked == true ? true : false;
                        if (rbtnQuarterYes.Checked)
                        {
                            if (txtAccommodation.Text == "")
                            {
                                Global.Msg("Please provide accommodation charge or select 'No' for quarter stay.");
                                txtAccommodation.Focus();
                                return;
                            }
                            else
                            {
                                emp.Accommodation = Convert.ToDouble(txtAccommodation.Text);
                            }
                        }
                        else
                        {
                            emp.Accommodation = 0;
                        }
                        #endregion
                        #region Loan and Advance
                        dtLoan.Clear();//Clear datatable before entering new datas
                        dtLoan.Columns.Clear();
                        dtLoan.Columns.Add("LoanID");
                        dtLoan.Columns.Add("LoanPrincipal");
                        dtLoan.Columns.Add("LoanMthInstallment");
                        dtLoan.Columns.Add("LoanMthInterest");
                        dtLoan.Columns.Add("LoanMthDecreaseAmt");
                        dtLoan.Columns.Add("LoanTotalMth");
                        dtLoan.Columns.Add("LoanRemainingMth");
                        dtLoan.Columns.Add("LoanStartDate");
                        dtLoan.Columns.Add("LoanEndDate");
                        dtLoan.Columns.Add("LoanMthPremium");
                        for (int j = 0; j < grdLoan.Rows.Count - 1; j++)
                        {
                            dtLoan.Rows.Add(grdLoan[j + 1, 0].Value, grdLoan[j + 1, 4].Value, grdLoan[j + 1, 5].Value, grdLoan[j + 1, 6].Value, grdLoan[j + 1, 7].Value, grdLoan[j + 1, 8].Value, grdLoan[j + 1, 9].Value, grdLoan[j + 1, 10].Value, grdLoan[j + 1, 11].Value, grdLoan[j + 1, 12].Value);
                        }

                        dtAdvance.Clear();//Clear datatable before entering new datas
                        dtAdvance.Columns.Clear();
                        dtAdvance.Columns.Add("AdvTitle");
                        dtAdvance.Columns.Add("TotalAmt");
                        dtAdvance.Columns.Add("Installment");
                        dtAdvance.Columns.Add("TakenDate");
                        dtAdvance.Columns.Add("ReturnDate");
                        dtAdvance.Columns.Add("RemainingAmt");
                        for (int j = 0; j < grdAdvance.Rows.Count - 1; j++)
                        {
                            dtAdvance.Rows.Add(grdAdvance[j + 1, 1].Value, grdAdvance[j + 1, 2].Value, grdAdvance[j + 1, 3].Value, grdAdvance[j + 1, 4].Value, grdAdvance[j + 1, 5].Value, grdAdvance[j + 1, 6].Value);
                        }
                       

                      
                        #endregion
                        TextBox txtLedgerCode = new TextBox();
                        //Ledger Code Creation
                        #region  For Automation of Ledgercode 
                        int numberingType = 0;
                            DataTable config = new DataTable();
                            DataTable format = new DataTable();

                            config = Ledger.GetLedgerConfig();
                        foreach (DataRow getconfig in config.Rows)
                        {
                            numberingType = Convert.ToInt32(getconfig["NumberingType"]);

                        }
                        if (numberingType == 1)
                        {
                            format = Ledger.GetFormatParameter();
                            string str = "";
                            foreach (DataRow dr in format.Rows)
                            {
                                switch (dr["Type"].ToString())
                                {
                                    case "Symbol":
                                        str = str + dr["Parameter"].ToString();
                                        break;
                                    case "(AutoNumber)":
                                        {
                                            if (Convert.ToInt32(dr["Parameter"]) < 10)
                                                str = str + "00000" + dr["parameter"].ToString();
                                            else if (Convert.ToInt32(dr["Parameter"]) >= 10 && (Convert.ToInt32(dr["Parameter"]) < 100))
                                                str = str + "0000" + dr["parameter"].ToString();
                                            else if (Convert.ToInt32(dr["Parameter"]) >= 100 && (Convert.ToInt32(dr["Parameter"]) < 1000))
                                                str = str + "000" + dr["parameter"].ToString();
                                            else if (Convert.ToInt32(dr["Parameter"]) >= 1000 && (Convert.ToInt32(dr["Parameter"]) < 10000))
                                                str = str + "00" + dr["parameter"].ToString();
                                            else if (Convert.ToInt32(dr["Parameter"]) >= 10000 && (Convert.ToInt32(dr["Parameter"]) < 100000))
                                                str = str + "0" + dr["parameter"].ToString();
                                            else
                                                str = str + dr["parameter"].ToString();
                                            break;

                                        }
                                    case "Date":
                                        {

                                            if (dr["Parameter"].ToString() == "NEPALI_FISCAL_YEAR")
                                                str = str + Global.Fiscal_Nepali_Year;

                                            else if (dr["Parameter"].ToString() == "ENGLISH_FISCAL_YEAR")
                                                str = str + Global.Fiscal_English_Year;
                                            break;

                                        }
                                }

                            }

                            txtLedgerCode.Text = str;
                            this.Refresh();
                        }
                        #endregion
                       
                        int employeeID = 0;
                       
                        string insertResult = employee.CreateEmployee(emp, dtQualification, dtExperiences, dtLoan, dtAdvance, out employeeID);
                        if (insertResult == "SUCCESS")
                        {
                            //Creation Of the Ledger of Respective ledgername
                            ILedger acLedger = new Ledger();
                            DateTime currentdate = Date.GetServerDate();
                            int ledgerIDOut = 0;
                            bool result = acLedger.Create(txtLedgerCode.Text.Trim(), txtEmployeeCode.Text.Trim() + "-" + txtFirstName.Text.Trim() +txtMiddleName.Text.Trim()+ txtLastName.Text.Trim(), 36, 0.00, "DEBIT", 1, 0.00, currentdate, "", "", "", "", "", "", "", "", "", "", 0.0, false, 0, "", "New Ledger Name=" + txtEmployeeCode + txtFirstName.Text + "Parent Group" + "Staff Account", true, out ledgerIDOut);
                            //Increment the ledger code (automated number)
                            Global.m_db.InsertUpdateQry("update acc.tblLedgerCodeFormat set Parameter=Parameter+1 where TYPE='(AutoNumber)' ");


                            //Get current LedgerID and save it to tblOpeningBalance
                            //int LdrID = Ledger.GetLedgerIdFromName(txtemployeecode.Text.Trim() + txtfirstname.Text, LangMgr.DefaultLanguage);
                            // int LdrID = Ledger.GetLedgerIdFromName(ledgername, LangMgr.DefaultLanguage);
                            OpeningBalance.InsertAccountOpeningBalance(ledgerIDOut, FromOpeningBalance);
                            OpeningBalance.InsertAccountPreYearBalance(ledgerIDOut, FromPreYearBalance);

                            //Adding ledgerid to HRM.tblEmployee
                            int ledgerUpdateResult = employee.UpdateEmployeeLedgerID(ledgerIDOut, employeeID);
                            if(ledgerUpdateResult < 0)
                            {
                                Global.Msg("There was a problem while creating a relation between the employee record and ledger");
                            }
                            btnNew_Click(sender, e);
                            ChangeState(EntryMode.NEW);
                            frmEmployeeRegistration_Load(sender, e);
                        }
                    }
                    catch(Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

                #region EDIT
                case EntryMode.EDIT: //Update Employee Information
                    try
                    {
                        BusinessLogic.HRM.Employee employee = new BusinessLogic.HRM.Employee();
                        BusinessLogic.HRM.EmployeeDetails emp = new BusinessLogic.HRM.EmployeeDetails();

                        if (CheckEmptyTxt(txtFirstName, "Please provide the first name"))
                            return;
                        if (CheckEmptyTxt(txtLastName, "Please provide the last name"))
                            return;
                        if (CheckEmptyTxt(txtEmployeeCode, "Please provide the employee code"))
                            return;
                        else if (!employee.CheckStaffCode(txtEmployeeCode.Text.Trim(), Convert.ToInt32(txtemployeeID.Text)))
                        {
                            MessageBox.Show("Employee Code already in use");
                            txtEmployeeCode.Focus();
                            return;
                        }
                        if (cmbEmpType.SelectedIndex == -1)
                        {
                            Global.Msg("Please select an employee type.");
                            TCEmployee.SelectedIndex = 0;
                            cmbEmpType.DroppedDown = true;
                            return;
                        }
                        if (cmbdesignation.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select a designation");
                            TCEmployee.SelectedIndex = 1;
                            cmbdesignation.DroppedDown = true;
                            return;
                        }
                        if (cmbdepartment.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select a department");
                            TCEmployee.SelectedIndex = 1;
                            cmbdepartment.DroppedDown = true;
                            return;
                        }

                        if(cmbLevel.SelectedIndex == -1)
                        {
                            Global.Msg("Please select a Level.");
                            TCEmployee.SelectedIndex = 4;
                            cmbLevel.DroppedDown = true;
                            return;
                        }

                        if (!UserValidation.validatecontactnumber(txtEmpGrade.Text.Trim()))
                        {
                            TCEmployee.SelectedIndex = 1;
                            MessageBox.Show("Grade of an employee must be a number.");
                            txtEmpGrade.Focus();
                            return;
                        }

                        emp.EmployeeID =Convert.ToInt32( txtemployeeID.Text);
                        emp.FirstName = txtFirstName.Text;
                        emp.MiddleName = txtMiddleName.Text; 
                        emp.LastName = txtLastName.Text;
                        emp.EmployeeCode = txtEmployeeCode.Text;
                        emp.BirthDate = Date.ToDotNet(txtBirthDate.Text);
                        emp.StartDate = Date.ToDotNet(txtStartDate.Text);
                        emp.Gender = rbtnMale.Checked ? 1 : rbtnFemale.Checked ? 2 : 0;
                        emp.IsSingle = rbtnSingle.Checked ? 1 : 0;
                        emp.IsCoupleWorking = chkCoupleWork.Checked ? true : false;
                        emp.PermAddress = txtPermAddress.Text;
                        emp.TempAddress = txtTempAddress.Text;
                        if (cmbPermDistrict.SelectedIndex == -1)
                            emp.PermDist = -1;
                        else
                        {
                            liDistrict = (ListItem)cmbPermDistrict.SelectedItem;
                            int PermDist = liDistrict.ID;
                            emp.PermDist = PermDist;
                        }

                        if (cmbPermZone.SelectedIndex == -1)
                            emp.PermZone = -1;
                        else
                        {
                            liZone = (ListItem)cmbPermZone.SelectedItem;
                            int PermZone = liZone.ID;
                            emp.PermZone = PermZone;
                        }

                        if (cmbTempDistrict.SelectedIndex == -1)
                            emp.TempDist = -1;
                        else
                        {
                            liDistrict = (ListItem)cmbTempDistrict.SelectedItem;
                            int TempDist = liDistrict.ID;
                            emp.TempDist = TempDist;
                        }

                        if (cmbTempZone.SelectedIndex == -1)
                            emp.TempZone = -1;
                        else
                        {
                            liZone = (ListItem)cmbTempZone.SelectedItem;
                            int TempZone = liZone.ID;
                            emp.TempZone = TempZone;
                        }
                        liNationality = (ListItem)cmbNationality.SelectedItem;
                        int nationalityid = liNationality.ID;
                        emp.NationalityID = nationalityid;
                        emp.CitizenshipNumber = txtCitizenshipNo.Text;
                        emp.FatherName = txtFatherName.Text;
                        emp.GrandfatherName = txtGFatherName.Text;
                        emp.Religion = cmbReligion.Text;
                        emp.EmpType = cmbEmpType.Text;
                        liEthnicity = (ListItem)cmbEthnicity.SelectedItem;
                        int ethnicityID = liEthnicity.ID;
                        emp.EthnicityID = ethnicityID;
                        emp.Phone1 = txtPhone1.Text;
                        emp.Phone2 = txtPhone2.Text;
                        emp.Email = txtEmail.Text;
                        emp.EmployeeNote = txtEmployeeNote.Text;
                        if (pbPhoto != null)
                        {
                            emp.EmployeePhoto = Picture;
                            if (txtImageLocation.Text != "")
                            {
                                string PhotoDestLocation = Settings.GetSettings("DEFAULT_EMP_PHOTOS_FOLDER");

                                string sourceFile = System.IO.Path.Combine(txtImageLocation.Text, "");
                                string destFile = System.IO.Path.Combine(PhotoDestLocation, txtFirstName.Text + txtMiddleName.Text + txtLastName.Text + txtEmployeeCode.Text + ".jpg");

                                System.IO.File.Copy(sourceFile, destFile, true);
                            }
                        }
                        emp.JoinDate = Date.ToDotNet(txtjoindate.Text);
                        emp.EndDate = Date.ToDotNet(txtenddate.Text);
                        liDepartment = (ListItem)cmbdepartment.SelectedItem;
                        int departmentid = liDepartment.ID;
                        emp.DepartmentID = departmentid;
                        liDesignation = (ListItem)cmbdesignation.SelectedItem;
                        int designationid = liDesignation.ID;
                        emp.DesignationID = designationid;
                        liFaculty = (ListItem)cmbFaculty.SelectedItem;
                        emp.FacultyID = liFaculty.ID;
                        emp.Type = cmbtype.SelectedIndex;
                        emp.Status = cmbstatus.SelectedIndex;
                        liLevel = (ListItem)cmbLevel.SelectedItem;
                        int levelID = liLevel.ID;
                        emp.Level = levelID;
                        emp.Grade = Convert.ToInt32(txtEmpGrade.Text);

                        emp.GradeIncrementDate = Date.ToDotNet(txtGrdIncrmtDate.Text);
                        emp.ElectricityCharge = Convert.ToDecimal(txtElectricity.Text);
                        #region salary infromation modify
                        //For Saving Salary Information
                        emp.StartingSalary = Convert.ToDouble(txtstartingsalary.Text);
                        emp.Adjusted = Convert.ToDouble(txtadusted.Text);
                        emp.IsPF = rbtnyes.Checked ? true : false;
                        if (txtpfnumber.Text == "")
                        {
                            emp.PFNumber = 0;
                        }
                        else
                        {
                            emp.PFNumber = Convert.ToInt32(txtpfnumber.Text);
                        }

                        emp.IsPension = rbtnPensionYes.Checked ? true : false;
                        if (rbtnPensionYes.Checked)
                        {
                            if (txtPensionNumber.Text == "")
                            {
                                Global.Msg("Please provide a Pension Fund number or select 'No' for Pension Fund.");
                                txtPensionNumber.Focus();
                                return;
                            }
                            else
                            {
                                emp.PensionNumber = txtPensionNumber.Text;
                            }
                        }
                        else
                        {
                            emp.PensionNumber = "0";
                        }

                        emp.OverTimeAllow = txtOverTimeAlw.Text == "" ? 0 : Convert.ToDecimal(txtOverTimeAlw.Text);

                        emp.IsInsurance = rbtnInsYes.Checked ? true : false;
                        if (rbtnInsYes.Checked)
                        {
                            if (txtInsNumber.Text == "" || txtInsAmt.Text == "" || txtInsPremium.Text == "")
                            {
                                Global.Msg("Please all insurance details or select 'No' for insurance.");
                                txtInsNumber.Focus();
                                return;
                            }
                            else
                            {
                                emp.InsuranceNumber = txtInsNumber.Text;
                                emp.InsuranceAmt = Convert.ToDouble(txtInsAmt.Text);
                                emp.InsurancePremium = Convert.ToDouble(txtInsPremium.Text);
                            }
                        }
                        else
                        {
                            emp.InsuranceNumber = "";
                            emp.InsuranceAmt = 0;
                            emp.InsurancePremium = 0;
                        }
                       
                        if (txtcifno.Text == "")
                            emp.CIFNumber = 0;
                        else
                            emp.CIFNumber = Convert.ToInt32(txtcifno.Text);
                        if (txtcitamount.Text == "")
                            emp.CITAmount = 0;
                        else
                            emp.CITAmount = Convert.ToDouble(txtcitamount.Text);
                        if (cmbbankname.SelectedIndex == -1)
                        {
                            emp.BankID = -1;
                        }
                        else
                        {
                            liBank = (ListItem)cmbbankname.SelectedItem;
                            emp.BankID = liBank.ID;
                        }
                        
                        emp.ACNumber = txtbankacnumber.Text;
                        if (txtAcademicAlw.Text == "")
                        {
                            emp.AcademicAlw = 0;
                        }
                        else
                        {
                            emp.AcademicAlw = Convert.ToDouble(txtAcademicAlw.Text);
                        }
                        emp.BasicSalary = Convert.ToDouble(txtbasicsalary.Text);

                        emp.PAN = txtpan.Text;

                        if (txtInflationAlw.Text == "")
                            emp.inflationAlw = 0;
                        else
                            emp.inflationAlw = Convert.ToDouble(txtInflationAlw.Text);
                        if (txtAdmAlw.Text == "")
                            emp.AdmAlw = 0;
                        else
                            emp.AdmAlw = Convert.ToDouble(txtAdmAlw.Text);
                        emp.PostAlw = txtPostAlw.Text == "" ? 0 : Convert.ToDouble(txtPostAlw.Text);
                        
                        if (txttada.Text == "")
                            emp.TADA = 0;
                        else
                            emp.TADA = Convert.ToDouble(txttada.Text);
                        if (txtmiscpaid.Text == "")
                            emp.MiscAllowance = 0;
                        else
                            emp.MiscAllowance = Convert.ToDouble(txtmiscpaid.Text);

                        if (txtNLKoshdeduct.Text == "")
                            emp.NLKoshDeduct = 0;
                        else
                            emp.NLKoshDeduct = Convert.ToDouble(txtNLKoshdeduct.Text);

                        emp.NLKoshNo = txtNLKoshNo.Text.Trim();

                        emp.KalyankariNo = txtKKNum.Text.Trim();
                        emp.KalyankariAmt = txtKKAmt.Text == "" ? 0 : Convert.ToDouble(txtKKAmt.Text);

                        emp.PFAdjust = txtPFAdjust.Text == "" ? 0 : Convert.ToDecimal(txtPFAdjust.Text);

                        emp.PensionAdjust = txtPensionAdjust.Text == "" ? 0 : Convert.ToDecimal(txtPensionAdjust.Text);

                        emp.Remarks = txtremarks.Text;

                        emp.IsQuarter = rbtnQuarterYes.Checked == true ? true : false;
                        if (rbtnQuarterYes.Checked)
                        {
                            if (txtAccommodation.Text == "")
                            {
                                Global.Msg("Please provide accommodation charge or select 'No' for quarter stay.");
                                txtAccommodation.Focus();
                                return;
                            }
                            else
                            {
                                emp.Accommodation = Convert.ToDouble(txtAccommodation.Text);
                            }
                        }
                        else
                        {
                            emp.Accommodation = 0;
                        }
#endregion

                        #region Loan and Advance
                        dtLoan.Columns.Clear();//Clear datatable before entering new datas
                        dtLoan.Rows.Clear();
                        dtLoan.Columns.Add("LoanID");
                        dtLoan.Columns.Add("LoanPrincipal");
                        dtLoan.Columns.Add("LoanMthInstallment");
                        dtLoan.Columns.Add("LoanMthInterest");
                        dtLoan.Columns.Add("LoanMthDecreaseAmt");
                        dtLoan.Columns.Add("LoanTotalMth");
                        dtLoan.Columns.Add("LoanRemainingMth");
                        dtLoan.Columns.Add("LoanStartDate");
                        dtLoan.Columns.Add("LoanEndDate");
                        dtLoan.Columns.Add("LoanMthPremium");

                        dtLoan.Columns.Add("InitialInstallment");
                        dtLoan.Columns.Add("ELID");

                        for (int j = 0; j < grdLoan.Rows.Count - 1; j++)
                        {
                            dtLoan.Rows.Add(grdLoan[j + 1, 0].Value, grdLoan[j + 1, 4].Value, grdLoan[j + 1, 5].Value, grdLoan[j + 1, 6].Value, grdLoan[j + 1, 7].Value, grdLoan[j + 1, 8].Value, grdLoan[j + 1, 9].Value, grdLoan[j + 1, 10].Value, grdLoan[j + 1, 11].Value, grdLoan[j + 1, 12].Value, grdLoan[j + 1, 13].Value, grdLoan[j + 1, 14].Value);
                        }

                        dtAdvance.Columns.Clear();//Clear datatable before entering new datas
                        dtAdvance.Rows.Clear();
                        dtAdvance.Columns.Add("AdvTitle");
                        dtAdvance.Columns.Add("TotalAmt");
                        dtAdvance.Columns.Add("Installment");
                        dtAdvance.Columns.Add("TakenDate");
                        dtAdvance.Columns.Add("ReturnDate");
                        dtAdvance.Columns.Add("RemainingAmt");
                        for (int j = 0; j < grdAdvance.Rows.Count - 1; j++)
                        {
                            dtAdvance.Rows.Add(grdAdvance[j + 1, 1].Value, grdAdvance[j + 1, 2].Value, grdAdvance[j + 1, 3].Value, grdAdvance[j + 1, 4].Value, grdAdvance[j + 1, 5].Value, grdAdvance[j + 1, 6].Value);
                        }
                        ////Loan and advance
                        //if (rbtnLoanYes.Checked)
                        //{
                        //    if (cmbLoan.SelectedIndex == -1)
                        //    {
                        //        Global.Msg("Please select a Loan Name.");
                        //        cmbLoan.DroppedDown = true;
                        //        return;
                        //    }
                        //    if (txtPrincipal.Text == "")
                        //    {
                        //        Global.Msg("Please enter principal amount.");
                        //        txtPrincipal.Select();
                        //        return;
                        //    }
                        //    if (txtMthInstallment.Text == "")
                        //    {
                        //        Global.Msg("Please enter installment amount.");
                        //        txtMthInstallment.Select();
                        //        return;
                        //    }
                        //    if (txtDuration.Text == "")
                        //    {
                        //        Global.Msg("Please enter total number of month.");
                        //        txtDuration.Select();
                        //        return;
                        //    }

                        //    if (!txtLoanStartDate.MaskCompleted)
                        //    {
                        //        Global.Msg("Please enter loan start date.");
                        //        txtLoanStartDate.Select();
                        //        return;
                        //    }
                        //    emp.IsLoan = true;
                        //    liLoan = (ListItem)cmbLoan.SelectedItem;
                        //    emp.LoanID = liLoan.ID;
                        //    emp.LoanPrincipal = Convert.ToDouble(txtPrincipal.Text);
                        //    emp.LoanMthInstallment = Convert.ToDouble(txtMthInstallment.Text);
                        //    emp.LoanMthInterest = txtMthInterest.Text == "" ? 0 : Convert.ToDouble(txtMthInterest.Text);
                        //    emp.LoanMthDecrease = txtMthDecreaseAmt.Text == "" ? 0 : Convert.ToDouble(txtMthDecreaseAmt.Text);
                        //    emp.LoanRemainingMth = txtRemainingMonth.Text == "" ? 0 : Convert.ToInt32(txtRemainingMonth.Text);
                        //    emp.LoanStartDate = Date.ToDotNet(txtLoanStartDate.Text);
                        //    emp.LoanEndDate = Date.ToDotNet(txtLoanEndDate.Text);
                        //    emp.LoanDuration = Convert.ToInt32(txtDuration.Text);
                        //    emp.LoanPremium = Convert.ToDouble(lblPremium.Text);
                        //}
                        //else
                        //{
                        //    emp.IsLoan = false;
                        //    liLoan = (ListItem)cmbLoan.SelectedItem;
                        //    emp.LoanID = liLoan.ID;
                        //    emp.LoanPrincipal = 0;
                        //    emp.LoanMthInstallment = 0;
                        //    emp.LoanMthInterest = 0;
                        //    emp.LoanMthDecrease = 0;
                        //    emp.LoanRemainingMth = 0;
                        //    emp.LoanStartDate = Date.ToDotNet(txtLoanStartDate.Text);
                        //    emp.LoanEndDate = Date.ToDotNet(txtLoanEndDate.Text);
                        //    emp.LoanDuration = 0;
                        //    emp.LoanPremium = 0;
                        //}

                        //if (rbtnAdvYes.Checked)
                        //{
                        //    if (txtAdvAmt.Text == "")
                        //    {
                        //        Global.Msg("Please enter advance amount or select 'no' for advance taken.");
                        //        txtAdvAmt.Select();
                        //        return;
                        //    }
                        //    if (txtAdvMthInstallment.Text == "")
                        //    {
                        //        Global.Msg("Please enter advance monthly installment amount or select 'no' for advance taken.");
                        //        txtAdvMthInstallment.Select();
                        //        return;
                        //    }

                        //    if (!txtAdvStartDate.MaskCompleted)
                        //    {
                        //        Global.Msg("Please enter advance taken date.");
                        //        txtAdvStartDate.Select();
                        //        return;
                        //    }
                        //    emp.IsAdvance = true;
                        //    emp.AdvAmt = Convert.ToDouble(txtAdvAmt.Text);
                        //    emp.AdvMthInstallment = Convert.ToDouble(txtAdvMthInstallment.Text);
                        //    emp.AdvStartDate = Date.ToDotNet(txtAdvStartDate.Text);
                        //    emp.AdvEndDate = Date.ToDotNet(txtAdvEndDate.Text);
                        //    emp.AdvRemainingAmt = txtAdvRemainingAmt.Text == "" ? 0 : Convert.ToDouble(txtAdvRemainingAmt.Text);
                        //}
                        //else
                        //{
                        //    emp.IsAdvance = false;
                        //    emp.AdvAmt = 0;
                        //    emp.AdvMthInstallment = 0;
                        //    emp.AdvStartDate = Date.ToDotNet(txtAdvStartDate.Text);
                        //    emp.AdvEndDate = Date.ToDotNet(txtAdvEndDate.Text);
                        //    emp.AdvRemainingAmt = 0;
                        //}
                        #endregion

                        emp.IsEmpDetailsChanged = isEmployeementDetailsChanged;

                        dtQualification.Columns.Clear();//Clear datatable before entering new datas
                        //Save Education Grid Data to Datatable
                        dtQualification.Rows.Clear();
                        dtQualification.Columns.Add("InstituteName");
                        dtQualification.Columns.Add("Board");
                        dtQualification.Columns.Add("Course");
                        dtQualification.Columns.Add("Percentage");
                        dtQualification.Columns.Add("PassYear");

                        for (int i = 0; i < grdacademicqualification.Rows.Count - 2; i++)
                        {
                            if (!UserValidation.validatecontactnumber(grdacademicqualification[i + 1, 5].Value.ToString()) || Convert.ToInt32(grdacademicqualification[i + 1, 5].Value) > DateTime.Now.Year || Convert.ToInt32(grdacademicqualification[i + 1, 5].Value) < DateTime.Now.Year - 100)
                            {
                                TCEmployee.SelectedIndex = 2;
                                MessageBox.Show("Invalid passed year");
                                return;
                            }
                            dtQualification.Rows.Add(grdacademicqualification[i + 1, 1].Value, grdacademicqualification[i + 1, 2].Value, grdacademicqualification[i + 1, 3].Value, grdacademicqualification[i + 1, 4].Value, grdacademicqualification[i + 1, 5].Value);
                        }

                        dtExperiences.Columns.Clear();//Clear datatable before entering new datas
                        //Save the Experience Grid Data to Datatable
                        dtExperiences.Rows.Clear();
                        dtExperiences.Columns.Add("CompanyName");
                        dtExperiences.Columns.Add("FromDate");
                        dtExperiences.Columns.Add("ToDate");
                        dtExperiences.Columns.Add("Designation");

                        for (int j = 0; j < grdworkexperience.Rows.Count - 2; j++)
                        {
                            dtExperiences.Rows.Add(grdworkexperience[j + 1, 1].Value, (Date.ToDotNet(grdworkexperience[j + 1, 3].Value.ToString())).ToString(), (Date.ToDotNet(grdworkexperience[j + 1, 4].Value.ToString())).ToString(), grdworkexperience[j + 1, 2].Value);
                        }

                        string updateResult = employee.UpdateEmployee(emp, dtQualification, dtExperiences, ispicupdate,dtLoan,dtAdvance);
                        if (updateResult == "SUCCESS")
                        {
                            int ledgerid = employee.GetEmployeeLedgerID(emp.EmployeeID);//Ledger.GetLedgerIdFromName(employeeledger, Lang.English);
                            string lname = txtEmployeeCode.Text + "-" + txtFirstName.Text.Trim() +txtMiddleName.Text.Trim()+ txtLastName.Text.Trim();
                            string SQL = "Update Acc.tblLedger set EngName='" + lname + "',NepName='" + lname + "' where ledgerid='" + ledgerid + "'";
                            Global.m_db.InsertUpdateQry(SQL);
                            isEmployeementDetailsChanged = false;
                            string empCode = txtEmployeeCode.Text;
                            btnNew_Click(sender, e);
                            txtcode.Text = empCode;
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion
            }
            //ChangeState(EntryMode.NEW);
            //frmEmployeeRegistration_Load(sender, e);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("HRM_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to modify. Please contact your administrator for permission.");
               return;
            }
            //panel3.Visible = true;
            ChangeState(EntryMode.EDIT);
            //TCEmployee.Enabled = true;
            for (int i = 0; i < 7; i++)
                TCEmployee.TabPages[i].Enabled = true;
            txtFirstName.Enabled = true;
            txtMiddleName.Enabled = true;
            txtLastName.Enabled = true;
            btnSave.Enabled = true;
            label25.Visible = true;
            //grdjobhistory.Visible = true;
        }

        private void btnjoindate_Click(object sender, EventArgs e)
        {
            DateStatus = "JOINDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtjoindate.Text));
            frm.ShowDialog();
        }

        private void btnretirementdate_Click(object sender, EventArgs e)
        {
            DateStatus = "RETIREMENTDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtenddate.Text));
            frm.ShowDialog();
        }

        private class EditorFile : SourceGrid.Cells.Editors.TextBoxButton
        {
            public EditorFile()
                : base(typeof(string))
            {
                Control.DialogOpen += new EventHandler(Control_DialogOpen);
            }

            void Control_DialogOpen(object sender, EventArgs e)
            {
               // frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtbirthdate.Text));
                //using (frmTestPicker dg = new frmTestPicker())
                TextBox txtcurrentdate = new TextBox();
                txtcurrentdate.Text =Date.ToSystem( Date.GetServerDate());
                frmEmployeeRegistration empreg = new frmEmployeeRegistration();
                using (frmDateConverter dg = new frmDateConverter(empreg, Date.ToDotNet(txtcurrentdate.Text)))
                {
                    if (dg.ShowDialog(EditCellContext.Grid) == DialogResult.OK)
                    {
                        Control.Value =Date.ToSystem(Convert.ToDateTime( dg.result));
                    }
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeState(EntryMode.NEW);
                //TCEmployee.Enabled = true;
                for (int i = 0; i < 7; i++)
                    TCEmployee.TabPages[i].Enabled = true;
                txtFirstName.Enabled = true;
                txtMiddleName.Enabled = true;
                txtLastName.Enabled = true;
                panel3.Visible = false;
                txtFirstName.Clear();
                txtMiddleName.Clear();
                txtLastName.Clear();
                txtEmployeeCode.Clear();
                txtBirthDate.Text = Date.ToSystem(Date.GetServerDate());
                txtStartDate.Text = Date.ToSystem(Date.GetServerDate());
                lblEmployeedFor.Text = "";
                //txtaddress1.Clear();
                //txtaddress2.Clear();
                //txtcity.Clear();
                if (cmbNationality.Items.Count > 0)
                    cmbNationality.SelectedIndex = 0;
                if (cmbReligion.Items.Count > 0)
                    cmbReligion.SelectedIndex = 0;
                if (cmbEmpType.Items.Count > 0)
                    cmbEmpType.SelectedIndex = 0;
                txtPhone1.Clear();
                txtPhone2.Clear();
                txtEmail.Clear();
                txtEmployeeNote.Clear();
                txtCitizenshipNo.Clear();
                txtFatherName.Clear();
                txtGFatherName.Clear();
                rbtnSingle.Checked = true;
                chkCoupleWork.Checked = false;
                txtPermAddress.Clear();
                txtTempAddress.Clear();
                pbPhoto.Image = Misc.GetImageFromByte(null);
                txtjoindate.Text = Date.ToSystem(Date.GetServerDate());
                txtenddate.Text = Date.ToSystem(Date.GetServerDate().AddYears(2));
                if (cmbdepartment.Items.Count > 0)
                    cmbdepartment.SelectedIndex = 0;
                if (cmbdesignation.Items.Count > 0)
                    cmbdesignation.SelectedIndex = 0;
                if (cmbFaculty.Items.Count > 0)
                    cmbFaculty.SelectedIndex = 0;
                if (cmbstatus.Items.Count > 0)
                    cmbstatus.SelectedIndex = 0;
                if (cmbtype.Items.Count > 0)
                    cmbtype.SelectedIndex = 0;
                txtEmpLevel.Clear();
                txtEmpGrade.Text = "0";

                txtGrdIncrmtDate.Text = Date.ToSystem(DateTime.Now);

                AddRowEducation(1);
                AddRowExperience(1);

                txtstartingsalary.Text = "0";
                txtadusted.Text = "0";
                txtcifno.Clear();
                txtcitamount.Clear();
                txtpan.Clear();
                if (cmbbankname.Items.Count > 0)
                    cmbbankname.SelectedIndex = 0;
                txtbankacnumber.Clear();
                txtAcademicAlw.Clear();
                txtbasicsalary.Clear();
                txtpfnumber.Clear();
                txtAdmAlw.Text = "0";
                txtInflationAlw.Text = "0";
                txtPostAlw.Text = "0";
                txtAcademicAlw.Text = "0";
                txtNLKoshdeduct.Text = "0";
                txtPensionNumber.Text = "";
                rbtnInsYes.Checked = true;
                txtInsNumber.Clear();
                txtInsAmt.Text = "0";
                txtInsPremium.Text = "0";
                txtNLKoshNo.Clear();
                txtremarks.Clear();

                txtPensionAdjust.Text = "0";
                txtPFAdjust.Text = "0";

                rbtnQuarterNo.Checked = true;
                txtAccommodation.Text = "0";

                //rbtnLoanNo.Checked = true;
                if (cmbLoan.Items.Count > 0)
                    cmbLoan.SelectedIndex = 0;
                txtPrincipal.Clear();
                txtMthInstallment.Clear();
                txtMthInterest.Clear();
                txtMthDecreaseAmt.Clear();
                txtDuration.Clear();
                txtRemainingMonth.Clear();
                txtLoanStartDate.Text = Date.ToSystem(DateTime.Now);
                txtLoanEndDate.Text = Date.ToSystem(DateTime.Now);
                lblPremium.Text = "0.00";

                //rbtnAdvNo.Checked = true;
                txtAdvAmt.Clear();
                txtAdvMthInstallment.Clear();
                txtAdvStartDate.Text = Date.ToSystem(DateTime.Now);
                txtAdvEndDate.Text = Date.ToSystem(DateTime.Now.AddYears(1));
                txtAdvRemainingAmt.Clear();

                txtKKAmt.Text = "0";
                txtKKNum.Clear();

                txtElectricity.Text = "0";
                txtOverTimeAlw.Clear();
                label25.Visible = false;
                grdjobhistory.Visible = false;
            
                FillLoan();
                FillAdvance();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void LoadComboboxItems(ComboBox comboboxitems)
        {
            try
            {
                comboboxitems.Items.Clear();
                if (comboboxitems == cmbNationality)
                {
                    DataTable dtnationality = Nationality.getNationality();
                    foreach (DataRow drnationality in dtnationality.Rows)
                    {
                        comboboxitems.Items.Add(new ListItem((int)drnationality["NationalityID"], drnationality["NationalityName"].ToString()));

                    }
                    comboboxitems.SelectedIndex = 0;
                    comboboxitems.DisplayMember = "value"; 
                    comboboxitems.ValueMember = "id";
                }
                else if (comboboxitems == cmbdepartment)
                {
                    DataTable dtdepartment = Hrm.getdep();
                    if (dtdepartment.Rows.Count > 0)
                    {
                        foreach (DataRow drdepartment in dtdepartment.Rows)
                        {
                            comboboxitems.Items.Add(new ListItem((int)drdepartment["DepartmentID"], drdepartment["DepartmentName"].ToString()));

                        }
                        comboboxitems.SelectedIndex = 0;
                        comboboxitems.DisplayMember = "value";
                        comboboxitems.ValueMember = "id";
                    }
                }
                else if (comboboxitems == cmbdesignation)
                {
                    DataTable dtdesignation = Hrm.getDesignation();
                    if (dtdesignation.Rows.Count > 0)
                    {
                        foreach (DataRow drdesignation in dtdesignation.Rows)
                        {
                            comboboxitems.Items.Add(new ListItem((int)drdesignation["DesignationID"], drdesignation["DesignationName"].ToString()));

                        }
                        comboboxitems.SelectedIndex = 0;
                        comboboxitems.DisplayMember = "value";
                        comboboxitems.ValueMember = "id";
                    }
                }
                else if (comboboxitems == cmbbankname)
                {
                    DataTable dtbankname = Hrm.getBankName();
                    foreach (DataRow drbank in dtbankname.Rows)
                    {
                        comboboxitems.Items.Add(new ListItem((int)drbank["BankID"], drbank["BankName"].ToString()));

                    }
                    comboboxitems.SelectedIndex = -1;
                    comboboxitems.DisplayMember = "value";
                    comboboxitems.ValueMember = "id";
                }
                else if (comboboxitems == cmbFaculty)
                {
                    DataTable dtFaculty = Hrm.GetEmpFacultyForCmb();
                    foreach(DataRow drFaculty in dtFaculty.Rows)
                    {
                        comboboxitems.Items.Add(new ListItem((int)drFaculty["ID"], drFaculty["Value"].ToString()));
                    }
                    comboboxitems.DisplayMember = "Value";
                    comboboxitems.ValueMember = "ID";
                    if(dtFaculty.Rows.Count > 0) comboboxitems.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
             frmCountry country = new frmCountry();
            country.ShowDialog();
            cmbNationality.Items.Clear();
            LoadComboboxItems(cmbNationality);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmDepartment department = new frmDepartment();
            department.ShowDialog();
            cmbdepartment.Items.Clear();
            LoadComboboxItems(cmbdepartment);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmDesignation designation = new frmDesignation();
            designation.ShowDialog();
            cmbdesignation.Items.Clear();
            LoadComboboxItems(cmbdesignation);
        }

        /// <summary>
        /// Loads All the employee details according to employeeID
        /// </summary>
        /// <param name="empId"></param>
        private void LoadEmployeeDetails(int empId)
        {
            DataTable dtfillemployeedetails;
            DataTable dtEmploymentDetails;

            BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
            dtfillemployeedetails = employees.FillEmployeeDetails(empId);
            DataRow dremployeedetails = dtfillemployeedetails.Rows[0];

            dtEmploymentDetails = employees.GetEmploymentDetails(empId);
           
            txtemployeeID.Text = dremployeedetails["ID"].ToString();
            txtFirstName.Text = dremployeedetails["FirstName"].ToString();
            txtMiddleName.Text = dremployeedetails["MiddleName"].ToString();
            txtLastName.Text = dremployeedetails["LastName"].ToString();
            txtEmployeeCode.Text = dremployeedetails["StaffCode"].ToString();
            employeeledger = dremployeedetails["StaffCode"].ToString() + "-" + dremployeedetails["FirstName"].ToString() + dremployeedetails["MiddleName"].ToString() + dremployeedetails["LastName"].ToString();
            if (dremployeedetails["BirthDate"].ToString() != "")
            {
                DateTime bday = Convert.ToDateTime(dremployeedetails["BirthDate"].ToString());
                 txtBirthDate.Text = Date.ToSystem(bday);
            }

            if (dremployeedetails["StartDate"].ToString() != "")
            {
                DateTime sday = Convert.ToDateTime(dremployeedetails["StartDate"].ToString());
                txtStartDate.Text = Date.ToSystem(sday);
            }
            if (txtStartDate.MaskCompleted)
            {
                try
                {
                    DateTime zeroTime = new DateTime(1, 1, 1);
                    DateTime StartDate = Date.ToDotNet(txtStartDate.Text);
                    //TimeSpan span = Date.GetServerDate() - StartDate;
                    DateTime today = Date.GetServerDate();
                    TimeSpan difference = today.Subtract(StartDate);
                    int result = Convert.ToInt32(difference.TotalDays / 365.25);
                    //int result = DateTime.Compare(DateTime.Today,StartDate );
                    //int years = (zeroTime + span).Year + 1;
                    //int years = Convert.ToInt32( Date.GetServerDate() - StartDate);
                    //txtenddate.Clear();
                    if (result == 1)
                        lblEmployeedFor.Text = "Employeed for : " + result.ToString() + " year";
                    else if (result > 1)
                        lblEmployeedFor.Text = "Employeed for : " + result.ToString() + " years";
                    else
                        lblEmployeedFor.Text = "";
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
            }

            //string testgender = dremployeedetails["IsMale"].ToString();
            if (dremployeedetails["Gender"].ToString() == "1")
                rbtnMale.Checked = true;
            else if (dremployeedetails["Gender"].ToString() == "2")
                rbtnFemale.Checked = true;
            else
                rbtnOther.Checked = true;

            if (dremployeedetails["IsSingle"].ToString() == "True")
                rbtnSingle.Checked = true;
            else if (dremployeedetails["IsSingle"].ToString() == "False")
                rbtnMarried.Checked = true;

            if (dremployeedetails["IsCoupleWorking"].ToString() == "True")
                chkCoupleWork.Checked = true;
            else if (dremployeedetails["IsCoupleWorking"].ToString() == "False")
                chkCoupleWork.Checked = false;

            txtPermAddress.Text = dremployeedetails["PermAddress"].ToString();
            txtTempAddress.Text = dremployeedetails["TempAddress"].ToString();
            
            cmbPermDistrict.Text = dremployeedetails["PermDistName"].ToString();
            
            cmbPermZone.Text = dremployeedetails["PermZoneName"].ToString();
            cmbTempDistrict.Text = dremployeedetails["TempDistName"].ToString();
            cmbTempZone.Text = dremployeedetails["TempZoneName"].ToString();
            cmbNationality.Text = dremployeedetails["NationalityName"].ToString();
            txtCitizenshipNo.Text = dremployeedetails["CitizenshipNo"].ToString();
            txtFatherName.Text = dremployeedetails["FatherName"] != DBNull.Value? dremployeedetails["FatherName"].ToString(): "";
            txtGFatherName.Text = dremployeedetails["GrandfatherName"] != DBNull.Value ? dremployeedetails["GrandfatherName"].ToString() : "";
            cmbReligion.Text = dremployeedetails["Religion"].ToString();
            cmbEmpType.Text = dremployeedetails["EmpType"].ToString();
            cmbEthnicity.Text = dremployeedetails["EthnicityName"].ToString();
            txtPhone1.Text = dremployeedetails["Phone1"].ToString();
            txtPhone2.Text = dremployeedetails["Phone2"].ToString();
            txtEmail.Text = dremployeedetails["Email"].ToString();
            txtEmployeeNote.Text = dremployeedetails["EmployeeNote"].ToString();
            txtpan.Text = dremployeedetails["PAN"].ToString();

            if (dremployeedetails["EmployeePhoto"].ToString() != "")
            {
                pbPhoto.Image = Misc.GetImageFromByte((byte[])dremployeedetails["EmployeePhoto"]);
            }

            else
            {
                pbPhoto.Image = Misc.GetImageFromByte(null);
            }

            DataTable dtjobhistory = employees.JobHistory(empId);
            evtJobHistoryDelete.Click += new EventHandler(Delete_JH_Row_Click);
            FillJobHistory(dtjobhistory);
            dtjobhistory.Rows.Clear();
            if (dtEmploymentDetails.Rows.Count > 0)
            {
                DataRow drEmploymentDetails = dtEmploymentDetails.Rows[0];

                txtjoindate.Text = Date.ToSystem(Convert.ToDateTime(drEmploymentDetails["JoinDate"].ToString()));
                txtenddate.Text = Date.ToSystem(Convert.ToDateTime(drEmploymentDetails["RetirementDate"].ToString()));
                cmbdepartment.Text = drEmploymentDetails["DepartmentName"].ToString();
                cmbdesignation.Text = drEmploymentDetails["DesignationName"].ToString();
                cmbFaculty.Text = drEmploymentDetails["FacultyName"].ToString();
                cmbtype.SelectedIndex = Convert.ToInt32(drEmploymentDetails["Type"].ToString());
                cmbstatus.SelectedIndex = Convert.ToInt32(drEmploymentDetails["Status"].ToString());
            }

            txtEmpGrade.Text = dremployeedetails["Grade"].ToString();
            txtGrdIncrmtDate.Text = Date.ToSystem(Convert.ToDateTime(dremployeedetails["GradeIncrementDate"] != DBNull.Value?dremployeedetails["GradeIncrementDate"].ToString():DateTime.Today.ToString()));
            //Clearing rows before adding rows for another employee
            int maxRows = grdacademicqualification.RowsCount - 1;
            if(maxRows>1)
                grdacademicqualification.Rows.RemoveRange(1,maxRows-1);
    
            DataTable dtqualification = employees.EmployeeQualification(empId);
            int RowCount = 0;
            foreach (DataRow DrQualification in dtqualification.Rows)
            {
                int RowNum = grdacademicqualification.RowsCount - 1;
                RowCount = RowNum;
                grdacademicqualification.Rows.Insert(RowNum + 1);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                //btnDelete.Image = global::Inventory.Properties.Resources.gnome_window_close;
                grdacademicqualification[RowNum, 0] = btnDelete;
                grdacademicqualification[RowNum, 0].AddController(evtDelete);

                SourceGrid.Cells.Editors.TextBox txtinstitutename = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtinstitutename.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdacademicqualification[RowNum, 1] = new SourceGrid.Cells.Cell("", txtinstitutename);
                grdacademicqualification[RowNum, 1].AddController(evtInstituteFocusLost);
                grdacademicqualification[RowNum, 1].Value = DrQualification["InstituteName"].ToString();

                SourceGrid.Cells.Editors.TextBox txtboard = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtboard.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdacademicqualification[RowNum, 2] = new SourceGrid.Cells.Cell("", txtboard);
                grdacademicqualification[RowNum, 2].Value = DrQualification["Board"].ToString();

                SourceGrid.Cells.Editors.TextBox txtcourse = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtcourse.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdacademicqualification[RowNum, 3] = new SourceGrid.Cells.Cell("", txtcourse);
                grdacademicqualification[RowNum, 3].Value = DrQualification["Course"].ToString();

                SourceGrid.Cells.Editors.TextBox txtpercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtpercentage.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdacademicqualification[RowNum, 4] = new SourceGrid.Cells.Cell("", txtpercentage);
                grdacademicqualification[RowNum, 4].Value = DrQualification["Percentage"].ToString();

                SourceGrid.Cells.Editors.TextBox txtpastyear = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtpastyear.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdacademicqualification[RowNum, 5] = new SourceGrid.Cells.Cell("", txtpastyear);
                //grdacademicqualification[RowNum, 4] = new SourceGrid.Cells.Cell("", new EditorFile());
                grdacademicqualification[RowNum, 5].Value = DrQualification["PassYear"].ToString();
                //grdacademicqualification[RowNum, 4].Value =Date.ToSystem(Convert.ToDateTime( DrQualification["PassYear"].ToString()));

            }
            AddRowEducation(RowCount + 1);

            //Clearing rows before adding rows for another employee
            maxRows = grdworkexperience.RowsCount - 1;
            if(maxRows>1)
                grdworkexperience.Rows.RemoveRange(1,maxRows-1);
               
            DataTable dtExperience = employees.EmployeeExperience(empId);
            int CountRow = 0;
            if (dtExperience.Rows.Count > 0)
            {
                CountRow = 0;
            }
            else
            {
                CountRow = 1;
            }
            foreach (DataRow DrExperience in dtExperience.Rows)
            {
                int RowNum = grdworkexperience.RowsCount;
                CountRow = RowNum;
                grdworkexperience.Rows.Insert(RowNum);
                SourceGrid.Cells.Button btnDeletee = new SourceGrid.Cells.Button("");
                // btnDeletee.Image = global::Inventory.Properties.Resources.gnome_window_close;
                //btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                int i = RowNum - 1;

                grdworkexperience[i, 0] = btnDeletee;
                grdworkexperience[i, 0].AddController(evtDeletee);

                SourceGrid.Cells.Editors.TextBox txtcompanyname = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtcompanyname.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdworkexperience[i, 1] = new SourceGrid.Cells.Cell("", txtcompanyname);
                grdworkexperience[i, 1].AddController(evtCompanyFocusLost);
                grdworkexperience[i, 1].Value = DrExperience["CompanyName"].ToString();

                SourceGrid.Cells.Editors.TextBox txtdesignation = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtdesignation.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdworkexperience[i, 2] = new SourceGrid.Cells.Cell("", txtdesignation);
                grdworkexperience[i, 2].Value = DrExperience["Designation"].ToString();

                // grid[currentRow, 1] = new SourceGrid.Cells.Cell("c:\\windows\\System32\\user32.dll", new EditorFile());
                SourceGrid.Cells.Editors.TextBox txtfromdate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtfromdate.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdworkexperience[i, 3] = new SourceGrid.Cells.Cell("", txtfromdate);
                grdworkexperience[i, 3] = new SourceGrid.Cells.Cell("", new EditorFile());
                grdworkexperience[i, 3].Value = Date.ToSystem(Convert.ToDateTime(DrExperience["FromDate"].ToString()));

                SourceGrid.Cells.Editors.TextBox txttodate = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txttodate.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdworkexperience[i, 4] = new SourceGrid.Cells.Cell("", txttodate);
                grdworkexperience[i, 4] = new SourceGrid.Cells.Cell("", new EditorFile());
                grdworkexperience[i, 4].Value = Date.ToSystem(Convert.ToDateTime(DrExperience["ToDate"].ToString()));

             
            }
            AddRowExperience(CountRow);
            ChangeState(EntryMode.NORMAL);
            //TCEmployee.Enabled = false;
            for (int i = 0; i < 7;i++ )
                TCEmployee.TabPages[i].Enabled = false;
            panel3.Visible = true;
            txtFirstName.Enabled = false;
            txtMiddleName.Enabled = false;
            txtLastName.Enabled = false;
            btnSave.Enabled = false;
            dtQualification.Columns.Clear();
            dtExperiences.Columns.Clear();

            //For Salary and Grade
            decimal StartingSalary = Convert.ToDecimal(dremployeedetails["StartingSalary"].ToString());
            txtstartingsalary.Text = StartingSalary.ToString(Misc.FormatNumber(Global.Comma_Separated,Global.DecimalPlaces));

            object pfAdj = dremployeedetails["PFAdjust"];

            decimal PFAdjust = Convert.ToDecimal(pfAdj  == DBNull.Value ? 0: pfAdj);
            txtPFAdjust.Text = PFAdjust.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            object pensionAdj = dremployeedetails["PensionAdjust"];
            decimal PensionAdjust = Convert.ToDecimal(pensionAdj == DBNull.Value ? 0 : pensionAdj);
            txtPensionAdjust.Text = PensionAdjust.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            decimal Adjusted = Convert.ToDecimal(dremployeedetails["Adjusted"].ToString());
            txtadusted.Text = Adjusted.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)); 
            if (dremployeedetails["IsPF"].ToString() == "True")
                rbtnyes.Checked = true;
            else
                rbtnno.Checked = true;
            txtpfnumber.Text = dremployeedetails["PFNumber"].ToString();
            if (dremployeedetails["isPension"].ToString() == "True")
                rbtnPensionYes.Checked = true;
            else
                rbtnPensionNo.Checked = true;
            txtPensionNumber.Text = dremployeedetails["PensionNumber"].ToString();

            cmbLevel.Text = dremployeedetails["LevelName"].ToString();

            if (dremployeedetails["isInsurance"].ToString() == "True")
                rbtnInsYes.Checked = true;
            else
                rbtnInsNo.Checked = true;
            decimal InsuranceAmount = Convert.ToDecimal(dremployeedetails["InsuranceAmount"].ToString());
            txtInsAmt.Text = InsuranceAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)); 
            txtInsNumber.Text = dremployeedetails["InsuranceNumber"].ToString();
            decimal InsurancePremium = Convert.ToDecimal(dremployeedetails["InsurancePremium"].ToString());
            txtInsPremium.Text = InsurancePremium.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)); 


            txtcifno.Text = dremployeedetails["CIFNumber"].ToString();
            txtcitamount.Text = dremployeedetails["CITAmount"].ToString();
            if(dremployeedetails["BankID"].ToString() == "-1")
                cmbbankname.Text = "";
            else
                cmbbankname.Text = Ledger.GetLedgerNameFromID(Convert.ToInt32(dremployeedetails["BankID"].ToString()));

            txtbankacnumber.Text = dremployeedetails["BankACNumber"].ToString();
            decimal AcademicAlw = Convert.ToDecimal(dremployeedetails["AcademicAlw"].ToString());
            txtAcademicAlw.Text = AcademicAlw.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            decimal BasicSalary = Convert.ToDecimal(dremployeedetails["BasicSalary"].ToString());
            txtbasicsalary.Text = BasicSalary.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            decimal InflationAlw = Convert.ToDecimal(dremployeedetails["InflationAlw"].ToString());
            txtInflationAlw.Text = InflationAlw.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            decimal AdmistrativeAlw = Convert.ToDecimal(dremployeedetails["AdmistrativeAlw"].ToString());
            txtAdmAlw.Text = AdmistrativeAlw.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            
            object overTimeAlw = dremployeedetails["OverTimeAlw"];
            decimal OverTimeAllow =Convert.ToDecimal(overTimeAlw==DBNull.Value?0:overTimeAlw);
            txtOverTimeAlw.Text = OverTimeAllow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            txttada.Text = dremployeedetails["TADA"].ToString();
            txtmiscpaid.Text = dremployeedetails["MiscAllowance"].ToString();
            decimal NLKoshDeduct = Convert.ToDecimal(dremployeedetails["NLKoshDeduct"].ToString());
            txtNLKoshdeduct.Text = NLKoshDeduct.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            txtNLKoshNo.Text = dremployeedetails["NLKoshNo"].ToString();
            txtKKNum.Text = dremployeedetails["KalyankariNo"].ToString();
            decimal KalyankariAmt = Convert.ToDecimal(dremployeedetails["KalyankariAmt"].ToString());
            txtKKAmt.Text = KalyankariAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            txtremarks.Text = dremployeedetails["Remarks"].ToString();
            decimal PostAlw = Convert.ToDecimal(dremployeedetails["PostAlw"].ToString());
            txtPostAlw.Text = PostAlw.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            decimal Electricity = Convert.ToDecimal(dremployeedetails["ElectricityCharge"]);
            txtElectricity.Text = Electricity.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            if (dremployeedetails["isQuarter"].ToString()=="True")
            {
                rbtnQuarterYes.Checked = true; 
                decimal accommodation = Convert.ToDecimal(dremployeedetails["Accommodation"].ToString());
                txtAccommodation.Text = accommodation.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            }
            else
            {
                rbtnQuarterNo.Checked = true;
                txtAccommodation.Text = "0";
            }

            #region Loan & Advance
            FillLoan(empId);
            FillAdvance(empId);
            //if (dremployeedetails["isLoan"].ToString() == "True")
            //{
            //    rbtnLoanYes.Checked = true;
            //    cmbLoan.Text = dremployeedetails["LoanName"].ToString();
            //    lblInstallment.Text = dremployeedetails["LoanType"].ToString();
            //    decimal principal = Convert.ToDecimal(dremployeedetails["LoanPrincipal"].ToString());
            //    txtPrincipal.Text = principal.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //    decimal LoanMthInstallment = Convert.ToDecimal(dremployeedetails["LoanMthInstallment"].ToString());
            //    txtMthInstallment.Text = LoanMthInstallment.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //    decimal LoanMthInterest = Convert.ToDecimal(dremployeedetails["LoanMthInterest"].ToString());
            //    txtMthInterest.Text = LoanMthInterest.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //    decimal LoanMthDecreaseAmt = Convert.ToDecimal(dremployeedetails["LoanMthDecreaseAmt"].ToString());
            //    txtMthDecreaseAmt.Text = LoanMthDecreaseAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //    txtDuration.Text = dremployeedetails["LoanTotalMth"].ToString();
            //    txtRemainingMonth.Text = dremployeedetails["LoanRemainingMth"].ToString();
            //    txtLoanStartDate.Text = Date.ToSystem(Convert.ToDateTime(dremployeedetails["LoanStartDate"].ToString()));
            //    txtLoanEndDate.Text = Date.ToSystem(Convert.ToDateTime(dremployeedetails["LoanEndDate"].ToString()));
            //    decimal LoanMthPremium = Convert.ToDecimal(dremployeedetails["LoanMthPremium"].ToString());
            //    lblPremium.Text = LoanMthPremium.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //}
            //else
            //{
            //    rbtnLoanNo.Checked = true;
            //    cmbLoan.SelectedIndex = 0;
            //    txtPrincipal.Clear();
            //    txtMthInstallment.Clear();
            //    txtMthInterest.Clear();
            //    txtMthDecreaseAmt.Clear();
            //    txtDuration.Clear();
            //    txtRemainingMonth.Clear();
            //    txtLoanStartDate.Text = Date.ToSystem(DateTime.Now);
            //    txtLoanEndDate.Text = Date.ToSystem(DateTime.Now);
            //    lblPremium.Text = "0.00";

            //    cmbLoan.Enabled = false;
            //    txtPrincipal.Enabled = false;
            //    txtMthInstallment.Enabled = false;
            //    txtMthInterest.Enabled = false;
            //    txtDuration.Enabled = false;
            //    txtRemainingMonth.Enabled = false;
            //    txtLoanStartDate.Enabled = false;
            //    txtLoanEndDate.Enabled = false;
            //    btnLoanStartDate.Enabled = false;
            //    btnLoanEndDate.Enabled = false;
            //    txtMthDecreaseAmt.Enabled = false;
            //}

            //if (dremployeedetails["isAdvance"].ToString() == "True")
            //{
            //    rbtnAdvYes.Checked = true;
            //    decimal AdvAmt = Convert.ToDecimal(dremployeedetails["AdvAmt"].ToString());
            //    txtAdvAmt.Text = AdvAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //    decimal AdvMthInstallment = Convert.ToDecimal(dremployeedetails["AdvMthInstallment"].ToString());
            //    txtAdvMthInstallment.Text = AdvMthInstallment.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //    decimal AdvRemainingAmt = Convert.ToDecimal(dremployeedetails["AdvRemainingAmt"].ToString());
            //    txtAdvRemainingAmt.Text = AdvRemainingAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            //    txtAdvStartDate.Text = Date.ToSystem(Convert.ToDateTime(dremployeedetails["AdvTakenDate"].ToString()));
            //    txtAdvEndDate.Text = Date.ToSystem(Convert.ToDateTime(dremployeedetails["AdvReturnDate"].ToString()));
            //}
            //else
            //{
            //    rbtnAdvNo.Checked = true;
            //    txtAdvAmt.Clear();
            //    txtAdvMthInstallment.Clear();
            //    txtAdvStartDate.Text = Date.ToSystem(DateTime.Now);
            //    txtAdvEndDate.Text = Date.ToSystem(DateTime.Now.AddYears(1));
            //    txtAdvRemainingAmt.Clear();

            //    txtAdvAmt.Enabled = false;
            //    txtAdvMthInstallment.Enabled = false;
            //    txtAdvStartDate.Enabled = false;
            //    txtAdvEndDate.Enabled = false;
            //    btnAdvTakenDate.Enabled = false;
            //    btnAdvRetrunDate.Enabled = false;
            //    txtAdvRemainingAmt.Enabled = false;
            //}
                                                                                                                            
            #endregion
        }

        private void FillLoan(int EmpID)
        {
            FillLoan();
            DataTable dt = employees.GetEmployeeLoan(EmpID);
            if(dt.Rows.Count > 0)
            {
                for(int i = 1; i <= dt.Rows.Count ; i++)
                {
                    DataRow dr = dt.Rows[i-1];
                    WriteLoanRow(i, Convert.ToInt32(dr["LoanID"].ToString()), dr["LoanName"].ToString(), dr["LoanType"].ToString(), Convert.ToDecimal(dr["LoanPrincipal"].ToString()), Convert.ToDecimal(dr["LoanMthInstallment"].ToString()), Convert.ToDecimal(dr["LoanMthInterest"].ToString()), Convert.ToDecimal(dr["LoanMthDecreaseAmt"].ToString()), Convert.ToInt32(dr["LoanTotalMth"].ToString()), Convert.ToInt32(dr["LoanRemainingMth"].ToString()), Date.ToSystem(Convert.ToDateTime(dr["LoanStartDate"].ToString())), Date.ToSystem(Convert.ToDateTime(dr["LoanEndDate"].ToString())), Convert.ToDecimal(dr["LoanMthPremium"].ToString()), Convert.ToDecimal(dr["InitialInstallment"].ToString()), Convert.ToInt32(dr["ELID"]));
                }
            }
        }

        private void FillAdvance(int EmpID)
        {
            FillAdvance();
            DataTable dt = employees.GetEmployeeAdvance(EmpID);
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i - 1];
                    WriteAdvanceRow(i, dr["AdvTitle"].ToString(), Convert.ToDecimal(dr["TotalAmt"].ToString()), Convert.ToDecimal(dr["Installment"].ToString()), Date.ToSystem(Convert.ToDateTime(dr["TakenDate"].ToString())), Date.ToSystem(Convert.ToDateTime(dr["ReturnDate"].ToString())), Convert.ToDecimal(dr["RemainingAmt"].ToString()));
                }
            }
        }
        private void Delete_JH_Row_Click(object sender, EventArgs e)
        {
            try
            {
                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
                int curRow = ctx.Position.Row;
                if (grdjobhistory.Rows.Count < 3)
                {
                    Global.Msg("At least one row is required.");
                    return;
                }

                #region Check if the row is current history
                
                int rowJobID = Convert.ToInt32(grdjobhistory[curRow, 8].Value.ToString());
                BusinessLogic.HRM.Employee employees = new BusinessLogic.HRM.Employee();
                int currentJob = employees.CurrentJobHistoryID(Convert.ToInt32(txtemployeeID.Text));
                if(currentJob == rowJobID)
                {
                    Global.Msg("Active job detail can not be deleted.");
                    return;
                }

                #endregion
                if (MessageBox.Show("Are you sure you want to delete the job history?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                
                if (employees.DeleteJobHistory(Convert.ToInt32(grdjobhistory[ctx.Position.Row,8].ToString())) > 0)
                {
                    grdjobhistory.Rows.Remove(ctx.Position.Row);
                    return;
                }
                else
                {
                    Global.Msg("Unable to delete the record.");
                }

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void grdemployee_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                
                //Get the Selected Row
                int CurRow = grdemployee.Selection.GetSelectionRegion().GetRowsIndex()[0];
                int E_ID =Convert.ToInt32( grdemployee[CurRow, 0].Value.ToString());
                LoadEmployeeDetails(E_ID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
              //  Global.Msg("Invalid selection");
            }
        }

        public void Handle_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            ispicupdate = true;
            pbPhoto.Image = Misc.GetImageFromByte(null);
            Picture = null;
            txtImageLocation.Text = string.Empty;
        }

        private void txtcode_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            string str = " where StaffCode LIKE '" + txtcode.Text + "%' and FirstName Like '%" + txtname.Text + "%' and MiddleName Like '%" + txtMName.Text + "%' and LastName Like '%" + txtLName.Text + "%' order by StaffCode";
            try
            {
                dTable = employees.EmployeeDetails(str);
                drFound = dTable.Select(this.FilterString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            FillEmployee();
        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void FillJobHistory(DataTable dtjobhistory)
        {
            GridType = "JOBHISTORY";
           
            grdjobhistory.Rows.Clear();
            grdjobhistory.Redim(dtjobhistory.Rows.Count+1, 9);
            WriteHeader();
            for (int i = 0; i <= dtjobhistory.Rows.Count-1; i++)
            {
                DataRow dr = dtjobhistory.Rows[i];
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::HRM.Properties.Resources.gnome_window_close;
                grdjobhistory[i+1, 0] = btnDelete;
                grdjobhistory[i + 1, 0].AddController(JHClick);
                grdjobhistory[i+1, 1] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime( dr["JoinDate"].ToString())));
                grdjobhistory[i+1, 2] = new SourceGrid.Cells.Cell(Date.ToSystem(Convert.ToDateTime(dr["RetirementDate"].ToString()))); 
                grdjobhistory[i+1, 3] = new SourceGrid.Cells.Cell(dr["DepartmentName"].ToString());
                grdjobhistory[i+1, 4] = new SourceGrid.Cells.Cell(dr["DesignationName"].ToString());
                grdjobhistory[i + 1, 5] = new SourceGrid.Cells.Cell(dr["FacultyName"].ToString());
                cmbcopystatus.SelectedIndex = Convert.ToInt32(dr["Status"].ToString());
                grdjobhistory[i+1, 6] = new SourceGrid.Cells.Cell(cmbcopystatus.Text);
                cmbcopytype.SelectedIndex = Convert.ToInt32(dr["Type"].ToString());
                grdjobhistory[i + 1, 7] = new SourceGrid.Cells.Cell(cmbcopytype.Text);
                grdjobhistory[i + 1, 8] = new SourceGrid.Cells.Cell(dr["EmployeementDetailsID"].ToString());

            }
            grdjobhistory.Visible = true;
        }

        private void btnadvancedsearch_Click(object sender, EventArgs e)
        {
            frmAdvanceSearch advancesearch = new frmAdvanceSearch();
            advancesearch.ShowDialog();
        }

        private void txtstartingsalary_Leave(object sender, EventArgs e)
        {
            try
            {
                txtbasicsalary.Text = (Convert.ToDouble(txtstartingsalary.Text) + Convert.ToDouble(txtadusted.Text)).ToString();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void txtadusted_Leave(object sender, EventArgs e)
        {
            try
            {
                string txtAdjust = txtadusted.Text;
                if (txtAdjust == "")
                {
                    txtadusted.Text = txtAdjust = "0";
                }
                txtbasicsalary.Text = (Convert.ToDouble(txtstartingsalary.Text) + Convert.ToDouble(txtAdjust)).ToString();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
                txtadusted.Text = "0.00";
                txtadusted_Leave(null, null);
            }
        }

        private void txtstartingsalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtadusted_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            // allow - sign in the first position

            if (e.KeyChar == '-')
            {
                e.Handled = false;
            }
        }

        private void txtcitamount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtinsurancepremium_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void TCEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbMonth.Items.Clear();
            LoadMonths();
            //fillSalaryPayInfo();
            //fillDeduction();
        }
        private void LoadMonths()
        {
            //Check Fiscal year(By default in English)
            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();
                 
            DateTime start = Convert.ToDateTime(CompDetails.FYFrom); //English fiscal year

            ListItem[] ListDate = new ListItem[12];
            for (int month = 0; month < 12; month++)
            {
                ListDate[month] = new ListItem();
                ListDate[month].ID = month + 1; 
                ListDate[month].Value = Date.GetMonthList((Date.DateType)Date.DefaultDate, Language.LanguageType.English)[month + 1];
            }
            //if(CompDetails.FYFrom != null)  
            DateTime FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);

            //Convert Fiscal year to nepali
            int refYear = 0;
            int FYMonth = FYStartDate.Month;
            int refDay = 0;
            //If DateType is Nepali, load Nepali month
            if (Date.DefaultDate == Date.DateType.Nepali)
                Date.EngToNep(start, ref refYear, ref FYMonth, ref refDay);

            //Get the nepali fiscal year starting month
            int MonthCounter = FYMonth;
            // old code to load month combobox starting from start month
            //do
            //{
            //    if (MonthCounter > 12)
            //        MonthCounter = 1;
            //    cmbMonth.Items.Add(ListDate[MonthCounter - 1]);
            //    MonthCounter++;

            //} while (MonthCounter != FYMonth);

            // new code 
            for (int i = 0; i < 12; i++)
            {
                cmbMonth.Items.Add(ListDate[MonthCounter - 1]);
                if (MonthCounter >= 12)
                    MonthCounter = 1;
                else
                    MonthCounter++;
            }
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
           TotalEarning = 0;
           TotalDeduction = 0;
            ListItem li = (ListItem)cmbMonth.SelectedItem;
            monthID = li.ID;
            earningRowCount = 2;
            deductionRowCount = 2;
            fillSalaryPayInfo();
            if (isSalaryGenerated == true)
            {
                fillDeduction();
                if (earningRowCount > deductionRowCount)
                {
                    while (earningRowCount > deductionRowCount)
                    {
                        grdSalaryPayment[deductionRowCount, 2] = new SourceGrid.Cells.Cell("");
                        //grdSalaryPayment[deductionRowCount, 3] = new SourceGrid.Cells.Cell("");

                        deductionRowCount++;
                    }
                }
                else if (deductionRowCount > earningRowCount)
                {
                    while (deductionRowCount > earningRowCount)
                    {
                        grdSalaryPayment[earningRowCount, 0] = new SourceGrid.Cells.Cell("");
                        grdSalaryPayment[earningRowCount, 1] = new SourceGrid.Cells.Cell("");
                        earningRowCount++;
                    }
                }
                int rowcount = grdSalaryPayment.Rows.Count;
                grdSalaryPayment.Rows.Insert(rowcount);
                grdSalaryPayment[rowcount - 1, 0] = new SourceGrid.Cells.Cell("Total Earning");
                grdSalaryPayment[rowcount - 1, 0].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                grdSalaryPayment[rowcount - 1, 1] = new SourceGrid.Cells.Cell(TotalEarning.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
                //grdSalaryPayment[rowcount - 1, 1].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;


                grdSalaryPayment[rowcount - 1, 2] = new SourceGrid.Cells.Cell("Total Deduction");
               // grdSalaryPayment[rowcount - 1, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

                grdSalaryPayment[rowcount - 1, 3] = new SourceGrid.Cells.Cell(TotalDeduction.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
               // grdSalaryPayment[rowcount - 1, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;

                grdSalaryPayment[rowcount, 0] = new SourceGrid.Cells.Cell("");
                grdSalaryPayment[rowcount, 1] = new SourceGrid.Cells.Cell("");
                grdSalaryPayment[rowcount, 2] = new SourceGrid.Cells.Cell("Net Payable Salary");
                grdSalaryPayment[rowcount, 2].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
                grdSalaryPayment[rowcount, 3] = new SourceGrid.Cells.Cell((TotalEarning - TotalDeduction).ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces)));
               // grdSalaryPayment[rowcount, 3].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            }

        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            frmProgress ProgressForm = new frmProgress();
            // Initialize the thread that will handle the background process
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    ProgressForm.ShowDialog();
                }
            ));

            backgroundThread.Start();
            //Update the progressbar
            ProgressForm.UpdateProgress(20, "Initializing Report Viewer...");

            //It clear the previous records of dataset on crystal report...when this button is pressed
            dsPaySlip.Clear();
            //otherwise it populate the records again and again


            rptPaySlip rpt = new rptPaySlip();
            //Fill the logo on the report
            Misc.WriteLogo(dsPaySlip, "tblImage");
            //Set DataSource to be dsTrial dataset on the report
            rpt.SetDataSource(dsPaySlip);
            //Provide values to the parameters on the report
            CrystalDecisions.Shared.ParameterValues pvCollection = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Name = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Address = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Phone = new CrystalDecisions.Shared.ParameterDiscreteValue();
           // CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_PAN = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvCompany_Slogan = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Head = new CrystalDecisions.Shared.ParameterDiscreteValue();
          //  CrystalDecisions.Shared.ParameterDiscreteValue pdvFiscal_Year = new CrystalDecisions.Shared.ParameterDiscreteValue();
            CrystalDecisions.Shared.ParameterDiscreteValue pdvReport_Date = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvPreparedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvCheckedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();
            //CrystalDecisions.Shared.ParameterDiscreteValue pdvApprovedBy = new CrystalDecisions.Shared.ParameterDiscreteValue();

            CrystalDecisions.Shared.ParameterDiscreteValue pdvFont = new CrystalDecisions.Shared.ParameterDiscreteValue();

            //Update the progressbar
            ProgressForm.UpdateProgress(50, "Initializing Report Viewer...");

            pdvFont.Value = "Arial";
            pvCollection.Clear();
            pvCollection.Add(pdvFont);
            rpt.DataDefinition.ParameterFields["Font"].ApplyCurrentValues(pvCollection);

            CompanyDetails m_CompanyDetails = CompanyInfo.GetInfo();

            pdvCompany_Name.Value = m_CompanyDetails.CompanyName;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Name);
            rpt.DataDefinition.ParameterFields["Company_Name"].ApplyCurrentValues(pvCollection);

            pdvCompany_Address.Value = m_CompanyDetails.Address1;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Address);
            rpt.DataDefinition.ParameterFields["Company_Address"].ApplyCurrentValues(pvCollection);

            //pdvCompany_PAN.Value = m_CompanyDetails.PAN;
            //pvCollection.Clear();
            //pvCollection.Add(pdvCompany_PAN);
            //rpt.DataDefinition.ParameterFields["Company_PAN"].ApplyCurrentValues(pvCollection);

            pdvCompany_Phone.Value = "Phone No.: " + m_CompanyDetails.Telephone;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Phone);
            rpt.DataDefinition.ParameterFields["Company_Phone"].ApplyCurrentValues(pvCollection);

            pdvCompany_Slogan.Value = m_CompanyDetails.Website;
            pvCollection.Clear();
            pvCollection.Add(pdvCompany_Slogan);
            rpt.DataDefinition.ParameterFields["Company_Slogan"].ApplyCurrentValues(pvCollection);

            pdvReport_Head.Value = "Pay Slip For The Month of"+" "+cmbMonth.Text;
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Head);
            rpt.DataDefinition.ParameterFields["Report_Head"].ApplyCurrentValues(pvCollection);

            //// pdvFiscal_Year.Value = "Fiscal Year:" +Date.ToSystem( m_CompanyDetails.FYFrom);
            //pdvFiscal_Year.Value = "Fiscal Year:" + m_CompanyDetails.FiscalYear;
            //pvCollection.Clear();
            //pvCollection.Add(pdvFiscal_Year);
            //rpt.DataDefinition.ParameterFields["Fiscal_Year"].ApplyCurrentValues(pvCollection);

            //Update the progressbar
            ProgressForm.UpdateProgress(80, "Calculating Parameters...");

           
            pdvReport_Date.Value = "As On Date:" + Date.ToSystem(Date.GetServerDate());
            pvCollection.Clear();
            pvCollection.Add(pdvReport_Date);
            rpt.DataDefinition.ParameterFields["Report_Date"].ApplyCurrentValues(pvCollection);


            PrintPaySlip();

            CrystalDecisions.Shared.ExportOptions CrExportOptions = new CrystalDecisions.Shared.ExportOptions();
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            CrDiskFileDestinationOptions.DiskFileName = FileName;

            //Finally, show the report form
            frmReportViewer frm = new frmReportViewer();
            frm.SetReportSource(rpt);
            //Update the progressbar
            ProgressForm.UpdateProgress(100, "Showing Report...");

            // Close the dialog
            ProgressForm.CloseForm();

            switch (prntDirect)
            {
                case 1:
                    rpt.PrintOptions.PrinterName = "";
                    rpt.PrintToPrinter(1, false, 0, 0);
                    prntDirect = 0;
                    return;
                case 2:
                    ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                    CrExportOptions = rpt.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    rpt.Export();
                    rpt.Close();
                    return;
                case 3:
                    PdfFormatOptions CrFormatTypeOptionsPdf = new PdfFormatOptions();
                    CrExportOptions = rpt.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsPdf;
                    rpt.Export();
                    rpt.Close();
                    return;
                case 4:
                    ExcelFormatOptions CrFormatTypeOptionsEmail = new ExcelFormatOptions();
                    CrExportOptions = rpt.ExportOptions;
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptionsEmail;
                    rpt.Export();
                    frmemail sendemail = new frmemail(FileName, 1);
                    sendemail.ShowDialog();
                    rpt.Close();
                    return;
                default:
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }

            frm.WindowState = FormWindowState.Maximized;
        }

        public void PrintPaySlip()
        {
            DataTable dtEmployeeDetails = employees.FillEmployeeDetails(employeeID);
            DataRow dremployeedetails = dtEmployeeDetails.Rows[0];
            dsPaySlip.Tables["tblPaySlipMaster"].Rows.Add(dremployeedetails["StaffCode"].ToString(), dremployeedetails["FirstName"].ToString() + " " + dremployeedetails["LastName"].ToString(), dremployeedetails["DepartmentName"].ToString(), dremployeedetails["DesignationName"].ToString(),Date.ToSystem(Convert.ToDateTime( dremployeedetails["JoinDate"].ToString())), dremployeedetails["PFNumber"].ToString(), dremployeedetails["CIFNumber"].ToString());

            for (int i = 0; i < grdSalaryPayment.Rows.Count - 2; i++)
            {
                dsPaySlip.Tables["tblPaySlipDetails"].Rows.Add(grdSalaryPayment[i + 2, 0].Value.ToString(), grdSalaryPayment[i + 2, 1].Value.ToString(), grdSalaryPayment[i + 2, 2].Value.ToString(), grdSalaryPayment[i + 2, 3].Value.ToString());
            }
        }

        private void btnDeletePayInfo_Click(object sender, EventArgs e)
        {
            try
            {
                ListItem li = (ListItem)cmbMonth.SelectedItem;
                int ID = li.ID;
                string monthName1 = li.Value;
                CompanyDetails CompDetails = new CompanyDetails();
                CompDetails = CompanyInfo.GetInfo();
                DateTime FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);
                if (Global.MsgQuest("Do you want to delete the Pay Slip record of " + txtFirstName.Text + " " + txtLastName.Text + " of the month " + monthName1) == DialogResult.Yes)
                {
                    if(BusinessLogic.HRM.Employee.DeletePaySlip(ID,Convert.ToInt32(txtemployeeID.Text.Trim()),FYStartDate))
                    {
                        Global.Msg("Delete Successful");
                        grdSalaryPayment.Rows.Clear();
                        cmbMonth.Text = "";
                        btnDeletePayInfo.Visible = false;
                    }
                    else
                    {
                        Global.MsgError("There has been an error while deleting records.");
                    }
                }
            }
            catch(Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool chkUserPermission = UserPermission.ChkUserPermission("HRM_DELETE");
                if (chkUserPermission == false)
                {
                    Global.MsgError("Sorry! you dont have permission to delete employee. Please contact your administrator for permission.");
                   return;
               }
                int empId = Convert.ToInt32(txtemployeeID.Text.Trim());
                if(!employees.CheckEmpPaySlip(empId))
                {
                    Global.Msg("The employee you are about to delete has records on Salary Sheet, so the employee record can not be deleted.");
                    return;
                }
                if (Global.MsgQuest("Do you want to delete the employee " + txtFirstName.Text + " " + txtLastName.Text + " with employee code: " + txtEmployeeCode.Text.Trim()) == DialogResult.Yes)
                {
                    if(employees.DeleteEmployee(empId))
                    {
                        Global.Msg("The employee has been deleted successfully.");
                        btnNew_Click(sender, e);
                        frmEmployeeRegistration_Load(sender, e);
                    }
                    else
                    {
                        Global.Msg("There has been a problem while deleting the employee.");
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void rbtnyes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnyes.Checked == true)
                txtpfnumber.Enabled = true;
            else
            {
                txtpfnumber.Enabled = false;
            }
        }
        ListItem liLevel;
        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbLevel.SelectedIndex != -1)
                {
                    liLevel = (ListItem)cmbLevel.SelectedItem;
                    DataTable dt = Hrm.GetEmpLevelByID(liLevel.ID);
                    decimal sSal = Convert.ToDecimal(dt.Rows[0]["LevelBasicSalary"]);
                    txtstartingsalary.Text = sSal.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    double bSal = (Convert.ToDouble(txtstartingsalary.Text) + Convert.ToDouble(txtadusted.Text));
                    txtbasicsalary.Text = bSal.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));


                    //Grade calculation according to level and job start and current date
                    int maxGrade = Convert.ToInt32(dt.Rows[0]["MaxGradeNo"]);

                    #region old code ( grade calculation according to join date)

                    //if(txtjoindate.MaskCompleted)
                    //{
                    //    DateTime joindate = Date.ToDotNet(txtjoindate.Text);
                    //    int numYears = GetDifferenceInYears(joindate); 
                    #endregion

                    #region newly changed code ( grade calculation according to Grade Increment Date )
                    if (txtGrdIncrmtDate.MaskCompleted)
                    {
                        DateTime gradedate = Date.ToDotNet(txtGrdIncrmtDate.Text);
                        int numYears = GetDifferenceInYears(gradedate); 
                    #endregion

                        if(numYears > maxGrade)
                        {
                            txtEmpGrade.Text = maxGrade.ToString();
                        }
                        else
                        {
                            txtEmpGrade.Text = numYears.ToString();
                        }
                    }
                    else
                    {
                        txtEmpGrade.Text = "0";
                    }
                }

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private static int GetDifferenceInYears(DateTime startDate)
        {
            int finalResult = 0;

            const int DaysInYear = 365;

            DateTime currDate = DateTime.Now;

            TimeSpan timeSpan = currDate - startDate;

            if (timeSpan.TotalDays > 365)
            {
                //finalResult = (int)Math.Round((timeSpan.TotalDays / DaysInYear), MidpointRounding.ToEven); // this expression gives th result, rounding off the year i.e 7.88 ~ 8
                finalResult = Convert.ToInt32( Math.Floor(Math.Abs(timeSpan.TotalDays / Convert.ToInt32(DaysInYear)))); // this expression does not result in round off
            }

            return finalResult;
        }

        //private void EMICalculate()
        //{
        //    double LoanAmount = 0;
        //    double Payment = 0;
        //    double InterestRate = 0;
        //    double PaymentPeriods = 0;
        //    try
        //    {
        //        InterestRate = txtRate.Text == ""?0: Convert.ToDouble(txtRate.Text);
        //        PaymentPeriods = txtDuration.Text == "" ? 0 : Convert.ToDouble(txtDuration.Text) * 12;
        //        LoanAmount = txtPrincipal.Text == "" ? 0 : Convert.ToDouble(txtPrincipal.Text);
        //        if (InterestRate > 1)
        //        {
        //            InterestRate = InterestRate / 100;
        //        }
        //        Payment = (LoanAmount * Math.Pow((InterestRate / 12) + 1,
        //                  (PaymentPeriods)) * InterestRate / 12) / (Math.Pow
        //                  (InterestRate / 12 + 1, (PaymentPeriods)) - 1);
        //        //lblMonthlyPayment.Text = "Monthly Payment: " + Payment.ToString("N2");
        //        lblPremium.Text = Payment.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
        //    }
        //    catch(Exception ex)
        //    {
        //        Global.MsgError(ex.Message);
        //    }
        //}
                
        private void txtDuration_Leave(object sender, EventArgs e)
        {
            //EMICalculate();
            calculateLoanEndDate();
            calculateRemainingLoanMonth();
        }

        private void calculateLoanEndDate()
        {
            try
            {
                if (txtDuration.Text != "" && txtLoanStartDate.MaskCompleted)
                {
                    int numMonth = Convert.ToInt32(txtDuration.Text);
                    DateTime startDate = Date.ToDotNet(txtLoanStartDate.Text);
                    DateTime endDate = startDate.AddMonths(numMonth);
                    txtLoanEndDate.Text = Date.ToSystem(endDate);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void calculateRemainingLoanMonth()
        {
            try
            {
                if(txtLoanEndDate.MaskCompleted)// && m_mode == EntryMode.NEW)
                {
                    int numMonth = 0;
                    DateTime endDate = Date.ToDotNet(txtLoanEndDate.Text);
                    double days = (endDate - DateTime.Now).TotalDays;
                    numMonth = (int)days / 30;
                    txtRemainingMonth.Text = numMonth.ToString();
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void txtPrincipal_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if(e.KeyChar == (char)13)
            //{
            //    EMICalculate();
            //    txtRate.Select();
            //}
        }

        private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)13)
            //{
            //    EMICalculate();
            //    txtDuration.Select();
            //}
        }

        private void txtDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)13)
            //{
            //    EMICalculate();
            //}
        }

        /// <summary>
        /// Select Zone to According to DistrictID
        /// </summary>
        /// <param name="cmbDist"></param>
        /// <param name="cmbZone"></param>
        private void LoadCmbZoneByDist(ComboBox cmbDist, ComboBox cmbZone)
        {
            if (cmbDist.SelectedIndex != -1)
            {
                liDistrict = (ListItem)cmbDist.SelectedItem;
                int districtId = liDistrict.ID;
                //int districtId = Convert.ToInt32(cmbDist.SelectedValue.ToString());
                DataTable dt = District.GetZoneByDist(districtId);
                if (dt.Rows.Count > 0)
                {
                    cmbZone.Text = dt.Rows[0]["ZoneName"].ToString();
                    // cmbZone.SelectedIndex = cmbZone.Items.IndexOf(Convert.ToInt32(dt.Rows[0]["ZoneID"].ToString()));
                }
            }
        }
        private void cmbPermDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCmbZoneByDist(cmbPermDistrict, cmbPermZone);
        }

        private void cmbTempDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCmbZoneByDist(cmbTempDistrict, cmbTempZone);
        }

        private void rbtnInsYes_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtnInsYes.Checked == true)
            {
                txtInsNumber.Enabled = true;
                txtInsAmt.Enabled = true;
                txtInsPremium.Enabled = true;
            }
            else
            {
                txtInsNumber.Enabled = false;
                txtInsAmt.Enabled = false;
                txtInsPremium.Enabled = false;
            }
        }

        private void rbtnPensionYes_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtnPensionYes.Checked == true)
            {
                txtPensionNumber.Enabled = true;
            }
            else
            {
                txtPensionNumber.Enabled = false;
            }
        }

        private void cmbLoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cmbLoan.SelectedIndex != -1)
                {
                    liLoan = (ListItem)cmbLoan.SelectedItem;
                    DataTable dt = Hrm.GetLoanByID(liLoan.ID);
                    lblInstallment.Text = dt.Rows[0]["LoanType"].ToString();
                    if(dt.Rows[0]["LoanType"].ToString() == "Fix Installment")
                    {
                        txtMthInterest.Visible = false;
                        txtMthDecreaseAmt.Visible = false;
                        lblMthInterest.Visible = false;
                        lblMthDecrease.Visible = false;
                    }
                    else
                    {
                        txtMthInterest.Visible = true;
                        txtMthDecreaseAmt.Visible = true;
                        lblMthInterest.Visible = true;
                        lblMthDecrease.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        
        private void txtPrincipal_Leave(object sender, EventArgs e)
        {
            DisplayMonthlyPremium();
        }

        private void DisplayMonthlyPremium()
        {
            try
            {
                if (lblInstallment.Text != "Fix Installment")
                {
                    if (txtMthInstallment.Text != "" && txtMthDecreaseAmt.Text != "" && txtMthInterest.Text != "")
                    {
                        decimal total = 0;
                        total = (Convert.ToDecimal(txtMthInstallment.Text) + Convert.ToDecimal(txtMthInterest.Text)) - Convert.ToDecimal(txtMthDecreaseAmt.Text);
                        lblPremium.Text = total.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    }
                }
                else
                {
                    if (txtMthInstallment.Text != "")
                    {
                        lblPremium.Text = txtMthInstallment.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void txtMthInstallment_Leave(object sender, EventArgs e)
        {
            DisplayMonthlyPremium();
        }

        private void txtMthInterest_Leave(object sender, EventArgs e)
        {
            DisplayMonthlyPremium();
        }

        private void txtMthDecreaseAmt_Leave(object sender, EventArgs e)
        {
            DisplayMonthlyPremium();
        }

        private void txtAdvAmt_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtAdvAmt.Text != "")
                {
                    if (!UserValidation.validDecimal(txtAdvAmt.Text))
                    {
                        Global.Msg("Invalid Amount.");
                        txtAdvAmt.Select();
                        return;
                    }
                    decimal amt = Convert.ToDecimal(txtAdvAmt.Text)/12;
                    txtAdvMthInstallment.Text = amt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    txtAdvRemainingAmt.Text = txtAdvAmt.Text;
                }

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        //private void CalculateAdvRemainingAmt()
        //{
        //    try
        //    {
        //        if (txtAdvStartDate.MaskCompleted && m_mode == EntryMode.NEW)
        //        {
        //            decimal mthInstallment = Convert.ToDecimal(txtAdvMthInstallment.Text);
        //            //int numMth = (DateTime.Now - Date.ToDotNet(txtAdvStartDate.Text)).TotalDays;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MsgError(ex.Message);
        //    }
        //}

        private void txtAdvStartDate_Leave(object sender, EventArgs e)
        {
            AutoAdvEndDate();
        }

        private void AutoAdvEndDate()
        {
            try
            {
                if (txtAdvStartDate.MaskCompleted)
                {
                    DateTime endDate = Date.ToDotNet(txtAdvStartDate.Text).AddYears(1);
                    txtAdvEndDate.Text = Date.ToSystem(endDate);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnLoanStartDate_Click(object sender, EventArgs e)
        {
            DateStatus = "LOANSTARTDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtLoanStartDate.Text));
            frm.ShowDialog();
            calculateLoanEndDate();
            calculateRemainingLoanMonth();
        }

        private void btnLoanEndDate_Click(object sender, EventArgs e)
        {
            DateStatus = "LOANENDDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtLoanEndDate.Text));
            frm.ShowDialog();
            calculateRemainingLoanMonth();
        }

        private void btnAdvTakenDate_Click(object sender, EventArgs e)
        {
            DateStatus = "ADVSTARTDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtAdvStartDate.Text));
            frm.ShowDialog();
            AutoAdvEndDate();
        }

        private void btnAdvRetrunDate_Click(object sender, EventArgs e)
        {
            DateStatus = "ADVENDDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtAdvEndDate.Text));
            frm.ShowDialog();
        }

        private void txtLoanStartDate_Leave_1(object sender, EventArgs e)
        {
            calculateLoanEndDate();
            calculateRemainingLoanMonth();
        }

        private void txtLoanEndDate_Leave_1(object sender, EventArgs e)
        {
            calculateRemainingLoanMonth();
        }

        private void rbtnQuarterYes_CheckedChanged(object sender, EventArgs e)
        {
            //if (rbtnQuarterYes.Checked == true)
            //{
            //    txtAccommodation.Enabled = true;
            //    txtElectricity.Enabled = 
            //}
            //else
            //{
            //    txtAccommodation.Enabled = false;
            //}
            txtElectricity.Enabled = txtAccommodation.Enabled = rbtnQuarterYes.Checked;
        }

        //private void rbtnLoanYes_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (rbtnLoanYes.Checked)
        //        {
        //            //cmbLoan.SelectedIndex = 0;
        //            cmbLoan.Enabled = true;
        //            txtPrincipal.Enabled = true;
        //            txtMthInstallment.Enabled = true;
        //            txtMthInterest.Enabled = true;
        //            txtMthInstallment.Enabled = true;
        //            txtDuration.Enabled = true;
        //            txtRemainingMonth.Enabled = true;
        //            txtLoanStartDate.Enabled = true;
        //            txtLoanEndDate.Enabled = true;
        //            btnLoanStartDate.Enabled = true;
        //            btnLoanEndDate.Enabled = true;
        //            txtMthDecreaseAmt.Enabled = true;
        //        }
        //        else
        //        {
        //            cmbLoan.Enabled = false;
        //            txtPrincipal.Enabled = false;
        //            txtMthInstallment.Enabled = false;
        //            txtMthInterest.Enabled = false;
        //            txtMthInstallment.Enabled = false;
        //            txtDuration.Enabled = false;
        //            txtRemainingMonth.Enabled = false;
        //            txtLoanStartDate.Enabled = false;
        //            txtLoanEndDate.Enabled = false;
        //            btnLoanStartDate.Enabled = false;
        //            btnLoanEndDate.Enabled = false;
        //            txtMthDecreaseAmt.Enabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MsgError(ex.Message);               
        //    }
        //}

        //private void rbtnAdvYes_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (rbtnAdvYes.Checked)
        //        {
        //            txtAdvAmt.Enabled = true;
        //            txtAdvMthInstallment.Enabled = true;
        //            txtAdvStartDate.Enabled = true;
        //            txtAdvEndDate.Enabled = true;
        //            btnAdvTakenDate.Enabled = true;
        //            btnAdvRetrunDate.Enabled = true;
        //            txtAdvRemainingAmt.Enabled = true;
        //        }
        //        else
        //        {
        //            txtAdvAmt.Enabled = false;
        //            txtAdvMthInstallment.Enabled = false;
        //            txtAdvStartDate.Enabled = false;
        //            txtAdvEndDate.Enabled = false;
        //            btnAdvTakenDate.Enabled = false;
        //            btnAdvRetrunDate.Enabled = false;
        //            txtAdvRemainingAmt.Enabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MsgError(ex.Message);
        //    }
        //}

        private void rbtnsingle_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnSingle.Checked)
                chkCoupleWork.Checked = false;
        }

        private void chkCoupleWork_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCoupleWork.Checked)
                rbtnMarried.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            View.frmEmpFaculty fef = new View.frmEmpFaculty();
            fef.ShowDialog();
            cmbFaculty.Items.Clear();
            LoadComboboxItems(cmbFaculty);
        }

        private void txtname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                txtMName.Select();
            }
        }

        private void txtMName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                txtLName.Select();
            }
        }

        private void txtbirthdate_TextChanged(object sender, EventArgs e)
        {
            if (txtBirthDate.MaskCompleted)
            {
                try
                {
                    DateTime birthDate = Date.ToDotNet(txtBirthDate.Text);
                    DateTime endDate = birthDate.AddYears(63);
                    //txtenddate.Clear();
                    txtenddate.Text = Date.ToSystem(endDate);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void txtenddate_TextChanged(object sender, EventArgs e)
        {
            if (txtenddate.MaskCompleted)
            {
                DateTime zeroTime = new DateTime(1, 1, 1);

                DateTime dt = Date.ToDotNet(txtenddate.Text);
                int result = DateTime.Compare(dt, DateTime.Today);
                if (result >= 0)
                {
                    TimeSpan span = dt - DateTime.Today;
                    int remainYrs = (zeroTime + span).Year - 1;

                    lblRemainingYrs.Text = remainYrs.ToString() + (remainYrs == 1 ? " Year" : " Years") + " remaining.";
                }
                else
                {
                    lblRemainingYrs.Text = "Age limit crossed.";
                }

            }
        }

        bool isEmployeementDetailsChanged = false;
        
        private void cmbdepartment_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isEmployeementDetailsChanged = true;
        }

        private void cmbdesignation_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isEmployeementDetailsChanged = true;
        }

        private void cmbFaculty_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isEmployeementDetailsChanged = true;
        }

        private void cmbstatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isEmployeementDetailsChanged = true;
        }

        private void cmbtype_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isEmployeementDetailsChanged = true;
        }

        private void btnLoanAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbLoan.SelectedIndex == -1)
                {
                    Global.Msg("Please select a Loan Name.");
                    cmbLoan.DroppedDown = true;
                    return;
                }
                if (txtPrincipal.Text == "")
                {
                    Global.Msg("Please enter principal amount.");
                    txtPrincipal.Select();
                    return;
                }
                if (txtMthInstallment.Text == "")
                {
                    Global.Msg("Please enter installment amount.");
                    txtMthInstallment.Select();
                    return;
                }
                if (txtDuration.Text == "")
                {
                    Global.Msg("Please enter total number of month.");
                    txtDuration.Select();
                    return;
                }

                if (!txtLoanStartDate.MaskCompleted)
                {
                    Global.Msg("Please enter loan start date.");
                    txtLoanStartDate.Select();
                    return;
                }

                
                liLoan = (ListItem)cmbLoan.SelectedItem;
                WriteLoanRow(grdLoan.Rows.Count, liLoan.ID, cmbLoan.Text, lblInstallment.Text, Convert.ToDecimal(txtPrincipal.Text), 
                    Convert.ToDecimal(txtMthInstallment.Text), txtMthInterest.Text == "" ? 0 : Convert.ToDecimal(txtMthInterest.Text),
                    txtMthDecreaseAmt.Text == "" ? 0 : Convert.ToDecimal(txtMthDecreaseAmt.Text), txtDuration.Text == "" ? 0 : Convert.ToInt32(txtDuration.Text), 
                    txtRemainingMonth.Text == "" ? 0 : Convert.ToInt32(txtRemainingMonth.Text), txtLoanStartDate.Text, txtLoanEndDate.Text,
                    Convert.ToDecimal(lblPremium.Text), Convert.ToDecimal(txtMthInstallment.Text));
             
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void WriteLoanRow(int curRow,int LoanID,string Name,string Type,decimal Principal,decimal MthInstall,decimal MthInterest,decimal PerMthDecAmt,int TotalMth,int RemMth,string SDate,string EDate,decimal Premium, decimal initialInstallment, int ELID =0)
        {
            grdLoan.Redim(Convert.ToInt32(grdLoan.Rows.Count + 1), grdLoan.ColumnsCount);
            grdLoan[curRow, 0] = new SourceGrid.Cells.Cell(LoanID);
            grdLoan[curRow, 1] = new SourceGrid.Cells.Cell("X");
            grdLoan[curRow, 1].AddController(evtRemoveLoan);
            grdLoan[curRow, 2] = new SourceGrid.Cells.Cell(Name);
            grdLoan[curRow, 3] = new SourceGrid.Cells.Cell(Type);
            grdLoan[curRow, 4] = new SourceGrid.Cells.Cell(Principal);
            grdLoan[curRow, 5] = new SourceGrid.Cells.Cell(MthInstall);
            grdLoan[curRow, 6] = new SourceGrid.Cells.Cell(MthInterest);
            grdLoan[curRow, 7] = new SourceGrid.Cells.Cell(PerMthDecAmt);
            grdLoan[curRow, 8] = new SourceGrid.Cells.Cell(TotalMth);
            grdLoan[curRow, 9] = new SourceGrid.Cells.Cell(RemMth);
            grdLoan[curRow, 10] = new SourceGrid.Cells.Cell(SDate);
            grdLoan[curRow, 11] = new SourceGrid.Cells.Cell(EDate);
            grdLoan[curRow, 12] = new SourceGrid.Cells.Cell(Premium);
            grdLoan[curRow, 13] = new SourceGrid.Cells.Cell(initialInstallment);
            grdLoan[curRow, 14] = new SourceGrid.Cells.Cell(ELID);

        }

        private void btnAdvanceAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdvAmt.Text == "")
                {
                    Global.Msg("Please enter advance amount or select 'no' for advance taken.");
                    txtAdvAmt.Select();
                    return;
                }
                if (txtAdvMthInstallment.Text == "")
                {
                    Global.Msg("Please enter advance monthly installment amount or select 'no' for advance taken.");
                    txtAdvMthInstallment.Select();
                    return;
                }

                if (!txtAdvStartDate.MaskCompleted)
                {
                    Global.Msg("Please enter advance taken date.");
                    txtAdvStartDate.Select();
                    return;
                }

                WriteAdvanceRow(grdAdvance.Rows.Count, txtAdvTitle.Text, Convert.ToDecimal(txtAdvAmt.Text), Convert.ToDecimal(txtAdvMthInstallment.Text), 
                    txtAdvStartDate.Text, txtAdvEndDate.Text, Convert.ToDecimal(txtAdvRemainingAmt.Text));
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void WriteAdvanceRow(int curRow,string title,decimal totalAmt, decimal installment,string takenDate,string returnDate,decimal remainingAmt)
        {
            grdAdvance.Redim(Convert.ToInt32(grdAdvance.Rows.Count + 1), grdAdvance.ColumnsCount);
            grdAdvance[curRow, 0] = new SourceGrid.Cells.Cell("X");
            grdAdvance[curRow, 0].AddController(evtRemoveAdvance);
            grdAdvance[curRow, 1] = new SourceGrid.Cells.Cell(title);
            grdAdvance[curRow, 2] = new SourceGrid.Cells.Cell(totalAmt);
            grdAdvance[curRow, 3] = new SourceGrid.Cells.Cell(installment);
            grdAdvance[curRow, 4] = new SourceGrid.Cells.Cell(takenDate);
            grdAdvance[curRow, 5] = new SourceGrid.Cells.Cell(returnDate);
            grdAdvance[curRow, 6] = new SourceGrid.Cells.Cell(remainingAmt);
        }

        private void btnStartDate_Click(object sender, EventArgs e)
        {
            

            DateStatus = "STARTDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtStartDate.Text));
            frm.ShowDialog();
        }

        private void btnGrdIncrmtDate_Click(object sender, EventArgs e)
        {
            try
            {
                DateStatus = "GRDINCRMTDATE";
                frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtGrdIncrmtDate.Text));
                frm.ShowDialog();


            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                //MessageBox.Show("What the Ctrl+F?");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void txtGrdIncrmtDate_Leave(object sender, EventArgs e)
        {
            cmbLevel_SelectedIndexChanged(null, e);
        }

        private void cmbEthnicity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void grdemployee_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }
  
    }
}
