using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using BusinessLogic;
using RegistryManager;


namespace AccSwift
{
    public partial class frmDBConnect : Form
    {        
        SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
        public string RootPath = "";
        public Boolean connSuccess = false;
        public enum Method { Startup, Menu };
        private Method _method;

        public frmDBConnect()
        {
            InitializeComponent();
        }

        public frmDBConnect(Method _method)
        {
            this._method = _method;
            InitializeComponent();
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }



        /////// <summary>
        /////// The main entry point for the application.
        /////// </summary>
        ////[STAThread]
        ////static void Main()
        ////{
        ////    Application.Run(new Form1());
        ////}      

        private void frmDBConnect_Load(object sender, EventArgs e)
        {
            try
            {
                //Initially load the defaults from the registry
                cbServer.Text = RegistryManager.RegManager.ServerName;
                txtUser.Text = RegistryManager.RegManager.DBUser;
                txtPassword.Text = RegistryManager.RegManager.DBPassword;
                cbDataBase.Text = RegistryManager.RegManager.DataBase;

                SqlInstances();

                SqlDatabaseNames();
            }
            catch
            { }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SqlInstances();
            
            SqlDatabaseNames();

        }

        public void SqlInstances()
        {
        
            Cursor.Current = Cursors.WaitCursor;
            try
                {
                    cbServer.Items.Clear();
                    DataTable dtsqlSources = new DataTable();
                    dtsqlSources = SqlDataSourceEnumerator.Instance.GetDataSources();   
                    foreach(DataRow drsqlSources in dtsqlSources.Rows)
                    {               
                        string datasource= drsqlSources["ServerName"].ToString();
                        if( drsqlSources["InstanceName"] != null)
                            {
                            //if Not drsqlSources("InstanceName") Is DBNull.Value Then
                                datasource += String.Format("\\{0}", drsqlSources["InstanceName"]);
                            }
                        cbServer.Items.Add(datasource);
                    }
                }
                catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }     
                Cursor.Current = Cursors.Default;   
        }

        public void  ContructConnection()
        {        
            conn.DataSource = cbServer.Text;          
            conn.IntegratedSecurity = true;
            conn.UserID = "";
            conn.Password = "";
            conn.InitialCatalog = "";

            if (rbAuthenticationSql.Checked)
            {
                conn.IntegratedSecurity = true;
                conn.UserID = txtUser.Text;
                conn.Password = txtPassword.Text;
            }
            if(cbDataBase.Text != "")
            {
                conn.InitialCatalog = "master";
            }   
         }


        public void SqlDatabaseNames()
        {

            if (txtPassword.Text == "" || txtUser.Text == "" || cbServer.Text == "")
                return;

            String conxString = "Data Source=" + cbServer.Text + "; uid = " + txtUser.Text + "; password = " + txtPassword.Text + "; ";

            try
            {
                using (SqlConnection sqlConx = new SqlConnection(conxString))
                {
                    sqlConx.Open();
                    DataTable tblDatabases = sqlConx.GetSchema("Databases");
                    sqlConx.Close();

                    cbDataBase.Items.Clear();
                    foreach (DataRow dr in tblDatabases.Rows)
                        cbDataBase.Items.Add(dr["database_name"].ToString());


                    Cursor.Current = Cursors.Default;

                }
            }
            catch
            { }
            
        }

        public void TestDB()
        {
            ContructConnection();
            try
            {
                SqlConnection objConn = new SqlConnection(conn.ConnectionString);               
                objConn.Open();
                connSuccess = true;
                objConn.Close();
                MessageBox.Show("Successful connection!!!");
            }
            catch (Exception ex)
            {
                connSuccess = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void cbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbServer.Items.Count == 0 )
            {
                SqlInstances();
            }

            SqlDatabaseNames();

            
        }

        private void cbDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void rbAuthenticationWin_CheckedChanged(object sender, EventArgs e)
        {
             txtUser.Enabled = false;
             txtPassword.Enabled = false;

        }

        private void rbAuthenticationSql_CheckedChanged(object sender, EventArgs e)
        {
             txtUser.Enabled = true;
             txtPassword.Enabled = true;

            
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TestDB();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_method == Method.Menu)
            {
                Global.Msg("You'll require to restart the application to apply the settings");
                
                #region Using Regedit

                RegManager.ServerName= cbServer.Text;
                RegManager.DataBase= cbDataBase.Text;
                RegManager.DBUser= txtUser.Text;
                RegManager.DBPassword=txtPassword.Text;

                #endregion

            }

        }    

        private void btnCancel_Click(object sender, EventArgs e)
        {


            this.Close();

            
        }

        public  string ConnectionString
        {        
            get {return conn.ConnectionString;}
             set{conn.ConnectionString = value;}         
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            SqlDatabaseNames();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            SqlDatabaseNames();
        }

        private void frmDBConnect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }



    }
}
