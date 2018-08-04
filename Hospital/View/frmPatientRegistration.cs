  using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic.HOS;
using DateManager;
using BusinessLogic;
using Common;
using System.Drawing.Printing;

namespace Hospital.View
{
    public partial class frmPatientRegistration : Form, IfrmDateConverter, IListOfPatient
    {
        public frmPatientRegistration()
        {
            InitializeComponent();
        }
        ListItem liNationality = new ListItem();
        ListItem liDoctor = new ListItem();
        private string DateStatus = "";
        private EntryMode m_mode = EntryMode.NORMAL;
        private bool IsFieldChanged = false;
        private DataTable dTable;
        private DataRow[] drFound;
        private string FilterString = "";
        Patient patient = new Patient();
        private string GridType = "";
        TextBox txtpatientID = new TextBox();

        public DataTable FromOpeningBalance = new DataTable();
        public DataTable FromPreYearBalance = new DataTable();


        private SourceGrid.Cells.Controllers.CustomEvents dblClick;
        private SourceGrid.Cells.Controllers.CustomEvents gridKeyDown;

        private string PatientLedger = "";

        private void frmPatientRegistration_Load(object sender, EventArgs e)
        {
            Patient patient = new Patient();
            dTable = patient.GetPAtientDetails("Order by RegistrationNo");
            drFound = dTable.Select(FilterString);

            dblClick = new SourceGrid.Cells.Controllers.CustomEvents();
            dblClick.DoubleClick += new EventHandler(grdpatient_DoubleClick);

            gridKeyDown = new SourceGrid.Cells.Controllers.CustomEvents();
           // gridKeyDown.KeyDown += new KeyEventHandler(Handle_KeyDown);
            grdPatient.Controller.AddController(gridKeyDown);

            AutoGenerateRegistrationNo();
            LoadComboboxItems(cmbNationality);
            LoadComboboxItems(cboDoctor);
            txtBirthDate.Text = Date.ToSystem(Date.GetServerDate());
            txtRegDate.Text = Date.ToSystem(Date.GetServerDate());

            FillPatient();
            ChangeState(EntryMode.NEW);
            cmbPatType.SelectedIndex = 0;        
        }


        private void grdpatient_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                //Get the Selected Row
                int CurRow = grdPatient.Selection.GetSelectionRegion().GetRowsIndex()[0];
                int D_ID = Convert.ToInt32(grdPatient[CurRow, 0].Value.ToString());
                LoadPatientDetails(D_ID);
                SetPatientDetail(D_ID);       

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public void AddPatient(string RegistrationNo, string Name, string Address, string Age, string Telephone, string Gender, int PatientID, int LedgerID)
        {
            try
            {
                ClearForm();
                LoadPatientDetails(PatientID);
                ChangeState(EntryMode.NORMAL);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //  Global.Msg("Invalid selection");
            }
           
        
        }
        private void LoadPatientDetails(int PatID)
        {
          DataTable dtfillpatientdetails;
          

          Patient patient = new Patient();
          dtfillpatientdetails = patient.FillPatientDetails(PatID);
          DataRow drpatinetsdetails = dtfillpatientdetails.Rows[0];

          
            txtpatientID.Text = drpatinetsdetails["PatientID"].ToString();
            txtFirstName.Text = drpatinetsdetails["FirstName"].ToString();
            txtMiddleName.Text = drpatinetsdetails["MiddleName"].ToString();
            txtLastName.Text = drpatinetsdetails["LastName"].ToString();
            txtRegNo.Text = drpatinetsdetails["RegistrationNo"].ToString();
            PatientLedger = txtRegNo.Text + "-" + txtFirstName.Text + txtMiddleName.Text + txtLastName.Text;
           
            if (drpatinetsdetails["BirthDate"].ToString() != "")
            {
               DateTime bday = Convert.ToDateTime(drpatinetsdetails["BirthDate"].ToString());
               txtBirthDate.Text = Date.ToSystem(bday);
            }

            if (drpatinetsdetails["RegistrationDate"].ToString() != "")
            {
                DateTime regdate = Convert.ToDateTime(drpatinetsdetails["RegistrationDate"].ToString());
                txtRegDate.Text = Date.ToSystem(regdate);
            }
            txtAge.Text = drpatinetsdetails["Age"].ToString();
          
            txtGuardianName.Text = drpatinetsdetails["GuardianName"].ToString();
            cmbPatType.Text = drpatinetsdetails["PatientType"].ToString();
            cmbNationality.Text = drpatinetsdetails["NationalityName"].ToString();

            cboDoctor.Text = drpatinetsdetails["DoctorName"].ToString();

            if (drpatinetsdetails["Gender"].ToString() == "1")
                btnMale.Checked = true;
            else if (drpatinetsdetails["Gender"].ToString() == "2")
                btnFemale.Checked = true;
            else
                btnOther.Checked = true;

            if (drpatinetsdetails["IsSingle"].ToString() == "True")
                btnSingle.Checked = true;
            else if (drpatinetsdetails["IsSingle"].ToString() == "False")
               btnMarried.Checked = true;


             txtAddress.Text = drpatinetsdetails["Address"].ToString();
             txtCity.Text = drpatinetsdetails["City"].ToString();       
             txtFatherName.Text = drpatinetsdetails["FatherName"] != DBNull.Value ? drpatinetsdetails["FatherName"].ToString() : "";
            
             cmbReligion.Text = drpatinetsdetails["Religion"].ToString();

             txtPhone1.Text = drpatinetsdetails["Phone1"].ToString();
             txtPhone2.Text = drpatinetsdetails["Phone2"].ToString();
             txtEmail.Text = drpatinetsdetails["Email"].ToString();
             txtReason.Text = drpatinetsdetails["Reason"].ToString();
      
        
              ChangeState(EntryMode.NORMAL);
              for (int i = 0; i < 1; i++)
                  TCDoctor.TabPages[i].Enabled = false;
              txtFirstName.Enabled = false;
              txtMiddleName.Enabled = false;
              txtLastName.Enabled = false;
              btnSave.Enabled = false;


           //Show ledger name of the student
              int ledgerID = Convert.ToInt32(drpatinetsdetails.IsNull("LedgerID") ? "0" : drpatinetsdetails["LedgerID"].ToString());
                if(ledgerID > 0)
                {
                    string ledgerName = Ledger.GetLedgerNameFromID(ledgerID);
                    txtLedgerName.Text = ledgerName; 
                }
                else
                {
                    txtLedgerName.Text = "No Ledger";
                }
        }

        private void FillPatient()
        {
            GridType = "PATIENT";
            grdPatient.Rows.Clear();
            grdPatient.Redim(drFound.Count() + 1, 3);
            WriteHeader();
            for (int i = 1; i <= drFound.Count(); i++)
            {
                DataRow dr = drFound[i - 1];
                grdPatient[i, 0] = new SourceGrid.Cells.Cell(dr["PatientID"].ToString());
                grdPatient[i, 1] = new SourceGrid.Cells.Cell(dr["RegistrationNo"].ToString());
                grdPatient[i, 1].AddController(dblClick);

                grdPatient[i, 2] = new SourceGrid.Cells.Cell(dr["PatientName"].ToString());
                grdPatient[i, 2].AddController(dblClick);
                grdPatient[i, 2].AddController(gridKeyDown);
            }


        }
        private void WriteHeader()
        {
            if (GridType == "PATIENT")
            {

                grdPatient[0, 0] = new SourceGrid.Cells.ColumnHeader("ID");
                grdPatient[0, 1] = new SourceGrid.Cells.ColumnHeader("RegNo");
                grdPatient[0, 2] = new SourceGrid.Cells.ColumnHeader("Patient Name");

                grdPatient[0, 0].Column.Width = 1;
                grdPatient[0, 1].Column.Width = 100;
                grdPatient[0, 2].Column.Width = 210;

                grdPatient[0, 0].Column.Visible = false;

            }
        }

        private void tpPersonalinfo_Click(object sender, EventArgs e)
        {
            
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
                else if(comboboxitems ==cboDoctor )
                {
                    DataTable dtdoctorName = Doctor.GetDoctorName();
                    foreach (DataRow drDoctor in dtdoctorName.Rows)
                    {
                        comboboxitems.Items.Add(new ListItem((int)drDoctor["DoctorID"], drDoctor["DoctorName"].ToString()));

                    }
                    comboboxitems.SelectedIndex = 0;
                    comboboxitems.DisplayMember = "value";
                    comboboxitems.ValueMember = "id";
                }
             

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
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
        public void ClearForm()
        {
            try
            {
                txtLedgerName.Clear();
                ChangeState(EntryMode.NEW);

                for (int i = 0; i < 1; i++)
                    TCDoctor.TabPages[i].Enabled = true;
                txtFirstName.Enabled = true;
                txtMiddleName.Enabled = true;
                txtLastName.Enabled = true;
                txtFirstName.Clear();
                txtMiddleName.Clear();
                txtLastName.Clear();
                txtRegNo.Clear();
                txtBirthDate.Text = Date.ToSystem(Date.GetServerDate());
                txtRegDate.Text = Date.ToSystem(Date.GetServerDate());
                txtAge.Clear();

                if (cmbPatType.Items.Count > 0)
                    cmbPatType.SelectedIndex = 0;

                if (cmbNationality.Items.Count > 0)
                    cmbNationality.SelectedIndex = 0;
                if (cmbReligion.Items.Count > 0)
                    cmbReligion.SelectedIndex = 0;

                txtPhone1.Clear();
                txtPhone2.Clear();
                txtEmail.Clear();
                txtAddress.Clear();

                txtFatherName.Clear();
                txtGuardianName.Clear();
                rbtnSingle.Checked = true;

                txtReason.Clear();
                btnMale.Checked = true;
                AutoGenerateRegistrationNo();


            }

            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                txtLedgerName.Clear();
                ChangeState(EntryMode.NEW);

                for (int i = 0; i < 1; i++)
                    TCDoctor.TabPages[i].Enabled = true;
                txtFirstName.Enabled = true;
                txtMiddleName.Enabled = true;
                txtLastName.Enabled = true;
                txtFirstName.Clear();
                txtMiddleName.Clear();
                txtLastName.Clear();
                txtRegNo.Clear();
                txtBirthDate.Text = Date.ToSystem(Date.GetServerDate());
                txtRegDate.Text = Date.ToSystem(Date.GetServerDate());
                txtAge.Clear();

                if (cmbPatType.Items.Count > 0)
                    cmbPatType.SelectedIndex = 0;
            
                if (cmbNationality.Items.Count > 0)
                    cmbNationality.SelectedIndex = 0;
                if (cmbReligion.Items.Count > 0)
                    cmbReligion.SelectedIndex = 0;
                
                txtPhone1.Clear();
                txtPhone2.Clear();
                txtEmail.Clear();
                txtAddress.Clear();
              
                txtFatherName.Clear();
                txtGuardianName.Clear();            
                rbtnSingle.Checked = true;

                txtReason.Clear();
                btnMale.Checked = true;
                AutoGenerateRegistrationNo();
               

            }
               
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private bool CheckEmptyTxt(TextBox txt, string errorMsg)
        {
            if (txt.Text == "")
            {
                MessageBox.Show(errorMsg);
                txt.Focus();
                return true;
            }
            return false;
        }
        public void AutoGenerateRegistrationNo()
        {
            DataTable dt = patient.GetMaxRegistrationNo();
            if (dt.Rows.Count > 0)
            {
                string registrationno = dt.Rows[0][0].ToString();
                if (registrationno == "")
                {
                    txtRegNo.Text = "R1";
                }
                else
                {
                    int registrationnumber = Convert.ToInt32(registrationno.Remove(0, 1)) + (int)1;
                    txtRegNo.Text = "R" + (registrationnumber).ToString();                   
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

             switch (m_mode)
            {
                #region NEW
                case EntryMode.NEW:// Insert doctor Information
                    try
                    {
                        FromOpeningBalance.Rows.Clear();
                        FromPreYearBalance.Rows.Clear();

                        if (CheckEmptyTxt(txtFirstName, "Please provide the first name"))
                            return;
                        if (CheckEmptyTxt(txtLastName, "Please provide the last name"))
                            return;

                        PatientDetails pat = new PatientDetails();
                        pat.FirstName = txtFirstName.Text;
                        pat.MiddleName = txtMiddleName.Text;
                        pat.LastName = txtLastName.Text;
                        pat.RegistrationNo = txtRegNo.Text.Trim();
                        pat.RegistartionDate = Date.ToDotNet(txtRegDate.Text);
                        pat.BirthDate = Date.ToDotNet(txtBirthDate.Text);
                        pat.Age = Convert.ToInt32(txtAge.Text);
                        pat.Gender = btnMale.Checked ? 1 : btnFemale.Checked ? 2 : 0;
                        pat.IsSingle = btnSingle.Checked ? 1 : 0;
                        pat.Address = txtAddress.Text;
                        pat.City = txtCity.Text;
                        liNationality = (ListItem)cmbNationality.SelectedItem;
                        int nationalityid = liNationality.ID;
                        pat.NationalityID = nationalityid;

                        liDoctor = (ListItem)cboDoctor.SelectedItem;
                        int doctorid = liDoctor.ID;
                        pat.DoctorID = doctorid;

                        pat.FatherName = txtFatherName.Text;
                        pat.GuardianName= txtGuardianName.Text;
                        pat.Religion = cmbReligion.Text;
                        pat.PatientType = cmbPatType.Text;
                        pat.Phone1 = txtPhone1.Text;
                        pat.Phone2 = txtPhone2.Text;
                        pat.Email = txtEmail.Text;
                        pat.Reason = txtReason.Text;

                        TextBox txtLedgerCode = new TextBox();
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


                        int returnStudentID = 0;
                        string createResult = patient.CreatePatient(pat, out returnStudentID);
                        //if (insertResult == "SUCCESS")
                        //{
                        //    AutoGenerateRegistrationNo();
                        //    btnNew_Click(sender, e);
                        //    frmPatientRegistration_Load(sender, e);
                        //    ChangeState(EntryMode.NEW);
                        
                        //}

                        if (createResult != "Failure")
                        {
                            //Creation Of the Ledger of Respective ledgername
                            ILedger acLedger = new Ledger();
                            DateTime currentdate = Date.GetServerDate();
                            int ledgerID = 0;
                            bool result = acLedger.Create(txtLedgerCode.Text.Trim(), txtRegNo.Text.Trim() + "-" + txtFirstName.Text.Trim() + txtMiddleName.Text.Trim() + txtLastName.Text.Trim(), 4010, 0.00, "DEBIT", 1, 0.00, currentdate, "", "", "", "", "", "", "", "", "", "", 0.0, false, 0, "", "New Ledger Name=" + txtRegNo + "-" + txtFirstName.Text + txtMiddleName.Text.Trim() + txtLastName.Text.Trim() + "Parent Group" + "Student", true, out ledgerID);
                            //Increment the ledger code (automated number)
                            Global.m_db.InsertUpdateQry("update acc.tblLedgerCodeFormat set Parameter=Parameter+1 where TYPE='(AutoNumber)' ");

                            //Get current LedgerID and save it to tblOpeningBalance
                            //int LdrID = Ledger.GetLedgerIdFromName(txtStudentCode.Text.Trim() + "-" + txtFirstName.Text.Trim() + txtMiddleName.Text.Trim() + txtLastName.Text.Trim(), LangMgr.DefaultLanguage);
                            // int LdrID = Ledger.GetLedgerIdFromName(ledgername, LangMgr.DefaultLanguage);
                            OpeningBalance.InsertAccountOpeningBalance(ledgerID, FromOpeningBalance);
                            OpeningBalance.InsertAccountPreYearBalance(ledgerID, FromPreYearBalance);

                            //Add ledgerID to Sch.tblStudentMaster
                            if ((Global.m_db.InsertUpdateQry("update HOS.tblPatient set LedgerID = '" + ledgerID + "' where PatientID = '" + returnStudentID + "'")) < 1)
                            {
                                Global.Msg("There was a problem while creating a relation between the Patient record and ledger");
                            }

                            AutoGenerateRegistrationNo();
                            btnNew_Click(sender, e);
                            frmPatientRegistration_Load(sender, e);
                            ChangeState(EntryMode.NEW);
                          
                        }

                     }
                          catch(Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion
                     
                #region EDIT
                case EntryMode.EDIT:
                       try
                    {
                        if (CheckEmptyTxt(txtFirstName, "Please provide the first name"))
                            return;
                        if (CheckEmptyTxt(txtLastName, "Please provide the last name"))
                            return;

                        PatientDetails pat = new PatientDetails();
                        pat.PatientID = Convert.ToInt32(txtpatientID.Text);
                        pat.FirstName = txtFirstName.Text;
                        pat.MiddleName = txtMiddleName.Text;
                        pat.LastName = txtLastName.Text;
                        pat.RegistrationNo = txtRegNo.Text.Trim();
                        pat.RegistartionDate = Date.ToDotNet(txtRegDate.Text);
                        pat.BirthDate = Date.ToDotNet(txtBirthDate.Text);
                        pat.Age = Convert.ToInt32(txtAge.Text);
                        pat.Gender = btnMale.Checked ? 1 : btnFemale.Checked ? 2 : 0;
                        pat.IsSingle = btnSingle.Checked ? 1 : 0;
                        pat.Address = txtAddress.Text;
                        pat.City = txtCity.Text;
                        liNationality = (ListItem)cmbNationality.SelectedItem;
                        int nationalityid = liNationality.ID;
                        pat.NationalityID = nationalityid;

                        liDoctor = (ListItem)cboDoctor.SelectedItem;
                        int doctorid = liDoctor.ID;
                        pat.DoctorID = doctorid;

                        pat.FatherName = txtFatherName.Text;
                        pat.GuardianName= txtGuardianName.Text;
                        pat.Religion = cmbReligion.Text;
                        pat.PatientType = cmbPatType.Text;
                        pat.Phone1 = txtPhone1.Text;
                        pat.Phone2 = txtPhone2.Text;
                        pat.Email = txtEmail.Text;
                        pat.Reason = txtReason.Text;
                        string updateResult = patient.UpdatePatient(pat);
                        if (updateResult == "SUCCESS")
                        {

                            int ledgerid = Patient.GetPatientLedgerId(pat.PatientID); //Ledger.GetLedgerIdFromName(studentLedger, Lang.English);
                            string lname = txtRegNo.Text.Trim() + "-" + txtFirstName.Text.Trim() + txtMiddleName.Text.Trim() + txtLastName.Text.Trim();
                            string SQL = "Update Acc.tblLedger set EngName='" + lname + "',NepName='" + lname + "' where ledgerid='" + ledgerid + "'";
                            Global.m_db.InsertUpdateQry(SQL);
                            AutoGenerateRegistrationNo();
                            btnNew_Click(sender, e);
                            frmPatientRegistration_Load(sender, e);
                        }
                     }
                          catch(Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                   

                 break;
                #endregion
            }
        }

        private void btnStartDate_Click(object sender, EventArgs e)
        {
            DateStatus = "STARTDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtRegDate.Text));
            frm.ShowDialog();
        }

        private void btnBirthDate_Click(object sender, EventArgs e)
        {
            DateStatus = "BIRTHDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtBirthDate.Text));
            frm.ShowDialog();
        }



        public void DateConvert(DateTime ReturnDotNetDate)
        {
            if (DateStatus == "BIRTHDATE")
                txtBirthDate.Text = Date.ToSystem(ReturnDotNetDate);
        }

        private void tbDiseases_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeState(EntryMode.EDIT);
                for (int i = 0; i < 1; i++)
                    TCDoctor.TabPages[i].Enabled = true;
                txtFirstName.Enabled = true;
                txtMiddleName.Enabled = true;
                txtLastName.Enabled = true;
                btnSave.Enabled = true;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

       
        private void btnSenseOrgans_Click(object sender, EventArgs e)
        {
         
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Patient patient = new Patient();
                int patID = Convert.ToInt32(txtpatientID.Text.Trim());
               
                if (Global.MsgQuest("Do you want to delete the doctor " + txtFirstName.Text + " " + txtLastName.Text + " with registration number: " + txtRegNo.Text.Trim()) == DialogResult.Yes)
                {
                    if ((patient.DeletePatient(patID)))
                    {
                        Global.Msg("The patient has been deleted successfully.");
                        btnNew_Click(sender, e);
                    }
                    else
                    {
                        Global.Msg("There has been a problem while deleting the patient.");
                        return;
                    }
                }
              
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            frmPatientRegistration_Load(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            switch (m_mode)
            {

                #region NEW
                case EntryMode.NEW:// Insert patient Information
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
                case EntryMode.EDIT: //Update patient Information
                    try
                    {
                        LoadPatientDetails(Convert.ToInt32(txtpatientID.Text.Trim()));
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                    break;
                #endregion
            }
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
            string str = " where RegistrationNo LIKE '" + txtcode.Text + "%' and FirstName Like '%" + txtname.Text + "%' and MiddleName Like '%" + txtMName.Text + "%' and LastName Like '%" + txtLName.Text + "%' ";
            try
            {
                dTable = patient.GetPAtientDetails(str);
                drFound = dTable.Select(this.FilterString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            FillPatient();
        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {
            Filter();

        }

        private void txtGuardianName_TextChanged(object sender, EventArgs e)
        {

        }
       
        private void txtBirthDate_TextChanged(object sender, EventArgs e)
        {
            
            try
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                DateTime StartDate = Date.ToDotNet(txtBirthDate.Text);
                //TimeSpan span = Date.GetServerDate() - StartDate;
                DateTime today = Date.GetServerDate();
                TimeSpan difference = today.Subtract(StartDate);
                int result = Convert.ToInt32(difference.TotalDays / 365.25);
                
                if (result == 1)
                    txtAge.Text = result.ToString();
                else if (result > 1)
                    txtAge.Text = result.ToString();
                else
                     
                    txtAge.Text = "0";
            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }

            //if (txtBirthDate.MaskCompleted)
            //{
            //    DateTime zeroTime = new DateTime(1, 1, 1);

            //    DateTime dt = Date.ToDotNet(txtBirthDate.Text);
            //    int result = DateTime.Compare(dt, DateTime.Today);
            //    if (result >= 0)
            //    {
            //        TimeSpan span = dt - DateTime.Today;
            //        int remainYrs = (zeroTime + span).Year - 1;

            //        txtAge.Text = remainYrs.ToString() + (remainYrs == 1 ? " Year" : " Years") + " remaining.";
            //    }
            //    else
            //    {
            //        txtAge.Text = "Age limit crossed.";
            //    }

            //}



        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FrmListOfPatientInfo info = new FrmListOfPatientInfo();
            info.ShowDialog();
        }

     

        private void btnPrint_Click(object sender, EventArgs e)
        {

            PrintHospitalReceipt();
        }
        PrintDocument pdoc = null;
        DataTable dtsalesInvoice;
        int docHeight = 0;
        int docwidth = 285;
        private int prntDirectForPOS = 0;
        public void PrintHospitalReceipt()
        {
            try
            {
                docHeight = 340 + grdPatient.Rows.Count * 20;
                //if (Global.Default_Sales_Report_Type == "Hospital")
                //{
                    PrintDialog diag = new PrintDialog();
                    PrinterSettings ps = new PrinterSettings();
                    pdoc = new PrintDocument();

                    Font font = new Font("Courier New", 7);

                    System.Drawing.Printing.PaperSize psize = new System.Drawing.Printing.PaperSize("Custom", docwidth, docHeight);
                    diag.Document = pdoc;
                    diag.Document.DefaultPageSettings.PaperSize = psize;

                    pdoc.PrintPage += new PrintPageEventHandler(p_doc_PrintHospital);
                    if (prntDirectForPOS == 1)
                    {
                        pdoc.Print();
                    }
                    else
                    {
                        DialogResult res = diag.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            PrintPreviewDialog pp = new PrintPreviewDialog();
                            pp.Document = pdoc;
                            DialogResult result = pp.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                pdoc.Print();
                            }
                        }
                    }
                //}
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        string m_PatientName = "";
        string m_Age = "";
        string m_sex = "";
        string m_Telephone = "";
        string m_Mobile = "";
        string m_Date = "";
        string m_Address = "";
        string m_City = "";
        int m_PatientID = 0;
        int m_ledgerId = 0;
        string m_DoctorName = "";
     

        void p_doc_PrintHospital(object sender, PrintPageEventArgs e)
        {
            
            PrintPOSForHospital(e);



        }

        public void SetPatientDetail(int PatientID)
        {
            DataTable dtStudent = Patient.GetPatientData(PatientID);
            if (dtStudent.Rows.Count > 0)
            {
                DataRow dr = dtStudent.Rows[0];

                m_PatientName = dr["PatientName"].ToString();
                m_Age = dr["Age"].ToString();
                int gender = Convert.ToInt32(dr["Sex"].ToString());
                if (gender == 1)
                {
                    m_sex = "Male";
                }
                else if (gender == 2)
                {
                    m_sex = "Female";
                }
                else
                {
                    m_sex = "Other";
                }
                m_Telephone = dr["Telephone"].ToString();
                m_Mobile = dr["Mobile"].ToString();
                m_Address = dr["Address"].ToString();
                m_PatientID = Convert.ToInt32(dr["PatientID"]);
                m_DoctorName = dr["DoctorName"].ToString();
                m_Date = dr["Date"].ToString();

            }

            else
            {
                //ClearPatientDetail();
            }
        }
        public void PrintPOSForHospital(PrintPageEventArgs e)
        {
            // Create rectangle for drawing.
            float x = 0.0F;
            float y = 10.0F;
            float width = 285.0F;
            float height = 20.0F;
            Font font3 = new Font("Courier New", 9, FontStyle.Bold);
            Font font2 = new Font("Courier New", 8, FontStyle.Bold);



            // formatting the string for central alignment
            StringFormat stringCentralFormat = new StringFormat();
            stringCentralFormat.Alignment = StringAlignment.Center;
            stringCentralFormat.LineAlignment = StringAlignment.Center;

            //formating string for right alignment
            StringFormat stringRightFormat = new StringFormat();
            stringRightFormat.Alignment = StringAlignment.Far;
            stringRightFormat.LineAlignment = StringAlignment.Far;

            DataTable dtCompany = CompanyDetails.GetCompanyInfo();
            Graphics g = e.Graphics;
            Font font = new Font("Courier New", 8);
            float fontHeight = font.GetHeight();
            int startX = 15;
            int startY = 20;
            int offset = 20;

            // write comapny name
            g.DrawString(dtCompany.Rows[0]["Name"].ToString(), new Font("Courier New", 8), new SolidBrush(Color.Black), new RectangleF(x, y, width, height), stringCentralFormat);
            //offset += 10;

            // write company address
            g.DrawString(dtCompany.Rows[0]["Address1"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width, height), stringCentralFormat);
            offset += 20;

            // write company PAN
            g.DrawString("PAN: " + dtCompany.Rows[0]["PAN"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width, height), stringCentralFormat);
            offset += 20;

            // write company phone number
            g.DrawString(dtCompany.Rows[0]["Telephone"].ToString(), font, new SolidBrush(Color.Black), new RectangleF(x, y + offset, width, height), stringCentralFormat);
            offset += 20;

            // to write bill number and date
            g.DrawString("Date:" + m_Date , font, new SolidBrush(Color.Black), 15, startY + offset);
            //offset += 10;
           

         //g.DrawString("Date:" + txtDate.Text, font, new SolidBrush(Color.Black), startX + 125, startY + offset);
            offset += 20;

          //  g.DrawString(m_PatientID > 0 ? "Name : " + m_PatientName : cboCashParty.Text, font3, new SolidBrush(Color.Black), 25, 140);
            

            g.DrawString("Name : " + m_PatientName, font3, new SolidBrush(Color.Black), 25, 140);

            g.DrawString("Age : " + m_Age, font3, new SolidBrush(Color.Black), 25, 155);

            g.DrawString("Address : " + m_Address, font3, new SolidBrush(Color.Black), 25, 170);


            g.DrawString("Sex : " + m_sex, font3, new SolidBrush(Color.Black), 25, 185);

            g.DrawString("Telephone : " + m_Telephone, font3, new SolidBrush(Color.Black), 25, 200);

            g.DrawString("Mobile : " + m_Mobile, font3, new SolidBrush(Color.Black), 25, 215);

            g.DrawString("Dr.Counsultant : " + "Dr." + m_DoctorName, font2, new SolidBrush(Color.Black), 25, 230);



        }







    }
}
