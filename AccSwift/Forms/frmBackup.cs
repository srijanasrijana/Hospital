using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using BusinessLogic;

namespace AccSwift
{
    public partial class frmBackup : Form
    {
        public frmBackup()
        {
            InitializeComponent();
        }

        private static Server srvSql;

        private void frmBackup_Load(object sender, EventArgs e)
        {
            
            // Create a DataTable where we enumerate the available servers
            DataTable dtServers = SmoApplication.EnumAvailableSqlServers(true);
            // If there are any servers at all
            if (dtServers.Rows.Count > 0)
            {
                // Loop through each server in the DataTable
                foreach (DataRow drServer in dtServers.Rows)
                {
                    // Add the name to the combobox
                    cmbServer.Items.Add(drServer["Name"]);
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
             // If a server was selected at all from the combobox
            if (cmbServer.SelectedItem != null && cmbServer.SelectedItem.ToString() != "")
            {
                // Create a new connection to the selected server name
                ServerConnection srvConn = new ServerConnection(cmbServer.SelectedItem.ToString());
                // Log in using SQL authentication instead of Windows authentication
                srvConn.LoginSecure = true;
                // Give the login username
                //srvConn.Login = txtUsername.Text;
                // Give the login password
                //srvConn.Password = txtPassword.Text;
                // Create a new SQL Server object using the connection we created
                srvSql = new Server(srvConn);
                // Loop through the databases list
                foreach (Database dbServer in srvSql.Databases)
                {
                    // Add database to combobox
                    cmbDatabase.Items.Add(dbServer.Name);
                }
            }
            else
            {
                // A server was not selected, show an error message
                MessageBox.Show("Please select a server first", "Server Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // If there was a SQL connection created
            if (srvSql != null)
            {
                // If the user has chosen a path where to save the backup file
                if (saveBackupDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create a new backup operation
                        Backup bkpDatabase = new Backup();
                        // Set the backup type to a database backup
                        bkpDatabase.Action = BackupActionType.Database;
                        // Set the database that we want to perform a backup on
                        bkpDatabase.Database = cmbDatabase.SelectedItem.ToString();

                        // Set the backup device to a file
                        BackupDeviceItem bkpDevice = new BackupDeviceItem(saveBackupDialog.FileName, DeviceType.File);
                        // Add the backup device to the backup
                        bkpDatabase.Devices.Add(bkpDevice);
                        // Perform the backup
                        bkpDatabase.SqlBackup(srvSql);
                    }
                    catch (Exception ex)
                    {
                        Global.MsgError(ex.Message);
                    }
                }
            }
            else
            {
                // There was no connection established; probably the Connect button was not clicked
                MessageBox.Show("A connection to a SQL server was not established.", "Not Connected to Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            // If there was a SQL connection created
            if (srvSql != null)
            {
                // If the user has chosen the file from which he wants the database to be restored
                if (openBackupDialog.ShowDialog() == DialogResult.OK)
                {
                    // Create a new database restore operation
                    Restore rstDatabase = new Restore();
                    // Set the restore type to a database restore
                    rstDatabase.Action = RestoreActionType.Database;
                    // Set the database that we want to perform the restore on
                    rstDatabase.Database = cmbDatabase.SelectedItem.ToString();

                    // Set the backup device from which we want to restore, to a file
                    BackupDeviceItem bkpDevice = new BackupDeviceItem(openBackupDialog.FileName, DeviceType.File);
                    // Add the backup device to the restore type
                    rstDatabase.Devices.Add(bkpDevice);
                    // If the database already exists, replace it
                    rstDatabase.ReplaceDatabase = true;
                    // Perform the restore
                    rstDatabase.SqlRestore(srvSql);
                }
            }
            else
            {
                // There was no connection established; probably the Connect button was not clicked
                MessageBox.Show("A connection to a SQL server was not established.", "Not Connected to Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}