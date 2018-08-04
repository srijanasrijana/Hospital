using BusinessLogic;
using BusinessLogic.HOS;
using BusinessLogic.HRM;
using Common;
using DateManager;
using Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hospital.View
{
    public partial class FrmRecipt : Form, IfrmDateConverter,IDoctorList
    {
        public FrmRecipt()
        {
            InitializeComponent();
        }
        private string DateStatus = "";
        private SourceGrid.Cells.Views.Cell RowView;
        bool hasChanged = false;
        DevAge.Windows.Forms.DevAgeTextBox ctx;
        private bool IsFieldChanged = false;
        private bool isNew;
        private EntryMode m_mode = EntryMode.NORMAL;
        ListItem LiPatientID = new ListItem();
        private int m_PartTimeSalaryMasterID = 0;   

       
        public enum EntryMode { NORMAL, NEW, EDIT }; 

        SourceGrid.Cells.Controllers.CustomEvents evtDelete = new SourceGrid.Cells.Controllers.CustomEvents();
        SourceGrid.Cells.Controllers.CustomEvents AmountFocusLost = new SourceGrid.Cells.Controllers.CustomEvents();
        private void FrmPatientsDoctor_Load(object sender, EventArgs e)
        {
            LoadComboBox(cboName,CmbType.Patient);
            txtRegDate.Text = Date.ToSystem(Date.GetServerDate());
           // txtBirthDate.Text = Date.ToSystem(Date.GetServerDate());

            evtDelete.Click += new EventHandler(Delete_Row_Click);
            AmountFocusLost.FocusLeft += new EventHandler(CRevtAmountFocusLost);

           
            AddGridHeader();
            AddRow(1);
           
          
        }
        private void CRevtAmountFocusLost(object sender, EventArgs e)
        {
            try
            {
                decimal tAmt = 0;
                int CurrRow = 0;
                for (int i = 1; i < grdSalarySheet.RowsCount - 1; i++)
                {
                    CurrRow = i;

                    tAmt += Convert.ToDecimal(grdSalarySheet[CurrRow, 4].Value);
                }
                lblNetAmout.Text = tAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
            }

            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }
        private void Delete_Row_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to delete the row?", "Question", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                SourceGrid.CellContext ctx = (SourceGrid.CellContext)sender;
                //Do not delete if its the last Row because it contains (NEW)
                if (ctx.Position.Row <= grdSalarySheet.RowsCount - 2)
                    grdSalarySheet.Rows.Remove(ctx.Position.Row);

                CalculateTotal();
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        private void CalculateTotal()
        {
            decimal tAmt = 0;
            int CurrRow = 0;
            for (int i = 1; i < grdSalarySheet.RowsCount - 1; i++)
            {
                CurrRow = i;
                
                tAmt += Convert.ToDecimal(grdSalarySheet[CurrRow, 4].Value);              
            }          
            lblNetAmout.Text = tAmt.ToString(Misc.FormatNumber(false, Global.DecimalPlaces));          
           
        }
        private void AddGridHeader()
        {
            grdSalarySheet.Rows.Clear();
            grdSalarySheet.Redim(1, 8);
            //
            grdSalarySheet[0, 0] = new SourceGrid.Cells.ColumnHeader("Del");
            grdSalarySheet[0, 0].Column.Width = 30;
            grdSalarySheet.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 1] = new SourceGrid.Cells.ColumnHeader("S.N.");
            grdSalarySheet.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
        
            grdSalarySheet[0, 2] = new SourceGrid.Cells.ColumnHeader("Doctor Name");
            grdSalarySheet[0, 2].Column.Width = 190;
            grdSalarySheet.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
           
            grdSalarySheet[0, 3] = new SourceGrid.Cells.ColumnHeader("Specilization");
            grdSalarySheet[0, 3].Column.Width = 100;
            grdSalarySheet.Columns[3].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdSalarySheet[0, 4] = new SourceGrid.Cells.ColumnHeader("Amount");
            grdSalarySheet[0, 4].Column.Width = 100;
            grdSalarySheet.Columns[4].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;

            grdSalarySheet[0, 5] = new SourceGrid.Cells.ColumnHeader("Remarks");
            grdSalarySheet[0, 5].Column.Width = 300;
            grdSalarySheet.Columns[5].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;

            grdSalarySheet[0, 6] = new SourceGrid.Cells.ColumnHeader("DoctorID");
            grdSalarySheet[0, 6].Column.Visible = false;

            grdSalarySheet.AutoStretchColumnsToFitWidth = true;
           
        }
        private void Employee_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;

                frmDoctorList fle = new frmDoctorList(this);
                fle.ShowDialog();
                SendKeys.Send("{TAB}{TAB}");
            }

        }

        private void Employee_Leave(object sender, EventArgs e)
        {
            hasChanged = false;
            int RowCount = grdSalarySheet.RowsCount;
            string LastServicesCell = (string)grdSalarySheet[RowCount - 1, 2].Value;

            if (LastServicesCell != "(NEW)")
            {
                AddRow(RowCount);
            }
        }
        private void AddRow(int RowCount)
        {
            try
            {
                int i = RowCount;
                grdSalarySheet.Redim(RowCount + 1, grdSalarySheet.ColumnsCount);

                SourceGrid.Cells.Button btnDelete = new SourceGrid.Cells.Button("");
                btnDelete.Image = global::Hospital.Properties.Resources.gnome_window_close;
                grdSalarySheet[i, 0] = btnDelete;
                grdSalarySheet[i, 0].AddController(evtDelete);

                grdSalarySheet[i, 1] = new SourceGrid.Cells.Cell(i.ToString());

                SourceGrid.Cells.Editors.TextBox txtName = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtName.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey;
                txtName.Control.GotFocus += new EventHandler(Employee_Focused);
                txtName.Control.LostFocus += new EventHandler(Employee_Leave);
                grdSalarySheet[i, 2] = new SourceGrid.Cells.Cell("", txtName);
                grdSalarySheet[i, 2].Value = "(NEW)";

                grdSalarySheet[i, 3] = new SourceGrid.Cells.Cell("");
                //Amount
                SourceGrid.Cells.Editors.TextBox txtAmount = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtAmount.EditableMode = SourceGrid.EditableMode.Focus;
                grdSalarySheet[i, 4] = new SourceGrid.Cells.Cell("", txtAmount);
                txtAmount.Control.TextChanged += new EventHandler(Text_Change);               
                grdSalarySheet[i, 4].AddController(AmountFocusLost);
                grdSalarySheet[i,4].Value = "0";
            
                SourceGrid.Cells.Editors.TextBox txtRemarks = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                txtRemarks.EditableMode = SourceGrid.EditableMode.Focus;
                txtRemarks.Control.TextChanged += new EventHandler(Text_Change);
                grdSalarySheet[i, 5] = new SourceGrid.Cells.Cell("", txtRemarks);
                grdSalarySheet[i, 5].Value = "";

                grdSalarySheet[i, 6] = new SourceGrid.Cells.Cell("0");

                   
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        
        }

        private enum CmbType
        {
           Patient
         }
        private void LoadComboBox(ComboBox cbo, CmbType type)
        {
            try
            {
                cbo.Items.Clear();
                DataTable dt = new DataTable();
                switch (type)
                {
                        //Load Patinet Name in ComboBox
                    case CmbType.Patient:
                        dt = Patient.GetPatientNameForCmb();
                        break;
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        cbo.Items.Add(new ListItem((int)dr["ID"], dr["Value"].ToString()));

                    }
                   // cbo.SelectedIndex = -1;

                   
                }
            } 
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        ListItem liPatientName;
        private void cboName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboName.SelectedIndex != -1)
                {
                    liPatientName = (ListItem)cboName.SelectedItem;
                    DataTable dt = Patient.GetPatientByID(liPatientName.ID);
                    txtRegNo.Text = Convert.ToString(dt.Rows[0]["RegistrationNo"]);
                  // DateTime date =Convert.ToDateTime (dt.Rows[0]["Age"]);
                  // txtBirthDate.Text = Date.ToSystem(date);
                    txtAge.Text =  (dt.Rows[0]["Age"].ToString());
                   DateTime regdate = Convert.ToDateTime(dt.Rows[0]["RegistrationDate"]);
                   txtRegDate.Text = Date.ToSystem(regdate);
                   txtAddress.Text = dt.Rows[0]["Address"].ToString();
                   txtCity.Text = dt.Rows[0]["City"].ToString();
                   txtTelephone.Text = dt.Rows[0]["Telephone"].ToString();
                   txtMobile.Text = dt.Rows[0]["Mobile"].ToString();
                   txtRemarks.Text = dt.Rows[0]["Reason"].ToString();
                   int gender = Convert.ToInt32(dt.Rows[0]["Gender"]);
                    if(gender == 1)
                    {
                        txtGender.Text = "Male";
                    }
                    else if(gender == 2)
                    {
                        txtGender.Text = "Female";
                    }
                    else
                    {
                        txtGender.Text = "Other";
                    }
                }

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }

        }


       //Implementing interface DateConvert
        public void DateConvert(DateTime ReturnDotNetDate)
        {
             //if (DateStatus == "BIRTHDATE")
                ////txtBirthDate.Text = Date.ToSystem(ReturnDotNetDate);
            if (DateStatus == "REGDATE")
                txtRegDate.Text = Date.ToSystem(ReturnDotNetDate);
        }

        //Converting Date of Birth in nepali Fomat
        private void btnBirthDate_Click(object sender, EventArgs e)
        {

            //DateStatus = "BIRTHDATE";
            //frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtBirthDate.Text));
            //frm.ShowDialog();
        }
        //Convrting Date of Patient Registation in nepali format
        private void btnRegistrationDate_Click(object sender, EventArgs e)
        {
            DateStatus = "REGDATE";
            frmDateConverter frm = new frmDateConverter(this, Date.ToDotNet(txtRegDate.Text));
            frm.ShowDialog();
        }
        private enum DoctorGridColumn : int
        {
            Del = 0, SNo, Code_No, DoctorName, Specilization, Amount,  NetAmount,DocID
        };



      
        private void Doctor_Focused(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                ctx = (DevAge.Windows.Forms.DevAgeTextBox)sender;

                frmDoctorList fle = new frmDoctorList(this);
               fle.ShowDialog();
                SendKeys.Send("{TAB}{TAB}{TAB}");
            }

        }
      

        private void Text_Change(object sender, EventArgs e)
        {
            IsFieldChanged = true;
            
        }

        public void AddDoctor(int DocID, string Code, string Name, string Specilization, string BankAC, bool IsSelected)
        {


            if (IsSelected)
            {
                int CurRow = grdSalarySheet.Selection.GetSelectionRegion().GetRowsIndex()[0];

                ctx.Text = Name;
               
                grdSalarySheet[CurRow, 3].Value = Specilization;
            
                grdSalarySheet[CurRow, 6].Value = DocID;

                int RowsCount = grdSalarySheet.RowsCount;
                string LastServicesCell = (string)grdSalarySheet[RowsCount - 1, 2].Value;
                if (LastServicesCell != "(NEW)" || (CurRow + 1 == RowsCount))
                {
                    AddRow(RowsCount);
                }
            }
            hasChanged = true;
        }
      

       
        private void btnTenderAmount_Click(object sender, EventArgs e)
        {
            double totalamount;
            double vatamount = 0;
            double tax1amount = 0;
            double tax2amount = 0;
            double tax3amount = 0;
            
          
            totalamount = (Convert.ToDouble(lblNetAmout.Text)) + vatamount + tax1amount + tax2amount + tax3amount;
            frmtenderamount ft = new frmtenderamount(totalamount);
            ft.ShowDialog();
            btnSave.Focus();
        }

       
        private void btnNew_Click(object sender, EventArgs e)
        {
            isNew = true;
            LoadComboBox(cboName, CmbType.Patient);
            EnableControls(true);
            ChangeState(EntryMode.NEW);
            IsFieldChanged = false;
            txtRegDate.Mask = Date.FormatToMask();
            txtRegDate.Text = Date.DBToSystem(Date.GetServerDate().ToString()); //By default show the current date from the sqlserver.

            //Event trigerred when delete button is clicked
            evtDelete.Click += new EventHandler(Delete_Row_Click);
          
            AddGridHeader();
            AddRow(1);
            lblNetAmout.Text = "0";
            cboName.SelectedIndex = -1;
          
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    EnableControls(false);
                    ButtonState(true, true, false, true, false, true);
                    IsFieldChanged = false;
                    break;
                case EntryMode.NEW:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true, true);
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
                case EntryMode.EDIT:
                    EnableControls(true);
                    ButtonState(false, false, true, false, true, true);
                    IsFieldChanged = false;
                    btnSetup.Enabled = chkRecurring.Checked;
                    break;
            }
        }
        private void EnableControls(bool Enable)
        {
            chkRecurring.Enabled = btnSetup.Enabled = tabControl1.Enabled = Enable;
        }

        private void ButtonState(bool New, bool Edit, bool Save, bool Delete, bool Cancel, bool AddAccClass)
        {
            btnNew.Enabled = New;
            btnEdit.Enabled = Edit;
            btnSave.Enabled = Save;
            btnDelete.Enabled = Delete;
            btnRCancel.Enabled = Cancel;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EnableControls(true);
            ChangeState(EntryMode.EDIT);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            if (cboName.SelectedIndex == -1)
            {
                Global.Msg("Please select an Patient Name.");
                tabControl1.SelectedIndex = 0;
                cboName.DroppedDown = true;
                return;
            }
            
            PatientDetails pat = new PatientDetails();

            LiPatientID = (ListItem)cboName.SelectedItem;
            int PatientNameByid = LiPatientID.ID;
            pat.PNID = PatientNameByid;              
            pat.RegistrationNo = txtRegNo.Text.Trim();
            pat.RegistartionDate = Date.ToDotNet(txtRegDate.Text);         
            pat.Age = Convert.ToInt32(txtAge.Text);
            pat.sex = txtGender.Text;
            pat.Address = txtAddress.Text;
            pat.City = txtCity.Text;
            pat.Phone1 = txtTelephone.Text;
            pat.Phone2 = txtMobile.Text;
           
            pat.Reason = txtRemarks.Text;

            DataTable dtDoctorDetails = new DataTable();
            
            dtDoctorDetails.Columns.Add("DoctorName");
            dtDoctorDetails.Columns.Add("Specilization");          
            dtDoctorDetails.Columns.Add("Amount");                 
            dtDoctorDetails.Columns.Add("Remarks");
            dtDoctorDetails.Columns.Add("DoctorID");

            if (grdSalarySheet.Rows.Count() > 2)
            {
                for (int i = 1; i <= grdSalarySheet.Rows.Count() - 2; i++)
                {
                    dtDoctorDetails.Rows.Add(grdSalarySheet[i, 2].ToString(), 
                                            grdSalarySheet[i, 3].ToString(),
                                            Convert.ToDecimal(grdSalarySheet[i, 4].ToString()),
                                            grdSalarySheet[i, 5].ToString(), 
                                            Convert.ToInt32(grdSalarySheet[i, 6].ToString()));
                }
            }
            else
            {
                Global.Msg("Please fill the form at least with an doctor.");
                return;
            }


            int ReceiptID = 0;
            string insertResult = Patient.CreateReceipt(pat, dtDoctorDetails, out ReceiptID);
            if (insertResult == "SUCCESS")
            {
                btnNew_Click(sender, e);
                ChangeState(EntryMode.NEW);

            }
        }


        private void LoadSavedSalary(DataTable dtMaster, bool isCrystal)
        {
            try
            {
                if (!isCrystal)
                {
                    m_PartTimeSalaryMasterID = Convert.ToInt32(dtMaster.Rows[0]["ID"].ToString());
                    txtRegDate.Text = Date.ToSystem(Convert.ToDateTime(dtMaster.Rows[0]["RegistrationDate"].ToString()));
                    txtRemarks.Text = dtMaster.Rows[0]["Reason"].ToString();
                    txtRegNo.Text = dtMaster.Rows[0]["RegistrationNo"].ToString();
                    cboName.Text = dtMaster.Rows[0]["Name"].ToString();
                    txtAddress.Text = dtMaster.Rows[0]["Address"].ToString();
                    txtCity.Text = dtMaster.Rows[0]["City"].ToString();
                    txtAge.Text = dtMaster.Rows[0]["Narration"].ToString();
                    txtGender.Text = dtMaster.Rows[0]["Narration"].ToString();
                    txtTelephone.Text = dtMaster.Rows[0]["SN"].ToString();
                    txtMobile.Text = dtMaster.Rows[0][""].ToString();

                    
                }
                DataTable dtDetail = BusinessLogic.HRM.Employee.GetPartTimeSalaryDetail(m_PartTimeSalaryMasterID);
                int CurrRow = 1;
                if (dtDetail.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        if (!isCrystal)
                        {
                            grdSalarySheet[CurrRow, 2].Value = dr["EmpName"].ToString();
                            grdSalarySheet[CurrRow, 3].Value = dr["Designation"].ToString();
                            grdSalarySheet[CurrRow, 4].Value = dr["BankACNo"].ToString();
                            grdSalarySheet[CurrRow, 5].Value = Convert.ToDecimal(dr["ClassQty"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 6].Value = Convert.ToDecimal(dr["Rate"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 7].Value = Convert.ToDecimal(dr["Amount"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 8].Value = Convert.ToDecimal(dr["Tax"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 9].Value = Convert.ToDecimal(dr["NetAmount"].ToString()).ToString(Misc.FormatNumber(false, Global.DecimalPlaces));
                            grdSalarySheet[CurrRow, 10].Value = dr["Remarks"].ToString();
                            grdSalarySheet[CurrRow, 11].Value = dr["EmployeeID"].ToString();
                            CurrRow = CurrRow + 1;
                            AddRow(CurrRow);
                        }
                        else
                        {
                     //       dsPTS.Tables["tblPartTimeSalaryDetail"].Rows.Add(dr["EmpName"].ToString(), dr["Designation"].ToString(), dr["BankACNo"].ToString(), Convert.ToDecimal(dr["ClassQty"].ToString()), Convert.ToDecimal(dr["Rate"].ToString()), Convert.ToDecimal(dr["Amount"].ToString()), Convert.ToDecimal(dr["Tax"].ToString()), Convert.ToDecimal(dr["NetAmount"].ToString()), dr["Remarks"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }


        private void lblPostStatus_Click(object sender, EventArgs e)
        {

        }

        private void lblPostStatus_Click_1(object sender, EventArgs e)
        {

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {

        }



    }
}
