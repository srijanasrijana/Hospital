using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusinessLogic;
using DateManager;
using Common;


namespace AccSwift
{
    public partial class frmReminderList : Form, IfrmDateConverter, IVoucherRecurring
    {
        DataTable dt = null;
        private IMDIMainForm m_MDIForm;
        private int RowID;
        private string voucherType;
        private bool isDateLoaded = false;

        SourceGrid.Cells.Controllers.CustomEvents evtDblClick = new SourceGrid.Cells.Controllers.CustomEvents();

        public frmReminderList()
        {
            InitializeComponent();
        }
        public frmReminderList(IMDIMainForm frm)
        {
            m_MDIForm = frm;
            InitializeComponent();
        }

        /// <summary>
        /// Here is the main bunch of code to load the reminder depending upon time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //check the reminder exist or not
        public void frmReminderList_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtSelectedReminder = Reminder.GetReminderIfExistToday(User.CurrUserID, 1);
                //Add items in the listview
                txtReminderID.Text = string.Empty;
                string[] arr = new string[7];
                ListViewItem itm;
                lvwReminder.Items.Clear();
                int i = 1;
                foreach (DataRow dr in dtSelectedReminder.Rows)
                {
                    //Add item              
                    arr[0] = i.ToString();
                    arr[1] = dr["Subject"].ToString();
                    int statusValue = Convert.ToInt32(dr["Status"]);
                    switch (statusValue)
                    {
                        case 0: arr[2] = "Inactive";
                            break;
                        case 1: arr[2] = "Active";
                            break;
                    }
                    int priorityValue = Convert.ToInt32(dr["Priority"]);
                    switch (priorityValue)
                    {
                        case 0: arr[3] = "Low";
                            break;
                        case 1: arr[3] = "Normal";
                            break;
                        case 2: arr[3] = "High";
                            break;
                    }
                    arr[4] = Date.ToSystem(Convert.ToDateTime(dr["Date"]));
                    arr[5] = dr["Description"].ToString();
                    arr[6] = dr["ReminderID"].ToString();
                    itm = new ListViewItem(arr);
                    lvwReminder.Items.Add(itm);
                    if (lvwReminder.Items[i - 1].SubItems[3].Text == "High")
                        lvwReminder.Items[i - 1].BackColor = Color.OrangeRed;
                    if (lvwReminder.Items[i - 1].SubItems[3].Text == "Normal")
                        lvwReminder.Items[i - 1].BackColor = Color.Orange;
                    i++;
                }


                //Select at least one items
                if (lvwReminder.Items.Count > 0)
                {
                    lvwReminder.Items[0].Selected = true;
                }

                //dt = RecurringVoucher.GetNotPostedRecurring(DateTime.Today);
                //LoadVoucherReminder(dt);

                if (txtDate.Text == "")
                    txtDate.Text = Date.ToSystem(Convert.ToDateTime(Date.GetServerDate())).ToString();

                evtDblClick.DoubleClick += new EventHandler(Row_Double_Click);

                txtDate.Mask = Date.FormatToMask();
                isDateLoaded = true;
                txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());

                if (grdVoucher.Rows.Count > lvwReminder.Items.Count)
                {
                    tabControl1.SelectedTab = tpVoucher;
                }
                else
                    tabControl1.SelectedTab = tpGeneral;
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }
        public void Row_Double_Click(object sender, EventArgs e)
        {
            try
            {
                // check if the user has permission to view the voucher
                //bool chkUserPermission = UserPermission.ChkUserPermission("SALE_INVOICE_VIEW");
                //if (chkUserPermission == false)
                //{
                //    Global.MsgError("Sorry! you dont have permission to View Sales Invoice. Please contact your administrator for permission.");
                //    return;
                //}
                int CurRow = grdVoucher.Selection.GetSelectionRegion().GetRowsIndex()[0];

                SourceGrid.CellContext cellRVPID = new SourceGrid.CellContext(grdVoucher, new SourceGrid.Position(CurRow, 0));
                int RVPID = Convert.ToInt32(cellRVPID.Value.ToString());

                SourceGrid.CellContext cellVouTypeTran = new SourceGrid.CellContext(grdVoucher, new SourceGrid.Position(CurRow, 3));
                voucherType = cellVouTypeTran.Value.ToString();

                SourceGrid.CellContext cellType = new SourceGrid.CellContext(grdVoucher, new SourceGrid.Position(CurRow, 1));
                RowID = Convert.ToInt32(cellType.Value);
                switch (voucherType)
                {
                    case "SALES_INVOICE":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionSales = UserPermission.ChkUserPermission("SALE_INVOICE_VIEW");
                        if (chkUserPermissionSales == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Sales Invoice. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param = new object[2];
                        param[0] = (RowID);
                        param[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmSalesInvoiceRecurring", param);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;

                    case "PURCHASE_INVOICE":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionPurchase = UserPermission.ChkUserPermission("PURCHASE_INVOICE_VIEW");
                        if (chkUserPermissionPurchase == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Purchase Invoice. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param1 = new object[2];
                        param1[0] = (RowID);
                        param1[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmPurchaseInvoiceRecurring", param1);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                    case "PURCHASE_RETURN":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionPurchaseR = UserPermission.ChkUserPermission("PURCHASE_RETURN_VIEW");
                        if (chkUserPermissionPurchaseR == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Purchase Return. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param2 = new object[2];
                        param2[0] = (RowID);
                        param2[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmPurchaseReturnRecurring", param2);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;

                    case "SALES_RETURN":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionSalesR = UserPermission.ChkUserPermission("SALE_RETURN_VIEW");
                        if (chkUserPermissionSalesR == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Sales Return. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param3 = new object[2];
                        param3[0] = (RowID);
                        param3[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmSalesReturnRecurring", param3);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                    case "SALES_ORDER":
                        //check if the user has permission to view the voucher
                        bool chkUserPermissionSalesO = UserPermission.ChkUserPermission("SALE_ORDER_VIEW");
                        if (chkUserPermissionSalesO == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Sales Order. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param4 = new object[2];
                        param4[0] = (RowID);
                        param4[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmSalesOrderRecurring", param4);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                    case "PURCHASE_ORDER":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionPurchaseO = UserPermission.ChkUserPermission("PURCHASE_ORDER_VIEW");
                        if (chkUserPermissionPurchaseO == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Purchase Order. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param5 = new object[2];
                        param5[0] = (RowID);
                        param5[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmPurchaseOrderRecurring", param5);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                    case "JOURNAL":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionJournal = UserPermission.ChkUserPermission("JOURNAL_VIEW");
                        if (chkUserPermissionJournal == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Journal. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param6 = new object[2];
                        param6[0] = (RowID);
                        param6[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmJournalRecurring", param6);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                    case "BANK_RECEIPT":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionBankR = UserPermission.ChkUserPermission("BANK_RECEIPT_VIEW");
                        if (chkUserPermissionBankR == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Bank Receipt. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param7 = new object[2];
                        param7[0] = (RowID);
                        param7[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmBankReceiptRecurring", param7);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                    case "BANK_PAYMENT":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionBankP = UserPermission.ChkUserPermission("BANK_PAYMENT_VIEW");
                        if (chkUserPermissionBankP == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Bank Payment. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param8 = new object[2];
                        param8[0] = (RowID);
                        param8[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmBankPaymentRecurring", param8);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                    case "CASH_RECEIPT":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionCashR = UserPermission.ChkUserPermission("CASH_RECEIPT_VIEW");
                        if (chkUserPermissionCashR == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Cash Receipt. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param9 = new object[2];
                        param9[0] = (RowID);
                        param9[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmCashReceiptRecurring", param9);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                    case "CASH_PAYMENT":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionCashP = UserPermission.ChkUserPermission("CASH_PAYMENT_VIEW");
                        if (chkUserPermissionCashP == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Cash Payment. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param10 = new object[2];
                        param10[0] = (RowID);
                        param10[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmCashPaymentRecurring", param10);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                    case "CONTRA":
                        // check if the user has permission to view the voucher
                        bool chkUserPermissionContra = UserPermission.ChkUserPermission("CONTRA_VIEW");
                        if (chkUserPermissionContra == false)
                        {
                            Global.MsgError("Sorry! you dont have permission to View Contra Voucher. Please contact your administrator for permission.");
                            return;
                        }
                        object[] param11 = new object[2];
                        param11[0] = (RowID);
                        param11[1] = (RVPID);
                        m_MDIForm.OpenFormArrayParam("frmContraRecurring", param11);

                        // reload the reminder list after voucher posting 
                        txtDate_TextChanged(null, null);
                        break;
                }

            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
            //frmSalesInvoice fsi = new frmSalesInvoice(Convert.ToInt32(grdVoucher[CurRow,1].Value));
            //fsi.ShowDialog();
        }
        private void LoadVoucherReminder(DataTable dtv)
        {
            try
            {
                int rowsCount = 0;
                //DataTable dtv = RecurringVoucher.GetNotPostedRecurring();
                rowsCount = dtv.Rows.Count;
                grdVoucher.Rows.Clear();

                grdVoucher.Redim(rowsCount + 1, 9);

                WriteHeaderRecurring();
                for (int i = 1; i <= rowsCount; i++)
                {
                    SourceGrid.Cells.Views.Cell alternate = new SourceGrid.Cells.Views.Cell();

                    if (i % 2 != 0)
                    {
                        alternate.Background = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.FromArgb(225, 255, 255));
                    }

                    DataRow dr = dtv.Rows[i - 1];
                    grdVoucher[i, 0] = new SourceGrid.Cells.Cell(dr["RVPID"].ToString());
                    grdVoucher[i, 0].View = alternate;

                    grdVoucher[i, 1] = new SourceGrid.Cells.Cell(dr["VoucherID"].ToString());
                    grdVoucher[i, 0].View = alternate;

                    grdVoucher[i, 2] = new SourceGrid.Cells.Cell(i.ToString());
                    grdVoucher[i, 2].View = alternate;
                    grdVoucher[i, 2].AddController(evtDblClick);

                    grdVoucher[i, 3] = new SourceGrid.Cells.Cell(dr["VoucherType"].ToString());
                    grdVoucher[i, 3].View = alternate;
                    grdVoucher[i, 3].AddController(evtDblClick);

                    grdVoucher[i, 4] = new SourceGrid.Cells.Cell(dr["RecurringType"].ToString());
                    grdVoucher[i, 4].View = alternate;
                    grdVoucher[i, 4].AddController(evtDblClick);

                    grdVoucher[i, 5] = new SourceGrid.Cells.Cell(dr["Description"].ToString());
                    grdVoucher[i, 5].View = alternate;
                    grdVoucher[i, 5].AddController(evtDblClick);

                    grdVoucher[i, 6] = new SourceGrid.Cells.Cell(Convert.ToDateTime(dr["Date"]).ToString("yyyy/MM/dd"));
                    grdVoucher[i, 6].View = alternate;
                    grdVoucher[i, 6].AddController(evtDblClick);

                    grdVoucher[i, 7] = new SourceGrid.Cells.Cell(dr["isPosted"].ToString());
                    grdVoucher[i, 7].View = alternate;

                    grdVoucher[i, 8] = new SourceGrid.Cells.Cell(dr["RVID"].ToString());//tblRecurringVoucher id
                    grdVoucher[i, 8].View = alternate;

                }
                if (rowsCount > 0)
                    tpVoucher.Text = "Voucher(" + rowsCount + ")";
                else
                    tpVoucher.Text = "Voucher";
            }
            catch (Exception ex)
            {
                Global.MsgError(ex.Message);
            }
        }

        private void WriteHeaderRecurring()
        {
            grdVoucher[0, 0] = new SourceGrid.Cells.ColumnHeader("RVPID");
            grdVoucher[0, 0].Column.Visible = false;
            grdVoucher[0, 1] = new SourceGrid.Cells.ColumnHeader("VoucherID");
            grdVoucher[0, 1].Column.Visible = false;
            grdVoucher[0, 2] = new SourceGrid.Cells.ColumnHeader("SN");
            grdVoucher[0, 2].Column.Width = 40;
            grdVoucher[0, 3] = new SourceGrid.Cells.ColumnHeader("Voucher Type");
            grdVoucher[0, 3].Column.Width = 150;
            grdVoucher[0, 4] = new SourceGrid.Cells.ColumnHeader("Recurring Type");
            grdVoucher[0, 4].Column.Width = 150;
            grdVoucher[0, 5] = new SourceGrid.Cells.ColumnHeader("Description");
            grdVoucher[0, 5].Column.Width = 325;
            grdVoucher[0, 6] = new SourceGrid.Cells.ColumnHeader("Date");
            grdVoucher[0, 6].Column.Width = 73;
            grdVoucher[0, 7] = new SourceGrid.Cells.ColumnHeader("isPosted");
            grdVoucher[0, 7].Column.Visible = false;
            grdVoucher[0, 8] = new SourceGrid.Cells.ColumnHeader("RVID"); //tblRecurringVoucher id
            grdVoucher[0, 8].Column.Visible = false;
        }

        private void lvwReminder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwReminder.SelectedItems.Count > 0)
            {
                txtReminderID.Text = lvwReminder.SelectedItems[0].SubItems[6].Text;
            }
            else
            {
                txtReminderID.Text = string.Empty;
            }
        }



        private void btnSnooze_Click(object sender, EventArgs e)
        {
            frmSnoozeReminder frm = new frmSnoozeReminder(txtReminderID.Text);
            frm.StartPosition = FormStartPosition.CenterScreen;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //Reload reminder list
                frmReminderList_Load(sender, e);
            }

        }

        private void btnDismiss_Click(object sender, EventArgs e)
        {
            if (txtReminderID.Text == "")
            {
                MessageBox.Show("Select an item first!!");
                return;
            }
            int ReminderID = Convert.ToInt32(lvwReminder.SelectedItems[0].SubItems[6].Text);
            string Subject = lvwReminder.SelectedItems[0].SubItems[1].Text;
            // int Status = 0;
            int Priority = 0;

            string priorityValue = lvwReminder.SelectedItems[0].SubItems[3].Text.Trim();
            switch (priorityValue)
            {
                case "Low": Priority = 0;
                    break;
                case "Normal": Priority = 1;
                    break;
                case "High": Priority = 2;
                    break;
            }
            DateTime startDate = Date.ToDotNet(lvwReminder.SelectedItems[0].SubItems[4].Text);
            string Description = lvwReminder.SelectedItems[0].SubItems[5].Text;
            DataTable dt = new DataTable();
            string strQuery = "Select * FROM System.tblRecurrence  where ReminderID=" + ReminderID;
            dt = Global.m_db.SelectQry(strQuery, "tblRecurrence");
            if (dt.Rows.Count != 0)
            {
                int occurencePattern = Convert.ToInt32(dt.Rows[0]["OccurencePattern"]);
                switch (occurencePattern)
                {
                    case 0:
                        Reminder.ModifyReminder(ReminderID, Subject, 1, Priority, Convert.ToDateTime(startDate).AddDays(1), Description);
                        break;

                    case 1:
                        Reminder.ModifyReminder(ReminderID, Subject, 1, Priority, Convert.ToDateTime(startDate).AddDays(7), Description);
                        break;
                    case 2:
                        Reminder.ModifyReminder(ReminderID, Subject, 1, Priority, Convert.ToDateTime(startDate).AddMonths(1), Description);
                        break;

                    case 3:
                        Reminder.ModifyReminder(ReminderID, Subject, 1, Priority, Convert.ToDateTime(startDate).AddYears(1), Description);
                        break;

                }

            }

            else
                Reminder.ModifyReminder(ReminderID, Subject, 0, Priority, startDate, Description);
            frmReminderList_Load(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmReminderList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            try
            {
                int CurRow = grdVoucher.Selection.GetSelectionRegion().GetRowsIndex()[0];

                frmVoucherRecurring frmv = new frmVoucherRecurring(this, Convert.ToInt32(grdVoucher[CurRow, 8].Value));
                frmv.ShowDialog();
            }
            catch (Exception)
            {

                Global.MsgError("You must select a row to proceed.");
            }
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            try
            {
                Common.frmDateConverter frm = new Common.frmDateConverter(this, Date.ToDotNet(txtDate.Text));
                frm.ShowDialog();
            }
            catch (Exception)
            {
                Global.MsgError("Date is not in correct format.");
                txtDate.Text = Date.DBToSystem(Date.GetServerDate().ToString());
            }
        }

        DataTable m_dtRecurringSetting = new DataTable();
        public void GetRecurringSetting(DataTable dat)
        {
            this.m_dtRecurringSetting = dat;
            RecurringVoucher.ModifyRecurringVoucherSetting(dat);

            //DataTable dt = RecurringVoucher.GetNotPostedRecurring();
            txtDate_TextChanged(null, null); // after saving recurring settings load data from date specified in txtDate
        }

        //int day = 0, month = 0, year = 0;
        //DateTime date;
        private void txtDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtDate.MaskCompleted && isDateLoaded && txtDate.MaskFull)
                {
                    // select all voucher recurring from the given date
                    //day = (int)Convert.ToDateTime(txtDate.Text).Day;
                    //month = (int)Convert.ToDateTime(txtDate.Text).Month;
                    //year = (int)Convert.ToDateTime(txtDate.Text).Year;

                    //date = Date.NepToEng(year, month, day);

                    DataTable dt = RecurringVoucher.GetNotPostedRecurring(Date.ToDotNet(txtDate.Text));
                    LoadVoucherReminder(dt);
                }
            }
            catch (Exception ex)
            {
                Global.MsgError("Date format is not comlete or is not valid. OR "+ex.Message);
            }
        }



        public void DateConvert(DateTime ReturnDotNetDate)
        {
            txtDate.Text = Date.ToSystem(ReturnDotNetDate);
        }
    }
}
