using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using ErrorManager;
using System.IO;
using Inventory.CrystalReports;
using Inventory.DataSet;
using System.Data.SqlClient;
using System.Threading;
using Common;


namespace AccSwift.Forms
{
    public partial class frmAuditLog : Form, IfrmDateConverter
    {
        private bool IsFromDate = false;
        private bool IsToDate = false;
        
      
        public frmAuditLog()
        {
            InitializeComponent();
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnFromDate_Click(object sender, EventArgs e)
        {
            IsFromDate = true;
            IsToDate = false;
            DateTime dtDate = Date.ToDotNet(txtFromDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }

        private void btnToDate_Click(object sender, EventArgs e)
        {
            IsFromDate = false;
            IsToDate = true;
            DateTime dtDate = Date.ToDotNet(txtToDate.Text);
            frmDateConverter _frmDateConverter = new frmDateConverter(this, dtDate);
            _frmDateConverter.Show();
            _frmDateConverter.StartPosition = FormStartPosition.CenterParent;
        }
        public void DateConvert(DateTime DotNetDate)
        {
            if (IsFromDate)//If From date is selected
            {
                txtFromDate.Text = Date.ToSystem(DotNetDate);
            }
            else if (IsToDate)//IF TO date is selected
            {
                txtToDate.Text = Date.ToSystem(DotNetDate);
            }
        }

        private void frmAuditLog_Load(object sender, EventArgs e)
        {
            ListProject(cmbuser);
            txtFromDate.Mask = Date.FormatToMask();
            txtFromDate.Text = Date.ToSystem(new DateTime(2009, 01, 24));         
            txtToDate.Mask = Date.FormatToMask();
            txtToDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
        }

        private void chkboxallusers_CheckedChanged(object sender, EventArgs e)
        {
            if (chkallusers.Checked == true)
            {
                cmbuser.SelectedIndex = -1;
                cmbuser.Enabled = false;
            }
            else
            {
                cmbuser.SelectedIndex = 0;
                cmbuser.Enabled = true;
            }
        }

        private void ListProject(ComboBox ComboBoxControl)
        {
            #region Language Management
            ComboBoxControl.Font = LangMgr.GetFont();
            string LangField = "English";
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

            ComboBoxControl.Items.Clear();

           // BusinessLogic.AuditLog al = new AuditLog();
            DataTable dt = AuditLog.GetUserTable(-1);
           // DataTable dt = Project.GetProjectTable(-1);
            
            ComboBoxControl.Items.Add(new ListItem((0), "None"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ComboBoxControl.Items.Add(new ListItem((int)dr["UserID"], dr["UserName"].ToString()));
            }
            ComboBoxControl.SelectedIndex = 0;
            ComboBoxControl.DisplayMember = "value";
            ComboBoxControl.ValueMember = "id";
        }

        private void btnShow_Click(object sender, EventArgs e)
        {

            string strTransQuery = " ";
            string username;
            DataTable dt = new DataTable();
            DateTime FromDate =Date.ToDotNet(txtFromDate.Text);
            DateTime ToDate =Date.ToDotNet( txtToDate.Text);

            string journal = chkjournal.Checked == true ? "JRNL" : " ";
            string bankreceipt = chkbankreceipt.Checked==true?"BANK_RCPT":" ";
            string bankpayment = chkbankpayment.Checked == true ? "BANK_PMNT" : " ";
            string cashreceipt = chkcashreceipt.Checked == true ? "CASH_RCPT" : " ";
            string cahspayment = chkcashpayment.Checked == true ? "CASH_PMNT" : " ";
            string bankreconciliation =chkbankreconciliation.Checked == true ? "BRECON" : " ";
            string contra = chkcontra.Checked == true ? "CONTRA" : " ";
            string debitnote =chkdebitnote.Checked == true ? "DNOTE" : " ";
            string creditnote = chkcreditnote.Checked == true ? "CNOTE" : " ";
            string purchasereturn = chkpurchasereturn.Checked == true ? "PRTN" : " ";
            string purchaseinvoice = chkpurchaseinvoice.Checked == true ? "PINV" : " ";
            string purchaseorder = chkpurchaseorder.Checked == true ? "PORD" : " ";
            string salesreturn = chksalesreturn.Checked == true ? "SRTN" : " ";
            string salesinvoice = chksalesinvoice.Checked== true ? "SINV" : " ";
            string salesorder = chksalesorder.Checked == true ? "SORD" : " ";
            string stocktransfer = chkstocktransfer.Checked == true ? "STRANS" : " ";
            string damageitems = chkdamageitems.Checked== true ? "DAMAGE" : " ";
            string bulkvoucherposting = chkbulkvoucherposting.Checked == true ? "BULKP" : " ";
            string accountclass = chkaccountclass.Checked== true ? "ACLASS" : " ";
            string accountgroup = chkaccountgroup.Checked == true ? "AGROUP" : " ";
            string ledger =chkledger.Checked== true ? "LEDGER" : " ";
            string slab = chkslab.Checked == true ? "SLAB" : " ";
            string service =chkservice.Checked == true ? "SERVICE" : " ";
            string product = chkproduct.Checked == true ? "PRODUCT" : " ";
            string productgroup = chkproductgroup.Checked == true ? "PGROUP" : " ";
            string voucherconfig = chkvoucherconfiguration.Checked == true ? "VCONFIG" : " ";
            string ledgerconfig = chkledgerconfiguration.Checked== true ? "LCONFIG" : " ";
            string depot =chkdepot.Checked == true ? "DEPOT" : " ";
            strTransQuery = string.Concat( journal.Trim(), ',', bankreceipt.Trim() , ',',  bankpayment.Trim(), ',', cashreceipt.Trim(), ',', cahspayment.Trim() , ',',  bankreconciliation.Trim() , ',',  contra.Trim() , ',',  debitnote.Trim() , ',', creditnote.Trim() , ',',  purchasereturn.Trim() , ',',  purchaseinvoice.Trim() ,
               ',',purchaseorder.Trim() , ',',  salesreturn.Trim(), ',', salesinvoice.Trim() , ',', salesorder.Trim(), ',', stocktransfer.Trim() , ',',  damageitems.Trim() , ',',  bulkvoucherposting.Trim() , ',', accountclass.Trim() , ',', accountgroup.Trim() , ',',  ledger.Trim() , ',',  slab.Trim(), ',', service.Trim() , ',',  product.Trim() , ',', productgroup.Trim() , ',', voucherconfig.Trim() ,
                ',', ledgerconfig.Trim() , ',',  depot.Trim() );

            frmProgress ProgressForm = new frmProgress();
            // Initialize the thread that will handle the background process
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                     ProgressForm.ShowDialog();
                }
            ));
            backgroundThread.Start();
            //Update the progressbar
            ProgressForm.UpdateProgress(20, "Initializing Report Viewer...");

            if (chkallusers.Checked == true)
            {
                dt=AuditLog.GetLogInfo(strTransQuery," ",FromDate,ToDate);
            }
            else
            {
                username = cmbuser.Text;
                dt = AuditLog.GetLogInfo(strTransQuery, username, FromDate, ToDate);
            }
            Report.frmAuditLogReport frmauditlogrpt = new Report.frmAuditLogReport(dt,FromDate,ToDate);

            ProgressForm.UpdateProgress(100, "Preparing report for display...");
            if (ProgressForm.InvokeRequired)
                ProgressForm.BeginInvoke(new Action(() => ProgressForm.Close()));

            ProgressForm.CloseForm();
            frmauditlogrpt.ShowDialog();

            //if (frmauditlogrpt.ShowDialog() == DialogResult.OK)
            //{
            //    ProgressForm.CloseForm();
            //}
           

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                chkaccountclass.Checked = chkaccountgroup.Checked = chkledger.Checked = chkslab.Checked = chkservice.Checked =
                    chkproduct.Checked = chkvoucherconfiguration.Checked = chkledgerconfiguration.Checked = chkproductgroup.Checked = chkdepot.Checked = true;
                chkjournal.Checked = chkbankreceipt.Checked = chkbankpayment.Checked = chkcashreceipt.Checked = chkcashpayment.Checked = chkbankreconciliation.Checked =
                    chkcontra.Checked = chkdebitnote.Checked = chkcreditnote.Checked = chkpurchasereturn.Checked = chkpurchaseorder.Checked = chkpurchaseinvoice.Checked =
                    chksalesreturn.Checked = chksalesorder.Checked = chksalesinvoice.Checked = chkstocktransfer.Checked = chkdamageitems.Checked = chkbulkvoucherposting.Checked = true;
            }
            else
            {
                chkaccountclass.Checked = chkaccountgroup.Checked = chkledger.Checked = chkslab.Checked = chkservice.Checked =
                    chkproduct.Checked = chkvoucherconfiguration.Checked = chkledgerconfiguration.Checked = chkproductgroup.Checked = chkdepot.Checked = false;
                chkjournal.Checked = chkbankreceipt.Checked = chkbankpayment.Checked = chkcashreceipt.Checked = chkcashpayment.Checked = chkbankreconciliation.Checked =
                    chkcontra.Checked = chkdebitnote.Checked = chkcreditnote.Checked = chkpurchasereturn.Checked = chkpurchaseorder.Checked = chkpurchaseinvoice.Checked =
                    chksalesreturn.Checked = chksalesorder.Checked = chksalesinvoice.Checked = chkstocktransfer.Checked = chkdamageitems.Checked = chkbulkvoucherposting.Checked = false;
            }
        }

        private void chkjournal_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panelfooter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chkpurchaseinvoice_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkproduct_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panelhead_Paint(object sender, PaintEventArgs e)
        {

        } 
    }
}
