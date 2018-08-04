using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using BusinessLogic;
using RegistryManager;
using DBLogic;
using DateManager;
using System.Threading;
using Common;
using System.Diagnostics;


namespace AccSwift
{
    public partial class frmNewCompany : Form, IfrmCompanyFiscalYear
    {
        CompanyDetails CompDetails = new CompanyDetails();
        private byte[] imgLogo = null;
        Form ParentForm = null;

        public frmNewCompany()
        {
            InitializeComponent();
        }

        public frmNewCompany(Form ParentForm)
        {
            this.ParentForm = ParentForm;
            InitializeComponent();
        }

        public void FiscalYearCalculate(CompanyDetails CompDetail)
        {
            CompDetails = CompDetail;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCompanyName.Text == "")
                {
                    MessageBox.Show("Enter valid company name!");
                    txtCompanyName.Focus();
                    return;
                }

                frmProgress ProgressForm = new frmProgress();
                // Initialize the thread that will handle the background process
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        ProgressForm.ShowDialog();
                    }
                ));

                backgroundThread.Start();
                ProgressForm.UpdateProgress(20, "Company create progress...");

                /// <summary>
                ///Run our validation rules such as dbname, fiscal year etc.
                /// </summary> 
                /// while determining dbname you must have to check if dbname already exist or not.
                /// validation 1 -- Check if dbname exist or not?               
                int i = 1;

                string TestDBName = txtDBName.Text + i.ToString().PadLeft(3, '0');
                while (CreateDB.IsDBExist(TestDBName))
                {
                    i++;
                    TestDBName = txtDBName.Text + i.ToString().PadLeft(3, '0');
                }
                txtDBName.Text = TestDBName;

                //Create DB here 
                #region Database Creation With Proper Location
                //for this we have to fix the database directory
                string appPath = "";
                System.IO.DirectoryInfo directoryInfo = null;
                System.IO.DirectoryInfo directoryInfo1 = null;
                string path = "";
                try
                {
                    appPath = Path.GetDirectoryName(Application.ExecutablePath);
                    directoryInfo = System.IO.Directory.GetParent(appPath);
                    directoryInfo1 = System.IO.Directory.GetParent(directoryInfo.FullName);
                    path = directoryInfo1.FullName + @"\Database";
                }
                catch 
                {
                    if (!Directory.Exists(path))
                    {
                        path = appPath + "\\Database"; // if the database folder does not exist i.e. if the release folder exists independently of the application code, then we add database file in the release folder
                    }
                }
                //to create directory
                if (Directory.Exists(path))
                {
                    //Do nothing
                }
                else
                {
                    Directory.CreateDirectory(path);
                }

                DatabaseParam DBParam = new DatabaseParam();
                // DBParam.ServerName = "";
                DBParam.DatabaseName = txtDBName.Text;
                DBParam.DataFileGrowth = "4";
                DBParam.DataFileName = txtDBName.Text + "_Data";
                DBParam.DataFileSize = "2";//2MB at the init state
                DBParam.DataPathName = path + "\\" + DBParam.DataFileName + ".mdf";
                DBParam.LogFileGrowth = "4";
                DBParam.LogFileName = txtDBName.Text + "_Log";
                DBParam.LogFileSize = "1";//1MB at the init state
                DBParam.LogPathName = path + "\\" + DBParam.LogFileName + ".ldf";
                CreateDB.CreateDatabase(DBParam);

                #endregion

                ProgressForm.UpdateProgress(40, "Company create progress...");

                // After successful creationof database attach our database to that db
                #region Restore .bak file to temporary database
               
                if (!BackUpRestore.RestoreDatabase(DBParam.DatabaseName, path + "\\Mydb.Bak", DBParam.DataPathName, DBParam.LogPathName))
                {
                    Global.MsgError("Could not restore the database");
                    return;
                }

                #endregion

                ProgressForm.UpdateProgress(80, "Company create progress...");


                //Now change the  connection string to newly connected db
                #region Switch Connection String To New Database
                RegManager.DataBase = DBParam.DatabaseName;
                //refresh the whole application           
                SqlDb m_db = new SqlDb();
                //In case of SQL Server connection
                m_db.ServerName = RegManager.ServerName;
                m_db.DbName = RegManager.DataBase;
                m_db.UserName = RegManager.DBUser;
                m_db.Password = RegManager.DBPassword;

                if (m_db.Connect())
                {
                    RegManager.ServerName = m_db.ServerName;
                    RegManager.DataBase = m_db.DbName;
                    RegManager.DBUser = m_db.UserName;
                    RegManager.DBPassword = m_db.Password;
                    Global.m_db.cn = m_db.cn;
                    Global.ConnectionString = Global.m_db.cn.ConnectionString;
                }
                else
                {
                    MessageBox.Show("Connection to new company failed!!");
                    return;
                }
                #endregion

                ///<Summary>
                ///Also insert the general setting in the setting table.
                ///</Summary>
                ///paste latest modify setting from setting form
                ModifySettings();

                //Now Insert the Company information
                #region Write Company in Database
                //Entry for fiscal year
                try
                {
                    frmCompanyFiscalYear frmFY = new frmCompanyFiscalYear(this, CompDetails);
                    if (!frmFY.IsDisposed)
                    {
                        frmFY.ShowDialog();
                        frmFY.BringToFront();
                        frmFY.Focus();
                    }

                    CompDetails.CompanyName = txtCompanyName.Text;
                    CompDetails.CompanyCode = txtCompanyCode.Text;
                    CompDetails.Address1 = txtAddress1.Text;
                    CompDetails.Address2 = txtAddress2.Text;
                    CompDetails.City = txtCity.Text;
                    CompDetails.District = txtDistrict.Text;
                    CompDetails.Email = txtEmail.Text;
                    CompDetails.PAN = txtPan.Text;
                    CompDetails.POBox = txtPOBox.Text;
                    CompDetails.Telephone = txtTel.Text;
                    CompDetails.Website = txtWebsite.Text;
                    CompDetails.Zone = txtZone.Text;
                    //CompDetails.FYFrom = null; // Date.ToDotNet(txtDateFY.Text);
                    //CompDetails.BookBeginFrom = null; // Date.ToDotNet(txtDateBookBegin.Text);
                    ///<Summary>
                    ///We have to predetermine the database name from above information and create it then insert the company information.
                    ///</Summary>                
                    CompDetails.DBName = txtDBName.Text;

                    if (imgLogo != null)
                        CompDetails.Logo = imgLogo;
                    string Return = "";

                    CompanyInfo CompInfo = new CompanyInfo();
                    Return = CompInfo.Insert(CompDetails);

                #endregion

                    ProgressForm.UpdateProgress(90, "Company create progress...");

                    ///<Summary>
                    ///Write on the registry
                    ///</Summary>
                    #region Write on Registry

                    string NewCompany = RegManager.CreateNewCompany();
                    string NewFY = RegManager.CreateNewFY(NewCompany);
                    object RegValue = new object();
                    RegValue = txtCompanyCode.Text.Trim().ToUpper();
                    RegManager.Write(NewCompany, "CODE", RegValue);
                    RegValue = DBParam.DatabaseName;
                    RegManager.Write(NewCompany, "DATABASE", RegValue);
                    RegValue = txtCompanyName.Text.Trim();
                    RegManager.Write(NewCompany, "NAME", RegValue);


                    RegValue = DBParam.DatabaseName;
                    RegManager.Write(NewCompany + "\\" + NewFY, "DATABASE", RegValue);
                    RegValue = "30";
                    RegManager.Write(NewCompany + "\\" + NewFY, "DAY", RegValue);
                    RegValue = "04";
                    RegManager.Write(NewCompany + "\\" + NewFY, "MONTH", RegValue);
                    RegValue = "2013";
                    RegManager.Write(NewCompany + "\\" + NewFY, "YEAR", RegValue);

                    #endregion

                    ProgressForm.UpdateProgress(100, "Company create progress...");
                    // Close the dialog if it hasn't been already
                    if (ProgressForm.InvokeRequired)
                        ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

                    if (Return == "SUCCESS")
                        Global.Msg("Company created Successfully!");
                    else
                        Global.MsgError("Some Unknown Error Occured!");
                    this.Dispose();

                }
                catch (Exception ex)
                {
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var line = frame.GetFileLineNumber();
                    Global.MsgError(ex.Message + " " + ex.Source + " " + line.ToString());
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                Global.MsgError(ex.Message + " " + ex.Source + " " + line.ToString());
            }
        }

        /// <summary>
        /// This is the method for Modification of tblSettings...The value of tblSetting will be changed according to Code 
        /// </summary>
        private void ModifySettings()
        {
            Settings m_Settings = new Settings();
            string Code, Value;

            //GMOptions Settings
            #region GMOptions Setting
            if (rbDateEnglish.Checked)
            {
                Code = "DEFAULT_DATE";
                Value = "English";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Date = Date.DateType.English;//Passing the Default Date to the Global variable When this information is saved to database
                //Such that not necessary to close the main form to update this information
            }
            else if (rbDateNepali.Checked)
            {
                Code = "DEFAULT_DATE";
                Value = "Nepali";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Date = Date.DateType.Nepali;
            }
            if (cboDateFormat.Text != "")
            {
                Code = "DATE_FORMAT";
                Value = cboDateFormat.Text.ToString();
                m_Settings.SetSettings(Code, Value);
                switch (cboDateFormat.Text)
                {
                    case "YYYY/MM/DD":
                        Global.Default_Formate = Date.DateFormat.YYYY_MM_DD;
                        break;
                    case "DD/MM/YYYY":
                        Global.Default_Formate = Date.DateFormat.DD_MM_YYYY;
                        break;
                    case "MM/DD/YYYY":
                        Global.Default_Formate = Date.DateFormat.MM_DD_YYYY;
                        break;
                    default:
                        Global.Default_Formate = Date.DateFormat.YYYY_MM_DD;
                        break;
                }
            }
            if (cboDecimalPlaces.Text != "")
            {
                Code = "DEFAULT_DECIMALPLACES";
                Value = cboDecimalPlaces.Text.ToString();
                m_Settings.SetSettings(Code, Value);
                Global.DecimalPlaces = Convert.ToInt16(cboDecimalPlaces.Text);
            }
            if (chkCommaSeparated.Checked)
            {
                Code = "COMMA_SEPARATED";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Comma_Separated = true;
            }
            else if (!chkCommaSeparated.Checked)
            {
                Code = "COMMA_SEPARATED";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Comma_Separated = false;
            }
            if (rdbDecimalFormatInBracket.Checked)
            {
                Code = "DECIMAL_FORMAT";
                Value = "0";
                m_Settings.SetSettings(Code, Value);
                Global.Decimal_Format = "0";
            }
            else if (rdbDecimalFormatInNegative.Checked)
            {
                Code = "DECIMAL_FORMAT";
                Value = "1";
                m_Settings.SetSettings(Code, Value);
                Global.Decimal_Format = "1";
            }
            if (rbLangEnglish.Checked)
            {
                Code = "DEFAULT_LANGUAGE";
                Value = "English";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Language = Lang.English;
            }
            else if (rbLangNepali.Checked)
            {
                Code = "DEFAULT_LANGUAGE";
                Value = "Nepali";
                m_Settings.SetSettings(Code, Value);
                Global.Default_Language = Lang.Nepali;
            }           
         
            #endregion
        }


        private void frmNewCompany_Load(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.Close();
            //Loading general setting tab
            //Adding Number of Decimal Places in Combobox
            cboDateFormat.SelectedIndex = 0;
            for (int i = 0; i < 7; i++)
            {
                cboDecimalPlaces.Items.Add(i.ToString());
            }
            cboDecimalPlaces.SelectedIndex = 2;

            //Loading Company tab.
            //txtDateFY.Mask = Date.FormatToMask();
            //txtDateFY.Text = Date.ToSystem(Date.GetServerDate());
            //txtDateBookBegin.Mask = Date.FormatToMask();
            //txtDateBookBegin.Text = Date.ToSystem(Date.GetServerDate());

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openIMG = new OpenFileDialog();
            openIMG.Filter = "Known graphics format (*.bmp,*.jpg,*.gif,*.png)|*.bmp;*.jpg;*.gif;*.jpeg;*.png";
            openIMG.ShowDialog();
            string imgPath = openIMG.FileName;
            try
            {
                picLogo.Image = Image.FromFile(imgPath);
                imgLogo = Misc.ReadBitmap2ByteArray(imgPath.ToString());
            }
            catch (Exception ex)
            {
                //Probably cancelled the selected
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCompanyName_Leave(object sender, EventArgs e)
        {           
                string PreDBName = txtCompanyName.Text + "AAA";
                PreDBName = Regex.Replace(PreDBName, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
                while (PreDBName.Contains(" "))
                    PreDBName = PreDBName.Replace(" ", "");
                PreDBName = PreDBName.Substring(0, 4).Trim().ToUpper();              
                txtDBName.Text = PreDBName;
                txtCompanyCode.Text = PreDBName;                      
        }

        private void tabCompanyInfo_Click(object sender, EventArgs e)
        {

        }

        private void frmNewCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtCompanyCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void txtCompanyCode_KeyDown(object sender, KeyEventArgs e)
        {
         
        }

        private void txtCompanyCode_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
