using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using BusinessLogic;

namespace AccSwift
{

 
    public partial class frmBconVoucherConfiguration : Form, IfrmVoucherFormat
    {
        private IfrmIsSalesConfigAdded m_ParentForm;//holds the datatable
        int SeriesID = 0;
        DataTable VoucherFormatDtl = new DataTable();
        VoucherConfiguration m_VouConfig = new VoucherConfiguration();
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        public frmBconVoucherConfiguration()
        {
            m_mode = EntryMode.NEW;
            InitializeComponent();
        }

        public frmBconVoucherConfiguration(Form ParentForm)
        {
            InitializeComponent();
            m_ParentForm = (IfrmIsSalesConfigAdded)ParentForm;
            m_mode = EntryMode.NEW;    
        }

        public frmBconVoucherConfiguration(int SeriesID,Form ParentForm)
        {
            m_mode = EntryMode.EDIT;
            this.SeriesID = SeriesID;
            m_ParentForm = (IfrmIsSalesConfigAdded)ParentForm;
            InitializeComponent();
        }

        public frmBconVoucherConfiguration(int SeriesID)
        {
            m_mode = EntryMode.EDIT;
            this.SeriesID = SeriesID;
            InitializeComponent();
        }

        private void frmPurchaseVoucherConfiguration_Load(object sender, EventArgs e)
        {
            #region SHOW THE DEFAULT VALUE IN RESPECTIVE FIELDS IN FORM LOAD CONDITION

            txtStartingNO.Text = "1";
            txtEndingNo.Text = "20";
            txtWarningVouLeft.Text = "15";
            txtWarningMsg.Text = "Voucher Number is going to finish soon!";
            txtTotalLengthNumPart.Text = "5";
            txtPaddingChar.Text = "0";
            chkSpecifyEndNo.Checked = true;
            chkFixLength.Checked = true;
            grpEndingDetails.Enabled = true;
            grpPaddingDetails.Enabled = true;

            #endregion
            ChangeState(m_mode);
            #region BY DEFAULT SELECTING THE FIRST INDEX OF THE COMBOBOX WHILE FORM LOAD CONDITION

            if (cmbNumberingType.Text == "")
            {
                cmbNumberingType.SelectedIndex = 0;
            }
            if (cmbBlankVouNum.Text == "")
            {
                cmbBlankVouNum.SelectedIndex = 0;
            }
            if (cmbDuplicateVouNum.Text == "")
            {
                cmbDuplicateVouNum.SelectedIndex = 0;
            }
            if (cboRenumberingFrq.Text == "")
            {
                cboRenumberingFrq.SelectedIndex = 0;
            }
            #endregion
            if (m_mode == EntryMode.EDIT)
            {
                //Fill up the form in respective filds
                #region FILL THIS FORM IN THE RESPECTIVE FIELDS IF EDIT BUTTON  IS CLICKED

                //BLOCK FOR FILLING THE SERIES NAME
                DataTable dt = VoucherConfiguration.GetSeriesInfo(SeriesID);
                DataRow dr = dt.Rows[0];
                txtSeriesName.Text = dr["EngName"].ToString();
                if (txtSeriesName.Text.ToUpper() == "MAIN")
                {
                    txtSeriesName.Enabled = false;
                }

                //BLOCK FOR FILLING THE RESPECTIVE FIELDS OF NUMBERING CONFIGURATION TAB
                DataTable dtVouNumConfig = m_VouConfig.GetVouNumConfiguration(SeriesID);
                DataRow drVouNumConfig = dtVouNumConfig.Rows[0];
                cmbNumberingType.Text = drVouNumConfig["NumberingType"].ToString();
                cmbDuplicateVouNum.Text = drVouNumConfig["DuplicateVouNum"].ToString();
                try
                {
                    if (drVouNumConfig["DuplicateVouNum"].ToString() == "WARNING_ONLY")
                    {
                        cmbDuplicateVouNum.SelectedIndex = 0;
                    }
                    if (drVouNumConfig["DuplicateVouNum"].ToString() == "DONT_ALLOW")
                    {
                        cmbDuplicateVouNum.SelectedIndex = 1;
                    }
                    if (drVouNumConfig["DuplicateVouNum"].ToString() == "NO_ACTION")
                    {
                        cmbDuplicateVouNum.SelectedIndex = 2;
                    }
                    if (drVouNumConfig["BlankVouNum"].ToString() == "WARNING_ONLY")
                    {
                        cmbBlankVouNum.SelectedIndex = 0;
                    }
                    if (drVouNumConfig["BlankVouNum"].ToString() == "DONT_ALLOW")
                    {
                        cmbBlankVouNum.SelectedIndex = 1;
                    }
                    if (drVouNumConfig["BlankVouNum"].ToString() == "NO_ACTION")
                    {
                        cmbBlankVouNum.SelectedIndex = 2;
                    }
                    txtStartingNO.Text = drVouNumConfig["StartingNo"].ToString();
                    if (drVouNumConfig["SpecifyEndNo"].ToString() == "True")
                    {
                        chkSpecifyEndNo.Checked = true;
                    }
                    txtEndingNo.Text = drVouNumConfig["EndNo"].ToString();
                    txtWarningVouLeft.Text = drVouNumConfig["WarningVouLeft"].ToString();
                    txtWarningMsg.Text = drVouNumConfig["WarningMsg"].ToString();
                    cboRenumberingFrq.Text = drVouNumConfig["RenumberingFrq"].ToString();
                    if (drVouNumConfig["NumericPart"].ToString() == "True")
                    {
                        chkFixLength.Checked = true;
                    }
                    txtTotalLengthNumPart.Text = drVouNumConfig["TotalLengthNumPart"].ToString();
                    txtPaddingChar.Text = drVouNumConfig["PaddingChar"].ToString();
                }
                catch (Exception ex)
                {
                    Global.Msg(ex.Message);
                }
            }
                #endregion
        }


        //A function from the Interface IfrmVoucherFormat. Used to apply the Datatable to this form from VoucherFormat
        public void AddNumberingFormat(DataTable NumberingFormat)
        {
            try
            {
                VoucherFormatDtl = NumberingFormat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNumberingFormat_Click(object sender, EventArgs e)
        {
            if (SeriesID > 0)//edit mode
            {
                frmVoucherFormat frm = new frmVoucherFormat(this, SeriesID);
                frm.Show();
            }
            else //new mode
            {
                frmVoucherFormat frm = new frmVoucherFormat(this);
                frm.Show();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtSeriesInfo = VoucherConfiguration.GetSeriesInfo("PURCH");
                if (txtSeriesName.Text.ToUpper() == "MAIN" && m_mode == EntryMode.NEW)
                {
                    foreach (DataRow drSeriesInfo in dtSeriesInfo.Rows)
                    {
                        if (drSeriesInfo["EngName"].ToString().ToUpper() == txtSeriesName.Text.ToUpper())
                        {
                            Global.Msg("Main Series Name already exist! Please choose another Series Name and try again!");
                            txtSeriesName.Focus();
                            return;
                        }
                    }
                }

                if (chkSpecifyEndNo.Checked && (txtEndingNo.Text == string.Empty || txtWarningMsg.Text == string.Empty || txtWarningVouLeft.Text == string.Empty))
                {
                    MessageBox.Show("Please specify the ending voucher number and all fields.");
                    txtEndingNo.Focus();
                    return;
                }
                if (Convert.ToInt32(txtEndingNo.Text) < Convert.ToInt32(txtWarningVouLeft.Text))
                {
                    MessageBox.Show("Warning voucher left number must be less than ending voucher number.");
                    txtWarningVouLeft.Focus();
                    return;
                }

                switch (m_mode)
                {

                    #region NEW
                    case EntryMode.NEW: //if new button is pressed                       

                        try
                        {
                            #region BLOCK FOR INSERTING THE PURCHASE VOUCHER CONFIGURATION

                            //FUNCTION FOR VALIDATING THE DATATYPE


                            if (ValidateDataType() == true)
                                return;
                            //If Numbering type is automatic then bydefault insert the value of Auto even datatable is empty
                            #region BYDEFALULT INSERT THE AUTO PART IN VOUCHER FORMAT IF AUTOMATIC NUMBERING TYPE

                            if (cmbNumberingType.Text == "Automatic")
                            {
                                if (VoucherFormatDtl.Rows.Count <= 0)
                                {
                                    VoucherFormatDtl.Columns.Add("Sno", typeof(int));//adding column as Sno(int type)
                                    VoucherFormatDtl.Columns.Add("Type", typeof(string));
                                    VoucherFormatDtl.Columns.Add("Param", typeof(string));
                                   VoucherFormatDtl.Rows.Add("1", "(AutoNumber)", "(Auto)");
                                }
                            }
                            #endregion

                            m_VouConfig.Insert(cmbNumberingType.Text, cmbDuplicateVouNum.Text, cmbBlankVouNum.Text, txtStartingNO.Text, chkSpecifyEndNo.Checked ? true : false, txtEndingNo.Text, txtWarningVouLeft.Text, txtWarningMsg.Text, cboRenumberingFrq.Text, chkFixLength.Checked ? true : false, txtTotalLengthNumPart.Text, txtPaddingChar.Text, txtSeriesName.Text, "PURCH", VoucherFormatDtl);
                            MessageBox.Show("Purchase Voucher Configuration Saved Successfully");
                            #endregion
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
                            #region SWITCH FOR HANDLING EXCEPTION WHILE INSERTING PURCHASE VOUCHER CONFIGURATION

                            switch (ex.Message)
                            {
                                case "BLANK_SERIES":
                                    MessageBox.Show("Please fill the Series Name first!");
                                    txtSeriesName.Focus();
                                    return;

                                case "COULD_NT_INSERT_SERIES":
                                    MessageBox.Show("Unable to insert the series");
                                    return;

                                case "BLANK_STARTING_NUM":
                                    MessageBox.Show("Please Fill the Starting Number First!");
                                    tabJournalVouConfig.SelectedTab = tabJournalVouConfig.TabPages[0];
                                    txtStartingNO.Focus();
                                    break;
                                case "SPECIFY_ENDING_NUM":
                                    MessageBox.Show("Please fill the Ending Number First!");
                                    tabJournalVouConfig.SelectedTab = tabJournalVouConfig.TabPages[0];
                                    txtEndingNo.Focus();
                                    break;

                                case "WARNING_VOU_NUM":
                                    MessageBox.Show("Please fill the Warning Number For Voucher Left!");
                                    txtWarningVouLeft.Focus();
                                    break;

                                case "WARNING_MESSAGE":
                                    MessageBox.Show("Please fill the Warning Message for Voucher Number Left!");
                                    txtWarningMsg.Focus();
                                    break;

                                case "FIX_LENGTH_NUMERIC_PART":
                                    MessageBox.Show("Please fill the Length of Numeric Part!");
                                    txtTotalLengthNumPart.Focus();
                                    break;
                                case "BLANK_PADDING_CHAR":
                                    MessageBox.Show("Please fill the Padding Character !");
                                    txtPaddingChar.Focus();
                                    break;

                            }
                            #endregion
                        }
                        break;

                    #endregion

                    #region EDIT
                    case EntryMode.EDIT: //if edit button is pressed
                        try
                        {

                            //FUNCTION FOR VALIDATING THE DATATYPE
                            if (ValidateDataType() == true)
                                return;
                            //If Numbering type is automatic then bydefault insert the value of Auto even datatable is empty
                            #region BYDEFALULT INSERT THE AUTO PART IN VOUCHER FORMAT IF AUTOMATIC NUMBERING TYPE

                            if (cmbNumberingType.Text == "Automatic")
                            {
                                if (VoucherFormatDtl.Rows.Count <= 0)
                                {
                                    VoucherFormatDtl.Columns.Add("Sno", typeof(int));//adding column as Sno(int type)
                                    VoucherFormatDtl.Columns.Add("Type", typeof(string));
                                    VoucherFormatDtl.Columns.Add("Param", typeof(string));
                                    VoucherFormatDtl.Rows.Add("1", "(AutoNumber)", "(Auto)");
                                }

                            }
                            #endregion
                            m_VouConfig.Modify(SeriesID, cmbNumberingType.Text, cmbDuplicateVouNum.Text, cmbBlankVouNum.Text, txtStartingNO.Text, chkSpecifyEndNo.Checked ? true : false, txtEndingNo.Text, txtWarningVouLeft.Text, txtWarningMsg.Text, cboRenumberingFrq.Text, chkFixLength.Checked ? true : false, txtTotalLengthNumPart.Text, txtPaddingChar.Text, txtSeriesName.Text, "PURCH", VoucherFormatDtl);
                            MessageBox.Show("Purchase Voucher Configuration Modified Successfully");
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
                            #region SWITCH FOR HANDLING EXCEPTION WHILE INSERTING PURCHASE VOUCHER CONFIGURATION

                            switch (ex.Message)
                            {
                                case "BLANK_SERIES":
                                    MessageBox.Show("Please fill the Series Name first!");
                                    txtSeriesName.Focus();
                                    break;

                                case "COULD_NT_INSERT_SERIES":
                                    MessageBox.Show("Unable to insert the series");
                                    break;
                                case "BLANK_STARTING_NUM":
                                    MessageBox.Show("Please Fill the Starting Number First!");
                                    tabJournalVouConfig.SelectedTab = tabJournalVouConfig.TabPages[0];
                                    txtStartingNO.Focus();
                                    break;
                                case "SPECIFY_ENDING_NUM":
                                    MessageBox.Show("Please fill the Ending Number First!");
                                    tabJournalVouConfig.SelectedTab = tabJournalVouConfig.TabPages[0];
                                    txtEndingNo.Focus();
                                    break;

                                case "WARNING_VOU_NUM":
                                    MessageBox.Show("Please fill the Warning Number For Voucher Left!");
                                    txtWarningVouLeft.Focus();
                                    break;

                                case "WARNING_MESSAGE":
                                    MessageBox.Show("Please fill the Warning Message for Voucher Number Left!");
                                    txtWarningMsg.Focus();
                                    break;

                                case "FIX_LENGTH_NUMERIC_PART":
                                    MessageBox.Show("Please fill the Length of Numeric Part!");
                                    txtTotalLengthNumPart.Focus();
                                    break;
                                case "BLANK_PADDING_CHAR":
                                    MessageBox.Show("Please fill the Padding Character !");
                                    txtPaddingChar.Focus();
                                    break;
                            }
                            #endregion
                        }
                        break;

                    #endregion
                }
                //Call the interface function to add the text in the parent form container
                m_ParentForm.IsVoucherConfigAdded(true);
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidateDataType()
        {

            #region VALIDATION FOR DATATYPE

            //VALIDATION FOR STARTING NUMBER
            object StartingNo = txtStartingNO.Text;
            bool IsInt = Misc.IsInt(StartingNo, false);//This function check whether variable is integer or not?
            if (IsInt == false)
            {
                MessageBox.Show("Please post the iteger type of Starting Number!");
                txtStartingNO.Focus();
                return true;
            }

            //VALIDATION FOR ENDING NUMBER
            object EndingNo = txtEndingNo.Text;

            bool IsInt1 = Misc.IsInt(EndingNo, false);

            if (IsInt1 == false)
            {
                MessageBox.Show("Please post the integer type of Ending Number!");
                txtEndingNo.Focus();
                return true;
            }

            //VALIDATATION FOR WARNING NUMBER FOR VOUCHER LEFT

            object WarningVouLeft = txtWarningVouLeft.Text;

            bool IsInt2 = Misc.IsInt(WarningVouLeft, false);
            if (IsInt2 == false)
            {
                MessageBox.Show("Please post the integer type of Warning Number for Voucher Left!");
                txtWarningVouLeft.Focus();
                return true;
            }

            //VALIDATION FOR TOTAL LENGTH OF NUMERIC PART

            object TotalLengthNumPart = txtTotalLengthNumPart.Text;
            bool IsInt3 = Misc.IsInt(TotalLengthNumPart, false);
            if (IsInt3 == false)
            {
                MessageBox.Show("Please post the integer type of Length of Numeric Part!");
                txtTotalLengthNumPart.Focus();
                return true;
            }

            #endregion
           return false;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;

            switch (m_mode)
            {
                case EntryMode.NORMAL:
                    ButtonState(true, false);
                    break;
                case EntryMode.NEW:
                    ButtonState(true, true);
                    break;
                case EntryMode.EDIT:

                    ButtonState(true, true);
                    break;
            }
        }

        //Enables and disables the button states
        private void ButtonState(bool Save, bool Cancel)
        {
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
        }

        private void cmbNumberingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNumberingType.SelectedItem.ToString() == "Automatic")
            {
                btnNumberingFormat.Visible = true;
                grpManualNumValidation.Enabled = false;
                grpAutoNumbering.Enabled = true;
            }
            //for making manual block enable true and other false
            else if (cmbNumberingType.SelectedItem.ToString() == "Manual")
            {
                btnNumberingFormat.Visible = false;
                grpAutoNumbering.Enabled = false;
                grpManualNumValidation.Enabled = true;
            }
            //for making all block enable false
            else
            {
                btnNumberingFormat.Visible = false;
                grpAutoNumbering.Enabled = false;
                grpManualNumValidation.Enabled = false;

            }
        }

        private void chkSpecifyEndNo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSpecifyEndNo.Checked == true)
            {
                grpEndingDetails.Enabled = true;
                txtEndingNo.Text = "20";
                txtWarningVouLeft.Text = "15";
                txtWarningMsg.Text = "Voucher Number is going to finish soon!";
            }
            else
            {
              grpEndingDetails.Enabled = false;
              txtEndingNo.Text = "";
              txtWarningVouLeft.Text = "";
              txtWarningMsg.Text = "";

            }
        }

        private void chkFixLength_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFixLength.Checked == true)
            {
                grpPaddingDetails.Enabled = true;
            }
            else
            {
                grpPaddingDetails.Enabled = false;

            }
        }

        private void frmPurchaseVoucherConfiguration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
