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
using Inventory.View.Reports;

namespace Inventory
{
    public partial class frmStockAgingSetting : Form, IfrmSelectAccClassID, IfrmDateConverter
    {
        ArrayList AccClassID = new ArrayList();
        private string Prefix = "";
        private bool IsAtTheEndDate = false;
        ReportPreference m_ReportPreference;
        public frmStockAgingSetting()
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
            if (IsAtTheEndDate)//If form date is selected
            {
                DateTextBox.Text = Date.ToSystem(DotNetDate);
            }

        }

        private void frmStockAgingSetting_Load(object sender, EventArgs e)
        {
            LoadMonths();
            m_ReportPreference = new ReportPreference();
            LoadComboboxProject(cboProjectName, 0);
            
            IsAtTheEndDate = true;
            DateTextBox.Mask = Date.FormatToMask();
            DateTextBox.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition

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
                        
                AccClassID.Add(Global.GlobalAccClassID.ToString());
                //just for test
                ArrayList arrchildAccClassIds = new ArrayList();
                AccountClass.GetChildIDs(Global.GlobalAccClassID, ref arrchildAccClassIds);//If nothing is selected then bydefault,root classid is selected and its id is zero
                foreach (object obj in arrchildAccClassIds)
                {
                    int i = (int)obj;
                    AccClassID.Add(i.ToString());
                }

                rdProductAll.Checked = true;
                cboProductGroup.Enabled = false;
                cboProductSingle.Enabled = false;

                chkDepot.Checked = true;
                cmboDepot.Enabled = false;
            
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

        private void btnDate_Click(object sender, EventArgs e)
        {
            DateTime dtDate = Date.ToDotNet(DateTextBox.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
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

        private void chkDepot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepot.Checked)
            {
                cmboDepot.Enabled = false;
            }
            else
            {
                cmboDepot.Enabled = true;
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ListItem liProductId = new ListItem();  
            int productID=0;
            ListItem liProductGroupId = new ListItem();
            int productGroupID=0;
            ListItem liDepotId = new ListItem();
            int DepotId=0;
            ListItem liProjectId = new ListItem();
            int ProjectId=0;
            BusinessLogic.Inventory.Reports.StockAgingSetting stockAging = new BusinessLogic.Inventory.Reports.StockAgingSetting();
            if (rdProductAll.Checked)
            {
                stockAging.allProduct = true;
            }
            if (rdProductSingle.Checked)
            {
                stockAging.singleProduct = true;
                liProductId = (ListItem)cboProductSingle.SelectedItem;
                productID = liProductId.ID;
                stockAging.ProductID = productID;

            }
            if (rdProductGroup.Checked)
            {
                stockAging.productGroup = true;
                liProductGroupId = (ListItem)cboProductGroup.SelectedItem;
                productGroupID = liProductGroupId.ID;
                stockAging.ProductGroupID = productGroupID;
            }
            if (chkDepot.Checked)
            {
                stockAging.depot = true;
                liDepotId = (ListItem)cmboDepot.SelectedItem;
                DepotId = liDepotId.ID;
                stockAging.DepotId = DepotId;
            }
            liProjectId = (ListItem)cboProjectName.SelectedItem;
            ProjectId = liProjectId.ID;
            stockAging.ProjectID = ProjectId;
            DateTextBox.Mask = Date.FormatToMask();
            stockAging.AtTheEndDate = Date.ToDotNet(DateTextBox.Text);
            stockAging.AccClassID = AccClassID;
            frmStockAgeing frmstock = new frmStockAgeing(stockAging);
            frmstock.Show();
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
                  //  StartDate = Date.NepToEng(FYYear, SelItem.ID, 1);
                    //Get Last Day
                    DataTable LastDay = Date.LastDayofMonthNep(FYYear, SelItem.ID);
                    FinalDate = Date.NepToEng(FYYear, SelItem.ID, Convert.ToInt16(LastDay.Rows[0][0]));
                }
                else
                {
                   // StartDate = Date.NepToEng(FYYear, SelItem.ID, 1);
                    FinalDate = new DateTime(FYYear, SelItem.ID, DateTime.DaysInMonth(FYYear, SelItem.ID));
                }
               // DateTextBox.Text = Date.ToSystem(StartDate);
                DateTextBox.Text = Date.ToSystem(FinalDate);
            }
            catch
            {
                //Ignore
            }
        }
    }
}
