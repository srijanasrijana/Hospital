using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using BusinessLogic;
using DBLogic;
using RegistryManager;


namespace AccSwift
{
    public partial class frmBackUpRestore : Form
    {
        public frmBackUpRestore()
        {
            InitializeComponent();
        }

        private void frmBackUpRestore_Load(object sender, EventArgs e)
        {
            cboDBName.Enabled = false;
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (rdRestore.Checked)
            {
                OpenFileDialog OpenFD = new OpenFileDialog();
                OpenFD.InitialDirectory = "D:";
                OpenFD.Title = "Select Databse..";
                openFD.Filter = "All files (*.*)|*.*|Backup Files(*.Bak)|*.Bak";
                if (openFD.ShowDialog() != DialogResult.Cancel)
                {
                    string FileToRestore = OpenFD.FileName;
                    txtFile.Text = openFD.FileName;
                }
            }

            if (rdBackup.Checked)
            {
                SaveFileDialog SaveFD = new SaveFileDialog();
                SaveFD.InitialDirectory = "D:";
                SaveFD.Title = "Select Databse..";
                SaveFD.Filter = "*.Bak|*.Bak";
                if (SaveFD.ShowDialog() != DialogResult.Cancel)
                {
                    string FileToRestore = SaveFD.FileName;
                    txtFile.Text = SaveFD.FileName;
                }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(rdBackup.Checked)
            {
                btnBackUpRestore.Text = "&Back-Up";
                lblFilePath.Text = "Backup To:";
                cboDBName.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdRestore.Checked)
            {
                btnBackUpRestore.Text = "&Restore";
                lblFilePath.Text = "Restore From:";
                cboDBName.Enabled = true;
            }

            cboDBName.Items.Clear();
            /* foreach (string DbName in DBLogic.SqlDb.GetDatabaseList(Global.m_db.ServerName, Global.m_db.UserName, Global.m_db.Password))
             {
                 cboDBName.Items.Add(DbName);
             }
             */
            DataTable dt = new DataTable();
            string strQuery = "select name from master.sys.databases";
            dt = Global.m_db.SelectQry(strQuery, "tblDatabase");
            foreach (DataRow dr in dt.Rows)
            {
                cboDBName.Items.Add(dr["name"]);
            }

        }

        private void btnBackUpRestore_Click(object sender, EventArgs e)
        {
            if (rdBackup.Checked)
            {
                try
                {
                      //if (SqlDb.BackupDatabase(SqlDb._DbName, txtFile.Text, Global.m_db.cn))
                      //{
                      //    Global.Msg("Database backup completed successfully!");
                      //}
                    
                   // string strQuery = "backup database [" + SqlDb._DbName + "] to disk ='" + txtFile.Text + "'";
                    string strQuery = "backup database [" + RegManager.DataBase + "] to disk ='" + txtFile.Text + "'";
                    Global.m_db.SelectQry(strQuery, "tblBackup");
                    Global.Msg ("Database backup completed successfully!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
                }
            }
            if (rdRestore.Checked)
            {

                try
                {
                    /* if (SqlDb.RestoreDatabase(cboDBName.Text, txtFile.Text, SqlDb._ServerName, SqlDb._UserName, SqlDb._Password))
                     {
                         Global.Msg("Database restore completed successfully!");
                     }
                     */
                    string strQuery = "use master restore database [" + cboDBName.Text + "] from disk ='" + txtFile.Text + "'";
                    Global.m_db.SelectQry(strQuery, "tblRestore");
                    Global.Msg("Database restore completed successfully!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    Global.MsgError(ex.Message);
                }

            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmBackUpRestore_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

      

    }

}

