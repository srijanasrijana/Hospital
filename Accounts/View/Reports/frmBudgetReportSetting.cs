using BusinessLogic;
using BusinessLogic.Accounting;
using BusinessLogic.Accounting.Reports;
using Common;
using DateManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Accounts.View.Reports
{
    public partial class frmBudgetReportSetting : Form, IfrmSelectAccClassID
    {
        public frmBudgetReportSetting()
        {
            InitializeComponent();
        }
        private string Prefix = "";
        private bool IsFromDate = false;
        ArrayList AccClassID = new ArrayList();
        Budget bgt = new Budget();


        private void frmBudgetReportSetting_Load(object sender, EventArgs e)
        {
            LoadMonths();

            grpDate.Enabled = false;// Disabling the DateRange Groupbox at FormLoad condition

            //unuseable
            //txtToDate.Mask = Date.FormatToMask();
            //txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
            //txtFromDate.Mask = Date.FormatToMask();
            //txtFromDate.Text = Date.ToSystem(Global.Fiscal_Year_Start);

            LoadBudgets();
            LoadComboboxProject(cboProjectName, 0);
           

            //initially add root class and its child for condition if users do not select class for report
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
        private void LoadBudgets()
        {
            DataTable dt = bgt.GetBudgetIDNName();
            if (dt.Rows.Count > 0)
            {

                DataRow dr = dt.NewRow();
                dr["budgetName"] = "Select Budget Name";
                dt.Rows.InsertAt(dr, 0);

                cmbChooseBudget.DataSource = dt;
                cmbChooseBudget.ValueMember = "budgetID";
                cmbChooseBudget.DisplayMember = "budgetName";
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

        //load project combobox 
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
        private void btnDate_Click(object sender, EventArgs e)
        {
            IsFromDate = true;//this variable is used as flag to notify which date is selected to change the date converter...coz same funtion is used to change the date  
            DateTime dtDate = Date.ToDotNet(txtFromDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IsFromDate = false;
            DateTime dtDate = Date.ToDotNet(txtToDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
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

        
        

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (cmbChooseBudget.SelectedIndex > 0)
            {
                BudgetReportSetting M_BudgetReport = new BudgetReportSetting();
                M_BudgetReport.AccClassID = AccClassID;
                //get project id from combobox
                ListItem liProjectID = new ListItem();
                liProjectID = (ListItem)cboProjectName.SelectedItem;
                M_BudgetReport.ProjectID = Convert.ToInt32(liProjectID.ID);

                M_BudgetReport.BudgetName = cmbChooseBudget.Text;
                txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate 
                txtToDate.Mask = Date.FormatToMask();
                M_BudgetReport.FromDate = Date.ToDotNet(txtFromDate.Text);
                M_BudgetReport.ToDate = Date.ToDotNet(txtToDate.Text);
                if (cmbChooseBudget.SelectedIndex > 0)
                {
                    M_BudgetReport.BudgetID = Convert.ToInt32(cmbChooseBudget.SelectedValue);
                }

                frmBudgetReport br = new frmBudgetReport(M_BudgetReport);
                br.ShowDialog();
            }
            else
            {
                Global.Msg("Please select a Budget First");
            }
        }

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {
            //just for test
            BudgetReportSetting M_BudgetReport = new BudgetReportSetting();
            try
            {
                M_BudgetReport.AccClassID = AccClassID;
            }
            catch
            {
                //Ignore 
            }
            frmSelectAccClass frm = new frmSelectAccClass(this, M_BudgetReport.AccClassID);

            if (!frm.IsDisposed)
                frm.ShowDialog();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cmbChooseBudget_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbChooseBudget.SelectedIndex > 0)
                {
                    DataTable dtdate = bgt.getStartNEndDate(Convert.ToInt32(cmbChooseBudget.SelectedValue));
                    if (dtdate.Rows.Count > 0)
                    {
                        int styear = 0;
                        int stmonth = 0;
                        int stday = 0;
                        int enyear = 0;
                        int enmonth = 0;
                        int enday = 0;
                        DateTime stdate = Convert.ToDateTime(dtdate.Rows[0][0].ToString());
                        Date.EngToNep(stdate, ref styear, ref stmonth, ref stday);
                        txtFromDate.Mask = Date.FormatToMask();
                        txtFromDate.Text = styear.ToString().PadLeft(4, '0') + "/" + stmonth.ToString().PadLeft(2, '0') + "/" + stday.ToString().PadLeft(2, '0');
                        DateTime endate = Convert.ToDateTime(dtdate.Rows[0][1].ToString());
                        Date.EngToNep(endate, ref enyear, ref enmonth, ref enday);
                        txtToDate.Mask = Date.FormatToMask();
                        txtToDate.Text = enyear.ToString().PadLeft(4, '0') + "/" + enmonth.ToString().PadLeft(2, '0') + "/" + enday.ToString().PadLeft(2, '0');
                    }
                }
                else
                {
                    txtFromDate.Text = "";
                    txtToDate.Text = "";
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void txtFromDate_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

                 
    }
}
