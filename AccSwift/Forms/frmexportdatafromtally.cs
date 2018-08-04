using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AccSwift.Forms
{
    public partial class frmexportdatafromtally : Form
    {
        //RTSlink DLL functions declaration  
        [DllImport("RTSLink.dll")]
        extern static int Open();

        [DllImport("RTSLink.dll")]
        extern static int Send(string request);

        [DllImport("RTSLink.dll")]
        extern static string ResponseText();
        public frmexportdatafromtally()
        {
            InitializeComponent();
        }

        private void btnexportfromtally_Click(object sender, EventArgs e)
        {
            string strResponse = null;
            string strXMLfile = "";
            int n = 0;
             //strXMLfile = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER>
             //            <BODY><IMPORTDATA>
             //            <REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC>
             //            <REQUESTDATA><TALLYMESSAGE xmlns:UDF='TallyUDF'>
             //            <GROUP NAME='My Debtors' ACTION='Create'>
             //            <NAME.LIST><NAME>My Debtors</NAME></NAME.LIST>
             //            <PARENT>Sundry Debtors</PARENT>
             //            <ISSUBLEDGER>No</ISSUBLEDGER>
             //            <ISBILLWISEON>No</ISBILLWISEON>
             //             <ISCOSTCENTRESON>No</ISCOSTCENTRESON>
             //             </GROUP>
             //             </TALLYMESSAGE></REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>"; 



            strXMLfile = " <ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA><TALLYMESSAGE xmlns:UDF='TallyUDF'><GROUP NAME='My Debtors' ACTION='Create'><NAME.LIST><NAME>My Debtors</NAME></NAME.LIST><PARENT>Sundry Debtors</PARENT><ISSUBLEDGER>No</ISSUBLEDGER><ISBILLWISEON>No</ISBILLWISEON><ISCOSTCENTRESON>No</ISCOSTCENTRESON></GROUP></TALLYMESSAGE></REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

            //Invoke RTSlink DLL Open() function to check whether Tally is running or not ?
            //Zero means successful
            n = Open();
            if (n == 0)
            {
                //Invoke Send() function of RTSlink DLL
                //Send request to Write data to Tally (Create a Group named "My Debtors")
                if (Send(strXMLfile) == 0)
                {

                    //Invoke ResponseText() of RTSlink DLL
                    strResponse = ResponseText();
                    MessageBox.Show(strResponse, "Group Created Successfully");
                }
                else
                {
                    MessageBox.Show("Send() function failed", "Send() error");
                }

            }
            else
            {
                MessageBox.Show("Cannot connect to Tally", "Tally not running");
            }
        }

        private void frmexportdatafromtally_Load(object sender, EventArgs e)
        {

        }
    }
}
