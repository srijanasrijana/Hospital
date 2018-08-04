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
    public partial class frmSalesReportSettings : Form, IfrmSelectAccClassID, IfrmDateConverter
    {
        private string Prefix = "";
        ReportPreference m_ReportPreference;
        ArrayList AccClassID = new ArrayList();
        private bool IsFromDate = false;
        Sales m_Sales = new Sales();
        public frmSalesReportSettings()
        {
            InitializeComponent();
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
            ComboBoxControl.Items.Add(new ListItem((0), "All"));
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

        private void frmSalesReport_Load(object sender, EventArgs e)
        {
            LoadMonths();
            m_ReportPreference = new ReportPreference();
            groupBox5.Enabled = false;
            rdProduct.Checked = true;
            // ListProject(cboProjectName);
            LoadComboboxProject(cboProjectName, 0);
            txtToDate.Mask = Date.FormatToMask();
            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
            txtFromDate.Mask = Date.FormatToMask();
            txtFromDate.Text = Date.ToSystem(Global.Fiscal_Year_Start);
            rdProductAll.Enabled = true;
            rdPartyAll.Enabled = true;
            cboProductGroup.Enabled = false;
            cboProductSingle.Enabled = false;
            cboPartyGroup.Enabled = false;
            cboPartySingle.Enabled = false;
            cboProjectName.Enabled = false;
            cboDeotwise.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            cboSalesAccount.Enabled = false;
           


            //Show the Ledgers of Product in form_Load condition
            DataTable dtProduct = Product.GetProductList(0);
            if (dtProduct.Rows.Count > 0)
            {
                foreach (DataRow drProduct in dtProduct.Rows)
                {
                    cboProductSingle.Items.Add(new ListItem((int)drProduct["ProductID"], drProduct["EngName"].ToString()));
                }
                cboProductSingle.DisplayMember = "value";//This value is  for showing at Load condition
                cboProductSingle.ValueMember = "id";//This value is stored only not to be shown at Load condition
                cboProductSingle.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
            }

            //Block for displaying the Ledgers of Cash/Party Account in combobox
            int Cash_In_Hand = 102;//GroupID of Cash_In_Hand is 102
            DataTable dtCash_In_HandLedgers = Ledger.GetAllLedger(Cash_In_Hand);//Collecting the Ledgers corresponding to Cash_In_Hand group
            foreach (DataRow drCash_In_HandLedgers in dtCash_In_HandLedgers.Rows)
            {
                cboPartySingle.Items.Add(new ListItem((int)drCash_In_HandLedgers["LedgerID"], drCash_In_HandLedgers["EngName"].ToString()));
            }
            int Debtor = 29;//GroupID of Debtor is 29
            DataTable dtDebtorLedgers = Ledger.GetAllLedger(Debtor);
            foreach (DataRow drDebtorLedgers in dtDebtorLedgers.Rows)
            {
                cboPartySingle.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drDebtorLedgers["EngName"].ToString()));
            }
            int Creditor = 114;
            DataTable dtCreditorLedgers = Ledger.GetAllLedger(Creditor);
            foreach (DataRow drCreditorLedgers in dtCreditorLedgers.Rows)
            {
                cboPartySingle.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drCreditorLedgers["EngName"].ToString()));
            }
            cboPartySingle.DisplayMember = "value";//This value is  for showing at Load condition
            cboPartySingle.ValueMember = "id";//This value is stored only not to be shown at Load condition
            cboPartySingle.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox

            //Display the Ledgers of Purchase in Form_Load Condition
            int Sales_ID = 112;//Manually inserting the value of GroupID for Purchase
            DataTable dtSalesLedgers = Ledger.GetAllLedger(Sales_ID);
            if (dtSalesLedgers.Rows.Count > 0)
            {
                foreach (DataRow drSalesLedgers in dtSalesLedgers.Rows)
                {
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drSalesLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboSalesAccount.Items.Add(new ListItem((int)drSalesLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }
                cboSalesAccount.DisplayMember = "value";//This value is  for showing at Load condition
                cboSalesAccount.ValueMember = "id";//This value is stored only not to be shown at Load condition
                cboSalesAccount.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
            }

            //Displaying Product Group on combobox
            DataTable dtProductGroupInfo = Product.GetGroupTable(-1);
            foreach (DataRow drProductGroupInfo in dtProductGroupInfo.Rows)
            {

                cboProductGroup.Items.Add(new ListItem((int)drProductGroupInfo["GroupID"], drProductGroupInfo["EngName"].ToString()));
            }
            cboProductGroup.SelectedIndex = 0;

            //Display the Cash/Party Group in load condition
            //In Cash/Party Group is the collection of Debtor,Creditor and CashInHand

            //For Cash_In_Hand Account Group which falls under Cash/Party Account Group
            DataTable dtCashInHandID = AccountGroup.GetGroupByID(102, LangMgr.DefaultLanguage);
            DataRow drCashInHandID = dtCashInHandID.Rows[0];
            DataTable dtCashInHandInfo = AccountGroup.GetGroupTable(Convert.ToInt32(drCashInHandID["ID"]));
            if (dtCashInHandID.Rows.Count > 0)
                cboPartyGroup.Items.Add(new ListItem((int)drCashInHandID["ID"], drCashInHandID["Name"].ToString()));
            foreach (DataRow drCashInHandInfo in dtCashInHandInfo.Rows)
            {
                cboPartyGroup.Items.Add(new ListItem((int)drCashInHandInfo["GroupID"], drCashInHandInfo["EngName"].ToString()));
            }
            //cboPartyGroup.SelectedIndex = 0;

            //For Debtor Account Group which falls under Cash/Party Account Group
            DataTable dtDebtorID = AccountGroup.GetGroupByID(29, LangMgr.DefaultLanguage);
            DataRow drDebtorID = dtDebtorID.Rows[0];
            if (dtDebtorID.Rows.Count > 0)
                cboPartyGroup.Items.Add(new ListItem((int)drDebtorID["ID"], drDebtorID["Name"].ToString()));
            DataTable dtDebtorInfo = AccountGroup.GetGroupTable(Convert.ToInt32(drDebtorID["ID"]));
            foreach (DataRow drDebtorInfo in dtDebtorInfo.Rows)
            {
                cboPartyGroup.Items.Add(new ListItem((int)drDebtorInfo["GroupID"], drDebtorInfo["EngName"].ToString()));
            }
            //For Creditor Account Group which falls under Cash/Party Account Group            
            DataTable dtCreditorID = AccountGroup.GetGroupByID(114, LangMgr.DefaultLanguage);
            DataRow drCreditID = dtCreditorID.Rows[0];
            if (dtCreditorID.Rows.Count > 0)
                cboPartyGroup.Items.Add(new ListItem((int)drCreditID["ID"], drCreditID["Name"].ToString()));
            DataTable dtCreditorInfo = AccountGroup.GetGroupTable(Convert.ToInt32(drCreditID["ID"]));
            foreach (DataRow drCreditorInfo in dtCreditorInfo.Rows)
            {
                cboPartyGroup.Items.Add(new ListItem((int)drCreditorInfo["GroupID"], drCreditorInfo["EngName"].ToString()));
            }
            cboPartyGroup.DisplayMember = "value";//This value is  for showing at Load condition
            cboPartyGroup.ValueMember = "id";//This value is stored only not to be shown at Load condition
            cboPartyGroup.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox

            //for depot

            #region BLOCK OF SHOWING DEPOT IN COMBOBOX
            DataTable dtDepotInfo = Depot.GetDepotInfo(-1);
            foreach (DataRow dr in dtDepotInfo.Rows)
            {

                cboDeotwise.Items.Add(new ListItem((int)dr["DepotID"], dr["DepotName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
            }
            //cboDeotwise.SelectedIndex = 0;

            #endregion
            //Load The User Wise Setting Stored
            int checkuserid = User.CurrUserID;
            DataTable dt = m_ReportPreference.GetPreferenceCount(checkuserid, "PURCHASE_REPORT");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    switch (dr["Code"].ToString())
                    {
                        case "PR_DATE":
                            txtToDate.Mask = Date.FormatToMask();
                            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
                            if (dr["Value"].ToString() == "1")
                            {
                                chkDateRange.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                chkDateRange.Checked = false;
                            }
                            break;

                        case "PR_ACCOUNTING_CLASS":
                            AccClassID.Add(dr["Value"].ToString());
                            ArrayList arrchildAccClassIds = new ArrayList();
                            AccountClass.GetChildIDs(Convert.ToInt32(dr["Value"].ToString()), ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                            foreach (object obj in arrchildAccClassIds)
                            {
                                int i = (int)obj;
                                AccClassID.Add(i.ToString());
                            }

                            break;
                        case "PR_PROJECT":
                            cboProjectName.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PR_SALESACCOUNT":
                            cboSalesAccount.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PR_DEPOT":
                            cboDeotwise.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PR_SINGLEPRODUCT":
                            cboProductSingle.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PR_PRODUCTGROUP":
                            cboProductGroup.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PR_SINGLEPARTY":
                            cboPartySingle.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PR_PARTYGROUP":
                            cboPartyGroup.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PR_PRODUCT":

                            if (dr["Value"].ToString() == "1")
                            {
                                rdProduct.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdProduct.Checked = false;
                            }
                            break;
                        case "PR_PARTY":

                            if (dr["Value"].ToString() == "1")
                            {
                                rdParty.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdParty.Checked = false;
                            }
                            break;
                        case "PR_RALLPARTY":

                            if (dr["Value"].ToString() == "1")
                            {
                                rdPartyAll.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdPartyAll.Checked = false;
                            }
                            break;
                        case "PR_RSINGLEPARTY":

                            if (dr["Value"].ToString() == "1")
                            {
                                rdpartySingle.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdpartySingle.Checked = false;
                            }
                            break;
                        case "PR_RPARTYGROUP":

                            if (dr["Value"].ToString() == "1")
                            {
                                rdPartyGroup.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdPartyGroup.Checked = false;
                            }
                            break;
                        case "PR_RALLPPRODUCT":

                            if (dr["Value"].ToString() == "1")
                            {
                                rdProductAll.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdProductAll.Checked = false;
                            }
                            break;
                        case "PR_RSINGLEPRODUCT":

                            if (dr["Value"].ToString() == "1")
                            {
                                rdProductSingle.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdProductSingle.Checked = false;
                            }
                            break;
                        case "PR_RPRODUCTGROUP":

                            if (dr["Value"].ToString() == "1")
                            {
                                rdProductGroup.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                rdProductGroup.Checked = false;
                            }
                            break;
                        case "PR_CDEPOT":

                            if (dr["Value"].ToString() == "1")
                            {
                                chkDepot.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                chkDepot.Checked = false;
                            }
                            break;
                        case "PR_CSALESACCOUNT":

                            if (dr["Value"].ToString() == "1")
                            {
                                chkSalesAccount.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                chkSalesAccount.Checked = false;
                            }
                            break;
                        case "PR_CPROJECT":

                            if (dr["Value"].ToString() == "1")
                            {
                                chkProject.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                chkProject.Checked = false;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("PURCH");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
            //    return;
            //}
            SalesReportSettings m_SalesReport = new SalesReportSettings();
            SalesPartyTransactSettings m_SalesParty = new SalesPartyTransactSettings();           

            if (chkDateRange.Checked)
            {
                txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate
                txtToDate.Mask = Date.FormatToMask();
                m_SalesReport.FromDate = Date.ToDotNet(txtFromDate.Text);
                m_SalesReport.ToDate = Date.ToDotNet(txtToDate.Text);
                m_SalesParty.FromDate = Date.ToDotNet(txtFromDate.Text);
                m_SalesParty.ToDate = Date.ToDotNet(txtFromDate.Text);
            }
            else
            {
                m_SalesReport.FromDate = null;
                m_SalesReport.ToDate = null;
                m_SalesParty.FromDate = null;
                m_SalesParty.ToDate = null;

            }
            //for product

            m_SalesReport.IsProductwise = rdProduct.Checked;
            //for single product
            ListItem LiProductSingleID = new ListItem();
            LiProductSingleID = (ListItem)cboProductSingle.SelectedItem;
            if (rdProductSingle.Checked)
            {
                m_SalesReport.ProductSingleID = Convert.ToInt32(LiProductSingleID.ID);
                m_SalesParty.ProductID = Convert.ToInt32(LiProductSingleID.ID);
            }
            else
            {
                m_SalesReport.ProductSingleID = null;
                m_SalesParty.ProductID = null;
            }
            //for product Group
            ListItem LiProductGroupID = new ListItem();
            LiProductGroupID = (ListItem)cboProductGroup.SelectedItem;
            if (rdProductGroup.Checked)
                m_SalesReport.ProductGroupID = Convert.ToInt32(LiProductGroupID.ID);
            else
                m_SalesReport.ProductGroupID = null;

            //for all product
            m_SalesReport.IsProductAll = rdProductAll.Checked;

            //For party
            m_SalesReport.IsPartywise = rdParty.Checked;
            //for Single Party
            ListItem LiPartySingleID = new ListItem();
            LiPartySingleID = (ListItem)cboPartySingle.SelectedItem;
            if (rdpartySingle.Checked)
            {
                m_SalesReport.PartySingleID = Convert.ToInt32(LiPartySingleID.ID);
                m_SalesParty.PartyID = Convert.ToInt32(LiPartySingleID.ID);
            }
            else
            {
                m_SalesReport.PartySingleID = null;
                m_SalesParty.PartyID = null;
            }

            //for Party group
            ListItem LiPartyGroupID = new ListItem();
            LiPartyGroupID = (ListItem)cboPartyGroup.SelectedItem;
            if (rdPartyGroup.Checked)
                m_SalesReport.PartyGroupID = Convert.ToInt32(LiPartyGroupID.ID);
            else
                m_SalesReport.PartyGroupID = null;
            //For All Party
            m_SalesReport.IsPartyAll = rdPartyAll.Checked;

            //for Depot
            ListItem LiDepotID = new ListItem();
            LiDepotID = (ListItem)cboDeotwise.SelectedItem;
            if (chkDepot.Checked)
            {
                m_SalesReport.DepotID = Convert.ToInt32(LiDepotID.ID);
                m_SalesParty.DepotID = Convert.ToInt32(LiDepotID.ID);
            }
            else
            {
                m_SalesReport.DepotID = null;
                m_SalesParty.DepotID = null;
            }

            //for Project 
            ListItem LiProjectID = new ListItem();
            //LiProjectID = (ListItem)cboDeotwise.SelectedItem;
            LiProjectID = (ListItem)cboProjectName.SelectedItem;
            if (chkProject.Checked)
            {
                m_SalesReport.ProjectID = Convert.ToInt32(LiProjectID.ID);
                m_SalesParty.ProjectID = Convert.ToInt32(LiProjectID.ID);
            }
            else
            {
                m_SalesReport.ProjectID = null;
                m_SalesParty.ProjectID = null;
            }
            //For Purchase Account
            
            ListItem liSalesLedgerID = new ListItem();
            liSalesLedgerID = (ListItem)cboSalesAccount.SelectedItem;
            if (chkSalesAccount.Checked)
            {
                m_SalesReport.SalesLedgerID = Convert.ToInt32(liSalesLedgerID.ID);
                m_SalesParty.SalesLedgerID = Convert.ToInt32(liSalesLedgerID.ID);
            }
            else
            {
                m_SalesReport.SalesLedgerID = null;
                m_SalesParty.SalesLedgerID = null;
            }
            m_SalesReport.AccClassID = AccClassID;
            m_SalesParty.AccClassID = AccClassID;
            if (m_SalesReport.PartySingleID > 0 || m_SalesReport.ProductSingleID > 0)
            {
                //if (m_SalesReport.IsProductwise)
                //    m_SalesParty.ReportType = InventoryReportType.ProductWise.ToString();
                //else if (m_SalesReport.IsPartywise)
                //    m_SalesParty.ReportType = InventoryReportType.PartyWise.ToString();

                if (rdProduct.Checked)
                    m_SalesParty.ReportType = InventoryReportType.ProductWise.ToString();
                else if (rdParty.Checked)
                    m_SalesParty.ReportType = InventoryReportType.PartyWise.ToString();
                

                frmSalesProductPartyTransaction frm1 = new frmSalesProductPartyTransaction(m_SalesParty);
                frm1.Show();
            }
            else
            {
                frmSalesReport frm = new frmSalesReport(m_SalesReport);
                frm.Show();
            }

        }

        private void rdProduct_CheckedChanged(object sender, EventArgs e)
        {
            MakeVisible(grpProduct);
            if (rdProduct.Checked == true)
            {
                rdProductAll.Checked = true;
            }
        }

        private void rdParty_CheckedChanged(object sender, EventArgs e)
        {
            MakeVisible(grpParty);
            if (rdParty.Checked == true)
            {
                rdPartyAll.Checked = true;
            }
        }

        private void MakeVisible(GroupBox grp)
        {
            grpProduct.Visible = false;
            grpParty.Visible = false;
            grp.Visible = true;
        }

        private void rdProductSingle_CheckedChanged(object sender, EventArgs e)
        {
            if (rdProductSingle.Checked == true)
            {
                cboProductSingle.Enabled = true;
                rdpartySingle.Checked = false;
            }
            else
            {
                cboProductSingle.Enabled = false;
            }
        }

        private void rdProductGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (rdProductGroup.Checked == true)
            {
                cboProductGroup.Enabled = true;
            }
            else
            {
                cboProductGroup.Enabled = false;
            }
        }

        private void chkDepot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepot.Checked)
                cboDeotwise.Enabled = true;
            else
                cboDeotwise.Enabled = false;
        }

        private void chkVoucherwise_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVoucherwise.Checked)
                cboVoucherwise.Enabled = true;
            else
                cboVoucherwise.Enabled = false;
        }

        private void chkSalesAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalesAccount.Checked)

                cboSalesAccount.Enabled = true;
            else
                cboSalesAccount.Enabled = false;
        }

        private void chkProject_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProject.Checked)
                cboProjectName.Enabled = true;
            else
                cboProjectName.Enabled = false;
        }

        private void chkDateRange_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDateRange.Checked)
            {
                
                    groupBox5.Enabled = true;
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = true;
                    btnFromDate.Enabled = true;
                    btnToDate.Enabled = true;

                
            }
            else
            {
                groupBox5.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                btnFromDate.Enabled = false;
                btnToDate.Enabled = false;
            }
        }

        private void rdpartySingle_CheckedChanged(object sender, EventArgs e)
        {
            if (rdpartySingle.Checked == true)
            {
                cboPartySingle.Enabled = true;
                rdProductSingle.Checked = false;
            }
            else
            {
                cboPartySingle.Enabled = false;
            }
        }

        private void rdPartyGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPartyGroup.Checked == true)
            {
                cboPartyGroup.Enabled = true;

            }
            else
            {
                cboPartyGroup.Enabled = false;
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
            //ComboBoxControl.SelectedIndex = 0;
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
                Code = "SR_DATE";
                if (chkDateRange.Checked)
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

                //For Accounting Class
                Code = "SR_ACCOUNTING_CLASS";
                Value = Global.GlobalAccClassID.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Project
                Code = "SR_PROJECT";
                Value = cboProjectName.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For SALES Account
                Code = "SR_SALESACCOUNT";
                Value = cboSalesAccount.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Depot Account
                Code = "SR_DEPOT";
                Value = cboDeotwise.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Single Product Account
                Code = "SR_SINGLEPRODUCT";
                Value = cboProductSingle.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For  Product Group
                Code = "SR_PRODUCTGROUP";
                Value = cboProductGroup.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For  single party
                Code = "SR_SINGLEPARTY";
                Value = cboPartySingle.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For   party group
                Code = "SR_PARTYGROUP";
                Value = cboPartyGroup.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Product
                Code = "SR_PRODUCT";
                if (rdProduct.Checked)
                    Value = "1";
                else
                    Value = "0";

                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Party
                Code = "SR_PARTY";
                if (rdParty.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For all Party
                Code = "SR_RALLPARTY";
                if (rdPartyAll.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Single Party
                Code = "SR_RSINGLEPARTY";
                if (rdpartySingle.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For party group
                Code = "SR_RPARTYGROUP";
                if (rdPartyGroup.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For all Product
                Code = "SR_RALLPPRODUCT";
                if (rdProductAll.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Single Product   
                Code = "SR_RSINGLEPRODUCT";
                if (rdProductSingle.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Product Group
                Code = "SR_RPRODUCTGROUP";
                if (rdProductGroup.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For CDepot
                Code = "SR_CDEPOT";
                if (chkDepot.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                 Code = "SR_CSALESACCOUNT";
                if (chkSalesAccount.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For CProject
                Code = "SR_CPROJECT";
                if (chkProject.Checked)
                    Value = "1";
                else
                    Value = "0";
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
