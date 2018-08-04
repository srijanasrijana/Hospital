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
    public partial class frmPurchaseReportSettings : Form, IfrmSelectAccClassID, IfrmDateConverter
    {
        private string Prefix = "";
        ReportPreference m_ReportPreference;
        ArrayList AccClassID = new ArrayList();
        private bool IsFromDate = false;
        Purchase m_Purchase = new Purchase();
        public frmPurchaseReportSettings()
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
            //ComboBoxControl.SelectedIndex = 0;
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

        private void frmPurchaseReportSettings_Load(object sender, EventArgs e)
        {
            LoadMonths();
            m_ReportPreference = new ReportPreference();
            grpDate.Enabled = false;
            rdProduct.Checked = true;
          //  cboPurchaseAccount.SelectedIndex =0;
            //ListProject(cboProjectName);
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
            cboPurchaseAccount.Enabled = false;
            //cboProjectName.Items.Add(new ListItem((0), "All"));
            //Show the Ledgers of Product in form_Load condition
            DataTable dtProduct = Product.GetProductList(0);
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

            //Block for displaying the Ledgers of Cash/Party Account in combobox
            int Cash_In_Hand = 102;//GroupID of Cash_In_Hand is 102
            DataTable dtCash_In_HandLedgers = Ledger.GetAllLedger(Cash_In_Hand);//Collecting the Ledgers corresponding to Cash_In_Hand group
            if (dtCash_In_HandLedgers.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCash_In_HandLedgers.Rows.Count; i++)
                {
                    DataRow drCash_In_HandLedgers = dtCash_In_HandLedgers.Rows[i - 1];
                    cboPartySingle.Items.Add(new ListItem((int)drCash_In_HandLedgers["LedgerID"], drCash_In_HandLedgers["EngName"].ToString()));
                }
            }
            int Debtor = 29;//GroupID of Debtor is 29
            DataTable dtDebtorLedgers = Ledger.GetAllLedger(Debtor);
            if (dtDebtorLedgers.Rows.Count > 0)
            {
                for (int i = 1; i <= dtDebtorLedgers.Rows.Count; i++)
                {
                    DataRow drDebtorLedgers = dtDebtorLedgers.Rows[i - 1];
                    cboPartySingle.Items.Add(new ListItem((int)drDebtorLedgers["LedgerID"], drDebtorLedgers["EngName"].ToString()));
                }
            }
            int Creditor = 114;
            DataTable dtCreditorLedgers = Ledger.GetAllLedger(Creditor);
            if (dtCreditorLedgers.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCreditorLedgers.Rows.Count; i++)
                {
                    DataRow drCreditorLedgers = dtCreditorLedgers.Rows[i - 1];
                    cboPartySingle.Items.Add(new ListItem((int)drCreditorLedgers["LedgerID"], drCreditorLedgers["EngName"].ToString()));
                }

            }

            //If any of the Ledger is present than only the following is implemented.
            if(dtCash_In_HandLedgers.Rows.Count > 0 || dtDebtorLedgers.Rows.Count > 0 || dtCreditorLedgers.Rows.Count > 0)
            {
                cboPartySingle.DisplayMember = "value";//This value is  for showing at Load condition
                cboPartySingle.ValueMember = "id";//This value is stored only not to be shown at Load condition
                cboPartySingle.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox

            }

            //Display the Ledgers of Purchase in Form_Load Condition
            int Purchase_ID = 12;//Manually inserting the value of GroupID for Purchase
            DataTable dtPurchaseLedgers = Ledger.GetAllLedger(Purchase_ID);
            if (dtPurchaseLedgers.Rows.Count > 0)
            {
                for (int i = 1; i <= dtPurchaseLedgers.Rows.Count; i++)
                {
                    DataRow drPurchaseLedgers = dtPurchaseLedgers.Rows[i - 1];
                    DataTable dtLedgerInfo = Ledger.GetLedgerInfo(Convert.ToInt32(drPurchaseLedgers["LedgerID"]), LangMgr.DefaultLanguage);
                    DataRow drLedgerInfo = dtLedgerInfo.Rows[0];//There is no multiple rows in datatable.So,in case of single row in datatable use this syntax
                    cboPurchaseAccount.Items.Add(new ListItem((int)drPurchaseLedgers["LedgerID"], drLedgerInfo["LedName"].ToString()));//It adds LedgerID as well as LedgerName in combobox
                }

                cboPurchaseAccount.DisplayMember = "value";//This value is  for showing at Load condition
                cboPurchaseAccount.ValueMember = "id";//This value is stored only not to be shown at Load condition
                //cboPurchaseAccount.SelectedIndex = 0;//At the form load condition by default show the displayMember of first index of combobox
            }


           
            //Displaying Product Group on combobox
            DataTable dtProductGroupInfo = Product.GetGroupTable(-1);
            foreach (DataRow drProductGroupInfo in dtProductGroupInfo.Rows)
            {

                cboProductGroup.Items.Add(new ListItem((int)drProductGroupInfo["GroupID"], drProductGroupInfo["EngName"].ToString()));
            }
            cboProductGroup.DisplayMember = "value";
            cboProductGroup.ValueMember = "id";
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
            LoadComboboxProject(cboProjectName, 0);
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
                            cboProjectName.SelectedIndex =  Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "PR_PURCHASEACCOUNT":
                            cboPurchaseAccount.SelectedIndex = Convert.ToInt32(dr["Value"].ToString());
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
                        case "PR_CPURCHASEACCOUNT":
                         
                            if (dr["Value"].ToString() == "1")
                            {
                                chkPurchaseAccount.Checked = true;
                            }
                            else if (dr["Value"].ToString() == "0")
                            {
                                chkPurchaseAccount.Checked = false;
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

        private void btnShow_Click(object sender, EventArgs e)
        {
            //bool chkUserPermission = UserPermission.ChkUserPermission("PURCH");
            //if (chkUserPermission == false)
            //{
            //    Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
            //    return;
            //}
            PurchaseReportSettings m_PurchaseReport = new PurchaseReportSettings();
            PurchasePartyTransactSettings m_PurchaseParty = new PurchasePartyTransactSettings();

            if (chkDateRange.Checked)
            {
                txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate
                txtToDate.Mask = Date.FormatToMask();
                m_PurchaseReport.FromDate = Date.ToDotNet(txtFromDate.Text);
                m_PurchaseParty.FromDate = Date.ToDotNet(txtFromDate.Text);
                m_PurchaseReport.ToDate = Date.ToDotNet(txtToDate.Text);
                m_PurchaseParty.ToDate = Date.ToDotNet(txtToDate.Text);
            }
            else
            {
                m_PurchaseReport.FromDate = null;
                m_PurchaseParty.FromDate = null;
                m_PurchaseReport.ToDate = null;
                m_PurchaseParty.ToDate = null;
             
            }
            //for product
            
            m_PurchaseReport.IsProductwise = rdProduct.Checked;
            //for single product
            ListItem LiProductSingleID = new ListItem();
            LiProductSingleID = (ListItem)cboProductSingle.SelectedItem;
            if (rdProductSingle.Checked)
            {
                m_PurchaseReport.ProductSingleID = Convert.ToInt32(LiProductSingleID.ID);
                m_PurchaseParty.ProductID = Convert.ToInt32(LiProductSingleID.ID); 
            }
            else
            {
                m_PurchaseReport.ProductSingleID = null;
                m_PurchaseParty.ProductID = null;
            }
            //for product Group
            ListItem LiProductGroupID = new ListItem();
            LiProductGroupID = (ListItem)cboProductGroup.SelectedItem;
            if (rdProductGroup.Checked && rdProduct.Checked)
                m_PurchaseReport.ProductGroupID = Convert.ToInt32(LiProductGroupID.ID);
            else
                m_PurchaseReport.ProductGroupID = null;

            //for all product
            m_PurchaseReport.IsProductAll = rdProductAll.Checked;

            //For party
            m_PurchaseReport.IsPartywise = rdParty.Checked;
            //for Single Party
            ListItem LiPartySingleID = new ListItem();
            LiPartySingleID = (ListItem)cboPartySingle.SelectedItem;
            if (rdpartySingle.Checked)
            {
                m_PurchaseReport.PartySingleID = Convert.ToInt32(LiPartySingleID.ID);
                m_PurchaseParty.PartyID = Convert.ToInt32(LiPartySingleID.ID);
            }
            else
            {
                m_PurchaseReport.PartySingleID = null;
                m_PurchaseParty.PartyID = null;
            }

            //for Party group
            ListItem LiPartyGroupID = new ListItem();
            LiPartyGroupID = (ListItem)cboPartyGroup.SelectedItem;
            if (rdPartyGroup.Checked)
                m_PurchaseReport.PartyGroupID = Convert.ToInt32(LiPartyGroupID.ID);
            else
                m_PurchaseReport.PartyGroupID = null;
            //For All Party
            m_PurchaseReport.IsPartyAll = rdPartyAll.Checked;

           //for Depot
            ListItem LiDepotID = new ListItem();
            LiDepotID = (ListItem)cboDeotwise.SelectedItem;
            if (chkDepot.Checked)
            {
                m_PurchaseReport.DepotID = Convert.ToInt32(LiDepotID.ID);
                m_PurchaseParty.DepotID = Convert.ToInt32(LiDepotID.ID);
            }
            else
            {
                m_PurchaseReport.DepotID = null;
                m_PurchaseParty.DepotID = null;
            }

            //for Project 
            ListItem LiProjectID = new ListItem();
            LiProjectID = (ListItem)cboProjectName.SelectedItem;
            if (chkProject.Checked)
            {
                m_PurchaseReport.ProjectID = Convert.ToInt32(LiProjectID.ID);
                m_PurchaseParty.ProjectID = Convert.ToInt32(LiProjectID.ID);
            }
            else
            {
                m_PurchaseReport.ProjectID = null;
                m_PurchaseParty.ProjectID = null;
            }


                
            //For Purchase Account

            ListItem liPurchaseLedgerID = new ListItem();
            liPurchaseLedgerID = (ListItem)cboPurchaseAccount.SelectedItem;
            if (chkPurchaseAccount.Checked)
            {
                if (cboPurchaseAccount.SelectedIndex == -1)
                {

                    MessageBox.Show("Please Select Purchase Account");
                
                }
                m_PurchaseReport.PurchaseLedgerID = Convert.ToInt32(liPurchaseLedgerID.ID);
                m_PurchaseParty.PurchaseLedgerID = Convert.ToInt32(liPurchaseLedgerID.ID);
            }
            else
            {
                m_PurchaseReport.PurchaseLedgerID = null;
                m_PurchaseParty.PurchaseLedgerID = null;
            }
            m_PurchaseReport.AccClassID = AccClassID;
            m_PurchaseParty.AccClassID = AccClassID;

            if (m_PurchaseReport.PartySingleID > 0 || m_PurchaseReport.ProductSingleID > 0)
            {
                if (rdProduct.Checked)
                    m_PurchaseParty.ReportType = InventoryReportType.ProductWise.ToString();
                else if (rdParty.Checked)
                    m_PurchaseParty.ReportType = InventoryReportType.PartyWise.ToString();

                frmProductPartyTransaction frm1 = new frmProductPartyTransaction(m_PurchaseParty);
                frm1.Show();
            }
            else
            {
                frmPurchaseReport frm = new frmPurchaseReport(m_PurchaseReport);
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

        private void frmPurchaseReportSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void MakeVisible(GroupBox grp)
        {
            grpProduct.Visible = false;
            grpParty.Visible = false;
            grp.Visible = true;
        }

        private void rdProductDepot_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void chkPurchaseAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPurchaseAccount.Checked)
                cboPurchaseAccount.Enabled = true;
            else
                cboPurchaseAccount.Enabled = false;
        }

        private void chkProductDepot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepot.Checked)
                cboDeotwise.Enabled = true;
            else
                cboDeotwise.Enabled = false;
        }

        private void rdPartyGroup_CheckedChanged_1(object sender, EventArgs e)
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

        private void rdpartySingle_CheckedChanged(object sender, EventArgs e)
        {
            if (rdpartySingle.Checked== true)
            {
                cboPartySingle.Enabled = true;
                rdProductSingle.Checked = false;
            }
            else
            {
                cboPartySingle.Enabled = false;
            }
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

        private void chkVoucherwise_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVoucherwise.Checked)
                cboVoucherwise.Enabled = true;
            else
                cboVoucherwise.Enabled = false;
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
                grpDate.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                btnFromDate.Enabled = true;
                btnToDate.Enabled = true;
            }
            else
            {
                grpDate.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                btnFromDate.Enabled = false;
                btnToDate.Enabled = false;
            }
        }

        private void rdProductAll_CheckedChanged(object sender, EventArgs e)
        {

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
                Code = "PR_DATE";
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
                Code = "PR_ACCOUNTING_CLASS";
                Value = Global.GlobalAccClassID.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Project
                Code = "PR_PROJECT";
                Value = cboProjectName.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For PURCHASE Account
                Code = "PR_PURCHASEACCOUNT";
                Value = cboPurchaseAccount.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Depot Account
                Code = "PR_DEPOT";
                Value = cboDeotwise.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Single Product Account
                Code = "PR_SINGLEPRODUCT";
                Value = cboProductSingle.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For  Product Group
                Code = "PR_PRODUCTGROUP";
                Value = cboProductGroup.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For  single party
                Code = "PR_SINGLEPARTY";
                Value = cboPartySingle.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For   party group
                Code = "PR_PARTYGROUP";
                Value = cboPartyGroup.SelectedIndex.ToString();
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Product
                Code = "PR_PRODUCT";
                if (rdProduct.Checked)
                    Value = "1";
                else
                    Value = "0";
               
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Party
                Code = "PR_PARTY";
                if (rdParty.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For all Party
                Code = "PR_RALLPARTY";
                if (rdPartyAll.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Single Party
                Code = "PR_RSINGLEPARTY";
                if (rdpartySingle.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For party group
                Code = "PR_RPARTYGROUP";
                if (rdPartyGroup.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For all Product
                Code = "PR_RALLPPRODUCT";
                if (rdProductAll.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Single Product   
                Code = "PR_RSINGLEPRODUCT";
                if (rdProductSingle.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Product Group
                Code = "PR_RPRODUCTGROUP";
                if (rdProductGroup.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For CDepot
                Code = "PR_CDEPOT";
                if (chkDepot.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For Cpurchase account
                Code = "PR_CPURCHASEACCOUNT";
                if (chkPurchaseAccount.Checked)
                    Value = "1";
                else
                    Value = "0";
                tw.WriteStartElement("RP");
                tw.WriteElementString("CODE", Code.ToString());
                tw.WriteElementString("VALUE", Value.ToString());
                tw.WriteEndElement();
                //For CProject
                Code = "PR_CPROJECT";
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
            
            //string Code, Value;
            //int PreferenceID, UserID;
            //UserID = User.CurrUserID;
            //DataTable dt = m_ReportPreference.GetPreferenceInfo(UserID, "PURCHASE_REPORT");
            //if (chkDateRange.Checked)
            //{
            //    Code = "PR_DATE";
            //    Value = "1";
            //    PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //    if (dt.Rows.Count < 1)
            //    {
            //        m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //    }
            //    else
            //    {
            //        m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //    }
            //}
            //else
            //{
            //    Code = "PR_DATE";
            //    Value = "0";
            //    PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //    if (dt.Rows.Count < 1)
            //    {
            //        m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //    }
            //    else
            //    {
            //        m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //    }
            //}

            ////For Accounting Class
            //Code = "PR_ACCOUNTING_CLASS";
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
            //Code = "PR_PROJECT";
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
            ////For PURCHASE Account
            //Code = "PR_PURCHASEACCOUNT";
            //Value = cboPurchaseAccount.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Depot Account
            //Code = "PR_DEPOT";
            //Value = cboDeotwise.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
          
            // //For Single Product Account
            //Code = "PR_SINGLEPRODUCT";
            //Value = cboProductSingle.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For  Product Group
            //Code = "PR_PRODUCTGROUP";
            //Value = cboProductGroup.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For  single party
            //Code = "PR_SINGLEPARTY";
            //Value = cboPartySingle.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For   party group
            //Code = "PR_PARTYGROUP";
            //Value = cboPartyGroup.SelectedIndex.ToString();
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Product
            //Code = "PR_PRODUCT";
            //if (rdProduct.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Party
            //Code = "PR_PARTY";
            //if (rdParty.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For all Party
            //Code = "PR_RALLPARTY";
            //if (rdPartyAll.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Single Party
            //Code = "PR_RSINGLEPARTY";
            //if (rdpartySingle.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For party group
            //Code = "PR_RPARTYGROUP";
            //if (rdPartyGroup.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For all Product
            //Code = "PR_RALLPPRODUCT";
            //if (rdProductAll.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Single Product   
            //Code = "PR_RSINGLEPRODUCT";
            //if (rdProductSingle.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Product Group
            //Code = "PR_RPRODUCTGROUP";
            //if (rdProductGroup.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For CDepot
            //Code = "PR_CDEPOT";
            //if (chkDepot.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For Cpurchase account
            //Code = "PR_CPURCHASEACCOUNT";
            //if (chkPurchaseAccount.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
            //PreferenceID = Convert.ToInt32(m_ReportPreference.GetUserPreference(Code));
            //if (dt.Rows.Count < 1)
            //{
            //    m_ReportPreference.SetUserPreference(UserID, PreferenceID, Value);
            //}
            //else
            //{
            //    m_ReportPreference.UpdateUserPreference(UserID, PreferenceID, Value);
            //}
            ////For CProject
            //Code = "PR_CPROJECT";
            //if (chkProject.Checked)
            //    Value = "1";
            //else
            //    Value = "0";
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
