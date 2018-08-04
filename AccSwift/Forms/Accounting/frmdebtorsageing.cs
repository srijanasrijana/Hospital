using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DateManager;
using BusinessLogic;
using BusinessLogic.Accounting;
using System.Collections;
using Common;

namespace Inventory.Forms.Accounting
{
    public partial class frmdebtorsageing : Form, IfrmDateConverter, IfrmSelectAccClassID
    {
        BusinessLogic.AccountGroup acc = new AccountGroup();
        private string Prefix = "";
        ArrayList AccClassID = new ArrayList();
        public frmdebtorsageing()
        {
            InitializeComponent();
        }

        private void frmdebtorsageing_Load(object sender, EventArgs e)
        {
            LoadMonths();
            txtToDate.Mask = Date.FormatToMask();
            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            loadDebtors(cmbdebtorsaccount);
            LoadComboboxProject(cboProjectName, 0);
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
        private void btnToday_Click(object sender, EventArgs e)
        {
            //Get Todays date in DateTime
            DateTime Today = Date.GetServerDate();
            //Convert it to the System type
            string SysDate = Date.ToSystem(Today);
            //Put the date in mask edit box
            txtToDate.Text = SysDate;
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            DateTime dtDate = Date.ToDotNet(txtToDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }
        public void DateConvert(DateTime DotNetDate)
        {
            
            txtToDate.Text = Date.ToSystem(DotNetDate);
        }

        private void chkshowalldebtors_CheckedChanged(object sender, EventArgs e)
        {
            if (chkshowalldebtors.Checked)
                cmbdebtorsaccount.Enabled = false;
            else
                cmbdebtorsaccount.Enabled = true;
        }
        private void loadDebtors(ComboBox combo)
        {
            combo.Items.Clear();
            DataTable dt = acc.getPartyByID();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                combo.Items.Add(new ListItem((int)dr["LedgerID"], dr["EngName"].ToString()));
            }
            combo.DisplayMember = "value";
            combo.ValueMember = "id";
        }

        private void chkshowvoucherbalance_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cmbdebtorsaccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbdebtorsaccount.SelectedIndex >= 0)
            {
                chkshowalldebtors.Checked = false;
            }
        }

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {
            DebtorsSettings DS = new DebtorsSettings();
            try
            {
                DS.AccClassID = AccClassID;
            }
            catch (Exception ex)
            {
                //Ignore
            }
            frmSelectAccClass frm = new frmSelectAccClass(this, DS.AccClassID);
            if (!frm.IsDisposed)
                frm.ShowDialog();
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

        private void button1_Click(object sender, EventArgs e)
        {
            DebtorsSettings m_DSShow = new DebtorsSettings();
            txtToDate.Mask = Date.FormatToMask();
            m_DSShow.ToDate = Date.ToDotNet(txtToDate.Text);
            if (rbtnageing.Checked)
                m_DSShow.Ageing = true;
            else if (rbtnbillwiseageing.Checked)
                m_DSShow.BillwiseAgeing = true;
            if (chkshowalldebtors.Checked)
            {
                m_DSShow.ShowAllDebtores = true;
                m_DSShow.DebtorsID = -1;
            }
            else
            {
                if (cmbdebtorsaccount.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Debtors Name");
                    return;
                }
                ListItem liDebtorID = new ListItem();
                liDebtorID = (ListItem)cmbdebtorsaccount.SelectedItem;
                m_DSShow.DebtorsID = liDebtorID.ID;
            }
            m_DSShow.FirstPeriod = Convert.ToInt32(txtfirstperiod.Text);
            m_DSShow.SecondPeriod = Convert.ToInt32(txtsecondperiod.Text);
            m_DSShow.ThirdPeriod = Convert.ToInt32(txtthirdperiod.Text);
            m_DSShow.FourthPeriod = Convert.ToInt32(txtfourthperiod.Text);
            m_DSShow.AccClassID = AccClassID;
            ListItem liProjectID = new ListItem();
            liProjectID = (ListItem)cboProjectName.SelectedItem;
            m_DSShow.ProjectID = Convert.ToInt32(liProjectID.ID);
            if(chkshowvoucherbalance.Checked)
                m_DSShow.ShowVoucherBalance = true;
            Forms.Accounting.Reports.frmdebtorsageing da = new Reports.frmdebtorsageing(m_DSShow);
            da.ShowDialog();

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
               // DateTime StartDate;
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
                txtToDate.Text = Date.ToSystem(FinalDate);
            }
            catch
            {
                //Ignore
            }
        }
    }
}
