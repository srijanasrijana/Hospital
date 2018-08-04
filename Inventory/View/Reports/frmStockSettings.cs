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
using System.Collections;
using Common;

namespace Inventory
{
    public partial class frmStockSettings : Form,IfrmSelectAccClassID,IfrmDateConverter
    {
        ArrayList AccClassID = new ArrayList();
        ReportPreference m_ReportPreference;
        private string Prefix = "";
        private bool IsAtTheEndDate = false;
        IMDIMainForm M_MDIMain;
        public frmStockSettings(IMDIMainForm frmMDI)
        {
            InitializeComponent();
            M_MDIMain = frmMDI;

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
                    tw.WriteStartElement("AccClassIDSettings");
                    foreach (string tag in AccClassID)
                    {
                        AccClassID.Add(Convert.ToInt32(tag));
                        tw.WriteElementString("AccClassID", Convert.ToInt32(tag).ToString());
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

        public void DateConvert(DateTime DotNetDate)
        {
            if (IsAtTheEndDate)//If form date is selected
            {
                DateTextBox.Text = Date.ToSystem(DotNetDate);
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
            do
            {
                if (MonthCounter > 12)
                    MonthCounter = 1;
                cboMonths.Items.Add(ListDate[MonthCounter - 1]);
                MonthCounter++;
            } while (MonthCounter != FYMonth);

        }            


        private void frmOpeningStockSettings_Load(object sender, EventArgs e)
        {
            LoadMonths();
            //rdProductAll.Checked = true;
            //cboProductGroup.Enabled = false;
            //cboProductSingle.Enabled = false;

            //chkDepot.Checked = false;
            //cmboDepot.Enabled = false;

            m_ReportPreference = new ReportPreference();
            Global.CheckAcc = "STOCKSTATUS";
            LoadComboboxProject(cboProjectName, 0);
            IsAtTheEndDate = true;
            DateTextBox.Mask = Date.FormatToMask();
            DateTextBox.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition

            rdProductAll.Checked = true;
            rdProductGroup.Checked = false;
            rdProductSingle.Checked = false;
            cboProductSingle.Enabled = false;
            cboProductGroup.Enabled = false;

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
            if (dtProductGroupInfo.Rows.Count > 0)
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
                    cmboDepot.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cmboDepot.SelectedIndex = 0;
            }
            #endregion
            int checkuserid = User.CurrUserID;
            DataTable dtrpt = m_ReportPreference.GetPreferenceCount(checkuserid, "STOCK_STATUS");
            if (dtrpt.Rows.Count > 0)
            {
                foreach (DataRow dr in dtrpt.Rows)
                {
                    switch (dr["Code"].ToString())
                    {

                        case "SS_ACCOUNTING_CLASS":
                            AccClassID.Add(dr["Value"].ToString());
                            ArrayList arrchildAccClassIds = new ArrayList();
                            AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                            foreach (object obj in arrchildAccClassIds)
                            {
                                int i = (int)obj;
                                AccClassID.Add(i.ToString());
                            }

                            break;
                        case "SS_PROJECT":
                            cboProjectName.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "SS_SINGLEPRODUCT":
                            cboProductSingle.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "SS_PRODUCTGROUP":
                            cboProductGroup.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "SS_DEPOT":
                            cmboDepot.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "SS_RALLPRODUCT":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdProductAll.Checked = true;
                            }
                            else
                            {
                                rdProductAll.Checked = false;
                            }
                            break;
                        case "SS_RSINGLEPRODUCT":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdProductSingle.Checked = true;
                            }
                            else
                            {
                                rdProductSingle.Checked = false;
                            }
                            break;
                        case "SS_RPRODUCTGROUP":
                            if (dr["Value"].ToString() == "1")
                            {
                                rdProductGroup.Checked = true;
                            }
                            else
                            {
                                rdProductGroup.Checked = false;
                            }
                            break;
                        case "SS_CDEPOT":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkDepot.Checked = true;

                            }
                            else
                            {
                                chkDepot.Checked = false;
                            }
                            break;
                        case "SS_SHOWZEROQUANTITY":
                            if (dr["Value"].ToString() == "1")
                            {
                                chkShowZeroQty.Checked = true;
                            }
                            else
                            {
                                chkShowZeroQty.Checked = false;
                            }
                            break;

                        case "SS_ISCLOSINGSTOCK":
                            if (dr["Value"].ToString() == "True")
                            {
                                rdClosingStock.Checked = true;
                            }
                            else
                            {
                                rdOpeningStock.Checked = true;
                            }
                            break;

                    }
                }
            }
            else
            {

                rdClosingStock.Checked = true;
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
            StockStatusSettings m_SLS = new StockStatusSettings();//for stock ledger settings
            ListItem LiProductGroupID = new ListItem();
            LiProductGroupID = (ListItem)cboProductGroup.SelectedItem;
            ListItem liProductSingleID = new ListItem();
            liProductSingleID = (ListItem)cboProductSingle.SelectedItem;
           
            //for Product group
            if (rdProductGroup.Checked)
            {
                m_SLS.ProductGroupID = Convert.ToInt32(LiProductGroupID.ID);
            }
            else
            {
                m_SLS.ProductGroupID = null;
            }
            //for single Product
            if (rdProductSingle.Checked)
            {
                
                    m_SLS.ProductID = Convert.ToInt32(liProductSingleID.ID);
            }
            else
            {
                m_SLS.ProductID = null;
            }
            //for date range
           

            //for Depot
            ListItem LiDepotID = new ListItem();
            LiDepotID = (ListItem)cmboDepot.SelectedItem;
            if (chkDepot.Checked)
            {
                m_SLS.Depot = LiDepotID.Value;
            }
            else
            {
                m_SLS.Depot = "";
            }

            if (chkShowZeroQty.Checked)
                m_SLS.ShowZeroQunatity = true;
           

            m_SLS.AtTheEndDate = Date.ToDotNet(DateTextBox.Text).AddDays(1);
            m_SLS.AccClassID = AccClassID;
            //m_SLS.OpeningStock = rdOpeningStock.Checked;
            //m_SLS.ClosingStock = rdClosingStock.Checked;
            ListItem liProjectID = new ListItem();
            liProjectID = (ListItem)cboProjectName.SelectedItem;
            m_SLS.ProjectID = liProjectID.ID;
           
            frmStockStatus frm = new frmStockStatus(m_SLS,M_MDIMain);
            
            frm.Show();
        }

        

        private void chkDepot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepot.Checked)
            {
                cmboDepot.Enabled = true;
            }
            else
            {
                cmboDepot.Enabled = false;

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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //From date converter
        private void btnDate_Click(object sender, EventArgs e)
        {
            DateTime dtDate = Date.ToDotNet(DateTextBox.Text);
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
                Code = "SS_ACCOUNTING_CLASS";
                Value = Global.GlobalAccClassID.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Project
                Code = "SS_PROJECT";
                Value = cboProjectName.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For R all product
                Code = "SS_RALLPRODUCT";
                if (rdProductAll.Checked)
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
                //For  rsingle product
                Code = "SS_RSINGLEPRODUCT";
                if (rdProductSingle.Checked)
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
                //For r product group
                Code = "SS_RPRODUCTGROUP";
                if (rdProductGroup.Checked)
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

                //For c depot
                Code = "SS_CDEPOT";
                if (chkDepot.Checked)
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
                //For zero quantity
                Code = "SS_SHOWZEROQUANTITY";
                if (chkShowZeroQty.Checked)
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
                //For product group
                Code = "SS_PRODUCTGROUP";
                Value = cboProductGroup.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For single product
                Code = "SS_SINGLEPRODUCT";
                Value = cboProductSingle.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For depot
                Code = "SS_DEPOT";
                Value = cmboDepot.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For is closing Stock
                Code = "SS_ISCLOSINGSTOCK";
                if (rdClosingStock.Checked)
                {
                    Value = "True";
                }
                else
                {
                    Value = "False";
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
              //  DateTime StartDate;
                if (Date.DefaultDate == Date.DateType.Nepali)
                {
                    //Get First Day
                   // StartDate = Date.NepToEng(FYYear, SelItem.ID, 1);
                    //Get Last Day
                    DataTable LastDay = Date.LastDayofMonthNep(FYYear, SelItem.ID);
                    FinalDate = Date.NepToEng(FYYear, SelItem.ID, Convert.ToInt16(LastDay.Rows[0][0]));
                }
                else
                {
                   // StartDate = Date.NepToEng(FYYear, SelItem.ID, 1);
                    FinalDate = new DateTime(FYYear, SelItem.ID, DateTime.DaysInMonth(FYYear, SelItem.ID));
                }
                //txtFromDate.Text = Date.ToSystem(StartDate);
                DateTextBox.Text = Date.ToSystem(FinalDate);
            }
            catch
            {
                //Ignore
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        
    }
}
