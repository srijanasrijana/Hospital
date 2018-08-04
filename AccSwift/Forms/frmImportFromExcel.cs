using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using BusinessLogic;


namespace AccSwift.Forms
{
    public partial class frmImportFromExcel : Form
    {
        private System.Windows.Forms.OpenFileDialog dlgopenFile;
        public frmImportFromExcel()
        {
            InitializeComponent();
        }

        public void importdatafromexcel(string excelfilepath)
        {
            if (txtdatabasetable.Text=="")
            {
                MessageBox.Show("Please Enter Table Name");
                return;
            }
            //declare variables - edit these based on your particular situation
            string ssqltable = txtdatabasetable.Text;
            // make sure your sheet name is correct, here sheet name is sheet1, so you can change your sheet name if have different
           // string myexceldataquery = "select student,rollno,course from [sheet1$]";
            if (txtsheetname.Text == "")
            {
                MessageBox.Show("Please Enter Sheet Name");
                return;
            }
            //string myexceldataquery = "select * from [sheet1$]";
            string sheetname = txtsheetname.Text;
            //select ProductID,EngName,NepName,GroupID,ProductCode,ProductColor,DepotID,Remarks,UnitMaintenanceID,BuiltIn,IsActive,Created_By,Created_Date,BackColor,IsVatApplicable,IsInventoryApplicable from Inv.tblProduct
            string myexceldataquery = "select * from [" + sheetname + "$]";
           
            try
            {
                //create our connection strings
                //string sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + excelfilepath + ";extended properties=" + "\"excel 8.0;hdr=yes;\"";
                string sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + excelfilepath + ";extended properties=" + "\"excel 8.0;hdr=yes;\"";
               // string ssqlconnectionstring = "server=mydatabaseservername;userid=dbuserid;password=dbuserpassword;database=databasename;connection reset=false";
                //execute a query to erase any previous data from our destination table
                string sclearsql = "delete from " + ssqltable;
                //sqlconnection sqlconn = new sqlconnection(ssqlconnectionstring);
                //sqlcommand sqlcmd = new sqlcommand(sclearsql, sqlconn);
                //sqlconn.open();
                //sqlcmd.executenonquery();
                //sqlconn.close();
                //series of commands to bulk copy data from the excel file into our sql table
                OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
                OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);
                oledbconn.Open();
                OleDbDataReader dr = oledbcmd.ExecuteReader();
                // SqlBulkCopy bulkcopy = new SqlBulkCopy(ssqlconnectionstring);Global.m_db.cn
                Global.m_db.cn.Open();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(Global.m_db.cn);
                bulkcopy.DestinationTableName = ssqltable;
                while (dr.Read())
                {
                    bulkcopy.WriteToServer(dr);
                }

                oledbconn.Close();
                MessageBox.Show("Data Imported Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //handle exception
            }
}

        private void btnshowfile_Click(object sender, EventArgs e)
        {
            this.dlgopenFile = new System.Windows.Forms.OpenFileDialog();
            dlgopenFile.ShowDialog();
            txtAttachment.Text = dlgopenFile.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            importdatafromexcel(txtAttachment.Text);
        }
    }
}
