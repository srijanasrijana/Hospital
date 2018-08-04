using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using System.Collections;
using DateManager;


namespace AccSwift
{
    public partial class frmCreditNoteRegisterSettings : Form, IfrmSelectAccClassID,IfrmDateConverter
    {
        private bool IsFromDate = false;
        ReportPreference m_ReportPreference;
        private string Prefix = "";
        ArrayList AccClassID = new ArrayList();

        DataTable dtAccClassID = new DataTable();
        public frmCreditNoteRegisterSettings()
        {
            InitializeComponent();
        }

        private void frmCreditNoteRegisterSettings_Load(object sender, EventArgs e)
        {

            try
            {
                m_ReportPreference = new ReportPreference();
                LoadComboboxProject(cboProjectName, 0);
                grpDate.Enabled = false;
                txtFromDate.Mask = Date.FormatToMask();
                txtToDate.Mask = Date.FormatToMask();
                txtToDate.Text = Date.ToSystem(Date.GetServerDate()); //By default show the current date from the sqlserver.
                txtFromDate.Text = Date.ToSystem(new DateTime(2009, 01, 24));
                int checkuserid = User.CurrUserID;
                DataTable dtrpt = m_ReportPreference.GetPreferenceCount(checkuserid, "CREDIT_NOTE");
                if (dtrpt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtrpt.Rows)
                    {
                        switch (dr["Code"].ToString())
                        {

                            case "CN_ACCOUNTING_CLASS":
                                AccClassID.Add(dr["Value"].ToString());
                                ArrayList arrchildAccClassIds = new ArrayList();
                                AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                                foreach (object obj in arrchildAccClassIds)
                                {
                                    int i = (int)obj;
                                    AccClassID.Add(i.ToString());
                                }

                                break;
                            case "CN_PROJECT":
                                cboProjectName.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                                break;

                            case "CN_DATE":
                                if (dr["Value"].ToString() == "1")
                                {
                                    chkDate.Checked = true;
                                }
                                else
                                {
                                    chkDate.Checked = false;
                                }
                                break;

                        }
                    }
                }
                else
                {
                    //If nothing is selected add Root class ID
                    AccClassID.Add(Global.GlobalAccClassID.ToString());
                    //just for test
                    ArrayList arrchildAccClassIds = new ArrayList();
                    AccountClass.GetChildIDs(Global.GlobalAccClassID, ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                    foreach (object obj in arrchildAccClassIds)
                    {
                        int i = (int)obj;
                        AccClassID.Add(i.ToString());
                    }

                }



            }
            catch (Exception ex)
            {
                Global.Msg(ex.Message);
            }
            
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked == true)
            {
                grpDate.Enabled = true;



            }
            else
            {

                grpDate.Enabled = false;


            }
        }

        //A function from the Interface IfrmAccClassID. Used to apply the Datatable to this form from AddAccClass Form

        public void AddSelectedAccClassID(DataTable AccClassID1)
        {
            try
            {

                AccClassID.Clear();
                for (int i = 0; i < AccClassID1.Rows.Count; i++)
                {
                    DataRow drAccClassID = AccClassID1.Rows[i];
                    AccClassID.Add(drAccClassID["AccClassID"].ToString());

                }

            }
            catch (Exception)
            {

                throw;
            }


        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CREDITNOTE_REGISTER");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            CreditNoteRegisterSettings m_CredNoteRegSettings = new CreditNoteRegisterSettings();
            txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate 
            txtToDate.Mask = Date.FormatToMask();
            m_CredNoteRegSettings.FromDate = Date.ToDotNet(txtFromDate.Text);// Converting  datetime came via controls into DonNet datetime formate 
            m_CredNoteRegSettings.ToDate = Date.ToDotNet(txtToDate.Text);
            m_CredNoteRegSettings.AccClassID = m_CredNoteRegSettings.AccClassID;
            frmCreditNoteRegister frm = new frmCreditNoteRegister(m_CredNoteRegSettings);
            frm.Show();

        }

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {

            frmSelectAccClass frm = new frmSelectAccClass(this);
            frm.Show();
        }

        private void frmCreditNoteRegisterSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        //From date converter
        private void btnFromDate_Click(object sender, EventArgs e)
        {
            IsFromDate = true;//this variable is used as flag to notify which date is selected to change the date converter...coz same funtion is used to change the date  
            DateTime dtDate = Date.ToDotNet(txtFromDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

        //TO date converter
        private void btnToDate_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadComboboxProject(ComboBox ComboBoxControl, int ProjectID)
        {
            #region Language Management
            string LangField = "EngName";
            switch (LangMgr.DefaultLanguage)
            {
                case Lang.English:
                    LangField = "EngName";
                    break;
                case Lang.Nepali:
                    LangField = "NepName";
                    break;
            }
            #endregion
            DataTable dt = Project.GetProjectTable(ProjectID);
            //DataTable dt1 = AccountClass.GetAccClassTable(ProjectID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], Prefix + " " + dr[LangField].ToString()));
                Prefix += "----";
                LoadComboboxProject(ComboBoxControl, Convert.ToInt16(dr["ProjectID"].ToString()));
            }
            //Prefix = "--";
            if (Prefix.Length > 1)
            {
                Prefix = Prefix.Remove(Prefix.Length - 4, 4);
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }

        private void btnsavestate_Click(object sender, EventArgs e)
        {
            int UserID;
            string ReadXMLDetails;
            UserID = User.CurrUserID;
            ReadXMLDetails = ChangeReportPreferences();
            string Result = BalanceSheet.RptPreferences(UserID, ReadXMLDetails);
            if (Result == "INSERT")
            {
                Global.Msg("Report Preferences Inserted Sucessfully!!!");
            }
            else if (Result == "UPDATE")
            {
                Global.Msg("Report Preferences Updated Sucessfully!!!");
            }
            else if (Result == "FAILURE")
            {
                Global.Msg("Failed!!!");
            }
        }
        private string ChangeReportPreferences()
        {
            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);
            tw.WriteStartDocument();
            #region  Report PreferenceDetail
            string Code, Value;
            tw.WriteStartElement("RPD");
            {
                //For Accounting Class  
                Code = "CN_ACCOUNTING_CLASS";
                Value = Global.GlobalAccClassID.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Project
                Code = "CN_PROJECT";
                Value = cboProjectName.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For  date
                Code = "CN_DATE";
                if (chkDate.Checked)
                {
                    Value = "1";
                }
                else
                {
                    Value = "0";
                }

                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            //MessageBox.Show(strXML);
            return strXML;
            //string Code, Value;
            //int PreferenceID, UserID;
            //UserID = User.CurrUserID;
            //DataTable dt = m_ReportPreference.GetPreferenceInfo(UserID, "CREDIT_NOTE");

            ////For Accounting Class  
            //Code = "CN_ACCOUNTING_CLASS";
            //Value = Global.GlobalAccClassID.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Project
            //Code = "CN_PROJECT";
            //Value = cboProjectName.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}

            ////For  date
            //Code = "CN_DATE";
            //if (chkDate.Checked)
            //{
            //    Value = "1";
            //}
            //else
            //{
            //    Value = "0";
            //}

            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            //if (dt.Rows.Count < 1)
            //{
            //    Global.Msg("Report Preferences Inserted Sucessfully!!!");
            //}
            //else
            //{
            //    Global.Msg("Report Preferences Modified Sucessfully!!!");
            //}
        }

    }
}
