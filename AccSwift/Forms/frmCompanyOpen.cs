using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using RegistryManager;
using DBLogic;
using Inventory;


namespace AccSwift
{
    public partial class frmCompanyOpen : Form
    {
        private SourceGrid.Cells.Controllers.CustomEvents dblClick;//Double click for DebitNote Register
        private int CompanyRowsCount;

        public frmCompanyOpen()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Initializes a list view and prepare it for the display for company enlistment
        /// </summary>
        public void InitializeListView()
        {
            this.lstCompany.View = View.Details;
            this.lstCompany.GridLines = true;
            this.lstCompany.FullRowSelect = true;
            this.lstCompany.HideSelection = false;
            
            this.lstCompany.Columns.Add("Company Name",400);
            this.lstCompany.Columns.Add("Code",80);
        }

        private void frmCompanyOpen_Load(object sender, EventArgs e)
        {

            //Prepare Listview 
            InitializeListView();


            //List Companies from registry
            foreach (string CompanyKey in RegManager.ListCompanyKeys())
            {
                ListViewItem lvItem = new ListViewItem(RegManager.GetCompanyName(CompanyKey),0);
                lvItem.SubItems.Add(RegManager.GetCompanyCode(CompanyKey));
                lvItem.SubItems.Add(CompanyKey);
                lvItem.Tag = RegManager.GetCompanyDefaultDB(CompanyKey);
                lstCompany.Items.Add(lvItem);
            }

            //If there is only one company, load it directly

            //Focus on Listview
            lstCompany.Select();
            //Select the first company by default
            if (lstCompany.Items.Count > 0)
                lstCompany.Items[0].Selected = true;


        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCompanyOpen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnToggleView_Click(object sender, EventArgs e)
        {
           lstCompany.View = lstCompany.View == View.LargeIcon ? View.Details : View.LargeIcon;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Global.CreateCompany = false;
            try
            {
               // MessageBox.Show("First Message");
                string Database = lstCompany.SelectedItems[0].Tag.ToString();
                Global.Company_Code = lstCompany.SelectedItems[0].SubItems[1].Text;
                Global.Company_RegCode = lstCompany.SelectedItems[0].SubItems[2].Text;
                RegManager.DataBase = Database;

                //Try connecting using the new connection
                SqlDb m_db = new SqlDb();
                //In case of SQL Server connection
                m_db.ServerName = RegManager.ServerName;
                m_db.DbName = RegManager.DataBase;
                m_db.UserName = RegManager.DBUser;
                m_db.Password = RegManager.DBPassword;
               // MessageBox.Show("Second Message");
                if (m_db.Connect())
                {
                    RegManager.ServerName = m_db.ServerName;
                    RegManager.DataBase = m_db.DbName;
                    RegManager.DBUser=m_db.UserName;
                    RegManager.DBPassword=m_db.Password;

                    Global.m_db.cn = m_db.cn;

                    
                    //Global.m_db.Connect();

                    Global.Default_Language=LangMgr.LangToLangType(Settings.GetSettings("DEFAULT_LANGUAGE"));

                    //Global.m_db.cn = m_db.cn;

                    //Success
                    //Global.m_db.cn = m_db.cn;
                    ////Global.m_db.cn = m_db.cn;

                    //Global.m_db.Connect();
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();

                   // MessageBox.Show("Third Message");

                }
                else //Connection failure
                {
                    Global.MsgError("There was an error selecting the company, please select another company and try again. If it doesnt resolve, try closing and reopening the application again!");
                }
                //Connection to the previous Years DataBase
                //Get Previous Year Database
                string PreviousYearDB = Settings.GetSettings("PREV_YR_DB");
                Global.PreviousYearDbConnection = PreviousYearDB;
                if (PreviousYearDB != "AccSwift_v1.10" && PreviousYearDB != "")//only if there is any database name in the setting, "AccSwift_v1.10" is default value
                {
                    SqlDb m_dbPY = new SqlDb();
                    //In case of SQL Server connection
                    m_dbPY.ServerName = RegManager.ServerName;
                    m_dbPY.DbName = PreviousYearDB;
                    m_dbPY.UserName = RegManager.DBUser;
                    m_dbPY.Password = RegManager.DBPassword;

                    //this fxn takes a bit time
                    if (m_dbPY.Connect())
                    {
                        Global.m_dbPY.cn = m_dbPY.cn;

                        Global.Default_Language = LangMgr.LangToLangType(Settings.GetSettings("DEFAULT_LANGUAGE"));

                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                    }
                }
               // MessageBox.Show("First Message");

            }
            catch (Exception ex)
            {
                Global.Msg("Please select the proper company and try again");
            }
        }

        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            Global.CreateCompany = true;
            frmNewCompany frm = new frmNewCompany(this);
            frm.ShowDialog();
            
        }

        private void lstCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void lstCompany_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnOpen_Click(sender, e);
        }

        private void frmCompanyOpen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Global.CreateCompany == false)
            {
                
                try
                {
                    //Try connecting using the new connection
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
                        this.Dispose();
                    }
                    else //Connection failure
                    {
                        Global.MsgError("There was an error selecting the company, please select another company and try again. If it doesnt resolve, try closing and reopening the application again!");
                    }

                }
                catch (Exception ex)
                {
                    Global.Msg("Please select the proper company and try again");
                }

            }
        }
    }
}
