using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using BusinessLogic.HOS;
using Common;
using DateManager;

namespace Hospital.View
{
    public partial class frmDoctorRegistration : Form,IfrmDateConverter
    
    {
        public frmDoctorRegistration()
        {
            InitializeComponent();
        }
        ListItem liDistrict = new ListItem();
        private string DateStatus = "";
        bool ispicupdate = false;
        private byte[] Picture = null;
        ListItem liZone = new ListItem();
        ListItem liNationality = new ListItem();
        ListItem liSpecilization = new ListItem();
        ListItem liDepartment = new ListItem();
        ListItem liFaculty = new ListItem();
        ListItem liEthnicity = new ListItem();
        ListItem liLevel=new ListItem();
        ListItem liBank = new ListItem();

        DataTable dtQualification = new DataTable();
        DataTable dtExperiences = new DataTable();
        DataTable dtLoan = new DataTable();
        DataTable dtAdvance = new DataTable();
        private string GridType = "";
        private int CurrRowPos = 0;
        private EntryMode m_mode = EntryMode.NORMAL;
        private bool IsFieldChanged = false;
        private DataRow[] drFound;
        public DataTable FromOpeningBalance = new DataTable();
        public DataTable FromPreYearBalance = new DataTable();
        private DataTable dTable;
        private string FilterString = "";
        TextBox txtdoctorID = new TextBox();
        Doctor doctor = new Doctor();
        bool isDoctorDetailsChanged = false;
        private string doctorledger = "";
        ListItem liLoan = new ListItem();

     

        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtInstituteFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtDeletee = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtCompanyFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtRemoveLoan = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents evtRemoveAdvance = new SourceGrid.Cells.Controllers.CustomEvents();
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;
        private enum CmbType
        {

            District,
            Zone,
            Ethnicity, 
            Religion,
            Level,
            Loan
        }


        private void frmDoctorRegister_Load(object sender, EventArgs e)
        {

            Doctor doctors = new Doctor();
            dTable = doctors.GetDoctorDetails("Order by DoctorCode");
            drFound = dTable.Select(FilterString);

            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grddoctor_DoubleClick);

            gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
            gridKeyDown.KeyDown += new KeyEventHandler(Handle_KeyDown);
            grdDoctor.Controller.AddController(gridKeyDown);

            LoadComboboxItems(cmbNationality);
            LoadComboboxItems(cmbdepartment);
            LoadComboboxItems(cmbSpecilization);
            LoadComboboxItems(cmbFaculty);
            LoadComboboxItems(cmbbankname);
            LoadComboBox(cmbEthnicity, CmbType.Ethnicity);
            LoadComboBox(cmbPermDistrict, CmbType.District);
            LoadComboBox(cmbPermZone, CmbType.Zone);
            LoadComboBox(cmbTempDistrict, CmbType.District);
            LoadComboBox(cmbTempZone, CmbType.Zone);
            LoadComboBox(cmbLevel, CmbType.Level);
            LoadComboBox(cmbLoan, CmbType.Loan);

            txtBirthDate.Text = Date.ToSystem(Date.GetServerDate());
            txtStartDate.Text = Date.ToSystem(Date.GetServerDate());
            txtjoindate.Text = Date.ToSystem(Date.GetServerDate());
            txtenddate.Text = Date.ToSystem(Date.GetServerDate());


            //txtDatetime for Loan
            txtAdvEndDate.Text = Date.ToSystem(Date.GetServerDate().AddYears(1));
            txtAdvStartDate.Text = Date.ToSystem(Date.GetServerDate());
            txtLoanEndDate.Text = Date.ToSystem(Date.GetServerDate());
            txtLoanStartDate.Text = Date.ToSystem(Date.GetServerDate());


            txtGrdIncrmtDate.Text = Date.ToSystem(Date.GetServerDate());

            btnNew.Enabled = false;
            btnEdit.Enabled = false;

            evtDelete.Click += new EventHandler(Delete_Row_Click);
            evtInstituteFocusLost.FocusLeft += new EventHandler(evtInstituteFocusLost_FocusLeft);
            evtDeletee.Click += new EventHandler(workexperienceDelete_Row_Click);
            evtCompanyFocusLost.FocusLeft += new EventHandler(evtCompanyFocusLost_FocusLeft);

            grdjobhistory.Visible = false;
            FillDoctor();
            FillEducation();
            FillExperience();
           
            FillLoan();
            FillAdvance();
            ChangeState(EntryMode.NEW);
            cmbDocType.SelectedIndex = 0;

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
            grdworkexperience[i, 1].Value = "Enter Company Name";

            SourceGrid.Cells.Editors.TextBox txtdesignation = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtdesignation.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdworkexperience[i, 2] = new SourceGrid.Cells.Cell("", txtdesignation);
            grdworkexperience[i, 2].Value = "";

 
            //SourceGrid.Cells.Editors.ComboBox txtdesignation = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            //txtdesignation.EditableMode = SourceGrid.EditableMode.Focus;
            //txtdesignation.StandardValues = new String[] { ""};
            //txtdesignation.Control.DropDownStyle = ComboBoxStyle.DropDownList;
            //grdworkexperience[i, 2] = new SourceGrid.Cells.Cell("", txtdesignation);
            //grdworkexperience[i, 2].Value = "";

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
        public void Handle_KeyDown(object sender, KeyEventArgs e)
        {
        }
        private void grddoctor_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                
                //Get the Selected Row
                int CurRow = grdDoctor.Selection.GetSelectionRegion().GetRowsIndex()[0];
                int D_ID =Convert.ToInt32( grdDoctor[CurRow, 0].Value.ToString());
                LoadDoctorDetails(D_ID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
              
            }
        }
        private void LoadDoctorDetails(int docId)
        {
            DataTable dtfilldoctordetails;
            DataTable dtDoctorEmploymentDetails;

            Doctor doctor = new Doctor();
            dtfilldoctordetails = doctor.FillDoctorDetails(docId);
            DataRow drdoctordetails = dtfilldoctordetails.Rows[0];

            dtDoctorEmploymentDetails = doctor.GetDoctorEmploymentDetails(docId);

            txtdoctorID.Text = drdoctordetails["ID"].ToString();
            txtFirstName.Text = drdoctordetails["FirstName"].ToString();
            txtMiddleName.Text = drdoctordetails["MiddleName"].ToString();
            txtLastName.Text = drdoctordetails["LastName"].ToString();
            txtDoctorCode.Text = drdoctordetails["DoctorCode"].ToString();
            doctorledger = drdoctordetails["DoctorCode"].ToString() + "-" + drdoctordetails["FirstName"].ToString() + drdoctordetails["MiddleName"].ToString() + drdoctordetails["LastName"].ToString();
            if (drdoctordetails["BirthDate"].ToString() != "")
            {
                DateTime bday = Convert.ToDateTime(drdoctordetails["BirthDate"].ToString());
                txtBirthDate.Text = Date.ToSystem(bday);
            }

            if (drdoctordetails["StartDate"].ToString() != "")
            {
                DateTime sday = Convert.ToDateTime(drdoctordetails["StartDate"].ToString());
                txtStartDate.Text = Date.ToSystem(sday);
            }
            if (txtStartDate.MaskCompleted)
            {
                try
                {
                    DateTime zeroTime = new DateTime(1, 1, 1);
                    DateTime StartDate = Date.ToDotNet(txtStartDate.Text);
                    DateTime today = Date.GetServerDate();
                    TimeSpan difference = today.Subtract(StartDate);
                    int result = Convert.ToInt32(difference.TotalDays / 365.25);
                    
                    if (result == 1)
                        lblDoctorFor.Text = "Doctor Employeed for : " + result.ToString() + " year";
                    else if (result > 1)
                        lblDoctorFor.Text = "Doctor Employeed for : " + result.ToString() + " years";
                    else
                        lblDoctorFor.Text = "";
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
            }

           
            if (drdoctordetails["Gender"].ToString() == "1")
                rbtnMale.Checked = true;
            else if (drdoctordetails["Gender"].ToString() == "2")
                rbtnFemale.Checked = true;
            else
                rbtnOther.Checked = true;

            if (drdoctordetails["IsSingle"].ToString() == "True")
                rbtnSingle.Checked = true;
            else if (drdoctordetails["IsSingle"].ToString() == "False")
                rbtnMarried.Checked = true;


            txtPermAddress.Text = drdoctordetails["PermAddress"].ToString();
            txtTempAddress.Text = drdoctordetails["TempAddress"].ToString();

            cmbPermDistrict.Text = drdoctordetails["PermanentDistrict"].ToString();

            cmbPermZone.Text = drdoctordetails["PermanentZone"].ToString();
            cmbTempDistrict.Text = drdoctordetails["TemporaryDistrict"].ToString();
            cmbTempZone.Text = drdoctordetails["TemporaryZone"].ToString();
            cmbNationality.Text = drdoctordetails["NationalityName"].ToString();
            txtCitizenshipNo.Text = drdoctordetails["CitizenshipNo"].ToString();
            txtFatherName.Text = drdoctordetails["FatherName"] != DBNull.Value ? drdoctordetails["FatherName"].ToString() : "";
            txtGFatherName.Text = drdoctordetails["GrandfatherName"] != DBNull.Value ? drdoctordetails["GrandfatherName"].ToString() : "";
            cmbReligion.Text = drdoctordetails["Religion"].ToString();
            cmbDocType.Text = drdoctordetails["DoctorType"].ToString();
            cmbEthnicity.Text = drdoctordetails["EthnicityName"].ToString();
            txtPhone1.Text = drdoctordetails["Phone1"].ToString();
            txtPhone2.Text = drdoctordetails["Phone2"].ToString();
            txtEmail.Text = drdoctordetails["Email"].ToString();
            txtDoctorNote.Text = drdoctordetails["DoctorNote"].ToString();
          

            if (drdoctordetails["DoctorPhoto"].ToString() != "")
            {
                pbPhoto.Image = Misc.GetImageFromByte((byte[])drdoctordetails["DoctorPhoto"]);
            }

            else
            {
                pbPhoto.Image = Misc.GetImageFromByte(null);
            }

           // DataTable dtjobhistory = doctor.JobHistory(empId);
            //evtJobHistoryDelete.Click += new EventHandler(Delete_JH_Row_Click);
            //FillJobHistory(dtjobhistory);
           // dtjobhistory.Rows.Clear();
            if (dtDoctorEmploymentDetails.Rows.Count > 0)
            {
                DataRow drEmploymentDetails = dtDoctorEmploymentDetails.Rows[0];

                txtjoindate.Text = Date.ToSystem(Convert.ToDateTime(drEmploymentDetails["JoinDate"].ToString()));
                txtenddate.Text = Date.ToSystem(Convert.ToDateTime(drEmploymentDetails["RetirementDate"].ToString()));
                cmbdepartment.Text = drEmploymentDetails["DepartmentName"].ToString();
                cmbSpecilization.Text = drEmploymentDetails["SpecilizationName"].ToString();
                cmbFaculty.Text = drEmploymentDetails["FacultyName"].ToString();
                cmbtype.SelectedIndex = Convert.ToInt32(drEmploymentDetails["Type"].ToString());
                cmbstatus.SelectedIndex = Convert.ToInt32(drEmploymentDetails["Status"].ToString());
            }

          
            //Clearing rows before adding rows for another doctor
            int maxRows = grdacademicqualification.RowsCount - 1;
            if (maxRows > 1)
                grdacademicqualification.Rows.RemoveRange(1, maxRows - 1);

            DataTable dtqualification = doctor.DoctorQualification(docId);
            int RowCount = 0;
            foreach (DataRow DrQualification in dtqualification.Rows)
            {
                int RowNum = grdacademicqualification.RowsCount - 1;
                RowCount = RowNum;
                grdacademicqualification.Rows.Insert(RowNum + 1);
                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
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

                //SourceGrid.Cells.Editors.TextBox txtcourse = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                //txtcourse.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                //grdacademicqualification[RowNum, 3] = new SourceGrid.Cells.Cell("", txtcourse);
                //grdacademicqualification[RowNum, 3].Value = DrQualification["Course"].ToString();

                SourceGrid.Cells.Editors.TextBox txtpercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtpercentage.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdacademicqualification[RowNum, 3] = new SourceGrid.Cells.Cell("", txtpercentage);
                grdacademicqualification[RowNum, 3].Value = DrQualification["Percentage"].ToString();

                SourceGrid.Cells.Editors.TextBox txtpastyear = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtpastyear.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                grdacademicqualification[RowNum, 4] = new SourceGrid.Cells.Cell("", txtpastyear);
                grdacademicqualification[RowNum, 4].Value = DrQualification["PassYear"].ToString();
                
            }
            AddRowEducation(RowCount + 1);

            //Clearing rows before adding rows for another doctor
            maxRows = grdworkexperience.RowsCount - 1;
            if (maxRows > 1)
                grdworkexperience.Rows.RemoveRange(1, maxRows - 1);

            DataTable dtExperience = doctor.DoctorExperience(docId);
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
                grdworkexperience[i, 2].Value = DrExperience["Specilization"].ToString();

               
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
            for (int i = 0; i < 7; i++)
            TCDoctor.TabPages[i].Enabled = false;
            txtFirstName.Enabled = false;
            txtMiddleName.Enabled = false;
            txtLastName.Enabled = false;
            btnSave.Enabled = false;
            dtQualification.Columns.Clear();
            dtExperiences.Columns.Clear();

            //For Salary and Grade
            decimal StartingSalary = Convert.ToDecimal(drdoctordetails["StartingSalary"].ToString());
            txtstartingsalary.Text = StartingSalary.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            object pfAdj = drdoctordetails["PFAdjust"];

            decimal PFAdjust = Convert.ToDecimal(pfAdj == DBNull.Value ? 0 : pfAdj);
            txtPFAdjust.Text = PFAdjust.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            object pensionAdj = drdoctordetails["PensionAdjust"];
            decimal PensionAdjust = Convert.ToDecimal(pensionAdj == DBNull.Value ? 0 : pensionAdj);
            txtPensionAdjust.Text = PensionAdjust.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            decimal Adjusted = Convert.ToDecimal(drdoctordetails["Adjusted"].ToString());
            txtadusted.Text = Adjusted.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            if (drdoctordetails["IsPF"].ToString() == "True")
                rbtnyes.Checked = true;
            else
                rbtnno.Checked = true;
            txtpfnumber.Text = drdoctordetails["PFNumber"].ToString();
            if (drdoctordetails["isPension"].ToString() == "True")
                rbtnPensionYes.Checked = true;
            else
                rbtnPensionNo.Checked = true;
            txtPensionNumber.Text = drdoctordetails["PensionNumber"].ToString();

            cmbLevel.Text = drdoctordetails["LevelName"].ToString();

            if (drdoctordetails["isInsurance"].ToString() == "True")
                rbtnInsYes.Checked = true;
            else
                rbtnInsNo.Checked = true;
            decimal InsuranceAmount = Convert.ToDecimal(drdoctordetails["InsuranceAmount"].ToString());
            txtInsAmt.Text = InsuranceAmount.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            txtInsNumber.Text = drdoctordetails["InsuranceNumber"].ToString();
            decimal InsurancePremium = Convert.ToDecimal(drdoctordetails["InsurancePremium"].ToString());
            txtInsPremium.Text = InsurancePremium.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));


            txtcifno.Text = drdoctordetails["CIFNumber"].ToString();
            txtcitamount.Text = drdoctordetails["CITAmount"].ToString();
            if (drdoctordetails["BankID"].ToString() == "-1")
                cmbbankname.Text = "";
            else
                cmbbankname.Text = Ledger.GetLedgerNameFromID(Convert.ToInt32(drdoctordetails["BankID"].ToString()));

            txtbankacnumber.Text = drdoctordetails["BankACNumber"].ToString();
            decimal AcademicAlw = Convert.ToDecimal(drdoctordetails["AcademicAlw"].ToString());
            txtAcademicAlw.Text = AcademicAlw.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            decimal BasicSalary = Convert.ToDecimal(drdoctordetails["BasicSalary"].ToString());
            txtbasicsalary.Text = BasicSalary.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            decimal InflationAlw = Convert.ToDecimal(drdoctordetails["InflationAlw"].ToString());
            txtInflationAlw.Text = InflationAlw.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            decimal AdmistrativeAlw = Convert.ToDecimal(drdoctordetails["AdmistrativeAlw"].ToString());
            txtAdmAlw.Text = AdmistrativeAlw.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            object overTimeAlw = drdoctordetails["OverTimeAlw"];
            decimal OverTimeAllow = Convert.ToDecimal(overTimeAlw == DBNull.Value ? 0 : overTimeAlw);
            txtOverTimeAlw.Text = OverTimeAllow.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            txttada.Text = drdoctordetails["TADA"].ToString();
            txtmiscpaid.Text = drdoctordetails["MiscAllowance"].ToString();
            decimal NLKoshDeduct = Convert.ToDecimal(drdoctordetails["NLKoshDeduct"].ToString());
            txtNLKoshdeduct.Text = NLKoshDeduct.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            txtNLKoshNo.Text = drdoctordetails["NLKoshNo"].ToString();
            txtKKNum.Text = drdoctordetails["KalyankariNo"].ToString();
            decimal KalyankariAmt = Convert.ToDecimal(drdoctordetails["KalyankariAmt"].ToString());
            txtKKAmt.Text = KalyankariAmt.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            txtremarks.Text = drdoctordetails["Remarks"].ToString();
            decimal PostAlw = Convert.ToDecimal(drdoctordetails["PostAlw"].ToString());
            txtPostAlw.Text = PostAlw.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            decimal Electricity = Convert.ToDecimal(drdoctordetails["ElectricityCharge"]);
            txtElectricity.Text = Electricity.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));

            if (drdoctordetails["isQuarter"].ToString() == "True")
            {
                rbtnQuarterYes.Checked = true;
                decimal accommodation = Convert.ToDecimal(drdoctordetails["Accommodation"].ToString());
                txtAccommodation.Text = accommodation.ToString(Misc.FormatNumber(Global.Comma_Separated, Global.DecimalPlaces));
            }
            else
            {
                rbtnQuarterNo.Checked = true;
                txtAccommodation.Text = "0";
            }

            #region Loan & Advance
            FillLoan(docId);
            FillAdvance(docId);
          

            #endregion
            
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
        private void evtInstituteFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row + 1;
            AddRowEducation(CurrRowPos);
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
        }
        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnDelete.Enabled = Delete;
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
        }

        private void LoadComboBox(ComboBox cbo, CmbType type)
        {
            try
            {

                cbo.Items.Clear();
                DataTable dt = new DataTable();
                switch (type)
                {
                    case CmbType.Zone:
                        dt = Zone.GetZone();
                        break;
                    case CmbType.District:
                        dt = District.GetDistrict();
                        break;
                    case CmbType.Ethnicity:
                        dt = Ethnicity.GetEthIdValue();
                        break;
                    case CmbType.Level:
                        dt = Doctor.GetDocLevelForCmb();
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
                MessageBox.Show(ex.Message);
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
        private void LoadCmbZoneByDist(ComboBox cmbDist, ComboBox cmbZone)
        {
            if (cmbDist.SelectedIndex != -1)
            {
                liDistrict = (ListItem)cmbDist.SelectedItem;
                int districtId = liDistrict.ID;
                DataTable dt = District.GetZoneByDist(districtId);
                if (dt.Rows.Count > 0)
                {
                    cmbZone.Text = dt.Rows[0]["ZoneName"].ToString();                   
                }
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
                    DataTable dtdepartment = Hos.getdepartment();
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
                else if(comboboxitems == cmbSpecilization)
                {

                    DataTable dtSpecilization = Hos.getSpecilization();
                    if (dtSpecilization.Rows.Count > 0)
                    {
                        foreach (DataRow drSpecilization in dtSpecilization.Rows)
                        {
                            comboboxitems.Items.Add(new ListItem((int)drSpecilization["SpecilizationID"], drSpecilization["SpecilizationName"].ToString()));

                        }
                        comboboxitems.SelectedIndex = 0;
                        comboboxitems.DisplayMember = "value";
                        comboboxitems.ValueMember = "id";
                    }
                }
                else if (comboboxitems == cmbFaculty)
                {

                    DataTable dtFaculty = Hrm.GetEmpFacultyForCmb();
                    if (dtFaculty.Rows.Count > 0)
                    {
                        foreach (DataRow drFaculty in dtFaculty.Rows)
                        {
                            comboboxitems.Items.Add(new ListItem((int)drFaculty["ID"], drFaculty["Value"].ToString()));

                        }
                        comboboxitems.SelectedIndex = 0;
                        comboboxitems.DisplayMember = "value";
                        comboboxitems.ValueMember = "id";
                    }
                }
                else if (comboboxitems == cmbbankname)
                {
                    DataTable dtbankname = Doctor.getBankName();
                    foreach (DataRow drbank in dtbankname.Rows)
                    {
                        comboboxitems.Items.Add(new ListItem((int)drbank["BankID"], drbank["BankName"].ToString()));

                    }
                    comboboxitems.SelectedIndex = -1;
                    comboboxitems.DisplayMember = "value";
                    comboboxitems.ValueMember = "id";
                }
               
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

       
        public void DateConvert(DateTime DotNetDate)
        {
            if (DateStatus == "BIRTHDATE")
                txtBirthDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "JOINDATE")
                txtjoindate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "RETIREMENTDATE")
                txtenddate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "STARTDATE")
                txtStartDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "GRDINCRMTDATE")
                 txtGrdIncrmtDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "ADVSTARTDATE")
                txtAdvStartDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "ADVENDDATE")
                txtAdvEndDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "LOANSTARTDATE")
                txtLoanStartDate.Text = Date.ToSystem(DotNetDate);
            else if (DateStatus == "LOANENDDATE")
                txtLoanEndDate.Text = Date.ToSystem(DotNetDate);
        }
        private void btnStartDate_Click(object sender, EventArgs e)
        {

            DateStatus = "STARTDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtStartDate.Text));
            frm.ShowDialog();
        }

        private void btnBirthDate_Click(object sender, EventArgs e)
        {
            DateStatus = "BIRTHDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtBirthDate.Text));
            frm.ShowDialog();
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

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ispicupdate = true;
            pbPhoto.Image = Misc.GetImageFromByte(null);
            Picture = null;
            txtImageLocation.Text = string.Empty;
        }
        private void WriteHeader()
        {
            if (GridType == "DOCTOR")
            {

                grdDoctor[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
                grdDoctor[0, 1] = new SourceGrid.Cells.ColumnHeader("Code");
                grdDoctor[0, 2] = new SourceGrid.Cells.ColumnHeader("Doctor Name");

                grdDoctor[0, 0].Column.Width = 1;
                grdDoctor[0, 1].Column.Width = 100;
                grdDoctor[0, 2].Column.Width = 210;

                grdDoctor[0, 0].Column.Visible = false;

            }
            else if (GridType == "EDUCATION")
            {
                grdacademicqualification[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
                grdacademicqualification[0, 1] = new SourceGrid.Cells.ColumnHeader("Name of Institute");
                grdacademicqualification[0, 2] = new SourceGrid.Cells.ColumnHeader("Board");
               // grdacademicqualification[0, 3] = new SourceGrid.Cells.ColumnHeader("Level/Course");
                grdacademicqualification[0, 3] = new SourceGrid.Cells.ColumnHeader("Percentage");
                grdacademicqualification[0, 4] = new SourceGrid.Cells.ColumnHeader("Passed Year");

                grdacademicqualification[0, 0].Column.Width = 30;
                grdacademicqualification[0, 1].Column.Width = 350;
                grdacademicqualification[0, 2].Column.Width = 150;
              //  grdacademicqualification[0, 3].Column.Width = 150;
                grdacademicqualification[0, 3].Column.Width = 70;
                grdacademicqualification[0, 4].Column.Width = 90;
            }
            else if (GridType == "EXPERIENCE")
            {
                grdworkexperience[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
                grdworkexperience[0, 1] = new SourceGrid.Cells.ColumnHeader("Company Name");
                grdworkexperience[0, 3] = new SourceGrid.Cells.ColumnHeader("From Date");
                grdworkexperience[0, 4] = new SourceGrid.Cells.ColumnHeader("To Date");
                grdworkexperience[0, 2] = new SourceGrid.Cells.ColumnHeader("Specilization");

                grdworkexperience[0, 0].Column.Width = 30;
                grdworkexperience[0, 1].Column.Width = 350;
                grdworkexperience[0, 3].Column.Width = 100;
                grdworkexperience[0, 4].Column.Width = 100;
                grdworkexperience[0, 2].Column.Width = 150;
            }
            if (GridType == "LOAN")
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
                grdLoan[0, 14] = new SourceGrid.Cells.ColumnHeader("DLID");

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
           
           
        }
       
        private void AddRowEducation(int RowCount)
        {
            grdacademicqualification.Redim(Convert.ToInt32(RowCount + 1), grdacademicqualification.ColumnsCount);
            SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("X");

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

            //SourceGrid.Cells.Editors.TextBox txtcourse = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            //txtcourse.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            //grdacademicqualification[i, 3] = new SourceGrid.Cells.Cell("", txtcourse);

            SourceGrid.Cells.Editors.TextBox txtpercentage = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtpercentage.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdacademicqualification[i, 3] = new SourceGrid.Cells.Cell("", txtpercentage);
            grdacademicqualification[i, 3].Value = "";

            SourceGrid.Cells.Editors.TextBox txtpastyear = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            txtpastyear.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
            grdacademicqualification[i, 4] = new SourceGrid.Cells.Cell("", txtpastyear);
            grdacademicqualification[i, 4].Value = "(AD)";
            
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
        private void evtCompanyFocusLost_FocusLeft(object sender, EventArgs e)
        {
            //If the row is not modified or in the (NEW) mode, just skip
            SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
            CurrRowPos = ctx.Position.Row + 1;
            AddRowExperience(CurrRowPos);
        }
   //     DataTable dt = Doctor.getSpecilizationName();
        
        private class EditorFile : SourceGrid.Cells.Editors.TextBoxButton
        {
            public EditorFile()
                : base(typeof(string))
            {
                Control.DialogOpen += new EventHandler(Control_DialogOpen);
            }

            void Control_DialogOpen(object sender, EventArgs e)
            {
               
                //using (frmTestPicker dg = new frmTestPicker())
                TextBox txtcurrentdate = new TextBox();
                txtcurrentdate.Text = Date.ToSystem(Date.GetServerDate());
                frmDoctorRegistration docreg = new frmDoctorRegistration();
                using (frmDateConverter dg = new frmDateConverter(docreg, Date.ToDotNet(txtcurrentdate.Text)))
                {
                    if (dg.ShowDialog(EditCellContext.Grid) == DialogResult.OK)
                    {
                        Control.Value = Date.ToSystem(Convert.ToDateTime(dg.result));
                    }
                }
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
        private void FillExperience()
        {
            GridType = "EXPERIENCE";
            grdworkexperience.Rows.Clear();
            grdworkexperience.Redim(1, 5);
            WriteHeader();
            AddRowExperience(1);
        }
        private void FillDoctor()
        {
            GridType = "DOCTOR";
            grdDoctor.Rows.Clear();
            grdDoctor.Redim(drFound.Count() + 1, 3);
            WriteHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {
                DataRow dr = drFound[i - 1];
                grdDoctor[i, 0] = new SourceGrid.Cells.Cell(dr["ID"].ToString());
                grdDoctor[i, 1] = new SourceGrid.Cells.Cell(dr["DoctorCode"].ToString());
                grdDoctor[i, 1].AddController(dblClick);

                grdDoctor[i, 2] = new SourceGrid.Cells.Cell(dr["DoctorName"].ToString());
                grdDoctor[i, 2].AddController(dblClick);
                grdDoctor[i, 2].AddController(gridKeyDown);
            }


        }
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
            switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW:// Insert doctor Information
                    try
                    {
                        Doctor doctor = new Doctor();

                        if (CheckEmptyTxt(txtFirstName, "Please provide the first name"))
                            return;
                        if (CheckEmptyTxt(txtLastName, "Please provide the last name"))
                            return;
                        if (CheckEmptyTxt(txtDoctorCode, "Please provide the Doctor code"))
                            return;
                        //else if (!doctor.CheckStaffCode(txtDoctorCode.Text.Trim(), Convert.ToInt32(txtdoctorID.Text)))
                        //{
                        //    MessageBox.Show("Employee Code already in use");
                        //    txtDoctorCode.Focus();
                        //    return;
                        //}
                        if (cmbSpecilization.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select a Specilization");
                            TCDoctor.SelectedIndex = 1;
                            cmbSpecilization.DroppedDown = true;
                            return;
                        }
                        if (cmbdepartment.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select a department");
                            TCDoctor.SelectedIndex = 1;
                            cmbdepartment.DroppedDown = true;
                            return;
                        }

                        if (cmbDocType.SelectedIndex == -1)
                        {
                            Global.Msg("Please select an Doctor type.");
                            TCDoctor.SelectedIndex = 0;
                            cmbDocType.DroppedDown = true;
                            return;
                        }
                        if (cmbLevel.SelectedIndex == -1)
                        {
                            Global.Msg("Please select a Level.");
                            TCDoctor.SelectedIndex = 4;
                            cmbLevel.DroppedDown = true;
                            return;
                        }




                        BusinessLogic.HOS.DoctorDetails doc = new BusinessLogic.HOS.DoctorDetails();
                        doc.FirstName = txtFirstName.Text;
                        doc.MiddleName = txtMiddleName.Text;
                        doc.LastName = txtLastName.Text;
                        doc.DoctorCode = txtDoctorCode.Text.Trim();
                        doc.BirthDate = Date.ToDotNet(txtBirthDate.Text);
                        doc.StartDate = Date.ToDotNet(txtStartDate.Text);
                        doc.Gender = rbtnMale.Checked ? 1 : rbtnFemale.Checked ? 2 : 0;
                        doc.IsSingle = rbtnSingle.Checked ? 1 : 0;
                        doc.PermAddress = txtPermAddress.Text;
                        doc.TdocAddress = txtTempAddress.Text;

                        if (cmbPermDistrict.SelectedIndex == -1)
                            doc.PermDist = -1;
                        else
                        {
                            liDistrict = (ListItem)cmbPermDistrict.SelectedItem;
                            int PermDist = liDistrict.ID;
                            doc.PermDist = PermDist;
                        }
                        if (cmbPermZone.SelectedIndex == -1)
                            doc.PermZone = -1;
                        else
                        {
                            liZone = (ListItem)cmbPermZone.SelectedItem;
                            int PermZone = liZone.ID;
                            doc.PermZone = PermZone;
                        }
                        if (cmbTempDistrict.SelectedIndex == -1)
                            doc.TdocDist = -1;
                        else
                        {
                            liDistrict = (ListItem)cmbTempDistrict.SelectedItem;
                            int TempDist = liDistrict.ID;
                            doc.TdocDist = TempDist;
                        }

                        if (cmbTempZone.SelectedIndex == -1)
                            doc.TdocZone = -1;
                        else
                        {
                            liZone = (ListItem)cmbTempZone.SelectedItem;
                            int TempZone = liZone.ID;
                            doc.TdocZone = TempZone;
                        }
                        liNationality = (ListItem)cmbNationality.SelectedItem;
                        int nationalityid = liNationality.ID;
                        doc.NationalityID = nationalityid;
                        doc.CitizenshipNumber = txtCitizenshipNo.Text;
                        doc.FatherName = txtFatherName.Text;
                        doc.GrandfatherName = txtGFatherName.Text;
                        doc.Religion = cmbReligion.Text;
                        doc.DoctorType = cmbDocType.Text;
                        liEthnicity = (ListItem)cmbEthnicity.SelectedItem;
                        int ethnicityID = liEthnicity.ID;
                        doc.EthnicityID = ethnicityID;
                        doc.Phone1 = txtPhone1.Text;
                        doc.Phone2 = txtPhone2.Text;
                        doc.Email = txtEmail.Text;
                        doc.DoctorNote = txtDoctorNote.Text;
                        if (Picture != null)
                            doc.DoctorPhoto = Picture;
                        else
                            doc.DoctorPhoto = null;
                        doc.JoinDate = Date.ToDotNet(txtjoindate.Text);
                        doc.EndDate = Date.ToDotNet(txtenddate.Text);
                        liDepartment = (ListItem)cmbdepartment.SelectedItem;
                        int departmentid = liDepartment.ID;
                        doc.DepartmentID = departmentid;
                        liSpecilization = (ListItem)cmbSpecilization.SelectedItem;
                        int designationid = liSpecilization.ID;
                        liFaculty = (ListItem)cmbFaculty.SelectedItem;
                        doc.FacultyID = liFaculty.ID;
                        doc.SpecilizationID = designationid;
                        doc.Type = cmbtype.SelectedIndex;
                        doc.Status = cmbstatus.SelectedIndex;

                        dtQualification.Clear();//Clear datatable before entering new datas
                        //Save Education Grid Data to Datatable
                        dtQualification.Columns.Clear();
                        dtQualification.Columns.Add("InstituteName");
                        dtQualification.Columns.Add("Board");
                     //   dtQualification.Columns.Add("Course");
                        dtQualification.Columns.Add("Percentage");
                        dtQualification.Columns.Add("PassYear");
                        for (int i = 0; i < grdacademicqualification.Rows.Count - 2; i++)
                        {
                           // dtQualification.Rows.Add(grdacademicqualification[i + 1, 1].Value, grdacademicqualification[i + 1, 2].Value, grdacademicqualification[i + 1, 3].Value, grdacademicqualification[i + 1, 4].Value, grdacademicqualification[i + 1, 5].Value);
                            dtQualification.Rows.Add(grdacademicqualification[i + 1, 1].Value, grdacademicqualification[i + 1, 2].Value, grdacademicqualification[i + 1, 3].Value, grdacademicqualification[i + 1, 4].Value);
                        }
                        dtExperiences.Clear();//Clear datatable before entering new datas
                        //Save the Experience Grid Data to Datatable
                        dtExperiences.Columns.Clear();
                        dtExperiences.Columns.Add("CompanyName");
                        dtExperiences.Columns.Add("FromDate");
                        dtExperiences.Columns.Add("ToDate");
                        dtExperiences.Columns.Add("Specilization");
                        for (int j = 0; j < grdworkexperience.Rows.Count - 2; j++)
                        {
                            dtExperiences.Rows.Add(grdworkexperience[j + 1, 1].Value, (Date.ToDotNet(grdworkexperience[j + 1, 3].Value.ToString())).ToString(), (Date.ToDotNet(grdworkexperience[j + 1, 4].Value.ToString())).ToString(), grdworkexperience[j + 1, 2].Value);
                        }

                    
                        #region Salary information
                        //For Saving Salary Information
                        doc.StartingSalary = Convert.ToDouble(txtstartingsalary.Text);
                        doc.Adjusted = Convert.ToDouble(txtadusted.Text);
                        doc.ElectricityCharge = Convert.ToDecimal(txtElectricity.Text);
                        doc.GradeIncrementDate = Date.ToDotNet(txtGrdIncrmtDate.Text);
                        liLevel = (ListItem)cmbLevel.SelectedItem;
                        int levelID = liLevel.ID;
                        doc.Level = levelID;

                        doc.IsPF = rbtnyes.Checked ? true : false;
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
                                doc.PFNumber = Convert.ToInt32(txtpfnumber.Text);
                            }
                        }
                        else
                        {
                            doc.PFNumber = 0;
                        }

                        doc.IsPension = rbtnPensionYes.Checked ? true : false;
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
                                doc.PensionNumber = txtPensionNumber.Text;
                            }
                        }
                        else
                        {
                            doc.PensionNumber = "0";
                        }

                        doc.IsInsurance = rbtnInsYes.Checked ? true : false;
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
                                doc.InsuranceNumber = txtInsNumber.Text;
                                doc.InsuranceAmt = Convert.ToDouble(txtInsAmt.Text);
                                doc.InsurancePremium = Convert.ToDouble(txtInsPremium.Text);
                            }
                        }
                        else
                        {
                            doc.InsuranceNumber = "";
                            doc.InsuranceAmt = 0;
                            doc.InsurancePremium = 0;
                        }

                        if (txtcifno.Text == "")
                            doc.CIFNumber = 0;
                        else
                            doc.CIFNumber = Convert.ToInt32(txtcifno.Text);
                        if (txtcitamount.Text == "")
                            doc.CITAmount = 0;
                        else
                            doc.CITAmount = Convert.ToDouble(txtcitamount.Text);
                        if (cmbbankname.SelectedIndex == -1)
                        {
                            doc.BankID = -1;
                        }
                        else
                        {
                            liBank = (ListItem)cmbbankname.SelectedItem;
                            doc.BankID = liBank.ID;
                        }

                        doc.ACNumber = txtbankacnumber.Text;
                        if (txtAcademicAlw.Text == "")
                        {
                            doc.AcademicAlw = 0;
                        }
                        else
                        {
                            doc.AcademicAlw = Convert.ToDouble(txtAcademicAlw.Text);
                        }
                        doc.BasicSalary = Convert.ToDouble(txtbasicsalary.Text);
                        doc.PAN = txtpan.Text;

                        if (txtInflationAlw.Text == "")
                            doc.inflationAlw = 0;
                        else
                            doc.inflationAlw = Convert.ToDouble(txtInflationAlw.Text);
                        if (txtAdmAlw.Text == "")
                            doc.AdmAlw = 0;
                        else
                            doc.AdmAlw = Convert.ToDouble(txtAdmAlw.Text);
                        doc.PostAlw = txtPostAlw.Text == "" ? 0 : Convert.ToDouble(txtPostAlw.Text);

                        if (txttada.Text == "")
                            doc.TADA = 0;
                        else
                            doc.TADA = Convert.ToDouble(txttada.Text);
                        if (txtmiscpaid.Text == "")
                            doc.MiscAllowance = 0;
                        else
                            doc.MiscAllowance = Convert.ToDouble(txtmiscpaid.Text);

                        if (txtNLKoshdeduct.Text == "")
                            doc.NLKoshDeduct = 0;
                        else
                            doc.NLKoshDeduct = Convert.ToDouble(txtNLKoshdeduct.Text);

                        doc.NLKoshNo = txtNLKoshNo.Text.Trim();

                        doc.KalyankariNo = txtKKNum.Text.Trim();
                        doc.KalyankariAmt = txtKKAmt.Text == "" ? 0 : Convert.ToDouble(txtKKAmt.Text);

                        doc.OverTimeAllow = txtOverTimeAlw.Text == "" ? 0 : Convert.ToDecimal(txtOverTimeAlw.Text);

                        doc.PFAdjust = txtPFAdjust.Text == "" ? 0 : Convert.ToDecimal(txtPFAdjust.Text);

                        doc.PensionAdjust = txtPensionAdjust.Text == "" ? 0 : Convert.ToDecimal(txtPensionAdjust.Text);


                        doc.Remarks = txtremarks.Text;

                        doc.IsQuarter = rbtnQuarterYes.Checked == true ? true : false;
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
                                doc.Accommodation = Convert.ToDouble(txtAccommodation.Text);
                            }
                        }
                        else
                        {
                            doc.Accommodation = 0;
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



                         int doctorID = 0;
                         string insertResult = doctor.CreateDoctor(doc, dtQualification, dtExperiences, dtLoan, dtAdvance, out doctorID);
                        if (insertResult == "SUCCESS")
                        {
                            btnNew_Click(sender, e);
                            ChangeState(EntryMode.NEW);
                            frmDoctorRegister_Load(sender, e);
                        }

                    }
                    catch(Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion

                #region EDIT
                case EntryMode.EDIT: //Update doctor Information
                    try
                    {
                       Doctor doctor = new Doctor();
                       BusinessLogic.HOS.DoctorDetails doc = new BusinessLogic.HOS.DoctorDetails();

                        if (CheckEmptyTxt(txtFirstName, "Please provide the first name"))
                            return;
                        if (CheckEmptyTxt(txtLastName, "Please provide the last name"))
                            return;
                        if (CheckEmptyTxt(txtDoctorCode, "Please provide the doctor code"))
                            return;
                      
                        if (cmbDocType.SelectedIndex == -1)
                        {
                            Global.Msg("Please select an doctor type.");
                            TCDoctor.SelectedIndex = 0;
                            cmbDocType.DroppedDown = true;
                            return;
                        }
                        if (cmbSpecilization.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select a designation");
                            TCDoctor.SelectedIndex = 1;
                            cmbSpecilization.DroppedDown = true;
                            return;
                        }
                        if (cmbdepartment.SelectedIndex == -1)
                        {
                            MessageBox.Show("Please select a department");
                            TCDoctor.SelectedIndex = 1;
                            cmbdepartment.DroppedDown = true;
                            return;
                        }
                        doc.DoctorID = Convert.ToInt32(txtdoctorID.Text);
                        doc.FirstName = txtFirstName.Text;
                        doc.MiddleName = txtMiddleName.Text;
                        doc.LastName = txtLastName.Text;
                        doc.DoctorCode = txtDoctorCode.Text;
                        doc.BirthDate = Date.ToDotNet(txtBirthDate.Text);
                        doc.StartDate = Date.ToDotNet(txtStartDate.Text);
                        doc.Gender = rbtnMale.Checked ? 1 : rbtnFemale.Checked ? 2 : 0;
                        doc.IsSingle = rbtnSingle.Checked ? 1 : 0;
                        doc.PermAddress = txtPermAddress.Text;
                        doc.TdocAddress = txtTempAddress.Text;
                        if (cmbPermDistrict.SelectedIndex == -1)
                            doc.PermDist = -1;
                        else
                        {
                            liDistrict = (ListItem)cmbPermDistrict.SelectedItem;
                            int PermDist = liDistrict.ID;
                            doc.PermDist = PermDist;
                        }

                        if (cmbPermZone.SelectedIndex == -1)
                            doc.PermZone = -1;
                        else
                        {
                            liZone = (ListItem)cmbPermZone.SelectedItem;
                            int PermZone = liZone.ID;
                            doc.PermZone = PermZone;
                        }

                        if (cmbTempDistrict.SelectedIndex == -1)
                            doc.TdocDist = -1;
                        else
                        {
                            liDistrict = (ListItem)cmbTempDistrict.SelectedItem;
                            int TempDist = liDistrict.ID;
                            doc.TdocDist = TempDist;
                        }

                        if (cmbTempZone.SelectedIndex == -1)
                            doc.TdocZone = -1;
                        else
                        {
                            liZone = (ListItem)cmbTempZone.SelectedItem;
                            int TempZone = liZone.ID;
                            doc.TdocZone = TempZone;
                        }
                        liNationality = (ListItem)cmbNationality.SelectedItem;
                        int nationalityid = liNationality.ID;
                        doc.NationalityID = nationalityid;
                        doc.CitizenshipNumber = txtCitizenshipNo.Text;
                        doc.FatherName = txtFatherName.Text;
                        doc.GrandfatherName = txtGFatherName.Text;
                        doc.Religion = cmbReligion.Text;
                        doc.DoctorType = cmbDocType.Text;
                        liEthnicity = (ListItem)cmbEthnicity.SelectedItem;
                        int ethnicityID = liEthnicity.ID;
                        doc.EthnicityID = ethnicityID;
                        doc.Phone1 = txtPhone1.Text;
                        doc.Phone2 = txtPhone2.Text;
                        doc.Email = txtEmail.Text;
                        doc.DoctorNote = txtDoctorNote.Text;
                        if (pbPhoto != null)
                            doc.DoctorPhoto = Picture;

                        doc.JoinDate = Date.ToDotNet(txtjoindate.Text);
                        doc.EndDate = Date.ToDotNet(txtenddate.Text);
                        liDepartment = (ListItem)cmbdepartment.SelectedItem;
                        int departmentid = liDepartment.ID;
                        doc.DepartmentID = departmentid;
                        liSpecilization = (ListItem)cmbSpecilization.SelectedItem;
                        int designationid = liSpecilization.ID;
                        doc.SpecilizationID = designationid;
                        liFaculty = (ListItem)cmbFaculty.SelectedItem;
                        doc.FacultyID = liFaculty.ID;
                        doc.Type = cmbtype.SelectedIndex;
                        doc.Status = cmbstatus.SelectedIndex;
                       
                     
                        doc.IsDocDetailsChanged = isDoctorDetailsChanged;

                        dtQualification.Columns.Clear();//Clear datatable before entering new datas
                        //Save Education Grid Data to Datatable
                        dtQualification.Rows.Clear();
                        dtQualification.Columns.Add("InstituteName");
                        dtQualification.Columns.Add("Board");
                      //  dtQualification.Columns.Add("Course");
                        dtQualification.Columns.Add("Percentage");
                        dtQualification.Columns.Add("PassYear");

                        for (int i = 0; i < grdacademicqualification.Rows.Count - 2; i++)
                        {
                            
                            dtQualification.Rows.Add(grdacademicqualification[i + 1, 1].Value, grdacademicqualification[i + 1, 2].Value, grdacademicqualification[i + 1, 3].Value, grdacademicqualification[i + 1, 4].Value);
                        }

                        dtExperiences.Columns.Clear();//Clear datatable before entering new datas
                        //Save the Experience Grid Data to Datatable
                        dtExperiences.Rows.Clear();
                        dtExperiences.Columns.Add("CompanyName");
                        dtExperiences.Columns.Add("FromDate");
                        dtExperiences.Columns.Add("ToDate");
                        dtExperiences.Columns.Add("Specilization");

                        for (int j = 0; j < grdworkexperience.Rows.Count - 2; j++)
                        {
                            dtExperiences.Rows.Add(grdworkexperience[j + 1, 1].Value, (Date.ToDotNet(grdworkexperience[j + 1, 3].Value.ToString())).ToString(), (Date.ToDotNet(grdworkexperience[j + 1, 4].Value.ToString())).ToString(), grdworkexperience[j + 1, 2].Value);
                        }

                        #region salary infromation modify
                        doc.GradeIncrementDate = Date.ToDotNet(txtGrdIncrmtDate.Text);
                        //For Saving Salary Information
                        doc.StartingSalary = Convert.ToDouble(txtstartingsalary.Text);
                        doc.Adjusted = Convert.ToDouble(txtadusted.Text);
                        liLevel = (ListItem)cmbLevel.SelectedItem;
                        int levelID = liLevel.ID;
                        doc.Level = levelID;
                        doc.IsPF = rbtnyes.Checked ? true : false;
                        if (txtpfnumber.Text == "")
                        {
                            doc.PFNumber = 0;
                        }
                        else
                        {
                            doc.PFNumber = Convert.ToInt32(txtpfnumber.Text);
                        }

                        doc.IsPension = rbtnPensionYes.Checked ? true : false;
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
                                doc.PensionNumber = txtPensionNumber.Text;
                            }
                        }
                        else
                        {
                            doc.PensionNumber = "0";
                        }

                        doc.OverTimeAllow = txtOverTimeAlw.Text == "" ? 0 : Convert.ToDecimal(txtOverTimeAlw.Text);

                        doc.IsInsurance = rbtnInsYes.Checked ? true : false;
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
                                doc.InsuranceNumber = txtInsNumber.Text;
                                doc.InsuranceAmt = Convert.ToDouble(txtInsAmt.Text);
                                doc.InsurancePremium = Convert.ToDouble(txtInsPremium.Text);
                            }
                        }
                        else
                        {
                            doc.InsuranceNumber = "";
                            doc.InsuranceAmt = 0;
                            doc.InsurancePremium = 0;
                        }

                        if (txtcifno.Text == "")
                            doc.CIFNumber = 0;
                        else
                            doc.CIFNumber = Convert.ToInt32(txtcifno.Text);
                        if (txtcitamount.Text == "")
                            doc.CITAmount = 0;
                        else
                            doc.CITAmount = Convert.ToDouble(txtcitamount.Text);
                        if (cmbbankname.SelectedIndex == -1)
                        {
                            doc.BankID = -1;
                        }
                        else
                        {
                            liBank = (ListItem)cmbbankname.SelectedItem;
                            doc.BankID = liBank.ID;
                        }

                        doc.ACNumber = txtbankacnumber.Text;
                        if (txtAcademicAlw.Text == "")
                        {
                            doc.AcademicAlw = 0;
                        }
                        else
                        {
                            doc.AcademicAlw = Convert.ToDouble(txtAcademicAlw.Text);
                        }
                        doc.BasicSalary = Convert.ToDouble(txtbasicsalary.Text);

                        doc.PAN = txtpan.Text;

                        if (txtInflationAlw.Text == "")
                            doc.inflationAlw = 0;
                        else
                            doc.inflationAlw = Convert.ToDouble(txtInflationAlw.Text);
                        if (txtAdmAlw.Text == "")
                            doc.AdmAlw = 0;
                        else
                            doc.AdmAlw = Convert.ToDouble(txtAdmAlw.Text);
                        doc.PostAlw = txtPostAlw.Text == "" ? 0 : Convert.ToDouble(txtPostAlw.Text);

                        if (txttada.Text == "")
                            doc.TADA = 0;
                        else
                            doc.TADA = Convert.ToDouble(txttada.Text);
                        if (txtmiscpaid.Text == "")
                            doc.MiscAllowance = 0;
                        else
                            doc.MiscAllowance = Convert.ToDouble(txtmiscpaid.Text);

                        if (txtNLKoshdeduct.Text == "")
                            doc.NLKoshDeduct = 0;
                        else
                            doc.NLKoshDeduct = Convert.ToDouble(txtNLKoshdeduct.Text);

                        doc.NLKoshNo = txtNLKoshNo.Text.Trim();

                        doc.KalyankariNo = txtKKNum.Text.Trim();
                        doc.KalyankariAmt = txtKKAmt.Text == "" ? 0 : Convert.ToDouble(txtKKAmt.Text);

                        doc.PFAdjust = txtPFAdjust.Text == "" ? 0 : Convert.ToDecimal(txtPFAdjust.Text);

                        doc.PensionAdjust = txtPensionAdjust.Text == "" ? 0 : Convert.ToDecimal(txtPensionAdjust.Text);

                        doc.Remarks = txtremarks.Text;

                        doc.IsQuarter = rbtnQuarterYes.Checked == true ? true : false;
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
                                doc.Accommodation = Convert.ToDouble(txtAccommodation.Text);
                            }
                        }
                        else
                        {
                            doc.Accommodation = 0;
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
                       
                        #endregion

                        string updateResult = doctor.UpdateDoctor(doc, dtQualification, dtExperiences, ispicupdate, dtLoan, dtAdvance);
                        if (updateResult == "SUCCESS")
                        {
                            isDoctorDetailsChanged = false;
                            string docCode = txtDoctorCode.Text;
                            btnNew_Click(sender, e);
                            txtcode.Text = docCode;
                            frmDoctorRegister_Load(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion
            }

        }

        private void grdemployee_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeState(EntryMode.NEW);
               
                for (int i = 0; i < 7; i++)
                    TCDoctor.TabPages[i].Enabled = true;
                txtFirstName.Enabled = true;
                txtMiddleName.Enabled = true;
                txtLastName.Enabled = true;
                txtFirstName.Clear();
                txtMiddleName.Clear();
                txtLastName.Clear();
                txtDoctorCode.Clear();
                txtBirthDate.Text = Date.ToSystem(Date.GetServerDate());
                txtStartDate.Text = Date.ToSystem(Date.GetServerDate());
                lblDoctorFor.Text = "";
              
                if (cmbNationality.Items.Count > 0)
                    cmbNationality.SelectedIndex = 0;
                if (cmbReligion.Items.Count > 0)
                    cmbReligion.SelectedIndex = 0;
                if (cmbDocType.Items.Count > 0)
                    cmbDocType.SelectedIndex = 0;
                txtPhone1.Clear();
                txtPhone2.Clear();
                txtEmail.Clear();
                txtDoctorNote.Clear();
                txtCitizenshipNo.Clear();
                txtFatherName.Clear();
                txtGFatherName.Clear();
                rbtnSingle.Checked = true;
               
                txtPermAddress.Clear();
                txtTempAddress.Clear();
                pbPhoto.Image = Misc.GetImageFromByte(null);
                txtjoindate.Text = Date.ToSystem(Date.GetServerDate());
                txtenddate.Text = Date.ToSystem(Date.GetServerDate().AddYears(2));
                if (cmbdepartment.Items.Count > 0)
                    cmbdepartment.SelectedIndex = 0;
                if (cmbSpecilization.Items.Count > 0)
                    cmbSpecilization.SelectedIndex = 0;
                if (cmbFaculty.Items.Count > 0)
                    cmbFaculty.SelectedIndex = 0;
                if (cmbstatus.Items.Count > 0)
                    cmbstatus.SelectedIndex = 0;
                if (cmbtype.Items.Count > 0)
                    cmbtype.SelectedIndex = 0;              

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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeState(EntryMode.EDIT);
                for (int i = 0; i < 7; i++)
                    TCDoctor.TabPages[i].Enabled = true;
                txtFirstName.Enabled = true;
                txtMiddleName.Enabled = true;
                txtLastName.Enabled = true;
                btnSave.Enabled = true;
                label25.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
               
                int docId = Convert.ToInt32(txtdoctorID.Text.Trim());
                
                if (Global.MsgQuest("Do you want to delete the doctor " + txtFirstName.Text + " " + txtLastName.Text + " with doctor code: " + txtDoctorCode.Text.Trim()) == DialogResult.Yes)
                {
                    if ((doctor.DeleteDoctor(docId)))
                    {
                        Global.Msg("The doctor has been deleted successfully.");
                        btnNew_Click(sender, e);
                    }
                    else
                    {
                        Global.Msg("There has been a problem while deleting the doctor.");
                        return;
                    }
                }
                frmDoctorRegister_Load(sender, e);
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            switch (m_mode)
            {

                #region NEW
                case EntryMode.NEW:// Insert Doctor Information
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
                case EntryMode.EDIT: //Update doctor Information
                    try
                    {
                       LoadDoctorDetails(Convert.ToInt32(txtdoctorID.Text.Trim()));
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion
            }
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            frmHosDepartment department = new frmHosDepartment();
            department.ShowDialog();
            cmbdepartment.Items.Clear();
            LoadComboboxItems(cmbdepartment);
        }

        private void btnSpecilization_Click(object sender, EventArgs e)
        {
            frmHosSpecialization specilization = new frmHosSpecialization();
            specilization.ShowDialog();
            cmbSpecilization.Items.Clear();
            LoadComboboxItems(cmbSpecilization);
        }

        private void btnDaculty_Click(object sender, EventArgs e)
        {
            frmDoctorFaculty faculty = new frmDoctorFaculty();
            faculty.ShowDialog();
            cmbFaculty.Items.Clear();
            LoadComboboxItems(cmbFaculty);
        }

        private void tpdoctoredetails_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtcode_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }
        private void Filter()
        {
            string str = " where DoctorCode LIKE '" + txtcode.Text + "%' and FirstName Like '%" + txtname.Text + "%' and MiddleName Like '%" + txtMName.Text + "%' and LastName Like '%" + txtLName.Text + "%' order by DoctorCode";
            try
            {
                dTable = doctor.GetDoctorDetails(str);
                drFound = dTable.Select(this.FilterString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            FillDoctor();
        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnadvancedsearch_Click(object sender, EventArgs e)
        {
          
        }

        private void txtjoindate_TextChanged(object sender, EventArgs e)
        {

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

        private void cmbdepartment_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isDoctorDetailsChanged = true;
        }

        private void cmbSpecilization_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isDoctorDetailsChanged = true;
        }

        private void cmbFaculty_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isDoctorDetailsChanged = true;
        }

        private void cmbstatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isDoctorDetailsChanged = true;
        }

        private void cmbtype_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isDoctorDetailsChanged = true;
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
                finalResult = Convert.ToInt32(Math.Floor(Math.Abs(timeSpan.TotalDays / Convert.ToInt32(DaysInYear)))); // this expression does not result in round off
            }

            return finalResult;
        }
        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbLevel.SelectedIndex != -1)
                {
                    liLevel = (ListItem)cmbLevel.SelectedItem;
                    DataTable dt = Doctor.GetDocLevelByID(liLevel.ID);
                    decimal sSal = Convert.ToDecimal(dt.Rows[0]["LevelBasicSalary"]);
                    txtstartingsalary.Text = sSal.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                    double bSal = (Convert.ToDouble(txtstartingsalary.Text) + Convert.ToDouble(txtadusted.Text));
                    txtbasicsalary.Text = bSal.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));


                    //Grade calculation according to level and job start and current date
                    int maxGrade = Convert.ToInt32(dt.Rows[0]["MaxGradeNo"]);               

                    #region  grade calculation according to Grade Increment Date 
                    if (txtGrdIncrmtDate.MaskCompleted)
                    {
                       DateTime gradedate = Date.ToDotNet(txtGrdIncrmtDate.Text);
                       int numYears = GetDifferenceInYears(gradedate);
                    #endregion

                       if (numYears > maxGrade)
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

        private void tbSalaryAndGrade_Click(object sender, EventArgs e)
        {

        }

        private void tbAdvance_Click(object sender, EventArgs e)
        {

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
        private void WriteLoanRow(int curRow, int LoanID, string Name, string Type, decimal Principal, decimal MthInstall, decimal MthInterest, decimal PerMthDecAmt, int TotalMth, int RemMth, string SDate, string EDate, decimal Premium, decimal initialInstallment, int DLID = 0)
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
            grdLoan[curRow, 14] = new SourceGrid.Cells.Cell(DLID);

        }
        private void FillLoan()
        {
            GridType = "LOAN";
            grdLoan.Rows.Clear();
            grdLoan.Redim(1, 15);
            evtRemoveLoan.Click += new EventHandler(Remove_Loan_Row_Click);
            WriteHeader();

        }
        private void FillLoan(int DocID)
        {
            FillLoan();
            DataTable dt = Doctor.GetDoctorLoan(DocID);
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i - 1];
                    WriteLoanRow(i, Convert.ToInt32(dr["LoanID"].ToString()), dr["LoanName"].ToString(), dr["LoanType"].ToString(), Convert.ToDecimal(dr["LoanPrincipal"].ToString()), Convert.ToDecimal(dr["LoanMthInstallment"].ToString()), Convert.ToDecimal(dr["LoanMthInterest"].ToString()), Convert.ToDecimal(dr["LoanMthDecreaseAmt"].ToString()), Convert.ToInt32(dr["LoanTotalMth"].ToString()), Convert.ToInt32(dr["LoanRemainingMth"].ToString()), Date.ToSystem(Convert.ToDateTime(dr["LoanStartDate"].ToString())), Date.ToSystem(Convert.ToDateTime(dr["LoanEndDate"].ToString())), Convert.ToDecimal(dr["LoanMthPremium"].ToString()), Convert.ToDecimal(dr["InitialInstallment"].ToString()), Convert.ToInt32(dr["ELID"]));
                }
            }
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
                if (txtLoanEndDate.MaskCompleted)// && m_mode == EntryMode.NEW)
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

        private void txtLoanStartDate_Leave(object sender, EventArgs e)
        {
            calculateLoanEndDate();
            calculateRemainingLoanMonth();
        }

        private void txtLoanEndDate_Leave(object sender, EventArgs e)
        {
            calculateRemainingLoanMonth();
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
        private void WriteAdvanceRow(int curRow, string title, decimal totalAmt, decimal installment, string takenDate, string returnDate, decimal remainingAmt)
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

        private void FillAdvance()
        {
            GridType = "ADVANCE";
            grdAdvance.Rows.Clear();
            grdAdvance.Redim(1, 7);
            evtRemoveAdvance.Click += new EventHandler(Remove_Advance_Row_Click);
            WriteHeader();

        }
        private void FillAdvance(int DocID)
        {
            FillAdvance();
            DataTable dt = doctor.GetDoctorAdvance(DocID);
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i - 1];
                    WriteAdvanceRow(i, dr["AdvTitle"].ToString(), Convert.ToDecimal(dr["TotalAmt"].ToString()), Convert.ToDecimal(dr["Installment"].ToString()), Date.ToSystem(Convert.ToDateTime(dr["TakenDate"].ToString())), Date.ToSystem(Convert.ToDateTime(dr["ReturnDate"].ToString())), Convert.ToDecimal(dr["RemainingAmt"].ToString()));
                }
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

        private void cmbLoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbLoan.SelectedIndex != -1)
                {
                    liLoan = (ListItem)cmbLoan.SelectedItem;
                    DataTable dt = Hrm.GetLoanByID(liLoan.ID);
                    lblInstallment.Text = dt.Rows[0]["LoanType"].ToString();
                    if (dt.Rows[0]["LoanType"].ToString() == "Fix Installment")
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
        private void txtMthInterest_Leave(object sender, EventArgs e)
        {
            DisplayMonthlyPremium();
        }

        private void txtMthInstallment_Leave(object sender, EventArgs e)
        {
            DisplayMonthlyPremium();
        }

        private void txtMthDecreaseAmt_Leave(object sender, EventArgs e)
        {
            DisplayMonthlyPremium();
        }

        private void txtGrdIncrmtDate_Leave(object sender, EventArgs e)
        {

            cmbLevel_SelectedIndexChanged(null, e);
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

        private void rbtnPensionYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPensionYes.Checked == true)
            {
                txtPensionNumber.Enabled = true;
            }
            else
            {
                txtPensionNumber.Enabled = false;
            }
        }

        private void rbtnInsYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnInsYes.Checked == true)
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

        private void rbtnQuarterYes_CheckedChanged(object sender, EventArgs e)
        {
              txtElectricity.Enabled = txtAccommodation.Enabled = rbtnQuarterYes.Checked;
        }

        

    }
}
