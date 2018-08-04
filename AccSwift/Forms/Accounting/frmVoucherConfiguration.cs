using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using BusinessLogic;
using Inventory.Forms.Accounting;
using Common;
using Accounts;


namespace Inventory
{
    public interface IfrmVouchrConfiguration
    {
        void AddVoucherConfig();
    }
    public partial class frmVoucherConfiguration : Form,IfrmIsSalesConfigAdded
    {
        private bool IsVouConfigInfo = false;
        private EntryMode m_mode = EntryMode.NORMAL; //Stores the current mode or state of which button is clicked
        public frmVoucherConfiguration()
        {
            InitializeComponent();
        }

        private void frmVoucherConfiguration_Load(object sender, EventArgs e)
        {
            //Fill the Voucher Configuration head name in the treeview
            ShowVoucherConfigurationHeadInTreeView(tvVoucherConfiguration, null, 0);
        }

       //Recursive Function to Show Account Group in Treeview
        public void ShowVoucherConfigurationHeadInTreeView(TreeView tv, TreeNode n, int Group_ID)
        {

            //BLOCK FOR SALES
            TreeNode tSales = new TreeNode("Sales", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tSales);
            tSales.Tag = "SALES";
            WriteOnTreeView("SALES",tSales);

            //BLOCK FOR PURCHASE
            TreeNode tPurchase = new TreeNode("Purchase", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tPurchase);
            tPurchase.Tag = "PURCH";
            WriteOnTreeView("PURCH", tPurchase);

            //BLOCK FOR SALES_RETURN
            TreeNode tSalesReturn = new TreeNode("Sales Return", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tSalesReturn);
            tSalesReturn.Tag = "SLS_RTN";
            WriteOnTreeView("SLS_RTN", tSalesReturn);

            //BLOCK FOR PURCHASE RETURN
            TreeNode tPurchaseReturn = new TreeNode("Purchase Return", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tPurchaseReturn);
            tPurchaseReturn.Tag = "PURCH_RTN";
            WriteOnTreeView("PURCH_RTN", tPurchaseReturn);

            //BLOCK FOR CASH PAYMENT
            TreeNode tCash_Payment = new TreeNode("Cash Payment", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tCash_Payment);
            tCash_Payment.Tag = "CASH_PMNT";
            WriteOnTreeView("CASH_PMNT", tCash_Payment);

            //BLOCK FOR BANK PAYMENT
            TreeNode tBank_Payment = new TreeNode("Bank Payment", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tBank_Payment);
            tBank_Payment.Tag = "BANK_PMNT";
            WriteOnTreeView("BANK_PMNT", tBank_Payment);

            //BLOCK FOR CASH RECEIPT
            TreeNode tCash_Receipt = new TreeNode("Cash Receipt", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tCash_Receipt);
            tCash_Receipt.Tag = "CASH_RCPT";
            WriteOnTreeView("CASH_RCPT", tCash_Receipt);

            //BLOCK FOR BANK RECEIPT
            TreeNode tBank_Receipt = new TreeNode("Bank Receipt", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tBank_Receipt);
            tBank_Receipt.Tag = "BANK_RCPT";
            WriteOnTreeView("BANK_RCPT", tBank_Receipt);


            //BLOCK FOR JOURNAL
            TreeNode tJournal = new TreeNode("Journal", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tJournal);
            tJournal.Tag = "JNL";
            WriteOnTreeView("JNL", tJournal);

            //BLOCK FOR CONTRA
            TreeNode tContra = new TreeNode("Contra", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tContra);
            tContra.Tag = "CNTR";
            WriteOnTreeView("CNTR", tContra);

            //BLOCK FOR DEBIT NOTE
            TreeNode tDebitNote= new TreeNode("Debit Note", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tDebitNote);
            tDebitNote.Tag = "DR_NOT";
            WriteOnTreeView("DR_NOT", tDebitNote);

            //BLOCK FOR CREDIT NOTE
            TreeNode tCreditNote = new TreeNode("Credit Note", 0, 0);        
            tvVoucherConfiguration.Nodes.Add(tCreditNote);
            tCreditNote.Tag = "CR_NOT";
            WriteOnTreeView("CR_NOT", tCreditNote);

            //Block for bankreconciliation
            TreeNode tbankreconciliation = new TreeNode("Bank Reconciliation", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tbankreconciliation);
            tbankreconciliation.Tag = "BRECON";
            WriteOnTreeView("BRECON", tbankreconciliation);

            //Block for StockTransfer
            TreeNode tStockTransfer = new TreeNode("Stock Transfer", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tStockTransfer);
            tStockTransfer.Tag = "STOCK_TRANS";
            WriteOnTreeView("STOCK_TRANS", tStockTransfer);

            //Block for ItemsDamage
            TreeNode tItemsDamage = new TreeNode("Items Damage", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tItemsDamage);
            tItemsDamage.Tag = "DAMAGE";
            WriteOnTreeView("DAMAGE", tItemsDamage);

            //Block For Cheque Receipt
            TreeNode tChequeReceipt = new TreeNode("Cheque Receipt", 0, 0);
            tvVoucherConfiguration.Nodes.Add(tChequeReceipt);
            tChequeReceipt.Tag = "CHEQUERCPT";
            WriteOnTreeView("CHEQUERCPT", tChequeReceipt);
        }

        //A function from the Interface IfrmVoucherFormat. Used to apply the Datatable to this form from VoucherFormat
        public void IsVoucherConfigAdded(bool IsVoucherConfigAdded)
        {
            try
            {
                IsVouConfigInfo = IsVoucherConfigAdded;
                if (IsVouConfigInfo == true)
                {
                    //For refresh......
                    tvVoucherConfiguration.Nodes.Clear();
                    ShowVoucherConfigurationHeadInTreeView(tvVoucherConfiguration, null, 0);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void WriteOnTreeView(string VoucherType,TreeNode t)
        {
            t.ForeColor = Color.Black ;
            DataTable dt = VoucherConfiguration.GetSeriesInfo(VoucherType);
            for (int i = 1; i <=dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i - 1];
                TreeNode n = new TreeNode(dr["EngName"].ToString(), 0, 0);
                n.Tag = dr["SeriesID"];            
                n.ForeColor = Color.Blue;
                t.Nodes.Add(n);
            
            }
        }

        private void tvVoucherConfiguration_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                //Show the selected value in respective fields
                if (e.Node.Level >0)//Series Name is being Selected
                {
                    if (e.Node.Text.ToUpper() == "MAIN")
                    {
                        btnNew.Enabled = false;
                        btnCancel.Enabled = true;
                        btnEdit.Enabled = true;
                        btnDelete.Enabled = false;

                    }
                    else
                    {
                        btnNew.Enabled = false;
                        btnCancel.Enabled = true;
                        btnEdit.Enabled = true;
                        btnDelete.Enabled = true;
                    }
                }
                else
                {
                    btnNew.Enabled = true;
                    btnCancel.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);

            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONFIGURATION_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Create. Please contact your administrator for permission.");
                return;
            }
            
            switch (tvVoucherConfiguration.SelectedNode.Tag.ToString())
            {
                case "SALES":
                    frmSalesVoucherConfig frm1 = new frmSalesVoucherConfig(this);
                    frm1.Show();
                    break;
                case "PURCH":
                    frmBconVoucherConfiguration frm2 = new frmBconVoucherConfiguration(this);
                    frm2.Show();
                    break;
                case "SLS_RTN":
                    Global.salesreturn = "New";
                    frmSalesReturnVoucherConfig frm3 = new frmSalesReturnVoucherConfig(this);
                    frm3.Show();
                    break;
                case "PURCH_RTN":
                    frmPurchaseReturnVoucherConfig frm4 = new frmPurchaseReturnVoucherConfig(this);
                    frm4.Show();
                    break;
                case "PMNT":
                    break;
                case "BANK_PMNT":
                    frmBankPaymentVoucherConfig frm5 = new frmBankPaymentVoucherConfig(this);
                    frm5.Show();
                    break;
                case "BANK_RCPT":
                    frmBankReceiptVoucherConfig frmBankRcpt = new frmBankReceiptVoucherConfig(this);
                    frmBankRcpt.Show();
                    break;
                case "CASH_PMNT":
                    frmCashPaymentVoucherConfig frm = new frmCashPaymentVoucherConfig(this);
                    frm.Show();
                    break;
                case "CASH_RCPT":
                    frmCashReceiptVoucherConfig frm6 = new frmCashReceiptVoucherConfig(this);
                    frm6.Show();
                    break;
                case "JNL":
                    frmJournalVoucherConfig frm7 = new frmJournalVoucherConfig(this);
                    frm7.Show();
                    break;
                case "CNTR":
                    frmContraVoucherConfig frm8 = new frmContraVoucherConfig(this);
                    frm8.Show();
                    break;
                case "DR_NOT":
                    frmDebitNoteVoucherConfig frm9 = new frmDebitNoteVoucherConfig(this);
                    frm9.Show();
                    break;
                case "CR_NOT":
                    frmCreditNoteVoucherConfig frm10 = new frmCreditNoteVoucherConfig(this);
                    frm10.Show();
                    break;
                case "BRECON":
                    Accounts.frmBreconVoucherConfiguration frm11 = new Accounts.frmBreconVoucherConfiguration(this);
                    frm11.Show();
                    break;
                case "STOCK_TRANS":
                   frmStockTransferVoucherConfig  frm12 = new frmStockTransferVoucherConfig(this);
                    frm12.Show();
                    break;
                case "DAMAGE":
                    frmDamageItemsVoucherConfig frm13 = new frmDamageItemsVoucherConfig(this);
                    frm13.Show();
                    break;
                case "CHEQUERCPT":
                    Inventory.Forms.Accounting.frmChequeReceiptVoucherConfiguration frm14 = new Forms.Accounting.frmChequeReceiptVoucherConfiguration(this);
                    frm14.Show();
                    break;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONFIGURATION_CREATE_MODIFY");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Modify. Please contact your administrator for permission.");
                return;
            }
            DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(tvVoucherConfiguration.SelectedNode.Tag));
            DataRow dr = dt.Rows[0];


            switch (dr["Voucher_Type"].ToString())
            {
                case "SALES":
                    frmSalesVoucherConfig frm1 = new frmSalesVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    frm1.Show();
                    break;
                case "PURCH":
                    frmBconVoucherConfiguration frm2 = new frmBconVoucherConfiguration(Convert.ToInt32(dr["SeriesID"]),this);
                    frm2.Show();
                    break;
                case "SLS_RTN":
                    frmSalesReturnVoucherConfig frm3 = new frmSalesReturnVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    Global.salesreturn="Edit";
                   //frmSalesReturnVoucherConfig frm3 = new frmSalesReturnVoucherConfig();
                    frm3.Show();
                    break;
                case "PURCH_RTN":
                    frmPurchaseReturnVoucherConfig frm4 = new frmPurchaseReturnVoucherConfig(Convert.ToInt32(dr["SeriesID"]), this);
                    frm4.Show();
                    break;
                case "CASH_PMNT":
                    frmCashPaymentVoucherConfig frm5 = new frmCashPaymentVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    frm5.Show();
                    break;
                case "CASH_RCPT":
                    frmCashReceiptVoucherConfig frm6 = new frmCashReceiptVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    frm6.Show();
                    break;
                case "BANK_PMNT":
                    frmBankPaymentVoucherConfig frmBankPmnt = new frmBankPaymentVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    frmBankPmnt.Show();
                    break;
                case "BANK_RCPT":
                    frmBankReceiptVoucherConfig frmBankRcpt = new frmBankReceiptVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    frmBankRcpt.Show();
                    break;
                case "JNL":
                    frmJournalVoucherConfig frm7 = new frmJournalVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    frm7.Show();
                    break;
                case "CNTR":
                    frmContraVoucherConfig frm8 = new frmContraVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    frm8.Show();
                    break;
                case "DR_NOT":
                    frmDebitNoteVoucherConfig frm9 = new frmDebitNoteVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    frm9.Show();
                    break;
                case "CR_NOT":
                    frmCreditNoteVoucherConfig frm10 = new frmCreditNoteVoucherConfig(Convert.ToInt32(dr["SeriesID"]),this);
                    frm10.Show();
                    break;
                case "BRECON":
                    Inventory.Forms.Accounting.frmBreconVoucherConfiguration frm11 = new Forms.Accounting.frmBreconVoucherConfiguration(Convert.ToInt32(dr["SeriesID"]), this);
                    frm11.Show();
                    break;

                case "STOCK_TRANS":
                    frmStockTransferVoucherConfig frm12 = new frmStockTransferVoucherConfig(Convert.ToInt32(dr["SeriesID"]), this);
                    frm12.Show();
                    break;
                case "DAMAGE":
                    frmDamageItemsVoucherConfig frm13 = new frmDamageItemsVoucherConfig(Convert.ToInt32(dr["SeriesID"]), this);
                    frm13.Show();
                    break;
                case "CHEQUERCPT":
                    Inventory.Forms.Accounting.frmChequeReceiptVoucherConfiguration frm14 = new Forms.Accounting.frmChequeReceiptVoucherConfiguration(Convert.ToInt32(dr["SeriesID"]), this);
                    frm14.Show();
                    break;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool chkUserPermission = UserPermission.ChkUserPermission("CONFIGURATION_DELETE");
            if (chkUserPermission == false)
            {
                Global.MsgError("Sorry! you dont have permission to Delete. Please contact your administrator for permission.");
                return;
            }
            //DELETING THE SELECTED SERIES NAME
            DataTable dt = VoucherConfiguration.GetSeriesInfo(Convert.ToInt32(tvVoucherConfiguration.SelectedNode.Tag));
            DataRow dr = dt.Rows[0];
            if (Global.MsgQuest("Are you sure you want to delete the Series -" + dr["EngName"].ToString() + "?") == DialogResult.No)
                return;
            VoucherConfiguration.Delete(Convert.ToInt32(tvVoucherConfiguration.SelectedNode.Tag));
            Global.Msg("Series - " + dr["EngName"].ToString() + " deleted successfully!");

            //For refresh......
            tvVoucherConfiguration.Nodes.Clear();
            ShowVoucherConfigurationHeadInTreeView(tvVoucherConfiguration, null, 0);
        }

        private void frmVoucherConfiguration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        private void ButtonState(bool Save, bool Cancel)
        {
           // btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
        }

        private void ChangeState(EntryMode Mode)
        {
            m_mode = Mode;
            switch (m_mode)
            {
                case EntryMode.NORMAL:

                    ButtonState(true, false);
                    break;
                case EntryMode.NEW:
                    ButtonState(true, true);
                    break;

                case EntryMode.EDIT:

                    ButtonState(true, true);
                    break;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ChangeState(EntryMode.NORMAL);
        }
    }
}
