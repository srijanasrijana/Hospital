using System;
using System.Data;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using System.Collections;
using Common;

namespace Inventory
{
    public partial class frmStockLedgerSettings : Form, IfrmSelectAccClassID, IfrmDateConverter
    {
        ReportPreference m_ReportPreference;
        ArrayList AccClassID = new ArrayList();
        private string Prefix = "";
        private bool IsFromDate = false;
        IMDIMainForm m_MdiMain;
        public frmStockLedgerSettings( IMDIMainForm frmMain)
        {
            InitializeComponent();
            m_MdiMain=frmMain;
        }
        public void AddSelectedAccClassID(DataTable AccClassID1)
        {
            try
            {
                AccClassID.Clear();
                ////If nothing is selected, simply send the root class id
                if (AccClassID1.Rows.Count == 0)
                {
                    AccClassID.Add("0");
                }

                else
                {
                    for (int i = 0; i < AccClassID1.Rows.Count; i++)
                    {
                        DataRow drAccClassID = AccClassID1.Rows[i];
                        AccClassID.Add(drAccClassID["AccClassID"].ToString());
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
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

        private void groupBox3_Enter(object sender, EventArgs e)
        {
            if (chkDepot.Checked)
            {
                cboDepotwise.Enabled = true;
            }
            else
            {
                cboDepotwise.Enabled = false;
            }
        }

        private void chkProject_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProject.Checked)
            {
                cboProjectName.Enabled = true;
            }
            else
            {
                cboProjectName.Enabled = false;
            }
        }

        private void chkDateRange_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDateRange.Checked)
            {
                groupBox2.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                btnFromDate.Enabled = true;
                btnToDate.Enabled = true;

            }
            else
            {
                groupBox2.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                btnFromDate.Enabled = false;
                btnToDate.Enabled = false;
            }
        }
       
        private void rdProductSingle_CheckedChanged(object sender, EventArgs e)
        {
            
            if (rdProductSingle.Checked)
            {
                cboProductSingle.Enabled = true;
            }
            else
            {
                cboProductSingle.Enabled = false;
            }
        }
        private void rdProductGroup_CheckedChanged(object sender, EventArgs e)
        {
            
            if (rdProductGroup.Checked)
            {
                cboProductGroup.Enabled = true;
            }
            else
            {
                cboProductGroup.Enabled = false;
            }

        }

        private void ListProject(ComboBox ComboBoxControl)
        {
            #region Language Management
            ComboBoxControl.Font = LangMgr.GetFont();
            string LangField = "English";
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

            ComboBoxControl.Items.Clear();
            DataTable dt = Project.GetProjectTable(-1);
            //ComboBoxControl.Items.Add(new ListItem((0), "All"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["ProjectID"], dr[LangField].ToString()));
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }
        private void LoadMonths()
        {
            //Check Fiscal year(By default in English)
            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();

            //IF there are no companies created, simply return
            if (CompDetails == null)
            {
                return;
            }
            //get first month from start fiscal date 
            DateTime start = new DateTime();
            if (CompDetails.FYFrom != null)
            {
                start = Convert.ToDateTime(CompDetails.FYFrom); //English fiscal year
            }

            ListItem[] ListDate = new ListItem[12];
            for (int month = 0; month < 12; month++)
            {
                ListDate[month] = new ListItem();
                ListDate[month].ID = month + 1;
                ListDate[month].Value = Date.GetMonthList((Date.DateType)Date.DefaultDate, Language.LanguageType.English)[month + 1];

            }

            //   DateTime FYStartDate =  new DateTime();
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
            //do
            //{
            //    if (MonthCounter > 12)
            //        MonthCounter = 1;
            //    cboMonths.Items.Add(ListDate[MonthCounter - 1]);
            //    MonthCounter++;
            //} while (MonthCounter != FYMonth);

            // new code 
            for (int i = 0; i < 12; i++)
            {
                cboMonths.Items.Add(ListDate[MonthCounter - 1]);
                if (MonthCounter >= 12)
                    MonthCounter = 1;
                else
                    MonthCounter++;
            }
        }            

        private void frmStockLedgerSettings_Load(object sender, EventArgs e)
        {
            LoadMonths();
            m_ReportPreference = new ReportPreference();
            groupBox2.Enabled=true;
           
            //ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            rdProductSingle.Checked = true;
            cboProductGroup.Enabled = false;
            cboProductSingle.Enabled = true; ;
            cboProjectName.Enabled = false;
            cboDepotwise.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;

            txtToDate.Mask = Date.FormatToMask();
            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
            txtFromDate.Mask = Date.FormatToMask();
            txtFromDate.Text = Date.ToSystem(new DateTime(2009, 01, 24));
            //Show the Ledgers of Product in form_Load condition
            DataTable dtProduct = Product.GetProductList1(0);
            if (dtProduct.Rows.Count > 0)
            {
                for (int i = 1; i <= dtProduct.Rows.Count; i++)
                {
                    DataRow drProduct = dtProduct.Rows[i - 1];
                    cboProductSingle.Items.Add(new ListItem((int)drProduct["ProductID"], drProduct["EngName"].ToString()));
                }
                cboProductSingle.DisplayMember = "value";//This value is  for showing at Load condition
                cboProductSingle.ValueMember = "id";//This value is stored only not to be shown at Load condition
                cboProductSingle.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
            }

            //Displaying Product Group on combobox
            DataTable dtProductGroupInfo = Product.GetGroupTable(-1);
            if(dtProductGroupInfo.Rows.Count>0)
            {
            foreach (DataRow drProductGroupInfo in dtProductGroupInfo.Rows)
            {

                cboProductGroup.Items.Add(new ListItem((int)drProductGroupInfo["GroupID"], drProductGroupInfo["EngName"].ToString()));
            }
            cboProductGroup.DisplayMember = "value";
            cboProductGroup.ValueMember = "id";
            cboProductGroup.SelectedIndex = 0;
            }
            //for depot
            #region BLOCK OF SHOWING DEPOT IN COMBOBOX
            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            if (dtDepotInfo.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDepotInfo.Rows)
                {
                    cboDepotwise.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cboDepotwise.SelectedIndex = 0;
            }
            #endregion
            int checkuserid = User.CurrUserID;

            //Get settings from Preference
            DataTable dtrpt = m_ReportPreference.GetPreferenceCount(checkuserid, "STOCK_LEDGER");
            if (dtrpt.Rows.Count > 0)
            {
                foreach (DataRow dr in dtrpt.Rows)
                {
                    switch (dr["Code"].ToString())
                    {

                        case "SL_ACCOUNTING_CLASS":
                            
                            AccClassID.Add(dr["Value"].ToString());
                            ArrayList arrchildAccClassIds = new ArrayList();
                            AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                            foreach (object obj in arrchildAccClassIds)
                            {
                                int i = (int)obj;
                                AccClassID.Add(i.ToString());
                            }
                             

                            break;
                        case "SL_PROJECT":
                            cboProjectName.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "SL_DEPOT":
                            cboDepotwise.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "SL_SINGLEPARTY":
                            cboProductSingle.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "SL_PARTYGROUP":
                            cboProductGroup.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "SL_RALLPARTY":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdProductAll.Checked = true;
                            }
                            else
                            {
                                rdProductAll.Checked = false;
                            }
                            break;
                        case "SL_RSINGLEPARTY":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdProductSingle.Checked = true;
                            }
                            else
                            {
                                rdProductSingle.Checked = false;
                            }
                            break;
                        case "SL_RPARTYGROUP":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdProductGroup.Checked = true;
                            }
                            else
                            {
                                rdProductGroup.Checked = false;
                            }
                            break;
                        case "SL_CDEPOT":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkDepot.Checked = true;

                            }
                            else
                            {
                                chkDepot.Checked = false;
                            }
                            break;
                        case "SL_CPROJECT":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkProject.Checked = true;
                            }
                            else
                            {
                                chkProject.Checked = false;
                            }
                            break;

                        case "SL_DATE":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkDateRange.Checked = true;
                            }
                            else
                            {
                                chkDateRange.Checked = false;
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

        private void btnShow_Click(object sender, EventArgs e)
        {

            InventoryBookSettings m_SLS = new InventoryBookSettings();//for stock ledger settings


            //for Product group
            if (rdProductGroup.Checked)
            {
                ListItem LiProductGroupID = new ListItem();
                LiProductGroupID = (ListItem)cboProductGroup.SelectedItem;
                m_SLS.ProductGroupID = Convert.ToInt32(LiProductGroupID.ID);
            }
            else
            {
                m_SLS.ProductGroupID = null;
            }

            //for single Product
            if (rdProductSingle.Checked && (cboProductSingle.SelectedIndex >= 0))
            {

                ListItem liPartyID = new ListItem();
                liPartyID = (ListItem)cboProductSingle.SelectedItem;
                m_SLS.ProductID = Convert.ToInt32(liPartyID.ID);
                m_SLS.ProductName = liPartyID.Value.ToString();


            }
            else
            {
                m_SLS.ProductID = null;
            }



            //for date range
            if (chkDateRange.Checked)
            {
                txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate
                txtToDate.Mask = Date.FormatToMask();
                m_SLS.FromDate = Date.ToDotNet(txtFromDate.Text);
                m_SLS.ToDate = Date.ToDotNet(txtToDate.Text);
            }
            else
            {
                m_SLS.FromDate = null;
                m_SLS.ToDate = null;
            }

            //for Depot
            ListItem LiDepotID = new ListItem();
            LiDepotID = (ListItem)cboDepotwise.SelectedItem;
            if (chkDepot.Checked)
            {
                m_SLS.DepotID = Convert.ToInt32(LiDepotID.ID);
            }
            else
            {
                m_SLS.DepotID = null;
            }

            //for Project 
            ListItem LiProjectID = new ListItem();
            LiProjectID = (ListItem)cboProjectName.SelectedItem;
            if (chkProject.Checked)
            {
                m_SLS.ProjectID = Convert.ToInt32(LiProjectID.ID);
            }
            else
            {
                m_SLS.ProjectID = null;
            }
            m_SLS.AccClassID = AccClassID;

            //Calculate opening Balance for specific product
            DataTable dtOpeningStockStatusInfo = StockStatusBook.GetOpeningStockStatusBook(Convert.ToInt32(m_SLS.ProductGroupID),Convert.ToInt32( m_SLS.ProductID), LiDepotID.Value, m_SLS.ToDate, true, StockStatusType.OpeningStock, ReadAllAccClassID(m_SLS.AccClassID));
           
                for (int i = 0; i < dtOpeningStockStatusInfo.Rows.Count; i++)
                {
                    DataRow dr = dtOpeningStockStatusInfo.Rows[i];
                    m_SLS.OpeningQty += Convert.ToInt32(dr["Quantity"]); //Get the sum of all opening quantities of all product in case of multiple products
                }


                frmStockLedger frm = new frmStockLedger(m_SLS, m_MdiMain);
                frm.Show();
           
            }
        


        /// <summary>
        /// Converts Array Lists Accounting Class IDs to XML Account Class IDs
        /// </summary>
        /// <param name="AccClassID"></param>
        /// <returns></returns>
        private string ReadAllAccClassID(ArrayList AccClassID)
        {
            #region  AccountingClassID

            ArrayList arrChildAccClassIDs = new ArrayList();
            foreach (object j in AccClassID)
            {
                AccountClass.GetChildIDs(Convert.ToInt32(j), ref arrChildAccClassIDs);

            }
            AccClassID.AddRange(arrChildAccClassIDs);

            #endregion

            System.Text.Encoding AEncoder = System.Text.Encoding.Unicode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(ms, AEncoder);

            tw.WriteStartDocument();
            #region  Accountclass
            tw.WriteStartElement("STOCKSTATUS");
            {
                //Write Checked Accounting class ID
                try
                {
                    tw.WriteStartElement("ACCCLASSIDS");
                    foreach (string tag in AccClassID)
                    {
                        AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccID", Convert.ToInt32(tag).ToString());
                    }
                    tw.WriteEndElement();
                }
                catch
                { }

            }
            tw.WriteFullEndElement();
            #endregion
            tw.WriteEndDocument();
            tw.Flush();
            tw.Close();
            string strXML = AEncoder.GetString(ms.ToArray());
            return strXML;
        }



        private void chkDepot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepot.Checked)
            {
                cboDepotwise.Enabled = true;
            }
            else
            {
                cboDepotwise.Enabled = false;

            }
        }

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {
            //just for test
            AccountLedgerSettings ALS = new AccountLedgerSettings();
            try
            {
                ALS.AccClassID = AccClassID;

            }
            catch
            {
                //Ignore 
            }
            Common.frmSelectAccClass frm = new Common.frmSelectAccClass(this, ALS.AccClassID);

            if (!frm.IsDisposed)
                frm.ShowDialog();
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
            ChangeReportPreferences();
        }
        private void ChangeReportPreferences()
        {
            string Code, Value;
            int PreferenceID, UserID;
            UserID = User.CurrUserID;
            DataTable dt = m_ReportPreference.GetPreferenceInfo(UserID, "STOCK_LEDGER");

            //For Accounting Class
            Code = "SL_ACCOUNTING_CLASS";
            Value = Global.GlobalAccClassID.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For Project
            Code = "SL_PROJECT";
            Value = cboProjectName.SelectedIndex.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For Depot
            Code = "SL_DEPOT";
            Value = cboDepotwise.SelectedIndex.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For Single Party
            Code = "SL_SINGLEPARTY";
            Value = cboProductSingle.SelectedIndex.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For Party Group
            Code = "SL_PARTYGROUP";
            Value = cboProductGroup.SelectedIndex.ToString();
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For R all party
            Code = "SL_RALLPARTY";
            if (rdProductAll.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }

            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For  rsingle party
            Code = "SL_RSINGLEPARTY";
            if (rdProductSingle.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For r product Party
            Code = "SL_RPARTYGROUP";
            if (rdProductGroup.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }
            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }

            //For c depot
            Code = "SL_CDEPOT";
            if (chkDepot.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }

            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For c project
            Code = "SL_CPROJECT";
            if (chkProject.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }

            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            //For c date
            Code = "SL_DATE";
            if (chkDateRange.Checked)
            {
                Value = "1";
            }
            else
            {
                Value = "0";
            }

            PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            if (dt.Rows.Count < 1)
            {
                m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            }
            else
            {
                m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            }
            if (dt.Rows.Count < 1)
            {
                Global.Msg("Report Preferences Inserted Sucessfully!!!");
            }
            else
            {
                Global.Msg("Report Preferences Modified Sucessfully!!!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboMonths_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                ListItem SelItem = (ListItem)cboMonths.SelectedItem;
                CompanyDetails CompDetails = new CompanyDetails();
                CompDetails = CompanyInfo.GetInfo();
                // DateTime FYStartDate = new DateTime();
                //if(CompDetails.FYFrom != null)
                DateTime FYStartDate = Convert.ToDateTime(CompDetails.FYFrom);

                //Convert Fiscal year to nepali
                int FYYear = FYStartDate.Year;
                int FYMonth = FYStartDate.Month;
                int FYDay = FYStartDate.Day;

                //If DateType is Nepali, load Nepali month
                if (Date.DefaultDate == Date.DateType.Nepali)
                    Date.EngToNep(FYStartDate, ref FYYear, ref FYMonth, ref FYDay);
                //Get the nepali fiscal year starting month


                //If FYMonth is greater than selected month, then the year is next year
                if (FYMonth > SelItem.ID)
                    FYYear++;

                //If it was Nepali, set back to DateTime type
                DateTime FinalDate;
                DateTime StartDate;
                if (Date.DefaultDate == Date.DateType.Nepali)
                {
                    //Get First Day
                    StartDate = Date.NepToEng(FYYear, SelItem.ID, 1);
                    //Get Last Day
                    DataTable LastDay = Date.LastDayofMonthNep(FYYear, SelItem.ID);
                    FinalDate = Date.NepToEng(FYYear, SelItem.ID, Convert.ToInt16(LastDay.Rows[0][0]));
                }
                else
                {
                    StartDate = Date.NepToEng(FYYear, SelItem.ID, 1);
                    FinalDate = new DateTime(FYYear, SelItem.ID, DateTime.DaysInMonth(FYYear, SelItem.ID));
                }
                txtFromDate.Text = Date.ToSystem(StartDate);
                txtToDate.Text = Date.ToSystem(FinalDate);
            }
            catch
            {
                //Ignore
            }
        }
    }
}
