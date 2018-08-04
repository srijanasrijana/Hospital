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
using Common;

namespace Inventory.Forms.Accounting.Reports
{
    public partial class frmMBalanceSheetSettings : Form, IfrmSelectAccClassID, IfrmDateConverter
    {
        private string Prefix = "";
        ArrayList AccClassID = new ArrayList();
        DataTable dtAccClassID = new DataTable();
        public frmMBalanceSheetSettings()
        {
            InitializeComponent();
        }

        private void frmMBalanceSheetSettings_Load(object sender, EventArgs e)
        {
            LoadComboboxProject(cboProjectName, 0);
            LoadMonths();
            txtToDate.Mask = Date.FormatToMask();
            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());// Displaying Current DateTime at FormLoad Condition
            AccClassID.Add(Global.GlobalAccClassID.ToString());
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
        private void LoadMonths()
        {
            //Check Fiscal year(By default in English)
            CompanyDetails CompDetails = new CompanyDetails();
            CompDetails = CompanyInfo.GetInfo();
            //get first month from start fiscal date
            // DateTime start = new DateTime();
            //if(CompDetails.FYFrom != null)                      
            DateTime start = Convert.ToDateTime(CompDetails.FYFrom); //English fiscal year

            ListItem[] ListDate = new ListItem[12];
            for (int month = 0; month < 12; month++)
            {
                ListDate[month] = new ListItem();
                ListDate[month].ID = month + 1;
                ListDate[month].Value = Date.GetMonthList((Date.DateType)Date.DefaultDate, Language.LanguageType.English)[month + 1];

            }
            // DateTime FYStartDate = new DateTime();
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

        private void btnSelectAccClass_Click(object sender, EventArgs e)
        {
            BalanceSheetSettings BSS = new BalanceSheetSettings();
            try
            {
                BSS.AccClassID = AccClassID;
            }
            catch
            {
                //Ignore 
            }
            frmSelectAccClass frm = new frmSelectAccClass(this, BSS.AccClassID);
            if (!frm.IsDisposed)
                frm.ShowDialog();
        }
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

        private void btnToday_Click(object sender, EventArgs e)
        {
            //Get Todays date in DateTime
            DateTime Today = Date.GetServerDate();
            //Convert it to the System type
            string SysDate = Date.ToSystem(Today);
            //Put the date in mask edit box
            txtToDate.Text = SysDate;

            //Blank the month combo
            cboMonths.SelectedIndex = -1;
            cboMonths.Text = "";
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("BALANCE_SHEET");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to View. Please contact your administrator for permission.");
                return;
            }
            BalanceSheetSettings m_BSShow = new BalanceSheetSettings();//Dynamic memory allocation of an object
            //txtFromDate.Mask = Date.FormatToMask();//Masking the datetime in required formate 

            txtToDate.Mask = Date.FormatToMask();

            //m_BSShow.FromDate = Date.ToDotNet(txtFromDate.Text);// Converting  datetime came via controls into DonNet datetime formate 
            m_BSShow.FromDate = Date.ToDotNet(Date.ToSystem(Global.Fiscal_Year_Start));// Converting  datetime came via controls into DonNet datetime formate 
            m_BSShow.ToDate = Date.ToDotNet(txtToDate.Text);

            //Get the GroupID of the Asset and Liabilities Group Account


            m_BSShow.AccClassID = AccClassID;
            m_BSShow.ShowZeroBalance = chkShowZeroBal.Checked;
            m_BSShow.ShowSecondLevelDtl = chkShowSecLevGrpDet.Checked;

            ListItem liProjectInfo = new ListItem();
            liProjectInfo = (ListItem)cboProjectName.SelectedItem;
            m_BSShow.ProjectID = Convert.ToInt32(liProjectInfo.ID);
            frmMBalanceSheet frmmbal = new frmMBalanceSheet(m_BSShow);
            frmmbal.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
